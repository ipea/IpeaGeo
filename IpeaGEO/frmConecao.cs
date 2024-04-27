using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace IpeaGEO
{
    public partial class frmExporta : Form
    {
        public frmExporta()
        {
            InitializeComponent();
        }

        //Propriedades do Dialog
        private string strEnderecoBase;
        private string strEnderecoShape;
        private string strIDbase;
        private string strIDmapa;
        private DataSet dsDados;
        private string[] strVariaveisMapa;
        private string strExtensao;
        public string ExtensaoDoArquivo
        {
            get
            {
                return strExtensao;
            }
            set
            {
                strExtensao = value;
            }
        }
        public string IDmapa
        {
            get
            {
                return strIDmapa;
            }
            set
            {
                strIDmapa = value;
            }
        }
        public string IDbase
        {
            get
            {
                return strIDbase;
            }
            set
            {
                strIDbase = value;
            }
        }

        public string  EnderecoBase
        {
            get 
            {
                return strEnderecoBase ; 
            }
            set 
            { 
                strEnderecoBase=value; 
            }
        }
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
        public DataSet BaseDeDados
        {
            get 
            {
                return dsDados ; 
            }
            set 
            { 
                dsDados=value; 
            }
        }
        public string[] VariaveiNoMapa
        {
            get
            {
                return strVariaveisMapa;
            }
            set
            {
                strVariaveisMapa = value;
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


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //Conexão MDB.
        private System.Data.OleDb.OleDbConnection m_cnn = new System.Data.OleDb.OleDbConnection();
        private System.Data.OleDb.OleDbDataAdapter m_dap = new System.Data.OleDb.OleDbDataAdapter();

        private void PopulaDataGridView(ref DataGridView dataGridView1, int iVariaveis)
        {
            //Cria as colunas do DataGridView
            DataGridViewTextBoxColumn txtbox = new DataGridViewTextBoxColumn();
            txtbox = new DataGridViewTextBoxColumn();
            txtbox.Width = 260;
            txtbox.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
            txtbox.ReadOnly = true;
            txtbox.HeaderText = "Variável";
            txtbox.DisplayIndex = 2;
            dataGridView1.Columns.Insert(0, txtbox);

            DataGridViewComboBoxColumn chkbox = new DataGridViewComboBoxColumn();
            chkbox = new DataGridViewComboBoxColumn();
            chkbox.Width = 230;
            chkbox.FlatStyle = FlatStyle.Popup;
            chkbox.HeaderText = "Tipo de agregação";
            chkbox.DisplayIndex = 2;
            chkbox.Items.Add("Soma");
            chkbox.Items.Add("Média");
            chkbox.Items.Add("Média ponderada pela área");
            chkbox.Items.Add("Média ponderada pela população");
            chkbox.Items.Add("Média ponderada pela densidade demográfica");
            chkbox.Items.Add("Primeiro valor");
            dataGridView1.Columns.Insert(1, chkbox);

            DataGridViewCheckBoxColumn chkbx_key = new DataGridViewCheckBoxColumn();
            chkbx_key = new DataGridViewCheckBoxColumn();
            chkbx_key.Width = 40;
            chkbx_key.FlatStyle = FlatStyle.Popup;
            chkbx_key.HeaderText = "Incluir";
            chkbx_key.DisplayIndex = 2;
            chkbx_key.TrueValue = true;
            dataGridView1.Columns.Insert(2, chkbx_key);

            //Adiciona as variáveis
            dataGridView1.Rows.Add(iVariaveis);
            for (int i = 0; i < iVariaveis; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = cmbIDdados.Items[i].ToString();
                dataGridView1.Rows[i].Cells[0].ToolTipText = cmbIDdados.Items[i].ToString();

                //dataGridView1.Rows[i].Cells[1].Value = "Primeiro valor";
                dataGridView1.Rows[i].Cells[2].Value = false;
            }
        }
       
        private void frmConecao_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            dataGridView1.AllowUserToAddRows = false;
            

            /*

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                //Get the appropriate cell using index, name or whatever and cast to DataGridViewCheckBoxCell
                //DataGridViewCheckBoxCell cell = row.Cells[2] as DataGridViewCheckBoxCell;
                //Compare to the true value because Value isn't boolean  if (cell.Value == cell.TrueValue) ;
                //The value is true
                DataGridViewTextBoxCell cell = row.Cells[0] as DataGridViewTextBoxCell;
                dataGridView1.Columns[0

             
             //int i = ((DataGridViewCheckBoxColumn)dataGridRow.Cells["checkbox_col"]).Value;
             //string s = dataGridRow.Cells["Name"].Value;
             //string j = dataGridRow.Cells["Abbreviation"].Value;
            }
             * */




            if (strExtensao == ".MDB")
            {
                /*Material como trabalhar com Dialogs: 
                  http://www.techexams.net/blogs/net/70-526/creating-dialog-boxes-in-net*/

                txtEndereco.Text = strEnderecoBase;
                //Inicia conexão com o MDB
                string m_cnnstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strEnderecoBase;
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

                //Acha as variaveis do mapa
                clsMapa classeMapa = new clsMapa();
                strVariaveisMapa = classeMapa.informacaoVariaveis(strEnderecoShape, (uint)0);

                //Coloca na combobox
                for (int i = 0; i < strVariaveisMapa.Length; i++) cmbIDmapa.Items.Add(strVariaveisMapa[i]);


            }
            else if (strExtensao == ".SAS7BDAT")
            {
                txtEndereco.Text = strEnderecoBase;
                label2.Text = "";
                cmbTabela.Enabled = false;
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

                //Acha as variaveis do mapa
                clsMapa classeMapa = new clsMapa();
                strVariaveisMapa = classeMapa.informacaoVariaveis(strEnderecoShape, (uint)0);

                //Coloca na combobox
                for (int i = 0; i < strVariaveisMapa.Length; i++) cmbIDmapa.Items.Add(strVariaveisMapa[i]);

                //Variaveis dos dados
                cmbIDdados.Items.Clear();
                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                {
                    cmbIDdados.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                }

                cmbIDdados.Enabled = true;
                cmbIDmapa.Enabled = true;

                //Popula o DataGridView
                PopulaDataGridView(ref dataGridView1, cmbIDdados.Items.Count);
            }
            else if (strExtensao == ".XML")
            {
                label2.Text = "";
                txtEndereco.Text = strEnderecoBase;

                cmbTabela.Enabled = false;

                /*
                    libname input 'c:\My Documents\myfiles';

                    filename xsd 'c:\My Documents\XML\suppliers.xsd';

                    libname output xml 'c:\My Documents\XML\suppliers.xml' xmltype=msaccess xmlmeta=schemadata xmlschema=xsd;

                    data output.suppliers;
                    set input.suppliers;
                    run;
 
                 */


                //Le arquivo
                string strSchema = strEnderecoBase.Substring(0, strEnderecoBase.Length - 3) + "xsd";
                if (File.Exists(strSchema))
                {
                    FileStream fsSchema = new FileStream(strSchema, FileMode.Open,FileAccess.Read, FileShare.ReadWrite);
                    dsDados.ReadXmlSchema(fsSchema);
                    fsSchema.Close();
                    //dsDados.Tables[0].ReadXmlSchema(strSchema);
                }
                dsDados.Tables[0].ReadXml(strEnderecoBase);
                
                //Acha as variaveis do mapa
                clsMapa classeMapa = new clsMapa();
                strVariaveisMapa = classeMapa.informacaoVariaveis(strEnderecoShape, (uint)0);

                //Coloca na combobox
                for (int i = 0; i < strVariaveisMapa.Length; i++) cmbIDmapa.Items.Add(strVariaveisMapa[i]);

                //Variaveis dos dados
                cmbIDdados.Items.Clear();
                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                {
                    cmbIDdados.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                }

                cmbIDdados.Enabled = true;
                cmbIDmapa.Enabled = true;
                //Popula o DataGridView
                PopulaDataGridView(ref dataGridView1, cmbIDdados.Items.Count);
            }
            else if (strExtensao == ".XLS")
            {
                label2.Text = "Planilha";
                txtEndereco.Text = strEnderecoBase;
                //Inicia conexão com o MDB
                string m_cnnstring = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strEnderecoBase + ";Extended Properties=Excel 8.0;";
                m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
                m_cnn.Open();
                System.Data.DataTable dt = null;

                dt = m_cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string[] restrictions = new string[dt.Rows.Count];

                foreach (DataRow row in dt.Rows)
                {
                   cmbTabela.Items.Add(row["TABLE_NAME"].ToString());
                }

                //Acha as variaveis do mapa
                clsMapa classeMapa = new clsMapa();
                strVariaveisMapa = classeMapa.informacaoVariaveis(strEnderecoShape, (uint)0);

                //Coloca na combobox
                for (int i = 0; i < strVariaveisMapa.Length; i++) cmbIDmapa.Items.Add(strVariaveisMapa[i]);
            }
            Cursor.Current = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Guarda o IDbase
                strIDbase = cmbIDdados.SelectedItem.ToString();

                //Guarda o IDmapa
                strIDmapa = cmbIDmapa.SelectedItem.ToString();

                //Guarda os

                //Definindo um layer
                SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("Temp");

                //Adicionando variaveis:
                SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEnderecoShape);

                //Abre o mapa para editar a suas propriedades
                shapefile.Open();

                //Guarda a informação do mapa no Layer
                layMapa.DataSource = shapefile;

                //Abre a classe
                clsMapa classeMapa = new clsMapa();

                //Cria um DataTable
                DataTable dTable = dsDados.Tables[0];

                //Mostra a progress bar
                progressBar1.Visible = true;

                //Popula o DataTable
                classeMapa.ConeccaoEntreMapaEdataTable(ref dTable,
                    strEnderecoShape, cmbIDdados.SelectedItem.ToString(),
                    cmbIDmapa.SelectedItem.ToString(), ref progressBar1, ref shapeAlex);

                //Limpa o DataSet
                dsDados.Tables.Clear();

                //Adiciona a tabela ao DataSet
                dsDados.Tables.Add(dTable);

                //Fecha o shapeFile
                shapefile.Close();

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTabela_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dsDados.Clear();
            dsDados.Reset();
            if (strExtensao == ".MDB")
            {
                m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM " + this.cmbTabela.SelectedItem.ToString(), m_cnn);
                m_dap.Fill(dsDados, "Table1");
                cmbIDdados.Items.Clear();

                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                {
                    cmbIDdados.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                }
                cmbIDdados.Enabled = true;
                cmbIDmapa.Enabled = true;
                m_cnn.Close();
            }
            else if (strExtensao == ".XLS")
            {
                m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + this.cmbTabela.SelectedItem.ToString() + "]", m_cnn);
                m_dap.Fill(dsDados, "Table1");
                cmbIDdados.Items.Clear();

                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                {
                    cmbIDdados.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                }
                cmbIDdados.Enabled = true;
                cmbIDmapa.Enabled = true;
                m_cnn.Close();
            }
        }

        private void cmbTabela_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbTabela_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void btnConecta_Click(object sender, EventArgs e)
        {
            
        }

        private void btnCancela_Click(object sender, EventArgs e)
        {
          
        }

        private void cmbTabela_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            //dsDados.Clear();
            dsDados.Reset();
            if (strExtensao == ".MDB")
            {
                m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM " + this.cmbTabela.SelectedItem.ToString(), m_cnn);
                m_dap.Fill(dsDados, "Table1");
                cmbIDdados.Items.Clear();

                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                {
                    cmbIDdados.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                }
                cmbIDdados.Enabled = true;
                cmbIDmapa.Enabled = true;
                m_cnn.Close();

                //Popula o DataGridView
                PopulaDataGridView(ref dataGridView1, cmbIDdados.Items.Count);
            }
            else if (strExtensao == ".XLS")
            {
                m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + this.cmbTabela.SelectedItem.ToString() + "]", m_cnn);
                m_dap.Fill(dsDados, "Table1");
                cmbIDdados.Items.Clear();

                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                {
                    cmbIDdados.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                }
                cmbIDdados.Enabled = true;
                cmbIDmapa.Enabled = true;
                m_cnn.Close();

                //Popula o DataGridView
                PopulaDataGridView(ref dataGridView1, cmbIDdados.Items.Count);
            }
        }
        private bool agrega(DataGridView grid)
        {
            for (int i = 0; i < cmbIDdados.Items.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[1].Value == null) return false;
            }

            return (true);
        }
        private void btnConecta_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (agrega(dataGridView1) == false)
                {
                    //Guarda o IDbase
                    strIDbase = cmbIDdados.SelectedItem.ToString();

                    //Guarda o IDmapa
                    strIDmapa = cmbIDmapa.SelectedItem.ToString();

                    //Guarda os

                    //Definindo um layer
                    SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("Temp");

                    //Adicionando variaveis:
                    SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEnderecoShape);

                    //Abre o mapa para editar a suas propriedades
                    shapefile.Open();

                    //Guarda a informação do mapa no Layer
                    layMapa.DataSource = shapefile;

                    //Abre a classe
                    clsMapa classeMapa = new clsMapa();

                    //Cria um DataTable
                    DataTable dTable = dsDados.Tables[0];

                    //Mostra a progress bar
                    progressBar1.Visible = true;

                    //Popula o DataTable
                    classeMapa.ConeccaoEntreMapaEdataTable(ref dTable,
                        strEnderecoShape, cmbIDdados.SelectedItem.ToString(),
                        cmbIDmapa.SelectedItem.ToString(), ref progressBar1, ref shapeAlex);

                    //Limpa o DataSet
                    dsDados.Tables.Clear();

                    //Adiciona a tabela ao DataSet
                    dsDados.Tables.Add(dTable);

                    //Fecha o shapeFile
                    shapefile.Close();
                }
                else 
                { 
                                        //Guarda o IDbase
                    strIDbase = cmbIDdados.SelectedItem.ToString();

                    //Guarda o IDmapa
                    strIDmapa = cmbIDmapa.SelectedItem.ToString();

                    //Guarda os

                    //Definindo um layer
                    SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("Temp");

                    //Adicionando variaveis:
                    SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(strEnderecoShape);

                    //Abre o mapa para editar a suas propriedades
                    shapefile.Open();

                    //Guarda a informação do mapa no Layer
                    layMapa.DataSource = shapefile;

                    //Abre a classe
                    clsMapa classeMapa = new clsMapa();

                    //Cria um DataTable
                    DataTable dTable = dsDados.Tables[0];

                    //Mostra a progress bar
                    progressBar1.Visible = true;

                    //Popula o DataTable
                    classeMapa.ConeccaoEntreMapaEdataTable(ref dTable,
                        strEnderecoShape, cmbIDdados.SelectedItem.ToString(),
                        cmbIDmapa.SelectedItem.ToString(), ref progressBar1, ref shapeAlex);

                    //Limpa o DataSet
                    dsDados.Tables.Clear();


                    //Criar o datatable das agregacoes
                    DataTable dt = dTable.Copy();
                    clsFuncaoAgregacao clsagrega = new clsFuncaoAgregacao();

                    for (int i = 0; i < cmbIDdados.Items.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[1].Value == "Primeiro Valor")
                        {
                            //clsagrega.agregacao_primeiro_valor(ref dt,dTable,
 
                        }
                        
                        if (dataGridView1.Rows[i].Cells[1].Value == "Soma")
                        {

                        }

                        if (dataGridView1.Rows[i].Cells[1].Value == "Media")
                        {

                        }

                        if (dataGridView1.Rows[i].Cells[1].Value == "Media Ponderada por Area")
                        {

                        }

                        if (dataGridView1.Rows[i].Cells[1].Value == "Media Ponderada por Populacao")
                        {

                        }

                        if (dataGridView1.Rows[i].Cells[1].Value == "Media Ponderada por Densidade Demografica")
                        {

                        }
                           
                    }





                    //Adiciona a tabela ao DataSet
                    dsDados.Tables.Add(dTable);

                    //Fecha o shapeFile
                    shapefile.Close();
                
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnCancela_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cbx = (ComboBox)e.Control;
                cbx.SelectedIndex = this.dataGridView1.CurrentCell.RowIndex % 3;
            }
            
        }
    }
}
