using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ADOX;
using SharpMap.Geometries;
using SharpMap.Data;
using SharpMap.Data.Providers;
using System.Collections;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;
using System.Drawing.Imaging;
using ZedGraph;


namespace IpeaGEO
{
    public partial class frmMapa : Form
    {
        public frmMapa()
        {
            InitializeComponent();
        }

        #region Métodos para a escolha do número de conglomerados 

        private double[] dblPseudoT;
        public double[] PseudoT
        {
            get
            {
                return dblPseudoT;
            }
            set
            {
                dblPseudoT = value;
            }
        }
        private double[] dblPseudoF;
        public double[] PseudoF
        {
            get
            {
                return dblPseudoF;
            }
            set
            {
                dblPseudoF = value;
            }
        }
        private double[] dblRSquare;
        public double[] RSquare
        {
            get
            {
                return dblRSquare;
            }
            set
            {
                dblRSquare = value;
            }
        }
        private double[] dblRSquarePartial;
        public double[] RSquarePartial
        {
            get
            {
                return dblRSquarePartial;
            }
            set
            {
                dblRSquarePartial = value;
            }
        }
        private double[] dblRSquareExpected;
        public double[] RSquareExpected
        {
            get
            {
                return dblRSquareExpected;
            }
            set
            {
                dblRSquareExpected = value;
            }
        }
        private double[] dblCCC;
        public double[] CCC
        {
            get
            {
                return dblCCC;
            }
            set
            {
                dblCCC = value;
            }
        }

        #endregion

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

        //Pasta com os dados
        public string strPastaDados;

        //Endereço do shapefile
        public string strEnderecoMapa;

        //Endereço da base
        public string strEnderecoBase;
        
        //Variaveis no mapa
        public string[] strVariaveisMapa;
        
        //Declara o formulario PAI.
        private MDIParent1 m_mdiparent;

        //Número de classes do mapa
        private double[] dblClasses;

        //Variaveis identificadoras
        private string strIDmapa;
        private string strIDbase;

        //Imagem
        //public System.Windows.Forms.ImageList imagesLarge1;

        public string appPath = Path.GetDirectoryName(Application.ExecutablePath);



        private void frmMapa_Load(object sender, EventArgs e)
        {
            //Parametros tooltip
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;

            //Cursor de espera
            Cursor.Current = Cursors.WaitCursor;

            clsMapa classeMapa = new clsMapa();
            classeMapa.loadingMapa(ref mapImage1, strEnderecoMapa, this.Name,ref shapeAlex);
            this.mapImage1.Map.ZoomToExtents();
            this.mapImage1.Refresh();

            //ImageList
            System.Windows.Forms.ImageList imagesLarge1 = new ImageList();            

            //Endereço da Imagem
            Image img1 = Image.FromFile(appPath + "\\Database1.png");
            Image img2 = Image.FromFile(appPath + "\\Database2.png");

            //Adicionando Imagens ao ListView
            imagesLarge1.Images.Add(img1);
            imagesLarge1.Images.Add(img2);
            imagesLarge1.ImageSize = new Size(45, 45);
        
            //Guarda as Imagens
            this.listView1.LargeImageList = imagesLarge1;

            //Guarda o item da Base Default
            this.listView1.Items.Add("IpeaGEO",1);
       
            //Cursor padrão
            Cursor.Current = Cursors.Default;

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (toolStripButton3.Checked == true)
            {
                this.tlInformacao.Checked = false;
                this.moveMapa.Checked = false;
                this.toolStripButton4.Checked = false;
                this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.ZoomIn;
            }
            else
            {
                this.tlInformacao.Checked = false;
                this.moveMapa.Checked = false;
                this.toolStripButton4.Checked = false;
                this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Bitmap (*.bmp)|*.bmp|All Files (*.*)|*.*";
            string FileName = "";
            saveFileDialog.FileName = this.Name;
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                FileName = saveFileDialog.FileName;


                //Coloca o fundo branco
                mapImage1.Map.BackColor = Color.White;

                //Gera o mapa
                System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                //Salva o mapa
                img.Save(FileName);

                //Dispose o mapa
                img.Dispose();
            }
        }

        private void adDados_Click(object sender, EventArgs e)
        {
            //Formulário Pai .sas7bdat
            m_mdiparent = (IpeaGEO.MDIParent1)this.MdiParent;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Excel (*.xls)|*.xls|Access (*.mdb)|*.mdb|SAS (*.sas7bdat)|*.sas7bdat|XML (*.xml)|*.xml";
            string FileName = "";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {

                FileName = openFileDialog.FileName;

                //Abre a conexão
                frmExporta frmConecta = new frmExporta();
                frmConecta.EnderecoBase = FileName;
                frmConecta.EnderecoShape = strEnderecoMapa;
                frmConecta.BaseDeDados = dsDados;

                //Passa a estrutura do shape
                frmConecta.EstruturaShape = shapeAlex;

                //Guarda o endereço da base
                strEnderecoBase = FileName;

                //Extensão
                string strExtensao = Path.GetExtension(FileName).ToUpper();

                //Guarda a extensão
                frmConecta.ExtensaoDoArquivo = strExtensao;

                frmConecta.ShowDialog();

                if (frmConecta.DialogResult == DialogResult.OK)
                {

                    Cursor.Current = Cursors.WaitCursor;

                    //Habilita a exportação
                    toolExportaDados.Enabled = true;
                    toolRefresh.Enabled = true;

                    //Guarda a base de dados conectada
                    dsDados = frmConecta.BaseDeDados;

                    //Guarda as variaveis que existem no mapa
                    strVariaveisMapa = frmConecta.VariaveiNoMapa;

                    //Guarda ID mapa
                    strIDmapa = frmConecta.IDmapa;

                    //Guarda ID Base
                    strIDbase = frmConecta.IDbase;

                    //Guarda a estrutura
                    shapeAlex = frmConecta.EstruturaShape;

                    //Fecha o Dialog
                    frmConecta.Close();

                    //Habilita os butões de análise
                    ToolspatialEstat.Enabled = true;
                    mapaTematico.Enabled = true;
                    tlInformacao.Enabled = true;
             
                    
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void moveMapa_Click(object sender, EventArgs e)
        {
            if (moveMapa.Checked == true)
            {
                this.tlInformacao.Checked = false;
                this.toolStripButton4.Checked = false;
                this.toolStripButton3.Checked = false;
                this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.Pan;
            }
            else
            {
                this.tlInformacao.Checked = false;
                this.toolStripButton4.Checked = false;
                this.toolStripButton3.Checked = false;
                this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (toolStripButton4.Checked == true)
            {
                this.tlInformacao.Checked = false;
                this.moveMapa.Checked = false;
                this.toolStripButton3.Checked = false;
                this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.ZoomOut;
            }
            else
            {
                this.tlInformacao.Checked = false;
                this.moveMapa.Checked = false;
                this.toolStripButton3.Checked = false;
                this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;
            }
           
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            this.mapImage1.Map.ZoomToExtents();
            this.mapImage1.Refresh();
        }

        private void frmMapa_MaximizedBoundsChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
        }

        private void frmMapa_Resize(object sender, EventArgs e)
        {
            this.mapImage1.Map.ZoomToExtents();
            this.mapImage1.Refresh();
        }

       
        
        private void mapaTematico_Click(object sender, EventArgs e)
        {

            //Formulário Pai
            m_mdiparent = (IpeaGEO.MDIParent1)this.MdiParent;

            //Abre a conexão
            frmMapaTematico frmMapa = new frmMapaTematico();

            //Aplica os Idenfificadores
            frmMapa.IdentificadorDados = strIDbase;
            frmMapa.IdentificadorMapa = strIDmapa;

            //Aplica o DataTable
            DataTable dTable = dsDados.Tables[0];
            frmMapa.DataTableDados = dTable;

            //Abre o Dialog
            frmMapa.ShowDialog();
            
            if (frmMapa.DialogResult == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                //Captura o mapa
                SharpMap.Map mMapa = mapImage1.Map;

                //Captura o layer
                SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[0];

                //Cria o vetor temático
                int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                //Guarda as classes
                 dblClasses = frmMapa.ClasseDoMapa;

                //Guarda o vetor temático
                 iVetor = frmMapa.vetorPoligonos;

                //Inicializa as cores
                 Brush[] cores = frmMapa.CoresParaMapa;

                //Guarda os vetores
                ArrayList vetorID = new ArrayList();
                int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                
                for(int i=0;i<dsDados.Tables[0].Rows.Count;i++)
                {
                    vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                    vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                    
                }

                //Pinta o mapa
                mTheme meuTema = new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice);
                layMapa.Theme = new SharpMap.Rendering.Thematics.CustomTheme(meuTema.GetStyle);
                if(frmMapa.GuardaClassificacao==true) 
                {
                    if(dsDados.Tables[0].Columns.Contains("MapaTematico")==false) dsDados.Tables[0].Columns.Add("MapaTematico");
                    for(int i=0;i<iVetor.Length;i++)
                    {
                        dsDados.Tables[0].Rows[i]["MapaTematico"] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]];
                    }
                }

                //Refresh
                mMapa.ZoomToExtents();

                //Refresh o mapa
                mapImage1.Refresh();

                //Gera relatório
                if (frmMapa.GeraRelatorio == true)
                {
                    //Coloca o fundo branco
                    mapImage1.Map.BackColor = Color.White;

                    //Gera o mapa
                    System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                    //Salva o mapa
                    img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);

                    clsReport classeReport = new clsReport();
                    string html = classeReport.MapaTematicoRelatorio(strEnderecoBase,strEnderecoMapa, Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count , frmMapa.Metodologia, frmMapa.ClasseDoMapa, frmMapa.CoresRGB, frmMapa.Variavel);

                    //Abre o relatório
                    frmRelatorio frmRelatorio = new frmRelatorio();
                    frmRelatorio.MdiParent = this.MdiParent;
                    frmRelatorio.CodigoHTML = html;
                    frmRelatorio.Text = "Relatório " + this.Text + ": " + frmMapa.Variavel;

                    string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count + 1, 2];
                    strClusterMapa[0, 0] = strIDbase;
                    strClusterMapa[0, 1] = "Classe";

                    for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                    {
                        string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                        string stCl = dsDados.Tables[0].Rows[i]["MapaTematico"].ToString();
                        strClusterMapa[i + 1, 0] = stId;
                        strClusterMapa[i + 1, 1] = stCl;
                    }

                    frmRelatorio.VariaveisMapa = strClusterMapa;

                    frmRelatorio.Show();
                }

                Cursor.Current = Cursors.Default;
            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            this.mapImage1.Map.Layers.Clear();
            clsMapa classeMapa = new clsMapa();
            classeMapa.loadingMapa(ref mapImage1, strEnderecoMapa, this.Name,ref shapeAlex);
            this.mapImage1.Refresh();
        }

