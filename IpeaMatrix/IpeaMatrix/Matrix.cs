using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaMatrix
{
    public class Matrix
    {
        private int m_ncols = 0;
        private int m_nrows = 0;
        private double[,] m_dados = new double[0, 0];

        public double this[int i]
        {
            get { return this[i, 0]; }
            set { this[i, 0] = value; }
        }

        public Matrix Subrow(int i)
        {
            return new Matrix();
        }

        public double this[int i, int j]
        {
            get { return this.m_dados[i, j]; }
            set 
            {
                if (i <= m_nrows && j <= m_ncols)
                {
                    this.m_dados[i, j] = value;
                }
                if (i > m_nrows || j > m_ncols)
                {
                    int dim1 = Math.Max(i+1, m_nrows);
                    int dim2 = Math.Max(j+1, m_ncols);

                    double[,] tm = this.m_dados;

                    this.m_dados = new double[dim1, dim2];
                    for (int k = 0; k < m_nrows; k++)
                        for (int l = 0; l < m_ncols; l++) this.m_dados[k, l] = tm[k, l];

                    this.m_dados[i, j] = value;
                    this.m_nrows = dim1;
                    this.m_ncols = dim2;
                }
            }
        }

        public int rows
        {
            get { return this.m_nrows; }
        }

        public int cols
        {
            get { return this.m_ncols; }
        }

        public Matrix()
        {          
        }

        public int size()
        {
            return this.m_nrows;
        }

        public int ncols()
        {
            return this.m_ncols;
        }

        public int nrows()
        {
            return this.m_nrows;
        }

        public void resize(int n, int m)
        {
            this.m_nrows = n;
            this.m_ncols = m;
            this.m_dados = new double[n, m];
        }

        public Matrix(int n, int m)
        {
            m_ncols = m;
            m_nrows = n;
            m_dados = new double[n, m];
        }

        public Matrix(int n)
        {
            m_ncols = 1;
            m_nrows = n;
            m_dados = new double[n, 1];
        }

        public Matrix(double[,] m)
        {
            m_nrows = m.GetLength(0);
            m_ncols = m.GetLength(1);
            m_dados = m;
        }

        public int GetLength(int dim)
        {
            if (dim == 0) return this.m_nrows;
            else return this.m_ncols;
        }

        public Matrix Clone()
        {
            Matrix r = new Matrix(this.m_nrows, this.m_ncols);
            for (int i = 0; i < m_nrows; i++)
                for (int j = 0; j < m_ncols; j++)
                    r[i, j] = this.m_dados[i, j];
            return r;
        }

        /// <summary>
        /// Remove-se determinada linha da matriz.
        /// </summary>
        /// <param name="iRow">Posição da linha</param>
        /// <returns></returns>
        public Matrix RemoveRow(int iRow)
        {
            Matrix r = new Matrix(this.m_nrows-1, this.m_ncols);
            for (int j = 0; j < m_ncols; j++)
            {
                int iContador = 0;
                for (int i = 0; i < m_nrows; i++)
                {
                    if (i != iRow)
                    {
                        r[iContador, j] = this.m_dados[i, j];
                        iContador++;
                    }
                }
            }
            return r;
        }
        
        /// <summary>
        /// Remove-se determinada coluna da matriz.
        /// </summary>
        /// <param name="iCol">Posição da coluna</param>
        /// <returns></returns>
        public Matrix RemoveCol(int iCol)
        {
            Matrix r = new Matrix(this.m_nrows, this.m_ncols-1);
            for (int i = 0; i < m_nrows; i++)
            {
                int iContador = 0;
                for (int j = 0; j < m_ncols; j++)
                    if (j != iCol)
                    {
                        r[i, iContador] = this.m_dados[i, j];
                        iContador++;
                    }
            }
            return r;
        }
              
        /// <summary>
        /// Remove-se determinada linha e coluna da matriz.
        /// </summary>
        /// <param name="iRow">Posição da linha</param>
        /// <param name="iCol">Posição da coluna</param>
        /// <returns></returns>
        public Matrix Remove(int iRow,int iCol)
        {
            Matrix r = new Matrix(this.m_nrows - 1, this.m_ncols-1);
            int iContadorRow = 0;
            
            int i = 0, j = 0;
            for (i = 0; i < m_nrows; i++)
            {
                int iContadorCol = 0;
                for (j = 0; j < m_ncols; j++)
                {
                    if (j != iCol)
                    {
                        r[iContadorRow, iContadorCol] = this.m_dados[i, j];
                        iContadorCol++;
                    }
                }
                if (i != iRow) iContadorRow++;
            }
            return r;
        }
        
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.cols != b.cols) throw new Exception("Matrizes com dimensões diferentes");

            Matrix res = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++) res[i, j] = a[i, j] + b[i, j];
            return res;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.cols != b.cols) throw new Exception("Matrizes com dimensões diferentes");

            Matrix res = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++) res[i, j] = a[i, j] - b[i, j];
            return res;
        }

        public static Matrix operator *(double s, Matrix a)
        {
            Matrix res = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++) res[i, j] = s*a[i, j];
            return res;
        }

        public static Matrix operator *(Matrix a, double s)
        {
            Matrix res = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++) res[i, j] = s * a[i, j];
            return res;
        }

        public static Matrix operator +(double s, Matrix a)
        {
            Matrix res = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++) res[i, j] = s + a[i, j];
            return res;
        }
        
        public static Matrix operator +(Matrix a, double s)
        {
            Matrix res = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++) res[i, j] = s + a[i, j];
            return res;
        }

        public static Matrix operator -(Matrix a)
        {
            Matrix res = new Matrix(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++) res[i, j] = - a[i, j];
            return res;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.GetLength(1) != b.GetLength(0)) throw new Exception("Dimensões inválidas");

            Matrix r = new Matrix(a.GetLength(0), b.GetLength(1));
            for (int i = 0; i < r.GetLength(0); i++)
            {
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    for (int k = 0; k < a.GetLength(1); k++)
                    {
                        r[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return r;
        }
    }
}
