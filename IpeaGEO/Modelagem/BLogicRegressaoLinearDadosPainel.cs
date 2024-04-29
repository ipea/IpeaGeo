using System;
using System.Collections;
using System.Data;
using System.Text;

using IpeaGeo.RegressoesEspaciais;
using MathNet.Numerics.Distributions;

namespace IpeaGeo.Modelagem
{
    #region enumerações

    public enum TipoEstimacaoDadosPainel : int
    {
        PooledOLS,
        EfeitosFixos,
        EfeitosAleatorios,
        PrimeirasDiferencas
    };

    public enum TipoPainel : int
    {
        Balanceado,
        NaoBalanceado
    };

    #endregion

    public class BLogicRegressaoLinearDadosPainel : BLogicBaseModelagem
    {
        #region Estruturas das funções de estimação.
        private struct FirstDifference
        {
            public double[,] beta_FD, beta_FD_covmat, delta_y_hat, residuos_delta,
                delta_y, delta_x, delta_unidades, delta_periodos, c_hat;

            public double sigma2_hat, R2_modelo_original, R2, R2_ajustado,
                Fstat, Fpvalue, X2stat, X2pvalue, loglik, AIC, BIC,
                variancia_efeitos_fixos, sigma2_hat_modelo_original,
                SSE, SST, SSR, Wtest, Wpvalor;
        }

        private struct FixedEffectsBallancedPanel
        {
            public double[,] beta_FE, beta_FE_covmat, c_hat, y_hat_FE, resduos_FE;
            public double sigma2_hat, R2, R2_ajustado, Fstat, Fpvalue, SSE, SST, SSR,
                R2_modelo_original, R2_ajust_modelo_original, SSE_modelo_original,
                SST_modelo_original, SSR_modelo_original, loglik_modelo_original,
                AIC_modelo_original, BIC_modelo_original, Fstat_modelo_original,
                Fpvalue_modelo_original, sigma2_hat_modelo_original, variancia_efeitos_fixos,
                Wtest, Wpvalor;
        }

        private struct PooledOLS
        {
            public double[,] beta_POLS, beta_POLS_cov_mat, y_hat_POLS, residuos_POLS;

            public double sigma2_hat, R2, R2_ajustado, Fstat, Fpvalue, loglik, AIC,
                BIC, SST, SSR, SSE, sigma2_c_test_stat, sigma2_c_pvalue, Wtest, Wpvalor;
        }

        private struct RandomEffects
        {
            public double[,] beta_RE, beta_RE_covmat, residuos_RE, y_hat_RE;

            public double sigma2_c, sigma2_u, sigma2_v, sigma2_c_test_stat,
                sigma2_c_pvalue, R2, R2_ajustado, Fstat, Fpvalue, SST, SSR,
                SSE, loglik, AIC, BIC, Wtest, Wpvalor;
        }

        #endregion

        //Observações. Ainda falta:
        // (1) matriz de cov robusta para FE estimator com FGLS - solução temporária => não é possível cov robusta com FGLS - OK!
        // (2) quando estimador de primeiras diferenças, não incluir tendências temporais - OK!
        // (3) matriz de cov robusta para primeiras diferenças estimator com FGLS - solução temporária => não é possível cov robusta com FGLS - OK!
        // (4) teste F e p-valor para FE estimator com FGLS - solução temporária => vai valer apenas para estimador OLS - OK!
        // (5) variáveis de output para estimador de primeiras diferenças - OK!
        // (6) para o estimador de FE, incluir as dummies temporais e tendências após a extração da média dos regressores - não precisa - OK!
        // (7) para o estimador de diferenças, incluir as dummies temporais e tendências após as primeiras diferenças - não precisa - OK!
        // (8) incluir a estatística teste (F) para as dummies temporais => solução temporária => utilizamos o teste de Wald, que aceita situações robustas e FGLS - OK!
        // (9) corrigir as variáveis geradas na tabela de dados para o estimador de efeitos aleatórios - OK!
        // (10) incluir os efeitos fixos estimados para o estimador de primeiras diferenças - OK!
        // (11) incluir os efeitos fixos estimados na tabela de dados para os estimadores de FE e FD - OK!
        // (12) incluir estatísticas de output para o estimador de efeitos aleatórios
        // (13) incluir as variâncias dos efeitos fixos e dos resíduos (puros) para o FE - OK!
        // (14) incluir as variâncias dos efeitos fixos e dos resíduos (puros) para o FD - OK!
        // (15) incluir o teste de Hausmman
        // (16) incluir o teste de não-autocorrelação nos resíduos para o pooled OLS - OK!
        // (17) incluir estimação via FGLS geral no OLS
        // (18) teste F e p-valor para RE estimator geral - solução temporária => trata-se da estatística F considerando-se OLS - OK!

        public BLogicRegressaoLinearDadosPainel()
            : base()
        {
        }

        #region teste para a signifância das dummies e tendências temporais 

        protected void TestaDummiesETendenciasTemporais(double[,] beta_hat, double[,] cov_beta_hat, out double Wtest, out double Wpvalor)
        {
            if (m_numero_tendencias_temporais + m_numero_dummies_temporais > 0)
            {
                int mvariaveis = m_numero_dummies_temporais + m_numero_tendencias_temporais;

                double[,] R = new double[mvariaveis, beta_hat.GetLength(0)];
                for (int i = 0; i < mvariaveis; i++)
                {
                    R[i, R.GetLength(1) - mvariaveis + i] = 1.0;
                }

                double[,] cov = m_clt.MatrizMult(R, m_clt.MatrizMult(cov_beta_hat, m_clt.MatrizTransp(R)));
                double[,] covinv = m_clt.MatrizInversa(cov);
                
                double[,] Rb = m_clt.MatrizMult(R, beta_hat);

                Wtest = (m_clt.MatrizMult(m_clt.MatrizTransp(Rb), m_clt.MatrizMult(covinv, Rb)))[0, 0];

                ChiSquared mchi = new ChiSquared((double)mvariaveis);
                Wpvalor = 1.0 - mchi.CumulativeDistribution(Wtest);
            }
            else
            {
                Wtest = 0.0;
                Wpvalor = 0.0;
            }
        }

        #endregion 

        #region variáveis internas

        protected bool m_usa_tendencia_temporal_linear;
        protected bool m_usa_tendencia_temporal_quadratica;
        protected bool m_usa_tendencia_temporal_cubica;
        protected bool m_usa_dummies_temporais;

        public bool UsaDummiesTemporais
        {
            get { return m_usa_dummies_temporais; }
            set { m_usa_dummies_temporais = value; }
        }

        public bool UsaTendenciaTemporalCubica
        {
            get { return m_usa_tendencia_temporal_cubica; }
            set { m_usa_tendencia_temporal_cubica = value; }
        }

        public bool UsaTendenciaTemporalQuadratica
        {
            get { return this.m_usa_tendencia_temporal_quadratica; }
            set {this.m_usa_tendencia_temporal_quadratica = value;}
        }

        public bool UsaTendenciaTemporalLinear
        {
            get { return m_usa_tendencia_temporal_linear; }
            set {m_usa_tendencia_temporal_linear = value;}
        }

        protected bool m_usa_general_FGLS_analysis = false;
        public bool UsaGeneralFGLS
        {
            get
            {
                return m_usa_general_FGLS_analysis;
            }
            set
            {
            	m_usa_general_FGLS_analysis = value;
            }
        }

        protected bool m_usa_robust_cov_matrix = false;
        public bool UsaRobustCovMatrix
        {
            get
            {
                return m_usa_robust_cov_matrix;
            }
            set
            {
            	m_usa_robust_cov_matrix = value;
            }
        }

        protected TipoPainel m_tipo_painel = TipoPainel.Balanceado;
        public TipoPainel TipoPainelDados
        {
            get
            {
                return m_tipo_painel;
            }
            set
            {
            	m_tipo_painel = value;
            }
        }

        protected TipoEstimacaoDadosPainel m_tipo_estimacao_painel = TipoEstimacaoDadosPainel.PooledOLS;
        public TipoEstimacaoDadosPainel TipoEstimacaoPainel
        {
            get { return this.m_tipo_estimacao_painel; }
            set { m_tipo_estimacao_painel = value; }
        }

        protected int m_num_unidades = 0;
        protected object[,] m_unidades = new object[0, 0];
        protected object[,] m_lista_unidades = new object[0, 0];
        protected string m_variavel_unidade_observacional = "";
        public string VarUnidadeObservacional
        {
            get { return m_variavel_unidade_observacional; }
            set { this.m_variavel_unidade_observacional = value; }
        }

        protected int m_num_periodos = 0;
        protected object[,] m_periodos = new object[0, 0];
        protected object[,] m_lista_periodos = new object[0, 0];
        protected string m_variavel_unidade_temporal = "";
        public string VarUnidadeTemporal
        {
            get { return m_variavel_unidade_temporal; }
            set { this.m_variavel_unidade_temporal = value; }
        }

        protected string m_text_efeitos_fixos = "";
        public string TextoEfeitosFixos
        {
            get
            {
                return m_text_efeitos_fixos;
            }
        }

        protected double[,] m_Y_hat = new double[0, 0];
        protected object[,] m_ordem_original = new object[0, 0];
        protected double[,] m_efeitos_unidades = new double[0, 0];
        protected double[,] m_efeitos_periodos = new double[0, 0]; 

        #endregion

        #region funções auxiliares

        public ArrayList possivel_lista_vars_periodos(DataTable dt, string var_unidades)
        {
            ArrayList res = new ArrayList();

            clsUtilTools clt = new clsUtilTools();

            object[,] v = new object[0, 0];
            string variavel = "";

            for (int j = 0; j < dt.Columns.Count; j++)
            {
                variavel = dt.Columns[j].ColumnName;
                if (variavel != var_unidades)
                {
                    v = clt.DataTableToObjectMatrix(dt, variavel);
                    if (possivel_variavel_periodos(dt, var_unidades, variavel))
                    {
                        res.Add(dt.Columns[j].ColumnName);
                    }
                }
            }

            return res;
        }

