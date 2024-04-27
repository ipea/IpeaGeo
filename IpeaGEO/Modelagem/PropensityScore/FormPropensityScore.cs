using System;
using System.Data;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public partial class FormPropensityScore : Form
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

                this.Text = "Propensity Score Matching - " + value;
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
                    this.btnPSM.Enabled = true;
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

        #endregion

        public FormPropensityScore()
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
                this.userControlPropensityScoreMatching1.ZeraControle();
                this.userControlPropensityScoreMatching1.VariaveisDB = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
                this.userControlPropensityScoreMatching1.VariaveisList = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);

                                             
                             
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
            // Variáveis sendo passadas para o UserControl
            userControlDataGrid1.TabelaDados = this.m_dt_tabela_dados;
            userControlDataGrid1.FuncaoFromFormulario = new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionFromFormulario(this.AtualizaTabelaDados);
            userControlDataGrid1.MostraOpcaoImportadaoDados = true;
            userControlDataGrid1.UserControlPropScoreMatching = this.userControlPropensityScoreMatching1;

            // Atualizando o user control de especificação da regressão
            this.userControlPropensityScoreMatching1.SelecaoInstrumentosDisponivel = false;
            this.userControlPropensityScoreMatching1.LabelListaVarsDependentes = "Variável indicadora do tratamento";
            this.userControlPropensityScoreMatching1.LabelListaVarsIndependentes = "Variáveis para cálculo do propensity score";

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

        private void button2_Click(object sender, EventArgs e)
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
                MessageBox.Show(er.Message, "Propensity Score Matching", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EstimaModelo()
        {
            clsUtilTools clt = new clsUtilTools();
            BLogicRegressaoDadosBinarios blr = new BLogicRegressaoDadosBinarios();
            BLogicPropensityScoreMatching blrpsMatch = new BLogicPropensityScoreMatching();





            if (ckbLimpaJanelaOutput.Checked)
            {
                this.userControlRichTextOutput1.Texto = "";
            }

            #region Checando valores inválidos

            int[] indicadores_val_invalidos;

            for (int i = 0; i < userControlPropensityScoreMatching1.VariavelDependente.GetLength(0); i++)
            {
                if (clt.ChecaValoresDoubleInvalidos(this.m_dt_tabela_dados, userControlPropensityScoreMatching1.VariavelDependente[i], out indicadores_val_invalidos))
                {
                    MessageBox.Show("Há valores double inválidos na variável " + userControlPropensityScoreMatching1.VariavelDependente[i] + ". Cheque a sua base de dados.");
                    return;
                }
            }

            for (int i = 0; i < userControlPropensityScoreMatching1.VariaveisIndependentes.GetLength(0); i++)
            {
                if (clt.ChecaValoresDoubleInvalidos(this.m_dt_tabela_dados, userControlPropensityScoreMatching1.VariaveisIndependentes[i], out indicadores_val_invalidos))
                {
                    MessageBox.Show("Há valores double inválidos na variável " + userControlPropensityScoreMatching1.VariaveisIndependentes[i] + ". Cheque a sua base de dados.");
                    return;
                }
            }

            #endregion

            #region Carrega as variaveis

            blrpsMatch.VariaveisDependentes = userControlPropensityScoreMatching1.VariavelDependente;
            blr.VariaveisDependentes = (string[])blrpsMatch.VariaveisDependentes.Clone();
            blrpsMatch.VariaveisIndependentes = userControlPropensityScoreMatching1.VariaveisIndependentes;
            blr.VariaveisIndependentes = (string[])blrpsMatch.VariaveisIndependentes.Clone();
            blrpsMatch.TabelaDados = this.m_dt_tabela_dados;
            blrpsMatch.nCat = (int)this.numericUpDown_numeroTratamentos.Value;
            blrpsMatch.Bandwidth = clt.DoubleFromTexto(txtBoxBandwidth.Text);
            blrpsMatch.criaMatrizDif();

            //Define o tipo de vizinhança
            //TODO: Mudei a disposicao dos checkbox
            if (rdbNearestNeighbo.Checked) blrpsMatch.Matching = TipoDeMatching.NearestNeighbo;
            if (rdbStratification.Checked) blrpsMatch.Matching = TipoDeMatching.Stratification;
            if (rdbKernel.Checked)
                switch (comboBoxListaKernel.SelectedIndex)
                {
                    case 0:
                        blrpsMatch.Matching = TipoDeMatching.Gaussiano;
                        break;
                    case 1:
                        blrpsMatch.Matching = TipoDeMatching.Epanechnikov;
                        break;
                    case 2:
                        blrpsMatch.Matching = TipoDeMatching.Biweight;
                        break;
                    case 3:
                        blrpsMatch.Matching = TipoDeMatching.Triangular;
                        break;
                    case 4:
                        blrpsMatch.Matching = TipoDeMatching.Retangular;
                        break;
                    default:
                        blrpsMatch.Matching = TipoDeMatching.Gaussiano;
                        break;
                }


            switch (cbxAT.SelectedIndex)
            {
                case 0:
                    blrpsMatch.AT = TipoDeAverage.ATT;
                    break;
                case 1:
                    blrpsMatch.AT = TipoDeAverage.ATE;
                    break;
                default:
                    blrpsMatch.AT = TipoDeAverage.ATT;
                    break;
            }


            double[,] variavelCategoria = clt.GetMatrizFromDataTable(blrpsMatch.TabelaDados, blrpsMatch.VariaveisDependentes);

            #endregion

            //TODO: adicionei este teste de bandwidth

            #region Checa bandwidth

            if (rdbBandwidthA.Checked)
                blrpsMatch.Bandwidth = 0.0;
            else
                if (((blrpsMatch.Bandwidth <= 0) || double.IsNaN(blrpsMatch.Bandwidth)) && (rdbKernel.Checked))
                {
                    MessageBox.Show("Para utilizar um kernel é preciso definir um bandwidth positivo");
                    return;
                }

            #endregion

            blrpsMatch.ChecaTodosValoresIguais();

            #region Checando multicolinearidade perfeita das regressores

            string mensagem_colinear;

            if (blrpsMatch.ChecarMulticolinearidade(out mensagem_colinear))
            {
                MessageBox.Show(mensagem_colinear + " Cheque a sua base de dados ou mude a especificação do modelo.");
                return;
            }

            #endregion

            #region Checa numero de categorias

            string mensagem_categorias = blrpsMatch.ChecaNumeroCategorias(blrpsMatch.nCat);
            if (mensagem_categorias != "")
            {
                DialogResult result1 = MessageBox.Show(mensagem_categorias,
                                                        "Checagem de Numero de Categorias",
                                                        MessageBoxButtons.YesNo);
                if (result1 == DialogResult.No)
                {
                    return;
                }
                else
                    variavelCategoria = blrpsMatch.RespostaDiscreta(variavelCategoria, blrpsMatch.nCat);
            }

            #endregion

            #region Gera as diferenças de media para cada par de categorias

            for (int categoriaBase = 0; categoriaBase < (blrpsMatch.nCat); categoriaBase++)
            {
                for (int categoriaComp = 0; categoriaComp < (blrpsMatch.nCat); categoriaComp++)
                {
                    if (categoriaBase == categoriaComp)
                    {
                        blrpsMatch.setMatrizDif(0.0, categoriaBase, categoriaComp, 0);
                        blrpsMatch.setMatrizDif(0.0, categoriaBase, categoriaComp, 1);
                        blrpsMatch.setMatrizDif(0.0, categoriaBase, categoriaComp, 2);
                        blrpsMatch.setMatrizDif(0.0, categoriaBase, categoriaComp, 3);
                    }
                    else
                    {

                        blr.TabelaDados = blrpsMatch.TabelaDados.Copy();

                        for (int z = 0; z < variavelCategoria.GetLength(0); z++)
                            if (variavelCategoria[z, 0] == blrpsMatch.categorias[categoriaBase])
                                blr.TabelaDados.Rows[z][blr.VariaveisDependentes[0]] = 1.0;
                            else if (variavelCategoria[z, 0] == blrpsMatch.categorias[categoriaComp])
                                blr.TabelaDados.Rows[z][blr.VariaveisDependentes[0]] = 0.0;
                            else
                                blr.TabelaDados.Rows[z].Delete();

                        blr.TabelaDados.AcceptChanges();

                        #region Checa dados binarios

                        string mensagem_binarios = blr.ChecaDadosBinarios();
                        if (mensagem_binarios != "")
                        {
                            DialogResult result1 = MessageBox.Show(mensagem_binarios,
                                                                    "Checagem de Dados Binarios",
                                                                    MessageBoxButtons.YesNo);
                            if (result1 == DialogResult.No)
                            {
                                return;
                            }
                        }

                        #endregion

                        #region Determina a funcao de ligacao

                        if (rdbLinkLogit.Checked) blr.FuncaoLigacao = TipoFuncaoLigacao.Logit;
                        if (rdbProbit.Checked) blr.FuncaoLigacao = TipoFuncaoLigacao.Probit;
                        if (rdbCompLogLog.Checked) blr.FuncaoLigacao = TipoFuncaoLigacao.Cloglog;

                        #endregion

                        //Roda o modelo de regressão para dados binários.
                        blr.EstimarModeloRegressao();

                        blrpsMatch.imprimirResultadoBinario(blr.ResultadoEstimacao, blr.VariaveisGeradas, categoriaBase, categoriaComp);

                        #region Cria o Matching

                        //Checa o Outcome
                        //blrpsMatch.ChecaOutcome();

                        //Define o vetor de outcomes
                        double[] dblOutocme = new double[blr.TabelaDados.Rows.Count];
                        for (int i = 0; i < blr.TabelaDados.Rows.Count; i++) dblOutocme[i] = (double)blr.TabelaDados.Rows[i][cmbOutcome.SelectedItem.ToString()];
                        blrpsMatch.Outcome = dblOutocme;


                        if (rdbStratification.Checked)
                        {
                            //Define o vetor de estrato
                            double[,] dblEstrato = new double[blr.TabelaDados.Rows.Count, 1];
                            for (int i = 0; i < blr.TabelaDados.Rows.Count; i++) dblEstrato[i, 0] = (double)blr.TabelaDados.Rows[i][cmbEstrato.SelectedItem.ToString()];
                            blrpsMatch.Estrato = dblEstrato;
                        }
                        //Guarda os participantes
                        double[] dblParticipantes = new double[blr.TabelaDados.Rows.Count];
                        for (int i = 0; i < blr.TabelaDados.Rows.Count; i++) dblParticipantes[i] = (double)blr.TabelaDados.Rows[i][userControlPropensityScoreMatching1.VariavelDependente[0]];
                        blrpsMatch.Participante = dblParticipantes;

                        //Executa o Matching Associado aos valores preditos
                        blrpsMatch.GeraMatching(blr.ProbabilidadePredita, categoriaBase, categoriaComp);

                        //blr.ResultadoEstimacao;
                        //    blr.

                        #endregion

                        #region Faz a análise do teste de médias



                        #endregion
                    }
                }
            }
            #endregion
            if (cbxAT.Text=="ATT")
            {
                blrpsMatch.imprimirResultadoPropensityScore(cmbOutcome.Text);
            }
            else
            {
                blrpsMatch.imprimirResultadoPropensityScoreATE(cmbOutcome.Text);
            }
            this.userControlRichTextOutput1.Texto += blrpsMatch.ResultadoEstimacao + "\n\n";
            this.userControlRichTextOutput1.Texto += blrpsMatch.outBin[0] + "\n\n";
            this.userControlRichTextOutput2.Texto += blrpsMatch.outBin[1] + "\n\n";
            if (!this.tabControl1.TabPages.Contains(tabPage2))
            {
                tabControl1.TabPages.Add(tabPage2);
            }
            tabControl1.SelectedTab = tabPage2;
        }
        private void rdbKernel_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbKernel.Checked)
            {
                comboBoxListaKernel.Enabled = true;
                comboBoxListaKernel.Visible = true;
                gbxBandWidth.Visible = true;
            }
            else
            {
                comboBoxListaKernel.Enabled = false;
                comboBoxListaKernel.Visible = false;
                gbxBandWidth.Visible = false;
            }

        }

        // Selecionando para o Outcome as variavis disponiveis menos as indep. e a dependente
        private void cmbOutcome_Click(object sender, EventArgs e)
        {
            int a = 0;
            if (cmbOutcome.Visible)
                a = cmbOutcome.Items.Count;
            if (a > 0)
            {
                for (int i = 0; i < a; i++)
                {
                    cmbOutcome.Items.RemoveAt(0);
                }
            }
                for (int i = 0; i < userControlPropensityScoreMatching1.VariaveisList.Length; i++)
                {
                    cmbOutcome.Items.Add(userControlPropensityScoreMatching1.VariaveisList[i]);
                }
                string texto = "";
                for (int i = 0; i < userControlPropensityScoreMatching1.VariaveisIndependentes.Length; i++)
                {
                    texto = userControlPropensityScoreMatching1.VariaveisIndependentes[i];
                    cmbOutcome.Items.Remove(texto);
                }
                for (int i = 0; i < userControlPropensityScoreMatching1.VariavelDependente.Length; i++)
                {
                    texto = userControlPropensityScoreMatching1.VariavelDependente[i];
                    cmbOutcome.Items.Remove(texto);
                }
        }
        private void cmbEstrato_Click(object sender, EventArgs e)
        {
            int a = 0;
            if (cmbEstrato.Visible)
                a = cmbEstrato.Items.Count;
            if (a > 0)
            {
                for (int i = 0; i < a; i++)
                {
                    cmbEstrato.Items.RemoveAt(0);
                }
            }
            for (int i = 0; i < userControlPropensityScoreMatching1.VariaveisList.Length; i++)
            {
                cmbEstrato.Items.Add(userControlPropensityScoreMatching1.VariaveisList[i]);
            }

            string texto = "";
            for (int i = 0; i < userControlPropensityScoreMatching1.VariaveisIndependentes.Length; i++)
            {
                texto = userControlPropensityScoreMatching1.VariaveisIndependentes[i];
                cmbEstrato.Items.Remove(texto);
            }
            for (int i = 0; i < userControlPropensityScoreMatching1.VariavelDependente.Length; i++)
            {
                texto = userControlPropensityScoreMatching1.VariavelDependente[i];
                cmbEstrato.Items.Remove(texto);
            }
            texto = cmbOutcome.Text;
            cmbEstrato.Items.Remove(texto);
        }

        private void rdbStratification_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbStratification.Checked)
            {
                cmbEstrato.Visible = true;
                label2.Visible = true;
            }
            else
            {
                cmbEstrato.Visible = false;
                label2.Visible = false;
            }
        }

        private void rdbBandwidthA_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbBandwidthA.Checked)
            {
                txtBoxBandwidth.Enabled = false;
            }
            else
            {
                txtBoxBandwidth.Enabled = true;
            }
        }
    } 
}
