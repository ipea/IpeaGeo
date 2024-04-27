using System;

using IpeaGeo.RegressoesEspaciais;
using MathNet.Numerics.Distributions;

namespace IpeaGeo.Modelagem
{
    public class BLogicRegressaoGLM : BLogicBaseModelagem
    {
        public enum FuncoesLigacaoGLM : int
        {
            Identidade = 1,
            Logaritmica = 2,
            Logit = 3,
            Probit = 4, 
            ComplentaryLogLog = 5,
            Power  = 6,
            BinomialNegativa = 7
        }

        public enum DistribuicoesGLM : int
        {
            Normal = 1,
            Bernoulli = 2,
            Gamma = 3,
            Poisson = 4,
            Geometrica = 5,
            InverseGaussian = 6,
            BinomialNegativa = 7,
            ZeroInflatedPoisson = 8, //ainda não programada
            ZeroInflatedNegBinomial = 9 //ainda não programada

        }

        private FuncoesLigacaoGLM m_funcao_ligacao = FuncoesLigacaoGLM.Identidade;
        public FuncoesLigacaoGLM FuncaoLigacaoModelo
        {
            get { return m_funcao_ligacao; }
            set { m_funcao_ligacao = value; }
        }

        private DistribuicoesGLM m_distribuicao = DistribuicoesGLM.Normal;
        public DistribuicoesGLM Distribuicao
        {
            get { return m_distribuicao; }
            set { m_distribuicao = value; }
        }

        private delegate void FuncaoLigacao(double bx, ref double mu, ref double g, ref double dg, ref double d2g);
        private delegate void FuncaoVariancia(double mu, ref double v, ref double dv);
        private delegate void FuncaoVariancia1(double mu, ref double v, ref double dv, double m_k_bin_neg);

        #region variance functions

        
        private void FuncaoVarianciaIdentidade(double mu, ref double v, ref double dv)
        {
            v = mu;
            dv = 1.0;
          
        }

        private void FuncaoVarianciaBernoulli(double mu, ref double v, ref double dv)
        {
            v = mu * (1.0 - mu) / m_ntrials_binomial;
            dv = (1.0 - 2.0 * mu) / m_ntrials_binomial;
        }

        private void FuncaoVarianciaUnitaria(double mu, ref double v, ref double dv)
        {
            v = 1.0;
            dv = 0.0;
        }

        private void FuncaoVarianciaGamma(double mu, ref double v, ref double dv)
        {
            v = Math.Pow(mu, 2.0);
            dv = 2.0*mu;
        }
        
        private void FuncaoVarianciaInverseGaussian(double mu, ref double v, ref double dv)
        {
            v = Math.Pow(mu, 3.0);
            dv = 3.0 * Math.Pow(mu, 2.0);
        }

        public double m_k_bin_neg = 0.0;

        private void FuncaoVarianciaBinomialNegativa(double mu, ref double v, ref double dv)
        {
            //double m_k_bin_neg = 1.0;
            v = mu + m_k_bin_neg * Math.Pow(mu, 2.0);
            dv = 1.0 + (m_k_bin_neg * 2.0 * mu);
            
        }

        private void FuncaoVarianciaGeometrica(double mu, ref double v, ref double dv)
        {
            v = mu + Math.Pow(mu, 2.0);
            dv = 1.0 + (2.0 * mu);
        }


        #endregion

        private double m_ntrials_binomial = 1.0;
        public double NumTrialsBinomial
        {
            get { return m_ntrials_binomial; }
            set { m_ntrials_binomial = value; }
        }

        #region funções de ligação

        private void FuncaoLigacaoIdentidade(double bx, ref double mu, ref double g, ref double dg, ref double d2g)
        {
            mu = bx;
            g = mu;
            dg = 1.0;
            d2g = 0.0;
        }

