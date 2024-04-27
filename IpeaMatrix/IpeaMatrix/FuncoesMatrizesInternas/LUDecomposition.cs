using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaMatrix
{
    internal class LUDecomposition
    {
	    private int n;
	    private Matrix lu;
	    private int[] indx;
	    private double d;
        private Matrix aref;
	    
        public LUDecomposition(ref Matrix a)
        {
            n = a.rows;
            lu = a.Clone();
            indx = new int[n];
            aref = a.Clone();

	        double TINY=1.0e-40;
	        int i,imax=0,j,k;
	        double big,temp;
	        double[] vv = new double[n]; 
	        d=1.0;
	        for (i=0;i<n;i++) {
		        big=0.0;
		        for (j=0;j<n;j++)
			        if ((temp=Math.Abs(lu[i,j])) > big) big=temp;
		        if (big == 0.0) throw new Exception("Singular matrix in LU decomposition");
		        vv[i]=1.0/big;
	        }
	        for (k=0;k<n;k++) {
		        big=0.0;
		        for (i=k;i<n;i++) {
			        temp=vv[i]*Math.Abs(lu[i,k]);
			        if (temp > big) {
				        big=temp;
				        imax=i;
			        }
		        }
		        if (k != imax) {
			        for (j=0;j<n;j++) {
				        temp=lu[imax,j];
				        lu[imax,j]=lu[k,j];
				        lu[k,j]=temp;
			        }
			        d = -d;
			        vv[imax]=vv[k];
		        }
		        indx[k]=imax;
		        if (lu[k,k] == 0.0) lu[k,k]=TINY;
		        for (i=k+1;i<n;i++) {
			        temp=lu[i,k] /= lu[k,k];
			        for (j=k+1;j<n;j++)
				        lu[i,j] -= temp*lu[k,j];
		        }
	        }
        }

        public void solve(ref Matrix b, ref Matrix x)
        {
	        int i,ii=0,ip,j;
	        double sum;
	        if (b.size() != n || x.size() != n)
		        throw new Exception("LUdcmp::solve bad sizes");
	        for (i=0;i<n;i++) x[i] = b[i];
	        for (i=0;i<n;i++) {
		        ip=indx[i];
		        sum=x[ip];
		        x[ip]=x[i];
		        if (ii != 0)
			        for (j=ii-1;j<i;j++) sum -= lu[i,j]*x[j];
		        else if (sum != 0.0)
			        ii=i+1;
		        x[i]=sum;
	        }
	        for (i=n-1;i>=0;i--) {
		        sum=x[i];
		        for (j=i+1;j<n;j++) sum -= lu[i,j]*x[j];
		        x[i]=sum/lu[i,i];
	        }
        }

        private void solvemat(ref Matrix b, ref Matrix x)
        {
	        int i,j,m=b.ncols();
	        if (b.nrows() != n || x.nrows() != n || b.ncols() != x.ncols())
		        throw new Exception("LUdcmp::solve bad sizes");
	        Matrix xx = new Matrix(n);
	        for (j=0;j<m;j++) {
		        for (i=0;i<n;i++) xx[i] = b[i,j];
		        solve(ref xx, ref xx);
		        for (i=0;i<n;i++) x[i,j] = xx[i];
	        }
        }

        public void inverse(ref Matrix ainv)
        {
	        int i,j;
	        ainv.resize(n,n);
	        for (i=0;i<n;i++) {
		        for (j=0;j<n;j++) ainv[i,j] = 0.0;
		        ainv[i,i] = 1.0;
	        }
	        solvemat(ref ainv, ref ainv);
        }

        public double det()
        {
	        double dd = d;
	        for (int i=0;i<n;i++) dd *= lu[i,i];
	        return dd;
        }
    }
}
