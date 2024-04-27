using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace IpeaGEO.RegressoesEspaciais
{
    public enum TipoFuncaoLigacao : int
    {
        Logaritmo,
        Logit,
        Cloglog,
        Probit,
        Identidade
    };

    public class BLNonLinearConleyGMM : clsModelosRegressaoEspacial
    {
        public BLNonLinearConleyGMM()
            : base()
        {
        }

        #region variáveis internas

        private DataTable m_dt_tabela_dados = new DataTable();
        private string[] m_variaveis_dependentes = new string[0];
        private string[] m_variaveis_independentes = new string[0];
        private string[] m_variaveis_instrumentais = new string[0];
        private bool m_usa_intercepto = true;
        private string m_output_text = "";
        private string m_output_variaveis_geradas = "";
        private bool m_adiciona_novas_variaveis = false;

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

        private bool m_apresenta_covmatrix_beta_hat = false;
        public bool ApresentaCovMatrixBetaHat { set { this.m_apresenta_covmatrix_beta_hat = value; } }

        private string m_variavel_coord_X = "";
        private string m_variavel_coord_Y = "";
        private double m_cutoff_X = 0.0;
        private double m_cutoff_Y = 0.0;

        public string VariavelCoordenadaX { set { this.m_variavel_coord_X = value; } }
        public string VariavelCoordenadaY { set { this.m_variavel_coord_Y = value; } }
        public double CutOffCoordenadaX { set { this.m_cutoff_X = value; } }
        public double CutOffCoordenadaY { set { this.m_cutoff_Y = value; } }

        private int num_nonzero_elementos_matriz_W = 0;

        private clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            get { return this.m_shape.Clone(); }
            set { this.m_shape = value.Clone(); }
        }

        private TipoMatrizVizinhanca m_tipo_matriz_vizinhanca = TipoMatrizVizinhanca.Normalizada;
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
            set { this.m_dt_tabela_dados = value; }
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

        public string[] VariaveisInstrumentais
        {
            set { this.m_variaveis_instrumentais = value; }
            get { return this.m_variaveis_instrumentais; }
        }

        private double[,] m_W_matriz = new double[0, 0];

        private delegate double inv_funcao_ligacao(double[,] x, double[,] beta);
        private delegate double[,] diff_inv_funcao_ligacao(double[,] x, double[,] beta);
        private delegate double[,] diff2_inv_funcao_ligacao(double[,] x, double[,] beta);

        private inv_funcao_ligacao m_inv_link_function;
        private diff_inv_funcao_ligacao m_diff_inv_link_function;
        private diff_inv_funcao_ligacao m_diff2_inv_link_function;

        private TipoFuncaoLigacao m_tipo_funcao_ligacao = TipoFuncaoLigacao.Identidade;
        public TipoFuncaoLigacao TipoFuncaoLigacao
        {
            get { return this.m_tipo_funcao_ligacao; }
            set
            {
                m_tipo_funcao_ligacao = value;

                switch (value)
                {
                    case TipoFuncaoLigacao.Identidade:
                        m_inv_link_function = new inv_funcao_ligacao(this.identidade);
                        m_diff_inv_link_function = new diff_inv_funcao_ligacao(this.diff_identidade);
                        m_diff2_inv_link_function = new diff_inv_funcao_ligacao(this.diff2_identidade);
                        break;
                    case TipoFuncaoLigacao.Cloglog:
                        m_inv_link_function = new inv_funcao_ligacao(this.cloglog);
                        m_diff_inv_link_function = new diff_inv_funcao_ligacao(this.diff_cloglog);
                        m_diff2_inv_link_function = new diff_inv_funcao_ligacao(this.diff2_cloglog);
                        break;
                    case TipoFuncaoLigacao.Logaritmo:
                        m_inv_link_function = new inv_funcao_ligacao(this.exponencial);
                        m_diff_inv_link_function = new diff_inv_funcao_ligacao(this.diff_exponencial);
                        m_diff2_inv_link_function = new diff_inv_funcao_ligacao(this.diff2_exponencial);
                        break;
                    case TipoFuncaoLigacao.Logit:
                        m_inv_link_function = new inv_funcao_ligacao(this.logit);
                        m_diff_inv_link_function = new diff_inv_funcao_ligacao(this.diff_logit);
                        m_diff2_inv_link_function = new diff_inv_funcao_ligacao(this.diff2_logit);
                        break;
                    case TipoFuncaoLigacao.Probit:
                        m_inv_link_function = new inv_funcao_ligacao(this.probit);
                        m_diff_inv_link_function = new diff_inv_funcao_ligacao(this.diff_probit);
                        m_diff2_inv_link_function = new diff_inv_funcao_ligacao(this.diff2_probit);
                        break;
                    default:
                        break;
                }
            }
        }

        private MathNormaldist m_normal = new MathNormaldist();
        private clsUtilTools m_clt = new clsUtilTools();

        #endregion

        #region Estimação do GMM não espacial

        public void EstimaGMMNaoLinear()
        {
            this.TipoModeloRegressaoEspacial = TipoModeloEspacial.nao_espacial;

            clsUtilTools clt = new clsUtilTools();

            double[,] X = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisIndependentes);
            double[,] Z = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisInstrumentais);

            double[,] Y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisDependentes);

            if (Z.GetLength(1) < X.GetLength(1))
            {
                Z = clt.ArrayDoubleClone(X);
                VariaveisInstrumentais = VariaveisIndependentes;
            }

            if (this.m_usa_intercepto)
            {
                X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
                Z = clt.Concateh(clt.Ones(Z.GetLength(0), 1), Z);
            }

            m_X = X;
            m_Y = Y;
            m_Z = Z;

            #region iteraçoes para estimação

            m_nobs = m_X.GetLength(0);

            double[,] theta_old = new double[m_X.GetLength(1), 1];
            double[,] theta_new = new double[m_X.GetLength(1), 1];

            double[,] gradiente = new double[0, 0];
            double[,] hessiana = new double[0, 0];
            double[,] inv_cov_matrix = new double[0, 0];
            double funcao = 0.0;
            double[,] inv_hessiana = clt.Identity(m_X.GetLength(1));
            double[,] gt_vector = new double[m_nobs, 1];
            double[,] erro = new double[m_nobs, 1];
            double[,] x;

            int num_max_iteracoes = 10000;
            double tolerancia = (1.0e-5) * ((double)m_Y.GetLength(1));

            this.m_W_matriz = clt.Identity(m_Z.GetLength(1));

            int iter = 0;
            for (iter = 0; iter < num_max_iteracoes; iter++)
            {
                obj_function(ref funcao, ref gradiente, ref hessiana, ref inv_cov_matrix, theta_old);
                inv_hessiana = m_clt.MatrizInversa(hessiana);
                theta_new = m_clt.MatrizSubtracao(theta_old, m_clt.MatrizMult(inv_hessiana, gradiente));

                if (m_X.GetLength(1) == m_Z.GetLength(1))
                {
                    if (Math.Abs(funcao) < tolerancia && m_clt.Norm(gradiente) < tolerancia)
                    {
                        break;
                    }
                }
                else
                {
                    if (m_clt.Norm(gradiente) < tolerancia)
                    {
                        break;
                    }
                }

                theta_old = m_clt.ArrayDoubleClone(theta_new);

                for (int i = 0; i < m_nobs; i++)
                {
                    x = m_clt.MatrizTransp(m_clt.SubRowArrayDouble(m_X, i));
                    erro[i, 0] = m_Y[i, 0] - this.m_inv_link_function(x, theta_old);
                }

                gt_vector = new double[m_Z.GetLength(0), m_Z.GetLength(1)];
                for (int i = 0; i < m_Z.GetLength(0); i++)
                {
                    for (int j = 0; j < m_Z.GetLength(1); j++)
                    {
                        gt_vector[i, j] = erro[i, 0] * Z[i, j];
                    }
                }
                m_W_matriz = this.GMM_weight_matrix(gt_vector);
            }

            #endregion

            #region outras estatísticas

            this.m_beta_hat = theta_new;
            this.m_beta_hat_cov = clt.MatrizMult(1.0/(double)m_nobs, clt.MatrizInversa(inv_cov_matrix));
            this.GeraSignificanciaCoeficientes();

            double[,] media_gt = clt.Meanc(gt_vector);

            double Jstat = (clt.MatrizMult((double)m_nobs, clt.MatrizMult(clt.MatrizMult(media_gt, m_W_matriz), 
                clt.MatrizTransp(media_gt))))[0, 0];
            
            double Jpvalue = 0.0;
            {
                int m = m_Z.GetLength(1);
                int k = m_X.GetLength(1);
                if (m - k > 0)
                {
                    MathChisqdist chi = new MathChisqdist(m - k);
                    Jpvalue = 1.0 - chi.cdf(Jstat);
                }
                else
                {
                    Jpvalue = 1.0;
                }
            }

            #endregion

            #region saída dos resultados

            string stipo_funcao_ligacao = "Identidade";
            switch (this.m_tipo_funcao_ligacao)
            {
                case TipoFuncaoLigacao.Cloglog:
                    stipo_funcao_ligacao = "Complementary log-log";
                    break;
                case TipoFuncaoLigacao.Logaritmo:
                    stipo_funcao_ligacao = "Logaritmo";
                    break;
                case TipoFuncaoLigacao.Logit:
                    stipo_funcao_ligacao = "Logit";
                    break;
                case TipoFuncaoLigacao.Probit:
                    stipo_funcao_ligacao = "Probit";
                    break;
                default:
                    stipo_funcao_ligacao = "Identidade";
                    break;
            }

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação via GMM (Modelos Não-Lineares)\n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Função de ligação: " + stipo_funcao_ligacao + "\n";
            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + m_X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + m_X.GetLength(1) + "\n";
            out_text += "Número de iterações: " + iter.ToString() + "\n\n";

            out_text += "Estatística J: " + clt.Double2Texto(Jstat, 6) + "\n";
            out_text += "P-valor J: " + clt.Double2Texto(Jpvalue, 6) + "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, this.m_beta_hat, this.m_beta_stderror,
                                             this.m_beta_tstat, this.m_beta_pvalor, this.m_usa_intercepto);

            out_text += "\n";
            out_text += "Variáveis instrumentais utilizadas na estimação: \n\n";
            if (this.m_usa_intercepto) out_text += "Constante" + "\n";
            for (int i = 0; i < VariaveisInstrumentais.GetLength(0); i++) out_text += VariaveisInstrumentais[i] + "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(this.m_beta_hat_cov, VariaveisIndependentes, m_usa_intercepto);
            }

            this.m_output_text = out_text;

            //double[,] variaveis_geradas = gmm.VariaveisGeradas;

            //string[] nomes_variaveis = new string[5];
            //nomes_variaveis[0] = "Observacao";
            //nomes_variaveis[1] = "Y_observado";
            //nomes_variaveis[2] = "Y_predito";
            //nomes_variaveis[3] = "Residuo";
            //nomes_variaveis[4] = "Residuo_padronizado";

            //m_output_variaveis_geradas = "============================================================================================================================\n\n";

            //m_output_variaveis_geradas += "Estimação via GMM Espacial\n\n";
            //m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            //m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            //m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);

            //AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);

            #endregion
        }

        #endregion

        #region Estimação do GMM espacial (Conley)

        public void EstimaGMMNaoLinearEspacial()
        {
            this.TipoModeloRegressaoEspacial = TipoModeloEspacial.nao_espacial;

            clsUtilTools clt = new clsUtilTools();

            double[,] X = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisIndependentes);
            double[,] Z = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisInstrumentais);

            double[,] Y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisDependentes);

            if (Z.GetLength(1) < X.GetLength(1))
            {
                Z = clt.ArrayDoubleClone(X);
                VariaveisInstrumentais = VariaveisIndependentes;
            }

            if (this.m_usa_intercepto)
            {
                X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
                Z = clt.Concateh(clt.Ones(Z.GetLength(0), 1), Z);
            }

            m_X = X;
            m_Y = Y;
            m_Z = Z;

            double[,] coord_X = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, m_variavel_coord_X);
            double[,] coord_Y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, m_variavel_coord_Y);

            #region iteraçoes para estimação

            m_nobs = m_X.GetLength(0);

            double[,] theta_old = new double[m_X.GetLength(1), 1];
            double[,] theta_new = new double[m_X.GetLength(1), 1];

            double[,] gradiente = new double[0, 0];
            double[,] hessiana = new double[0, 0];
            double[,] inv_cov_matrix = new double[0, 0];
            double funcao = 0.0;
            double[,] inv_hessiana = clt.Identity(m_X.GetLength(1));
            double[,] gt_vector = new double[m_nobs, 1];
            double[,] erro = new double[m_nobs, 1];
            double[,] x;

            int num_max_iteracoes = 10000;
            double tolerancia = (1.0e-5) * ((double)m_Y.GetLength(1));

            this.m_W_matriz = clt.Identity(m_Z.GetLength(1));

            int iter = 0;
            for (iter = 0; iter < num_max_iteracoes; iter++)
            {
                obj_function(ref funcao, ref gradiente, ref hessiana, ref inv_cov_matrix, theta_old);
                inv_hessiana = m_clt.MatrizInversa(hessiana);
                theta_new = m_clt.MatrizSubtracao(theta_old, m_clt.MatrizMult(inv_hessiana, gradiente));

                if (m_X.GetLength(1) == m_Z.GetLength(1))
                {
                    if (Math.Abs(funcao) < tolerancia && m_clt.Norm(gradiente) < tolerancia)
                    {
                        break;
                    }
                }
                else
                {
                    if (m_clt.Norm(gradiente) < tolerancia)
                    {
                        break;
                    }
                }

                theta_old = m_clt.ArrayDoubleClone(theta_new);

                for (int i = 0; i < m_nobs; i++)
                {
                    x = m_clt.MatrizTransp(m_clt.SubRowArrayDouble(m_X, i));
                    erro[i, 0] = m_Y[i, 0] - this.m_inv_link_function(x, theta_old);
                }

                gt_vector = new double[m_Z.GetLength(0), m_Z.GetLength(1)];
                for (int i = 0; i < m_Z.GetLength(0); i++)
                {
                    for (int j = 0; j < m_Z.GetLength(1); j++)
                    {
                        gt_vector[i, j] = erro[i, 0] * Z[i, j];
                    }
                }

                m_W_matriz = this.Spatial_GMM_weight_matrix(gt_vector, coord_X, coord_Y, this.m_cutoff_X, this.m_cutoff_Y);
            }

            #endregion

            #region outras estatísticas

            this.m_beta_hat = theta_new;
            this.m_beta_hat_cov = clt.MatrizMult(1.0 / (double)m_nobs, clt.MatrizInversa(inv_cov_matrix));
            this.GeraSignificanciaCoeficientes();

            double[,] media_gt = clt.Meanc(gt_vector);

            double Jstat = (clt.MatrizMult((double)m_nobs, clt.MatrizMult(clt.MatrizMult(media_gt, m_W_matriz),
                clt.MatrizTransp(media_gt))))[0, 0];

            double Jpvalue = 0.0;
            {
                int m = m_Z.GetLength(1);
                int k = m_X.GetLength(1);
                if (m - k > 0)
                {
                    MathChisqdist chi = new MathChisqdist(m - k);
                    Jpvalue = 1.0 - chi.cdf(Jstat);
                }
                else
                {
                    Jpvalue = 1.0;
                }
            }

            #endregion

            #region saída dos resultados

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação via GMM Espacial\n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + m_X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + m_X.GetLength(1) + "\n";
            out_text += "Número de iterações: " + iter.ToString() + "\n\n";

            out_text += "Estatística J: " + clt.Double2Texto(Jstat, 6) + "\n";
            out_text += "P-valor J: " + clt.Double2Texto(Jpvalue, 6) + "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, this.m_beta_hat, this.m_beta_stderror,
                                             this.m_beta_tstat, this.m_beta_pvalor, this.m_usa_intercepto);

            out_text += "\n";
            out_text += "Variáveis instrumentais utilizadas na estimação: \n\n";
            if (this.m_usa_intercepto) out_text += "Constante" + "\n";
            for (int i = 0; i < VariaveisInstrumentais.GetLength(0); i++) out_text += VariaveisInstrumentais[i] + "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(this.m_beta_hat_cov, VariaveisIndependentes, m_usa_intercepto);
            }

            this.m_output_text = out_text;

            //double[,] variaveis_geradas = gmm.VariaveisGeradas;

            //string[] nomes_variaveis = new string[5];
            //nomes_variaveis[0] = "Observacao";
            //nomes_variaveis[1] = "Y_observado";
            //nomes_variaveis[2] = "Y_predito";
            //nomes_variaveis[3] = "Residuo";
            //nomes_variaveis[4] = "Residuo_padronizado";

            //m_output_variaveis_geradas = "============================================================================================================================\n\n";

            //m_output_variaveis_geradas += "Estimação via GMM Espacial\n\n";
            //m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            //m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            //m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);

            //AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);

            #endregion
        }

        #endregion

        #region função objetivo, seu gradiente e a sua hessiana

        private void obj_function(ref double obj_func, ref double[,] grad_obj_func, 
                                  ref double[,] hess_obj_func, ref double[,] inv_cov_matrix, double[,] theta)
        {
            double[,] x;
            double[,] z;
            double y;

            double obj_f = 0.0;
            double[,] gradiente = new double[m_X.GetLength(1), 1];
            double[,] hessiana1 = new double[m_X.GetLength(1), m_X.GetLength(1)];
            double[,] hessiana2 = new double[m_X.GetLength(1), m_X.GetLength(1)];

            double[,] v1 = new double[m_Z.GetLength(1),1];
            double[,] v2 = new double[m_X.GetLength(1), m_Z.GetLength(1)];
            double[,] v3 = new double[m_X.GetLength(1), m_X.GetLength(1)];

            for (int i = 0; i < m_nobs; i++)
            {
                x = m_clt.MatrizTransp(m_clt.SubRowArrayDouble(m_X, i));
                z = m_clt.MatrizTransp(m_clt.SubRowArrayDouble(m_Z, i));
                y = m_Y[i, 0];

                v1 = m_clt.MatrizSoma(v1, m_clt.MatrizMult((y - this.m_inv_link_function(x, theta)), z));
                v2 = m_clt.MatrizSoma(v2, m_clt.MatrizMult(m_diff_inv_link_function(x, theta), m_clt.MatrizTransp(z)));
            }

            v1 = m_clt.MatrizMult(v1, 1.0 / (double)m_nobs);
            v2 = m_clt.MatrizMult(v2, -2.0 / (double)m_nobs);

            hessiana1 = m_clt.MatrizMult(0.5, m_clt.MatrizMult(m_clt.MatrizMult(v2, m_W_matriz), m_clt.MatrizTransp(v2)));

            obj_f = m_clt.MatrizMult(m_clt.MatrizMult(m_clt.MatrizTransp(v1), m_W_matriz), v1)[0,0];
            gradiente = m_clt.MatrizMult(m_clt.MatrizMult(v2, m_W_matriz), v1);

            for (int i = 0; i < m_nobs; i++)
            {
                x = m_clt.MatrizTransp(m_clt.SubRowArrayDouble(m_X, i));
                z = m_clt.MatrizTransp(m_clt.SubRowArrayDouble(m_Z, i));
                
                v3 = m_clt.MatrizSoma(v3, m_clt.MatrizMult(
                     m_clt.MatrizMult(m_clt.MatrizMult(m_diff2_inv_link_function(x, theta), m_clt.MatrizTransp(z)),
                     m_clt.MatrizMult(m_W_matriz, v1)), m_clt.MatrizTransp(x)));
            }
            hessiana2 = m_clt.MatrizMult(v3, -2.0 / (double)m_nobs);
          
            double[,] hessiana = m_clt.MatrizSoma(hessiana1, hessiana2);

            obj_func = obj_f;
            grad_obj_func = gradiente;
            hess_obj_func = hessiana;
            inv_cov_matrix = m_clt.MatrizMult(0.5, hessiana1);
        }

        #endregion

        #region derivadas parciais de segunda ordem do inverso das funções de ligação

        private double[,] diff2_probit(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            double diff = - m * m_normal.p(m);
            //double[,] xxt = m_clt.MatrizMultMtranspM(x);
            //return m_clt.MatrizMult(diff, xxt);
            return m_clt.MatrizMult(diff, x);
        }

        private double[,] diff2_cloglog(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            double diff = (1.0 - Math.Exp(m)) * Math.Exp(m - Math.Exp(m));
            //double[,] xxt = m_clt.MatrizMultMtranspM(x);
            //return m_clt.MatrizMult(diff, xxt);
            return m_clt.MatrizMult(diff, x);
        }

        private double[,] diff2_logit(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            double diff = Math.Exp(m);
            diff = (diff * (1.0 - diff) / Math.Pow(1.0 + diff, 3.0));
            //double[,] xxt = m_clt.MatrizMultMtranspM(x);
            //return m_clt.MatrizMult(diff, xxt);
            return m_clt.MatrizMult(diff, x);
        }

        private double[,] diff2_identidade(double[,] x, double[,] beta)
        {
            //return new double[x.GetLength(0), x.GetLength(0)];
            return new double[x.GetLength(0), 1];
        }

        private double[,] diff2_exponencial(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            double diff = Math.Exp(m);
            //double[,] xxt = m_clt.MatrizMultMtranspM(x);
            //return m_clt.MatrizMult(diff, xxt);
            return m_clt.MatrizMult(diff, x);
        }

        #endregion

        #region derivadas parciais de primeira ordem do inverso das funções de ligação

        private double[,] diff_probit(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            double diff = m_normal.p(m);
            return m_clt.MatrizMult(diff, x);
        }

        private double[,] diff_cloglog(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            double diff = Math.Exp(m - Math.Exp(m));
            return m_clt.MatrizMult(diff, x);
        }

        private double[,] diff_logit(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            double diff = (Math.Exp(m) / Math.Pow(1.0 + Math.Exp(m), 2.0));
            return m_clt.MatrizMult(diff, x);
        }

        private double[,] diff_identidade(double[,] x, double[,] beta)
        {
            return m_clt.ArrayDoubleClone(x);
        }

        private double[,] diff_exponencial(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            double diff = Math.Exp(m);
            return m_clt.MatrizMult(diff, x);
        }

        #endregion

        #region inverso das funções de ligação

        private double probit(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            return m_normal.cdf(m);
        }

        private double cloglog(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            return 1.0 - Math.Exp(-Math.Exp(m)); 
        }

        private double logit(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            return (Math.Exp(m) / (1.0 + Math.Exp(m)));
        }

        private double identidade(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            return m;
        }

        private double exponencial(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            return Math.Exp(m);
        }

        #endregion

        #region tabulação matriz de variância-covariância

        private string GeraTabelaCovMatrix(double[,] covmat, string[] variaveis_independentes, bool inclui_intercepto)
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

        #region tabulação novas variáveis

        private void AdicionaNovasVariaveisToDataTable(double[,] dados, string[] variaveis)
        {
            if (m_adiciona_novas_variaveis)
            {
                for (int i = 0; i < variaveis.GetLength(0); i++)
                {
                    if (!this.m_dt_tabela_dados.Columns.Contains(variaveis[i]))
                    {
                        this.m_dt_tabela_dados.Columns.Add(variaveis[i], typeof(double));
                    }

                    for (int j = 0; j < dados.GetLength(0); j++)
                    {
                        this.m_dt_tabela_dados.Rows[j][variaveis[i]] = dados[j, i];
                    }
                }
            }
        }

        private string GeraTabelaNovasVariaveis(double[,] dados, string[] variaveis)
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

        #endregion

        #region tabulação estimações

        private string ImprimeVetorColuna(double[] v)
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
            {
                r += "[" + this.PreencheEspacos(max_length - rv[i].Length) + rv[i] + "]\n";
            }
            return r;
        }

        private string GeraTabelaEstimacoes(string[] variaveis_independentes, double[,] beta,
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

        private string PreencheEspacos(int n)
        {
            string res = "";
            for (int i = 0; i < n; i++) res += " ";
            return res;
        }

        #endregion

        #region matriz de pesos do GMM

        public double[,] GMM_weight_matrix(double[,] gt_vector)
        {
            double[,] dgt_vector = new double[gt_vector.GetLength(0), gt_vector.GetLength(1)];
            double[,] media = m_clt.Meanc(gt_vector);
            for (int i = 0; i < gt_vector.GetLength(0); i++)
            {
                for (int j = 0; j < gt_vector.GetLength(1); j++)
                {
                    dgt_vector[i, j] = gt_vector[i, j] - media[0, j];
                }
            }
            return m_clt.MatrizInversa(m_clt.MatrizMult(m_clt.MatrizMult(m_clt.MatrizTransp(dgt_vector), dgt_vector), 
                                        1.0 / (double)dgt_vector.GetLength(0)));
        }

        public double[,] Spatial_GMM_weight_matrix(double[,] gt_vector_in, double[,] x_coord, double[,] y_coord, double cutoff_x, double cutoff_y)
        {
            double[,] gt_vector = new double[gt_vector_in.GetLength(0), gt_vector_in.GetLength(1)];
            double[,] media = m_clt.Meanc(gt_vector_in);
            for (int i = 0; i < gt_vector_in.GetLength(0); i++)
            {
                for (int j = 0; j < gt_vector_in.GetLength(1); j++)
                {
                    gt_vector[i, j] = gt_vector_in[i, j] - media[0, j];
                }
            } 
            
            int T = gt_vector.GetLength(0);
            double peso = 0.0;
            double D_horizontal = 0.0;
            double D_vertical = 0.0;
            int col_gt = gt_vector.GetLength(1);
            double[,] Omega_hat = new double[col_gt, col_gt];
            int contador = 0;
            for (int i = 0; i < T; i++)
            {
                for (int j = 0; j < T; j++)
                {
                    D_horizontal = Math.Abs(x_coord[i, 0] - x_coord[j, 0]);
                    D_vertical = Math.Abs(y_coord[i, 0] - y_coord[j, 0]);
                    if ((D_horizontal <= cutoff_x) && (D_vertical <= cutoff_y))
                    {
                        peso = (1.0 - (D_horizontal / cutoff_x)) * (1.0 - (D_vertical / cutoff_y));
                        Omega_hat = m_clt.MatrizSoma(Omega_hat,
                            m_clt.MatrizMult(peso, m_clt.MatrizMult(m_clt.MatrizTransp(m_clt.SubRowArrayDouble(gt_vector, i)), 
                                                   m_clt.SubRowArrayDouble(gt_vector, j))));
                        contador++;
                    }
                }
            }
            Omega_hat = m_clt.MatrizMult(Omega_hat, 1.0 / (double)gt_vector.GetLength(0));
            return m_clt.MatrizInversa(Omega_hat);
        }       

        #endregion
    }
}