        private void FuncaoLigacaoLogaritimica(double bx, ref double mu, ref double g, ref double dg, ref double d2g)
        {
            mu = Math.Exp(bx);
            if (mu == 0)
            {
                mu = 0.0000001;
            }
            g = Math.Log(mu);
            dg = 1.0 / mu;
            d2g = -1.0 / Math.Pow(mu, 2.0);
        }

        private void FuncaoLigacaoLogit(double bx, ref double mu, ref double g, ref double dg, ref double d2g)
        {
            mu = Math.Exp(bx) / (1.0 + Math.Exp(bx));
            if (mu == 1.0)
            {
                mu = 0.9999999;
            }

            if (mu == 0)
            {
                mu = 0.0000001;
            }

            g = Math.Log(mu / (1.0 - mu));
            dg = 1.0 / (mu * (1.0 - mu));
            d2g = Math.Pow(1.0 / (mu * (1.0 - mu)), 2.0) * (2.0 * mu - 1.0);
        }

        private void FuncaoLigacaoComplementaryLogLog(double bx, ref double mu, ref double g, ref double dg, ref double d2g)
        {
            mu = 1.0 - Math.Exp((-Math.Exp(bx)));
            if (mu >= 1.0)
            {
                mu = 0.9999999;
            }

            if (mu <= 0)
            {
                mu = 0.0000001;
            }
            g = Math.Log((-Math.Log((1.0 - mu))));
            dg = 1.0/((mu-1.0)*Math.Log(1.0-mu));
            d2g = -(Math.Pow(1.0 / ((mu - 1.0) * Math.Log(1.0 - mu)), 2.0)) * (1.0 + Math.Log(1.0 - mu));
        }

        private double m_alpha_powerlink = -1.0;

        private void FuncaoLigacaoPower(double bx, ref double mu, ref double g, ref double dg, ref double d2g)
        {
            if (m_alpha_powerlink == 0.0)
            {
                mu = Math.Exp(bx);
                g = Math.Log(mu);
                dg = 1.0 / mu;
                d2g = -1.0 / (Math.Pow(mu, 2.0));
            }

            else 
            {
                mu = Math.Pow(bx, (1.0/m_alpha_powerlink));
                g = Math.Pow(mu, m_alpha_powerlink);
                dg = m_alpha_powerlink * Math.Pow(mu, (m_alpha_powerlink - 1.0));
                d2g = m_alpha_powerlink * Math.Pow(mu, (m_alpha_powerlink - 1.0)) * ((m_alpha_powerlink - 1.0) / mu);
            }
        }

        private void FuncaoLigacaoBinomialNegativa(double bx, ref double mu, ref double g, ref double dg, ref double d2g)
        {
            double m_k_bin_neg = 1.0;
            mu = Math.Exp(bx) / (m_k_bin_neg * (1.0 - Math.Exp(bx)));
            g = Math.Log(mu / (mu + (1 / m_k_bin_neg)));
            dg = 1.0 / (mu + (m_k_bin_neg * Math.Pow(mu, 2.0)));
            d2g = -Math.Pow(1.0 / (mu + m_k_bin_neg * Math.Pow(mu, 2.0)), 2.0) * (1.0 + 2.0 * m_k_bin_neg * mu);
        }

        private void FuncaoLigacaoProbit(double bx, ref double mu, ref double g, ref double dg, ref double d2g)
        {
            Normal norm = new Normal();
            mu = norm.CumulativeDistribution(bx);
            g = norm.InverseCumulativeDistribution(mu);
            dg = 1.0 / (norm.Density(g));
            d2g = Math.Pow(dg, 2.0) * g;
        }

         
        #endregion

        private double[,] m_weights = new double[0, 0];

        private double m_phi_glm = 1.0;

        private bool NumeroInvalido(double[,] v)
        {
            for (int i = 0; i < v.GetLength(0); i++)
            {
                if (double.IsNaN(v[i,0]) || double.IsInfinity(v[i,0]) || double.IsNegativeInfinity(v[i,0]) || double.IsPositiveInfinity(v[i,0])) return true;
            }

            return false;
        }

        #region deletar depois de usar

        private double[,] beta_global = new double[0, 0];

