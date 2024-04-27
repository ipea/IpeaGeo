using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Collections;

namespace IpeaGEO
{
    #region Enumerações

    public enum TipoEstatisticaAmostra : int
    {
        N = 1,
        Soma = 2,
        Media = 3,
        VarianciaPopulacional = 4,
        DesvioPadraoPopulacional = 5,
        Minimo = 6,
        Maximo = 7
    };

    #endregion

    #region Funções matriciais

    class clsUtilTools
    {
        public clsUtilTools()
        {
        }
        
        #region Alterações Pedro

        /// <summary>Média das colunas de uma matriz.</summary>
        public double[] VetorMedias(double[,] a)
        {
            double[] r = new double[a.GetLength(1)];
            for (int j = 0; j < a.GetLength(1); j++)
            {
                r[j] = 0.0;
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    r[j] += a[i, j];
                }
                r[j] = r[j] / Convert.ToDouble(a.GetLength(0));
            }
            return r;
        }

        private bool novaClasse(int iClasse, ArrayList arLista)
        {
            for (int i = 0; i < arLista.Count; i++)
            {
                if (Convert.ToInt32(arLista[i]) == iClasse) return (false);
            }
            return (true);
        }

        private int idClasse(int iClasse, ArrayList arLista)
        {
            for (int i = 0; i < arLista.Count; i++)
            {
                if (Convert.ToInt32(arLista[i]) == iClasse) return (i);
            }
            return (-1);
        }

        public int[] ConverteClusterTree(double[,] mClusterTree, int iColuna)
        {

            ArrayList arLista = new ArrayList();
            int[] iSaida = new int[mClusterTree.GetLength(0)];
            for (int i = 0; i < iSaida.Length; i++)
            {
                iSaida[i] = (int)mClusterTree[i, iColuna];
                if (novaClasse(iSaida[i], arLista) == true)
                {
                    arLista.Add(iSaida[i]);
                }
            }
            //FileInfo t = new FileInfo(@"F:\IpeaGEO.txt");
            //StreamWriter Tex = t.CreateText();
            for (int i = 0; i < iSaida.Length; i++)
            {
                iSaida[i] = idClasse(iSaida[i], arLista);
                //Tex.WriteLine(i.ToString() + "\t" + iSaida[i].ToString());
            }
            //Tex.Close();
            return (iSaida);
        }

        /// <summary>
        /// Função para decomposição LU
        /// </summary>
        /// <param name="a"></param>
        public void DecomposicaoLU(ref double[,] a)
        {
            int n = 0;
            int[] indx = new int[n];
            double d;

            n = a.GetLength(0);
            double[,] lu = (double[,])a.Clone();
            double[,] aref = (double[,])a.Clone();

            double TINY = 1.0e-40;
            int i, imax = 0, j, k;
            double big, temp;
            double[] vv = new double[n];
            d = 1.0;
            for (i = 0; i < n; i++)
            {
                big = 0.0;
                for (j = 0; j < n; j++)
                    if ((temp = Math.Abs(lu[i, j])) > big) big = temp;
                if (big == 0.0) throw new Exception("Singular matrix in LU decomposition");
                vv[i] = 1.0 / big;
            }
            for (k = 0; k < n; k++)
            {
                big = 0.0;
                for (i = k; i < n; i++)
                {
                    temp = vv[i] * Math.Abs(lu[i, k]);
                    if (temp > big)
                    {
                        big = temp;
                        imax = i;
                    }
                }
                if (k != imax)
                {
                    for (j = 0; j < n; j++)
                    {
                        temp = lu[imax, j];
                        lu[imax, j] = lu[k, j];
                        lu[k, j] = temp;
                    }
                    d = -d;
                    vv[imax] = vv[k];
                }
                indx[k] = imax;
                if (lu[k, k] == 0.0) lu[k, k] = TINY;
                for (i = k + 1; i < n; i++)
                {
                    temp = lu[i, k] /= lu[k, k];
                    for (j = k + 1; j < n; j++)
                        lu[i, j] -= temp * lu[k, j];
                }
            }
        }

        #endregion

        /// <summary>
        /// Retorna o log-determinante de uma matriz double[,] simétrica.
        /// </summary>
        /// <param name="a">Matriz simétrica.</param>
        /// <returns>Log-determinate.</returns>
        public double LogDet(double[,] a)
        {
            Cholesky chol = new Cholesky(ref a);
            return chol.logdet();
        }

