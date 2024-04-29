using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Windows.Forms;
using SharpMap.Data;

namespace IpeaGeo.Modelagem.RotaOtima
{
    public partial class FormRotaOtimaDijkstra : Form
    {
        public FormRotaOtimaDijkstra()
        {
            InitializeComponent();
        }

        //Pasta com os dados
        public string strPastaDados;

        //Layer do mapa
        public SharpMap.Layers.VectorLayer mLayer;

        //Endereço do shapefile
        public string strEnderecoMapa;

        //Endereço da base
        public string strEnderecoBase;

        //Extensão da base
        public string strExtensao;

        //Variaveis no mapa
        public string[] strVariaveisMapa;

        public string appPath = Path.GetDirectoryName(Application.ExecutablePath);

        ArrayList m_nodes = new ArrayList();
        ArrayList m_vertices = new ArrayList();
        ArrayList m_linhas_in_shape = new ArrayList();
        
        public double Distancia(double long_x1, double lat_y1, double long_x2, double lat_y2)
        {
            double res = 0.0;
            
            double dlat = (lat_y2 - lat_y1)*Math.PI / 180.0;
            double dlon = (long_x2 - long_x1)*Math.PI / 180.0;
            double lat1 = lat_y1 * Math.PI / 180.0;
            double lat2 = lat_y2 * Math.PI / 180.0;

            double cr = Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(dlon);

            res = 6371 * Math.Acos(cr); // distância em kilômetros

            //double a = Math.Sin(dlat / 2.0) * Math.Sin(dlat / 2.0)
            //         + Math.Sin(dlon / 2.0) * Math.Sin(dlon / 2.0) * Math.Cos(lat1) * Math.Cos(lat2);
            //double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            //res = 6371.0 * c;

            //res = Math.Pow(Math.Pow(x1 - x2, 2.0) + Math.Pow(y1 - y2, 2.0), 0.5);

            return res;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                m_dt_ferrovias = new DataTable();
                m_dt_hidrovias = new DataTable();
                m_dt_rodovias = new DataTable();

                ImportaAtributos(ref m_dt_rodovias, "Rodovias_previsao_com_custos.xls");
                ImportaAtributos(ref m_dt_hidrovias, "Hidrovias_com_custos.xls");
                ImportaAtributos(ref m_dt_ferrovias, "Ferrovias_com_custos.xls");

                ImportaDadosPreparaGrafo();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private DataTable m_dt_rodovias = new DataTable();
        private DataTable m_dt_ferrovias = new DataTable();
        private DataTable m_dt_hidrovias = new DataTable();

        private DataTable m_dt_linhas_in_shape = new DataTable();

        private System.Data.OleDb.OleDbConnection m_cnn = new System.Data.OleDb.OleDbConnection();
        private System.Data.OleDb.OleDbDataAdapter m_dap = new System.Data.OleDb.OleDbDataAdapter();
        private DataSet dsDados = new DataSet();

        private void ImportaAtributos(ref DataTable dados, string arquivo)
        {
            string m_cnnstring = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source="
                + "C:\\Alex\\Pesquisa\\PortCompetition\\TransportationCosts\\DadosToPedro\\" + arquivo
                + ";Extended Properties=Excel 8.0;";
            m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
            m_cnn.Open();

            m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + "Plan1" + "]", m_cnn);
            m_dap.Fill(dsDados, "Table1");

            m_cnn.Close();

            dados = dsDados.Tables["Table1"];
        }

        private ArrayList m_nodes_ferrovias = new ArrayList();
        private ArrayList m_nodes_rodovias = new ArrayList();
        private ArrayList m_nodes_hidrovias = new ArrayList();

