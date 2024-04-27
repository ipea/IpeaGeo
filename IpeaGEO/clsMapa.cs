using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SharpMap.Geometries;
using SharpMap.Data;
using SharpMap.Data.Providers;
using System.Collections;
using System.Linq;
using SharpMap;
using System.Globalization;
using System.IO;


namespace IpeaGEO
{
    class clsMapa
    {

        #region Mudanças Alexandre

        //private SharpMap.Data.Providers.ShapeFile m_shapefile;
        //private SharpMap.Layers.VectorLayer m_layMapa;
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
        private double Minimo(double[] vetor)
        {
            double min = vetor[0];
            int iIndex = 0;
            for (int i = 0; i < vetor.Length; i++)
            {
                if (vetor[i] <= min)
                {
                    min = vetor[i];
                    iIndex = i;
                }
            }
            return (min);
        }
        private double Maximo(double[] vetor)
        {
            double max = vetor[0];
            int iIndex = 0;
            for (int i = 0; i < vetor.Length; i++)
            {
                if (vetor[i] >= max)
                {
                    max = vetor[i];
                    iIndex = i;
                }
            }
            return (max);
        }

        private void CalculaArea(ref clsIpeaPoligono poligono)
        {
            double area = 0;
            double[] X = new double[poligono.Count + 1];
            double[] Y = new double[poligono.Count + 1];
            for (int i = 0; i < poligono.Count; i++)
            {
                X[i] = poligono.X(i);
                Y[i] = poligono.Y(i);
            }
            X[poligono.Count] = poligono.X(0);
            Y[poligono.Count] = poligono.Y(0);

            for (int i = 0; i < poligono.Count; i++)
            {
                area += (X[i] * Y[i + 1]) - (X[i + 1] * Y[i]);
            }
            poligono.Area = (area / 2.0);
        }

        private void CalculaCentroide(ref clsIpeaPoligono poligono, double area)
        {
            double centroideX = 0;
            double centroideY = 0;
            double X0 = poligono.X(0);
            double Y0 = poligono.X(0);
            for (int i = 0; i < poligono.Count; i++)
            {
                centroideX += (poligono.X(i) + poligono.X(i + 1)) * ((poligono.X(i) * poligono.Y(i + 1)) - (poligono.X(i + 1) * poligono.Y(i)));
                centroideY += (poligono.Y(i) + poligono.Y(i + 1)) * ((poligono.X(i) * poligono.Y(i + 1)) - (poligono.X(i + 1) * poligono.Y(i)));
            }
            centroideX += (poligono.X(poligono.Count - 1) + X0) * ((poligono.X(poligono.Count - 1) * Y0) - (X0 * poligono.Y(poligono.Count - 1)));
            centroideY += (poligono.Y(poligono.Count - 1) + Y0) * ((poligono.X(poligono.Count - 1) * Y0) - (X0 * poligono.Y(poligono.Count - 1)));

            poligono.XCentroide = (centroideX / (6.0 * area));
            poligono.YCentroide = (centroideY / (6.0 * area));
        }

        #endregion

        #region Ponto no Poligono

        public bool PointInPolygon(SharpMap.Geometries.Point mPontos, SharpMap.Geometries.Polygon Poligono)
        {
            SharpMap.Geometries.Point p1, p2;

            bool inside = false;

            if (Poligono.ExteriorRing.Vertices.Count < 3)
            {
                return inside;
            }

            SharpMap.Geometries.Point oldPoint = new SharpMap.Geometries.Point(Poligono.ExteriorRing.Vertices[Poligono.ExteriorRing.Vertices.Count - 1].X, Poligono.ExteriorRing.Vertices[Poligono.ExteriorRing.Vertices.Count - 1].Y);

            for (int i = 0; i < Poligono.ExteriorRing.Vertices.Count; i++)
            {
                SharpMap.Geometries.Point newPoint = new SharpMap.Geometries.Point(Poligono.ExteriorRing.Vertices[i].X, Poligono.ExteriorRing.Vertices[i].Y);
                if (newPoint.X > oldPoint.X)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }
                if ((newPoint.X < mPontos.X) == (mPontos.X <= oldPoint.X) && (mPontos.Y - p1.Y) * (p2.X - p1.X) < (p2.Y - p1.Y) * (mPontos.X - p1.X))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }
            return inside;
        }


