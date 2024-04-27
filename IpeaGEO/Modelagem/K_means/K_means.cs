using System;
using System.Data;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public partial class K_means : Form
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

                this.Text = "Clusters com Algoritmo K-Means - " + value;
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
                    this.btnExecutar.Enabled = true;
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

        public K_means()
        {
            InitializeComponent();
        }

        #region atualização da tabela de dados

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
                this.userControlSelecaoVariaveis1.ZeraControle();
                this.userControlSelecaoVariaveis1.VariaveisDB = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
                this.userControlSelecaoVariaveis1.VariaveisList = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
            }

            if (!this.tabControl1.TabPages.Contains(tabPage0))
            {
                tabControl1.TabPages.Add(tabPage0);
            }
            this.tabControl1.SelectedTab = tabPage0;

            string[] all_variables = clt.RetornaUniqueColunas(m_dt_tabela_dados);

            for (int i = 0; i < m_dt_tabela_dados.Columns.Count; i++)
            {
                comboBox1.Items.Add(m_dt_tabela_dados.Columns[i].ToString());
            }
            comboBox1.Enabled = true;

            if (all_variables.GetLength(0) > 0)
            {
                comboBox1.SelectedItem = all_variables[0];
            }
            else
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        #endregion 

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {

                if (checkBox1.Checked)
                {
                    MessageBox.Show("Tabela de dados Atualizada", "Atualização", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.OK;

                    Cursor = Cursors.WaitCursor;

                    m_gridview_externo.DataSource = ((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy();

                    this.DataSetExterno.Tables.Clear();
                    this.DataSetExterno.Tables.Add(((DataTable)this.userControlDataGrid1.Datagridview.DataSource).Copy());

                    lblProgressBar.Visible = true;
                    lblProgressBar.Text = "Tabela atualizada no formulário de mapas";

                    Cursor = Cursors.Default;

                }

                else
                {
                    MessageBox.Show("Selecione a opção 'Apresentar variáveis geradas na tabela de dados', localizada na aba Especificações", "Atualização", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormBaseModelagem_Load(object sender, EventArgs e)
        {
            try
            {
                userControlSelecaoVariaveis1.LabelListBoxEsquerda = "Variáveis numéricas na tabela";
                userControlSelecaoVariaveis1.LabelListBoxDireita = "Variáveis selecionadas";
                userControlSelecaoVariaveis1.PermiteSelecaoMultipla = true;

                // Variáveis sendo passadas para o UserControl
                userControlDataGrid1.TabelaDados = this.m_dt_tabela_dados;
                userControlDataGrid1.FuncaoFromFormulario = new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionFromFormulario(this.AtualizaTabelaDados);
                userControlDataGrid1.MostraOpcaoImportadaoDados = true;
                userControlDataGrid1.UserControlSelecaoVariaveis = this.userControlSelecaoVariaveis1;

                if (tabControl1.TabPages.Contains(tabPage2))
                {
                    tabControl1.TabPages.Remove(tabPage2);
                }

                if (tabControl1.TabPages.Contains(tabPage3))
                {
                    tabControl1.TabPages.Remove(tabPage3);
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

        #region Executar a análise de clusters

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            timer1.Interval = 2500;
            timer1.Start();
            Cursor = Cursors.WaitCursor;
            progressBar1.Visible = true;
            AnaliseClustersKMeans();

            Cursor = Cursors.Default;
        }

        private void AnaliseClustersKMeans()
        {
            try
            {
                progressBar1.Visible = true;
                Application.DoEvents();

                BLogicKMeans blk = new BLogicKMeans();

                int[] indicadores = new int[0];
                int num_iteracoes = 0;
                double impureza = 0.0; 

                int[,] matrix_indicadores = new int[0, 0];
                int[] vetor_num_iteracoes = new int[0];
                double[] vetor_impureza = new double[0];

                blk.GeraClusters(m_dt_tabela_dados, userControlSelecaoVariaveis1.VariaveisIndependentes, Convert.ToInt32(numericUpDown2.Value), ckbUsarCentroidesAleatorios.Checked, 
                    Convert.ToInt32(nudCentroidesGerados.Value), ckbNormalizarVariaveis.Checked, 
                    ref matrix_indicadores, ref vetor_num_iteracoes, ref vetor_impureza, ref indicadores, ref num_iteracoes, ref impureza, ref progressBar1);

                if (numericUpDown2.Value < m_dt_tabela_dados.Rows.Count)
                {
                    if (comboBox1.SelectedItem != null)
                    {
                        double[,] dados = new double[m_dt_tabela_dados.Rows.Count, 1];

                        clsUtilTools m_clt = new clsUtilTools();
                        double[] matriz = new double[indicadores.GetLength(0)];
                        string nova_variavel;

                        for (int i = 0; i < indicadores.GetLength(0); i++)
                        {
                            matriz[i] = indicadores[i];
                        }

                        if (checkBox1.Checked)
                        {
                            for (int m = 0; m < m_dt_tabela_dados.Columns.Count; m++)
                            {
                                if (m_dt_tabela_dados.Columns[m].ToString() == "Cluster_KMeans")
                                {
                                    m_dt_tabela_dados.Columns.Remove("Cluster_KMeans");
                                }
                                if (m_dt_tabela_dados.Columns[m].ToString() == "Label_KMeans")
                                {
                                    m_dt_tabela_dados.Columns.Remove("Label_KMeans");
                                }
                            }

                            nova_variavel = "Cluster_KMeans";

                            m_dt_tabela_dados.Columns.Add(nova_variavel, typeof(double));
                            m_dt_tabela_dados.Columns.Add("Label_KMeans", typeof(string));

                            for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
                            {
                                m_dt_tabela_dados.Rows[i][nova_variavel] = matriz[i];
                                m_dt_tabela_dados.Rows[i]["Label_KMeans"] = "Cluster_" + matriz[i].ToString();
                            }

                            this.userControlDataGrid1.Datagridview.Columns[nova_variavel].Width = 150;
                            this.userControlDataGrid1.Datagridview.Columns[nova_variavel].ToolTipText = "Coluna contendo os indicadores dos clusters para cada observação na tabela de dados";

                            this.userControlDataGrid1.Datagridview.Columns["Label_KMeans"].Width = 150;
                            this.userControlDataGrid1.Datagridview.Columns["Label_KMeans"].ToolTipText = "Coluna contendo os labels dos clusters para cada observação na tabela de dados (útil para mapas de temáticos)";
                        }
                        
                        string[] nome_var = new string[2];

                        nome_var[0] = comboBox1.Text.ToString();
                        nome_var[1] = "Cluster_KMeans";

                        object[,] id = new object[m_dt_tabela_dados.Rows.Count, 1];
                        object[,] resultado = new object[m_dt_tabela_dados.Rows.Count, 2];

                        id = m_clt.GetObjMatrizFromDataTable(m_dt_tabela_dados, comboBox1.Text.ToString());

                        for (int i = 0; i < resultado.GetLength(0); i++)
                        {
                            resultado[i, 0] = id[i, 0];
                            resultado[i, 1] = indicadores[i];
                        }

                        this.tabControl1.SelectedTab = tabPage2;

                        string[] m_variaveis_dependentes = userControlSelecaoVariaveis1.VariaveisIndependentes;

                        if (checkBox2.Checked)
                        {
                            m_output_text = "";
                            m_output_vars_geradas = "";
                        }

                        string out_text = "==============================================================================================================\n\n";
                        out_text += "Resultado do Método K-Means para Agrupamentos Homogêneos \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
                        out_text += "Número de observações: " + resultado.GetLength(0) + "\n";
                        out_text += "Número de interações: " + num_iteracoes.ToString() + "\n";
                        if (ckbUsarCentroidesAleatorios.Checked)
                        {
                            out_text += "Centróides iniciais: " + "seleção entre " + Convert.ToInt32(nudCentroidesGerados.Value) + " centróides aleatórios" + "\n";
                        }
                        else
                        {
                            out_text += "Centróides iniciais: " + "ordenação do primeiro componente principal" + "\n";
                        }
                        if (ckbNormalizarVariaveis.Checked)
                        {
                            out_text += "Normalização das variáveis: " + "variáveis normalizadas antes do algoritmo" + "\n";
                        }
                        else
                        {
                            out_text += "Normalização das variáveis: " + "variáveis não foram normalizadas" + "\n";
                        }
                        out_text += "Variabilidade média intra-clusters: " + m_clt.Double2Texto(impureza, 6) + "\n\n";

                        out_text += "Variáveis escolhidas para geração dos clusters: \n\n";
                        for (int m = 0; m < m_variaveis_dependentes.Length; m++)
                        {
                            out_text += "\t(" + (m+1).ToString() + ") " + m_variaveis_dependentes[m] + "\n";
                        }
                        out_text += "\n";

                        out_text += "Variável identificadora das observações:\t" + comboBox1.SelectedItem.ToString() + "\n\n";

                        string out_text_variaveis = "==============================================================================================================\n\n";
                        out_text_variaveis += "Resultado do Método K-Means para Agrupamentos Homogêneos - Identificação dos Clusters \n\n";
                        out_text_variaveis += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text_variaveis += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                        out_text_variaveis += "\t" + nome_var[0] + "\t\t" + nome_var[1] + "\n\n";

                        for (int j = 0; j < resultado.GetLength(0); j++)
                        {
                            out_text_variaveis += "\t" + resultado[j, 0].ToString() + "\t\t\t" + resultado[j, 1].ToString() + "\n";
                        }
                        out_text_variaveis += "\n";

                        if (checkBox2.Checked)
                        {
                            m_output_text = out_text;
                            m_output_vars_geradas = out_text_variaveis;
                        }
                        else
                        {
                            m_output_text += out_text;
                            m_output_vars_geradas += out_text_variaveis;
                        }

                        userControlRichTextOutput1.Texto = m_output_text;
                        userControlRichTextOutput2.Texto = m_output_vars_geradas;

                        if (!this.tabControl1.TabPages.Contains(tabPage2))
                        {
                            tabControl1.TabPages.Add(tabPage2);
                        }
                        this.tabControl1.SelectedTab = tabPage2;

                        if (!this.tabControl1.TabPages.Contains(tabPage3))
                        {
                            tabControl1.TabPages.Add(tabPage3);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Para gerar o resultado é necessário selecionar a variável identificadora.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("O número de classes excede o tamanho da tabela dos dados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        private void ckbUsarCentroidesAleatorios_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckbUsarCentroidesAleatorios.Checked) nudCentroidesGerados.Enabled = true;
                else nudCentroidesGerados.Enabled = false;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void userControlRichTextOutput1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Visible = false;
        }
    }
} 