        private double funi1(double x)
        {
            return Math.Pow(x-2.1, 2.0) + 2.5;
        }

        private double f2(double[,] x)
        {
            return Math.Pow(x[0, 0] - 4.0, 2.0) + Math.Pow(x[1, 0] - 3.2, 4.0) 
                    + x[1,0] + Math.Pow(x[2, 0] - 3.2, 4.0) + 34.125;
        }

        private void teste_otimizacao()
        {
            clsUtilOptimization opt = new clsUtilOptimization();

            //clsUtilOptimization.FunctionSimple funcao = new RegressoesEspaciais.clsUtilOptimization.FunctionSimple(this.f1);
            clsUtilOptimization.FunctionSimple funcao = new RegressoesEspaciais.clsUtilOptimization.FunctionSimple(this.f2);

            clsUtilOptimization.FunctionUnivariate funcuni = new RegressoesEspaciais.clsUtilOptimization.FunctionUnivariate(this.funi1);

            IpeaGeo.RegressoesEspaciais.Fminsearch fm = new RegressoesEspaciais.Fminsearch();

            double[,] x0 = new double[3,1];
            double[,] x = new double[0,0];
            double fvalor = 0.0;

            fm.fminsearch(funcao, x0, ref x, ref fvalor);

            double xopt = opt.MinBrentSearch(funcuni, -10.0, 10.0, ref fvalor);
        }

        #endregion

        public void EstimaModeloGLM(ref double[,] residuos_brutos, bool m_estima_phi, double m_k_binomialnegativa, double m_alphapower)
        {                   
            teste_otimizacao();

            FuncaoLigacao flink = new FuncaoLigacao(this.FuncaoLigacaoIdentidade);
            FuncaoVariancia fvar = new FuncaoVariancia(this.FuncaoVarianciaIdentidade);
           
            #region especificando funções de ligação e de variância
            
            switch (this.m_funcao_ligacao)
            {
                case FuncoesLigacaoGLM.Logit:
                    flink = new FuncaoLigacao(this.FuncaoLigacaoLogit);
                    break;
                case FuncoesLigacaoGLM.Logaritmica:
                    flink = new FuncaoLigacao(this.FuncaoLigacaoLogaritimica);
                    break;
                case FuncoesLigacaoGLM.Identidade:
                    flink = new FuncaoLigacao(this.FuncaoLigacaoIdentidade);
                    break;
                case FuncoesLigacaoGLM.ComplentaryLogLog:
                    flink = new FuncaoLigacao(this.FuncaoLigacaoComplementaryLogLog);
                    break;
                case FuncoesLigacaoGLM.Power:
                    flink = new FuncaoLigacao(this.FuncaoLigacaoPower);
                    break;
                case FuncoesLigacaoGLM.BinomialNegativa:
                    flink = new FuncaoLigacao(this.FuncaoLigacaoBinomialNegativa);
                    break;
                case FuncoesLigacaoGLM.Probit:
                    flink = new FuncaoLigacao(this.FuncaoLigacaoProbit);
                    break;
                default:
                    break;
            }

            m_phi_glm = 1.0;
            
            switch (this.m_distribuicao)
            {
                case DistribuicoesGLM.Bernoulli:
                    fvar = new FuncaoVariancia(this.FuncaoVarianciaBernoulli);
                    m_phi_glm = 1.0;
                    break;
                case DistribuicoesGLM.Normal:
                    fvar = new FuncaoVariancia(this.FuncaoVarianciaUnitaria);
                    m_phi_glm = 1.0;
                    break;
                case DistribuicoesGLM.Poisson:
                    fvar = new FuncaoVariancia(this.FuncaoVarianciaIdentidade);
                    m_phi_glm = 1.0;
                    break;
                case DistribuicoesGLM.Gamma:
                    fvar = new FuncaoVariancia(this.FuncaoVarianciaGamma);
                    m_phi_glm = 1.0;
                    break;
                case DistribuicoesGLM.Geometrica:
                    fvar = new FuncaoVariancia(this.FuncaoVarianciaGeometrica);
                    m_phi_glm = 1.0;
                    break;
                case DistribuicoesGLM.BinomialNegativa:
                    fvar = new FuncaoVariancia(this.FuncaoVarianciaBinomialNegativa);
                    m_phi_glm = 1.0;
                    break;
                case DistribuicoesGLM.InverseGaussian:
                    fvar = new FuncaoVariancia(this.FuncaoVarianciaInverseGaussian);
                    m_phi_glm = 1.0;
                    break;
                default:
                    break;
            }

            #endregion


            

            clsUtilTools clt = new clsUtilTools();

            double[,] X = clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_independentes);
            double[,] y = clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_dependentes);

