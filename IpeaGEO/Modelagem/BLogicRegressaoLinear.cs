using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IpeaGeo.RegressoesEspaciais;
using MathNet.Numerics.Distributions;

namespace IpeaGeo.Modelagem
{
    public class BLogicRegressaoLinear : BLogicBaseModelagem
    {
        public BLogicRegressaoLinear()
            : base()
        {
        }

        #region variáveis internas

        private bool m_correcao_white = false;
        public bool CorrecaoWhite
        {
            get { return m_correcao_white; }
            set { m_correcao_white = value; }
        }

        private bool m_ckb_influentes = false;
        public bool influentes
        {
            get { return m_ckb_influentes; }
            set { m_ckb_influentes = value; }
        }

        private bool m_intercepto = false;
        public bool intercepto
        {
            get { return m_intercepto; }
            set { m_intercepto = value; }          
        }

        #endregion

        #region funções de estimação

        public void EstimarModeloRegressao(ref double[,] erro)
        {
            clsUtilTools clt = new clsUtilTools();

            double[,] X = clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_independentes);
            double[,] y = clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_dependentes);

            if (intercepto)
            {
                double[,] um1 = clt.Ones(X.GetLength(0), 1);
                X = clt.Concateh(um1, X);
            }

            double n = (double)X.GetLength(0);
            double k = (double)X.GetLength(1);

            //Calcula coeficiente beta chapeu (B_hat = (x`x)^-1 . x`y)
            double[,] XtX = clt.MatrizMult(clt.MatrizTransp(X), X); //(x`x)
            double[,] invXtX = clt.MatrizInversa(XtX); //(x`x)^-1
            double[,] Xty = clt.MatrizMult(clt.MatrizTransp(X), y); //(x`x)^-1 . x`y
            m_beta_hat = clt.MatrizMult(invXtX, Xty);

            //Calcula Y_hat, erro e Soma dos Quadrados dos Residuos (SQE)
            double[,] y_hat = clt.MatrizMult(X, m_beta_hat);
            erro = clt.MatrizSubtracao(y, y_hat);
            double SQE = (clt.MatrizMult(clt.MatrizTransp(erro), erro))[0, 0];

            //talvez esteja dando erro na estimação desta variância;
            double sigma2_hat = SQE / (n - k);
            m_beta_hat_cov = clt.MatrizMult(sigma2_hat, invXtX);
            double ybarravalor = clt.Meanc(y)[0, 0];
            double[,] ybarra = new double[X.GetLength(0),1];

            for (int i=0; i<X.GetLength(0);i++)

            {
                ybarra[i,0] = ybarravalor;
            }            

            double[,] M = clt.MatrizSubtracao(y_hat, ybarra);

            bool umavariavel = false;
            if (X.GetLength(1) == 1)
            {
                umavariavel = true;
            }
            else
            {
                umavariavel = false;
            }

            double SQM = (clt.MatrizMult(clt.MatrizTransp(M), M))[0, 0];
                        
            //Correção na soma de quadrados do modelo
            if (umavariavel)
            {
                double correcao = Math.Pow(clt.Mean(y), 2) * y.GetLength(0);
                SQM = SQM +correcao;                
            }

            double SQT = SQE + SQM;
                       
            int GLreg = 0;
            
            if (umavariavel)
            {
                GLreg = X.GetLength(1);
            }
            else
            {
                GLreg = X.GetLength(1) - 1;
            }                      

            int GLerro = X.GetLength(0) - X.GetLength(1);

            int GLtotal = GLreg + GLerro;

            double EPreg = SQM / GLreg;

            double EPerro = SQE / GLerro;

            double Fstatistic = EPreg / EPerro;

            FisherSnedecor fdist = new FisherSnedecor(GLreg, GLerro);
            double pval_Fstatistic = 1.0 - fdist.CumulativeDistribution(Fstatistic);

            double Rsquare = SQM / SQT;

