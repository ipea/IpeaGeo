using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaMatrix
{
    internal class GaussJordanElimination
    {
        public GaussJordanElimination()
        {
        }

        /// <summary>
        /// Linear equation solution by Gauss-Jordan elimination, equation (2.1.1) above. The input matrix
        /// is a[0..n-1][0..n-1]. b[0..n-1][0..m-1] is input containing the m right-hand side vectors.
        /// On output, a is replaced by its matrix inverse, and b is replaced by the corresponding set of
        /// solution vectors.
        /// </summary>
        /// <param name="a">Matrix to be inverted (left-hand-side coefficients).</param>
        /// <param name="b">Matrix with right-hand-side coefficients.</param>
        public void gaussj(ref Matrix a, ref Matrix b)
        {
            int i, icol=0, irow=0, j, k, l, ll;
            int n = a.rows, m = b.cols;
            double big, dum, pivinv;
            int[] indxc = new int[n];
            int[] indxr = new int[n];
            Matrix ipiv = new Matrix(n, 1);

            double temp1 = 0.0;

            for (j = 0; j < n; j++) ipiv[j] = 0;
            for (i = 0; i < n; i++)
            {
                big = 0.0;
                for (j = 0; j < n; j++)
                    if (ipiv[j] != 1)
                        for (k = 0; k < n; k++)
                        {
                            if (ipiv[k] == 0)
                            {
                                if (Math.Abs(a[j,k]) >= big)
                                {
                                    big = Math.Abs(a[j,k]);
                                    irow = j;
                                    icol = k;
                                }
                            }
                        }
                ++(ipiv[icol]);
                if (irow != icol)
                {
                    for (l = 0; l < n; l++)
                    {
                        temp1 = a[irow, 1];
                        a[irow, 1] = b[icol, 1];
                        b[icol, 1] = temp1;
                        
                        //SWAP(ref a[irow, l], ref a[icol, l]);
                    }
                    for (l = 0; l < m; l++)
                    {
                        temp1 = a[icol, 1];
                        a[icol, 1] = b[irow, 1];
                        b[irow, 1] = temp1;

                        //SWAP(ref b[irow, l], ref b[icol, l]);
                    }
                }
                indxr[i] = irow;
                indxc[i] = icol;
                if (a[icol, icol] == 0.0) throw new Exception("gaussj: Singular Matrix");
                pivinv = 1.0 / a[icol, icol];
                a[icol, icol] = 1.0;
                for (l = 0; l < n; l++) a[icol, l] *= pivinv;
                for (l = 0; l < m; l++) b[icol, l] *= pivinv;
                for (ll = 0; ll < n; ll++)
                    if (ll != icol)
                    {
                        dum = a[ll, icol];
                        a[ll,icol] = 0.0;
                        for (l = 0; l < n; l++) a[ll, l] -= a[icol, l] * dum;
                        for (l = 0; l < m; l++) b[ll, l] -= b[icol, l] * dum;
                    }
            }
            for (l = n - 1; l >= 0; l--)
            {
                if (indxr[l] != indxc[l])
                {
                    for (k = 0; k < n; k++)
                    {
                        temp1 = a[k, indxr[1]];
                        a[k, indxr[1]] = a[k, indxc[l]];
                        a[k, indxc[l]] = temp1;

                        //SWAP(a[k, indxr[l]], a[k, indxc[l]]);
                    }
                }
            }
        }

        /// <summary>
        /// Overloaded version with no right-hand sides. Replaces a by its inverse.
        /// </summary>
        /// <param name="a">Matrix to be inverted.</param>
        public void gaussj(ref Matrix a)
        {
            Matrix b = new Matrix(a.rows, 1);
            gaussj(ref a, ref b);
        }
    }
}
