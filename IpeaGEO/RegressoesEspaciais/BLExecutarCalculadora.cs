using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Drawing;

namespace IpeaGeo.RegressoesEspaciais
{
    public class BLExecutarCalculadora
    {
        public BLExecutarCalculadora()
        {
        }
        
        private bool m_usa_W_sparsa_from_distancias = false;
        public bool UsaMatrizEsparsaFromDistancias
        {
            get
            {
                return m_usa_W_sparsa_from_distancias;
            }
            set
            {
                this.m_usa_W_sparsa_from_distancias = value;
            }
        }

        private clsMatrizEsparsa m_W_esparsa_from_distancias = new clsMatrizEsparsa();
        public clsMatrizEsparsa MatrizEsparsaFromDistancias
        {
            get
            {
                return m_W_esparsa_from_distancias;
            }
            set
            {
                m_W_esparsa_from_distancias = value;
            }
        }

        #region medidas para os polígonos no shape

        private clsTriangulosFromPoligonos m_tpol = new clsTriangulosFromPoligonos();

        private double Perimetro(ArrayList x, ArrayList y)
        {
            double res = 0.0;

            if (((double)x[0] != (double)x[x.Count - 1]) || ((double)y[0] != (double)y[y.Count - 1]))
            {
                x.Add((double)x[0]);
                y.Add((double)y[0]);
            }

            for (int i = 1; i < x.Count; i++)
            {
                res += Math.Sqrt(Math.Pow((double)x[i] - (double)x[i - 1], 2.0) + Math.Pow((double)y[i] - (double)y[i - 1], 2.0));
            }

            return res;
        }

        private void DefineTriangulosFromPoligonos(out double[] areas_poligonos, out double[] centroides_x, 
            out double[] centroides_y, out double[] perimetros, clsIpeaShape shape)
        {
            areas_poligonos = new double[shape.Count];
            centroides_x = new double[shape.Count];
            centroides_y = new double[shape.Count];
            perimetros = new double[shape.Count];

            ArrayList all_poligonos_x = new ArrayList();
            ArrayList all_poligonos_y = new ArrayList();

            ArrayList poligonos_x = new ArrayList();
            ArrayList poligonos_y = new ArrayList();
            clsIpeaPoligono pol = new clsIpeaPoligono();

            double[] x;
            double[] y;

            double[] x_aux;
            double[] y_aux;

            for (int i = 0; i < shape.Count; i++)
            {
                pol = shape[i];
                x = new double[pol.GetArrayX().GetLength(0) + 1];
                y = new double[pol.GetArrayY().GetLength(0) + 1];
                for (int j = 0; j < x.GetLength(0)-1; j++)
                {
                    x[j] = pol.GetArrayX()[j];
                    y[j] = pol.GetArrayY()[j];
                }
                x[x.GetLength(0) - 1] = double.NaN;
                y[y.GetLength(0) - 1] = double.NaN;

                poligonos_x.Clear();
                poligonos_y.Clear();

                all_poligonos_x.Clear();
                all_poligonos_y.Clear();

                for (int j = 0; j < x.GetLength(0); j++)
                {
                    if (!double.IsNaN(x[j]))
                    {
                        poligonos_x.Add(x[j]);
                        poligonos_y.Add(y[j]);
                    }
                    else
                    {
                        x_aux = new double[poligonos_x.Count];
                        y_aux = new double[poligonos_y.Count];
                        for (int k = 0; k < y_aux.GetLength(0); k++)
                        {
                            x_aux[k] = (double)poligonos_x[k];
                            y_aux[k] = (double)poligonos_y[k];
                        }

                        perimetros[i] += Perimetro(poligonos_x, poligonos_y);

                        poligonos_x.Clear();
                        poligonos_y.Clear();

                        all_poligonos_y.Add(y_aux);
                        all_poligonos_x.Add(x_aux);
                    }
                }

                m_tpol.ZeraTriangulos();
                for (int j = 0; j < all_poligonos_x.Count; j++)
                {
                    if (pol.TiposSubpoligonos[j] == "ExteriorRing")
                    {
                        m_tpol.AdicionaTriangulosFromPoligono((double[])all_poligonos_x[j], (double[])all_poligonos_y[j], true);
                    }
                    else
                    {
                        m_tpol.AdicionaTriangulosFromPoligono((double[])all_poligonos_x[j], (double[])all_poligonos_y[j], false);
                    }
                }

                areas_poligonos[i] = m_tpol.AreaTotal();
                centroides_x[i] = m_tpol.CentroidCoords()[0];
                centroides_y[i] = m_tpol.CentroidCoords()[1];
            }
        }