        /// <summary>Returns the sum of elements of the rows of a matrix</summary>
        public double[,] Sumr(double[,] a)
        {
            double[,] r = new double[a.GetLength(0), 1];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                r[i, 0] = 0.0;
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    r[i, 0] += a[i, j];
                }
            }
            return r;
        }

		/// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="a">Input matrix</param>
		/// <param name="i0">Start row index</param>
		/// <param name="i1">End row index</param>
		/// <param name="j0">Start column index</param>
		/// <param name="j1">End column index</param>
		public double[,] SubMatriz(double[,] a, int i0, int i1, int j0, int j1)
		{
			if ((i0 > i1) || (j0 > j1) ||  (i0 < 0) || (i0 >= a.GetLength(0) ||  (i1 < 0) || (i1 >= a.GetLength(0)) ||  
				(j0 < 0) || (j0 >= a.GetLength(1)) ||  (j1 < 0) || (j1 >= a.GetLength(1))))
			{ 
				throw new ArgumentException(); 
			}			
			double[,] X = new double[i1-i0+1,j1-j0+1];
			for (int i = i0; i <= i1; i++)
			{
				for (int j = j0; j <= j1; j++)
				{
					X[i - i0,j - j0] = a[i,j];
				}
			}
			return X;
        }

        /// <summary>Returns the maximum of elements of the vector</summary>
        public double Max(double[] a)
        {
            double m = a[0];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                if (a[i] > m) m = a[i];
            }
            return m;
        }

        /// <summary>Returns the minimum of elements of the vector</summary>
        public double Min(double[] a)
        {
            double m = a[0];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                if (a[i] < m) m = a[i];
            }
            return m;
        }

        /// <summary>Returns the maximum of elements of the matrix</summary>
        public double Max(double[,] a)
        {
            double m = a[0, 0];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] > m) { m = a[i, j]; }
                }
            }
            return m;
        }
        
        /// <summary>Returns the minimum of elements of the matrix</summary>
        public double Min(double[,] a)
        {
            double m = a[0, 0];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] < m) { m = a[i, j]; }
                }
            }
            return m;
        }

        public double[,] Ones(int n, int m)
        {
            double[,] res = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    res[i, j] = 1.0;

            return res;
        }

        public double[,] Zeros(int n, int m)
        {
            double[,] res = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    res[i, j] = 0.0;

            return res;
        }

        public double[,] unit(int m)
        {
            double[,] res = new double[m, m];
            for (int i = 0; i < m; i++) res[i, i] = 1.0;

            return res;
        }

        public int columns(double[,] a) { return a.GetLength(1); }
        public int rows(double[,] a) { return a.GetLength(0); }

        public object[,] NullObjectArray(int rows, int cols)
        {
            object[,] res = new object[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    res[i, j] = null;

            return res;
        }

        /// <summary>
        /// Ainda não está funcionando.
        /// </summary>
        /// <param name="tabela1"></param>
        /// <param name="coluna1"></param>
        /// <param name="tabela2"></param>
        /// <param name="coluna2"></param>
        /// <returns></returns>
        public object[,] MergeBy(object[,] tabela1, int coluna1, object[,] tabela2, int coluna2)
        {
            clsSort srt = new clsSort();

            object[,] stabela1 = new object[0, 0];
            object[,] stabela2 = new object[0, 0];

            this.SortByColumn(ref stabela1, tabela1, coluna1);
            this.SortByColumn(ref stabela2, tabela2, coluna2);

            object[,] all_cats = this.Concatev(this.SubColumnArrayDouble(stabela1, coluna1), this.SubColumnArrayDouble(stabela2, coluna2));

            object[,] main_cats = new object[0, 0];

            this.FrequencyTable(ref main_cats, all_cats);
            main_cats = this.SubColumnArrayDouble(main_cats, 0);

            object[,] aux1 = this.NullObjectArray(main_cats.GetLength(0), stabela1.GetLength(1));
            object[,] aux2 = this.NullObjectArray(main_cats.GetLength(0), stabela2.GetLength(1));

            for (int i = 0; i < main_cats.GetLength(0); i++)
            {

            }

            return new object[0, 0];
        }

        public double[,] GeraEstatisticasPorSubgrupo(ref object[,] tabela_frequencias, 
            double[,] dados, object[,] categorias, TipoEstatisticaAmostra tipo_estatistica)
        {
            this.FrequencyTable(ref tabela_frequencias, categorias);
            double[,] res = new double[tabela_frequencias.GetLength(0), dados.GetLength(1)];

            int[] freq_acumulada = new int[tabela_frequencias.GetLength(0)+1];
            int[] freq_absoluta = new int[tabela_frequencias.GetLength(0)];

            freq_acumulada[0] = 0;
            freq_absoluta[0] = Convert.ToInt32(tabela_frequencias[0, 1]);
            for (int i = 0; i < freq_absoluta.GetLength(0); i++)
            {
                freq_acumulada[i+1] = freq_acumulada[i] + Convert.ToInt32(tabela_frequencias[i, 1]);
                freq_absoluta[i] = Convert.ToInt32(tabela_frequencias[i, 1]);
            }

            object[,] sortedx = new object[0, 0];
            double[,] medias;

            switch (tipo_estatistica)
            {
                case TipoEstatisticaAmostra.Minimo:
                    this.SortByColumn(ref sortedx, this.Concateh(this.ConvertArray(dados), categorias), dados.GetLength(1));
                    for (int k = 0; k < res.GetLength(1); k++)
                    {
                        for (int ind_categoria = 0; ind_categoria < freq_absoluta.GetLength(0); ind_categoria++)
                        {
                            res[ind_categoria, k] = Convert.ToDouble(sortedx[freq_acumulada[ind_categoria], k]);
                            for (int i = freq_acumulada[ind_categoria]+1; i < freq_acumulada[ind_categoria + 1]; i++)
                            {
                                if (res[ind_categoria, k] > Convert.ToDouble(sortedx[i, k]))
                                {
                                    res[ind_categoria, k] = Convert.ToDouble(sortedx[i, k]);
                                }
                            }
                        }
                    }
                    break;

                case TipoEstatisticaAmostra.Maximo:
                    this.SortByColumn(ref sortedx, this.Concateh(this.ConvertArray(dados), categorias), dados.GetLength(1));
                    for (int k = 0; k < res.GetLength(1); k++)
                    {
                        for (int ind_categoria = 0; ind_categoria < freq_absoluta.GetLength(0); ind_categoria++)
                        {
                            res[ind_categoria, k] = Convert.ToDouble(sortedx[freq_acumulada[ind_categoria], k]);
                            for (int i = freq_acumulada[ind_categoria] + 1; i < freq_acumulada[ind_categoria + 1]; i++)
                            {
                                if (res[ind_categoria, k] < Convert.ToDouble(sortedx[i, k]))
                                {
                                    res[ind_categoria, k] = Convert.ToDouble(sortedx[i, k]);
                                }
                            }
                        }
                    }
                    break;

                case TipoEstatisticaAmostra.N:
                    for (int k = 0; k < res.GetLength(1); k++)
                    {
                        for (int i = 0; i < res.GetLength(0); i++)
                        {
                            res[i, k] = Convert.ToDouble(tabela_frequencias[k, 1]);
                        }
                    }
                    break;

                case TipoEstatisticaAmostra.Soma:
                    this.SortByColumn(ref sortedx, this.Concateh(this.ConvertArray(dados), categorias), dados.GetLength(1));
                    for (int k = 0; k < res.GetLength(1); k++)
                    {
                        for (int ind_categoria = 0; ind_categoria < freq_absoluta.GetLength(0); ind_categoria++)
                        {
                            res[ind_categoria, k] = 0.0;
                            for (int i = freq_acumulada[ind_categoria]; i < freq_acumulada[ind_categoria + 1]; i++)
                            {
                                res[ind_categoria, k] += Convert.ToDouble(sortedx[i, k]);
                            }
                        }
                    }
                    break;
                case TipoEstatisticaAmostra.Media:
                    this.SortByColumn(ref sortedx, this.Concateh(this.ConvertArray(dados), categorias), dados.GetLength(1));
                    for (int k = 0; k < res.GetLength(1); k++)
                    {
                        for (int ind_categoria = 0; ind_categoria < freq_absoluta.GetLength(0); ind_categoria++)
                        {
                            res[ind_categoria, k] = 0.0;
                            for (int i = freq_acumulada[ind_categoria]; i < freq_acumulada[ind_categoria + 1]; i++)
                            {
                                res[ind_categoria, k] += Convert.ToDouble(sortedx[i, k]);
                            }
                            res[ind_categoria, k] = res[ind_categoria, k] / (double)freq_absoluta[ind_categoria];
                        }
                    }
                    break;

                case TipoEstatisticaAmostra.VarianciaPopulacional:

                    this.SortByColumn(ref sortedx, this.Concateh(this.ConvertArray(dados), categorias), dados.GetLength(1));

                    medias = new double[res.GetLength(0), res.GetLength(1)];

                    for (int k = 0; k < res.GetLength(1); k++)
                    {
                        for (int ind_categoria = 0; ind_categoria < freq_absoluta.GetLength(0); ind_categoria++)
                        {
                            medias[ind_categoria, k] = 0.0;
                            for (int i = freq_acumulada[ind_categoria]; i < freq_acumulada[ind_categoria + 1]; i++)
                            {
                                medias[ind_categoria, k] += Convert.ToDouble(sortedx[i, k]);
                            }
                            medias[ind_categoria, k] = medias[ind_categoria, k] / (double)freq_absoluta[ind_categoria];

                            res[ind_categoria, k] = 0.0;
                            for (int i = freq_acumulada[ind_categoria]; i < freq_acumulada[ind_categoria + 1]; i++)
                            {
                                res[ind_categoria, k] += Math.Pow(Convert.ToDouble(sortedx[i, k]) - medias[ind_categoria, k], 2.0);
                            }
                            res[ind_categoria, k] = res[ind_categoria, k] / (double)freq_absoluta[ind_categoria];
                        }
                    }

                    break;

                case TipoEstatisticaAmostra.DesvioPadraoPopulacional:

                    this.SortByColumn(ref sortedx, this.Concateh(this.ConvertArray(dados), categorias), dados.GetLength(1));

                    medias = new double[res.GetLength(0), res.GetLength(1)];

                    for (int k = 0; k < res.GetLength(1); k++)
                    {
                        for (int ind_categoria = 0; ind_categoria < freq_absoluta.GetLength(0); ind_categoria++)
                        {
                            medias[ind_categoria, k] = 0.0;
                            for (int i = freq_acumulada[ind_categoria]; i < freq_acumulada[ind_categoria + 1]; i++)
                            {
                                medias[ind_categoria, k] += Convert.ToDouble(sortedx[i, k]);
                            }
                            medias[ind_categoria, k] = medias[ind_categoria, k] / (double)freq_absoluta[ind_categoria];

                            res[ind_categoria, k] = 0.0;
                            for (int i = freq_acumulada[ind_categoria]; i < freq_acumulada[ind_categoria + 1]; i++)
                            {
                                res[ind_categoria, k] += Math.Pow(Convert.ToDouble(sortedx[i, k]) - medias[ind_categoria, k], 2.0);
                            }
                            res[ind_categoria, k] = Math.Sqrt(res[ind_categoria, k] / (double)freq_absoluta[ind_categoria]);
                        }
                    }

                    break;
                default:
                    break;
            }

            return res;
        }

        public object[,] ConvertArray(double[,] a)
        {
            object[,] res = new object[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    res[i, j] = a[i, j];
                }
            }
            return res;
        }

        /// <summary>
        /// Sort all rows of a matrix, according to a specified column (between 0 and n-1, where
        /// n is the number of columns). 
        /// </summary>
        /// <param name="sortedx">Output sorted matrix by specified columns.</param>
        /// <param name="x">Matrix to be sorted.</param>
        /// <param name="sorting_column">Integer with index of sorting column.</param>
        /// <returns>Returns 1 if algorithm successful or -1 if indexes out of range.</returns>
        public int SortByColumn(ref object[,] sortedx, object[,] x, int sorting_column)
        {
            int sortcolumn = sorting_column;
            if (sortcolumn > x.GetLength(1) - 1) sortcolumn = x.GetLength(1) - 1;
            if (sortcolumn < 0) sortcolumn = 0;

            Rank rk = new Rank();

            object[,] v = this.SubColumnArrayDouble(x, sortcolumn);

            ulong n = Convert.ToUInt64(v.GetLength(0));
            ulong[] indx = new ulong[n];
            ulong[] irank = new ulong[n];

            clsSort sort = new clsSort();

            sort.indexx(n, v, indx);
            rk.rank(ref indx, ref irank);

            object[,] res = new object[x.GetLength(0), x.GetLength(1)];
            for (int i = 0; i < x.GetLength(0); i++)
            {
                for (int j = 0; j < x.GetLength(1); j++)
                {
                    res[Convert.ToInt32(irank[i]), j] = x[i, j];
                }
            }

            sortedx = this.ArrayDoubleClone(res);
            return 1;
        }

        /// <summary>
        /// Função para retornar a matriz de ranks a partir de uma determinada matriz de entrada.
        /// </summary>
        /// <param name="v">Matriz de doubles de entrada.</param>
        /// <returns>Substitui cada coluna da matriz original, pelos ranks das observações naquela coluna. Os ranks vão de 1 até o número de 
        /// valores distintos na coluna.</returns>
        public double[,] ArrayRanks(double[,] v)
        {
            double[,] r;
            double[,] ftable = new double[0, 0];
            double[,] ranks = new double[v.GetLength(0), v.GetLength(1)];
            int n = v.GetLength(0);

            for (int k = 0; k < v.GetLength(1); k++)
            {
                r = this.SubColumnArrayDouble(v, k);
                this.FrequencyTable(ref ftable, r);

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < ftable.GetLength(0); j++)
                    {
                        if (v[i, k] == ftable[j, 0])
                        {
                            ranks[i, k] = (double)(j + 1);
                            break;
                        }
                    }
                }
            }

            return ranks;
        }

        public double Sum(double[,] v)
        {
            double res = 0.0;
            for (int i = 0; i < v.GetLength(0); i++)
            {
                for (int j = 0; j < v.GetLength(1); j++)
                {
                    res += v[i, j];
                }
            }
            return res;
        }

        public void GeraSubRows(ref double[] res, double[] dados, int first_row, int last_row)
        {
            if (first_row < 0 || last_row > dados.GetLength(0) - 1) throw new Exception("Primeira e última linhas não válidas em rotina de sublinhas");
            res = new double[last_row - first_row + 1];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                res[i] = dados[i + first_row];
            }
        }

        public void GeraSubRows(ref double[,] res, double[,] dados, int first_row, int last_row)
        {
            if (first_row < 0 || last_row > dados.GetLength(0)-1) throw new Exception("Primeira e última linhas não válidas em rotina de sublinhas");
            res = new double[last_row - first_row + 1, dados.GetLength(1)];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                    res[i, j] = dados[i+first_row, j];
            }
        }

        public double VarianciaColumnMatrix(double[,] dados)
        {
            double media = 0.0;
            for (int i = 0; i < dados.GetLength(0); i++) media += dados[i, 0];
            media /= (double)dados.GetLength(0);

            double v = 0.0;
            for (int i = 0; i < dados.GetLength(0); i++) v += Math.Pow((dados[i, 0] - media), 2.0);
            v /= (double)(dados.GetLength(0) - 1);
            return v;
        }

        /// <summary>
        /// Subcoluna de uma matriz.
        /// </summary>
        public object[,] SubColumnArrayDouble(object[,] a, int indice)
        {
            if (a.GetLength(0) == 0 || a.GetLength(1) == 0) return new object[0, 0];

            if (indice < 0 || indice > a.GetLength(1) - 1) throw new Exception("Índice da subcoluna inválido");

            object[,] r = new object[a.GetLength(0), 1];
            for (int i = 0; i < r.GetLength(0); i++)
                r[i, 0] = a[i, indice];

            return r;
        }

        /// <summary>
        /// Subcoluna de uma matriz.
        /// </summary>
        public double[,] SubColumnArrayDouble(double[,] a, int indice)
        {
            if (a.GetLength(0) == 0 || a.GetLength(1) == 0) return new double[0, 0];

            if (indice < 0 || indice > a.GetLength(1) - 1) throw new Exception("Índice da subcoluna inválido");

            double[,] r = new double[a.GetLength(0), 1];
            for (int i = 0; i < r.GetLength(0); i++)
                r[i, 0] = a[i, indice];

            return r;
        }

        /// <summary>
        /// Sublinha de uma matriz.
        /// </summary>
        public double[,] SubRowArrayDouble(double[,] a, int indice)
        {
            if (a.GetLength(0) == 0 || a.GetLength(1) == 0) return new double[0, 0];

            if (indice < 0 || indice > a.GetLength(0) - 1) throw new Exception("Índice da sublinha inválido");

            double[,] r = new double[1, a.GetLength(1)];
            for (int i = 0; i < r.GetLength(1); i++)
                r[0, i] = a[indice, i];

            return r;
        }

        /// <summary>
        /// Diferença entre duas matrizes.
        /// </summary>
        public double[,] DiffArrayDouble(double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || b.GetLength(1) != a.GetLength(1))
                throw new Exception("Matrizes devem possuir mesma dimensão em rotina de diferença de matrizes");

            double[,] r = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    r[i, j] = a[i, j] - b[i, j];

            return r;
        }

        public double[,] MatrizDiv(double[,] a, double c)
        {
            double[,] res = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j] / c;

            return res;
        }
        
        public double[,] MatrizMult(double c, double[,] a)
        {
            double[,] res = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j] * c;

            return res;
        }

        public double[,] MatrizMult(double[,] a, double c)
        {
            double[,] res = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j] * c;

            return res;
        }

        public double[,] MatrizSubtracao(double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1)) throw new Exception("Dimensões das matrizes não estão adequadas para soma das matrizes");
            double[,] r = new double[a.GetLength(0), b.GetLength(1)];

            for (int i = 0; i < r.GetLength(0); i++)
            {
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    r[i, j] = a[i, j] - b[i, j];
                }
            }

            return r;
        }

        public double[,] MatrizSoma(double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1)) throw new Exception("Dimensões das matrizes não estão adequadas para soma das matrizes");
            double[,] r = new double[a.GetLength(0), b.GetLength(1)];

            for (int i = 0; i < r.GetLength(0); i++)
            {
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    r[i, j] = a[i, j] + b[i, j];
                }
            }

            return r;
        }

        public double[,] MatrizMult(double[,] a, double[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0)) throw new Exception("Dimensões das matrizes não estão adequadas para multiplicação");
            double[,] r = new double[a.GetLength(0), b.GetLength(1)];

            for (int i = 0; i < r.GetLength(0); i++)
            {
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    r[i, j] = 0.0;
                    for (int k = 0; k < a.GetLength(1); k++)
                        r[i, j] += a[i, k] * b[k, j];
                }
            }

            return r;
        }

        public double[,] MatrizTransp(double[,] a)
        {
            double[,] r = new double[a.GetLength(1), a.GetLength(0)];
            for (int i = 0; i < r.GetLength(0); i++)
                for (int j = 0; j < r.GetLength(1); j++)
                    r[i, j] = a[j, i];

            return r;
        }

        public double[,] CovSampleMatrix(double[,] col_data)
        {
            int nvar = col_data.GetLength(1);
            int nobs = col_data.GetLength(0);

            double[,] medias = this.Meanc(col_data);
            double[,] demeaned = new double[col_data.GetLength(0), col_data.GetLength(1)];

            for (int i = 0; i < nobs; i++)
            {
                for (int j = 0; j < nvar; j++)
                {
                    demeaned[i, j] = col_data[i, j] - medias[0, j];
                }
            }

            double[,] covm = this.MatrizMult(this.MatrizTransp(demeaned), demeaned);

            for (int i = 0; i < covm.GetLength(0); i++)
                for (int j = 0; j < covm.GetLength(1); j++)
                    covm[i, j] = covm[i, j] / (double)(covm.GetLength(0) - 1);

            return covm;
        }

        /// <summary>Norma Euclidiana de uma matriz.</summary>
        public double Norm(double[,] A)
        {
            double f = 0;
            for (int j = 0; j < A.GetLength(1); j++)
            {
                for (int i = 0; i < A.GetLength(0); i++)
                {
                    f += Math.Pow(A[i, j], 2);
                }
            }
            return Math.Sqrt(f);
        }

        /// <summary>Mínimos das colunas de uma matriz.</summary>
        public double[,] Minc(double[,] a)
        {
            double[,] r = new double[1, a.GetLength(1)];
            for (int j = 0; j < a.GetLength(1); j++)
            {
                r[0, j] = a[0, j];
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    if (r[0, j] > a[i, j]) r[0, j] = a[i, j];
                }
            }
            return r;
        }

        /// <summary>Máximos das colunas de uma matriz.</summary>
        public double[,] Maxc(double[,] a)
        {
            double[,] r = new double[1, a.GetLength(1)];
            for (int j = 0; j < a.GetLength(1); j++)
            {
                r[0, j] = a[0,j];
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    if (r[0, j] < a[i, j]) r[0, j] = a[i,j];
                }
            }
            return r;
        }

        public double[,] Medianc(double[,] a)
        {
            double[,] r = new double[1, a.GetLength(1)];
            double[,] sorted_a = this.SortcDoubleArray(a);
            int indice_baixo = 0;
            int indice_alto = 0;

            if (Math.Floor((double)a.GetLength(0) / 2.0) == (double)a.GetLength(0) / 2.0)
            {
                indice_baixo = (int)Math.Floor((double)a.GetLength(0) / 2.0);
                indice_alto = (int)Math.Floor((double)a.GetLength(0) / 2.0) + 1;
            }
            else
            {
                indice_baixo = (int)Math.Floor((double)a.GetLength(0) / 2.0) + 1;
                indice_alto = (int)Math.Floor((double)a.GetLength(0) / 2.0) + 1;
            }

            for (int j = 0; j < a.GetLength(1); j++)
            {
                r[0, j] = (sorted_a[indice_baixo-1, j] + sorted_a[indice_alto-1, j])/2.0;
            }

            return r;
        }

        /// <summary>Média das colunas de uma matriz.</summary>
        public double[,] Meanc(double[,] a)
        {
            double[,] r = new double[1, a.GetLength(1)];
            for (int j = 0; j < a.GetLength(1); j++)
            {
                r[0, j] = 0.0;
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    r[0, j] += a[i, j];
                }
                r[0, j] = r[0, j] / Convert.ToDouble(a.GetLength(0));
            }
            return r;
        }

        /// <summary>
        /// Concatenação de vetores.
        /// </summary>
        public double[] ConcateArraysDouble(double[] a, double[] b)
        {
            if (a.GetLength(0) == 0)
            {
                double[] rb = new double[b.GetLength(0)];
                for (int i = 0; i < rb.GetLength(0); i++) rb[i] = b[i];
                return rb;
            }

            if (b.GetLength(0) == 0)
            {
                double[] ra = new double[a.GetLength(0)];
                for (int i = 0; i < ra.GetLength(0); i++) ra[i] = a[i];
                return ra;
            }

            double[] res = new double[a.GetLength(0) + b.GetLength(0)];
            for (int j = 0; j < a.GetLength(0); j++)
                res[j] = a[j];

            for (int j = 0; j < b.GetLength(0); j++)
                res[j + a.GetLength(0)] = b[j];

            return res;
        }

        /// <summary>
        /// Concatenação vertical de matrizes.
        /// </summary>
        public object[,] Concatev(object[,] a, object[,] b)
        {
            if (b.GetLength(0) == 0 && b.GetLength(1) == 0) return this.ArrayDoubleClone(a);
            if (a.GetLength(0) == 0 && a.GetLength(1) == 0) return this.ArrayDoubleClone(b);

            if (a.GetLength(1) != b.GetLength(1)) throw new Exception("Matrizes devem possuir o mesmo número de colunas em concatenação vertical");

            object[,] res = new object[a.GetLength(0) + b.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(1); i++)
            {
                for (int j = 0; j < a.GetLength(0); j++)
                    res[j, i] = a[j, i];

                for (int j = 0; j < b.GetLength(0); j++)
                    res[j + a.GetLength(0), i] = b[j, i];
            }

            return res;
        }

        /// <summary>
        /// Concatenação vertical de matrizes.
        /// </summary>
        public double[,] Concatev(double[,] a, double[,] b)
        {
            if (b.GetLength(0) == 0 && b.GetLength(1) == 0) return this.ArrayDoubleClone(a);
            if (a.GetLength(0) == 0 && a.GetLength(1) == 0) return this.ArrayDoubleClone(b);

            if (a.GetLength(1) != b.GetLength(1)) throw new Exception("Matrizes devem possuir o mesmo número de colunas em concatenação vertical");

            double[,] res = new double[a.GetLength(0) + b.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(1); i++)
            {
                for (int j = 0; j < a.GetLength(0); j++)
                    res[j, i] = a[j, i];

                for (int j = 0; j < b.GetLength(0); j++)
                    res[j + a.GetLength(0), i] = b[j, i];
            }

            return res;
        }

        /// <summary>
        /// Concatenação horizontal de matrizes.
        /// </summary>
        public object[,] Concateh(object[,] a, object[,] b)
        {
            if (b.GetLength(0) == 0 && b.GetLength(1) == 0) return this.ArrayObjectClone(a);
            if (a.GetLength(0) == 0 && a.GetLength(1) == 0) return this.ArrayObjectClone(b);

            if (a.GetLength(0) != b.GetLength(0)) throw new Exception("Matrizes devem possuir o mesmo número de linhas em concatenação horizontal");

            object[,] res = new object[a.GetLength(0), a.GetLength(1) + b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j];

                for (int j = 0; j < b.GetLength(1); j++)
                    res[i, a.GetLength(1) + j] = b[i, j];
            }

            return res;
        }

        /// <summary>
        /// Concatenação horizontal de matrizes.
        /// </summary>
        public double[,] Concateh(double[,] a, double[,] b)
        {
            if (b.GetLength(0) == 0 && b.GetLength(1) == 0) return this.ArrayDoubleClone(a);
            if (a.GetLength(0) == 0 && a.GetLength(1) == 0) return this.ArrayDoubleClone(b);

            if (a.GetLength(0) != b.GetLength(0)) throw new Exception("Matrizes devem possuir o mesmo número de linhas em concatenação horizontal");

            double[,] res = new double[a.GetLength(0), a.GetLength(1) + b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j];

                for (int j = 0; j < b.GetLength(1); j++)
                    res[i, a.GetLength(1) + j] = b[i, j];
            }

            return res;
        }

        /// <summary>
        /// Clona uma matriz.
        /// </summary>
        public double[,] ArrayDoubleClone(double[] a)
        {
            double[,] res = new double[a.GetLength(0), 1];
            for (int i = 0; i < a.GetLength(0); i++)
                    res[i, 0] = a[i];

            return res;
        }

        /// <summary>
        /// Clona uma matriz.
        /// </summary>
        public object[,] ArrayDoubleClone(object[] a)
        {
            object[,] res = new object[a.GetLength(0), 1];
            for (int i = 0; i < a.GetLength(0); i++)
                res[i, 0] = a[i];

            return res;
        }

        /// <summary>
        /// Clona uma matriz.
        /// </summary>
        public object[,] ArrayObjectClone(object[] a)
        {
            object[,] res = new object[a.GetLength(0), 1];
            for (int i = 0; i < a.GetLength(0); i++)
                res[i, 0] = a[i];

            return res;
        }

        /// <summary>
        /// Clona uma matriz.
        /// </summary>
        public object[,] ArrayDoubleClone(object[,] a)
        {
            object[,] res = new object[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j];

            return res;
        }

        /// <summary>
        /// Clona uma matriz.
        /// </summary>
        public object[,] ArrayObjectClone(object[,] a)
        {
            object[,] res = new object[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j];

            return res;
        }

        /// <summary>
        /// Clona uma matriz.
        /// </summary>
        public double[,] ArrayDoubleClone(double[,] a)
        {
            double[,] res = new double[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j];

            return res;
        }

        /// <summary>
        /// Retorna uma matriz, com colunas ordenadas independentemente.
        /// </summary>
        public double[,] SortcDoubleArray(double[,] x)
        {
            int problem = 1;
            string message = "";

            clsSort auxc = new clsSort();
            int n = x.GetLength(0);
            ulong ln = Convert.ToUInt64(n);
            double[] a = new double[n];
            for (int i = 0; i < n; i++) { a[i] = x[i, 0]; }
            auxc.sort(ref a, ln, ref problem, ref message);
            double[,] X = this.ArrayDoubleClone(a);
            double[,] aux;
            for (int j = 1; j < x.GetLength(1); j++)
            {
                for (int i = 0; i < n; i++) { a[i] = x[i, j]; }
                auxc.sort(ref a, ln, ref problem, ref message);
                aux = this.ArrayDoubleClone(a);
                X = this.Concateh(X, aux);
            }
            return X;
        }

        /// <summary>
        /// Retorna uma matriz, com colunas ordenadas independentemente.
        /// </summary>
        public object[,] SortcDoubleArray(object[,] x)
        {
            int problem = 1;
            string message = "";

            clsSort auxc = new clsSort();
            int n = x.GetLength(0);
            ulong ln = Convert.ToUInt64(n);
            object[] a = new object[n];
            for (int i = 0; i < n; i++) { a[i] = x[i, 0]; }
            auxc.sort(ref a, ln, ref problem, ref message);
            object[,] X = this.ArrayObjectClone(a);
            object[,] aux;
            for (int j = 1; j < x.GetLength(1); j++)
            {
                for (int i = 0; i < n; i++) { a[i] = x[i, j]; }
                auxc.sort(ref a, ln, ref problem, ref message);
                aux = this.ArrayObjectClone(a);
                X = this.Concateh(X, aux);
            }
            return X;
        }

        /// <summary>
        /// Tabela de frequência para uma variável categórica. 
        /// </summary>
        /// <param name="table">Matriz de saída: primeira coluna corresponde às categorias; segunda coluna corresponde à contagem de cada categoria.</param>
        /// <param name="cats">Vetor coluna com as categorias.</param>
        public void FrequencyTable(ref object[,] table, object[,] cats)
        {
            int nobs = cats.GetLength(0);
            object[,] scats = this.SortcDoubleArray(cats);
            object[,] categorias;

            int index = 1;
            object temp = scats[0, 0];

            clsSort srt = new clsSort();

            for (int i = 1; i < nobs; i++)
            {
                //if (scats[i, 0] != temp)
                if (!srt.EqualTo(scats[i,0], temp))
                {
                    index++;
                    temp = scats[i, 0];
                }
            }

            categorias = new object[index, 1];
            categorias[0, 0] = scats[0, 0];
            index = 0;
            for (int i = 1; i < nobs; i++)
            {
                //if (scats[i, 0] != categorias[index, 0])
                if (!srt.EqualTo(scats[i,0], categorias[index, 0]))
                {
                    index++;
                    categorias[index, 0] = scats[i, 0];
                }
            }

            object[,] counts = new object[categorias.GetLength(0), 1];
            for (int i = 0; i < nobs; i++)
            {
                for (int k = 0; k < categorias.GetLength(0); k++)
                {
                    //if (scats[i, 0] == categorias[k, 0])
                    if (srt.EqualTo(scats[i,0], categorias[k,0]))
                    {
                        counts[k, 0] = Convert.ToDouble(counts[k,0]) + 1.0;
                        break;
                    }
                }
            }
            table = this.Concateh(categorias, counts);
        }

        /// <summary>
        /// Tabela de frequência para uma variável categórica. 
        /// </summary>
        /// <param name="table">Matriz de saída: primeira coluna corresponde às categorias; segunda coluna corresponde à contagem de cada categoria.</param>
        /// <param name="cats">Vetor coluna com as categorias.</param>
        public void FrequencyTable(ref double[,] table, double[,] cats)
        {
            int nobs = cats.GetLength(0);
            double[,] scats = this.SortcDoubleArray(cats);
            double[,] categorias;

            int index = 1;
            double temp = scats[0, 0];

            for (int i = 1; i < nobs; i++)
            {
                if (scats[i,0] != temp)
                {
                    index++;
                    temp = scats[i,0];
                }
            }

            categorias = new double[index, 1];
            categorias[0, 0] = scats[0,0];
            index = 0;
            for (int i = 1; i < nobs; i++)
            {
                if (scats[i,0] != categorias[index,0])
                {
                    index++;
                    categorias[index,0] = scats[i,0];
                }
            }            
            
            double[,] counts = new double[categorias.GetLength(0), 1];
            for (int i = 0; i < nobs; i++)
            {
                for (int k = 0; k < categorias.GetLength(0); k++)
                {
                    if (scats[i,0] == categorias[k,0])
                    {
                        counts[k,0]++;
                        break;
                    }
                }
            }
            table = this.Concateh(categorias, counts);
        }

        public double[,] MatrizInversa(double[,] mat)
        {
            GaussJordan gj = new GaussJordan();
            double[,] b = this.ArrayDoubleClone(mat);

            gj.gaussj(ref b);

            return b;
        }

        /// <summary>
        /// Calcula os autovalores e os autovetores de uma matriz simétrica (retorna autovalores ordenados)
        /// </summary>
        /// <param name="x">Matriz de entrada</param>
        /// <param name="V">Referencia para os autovetores (em colunas)</param>
        /// <param name="D">Referencis para os autovalores ordenados</param>
        public void AutovaloresMatrizSimetrica(double[,] x, ref double[,] V, ref double[] D)
        {
            Jacobi jac = new Jacobi(x);
            V = jac.v;
            D = jac.d;
        }
    }

    #endregion 

    #region Classe para quicksort

    /// <summary>
    /// Given indx[1..n] as output from the routine indexx, returns an array irank[1..n], the
    /// corresponding table of ranks.
    /// </summary>
    internal class Rank
    {
        public Rank()
        {
        }

        public void rank(ref ulong[] indx, ref ulong[] irank)
        {
            ulong j = 0;

            ulong n = Convert.ToUInt64(indx.GetLength(0));
            for (j = 0; j < n; j++) irank[Convert.ToInt32(indx[j]) - 1] = j;
        }
    }

    internal class clsSort
    {
        #region Funções de comparação

        public bool LessOrEqualTo(object v1, object v2)
        {
            if ((v1 is string) && (v2 is string))
            {
                int rcompare = Convert.ToString(v1).CompareTo(Convert.ToString(v2));
                if (rcompare > 0) return false;
                else return true;
            }

            return (this.LessThan(v1, v2) || this.EqualTo(v1, v2));
        }

        public bool GreaterOrEqualTo(object v1, object v2)
        {
            if ((v1 is string) && (v2 is string))
            {
                int rcompare = Convert.ToString(v1).CompareTo(Convert.ToString(v2));
                if (rcompare < 0) return false;
                else return true;
            }

            return (this.GreaterThan(v1, v2) || this.EqualTo(v1, v2));
        }

        public bool GreaterThan(object v1, object v2)
        {
            if ((v1 is string) && (v2 is string))
            {
                int rcompare = Convert.ToString(v1).CompareTo(Convert.ToString(v2));
                if (rcompare <= 0) return false;
                else return true;
            }

            if ((v1 is DateTime) && (v2 is DateTime))
            {
                return (Convert.ToDateTime(v1) > Convert.ToDateTime(v2));
            }

            if (!(v1 is string) && !(v2 is string))
            {
                return (Convert.ToDouble(v1) > Convert.ToDouble(v2));
            }
            else
            {
                double u1;
                double u2;
                string s1 = v1.ToString();
                string s2 = v2.ToString();
                if (double.TryParse(s1, this.m_style, this.m_culture, out u1) &&
                    double.TryParse(s2, this.m_style, this.m_culture, out u2))
                {
                    return (u1 > u2);
                }
                else
                {
                    if (s1.Length == s2.Length)
                    {
                        for (int i = 0; i < s2.Length; i++)
                        {
                            if (Convert.ToChar(s1[i]) > Convert.ToChar(s2[i])) return true;
                        }
                        return false;
                    }
                    if (s1.Length > s2.Length)
                    {
                        if (s1.Substring(0, s2.Length) == s2) return true;
                        for (int i = 0; i < s2.Length; i++)
                        {
                            if (Convert.ToChar(s1[i]) > Convert.ToChar(s2[i])) return true;
                        }
                        return false;
                    }
                    if (s1.Length < s2.Length)
                    {
                        if (s1 == s2.Substring(0, s1.Length)) return false;
                        for (int i = 0; i < s1.Length; i++)
                        {
                            if (Convert.ToChar(s1[i]) > Convert.ToChar(s2[i])) return true;
                        }
                        return false;
                    }
                    return false;
                }
            }
        }

        public bool LessThan(object v1, object v2)
        {
            if ((v1 is string) && (v2 is string))
            {
                int rcompare = Convert.ToString(v1).CompareTo(Convert.ToString(v2));
                if (rcompare >= 0) return false;
                else return true;
            }

            if ((v1 is DateTime) && (v2 is DateTime))
            {
                return (Convert.ToDateTime(v1) < Convert.ToDateTime(v2));
            }

            if (!(v1 is string) && !(v2 is string))
            {
                return (Convert.ToDouble(v1) < Convert.ToDouble(v2));
            }
            else
            {
                double u1;
                double u2;
                string s1 = v1.ToString();
                string s2 = v2.ToString();
                if (double.TryParse(s1, this.m_style, this.m_culture, out u1) &&
                    double.TryParse(s2, this.m_style, this.m_culture, out u2))
                {
                    return (u1 < u2);
                }
                else
                {
                    if (s1.Length == s2.Length)
                    {
                        for (int i = 0; i < s2.Length; i++)
                        {
                            if (Convert.ToChar(s1[i]) < Convert.ToChar(s2[i])) return true;
                        }
                        return false;
                    }
                    if (s1.Length > s2.Length)
                    {
                        if (s1.Substring(0, s2.Length) == s2) return false;
                        for (int i = 0; i < s2.Length; i++)
                        {
                            if (Convert.ToChar(s1[i]) < Convert.ToChar(s2[i])) return true;
                        }
                        return false;
                    }
                    if (s1.Length < s2.Length)
                    {
                        if (s1 == s2.Substring(0, s1.Length)) return true;
                        for (int i = 0; i < s1.Length; i++)
                        {
                            if (Convert.ToChar(s1[i]) < Convert.ToChar(s2[i])) return true;
                        }
                        return false;
                    }
                    return false;
                }
            }
        }

        public bool EqualTo(object[,] v1, object[,] v2)
        {
            if (v1.GetLength(0) != v2.GetLength(0)) return false;
            if (v1.GetLength(1) != v2.GetLength(1)) return false;
            for (int i = 0; i < v1.GetLength(0); i++)
            {
                for (int j = 0; j < v1.GetLength(1); j++)
                {
                    if (!this.EqualTo(v1[i, j], v2[i, j])) return false;
                }
            }
            return true;
        }

        public bool EqualTo(object v1, object v2)
        {
            if ((v1 is DateTime) && (v2 is DateTime))
            {
                return (Convert.ToDateTime(v1) == Convert.ToDateTime(v2));
            }

            if (!(v1 is string) && !(v2 is string))
            {
                return (Convert.ToDouble(v1) == Convert.ToDouble(v2));
            }
            else
            {
                double u1;
                double u2;
                string s1 = v1.ToString();
                string s2 = v2.ToString();
                if (double.TryParse(s1, this.m_style, this.m_culture, out u1) &&
                    double.TryParse(s2, this.m_style, this.m_culture, out u2))
                {
                    return (u1 == u2);
                }
                else
                {
                    return (s1 == s2);
                }
            }
        }

        #endregion

        private NumberStyles m_style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands 
                                       | NumberStyles.AllowCurrencySymbol | NumberStyles.Number;

		private CultureInfo m_culture = CultureInfo.CurrentCulture;
        //private CultureInfo m_culture = CultureInfo.GetCultureInfo("en-US");
        //private CultureInfo m_culture = CultureInfo.GetCultureInfo("pt-BR");

        public clsSort()
        {
            this.m_style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands
                                     | NumberStyles.AllowCurrencySymbol | NumberStyles.Number;
            //this.m_culture = CultureInfo.GetCultureInfo("en-US");
            this.m_culture = CultureInfo.CurrentCulture;
        }

        #region Sorting arrays de doubles

        private int M = 7;
        private int NSTACK = 500;
        
        public int sort(ref object[] arr, ulong n, ref int problem, ref string message)
        {
            problem = 1;
            message = "Quicksort routine successful";

            ulong i, ir = n, j, k, l = 1;
            int jstack = 0;
            object a, temp;

            ulong[] istack = new ulong[NSTACK];
            for (; ; )
            {
                if (ir - l < (ulong)M)
                {
                    for (j = l + 1; j <= ir; j++)
                    {
                        a = arr[j - 1];
                        for (i = j - 1; i >= 1; i--)
                        {
                            //if (arr[i - 1] <= a) break;

                            if (this.LessOrEqualTo(arr[i - 1], a)) break;
                            arr[i] = arr[i - 1];
                        }
                        arr[i] = a;
                    }
                    if (jstack == 0) break;
                    ir = istack[(jstack--) - 1];
                    l = istack[(jstack--) - 1];
                }
                else
                {
                    k = (l + ir) >> 1;
                    temp = arr[k - 1];
                    arr[k - 1] = arr[l];
                    arr[l] = temp;

                    //if (arr[l] > arr[ir - 1])
                    if (this.GreaterThan(arr[l], arr[ir - 1]))
                    {
                        temp = arr[l];
                        arr[l] = arr[ir - 1];
                        arr[ir - 1] = temp;
                    }

                    //if (arr[l - 1] > arr[ir - 1])
                    if (this.GreaterThan(arr[l - 1], arr[ir - 1]))
                    {
                        temp = arr[l - 1];
                        arr[l - 1] = arr[ir - 1];
                        arr[ir - 1] = temp;
                    }

                    //if (arr[l] > arr[l - 1])
                    if (this.GreaterThan(arr[l], arr[l - 1]))
                    {
                        temp = arr[l];
                        arr[l] = arr[l - 1];
                        arr[l - 1] = temp;
                    }
                    i = l + 1;
                    j = ir;
                    a = arr[l - 1];
                    for (; ; )
                    {
                        //do i++; while (arr[i - 1] < a);
                        do i++; while (this.LessThan(arr[i - 1], a));
                        
                        //do j--; while (arr[j - 1] > a);
                        do j--; while (this.GreaterThan(arr[j - 1], a));

                        if (j < i) break;
                        temp = arr[i - 1];
                        arr[i - 1] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                    arr[l - 1] = arr[j - 1];
                    arr[j - 1] = a;
                    jstack += 2;
                    if (jstack > NSTACK)
                    {
                        message = "NSTACK too small in quicksort routine";
                        problem = 1;
                        return 1;
                    }
                    if (ir - i + 1 >= j - l)
                    {
                        istack[jstack - 1] = ir;
                        istack[jstack - 2] = i;
                        ir = j - 1;
                    }
                    else
                    {
                        istack[jstack - 1] = j - 1;
                        istack[jstack - 2] = l;
                        l = i;
                    }
                }
            }
            return 0;
        }

        #endregion

        #region Sorting arrays de objects

        public int sort(ref double[] arr, ulong n, ref int problem, ref string message)
        {
            problem = 1;
            message = "Quicksort routine successful";

            ulong i, ir = n, j, k, l = 1;
            int jstack = 0;
            double a, temp;

            ulong[] istack = new ulong[NSTACK];
            for (; ; )
            {
                if (ir - l < (ulong)M)
                {
                    for (j = l + 1; j <= ir; j++)
                    {
                        a = arr[j - 1];
                        for (i = j - 1; i >= 1; i--)
                        {
                            if (arr[i - 1] <= a) break;
                            arr[i] = arr[i - 1];
                        }
                        arr[i] = a;
                    }
                    if (jstack == 0) break;
                    ir = istack[(jstack--) - 1];
                    l = istack[(jstack--) - 1];
                }
                else
                {
                    k = (l + ir) >> 1;
                    temp = arr[k - 1];
                    arr[k - 1] = arr[l];
                    arr[l] = temp;
                    if (arr[l] > arr[ir - 1])
                    {
                        temp = arr[l];
                        arr[l] = arr[ir - 1];
                        arr[ir - 1] = temp;
                    }
                    if (arr[l - 1] > arr[ir - 1])
                    {
                        temp = arr[l - 1];
                        arr[l - 1] = arr[ir - 1];
                        arr[ir - 1] = temp;
                    }
                    if (arr[l] > arr[l - 1])
                    {
                        temp = arr[l];
                        arr[l] = arr[l - 1];
                        arr[l - 1] = temp;
                    }
                    i = l + 1;
                    j = ir;
                    a = arr[l - 1];
                    for (; ; )
                    {
                        do i++; while (arr[i - 1] < a);
                        do j--; while (arr[j - 1] > a);
                        if (j < i) break;
                        temp = arr[i - 1];
                        arr[i - 1] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                    arr[l - 1] = arr[j - 1];
                    arr[j - 1] = a;
                    jstack += 2;
                    if (jstack > NSTACK)
                    {
                        message = "NSTACK too small in quicksort routine";
                        problem = 1;
                        return 1;
                    }
                    if (ir - i + 1 >= j - l)
                    {
                        istack[jstack - 1] = ir;
                        istack[jstack - 2] = i;
                        ir = j - 1;
                    }
                    else
                    {
                        istack[jstack - 1] = j - 1;
                        istack[jstack - 2] = l;
                        l = i;
                    }
                }
            }
            return 0;
        }

        #endregion

        #region Indexização de ranks

        //private int M = 7;
        //private int NSTACK = 500;
        //private int NSTACK = 50;

        /// <summary>Indexes an array arr[0..n-1], i.e., outputs the array indx[0..n-1] such that arr[indx[j-1]-1] is
        /// in ascending order for j = 1, 2, . . . ,N. The input quantities n and arr are not changed.</summary>
        /// <param name="n">Ulong variable with size of the vector to be sorted.</param>
        /// <param name="arr">Double vector with data to be sorted.</param>
        /// <param name="indx">Output ulong vector with indexes for sorting.</param>
        public void indexx(ulong n, object[,] arr, ulong[] indx)
        {
            ulong i, indxt, ir = n, itemp, j, k, l = 1;
            int jstack = 0;
            //double a;
            object a;
            ulong[] istack = new ulong[NSTACK];
            for (j = 1; j <= n; j++) indx[j - 1] = j;
            for (; ; )
            {
                if (ir - l < (ulong)M)
                {
                    for (j = l + 1; j <= ir; j++)
                    {
                        indxt = indx[j - 1];
                        a = arr[indxt - 1, 0];
                        for (i = j - 1; i >= 1; i--)
                        {
                            //if (arr[indx[i - 1] - 1] <= a) break;
                            if (this.LessOrEqualTo(arr[indx[i - 1] - 1, 0], a)) break;
                            indx[i] = indx[i - 1];
                        }
                        indx[i] = indxt;
                    }
                    if (jstack == 0) break;
                    ir = istack[jstack-- - 1];
                    l = istack[jstack-- - 1];
                }
                else
                {
                    k = (l + ir) >> 1;
                    itemp = indx[k - 1];
                    indx[k - 1] = indx[l];
                    indx[l] = itemp;
                    //if (arr[indx[l] - 1] > arr[indx[ir - 1] - 1])
                    if (this.GreaterThan(arr[indx[l] - 1,0], arr[indx[ir - 1] - 1,0]))
                    {
                        itemp = indx[l];
                        indx[l] = indx[ir - 1];
                        indx[ir - 1] = itemp;
                    }
                    //if (arr[indx[l - 1] - 1] > arr[indx[ir - 1] - 1])
                    if (this.GreaterThan(arr[indx[l - 1] - 1,0], arr[indx[ir - 1] - 1,0]))
                    {
                        itemp = indx[l - 1];
                        indx[l - 1] = indx[ir - 1];
                        indx[ir - 1] = itemp;
                    }
                    //if (arr[indx[l] - 1] > arr[indx[l - 1] - 1])
                    if (this.GreaterThan(arr[indx[l] - 1,0], arr[indx[l - 1] - 1,0]))
                    {
                        itemp = indx[l];
                        indx[l] = indx[l - 1];
                        indx[l - 1] = itemp;
                    }
                    i = l + 1;
                    j = ir;
                    indxt = indx[l - 1];
                    a = arr[indxt - 1,0];
                    for (; ; )
                    {
                        //do i++; while (arr[indx[i - 1] - 1] < a);
                        do i++; while (this.LessThan(arr[indx[i - 1] - 1,0], a));
                        //do j--; while (arr[indx[j - 1] - 1] > a);
                        do j--; while (this.GreaterThan(arr[indx[j - 1] - 1,0], a));
                        if (j < i) break;
                        itemp = indx[i - 1];
                        indx[i - 1] = indx[j - 1];
                        indx[j - 1] = itemp;
                    }
                    indx[l - 1] = indx[j - 1];
                    indx[j - 1] = indxt;
                    jstack += 2;
                    if (jstack > NSTACK) try { throw new Exception(); }
                        catch (Exception)
                        {
                            //MessageBox.Show("NSTACK too small in indexx.",
                            //	 "Invalid method",MessageBoxButtons.OK, MessageBoxIcon.Error );
                        }
                    if (ir - i + 1 >= j - l)
                    {
                        istack[jstack - 1] = ir;
                        istack[jstack - 2] = i;
                        ir = j - 1;
                    }
                    else
                    {
                        istack[jstack - 1] = j - 1;
                        istack[jstack - 2] = l;
                        l = i;
                    }
                }
            }
        }

        #endregion 
    }

    #endregion 

    #region Classe para Gauss-Jordan elimination

    public class GaussJordan
    {
        /// <summary>
        /// Linear equation solution by Gauss-Jordan elimination, equation (2.1.1) above. The input matrix
        /// is a[0..n-1][0..n-1]. b[0..n-1][0..m-1] is input containing the m right-hand side vectors.
        /// On output, a is replaced by its matrix inverse, and b is replaced by the corresponding set of
        /// solution vectors.
        /// </summary>
        public void gaussj(ref double[,] a, ref double[,] b)
        {
            double temp = 0.0;
            int i, icol = 0, irow = 0, j, k, l, ll, n = a.GetLength(0), m = b.GetLength(1);
            double big, dum, pivinv;
            int[] indxc = new int[n];
            int[] indxr = new int[n];
            int[] ipiv = new int[n];
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
                                if (Math.Abs(a[j, k]) >= big)
                                {
                                    //big=Math.Abs(a[j,k]);
                                    big = Math.Abs(a[j, k]);
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
                        //SWAP(a[irow,l],a[icol,l]);
                        temp = a[irow, 1];
                        a[irow, 1] = a[icol, 1];
                        a[icol, 1] = temp;
                    }
                    for (l = 0; l < m; l++)
                    {
                        //SWAP(b[irow,l],b[icol,l]);
                        temp = b[irow, 1];
                        b[irow, 1] = b[icol, 1];
                        b[icol, 1] = temp;
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
                        a[ll, icol] = 0.0;
                        for (l = 0; l < n; l++) a[ll, l] -= a[icol, l] * dum;
                        for (l = 0; l < m; l++) b[ll, l] -= b[icol, l] * dum;
                    }
            }
            for (l = n - 1; l >= 0; l--)
            {
                if (indxr[l] != indxc[l])
                    for (k = 0; k < n; k++)
                    {
                        //SWAP(a[k,indxr[l]],a[k,indxc[l]]);
                        temp = a[k, indxr[l]];
                        a[k, indxr[l]] = a[k, indxc[l]];
                        a[k, indxc[l]] = temp;
                    }
            }
        }

        /// <summary>
        /// Overloaded version with no right-hand sides. Replaces a by its inverse.
        /// </summary>
        public void gaussj(ref double[,] a)
        {
            double[,] b = new double[a.GetLength(0), 1];
            gaussj(ref a, ref b);
        }
    }

    #endregion 
    
    #region Classe para encontrar auto-valores e auto-vetores de matrizes simétricas

    public class Jacobi
    {
        int n;
        double[,] a;
        public double[,] v;
        public double[] d;
        int nrot;
        double EPS;

        private void eigsrt(ref double[] d, ref double[,] v)
        {
	        int k;
	        int n=d.GetLength(0);
	        for (int i=0;i<n-1;i++) {
		        double p=d[k=i];
		        for (int j=i;j<n;j++)
			        if (d[j] >= p) p=d[k=j];
		        if (k != i) {
			        d[k]=d[i];
			        d[i]=p;
			        if (v != null)
				        for (int j=0;j<n;j++) {
					        p=v[j,i];
					        v[j,i]=v[j,k];
					        v[j,k]=p;
				        }
		        }
	        }
        }

        public Jacobi(double[,] aa)
        {
            clsUtilTools clt = new clsUtilTools();

            n = aa.GetLength(0);
            a = clt.ArrayDoubleClone(aa);
            v = new double[n,n];
            d = new double[n];
            nrot = 0;
            EPS = 1.0e-6;

            int i,j,ip,iq;
		    double tresh,theta,tau,t,sm,s,h,g,c;
		    double[] b = new double[n];
            double[] z = new double[n];
		    for (ip=0;ip<n;ip++) {
			    for (iq=0;iq<n;iq++) v[ip,iq]=0.0;
			    v[ip,ip]=1.0;
		    }
		    for (ip=0;ip<n;ip++) {
			    b[ip]=d[ip]=a[ip,ip];
			    z[ip]=0.0;
		    }
		    for (i=1;i<=50;i++) {
			    sm=0.0;
			    for (ip=0;ip<n-1;ip++) {
				    for (iq=ip+1;iq<n;iq++)
					    sm += Math.Abs(a[ip,iq]);
			    }
			    if (sm == 0.0) {
				    eigsrt(ref d, ref v);
				    return;
			    }
			    if (i < 4)
				    tresh=0.2*sm/(n*n);
			    else
				    tresh=0.0;
			    for (ip=0;ip<n-1;ip++) {
				    for (iq=ip+1;iq<n;iq++) {
					    g=100.0*Math.Abs(a[ip,iq]);
					    if (i > 4 && g <= EPS*Math.Abs(d[ip]) && g <= EPS*Math.Abs(d[iq]))
							    a[ip,iq]=0.0;
					    else if (Math.Abs(a[ip,iq]) > tresh) {
						    h=d[iq]-d[ip];
						    if (g <= EPS*Math.Abs(h))
							    t=(a[ip,iq])/h;
						    else {
							    theta=0.5*h/(a[ip,iq]);
							    t=1.0/(Math.Abs(theta)+Math.Sqrt(1.0+theta*theta));
							    if (theta < 0.0) t = -t;
						    }
						    c=1.0/Math.Sqrt(1+t*t);
						    s=t*c;
						    tau=s/(1.0+c);
						    h=t*a[ip,iq];
						    z[ip] -= h;
						    z[iq] += h;
						    d[ip] -= h;
						    d[iq] += h;
						    a[ip,iq]=0.0;
						    for (j=0;j<ip;j++)
							    rot(ref a,s,tau,j,ip,j,iq);
						    for (j=ip+1;j<iq;j++)
							    rot(ref a,s,tau,ip,j,j,iq);
						    for (j=iq+1;j<n;j++)
							    rot(ref a,s,tau,ip,j,iq,j);
						    for (j=0;j<n;j++)
							    rot(ref v,s,tau,j,ip,j,iq);
						    ++nrot;
					    }
				    }
			    }
			    for (ip=0;ip<n;ip++) {
				    b[ip] += z[ip];
				    d[ip]=b[ip];
				    z[ip]=0.0;
			    }
		    }
		    throw new Exception("Too many iterations in routine jacobi");
        }

        private void rot(ref double[,] a, double s, double tau, int i, int j, int k, int l)
	    {
		    double g=a[i,j];
		    double h=a[k,l];
		    a[i,j]=g-s*(h+g*tau);
		    a[k,l]=h+s*(g-h*tau);
	    }
    }

    #endregion 

    #region Classe para encontrar a decomposição de Cholesky

    public class Cholesky
    {
        private int n;
        private double[,] el;

        /// <summary>
        /// Constructor. Given a positive-definite symmetric matrix a[0..n-1][0..n-1], construct
        /// and store its Cholesky decomposition, A = L x L' .
        /// </summary>
        /// <param name="a">Input matrix.</param>
        public Cholesky(ref double[,] a)
        {
            clsUtilTools clt = new clsUtilTools();

            n = a.GetLength(0);
            el = clt.ArrayDoubleClone(a); 

            int i, j, k;
            double sum;
            if (el.GetLength(1) != n) throw new Exception("need square matrix");
            for (i = 0; i < n; i++)
            {
                for (j = i; j < n; j++)
                {
                    for (sum = el[i, j], k = i - 1; k >= 0; k--) sum -= el[i, k] * el[j, k];
                    if (i == j)
                    {
                        if (sum <= 0.0)
                            throw new Exception("Cholesky failed");
                        el[i, i] = Math.Sqrt(sum);
                    }
                    else el[j, i] = sum / el[i, i];
                }
            }
            for (i = 0; i < n; i++) for (j = 0; j < i; j++) el[j, i] = 0.0;
        }

        /// <summary>
        /// Solve the set of n linear equations Ax = b, where a is a positive-definite symmetric matrix
        /// whose Cholesky decomposition has been stored. b[0..n-1] is input as the right-hand side
        /// vector. The solution vector is returned in x[0..n-1].
        /// </summary>
        /// <param name="b">Right-hand-side coefficients.</param>
        /// <param name="x">Solution coefficients.</param>
        public void solve(ref double[,] b, ref double[,] x)
        {
            int i, k;
            double sum;
            if (b.GetLength(0) != n || x.GetLength(0) != n) throw new Exception("bad lengths in Cholesky");
            for (i = 0; i < n; i++)
            {
                for (sum = b[i,0], k = i - 1; k >= 0; k--) sum -= el[i, k] * x[k,0];
                x[i,0] = sum / el[i, i];
            }
            for (i = n - 1; i >= 0; i--)
            {
                for (sum = x[i,0], k = i + 1; k < n; k++) sum -= el[k, i] * x[k,0];
                x[i,0] = sum / el[i, i];
            }
        }

        public void elmult(ref double[,] y, ref double[,] b)
        {
            int i, j;
            if (b.GetLength(0) != n || y.GetLength(0) != n) throw new Exception("bad lengths");
            for (i = 0; i < n; i++)
            {
                b[i,0] = 0.0;
                for (j = 0; j <= i; j++) b[i, 0] += el[i, j] * y[j, 0];
            }
        }

        public void elsolve(ref double[,] b, ref double[,] y)
        {
            int i, j;
            double sum;
            if (b.GetLength(0) != n || y.GetLength(0) != n) throw new Exception("bad lengths");
            for (i = 0; i < n; i++)
            {
                for (sum = b[i,0], j = 0; j < i; j++) sum -= el[i, j] * y[j,0];
                y[i,0] = sum / el[i, i];
            }
        }

        public void inverse(ref double[,] ainv)
        {
            int i, j, k;
            double sum;
            
            //ainv.resize(n, n);
            ainv = new double[n, n];

            for (i = 0; i < n; i++) for (j = 0; j <= i; j++)
                {
                    sum = (i == j ? 1.0 : 0.0);
                    for (k = i - 1; k >= j; k--) sum -= el[i, k] * ainv[j, k];
                    ainv[j, i] = sum / el[i, i];
                }
            for (i = n - 1; i >= 0; i--) for (j = 0; j <= i; j++)
                {
                    sum = (i < j ? 0.0 : ainv[j, i]);
                    for (k = i + 1; k < n; k++) sum -= el[k, i] * ainv[j, k];
                    ainv[i, j] = ainv[j, i] = sum / el[i, i];
                }
        }

        public double logdet()
        {
            double sum = 0.0;
            for (int i = 0; i < n; i++) sum += Math.Log(el[i, i]);
            return 2.0 * sum;
        }
    }
    #endregion
}