        protected bool possivel_variavel_periodos(DataTable dt, string v_unidades, string v_periodos)
        {
            string[] variaveis = new string[2];
            variaveis[0] = v_unidades;
            variaveis[1] = v_periodos;

            object[,] lista_categorias = new object[0, 0];
            int[] frequencias_categorias = new int[0];
            int numero_categorias = 0;
            int[] sorting_columns = new int[2];
            sorting_columns[1] = 1;

            object[,] tabela = m_clt.DataTableToObjectMatrix(dt, variaveis);
            m_clt.SortByColumn(ref tabela, ref lista_categorias, ref frequencias_categorias, ref numero_categorias, tabela, sorting_columns);

            int first = 0;
            int last = 0;
            string foco = "";

            for (int k = 0; k < Math.Min(100, frequencias_categorias.GetLength(0)); k++)
            {
                if (k == 0)
                {
                    first = 0;
                    last = frequencias_categorias[0] - 1;
                }
                else
                {
                    first = first + frequencias_categorias[k-1];
                    last = last + frequencias_categorias[k];
                }

                foco = "";
                for (int i = first; i <= last; i++)
                {
                    if (foco != tabela[i, 1].ToString())
                    {
                        foco = tabela[i, 1].ToString();
                    }
                    else return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Retorna uma matriz de dados para as observações para o período t.
        /// </summary>
        /// <param name="m">Matriz de dados. Assume-se que essa matriz está ordenada por unidade observacional e por período de tempo.</param>
        /// <param name="t">Índice do período de tempo (de 0 a T-1).</param>
        /// <returns>Dados para o período t.</returns>
        protected double[,] painel_periodo(double[,] m, int t)
        {
            if (m_tipo_painel == TipoPainel.Balanceado)
            {
                if (t >= m_num_periodos || t < 0) throw new Exception("Índice da unidade de tempo fora do intervalo permitido.");

                double[,] r = new double[m_num_unidades, m.GetLength(1)];
                for (int i = 0; i < r.GetLength(0); i++)
                {
                    for (int j = 0; j < r.GetLength(1); j++)
                    {
                        r[i, j] = m[i * m_num_periodos + t, j];
                    }
                }
                return r;
            }
            else
            {
                if (t >= m_num_periodos || t < 0) throw new Exception("Índice da unidade de tempo fora do intervalo permitido.");

                double[,] r = new double[m_num_unidades, m.GetLength(1)];
                for (int i = 0; i < r.GetLength(0); i++)
                {
                    for (int j = 0; j < r.GetLength(1); j++)
                    {
                        r[i, j] = m[i * m_num_periodos + t, j];
                    }
                }
                return r;
            }
        }

        /// <summary>
        /// Retorna uma matriz de dados para as observações para a unidade observacional i.
        /// </summary>
        /// <param name="m">Matriz de dados. Assume-se que essa matriz está ordenada por unidade observacional e por período de tempo.</param>
        /// <param name="i">Índice da unidade observacional (de 0 a N-1).</param>
        /// <returns>Dados para o unidade observacional i.</returns>
        protected double[,] painel_unidade(double[,] m, int i)
        {
            if (m_tipo_painel == TipoPainel.Balanceado)
            {
                if (i >= m_num_unidades || i<0) throw new Exception("Índice da unidade observacional fora do intervalo permitido.");

                double[,] r = new double[m_num_periodos, m.GetLength(1)];
                for (int t = 0; t < r.GetLength(0); t++)
                {
                    for (int j = 0; j < r.GetLength(1); j++)
                    {
                        r[t, j] = m[i * m_num_periodos + t, j];
                    }
                }
                return r;
            }
            else
            {
                if (i >= m_num_unidades || i < 0) throw new Exception("Índice da unidade observacional fora do intervalo permitido.");

                double[,] r = new double[m_num_periodos, m.GetLength(1)];
                for (int t = 0; t < r.GetLength(0); t++)
                {
                    for (int j = 0; j < r.GetLength(1); j++)
                    {
                        r[t, j] = m[i * m_num_periodos + t, j];
                    }
                }
                return r;
            }
        }

        /// <summary>
        /// Retorna a linha da unidade observacional i, no período t, em uma determinada matriz de dados.
        /// </summary>
        /// <param name="m">Matriz de dados. Assume-se que essa matriz está ordenada por unidade observacional e por período de tempo.</param>
        /// <param name="i">Índice da unidade observacional (de 0 a N-1).</param>
        /// <param name="t">Índice da unidade de tempo (de 0 a T-1).</param>
        /// <returns>Retorna a linha da matriz na posição específica.</returns>
        protected double[,] painel_row(double[,] m, int i, int t)
        {
            if (m_tipo_painel == TipoPainel.Balanceado)
            {
                if (i >= m_num_unidades || i < 0) throw new Exception("Índice da unidade observacional fora do intervalo permitido.");
                if (t >= m_num_periodos || t < 0) throw new Exception("Índice da unidade de tempo fora do intervalo permitido.");
                return m_clt.SubRowArrayDouble(m, i * m_num_periodos + t);
            }
            else
            {
                if (i >= m_num_unidades || i < 0) throw new Exception("Índice da unidade observacional fora do intervalo permitido.");
                if (t >= m_num_periodos || t < 0) throw new Exception("Índice da unidade de tempo fora do intervalo permitido.");
                return m_clt.SubRowArrayDouble(m, i * m_num_periodos + t);
            }
        }

        /// <summary>
        /// Retorna o elemento da unidade observacional i, no período t, da coluna col em uma determinada matriz de dados.
        /// </summary>
        /// <param name="m">Matriz de dados. Assume-se que essa matriz está ordenada por unidade observacional e por período de tempo.</param>
        /// <param name="i">Índice da unidade observacional (de 0 a N-1).</param>
        /// <param name="t">Índice da unidade de tempo (de 0 a T-1).</param>
        /// <param name="col">Coluna da matriz para o elemento a ser acessado.</param>
        /// <returns>Retorna o elemento da matriz na posição específica.</returns>
        protected double painel_elemento(double[,] m, int i, int t, int col)
        {
            if (m_tipo_painel == TipoPainel.Balanceado)
            {
                if (i >= m_num_unidades || i < 0) throw new Exception("Índice da unidade observacional fora do intervalo permitido.");
                if (t >= m_num_periodos || t < 0) throw new Exception("Índice da unidade de tempo fora do intervalo permitido.");
                return m[i * m_num_periodos + t, col];
            }
            else
            {
                if (i >= m_num_unidades || i < 0) throw new Exception("Índice da unidade observacional fora do intervalo permitido.");
                if (t >= m_num_periodos || t < 0) throw new Exception("Índice da unidade de tempo fora do intervalo permitido.");
                return m[i * m_num_periodos + t, col];
            }
        }

        protected int m_numero_dummies_temporais = 0;
        protected int m_numero_tendencias_temporais = 0;

        protected void GeraDummiesTemporais(out double[,] dummies_temporais, out string[] nomes_dummies)
        {
            object[,] dados = m_clt.GetObjMatrizFromDataTable(m_dt_tabela_dados, m_variavel_unidade_temporal);
            object[,] freqs = new object[0, 0];
            m_clt.FrequencyTable(ref freqs, dados);

            double[,] dts = new double[dados.GetLength(0), freqs.GetLength(0)];
            for (int i = 0; i < dts.GetLength(0); i++)
            {
                for (int j = 0; j < dts.GetLength(1); j++)
                {
                    if (dados[i, 0].ToString() == freqs[j, 0].ToString())
                    {
                        dts[i, j] = 1.0;
                        break;
                    }
                }
            }

            dummies_temporais = dts;
            nomes_dummies = new string[freqs.GetLength(0)];
            for (int j = 0; j < freqs.GetLength(0); j++) nomes_dummies[j] = m_clt.RetornaNovoNomeSemRepeticao(m_variaveis_independentes, "Dummy " + freqs[j, 0].ToString()); 
        }

        protected void GeraTendenciaTemporal(out double[,] tendencia_temporal, out string[] nomes_tendencias, bool gera_tend_linear, bool gera_tend_quadratica, bool gera_tend_cubica)
        {
            int ncols = 1;

            if (gera_tend_linear && gera_tend_quadratica && gera_tend_cubica)
            {
                ncols = 3;
                nomes_tendencias = new string[ncols];
                nomes_tendencias[0] = m_clt.RetornaNovoNomeSemRepeticao(m_variaveis_independentes, "_Tendencia_linear");
                nomes_tendencias[1] = m_clt.RetornaNovoNomeSemRepeticao(m_variaveis_independentes, "_Tendencia_quadratica");
                nomes_tendencias[2] = m_clt.RetornaNovoNomeSemRepeticao(m_variaveis_independentes, "_Tendencia_cubica");
            }
            else if (gera_tend_linear && gera_tend_quadratica)
            {
                ncols = 2;
                nomes_tendencias = new string[ncols];
                nomes_tendencias[0] = m_clt.RetornaNovoNomeSemRepeticao(m_variaveis_independentes, "_Tendencia_linear");
                nomes_tendencias[1] = m_clt.RetornaNovoNomeSemRepeticao(m_variaveis_independentes, "_Tendencia_quadratica");
            }
            else if (gera_tend_linear && gera_tend_cubica)
            {
                ncols = 2;
                nomes_tendencias = new string[ncols];
                nomes_tendencias[0] = m_clt.RetornaNovoNomeSemRepeticao(m_variaveis_independentes, "_Tendencia_linear");
                nomes_tendencias[1] = m_clt.RetornaNovoNomeSemRepeticao(m_variaveis_independentes, "_Tendencia_cubica");
            }
            else if (gera_tend_quadratica && gera_tend_cubica)
            {
                ncols = 2;
                nomes_tendencias = new string[ncols];
                nomes_tendencias[0] = m_clt.RetornaNovoNomeSemRepeticao(m_variaveis_independentes, "_Tendencia_quadratica");
                nomes_tendencias[1] = m_clt.RetornaNovoNomeSemRepeticao(m_variaveis_independentes, "_Tendencia_cubica");
            }
            else if (gera_tend_linear)
            {
                nomes_tendencias = new string[ncols];
                nomes_tendencias[0] = m_clt.RetornaNovoNomeSemRepeticao(m_variaveis_independentes, "_Tendencia_linear");
            }
            else if (gera_tend_quadratica)
            {
                nomes_tendencias = new string[ncols];
                nomes_tendencias[0] = m_clt.RetornaNovoNomeSemRepeticao(m_variaveis_independentes, "_Tendencia_quadratica");
            }
            else // (gera_tend_cubica)
            {
                nomes_tendencias = new string[ncols];
                nomes_tendencias[0] = m_clt.RetornaNovoNomeSemRepeticao(m_variaveis_independentes, "_Tendencia_cubica");
            }

            tendencia_temporal = new double[m_periodos.GetLength(0), ncols];

            string unidade_foco = m_unidades[0, 0].ToString();
            int contador_periodo = 0;
            for (int i = 0; i < m_unidades.GetLength(0); i++)
            {
                if (unidade_foco == m_unidades[i, 0].ToString())
                {
                    if (gera_tend_linear && gera_tend_quadratica && gera_tend_cubica)
                    {
                        tendencia_temporal[i, 0] = (double)contador_periodo;
                        tendencia_temporal[i, 1] = Math.Pow((double)contador_periodo, 2.0);
                        tendencia_temporal[i, 2] = Math.Pow((double)contador_periodo, 3.0);
                    }
                    else if (gera_tend_linear && gera_tend_quadratica)
                    {
                        tendencia_temporal[i, 0] = (double)contador_periodo;
                        tendencia_temporal[i, 1] = Math.Pow((double)contador_periodo, 2.0);
                    }
                    else if (gera_tend_linear && gera_tend_cubica)
                    {
                        tendencia_temporal[i, 0] = (double)contador_periodo;
                        tendencia_temporal[i, 1] = Math.Pow((double)contador_periodo, 3.0);
                    }
                    else if (gera_tend_quadratica && gera_tend_cubica)
                    {
                        tendencia_temporal[i, 0] = Math.Pow((double)contador_periodo, 2.0);
                        tendencia_temporal[i, 1] = Math.Pow((double)contador_periodo, 3.0);
                    }
                    else if (gera_tend_linear)
                    {
                        tendencia_temporal[i, 0] = (double)contador_periodo;
                    }
                    else if (gera_tend_quadratica)
                    {
                        tendencia_temporal[i, 0] = Math.Pow((double)contador_periodo, 2.0);
                    }
                    else // (gera_tend_cubica)
                    {
                        tendencia_temporal[i, 0] = Math.Pow((double)contador_periodo, 3.0);
                    }

                    contador_periodo++;
                }
                else
                {
                    unidade_foco = m_unidades[i, 0].ToString();
                    contador_periodo = 0;

                    if (gera_tend_linear && gera_tend_quadratica && gera_tend_cubica)
                    {
                        tendencia_temporal[i, 0] = (double)contador_periodo;
                        tendencia_temporal[i, 1] = Math.Pow((double)contador_periodo, 2.0);
                        tendencia_temporal[i, 2] = Math.Pow((double)contador_periodo, 3.0);
                    }
                    else if (gera_tend_linear && gera_tend_quadratica)
                    {
                        tendencia_temporal[i, 0] = (double)contador_periodo;
                        tendencia_temporal[i, 1] = Math.Pow((double)contador_periodo, 2.0);
                    }
                    else if (gera_tend_linear && gera_tend_cubica)
                    {
                        tendencia_temporal[i, 0] = (double)contador_periodo;
                        tendencia_temporal[i, 1] = Math.Pow((double)contador_periodo, 3.0);
                    }
                    else if (gera_tend_quadratica && gera_tend_cubica)
                    {
                        tendencia_temporal[i, 0] = Math.Pow((double)contador_periodo, 2.0);
                        tendencia_temporal[i, 1] = Math.Pow((double)contador_periodo, 3.0);
                    }
                    else if (gera_tend_linear)
                    {
                        tendencia_temporal[i, 0] = (double)contador_periodo;
                    }
                    else if (gera_tend_quadratica)
                    {
                        tendencia_temporal[i, 0] = Math.Pow((double)contador_periodo, 2.0);
                    }
                    else // (gera_tend_cubica)
                    {
                        tendencia_temporal[i, 0] = Math.Pow((double)contador_periodo, 3.0);
                    }

                    contador_periodo++;
                }
            }
        }

        protected void OrdemPeriodoTempoPainelBalanceado(out double[,] ys, out double[,] Xs, double[,] y, double[,] X)
        {
            ys = new double[y.GetLength(0), 1];
            Xs = new double[X.GetLength(0), X.GetLength(1)];
            
            for (int i = 0; i < m_num_unidades; i++)
            {
                for (int k = 0; k < m_num_periodos; k++)
                {
                    ys[k*m_num_unidades + i, 0] = y[i * m_num_periodos + k, 0];
                    for (int j = 0; j < X.GetLength(1); j++)
                    {
                        Xs[k * m_num_unidades + i, j] = X[i * m_num_periodos + k, j];
                    }
                }
            }
        }

        protected void OrganizaPainelBalanceado()
        {
            m_periodos = m_clt.GetObjMatrizFromDataTable(m_dt_tabela_dados, m_variavel_unidade_temporal);
            m_unidades = m_clt.GetObjMatrizFromDataTable(m_dt_tabela_dados, m_variavel_unidade_observacional);

            m_ordem_original = new object[m_dt_tabela_dados.Rows.Count, 1];
            for (int i = 0; i < m_ordem_original.GetLength(0); i++) m_ordem_original[i, 0] = i; 

            object[,] tab_freq = new object[0, 0];
            m_clt.FrequencyTable(ref tab_freq, m_periodos);
            int k = Convert.ToInt32(tab_freq[0, 1]);
            for (int i = 1; i < tab_freq.GetLength(0); i++)
            {
                if (Convert.ToInt32(tab_freq[i, 1]) != k)
                {
                    throw new Exception("Painel de dados não é balanceado.");
                }
            }
            m_num_periodos = tab_freq.GetLength(0);
            m_lista_periodos = m_clt.SubColumnArrayObject(tab_freq, 0);

            m_clt.FrequencyTable(ref tab_freq, m_unidades);
            int T = Convert.ToInt32(tab_freq[0, 1]);
            for (int i = 1; i < tab_freq.GetLength(0); i++)
            {
                if (Convert.ToInt32(tab_freq[i, 1]) != T)
                {
                    throw new Exception("Painel de dados não é balanceado.");
                }
            }
            m_num_unidades = tab_freq.GetLength(0);
            m_lista_unidades = m_clt.SubColumnArrayObject(tab_freq, 0);

            m_X = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_independentes);
            m_Y = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_dependentes);

            object[,] all_dados = m_clt.Concateh(m_clt.Concateh(m_unidades, m_periodos), m_clt.Concateh(m_Y, m_X));
            all_dados = m_clt.Concateh(m_ordem_original, all_dados);

            int num_categorias = 0;
            int[] freq_categorias = new int[0];
            object[,] lista_categorias = new object[0,0];
            int[] sorting_columns = new int[2];
            sorting_columns[0] = 1;
            sorting_columns[1] = 2;

            m_clt.SortByColumn(ref all_dados, ref lista_categorias, ref freq_categorias, ref num_categorias, all_dados, sorting_columns);

            m_ordem_original = m_clt.SubColumnArrayDouble(all_dados, 0); 
            m_unidades = m_clt.SubColumnArrayDouble(all_dados, 1);
            m_periodos = m_clt.SubColumnArrayDouble(all_dados, 2);

            m_Y = m_clt.ConvertMatrixObjToDouble(m_clt.SubColumnArrayObject(all_dados, 3));
            int[] cols_X = new int[m_X.GetLength(1)];
            for (int i = 0; i < cols_X.GetLength(0); i++)
            {
                cols_X[i] = i + 4;
            }
            m_X = m_clt.ConvertMatrixObjToDouble(m_clt.SubColumnsArrayObject(all_dados, cols_X));

            ChecaVariabilidadeTemporalEfeitosFixos(m_X, m_num_unidades, m_num_periodos);
        }

        protected double[,] OrganizaEfeitosIdiossincraticosUnidades(double[,] c)
        {
            double[,] res = new double[m_dt_tabela_dados.Rows.Count, 1];

            for (int i = 0; i < m_num_unidades; i++)
            {
                for (int j = 0; j < m_num_periodos; j++)
                {
                    res[i * m_num_periodos + j, 0] = c[i,0];
                }
            }

            return res;
        }

        #endregion

        #region chamada das funções de estimação

        clsUtilTools m_clt = new clsUtilTools();

        protected double[,] m_dummies_temporais = new double[0, 0];
        protected string[] m_nomes_dummies_temporais = new string[0];
        protected double[,] m_tendencia_temporal = new double[0, 0];
        protected string[] m_nomes_tendencia = new string[0];

        public void EstimarModelo()
        {
            this.OrganizaPainelBalanceado();

            double[,] X_original = m_clt.ArrayDoubleClone(m_X);

            if (m_usa_intercepto && m_tipo_estimacao_painel != TipoEstimacaoDadosPainel.EfeitosFixos 
                && m_tipo_estimacao_painel != TipoEstimacaoDadosPainel.PrimeirasDiferencas)
            {
                m_X = m_clt.Concateh(m_clt.Ones(m_X.GetLength(0), 1), m_X);
            }

            if (m_usa_dummies_temporais)
            {
                this.GeraDummiesTemporais(out m_dummies_temporais, out m_nomes_dummies_temporais);

                if (m_usa_intercepto || m_tipo_estimacao_painel != TipoEstimacaoDadosPainel.EfeitosFixos
                       || m_tipo_estimacao_painel != TipoEstimacaoDadosPainel.PrimeirasDiferencas)
                {
                    m_dummies_temporais = m_clt.RemoveColumnArrayDouble(m_dummies_temporais, m_dummies_temporais.GetLength(1) - 1);
                    m_nomes_dummies_temporais = m_clt.RemoveElementoArrayString(m_nomes_dummies_temporais, m_nomes_dummies_temporais.GetLength(0) - 1);
                }

                m_X = m_clt.Concateh(m_X, m_dummies_temporais);
                X_original = m_clt.Concateh(X_original, m_dummies_temporais);

                m_numero_dummies_temporais = m_dummies_temporais.GetLength(1);
            }
            else
            {
                m_numero_dummies_temporais = 0;
                m_nomes_dummies_temporais = new string[0];
                m_dummies_temporais = new double[0, 0];
            }

            if (m_usa_tendencia_temporal_linear || m_usa_tendencia_temporal_quadratica || m_usa_tendencia_temporal_cubica)
            {
                this.GeraTendenciaTemporal(out m_tendencia_temporal, out m_nomes_tendencia, m_usa_tendencia_temporal_linear, m_usa_tendencia_temporal_quadratica, m_usa_tendencia_temporal_cubica);

                m_X = m_clt.Concateh(m_X, m_tendencia_temporal);
                X_original = m_clt.Concateh(X_original, m_tendencia_temporal);
                m_numero_tendencias_temporais = m_tendencia_temporal.GetLength(1);
            }
            else
            {
                m_numero_tendencias_temporais = 0;
                m_tendencia_temporal = new double[0, 0];
                m_nomes_tendencia = new string[0];
            }

            #region checando multicolinearidade depois da adição de dummies e tendencias temporais

            if (m_usa_dummies_temporais || m_usa_tendencia_temporal_linear || m_usa_tendencia_temporal_quadratica || m_usa_tendencia_temporal_cubica)
            {
                string[] nomes = m_clt.Concate(m_clt.Concate(m_variaveis_independentes, m_nomes_dummies_temporais), m_nomes_tendencia);
                DataTable dt_temp = m_clt.DataTableFromMatriz(X_original, nomes);
                                
                BLogicRegressaoLinear blr = new BLogicRegressaoLinear();

                blr.VariaveisIndependentes = nomes;
                blr.TabelaDados = dt_temp;
                blr.intercepto = (m_usa_intercepto && m_tipo_estimacao_painel != TipoEstimacaoDadosPainel.EfeitosFixos
                    && m_tipo_estimacao_painel != TipoEstimacaoDadosPainel.PrimeirasDiferencas);

                string mensagem_colinear = "";
                
                if (blr.ChecarMulticolinearidade(out mensagem_colinear))
                {
                    throw new Exception(mensagem_colinear + " Cheque a sua base de dados ou mude a especificação do modelo.");
                }
            }

            #endregion

            switch (m_tipo_estimacao_painel)
            {
                case TipoEstimacaoDadosPainel.EfeitosAleatorios:
                    this.EstimacaoEfeitosAleatorios();
                    break;
                case TipoEstimacaoDadosPainel.EfeitosFixos:
                    m_usa_intercepto = false;
                    if (this.TipoPainelDados == TipoPainel.Balanceado) this.EstimacaoEfeitosFixosPainelBalanceado();
                    else this.EstimacaoEfeitosFixosPainelBalanceado();
                    break;
                case TipoEstimacaoDadosPainel.PooledOLS:
                    this.EstimacaoPooledOLS();
                    break;
                case TipoEstimacaoDadosPainel.PrimeirasDiferencas:
                    this.m_usa_intercepto = false;
                    this.EstimacaoPrimeirasDiferencas();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region estimação via pooled OLS

        protected void EstimacaoPooledOLS()
        {
            PooledOLS pols = new PooledOLS();

            EstimarPooledOLS(out pols);

            m_beta_hat = pols.beta_POLS;
            m_beta_hat_cov = pols.beta_POLS_cov_mat;
            this.GeraSignificanciaCoeficientes(false);

            #region gerando o output para resultado das estimacoes

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação para Dados em Painel (Pooled OLS) \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
            out_text += "Variavel dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + m_X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + m_X.GetLength(1) + "\n";
            out_text += "Variancia dos erros: " + m_clt.Double2Texto(pols.sigma2_hat, 6) + "\n";
            if (m_usa_general_FGLS_analysis)
            {
                out_text += "Estimação utilizando FGLS (feasible generalized least squares) \n";
            }
            if (m_usa_robust_cov_matrix)
            {
                out_text += "Estimação robusta para a matriz de covariâncias \n";
            }
            out_text += "\n";

            out_text += GeraTabelaEstimacoes(m_clt.Concate(m_clt.Concate(VariaveisIndependentes, m_nomes_dummies_temporais), m_nomes_tendencia), 
                                                                            m_beta_hat, m_beta_stderror, m_beta_tstat, m_beta_pvalor, m_usa_intercepto);
            out_text += "\n";

            out_text += "Soma dos quadrados totais: " + m_clt.Double2Texto(pols.SST, 6) + "\n";
            out_text += "Soma dos quadrados da regressão: " + m_clt.Double2Texto(pols.SSR, 6) + "\n";
            out_text += "Soma dos quadrados dos resíduos: " + m_clt.Double2Texto(pols.SSE, 6) + "\n\n";

            out_text += "R2 (coeficiente de determinação): " + m_clt.Double2Texto(pols.R2, 6) + "\n";
            out_text += "R2-ajustado: " + m_clt.Double2Texto(pols.R2_ajustado, 6) + "\n\n";

            out_text += "F (estatistica teste): " + m_clt.Double2Texto(pols.Fstat, 6) + "\n";
            out_text += "F (p-valor): " + m_clt.Double2Texto(pols.Fpvalue, 6) + "\n\n";

            out_text += "Log-likelihood: " + m_clt.Double2Texto(pols.loglik, 6) + "\n";
            out_text += "AIC: " + m_clt.Double2Texto(AIC, 6) + "\n";
            out_text += "BIC: " + m_clt.Double2Texto(BIC, 6) + "\n\n";

            out_text += "Teste para autocorrelação nos resíduos da regressão (indicador de presença de efeitos idiossincráticos não observáveis): \n";
            out_text += "Estatística teste: " + m_clt.Double2Texto(pols.sigma2_c_test_stat, 6) + "\n";
            out_text += "P-valor: " + m_clt.Double2Texto(pols.sigma2_c_pvalue, 6) + "\n";

            if (m_numero_dummies_temporais > 0)
            {
                out_text += "\n";
                out_text += "Teste de Wald para significância das dummies temporais (para os diferentes períodos de tempo): \n";
                out_text += "Estatística teste: " + m_clt.Double2Texto(pols.Wtest, 6) + "\n";
                out_text += "P-valor: " + m_clt.Double2Texto(pols.Wpvalor, 6) + "\n";
                out_text += "Número de restrições: " + m_numero_dummies_temporais + "\n";
            }

            if (m_numero_tendencias_temporais > 0)
            {
                out_text += "\n";
                out_text += "Teste de Wald para significância das tendências temporais escolhidas: \n";
                out_text += "Estatística teste: " + m_clt.Double2Texto(pols.Wtest, 6) + "\n";
                out_text += "P-valor: " + m_clt.Double2Texto(pols.Wpvalor, 6) + "\n";
                out_text += "Número de restrições: " + m_numero_tendencias_temporais + "\n";
            }

            out_text += "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";
                out_text += this.GeraTabelaCovMatrix(m_beta_hat_cov, m_clt.Concate(m_clt.Concate(VariaveisIndependentes, m_nomes_dummies_temporais), m_nomes_tendencia), m_usa_intercepto);
                out_text += "\n";
            }

            this.m_output_text = out_text;

            #endregion
            
            #region adicionando variveis base de dados

            for (int i = 0; i < m_Y.GetLength(0); i++)
            {
                m_ordem_original[i,0] = 1 + Convert.ToInt32(m_ordem_original[i,0]);
            }

            object[,] novas_variaveis = m_clt.Concateh(m_ordem_original, m_unidades);
            novas_variaveis = m_clt.Concateh(novas_variaveis, m_periodos);
            novas_variaveis = m_clt.Concateh(novas_variaveis, m_clt.Concateh(m_Y, m_clt.Concateh(pols.y_hat_POLS, pols.residuos_POLS)));

            m_clt.SortByColumn(ref novas_variaveis, novas_variaveis, 0); 

            string[] nomes_variaveis = new string[6];
            nomes_variaveis[0] = "Observacao_";
            nomes_variaveis[1] = "Unidade_observacional_";
            nomes_variaveis[2] = "Periodo_de_tempo_";
            nomes_variaveis[3] = "Y_observado_";
            nomes_variaveis[4] = "Y_predito_";
            nomes_variaveis[5] = "Residuo_";

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação para Dados em Painel (Pooled OLS) \n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(novas_variaveis, nomes_variaveis);
            
            m_output_variaveis_geradas += "\n";
            
            //------------- adicionando à tabela de dados no datagrid

            string[] nomes_vars_dt = new string[nomes_variaveis.GetLength(0) - 3];
            for (int i = 0; i < nomes_vars_dt.GetLength(0); i++) nomes_vars_dt[i] = nomes_variaveis[i + 3];

            novas_variaveis = m_clt.SubColumnsArrayObject(novas_variaveis, 3, novas_variaveis.GetLength(1) - 1); 
            
            AdicionaNovasVariaveisToDataTable(novas_variaveis, nomes_vars_dt);

            #endregion
        }

        private void EstimarPooledOLS(out PooledOLS pols)
        {
            double[,] XtX = m_clt.MatrizMult(m_clt.MatrizTransp(m_X), m_X);
            double[,] XtY = m_clt.MatrizMult(m_clt.MatrizTransp(m_X), m_Y);

            double[,] inv_XtX = m_clt.MatrizInversa(XtX);
            pols.beta_POLS = m_clt.MatrizMult(inv_XtX, XtY);
            pols.y_hat_POLS = m_clt.MatrizMult(m_X, pols.beta_POLS);
            double[,] u_pols = m_clt.MatrizSubtracao(m_Y, pols.y_hat_POLS);
            pols.residuos_POLS = u_pols;

            pols.sigma2_hat = m_clt.MatrizMult(m_clt.MatrizTransp(u_pols), u_pols)[0, 0] 
                            / ((double)(m_num_periodos * m_num_unidades - m_X.GetLength(1)));

            if (!m_usa_robust_cov_matrix)
            {
                pols.beta_POLS_cov_mat = m_clt.MatrizMult(pols.sigma2_hat, inv_XtX);
            }
            else
            {
                double[,] m3 = new double[m_X.GetLength(1), m_X.GetLength(1)];
                double[,] ui;
                double[,] Xtui;
                double[,] Xi;

                for (int i = 0; i < m_num_unidades; i++)
                {
                    Xi = painel_unidade(m_X, i);
                    ui = painel_unidade(u_pols, i);

                    Xtui = m_clt.MatrizMult(m_clt.MatrizTransp(Xi), ui);
                    m3 = m_clt.MatrizSoma(m3, m_clt.MatrizMult(Xtui, m_clt.MatrizTransp(Xtui)));
                }

                pols.beta_POLS_cov_mat = m_clt.MatrizMult(m_clt.MatrizMult(inv_XtX, m3), inv_XtX);
            }

            //----- calculando o R2
            if (m_usa_intercepto) 
            {
                pols.SST = (m_clt.MatrizMultMtranspM(m_clt.MatrizSubtracao(m_Y, m_clt.Mean(m_Y))))[0, 0]; 
            }
            else pols.SST = (m_clt.MatrizMultMtranspM(m_Y))[0, 0];

            pols.SSR = (m_clt.MatrizMultMtranspM(m_clt.MatrizSubtracao(pols.y_hat_POLS, m_clt.Mean(pols.y_hat_POLS))))[0, 0];
            pols.SSE = (m_clt.MatrizMultMtranspM(u_pols))[0, 0];

            pols.R2 = pols.SSR / pols.SST;

            pols.R2_ajustado = 1.0 - (1.0 - pols.R2) * ((double)(m_num_unidades * m_num_periodos) - 1.0) / ((double)(m_num_unidades * m_num_periodos) - (double)m_X.GetLength(1));

            //------ teste F
            pols.Fstat = ((pols.SST - pols.SSE) / pols.SSE) * ((double)(m_num_periodos * m_num_unidades - m_X.GetLength(1))) / ((double)(m_X.GetLength(1) - 1));

            FisherSnedecor fdist = new FisherSnedecor(X.GetLength(1) - 1, X.GetLength(0) - X.GetLength(1));
            pols.Fpvalue = 1.0 - fdist.CumulativeDistribution(pols.Fstat);
            
            //------- loglik
            pols.loglik = -((double)(m_num_periodos * m_num_unidades) / 2.0) * (1.0 + Math.Log(2.0 * Math.PI) + Math.Log(pols.SSE / ((double)(m_num_unidades * m_num_periodos))));
            pols.AIC = (-2.0 * pols.loglik + 2.0 * (double)m_X.GetLength(1)) / ((double)(m_num_periodos * m_num_unidades));
            pols.BIC = (-2.0 * pols.loglik + Math.Log((double)(m_num_periodos * m_num_unidades)) * (double)m_X.GetLength(1)) / ((double)(m_num_periodos * m_num_unidades));
            
            #region testing serial correlation nos residuos

            // trata-se de um teste para a presença de termos idiossincráticos não-observáveis 
            // o teste está descrito em Woodridge (2002), página 264, seção 10.4.4

            double num_test_stat = 0.0;
            double den_test_stat = 0.0;
            double aux_den_test_stat = 0.0;

            for (int i = 0; i < m_num_unidades; i++)
            {
                aux_den_test_stat = 0.0;
                for (int t = 0; t < m_num_periodos - 1; t++)
                {
                    for (int s = t + 1; s < m_num_periodos; s++)
                    {
                        num_test_stat += painel_elemento(u_pols, i, t, 0) * painel_elemento(u_pols, i, s, 0);
                        aux_den_test_stat += painel_elemento(u_pols, i, t, 0) * painel_elemento(u_pols, i, s, 0);
                    }
                }
                den_test_stat += Math.Pow(aux_den_test_stat, 2.0);
            }
            den_test_stat = Math.Pow(den_test_stat, 0.5);

            Normal normal = new Normal();

            pols.sigma2_c_test_stat = num_test_stat / den_test_stat;
            pols.sigma2_c_pvalue = 2.0 * (1.0 - normal.CumulativeDistribution(Math.Abs(pols.sigma2_c_test_stat)));

            #endregion

            #region testando a significância das dummies de tendencia e temporais

            this.TestaDummiesETendenciasTemporais(pols.beta_POLS, pols.beta_POLS_cov_mat, out pols.Wtest, out pols.Wpvalor);

            #endregion
        }

        #endregion

        #region estimação via efeitos aleatórios para painel balanceado

        protected void EstimacaoEfeitosAleatorios()
        {
            RandomEffects re = new RandomEffects();

            EstimarRandomEffects(out re);

            m_beta_hat = re.beta_RE;
            m_beta_hat_cov = re.beta_RE_covmat;
            this.GeraSignificanciaCoeficientes(false);

            #region gerando o output para resultado das estimates

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação para Dados em Painel (Efeitos Aleatorios) \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
            out_text += "Variavel dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + m_X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + m_X.GetLength(1) + "\n";
            if (m_usa_general_FGLS_analysis)
            {
                out_text += "Estimação utilizando FGLS (feasible generalized least squares) \n";
            }
            if (m_usa_robust_cov_matrix)
            {
                out_text += "Estimação robusta para a matriz de covariâncias \n";
            }
            out_text += "\n";

            out_text += "Variancia total dos resíduos: " + m_clt.Double2Texto(re.sigma2_v, 6) + "\n";
            out_text += "Variancia dos termos não-observados: " + m_clt.Double2Texto(re.sigma2_c, 6) + "\n";
            out_text += "Variancia dos resíduos idiossincráticos: " + m_clt.Double2Texto(re.sigma2_u, 6) + "\n";
            out_text += "\n";

            out_text += GeraTabelaEstimacoes(m_clt.Concate(m_clt.Concate(VariaveisIndependentes, m_nomes_dummies_temporais), m_nomes_tendencia), 
                m_beta_hat, m_beta_stderror, m_beta_tstat, m_beta_pvalor, m_usa_intercepto);
            out_text += "\n";

            out_text += "Soma total dos quadrados: " + m_clt.Double2Texto(re.SST, 6) + "\n";
            out_text += "Soma dos quadrados da regressão: " + m_clt.Double2Texto(re.SSR, 6) + "\n";
            out_text += "Soma dos quadrados dos resíduos: " + m_clt.Double2Texto(re.SSE, 6) + "\n";
            out_text += "R2 (coeficiente de determinação): " + m_clt.Double2Texto(re.R2, 6) + "\n\n";

            out_text += "F (estatística teste): " + m_clt.Double2Texto(re.Fstat, 6) + "\n";
            out_text += "F (p-valor): " + m_clt.Double2Texto(re.Fpvalue, 6) + "\n\n";

            out_text += "Teste para autocorrelação nos resíduos da regressão (indicador de presença de efeitos idiossincráticos não observáveis): \n";
            out_text += "Estatística teste: " + m_clt.Double2Texto(re.sigma2_c_test_stat, 6) + "\n";
            out_text += "P-valor: " + m_clt.Double2Texto(re.sigma2_c_pvalue, 6) + "\n";

            if (m_numero_dummies_temporais > 0)
            {
                out_text += "\n";
                out_text += "Teste de Wald para significância das dummies temporais (para os diferentes períodos de tempo): \n";
                out_text += "Estatística teste: " + m_clt.Double2Texto(re.Wtest, 6) + "\n";
                out_text += "P-valor: " + m_clt.Double2Texto(re.Wpvalor, 6) + "\n";
                out_text += "Número de restrições: " + m_numero_dummies_temporais + "\n";
            }

            if (m_numero_tendencias_temporais > 0)
            {
                out_text += "\n";
                out_text += "Teste de Wald para significância das tendências temporais escolhidas: \n";
                out_text += "Estatística teste: " + m_clt.Double2Texto(re.Wtest, 6) + "\n";
                out_text += "P-valor: " + m_clt.Double2Texto(re.Wpvalor, 6) + "\n";
                out_text += "Número de restrições: " + m_numero_tendencias_temporais + "\n";
            }

            out_text += "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";
                out_text += this.GeraTabelaCovMatrix(m_beta_hat_cov, m_clt.Concate(m_clt.Concate(VariaveisIndependentes, m_nomes_dummies_temporais), m_nomes_tendencia), m_usa_intercepto);
                out_text += "\n";
            }

            this.m_output_text = out_text;

            #endregion

            #region adicionando variveis base de dados

            double[,] observacoes = new double[X.GetLength(0), 1];
            for (int i = 0; i < m_Y.GetLength(0); i++)
            {
                observacoes[i, 0] = (double)i;
            }

            object[,] novas_variaveis = m_clt.Concateh(observacoes, m_unidades);
            novas_variaveis = m_clt.Concateh(novas_variaveis, m_periodos);
            novas_variaveis = m_clt.Concateh(novas_variaveis, m_clt.Concateh(m_Y, m_clt.Concateh(re.y_hat_RE, re.residuos_RE)));

            string[] nomes_variaveis = new string[6];
            nomes_variaveis[0] = "Observacao_";
            nomes_variaveis[1] = "Unidade_observacional_";
            nomes_variaveis[2] = "Periodo_de_tempo_";
            nomes_variaveis[3] = "Y_observado_";
            nomes_variaveis[4] = "Y_predito_";
            nomes_variaveis[5] = "Residuo_";

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação para Dados em Painel (Efeitos Aleatorios) \n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(novas_variaveis, nomes_variaveis);

            m_output_variaveis_geradas += "\n";

            //------------- adicionando à tabela de dados no datagrid

            string[] nomes_vars_dt = new string[nomes_variaveis.GetLength(0) - 3];
            for (int i = 0; i < nomes_vars_dt.GetLength(0); i++) nomes_vars_dt[i] = nomes_variaveis[i + 3];

            novas_variaveis = m_clt.SubColumnsArrayObject(novas_variaveis, 3, novas_variaveis.GetLength(1) - 1);

            AdicionaNovasVariaveisToDataTable(novas_variaveis, nomes_vars_dt);

            #endregion
        }

        private void EstimarRandomEffects(out RandomEffects re)
        {
            double[,] XtX = (m_clt.MatrizMult(m_clt.MatrizTransp(m_X), m_X));
            double[,] XtY = (m_clt.MatrizMult(m_clt.MatrizTransp(m_X), m_Y));

            double[,] beta_pols = m_clt.MatrizMult(m_clt.MatrizInversa(XtX), XtY);
            double[,] y_hat_pols = m_clt.MatrizMult(m_X, beta_pols);
            double[,] e_pols = m_clt.MatrizSubtracao(m_Y, m_clt.MatrizMult(m_X, beta_pols));
            double sse = m_clt.MatrizMult(m_clt.MatrizTransp(e_pols), e_pols)[0,0];
            re.sigma2_v = sse / ((double)(m_num_periodos * m_num_unidades - m_X.GetLength(1)));

            // variância para o efeito aleatório
            // Fonte: Woodridge (2002), página 259 e equação (10.37) na página 261

            double SS_sigma2_c = 0.0;

            for (int i = 0; i < m_num_unidades; i++)
            {
                for (int t = 0; t < m_num_periodos-1; t++)
                {
                    for (int s = t+1; s < m_num_periodos; s++)
                    {
                        SS_sigma2_c += painel_elemento(e_pols, i, t, 0) * painel_elemento(e_pols, i, s, 0);
                    }
                }
            }
            re.sigma2_c = SS_sigma2_c / ((((double)(m_num_periodos * m_num_unidades * (m_num_periodos - 1))) / 2.0) - m_X.GetLength(0));
            re.sigma2_u = re.sigma2_v - re.sigma2_c;

            double[,] Omega = new double[m_X.GetLength(1), m_X.GetLength(1)];

            if (!m_usa_general_FGLS_analysis && re.sigma2_c > 0.0 && re.sigma2_u > 0.0)
            {
                // matriz Omega para tornar o estimador mais eficiente
                // especificação em Woodridge (2002), página 259

                Omega = m_clt.MatrizMult(re.sigma2_c, m_clt.Ones(m_num_periodos, m_num_periodos));
                Omega = m_clt.MatrizSoma(Omega, m_clt.MatrizMult(re.sigma2_u, m_clt.Identity(m_num_periodos)));
            }
            else
            {
                // matriz Omega considerando uma matriz geral para o estimador FGLS
                // especificação em Woodridge (2002), página 263

                Omega = new double[m_X.GetLength(1), m_X.GetLength(1)];

                double[,] ei = new double[0, 0];
                for (int i = 0; i < m_num_unidades; i++)
                {
                    ei = painel_unidade(e_pols, i);
                    Omega = m_clt.MatrizSoma(Omega, m_clt.MatrizMult(ei, m_clt.MatrizTransp(ei)));
                }

                Omega = m_clt.MatrizMult(Omega, 1.0 / ((double)m_num_unidades));
            }

            double[,] inv_Omega = m_clt.MatrizInversa(Omega);

            double[,] m1 = new double[m_X.GetLength(1), m_X.GetLength(1)];
            double[,] m2 = new double[m_X.GetLength(1), 1];

            double[,] Xi = new double[0, 0];
            double[,] Yi = new double[0, 0];
            for (int i = 0; i < m_num_unidades; i++)
            {
                Xi = painel_unidade(m_X, i);
                Yi = painel_unidade(m_Y, i);

                XtX = m_clt.MatrizMult(m_clt.MatrizTransp(Xi), m_clt.MatrizMult(inv_Omega, Xi));
                XtY = m_clt.MatrizMult(m_clt.MatrizTransp(Xi), m_clt.MatrizMult(inv_Omega, Yi));

                m1 = m_clt.MatrizSoma(m1, XtX);
                m2 = m_clt.MatrizSoma(m2, XtY);
            }

            double[,] inv_m1 = m_clt.MatrizInversa(m1);

            re.beta_RE = m_clt.MatrizMult(inv_m1, m2);
            re.y_hat_RE = m_clt.MatrizMult(m_X, re.beta_RE);
            double[,] u_re = m_clt.MatrizSubtracao(m_Y, re.y_hat_RE);
            re.residuos_RE = u_re;

            if (!m_usa_robust_cov_matrix)
            {
                re.beta_RE_covmat = inv_m1;
            }
            else
            {                
                double[,] m3 = new double[m_X.GetLength(1), m_X.GetLength(1)];
                double[,] invOmegaX;
                double[,] uiu;
                double[,] ui;

                for (int i = 0; i < m_num_unidades; i++)
                {
                    Xi = painel_unidade(m_X, i);
                    ui = painel_unidade(u_re, i);

                    invOmegaX = m_clt.MatrizMult(inv_Omega, Xi);
                    uiu = m_clt.MatrizMult(ui, m_clt.MatrizTransp(ui));

                    m3 = m_clt.MatrizSoma(m3, m_clt.MatrizMult(m_clt.MatrizMult(m_clt.MatrizTransp(invOmegaX), uiu), invOmegaX));
                }

                re.beta_RE_covmat = m_clt.MatrizMult(m_clt.MatrizMult(inv_m1, m3), inv_m1);
            }

            //--------------------- variáveis de output ---------------------------//

            //------ R2 com base no OLS 
            if (m_usa_intercepto)
            {
                re.SST = (m_clt.MatrizMultMtranspM(m_clt.MatrizSubtracao(m_Y, m_clt.Mean(m_Y))))[0, 0];
            }
            else re.SST = (m_clt.MatrizMultMtranspM(m_Y))[0, 0];

            re.SSR = (m_clt.MatrizMultMtranspM(m_clt.MatrizSubtracao(y_hat_pols, m_clt.Mean(y_hat_pols))))[0, 0];
            re.SSE = (m_clt.MatrizMultMtranspM(e_pols))[0, 0];

            re.R2 = re.SSR / re.SST;

            re.R2_ajustado = 1.0 - (1.0 - re.R2) * ((double)(m_num_unidades * m_num_periodos) - 1.0) / ((double)(m_num_unidades * m_num_periodos) - (double)m_X.GetLength(1));

            //------ teste F com base no OLS
            re.Fstat = ((re.SST - re.SSE) / re.SSE) * ((double)(m_num_periodos * m_num_unidades - m_X.GetLength(1))) / ((double)(m_X.GetLength(1) - 1));

            FisherSnedecor fdist = new FisherSnedecor(X.GetLength(1) - 1, X.GetLength(0) - X.GetLength(1));
            re.Fpvalue = 1.0 - fdist.CumulativeDistribution(re.Fstat);

            //------- loglik com base no OLS e modelo normal
            re.loglik = -((double)(m_num_periodos * m_num_unidades) / 2.0) * (1.0 + Math.Log(2.0 * Math.PI) + Math.Log(re.SSE / ((double)(m_num_unidades * m_num_periodos))));
            re.AIC = (-2.0 * re.loglik + 2.0 * (double)m_X.GetLength(1)) / ((double)(m_num_periodos * m_num_unidades));
            re.BIC = (-2.0 * re.loglik + Math.Log((double)(m_num_periodos * m_num_unidades)) * (double)m_X.GetLength(1)) / ((double)(m_num_periodos * m_num_unidades));

            #region testing serial correlation nos residuos

            // trata-se de um teste para a presença de termos idiossincráticos não-observáveis 
            // o teste está descrito em Woodridge (2002), página 264, seção 10.4.4

            beta_pols = m_clt.MatrizMult(m_clt.MatrizInversa(XtX), XtY);
            e_pols = m_clt.MatrizSubtracao(m_Y, m_clt.MatrizMult(m_X, beta_pols));

            double num_test_stat = 0.0;
            double den_test_stat = 0.0;
            double aux_den_test_stat = 0.0;

            for (int i = 0; i < m_num_unidades; i++)
            {
                aux_den_test_stat = 0.0;
                for (int t = 0; t < m_num_periodos - 1; t++)
                {
                    for (int s = t + 1; s < m_num_periodos; s++)
                    {
                        num_test_stat += painel_elemento(e_pols, i, t, 0) * painel_elemento(e_pols, i, s, 0);
                        aux_den_test_stat += painel_elemento(e_pols, i, t, 0) * painel_elemento(e_pols, i, s, 0);
                    }
                }
                den_test_stat += Math.Pow(aux_den_test_stat, 2.0);
            }
            den_test_stat = Math.Pow(den_test_stat, 0.5);
            
            Normal normal = new Normal();

            re.sigma2_c_test_stat = num_test_stat / den_test_stat;
            re.sigma2_c_pvalue = 2.0 * (1.0 - normal.CumulativeDistribution(Math.Abs(re.sigma2_c_test_stat)));

            #endregion

            #region testando a significância das dummies de tendencia e temporais

            this.TestaDummiesETendenciasTemporais(re.beta_RE, re.beta_RE_covmat, out re.Wtest, out re.Wpvalor);

            #endregion
        }

        #endregion

        #region checando se existe variabilidade temporal em regressores para efeitos fixos em pelo menos uma unidade observacional

        public void ChecaVariabilidadeTemporalEfeitosFixos(double[,] X, int N, int T)
        {
            if (m_tipo_estimacao_painel == TipoEstimacaoDadosPainel.EfeitosFixos || m_tipo_estimacao_painel == TipoEstimacaoDadosPainel.PrimeirasDiferencas)
            {
                bool checagem_ok = false;
                double[,] X_foco = new double[0, 0];
                clsUtilTools clt = new clsUtilTools();
                double[,] var_cols = new double[0, 0];
                for (int i = 0; i < N; i++)
                {
                    clt.GeraSubRows(ref X_foco, X, i * T, (i + 1) * T - 1);
                    var_cols = clt.VarianciasColumnMatrix(X_foco);
                    for (int k = 0; k < var_cols.GetLength(1); k++)
                    {
                        if (var_cols[k, 0] > 0.0)
                        {
                            checagem_ok = true;
                            break; 
                        }
                    }
                    if (checagem_ok) break;
                }

                if (!checagem_ok) throw new Exception("Não é possível estimar o modelo de painel pois não há variação temporal em pelo menos um dos regressores."); 
            }
        }

        #endregion

        #region estimação via efeitos fixos painel balanceado

        protected void EstimacaoEfeitosFixosPainelBalanceado()
        {
            //double[,] beta_fe, beta_fe_covmat, y_hat_fe, residuos_fe, c_hat;
            //double sigma2_hat, R2, Fstat, Fpvalue, SSE, SST, SSR, R2_ajustado;
            //double SSE_modelo_original, SST_modelo_original, SSR_modelo_original, R2_modelo_original, R2_ajust_modelo_original,
            //        loglik_modelo_original, AIC_modelo_original, BIC_modelo_original, Fstat_modelo_original, Fpvalue_modelo_original, sigma2_hat_modelo_original, variancia_efeitos_fixos;
            //double Wtest, Wpvalor;

            // Creates a struct and put the data in to run the model.
            FixedEffectsBallancedPanel fe = new FixedEffectsBallancedPanel();
            EstimarFixedEffectsPainelBalanceado(out fe);

            m_beta_hat = fe.beta_FE;
            m_beta_hat_cov = fe.beta_FE_covmat;
            this.GeraSignificanciaCoeficientes(false);

            #region gerando o output para resultado das estimacoes

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação para Dados em Painel (Efeitos Fixos) \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + m_X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + m_X.GetLength(1) + "\n";
            out_text += "Variância dos resíduos: " + m_clt.Double2Texto(fe.sigma2_hat_modelo_original, 6) + "\n";
            out_text += "Variância dos efeitos fixos: " + m_clt.Double2Texto(fe.variancia_efeitos_fixos, 6) + "\n";
            if (m_usa_general_FGLS_analysis)
            {
                out_text += "Estimação utilizando FGLS (feasible generalized least squares) \n";
            }
            if (m_usa_robust_cov_matrix)
            {
                out_text += "Estimação robusta para a matriz de covariâncias \n";
            }
            out_text += "\n";

            out_text += GeraTabelaEstimacoes(m_clt.Concate(m_clt.Concate(VariaveisIndependentes, m_nomes_dummies_temporais), m_nomes_tendencia), m_beta_hat, m_beta_stderror, m_beta_tstat, m_beta_pvalor, m_usa_intercepto);
            out_text += "\n";

            out_text += "======== Estatísticas de ajuste considerando-se o modelo com dummies para unidades observacionais ================= \n";
            out_text += "======== Observação: a estatística F não é válida para o modelo estimado via FGLS. O p-valor para a estatística F \n";
            out_text += "======== vale assintoticamente para o número de períodos tendendo ao infinito. \n\n";

            out_text += "Soma total dos quadrados: " + m_clt.Double2Texto(fe.SST_modelo_original, 6) + "\n";
            out_text += "Soma dos quadrados da regressão: " + m_clt.Double2Texto(fe.SSR_modelo_original, 6) + "\n";
            out_text += "Soma dos quadrados dos resíduos: " + m_clt.Double2Texto(fe.SSE_modelo_original, 6) + "\n";
            out_text += "R2 (coeficiente de determinação): " + m_clt.Double2Texto(fe.R2_modelo_original, 6) + "\n";
            out_text += "R2-ajustado (coeficiente de determinação ajustado): " + m_clt.Double2Texto(fe.R2_ajust_modelo_original, 6) + "\n\n";
            out_text += "F (estatistica teste): " + m_clt.Double2Texto(fe.Fstat_modelo_original, 6) + "\n";
            out_text += "F (p-valor): " + m_clt.Double2Texto(fe.Fpvalue_modelo_original, 6) + "\n";

            if (m_numero_dummies_temporais > 0)
            {
                out_text += "\n";
                out_text += "Teste de Wald para significância das dummies temporais (para os diferentes períodos de tempo): \n";
                out_text += "Estatística teste: " + m_clt.Double2Texto(fe.Wtest, 6) + "\n";
                out_text += "P-valor: " + m_clt.Double2Texto(fe.Wpvalor, 6) + "\n";
                out_text += "Número de restrições: " + m_numero_dummies_temporais + "\n";
            }

            if (m_numero_tendencias_temporais > 0)
            {
                out_text += "\n";
                out_text += "Teste de Wald para significância das tendências temporais escolhidas: \n";
                out_text += "Estatística teste: " + m_clt.Double2Texto(fe.Wtest, 6) + "\n";
                out_text += "P-valor: " + m_clt.Double2Texto(fe.Wpvalor, 6) + "\n";
                out_text += "Número de restrições: " + m_numero_tendencias_temporais + "\n";
            }

            out_text += "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";
                out_text += this.GeraTabelaCovMatrix(m_beta_hat_cov, m_clt.Concate(m_clt.Concate(VariaveisIndependentes, m_nomes_dummies_temporais), m_nomes_tendencia), m_usa_intercepto);
                out_text += "\n";
            }

            this.m_output_text = out_text;

            #endregion

            #region adicionando variveis base de dados

            for (int i = 0; i < m_Y.GetLength(0); i++)
            {
                m_ordem_original[i, 0] = 1 + Convert.ToInt32(m_ordem_original[i,0]);
            }

            object[,] novas_variaveis = m_clt.Concateh(m_ordem_original, m_unidades);
            novas_variaveis = m_clt.Concateh(novas_variaveis, m_periodos);
            novas_variaveis = m_clt.Concateh(novas_variaveis, m_clt.Concateh(m_Y, m_clt.Concateh(m_Y_hat, m_residuos)));
            novas_variaveis = m_clt.Concateh(novas_variaveis, m_efeitos_unidades);

            m_clt.SortByColumn(ref novas_variaveis, novas_variaveis, 0);

            string[] nomes_variaveis = new string[7];
            nomes_variaveis[0] = "Observacao_";
            nomes_variaveis[1] = "Unidade_observacional_";
            nomes_variaveis[2] = "Periodo_de_tempo_";
            nomes_variaveis[3] = "Y_observado_";
            nomes_variaveis[4] = "Y_predito_";
            nomes_variaveis[5] = "Residuo_";
            nomes_variaveis[6] = "EfeitosFixos_";

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação para Dados em Painel (Efeitos Fixos) \n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += "Observação: as variáveis apresentadas na tabela a seguir correspondem \n";
            m_output_variaveis_geradas += "à variável resposta original na ordem em que elas aparecem na tabela de dados. \n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(novas_variaveis, nomes_variaveis);

            m_output_variaveis_geradas += "\n";
            
            //------------- adicionando à tabela de dados no datagrid

            string[] nomes_vars_dt = new string[nomes_variaveis.GetLength(0) - 3];
            for (int i = 0; i < nomes_vars_dt.GetLength(0); i++) nomes_vars_dt[i] = nomes_variaveis[i + 3];

            novas_variaveis = m_clt.SubColumnsArrayObject(novas_variaveis, 3, novas_variaveis.GetLength(1) - 1);

            AdicionaNovasVariaveisToDataTable(novas_variaveis, nomes_vars_dt);

            #endregion

            #region gerando o output para as dummies de efeitos fixos

            int max_length1 = m_variavel_unidade_observacional.Length;
            string[] su = new string[this.m_lista_unidades.GetLength(0)];
            for (int i = 0; i < m_lista_unidades.GetLength(0); i++)
            {
                su[i] = m_clt.Double2Texto(m_lista_unidades[i, 0]);
                if (su[i].Length > max_length1) max_length1 = su[i].Length;
            }

            int max_length2 = "Efeitos fixos".Length;
            string[] sc = new string[fe.c_hat.GetLength(0)];
            for (int i = 0; i < sc.GetLength(0); i++)
            {
                sc[i] = m_clt.Double2Texto(fe.c_hat[i, 0], 6);
                if (sc[i].Length > max_length2) max_length2 = sc[i].Length;
            }
            max_length2 += 6;
            
            StringBuilder sb = new StringBuilder("============================================================================================================================\n\n");        

            sb.Append("Estimação para Dados em Painel (Efeitos Fixos) \n\n");
            sb.Append("Data: " + System.DateTime.Now.ToLongDateString() + "\n");
            sb.Append("Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n");

            sb.Append(PreencheEspacos(3) + m_variavel_unidade_observacional + PreencheEspacos(max_length1 - m_variavel_unidade_observacional.Length));
            sb.Append("\t" + PreencheEspacos(max_length2 - "Efeitos fixos".Length) + "Efeitos fixos");
            sb.Append("\n\n");

            for (int i = 0; i < sc.GetLength(0); i++)
            {
                sb.Append(PreencheEspacos(3) + su[i] + PreencheEspacos(max_length1 - su[i].Length));
                sb.Append("\t" + PreencheEspacos(max_length2 - sc[i].Length) + sc[i]);
                sb.Append("\n");
            }

            m_text_efeitos_fixos = sb.ToString();

            #endregion
        }

