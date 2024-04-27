using System;
using System.Data;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem.Clusters_hierarquicos
{
    public partial class FormClustersHierarquicosNaoEspaciais : Form
    {
        #region atualização da tabela de dados
        
        private void PopulaDataGridView(ref DataGridView dataGridView1, int iVariaveis)
        {
            //Cria as colunas do DataGridView
            DataGridViewTextBoxColumn txtbox = new DataGridViewTextBoxColumn();
            txtbox = new DataGridViewTextBoxColumn();
            //txtbox.Width = 260;
            txtbox.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
            txtbox.ReadOnly = true;
            txtbox.HeaderText = "Variável";
            txtbox.DisplayIndex = 2;
            dataGridView1.Columns.Insert(0, txtbox);

            DataGridViewComboBoxColumn chkbox = new DataGridViewComboBoxColumn();
            chkbox = new DataGridViewComboBoxColumn();
            //chkbox.Width = 50;
            chkbox.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
            chkbox.FlatStyle = FlatStyle.Popup;
            chkbox.HeaderText = "Tipo de variável";
            chkbox.DisplayIndex = 2;
            chkbox.Items.Add("Contínuas");
            chkbox.Items.Add("Binárias");
            chkbox.Items.Add("Categóricas");
            chkbox.Items.Add("Ordinais");
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
            for (int i = 0; i < strVariaveis.Length; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = strVariaveis[i];
                dataGridView1.Rows[i].Cells[0].ToolTipText = strVariaveis[i];
                dataGridView1.Rows[i].Cells[2].ToolTipText = strVariaveis[i];
                dataGridView1.Rows[i].Cells[1].Value = "Contínuas";
                dataGridView1.Rows[i].Cells[2].Value = false;
            }
        }

        private IpeaGeo.Classes.clsStat m_blc = new IpeaGeo.Classes.clsStat();

        private void AtualizaTabelaDados(bool atualizaUControl)
        {
            if (atualizaUControl && m_dt_tabela_dados.Columns.Count > 0 && m_dt_tabela_dados.Rows.Count > 0)
            {
                this.userControlDataGrid1.TabelaDados = m_dt_tabela_dados;
            }
            else
            {
                if (this.userControlDataGrid1.TabelaDados.Columns.Count > 0 && this.userControlDataGrid1.TabelaDados.Rows.Count > 0)
                {
                    m_dt_tabela_dados = this.userControlDataGrid1.TabelaDados;
                }
            }

            clsUtilTools clt = new clsUtilTools();

            if (m_dt_tabela_dados.Columns.Count > 0)
            {
                string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);
                PopulaDataGridView(ref this.dataGridView2, variaveis_numericas.GetLength(0)); 
            }

            if (!this.tabControl1.TabPages.Contains(tabPage0))
            {
                tabControl1.TabPages.Add(tabPage0);
            }
            this.tabControl1.SelectedTab = tabPage0;
        }

        #endregion 

        #region Variáveis internas

        private string[] strVariaveis;
        public string[] Variaveis
        {
            get
            {
                return strVariaveis;
            }
            set
            {
                strVariaveis = value;
            }
        }

        public void HabilitarDadosExternos()
        {
            this.btnOK.Visible =
                btnOK.Enabled = true;
        }

        private DataSet m_dataset_externo = new DataSet();
        public DataSet DataSetExterno
        {
            set
            {
                m_dataset_externo = value;
            }
            get
            {
                return m_dataset_externo;
            }
        }

        private DataGridView m_gridview_externo = new DataGridView();
        public DataGridView GridViewExterno
        {
            set
            {
                m_gridview_externo = value;
            }
        }

        private string m_label_tabela_dados = "";
        public string LabelTabelaDados
        {
            set
            {
                m_label_tabela_dados = value;

                this.Text = "Clusters Hierárquicos (não-espaciais) - " + value;
            }
        }

        public DataTable TabelaDeDados
        {
            get { return this.m_dt_tabela_dados; }
            set
            {
                this.m_dt_tabela_dados = value;

                AtualizaTabelaDados(true);

                if (value.Rows.Count > 0 && value.Columns.Count > 0)
                {
                    this.button2.Enabled = true;
                }
            }
        }

        private DataTable m_dt_tabela_dados = new DataTable();
        public DataTable Dados
        {
            get { return m_dt_tabela_dados; }
            set { m_dt_tabela_dados = value; }
        }

        public string m_output_text = "";
        public string m_output_vars_geradas = "";

        #endregion

        public FormClustersHierarquicosNaoEspaciais()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormClustersHierarquicosNaoEspaciais_Load(object sender, EventArgs e)
        {
            try
            {
                // Variáveis sendo passadas para o UserControl
                userControlDataGrid1.TabelaDados = this.m_dt_tabela_dados;
                userControlDataGrid1.FuncaoFromFormulario = new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionFromFormulario(this.AtualizaTabelaDados);
                userControlDataGrid1.MostraOpcaoImportadaoDados = true;

                if (tabControl1.TabPages.Contains(tabPage2))
                {
                    tabControl1.TabPages.Remove(tabPage2);
                }

                if (tabControl1.TabPages.Contains(tabPage3))
                {
                    tabControl1.TabPages.Remove(tabPage3);
                }
                
                if (tabControl1.TabPages.Contains(tabPage4))
                {
                    tabControl1.TabPages.Remove(tabPage4);
                }

                if (m_dt_tabela_dados.Rows.Count <= 0 || m_dt_tabela_dados.Columns.Count <= 0)
                {
                    if (tabControl1.TabPages.Contains(tabPage0))
                    {
                        tabControl1.TabPages.Remove(tabPage0);
                    }
                }                
                
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
