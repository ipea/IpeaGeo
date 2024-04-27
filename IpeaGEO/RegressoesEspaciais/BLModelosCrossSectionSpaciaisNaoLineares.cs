using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace IpeaGeo.RegressoesEspaciais
{
    public class BLModelosCrossSectionSpaciaisNaoLineares : BLModelosCrossSectionSpaciais
    {
        public BLModelosCrossSectionSpaciaisNaoLineares()
            : base()
        {
        }

        #region variáveis internas

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
            double[,] theta_diff = new double[m_X.GetLength(1), 1];

            double[,] gradiente = new double[0, 0];
            double[,] hessiana = new double[0, 0];
            double[,] inv_cov_matrix = new double[0, 0];
            double funcao = 0.0;
            double[,] inv_hessiana = clt.Identity(m_X.GetLength(1));
            double[,] gt_vector = new double[m_nobs, 1];
            double[,] erro = new double[m_nobs, 1];
            double[,] x;

            int num_max_iteracoes = 10000;
            double tolerancia_funcao = (1.0e-5) * ((double)m_X.GetLength(1));
            double tolerancia_gradiente = (1.0e-8) * ((double)m_X.GetLength(1));
            double tolerancia_beta_diff = (1.0e-5) * ((double)m_X.GetLength(1));

            this.m_W_matriz = clt.Identity(m_Z.GetLength(1));

            int iter = 0;
            for (iter = 0; iter < num_max_iteracoes; iter++)
            {
                obj_function(ref funcao, ref gradiente, ref hessiana, ref inv_cov_matrix, theta_old);
                inv_hessiana = m_clt.MatrizInversa(hessiana);
                theta_new = m_clt.MatrizSubtracao(theta_old, m_clt.MatrizMult(inv_hessiana, gradiente));
                
                if (m_X.GetLength(1) == m_Z.GetLength(1))
                {
                    if (Math.Abs(funcao) < tolerancia_funcao && m_clt.Norm(gradiente) < tolerancia_funcao)
                    {
                        break;
                    }
                }
                else
                {
                    theta_diff = clt.MatrizSubtracao(theta_new, theta_old);

                    if (m_clt.Norm(gradiente) < tolerancia_gradiente && clt.Norm(theta_diff) < tolerancia_beta_diff)
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
            iter++;
            if (iter <= num_max_iteracoes)
            {
                this.m_mensagem_iterations_till_convergence = "Convergência atingida com sucesso";
            }
            else
            {
                this.m_mensagem_iterations_till_convergence = "Convergência não atingida no limite de iterações especificado";
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

            double[,] Y_hat = new double[Y.GetLength(0), 1];
            for (int i = 0; i < m_nobs; i++)
            {
                x = m_clt.MatrizTransp(m_clt.SubRowArrayDouble(m_X, i));
            	Y_hat[i,0] = this.m_inv_link_function(x, m_beta_hat);
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
            out_text += this.m_mensagem_iterations_till_convergence + "\n";
            out_text += "Número de iterações: " + iter.ToString() + "\n\n";

            out_text += "Estatística J: " + clt.Double2Texto(Jstat, 6) + "\n";
            out_text += "P-valor J: " + clt.Double2Texto(Jpvalue, 6) + "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, this.m_beta_hat, this.m_beta_stderror,
                                             this.m_beta_tstat, this.m_beta_pvalor, this.m_usa_intercepto);

            out_text += "\n";
            out_text += "Variáveis instrumentais utilizadas na estimação: \n\n";
            if (this.m_usa_intercepto) out_text += "Constante" + "\n";
            for (int i = 0; i < VariaveisInstrumentais.GetLength(0); i++) out_text += VariaveisInstrumentais[i] + "\n";

            if (m_tipo_funcao_ligacao == TipoFuncaoLigacao.Cloglog
                || m_tipo_funcao_ligacao == TipoFuncaoLigacao.Logit
                || m_tipo_funcao_ligacao == TipoFuncaoLigacao.Probit)
            {
                out_text += "\n";
                out_text += "============================================================================================================================\n\n";
                out_text += "Medidas de ajuste para modelos de resposta binária \n\n";                
                out_text += this.ChecaAjusteModelosBinarios(Y, Y_hat);
            }

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(this.m_beta_hat_cov, VariaveisIndependentes, m_usa_intercepto);
            }

            this.m_output_text = out_text;

            #endregion

            #region novas variáveis na base de dados

            double[,] variaveis_geradas = new double[m_nobs, 4];
            for (int i=0; i<m_nobs; i++)
            {
                variaveis_geradas[i, 0] = (double)(i + 1);
                variaveis_geradas[i, 1] = m_Y[i, 0];
                variaveis_geradas[i, 2] = m_inv_link_function(m_clt.MatrizTransp(m_clt.SubRowArrayDouble(m_X, i)), this.m_beta_hat);
                variaveis_geradas[i, 3] = variaveis_geradas[i, 2] - variaveis_geradas[i, 1];
            }

            string[] nomes_variaveis = new string[4];
            nomes_variaveis[0] = "Observacao_";
            nomes_variaveis[1] = "Y_observado_";
            nomes_variaveis[2] = "Y_predito_";
            nomes_variaveis[3] = "Residuo_";

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação via GMM (Modelos Não-Lineares)\n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);

            AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);

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
            double[,] theta_diff = new double[m_X.GetLength(1), 1];

            double[,] gradiente = new double[0, 0];
            double[,] hessiana = new double[0, 0];
            double[,] inv_cov_matrix = new double[0, 0];
            double funcao = 0.0;
            double[,] inv_hessiana = clt.Identity(m_X.GetLength(1));
            double[,] gt_vector = new double[m_nobs, 1];
            double[,] erro = new double[m_nobs, 1];
            double[,] x;

            int num_max_iteracoes = 10000;
            double tolerancia_funcao = (1.0e-5) * ((double)m_X.GetLength(1));
            double tolerancia_gradiente = (1.0e-8) * ((double)m_X.GetLength(1));
            double tolerancia_beta_diff = (1.0e-5) * ((double)m_X.GetLength(1));

            this.m_W_matriz = clt.Identity(m_Z.GetLength(1));

            int iter = 0;
            for (iter = 0; iter < num_max_iteracoes; iter++)
            {
                obj_function(ref funcao, ref gradiente, ref hessiana, ref inv_cov_matrix, theta_old);
                inv_hessiana = m_clt.MatrizInversa(hessiana);
                theta_new = m_clt.MatrizSubtracao(theta_old, m_clt.MatrizMult(inv_hessiana, gradiente));

                if (m_X.GetLength(1) == m_Z.GetLength(1))
                {
                    if (Math.Abs(funcao) < tolerancia_funcao && m_clt.Norm(gradiente) < tolerancia_funcao)
                    {
                        break;
                    }
                }
                else
                {
                    theta_diff = clt.MatrizSubtracao(theta_new, theta_old);

                    if (m_clt.Norm(gradiente) < tolerancia_gradiente && clt.Norm(theta_diff) < tolerancia_beta_diff)
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
            iter++;
            if (iter <= num_max_iteracoes)
            {
                this.m_mensagem_iterations_till_convergence = "Convergência atingida com sucesso";
            }
            else
            {
                this.m_mensagem_iterations_till_convergence = "Convergência não atingida no limite de iterações especificado";
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

            double[,] Y_hat = new double[Y.GetLength(0), 1];
            for (int i = 0; i < m_nobs; i++)
            {
                x = m_clt.MatrizTransp(m_clt.SubRowArrayDouble(m_X, i));
                Y_hat[i, 0] = this.m_inv_link_function(x, m_beta_hat);
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

            out_text += "Estimação via GMM Espacial (Conley, modelos não-lineares)\n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Função de ligação: " + stipo_funcao_ligacao + "\n";
            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + m_X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + m_X.GetLength(1) + "\n";
            out_text += this.m_mensagem_iterations_till_convergence + "\n";
            out_text += "Número de iterações: " + iter.ToString() +"\n\n";

            out_text += "Variável de coordenada x: " + m_variavel_coord_X + "\n";
            out_text += "Variável de coordenada y: " + m_variavel_coord_Y + "\n";
            out_text += "Cut-off coordenada x: " + m_clt.Double2Texto(m_cutoff_X, 6) + "\n";
            out_text += "Cut-off coordenada y: " + m_clt.Double2Texto(m_cutoff_Y, 6) + "\n\n";

            out_text += "Estatística J: " + clt.Double2Texto(Jstat, 6) + "\n";
            out_text += "P-valor J: " + clt.Double2Texto(Jpvalue, 6) + "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, this.m_beta_hat, this.m_beta_stderror,
                                             this.m_beta_tstat, this.m_beta_pvalor, this.m_usa_intercepto);

            out_text += "\n";
            out_text += "Variáveis instrumentais utilizadas na estimação: \n\n";
            if (this.m_usa_intercepto) out_text += "Constante" + "\n";
            for (int i = 0; i < VariaveisInstrumentais.GetLength(0); i++) out_text += VariaveisInstrumentais[i] + "\n";

            if (m_tipo_funcao_ligacao == TipoFuncaoLigacao.Cloglog
                || m_tipo_funcao_ligacao == TipoFuncaoLigacao.Logit
                || m_tipo_funcao_ligacao == TipoFuncaoLigacao.Probit)
            {
                out_text += "\n";
                out_text += "============================================================================================================================\n\n";
                out_text += "Medidas de ajuste para modelos de resposta binária \n\n";
                out_text += this.ChecaAjusteModelosBinarios(Y, Y_hat);
            }

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(this.m_beta_hat_cov, VariaveisIndependentes, m_usa_intercepto);
            }

            this.m_output_text = out_text;

            #endregion
            
            #region novas variáveis na base de dados

            double[,] variaveis_geradas = new double[m_nobs, 4];
            for (int i = 0; i < m_nobs; i++)
            {
                variaveis_geradas[i, 0] = (double)(i + 1);
                variaveis_geradas[i, 1] = m_Y[i, 0];
                variaveis_geradas[i, 2] = m_inv_link_function(m_clt.MatrizTransp(m_clt.SubRowArrayDouble(m_X, i)), this.m_beta_hat);
                variaveis_geradas[i, 3] = variaveis_geradas[i, 2] - variaveis_geradas[i, 1];
            }

            string[] nomes_variaveis = new string[4];
            nomes_variaveis[0] = "Observacao_";
            nomes_variaveis[1] = "Y_observado_";
            nomes_variaveis[2] = "Y_predito_";
            nomes_variaveis[3] = "Residuo_";

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação via GMM Espacial (Conley, modelos não-lineares)\n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);

            AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);

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

        #region avaliação do ajuste para modelos con variáveis binárias

        private string ChecaAjusteModelosBinarios(double[,] y, double[,] y_hat)
        {
            int n = y.GetLength(0);
            int nc = 0;
            int nd = 0;
            int nties = 0;
            int t = 0;

            for (int i = 0; i < n; i++)
            {
            	for (int j = i+1; j < n; j++)
                {
                	if (y[i,0] != y[j,0])
                    {
                        t++;
				        if (y[i,0] > y[j,0] && y_hat[i,0] > y_hat[j,0]) nc = nc+1;
				        if (y[i,0] < y[j,0] && y_hat[i,0] < y_hat[j,0]) nc = nc+1;
				        if (y[i,0] > y[j,0] && y_hat[i,0] < y_hat[j,0]) nd = nd+1;
				        if (y[i,0] < y[j,0] && y_hat[i,0] > y_hat[j,0]) nd = nd+1;
                    }
                }
            }

        	nties = t - nc - nd;

	        double perc_pares_concordantes = 100.0 * (double)nc / (double)t;
	        double perc_pares_discordantes = 100.0 * (double)nd / (double)t;
	        double perc_pares_ties = 100.0 * (double)nties / (double)t;
	        double total_pares = (double)t;

	        double croc = ((double)nc + 0.5*((double)t-(double)nc-(double)nd))/(double)t;
	        double somersD = ((double)nc - (double)nd)/(double)t;
	        double kruskal = ((double)nc - (double)nd)/((double)nc+(double)nd);
	        double kendaltau = ((double)nc-(double)nd)/(0.5*(double)n*((double)n-1));

            string res = "";
            res += "Total de pares: " + t + "\n";
            res += "Total de pares concordantes: " + nc.ToString() + "\n";
            res += "Total de pares discordantes: " + nd.ToString() + "\n";
            res += "Total de pares empatados: " + nties.ToString() + "\n\n";

            res += "Percentual de pares concordantes: " + m_clt.Double2Texto(perc_pares_concordantes, 6) + "% \n";
            res += "Percentual de pares discordantes: " + m_clt.Double2Texto(perc_pares_discordantes, 6) + "% \n";
            res += "Percentual de pares empatados: " + m_clt.Double2Texto(perc_pares_ties, 6) + "% \n\n";

            res += "C: " + m_clt.Double2Texto(croc, 6) + "\n";
            res += "Somer's D: " + m_clt.Double2Texto(somersD, 6) + "\n";
            res += "Kruskal: " + m_clt.Double2Texto(kruskal, 6) + "\n";
            res += "Kendal-Tau: " + m_clt.Double2Texto(kendaltau, 6) + "\n";

            return res;
        }

        #endregion
    }
}
