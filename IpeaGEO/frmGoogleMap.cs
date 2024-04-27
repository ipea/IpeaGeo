using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

namespace IpeaGEO
{
    public partial class frmGoogleMap : Form
    {
        public frmGoogleMap()
        {
            InitializeComponent();
        }

        private void frmGoogleMap_Load(object sender, EventArgs e)
        {

        }
        //Conexão MDB.
        private System.Data.OleDb.OleDbConnection m_cnn = new System.Data.OleDb.OleDbConnection();
        private System.Data.OleDb.OleDbDataAdapter m_dap = new System.Data.OleDb.OleDbDataAdapter();

        private string EndedrecoBase = "";
        private string strExtensao = "";


        private void openToolStripButton_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Excel (*.xls)|*.xls|Access (*.mdb)|*.mdb|SAS (*.sas7bdat)|*.sas7bdat|XML (*.xml)|*.xml";
            string FileName = "";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                //Extensão
                FileName = openFileDialog.FileName;
                strExtensao = Path.GetExtension(FileName).ToUpper();
                EndedrecoBase=FileName;
                txtBase.Text = FileName;

                txtBase.Enabled = true;
                saveToolStripButton.Enabled = true;

                if (strExtensao == ".MDB")
                {
                    
                    //Inicia conexão com o MDB
                    string m_cnnstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName;
                    m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
                    m_cnn.Open();
                    string[] restrictions = new string[4];
                    restrictions[3] = "Table";

                    //Mostra as tabelas de usuario.
                    DataTable userTables = null;
                    userTables = m_cnn.GetSchema("Tables", restrictions);
                    cmbTabela.Items.Clear();
                    for (int i = 0; i < userTables.Rows.Count; i++)
                    {
                        cmbTabela.Items.Add(userTables.Rows[i][2].ToString());
                    }
                    cmbTabela.Enabled = true;
                }
                else if (strExtensao == ".SAS")
                {
                    cmbTabela.Enabled = false;
                    //Endereço da pasta com o arquivo
                    string strPath = Path.GetDirectoryName(EndedrecoBase);
                    //Nome do arquivo
                    string strAqruivo = Path.GetFileNameWithoutExtension(EndedrecoBase);
                    //String de conexão
                    string m_cnnstring = "Provider=sas.LocalProvider; Data Source=" + strPath;
                    m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
                    m_cnn.Open();
                    OleDbCommand sasCommand = m_cnn.CreateCommand();
                    sasCommand.CommandType = CommandType.TableDirect;
                    sasCommand.CommandText = strAqruivo;
                    m_dap = new System.Data.OleDb.OleDbDataAdapter(sasCommand);

                    //Guarda no DataSet
                    m_dap.Fill(dsDados, "Table1");
                    m_cnn.Close();

                    //Variaveis dos dados
                    cmbVariavel.Items.Clear();
                    for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                    {
                        cmbVariavel.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                    }

                    cmbVariavel.Enabled = true;
                }
                else if (strExtensao == ".SAS")
                {
                    cmbTabela.Enabled = false;
                    //Endereço da pasta com o arquivo
                    string strPath = Path.GetDirectoryName(EndedrecoBase);
                    //Nome do arquivo
                    string strAqruivo = Path.GetFileNameWithoutExtension(EndedrecoBase);
                    //String de conexão
                    string m_cnnstring = "Provider=sas.LocalProvider; Data Source=" + strPath;
                    m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
                    m_cnn.Open();
                    OleDbCommand sasCommand = m_cnn.CreateCommand();
                    sasCommand.CommandType = CommandType.TableDirect;
                    sasCommand.CommandText = strAqruivo;
                    m_dap = new System.Data.OleDb.OleDbDataAdapter(sasCommand);

                    //Guarda no DataSet
                    m_dap.Fill(dsDados, "Table1");
                    m_cnn.Close();

                    //Variaveis dos dados
                    cmbVariavel.Items.Clear();
                    for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                    {
                        cmbVariavel.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                    }

                    cmbVariavel.Enabled = true;
                }
                else if (strExtensao == ".XLS")
                {
                    label2.Text = "Planilha";
                     //Inicia conexão com o MDB
                    string m_cnnstring = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + EndedrecoBase + ";Extended Properties=Excel 8.0;";
                    m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
                    m_cnn.Open();
                    System.Data.DataTable dt = null;

                    dt = m_cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string[] restrictions = new string[dt.Rows.Count];

                    foreach (DataRow row in dt.Rows)
                    {
                        cmbTabela.Items.Add(row["TABLE_NAME"].ToString());
                    }
                    cmbTabela.Enabled = true;
                }
                else if (strExtensao == ".XML")
                {
                    label2.Text = "";

                    cmbTabela.Enabled = false;

                    //Le arquivo
                    dsDados.ReadXml(EndedrecoBase);

                   //Variaveis dos dados
                    cmbVariavel.Items.Clear();
                    for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                    {
                        cmbVariavel.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                    }

                    cmbVariavel.Enabled = true;
                }

