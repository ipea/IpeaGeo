using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpMap.Geometries;
using SharpMap.Data;
using SharpMap.Data.Providers;
using System.IO;
using System.Collections;
using System.Globalization;

namespace IpeaGEO
{
    public partial class frmGeoreferenciamento : Form
    {
        public frmGeoreferenciamento()
        {
            InitializeComponent();
        }

        private string strEnderecoShape;
        public string EnderecoShape
        {
            get
            {
                return strEnderecoShape;
            }
            set
            {
                strEnderecoShape = value;
            }
        }

        private clsIpeaShape shapeAlex;
        public clsIpeaShape EstruturaShape
        {
            get
            {
                return shapeAlex;
            }
            set
            {
                shapeAlex = value;
            }
        }


        /*Material como trabalhar com Dialogs:  http://www.techexams.net/blogs/net/70-526/creating-dialog-boxes-in-net*/
        private void frmGeoreferenciamento_Load(object sender, EventArgs e)
        {
            //Acha as variaveis do mapa
            clsMapa classeMapa = new clsMapa();
            string[] strVariaveisMapa = classeMapa.informacaoVariaveis(strEnderecoShape, (uint)0);

            //Coloca na combobox
            for (int i = 0; i < strVariaveisMapa.Length; i++) cmbID.Items.Add(strVariaveisMapa[i]);

        }

        private void btnExporta_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            //Definindo os Layers
            SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("TEMP");
            SharpMap.Layers.VectorLayer layMapa2 = new SharpMap.Layers.VectorLayer("TEMP2");
            SharpMap.Layers.VectorLayer layMapa3 = new SharpMap.Layers.VectorLayer("TEMP3");
            SharpMap.Layers.VectorLayer layMapa4 = new SharpMap.Layers.VectorLayer("TEMP4");
            SharpMap.Layers.VectorLayer layMapa5 = new SharpMap.Layers.VectorLayer("TEMP5");
            SharpMap.Layers.VectorLayer layMapa6 = new SharpMap.Layers.VectorLayer("TEMP6");

            //Adicionando variaveis:
            SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEnderecoShape);

            //Abre o mapa para editar a suas propriedades
            shapefile.Open();

            //Guarda a informação do mapa nos Layers
            layMapa.DataSource = shapefile;
            layMapa2.DataSource = shapefile;
            layMapa3.DataSource = shapefile;
            layMapa4.DataSource = shapefile;
            layMapa5.DataSource = shapefile;
            layMapa6.DataSource = shapefile;

            clsIpeaShape shp_dados = new clsIpeaShape();

            if (ckbEstruturaVizinhanca.Checked)
            {
                //Inicializa a progress bar
                progressBar1.Value = 0;
                progressBar1.Maximum = layMapa.DataSource.GetFeatureCount();
                progressBar1.Minimum = 0;
                Application.DoEvents();

                //Remove as variáveis caso existam.
                if (dataSet1.Tables["tblVizinhanca"].Columns.Contains(cmbID.SelectedItem.ToString()) == true) dataSet1.Tables["tblVizinhanca"].Columns.Remove(cmbID.SelectedItem.ToString() + "_1");
                dataSet1.Tables["tblVizinhanca"].Columns.Add(cmbID.SelectedItem.ToString() + "_1", Type.GetType("System.String"));
                if (dataSet1.Tables["tblVizinhanca"].Columns.Contains(cmbID.SelectedItem.ToString()) == true) dataSet1.Tables["tblVizinhanca"].Columns.Remove(cmbID.SelectedItem.ToString() + "_2");
                dataSet1.Tables["tblVizinhanca"].Columns.Add(cmbID.SelectedItem.ToString() + "_2", Type.GetType("System.String"));

                //Define a classe dos Poligonos
                clsIpeaPoligono pol = new clsIpeaPoligono();

                //Salva estrutura temporária
                shp_dados = shapeAlex;

                //Cria a vizinhnaça
                clsIpeaShape cps = new clsIpeaShape();

                //TODO: Programar a geração de vizinhos de ordem superior a 1.
                int tipo_vizinhanca = 1;
                if (this.rdbRook.Checked) tipo_vizinhanca = 2;

                cps.DefinicaoVizinhos(ref shp_dados, tipo_vizinhanca,ref progressBar1);

                //Exporta os vizinhos
                for (int iP = 0; iP < shp_dados.Count; iP++)
                {
                    for (int iP2 = 0; iP2 < shp_dados[iP].NumeroVizinhos; iP2++)
                    {
                        //Cria uma nova linha
                        DataRow dLinha = dataSet1.Tables["tblVizinhanca"].NewRow();
                        dLinha[cmbID.SelectedItem.ToString() + "_1"] = shp_dados[iP].Nome;
                        dLinha[cmbID.SelectedItem.ToString() + "_2"] = shp_dados[iP2].Nome;
                        dataSet1.Tables["tblVizinhanca"].Rows.Add(dLinha);
                    }
                }
                if (tipo_vizinhanca == 1) shp_dados.TipoVizinhanca = "Queen";
                if (tipo_vizinhanca == 2) shp_dados.TipoVizinhanca = "Rook";


                shapeAlex = shp_dados;
            }

