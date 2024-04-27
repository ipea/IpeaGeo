using System;
using System.Windows.Forms;

namespace IpeaGeo
{
    /// <summary>
    /// Computes all eigenvalues and eigenvectors of a real symmetric matrix a, 
    /// which is of size n by n, stored in a physical np by np array. On output, 
    /// elements of a above the diagonal are destroyed. d returns the eigenvalues 
    /// of a in its first n elements. v is a matrix with the same logical and 
    /// physical dimensions as a, whose columns contain, on output, the normalized
    /// eigenvectors of a. nrot returns the number of Jacobi rotations 
    /// that were required.
    /// </summary>
    [Obsolete("Use MathNet.Numerics.Distributions instead")]
    public class Jacobi
    {
        private double[] d;
        private double[,] v;
        private int nrot = 0;
        public Jacobi()
        {
        }
        public double[] Eigenvalues
        {
            get { return d; }
        }
        public double[,] Eigenvectors
        {
            get { return v; }
        }
        private double tresh, theta, tau, t, sm, s, h, g, c;
        private void Rotate(double[,] a, int i, int j, int k, int l)
        {
            g = a[i, j];
            h = a[k, l];
            a[i, j] = g - s * (h + g * tau);
            a[k, l] = h + s * (g - h * tau);
        }
        public void jacobi(double[,] a) //only input parameters
        {
            int n = Convert.ToInt32(Math.Sqrt(a.Length));
            d = new double[n];
            v = new double[n, n];
            int i, j, ip, iq;
            double[] b = new double[n];
            double[] z = new double[n];
            for (ip = 0; ip < n; ip++) //Initialize to the identity matrix.
            {
                for (iq = 0; iq < n; iq++) v[ip, iq] = 0.0;
                v[ip, ip] = 1.0;
            }
            for (ip = 0; ip < n; ip++) // Initialize b and d to the diagonal of a.
            {
                b[ip] = d[ip] = a[ip, ip];
                z[ip] = 0.0;   // This vector will accumulate terms of the form tapq.
            }
            nrot = 0;
            try
            {
                for (i = 1; i <= 50; i++)
                {
                    sm = 0.0;
                    for (ip = 0; ip < n - 1; ip++) // Sum off-diagonal elements.
                    {
                        for (iq = ip + 1; iq < n; iq++) sm += Math.Abs(a[ip, iq]);
                    }
                    if ((double)sm == 0.0) return; // The normal return 
                    if (i < 4) tresh = 0.2 * sm / (n * n);   //...on the first three sweeps.
                    else tresh = 0.0;            //...there after.
                    for (ip = 0; ip < n - 1; ip++)
                    {
                        for (iq = ip + 1; iq < n; iq++)
                        {
                            g = 100.0 * Math.Abs(a[ip, iq]);
                            // After four sweeps, skip the rotation 
                            // if the off-diagonal element is small.
                            if (i > 4 && (float)(Math.Abs(d[ip]) + g) == (float)Math.Abs(d[ip])
                                && (float)(Math.Abs(d[iq]) + g) == (float)Math.Abs(d[iq]))
                                a[ip, iq] = 0.0;
                            else if (Math.Abs(a[ip, iq]) > tresh)
                            {
                                h = d[iq] - d[ip];
                                if ((float)(Math.Abs(h) + g) == (float)Math.Abs(h))
                                    t = (a[ip, iq]) / h;   // t = 1/(2\theta)
                                else
                                {
                                    theta = 0.5 * h / (a[ip, iq]);   // Equation (11.1.10).
                                    t = 1.0 / (Math.Abs(theta) + Math.Sqrt(1.0 + theta * theta));
                                    if (theta < 0.0) t = -t;
                                }
                                c = 1.0 / Math.Sqrt(1.0 + t * t);
                                s = t * c;
                                tau = s / (1.0 + c);
                                h = t * a[ip, iq];
                                z[ip] -= h;
                                z[iq] += h;
                                d[ip] -= h;
                                d[iq] += h;
                                a[ip, iq] = 0.0;
                                //Case of rotations 0 <= j < p.
                                for (j = 0; j < ip; j++) Rotate(a, j, ip, j, iq);
                                //Case of rotations p < j < q.
                                for (j = ip + 1; j < iq; j++) Rotate(a, ip, j, j, iq);
                                //Case of rotations q < j < n.
                                for (j = iq + 1; j < n; j++) Rotate(a, ip, j, iq, j);
                                for (j = 0; j < n; j++) Rotate(v, j, ip, j, iq);
                                ++nrot;
                            }   // end of else if
                        }  // end of for
                    } // end of for
                    for (ip = 0; ip < n; ip++)
                    {
                        b[ip] += z[ip];
                        d[ip] = b[ip];       // Update d with the sum of tapq,
                        z[ip] = 0.0;         // and reinitialize z.
                    }
                }  //end of for
                throw new GeneralException();
            } // end of try 
            catch (GeneralException)
            {
                MessageBox.Show("O método de Jacobinão funciona para encontrar os autovetores e autovalores. É necessário usar outros métodos",
                            "Método Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error,MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            }
        } // end of jacobi
    }

}