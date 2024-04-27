using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using SharpMap.Data;
using SharpMap.Geometries;

namespace IpeaGeo.RegressoesEspaciais
{
    [Obsolete("To be reimplemented in IpeaGEO.GIS")]
    class clsMapa
    {
        public clsMapa()
        {
        }

        #region variáveis internas

        protected SharpMap.Data.Providers.ShapeFile m_shapefile;
        protected SharpMap.Layers.VectorLayer m_layMapa;
        protected clsIpeaShape m_shape = new clsIpeaShape();
        protected SharpMap.Map m_mapa;
        protected DataTable m_table_dados = new DataTable();

        public DataTable TabelaDados
        {
            get { return this.m_table_dados; }
        }

        public SharpMap.Map Sharp_Mapa
        {
            get { return this.m_mapa; }
        }

        protected clsIpeaShape[] m_vector_shapes = new clsIpeaShape[0];
        public clsIpeaShape[] VetorShapes
        {
            get { return this.m_vector_shapes; }
        }

        protected System.Data.OleDb.OleDbConnection m_cnn = new System.Data.OleDb.OleDbConnection();
        protected System.Data.OleDb.OleDbDataAdapter m_dap = new System.Data.OleDb.OleDbDataAdapter();

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
                }
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
                            }

                            tipos_poligonos = new string[1 + poligono.NumInteriorRing];
                            tipos_poligonos[0] = "ExteriorRing";

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
                                    tipos_poligonos[i + 1] = "InteriorRing";

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
                            pol.Area = poligono.ExteriorRing.Area;

                            pol.TiposSubpoligonos = tipos_poligonos;

                            m_shape.AddPoligono(pol);
                        }
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
                                {
                                    n_vertices += mpoligono.Polygons[j].ExteriorRing.Vertices.Count;
                                }

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
                                                a_tipos_subpoligonos.Add("InteriorRing");

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

                                tipos_poligonos = new string[a_tipos_subpoligonos.Count];
                                for (int i = 0; i < tipos_poligonos.GetLength(0); i++) { tipos_poligonos[i] = Convert.ToString(a_tipos_subpoligonos[i]); }
                                pol.TiposSubpoligonos = tipos_poligonos;

                                //--------------------------- Fim da segunda adição do dia 2 de fevereiro de 2009 ---------------------//

                                m_shape.AddPoligono(pol);
                            }
                        }
                    }

                    m_vector_shapes = new clsIpeaShape[1];
                    m_vector_shapes[0] = new clsIpeaShape();
                    m_vector_shapes[0] = m_shape;
                }

                #endregion 

                //Fecha o shapeFile
                m_shapefile.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
