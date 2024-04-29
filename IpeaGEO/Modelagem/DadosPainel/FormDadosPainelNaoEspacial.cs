using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public partial class FormDadosPainelNaoEspacial : FormStatisticalAnalysis
    {
        #region Variáveis internas

        public new string VariavelPeriodosPainel
        {
            set 
            { 
                m_variavel_periodos = value;

                if (cmbVariavelPeriodosTempo.Items.Contains(value))
                {
                    cmbVariavelPeriodosTempo.SelectedItem = value;
                    cmbVariavelPeriodosTempo.Enabled = false;
                }
            }
        }

        public new string VariavelUnidadesPainel
        {
            set 
            {
                m_variavel_unidades = value;

                if (this.cmbVariavelUnidadesObservacionais.Items.Contains(value))
                {
                    this.cmbVariavelUnidadesObservacionais.SelectedItem = value;
                    this.cmbVariavelUnidadesObservacionais.Enabled = false;
                }
            }
        }

        public override void HabilitarDadosExternos()
        {
            this.btnOK.Visible =
                btnOK.Enabled = true;
        }

        public new DataSet DadosPainelEspacial
        {
            get { return m_ds_dados_painel_espacial; }
            set 
            { 
                m_ds_dados_painel_espacial = value;

                m_dt_tabela_dados = ((DataTable)value.Tables[0]).Copy();
                for (int k = 1; k < m_ds_dados_painel_espacial.Tables.Count; k++)
                {
                    m_dt_tabela_dados.Merge(((DataTable)m_ds_dados_painel_espacial.Tables[k]).Copy());
                }

                AtualizaTabelaDados(true);

                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    this.btnExecutar.Enabled = true;
                }

                this.userControlDataGrid1.HabilitarImportacaoDados = false;
            }
        }

        public new string LabelTabelaDados
        {
            set
            {
                m_label_tabela_dados = value;

                this.Text = "Regressão Linear com Dados de Painel - " + value;
            }
        }

        private DataTable m_dt_tabela_shape = new DataTable();
        public DataTable DadosShape
        {
            get { return m_dt_tabela_shape; }
            set { m_dt_tabela_shape = value; }
        }

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

        public FormDadosPainelNaoEspacial()
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
                string[] todas_variaveis = clt.RetornaTodasColunas(m_dt_tabela_dados);
                string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);

                this.userControlRegressaoInstrumentos1.ZeraControle();
                this.userControlRegressaoInstrumentos1.VariaveisDB = variaveis_numericas;
                this.userControlRegressaoInstrumentos1.VariaveisList = variaveis_numericas;

                this.cmbVariavelPeriodosTempo.Items.Clear();
                this.cmbVariavelPeriodosTempo.Items.AddRange(todas_variaveis);
                this.cmbVariavelPeriodosTempo.SelectedIndex = 0;

                this.cmbVariavelUnidadesObservacionais.Items.Clear();
                this.cmbVariavelUnidadesObservacionais.Items.AddRange(todas_variaveis);
                this.cmbVariavelUnidadesObservacionais.SelectedIndex = 0;
            }

            btnOpções.Enabled = true;

            if (!this.tabControl1.TabPages.Contains(tabPage0))
            {
                tabControl1.TabPages.Add(tabPage0);
            }

            if (!this.tabControl1.TabPages.Contains(tabPage6))
            {
                tabControl1.TabPages.Add(tabPage6);
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
                Cursor = Cursors.WaitCursor;

                AtualizaDataSetPainelEspacial();

                Cursor = Cursors.Default;

                //this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AtualizaDataSetPainelEspacial()
        {
            DadosPainelEspacial.Tables.Clear();

            DataTable dt = (DataTable)userControlDataGrid1.TabelaDados.Copy();
            DataTable temp = new DataTable();
            string periodo = "";

            for (int k = 0; k < m_lista_periodos_painel_espacial.GetLength(0); k++)
            {
                periodo = m_lista_periodos_painel_espacial[k, 0].ToString();
                temp = dt.Clone();
                temp.TableName = periodo;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][m_variavel_periodos].ToString() == periodo)
                    {
                        temp.Rows.Add(dt.Rows[i].ItemArray);
                    }
                }
                DadosPainelEspacial.Tables.Add(temp.Copy());

                if (periodo == m_periodo_foco)
                {
                    m_gridview_externo.DataSource = temp.Copy();

                    this.DataSetExterno.Tables.Clear();
                    this.DataSetExterno.Tables.Add(temp.Copy());
                }
            }
        }

        private void FormBaseModelagem_Load(object sender, EventArgs e)
        {
            // Variáveis sendo passadas para o UserControl
            userControlDataGrid1.TabelaDados = this.m_dt_tabela_dados;
            userControlDataGrid1.FuncaoFromFormulario = new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionFromFormulario(this.AtualizaTabelaDados);
            userControlDataGrid1.MostraOpcaoImportadaoDados = true;
            userControlDataGrid1.UserControlRegInstrumentos = this.userControlRegressaoInstrumentos1;

            // Atualizando o user control de especificação da regressão
            this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = false;
            this.userControlRegressaoInstrumentos1.EsconderSelecaoInstrumentos(true);

            if (this.tabControl1.TabPages.Contains(tabPage0))
                this.tabControl1.TabPages.Remove(tabPage0);

            if (this.tabControl1.TabPages.Contains(tabPage2))
                this.tabControl1.TabPages.Remove(tabPage2);

            if (this.tabControl1.TabPages.Contains(tabPage3))
                this.tabControl1.TabPages.Remove(tabPage3);

            if (this.tabControl1.TabPages.Contains(tabPage4))
                this.tabControl1.TabPages.Remove(tabPage4);

            if (this.tabControl1.TabPages.Contains(tabPage5))
                this.tabControl1.TabPages.Remove(tabPage5);

            if (this.tabControl1.TabPages.Contains(tabPage6))
                this.tabControl1.TabPages.Remove(tabPage6);
        }

        #region Eventos

        private void ckbCovMatrizRobusta_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbCovMatrizRobusta.Checked)
            {
                this.cktGeneralFGLS.Checked = false;
                this.cktGeneralFGLS.Enabled = false;
            }
            else
            {
                this.cktGeneralFGLS.Enabled = true;
            }
        }

        private void cktGeneralFGLS_CheckedChanged(object sender, EventArgs e)
        {
            if (cktGeneralFGLS.Checked)
            {
                this.ckbCovMatrizRobusta.Checked = false;
                this.ckbCovMatrizRobusta.Enabled = false;
            }
            else
            {
                this.ckbCovMatrizRobusta.Enabled = true;
            }
        }

        private void rdbFirstDiferencesEstimator_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFirstDiferencesEstimator.Checked)
            {
                this.ckbIncluirNovasVariaveisTabelaDados.Enabled = true;

                this.ckbIncluiIntercepto.Checked = false;                            
                this.ckbIncluiIntercepto.Enabled = false;

                ckbIncluiTendenciaTemporalLinear.Enabled =
                    ckbIncluiTendenciaTemporalQuadratica.Enabled =
                    ckbTendenciaTemporalCubica.Enabled = true;

                ckbIncluiTendenciaTemporalLinear.Checked =
                    ckbIncluiTendenciaTemporalQuadratica.Checked =
                    ckbTendenciaTemporalCubica.Checked = false;

                ckbIncluiTendenciaTemporalLinear.Enabled =
                    ckbIncluiTendenciaTemporalQuadratica.Enabled =
                    ckbTendenciaTemporalCubica.Enabled = false;
            }
            else
            {
                ckbIncluiTendenciaTemporalLinear.Enabled =
                    ckbIncluiTendenciaTemporalQuadratica.Enabled =
                    ckbTendenciaTemporalCubica.Enabled = true;
            }
        }

        private void rdbFixedEffects_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbFixedEffects.Checked)
            {
                this.ckbIncluirNovasVariaveisTabelaDados.Enabled = true;

                this.ckbIncluiIntercepto.Checked = false;
                this.ckbIncluiIntercepto.Enabled = false;

                ckbIncluiTendenciaTemporalLinear.Enabled =
                    ckbIncluiTendenciaTemporalQuadratica.Enabled =
                    ckbTendenciaTemporalCubica.Enabled = true;
            }
        }

        private void rdbPooledOLS_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbPooledOLS.Checked)
            {
                this.ckbIncluirNovasVariaveisTabelaDados.Enabled = true;
                this.ckbIncluiIntercepto.Enabled = true;

                this.ckbIncluiIntercepto.Checked = true;

                ckbIncluiTendenciaTemporalLinear.Enabled =
                    ckbIncluiTendenciaTemporalQuadratica.Enabled =
                    ckbTendenciaTemporalCubica.Enabled = true;
            }
        }

        private void rdbRandomEffects_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbRandomEffects.Checked)
            {
                this.ckbIncluirNovasVariaveisTabelaDados.Enabled = true;
                this.ckbIncluiIntercepto.Enabled = true;

                this.ckbIncluiIntercepto.Checked = true;

                ckbIncluiTendenciaTemporalLinear.Enabled =
                    ckbIncluiTendenciaTemporalQuadratica.Enabled =
                    ckbTendenciaTemporalCubica.Enabled = true;
            }
        }

        #endregion
        
        #region estimação do modelo
        
        private void btnExecutar2_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                EstimarModelo();

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                this.lblProgressBar.Text = "Falha na estimacao do modelo de painel";
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                EstimarModelo();

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                this.lblProgressBar.Text = "Falha na estimacao do modelo de painel";
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private BLogicRegressaoLinearDadosPainel m_painel = new BLogicRegressaoLinearDadosPainel();

        private void EstimarModelo()
        {
            string[] vars_independentes = userControlRegressaoInstrumentos1.VariaveisIndependentes;
            string[] vars_dependentes = userControlRegressaoInstrumentos1.VariavelDependente;
            string[] vars_instrumentais = userControlRegressaoInstrumentos1.VariaveisInstrumentais;
            
            #region checando valores inválidos

            clsUtilTools clt = new clsUtilTools();

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
            
            if (vars_dependentes.GetLength(0) <= 0)
            {
                throw new Exception("Escolha uma variável dependente para o modelo de regressão.");
            }

            if (vars_independentes.GetLength(0) <= 0)
            {
                throw new Exception("Escolha pelo menos uma variávei independente para o modelo de regressão.");
            }

            #endregion

            #region checando multicolinearidade perfeita das regressores

            BLogicRegressaoLinear blr = new BLogicRegressaoLinear();

            blr.VariaveisIndependentes = userControlRegressaoInstrumentos1.VariaveisIndependentes;
            blr.TabelaDados = this.m_dt_tabela_dados;
            blr.intercepto = ckbIncluiIntercepto.Checked;

            string mensagem_colinear;

            if (blr.ChecarMulticolinearidade(out mensagem_colinear))
            {
                MessageBox.Show("Multicolinearidade dos regressores", mensagem_colinear + "\n" + "Cheque a sua base de dados ou mude a especificação do modelo.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #endregion

            if (cmbVariavelPeriodosTempo.SelectedItem.ToString() == cmbVariavelUnidadesObservacionais.SelectedItem.ToString())
            {
                throw new Exception("A variável de tempo não pode ser a mesma variável das unidades observacionais. Refaça a sua escolha."); 
            }

            m_painel.TabelaDados = m_dt_tabela_dados;
            m_painel.VariaveisDependentes = vars_dependentes;
            m_painel.VariaveisInstrumentais = vars_instrumentais;
            m_painel.VariaveisIndependentes = vars_independentes;
            m_painel.IncluiIntercepto = this.ckbIncluiIntercepto.Checked;
            m_painel.UsaGeneralFGLS = cktGeneralFGLS.Checked;
            m_painel.UsaRobustCovMatrix = ckbCovMatrizRobusta.Checked;
            m_painel.AdicionaNovaVariaveis = ckbIncluirNovasVariaveisTabelaDados.Checked;
            m_painel.ApresentaCovMatrixBetaHat = ckbApresentaCovMatrixBetaHat.Checked;

            m_painel.UsaDummiesTemporais = ckbIncluiDummiesTemporais.Checked;
            m_painel.UsaTendenciaTemporalLinear = ckbIncluiTendenciaTemporalLinear.Checked;
            m_painel.UsaTendenciaTemporalQuadratica = ckbIncluiTendenciaTemporalQuadratica.Checked;
            m_painel.UsaTendenciaTemporalCubica = ckbTendenciaTemporalCubica.Checked;

            if (rdbPooledOLS.Checked) m_painel.TipoEstimacaoPainel = TipoEstimacaoDadosPainel.PooledOLS;
            if (rdbFixedEffects.Checked) m_painel.TipoEstimacaoPainel = TipoEstimacaoDadosPainel.EfeitosFixos;
            if (rdbRandomEffects.Checked) m_painel.TipoEstimacaoPainel = TipoEstimacaoDadosPainel.EfeitosAleatorios;
            if (rdbFirstDiferencesEstimator.Checked) m_painel.TipoEstimacaoPainel = TipoEstimacaoDadosPainel.PrimeirasDiferencas;

            m_painel.VarUnidadeObservacional = cmbVariavelUnidadesObservacionais.SelectedItem.ToString();
            m_painel.VarUnidadeTemporal = cmbVariavelPeriodosTempo.SelectedItem.ToString();

            m_painel.EstimarModelo();

            if (!this.tabControl1.TabPages.Contains(tabPage2))
                this.tabControl1.TabPages.Add(tabPage2);
            this.tabControl1.SelectedTab = tabPage2;

            if (!this.tabControl1.TabPages.Contains(tabPage4))
                this.tabControl1.TabPages.Add(tabPage4);

            if (ckbLimpaJanelaOutput.Checked)
            {
                userControlRichTextOutput1.Texto = "";
                userControlRichTextOutput3.Texto = "";
            }

            if (ckbLimpaJanelaNovasVariaveis.Checked)
            {
                userControlRichTextOutput2.Texto = "";
            }

            this.userControlRichTextOutput1.Texto += m_painel.ResultadoEstimacao;
            this.userControlRichTextOutput2.Texto += m_painel.VariaveisGeradas;

            if (rdbPooledOLS.Checked) lblProgressBar.Text = "Modelo de painel estimado via pooled OLS";
            if (rdbFixedEffects.Checked) lblProgressBar.Text = "Modelo de painel estimado via efeitos fixos";
            if (rdbRandomEffects.Checked) lblProgressBar.Text = "Modelo de painel estimado via efeitos aleatorios";
            if (rdbFirstDiferencesEstimator.Checked) lblProgressBar.Text = "Modelo de painel estimado via primeiras diferencas";

            if (m_painel.TipoEstimacaoPainel == TipoEstimacaoDadosPainel.EfeitosFixos || m_painel.TipoEstimacaoPainel == TipoEstimacaoDadosPainel.PrimeirasDiferencas)
            {
                if (!this.tabControl1.TabPages.Contains(tabPage5))
                    this.tabControl1.TabPages.Add(tabPage5);

                this.userControlRichTextOutput3.Texto += m_painel.TextoEfeitosFixos;
            }
            else
            {
                if (this.tabControl1.TabPages.Contains(tabPage5))
                    this.tabControl1.TabPages.Remove(tabPage5);
            }

            this.userControlDataGrid1.TabelaDados = this.m_painel.TabelaDados;
        }

        private void btnOpções_Click(object sender, EventArgs e)
        {
            if (!this.tabControl1.TabPages.Contains(tabPage6))
                this.tabControl1.TabPages.Add(tabPage6);

            this.tabControl1.SelectedTab = tabPage6;
        }

        #endregion

        #region eventos

        private void ckbIncluiDummiesTemporais_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbIncluiDummiesTemporais.Checked)
            {
                this.ckbIncluiTendenciaTemporalLinear.Checked =
                    this.ckbIncluiTendenciaTemporalQuadratica.Checked =
                    this.ckbTendenciaTemporalCubica.Checked = false;

                this.ckbIncluiTendenciaTemporalLinear.Enabled =
                    this.ckbIncluiTendenciaTemporalQuadratica.Enabled =
                    this.ckbTendenciaTemporalCubica.Enabled = false;
            }
            else
            {
                this.ckbIncluiTendenciaTemporalLinear.Enabled =
                    this.ckbIncluiTendenciaTemporalQuadratica.Enabled =
                    this.ckbTendenciaTemporalCubica.Enabled = true;
            }
        }

        private void ckbIncluiTendenciaTemporalLinear_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbIncluiTendenciaTemporalLinear.Checked || ckbIncluiTendenciaTemporalQuadratica.Checked || this.ckbTendenciaTemporalCubica.Checked)
            {
                this.ckbIncluiDummiesTemporais.Checked = false;
                this.ckbIncluiDummiesTemporais.Enabled = false;
            }
            else
            {
                this.ckbIncluiDummiesTemporais.Enabled = true;
            }
        }

        private void ckbIncluiTendenciaTemporalQuadratica_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbIncluiTendenciaTemporalLinear.Checked || ckbIncluiTendenciaTemporalQuadratica.Checked || this.ckbTendenciaTemporalCubica.Checked)
            {
                this.ckbIncluiDummiesTemporais.Checked = false;
                this.ckbIncluiDummiesTemporais.Enabled = false;
            }
            else
            {
                this.ckbIncluiDummiesTemporais.Enabled = true;
            }
        }

        private void ckbTendenciaTemporalCubica_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbIncluiTendenciaTemporalLinear.Checked || ckbIncluiTendenciaTemporalQuadratica.Checked || this.ckbTendenciaTemporalCubica.Checked)
            {
                this.ckbIncluiDummiesTemporais.Checked = false;
                this.ckbIncluiDummiesTemporais.Enabled = false;
            }
            else
            {
                this.ckbIncluiDummiesTemporais.Enabled = true;
            }
        }

        #endregion

        #region ajuste automatico de possiveis variaveis para os periodos

        private IpeaGeo.RegressoesEspaciais.clsUtilTools m_clt = new clsUtilTools();

        private void cmbVariavelUnidadesObservacionais_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                ArrayList lista = this.m_painel.possivel_lista_vars_periodos(m_dt_tabela_dados, this.cmbVariavelUnidadesObservacionais.SelectedItem.ToString());

                if (lista.Count > 0)
                {
                    if (cmbVariavelPeriodosTempo.Items.Contains(lista[0].ToString()))
                    {
                        cmbVariavelPeriodosTempo.SelectedItem = lista[0].ToString();
                    }
                }

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion 
    }
}