            int n = X.GetLength(0);

            m_weights = clt.Ones(n, 1);

            if (this.IncluiIntercepto)
            {
                X = clt.Concateh(clt.Ones(n, 1), X);
            }

            int k = X.GetLength(1);

            double mu = 0.0;
            double g = 0.0;
            double dg = 0.0;
            double d2g = 0.0;
            double v = 0.0;
            double dv = 0.0;
            m_k_bin_neg = m_k_binomialnegativa;
            m_alpha_powerlink = m_alphapower;

            double[,] grad = new double[k, 1];
            double aux_const = 1.0;

            double[,] we = new double[n, 1];
            double[,] wo = new double[n,1];

            double[,] hess = new double[k, k];
            double[,] hess_aux = new double[n, k];
            double[,] xi = new double[0, 0];
            double[,] invH = new double[0, 0];

            double[,] beta = clt.Zeros(k, 1);
            double[,] beta_old = new double[0, 0];

            double x2 = 0.0;
            double deviance = 0.0;
            double deviance_aux = 0.0;

            double AICc = 0.0;
            double AIC = 0.0;
            double BIC = 0.0;
            double loglik = 0.0;
            double fullloglik = 0.0;


            //double[,] LX = new double[n, k];
            //double[,] lambda = new double[0, 0];
            //double[,] gradold = new double[0, 0];
            //double[,] hessold = new double[0, 0];

            #region escolhendo um beta inicial com base em regressão linear

            double[,] ty = new double[n, 1];
            double yaux = 0.0;
            for (int i = 0; i < n; i++)
            {
                ty[i, 0] = y[i, 0];

                if (this.m_funcao_ligacao == FuncoesLigacaoGLM.Logaritmica)
                {
                    ty[i, 0] = Math.Log(y[i, 0] > 0 ? y[i, 0] : 0.000001);
                }

                if (this.m_funcao_ligacao == FuncoesLigacaoGLM.Logit)
                {
                    yaux = y[i, 0];
                    if (yaux <= 0.0) yaux = 0.0000001;
                    if (yaux >= 1.0) yaux = 0.9999999;
                    ty[i, 0] = Math.Log(yaux / (1.0 - yaux));
                }
            }

