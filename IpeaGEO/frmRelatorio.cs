using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace IpeaGEO
{
    public partial class frmRelatorio : Form
    {
        public frmRelatorio()
        {
            InitializeComponent();
        }

        private string strImagem = "";

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


        private string strHTML;
        public string CodigoHTML
        {
            get
            {
                return strHTML;
            }
            set
            {
                strHTML = value;
            }
        }

        private string[,] strMapa;
        public string[,] VariaveisMapa
        {
            get
            {
                return strMapa;
            }
            set
            {
                strMapa = value;
            }
        }

        private DataSet dsGraficos;
        public DataSet DadosGraficos
        {
            get
            {
                return dsGraficos;
            }
        }
        private DataTable dsTable;
        public DataTable TabelaDados
        {
            get
            {
                return dsTable;
            }
        }

        private double[] dblHistogramaX;
        public double[] HistogramaX
        {
            get
            {
                return dblHistogramaX;
            }
            set
            {
                dblHistogramaX = value;
            }
        }
        private double[] dblHistogramaY;
        public double[] HistogramaY
        {
            get
            {
                return dblHistogramaY;
            }
            set
            {
                dblHistogramaY = value;
            }
        }

        private string[] arID;
        public string[] ID
        {
            get
            {
                return arID;
            }
            set
            {
                arID = value;
            }
        }
        private ArrayList dblEspalha;
        public ArrayList Espalhamento
        {
            get
            {
                return dblEspalha;
            }
            set
            {
                dblEspalha = value;
            }
        }

        private ArrayList arMapaLisa;
        public ArrayList MapaLisa
        {
            get
            {
                return arMapaLisa;
            }
            set
            {
                arMapaLisa = value;
            }
        }

        private ArrayList arMapaGetis;
        public ArrayList MapaGetis
        {
            get
            {
                return arMapaGetis;
            }
            set
            {
                arMapaGetis = value;
            }
        }

        private ArrayList arMapaEscore;
        public ArrayList MapaEscore
        {
            get
            {
                return arMapaEscore;
            }
            set
            {
                arMapaEscore = value;
            }
        }
        
        private void frmRelatorio_Load(object sender, EventArgs e)
        {
            this.webBrowser1.DocumentText = strHTML;
            this.webBrowser1.Refresh();
            this.webBrowser1.ContextMenuStrip = contextMenuStrip1;    //! Set our ContextMenuStrip
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;  //! Disable the default IE ContextMenu
           
            //Limpa o DataSet
            dataSet1.Clear();

            //Guarda os dados
            dataSet1.Tables.Add("Mapa");
            if (strMapa != null)
            {
                dataSet1.Tables["Mapa"].Columns.Add(strMapa[0, 0], Type.GetType("System.String"));
                dataSet1.Tables["Mapa"].Columns.Add(strMapa[0, 1], Type.GetType("System.String"));

                for (int i = 1; i < strMapa.GetLength(0); i++)
                {
                    DataRow dLinha = dataSet1.Tables["Mapa"].NewRow();
                    dLinha[strMapa[0, 0]] = strMapa[i, 0];
                    dLinha[strMapa[0, 1]] = strMapa[i, 1];
                    dataSet1.Tables["Mapa"].Rows.Add(dLinha);
                }
            }
            if (dblPseudoF != null)
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

                int iConta = dblPseudoF.Length - 1;
                for (int i = 0; i < dblPseudoF.Length; i++)
                {
                    DataRow dLinha = dataSet1.Tables["NumeroClusters"].NewRow();
                    dLinha["NumeroConglomerados"] = (i+2).ToString();
                    dLinha["PseudoF"] = dblPseudoF[iConta].ToString();
                    dLinha["PseudoT"] = dblPseudoT[iConta].ToString();
                    dLinha["CCC"] = dblCCC[iConta].ToString();
                    dLinha["RSquare"] = dblRSquare[iConta].ToString();
                    dLinha["RSquareExpected"] = dblRSquareExpected[iConta].ToString();
                    dLinha["RSquarePartial"] = dblRSquarePartial[iConta].ToString();
                    dataSet1.Tables["NumeroClusters"].Rows.Add(dLinha);
                    iConta--;
                }
            }
            if (dblHistogramaX != null)
            {
                dataSet1.Tables.Add("Histograma");
                dataSet1.Tables["Histograma"].Columns.Add("X", Type.GetType("System.String"));
                dataSet1.Tables["Histograma"].Columns.Add("Y", Type.GetType("System.String"));

                for (int i = 0; i < dblHistogramaX.Length; i++)
                {
                    DataRow dLinha = dataSet1.Tables["Histograma"].NewRow();
                    dLinha["X"] = dblHistogramaX[i].ToString();
                    dLinha["Y"] = dblHistogramaY[i].ToString();
                    dataSet1.Tables["Histograma"].Rows.Add(dLinha);
                }
            }
            if(arMapaLisa!=null) if (arMapaLisa.Count>0)
            {
                for (int v = 0; v < arMapaLisa.Count; v++)
                {
                    dataSet1.Tables.Add("Lisa_" + v.ToString());
                    dataSet1.Tables["Lisa_" + v.ToString()].Columns.Add("ID", Type.GetType("System.String"));
                    dataSet1.Tables["Lisa_" + v.ToString()].Columns.Add("LISA", Type.GetType("System.String"));
                    int[] iPinta = (int[])arMapaLisa[v];
                    for (int i = 0; i < iPinta.Length; i++)
                    {
                        DataRow dLinha = dataSet1.Tables["Lisa_" + v.ToString()].NewRow();
                        dLinha["ID"] = arID[i];
                        dLinha["LISA"] = iPinta[i];
                        dataSet1.Tables["Lisa_" + v.ToString()].Rows.Add(dLinha);
                    }
                    dataSet1.Tables.Add("Espalhamento_" + v.ToString());
                    dataSet1.Tables["Espalhamento_" + v.ToString()].Columns.Add("ID", Type.GetType("System.String"));
                    dataSet1.Tables["Espalhamento_" + v.ToString()].Columns.Add("X", Type.GetType("System.String"));
                    dataSet1.Tables["Espalhamento_" + v.ToString()].Columns.Add("Y", Type.GetType("System.String"));
                    double[,] dbEspalha = (double[,])dblEspalha[v];
                    for (int i = 0; i < iPinta.Length; i++)
                    {
                        DataRow dLinha = dataSet1.Tables["Espalhamento_" + v.ToString()].NewRow();
                        dLinha["ID"] = arID[i];
                        dLinha["X"] = dbEspalha[i, 0];
                        dLinha["Y"] = dbEspalha[i, 1];
                        dataSet1.Tables["Espalhamento_" + v.ToString()].Rows.Add(dLinha);
                    }


                }
            }
            if (arMapaGetis != null) if (arMapaGetis.Count > 0 )
            {
                for (int v = 0; v < arMapaLisa.Count; v++)
                {
                    dataSet1.Tables.Add("Getis_" + v.ToString());
                    dataSet1.Tables["Getis_" + v.ToString()].Columns.Add("ID", Type.GetType("System.String"));
                    dataSet1.Tables["Getis_" + v.ToString()].Columns.Add("GETIS", Type.GetType("System.String"));
                    int[] iPinta = (int[])arMapaGetis[v];
                    for (int i = 0; i < iPinta.Length; i++)
                    {
                        DataRow dLinha = dataSet1.Tables["Getis_" + v.ToString()].NewRow();
                        dLinha["ID"] = arID[i];
                        dLinha["GETIS"] = iPinta[i];
                        dataSet1.Tables["Getis_" + v.ToString()].Rows.Add(dLinha);
                    }
                }
            }
            if (arMapaEscore != null) if (arMapaEscore.Count > 0)
            {
                for (int v = 0; v < arMapaLisa.Count; v++)
                {
                    dataSet1.Tables.Add("Escore_" + v.ToString());
                    dataSet1.Tables["Escore_" + v.ToString()].Columns.Add("ID", Type.GetType("System.String"));
                    dataSet1.Tables["Escore_" + v.ToString()].Columns.Add("ESCORE", Type.GetType("System.String"));
                    int[] iPinta = (int[])arMapaEscore[v];
                    for (int i = 0; i < iPinta.Length; i++)
                    {
                        DataRow dLinha = dataSet1.Tables["Escore_" + v.ToString()].NewRow();
                        dLinha["ID"] = arID[i];
                        dLinha["ESCORE"] = iPinta[i];
                        dataSet1.Tables["Escore_" + v.ToString()].Rows.Add(dLinha);
                    }
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            //http://www.gutgames.com/post/HTML-to-PDF.aspx
            //TODO:http://www.codeproject.com/KB/graphics/iTextSharpTutorial.aspx
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "RichTextbox (*.rtf)|*.rtf|Word (*.doc)|*.doc|Html (*.html)|*.html|All Files (*.*)|*.*";
            string FileName = "";
            saveFileDialog.FileName = this.Name;
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                FileName = saveFileDialog.FileName;
                //TODO: Salvar a página em DOC, RTF ou HTML
                if (FileName == "Html")
                {

                }
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
             webBrowser1.ShowPrintPreviewDialog();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void mapaToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void gráficoToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
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
                    strImagem = strCodigo.Substring(i_inicial+1, i_final - i_inicial-1); 

                    exportarDadosToolStripMenuItem.Visible = true;
                    break;
                default:
                    //! This is anywhere else
                    //refreshToolStripMenuItem.Visible = true;
                    //viewSourceToolStripMenuItem.Visible = true;
                    break;
            }
        }

        private void ExportaDadosLisa(string strVariavel)
        {
            FileInfo fInfo = new FileInfo(strImagem);
            string strImage = fInfo.Name;
            string[] cPos = new string[2] { "_", "." };
            int iPosicaoInicial = strVariavel.IndexOf(cPos[0]);
            int iPosicaoFinal = strVariavel.IndexOf(cPos[1]);

            string iVariaveis = strVariavel.Substring(iPosicaoInicial+1, iPosicaoFinal - iPosicaoInicial-1);
      
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
        private void ExportaDadosEspalhamento(string strVariavel)
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
        private void ExportaDadosGetis(string strVariavel)
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
        private void ExportaDadosEscore(string strVariavel)
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
        private void exportarDadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo fInfo = new FileInfo(strImagem);
            string strImage = fInfo.Name;

            if (strImage == "Mapa.jpeg")
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

            

    
        }
    }
}