        private void ImportaDadosPreparaGrafo()
        {
            #region importando os dados de malhas

            string strEndereco = "C:\\Alex\\Pesquisa\\PortCompetition\\TransportationCosts\\DadosToPedro\\Rodovias_previsão.shp";
            SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("Rodovias_previsão");

            string strEndereco1 = "C:\\Alex\\Pesquisa\\PortCompetition\\TransportationCosts\\DadosToPedro\\Hidrovias.shp";
            SharpMap.Layers.VectorLayer layMapa1 = new SharpMap.Layers.VectorLayer("Hidrovias");

            string strEndereco2 = "C:\\Alex\\Pesquisa\\PortCompetition\\TransportationCosts\\DadosToPedro\\Ferrovias.shp";
            SharpMap.Layers.VectorLayer layMapa2 = new SharpMap.Layers.VectorLayer("Ferrovias");

            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);
            SharpMap.Data.Providers.ShapeFile shapefile1 = new SharpMap.Data.Providers.ShapeFile(strEndereco1);
            SharpMap.Data.Providers.ShapeFile shapefile2 = new SharpMap.Data.Providers.ShapeFile(strEndereco2);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();
            layMapa.DataSource = shapefile;

            shapefile1.Open();
            layMapa1.DataSource = shapefile1;

            shapefile2.Open();
            layMapa2.DataSource = shapefile2;

            SharpMap.Geometries.MultiCurve mcurva;
            SharpMap.Geometries.LinearRing linear_ring;
            SharpMap.Geometries.LineString line_string;

            SharpMap.Geometries.MultiCurve mcurva1;
            SharpMap.Geometries.LinearRing linear_ring1;
            SharpMap.Geometries.LineString line_string1;

            SharpMap.Geometries.MultiCurve mcurva2;
            SharpMap.Geometries.LinearRing linear_ring2;
            SharpMap.Geometries.LineString line_string2;

            double x, y, dist;

            x = 0.0;
            y = 0.0;

            double mindist = double.PositiveInfinity;
            double max_y = double.NegativeInfinity;
            double max_x = double.NegativeInfinity;
            double min_y = double.PositiveInfinity;
            double min_x = double.PositiveInfinity;

            int numero_poligonos = layMapa.DataSource.GetFeatureCount();
            int numero_poligonos1 = layMapa1.DataSource.GetFeatureCount();
            int numero_poligonos2 = layMapa2.DataSource.GetFeatureCount();

            int npontos = 0;

            FeatureDataRow feature;
            SharpMap.Geometries.Geometry geoMetria;

            FeatureDataRow feature1;
            SharpMap.Geometries.Geometry geoMetria1;

            FeatureDataRow feature2;
            SharpMap.Geometries.Geometry geoMetria2;

            #endregion

            #region preparando as listas de pontos

            ArrayList lista_x = new ArrayList();
            ArrayList lista_y = new ArrayList();
            ArrayList lista_modal = new ArrayList();

            ArrayList lista_r_x = new ArrayList();
            ArrayList lista_r_y = new ArrayList();

            ArrayList lista_h_x = new ArrayList();
            ArrayList lista_h_y = new ArrayList();

            ArrayList lista_f_x = new ArrayList();
            ArrayList lista_f_y = new ArrayList();

            Vertice vertice;

            double xant, yant, dist_vertice, custo_unitario, dist_total_in_shape, dist_total_calculado;
            string id_in_shape;

            m_dt_linhas_in_shape.Rows.Clear();

            DataRow dr;

            double lat_inicial = 0.0, lat_final = 0.0, long_inicial = 0.0, long_final = 0.0;

            int sequencial_poligonos = 0;

