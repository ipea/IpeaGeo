using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using IpeaGeo.RegressoesEspaciais;
using SharpMap;
using SharpMap.Data;
using SharpMap.Geometries;

namespace IpeaGeo
{
    [Obsolete("To be reimplemented in IpeaGEO.GIS.Map")]
    public class clsMapa
    {
        public void Preenche_Distancias_Poligonos(ref clsIpeaShape shapeAlex)
        {
            shapeAlex.TipoDistancia = true;
            clsAreaPerimetroCentroide cap = new clsAreaPerimetroCentroide();

            double[,] distancias = new double[shapeAlex.Count, shapeAlex.Count];

            for (int i = 0; i < shapeAlex.Count; i++)
                for (int j = i + 1; j < shapeAlex.Count; j++)
                {
                    distancias[i, j] = cap.distancia(shapeAlex[i].YCentroide, shapeAlex[i].XCentroide, shapeAlex[j].YCentroide, shapeAlex[j].XCentroide, false);
                    distancias[j, i] = distancias[i, j];
                } // for

            for (int i = 0; i < shapeAlex.Count; i++)
            {
                shapeAlex[i].Determine_Nobs(shapeAlex.Count);
                for (int j = 0; j < shapeAlex.Count; j++)
                    shapeAlex[i].AddListaDistancias(j, distancias[i, j]);
            } // for
        } // Preenche_Distancias_Poligonos()

        #region Mudanças Alexandre

        private clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shapes
        {
            get { return this.m_shape; }
        }

        private SharpMap.Map m_mapa;
        public SharpMap.Map Sharp_Mapa
        {
            get { return this.m_mapa; }
        }

        private DataTable m_table_dados = new DataTable();

        public DataTable TabelaDados
        {
            get { return this.m_table_dados; }
        }

        private clsIpeaShape[] m_vector_shapes = new clsIpeaShape[0];
        public clsIpeaShape[] VetorShapes
        {
            get { return this.m_vector_shapes; }
        }

        private System.Data.OleDb.OleDbConnection m_cnn = new System.Data.OleDb.OleDbConnection();
        private System.Data.OleDb.OleDbDataAdapter m_dap = new System.Data.OleDb.OleDbDataAdapter();

        #endregion

        #region Funções Básicas

        [Obsolete("Check if it is necessary.")]
        private void area(ref clsIpeaPoligono poligono)
        {
            double area = 0;
            double[] X = new double[poligono.Count + 1];
            double[] Y = new double[poligono.Count + 1];
            for (int i = 0; i < poligono.Count; i++)
            {
                X[i] = poligono.X(i);
                Y[i] = poligono.Y(i);
            } // for

            X[poligono.Count] = poligono.X(0);
            Y[poligono.Count] = poligono.Y(0);

            for (int i = 0; i < poligono.Count; i++)
                area += (X[i] * Y[i + 1]) - (X[i + 1] * Y[i]);
            
            poligono.Area = (area / 2.0);
        } // area()

        [Obsolete("Check if it is necessary.")]
        private void centroid(ref clsIpeaPoligono poligono, double area)
        {
            double centroideX = 0;
            double centroideY = 0;
            double X0 = poligono.X(0);
            double Y0 = poligono.X(0);
            for (int i = 0; i < poligono.Count; i++)
            {
                centroideX += (poligono.X(i) + poligono.X(i + 1)) * ((poligono.X(i) * poligono.Y(i + 1)) - (poligono.X(i + 1) * poligono.Y(i)));
                centroideY += (poligono.Y(i) + poligono.Y(i + 1)) * ((poligono.X(i) * poligono.Y(i + 1)) - (poligono.X(i + 1) * poligono.Y(i)));
            } // for
            centroideX += (poligono.X(poligono.Count - 1) + X0) * ((poligono.X(poligono.Count - 1) * Y0) - (X0 * poligono.Y(poligono.Count - 1)));
            centroideY += (poligono.Y(poligono.Count - 1) + Y0) * ((poligono.X(poligono.Count - 1) * Y0) - (X0 * poligono.Y(poligono.Count - 1)));

            poligono.XCentroide = (centroideX / (6.0 * area));
            poligono.YCentroide = (centroideY / (6.0 * area));
        } // centroid()

        #endregion

        #region Pontos para Poligono

        [Obsolete("Check if it is necessary.")]
        public SharpMap.Geometries.Polygon polygonFromList(List<SharpMap.Geometries.Point> pointList)
        {
            SharpMap.Geometries.LinearRing extRing = new SharpMap.Geometries.LinearRing(pointList);
            SharpMap.Geometries.Polygon mySolidPolygon = new SharpMap.Geometries.Polygon(extRing);

            return mySolidPolygon;
        } // polygonFromList()

        #endregion

        #region Ponto no Poligono

        public SharpMap.Data.FeatureDataRow nearGeoPoint(double xmin, double xmax, double ymin, double ymax, SharpMap.Layers.VectorLayer layer, double amountGrow)
        {
            SharpMap.Geometries.BoundingBox boundingBox = new SharpMap.Geometries.BoundingBox(new SharpMap.Geometries.Point(xmin, ymin), new SharpMap.Geometries.Point(xmax, ymax));
            SharpMap.Data.FeatureDataSet dataSet = new SharpMap.Data.FeatureDataSet();
            layer.DataSource.Open();
            layer.DataSource.ExecuteIntersectionQuery(boundingBox, dataSet);
            DataTable myDataTable = dataSet.Tables[0] as SharpMap.Data.FeatureDataTable; // --     While myDataTable is empty??
            GisSharpBlog.NetTopologySuite.IO.WKTReader myReader = new GisSharpBlog.NetTopologySuite.IO.WKTReader();
            GisSharpBlog.NetTopologySuite.Geometries.Geometry point = myReader.Read(xmin.ToString());
            
            if (myDataTable.Rows.Count == 0) return null;

            double distance = point.Distance(myReader.Read((myDataTable.Rows[0] as SharpMap.Data.FeatureDataRow).Geometry.ToString()));

            SharpMap.Data.FeatureDataRow selectedFeature = myDataTable.Rows[0] as SharpMap.Data.FeatureDataRow;
            if (myDataTable.Rows.Count > 1)
                for (int i = 1; i < myDataTable.Rows.Count; i++)
                {
                    GisSharpBlog.NetTopologySuite.Geometries.Geometry line = myReader.Read((myDataTable.Rows[i] as SharpMap.Data.FeatureDataRow).Geometry.ToString());
                    if (point.Distance(line) < distance)
                    {
                        distance = point.Distance(line);
                        selectedFeature = myDataTable.Rows[i] as SharpMap.Data.FeatureDataRow;
                    } // if
                } // for

            return selectedFeature;
        } // nearGeoPoint()

        public bool isPointInPolygon(SharpMap.Geometries.Point mPontos, SharpMap.Geometries.Polygon Poligono)
        {
            SharpMap.Geometries.Point p1, p2;
            bool inside = false;

            if (Poligono.ExteriorRing.Vertices.Count < 3) return inside;

            SharpMap.Geometries.Point oldPoint = new SharpMap.Geometries.Point(Poligono.ExteriorRing.Vertices[Poligono.ExteriorRing.Vertices.Count - 1].X, Poligono.ExteriorRing.Vertices[Poligono.ExteriorRing.Vertices.Count - 1].Y);

            for (int i = 0; i < Poligono.ExteriorRing.Vertices.Count; i++)
            {
                SharpMap.Geometries.Point newPoint = new SharpMap.Geometries.Point(Poligono.ExteriorRing.Vertices[i].X, Poligono.ExteriorRing.Vertices[i].Y);
                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                } // if
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                } // else

                if ((newPoint.X < mPontos.X) == (mPontos.X <= oldPoint.X) && (mPontos.Y - p1.Y) * (p2.X - p1.X) < (p2.Y - p1.Y) * (mPontos.X - p1.X))
                    inside = !inside;

                oldPoint = newPoint;
            } // for

