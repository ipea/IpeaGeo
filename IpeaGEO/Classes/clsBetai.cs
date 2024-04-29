using System;
using System.Windows.Forms;

namespace IpeaGeo
{
	/// <summary>
	/// Summary description for Betai.
	/// </summary>
	[Obsolete("Use MathNet.Numerics instead.")]
        public class Betai
	{
		public Betai()
		{
		}

		//Retorna  a função BETA incompleta Bx(a,b)/B(a,b) onde a,b >0

		public double betai(double a, double b, double x)
		{
			Betacf be = new Betacf();
			Gammln g = new Gammln();
			double bt;

			if (x < 0.0 || x > 1.0) try{throw new Exception();}
									catch (Exception)
									{
										MessageBox.Show("Valor de x menor do que 0 ou maior do que 1. Rotina betai",
											"Método Inválido",MessageBoxButtons.OK, MessageBoxIcon.Error );
									}
			if (x == 0.0 || x == 1.0) bt=0.0;
			else
				bt=Math.Exp(g.gammln(a+b)-g.gammln(a)-g.gammln(b)+a*Math.Log(x)+b*Math.Log(1.0-x));
			if (x < (a+1.0)/(a+b+2.0))
				return bt*be.betacf(a,b,x)/a;
			else
				return 1.0-bt*be.betacf(b,a,1.0-x)/b;
		}
	}
}

