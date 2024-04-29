using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IpeaGeo
{
    public partial class frmRelatorio : Form
    {
        public frmRelatorio()
        {
            InitializeComponent();
        }

        private string strImagem = "";

        #region Métodos para a escolha do número de conglomerados
        
        public double[] pseudoT, pseudoF, rSquare, partialRSquare, expectedRSquare, ccc;
        
        #endregion

        // Public variables
        
        public ArrayList mapaEspalhamento, mapaLisa, mapaGetis, mapaEscore, mapaGetis2;

        public bool isCluster, isGlobalDependence, isLocalDependence, isScan,
            isSegregation, isThematicMap;

        public double[] pValor, moran, moranP, moranSimp, moranSimpP, 
            rogerson, rogersonP, tango, tangoP, getis, getisP, geary, gearyP, 
            histogramaX, histogramaY;

        public double[,] segregacaoValores;

        public int numeroSimulacoes;

        public string distribuicao, variavelBase, variavelEvento, raioMaximo,
            raioMinimo, proporcaoMaxima, enderecoHistograma, strNumeroSimulacoes,
            numeroPontosGrid, tipoCorrecao, confiabilidade, populacao, tipoVizinhanca,
            variavelPopulacao, tipoDePeso, fatorMinkowski, numeroConglomerados, distancia, 
            endCCC, codigoHTML;

        public string[] legendaMapaTematico, segregacaoIndices, segregacaoVariaveis,
            strMapaLisa, strMapaGetis, strMapaEscore, strMapaEspalhamento,
            variaveisSelecionadasQuant, nomeVariaveisMapa, id, strMapaGetis2;

        public string[,] variaveisMapa;

        // Private variables

        private bool isSpatialCluster, modelFit;

        private DataSet graphicsData;

        private DataTable dataTable, residuals;

        private string regressionMethod, htmlToPDF;

        private string[] weights;

        #region Variaveis regressao

        private string[] sbetas;
        
        private double[] dbetas;
        
        private double[] t;
        
        private double[] p;
        
        private double[] dev;

        private double rho;

        private double rhoT;

        private double rhoP;

        private double rhoDev;

        private double aic;

        private double bic;

        private double loglik;
        
        private double sigma2;

        public string metodo;

        public string varMapa;

        public string endMapa;

        public double[] classesMapaReg;
        
        public string[] coresMapaReg;

        private Brush[] coresMapaReg2;

        public string enderecoMapa;

        public string enderecoBase;

        public int shapeCount;

        #endregion

        private void frmRelatorio_Load(object sender, EventArgs e)
        {
            try
            {
                this.webBrowser1.DocumentText = codigoHTML;
                this.webBrowser1.Refresh();
                this.webBrowser1.ContextMenuStrip = contextMenuStrip1;    //! Set our ContextMenuStrip
                this.webBrowser1.IsWebBrowserContextMenuEnabled = false;  //! Disable the default IE ContextMenu

                //Limpa o DataSet
                dataSet1.Clear();

                //Guarda os dados
                dataSet1.Tables.Add("Mapa");
                if (variaveisMapa != null)
                {
                    dataSet1.Tables["Mapa"].Columns.Add(variaveisMapa[0, 0], Type.GetType("System.String"));
                    dataSet1.Tables["Mapa"].Columns.Add(variaveisMapa[0, 1], Type.GetType("System.String"));

                    for (int i = 1; i < variaveisMapa.GetLength(0); i++)
                    {
                        DataRow dLinha = dataSet1.Tables["Mapa"].NewRow();
                        dLinha[variaveisMapa[0, 0]] = variaveisMapa[i, 0];
                        dLinha[variaveisMapa[0, 1]] = variaveisMapa[i, 1];
                        dataSet1.Tables["Mapa"].Rows.Add(dLinha);
                    }
                }

                //Cluster espacial
                if (pseudoF != null)
                {
                    //"System.Int32"
                    //"System.Double"

                    dataSet1.Tables.Add("NumeroClusters");
                    dataSet1.Tables["NumeroClusters"].Columns.Add("NumeroConglomerados", Type.GetType("System.String"));
                    dataSet1.Tables["NumeroClusters"].Columns.Add("PseudoF", Type.GetType("System.String"));
                    dataSet1.Tables["NumeroClusters"].Columns.Add("PseudoT", Type.GetType("System.String"));
                    dataSet1.Tables["NumeroClusters"].Columns.Add("CCC", Type.GetType("System.String"));
                    dataSet1.Tables["NumeroClusters"].Columns.Add("RSquare", Type.GetType("System.String"));
                    dataSet1.Tables["NumeroClusters"].Columns.Add("RSquareExpected", Type.GetType("System.String"));
                    dataSet1.Tables["NumeroClusters"].Columns.Add("RSquarePartial", Type.GetType("System.String"));

                    int iConta = pseudoF.Length - 1;
                    for (int i = 0; i < pseudoF.Length; i++)
                    {
                        DataRow dLinha = dataSet1.Tables["NumeroClusters"].NewRow();
                        dLinha["NumeroConglomerados"] = (i + 2).ToString();
                        dLinha["PseudoF"] = pseudoF[iConta].ToString();
                        dLinha["PseudoT"] = pseudoT[iConta].ToString();
                        dLinha["CCC"] = ccc[iConta].ToString();
                        dLinha["RSquare"] = rSquare[iConta].ToString();
                        dLinha["RSquareExpected"] = expectedRSquare[iConta].ToString();
                        dLinha["RSquarePartial"] = partialRSquare[iConta].ToString();
                        dataSet1.Tables["NumeroClusters"].Rows.Add(dLinha);
                        iConta--;
                    }
                }

                //Scan
                if (histogramaX != null)
                {
                    dataSet1.Tables.Add("Histograma");
                    dataSet1.Tables["Histograma"].Columns.Add("X", Type.GetType("System.String"));
                    dataSet1.Tables["Histograma"].Columns.Add("Y", Type.GetType("System.String"));

                    for (int i = 0; i < histogramaX.Length; i++)
                    {
                        DataRow dLinha = dataSet1.Tables["Histograma"].NewRow();
                        dLinha["X"] = histogramaX[i].ToString();
                        dLinha["Y"] = histogramaY[i].ToString();
                        dataSet1.Tables["Histograma"].Rows.Add(dLinha);
                    }
                }

                //Lisa
                if (mapaLisa != null) if (mapaLisa.Count > 0)
                {
                    for (int v = 0; v < mapaLisa.Count; v++)
                    {
                        dataSet1.Tables.Add("Lisa_" + v.ToString());
                        dataSet1.Tables["Lisa_" + v.ToString()].Columns.Add("ID", Type.GetType("System.String"));
                        dataSet1.Tables["Lisa_" + v.ToString()].Columns.Add("LISA", Type.GetType("System.String"));
                        int[] iPinta = (int[])mapaLisa[v];
                        for (int i = 0; i < iPinta.Length; i++)
                        {
                            DataRow dLinha = dataSet1.Tables["Lisa_" + v.ToString()].NewRow();
                            dLinha["ID"] = id[i];
                            dLinha["LISA"] = iPinta[i];
                            dataSet1.Tables["Lisa_" + v.ToString()].Rows.Add(dLinha);
                        }
                        dataSet1.Tables.Add("Espalhamento_" + v.ToString());
                        dataSet1.Tables["Espalhamento_" + v.ToString()].Columns.Add("ID", Type.GetType("System.String"));
                        dataSet1.Tables["Espalhamento_" + v.ToString()].Columns.Add("X", Type.GetType("System.String"));
                        dataSet1.Tables["Espalhamento_" + v.ToString()].Columns.Add("Y", Type.GetType("System.String"));
                        double[,] dbEspalha = (double[,])mapaEspalhamento[v];
                        for (int i = 0; i < iPinta.Length; i++)
                        {
                            DataRow dLinha = dataSet1.Tables["Espalhamento_" + v.ToString()].NewRow();
                            dLinha["ID"] = id[i];
                            dLinha["X"] = dbEspalha[i, 0];
                            dLinha["Y"] = dbEspalha[i, 1];
                            dataSet1.Tables["Espalhamento_" + v.ToString()].Rows.Add(dLinha);
                        }
                    }
                }

                //Getis*
                if (mapaGetis != null) if (mapaGetis.Count > 0)
                {
                    for (int v = 0; v < mapaGetis.Count; v++)
                    {
                        dataSet1.Tables.Add("GetisAst_" + v.ToString());
                        dataSet1.Tables["GetisAst_" + v.ToString()].Columns.Add("ID", Type.GetType("System.String"));
                        dataSet1.Tables["GetisAst_" + v.ToString()].Columns.Add("GETIS_AST", Type.GetType("System.String"));
                        int[] iPinta = (int[])mapaGetis[v];
                        for (int i = 0; i < iPinta.Length; i++)
                        {
                            DataRow dLinha = dataSet1.Tables["GetisAst_" + v.ToString()].NewRow();
                            dLinha["ID"] = id[i];
                            dLinha["GETIS_AST"] = iPinta[i];
                            dataSet1.Tables["GetisAst_" + v.ToString()].Rows.Add(dLinha);
                        }
                    }
                }

                //Getis2 (Não asterisco)
                if (mapaGetis2 != null) if (mapaGetis2.Count > 0)
                {
                    for (int v = 0; v < mapaGetis2.Count; v++)
                    {
                        dataSet1.Tables.Add("Getis_" + v.ToString());
                        dataSet1.Tables["Getis_" + v.ToString()].Columns.Add("ID", Type.GetType("System.String"));
                        dataSet1.Tables["Getis_" + v.ToString()].Columns.Add("GETIS", Type.GetType("System.String"));
                        int[] iPinta = (int[])mapaGetis2[v];
                        for (int i = 0; i < iPinta.Length; i++)
                        {
                            DataRow dLinha = dataSet1.Tables["Getis_" + v.ToString()].NewRow();
                            dLinha["ID"] = id[i];
                            dLinha["GETIS"] = iPinta[i];
                            dataSet1.Tables["Getis_" + v.ToString()].Rows.Add(dLinha);
                        }
                    }
                }

                //Escore
                if (mapaEscore != null) if (mapaEscore.Count > 0)
                {
                    for (int v = 0; v < mapaEscore.Count; v++)
                    {
                        dataSet1.Tables.Add("Escore_" + v.ToString());
                        dataSet1.Tables["Escore_" + v.ToString()].Columns.Add("ID", Type.GetType("System.String"));
                        dataSet1.Tables["Escore_" + v.ToString()].Columns.Add("ESCORE", Type.GetType("System.String"));
                        int[] iPinta = (int[])mapaEscore[v];
                        for (int i = 0; i < iPinta.Length; i++)
                        {
                            DataRow dLinha = dataSet1.Tables["Escore_" + v.ToString()].NewRow();
                            dLinha["ID"] = id[i];
                            dLinha["ESCORE"] = iPinta[i];
                            dataSet1.Tables["Escore_" + v.ToString()].Rows.Add(dLinha);
                        }
                    }
                }
            }
            catch (Exception er)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO:http://www.codeproject.com/KB/graphics/iTextSharpTutorial.aspx
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                saveFileDialog.Filter = "PDF (*.pdf)|*.pdf"/*|RichTextbox (*.rtf)|*.rtf|Word (*.doc)|*.doc|Html (*.html)|*.html|All Files (*.*)|*.*"*/;
                string FileName = "";
                saveFileDialog.FileName = "";
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    FileName = saveFileDialog.FileName;
                    //TODO: Salvar a página em DOC, RTF ou HTML

                    bool eh_pdf = FileName.EndsWith(".pdf");
                    bool eh_html = FileName.EndsWith(".html");
                    bool eh_rtf = FileName.EndsWith(".rtf");

                    if (eh_pdf == true)
                    {
                        // Capturando os dados da regressao

                        bool regressao = modelFit;

                        if (regressao)
                        {
                            IpeaGeo.Classes.clsReportPDF.Regression reg = new Classes.clsReportPDF.Regression();
                            IpeaGeo.Classes.clsReportPDF pdf = new IpeaGeo.Classes.clsReportPDF();
                            
                            // Populate the Regression struct.
                            reg.pdfFile = FileName; reg.shape = endMapa; 
                            reg.sBetas = sbetas; reg.dBetas = dbetas; reg.desv = dev; 
                            reg.t = t; reg.pValue = p; reg.aic = aic; reg.bic = bic; 
                            reg.loglik = loglik; reg.sigma2 = sigma2; reg.rho = rho; 
                            reg.rhoT = rhoT; reg.rhoP = rhoP; reg.rhoDesv = rhoDev; 
                            reg.regressionMethod = regressionMethod; reg.mapVar = varMapa; 
                            reg.mapClasses = classesMapaReg; reg.mapColors = coresMapaReg; 
                            reg.finalShape = enderecoMapa; reg.database = enderecoBase;
                            reg.polygons = shapeCount; reg.mapMethod = metodo;

                            // Run the report method.
                            pdf.RelatorioPDF_Regressao(reg);
                            
                            // Retrieve the result.
                            FileName = reg.pdfFile; endMapa = reg.shape;
                            sbetas = reg.sBetas; dbetas = reg.dBetas; dev = reg.desv;
                            t = reg.t; p = reg.pValue; aic = reg.aic; bic = reg.bic;
                            loglik = reg.loglik; sigma2 = reg.sigma2; rho = reg.rho;
                            rhoT = reg.rhoT; rhoP = reg.rhoP; rhoDev = reg.rhoDesv;
                            regressionMethod = reg.regressionMethod; varMapa = reg.mapVar;
                            classesMapaReg = reg.mapClasses; coresMapaReg = reg.mapColors;
                            enderecoMapa = reg.finalShape; enderecoBase = reg.database;
                            shapeCount = reg.polygons; metodo = reg.mapMethod;
                        }
                        
                        if (isThematicMap && !isCluster)
                        {
                            IpeaGeo.Classes.clsReportPDF pdf = new IpeaGeo.Classes.clsReportPDF();
                            if (legendaMapaTematico != null)
                            {
                                pdf.RelatorioPDF_MapaTematico(FileName, enderecoBase, enderecoMapa, endMapa, shapeCount, metodo, classesMapaReg, coresMapaReg, varMapa, legendaMapaTematico);
                            }
                            else
                            {
                                pdf.RelatorioPDF_MapaTematico(FileName, enderecoBase, enderecoMapa, endMapa, shapeCount, metodo, classesMapaReg, coresMapaReg, varMapa);
                            }
                        }
                        
                        if (isCluster)
                        {
                            IpeaGeo.Classes.clsReportPDF pdf = new IpeaGeo.Classes.clsReportPDF();
                            pdf.RelatorioPDF_Conglomerados(FileName, enderecoBase, enderecoMapa, endMapa, numeroConglomerados, fatorMinkowski, shapeCount, metodo, distancia, isSpatialCluster, nomeVariaveisMapa, tipoVizinhanca, coresMapaReg, endCCC);
                        }
                        
                        if (isGlobalDependence)
                        {
                            IpeaGeo.Classes.clsReportPDF.GlobalDependence gd = new Classes.clsReportPDF.GlobalDependence();
                            IpeaGeo.Classes.clsReportPDF pdf = new IpeaGeo.Classes.clsReportPDF();
                            
                            // Populate the GlobalDependence struct.
                            gd.pdfFile = FileName; gd.database = enderecoBase;
                            gd.shape = enderecoMapa; gd.polygons = shapeCount;
                            gd.neighborhoodType = tipoVizinhanca;
                            gd.quantitativeSelectedVariables = variaveisSelecionadasQuant;
                            gd.populationVariables = variavelPopulacao;
                            gd.simulations = numeroSimulacoes; gd.weightKind = tipoDePeso;
                            gd.gearyIndex = geary; gd.gearyPValue = gearyP;
                            gd.getisIndex = getis; gd.getisPValue = getisP;
                            gd.moranIndex = moran; gd.moranPValue = moranP;
                            gd.simpleMoranIndex = moranSimp; gd.simpleMoranPValue = moranSimpP;
                            gd.rogersonIndex = rogerson; gd.rogersonPValue = rogersonP;
                            gd.tangoIndex = tango; gd.tangoPValue = tangoP;

                            // Call the report method.
                            pdf.RelatorioPDF_DependenciaGlobal(gd);
                            
                            // Retrieve the result.
                            FileName = gd.pdfFile; enderecoBase = gd.database;
                            enderecoMapa = gd.shape; shapeCount = gd.polygons;
                            tipoVizinhanca = gd.neighborhoodType; 
                            variaveisSelecionadasQuant = gd.quantitativeSelectedVariables;
                            variavelPopulacao = gd.populationVariables; 
                            numeroSimulacoes = gd.simulations; 
                            tipoDePeso = gd.weightKind; 
                            geary = gd.gearyIndex; gearyP = gd.gearyPValue;
                            getis = gd.getisIndex; getisP = gd.getisPValue;
                            moran = gd.moranIndex; moranP = gd.moranPValue;
                            moranSimp = gd.simpleMoranIndex;
                            moranSimpP = gd.simpleMoranPValue;
                            rogerson = gd.rogersonIndex; 
                            rogersonP = gd.rogersonPValue;
                            tango = gd.tangoIndex;
                            tangoP = gd.tangoPValue;
                        }
                        
                        if (isLocalDependence)
                        {
                            IpeaGeo.Classes.clsReportPDF pdf = new IpeaGeo.Classes.clsReportPDF();
                            pdf.RelatorioPDF_DependenciaLocal(FileName, enderecoBase, enderecoMapa, shapeCount, strMapaLisa, strMapaGetis, strMapaGetis2, strMapaEscore, strMapaEspalhamento, tipoVizinhanca, tipoCorrecao, confiabilidade, populacao, variaveisSelecionadasQuant, coresMapaReg);
                        }
                        
                        if (isScan)
                        {
                            IpeaGeo.Classes.clsReportPDF pdf = new IpeaGeo.Classes.clsReportPDF();
                            pdf.RelatorioPDF_Scan(FileName, enderecoBase, enderecoMapa, endMapa, shapeCount, metodo, pValor, coresMapaReg, variavelBase, variavelEvento, strNumeroSimulacoes, numeroPontosGrid, raioMaximo, raioMinimo, proporcaoMaxima, enderecoHistograma);
                        }
                        
                        if (isSegregation)
                        {
                            IpeaGeo.Classes.clsReportPDF pdf = new IpeaGeo.Classes.clsReportPDF();
                            pdf.RelatorioPDF_Segregacao(FileName, segregacaoValores, segregacaoIndices, segregacaoVariaveis);
                        }
                    }
                }
            }
            catch (Exception er)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.ShowPrintDialog();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.ShowPrintPreviewDialog();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                //! Screen coordinates
                Point ScreenCoord = new Point(MousePosition.X, MousePosition.Y);
                
                //! Browser coordinates
                Point BrowserCoord = webBrowser1.PointToClient(ScreenCoord);
                HtmlElement elem = webBrowser1.Document.GetElementFromPoint(BrowserCoord);

                //! Hide all menu items
                for (int i = 0; i < contextMenuStrip1.Items.Count; i++)
                {
                    contextMenuStrip1.Items[i].Visible = false;
                }

                //! Show what we want to see.
                switch (elem.TagName)
                {
                    case "A":
                        //! This is a link.. display the appropriate menu items
                        //openToolStripMenuItem.Visible = true;
                        //openInNewTabToolStripMenuItem.Visible = true;
                        //openInNewWindowToolStripMenuItem.Visible = true;
                        break;
                    case "IMG":
                        //! This is an image.. show our image menu items
                        string strCodigo = elem.OuterHtml;
                        int i_inicial = strCodigo.IndexOf("\"");
                        int i_final = strCodigo.IndexOf("\"", i_inicial + 1);
                        strImagem = strCodigo.Substring(i_inicial + 1, i_final - i_inicial - 1);

                        exportarDadosToolStripMenuItem.Visible = true;
                        break;
                    default:
                        //! This is anywhere else
                        //refreshToolStripMenuItem.Visible = true;
                        //viewSourceToolStripMenuItem.Visible = true;
                        break;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ExportaDadosLisa(string strVariavel)
        {
            try
            {
                FileInfo fInfo = new FileInfo(strImagem);
                string strImage = fInfo.Name;
                string[] cPos = new string[2] { "_", "." };
                int iPosicaoInicial = strVariavel.IndexOf(cPos[0]);
                int iPosicaoFinal = strVariavel.IndexOf(cPos[1]);

                string iVariaveis = strVariavel.Substring(iPosicaoInicial + 1, iPosicaoFinal - iPosicaoInicial - 1);

                DataSet dsTemp = dataSet1;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Salva dados LISA.";

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
                        exporta.exportToExcel(dsTemp.Tables["Lisa_" + iVariaveis], strFile);
                    }
                    else if (strExtensao == ".XML")
                    {
                        dsTemp.Tables["Lisa_" + iVariaveis].WriteXml(strFile);
                    }
                    else if (strExtensao == ".MDB")
                    {
                        //Cria o arquivo MDB
                        exporta.exportaToAccess(dsTemp.Tables["Lisa_" + iVariaveis], strFile, this.Name);
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void ExportaDadosEspalhamento(string strVariavel)
        {
            try
            {
                FileInfo fInfo = new FileInfo(strImagem);
                string strImage = fInfo.Name;
                string[] cPos = new string[2] { "_", "." };
                int iPosicaoInicial = strVariavel.IndexOf(cPos[0]);
                int iPosicaoFinal = strVariavel.IndexOf(cPos[1]);

                string iVariaveis = strVariavel.Substring(iPosicaoInicial + 1, iPosicaoFinal - iPosicaoInicial - 1);

                DataSet dsTemp = dataSet1;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Salva dados espalhamento.";

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
                        exporta.exportToExcel(dsTemp.Tables["Espalhamento_" + iVariaveis], strFile);
                    }
                    else if (strExtensao == ".XML")
                    {
                        dsTemp.Tables["Espalhamento_" + iVariaveis].WriteXml(strFile);
                    }
                    else if (strExtensao == ".MDB")
                    {
                        //Cria o arquivo MDB
                        exporta.exportaToAccess(dsTemp.Tables["Espalhamento_" + iVariaveis], strFile, this.Name);
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void ExportaDadosGetis(string strVariavel)
        {
            try
            {
                FileInfo fInfo = new FileInfo(strImagem);
                string strImage = fInfo.Name;
                string[] cPos = new string[2] { "_", "." };
                int iPosicaoInicial = strVariavel.IndexOf(cPos[0]);
                int iPosicaoFinal = strVariavel.IndexOf(cPos[1]);

                string iVariaveis = strVariavel.Substring(iPosicaoInicial + 1, iPosicaoFinal - iPosicaoInicial - 1);

                DataSet dsTemp = dataSet1;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Salva dados Getis.";

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
                        exporta.exportToExcel(dsTemp.Tables["Getis_" + iVariaveis], strFile);
                    }
                    else if (strExtensao == ".XML")
                    {
                        dsTemp.Tables["Getis_" + iVariaveis].WriteXml(strFile);
                    }
                    else if (strExtensao == ".MDB")
                    {
                        //Cria o arquivo MDB
                        exporta.exportaToAccess(dsTemp.Tables["Getis_" + iVariaveis], strFile, this.Name);
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void ExportaDadosEscore(string strVariavel)
        {
            try
            {
                FileInfo fInfo = new FileInfo(strImagem);
                string strImage = fInfo.Name;
                string[] cPos = new string[2] { "_", "." };
                int iPosicaoInicial = strVariavel.IndexOf(cPos[0]);
                int iPosicaoFinal = strVariavel.IndexOf(cPos[1]);

                string iVariaveis = strVariavel.Substring(iPosicaoInicial + 1, iPosicaoFinal - iPosicaoInicial - 1);

                DataSet dsTemp = dataSet1;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Salva dados Escore.";

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
                        exporta.exportToExcel(dsTemp.Tables["Escore_" + iVariaveis], strFile);
                    }
                    else if (strExtensao == ".XML")
                    {
                        dsTemp.Tables["Escore_" + iVariaveis].WriteXml(strFile);
                    }
                    else if (strExtensao == ".MDB")
                    {
                        //Cria o arquivo MDB
                        exporta.exportaToAccess(dsTemp.Tables["Escore_" + iVariaveis], strFile, this.Name);
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void exportarDadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo fInfo = new FileInfo(strImagem);
                string strImage = fInfo.Name;

                frmSpatialRegression frmreg = new frmSpatialRegression();

                DataTable residuos = frmreg.DataTable_Residuos;

                if (strImage == "Mapa.jpeg" && modelFit == false)
                {
                    DataSet dsTemp = dataSet1;
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
                            exporta.exportToExcel(dsTemp.Tables["Mapa"], strFile);
                        }
                        else if (strExtensao == ".XML")
                        {
                            dsTemp.Tables["Mapa"].WriteXml(strFile);
                        }
                        else if (strExtensao == ".MDB")
                        {
                            //Cria o arquivo MDB
                            exporta.exportaToAccess(dsTemp.Tables["Mapa"], strFile, this.Name);
                        }
                        Cursor.Current = Cursors.Default;
                    }
                }
                else if (strImage == "NumeroConglomerados.jpeg")
                {
                    DataSet dsTemp = dataSet1;
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
                            exporta.exportToExcel(dsTemp.Tables["NumeroClusters"], strFile);
                        }
                        else if (strExtensao == ".XML")
                        {
                            dsTemp.Tables["NumeroClusters"].WriteXml(strFile);
                        }
                        else if (strExtensao == ".MDB")
                        {
                            //Cria o arquivo MDB
                            exporta.exportaToAccess(dsTemp.Tables["NumeroClusters"], strFile, this.Name);
                        }
                        Cursor.Current = Cursors.Default;
                    }
                }
                else if (strImage == "Histograma.jpeg")
                {
                    DataSet dsTemp = dataSet1;
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
                            exporta.exportToExcel(dsTemp.Tables["Histograma"], strFile);
                        }
                        else if (strExtensao == ".XML")
                        {
                            dsTemp.Tables["Histograma"].WriteXml(strFile);
                        }
                        else if (strExtensao == ".MDB")
                        {
                            //Cria o arquivo MDB
                            exporta.exportaToAccess(dsTemp.Tables["Histograma"], strFile, this.Name);
                        }
                        Cursor.Current = Cursors.Default;
                    }
                }
                else if (strImage.Substring(0, 8) == "MapaLisa")
                {
                    ExportaDadosLisa(strImage);
                }
                else if (strImage.Substring(0, 8) == "Espalham")
                {
                    ExportaDadosEspalhamento(strImage);
                }
                else if (strImage.Substring(0, 8) == "MapaGeti")
                {
                    ExportaDadosGetis(strImage);
                }
                else if (strImage.Substring(0, 8) == "MapaEsco")
                {
                    ExportaDadosEscore(strImage);
                }

                //Caso queiramos exportar dados de regressao            
                //DataTable residuos = frmreg.DataTable_Residuos;

                if (modelFit == true)
                {
                    //DataSet dsTemp = dataSet1;
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

                        //TODO: E exportação para o excel portugues não entende as vírgulas.

                        if (strExtensao == ".XLS")
                        {
                            exporta.exportToExcel(residuals, strFile);
                        }
                        else if (strExtensao == ".XML")
                        {
                            residuals.WriteXml(strFile);
                        }
                        else if (strExtensao == ".MDB")
                        {
                            //Cria o arquivo MDB
                            exporta.exportaToAccess(residuals, strFile, this.Name);
                        }
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }
    }
}