        public void GeraMatrizDistanciasPoligonos(DataTable dados, string variavel_id, clsIpeaShape shape, out DataTable dt_distancias)
        {
            double[] areas;
            double[] centx;
            double[] centy;
            double[] perimetros;

            DefineTriangulosFromPoligonos(out areas, out centx, out centy, out perimetros, shape);

            dt_distancias = new DataTable();
            dt_distancias.Columns.Add(variavel_id + "_1", dados.Columns[variavel_id].DataType);
            dt_distancias.Columns.Add(variavel_id + "_2", dados.Columns[variavel_id].DataType);
            dt_distancias.Columns.Add("Distancias", typeof(double));

            clsUtilTools clt = new clsUtilTools();

            for (int i = 0; i < dados.Rows.Count * dados.Rows.Count; i++)
            {
                dt_distancias.Rows.Add();
            }

            int irow = 0;
            for (int i = 0; i < dados.Rows.Count; i++)
            {
                for (int j = 0; j < dados.Rows.Count; j++)
                {
                    dt_distancias.Rows[irow][variavel_id + "_1"] = dados.Rows[i][variavel_id];
                    dt_distancias.Rows[irow][variavel_id + "_2"] = dados.Rows[j][variavel_id];
                    dt_distancias.Rows[irow]["Distancias"] = Math.Sqrt(Math.Pow(centx[j] - centx[i], 2.0) + Math.Pow(centy[j] - centy[i], 2.0));
                    irow++;
                }
            }
        }

        public void AdicionaMedidasPoligonos(DataTable dados, clsIpeaShape shape, bool adiciona_area, bool adiciona_centroides, bool adiciona_perimetro, 
            bool adiciona_distancia, bool adiciona_bounding_box, string variavel_selecao, object valor_selecao, int[] filtro)
        {
            double[] areas;
            double[] centx;
            double[] centy;
            double[] perimetros;

            double[] minX = new double[shape.Count];
            double[] minY = new double[shape.Count];
            double[] maxX = new double[shape.Count];
            double[] maxY = new double[shape.Count];

            DefineTriangulosFromPoligonos(out areas, out centx, out centy, out perimetros, shape);

            if (adiciona_bounding_box)
            {
                for (int i = 0; i < shape.Count; i++)
                {
                    maxX[i] = shape[i].BoundingBoxXMax;
                    minX[i] = shape[i].BoundingBoxXMin;
                    minY[i] = shape[i].BoundingBoxYMin;
                    maxY[i] = shape[i].BoundingBoxYMax;
                }
            }

            if (adiciona_area && !dados.Columns.Contains("Areas_poligonos")) dados.Columns.Add("Areas_poligonos", typeof(double));
            if (adiciona_centroides && !dados.Columns.Contains("Centroides_x_poligonos")) dados.Columns.Add("Centroides_x_poligonos", typeof(double));
            if (adiciona_centroides && !dados.Columns.Contains("Centroides_y_poligonos")) dados.Columns.Add("Centroides_y_poligonos", typeof(double));
            if (adiciona_perimetro && !dados.Columns.Contains("Perimetros_poligonos")) dados.Columns.Add("Perimetros_poligonos", typeof(double));

            if (adiciona_bounding_box && !dados.Columns.Contains("BoundingBox_minX")) dados.Columns.Add("BoundingBox_minX", typeof(double));
            if (adiciona_bounding_box && !dados.Columns.Contains("BoundingBox_minY")) dados.Columns.Add("BoundingBox_minY", typeof(double));
            if (adiciona_bounding_box && !dados.Columns.Contains("BoundingBox_maxX")) dados.Columns.Add("BoundingBox_maxX", typeof(double));
            if (adiciona_bounding_box && !dados.Columns.Contains("BoundingBox_maxY")) dados.Columns.Add("BoundingBox_maxY", typeof(double));

            for (int i = 0; i < dados.Rows.Count; i++)
            {
                if (filtro[i] > 0)
                {
                    if (adiciona_area) dados.Rows[i]["Areas_poligonos"] = areas[i];
                    if (adiciona_centroides)
                    {
                        dados.Rows[i]["Centroides_x_poligonos"] = centx[i];
                        dados.Rows[i]["Centroides_y_poligonos"] = centy[i];
                    }
                    if (adiciona_perimetro) dados.Rows[i]["Perimetros_poligonos"] = perimetros[i];
                    if (adiciona_bounding_box)
                    {
                        dados.Rows[i]["BoundingBox_minX"] = minX[i];
                        dados.Rows[i]["BoundingBox_maxX"] = maxX[i];
                        dados.Rows[i]["BoundingBox_minY"] = minY[i];
                        dados.Rows[i]["BoundingBox_maxY"] = maxY[i];
                    }
                }
            }

            if (adiciona_distancia)
            {
                int obs_foco = 0;
                for (int i = 0; i < dados.Rows.Count; i++)
                {
                    if (dados.Rows[i][variavel_selecao].ToString() == valor_selecao.ToString())
                    {
                        obs_foco = i;
                        break;
                    }
                }

                if (!dados.Columns.Contains("Distancias_ao_poligono_" + (obs_foco + 1).ToString()))
                {
                    dados.Columns.Add("Distancias_ao_poligono_" + (obs_foco + 1).ToString(), typeof(double));
                }

                double x_foco = centx[obs_foco];
                double y_foco = centy[obs_foco];

                double[] dists = new double[shape.Count];
                for (int i = 0; i < shape.Count; i++)
                {
                    if (filtro[i] > 0)
                    {
                        dists[i] = Math.Sqrt(Math.Pow(x_foco - centx[i], 2.0) + Math.Pow(y_foco - centy[i], 2.0));
                        dados.Rows[i]["Distancias_ao_poligono_" + (obs_foco + 1).ToString()] = dists[i];
                    }
                }
            }
        }