            for (int k = 0; k < numero_poligonos; k++)
            {
                custo_unitario = Convert.ToDouble(m_dt_rodovias.Rows[k]["custo_ajust_cgeral"]);
                id_in_shape = (m_dt_rodovias.Rows[k]["ID"]).ToString();
                dist_total_in_shape = Convert.ToDouble(m_dt_rodovias.Rows[k]["EXTENSAO"]);
                dist_total_calculado = 0.0;

                feature = layMapa.DataSource.GetFeature((uint)k);
                geoMetria = feature.Geometry;

                mcurva = geoMetria as SharpMap.Geometries.MultiCurve;
                linear_ring = geoMetria as SharpMap.Geometries.LinearRing;
                line_string = geoMetria as SharpMap.Geometries.LineString;

                if (line_string != null)
                {
                    for (int j = 0; j < line_string.NumPoints; j++)
                    {
                        xant = x;
                        yant = y;

                        x = line_string.Point(j).X;
                        y = line_string.Point(j).Y;

                        lista_x.Add(x);
                        lista_y.Add(y);
                        lista_modal.Add("rodovia");

                        lista_r_x.Add(x);
                        lista_r_y.Add(y);

                        if (j > 0)
                        {
                            dist_vertice = Distancia(xant, yant, x, y);
                            vertice = new Vertice(npontos - 1, npontos, "rodovia", dist_vertice);
                            vertice.CustoTotal = dist_vertice * custo_unitario;
                            vertice.CustoUnitario = custo_unitario;
                            vertice.VerticeIDinShape = id_in_shape;
                            vertice.DistTotalLinhaInShape = dist_total_in_shape;
                            vertice.SequencialArquivosShape = sequencial_poligonos;
                            this.m_vertices.Add(vertice);

                            dist_total_calculado += dist_vertice;

                            if (j == 1)
                            {
                                lat_inicial = x;
                                long_inicial = y;
                            }
                            if (j == line_string.NumPoints - 1)
                            {
                                lat_final = x;
                                long_final = y;
                            }
                        }

                        npontos++;
                    }

                    sequencial_poligonos++;
                }

                LinhaFromShape linha = new LinhaFromShape("rodovia", dist_total_in_shape, id_in_shape);
                linha.DistTotalCalculada = dist_total_calculado;
                m_linhas_in_shape.Add(linha);

                dr = m_dt_linhas_in_shape.NewRow();
                dr["IDinShape"] = id_in_shape;
                dr["TipoModal"] = "rodovia";
                dr["SequencialNosShapes"] = sequencial_poligonos - 1;
                dr["DistTotalInShape"] = dist_total_in_shape;
                dr["DistTotalCalculado"] = dist_total_calculado;
                dr["LatInicial"] = lat_inicial;
                dr["LatFinal"] = lat_final;
                dr["LongInicial"] = long_inicial;
                dr["LongFinal"] = long_final;
                m_dt_linhas_in_shape.Rows.Add(dr);
                m_dt_linhas_in_shape.AcceptChanges();
            }

            for (int k = 0; k < numero_poligonos1; k++)
            {
                custo_unitario = Convert.ToDouble(m_dt_hidrovias.Rows[k]["custo_ajust_cgeral"]);
                id_in_shape = (this.m_dt_hidrovias.Rows[k]["CODIGO_TRE"]).ToString();
                dist_total_in_shape = Convert.ToDouble(this.m_dt_hidrovias.Rows[k]["EXTENSAO"]);
                dist_total_calculado = 0.0;

                feature1 = layMapa1.DataSource.GetFeature((uint)k);
                geoMetria1 = feature1.Geometry;

                mcurva1 = geoMetria1 as SharpMap.Geometries.MultiCurve;
                linear_ring1 = geoMetria1 as SharpMap.Geometries.LinearRing;
                line_string1 = geoMetria1 as SharpMap.Geometries.LineString;

                if (line_string1 != null)
                {
                    for (int j = 0; j < line_string1.NumPoints; j++)
                    {
                        xant = x;
                        yant = y;

                        x = line_string1.Point(j).X;
                        y = line_string1.Point(j).Y;

                        lista_x.Add(x);
                        lista_y.Add(y);
                        lista_modal.Add("hidrovia");

                        lista_h_x.Add(x);
                        lista_h_y.Add(y);

                        if (j > 0)
                        {
                            dist_vertice = Distancia(xant, yant, x, y);
                            vertice = new Vertice(npontos - 1, npontos, "hidrovia", dist_vertice);
                            vertice.VerticeIDinShape = id_in_shape;
                            vertice.DistTotalLinhaInShape = dist_total_in_shape;
                            vertice.CustoTotal = dist_vertice * custo_unitario;
                            vertice.CustoUnitario = custo_unitario;
                            vertice.SequencialArquivosShape = sequencial_poligonos;
                            this.m_vertices.Add(vertice);

                            dist_total_calculado += dist_vertice;

                            if (j == 1)
                            {
                                lat_inicial = x;
                                long_inicial = y;
                            }
                            if (j == line_string1.NumPoints - 1)
                            {
                                lat_final = x;
                                long_final = y;
                            }
                        }

                        npontos++;
                    }

                    sequencial_poligonos++;
                }

                LinhaFromShape linha = new LinhaFromShape("hidrovia", dist_total_in_shape, id_in_shape);
                linha.DistTotalCalculada = dist_total_calculado;
                m_linhas_in_shape.Add(linha);

                dr = m_dt_linhas_in_shape.NewRow();
                dr["IDinShape"] = id_in_shape;
                dr["TipoModal"] = "hidrovia";
                dr["DistTotalInShape"] = dist_total_in_shape;
                dr["DistTotalCalculado"] = dist_total_calculado;
                dr["LatInicial"] = lat_inicial;
                dr["LatFinal"] = lat_final;
                dr["LongInicial"] = long_inicial;
                dr["LongFinal"] = long_final;
                dr["SequencialNosShapes"] = sequencial_poligonos - 1;
                m_dt_linhas_in_shape.Rows.Add(dr);
                m_dt_linhas_in_shape.AcceptChanges();
            }