        private void toolExportaDados_Click(object sender, EventArgs e)
        {

            DataSet dsTemp = dsDados.Copy();
            //dsTemp.Tables[0].Columns.Remove("Mapa"+strIDmapa);
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "C:\\";
            saveFileDialog1.Filter = "Excel (*.xls)|*.xls|Access (*.mdb)|*.mdb|XML (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                string strFile = saveFileDialog1.FileName;

                ExportData exporta = new ExportData();

                if (strExtensao == ".XLS")
                {
                    exporta.exportToExcel(dsTemp, strFile);
                }
                else if (strExtensao == ".XML")
                {
                    dsTemp.WriteXml(strFile);
                }
                else if (strExtensao == ".MDB")
                {
                    //Cria o arquivo MDB
                    exporta.exportaToAccess(dsTemp, strFile, this.Name);
                }
                Cursor.Current = Cursors.Default;
            } 
        }

         private void bernoulliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Abre a conexão
            frmScan frmScanEstat = new frmScan();
            
            //Guarda a base de dados
            frmScanEstat.DataTableDados = dsDados.Tables[0];

            //Guarda o ID do mapa
            frmScanEstat.IdentificadorMapa = strIDmapa;

            //Guarda o ID da base
            frmScanEstat.IdentificadorBase = strIDbase;
            
            //Guarda o endereço do mapa
            frmScanEstat.EnderecoMapa = strEnderecoMapa;

            //Define a distribuição
            frmScanEstat.Distribuicao = "BERNOULLI";

            //Abre o Dialog
            frmScanEstat.ShowDialog();

            if (frmScanEstat.DialogResult == DialogResult.OK)
            {

                Cursor.Current = Cursors.WaitCursor;

                //Captura o mapa
                SharpMap.Map mMapa = mapImage1.Map;

                //Captura o layer
                SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[0];

                //Cria o vetor temático
                int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                //Guarda o vetor temático
                iVetor = frmScanEstat.vetorPoligonos;

                //Inicializa as cores
                Brush[] cores = frmScanEstat.CoresParaMapa;

                //Adiciona o pvalor na tabela
                double[] pValor = frmScanEstat.vetorPvalor;

                //Retira a variável caso já exista
                string nome = "ScanB_" + frmScanEstat.VariavelEvento+"_"+ frmScanEstat.VariavelBase;
                if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);
                string nome2 = "ScanB1_" + frmScanEstat.VariavelEvento + "_" + frmScanEstat.VariavelBase;
                if (dsDados.Tables[0].Columns.Contains(nome2) == true) dsDados.Tables[0].Columns.Remove(nome2);

                //Adiciona a variavel no mapa
                dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));
                dsDados.Tables[0].Columns.Add(nome2, Type.GetType("System.String"));

                
                //Guarda os vetores
                ArrayList vetorID = new ArrayList();
                int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                {
                    vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                    vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                    if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0) dsDados.Tables[0].Rows[i][nome] = pValor[iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] - 1].ToString();
                    if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0) dsDados.Tables[0].Rows[i][nome2] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                }

                //Pinta o mapa
                SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetor, cores,strIDmapa,vetorID, vetorIndice)).GetStyle);
                layMapa.Theme = iTheme;

                //Refresh
                mMapa.ZoomToExtents();

                //Refresh o mapa
                mapImage1.Refresh();

                //Gera relatório
                if (frmScanEstat.GeraRelatorio == true)
                {
                    //Coloca o fundo branco
                    mapImage1.Map.BackColor = Color.White;

                    //Gera o mapa
                    System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                    //Salva o mapa
                    img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);


                    double[] vetorMontecarlo = frmScanEstat.MonteCarlo;
                    Array.Sort(vetorMontecarlo);
                    //Cria os intervalos
                    int iRazao = vetorMontecarlo.Length / 10;
                    double iIntervalos = (vetorMontecarlo[vetorMontecarlo.Length - 1] - vetorMontecarlo[0]) / iRazao;
                    double[] dblAbcissa = new double[iRazao];
                    double iDummy = iIntervalos;
                    for (int i = 0; i < iRazao; i++)
                    {
                        dblAbcissa[i] = iDummy;
                        iDummy += iIntervalos;
                    }

                    //Conta a frequencia
                    double[] dblOrdenada = new double[iRazao];
                    int iConta = 0;
                    for (int j = 0; j < vetorMontecarlo.Length; j++)
                    {
                        if (vetorMontecarlo[j] < dblAbcissa[iConta])
                        {
                            dblOrdenada[iConta]++;
                        }
                        else
                        {
                            dblOrdenada[iConta] /= vetorMontecarlo.Length;
                            if (iConta < dblOrdenada.Length - 1) iConta++;
                            dblOrdenada[iConta]++;
                        }

                    }
                    dblOrdenada[dblOrdenada.Length - 1] /= vetorMontecarlo.Length;

                    //TODO:Programar o HISTOGRAMA

                    //Gera o histograma
                    GraphPane myPane = new GraphPane(new RectangleF(0, 0, 824, 631), "Densidade : Razão de verossimilhança", "Razão de verossimilhança", "Frequência");

                    // Add a red curve with circle symbols
                    LineItem curve = myPane.AddCurve("LLR", dblAbcissa, dblOrdenada, Color.White, SymbolType.None);

                    curve.Line.Width = 1.5F;
                    curve.Line.IsSmooth = true;
                    curve.Line.SmoothTension = 0.8F;

                    // Fill the area under the curve
                    curve.Line.Fill = new Fill(Color.Blue);

                    // Fill the symbols with white to make them opaque
                    curve.Symbol.Fill = new Fill(Color.Blue);

                    // Set the curve type to forward steps
                    //curve.Line.StepType = StepType.ForwardStep;

                    // Fill the axis background with a color gradient
                    myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 45.0F);

                    myPane.AxisChange();

                    //Salva o histograma
                    myPane.GetImage().Save(Application.StartupPath + "\\Histograma.jpeg", ImageFormat.Jpeg);


                    //Gera o relatório
                    clsReport classeReport = new clsReport();
                    string html = classeReport.EstatisticaScanRelatorio(strEnderecoBase,strEnderecoMapa, Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count, frmScanEstat.Distribuicao, frmScanEstat.vetorPvalor,
                    frmScanEstat.CoresRGB, frmScanEstat.VariavelBase, frmScanEstat.VariavelEvento, frmScanEstat.NumeroDeSimulacoes, frmScanEstat.NumeroDePontosGrid, frmScanEstat.RaioMaximo, frmScanEstat.RaioMinimo, frmScanEstat.ProporcaoMaxima, Application.StartupPath + "\\Histograma.jpeg");

                    //Abre o relatório
                    frmRelatorio frmRelatorio = new frmRelatorio();
                    frmRelatorio.MdiParent = this.MdiParent;
                    frmRelatorio.CodigoHTML = html;
                    frmRelatorio.Text = "Relatório Estatística Scan: " + this.Text;

                    string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count + 1, 2];
                    strClusterMapa[0, 0] = strIDbase;
                    strClusterMapa[0, 1] = "Conglomerado";

                    for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                    {
                        string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                        string stCl = dsDados.Tables[0].Rows[i][nome2].ToString();
                        strClusterMapa[i + 1, 0] = stId;
                        strClusterMapa[i + 1, 1] = stCl;
                    }

                    frmRelatorio.HistogramaX = dblAbcissa;
                    frmRelatorio.HistogramaY = dblOrdenada;
                    frmRelatorio.VariaveisMapa = strClusterMapa;

                    frmRelatorio.Show();
                }



                Cursor.Current = Cursors.Default;
            }

        }

         private void geoReferenciamento_Click(object sender, EventArgs e)
         {
             frmGeoreferenciamento frmGeo = new frmGeoreferenciamento();
             frmGeo.EnderecoShape = strEnderecoMapa;
             frmGeo.EstruturaShape = shapeAlex;
             frmGeo.ShowDialog();
             if (frmGeo.DialogResult == DialogResult.OK)
             {
                 shapeAlex = frmGeo.EstruturaShape;
             }
         }

         private void poissonToolStripMenuItem_Click(object sender, EventArgs e)
         {
             //Abre a conexão
             frmScan frmScanEstat = new frmScan();

             //Guarda a base de dados
             frmScanEstat.DataTableDados = dsDados.Tables[0];

             //Guarda o ID do mapa
             frmScanEstat.IdentificadorMapa = strIDmapa;

             //Guarda o ID da base
             frmScanEstat.IdentificadorBase = strIDbase;

             //Define a distribuição
             frmScanEstat.Distribuicao = "POISSON";

             //Guarda o endereço do mapa
             frmScanEstat.EnderecoMapa = strEnderecoMapa;

             //Abre o Dialog
             frmScanEstat.ShowDialog();

             if (frmScanEstat.DialogResult == DialogResult.OK)
             {

                 Cursor.Current = Cursors.WaitCursor;

                 //Captura o mapa
                 SharpMap.Map mMapa = mapImage1.Map;

                 //Captura o layer
                 SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[0];

                 //Cria o vetor temático
                 int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                 //Guarda o vetor temático
                 iVetor = frmScanEstat.vetorPoligonos;

                 //Inicializa as cores
                 Brush[] cores = frmScanEstat.CoresParaMapa;

                 //Adiciona o pvalor na tabela
                 double[] pValor = frmScanEstat.vetorPvalor;

                 //Retira a variável caso já exista
                 string nome = "ScanP_" + frmScanEstat.VariavelEvento + "_" + frmScanEstat.VariavelBase;
                 if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);
                 string nome2 = "ScanP1_" + frmScanEstat.VariavelEvento + "_" + frmScanEstat.VariavelBase;
                 if (dsDados.Tables[0].Columns.Contains(nome2) == true) dsDados.Tables[0].Columns.Remove(nome2);

                 //Adiciona a variavel no mapa
                 dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));
                 dsDados.Tables[0].Columns.Add(nome2, Type.GetType("System.String"));


                 //Guarda os vetores
                 ArrayList vetorID = new ArrayList();
                 int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                 for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                 {
                     vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                     vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                     if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] > 0) dsDados.Tables[0].Rows[i][nome] = pValor[iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] - 1].ToString();
                     if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] > 0) dsDados.Tables[0].Rows[i][nome2] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                 }

                 //Pinta o mapa
                 SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                 layMapa.Theme = iTheme;

                 //Refresh
                 mMapa.ZoomToExtents();

                 //Refresh o mapa
                 mapImage1.Refresh();

                 //Gera relatório
                 if (frmScanEstat.GeraRelatorio == true)
                 {
                     //Coloca o fundo branco
                     mapImage1.Map.BackColor = Color.White;

                     //Gera o mapa
                     System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                     //Salva o mapa
                     img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);

                     
                     double[] vetorMontecarlo = frmScanEstat.MonteCarlo;
                     Array.Sort(vetorMontecarlo);
                     //Cria os intervalos
                     int iRazao = vetorMontecarlo.Length/10;
                     double iIntervalos = (vetorMontecarlo[vetorMontecarlo.Length - 1] - vetorMontecarlo[0]) / iRazao;
                     double[] dblAbcissa = new double[iRazao];
                     double iDummy=iIntervalos;
                     for (int i = 0; i < iRazao; i++)
                     {
                         dblAbcissa[i] = iDummy;
                         iDummy += iIntervalos;
                     }

                     //Conta a frequencia
                     double[] dblOrdenada = new double[iRazao];
                     int iConta = 0; 
                     for (int j = 0; j < vetorMontecarlo.Length; j++)
                     {
                         if (vetorMontecarlo[j] < dblAbcissa[iConta])
                         {
                             dblOrdenada[iConta]++;
                         }
                         else
                         {
                             dblOrdenada[iConta] /= vetorMontecarlo.Length;
                             if (iConta < dblOrdenada.Length - 1) iConta++;
                             dblOrdenada[iConta]++;
                         }
                         
                     }
                     dblOrdenada[dblOrdenada.Length-1] /= vetorMontecarlo.Length;

                     //TODO:Programar o HISTOGRAMA

                     //Gera o histograma
                     GraphPane myPane = new GraphPane(new RectangleF(0, 0, 824, 631), "Densidade : Razão de verossimilhança", "Razão de verossimilhança", "Frequência");

                     // Add a red curve with circle symbols
                     LineItem curve = myPane.AddCurve("LLR", dblAbcissa, dblOrdenada, Color.White, SymbolType.None);

                     curve.Line.Width = 1.5F;
                     curve.Line.IsSmooth = true;
                     curve.Line.SmoothTension = 0.8F;

                     // Fill the area under the curve
                     curve.Line.Fill = new Fill(Color.Blue);

                     // Fill the symbols with white to make them opaque
                     curve.Symbol.Fill = new Fill(Color.Blue);

                     // Set the curve type to forward steps
                     //curve.Line.StepType = StepType.ForwardStep;

                     // Fill the axis background with a color gradient
                     myPane.Chart.Fill = new Fill(Color.White,Color.FromArgb(255, 255, 166), 45.0F);

                     myPane.AxisChange();

                     //Salva o histograma
                     myPane.GetImage().Save(Application.StartupPath + "\\Histograma.jpeg", ImageFormat.Jpeg);

                     //Gera o relatório
                     clsReport classeReport = new clsReport();
                     string html = classeReport.EstatisticaScanRelatorio(strEnderecoBase,strEnderecoMapa, Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count, frmScanEstat.Distribuicao, frmScanEstat.vetorPvalor,
                     frmScanEstat.CoresRGB, frmScanEstat.VariavelBase, frmScanEstat.VariavelEvento, frmScanEstat.NumeroDeSimulacoes, frmScanEstat.NumeroDePontosGrid, frmScanEstat.RaioMaximo, frmScanEstat.RaioMinimo, frmScanEstat.ProporcaoMaxima, Application.StartupPath + "\\Histograma.jpeg");

                     //Abre o relatório
                     frmRelatorio frmRelatorio = new frmRelatorio();
                     frmRelatorio.MdiParent = this.MdiParent;
                     frmRelatorio.CodigoHTML = html;
                     frmRelatorio.Text = "Relatório Estatística Scan: " + this.Text;

                     string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count + 1, 2];
                     strClusterMapa[0, 0] = strIDbase;
                     strClusterMapa[0, 1] = "Conglomerado";

                     for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                     {
                         string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                         string stCl = dsDados.Tables[0].Rows[i][nome2].ToString();
                         strClusterMapa[i + 1, 0] = stId;
                         strClusterMapa[i + 1, 1] = stCl;
                     }

                     frmRelatorio.HistogramaX = dblAbcissa;
                     frmRelatorio.HistogramaY = dblOrdenada;
                     frmRelatorio.VariaveisMapa = strClusterMapa;

                     frmRelatorio.Show();
                 }


                 Cursor.Current = Cursors.Default;


             }
         }


         private void hierarquicoToolStripMenuItem_Click(object sender, EventArgs e)
         {
             //Abre a conexão
             frmCluster frmCluster = new frmCluster();

             //Guarda a base de dados
             frmCluster.DataTableDados = dsDados.Tables[0];

             //Guarda o ID do mapa
             frmCluster.IdentificadorMapa = strIDmapa;

             //Guarda o ID da base
             frmCluster.IdentificadorDados = strIDbase;

             //Guarda o endereço do mapa
             frmCluster.EnderecoMapa = strEnderecoMapa;

             //Guarda a estrutura do shape
             frmCluster.EstruturaShape = shapeAlex;

             //Define como conglomerado espacial
             frmCluster.IsSpatialCluster = true;

             //Habilita o Label "Vizinhança"
             frmCluster.label4.Visible = true;

             //Modifica o tamanho do GroupBox
             //frmCluster.groupBox1.Size = new Size(240, 256);

             //Guarda as variáveis
             string[] strVariaveis = new string[dsDados.Tables[0].Columns.Count];
             for (int i = 0; i < strVariaveis.Length; i++) strVariaveis[i] = dsDados.Tables[0].Columns[i].ColumnName;
             frmCluster.Variaveis = strVariaveis;

             //Abre o Dialog
             frmCluster.ShowDialog();

             if (frmCluster.DialogResult == DialogResult.OK)
             {

                 Cursor.Current = Cursors.WaitCursor;

                 //Captura o mapa
                 SharpMap.Map mMapa = mapImage1.Map;

                 //Captura o layer
                 SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[0];

                 //Cria o vetor temático
                 int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                 //Guarda o vetor temático
                 iVetor = frmCluster.vetorPoligonos;

                 //Inicializa as cores
                 Brush[] cores = frmCluster.CoresParaMapa;

                 //Retira a variável caso já exista
                 string nome = "ScanH_" + this.Name;
                 if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);
                 
                 //Adiciona a variavel no mapa
                 dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                 //Guarda os vetores
                 ArrayList vetorID = new ArrayList();
                 int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                 for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                 {
                     vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                     vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                     if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0) dsDados.Tables[0].Rows[i][nome] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                 }

                 //Pinta o mapa
                 SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                 layMapa.Theme = iTheme;

                 //Refresh
                 mMapa.ZoomToExtents();

                 //Refresh o mapa
                 mapImage1.Refresh();

                 #region Gera o Relatório

                 if (frmCluster.GeraRelatorio == true)
                 {
                     //Métodos para a escolha do número de conglomerados
                     dblCCC = frmCluster.CCC;
                     dblPseudoF = frmCluster.PseudoF;
                     dblPseudoT = frmCluster.PseudoT;
                     dblRSquare = frmCluster.RSquare;
                     dblRSquareExpected = frmCluster.RSquareExpected;
                     dblRSquarePartial = frmCluster.RSquarePartial;

                     #region Gera Gráficos

                     // Get a reference to the GraphPane
                     GraphPane myPane = new GraphPane(new RectangleF(0, 0, 824, 631), "Escolha do tamanho ótimo do número de conglomerados.", "Número de conglomerados", "Pseudo T");

                     // Set the titles and axis labels
                     myPane.Title.Text = "Escolha do tamanho ótimo do número de conglomerados.";
                     myPane.XAxis.Title.Text = "Número de conglomerados";
                     myPane.YAxis.Title.Text = "Pseudo T";
                     myPane.Y2Axis.Title.Text = "R-Square";

                     //List of points
                     PointPairList ptList = new PointPairList();
                     PointPairList rsList = new PointPairList();
                     PointPairList pfList = new PointPairList();
                     PointPairList cccList = new PointPairList();

                     // Fabricate some data values
                     int iObs = Convert.ToInt32(frmCluster.NumeroDeConglomerados) * 2;
                     int iTotal = dblRSquare.Length - 1;
                     for (int i = 0; i < iObs; i++)
                     {
                         rsList.Add(i + 2, dblRSquare[iTotal]);
                         ptList.Add(i + 2, dblPseudoT[iTotal]);
                         cccList.Add(i + 2, dblCCC[iTotal]);
                         pfList.Add(i + 2, dblPseudoF[iTotal]);
                         iTotal--;
                     }

                     // Generate a red curve with diamond symbols, and "PseudoT" in the legend
                     LineItem myCurve = myPane.AddCurve("Pseudo T", ptList, Color.Red, SymbolType.Diamond);
                     // Fill the symbols with white
                     myCurve.Symbol.Fill = new Fill(Color.White);

                     // Generate a blue curve with circle symbols, and "RSquare" in the legend
                     myCurve = myPane.AddCurve("R-Square", rsList, Color.Blue, SymbolType.Circle);
                     // Fill the symbols with white
                     myCurve.Symbol.Fill = new Fill(Color.White);
                     // Associate this curve with the Y2 axis
                     myCurve.IsY2Axis = true;

                     // Generate a green curve with square symbols, and "PseudoF" in the legend
                     myCurve = myPane.AddCurve("Pseudo F", pfList, Color.Green, SymbolType.Square);
                     // Fill the symbols with white
                     myCurve.Symbol.Fill = new Fill(Color.White);
                     // Associate this curve with the second Y axis
                     myCurve.YAxisIndex = 1;

                     // Generate a Black curve with triangle symbols, and "CCC" in the legend
                     myCurve = myPane.AddCurve("CCC", cccList, Color.Black, SymbolType.Triangle);
                     // Fill the symbols with white
                     myCurve.Symbol.Fill = new Fill(Color.White);
                     // Associate this curve with the Y2 axis
                     myCurve.IsY2Axis = true;
                     // Associate this curve with the second Y2 axis
                     myCurve.YAxisIndex = 1;

                     // Show the x axis grid
                     myPane.XAxis.MajorGrid.IsVisible = true;

                     // Make the Y axis scale red
                     myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
                     myPane.YAxis.Title.FontSpec.FontColor = Color.Red;
                     // turn off the opposite tics so the Y tics don't show up on the Y2 axis
                     myPane.YAxis.MajorTic.IsOpposite = false;
                     myPane.YAxis.MinorTic.IsOpposite = false;
                     // Don't display the Y zero line
                     myPane.YAxis.MajorGrid.IsZeroLine = false;

                     // Enable the Y2 axis display
                     myPane.Y2Axis.IsVisible = true;
                     // Make the Y2 axis scale blue
                     myPane.Y2Axis.Scale.FontSpec.FontColor = Color.Blue;
                     myPane.Y2Axis.Title.FontSpec.FontColor = Color.Blue;
                     // turn off the opposite tics so the Y2 tics don't show up on the Y axis
                     myPane.Y2Axis.MajorTic.IsOpposite = false;
                     myPane.Y2Axis.MinorTic.IsOpposite = false;
                     // Display the Y2 axis grid lines
                     myPane.Y2Axis.MajorGrid.IsVisible = true;
                     // Align the Y2 axis labels so they are flush to the axis
                     myPane.Y2Axis.Scale.Align = AlignP.Inside;

                     // Create a second Y Axis, green
                     YAxis yAxis3 = new YAxis("Pseudo F");
                     myPane.YAxisList.Add(yAxis3);
                     yAxis3.Scale.FontSpec.FontColor = Color.Green;
                     yAxis3.Title.FontSpec.FontColor = Color.Green;
                     yAxis3.Color = Color.Green;
                     // turn off the opposite tics so the Y2 tics don't show up on the Y axis
                     yAxis3.MajorTic.IsInside = false;
                     yAxis3.MinorTic.IsInside = false;
                     yAxis3.MajorTic.IsOpposite = false;
                     yAxis3.MinorTic.IsOpposite = false;
                     // Align the Y2 axis labels so they are flush to the axis
                     yAxis3.Scale.Align = AlignP.Inside;

                     Y2Axis yAxis4 = new Y2Axis("CCC");
                     yAxis4.IsVisible = true;
                     myPane.Y2AxisList.Add(yAxis4);
                     // turn off the opposite tics so the Y2 tics don't show up on the Y axis
                     yAxis4.MajorTic.IsInside = false;
                     yAxis4.MinorTic.IsInside = false;
                     yAxis4.MajorTic.IsOpposite = false;
                     yAxis4.MinorTic.IsOpposite = false;
                     // Align the Y2 axis labels so they are flush to the axis
                     yAxis4.Scale.Align = AlignP.Inside;

                     // Fill the axis background with a gradient
                     myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);

                     myPane.AxisChange();

                     myPane.Border.IsVisible = false;

                     //Salva o histograma
                     myPane.GetImage().Save(Application.StartupPath + "\\NumeroConglomerados.jpeg", ImageFormat.Jpeg);


                     #endregion

                     //Coloca o fundo branco
                     mapImage1.Map.BackColor = Color.White;

                     //Gera o mapa
                     System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                     //Salva o mapa
                     img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);

                     clsReport classeReport = new clsReport();
                     string html = classeReport.AnaliseDeConglomeradosEspacial(strEnderecoBase,strEnderecoMapa, Application.StartupPath + "\\Mapa.jpeg", frmCluster.NumeroDeConglomerados, frmCluster.FatorMinkowsky, shapeAlex.Count, frmCluster.Metodo, frmCluster.Distancia,frmCluster.strEML , frmCluster.IsSpatialCluster, frmCluster.VariaveisSelecionadas, frmCluster.CoresRGB, shapeAlex.TipoVizinhanca, Application.StartupPath + "\\NumeroConglomerados.jpeg");

                     //Abre o relatório
                     frmRelatorio frmRelatorio = new frmRelatorio();
                     frmRelatorio.MdiParent = this.MdiParent;
                     frmRelatorio.CodigoHTML = html;
                     frmRelatorio.Text = "Relatório " + this.Text + ": Análise de conglomerados espaciais";

                     //Métodos para a escolha do número de conglomerados
                     frmRelatorio.CCC = dblCCC;
                     frmRelatorio.PseudoF = dblPseudoF;
                     frmRelatorio.PseudoT = dblPseudoT;
                     frmRelatorio.RSquare = dblRSquare;
                     frmRelatorio.RSquareExpected = dblRSquareExpected;
                     frmRelatorio.RSquarePartial = dblRSquarePartial;

                     string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count+1, 2];
                     strClusterMapa[0,0]=strIDbase;
                     strClusterMapa[0,1]="Conglomerado";

                     for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                     {
                         string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                         string stCl = dsDados.Tables[0].Rows[i][nome].ToString();
                         strClusterMapa[i + 1, 0] = stId;
                         strClusterMapa[i + 1, 1] = stCl;
                     }

                     frmRelatorio.VariaveisMapa = strClusterMapa;

                     frmRelatorio.Show();
                 }

                 #endregion

                 Cursor.Current = Cursors.Default;
             }
              
         }


         private void mapImage1_MouseDown(SharpMap.Geometries.Point WorldPos, MouseEventArgs ImagePos)
         {
             //Se o usuário habilitar a pesquisa
             if (tlInformacao.Checked == true)
             {
                 //Desabilita qualquer função de ZOOM ou PAN
                 this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;

                 //Passo 1: Obtém o ponto clicado
                 double x = WorldPos.X;
                 double y = WorldPos.Y;

                 //Passo 2: Obtém o polígono escolhido
                 clsMapa ClasseMapa = new clsMapa();
                 int iPoligono = -1;
                 int iPoligonoBase = -1;
                     
                 iPoligono = ClasseMapa.identificaPontoPoligono(strEnderecoMapa, x, y);

                 //Captura os dados
                 DataRow dRow = dsDados.Tables[0].Rows[0];
                 for(int i=0;i<dsDados.Tables[0].Rows.Count;i++)
                 {
                     if (dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa].ToString() == iPoligono.ToString())
                     {
                         dRow = dsDados.Tables[0].Rows[i];
                         iPoligonoBase = i;
                         break;
                     }
                 }
                 string strInfor="";

                 if (-1 < iPoligono && iPoligono < shapeAlex.Count)
                 {
                     for(int i=0;i<dRow.Table.Columns.Count;i++)
                     {
                         strInfor += dRow.Table.Columns[i] + ": " + dRow.Table.Rows[iPoligonoBase][i].ToString() + "\n";
                     }

                     toolTip1.Show(strInfor, this, ImagePos.X, ImagePos.X, 5000);
                     
                     
                 }
             }

             
         }

         private void tlInformacao_Click(object sender, EventArgs e)
         {
 
         }

         private void mapImage1_Click(object sender, EventArgs e)
         {

         }

         private void análiseDeConglomeradosToolStripMenuItem_Click(object sender, EventArgs e)
         {
             //Abre a conexão
             frmCluster frmCluster = new frmCluster();

             //Guarda a base de dados
             frmCluster.DataTableDados = dsDados.Tables[0];

             //Guarda o ID do mapa
             frmCluster.IdentificadorMapa = strIDmapa;

             //Guarda o ID da base
             frmCluster.IdentificadorDados = strIDbase;

             //Guarda o endereço do mapa
             frmCluster.EnderecoMapa = strEnderecoMapa;

             //Define como Conglomerado não espacial
             frmCluster.IsSpatialCluster = false;

             frmCluster.Text = "Análise de conglomerados";

             frmCluster.EstruturaShape = shapeAlex;

             //Guarda as variáveis
             string[] strVariaveis = new string[dsDados.Tables[0].Columns.Count];
             for (int i = 0; i < strVariaveis.Length; i++) strVariaveis[i] = dsDados.Tables[0].Columns[i].ColumnName;
             frmCluster.Variaveis = strVariaveis;

             //Abre o Dialog
             frmCluster.ShowDialog();

             if (frmCluster.DialogResult == DialogResult.OK)
             {

                 Cursor.Current = Cursors.WaitCursor;

                 //Captura o mapa
                 SharpMap.Map mMapa = mapImage1.Map;

                 //Captura o layer
                 SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[0];

                 //Cria o vetor temático
                 int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                 //Guarda o vetor temático
                 iVetor = frmCluster.vetorPoligonos;

                 //Inicializa as cores
                 Brush[] cores = frmCluster.CoresParaMapa;

                 //Retira a variável caso já exista
                 string nome = "ClusterH_" + this.Name;
                 if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);

                 //Adiciona a variavel no mapa
                 dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                 //Guarda os vetores
                 ArrayList vetorID = new ArrayList();
                 int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                 for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                 {
                     vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                     vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                     if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] > 0) dsDados.Tables[0].Rows[i][nome] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                 }

                 //Pinta o mapa
                 SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                 layMapa.Theme = iTheme;

                 //Refresh
                 mMapa.ZoomToExtents();

                 //Refresh o mapa
                 mapImage1.Refresh();

                 #region Gera o Relatório 

                 //Coloca o fundo branco
                 mapImage1.Map.BackColor = Color.White;

                 //Gera o mapa
                 System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                 //Salva o mapa
                 img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);

                 clsReport classeReport = new clsReport();
                 string html = classeReport.AnaliseDeConglomerados(strEnderecoBase,strEnderecoMapa, Application.StartupPath + "\\Mapa.jpeg", frmCluster.NumeroDeConglomerados, frmCluster.FatorMinkowsky, shapeAlex.Count, frmCluster.Metodo, frmCluster.Distancia, frmCluster.IsSpatialCluster, frmCluster.VariaveisSelecionadas,frmCluster.VetorDePesos ,frmCluster.CoresRGB);

                 //Abre o relatório
                 frmRelatorio frmRelatorio = new frmRelatorio();
                 frmRelatorio.MdiParent = this.MdiParent;
                 frmRelatorio.CodigoHTML = html;
                 frmRelatorio.Text = "Relatório " + this.Text + ": Análise de conglomerados";
                 frmRelatorio.Show();


                 #endregion

                 Cursor.Current = Cursors.Default;
             }
              
         }

         private void entropiaToolStripMenuItem_Click(object sender, EventArgs e)
         {
             if (shapeAlex.TipoVizinhanca == "")
             {
                 //Cria a vizinhnaça
                 clsIpeaShape cps = new clsIpeaShape();
                 int tipo_vizinhanca = 1;

                 //"Queen" tipo_vizinhanca = 1;
                 //"Rook " tipo_vizinhanca = 2;

                 if (tipo_vizinhanca == 1) shapeAlex.TipoVizinhanca = "Queen";
                 if (tipo_vizinhanca == 2) shapeAlex.TipoVizinhanca = "Rook";

                 cps.DefinicaoVizinhos(ref shapeAlex, tipo_vizinhanca);
             }

             clsSpatialCluster clsEspacial = new clsSpatialCluster();
             clsEspacial.PopulacaoInicial(shapeAlex, 5);
         }

         

         private void toolStripMenuItem1_Click(object sender, EventArgs e)
         {
             //Abre o formulário
             frmIndicesSegregacao frmIndices = new frmIndicesSegregacao();

             //Variáveis
             string[] strVariaveis = new string[dsDados.Tables[0].Columns.Count];
             for (int i = 0; i < strVariaveis.Length; i++) strVariaveis[i] = dsDados.Tables[0].Columns[i].ColumnName;
             frmIndices.Variaveis = strVariaveis;
         }

         private void índiceDeMoranToolStripMenuItem_Click(object sender, EventArgs e)
         {
             //Abre a conexão
             frmDependenciaGlobal frmDependenciaGlobal = new frmDependenciaGlobal();

             //Guarda a base de dados
             frmDependenciaGlobal.DataTableDados = dsDados.Tables[0];

             //Guarda o ID do mapa
             frmDependenciaGlobal.IdentificadorMapa = strIDmapa;

             //Guarda o ID da base
             frmDependenciaGlobal.IdentificadorDados = strIDbase;

             //Guarda o endereço do mapa
             frmDependenciaGlobal.EnderecoMapa = strEnderecoMapa;

             //Guarda a estrutura do shape
             frmDependenciaGlobal.EstruturaShape = shapeAlex;

             //Guarda as variáveis
             int intQuantitiativas = 0;
             for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
             {
                 //Guarda as informações
                 Type tipo = dsDados.Tables[0].Columns[i].DataType;
                 //Salva o tipo de interesse
                 string strTipo = tipo.ToString();

                 if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16" || strTipo == "System.Int64" || strTipo == "System.Double")
                 {
                     intQuantitiativas++;
                 }
             }
             string[] strVariaveisQuantitativas = new string[intQuantitiativas];
             string[] strVariaveisQualitativas = new string[dsDados.Tables[0].Columns.Count - intQuantitiativas];
             int dummyQuali = 0;
             int dummyQuanti = 0;

             for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
             {
                 //Guarda as informações
                 Type tipo = dsDados.Tables[0].Columns[i].DataType;
                 //Salva o tipo de interesse
                 string strTipo = tipo.ToString();

                 if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16" || strTipo == "System.Int64" || strTipo == "System.Double")
                 {
                     strVariaveisQuantitativas[dummyQuanti] = dsDados.Tables[0].Columns[i].ColumnName;
                     dummyQuanti++;
                 }
                 else
                 {
                     strVariaveisQualitativas[dummyQuali] = dsDados.Tables[0].Columns[i].ColumnName;
                     dummyQuali++;
                 }
             }

             frmDependenciaGlobal.VariaveisQualitativas = strVariaveisQualitativas;
             frmDependenciaGlobal.VariaveisQuantitativas = strVariaveisQuantitativas;


             //Abre o Dialog
             frmDependenciaGlobal.ShowDialog();

             if (frmDependenciaGlobal.DialogResult == DialogResult.OK)
             {

                 Cursor.Current = Cursors.WaitCursor;
                 Application.DoEvents();


                 #region Gera o Relatório

                 if (frmDependenciaGlobal.GeraRelatorio == true)
                 {



                     clsReport classeReport = new clsReport();
                     string html = classeReport.IndicesDeDependenciaEspacialGlobais(strEnderecoBase, strEnderecoMapa, shapeAlex.Count, shapeAlex.TipoVizinhanca, frmDependenciaGlobal.VariaveisSelecionadasQuantitativas, frmDependenciaGlobal.VariavelPopulacao, frmDependenciaGlobal.NumeroDeSimulacoes, frmDependenciaGlobal.TipoDoPeso, frmDependenciaGlobal.IndiceGeary, frmDependenciaGlobal.pValorIndiceGeary, frmDependenciaGlobal.IndiceGetis, frmDependenciaGlobal.pValorIndiceGetis, frmDependenciaGlobal.IndiceMoran, frmDependenciaGlobal.pValorIndiceMoran, frmDependenciaGlobal.IndiceMoranSimples, frmDependenciaGlobal.pValorIndiceMoranSimples, frmDependenciaGlobal.IndiceRogerson, frmDependenciaGlobal.pValorIndiceRogerson, frmDependenciaGlobal.IndiceTango, frmDependenciaGlobal.pValorIndiceTango);

                     //Abre o relatório
                     frmRelatorio frmRelatorio = new frmRelatorio();
                     frmRelatorio.MdiParent = this.MdiParent;
                     frmRelatorio.CodigoHTML = html;
                     frmRelatorio.Text = "Relatório " + this.Text + ": Índices de dependência espacial globais";
                     frmRelatorio.Show();

                 }



                 #endregion

                 Cursor.Current = Cursors.Default;
                 Application.DoEvents();
             }
         }

         private void lISAToolStripMenuItem_Click(object sender, EventArgs e)
         {
             //Abre a conexão
             frmDependenciaLocal frmDependenciaLocal = new frmDependenciaLocal();

             //Guarda a base de dados
             frmDependenciaLocal.DataTableDados = dsDados.Tables[0];

             //Guarda o ID do mapa
             frmDependenciaLocal.IdentificadorMapa = strIDmapa;

             //Guarda o ID da base
             frmDependenciaLocal.IdentificadorDados = strIDbase;

             //Guarda o endereço do mapa
             frmDependenciaLocal.EnderecoMapa = strEnderecoMapa;

             //Guarda a estrutura do shape
             frmDependenciaLocal.EstruturaShape = shapeAlex;

             //Guarda as variáveis
             int intQuantitiativas = 0;
             for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
             {
                 //Guarda as informações
                 Type tipo = dsDados.Tables[0].Columns[i].DataType;
                 //Salva o tipo de interesse
                 string strTipo = tipo.ToString();

                 if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16" || strTipo == "System.Int64" || strTipo == "System.Double")
                 {
                     intQuantitiativas++;
                 }
             }
             string[] strVariaveisQuantitativas = new string[intQuantitiativas];
             string[] strVariaveisQualitativas = new string[dsDados.Tables[0].Columns.Count - intQuantitiativas];
             int dummyQuali = 0;
             int dummyQuanti = 0;

             for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
             {
                 //Guarda as informações
                 Type tipo = dsDados.Tables[0].Columns[i].DataType;
                 //Salva o tipo de interesse
                 string strTipo = tipo.ToString();

                 if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16" || strTipo == "System.Int64" || strTipo == "System.Double")
                 {
                     strVariaveisQuantitativas[dummyQuanti] = dsDados.Tables[0].Columns[i].ColumnName;
                     dummyQuanti++;
                 }
                 else
                 {
                     strVariaveisQualitativas[dummyQuali] = dsDados.Tables[0].Columns[i].ColumnName;
                     dummyQuali++;
                 }
             }

             frmDependenciaLocal.VariaveisQualitativas = strVariaveisQualitativas;
             frmDependenciaLocal.VariaveisQuantitativas = strVariaveisQuantitativas;


             //Abre o Dialog
             frmDependenciaLocal.ShowDialog();

             if (frmDependenciaLocal.DialogResult == DialogResult.OK)
             {

                 Cursor.Current = Cursors.WaitCursor;
                 Application.DoEvents();

                 //Captura o mapa
                 SharpMap.Map mMapa = mapImage1.Map;

                 //Captura o layer
                 SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[0];

                 //Cria o vetor temático
                 int[] iVetorLisa = new int[dsDados.Tables[0].Rows.Count];
                 int[] iVetorEscore = new int[dsDados.Tables[0].Rows.Count];
                 int[] iVetorGetis = new int[dsDados.Tables[0].Rows.Count];

                 string[] iBaseMapa = new string[dsDados.Tables[0].Rows.Count];
                 for (int i = 0; i < shapeAlex.Count; i++)
                 {
                     iBaseMapa[i] = dsDados.Tables[0].Rows[shapeAlex[i].PosicaoNoDataTable][strIDbase].ToString();
                 }

                 ArrayList arEscore = frmDependenciaLocal.ESCOREmapa;
                 string[] strMapaEscore = new string[frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length];
                 ArrayList arLisa = frmDependenciaLocal.LISAmapa;
                 string[] strMapaLisa = new string[frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length];
                 ArrayList arGetis = frmDependenciaLocal.GETISmapa;
                 string[] strMapaGetis = new string[frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length];
                 ArrayList arEspalhamento = frmDependenciaLocal.ESPALHAMENTOmapa;
                 string[] strMapaEspalhamento = new string[frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length];

                 for (int v = 0; v < frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length; v++)
                 {
                     //Inicializa as cores
                     Brush[] cores = frmDependenciaLocal.CoresParaMapa;
                     if (arLisa.Count > 0)
                     {
                         iVetorLisa = (int[])arLisa[v];
                         //Retira a variável caso já exista
                         string nome = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v]+"_LM";
                         if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);

                         //Adiciona a variavel no mapa
                         dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                         //Guarda os vetores
                         ArrayList vetorID = new ArrayList();
                         int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                         for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                         {
                             vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                             vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                             if (iVetorLisa[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0) dsDados.Tables[0].Rows[i][nome] = iVetorLisa[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                         }

                         //Pinta o mapa
                         SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetorLisa, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                         layMapa.Theme = iTheme;

                         //Refresh
                         mMapa.ZoomToExtents();

                         //Coloca o fundo branco
                         mapImage1.Map.BackColor = Color.White;

                         //Refresh o mapa
                         mapImage1.Refresh();

                         //Gera o mapa
                         System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                         //Nome do MAPA
                         strMapaLisa[v] = Application.StartupPath + "\\MapaLisa_" + v.ToString() + ".jpeg";

                         //Salva o mapa
                         img.Save(strMapaLisa[v], ImageFormat.Jpeg);

                         //Gera espalhamento de MORAN
                         GraphPane myPane = new GraphPane(new RectangleF(0, 0, 824, 631), "Gráfico de espalhamento de Moran.", frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v], "Média dos vizinhos " + frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v]);


                         // Enter some calculated data constants
                         PointPairList list1 = new PointPairList();
                         double[,] dblEspalha = (double[,])arEspalhamento[v];
                         for(int k=0;k<dblEspalha.GetLength(0);k++)
                         {
                             list1.Add(dblEspalha[k, 0],dblEspalha[k, 1]);
                         }

                         // Generate a red curve with diamond symbols, and "Gas Data" in the legend
                         LineItem myCurve = myPane.AddCurve("Espalhamento de Moran", list1, Color.Red,SymbolType.Diamond);

                         myCurve.Symbol.Size = 12;
                         // Set up a red-blue color gradient to be used for the fill
                         myCurve.Symbol.Fill = new Fill(Color.Red, Color.Blue);
                         // Turn off the symbol borders
                         myCurve.Symbol.Border.IsVisible = false;
                         // Turn off the line, so the curve will by symbols only
                         myCurve.Line.IsVisible = false;

                         //Alto-Alto
                         TextObj text1 = new TextObj("Alto-Alto", 2.0, 2.0, CoordType.AxisXYScale);
                         text1.FontSpec.FontColor = Color.Black;
                         text1.FontSpec.Border.IsVisible = false;
                         text1.FontSpec.Fill.IsVisible = false;
                         text1.FontSpec.Size = 14;
                         myPane.GraphObjList.Add(text1);

                         //Alto-Baixo
                         TextObj text2 = new TextObj("Alto-Baixo", 2.0, -2.0, CoordType.AxisXYScale);
                         text2.FontSpec.FontColor = Color.Black;
                         text2.FontSpec.Border.IsVisible = false;
                         text2.FontSpec.Fill.IsVisible = false;
                         text2.FontSpec.Size = 14;
                         myPane.GraphObjList.Add(text2);

                         //Baixo-Alto
                         TextObj text3 = new TextObj("Baixo-Alto", -2.0, 2.0, CoordType.AxisXYScale);
                         text3.FontSpec.FontColor = Color.Black;
                         text3.FontSpec.Border.IsVisible = false;
                         text3.FontSpec.Fill.IsVisible = false;
                         text3.FontSpec.Size = 14;
                         myPane.GraphObjList.Add(text3);

                         //Baixo-Baixo
                         TextObj text4 = new TextObj("Baixo-Baixo", -2.0, -2.0, CoordType.AxisXYScale);
                         text4.FontSpec.FontColor = Color.Black;
                         text4.FontSpec.Border.IsVisible = false;
                         text4.FontSpec.Fill.IsVisible = false;
                         text4.FontSpec.Size = 14;
                         myPane.GraphObjList.Add(text4);


                         // Show the X and Y grids
                         myPane.XAxis.MajorGrid.IsVisible = true;
                         myPane.YAxis.MajorGrid.IsVisible = true;

                         // Set the x and y scale and title font sizes to 14
                         myPane.XAxis.Scale.FontSpec.Size = 14;
                         myPane.XAxis.Title.FontSpec.Size = 14;
                         myPane.YAxis.Scale.FontSpec.Size = 14;
                         myPane.YAxis.Title.FontSpec.Size = 14;
                         // Set the GraphPane title font size to 16
                         myPane.Title.FontSpec.Size = 16;
                         // Turn off the legend
                         myPane.Legend.IsVisible = false;

                         // Fill the axis background with a color gradient
                         myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 90F);
                         
                         myPane.AxisChange();

                         strMapaEspalhamento[v] = Application.StartupPath + "\\Espalhamento_"+v.ToString()+".jpeg";

                        //Salva o histograma
                         myPane.GetImage().Save(strMapaEspalhamento[v], ImageFormat.Jpeg);

                     }
                     if (arGetis.Count > 0)
                     {
                         iVetorGetis = (int[])arGetis[v];
                         //Retira a variável caso já exista
                         string nome = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v] + "_GM";
                         if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);

                         //Adiciona a variavel no mapa
                         dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                         //Guarda os vetores
                         ArrayList vetorID = new ArrayList();
                         int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                         for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                         {
                             vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                             vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                             if (iVetorGetis[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0) dsDados.Tables[0].Rows[i][nome] = iVetorGetis[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                         }

                         //Pinta o mapa
                         SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetorGetis, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                         layMapa.Theme = iTheme;

                         //Refresh
                         mMapa.ZoomToExtents();

                         //Coloca o fundo branco
                         mapImage1.Map.BackColor = Color.White;

                         //Refresh o mapa
                         mapImage1.Refresh();

                         //Gera o mapa
                         System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                         //Nome do MAPA
                         strMapaGetis[v] = Application.StartupPath + "\\MapaGetis_" + v.ToString() + ".jpeg";

                         //Salva o mapa
                         img.Save(strMapaGetis[v], ImageFormat.Jpeg);
                     }
                     if (arEscore.Count > 0)
                     {
                         iVetorEscore = (int[])arEscore[v];
                         //Retira a variável caso já exista
                         string nome = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v] + "_EM";
                         if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);

                         //Adiciona a variavel no mapa
                         dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                         //Guarda os vetores
                         ArrayList vetorID = new ArrayList();
                         int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                         for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                         {
                             vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                             vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                             if (iVetorEscore[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0) dsDados.Tables[0].Rows[i][nome] = iVetorEscore[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                         }

                         //Pinta o mapa
                         SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetorEscore, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                         layMapa.Theme = iTheme;

                         //Refresh
                         mMapa.ZoomToExtents();

                         //Refresh o mapa
                         mapImage1.Refresh();

                         //Gera o mapa
                         System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                         //Nome do MAPA
                         strMapaEscore[v] = Application.StartupPath + "\\MapaEscore_" + v.ToString() + ".jpeg";

                         //Salva o mapa
                         img.Save(strMapaEscore[v], ImageFormat.Jpeg);
                     }
                 }


                 #region Gera o Relatório

                 if (frmDependenciaLocal.GeraRelatorio == true)
                 {

                     clsReport classeReport = new clsReport();
                     string html = classeReport.IndiceDeDependenciaLocal(strEnderecoBase, strEnderecoMapa, shapeAlex.Count,strMapaLisa,strMapaGetis,strMapaEscore,strMapaEspalhamento,frmDependenciaLocal.TipoDeVizinhanca,frmDependenciaLocal.TipoDeCorrecao,frmDependenciaLocal.Confiabilidade,frmDependenciaLocal.Populacao,frmDependenciaLocal.VariaveisSelecionadasQuantitativas,frmDependenciaLocal.CoresRGB);

                     //Abre o relatório
                     frmRelatorio frmRelatorio = new frmRelatorio();
                     frmRelatorio.MdiParent = this.MdiParent;
                     frmRelatorio.CodigoHTML = html;
                     frmRelatorio.Text = "Relatório " + this.Text + ": Índices de dependência espacial locais";

                     //Variáveis
                     frmRelatorio.MapaLisa = arLisa;
                     frmRelatorio.MapaEscore = arEscore;
                     frmRelatorio.Espalhamento = arEspalhamento;
                     frmRelatorio.MapaGetis = arGetis;
                     frmRelatorio.ID = iBaseMapa;

                     frmRelatorio.Show();
                 }



                 #endregion

                 Cursor.Current = Cursors.Default;
                 Application.DoEvents();
             }
         }

         private void toolStripButton1_Click(object sender, EventArgs e)
         {
             clsIndicesDeSegregacao mclasse = new clsIndicesDeSegregacao();
             //double[] medias = mclasse.MediasMoveis(shapeAlex, dsDados.Tables[0], "v_sin01", 1);
             double Segregation = mclasse.Eveness_Segregation_Index(dsDados.Tables[0], "v_sin01",
                 "v_sin09"); 
             int t = 0;
         }

         private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
         {
             this.mapImage1.Map.ZoomToExtents();
             this.mapImage1.Refresh();
         }

         private void listView1_MouseEnter(object sender, EventArgs e)
         {
             this.Cursor = Cursors.Hand;
         }

         private void listView1_MouseLeave(object sender, EventArgs e)
         {
             this.Cursor = Cursors.Default;
         }

         private void LimpaListViewItens(ref ListView lsView)
         {
             int iCont = lsView.Items.Count;
             if (lsView.Items.Count > 1)
             {
                 for (int i = iCont-1; i >= 1; i--)
                 {
                     lsView.Items[i].Remove();
                 }
             }
         }
         private void selecionarPastaToolStripMenuItem_Click(object sender, EventArgs e)
         {
             string startupPath = Application.StartupPath; 
             using (FolderBrowserDialog dialog = new FolderBrowserDialog()) 
             { 
                 dialog.Description = "Selecione a pasta com as bases de dados."; 
                 dialog.ShowNewFolderButton = false; 
                 //dialog.RootFolder = Environment.SpecialFolder.MyComputer; 
                 dialog.RootFolder = Environment.SpecialFolder.Desktop; 
                 if (dialog.ShowDialog() == DialogResult.OK) 
                 {
                     strPastaDados = dialog.SelectedPath;
                     if (listView1.Items.Count > 1) LimpaListViewItens(ref listView1);
                     foreach (string fileName in Directory.GetFiles(strPastaDados, "*.*", SearchOption.TopDirectoryOnly)) 
                     { 
                         FileInfo finfo = new FileInfo(fileName);
                         string strExt = finfo.Extension.ToLower();
                         if (strExt == ".xml" || strExt == ".mdb" || strExt == ".xls" || strExt == ".sas7bdat")
                         {
                             
                             this.listView1.Items.Add(finfo.Name , 0);
                         }
                     } 
                 } 
             }
         }

         private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
         {
             
         }

         private void adDados_Click_1(object sender, EventArgs e)
         {
             //Formulário Pai .sas7bdat
             m_mdiparent = (IpeaGEO.MDIParent1)this.MdiParent;

             OpenFileDialog openFileDialog = new OpenFileDialog();
             openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
             openFileDialog.Filter = "Excel (*.xls)|*.xls|Access (*.mdb)|*.mdb|SAS (*.sas7bdat)|*.sas7bdat|XML (*.xml)|*.xml";
             string FileName = "";
             if (openFileDialog.ShowDialog(this) == DialogResult.OK)
             {

                 FileName = openFileDialog.FileName;

                 //Abre a conexão
                 frmExporta frmConecta = new frmExporta();
                 frmConecta.EnderecoBase = FileName;
                 frmConecta.EnderecoShape = strEnderecoMapa;
                 frmConecta.BaseDeDados = dsDados;

                 //Passa a estrutura do shape
                 frmConecta.EstruturaShape = shapeAlex;

                 //Guarda o endereço da base
                 strEnderecoBase = FileName;

                 //Extensão
                 string strExtensao = Path.GetExtension(FileName).ToUpper();

                 //Guarda a extensão
                 frmConecta.ExtensaoDoArquivo = strExtensao;

                 frmConecta.ShowDialog();

                 if (frmConecta.DialogResult == DialogResult.OK)
                 {

                     Cursor.Current = Cursors.WaitCursor;

                     //Habilita a exportação
                     toolExportaDados.Enabled = true;
                     toolRefresh.Enabled = true;

                     //Guarda a base de dados conectada
                     dsDados = frmConecta.BaseDeDados;

                     //Guarda as variaveis que existem no mapa
                     strVariaveisMapa = frmConecta.VariaveiNoMapa;

                     //Guarda ID mapa
                     strIDmapa = frmConecta.IDmapa;

                     //Guarda ID Base
                     strIDbase = frmConecta.IDbase;

                     //Guarda a estrutura
                     shapeAlex = frmConecta.EstruturaShape;

                     //Fecha o Dialog
                     frmConecta.Close();

                     //Habilita os butões de análise
                     ToolspatialEstat.Enabled = true;
                     mapaTematico.Enabled = true;
                     tlInformacao.Enabled = true;
     

                     //Guarda as bases contidas na pasta de interesse
                     FileInfo finfo1 = new FileInfo(FileName);
                     strPastaDados = finfo1.DirectoryName;
                     int dummy = 1;
                     if (listView1.Items.Count > 1) LimpaListViewItens(ref listView1);
                     foreach (string fileName in Directory.GetFiles(strPastaDados, "*.*", SearchOption.TopDirectoryOnly))
                     {
                         FileInfo finfo = new FileInfo(fileName);
                         string strExt = finfo.Extension.ToLower();
                         if ((strExt == ".xml" || strExt == ".mdb" || strExt == ".xls" || strExt == ".sas7bdat")) //&& (NomeDiferente(listView1, finfo.Name)))
                         {
                             
                             this.listView1.Items.Add(finfo.Name, 0);
                             //this.listView1.Items[dummy].Selected = false;
                             this.listView1.Items[dummy].BackColor = Color.Empty;
                             this.listView1.Items[dummy].ForeColor = this.ForeColor;

                             //Deixa a bse escolhida Checked
                             if (finfo.Name == finfo1.Name)
                             {
                                 //this.listView1.Items[dummy].Selected = true;
                                 this.listView1.Items[dummy].BackColor = SystemColors.ActiveCaption;
                                 this.listView1.Items[dummy].ForeColor = SystemColors.ActiveCaptionText;
                                                                
                             }
                              this.listView1.Refresh(); 
                             dummy++;

                         }
                     }


                     //Guarda o dataTable
                     dataGridView1.DataSource = dsDados.Tables[0];
                     dataGridView1.AllowUserToAddRows = false;
                     dataGridView1.Refresh();

                     //Lista as variáveis
                     listBox1.Items.Clear();
                     for(int i=0; i<dsDados.Tables[0].Columns.Count;i++) listBox1.Items.Add(dsDados.Tables[0].Columns[i].ToString());

                     Cursor.Current = Cursors.Default;
                 }
             }
         }

         private void newToolStripButton_Click_1(object sender, EventArgs e)
         {
             this.mapImage1.Map.Layers.Clear();
             clsMapa classeMapa = new clsMapa();
             classeMapa.loadingMapa(ref mapImage1, strEnderecoMapa, this.Name, ref shapeAlex);
             this.mapImage1.Refresh();
         }

         private void saveToolStripButton_Click_1(object sender, EventArgs e)
         {
             SaveFileDialog saveFileDialog = new SaveFileDialog();
             saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
             saveFileDialog.Filter = "Bitmap (*.bmp)|*.bmp|All Files (*.*)|*.*";
             string FileName = "";
             saveFileDialog.FileName = this.Name;
             if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
             {
                 FileName = saveFileDialog.FileName;


                 //Coloca o fundo branco
                 mapImage1.Map.BackColor = Color.White;

                 //Gera o mapa
                 System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                 //Salva o mapa
                 img.Save(FileName);

                 //Dispose o mapa
                 img.Dispose();
             }
         }

         private void toolExportaDados_Click_1(object sender, EventArgs e)
         {
             DataSet dsTemp = dsDados.Copy();
             //dsTemp.Tables[0].Columns.Remove("Mapa"+strIDmapa);
             SaveFileDialog saveFileDialog1 = new SaveFileDialog();
             saveFileDialog1.InitialDirectory = "C:\\";
             saveFileDialog1.Filter = "Excel (*.xls)|*.xls|Access (*.mdb)|*.mdb|XML (*.xml)|*.xml";
             saveFileDialog1.FilterIndex = 1;
             saveFileDialog1.RestoreDirectory = true;
             if (saveFileDialog1.ShowDialog() == DialogResult.OK)
             {
                 Cursor.Current = Cursors.WaitCursor;

                 string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                 string strFile = saveFileDialog1.FileName;

                 ExportData exporta = new ExportData();

                 if (strExtensao == ".XLS")
                 {
                     exporta.exportToExcel(dsTemp, strFile);
                 }
                 else if (strExtensao == ".XML")
                 {
                     dsTemp.WriteXml(strFile);
                 }
                 else if (strExtensao == ".MDB")
                 {
                     //Cria o arquivo MDB
                     exporta.exportaToAccess(dsTemp, strFile, this.Name);
                 }
                 Cursor.Current = Cursors.Default;
             } 
         }

         private void moveMapa_Click_1(object sender, EventArgs e)
         {
             if (moveMapa.Checked == true)
             {
                 this.tlInformacao.Checked = false;
                 this.toolStripButton4.Checked = false;
                 this.toolStripButton3.Checked = false;
                 this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.Pan;
             }
             else
             {
                 this.tlInformacao.Checked = false;
                 this.toolStripButton4.Checked = false;
                 this.toolStripButton3.Checked = false;
                 this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;
             }
         }

         private void toolStripButton3_Click_1(object sender, EventArgs e)
         {
             if (toolStripButton3.Checked == true)
             {
                 this.tlInformacao.Checked = false;
                 this.moveMapa.Checked = false;
                 this.toolStripButton4.Checked = false;
                 this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.ZoomIn;
             }
             else
             {
                 this.tlInformacao.Checked = false;
                 this.moveMapa.Checked = false;
                 this.toolStripButton4.Checked = false;
                 this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;
             }
         }

         private void toolStripButton4_Click_1(object sender, EventArgs e)
         {
             if (toolStripButton4.Checked == true)
             {
                 this.tlInformacao.Checked = false;
                 this.moveMapa.Checked = false;
                 this.toolStripButton3.Checked = false;
                 this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.ZoomOut;
             }
             else
             {
                 this.tlInformacao.Checked = false;
                 this.moveMapa.Checked = false;
                 this.toolStripButton3.Checked = false;
                 this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;
             }
         }

         private void toolStripButton5_Click_1(object sender, EventArgs e)
         {
             this.mapImage1.Map.ZoomToExtents();
             this.mapImage1.Refresh();
         }

         private void geoReferenciamento_Click_1(object sender, EventArgs e)
         {
             frmGeoreferenciamento frmGeo = new frmGeoreferenciamento();
             frmGeo.EnderecoShape = strEnderecoMapa;
             frmGeo.EstruturaShape = shapeAlex;
             frmGeo.ShowDialog();
             if (frmGeo.DialogResult == DialogResult.OK)
             {
                 shapeAlex = frmGeo.EstruturaShape;
             }
         }

         private void mapaTematico_Click_1(object sender, EventArgs e)
         {
             //Formulário Pai
             m_mdiparent = (IpeaGEO.MDIParent1)this.MdiParent;

             //Abre a conexão
             frmMapaTematico frmMapa = new frmMapaTematico();

             //Aplica os Idenfificadores
             frmMapa.IdentificadorDados = strIDbase;
             frmMapa.IdentificadorMapa = strIDmapa;

             //Aplica o DataTable
             DataTable dTable = dsDados.Tables[0];
             frmMapa.DataTableDados = dTable;

             //Abre o Dialog
             frmMapa.ShowDialog();

             if (frmMapa.DialogResult == DialogResult.OK)
             {
                 Cursor.Current = Cursors.WaitCursor;

                 //Captura o mapa
                 SharpMap.Map mMapa = mapImage1.Map;

                 //Captura o layer
                 SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[0];

                 //Cria o vetor temático
                 int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                 //Guarda as classes
                 dblClasses = frmMapa.ClasseDoMapa;

                 //Guarda o vetor temático
                 iVetor = frmMapa.vetorPoligonos;

                 //Inicializa as cores
                 Brush[] cores = frmMapa.CoresParaMapa;

                 //Guarda os vetores
                 ArrayList vetorID = new ArrayList();
                 int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];

                 for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                 {
                     vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                     vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];

                 }

                 //Pinta o mapa
                 mTheme meuTema = new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice);
                 layMapa.Theme = new SharpMap.Rendering.Thematics.CustomTheme(meuTema.GetStyle);
                 if (frmMapa.GuardaClassificacao == true)
                 {
                     if (dsDados.Tables[0].Columns.Contains("MapaTematico") == false) dsDados.Tables[0].Columns.Add("MapaTematico");
                     for (int i = 0; i < iVetor.Length; i++)
                     {
                         dsDados.Tables[0].Rows[i]["MapaTematico"] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]];
                     }
                 }

                 //Refresh
                 mMapa.ZoomToExtents();

                 //Refresh o mapa
                 mapImage1.Refresh();

                 //Gera relatório
                 if (frmMapa.GeraRelatorio == true)
                 {
                     //Coloca o fundo branco
                     mapImage1.Map.BackColor = Color.White;

                     //Gera o mapa
                     System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                     //Salva o mapa
                     img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);

                     clsReport classeReport = new clsReport();
                     string html = classeReport.MapaTematicoRelatorio(strEnderecoBase, strEnderecoMapa, Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count, frmMapa.Metodologia, frmMapa.ClasseDoMapa, frmMapa.CoresRGB, frmMapa.Variavel);

                     //Abre o relatório
                     frmRelatorio frmRelatorio = new frmRelatorio();
                     frmRelatorio.MdiParent = this.MdiParent;
                     frmRelatorio.CodigoHTML = html;
                     frmRelatorio.Text = "Relatório " + this.Text + ": " + frmMapa.Variavel;

                     string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count + 1, 2];
                     strClusterMapa[0, 0] = strIDbase;
                     strClusterMapa[0, 1] = "Classe";

                     for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                     {
                         string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                         string stCl = dsDados.Tables[0].Rows[i]["MapaTematico"].ToString();
                         strClusterMapa[i + 1, 0] = stId;
                         strClusterMapa[i + 1, 1] = stCl;
                     }

                     frmRelatorio.VariaveisMapa = strClusterMapa;

                     frmRelatorio.Show();
                 }

                 Cursor.Current = Cursors.Default;
             }
         }

         private void análiseDeConglomeradosToolStripMenuItem_Click_1(object sender, EventArgs e)
         {
             //Abre a conexão
             frmCluster frmCluster = new frmCluster();

             //Guarda a base de dados
             frmCluster.DataTableDados = dsDados.Tables[0];

             //Guarda o ID do mapa
             frmCluster.IdentificadorMapa = strIDmapa;

             //Guarda o ID da base
             frmCluster.IdentificadorDados = strIDbase;

             //Guarda o endereço do mapa
             frmCluster.EnderecoMapa = strEnderecoMapa;

             //Define como Conglomerado não espacial
             frmCluster.IsSpatialCluster = false;

             frmCluster.Text = "Análise de conglomerados";

             frmCluster.EstruturaShape = shapeAlex;

             //Guarda as variáveis
             string[] strVariaveis = new string[dsDados.Tables[0].Columns.Count];
             for (int i = 0; i < strVariaveis.Length; i++) strVariaveis[i] = dsDados.Tables[0].Columns[i].ColumnName;
             frmCluster.Variaveis = strVariaveis;

             //Abre o Dialog
             frmCluster.ShowDialog();

             if (frmCluster.DialogResult == DialogResult.OK)
             {

                 Cursor.Current = Cursors.WaitCursor;

                 //Captura o mapa
                 SharpMap.Map mMapa = mapImage1.Map;

                 //Captura o layer
                 SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[0];

                 //Cria o vetor temático
                 int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                 //Guarda o vetor temático
                 iVetor = frmCluster.vetorPoligonos;

                 //Inicializa as cores
                 Brush[] cores = frmCluster.CoresParaMapa;

                 //Retira a variável caso já exista
                 string nome = "ClusterH_" + this.Name;
                 if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);

                 //Adiciona a variavel no mapa
                 dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                 //Guarda os vetores
                 ArrayList vetorID = new ArrayList();
                 int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                 for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                 {
                     vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                     vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                     if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] > 0) dsDados.Tables[0].Rows[i][nome] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                 }

                 //Pinta o mapa
                 SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                 layMapa.Theme = iTheme;

                 //Refresh
                 mMapa.ZoomToExtents();

                 //Refresh o mapa
                 mapImage1.Refresh();

                 #region Gera o Relatório

                 //Coloca o fundo branco
                 mapImage1.Map.BackColor = Color.White;

                 //Gera o mapa
                 System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                 //Salva o mapa
                 img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);

                 clsReport classeReport = new clsReport();
                 string html = classeReport.AnaliseDeConglomerados(strEnderecoBase, strEnderecoMapa, Application.StartupPath + "\\Mapa.jpeg", frmCluster.NumeroDeConglomerados, frmCluster.FatorMinkowsky, shapeAlex.Count, frmCluster.Metodo, frmCluster.Distancia, frmCluster.IsSpatialCluster, frmCluster.VariaveisSelecionadas, frmCluster.VetorDePesos, frmCluster.CoresRGB);

                 //Abre o relatório
                 frmRelatorio frmRelatorio = new frmRelatorio();
                 frmRelatorio.MdiParent = this.MdiParent;
                 frmRelatorio.CodigoHTML = html;
                 frmRelatorio.Text = "Relatório " + this.Text + ": Análise de conglomerados";
                 frmRelatorio.Show();


                 #endregion

                 Cursor.Current = Cursors.Default;
             }
         }

         private void índiceDeMoranToolStripMenuItem_Click_1(object sender, EventArgs e)
         {
             //Abre a conexão
             frmDependenciaGlobal frmDependenciaGlobal = new frmDependenciaGlobal();

             //Guarda a base de dados
             frmDependenciaGlobal.DataTableDados = dsDados.Tables[0];

             //Guarda o ID do mapa
             frmDependenciaGlobal.IdentificadorMapa = strIDmapa;

             //Guarda o ID da base
             frmDependenciaGlobal.IdentificadorDados = strIDbase;

             //Guarda o endereço do mapa
             frmDependenciaGlobal.EnderecoMapa = strEnderecoMapa;

             //Guarda a estrutura do shape
             frmDependenciaGlobal.EstruturaShape = shapeAlex;

             //Guarda as variáveis
             int intQuantitiativas = 0;
             for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
             {
                 //Guarda as informações
                 Type tipo = dsDados.Tables[0].Columns[i].DataType;
                 //Salva o tipo de interesse
                 string strTipo = tipo.ToString();

                 if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16" || strTipo == "System.Int64" || strTipo == "System.Double")
                 {
                     intQuantitiativas++;
                 }
             }
             string[] strVariaveisQuantitativas = new string[intQuantitiativas];
             string[] strVariaveisQualitativas = new string[dsDados.Tables[0].Columns.Count - intQuantitiativas];
             int dummyQuali = 0;
             int dummyQuanti = 0;

             for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
             {
                 //Guarda as informações
                 Type tipo = dsDados.Tables[0].Columns[i].DataType;
                 //Salva o tipo de interesse
                 string strTipo = tipo.ToString();

                 if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16" || strTipo == "System.Int64" || strTipo == "System.Double")
                 {
                     strVariaveisQuantitativas[dummyQuanti] = dsDados.Tables[0].Columns[i].ColumnName;
                     dummyQuanti++;
                 }
                 else
                 {
                     strVariaveisQualitativas[dummyQuali] = dsDados.Tables[0].Columns[i].ColumnName;
                     dummyQuali++;
                 }
             }

             frmDependenciaGlobal.VariaveisQualitativas = strVariaveisQualitativas;
             frmDependenciaGlobal.VariaveisQuantitativas = strVariaveisQuantitativas;


             //Abre o Dialog
             frmDependenciaGlobal.ShowDialog();

             if (frmDependenciaGlobal.DialogResult == DialogResult.OK)
             {

                 Cursor.Current = Cursors.WaitCursor;
                 Application.DoEvents();


                 #region Gera o Relatório

                 if (frmDependenciaGlobal.GeraRelatorio == true)
                 {



                     clsReport classeReport = new clsReport();
                     string html = classeReport.IndicesDeDependenciaEspacialGlobais(strEnderecoBase, strEnderecoMapa, shapeAlex.Count, shapeAlex.TipoVizinhanca, frmDependenciaGlobal.VariaveisSelecionadasQuantitativas, frmDependenciaGlobal.VariavelPopulacao, frmDependenciaGlobal.NumeroDeSimulacoes, frmDependenciaGlobal.TipoDoPeso, frmDependenciaGlobal.IndiceGeary, frmDependenciaGlobal.pValorIndiceGeary, frmDependenciaGlobal.IndiceGetis, frmDependenciaGlobal.pValorIndiceGetis, frmDependenciaGlobal.IndiceMoran, frmDependenciaGlobal.pValorIndiceMoran, frmDependenciaGlobal.IndiceMoranSimples, frmDependenciaGlobal.pValorIndiceMoranSimples, frmDependenciaGlobal.IndiceRogerson, frmDependenciaGlobal.pValorIndiceRogerson, frmDependenciaGlobal.IndiceTango, frmDependenciaGlobal.pValorIndiceTango);

                     //Abre o relatório
                     frmRelatorio frmRelatorio = new frmRelatorio();
                     frmRelatorio.MdiParent = this.MdiParent;
                     frmRelatorio.CodigoHTML = html;
                     frmRelatorio.Text = "Relatório " + this.Text + ": Índices de dependência espacial globais";
                     frmRelatorio.Show();

                 }



                 #endregion

                 Cursor.Current = Cursors.Default;
                 Application.DoEvents();
             }
         }

         private void lISAToolStripMenuItem_Click_1(object sender, EventArgs e)
         {
             //Abre a conexão
             frmDependenciaLocal frmDependenciaLocal = new frmDependenciaLocal();

             //Guarda a base de dados
             frmDependenciaLocal.DataTableDados = dsDados.Tables[0];

             //Guarda o ID do mapa
             frmDependenciaLocal.IdentificadorMapa = strIDmapa;

             //Guarda o ID da base
             frmDependenciaLocal.IdentificadorDados = strIDbase;

             //Guarda o endereço do mapa
             frmDependenciaLocal.EnderecoMapa = strEnderecoMapa;

             //Guarda a estrutura do shape
             frmDependenciaLocal.EstruturaShape = shapeAlex;

             //Guarda as variáveis
             int intQuantitiativas = 0;
             for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
             {
                 //Guarda as informações
                 Type tipo = dsDados.Tables[0].Columns[i].DataType;
                 //Salva o tipo de interesse
                 string strTipo = tipo.ToString();

                 if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16" || strTipo == "System.Int64" || strTipo == "System.Double")
                 {
                     intQuantitiativas++;
                 }
             }
             string[] strVariaveisQuantitativas = new string[intQuantitiativas];
             string[] strVariaveisQualitativas = new string[dsDados.Tables[0].Columns.Count - intQuantitiativas];
             int dummyQuali = 0;
             int dummyQuanti = 0;

             for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
             {
                 //Guarda as informações
                 Type tipo = dsDados.Tables[0].Columns[i].DataType;
                 //Salva o tipo de interesse
                 string strTipo = tipo.ToString();

                 if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16" || strTipo == "System.Int64" || strTipo == "System.Double")
                 {
                     strVariaveisQuantitativas[dummyQuanti] = dsDados.Tables[0].Columns[i].ColumnName;
                     dummyQuanti++;
                 }
                 else
                 {
                     strVariaveisQualitativas[dummyQuali] = dsDados.Tables[0].Columns[i].ColumnName;
                     dummyQuali++;
                 }
             }

             frmDependenciaLocal.VariaveisQualitativas = strVariaveisQualitativas;
             frmDependenciaLocal.VariaveisQuantitativas = strVariaveisQuantitativas;


             //Abre o Dialog
             frmDependenciaLocal.ShowDialog();

             if (frmDependenciaLocal.DialogResult == DialogResult.OK)
             {

                 Cursor.Current = Cursors.WaitCursor;
                 Application.DoEvents();

                 //Captura o mapa
                 SharpMap.Map mMapa = mapImage1.Map;

                 //Captura o layer
                 SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[0];

                 //Cria o vetor temático
                 int[] iVetorLisa = new int[dsDados.Tables[0].Rows.Count];
                 int[] iVetorEscore = new int[dsDados.Tables[0].Rows.Count];
                 int[] iVetorGetis = new int[dsDados.Tables[0].Rows.Count];

                 string[] iBaseMapa = new string[dsDados.Tables[0].Rows.Count];
                 for (int i = 0; i < shapeAlex.Count; i++)
                 {
                     iBaseMapa[i] = dsDados.Tables[0].Rows[shapeAlex[i].PosicaoNoDataTable][strIDbase].ToString();
                 }

                 ArrayList arEscore = frmDependenciaLocal.ESCOREmapa;
                 string[] strMapaEscore = new string[frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length];
                 ArrayList arLisa = frmDependenciaLocal.LISAmapa;
                 string[] strMapaLisa = new string[frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length];
                 ArrayList arGetis = frmDependenciaLocal.GETISmapa;
                 string[] strMapaGetis = new string[frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length];
                 ArrayList arEspalhamento = frmDependenciaLocal.ESPALHAMENTOmapa;
                 string[] strMapaEspalhamento = new string[frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length];

                 for (int v = 0; v < frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length; v++)
                 {
                     //Inicializa as cores
                     Brush[] cores = frmDependenciaLocal.CoresParaMapa;
                     if (arLisa.Count > 0)
                     {
                         iVetorLisa = (int[])arLisa[v];
                         //Retira a variável caso já exista
                         string nome = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v] + "_LM";
                         if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);

                         //Adiciona a variavel no mapa
                         dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                         //Guarda os vetores
                         ArrayList vetorID = new ArrayList();
                         int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                         for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                         {
                             vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                             vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                             if (iVetorLisa[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0) dsDados.Tables[0].Rows[i][nome] = iVetorLisa[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                         }

                         //Pinta o mapa
                         SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetorLisa, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                         layMapa.Theme = iTheme;

                         //Refresh
                         mMapa.ZoomToExtents();

                         //Coloca o fundo branco
                         mapImage1.Map.BackColor = Color.White;

                         //Refresh o mapa
                         mapImage1.Refresh();

                         //Gera o mapa
                         System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                         //Nome do MAPA
                         strMapaLisa[v] = Application.StartupPath + "\\MapaLisa_" + v.ToString() + ".jpeg";

                         //Salva o mapa
                         img.Save(strMapaLisa[v], ImageFormat.Jpeg);

                         //Gera espalhamento de MORAN
                         GraphPane myPane = new GraphPane(new RectangleF(0, 0, 824, 631), "Gráfico de espalhamento de Moran.", frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v], "Média dos vizinhos " + frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v]);


                         // Enter some calculated data constants
                         PointPairList list1 = new PointPairList();
                         double[,] dblEspalha = (double[,])arEspalhamento[v];
                         for (int k = 0; k < dblEspalha.GetLength(0); k++)
                         {
                             list1.Add(dblEspalha[k, 0], dblEspalha[k, 1]);
                         }

                         // Generate a red curve with diamond symbols, and "Gas Data" in the legend
                         LineItem myCurve = myPane.AddCurve("Espalhamento de Moran", list1, Color.Red, SymbolType.Diamond);

                         myCurve.Symbol.Size = 12;
                         // Set up a red-blue color gradient to be used for the fill
                         myCurve.Symbol.Fill = new Fill(Color.Red, Color.Blue);
                         // Turn off the symbol borders
                         myCurve.Symbol.Border.IsVisible = false;
                         // Turn off the line, so the curve will by symbols only
                         myCurve.Line.IsVisible = false;

                         //Alto-Alto
                         TextObj text1 = new TextObj("Alto-Alto", 2.0, 2.0, CoordType.AxisXYScale);
                         text1.FontSpec.FontColor = Color.Black;
                         text1.FontSpec.Border.IsVisible = false;
                         text1.FontSpec.Fill.IsVisible = false;
                         text1.FontSpec.Size = 14;
                         myPane.GraphObjList.Add(text1);

                         //Alto-Baixo
                         TextObj text2 = new TextObj("Alto-Baixo", 2.0, -2.0, CoordType.AxisXYScale);
                         text2.FontSpec.FontColor = Color.Black;
                         text2.FontSpec.Border.IsVisible = false;
                         text2.FontSpec.Fill.IsVisible = false;
                         text2.FontSpec.Size = 14;
                         myPane.GraphObjList.Add(text2);

                         //Baixo-Alto
                         TextObj text3 = new TextObj("Baixo-Alto", -2.0, 2.0, CoordType.AxisXYScale);
                         text3.FontSpec.FontColor = Color.Black;
                         text3.FontSpec.Border.IsVisible = false;
                         text3.FontSpec.Fill.IsVisible = false;
                         text3.FontSpec.Size = 14;
                         myPane.GraphObjList.Add(text3);

                         //Baixo-Baixo
                         TextObj text4 = new TextObj("Baixo-Baixo", -2.0, -2.0, CoordType.AxisXYScale);
                         text4.FontSpec.FontColor = Color.Black;
                         text4.FontSpec.Border.IsVisible = false;
                         text4.FontSpec.Fill.IsVisible = false;
                         text4.FontSpec.Size = 14;
                         myPane.GraphObjList.Add(text4);


                         // Show the X and Y grids
                         myPane.XAxis.MajorGrid.IsVisible = true;
                         myPane.YAxis.MajorGrid.IsVisible = true;

                         // Set the x and y scale and title font sizes to 14
                         myPane.XAxis.Scale.FontSpec.Size = 14;
                         myPane.XAxis.Title.FontSpec.Size = 14;
                         myPane.YAxis.Scale.FontSpec.Size = 14;
                         myPane.YAxis.Title.FontSpec.Size = 14;
                         // Set the GraphPane title font size to 16
                         myPane.Title.FontSpec.Size = 16;
                         // Turn off the legend
                         myPane.Legend.IsVisible = false;

                         // Fill the axis background with a color gradient
                         myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 90F);

                         myPane.AxisChange();

                         strMapaEspalhamento[v] = Application.StartupPath + "\\Espalhamento_" + v.ToString() + ".jpeg";

                         //Salva o histograma
                         myPane.GetImage().Save(strMapaEspalhamento[v], ImageFormat.Jpeg);

                     }
                     if (arGetis.Count > 0)
                     {
                         iVetorGetis = (int[])arGetis[v];
                         //Retira a variável caso já exista
                         string nome = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v] + "_GM";
                         if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);

                         //Adiciona a variavel no mapa
                         dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                         //Guarda os vetores
                         ArrayList vetorID = new ArrayList();
                         int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                         for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                         {
                             vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                             vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                             if (iVetorGetis[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0) dsDados.Tables[0].Rows[i][nome] = iVetorGetis[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                         }

                         //Pinta o mapa
                         SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetorGetis, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                         layMapa.Theme = iTheme;

                         //Refresh
                         mMapa.ZoomToExtents();

                         //Coloca o fundo branco
                         mapImage1.Map.BackColor = Color.White;

                         //Refresh o mapa
                         mapImage1.Refresh();

                         //Gera o mapa
                         System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                         //Nome do MAPA
                         strMapaGetis[v] = Application.StartupPath + "\\MapaGetis_" + v.ToString() + ".jpeg";

                         //Salva o mapa
                         img.Save(strMapaGetis[v], ImageFormat.Jpeg);
                     }
                     if (arEscore.Count > 0)
                     {
                         iVetorEscore = (int[])arEscore[v];
                         //Retira a variável caso já exista
                         string nome = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v] + "_EM";
                         if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);

                         //Adiciona a variavel no mapa
                         dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                         //Guarda os vetores
                         ArrayList vetorID = new ArrayList();
                         int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                         for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                         {
                             vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                             vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                             if (iVetorEscore[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0) dsDados.Tables[0].Rows[i][nome] = iVetorEscore[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                         }

                         //Pinta o mapa
                         SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetorEscore, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                         layMapa.Theme = iTheme;

                         //Refresh
                         mMapa.ZoomToExtents();

                         //Refresh o mapa
                         mapImage1.Refresh();

                         //Gera o mapa
                         System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                         //Nome do MAPA
                         strMapaEscore[v] = Application.StartupPath + "\\MapaEscore_" + v.ToString() + ".jpeg";

                         //Salva o mapa
                         img.Save(strMapaEscore[v], ImageFormat.Jpeg);
                     }
                 }


                 #region Gera o Relatório

                 if (frmDependenciaLocal.GeraRelatorio == true)
                 {

                     clsReport classeReport = new clsReport();
                     string html = classeReport.IndiceDeDependenciaLocal(strEnderecoBase, strEnderecoMapa, shapeAlex.Count, strMapaLisa, strMapaGetis, strMapaEscore, strMapaEspalhamento, frmDependenciaLocal.TipoDeVizinhanca, frmDependenciaLocal.TipoDeCorrecao, frmDependenciaLocal.Confiabilidade, frmDependenciaLocal.Populacao, frmDependenciaLocal.VariaveisSelecionadasQuantitativas, frmDependenciaLocal.CoresRGB);

                     //Abre o relatório
                     frmRelatorio frmRelatorio = new frmRelatorio();
                     frmRelatorio.MdiParent = this.MdiParent;
                     frmRelatorio.CodigoHTML = html;
                     frmRelatorio.Text = "Relatório " + this.Text + ": Índices de dependência espacial locais";

                     //Variáveis
                     frmRelatorio.MapaLisa = arLisa;
                     frmRelatorio.MapaEscore = arEscore;
                     frmRelatorio.Espalhamento = arEspalhamento;
                     frmRelatorio.MapaGetis = arGetis;
                     frmRelatorio.ID = iBaseMapa;

                     frmRelatorio.Show();
                 }



                 #endregion

                 Cursor.Current = Cursors.Default;
                 Application.DoEvents();
             }
         }

         private void bernoulliToolStripMenuItem_Click_1(object sender, EventArgs e)
         {
             //Abre a conexão
             frmScan frmScanEstat = new frmScan();

             //Guarda a base de dados
             frmScanEstat.DataTableDados = dsDados.Tables[0];

             //Guarda o ID do mapa
             frmScanEstat.IdentificadorMapa = strIDmapa;

             //Guarda o ID da base
             frmScanEstat.IdentificadorBase = strIDbase;

             //Guarda o endereço do mapa
             frmScanEstat.EnderecoMapa = strEnderecoMapa;

             //Define a distribuição
             frmScanEstat.Distribuicao = "BERNOULLI";

             //Abre o Dialog
             frmScanEstat.ShowDialog();

             if (frmScanEstat.DialogResult == DialogResult.OK)
             {

                 Cursor.Current = Cursors.WaitCursor;

                 //Captura o mapa
                 SharpMap.Map mMapa = mapImage1.Map;

                 //Captura o layer
                 SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[0];

                 //Cria o vetor temático
                 int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                 //Guarda o vetor temático
                 iVetor = frmScanEstat.vetorPoligonos;

                 //Inicializa as cores
                 Brush[] cores = frmScanEstat.CoresParaMapa;

                 //Adiciona o pvalor na tabela
                 double[] pValor = frmScanEstat.vetorPvalor;

                 //Retira a variável caso já exista
                 string nome = "ScanB_" + frmScanEstat.VariavelEvento + "_" + frmScanEstat.VariavelBase;
                 if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);
                 string nome2 = "ScanB1_" + frmScanEstat.VariavelEvento + "_" + frmScanEstat.VariavelBase;
                 if (dsDados.Tables[0].Columns.Contains(nome2) == true) dsDados.Tables[0].Columns.Remove(nome2);

                 //Adiciona a variavel no mapa
                 dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));
                 dsDados.Tables[0].Columns.Add(nome2, Type.GetType("System.String"));


                 //Guarda os vetores
                 ArrayList vetorID = new ArrayList();
                 int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                 for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                 {
                     vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                     vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                     int iPosicao=Convert.ToInt32(dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]);
                     if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0 && (int)iVetor[iPosicao] < 5) dsDados.Tables[0].Rows[i][nome] = pValor[iVetor[iPosicao]].ToString();
                     if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0 && (int)iVetor[iPosicao] < 5) dsDados.Tables[0].Rows[i][nome2] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                 }

                 //Pinta o mapa
                 SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                 layMapa.Theme = iTheme;

                 //Refresh
                 mMapa.ZoomToExtents();

                 //Refresh o mapa
                 mapImage1.Refresh();

                 //Gera relatório
                 if (frmScanEstat.GeraRelatorio == true)
                 {
                     //Coloca o fundo branco
                     mapImage1.Map.BackColor = Color.White;

                     //Gera o mapa
                     System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                     //Salva o mapa
                     img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);


                     double[] vetorMontecarlo = frmScanEstat.MonteCarlo;
                     Array.Sort(vetorMontecarlo);
                     //Cria os intervalos
                     int iRazao = vetorMontecarlo.Length / 10;
                     double iIntervalos = (vetorMontecarlo[vetorMontecarlo.Length - 1] - vetorMontecarlo[0]) / iRazao;
                     double[] dblAbcissa = new double[iRazao];
                     double iDummy = iIntervalos;
                     for (int i = 0; i < iRazao; i++)
                     {
                         dblAbcissa[i] = iDummy;
                         iDummy += iIntervalos;
                     }

                     //Conta a frequencia
                     double[] dblOrdenada = new double[iRazao];
                     int iConta = 0;
                     for (int j = 0; j < vetorMontecarlo.Length; j++)
                     {
                         if (vetorMontecarlo[j] < dblAbcissa[iConta])
                         {
                             dblOrdenada[iConta]++;
                         }
                         else
                         {
                             dblOrdenada[iConta] /= vetorMontecarlo.Length;
                             if (iConta < dblOrdenada.Length - 1) iConta++;
                             dblOrdenada[iConta]++;
                         }

                     }
                     dblOrdenada[dblOrdenada.Length - 1] /= vetorMontecarlo.Length;

                     //TODO:Programar o HISTOGRAMA

                     //Gera o histograma
                     GraphPane myPane = new GraphPane(new RectangleF(0, 0, 824, 631), "Densidade : Razão de verossimilhança", "Razão de verossimilhança", "Frequência");

                     // Add a red curve with circle symbols
                     LineItem curve = myPane.AddCurve("LLR", dblAbcissa, dblOrdenada, Color.White, SymbolType.None);

                     curve.Line.Width = 1.5F;
                     curve.Line.IsSmooth = true;
                     curve.Line.SmoothTension = 0.8F;

                     // Fill the area under the curve
                     curve.Line.Fill = new Fill(Color.Blue);

                     // Fill the symbols with white to make them opaque
                     curve.Symbol.Fill = new Fill(Color.Blue);

                     // Set the curve type to forward steps
                     //curve.Line.StepType = StepType.ForwardStep;

                     // Fill the axis background with a color gradient
                     myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 45.0F);

                     myPane.AxisChange();

                     //Salva o histograma
                     myPane.GetImage().Save(Application.StartupPath + "\\Histograma.jpeg", ImageFormat.Jpeg);


                     //Gera o relatório
                     clsReport classeReport = new clsReport();
                     string html = classeReport.EstatisticaScanRelatorio(strEnderecoBase, strEnderecoMapa, Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count, frmScanEstat.Distribuicao, frmScanEstat.vetorPvalor,
                     frmScanEstat.CoresRGB, frmScanEstat.VariavelBase, frmScanEstat.VariavelEvento, frmScanEstat.NumeroDeSimulacoes, frmScanEstat.NumeroDePontosGrid, frmScanEstat.RaioMaximo, frmScanEstat.RaioMinimo, frmScanEstat.ProporcaoMaxima, Application.StartupPath + "\\Histograma.jpeg");

                     //Abre o relatório
                     frmRelatorio frmRelatorio = new frmRelatorio();
                     frmRelatorio.MdiParent = this.MdiParent;
                     frmRelatorio.CodigoHTML = html;
                     frmRelatorio.Text = "Relatório Estatística Scan: " + this.Text;

                     string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count + 1, 2];
                     strClusterMapa[0, 0] = strIDbase;
                     strClusterMapa[0, 1] = "Conglomerado";

                     for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                     {
                         string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                         string stCl = dsDados.Tables[0].Rows[i][nome2].ToString();
                         strClusterMapa[i + 1, 0] = stId;
                         strClusterMapa[i + 1, 1] = stCl;
                     }

                     frmRelatorio.HistogramaX = dblAbcissa;
                     frmRelatorio.HistogramaY = dblOrdenada;
                     frmRelatorio.VariaveisMapa = strClusterMapa;

                     frmRelatorio.Show();
                 }



                 Cursor.Current = Cursors.Default;
             }
         }

         private void poissonToolStripMenuItem_Click_1(object sender, EventArgs e)
         {
             //Abre a conexão
             frmScan frmScanEstat = new frmScan();

             //Guarda a base de dados
             frmScanEstat.DataTableDados = dsDados.Tables[0];

             //Guarda o ID do mapa
             frmScanEstat.IdentificadorMapa = strIDmapa;

             //Guarda o ID da base
             frmScanEstat.IdentificadorBase = strIDbase;

             //Define a distribuição
             frmScanEstat.Distribuicao = "POISSON";

             //Guarda o endereço do mapa
             frmScanEstat.EnderecoMapa = strEnderecoMapa;

             //Abre o Dialog
             frmScanEstat.ShowDialog();

             if (frmScanEstat.DialogResult == DialogResult.OK)
             {

                 Cursor.Current = Cursors.WaitCursor;

                 //Captura o mapa
                 SharpMap.Map mMapa = mapImage1.Map;

                 //Captura o layer
                 SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[0];

                 //Cria o vetor temático
                 int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                 //Guarda o vetor temático
                 iVetor = frmScanEstat.vetorPoligonos;

                 //Inicializa as cores
                 Brush[] cores = frmScanEstat.CoresParaMapa;

                 //Adiciona o pvalor na tabela
                 double[] pValor = frmScanEstat.vetorPvalor;

                 //Retira a variável caso já exista
                 string nome = "ScanP_" + frmScanEstat.VariavelEvento + "_" + frmScanEstat.VariavelBase;
                 if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);
                 string nome2 = "ScanP1_" + frmScanEstat.VariavelEvento + "_" + frmScanEstat.VariavelBase;
                 if (dsDados.Tables[0].Columns.Contains(nome2) == true) dsDados.Tables[0].Columns.Remove(nome2);

                 //Adiciona a variavel no mapa
                 dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));
                 dsDados.Tables[0].Columns.Add(nome2, Type.GetType("System.String"));


                 //Guarda os vetores
                 ArrayList vetorID = new ArrayList();
                 int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                 for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                 {
                     vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                     vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                     if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] > 0) dsDados.Tables[0].Rows[i][nome] = pValor[iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] - 1].ToString();
                     if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] > 0) dsDados.Tables[0].Rows[i][nome2] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                 }

                 //Pinta o mapa
                 SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                 layMapa.Theme = iTheme;

                 //Refresh
                 mMapa.ZoomToExtents();

                 //Refresh o mapa
                 mapImage1.Refresh();

                 //Gera relatório
                 if (frmScanEstat.GeraRelatorio == true)
                 {
                     //Coloca o fundo branco
                     mapImage1.Map.BackColor = Color.White;

                     //Gera o mapa
                     System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                     //Salva o mapa
                     img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);


                     double[] vetorMontecarlo = frmScanEstat.MonteCarlo;
                     Array.Sort(vetorMontecarlo);
                     //Cria os intervalos
                     int iRazao = vetorMontecarlo.Length / 10;
                     double iIntervalos = (vetorMontecarlo[vetorMontecarlo.Length - 1] - vetorMontecarlo[0]) / iRazao;
                     double[] dblAbcissa = new double[iRazao];
                     double iDummy = iIntervalos;
                     for (int i = 0; i < iRazao; i++)
                     {
                         dblAbcissa[i] = iDummy;
                         iDummy += iIntervalos;
                     }

                     //Conta a frequencia
                     double[] dblOrdenada = new double[iRazao];
                     int iConta = 0;
                     for (int j = 0; j < vetorMontecarlo.Length; j++)
                     {
                         if (vetorMontecarlo[j] < dblAbcissa[iConta])
                         {
                             dblOrdenada[iConta]++;
                         }
                         else
                         {
                             dblOrdenada[iConta] /= vetorMontecarlo.Length;
                             if (iConta < dblOrdenada.Length - 1) iConta++;
                             dblOrdenada[iConta]++;
                         }

                     }
                     dblOrdenada[dblOrdenada.Length - 1] /= vetorMontecarlo.Length;

                     //TODO:Programar o HISTOGRAMA

                     //Gera o histograma
                     GraphPane myPane = new GraphPane(new RectangleF(0, 0, 824, 631), "Densidade : Razão de verossimilhança", "Razão de verossimilhança", "Frequência");

                     // Add a red curve with circle symbols
                     LineItem curve = myPane.AddCurve("LLR", dblAbcissa, dblOrdenada, Color.White, SymbolType.None);

                     curve.Line.Width = 1.5F;
                     curve.Line.IsSmooth = true;
                     curve.Line.SmoothTension = 0.8F;

                     // Fill the area under the curve
                     curve.Line.Fill = new Fill(Color.Blue);

                     // Fill the symbols with white to make them opaque
                     curve.Symbol.Fill = new Fill(Color.Blue);

                     // Set the curve type to forward steps
                     //curve.Line.StepType = StepType.ForwardStep;

                     // Fill the axis background with a color gradient
                     myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 45.0F);

                     myPane.AxisChange();

                     //Salva o histograma
                     myPane.GetImage().Save(Application.StartupPath + "\\Histograma.jpeg", ImageFormat.Jpeg);

                     //Gera o relatório
                     clsReport classeReport = new clsReport();
                     string html = classeReport.EstatisticaScanRelatorio(strEnderecoBase, strEnderecoMapa, Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count, frmScanEstat.Distribuicao, frmScanEstat.vetorPvalor,
                     frmScanEstat.CoresRGB, frmScanEstat.VariavelBase, frmScanEstat.VariavelEvento, frmScanEstat.NumeroDeSimulacoes, frmScanEstat.NumeroDePontosGrid, frmScanEstat.RaioMaximo, frmScanEstat.RaioMinimo, frmScanEstat.ProporcaoMaxima, Application.StartupPath + "\\Histograma.jpeg");

                     //Abre o relatório
                     frmRelatorio frmRelatorio = new frmRelatorio();
                     frmRelatorio.MdiParent = this.MdiParent;
                     frmRelatorio.CodigoHTML = html;
                     frmRelatorio.Text = "Relatório Estatística Scan: " + this.Text;

                     string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count + 1, 2];
                     strClusterMapa[0, 0] = strIDbase;
                     strClusterMapa[0, 1] = "Conglomerado";

                     for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                     {
                         string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                         string stCl = dsDados.Tables[0].Rows[i][nome2].ToString();
                         strClusterMapa[i + 1, 0] = stId;
                         strClusterMapa[i + 1, 1] = stCl;
                     }

                     frmRelatorio.HistogramaX = dblAbcissa;
                     frmRelatorio.HistogramaY = dblOrdenada;
                     frmRelatorio.VariaveisMapa = strClusterMapa;

                     frmRelatorio.Show();
                 }


                 Cursor.Current = Cursors.Default;


             }
         }

         private void hierarquicoToolStripMenuItem_Click_1(object sender, EventArgs e)
         {
             //Abre a conexão
             frmCluster frmCluster = new frmCluster();

             //Guarda a base de dados
             frmCluster.DataTableDados = dsDados.Tables[0];

             //Guarda o ID do mapa
             frmCluster.IdentificadorMapa = strIDmapa;

             //Guarda o ID da base
             frmCluster.IdentificadorDados = strIDbase;

             //Guarda o endereço do mapa
             frmCluster.EnderecoMapa = strEnderecoMapa;

             //Guarda a estrutura do shape
             frmCluster.EstruturaShape = shapeAlex;

             //Define como conglomerado espacial
             frmCluster.IsSpatialCluster = true;

             //Habilita o Label "Vizinhança"
             frmCluster.label4.Visible = true;

             //Modifica o tamanho do GroupBox
             //frmCluster.groupBox1.Size = new Size(240, 256);

             //Guarda as variáveis
             string[] strVariaveis = new string[dsDados.Tables[0].Columns.Count];
             for (int i = 0; i < strVariaveis.Length; i++) strVariaveis[i] = dsDados.Tables[0].Columns[i].ColumnName;
             frmCluster.Variaveis = strVariaveis;

             //Abre o Dialog
             frmCluster.ShowDialog();

             if (frmCluster.DialogResult == DialogResult.OK)
             {

                 Cursor.Current = Cursors.WaitCursor;

                 //Captura o mapa
                 SharpMap.Map mMapa = mapImage1.Map;

                 //Captura o layer
                 SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[0];

                 //Cria o vetor temático
                 int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                 //Guarda o vetor temático
                 iVetor = frmCluster.vetorPoligonos;

                 //Inicializa as cores
                 Brush[] cores = frmCluster.CoresParaMapa;

                 //Retira a variável caso já exista
                 string nome = "ScanH_" + this.Name;
                 if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);

                 //Adiciona a variavel no mapa
                 dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                 //Guarda os vetores
                 ArrayList vetorID = new ArrayList();
                 int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                 for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                 {
                     vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                     vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                     if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0) dsDados.Tables[0].Rows[i][nome] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                 }

                 //Pinta o mapa
                 SharpMap.Rendering.Thematics.CustomTheme iTheme = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                 layMapa.Theme = iTheme;

                 //Refresh
                 mMapa.ZoomToExtents();

                 //Refresh o mapa
                 mapImage1.Refresh();

                 #region Gera o Relatório

                 if (frmCluster.GeraRelatorio == true)
                 {
                     //Métodos para a escolha do número de conglomerados
                     dblCCC = frmCluster.CCC;
                     dblPseudoF = frmCluster.PseudoF;
                     dblPseudoT = frmCluster.PseudoT;
                     dblRSquare = frmCluster.RSquare;
                     dblRSquareExpected = frmCluster.RSquareExpected;
                     dblRSquarePartial = frmCluster.RSquarePartial;

                     #region Gera Gráficos

                     // Get a reference to the GraphPane
                     GraphPane myPane = new GraphPane(new RectangleF(0, 0, 824, 631), "Escolha do tamanho ótimo do número de conglomerados.", "Número de conglomerados", "Pseudo T");

                     // Set the titles and axis labels
                     myPane.Title.Text = "Escolha do tamanho ótimo do número de conglomerados.";
                     myPane.XAxis.Title.Text = "Número de conglomerados";
                     myPane.YAxis.Title.Text = "Pseudo T";
                     myPane.Y2Axis.Title.Text = "R-Square";

                     //List of points
                     PointPairList ptList = new PointPairList();
                     PointPairList rsList = new PointPairList();
                     PointPairList pfList = new PointPairList();
                     PointPairList cccList = new PointPairList();

                     // Fabricate some data values
                     int iObs = Convert.ToInt32(frmCluster.NumeroDeConglomerados) * 2;
                     int iTotal = dblRSquare.Length - 1;
                     for (int i = 0; i < dblRSquare.Length /*iObs*/; i++)
                     {
                         rsList.Add(i + 2, dblRSquare[iTotal]);
                         ptList.Add(i + 2, dblPseudoT[iTotal]);
                         cccList.Add(i + 2, dblCCC[iTotal]);
                         pfList.Add(i + 2, dblPseudoF[iTotal]);
                         iTotal--;
                     }

                     // Generate a red curve with diamond symbols, and "PseudoT" in the legend
                     LineItem myCurve = myPane.AddCurve("Pseudo T", ptList, Color.Red, SymbolType.Diamond);
                     // Fill the symbols with white
                     myCurve.Symbol.Fill = new Fill(Color.White);

                     // Generate a blue curve with circle symbols, and "RSquare" in the legend
                     myCurve = myPane.AddCurve("R-Square", rsList, Color.Blue, SymbolType.Circle);
                     // Fill the symbols with white
                     myCurve.Symbol.Fill = new Fill(Color.White);
                     // Associate this curve with the Y2 axis
                     myCurve.IsY2Axis = true;

                     // Generate a green curve with square symbols, and "PseudoF" in the legend
                     myCurve = myPane.AddCurve("Pseudo F", pfList, Color.Green, SymbolType.Square);
                     // Fill the symbols with white
                     myCurve.Symbol.Fill = new Fill(Color.White);
                     // Associate this curve with the second Y axis
                     myCurve.YAxisIndex = 1;

                     // Generate a Black curve with triangle symbols, and "CCC" in the legend
                     myCurve = myPane.AddCurve("CCC", cccList, Color.Black, SymbolType.Triangle);
                     // Fill the symbols with white
                     myCurve.Symbol.Fill = new Fill(Color.White);
                     // Associate this curve with the Y2 axis
                     myCurve.IsY2Axis = true;
                     // Associate this curve with the second Y2 axis
                     myCurve.YAxisIndex = 1;

                     // Show the x axis grid
                     myPane.XAxis.MajorGrid.IsVisible = true;

                     // Make the Y axis scale red
                     myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
                     myPane.YAxis.Title.FontSpec.FontColor = Color.Red;
                     // turn off the opposite tics so the Y tics don't show up on the Y2 axis
                     myPane.YAxis.MajorTic.IsOpposite = false;
                     myPane.YAxis.MinorTic.IsOpposite = false;
                     // Don't display the Y zero line
                     myPane.YAxis.MajorGrid.IsZeroLine = false;

                     // Enable the Y2 axis display
                     myPane.Y2Axis.IsVisible = true;
                     // Make the Y2 axis scale blue
                     myPane.Y2Axis.Scale.FontSpec.FontColor = Color.Blue;
                     myPane.Y2Axis.Title.FontSpec.FontColor = Color.Blue;
                     // turn off the opposite tics so the Y2 tics don't show up on the Y axis
                     myPane.Y2Axis.MajorTic.IsOpposite = false;
                     myPane.Y2Axis.MinorTic.IsOpposite = false;
                     // Display the Y2 axis grid lines
                     myPane.Y2Axis.MajorGrid.IsVisible = true;
                     // Align the Y2 axis labels so they are flush to the axis
                     myPane.Y2Axis.Scale.Align = AlignP.Inside;

                     // Create a second Y Axis, green
                     YAxis yAxis3 = new YAxis("Pseudo F");
                     myPane.YAxisList.Add(yAxis3);
                     yAxis3.Scale.FontSpec.FontColor = Color.Green;
                     yAxis3.Title.FontSpec.FontColor = Color.Green;
                     yAxis3.Color = Color.Green;
                     // turn off the opposite tics so the Y2 tics don't show up on the Y axis
                     yAxis3.MajorTic.IsInside = false;
                     yAxis3.MinorTic.IsInside = false;
                     yAxis3.MajorTic.IsOpposite = false;
                     yAxis3.MinorTic.IsOpposite = false;
                     // Align the Y2 axis labels so they are flush to the axis
                     yAxis3.Scale.Align = AlignP.Inside;

                     Y2Axis yAxis4 = new Y2Axis("CCC");
                     yAxis4.IsVisible = true;
                     myPane.Y2AxisList.Add(yAxis4);
                     // turn off the opposite tics so the Y2 tics don't show up on the Y axis
                     yAxis4.MajorTic.IsInside = false;
                     yAxis4.MinorTic.IsInside = false;
                     yAxis4.MajorTic.IsOpposite = false;
                     yAxis4.MinorTic.IsOpposite = false;
                     // Align the Y2 axis labels so they are flush to the axis
                     yAxis4.Scale.Align = AlignP.Inside;

                     // Fill the axis background with a gradient
                     myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);

                     myPane.AxisChange();

                     myPane.Border.IsVisible = false;

                     //Salva o histograma
                     myPane.GetImage().Save(Application.StartupPath + "\\NumeroConglomerados.jpeg", ImageFormat.Jpeg);


                     #endregion

                     //Coloca o fundo branco
                     mapImage1.Map.BackColor = Color.White;

                     //Gera o mapa
                     System.Drawing.Bitmap img = (System.Drawing.Bitmap)this.mapImage1.Map.GetMap();

                     //Salva o mapa
                     img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);

                     clsReport classeReport = new clsReport();
                     string html = classeReport.AnaliseDeConglomeradosEspacial(strEnderecoBase, strEnderecoMapa, Application.StartupPath + "\\Mapa.jpeg", frmCluster.NumeroDeConglomerados, frmCluster.FatorMinkowsky, shapeAlex.Count, frmCluster.Metodo, frmCluster.Distancia, frmCluster.strEML, frmCluster.IsSpatialCluster, frmCluster.VariaveisSelecionadas, frmCluster.CoresRGB, shapeAlex.TipoVizinhanca, Application.StartupPath + "\\NumeroConglomerados.jpeg");

                     //Abre o relatório
                     frmRelatorio frmRelatorio = new frmRelatorio();
                     frmRelatorio.MdiParent = this.MdiParent;
                     frmRelatorio.CodigoHTML = html;
                     frmRelatorio.Text = "Relatório " + this.Text + ": Análise de conglomerados espaciais";

                     //Métodos para a escolha do número de conglomerados
                     frmRelatorio.CCC = dblCCC;
                     frmRelatorio.PseudoF = dblPseudoF;
                     frmRelatorio.PseudoT = dblPseudoT;
                     frmRelatorio.RSquare = dblRSquare;
                     frmRelatorio.RSquareExpected = dblRSquareExpected;
                     frmRelatorio.RSquarePartial = dblRSquarePartial;

                     string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count + 1, 2];
                     strClusterMapa[0, 0] = strIDbase;
                     strClusterMapa[0, 1] = "Conglomerado";

                     for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                     {
                         string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                         string stCl = dsDados.Tables[0].Rows[i][nome].ToString();
                         strClusterMapa[i + 1, 0] = stId;
                         strClusterMapa[i + 1, 1] = stCl;
                     }

                     frmRelatorio.VariaveisMapa = strClusterMapa;

                     frmRelatorio.Show();
                 }

                 #endregion

                 Cursor.Current = Cursors.Default;
             }
              
         }

         private void entropiaToolStripMenuItem_Click_1(object sender, EventArgs e)
         {
             if (shapeAlex.TipoVizinhanca == "")
             {
                 //Cria a vizinhnaça
                 clsIpeaShape cps = new clsIpeaShape();
                 int tipo_vizinhanca = 1;

                 //"Queen" tipo_vizinhanca = 1;
                 //"Rook " tipo_vizinhanca = 2;

                 if (tipo_vizinhanca == 1) shapeAlex.TipoVizinhanca = "Queen";
                 if (tipo_vizinhanca == 2) shapeAlex.TipoVizinhanca = "Rook";

                 cps.DefinicaoVizinhos(ref shapeAlex, tipo_vizinhanca);
             }

             clsSpatialCluster clsEspacial = new clsSpatialCluster();
             clsEspacial.PopulacaoInicial(shapeAlex, 5);
         }

         private void modelosLinearesEspaciaisToolStripMenuItem_Click(object sender, EventArgs e)
         {

         }

         private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
         {
             //Abre o formulário
             frmIndicesSegregacao frmIndices = new frmIndicesSegregacao();

             //Variáveis
             string[] strVariaveis = new string[dsDados.Tables[0].Columns.Count];
             for (int i = 0; i < strVariaveis.Length; i++) strVariaveis[i] = dsDados.Tables[0].Columns[i].ColumnName;
             frmIndices.Variaveis = strVariaveis;
         }

         private bool NomeDiferente(ListView listView1, string finfo)
         {
             for (int i = 0; i < listView1.Items.Count; i++)
             {
                 if (listView1.Items[i].Text == finfo) return (false);
             }
             return (false);

         }

         private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
         {
             if ((e.Item.BackColor != SystemColors.ActiveCaption) && e.Item.Selected )
             {

                 string FileName = "";
                 if (strPastaDados == null || e.Item.Text == "IpeaGEO") FileName = appPath + "\\IpeaGEO.xls";
                 else FileName = strPastaDados + "\\" + listView1.Items[e.Item.Index].Text;

                 //Abre a conexão
                 frmExporta frmConecta = new frmExporta();
                 frmConecta.EnderecoBase = FileName;
                 frmConecta.EnderecoShape = strEnderecoMapa;
                 frmConecta.BaseDeDados = dsDados.Clone();

                 //Passa a estrutura do shape
                 frmConecta.EstruturaShape = shapeAlex;

                 //Guarda o endereço da base
                 strEnderecoBase = FileName;

                 //Extensão
                 string strExtensao = Path.GetExtension(FileName).ToUpper();

                 //Guarda a extensão
                 frmConecta.ExtensaoDoArquivo = strExtensao;

                 frmConecta.ShowDialog();

                 if (frmConecta.DialogResult == DialogResult.OK)
                 {

                     Cursor.Current = Cursors.WaitCursor;

                     //Habilita a exportação
                     toolExportaDados.Enabled = true;
                     toolRefresh.Enabled = true;

                     //Guarda a base de dados conectada
                     dsDados = frmConecta.BaseDeDados;

                     //Guarda as variaveis que existem no mapa
                     strVariaveisMapa = frmConecta.VariaveiNoMapa;

                     //Guarda ID mapa
                     strIDmapa = frmConecta.IDmapa;

                     //Guarda ID Base
                     strIDbase = frmConecta.IDbase;

                     //Guarda a estrutura
                     shapeAlex = frmConecta.EstruturaShape;

                     //Fecha o Dialog
                     frmConecta.Close();

                     //Habilita os butões de análise
                     ToolspatialEstat.Enabled = true;
                     mapaTematico.Enabled = true;
                     tlInformacao.Enabled = true;
          

                     if (strPastaDados != null)
                     {
                         //Guarda as bases contidas na pasta de interesse
                         FileInfo finfo1 = new FileInfo(FileName);
                         strPastaDados = finfo1.DirectoryName;

                         for (int i = 0; i < listView1.Items.Count; i++)
                         {
                             if (listView1.Items[i].Text == finfo1.Name)
                             {
                                 this.listView1.Items[i].BackColor = SystemColors.ActiveCaption;
                                 this.listView1.Items[i].ForeColor = SystemColors.ActiveCaptionText;
                             }
                             else
                             {
                                 this.listView1.Items[i].BackColor = Color.Empty;
                                 this.listView1.Items[i].ForeColor = this.ForeColor;

                             }
                         }
                         this.listView1.Refresh();
    
                         //Guarda o dataTable
                         dataGridView1.DataSource = dsDados.Tables[0];

                         //Lista as variáveis
                         listBox1.Items.Clear();
                         for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++) listBox1.Items.Add(dsDados.Tables[0].Columns[i].ToString());

                         Cursor.Current = Cursors.Default;
                     }
                     else
                     {
                         //Guarda as bases contidas na pasta de interesse
                         FileInfo finfo1 = new FileInfo(FileName);
                         strPastaDados = finfo1.DirectoryName;

                         for (int i = 0; i < listView1.Items.Count; i++)
                         {
                             if (listView1.Items[i].Text == "IpeaGEO")
                             {
                                 this.listView1.Items[i].BackColor = SystemColors.ActiveCaption;
                                 this.listView1.Items[i].ForeColor = SystemColors.ActiveCaptionText;
                             }
                             else
                             {
                                 this.listView1.Items[i].BackColor = Color.Empty;
                                 this.listView1.Items[i].ForeColor = this.ForeColor;

                             }
                         }
                         this.listView1.Refresh();

                         //Guarda o dataTable
                         dataGridView1.DataSource = dsDados.Tables[0];

                         //Lista as variáveis
                         listBox1.Items.Clear();
                         for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++) listBox1.Items.Add(dsDados.Tables[0].Columns[i].ToString());

                         Cursor.Current = Cursors.Default;
                     }

                     int t = 0;
                 }
             }
         }

  
         private void iconesPequenosToolStripMenuItem_Click(object sender, EventArgs e)
         {
             listView1.View = System.Windows.Forms.View.SmallIcon;
         }

         private void iconesGranesToolStripMenuItem_Click(object sender, EventArgs e)
         {
             listView1.View = System.Windows.Forms.View.LargeIcon;
         }

         private void selecionaTodosToolStripMenuItem_Click(object sender, EventArgs e)
         {

             for (int i = 0; i < listBox1.Items.Count; i++) listBox1.SetSelected(i, true);
         }

         private void limpaTodosToolStripMenuItem_Click(object sender, EventArgs e)
         {
             for (int i = 0; i < listBox1.Items.Count; i++) listBox1.SetSelected(i, false);
         }

         private void toolRefresh_Click(object sender, EventArgs e)
         {
             this.Cursor = Cursors.WaitCursor;
             //Captura o mapa
             SharpMap.Map mMapa = mapImage1.Map;

             //Captura o layer
             SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[0];

             DataSet dsTemp = new DataSet();
             dsTemp = dsDados.Copy();

             if (listBox1.SelectedItems.Count == listBox1.Items.Count || listBox1.SelectedItems.Count == 0)
             {
                 //Lista de polígonos viziveis
                 double minX = mapImage1.Map.Envelope.Min.X;
                 double minY = mapImage1.Map.Envelope.Min.Y;
                 double maxX = mapImage1.Map.Envelope.Max.X;
                 double maxY = mapImage1.Map.Envelope.Max.Y;

                 dsTemp.Tables[0].Clear();

                 //Lista de poligonos visiveis
                 for (int i = 0; i < shapeAlex.Count; i++)
                 {
                     if (shapeAlex[i].XCentroide < maxX && shapeAlex[i].XCentroide > minX && shapeAlex[i].YCentroide < maxY && shapeAlex[i].YCentroide > minY)
                     {
                         DataRow dr = dsDados.Tables[0].NewRow();
                         dr = dsDados.Tables[0].Rows[shapeAlex[i].PosicaoNoDataTable];
                         dsTemp.Tables[0].ImportRow(dr);
                     }
                 }
                 dataGridView1.DataSource = dsTemp.Tables[0];
                 dataGridView1.Refresh();
             }
             else
             {
                 for (int i = 0; i < listBox1.Items.Count; i++)
                 {
                     if (!listBox1.GetSelected(i))
                     {
                         dsTemp.Tables[0].Columns.Remove(listBox1.Items[i].ToString());
                     }
   
                 }

                 //Lista de polígonos viziveis
                 double minX = mapImage1.Map.Envelope.Min.X;
                 double minY = mapImage1.Map.Envelope.Min.Y;
                 double maxX = mapImage1.Map.Envelope.Max.X;
                 double maxY = mapImage1.Map.Envelope.Max.Y;

                 dsTemp.Tables[0].Clear();
                 
                 //Lista de poligonos visiveis
                 for (int i = 0; i < shapeAlex.Count ; i++)
                 {
                     if (shapeAlex[i].XCentroide < maxX && shapeAlex[i].XCentroide > minX && shapeAlex[i].YCentroide < maxY && shapeAlex[i].YCentroide  > minY)
                     {
                         DataRow dr = dsDados.Tables[0].NewRow();
                         dr = dsDados.Tables[0].Rows[shapeAlex[i].PosicaoNoDataTable];
                         dsTemp.Tables[0].ImportRow(dr);
                     }
                 }



                 dataGridView1.DataSource = dsTemp.Tables[0];
                 dataGridView1.Refresh();




             }

      

             this.Cursor = Cursors.Default;
         }

 

         private void tlInformacao_Click_1(object sender, EventArgs e)
         {

         }
    
         private void mapImage1_MouseMove(SharpMap.Geometries.Point WorldPos, MouseEventArgs ImagePos)
         {
           
         }

         private void mapImage1_MouseDown_1(SharpMap.Geometries.Point WorldPos, MouseEventArgs ImagePos)
         {
             //Se o usuário habilitar a pesquisa
             if (tlInformacao.Checked == true)
             {
                 //Desabilita qualquer função de ZOOM ou PAN
                 this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;

                 //Passo 1: Obtém o ponto clicado
                 double x = WorldPos.X;
                 double y = WorldPos.Y;

                 //Passo 2: Obtém o polígono escolhido
                 clsMapa ClasseMapa = new clsMapa();
                 int iPoligono = -1;
                 int iPoligonoBase = -1;

                 iPoligono = ClasseMapa.identificaPontoPoligono(strEnderecoMapa, x, y);

                 //Captura os dados
                 DataSet dsTemp = new DataSet();
                 dsTemp = dsDados.Copy();
                 if (listBox1.SelectedItems.Count == listBox1.Items.Count || listBox1.SelectedItems.Count == 0)
                 {

                     dataGridView1.DataSource = dsDados.Tables[0];
                     dataGridView1.Refresh();
                 }
                 else
                 {
                     for (int i = 0; i < listBox1.Items.Count; i++)
                     {
                         if (!listBox1.GetSelected(i))
                         {
                             dsTemp.Tables[0].Columns.Remove(listBox1.Items[i].ToString());
                         }

                     }
                     dataGridView1.DataSource = dsTemp.Tables[0];
                     dataGridView1.Refresh();
                 }


                 DataRow dRow = dsTemp.Tables[0].Rows[0];
                 for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                 {
                     if (dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa].ToString() == iPoligono.ToString())
                     {
                         dRow = dsTemp.Tables[0].Rows[i];
                         iPoligonoBase = i;
                         break;
                     }
                 }
                 string strInfor = "";

                 if (-1 < iPoligono && iPoligono < shapeAlex.Count)
                 {
                     for (int i = 0; i < dRow.Table.Columns.Count; i++)
                     {
                         strInfor += dRow.Table.Columns[i] + ": " + dRow.Table.Rows[iPoligonoBase][i].ToString() + "\n";
                     }

                     toolTip1.Show(strInfor, this, ImagePos.X, ImagePos.X, 5000);
                 }
             }

         }

         private void frmMapa_MaximizedBoundsChanged_1(object sender, EventArgs e)
         {
          
         }

         private void frmMapa_Resize_1(object sender, EventArgs e)
         {
             this.mapImage1.Map.ZoomToExtents();
             this.mapImage1.Refresh();
         }








    }
}
