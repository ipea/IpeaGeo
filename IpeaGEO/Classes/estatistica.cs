using System;
using System.Windows.Forms;
using System.Threading;

// Simple threading scenario:  Start a static method running
// on a second thread.



namespace IpeaGEO
{
	/// <summary>
	/// Summary description for estatistica.
	/// </summary>
#warning Verificar a necessidade de manter esta classe.
	public class estatistica
	{
		public estatistica()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		private double ave, var;
		public double Average
		{
			get{return ave;}
		}
		public double Variance
		{
			get{return var;}
		}

		public void avevar(double[] data, ulong n)
		{
			ulong j;
			ulong total;
			double s,ep;
			ulong error=0;
			

			for (ave=0.0,j=0;j<n;j++) 
			{
				
				if(double.IsNaN(data[j])== true)
				{
					error+=1;
					ave+=0;
					
				}
				else 
				{
					ave += data[j];
				}
				
			}
			ave /= (double)n-error;

			var=ep=0.0;
			error=0;

			for (j=0;j<n;j++) 
			{
				if(double.IsNaN(data[j])== true)
				{
					error+=1;
				}
				else
				{
					s=data[j]-ave;
					ep += s;
					var += s*s;
				}
			}
			total=n-error;

			var=(var-ep*ep/(double)total)/(double)(total-1);
		} 


		private double ave2, adev, sdev, var2 ,skew,curt,maximo,minimo,r;

		public double Average2
		{
			get{return ave2;}
		}
		public double AverageDeviation
		{
			get{return adev;}
		}
		public double StandardDeviation
		{
			get{return sdev;}
		} 
		public double Variance2
		{
			get{return var2;}
		}
		public double Skewness
		{
			get{return skew;}
		}
		public double Kurtosis
		{
			get{return curt;}
		}

		
		public void moment(double[] data, int n)
		{
	
			int j;
			ulong error=0;
			ulong total;
			double ep=0.0,s,s2,p,C_4,C4,tudo;
			
			

			if (n <= 1) try{throw new Exception();}
						catch (Exception)
						{
							MessageBox.Show("n deve ser ao menos 2 para o momento",
								"Método Invalido",MessageBoxButtons.OK, MessageBoxIcon.Error );
						}
			s=0.0;
			s2=0.0;
			C_4=0.0;
			C4=0.0;
			for (j=0;j<n;j++)
			{

				if(double.IsNaN(data[j])== true)
				{
					error+=1;
					s+=0;
					
				}
				else
				{
					
					s += data[j];
					s2+=Math.Pow(data[j],2);
				
				}
					
			}
		

			total=(ulong)n-error;
			tudo=(double)total;
			ave2=s/(total);
			adev=var2=skew=curt=0.0;
			for (j=0;j<n;j++) 
			{
				if(double.IsNaN(data[j])!= true)
				{
					adev += Math.Abs(s=data[j]-ave2);
					var2 += (p=s*s);
					skew += (p *= s);
					curt += (p *= s);
					
				}
				else
				{
					adev +=0.0;
					var2 +=0.0;
					skew +=0.0;
					curt +=0.0;
				}
			}
			adev /= total;
			var2=(var2-ep*ep/total)/(total-1);
			sdev=Math.Sqrt(var2);
			if (var2 != 0.0) 
			{
				skew *= (total)/((total-1)*(total-2)*var2*sdev);
				C4=(tudo*(tudo+1))/((tudo-1)*(tudo-2)*(tudo-3)*var2*var2);
				C_4=(Math.Pow((tudo-1),2)/((tudo-2)*(tudo-3)));
				curt=(C4*curt)-3*C_4;
			} 
			else try{throw new Exception();}
				 catch (Exception)
				 {
					 MessageBox.Show("Nenhuma Assimetria/Curtose quando variancia = 0 (no momento)",
						 "Método Invalido",MessageBoxButtons.OK, MessageBoxIcon.Error );
				 }
		}







		public void Minimo_(double[] data)
		{
			

			int error=0;
			
			Array.Sort(data);
			for(int i=0;i<data.Length;i++)
			{
				if(double.IsNaN(data[i])== false)
				{
					minimo=data[i];
					break;
				}
				else
				{
					error++;
				}

			}
		
		} 

		public void Maximo_(double[] data)
		{
			int error=0;
			Array.Sort(data);
			for(int i=data.Length-1; i>-1;i--)
			{
			
				if(double.IsNaN(data[i])== false)
				{
					maximo=data[i];
					break;
				}
				else
				{
					error++;
				}
			}
		
		} 
		public double Maximo
		{
			get{return maximo;}
		}
		public double Minimo
		{
			get{return minimo;}
		}

		private double TINY = 1.0e-20;
		public void pearsn(double[] x, double[] y, ulong n) 
		{

			ulong j;
			double yt,xt,erro=0.0,total=0.0;
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

		}
		public double R
		{
			get{return r;}
		}
		
	}
	

}