            return inside;
        } // PointInPolygon()

        public int identifyPointInPoligono(clsIpeaShape shape, double X, double Y)
        {
            int res = -1;
            clsIpeaPoligono poligono = new clsIpeaPoligono();

            for (int i = 0; i < shape.Count; i++)
            {
                poligono = shape[i];
                if (poligono.IsInPoligono(X, Y)) return i;
            } // for

            return res;
        } // identifyPointInPoligono()

        #endregion

        #region Funções do SharpMap

        /// <summary>
        /// Calcula a distancia pelo método de Haversine
        /// </summary>
        /// <param name="from">Ponto de origem.</param>
        /// <param name="to">Ponto de destino.</param>
        /// <returns></returns>
        public double distance(SharpMap.Geometries.Point from, SharpMap.Geometries.Point to)
        {
            double rad = 6371; //Earth radius in Km
            //Convert to radians
            double p1X = from.X / 180 * Math.PI;
            double p1Y = from.Y / 180 * Math.PI;
            double p2X = to.X / 180 * Math.PI;
            double p2Y = to.Y / 180 * Math.PI;

            return Math.Acos(Math.Sin(p1Y) * Math.Sin(p2Y) +
                Math.Cos(p1Y) * Math.Cos(p2Y) * Math.Cos(p2X - p1X)) * rad;
        } // distance()

        public void ReapresentarLayersExistentes(ref SharpMap.Forms.MapImage mapImage, ref SharpMap.Map map, ArrayList strLayers)
        {
            ArrayList layers_to_show = new ArrayList();
            for (int i = 0; i < map.Layers.Count; i++)
                if (strLayers.Contains(map.Layers[i].LayerName))
                    layers_to_show.Add(i);

            if (layers_to_show.Count > 0)
            {
                SharpMap.Map mMapa = new SharpMap.Map(new Size(400, 200));
                for (int i = 0; i < layers_to_show.Count; i++)
                    mMapa.Layers.Add(map.Layers[Convert.ToInt32(layers_to_show[i])]);

                //Coloca o mapa no mapImage
                mapImage.Map = mMapa;

                //Refresh o mapa no mapImage
                mapImage.Map.ZoomToExtents();
                mapImage.Refresh();
            } // if
        } // ReapresentarLayersExistentes()

        public void ExcluirLayerExistente(ref SharpMap.Forms.MapImage mapImage, ref SharpMap.Map map, string strLayer)
        {
            int layerindice = -1;
            for (int i = 0; i < map.Layers.Count; i++)
                if (map.Layers[i].LayerName == strLayer)
                {
                    layerindice = i;
                    break;
                } // if

            if (layerindice < 0) throw new Exception("Layer selecionado não existente.");

            map.Layers.RemoveAt(layerindice);

            //Zoom todo o mapa.
            map.ZoomToExtents();

            //Coloca o mapa no mapImage
            mapImage.Map = map;

            //Refresh o mapa no mapImage
            mapImage.Refresh();
        } // ExcluirLayerExistente() 

        public static string[] TiposGeometrias = new string[] { "Polígonos", "Multi-Polígonos", "Pontos", "Multi-Pontos", "Multi-Curvas", "LineString", "LinearRing" };

        public void AdicionaLayerToMap(ref SharpMap.Layers.VectorLayer layMapa, ref SharpMap.Forms.MapImage mapImage,
            ref SharpMap.Map map, ref DataTable dt_shape, string strEndereco, string strLayer, ref Hashtable[] ht, ref clsIpeaShape shapeAlex, ref string tipo_geometria)
        {
            for (int i = 0; i < map.Layers.Count; i++)
                if (map.Layers[i].LayerName == strLayer)
                    throw new Exception("Layer já existente no mapa. Para inserir um layer com o mesmo nome, é preciso remover o layer existente.");

            layMapa = new SharpMap.Layers.VectorLayer(strLayer);
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);
            shapefile.Open();

            layMapa.DataSource = shapefile;

            //Adiciona dados ao datatable
            DataRow dr = shapefile.GetFeature(0);
            dt_shape = dr.Table.Clone();
            object[] nova_linha = new object[dt_shape.Columns.Count];
            for (uint i = 0; i < shapefile.GetFeatureCount(); i++)
            {
                nova_linha = shapefile.GetFeature(i).ItemArray;
                dt_shape.Rows.Add(nova_linha);
            } // for

            #region Gerando a estrutura de polígonos

            int numero_poligonos = layMapa.DataSource.GetFeatureCount();

            FeatureDataRow feature;
            SharpMap.Geometries.Geometry geoMetria;
            Polygon poligono;
            MultiPolygon mpoligono;
            SharpMap.Geometries.Point ponto;
            SharpMap.Geometries.MultiPoint mponto;
            SharpMap.Geometries.MultiCurve mcurva;
            SharpMap.Geometries.LinearRing linear_ring;
            SharpMap.Geometries.LineString line_string;

            clsUtilTools clt = new clsUtilTools();

            int n_vertices;
            double[] Xcoord;
            double[] Ycoord;

            double[] aux_x;
            double[] aux_y;

            m_shape = new clsIpeaShape();
            clsIpeaPoligono pol = new clsIpeaPoligono();
            int k = 0;

            int n_vertices_interior_ring = 0;
            string[] tipos_poligonos = new string[0];
            clsAreaPerimetroCentroide minhafuncao = new clsAreaPerimetroCentroide();

            for (k = 0; k < numero_poligonos; k++)
            {
                feature = layMapa.DataSource.GetFeature((uint)k);
                geoMetria = feature.Geometry;

                poligono = geoMetria as SharpMap.Geometries.Polygon;

                if (poligono != null)
                {
                    #region Geometria do poligono

                    tipo_geometria = "Polígonos";
                    n_vertices = poligono.ExteriorRing.Vertices.Count;

                    Xcoord = new double[n_vertices];
                    Ycoord = new double[n_vertices];

                    for (int i = 0; i < n_vertices; i++)
                    {
                        Xcoord[i] = poligono.ExteriorRing.Vertices[i].X;
                        Ycoord[i] = poligono.ExteriorRing.Vertices[i].Y;
                    } // for

                    tipos_poligonos = new string[1 + poligono.NumInteriorRing];
                    tipos_poligonos[0] = "ExteriorRing";

                    #region Primeira adição dia 2 de fevereiro de 2009

                    if (poligono.NumInteriorRing > 0)
                    {
                        n_vertices_interior_ring = poligono.NumInteriorRing;

                        for (int i = 0; i < poligono.NumInteriorRing; i++)
                            n_vertices_interior_ring += poligono.InteriorRings[i].Vertices.Count;

                        aux_x = new double[n_vertices_interior_ring];
                        aux_y = new double[n_vertices_interior_ring];

                        int indice_foco = 0;

                        for (int i = 0; i < poligono.NumInteriorRing; i++)
                        {
                            tipos_poligonos[i + 1] = "InteriorRing";

                            aux_x[indice_foco] = double.NaN;
                            aux_y[indice_foco] = double.NaN;
                            indice_foco++;

                            for (int j = 0; j < poligono.InteriorRings[i].Vertices.Count; j++)
                            {
                                aux_x[indice_foco] = poligono.InteriorRings[i].Vertices[j].X;
                                aux_y[indice_foco] = poligono.InteriorRings[i].Vertices[j].Y;
                                indice_foco++;
                            } // for
                        } // for

                        Xcoord = clt.ConcateArraysDouble(Xcoord, aux_x);
                        Ycoord = clt.ConcateArraysDouble(Ycoord, aux_y);
                    } // if

                    #endregion

                    pol = new clsIpeaPoligono();
                    pol.Nome = "Poligono " + k.ToString();

                    pol.IDSharpMap = k;

                    pol.AddCoordenadasX(Xcoord);
                    pol.AddCoordenadasY(Ycoord);
                    pol.Area = minhafuncao.area(Xcoord, Ycoord);
                    pol.XCentroide = poligono.Centroid.X;
                    pol.YCentroide = poligono.Centroid.Y;

                    pol.IndiceCluster = -1;
                    pol.TiposSubpoligonos = tipos_poligonos;
                    m_shape.AddPoligono(pol);

                    #endregion
                } // if
                else
                {
                    ArrayList a_tipos_subpoligonos = new ArrayList();

                    mpoligono = geoMetria as SharpMap.Geometries.MultiPolygon;
                    if (mpoligono != null)
                    {
                        #region geometria de multi-poligono

                        int num_total_poligonos_com_inner_ring = 0;

                        if (mpoligono != null)
                        {
                            tipo_geometria = "Multi-Polígonos";

                            pol = new clsIpeaPoligono();
                            pol.Nome = "Poligono " + k.ToString();

                            n_vertices = mpoligono.Polygons.Count - 1;

                            for (int j = 0; j < mpoligono.Polygons.Count; j++)
                                n_vertices += mpoligono.Polygons[j].ExteriorRing.Vertices.Count;

                            Xcoord = new double[n_vertices];
                            Ycoord = new double[n_vertices];

                            int indice_foco = 0;

                            for (int j = 0; j < mpoligono.Polygons.Count; j++)
                            {
                                a_tipos_subpoligonos.Add("ExteriorRing");

                                poligono = mpoligono.Polygons[j];
                                for (int i = 0; i < poligono.ExteriorRing.Vertices.Count; i++)
                                {
                                    Xcoord[indice_foco] = poligono.ExteriorRing.Vertices[i].X;
                                    Ycoord[indice_foco] = poligono.ExteriorRing.Vertices[i].Y;
                                    indice_foco++;
                                } // for

                                if (j < mpoligono.Polygons.Count - 1)
                                {
                                    Xcoord[indice_foco] = double.NaN;
                                    Ycoord[indice_foco] = double.NaN;
                                    indice_foco++;
                                } // if

                                num_total_poligonos_com_inner_ring += poligono.NumInteriorRing;
                            } // for

                            pol.AddCoordenadasX(Xcoord);
                            pol.AddCoordenadasY(Ycoord);

                            #region Segunda adição dia 2 de fevereiro de 2009

                            if (num_total_poligonos_com_inner_ring > 0)
                            {
                                double[] original_Xcoord = new double[Xcoord.GetLength(0)];
                                double[] original_Ycoord = new double[Ycoord.GetLength(0)];

                                for (int i = 0; i < original_Xcoord.GetLength(0); i++)
                                {
                                    original_Xcoord[i] = Xcoord[i];
                                    original_Ycoord[i] = Ycoord[i];
                                } // for

                                Xcoord = new double[0];
                                Ycoord = new double[0];

                                for (int j = 0; j < mpoligono.Polygons.Count; j++)
                                {
                                    poligono = mpoligono.Polygons[j];
                                    if (poligono.NumInteriorRing > 0)
                                    {
                                        n_vertices_interior_ring = poligono.NumInteriorRing;

                                        for (int i = 0; i < poligono.NumInteriorRing; i++)
                                            n_vertices_interior_ring += poligono.InteriorRings[i].Vertices.Count;

                                        aux_x = new double[n_vertices_interior_ring];
                                        aux_y = new double[n_vertices_interior_ring];

                                        indice_foco = 0;

                                        for (int i = 0; i < poligono.NumInteriorRing; i++)
                                        {
                                            a_tipos_subpoligonos.Add("InteriorRing");
                                            aux_x[indice_foco] = double.NaN;
                                            aux_y[indice_foco] = double.NaN;
                                            indice_foco++;

                                            for (int ii = 0; ii < poligono.InteriorRings[i].Vertices.Count; ii++)
                                            {
                                                aux_x[indice_foco] = poligono.InteriorRings[i].Vertices[ii].X;
                                                aux_y[indice_foco] = poligono.InteriorRings[i].Vertices[ii].Y;
                                                indice_foco++;
                                            } // for
                                        } // for

                                        Xcoord = clt.ConcateArraysDouble(Xcoord, aux_x);
                                        Ycoord = clt.ConcateArraysDouble(Ycoord, aux_y);
                                    } // if
                                } // for

                                Xcoord = clt.ConcateArraysDouble(original_Xcoord, Xcoord);
                                Ycoord = clt.ConcateArraysDouble(original_Ycoord, Ycoord);

                                pol.AddCoordenadasX(Xcoord);
                                pol.AddCoordenadasY(Ycoord);
                            } // if

                            tipos_poligonos = new string[a_tipos_subpoligonos.Count];
                            for (int i = 0; i < tipos_poligonos.GetLength(0); i++) { tipos_poligonos[i] = Convert.ToString(a_tipos_subpoligonos[i]); }
                            pol.TiposSubpoligonos = tipos_poligonos;

                            #endregion

                            pol.IndiceCluster = -1;
                            m_shape.AddPoligono(pol);
                        }
                        #endregion
                    } // if
                    else
                    {
                        ponto = geoMetria as SharpMap.Geometries.Point;
                        if (ponto != null)
                            tipo_geometria = "Pontos";
                        else
                        {
                            mponto = geoMetria as SharpMap.Geometries.MultiPoint;
                            if (mponto != null)
                                tipo_geometria = "Multi-Pontos";
                            else
                            {
                                mcurva = geoMetria as SharpMap.Geometries.MultiCurve;

                                if (mcurva != null)
                                    tipo_geometria = "Multi-Curvas";
                                else
                                {
                                    line_string = geoMetria as SharpMap.Geometries.LineString;

                                    if (line_string != null)
                                        tipo_geometria = "LineString";
                                    else
                                    {
                                        linear_ring = geoMetria as SharpMap.Geometries.LinearRing;

                                        if (linear_ring != null)
                                            tipo_geometria = "LinearRing";
                                    } // else
                                } // else
                            } // else
                        } // else
                    } // else
                } // else
            } // for

            m_vector_shapes = new clsIpeaShape[1];
            m_vector_shapes[0] = new clsIpeaShape();
            m_vector_shapes[0] = m_shape;

            #endregion

            shapefile.Close();

            //Cor do mapa
            layMapa.Style.Fill = new SolidBrush(Color.Transparent);

            //Cor da linha
            layMapa.Style.Outline = System.Drawing.Pens.Black;
            layMapa.Style.EnableOutline = true;

            map.Layers.Insert(0, layMapa);

            //Coloca o mapa no mapImage
            mapImage.Map = map;

            //Zoom todo o mapa.
            map.ZoomToExtents();

            //Refresh o mapa no mapImage
            mapImage.Refresh();

            shapeAlex = m_shape;

            #region Hashtable com as características dos polígonos

            ht = new Hashtable[shapefile.GetFeatureCount()];

            SharpMap.Styles.VectorStyle vs = (SharpMap.Styles.VectorStyle)layMapa.Style;

            for (int i = 0; i < shapefile.GetFeatureCount(); i++)
            {
                ht[i] = new Hashtable();
                ht[i].Add("Fill", vs.Fill.Clone());
                ht[i].Add("Line", vs.Line.Clone());
                ht[i].Add("Outline", vs.Outline.Clone());

                if (vs.Symbol != null) ht[i].Add("Symbol", vs.Symbol.Clone());
                else ht[i].Add("Symbol", new Bitmap(1, 1));
            } // for

            #endregion
        } // AdicionaLayerToMap()

        /// <summary>
        /// Faz o load do mapa no MapImage
        /// </summary>
        ///<param name="layMapa">Layer do mapa.</param>
        /// <param name="mapImage">MapImage.</param>
        /// <param name="strEndereco">Endereço do shapeFile.</param>
        /// <param name="strLayer">Nome do Layer a ser adicionado no mapa.</param>
        public void loadingMapa(ref SharpMap.Layers.VectorLayer layMapa, ref SharpMap.Forms.MapImage mapImage, ref SharpMap.Map map, string strEndereco, 
            string strLayer, ref clsIpeaShape shapeAlex, ref Hashtable[] ht, ref string tipo_geometria)
        {
            //Definindo um layer
            layMapa = new SharpMap.Layers.VectorLayer(strLayer);

            map = new SharpMap.Map(new Size(800, 400));
            map.BackColor = Color.White;

            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Guarda a informação do mapa no Layer
            layMapa.DataSource = shapefile;

            //Adiciona dados ao datatable
            DataRow dr = shapefile.GetFeature(0);
            DataTable dt_shape = dr.Table.Clone();
            object[] nova_linha = new object[dt_shape.Columns.Count];
            for (uint i = 0; i < shapefile.GetFeatureCount(); i++)
            {
                nova_linha = shapefile.GetFeature(i).ItemArray;
                dt_shape.Rows.Add(nova_linha);
            } // for
            m_table_dados = dt_shape;

            #region Gerando a estrutura de polígonos

            int numero_poligonos = layMapa.DataSource.GetFeatureCount();

            FeatureDataRow feature;
            SharpMap.Geometries.Geometry geoMetria;
            Polygon poligono;
            MultiPolygon mpoligono;
            SharpMap.Geometries.Point ponto;
            SharpMap.Geometries.MultiPoint mponto;
            SharpMap.Geometries.MultiCurve mcurva;
            SharpMap.Geometries.LinearRing linear_ring;
            SharpMap.Geometries.LineString line_string;

            clsUtilTools clt = new clsUtilTools();

            int n_vertices;
            double[] Xcoord;
            double[] Ycoord;

            double[] aux_x;
            double[] aux_y;

            m_shape = new clsIpeaShape();
            clsIpeaPoligono pol = new clsIpeaPoligono();
            int k = 0;

            int n_vertices_interior_ring = 0;

            string[] tipos_poligonos = new string[0];

            clsAreaPerimetroCentroide minhafuncao = new clsAreaPerimetroCentroide();
            for (k = 0; k < numero_poligonos; k++)
            {
                feature = layMapa.DataSource.GetFeature((uint)k);
                geoMetria = feature.Geometry;

                poligono = geoMetria as SharpMap.Geometries.Polygon;

                if (poligono != null)
                {
                    #region Geometria do poligono

                    tipo_geometria = "Polígonos";

                    n_vertices = poligono.ExteriorRing.Vertices.Count;

                    Xcoord = new double[n_vertices];
                    Ycoord = new double[n_vertices];

                    for (int i = 0; i < n_vertices; i++)
                    {
                        Xcoord[i] = poligono.ExteriorRing.Vertices[i].X;
                        Ycoord[i] = poligono.ExteriorRing.Vertices[i].Y;
                    } // for

                    tipos_poligonos = new string[1 + poligono.NumInteriorRing];
                    tipos_poligonos[0] = "ExteriorRing";

                    #region Primeira adição dia 2 de fevereiro de 2009

                    if (poligono.NumInteriorRing > 0)
                    {
                        n_vertices_interior_ring = poligono.NumInteriorRing;

                        for (int i = 0; i < poligono.NumInteriorRing; i++)
                            n_vertices_interior_ring += poligono.InteriorRings[i].Vertices.Count;

                        aux_x = new double[n_vertices_interior_ring];
                        aux_y = new double[n_vertices_interior_ring];

                        int indice_foco = 0;

                        for (int i = 0; i < poligono.NumInteriorRing; i++)
                        {
                            tipos_poligonos[i + 1] = "InteriorRing";

                            aux_x[indice_foco] = double.NaN;
                            aux_y[indice_foco] = double.NaN;
                            indice_foco++;

                            for (int j = 0; j < poligono.InteriorRings[i].Vertices.Count; j++)
                            {
                                aux_x[indice_foco] = poligono.InteriorRings[i].Vertices[j].X;
                                aux_y[indice_foco] = poligono.InteriorRings[i].Vertices[j].Y;
                                indice_foco++;
                            } // for
                        } // for

                        Xcoord = clt.ConcateArraysDouble(Xcoord, aux_x);
                        Ycoord = clt.ConcateArraysDouble(Ycoord, aux_y);
                    } // if

                    #endregion

                    pol = new clsIpeaPoligono();
                    pol.Nome = "Poligono " + k.ToString();

                    pol.IDSharpMap = k;

                    pol.AddCoordenadasX(Xcoord);
                    pol.AddCoordenadasY(Ycoord);
                    pol.Area = minhafuncao.area(Xcoord, Ycoord);
                    pol.XCentroide = poligono.Centroid.X;
                    pol.YCentroide = poligono.Centroid.Y;
                    pol.IndiceCluster = -1;
                    pol.TiposSubpoligonos = tipos_poligonos;
                    m_shape.AddPoligono(pol);

                    #endregion 
                } // if
                else
                {
                    ArrayList a_tipos_subpoligonos = new ArrayList();
                    mpoligono = geoMetria as SharpMap.Geometries.MultiPolygon;
                    int num_total_poligonos_com_inner_ring = 0;

                    if (mpoligono != null)
                    {
                        #region Geometria do multi-poligono

                        tipo_geometria = "Multi-Polígonos";
                        pol = new clsIpeaPoligono();
                        pol.Nome = "Poligono " + k.ToString();
                        n_vertices = mpoligono.Polygons.Count - 1;

                        for (int j = 0; j < mpoligono.Polygons.Count; j++)
                            n_vertices += mpoligono.Polygons[j].ExteriorRing.Vertices.Count;

                        Xcoord = new double[n_vertices];
                        Ycoord = new double[n_vertices];

                        int indice_foco = 0;
                        for (int j = 0; j < mpoligono.Polygons.Count; j++)
                        {
                            a_tipos_subpoligonos.Add("ExteriorRing");

                            poligono = mpoligono.Polygons[j];
                            for (int i = 0; i < poligono.ExteriorRing.Vertices.Count; i++)
                            {
                                Xcoord[indice_foco] = poligono.ExteriorRing.Vertices[i].X;
                                Ycoord[indice_foco] = poligono.ExteriorRing.Vertices[i].Y;
                                indice_foco++;
                            } // if

                            if (j < mpoligono.Polygons.Count - 1)
                            {
                                Xcoord[indice_foco] = double.NaN;
                                Ycoord[indice_foco] = double.NaN;
                                indice_foco++;
                            } // if

                            num_total_poligonos_com_inner_ring += poligono.NumInteriorRing;
                        } // for

                        pol.AddCoordenadasX(Xcoord);
                        pol.AddCoordenadasY(Ycoord);

                        #region Segunda adição dia 2 de fevereiro de 2009

                        if (num_total_poligonos_com_inner_ring > 0)
                        {
                            double[] original_Xcoord = new double[Xcoord.GetLength(0)];
                            double[] original_Ycoord = new double[Ycoord.GetLength(0)];

                            for (int i = 0; i < original_Xcoord.GetLength(0); i++)
                            {
                                original_Xcoord[i] = Xcoord[i];
                                original_Ycoord[i] = Ycoord[i];
                            } // for

                            Xcoord = new double[0];
                            Ycoord = new double[0];

                            for (int j = 0; j < mpoligono.Polygons.Count; j++)
                            {
                                poligono = mpoligono.Polygons[j];
                                if (poligono.NumInteriorRing > 0)
                                {
                                    n_vertices_interior_ring = poligono.NumInteriorRing;
                                    for (int i = 0; i < poligono.NumInteriorRing; i++)
                                        n_vertices_interior_ring += poligono.InteriorRings[i].Vertices.Count;

                                    aux_x = new double[n_vertices_interior_ring];
                                    aux_y = new double[n_vertices_interior_ring];

                                    indice_foco = 0;
                                    for (int i = 0; i < poligono.NumInteriorRing; i++)
                                    {
                                        a_tipos_subpoligonos.Add("InteriorRing");
                                        aux_x[indice_foco] = double.NaN;
                                        aux_y[indice_foco] = double.NaN;
                                        indice_foco++;

                                        for (int ii = 0; ii < poligono.InteriorRings[i].Vertices.Count; ii++)
                                        {
                                            aux_x[indice_foco] = poligono.InteriorRings[i].Vertices[ii].X;
                                            aux_y[indice_foco] = poligono.InteriorRings[i].Vertices[ii].Y;
                                            indice_foco++;
                                        } // for
                                    } // for

                                    Xcoord = clt.ConcateArraysDouble(Xcoord, aux_x);
                                    Ycoord = clt.ConcateArraysDouble(Ycoord, aux_y);
                                } // if
                            } // for

                            Xcoord = clt.ConcateArraysDouble(original_Xcoord, Xcoord);
                            Ycoord = clt.ConcateArraysDouble(original_Ycoord, Ycoord);

                            pol.AddCoordenadasX(Xcoord);
                            pol.AddCoordenadasY(Ycoord);
                        } // if

                        tipos_poligonos = new string[a_tipos_subpoligonos.Count];
                        for (int i = 0; i < tipos_poligonos.GetLength(0); i++) { tipos_poligonos[i] = Convert.ToString(a_tipos_subpoligonos[i]); }
                        pol.TiposSubpoligonos = tipos_poligonos;

                        #endregion
                        
						pol.IndiceCluster = -1;
                        m_shape.AddPoligono(pol);

                        #endregion
                    } // if
                    else
                    {
                        ponto = geoMetria as SharpMap.Geometries.Point;
                        if (ponto != null)
                            tipo_geometria = "Pontos";
                        else
                        {
                            mponto = geoMetria as SharpMap.Geometries.MultiPoint;
                            if (mponto != null)
                                tipo_geometria = "Multi-Pontos";
                            else
                            {
                                mcurva = geoMetria as SharpMap.Geometries.MultiCurve;
                                if (mcurva != null)
                                    tipo_geometria = "Multi-Curvas";
                                else
                                {
                                    line_string = geoMetria as SharpMap.Geometries.LineString;
                                    if (line_string != null)
                                        tipo_geometria = "LineString";
                                    else
                                    {
                                        linear_ring = geoMetria as SharpMap.Geometries.LinearRing;
                                        if (linear_ring != null)
                                            tipo_geometria = "LinearRing";
                                    } // else
                                } // else
                            } // else
                        } // else
                    } // else
                } // else
            }

            m_vector_shapes = new clsIpeaShape[1];
            m_vector_shapes[0] = new clsIpeaShape();
            m_vector_shapes[0] = m_shape;

            #endregion

            //Fecha o shapeFile
            shapefile.Close();

            //Cor do mapa
			layMapa.Style.Fill = new SolidBrush(Color.Transparent);

            //Cor da linha
            layMapa.Style.Outline = System.Drawing.Pens.Black;
            layMapa.Style.EnableOutline = true;

            //Adiciona layer
            map.Layers.Add(layMapa);

            Classes.clsArmazenamentoDados salvar = new Classes.clsArmazenamentoDados();
            if (salvar.Leitura_efetuada == true)
            {
                map.Zoom = salvar.zoom_m;
            }
            else
            {
                map.ZoomToExtents();
            }

            //Coloca o mapa no mapImage
            mapImage.Map = map;

            //Refresh o mapa no mapImage
            mapImage.Refresh();

            //Guarda a estrutura do Shape
            shapeAlex = m_shape;
            
            #region Hashtable com as características dos polígonos
            ht = new Hashtable[shapefile.GetFeatureCount()];
            SharpMap.Styles.VectorStyle vs = (SharpMap.Styles.VectorStyle)layMapa.Style;

            for (int i = 0; i < shapefile.GetFeatureCount(); i++)
            {
                ht[i] = new Hashtable();
                ht[i].Add("Fill", vs.Fill.Clone());
                ht[i].Add("Line", vs.Line.Clone());
                ht[i].Add("Outline", vs.Outline.Clone());
                if (vs.Symbol != null)
                    ht[i].Add("Symbol", vs.Symbol.Clone());
                else
                    ht[i].Add("Symbol", new Bitmap(1, 1));
            } // for

            #endregion
        } // loadingMapa()

        /// <summary>
        /// Captura os pontos que formam um determinado poligono
        /// </summary>
        /// <param name="layMapa">Layer do mapa.</param>
        /// <param name="strEndereco">Endereço do shapefile.</param>
        /// <param name="uID">ID do poligono de interesse.</param>
        /// <returns></returns>
        public List<PointF> coordenadasPoligono(SharpMap.Layers.VectorLayer layMapa, string strEndereco, uint uID)
        {
            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Guarda a informação do mapa no Layer
            layMapa.DataSource = shapefile;

            //Captura o FeatureDataRow
            FeatureDataRow feature = layMapa.DataSource.GetFeature(uID);

            //Captura a geometria
            SharpMap.Geometries.Geometry geoMetria = feature.Geometry;

            //Lista de pontos do tipo Float
            List<PointF> pontos = new List<PointF>();

            //Geometria do poligono
            Polygon poligono = geoMetria as SharpMap.Geometries.Polygon;

            if (poligono != null)
                foreach (var verice in poligono.ExteriorRing.Vertices)
                    pontos.Add(new PointF((float)verice.X, (float)verice.Y));

            //Fecha o shapeFile
            shapefile.Close();

            return (pontos);
        } // coordenadasPoligono()

        /// <summary>
        /// Captura os centroids de todos os poligonos
        /// </summary>
        /// <param name="layMapa">Layer do mapa</param>
        /// <param name="strEndereco">Endereço do shapefile</param>
        /// <returns></returns>
        public List<PointF> centroidsPoligono(SharpMap.Layers.VectorLayer layMapa, string strEndereco)
        {
            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Guarda a informação do mapa no Layer
            layMapa.DataSource = shapefile;

            //Lista de pontos do tipo Float
            List<PointF> mPontos = new List<PointF>();

            for (int i = 0; i < shapefile.GetFeatureCount(); i++)
            {
                //Captura o FeatureDataRow
                FeatureDataRow feature = shapefile.GetFeature((uint)i);

                //Captura a geometria
                SharpMap.Geometries.Geometry geoMetria = feature.Geometry;

                //Geometria do poligono
                Polygon poligono = geoMetria as SharpMap.Geometries.Polygon;

                if (poligono != null)
                    mPontos.Add(new PointF((float)poligono.Centroid.X, (float)poligono.Centroid.Y));
            } // for

            //Fecha o shapeFile
            shapefile.Close();

            return (mPontos);
        } // centroidsPoligono()

        /// <summary>
        /// Captura os centroids de um determinado poligono.
        /// </summary>
        /// <param name="layMapa">layer do mapa.</param>
        /// <param name="strEndereco">Enedereço do shapefile.</param>
        /// <param name="uID">ID do poligono de interesse.</param>
        /// <returns></returns>
        public SharpMap.Geometries.Point centroidsPoligono(SharpMap.Layers.VectorLayer layMapa, string strEndereco, uint uID)
        {
            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Guarda a informação do mapa no Layer
            layMapa.DataSource = shapefile;

            //Captura o FeatureDataRow
            FeatureDataRow feature = layMapa.DataSource.GetFeature(uID);

            //Captura a geometria
            SharpMap.Geometries.Geometry geoMetria = feature.Geometry;

            //Geometria do poligono
            Polygon poligono = geoMetria as SharpMap.Geometries.Polygon;

            //Lista de pontos do tipo Float
            SharpMap.Geometries.Point mPontos = new SharpMap.Geometries.Point(poligono.Centroid.X, poligono.Centroid.Y);

            //Fecha o shapeFile
            shapefile.Close();

            return (mPontos);
        } // centroidsPoligono()

        public List<PointF> boundingMapa(SharpMap.Layers.VectorLayer layMapa, string strEndereco)
        {
            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Guarda a informação do mapa no Layer
            layMapa.DataSource = shapefile;

            //Captura o FeatureDataRow
            FeatureDataRow feature = shapefile.GetFeature(0);


            //Captura a geometria
            SharpMap.Geometries.Geometry geoMetria = feature.Geometry.Envelope();

            //Lista de pontos do tipo Float
            List<PointF> pontos = new List<PointF>();

            //Geometria do poligono
            Polygon poligono = geoMetria as SharpMap.Geometries.Polygon;

            if (poligono != null)
                foreach (var verice in poligono.ExteriorRing.Vertices)
                    pontos.Add(new PointF((float)verice.X, (float)verice.Y));

            //Fecha o shapeFile
            shapefile.Close();

            return (pontos);
        } // boundingMapa()

        /// <summary>
        /// Lista as variaveis que estão no mapa
        /// </summary>
        /// <param name="strEndereco">Endereço do shapefile.</param>
        /// <param name="uID">ID do poligono.</param>
        /// <returns></returns>
        public string[] informacaoVariaveis(string strEndereco, uint uID)
        {
            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Definindo um layer
            SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("temporario");

            //Guarda a informação do mapa no Layer
            layMapa.DataSource = shapefile;

            //Captura o FeatureDataRow
            FeatureDataRow feature = layMapa.DataSource.GetFeature(uID);

            //Guarda as informações
            string[] strVariaveis = new string[feature.Table.Columns.Count];

            for (int i = 0; i < strVariaveis.Length; i++)
                strVariaveis[i] = feature.Table.Columns[i].ToString();

            //Fecha o shapeFile
            shapefile.Close();

            return (strVariaveis);
        } // informacaoVariaveis()

        public string informacaoVariavel(string strEndereco, string strVariavel, uint uID)
        {
            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Definindo um layer
            SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("temporario");

            //Guarda a informação do mapa no Layer
            layMapa.DataSource = shapefile;

            //Captura o FeatureDataRow
            FeatureDataRow feature = layMapa.DataSource.GetFeature(uID);

            //Guarda as informações
            Type tipo = feature.Table.Columns[strVariavel].DataType;

            //Salva o tipo de interesse
            string strTipo = tipo.ToString();

            //Tipo da variavel
            Type tipoVar = Type.GetType(strTipo);

            //Inicializa a saida
            string strDados = "";

            if (strTipo == "System.Int32")
            {
                Int32 strInt = feature.Field<Int32>(strVariavel);
                strDados = strInt.ToString();
            } // if
            else if (strTipo == "System.Int16")
            {
                Int16 strInt = feature.Field<Int16>(strVariavel);
                strDados = strInt.ToString();
            } // else if
            else if (strTipo == "System.Int64")
            {
                Int64 strInt = feature.Field<Int64>(strVariavel);
                strDados = strInt.ToString();
            } // else if
            else if (strTipo == "System.Double")
            {
                double strInt = feature.Field<double>(strVariavel);
                strDados = strInt.ToString();
            } // else if
            else
                strDados = feature.Field<string>(strVariavel);

            //Fecha o shapeFile
            shapefile.Close();

            return (strDados);
        } // informacaoVariavel()

        #endregion

        #region Vizinhança Espacial

        public bool poligonosSaoVizinhos(string strEndereco, uint uID1, uint uID2)
        {
            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Layer temporario
            SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("layTemporario");

            //Guarda a informação do mapa no Layer
            layMapa.DataSource = shapefile;

            //Captura o FeatureDataRow
            FeatureDataRow feature1 = layMapa.DataSource.GetFeature(uID1);
            FeatureDataRow feature2 = layMapa.DataSource.GetFeature(uID2);

            //Captura a geometria
            SharpMap.Geometries.Geometry geoMetria1 = feature1.Geometry;
            SharpMap.Geometries.Geometry geoMetria2 = feature2.Geometry;

            //Geometria do poligono
            Polygon poligono1 = geoMetria1 as SharpMap.Geometries.Polygon;
            Polygon poligono2 = geoMetria2 as SharpMap.Geometries.Polygon;

            if (poligono1 != null && poligono2 != null)
                for (int i = 0; i < poligono1.ExteriorRing.Vertices.Count; i++)
                    for (int j = 0; j < poligono2.ExteriorRing.Vertices.Count; j++)
                        if ((poligono1.ExteriorRing.Vertices[i].X == poligono2.ExteriorRing.Vertices[j].X) && (poligono1.ExteriorRing.Vertices[i].Y == poligono2.ExteriorRing.Vertices[j].Y))
                        {
                            //Fecha o shapeFile
                            shapefile.Close();
                            return (true);
                        } // if

            //Fecha o shapeFile
            shapefile.Close();

            //Retorna o resultado
            return (false);
        } // poligonosSaoVizinhos()

        #endregion

        #region Themes para o mapa e Classes

        /// <summary>
        /// Faz a conecção entre o DataTable e o Mapa
        /// </summary>
        /// <param name="dTable">DataTable</param>
        /// <param name="layMapa">layer do mapa</param>
        /// <param name="strID">Identificador do DataTable</param>
        /// <param name="strIDmapa">Identificador do Mapa</param>
        public void ConeccaoEntreMapaEdataTable(ref DataTable dTable, SharpMap.Layers.VectorLayer layMapa, string strID, string strIDmapa)
        {
            //Verifica se já houve a conexão com o DataTable
            if (dTable.Columns.Contains("Mapa" + strIDmapa) == false)
            {
                //Adiciona a coluna
                dTable.Columns.Add("Mapa" + strIDmapa, Type.GetType("System.Int32"));

                //Guarda os dados do dataTable em um ArrayList
                ArrayList arLista = new ArrayList(dTable.Rows.Count);
                for (int k = 0; k < dTable.Rows.Count; k++) arLista.Add(dTable.Rows[k][strID].ToString());

                //Encontra a observação de interesse
                for (int j = 0; j < arLista.Count; j++)
                {
                    int i = arLista.BinarySearch(layMapa.DataSource.GetFeature((uint)j)[strIDmapa].ToString());
                    dTable.Rows[i]["Mapa" + strIDmapa] = j;
                } // for
            } // if
        } // ConeccaoEntreMapaEdataTable()

        //Criando uma funcao de search
        public int Busca(ArrayList vetorstrings, string buscar)
        {
            int result = -1;
            for (int i = 0; i < vetorstrings.Count; i++)
                if (vetorstrings[i].ToString() == buscar)
                {
                    result = i;
                    break;
                } // if
            
            return (result);
        } // Busca()

        public void ConeccaoEntreMapaEdataTable(ref DataTable dTable, string strEnderecoShape,
            string strID, string strIDmapa, ref ProgressBar pBar, ref clsIpeaShape classeShape, ref bool erro)
        {
            try
            {
                //Teste: criando um novo poligono que so contenha os que tem dados no datatable
                clsIpeaShape novoshape = new clsIpeaShape();

                SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("TEMP");

                //Adicionando variaveis:
                SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEnderecoShape);

                //Abre o mapa para editar a suas propriedades
                shapefile.Open();

                //Guarda a informação do mapa nos Layers
                layMapa.DataSource = shapefile;
                
                char[] separator = { ',' };
                CultureInfo cc = new CultureInfo("en-US", true);

                //Inicializa a Progress Bar
                pBar.Minimum = 0;
                pBar.Value = 0;
                pBar.Maximum = layMapa.DataSource.GetFeatureCount();
                pBar.Refresh();
                
                //Verifica se já houve a conexão com o DataTable
                if (dTable.Columns.Contains("Mapa" + strIDmapa) == false)
                {
                    //Adiciona a coluna
                    dTable.Columns.Add("Mapa" + strIDmapa, Type.GetType("System.Int32"));

                    //Guarda os dados do dataTable em um ArrayList
                    ArrayList arLista = new ArrayList(dTable.Rows.Count);
                    for (int k = 0; k < dTable.Rows.Count; k++) arLista.Add(dTable.Rows[k][strID].ToString());

                    bool m_shape_sem_obs_no_dtable = false;
                    DataTable completa_dt = dTable.Copy();
                    DataRow nova_linha = completa_dt.NewRow();
                    completa_dt.Rows.Clear();
                    int indice_nova_linha = dTable.Rows.Count;
                    DataRow drBase;
                    
                    //Encontra a observação de interesse
                    for (int j = 0; j < classeShape.Count; j++)
                    {
                        //Encontra o Identificador do mapa para o j-ésimo polígono
                        string strIdent = layMapa.DataSource.GetFeature((uint)j)[strIDmapa].ToString();

                        //Captura os dados do j-ésimo polígono
                        FeatureDataRow ftDataRow1 = layMapa.DataSource.GetFeature((uint)j);

                        int i = Busca(arLista, strIdent);

                        if (i > -1)
                        {
                            //Dados da base que é igual a base do mapa
                            drBase = dTable.Rows[i];

                            dTable.Rows[i]["Mapa" + strIDmapa] = j;
                            classeShape[j].PosicaoNoDataTable = i;
                            classeShape[j].ID = strIdent;
                            novoshape.AddPoligono(classeShape[j]);
                        } // if
                        else
                        {
							erro = true;

                            MessageBox.Show("Não há correspondência entre as variáveis de união entre base e malha.",
                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.None);
                            
                            break;
                        } // else

						#warning Implementar usando threads.
                        //Incrementa a progressbar.
                        pBar.Increment(1);
                        Application.DoEvents();
                    } // for
					
                    int diferenca = classeShape.Count - arLista.Count;
                    for (int i = 0; i < diferenca; i++)
                    {
                        classeShape[arLista.Count + i].PosicaoNoDataTable = -1;
                        classeShape[arLista.Count + i].ID = "Null";
                    } // for

                    if (m_shape_sem_obs_no_dtable)
                        MessageBox.Show("Não há correspondência entre as variáveis de união entre base e malha.",
                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.None);
                } // if
            } // try
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // ConeccaoEntreMapaEdataTable()

        /// <summary>
        /// Faz a conecção entre o DataTable e o Mapa
        /// </summary>
        /// <param name="dTable">DataTable</param>
        /// <param name="layMapa">layer do mapa</param>
        /// <param name="strID">Identificador do DataTable</param>
        /// <param name="strIDmapa">Identificador do Mapa</param>
        /// <param name="pBar">ProgressBar</param>
        public void ConeccaoEntreMapaEdataTable(ref DataTable dTable, SharpMap.Layers.VectorLayer layMapa, string strID, string strIDmapa, ref ProgressBar pBar)
        {
            try
            {
                //Inicializa a Progress Bar
                pBar.Minimum = 0;
                pBar.Value = 0;
                pBar.Maximum = layMapa.DataSource.GetFeatureCount();
                pBar.Refresh();

                //Verifica se já houve a conexão com o DataTable
                if (dTable.Columns.Contains("Mapa" + strIDmapa) == false)
                {
                    //Adiciona a coluna
                    dTable.Columns.Add("Mapa" + strIDmapa, Type.GetType("System.Int32"));

                    //Guarda os dados do dataTable em um ArrayList
                    ArrayList arLista = new ArrayList(dTable.Rows.Count);
                    for (int k = 0; k < dTable.Rows.Count; k++) arLista.Add(dTable.Rows[k][strID].ToString());

                    //Encontra a observação de interesse
                    for (int j = 0; j < arLista.Count; j++)
                    {
                        int i = arLista.BinarySearch(layMapa.DataSource.GetFeature((uint)j)[strIDmapa].ToString());
                        dTable.Rows[i]["Mapa" + strIDmapa] = j;

                        //Incrementa a progressbar.
                        pBar.Increment(1);
                        Application.DoEvents();
                    } // for
                } // if
            } // try
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // ConeccaoEntreMapaEdataTable()

        /// <summary>
        /// Cria o vetor temático de classes por quantil
        /// </summary>
        /// <param name="dTable">DataTable</param>
        /// <param name="strVariavel">Variável a ser classificada</param>
        /// <param name="strIDmapa">ID do Mapa</param>
        /// <param name="strID">ID da Base</param>
        /// <param name="nClasses">Número de classes</param>
        /// <param name="limitesClasses">Vetor de saida com os limites das classes</param>
        /// <returns></returns>
        public int[] criaQuantis(DataTable dTable, string strVariavel, string strIDmapa, string strID, int nClasses, ref double[] limitesClasses)
        {
            //Número de observações na base
            int nDados = dTable.Rows.Count;

            //Guarda os dados de interesse no vetor
            double[] dblDados = new double[nDados];
            int[] idDados = new int[nDados];
            
            //Guarda os dados no vetor
            for (int i = 0; i < nDados; i++)
            {
                if (dTable.Rows[i][strVariavel].ToString() != "")
                    dblDados[i] = Convert.ToDouble(dTable.Rows[i][strVariavel]);
                
                if (dTable.Rows[i]["Mapa" + strIDmapa].ToString() != "")
                    idDados[i] = Convert.ToInt32(dTable.Rows[i]["Mapa" + strIDmapa]);
                
            } // for

            //Ordena o vetor
            Array.Sort(dblDados, idDados);

            for (int c = 0; c < nClasses; c++)
            {
                double fator = ((double)c + 1) / (double)nClasses;
                int iPosicao = (int)Math.Ceiling(fator * ((double)nDados - 1));
                limitesClasses[c] = dblDados[iPosicao];
            } // for

            //Inicializa o vetor temático
            int[] vetorTematico = new int[nDados];

            //Guarda os valores no vetorTemático
            for (int i = 0; i < nDados; i++)
                for (int c = 0; c < nClasses; c++)
                    if (dblDados[i] <= limitesClasses[c])
                    {
                        vetorTematico[idDados[i]] = c;
                        break;
                    } // if

            //Retorna o resultado
            return (vetorTematico);
        } // criaQuantis()
        
        /// <summary>
        /// Cria o vetor temático de classes por geometric
        /// </summary>
        /// <param name="dTable">DataTable</param>
        /// <param name="strVariavel">Variável a ser classificada</param>
        /// <param name="strIDmapa">ID do Mapa</param>
        /// <param name="strID">ID da Base</param>
        /// <param name="nClasses">Número de classes</param>
        /// <param name="limitesClasses">Vetor de saida com os limites das classes</param>
        /// <returns></returns>
        public int[] criaGeometric(DataTable dTable, string strVariavel, string strIDmapa, string strID, int nClasses, ref double[] limitesClasses)
        {
            //Número de observações na base
            int nDados = dTable.Rows.Count;

            //Guarda os dados de interesse no vetor
            double[] dblDados = new double[nDados];
            int[] idDados = new int[nDados];
            
            //Guarda os dados no vetor
            for (int i = 0; i < nDados; i++)
            {
                if (dTable.Rows[i][strVariavel].ToString() != "")
                    dblDados[i] = Convert.ToDouble(dTable.Rows[i][strVariavel]);
                
                if (dTable.Rows[i]["Mapa" + strIDmapa].ToString() != "")
                    idDados[i] = Convert.ToInt32(dTable.Rows[i]["Mapa" + strIDmapa]);
            } // for

            //Ordena o vetor
            Array.Sort(dblDados, idDados);

            limitesClasses = geometric(dblDados, nClasses);

            //Inicializa o vetor temático
            int[] vetorTematico = new int[nDados];

            //Guarda os valores no vetorTemático
            for (int i = 0; i < nDados; i++)
                for (int c = 0; c < nClasses; c++)
                    if (dblDados[i] <= limitesClasses[c])
                    {
                        vetorTematico[idDados[i]] = c;
                        break;
                    } // if

            //Retorna o resultado
            return (vetorTematico);
        } // criaGeometric()

        /// <summary>
        /// Cria o vetor temático de classes por jenks
        /// </summary>
        /// <param name="dTable">DataTable</param>
        /// <param name="strVariavel">Variável a ser classificada</param>
        /// <param name="strIDmapa">ID do Mapa</param>
        /// <param name="strID">ID da Base</param>
        /// <param name="nClasses">Número de classes</param>
        /// <param name="limitesClasses">Vetor de saida com os limites das classes</param>
        /// <returns></returns>
        public int[] criaJenks(DataTable dTable, string strVariavel, string strIDmapa, string strID, int nClasses, ref double[] limitesClasses)
        {
            //Número de observações na base
            int nDados = dTable.Rows.Count;

            //Guarda os dados de interesse no vetor
            double[] dblDados = new double[nDados];
            int[] idDados = new int[nDados];
            
            //Guarda os dados no vetor
            for (int i = 0; i < nDados; i++)
            {
                if (dTable.Rows[i][strVariavel].ToString() != "")
                    dblDados[i] = Convert.ToDouble(dTable.Rows[i][strVariavel]);
                
                if (dTable.Rows[i]["Mapa" + strIDmapa].ToString() != "")
                    idDados[i] = Convert.ToInt32(dTable.Rows[i]["Mapa" + strIDmapa]);
            } // for

            //Ordena o vetor
            Array.Sort(dblDados, idDados);

            double[] dblDados2 = new double[nDados];
            dblDados2 = (double[])dblDados.Clone();

            limitesClasses = jenks(dblDados2, nClasses);

            //Inicializa o vetor temático
            int[] vetorTematico = new int[nDados];
            
            //Guarda os valores no vetorTemático
            for (int i = 0; i < nDados; i++)
                for (int c = 0; c < nClasses; c++)
                    if (dblDados[i] <= limitesClasses[c])
                    {
                        vetorTematico[idDados[i]] = c;
                        break;
                    } // if
            
            //Retorna o resultado
            return (vetorTematico);
        } // criaJenks()

        /// <summary>
        /// Cria o vetor temático de classes por equal
        /// </summary>
        /// <param name="dTable">DataTable</param>
        /// <param name="strVariavel">Variável a ser classificada</param>
        /// <param name="strIDmapa">ID do Mapa</param>
        /// <param name="strID">ID da Base</param>
        /// <param name="nClasses">Número de classes</param>
        /// <param name="limitesClasses">Vetor de saida com os limites das classes</param>
        /// <returns></returns>
        public int[] criaEqual(DataTable dTable, string strVariavel, string strIDmapa, string strID, int nClasses, ref double[] limitesClasses)
        {
            //Número de observações na base
            int nDados = dTable.Rows.Count;

            //Guarda os dados de interesse no vetor
            double[] dblDados = new double[nDados];
            int[] idDados = new int[nDados];
            
            //Guarda os dados no vetor
            for (int i = 0; i < nDados; i++)
            {
                if (dTable.Rows[i][strVariavel].ToString() != "")
                    dblDados[i] = Convert.ToDouble(dTable.Rows[i][strVariavel]);
                
                if (dTable.Rows[i]["Mapa" + strIDmapa].ToString() != "")
                    idDados[i] = Convert.ToInt32(dTable.Rows[i]["Mapa" + strIDmapa]);
            } // for

            //Ordena o vetor
            Array.Sort(dblDados, idDados);

            limitesClasses = equal(dblDados, nClasses);

            //Inicializa o vetor temático
            int[] vetorTematico = new int[nDados];

            //Guarda os valores no vetorTemático
            for (int i = 0; i < nDados; i++)
                for (int c = 0; c < nClasses; c++)
                    if (dblDados[i] <= limitesClasses[c])
                    {
                        vetorTematico[idDados[i]] = c;
                        break;
                    } // if

            //Retorna o resultado
            return (vetorTematico);
        } // criaEqual()

        public int[] criaManual(DataTable dTable, string strVariavel, string strIDmapa, string strID, int nClasses, ref double[] limitesClasses)
        {
            //Número de observações na base
            int nDados = dTable.Rows.Count;

            //Guarda os dados de interesse no vetor
            double[] dblDados = new double[nDados];
            int[] idDados = new int[nDados];
            
            //Guarda os dados no vetor
            for (int i = 0; i < nDados; i++)
            {
                if (dTable.Rows[i][strVariavel].ToString() != "")
                    dblDados[i] = Convert.ToDouble(dTable.Rows[i][strVariavel]);
                
                if (dTable.Rows[i]["Mapa" + strIDmapa].ToString() != "")
                    idDados[i] = Convert.ToInt32(dTable.Rows[i]["Mapa" + strIDmapa]);
            } // for

            //Ordena o vetor
            Array.Sort(dblDados, idDados);

            //limitesClasses = equal(dblDados, nClasses);

            //Inicializa o vetor temático
            int[] vetorTematico = new int[nDados];

            //Guarda os valores no vetorTemático
            for (int i = 0; i < nDados; i++)
                for (int c = 0; c < nClasses; c++)
                    if (dblDados[i] <= limitesClasses[c])
                    {
                        vetorTematico[idDados[i]] = c;
                        break;
                    } // if

            //Retorna o resultado
            return (vetorTematico);
        } // criaManual()

        /// <summary>
        /// Cria o vetor temático de classes por valores unicos
        /// </summary>
        /// <param name="dTable"></param>
        /// <param name="strVariavel"></param>
        /// <param name="strIDmapa"></param>
        /// <param name="strID"></param>
        /// <param name="nClasses"></param>
        /// <param name="limitesClasses"></param>
        /// <returns></returns>
        public int[] criaValoresUnicos(DataTable dTable, string strVariavel, string strIDmapa, string strID, int nClasses, ref double[] limitesClasses)
        {
            //Número de observações na base
            int nDados = dTable.Rows.Count;

            //Guarda os dados de interesse no vetor
            double[] dblDados = new double[nDados];
            int[] idDados = new int[nDados];
            
            //Guarda os dados no vetor
            for (int i = 0; i < nDados; i++)
            {
                if (dTable.Rows[i][strVariavel].ToString() != "")
                    dblDados[i] = Convert.ToDouble(dTable.Rows[i][strVariavel]);
                
                if (dTable.Rows[i]["Mapa" + strIDmapa].ToString() != "")
                    idDados[i] = Convert.ToInt32(dTable.Rows[i]["Mapa" + strIDmapa]);
            } // for

            //Ordena o vetor
            Array.Sort(dblDados, idDados);

            limitesClasses = equal(dblDados, nClasses);

            //Inicializa o vetor temático
            int[] vetorTematico = new int[nDados];

            //Guarda os valores no vetorTemático
            for (int i = 0; i < nDados; i++)
                for (int c = 0; c < nClasses; c++)
                    if (dblDados[i] <= limitesClasses[c])
                    {
                        vetorTematico[idDados[i]] = c;
                        break;
                    } // if

            //Retorna o resultado
            return (vetorTematico);
        } // criaValoresUnicos()
        
        /// <summary>
        /// Cria o vetor temático de classes por desvio
        /// </summary>
        /// <param name="dTable">DataTable</param>
        /// <param name="strVariavel">Variável a ser classificada</param>
        /// <param name="strIDmapa">ID do Mapa</param>
        /// <param name="strID">ID da Base</param>
        /// <param name="nClasses">Número de classes</param>
        /// <param name="limitesClasses">Vetor de saida com os limites das classes</param>
        /// <returns></returns>
        public int[] criaDesvios(DataTable dTable, string strVariavel, string strIDmapa, string strID, double nClasses, ref double[] limitesClasses, ref int numclass)
        {
            //Número de observações na base
            int nDados = dTable.Rows.Count;

            //Guarda os dados de interesse no vetor
            double[] dblDados = new double[nDados];
            int[] idDados = new int[nDados];

            //Guarda os dados no vetor
            for (int i = 0; i < nDados; i++)
            {
                if (dTable.Rows[i][strVariavel].ToString() != "")
                    dblDados[i] = Convert.ToDouble(dTable.Rows[i][strVariavel]);
                
                if (dTable.Rows[i]["Mapa" + strIDmapa].ToString() != "")
                    idDados[i] = Convert.ToInt32(dTable.Rows[i]["Mapa" + strIDmapa]);
            } // for

            //Ordena o vetor
            Array.Sort(dblDados, idDados);

            limitesClasses = std(dblDados, nClasses);
            numclass = stdnclasses;

            //Inicializa o vetor temático
            int[] vetorTematico = new int[nDados];

            //Guarda os valores no vetorTemático
            for (int i = 0; i < nDados; i++)
                for (int c = 0; c < stdnclasses; c++)
                    if (dblDados[i] <= limitesClasses[c])
                    {
                        vetorTematico[idDados[i]] = c;
                        break;
                    } // if

            //Retorna o resultado
            return (vetorTematico);
        } // criaDesvios()

        #region Funções Mapa Temático

        private double[] equal(double[] vetor, int nclasses)
        {
            Array.Sort(vetor);
            int total = vetor.Length;
            double[] breaks = new double[nclasses];
            breaks[nclasses - 1] = vetor[total - 1];
            double max = vetor.Max();
            double min = vetor.Min();
            double diferenca = max - min;
            double posicao = diferenca / nclasses;
            for (int i = 0; i < nclasses - 1; i++)
            {
                double quebra = min + posicao + (posicao * (double)i);
                breaks[i] = quebra;
            } // for
            return (breaks);
        } // equal()

        private double[] geometric(double[] vetor, int nclasses)
        {
            double[] saida = new double[nclasses];
            Array.Sort(vetor);
            double classes = Convert.ToDouble(nclasses);
            double expoente = (1 / classes);
            double posicao = Math.Pow(vetor.Max(), (expoente));
            double conta = 1;

            for (int i = 0; i < nclasses; i++)
            {
                conta = conta * posicao;
                saida[i] = conta;
            } // for
			
            saida[nclasses - 1] = vetor[vetor.Length - 1];
            return (saida);
        } // geometric()

        int stdnclasses = 0;
        private double[] std(double[] vetor, double ndesvios)
        {
            Array.Sort(vetor);
            double media = vetor.Average();
            double[] vetornovo = new double[vetor.Length];
            for (int i = 0; i < vetor.Length; i++)
            {
                double numero = vetor[i] - media;
                double numero2 = Math.Pow(numero, 2);
                vetornovo[i] = numero2;
            } // for
			
            double variancia = vetornovo.Average();
            double desvio = Math.Sqrt(variancia);                            //Até aqui o que fiz foi só calcular o desvio padrão do vetor dado
            double[] vetorfinal = new double[vetor.Length];
            for (int i = 0; i < vetor.Length - 1; i++)
                vetorfinal[i] = (vetor[i] - media) / desvio;                   //Agora o vetor está em termos da sua distância para a média em desvios padrão
            
            Array.Sort(vetorfinal);
            int posicao = 0;
            double valor = vetorfinal[vetorfinal.Length - 1] - vetorfinal[0];      //Esta double contém a diferença entre o maior e o menor dado do vetor
            stdnclasses = (int)Math.Round(valor / ndesvios);
            double[] saida = new double[stdnclasses];
            int b = 0;
            for (int i = 1; i < vetorfinal.Length; i++)
                if (Math.Abs(vetorfinal[i] - vetorfinal[b]) >= ndesvios)
                {
                    b = i;
                    saida[posicao] = vetor[i];
                    posicao++;
                } // if
				
            saida[saida.Length - 1] = vetor.Max();

            return (saida);
        } // std()
		
        private double[,] Ones(double k, int nrow, int ncol)
        {
            double[,] matriz = new double[nrow, ncol];
            for (int i = 0; i < nrow; i++)
                for (int j = 0; j < ncol; j++)
                    matriz[i, j] = k;
            
            return (matriz);
        } // Ones()

        private double[] jenks(double[] var, int n)
        {
            bool aplicalog = false;
            if (var.Max() > 1000000)
            {
                for (int i = 0; i < var.Length; i++)
                {
                    if (var[i] == 0) var[i] = 0.000000001;
                    var[i] = Math.Log(var[i]);
                } // for
                aplicalog = true;
            } // if

            double[] d = (double[])var.Clone();
            Array.Sort(d);
            int k = n;
            double[,] mat1 = Ones(1, var.Length, k);
            double[,] mat2 = Ones(double.MaxValue, var.Length, k);
            for (int i = 0; i < mat2.GetLength(1); i++) mat2[0, i] = 0;
            double v = 0;
            int[] kclass = new int[k];
            double s1 = 0;
            double s2 = 0;
            double w = 0;
            double val = 0;
            int i3 = 0;
            int i4 = 0;

            for (int l = 2; l <= d.Length; l++)
            {
                s1 = 0;
                s2 = 0;
                w = 0;

                for (int m = 1; m <= l; m++)
                {
                    i3 = l - m + 1;
                    val = d[i3 - 1];
                    s2 += (val * val);
                    s1 += val;
                    w++;
                    v = s2 - (s1 * s1) / w;
                    i4 = (int)Math.Truncate((double)i3 - 1.0);

                    if (i4 != 0)
                        for (int j = 2; j <= k; j++)
                            if (mat2[l - 1, j - 1] >= (v + mat2[i4 - 1, j - 2]))
                            {
                                mat1[l - 1, j - 1] = (double)i3;
                                mat2[l - 1, j - 1] = v + mat2[i4 - 1, j - 2];
                            } // if
                } // for
                mat1[l - 1, 0] = 1;
                mat2[l - 1, 0] = v;
            } // for

            int final = d.Length - 1;
            kclass[n - 1] = final;
            k = d.Length - 1;

            for (int j = kclass.Length - 1; j >= 0; j--)
            {
                int id = (int)Math.Truncate(mat1[k, j]) - 1;
                if (j > 0)
                {
                    kclass[j - 1] = id;
                    k = id;
                } // if
            } // for

            double[] brks = new double[n];
			
            if (aplicalog == true)
                for (int i = 0; i < brks.Length; i++)
                    brks[i] = Math.Exp(d[(int)kclass[i]]);
            else
                for (int i = 0; i < brks.Length; i++)
                    brks[i] = d[(int)kclass[i]];

            return (brks);
        } // jenks()

        #endregion
        
        #endregion

        #region Cria classe Alex Shape
        public clsIpeaShape CriaEstruturaShapeFile(string strEndereco, string variavelID)
        {
            //Cria estrtura Alex
            clsIpeaShape shp_dados = new clsIpeaShape();

            SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("TEMP");

            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Guarda a informação do mapa nos Layers
            layMapa.DataSource = shapefile;

            clsIpeaPoligono pol = new clsIpeaPoligono();
            char[] separator = { ',' };
            int index = 0;
            int n_pontos;
            double[] temp_x;
            double[] temp_y;
            string num_string = "";
            CultureInfo cc = new CultureInfo("en-US", true);

            for (int iPoligono = 0; iPoligono < layMapa.DataSource.GetFeatureCount(); iPoligono++)
            {
                pol = new clsIpeaPoligono();

                //Captura o FeatureDataRow
                FeatureDataRow feature0 = layMapa.DataSource.GetFeature((uint)iPoligono);
                pol.Nome = feature0[variavelID].ToString();

                //Captura a geometria
                SharpMap.Geometries.Geometry shape0 = feature0.Geometry;

                //Geometria do poligono
                Polygon poligono = shape0 as SharpMap.Geometries.Polygon;

                //Coordenadas
                string strCoordenadas0 = shape0.ToString();
                strCoordenadas0 = strCoordenadas0.Replace("POLYGON ((", "");
                strCoordenadas0 = strCoordenadas0.Replace("MULTI(", "");
                strCoordenadas0 = strCoordenadas0.Replace("))", "");
                strCoordenadas0 = strCoordenadas0.Replace("((", "");
                strCoordenadas0 = strCoordenadas0.Replace(")", "");
                strCoordenadas0 = strCoordenadas0.Replace("(", "");

                string[] strCoordenadas_0 = strCoordenadas0.Split(separator);

                n_pontos = strCoordenadas_0.GetLength(0);
                temp_x = new double[n_pontos];
                temp_y = new double[n_pontos];

                for (int i = 0; i < n_pontos; i++)
                {
                    index = 0;
                    for (int k = 1; k < strCoordenadas_0[i].Length; k++)
                        if (Convert.ToString(strCoordenadas_0[i][k]) == " ")
                        {
                            index = k;
                            break;
                        } // if
                    
                    num_string = strCoordenadas_0[i].Substring(0, index);
                    temp_x[i] = Convert.ToDouble(num_string, cc.NumberFormat);
                    num_string = strCoordenadas_0[i].Substring(index + 1, strCoordenadas_0[i].Length - index - 1);
                    temp_y[i] = Convert.ToDouble(num_string, cc.NumberFormat);
                } // for

                pol.AddCoordenadasX(temp_x);
                pol.AddCoordenadasY(temp_y);

                if (poligono != null)
                {
                    pol.Area = poligono.Area;
                    pol.XCentroide = poligono.Centroid.X;
                    pol.YCentroide = poligono.Centroid.Y;
                } // if
                else
                {
                    area(ref pol);
                    centroid(ref pol, pol.Area);
                } // else

                shp_dados.AddPoligono(pol);
            } // for
            shapefile.Close();

            return (shp_dados);
        } // CriaEstruturaShapeFile()
        #endregion

        #region loading mapa

        #region variáveis internas adicionais

        protected SharpMap.Data.Providers.ShapeFile m_shapefile;
        protected SharpMap.Layers.VectorLayer m_layMapa;

        #endregion

        public void LoadingMapa(string nome_layer, string arquivo_shape, bool salva_poligonos)
        {
            try
            {
                //Definindo um layer
                m_layMapa = new SharpMap.Layers.VectorLayer(nome_layer);

                //Adicionando variaveis:
                m_shapefile = new SharpMap.Data.Providers.ShapeFile(arquivo_shape);

                //Abre o mapa para editar a suas propriedades
                m_shapefile.Open();

                //Guarda a informação do mapa no Layer
                m_layMapa.DataSource = m_shapefile;

                //Tamanho inicial do mapa 400 x 200 pixels
                m_mapa = new SharpMap.Map();

                //Adiciona o Layer
                m_mapa.Layers.Add(m_layMapa);

                //Adiciona dados ao datatable
                DataRow dr = m_shapefile.GetFeature(0);
                DataTable dt_shape = dr.Table.Clone();
                object[] nova_linha = new object[dt_shape.Columns.Count];
                for (uint i = 0; i < m_shapefile.GetFeatureCount(); i++)
                {
                    nova_linha = m_shapefile.GetFeature(i).ItemArray;
                    dt_shape.Rows.Add(nova_linha);
                } // for
				
                m_table_dados = dt_shape;

                #region Gerando a estrutura de polígonos

                if (salva_poligonos)
                {
                    int numero_poligonos = m_layMapa.DataSource.GetFeatureCount();

                    FeatureDataRow feature;
                    SharpMap.Geometries.Geometry geoMetria;
                    Polygon poligono;
                    MultiPolygon mpoligono;

                    clsUtilTools clt = new clsUtilTools();

                    int n_vertices;
                    double[] Xcoord;
                    double[] Ycoord;

                    double[] aux_x;
                    double[] aux_y;

                    m_shape = new clsIpeaShape();
                    clsIpeaPoligono pol = new clsIpeaPoligono();
                    int k = 0;
                    int n_vertices_interior_ring = 0;
                    string[] tipos_poligonos = new string[0];

                    for (k = 0; k < numero_poligonos; k++)
                    {
                        feature = m_layMapa.DataSource.GetFeature((uint)k);
                        geoMetria = feature.Geometry;

                        //Geometria do poligono
                        poligono = geoMetria as SharpMap.Geometries.Polygon;

                        if (poligono != null)
                        {
                            n_vertices = poligono.ExteriorRing.Vertices.Count;

                            Xcoord = new double[n_vertices];
                            Ycoord = new double[n_vertices];

                            for (int i = 0; i < n_vertices; i++)
                            {
                                Xcoord[i] = poligono.ExteriorRing.Vertices[i].X;
                                Ycoord[i] = poligono.ExteriorRing.Vertices[i].Y;
                            } // for

                            tipos_poligonos = new string[1 + poligono.NumInteriorRing];
                            tipos_poligonos[0] = "ExteriorRing";

                            #region Primeira adição dia 2 de fevereiro de 2009

                            if (poligono.NumInteriorRing > 0)
                            {
                                n_vertices_interior_ring = poligono.NumInteriorRing;

                                for (int i = 0; i < poligono.NumInteriorRing; i++)
                                    n_vertices_interior_ring += poligono.InteriorRings[i].Vertices.Count;

                                aux_x = new double[n_vertices_interior_ring];
                                aux_y = new double[n_vertices_interior_ring];

                                int indice_foco = 0;
                                for (int i = 0; i < poligono.NumInteriorRing; i++)
                                {
                                    tipos_poligonos[i + 1] = "InteriorRing";

                                    aux_x[indice_foco] = double.NaN;
                                    aux_y[indice_foco] = double.NaN;
                                    indice_foco++;

                                    for (int j = 0; j < poligono.InteriorRings[i].Vertices.Count; j++)
                                    {
                                        aux_x[indice_foco] = poligono.InteriorRings[i].Vertices[j].X;
                                        aux_y[indice_foco] = poligono.InteriorRings[i].Vertices[j].Y;
                                        indice_foco++;
                                    } // for
                                } // for

                                Xcoord = clt.ConcateArraysDouble(Xcoord, aux_x);
                                Ycoord = clt.ConcateArraysDouble(Ycoord, aux_y);
                            } // if

                            #endregion

                            pol = new clsIpeaPoligono();
                            pol.Nome = "Poligono " + k.ToString();

                            pol.AddCoordenadasX(Xcoord);
                            pol.AddCoordenadasY(Ycoord);
                            pol.Area = poligono.ExteriorRing.Area;

                            pol.TiposSubpoligonos = tipos_poligonos;

                            m_shape.AddPoligono(pol);
                        } // if
                        else
                        {
                            ArrayList a_tipos_subpoligonos = new ArrayList();
                            mpoligono = geoMetria as SharpMap.Geometries.MultiPolygon;

                            int num_total_poligonos_com_inner_ring = 0;
                            if (mpoligono != null)
                            {
                                pol = new clsIpeaPoligono();
                                pol.Nome = "Poligono " + k.ToString();
                                n_vertices = mpoligono.Polygons.Count - 1;

                                for (int j = 0; j < mpoligono.Polygons.Count; j++)
                                    n_vertices += mpoligono.Polygons[j].ExteriorRing.Vertices.Count;

                                Xcoord = new double[n_vertices];
                                Ycoord = new double[n_vertices];

                                int indice_foco = 0;
                                for (int j = 0; j < mpoligono.Polygons.Count; j++)
                                {
                                    a_tipos_subpoligonos.Add("ExteriorRing");

                                    poligono = mpoligono.Polygons[j];
                                    for (int i = 0; i < poligono.ExteriorRing.Vertices.Count; i++)
                                    {
                                        Xcoord[indice_foco] = poligono.ExteriorRing.Vertices[i].X;
                                        Ycoord[indice_foco] = poligono.ExteriorRing.Vertices[i].Y;
                                        indice_foco++;
                                    } // for

                                    if (j < mpoligono.Polygons.Count - 1)
                                    {
                                        Xcoord[indice_foco] = double.NaN;
                                        Ycoord[indice_foco] = double.NaN;
                                        indice_foco++;
                                    } // if

                                    num_total_poligonos_com_inner_ring += poligono.NumInteriorRing;
                                } // for

                                pol.AddCoordenadasX(Xcoord);
                                pol.AddCoordenadasY(Ycoord);

                                #region Segunda adição dia 2 de fevereiro de 2009
                                if (num_total_poligonos_com_inner_ring > 0)
                                {
                                    double[] original_Xcoord = new double[Xcoord.GetLength(0)];
                                    double[] original_Ycoord = new double[Ycoord.GetLength(0)];

                                    for (int i = 0; i < original_Xcoord.GetLength(0); i++)
                                    {
                                        original_Xcoord[i] = Xcoord[i];
                                        original_Ycoord[i] = Ycoord[i];
                                    } // for

                                    Xcoord = new double[0];
                                    Ycoord = new double[0];

                                    for (int j = 0; j < mpoligono.Polygons.Count; j++)
                                    {
                                        poligono = mpoligono.Polygons[j];
                                        if (poligono.NumInteriorRing > 0)
                                        {
                                            n_vertices_interior_ring = poligono.NumInteriorRing;

                                            for (int i = 0; i < poligono.NumInteriorRing; i++)
                                                n_vertices_interior_ring += poligono.InteriorRings[i].Vertices.Count;

                                            aux_x = new double[n_vertices_interior_ring];
                                            aux_y = new double[n_vertices_interior_ring];

                                            indice_foco = 0;

                                            for (int i = 0; i < poligono.NumInteriorRing; i++)
                                            {
                                                a_tipos_subpoligonos.Add("InteriorRing");

                                                aux_x[indice_foco] = double.NaN;
                                                aux_y[indice_foco] = double.NaN;
                                                indice_foco++;

                                                for (int ii = 0; ii < poligono.InteriorRings[i].Vertices.Count; ii++)
                                                {
                                                    aux_x[indice_foco] = poligono.InteriorRings[i].Vertices[ii].X;
                                                    aux_y[indice_foco] = poligono.InteriorRings[i].Vertices[ii].Y;
                                                    indice_foco++;
                                                } // for
                                            } // for

                                            Xcoord = clt.ConcateArraysDouble(Xcoord, aux_x);
                                            Ycoord = clt.ConcateArraysDouble(Ycoord, aux_y);
                                        } // if
                                    } // for

                                    Xcoord = clt.ConcateArraysDouble(original_Xcoord, Xcoord);
                                    Ycoord = clt.ConcateArraysDouble(original_Ycoord, Ycoord);

                                    pol.AddCoordenadasX(Xcoord);
                                    pol.AddCoordenadasY(Ycoord);
                                } // if

                                tipos_poligonos = new string[a_tipos_subpoligonos.Count];
                                for (int i = 0; i < tipos_poligonos.GetLength(0); i++) { tipos_poligonos[i] = Convert.ToString(a_tipos_subpoligonos[i]); }
                                pol.TiposSubpoligonos = tipos_poligonos;

                                #endregion

                                m_shape.AddPoligono(pol);
                            } // if
                        } // else
                    } // for

                    m_vector_shapes = new clsIpeaShape[1];
                    m_vector_shapes[0] = new clsIpeaShape();
                    m_vector_shapes[0] = m_shape;
                } // if 

                #endregion

                //Fecha o shapeFile
                m_shapefile.Close();
            } // try
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // loadingMapa()

        #endregion
    } // class
} // namespace
