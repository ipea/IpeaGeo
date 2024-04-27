using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsTridag
    {
        public clsTridag()
        {
        }

        public void tridag(ref double[,] a, ref double[,] b, ref double[,] c, ref double[,] r, ref double[,] u)
        {
	        int j,n=a.GetLength(0);
	        double bet;
	        double[,] gam = new double[n,1];
            if (b[0, 0] == 0.0) throw new Exception("Error 1 in tridag");
	        u[0,0]=r[0,0]/(bet=b[0,0]);
	        for (j=1;j<n;j++) {
		        gam[j,0]=c[j-1,0]/bet;
		        bet=b[j,0]-a[j,0]*gam[j,0];
		        if (bet == 0.0) throw new Exception ("Error 2 in tridag");
		        u[j,0]=(r[j,0]-a[j,0]*u[j-1,0])/bet;
	        }
	        for (j=(n-2);j>=0;j--)
		        u[j,0] -= gam[j+1,0]*u[j+1,0];
        }

        public void cyclic(ref double[,] a, ref double[,] b, ref double[,] c, double alpha,
	        double beta, ref double[,] r, ref double[,] x)
        {
	        int i,n=a.GetLength(0);
	        double fact,gamma;
	        if (n <= 2) throw new Exception("n too small in cyclic");
	        double[,] bb = new double[n,1];
            double[,] u = new double[n,1];
            double[,] z = new double[n,1];
	        gamma = -b[0,0];
            bb[0, 0] = b[0, 0] - gamma;
            bb[n - 1, 0] = b[n - 1, 0] - alpha * beta / gamma;
            for (i = 1; i < n - 1; i++) bb[i, 0] = b[i, 0];
	        tridag(ref a, ref bb, ref c, ref r, ref x);
            u[0, 0] = gamma;
            u[n - 1, 0] = alpha;
            for (i = 1; i < n - 1; i++) u[i, 0] = 0.0;
	        tridag(ref a, ref bb, ref c, ref u, ref z);
            fact = (x[0, 0] + beta * x[n - 1, 0] / gamma) /
                (1.0 + z[0, 0] + beta * z[n - 1, 0] / gamma);
            for (i = 0; i < n; i++) x[i, 0] -= fact * z[i, 0];
        }
    }
}