            for (int k = 0; k < numero_poligonos2; k++)
            {
                custo_unitario = Convert.ToDouble(this.m_dt_ferrovias.Rows[k]["custo_ajust_cgeral"]);
                id_in_shape = (this.m_dt_ferrovias.Rows[k]["COD_PNV"]).ToString();
                dist_total_in_shape = Convert.ToDouble(this.m_dt_ferrovias.Rows[k]["EXTENSAO"]);
                dist_total_calculado = 0.0;

                feature2 = layMapa2.DataSource.GetFeature((uint)k);
                geoMetria2 = feature2.Geometry;

                mcurva2 = geoMetria2 as SharpMap.Geometries.MultiCurve;
                linear_ring2 = geoMetria2 as SharpMap.Geometries.LinearRing;
                line_string2 = geoMetria2 as SharpMap.Geometries.LineString;

                if (line_string2 != null)
                {
                    for (int j = 0; j < line_string2.NumPoints; j++)
                    {
                        xant = x;
                        yant = y;

                        x = line_string2.Point(j).X;
                        y = line_string2.Point(j).Y;

                        lista_x.Add(x);
                        lista_y.Add(y);
                        lista_modal.Add("ferrovia");

                        lista_f_x.Add(x);
                        lista_f_y.Add(y);

                        if (j > 0)
                        {
                            dist_vertice = Distancia(xant, yant, x, y);
                            vertice = new Vertice(npontos - 1, npontos, "ferrovia", dist_vertice);
                            vertice.CustoTotal = dist_vertice * custo_unitario;
                            vertice.CustoUnitario = custo_unitario;
                            vertice.DistTotalLinhaInShape = dist_total_in_shape;
                            vertice.VerticeIDinShape = id_in_shape;
                            vertice.SequencialArquivosShape = sequencial_poligonos;
                            this.m_vertices.Add(vertice);

                            dist_total_calculado += dist_vertice;

                            if (j == 1)
                            {
                                lat_inicial = x;
                                long_inicial = y;
                            }
                            if (j == line_string2.NumPoints - 1)
                            {
                                lat_final = x;
                                long_final = y;
                            }
                        }

                        npontos++;
                    }

                    sequencial_poligonos++;
                }

                LinhaFromShape linha = new LinhaFromShape("ferrovia", dist_total_in_shape, id_in_shape);
                linha.DistTotalCalculada = dist_total_calculado;
                m_linhas_in_shape.Add(linha);

                dr = m_dt_linhas_in_shape.NewRow();
                dr["IDinShape"] = id_in_shape;
                dr["TipoModal"] = "ferrovia";
                dr["DistTotalInShape"] = dist_total_in_shape;
                dr["DistTotalCalculado"] = dist_total_calculado;
                dr["LatInicial"] = lat_inicial;
                dr["LatFinal"] = lat_final;
                dr["LongInicial"] = long_inicial;
                dr["LongFinal"] = long_final;
                dr["SequencialNosShapes"] = sequencial_poligonos - 1;
                m_dt_linhas_in_shape.Rows.Add(dr);
                m_dt_linhas_in_shape.AcceptChanges();
            }

