using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem.decisaoMulticriterios
{
    public partial class FormDecisaoMulticriterios : Form
    {
        #region Variáveis internas

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

                this.Text = "Modelos de Regressão Linear - " + value;
            }
        }

        private clsUtilTools m_clt = new clsUtilTools();

        private BLogicDecisaoMulticriterios bldm;

        private DataTable m_dt_tabela_shape = new DataTable();
        public DataTable DadosShape
        {
            get { return m_dt_tabela_shape; }
            set { m_dt_tabela_shape = value; }
        }

        private DataTable m_dt_tabela_dados = new DataTable();
        public DataTable Dados
        {
            get { return this.m_dt_tabela_dados; }
            set
            {
                this.m_dt_tabela_dados = value;

                AtualizaTabelaDados(true);

                if (value.Rows.Count > 0 && value.Columns.Count > 0)
                {
                    this.buttonExecutar.Enabled = true;
                }
            }
        }

        private clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            get { return m_shape; }
            set { m_shape = value; }
        }

        #endregion

        #region métodos básicos do form

        public FormDecisaoMulticriterios()
        {
            InitializeComponent();
        }

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
                this.userControlSelecaoVariaveis1.ZeraControle();
                this.userControlSelecaoVariaveis1.VariaveisDB = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
                this.userControlSelecaoVariaveis1.VariaveisList = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);

                string[] variaveis = clt.RetornaTodasColunas(m_dt_tabela_dados);

                comboBoxVariavelIdentificadora.Items.AddRange(variaveis);
                comboBoxVariavelIdentificadora.SelectedIndex = 0;
            }

            if (!this.tabControl1.TabPages.Contains(tabPage0))
            {
                tabControl1.TabPages.Add(tabPage0);
            }
            this.tabControl1.SelectedTab = tabPage0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (ckbIncluirNovasVariaveisTabelaDados.Checked)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    m_gridview_externo.DataSource = ((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy();

                    this.DataSetExterno.Tables.Clear();
                    this.DataSetExterno.Tables.Add(((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy());
                   
                    lblProgressBar.Visible = true;
                    lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
                    this.Cursor = Cursors.Default;
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void FormBaseModelagem_Load(object sender, EventArgs e)
        {
            try
            {
                buttonExecutar.Enabled = false;
                this.Text = "Métodos Multicritérios de Apoio a Decisão";
                userControlSelecaoVariaveis1.LabelListBoxEsquerda = "Variáveis numéricas na tabela";
                userControlSelecaoVariaveis1.LabelListBoxDireita = "Variáveis selecionadas";
                userControlSelecaoVariaveis1.PermiteSelecaoMultipla = true;

                // Variáveis sendo passadas para o UserControl
                userControlDataGrid1.TabelaDados = this.m_dt_tabela_dados;
                userControlDataGrid1.FuncaoFromFormulario =
                   new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionFromFormulario(this.AtualizaTabelaDados);

                userControlDataGrid1.MostraOpcaoImportadaoDados = true;
                userControlDataGrid1.UserControlSelecaoVariaveis = this.userControlSelecaoVariaveis1;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Open tabela de dados e tabela shape

        private void btnAbrirArquivoShape_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblProgressBar.Text = "Importando arquivo em formato shape";

                clsUtilArquivos cla = new clsUtilArquivos();
                cla.ImportarArquivoShape(ref m_shape, ref m_dt_tabela_shape);

                this.lblProgressBar.Text = "Arquivo shape importado com sucesso";
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAbrirTabelaDados_Click(object sender, EventArgs e)
        {
            try
            {
                clsUtilArquivos cla = new clsUtilArquivos();
                if (cla.ImportarTabelaDados(ref m_dt_tabela_dados))
                {
                    AtualizaTabelaDados(true);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region toolstrips menus

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblProgressBar.Text = "Importando arquivo em formato shape";

                clsUtilArquivos cla = new clsUtilArquivos();
                cla.ImportarArquivoShape(ref m_shape, ref m_dt_tabela_shape);

                this.lblProgressBar.Text = "Arquivo shape importado com sucesso";
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                clsUtilArquivos cla = new clsUtilArquivos();
                if (cla.ImportarTabelaDados(ref m_dt_tabela_dados))
                {
                    AtualizaTabelaDados(true);
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
                clsUtilArquivos cla = new clsUtilArquivos();
                cla.ExportarTabela((DataTable)this.userControlDataGrid1.TabelaDados, this.Name);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void calculadoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormCalculadora frm = new IpeaGeo.RegressoesEspaciais.FormCalculadora();
                    frm.MdiParent = this.MdiParent;
                    frm.Dados = this.m_dt_tabela_dados;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.m_dt_tabela_dados = frm.Dados;
                    }
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0))
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void excluirVariáveisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormCalculadora frm = new IpeaGeo.RegressoesEspaciais.FormCalculadora();
                    frm.MdiParent = this.MdiParent;
                    frm.Dados = this.m_dt_tabela_dados;
                    frm.AtivaExclusaoVariaveis = true;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.m_dt_tabela_dados = frm.Dados;
                        this.AtualizaTabelaDados(true);
                    }
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void estatísticasDescritivasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormEstatisticasDescritivas frm = new IpeaGeo.RegressoesEspaciais.FormEstatisticasDescritivas();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.m_dt_tabela_dados;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormTabelaFrequencias frm = new IpeaGeo.RegressoesEspaciais.FormTabelaFrequencias();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.m_dt_tabela_dados;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormTabelasCruzadas frm = new IpeaGeo.RegressoesEspaciais.FormTabelasCruzadas();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.m_dt_tabela_dados;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormCorrelacoes frm = new IpeaGeo.RegressoesEspaciais.FormCorrelacoes();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.m_dt_tabela_dados;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region eventos de buttons

        private void buttonImportarArvore_Click(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show("Deseja importar uma nova árvore hierarquica? Os dados não salvos da árvore atual serão perdidos",
                            "Importar árvore hierarquica",
                            MessageBoxButtons.YesNo);
            if (result1 == DialogResult.No)
            {
                return;
            }
            else
                importarArvoreHierarquica();
        }

        private void buttonApagarArvore_Click(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show("Deseja apagar todas as informações da árvore hierárquica?",
                                        "Apagar árvore hierarquica",
                                        MessageBoxButtons.YesNo);
            if (result1 == DialogResult.No)
            {
                return;
            }
            else
                apagarArvoreHierarquica();
        }

        private void buttonApagarAlternativa_Click(object sender, EventArgs e)
        {
            apagarAlternativa(listBoxAlternativas.SelectedIndex);
        }

        private void buttonExecutar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string nomeVariavelIdentificadora;
                if (comboBoxVariavelIdentificadora.SelectedItem == null)
                {
                    DialogResult result1 = MessageBox.Show("Selecione a variável identificadora",
                                        "Executar",
                                        MessageBoxButtons.OK);
                    return;
                }
                else
                    nomeVariavelIdentificadora = comboBoxVariavelIdentificadora.SelectedItem.ToString();

                if (radioButtonAlgebrico.Checked)
                {

                    BLogicDecisaoMulticriterios bldm = new BLogicDecisaoMulticriterios("Índice Algébrico", BLogicDecisaoMulticriterios.Metodo.Geral);

                    //Passo 1: Passa a lista de variáveis para criar o indice
                    string[] strVariaveisSelecionadas = userControlSelecaoVariaveis1.VariaveisIndependentes;

                    //Passo 2: Captura os pesos
                    double[] pesos = new double[strVariaveisSelecionadas.Length];
                    bool[] padronizados = new bool[strVariaveisSelecionadas.Length];

                    for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
                    {
                        pesos[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value);
                        padronizados[i] = Convert.ToBoolean(dataGridView1.Rows[i].Cells[2].Value);
                    }

                    double[] indiceAlgebrico = bldm.IndiceAlgebrico(ref m_dt_tabela_dados, strVariaveisSelecionadas, pesos, padronizados);

                    //Verifica se a variável indice algebrico existe no dataTable
                    string[] strVariavelID = new string[m_dt_tabela_dados.Rows.Count];
                    if (m_dt_tabela_dados.Columns.Contains("Índice Algébrico"))
                    {
                        m_dt_tabela_dados.Columns.Remove("Índice Algébrico");
                    }
                    
                    if (m_dt_tabela_dados.Columns.Contains("Índice Algébrico"))
                    {
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Algébrico"] = indiceAlgebrico[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    else
                    {
                        m_dt_tabela_dados.Columns.Add("Índice Algébrico", typeof(double));
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Algébrico"] = indiceAlgebrico[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    
                    if (checkBoxLimparResultado.Checked) this.userControlRichTextOutput2.Texto = "";
                    this.userControlRichTextOutput2.Texto += bldm.imprimirAlgebrico(indiceAlgebrico, strVariavelID, "Índice Algébrico", nomeVariavelIdentificadora, strVariaveisSelecionadas, pesos, padronizados);

                    if (ckbIncluirNovasVariaveisTabelaDados.Checked)
                    {
                        m_gridview_externo.DataSource = ((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy();

                        this.DataSetExterno.Tables.Clear();
                        this.DataSetExterno.Tables.Add(((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy());

                        
                        lblProgressBar.Visible = true;
                        lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
                    }
                    tabControl1.SelectedTab = tabPage5;

                }
                
                if (rbComponentesPrincipais.Checked)
                {
                    BLogicDecisaoMulticriterios bldm = new BLogicDecisaoMulticriterios("Índice Componentes Principais", BLogicDecisaoMulticriterios.Metodo.Geral);

                    //Passo 1: Passa a lista de variáveis para criar o indice
                    string[] strVariaveisSelecionadas = userControlSelecaoVariaveis1.VariaveisIndependentes;

                    //Passo 2: Captura os pesos
                    double[] pesos = new double[strVariaveisSelecionadas.Length];
                    bool[] padronizados = new bool[strVariaveisSelecionadas.Length];

                    for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
                    {
                          padronizados[i] = Convert.ToBoolean(dataGridView1.Rows[i].Cells[1].Value);
                    }

                    double[] indiceCompPrin = bldm.IndiceComponentesPrincipais(ref m_dt_tabela_dados, strVariaveisSelecionadas, padronizados, out pesos);

                    //Verifica se a variável indice algebrico existe no dataTable
                    string[] strVariavelID = new string[m_dt_tabela_dados.Rows.Count];
                    
                    if (m_dt_tabela_dados.Columns.Contains("Índice Componentes Principais"))
                    {
                        m_dt_tabela_dados.Columns.Remove("Índice Componentes Principais");
                    }
                    
                    if (m_dt_tabela_dados.Columns.Contains("Índice Componentes Principais"))
                    {
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Componentes Principais"] = indiceCompPrin[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    else
                    {
                        m_dt_tabela_dados.Columns.Add("Índice Componentes Principais", typeof(double));
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Componentes Principais"] = indiceCompPrin[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    
                    if (checkBoxLimparResultado.Checked) this.userControlRichTextOutput2.Texto = "";
                    this.userControlRichTextOutput2.Texto += bldm.imprimirComponentesPrincipais(indiceCompPrin, strVariavelID, "Índice Componentes Principais", nomeVariavelIdentificadora, strVariaveisSelecionadas, pesos, padronizados);

                    if (ckbIncluirNovasVariaveisTabelaDados.Checked)
                    {
                        m_gridview_externo.DataSource = ((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy();

                        this.DataSetExterno.Tables.Clear();
                        this.DataSetExterno.Tables.Add(((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy());

                        
                        lblProgressBar.Visible = true;
                        lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
                    }
                  
                    tabControl1.SelectedTab = tabPage5;
                }
                
                if (radioButtonHierarquico.Checked)
                {
                    BLogicDecisaoMulticriterios bldm = new BLogicDecisaoMulticriterios("Análise Hierárquica Clássica", BLogicDecisaoMulticriterios.Metodo.Geral);

                    //Passo 1: Passa a lista de variáveis para criar o indice
                    string[] strVariaveisSelecionadas = userControlSelecaoVariaveis1.VariaveisIndependentes;

                    //Passo 2: Calcula o indice
                    double lambdaMAX = 0;
                    double RC = 0;
                    double IC = 0;
                    double[,] pesos = new double[strVariaveisSelecionadas.GetLength(0), strVariaveisSelecionadas.GetLength(0)];
                    double[] indiceHierar = bldm.IndiceAnaliseHierarquicaClassica(ref m_dt_tabela_dados, dataGridView1, strVariaveisSelecionadas, ref pesos, out lambdaMAX, out RC, out IC);

                    //Verifica se a variável indice algebrico existe no dataTable
                    string[] strVariavelID = new string[m_dt_tabela_dados.Rows.Count];
                    if (m_dt_tabela_dados.Columns.Contains("Índice Análise Hierárquica Clássica"))
                    {
                        m_dt_tabela_dados.Columns.Remove("Índice Análise Hierárquica Clássica");
                    }
                    if (m_dt_tabela_dados.Columns.Contains("Índice Análise Hierárquica Clássica"))
                    {
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Análise Hierárquica Clássica"] = indiceHierar[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    else
                    {
                        m_dt_tabela_dados.Columns.Add("Índice Análise Hierárquica Clássica", typeof(double));
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Análise Hierárquica Clássica"] = indiceHierar[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    
                    if (checkBoxLimparResultado.Checked) this.userControlRichTextOutput2.Texto = "";
                    this.userControlRichTextOutput2.Texto += bldm.imprimirHierarquicoClassico(indiceHierar, strVariavelID, "Índice Análise Hierárquica Clássica", nomeVariavelIdentificadora, strVariaveisSelecionadas, pesos, lambdaMAX, IC, RC);
                    if (ckbIncluirNovasVariaveisTabelaDados.Checked)
                    {
                        m_gridview_externo.DataSource = ((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy();

                        this.DataSetExterno.Tables.Clear();
                        this.DataSetExterno.Tables.Add(((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy());
                       
                        lblProgressBar.Visible = true;
                        lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
                    }
                    tabControl1.SelectedTab = tabPage5;
                }
                if (radioButton1.Checked)
                {
                    BLogicDecisaoMulticriterios bldm = new BLogicDecisaoMulticriterios("Análise Hierárquica Multiplicativa", BLogicDecisaoMulticriterios.Metodo.Geral);

                    //Passo 1: Passa a lista de variáveis para criar o indice
                    string[] strVariaveisSelecionadas = userControlSelecaoVariaveis1.VariaveisIndependentes;

                    //Passo 2: Calcula o indice
                    double lambdaMAX = 0;
                    double RC = 0;
                    double IC = 0;
                    double[,] pesos = new double[strVariaveisSelecionadas.GetLength(0), strVariaveisSelecionadas.GetLength(0)];
                    double[] indiceHierar = bldm.IndiceAnaliseHierarquicaMultiplicativa(ref m_dt_tabela_dados, dataGridView1, strVariaveisSelecionadas, ref pesos, out lambdaMAX, out RC, out IC);

                    //Verifica se a variável indice algebrico existe no dataTable
                    string[] strVariavelID = new string[m_dt_tabela_dados.Rows.Count];
                    if (m_dt_tabela_dados.Columns.Contains("Índice Análise Hierárquica Multiplicativa"))
                    {
                        m_dt_tabela_dados.Columns.Remove("Índice Análise Hierárquica Multiplicativa");
                    }
                    
                    if (m_dt_tabela_dados.Columns.Contains("Índice Análise Hierárquica Multiplicativa"))
                    {
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Análise Hierárquica Multiplicativa"] = indiceHierar[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    else
                    {
                        m_dt_tabela_dados.Columns.Add("Índice Análise Hierárquica Multiplicativa", typeof(double));
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Análise Hierárquica Multiplicativa"] = indiceHierar[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    
                    if (checkBoxLimparResultado.Checked) this.userControlRichTextOutput2.Texto = "";
                    this.userControlRichTextOutput2.Texto += bldm.imprimirHierarquicoMultiplicativo(indiceHierar, strVariavelID, "Índice Análise Hierárquica Multiplicativa", nomeVariavelIdentificadora, strVariaveisSelecionadas, pesos, lambdaMAX, IC, RC);
                    
                    if (ckbIncluirNovasVariaveisTabelaDados.Checked)
                    {
                        m_gridview_externo.DataSource = ((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy();

                        this.DataSetExterno.Tables.Clear();
                        this.DataSetExterno.Tables.Add(((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy());
                       
                        lblProgressBar.Visible = true;
                        lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
                    }
                    tabControl1.SelectedTab = tabPage5;
                }

                if (rbAHPReferenciado.Checked)
                {
                    BLogicDecisaoMulticriterios bldm = new BLogicDecisaoMulticriterios("Análise Hierárquica Referenciada", BLogicDecisaoMulticriterios.Metodo.Geral);

                    //Passo 1: Passa a lista de variáveis para criar o indice
                    string[] strVariaveisSelecionadas = userControlSelecaoVariaveis1.VariaveisIndependentes;

                    //Passo 2: Calcula o indice
                    double lambdaMAX = 0;
                    double RC = 0;
                    double IC = 0;
                    double[,] pesos = new double[strVariaveisSelecionadas.GetLength(0), strVariaveisSelecionadas.GetLength(0)];
                    double[] indiceHierar = bldm.IndiceAnaliseHierarquicaReferenciado(ref m_dt_tabela_dados, dataGridView1, strVariaveisSelecionadas, ref pesos, out lambdaMAX, out RC, out IC);

                    //Verifica se a variável indice algebrico existe no dataTable
                    string[] strVariavelID = new string[m_dt_tabela_dados.Rows.Count];

                    if (m_dt_tabela_dados.Columns.Contains("Índice Análise Hierárquica Referenciada"))
                    {
                        m_dt_tabela_dados.Columns.Remove("Índice Análise Hierárquica Referenciada");
                    }
                    
                    if (m_dt_tabela_dados.Columns.Contains("Índice Análise Hierárquica Referenciada"))
                    {
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Análise Hierárquica Referenciada"] = indiceHierar[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    else
                    {
                        m_dt_tabela_dados.Columns.Add("Índice Análise Hierárquica Referenciada", typeof(double));
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Análise Hierárquica Referenciada"] = indiceHierar[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    
                    if (checkBoxLimparResultado.Checked) this.userControlRichTextOutput2.Texto = "";
                    string[] strEstatistica = new string[dataGridView1.RowCount];
                    string[] strPadroniza = new string[dataGridView1.RowCount];
                    
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        strEstatistica[i] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "True")
                        {
                            strPadroniza[i] = "Sim";
                        }
                        else
                        {
                            strPadroniza[i] = "Não";
                        }
                    }

                    this.userControlRichTextOutput2.Texto += bldm.imprimirHierarquicoReferenciado(indiceHierar, strVariavelID, "Índice Análise Hierárquica Referenciada", nomeVariavelIdentificadora, strVariaveisSelecionadas, pesos, lambdaMAX, IC, RC, strEstatistica, strPadroniza);
                    if (ckbIncluirNovasVariaveisTabelaDados.Checked)
                    {
                        m_gridview_externo.DataSource = ((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy();

                        this.DataSetExterno.Tables.Clear();
                        this.DataSetExterno.Tables.Add(((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy());
                       
                        lblProgressBar.Visible = true;
                        lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
                    }
                    tabControl1.SelectedTab = tabPage5;
                }
                
                if (rbAHPbg.Checked)
                {
                    BLogicDecisaoMulticriterios bldm = new BLogicDecisaoMulticriterios("Análise Hierárquica B-G", BLogicDecisaoMulticriterios.Metodo.Geral);

                    //Passo 1: Passa a lista de variáveis para criar o indice
                    string[] strVariaveisSelecionadas = userControlSelecaoVariaveis1.VariaveisIndependentes;

                    //Passo 2: Calcula o indice
                    double lambdaMAX = 0;
                    double RC = 0;
                    double IC = 0;
                    double[,] pesos = new double[strVariaveisSelecionadas.GetLength(0), strVariaveisSelecionadas.GetLength(0)];
                    double[] indiceHierar = bldm.IndiceAnaliseHierarquicaBG(ref m_dt_tabela_dados, dataGridView1, strVariaveisSelecionadas, ref pesos, out lambdaMAX, out RC, out IC);

                    //Verifica se a variável indice algebrico existe no dataTable
                    string[] strVariavelID = new string[m_dt_tabela_dados.Rows.Count];
                    if (m_dt_tabela_dados.Columns.Contains("Índice Análise Hierárquica B-G"))
                    {
                        m_dt_tabela_dados.Columns.Remove("Índice Análise Hierárquica B-G");
                    }

                    if (m_dt_tabela_dados.Columns.Contains("Índice Análise Hierárquica B-G"))
                    {
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Análise Hierárquica B-G"] = indiceHierar[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    else
                    {
                        m_dt_tabela_dados.Columns.Add("Índice Análise Hierárquica B-G", typeof(double));
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Análise Hierárquica B-G"] = indiceHierar[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }

                    if (checkBoxLimparResultado.Checked) this.userControlRichTextOutput2.Texto = "";
                    this.userControlRichTextOutput2.Texto += bldm.imprimirHierarquicoBG(indiceHierar, strVariavelID, "Índice Análise Hierárquica B-G", nomeVariavelIdentificadora, strVariaveisSelecionadas, pesos, lambdaMAX, IC, RC);
                    
                    if (ckbIncluirNovasVariaveisTabelaDados.Checked)
                    {
                        m_gridview_externo.DataSource = ((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy();

                        this.DataSetExterno.Tables.Clear();
                        this.DataSetExterno.Tables.Add(((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy());

                      
                        lblProgressBar.Visible = true;
                        lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
                    }
                    tabControl1.SelectedTab = tabPage5;
                }

                if (rbPromethe1.Checked)
                {
                    BLogicDecisaoMulticriterios bldm = new BLogicDecisaoMulticriterios("Prométhée 1", BLogicDecisaoMulticriterios.Metodo.Geral);

                    //Passo 1: Passa a lista de variáveis para criar o indice
                    string[] strVariaveisSelecionadas = userControlSelecaoVariaveis1.VariaveisIndependentes;

                    //Passo 2: Calcula o indice
                    double[] pesos = new double[strVariaveisSelecionadas.GetLength(0)];
                    double[] variancias = new double[strVariaveisSelecionadas.GetLength(0)];
                    double[] indicePromethee1 = bldm.IndicePromethee1(ref progressBar1, ref m_dt_tabela_dados, dataGridView1, strVariaveisSelecionadas,out variancias);

                    //Verifica se a variável indice algebrico existe no dataTable
                    string[] strVariavelID = new string[m_dt_tabela_dados.Rows.Count];
                    if (m_dt_tabela_dados.Columns.Contains("Índice Prométhée 1"))
                    {
                        m_dt_tabela_dados.Columns.Remove("Índice Prométhée 1");
                    }
                    
                    if (m_dt_tabela_dados.Columns.Contains("Índice Prométhée 1"))
                    {
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Prométhée 1"] = indicePromethee1[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    else
                    {
                        m_dt_tabela_dados.Columns.Add("Índice Prométhée 1", typeof(double));
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Prométhée 1"] = indicePromethee1[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    
                    if (checkBoxLimparResultado.Checked) this.userControlRichTextOutput2.Texto = "";

                    //Passa os pesos:
                    for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
                    {
                        pesos[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value);
                    }

                    this.userControlRichTextOutput2.Texto += bldm.imprimirPromethee1(dataGridView1, m_dt_tabela_dados, indicePromethee1, strVariavelID, "Índice Prométhée 1", nomeVariavelIdentificadora, strVariaveisSelecionadas, pesos, variancias);
                    if (ckbIncluirNovasVariaveisTabelaDados.Checked)
                    {
                        m_gridview_externo.DataSource = ((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy();

                        this.DataSetExterno.Tables.Clear();
                        this.DataSetExterno.Tables.Add(((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy());
                       
                        lblProgressBar.Visible = true;
                        lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
                    }
                    tabControl1.SelectedTab = tabPage5;
                }
                
                if (rbPromethe2.Checked)
                {
                    BLogicDecisaoMulticriterios bldm = new BLogicDecisaoMulticriterios("Prométhée 2", BLogicDecisaoMulticriterios.Metodo.Geral);

                    //Passo 1: Passa a lista de variáveis para criar o indice
                    string[] strVariaveisSelecionadas = userControlSelecaoVariaveis1.VariaveisIndependentes;

                    //Passo 2: Calcula o indice
                    double[] pesos = new double[strVariaveisSelecionadas.GetLength(0)];
                    double[] variancias = new double[strVariaveisSelecionadas.GetLength(0)];
                    double[] indicePromethee1 = bldm.IndicePromethee2(ref progressBar1, ref m_dt_tabela_dados, dataGridView1, strVariaveisSelecionadas, out variancias);

                    //Verifica se a variável indice algebrico existe no dataTable
                    string[] strVariavelID = new string[m_dt_tabela_dados.Rows.Count];
                    if (m_dt_tabela_dados.Columns.Contains("Índice Prométhée 2"))
                    {
                        m_dt_tabela_dados.Columns.Remove("Índice Prométhée 2");
                    }
                    
                    if (m_dt_tabela_dados.Columns.Contains("Índice Prométhée 2"))
                    {
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Prométhée 2"] = indicePromethee1[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    else
                    {
                        m_dt_tabela_dados.Columns.Add("Índice Prométhée 2", typeof(double));
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Prométhée 2"] = indicePromethee1[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    
                    if (checkBoxLimparResultado.Checked) this.userControlRichTextOutput2.Texto = "";

                    //Passa os pesos:
                    for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
                    {
                        pesos[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value);
                    }

                    this.userControlRichTextOutput2.Texto += bldm.imprimirPromethee1(dataGridView1, m_dt_tabela_dados, indicePromethee1, strVariavelID, "Índice Prométhée 2", nomeVariavelIdentificadora, strVariaveisSelecionadas, pesos, variancias);
                    if (ckbIncluirNovasVariaveisTabelaDados.Checked)
                    {
                        m_gridview_externo.DataSource = ((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy();

                        this.DataSetExterno.Tables.Clear();
                        this.DataSetExterno.Tables.Add(((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy());
                      
                        lblProgressBar.Visible = true;
                        lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
                    }
                    tabControl1.SelectedTab = tabPage5;
                }
                
                if (radioButton5.Checked)
                {
                    BLogicDecisaoMulticriterios bldm = new BLogicDecisaoMulticriterios("Prométhée 3", BLogicDecisaoMulticriterios.Metodo.Geral);

                    //Passo 1: Passa a lista de variáveis para criar o indice
                    string[] strVariaveisSelecionadas = userControlSelecaoVariaveis1.VariaveisIndependentes;

                    double alfa = Convert.ToDouble(numericUpDown1.Value);

                    //Passo 2: Calcula o indice
                    double[] pesos = new double[strVariaveisSelecionadas.GetLength(0)];
                    double[] variancias = new double[strVariaveisSelecionadas.GetLength(0)];
                    double[] indicePromethee1 = bldm.IndicePromethee3(ref progressBar1, ref m_dt_tabela_dados, dataGridView1, strVariaveisSelecionadas, alfa,out variancias);

                    //Verifica se a variável indice algebrico existe no dataTable
                    string[] strVariavelID = new string[m_dt_tabela_dados.Rows.Count];
                    if (m_dt_tabela_dados.Columns.Contains("Índice Prométhée 3"))
                    {
                        m_dt_tabela_dados.Columns.Remove("Índice Prométhée 3");
                    }
                    if (m_dt_tabela_dados.Columns.Contains("Índice Prométhée 3"))
                    {
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Prométhée 3"] = indicePromethee1[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    else
                    {
                        m_dt_tabela_dados.Columns.Add("Índice Prométhée 3", typeof(double));
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Prométhée 3"] = indicePromethee1[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    if (checkBoxLimparResultado.Checked) this.userControlRichTextOutput2.Texto = "";

                    //Passa os pesos:
                    for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
                    {
                        pesos[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value);
                    }

                    this.userControlRichTextOutput2.Texto += bldm.imprimirPromethee3(dataGridView1, m_dt_tabela_dados, indicePromethee1, strVariavelID, "Índice Prométhée 3", nomeVariavelIdentificadora, strVariaveisSelecionadas, pesos, alfa, variancias);
                    if (ckbIncluirNovasVariaveisTabelaDados.Checked)
                    {
                        m_gridview_externo.DataSource = ((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy();

                        this.DataSetExterno.Tables.Clear();
                        this.DataSetExterno.Tables.Add(((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy());

                       
                        lblProgressBar.Visible = true;
                        lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
                    }
                    tabControl1.SelectedTab = tabPage5;
                }
                
                if (rbPromethe4.Checked)
                {
                    BLogicDecisaoMulticriterios bldm = new BLogicDecisaoMulticriterios("Prométhée 4", BLogicDecisaoMulticriterios.Metodo.Geral);

                    //Passo 1: Passa a lista de variáveis para criar o indice
                    string[] strVariaveisSelecionadas = userControlSelecaoVariaveis1.VariaveisIndependentes;

                    //Passo 2: Calcula o indice
                    double[] pesos = new double[strVariaveisSelecionadas.GetLength(0)];
                    double[] variancias = new double[strVariaveisSelecionadas.GetLength(0)];
                    double[] indicePromethee1 = bldm.IndicePromethee4(ref progressBar1, ref m_dt_tabela_dados, dataGridView1, strVariaveisSelecionadas, out variancias);

                    //Verifica se a variável indice algebrico existe no dataTable
                    string[] strVariavelID = new string[m_dt_tabela_dados.Rows.Count];
                    if (m_dt_tabela_dados.Columns.Contains("Índice Prométhée 4"))
                    {
                        m_dt_tabela_dados.Columns.Remove("Índice Prométhée 4");
                    }
                    
                    if (m_dt_tabela_dados.Columns.Contains("Índice Prométhée 4"))
                    {
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Prométhée 4"] = indicePromethee1[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    else
                    {
                        m_dt_tabela_dados.Columns.Add("Índice Prométhée 4", typeof(double));
                        for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                        {
                            m_dt_tabela_dados.Rows[i]["Índice Prométhée 4"] = indicePromethee1[i];
                            strVariavelID[i] = m_dt_tabela_dados.Rows[i][nomeVariavelIdentificadora].ToString();
                        }
                    }
                    if (checkBoxLimparResultado.Checked) this.userControlRichTextOutput2.Texto = "";

                    //Passa os pesos:
                    for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
                    {
                        pesos[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value);
                    }

                    this.userControlRichTextOutput2.Texto += bldm.imprimirPromethee4(dataGridView1, m_dt_tabela_dados, indicePromethee1, strVariavelID, "Índice Prométhée 4", nomeVariavelIdentificadora, strVariaveisSelecionadas, pesos, variancias);
                    if (ckbIncluirNovasVariaveisTabelaDados.Checked)
                    {
                        m_gridview_externo.DataSource = ((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy();

                        this.DataSetExterno.Tables.Clear();
                        this.DataSetExterno.Tables.Add(((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy());

                       
                        lblProgressBar.Visible = true;
                        lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
                    }
                    tabControl1.SelectedTab = tabPage5;
                } 
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Erro.",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void buttonAdicionarCriterio_Click(object sender, EventArgs e)
        {
            adicionarCriterio();
        }

        private void buttonApagarCriterio_Click(object sender, EventArgs e)
        {
            apagarCriterio(treeViewHierarquico.SelectedNode);
        }

        private void buttonEditarMatriz_Click(object sender, EventArgs e)
        {
            editarMatrizComparacao();
        }

        private void buttonCriarAlternativa_Click(object sender, EventArgs e)
        {
            criarAlternativa();
        }

        private void buttonEditarMatrizAlternativas_Click(object sender, EventArgs e)
        {
            BLogicDecisaoMulticriterios.elementoArvore elemento = bldm.arvore.retornaElemento(treeViewHierarquico.SelectedNode.FullPath.Split('\\'));

            if (bldm.arvore.alternativas.GetLength(0) == 0)
            {
                DialogResult result1 = MessageBox.Show("Nenhuma alternativa foi adicionada",
                                    "Editar matriz de comparação de alternativas",
                                    MessageBoxButtons.OK);
                return;
            }

            if (elemento.isAlternativa)
            {
                DialogResult result1 = MessageBox.Show("O elemento selecionado é uma alternativa",
                                        "Adicionar critério",
                                        MessageBoxButtons.OK);
                return;
            }

            if (elemento.filhos.GetLength(0) == 0)
            {
                bldm.arvore.addCriterioFinal(elemento);

                for (int i = 0; i < bldm.arvore.alternativas.GetLength(0); i++)
                {
                    if (radioButtonHierarquicoClassico.Checked)
                        elemento.addFilho(bldm.arvore.alternativas[i], 1.0);
                    else
                        if (radioButtonHierarquicoMultiplicativo.Checked)
                            elemento.addFilho(bldm.arvore.alternativas[i], 0.0);
                        else
                            elemento.addFilho(bldm.arvore.alternativas[i], 1.0);

                    elemento.filhos[i].isAlternativa = true;
                    treeViewHierarquico.SelectedNode.Nodes.Add(bldm.arvore.alternativas[i]);
                }

                editarMatrizComparacao();
            }
            else
                if (elemento.filhos[0].isAlternativa == false)
                {
                    DialogResult result1 = MessageBox.Show("Um mesmo nível não pode conter alternativas e critérios",
                                        "Editar matriz de comparação de alternativas",
                                        MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    treeViewHierarquico.SelectedNode.Nodes.Clear();
                    for (int i = 0; i < bldm.arvore.alternativas.GetLength(0); i++)
                        treeViewHierarquico.SelectedNode.Nodes.Add(bldm.arvore.alternativas[i]);

                    editarMatrizComparacao();
                }
        }

        private void buttonExecutarHierarquico_Click(object sender, EventArgs e)
        {
            if (radioButtonHierarquicoClassico.Checked)
            {
                bldm.resultado = new BLogicDecisaoMulticriterios.Resultado();
                bldm.arvore.niveis = new BLogicDecisaoMulticriterios.elementoArvore[0][];
                bldm.arvore.outText = "";
                bldm.executarHierarquicoClassico();
            }

            if (radioButtonHierarquicoMultiplicativo.Checked)
            {
                bldm.resultado = new BLogicDecisaoMulticriterios.Resultado();
                bldm.arvore.niveis = new BLogicDecisaoMulticriterios.elementoArvore[0][];
                bldm.arvore.outText = "";
                bldm.executarHierarquicoMultiplicativo();
            }

            bldm.resultado.imprimirHierarquico();

            if (checkBoxImprimirArvoreHierarquica.Checked)
                bldm.arvore.imprimirArvore();

            if (checkBoxImprimirInconsistencia.Checked)
            {
                bldm.arvore.calculaConsistenciaArvore();
                bldm.arvore.imprimeRazaoConsistencia();
            }

            if (checkBoxLimparResultado.Checked)
                this.userControlRichTextOutput1.Texto = "";

            this.userControlRichTextOutput1.Texto += bldm.resultado.outText;
            this.userControlRichTextOutput1.Texto += bldm.arvore.outText;

            if (!this.tabControl1.TabPages.Contains(tabPage2))
            {
                tabControl1.TabPages.Add(tabPage2);
            }
            tabControl1.SelectedTab = tabPage2;

        }

        private void buttonExportarArvore_Click(object sender, EventArgs e)
        {
            exportarArvore();
        }

        #endregion

        #region eventos de radioButtons e checkBoxes          

        private void radioButtonAlgebricoSimples_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAlgebricoSimples.Checked)
            {
                dataGridViewAlgebrico.Columns[1].ReadOnly = true;
                for (int i = 0; i < dataGridViewAlgebrico.Rows.Count; i++)
                    dataGridViewAlgebrico.Rows[i].Cells[1].Value = "1.0";
            }
        }

        private void radioButtonAlgebricoPonderado_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAlgebricoPonderado.Checked)
                dataGridViewAlgebrico.Columns[1].ReadOnly = false;
        }

        private void radioButtonHierarquicoClassico_CheckedChanged(object sender, EventArgs e)
        {
            if (bldm != null)
                if (radioButtonHierarquicoClassico.Checked)
                    if (bldm != null)
                        if (bldm.entrada != null)
                        {
                            if (bldm.entrada.metodo != BLogicDecisaoMulticriterios.Metodo.HierarquicoClassico)
                            {
                                DialogResult result1 = MessageBox.Show("Deseja mudar o método de análise? As matrizes de comparação da árvore atual serão convertidas",
                                "Mudança de método",
                               MessageBoxButtons.YesNo);

                                if (result1 == DialogResult.No)
                                {
                                    switch (bldm.entrada.metodo)
                                    {
                                        case BLogicDecisaoMulticriterios.Metodo.HierarquicoClassico:
                                            radioButtonHierarquicoClassico.Checked = true;
                                            break;
                                        case BLogicDecisaoMulticriterios.Metodo.HierarquicoMultiplicativo:
                                            radioButtonHierarquicoMultiplicativo.Checked = true;
                                            break;
                                        default:
                                            radioButtonHierarquicoClassico.Checked = true;
                                            break;
                                    }
                                    return;
                                }
                                else
                                {
                                    bldm.mudarMetodo(BLogicDecisaoMulticriterios.Metodo.HierarquicoClassico);
                                    bldm.entrada.metodo = BLogicDecisaoMulticriterios.Metodo.HierarquicoClassico;
                                    bldm.arvore.metodo = BLogicDecisaoMulticriterios.Metodo.HierarquicoClassico;
                                }
                            }
                        }

            dataGridViewMatrizComparacao.Hide();
            labelMatrizComparacao.Hide();
        }

        private void radioButtonHierarquicoMultiplicativo_CheckedChanged(object sender, EventArgs e)
        {
            if (bldm != null)
                if (radioButtonHierarquicoMultiplicativo.Checked)
                    if (bldm.entrada.metodo != BLogicDecisaoMulticriterios.Metodo.HierarquicoMultiplicativo)
                    {
                        DialogResult result1 = MessageBox.Show("Deseja mudar o método de análise? As matrizes de comparação da árvore atual serão convertidas",
                        "Mudança de método",
                        MessageBoxButtons.YesNo);

                        if (result1 == DialogResult.No)
                        {
                            switch (bldm.entrada.metodo)
                            {
                                case BLogicDecisaoMulticriterios.Metodo.HierarquicoClassico:
                                    radioButtonHierarquicoClassico.Checked = true;
                                    break;
                                case BLogicDecisaoMulticriterios.Metodo.HierarquicoMultiplicativo:
                                    radioButtonHierarquicoMultiplicativo.Checked = true;
                                    break;
                                default:
                                    radioButtonHierarquicoClassico.Checked = true;
                                    break;
                            }
                            return;
                        }
                        else
                        {
                            bldm.mudarMetodo(BLogicDecisaoMulticriterios.Metodo.HierarquicoMultiplicativo);
                            bldm.entrada.metodo = BLogicDecisaoMulticriterios.Metodo.HierarquicoMultiplicativo;
                            bldm.arvore.metodo = BLogicDecisaoMulticriterios.Metodo.HierarquicoMultiplicativo;
                        }
                    }

            dataGridViewMatrizComparacao.Hide();
            labelMatrizComparacao.Hide();

            dataGridViewMatrizComparacao.Hide();
            labelMatrizComparacao.Hide();
        }

        #endregion

        #region demais eventos

        private void userControlSelecaoVariaveis1_Load(object sender, EventArgs e)
        {
            System.EventHandler evento = new System.EventHandler(userControlSelecaoVariaveis1_Changed);
            userControlSelecaoVariaveis1.addEvent(evento);
            userControlSelecaoVariaveis1.PermiteSelecaoMultipla = true;

        }

        private void userControlSelecaoVariaveis1_Changed(object sender, EventArgs e)
        {
            if (radioButtonAlgebrico.Checked)
            {
                dataGridViewAlgebrico.Rows.Clear();
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.GetLength(0) > 0)
                    dataGridViewAlgebrico.Rows.Add(userControlSelecaoVariaveis1.VariaveisIndependentes.GetLength(0));
                for (int i = 0; i < dataGridViewAlgebrico.Rows.Count; i++)
                {
                    dataGridViewAlgebrico.Rows[i].Cells[0].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[i];
                    dataGridViewAlgebrico.Rows[i].Cells[1].Value = "1.0";
                }
            }
        }

        #endregion

        #region métodos auxiliares

        void atualizaTreeViewHierarquico()
        {
            treeViewHierarquico.Nodes.Clear();

            treeViewHierarquico.Nodes.Add(bldm.arvore.nome);

            if (bldm.arvore.filhos != null)
                for (int i = 0; i < bldm.arvore.filhos.GetLength(0); i++)
                    adicionaFilho(treeViewHierarquico.Nodes[0], bldm.arvore.filhos[i]);
        }

        void adicionaFilho(TreeNode no, BLogicDecisaoMulticriterios.elementoArvore filho)
        {
            no.Nodes.Add(filho.nome);

            if (filho.filhos != null)
                for (int i = 0; i < filho.filhos.GetLength(0); i++)
                    adicionaFilho(no.Nodes[no.Nodes.Count - 1], filho.filhos[i]);
        }

        void atualizalistBoxAlternativas()
        {
            if (bldm.arvore.alternativas != null)
                for (int i = 0; i < bldm.arvore.alternativas.GetLength(0); i++)
                    listBoxAlternativas.Items.Add(bldm.arvore.alternativas[i]);
        }

        private void importarArvoreHierarquica()
        {
            bool importouArvore = false;
            apagarArvoreHierarquica();

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

                openFileDialog.Filter = "Arvore (*.arv)|*.arv|All Files (*.*)|*.*";

                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    importouArvore = BLogicDecisaoMulticriterios.importarArvore(ref bldm, openFileDialog.FileName);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (importouArvore)
            {
                atualizaTreeViewHierarquico();
                atualizalistBoxAlternativas();

                switch (bldm.arvore.metodo)
                {
                    case BLogicDecisaoMulticriterios.Metodo.HierarquicoClassico:
                        if (!radioButtonHierarquicoClassico.Checked)
                        {
                            DialogResult result1 = MessageBox.Show("O método selecionado é diferente do método da árvore importada. Deseja mudar o método selecionado? Caso contrário, as matrizes de comparação da árvore serão convertidas",
                            "Importar árvore",
                            MessageBoxButtons.YesNo);

                            if (result1 == DialogResult.Yes)
                            {
                                bldm.entrada.metodo = BLogicDecisaoMulticriterios.Metodo.HierarquicoClassico;
                                radioButtonHierarquicoClassico.Checked = true;
                            }
                            else
                            {
                                if (radioButtonHierarquicoMultiplicativo.Checked)
                                {
                                    bldm.mudarMetodo(BLogicDecisaoMulticriterios.Metodo.HierarquicoMultiplicativo);
                                    bldm.entrada.metodo = BLogicDecisaoMulticriterios.Metodo.HierarquicoMultiplicativo;
                                    bldm.arvore.metodo = BLogicDecisaoMulticriterios.Metodo.HierarquicoMultiplicativo;
                                }
                            }
                        }
                        break;
                    case BLogicDecisaoMulticriterios.Metodo.HierarquicoMultiplicativo:
                        if (!radioButtonHierarquicoMultiplicativo.Checked)
                        {
                            DialogResult result1 = MessageBox.Show("O método selecionado é diferente do método da árvore importada. Deseja mudar o método de análise? Caso contrário, as matrizes de comparação da árvore serão reinicializadas",
                            "Importar árvore",
                            MessageBoxButtons.YesNo);

                            if (result1 == DialogResult.Yes)
                            {
                                bldm.entrada.metodo = BLogicDecisaoMulticriterios.Metodo.HierarquicoMultiplicativo;
                                radioButtonHierarquicoMultiplicativo.Checked = true;
                            }
                            else
                            {
                                if (radioButtonHierarquicoClassico.Checked)
                                {
                                    bldm.mudarMetodo(BLogicDecisaoMulticriterios.Metodo.HierarquicoClassico);
                                    bldm.entrada.metodo = BLogicDecisaoMulticriterios.Metodo.HierarquicoClassico;
                                    bldm.arvore.metodo = BLogicDecisaoMulticriterios.Metodo.HierarquicoClassico;
                                }
                            }
                        }
                        break;
                    default:
                        radioButtonHierarquicoClassico.Checked = true;
                        bldm.arvore.inicializaMatrizComparacao(1.1);
                        bldm.entrada.metodo = BLogicDecisaoMulticriterios.Metodo.HierarquicoClassico;
                        bldm.arvore.metodo = BLogicDecisaoMulticriterios.Metodo.HierarquicoClassico;
                        break;
                }
                return;

            }
        }

        private void apagarArvoreHierarquica()
        {
            treeViewHierarquico.Nodes.Clear();
            treeViewHierarquico.Nodes.Add("Objetivo");
            treeViewHierarquico.SelectedNode = treeViewHierarquico.Nodes[0];

            listBoxAlternativas.Items.Clear();
            dataGridViewMatrizComparacao.Hide();
            labelMatrizComparacao.Hide();

            BLogicDecisaoMulticriterios.clear(ref bldm);
        }

        private void exportarArvore()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                saveFileDialog.Filter = "Arvore (*.arv)|*.arv|All Files (*.*)|*.*";

                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    bldm.arvore.exportarArvore(saveFileDialog.FileName);
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool apagarAlternativa(int index)
        {
            dataGridViewMatrizComparacao.Hide();
            labelMatrizComparacao.Hide();

            bool removeuAlternativa = false;

            if ((listBoxAlternativas.Items.Count - 1 < index) || index < 0)
                return false;

            removeuAlternativa = bldm.arvore.removerAlternativa(listBoxAlternativas.Items[index].ToString());
            listBoxAlternativas.Items.RemoveAt(index);

            TreeNode no;

            for (int i = 0; i < bldm.arvore.criteriosFinais.GetLength(0); i++)
            {
                no = retornaNo(bldm.arvore.criteriosFinais[i].fullPath);
                no.Nodes.Clear();

                for (int j = 0; j < bldm.arvore.alternativas.GetLength(0); j++)
                    no.Nodes.Add(bldm.arvore.alternativas[j]);
            }

            removeuAlternativa = true;

            return removeuAlternativa;
        }

        private void adicionarCriterio()
        {

            BLogicDecisaoMulticriterios.elementoArvore elemento = bldm.arvore.retornaElemento(treeViewHierarquico.SelectedNode.FullPath.Split('\\'));

            if (elemento.filhos.GetLength(0) > 0)
                if (elemento.filhos[0].isAlternativa == true)
                {
                    DialogResult result1 = MessageBox.Show("Um mesmo nível não pode conter alternativas e critérios",
                                        "Adicionar critério",
                                        MessageBoxButtons.OK);
                    return;
                }

            if (elemento.isAlternativa)
            {
                DialogResult result1 = MessageBox.Show("Uma alternativa não pode conter subcritérios",
                                        "Adicionar critério",
                                        MessageBoxButtons.OK);
                return;
            }

            if (textBoxNomeCriterio.Text.ToString() == "")
                return;

            if (radioButtonHierarquicoClassico.Checked)
                elemento.addFilho(textBoxNomeCriterio.Text, 1.0);
            else
                if (radioButtonHierarquicoMultiplicativo.Checked)
                    elemento.addFilho(textBoxNomeCriterio.Text, 0.0);
                else
                    elemento.addFilho(textBoxNomeCriterio.Text, 1.0);

            treeViewHierarquico.SelectedNode.Nodes.Add(textBoxNomeCriterio.Text, textBoxNomeCriterio.Text);

            textBoxNomeCriterio.Text = "";

            dataGridViewMatrizComparacao.Hide();
            labelMatrizComparacao.Hide();
        }

        private bool apagarCriterio(TreeNode no)
        {
            BLogicDecisaoMulticriterios.elementoArvore elementoApagar = bldm.arvore.retornaElemento(no.FullPath.Split('\\'));
            dataGridViewMatrizComparacao.Hide();
            labelMatrizComparacao.Hide();

            if (elementoApagar == null)
                return false;

            if (elementoApagar.isAlternativa)
            {
                DialogResult result1 = MessageBox.Show("O nó selecionado é uma alternativa, não um critério. Para apagar uma alternativa utilize a função 'apagar alternativa'",
                                    "Apagar critério",
                                    MessageBoxButtons.OK);
                return false;
            }

            if (treeViewHierarquico.Nodes.Contains(no))
            {
                DialogResult result1 = MessageBox.Show("A árvore não contém o no escolhido",
                                    "Apagar critério",
                                    MessageBoxButtons.OK);
                return false;
            }

            treeViewHierarquico.Nodes.Remove(no);

            bldm.arvore.removerElemento(elementoApagar);

            return true;
        }

        private void editarMatrizComparacao()
        {
            BLogicDecisaoMulticriterios.elementoArvore elemento;
            bool isClear = false;

            dataGridViewMatrizComparacao.ReadOnly = false;
            dataGridViewMatrizComparacao.AllowUserToAddRows = false;
            dataGridViewMatrizComparacao.AllowUserToDeleteRows = false;
            dataGridViewMatrizComparacao.AllowUserToOrderColumns = false;

            dataGridViewMatrizComparacao.Show();
            elemento = bldm.arvore.retornaElemento(treeViewHierarquico.SelectedNode.FullPath.Split('\\'));

            bldm.arvore.elementoSelecionado = elemento;

            labelMatrizComparacao.Show();

            if (elemento.filhos.GetLength(0) > 0)
                if (elemento.filhos[0].isAlternativa == true)
                    labelMatrizComparacao.Text = "Matriz de comparacao das alternativas segundo " + elemento.nome;
                else
                    if (elemento.nivel == 0)
                        labelMatrizComparacao.Text = "Matriz de comparacao do criterios de primeiro nível";
                    else
                        labelMatrizComparacao.Text = "Matriz de comparacao dos subcriterios de " + elemento.nome;
            else
                if (elemento.nivel == 0)
                    labelMatrizComparacao.Text = "Matriz de comparacao do criterios de primeiro nível (Não há critérios)";
                else
                    labelMatrizComparacao.Text = "Matriz de comparacao dos subcriterios de " + elemento.nome + " (Não há subcritérios)";

            dataGridViewMatrizComparacao.Rows.Clear();
            dataGridViewMatrizComparacao.Columns.Clear();
            isClear = true;

            if (isClear)
                if (elemento.isAlternativa)
                {
                    DialogResult result1 = MessageBox.Show("O nó selecionado é uma alternativa e não possui matriz de comparação",
                                        "Editar matriz de comparação",
                                        MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    for (int i = 0; i < elemento.filhos.GetLength(0); i++)
                    {
                        dataGridViewMatrizComparacao.Columns.Add(elemento.filhos[i].nome, elemento.filhos[i].nome);
                        dataGridViewMatrizComparacao.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    for (int i = 0; i < elemento.filhos.GetLength(0); i++)
                    {
                        dataGridViewMatrizComparacao.Rows.Add();
                        dataGridViewMatrizComparacao.Rows[i].HeaderCell.Value = elemento.filhos[i].nome;
                    }

                    for (int i = 0; i < elemento.filhos.GetLength(0); i++)
                        for (int j = 0; j < elemento.filhos.GetLength(0); j++)
                        {
                            if (radioButtonHierarquicoClassico.Checked)
                                dataGridViewMatrizComparacao.Rows[i].Cells[j].Value = BLogicDecisaoMulticriterios.double2saaty(elemento.matrizComparacao[i, j]);
                            else
                                if (radioButtonHierarquicoMultiplicativo.Checked)
                                    dataGridViewMatrizComparacao.Rows[i].Cells[j].Value = BLogicDecisaoMulticriterios.double2Lootsma(elemento.matrizComparacao[i, j]);
                                else
                                    dataGridViewMatrizComparacao.Rows[i].Cells[j].Value = BLogicDecisaoMulticriterios.double2saaty(elemento.matrizComparacao[i, j]);

                            if (i >= j)
                                dataGridViewMatrizComparacao.Rows[i].Cells[j].ReadOnly = true;
                        }
                }
        }

        private void criarAlternativa()
        {
            dataGridViewMatrizComparacao.Hide();
            labelMatrizComparacao.Hide();

            if (textBoxNomeAlternativa.Text == "")
                return;
            else
            {
                TreeNode no;
                bldm.arvore.addAlternativa(textBoxNomeAlternativa.Text);
                listBoxAlternativas.Items.Add(textBoxNomeAlternativa.Text);

                for (int i = 0; i < bldm.arvore.criteriosFinais.GetLength(0); i++)
                {
                    no = retornaNo(bldm.arvore.criteriosFinais[i].fullPath);
                    no.Nodes.Clear();
                    for (int j = 0; j < bldm.arvore.alternativas.GetLength(0); j++)
                        no.Nodes.Add(bldm.arvore.alternativas[j], bldm.arvore.alternativas[j]);
                }

                textBoxNomeAlternativa.Text = "";
            }
        }

        private TreeNode retornaNo(string fullPath)
        {
            TreeNode no;
            string[] fullPathDiv = fullPath.Split('\\');

            no = treeViewHierarquico.Nodes[0];

            if (no == null)
                return null;

            for (int i = 1; i < fullPathDiv.GetLength(0); i++)
            {
                bool achouNo = false;
                for (int j = 0; j < no.Nodes.Count; j++)
                    if (no.Nodes[j].Text == fullPathDiv[i])
                    {
                        no = no.Nodes[j];
                        achouNo = true;
                        break;
                    }
                if (!achouNo)
                    return null;

                if (no == null)
                    return null;
            }

            return no;
        }

        #endregion

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //Impede que mais de um radio button seja selecionado de cada vez
            radioButtonAlgebrico.Checked = false;
            rbComponentesPrincipais.Checked = false;
            radioButton5.Checked = false;
            rbPromethe1.Checked = false;
            rbPromethe2.Checked = false;
            rbPromethe4.Checked = false;

            try
            {
                if (!radioButton5.Checked) label2.Visible = false;
                if (!radioButton5.Checked) numericUpDown1.Visible = false;
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.Length < 2)
                {
                    MessageBox.Show("É necessário pelo menos duas variáveis para realizar a análise.", "Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (!radioButton1.Checked)
                    {
                        dataGridView1.Columns.Clear();
                        dataGridView1.Rows.Clear();
                    }
                    else
                    {
                        buttonExecutar.Enabled = true;
                        
                        //Preenche a legenda do Mapa
                        if (dataGridView1.ColumnCount == 0)
                        {
                            //Adiciona nome da variável
                            DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                            txtCor = new DataGridViewTextBoxColumn();
                            txtCor.Width = 225;
                            txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                            txtCor.ReadOnly = true;
                            txtCor.HeaderText = "Variável";
                            txtCor.DisplayIndex = 2;
                            txtCor.DefaultCellStyle.BackColor = Color.White;
                            dataGridView1.Columns.Insert(0, txtCor);

                            //Adiciona coluna de peso.
                            DataGridViewTrackBarColumn txtPeso = new DataGridViewTrackBarColumn();
                            txtPeso = new DataGridViewTrackBarColumn();
                            txtPeso.Width = 100;
                            txtPeso.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                            txtPeso.ReadOnly = false;
                            txtPeso.HeaderText = "Preferência";
                            txtPeso.DisplayIndex = 2;
                            dataGridView1.Columns.Insert(1, txtPeso);

                            //Adiciona nome da variável
                            DataGridViewTextBoxColumn txtCor2 = new DataGridViewTextBoxColumn();
                            txtCor2 = new DataGridViewTextBoxColumn();
                            txtCor2.Width = 225;
                            txtCor2.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                            txtCor2.ReadOnly = true;
                            txtCor2.HeaderText = "Variável";
                            txtCor2.DisplayIndex = 2;
                            txtCor2.DefaultCellStyle.BackColor = Color.White;
                            dataGridView1.Columns.Insert(2, txtCor2);
                        }

                        int contador = 0;
                        for (int i = 0; i < userControlSelecaoVariaveis1.VariaveisIndependentes.Length - 1; i++)
                        {
                            for (int j = i + 1; j < userControlSelecaoVariaveis1.VariaveisIndependentes.Length; j++)
                            {
                                dataGridView1.Rows.Add();
                                dataGridView1.Rows[contador].Cells[0].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[i];
                                dataGridView1.Rows[contador].Cells[1].Value = 0;
                                dataGridView1.Rows[contador].Cells[2].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[j];
                                contador++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButtonAlgebrico_CheckedChanged_1(object sender, EventArgs e)
        {
            //Impede que mais de um radio button seja selecionado de cada vez
            radioButton5.Checked = false;
            radioButtonHierarquico.Checked = false;
            rbAHPbg.Checked = false;
            rbAHPReferenciado.Checked = false;
            rbPromethe1.Checked = false;
            rbPromethe2.Checked = false;
            radioButton1.Checked = false;
            rbPromethe4.Checked = false;

            try
            {
                if (!radioButton5.Checked) label2.Visible = false;
                if (!radioButton5.Checked) numericUpDown1.Visible = false;
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.Length > 0)
                {
                    buttonExecutar.Enabled = true;
                }
                else
                {
                    buttonExecutar.Enabled = false;
                }
                if (!radioButtonAlgebrico.Checked)
                {
                    dataGridView1.Columns.Clear();
                    dataGridView1.Rows.Clear();
                }
                else
                {
                    buttonExecutar.Enabled = true;
                    //Preenche a legenda do Mapa
                    if (dataGridView1.ColumnCount == 0)
                    {
                        //Adiciona nome da variável
                        DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                        txtCor = new DataGridViewTextBoxColumn();
                        txtCor.Width = 450;
                        txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor.ReadOnly = true;
                        txtCor.HeaderText = "Variável";
                        txtCor.DisplayIndex = 2;
                        txtCor.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(0, txtCor);

                        //Adiciona coluna de peso.
                        DataGridViewNumericUpDownColumn txtPeso = new DataGridViewNumericUpDownColumn();
                        txtPeso = new DataGridViewNumericUpDownColumn();
                        txtPeso.Width = 100;
                        txtPeso.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtPeso.ReadOnly = false;
                        txtPeso.HeaderText = "Peso";
                        txtPeso.DisplayIndex = 2;
                        dataGridView1.Columns.Insert(1, txtPeso);

                        DataGridViewCheckBoxColumn chkSelecionar = new DataGridViewCheckBoxColumn();
                        chkSelecionar = new DataGridViewCheckBoxColumn();
                        chkSelecionar.Width = 70;
                        chkSelecionar.HeaderText = "Padronizar";
                        chkSelecionar.DisplayIndex = 2;
                        dataGridView1.Columns.Insert(2, chkSelecionar);
                    }

                    for (int i = 0; i < userControlSelecaoVariaveis1.VariaveisIndependentes.Length; i++)
                    {
                        if (dataGridView1.Rows.Count < userControlSelecaoVariaveis1.VariaveisIndependentes.Length) dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[i];
                        dataGridView1.Rows[i].Cells[1].Value = 1;
                        dataGridView1.Rows[i].Cells[2].Value = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButtonHierarquico_CheckedChanged_1(object sender, EventArgs e)
        {
            //Impede que mais de um radio button seja selecionado de cada vez
            radioButtonAlgebrico.Checked = false;
            rbComponentesPrincipais.Checked = false;
            radioButton5.Checked = false;
            rbPromethe1.Checked = false;
            rbPromethe2.Checked = false;
            rbPromethe4.Checked = false;
            try
            {
                if (!radioButton5.Checked) label2.Visible = false;
                if (!radioButton5.Checked) numericUpDown1.Visible = false;
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.Length > 0)
                {
                    buttonExecutar.Enabled = true;
                }
                else
                {
                    buttonExecutar.Enabled = false;
                }
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.Length < 2)
                {
                    MessageBox.Show("É necessário pelo menos duas variáveis para realizar a análise.", "Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (!radioButtonHierarquico.Checked)
                    {
                        dataGridView1.Columns.Clear();
                        dataGridView1.Rows.Clear();
                    }
                    else
                    {
                        buttonExecutar.Enabled = true;
                        //Preenche a legenda do Mapa
                        if (dataGridView1.ColumnCount == 0)
                        {
                            //Adiciona nome da variável
                            DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                            txtCor = new DataGridViewTextBoxColumn();
                            txtCor.Width = 225;
                            txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                            txtCor.ReadOnly = true;
                            txtCor.HeaderText = "Variável";
                            txtCor.DisplayIndex = 2;
                            txtCor.DefaultCellStyle.BackColor = Color.White;
                            dataGridView1.Columns.Insert(0, txtCor);

                            //Adiciona coluna de peso.
                            DataGridViewTrackBarColumn txtPeso = new DataGridViewTrackBarColumn();
                            txtPeso = new DataGridViewTrackBarColumn();
                            txtPeso.Width = 100;
                            txtPeso.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                            txtPeso.ReadOnly = false;
                            txtPeso.HeaderText = "Preferência";
                            txtPeso.DisplayIndex = 2;
                            dataGridView1.Columns.Insert(1, txtPeso);

                            //Adiciona nome da variável
                            DataGridViewTextBoxColumn txtCor2 = new DataGridViewTextBoxColumn();
                            txtCor2 = new DataGridViewTextBoxColumn();
                            txtCor2.Width = 225;
                            txtCor2.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                            txtCor2.ReadOnly = true;
                            txtCor2.HeaderText = "Variável";
                            txtCor2.DisplayIndex = 2;
                            txtCor2.DefaultCellStyle.BackColor = Color.White;
                            dataGridView1.Columns.Insert(2, txtCor2);
                        }

                        int contador = 0;
                        for (int i = 0; i < userControlSelecaoVariaveis1.VariaveisIndependentes.Length - 1; i++)
                        {
                            for (int j = i + 1; j < userControlSelecaoVariaveis1.VariaveisIndependentes.Length; j++)
                            {
                                dataGridView1.Rows.Add();
                                dataGridView1.Rows[contador].Cells[0].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[i];
                                dataGridView1.Rows[contador].Cells[1].Value = 0;
                                dataGridView1.Rows[contador].Cells[2].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[j];
                                contador++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbAHPbg_CheckedChanged(object sender, EventArgs e)
        {
            //Impede que mais de um radio button seja selecionado de cada vez
            radioButtonAlgebrico.Checked = false;
            rbComponentesPrincipais.Checked = false;
            radioButton5.Checked = false;
            rbPromethe1.Checked = false;
            rbPromethe2.Checked = false;
            rbPromethe4.Checked = false;
            try
            {
                if (!radioButton5.Checked) label2.Visible = false;
                if (!radioButton5.Checked) numericUpDown1.Visible = false;
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.Length > 0)
                {
                    buttonExecutar.Enabled = true;
                }
                else
                {
                    buttonExecutar.Enabled = false;
                }
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.Length < 2)
                {
                    MessageBox.Show("É necessário pelo menos duas variáveis para realizar a análise.", "Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (!rbAHPbg.Checked)
                    {
                        dataGridView1.Columns.Clear();
                        dataGridView1.Rows.Clear();
                    }
                    else
                    {
                        buttonExecutar.Enabled = true;
                        //Preenche a legenda do Mapa
                        if (dataGridView1.ColumnCount == 0)
                        {
                            //Adiciona nome da variável
                            DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                            txtCor = new DataGridViewTextBoxColumn();
                            txtCor.Width = 225;
                            txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                            txtCor.ReadOnly = true;
                            txtCor.HeaderText = "Variável";
                            txtCor.DisplayIndex = 2;
                            txtCor.DefaultCellStyle.BackColor = Color.White;
                            dataGridView1.Columns.Insert(0, txtCor);

                            //Adiciona coluna de peso.
                            DataGridViewTrackBarColumn txtPeso = new DataGridViewTrackBarColumn();
                            txtPeso = new DataGridViewTrackBarColumn();
                            txtPeso.Width = 100;
                            txtPeso.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                            txtPeso.ReadOnly = false;
                            txtPeso.HeaderText = "Preferência";
                            txtPeso.DisplayIndex = 2;
                            dataGridView1.Columns.Insert(1, txtPeso);

                            //Adiciona nome da variável
                            DataGridViewTextBoxColumn txtCor2 = new DataGridViewTextBoxColumn();
                            txtCor2 = new DataGridViewTextBoxColumn();
                            txtCor2.Width = 225;
                            txtCor2.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                            txtCor2.ReadOnly = true;
                            txtCor2.HeaderText = "Variável";
                            txtCor2.DisplayIndex = 2;
                            txtCor2.DefaultCellStyle.BackColor = Color.White;
                            dataGridView1.Columns.Insert(2, txtCor2);
                        }

                        int contador = 0;
                        for (int i = 0; i < userControlSelecaoVariaveis1.VariaveisIndependentes.Length - 1; i++)
                        {
                            for (int j = i + 1; j < userControlSelecaoVariaveis1.VariaveisIndependentes.Length; j++)
                            {
                                dataGridView1.Rows.Add();
                                dataGridView1.Rows[contador].Cells[0].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[i];
                                dataGridView1.Rows[contador].Cells[1].Value = 0;
                                dataGridView1.Rows[contador].Cells[2].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[j];
                                contador++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbAHPReferenciado_CheckedChanged_1(object sender, EventArgs e)
        {
            //Impede que mais de um radio button seja selecionado de cada vez
            radioButtonAlgebrico.Checked = false;
            rbComponentesPrincipais.Checked = false;
            radioButton5.Checked = false;
            rbPromethe1.Checked = false;
            rbPromethe2.Checked = false;
            rbPromethe4.Checked = false;
            try
            {
                if (!radioButton5.Checked) label2.Visible = false;
                if (!radioButton5.Checked) numericUpDown1.Visible = false;
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.Length > 0)
                {
                    buttonExecutar.Enabled = true;
                }
                else
                {
                    buttonExecutar.Enabled = false;
                }
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.Length < 2)
                {
                    MessageBox.Show("É necessário pelo menos duas variáveis para realizar a análise.", "Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (!rbAHPReferenciado.Checked)
                    {
                        dataGridView1.Columns.Clear();
                        dataGridView1.Rows.Clear();
                    }
                    else
                    {
                        buttonExecutar.Enabled = true;
                        //Preenche a legenda do Mapa
                        if (dataGridView1.ColumnCount == 0)
                        {
                            //Adiciona nome da variável
                            DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                            txtCor = new DataGridViewTextBoxColumn();
                            txtCor.Width = 450;
                            txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                            txtCor.ReadOnly = true;
                            txtCor.HeaderText = "Variável";
                            txtCor.DisplayIndex = 2;
                            txtCor.DefaultCellStyle.BackColor = Color.White;
                            dataGridView1.Columns.Insert(0, txtCor);

                            //Adiciona coluna de peso.
                            DataGridViewComboBoxColumn txtPeso = new DataGridViewComboBoxColumn();
                            txtPeso = new DataGridViewComboBoxColumn();
                            txtPeso.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                            txtPeso.FlatStyle = FlatStyle.Popup;
                            txtPeso.Width = 100;
                            txtPeso.ReadOnly = false;
                            txtPeso.HeaderText = "Estatística";
                            txtPeso.DisplayIndex = 2;
                            txtPeso.Items.Add("Média");
                            txtPeso.Items.Add("Mediana");
                            txtPeso.Items.Add("Trimean");
                            txtPeso.Items.Add("Midrange");

                            dataGridView1.Columns.Insert(1, txtPeso);

                            DataGridViewCheckBoxColumn chkSelecionar = new DataGridViewCheckBoxColumn();
                            chkSelecionar = new DataGridViewCheckBoxColumn();
                            chkSelecionar.Width = 70;
                            chkSelecionar.HeaderText = "Padronizar";
                            chkSelecionar.DisplayIndex = 2;
                            dataGridView1.Columns.Insert(2, chkSelecionar);
                        }

                        for (int i = 0; i < userControlSelecaoVariaveis1.VariaveisIndependentes.Length; i++)
                        {
                            if (dataGridView1.Rows.Count < userControlSelecaoVariaveis1.VariaveisIndependentes.Length) dataGridView1.Rows.Add();
                            dataGridView1.Rows[i].Cells[0].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[i];
                            dataGridView1.Rows[i].Cells[1].Value = "Média";
                            dataGridView1.Rows[i].Cells[2].Value = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbComponentesPrincipais_CheckedChanged(object sender, EventArgs e)
        {
            //Impede que mais de um radio button seja selecionado de cada vez
            radioButton5.Checked = false;
            radioButtonHierarquico.Checked = false;
            rbAHPbg.Checked = false;
            rbAHPReferenciado.Checked = false;
            rbPromethe1.Checked = false;
            rbPromethe2.Checked = false;
            radioButton1.Checked = false;
            rbPromethe4.Checked = false;
            try
            {
                if (!radioButton5.Checked) label2.Visible = false;
                if (!radioButton5.Checked) numericUpDown1.Visible = false;
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.Length > 0)
                {
                    buttonExecutar.Enabled = true;
                }
                else
                {
                    buttonExecutar.Enabled = false;
                }
                if (!rbComponentesPrincipais.Checked)
                {
                    dataGridView1.Columns.Clear();
                    dataGridView1.Rows.Clear();
                }
                else
                {
                    buttonExecutar.Enabled = true;
                    //Preenche a legenda do Mapa
                    if (dataGridView1.ColumnCount == 0)
                    {
                        //Adiciona nome da variável
                        DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                        txtCor = new DataGridViewTextBoxColumn();
                        txtCor.Width = 450;
                        txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor.ReadOnly = true;
                        txtCor.HeaderText = "Variável";
                        txtCor.DisplayIndex = 2;
                        txtCor.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(0, txtCor);

                        DataGridViewCheckBoxColumn chkSelecionar = new DataGridViewCheckBoxColumn();
                        chkSelecionar = new DataGridViewCheckBoxColumn();
                        chkSelecionar.Width = 70;
                        chkSelecionar.HeaderText = "Padronizar";
                        chkSelecionar.DisplayIndex = 2;
                        dataGridView1.Columns.Insert(1, chkSelecionar);
                    }

                    for (int i = 0; i < userControlSelecaoVariaveis1.VariaveisIndependentes.Length; i++)
                    {
                        if (dataGridView1.Rows.Count < userControlSelecaoVariaveis1.VariaveisIndependentes.Length) dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[i];
                        dataGridView1.Rows[i].Cells[1].Value = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       
        private void rbPromethe1_CheckedChanged(object sender, EventArgs e)
        {
            //Impede que mais de um radio button seja selecionado de cada vez
            radioButtonAlgebrico.Checked = false;
            rbComponentesPrincipais.Checked = false;
            radioButtonHierarquico.Checked = false;
            rbAHPbg.Checked = false;
            rbAHPReferenciado.Checked = false;
            radioButton1.Checked = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (!radioButton5.Checked) label2.Visible = false;
                if (!radioButton5.Checked) numericUpDown1.Visible = false;
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.Length > 0)
                {
                    buttonExecutar.Enabled = true;
                }
                else
                {
                    buttonExecutar.Enabled = false;
                }
                if (!rbPromethe1.Checked)
                {
                    dataGridView1.Columns.Clear();
                    dataGridView1.Rows.Clear();
                }
                else
                {
                    buttonExecutar.Enabled = true;
                    //Preenche a legenda do Mapa
                    if (dataGridView1.ColumnCount == 0)
                    {
                        //Adiciona nome da variável
                        DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                        txtCor = new DataGridViewTextBoxColumn();
                        txtCor.Width = 300;
                        txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor.ReadOnly = true;
                        txtCor.HeaderText = "Variável";
                        txtCor.DisplayIndex = 2;
                        txtCor.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(0, txtCor);

                        //Adiciona coluna de peso.
                        DataGridViewNumericUpDownColumn txtPeso = new DataGridViewNumericUpDownColumn();
                        txtPeso = new DataGridViewNumericUpDownColumn();
                        txtPeso.Width = 50;
                        txtPeso.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtPeso.ReadOnly = false;
                        txtPeso.HeaderText = "Peso";
                        txtPeso.DisplayIndex = 2;
                        dataGridView1.Columns.Insert(1, txtPeso);

                        //Adiciona Função
                         DataGridViewComboBoxColumn txtCor1 = new DataGridViewComboBoxColumn();
                         txtCor1 = new DataGridViewComboBoxColumn();
                         txtCor1.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                         txtCor1.FlatStyle = FlatStyle.Popup;
                         txtCor1.Width = 200;
                         txtCor1.ReadOnly = false;
                         txtCor1.HeaderText = "Função";
                         txtCor1.DisplayIndex = 2;
                         txtCor1.Items.Add("Verdadeiro Critério");
                         txtCor1.Items.Add("Quase Critério");
                         txtCor1.Items.Add("Pseudocritério com preferência linear");
                         txtCor1.Items.Add("Critério de Nível");
                         txtCor1.Items.Add("Critério com preferência linear");
                         txtCor1.Items.Add("Critério Gaussiano");
                         dataGridView1.Columns.Insert(2, txtCor1);

                        //Adiciona Limite de indiferença
                        DataGridViewTextBoxColumn txtCor2 = new DataGridViewTextBoxColumn();
                        txtCor2 = new DataGridViewTextBoxColumn();
                        txtCor2.Width = 100;
                        txtCor2.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor2.ReadOnly = false;
                        txtCor2.HeaderText = "Limite de preferência";
                        txtCor2.DisplayIndex = 2;
                        txtCor2.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(3, txtCor2);

                        //Adiciona Limite de preferência
                        DataGridViewTextBoxColumn txtCor3 = new DataGridViewTextBoxColumn();
                        txtCor3 = new DataGridViewTextBoxColumn();
                        txtCor3.Width = 100;
                        txtCor3.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor3.ReadOnly = false;
                        txtCor3.HeaderText = "Limite de indiferença";
                        txtCor3.DisplayIndex = 2;
                        txtCor3.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(4, txtCor3);

                        //Adiciona coluna de direcao.
                        DataGridViewComboBoxColumn txtDirecao = new DataGridViewComboBoxColumn();
                        txtDirecao = new DataGridViewComboBoxColumn();
                        txtDirecao.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtDirecao.FlatStyle = FlatStyle.Popup;
                        txtDirecao.Width = 100;
                        txtDirecao.ReadOnly = false;
                        txtDirecao.HeaderText = "Direção";
                        txtDirecao.DisplayIndex = 2;
                        txtDirecao.Items.Add("Maximizar");
                        txtDirecao.Items.Add("Minimizar");
                        dataGridView1.Columns.Insert(5, txtDirecao);       
                    }

                    BLogicDecisaoMulticriterios blogic = new BLogicDecisaoMulticriterios();
                    for (int i = 0; i < userControlSelecaoVariaveis1.VariaveisIndependentes.Length; i++)
                    {
                        if (dataGridView1.Rows.Count < userControlSelecaoVariaveis1.VariaveisIndependentes.Length) dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[i];
                        dataGridView1.Rows[i].Cells[1].Value = 1;
                        dataGridView1.Rows[i].Cells[2].Value = "Critério de Nível";
                        dataGridView1.Rows[i].Cells[4].Value = blogic.LowerQuartile(m_dt_tabela_dados, this.userControlSelecaoVariaveis1.VariaveisIndependentes[i]);
                        dataGridView1.Rows[i].Cells[3].Value = blogic.UpperQuartile(m_dt_tabela_dados, this.userControlSelecaoVariaveis1.VariaveisIndependentes[i]);
                        dataGridView1.Rows[i].Cells[5].Value = "Maximizar";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void rbPromethe2_CheckedChanged(object sender, EventArgs e)
        {
            //Impede que mais de um radio button seja selecionado de cada vez
            radioButtonAlgebrico.Checked = false;
            rbComponentesPrincipais.Checked = false;
            radioButtonHierarquico.Checked = false;
            rbAHPbg.Checked = false;
            rbAHPReferenciado.Checked = false;
            radioButton1.Checked = false;
            this.Cursor = Cursors.WaitCursor;
            if (!radioButton5.Checked) label2.Visible = false;
            if (!radioButton5.Checked) numericUpDown1.Visible = false;
            try
            {
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.Length > 0)
                {
                    buttonExecutar.Enabled = true;
                }
                else
                {
                    buttonExecutar.Enabled = false;
                }
                if (!rbPromethe2.Checked)
                {
                    dataGridView1.Columns.Clear();
                    dataGridView1.Rows.Clear();
                }
                else
                {
                    buttonExecutar.Enabled = true;
                    //Preenche a legenda do Mapa
                    if (dataGridView1.ColumnCount == 0)
                    {
                        //Adiciona nome da variável
                        DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                        txtCor = new DataGridViewTextBoxColumn();
                        txtCor.Width = 300;
                        txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor.ReadOnly = true;
                        txtCor.HeaderText = "Variável";
                        txtCor.DisplayIndex = 2;
                        txtCor.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(0, txtCor);

                        //Adiciona coluna de peso.
                        DataGridViewNumericUpDownColumn txtPeso = new DataGridViewNumericUpDownColumn();
                        txtPeso = new DataGridViewNumericUpDownColumn();
                        txtPeso.Width = 50;
                        txtPeso.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtPeso.ReadOnly = false;
                        txtPeso.HeaderText = "Peso";
                        txtPeso.DisplayIndex = 2;
                        dataGridView1.Columns.Insert(1, txtPeso);

                        //Adiciona Função
                        DataGridViewComboBoxColumn txtCor1 = new DataGridViewComboBoxColumn();
                        txtCor1 = new DataGridViewComboBoxColumn();
                        txtCor1.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor1.FlatStyle = FlatStyle.Popup;
                        txtCor1.Width = 200;
                        txtCor1.ReadOnly = false;
                        txtCor1.HeaderText = "Função";
                        txtCor1.DisplayIndex = 2;
                        txtCor1.Items.Add("Verdadeiro Critério");
                        txtCor1.Items.Add("Quase Critério");
                        txtCor1.Items.Add("Pseudocritério com preferência linear");
                        txtCor1.Items.Add("Critério de Nível");
                        txtCor1.Items.Add("Critério com preferência linear");
                        txtCor1.Items.Add("Critério Gaussiano");
                        dataGridView1.Columns.Insert(2, txtCor1);

                        //Adiciona Limite de indiferença
                        DataGridViewTextBoxColumn txtCor2 = new DataGridViewTextBoxColumn();
                        txtCor2 = new DataGridViewTextBoxColumn();
                        txtCor2.Width = 100;
                        txtCor2.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor2.ReadOnly = false;
                        txtCor2.HeaderText = "Limite de preferência";
                        txtCor2.DisplayIndex = 2;
                        txtCor2.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(3, txtCor2);

                        //Adiciona Limite de preferência
                        DataGridViewTextBoxColumn txtCor3 = new DataGridViewTextBoxColumn();
                        txtCor3 = new DataGridViewTextBoxColumn();
                        txtCor3.Width = 100;
                        txtCor3.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor3.ReadOnly = false;
                        txtCor3.HeaderText = "Limite de indiferença";
                        txtCor3.DisplayIndex = 2;
                        txtCor3.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(4, txtCor3);

                        //Adiciona coluna de direcao.
                        DataGridViewComboBoxColumn txtDirecao = new DataGridViewComboBoxColumn();
                        txtDirecao = new DataGridViewComboBoxColumn();
                        txtDirecao.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtDirecao.FlatStyle = FlatStyle.Popup;
                        txtDirecao.Width = 100;
                        txtDirecao.ReadOnly = false;
                        txtDirecao.HeaderText = "Direção";
                        txtDirecao.DisplayIndex = 2;
                        txtDirecao.Items.Add("Maximizar");
                        txtDirecao.Items.Add("Minimizar");
                        dataGridView1.Columns.Insert(5, txtDirecao);
                    }

                    BLogicDecisaoMulticriterios blogic = new BLogicDecisaoMulticriterios();
                    for (int i = 0; i < userControlSelecaoVariaveis1.VariaveisIndependentes.Length; i++)
                    {
                        if (dataGridView1.Rows.Count < userControlSelecaoVariaveis1.VariaveisIndependentes.Length) dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[i];
                        dataGridView1.Rows[i].Cells[1].Value = 1;
                        dataGridView1.Rows[i].Cells[2].Value = "Critério de Nível";
                        dataGridView1.Rows[i].Cells[4].Value = blogic.LowerQuartile(m_dt_tabela_dados, this.userControlSelecaoVariaveis1.VariaveisIndependentes[i]);
                        dataGridView1.Rows[i].Cells[3].Value = blogic.UpperQuartile(m_dt_tabela_dados, this.userControlSelecaoVariaveis1.VariaveisIndependentes[i]);
                        dataGridView1.Rows[i].Cells[5].Value = "Maximizar";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            //Impede que mais de um radio button seja selecionado de cada vez
            radioButtonAlgebrico.Checked = false;
            rbComponentesPrincipais.Checked = false;
            radioButtonHierarquico.Checked = false;
            rbAHPbg.Checked = false;
            rbAHPReferenciado.Checked = false;
            radioButton1.Checked = false;

            this.Cursor = Cursors.WaitCursor;
            if (radioButton5.Checked) label2.Visible = true;
            if (radioButton5.Checked) numericUpDown1.Visible = true;
            if (!radioButton5.Checked) label2.Visible = false;
            if (!radioButton5.Checked) numericUpDown1.Visible = false;
            rbAHPReferenciado.Checked = false;
            try
            {
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.Length > 0)
                {
                    buttonExecutar.Enabled = true;
                }
                else
                {
                    buttonExecutar.Enabled = false;
                }
                if (!radioButton5.Checked)
                {
                    dataGridView1.Columns.Clear();
                    dataGridView1.Rows.Clear();
                }
                else
                {
                    buttonExecutar.Enabled = true;
                    //Preenche a legenda do Mapa
                    if (dataGridView1.ColumnCount == 0)
                    {
                        //Adiciona nome da variável
                        DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                        txtCor = new DataGridViewTextBoxColumn();
                        txtCor.Width = 300;
                        txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor.ReadOnly = true;
                        txtCor.HeaderText = "Variável";
                        txtCor.DisplayIndex = 2;
                        txtCor.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(0, txtCor);

                        //Adiciona coluna de peso.
                        DataGridViewNumericUpDownColumn txtPeso = new DataGridViewNumericUpDownColumn();
                        txtPeso = new DataGridViewNumericUpDownColumn();
                        txtPeso.Width = 50;
                        txtPeso.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtPeso.ReadOnly = false;
                        txtPeso.HeaderText = "Peso";
                        txtPeso.DisplayIndex = 2;
                        dataGridView1.Columns.Insert(1, txtPeso);

                        //Adiciona Função
                        DataGridViewComboBoxColumn txtCor1 = new DataGridViewComboBoxColumn();
                        txtCor1 = new DataGridViewComboBoxColumn();
                        txtCor1.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor1.FlatStyle = FlatStyle.Popup;
                        txtCor1.Width = 200;
                        txtCor1.ReadOnly = false;
                        txtCor1.HeaderText = "Função";
                        txtCor1.DisplayIndex = 2;
                        txtCor1.Items.Add("Verdadeiro Critério");
                        txtCor1.Items.Add("Quase Critério");
                        txtCor1.Items.Add("Pseudocritério com preferência linear");
                        txtCor1.Items.Add("Critério de Nível");
                        txtCor1.Items.Add("Critério com preferência linear");
                        txtCor1.Items.Add("Critério Gaussiano");
                        dataGridView1.Columns.Insert(2, txtCor1);

                        //Adiciona Limite de indiferença
                        DataGridViewTextBoxColumn txtCor2 = new DataGridViewTextBoxColumn();
                        txtCor2 = new DataGridViewTextBoxColumn();
                        txtCor2.Width = 100;
                        txtCor2.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor2.ReadOnly = false;
                        txtCor2.HeaderText = "Limite de preferência";
                        txtCor2.DisplayIndex = 2;
                        txtCor2.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(3, txtCor2);

                        //Adiciona Limite de preferência
                        DataGridViewTextBoxColumn txtCor3 = new DataGridViewTextBoxColumn();
                        txtCor3 = new DataGridViewTextBoxColumn();
                        txtCor3.Width = 100;
                        txtCor3.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor3.ReadOnly = false;
                        txtCor3.HeaderText = "Limite de indiferença";
                        txtCor3.DisplayIndex = 2;
                        txtCor3.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(4, txtCor3);

                        //Adiciona coluna de direcao.
                        DataGridViewComboBoxColumn txtDirecao = new DataGridViewComboBoxColumn();
                        txtDirecao = new DataGridViewComboBoxColumn();
                        txtDirecao.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtDirecao.FlatStyle = FlatStyle.Popup;
                        txtDirecao.Width = 100;
                        txtDirecao.ReadOnly = false;
                        txtDirecao.HeaderText = "Direção";
                        txtDirecao.DisplayIndex = 2;
                        txtDirecao.Items.Add("Maximizar");
                        txtDirecao.Items.Add("Minimizar");
                        dataGridView1.Columns.Insert(5, txtDirecao);
                    }

                    BLogicDecisaoMulticriterios blogic = new BLogicDecisaoMulticriterios();
                    for (int i = 0; i < userControlSelecaoVariaveis1.VariaveisIndependentes.Length; i++)
                    {
                        if (dataGridView1.Rows.Count < userControlSelecaoVariaveis1.VariaveisIndependentes.Length) dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[i];
                        dataGridView1.Rows[i].Cells[1].Value = 1;
                        dataGridView1.Rows[i].Cells[2].Value = "Critério de Nível";
                        dataGridView1.Rows[i].Cells[4].Value = blogic.LowerQuartile(m_dt_tabela_dados, this.userControlSelecaoVariaveis1.VariaveisIndependentes[i]);
                        dataGridView1.Rows[i].Cells[3].Value = blogic.UpperQuartile(m_dt_tabela_dados, this.userControlSelecaoVariaveis1.VariaveisIndependentes[i]);
                        dataGridView1.Rows[i].Cells[5].Value = "Maximizar";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (userControlSelecaoVariaveis1.VariaveisIndependentes.Length > 0)
                {
                    buttonExecutar.Enabled = true;
                }
                else
                {
                    buttonExecutar.Enabled = false;
                }
                if (!rbPromethe4.Checked)
                {
                    dataGridView1.Columns.Clear();
                    dataGridView1.Rows.Clear();
                }
                else
                {
                    buttonExecutar.Enabled = true;
                    //Preenche a legenda do Mapa
                    if (dataGridView1.ColumnCount == 0)
                    {
                        //Adiciona nome da variável
                        DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                        txtCor = new DataGridViewTextBoxColumn();
                        txtCor.Width = 300;
                        txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor.ReadOnly = true;
                        txtCor.HeaderText = "Variável";
                        txtCor.DisplayIndex = 2;
                        txtCor.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(0, txtCor);

                        //Adiciona coluna de peso.
                        DataGridViewNumericUpDownColumn txtPeso = new DataGridViewNumericUpDownColumn();
                        txtPeso = new DataGridViewNumericUpDownColumn();
                        txtPeso.Width = 50;
                        txtPeso.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtPeso.ReadOnly = false;
                        txtPeso.HeaderText = "Peso";
                        txtPeso.DisplayIndex = 2;
                        dataGridView1.Columns.Insert(1, txtPeso);

                        //Adiciona Função
                        DataGridViewComboBoxColumn txtCor1 = new DataGridViewComboBoxColumn();
                        txtCor1 = new DataGridViewComboBoxColumn();
                        txtCor1.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor1.FlatStyle = FlatStyle.Popup;
                        txtCor1.Width = 200;
                        txtCor1.ReadOnly = false;
                        txtCor1.HeaderText = "Função";
                        txtCor1.DisplayIndex = 2;
                        txtCor1.Items.Add("Verdadeiro Critério");
                        txtCor1.Items.Add("Quase Critério");
                        txtCor1.Items.Add("Pseudocritério com preferência linear");
                        txtCor1.Items.Add("Critério de Nível");
                        txtCor1.Items.Add("Critério com preferência linear");
                        txtCor1.Items.Add("Critério Gaussiano");
                        dataGridView1.Columns.Insert(2, txtCor1);

                        //Adiciona Limite de indiferença
                        DataGridViewTextBoxColumn txtCor2 = new DataGridViewTextBoxColumn();
                        txtCor2 = new DataGridViewTextBoxColumn();
                        txtCor2.Width = 100;
                        txtCor2.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor2.ReadOnly = false;
                        txtCor2.HeaderText = "Limite de preferência";
                        txtCor2.DisplayIndex = 2;
                        txtCor2.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(3, txtCor2);

                        //Adiciona Limite de preferência
                        DataGridViewTextBoxColumn txtCor3 = new DataGridViewTextBoxColumn();
                        txtCor3 = new DataGridViewTextBoxColumn();
                        txtCor3.Width = 100;
                        txtCor3.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor3.ReadOnly = false;
                        txtCor3.HeaderText = "Limite de indiferença";
                        txtCor3.DisplayIndex = 2;
                        txtCor3.DefaultCellStyle.BackColor = Color.White;
                        dataGridView1.Columns.Insert(4, txtCor3);

                        //Adiciona coluna de direcao.
                        DataGridViewComboBoxColumn txtDirecao = new DataGridViewComboBoxColumn();
                        txtDirecao = new DataGridViewComboBoxColumn();
                        txtDirecao.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtDirecao.FlatStyle = FlatStyle.Popup;
                        txtDirecao.Width = 100;
                        txtDirecao.ReadOnly = false;
                        txtDirecao.HeaderText = "Direção";
                        txtDirecao.DisplayIndex = 2;
                        txtDirecao.Items.Add("Maximizar");
                        txtDirecao.Items.Add("Minimizar");
                        dataGridView1.Columns.Insert(5, txtDirecao);
                    }

                    BLogicDecisaoMulticriterios blogic = new BLogicDecisaoMulticriterios();
                    for (int i = 0; i < userControlSelecaoVariaveis1.VariaveisIndependentes.Length; i++)
                    {
                        if (dataGridView1.Rows.Count < userControlSelecaoVariaveis1.VariaveisIndependentes.Length) dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = userControlSelecaoVariaveis1.VariaveisIndependentes[i];
                        dataGridView1.Rows[i].Cells[1].Value = 1;
                        dataGridView1.Rows[i].Cells[2].Value = "Critério de Nível";
                        dataGridView1.Rows[i].Cells[4].Value = blogic.LowerQuartile(m_dt_tabela_dados, this.userControlSelecaoVariaveis1.VariaveisIndependentes[i]);
                        dataGridView1.Rows[i].Cells[3].Value = blogic.UpperQuartile(m_dt_tabela_dados, this.userControlSelecaoVariaveis1.VariaveisIndependentes[i]);
                        dataGridView1.Rows[i].Cells[5].Value = "Maximizar";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }
    }
}
