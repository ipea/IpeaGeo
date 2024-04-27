using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Troschuetz.Random;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsLogDetMatrizEsparsa
    {
        public clsLogDetMatrizEsparsa()
        {
        }

        private clsFuncoesMatrizEsparsa m_fme = new clsFuncoesMatrizEsparsa();
        private clsUtilTools m_clt = new clsUtilTools();
        private NormalDistribution m_norm = new NormalDistribution();

        #region lndetit

        //% -----------------------------------------------------------------------------//
        //% PURPOSE: computes Pace and Barry's spline approximation to log det(I-rho*W)
        //% -----------------------------------------------------------------------------//
        //% USAGE: out = lndetint(W)
        //% where:    
        //%             W     = symmetric spatial weight matrix (standardized)
        //% -----------------------------------------------------------------------------//
        //% RETURNS: out = a structure variable
        //%          out.lndet = a vector of log determinants for 0 < rho < 1
        //%          out.rho   = a vector of rho values associated with lndet values
        //% -----------------------------------------------------------------------------//
        //% NOTES: only produces results for a grid of 0 < rho < 1
        //%        where the grid ranges by 0.01 increments
        //% -----------------------------------------------------------------------------//
        //% References: % R. Kelley Pace and Ronald P. Barry "Simulating Mixed Regressive
        //% Spatially autoregressive Estimators", Computational Statistics, 1998,
        //% Vol. 13, pp. 397-418.
        //% -----------------------------------------------------------------------------//    
        //% This function computes a vector of log-determinants for a vector 
        //% of AR parameters (alpha)
        //% It uses a spline interpolation routine to reduce the number of 
        //% determinants computed
        //% Written by Kelley Pace, 3/19/98
        //% (named fdetinterpasym1.m in the spatial statistics toolbox )
        //% Documentation and output format changed by J. LeSage
        //% for consistency with the Econometrics Toolbox 12/99
        //% -----------------------------------------------------------------------------//  

        public void lndetint(clsMatrizEsparsa wsw, double rmin, double rmax)
        {
            //spparms('tight');
            //spparms('autommd',0);
            //%these settings help optimize the sparse matrix routines

            clsMatrizEsparsa c=wsw.Clone();
            int n = wsw.m;
            clsMatrizEsparsa s1 = m_fme.Identity(n);
            clsMatrizEsparsa z = m_fme.MatrizSoma(s1, c, 1.0, -0.1);
            
            //[n,n]=size(c);
            //s1=speye(n);
            //z=s1-.1*c;
            //p=colmmd(z);
            //%this is the symmetric minimum degree ordering
            
            //iter=100;
            //alphavec=((1:iter)-1)/iter;
            //%selecting points to use for the interpolation
            //alphaselect=[ 10 20 40 50 60  70  80 85 90 95 96 97 98 99 100];
            //itersub=length(alphaselect);

            //detsub=zeros(itersub,1);
            //for i=1:itersub;
            //alpha=alphavec(alphaselect(i));
            //z=s1-alpha*c;
            //[l,u]=lu(z(:,p));
            //%LU decomposition
            //detsub(i)=sum(log(abs(diag(u))));
            //end;

            ////%stores grid and log-determinant for later use
            ////interpolating for finer grid of alpha
            //out.lndet = interp1([0;alphavec(alphaselect)'],[0;detsub],alphavec,'spline')';
            //out.rho = alphavec';
        }

        #endregion 

        #region lndetmc

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="iter"></param>
        /// <param name="wsw"></param>
        /// <param name="rmin"></param>
        /// <param name="rmax"></param>
        public void lndetmc(int order, int iter, clsMatrizEsparsa wsw, double rmin, double rmax, double rpasso,
            ref double[,] rho, ref double[,] lndet, ref double[,] low95, ref double[,] high95)
        {
            //if (rmin == null) rmin = 1.0e-5;
            //if (rmax == null) rmax = 1.0;

            int n = wsw.n;

            clsMatrizEsparsa s1 = m_fme.MatrizDotPower(wsw, 2.0);
            double soma = m_fme.SumMatrixElements(s1) / 2.0;
            double[,] td = new double[2,1];
            td[1,0] = soma;
            int oexact = td.GetLength(0);
            int o = order;

            double[,] mavmomi = m_clt.Zeros(o,iter);
            m_norm.Mu = 0.0;
            m_norm.Sigma = 1.0;
            double[,] u = new double[n, 1];
            double[,] v = new double[n, 1];

            for (int j = 0; j < iter; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    u[k, 0] = m_norm.NextDouble();
                }
                v = m_clt.ArrayDoubleClone(u);
                double[,] utu = m_clt.MatrizMult(m_clt.MatrizTransp(u), u);

                for (int i = 0; i < o; i++)
                {
                    v = m_fme.MatrizMult(wsw, v);
                    mavmomi[i, j] = ((double)n) / (((double)i+1)*utu[0,0]) * (m_clt.MatrizMult(m_clt.MatrizTransp(u), v))[0,0];
                }
            }

            for (int i = 0; i < oexact; i++)
            {
                for (int j = 0; j < mavmomi.GetLength(1); j++)
                {
                    mavmomi[i, j] = td[i, 0];
                }                
            }

            double[,] avmomi = new double[mavmomi.GetLength(0), 1];
            for (int i = 0; i < avmomi.GetLength(0); i++)
            {
                for (int j = 0; j < mavmomi.GetLength(1); j++)
                {
                    avmomi[i, 0] += mavmomi[i, j];
                }
                avmomi[i, 0] = avmomi[i, 0] / (double)mavmomi.GetLength(1);
            }

            //double[,] alpha = this.Range(rmin, 0.01, rmax);

            double[,] alpha = this.Range(rmin, rpasso, rmax);
            double[,] valpha = this.VandermondeMatrix(alpha);
            double[,] valphaf = this.Fliplr(valpha);

            double[,] alomat = new double[valphaf.GetLength(0), o];
            for (int i = 0; i < alomat.GetLength(0); i++)
            {
                for (int j = 0; j < o; j++)
                {
                    alomat[i, j] = - valphaf[i, j + 1];
                }
            }

            //%Estimated ln|I-aD| using mixture of exact, stochastic moments
            //%exact from 1 to oexact, stochastic from (oexact+1) to o
            double[,] lndetmat = m_clt.MatrizMult(alomat, avmomi);

            //%standard error computations
            double[,] srvs= m_clt.MatrizTransp(m_clt.MatrizMult(alomat, mavmomi));
            double[,] m1 = m_clt.Meanc(m_clt.MatrizDotMult(srvs, srvs));
            double[,] m2 = m_clt.MatrizDotPower(m_clt.Meanc(srvs), 2.0);
            double[,] sderr = m_clt.MatrizTransp(m_clt.MatrizDotPower(m_clt.MatrizDiv(m_clt.MatrizSubtracao(m1, m2), ((double)iter)), 0.5));

            double[,] fbound = m_clt.ArrayDoubleClone(sderr);

            rho = m_clt.ArrayDoubleClone(alpha);
            lndet = m_clt.ArrayDoubleClone(lndetmat);
            low95 = m_clt.ArrayDoubleClone(sderr);
            high95 = m_clt.ArrayDoubleClone(sderr);
        }

        #endregion

        #region funcoes auxiliares

        /// <summary>
        /// B = fliplr(A) returns A with columns flipped in the left-right direction, that is, 
        /// about a vertical axis.If A is a row vector, then fliplr(A) returns
        /// a vector of the same length with the order of its elements reversed. If A is 
        /// a column vector, then fliplr(A) simply returns A.
        /// </summary>
        /// <param name="a">Matrix A.</param>
        /// <returns>Matrix B.</returns>
        public double[,] Fliplr(double[,] a)
        {
            if (a.GetLength(1) == 1) return m_clt.ArrayDoubleClone(a);

            int ncols = a.GetLength(1);
            int nrows = a.GetLength(0);
            double[,] r = new double[nrows, ncols];

            for (int j = 0; j < ncols; j++)
            {
                for (int i = 0; i < nrows; i++)
                {
                    r[i, j] = a[i, ncols - 1 - j];
                }
            }

            return r;
        }

        public double[,] Range(double vmin, double delta, double vmax)
        {
            int n = Convert.ToInt32(1 + ((vmax - vmin) / delta));
            double[,] res = new double[n,1];
            for (int i = 0; i < n; i++)
            {
                res[i,0] = vmin + ((double)i) * delta;
            }
            return res;
        }

        public double[,] VandermondeMatrix(double[,] v)
        {
            int n = v.GetLength(0);
            double[,] A = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = Math.Pow(v[i,0], (double)(n - j - 1));
                }
            }
            return A;
        }

        #endregion

        #region aproximações via expansões com o traço da matriz

        private clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();

        public double log_det_trace(clsMatrizEsparsa W, double rho)
        {
            int num_termos = 50;
            double alpha = 0.0;
            clsMatrizEsparsa aux = W.Clone();
            double trace = 0.0;
            double d = 0.0;

            for (int k = 2; k <= num_termos; k++)
            {
                aux = fme.MatrizMult(aux, W);
                trace = trace_sparse_matrix(aux);
                d = - (Math.Pow(rho, (double)k) / ((double)k)) * trace;
                alpha += d;

                if (Math.Abs(d) < 1.0e-6) break;
            }

            return alpha;
        }

        public double trace_sparse_matrix(clsMatrizEsparsa w)
        {
            clsMatrizEsparsa a = fme.CompressColumn2TripletForm(w);
            double res = 0.0;
            for (int k = 0; k < a.i.GetLength(0); k++)
            {
                if (a.i[k] == a.p[k]) res += a.x[k];
            }
            return res;
        }

        #endregion
    }
}
