using System;
using System.Collections.Generic;
using System.Text;

namespace IpeaGEO
{
    public class clsLinearRegressionModelsMLE : clsModelosRegressaoEspacial
    {
        public clsLinearRegressionModelsMLE()
        {
        }

        #region internal variables
        
        private clsUtilOptimization opt = new clsUtilOptimization();
        private clsUtilTools clt = new clsUtilTools();
        private double[,] e_O = new double[0, 0];
        private double[,] e_L = new double[0, 0];

        #endregion

        #region estimação dos modelos SEM

        /// <summary>
        /// Retorna a função de loglikelihood profile de um modelo SEM. Essa função é utilizada
        /// especificamente para estimarmos a desvio padrão do estimador do parâmetro de autocorrelação. 
        /// Nesse caso, é preciso encontrar a segunda derivada dessa função, em relação ao parâmetro rho.
        /// </summary>
        /// <param name="rho">Parâmetro de autocorrelação espacial dos resíduos.</param>
        /// <returns>Função de loglikelihood.</returns>
        private double loglikSEM(double rho)
        {
            double[,] B = clt.MatrizSubtracao(clt.unit(m_nobs), clt.MatrizMult(rho, this.m_W));

            double[,] X_tilde = clt.MatrizMult(B, this.m_X);
            double[,] y_tilde = clt.MatrizMult(B, this.m_Y);

            double[,] beta = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(X_tilde), X_tilde)),
                            clt.MatrizMult(clt.MatrizTransp(X_tilde), y_tilde));

            e_O = clt.MatrizSubtracao(this.m_Y, clt.MatrizMult(this.m_X, beta));

            double[,] e_tilde = clt.MatrizMult(B, e_O);
            double sigma_2 = (clt.MatrizMult(clt.MatrizTransp(e_tilde), e_tilde))[0, 0] / (double)m_nobs;

            return -(((double)m_nobs) / 2.0) * (Math.Log(2.0 * Math.PI) + Math.Log(sigma_2)) + clt.LogDet(B);
        }

        /// <summary>
        /// Retorna o negativo da função de profile loglikelihood de uma modelo SEM. Essa função é utilizada
        /// especificamente para estimação dos parâmetros do modelo, a partir do algoritmo iterativo. 
        /// </summary>
        /// <param name="rho">Parâmetro de autocorrelação espacial dos erros.</param>
        /// <returns>Oposto da função de profile likelihood.</returns>
        private double minusloglikSEM(double rho)
        {
            double[,] B = clt.MatrizSubtracao(clt.unit(m_nobs), clt.MatrizMult(rho, this.m_W));
            double ldet = clt.LogDet(B);

            double[,] e_tilde = clt.MatrizMult(B, e_O);
            double v = (clt.MatrizMult(clt.MatrizTransp(e_tilde), e_tilde))[0, 0] / (double)m_nobs;

            return - (ldet - Math.Log(v) * ((double)m_nobs) / 2.0);
        }
        
        /// <summary>
        /// Função para estimar os modelos de spatial error models (SEM).
        /// </summary>
        public void EstimateModeloSEM()
        {
            this.m_nobs = this.m_Y.GetLength(0);

            if (this.m_W.GetLength(0) == 0) this.MatrizWFromVizinhos();

            double[,] beta_new = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(this.m_X), this.m_X)),
                                clt.MatrizMult(clt.MatrizTransp(this.m_X), this.m_Y));

            e_O = clt.MatrizSubtracao(this.m_Y, clt.MatrizMult(this.m_X, beta_new));

            double[,] V = new double[0, 0];
            double[] D = new double[0];
            clt.AutovaloresMatrizSimetrica(this.m_W, ref V, ref D);

            double lambda_min = clt.Min(D);
            double lambda_max = clt.Max(D);

            double xmin = 1.0 / lambda_max;
            double xmax = 1.0 / lambda_min;

            clsUtilOptimization.FunctionUnivariate func = new clsUtilOptimization.FunctionUnivariate(this.minusloglikSEM);
            double fval = 0.0;

            double rho_old = 0.0;
            double rho_new = 0.0;

            rho_new = opt.MinBrentSearch(func, xmin, xmax, ref fval);
            rho_old = rho_new;

            int max_iter = 100;
            double tolerancia = 1.0e-6;

            double[,] B = new double[0,0];
            double[,] X_tilde = new double[0, 0];
            double[,] y_tilde = new double[0, 0];
            double[,] beta_old = clt.ArrayDoubleClone(beta_new);

            for (int iter = 0; iter < max_iter; iter++)
            {
                B = clt.MatrizSubtracao(clt.unit(m_nobs), clt.MatrizMult(rho_new, this.m_W));

                X_tilde = clt.MatrizMult(B, this.m_X);
                y_tilde = clt.MatrizMult(B, this.m_Y);

                beta_new = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(X_tilde), X_tilde)),
                                clt.MatrizMult(clt.MatrizTransp(X_tilde), y_tilde));
                
                e_O = clt.MatrizSubtracao(this.m_Y, clt.MatrizMult(this.m_X, beta_new));

                rho_new = opt.MinBrentSearch(func, xmin, xmax, ref fval);

                if (clt.Norm(clt.MatrizSubtracao(beta_new, beta_old)) < tolerancia &&
                    Math.Abs(rho_new - rho_old) < tolerancia)
                {
                    break;
                }

                rho_old = rho_new;
                beta_old = clt.ArrayDoubleClone(beta_new);
            }

            double[,] e_tilde = clt.MatrizMult(B, e_O);
            this.m_sigma2_hat = (clt.MatrizMult(clt.MatrizTransp(e_tilde), e_tilde))[0, 0] / (double)m_nobs;
            this.m_beta_hat = beta_new;
            this.m_rho_hat = rho_new;

            this.m_beta_hat_cov = clt.MatrizMult(this.m_sigma2_hat, clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(X_tilde), X_tilde)));

            this.GeraSignificanciaCoeficientes();

            this.m_loglik = -minusloglikSEM(rho_new) - (((double)m_nobs) / 2.0) * Math.Log(2.0 * Math.PI);
            this.m_aic = -2.0 * this.m_loglik + 2.0 * ((double)this.m_beta_hat.GetLength(0) + 2.0);
            this.m_bic = -2.0 * this.m_loglik + Math.Log((double)m_nobs) * ((double)this.m_beta_hat.GetLength(0) + 2.0);

            clsUtilOptimization.FunctionUnivariate f = new clsUtilOptimization.FunctionUnivariate(this.loglikSEM);
            this.m_rho_stderror = - 1.0 / opt.Diff2Function(f, this.m_rho_hat);
            this.m_rho_stderror = Math.Sqrt(this.m_rho_stderror);

            MathNormaldist normal = new MathNormaldist();

            double prob_cauda = this.m_prob_confidence_intervals / 200.0;
            double cv = normal.invcdf(1.0 - prob_cauda);

            this.m_rho_tstat = this.m_rho_hat / this.m_rho_stderror;
            this.m_rho_pvalor = 2.0 * (1.0 - normal.cdf(Math.Abs(m_rho_tstat)));
            m_rho_liminf_ci = m_rho_hat - cv * m_rho_stderror;
            m_rho_limsup_ci = m_rho_hat + cv * m_rho_stderror;
        }

        #endregion

        #region estimação dos modelos SAR

        private double minusloglikSAR(double rho)
        {
            double[,] aux1 = clt.MatrizSubtracao(clt.unit(m_nobs), clt.MatrizMult(rho, this.m_W));
            double res = clt.LogDet(aux1);

            aux1 = clt.MatrizSubtracao(e_O, clt.MatrizMult(rho, e_L));
            double aux2 = -(Math.Log((clt.MatrizMult(clt.MatrizTransp(aux1), aux1))[0, 0] / (double)m_nobs)) * ((double)m_nobs / 2.0);

            return -(res + aux2);
        }

        public void EstimateModeloSAR()
        {
            this.m_nobs = this.m_Y.GetLength(0);

            if (this.m_W.GetLength(0) == 0) this.MatrizWFromVizinhos();

            double[,] beta_O = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(this.m_X), this.m_X)),
                                clt.MatrizMult(clt.MatrizTransp(this.m_X), this.m_Y));

            double[,] y_tilde = clt.MatrizMult(this.m_W, this.m_Y);

            double[,] beta_L = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(this.m_X), this.m_X)),
                                clt.MatrizMult(clt.MatrizTransp(this.m_X), y_tilde));

            e_O = clt.MatrizSubtracao(this.m_Y, clt.MatrizMult(this.m_X, beta_O));
            e_L = clt.MatrizSubtracao(y_tilde, clt.MatrizMult(this.m_X, beta_L));

            double[,] V = new double[0,0];
            double[] D = new double[0];
            clt.AutovaloresMatrizSimetrica(this.m_W, ref V, ref D);

            double lambda_min = clt.Min(D);
            double lambda_max = clt.Max(D);

            double xmin = 1.0 / lambda_max;
            double xmax = 1.0 / lambda_min;

            clsUtilOptimization.FunctionUnivariate func = new clsUtilOptimization.FunctionUnivariate(this.minusloglikSAR);
            double fval = 0.0;

            this.m_rho_hat = opt.MinBrentSearch(func, xmin, xmax, ref fval);
            this.m_beta_hat = clt.MatrizSubtracao(beta_O, clt.MatrizMult(this.m_rho_hat, beta_L));
            double[,] aux1 = clt.MatrizSubtracao(e_O, clt.MatrizMult(m_rho_hat, e_L));
            this.m_sigma2_hat = (clt.MatrizMult(clt.MatrizTransp(aux1), aux1))[0, 0] / (double)m_nobs;
        }

        #endregion
    }
}