        private void EstimarFixedEffectsPainelBalanceado(out FixedEffectsBallancedPanel fe)
        {
            double[,] y = new double[m_Y.GetLength(0), 1];
            double[,] X = new double[m_X.GetLength(0), m_X.GetLength(1)];

            double[,] X_i;
            double[,] y_i;
            double[,] mean_X_i, mean_Y_i;

            for (int i = 0; i < m_num_unidades; i++)
            {
                X_i = painel_unidade(m_X, i);
                y_i = painel_unidade(m_Y, i);

                mean_X_i = m_clt.Meanc(X_i);
                mean_Y_i = m_clt.Meanc(y_i);

                for (int t = 0; t < m_num_periodos; t++)
                {
                    y[i * m_num_periodos + t, 0] = m_Y[i * m_num_periodos + t, 0] - mean_Y_i[0, 0];
                    for (int j = 0; j < m_X.GetLength(1); j++)
                    {
                        X[i * m_num_periodos + t, j] = m_X[i * m_num_periodos + t, j] - mean_X_i[0, j];
                    }
                }
            }

            double[,] XtX = m_clt.MatrizMult(m_clt.MatrizTransp(X), X);
            double[,] XtY = m_clt.MatrizMult(m_clt.MatrizTransp(X), y);

            double[,] inv_XtX = m_clt.MatrizInversa(XtX);
            fe.beta_FE = m_clt.MatrizMult(inv_XtX, XtY);
            double[,] u_FE = m_clt.MatrizSubtracao(y, m_clt.MatrizMult(X, fe.beta_FE));

            double sigma2 = m_clt.MatrizMult(m_clt.MatrizTransp(u_FE), u_FE)[0, 0]
                            / ((double)((m_num_periodos - 1) * m_num_unidades - X.GetLength(1)));

            double[,] ui;
            double[,] m3;
            double[,] Xtui;

            //-------------------------- variáveis de output para o modelo com variáveis transformadas (demeaned) - não vale para FGLS ----------------------------------//

            double[,] yhat_FE = m_clt.MatrizMult(X, fe.beta_FE);
            fe.SST = (m_clt.MatrizMultMtranspM(m_clt.MatrizSubtracao(y, m_clt.Mean(y))))[0, 0];
            fe.SSR = (m_clt.MatrizMultMtranspM(m_clt.MatrizSubtracao(yhat_FE, m_clt.Mean(yhat_FE))))[0, 0];
            fe.SSE = (m_clt.MatrizMultMtranspM(u_FE))[0, 0];

            fe.R2 = fe.SSR / fe.SST;
            fe.R2_ajustado = 1.0 - (1.0 - fe.R2) * ((double)(m_num_unidades * m_num_periodos)) / ((double)(m_num_unidades * m_num_periodos) - (double)m_X.GetLength(1));

            fe.Fstat = ((fe.SST - fe.SSE) / fe.SSE) * ((double)(m_num_periodos * m_num_unidades - m_X.GetLength(1))) / ((double)(m_X.GetLength(1)));
            FisherSnedecor fdist = new FisherSnedecor(X.GetLength(1), X.GetLength(0) - X.GetLength(1));
            fe.Fpvalue = 1.0 - fdist.CumulativeDistribution(fe.Fstat);

            //------------------------ estimação da matriz de variância-covariância -----------------------//

            if (!m_usa_general_FGLS_analysis)
            {
                if (!m_usa_robust_cov_matrix)
                {
                    // Matriz de covariância sem correção (matriz não robusta), quando não usamos FGLS
                    // Estimador em Woodridge (2002), página 271

                    fe.beta_FE_covmat = m_clt.MatrizMult(sigma2, inv_XtX);
                }
                else
                {
                    // Matriz de covariância robusta, quando não usamos FGLS
                    // Estimador em Woodridge (2002), página 275

                    m3 = new double[X.GetLength(1), X.GetLength(1)];

                    for (int i = 0; i < m_num_unidades; i++)
                    {
                        X_i = painel_unidade(X, i);
                        ui = painel_unidade(u_FE, i);

                        Xtui = m_clt.MatrizMult(m_clt.MatrizTransp(X_i), ui);
                        m3 = m_clt.MatrizSoma(m3, m_clt.MatrizMult(Xtui, m_clt.MatrizTransp(Xtui)));
                    }

                    fe.beta_FE_covmat = m_clt.MatrizMult(m_clt.MatrizMult(inv_XtX, m3), inv_XtX);
                }
            }
            else
            {
                // Estimação do modelo de efeitos fixos (com within estimator) via FGLS
                // Expressão em Woodridge (2002), página 277

                double[,] Omega = new double[m_num_periodos - 1, m_num_periodos - 1];
                for (int i = 0; i < m_num_unidades; i++)
                {
                    ui = painel_unidade(u_FE, i);
                    ui = m_clt.DeleteRow(ui, ui.GetLength(0) - 1);
                    Omega = m_clt.MatrizSoma(Omega, m_clt.MatrizMult(ui, m_clt.MatrizTransp(ui)));
                }
                Omega = m_clt.MatrizMult(Omega, 1.0 / ((double)m_num_unidades));
                double[,] inv_Omega = m_clt.MatrizInversa(Omega);

                double[,] m1 = new double[X.GetLength(1), X.GetLength(1)];
                double[,] m2 = new double[X.GetLength(1), 1];
                double[,] XtinvOmega;

                for (int i = 0; i < m_num_unidades; i++)
                {
                    X_i = painel_unidade(X, i);
                    y_i = painel_unidade(Y, i);

                    X_i = m_clt.DeleteRow(X_i, X_i.GetLength(0) - 1);
                    y_i = m_clt.DeleteRow(y_i, y_i.GetLength(0) - 1);

                    XtinvOmega = m_clt.MatrizMult(m_clt.MatrizTransp(X_i), inv_Omega);

                    m1 = m_clt.MatrizSoma(m1, m_clt.MatrizMult(XtinvOmega, X_i));
                    m2 = m_clt.MatrizSoma(m2, m_clt.MatrizMult(XtinvOmega, y_i));
                }

                double[,] inv_m1 = m_clt.MatrizInversa(m1);

                fe.beta_FE = m_clt.MatrizMult(inv_m1, m2);
                u_FE = m_clt.MatrizSubtracao(y, m_clt.MatrizMult(X, fe.beta_FE));
                sigma2 = m_clt.MatrizMult(m_clt.MatrizTransp(u_FE), u_FE)[0, 0]
                                / ((double)((m_num_periodos - 1) * m_num_unidades - (double)X.GetLength(1)));

                if (!m_usa_robust_cov_matrix)
                {
                    // Matriz de covariância (não-robusta) para estimação do modelo de efeitos fixos (com within estimator) via FGLS
                    // Expressão em Woodridge (2002), página 277

                    fe.beta_FE_covmat = inv_m1;
                }
                else
                {
                    // Matriz de covariância (robusta) para estimação do modelo de efeitos fixos (com within estimator) via FGLS
                    // ---- checar no Livro do Cameron e Trivedi

                    m3 = new double[X.GetLength(1), X.GetLength(1)];
                    for (int i = 0; i < m_num_unidades; i++)
                    {
                        X_i = painel_unidade(X, i);
                        ui = painel_unidade(u_FE, i);

                        Xtui = m_clt.MatrizMult(m_clt.MatrizTransp(X_i), ui);
                        m3 = m_clt.MatrizSoma(m3, m_clt.MatrizMult(Xtui, m_clt.MatrizTransp(Xtui)));
                    }

                    fe.beta_FE_covmat = m_clt.MatrizMult(m_clt.MatrizMult(inv_m1, m3), inv_m1);
                }
            }

            //------------------------ estimando os efeitos fixos ----------------------------//

            fe.c_hat = new double[m_num_unidades, 1];
            for (int i = 0; i < m_num_unidades; i++)
            {
                X_i = painel_unidade(m_X, i);
                y_i = painel_unidade(m_Y, i);

                mean_X_i = m_clt.Meanc(X_i);
                mean_Y_i = m_clt.Meanc(y_i);

                fe.c_hat[i, 0] = mean_Y_i[0, 0] - m_clt.MatrizMult(mean_X_i, fe.beta_FE)[0, 0];
            }

            fe.variancia_efeitos_fixos = (m_clt.Varianciaca(fe.c_hat))[0, 0];

            //-------------------------- variáveis de output para o modelo original (não trasformado) ----------------------------------//

            m_efeitos_unidades = this.OrganizaEfeitosIdiossincraticosUnidades(fe.c_hat);
            m_Y_hat = m_clt.MatrizSoma(m_efeitos_unidades, m_clt.MatrizMult(m_X, fe.beta_FE));
            m_residuos = m_clt.MatrizSubtracao(m_Y_hat, m_Y);

            fe.y_hat_FE = m_clt.MatrizMult(X, fe.beta_FE);
            fe.resduos_FE = m_clt.MatrizSubtracao(y, fe.y_hat_FE);

            fe.sigma2_hat = m_clt.MatrizMult(m_clt.MatrizTransp(fe.resduos_FE), fe.resduos_FE)[0, 0] / ((double)(m_num_unidades * (m_num_periodos - 1) - X.GetLength(1)));

            //------ SST, SSR, SSE, R2 e R2-ajustado com base nos resíduos do modelo original (com dummies de unidades observacionais)

            fe.SST_modelo_original = (m_clt.MatrizMultMtranspM(m_clt.MatrizSubtracao(m_Y, m_clt.Mean(m_Y))))[0, 0];
            fe.SSR_modelo_original = (m_clt.MatrizMultMtranspM(m_clt.MatrizSubtracao(m_Y_hat, m_clt.Mean(m_Y_hat))))[0, 0];
            fe.SSE_modelo_original = (m_clt.MatrizMultMtranspM(m_clt.MatrizSubtracao(m_residuos, m_clt.Mean(m_residuos))))[0, 0];
            fe.R2_modelo_original = fe.SSR_modelo_original / fe.SST_modelo_original;
            fe.R2_ajust_modelo_original = 1.0 - (1.0 - fe.R2_modelo_original) * ((double)(m_num_unidades * m_num_periodos) - 1.0) / ((double)(m_num_unidades * m_num_periodos) - (double)(m_X.GetLength(1) + m_num_unidades));
            fe.sigma2_hat_modelo_original = fe.SSE_modelo_original / ((double)(m_num_periodos * m_num_unidades - m_X.GetLength(1) - m_num_unidades + 1));

            //------- loglik com base no modelo não transformado (modelo original)

            fe.loglik_modelo_original = -((double)(m_num_periodos * m_num_unidades) / 2.0) * (1.0 + Math.Log(2.0 * Math.PI) + Math.Log(fe.SSE_modelo_original / ((double)(m_num_unidades * m_num_periodos))));
            fe.AIC_modelo_original = (-2.0 * fe.loglik_modelo_original + 2.0 * (double)(m_X.GetLength(1)) + m_num_unidades) / ((double)(m_num_periodos * m_num_unidades));
            fe.BIC_modelo_original = (-2.0 * fe.loglik_modelo_original + Math.Log((double)(m_num_periodos * m_num_unidades)) * (double)(m_X.GetLength(1) + m_num_unidades)) / ((double)(m_num_periodos * m_num_unidades));

            //-------- estatística F com base no modelo não transformado (modelo original)

            fe.Fstat_modelo_original = ((fe.SST_modelo_original - fe.SSE_modelo_original) / fe.SSE_modelo_original) * ((double)(m_num_periodos * m_num_unidades - m_X.GetLength(1) - m_num_unidades)) / ((double)(m_X.GetLength(1) + m_num_unidades - 1));
            FisherSnedecor fdist_modelo_original = new FisherSnedecor(X.GetLength(1) + m_num_unidades - 1, X.GetLength(0) - X.GetLength(1) - m_num_unidades);
            fe.Fpvalue_modelo_original = 1.0 - fdist_modelo_original.CumulativeDistribution(fe.Fstat_modelo_original);

            #region testando a significância das dummies de tendencia e temporais

            this.TestaDummiesETendenciasTemporais(fe.beta_FE, fe.beta_FE_covmat, out fe.Wtest, out fe.Wpvalor);

            #endregion
        }