            beta = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(X), X)), clt.MatrizMult(clt.MatrizTransp(X), ty));
            
            #endregion

            bool ultima_chance = false;
            int maxiteracoes = 2000;
            int iter = 0;
            double mu_barra = 0.0;
            double[,] bx = new double[n,1];
            
            //double pi = 0.0;
            double[,] num_cat = new double[2, 2];

            if (m_distribuicao.ToString() == "Bernoulli")
            {
                clt.FrequencyTable(ref num_cat, y);
            }

            double ri = 0.0;
            double mi = 1.0;

            Gamma gamma = new Gamma(1.0, 1.0);

            IpeaGeo.RegressoesEspaciais.MathSpecialFunctions special = new RegressoesEspaciais.MathSpecialFunctions();

            if (m_funcao_ligacao.ToString() == "Power")
            {
                for (int t = 0; t < beta.GetLength(0); t++)
                {
                    beta[t, 0] = 0.000000000001;
                }
            }
            for (int z = 0; z < 2; z++)
            {
                 while (iter < maxiteracoes)
                {
                    beta_old = clt.ArrayDoubleClone(beta);

                    #region versão antiga poisson

                    //lambda = clt.ExpMatriz(clt.MatrizMult(X, beta));

                    //gradold = clt.MatrizTransp(clt.MatrizMult(clt.MatrizTransp(clt.MatrizSubtracao(lambda, y)), X));

                    //for (int i = 0; i < n; i++)
                    //{
                    //    for (int j = 0; j < k; j++)
                    //    {
                    //        LX[i, j] = X[i, j] * lambda[i, 0];
                    //    }
                    //}

                    //hessold = clt.MatrizMult(clt.MatrizTransp(LX), X);

                    #endregion

                    bx = clt.MatrizMult(X, beta);

                    grad = clt.Zeros(k, 1);
                    hess = clt.Zeros(k, k);

                    x2 = 0.0;
                    deviance = 0.0;
                    deviance_aux = 0.0;

                    if (iter == 5)
                    {
                        double a = 1.0;
                    }

                    for (int i = 0; i < n; i++)
                    {

                        flink(bx[i, 0], ref mu, ref g, ref dg, ref d2g);
                        fvar(mu, ref v, ref dv);

                        aux_const = (m_weights[i, 0] * (y[i, 0] - mu)) / (v * dg * m_phi_glm);

                        for (int j = 0; j < k; j++)
                        {
                            grad[j, 0] = grad[j, 0] + aux_const * X[i, j];
                        }

                        we[i, 0] = m_weights[i, 0] / (m_phi_glm * v * Math.Pow(dg, 2.0));
                        wo[i, 0] = we[i, 0] + (m_weights[i, 0] * (y[i, 0] - mu)) * (v * d2g + dv * dg) / (Math.Pow(v, 2.0) * Math.Pow(dg, 3.0) * m_phi_glm);

                        wo[i, 0] = -wo[i, 0];

                        xi = clt.SubRowArrayDouble(X, i);

                        hess_aux = clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(xi), xi), wo[i, 0]);

                        hess = clt.MatrizSoma(hess, hess_aux);

                        x2 += (m_weights[i, 0] * (Math.Pow(y[i, 0] - mu, 2.0))) / v;


                        #region Log-Likelihood das distribuições

                        if (z == 1 && m_distribuicao.ToString() == "Normal")
                        {
                            loglik += (-0.5) * ((m_weights[i,0] * Math.Pow(y[i, 0] - mu, 2) / m_phi_glm) + Math.Log(m_phi_glm / m_weights[i, 0]) + Math.Log(2 * Math.PI));
                            fullloglik = loglik;
                            deviance_aux += m_weights[i, 0] * (Math.Pow(y[i, 0] - mu, 2.0));
                        }

                        //if (z == 0 && (m_distribuicao.ToString() == "InverseGaussian" || m_distribuicao.ToString() == "BinomialNegativa" || m_distribuicao.ToString() == "Geometrica"))
                        //{
                        //    mu_barra+=mu;
                        
                        //}


                        if (z == 1 && m_distribuicao.ToString() == "InverseGaussian")
                        {
                            loglik += (-0.5) * ((m_weights[i, 0] * Math.Pow(y[i, 0] - mu, 2.0) / (y[i, 0] * m_phi_glm * Math.Pow(mu_barra, 2.0)) + Math.Log(m_phi_glm / m_weights[i, 0]) + Math.Log(2 * Math.PI)));
                            fullloglik = loglik;
                            deviance_aux += (m_weights[i, 0] * (Math.Pow(y[i, 0] - mu, 2.0))) / (Math.Pow(mu, 2.0) * y[i, 0]);
                        }

                        if (z == 1 && m_distribuicao.ToString() == "Gamma")
                        {
                            loglik += ((m_weights[i, 0] / m_phi_glm) * (Math.Log((m_weights[i, 0] * y[i, 0]) / (m_phi_glm * mu)))) - ((m_weights[i, 0] * y[i, 0]) / (m_phi_glm * mu)) - (Math.Log(y[i, 0])) - (IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln((m_weights[i, 0] / m_phi_glm)));
                            fullloglik = loglik;
                            deviance_aux += 2.0 * m_weights[i, 0] * ((-Math.Log(y[i, 0] / mu)) + (y[i, 0] - mu) / mu);
                        }

                        if (z == 1 && m_distribuicao.ToString() == "BinomialNegativa")
                        {
                            loglik += y[i, 0] * Math.Log(m_k_bin_neg * mu / m_weights[i, 0]) - (y[i, 0] + m_weights[i, 0] / m_k_bin_neg) * Math.Log(1.0 + (m_k_bin_neg * mu / m_weights[i, 0])) + IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(y[i, 0] + m_weights[i, 0] / m_k_bin_neg) - IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(m_weights[i, 0] / m_k_bin_neg);
                            fullloglik += y[i, 0] * Math.Log(m_k_bin_neg * mu / m_weights[i, 0]) - (y[i, 0] + m_weights[i, 0] / m_k_bin_neg) * Math.Log(1.0 + (m_k_bin_neg * mu / m_weights[i, 0])) + IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(y[i, 0] + m_weights[i, 0] / m_k_bin_neg) - (IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(y[i, 0] + 1.0)+ IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(m_weights[i, 0] / m_k_bin_neg) );  
                            deviance_aux += 2.0 * (y[i, 0] * Math.Log(y[i, 0] / mu) - (y[i, 0] + (1.0 / m_k_bin_neg)) * Math.Log((y[i, 0] + (1.0 / m_k_bin_neg)) / (mu + 1.0 / m_k_bin_neg)));
                        }


                        if (z == 1 && m_distribuicao.ToString() == "Poisson")
                        {
                            loglik += (m_weights[i, 0] / m_phi_glm) * (y[i, 0] * Math.Log(mu) - mu);
                            fullloglik += m_weights[i, 0] * ((y[i, 0] * Math.Log(mu)) - mu - IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(y[i, 0] + 1.0));
                            deviance_aux += 2.0 * m_weights[i, 0] * ((y[i, 0] * Math.Log(y[i, 0] / mu)) - (y[i, 0] - mu));
                        }

                        if (z == 1 && m_distribuicao.ToString() == "Bernoulli")
                        {
                            ri = y[i, 0];
                            loglik += (m_weights[i, 0] / m_phi_glm) * (y[i, 0] * Math.Log(mu) + (1.0 - y[i, 0]) * Math.Log(1.0 - mu));
                            fullloglik += (m_weights[i, 0] / m_phi_glm) * (y[i, 0] * Math.Log(mu) + (1.0 - y[i, 0]) * Math.Log(1.0 - mu)) + (m_weights[i, 0] / m_phi_glm) * (IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(mi + 1) - (IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(ri + 1) + IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln((mi - ri) + 1)));
                            deviance_aux += 2.0 * m_weights[i, 0] * ((y[i, 0] * Math.Log(y[i, 0] / mu)) + ((1.0 - y[i, 0]) * Math.Log((1.0 - y[i, 0]) / (1.0 - mu))));
                        }


                        if (z == 1 && m_distribuicao.ToString() == "Geometrica")
                        {
                            m_k_bin_neg = 1.0;
                            loglik += y[i, 0] * Math.Log(m_k_bin_neg * mu / m_weights[i, 0]) - (y[i, 0] + m_weights[i, 0] / m_k_bin_neg) * Math.Log(1.0 + (m_k_bin_neg * mu / m_weights[i, 0])) + IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(y[i, 0] + m_weights[i, 0] / m_k_bin_neg) - IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(m_weights[i, 0] / m_k_bin_neg);
                            fullloglik += y[i, 0] * Math.Log(m_k_bin_neg * mu / m_weights[i, 0]) - (y[i, 0] + m_weights[i, 0] / m_k_bin_neg) * Math.Log(1.0 + (m_k_bin_neg * mu / m_weights[i, 0])) + IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(y[i, 0] + m_weights[i, 0] / m_k_bin_neg) - (IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(y[i, 0] + 1.0) + IpeaGeo.RegressoesEspaciais.MathSpecialFunctions.gammln(m_weights[i, 0] / m_k_bin_neg));
                            deviance_aux += 2.0 * (y[i, 0] * Math.Log(y[i, 0] / mu) - (y[i, 0] + (1.0 / m_k_bin_neg)) * Math.Log((y[i, 0] + (1.0 / m_k_bin_neg)) / (mu + 1.0 / m_k_bin_neg)));
                        }

                        #endregion


                    }

                   
                    try
                    {
                        //invH = clt.Identity(k);
                        invH = clt.MatrizInversa(hess);
                    }
                    catch
                    {
                        invH = clt.Identity(k);
                    }

                    beta = clt.MatrizSubtracao(beta, clt.MatrizMult(invH, grad));

                    if (!ultima_chance && NumeroInvalido(beta) && m_funcao_ligacao.ToString()!="Power")
                    {
                        beta = clt.Zeros(beta.GetLength(0), 1);
                        iter = 0;
                        ultima_chance = true;
                    }



                    //if (clt.Norm(grad) < 1.0e-6 
                    //    || (clt.Norm(clt.MatrizSubtracao(beta, beta_old)) / (1.0 + clt.Norm(beta_old))) < 1.0e-6)
                    if (clt.Norm(grad) < 1.0e-6)
                    {
                        break;
                    }

                    iter++;
                }

                if (m_estima_phi == true && z == 0)
                {
                    m_phi_glm = x2 / (n - X.GetLength(1));

                }

                if (z == 0 && (m_distribuicao.ToString() == "InverseGaussian" || m_distribuicao.ToString() == "BinomialNegativa" || m_distribuicao.ToString() == "Geometrica"))
                {
                    mu_barra=mu_barra/n;

                }
                //else 
               // {
               //     break;
 
              //  }


            }

            double scaled_x2 = x2 / m_phi_glm;

            deviance = deviance_aux / m_phi_glm;

            this.BetaHat = clt.ArrayDoubleClone(beta);

            #region gerando a previsão do modelo 

            double[,] y_pred = new double[n, 1];
            //double[,] residuos_brutos = new double[n, 1];

            bx = clt.MatrizMult(X, this.m_beta_hat);

            for (int i = 0; i < n; i++)
            {
                flink(bx[i, 0], ref mu, ref g, ref dg, ref d2g);
                y_pred[i, 0] = mu;
                residuos_brutos[i, 0] = y[i, 0] - y_pred[i, 0];
            }

            #endregion

            double[,] cov_mat = clt.MatrizMult(-1.0, invH);

            this.NumIterations = iter;
            this.BetaHat = beta;
            this.BetaHatCovMatrix = cov_mat;

            this.GeraSignificanciaCoeficientes();      

            #region testes para comparação de modelo

            //int GLerro = X.GetLength(0) - X.GetLength(1);
            //double SQE = (clt.MatrizMult(clt.MatrizTransp(residuos_brutos), residuos_brutos))[0, 0];
            //double EPerro = SQE / GLerro;

            AIC = -2.0 * fullloglik + 2.0 * X.GetLength(1);

            AICc = -2.0 * fullloglik + ((2.0 * X.GetLength(1) * n) / (n - X.GetLength(1) - 1.0));

            BIC = -2.0 * fullloglik + X.GetLength(1) * Math.Log(n);

            #endregion
           // this.loglikelihood(beta, m_phi_glm, y, X, ref loglik, ref fullloglik, ref AIC, ref BIC, ref AICc);
            
            #region gerando o output para resultado das estimações

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação via Máxima Verossimilhança para Modelos Lineares Generalizados \n\n";
            
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
            out_text += "Distribuição: " + this.m_distribuicao.ToString() + "\n";
            out_text += "Função de ligação: " + this.m_funcao_ligacao.ToString() + "\n";
            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            //out_text += "Variância dos erros: " + clt.Double2Texto(m_sigma2_hat, 6) + "\n";
            out_text += "Número de iterações até a convergência: " + iter.ToString() + "\n";
            out_text += "Número máximo de iterações permitido: " + maxiteracoes.ToString() + "\n";

            if (iter < maxiteracoes) out_text += "Obs.: algoritmo convergiu com sucesso (norma Euclidiana do gradiente < 1.0e-6)";
            else out_text += "Obs.: número máximo de iterações atingido - o algoritmo não convergiu (norma Euclidiana do gradiente > 1.0e-6).";

            out_text += "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, m_beta_hat, m_beta_stderror, m_beta_pvalor, this.IncluiIntercepto);

            out_text += "\n";


            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";

                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(m_beta_hat_cov, VariaveisIndependentes, m_usa_intercepto);
            }

            out_text += "\n";

            out_text += "Graus de liberdade: " + clt.Double2Texto((n - X.GetLength(1)), 6) + "\n";
            out_text += "Parâmetro de Dispersão (via X² de Pearson): " + clt.Double2Texto(m_phi_glm, 6); 
            out_text += "\n";

            out_text += "\n=================== Análise de Ajuste do Modelo\n\n";

            if (m_distribuicao.ToString() != "Bernoulli")
            {
                out_text += "Chi-Quadrado de Pearson: " + clt.Double2Texto(x2, 6) + "\n";
               // out_text += "Chi-Quadrado de Pearson em escala: " + clt.Double2Texto(scaled_x2, 6) + "\n";
                out_text += "Deviance: " + clt.Double2Texto(deviance, 6) + "\n";

            }
            
            out_text += "AIC: " + clt.Double2Texto(AIC, 6) + "\n";
            out_text += "AICC: " + clt.Double2Texto(AICc, 6) + "\n";
            out_text += "BIC: " + clt.Double2Texto(BIC, 6) + "\n";
            out_text += "Log-Verossimilhança: " + clt.Double2Texto(loglik, 6) + "\n";
            out_text += "Full Log-Verossimilhança: " + clt.Double2Texto(fullloglik, 6);
            
            

            this.m_output_text = out_text;

            #endregion

            #region adicionando variveis base de dados

            double[,] observacoes = new double[X.GetLength(0), 1];

            for (int i = 0; i < n; i++)
            {
                observacoes[i, 0] = (double)i;
            }

            double[,] variaveis_geradas = clt.Concateh(observacoes, clt.Concateh(y, clt.Concateh(y_pred, residuos_brutos)));

            //if (m_ckb_influentes)
            //{
            //    variaveis_geradas = clt.Concateh(variaveis_geradas, studentizedresidual);

            //    variaveis_geradas = clt.Concateh(variaveis_geradas, CookD);

            //    variaveis_geradas = clt.Concateh(variaveis_geradas, DFFITS);

            //    variaveis_geradas = clt.Concateh(variaveis_geradas, hii);
            //}

            string[] nomes_variaveis = new string[0];

            nomes_variaveis = new string[4];

            //if (m_ckb_influentes)
            //{
            //    nomes_variaveis = new string[8];
            //}
            //else
            //{
            //    nomes_variaveis = new string[4];
            //}
            
            nomes_variaveis[0] = "Observacao_";

            nomes_variaveis[1] = "Y_observado_";

            nomes_variaveis[2] = "Y_predito_";

            nomes_variaveis[3] = "Residuo_Bruto_";

            //if (m_ckb_influentes)
            //{
            //    nomes_variaveis[4] = "Resíduo Studentizado";

            //    nomes_variaveis[5] = "D de Cook";

            //    nomes_variaveis[6] = "DFFITS";

            //    nomes_variaveis[7] = "Leverage (hii)";
            //}

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação via Máxima Verossimilhança para Modelos Lineares Generalizados \n\n"; 

            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";

            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);

            AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);

            #endregion                                        
        }
    }
}
