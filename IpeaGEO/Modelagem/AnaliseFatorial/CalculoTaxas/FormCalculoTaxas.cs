using System;
using System.Data;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public partial class FormCalculoTaxas : Form
    {
        public FormCalculoTaxas()
        {
            InitializeComponent();
        }

        #region Variáveis internas

        private bool m_shape_carregado_do_mapa = false;
        private bool m_habilita_import_dados_shape = true;

        public void HabilitarDadosExternos()
        {
            this.btnOK.Visible = true;
            this.btnOK.Enabled = true;
            
            m_shape_carregado_do_mapa = true;

            userControlDataGrid1.HabilitarImportacaoDados = false;
            userControlDataGrid1.HabilitarImportacaoShape = false;

            m_habilita_import_dados_shape = false;

            lblProgressBar.Text = "Arquivos de dados e shape do formulário de mapas";
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

                this.Text = "Cálculo de Taxas - " + value;
            }
        }

        private DataTable m_dt_tabela_dados = new DataTable();
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

        private string[] m_nomes_variaveis_ocorrencias = new string[0];
        private string m_nome_variavel_populacao = "";

        private bool m_dados_concatenados = false;

        private DataTable m_dt_tabela_shape = new DataTable();
        public DataTable DadosShape
        {
            get { return m_dt_tabela_shape; }
            set { m_dt_tabela_shape = value; }
        }

        private IpeaGeo.RegressoesEspaciais.clsIpeaShape m_shape = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
        public IpeaGeo.RegressoesEspaciais.clsIpeaShape Shape
        {
            get { return m_shape; }
            set { m_shape = value; }
        }

        private IpeaGeo.RegressoesEspaciais.clsIpeaShape shapeAlex = new RegressoesEspaciais.clsIpeaShape();
        public IpeaGeo.RegressoesEspaciais.clsIpeaShape EstruturaShape
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

        private bool shape_ok = false;
        private bool tabela_ok = false;
        private bool vizinhanca_ok = false;

        #endregion

        private void AtualizarArquivoConcatenacao(bool atualizaUControl)
        {
            progressBar1.Visible = true;
            tabela_ok = true;
            //if (shape_ok && tabela_ok) button1.Enabled = true;
            this.lblProgressBar.Text = "Arquivo de dados importado com sucesso";

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

            string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);
            string[] todasvariaveis = clt.RetornaTodasColunas(m_dt_tabela_dados);
            userControlSelecaoVariaveis1.PermiteSelecaoMultipla = true;
            userControlSelecaoVariaveis1.ZeraControle();
            userControlSelecaoVariaveis1.VariaveisList = variaveis_numericas;
            userControlSelecaoVariaveis1.VariaveisDB = variaveis_numericas;

            lstVarPopulacao.Items.Clear();
            lstVarPopulacao.Items.AddRange(variaveis_numericas);
            lstVarPopulacao.SelectedIndex = 0;

            /*ComboBox de Labels*/

            cboxLabel.Items.Clear();
            cboxLabel.Items.AddRange(todasvariaveis);
            cboxLabel.SelectedIndex = 0;

            ConcatenarDados();
            if (!this.tabControl1.TabPages.Contains(this.tabPage0))
                this.tabControl1.TabPages.Add(tabPage0);
            tabControl1.SelectedTab = tabPage0;
            progressBar1.Visible = false;
        }

        private void AtualizarArquivoShape(bool atualiza)
        {
            this.lblProgressBar.Text = "Arquivo shape importado com sucesso";
            shape_ok = true;
            //if (shape_ok && tabela_ok) button1.Enabled = true;

            if (atualiza)
            {
                this.userControlDataGrid1.Shape = m_shape;
                this.userControlDataGrid1.DadosShape = m_dt_tabela_shape;
            }
            else
            {
                m_shape = this.userControlDataGrid1.Shape;
                m_dt_tabela_shape = userControlDataGrid1.DadosShape;
            } 
        }

        private void AtualizaTabelaDados(bool atualizaUControl)
        {
            tabela_ok = true;
            //if (shape_ok && tabela_ok) button1.Enabled = true;
            this.lblProgressBar.Text = "Arquivo de dados importado com sucesso";

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

            string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);
            string[] todasvariaveis = clt.RetornaTodasColunas(m_dt_tabela_dados);
            userControlSelecaoVariaveis1.PermiteSelecaoMultipla = true;
            userControlSelecaoVariaveis1.ZeraControle();
            userControlSelecaoVariaveis1.VariaveisList = variaveis_numericas;
            userControlSelecaoVariaveis1.VariaveisDB = variaveis_numericas;

            lstVarPopulacao.Items.Clear();
            lstVarPopulacao.Items.AddRange(variaveis_numericas);
            lstVarPopulacao.SelectedIndex = 0;
            
            /*ComboBox de Labels*/
            
            cboxLabel.Items.Clear();
            cboxLabel.Items.AddRange(todasvariaveis);
            cboxLabel.SelectedIndex = 0;

            // mostra aba de especificação do modelo para cálculo de taxas;
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
            try
            {
                m_gridview_externo.DataSource = ((DataTable)this.userControlDataGrid1.TabelaDados).Copy();

                this.DataSetExterno.Tables.Clear();
                this.DataSetExterno.Tables.Add(((DataTable)this.userControlDataGrid1.TabelaDados).Copy());

                lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
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
                #region parametros do user control data grid

                // Variáveis sendo passadas para o UserControl
                userControlDataGrid1.TabelaDados = this.m_dt_tabela_dados;
                userControlDataGrid1.FuncaoFromFormulario = new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionFromFormulario(this.AtualizaTabelaDados);
                userControlDataGrid1.FuncaoShapeFromFormulario = new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionShapeFromFormulario(this.AtualizarArquivoShape);
                userControlDataGrid1.FuncaoConcatenacaoFromFormulario = new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionConcatenacaoFromFormulario(this.AtualizarArquivoConcatenacao);
                userControlDataGrid1.MostraOpcaoImportadaoDados = m_habilita_import_dados_shape;
                userControlDataGrid1.UserControlSelecaoVariaveis = this.userControlSelecaoVariaveis1;
                userControlDataGrid1.HabilitarImportacaoShape = m_habilita_import_dados_shape;
                userControlDataGrid1.HabilitarImportacaoDados = m_habilita_import_dados_shape;
                
                #endregion

                if (m_dt_tabela_dados.Rows.Count <= 0 || m_dt_tabela_dados.Columns.Count <= 0)
                {
                    if (tabControl1.TabPages.Contains(tabPage0))
                    {
                        tabControl1.TabPages.Remove(tabPage0);
                    }
                }

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

                if (tabControl1.TabPages.Contains(tabPage5))
                {
                    tabControl1.TabPages.Remove(tabPage5);
                }
                
                if (this.m_shape.TipoVizinhanca != "")
                {
                    radioButton1.Visible = false;
                    radioButton2.Visible = false;
                    radioButton3.Visible = false;
                    radioButton4.Visible = false;
                    button2.Visible = false;
                    labelTipoViz.Visible = true;
                    labelTipoViz.Text = "Vizinhança do tipo " + m_shape.TipoVizinhanca;

                    int tipo_viz = 0;
                    if (m_shape.TipoVizinhanca.ToString() == "Queen")
                    {
                        tipo_viz = 1;
                    }
                    else if (m_shape.TipoVizinhanca.ToString() == "Rook")
                    {
                        tipo_viz = 2;
                    }
                    else if (m_shape.TipoVizinhanca.ToString() == "Queen Normalizada")
                    {
                        tipo_viz = 3;
                    }
                    else if (m_shape.TipoVizinhanca.ToString() == "Rook Normalizada")
                    {
                        tipo_viz = 4;
                    }
                    
                    vizinhanca_ok = true;
                    ckbSpatialEmpricalBayes.Enabled = true;
                    ckbTaxaEspacial.Enabled = true;
                    ckbRRelLogNEsp.Enabled = true;
                    ckbRRelGammaEsp.Enabled = true;
                }

                if (this.m_shape.TipoVizinhanca == "") 
                {
                    radioButton1.Visible = true;
                    radioButton2.Visible = true;
                    radioButton3.Visible = true;
                    radioButton4.Visible = true;
                    labelTipoViz.Visible = false ;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        #region concatenação da tabela de shape com a tabela de dados

        private clsMatrizEsparsa m_W_sparsa_from_dists = new clsMatrizEsparsa();
        private bool m_W_sparsa_from_dists_existente = false;

        private bool m_W_sparsa_from_arquivo_existente = false;
        private clsMatrizEsparsa m_W_sparsa_from_arquivo = new clsMatrizEsparsa();

        private void ConcatenarDados()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int obs_shape = m_shape.Count;
                int obs_dados = this.m_dt_tabela_dados.Rows.Count;
                m_dados_concatenados = false;

                if (obs_dados > obs_shape) MessageBox.Show("O número de observações na tabela de dados é maior do que o número de observações no arquivo shape.",
                    "Falha na concatenação", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (obs_dados < obs_shape) MessageBox.Show("O número de observações na tabela de dados é menor do que o número de observações no arquivo shape.",
                    "Falha na concatenação", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (obs_dados == obs_shape)
                {
                    m_dados_concatenados = true;

                    this.m_W_sparsa_from_dists_existente = false;
                    this.m_W_sparsa_from_arquivo_existente = false;
                }

                lblProgressBar.Text = "Tabelas concatenadas com sucesso";

                this.Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
        
        private void lstVarPopulacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string m_nome_variavel_populacao = this.lstVarPopulacao.SelectedItem.ToString();
                textBox1.Text = m_nome_variavel_populacao;
            }
            catch (Exception er)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                lblProgressBar.Text = "Executando o cálculo de taxas";

                EstimarTaxas();

                if (!this.tabControl1.TabPages.Contains(this.tabPage4))
                    this.tabControl1.TabPages.Add(tabPage4);
               
                if (!this.tabControl1.TabPages.Contains(this.tabPage5))
                    this.tabControl1.TabPages.Add(tabPage5);

                this.tabControl1.SelectedTab = this.tabPage4;

                lblProgressBar.Text = "Taxas calculadas com sucesso";

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                this.Cursor = Cursors.Default;
                lblProgressBar.Text = "Erro no cálculo de taxas";
                MessageBox.Show(er.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #region estimação das taxas

        private void EstimarTaxas()
        {
            m_nomes_variaveis_ocorrencias = this.userControlSelecaoVariaveis1.VariaveisIndependentes;
            if (m_nomes_variaveis_ocorrencias.GetLength(0) <= 0)
            {
                throw new Exception("Escolha pelo menos uma variável de ocorrência.");
            }

            if (ckbBayesianClayton.Checked || ckbTaxaClaytonCaldor.Checked)
            {
                if (!ckbRRelGamma.Checked && !ckbRRelLogN.Checked)
                {
                    throw new Exception("Selecione pelo menos uma das prioris para os métodos de Clayton e Caldor.");
                }
            }

            BLogicCalculoTaxas blc = new BLogicCalculoTaxas();

            blc.ValorTolerancia = Convert.ToDouble(this.nudTolerancia.Value);
            blc.ValorSimulacoes = Convert.ToInt32(this.nudSimulacoes.Value);
            blc.ComBOX = this.cboxLabel;
            blc.CalculaEmpiricalBayesRate = this.ckbEmpiricalBayes.Checked;
            blc.CalculaEmpiricalSpatialBayesRate = this.ckbSpatialEmpricalBayes.Checked;
            blc.CalculaRiscoRelativo = this.ckbRiscoRelativo.Checked;
            blc.CalculaTaxaBruta = this.cktTaxaBruta.Checked;
            blc.CalculaTaxaEspacial = this.ckbTaxaEspacial.Checked;
            blc.CalculaTaxaBayesClaytonGama = this.ckbTaxaClaytonCaldor.Checked & this.ckbRRelGamma.Checked;
            blc.CalculaTaxaBayesClaytonLogN = this.ckbTaxaClaytonCaldor.Checked & this.ckbRRelLogN.Checked;
            blc.CalculaRRelBayesClaytonGama = this.ckbBayesianClayton.Checked & this.ckbRRelGamma.Checked;
            blc.CalculaRRelBayesClaytonLogN = this.ckbBayesianClayton.Checked & this.ckbRRelLogN.Checked;

            blc.CalculaTaxaBayesClaytonGamaEsp = this.ckbTaxaClaytonCaldor.Checked & this.ckbRRelGammaEsp.Checked;
            blc.CalculaTaxaBayesClaytonLogNEsp = this.ckbTaxaClaytonCaldor.Checked & this.ckbRRelLogNEsp.Checked;
            blc.CalculaRRelBayesClaytonGamaEsp = this.ckbBayesianClayton.Checked & this.ckbRRelGammaEsp.Checked;
            blc.CalculaRRelBayesClaytonLogNEsp = this.ckbBayesianClayton.Checked & this.ckbRRelLogNEsp.Checked;

            blc.UsaMultiplicadorTaxas = ckbMultiplicarTaxas.Checked;
            blc.ValorMultiplicadorTaxas = Convert.ToDouble(nudMultiplicadorTaxas.Value);          
  
            if (!(blc.CalculaEmpiricalBayesRate || blc.CalculaEmpiricalSpatialBayesRate ||
                blc.CalculaRiscoRelativo || blc.CalculaTaxaBruta || blc.CalculaTaxaEspacial
                || blc.CalculaTaxaBayesClaytonGama || blc.CalculaTaxaBayesClaytonLogN
                || blc.CalculaRRelBayesClaytonGama || blc.CalculaRRelBayesClaytonLogN
                || blc.CalculaRRelBayesClaytonGamaEsp || blc.CalculaRRelBayesClaytonLogNEsp
                || blc.CalculaTaxaBayesClaytonGamaEsp || blc.CalculaTaxaBayesClaytonLogNEsp))
            {
                throw new Exception("Escolha pelo menos uma forma de cálculo de taxa.");
            }
            
            blc.Shape = this.m_shape;
            blc.TabelaDados = m_dt_tabela_dados;
            blc.VariaveisOcorrencias = m_nomes_variaveis_ocorrencias;
            blc.VariavelPopulacao = this.lstVarPopulacao.SelectedItem.ToString();

            blc.EstimarTaxas(ref progressBar1, ref lblProgressBar);
            
            this.userControlRichTextOutput2.Texto = blc.VariaveisGeradas;
            this.userControlRichTextOutput3.Texto = blc.MedidasResumo;
            tabControl1.SelectedTab = this.tabPage2;

            //dataGridView1.DataSource = blc.TabelaDados;
            //dataGridView1.Refresh();

            userControlDataGrid1.TabelaDados = blc.TabelaDados;
        }

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                int tipo_viz = 0;
                if (radioButton1.Checked) tipo_viz = 1;
                if (radioButton2.Checked) tipo_viz = 3;
                if (radioButton3.Checked) tipo_viz = 2;
                if (radioButton4.Checked) tipo_viz = 4;

                lblProgressBar.Text = "Gerando a matriz de vizinhança";

                m_shape.DefinicaoVizinhos(ref m_shape, tipo_viz, ref progressBar1);

                lblProgressBar.Text = "Matriz de vizinhança gerada com sucesso";

                vizinhanca_ok = true;
                ckbSpatialEmpricalBayes.Enabled = true;
                ckbTaxaEspacial.Enabled = true;
                ckbRRelLogNEsp.Enabled = true;
                ckbRRelGammaEsp.Enabled = true;

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                lblProgressBar.Text = "Erro na geração da matriz de vizinhança";
                MessageBox.Show(er.Message, "Erro - Geração Matriz de Vizinhança", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ckbBayesianClayton_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ckbBayesianClayton.Checked || ckbTaxaClaytonCaldor.Checked) groupBox6.Enabled = true;
            else groupBox6.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void ckbTaxaClaytonCaldor_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbBayesianClayton.Checked || ckbTaxaClaytonCaldor.Checked) groupBox6.Enabled = true;
            else groupBox6.Enabled = false;
        }

        private void nudMultiplicadorTaxas_ValueChanged(object sender, EventArgs e)
        {
        }

        private void ckbMultiplicarTaxas_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ckbMultiplicarTaxas.Checked) nudMultiplicadorTaxas.Enabled = true;
            else nudMultiplicadorTaxas.Enabled = false;
        }

        private void selecionarTodasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ckbEmpiricalBayes.Checked = true;
            ckbRiscoRelativo.Checked = true;
            cktTaxaBruta.Checked = true;
            ckbTaxaClaytonCaldor.Checked = true;
            ckbBayesianClayton.Checked = true;
            ckbRRelGamma.Checked = true;
            ckbRRelLogN.Checked = true;

            if (vizinhanca_ok)
            {
                ckbSpatialEmpricalBayes.Checked = true;
                ckbTaxaEspacial.Checked = true;
                ckbRRelGammaEsp.Checked = true;
                ckbRRelLogNEsp.Checked = true;
            }
        }

        private void limparTodasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ckbEmpiricalBayes.Checked = false;
            ckbSpatialEmpricalBayes.Checked = false;
            ckbRiscoRelativo.Checked = false;
            cktTaxaBruta.Checked = false;
            ckbTaxaEspacial.Checked = false;
            ckbTaxaClaytonCaldor.Checked = false;
            ckbBayesianClayton.Checked = false;
            ckbRRelGamma.Checked = false;
            ckbRRelLogN.Checked = false;
        }
    }
}