        #endregion

        #region estimação via primeiras diferenças
        
        #region variáveis para estimação em primeiras diferenças

        protected double[,] m_DeltaX = new double[0, 0];
        protected double[,] m_DeltaY = new double[0, 0];
        protected double[,] m_DeltaRes = new double[0, 0];

        protected double[,] DeltaRes_i(int i)
        {
            double[,] r = new double[m_num_periodos - 1, m_DeltaRes.GetLength(1)];
            for (int t = 0; t < m_num_periodos - 1; t++)
            {
                for (int j = 0; j < m_DeltaRes.GetLength(1); j++)
                {
                    r[i, j] = m_DeltaRes[i * (m_num_periodos - 1) + t, j];
                }
            }
            return r;
        }

        protected double[,] DeltaX_i(int i)
        {
            double[,] r = new double[m_num_periodos - 1, m_DeltaX.GetLength(1)];
            for (int t = 0; t < m_num_periodos-1; t++)
            {
                for (int j = 0; j < m_DeltaX.GetLength(1); j++)
                {
                    r[i, j] = m_DeltaX[i * (m_num_periodos - 1) + t, j];
                }
            }
            return r;
        }

        protected double[,] DeltaY_i(int i)
        {
            double[,] r = new double[m_num_periodos - 1, m_DeltaY.GetLength(1)];
            for (int t = 0; t < m_num_periodos - 1; t++)
            {
                for (int j = 0; j < m_DeltaY.GetLength(1); j++)
                {
                    r[i, j] = m_DeltaY[i * (m_num_periodos - 1) + t, j];
                }
            }
            return r;
        }

