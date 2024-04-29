using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaMatrix
{
    internal class Cholesky
    {       
        private int n;
	    private Matrix el;

        /// <summary>
        /// Constructor. Given a positive-definite symmetric matrix a[0..n-1][0..n-1], construct
        /// and store its Cholesky decomposition, A = L x L' .
        /// </summary>
        /// <param name="a">Input matrix.</param>
        public Cholesky(ref Matrix a) 
        {
            n = a.GetLength(0);
            el = a.Clone();

	    int i,j,k;
	    double sum;
	    if (el.ncols() != n) throw new Exception("need square matrix");
	    for (i=0;i<n;i++) {
		    for (j=i;j<n;j++) {
			    for (sum=el[i,j],k=i-1;k>=0;k--) sum -= el[i,k]*el[j,k];
			    if (i == j) {
				    if (sum <= 0.0)
					    throw new Exception("Cholesky failed");
				    el[i,i]=Math.Sqrt(sum);
			    } else el[j,i]=sum/el[i,i];
		    }
	    }
	    for (i=0;i<n;i++) for (j=0;j<i;j++) el[j,i] = 0.0;
        }

        /// <summary>
        /// Solve the set of n linear equations Ax = b, where a is a positive-definite symmetric matrix
        /// whose Cholesky decomposition has been stored. b[0..n-1] is input as the right-hand side
        /// vector. The solution vector is returned in x[0..n-1].
        /// </summary>
        /// <param name="b">Right-hand-side coefficients.</param>
        /// <param name="x">Solution coefficients.</param>
        public void solve(ref Matrix b, ref Matrix x) 
        {
	    int i,k;
	    double sum;
	    if (b.size() != n || x.size() != n) throw  new Exception("bad lengths in Cholesky");
	    for (i=0;i<n;i++) {
		    for (sum=b[i],k=i-1;k>=0;k--) sum -= el[i,k]*x[k];
		    x[i]=sum/el[i,i];
	    }
	    for (i=n-1;i>=0;i--) {
		    for (sum=x[i],k=i+1;k<n;k++) sum -= el[k,i]*x[k];
		    x[i]=sum/el[i,i];
	    }		
        }

        public void elmult(ref Matrix y, ref Matrix b) 
        {
	    int i,j;
	    if (b.size() != n || y.size() != n) throw new Exception("bad lengths");
	    for (i=0;i<n;i++) {
		    b[i] = 0.0;
		    for (j=0;j<=i;j++) b[i] += el[i,j]*y[j];
	    }
        }

        public void elsolve(ref Matrix b, ref Matrix y) 
        {
	    int i,j;
	    double sum;
	    if (b.size() != n || y.size() != n) throw new Exception("bad lengths");
	    for (i=0;i<n;i++) {
		    for (sum=b[i],j=0; j<i; j++) sum -= el[i,j]*y[j];
		    y[i] = sum/el[i,i];
	    }
        }

	public void inverse(ref Matrix ainv) 
        {
	    int i,j,k;
	    double sum;
	    ainv.resize(n,n);
	    for (i=0;i<n;i++) for (j=0;j<=i;j++){
		    sum = (i==j? 1.0 : 0.0);
		    for (k=i-1;k>=j;k--) sum -= el[i,k]*ainv[j,k];
		    ainv[j,i]= sum/el[i,i];
	    }
	    for (i=n-1;i>=0;i--) for (j=0;j<=i;j++){
		    sum = (i<j? 0.0 : ainv[j,i]);
		    for (k=i+1;k<n;k++) sum -= el[k,i]*ainv[j,k];
		    ainv[i,j] = ainv[j,i] = sum/el[i,i];
	    }				
	}

	public double logdet() 
        {
	    double sum = 0.0;
	    for (int i=0; i<n; i++) sum += Math.Log(el[i,i]);
	    return 2.0*sum;
	}
    }
}
