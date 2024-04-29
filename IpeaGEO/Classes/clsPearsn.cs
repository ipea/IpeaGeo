using System;

using MathNet.Numerics;

namespace IpeaGeo
{
	/// <summary>
	/// Summary description for Pearsn.
	/// </summary>
        [Obsolete("Use MathNet.Numerics.StatisticalFunctions instead.")]
	public class Pearsn
	{
		public Pearsn()
		{
		}
		private double r, prob, z,cov;

		public double Covariancia
		{
			get{return cov;}
		}
		public double R
		{
			get{return r;}
		}
		public double Probability
		{
			get{return prob;}
		}
		public double Z
		{
			get{return z;}
		}		
		private double TINY = 1.0e-20;
		public void pearsn(double[] x, double[] y, ulong n) 
		{
			Betai obj1 = new Betai();
			Erfcc obj2 = new Erfcc();
			ulong j;
			double yt,xt,t,df,erro=0.0,total=0.0;
			double syy=0.0,sxy=0.0,sxx=0.0,ay=0.0,ax=0.0;

			for (j=0;j<n;j++) 
			{
				if(double.IsNaN(x[j])!= true && double.IsNaN(y[j])!= true )
				{
					ax += x[j];
					ay += y[j];
				}
				else
				{
					erro+=1;
				}
			}
			
			total=(double)n - erro;

			ax /= total;
			ay /= total;

			for (j=0;j<n;j++) 
			{
				if(double.IsNaN(x[j])!= true && double.IsNaN(y[j])!= true )
				{
					xt=x[j]-ax;
					yt=y[j]-ay;
					sxx += xt*xt;
					syy += yt*yt;
					sxy += xt*yt;
				}
			}
			
			r=sxy/(Math.Sqrt(sxx*syy)+TINY);
			cov=(sxy)/(total-1);
			z=0.5*Math.Log((1.0+r+TINY)/(1.0-r+TINY));
			df=total-2;
			t=r*Math.Sqrt(df/((1.0-r+TINY)*(1.0+r+TINY)));

			if(total > 500)
			{
				prob=obj2.erfcc(Math.Abs((z*Math.Sqrt(total-1.0))/1.4142136));
			}
			else
			{
				prob=obj1.betai(0.5*df,0.5,df/(df+t*t));
			}			
		}
	}
}


