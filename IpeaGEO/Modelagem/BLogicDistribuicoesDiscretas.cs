using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public class BLogicDistribuicoesDiscretas : BLogicBaseModelagem
    {
        private string m_output_text2 = "";
        public string ResultadoEstimacao2
        {
            get { return m_output_text2; }
            set { this.m_output_text2 = value; }
        }

        public BLogicDistribuicoesDiscretas()
            : base()
        {
            ListaDistribuicoesDiscretas = new string[] { "Binomial", "Bernoulli", "Geométrica", "Pascal", 
                "Poisson"};
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

        public string[] ListaDistribuicoesDiscretas = new string[0];
        private clsUtilTools m_clt = new clsUtilTools();
        private double[,] m_x = new double[0, 0];

        private string[] m_dist_escolhidas = new string[0];
        public string[] DistribuicoesEscolhidas
        {
            get { return m_dist_escolhidas; }
            set { m_dist_escolhidas = value; }
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
                sb.Append(smat[i, 0] + PreencheEspacos(max_length[0]-smat[i,0].Length));
                for (int j=1; j<smat.GetLength(1); j++)
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

            double[,] covmat;

            double p_bernoulli, aicbernoulli, bicbernoulli, loglikbernoulli, p_geo, aicgeo, bicgeo, loglikgeo, lambda_poisson, loglikpoisson,
                aicpoisson, bicpoisson, p_pascal, aicpascal, bicpascal, loglikpascal, aicbinomial, bicbinomial, loglikbinomial, p_binomial;

            double[,] dados = new double[m_dt_tabela_dados.Rows.Count, 1];

            for (int k = 0; k < m_variaveis_dependentes.Length; k++)
            {
                dados = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_dependentes[k]);

                #region gerando o output para resultado das estimaes
                
                object[,] matriz_dados = new object[m_dist_escolhidas.Length + 1, 3];
                string[] result = { "AIC", "BIC" };
                matriz_dados[0, 0] = "Variáveis";
                matriz_dados[0, 1] = "AIC";
                matriz_dados[0, 2] = "BIC";

                //this.AdicionaNovasVariaveisToDataTable1(matriz_dados, result);

                for (int i = 0; i < m_dist_escolhidas.Length; i++)
                {

                    if (m_dist_escolhidas[i] == "Bernoulli")
                    {
                        this.Estimabernoulli(out p_bernoulli, out covmat, out aicbernoulli, out bicbernoulli, out loglikbernoulli, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Bernoulli" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro P: " + m_clt.Double2Texto(p_bernoulli, 6) + "\n";

                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikbernoulli, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicbernoulli, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicbernoulli, 6) + "\n";

                        m_output_text += out_text;

                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aicbernoulli, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(bicbernoulli, 6);
                    }

                    if (m_dist_escolhidas[i] == "Poisson")
                    {
                        this.Estimapoisson(out lambda_poisson, out covmat, out aicpoisson, out bicpoisson, out loglikpoisson, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Poisson" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro lambda: " + m_clt.Double2Texto(lambda_poisson, 6) + "\n";

                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikpoisson, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicpoisson, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicpoisson, 6) + "\n";

                        m_output_text += out_text;

                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aicpoisson, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(bicpoisson, 6);
                    }

                    if (m_dist_escolhidas[i] == "Geométrica")
                    {
                        this.Estimageometrica(out p_geo, out covmat, out aicgeo, out bicgeo, out loglikgeo, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Geométrica" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro P: " + m_clt.Double2Texto(p_geo, 6) + "\n";

                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikgeo, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicgeo, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicgeo, 6) + "\n";

                        m_output_text += out_text;
                        
                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aicgeo, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(bicgeo, 6);
                    }

                    if (m_dist_escolhidas[i] == "Pascal")
                    {
                        this.Estimapascal(out p_pascal, out covmat, out aicpascal, out bicpascal, out loglikpascal, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Pascal" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro P: " + m_clt.Double2Texto(p_pascal, 6) + "\n";

                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikpascal, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicpascal, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicpascal, 6) + "\n";

                        m_output_text += out_text;
                        
                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aicpascal, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(bicpascal, 6);
                    }

                    if (m_dist_escolhidas[i] == "Binomial")
                    {
                        this.Estimabinomial(out p_binomial, out covmat, out aicbinomial, out bicbinomial, out loglikbinomial, dados);
                        string out_text = "============================================================================================================================\n\n";

                        out_text += "Estimação via Máxima Verossimilhança \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n";
                        out_text += "Distribuição estimada: " + "Binomial" + "\n\n";

                        out_text += "Numero de observacoes: " + dados.GetLength(0) + "\n";
                        out_text += "Parâmetro P: " + m_clt.Double2Texto(p_binomial, 6) + "\n";

                        out_text += "Log-likelihood: " + m_clt.Double2Texto(loglikbinomial, 6) + "\n";
                        out_text += "AIC: " + m_clt.Double2Texto(aicbinomial, 6) + "\n";
                        out_text += "BIC: " + m_clt.Double2Texto(bicbinomial, 6) + "\n";

                        m_output_text += out_text;
                        
                        matriz_dados[i + 1, 0] = m_dist_escolhidas[i];
                        matriz_dados[i + 1, 1] = m_clt.Double2Texto(aicbinomial, 6);
                        matriz_dados[i + 1, 2] = m_clt.Double2Texto(bicbinomial, 6);
                    }

                    m_output_text += "\n";
                }
                
                m_output_text2 += "============================================================================================================================\n\n";
                m_output_text2 += "Variável dos dados: " + m_variaveis_dependentes[k] + "\n\n";
                m_output_text2 += MatrizToString(matriz_dados) + "\n";

                #endregion
            }
        }
        
        #region distribuição bernoulli

        private void Estimabernoulli(out double p_bernoulli, out double[,] covmat, out double aicbernoulli, out double bicbernoulli,
            out double loglikbernoulli, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] < 0.0 || dados[i,0] > 1.0) throw new Exception("Valores para estimação da distribuição de Bernoulli devem estar no intervalo entre 0 e 1.");
            }
            
            p_bernoulli = m_clt.Mean(dados);

            int n = dados.GetLength(0);

            loglikbernoulli = m_clt.Sum(dados) * Math.Log(p_bernoulli) + ((double)n - m_clt.Sum(dados)) * Math.Log(1 - p_bernoulli);

            covmat = new double[2, 2];
            aicbernoulli = 2 * 2 - 2 * loglikbernoulli;
            bicbernoulli = 2 * loglikbernoulli + 2 * Math.Log(dados.GetLength(0));
        }

        #endregion

        #region Distribuição Geométrica

        private void Estimageometrica(out double p_geo, out double[,] covmat, out double aicgeo, out double bicgeo,
           out double loglikgeo, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] < 0.0) throw new Exception("Valores para estimação da distribuição geométrica devem ser todos não negativos.");
            }

            int n = dados.GetLength(0);

            p_geo =  (double) n / ( m_clt.Sum(dados) + (double) n );

            loglikgeo = (double) n * Math.Log(p_geo) + m_clt.Sum(dados) * Math.Log(1-p_geo);

            covmat = new double[2, 2];
            aicgeo = 2 * 2 - 2 * loglikgeo;
            bicgeo = 2 * loglikgeo + 2 * Math.Log(dados.GetLength(0));
        }

        #endregion

        #region distribuição poisson

        private void Estimapoisson(out double lambda_poisson, out double[,] covmat, out double aicpoisson, out double bicpoisson,
                    out double loglikpoisson, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] < 0.0) throw new Exception("Valores para estimação da distribuição de Poisson devem ser todos não negativos.");
            }

            double dados_ =0.0;

            for(int i=0; i<dados.GetLength(0);i++)
            {
                for(int k=0; k< dados.GetLength(1);k++)
                {
                    //dados_ += Math.Log(MathSpecialFunctions.factrl((int)dados[i,k]));
                    dados_ += IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(dados[i, k] + 1);
                }
            }
            
            int n = dados.GetLength(0);

            lambda_poisson = m_clt.Mean(dados);

            loglikpoisson = m_clt.Sum(dados) * Math.Log(lambda_poisson) - (double)n * lambda_poisson - dados_;

            covmat = new double[2, 2];
            aicpoisson = 2 * 2 - 2 * loglikpoisson;
            bicpoisson = 2 * loglikpoisson + 2 * Math.Log(dados.GetLength(0));
        }
        
        #endregion
         
        
        #region distribuição Pascal
        
        private void Estimapascal(out double p_pascal, out double[,] covmat, out double aicpascal, out double bicpascal,
            out double loglikpascal, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] < 0.0) throw new Exception("Valores para estimação da distribuição de Pascal devem ser todos não negativos.");
            }

            int n = dados.GetLength(0);

            p_pascal = Math.Pow((double)n,2)/(Math.Pow((double)n,2) + m_clt.Sum(dados)); 

            double dados_=0.0;

            for(int i=0; i< dados.GetLength(0);i++)
            {
                int comb1 = n - 1 + (int)dados[i,0];
                //dados_+=Math.Log(combinacao(comb1,(int)dados[i,0]));
                dados_ += IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln((double)(comb1 + 1))
                            - IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln((double)n - dados[i, 0] + 1)
                            - IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(dados[i, 0] + 1);
            }
            
            loglikpascal = dados_ + Math.Pow((double)n,2) * Math.Log(p_pascal) + m_clt.Sum(dados)*Math.Log(1-p_pascal);

            covmat = new double[2, 2];
            aicpascal = 2 * 2 - 2 * loglikpascal;
            bicpascal = 2 * loglikpascal + 2 * Math.Log(dados.GetLength(0));
        }

        #endregion

        #region distribuição Binomial

        private void Estimabinomial(out double p_binomial, out double[,] covmat, out double aicbinomial, out double bicbinomial,
            out double loglikbinomial, double[,] dados)
        {
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados[i, 0] < 0.0) throw new Exception("Valores para estimação da distribuição binomial devem ser todos não negativos.");
            }

            int n = dados.GetLength(0);

            p_binomial = m_clt.Mean(dados)* 1/(double) n;

            double dados_ = 0.0;

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                //dados_ += Math.Log(combinacao((int)n, (int)dados[i, 0]));
                dados_ += IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln((double)(n + 1)) -
                    IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln((double)n - dados[i, 0] + 1)
                    - IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(dados[i, 0] + 1);
            }
            
            loglikbinomial = dados_ + m_clt.Sum(dados) * Math.Log(p_binomial) + (Math.Pow((double)n,2) - m_clt.Sum(dados)) * Math.Log(1-p_binomial) ;

            covmat = new double[2, 2];
            aicbinomial = 2 * 2 - 2 * loglikbinomial;
            bicbinomial = 2 * loglikbinomial + 2 * Math.Log(dados.GetLength(0));
        }

        #endregion;
    }
}

