using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace IpeaGeo.RegressoesEspaciais
{
    public enum TipoFuncaoDecaimento : int
    {
        Exponencial,
        Gaussiana,
        Uniforme,
        Quadratico,
        Linear
    };

    public class BLMatrizDistanciasParametricas
    {
        public BLMatrizDistanciasParametricas()
        {
        }

        #region variáveis internas

        protected bool m_usa_num_vizinhos_para_distancia = false;
        public bool UsaNumVizinhosParaDistancia
        {
            get
            {
                return m_usa_num_vizinhos_para_distancia;
            }
            set
            {
                this.m_usa_num_vizinhos_para_distancia = value;
            }
        }

        protected int m_num_vizinhos_para_distancia = 10;
        public int NumVizinhosParaDistancia
        {
            get
            {
                return m_num_vizinhos_para_distancia;
            }
            set
            {
                this.m_num_vizinhos_para_distancia = value;
            }
        }

        private TipoFuncaoDecaimento m_tipo_decaimento = TipoFuncaoDecaimento.Exponencial;
        public TipoFuncaoDecaimento TipoDecaimento
        {
            get
            {
                return m_tipo_decaimento;
            }
            set
            {
            	 m_tipo_decaimento = value;
            }
        }

        private clsMatrizEsparsa m_W_esparsa = new clsMatrizEsparsa();
        public clsMatrizEsparsa Wesparsa
        {
            get
            {
                return m_W_esparsa.Clone();
            }
        }

        private double[] m_y;
        public double[,] Y
        {
            get
            {
                double[,] v = new double[m_y.GetLength(0), 1];
                for (int i = 0; i < v.GetLength(0); i++)
                    v[i, 0] = m_y[i];
                return v;
            }
            set
            {
                m_y = new double[value.GetLength(0)];
                for (int i = 0; i < m_y.GetLength(0); i++)
                    m_y[i] = value[i, 0];
            }
        }

        private double[] m_x;
        public double[,] X
        {
            get
            {
                double[,] v = new double[m_x.GetLength(0), 1];
                for (int i = 0; i < v.GetLength(0); i++)
                    v[i, 0] = m_x[i];
                return v;
            }
            set
            {
                m_x = new double[value.GetLength(0)];
                for (int i = 0; i < m_x.GetLength(0); i++)
                    m_x[i] = value[i,0];
            }
        }

        private double m_dist_max = 0.0;
        public double DistMax
        {
            get
            {
                return m_dist_max;
            }
            set
            {
                m_dist_max = value;
            }
        }

        private double m_parm1 = 0.0;
        public double Parm1
        {
            get
            {
                return m_parm1;
            }
            set
            {
            	m_parm1 = value;
            }
        }

        #endregion

        #region funções de decaimento

        private double linear(double d)
        {
            double fx = 0.0;
            if (d < m_parm1)
            {
                fx = 1.0 - (d / m_parm1);
            }
            return fx;
        }

        private double quadratico(double d)
        {
            double fx = 0.0;
            if (d < m_parm1)
            {
                fx = 1.0 - Math.Pow(d / m_parm1, 2.0);
            }
            return fx;
        }

        private double normal(double d)
        {
            double sigma2 = m_parm1*m_parm1;
            double fx = Math.Exp(-d*d / (2*sigma2));
            return fx;
        }

        private double exponencial(double d)
        {
            double fx = Math.Exp(-m_parm1 * d);
            return fx;                
        }

        private double uniforme(double d)
        {
            double fx = 1.0;
            return fx;
        }

        #endregion

        #region Matriz esparsa não normalizada

        public void MatrizWesparsaFromDistancia()
        {
            ArrayList indi = new ArrayList();
            ArrayList indj = new ArrayList();
            ArrayList valor = new ArrayList();
            double d = 0.0;
            double fd = 0.0;
            switch (m_tipo_decaimento)
            {
                case TipoFuncaoDecaimento.Exponencial:
                    for (int i = 0; i < m_x.GetLength(0); i++)
                    {
                        for (int j = i+1; j < m_x.GetLength(0); j++)
                        {
                            d = Math.Sqrt(Math.Pow(m_x[i] - m_x[j], 2.0) + Math.Pow(m_y[i] - m_y[j], 2.0));
                            if (d <= m_dist_max)
                            {
                                fd = exponencial(d);

                                indi.Add(i);
                                indj.Add(j);
                                valor.Add(fd);

                                indi.Add(j);
                                indj.Add(i);
                                valor.Add(fd);
                            }
                        }
                    }
                    break;
                case TipoFuncaoDecaimento.Gaussiana:
                    for (int i = 0; i < m_x.GetLength(0); i++)
                    {
                        for (int j = i + 1; j < m_x.GetLength(0); j++)
                        {
                            d = Math.Sqrt(Math.Pow(m_x[i] - m_x[j], 2.0) + Math.Pow(m_y[i] - m_y[j], 2.0));
                            if (d <= m_dist_max)
                            {
                                fd = normal(d);

                                indi.Add(i);
                                indj.Add(j);
                                valor.Add(fd);

                                indi.Add(j);
                                indj.Add(i);
                                valor.Add(fd);
                            }
                        }
                    }
                    break;
                case TipoFuncaoDecaimento.Uniforme:
                    for (int i = 0; i < m_x.GetLength(0); i++)
                    {
                        for (int j = i + 1; j < m_x.GetLength(0); j++)
                        {
                            d = Math.Sqrt(Math.Pow(m_x[i] - m_x[j], 2.0) + Math.Pow(m_y[i] - m_y[j], 2.0));
                            if (d <= m_dist_max)
                            {
                                fd = uniforme(d);

                                indi.Add(i);
                                indj.Add(j);
                                valor.Add(fd);

                                indi.Add(j);
                                indj.Add(i);
                                valor.Add(fd);
                            }
                        }
                    }
                    break;
                case TipoFuncaoDecaimento.Quadratico:
                    for (int i = 0; i < m_x.GetLength(0); i++)
                    {
                        for (int j = i + 1; j < m_x.GetLength(0); j++)
                        {
                            d = Math.Sqrt(Math.Pow(m_x[i] - m_x[j], 2.0) + Math.Pow(m_y[i] - m_y[j], 2.0));
                            if (d <= m_dist_max)
                            {
                                fd = quadratico(d);

                                indi.Add(i);
                                indj.Add(j);
                                valor.Add(fd);

                                indi.Add(j);
                                indj.Add(i);
                                valor.Add(fd);
                            }
                        }
                    }
                    break;
                case TipoFuncaoDecaimento.Linear:
                    for (int i = 0; i < m_x.GetLength(0); i++)
                    {
                        for (int j = i + 1; j < m_x.GetLength(0); j++)
                        {
                            d = Math.Sqrt(Math.Pow(m_x[i] - m_x[j], 2.0) + Math.Pow(m_y[i] - m_y[j], 2.0));
                            if (d <= m_dist_max)
                            {
                                fd = linear(d);

                                indi.Add(i);
                                indj.Add(j);
                                valor.Add(fd);

                                indi.Add(j);
                                indj.Add(i);
                                valor.Add(fd);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            int nzmax = valor.Count;

            clsMatrizEsparsa a = new clsMatrizEsparsa(m_x.GetLength(0), m_y.GetLength(0), nzmax, true);

            for (int i = 0; i < nzmax; i++)
            {
                a.x[i] = (double)valor[i];
                a.i[i] = (int)indi[i];
                a.p[i] = (int)indj[i];
            }

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            m_W_esparsa = fme.TripletForm2CompressColumn(a);
        }

        #endregion
    
        #region Matriz esparsa normalizada

        public void MatrizWesparsaFromDistanciaNorm()
        {
            ArrayList indi = new ArrayList();
            ArrayList indj = new ArrayList();
            ArrayList valor = new ArrayList();
            double d = 0.0;
            double fd = 0.0;
            double[] soma_rows = new double[m_x.GetLength(0)];

            switch (m_tipo_decaimento)
            {
                case TipoFuncaoDecaimento.Exponencial:
                    for (int i = 0; i < m_x.GetLength(0); i++)
                    {
                        for (int j = i+1; j < m_x.GetLength(0); j++)
                        {
                            d = Math.Sqrt(Math.Pow(m_x[i] - m_x[j], 2.0) + Math.Pow(m_y[i] - m_y[j], 2.0));
                            if (d <= m_dist_max)
                            {
                                fd = exponencial(d);

                                indi.Add(i);
                                indj.Add(j);
                                valor.Add(fd);
                                soma_rows[i] += fd;

                                indi.Add(j);
                                indj.Add(i);
                                valor.Add(fd);
                                soma_rows[j] += fd;
                            }
                        }
                    }
                    break;
                case TipoFuncaoDecaimento.Gaussiana:
                    for (int i = 0; i < m_x.GetLength(0); i++)
                    {
                        for (int j = i + 1; j < m_x.GetLength(0); j++)
                        {
                            d = Math.Sqrt(Math.Pow(m_x[i] - m_x[j], 2.0) + Math.Pow(m_y[i] - m_y[j], 2.0));
                            if (d <= m_dist_max)
                            {
                                fd = normal(d);

                                indi.Add(i);
                                indj.Add(j);
                                valor.Add(fd);
                                soma_rows[i] += fd;

                                indi.Add(j);
                                indj.Add(i);
                                valor.Add(fd);
                                soma_rows[j] += fd;
                            }
                        }
                    }
                    break;
                case TipoFuncaoDecaimento.Uniforme:
                    for (int i = 0; i < m_x.GetLength(0); i++)
                    {
                        for (int j = i + 1; j < m_x.GetLength(0); j++)
                        {
                            d = Math.Sqrt(Math.Pow(m_x[i] - m_x[j], 2.0) + Math.Pow(m_y[i] - m_y[j], 2.0));
                            if (d <= m_dist_max)
                            {
                                fd = uniforme(d);

                                indi.Add(i);
                                indj.Add(j);
                                valor.Add(fd);
                                soma_rows[i] += fd;

                                indi.Add(j);
                                indj.Add(i);
                                valor.Add(fd);
                                soma_rows[j] += fd;
                            }
                        }
                    }
                    break;
                case TipoFuncaoDecaimento.Quadratico:
                    for (int i = 0; i < m_x.GetLength(0); i++)
                    {
                        for (int j = i + 1; j < m_x.GetLength(0); j++)
                        {
                            d = Math.Sqrt(Math.Pow(m_x[i] - m_x[j], 2.0) + Math.Pow(m_y[i] - m_y[j], 2.0));
                            if (d <= m_dist_max)
                            {
                                fd = quadratico(d);

                                indi.Add(i);
                                indj.Add(j);
                                valor.Add(fd);
                                soma_rows[i] += fd;

                                indi.Add(j);
                                indj.Add(i);
                                valor.Add(fd);
                                soma_rows[j] += fd;
                            }
                        }
                    }
                    break;
                case TipoFuncaoDecaimento.Linear:
                    for (int i = 0; i < m_x.GetLength(0); i++)
                    {
                        for (int j = i + 1; j < m_x.GetLength(0); j++)
                        {
                            d = Math.Sqrt(Math.Pow(m_x[i] - m_x[j], 2.0) + Math.Pow(m_y[i] - m_y[j], 2.0));
                            if (d <= m_dist_max)
                            {
                                fd = linear(d);

                                indi.Add(i);
                                indj.Add(j);
                                valor.Add(fd);
                                soma_rows[i] += fd;

                                indi.Add(j);
                                indj.Add(i);
                                valor.Add(fd);
                                soma_rows[j] += fd;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            int nzmax = valor.Count;

            clsMatrizEsparsa a = new clsMatrizEsparsa(m_x.GetLength(0), m_y.GetLength(0), nzmax, true);

            int row = 0;
            for (int i = 0; i < nzmax; i++)
            {
                row = (int)indi[i];
                a.x[i] = (double)valor[i] / soma_rows[row];
                a.i[i] = row;
                a.p[i] = (int)indj[i];
            }

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            m_W_esparsa = fme.TripletForm2CompressColumn(a);
        }

        #endregion

        #region Gera distância máxima a partir dos centroides

        public double GeraMaxDistancia(DataTable dt, string v1, string v2)
        {
            clsUtilTools clt = new clsUtilTools();
            double[,] x = clt.GetMatrizFromDataTable(dt, v1);
            double[,] y = clt.GetMatrizFromDataTable(dt, v2);

            double max = 0.0;
            double d = 0.0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                for (int j = i+1; j < y.GetLength(0); j++)
                {
                    d = (Math.Pow(x[i, 0] - x[j, 0], 2.0) + Math.Pow(y[i, 0] - y[j, 0], 2.0));
                    if (d > max)
                    {
                        max = d;
                    }
                }
            }
            return Math.Sqrt(max);
        }

        #endregion

        #region Gera função de decaimento para mostrar na tabela os valores

        public DataTable FuncaoDecaimento()
        {
            clsUtilTools clt = new clsUtilTools();

            int npontos = 200;
            double min = 0.0;
            double max = (200.0 / 198.0) * this.m_dist_max;
            double delta = (max - min) / ((double)(npontos - 1));
            double[,] xf = new double[npontos, 2];
            double[,] obs = new double[npontos, 1];
            for (int i = 0; i < npontos; i++)
            {
                obs[i, 0] = (double)(i + 1);
                xf[i, 0] = min + ((double)i) * delta;
                if (xf[i, 0] <= m_dist_max)
                {
                    switch (this.m_tipo_decaimento)
                    {
                        case TipoFuncaoDecaimento.Exponencial:
                            xf[i, 1] = exponencial(xf[i, 0]);
                            break;
                        case TipoFuncaoDecaimento.Gaussiana:
                            xf[i, 1] = normal(xf[i, 0]);
                            break;
                        case TipoFuncaoDecaimento.Linear:
                            xf[i, 1] = linear(xf[i, 0]);
                            break;
                        case TipoFuncaoDecaimento.Quadratico:
                            xf[i, 1] = quadratico(xf[i, 0]);
                            break;
                        case TipoFuncaoDecaimento.Uniforme:
                            xf[i, 1] = uniforme(xf[i, 0]);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    xf[i, 1] = 0.0;
                }
            }

            xf = clt.Concateh(obs, xf);

            string[,] sxf = new string[xf.GetLength(0), xf.GetLength(1)];
            for (int i = 0; i < xf.GetLength(0); i++)
            {
                sxf[i, 0] = xf[i, 0].ToString();
                for (int j = 1; j < xf.GetLength(1); j++)
                {
                    sxf[i, j] = clt.Double2Texto(xf[i, j], 8);
                }
            }

            string[] vs = new string[3];
            vs[0] = "observacao";
            vs[1] = "distancia";
            vs[2] = "funcao_decaimento";

            DataTable dt = clt.DataTableFromStringMatriz(sxf, vs);

            dt.Columns["observacao"].Caption = "Obs.";
            dt.Columns["distancia"].Caption = "Distância";
            dt.Columns["funcao_decaimento"].Caption = "Função de decaimento";

            return dt.Copy();
        }

        #endregion
        
    }
}