        /// <summary>
        /// Função para verificar se um ponto está ou não em um determinado poligono.
        /// </summary>
        /// <param name="strEndereco">Endereço do aqrquivo shapefile.</param>
        /// <param name="uID">ID do poligono de interesse.</param>
        /// <param name="X">Coordenada X do ponto.</param>
        /// <param name="Y">Coordenada Y do ponto.</param>
        /// <returns></returns>
        public bool pontosNoPoligono(string strEndereco, uint uID, double X, double Y)
        {
            //Inicializa o resultado
            bool resutado = false;

            //Guarda os pontos
            SharpMap.Geometries.Point mPontos = new SharpMap.Geometries.Point(X, Y);

            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Layer temporario
            SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("layTemporario");

            //Guarda a informação do mapa no Layer
            layMapa.DataSource = shapefile;

            //Captura o FeatureDataRow
            FeatureDataRow feature1 = layMapa.DataSource.GetFeature(uID);

            //Captura a geometria
            SharpMap.Geometries.Geometry geoMetria1 = feature1.Geometry;

            //Geometria do poligono
            Polygon poligono1 = geoMetria1 as SharpMap.Geometries.Polygon;

            //Fecha o shapeFile
            shapefile.Close();

            if (poligono1 != null)
            {
                //Resultado
                resutado = PointInPolygon(mPontos, poligono1);
            }

            //Retorna o resultado
            return (resutado);
        }

        public int identificaPontoPoligono(string strEndereco, double X, double Y)
        {
            //Guarda os pontos
            SharpMap.Geometries.Point mPontos = new SharpMap.Geometries.Point(X, Y);

            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Layer temporario
            SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("layTemporario");

            //Guarda a informação do mapa no Layer
            layMapa.DataSource = shapefile;

            //Número do poligono
            int iPoligono = 0;

            for (iPoligono = 0; iPoligono < layMapa.DataSource.GetFeatureCount(); iPoligono++)
            {

                //Captura o FeatureDataRow
                FeatureDataRow feature1 = layMapa.DataSource.GetFeature((uint)iPoligono);

                //Captura a geometria
                SharpMap.Geometries.Geometry geoMetria1 = feature1.Geometry;

                //Geometria do poligono
                Polygon poligono1 = geoMetria1 as SharpMap.Geometries.Polygon;

                if (poligono1 != null)
                {
                    //Resultado
                    bool resutado = PointInPolygon(mPontos, poligono1);

                    if (resutado == true)
                    {
                        return (iPoligono);
                    }
                }
            }

            //Fecha o shapeFile
            shapefile.Close();

            //Retorna o resultado
            return (iPoligono);
        }

        #endregion

        #region Funções do SharpMap

        /// <summary>
        /// Calcula a distancia pelo método de Haversine
        /// </summary>
        /// <param name="from">Ponto de origem.</param>
        /// <param name="to">Ponto de destino.</param>
        /// <returns></returns>
        public double calDistancia(SharpMap.Geometries.Point from, SharpMap.Geometries.Point to)
        {
            double rad = 6371; //Earth radius in Km
            //Convert to radians
            double p1X = from.X / 180 * Math.PI;
            double p1Y = from.Y / 180 * Math.PI;
            double p2X = to.X / 180 * Math.PI;
            double p2Y = to.Y / 180 * Math.PI;

            return Math.Acos(Math.Sin(p1Y) * Math.Sin(p2Y) +
                Math.Cos(p1Y) * Math.Cos(p2Y) * Math.Cos(p2X - p1X)) * rad;
        }

        /// <summary>
        /// Faz o load do mapa no MapImage
        /// </summary>
        /// <param name="mapImage">MapImage.</param>
        /// <param name="strEndereco">Endereço do shapeFile.</param>
        /// <param name="strLayer">Nome do Layer a ser adicionado no mapa.</param>
        public void loadingMapa(ref SharpMap.Forms.MapImage mapImage, string strEndereco, string strLayer,ref clsIpeaShape shapeAlex)
        {

            //Definindo um layer
            SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer(strLayer);

            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEndereco);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Guarda a informação do mapa no Layer
            layMapa.DataSource = shapefile;