            double AdjustedRsquare = 1 - ((SQE*GLtotal)/(SQT*GLerro));

            string[] nomes = {"Modelo", "Erro", "Total" };

            if (umavariavel)
            {
                nomes[0] = "Modelo*";
                nomes[2] = "Total*";
            }
            
            double[] vetorsomadequadrados = {SQM,SQE,SQT};
            
            double[] vetorgl = {GLreg,GLerro,GLtotal};

            string[] vetorerroPAD = {clt.Double2Texto(EPreg,6),clt.Double2Texto(EPerro,6)," "};

            string[] vetorF = {clt.Double2Texto(Fstatistic,6),"",""};

            string spval_Fstatistic = clt.Double2Texto(pval_Fstatistic,6);

            string[] vetorpvalue = { spval_Fstatistic , "", "" };

            double[,] XinvXtX = clt.MatrizMult(X, invXtX);

            double[,] Hatmatrix = clt.MatrizMult(XinvXtX, clt.MatrizTransp(X));

            #region testes de detecção de outliers e pontos influentes




                #region resíduo Studentizado

                double[,] studentizedresidual = new double[Hatmatrix.GetLength(0), 1];
                double[,] varEi = new double[Hatmatrix.GetLength(0), 1];

                for (int i = 0; i < Hatmatrix.GetLength(0); i++)
                {
                    varEi[i, 0] = sigma2_hat * (1.0 - Hatmatrix[i, i]);
                    studentizedresidual[i, 0] = erro[i, 0] / varEi[i, 0];
                }



                #endregion

                #region D de Cook

                double[,] CookD = new double[Hatmatrix.GetLength(0), 1];

                for (int i = 0; i < Hatmatrix.GetLength(0); i++)
                {
                    CookD[i, 0] = ((Math.Pow(erro[i, 0], 2)) / X.GetLength(1) * EPerro) * (Hatmatrix[i, i] / Math.Pow((1.0 - Hatmatrix[i, i]), 2));
                }

                #endregion

                #region DFFITS

                double[,] DFFITS = new double[Hatmatrix.GetLength(0), 1];

                for (int i = 0; i < Hatmatrix.GetLength(0); i++)
                {
                    DFFITS[i, 0] = studentizedresidual[i, 0] * (Math.Sqrt(Hatmatrix[i, i] / (1.0 - Hatmatrix[i, i])));
                }

                #endregion

                #region Leverage ou hii

                double[,] hii = new double[Hatmatrix.GetLength(0), 1];

                for (int i = 0; i < Hatmatrix.GetLength(0); i++)
                {
                    hii[i, 0] = Hatmatrix[i, i];
                }

                #endregion

           

            #endregion
            
            #region testes de heterocedasticidade: White e Breusch–Pagan

            #region trecho antigo - correção de White

            //double[,] ywhite = clt.MatrizDotMult(erro, erro);

            //Xty = clt.MatrizMult(clt.MatrizTransp(X), ywhite);

            //double[,] beta_hat_White = clt.MatrizMult(invXtX, Xty);

            //double[,] y_hatwhite = clt.MatrizMult(X, beta_hat_White);

            //double[,] errowhite = clt.MatrizSubtracao(ywhite, y_hat);

            //double ybarravalorwhite = clt.Meanc(ywhite)[0, 0];

            //double[,] ybarrawhite = new double[X.GetLength(0), 1];

            //for (int i = 0; i < X.GetLength(0); i++)
            //{
            //    ybarrawhite[i, 0] = ybarravalorwhite;
            //}

            //double[,] Mwhite = clt.MatrizSubtracao(y_hatwhite, ybarrawhite);

            //double SQEwhite = (clt.MatrizMult(clt.MatrizTransp(errowhite), errowhite))[0, 0];

            //double SQMwhite = (clt.MatrizMult(clt.MatrizTransp(Mwhite), Mwhite))[0, 0];

            //double SQTwhite = SQEwhite + SQMwhite;