            // encontrando pontos que sao coincidentes para um mesmo modal
            double[] vx = new double[lista_x.Count];
            double[] vy = new double[lista_y.Count];
            string[] tipo_modal = new string[lista_y.Count];

            double tolerancia_rodovia = 0.001;
            double tolerancia_ferrovia = 0.002;
            double tolerancia_hidrovia = 0.003;

            for (int i = 0; i < vx.GetLength(0); i++)
            {
                vx[i] = Convert.ToDouble(lista_x[i]);
                vy[i] = Convert.ToDouble(lista_y[i]);
                tipo_modal[i] = lista_modal[i].ToString();
            }

            ArrayList lista_nos_intersecao_rodovia = new ArrayList();
            ArrayList lista_nos_intersecao_ferrovia = new ArrayList();
            ArrayList lista_nos_intersecao_hidrovia = new ArrayList();

            DateTime inicio_rodagem = DateTime.Now;

            for (int j = 0; j < lista_y.Count; j++)
            {
                if (vx[j] > max_x) max_x = vx[j];
                if (vy[j] > max_y) max_y = vy[j];
                if (vx[j] < min_x) min_x = vx[j];
                if (vy[j] < min_y) min_y = vy[j];

                for (int i = j + 1; i < lista_y.Count; i++)
                {
                    if (Math.Abs(vx[i] - vx[j]) < 0.1 && Math.Abs(vy[i] - vy[j]) < 0.1 && tipo_modal[i] == tipo_modal[j])
                    {
                        dist = Distancia(vx[i], vy[i], vx[j], vy[j]);

                        if (dist < tolerancia_rodovia && tipo_modal[i] == "rodovia")
                        {
                            if (!lista_nos_intersecao_rodovia.Contains(i)) lista_nos_intersecao_rodovia.Add(i);
                            if (!lista_nos_intersecao_rodovia.Contains(j)) lista_nos_intersecao_rodovia.Add(j);
                        }

                        if (dist < tolerancia_ferrovia && tipo_modal[i] == "ferrovia")
                        {
                            if (!lista_nos_intersecao_ferrovia.Contains(i)) lista_nos_intersecao_ferrovia.Add(i);
                            if (!lista_nos_intersecao_ferrovia.Contains(j)) lista_nos_intersecao_ferrovia.Add(j);
                        }

                        if (dist < tolerancia_hidrovia && tipo_modal[i] == "hidrovia")
                        {
                            if (!lista_nos_intersecao_hidrovia.Contains(i)) lista_nos_intersecao_hidrovia.Add(i);
                            if (!lista_nos_intersecao_hidrovia.Contains(j)) lista_nos_intersecao_hidrovia.Add(j);
                        }
                    }
                }
            }

            DateTime fim_rodagem = DateTime.Now;

            int tempo_execucao = (fim_rodagem - inicio_rodagem).Minutes;

            mindist = double.PositiveInfinity;

            #endregion
        }

        private void FormRotaOtimaDijkstra_Load(object sender, EventArgs e)
        {
            m_dt_linhas_in_shape.Columns.Add("SequencialNosShapes", typeof(int));
            m_dt_linhas_in_shape.Columns.Add("IDinShape", typeof(string));
            m_dt_linhas_in_shape.Columns.Add("TipoModal", typeof(string));
            m_dt_linhas_in_shape.Columns.Add("DistTotalInShape", typeof(double));
            m_dt_linhas_in_shape.Columns.Add("DistTotalCalculado", typeof(double));
            m_dt_linhas_in_shape.Columns.Add("LatInicial", typeof(double));
            m_dt_linhas_in_shape.Columns.Add("LongInicial", typeof(double));
            m_dt_linhas_in_shape.Columns.Add("LatFinal", typeof(double));
            m_dt_linhas_in_shape.Columns.Add("LongFinal", typeof(double));
        }
    }
}

