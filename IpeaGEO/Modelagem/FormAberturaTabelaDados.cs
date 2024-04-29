using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo
{
    public partial class FormAberturaTabelaDados : Form
    {
        #region inicialização do formulário

        public FormAberturaTabelaDados()
        {
            InitializeComponent();
        }

        private void FormAberturaTabelaDados_Load(object sender, EventArgs e)
        {
            try
            {
                System.Globalization.NumberFormatInfo nfi = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
                if (nfi.NumberDecimalSeparator == ",")
                {
                    this.ckbFormatoNumeroPortugues.Checked = true;
                }
                else
                {
                    this.ckbFormatoNumeroPortugues.Checked = false;
                }

                this.btnImportarTabela.Enabled = false;

                this.txtCaracterDelimitacao.Enabled = false;

                this.cmbTabelasNoArquivo.Enabled = true;

                this.grbArquivosTXT.Enabled = false;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region variáveis internas

        private DataTable m_dt_dados = new DataTable();
        public DataTable TabelaDeDados
        {
            get { return this.m_dt_dados; }
        }

        private string m_file_name = "";

        //============ Conexão MDB =====================================================================//
        private System.Data.OleDb.OleDbConnection m_cnn = new System.Data.OleDb.OleDbConnection();
        private System.Data.OleDb.OleDbDataAdapter m_dap = new System.Data.OleDb.OleDbDataAdapter();
        private string strExtensao = "";
        private string strEnderecoBase = "";
        private DataSet dsDados = new DataSet();

        private string m_nome_arquivo_importado = "";
        private string m_path_arquivo_importado = "";

        public string NomeArquivoImportado
        {
            get { return m_nome_arquivo_importado; }
        }

        public string PathArquivoImportado
        {
            get { return m_path_arquivo_importado; }
        }

        #endregion

        #region função AbrirTabelaDados

        private void AbrirTabelaDados()
        {
            try
            {
                this.lblArquivoDeDados.Text = "";

                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (rdbArquivosSAS.Checked)
                {
                    openFileDialog.Filter = "SAS (*.sas7bdat)|*.sas7bdat";
                }
                else if (rdbArquivosExcelMDB.Checked)
                {
                    openFileDialog.Filter = "Excel (*.xls)|*.xls|Excel (*.xlsx)|*.xlsx|Access (*.mdb)|*.mdb";
                }
                else
                {
                    openFileDialog.Filter = "Delimited File (*.txt)|*.txt|Delimited File (*.dat)|*.dat|Comma Separated (*.csv)|*.csv";
                }

                string FileName = "";
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    FileName = openFileDialog.FileName;
                    m_file_name = FileName;
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
                            this.btnImportarTabela.Enabled = true;
                            this.cmbTabelasNoArquivo.SelectedIndex = 0;
                        }
                        else
                        {
                            this.btnImportarTabela.Enabled = false;
                        }
                    }

                    if (strExtensao == ".XLSX")
                    {
                        //Inicia conexão com o OLEDB
                        string m_cnnstring = "Provider=Microsoft.ace.OLEDB.12.0;" + "Data Source=" + strEnderecoBase + ";Extended Properties=Excel 12.0 Xml;";
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
                            this.btnImportarTabela.Enabled = true;
                            this.cmbTabelasNoArquivo.SelectedIndex = 0;
                        }
                        else
                        {
                            this.btnImportarTabela.Enabled = false;
                        }
                    }

                    if (strExtensao == ".TXT" || strExtensao == ".DAT" || strExtensao == ".CSV")
                    {
                        ckbDelimitadoTab.Enabled =
                            ckbDelimitadoVirgula.Enabled =
                            ckbNomesPrimeiraLinha.Enabled =
                            ckbDelimitadoPontoVirgula.Enabled =
                            ckbDelimitadoCaracter.Enabled =
                            this.ckbFormatoNumeroPortugues.Enabled = true;

                        if (strExtensao == ".TXT" || strExtensao == ".DAT")
                        {
                            ckbDelimitadoTab.Checked = true;
                            ckbNomesPrimeiraLinha.Checked = true;
                        }

                        if (strExtensao == ".CSV")
                        {
                            this.ckbDelimitadoPontoVirgula.Checked = true;
                            ckbNomesPrimeiraLinha.Checked = true;
                        }

                        //ckbFormatoNumeroPortugues.Checked = false;

                        this.btnImportarTabela.Enabled = true;
                    }
                    else
                    {
                        ckbDelimitadoTab.Enabled =
                            ckbDelimitadoVirgula.Enabled =
                            ckbNomesPrimeiraLinha.Enabled =
                            ckbDelimitadoPontoVirgula.Enabled =
                            ckbDelimitadoCaracter.Enabled = 
                            this.ckbFormatoNumeroPortugues.Enabled = false;

                        this.btnImportarTabela.Enabled = true;
                        //ckbFormatoNumeroPortugues.Checked = false;
                    }

                    if (strExtensao == ".SAS7BDAT")
                    {
                        //txtEndereco.Text = strEnderecoBase;
                        //label2.Text = "";
                        cmbTabelasNoArquivo.Enabled = false;
                        //Endereço da pasta com o arquivo
                        string strPath = Path.GetDirectoryName(strEnderecoBase);
                        //Nome do arquivo
                        string strAqruivo = Path.GetFileNameWithoutExtension(strEnderecoBase);
                        //String de conexão
                        string m_cnnstring = "Provider=sas.LocalProvider; Data Source=" + strPath;
                        m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
                        m_cnn.Open();

                        System.Data.DataTable dt = null;

                        dt = m_cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        //string[] restrictions = new string[dt.Rows.Count];
                        this.cmbTabelasNoArquivo.Items.Clear();
                        foreach (DataRow row in dt.Rows)
                        {
                            this.cmbTabelasNoArquivo.Items.Add(row["TABLE_NAME"].ToString());
                        }
                        if (this.cmbTabelasNoArquivo.Items.Count > 0)
                        {
                            this.btnImportarTabela.Enabled = true;
                            this.cmbTabelasNoArquivo.SelectedIndex = 0;
                        }
                        else
                        {
                            this.btnImportarTabela.Enabled = false;
                        }

                        m_cnn.Close();
                    }

                    if (strExtensao == ".MDB")
                    {
                        //Inicia conexão com o MDB
                        string m_cnnstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strEnderecoBase;
                        m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
                        m_cnn.Open();
                        string[] restrictions = new string[4];
                        restrictions[3] = "Table";

                        //Mostra as tabelas de usuario.
                        DataTable userTables = null;
                        userTables = m_cnn.GetSchema("Tables", restrictions);
                        cmbTabelasNoArquivo.Items.Clear();
                        for (int i = 0; i < userTables.Rows.Count; i++)
                        {
                            cmbTabelasNoArquivo.Items.Add(userTables.Rows[i][2].ToString());
                        }

                        System.Data.DataTable dt = null;
                        dt = m_cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        //string[] restrictions = new string[dt.Rows.Count];
                        this.cmbTabelasNoArquivo.Items.Clear();
                        foreach (DataRow row in dt.Rows)
                        {
                            this.cmbTabelasNoArquivo.Items.Add(row["TABLE_NAME"].ToString());
                        }
                        if (this.cmbTabelasNoArquivo.Items.Count > 0)
                        {
                            this.btnImportarTabela.Enabled = true;
                            this.cmbTabelasNoArquivo.SelectedIndex = 0;
                        }
                        else
                        {
                            this.btnImportarTabela.Enabled = false;
                        }
                    }

                    this.lblArquivoDeDados.Text = FileName;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region função ImportarTabela

        private void ImportarTabela()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (this.cmbTabelasNoArquivo.Items.Count <= 0 && strExtensao != ".DAT" && strExtensao != ".TXT" && strExtensao != ".CSV" && strExtensao != ".XLSX")
                {
                    throw new Exception("Selecione um arquivo de dados.");
                }

                dsDados.Reset();

                if (strExtensao == ".SAS7BDAT")
                {
                    string strAqruivo = Path.GetFileNameWithoutExtension(strEnderecoBase);
                    OleDbCommand sasCommand = m_cnn.CreateCommand();
                    sasCommand.CommandType = CommandType.TableDirect;
                    sasCommand.CommandText = strAqruivo;
                    m_dap = new System.Data.OleDb.OleDbDataAdapter(sasCommand);
                    //Guarda no DataSet
                    m_dap.Fill(dsDados, "Table1");

                    m_cnn.Close();

                    m_dt_dados = dsDados.Tables["Table1"];
                }

                if (strExtensao == ".MDB")
                {
                    m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM " + this.cmbTabelasNoArquivo.SelectedItem.ToString(), m_cnn);
                    m_cnn.Close();

                    m_dt_dados = dsDados.Tables["Table1"];
                }

                if (strExtensao == ".TXT" || strExtensao == ".DAT" || strExtensao == ".CSV")
                {
                    DataTable dt_saida = new DataTable();

                    clsUtilTools clt = new clsUtilTools();

                    clt.LerArquivoTextoDelimited(strEnderecoBase, ref dt_saida, ckbDelimitadoPontoVirgula.Checked,
                        ckbDelimitadoVirgula.Checked, ckbDelimitadoCaracter.Checked, txtCaracterDelimitacao.Text, 
                        ckbNomesPrimeiraLinha.Checked, ckbFormatoNumeroPortugues.Checked);

                    m_dt_dados = dt_saida;
                }

                if (strExtensao == ".XLS")
                {
                    m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + this.cmbTabelasNoArquivo.SelectedItem.ToString() + "]", m_cnn);
                    m_dap.Fill(dsDados, "Table1");

                    m_cnn.Close();

                    m_dt_dados = dsDados.Tables["Table1"];
                }

                else if (strExtensao == ".XLSX")
                {
                    m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + this.cmbTabelasNoArquivo.SelectedItem.ToString() + "]", m_cnn);
                    m_dap.Fill(dsDados, "Table1");

                    m_cnn.Close();

                    m_dt_dados = dsDados.Tables["Table1"];
                }

                else if (strExtensao == ".XLSX")
                {
                    m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + this.cmbTabelasNoArquivo.SelectedItem.ToString() + "]", m_cnn);
                    m_dap.Fill(dsDados, "Table1");

                    m_cnn.Close();

                    m_dt_dados = dsDados.Tables["Table1"];
                }

                m_nome_arquivo_importado = Path.GetFileNameWithoutExtension(strEnderecoBase);
                m_path_arquivo_importado = Path.GetFullPath(strEnderecoBase);

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region controles

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnImportarTabela_Click(object sender, EventArgs e)
        {
            try
            {
                ImportarTabela();

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Importação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnArquivoDados_Click(object sender, EventArgs e)
        {
            AbrirTabelaDados();
        }

        private void ckbDelimitadoTab_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbDelimitadoTab.Checked)
            {
                ckbDelimitadoTab.Checked = true;
                ckbDelimitadoPontoVirgula.Checked = false;
                ckbDelimitadoVirgula.Checked = false;
                ckbDelimitadoCaracter.Checked = false;

                txtCaracterDelimitacao.Enabled = false;
            }
            AjustaDelimitacao();
        }

        private void AjustaDelimitacao()
        {
            try
            {
                if (ckbDelimitadoCaracter.Checked || ckbDelimitadoPontoVirgula.Checked ||
                    ckbDelimitadoTab.Checked || ckbDelimitadoVirgula.Checked)
                {
                    cmbTabelasNoArquivo.Enabled = false;
                    ckbNomesPrimeiraLinha.Enabled = true;
                    ckbNomesPrimeiraLinha.Checked = true;
                }
                else
                {
                    if (strExtensao == ".TXT" || strExtensao == ".DAT")
                    {
                        ckbDelimitadoTab.Checked = true;
                    }
                    else
                    {
                        cmbTabelasNoArquivo.Enabled = true;
                        txtCaracterDelimitacao.Enabled = false;
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ckbDelimitadoVirgula_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbDelimitadoVirgula.Checked)
            {
                ckbDelimitadoTab.Checked = false;
                ckbDelimitadoPontoVirgula.Checked = false;
                ckbDelimitadoVirgula.Checked = true;
                ckbDelimitadoCaracter.Checked = false;

                txtCaracterDelimitacao.Enabled = false;
            }
            AjustaDelimitacao();
        }

        private void ckbDelimitadoPontoVirgula_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbDelimitadoPontoVirgula.Checked)
            {
                ckbDelimitadoTab.Checked = false;
                ckbDelimitadoPontoVirgula.Checked = true;
                ckbDelimitadoVirgula.Checked = false;
                ckbDelimitadoCaracter.Checked = false;

                txtCaracterDelimitacao.Enabled = false;
            }
            AjustaDelimitacao();
        }

        private void ckbDelimitadoCaracter_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbDelimitadoCaracter.Checked)
            {
                ckbDelimitadoTab.Checked = false;
                ckbDelimitadoPontoVirgula.Checked = false;
                ckbDelimitadoVirgula.Checked = false;
                ckbDelimitadoCaracter.Checked = true;

                txtCaracterDelimitacao.Enabled = true;
            }
            AjustaDelimitacao();
        }

        private void rdbArquivosExcelMDB_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdbArquivosExcelMDB.Checked)
                {
                    grbArquivosTXT.Enabled = false;
                    grbArquivosExcelMDB.Enabled = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void rdbArquivosTXT_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rdbArquivosTXT.Checked)
                {
                    grbArquivosTXT.Enabled = true;
                    grbArquivosExcelMDB.Enabled = false;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void rdbArquivosSAS_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rdbArquivosSAS.Checked)
                {
                    grbArquivosTXT.Enabled = false;
                    grbArquivosExcelMDB.Enabled = false;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
    }
}

