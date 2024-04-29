using System;
using System.Windows.Forms;

namespace IpeaGeo
{
	/// <summary>
	/// Summary description for Betacf.
	/// </summary>
	[Obsolete("Use MathNet.Numerics instead.")]
        public class Betacf
	{
		int MAXIT = 10000;
		double EPS = 3.0e-7;
		double FPMIN = 1.0e-30;
		public Betacf()
		{
		}

		//Algoritimo melhor usado para a fração da função beta incompleta
		public double betacf(double a, double b, double x)
		{
			int m,m2;
			double aa,c,d,del,h,qab,qam,qap;

			qab=a+b;
			qap=a+1.0;
			qam=a-1.0;
			c=1.0;
			d=1.0-qab*x/qap;
			if (Math.Abs(d) < FPMIN) d=FPMIN;
			d=1.0/d;
			h=d;
			for (m=1;m<=MAXIT;m++) 
			{
				m2=2*m;
				aa=m*(b-m)*x/((qam+m2)*(a+m2));
				d=1.0+aa*d;
				if (Math.Abs(d) < FPMIN) d=FPMIN;
				c=1.0+aa/c;
				if (Math.Abs(c) < FPMIN) c=FPMIN;
				d=1.0/d;
				h *= d*c;
				aa = -(a+m)*(qab+m)*x/((a+m2)*(qap+m2));
				d=1.0+aa*d;
				if (Math.Abs(d) < FPMIN) d=FPMIN;
				c=1.0+aa/c;
				if (Math.Abs(c) < FPMIN) c=FPMIN;
				d=1.0/d;
				del=d*c;
				h *= del;
				if (Math.Abs(del-1.0) < EPS) break;
			}
			if (m > MAXIT) try{throw new Exception();}
						   catch (Exception)
						   {
							   MessageBox.Show("a ou b é muito grande, ou MAXIT é muito pequeno na classe betacf",
								   "Método Inválido",MessageBoxButtons.OK, MessageBoxIcon.Error );
						   }
			return h;
		}
	}
}

