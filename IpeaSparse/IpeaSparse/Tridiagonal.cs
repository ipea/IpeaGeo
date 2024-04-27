using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IpeaMatrix;

namespace IpeaSparse
{
    public class Tridiagonal
    {
        public void tridag(ref Matrix a, ref Matrix b, ref Matrix c,ref Matrix r,ref Matrix u)
        {
	        int j,n=a.size();
	        double bet;
	        Matrix gam = new Matrix(n);
            if (b[0] == 0.0) throw new Exception("Error 1 in tridag");
	        u[0]=r[0]/(bet=b[0]);
	        for (j=1;j<n;j++) {
		        gam[j]=c[j-1]/bet;
		        bet=b[j]-a[j]*gam[j];
                if (bet == 0.0) throw new Exception("Error 2 in tridag");
		        u[j]=(r[j]-a[j]*u[j-1])/bet;
	        }
	        for (j=(n-2);j>=0;j--)
		        u[j] -= gam[j+1]*u[j+1];
        }

        public void cyclic(ref Matrix a, ref Matrix b, ref Matrix c, double alpha,double beta, ref Matrix r, ref Matrix x)
        {
            int i, n = a.size();
            double fact, gamma;
            if (n <= 2) throw new Exception("n too small in cyclic");
            Matrix bb = new Matrix(n);
            Matrix u = new Matrix(n);
            Matrix z = new Matrix(n);
            gamma = -b[0];
            bb[0] = b[0] - gamma;
            bb[n - 1] = b[n - 1] - alpha * beta / gamma;
            for (i = 1; i < n - 1; i++) bb[i] = b[i];
            tridag(ref a, ref bb, ref c, ref r, ref x);
            u[0] = gamma;
            u[n - 1] = alpha;
            for (i = 1; i < n - 1; i++) u[i] = 0.0;
            tridag(ref a, ref bb, ref c, ref u, ref z);
            fact = (x[0] + beta * x[n - 1] / gamma) /
                (1.0 + z[0] + beta * z[n - 1] / gamma);
            for (i = 0; i < n; i++) x[i] -= fact * z[i];
        }


    }
}
