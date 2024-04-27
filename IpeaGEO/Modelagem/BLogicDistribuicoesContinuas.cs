using System;
using System.Text;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public class BLogicDistribuicoesContinuas : BLogicBaseModelagem
    {
        private string m_output_text2 = "";
        public string ResultadoEstimacao2
        {
            get { return m_output_text2; }
            set { this.m_output_text2 = value; }
        }

        public BLogicDistribuicoesContinuas()
            : base()
        {
            ListaDistribuicoesContinuas = new string[] { "Normal", "Exponencial", "Gamma", "Cauchy", 
                "Distribuição F", "Chi-quadrada", "Beta", "Distribuição Chi", "Weibull", "Logística","Log-normal"};
        }

        public string[] ListaDistribuicoesContinuas = new string[0];
        private clsUtilTools m_clt = new clsUtilTools();
        private double[,] m_x = new double[0, 0];

        private string[] m_dist_escolhidas = new string[0];
        public string[] DistribuicoesEscolhidas
        {
            get { return m_dist_escolhidas; }
            set { m_dist_escolhidas = value; }
        }

        protected bool m_limpa_janela_resultados = true;
        public bool LimpaJanelaResultados
        {
            get { return m_limpa_janela_resultados; }
            set { m_limpa_janela_resultados = value; }
        }

        protected bool m_limpa_janela_consolidacao = true;
        public bool LimpaJanelaConsolidacao
        {
            get { return m_limpa_janela_consolidacao; }
            set { m_limpa_janela_consolidacao = value; }
        }

        protected string MatrizToString(object[,] mat)
        {
            clsUtilTools clt = new clsUtilTools();
            string[,] smat = new string[mat.GetLength(0), mat.GetLength(1)];
            for (int i = 0; i < smat.GetLength(0); i++)
            {
                for (int j = 0; j < smat.GetLength(1); j++)
                {
                    if (mat[i, j].GetType() == typeof(double) || mat[i, j].GetType() == typeof(Double))
                    {
                        smat[i, j] = clt.Double2Texto((double)mat[i, j], 6);
                    }
                    else
                    {
                        smat[i, j] = mat[i, j].ToString();
                    }
                }
            }

            int[] max_length = new int[smat.GetLength(1)];
            for (int j = 0; j < max_length.GetLength(0); j++)
            {
                max_length[j] = 0;
                for (int i = 0; i < smat.GetLength(0); i++)
                {
                    if (smat[i, j].Length > max_length[j]) max_length[j] = smat[i, j].Length;
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < smat.GetLength(0); i++)
            {
                sb.Append(smat[i, 0] + PreencheEspacos(max_length[0] - smat[i, 0].Length));
                for (int j = 1; j < smat.GetLength(1); j++)
                {
                    sb.Append("\t" + PreencheEspacos(max_length[j] - smat[i, j].Length) + smat[i, j]);
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        public void EstimaDistribuicao()
        {
            if (m_limpa_janela_consolidacao) m_output_text2 = "";
            if (m_limpa_janela_resultados) m_output_text = "";

            double[,] dados = new double[m_dt_tabela_dados.Rows.Count,1];
            double[,] covmat = new double[2,2];

            double alpha, beta, df, loglikgamma, loglikChiSquared, logliknormal, loglikexp, loglikpareto, betapareto, alphapareto, aicChiSquared,
                            bicChiSquared, aicnormal, bicnormal, aicgamma, bicgamma, aicexp, bicexp, mulognormal, sigma2lognormal, aiclognormal, biclognormal,
                            logliklognormal, aicweilbull, bicweilbull, gamma_weilbull, beta_weilbull, loglikweilbull, loglikChi, bicChi, aicChi,
                            m_chi, lambda, sigma2, mu, N1_F, N2_F, aicF, bicF, loglikF, gamma_b, beta_b, aicbeta, bicbeta, loglikbeta, lambda_logi,
                            k_logi, aiclogi, biclogi, logliklogi, alpha_cauchy, aiccauchy, biccauchy, loglikcauchy, aicpareto = 0.0, bicpareto = 0.0;

            alphapareto = 0.0;
            betapareto = 0.0;
            loglikpareto = 0.0;
            aicpareto = 0.0;
            bicpareto = 0.0;

            if (m_limpa_janela_consolidacao) m_output_text2 = "";
            if (m_limpa_janela_resultados) m_output_text = "";

            for (int k = 0; k < m_variaveis_dependentes.Length; k++)
            {                
                #region gerando o output para resultado das estimativas

                object[,] matriz_dados = new object[m_dist_escolhidas.Length + 1, 3];
                string[] result = { "AIC", "BIC" };
                matriz_dados[0, 0] = "Variáveis";
                matriz_dados[0, 1] = "AIC";
                matriz_dados[0, 2] = "BIC";
             
                for (int i = 0; i < m_dist_escolhidas.Length; i++)
                {
                    dados = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_dependentes[k]);

                    if (m_dist_escolhidas[i] == "Gamma")
                    {
                        this.EstimaGamma(out alpha, out beta, out covmat, out aicgamma, out bicgamma, out loglikgamma, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Gamma" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro alpha: " + m_clt.Double2Texto(alpha, 6) + "\n";
                        out_text += "Parâmetro beta: " + m_clt.Double2Texto(beta, 6) + "\n";
                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikgamma, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicgamma, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicgamma, 6) + "\n";

                        m_output_text += out_text;

                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aicgamma, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(bicgamma, 6);
                    }

                    if (m_dist_escolhidas[i] == "Exponencial")
                    {
                        this.estimaExponencial(out lambda, out covmat, out aicexp, out bicexp, out loglikexp, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Exponencial" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro lambda: " + m_clt.Double2Texto(lambda, 6) + "\n";
                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikexp, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicexp, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicexp, 6) + "\n";

                        m_output_text += out_text;

                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aicexp, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(bicexp, 6);
                    }

                    if (m_dist_escolhidas[i] == "Chi-quadrada")
                    {
                        this.estimachisquared(out df, out covmat, out aicChiSquared, out bicChiSquared, out loglikChiSquared, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Chi-quadrada" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Graus de liberdade: " + m_clt.Double2Texto(df, 6) + "\n";
                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikChiSquared, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicChiSquared, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicChiSquared, 6) + "\n";

                        m_output_text += out_text;

                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aicChiSquared, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(bicChiSquared, 6);
                    }

                    if (m_dist_escolhidas[i] == "Normal")
                    {
                        this.EstimaNormal(out mu, out sigma2, out covmat, out aicnormal, out bicnormal, out logliknormal, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Normal" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro mu: " + m_clt.Double2Texto(mu, 6) + "\n";
                        out_text += "Parâmetro sigma2: " + m_clt.Double2Texto(sigma2, 6) + "\n";
                        out_text += "Log-likelihood: " + m_clt.Double2Texto(logliknormal, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicnormal, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicnormal, 6) + "\n";

                        m_output_text += out_text;

                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aicnormal, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(bicnormal, 6);
                    }

                    if (m_dist_escolhidas[i] == "Pareto")
                    {
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[0] + "\n";
                        out_text += "Distribuição estimada: " + "Pareto" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro alpha: " + m_clt.Double2Texto(alphapareto, 6) + "\n";
                        out_text += "Parâmetro beta: " + m_clt.Double2Texto(betapareto, 6) + "\n";
                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikpareto, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicpareto, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicpareto, 6) + "\n";

                        m_output_text += out_text;
                    }

                    if (m_dist_escolhidas[i] == "Log-normal")
                    {
                        this.EstimalogNormal(out mulognormal, out sigma2lognormal, out covmat, out aiclognormal, out biclognormal, out logliklognormal, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Log-normal" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro mu: " + m_clt.Double2Texto(mulognormal, 6) + "\n";
                        out_text += "Parâmetro sigma2: " + m_clt.Double2Texto(sigma2lognormal, 6) + "\n";
                        out_text += "Log-likelihood: " + m_clt.Double2Texto(logliklognormal, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aiclognormal, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(biclognormal, 6) + "\n";

                        m_output_text += out_text;

                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aiclognormal, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(biclognormal, 6);
                    }

                    if (m_dist_escolhidas[i] == "Weibull")
                    {
                        this.EstimaWeibull(out gamma_weilbull, out beta_weilbull, out covmat, out aicweilbull, out bicweilbull, out loglikweilbull, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Weibull" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro Gamma: " + m_clt.Double2Texto(gamma_weilbull, 6) + "\n";
                        out_text += "Parâmetro Beta: " + m_clt.Double2Texto(beta_weilbull, 6) + "\n";
                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikweilbull, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicweilbull, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicweilbull, 6) + "\n";

                        m_output_text += out_text;

                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aicweilbull, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(bicweilbull, 6);
                    }

                    if (m_dist_escolhidas[i] == "Distribuição Chi")
                    {
                        this.estimachi(out m_chi, out covmat, out aicChi, out bicChi, out loglikChi, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Distribuição Chi" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Graus de liberdade: " + m_clt.Double2Texto(m_chi, 6) + "\n";
                        
                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikChi, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicChi, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicChi, 6) + "\n";

                        m_output_text += out_text;

                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aicChi, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(bicChi, 6);
                    }

                    if (m_dist_escolhidas[i] == "Distribuição F")
                    {
                        this.EstimaF(out N1_F, out N2_F, out covmat, out aicF, out bicF, out loglikF, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Distribuição F" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Graus de liberdade: " + m_clt.Double2Texto(N1_F, 6) + "\n";
                        out_text += "Graus de liberdade 2: " + m_clt.Double2Texto(N2_F, 6) + "\n";
                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikF, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicF, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicF, 6) + "\n";

                        m_output_text += out_text;

                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aicF, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(bicF, 6);
                    }

                    if (m_dist_escolhidas[i] == "Beta")
                    {
                        this.Estimabeta(out beta_b, out gamma_b, out covmat, out aicbeta, out bicbeta, out loglikbeta, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Distribuição Beta" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro Gamma: " + m_clt.Double2Texto(gamma_b, 6) + "\n";
                        out_text += "Parâmetro Beta: " + m_clt.Double2Texto(beta_b, 6) + "\n";
                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikbeta, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicbeta, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicbeta, 6) + "\n";

                        m_output_text += out_text;

                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aicbeta, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(bicbeta, 6);
                    }

                    if (m_dist_escolhidas[i] == "Logística")
                    {
                        this.Estimalogistica(out lambda_logi, out k_logi, out covmat, out aiclogi, out biclogi, out logliklogi, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Distribuição Logística" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro lambda: " + m_clt.Double2Texto(lambda_logi, 6) + "\n";
                        out_text += "Parâmetro k: " + m_clt.Double2Texto(k_logi, 6) + "\n";
                        out_text += "Log-likelihood: " + m_clt.Double2Texto(logliklogi, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aiclogi, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(biclogi, 6) + "\n";

                        m_output_text += out_text;

                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aiclogi, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(biclogi, 6);
                    }

                    if (m_dist_escolhidas[i] == "Cauchy")
                    {
                        this.Estimacauchy(out alpha_cauchy, out covmat, out aiccauchy, out biccauchy, out loglikcauchy, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Distribuição Cauchy" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro Alpha: " + m_clt.Double2Texto(alpha_cauchy, 6) + "\n";
                        
                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikcauchy, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aiccauchy, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(biccauchy, 6) + "\n";

                        m_output_text += out_text;

                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aiccauchy, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(biccauchy, 6);
                    }

                    m_output_text += "\n";
                }

                m_output_text2 += "============================================================================================================================\n\n";
                m_output_text2 += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n\n";
                m_output_text2 += MatrizToString(matriz_dados) + "\n";

                #endregion
            }
        }        

        #region distribuição normal

        private void EstimaNormal(out double mu, out double sigma2, out double[,] covmat, out double aicnormal, out double bicnormal,
            out double logliknormal, double[,] dados)
        {
            mu = m_clt.Mean(dados);
            sigma2 = m_clt.Varianciac(dados)[0,0];
            
            double dado_mu = new double();

            for(int i=0; i<dados.GetLength(0); i++)
            {
                for(int j=0; j<dados.GetLength(1); j++) 
                {
                    dado_mu += Math.Pow(dados[i,j]-mu,2);
                }
            }

            logliknormal = -(double)dados.GetLength(0) * Math.Log(Math.Sqrt(2 * Math.PI)) - (double)dados.GetLength(0) * Math.Log(sigma2) - (dado_mu / (2 * sigma2));

            covmat = new double[2, 2];
            aicnormal = 2 * 2 - 2 * logliknormal;
            bicnormal = 2 * logliknormal + 2 * Math.Log(dados.GetLength(0));
        }

        #endregion

        #region distribuição gamma

        private void EstimaGamma(out double alpha, out double beta, out double[,] covmat, out double aicgamma, out double bicgamma,
            out double loglikgamma, double[,] dados)
        {
            m_x = m_clt.ArrayDoubleClone(dados);

            for (int i = 0; i < m_x.GetLength(0); i++)
            {
                if (m_x[i, 0] < 0) throw new Exception("Valores para estimação da distribuição gamma devem ser todos não negativos.");
            }

            clsUtilOptimization.FunctionSimple func = new clsUtilOptimization.FunctionSimple(this.minus_loglik_gamma);
            IpeaGeo.RegressoesEspaciais.Fminsearch fmin = new IpeaGeo.RegressoesEspaciais.Fminsearch();
            double[,] x0 = new double[2, 1];

            x0[0, 0] = 0.0;
            x0[1, 0] = 0.0;

            double[,] x = new double[2, 1];
            double fval = 0.0;
            fmin.fminsearch(func, x0, ref x, ref fval);

            alpha = Math.Exp(x[0, 0]);
            beta = Math.Exp(x[1, 0]);

            loglikgamma = -fval;
            covmat = new double[2, 2];
            aicgamma = 2 * 2 - 2*loglikgamma;
            bicgamma = 2*loglikgamma+2*Math.Log(dados.GetLength(0));
        }

        private double minus_loglik_gamma(double[,] theta)
        {
            int n = m_x.GetLength(0);
            double a = Math.Exp(theta[0, 0]);
            double b = Math.Exp(theta[1, 0]);

            double l = -(double)n * MathSpecialFunctions.gammln(a) - (double)n * a * Math.Log(b) + (a - 1.0) * m_clt.Sum(m_clt.LogMatriz(m_x))
                - m_clt.Sum(m_x) / b;

            return - l;
        }

        #endregion

        #region distribuição_exponencial

        private void estimaExponencial(out double lambda, out double[,] covmat, out double aicexp, out double bicexp,
            out double loglikexp, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] < 0) throw new Exception("Valores para estimação da distribuição exponencial devem ser todos não negativos.");
            }

            loglikexp = 0.0;
            covmat = new double[2, 2];

            int n = dados.GetLength(0);
            
            lambda = 1 / m_clt.Mean(dados);

            loglikexp = n*Math.Log(lambda,Math.E) - lambda*m_clt.Sum(dados);
            aicexp = 2 * 1 - 2 * loglikexp;
            bicexp = 2 * loglikexp + 1 * Math.Log(dados.GetLength(0));
        }

        #endregion

        #region distribuição ChiSquared

        private void estimachisquared(out double df, out double[,] covmat, out double aicChiSquared, out double bicChiSquared,
        out double loglikChiSquared, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] <= 0.0) throw new Exception("Valores para estimação da distribuição qui-quadrada devem ser todos positivos.");
            }

            m_x = m_clt.ArrayDoubleClone(dados);

            clsUtilOptimization.FunctionSimple func = new clsUtilOptimization.FunctionSimple(this.minus_loglik_chisquared);
            IpeaGeo.RegressoesEspaciais.Fminsearch fmin = new IpeaGeo.RegressoesEspaciais.Fminsearch();

            double[,] x0 = new double[1, 1];

            x0[0, 0] = 0.0;
            
            double[,] x = new double[1, 1];
            double fval = new double();
            fmin.fminsearch(func, x0, ref x, ref fval);

            df = Math.Exp(x[0, 0]);

            loglikChiSquared = -fval;
            covmat = new double[2, 2];
            aicChiSquared = 2 * 1 - 2 * loglikChiSquared;
            bicChiSquared = 2 * loglikChiSquared + 1 * Math.Log(dados.GetLength(0));
        }
        
        private double minus_loglik_chisquared(double[,] theta)
        {
            int n = m_x.GetLength(0);
            double df = Math.Exp(theta[0,0]);

            double l = -(double)n * MathSpecialFunctions.gammln(df/2) - (double)n * (df/2) * Math.Log(2) + ((df/2) - 1.0) * m_clt.Sum(m_clt.LogMatriz(m_x))
                - m_clt.Sum(m_x) / 2;

            return -l;
        }

        #endregion

        #region distribuição lognormal

        private void EstimalogNormal(out double mulognormal, out double sigma2lognormal, out double[,] covmat, out double aiclognormal, out double biclognormal,
                    out double logliklognormal, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] <= 0.0) throw new Exception("Valores para estimação da distribuição lognormal devem ser todos positivos.");
            }

            double[,] dados_log = m_clt.LogMatriz(dados);

            mulognormal = m_clt.Mean(dados_log);
            sigma2lognormal = m_clt.Varianciac(dados_log)[0, 0];

            double dado_mu = new double();

            for (int i = 0; i < dados_log.GetLength(0); i++)
            {
                for (int j = 0; j < dados_log.GetLength(1); j++)
                {
                    dado_mu += Math.Pow(dados_log[i, j] - mulognormal, 2);
                }
            }

            logliklognormal = -(double)dados_log.GetLength(0) * Math.Log(Math.Sqrt(2 * Math.PI)) - (double)dados_log.GetLength(0) * Math.Log(sigma2lognormal) - (dado_mu / (2 * sigma2lognormal))- m_clt.Sum(dados_log);

            covmat = new double[2, 2];
            aiclognormal = 2 * 2 - 2 * logliklognormal;
            biclognormal = 2 * logliklognormal + 2 * Math.Log(dados_log.GetLength(0));
        }


        #endregion

        #region distribuição Weibull

        private void EstimaWeibull(out double gamma_weilbull, out double beta_weilbull, out double[,] covmat, out double aicweilbull, out double bicweilbull,
                    out double loglikweilbull, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] < 0.0) throw new Exception("Valores para estimação da distribuição Weibull devem ser todos não negativos.");
            }

            m_x = m_clt.ArrayDoubleClone(dados);

            clsUtilOptimization.FunctionSimple func = new clsUtilOptimization.FunctionSimple(this.minus_loglik_weilbull);
            IpeaGeo.RegressoesEspaciais.Fminsearch fmin = new IpeaGeo.RegressoesEspaciais.Fminsearch();
            double[,] x0 = new double[2, 1];

            x0[0, 0] = 0.0;
            x0[1, 0] = 0.0;

            double[,] x = new double[2, 1];
            double fval = 0.0;
            fmin.fminsearch(func, x0, ref x, ref fval);

            gamma_weilbull = Math.Exp(x[0, 0]);
            beta_weilbull = Math.Exp(x[1, 0]);

            loglikweilbull = -fval;
            covmat = new double[2, 2];
            aicweilbull = 2 * 2 - 2 * loglikweilbull;
            bicweilbull = 2 * loglikweilbull + 2 * Math.Log(dados.GetLength(0));
        }

        private double minus_loglik_weilbull(double[,] theta)
        {
            int n = m_x.GetLength(0);
            double beta_weibull = Math.Exp(theta[0, 0]);
            double alpha_weibull = Math.Exp(theta[1, 0]);

            double[,] m_x_ = new double[m_x.GetLength(0), m_x.GetLength(1)];

            for (int i = 0; i < m_x.GetLength(0); i++)
            {
                for (int j = 0; j < m_x.GetLength(1); j++)
                {
                    m_x_[i, j] = Math.Pow(m_x[i, j], beta_weibull);

                }
            }

            double l = (double)n * Math.Log(beta_weibull) - (double)n * Math.Log(alpha_weibull) + (beta_weibull - 1.0) * m_clt.Sum(m_clt.LogMatriz(m_x))
                -  (1.0/alpha_weibull) * m_clt.Sum(m_x_);

            return -l;
        }

        #endregion 

        #region distribuição chi;

        private void estimachi(out double m_chi, out double[,] covmat, out double aicChi, out double bicChi,
                    out double loglikChi, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] < 0.0) throw new Exception("Valores para estimação da distribuição Qui devem ser todos não negativos.");
            }

            m_x = m_clt.ArrayDoubleClone(dados);

            clsUtilOptimization.FunctionSimple func = new clsUtilOptimization.FunctionSimple(this.minus_loglik_chi);
            IpeaGeo.RegressoesEspaciais.Fminsearch fmin = new IpeaGeo.RegressoesEspaciais.Fminsearch();

            double[,] x0 = new double[1, 1];

            x0[0, 0] = 0.0;
            
            double[,] x = new double[1, 1];
            double fval = new double();
            fmin.fminsearch(func, x0, ref x, ref fval);

            m_chi  = Math.Exp(x[0, 0]);

            loglikChi = -fval;
            covmat = new double[2, 2];
            aicChi= 2 * 1 - 2 * loglikChi;
            bicChi = 2 * loglikChi + 1 * Math.Log(dados.GetLength(0));
        }
        
        private double minus_loglik_chi(double[,] theta)
        {
            int n = m_x.GetLength(0);
            double m_chi = Math.Exp(theta[0, 0]);

            double m_x_ = new double();

            for (int i = 0; i < m_x.GetLength(0);i++)
            {
                for (int j = 0; j < m_x.GetLength(1);j++)
                {
                    m_x_ += Math.Pow(m_x[i, j], 2); 
                }
            }

            double l = (m_chi - 1.0) * m_clt.Sum(m_clt.LogMatriz(m_x)) - (m_x_ / 2) - (double)n * ((m_chi / 2)-1) * Math.Log(2) - (double)n * MathSpecialFunctions.gammln(m_chi / 2);

            return -l;
        }
        
        #endregion

        #region distribuição F

        private void EstimaF(out double N1_F, out double N2_F, out double[,] covmat, out double aicF, out double bicF,
                    out double loglikF, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] < 0.0) throw new Exception("Valores para estimação da distribuição F devem ser todos não negativos.");
            }

            m_x = m_clt.ArrayDoubleClone(dados);

            clsUtilOptimization.FunctionSimple func = new clsUtilOptimization.FunctionSimple(this.minus_loglik_F);
            IpeaGeo.RegressoesEspaciais.Fminsearch fmin = new IpeaGeo.RegressoesEspaciais.Fminsearch();
            double[,] x0 = new double[2, 1];

            x0[0, 0] = 0.0;
            x0[1, 0] = 0.0;

            double[,] x = new double[2, 1];
            double fval = 0.0;
            fmin.fminsearch(func, x0, ref x, ref fval);

            N1_F  = Math.Exp(x[0, 0]);
            N2_F  = Math.Exp(x[1, 0]);

            loglikF = -fval;
            covmat = new double[2, 2];
            aicF = 2 * 2 - 2 * loglikF;
            bicF = 2 * loglikF + 2 * Math.Log(dados.GetLength(0));
        }

        private double minus_loglik_F(double[,] theta)
        {
            int n = m_x.GetLength(0);
            double N1F = Math.Exp(theta[0, 0]);
            double N2F = Math.Exp(theta[1, 0]);

            double m_x_ = new double();

            for (int i = 0; i < m_x.GetLength(0); i++)
            {
                for (int j = 0; j < m_x.GetLength(1); j++)
                {
                    m_x_ += Math.Log(m_x[i, j] * (N1F / N2F) + 1);

                }
            }

            double l = (double)n * MathSpecialFunctions.gammln((N1F + N2F)/2) + (((double)n * N1F )/2) * Math.Log(N1F/N2F) + ((N1F/2)-1.0) *
                m_clt.Sum(m_clt.LogMatriz(m_x)) - (double)n * MathSpecialFunctions.gammln(N1F / 2) - (double)n * MathSpecialFunctions.gammln(N2F / 2)
                 - ((N1F+N2F)/2) * m_x_;

            return -l;
        }


        #endregion

        #region distribuição Beta

        private void Estimabeta(out double beta_b, out double gamma_b, out double[,] covmat, out double aicbeta, out double bicbeta,
            out double loglikbeta, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] < 0.0 || dados[i,0] > 1.0) throw new Exception("Valores para estimação da distribuição beta devem estar no intervalo entre 0 e 1.");
            }

            m_x = m_clt.ArrayDoubleClone(dados);

            clsUtilOptimization.FunctionSimple func = new clsUtilOptimization.FunctionSimple(this.minus_loglik_beta);
            IpeaGeo.RegressoesEspaciais.Fminsearch fmin = new IpeaGeo.RegressoesEspaciais.Fminsearch();
            double[,] x0 = new double[2, 1];

            x0[0, 0] = 0.0;
            x0[1, 0] = 0.0;

            double[,] x = new double[2, 1];
            double fval = 0.0;
            fmin.fminsearch(func, x0, ref x, ref fval);

            beta_b  = Math.Exp(x[0, 0]);
            gamma_b  = Math.Exp(x[1, 0]);

            loglikbeta = -fval;
            covmat = new double[2, 2];
            aicbeta = 2 * 2 - 2 * loglikbeta;
            bicbeta = 2 * loglikbeta + 2 * Math.Log(dados.GetLength(0));
        }

        private double minus_loglik_beta(double[,] theta)
        {
            double[,] m_x_ = new double[m_x.GetLength(0),m_x.GetLength(1)];

            for (int i = 0; i < m_x.GetLength(0);i++)
            {
                for (int j = 0; j < m_x.GetLength(1);j++)
                {
                    m_x_[i,j] = (1 - m_x[i, j]); 

                }
            }

            int n = m_x.GetLength(0);
            double betab = Math.Exp(theta[0, 0]);
            double gammab = Math.Exp(theta[1, 0]);

            double l = (double)n * MathSpecialFunctions.gammln(betab + gammab) - (double)n * MathSpecialFunctions.gammln(betab) - (double)n *
                MathSpecialFunctions.gammln(gammab) + (betab - 1) * m_clt.Sum(m_clt.LogMatriz(m_x)) + (gammab - 1) * m_clt.Sum(m_clt.LogMatriz(m_x_));

            return -l;
        }

        #endregion

        #region distribuição logística

        private void Estimalogistica(out double lambda_logi, out double k_logi, out double[,] covmat, out double aiclogi, out double biclogi,
                    out double logliklogi, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] < 0.0) throw new Exception("Valores para estimação da distribuição logística devem ser todos não negativos.");
            }

            m_x = m_clt.ArrayDoubleClone(dados);

            clsUtilOptimization.FunctionSimple func = new clsUtilOptimization.FunctionSimple(this.minus_loglik_logi);
            IpeaGeo.RegressoesEspaciais.Fminsearch fmin = new IpeaGeo.RegressoesEspaciais.Fminsearch();
            double[,] x0 = new double[2, 1];

            x0[0, 0] = 0.0;
            x0[1, 0] = 0.0;

            double[,] x = new double[2, 1];
            double fval = 0.0;
            fmin.fminsearch(func, x0, ref x, ref fval);

            lambda_logi = Math.Exp(x[0, 0]);
            k_logi = Math.Exp(x[1, 0]);

            logliklogi = -fval;
            covmat = new double[2, 2];
            aiclogi = 2 * 2 - 2 * logliklogi;
            biclogi = 2 * logliklogi + 2 * Math.Log(dados.GetLength(0));
        }

        private double minus_loglik_logi(double[,] theta)
        {
            int n = m_x.GetLength(0);

            double  lambdalogi = Math.Exp(theta[0, 0]);
            double klogi = Math.Exp(theta[1, 0]);

            double m_x_ = new double();
            double[,] m_x_1 = new double[m_x.GetLength(0),m_x.GetLength(1)];

            for (int i = 0; i < m_x.GetLength(0); i++)
            {
                for (int j = 0; j < m_x.GetLength(1); j++)
                {
                    m_x_1[i,j] = lambdalogi* Math.Pow(Math.E,m_x[i,j]);

                    m_x_ += Math.Log(1 + Math.Pow(m_x_1[i,j],klogi));

                }
            }

            double l = (double)n * klogi * Math.Log(lambdalogi) + (double)n * Math.Log(klogi) + klogi * m_clt.Sum(m_x) - 2 * m_x_;

            return -l;
        }

        #endregion

        #region distribuição Cauchy

        private void Estimacauchy(out double alpha_cauchy, out double[,] covmat, out double aiccauchy, out double biccauchy,
              out double loglikcauchy, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] < 0.0) throw new Exception("Valores para estimação da distribuição Cauchy devem ser todos não negativos.");
            }

            m_x = m_clt.ArrayDoubleClone(dados);

            clsUtilOptimization.FunctionSimple func = new clsUtilOptimization.FunctionSimple(this.minus_loglik_cauchy);
            IpeaGeo.RegressoesEspaciais.Fminsearch fmin = new IpeaGeo.RegressoesEspaciais.Fminsearch();
            double[,] x0 = new double[1, 1];

            x0[0, 0] = 0.0;
           
            double[,] x = new double[2, 1];
            double fval = 0.0;
            fmin.fminsearch(func, x0, ref x, ref fval);

            alpha_cauchy = Math.Exp(x[0, 0]);
            
            loglikcauchy = -fval;
            covmat = new double[2, 2];
            aiccauchy = 2 * 2 - 2 * loglikcauchy;
            biccauchy = 2 * loglikcauchy  + 2 * Math.Log(dados.GetLength(0));
        }

        private double minus_loglik_cauchy(double[,] theta)
        {
            int n = m_x.GetLength(0);

            double alphacauchy = Math.Exp(theta[0, 0]);

            double m_x_ = new double();
            double[,] m_x_1 = new double[m_x.GetLength(0), m_x.GetLength(1)];

            for (int i = 0; i < m_x.GetLength(0); i++)
            {
                for (int j = 0; j < m_x.GetLength(1); j++)
                {
                    m_x_1[i, j] = ((m_x[i, j] - alphacauchy) / alphacauchy);

                    m_x_ += Math.Log(1 + Math.Pow(m_x_1[i,j], 2));
                }
            }

            double l = -(double)n * Math.Log(alphacauchy) - (double)n * Math.Log(Math.PI) - m_x_;

            return -l;
        }

        #endregion
    }
}
