using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsFuncoesMatrizEsparsa
    {
        public clsFuncoesMatrizEsparsa()
        {
        }

        #region Operações com matrizes

        public clsMatrizEsparsa MatrizDotPower(clsMatrizEsparsa a, double p)
        {
            clsMatrizEsparsa b = a.Clone();
            for (int i = 0; i < b.x.GetLength(0); i++)
            {
                b.x[i] = Math.Pow(b.x[i], p);
            }
            return b;
        }

        public double SumMatrixElements(clsMatrizEsparsa a)
        {
            double s = 0.0;
            for (int i = 0; i < a.x.GetLength(0); i++)
            {
                s += a.x[i];
            }
            return s;
        }

        public bool ComparaComMatrizIdentidade(clsMatrizEsparsa a)
        {
            clsMatrizEsparsa id;
            if (a.FormatoMatrizEsparsa == TipoMatrizEsparsa.TripletForm) id = this.Identity(a.m, true);
            else id = this.Identity(a.m, false);

            if (a.nzmax != id.nzmax) return false;
            if (a.m != id.m) return false;
            if (a.n != id.n) return false;
            if (a.x.GetLength(0) != id.x.GetLength(0)) return false;
            if (a.p.GetLength(0) != id.p.GetLength(0)) return false;
            if (a.i.GetLength(0) != id.i.GetLength(0)) return false;
            for (int k = 0; k < a.i.GetLength(0); k++)
            {
                if (a.i[k] != id.i[k]) return false;
            }
            for (int k = 0; k < a.p.GetLength(0); k++)
            {
                if (a.p[k] != id.p[k]) return false;
            }
            for (int k = 0; k < a.x.GetLength(0); k++)
            {
                if (a.x[k] != id.x[k]) return false;
            }

            return true;
        }

        /// <summary>
        /// Calcula o produto inv(a) times x, onde a_in é uma matriz esparsa e x é um matriz full.
        /// </summary>
        public double[,] InvMultX(clsMatrizEsparsa a_in, double[,] x)
        {
            /*---------------------------------------------------------------------------------*/
            /*-- simplifica a matriz a ser invertida e compara com identidade                --*/
            /*---------------------------------------------------------------------------------*/

            clsUtilTools clt = new clsUtilTools();
            clsMatrizEsparsa a = a_in.LimparElementosZero();
            if (ComparaComMatrizIdentidade(a))
            {
                return clt.ArrayDoubleClone(x);
            }

            /*---------------------------------------------------------------------------------*/
            /*-- executa a inversão de matrizes                                              --*/
            /*---------------------------------------------------------------------------------*/

            int ncols = x.GetLength(1);
            double[,] res = new double[x.GetLength(0), x.GetLength(1)];
            double[] aux_b;
            double[] aux_x;

            for (int j = 0; j < ncols; j++)
            {
                aux_b = new double[x.GetLength(0)];
                for (int i = 0; i < x.GetLength(0); i++)
                {
                    aux_b[i] = x[i, j];
                }
                aux_x = this.SolveLinSystem(a, aux_b);
                for (int i = 0; i < x.GetLength(0); i++)
                {
                    res[i, j] = aux_x[i];
                }
            }

            return res;
        }

        /// <summary>
        /// Solução de um sistema de equações lineares, utilizando o método do gradiente conjugado. O algoritmo
        /// foi tirado de numerical recipes, terceira edição.
        /// </summary>
        /// <param name="a">Matriz esparsa no formato compressed-column.</param>
        /// <param name="b">Vetor coluna de right-hand-side coeficientes.</param>
        /// <returns>Retorna um vetor coluna com a solução do sistema.</returns>
        public double[,] SolveLinSystem(clsMatrizEsparsa a, double[,] b)
        {
            double[] bb = new double[b.GetLength(0)];
            for (int i = 0; i < bb.GetLength(0); i++)
            {
                bb[i] = b[i,0];
            }
            double[] r = SolveLinSystem(a, bb);
            double[,] res = new double[r.GetLength(0),1];

            for (int i = 0; i < res.GetLength(0); i++)
            {
                res[i, 0] = r[i];
            }
            return res;
        }

        /// <summary>
        /// Solução de um sistema de equações lineares, utilizando o método do gradiente conjugado. O algoritmo
        /// foi tirado de numerical recipes, terceira edição.
        /// </summary>
        /// <param name="a">Matriz esparsa no formato compressed-column.</param>
        /// <param name="b">Vetor coluna de right-hand-side coeficientes.</param>
        /// <returns>Retorna um vetor coluna com a solução do sistema.</returns>
        public double[] SolveLinSystem(clsMatrizEsparsa a, double[] b)
        {
            if (a.FormatoMatrizEsparsa != TipoMatrizEsparsa.CompressColumn) throw new Exception("Matriz esparsa deve estar no formato compressed-column.");
            if (a.n != a.m) throw new Exception("Matriz esparsa deve ser quadrada.");

            NRsparseMat Cm = new NRsparseMat();
            Cm.ncols = a.n;
            Cm.nrows = a.m;
            Cm.nvals = a.nzmax;
            Cm.row_ind = a.row_indices;
            Cm.val = a.x;
            Cm.col_ptr = a.p;

            double[] x = new double[b.GetLength(0)];
            for (int i = 0; i < x.GetLength(0); i++) x[i] = 1.0;

            int iter = 0;
            double err = 0.0;

            NRsparseLinbcg nrs = new NRsparseLinbcg(Cm);
            nrs.solve(ref b, ref x, 1, 1.0e-4, 100000, ref iter, ref err);

            return x;
        }

        #endregion

        #region utilidades

        public string ImprimeMatrizEsparsaVizinhos(clsMatrizEsparsa T, string[] labels)
        {
            StringBuilder sb = new StringBuilder();

            clsMatrizEsparsa M;
            if (T.FormatoMatrizEsparsa == TipoMatrizEsparsa.TripletForm)
                M = T.Clone();
            else
                M = this.CompressColumn2TripletForm(T);

            object[,] cols = new object[M.nz, 3];
            for (int i = 0; i < cols.GetLength(0); i++)
            {
                cols[i, 0] = labels[M.row_indices[i]];
                cols[i, 1] = labels[M.col_indices[i]];
                cols[i, 2] = M.x[i];
            }
            clsUtilTools clt = new clsUtilTools();
            clt.SortByColumn(ref cols, cols, 0);

            string foco = cols[0, 0].ToString();
            sb.Append("[" + foco + "]\n");
            sb.Append("[");
            for (int i = 0; i < M.nz; i++)
            {
                if (cols[i, 0].ToString() == foco)
                {
                    sb.Append(cols[i, 1].ToString() + ", ");
                }
                else
                {
                    
                }
            }

            return sb.ToString();
        }

        #region transforma em string para impressão e salvamento em arquivo texto do format triplet

        //public string ImprimeMatrizEsparsaTripletForm(clsMatrizEsparsa T, string[] labels, bool ordena_linhas)
        //{
        //    return ImprimeMatrizEsparsaTripletForm(T, labels, ordena_linhas, true);
        //}

        public string ImprimeMatrizEsparsaTripletForm(clsMatrizEsparsa T, string[] labels, bool ordena_linhas, bool inclui_linha_com_nomes_variaveis)
        {
            clsUtilTools clt = new clsUtilTools();

            if (!ordena_linhas)
            {
                StringBuilder sb = new StringBuilder();

                clsMatrizEsparsa M;
                if (T.FormatoMatrizEsparsa == TipoMatrizEsparsa.TripletForm)
                    M = T.Clone();
                else
                    M = this.CompressColumn2TripletForm(T);

                if (inclui_linha_com_nomes_variaveis)
                {
                    sb.Append("Linha" + "\t\t\t" + "Coluna" + "\t\t\t" + "Valor" + "\n");
                }

                for (int i = 0; i < M.nz; i++)
                {
                    sb.Append(labels[M.row_indices[i]] + "\t\t\t " + labels[M.col_indices[i]] + "\t\t\t " + clt.Double2Texto(M.x[i]) + "\n");
                }

                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                clsMatrizEsparsa M;
                if (T.FormatoMatrizEsparsa == TipoMatrizEsparsa.TripletForm)
                    M = T.Clone();
                else
                    M = this.CompressColumn2TripletForm(T);

                object[,] cols = new object[M.nz, 3];
                for (int i = 0; i < cols.GetLength(0); i++)
                {
                    cols[i, 0] = labels[M.row_indices[i]];
                    cols[i, 1] = labels[M.col_indices[i]];
                    cols[i, 2] = M.x[i];
                }
                clt.SortByColumn(ref cols, cols, 0);

                if (inclui_linha_com_nomes_variaveis)
                {
                    sb.Append("Linha" + "\t\t\t" + "Coluna" + "\t\t\t" + "Valor" + "\n");
                }

                //sb.Append("Linha" + "\t\t\t" + "Coluna" + "\t\t\t" + "Valor" + "\n");

                for (int i = 0; i < M.nz; i++)
                {
                    //sb.Append(cols[i, 0].ToString() + "\t\t\t " + cols[i, 1].ToString() + "\t\t\t " + cols[i, 2].ToString() + "\n");
                    sb.Append(cols[i, 0].ToString() + "\t\t\t " + cols[i, 1].ToString() + "\t\t\t " + clt.Double2Texto(cols[i, 2]) + "\n");
                }

                return sb.ToString();
            }
        }

        //public string ImprimeMatrizEsparsaTripletForm(clsMatrizEsparsa T, bool ordena_linhas)
        //{
        //    return ImprimeMatrizEsparsaTripletForm(T, ordena_linhas, true);
        //}

        public string ImprimeMatrizEsparsaTripletForm(clsMatrizEsparsa T, bool ordena_linhas, bool inclui_linha_com_nomes_variaveis)
        {
            if (!ordena_linhas)
            {
                return ImprimeMatrizEsparsaTripletForm(T, inclui_linha_com_nomes_variaveis);
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                clsMatrizEsparsa M;
                if (T.FormatoMatrizEsparsa == TipoMatrizEsparsa.TripletForm)
                    M = T.Clone();
                else
                    M = this.CompressColumn2TripletForm(T);

                object[,] cols = new object[M.nz, 3];
                for (int i = 0; i < cols.GetLength(0); i++)
                {
                    cols[i,0] = M.row_indices[i];
                    cols[i,1] = M.col_indices[i];
                    cols[i,2] = M.x[i];
                }
                clsUtilTools clt = new clsUtilTools();
                clt.SortByColumn(ref cols, cols, 0);

                if (inclui_linha_com_nomes_variaveis)
                {
                    sb.Append("Linha" + "\t\t\t" + "Coluna" + "\t\t\t" + "Valor" + "\n");
                }

                //sb.Append("Linha" + "\t\t\t" + "Coluna" + "\t\t\t" + "Valor" + "\n");

                for (int i = 0; i < M.nz; i++)
                {
                    //sb.Append(cols[i,0].ToString() + "\t\t\t " + cols[i, 1].ToString() + "\t\t\t " + cols[i, 2].ToString() + "\n");
                    sb.Append(cols[i, 0].ToString() + "\t\t\t " + cols[i, 1].ToString() + "\t\t\t " + clt.Double2Texto(cols[i, 2]) + "\n");
                }

                return sb.ToString();
            }
        }

        //public string ImprimeMatrizEsparsaTripletForm(clsMatrizEsparsa T)
        //{
        //    return ImprimeMatrizEsparsaTripletForm(T, true);
        //}

        public string ImprimeMatrizEsparsaTripletForm(clsMatrizEsparsa T, bool inclui_linha_com_nomes_variaveis)
        {
            clsUtilTools clt = new clsUtilTools();

            StringBuilder sb = new StringBuilder();

            clsMatrizEsparsa M;
            if (T.FormatoMatrizEsparsa == TipoMatrizEsparsa.TripletForm)
                M = T.Clone();
            else
                M = this.CompressColumn2TripletForm(T);

            //sb.Append("Linha" + "\t\t\t" + "Coluna" + "\t\t\t" + "Valor" + "\n");

            if (inclui_linha_com_nomes_variaveis)
            {
                sb.Append("Linha" + "\t\t\t" + "Coluna" + "\t\t\t" + "Valor" + "\n");
            }

            for (int i = 0; i < M.nz; i++)
            {
                //sb.Append(M.row_indices[i].ToString() + "\t\t\t " + M.col_indices[i].ToString() + "\t\t\t " + M.x[i].ToString() + "\n");
                sb.Append(M.row_indices[i].ToString() + "\t\t\t " + M.col_indices[i].ToString() + "\t\t\t " + clt.Double2Texto(M.x[i]) + "\n");
            }

            return sb.ToString();
        }

        #endregion

        #region conversão entre formatos de matrizes esparsas

        public clsMatrizEsparsa CompressColumn2TripletForm(clsMatrizEsparsa T)
        {
            if (T.FormatoMatrizEsparsa == TipoMatrizEsparsa.TripletForm) return T.Clone();

            clsMatrizEsparsa res = T.Clone();

            res.nz = T.x.GetLength(0);
            res.nzmax = T.x.GetLength(0);
            int[] aux_p = new int[res.nz];
            int novo_comeco = 0;
            int indice = 0;
            for (int i = 1; i < T.col_indices.GetLength(0); i++)
            {
                for (int j = novo_comeco; j < T.col_indices[i]; j++)
                {
                    aux_p[indice] = i - 1;
                    indice++;
                }
                novo_comeco = T.col_indices[i];
            }
            res.col_indices = aux_p;

            return res;
        }

        public clsMatrizEsparsa TripletForm2CompressColumn(clsMatrizEsparsa T)
        {
            if (T.FormatoMatrizEsparsa == TipoMatrizEsparsa.CompressColumn) return T.Clone();

            int m, n, nz, p, k;
            int[] Cp, Ci, w, Ti, Tj ;
            double[] Cx, Tx;
            clsMatrizEsparsa C;

            m = T.m;
            n = T.n;
            Ti = T.i;
            Tj = T.p; 
            Tx = T.x; 
            nz = T.nz;

            C = new clsMatrizEsparsa(m, n, nz, Tx, false);

            w = new int[n];

            Cp = C.p; 
            Ci = C.i; 
            Cx = C.x;

            for (k = 0 ; k < nz ; k++) w [Tj [k]]++ ;           /* column counts */
            cs_cumsum(ref Cp, ref w, n) ;                       /* column pointers */
            for (k = 0 ; k < nz ; k++)
            {
                Ci [p = w [Tj [k]]++] = Ti [k] ;    /* A(i,j) is the pth entry in C */
                Cx [p] = Tx [k] ;
            }

            C.p = Cp;
            C.i = Ci;
            C.x = Cx;

            return C;
        }

        #endregion

        #endregion

        #region Criação de uma matriz identidade 

        public clsMatrizEsparsa Identity(int n)
        {
            return Identity(n, false);
        }

        public clsMatrizEsparsa Identity(int n, bool triplet_format)
        {
            clsMatrizEsparsa res = new clsMatrizEsparsa(n, n, n, true);

            res.x = new double[n];
            res.p = new int[n];
            res.i = new int[n];

            for (int i = 0; i < n; i++)
            {
                res.x[i] = 1.0;
                res.p[i] = i;
                res.i[i] = i;
            }

            if (triplet_format) return res;
            else return this.TripletForm2CompressColumn(res);
        }

        #endregion

        #region multiplicação esparsa por vetor double
        
        /// <summary>
        /// Função para calcular a expressão: A*x. 
        /// </summary>
        /// <param name="A">Matriz esparsa no formato de compress-column.</param>
        /// <param name="x">Vetor de doubles.</param>
        /// <returns>Retorna um vetor de doubles.</returns>
        public double[,] MatrizMult(clsMatrizEsparsa A, double[,] x)
        {
            double[,] res = new double[x.GetLength(0), x.GetLength(1)];                     
            double[] y = new double[x.GetLength(0)];

            for (int j = 0; j < x.GetLength(1); j++)
            {
                for (int i = 0; i < y.GetLength(0); i++)
                {
                    y[i] = x[i, j];
                }
                y = MatrizMult(A, y);
                for (int i = 0; i < res.GetLength(0); i++)
                {
                    res[i, j] = y[i];
                }
            }

            return res;
        }

        /// <summary>
        /// Função para calcular a expressão: A*x. 
        /// </summary>
        /// <param name="A">Matriz esparsa no formato de compress-column.</param>
        /// <param name="x">Vetor de doubles.</param>
        /// <returns>Retorna um vetor de doubles.</returns>
        public double[] MatrizMult(clsMatrizEsparsa A, double[] x)
        {
            double[] y = new double[x.GetLength(0)];

            if (A.FormatoMatrizEsparsa != TipoMatrizEsparsa.CompressColumn) throw new Exception("Matriz esparsa deve estar no formato compress-column.");
            if (A.n != x.GetLength(0) || A.n != y.GetLength(0)) throw new Exception("Dimensões das matrizes e vetores não estão adequadas para a operação.");

            int p, j, n;
            int[] Ap, Ai;
            double[] Ax;

            n = A.n;
            Ap = A.p;
            Ai = A.i;
            Ax = A.x;

            for (j = 0; j < n; j++)
            {
                for (p = Ap[j]; p < Ap[j + 1]; p++)
                {
                    y[Ai[p]] += Ax[p] * x[j];
                }
            }

            return y;
        }
        #endregion

        #region transposta de uma matriz esparsa
        /// <summary>
        /// Retorna a transporta de uma matriz esparsa.
        /// </summary>
        /// <param name="A">Matriz esparsa (em qualquer formato).</param>
        /// <returns>Retorna uma matriz esparsa (no formato da matriz orginal).</returns>
        public clsMatrizEsparsa MatrizTransp(clsMatrizEsparsa A)
        {
            if (A.FormatoMatrizEsparsa == TipoMatrizEsparsa.TripletForm)
            {
                clsMatrizEsparsa res = new clsMatrizEsparsa();
                res = A.Clone();
                res.col_indices = A.row_indices;
                res.row_indices = A.col_indices;
                res.m = A.n;
                res.n = A.m;
                return res;
            }
            else
            {
                int p, q, j;
                int[] Cp, Ci;
                int n, m;
                int[] Ap, Ai, w;
                double[] Cx, Ax;
                clsMatrizEsparsa C;

                m = A.m; 
                n = A.n; 
                Ap = A.p; 
                Ai = A.i; 
                Ax = A.x;

                C = new clsMatrizEsparsa(n, m, Ap[n], Ax, false);
                w = new int[m];
                
                Cp = C.p; 
                Ci = C.i; 
                Cx = C.x;

                for (p = 0 ; p < Ap [n] ; p++) w [Ai [p]]++ ;                  /* row counts */
                cs_cumsum (ref Cp, ref w, m) ;                                 /* row pointers */
                for (j = 0 ; j < n ; j++)
                {
                    for (p = Ap [j] ; p < Ap [j+1] ; p++)
                    {
                        Ci [q = w [Ai [p]]++] = j ; /* place A(i,j) as entry C(j,i) */
                        Cx [q] = Ax [p] ;
                    }
                }

                C.x = Cx;
                C.i = Ci;
                C.p = Cp;

                return C;
            }
        }
        #endregion

        #region multiplicação de duas matrizes esparsas

        /* C = A*B */
        private clsMatrizEsparsa cs_multiply (clsMatrizEsparsa A, clsMatrizEsparsa B)
        {
            int p, j, nz = 0, anz;
            int[] Cp, Ci, Bp;
            int m, n, bnz;
            int[] w;
            int values;
            int[] Bi;

            double[] x, Bx, Cx ;
            
            clsMatrizEsparsa C;
            
            //cs *C ;
            //if (!CS_CSC (A) || !CS_CSC (B)) return (NULL) ;      /* check inputs */
            if (A.n != B.m) throw new Exception("Cheque dimensões das matrizes.");
            
            m = A.m ; anz = A.p [A.n] ;
            n = B.n ; Bp = B.p ; Bi = B.i ; Bx = B.x ; bnz = Bp [n] ;

            //w = cs_calloc (m, sizeof (int)) ;                    /* get workspace */
            w = new int[m];

            values = Convert.ToInt32((A.x != null) && (Bx != null)) ;
            
            //x = values ? cs_malloc (m, sizeof (double)) : NULL ; /* get workspace */

            x = values != 0 ? new double[m] : null;

            //C = cs_spalloc (m, n, anz + bnz, values, 0) ;        /* allocate result */

            C = new clsMatrizEsparsa(m, n, anz + bnz, false);

            //if (!C || !w || (values && !x)) return (cs_done (C, w, x, 0)) ;
            
            Cp = C.p ;

            for (j = 0 ; j < n ; j++)
            {
                if (nz + m > C.nzmax && (cs_sprealloc (ref C, 2*(C.nzmax)+m) == 0))
                {
                    return C; 
                    //return (cs_done (C, w, x, 0)) ;             /* out of memory */
                } 

                Ci = C.i ; Cx = C.x ;          /* C->i and C->x may be reallocated */
                Cp [j] = nz ;                   /* column j of C starts here */
                for (p = Bp [j] ; p < Bp [j+1] ; p++)
                {
                    nz = cs_scatter (A, Bi [p], Bx != null ? Bx [p] : 1, w, x, j+1, ref C, nz) ;
                }
                if (values != 0) for (p = Cp [j] ; p < nz ; p++) Cx [p] = x [Ci [p]] ;
            }

            Cp [n] = nz ;                       /* finalize the last column of C */

            cs_sprealloc(ref C, 0);
            return C;

            //cs_sprealloc (C, 0) ;               /* remove extra space from C */            
            //return (cs_done (C, w, x, 1)) ;     /* success; free workspace, return C */
        }

        /// <summary>
        /// Multiplicação de duas matrizes esparsas (ambas na forma de compress-column).
        /// </summary>
        public clsMatrizEsparsa MatrizMult(clsMatrizEsparsa A, clsMatrizEsparsa B)
        {
            return cs_multiply(A, B);

            //int p, j, nz = 0, anz;
            //int[] Cp, Ci, Bp, Bi, w;
            //int m, n, bnz;

            //double[] x, Bx, Cx;
            //clsMatrizEsparsa C;

            //if (!CS_CSC(A) || !CS_CSC(B)) throw new Exception("Matrizes esparsas devem estar no formato compress-column.");
            //if (A.n != B.m) throw new Exception("Dimensões das matrizes esparsas não estão de acordo para multiplicação.");

            //m = A.m;
            //anz = A.p[A.n];
            //n = B.n;
            //Bp = B.p;
            //Bi = B.i;
            //Bx = B.x;
            //bnz = Bp[n];
            //w = new int[m];                  

            //x = new double[m];

            //C = new clsMatrizEsparsa(m, n, anz + bnz, false);
            //Ci = C.i;
            //Cx = C.x;

            //Cp = C.p;
            //for (j = 0; j < n; j++)
            //{
            //    Ci = C.i;
            //    Cx = C.x;                  /* C->i and C->x may be reallocated */
            //    Cp[j] = nz;                /* column j of C starts here */
            //    for (p = Bp[j]; p < Bp[j + 1]; p++)
            //    {
            //        nz = cs_scatter(A, Bi[p], Bx[p], w, x, j + 1, ref C, nz);
            //    }
            //    for (p = Cp[j]; p < nz; p++) Cx[p] = x[Ci[p]];
            //}

            //Cp[n] = nz;                           /* finalize the last column of C */

            //C.p = Cp;
            //C.i = Ci;
            //C.x = Cx;
            //C.nz = -1;
            //C.nzmax = nz;
            //this.cs_sprealloc(ref C, 0);

            //return C;
        }

        #endregion

        #region soma de duas matrizes esparsas

        /// <summary>
        /// Calcula a equação: C = alpha*A + beta*B.
        /// </summary>
        /// <param name="A">Matriz esparsa (em formato compressed-column).</param>
        /// <param name="B">Matriz esparsa (em formato compressed-column).</param>
        /// <param name="alpha">Escalar (double).</param>
        /// <param name="beta">Escalar (double).</param>
        /// <returns>Retorna uma matriz esparsa em formato compressed-column.</returns>
        public clsMatrizEsparsa MatrizSoma(clsMatrizEsparsa A, clsMatrizEsparsa B, double alpha, double beta)
        {
            return this.cs_add(A, B, alpha, beta);
        }
        
        /// <summary>
        /// Calcula a equação: C = alpha*A + beta*B.
        /// </summary>
        /// <param name="A">Matriz esparsa (em formato compressed-column).</param>
        /// <param name="B">Matriz esparsa (em formato compressed-column).</param>
        /// <param name="alpha">Escalar (double).</param>
        /// <param name="beta">Escalar (double).</param>
        /// <returns>Retorna uma matriz esparsa em formato compressed-column.</returns>
        private clsMatrizEsparsa cs_add(clsMatrizEsparsa A, clsMatrizEsparsa B, double alpha, double beta)
        {
            int p, j, nz = 0, anz;
            int[] Cp, Ci, Bp, w;
            int m, n, bnz, values;
            double[] x, Bx, Cx;

            clsMatrizEsparsa C;
            if (!CS_CSC(A) || !CS_CSC(B)) throw new Exception("Ambas as matrizes esparsas precisam estar em formato compressed-column.");
            if (A.m != B.m || A.n != B.n) throw new Exception("Dimensões não estão corretas para a soma de matrizes esparsas.");

            m = A.m;
            anz = A.p[A.n];
            n = B.n;
            Bp = B.p;
            Bx = B.x;
            bnz = Bp[n];
            w = new int[m];
            x = new double[m];
            C = new clsMatrizEsparsa(m, n, anz + bnz, false);

            Cp = C.p;
            Ci = C.i;
            Cx = C.x;

            for (j = 0; j < n; j++)
            {
                Cp[j] = nz;                                         /* column j of C starts here */

                nz = cs_scatter(A, j, alpha, w, x, j + 1, ref C, nz);   /* alpha*A(:,j)*/
                nz = cs_scatter(B, j, beta, w, x, j + 1, ref C, nz);    /* beta*B(:,j) */

                for (p = Cp[j]; p < nz; p++) Cx[p] = x[Ci[p]];
            }

            Cp[n] = nz;                         /* finalize the last column of C */

            C.p = Cp;
            C.i = Ci;
            C.x = Cx;

            cs_sprealloc(ref C, 0);               /* remove extra space from C */

            return C;
        }

        #endregion

        #region solving triangular systems (chapter 3)

        /// <summary>
        /// Solve Gx=b(:,k), where G is either upper (lo=0) or lower (lo=1) triangular. Para resolver o sistema linear, 
        /// utilize pinv = null.
        /// </summary>
        public int cs_spsolve (clsMatrizEsparsa G, clsMatrizEsparsa B, int k, ref int[] xi, ref double[] x, int[] pinv, int lo)
        {
            int j, J, p, q, px, top, n;
            int[] Gp, Gi, Bp, Bi;
            double[] Gx, Bx;

            if (!CS_CSC(G) || !CS_CSC(B)) throw new Exception("Matrizes esparsas devem estar no formato compressed-column.");

            Gp = G.p; 
            Gi = G.i; 
            Gx = G.x; 
            n = G.n;

            Bp = B.p; 
            Bi = B.i; 
            Bx = B.x;

            top = cs_reach (G, B, k, ref xi, pinv) ;        /* xi[top..n-1]=Reach(B(:,k)) */

            for (p = top ; p < n ; p++) x [xi [p]] = 0 ;    /* clear x */
            for (p = Bp [k] ; p < Bp [k+1] ; p++) x [Bi [p]] = Bx [p] ; /* scatter B */
            for (px = top ; px < n ; px++)
            {
                j = xi [px] ;                               /* x(j) is nonzero */
                J = pinv != null ? (pinv [j]) : j ;                 /* j maps to col J of G */
                if (J < 0) continue ;                       /* column J is empty */
                x [j] /= Gx [lo > 0 ? (Gp [J]) : (Gp [J+1]-1)] ;/* x(j) /= G(j,j) */
                p = lo != 0 ? (Gp [J]+1) : (Gp [J]) ;            /* lo: L(j,j) 1st entry */
                q = lo != 0 ? (Gp [J+1]) : (Gp [J+1]-1) ;        /* up: U(j,j) last entry */
                for ( ; p < q ; p++)
                {
                    x [Gi [p]] -= Gx [p] * x [j] ;          /* x(i) -= G(i,j) * x(j) */
                }
            }
            return (top) ;                                  /* return top of stack */
        }

        private int cs_dfs (int j, clsMatrizEsparsa G, int top, ref int[] xi, ref int[] pstack, int[] pinv)
        {
            int i, p, p2, done, jnew, head = 0;
            int[] Gp, Gi;

            if (!CS_CSC(G)) throw new Exception("Matriz esparsa deve estar no formato compressed-column.");

            Gp = G.p; 
            Gi = G.i;
            xi [0] = j;                     /* initialize the recursion stack */

            while (head >= 0)
            {
                j = xi [head] ;             /* get j from the top of the recursion stack */
                jnew = pinv != null ? (pinv [j]) : j ;

                if (!CS_MARKED (Gp, j))
                {
                    CS_MARK (ref Gp, j) ;       /* mark node j as visited */
                    pstack [head] = (jnew < 0) ? 0 : CS_UNFLIP (Gp [jnew]) ;
                }
                done = 1 ;                  /* node j done if no unvisited neighbors */

                //p2 = (jnew < 0) ? 0 : CS_UNFLIP (Gp [jnew+1]) ;
                if (jnew < 0)
                {
                    p2 = 0;
                }
                else
                {
                    p2 = CS_UNFLIP(Gp[jnew + 1]);
                }
                
                for (p = pstack [head] ; p < p2 ; p++)  /* examine all neighbors of j */
                {
                    i = Gi [p] ;            /* consider neighbor node i */
                    if (CS_MARKED (Gp, i)) continue ;   /* skip visited node i */
                    pstack [head] = p ;     /* pause depth-first search of node j */
                    xi [++head] = i ;       /* start dfs at node i */
                    done = 0 ;              /* node j is not done */
                    break ;                 /* break, to start dfs (i) */
                }
                if (done != 0)               /* depth-first search at node j is done */
                {
                    head-- ;            /* remove j from the recursion stack */
                    xi [--top] = j ;    /* and place in the output stack */
                }
            }

            G.p = Gp;
            G.i = Gi;

            return (top) ;
        }

        private int cs_reach (clsMatrizEsparsa G, clsMatrizEsparsa B, int k, ref int[] xi, int[] pinv)
        {
            int p, n, top;
            int[] Bp, Bi, Gp;

            if (!CS_CSC (G) || !CS_CSC (B)) throw new Exception ("Todas as matrizes esparsas devem estar no formato compressed-column.");
            
            n = G.n; 
            Bp = B.p; 
            Bi = B.i; 
            Gp = G.p;
            top = n;

            //------------ parte adicionada por Alexandre para resolver o problema de ponteiros para subarrays --------//
            int[] pstack = new int[n];
            for (int i = n; i < xi.GetLength(0); i++)
            {
                pstack[i - n] = xi[i];
            }

            for (p = Bp[k]; p < Bp[k + 1]; p++)
            {
                if (!CS_MARKED(Gp, Bi[p]))    /* start a dfs at unmarked node i */
                {
                    //top = cs_dfs(Bi[p], G, top, xi, xi + n, pinv);
                    top = cs_dfs(Bi[p], G, top, ref xi, ref pstack, pinv);
                }
            }
            for (p = top ; p < n ; p++) CS_MARK (ref Gp, xi [p]) ;  /* restore G */

            G.p = Gp;

            //------------ parte adicionada por Alexandre para resolver o problema de ponteiros para subarrays --------//
            int[] aux_xi = new int[2 * n];
            for (int i = 0; i < n; i++) aux_xi[i] = xi[i];
            for (int i = 0; i < n; i++) aux_xi[i + n] = pstack[i];
            xi = aux_xi;

            return (top);
        }

        #endregion

        #region traço e produto diagonal principal de uma matriz esparsa

        public double soma_log_diagonal(clsMatrizEsparsa A)
        {
            if (A.n != A.m) return double.NaN;
            double r = 0.0;
            int n_prods = 0;
            int inicio;
            int fim;
            if (CS_CSC(A))
            {
                for (int k = 0; k < A.p.GetLength(0) - 1; k++)
                {
                    inicio = A.p[k];
                    fim = A.p[k + 1];
                    for (int j = inicio; j < fim; j++)
                    {
                        if (A.i[j] == k)
                        {
                            if (A.x[j] != 0.0)
                            {
                                r += Math.Log(A.x[j]);
                                n_prods++;
                            }
                            else return double.NaN;
                        }
                    }
                }
            }
            else
            {
                for (int k = 0; k < A.x.GetLength(0); k++)
                {
                    if (A.i[k] == A.p[k])
                    {
                        if (A.x[k] != 0.0)
                        {
                            r += Math.Log(A.x[k]);
                            n_prods++;
                        }
                        else return double.NaN;
                    }
                }
            }
            if (n_prods < A.n) return double.NaN;
            return r;
        }

        public double prod_diagonal(clsMatrizEsparsa A)
        {
            if (A.n != A.m) return double.NaN;
            double r = 1.0;
            int n_prods = 0;
            int inicio;
            int fim;
            if (CS_CSC(A))
            {
                for (int k = 0; k < A.p.GetLength(0) - 1; k++)
                {
                    inicio = A.p[k];
                    fim = A.p[k + 1];
                    for (int j = inicio; j < fim; j++)
                    {
                        if (A.i[j] == k)
                        {
                            r *= A.x[j];
                            n_prods++;
                        }
                    }
                }
            }
            else
            {
                for (int k = 0; k < A.x.GetLength(0); k++)
                {
                    if (A.i[k] == A.p[k])
                    {
                        r *= A.x[k];
                        n_prods++;
                    }
                }
            }
            if (n_prods < A.n) return 0.0;
            return r;
        }

        public double traco(clsMatrizEsparsa A)
        {
            if (A.n != A.m) return double.NaN;
            double r = 0.0;
            int inicio;
            int fim;
            if (CS_CSC(A))
            {
                for (int k = 0; k < A.p.GetLength(0) - 1; k++)
                {
                    inicio = A.p[k];
                    fim = A.p[k + 1];
                    for (int j = inicio; j < fim; j++)
                    {
                        if (A.i[j] == k)
                        {
                            r += A.x[j];
                        }
                    }
                }
            }
            else
            {
                for (int k = 0; k < A.x.GetLength(0); k++)
                {
                    if (A.i[k] == A.p[k])
                    {
                        r += A.x[k];
                    }
                }
            }
            return r;
        }

        #endregion

        #region determinante matriz esparsa usando LU factorization

        public double LogDet(clsMatrizEsparsa C)
        {
            if (C.n != C.m) return double.NaN;

            double tol = 1.0e-4;
            clsSparseSymbolicFactorization S = new clsSparseSymbolicFactorization();
            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();

            S.unz = Math.Max(Math.Min(200, C.n * C.m), (int)Math.Min(10 * C.nzmax, C.n * C.m));
            S.lnz = Math.Max(Math.Min(200, C.n * C.m), (int)Math.Min(10 * C.nzmax, C.n * C.m));

            clsSparseNumericFactorization N = fme.cs_lu(C, S, tol);

            return this.soma_log_diagonal(N.U);
        }

        public double Det(clsMatrizEsparsa C)
        {
            if (C.n != C.m) return double.NaN;

            double tol = 1.0e-4;
            clsSparseSymbolicFactorization S = new clsSparseSymbolicFactorization();
            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();

            S.unz = Math.Max(Math.Min(200, C.n * C.m), (int)Math.Min(10 * C.nzmax, C.n * C.m));
            S.lnz = Math.Max(Math.Min(200, C.n * C.m), (int)Math.Min(10 * C.nzmax, C.n * C.m));

            clsSparseNumericFactorization N = fme.cs_lu(C, S, tol);

            return this.prod_diagonal(N.U);
        }

        #endregion

        #region LU factorization (chapter 6)

        /* [L,U,pinv]=lu(A, [q lnz unz]). lnz and unz can be guess */
        public clsSparseNumericFactorization cs_lu (clsMatrizEsparsa A, clsSparseSymbolicFactorization S, double tol)
        {
            clsUtilTools clt = new clsUtilTools();

            clsMatrizEsparsa L, U;
            clsSparseNumericFactorization N;

            double pivot;
            double[] Lx, Ux, x;
            double a, t;

            int[] Lp, Li, Up, Ui, pinv, xi, q; 
            int n, ipiv, k, top, p, i, col, lnz,unz;

            if (!CS_CSC(A)) throw new Exception("Matriz esparsa tem que estar no formato compressed-column.");

            n = A.n;
            q = S.q; 
            lnz = (int)S.lnz; 
            unz = (int)S.unz;

            //x = cs_malloc (n, sizeof (double)) ;            /* get double workspace */
            //xi = cs_malloc (2*n, sizeof (int)) ;            /* get int workspace */
            //N = cs_calloc (1, sizeof (csn)) ;               /* allocate result */

            x = new double[n];
            xi = new int[2 * n];
            N = new clsSparseNumericFactorization();

            //if (!x || !xi || !N) return (cs_ndone (N, NULL, xi, x, 0)) ;

            //N->L = L = cs_spalloc (n, n, lnz, 1, 0) ;       /* allocate result L */
            //N->U = U = cs_spalloc (n, n, unz, 1, 0) ;       /* allocate result U */
            //N->pinv = pinv = cs_malloc (n, sizeof (int)) ;  /* allocate result pinv */

            N.L = L = new clsMatrizEsparsa(n, n, lnz, false);
            N.U = U = new clsMatrizEsparsa(n, n, unz, false);
            N.pinv = pinv = new int[n];
            
            //if (!L || !U || !pinv) return (cs_ndone (N, NULL, xi, x, 0)) ;

            Lp = L.p; 
            Up = U.p;

            for (i = 0 ; i < n ; i++) x [i] = 0 ;           /* clear workspace */
            for (i = 0 ; i < n ; i++) pinv [i] = -1 ;       /* no rows pivotal yet */
            for (k = 0 ; k <= n ; k++) Lp [k] = 0 ;         /* no cols of L yet */
            lnz = unz = 0 ;

            for (k = 0 ; k < n ; k++)       /* compute L(:,k) and U(:,k) */
            {
                /* --- Triangular solve --------------------------------------------- */
                Lp [k] = lnz ;              /* L(:,k) starts here */
                Up [k] = unz ;              /* U(:,k) starts here */

                //if ((lnz + n > L->nzmax && !cs_sprealloc (L, 2*L->nzmax + n)) ||
                //    (unz + n > U->nzmax && !cs_sprealloc (U, 2*U->nzmax + n)))
                //{
                //    return (cs_ndone (N, NULL, xi, x, 0)) ;
                //}

                Li = L.i; 
                Lx = L.x; 
                Ui = U.i; 
                Ux = U.x;

                //col = q ? (q [k]) : k ;
                col = (q != null && q.GetLength(0) > 0) ? (q[k]) : k;

                top = cs_spsolve (L, A, col, ref xi, ref x, pinv, 1) ;  /* x = L\A(:,col) */

                /* --- Find pivot --------------------------------------------------- */
                ipiv = -1 ;
                a = -1 ;
                for (p = top ; p < n ; p++)
                {
                    i = xi [p] ;            /* x(i) is nonzero */
                    if (pinv [i] < 0)       /* row i is not yet pivotal */
                    {
                        if ((t = Math.Abs (x [i])) > a)
                        {
                            a = t ;         /* largest pivot candidate so far */
                            ipiv = i ;
                        }
                    }
                    else                    /* x(i) is the entry U(pinv[i],k) */
                    {
                        // aumenta tamanho se necessário
                        if (unz == Ui.GetLength(0))
                        {
                            U.i = clt.DobraTamanhoVetor(Ui);
                            U.x = clt.DobraTamanhoVetor(Ux);
                            U.nzmax = U.i.GetLength(0);
                            Ui = U.i;
                            Ux = U.x;
                        }

                        Ui [unz] = pinv [i] ;
                        Ux [unz++] = x [i] ;
                    }
                }

                //if (ipiv == -1 || a <= 0) return (cs_ndone (N, NULL, xi, x, 0)) ;
                
                if (pinv [col] < 0 && Math.Abs (x [col]) >= a*tol) ipiv = col ;

                /* --- Divide by pivot ---------------------------------------------- */
                pivot = x[ipiv];          /* the chosen pivot */
                
                // aumenta tamanho se necessário
                if (unz == Ui.GetLength(0))
                {
                    U.i = clt.DobraTamanhoVetor(Ui);
                    U.x = clt.DobraTamanhoVetor(Ux);
                    U.nzmax = U.i.GetLength(0);
                    Ui = U.i;
                    Ux = U.x;
                }

                Ui [unz] = k ;              /* last entry in U(:,k) is U(k,k) */
                Ux [unz++] = pivot ;
                pinv [ipiv] = k ;           /* ipiv is the kth pivot row */

                // aumenta tamanho se necessário
                if (lnz == Li.GetLength(0))
                {
                    L.i = clt.DobraTamanhoVetor(Li);
                    L.x = clt.DobraTamanhoVetor(Lx);
                    L.nzmax = L.i.GetLength(0);
                    Li = L.i;
                    Lx = L.x;
                }

                Li [lnz] = ipiv ;           /* first entry in L(:,k) is L(k,k) = 1 */
                Lx [lnz++] = 1 ;
                for (p = top ; p < n ; p++) /* L(k+1:n,k) = x / pivot */
                {
                    i = xi [p] ;
                    if (pinv [i] < 0)       /* x(i) is an entry in L(:,k) */
                    {
                        // aumenta tamanho se necessário
                        if (lnz == Li.GetLength(0))
                        {
                            L.i = clt.DobraTamanhoVetor(Li);
                            L.x = clt.DobraTamanhoVetor(Lx);
                            L.nzmax = L.i.GetLength(0);
                            Li = L.i;
                            Lx = L.x;
                        }

                        Li [lnz] = i ;      /* save unpermuted row in L */
                        Lx [lnz++] = x [i] / pivot ;    /* scale pivot column */
                    }
                    x [i] = 0 ;             /* x [0..n-1] = 0 for next k */
                }
            }
            /* --- Finalize L and U ------------------------------------------------- */
            Lp [n] = lnz ;
            Up [n] = unz ;
            Li = L.i ;                     /* fix row indices of L for final pinv */
            for (p = 0 ; p < lnz ; p++) Li [p] = pinv [Li [p]] ;

            cs_sprealloc (ref L, 0) ;           /* remove extra space from L and U */
            cs_sprealloc (ref U, 0) ;
            
            //return (cs_ndone (N, NULL, xi, x, 1)) ;     /* success */

            return N;
        }

        #endregion

        #region funções auxiliares da biblioteca de matrizes esparsas

        private int CS_FLIP(int i) { return -i - 2; }
        private int CS_UNFLIP(int i) { return (i < 0) ? CS_FLIP(i) : i; }
        private bool CS_MARKED(int[] w, int j) { return w[j] < 0; }
        private void CS_MARK(ref int[] w, int j) { w[j] = CS_FLIP(w[j]); }
        private bool CS_CSC(clsMatrizEsparsa A)
        {
            return (A.FormatoMatrizEsparsa == TipoMatrizEsparsa.CompressColumn);
        }

        /* C = A(p,q) where p and q are permutations of 0..m-1 and 0..n-1. */
        private clsMatrizEsparsa cs_permute (clsMatrizEsparsa A, int[] pinv, int[] q, int values)
        {
            int t, j, k, nz = 0, m, n;
            int[] Ap, Ai, Cp, Ci ;
            double[] Cx, Ax;
            clsMatrizEsparsa C ;

            m = A.m ; 
            n = A.n ; 
            Ap = A.p ; 
            Ai = A.i ; 
            Ax = A.x ;

            C = new clsMatrizEsparsa(m, n, Ap[n], Ax, false);

            Cp = C.p ; 
            Ci = C.i ; 
            Cx = C.x ;

            for (k = 0 ; k < n ; k++)
            {
                Cp [k] = nz ;                   /* column k of C is column q[k] of A */
                j = q != null ? (q [k]) : k ;
                for (t = Ap [j] ; t < Ap [j+1] ; t++)
                {
                    if (Cx != null) Cx [nz] = Ax [t] ;  /* row i of A is row pinv[i] of C */
                    Ci [nz++] = pinv != null ? (pinv [Ai [t]]) : Ai [t] ;
                }
            }
            Cp [n] = nz ;                       /* finalize the last column of C */

            C.p = Cp;
            C.x = Cx;
            C.i = Ci;

            return C;
        }

        /* pinv = p', or p = pinv' */
        private int[] cs_pinv (int[] p, int n)
        {
            int k;
            int[] pinv = new int[n];

            for (k = 0 ; k < n ; k++) pinv [p [k]] = k ;/* invert the permutation */
            return (pinv) ;                             /* return result */
        }

        private double cs_cumsum(ref int[] p, ref int[] c, int n)
        {
            int i, nz = 0;
            double nz2 = 0;
            for (i = 0; i < n; i++)
            {
                p[i] = nz;
                nz += c[i];
                nz2 += c[i];              /* also in double to avoid int overflow */
                c[i] = p[i];             /* also copy p[0..n-1] back into c[0..n-1]*/
            }
            p[n] = nz;
            return (nz2);                  /* return sum (c [0..n-1]) */
        }

        /// <summary>
        /// x = x + beta * A(:,j), where x is a dense vector and A(:,j) is sparse 
        /// </summary>
        private int cs_scatter (clsMatrizEsparsa A, int j, double beta, int[] w, double[] x, int mark, ref clsMatrizEsparsa C, int nz)
        {
            int i, p;
            int[] Ap, Ai, Ci;
            double[] Ax;

            //if (!CS_CSC (A) || !w || !CS_CSC (C)) return (-1) ;     /* check inputs */
            
            Ap = A.p ; Ai = A.i ; Ax = A.x ; Ci = C.i ;
            for (p = Ap [j] ; p < Ap [j+1] ; p++)
            {
                i = Ai [p] ;                            /* A(i,j) is nonzero */
                if (w [i] < mark)
                {
                    w [i] = mark ;                      /* i is new entry in column j */
                    Ci [nz++] = i ;                     /* add i to pattern of C(:,j) */
                    if (x != null) x [i] = beta * Ax [p] ;      /* x(i) = beta*A(i,j) */
                }
                else if (x != null) x [i] += beta * Ax [p] ;    /* i exists in C(:,j) already */
            }
            return (nz) ;
        }


        private int cs_sprealloc(ref clsMatrizEsparsa A, int nzmax)
        {
            if (nzmax <= 0) nzmax = (CS_CSC(A)) ? A.p[A.n] : A.nz;

            //if (nzmax < A.x.GetLength(0)) nzmax = A.x.GetLength(0);

            int[] inew = new int[nzmax];
            double[] xnew = new double[nzmax];

            for (int i = 0; i < Math.Min(A.x.GetLength(0), nzmax); i++)
            {
                inew[i] = A.i[i];
                xnew[i] = A.x[i];
            }

            A.i = inew;
            A.x = xnew;

            return 1;
        }

        #endregion

        #region Dulmage-Mendelsohn Decomposition (chapter 7)

        #region breadth-first search for coarse decomposition (C0,C1,R1 or R0,R3,C3)

        public int cs_bfs(clsMatrizEsparsa A, int n, int[] wi, int[] wj, int[] queue,
            int[] imatch, int[] jmatch, int mark)
        {
            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            int[] Ap, Ai;
            int head = 0, tail = 0, j, i, p, j2;
            clsMatrizEsparsa C;
            for (j = 0; j < n; j++)           /* place all unmatched nodes in queue */
            {
                if (imatch[j] >= 0) continue; /* skip j if matched */
                wj[j] = 0;                    /* j in set C0 (R0 if transpose) */
                queue[tail++] = j;            /* place unmatched col j in queue */
            }
            if (tail == 0) return (1);         /* quick return if no unmatched nodes */

            //C = (mark == 1) ? ((cs *) A) : cs_transpose (A, 0) ;            
            //if (!C) return (0) ;                /* bfs of C=A' to find R3,C3 from R0 */
            //Ap = C->p ; Ai = C->i ;

            C = (mark == 1) ? A.Clone() : fme.MatrizTransp(A);
            if (C == null) return (0);
            Ap = C.p;
            Ai = C.i;

            while (head < tail)                 /* while queue is not empty */
            {
                j = queue[head++];            /* get the head of the queue */
                for (p = Ap[j]; p < Ap[j + 1]; p++)
                {
                    i = Ai[p];
                    if (wi[i] >= 0) continue; /* skip if i is marked */
                    wi[i] = mark;             /* i in set R1 (C3 if transpose) */
                    j2 = jmatch[i];           /* traverse alternating path to j2 */
                    if (wj[j2] >= 0) continue;/* skip j2 if it is marked */
                    wj[j2] = mark;            /* j2 in set C1 (R3 if transpose) */
                    queue[tail++] = j2;       /* add j2 to queue */
                }
            }

            //if (mark != 1) cs_spfree (C) ;      /* free A' if it was created */

            if (mark != 1) C = null;
            return (1);
        }

        #endregion

        #region collect matched rows and columns into p and q

        public void cs_matched(int n, int[] wj, int[] imatch, int[] p, int[] q,
            int[] cc, int[] rr, int set, int mark)
        {
            int kc = cc[set], j;
            int kr = rr[set - 1];
            for (j = 0; j < n; j++)
            {
                if (wj[j] != mark) continue;      /* skip if j is not in C set */
                p[kr++] = imatch[j];
                q[kc++] = j;
            }
            cc[set + 1] = kc;
            rr[set] = kr;
        }

        #endregion

        #region collect unmatched rows into the permutation vector p

        public void cs_unmatched(int m, int[] wi, int[] p, int[] rr, int set)
        {
            int i, kr = rr[set];
            for (i = 0; i < m; i++) if (wi[i] == 0) p[kr++] = i;
            rr[set + 1] = kr;
        }

        /// <summary>
        /// return 1 if row i is in R2 
        /// </summary>
        private int cs_rprune(int i, int j, double aij, object other)
        {
            int[] rr = (int[])other;
            return (i >= rr[1] && i < rr[2]) ? 1 : 0;
        }

        #endregion

        #region Given A, compute coarse and then fine dmperm

        /// <summary>
        /// Dulmage-Mendelsohn decompositon of a sparse-matrix.
        /// </summary>
        /// <param name="A">Sparse matrix.</param>
        /// <param name="seed">Seed: 0 (ordem natural), -1 (ordem reversa), 1 (ordem aleatoria) </param>
        /// <returns></returns>
        public cs_dmperm_results cs_dmperm(clsMatrizEsparsa A, int seed)
        {
            clsUtilTools clt = new clsUtilTools();

            int m, n, i, j, k, cnz, nc, nb1, nb2, ok;
            int[] jmatch, imatch, wi, wj, pinv, Cp, Ci, ps, rs, p, q, cc, rr, r, s;
            clsMatrizEsparsa C;
            cs_dmperm_results D, scc;
            /* --- Maximum matching ------------------------------------------------- */
            //if (!CS_CSC (A)) return (NULL) ;            /* check inputs */
            m = A.m;
            n = A.n;

            //D = cs_dalloc (m, n) ;                      /* allocate result */
            D = new cs_dmperm_results(m, n);

            //if (!D) return (NULL) ;

            p = D.p; q = D.q; r = D.r; s = D.s; cc = D.cc; rr = D.rr;
            jmatch = cs_maxtrans(A, seed);            /* max transversal */

            //imatch = jmatch + m ;                       /* imatch = inverse of jmatch */
            imatch = clt.SubVector(jmatch, m);                       /* imatch = inverse of jmatch */

            //if (!jmatch) return (cs_ddone (D, NULL, jmatch, 0)) ;

            /* --- Coarse decomposition --------------------------------------------- */
            wi = r; wj = s;                           /* use r and s as workspace */
            for (j = 0; j < n; j++) wj[j] = -1;     /* unmark all cols for bfs */
            for (i = 0; i < m; i++) wi[i] = -1;     /* unmark all rows for bfs */
            cs_bfs(A, n, wi, wj, q, imatch, jmatch, 1);       /* find C1, R1 from C0*/
            ok = cs_bfs(A, m, wj, wi, p, jmatch, imatch, 3);  /* find R3, C3 from R0*/
            //if (!ok) return (cs_ddone (D, NULL, jmatch, 0)) ;
            cs_unmatched(n, wj, q, cc, 0);                    /* unmatched set C0 */
            cs_matched(n, wj, imatch, p, q, cc, rr, 1, 1);    /* set R1 and C1 */
            cs_matched(n, wj, imatch, p, q, cc, rr, 2, -1);   /* set R2 and C2 */
            cs_matched(n, wj, imatch, p, q, cc, rr, 3, 3);    /* set R3 and C3 */
            cs_unmatched(m, wi, p, rr, 3);                    /* unmatched set R0 */
            //cs_free (jmatch) ;
            jmatch = null;
            /* --- Fine decomposition ----------------------------------------------- */
            pinv = cs_pinv(p, m);         /* pinv=p' */
            //if (!pinv) return (cs_ddone (D, NULL, NULL, 0)) ;
            if (pinv == null) return D;
            C = cs_permute(A, pinv, q, 0);/* C=A(p,q) (it will hold A(R2,C2)) */
            //cs_free (pinv) ;
            pinv = null;
            //if (!C) return (cs_ddone (D, NULL, NULL, 0)) ;
            if (C == null) return D;
            Cp = C.p;
            nc = cc[3] - cc[2];          /* delete cols C0, C1, and C3 from C */
            if (cc[2] > 0) for (j = cc[2]; j <= cc[3]; j++) Cp[j - cc[2]] = Cp[j];
            C.n = nc;
            if (rr[2] - rr[1] < m)        /* delete rows R0, R1, and R3 from C */
            {
                cs_fkeep(C, rr);
                cnz = Cp[nc];
                Ci = C.i;
                if (rr[1] > 0) for (k = 0; k < cnz; k++) Ci[k] -= rr[1];
            }
            C.m = nc;
            scc = cs_scc(C);              /* find strongly connected components of C*/

            //if (!scc) return (cs_ddone (D, C, NULL, 0)) ;
            if (scc == null) return D;

            /* --- Combine coarse and fine decompositions --------------------------- */
            ps = scc.p;                   /* C(ps,ps) is the permuted matrix */
            rs = scc.r;                   /* kth block is rs[k]..rs[k+1]-1 */
            nb1 = scc.nb;                /* # of blocks of A(R2,C2) */
            for (k = 0; k < nc; k++) wj[k] = q[ps[k] + cc[2]];
            for (k = 0; k < nc; k++) q[k + cc[2]] = wj[k];
            for (k = 0; k < nc; k++) wi[k] = p[ps[k] + rr[1]];
            for (k = 0; k < nc; k++) p[k + rr[1]] = wi[k];
            nb2 = 0;                       /* create the fine block partitions */
            r[0] = s[0] = 0;
            if (cc[2] > 0) nb2++;         /* leading coarse block A (R1, [C0 C1]) */
            for (k = 0; k < nb1; k++)     /* coarse block A (R2,C2) */
            {
                r[nb2] = rs[k] + rr[1]; /* A (R2,C2) splits into nb1 fine blocks */
                s[nb2] = rs[k] + cc[2];
                nb2++;
            }
            if (rr[2] < m)
            {
                r[nb2] = rr[2];          /* trailing coarse block A ([R3 R0], C3) */
                s[nb2] = cc[3];
                nb2++;
            }
            r[nb2] = m;
            s[nb2] = n;
            D.nb = nb2;

            return D;

            //cs_dfree (scc) ;
            //return (cs_ddone (D, C, NULL, 1)) ;
        }

        #endregion

        #region funções auxiliares para a classe de permutação de Dulmage-Mendelsohn

        #region find the strongly connected components of a square matrix

        cs_dmperm_results cs_scc(clsMatrizEsparsa A)     /* matrix A temporarily modified, then restored */
        {
            int n, i, k, b, nb = 0, top;
            int[] xi, pstack, p, r, Ap, ATp, rcopy, Blk;
            clsMatrizEsparsa AT;
            cs_dmperm_results D;
            //if (!CS_CSC (A)) return (NULL) ;                /* check inputs */
            n = A.n; Ap = A.p;

            //D = cs_dalloc (n, 0) ;                          /* allocate result */
            D = new cs_dmperm_results(n, 0);

            //AT = cs_transpose (A, 0) ;                      /* AT = A' */
            //xi = cs_malloc (2*n+1, sizeof (int)) ;          /* get workspace */

            xi = new int[2 * n + 1];
            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();
            AT = fme.MatrizTransp(A);

            //if (!D || !AT || !xi) return (cs_ddone (D, AT, xi, 0)) ;
            if (D == null || AT == null || xi == null) return D;

            Blk = xi;

            //rcopy = pstack = xi + n ;
            rcopy = pstack = clt.SubVector(xi, n);

            p = D.p; r = D.r; ATp = AT.p;
            top = n;
            for (i = 0; i < n; i++)   /* first dfs(A) to find finish times (xi) */
            {
                if (!CS_MARKED(Ap, i)) top = cs_dfs(i, A, top, ref xi, ref pstack, null);
            }
            for (i = 0; i < n; i++) CS_MARK(ref Ap, i); /* restore A; unmark all nodes*/
            top = n;
            nb = n;
            for (k = 0; k < n; k++)   /* dfs(A') to find strongly connnected comp */
            {
                i = xi[k];            /* get i in reverse order of finish times */
                if (CS_MARKED(ATp, i)) continue;  /* skip node i if already ordered */
                r[nb--] = top;        /* node i is the start of a component in p */
                top = cs_dfs(i, AT, top, ref p, ref pstack, null);
            }
            r[nb] = 0;                /* first block starts at zero; shift r up */
            for (k = nb; k <= n; k++) r[k - nb] = r[k];
            D.nb = nb = n - nb;         /* nb = # of strongly connected components */
            for (b = 0; b < nb; b++)  /* sort each block in natural order */
            {
                for (k = r[b]; k < r[b + 1]; k++) Blk[p[k]] = b;
            }
            for (b = 0; b <= nb; b++) rcopy[b] = r[b];
            for (i = 0; i < n; i++) p[rcopy[Blk[i]]++] = i;

            return D;
            //return (cs_ddone (D, AT, xi, 1)) ;
        }

        #endregion

        #region drop entries for which fkeep(A(i,j)) is false; return nz if OK, else -1

        private int cs_fkeep(clsMatrizEsparsa A, object other)
        {
            int j, p, nz = 0, n;
            int[] Ap, Ai;
            double[] Ax;

            //if (!CS_CSC (A) || !fkeep) return (-1) ;    /* check inputs */

            n = A.n; Ap = A.p; Ai = A.i; Ax = A.x;
            for (j = 0; j < n; j++)
            {
                p = Ap[j];                        /* get current location of col j */
                Ap[j] = nz;                       /* record new location of col j */
                for (; p < Ap[j + 1]; p++)
                {
                    if (cs_rprune(Ai[p], j, Ax != null ? Ax[p] : 1, other) == 1)
                    {
                        if (Ax != null) Ax[nz] = Ax[p];  /* keep A(i,j) */
                        Ai[nz++] = Ai[p];
                    }
                }
            }
            Ap[n] = nz;                           /* finalize A */

            //cs_sprealloc (A, 0) ;                   /* remove extra space from A */

            A.LimparElementosZero();

            return (nz);
        }

        #endregion

        #region find an augmenting path starting at column k and extend the match if found

        private static void cs_augment(int k, clsMatrizEsparsa A, int[] jmatch, int[] cheap, int[] w,
                int[] js, int[] isi, int[] ps)
        {
            int found = 0, p, i = -1;
            int[] Ap = A.p;
            int[] Ai = A.i;
            int head = 0, j;
            js[0] = k;                        /* start with just node k in jstack */
            while (head >= 0)
            {
                /* --- Start (or continue) depth-first-search at node j ------------- */
                j = js[head];                 /* get j from top of jstack */
                if (w[j] != k)                 /* 1st time j visited for kth path */
                {
                    w[j] = k;                 /* mark j as visited for kth path */
                    //for (p = cheap[j]; p < Ap[j + 1] && !found; p++)
                    for (p = cheap[j]; p < Ap[j + 1] && found == 0; p++)
                    {
                        i = Ai[p];            /* try a cheap assignment (i,j) */
                        found = (jmatch[i] == -1) ? 1 : 0;
                    }
                    cheap[j] = p;             /* start here next time j is traversed*/
                    if (found == 1)
                    {
                        isi[head] = i;         /* column j matched with row i */
                        break;                 /* end of augmenting path */
                    }
                    ps[head] = Ap[j];        /* no cheap match: start dfs for j */
                }
                /* --- Depth-first-search of neighbors of j ------------------------- */
                for (p = ps[head]; p < Ap[j + 1]; p++)
                {
                    i = Ai[p];                /* consider row i */
                    if (w[jmatch[i]] == k) continue; /* skip jmatch [i] if marked */
                    ps[head] = p + 1;         /* pause dfs of node j */
                    isi[head] = i;             /* i will be matched with j if found */
                    js[++head] = jmatch[i];  /* start dfs at column jmatch [i] */
                    break;
                }
                if (p == Ap[j + 1]) head--;     /* node j is done; pop from stack */
            }                                   /* augment the match if path found: */
            if (found == 1) for (p = head; p >= 0; p--) jmatch[isi[p]] = js[p];
        }

        #endregion

        #region find a maximum transveral

        public int[] cs_maxtrans(clsMatrizEsparsa A, int seed)  /*[jmatch [0..m-1]; imatch [0..n-1]]*/
        {
            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();

            int i, j, k, n, m, p, n2 = 0, m2 = 0;
            int[] Ap, jimatch, w, cheap, js, isi, ps, Ai, Cp, jmatch, imatch, q;
            clsMatrizEsparsa C;
            //if (!CS_CSC (A)) return (NULL) ;                /* check inputs */
            n = A.n; m = A.m; Ap = A.p; Ai = A.i;
            //w = jimatch = cs_calloc (m+n, sizeof (int)) ;   /* allocate result */
            w = new int[m + n];
            jimatch = new int[m + n];
            //if (!jimatch) return (NULL) ;
            for (k = 0, j = 0; j < n; j++)    /* count nonempty rows and columns */
            {
                n2 += (Ap[j] < Ap[j + 1]) ? 1 : 0;
                for (p = Ap[j]; p < Ap[j + 1]; p++)
                {
                    w[Ai[p]] = 1;
                    k += (j == Ai[p]) ? 1 : 0;        /* count entries already on diagonal */
                }
            }
            if (k == Math.Min(m, n))              /* quick return if diagonal zero-free */
            {
                jmatch = jimatch;

                //imatch = jimatch + m;
                imatch = clt.SubVector(jimatch, m);

                for (i = 0; i < k; i++) jmatch[i] = i;
                for (; i < m; i++) jmatch[i] = -1;
                for (j = 0; j < k; j++) imatch[j] = j;
                for (; j < n; j++) imatch[j] = -1;

                //return (cs_idone(jimatch, NULL, NULL, 1));
                return jimatch;
            }
            for (i = 0; i < m; i++) m2 += w[i];

            C = (m2 < n2) ? fme.MatrizTransp(A) : (A.Clone()); /* transpose if needed */

            //C = (m2 < n2) ? cs_transpose(A, 0) : (A.Clone()); /* transpose if needed */
            //if (!C) return (cs_idone (jimatch, (m2 < n2) ? C : NULL, NULL, 0)) ;

            n = C.n; m = C.m; Cp = C.p;

            //jmatch = (m2 < n2) ? jimatch + n : jimatch;
            //imatch = (m2 < n2) ? jimatch : jimatch + m;

            jmatch = (m2 < n2) ? clt.SubVector(jimatch, n) : jimatch;
            imatch = (m2 < n2) ? jimatch : clt.SubVector(jimatch, m);

            //w = cs_malloc (5*n, sizeof (int)) ;             /* get workspace */
            w = new int[5 * n];
            //if (!w) return (cs_idone (jimatch, (m2 < n2) ? C : NULL, w, 0)) ;

            //cheap = w + n; 
            //js = w + 2 * n; 
            //isi = w + 3 * n; 
            //ps = w + 4 * n;

            cheap = clt.SubVector(w, n);
            js = clt.SubVector(w, 2 * n);
            isi = clt.SubVector(w, 3 * n);
            ps = clt.SubVector(w, 4 * n);

            for (j = 0; j < n; j++) cheap[j] = Cp[j];  /* for cheap assignment */
            for (j = 0; j < n; j++) w[j] = -1;          /* all columns unflagged */
            for (i = 0; i < m; i++) jmatch[i] = -1;     /* nothing matched yet */
            q = cs_randperm(n, seed);                     /* q = random permutation */
            for (k = 0; k < n; k++)   /* augment, starting at column q[k] */
            {
                cs_augment(q != null ? q[k] : k, C, jmatch, cheap, w, js, isi, ps);
            }

            //cs_free(q);
            q = null;

            for (j = 0; j < n; j++) imatch[j] = -1;     /* find row match */
            for (i = 0; i < m; i++) if (jmatch[i] >= 0) imatch[jmatch[i]] = i;

            //return (cs_idone (jimatch, (m2 < n2) ? C : NULL, w, 1)) ;
            return jimatch;
        }

        #endregion

        #region random permutation vector

        /// <summary>
        /// return a random permutation vector, the identity perm, or p = n-1:-1:0.
        /// * seed = -1 means p = n-1:-1:0.  seed = 0 means p = identity.  otherwise
        /// * p = random permutation.
        /// </summary>
        private int[] cs_randperm(int n, int seed)
        {
            int[] p;
            int k, j, t;
            //if (seed == 0) return (NULL);      /* return p = NULL (identity) */
            if (seed == 0) return null;
            //p = cs_malloc(n, sizeof(int));     /* allocate result */
            p = new int[n];
            //if (!p) return (NULL);             /* out of memory */
            for (k = 0; k < n; k++) p[k] = n - k - 1;
            if (seed == -1) return (p);        /* return reverse permutation */
            //srand(seed);                       /* get new random number seed */
            Random rnd = new Random();
            for (k = 0; k < n; k++)
            {
                j = rnd.Next(k, n - 1);
                //j = k + (rand() % (n - k));    /* j = rand int in range k to n-1 */
                t = p[j];                      /* swap p[k] and p[j] */
                p[j] = p[k];
                p[k] = t;
            }
            return (p);
        }

        #endregion

        #endregion

        #endregion
    }
}
