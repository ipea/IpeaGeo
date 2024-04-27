using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IpeaGeo.RegressoesEspaciais
{
    public enum TipoMatrizEsparsa : int
    {
        TripletForm,
        CompressColumn
    };

    public class clsMatrizEsparsa
    {
        #region Construtores

        public clsMatrizEsparsa(double[,] mat)
        {
            m_hora_criacao = DateTime.Now;

            m_nz = 0;
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    if (mat[i, j] != 0.0) m_nz++;
                }
            }
            m_nzmax = m_nz;
            m_nz = -1;

            m_x = new double[m_nzmax];
            m_i = new int[m_nzmax];
            m_p = new int[mat.GetLength(1) + 1];
            m_m = mat.GetLength(0);
            m_n = mat.GetLength(1);

            int indice = 0;
            int indice_p = 1;
            for (int j = 0; j < mat.GetLength(1); j++)
            {
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    if (mat[i, j] != 0.0)
                    {
                        m_i[indice] = i;
                        m_x[indice] = mat[i, j];
                        indice++;
                    }
                }
                m_p[indice_p] = indice;
                indice_p++;
            }
        }

        public clsMatrizEsparsa(double[,] mat, bool formato_triplet)
        {
            m_hora_criacao = DateTime.Now;

            if (formato_triplet)
            {
                m_nz = 0;
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    for (int j = 0; j < mat.GetLength(1); j++)
                    {
                        if (mat[i, j] != 0.0) m_nz++;
                    }
                }
                m_nzmax = m_nz;

                m_x = new double[m_nzmax];
                m_i = new int[m_nz];
                m_p = new int[m_nz];
                m_m = mat.GetLength(0);
                m_n = mat.GetLength(1);

                int indice = 0;
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    for (int i = 0; i < mat.GetLength(0); i++)
                    {
                            if (mat[i, j] != 0.0)
                        {
                            m_i[indice] = i;
                            m_p[indice] = j;
                            m_x[indice] = mat[i, j];
                            indice++;
                        }
                    }
                }
            }
            else
            {
                m_nz = 0;
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    for (int j = 0; j < mat.GetLength(1); j++)
                    {
                        if (mat[i, j] != 0.0) m_nz++;
                    }
                }
                m_nzmax = m_nz;
                m_nz = -1;

                m_x = new double[m_nzmax];
                m_i = new int[m_nzmax];
                m_p = new int[mat.GetLength(1) + 1];
                m_m = mat.GetLength(0);
                m_n = mat.GetLength(1);

                int indice = 0;
                int indice_p = 1;
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    for (int i = 0; i < mat.GetLength(0); i++)
                    {
                        if (mat[i, j] != 0.0)
                        {
                            m_i[indice] = i;
                            m_x[indice] = mat[i, j];
                            indice++;
                        }
                    }
                    m_p[indice_p] = indice;
                    indice_p++;
                }
            }
        }

        public clsMatrizEsparsa()
        {
            m_hora_criacao = DateTime.Now;

            m_x = new double[m_nzmax];
            m_i = new int[m_nzmax];
            m_p = new int[m_n + 1];

            m_nz = -1; //--- default é matriz esparsa em formato compress-column 
        }

        public clsMatrizEsparsa(int m, int n, int nzmax, bool formato_triplet)
        {
            m_hora_criacao = DateTime.Now;

            m_nzmax = nzmax;
            m_n = n;
            m_m = m;
            m_x = new double[m_nzmax];
            m_i = new int[m_nzmax];
            m_p = formato_triplet ? new int[m_nzmax] : new int[m_n + 1];
            m_nz = formato_triplet ? nzmax : -1;
        }

        public clsMatrizEsparsa(int m, int n, int nzmax, double[] x, bool formato_triplet)
        {
            m_hora_criacao = DateTime.Now;

            m_nzmax = Math.Max(nzmax, x.GetLength(0));
            m_n = n;
            m_m = m;
            m_x = (double[])x.Clone();
            m_i = new int[x.GetLength(0)];
            m_p = formato_triplet ? new int[x.GetLength(0)] : new int[m_n + 1];
            m_nz = formato_triplet ? x.GetLength(0): -1;
        }

        #endregion

        #region Limpa matriz esparsa, para se livrar dos elementos zero

        /// <summary>
        /// Função para percorrer a matriz esparsa, e se livrar dos elementos nulos, tornando a matriz mais compacta, 
        /// evitando problemas numéricos nas rotinas futuras. 
        /// </summary>
        public clsMatrizEsparsa LimparElementosZero()
        {
            TipoMatrizEsparsa tipo_original = this.FormatoMatrizEsparsa;
            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsMatrizEsparsa T = fme.CompressColumn2TripletForm(this);
            ArrayList tx = new ArrayList();
            ArrayList ti = new ArrayList();
            ArrayList tp = new ArrayList();
            for (int i = 0; i < T.x.GetLength(0); i++)
            {
                if (T.x[i] != 0.0)
                {
                    tx.Add(T.x[i]);
                    ti.Add(T.i[i]);
                    tp.Add(T.p[i]);
                }
            }
            double[] mx = new double[tx.Count];
            int[] mi = new int[tx.Count];
            int[] mp = new int[tx.Count];
            for (int i = 0; i < mx.GetLength(0); i++)
            {
                mx[i] = Convert.ToDouble(tx[i]);
                mi[i] = Convert.ToInt32(ti[i]);
                mp[i] = Convert.ToInt32(tp[i]);
            }
            clsMatrizEsparsa T1 = new clsMatrizEsparsa(T.m, T.n, mx.GetLength(0), mx, true);
            T1.x = mx;
            T1.i = mi;
            T1.p = mp;
            T1.nz = mx.GetLength(0);
            T1.nzmax = mx.GetLength(0);
            if (tipo_original == TipoMatrizEsparsa.TripletForm)
            {
                return T1;
            }
            else
            {
                return fme.TripletForm2CompressColumn(T1);
            }
        }

        #endregion

        #region Variáveis internas

        protected DateTime m_hora_criacao;
        public DateTime HoraCriacao
        {
            get
            {
                return m_hora_criacao;
            }
            set
            {
            	m_hora_criacao = value;
            }
        }

        protected int m_nzmax = 1;
        protected int m_m = 1;
        protected int m_n = 1;
        protected int[] m_p = new int[0];
        protected int[] m_i = new int[0];
        protected double[] m_x = new double[0];
        protected int m_nz = -1;

        public double[,] AsDoubleMatrix()
        {
            //get
            {
                if (this.m_m * this.m_n > 1000000) throw new Exception("Matriz esparsa é muito grande para ser convertida em matriz de doubles.");

                double[,] res = new double[m_m, m_n];

                clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
                clsMatrizEsparsa aux_mat = fme.CompressColumn2TripletForm(this);

                for (int i = 0; i < aux_mat.nz; i++)
                {
                    res[aux_mat.row_indices[i], aux_mat.col_indices[i]] = aux_mat.x[i];
                }

                return res;
            }
        }

        public clsMatrizEsparsa Clone()
        {
            clsMatrizEsparsa res = new clsMatrizEsparsa(this.m, this.n, this.nzmax, (this.m_nz < 0) ? true : false);
            res.m_x = (double[])this.m_x.Clone();
            res.m_p = (int[])this.m_p.Clone();
            res.m_nzmax = this.m_nzmax;
            res.m_m = this.m_m;
            res.m_i = (int[])this.m_i.Clone();
            res.m_n = this.m_n;
            res.m_nz = this.m_nz;

            res.m_hora_criacao = this.m_hora_criacao;

            return res;
        }

        public TipoMatrizEsparsa FormatoMatrizEsparsa
        {
            get
            {
                if (m_nz >= 0) return TipoMatrizEsparsa.TripletForm;
                else return TipoMatrizEsparsa.CompressColumn;
            }
        }

        public int[] i
        {
            get { return this.m_i; }
            set { this.m_i = value; }
        }

        public int[] p
        {
            get { return this.m_p; }
            set { this.m_p = value; }
        }

        public int[] row_indices
        {
            get { return this.m_i; }
            set { this.m_i = value; }
        }

        public int[] col_indices
        {
            get { return this.m_p; }
            set { this.m_p = value; }
        }

        public double[] x
        {
            get { return this.m_x; }
            set { this.m_x = value; }
        }

        public int n
        {
            get { return this.m_n; }
            set { this.m_n = value; }
        }

        public int m
        {
            get { return this.m_m; }
            set { this.m_m = value; }
        }

        public int nzmax
        {
            get { return this.m_nzmax; }
            set { this.m_nzmax = value; }
        }

        public int nz
        {
            get { return this.m_nz; }
            set { this.m_nz = value; }
        }

        #endregion

        #region Estatísticas descritivas

        public string EstatisticasDescritivas()
        {
            System.Globalization.NumberFormatInfo m_format = new System.Globalization.CultureInfo("en-US", false).NumberFormat;

            string out_text = "=========================================================\n\n";

            out_text += "Características da Matriz Esparsa \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Número de linhas: " + String.Format(m_format, "{0:0,0}", m_m) + "\n";
            out_text += "Número de colunas: " + String.Format(m_format, "{0:0,0}", m_n) + "\n";
            out_text += "Número total de elementos: " + String.Format(m_format, "{0:0,0}", m_m * m_n) + "\n";
            out_text += "Número elementos não-nulos: " + String.Format(m_format, "{0:0,0}", m_x.GetLength(0)) + "\n";
            out_text += "Número elementos nulos: " + String.Format(m_format, "{0:0,0}", (m_m * m_n - m_x.GetLength(0))) + "\n";
            out_text += "Percentual de elementos não-nulos: " + String.Format(m_format, "{0:0.00000000}", 
                                                100.0 * ((double)m_x.GetLength(0))/((double)m_m * m_n)) + " %" + "\n\n";

            clsUtilTools clt = new clsUtilTools();

            double media = clt.Mean(x);
            double max = clt.Max(x);
            double min = clt.Min(x);
            double p75 = clt.Percentil(x, 75.0);
            double p25 = clt.Percentil(x, 25.0);
            double p90 = clt.Percentil(x, 90.0);
            double p10 = clt.Percentil(x, 10.0);
            double p50 = clt.Percentil(x, 50.0);

            out_text += "Valor médio dos elementos não-nulos: " + String.Format(m_format, "{0:0.00000000}", media) + "\n";
            out_text += "Valor mínimo dos elementos não-nulos: " + String.Format(m_format, "{0:0.00000000}", min) + "\n";
            out_text += "Valor máximo dos elementos não-nulos: " + String.Format(m_format, "{0:0.00000000}", max) + "\n\n";

            out_text += "Percentil 10% dos elementos não-nulos: " + String.Format(m_format, "{0:0.00000000}", p10) + "\n";
            out_text += "Percentil 25% dos elementos não-nulos: " + String.Format(m_format, "{0:0.00000000}", p25) + "\n";
            out_text += "Percentil 50% dos elementos não-nulos: " + String.Format(m_format, "{0:0.00000000}", p50) + "\n";
            out_text += "Percentil 75% dos elementos não-nulos: " + String.Format(m_format, "{0:0.00000000}", p75) + "\n";
            out_text += "Percentil 90% dos elementos não-nulos: " + String.Format(m_format, "{0:0.00000000}", p90) + "\n\n";

            double[] numero_vizinhos = new double[m_m];
            if (this.FormatoMatrizEsparsa == TipoMatrizEsparsa.CompressColumn)
            {
                clsFuncoesMatrizEsparsa cfm = new clsFuncoesMatrizEsparsa();
                clsMatrizEsparsa aux = cfm.CompressColumn2TripletForm(this);
                for (int i = 0; i < aux.row_indices.GetLength(0); i++)
                {
                    numero_vizinhos[aux.row_indices[i]]++;
                }
            }
            else
            {
                for (int i = 0; i < this.row_indices.GetLength(0); i++)
                {
                    numero_vizinhos[this.row_indices[i]]++;
                }
            }

            media = clt.Mean(numero_vizinhos);
            max = clt.Max(numero_vizinhos);
            min = clt.Min(numero_vizinhos);

            out_text += "Valor médio do número de vizinhos: " + String.Format(m_format, "{0:0.00000000}", media) + "\n";
            out_text += "Valor mínimo do número de vizinhos: " + String.Format(m_format, "{0:n}", min) + "\n";
            out_text += "Valor máximo do número de vizinhos: " + String.Format(m_format, "{0:n}", max) + "\n\n";

            return out_text;
        }

        #endregion

        #region rotinas para cálculo do log Jacobiano da matriz I - rho x W via eigenvalue decomposition

        protected bool m_autovalores_gerados = false;
        public void ResetMatrizEsparsa()
        {
            m_autovalores_gerados = false;
            m_autovalores_W = new double[0];
            m_tabela_logdet_gerada = false;
        }

        protected double[] m_autovalores_W = new double[0];
        public double[] Autovalores
        {
            get
            {
                return m_autovalores_W;
            }
        }

        public void CalcularAutovaloresW()
        {
            if (!m_autovalores_gerados)
            {
                clsUtilTools clt = new clsUtilTools();
                Complex[] autovalores = new Complex[0];
                clt.AutovaloresMatrizAssimetrica(this.AsDoubleMatrix(), ref autovalores);
                m_autovalores_W = new double[autovalores.GetLength(0)];
                for (int i = 0; i < m_autovalores_W.GetLength(0); i++)
                {
                    m_autovalores_W[i] = autovalores[i].Real;
                }
                m_autovalores_gerados = true;
            }
        }

        public double LogDetIrhoW(double rho)
        {
            if (!m_autovalores_gerados)
            {
                this.CalcularAutovaloresW();
            }

            double res = 0.0;
            for (int i = 0; i < m_autovalores_W.GetLength(0); i++)
            {
                res += Math.Log(1.0 - rho * m_autovalores_W[i]);
            }
            return res;
        }

        #endregion

        #region Tabela com valores do determinante da matriz (I - rho x W) como função do parâmetro rho

        protected Spline_interp m_spline;
        public Spline_interp Spline
        {
            get
            {
                return m_spline;
            }
            set
            {
                m_spline = value;
            }
        }

        protected bool m_tabela_logdet_gerada = false;

        public void GeraTabelaLogDetAutovalores(double lmin, double lmax)
        {
            if (!m_tabela_logdet_gerada)
            {
                if (this.m > 1000 && this.n > 1000)
                {
                    GeraTabelaLogDetLU(lmin, lmax);
                }
                else
                {
                    if (!m_autovalores_gerados) this.CalcularAutovaloresW();

                    int numl = Convert.ToInt32(Math.Ceiling((lmax - lmin) / 0.01));
                    numl = Math.Min(250, numl);
                    double delta = (lmax - lmin) / ((double)numl);
                    double[] vetor_rho = new double[numl + 1];
                    double[] vetor_ldet = new double[numl + 1];
                    ArrayList array_rho = new ArrayList();
                    ArrayList array_ldet = new ArrayList();
                    for (int i = 0; i < numl + 1; i++)
                    {
                        vetor_rho[i] = ((double)i) * delta + lmin;
                        vetor_ldet[i] = LogDetIrhoW(vetor_rho[i]);
                        if (!double.IsInfinity(vetor_ldet[i]) && !double.IsNaN(vetor_ldet[i])
                            && !double.IsNegativeInfinity(vetor_ldet[i]) && !double.IsPositiveInfinity(vetor_ldet[i]))
                        {
                            array_rho.Add(vetor_rho[i]);
                            array_ldet.Add(vetor_ldet[i]);
                        }
                    }
                    vetor_rho = new double[array_rho.Count];
                    vetor_ldet = new double[array_ldet.Count];
                    for (int i = 0; i < vetor_ldet.GetLength(0); i++)
                    {
                        vetor_ldet[i] = (double)array_ldet[i];
                        vetor_rho[i] = (double)array_rho[i];
                    }
                    m_spline = new Spline_interp(vetor_rho, vetor_ldet);
                    this.m_tabela_logdet_gerada = true;
                }
            }
        }

        public void GeraTabelaLogDetLU(double lmin, double lmax)
        {
            if (!m_tabela_logdet_gerada)
            {
                clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
                clsMatrizEsparsa eye = fme.Identity(this.m);
                clsMatrizEsparsa B;
                int numl = Convert.ToInt32(Math.Ceiling((lmax - lmin) / 0.01));
                numl = Math.Min(250, numl);
                double delta = (lmax - lmin) / ((double)numl);
                double[] vetor_rho = new double[numl + 1];
                double[] vetor_ldet = new double[numl + 1];
                ArrayList array_rho = new ArrayList();
                ArrayList array_ldet = new ArrayList();
                for (int i = 0; i < numl + 1; i++)
                {
                    vetor_rho[i] = ((double)i) * delta + lmin;
                    B = fme.MatrizSoma(eye, this, 1.0, -vetor_rho[i]);
                    vetor_ldet[i] = fme.LogDet(B);
                    if (!double.IsInfinity(vetor_ldet[i]) && !double.IsNaN(vetor_ldet[i])
                        && !double.IsNegativeInfinity(vetor_ldet[i]) && !double.IsPositiveInfinity(vetor_ldet[i]))
                    {
                        array_rho.Add(vetor_rho[i]);
                        array_ldet.Add(vetor_ldet[i]);
                    }
                }
                vetor_rho = new double[array_rho.Count];
                vetor_ldet = new double[array_ldet.Count];
                for (int i = 0; i < vetor_ldet.GetLength(0); i++)
                {
                    vetor_ldet[i] = (double)array_ldet[i];
                    vetor_rho[i] = (double)array_rho[i];
                }
                m_spline = new Spline_interp(vetor_rho, vetor_ldet);
                this.m_tabela_logdet_gerada = true;
            }
        }

        #endregion
    }
}