            if (chkCentroid.Checked == true)
            {
                progressBar1.Value = 0;
                progressBar1.Maximum = layMapa.DataSource.GetFeatureCount();
                progressBar1.Minimum = 0;
                Application.DoEvents();

                //Remove as variáveis caso existam.
                if (dataSet1.Tables["tblCentroid"].Columns.Contains(cmbID.SelectedItem.ToString()) == true) dataSet1.Tables["tblCentroid"].Columns.Remove(cmbID.SelectedItem.ToString());
                if (dataSet1.Tables["tblCentroid"].Columns.Contains("X") == true) dataSet1.Tables["tblCentroid"].Columns.Remove("X");
                if (dataSet1.Tables["tblCentroid"].Columns.Contains("Y") == true) dataSet1.Tables["tblCentroid"].Columns.Remove("Y");

                //Adiciona as variáveis
                dataSet1.Tables["tblCentroid"].Columns.Add("X",Type.GetType("System.Double"));
                dataSet1.Tables["tblCentroid"].Columns.Add("Y",Type.GetType("System.Double"));
                dataSet1.Tables["tblCentroid"].Columns.Add(cmbID.SelectedItem.ToString(),Type.GetType("System.String"));


                for (int iPoligono = 0; iPoligono < layMapa.DataSource.GetFeatureCount(); iPoligono++)
                {
                    //Cria uma nova linha
                    DataRow dLinha = dataSet1.Tables["tblCentroid"].NewRow();

                    //Captura o FeatureDataRow
                    FeatureDataRow feature0 = layMapa.DataSource.GetFeature((uint)iPoligono);

                    //Captura a geometria
                    SharpMap.Geometries.Geometry shape0 = feature0.Geometry;

                    //Geometria do poligono
                    Polygon poligono0 = shape0 as SharpMap.Geometries.Polygon;

                    if (poligono0 != null)
                    {
                        dLinha["X"] = poligono0.Centroid.X;
                        dLinha["Y"] = poligono0.Centroid.Y;
                        dLinha[cmbID.SelectedItem.ToString()] = feature0[cmbID.SelectedItem.ToString()].ToString();
                    }
                    else
                    {
                        BoundingBox bBoxCentro = shape0.GetBoundingBox();
                        double XCentro = bBoxCentro.Min.X + ((bBoxCentro.Max.X - bBoxCentro.Min.X) / 2);
                        double YCentro = bBoxCentro.Min.Y + ((bBoxCentro.Max.Y - bBoxCentro.Min.Y) / 2);
                        dLinha["X"] = XCentro;
                        dLinha["Y"] = YCentro;
                        dLinha[cmbID.SelectedItem.ToString()] = feature0[cmbID.SelectedItem.ToString()].ToString();
                    }

                    dataSet1.Tables["tblCentroid"].Rows.Add(dLinha);

                    //Incrementa a progress bar
                    progressBar1.Increment(1);
                    Application.DoEvents();
                }
            }

