using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsLinearRegressionModelsMLE : clsModelosRegressaoEspacial
    {
        public clsLinearRegressionModelsMLE()
        {
        }

        #region internal variables
        
        private clsUtilOptimization opt = new clsUtilOptimization();
        private clsUtilTools clt = new clsUtilTools();
        private clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
        private clsMatrizEsparsa m_sparse_ident = new clsMatrizEsparsa();
        private clsLogDetMatrizEsparsa m_ldet = new clsLogDetMatrizEsparsa();

        #endregion

        #region estimação dos modelos SEM

        #region Estimação do modelo SEM via FGLS

        /// <summary>
        /// Função para estimar os modelos de spatial error models (SEM) via mínimos quadrados ordinários.
        /// </summary>
        public void EstimateModeloSEM_via_GLS()
        {
            EstimateModeloSEM();
        }

        #endregion

        #region Estimação do modelo SEM via OLS

        /// <summary>
        /// Função para estimar os modelos de spatial error models (SEM) via mínimos quadrados ordinários.
        /// </summary>
        public void EstimateModeloSEM_via_OLS()
        {
            EstimateModeloSEM();
        }

        #endregion

        #region Estimação do modelo SEM via MLE

        #region rotinas para estimação via MLE dos modelos SEM

        private double diff2loglik_SEM(double lambda)
        {
            double h = 1.0e-4;
            double res = (minusloglik_SEM(lambda + 2.0 * h) - 2.0 * minusloglik_SEM(lambda) + minusloglik_SEM(lambda - 2.0 * h)) / (4.0 * h * h);
            return -res;
        }

        private double loglik_SEM(double lambda)
        {
            return - minusloglik_SEM(lambda) - ((double)m_nobs / 2.0) * (1.0 - Math.Log((double)m_nobs) + Math.Log(Math.PI));
        }

        private double minusloglik_SEM(double lambda)
        {
            double[,] BAy = fme.MatrizMult(fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, -lambda), m_Y);
            double[,] Bx = fme.MatrizMult(fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, -lambda), m_X);

            double[,] XXinv = clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(Bx), Bx));
            double[,] beta = clt.MatrizMult(XXinv, (clt.MatrizMult(clt.MatrizTransp(Bx), BAy)));

            double[,] v = clt.MatrizSubtracao(BAy, clt.MatrizMult(Bx, beta));
            double vpv = (clt.MatrizMult(clt.MatrizTransp(v), v))[0, 0];

            double llike = (((double)m_nobs) / 2.0) * Math.Log(vpv)
                            - m_W_esparsa.Spline.interp(lambda);
            return llike;
        }

        #endregion

        #region chamada da função de estimação dos modelos SEM

        /// <summary>
        /// Função para estimar os modelos de spatial error models (SEM) via máxima verossimilhança.
        /// </summary>
        public void EstimateModeloSEM()
        {
            this.m_nobs = this.m_Y.GetLength(0);
            if (this.m_W_esparsa.n == 0) this.MatrizWesparsaFromVizinhosNorm();

            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.MatrizDensa)
            {
                this.m_W_esparsa.CalcularAutovaloresW();
            }

            double lambda_min = -1.0;
            double lambda_max = 0.99;

            if ((this.m_nobs <= 300) || (this.m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Original && this.m_nobs <= 2000))
            {
                lambda_min = 0.0;
                lambda_max = 0.0;

                if (this.m_tipo_logdet != TipoCalculoLogDetWMatrix.MatrizDensa)
                {
                    this.m_W_esparsa.CalcularAutovaloresW();
                }

                lambda_min = (1.0 / this.m_W_esparsa.Autovalores[this.m_W_esparsa.Autovalores.GetLength(0) - 1]) + 0.01;
                lambda_max = (1.0 / this.m_W_esparsa.Autovalores[0]) - 0.01;
            }

            double xmin = lambda_min;
            double xmax = lambda_max;
            
            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.MatrizDensa)
            {
                this.m_W_esparsa.GeraTabelaLogDetAutovalores(xmin, xmax);
            }
            else
            {
                this.m_W_esparsa.GeraTabelaLogDetLU(xmin, xmax);
            }

            m_sparse_ident = fme.Identity(m_nobs);

            double rho_new = 0.0;
            double fval = 0.0;

            clsUtilOptimization.FunctionUnivariate func = new clsUtilOptimization.FunctionUnivariate(this.minusloglik_SEM);
            rho_new = opt.MinBrentSearch(func, xmin, xmax, ref fval);

            double[,] BAy = fme.MatrizMult(fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, -rho_new), m_Y);
            double[,] Bx = fme.MatrizMult(fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, -rho_new), m_X);

            double[,] XXinv = clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(Bx), Bx));
            m_beta_hat = clt.MatrizMult(XXinv, (clt.MatrizMult(clt.MatrizTransp(Bx), BAy)));

            double[,] v = clt.MatrizSubtracao(BAy, clt.MatrizMult(Bx, this.m_beta_hat));
            double vpv = (clt.MatrizMult(clt.MatrizTransp(v), v))[0, 0];
            
            this.m_sigma2_hat = vpv / (double)m_nobs;
            this.m_rho_hat = rho_new;

            //=========================== chamada da geração da matriz de variância-covariância ==============================//

            double[,] covmat_beta = new double[0, 0];
            double[,] covmat_all = new double[0, 0];
            double var_rho = 0.0;

            this.CovarianceMatrixSEMModels(out covmat_beta, out covmat_all, out var_rho);

            this.m_beta_hat_cov = covmat_beta;

            this.GeraSignificanciaCoeficientes();

            this.m_loglik = loglik_SEM(this.m_rho_hat);
            
            this.m_rho_stderror = var_rho;
            this.m_rho_stderror = Math.Sqrt(this.m_rho_stderror);

            this.m_aic = -2.0 * this.m_loglik + 2.0 * ((double)this.m_beta_hat.GetLength(0) + 2.0);
            this.m_bic = -2.0 * this.m_loglik + Math.Log((double)m_nobs) * ((double)this.m_beta_hat.GetLength(0) + 2.0);

            MathNormaldist normal = new MathNormaldist();

            double prob_cauda = this.m_prob_confidence_intervals / 200.0;
            double cv = normal.invcdf(0.5 + prob_cauda);

            this.m_rho_tstat = this.m_rho_hat / this.m_rho_stderror;
            this.m_rho_pvalor = 2.0 * (1.0 - normal.cdf(Math.Abs(m_rho_tstat)));
            m_rho_liminf_ci = m_rho_hat - cv * m_rho_stderror;
            m_rho_limsup_ci = m_rho_hat + cv * m_rho_stderror;

            this.LikelihoodRatioTestStatRho = 2.0 * Math.Abs(this.loglik_SEM(this.m_rho_hat) - this.loglik_SEM(0.0));
            this.LikelihoodRatioTestPvalueRho = 2.0 * (1.0 - normal.cdf(Math.Abs(Math.Sqrt(this.LikelihoodRatioTestStatRho))));

            //--------------------------- geração de novas variáveis --------------------------//

            double[,] y = m_Y;
            double[,] y_hat = clt.MatrizMult(this.m_X, this.m_beta_hat);
            double[,] residuos = clt.MatrizSubtracao(m_Y, y_hat);
            double var_residuos = clt.VarianciaColumnMatrix(residuos);
            double[,] Wresiduos = fme.MatrizMult(this.m_W_esparsa, residuos);
            double[,] observacoes = new double[y.GetLength(0), 1];
            for (int i = 0; i < y.GetLength(0); i++) observacoes[i, 0] = (double)(i + 1);

            clsMatrizEsparsa IrW;
            if (this.m_rho_hat == 0)
            {
                IrW = fme.Identity(y.GetLength(0));
            }
            else
            {
                IrW = fme.MatrizSoma(fme.Identity(y.GetLength(0)), this.m_W_esparsa, 1.0, -this.m_rho_hat);
            }
            double[,] epsilon = fme.MatrizMult(IrW, residuos);
            double[,] Wepsilon = fme.MatrizMult(this.m_W_esparsa, epsilon);
            double var_epsilon = clt.VarianciaColumnMatrix(epsilon);

            this.m_variaveis_geradas = clt.Concateh(observacoes, y);
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, y_hat);
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, residuos);
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, clt.MatrizDiv(residuos, Math.Sqrt(var_residuos)));
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, Wresiduos);
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, epsilon);
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, clt.MatrizDiv(epsilon, Math.Sqrt(var_epsilon)));
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, Wepsilon);

            this.m_nomes_variaveis_geradas = new string[9];
            this.m_nomes_variaveis_geradas[0] = "Observacao_";
            this.m_nomes_variaveis_geradas[1] = "Y_observado_";
            this.m_nomes_variaveis_geradas[2] = "Y_predito_condicional_";
            this.m_nomes_variaveis_geradas[3] = "Residuo_";
            this.m_nomes_variaveis_geradas[4] = "Residuo_padronizado_";
            this.m_nomes_variaveis_geradas[5] = "SpatialW_residuo_";
            this.m_nomes_variaveis_geradas[6] = "Epsilon_";
            this.m_nomes_variaveis_geradas[7] = "Epsilon_padronizado_";
            this.m_nomes_variaveis_geradas[8] = "SpatialW_epsilon_";
        }
        #endregion

        #endregion

        #endregion

        #region estimação dos modelos SAR via MLE

        #region rotinas para função de likelihood nos modelos SAR

        private double diff2loglik_SAR(double rho)
        {
            double h = 1.0e-4;
            double res = (minusloglik_SAR(rho + 2.0 * h) - 2.0 * minusloglik_SAR(rho) + minusloglik_SAR(rho - 2.0 * h)) / (4.0 * h * h);
            return -res;
        }

        private double loglik_SAR(double rho)
        {
            return -minusloglik_SAR(rho) - ((double)m_nobs / 2.0) * (1.0 - Math.Log((double)m_nobs) + Math.Log(Math.PI));
        }

        private double minusloglik_SAR(double rho)
        {
            double[,] Ay = fme.MatrizMult(fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, -rho), m_Y);

            double[,] XXinv = clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(m_X), m_X));
            double[,] beta = clt.MatrizMult(XXinv, (clt.MatrizMult(clt.MatrizTransp(m_X), Ay)));

            double[,] v = clt.MatrizSubtracao(Ay, clt.MatrizMult(m_X, beta));
            double vpv = (clt.MatrizMult(clt.MatrizTransp(v), v))[0, 0];

            double llike = (((double)m_nobs) / 2.0) * Math.Log(vpv)
                            - m_W_esparsa.Spline.interp(rho);
            return llike;
        }

        #endregion 

        #region Chamada da função de estimação do modelo SAR via máxima verossimilhança

        public void EstimateModeloSAR()
        {
            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.MatrizDensa)
            {
                this.m_W_esparsa.CalcularAutovaloresW();
            }

            this.m_nobs = this.m_Y.GetLength(0);

            double xmin = -1.0;
            double xmax = 0.99;

            m_sparse_ident = fme.Identity(m_nobs);

            if ((this.m_nobs <= 300) || (this.m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Original && this.m_nobs <= 2000))
            {
                double lambda_min = 0.0;
                double lambda_max = 0.0;

                if (this.m_tipo_logdet != TipoCalculoLogDetWMatrix.MatrizDensa)
                {
                    this.m_W_esparsa.CalcularAutovaloresW();
                }

                lambda_min = (1.0 / this.m_W_esparsa.Autovalores[this.m_W_esparsa.Autovalores.GetLength(0) - 1]) + 0.01;
                lambda_max = (1.0 / this.m_W_esparsa.Autovalores[0]) - 0.01;

                xmin = lambda_min;
                xmax = lambda_max;
            }

            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.MatrizDensa)
            {
                this.m_W_esparsa.GeraTabelaLogDetAutovalores(xmin, xmax);
            }
            else
            {
                this.m_W_esparsa.GeraTabelaLogDetLU(xmin, xmax);
            }

            clsUtilOptimization.FunctionUnivariate func = new clsUtilOptimization.FunctionUnivariate(this.minusloglik_SAR);
            double fval = 0.0;

            this.m_rho_hat = opt.MinBrentSearch(func, xmin, xmax, ref fval);

            double[,] Ay = fme.MatrizMult(fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, -this.m_rho_hat), m_Y);  
            double[,] XXinv = clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(m_X), m_X));
            m_beta_hat = clt.MatrizMult(XXinv, (clt.MatrizMult(clt.MatrizTransp(m_X), Ay)));

            double[,] v = clt.MatrizSubtracao(Ay, clt.MatrizMult(m_X, this.m_beta_hat));
            double vpv = (clt.MatrizMult(clt.MatrizTransp(v), v))[0, 0];

            this.m_sigma2_hat = vpv / ((double)m_nobs);

            //================= chamando a função para geração da matriz de covariância dos parâmetros ======================//

            double[,] cov_mat_beta = new double[0, 0];
            double[,] cov_mat_all = new double[0, 0];
            double var_rho = 0.0;

            this.CovarianceMatrixSARModels(out cov_mat_beta, out cov_mat_all, out var_rho);
            this.m_beta_hat_cov = cov_mat_beta;

            this.GeraSignificanciaCoeficientes();

            //-------------------- demais estatísticas para inferencia --------------------------------//

            this.m_loglik = loglik_SAR(this.m_rho_hat);

            this.m_rho_stderror = var_rho;
            this.m_rho_stderror = Math.Sqrt(this.m_rho_stderror);

            this.m_aic = -2.0 * this.m_loglik + 2.0 * ((double)this.m_beta_hat.GetLength(0) + 2.0);
            this.m_bic = -2.0 * this.m_loglik + Math.Log((double)m_nobs) * ((double)this.m_beta_hat.GetLength(0) + 2.0);

            MathNormaldist normal = new MathNormaldist();

            double prob_cauda = this.m_prob_confidence_intervals / 200.0;
            double cv = normal.invcdf(0.5 + prob_cauda);

            this.m_rho_tstat = this.m_rho_hat / this.m_rho_stderror;
            this.m_rho_pvalor = 2.0 * (1.0 - normal.cdf(Math.Abs(m_rho_tstat)));
            m_rho_liminf_ci = m_rho_hat - cv * m_rho_stderror;
            m_rho_limsup_ci = m_rho_hat + cv * m_rho_stderror;

            this.LikelihoodRatioTestStatRho = 2.0 * Math.Abs(this.loglik_SAR(this.m_rho_hat) - this.loglik_SAR(0.0));
            this.LikelihoodRatioTestPvalueRho = 2.0 * (1.0 - normal.cdf(Math.Abs(Math.Sqrt(this.LikelihoodRatioTestStatRho))));

            //--------------------------- geração de novas variáveis --------------------------//

            double var_residuos = clt.VarianciaColumnMatrix(v);
            double[,] y_hat = clt.MatrizMult(m_rho_hat, fme.MatrizMult(m_W_esparsa, m_Y));
            y_hat = clt.MatrizSoma(y_hat, clt.MatrizMult(m_X, m_beta_hat));

            double[,] Wresiduos = fme.MatrizMult(this.m_W_esparsa, v);
            double[,] observacoes = new double[m_Y.GetLength(0), 1];
            for (int i = 0; i < m_Y.GetLength(0); i++) observacoes[i, 0] = (double)(i + 1);

            this.m_variaveis_geradas = clt.Concateh(observacoes, m_Y);
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, y_hat);
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, v);
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, clt.MatrizDiv(v, Math.Sqrt(var_residuos)));
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, Wresiduos);

            this.m_nomes_variaveis_geradas = new string[6];
            this.m_nomes_variaveis_geradas[0] = "Observacao_";
            this.m_nomes_variaveis_geradas[1] = "Y_observado_";
            this.m_nomes_variaveis_geradas[2] = "Y_predito_condicional_";
            this.m_nomes_variaveis_geradas[3] = "Residuo_";
            this.m_nomes_variaveis_geradas[4] = "Residuo_padronizado_";
            this.m_nomes_variaveis_geradas[5] = "SpatialW_residuo_";
        }

        #endregion

        #endregion

        #region estimação dos modelos SAC via MLE

        #region Chamada da função de estimação do modelo SAC via máxima verossimilhança

        public void EstimateModeloSAC()
        {
            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.MatrizDensa)
            {
                this.m_W_esparsa.CalcularAutovaloresW();
            }

            this.m_nobs = this.m_Y.GetLength(0);

            double lmin = -1.0;
            double lmax = 0.99;

            if ((this.m_nobs <= 300) || (this.m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Original && this.m_nobs <= 2000))
            {
                lmin = 0.0;
                lmax = 0.0;

                if (this.m_tipo_logdet != TipoCalculoLogDetWMatrix.MatrizDensa)
                {
                    this.m_W_esparsa.CalcularAutovaloresW();
                }

                lmin = (1.0 / this.m_W_esparsa.Autovalores[this.m_W_esparsa.Autovalores.GetLength(0) - 1]) + 0.01;
                lmax = (1.0 / this.m_W_esparsa.Autovalores[0]) - 0.01;
            }

            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.MatrizDensa)
            {
                this.m_W_esparsa.GeraTabelaLogDetAutovalores(lmin, lmax);
            }
            else
            {
                this.m_W_esparsa.GeraTabelaLogDetLU(lmin, lmax);
            }

            m_sparse_ident = fme.Identity(m_nobs);

            clsUtilOptimization.FunctionSimple func = new clsUtilOptimization.FunctionSimple(this.minusloglik_SAC);
            Fminsearch fmin = new Fminsearch();
            double[,] x0 = new double[2, 1];
            double[,] x = new double[2, 1];
            double fval = 0.0;
            fmin.fminsearch(func, x0, ref x, ref fval);

            double rho = x[0, 0];
            double lambda = x[1, 0];

            this.m_rho_hat = rho;
            this.m_lambda_hat = lambda;

            clsMatrizEsparsa A = fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, -rho);
            clsMatrizEsparsa B = fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, -lambda);
            double[,] Ay = fme.MatrizMult(A, m_Y);
            double[,] BAy = fme.MatrizMult(B, Ay);
            double[,] Bx = fme.MatrizMult(fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, -lambda), m_X);

            double[,] XXinv = clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(Bx), Bx));
            m_beta_hat = clt.MatrizMult(XXinv, (clt.MatrizMult(clt.MatrizTransp(Bx), BAy)));

            double[,] e = clt.MatrizSubtracao(BAy, clt.MatrizMult(Bx, this.m_beta_hat));
            double epe = (clt.MatrizMult(clt.MatrizTransp(e), e))[0, 0];

            m_sigma2_hat = epe / ((double)m_nobs);
            m_residuos = e;

            //================================ chamada da função para geração da matriz de covariância ==========================//

            double[,] covmat_beta = new double[0,0];
            double[,] covmat_all = new double[0,0];
            double var_rho = 0.0;
            double var_lambda = 0.0;

            CovarianceMatrixSACModels(out covmat_beta, out covmat_all, out var_rho, out var_lambda);

            this.m_beta_hat_cov = covmat_beta;
            this.GeraSignificanciaCoeficientes();
            
            this.m_rho_stderror = var_rho;
            this.m_rho_stderror = Math.Sqrt(this.m_rho_stderror);

            this.m_lambda_stderror = var_lambda;
            this.m_lambda_stderror = Math.Sqrt(this.m_lambda_stderror);

            MathNormaldist normal = new MathNormaldist();

            double prob_cauda = this.m_prob_confidence_intervals / 200.0;
            double cv = normal.invcdf(0.5 + prob_cauda);

            this.m_rho_tstat = this.m_rho_hat / this.m_rho_stderror;
            this.m_rho_pvalor = 2.0 * (1.0 - normal.cdf(Math.Abs(m_rho_tstat)));
            this.m_rho_liminf_ci = m_rho_hat - cv * m_rho_stderror;
            this.m_rho_limsup_ci = m_rho_hat + cv * m_rho_stderror;

            this.m_lambda_tstat = this.m_lambda_hat / this.m_lambda_stderror;
            this.m_lambda_pvalor = 2.0 * (1.0 - normal.cdf(Math.Abs(m_lambda_tstat)));
            this.m_lambda_liminf_ci = m_lambda_hat - cv * m_lambda_stderror;
            this.m_lambda_limsup_ci = m_lambda_hat + cv * m_lambda_stderror;

            //============================ estatísticas de saída ==============================================================//

            this.m_loglik = this.loglik_SAC(x);
            this.m_aic = -2.0 * this.m_loglik + 2.0 * ((double)this.m_beta_hat.GetLength(0) + 3.0);
            this.m_bic = -2.0 * this.m_loglik + Math.Log((double)m_nobs) * ((double)this.m_beta_hat.GetLength(0) + 3.0);

            this.LikelihoodRatioTestStatRhoLambda = 2.0 * Math.Abs(this.loglik_SAC(x) - this.loglik_SAC(new double[2,1]));
            this.LikelihoodRatioTestPvalueRhoLambda = 2.0 * (1.0 - normal.cdf(Math.Abs(Math.Sqrt(this.LikelihoodRatioTestStatRhoLambda))));

            double ropt = optimiza_modelo_SAC_rho(lmin, lmax);
            double lopt = optimiza_modelo_SAC_lambda(lmin, lmax);

            m_rho_com_lambda_nulo = ropt;
            m_lambda_com_rho_nulo = lopt;

            this.LikelihoodRatioTestStatRho = 2.0 * Math.Abs(this.loglik_SAC(x) - this.loglik_SAC_lambda(lopt));
            this.LikelihoodRatioTestPvalueRho = 2.0 * (1.0 - normal.cdf(Math.Abs(Math.Sqrt(this.LikelihoodRatioTestStatRho))));

            this.LikelihoodRatioTestStatLambda = 2.0 * Math.Abs(this.loglik_SAC(x) - this.loglik_SAC_rho(ropt));
            this.LikelihoodRatioTestPvalueLambda = 2.0 * (1.0 - normal.cdf(Math.Abs(Math.Sqrt(this.LikelihoodRatioTestStatLambda))));

            //================================= geração de novas variáveis ====================================================//

            double[,] y = m_Y;
            double[,] y_hat_conditional = clt.MatrizSoma(clt.MatrizMult(m_rho_hat, fme.MatrizMult(m_W_esparsa, m_Y)), 
                                                                clt.MatrizMult(this.m_X, this.m_beta_hat));            
            double[,] residuos = clt.MatrizSubtracao(m_Y, y_hat_conditional);
            double[,] epsilon = e;
            double var_residuos = clt.VarianciaColumnMatrix(residuos);
            double[,] observacoes = new double[y.GetLength(0), 1];
            for (int i = 0; i < y.GetLength(0); i++)
            {
                observacoes[i, 0] = (double)(i + 1);
            }
            double var_epsilon = clt.VarianciaColumnMatrix(epsilon);

            double[,] Wepsilon = fme.MatrizMult(m_W_esparsa, epsilon);
            double[,] Wresiduo = fme.MatrizMult(m_W_esparsa, residuos);

            this.m_variaveis_geradas = clt.Concateh(observacoes, y);
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, y_hat_conditional);
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, residuos);
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, clt.MatrizDiv(residuos, Math.Sqrt(var_residuos)));
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, Wresiduo);
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, epsilon);
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, clt.MatrizDiv(epsilon, Math.Sqrt(var_epsilon)));
            this.m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, Wepsilon);

            this.m_nomes_variaveis_geradas = new string[9];
            this.m_nomes_variaveis_geradas[0] = "Observacao_";
            this.m_nomes_variaveis_geradas[1] = "Y_observado_";
            this.m_nomes_variaveis_geradas[2] = "Y_predito_condicional_";
            this.m_nomes_variaveis_geradas[3] = "Residuo_";
            this.m_nomes_variaveis_geradas[4] = "Residuo_padronizado_";
            this.m_nomes_variaveis_geradas[5] = "SpatialW_residuo_";
            this.m_nomes_variaveis_geradas[6] = "Epsilon_";
            this.m_nomes_variaveis_geradas[7] = "Epsilon_padronizado_";
            this.m_nomes_variaveis_geradas[8] = "SpatialW_epsilon_";
        }

        #endregion

        #region rotinas para estimação via MLE dos modelos SAC

        private double optimiza_modelo_SAC_lambda(double lmin, double lmax)
        {
            clsUtilOptimization opt = new clsUtilOptimization();
            clsUtilOptimization.FunctionUnivariate func2 = new clsUtilOptimization.FunctionUnivariate(this.minusloglik_SAC_lambda);
            double fval = 0.0;
            return opt.MinBrentSearch(func2, lmin, lmax, ref fval);
        }

        private double optimiza_modelo_SAC_rho(double rmin, double rmax)
        {
            clsUtilOptimization opt = new clsUtilOptimization();
            clsUtilOptimization.FunctionUnivariate func1 = new clsUtilOptimization.FunctionUnivariate(this.minusloglik_SAC_rho);
            double fval = 0.0;
            return opt.MinBrentSearch(func1, rmin, rmax, ref fval);
        }

        private double loglik_SAC_lambda(double lambda)
        {
            return -minusloglik_SAC_lambda(lambda) - ((double)m_nobs / 2.0) * (1.0 - Math.Log((double)m_nobs) + Math.Log(Math.PI));
        }

        private double loglik_SAC_rho(double rho)
        {
            return -minusloglik_SAC_rho(rho) - ((double)m_nobs / 2.0) * (1.0 - Math.Log((double)m_nobs) + Math.Log(Math.PI));
        }

        private double minusloglik_SAC_lambda(double lambda)
        {
            double[,] x = new double[2, 1];
            x[1, 0] = lambda;
            return minusloglik_SAC(x);
        }

        private double minusloglik_SAC_rho(double rho)
        {
            double[,] x = new double[2, 1];
            x[0, 0] = rho;
            return minusloglik_SAC(x);
        }

        private double[,] diff2loglik_SAC(double[,] parms)
        {
            double h = 1.0e-4;
            double[,] x0 = clt.ArrayDoubleClone(parms);

            double[,] xlow = clt.ArrayDoubleClone(parms);
            double[,] xhig = clt.ArrayDoubleClone(parms);
            xhig[0,0] = xhig[0,0] + 2.0 * h;
            xlow[0,0] = xlow[0,0] - 2.0 * h;
            double p11 = (minusloglik_SAC(xhig) - 2.0 * minusloglik_SAC(x0) + minusloglik_SAC(xlow)) / (4.0 * h * h);

            xlow = clt.ArrayDoubleClone(parms);
            xhig = clt.ArrayDoubleClone(parms);
            xhig[1, 0] = xhig[1, 0] + 2.0 * h;
            xlow[1, 0] = xlow[1, 0] - 2.0 * h;
            double p22 = (minusloglik_SAC(xhig) - 2.0 * minusloglik_SAC(x0) + minusloglik_SAC(xlow)) / (4.0 * h * h);

            double[,] x11 = clt.ArrayDoubleClone(parms);
            double[,] x12 = clt.ArrayDoubleClone(parms);
            double[,] x21 = clt.ArrayDoubleClone(parms);
            double[,] x22 = clt.ArrayDoubleClone(parms);

            x22[0, 0] += h;
            x22[1, 0] += h; 

            x11[0, 0] -= h;
            x11[1, 0] -= h;

            x12[0, 0] -= h;
            x12[1, 0] += h;

            x21[0, 0] += h;
            x21[1, 0] -= h;

            double p12 = (minusloglik_SAC(x22) + minusloglik_SAC(x11) - minusloglik_SAC(x12) - minusloglik_SAC(x21)) / (4.0 * h * h);

            double[,] res = new double[2,2];
            res[0,0] = p11;
            res[1,1] = p22;
            res[0,1] = res[1,0] = p12;

            return clt.MatrizMult(-1.0, res);
        }

        private double loglik_SAC(double[,] parms)
        {
            return -minusloglik_SAC(parms) - ((double)m_nobs / 2.0) * (1.0 - Math.Log((double)m_nobs) + Math.Log(Math.PI));
        }

        private double minusloglik_SAC(double[,] parms)
        {
            double rho = parms[0, 0];
            double lambda = parms[1, 0];

            double[,] Ay = fme.MatrizMult(fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, -rho), m_Y);
            double[,] BAy = fme.MatrizMult(fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, -lambda), Ay);
            double[,] Bx = fme.MatrizMult(fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, -lambda), m_X);

            double[,] XXinv = clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(Bx), Bx));
            double[,] beta = clt.MatrizMult(XXinv, (clt.MatrizMult(clt.MatrizTransp(Bx), BAy)));

            double[,] v = clt.MatrizSubtracao(BAy, clt.MatrizMult(Bx, beta));
            double vpv = (clt.MatrizMult(clt.MatrizTransp(v), v))[0, 0];

            double llike = (((double)m_nobs) / 2.0) * Math.Log(vpv) 
                            - m_W_esparsa.Spline.interp(rho)
                            - m_W_esparsa.Spline.interp(lambda);
            return llike;
        }

        #endregion

        #endregion

        #region matriz de covariância para modelos SAR

        private void CovarianceMatrixSARModels(out double[,] covmat_beta, out double[,] covmat_all, out double var_rho)
        {
            this.m_nobs = this.m_Y.GetLength(0);

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();

            double[,] y_tilde = fme.MatrizMult(this.m_W_esparsa, this.m_Y);
            double[,] y_hat = clt.MatrizMult(this.m_rho_hat, y_tilde);
            y_hat = clt.MatrizSoma(y_hat, clt.MatrizMult(this.m_X, this.m_beta_hat));
            double[,] erro_hat = clt.MatrizSubtracao(y_hat, m_Y);
            double sigma2 = ((clt.MatrizMult(clt.MatrizTransp(erro_hat), erro_hat))[0,0]) / ((double)m_nobs);
            double[,] XtX = clt.MatrizMult(clt.MatrizTransp(m_X), m_X);

            double[,] A22 = clt.MatrizMult(-1.0 / sigma2, XtX);
            double[,] A33 = new double[1, 1];
            A33[0, 0] = -((double)m_nobs) / (2.0 * Math.Pow(sigma2, 2.0));
            double[,] A23 = new double[XtX.GetLength(0), 1];
            double[,] A12 = clt.MatrizMult(-1.0 / sigma2, clt.MatrizMult(clt.MatrizTransp(y_tilde), m_X));
            double[,] aux_B = fme.MatrizMult(fme.MatrizSoma(m_W_esparsa, fme.MatrizTransp(m_W_esparsa), 1.0, 1.0), m_Y);
            double B = (clt.MatrizMult(clt.MatrizTransp(m_Y), aux_B))[0,0];
            double C = (clt.MatrizMult(clt.MatrizTransp(y_tilde), y_tilde))[0,0];

            double aux_A13 = (clt.MatrizMult(clt.MatrizTransp(y_tilde), clt.MatrizMult(m_X, this.m_beta_hat)))[0,0];
            double[,] A13 = new double[1, 1];
            A13[0,0] = (2.0 * C - B + 2.0 * aux_A13) / (2.0 * Math.Pow(sigma2, 2.0));

            double[,] v = clt.Concatev(clt.MatrizTransp(A12), A13);
            double[,] aux1_Q = clt.Concateh(A22, A23);
            double[,] aux2_Q = clt.Concateh(clt.MatrizTransp(A23), A33);
            double[,] aux_Q = clt.Concatev(aux1_Q, aux2_Q);
            aux_Q = clt.MatrizInversa(aux_Q);
            aux_Q = clt.MatrizMult(clt.MatrizTransp(v), aux_Q);
            aux_Q = clt.MatrizMult(aux_Q, v);
            double Q = aux_Q[0, 0];
            double diff2Lp = 0.0;

            diff2Lp = this.diff2loglik_SAR(this.m_rho_hat);

            double[,] A11 = new double[1, 1];
            A11[0, 0] = diff2Lp + Q;

            double[,] H1 = clt.Concateh(A11, clt.Concateh(A12, A13));
            double[,] H2 = clt.Concateh(clt.Concateh(clt.MatrizTransp(A12), A22), A23);
            double[,] H3 = clt.Concateh(clt.Concateh(clt.MatrizTransp(A13), clt.MatrizTransp(A23)), A33);

            double[,] H = clt.Concatev(H1, clt.Concatev(H2, H3));
            double[,] cov = clt.MatrizInversa(clt.MatrizMult(-1.0, H));

            covmat_all = cov;
            var_rho = cov[0, 0];
            covmat_beta = clt.SubMatriz(cov, 1, cov.GetLength(0) - 2, 1, cov.GetLength(0) - 2);
        }

        #endregion

        #region matriz de covariância para modelos SEM

        private void CovarianceMatrixSEMModels(out double[,] covmat_beta, out double[,] covmat_all, out double var_rho)
        {
            this.m_nobs = this.m_Y.GetLength(0);

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();

            clsMatrizEsparsa B = fme.MatrizSoma(fme.Identity(m_nobs), m_W_esparsa, 1.0, -m_rho_hat);
            double[,] BX = fme.MatrizMult(B, m_X);
            double[,] erro = clt.MatrizSubtracao(m_Y, clt.MatrizMult(m_X, m_beta_hat));
            double[,] Berro = fme.MatrizMult(B, erro);
            double[,] D = fme.MatrizMult(m_W_esparsa, erro);

            double sigma2 = (clt.MatrizMult(clt.MatrizTransp(Berro), Berro))[0,0] / ((double)m_nobs);

            double[,] A22 = clt.MatrizMult(-1.0 / sigma2, clt.MatrizMult(clt.MatrizTransp(BX), BX));
            double[,] A33 = new double[1, 1];
            A33[0, 0] = -((double)m_nobs) / (2.0 * Math.Pow(sigma2, 2.0));
            clsMatrizEsparsa aux_1 = fme.MatrizMult(fme.MatrizTransp(B), m_W_esparsa);
            clsMatrizEsparsa aux_2 = fme.MatrizSoma(fme.MatrizTransp(aux_1), aux_1, 1.0, 1.0);
            double[,] A12 = clt.MatrizMult(-1.0 / sigma2, clt.MatrizMult(clt.MatrizTransp(erro), fme.MatrizMult(aux_2, m_X)));
            double[,] A13 = clt.MatrizMult(-1.0 / Math.Pow(sigma2, 2.0), clt.MatrizMult(clt.MatrizTransp(Berro), D));
            //double[,] A23 = clt.MatrizMult(-1.0 / Math.Pow(sigma2, 2.0), clt.MatrizMult(clt.MatrizTransp(Berro), BX));
            double[,] A23 = new double[A22.GetLength(0), 1];

            double[,] v = clt.Concatev(clt.MatrizTransp(A12), A13);
            double[,] aux1_Q = clt.Concateh(A22, A23);
            double[,] aux2_Q = clt.Concateh(clt.MatrizTransp(A23), A33);
            double[,] aux_Q = clt.Concatev(aux1_Q, aux2_Q);
            aux_Q = clt.MatrizInversa(aux_Q);
            aux_Q = clt.MatrizMult(clt.MatrizTransp(v), aux_Q);
            aux_Q = clt.MatrizMult(aux_Q, v);
            double Q = aux_Q[0, 0];
            double diff2Lp = 0.0;

            diff2Lp = this.diff2loglik_SEM(this.m_rho_hat);

            double[,] A11 = new double[1, 1];
            A11[0, 0] = diff2Lp + Q;

            double[,] H1 = clt.Concateh(A11, clt.Concateh(A12, A13));
            double[,] H2 = clt.Concateh(clt.Concateh(clt.MatrizTransp(A12), A22), A23);
            double[,] H3 = clt.Concateh(clt.Concateh(clt.MatrizTransp(A13), clt.MatrizTransp(A23)), A33);

            double[,] H = clt.Concatev(H1, clt.Concatev(H2, H3));
            double[,] cov = clt.MatrizInversa(clt.MatrizMult(-1.0, H));

            covmat_all = cov;
            var_rho = cov[0, 0];
            covmat_beta = clt.SubMatriz(cov, 1, cov.GetLength(0) - 2, 1, cov.GetLength(0) - 2);
        }

        #endregion

        #region matriz de covariância para modelos SAC

        private void CovarianceMatrixSACModels(out double[,] covmat_beta, out double[,] covmat_all, out double var_rho, out double var_lambda)
        {
            this.m_nobs = this.m_Y.GetLength(0);

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();

            clsMatrizEsparsa A = fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, - m_rho_hat);
            clsMatrizEsparsa B = fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, - m_lambda_hat);
            double[,] Ay = fme.MatrizMult(A, m_Y);
            double[,] BAy = fme.MatrizMult(B, Ay);
            double[,] Bx = fme.MatrizMult(B, m_X);
            double[,] BxBeta = clt.MatrizMult(Bx, m_beta_hat);

            double[,] eX = clt.MatrizSubtracao(Ay, clt.MatrizMult(m_X, m_beta_hat));
            double[,] e = clt.MatrizSubtracao(BAy, BxBeta);
            double epe = (clt.MatrizMult(clt.MatrizTransp(e), e))[0, 0];
            m_sigma2_hat = epe / ((double)m_nobs);

            double[,] F13 = fme.MatrizMult(fme.MatrizTransp(m_W_esparsa), fme.MatrizMult(fme.MatrizTransp(B), fme.MatrizMult(B, m_X)));
            F13 = clt.MatrizMult(-1.0 / m_sigma2_hat, clt.MatrizMult(clt.MatrizTransp(m_Y), F13));

            double[,] aux1 = clt.MatrizMult(clt.MatrizTransp(e), fme.MatrizMult(m_W_esparsa, m_X));
            double[,] aux2 = clt.MatrizMult(clt.MatrizTransp(eX), fme.MatrizMult(fme.MatrizTransp(m_W_esparsa), fme.MatrizMult(B, m_X)));
            double[,] F23 = clt.MatrizMult(-1.0 / m_sigma2_hat, clt.MatrizSoma(aux1, aux2));

            double[,] F33 = clt.MatrizMult(-1.0 / m_sigma2_hat, clt.MatrizMult(clt.MatrizTransp(Bx), Bx));

            double[,] F14 = clt.MatrizMult(-1.0 / Math.Pow(m_sigma2_hat, 2.0), 
                                clt.MatrizMult(clt.MatrizTransp(e), fme.MatrizMult(B, fme.MatrizMult(m_W_esparsa, m_Y))));

            double[,] F24 = clt.MatrizMult(-1.0 / Math.Pow(m_sigma2_hat, 2.0),
                                clt.MatrizMult(clt.MatrizTransp(e), fme.MatrizMult(m_W_esparsa, eX)));

            double[,] F34 = clt.MatrizTransp(clt.MatrizMult(-1.0 / Math.Pow(m_sigma2_hat, 2.0), 
                clt.MatrizMult(clt.MatrizTransp(e), fme.MatrizMult(B, m_X))));

            double[,] F44 = new double[1, 1];
            F44[0, 0] = -((double)m_nobs) / (2.0 * Math.Pow(m_sigma2_hat, 2.0));

            double[,] v = clt.Concatev(clt.Concateh(F13, F14), clt.Concateh(F23, F24));
            double[,] aux_Q = clt.Concatev(clt.Concateh(F33, F34), clt.Concateh(clt.MatrizTransp(F34), F44));

            double[,] Q = clt.MatrizMult(clt.MatrizMult(v, clt.MatrizInversa(aux_Q)), clt.MatrizTransp(v));

            double[,] parms = new double[2,1];
            parms[0, 0] = m_rho_hat;
            parms[1, 0] = m_lambda_hat;
            double[,] diff2 = this.diff2loglik_SAC(parms);

            double[,] F11 = clt.MatrizSoma(diff2, Q);

            double[,] H1 = clt.Concateh(F11, v);
            double[,] H2 = clt.Concateh(clt.MatrizTransp(v), aux_Q);
            double[,] H = clt.Concatev(H1, H2);

            covmat_all = clt.MatrizMult(-1.0, clt.MatrizInversa(H));
            covmat_beta = clt.SubMatriz(covmat_all, 2, covmat_all.GetLength(0)-2, 2, covmat_all.GetLength(1) - 2);
            var_lambda = covmat_all[1,1];
            var_rho = covmat_all[0,0];
        }

        #endregion
    }
}