        #endregion

        protected void EstimacaoPrimeirasDiferencas()
        {
            FirstDifference fd = new FirstDifference();

            EstimarPrimeirasDiferencas(out fd);

            m_beta_hat = fd.beta_FD;
            m_beta_hat_cov = fd.beta_FD_covmat;
            this.GeraSignificanciaCoeficientes(false);

            #region gerando o output para resultado das estimacoes

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação para Dados em Painel (Primeiras Diferenças) \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + m_X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + m_X.GetLength(1) + "\n";
            out_text += "Variância dos resíduos (modelo estimado em primeiras diferenças): " + m_clt.Double2Texto(fd.sigma2_hat, 6) + "\n";
            out_text += "Variância dos resíduos (equação original): " + m_clt.Double2Texto(fd.sigma2_hat_modelo_original, 6) + "\n";
            out_text += "Variância dos efeitos fixos: " + m_clt.Double2Texto(fd.variancia_efeitos_fixos, 6) + "\n";
            if (m_usa_general_FGLS_analysis)
            {
                out_text += "Estimação utilizando FGLS (feasible generalized least squares) \n";
            }
            if (m_usa_robust_cov_matrix)
            {
                out_text += "Estimação robusta para a matriz de covariâncias \n";
            }
            out_text += "\n";

            out_text += GeraTabelaEstimacoes(m_clt.Concate(m_clt.Concate(VariaveisIndependentes, m_nomes_dummies_temporais), m_nomes_tendencia), m_beta_hat, m_beta_stderror, m_beta_tstat, m_beta_pvalor, m_usa_intercepto);
            out_text += "\n";

            out_text += "R2 (coeficiente de determinação): " + m_clt.Double2Texto(fd.R2, 6) + "\n";
            out_text += "F (estatistica teste): " + m_clt.Double2Texto(fd.Fstat, 6) + "\n";
            out_text += "F (p-valor): " + m_clt.Double2Texto(fd.Fpvalue, 6) + "\n";
            out_text += "Chi-quadrado (estatistica teste): " + m_clt.Double2Texto(fd.X2stat, 6) + "\n";
            out_text += "Chi-quadrado (p-valor): " + m_clt.Double2Texto(fd.X2pvalue, 6) + "\n";
            
            if (m_numero_dummies_temporais > 0)
            {
                out_text += "\n";
                out_text += "Teste de Wald para significância das dummies temporais (para os diferentes períodos de tempo): \n";
                out_text += "Estatística teste: " + m_clt.Double2Texto(fd.Wtest, 6) + "\n";
                out_text += "P-valor: " + m_clt.Double2Texto(fd.Wpvalor, 6) + "\n";
                out_text += "Número de restrições: " + m_numero_dummies_temporais + "\n";
            }

            if (m_numero_tendencias_temporais > 0)
            {
                out_text += "\n";
                out_text += "Teste de Wald para significância das tendências temporais escolhidas: \n";
                out_text += "Estatística teste: " + m_clt.Double2Texto(fd.Wtest, 6) + "\n";
                out_text += "P-valor: " + m_clt.Double2Texto(fd.Wpvalor, 6) + "\n";
                out_text += "Número de restrições: " + m_numero_tendencias_temporais + "\n";
            }

            out_text += "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";
                out_text += this.GeraTabelaCovMatrix(m_beta_hat_cov, m_clt.Concate(m_clt.Concate(VariaveisIndependentes, m_nomes_dummies_temporais), m_nomes_tendencia), m_usa_intercepto);
                out_text += "\n";
            }

