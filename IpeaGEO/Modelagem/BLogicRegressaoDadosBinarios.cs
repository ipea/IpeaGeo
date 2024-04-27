using System;

using IpeaGeo.RegressoesEspaciais;
using MathNet.Numerics.Distributions;

namespace IpeaGeo.Modelagem
{
    #region enumerações

    public enum TipoFuncaoLigacao : int
    {
        Logaritmo,
        Logit,
        Cloglog,
        Probit,
        Identidade
    };

    #endregion

    class BLogicRegressaoDadosBinarios : BLogicBaseModelagem
    {
        public BLogicRegressaoDadosBinarios()
        {
        }

        private double[,] m_prob_hat = new double[0, 0];
        public double[,] ProbabilidadePredita
        {
            get { return m_prob_hat; }
        }

        private double m_erro = 0.999;
        public double erro
        {
            set { this.m_erro = value; }
        }

        private bool classificationchecked = false;
        public bool mclassificationtablechecked
        {
            get { return classificationchecked; }
            set { classificationchecked = value; }
        }

        private bool influencechecked = false;
        public bool minfluencechecked
        {
            set { influencechecked = value; }
        }

        private bool residuoscheked = false;
        public bool mresiduoschecked
        {
            set { residuoscheked = value; }
        }

        private double cutoffCT = new double();
        public double mcutoffCT
        {
            get { return cutoffCT; }
            set { cutoffCT = value; }
        }

        clsUtilTools m_clt = new clsUtilTools();


        protected TipoFuncaoLigacao m_tipo_funcao_ligacao = TipoFuncaoLigacao.Logit;
        public TipoFuncaoLigacao FuncaoLigacao
        {
            set { m_tipo_funcao_ligacao = value; }
        }

        public string ChecaDadosBinarios()
        {
            string mensagem = "";
            double[,] y = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_dependentes);
            object[,] yo = new object[y.GetLength(0), 1];
            for (int i = 0; i<yo.GetLength(0); i++)
            {
                yo[i,0] = y[i,0];
            }
            if (!m_clt.ChecaLimiteCategorias(2, yo))
            {
                mensagem = "Variável resposta escolhida não é binária. Deseja prosseguir definindo dados binários, utilizando a média como valor de corte?";
            }
            return mensagem;
        }