            //int GLregwhite = GLreg;

            //int GLerrowhite = GLerro;

            //int GLtotalwhite = GLregwhite + GLerrowhite;

            //double EPregwhite = SQMwhite / GLregwhite;

            //double EPerrowhite = SQEwhite / GLerrowhite;

            //double Fstatisticwhite = EPregwhite / EPerrowhite;

            //double pval_Fstatisticwhite = 1.0 - fdist.cdf(Fstatisticwhite);

            //double Rsquarewhite = SQMwhite / SQTwhite;

            //double BreuschPaganteste = X.GetLength(0) * Rsquarewhite;

            //MathChisqdist chi2 = new MathChisqdist((double)(GLreg));
            //double pval_BreuschPagan = 1.0 - chi2.cdf(BreuschPaganteste);

            #endregion 

            double White_stat1, White_pvalue1, White_stat2, 
                    White_pvalue2, White_stat3, White_pvalue3,
                    BreuschPaganteste1, BreuschPaganteste2, BreuschPaganteste3,
                    BreuschPaganpvalue1, BreuschPaganpvalue2, BreuschPaganpvalue3;

            this.TesteWhite(erro, out White_stat1, out White_pvalue1, out White_stat2, 
                    out White_pvalue2, out White_stat3, out White_pvalue3,
                    out BreuschPaganteste1, out BreuschPaganteste2, out BreuschPaganteste3,
                    out BreuschPaganpvalue1, out BreuschPaganpvalue2, out BreuschPaganpvalue3);

            #region aplicando a correção de white

                if (m_correcao_white == true)

            { 
                double[,] matriztempoerros = clt.MatrizMult(erro,clt.MatrizTransp(erro)); 
                double[,] matrizdiagonalerro = clt.Zeros(matriztempoerros.GetLength(0), matriztempoerros.GetLength(1)); 
                for (int i=0; i<matrizdiagonalerro.GetLength(0); i++)
                {
                matrizdiagonalerro[i,i]=matriztempoerros[i,i];
                }

                double[,] temp1=clt.MatrizMult(clt.MatrizTransp(X),matrizdiagonalerro);
                double[,] temp2=clt.MatrizMult(temp1,X);
                double[,] temp3=clt.MatrizMult(invXtX,temp2);
                m_beta_hat_cov = clt.MatrizMult(temp3,invXtX);
            
            }


            #endregion
            
            #endregion

            #region Testes de autocorrelação dos resíduos: Durbin–Watson statistic

            double[,] d1 = new double[X.GetLength(0)-1,1];

            for (int i = 1; i < X.GetLength(0); i++)
            {
                d1[i - 1,0] = erro[i,0] - erro[i - 1,0];
            }

            double sumd1 = (clt.MatrizMult(clt.MatrizTransp(d1), d1))[0, 0];

            /* Cálculo do Durbin-Watson , pode dar erro quando a soma dos quadrados dos erros for 0*/

            double d = sumd1 / SQE;

            #endregion
            
            #region variveis globais

            this.GeraSignificanciaCoeficientes();

            m_sigma2_hat = sigma2_hat;

            #endregion

            #region testes de normalidade do resíduo

            double andersondarlingstatistic = this.normalitytest_andersondarling(erro);
            double kolmogorovstatistic = this.normalitytest_kolmogorov_Smirnov(erro);


            #endregion
            
            
            
            #region testes para comparação de modelo

            double aux_loglike = 0.0;
            double loglike = 0.0;
            double AIC = 0.0;
            double AICc = 0.0;
            double BIC = 0.0;

            for (int i = 0; i < n; i++)
            {
                aux_loglike += Math.Pow(erro[i,0], 2.0);
            
            }

