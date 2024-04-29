using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaMatrix
{
    public static class Mfunc
    {
        #region Linear algebra functions

        /// <summary>
        /// Invert the input matrix by LU decomposition.
        /// </summary>
        /// <param name="a">Input matrix.</param>
        /// <returns>Inverse matrix.</returns>
        public static Matrix Inv(Matrix a)
        {
            LUDecomposition lu = new LUDecomposition(ref a);
            Matrix res = a.Clone();
            lu.inverse(ref res);
            return res;
        }

        public static Matrix GaussJordanInv(Matrix a)
        {
            GaussJordanElimination gj = new GaussJordanElimination();
            Matrix r = a.Clone();
            gj.gaussj(ref r);
            return r;
        }

        public static double Det(Matrix a)
        {
            LUDecomposition lu = new LUDecomposition(ref a);
            return lu.det();
        }

        public static Matrix Transp(Matrix a)
        {
            Matrix res = new Matrix(a.GetLength(1), a.GetLength(0));
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[j, i] = a[i, j];
            return res;
        }

        public static Matrix Pow(Matrix a,double p)
        {
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    a[i, j] = Math.Pow(a[i, j], p);
            return a;
        }

        public static Matrix Ident(int m)
        {
            Matrix r = new Matrix(m, m);
            for (int i = 0; i < m; i++) r[i, i] = 1.0;
            return r;
        }

        public static Matrix LinearSystem(Matrix A, Matrix b)
        {
            LUDecomposition lu = new LUDecomposition(ref A);
            Matrix x = new Matrix();
            lu.solve(ref b, ref x);
            return x;
        }

        #endregion 

        #region Statistical functions

        public static double Mean(Matrix a)
        {
            int n = a.GetLength(0) * a.GetLength(1);
            double soma = 0.0;
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    soma += a[i, j];
            return soma / (double)n;
        }

        public static double Norm(Matrix a)
        {
            double soma = 0.0;
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    soma += a[i, j]*a[i,j];
            return Math.Sqrt(soma);
        }

        #endregion
    }
}