        #endregion

        #region definição dos filtros

        public int[] DefinicaoObservacoesFiltradas(DataTable dados, string variavel, string tipo_variavel, 
            string relacao, object valor1, object valor2)
        {
            int[] res = new int[dados.Rows.Count];
            double v;
            DateTime vd;
            string vs;
            switch (tipo_variavel)
            {
                case "texto":
                    string vs1 = Convert.ToString(valor1);
                    string vs2 = Convert.ToString(valor2);
                    switch (relacao)
                    {
                        case "Igual (=)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if (Convert.ToString(dados.Rows[i][variavel]).CompareTo(vs1) == 0) res[i] = 1;
                            }
                            break;
                        case "Menor (<)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if (Convert.ToString(dados.Rows[i][variavel]).CompareTo(vs1) < 0) res[i] = 1;
                            }
                            break;
                        case "Menor ou igual (<=)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if (Convert.ToString(dados.Rows[i][variavel]).CompareTo(vs1) <= 0) res[i] = 1;
                            }
                            break;
                        case "Maior (>)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if (Convert.ToString(dados.Rows[i][variavel]).CompareTo(vs1) > 0) res[i] = 1;
                            }
                            break;
                        case "Maior or igual (>=)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if (Convert.ToString(dados.Rows[i][variavel]).CompareTo(vs1) >= 0) res[i] = 1;
                            }
                            break;
                        case "Entre (inclusive)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                vs = Convert.ToString(dados.Rows[i][variavel]);
                                if (vs.CompareTo(vs1) >= 0 && vs.CompareTo(vs2) <= 0) res[i] = 1;
                            }
                            break;
                        case "Entre (exclusive)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                vs = Convert.ToString(dados.Rows[i][variavel]);
                                if (vs.CompareTo(vs1) > 0 && vs.CompareTo(vs2) < 0) res[i] = 1;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "data":
                    DateTime vd1 = Convert.ToDateTime(valor1);
                    DateTime vd2 = Convert.ToDateTime(valor2);
                    switch (relacao)
                    {
                        case "Igual (=)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if (Convert.ToDateTime(dados.Rows[i][variavel]).CompareTo(vd1) == 0) res[i] = 1;
                            }
                            break;
                        case "Menor (<)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if (Convert.ToDateTime(dados.Rows[i][variavel]).CompareTo(vd1) < 0) res[i] = 1;
                            }
                            break;
                        case "Menor ou igual (<=)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if (Convert.ToDateTime(dados.Rows[i][variavel]).CompareTo(vd1) <= 0) res[i] = 1;
                            }
                            break;
                        case "Maior (>)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if (Convert.ToDateTime(dados.Rows[i][variavel]).CompareTo(vd1) > 0) res[i] = 1;
                            }
                            break;
                        case "Maior or igual (>=)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if (Convert.ToDateTime(dados.Rows[i][variavel]).CompareTo(vd1) >= 0) res[i] = 1;
                            }
                            break;
                        case "Entre (inclusive)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                vd = Convert.ToDateTime(dados.Rows[i][variavel]);
                                if (vd.CompareTo(vd1) >= 0 && vd.CompareTo(vd2) <= 0) res[i] = 1;
                            }
                            break;
                        case "Entre (exclusive)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                vd = Convert.ToDateTime(dados.Rows[i][variavel]);
                                if (vd.CompareTo(vd1) > 0 && vd.CompareTo(vd2) < 0) res[i] = 1;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    double v1 = (double)valor1;
                    double v2 = (double)valor2;
                    switch (relacao)
                    {
                        case "Igual (=)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if ((double)dados.Rows[i][variavel] == v1) res[i] = 1;
                            }               
                            break;
                        case "Menor (<)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if ((double)dados.Rows[i][variavel] < v1) res[i] = 1;
                            }
                            break;
                        case "Menor ou igual (<=)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if ((double)dados.Rows[i][variavel] <= v1) res[i] = 1;
                            }
                            break;
                        case "Maior (>)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if ((double)dados.Rows[i][variavel] > v1) res[i] = 1;
                            }
                            break;
                        case "Maior or igual (>=)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                if ((double)dados.Rows[i][variavel] >= v1) res[i] = 1;
                            }
                            break;
                        case "Entre (inclusive)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                v = (double)dados.Rows[i][variavel];
                                if (v >= v1 && v <= v2) res[i] = 1;
                            }
                            break;
                        case "Entre (exclusive)":
                            for (int i = 0; i < res.GetLength(0); i++)
                            {
                                v = (double)dados.Rows[i][variavel];
                                if (v > v1 && v < v2) res[i] = 1;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
            }
            return res;
        }

        #endregion

        #region Matriz de vizinhança

        private clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            get { return this.m_shape; }
            set
            {
            	m_shape = value;
            }
        }

        private clsLinearRegressionModelsMLE m_geomle = new clsLinearRegressionModelsMLE();

        public void GeraMatrizVizinhanca(clsIpeaShape shape)
        {
            m_geomle.Shape = shape;
            m_geomle.MatrizWesparsaFromVizinhosNorm();
        }

        #endregion

        #region funções bivariadas

        public void ExecutarFuncaoBivariada(DataTable dados, string funcao, double valor1, double valor2, 
            string nome_nova_variavel_in, int[] filtro)
        {
            string nome_nova_variavel = nome_nova_variavel_in.Trim();

            if (nome_nova_variavel.Length <= 1) throw new Exception("Nome para a nova variável é inválido.");

            bool inclui_variavel = true;
            for (int i = 0; i < dados.Columns.Count; i++)
            {
                if (dados.Columns[i].ColumnName == nome_nova_variavel)
                {
                    clsUtilTools clt = new clsUtilTools();
                    if (EstaNaLista(clt.RetornaColunasNumericas(dados), nome_nova_variavel))
                    {
                        inclui_variavel = false;
                    }
                    else
                    {
                        throw new Exception("Já existe uma variável não numérica com o mesmo nome da nova variável.");
                    }
                    break;
                }
            }
            if (inclui_variavel)
            {
                dados.Columns.Add(nome_nova_variavel, typeof(double));
                for (int i = 0; i < dados.Rows.Count; i++)
                {
                    dados.Rows[i][nome_nova_variavel] = 0.0;
                }
            }

            double[] v1 = new double[dados.Rows.Count];
            double[] v2 = new double[dados.Rows.Count];
            for (int i = 0; i < dados.Rows.Count; i++)
            {
                v1[i] = valor1;
                v2[i] = valor2;
            }

            switch (funcao)
            {
                case "Soma":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = v1[i] + v2[i];
                        }
                    }
                    break;
                case "Subtração":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = v1[i] - v2[i];
                        }
                    }
                    break;
                case "Divisão":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            if (v2[i] != 0.0) dados.Rows[i][nome_nova_variavel] = v1[i] / v2[i];
                            else dados.Rows[i][nome_nova_variavel] = double.NaN;
                        }
                    }
                    break;
                case "Multiplicação":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = v1[i] * v2[i];
                        }
                    }
                    break;
                case "Potência":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Pow(v1[i], v2[i]);
                        }
                    }
                    break;
                case "Máximo":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Max(v1[i], v2[i]);
                        }
                    }
                    break;
                case "Mínimo":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Min(v1[i], v2[i]);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void ExecutarFuncaoBivariada(DataTable dados, string funcao, double valor1, string var2, 
            string nome_nova_variavel_in, int[] filtro)
        {
            string nome_nova_variavel = nome_nova_variavel_in.Trim();

            if (nome_nova_variavel.Length <= 1) throw new Exception("Nome para a nova variável é inválido.");

            bool inclui_variavel = true;
            for (int i = 0; i < dados.Columns.Count; i++)
            {
                if (dados.Columns[i].ColumnName == nome_nova_variavel)
                {
                    clsUtilTools clt = new clsUtilTools();
                    if (EstaNaLista(clt.RetornaColunasNumericas(dados), nome_nova_variavel))
                    {
                        inclui_variavel = false;
                    }
                    else
                    {
                        throw new Exception("Já existe uma variável não numérica com o mesmo nome da nova variável.");
                    }
                    break;
                }
            }
            if (inclui_variavel)
            {
                dados.Columns.Add(nome_nova_variavel, typeof(double));
                for (int i = 0; i < dados.Rows.Count; i++)
                {
                    dados.Rows[i][nome_nova_variavel] = 0.0;
                }
            }

            double[] v1 = new double[dados.Rows.Count];
            double[] v2 = new double[dados.Rows.Count];
            for (int i = 0; i < dados.Rows.Count; i++)
            {
                v2[i] = Convert.ToDouble(dados.Rows[i][var2]);
                v1[i] = valor1;
            }

            switch (funcao)
            {
                case "Soma":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = v1[i] + v2[i];
                        }
                    }
                    break;
                case "Subtração":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = v1[i] - v2[i];
                        }
                    }
                    break;
                case "Divisão":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            if (v2[i] != 0.0) dados.Rows[i][nome_nova_variavel] = v1[i] / v2[i];
                            else dados.Rows[i][nome_nova_variavel] = double.NaN;
                        }
                    }
                    break;
                case "Multiplicação":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = v1[i] * v2[i];
                        }
                    }
                    break;
                case "Potência":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Pow(v1[i], v2[i]);
                        }
                    }
                    break;
                case "Máximo":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Max(v1[i], v2[i]);
                        }
                    }
                    break;
                case "Mínimo":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Min(v1[i], v2[i]);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void ExecutarFuncaoBivariada(DataTable dados, string funcao, string var1, 
            double valor2, string nome_nova_variavel_in, int[] filtro)
        {
            string nome_nova_variavel = nome_nova_variavel_in.Trim();

            if (nome_nova_variavel.Length <= 1) throw new Exception("Nome para a nova variável é inválido.");

            bool inclui_variavel = true;
            for (int i = 0; i < dados.Columns.Count; i++)
            {
                if (dados.Columns[i].ColumnName == nome_nova_variavel)
                {
                    clsUtilTools clt = new clsUtilTools();
                    if (EstaNaLista(clt.RetornaColunasNumericas(dados), nome_nova_variavel))
                    {
                        inclui_variavel = false;
                    }
                    else
                    {
                        throw new Exception("Já existe uma variável não numérica com o mesmo nome da nova variável.");
                    }
                    break;
                }
            }
            if (inclui_variavel)
            {
                dados.Columns.Add(nome_nova_variavel, typeof(double));
                for (int i = 0; i < dados.Rows.Count; i++)
                {
                    dados.Rows[i][nome_nova_variavel] = 0.0;
                }
            }

            double[] v1 = new double[dados.Rows.Count];
            double[] v2 = new double[dados.Rows.Count];
            for (int i = 0; i < dados.Rows.Count; i++)
            {
                v1[i] = Convert.ToDouble(dados.Rows[i][var1]);
                v2[i] = valor2;
            }

            switch (funcao)
            {
                case "Soma":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = v1[i] + v2[i];
                        }
                    }
                    break;
                case "Subtração":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = v1[i] - v2[i];
                        }
                    }
                    break;
                case "Divisão":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            if (v2[i] != 0.0) dados.Rows[i][nome_nova_variavel] = v1[i] / v2[i];
                            else dados.Rows[i][nome_nova_variavel] = double.NaN;
                        }
                    }
                    break;
                case "Multiplicação":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = v1[i] * v2[i];
                        }
                    }
                    break;
                case "Potência":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Pow(v1[i], v2[i]);
                        }
                    }
                    break;
                case "Máximo":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Max(v1[i], v2[i]);
                        }
                    }
                    break;
                case "Mínimo":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Min(v1[i], v2[i]);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void ExecutarFuncaoBivariada(DataTable dados, string funcao, string var1, string var2, 
            string nome_nova_variavel_in, int[] filtro)
        {
            string nome_nova_variavel = nome_nova_variavel_in.Trim();

            if (nome_nova_variavel.Length <= 1) throw new Exception("Nome para a nova variável é inválido.");

            bool inclui_variavel = true;
            for (int i = 0; i < dados.Columns.Count; i++)
            {
                if (dados.Columns[i].ColumnName == nome_nova_variavel)
                {
                    clsUtilTools clt = new clsUtilTools();
                    if (EstaNaLista(clt.RetornaColunasNumericas(dados), nome_nova_variavel))
                    {
                        inclui_variavel = false;
                    }
                    else
                    {
                        throw new Exception("Já existe uma variável não numérica com o mesmo nome da nova variável.");
                    }
                    break;
                }
            }
            if (inclui_variavel)
            {
                dados.Columns.Add(nome_nova_variavel, typeof(double));
                for (int i = 0; i < dados.Rows.Count; i++)
                {
                    dados.Rows[i][nome_nova_variavel] = 0.0;
                }
            }

            double[] v1 = new double[dados.Rows.Count];
            double[] v2 = new double[dados.Rows.Count];
            for (int i = 0; i < dados.Rows.Count; i++)
            {
                v1[i] = Convert.ToDouble(dados.Rows[i][var1]);
                v2[i] = Convert.ToDouble(dados.Rows[i][var2]);
            }

            switch (funcao)
            {
                case "Soma":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = v1[i] + v2[i];
                        }
                    }
                    break;
                case "Subtração":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = v1[i] - v2[i];
                        }
                    }
                    break;
                case "Divisão":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            if (v2[i] != 0.0) dados.Rows[i][nome_nova_variavel] = v1[i] / v2[i];
                            else dados.Rows[i][nome_nova_variavel] = double.NaN;
                        }
                    }
                    break;
                case "Multiplicação":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = v1[i] * v2[i];
                        }
                    }
                    break;
                case "Potência":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Pow(v1[i], v2[i]);
                        }
                    }
                    break;
                case "Máximo":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Max(v1[i], v2[i]);
                        }
                    }
                    break;
                case "Mínimo":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Min(v1[i], v2[i]);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region funções univariadas

        private bool EstaNaLista(string[] vs, string s)
        {
            for (int i = 0; i < vs.GetLength(0); i++)
            {
                if (vs[i] == s) return true;
            }
            return false;
        }

        public void ExecutarFuncaoUnivariada(DataTable dados, double valor, string nome_nova_variavel_in, int[] filtro)
        {
            string nome_nova_variavel = nome_nova_variavel_in.Trim();

            if (nome_nova_variavel.Length <= 1) throw new Exception("Nome para a nova variável é inválido.");

            bool inclui_variavel = true;
            for (int i = 0; i < dados.Columns.Count; i++)
            {
                if (dados.Columns[i].ColumnName == nome_nova_variavel)
                {
                    clsUtilTools clt = new clsUtilTools();
                    if (EstaNaLista(clt.RetornaColunasNumericas(dados), nome_nova_variavel))
                    {
                        inclui_variavel = false;
                    }
                    else
                    {
                        throw new Exception("Já existe uma variável não numérica com o mesmo nome da nova variável.");
                    }
                    break;
                }
            }
            if (inclui_variavel)
            {
                dados.Columns.Add(nome_nova_variavel, typeof(double));
                for (int i = 0; i < dados.Rows.Count; i++)
                {
                    dados.Rows[i][nome_nova_variavel] = 0.0;
                }
            }

            for (int i = 0; i < dados.Rows.Count; i++)
            {
                if (filtro[i] == 1)
                {
                    dados.Rows[i][nome_nova_variavel] = valor;
                }
            }
        }

        public void ExecutarFuncaoUnivariada(DataTable dados, string funcao, string variavel, string nome_nova_variavel_in, int[] filtro)
        {
            string nome_nova_variavel = nome_nova_variavel_in.Trim();

            if (nome_nova_variavel.Length <= 1) throw new Exception("Nome para a nova variável é inválido.");

            bool inclui_variavel = true;
            for (int i = 0; i < dados.Columns.Count; i++)
            {
                if (dados.Columns[i].ColumnName == nome_nova_variavel)
                {
                    clsUtilTools clt = new clsUtilTools();
                    if (EstaNaLista(clt.RetornaColunasNumericas(dados), nome_nova_variavel))
                    {
                        inclui_variavel = false;
                    }
                    else
                    {
                        throw new Exception("Já existe uma variável não numérica com o mesmo nome da nova variável.");
                    }
                    break;
                }
            }
            if (inclui_variavel)
            {
                dados.Columns.Add(nome_nova_variavel, typeof(double));
                for (int i = 0; i < dados.Rows.Count; i++)
                {
                    dados.Rows[i][nome_nova_variavel] = 0.0;
                }
            }

            double[,] v = new double[dados.Rows.Count, 1];
            for (int i = 0; i < dados.Rows.Count; i++ )
            {
                v[i,0] = Convert.ToDouble(dados.Rows[i][variavel]);
            }
            int a;
            switch (funcao)
            {
                case "Logaritmo":
                    for (int i = 0; i < v.GetLength(0); i++)
                    {
                        if (filtro[i] == 1)
                        {
                            if (v[i, 0] > 0.0) dados.Rows[i][nome_nova_variavel] = Math.Log(v[i, 0]);
                            else dados.Rows[i][nome_nova_variavel] = double.NaN;
                        }
                    }
                    break;
                case "Logaritmo10":
                    for (int i = 0; i < v.GetLength(0); i++)
                    {
                        if (filtro[i] == 1)
                        {
                            if (v[i, 0] > 0.0) dados.Rows[i][nome_nova_variavel] = Math.Log10(v[i, 0]);
                            else dados.Rows[i][nome_nova_variavel] = double.NaN;
                        }
                    }
                    break;
                case "Exponencial":
                    for (int i = 0; i < v.GetLength(0); i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Exp(v[i, 0]);
                        }
                    }
                    break;
                case "Raiz quadrada":
                    for (int i = 0; i < v.GetLength(0); i++)
                    {
                        if (filtro[i] == 1)
                        {
                            if (v[i, 0] > 0.0) dados.Rows[i][nome_nova_variavel] = Math.Sqrt(v[i, 0]);
                            else dados.Rows[i][nome_nova_variavel] = double.NaN;
                        }
                    }
                    break;
                case "Somatório":
                    double soma = 0;
                    for (int i = 0; i < v.GetLength(0); i++)
                    {
                        if (filtro[i] == 1)
                        {   
                            soma = soma + v[i,0];                            
                        }
                    }
                    
                    for (int h = 0; h < v.GetLength(0); h++)
                    {
                    //    if (h == 0)
                    //    {
                            dados.Rows[h][nome_nova_variavel] = soma;
                        //}
                        //else 
                        //{
                        //    dados.Rows[h][nome_nova_variavel] = DBNull.Value;
                        //}
                    }
                    break;
                case "Quadrado":
                    for (int i = 0; i < v.GetLength(0); i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Pow(v[i, 0], 2.0);
                        }
                    }
                    break;
                case "Cubo":
                    for (int i = 0; i < v.GetLength(0); i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Pow(v[i, 0], 3.0);
                        }
                    }
                    break;
                case "Valor absoluto":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Abs(v[i, 0]);
                        }
                    }
                    break;
                case "Seno":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Sin(v[i, 0]);
                        }
                    }
                    break;
                case "Cosseno":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Cos(v[i, 0]);
                        }
                    }
                    break;
                case "Tangente":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Tan(v[i, 0]);
                        }
                    }
                    break;
                case "Arccosseno":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Acos(v[i, 0]);
                        }
                    }
                    break;
                case "Arcseno":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Asin(v[i, 0]);
                        }
                    }
                    break;
                case "Arctangente":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Atan(v[i, 0]);
                        }
                    }
                    break;
                case "Cosseno hiperbólico":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Cosh(v[i, 0]);
                        }
                    }
                    break;
                case "Seno hiperbólico":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Sinh(v[i, 0]);
                        }
                    }
                    break;
                case "Tangente hiperbólica":
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = Math.Tanh(v[i, 0]);
                        }
                    }
                    break;
                case "Lag espacial":
                    clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
                    double[,] rs;
                    if (!this.UsaMatrizEsparsaFromDistancias)
                    {
                        rs = fme.MatrizMult(m_geomle.Wesparsa, v);
                    }
                    else
                    {
                        rs = fme.MatrizMult(m_W_esparsa_from_distancias, v);
                    }
                    for (int i = 0; i < v.GetLength(0); i++)
                    {
                        if (filtro[i] == 1)
                        {
                            dados.Rows[i][nome_nova_variavel] = rs[i, 0];
                        }
                    }
                    break;
                case "Transformação Box-Cox":
                    if (!UsaLambdaOtimoBoxCox)
                    {
                        for (int i = 0; i < dados.Rows.Count; i++)
                        {
                            if (filtro[i] == 1)
                            {
                                dados.Rows[i][nome_nova_variavel] = TransformacaoBoxCox(v[i, 0], m_parametro_funcao_univariada);
                            }
                        }
                    }
                    else
                    {
                        double corr_max = 0.0;
                        double lambda_otimo = this.LambdaOtimoBoxCox(v, -5.0, 5.0, ref corr_max);
                        for (int i = 0; i < dados.Rows.Count; i++)
                        {
                            if (filtro[i] == 1)
                            {
                                dados.Rows[i][nome_nova_variavel] = TransformacaoBoxCox(v[i, 0], lambda_otimo);
                            }
                        }                        
                        this.m_parametro_funcao_univariada = lambda_otimo;
                    }
                    break;
                default:
                    break;
            }
        }

        private bool m_usa_lamdda_otimo = false;
        public bool UsaLambdaOtimoBoxCox
        {
            set { this.m_usa_lamdda_otimo = value; }
            get { return this.m_usa_lamdda_otimo; }
        }

        private double m_parametro_funcao_univariada = 0.0;
        public double ParametroFuncaoUnivariada
        {
            get { return this.m_parametro_funcao_univariada; }
            set { this.m_parametro_funcao_univariada = value; }
        }

        private double TransformacaoBoxCox(double x, double lambda)
        {
            if (lambda == 0.0)
            {
                return Math.Log(x);
            }
            return (Math.Pow(x, lambda) - 1.0) / lambda;
        }

        public double LambdaOtimoBoxCox(double[,] v, double min_lambda, double max_lambda, ref double corr_max)
        {
            int npassos = 400;
            double delta = (max_lambda - min_lambda) / ((double)npassos - 1.0);
            double max_corr = -1.0;
            double lambda_otimo = 0.0;
            double lambda = 0.0;
            double corr = 0.0;
            for (int i = 0; i < npassos; i++)
            {
                lambda = min_lambda + ((double)i) * delta;
                corr = ObjFunctionLambdaBoxCox(v, lambda);
                if (corr > max_corr)
                {
                    max_corr = corr;
                    lambda_otimo = lambda;
                }
            }
            corr_max = max_corr;
            return lambda_otimo;
        }

        private MathNormaldist m_norm = new MathNormaldist();
        private double ObjFunctionLambdaBoxCox(double[,] v, double lambda)
        {
            int n = v.GetLength(0);
            double[,] x = new double[n, 1];
            for (int i = 0; i < n; i++) x[i, 0] = TransformacaoBoxCox(v[i, 0], lambda);
            clsUtilTools clt = new clsUtilTools();
            double[,] xs = clt.Standardizec(x);
            ArrayList ax = new ArrayList();
            double[,] m = new double[n,1];
            for (int i = 0; i < n; i++) 
            {
                if (i > 0 && i < n-1)
                {
                    m[i,0] = ((double)i + 1.0 - 0.3175) / ((double)n + 0.365);
                }
                if (i == n-1) m[i,0] = Math.Pow(0.5, 1.0 / ((double)n));
                ax.Add(xs[i, 0]);
            }
            m[0,0] = 1.0 - m[n-1, 0];
            ax.Sort();

            double[,] dados = new double[n, 2];
            for (int i = 0; i < n; i++)
            {
                dados[i, 0] = Convert.ToDouble(ax[i]);
                dados[i, 1] = m_norm.invcdf(m[i, 0]);
            }

            double[,] media = clt.Meanc(dados);
            double[,] desvpad = clt.Despadc(dados);
            double soma = 0.0;
            for (int i = 0; i < n; i++)
            {
                soma += (dados[i, 0] - media[0, 0]) * (dados[i, 1] - media[0, 1]);
            }
            soma = soma / (double)n;
                        
            return soma / (desvpad[0,0] * desvpad[0,1]);
        }

        #endregion

        #region exclusão de variáveis

        public void ExcluiVariaveis(DataTable dados, string[] variaveis)
        {
            for (int i = 0; i < variaveis.GetLength(0); i++) dados.Columns.Remove(variaveis[i]);
        }

        #endregion
    }
}