                Cursor.Current = Cursors.Default;

            }
        }

        private void cmbTabela_SelectedIndexChanged(object sender, EventArgs e)
        {
            dsDados.Tables[0].Clear();
            if (strExtensao == ".MDB")
            {
                m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM " + this.cmbTabela.SelectedItem.ToString(), m_cnn);
                m_dap.Fill(dsDados, "Table1");
                cmbVariavel.Items.Clear();

                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                {
                    cmbVariavel.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                }
                cmbVariavel.Enabled = true;
                m_cnn.Close();
            }
            else if (strExtensao == ".XLS")
            {
                m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + this.cmbTabela.SelectedItem.ToString() + "]", m_cnn);
                m_dap.Fill(dsDados, "Table1");
                cmbVariavel.Items.Clear();

                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                {
                    cmbVariavel.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                }
                cmbVariavel.Enabled = true;
                m_cnn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            dsDados.Tables[0].Columns.Add("Latitude");
            dsDados.Tables[0].Columns.Add("Longitude");
            progressBar1.Minimum = 0;
            progressBar1.Maximum = dsDados.Tables[0].Rows.Count;

            for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
            {
                bool conecta = false;
                int iContador = 0;
                string strEndereco = dsDados.Tables[0].Rows[i][cmbVariavel.SelectedItem.ToString()].ToString();
                string link = "http://maps.google.com/maps?output=setprefs&near=" + strEndereco + ",+Brazil";
                string lat = "";
                string lng = "";
                do
                {
                    System.Net.WebClient client = new System.Net.WebClient();
                    string page = client.DownloadString(link);
                    if (page != "" && client.IsBusy == false) conecta = true;
                    
                    int begin = page.IndexOf("viewport:{center:{");
                    string str = "";
                    if (begin > -1)
                    {
                        str = page.Substring(begin);
                        int end = str.IndexOf("},span:");
                        if (end > -1)
                        {
                            str = str.Substring(0, end);

                            //Tamanho da Latitude
                            int iLat0 = str.IndexOf(",");
                            int iLat1 = str.IndexOf("lat:");
                            lat = str.Substring(iLat1+4, iLat0-iLat1-4);

                            //Tamanho da Longitude
                            lng = str.Substring(str.IndexOf(",lng:") + 5);
                        }
                    }
                    iContador++;
                } while (conecta == false && iContador < (int)numericUpDown1.Value);

                //Salva os dados
                dsDados.Tables[0].Rows[i]["Latitude"] = lat;
                dsDados.Tables[0].Rows[i]["Longitude"] = lng;
                progressBar1.Increment(1);
                Application.DoEvents();
            }

            dataGridView1.DataSource = dsDados.Tables[0];
            dataGridView1.Refresh();
            Cursor.Current = Cursors.Default;

        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            DataSet dsTemp = dsDados;
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

        private void newToolStripButton_Click(object sender, EventArgs e)
        {

        }
    }
}