            this.m_output_text = out_text;

            #endregion

            #region adicionando variveis base de dados

            for (int i = 0; i < m_Y.GetLength(0); i++)
            {
                m_ordem_original[i, 0] = 1 + Convert.ToInt32(m_ordem_original[i, 0]);
            }

            object[,] novas_variaveis = m_clt.Concateh(m_ordem_original, m_unidades);
            novas_variaveis = m_clt.Concateh(novas_variaveis, m_periodos);
            novas_variaveis = m_clt.Concateh(novas_variaveis, m_clt.Concateh(m_Y, m_clt.Concateh(m_Y_hat, m_residuos)));
            novas_variaveis = m_clt.Concateh(novas_variaveis, m_efeitos_unidades); 

            m_clt.SortByColumn(ref novas_variaveis, novas_variaveis, 0);

            string[] nomes_variaveis = new string[7];
            nomes_variaveis[0] = "Observacao_";
            nomes_variaveis[1] = "Unidade_observacional_";
            nomes_variaveis[2] = "Periodo_de_tempo_";
            nomes_variaveis[3] = "Y_observado_";
            nomes_variaveis[4] = "Y_predito_";
            nomes_variaveis[5] = "Residuo_";
            nomes_variaveis[6] = "EfeitosFixos_";

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação para Dados em Painel (Estimação em Primeiras Diferenças) \n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += "Observação: as variáveis apresentadas na tabela a seguir correspondem \n";
            m_output_variaveis_geradas += "à variável resposta original na ordem em que elas aparecem na tabela de dados. \n"; 
            m_output_variaveis_geradas += "As estimações foram feitas com as variáveis em primeiras diferenças. \n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(novas_variaveis, nomes_variaveis);

