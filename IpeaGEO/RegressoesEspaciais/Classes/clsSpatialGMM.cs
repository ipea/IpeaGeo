using System;
using System.Collections.Generic;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsSpatialGMM
    {
        public clsSpatialGMM()
        {
        }

        #region variáveis internas

        private clsUtilTools clt = new clsUtilTools();
        
        protected double[,] m_variaveis_geradas = new double[0, 0];
        public double[,] VariaveisGeradas
        {
            get { return this.m_variaveis_geradas; }
        }

        #endregion

        #region calculo da matriz de peso do GMM

        public double[,] GMM_weight_matrix(double[,] gt_vector)
        {
            double[,] dgt_vector = new double[gt_vector.GetLength(0), gt_vector.GetLength(1)];
            double[,] media = clt.Meanc(gt_vector);
            for (int i = 0; i < gt_vector.GetLength(0); i++)
            {
                for (int j = 0; j < gt_vector.GetLength(1); j++)
                {
                    dgt_vector[i, j] = gt_vector[i, j] - media[0, j];
                }
            }
            return clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(dgt_vector), dgt_vector),
                                        1.0 / (double)dgt_vector.GetLength(0)));

            //return clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(gt_vector), gt_vector), 
            //                                                      1.0/(double)gt_vector.GetLength(0)));
        }

        public double[,] Spatial_GMM_weight_matrix(double[,] gt_vector_in, double[,] x_coord, double[,] y_coord, double cutoff_x, double cutoff_y)
        {
            double[,] gt_vector = new double[gt_vector_in.GetLength(0), gt_vector_in.GetLength(1)];
            double[,] media = clt.Meanc(gt_vector_in);
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
	        for (int i=0; i<T; i++) 
            {
		        for (int j=0; j<T; j++) 
                {
			        D_horizontal = Math.Abs(x_coord[i,0] - x_coord[j,0]);
			        D_vertical = Math.Abs(y_coord[i,0] - y_coord[j,0]);
			        if ((D_horizontal <= cutoff_x) && (D_vertical <= cutoff_y)) 
                    {
				        peso = (1.0 - (D_horizontal / cutoff_x))*(1.0 - (D_vertical / cutoff_y));	
				        Omega_hat = clt.MatrizSoma(Omega_hat, 
                            clt.MatrizMult(peso, clt.MatrizMult(clt.MatrizTransp(clt.SubRowArrayDouble(gt_vector, i)), clt.SubRowArrayDouble(gt_vector,j))));
				        contador++;
			        }
		        }
	        }
	        Omega_hat = clt.MatrizMult(Omega_hat, 1.0 / (double)gt_vector.GetLength(0));	
	        return clt.MatrizInversa(Omega_hat);			
        }       
        
        protected double[,] W_matrix = new double[0,0];
        protected int k = 0;
        protected int T = 0;
        protected double[,] Y = new double[0,0];
        protected double[,] X = new double[0,0];
        protected double[,] Z = new double[0,0];
        protected int m = 0;

        #endregion

        #region estimação via OLS

        public void Simple_OLS_estimation(double[,] Y_in, double[,] X_in, ref double[,] adBeta,
                              ref double[,] adCovBeta, ref double[,] adStdError, ref double[,] adTstat, 
                              ref double[,] adPvalue, ref double adSigma2)
        {
	        X = clt.ArrayDoubleClone(X_in);
	        Y = clt.ArrayDoubleClone(Y_in);
	        T = X.GetLength(0);
	        k = X.GetLength(1);
		   
            double[,] XXinv = clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(X), X));
            double[,] beta_hat = clt.MatrizMult(XXinv, (clt.MatrizMult(clt.MatrizTransp(X), Y))); 

            double[,] erro = clt.MatrizSubtracao(Y, clt.MatrizMult(X, beta_hat));
            double sigma2_hat = clt.MatrizMult(clt.MatrizTransp(erro), erro)[0,0] / (double)(T-k);

	        adBeta = clt.ArrayDoubleClone(beta_hat);
	        adCovBeta = clt.MatrizMult(sigma2_hat, XXinv);
            adStdError= new double[k,1];
            adTstat = new double[k,1];
            adPvalue = new double[k,1];

            MathNormaldist normal = new MathNormaldist();

            for (int i=0; i<k; i++)
            {
                adStdError[i,0] = Math.Sqrt(adCovBeta[i,i]);
                adTstat[i,0] = adBeta[i,0] / adStdError[i,0];
                adPvalue[i, 0] = 2.0 * (1.0 - normal.cdf(Math.Abs(adTstat[i, 0])));
            }

	        adSigma2 = sigma2_hat;

            double[,] y_hat = clt.MatrizMult(X, beta_hat);
            double[,] erro_std = clt.MatrizDiv(erro, Math.Sqrt(sigma2_hat));
            double[,] observacao = clt.Zeros(y_hat.GetLength(0), 1);
            for (int i = 0; i < observacao.GetLength(0); i++) observacao[i, 0] = (double)(i + 1);

            m_variaveis_geradas = clt.Concateh(observacao, Y);
            m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, y_hat);
            m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, erro);
            m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, erro_std);
        }

        #endregion

        #region estimação GMM não-espacial

        public void Limited_info_GMM_estimation(double[,] Y_in, double[,] X_in, double[,] Z_in,
                               ref double[,] adBeta, ref double[,] adCovBeta, ref double[,] adStdError, ref double[,] adTstat, ref double[,] adPvalue,
					           ref double adJstat, ref double adJpvalue, ref double adSigma2, 
                               bool iterate_till_convergence, double tol_iterate_till_convergence, int max_iterate_till_convergence, 
                               ref int num_iterations, ref string mensagem_iterations)
        {
	        X = clt.ArrayDoubleClone(X_in);
	        Y = clt.ArrayDoubleClone(Y_in);
	        Z = clt.ArrayDoubleClone(Z_in);	
	        T = clt.rows(X);
	        k = clt.columns(X);
	        m = clt.columns(Z);
	        W_matrix = clt.unit(m);

	        double[,] A_T = clt.MatrizMult(clt.MatrizTransp(Z), X);
	        double[,] B_T = clt.MatrizMult(clt.MatrizTransp(Z), Y);
            
	        double[,] beta_hat = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), A_T)),
                                                clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), B_T));

            double[,] erro = clt.MatrizSubtracao(Y, clt.MatrizMult(X, beta_hat));
            double[,] gt_vector = new double[Z.GetLength(0),Z.GetLength(1)];
            for (int i=0; i<Z.GetLength(0); i++)
            {
                for (int j=0; j<Z.GetLength(1); j++)
                {
                    gt_vector[i,j] = erro[i,0]*Z[i,j];
                }
            }

            W_matrix = this.GMM_weight_matrix(gt_vector);

  	        beta_hat = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), A_T)),
                                                clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), B_T));

            erro = clt.MatrizSubtracao(Y, clt.MatrizMult(X, beta_hat));
            for (int i=0; i<Z.GetLength(0); i++)
            {
                for (int j=0; j<Z.GetLength(1); j++)
                {
                    gt_vector[i,j] = erro[i,0]*Z[i,j];
                }
            }
            
            W_matrix = this.GMM_weight_matrix(gt_vector);

            int iter = 0;
            if (iterate_till_convergence)
            {
                double[,] beta_old;
                double diff_beta = 0.0;

                for (iter = 0; iter < max_iterate_till_convergence; iter++)
                {
                    beta_old = clt.ArrayDoubleClone(beta_hat);

                    beta_hat = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), A_T)),
                                        clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), B_T));

                    erro = clt.MatrizSubtracao(Y, clt.MatrizMult(X, beta_hat));
                    for (int i = 0; i < Z.GetLength(0); i++)
                    {
                        for (int j = 0; j < Z.GetLength(1); j++)
                        {
                            gt_vector[i, j] = erro[i, 0] * Z[i, j];
                        }
                    }

                    W_matrix = this.GMM_weight_matrix(gt_vector);

                    diff_beta = clt.Norm(clt.MatrizSubtracao(beta_hat, beta_old)) / (clt.Norm(beta_hat) + 1.0);
                    if (diff_beta < tol_iterate_till_convergence)
                    {
                        break;
                    }
                }
                if (iter <= max_iterate_till_convergence)
                {
                    mensagem_iterations = "Convergência atingida com sucesso";
                }
                else
                {
                    mensagem_iterations = "Convergência não atingida no limite de iterações especificado";
                }
            }
            iter++;
            num_iterations = iter;

            adSigma2 = clt.MatrizMult(clt.MatrizTransp(erro), erro)[0,0] / (double)(T-k);
	        adBeta = clt.ArrayDoubleClone(beta_hat);
	        adCovBeta = clt.MatrizMult((double)T, clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), A_T)));
            
            adStdError= new double[k,1];
            adTstat = new double[k,1];
            adPvalue = new double[k,1];

            MathNormaldist normal = new MathNormaldist();
            for (int i=0; i<k; i++)
            {
                adStdError[i,0] = Math.Sqrt(adCovBeta[i,i]);
                adTstat[i,0] = adBeta[i,0] / adStdError[i,0];
                adPvalue[i, 0] = 2.0 * (1.0 - normal.cdf(Math.Abs(adTstat[i, 0])));
            }

            double[,] media_gt = clt.Meanc(gt_vector);

            adJstat = (clt.MatrizMult((double)T, clt.MatrizMult(clt.MatrizMult(media_gt, W_matrix), clt.MatrizTransp(media_gt))))[0,0];

	        if (m-k > 0) 
            {
                MathChisqdist chi = new MathChisqdist(m - k);
                adJpvalue = 1.0 - chi.cdf(adJstat);
	        }
	        else 
            {
		        adJpvalue= 1.0;
            }

            double sigma2_hat = adSigma2;

            double[,] y_hat = clt.MatrizMult(X, beta_hat);
            double[,] erro_std = clt.MatrizDiv(erro, Math.Sqrt(sigma2_hat));
            double[,] observacao = clt.Zeros(y_hat.GetLength(0), 1);
            for (int i = 0; i < observacao.GetLength(0); i++) observacao[i, 0] = (double)(i + 1);

            m_variaveis_geradas = clt.Concateh(observacao, Y);
            m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, y_hat);
            m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, erro);
            m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, erro_std);
        }

        #endregion

        #region estimação GMM espacial

        public void Limited_info_spatial_GMM_estimation(double[,] Y_in, double[,] X_in, double[,] Z_in,
                                            double[,] x_coord, double[,] y_coord, double cutoff_x, double cutoff_y,
                                            ref double[,] adBeta, ref double[,] adCovBeta, ref double[,] adStdError, ref double[,] adTstat,
                                            ref double[,] adPvalue, ref double adJstat, ref double adJpvalue, ref double adSigma2,
                                            bool iterate_till_convergence, double tol_iterate_till_convergence, int max_iterate_till_convergence,
                                            ref int num_iterations, ref string mensagem_iterations)
        {
	        X = clt.ArrayDoubleClone(X_in);
	        Y = clt.ArrayDoubleClone(Y_in);
	        Z = clt.ArrayDoubleClone(Z_in);	
	        T = clt.rows(X);
	        k = clt.columns(X);
	        m = clt.columns(Z);
	        W_matrix = clt.unit(m);

	        double[,] A_T = clt.MatrizMult(clt.MatrizTransp(Z), X);
	        double[,] B_T = clt.MatrizMult(clt.MatrizTransp(Z), Y);
	        double[,] beta_hat = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), A_T)),
                                                clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), B_T));

            double[,] erro = clt.MatrizSubtracao(Y, clt.MatrizMult(X, beta_hat));
            double[,] gt_vector = new double[Z.GetLength(0),Z.GetLength(1)];
            for (int i=0; i<Z.GetLength(0); i++)
            {
                for (int j=0; j<Z.GetLength(1); j++)
                {
                    gt_vector[i,j] = erro[i,0]*Z[i,j];
                }
            }

	        W_matrix = this.Spatial_GMM_weight_matrix(gt_vector, x_coord, y_coord, cutoff_x, cutoff_y);

  	        beta_hat = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), A_T)),
                                                clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), B_T));

            erro = clt.MatrizSubtracao(Y, clt.MatrizMult(X, beta_hat));
            for (int i=0; i<Z.GetLength(0); i++)
            {
                for (int j=0; j<Z.GetLength(1); j++)
                {
                    gt_vector[i,j] = erro[i,0]*Z[i,j];
                }
            }

            W_matrix = this.Spatial_GMM_weight_matrix(gt_vector, x_coord, y_coord, cutoff_x, cutoff_y); 
            
            int iter = 0;
            if (iterate_till_convergence)
            {
                double[,] beta_old;
                double diff_beta = 0.0;

                for (iter = 0; iter < max_iterate_till_convergence; iter++)
                {
                    beta_old = clt.ArrayDoubleClone(beta_hat);

                    beta_hat = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), A_T)),
                                                        clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), B_T));

                    erro = clt.MatrizSubtracao(Y, clt.MatrizMult(X, beta_hat));
                    for (int i = 0; i < Z.GetLength(0); i++)
                    {
                        for (int j = 0; j < Z.GetLength(1); j++)
                        {
                            gt_vector[i, j] = erro[i, 0] * Z[i, j];
                        }
                    }

                    W_matrix = this.Spatial_GMM_weight_matrix(gt_vector, x_coord, y_coord, cutoff_x, cutoff_y); 

                    diff_beta = clt.Norm(clt.MatrizSubtracao(beta_hat, beta_old)) / (clt.Norm(beta_hat) + 1.0);
                    if (diff_beta < tol_iterate_till_convergence)
                    {
                        break;
                    }
                }
                if (iter <= max_iterate_till_convergence)
                {
                    mensagem_iterations = "Convergência atingida com sucesso";
                }
                else
                {
                    mensagem_iterations = "Convergência não atingida no limite de iterações especificado";
                }
            }
            iter++;
            num_iterations = iter;

            adSigma2 = clt.MatrizMult(clt.MatrizTransp(erro), erro)[0,0] / (double)(T-k);
            adBeta = clt.ArrayDoubleClone(beta_hat);
            adCovBeta = clt.MatrizMult((double)T, clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), A_T)));

            adStdError = new double[k, 1];
            adTstat = new double[k, 1];
            adPvalue = new double[k, 1];

            MathNormaldist normal = new MathNormaldist();
            for (int i = 0; i < k; i++)
            {
                adStdError[i, 0] = Math.Sqrt(adCovBeta[i, i]);
                adTstat[i, 0] = adBeta[i, 0] / adStdError[i, 0];
                adPvalue[i, 0] = 2.0 * (1.0 - normal.cdf(Math.Abs(adTstat[i, 0])));
            }

            double[,] media_gt = clt.Meanc(gt_vector);

            adJstat = (clt.MatrizMult((double)T, clt.MatrizMult(clt.MatrizMult(media_gt, W_matrix), clt.MatrizTransp(media_gt))))[0, 0];

            if (m - k > 0)
            {
                MathChisqdist chi = new MathChisqdist(m - k);
                adJpvalue = 1.0 - chi.cdf(adJstat);
            }
            else
            {
                adJpvalue = 1.0;
            }

            double sigma2_hat = adSigma2;

            double[,] y_hat = clt.MatrizMult(X, beta_hat);
            double[,] erro_std = clt.MatrizDiv(erro, Math.Sqrt(sigma2_hat));
            double[,] observacao = clt.Zeros(y_hat.GetLength(0), 1);
            for (int i = 0; i < observacao.GetLength(0); i++) observacao[i, 0] = (double)(i + 1);

            m_variaveis_geradas = clt.Concateh(observacao, Y);
            m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, y_hat);
            m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, erro);
            m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, erro_std);
        }

        #endregion

        #region estimação 2SLS

        public void Simple_2SLS_estimation(double[,] Y_in, double[,] X_in, double[,] Z_in,
                               ref double[,] adBeta, ref double[,] adCovBeta, 
                               ref double[,] adStdError, ref double[,] adTstat, ref double[,] adPvalue, ref double adSigma2)
        {
	        X = clt.ArrayDoubleClone(X_in);
	        Y = clt.ArrayDoubleClone(Y_in);
	        Z = clt.ArrayDoubleClone(Z_in);	
	        T = clt.rows(X);
	        k = clt.columns(X);
	        m = clt.columns(Z);

            W_matrix = clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(Z), Z));

	        double[,] A_T = clt.MatrizMult(clt.MatrizTransp(Z), X);
	        double[,] B_T = clt.MatrizMult(clt.MatrizTransp(Z), Y);
	        double[,] beta_hat = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), A_T)),
                                                clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), B_T));

            double[,] erro = clt.MatrizSubtracao(Y, clt.MatrizMult(X, beta_hat));
            adSigma2 = clt.MatrizMult(clt.MatrizTransp(erro), erro)[0,0] / (double)(T-k);
            adBeta = clt.ArrayDoubleClone(beta_hat);
            //adCovBeta = clt.MatrizMult(adSigma2, clt.MatrizMult((double)T, clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), A_T))));
            adCovBeta = clt.MatrizMult(adSigma2, clt.MatrizMult(1.0, clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(A_T), W_matrix), A_T))));

            adStdError = new double[k, 1];
            adTstat = new double[k, 1];
            adPvalue = new double[k, 1];

            double sigma2_hat = adSigma2;

            MathNormaldist normal = new MathNormaldist();
            for (int i = 0; i < k; i++)
            {
                adStdError[i, 0] = Math.Sqrt(adCovBeta[i, i]);
                adTstat[i, 0] = adBeta[i, 0] / adStdError[i, 0];
                adPvalue[i, 0] = 2.0 * (1.0 - normal.cdf(Math.Abs(adTstat[i, 0])));
            }

            double[,] y_hat = clt.MatrizMult(X, beta_hat);
            double[,] erro_std = clt.MatrizDiv(erro, Math.Sqrt(sigma2_hat));
            double[,] observacao = clt.Zeros(y_hat.GetLength(0), 1);
            for (int i = 0; i < observacao.GetLength(0); i++) observacao[i, 0] = (double)(i + 1);

            m_variaveis_geradas = clt.Concateh(observacao, Y);
            m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, y_hat);
            m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, erro);
            m_variaveis_geradas = clt.Concateh(m_variaveis_geradas, erro_std);
        }

        #endregion
    }
}
