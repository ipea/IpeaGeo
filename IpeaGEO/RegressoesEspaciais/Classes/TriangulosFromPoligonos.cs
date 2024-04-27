using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace IpeaGEO.RegressoesEspaciais
{
    public class TriangulosFromPoligonos
    {
        public TriangulosFromPoligonos()
        {
        }

        private ArrayList m_px = new ArrayList();
        private ArrayList m_py = new ArrayList();
        private ArrayList m_triangulos = new ArrayList();
        private ArrayList m_areas_triangulos = new ArrayList();
        private ArrayList m_centroids_triangulos = new ArrayList();
        private ArrayList m_tipo_triangulos = new ArrayList();
        private bool m_area_calculada = false;

        private double m_max_x = double.NegativeInfinity;
        private double m_max_y = double.NegativeInfinity;
        private double m_min_x = double.PositiveInfinity;
        private double m_min_y = double.PositiveInfinity;

        public bool IsInPoligono(double x0, double y0)
        {
            if (x0 > m_max_x) return false;
            if (x0 < m_min_x) return false;
            if (y0 > m_max_y) return false;
            if (y0 < m_min_y) return false;

            double[,] c = new double[0, 0];
            bool dentro_outer = false;
            for (int i = 0; i < m_triangulos.Count; i++)
            {
                if ((double)m_tipo_triangulos[i] > 0.0)
                {
                    c = (double[,])m_triangulos[i];
                    if (IsInTriangulo(x0, y0, c))
                    {
                        dentro_outer = true;
                        break;
                    }
                }
            }

            if (dentro_outer)
            {
                for (int i = 0; i < m_triangulos.Count; i++)
                {
                    if ((double)m_tipo_triangulos[i] < 0.0)
                    {
                        c = (double[,])m_triangulos[i];
                        if (IsInTriangulo(x0, y0, c))
                        {
                            return false;
                        }
                    }
                }
            }

            return dentro_outer;
        }

        private bool IsInTriangulo(double x0, double y0, double[,] c)
        {
            if (x0 < c[0, 0] && x0 < c[1, 0] && x0 < c[2, 0]) return false;
            if (y0 < c[0, 1] && y0 < c[1, 1] && y0 < c[2, 1]) return false;
            if (x0 > c[0, 0] && x0 > c[1, 0] && x0 > c[2, 0]) return false;
            if (y0 > c[0, 1] && y0 > c[1, 1] && y0 > c[2, 1]) return false;

            double area0 = Math.Abs(c[0, 0] * c[1, 1] + c[1, 0] * c[2, 1] + c[2, 0] * c[0, 1] - c[0, 0] * c[2, 1] - c[2, 0] * c[1, 1] - c[1, 0] * c[0, 1]);

            double area1 = Math.Abs(x0 * c[1, 1] + c[1, 0] * c[2, 1] + c[2, 0] * y0 - x0 * c[2, 1] - c[2, 0] * c[1, 1] - c[1, 0] * y0);
            double area2 = Math.Abs(c[0, 0] * y0 + x0 * c[2, 1] + c[2, 0] * c[0, 1] - c[0, 0] * c[2, 1] - c[2, 0] * y0 - x0 * c[0, 1]);
            double area3 = Math.Abs(c[0, 0] * c[1, 1] + c[1, 0] * y0 + x0 * c[0, 1] - c[0, 0] * y0 - x0 * c[1, 1] - c[1, 0] * c[0, 1]);

            return Math.Abs(area0 - (area1 + area2 +area3)) < 1.0e-8;
        }

        public double[] CentroidCoords()
        {
            if (!m_area_calculada)
            {
                double r = this.AreaTotal();
            }

            double[,] c = new double[0, 0];
            double[] xy = new double[2];
            double area = 0.0;

            double numeradorx = 0.0;
            double numeradory = 0.0;
            double denominador = 0.0;

            for (int i = 0; i < m_triangulos.Count; i++)
            {
                c = (double[,])m_triangulos[i];

                xy[0] = (c[0, 0] + c[1, 0] + c[2, 0]) / 3.0;
                xy[1] = (c[0, 1] + c[1, 1] + c[2, 1]) / 3.0;
                m_centroids_triangulos.Add(xy);

                area = (double)m_areas_triangulos[i];
                numeradorx += area * xy[0];
                numeradory += area * xy[1];
                denominador += area;
            }

            double[] centroidxy = new double[2];
            centroidxy[0] = numeradorx / denominador;
            centroidxy[1] = numeradory / denominador;

            return centroidxy;
        }

        public double AreaTotal()
        {
            double area_total = 0.0;
            double[,] c = new double[0,0];
            double area = 0.0;

            for (int i = 0; i < m_triangulos.Count; i++)
            {
                c = (double[,])m_triangulos[i];

                area = Math.Abs(c[0,0] * c[1,1] + c[1,0] * c[2,1] + c[2,0] * c[0,1] - c[0,0] * c[2,1] - c[2,0] * c[1,1] - c[1,0] * c[0,1]) / 2.0;

                area *= (double)this.m_tipo_triangulos[i];
                area_total += area;
                m_areas_triangulos.Add(area);
            }

            m_area_calculada = true;
            return area_total;
        }

        public void ZeraTriangulos()
        {
            m_area_calculada = false;

            m_px.Clear();
            m_py.Clear();
            m_triangulos.Clear();
            m_areas_triangulos.Clear();
            m_centroids_triangulos.Clear();
            m_tipo_triangulos.Clear();
        }

        public void AdicionaTriangulosFromPoligono(double[] x, double[] y, bool poligono_externo)
        {
            m_area_calculada = false;

            m_px.Clear();
            m_py.Clear();
            m_areas_triangulos.Clear();
            m_centroids_triangulos.Clear();

            int ultimo = x.GetLength(0);
            if (x[0] == x[x.GetLength(0) - 1] && y[0] == y[y.GetLength(0) - 1]) ultimo--; 

            for (int i = 0; i < ultimo; i++)
            {
                m_px.Add(x[i]);
                m_py.Add(y[i]);

                if (x[i] > m_max_x) m_max_x = x[i];
                if (x[i] < m_min_x) m_min_x = x[i];
                if (y[i] > m_max_y) m_max_y = y[i];
                if (y[i] < m_min_y) m_min_y = y[i];
            }

            bool retorno = true;
            while (retorno)
            {
                retorno = AddTriangulo(poligono_externo);
            }
        }

        private bool AddTriangulo(bool poligono_externo)
        {
            double[,] triang = new double[3, 2];
            if (m_px.Count <= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    triang[i, 0] = (double)m_px[i];
                    triang[i, 1] = (double)m_py[i];
                }
                m_triangulos.Add(triang);

                if (poligono_externo) this.m_tipo_triangulos.Add(1.0);
                else m_tipo_triangulos.Add(-1.0);

                return false;
            }

            double max_y = (double)m_py[0];
            int ind_max_y = 0;
            for (int i = 1; i < m_py.Count; i++)
            {
                if ((double)m_py[i] > max_y)
                {
                    max_y = (double)m_py[i];
                    ind_max_y = i;
                }
            }

            triang[0, 0] = (double)m_px[ind_max_y];
            triang[0, 1] = (double)m_py[ind_max_y];

            if (ind_max_y > 0 && ind_max_y < m_py.Count - 1)
            {
                triang[1, 0] = (double)m_px[ind_max_y - 1];
                triang[1, 1] = (double)m_py[ind_max_y - 1];
                triang[2, 0] = (double)m_px[ind_max_y + 1];
                triang[2, 1] = (double)m_py[ind_max_y + 1];
            }
            else
            {
                if (ind_max_y == m_py.Count - 1)
                {
                    triang[1, 0] = (double)m_px[ind_max_y - 1];
                    triang[1, 1] = (double)m_py[ind_max_y - 1];
                    triang[2, 0] = (double)m_px[0];
                    triang[2, 1] = (double)m_py[0];
                }
                else
                {
                    triang[1, 0] = (double)m_px[ind_max_y + 1];
                    triang[1, 1] = (double)m_py[ind_max_y + 1];
                    triang[2, 0] = (double)m_px[m_py.Count - 1];
                    triang[2, 1] = (double)m_py[m_py.Count - 1];
                }
            }

            m_py.RemoveAt(ind_max_y);
            m_px.RemoveAt(ind_max_y);

            m_triangulos.Add(triang);

            if (poligono_externo) this.m_tipo_triangulos.Add(1.0);
            else m_tipo_triangulos.Add(-1.0);

            return true;
        }
    }
}
