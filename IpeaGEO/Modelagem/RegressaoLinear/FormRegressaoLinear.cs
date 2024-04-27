using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Data.OleDb;
using System.Threading;

namespace IpeaGEO.Modelagem
{
    public partial class FormRegressaoLinear : Form
    {
        #region Variáveis internas

        private DataTable m_dt_tabela_dados = new DataTable();
        public DataTable Dados
        {
            get { return m_dt_tabela_dados; }
            set { m_dt_tabela_dados = value; }
        }

        private clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            get { return m_shape; }
            set { m_shape = value; }
        }

        #endregion

        public FormRegressaoLinear()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }

        private void FormBaseModelagem_Load(object sender, EventArgs e)
        {
            try
            {
                this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = false;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void grbImportacaoDosArquivos_Enter(object sender, EventArgs e)
        {

        }

        private void AtualizaTabelaDados()
        {
            this.dataGridView1.DataSource = m_dt_tabela_dados;

            clsUtilTools clt = new clsUtilTools();

            string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);
            
            this.userControlRegressaoInstrumentos1.VariaveisDB = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
            this.userControlRegressaoInstrumentos1.VariaveisList = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
        }

        #region Open tabela de dados

        //Conexão MDB.
        private System.Data.OleDb.OleDbConnection m_cnn = new System.Data.OleDb.OleDbConnection();
        private System.Data.OleDb.OleDbDataAdapter m_dap = new System.Data.OleDb.OleDbDataAdapter();
        private string strExtensao = "";
        private string strEnderecoBase = "";
        private DataSet dsDados = new DataSet();

        private void AbrirTabelaDados()
        {
            try
            {
                //Formulário Pai .sas7bdat
                //m_mdiparent = this.MdiParent;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                openFileDialog.Filter = "Excel (*.xls)|*.xls|Access (*.mdb)|*.mdb|SAS (*.sas7bdat)|*.sas7bdat|XML (*.xml)|*.xml";
                string FileName = "";
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {

                    FileName = openFileDialog.FileName;
                    strExtensao = Path.GetExtension(FileName).ToUpper();
                    strEnderecoBase = FileName;

                    if (strExtensao == ".XLS")
                    {
                        //Inicia conexão com o OLEDB
                        string m_cnnstring = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strEnderecoBase + ";Extended Properties=Excel 8.0;";
                        m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
                        m_cnn.Open();
                        System.Data.DataTable dt = null;

                        dt = m_cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        //Eliminando linhas duplicadas com cifrão no final                    
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            string nome_celula = Convert.ToString(dt.Rows[i]["TABLE_NAME"]);
                            int indice_cifrao = nome_celula.IndexOf("$");
                            if (indice_cifrao == -1)
                            {
                                dt.Rows.RemoveAt(i);
                            }
                        }

                        //string[] restrictions = new string[dt.Rows.Count];
                        this.cmbTabelasNoArquivo.Items.Clear();
                        foreach (DataRow row in dt.Rows)
                        {
                            this.cmbTabelasNoArquivo.Items.Add(row["TABLE_NAME"].ToString());
                        }
                        if (this.cmbTabelasNoArquivo.Items.Count > 0)
                        {
                            toolStripMenuItem8.Enabled = true;
                            this.btnImportarTabela.Enabled = true;
                            this.cmbTabelasNoArquivo.SelectedIndex = 0;
                        }
                        else
                        {
                            this.btnImportarTabela.Enabled = false;
                        }
                    }

                    //Importação em SAS
                    else if (strExtensao == ".SAS7BDAT")
                    {
                        this.cmbTabelasNoArquivo.Items.Add("Base SAS");
                        this.cmbTabelasNoArquivo.SelectedIndex = 0;
                        this.btnImportarTabela.Enabled = true;
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAbrirTabelaDados_Click(object sender, EventArgs e)
        {
            this.AbrirTabelaDados();
        }

        private void ImportarTabela()
        {
            try
            {
                if (this.cmbTabelasNoArquivo.Items.Count <= 0)
                {
                    throw new Exception("Selecione um arquivo de dados.");
                }

                //dsDados.Clear();
                dsDados.Reset();
                if (strExtensao == ".MDB")
                {
                    m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM " + this.cmbTabelasNoArquivo.SelectedItem.ToString(), m_cnn);
                    m_dap.Fill(dsDados, "Table1");

                    //cmbIDdados.Items.Clear();
                    //for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                    //{
                    //    cmbIDdados.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                    //}
                    //cmbIDdados.Enabled = true;
                    //cmbIDmapa.Enabled = true;

                    DataTable dt_dados = dsDados.Tables["Table1"];

                    m_cnn.Close();
                }
                else if (strExtensao == ".XLS")
                {
                    m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + this.cmbTabelasNoArquivo.SelectedItem.ToString() + "]", m_cnn);
                    m_dap.Fill(dsDados, "Table1");

                    //cmbIDdados.Items.Clear();
                    //for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                    //{
                    //    cmbIDdados.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                    //}
                    //cmbIDdados.Enabled = true;
                    //cmbIDmapa.Enabled = true;

                    m_cnn.Close();

                    m_dt_tabela_dados = dsDados.Tables["Table1"];

                    AtualizaTabelaDados();
                }
                else if (strExtensao == ".SAS7BDAT")
                {
                    //label2.Text = "";
                    //Endereço da pasta com o arquivo
                    string strPath = Path.GetDirectoryName(strEnderecoBase);
                    //Nome do arquivo
                    string strAqruivo = Path.GetFileNameWithoutExtension(strEnderecoBase);
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

                    m_dt_tabela_dados = dsDados.Tables["Table1"];

                    AtualizaTabelaDados();
                }

                //this.btnEstimarModelo.Enabled =
                //this.btnEstimarModelo1.Enabled = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Importação do arquivo de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImportarTabela_Click(object sender, EventArgs e)
        {
            ImportarTabela();
        }

        #endregion

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                this.EstimaModelo();
                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Regressão Linear", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EstimaModelo()
        {
            clsUtilTools clt = new clsUtilTools();

            #region checando valores inválidos

            int[] indicadores_val_invalidos;

            for (int i = 0; i < userControlRegressaoInstrumentos1.VariavelDependente.GetLength(0); i++)
            {
                if (clt.ChecaValoresDoubleInvalidos(this.m_dt_tabela_dados, userControlRegressaoInstrumentos1.VariavelDependente[i], out indicadores_val_invalidos))
                {
                    MessageBox.Show("Há valores double inválidos na variável " + userControlRegressaoInstrumentos1.VariavelDependente[i] + ". Cheque a sua base de dados.");
                    return;
                }
            }

            for (int i = 0; i < userControlRegressaoInstrumentos1.VariaveisIndependentes.GetLength(0); i++)
            {
                if (clt.ChecaValoresDoubleInvalidos(this.m_dt_tabela_dados, userControlRegressaoInstrumentos1.VariaveisIndependentes[i], out indicadores_val_invalidos))
                {
                    MessageBox.Show("Há valores double inválidos na variável " + userControlRegressaoInstrumentos1.VariaveisIndependentes[i] + ". Cheque a sua base de dados.");
                    return;
                }
            }

            #endregion

            BLogicRegressaoLinear Cam1 = new BLogicRegressaoLinear();
            Cam1.VariaveisDependentes = userControlRegressaoInstrumentos1.VariavelDependente;
            Cam1.VariaveisIndependentes = userControlRegressaoInstrumentos1.VariaveisIndependentes;
            Cam1.TabelaDados = this.m_dt_tabela_dados;

            Cam1.EstimarModeloRegressao();

            this.userControlRichTextOutput1.Texto = Cam1.ResultadoEstimacao;
        }
    }
}