            m_output_variaveis_geradas += "\n";

            //------------- adicionando à tabela de dados no datagrid

            string[] nomes_vars_dt = new string[nomes_variaveis.GetLength(0) - 3];
            for (int i = 0; i < nomes_vars_dt.GetLength(0); i++) nomes_vars_dt[i] = nomes_variaveis[i + 3];

            novas_variaveis = m_clt.SubColumnsArrayObject(novas_variaveis, 3, novas_variaveis.GetLength(1) - 1);

            AdicionaNovasVariaveisToDataTable(novas_variaveis, nomes_vars_dt);

            #endregion
            
            #region gerando o output para as dummies de efeitos fixos

            int max_length1 = m_variavel_unidade_observacional.Length;
            string[] su = new string[this.m_lista_unidades.GetLength(0)];
            for (int i = 0; i < m_lista_unidades.GetLength(0); i++)
            {
                su[i] = m_clt.Double2Texto(m_lista_unidades[i, 0]);
                if (su[i].Length > max_length1) max_length1 = su[i].Length;
            }

            int max_length2 = "Efeitos fixos".Length;
            string[] sc = new string[fd.c_hat.GetLength(0)];
            for (int i = 0; i < sc.GetLength(0); i++)
            {
                sc[i] = m_clt.Double2Texto(fd.c_hat[i, 0], 6);
                if (sc[i].Length > max_length2) max_length2 = sc[i].Length;
            }
            max_length2 += 6;