            loglike = -(n / 2.0) * (Math.Log(2 * Math.PI) + 1) - (n / 2.0) * (Math.Log(aux_loglike / n));
            AIC = -2.0 * loglike + 2.0 * X.GetLength(1);
            AICc = -2.0 * loglike + ((2.0 * X.GetLength(1) * n) / (n - X.GetLength(1) - 1.0));
            BIC = -2.0 * loglike + X.GetLength(1) * Math.Log(n);

            #endregion
                        
            #region gerando o output para resultado das estimações

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação via OLS (Mínimos Quadrados Ordinários) \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Variância dos erros: " + clt.Double2Texto(m_sigma2_hat, 6) + "\n";           
            out_text += "\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, m_beta_hat, m_beta_stderror, m_beta_tstat, m_beta_pvalor, intercepto);                   

            out_text += "\n";

            out_text += GeraANOVA(nomes, vetorsomadequadrados, vetorgl, vetorerroPAD, vetorF, vetorpvalue);

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";

                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(m_beta_hat_cov, VariaveisIndependentes, m_usa_intercepto);
            }

            if (umavariavel)
            {
                out_text += "\n * O grau de liberdade do Total e da Regressão não estão com correção de viés.";
            }

            out_text += "\n";

            out_text += "\n=================== Análise de Ajuste do Modelo\n\n";

            out_text += "R² (coeficiente de determinação): " + clt.Double2Texto(Rsquare, 6) + "\n";
            out_text += "R²-ajustado: " + clt.Double2Texto(AdjustedRsquare, 6) + "\n";
            out_text += "AIC: " + clt.Double2Texto(AIC, 6) + "\n";
            out_text += "AICC: " + clt.Double2Texto(AICc, 6) + "\n";
            out_text += "BIC: " + clt.Double2Texto(BIC, 6) + "\n";
            out_text += "Log-likelihood: " + clt.Double2Texto(loglike, 6) + "\n";
            out_text += "Durbin-Watson: " + clt.Double2Texto(d, 6) + "\n";
            
            out_text += "\n=================== Teste de White para Homocedasticidade dos Resíduos (sem termos quadráticos ou produtos cruzados) \n\n";
            out_text += "Estatística do teste de White (teste F): " + clt.Double2Texto(White_stat1, 6) + "\n";
            out_text += "P-valor do teste de White (teste F): " + clt.Double2Texto(White_pvalue1, 6) + "\n";
            out_text += "Estatística do teste de Breusch–Pagan (teste qui-quadrado): " + clt.Double2Texto(BreuschPaganteste1, 6) + "\n";
            out_text += "P-valor do teste de Breusch–Pagan (teste qui-quadrado): " + clt.Double2Texto(White_pvalue1, 6) + "\n";

            out_text += "\n=================== Teste de White para Homocedasticidade dos Resíduos (com termos quadráticos) \n\n";
            out_text += "Estatística do teste de White (teste F): " + clt.Double2Texto(White_stat2, 6) + "\n";
            out_text += "P-valor do teste de White (teste F): " + clt.Double2Texto(White_pvalue2, 6) + "\n";
            out_text += "Estatística do teste de Breusch–Pagan (teste qui-quadrado): " + clt.Double2Texto(BreuschPaganteste2, 6) + "\n";
            out_text += "P-valor do teste de Breusch–Pagan (teste qui-quadrado): " + clt.Double2Texto(White_pvalue2, 6) + "\n";

            out_text += "\n=================== Teste de White para Homocedasticidade dos Resíduos (com termos quadráticos e produtos cruzados) \n\n";
            out_text += "Estatística do teste de White (teste F): " + clt.Double2Texto(White_stat3, 6) + "\n";
            out_text += "P-valor do teste de White (teste F): " + clt.Double2Texto(White_pvalue3, 6) + "\n";
            out_text += "Estatística do teste de Breusch–Pagan (teste qui-quadrado): " + clt.Double2Texto(BreuschPaganteste3, 6) + "\n";
            out_text += "P-valor do teste de Breusch–Pagan (teste qui-quadrado): " + clt.Double2Texto(White_pvalue3, 6) + "\n";

            //out_text += "Estatística do teste de White: " + clt.Double2Texto(Fstatisticwhite, 6) + "\n";
            //out_text += "P-valor do teste de White: " + clt.Double2Texto(pval_Fstatisticwhite, 6) + "\n";
            //out_text += "Estatística do teste de Breusch–Pagan " + clt.Double2Texto(BreuschPaganteste, 6) + "\n";
            //out_text += "P-valor do teste de Breusch–Pagan: " + clt.Double2Texto(pval_BreuschPagan, 6) + "\n";

            out_text += "\n=================== Testes de Normalidade dos Resíduos\n\n";
            out_text += "Estatística de Anderson-Darling: " + clt.Double2Texto(andersondarlingstatistic, 6) + "\n";
            out_text += "Estatística de Kolmogorov-Smirnov: " + clt.Double2Texto(kolmogorovstatistic, 6) + "\n";
            
            this.m_output_text = out_text;

            #endregion

            #region adicionando variveis base de dados

            double[,] observacoes = new double[X.GetLength(0), 1];

            for (int i = 0; i < n; i++)
            {
                observacoes[i, 0] = (double)i;
            }

            double[,] variaveis_geradas = clt.Concateh(observacoes, clt.Concateh(y, clt.Concateh(y_hat, erro)));

            if (m_ckb_influentes)
            {
                variaveis_geradas = clt.Concateh(variaveis_geradas, studentizedresidual);

                variaveis_geradas = clt.Concateh(variaveis_geradas, CookD);

                variaveis_geradas = clt.Concateh(variaveis_geradas, DFFITS);

                variaveis_geradas = clt.Concateh(variaveis_geradas, hii);
            }

            string[] nomes_variaveis = new string[0];

            if (m_ckb_influentes)
            {
                nomes_variaveis = new string[8];
            }
            else
            {
                nomes_variaveis = new string[4];
            }


            nomes_variaveis[0] = "Observacao_";

            nomes_variaveis[1] = "Y_observado_";

            nomes_variaveis[2] = "Y_predito_";

            nomes_variaveis[3] = "Residuo_";

            if (m_ckb_influentes)
            {

                nomes_variaveis[4] = "Resíduo Studentizado";

                nomes_variaveis[5] = "D de Cook";

                nomes_variaveis[6] = "DFFITS";

                nomes_variaveis[7] = "Leverage (hii)";

            }

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação via OLS\n\n";

            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";

            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);

            AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);

            #endregion                                        
        }

        public void TesteWhite(double[,] erro, out double White_stat1, out double White_pvalue1, out double White_stat2, 
                    out double White_pvalue2, out double White_stat3, out double White_pvalue3,
                    out double BreuschPaganteste1, out double BreuschPaganteste2, out double BreuschPaganteste3,
                    out double BreuschPaganpvalue1, out double BreuschPaganpvalue2, out double BreuschPaganpvalue3)
        {
            White_stat1 = 0.0;
            White_pvalue1 = 0.0;
            White_stat2 = 0.0;
            White_pvalue2 = 0.0;
            White_stat3 = 0.0;
            White_pvalue3 = 0.0;

            BreuschPaganteste1 = 0.0;
            BreuschPaganteste2 = 0.0;
            BreuschPaganteste3 = 0.0;

            BreuschPaganpvalue1 = 0.0;
            BreuschPaganpvalue2 = 0.0;
            BreuschPaganpvalue3 = 0.0;

            clsUtilTools clt = new clsUtilTools();

            double[,] Xteste1 = clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_independentes);

            CalculaTesteWhite(erro, Xteste1, ref White_stat1, ref White_pvalue1, ref BreuschPaganteste1, ref BreuschPaganpvalue1);

            double[,] Xteste2 = new double[Xteste1.GetLength(0), Xteste1.GetLength(1)];
            for (int i = 0; i < Xteste1.GetLength(0); i++)
            {
                for (int j = 0; j < Xteste1.GetLength(1); j++)
                {
                    Xteste2[i, j] = Math.Pow(Xteste1[i, j], 2.0);
                }
            }

            double[,] Xteste12 = clt.Concateh(Xteste1, Xteste2);

            CalculaTesteWhite(erro, Xteste12, ref White_stat2, ref White_pvalue2, ref BreuschPaganteste2, ref BreuschPaganpvalue2);

            int coluna = 0;

            double[,] Xteste3 = new double[Xteste1.GetLength(0), Xteste1.GetLength(1) * (Xteste1.GetLength(1) - 1) / 2];
            for (int i = 0; i < Xteste1.GetLength(0); i++)
            {
                coluna = 0;
                for (int j = 0; j < Xteste1.GetLength(1); j++)
                {
                    for (int k = j+1; k < Xteste1.GetLength(1); k++)
                    {
                        Xteste3[i, coluna] = Xteste1[i, j] * Xteste1[i, k];
                        coluna++;
                    }
                }
            }

            double[,] Xteste123 = clt.Concateh(Xteste12, Xteste3);

            CalculaTesteWhite(erro, Xteste123, ref White_stat3, ref White_pvalue3, ref BreuschPaganteste3, ref BreuschPaganpvalue3);
        }

        private void CalculaTesteWhite(double[,] erro, double[,] Xteste, ref double White_stat, ref double White_pvalue, ref double BreuschPaganteste,
                    ref double BreuschPaganpvalue)
        {
            White_stat = 0.0;
            White_pvalue = 0.0;
            BreuschPaganteste = 0.0;
            BreuschPaganpvalue = 0.0;

            clsUtilTools clt = new clsUtilTools();

            double[,] X = clt.Concateh(clt.Ones(Xteste.GetLength(0), 1), Xteste);

            double[,] XtX = clt.MatrizMult(clt.MatrizTransp(X), X);
            double[,] invXtX = clt.MatrizInversa(XtX);

            double[,] ywhite = clt.MatrizDotMult(erro, erro);

            double[,] Xty = clt.MatrizMult(clt.MatrizTransp(X), ywhite);

            double[,] beta_hat_White = clt.MatrizMult(invXtX, Xty);

            double[,] y_hatwhite = clt.MatrizMult(X, beta_hat_White);

            double[,] errowhite = clt.MatrizSubtracao(ywhite, y_hatwhite);

            double ybarravalorwhite = clt.Meanc(ywhite)[0, 0];

            double[,] ybarrawhite = new double[X.GetLength(0), 1];

            for (int i = 0; i < X.GetLength(0); i++)
            {
                ybarrawhite[i, 0] = ybarravalorwhite;
            }

            double[,] Mwhite = clt.MatrizSubtracao(y_hatwhite, ybarrawhite);

            double SQEwhite = (clt.MatrizMult(clt.MatrizTransp(errowhite), errowhite))[0, 0];

            double SQMwhite = (clt.MatrizMult(clt.MatrizTransp(Mwhite), Mwhite))[0, 0];

            double SQTwhite = SQEwhite + SQMwhite;

            int GLregwhite = X.GetLength(1) - 1;

            int GLerrowhite = X.GetLength(0) - (X.GetLength(1));

            int GLtotalwhite = GLregwhite + GLerrowhite;

            double EPregwhite = SQMwhite / GLregwhite;

            double EPerrowhite = SQEwhite / GLerrowhite;

            White_stat = EPregwhite / EPerrowhite;

            FisherSnedecor fdist = new FisherSnedecor((double)GLregwhite, (double)GLerrowhite);
            White_pvalue = 1.0 - fdist.CumulativeDistribution(White_stat);

            double Rsquarewhite = SQMwhite / SQTwhite;

            BreuschPaganteste = X.GetLength(0) * Rsquarewhite;

            ChiSquared chi2 = new ChiSquared((double)(GLregwhite));
            BreuschPaganpvalue = 1.0 - chi2.CumulativeDistribution(BreuschPaganteste);
        }

        #endregion

        # region teste de normalidade Anderson-Darling

        /// <summary>
        /// Retorna a Estatística do teste de normalidade Anderson-Darling
        /// </summary>
        /// <param name="variavelteste">Variável a ser testada, entrar com um vetor no formato matriz</param>
        /// <returns></returns>
        private double normalitytest_andersondarling(double[,] variavelteste)
        {

            clsUtilTools clt = new clsUtilTools();
            Normal normdist = new Normal(0, 1);


            double AD = new double();

            variavelteste = clt.SortcDoubleArray(variavelteste);
            double xbarra = clt.Meanc(variavelteste)[0, 0];
            double desvio = clt.Despadc(variavelteste)[0, 0];
            double[,] Yi = new double[variavelteste.GetLength(0), 1];
            double ad1 = new double();
            double nd1 = 0.0;
            double nd2 = 0.0;

            //for (int i = 0; i < variavelteste.GetLength(0); i++)
            //{
            //    Yi[i, 0] = (variavelteste[i, 0] - xbarra) / desvio;
            //}
 
            for (int i = 1; i < variavelteste.GetLength(0) + 1; i++)
            {
                Yi[i-1, 0] = (variavelteste[i-1, 0] - xbarra) / desvio;
                Yi[variavelteste.GetLength(0) - i, 0] = (variavelteste[variavelteste.GetLength(0) - i, 0] - xbarra) / desvio;

                nd1 = normdist.CumulativeDistribution(Yi[i - 1, 0]);
                nd2 = Yi[variavelteste.GetLength(0) - i, 0];

                ad1 += ((double)(2 * i) - 1.0) * ((Math.Log(nd1)) + Math.Log(1.0 - (normdist.CumulativeDistribution(nd2))));
            }



            AD = -Yi.GetLength(0) - (1.0 / Yi.GetLength(0)) * ad1;

            return (AD);
        }

        #endregion

        # region teste de normalidade Komogorov-Smirnov

        /// <summary>
        /// Retorna a Estatística do teste de normalidade Kolmogorov-Smirnov
        /// </summary>
        /// <param name="variavelteste">Variável a ser testada, entrar com um vetor no formato matriz</param>
        /// <returns></returns>
        private double normalitytest_kolmogorov_Smirnov(double[,] variavelteste)
        {

            clsUtilTools clt = new clsUtilTools();

            double[,] D = new double[variavelteste.GetLength(0),1];


            variavelteste = clt.SortcDoubleArray(variavelteste);
            double xbarra = clt.Meanc(variavelteste)[0, 0];
            double desvio = clt.Despadc(variavelteste)[0, 0];

            Normal normdist = new Normal(xbarra, desvio);

            double[,] Yi = new double[variavelteste.GetLength(0), 1];
            bool f = false;
            double m = new double();
            double ad1 = new double();
            double[,] norm = new double[variavelteste.GetLength(0), 1];

            for (int i = 0; i < variavelteste.GetLength(0)-1; i++)
            {
                m = variavelteste[i, 0];
                int j = 0;
                do
                {
                    j++;
                } while (variavelteste[i+j, 0] <= m);
                for (int k=0; k<j;k++)    
                {
                    Yi[i+k, 0] = ((double)(j+i) / (double)variavelteste.GetLength(0)); 
                }
                i += j-1;
            }

            Yi[variavelteste.GetLength(0) - 1, 0] = 1;

            for (int i = 0; i < variavelteste.GetLength(0); i++)
            {
                norm[i, 0] = normdist.CumulativeDistribution(variavelteste[i, 0]);
                D[i,0] = Math.Abs(Yi[i, 0] - norm[i, 0]);
            }

            D = clt.SortcDoubleArray(D);
 
 
            return (D[D.GetLength(0)-1,0]);
        }

        #endregion
    }
}
