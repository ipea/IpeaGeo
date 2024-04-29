using System;
using System.Data;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public partial class FormRegressaoDadosBinarios : Form
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

                this.Text = "Modelos de Regressão com Resposta Binária - " + value;
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

        private DataTable m_dt_tabela_shape = new DataTable();
        public DataTable DadosShape
        {
            get { return m_dt_tabela_shape; }
            set { m_dt_tabela_shape = value; }
        }

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

        //private clsMapa m_mapa = new clsMapa();
        //public clsMapa Mapa
        //{
        //    get { return m_mapa; }
        //    set { m_mapa = value; }
        //}

        #endregion

        public FormRegressaoDadosBinarios()
        {
            InitializeComponent();
        }

        private void AtualizaTabelaDados(bool atualizaUControl)
        {
            //this.dataGridView1.DataSource = m_dt_tabela_dados;

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
                this.userControlRegressaoInstrumentos1.ZeraControle();
                this.userControlRegressaoInstrumentos1.VariaveisDB = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
                this.userControlRegressaoInstrumentos1.VariaveisList = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
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
            try
            {
                if (ckbIncluirNovasVariaveisTabelaDados.Checked)
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
                    MessageBox.Show("Selecione a opção 'Mostrar novas variáveis na tabela de dados', localizada na aba Especificações", "Atualização", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }                               
        }

        private void FormBaseModelagem_Load(object sender, EventArgs e)
        {
            try
            {
                // Variáveis sendo passadas para o UserControl
                userControlDataGrid1.TabelaDados = this.m_dt_tabela_dados;
                userControlDataGrid1.FuncaoFromFormulario = new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionFromFormulario(this.AtualizaTabelaDados);
                userControlDataGrid1.MostraOpcaoImportadaoDados = true;
                userControlDataGrid1.UserControlRegInstrumentos = this.userControlRegressaoInstrumentos1;

                // Atualizando o user control de especificação da regressão
                this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = false;
                this.userControlRegressaoInstrumentos1.EsconderSelecaoInstrumentos(true);

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
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Regressão Linear", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EstimaModelo()
        {
            clsUtilTools clt = new clsUtilTools();
            BLogicRegressaoDadosBinarios blr = new BLogicRegressaoDadosBinarios();

            blr.VariaveisDependentes = userControlRegressaoInstrumentos1.VariavelDependente;
            blr.VariaveisIndependentes = userControlRegressaoInstrumentos1.VariaveisIndependentes;
            blr.TabelaDados = this.m_dt_tabela_dados;
            blr.AdicionaNovaVariaveis = ckbIncluirNovasVariaveisTabelaDados.Checked;
            blr.erro = clt.DoubleFromTexto(this.textBox1.Text);
            
            if (ckbLimpaJanelaOutput.Checked)
            {
                this.userControlRichTextOutput1.Texto = "";
            }

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

            blr.ChecaTodosValoresIguais();

            string mensagem_binarios = blr.ChecaDadosBinarios();
            if (mensagem_binarios != "")
            {
                DialogResult result1 = MessageBox.Show(mensagem_binarios,
                                                        "Checagem de Dados Binários",
                                                        MessageBoxButtons.YesNo);
                if (result1 == DialogResult.No)
                {
                    return;
                }
            }

            #region checando multicolinearidade perfeita das regressores

            string mensagem_colinear;

            if (blr.ChecarMulticolinearidade(out mensagem_colinear))
            {
                MessageBox.Show(mensagem_colinear + " Cheque a sua base de dados ou mude a especificação do modelo.");
                return;
            }

            #endregion

            if (rdbLinkLogit.Checked) blr.FuncaoLigacao = TipoFuncaoLigacao.Logit;
            if (rdbProbit.Checked) blr.FuncaoLigacao = TipoFuncaoLigacao.Probit;
            if (rdbCompLogLog.Checked) blr.FuncaoLigacao = TipoFuncaoLigacao.Cloglog;

            bool classificationchecked = false;

            if (this.ckbClassificationTable.Checked)
            {
                classificationchecked = true;
                blr.mclassificationtablechecked = classificationchecked;
            }

            if (this.ckbObservacoesInfluente.Checked)
            {
                blr.minfluencechecked = true;
            }

            bool residuoscheked = false;

            if (this.ckbAnaliseResiduos.Checked)
            {
                residuoscheked = true;
                blr.mresiduoschecked = residuoscheked;
            }

            blr.mcutoffCT = Convert.ToDouble(numericUpDownCT.Value);

            blr.EstimarModeloRegressao();

            double[,] prob_predita = blr.ProbabilidadePredita;

            this.userControlRichTextOutput1.Texto += blr.ResultadoEstimacao + "\n\n";
            this.userControlRichTextOutput2.Texto = blr.VariaveisGeradas + "\n\n";

            if (!this.tabControl1.TabPages.Contains(tabPage2))
            {
                tabControl1.TabPages.Add(tabPage2);
            }
            tabControl1.SelectedTab = tabPage2;

            if (!this.tabControl1.TabPages.Contains(tabPage4))
            {
                tabControl1.TabPages.Add(tabPage4);
            }

            if (this.ckbMulticolinearidade.Checked)
            {
                this.userControlRichTextOutput1.Texto += blr.AnaliseMulticolinearidade();
            }
            else
            {
                if (this.ckbApresentaCovMatrixBetaHat.Checked)
                    this.userControlRichTextOutput1.Texto += blr.Analisedecorrelacao();
            }
            
            if (blr.AdicionaNovaVariaveis)
            {
                this.userControlDataGrid1.TabelaDados = blr.TabelaDados;
            }
        }

        private void numericUpDownCT_ValueChanged(object sender, EventArgs e)
        {
        }

        private void ckbClassificationTable_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbClassificationTable.Checked)
            {
                numericUpDownCT.Enabled = true;
            }
            else
            {
                numericUpDownCT.Enabled = false;
            }
        }

        private void ckbAnaliseResiduos_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbAnaliseResiduos.Checked)
            {
             ckbObservacoesInfluente.Enabled = true;
            }
            else
            {
             ckbObservacoesInfluente.Enabled = false;
             ckbObservacoesInfluente.Checked = false;
            }
        }
    }
}