            if (chkBounding.Checked == true)
            {
                progressBar1.Value = 0;
                progressBar1.Maximum = layMapa.DataSource.GetFeatureCount();
                progressBar1.Minimum = 0;
                Application.DoEvents();

                //Remove as variáveis caso existam.
                if (dataSet1.Tables["tblBounding"].Columns.Contains(cmbID.SelectedItem.ToString()) == true) dataSet1.Tables["tblBounding"].Columns.Remove(cmbID.SelectedItem.ToString());
                if (dataSet1.Tables["tblBounding"].Columns.Contains("XMIN") == true) dataSet1.Tables["tblBounding"].Columns.Remove("XMIN");
                if (dataSet1.Tables["tblBounding"].Columns.Contains("YMIN") == true) dataSet1.Tables["tblBounding"].Columns.Remove("YMIN");
                if (dataSet1.Tables["tblBounding"].Columns.Contains("XMAX") == true) dataSet1.Tables["tblBounding"].Columns.Remove("XMAX");
                if (dataSet1.Tables["tblBounding"].Columns.Contains("YMAX") == true) dataSet1.Tables["tblBounding"].Columns.Remove("YMAX");

                //Adiciona as variáveis
                dataSet1.Tables["tblBounding"].Columns.Add("XMAX", Type.GetType("System.Double"));
                dataSet1.Tables["tblBounding"].Columns.Add("YMAX", Type.GetType("System.Double"));
                dataSet1.Tables["tblBounding"].Columns.Add("XMIN", Type.GetType("System.Double"));
                dataSet1.Tables["tblBounding"].Columns.Add("YMIN", Type.GetType("System.Double"));
                dataSet1.Tables["tblBounding"].Columns.Add(cmbID.SelectedItem.ToString(), Type.GetType("System.String"));


                for (int iPoligono = 0; iPoligono < layMapa.DataSource.GetFeatureCount(); iPoligono++)
                {
                    //Cria uma nova linha
                    DataRow dLinha = dataSet1.Tables["tblBounding"].NewRow();

                    //Captura o FeatureDataRow
                    FeatureDataRow feature0 = layMapa.DataSource.GetFeature((uint)iPoligono);

                    //Captura a geometria
                    SharpMap.Geometries.Geometry shape0 = feature0.Geometry;

                    //Captura a bounding box
                    BoundingBox bBoxCentro = shape0.GetBoundingBox();
                    dLinha["XMAX"] = bBoxCentro.Max.X;
                    dLinha["YMAX"] = bBoxCentro.Max.Y;
                    dLinha["XMIN"] = bBoxCentro.Min.X;
                    dLinha["YMIN"] = bBoxCentro.Min.Y;
                    dLinha[cmbID.SelectedItem.ToString()] = feature0[cmbID.SelectedItem.ToString()].ToString();

                    //Guarda a linha
                    dataSet1.Tables["tblBounding"].Rows.Add(dLinha);

                    //Incrementa a progress bar
                    progressBar1.Increment(1);
                    Application.DoEvents();
                }
            }

            if (chkCoordenadas.Checked == true)
            {
                //Inicializa a progress bar
                progressBar1.Value = 0;
                progressBar1.Maximum = layMapa.DataSource.GetFeatureCount();
                progressBar1.Minimum = 0;
                Application.DoEvents();

                //Remove as variáveis caso existam.
                if (dataSet1.Tables["tblCoordenadas"].Columns.Contains(cmbID.SelectedItem.ToString()) == true) dataSet1.Tables["tblCoordenadas"].Columns.Remove(cmbID.SelectedItem.ToString());
                if (dataSet1.Tables["tblCoordenadas"].Columns.Contains("X") == true) dataSet1.Tables["tblCoordenadas"].Columns.Remove("X");
                if (dataSet1.Tables["tblCoordenadas"].Columns.Contains("Y") == true) dataSet1.Tables["tblCoordenadas"].Columns.Remove("Y");


                //Adiciona as variáveis
                dataSet1.Tables["tblCoordenadas"].Columns.Add("X", Type.GetType("System.Double"));
                dataSet1.Tables["tblCoordenadas"].Columns.Add("Y", Type.GetType("System.Double"));
                dataSet1.Tables["tblCoordenadas"].Columns.Add(cmbID.SelectedItem.ToString(), Type.GetType("System.String"));

                //Inicializa a classe de funções espaciais
                clsMapa classeMapa = new clsMapa();

                for (int iPoligono = 0; iPoligono < layMapa.DataSource.GetFeatureCount(); iPoligono++)
                {
                    //Pontos das coordenadas
                    List<PointF> pontos = classeMapa.coordenadasPoligono(layMapa, strEnderecoShape, (uint)iPoligono);

                    for (int iPontos = 0; iPontos < pontos.Count; iPontos++)
                    {
                        //Cria uma nova linha
                        DataRow dLinha = dataSet1.Tables["tblCoordenadas"].NewRow();

                        //Guarda os pontos
                        dLinha["X"] = pontos[iPontos].X;
                        dLinha["Y"] = pontos[iPontos].Y;

                        //Captura o FeatureDataRow
                        FeatureDataRow feature0 = layMapa2.DataSource.GetFeature((uint)iPoligono);

                        //Captura a geometria
                        SharpMap.Geometries.Geometry shape0 = feature0.Geometry;

                        dLinha[cmbID.SelectedItem.ToString()] = feature0[cmbID.SelectedItem.ToString()].ToString();

                        //Guarda a linha
                        dataSet1.Tables["tblCoordenadas"].Rows.Add(dLinha);
                    }

                    //Incrementa a progress bar
                    progressBar1.Increment(1);
                    Application.DoEvents();
                }
            }
            
          
            Cursor.Current = Cursors.Default;
            Application.DoEvents();