            #region Gerando a estrutura de polígonos

            int numero_poligonos = layMapa.DataSource.GetFeatureCount();

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

            clsAreaPerimetroCentroide minhafuncao = new clsAreaPerimetroCentroide();
            
            //FileInfo t = new FileInfo(@"F:\IpeaGEO.txt");
            //StreamWriter Tex = t.CreateText();
            //Tex.WriteLine("MUN\tPOLIGONO");
            for (k = 0; k < numero_poligonos; k++)
            {

                //string AMC = layMapa.DataSource.GetFeature((uint)k)["MUNICÍPI0"].ToString();
                //Tex.WriteLine(AMC + "\t" + k.ToString());

                feature = layMapa.DataSource.GetFeature((uint)k);
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
                    }

                    //--------------------------- Primeira adição dia 2 de fevereiro de 2009 -------------------------------//

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
                            aux_x[indice_foco] = double.NaN;
                            aux_y[indice_foco] = double.NaN;
                            indice_foco++;

                            for (int j = 0; j < poligono.InteriorRings[i].Vertices.Count; j++)
                            {
                                aux_x[indice_foco] = poligono.InteriorRings[i].Vertices[j].X;
                                aux_y[indice_foco] = poligono.InteriorRings[i].Vertices[j].Y;
                                indice_foco++;
                            }
                        }

                        Xcoord = clt.ConcateArraysDouble(Xcoord, aux_x);
                        Ycoord = clt.ConcateArraysDouble(Ycoord, aux_y);
                    }

                    //--------------------------- Fim da primeira adição do dia 2 de fevereiro de 2009 ---------------------//

                    pol = new clsIpeaPoligono();
                    pol.Nome = "Poligono " + k.ToString();

                    pol.AddCoordenadasX(Xcoord);
                    pol.AddCoordenadasY(Ycoord);
                    pol.Area = minhafuncao.area(Xcoord, Ycoord);
                    //double[] centroide = minhafuncao.centroid(Xcoord, Ycoord);
                    //pol.XCentroide = centroide[0];
                    //pol.YCentroide = centroide[1];
                    pol.XCentroide = poligono.Centroid.X ;
                    pol.YCentroide = poligono.Centroid.Y;

                    pol.IndiceCluster = -1;

                    m_shape.AddPoligono(pol);
                }
                else
                {
                    mpoligono = geoMetria as SharpMap.Geometries.MultiPolygon;

                    int num_total_poligonos_com_inner_ring = 0;

                    if (mpoligono != null)
                    {
                        pol = new clsIpeaPoligono();
                        pol.Nome = "Poligono " + k.ToString();

                        n_vertices = mpoligono.Polygons.Count - 1;

                        for (int j = 0; j < mpoligono.Polygons.Count; j++)
                        {
                            n_vertices += mpoligono.Polygons[j].ExteriorRing.Vertices.Count;
                        }

                        Xcoord = new double[n_vertices];
                        Ycoord = new double[n_vertices];

                        int indice_foco = 0;

                        for (int j = 0; j < mpoligono.Polygons.Count; j++)
                        {
                            poligono = mpoligono.Polygons[j];
                            for (int i = 0; i < poligono.ExteriorRing.Vertices.Count; i++)
                            {
                                Xcoord[indice_foco] = poligono.ExteriorRing.Vertices[i].X;
                                Ycoord[indice_foco] = poligono.ExteriorRing.Vertices[i].Y;
                                indice_foco++;
                            }

                            if (j < mpoligono.Polygons.Count - 1)
                            {
                                Xcoord[indice_foco] = double.NaN;
                                Ycoord[indice_foco] = double.NaN;
                                indice_foco++;
                            }

                            num_total_poligonos_com_inner_ring += poligono.NumInteriorRing;
                        }

                        pol.AddCoordenadasX(Xcoord);
                        pol.AddCoordenadasY(Ycoord);

                        //--------------------------- Segunda adição dia 2 de fevereiro de 2009 -------------------------------//

                        if (num_total_poligonos_com_inner_ring > 0)
                        {
                            double[] original_Xcoord = new double[Xcoord.GetLength(0)];
                            double[] original_Ycoord = new double[Ycoord.GetLength(0)];

                            for (int i = 0; i < original_Xcoord.GetLength(0); i++)
                            {
                                original_Xcoord[i] = Xcoord[i];
                                original_Ycoord[i] = Ycoord[i];
                            }

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
                                        aux_x[indice_foco] = double.NaN;
                                        aux_y[indice_foco] = double.NaN;
                                        indice_foco++;

                                        for (int ii = 0; ii < poligono.InteriorRings[i].Vertices.Count; ii++)
                                        {
                                            aux_x[indice_foco] = poligono.InteriorRings[i].Vertices[ii].X;
                                            aux_y[indice_foco] = poligono.InteriorRings[i].Vertices[ii].Y;
                                            indice_foco++;
                                        }
                                    }

                                    Xcoord = clt.ConcateArraysDouble(Xcoord, aux_x);
                                    Ycoord = clt.ConcateArraysDouble(Ycoord, aux_y);
                                }
                            }

                            Xcoord = clt.ConcateArraysDouble(original_Xcoord, Xcoord);
                            Ycoord = clt.ConcateArraysDouble(original_Ycoord, Ycoord);

                            pol.AddCoordenadasX(Xcoord);
                            pol.AddCoordenadasY(Ycoord);
                        }

                        //--------------------------- Fim da segunda adição do dia 2 de fevereiro de 2009 ---------------------//
                        pol.IndiceCluster = -1;
                        m_shape.AddPoligono(pol);
                    }
                }
            }

            //Tex.Close();

            m_vector_shapes = new clsIpeaShape[1];
            m_vector_shapes[0] = new clsIpeaShape();
            m_vector_shapes[0] = m_shape;
            

            #endregion 

            //Fecha o shapeFile
            shapefile.Close();

            //Cor do mapa
            layMapa.Style.Fill = new SolidBrush(Color.White);

            //Cor da linha
            layMapa.Style.Outline = System.Drawing.Pens.Black;
            layMapa.Style.EnableOutline = true;

            //Tamanho inicial do mapa 400 x 200 pixels
            SharpMap.Map mMapa = new SharpMap.Map(new Size(400, 200));

            //Adiciona o Layer
            mMapa.Layers.Add(layMapa);

            //Zoom todo o mapa.
            mMapa.ZoomToExtents();

            //Coloca o mapa no mapImage
            mapImage.Map = mMapa;

            //Refresh o mapa no mapImage
            mapImage.Refresh();

            //Guarda a estrutura do Shape
            shapeAlex = m_shape;

        }

       

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
            {
                foreach (var verice in poligono.ExteriorRing.Vertices)
                {
                    pontos.Add(new PointF((float)verice.X, (float)verice.Y));
                }
            }

            //Fecha o shapeFile
            shapefile.Close();

            return (pontos);
        }

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
                {
                    mPontos.Add(new PointF((float)poligono.Centroid.X, (float)poligono.Centroid.Y));
                }
            }

            //Fecha o shapeFile
            shapefile.Close();

            return (mPontos);
        }

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
        }

       
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
            {
                foreach (var verice in poligono.ExteriorRing.Vertices)
                {
                    pontos.Add(new PointF((float)verice.X, (float)verice.Y));
                }
            }

            //Fecha o shapeFile
            shapefile.Close();

            return (pontos);
        }

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
            {
                strVariaveis[i] = feature.Table.Columns[i].ToString();
            }

            //Fecha o shapeFile
            shapefile.Close();

            return (strVariaveis);
        }

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
            }
            else if (strTipo == "System.Int16")
            {
                Int16 strInt = feature.Field<Int16>(strVariavel);
                strDados = strInt.ToString();
            }
            else if (strTipo == "System.Int64")
            {
                Int64 strInt = feature.Field<Int64>(strVariavel);
                strDados = strInt.ToString();
            }
            else if (strTipo == "System.Double")
            {
                double strInt = feature.Field<double>(strVariavel);
                strDados = strInt.ToString();
            }
            else
            {
                strDados = feature.Field<string>(strVariavel);
            }

            //Fecha o shapeFile
            shapefile.Close();

            return (strDados);
        }

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
            {
                for (int i = 0; i < poligono1.ExteriorRing.Vertices.Count; i++)
                {
                    for (int j = 0; j < poligono2.ExteriorRing.Vertices.Count; j++)
                    {
                        if ((poligono1.ExteriorRing.Vertices[i].X == poligono2.ExteriorRing.Vertices[j].X) && (poligono1.ExteriorRing.Vertices[i].Y == poligono2.ExteriorRing.Vertices[j].Y))
                        {
                            //Fecha o shapeFile
                            shapefile.Close();
                            return (true);
                        }
                    }
                }
            }


            //Fecha o shapeFile
            shapefile.Close();

            //Retorna o resultado
            return (false);
        }

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
                }
            }
        }

        public void ConeccaoEntreMapaEdataTable(ref DataTable dTable, string strEnderecoShape, string strID, string strIDmapa, ref ProgressBar pBar,ref clsIpeaShape classeShape )
        {
            try
            {

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

                    //Encontra a observação de interesse
                    for (int j = 0; j < arLista.Count; j++)
                    {
                        //Encontra o Identificador do mapa para o j-ésimo polígono
                        string strIdent = layMapa.DataSource.GetFeature((uint)j)[strIDmapa].ToString();

                        //Captura os dados do j-ésimo polígono
                        FeatureDataRow ftDataRow1 = layMapa.DataSource.GetFeature((uint)j);

                        //Procura na base de dados a linha que contenha o ID do j-ésimo polígono
                        int i = arLista.BinarySearch(strIdent);

                        //Dados da base que é igual a base do mapa
                        DataRow drBase = dTable.Rows[i];

                        if (i > -1)
                        {
                            dTable.Rows[i]["Mapa" + strIDmapa] = j;
                            classeShape[j].PosicaoNoDataTable = i;
                            classeShape[j].ID = strIdent;
                        }
                        else
                        {
                            classeShape[j].PosicaoNoDataTable = -1;
                            classeShape[j].ID = "Null";
                        }

                        //Incrementa a progressbar.
                        pBar.Increment(1);
                        Application.DoEvents();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
                {
                    dblDados[i] = Convert.ToDouble(dTable.Rows[i][strVariavel]);
                }
                if (dTable.Rows[i]["Mapa" + strIDmapa].ToString() != "")
                {
                    idDados[i] = Convert.ToInt32(dTable.Rows[i]["Mapa" + strIDmapa]);
                }
            }

            //Ordena o vetor
            Array.Sort(dblDados, idDados);

            for (int c = 0; c < nClasses; c++)
            {
                double fator = ((double)c + 1) / (double)nClasses;
                int iPosicao = (int)Math.Ceiling(fator * ((double)nDados - 1));
                limitesClasses[c] = dblDados[iPosicao];
            }

            //Inicializa o vetor temático
            int[] vetorTematico = new int[nDados];

            //Guarda os valores no vetorTemático
            for (int i = 0; i < nDados; i++)
            {
                for (int c = 0; c < nClasses; c++)
                {
                    if (dblDados[i] <= limitesClasses[c])
                    {
                        vetorTematico[idDados[i]] = c;
                        break;
                    }
                }
            }

            //Retorna o resultado
            return (vetorTematico);
        }


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
                {
                    dblDados[i] = Convert.ToDouble(dTable.Rows[i][strVariavel]);
                }
                if (dTable.Rows[i]["Mapa" + strIDmapa].ToString() != "")
                {
                    idDados[i] = Convert.ToInt32(dTable.Rows[i]["Mapa" + strIDmapa]);
                }
            }

            //Ordena o vetor
            Array.Sort(dblDados, idDados);

            limitesClasses = geometric(dblDados, nClasses);

            //Inicializa o vetor temático
            int[] vetorTematico = new int[nDados];

            //Guarda os valores no vetorTemático
            for (int i = 0; i < nDados; i++)
            {
                for (int c = 0; c < nClasses; c++)
                {
                    if (dblDados[i] <= limitesClasses[c])
                    {
                        vetorTematico[idDados[i]] = c;
                        break;
                    }
                }
            }

            //Retorna o resultado
            return (vetorTematico);
        }

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
                {
                    dblDados[i] = Convert.ToDouble(dTable.Rows[i][strVariavel]);
                }
                if (dTable.Rows[i]["Mapa" + strIDmapa].ToString() != "")
                {
                    idDados[i] = Convert.ToInt32(dTable.Rows[i]["Mapa" + strIDmapa]);
                }
            }

            //Ordena o vetor
            Array.Sort(dblDados, idDados);

            limitesClasses = jenks(dblDados, nClasses);

            //Inicializa o vetor temático
            int[] vetorTematico = new int[nDados];

        

            //Guarda os valores no vetorTemático
            for (int i = 0; i < nDados; i++)
            {
                dblDados[i]=Math.Exp(dblDados[i]);
                for (int c = 0; c < nClasses; c++)
                {
                    if (dblDados[i] <= limitesClasses[c])
                    {
                        vetorTematico[idDados[i]] = c;
                        break;
                    }
                }
            }



            //Retorna o resultado
            return (vetorTematico);
        }

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
                {
                    dblDados[i] = Convert.ToDouble(dTable.Rows[i][strVariavel]);
                }
                if (dTable.Rows[i]["Mapa" + strIDmapa].ToString() != "")
                {
                    idDados[i] = Convert.ToInt32(dTable.Rows[i]["Mapa" + strIDmapa]);
                }
            }

            //Ordena o vetor
            Array.Sort(dblDados, idDados);

            limitesClasses = equal(dblDados, nClasses);

            //Inicializa o vetor temático
            int[] vetorTematico = new int[nDados];

            //Guarda os valores no vetorTemático
            for (int i = 0; i < nDados; i++)
            {
                for (int c = 0; c < nClasses; c++)
                {
                    if (dblDados[i] <= limitesClasses[c])
                    {
                        vetorTematico[idDados[i]] = c;
                        break;
                    }
                }
            }

            //Retorna o resultado
            return (vetorTematico);
        }

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
        public int[] criaDesvios(DataTable dTable, string strVariavel, string strIDmapa, string strID, double nClasses, ref double[] limitesClasses)
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
                {
                    dblDados[i] = Convert.ToDouble(dTable.Rows[i][strVariavel]);
                }
                if (dTable.Rows[i]["Mapa" + strIDmapa].ToString() != "")
                {
                    idDados[i] = Convert.ToInt32(dTable.Rows[i]["Mapa" + strIDmapa]);
                }
            }

            //Ordena o vetor
            Array.Sort(dblDados, idDados);

            limitesClasses = std(dblDados, nClasses);
            

            //Inicializa o vetor temático
            int[] vetorTematico = new int[nDados];

            //Guarda os valores no vetorTemático
            for (int i = 0; i < nDados; i++)
            {
                for (int c = 0; c < stdnclasses; c++)
                {
                    if (dblDados[i] <= limitesClasses[c])
                    {
                        vetorTematico[idDados[i]] = c;
                        break;
                    }
                }
            }

            //Retorna o resultado
            return (vetorTematico);
        }

        #region Funções Mapa Temático
        
        private double[] equal(double[] vetor, int nclasses)
        {
            Array.Sort(vetor);
            int total = vetor.Length;
            double[] breaks = new double[nclasses];
            breaks[nclasses-1] = vetor[total - 1];
            double max = vetor.Max();
            double min = vetor.Min();
            double diferenca = max - min;
            double posicao = diferenca / nclasses;
            for (int i = 0; i < nclasses - 1; i++)
            {
                double quebra = min + posicao + (posicao * (double)i);
                breaks[i] = quebra;
            }
            return (breaks);
        }

        private double[] quantil(double[] vetor, int nclasses)
        {
            double[] saida = new double[nclasses - 1];
            Array.Sort(vetor);
            int partes = (vetor.Length / nclasses);
            int conta = partes;
            for (int i = 0; i < nclasses - 1; i++)
            {
                saida[i] = vetor[conta];
                conta += partes;
            }

            return (saida);

        }

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
                               
            }
            saida[nclasses - 1] = vetor[vetor.Length - 1];
            return (saida);
        }

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
            }
            double variancia = vetornovo.Average();
            double desvio = Math.Sqrt(variancia);                            //Até aqui o que fiz foi só calcular o desvio padrão do vetor dado
            double[] vetorfinal = new double[vetor.Length];
            for (int i = 0; i < vetor.Length - 1; i++)
            {
                vetorfinal[i] = (vetor[i] - media) / desvio;                   //Agora o vetor está em termos da sua distância para a média em desvios padrão
            }
            Array.Sort(vetorfinal);
            int posicao = 0;
            double valor = vetorfinal[vetorfinal.Length - 1] - vetorfinal[0];      //Esta double contém a diferença entre o maior e o menor dado do vetor
            stdnclasses = (int)Math.Round(valor / ndesvios);
            double[] saida = new double[stdnclasses];
            int b = 0;
            for (int i = 1; i < vetorfinal.Length; i++)
            {

                if (Math.Abs(vetorfinal[i] - vetorfinal[b]) >= ndesvios)
                {
                    b = i - 1;
                    saida[posicao] = vetor[i];
                    posicao++;
                }

            }
            saida[saida.Length - 1] = vetor.Max();

            return (saida);
        }

        private void Ks(ref double[,] matriz, double k)
        {
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    matriz[i, j] = k;
                }
            }
        }
        private double[,] Ones(double k, int nrow, int ncol)
        {
            double[,] matriz = new double[nrow, ncol];
            for (int i = 0; i < nrow; i++)
            {
                for (int j = 0; j < ncol; j++)
                {
                    matriz[i, j] = k;
                }
            }
            return (matriz);
        }



        private double[] jenks(double[] var, int n)
        {
            bool aplicalog = false;
            if (var.Max() > 1000000)
            {
                for (int i = 0; i < var.Length; i++)
                {
                    if (var[i] == 0)
                    {
                        var[i] = 0.000000001;
                    }
                    var[i] = Math.Log(var[i]);
                }
                aplicalog = true;
            }

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
                    {
                        for (int j = 2; j <= k; j++)
                        {
                            if (mat2[l - 1, j - 1] >= (v + mat2[i4 - 1, j - 2]))
                            {
                                mat1[l - 1, j - 1] = (double)i3;
                                mat2[l - 1, j - 1] = v + mat2[i4 - 1, j - 2];
                            }
                        }
                    }
                }
                mat1[l - 1, 0] = 1;
                mat2[l - 1, 0] = v;
            }

            int final = d.Length - 1;
            kclass[n - 1] = final;
            k = d.Length - 1;
            //int last = d.Length;

            for (int j = kclass.Length - 1; j >= 0; j--)
            {
                int id = (int)Math.Truncate(mat1[k, j]) - 1;
                if (j > 0)
                {
                    kclass[j - 1] = id;
                    k = id;
                }
                //last = k-1;
            }


            double[] brks = new double[n];


            if (aplicalog == true)
            {
                for (int i = 0; i < brks.Length; i++)
                {
                    brks[i] = Math.Exp(d[(int)kclass[i]]);
                }
            }
            else
            {
                for (int i = 0; i < brks.Length; i++)
                {
                    brks[i] = d[(int)kclass[i]];
                }
            }
            
            return (brks);
        }


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
                    {
                        if (Convert.ToString(strCoordenadas_0[i][k]) == " ")
                        {
                            index = k;
                            break;
                        }
                    }
                    num_string = strCoordenadas_0[i].Substring(0, index);
                    temp_x[i] = Convert.ToDouble(num_string, cc.NumberFormat);
                    num_string = strCoordenadas_0[i].Substring(index + 1, strCoordenadas_0[i].Length - index - 1);
                    temp_y[i] = Convert.ToDouble(num_string, cc.NumberFormat);
                }

                pol.AddCoordenadasX(temp_x);
                pol.AddCoordenadasY(temp_y);

                if(poligono!=null)
                {
                    pol.Area = poligono.Area;
                    pol.XCentroide = poligono.Centroid.X;
                    pol.YCentroide = poligono.Centroid.Y;
                }
                else
                {
                    CalculaArea(ref pol);
                    CalculaCentroide(ref pol, pol.Area);
                }
                

                shp_dados.AddPoligono(pol);
            }
            shapefile.Close();

            return (shp_dados);
        }
        #endregion
    }
}