            StringBuilder sb = new StringBuilder("============================================================================================================================\n\n");

            sb.Append("Estimação para Dados em Painel (Estimação via Primeiras Diferenças) \n\n");
            sb.Append("Data: " + System.DateTime.Now.ToLongDateString() + "\n");
            sb.Append("Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n");

            sb.Append(PreencheEspacos(3) + m_variavel_unidade_observacional + PreencheEspacos(max_length1 - m_variavel_unidade_observacional.Length));
            sb.Append("\t" + PreencheEspacos(max_length2 - "Efeitos fixos".Length) + "Efeitos fixos");
            sb.Append("\n\n");

            for (int i = 0; i < sc.GetLength(0); i++)
            {
                sb.Append(PreencheEspacos(3) + su[i] + PreencheEspacos(max_length1 - su[i].Length));
                sb.Append("\t" + PreencheEspacos(max_length2 - sc[i].Length) + sc[i]);
                sb.Append("\n");
            }

            m_text_efeitos_fixos = sb.ToString();

            #endregion
        }

        private void EstimarPrimeirasDiferencas(out FirstDifference fd)
        {
            m_DeltaX = new double[(m_num_periodos - 1) * m_num_unidades, m_X.GetLength(1)];
            m_DeltaY = new double[(m_num_periodos - 1) * m_num_unidades, 1];

            double[,] Xi, Yi, Du_i, DX_i, Dy_i, Xtui;

            // Gerando as primeiras diferenças das variáveis X e das variáveis Y
            // Estimador em Woodridge (2002), pagina 279

            for (int i = 0; i < m_num_unidades; i++)
            {
                Xi = painel_unidade(m_X, i);
                Yi = painel_unidade(m_Y, i);

                for (int t = 0; t < m_num_periodos-1; t++)
                {
                    m_DeltaY[i * (m_num_periodos - 1) + t, 0] = Yi[t + 1, 0] - Yi[t, 0];
                    for (int j = 0; j < m_X.GetLength(1); j++)
                    {
                        m_DeltaX[i * (m_num_periodos - 1) + t, j] = Xi[t + 1, j] - Xi[t, j];
                    }
                }
            }

            fd.delta_x = m_DeltaX;
            fd.delta_y = m_DeltaY;

            double[,] XtX = m_clt.MatrizMult(m_clt.MatrizTransp(m_DeltaX), m_DeltaX);
            double[,] XtY = m_clt.MatrizMult(m_clt.MatrizTransp(m_DeltaX), m_DeltaY);
            double[,] inv_XtX = m_clt.MatrizInversa(XtX);
            
            // Estimação via OLS nas primeiras diferenças
            // Estimador em Woodridge (2002), pagina 279

            fd.beta_FD = m_clt.MatrizMult(inv_XtX, XtY);
            double[,] e_FD = m_clt.MatrizSubtracao(m_DeltaY, m_clt.MatrizMult(m_DeltaX, fd.beta_FD));
            double sigma2 = 0.0;
            m_DeltaRes = e_FD;

            double[,] m1, m2, Xtu, m3;

            if (!m_usa_general_FGLS_analysis)
            {
                if (!m_usa_robust_cov_matrix)
                {
                    // Matriz de variância-covariância (não-robusta) para o estimador de primeiras diferenças
                    // Expressão em Woodridge (2002), página 281
                    
                    sigma2 = m_clt.MatrizMult(m_clt.MatrizTransp(e_FD), e_FD)[0, 0] / ((double)(e_FD.GetLength(0) - m_DeltaX.GetLength(1)));
                    fd.beta_FD_covmat = m_clt.MatrizMult(sigma2, inv_XtX);
                }
                else
                {
                    // Matriz de variância-covariância (robusta) para o estimador de primeiras diferenças
                    // Expressão em Woodridge (2002), página 282

                    m1 = new double[m_DeltaX.GetLength(1), m_DeltaX.GetLength(1)];
                    for (int i = 0; i < m_num_unidades; i++)
                    {
                        Du_i = DeltaRes_i(i);
                        DX_i = DeltaX_i(i);
                        Xtu = m_clt.MatrizMult(m_clt.MatrizTransp(DX_i), Du_i);

                        m1 = m_clt.MatrizSoma(m1, m_clt.MatrizMult(Xtu, m_clt.MatrizTransp(Xtu)));
                    }

                    fd.beta_FD_covmat = m_clt.MatrizMult(m_clt.MatrizMult(inv_XtX, m1), inv_XtX);
                }
            }
            else
            {
                // Estimação em primeiras diferenças, utilizando FGLS
                // Comentários em Woodridge (2002), página 283, utilizando expressões para o FE, na página 277 (sem dropar a última observação das diferenças)

                double[,] Omega = new double[m_num_periodos - 1, m_num_periodos - 1];
                for (int i = 0; i < m_num_unidades; i++)
                {
                    Du_i = DeltaRes_i(i);
                    Omega = m_clt.MatrizSoma(Omega, m_clt.MatrizMult(Du_i, m_clt.MatrizTransp(Du_i)));
                }
                Omega = m_clt.MatrizMult(Omega, 1.0 / ((double)m_num_unidades));
                double[,] inv_Omega = m_clt.MatrizInversa(Omega);

                m1 = new double[m_DeltaX.GetLength(1), m_DeltaX.GetLength(1)];
                m2 = new double[m_DeltaX.GetLength(1), 1];
                double[,] XtinvOmega;

                for (int i = 0; i < m_num_unidades; i++)
                {
                    Du_i = DeltaRes_i(i);
                    DX_i = DeltaX_i(i);
                    Dy_i = DeltaY_i(i);

                    XtinvOmega = m_clt.MatrizMult(m_clt.MatrizTransp(DX_i), inv_Omega);

                    m1 = m_clt.MatrizSoma(m1, m_clt.MatrizMult(XtinvOmega, DX_i));
                    m2 = m_clt.MatrizSoma(m2, m_clt.MatrizMult(XtinvOmega, Dy_i));
                }
                double[,] inv_m1 = m_clt.MatrizInversa(m1);

                fd.beta_FD = m_clt.MatrizMult(inv_m1, m2);
                e_FD = m_clt.MatrizSubtracao(m_DeltaY, m_clt.MatrizMult(m_DeltaX, fd.beta_FD));
                m_DeltaRes = e_FD;

                sigma2 = m_clt.MatrizMult(m_clt.MatrizTransp(e_FD), e_FD)[0, 0]
                                / ((double)(e_FD.GetLength(0) - X.GetLength(1)));

                if (!m_usa_robust_cov_matrix)
                {
                    // Matriz de variância-covariância (não-robusta) para o estimador FGLS para primeiras diferenças
                    // Adaptação das fórmulas em Woodridge (2002) para FE, página 277                    

                    fd.beta_FD_covmat = inv_m1;
                }
                else
                {
                    // Matriz de variância-covariância (robusta) para o estimador FGLS para primeiras diferenças
                    // Ainda falta implementar 

                    m3 = new double[m_DeltaX.GetLength(1), m_DeltaX.GetLength(1)];
                    for (int i = 0; i < m_num_unidades; i++)
                    {
                        Du_i = DeltaRes_i(i);
                        DX_i = DeltaX_i(i);

                        Xtui = m_clt.MatrizMult(m_clt.MatrizTransp(DX_i), Du_i);
                        m3 = m_clt.MatrizSoma(m3, m_clt.MatrizMult(Xtui, m_clt.MatrizTransp(Xtui)));
                    }

                    fd.beta_FD_covmat = m_clt.MatrizMult(m_clt.MatrizMult(inv_m1, m3), inv_m1);
                }
            }

            //------------------- estatísticas correspondentes ao modelo estimado em primeiras diferenças ----------------------//

            fd.X2pvalue = 0.0;
            fd.X2stat = 0.0;

            fd.delta_y_hat = m_clt.MatrizMult(m_DeltaX, fd.beta_FD);
            fd.residuos_delta = m_clt.MatrizSubtracao(m_DeltaY, fd.delta_y_hat);
            fd.sigma2_hat = m_clt.MatrizMult(m_clt.MatrizTransp(fd.residuos_delta),
                fd.residuos_delta)[0, 0] / ((double)(m_DeltaX.GetLength(0) - m_DeltaX.GetLength(1)));

            fd.SSR = (m_clt.MatrizMultMtranspM(m_clt.MatrizSubtracao(fd.delta_y_hat, m_clt.Mean(fd.delta_y_hat))))[0, 0];
            fd.SST = (m_clt.MatrizMultMtranspM(m_clt.MatrizSubtracao(fd.delta_y, m_clt.Mean(fd.delta_y))))[0, 0];
            fd.SSE = (m_clt.MatrizMultMtranspM(m_clt.MatrizSubtracao(fd.residuos_delta, m_clt.Mean(fd.residuos_delta))))[0, 0];

            fd.R2 = fd.SSR / fd.SST;
            fd.R2_ajustado = 1.0 - (1.0 - fd.R2) * ((double)(m_num_unidades * m_num_periodos) - 1.0) / ((double)(m_num_unidades * m_num_periodos) - (double)m_X.GetLength(1));

            //------ teste F (para o modelo em primeiras diferenças) 

            fd.Fstat = ((fd.SST - fd.SSE) / fd.SSE) * ((double)(m_num_periodos * m_num_unidades - m_X.GetLength(1))) / ((double)(m_X.GetLength(1) - 1));

            FisherSnedecor fdist = new FisherSnedecor(X.GetLength(1) - 1, X.GetLength(0) - X.GetLength(1));
            fd.Fpvalue = 1.0 - fdist.CumulativeDistribution(fd.Fstat);

            string primeiro_periodo = m_lista_periodos[0, 0].ToString();
            ArrayList list_periodos = new ArrayList();
            ArrayList list_unidades = new ArrayList();
            for (int i = 0; i < m_periodos.GetLength(0); i++)
            {
                if (primeiro_periodo != m_periodos[i, 0].ToString())
                {
                    list_periodos.Add(m_periodos[i, 0]);
                    list_unidades.Add(m_unidades[i, 0]);
                }
            }
            fd.delta_periodos = new double[list_periodos.Count, 1];
            fd.delta_unidades = new double[list_unidades.Count, 1];
            for (int i = 0; i < fd.delta_unidades.GetLength(0); i++)
            {
                fd.delta_periodos[i, 0] = (double)list_periodos[i];
                fd.delta_unidades[i, 0] = (double)list_unidades[i];
            }

            //------------------------ estimando os efeitos fixos ----------------------------//
            // Os cálculos do R2 e variáveis de output serão feitos considerando-se o modelo não em primieras diferenças (a partir dos coeficientes estimados)

            fd.c_hat = new double[m_num_unidades, 1];
            double[,] X_i = new double[0, 0];
            double[,] y_i = new double[0, 0];
            double[,] mean_X_i = new double[0, 0];
            double[,] mean_Y_i = new double[0, 0];

            for (int i = 0; i < m_num_unidades; i++)
            {
                X_i = painel_unidade(m_X, i);
                y_i = painel_unidade(m_Y, i);

                mean_X_i = m_clt.Meanc(X_i);
                mean_Y_i = m_clt.Meanc(y_i);

                fd.c_hat[i, 0] = mean_Y_i[0, 0] - m_clt.MatrizMult(mean_X_i, fd.beta_FD)[0, 0];
            }

            fd.variancia_efeitos_fixos = (m_clt.Varianciaca(fd.c_hat))[0, 0];

            //-------------------------- variáveis de output ----------------------------------//

            m_efeitos_unidades = this.OrganizaEfeitosIdiossincraticosUnidades(fd.c_hat);
            m_Y_hat = m_clt.MatrizSoma(m_efeitos_unidades, m_clt.MatrizMult(m_X, fd.beta_FD));
            m_residuos = m_clt.MatrizSubtracao(m_Y_hat, m_Y);

            // O sigma2_hat_modelo_original foi calculado com denominador para ser compatível com o mesmo parâmetro calculado no modelo de efeitos fixos
            fd.sigma2_hat_modelo_original = m_clt.MatrizMultMtranspM(m_clt.MatrizSubtracao(m_residuos, m_clt.Mean(m_residuos)))[0, 0] / ((double)(m_num_periodos * m_num_unidades - m_X.GetLength(1) - m_num_unidades + 1));

            fd.R2_modelo_original = 0.0;

            fd.loglik = 0.0;
            fd.AIC = 0.0;
            fd.BIC = 0.0;

            #region testando a significância das dummies de tendencia e temporais

            this.TestaDummiesETendenciasTemporais(fd.beta_FD, fd.beta_FD_covmat, out fd.Wtest, out fd.Wpvalor);

            #endregion
        }

        #endregion
    }
}

