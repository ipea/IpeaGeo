using System;

namespace IpeaGeo
{
	/// <summary>
	/// Summary description for Erfcc.
	/// </summary>
	[Obsolete("Use of MathNet.Numerics.")]
    public class Erfcc
	{
		public Erfcc()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		//Retorna a função erro complementar com um erro fracionário de 1.2 *10^-7
		
		public double erfcc(double x)
		{
			double t,z,ans;

			z=Math.Abs(x);
			t=1.0/(1.0+0.5*z);
			ans=t*Math.Exp(-z*z-1.26551223+t*(1.00002368+t*(0.37409196+t*(0.09678418+
				t*(-0.18628806+t*(0.27886807+t*(-1.13520398+t*(1.48851587+
				t*(-0.82215223+t*0.17087277)))))))));
			return x >= 0.0 ? ans : 2.0-ans;
		}
	
	}
}
