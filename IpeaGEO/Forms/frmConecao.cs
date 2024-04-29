using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace IpeaGeo
{
    public partial class frmExporta : Form
    {
        public frmExporta()
        {
            InitializeComponent();
        }

        //Propriedades do Dialog
        private string EnderecoBase;
        private string EnderecoShape;
        private string IDbase;
        private string IDmapa;
        private DataSet dsDados;
        private string[] strVariaveisMapa;
        private string ExtensaoDoArquivo;
        private bool deuerro_;
        public bool DeuErro
        {
            get { return deuerro_; }
        }
        
        private bool deuerro2_ = false;
        public bool DeuErro2
        {
            get { return deuerro2_; }
        }

        public clsIpeaShape EstruturaShape;

        //Conexão MDB.
        private System.Data.OleDb.OleDbConnection m_cnn = new System.Data.OleDb.OleDbConnection();
        private System.Data.OleDb.OleDbDataAdapter m_dap = new System.Data.OleDb.OleDbDataAdapter();

        private void PopulaDataGridView(ref DataGridView dataGridView1, int iVariaveis)
        {
            try
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
                    dataGridView1.Rows[i].Cells[2].Value = false;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmConecao_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                dataGridView1.AllowUserToAddRows = false;
                tabControl1.TabPages.RemoveAt(1);
                dsDados.Reset();

                if (ExtensaoDoArquivo == ".MDB")
                {
                    /*Material como trabalhar com Dialogs: 
                      http://www.techexams.net/blogs/net/70-526/creating-dialog-boxes-in-net*/

                    txtEndereco.Text = EnderecoBase;
                    //Inicia conexão com o MDB
                    string m_cnnstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + EnderecoBase;
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
                    strVariaveisMapa = classeMapa.informacaoVariaveis(EnderecoShape, (uint)0);

                    //Coloca na combobox
                    for (int i = 0; i < strVariaveisMapa.Length; i++) cmbIDmapa.Items.Add(strVariaveisMapa[i]);
                }
                else if (ExtensaoDoArquivo == ".SAS7BDAT")
                {
                    txtEndereco.Text = EnderecoBase;
                    label2.Text = "";
                    cmbTabela.Enabled = false;
                    
                    //Endereço da pasta com o arquivo
                    string strPath = Path.GetDirectoryName(EnderecoBase);
                    
                    //Nome do arquivo
                    string strAqruivo = Path.GetFileNameWithoutExtension(EnderecoBase);
                    
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
                    strVariaveisMapa = classeMapa.informacaoVariaveis(EnderecoShape, (uint)0);

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
                else if (ExtensaoDoArquivo == ".XML")
                {
                    label2.Text = "";
                    txtEndereco.Text = EnderecoBase;

                    cmbTabela.Enabled = false;

                    //Le arquivo
                    string strSchema = EnderecoBase.Substring(0, EnderecoBase.Length - 3) + "xsd";
                    if (File.Exists(strSchema))
                    {
                        FileStream fsSchema = new FileStream(strSchema, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        dsDados.ReadXmlSchema(fsSchema);
                        fsSchema.Close();
                        //dsDados.Tables[0].ReadXmlSchema(strSchema);
                    }
                    else
                    {
                        dsDados.Tables[0].WriteXml(EnderecoBase, XmlWriteMode.WriteSchema);
                    }

                    dsDados.Tables[0].ReadXml(EnderecoBase);

                    //Acha as variaveis do mapa
                    clsMapa classeMapa = new clsMapa();
                    strVariaveisMapa = classeMapa.informacaoVariaveis(EnderecoShape, (uint)0);

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
                else if (ExtensaoDoArquivo == ".XLS")
                {
                    label2.Text = "Planilha";
                    txtEndereco.Text = EnderecoBase;
                    
                    //Inicia conexão com o MDB
                    string m_cnnstring = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + EnderecoBase + ";Extended Properties=Excel 8.0;";
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

                    string[] restrictions = new string[dt.Rows.Count];

                    foreach (DataRow row in dt.Rows)
                    {
                        cmbTabela.Items.Add(row["TABLE_NAME"].ToString());
                    }

                    //Acha as variaveis do mapa
                    clsMapa classeMapa = new clsMapa();
                    strVariaveisMapa = classeMapa.informacaoVariaveis(EnderecoShape, (uint)0);

                    //Coloca na combobox
                    for (int i = 0; i < strVariaveisMapa.Length; i++) cmbIDmapa.Items.Add(strVariaveisMapa[i]);
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmbTabela_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                dsDados.Reset();
                if (ExtensaoDoArquivo == ".MDB")
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
                else if (ExtensaoDoArquivo == ".XLS")
                {
                    m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + this.cmbTabela.SelectedItem.ToString() + "]", m_cnn);
                    m_dap.Fill(dsDados, "Table1");

                    bool corrigir_tabela = false;
                    for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                    {
                        string teste = dsDados.Tables[0].Columns[i].ColumnName.ToString();
                        int tamanho = teste.Length;
                        if (tamanho == 64)
                        {
                            corrigir_tabela = true;
                            break;
                        }
                    }

                    if (corrigir_tabela)
                    {
                        label5.Text = "Nomes de colunas maiores que 64 caracteres serão truncados.";
                        Application.DoEvents();
                    }
                    else
                    {
                        label5.Text = "";
                        Application.DoEvents();
                    }
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
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    IDbase = cmbIDdados.SelectedItem.ToString();

                    //Guarda o IDmapa
                    IDmapa = cmbIDmapa.SelectedItem.ToString();

                    //Definindo um layer
                    SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("Temp");

                    //Adicionando variaveis:
                    SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(EnderecoShape);

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

                    int linhastabela = dTable.Rows.Count;
                    int linhasshape = EstruturaShape.Count;

                    int abc = 0;
                    if (linhastabela != linhasshape)
                    {
                        dsDados.Tables.Clear();
                        deuerro_ = true;
                        this.Cursor = Cursors.Default;
                        Application.DoEvents();
                        MessageBox.Show("Os dados da base não correspondem aos polígonos do mapa", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    if (deuerro_ == false)
                    {
                        deuerro_ = false;

                        //Popula o DataTable
                        classeMapa.ConeccaoEntreMapaEdataTable(ref dTable,
                            EnderecoShape, cmbIDdados.SelectedItem.ToString(),
                            cmbIDmapa.SelectedItem.ToString(), ref progressBar1, ref EstruturaShape, ref deuerro_);

                        //Checar se existe a coluna final de mapacode para TODAS as linhas. (Evitar conexão com var diferente do mapa)
                        string nomecol = "MAPA" + cmbIDmapa.SelectedItem.ToString();
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            string teste = dTable.Rows[i][nomecol].ToString();
                            if (teste == "")
                            {
                                deuerro2_ = true;
                                MessageBox.Show("A variável de conexão não corresponde à divisão do mapa");
                                this.DialogResult = DialogResult.OK;
                                dTable.Clear();
                                dTable.Reset();
                                this.Close();
                                break;

                            }
                        }

                        //Atribui nomes aos polígonos no shapeAlex
                        string[] nomes_poligonos = new string[dTable.Rows.Count];
                        int indice = 0;
                        foreach (DataRow dr in dTable.Rows)
                        {
                            nomes_poligonos[indice] = dr[cmbIDdados.SelectedItem.ToString()].ToString();
                            EstruturaShape[indice].Label = nomes_poligonos[indice];
                            indice++;
                        }

                        //Limpa o DataSet
                        dsDados.Tables.Clear();

                        //Adiciona a tabela ao DataSet
                        dsDados.Tables.Add(dTable);
                    }

                    //Fecha o shapeFile
                    shapefile.Close();
                }
                else
                {
                    //Guarda o IDbase
                    IDbase = cmbIDdados.SelectedItem.ToString();

                    //Guarda o IDmapa
                    IDmapa = cmbIDmapa.SelectedItem.ToString();

                    //Definindo um layer
                    SharpMap.Layers.VectorLayer layMapa = new SharpMap.Layers.VectorLayer("Temp");

                    //Adicionando variaveis:
                    SharpMap.Data.Providers.ShapeFile shapefile = new SharpMap.Data.Providers.ShapeFile(EnderecoShape);

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
                        EnderecoShape, cmbIDdados.SelectedItem.ToString(),
                        cmbIDmapa.SelectedItem.ToString(), ref progressBar1, ref EstruturaShape, ref deuerro_);

                    //Checar se existe a coluna final de mapacode para TODAS as linhas. (Evitar conexão com var diferente do mapa)
                    string nomecol = "MAPA" + cmbIDmapa.SelectedItem.ToString();
                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        string teste = dTable.Rows[i][nomecol].ToString();
                        if (teste == "")
                        {
                            deuerro2_ = true;
                            MessageBox.Show("A variável de conexão não corresponde à divisão do mapa");
                            this.DialogResult = DialogResult.OK;
                            dTable.Dispose();
                            dTable.Reset();
                            this.Close();
                            break;

                        }
                    }

                    //Limpa o DataSet
                    dsDados.Tables.Clear();

                    //Criar o datatable das agregacoes
                    DataTable dt = dTable.Copy();

                    //Adiciona a tabela ao DataSet
                    dsDados.Tables.Add(dTable);

                    //O dTable deve ter o numero de linhas igual ao numero de poligonos do mapa
                    int linhastabela = dTable.Rows.Count;
                    int linhasshape = EstruturaShape.Count;
                    if (linhasshape != linhastabela)
                    {
                        dsDados.Tables.Clear();
                        deuerro_ = true;
                        MessageBox.Show("Os dados da base não correspondem aos polígonos do mapa", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    //Fecha o shapeFile
                    shapefile.Close();
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancela_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (e.Control is ComboBox)
                {
                    ComboBox cbx = (ComboBox)e.Control;
                    cbx.SelectedIndex = this.dataGridView1.CurrentCell.RowIndex % 3;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
}