            //Exporta as tabelas.
            DataSet dsTempVizinha = new DataSet();
            dsTempVizinha = dataSet1.Copy();
            dsTempVizinha.Tables.Remove("tblBounding");
            dsTempVizinha.Tables.Remove("tblCentroid");
            dsTempVizinha.Tables.Remove("tblCoordenadas");

            DataSet dsTempBounding = new DataSet();
            dsTempBounding = dataSet1.Copy();
            dsTempBounding.Tables.Remove("tblCentroid");
            dsTempBounding.Tables.Remove("tblVizinhanca");
            dsTempBounding.Tables.Remove("tblCoordenadas");

            DataSet dsTempCentroid = new DataSet();
            dsTempCentroid = dataSet1.Copy();
            dsTempCentroid.Tables.Remove("tblVizinhanca");
            dsTempCentroid.Tables.Remove("tblBounding");
            dsTempCentroid.Tables.Remove("tblCoordenadas");

            DataSet dsTempCoordenadas = new DataSet();
            dsTempCoordenadas = dataSet1.Copy();
            dsTempCoordenadas.Tables.Remove("tblVizinhanca");
            dsTempCoordenadas.Tables.Remove("tblBounding");
            dsTempCoordenadas.Tables.Remove("tblCentroid");

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "C:\\";
            saveFileDialog1.Filter = "Access (*.mdb)|*.mdb|Excel (*.xls)|*.xls|XML (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                string strFile = saveFileDialog1.FileName;
                string strNome = Path.GetFileName(strFile);
                string strPath = Path.GetDirectoryName(strFile);

                ExportData exporta = new ExportData();

                if (strExtensao == ".XLS")
                {
                    if (ckbEstruturaVizinhanca.Checked == true) exporta.exportToExcel(dsTempVizinha, strPath + "\\" + "Vizinhanca_" + strNome);
                    if (chkBounding.Checked == true) exporta.exportToExcel(dsTempBounding, strPath + "\\" + "Bounding_" + strNome);
                    if (chkCentroid.Checked == true) exporta.exportToExcel(dsTempCentroid, strPath + "\\" + "Centroid_" + strNome);
                    if (chkCoordenadas.Checked == true) exporta.exportToExcel(dsTempCoordenadas, strPath + "\\" + "Coordenadas_" + strNome);


                }
                else if (strExtensao == ".XML")
                {
                    if (ckbEstruturaVizinhanca.Checked == true) dsTempVizinha.WriteXml(strPath + "\\" + "Vizinhanca_" + strNome);
                    if (chkBounding.Checked == true) dsTempBounding.WriteXml(strPath + "\\" + "Bounding_" + strNome);
                    if (chkCentroid.Checked == true) dsTempCentroid.WriteXml(strPath + "\\" + "Centroid_" + strNome);
                    if (chkCoordenadas.Checked == true) dsTempCoordenadas.WriteXml(strPath + "\\" + "Coordenadas_" + strNome);
                }
                else if (strExtensao == ".MDB")
                {
                    //Cria o arquivo MDB
                    if (ckbEstruturaVizinhanca.Checked == true) exporta.exportaToAccess(dsTempVizinha, strFile, "Vizinhanca");
                    if (chkBounding.Checked == true) exporta.exportaToAccess(dsTempBounding, strFile, "Bounding");
                    if (chkCentroid.Checked == true) exporta.exportaToAccess(dsTempCentroid, strFile, "Centroid");
                    if (chkCoordenadas.Checked == true) exporta.exportaToAccess(dsTempCoordenadas, strFile, "Coordenadas");
                }

                Cursor.Current = Cursors.Default;
            } 

            //Fecha o mapa
            shapefile.Close();

            //OK
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            //Fecha o dialog
            this.Close();
        }
    }
}
