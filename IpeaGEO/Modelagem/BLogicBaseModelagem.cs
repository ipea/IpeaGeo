using System;
using System.Data;
using System.Text;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public enum TiposMensagemRegressao : int
    {
        RegressaoBemSucedida = 1,
        Multicolinearidade = 2,
        BetasNull = 3
    }

    public class BLogicBaseModelagem : IpeaGeo.RegressoesEspaciais.clsModelosRegressaoEspacial
    {
        public BLogicBaseModelagem()
        {
        }

        #region Variáveis 

        protected TiposMensagemRegressao m_tipo_mensagem_regressao = TiposMensagemRegressao.RegressaoBemSucedida;
        public TiposMensagemRegressao TipoMensagemRegressao
        {
            get { return m_tipo_mensagem_regressao; }
            set { m_tipo_mensagem_regressao = value; }
        }

        protected string m_mensagem_regressao = "";
        public string MessagemRegressao
        {
            get { return m_mensagem_regressao; }
            set { m_mensagem_regressao = value; }
        }

        protected bool m_apresenta_covmatrix_beta_hat = false;
        public bool ApresentaCovMatrixBetaHat { set { this.m_apresenta_covmatrix_beta_hat = value; } }

        protected DataTable m_dt_tabela_dados = new DataTable();
        protected string[] m_variaveis_dependentes = new string[0];
        protected string[] m_variaveis_independentes = new string[0];
        protected string[] m_variaveis_instrumentais = new string[0];
        protected string[] m_variaveis_exogenas = new string[0];
        protected string[] m_variaveis_endogenas = new string[0];

        protected bool m_usa_intercepto = true;
        protected string m_output_text = "";
        protected string m_output_variaveis_geradas = "";
        protected bool m_adiciona_novas_variaveis = false;

        public bool AdicionaNovaVariaveis
        {
            get
            {
                return m_adiciona_novas_variaveis;
            }
            set
            {
                m_adiciona_novas_variaveis = value;
            }
        }

        protected string m_variavel_coord_X = "";
        protected string m_variavel_coord_Y = "";
        protected double m_cutoff_X = 0.0;
        protected double m_cutoff_Y = 0.0;

        public string VariavelCoordenadaX { set { this.m_variavel_coord_X = value; } }
        public string VariavelCoordenadaY { set { this.m_variavel_coord_Y = value; } }
        public double CutOffCoordenadaX { set { this.m_cutoff_X = value; } }
        public double CutOffCoordenadaY { set { this.m_cutoff_Y = value; } }

        protected IpeaGeo.RegressoesEspaciais.clsIpeaShape m_shape = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
        public IpeaGeo.RegressoesEspaciais.clsIpeaShape Shape
        {
            get { return this.m_shape.Clone(); }
            set { this.m_shape = value.Clone(); }
        }

        protected TipoMatrizVizinhanca m_tipo_matriz_vizinhanca = TipoMatrizVizinhanca.Normalizada;
        public TipoMatrizVizinhanca TipoMatrizVizinhanca
        {
            set { this.m_tipo_matriz_vizinhanca = value; }
            get { return this.m_tipo_matriz_vizinhanca; }
        }

        public string VariaveisGeradas
        {
            get { return this.m_output_variaveis_geradas; }
        }

        public string ResultadoEstimacao
        {
            get { return this.m_output_text; }
        }

        public bool IncluiIntercepto
        {
            get { return this.m_usa_intercepto; }
            set { this.m_usa_intercepto = value; }
        }

        public DataTable TabelaDados
        {
            set { this.m_dt_tabela_dados = value.Copy(); }
            get { return this.m_dt_tabela_dados; }
        }

        public string[] VariaveisDependentes
        {
            set { this.m_variaveis_dependentes = value; }
            get { return this.m_variaveis_dependentes; }
        }

        public string[] VariaveisIndependentes
        {
            set { this.m_variaveis_independentes = value; }
            get { return this.m_variaveis_independentes; }
        }

        public string[] VariaveisExogenas
        {
            get { return this.m_variaveis_exogenas; }
        }

        public string[] VariaveisInstrumentais
        {
            set { this.m_variaveis_instrumentais = value; }
            get { return this.m_variaveis_instrumentais; }
        }

        #endregion

        #region funções auxiliares

        public double GeraMaximaDistancia(DataTable dt, string variavel)
        {
            IpeaGeo.RegressoesEspaciais.clsUtilTools clt = new IpeaGeo.RegressoesEspaciais.clsUtilTools();
            double[,] w = clt.GetMatrizFromDataTable(dt, variavel);

            return Math.Abs(clt.Max(w) - clt.Min(w));
        }

        #endregion
        
        #region checando a presença de multicolinearidade

        public bool ChecarMulticolinearidade(out string mensagem)
        {
            bool res = false;
            mensagem = "";

            clsUtilTools clt = new clsUtilTools();

            double[,] X_original = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisIndependentes);
            double[,] X = clt.ArrayDoubleClone(X_original);

            double[] S = new double[X.GetLength(1)];
            double[,] x;
            for (int k = 0; k < X.GetLength(1); k++)
            {
                x = clt.SubColumnArrayDouble(X, k);

                if (clt.Sum(x) == 0.0 && clt.Varianciac(x)[0, 0] == 0.0)
                {
                    mensagem = "A variável independente " + VariaveisIndependentes[k] + " é constante (igual a " + clt.Double2Texto(X_original[0, k], 8)
                                + ") para todas as observações na tabela de dados.";
                    return true;
                }

                S[k] = Math.Sqrt(clt.MatrizMult(clt.MatrizTransp(x), x)[0, 0] / ((double)x.GetLength(0)));
                for (int i = 0; i < X.GetLength(0); i++)
                {
                    X[i, k] = X[i, k] / S[k];
                }
            }
            double[,] X_noint = clt.ArrayDoubleClone(X);

            double variancia_x = 0.0;
            for (int k = 0; k < X_noint.GetLength(1); k++)
            {
                x = clt.SubColumnArrayDouble(X, k);
                variancia_x = clt.Varianciac(x)[0, 0];
                if (variancia_x < 1.0e-10)
                {
                    mensagem = "A variável independente " + VariaveisIndependentes[k] + " é constante (igual a " + clt.Double2Texto(X_original[0, k], 8)
                                + ") para todas as observações na tabela de dados.";
                    return true;
                }
            }

            double[,] XtX;
            double[,] XtY;
            double[,] yk;
            double[,] Xk;
            double[,] beta = new double[0, 0];
            double[,] yk_hat;
            double[,] R2k = new double[X_noint.GetLength(1), 1];
            IpeaGeo.RegressoesEspaciais.clsSingularValueDecomposition svd;

            for (int k = 0; k < X_noint.GetLength(1); k++)
            {
                yk = clt.SubColumnArrayDouble(X_noint, k);
                Xk = clt.Concateh(clt.Ones(X_noint.GetLength(0), 1), clt.DeleteCol(X_noint, k));

                XtX = clt.MatrizMult(clt.MatrizTransp(Xk), Xk);
                XtY = clt.MatrizMult(clt.MatrizTransp(Xk), yk);

                beta = new double[XtX.GetLength(0), 1];
                svd = new IpeaGeo.RegressoesEspaciais.clsSingularValueDecomposition(ref XtX);
                svd.solve(ref XtY, ref beta);

                //beta = clt.MatrizMult(clt.MatrizInversa(XtX, XtY);

                yk_hat = clt.MatrizMult(Xk, beta);
                R2k[k, 0] = clt.Varianciac(yk_hat)[0, 0] / clt.Varianciac(yk)[0, 0];
                if (R2k[k, 0] > 0.999999)
                {
                    string[] variaveis = clt.DeleteStringFromArray(VariaveisIndependentes, k);
                    mensagem = "A variável independente " + VariaveisIndependentes[k]
                        + " é uma combinação linear dos demais regressores (R2 da regressão dessa variável versus as demais é igual a = "
                        + clt.Double2Texto(R2k[k, 0], 8) + "). A relação linear para as variáveis é "
                        + VariaveisIndependentes[k] + " = " + clt.Double2Texto(beta[0, 0] * S[k], 4);

                    for (int i = 1; i < beta.GetLength(0); i++)
                    {
                        mensagem += " + " + clt.Double2Texto(beta[i, 0] * S[k] / S[i], 4) + "x" + variaveis[i - 1];
                    }
                    mensagem += ".";

                    return true;
                }
            }

            return res;
        }

        #endregion

        #region tabulações indicadores para diagnóstico de multicolinearidade

        protected string IndicadoresDuasColunas(string[] variaveis, double[,] beta, double[,] stderror, string nome_indicador1, string nome_indicador2)
        {
            clsUtilTools clt = new clsUtilTools();

            int max_length_variaveis = 0;
            int max_length_beta = 0;
            int max_length_stderror = 0;

            string[] st_beta = new string[beta.GetLength(0)];
            string[] st_stderror = new string[beta.GetLength(0)];

            for (int i = 0; i < beta.GetLength(0); i++)
            {
                st_beta[i] = clt.Double2Texto(beta[i, 0], 6);
                if (st_beta[i].Length > max_length_beta) max_length_beta = st_beta[i].Length;

                st_stderror[i] = clt.Double2Texto(stderror[i, 0], 6);
                if (st_stderror[i].Length > max_length_stderror) max_length_stderror = st_stderror[i].Length;

                if (variaveis[i].Length > max_length_variaveis) max_length_variaveis = variaveis[i].Length;
            }

            if ("Variável".Length > max_length_variaveis) max_length_variaveis = "Variável".Length;
            if (nome_indicador1.Length > max_length_beta) max_length_beta = nome_indicador1.Length;
            if (nome_indicador2.Length > max_length_stderror) max_length_stderror = nome_indicador2.Length;

            max_length_variaveis += 2;
            max_length_beta += 6;
            max_length_stderror += 15;

            string res = "";

            res += "Variável" + PreencheEspacos(max_length_variaveis - "Variável".Length);
            res += PreencheEspacos(max_length_beta - nome_indicador1.Length) + nome_indicador1;
            res += PreencheEspacos(max_length_stderror - nome_indicador2.Length) + nome_indicador2;
            res += "\n\n";

            for (int i = 0; i < beta.GetLength(0); i++)
            {
                res += variaveis[i] + PreencheEspacos(max_length_variaveis - variaveis[i].Length);
                res += PreencheEspacos(max_length_beta - st_beta[i].Length) + st_beta[i];
                res += PreencheEspacos(max_length_stderror - st_stderror[i].Length) + st_stderror[i];
                res += "\n";
            }

            return res;
        }

        #endregion

        #region tabulação matriz de variância-covariância

        protected string GeraTabelaCovMatrix(double[,] covmat, string[] variaveis_independentes, bool inclui_intercepto)
        {
            clsUtilTools clt = new clsUtilTools();
            string[] variaveis = new string[covmat.GetLength(0)];
            if (inclui_intercepto)
            {
                variaveis[0] = "Intercepto";
                for (int j = 0; j < variaveis_independentes.GetLength(0); j++)
                {
                    variaveis[j + 1] = variaveis_independentes[j];
                }
            }
            else
            {
                for (int j = 0; j < variaveis_independentes.GetLength(0); j++)
                {
                    variaveis[j] = variaveis_independentes[j];
                }
            }

            string[,] mat = new string[covmat.GetLength(0) + 1, covmat.GetLength(1) + 1];
            mat[0, 0] = " ";
            for (int j = 0; j < variaveis.GetLength(0); j++)
            {
                mat[0, j + 1] = variaveis[j];
                mat[j + 1, 0] = variaveis[j];
            }

            for (int i = 1; i < mat.GetLength(0); i++)
            {
                for (int j = 1; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = clt.Double2Texto(covmat[i - 1, j - 1], 6);
                }
            }

            int[] cols_length = new int[mat.GetLength(1)];
            for (int j = 0; j < mat.GetLength(1); j++)
            {
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    if (cols_length[j] < mat[i, j].Length) cols_length[j] = mat[i, j].Length;
                }
                cols_length[j] += 3;
            }

            string res = "";
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                res += mat[i, 0] + PreencheEspacos(cols_length[0] - mat[i, 0].Length);
                for (int j = 1; j < mat.GetLength(1); j++)
                {
                    res += "\t" + PreencheEspacos(cols_length[j] - mat[i, j].Length) + mat[i, j];
                }
                res += "\n";
            }

            return res;
        }

        #endregion

        #region tabulação matriz geral

        protected string GeraTabelaMatrix(double[,] matriz)
        {
            clsUtilTools clt = new clsUtilTools();

            string[,] mat = new string[matriz.GetLength(0), matriz.GetLength(1)];


            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = clt.Double2Texto(matriz[i, j], 6);
                }
            }

            int[] cols_length = new int[mat.GetLength(1)];
            for (int j = 0; j < mat.GetLength(1); j++)
            {
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    //if (cols_length[j] < mat[i, j].Length) cols_length[j] = mat[i, j].Length;
                    cols_length[j] = mat[i, j].Length;
                }
                cols_length[j] += 3;
            }

            string res = "";
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                //res += mat[i, 0] + PreencheEspacos(cols_length[0] - mat[i, 0].Length);
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    //res += "\t" + PreencheEspacos(cols_length[j] - mat[i, j].Length) + mat[i, j];
                    res += PreencheEspacos(cols_length[j] - mat[i, j].Length) + mat[i, j];

                }
                res += "\n";
            }

            return res;
        }



        #endregion

        #region tabulação novas variáveis

        protected void AdicionaNovasVariaveisToDataTable(object[,] dados, string[] variaveis)
        {
            this.LimparTabelaDasVariaveisGeradas(ref this.m_dt_tabela_dados);

            if (m_adiciona_novas_variaveis)
            {
                for (int i = 0; i < variaveis.GetLength(0); i++)
                {
                    if (!this.m_dt_tabela_dados.Columns.Contains(variaveis[i]))
                    {
                        this.m_dt_tabela_dados.Columns.Add(variaveis[i], dados[0,i].GetType());
                    }

                    for (int j = 0; j < dados.GetLength(0); j++)
                    {
                        this.m_dt_tabela_dados.Rows[j][variaveis[i]] = dados[j, i];
                    }
                }
            }
        }

        protected void AdicionaNovasVariaveisToDataTable(double[,] dados, string[] variaveis)
        {
            clsUtilTools clt = new clsUtilTools();

            this.LimparTabelaDasVariaveisGeradas(ref this.m_dt_tabela_dados);

            if (m_adiciona_novas_variaveis)
            {
                clt.AdicionaColunasToDataTable(ref m_dt_tabela_dados, dados, variaveis);

                //for (int i = 0; i < variaveis.GetLength(0); i++)
                //{
                //    if (!this.m_dt_tabela_dados.Columns.Contains(variaveis[i]))
                //    {
                //        this.m_dt_tabela_dados.Columns.Add(variaveis[i], typeof(double));
                //    }

                //    for (int j = 0; j < dados.GetLength(0); j++)
                //    {
                //        this.m_dt_tabela_dados.Rows[j][variaveis[i]] = dados[j, i];
                //    }
                //}
            }
        }

        public string GeraTabelaNovasVariaveis(object[,] dados, string[] variaveis) 
        {
            clsUtilTools clt = new clsUtilTools();

            int[] max_length = new int[variaveis.GetLength(0)];
            string[,] st_dados = new string[dados.GetLength(0), dados.GetLength(1)];
            for (int i = 0; i < st_dados.GetLength(0); i++)
            {
                st_dados[i, 0] = dados[i, 0].ToString();
                if (max_length[0] < st_dados[i, 0].Length) max_length[0] = st_dados[i, 0].Length;

                for (int j = 1; j < st_dados.GetLength(1); j++)
                {
                    st_dados[i, j] = clt.Double2Texto(dados[i, j], 6);
                    if (max_length[j] < st_dados[i, j].Length) max_length[j] = st_dados[i, j].Length;
                }
            }

            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < variaveis.GetLength(0); j++)
            {
                if (max_length[j] < variaveis[j].Length) max_length[j] = variaveis[j].Length;
                max_length[j] = max_length[j] + 1;

                if (j == 0)
                {
                    sb.Append(variaveis[j] + PreencheEspacos(max_length[j] - variaveis[j].Length));
                }
                else
                {
                    sb.Append("\t" + PreencheEspacos(max_length[j] - variaveis[j].Length) + variaveis[j]);
                }
            }
            sb.Append("\n");

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                sb.Append(st_dados[i, 0] + PreencheEspacos(max_length[0] - st_dados[i, 0].Length));

                for (int j = 1; j < dados.GetLength(1); j++)
                {
                    sb.Append("\t" + PreencheEspacos(max_length[j] - st_dados[i, j].Length) + st_dados[i, j]);
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }

        public string GeraTabelaNovasVariaveis(double[,] dados, string[] variaveis)
        {
            clsUtilTools clt = new clsUtilTools();

            int[] max_length = new int[variaveis.GetLength(0)];
            string[,] st_dados = new string[dados.GetLength(0), dados.GetLength(1)];
            for (int i = 0; i < st_dados.GetLength(0); i++)
            {
                st_dados[i, 0] = dados[i, 0].ToString();
                if (max_length[0] < st_dados[i, 0].Length) max_length[0] = st_dados[i, 0].Length;

                for (int j = 1; j < st_dados.GetLength(1); j++)
                {
                    st_dados[i, j] = clt.Double2Texto(dados[i, j], 6);
                    if (max_length[j] < st_dados[i, j].Length) max_length[j] = st_dados[i, j].Length;
                }
            }

            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < variaveis.GetLength(0); j++)
            {
                if (max_length[j] < variaveis[j].Length) max_length[j] = variaveis[j].Length;
                max_length[j] = max_length[j] + 3;

                if (j == 0)
                {
                    sb.Append(variaveis[j] + PreencheEspacos(max_length[j] - variaveis[j].Length));
                }
                else
                {
                    sb.Append("\t" + PreencheEspacos(max_length[j] - variaveis[j].Length) + variaveis[j]);
                }
            }
            sb.Append("\n");

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                sb.Append(st_dados[i, 0] + PreencheEspacos(max_length[0] - st_dados[i, 0].Length));

                for (int j = 1; j < dados.GetLength(1); j++)
                {
                    sb.Append("\t" + PreencheEspacos(max_length[j] - st_dados[i, j].Length) + st_dados[i, j]);
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }

        protected string GeraTabelaNovasVariaveis_tabelanumerica(double[,] dados, string[] variaveis)
        {
            clsUtilTools clt = new clsUtilTools();

            int[] max_length = new int[variaveis.GetLength(0)];
            string[,] st_dados = new string[dados.GetLength(0), dados.GetLength(1)];
            for (int i = 0; i < st_dados.GetLength(0); i++)
            {
                st_dados[i, 0] = dados[i, 0].ToString();
                if (max_length[0] < st_dados[i, 0].Length) max_length[0] = st_dados[i, 0].Length;

                for (int j = 0; j < st_dados.GetLength(1); j++)
                {
                    st_dados[i, j] = clt.Double2Texto(dados[i, j], 6);
                    if (max_length[j] < st_dados[i, j].Length) max_length[j] = st_dados[i, j].Length;
                }
            }

            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < variaveis.GetLength(0); j++)
            {
                if (max_length[j] < variaveis[j].Length) max_length[j] = variaveis[j].Length;
                max_length[j] = max_length[j] + 3;

                if (j == 0)
                {
                    sb.Append(variaveis[j] + PreencheEspacos(max_length[j] - variaveis[j].Length));
                }
                else
                {
                    sb.Append("\t" + PreencheEspacos(max_length[j] - variaveis[j].Length) + variaveis[j]);
                }
            }
            sb.Append("\n");

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                sb.Append(st_dados[i, 0] + PreencheEspacos(max_length[0] - st_dados[i, 0].Length));

                for (int j = 1; j < dados.GetLength(1); j++)
                {
                    sb.Append("\t" + PreencheEspacos(max_length[j] - st_dados[i, j].Length) + st_dados[i, j]);
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }
 

        #endregion

        #region tabulação estimações

        /// <summary>
        /// Tabulação da tabela ANOVA para regressão linear.
        /// </summary>
        /// <param name="nomes">nomes</param>
        /// <param name="somadequadrados">vetor com soma de quadrados</param>
        /// <param name="grausdeliberdades">vetor com graus de liberdade</param>
        /// <param name="erropadrao">vetor com erros padrões e resto vazio</param>
        /// <param name="fstat">vetor com valor da estatistica F e o resto vazio</param>
        /// <param name="pvalue"></param>
        /// <returns></returns>
        public string GeraANOVA(string[] nomes, double[] somadequadrados, double[] grausdeliberdades, string[] erropadrao, string[] fstat, string[] pvalue)
        {
            clsUtilTools clt = new clsUtilTools();
            string[] variaveis = new string[somadequadrados.GetLength(0)];

            int max_length_variaveis = 0;
            int max_length_somadequadrados = 0;
            int max_length_grausdeliberdades = 0;
            int max_length_erropadrao = 0;
            int max_length_pvalue = 0;
            int max_length_fstat = 0;

            for (int j = 0; j < nomes.GetLength(0); j++)
            {
                variaveis[j] = nomes[j];
            }

            string[] st_somadequadrados = new string[somadequadrados.GetLength(0)];
            string[] st_grausdeliberdades = new string[somadequadrados.GetLength(0)];
            string[] st_erropadrao = new string[somadequadrados.GetLength(0)];
            string[] st_pvalue = new string[somadequadrados.GetLength(0)];
            string[] st_fstat = new string[somadequadrados.GetLength(0)];

            for (int i = 0; i < somadequadrados.GetLength(0); i++)
            {
                st_somadequadrados[i] = clt.Double2Texto(somadequadrados[i], 6);
                if (st_somadequadrados[i].Length > max_length_somadequadrados) max_length_somadequadrados = st_somadequadrados[i].Length;

                st_grausdeliberdades[i] = clt.Double2Texto(grausdeliberdades[i], 6);
                if (st_grausdeliberdades[i].Length > max_length_grausdeliberdades) max_length_grausdeliberdades = st_grausdeliberdades[i].Length;

                st_erropadrao[i] = clt.Double2Texto(erropadrao[i], 6);
                if (st_erropadrao[i].Length > max_length_erropadrao) max_length_erropadrao = st_erropadrao[i].Length;

                st_fstat[i] = clt.Double2Texto(fstat[i], 6);
                if (st_fstat[i].Length > max_length_fstat) max_length_fstat = st_fstat[i].Length;

                st_pvalue[i] = clt.Double2Texto(pvalue[i], 6);
                if (st_pvalue[i].Length > max_length_pvalue) max_length_pvalue = st_pvalue[i].Length;

                if (variaveis[i].Length > max_length_variaveis) max_length_variaveis = variaveis[i].Length;
            }

            if (" ".Length > max_length_variaveis) max_length_variaveis = " ".Length;
            if ("Soma de Quadrados".Length > max_length_somadequadrados) max_length_somadequadrados = "Soma de Quadrados".Length;
            if ("Graus de Liberdade".Length > max_length_grausdeliberdades) max_length_grausdeliberdades = "Graus de Liberdade".Length;
            if ("Erro Padrão".Length > max_length_erropadrao) max_length_erropadrao = "Erro Padrão".Length;
            if ("Estatística F".Length > max_length_fstat) max_length_fstat = "Estatística F".Length;
            if ("P-valor".Length > max_length_pvalue) max_length_pvalue = "P-valor".Length;

            max_length_variaveis += 10;
            max_length_somadequadrados += 6;
            max_length_pvalue += 6;
            max_length_grausdeliberdades += 6;
            max_length_erropadrao += 6;
            max_length_fstat += 6;

            string res = "";

            res += " " + PreencheEspacos(max_length_variaveis - " ".Length);
            res += PreencheEspacos(max_length_somadequadrados - "Soma de Quadrados".Length) + "Soma de Quadrados";
            res += PreencheEspacos(max_length_grausdeliberdades - "Graus de Liberdade".Length) + "Graus de Liberdade";
            res += PreencheEspacos(max_length_erropadrao - "Erro Padrão".Length) + "Erro Padrão";
            res += PreencheEspacos(max_length_fstat - "Estatística F".Length) + "Estatística F";
            res += PreencheEspacos(max_length_pvalue - "P-valor".Length) + "P-valor";
            res += "\n\n";

            for (int i = 0; i < somadequadrados.GetLength(0); i++)
            {
                res += variaveis[i] + PreencheEspacos(max_length_variaveis - variaveis[i].Length);
                res += PreencheEspacos(max_length_somadequadrados - st_somadequadrados[i].Length) + st_somadequadrados[i];
                res += PreencheEspacos(max_length_grausdeliberdades - st_grausdeliberdades[i].Length) + st_grausdeliberdades[i];
                res += PreencheEspacos(max_length_erropadrao - st_erropadrao[i].Length) + st_erropadrao[i];
                res += PreencheEspacos(max_length_fstat - st_fstat[i].Length) + st_fstat[i];
                res += PreencheEspacos(max_length_pvalue - st_pvalue[i].Length) + st_pvalue[i];
                res += "\n";
            }

            return res;
        }

        protected string ImprimeVetorColuna(double[] v)
        {
            clsUtilTools clt = new clsUtilTools();
            string[] rv = new string[v.GetLength(0)];
            int max_length = 0;
            for (int i = 0; i < rv.GetLength(0); i++)
            {
                rv[i] = clt.Double2Texto(v[i], 6);
                if (rv[i].Length > max_length) max_length = rv[i].Length;
            }
            string r = "";
            for (int i = 0; i < rv.GetLength(0); i++)
            {//TODO: quando eu classifiquei o preencheespaços como estatico, tive que tirar o this daqui (caue)
                r += "[" + PreencheEspacos(max_length - rv[i].Length) + rv[i] + "]\n";
            }
            return r;
        }

        protected string GeraTabelaEstimacoes(string[] variaveis_independentes, double[,] beta,
            double[,] stderror, double[,] tstat, double[,] pvalue, bool inclui_intercepto)
        {
            clsUtilTools clt = new clsUtilTools();
            string[] variaveis = new string[beta.GetLength(0)];

            if (inclui_intercepto)
            {
                variaveis[0] = "Intercepto";
                for (int j = 0; j < variaveis_independentes.GetLength(0); j++)
                {
                    variaveis[j + 1] = variaveis_independentes[j];
                }
            }
            else
            {
                for (int j = 0; j < variaveis_independentes.GetLength(0); j++)
                {
                    variaveis[j] = variaveis_independentes[j];
                }
            }

            int max_length_variaveis = 0;
            int max_length_beta = 0;
            int max_length_stderror = 0;
            int max_length_tstat = 0;
            int max_length_pvalue = 0;

            string[] st_beta = new string[beta.GetLength(0)];
            string[] st_stderror = new string[beta.GetLength(0)];
            string[] st_tstat = new string[beta.GetLength(0)];
            string[] st_pvalue = new string[beta.GetLength(0)];

            for (int i = 0; i < beta.GetLength(0); i++)
            {
                st_beta[i] = clt.Double2Texto(beta[i, 0], 6);
                if (st_beta[i].Length > max_length_beta) max_length_beta = st_beta[i].Length;

                st_stderror[i] = clt.Double2Texto(stderror[i, 0], 6);
                if (st_stderror[i].Length > max_length_stderror) max_length_stderror = st_stderror[i].Length;

                st_tstat[i] = clt.Double2Texto(tstat[i, 0], 6);
                if (st_tstat[i].Length > max_length_tstat) max_length_tstat = st_tstat[i].Length;

                st_pvalue[i] = clt.Double2Texto(pvalue[i, 0], 6);
                if (st_pvalue[i].Length > max_length_pvalue) max_length_pvalue = st_pvalue[i].Length;

                if (variaveis[i].Length > max_length_variaveis) max_length_variaveis = variaveis[i].Length;
            }

            if ("Variável".Length > max_length_variaveis) max_length_variaveis = "Variável".Length;
            if ("Coeficiente".Length > max_length_beta) max_length_beta = "Coeficiente".Length;
            if ("Erro padrão".Length > max_length_stderror) max_length_stderror = "Erro padrão".Length;
            if ("Estatística t".Length > max_length_tstat) max_length_tstat = "Estatística t".Length;
            if ("P-valor".Length > max_length_pvalue) max_length_pvalue = "P-valor".Length;

            max_length_variaveis += 2;
            max_length_beta += 6;
            max_length_pvalue += 6;
            max_length_stderror += 6;
            max_length_tstat += 6;

            string res = "";

            res += "Variável" + PreencheEspacos(max_length_variaveis - "Variável".Length);
            res += PreencheEspacos(max_length_beta - "Coeficiente".Length) + "Coeficiente";
            res += PreencheEspacos(max_length_stderror - "Erro padrão".Length) + "Erro padrão";
            res += PreencheEspacos(max_length_tstat - "Estatística t".Length) + "Estatística t";
            res += PreencheEspacos(max_length_pvalue - "P-valor".Length) + "P-valor";
            res += "\n\n";

            for (int i = 0; i < beta.GetLength(0); i++)
            {
                res += variaveis[i] + PreencheEspacos(max_length_variaveis - variaveis[i].Length);
                res += PreencheEspacos(max_length_beta - st_beta[i].Length) + st_beta[i];
                res += PreencheEspacos(max_length_stderror - st_stderror[i].Length) + st_stderror[i];
                res += PreencheEspacos(max_length_tstat - st_tstat[i].Length) + st_tstat[i];
                res += PreencheEspacos(max_length_pvalue - st_pvalue[i].Length) + st_pvalue[i];
                res += "\n";
            }

            return res;
        }

        protected string GeraTabelaEstimacoes(string[] variaveis_independentes, double[,] beta,
            double[,] stderror, double[,] pvalue, bool inclui_intercepto)
        {
            clsUtilTools clt = new clsUtilTools();
            string[] variaveis = new string[beta.GetLength(0)];

            if (inclui_intercepto)
            {
                variaveis[0] = "Intercepto";
                for (int j = 0; j < variaveis_independentes.GetLength(0); j++)
                {
                    variaveis[j + 1] = variaveis_independentes[j];
                }
            }
            else
            {
                for (int j = 0; j < variaveis_independentes.GetLength(0); j++)
                {
                    variaveis[j] = variaveis_independentes[j];
                }
            }

            int max_length_variaveis = 0;
            int max_length_beta = 0;
            int max_length_stderror = 0;
            int max_length_pvalue = 0;

            string[] st_beta = new string[beta.GetLength(0)];
            string[] st_stderror = new string[beta.GetLength(0)];
            string[] st_pvalue = new string[beta.GetLength(0)];

            for (int i = 0; i < beta.GetLength(0); i++)
            {
                st_beta[i] = clt.Double2Texto(beta[i, 0], 6);
                if (st_beta[i].Length > max_length_beta) max_length_beta = st_beta[i].Length;

                st_stderror[i] = clt.Double2Texto(stderror[i, 0], 6);
                if (st_stderror[i].Length > max_length_stderror) max_length_stderror = st_stderror[i].Length;

                st_pvalue[i] = clt.Double2Texto(pvalue[i, 0], 6);
                if (st_pvalue[i].Length > max_length_pvalue) max_length_pvalue = st_pvalue[i].Length;

                if (variaveis[i].Length > max_length_variaveis) max_length_variaveis = variaveis[i].Length;
            }

            if ("Variável".Length > max_length_variaveis) max_length_variaveis = "Variável".Length;
            if ("Coeficiente".Length > max_length_beta) max_length_beta = "Coeficiente".Length;
            if ("Erro padrão".Length > max_length_stderror) max_length_stderror = "Erro padrão".Length;
            if ("P-valor".Length > max_length_pvalue) max_length_pvalue = "P-valor".Length;

            max_length_variaveis += 2;
            max_length_beta += 6;
            max_length_pvalue += 6;
            max_length_stderror += 6;

            string res = "";

            res += "Variável" + PreencheEspacos(max_length_variaveis - "Variável".Length);
            res += PreencheEspacos(max_length_beta - "Coeficiente".Length) + "Coeficiente";
            res += PreencheEspacos(max_length_stderror - "Erro padrão".Length) + "Erro padrão";
            res += PreencheEspacos(max_length_pvalue - "P-valor".Length) + "P-valor";
            res += "\n\n";

            for (int i = 0; i < beta.GetLength(0); i++)
            {
                res += variaveis[i] + PreencheEspacos(max_length_variaveis - variaveis[i].Length);
                res += PreencheEspacos(max_length_beta - st_beta[i].Length) + st_beta[i];
                res += PreencheEspacos(max_length_stderror - st_stderror[i].Length) + st_stderror[i];
                res += PreencheEspacos(max_length_pvalue - st_pvalue[i].Length) + st_pvalue[i];
                res += "\n";
            }

            return res;
        }

        protected string GeraTabelaEstimacoesBinaryLogistic(string[] variaveis_independentes, double[,] beta,
            double[,] stderror, double[,] tstat, double[,] pvalue,double[,] oddsratio, bool inclui_intercepto)
        {
            clsUtilTools clt = new clsUtilTools();
            string[] variaveis = new string[beta.GetLength(0)];

            if (inclui_intercepto)
            {
                variaveis[0] = "Intercepto";
                for (int j = 0; j < variaveis_independentes.GetLength(0); j++)
                {
                    variaveis[j + 1] = variaveis_independentes[j];
                }
            }
            else
            {
                for (int j = 0; j < variaveis_independentes.GetLength(0); j++)
                {
                    variaveis[j] = variaveis_independentes[j];
                }
            }

            int max_length_variaveis = 0;
            int max_length_beta = 0;
            int max_length_stderror = 0;
            int max_length_tstat = 0;
            int max_length_pvalue = 0;
            int max_length_oddsratio = 0;

            string[] st_beta = new string[beta.GetLength(0)];
            string[] st_stderror = new string[beta.GetLength(0)];
            string[] st_tstat = new string[beta.GetLength(0)];
            string[] st_pvalue = new string[beta.GetLength(0)];
            string[] st_oddsratio = new string[beta.GetLength(0)];

            for (int i = 0; i < beta.GetLength(0); i++)
            {
                st_beta[i] = clt.Double2Texto(beta[i, 0], 6);
                if (st_beta[i].Length > max_length_beta) max_length_beta = st_beta[i].Length;

                st_stderror[i] = clt.Double2Texto(stderror[i, 0], 6);
                if (st_stderror[i].Length > max_length_stderror) max_length_stderror = st_stderror[i].Length;

                st_tstat[i] = clt.Double2Texto(tstat[i, 0], 6);
                if (st_tstat[i].Length > max_length_tstat) max_length_tstat = st_tstat[i].Length;

                st_pvalue[i] = clt.Double2Texto(pvalue[i, 0], 6);
                if (st_pvalue[i].Length > max_length_pvalue) max_length_pvalue = st_pvalue[i].Length;

                st_oddsratio[i] = clt.Double2Texto(oddsratio[i, 0], 6);
                if (st_oddsratio[i].Length > max_length_oddsratio) max_length_oddsratio = st_oddsratio[i].Length;


                if (variaveis[i].Length > max_length_variaveis) max_length_variaveis = variaveis[i].Length;
            }

            if ("Variável".Length > max_length_variaveis) max_length_variaveis = "Variável".Length;
            if ("Coeficiente".Length > max_length_beta) max_length_beta = "Coeficiente".Length;
            if ("Erro padrão".Length > max_length_stderror) max_length_stderror = "Erro padrão".Length;
            if ("Estatística t".Length > max_length_tstat) max_length_tstat = "Estatística t".Length;
            if ("P-valor".Length > max_length_pvalue) max_length_pvalue = "P-valor".Length;
            if ("Odds Ratio".Length > max_length_oddsratio) max_length_oddsratio = "Odds Ratio".Length;


            max_length_variaveis += 2;
            max_length_beta += 6;
            max_length_pvalue += 6;
            max_length_stderror += 6;
            max_length_tstat += 6;
            max_length_oddsratio += 6;


            string res = "";

            res += "Variável" + PreencheEspacos(max_length_variaveis - "Variável".Length);
            res += PreencheEspacos(max_length_beta - "Coeficiente".Length) + "Coeficiente";
            res += PreencheEspacos(max_length_stderror - "Erro padrão".Length) + "Erro padrão";
            res += PreencheEspacos(max_length_tstat - "Estatística t".Length) + "Estatística t";
            res += PreencheEspacos(max_length_pvalue - "P-valor".Length) + "P-valor";
            res += PreencheEspacos(max_length_oddsratio - "Odds Ratio".Length) + "Odds Ratio";
            res += "\n\n";

            for (int i = 0; i < beta.GetLength(0); i++)
            {
                res += variaveis[i] + PreencheEspacos(max_length_variaveis - variaveis[i].Length);
                res += PreencheEspacos(max_length_beta - st_beta[i].Length) + st_beta[i];
                res += PreencheEspacos(max_length_stderror - st_stderror[i].Length) + st_stderror[i];
                res += PreencheEspacos(max_length_tstat - st_tstat[i].Length) + st_tstat[i];
                res += PreencheEspacos(max_length_pvalue - st_pvalue[i].Length) + st_pvalue[i];
                res += PreencheEspacos(max_length_oddsratio - st_oddsratio[i].Length) + st_oddsratio[i];
                res += "\n";
            }

            return res;
        }

        //TODO: mudei a visibilidade desse método e o declarei como static. Poderiamos mudar também para os metodos do clsUtilTools(Cauê)

        public static string PreencheEspacos(int n)
        {
            string res = "";
            for (int i = 0; i < n; i++) res += " ";
            return res;
        }

        #endregion
        
        #region Análise de multicolinearidade

        public string AnaliseMulticolinearidade()
        {
            clsUtilTools clt = new clsUtilTools();

            //string[] variaveis_na_base;
            //string[] Wvariaveis_na_base;
            //SeparaBlocosVariaveis(out variaveis_na_base, out Wvariaveis_na_base, VariaveisIndependentes);

            double[,] X = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisIndependentes);

            //double[,] WX = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, Wvariaveis_na_base);
            //if (WX.GetLength(1) > 0)
            //{
            //    clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            //    this.GeraMatrizVizinhanca();
            //    WX = fme.MatrizMult(this.m_geomle.Wesparsa, WX);
            //    X = clt.Concateh(X, WX);
            //}

            double[,] corr_mat = clt.CorrSampleMatrix(X);

            string out_text = "============================================================================================================================\n\n";

            out_text += "Análise de multicolinearidade dos regressores\n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de variáveis: " + X.GetLength(1) + "\n";

            out_text += "\n";
            out_text += "Matriz de correlação: \n\n";

            out_text += this.GeraTabelaCovMatrix(corr_mat, VariaveisIndependentes, false);

            out_text += "\n";

            out_text += "Auto-valores da matrix SX'XS (a partir da matriz X normalizada): \n\n";

            double[,] X_noint = clt.ArrayDoubleClone(X);
            if (this.m_usa_intercepto)
            {
                X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
            }

            double[,] S = new double[X.GetLength(1), X.GetLength(1)];
            double[,] x;
            for (int i = 0; i < S.GetLength(0); i++)
            {
                x = clt.SubColumnArrayDouble(X, i);
                S[i, i] = 1.0 / Math.Sqrt(clt.MatrizMult(clt.MatrizTransp(x), x)[0, 0]);
            }

            double[,] XX = clt.MatrizMult(clt.MatrizTransp(X), X);
            double[,] SXXS = clt.MatrizMult(clt.MatrizMult(S, XX), S);

            double[,] V = new double[0, 0];
            double[] D = new double[0];

            clt.AutovaloresMatrizSimetrica(SXXS, ref V, ref D);

            string a = this.ImprimeVetorColuna(D);

            a += "\n";

            a += "Raiz quadrada da razão entre o maior e menor auto-valor: "
                + (D[D.GetLength(0) - 1] != 0 ? clt.Double2Texto(Math.Sqrt(D[0] / D[D.GetLength(0) - 1]), 6) : "Infinito");

            a += "\n\n";

            a += "OBS 1) A matriz (SX'SX) corresponde à matriz X'X normalizada, para evitar problemas de escala das variáveis. \n";
            a += "A razão acima é conhecida como condition number. A definição do condition number utilizado aqui pode ser encontrada \n";
            a += "na página 269, em W. H. Greene (1993), Econometric Analysis, Segunda Edição, Prentice Hall. Se o condition number for \n";
            a += "maior do que 20, há indicações de possíveis problemas causados por multicolinearidade.";

            a += "\n\n";

            double[,] yk;
            double[,] Xk;
            double[,] beta;
            double[,] yk_hat;
            double[,] R2k = new double[X_noint.GetLength(1), 1];
            double[,] Ck = new double[X_noint.GetLength(1), 1];
            for (int k = 0; k < X_noint.GetLength(1); k++)
            {
                yk = clt.SubColumnArrayDouble(X_noint, k);
                Xk = clt.Concateh(clt.Ones(X_noint.GetLength(0), 1), clt.DeleteCol(X_noint, k));
                beta = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(Xk), Xk)), clt.MatrizMult(clt.MatrizTransp(Xk), yk));
                yk_hat = clt.MatrizMult(Xk, beta);
                R2k[k, 0] = clt.Varianciac(yk_hat)[0, 0] / clt.Varianciac(yk)[0, 0];
                Ck[k, 0] = Math.Sqrt(1.0 - R2k[k, 0]);
            }

            a += IndicadoresDuasColunas(VariaveisIndependentes, R2k, Ck, "R2k", "Ck = (1 - R2k)^(1/2)");

            double[,] Y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisDependentes);

            yk = clt.ArrayDoubleClone(Y);
            Xk = clt.Concateh(clt.Ones(X_noint.GetLength(0), 1), X_noint);
            beta = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(Xk), Xk)), clt.MatrizMult(clt.MatrizTransp(Xk), yk));
            yk_hat = clt.MatrizMult(Xk, beta);
            double R2 = clt.Varianciac(yk_hat)[0, 0] / clt.Varianciac(yk)[0, 0];

            a += "\n";

            a += "R2 para a regressão original (estimada via OLS): " + clt.Double2Texto(R2, 6);

            a += "\n\n";

            a += "OBS 2) O R2k na tabela acima corresponde ao R2 da regressão entre cada variável independente versus as demais. Quando o R2k \n";
            a += "é igual a 1, significa que existe uma relação linear perfeita entre a variável independentes k e as demais, indicando a \n";
            a += "presença de multicolinearidade perfeita. Em casos menos extremos, valores de R2k próximos a 1, indicam regressores próximos da \n";
            a += "multicolinearidade. Quando o R2 da regressão (estimada via OLS) original for menor do que algum dos R2k, então há indicativo \n";
            a += "de problemas com multicolinearidade.";

            a += "\n\n";

            a += "OBS 3) A medida Ck = (1 - R2k)^(1/2) é sugerida na página 268, em  em W. H. Greene (1993), Econometric Analysis, Segunda  \n";
            a += "Edição, Prentice Hall. Ck corresponde à raiz quadrada da razão entre a variância de beta_k estimado com e sem as demais \n";
            a += "variáveis explicativas. Quando xk é não correlacionado com as demais variáveis independentes, Ck = 1.";

            a += "\n\n";

            return out_text + a;
        }


        #region Output Correlação;

        public string Analisedecorrelacao()
        {
            clsUtilTools clt = new clsUtilTools();


            double[,] X = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisIndependentes);
          

            double[,] corr_mat = clt.CorrSampleMatrix(X);

            string out_text = "";

            out_text = "=================== Matriz de Correlação dos Regressores \n\n";

            out_text += this.GeraTabelaCovMatrix(corr_mat, VariaveisIndependentes, false);

            out_text += "\n";

            
            
            return out_text; 
        }

        #endregion



        #endregion
    }
}
