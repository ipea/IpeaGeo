using System;

namespace IpeaGeo
{
	/// <summary>
	/// Summary description for Gammln.
	/// </summary>
	[Obsolete("Use MathNet.Numerics instead.")]
    public class Gammln
	{
		public Gammln()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		//Retorna o valor do ln(gama(x)) onde x > 0

		public double gammln(double xx)
		{
			// Internal arithmetic will be done in double precision, a nicety that you can omit if five-figure
			// accuracy is good enough.
			double x,y,tmp,ser;
			double[]  cof = new Double[6];
			cof[0] = 76.18009172947146;
			cof[1] = -86.50532032941677;
			cof[2] = 24.01409824083091;
			cof[3] = -1.231739572450155; 
			cof[4] = 0.1208650973866179e-2;
			cof[5] = -0.5395239384953e-5;
			int j;
			y=x=xx;
			tmp=x+5.5;
			tmp -= (x+0.5)*Math.Log(tmp);
			ser=1.000000000190015;
			for (j=0;j<=5;j++) ser += cof[j]/++y;
			return -tmp+Math.Log(2.5066282746310005*ser/x);
		}
	}
}