        public void ChecaTodosValoresIguais()
        {
            double[,] y = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_dependentes);
            object[,] yo = new object[y.GetLength(0), 1];
            for (int i = 0; i < yo.GetLength(0); i++)
            {
                yo[i, 0] = y[i, 0];
            }
            if (m_clt.ChecaLimiteCategorias(1, yo))
            {
                throw new Exception("Todas os valores para a variavel resposta sao identicos. Escolha outra variavel resposta.");
            }
        }

        double dividir(double a, double b, double c)
        {
            double div;
            div = ((a * b) / c);
            if (!Double.IsInfinity(div) && !Double.IsNaN(div))
                return div;

            if (c < m_erro)
            {
                if (a < m_erro)
                    return b;
                if (b < m_erro)
                    return a;
                if ((a * b) < m_erro)
                    return 1.0;
                return ((a*b) / m_erro);
            }

            return ((a * b) / c);
        }

        protected double[,] RespostaBinaria(double[,] y)
        {
            double corte = (m_clt.Mean(y));
            double[,] res = new double[y.GetLength(0), 1];
            for (int i = 0; i < y.GetLength(0); i++)
            {
                if (y[i, 0] >= corte) res[i, 0] = 1.0;
                else res[i, 0] = 0.0;
            }
            return res;
        }

        #region funções de estimação para regressão

        public void EstimarModeloRegressao()
        {
            clsUtilTools clt = new clsUtilTools();

            double[,] X = clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_independentes);
            double[,] y = clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_dependentes);

            y = RespostaBinaria(y);

            double n = (double)X.GetLength(0);
            double k = (double)X.GetLength(1);

            m_nobs = X.GetLength(0);

            if (m_usa_intercepto)
            {
                double[,] um1 = clt.Ones(X.GetLength(0), 1);
                X = clt.Concateh(um1, X);
            }

            #region iterações para convergência

            double[,] beta_old = new double[X.GetLength(1), 1];
            double[,] beta_new = new double[X.GetLength(1), 1];

            double[,] z = new double[m_nobs, 2];
            double[,] pi = new double[m_nobs, 2];
            for (int i = 0; i < m_nobs; i++)
            {
                if (y[i, 0] == 1.0)
                {
                    z[i, 0] = 1.0;
                    z[i, 1] = 0.0;
                }
                else
                {
                    z[i, 0] = 0.0;
                    z[i, 1] = 1.0;
                }
            }
            
            int iter = 0;
            double[,] xi;
            double[,] Di;
            double[,] Wi = new double[2, 2];
            double[,] aux1 = new double[X.GetLength(1), X.GetLength(1)];
            double[,] aux2 = new double[X.GetLength(1), 1];
            double[,] zi = new double[1, 2];
            double[,] pi_i = new double[1, 2];
            double[,] delta;

            for (iter = 0; iter < m_max_iterations_till_convergence; iter++)
            {
                aux1 = new double[X.GetLength(1), X.GetLength(1)];
                aux2 = new double[X.GetLength(1), 1];
                
                for (int i = 0; i < m_nobs; i++)
                {
                    xi = m_clt.MatrizTransp(m_clt.SubRowArrayDouble(X, i));

                    switch (m_tipo_funcao_ligacao)
                    {
                        case TipoFuncaoLigacao.Logit:
                            pi[i, 0] = logit(xi, beta_old);
                            Di = diff_logit(xi, beta_old);
                            break;
                        case TipoFuncaoLigacao.Cloglog:
                            pi[i, 0] = cloglog(xi, beta_old);
                            Di = diff_cloglog(xi, beta_old);
                            break;
                        case TipoFuncaoLigacao.Probit:
                            pi[i, 0] = probit(xi, beta_old);
                            Di = diff_probit(xi, beta_old);
                            break;
                        default:
                            pi[i, 0] = logit(xi, beta_old);
                            Di = diff_logit(xi, beta_old);
                            break;
                    }

                        double[,] dAux1 = new double[Di.GetLength(0),Di.GetLength(0)];
                        double[,] dAux2 = new double[Di.GetLength(0),1];

                        int ii, jj;
                        for (ii = 0; ii < Di.GetLength(0); ii++)
                            for (jj = 0; jj < Di.GetLength(0); jj++)
                                dAux1[ii, jj] = dividir(Di[ii, 0], Di[jj, 0], pi[i, 0]) + dividir(Di[ii, 0], Di[jj, 0], (1-pi[i, 0]));

                        for (ii = 0; ii < Di.GetLength(0); ii++)
                            dAux2[ii, 0] = dividir(Di[ii, 0], (z[i, 0] - pi[i, 0]), pi[i, 0]) + dividir(Di[ii, 0], (z[i, 0] - pi[i, 0]), (1 - pi[i, 0]));

                        aux1 = m_clt.MatrizSoma(aux1, dAux1);
                        aux2 = m_clt.MatrizSoma(aux2, dAux2);
                 
                }
                
                aux1 = m_clt.MatrizInversa(aux1);
                delta = m_clt.MatrizMult(aux1, aux2);


                beta_new = m_clt.MatrizSoma(beta_old, delta);

                for (int i = 0; i < beta_new.GetLength(0); i++)
                {
                    if (Double.IsNaN(beta_new[i, 0])
                        || Double.IsInfinity(beta_new[i, 0])
                        || Double.IsNegativeInfinity(beta_new[i, 0])
                        || Double.IsPositiveInfinity(beta_new[i, 0]))
                    {
                        this.m_tipo_mensagem_regressao = TiposMensagemRegressao.BetasNull;
                        this.m_mensagem_regressao = "O processo de iterações falhou."; 
                        this.m_mensagem_regressao += " O betas resultaram em valores inválidos.";
                        this.m_mensagem_regressao += " Verique a sua base de dados ou problemas de multicolinearidade.";
                        break;
                    }
                }

                if (m_clt.Norm(m_clt.MatrizSubtracao(beta_new, beta_old)) < m_tol_iterate_till_convergence)
                {
                    break;
                }
                beta_old = m_clt.ArrayDoubleClone(beta_new);
            }

            #endregion

            m_beta_hat = beta_new;
            m_beta_hat_cov = aux1;
            this.GeraSignificanciaCoeficientes();

            double[,] prob_hat = new double[m_nobs, 1];
            for (int i = 0; i < m_nobs; i++)
            {
                xi = m_clt.MatrizTransp(m_clt.SubRowArrayDouble(X, i));
                switch (m_tipo_funcao_ligacao)
                {
                    case TipoFuncaoLigacao.Logit:
                        prob_hat[i, 0] = logit(xi, m_beta_hat);
                        break;
                    case TipoFuncaoLigacao.Cloglog:
                        prob_hat[i, 0] = cloglog(xi, m_beta_hat);
                        break;
                    case TipoFuncaoLigacao.Probit:
                        prob_hat[i, 0] = probit(xi, m_beta_hat);
                        break;
                    default:
                        prob_hat[i, 0] = logit(xi, m_beta_hat);
                        break;
                }
            }

            m_prob_hat = prob_hat;

            #region Analise de Residuos

            double[,] residuos = new double[X.GetLength(0),1];
            double[,] residuoslogit = new double[X.GetLength(0),1];
            double[,] residuospadronizados = new double[X.GetLength(0),1];
            double mediares; double desviores;

            if (residuoscheked)
            { 
                for (int i=0; i<X.GetLength(0);i++)
                {
                    residuos[i,0] = y[i,0] - prob_hat[i,0];
                    residuoslogit[i,0] = (residuos[i,0]) / (prob_hat[i,0] * (1 - prob_hat[i,0]));
                }

                mediares = clt.Meanc(residuos)[0,0];
                desviores = clt.Despadc(residuos)[0,0];

                for (int i = 0; i < X.GetLength(0); i++)
                {
                    residuospadronizados[i, 0] = (residuos[i, 0] - mediares) / (desviores);
                }

            }

            #endregion
            
            #region Pontos Influentes

            double SQE = (clt.MatrizMult(clt.MatrizTransp(residuos), residuos))[0, 0];

            int GLerro = X.GetLength(0) - X.GetLength(1);

            double sigma2_hat = SQE / GLerro;

            double[,] XtX = clt.MatrizMult(clt.MatrizTransp(X), X);
            double[,] invXtX = clt.MatrizInversa(XtX);

            double[,] XinvXtX = clt.MatrizMult(X, invXtX);

            double[,] Hatmatrix = clt.MatrizMult(XinvXtX, clt.MatrizTransp(X));


            #region resíduo Studentizado

            double[,] studentizedresidual = new double[Hatmatrix.GetLength(0), 1];
            double[,] varEi = new double[Hatmatrix.GetLength(0), 1];

            for (int i = 0; i < Hatmatrix.GetLength(0); i++)
            {
                varEi[i, 0] = sigma2_hat * (1.0 - Hatmatrix[i, i]);
                studentizedresidual[i, 0] = residuos[i, 0] / varEi[i, 0];
            }
            #endregion

            #region D de Cook

            double[,] CookD = new double[Hatmatrix.GetLength(0), 1];

            for (int i = 0; i < Hatmatrix.GetLength(0); i++)
            {
                CookD[i, 0] = ((Math.Pow(residuos[i, 0], 2)) / X.GetLength(1) * sigma2_hat) * (Hatmatrix[i, i] / Math.Pow((1.0 - Hatmatrix[i, i]), 2));
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

            #region SAS Model Fitting Information

            #region -2 Log Likelihood

            double loglikelihood= new double();

            for (int j=0 ; j<X.GetLength(0);j++)
            {
                loglikelihood += y[j,0]*Math.Log(prob_hat[j, 0]) + (1.0 - y[j,0])*Math.Log(1.0 - prob_hat[j,0]);
            }

            double menosdoisloglikelihood = -2.0 * loglikelihood;

            double AIC = menosdoisloglikelihood + 2.0 * ((double)X.GetLength(1));

            double SC = menosdoisloglikelihood + (double)X.GetLength(1) * Math.Log((double)X.GetLength(0));




            #endregion







            #endregion;

            #region classification table

            #region função para geração da tabela de classificação       
            
            double[,] ypred = new double[m_prob_hat.GetLength(0), 1];
            double[,] class_table_count = new double[2, 4];
            string[] nomesvarclasstable = new string[class_table_count.GetLength(1)];

            if (mclassificationtablechecked == true)
            {
               for (int i=0; i<m_prob_hat.GetLength(0); i++)
               {
                   if (m_prob_hat[i, 0] <= mcutoffCT)
                   {
                       ypred[i, 0] = 0.0;
                       if (y[i, 0] <= 0.5) class_table_count[0, 1]++;
                       else class_table_count[1, 1]++;
                   }
                   else
                   {
                       ypred[i, 0] = 1.0;
                       if (y[i, 0] <= 0.5) class_table_count[0, 2]++;
                       else class_table_count[1, 2]++;
                   }              
               }
                class_table_count[0, 3] = ((double)class_table_count[0, 1]) /((double)(class_table_count[0, 1] + class_table_count[0, 2]));
                class_table_count[1, 3] = (double)class_table_count[1, 2] / (double)(class_table_count[1, 1] + class_table_count[1, 2]);

                class_table_count[0, 0] = 0;
                class_table_count[1, 0] = 1;

                nomesvarclasstable[0] = "Valor";
                nomesvarclasstable[1] = "0";
                nomesvarclasstable[2] = "1";
                nomesvarclasstable[3] = "Percentual de Acerto";

            }

            double[,] oddsratio = new double[X.GetLength(1),1];
            
            for (int i = 0; i < X.GetLength(1); i++)
            {
                oddsratio[i,0] = Math.Exp(m_beta_hat[i,0]);
            }
            #endregion

            #endregion
                 
            #region gerando o output para resultado das estimaes

            string out_text = "============================================================================================================================\n\n";

            string nome_funcao_ligacao = "Logit";
            if (m_tipo_funcao_ligacao == TipoFuncaoLigacao.Cloglog) nome_funcao_ligacao = "Complementary log-log";
            if (m_tipo_funcao_ligacao == TipoFuncaoLigacao.Probit) nome_funcao_ligacao = "Probit";

            out_text += "Estimação via Máxima Verossimilhança de Regressão com Dados Binários \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Função de ligação: " + nome_funcao_ligacao + "\n";
            out_text += "\n";

            out_text += GeraTabelaEstimacoesBinaryLogistic(VariaveisIndependentes,
                m_beta_hat, m_beta_stderror, m_beta_tstat, m_beta_pvalor,oddsratio, m_usa_intercepto);

            out_text += "\n";

            if (mclassificationtablechecked == true)
            {
                out_text += "=================== Tabela de Classificação - Valores Observados x Valores Preditos\n\n";

                out_text += GeraTabelaNovasVariaveis(class_table_count,nomesvarclasstable);
                
                out_text += "\n";
            }

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de varincia-covarincia do estimador do vetor de coeficientes: \n\n";
                out_text += this.GeraTabelaCovMatrix(m_beta_hat_cov, VariaveisIndependentes, m_usa_intercepto);
            }

            out_text += "=================== Análise de Ajuste do Modelo\n\n";

            out_text += "AIC: " + clt.Double2Texto(AIC, 6) + "\n";
            out_text += "Schwarz Criterion (SC): " + clt.Double2Texto(SC, 6) + "\n";
            out_text += "-2 Log-likelihood: " + clt.Double2Texto(menosdoisloglikelihood, 6) + "\n\n";

            out_text += ChecaAjusteModelosBinarios(y, prob_hat);

            this.m_output_text = out_text;

            #endregion

            #region adicionando variveis base de dados

            double[,] observacoes = new double[X.GetLength(0), 1];

            for (int i = 0; i < n; i++)
            {
                observacoes[i, 0] = (double)i;
            }

            double[,] variaveis_geradas = clt.Concateh(observacoes, clt.Concateh(y, prob_hat));


            string[] nomes_variaveis = new string[3];


            if (residuoscheked)
            {
                variaveis_geradas = clt.Concateh(variaveis_geradas, clt.Concateh(residuos, clt.Concateh(residuoslogit, residuospadronizados)));

                if (influencechecked)
                {
                    variaveis_geradas = clt.Concateh(variaveis_geradas, clt.Concateh(CookD, clt.Concateh(DFFITS, hii)));
                    string[] nomes_variaveis3 = new string[9];
                    nomes_variaveis3[0] = "Observacao_";
                    nomes_variaveis3[1] = "Y_observado_";
                    nomes_variaveis3[2] = "Prob_predita_";
                    nomes_variaveis3[3] = "Resíduo";
                    nomes_variaveis3[4] = "Resíduo Logit";
                    nomes_variaveis3[5] = "Resíduo Padronizado";
                    nomes_variaveis3[6] = "D de Cook";
                    nomes_variaveis3[7] = "DFFITS";
                    nomes_variaveis3[8] = "Leverage (hii)";


                    m_output_variaveis_geradas = "============================================================================================================================\n\n";

                    m_output_variaveis_geradas += "Estimao via OLS\n\n";
                    m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                    m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                    m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis3);

                    AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis3);
                }
                else
                {
                    string[] nomes_variaveis2 = new string[6];
                    nomes_variaveis2[0] = "Observacao_";
                    nomes_variaveis2[1] = "Y_observado_";
                    nomes_variaveis2[2] = "Prob_predita_";
                    nomes_variaveis2[3] = "Resíduo";
                    nomes_variaveis2[4] = "Resíduo Logit";
                    nomes_variaveis2[5] = "Resíduo Padronizado";
                    m_output_variaveis_geradas = "============================================================================================================================\n\n";

                    m_output_variaveis_geradas += "Estimao via OLS\n\n";
                    m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                    m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                    m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis2);

                    AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis2);
                }
            }
            else 
            {

                nomes_variaveis[0] = "Observacao_";
                nomes_variaveis[1] = "Y_observado_";
                nomes_variaveis[2] = "Prob_predita_";

                m_output_variaveis_geradas = "============================================================================================================================\n\n";

                m_output_variaveis_geradas += "Estimao via OLS\n\n";
                m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);

                AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);
            }            

            #endregion           
        }        

        #region derivadas parciais de primeira ordem do inverso das funções de ligação

        private double[,] diff_probit(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            double diff = m_normal.Density(m);
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

        #endregion

        #region inverso das funções de ligação

        Normal m_normal = new Normal();

        private double probit(double[,] x, double[,] beta)
        {
            double m = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                m += x[i, 0] * beta[i, 0];
            }
            return m_normal.CumulativeDistribution(m);
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

        #endregion

        #region avaliação do ajuste para modelos com variáveis binárias

        private string ChecaAjusteModelosBinarios(double[,] y, double[,] y_hat)
        {
            clsUtilTools clt = new clsUtilTools();

            int n = y.GetLength(0);
            int nc = 0;
            int nd = 0;
            int nties = 0;
            int t = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (y[i, 0] != y[j, 0])
                    {
                        t++;
                        if (y[i, 0] > y[j, 0] && y_hat[i, 0] > y_hat[j, 0]) nc = nc + 1;
                        if (y[i, 0] < y[j, 0] && y_hat[i, 0] < y_hat[j, 0]) nc = nc + 1;
                        if (y[i, 0] > y[j, 0] && y_hat[i, 0] < y_hat[j, 0]) nd = nd + 1;
                        if (y[i, 0] < y[j, 0] && y_hat[i, 0] > y_hat[j, 0]) nd = nd + 1;
                    }
                }
            }

            nties = t - nc - nd;

            double perc_pares_concordantes = 100.0 * (double)nc / (double)t;
            double perc_pares_discordantes = 100.0 * (double)nd / (double)t;
            double perc_pares_ties = 100.0 * (double)nties / (double)t;
            double total_pares = (double)t;

            double croc = ((double)nc + 0.5 * ((double)t - (double)nc - (double)nd)) / (double)t;
            double somersD = ((double)nc - (double)nd) / (double)t;
            double kruskal = ((double)nc - (double)nd) / ((double)nc + (double)nd);
            double kendaltau = ((double)nc - (double)nd) / (0.5 * (double)n * ((double)n - 1));

            string res = "";
            res += "Total de pares: " + t + "\n";
            res += "Total de pares concordantes: " + nc.ToString() + "\n";
            res += "Total de pares discordantes: " + nd.ToString() + "\n";
            res += "Total de pares empatados: " + nties.ToString() + "\n\n";

            res += "Percentual de pares concordantes: " + clt.Double2Texto(perc_pares_concordantes, 6) + "% \n";
            res += "Percentual de pares discordantes: " + clt.Double2Texto(perc_pares_discordantes, 6) + "% \n";
            res += "Percentual de pares empatados: " + clt.Double2Texto(perc_pares_ties, 6) + "% \n\n";

            res += "C: " + clt.Double2Texto(croc, 6) + "\n";
            res += "Somer's D: " + clt.Double2Texto(somersD, 6) + "\n";
            res += "Kruskal: " + clt.Double2Texto(kruskal, 6) + "\n";
            res += "Kendal-Tau: " + clt.Double2Texto(kendaltau, 6) + "\n";

            return res;
        }

        #endregion
        
        #endregion
    }
}
