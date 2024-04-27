using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Threading;
using System.Collections;

namespace IpeaGeo.RegressoesEspaciais
{
    public partial class FormRegressaoGMMConley : Form
    {
        #region Carrega formulário

        public FormRegressaoGMMConley()
        {
            InitializeComponent();
            tabControl1.SelectTab(1);
            
        }
        private Thread m_thread_estimacoes;
        private Thread m_thread_progress_bar;
        private bool m_shape_carregado_do_mapa = false;
        private DataTable m_dt_dados_originais = new DataTable();

        public void HabilitarDadosExternos()
        {
            btnAtualizarTabelaExterna.Visible = true;
            btnAtualizarTabelaExterna.Enabled = true;

            btnAbrirArquivoShape.Enabled =
                btnImportarTabela.Enabled = 
                btnConcatenarDados.Enabled = false;

            m_shape_carregado_do_mapa = true;
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

                this.Text = "Modelos de Regressão com Dados Espaciais - " + value;
            }
        }

        private DataTable m_dt_tabela_dados = new DataTable();
        public DataTable TabelaDeDados
        {
            get { return this.m_dt_tabela_dados; }
            set 
            { 
                this.m_dt_tabela_dados = value;
                
                AtualizaTabelaDados();

                if (value.Rows.Count > 0 && value.Columns.Count > 0)
                {
                    this.btnEstimarModelo.Enabled =
                        this.btnEstimarModelo1.Enabled = true;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool m_form_iniciado = false;

        private void AtualizaTabelaDadosCalculadora()
        {
            clsUtilTools clt = new clsUtilTools();

            this.userControlRegressaoInstrumentos1.ZeraControle();

            this.userControlRegressaoInstrumentos1.VariaveisDB = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
            this.userControlRegressaoInstrumentos1.VariaveisList = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);

            this.dataGridView1.DataSource = this.m_dt_tabela_dados;

            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].Width = 180;
            }

            this.lstCoordenadasX.Items.Clear();
            this.lstCoordenadasY.Items.Clear();

            this.lstCoordenadasX.Items.AddRange(clt.RetornaColunasNumericas(this.m_dt_tabela_dados));
            this.lstCoordenadasY.Items.AddRange(clt.RetornaColunasNumericas(this.m_dt_tabela_dados));

            this.m_form_iniciado = false;

            this.lstCoordenadasX.SelectedIndex = 0;
            this.lstCoordenadasY.SelectedIndex = 0;

            this.m_form_iniciado = true;

            this.AjustaMaximasCoordenadas();

            this.cmbVarIDVizinhanca.Items.Clear();
            this.cmbVarIDVizinhanca.Items.AddRange(clt.RetornaTodasColunas(m_dt_tabela_dados));
            this.cmbVarIDVizinhanca.SelectedIndex = 0;

            this.m_tabela_importada = true;
            if (this.m_vizinhanca_definida == true) this.btnConcatenarDados.Enabled = true;

            //m_dados_concatenados = false;
            //this.rdbModelosSAR_viaMLE.Enabled = false;
            //this.rdbModelosSEM_viaMLE.Enabled = false;
            //this.rdbSARKelejianPrucha.Enabled = false;
            //this.ckbWXcovariaveis.Enabled = false;

            //this.rdbOLS.Checked = true;
            //this.grbTipoMatrizVizinhanca.Enabled = false;
            //this.grbCalculoLogDet.Enabled = false;
        }

        private void AtualizaTabelaDados()
        {
            clsUtilTools clt = new clsUtilTools();

            this.userControlRegressaoInstrumentos1.ZeraControle();

            this.userControlRegressaoInstrumentos1.VariaveisDB = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
            this.userControlRegressaoInstrumentos1.VariaveisList = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);

            this.dataGridView1.DataSource = this.m_dt_tabela_dados;
            this.dataGridView1.Refresh();

            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].Width = 150;
            }
            
            this.lstCoordenadasX.Items.Clear();
            this.lstCoordenadasY.Items.Clear();

            this.lstCoordenadasX.Items.AddRange(clt.RetornaColunasNumericas(this.m_dt_tabela_dados));
            this.lstCoordenadasY.Items.AddRange(clt.RetornaColunasNumericas(this.m_dt_tabela_dados));

            this.m_form_iniciado = false;

            this.lstCoordenadasX.SelectedIndex = 0;
            this.lstCoordenadasY.SelectedIndex = 0;

            this.m_form_iniciado = true;

            this.AjustaMaximasCoordenadas();

            this.cmbVarIDVizinhanca.Items.Clear();
            this.cmbVarIDVizinhanca.Items.AddRange(clt.RetornaTodasColunas(m_dt_tabela_dados));
            this.cmbVarIDVizinhanca.SelectedIndex = 0;

            this.m_tabela_importada = true;
            if (this.m_vizinhanca_definida == true) this.btnConcatenarDados.Enabled = true;

            if (!m_usa_W_sparsa_predefinida)
            {
                m_dados_concatenados = false;
                this.rdbModelosSAR_viaMLE.Enabled = false;
                this.rdbModelosSEM_viaMLE.Enabled = false;
                this.rdbModeloSAC_viaMLE.Enabled = false;
                
                this.rdbSARKelejianPrucha.Enabled = false;
                this.ckbWXcovariaveis.Enabled = false;

                this.rdbOLS.Checked = true;
                this.grbTipoMatrizVizinhanca.Enabled = false;
                this.grbCalculoLogDet.Enabled = false;
            }
        }

        private void FormRegressaoGMMConley_Load(object sender, EventArgs e)
        {
            if (!m_usa_W_sparsa_predefinida) lblAvisoMatrizWPredefinida.Text = "";

            if (this.tabControl1.TabPages.Contains(tabPage2))
                this.tabControl1.TabPages.Remove(tabPage2);

            if (this.tabControl1.TabPages.Contains(tabPage3))
                this.tabControl1.TabPages.Remove(tabPage3);

            this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = false;
            this.grbCoordenadasGeograficas.Enabled = false;
            this.grbSelecaoBandwidth.Enabled = false;

            if (m_dt_tabela_dados.Rows.Count <= 0 || m_dt_tabela_dados.Columns.Count <= 0)
            {
                this.btnEstimarModelo.Enabled =
                    this.btnEstimarModelo1.Enabled = false;
            }
        }

        #endregion

        #region Eventos

        private void rdbOLS_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbOLS.Checked)
            {
                this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = false;
                this.grbCoordenadasGeograficas.Enabled = false;
                this.grbSelecaoBandwidth.Enabled = false;
                this.grbCalculoLogDet.Enabled = false;
                this.grbGMM.Enabled = false;
                this.ckbWXcovariaveis.Enabled = true;
                this.grbTipoMatrizVizinhanca.Enabled = false;

                this.ckbIterationTillConvergence.Enabled = false;
            }
        }

        private void rdb2SLS_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdb2SLS.Checked)
            {
                this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = true;
                this.grbCoordenadasGeograficas.Enabled = false;
                this.grbSelecaoBandwidth.Enabled = false;
                this.grbCalculoLogDet.Enabled = false;
                this.grbGMM.Enabled = false;
                this.ckbWXcovariaveis.Enabled = true;
                this.grbTipoMatrizVizinhanca.Enabled = false;

                this.ckbIterationTillConvergence.Enabled = false;
            }
        }

        private void rdbGMM_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbGMMConley.Checked || this.rdbGMM.Checked)
            {
                this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = true;
                this.grbCoordenadasGeograficas.Enabled = false;
                this.grbSelecaoBandwidth.Enabled = false;
                this.grbCalculoLogDet.Enabled = false;
                this.grbTipoMatrizVizinhanca.Enabled = false;

                this.grbGMM.Enabled = true;
                this.rdbLinkIdentidade.Checked = true;
                this.ckbWXcovariaveis.Enabled = false;

                this.ckbIterationTillConvergence.Enabled = true;

                this.ckbModelosNaoLineares.Enabled = true; 
                this.ckbModelosNaoLineares.Checked = false;
                this.rdbLinkIdentidade.Enabled =
                    this.rdbLinkLogaritmo.Enabled =
                    this.rdbLinkLogit.Enabled =
                    this.rdbProbit.Enabled =
                    this.rdbCompLogLog.Enabled = false;
            }
            else
            {
                this.ckbModelosNaoLineares.Enabled = false;
            }
        }

        private void rdbGMMConley_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbGMMConley.Checked || this.rdbGMM.Checked)
            {
                this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = true;
                this.grbCoordenadasGeograficas.Enabled = true;
                this.grbSelecaoBandwidth.Enabled = true;
                this.grbCalculoLogDet.Enabled = false;
                this.grbTipoMatrizVizinhanca.Enabled = false;

                this.grbGMM.Enabled = true;
                this.rdbLinkIdentidade.Checked = true;
                this.ckbWXcovariaveis.Enabled = false;

                this.ckbIterationTillConvergence.Enabled = true;

                this.ckbModelosNaoLineares.Enabled = true;
                this.ckbModelosNaoLineares.Checked = false;
                this.rdbLinkIdentidade.Enabled =
                    this.rdbLinkLogaritmo.Enabled =
                    this.rdbLinkLogit.Enabled =
                    this.rdbProbit.Enabled =
                    this.rdbCompLogLog.Enabled = false;

                if (this.rdbGMMConley.Checked)
                {
                    this.tabControl1.SelectedTab = tabPage4;
                }
            }
            else
            {
                this.ckbModelosNaoLineares.Enabled = false;
            }
        }

        private void btnEstimarModelo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                this.EstimaModelo();

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                if (this.m_thread_estimacoes != null)
                {
                    this.m_thread_estimacoes.Abort();
                    this.m_thread_estimacoes = null;
                }

                if (this.m_thread_progress_bar != null)
                {
                    this.m_thread_progress_bar.Abort();
                    this.m_thread_progress_bar = null;
                }

                Cursor = Cursors.Default;
                btnEstimarModelo.Enabled = this.btnEstimarModelo1.Enabled = true;
                btnInterromper1.Enabled = btnInterromper2.Enabled = false;
                this.lblProgressBar.Text = "Estimação do modelo de regressão falhou.";
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEstimarModelo1_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                this.EstimaModelo();

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                this.lblProgressBar.Text = "Estimação do modelo de regressão falhou.";

                if (this.m_thread_estimacoes != null)
                {
                    this.m_thread_estimacoes.Abort();
                    this.m_thread_estimacoes = null;
                }

                if (this.m_thread_progress_bar != null)
                {
                    this.m_thread_progress_bar.Abort();
                    this.m_thread_progress_bar = null;
                }

                Cursor = Cursors.Default;
                btnEstimarModelo.Enabled = this.btnEstimarModelo1.Enabled = true;
                btnInterromper1.Enabled = btnInterromper2.Enabled = false;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void rdbModelosSEM_viaMLE_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbModelosSAR_viaMLE.Checked || this.rdbModelosSEM_viaMLE.Checked || this.rdbModeloSAC_viaMLE.Checked)
            {
                if (this.m_dados_concatenados)
                {
                    if (!m_usa_W_sparsa_predefinida)
                    {
                        this.grbTipoMatrizVizinhanca.Enabled = true;
                    }
                }
                else
                {
                    this.grbTipoMatrizVizinhanca.Enabled = false;
                }
                
                this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = false;
                this.grbCoordenadasGeograficas.Enabled = false;
                this.grbSelecaoBandwidth.Enabled = false;
                this.grbCalculoLogDet.Enabled = true;
                this.grbGMM.Enabled = false;

                this.ckbIterationTillConvergence.Enabled = false;

                this.ckbWXcovariaveis.Enabled = false;
            }
            //else
            //{
            //    this.grbTipoMatrizVizinhanca.Enabled = false;
            //    this.grbCalculoLogDet.Enabled = false;
            //}
        }

        private clsMatrizEsparsa m_W_sparsa_from_dists = new clsMatrizEsparsa();
        private bool m_W_sparsa_from_dists_existente = false;

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    FormMatrizVizinhancaComDistancias frm = new FormMatrizVizinhancaComDistancias();
                    //frm.MdiParent = this.MdiParent;
                    frm.Dados = this.m_dt_tabela_dados;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        m_dados_concatenados = false;
                        m_W_sparsa_from_arquivo_existente = false;

                        m_W_sparsa_from_dists_existente = true;
                        m_W_sparsa_from_dists = frm.Wesparsa;

                        if (!m_usa_W_sparsa_predefinida)
                        {
                            this.grbTipoMatrizVizinhanca.Enabled = true;
                        }

                        if (frm.MatrizWNormalizada) this.rdbMatrizVizinhancaNormalizada.Checked = true;
                        else this.rdbMatrizVizinhancaOriginal.Checked = true;

                        this.ckbWXcovariaveis.Enabled = true;
                        this.rdbModelosSAR_viaMLE.Enabled = true;
                        this.rdbModelosSEM_viaMLE.Enabled = true;
                        this.rdbModeloSAC_viaMLE.Enabled = true;
                        
                        this.rdbSARKelejianPrucha.Enabled = true;
                        this.rdbModelosSEM_viaMLE.Checked = true;
                        this.grbTipoMatrizVizinhanca.Enabled = false;
                        this.grbCalculoLogDet.Enabled = true;

                        MessageBox.Show("Matriz de vizinhança a partir das distâncias foi atualizada. A partir de agora, todos os cálculos serão feitos considerando-se essa nova matriz.",
                                        "Matriz de vizinhança", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ckbModelosNaoLineares_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbModelosNaoLineares.Checked)
            {
                this.rdbLinkIdentidade.Checked = true;

                this.rdbLinkIdentidade.Enabled =
                    this.rdbLinkLogaritmo.Enabled =
                    this.rdbLinkLogit.Enabled =
                    this.rdbProbit.Enabled =
                    this.rdbCompLogLog.Enabled = true;

                this.ckbIterationTillConvergence.Enabled = true;
                this.ckbIterationTillConvergence.Checked = true;
                this.ckbIterationTillConvergence.Enabled = false;
            }
            else
            {
                this.rdbLinkIdentidade.Enabled =
                    this.rdbLinkLogaritmo.Enabled =
                    this.rdbLinkLogit.Enabled =
                    this.rdbProbit.Enabled =
                    this.rdbCompLogLog.Enabled = false;

                this.ckbIterationTillConvergence.Enabled = true;
                this.ckbIterationTillConvergence.Checked = false;
            }
        }

        private void rdbSARKelejianPrucha_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbSARKelejianPrucha.Checked)
            {
                if (this.m_dados_concatenados)
                {
                    if (!m_usa_W_sparsa_predefinida)
                    {
                        this.grbTipoMatrizVizinhanca.Enabled = true;
                    }
                }
                else
                {
                    this.grbTipoMatrizVizinhanca.Enabled = false;
                }

                this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = true;
                this.grbCoordenadasGeograficas.Enabled = false;
                this.grbSelecaoBandwidth.Enabled = false;
                this.grbCalculoLogDet.Enabled = false;

                this.grbGMM.Enabled = false;
                this.ckbWXcovariaveis.Enabled = false;
                this.grbCovMatrixKelejianPrucha.Enabled = true;
                this.rdbKelejianPruchaResHomocedasticos.Checked = true;

                this.ckbIterationTillConvergence.Enabled = false;
            }
            else
            {
                this.grbCovMatrixKelejianPrucha.Enabled = false;
            }
        }
        
        private void rdbKelejianPruchaHAC_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (rdbKelejianPruchaHAC.Checked)
            {
                AjustaDistanciaEuclidianaMaxima();

                nudPropDistMaxima.Enabled =
                    nudBandwidthKernelHAC.Enabled =
                    lblMaxDistEuclidiana.Enabled =
                    grbCoordenadasGeograficas.Enabled =
                    ckbNumVizinhosDistancia.Enabled =
                    grbFuncaoKernelHACKelejianPrucha.Enabled = true;

                nudNumVizinhosDistancia.Enabled = true;

                int n = this.m_dt_tabela_dados.Rows.Count;
                n = n - 1;
                nudNumVizinhosDistancia.Maximum = Convert.ToDecimal(n);

                nudNumVizinhosDistancia.Enabled = false;
                ckbNumVizinhosDistancia.Checked = false;

                ckbIterationTillConvergence.Enabled = true;
                ckbIterationTillConvergence.Checked = false;
                ckbIterationTillConvergence.Enabled = false;
            }
            else
            {
                nudNumVizinhosDistancia.Enabled = false;
                ckbNumVizinhosDistancia.Checked = false;

                nudPropDistMaxima.Enabled =
                    nudBandwidthKernelHAC.Enabled =
                    lblMaxDistEuclidiana.Enabled =
                    grbCoordenadasGeograficas.Enabled =
                    ckbNumVizinhosDistancia.Enabled =
                    grbFuncaoKernelHACKelejianPrucha.Enabled = false;
            }

            Cursor = Cursors.Default;
        }

        private void AjustaDistanciaEuclidianaMaxima()
        {
            m_alteracao_prop_dist = false;
            m_alteracao_max_dist = false;

            BLMatrizDistanciasParametricas blp = new BLMatrizDistanciasParametricas();

            double dmax = blp.GeraMaxDistancia(this.m_dt_tabela_dados,
                this.lstCoordenadasX.SelectedItem.ToString(), this.lstCoordenadasY.SelectedItem.ToString());

            clsUtilTools clt = new clsUtilTools();

            lblMaxDistEuclidiana.Text = clt.Double2Texto(dmax, 8);

            this.nudBandwidthKernelHAC.Maximum = Convert.ToDecimal(dmax);
            this.nudBandwidthKernelHAC.Value = Convert.ToDecimal(dmax * Convert.ToDouble(this.nudPropDistMaxima.Value) / 100.0);

            m_max_distancia_Euclidiana = dmax;

            m_alteracao_prop_dist = true;
            m_alteracao_max_dist = true;
        }

        private bool m_alteracao_prop_dist = false;
        private bool m_alteracao_max_dist = false;
        private double m_max_distancia_Euclidiana = 0.0;

        private void nudBandwidthKernelHAC_ValueChanged(object sender, EventArgs e)
        {
            m_alteracao_prop_dist = false;
            if (m_alteracao_max_dist)
            {
                nudPropDistMaxima.Value = Convert.ToDecimal((Convert.ToDouble(this.nudBandwidthKernelHAC.Value) 
                                                                / m_max_distancia_Euclidiana) * 100.0);
            }
            m_alteracao_prop_dist = true;
        }

        private void nudPropDistMaxima_ValueChanged(object sender, EventArgs e)
        {
            m_alteracao_max_dist = false;
            if (m_alteracao_prop_dist)
            {
                this.nudBandwidthKernelHAC.Value = Convert.ToDecimal(m_max_distancia_Euclidiana * Convert.ToDouble(this.nudPropDistMaxima.Value) / 100.0);
            }
            m_alteracao_max_dist = true;
        }
        
        private void tamanhoDaFonteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormTamanhoFonte frm = new FormTamanhoFonte();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    this.richTextBoxResultadosEstimacao.SelectAll();
                    this.richTextBoxResultadosEstimacao.SelectionFont = new Font("Courier New", frm.TamanhoFonte);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormTamanhoFonte frm = new FormTamanhoFonte();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    this.richTextValoresGerados.SelectAll();
                    this.richTextValoresGerados.SelectionFont = new Font("Courier New", frm.TamanhoFonte);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void btnOpções_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedTab = tabPage4;
        }

        private void rdbDistanciaMaximaAbsoluta_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbDistanciaMaximaAbsoluta.Checked)
            {
                this.nudPropDistMaximoX.Enabled = false;
                this.nudPropDistMaximoY.Enabled = false;

                this.nudBandwidthX.Enabled = true;
                this.nudBandwidthY.Enabled = true;
            }
            else
            {
                this.nudPropDistMaximoX.Enabled = true;
                this.nudPropDistMaximoY.Enabled = true;

                this.nudBandwidthX.Enabled = false;
                this.nudBandwidthY.Enabled = false;
            }
        }

        private void AjustaMaximasCoordenadas()
        {
            BLModelosCrossSectionSpaciaisLineares gmm = new BLModelosCrossSectionSpaciaisLineares();

            double xmax = gmm.GeraMaximaDistancia(this.m_dt_tabela_dados, this.lstCoordenadasX.SelectedItem.ToString());
            double ymax = gmm.GeraMaximaDistancia(this.m_dt_tabela_dados, this.lstCoordenadasY.SelectedItem.ToString());

            clsUtilTools clt = new clsUtilTools();

            lblMaxCoordenadaX.Text = clt.Double2Texto(xmax, 8);
            lblMaximaCoordenadaY.Text = clt.Double2Texto(ymax, 8);

            nudBandwidthX.Maximum = Convert.ToDecimal(xmax);
            nudBandwidthY.Maximum = Convert.ToDecimal(ymax);

            nudBandwidthX.Value = Convert.ToDecimal(xmax * Convert.ToDouble(this.nudPropDistMaximoX.Value) / 100.0);
            nudBandwidthY.Value = Convert.ToDecimal(ymax * Convert.ToDouble(this.nudPropDistMaximoY.Value) / 100.0);
        }

        private void rdbProporcaoDistMaxima_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.rdbProporcaoDistMaxima.Checked)
            {
                this.nudPropDistMaximoX.Enabled = false;
                this.nudPropDistMaximoY.Enabled = false;

                this.nudBandwidthX.Enabled = true;
                this.nudBandwidthY.Enabled = true;
            }
            else
            {
                this.nudPropDistMaximoX.Enabled = true;
                this.nudPropDistMaximoY.Enabled = true;

                this.nudBandwidthX.Enabled = false;
                this.nudBandwidthY.Enabled = false;
            }
        }

        private void nudPropDistMaximoX_ValueChanged(object sender, EventArgs e)
        {
            if (m_form_iniciado)
            {
                this.AjustaMaximasCoordenadas();
            }
        }

        private void nudPropDistMaximoY_ValueChanged(object sender, EventArgs e)
        {
            if (m_form_iniciado)
            {
                this.AjustaMaximasCoordenadas();
            }
        }

        private void lstCoordenadasX_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (m_form_iniciado)
            {
                Cursor = Cursors.WaitCursor;

                this.AjustaMaximasCoordenadas();

                if (this.rdbKelejianPruchaHAC.Checked) AjustaDistanciaEuclidianaMaxima();

                Cursor = Cursors.Default;
            }
        }

        private void lstCoordenadasY_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (m_form_iniciado)
            {
                Cursor = Cursors.WaitCursor;

                this.AjustaMaximasCoordenadas();

                if (this.rdbKelejianPruchaHAC.Checked) AjustaDistanciaEuclidianaMaxima();

                Cursor = Cursors.Default;
            }
        }

        private void rdbModeloSAC_viaMLE_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbModelosSAR_viaMLE.Checked || this.rdbModelosSEM_viaMLE.Checked || this.rdbModeloSAC_viaMLE.Checked)
            {
                if (this.m_dados_concatenados)
                {
                    if (!m_usa_W_sparsa_predefinida)
                    {
                        this.grbTipoMatrizVizinhanca.Enabled = true;
                    }
                }
                else
                {
                    this.grbTipoMatrizVizinhanca.Enabled = false;
                }

                //this.grbTipoMatrizVizinhanca.Enabled = true;

                this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = false;
                this.grbCoordenadasGeograficas.Enabled = false;
                this.grbSelecaoBandwidth.Enabled = false;
                this.grbCalculoLogDet.Enabled = true;
                this.grbGMM.Enabled = false;

                this.ckbIterationTillConvergence.Enabled = false;
            }
            else
            {
                this.grbTipoMatrizVizinhanca.Enabled = false;
                this.grbCalculoLogDet.Enabled = false;
            }
        }

        private void rdbModelosSAR_viaMLE_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbModelosSAR_viaMLE.Checked || this.rdbModelosSEM_viaMLE.Checked || this.rdbModeloSAC_viaMLE.Checked)
            {
                if (this.m_dados_concatenados)
                {
                    if (!m_usa_W_sparsa_predefinida)
                    {
                        this.grbTipoMatrizVizinhanca.Enabled = true;
                    }
                }
                else
                {
                    this.grbTipoMatrizVizinhanca.Enabled = false;
                }

                //this.grbTipoMatrizVizinhanca.Enabled = true;

                this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = false;
                this.grbCoordenadasGeograficas.Enabled = false;
                this.grbSelecaoBandwidth.Enabled = false;
                this.grbCalculoLogDet.Enabled = true;
                this.grbGMM.Enabled = false;

                this.ckbIterationTillConvergence.Enabled = false;
            }
            else
            {
                this.grbTipoMatrizVizinhanca.Enabled = false;
                this.grbCalculoLogDet.Enabled = false;
            }
        }

        private void ckbNumVizinhosDistancia_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbNumVizinhosDistancia.Checked) nudNumVizinhosDistancia.Enabled = true;
            else nudNumVizinhosDistancia.Enabled = false;
        }

        #endregion

        #region Estimação do modelo
        
        private void IniciarProgressBar()
        {
            this.m_thread_progress_bar = new Thread(new ThreadStart(this.CorrerProgressBarThread));
            this.m_thread_progress_bar.Name = "Progress Bar";
            this.m_thread_progress_bar.Start();
        }

        private delegate void AtualizaProgressBarCall(int valor);
        private void AtualizaProgressBar(int valor)
        {
            this.progressBar1.Value = valor;
        }

        private void CorrerProgressBarThread()
        {
            int valor = 0;
            int div = 0;
            for (int i = 0; i < 100000000; i++)
            {
                div = Math.DivRem(i, 101, out valor);
                this.progressBar1.Invoke(new AtualizaProgressBarCall(this.AtualizaProgressBar),
                    new object[] { valor });
                System.Threading.Thread.Sleep(100);
            }
        }

        private void EstimaModelo()
        {
            try
            {
                //progressBar1.Visible = true;
                #region checando presença de valores inválidos nos dados

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

                if (this.rdb2SLS.Checked || this.rdbGMM.Checked || this.rdbGMMConley.Checked || this.rdbSARKelejianPrucha.Checked)
                {
                    for (int i = 0; i < userControlRegressaoInstrumentos1.VariaveisInstrumentais.GetLength(0); i++)
                    {
                        if (clt.ChecaValoresDoubleInvalidos(this.m_dt_tabela_dados, userControlRegressaoInstrumentos1.VariaveisInstrumentais[i], out indicadores_val_invalidos))
                        {
                            MessageBox.Show("Há valores double inválidos na variável " + userControlRegressaoInstrumentos1.VariaveisInstrumentais[i] + ". Cheque a sua base de dados.");
                            return;
                        }
                    }
                }

                if (this.rdbGMMConley.Checked || this.rdbKelejianPruchaHAC.Checked)
                {
                    if (clt.ChecaValoresDoubleInvalidos(this.m_dt_tabela_dados, this.lstCoordenadasX.SelectedItem.ToString(), out indicadores_val_invalidos))
                    {
                        MessageBox.Show("Há valores double inválidos na variável " + this.lstCoordenadasX.SelectedItem.ToString() + ". Cheque a sua base de dados.");
                        return;
                    }
                    if (clt.ChecaValoresDoubleInvalidos(this.m_dt_tabela_dados, this.lstCoordenadasY.SelectedItem.ToString(), out indicadores_val_invalidos))
                    {
                        MessageBox.Show("Há valores double inválidos na variável " + this.lstCoordenadasY.SelectedItem.ToString() + ". Cheque a sua base de dados.");
                        return;
                    }
                }

                #endregion

                #region checando multicolinearidade perfeita das regressores

                blr.VariaveisIndependentes = userControlRegressaoInstrumentos1.VariaveisIndependentes;
                blr.TabelaDados = this.m_dt_tabela_dados;

                string mensagem_colinear;

                if (blr.ChecarMulticolinearidade(out mensagem_colinear))
                {
                    MessageBox.Show(mensagem_colinear + " Cheque a sua base de dados ou mude a especificação do modelo.");
                    return;
                }

                #endregion

                this.lblProgressBar.Text = "Estimando o modelo de regressão ...";

                this.btnEstimarModelo.Enabled =
                    this.btnEstimarModelo1.Enabled = false;

                this.btnInterromper1.Enabled =
                    this.btnInterromper2.Enabled = true;

                #region parâmetros das estimações

                this.progressBar1.Maximum = 100;
                this.progressBar1.Minimum = 0;
                this.progressBar1.Value = 0;


                blr.CutOffCoordenadaX = Convert.ToDouble(this.nudBandwidthX.Value);
                blr.CutOffCoordenadaY = Convert.ToDouble(this.nudBandwidthY.Value);

                blr.VariavelCoordenadaX = this.lstCoordenadasX.SelectedItem.ToString();
                blr.VariavelCoordenadaY = this.lstCoordenadasY.SelectedItem.ToString();

                bln.CutOffCoordenadaX = Convert.ToDouble(this.nudBandwidthX.Value);
                bln.CutOffCoordenadaY = Convert.ToDouble(this.nudBandwidthY.Value);

                bln.VariavelCoordenadaX = this.lstCoordenadasX.SelectedItem.ToString();
                bln.VariavelCoordenadaY = this.lstCoordenadasY.SelectedItem.ToString();

                blr.BandWidthKernelHAC = Convert.ToDouble(this.nudBandwidthKernelHAC.Value);
                blr.VariavelCoordenadaX = this.lstCoordenadasX.SelectedItem.ToString();
                blr.VariavelCoordenadaY = this.lstCoordenadasY.SelectedItem.ToString();

                blr.UsaNumVizinhosParaDistancia = this.ckbNumVizinhosDistancia.Checked;
                blr.NumVizinhosParaDistancia = Convert.ToInt32(nudNumVizinhosDistancia.Value);

                if (this.rdbKelejianPruchaResHomocedasticos.Checked) blr.TipoCorrecaoCovMatrix = TipoCorrecaoMatrizCovariancia.SemCorrecao;
                if (this.rdbKelejianPruchaResHeteroscedasticos.Checked) blr.TipoCorrecaoCovMatrix = TipoCorrecaoMatrizCovariancia.Heteroscedasticidade;
                if (this.rdbKelejianPruchaHAC.Checked) blr.TipoCorrecaoCovMatrix = TipoCorrecaoMatrizCovariancia.HAC;

                if (this.rdbBarlletKernelHAC.Checked) blr.TipoKernelCorrecaoHAC = TipoKernelCorrecaoHAC.Barlett;
                if (this.rdbEpanechnikovKernelHAC.Checked) blr.TipoKernelCorrecaoHAC = TipoKernelCorrecaoHAC.Epanechnikov;
                if (this.rdbBisquareKernelHAC.Checked) blr.TipoKernelCorrecaoHAC = TipoKernelCorrecaoHAC.Biquadrado;

                #endregion

                m_thread_estimacoes = new Thread(new ThreadStart(this.EstimacaoModelosCrossSection));
                m_thread_estimacoes.Name = "Estimação modelo";
                m_thread_estimacoes.Start();
                //m_thread_progress_bar = new Thread(new ThreadStart(this.EstimacaoModelosCrossSection));
                //m_thread_progress_bar.Start();
                //progressBar1.Visible = false;
            }
            catch (Exception er)
            {
                this.m_thread_estimacoes.Abort();
                this.m_thread_progress_bar.Abort();

                this.btnEstimarModelo.Enabled =
                    this.btnEstimarModelo1.Enabled = true;

                this.btnInterromper1.Enabled =
                    this.btnInterromper2.Enabled = false;

                this.lblProgressBar.Text = "Erro na estimação do modelo de regressão.";
                this.progressBar1.Value = 0;

                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private BLModelosCrossSectionSpaciaisLineares blr = new BLModelosCrossSectionSpaciaisLineares();
        private BLModelosCrossSectionSpaciaisNaoLineares bln = new BLModelosCrossSectionSpaciaisNaoLineares();
        private clsUtilTools clt = new clsUtilTools();

        private void EstimacaoModelosCrossSection()
        {
            try
            {
                
                this.IniciarProgressBar();

                if (this.ckbLimpaJanelaOutput.Checked)
                {
                    if (this.tabControl1.TabPages.Contains(tabPage2))
                    {
                        this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                new object[] { richTextBoxResultadosEstimacao, "", true });
                    }
                    else
                    {
                        this.richTextBoxResultadosEstimacao.Text = "";
                    }
                }

                blr.VariaveisDependentes = userControlRegressaoInstrumentos1.VariavelDependente;
                blr.VariaveisIndependentes = userControlRegressaoInstrumentos1.VariaveisIndependentes;
                blr.VariaveisInstrumentais = userControlRegressaoInstrumentos1.VariaveisInstrumentais;

                if (blr.VariaveisDependentes.GetLength(0) < 1)
                {
                    this.m_thread_progress_bar.Abort();

                    this.btnEstimarModelo.Invoke(new MostraBotoesExecucaoCall(this.MostraBotoesExecucao),
                        new object[] { });

                    this.lblProgressBar.Invoke(new MostraLabelCall(this.MostraLabel),
                        new object[] { this.lblProgressBar, "Estimação do modelo de regressão finalizada." });

                    this.progressBar1.Invoke(new AtualizaProgressBarCall(this.AtualizaProgressBar),
                            new object[] { 0 });

                    MessageBox.Show("Selecione uma variável dependente.");
                    return;
                }

                if (blr.VariaveisIndependentes.GetLength(0) < 1)
                {
                    this.m_thread_progress_bar.Abort();

                    this.btnEstimarModelo.Invoke(new MostraBotoesExecucaoCall(this.MostraBotoesExecucao),
                        new object[] { });

                    this.lblProgressBar.Invoke(new MostraLabelCall(this.MostraLabel),
                        new object[] { this.lblProgressBar, "Estimação do modelo de regressão finalizada." });

                    this.progressBar1.Invoke(new AtualizaProgressBarCall(this.AtualizaProgressBar),
                            new object[] { 0 });

                    MessageBox.Show("Selecione pelo menos uma variável independente.");
                    return;
                }

                blr.IncluiIntercepto = this.ckbIncluiIntercepto.Checked;
                blr.ApresentaCovMatrixBetaHat = this.ckbApresentaCovMatrixBetaHat.Checked;
                blr.TabelaDados = this.m_dt_tabela_dados;
                blr.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo;
                blr.AdicionaNovaVariaveis = this.ckbIncluirNovasVariaveisTabelaDados.Checked;

                #region opções para cálculo do log-determinante de matrizes esparsas

                if (rdbLogDetLUEsparsa.Checked)
                {
                    blr.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.MatrizEsparsaDecomposicaoLU;
                }

                if (this.rdbLogDetMatrizDensa.Checked)
                {
                    blr.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.MatrizDensa;

                    int n = m_dt_tabela_dados.Rows.Count;

                    if (this.rdbModelosSAR_viaMLE.Checked || this.rdbModelosSEM_viaMLE.Checked || this.rdbModeloSAC_viaMLE.Checked)
                    {
                        if (n <= 2000)
                        {
                            blr.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.MatrizDensa;
                        }
                        else
                        {
                            blr.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.MatrizEsparsaDecomposicaoLU;
                            MessageBox.Show("Tabela de dados contém mais de 2000 observações. Será utilizado cálculo do determinante via decomposição LU para matrizes esparsas.",
                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.rdbLogDetLUEsparsa.Checked = true;

                            //blr.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo;
                            //MessageBox.Show("Tabela de dados contém mais de 2000 observações. Será utilizado cálculo do determinante via simulações de Monte Carlo.",
                            //    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //this.rdbLogDetMonteCarlo.Checked = true;
                        }
                    }
                }

                if (this.rdbLogDetMonteCarlo.Checked)
                {
                    blr.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo;
                }

                #endregion

                #region estimação via OLS e 2SLS

                if (this.rdbOLS.Checked)
                {
                    //blr.Shape = this.m_shape;
                    if (this.rdbMatrizVizinhancaNormalizada.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Normalizada;
                    if (this.rdbMatrizVizinhancaOriginal.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Original;

                    if (this.ckbWXcovariaveis.Checked)
                    {
                        blr.Shape = this.m_shape;
                        blr.GeraMatrizVizinhanca();
                    }

                    if (this.ckbMulticolinearidade.Checked)
                    {
                        if (!this.tabControl1.TabPages.Contains(tabPage2))
                        {
                            this.richTextBoxResultadosEstimacao.Text += blr.AnaliseMulticolinearidade();
                        }
                        else
                        {
                            this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                    new object[] { richTextBoxResultadosEstimacao, blr.AnaliseMulticolinearidade(), false });
                        }
                    }

                    blr.EstimaOLSRegression();
                }

                if (this.rdb2SLS.Checked)
                {
                    //blr.Shape = this.m_shape;
                    if (this.rdbMatrizVizinhancaNormalizada.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Normalizada;
                    if (this.rdbMatrizVizinhancaOriginal.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Original;

                    if (this.ckbWXcovariaveis.Checked)
                    {
                        blr.Shape = this.m_shape;
                        blr.GeraMatrizVizinhanca();
                    }

                    if (this.ckbMulticolinearidade.Checked)
                    {
                        if (!this.tabControl1.TabPages.Contains(tabPage2))
                        {
                            this.richTextBoxResultadosEstimacao.Text += blr.AnaliseMulticolinearidade();
                        }
                        else
                        {
                            this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                    new object[] { richTextBoxResultadosEstimacao, blr.AnaliseMulticolinearidade(), false });
                        }
                    }

                    blr.IterateTillConvergence = this.ckbIterationTillConvergence.Checked;
                    blr.Estima2SLSRegression();
                }

                #endregion

                #region GMM não linear

                if ((this.rdbGMM.Checked || this.rdbGMMConley.Checked) && this.ckbModelosNaoLineares.Checked)
                {
                    if (rdbLinkIdentidade.Checked)
                    {
                        bln.TipoFuncaoLigacao = TipoFuncaoLigacao.Identidade;
                    }
                    if (rdbLinkLogaritmo.Checked)
                    {
                        bln.TipoFuncaoLigacao = TipoFuncaoLigacao.Logaritmo;
                    }
                    if (rdbLinkLogit.Checked)
                    {
                        bln.TipoFuncaoLigacao = TipoFuncaoLigacao.Logit;
                    }
                    if (rdbCompLogLog.Checked)
                    {
                        bln.TipoFuncaoLigacao = TipoFuncaoLigacao.Cloglog;
                    }
                    if (rdbProbit.Checked)
                    {
                        bln.TipoFuncaoLigacao = TipoFuncaoLigacao.Probit;
                    }

                    bln.VariaveisDependentes = userControlRegressaoInstrumentos1.VariavelDependente;
                    bln.VariaveisIndependentes = userControlRegressaoInstrumentos1.VariaveisIndependentes;
                    bln.VariaveisInstrumentais = userControlRegressaoInstrumentos1.VariaveisInstrumentais;

                    if (bln.VariaveisDependentes.GetLength(0) < 1)
                    {
                        MessageBox.Show("Selecione uma variável dependente.");
                        return;
                    }
                    if (bln.VariaveisIndependentes.GetLength(0) < 1)
                    {
                        MessageBox.Show("Selecione pelo menos uma variável independente.");
                        return;
                    }

                    bln.IncluiIntercepto = this.ckbIncluiIntercepto.Checked;
                    bln.ApresentaCovMatrixBetaHat = this.ckbApresentaCovMatrixBetaHat.Checked;
                    bln.TabelaDados = this.m_dt_tabela_dados;
                    bln.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo;
                    bln.AdicionaNovaVariaveis = this.ckbIncluirNovasVariaveisTabelaDados.Checked;

                    if (this.rdbGMM.Checked)
                    {
                        blr.IterateTillConvergence = this.ckbIterationTillConvergence.Checked;
                        bln.EstimaGMMNaoLinear();

                        if (this.ckbMulticolinearidade.Checked)
                        {
                            if (!this.tabControl1.TabPages.Contains(tabPage2))
                            {
                                this.richTextBoxResultadosEstimacao.Text += blr.AnaliseMulticolinearidade();
                            }
                            else
                            {
                                this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                        new object[] { richTextBoxResultadosEstimacao, blr.AnaliseMulticolinearidade(), false });
                            }
                        }
                    }
                    else
                    {
                        //bln.CutOffCoordenadaX = Convert.ToDouble(this.nudBandwidthX.Value);
                        //bln.CutOffCoordenadaY = Convert.ToDouble(this.nudBandwidthY.Value);

                        //bln.VariavelCoordenadaX = this.lstCoordenadasX.SelectedItem.ToString();
                        //bln.VariavelCoordenadaY = this.lstCoordenadasY.SelectedItem.ToString();

                        blr.IterateTillConvergence = this.ckbIterationTillConvergence.Checked;
                        bln.EstimaGMMNaoLinearEspacial();

                        if (this.ckbMulticolinearidade.Checked)
                        {
                            if (!this.tabControl1.TabPages.Contains(tabPage2))
                            {
                                this.richTextBoxResultadosEstimacao.Text += blr.AnaliseMulticolinearidade();
                            }
                            else
                            {
                                this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                        new object[] { richTextBoxResultadosEstimacao, blr.AnaliseMulticolinearidade(), false });
                            }
                        }
                    }
                }

                #endregion

                #region GMM linear

                if (this.rdbGMM.Checked && !this.ckbModelosNaoLineares.Checked)
                {
                    //blr.Shape = this.m_shape;
                    if (this.rdbMatrizVizinhancaNormalizada.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Normalizada;
                    if (this.rdbMatrizVizinhancaOriginal.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Original;

                    if (this.ckbWXcovariaveis.Checked)
                    {
                        blr.Shape = this.m_shape;
                        blr.GeraMatrizVizinhanca();
                    }

                    if (this.ckbMulticolinearidade.Checked)
                    {
                        if (!this.tabControl1.TabPages.Contains(tabPage2))
                        {
                            this.richTextBoxResultadosEstimacao.Text += blr.AnaliseMulticolinearidade();
                        }
                        else
                        {
                            this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                    new object[] { richTextBoxResultadosEstimacao, blr.AnaliseMulticolinearidade(), false });
                        }
                    }

                    blr.IterateTillConvergence = this.ckbIterationTillConvergence.Checked;
                    blr.EstimaNaoEspacialGMM();
                }

                if (this.rdbGMMConley.Checked && !this.ckbModelosNaoLineares.Checked)
                {
                    //blr.CutOffCoordenadaX = Convert.ToDouble(this.nudBandwidthX.Value);
                    //blr.CutOffCoordenadaY = Convert.ToDouble(this.nudBandwidthY.Value);

                    //blr.VariavelCoordenadaX = this.lstCoordenadasX.SelectedItem.ToString();
                    //blr.VariavelCoordenadaY = this.lstCoordenadasY.SelectedItem.ToString();

                    if (this.ckbWXcovariaveis.Checked)
                    {
                        blr.Shape = this.m_shape;
                        blr.GeraMatrizVizinhanca();
                    }

                    if (this.ckbMulticolinearidade.Checked)
                    {
                        if (!this.tabControl1.TabPages.Contains(tabPage2))
                        {
                            this.richTextBoxResultadosEstimacao.Text += blr.AnaliseMulticolinearidade();
                        }
                        else
                        {
                            this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                    new object[] { richTextBoxResultadosEstimacao, blr.AnaliseMulticolinearidade(), false });
                        }
                    }

                    blr.IterateTillConvergence = this.ckbIterationTillConvergence.Checked;
                    blr.EstimaEspacialGMM();
                }

                #endregion

                #region modelos SAR (via MLE e Kelejian_Prucha), SEM (via MLE, OLS e FGLS) e SAC (via MLE)

                #region SAC via MLE

                if (this.rdbModeloSAC_viaMLE.Checked)
                {
                    if (this.rdbMatrizVizinhancaNormalizada.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Normalizada;
                    if (this.rdbMatrizVizinhancaOriginal.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Original;

                    if (!this.m_W_sparsa_from_dists_existente && !this.m_W_sparsa_from_arquivo_existente && !this.m_usa_W_sparsa_predefinida)
                    {
                        blr.Shape = this.m_shape;
                        blr.GeraMatrizVizinhanca();
                        blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.ArquivoShape;
                    }
                    else
                    {
                        if (this.m_W_sparsa_from_dists_existente)
                        {
                            blr.MatrizEsparsaFromDistancias = m_W_sparsa_from_dists;
                            blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizFromDistancias;
                        }
                        if (this.m_W_sparsa_from_arquivo_existente)
                        {
                            blr.MatrizEsparsaFromDistancias = m_W_sparsa_from_arquivo;
                            blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizFromArquivo;
                        }
                        if (this.m_usa_W_sparsa_predefinida)
                        {
                            blr.MatrizEsparsaFromDistancias = m_W_sparsa_predefinida;
                            blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizPreDefinida;
                            blr.TipoMatrizVizinhancaPredefinida = m_tipo_matriz_W_predefinida;
                        }
                    }

                    if (this.ckbMulticolinearidade.Checked)
                    {
                        if (!this.tabControl1.TabPages.Contains(tabPage2))
                        {
                            this.richTextBoxResultadosEstimacao.Text += blr.AnaliseMulticolinearidade();
                        }
                        else
                        {
                            this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                    new object[] { richTextBoxResultadosEstimacao, blr.AnaliseMulticolinearidade(), false });
                        }
                    }

                    blr.EstimaModelosSAC();

                    blr.EstimacaoBemSucedida = true;
                }

                #endregion

                #region SAR via MLE

                if (this.rdbModelosSAR_viaMLE.Checked)
                {
                    if (this.rdbMatrizVizinhancaNormalizada.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Normalizada;
                    if (this.rdbMatrizVizinhancaOriginal.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Original;

                    if (!this.m_W_sparsa_from_dists_existente && !this.m_W_sparsa_from_arquivo_existente && !this.m_usa_W_sparsa_predefinida)
                    {
                        blr.Shape = this.m_shape;
                        blr.GeraMatrizVizinhanca();
                        blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.ArquivoShape;
                    }
                    else
                    {
                        if (this.m_W_sparsa_from_dists_existente)
                        {
                            blr.MatrizEsparsaFromDistancias = m_W_sparsa_from_dists;
                            blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizFromDistancias;
                        }
                        if (this.m_W_sparsa_from_arquivo_existente)
                        {
                            blr.MatrizEsparsaFromDistancias = m_W_sparsa_from_arquivo;
                            blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizFromArquivo;
                        }
                        if (this.m_usa_W_sparsa_predefinida)
                        {
                            blr.MatrizEsparsaFromDistancias = m_W_sparsa_predefinida;
                            blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizPreDefinida;
                            blr.TipoMatrizVizinhancaPredefinida = m_tipo_matriz_W_predefinida;
                        }
                    }

                    if (this.ckbMulticolinearidade.Checked)
                    {
                        if (!this.tabControl1.TabPages.Contains(tabPage2))
                        {
                            this.richTextBoxResultadosEstimacao.Text += blr.AnaliseMulticolinearidade();
                        }
                        else
                        {
                            this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                    new object[] { richTextBoxResultadosEstimacao, blr.AnaliseMulticolinearidade(), false });
                        }
                    }

                    blr.EstimaModelosSAR();

                    blr.EstimacaoBemSucedida = true;
                }

                #endregion
                                
                #region SEM via MLE

                if (this.rdbModelosSEM_viaMLE.Checked)
                {
                    if (this.rdbMatrizVizinhancaNormalizada.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Normalizada;
                    if (this.rdbMatrizVizinhancaOriginal.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Original;

                    if (!m_W_sparsa_from_dists_existente && !this.m_W_sparsa_from_arquivo_existente && !this.m_usa_W_sparsa_predefinida)
                    {
                        blr.Shape = this.m_shape;
                        blr.GeraMatrizVizinhanca();
                        blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.ArquivoShape;
                    }
                    else
                    {
                        if (m_W_sparsa_from_dists_existente)
                        {
                            blr.MatrizEsparsaFromDistancias = m_W_sparsa_from_dists;
                            blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizFromDistancias;
                        }
                        if (m_W_sparsa_from_arquivo_existente)
                        {
                            blr.MatrizEsparsaFromDistancias = m_W_sparsa_from_arquivo;
                            blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizFromArquivo;
                        }
                        if (m_usa_W_sparsa_predefinida)
                        {
                            blr.MatrizEsparsaFromDistancias = m_W_sparsa_predefinida;
                            blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizPreDefinida;
                            blr.TipoMatrizVizinhancaPredefinida = m_tipo_matriz_W_predefinida;
                        }
                    }

                    if (this.ckbMulticolinearidade.Checked)
                    {
                        if (!this.tabControl1.TabPages.Contains(tabPage2))
                        {
                            this.richTextBoxResultadosEstimacao.Text += blr.AnaliseMulticolinearidade();
                        }
                        else
                        {
                            this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                    new object[] { richTextBoxResultadosEstimacao, blr.AnaliseMulticolinearidade(), false });
                        }
                    }

                    blr.EstimaModelosSEM();

                    blr.EstimacaoBemSucedida = true;
                }

                #endregion

                #region SAR via Kelejian e Prucha

                if (this.rdbSARKelejianPrucha.Checked)
                {
                    if (this.rdbMatrizVizinhancaNormalizada.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Normalizada;
                    if (this.rdbMatrizVizinhancaOriginal.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Original;

                    if (!m_W_sparsa_from_dists_existente && !this.m_W_sparsa_from_arquivo_existente && !this.m_usa_W_sparsa_predefinida)
                    {
                        blr.Shape = this.m_shape;
                        blr.GeraMatrizVizinhanca();
                        blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.ArquivoShape;
                    }
                    else
                    {
                        if (m_W_sparsa_from_dists_existente)
                        {
                            blr.MatrizEsparsaFromDistancias = m_W_sparsa_from_dists;
                            blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizFromDistancias;
                        }
                        if (m_W_sparsa_from_arquivo_existente)
                        {
                            blr.MatrizEsparsaFromDistancias = m_W_sparsa_from_arquivo;
                            blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizFromArquivo;
                        }
                        if (m_usa_W_sparsa_predefinida)
                        {
                            blr.MatrizEsparsaFromDistancias = m_W_sparsa_predefinida;
                            blr.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizPreDefinida;
                            blr.TipoMatrizVizinhancaPredefinida = m_tipo_matriz_W_predefinida;
                        }
                    }

                    if (this.ckbMulticolinearidade.Checked)
                    {
                        if (!this.tabControl1.TabPages.Contains(tabPage2))
                        {
                            this.richTextBoxResultadosEstimacao.Text += blr.AnaliseMulticolinearidade();
                        }
                        else
                        {
                            this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                    new object[] { richTextBoxResultadosEstimacao, blr.AnaliseMulticolinearidade(), false });
                        }
                    }

                    blr.EstimaModelosSAR_Kelejian_Prucha();
                }

                #endregion

                #endregion

                #region saída dos resultados

                if (!this.tabControl1.TabPages.Contains(tabPage2))
                {
                    if ((this.rdbGMM.Checked || this.rdbGMMConley.Checked) && this.ckbModelosNaoLineares.Checked)
                    {
                        this.richTextBoxResultadosEstimacao.Text += bln.ResultadoEstimacao + "\n";
                        if (this.ckbLimpaJanelaNovasVariaveis.Checked)
                        {
                            this.richTextValoresGerados.Text = bln.VariaveisGeradas + "\n";
                        }
                        else
                        {
                            this.richTextValoresGerados.Text += bln.VariaveisGeradas + "\n";
                        }
                    }
                    else
                    {
                        this.richTextBoxResultadosEstimacao.Text += blr.ResultadoEstimacao + "\n";
                        if (this.ckbLimpaJanelaNovasVariaveis.Checked)
                        {
                            this.richTextValoresGerados.Text = blr.VariaveisGeradas + "\n";
                        }
                        else
                        {
                            this.richTextValoresGerados.Text += blr.VariaveisGeradas + "\n";
                        }
                    }
                }
                else
                {
                    if ((this.rdbGMM.Checked || this.rdbGMMConley.Checked) && this.ckbModelosNaoLineares.Checked)
                    {
                        this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                            new object[] { this.richTextBoxResultadosEstimacao, bln.ResultadoEstimacao + "\n", false });

                        //this.richTextBoxResultadosEstimacao.Text += bln.ResultadoEstimacao + "\n";

                        if (this.ckbLimpaJanelaNovasVariaveis.Checked)
                        {
                            //this.richTextValoresGerados.Text = bln.VariaveisGeradas + "\n";

                            this.richTextValoresGerados.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                new object[] { this.richTextValoresGerados, bln.VariaveisGeradas + "\n", true });
                        }
                        else
                        {
                            //this.richTextValoresGerados.Text += bln.VariaveisGeradas + "\n";

                            this.richTextValoresGerados.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                new object[] { this.richTextValoresGerados, bln.VariaveisGeradas + "\n", false });
                        }
                    }
                    else
                    {
                        this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                            new object[] { this.richTextBoxResultadosEstimacao, blr.ResultadoEstimacao + "\n", false });

                        //this.richTextBoxResultadosEstimacao.Text += blr.ResultadoEstimacao + "\n";

                        if (this.ckbLimpaJanelaNovasVariaveis.Checked)
                        {
                            //this.richTextValoresGerados.Text = blr.VariaveisGeradas + "\n";

                            this.richTextValoresGerados.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                new object[] { this.richTextValoresGerados, blr.VariaveisGeradas + "\n", true });
                        }
                        else
                        {
                            //this.richTextValoresGerados.Text += blr.VariaveisGeradas + "\n";

                            this.richTextValoresGerados.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                new object[] { this.richTextValoresGerados, blr.VariaveisGeradas + "\n", false });
                        }
                    }
                }

                this.tabControl1.Invoke(new MostraTabPageCall(this.MostraTabPage),
                    new object[] { this.tabPage2, true });

                this.tabControl1.Invoke(new MostraTabPageCall(this.MostraTabPage),
                    new object[] { this.tabPage3, false });

                if (this.ckbIncluirNovasVariaveisTabelaDados.Checked)
                {
                    if (!(this.rdbGMM.Checked || this.rdbGMMConley.Checked))
                    {
                        this.dataGridView1.Invoke(new AtualizarDataGridViewCall(this.AtualizarDataGridView),
                            new object[] { this.dataGridView1, blr.TabelaDados });
                    }
                    else
                    {
                        if (this.ckbModelosNaoLineares.Checked)
                        {
                            this.dataGridView1.Invoke(new AtualizarDataGridViewCall(this.AtualizarDataGridView),
                                new object[] { this.dataGridView1, bln.TabelaDados });
                        }
                        else
                        {
                            this.dataGridView1.Invoke(new AtualizarDataGridViewCall(this.AtualizarDataGridView),
                                new object[] { this.dataGridView1, blr.TabelaDados });
                        }
                    }
                }
                else
                {
                    this.dataGridView1.Invoke(new LimparVariaveisGeradasCall(this.LimparVariaveisGeradas),
                                new object[] { this.dataGridView1 });
                }

                #endregion

                this.btnEstimarModelo.Invoke(new MostraBotoesExecucaoCall(this.MostraBotoesExecucao),
                    new object[] { });

                this.lblProgressBar.Invoke(new MostraLabelCall(this.MostraLabel),
                    new object[] { this.lblProgressBar, "Estimação do modelo de regressão finalizada." });

                this.m_thread_progress_bar.Abort();

                this.progressBar1.Invoke(new AtualizaProgressBarCall(this.AtualizaProgressBar),
                        new object[] { 0 });

                System.GC.Collect();
                progressBar1.Visible = false;
            }
            catch (Exception er)
            {
                this.btnEstimarModelo.Invoke(new InterrompeThreadPorFalhaCall(this.InterrompeThreadPorFalha),
                    new object[] {er.Message});
            }
        }

        #endregion

        #region Variáveis da matriz esparsa externa (pré-definida no frmMapas)
        
        private string m_tipo_matriz_W_predefinida = "";
        private int m_ordem_matriz_W_predefinida = 1;
        private bool m_matriz_W_normalizada = true;
        private bool m_usa_W_sparsa_predefinida = false;
        private clsMatrizEsparsa m_W_sparsa_predefinida = new clsMatrizEsparsa();

        public clsMatrizEsparsa MatrizWEsparsaPredefinida
        {
            get { return m_W_sparsa_predefinida; }
            set 
            { 
                m_W_sparsa_predefinida = value;
            }
        }

        public bool MatrizWNormalizadaPredefinida
        {
            set { m_matriz_W_normalizada = value; }
        }

        public int OrdemMatrizVizinhancaPredefinida 
        {
            set {m_ordem_matriz_W_predefinida = value;}
        }

        public string TipoMatrizVizinhancaPredefinida
        {
            set {m_tipo_matriz_W_predefinida = value;}
        }
        
        public bool UsaWSparsaPredefinida
        {
            set 
            { 
                m_usa_W_sparsa_predefinida = value;
                UsaApenasMatrizWPredefinida();
            }
        }

        private void UsaApenasMatrizWPredefinida()
        {
            lblAvisoMatrizWPredefinida.Text = "Obs. Matriz de vizinhança pré-definida no formulário de mapas";
            btnEstruturaVizinhanca.Enabled = false;
            grbTipoVizinhanca.Enabled = false;
            rdbMatrizVizinhancaNormalizada.Checked = m_matriz_W_normalizada;
            rdbMatrizVizinhancaOriginal.Checked = !m_matriz_W_normalizada;
            grbTipoMatrizVizinhanca.Enabled = false;

            m_dados_concatenados = true;

            this.ckbWXcovariaveis.Enabled = true;
            this.rdbModelosSAR_viaMLE.Enabled = true;
            this.rdbModelosSEM_viaMLE.Enabled = true;
            this.rdbModeloSAC_viaMLE.Enabled = true;
            
            this.rdbSARKelejianPrucha.Enabled = true;
            this.rdbModelosSEM_viaMLE.Checked = true;

            this.grbCalculoLogDet.Enabled = true;

            this.tabControl1.SelectedTab = tabPage1;

            this.m_W_sparsa_from_dists_existente = false;
            this.m_W_sparsa_from_arquivo_existente = false;
        }

        #endregion

        #region Open shape file

        private clsMapa m_mapa;
        private SharpMap.Map m_sharp_mapa;
        private DataTable m_tabela_dados;
        private DataTable m_tabela_shape;

        private bool m_vizinhanca_definida = false;
        private bool m_dados_concatenados = false;
        private bool m_tabela_importada = false;

        private clsIpeaShape m_shape;
        private string m_tipo_vizinhanca = "";

        public clsIpeaShape Shape
        {
            get { return this.m_shape; }
            set 
            { 
                this.m_shape = value;

                this.ckbSalvaVizinhanca.Enabled = true;

                if (!m_usa_W_sparsa_predefinida)
                {
                    this.grbTipoVizinhanca.Enabled =
                    this.btnEstruturaVizinhanca.Enabled = true;

                    this.btnConcatenarDados.Enabled = false;

                    m_vizinhanca_definida = false;
                    m_dados_concatenados = false;
                    this.rdbModelosSAR_viaMLE.Enabled = false;
                    this.rdbModelosSEM_viaMLE.Enabled = false;
                    this.rdbModeloSAC_viaMLE.Enabled = false;
                    
                    this.rdbSARKelejianPrucha.Enabled = false;
                    this.ckbWXcovariaveis.Enabled = false;
                }

                this.rdbOLS.Checked = true;
                this.grbTipoMatrizVizinhanca.Enabled = false;
                this.grbCalculoLogDet.Enabled = false;
            }
        }

        public DataTable TabelaShape
        {
            get
            {
                return m_tabela_shape;
            }
            set
            {
            	m_tabela_shape = value;
            }
        }

        private void OpenShapeFile()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "ShapeFile (*.shp)|*.shp|All Files (*.*)|*.*";
                string FileName = "";
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    FileName = openFileDialog.FileName;
                    Cursor.Current = Cursors.WaitCursor;

                    m_mapa = new clsMapa();

                    m_mapa.LoadingMapa("mapa teste", FileName, this.ckbCriarEstruturaPoligonos.Checked);

                    this.m_tabela_shape = m_mapa.TabelaDados;

                    this.m_sharp_mapa = m_mapa.Sharp_Mapa;

                    Cursor.Current = Cursors.Default;

                    if (this.m_mapa.VetorShapes.GetLength(0) > 0 && (this.m_mapa.VetorShapes)[0] != null)
                    {
                        m_shape = (this.m_mapa.VetorShapes)[0];

                        this.ckbSalvaVizinhanca.Enabled = true;

                        if (!m_usa_W_sparsa_predefinida)
                        {
                            this.grbTipoVizinhanca.Enabled =
                            this.btnEstruturaVizinhanca.Enabled = true;
                        }
                        
                        this.btnConcatenarDados.Enabled = false;
                    }

                    m_vizinhanca_definida = false;
                    m_dados_concatenados = false;
                    this.rdbModelosSAR_viaMLE.Enabled = false;
                    this.rdbModelosSEM_viaMLE.Enabled = false;
                    this.rdbModeloSAC_viaMLE.Enabled = false;
                    
                    this.rdbSARKelejianPrucha.Enabled = false;
                    this.ckbWXcovariaveis.Enabled = false;

                    this.rdbOLS.Checked = true;
                    this.grbTipoMatrizVizinhanca.Enabled = false;
                    this.grbCalculoLogDet.Enabled = false;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAbrirArquivoShape_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Visible = true;
                this.lblProgressBar.Text = "Importando arquivo em formato shape";

                this.OpenShapeFile();

                this.lblProgressBar.Text = "Arquivo shape importado com sucesso";
                progressBar1.Visible = false;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEstruturaVizinhanca_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if ((this.m_mapa != null && this.m_mapa.VetorShapes.GetLength(0) > 0 && (this.m_mapa.VetorShapes)[0] != null) 
                    || (m_shape != null && m_shape.Count > 0))
                {
                    clsIpeaShape cs = new clsIpeaShape();
                    //m_shape = (this.m_mapa.VetorShapes)[0];

                    int tipo_vizinhanca = 2;
                    string nome_tipo_vizinhanca = "Rook";
                    this.m_tipo_vizinhanca = "Rook";

                    if (this.rdbQueen.Checked)
                    {
                        tipo_vizinhanca = 1;
                        nome_tipo_vizinhanca = "Queen";
                        this.m_tipo_vizinhanca = "Queen";
                    }

                    this.lblProgressBar.Text = "Evolução da criação da vizinhança dos polígonos";

                    if (this.ckbSalvaVizinhanca.Checked)
                    {
                        clsUtilObjects uto = new clsUtilObjects();

                        object[,] v_aux = uto.DataTableToMatrizObject(this.m_tabela_dados,
                            Convert.ToString(this.cmbVarIDVizinhanca.SelectedItem));

                        string[] var_id = new string[v_aux.GetLength(0)];
                        for (int i = 0; i < v_aux.GetLength(0); i++) var_id[i] = Convert.ToString(v_aux[i,0]);                        

                        //cs.DefinicaoVizinhos(ref m_shape, tipo_vizinhanca, var_id, ref progressBar1);
                        cs.DefinicaoVizinhos(ref m_shape, tipo_vizinhanca, var_id, ref progressBar1);
                        cs.AdicionaVizinhoProximo(ref m_shape);

                        string saida = m_shape.VizinhosToString(nome_tipo_vizinhanca);

                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "TextFile (*.dat)|*.dat|All Files (*.*)|*.*";
                        sfd.FilterIndex = 1;
                        sfd.RestoreDirectory = true;
                        if (sfd.ShowDialog() != DialogResult.Cancel)
                        {
                            this.Cursor = Cursors.WaitCursor;

                            clsUtilTools be = new clsUtilTools();
                            if (!be.ExportToTXT(saida, sfd.FileName))
                            {
                                throw new Exception("Processo de exportação para arquivo txt Falhou.");
                            }
                            this.Cursor = Cursors.Default;
                        }
                    }
                    else
                    {
                        //cs.DefinicaoVizinhos(ref m_shape, tipo_vizinhanca, ref progressBar1);
                        cs.DefinicaoVizinhos(ref m_shape, tipo_vizinhanca, ref progressBar1);
                        cs.AdicionaVizinhoProximo(ref m_shape);
                    }

                    this.lblProgressBar.Text = "Estrutura de vizinhança criada com sucesso";

                    this.m_vizinhanca_definida = true;
                    if (m_tabela_importada == true) this.btnConcatenarDados.Enabled = true;

                    if (m_shape_carregado_do_mapa)
                    {
                        this.btnConcatenarDados.Enabled = false;

                        ConcatenarDados("Estrutura de vizinhança criada com sucesso");
                    }
                }
                else
                {
                    MessageBox.Show("A estrutura de vértices de polígonos ainda não está definida", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnConcatenarDados_Click(object sender, EventArgs e)
        {
            FormConcatenacaoTabelaToShape frm = new FormConcatenacaoTabelaToShape();
            //frm.MdiParent = this.MdiParent;
            frm.TabelaDados = this.m_dt_tabela_dados;
            frm.TabelaShape = this.m_tabela_shape;
            frm.Shape = this.m_shape;
            
            if (frm.ShowDialog() == DialogResult.OK)
            {
                TabelaDeDados = frm.TabelaDadosConcatenados;

                ConcatenarDados();
            }

            //ConcatenarDados();
        }
        
        private void ConcatenarDados()
        {
            ConcatenarDados("");
        }

        private void ConcatenarDados(string mensagem)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int obs_shape = m_shape.Count;
                int obs_dados = this.m_dt_tabela_dados.Rows.Count;

                if (obs_dados > obs_shape) MessageBox.Show("O número de observações na tabela de dados é maior do que o número de observações no arquivo shape.",
                    "Falha na concatenação", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (obs_dados < obs_shape) MessageBox.Show("O número de observações na tabela de dados é menor do que o número de observações no arquivo shape.",
                    "Falha na concatenação", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (obs_dados == obs_shape)
                {
                    //MessageBox.Show("Tabela de dados e arquivo shape concatenados. A concatenação assume que os elementos na tabela de dados está na mesma ordem que os elementos no arquivo shape.",
                    //"Concatenação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    m_dados_concatenados = true;

                    this.ckbWXcovariaveis.Enabled = true;
                    this.rdbModelosSAR_viaMLE.Enabled = true;
                    this.rdbModelosSEM_viaMLE.Enabled = true;
                    this.rdbModeloSAC_viaMLE.Enabled = true;

                    this.rdbSARKelejianPrucha.Enabled = true;
                    this.rdbModelosSEM_viaMLE.Checked = true;

                    if (!m_usa_W_sparsa_predefinida)
                    {
                        this.grbTipoMatrizVizinhanca.Enabled = true;
                    }
                    this.grbCalculoLogDet.Enabled = true;

                    this.tabControl1.SelectedTab = tabPage1;

                    this.m_W_sparsa_from_dists_existente = false;
                    this.m_W_sparsa_from_arquivo_existente = false;
                }

                if (mensagem == "")
                {
                    lblProgressBar.Text = "Tabelas concatenadas com sucesso";
                }
                else
                {
                    lblProgressBar.Text = mensagem;
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Open tabela de dados
        
        private void ImportarArquivoDeDados()
        {
            try
            {
                clsUtilArquivos cla = new clsUtilArquivos();
                if (cla.ImportarTabelaDados(ref m_dt_tabela_dados))
                {
                    this.dataGridView1.DataSource = m_dt_tabela_dados;

                    m_dt_dados_originais = m_dt_tabela_dados.Copy();

                    AtualizaTabelaDados();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnImportarTabela_Click(object sender, EventArgs e)
        {
            try
            {
                ImportarArquivoDeDados();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Testes

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double[,] A = new double[4, 4];
                A[0, 0] = 4.5;
                A[0, 2] = 3.2;
                A[1, 0] = 3.1;
                A[1, 1] = 2.9;
                A[1, 3] = 0.9;
                A[2, 1] = 1.7;
                A[2, 2] = 3.0;
                A[3, 0] = 3.5;
                A[3, 1] = 0.4;
                A[3, 3] = 1.0;

                clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();

                clsMatrizEsparsa T = new clsMatrizEsparsa(A, true);

                clsMatrizEsparsa C = new clsMatrizEsparsa(A, false);

                clsMatrizEsparsa C1 = fme.TripletForm2CompressColumn(T);

                clsMatrizEsparsa T1 = fme.CompressColumn2TripletForm(C1);

                string s = fme.ImprimeMatrizEsparsaTripletForm(C1, true);

                double[,] m1 = T1.AsDoubleMatrix();
                double[,] m2 = C1.AsDoubleMatrix();

                clsUtilTools clt = new clsUtilTools();

                clsMatrizEsparsa v1 = fme.MatrizMult(C,C);
                clsMatrizEsparsa v2 = fme.MatrizMult(C, fme.MatrizTransp(C));
                double[,] v2d = v2.AsDoubleMatrix();
                double[,] vv2 = clt.MatrizMult(A, clt.MatrizTransp(A));
                double[,] diff2 = clt.MatrizSubtracao(v2d, vv2);
                double norm_diff2 = clt.Norm(diff2);

                double[,] vv = v1.AsDoubleMatrix();
                double[,] vv1 = clt.MatrizMult(A,A);
                double[,] diff = clt.MatrizSubtracao(vv, vv1);
                double norma = clt.Norm(diff);

                C = new clsMatrizEsparsa(A, false);

                clsMatrizEsparsa Ctransp = fme.MatrizTransp(C);

                double[,] t1 = Ctransp.AsDoubleMatrix();
                diff = clt.MatrizSubtracao(t1, clt.MatrizTransp(A));
                
                double[,] B = new double[4, 4];
                B[0, 0] = 1.0;
                B[1, 0] = 2.5;
                B[1, 1] = 1.3;
                B[2, 1] = 2.8;
                B[2, 2] = 5.1;

                clsMatrizEsparsa Bs = new clsMatrizEsparsa(B);
                clsMatrizEsparsa As = new clsMatrizEsparsa(A);

                B = Bs.AsDoubleMatrix();

                clsMatrizEsparsa Diff = fme.MatrizSoma(Bs, As, 1.0, -1.0);
                double[,] Diff1 = Diff.AsDoubleMatrix();

                double[,] Diff2 = clt.MatrizSubtracao(B, A);

                //------------- testando a solução de sistemas lineares ------------//

                //NRsparseMat Cm = new NRsparseMat();
                //Cm.ncols = C.n;
                //Cm.nrows = C.m;
                //Cm.nvals = C.nzmax;
                //Cm.row_ind = C.row_indices;
                //Cm.val = C.x;
                //Cm.col_ptr = C.p;

                //double[] x = new double[4];
                //x[0] = 1.0;
                //x[1] = 1.0;
                //x[2] = 1.0;
                //x[3] = 1.0;

                int[] xi = new int[0];
                int[] pinv = null;

                double[] b = new double[4];
                b[0] = 1.0;
                b[2] = 0.0;

                //int iter = 0;
                //double err = 0.0;

                //NRsparseLinbcg nrs = new NRsparseLinbcg(Cm);
                //nrs.solve(ref b, ref x, 1, 1.0e-4, 10000, ref iter, ref err);

                double[] xr = fme.SolveLinSystem(C, b);

                //int rp = fme.cs_spsolve(Bs, As, 0, ref xi, ref x, pinv, 1);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                clsUtilTools clt = new clsUtilTools();
                double[,] a = clt.MatrizDiagonal(clt.Ones(4, 1));
                a[0, 1] = a[1, 0] = -0.5;

                double rmin = -1.0;
                double rmax = 0.99;
                int order = 50;
                int iter = 30;

                clsMatrizEsparsa sws = new clsMatrizEsparsa(a);

                double[,] rho = new double[0, 0];
                double[,] lndet = new double[0, 0];
                double[,] low95 = new double[0, 0];
                double[,] high95 = new double[0, 0];

                clsLogDetMatrizEsparsa cld = new clsLogDetMatrizEsparsa();

                cld.lndetmc(order, iter, sws, rmin, rmax, 0.01, ref rho, ref lndet, ref low95, ref high95);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Context menu strips
        
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    FormCalculadora frm = new FormCalculadora();
                    //frm.MdiParent = this.MdiParent;
                    frm.Dados = this.m_dt_tabela_dados;
                    frm.AtivaMedidasPoligonos = true;
                    if (m_dados_concatenados)
                    {
                        frm.DadosConcatenados = true;
                        frm.Shape = m_shape;
                    }

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.m_dt_tabela_dados = frm.Dados;

                        this.AtualizaTabelaDadosCalculadora();
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

        private void calculadoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    FormCalculadora frm = new FormCalculadora();
                    //frm.MdiParent = this.MdiParent;
                    frm.Dados = this.m_dt_tabela_dados;
                    if (m_dados_concatenados)
                    {
                        frm.DadosConcatenados = true;
                        frm.Shape = m_shape;
                    }
                    else
                    {
                        if (m_W_sparsa_from_dists_existente)
                        {
                            frm.UsaMatrizEsparsaFromDistancias = true;
                            frm.MatrizEsparsaFromDistancias = m_W_sparsa_from_dists;
                        }
                        if (m_W_sparsa_from_arquivo_existente)
                        {
                            frm.UsaMatrizEsparsaFromDistancias = true;
                            frm.MatrizEsparsaFromDistancias = m_W_sparsa_from_arquivo;
                        }
                    }

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.m_dt_tabela_dados = frm.Dados;

                        this.AtualizaTabelaDadosCalculadora();
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

        private void geraçãoDeVariáveisDummyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    FormGeracaoDummies frm = new FormGeracaoDummies();
                    //frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.m_dt_tabela_dados.Copy();

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.m_dt_tabela_dados = frm.TabelaDados;

                        this.AtualizaTabelaDados();
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

        private void excluirVariáveisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    FormCalculadora frm = new FormCalculadora();
                    //frm.MdiParent = this.MdiParent;
                    frm.Dados = this.m_dt_tabela_dados;
                    frm.AtivaExclusaoVariaveis = true;
                    if (m_dados_concatenados)
                    {
                        frm.DadosConcatenados = true;
                        frm.Shape = m_shape;
                    }

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.m_dt_tabela_dados = frm.Dados;

                        this.AtualizaTabelaDados();
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

        private void salvarTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBoxResultadosEstimacao.SelectAll();
                Application.DoEvents();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblProgressBar.Text = "Importando arquivo em formato shape";

                this.OpenShapeFile();

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
                ImportarArquivoDeDados();
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
                ExportData ed = new ExportData();
                ed.ExportarDados(dataGridView1, this.Name);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exportarSeleçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string texto = richTextBoxResultadosEstimacao.SelectedText;
                
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = "C:\\";
                saveFileDialog1.Filter = "Documento Word 2003 (*.doc)|*.doc|Texto (*.txt)|*.txt";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                    string strFile = saveFileDialog1.FileName;
                   
                    //Cria uma string para exportar
                    StreamWriter meustream = new StreamWriter(strFile);

                    meustream.WriteLine(texto);

                    meustream.Close();
                
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                string texto = richTextValoresGerados.SelectedText;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = "C:\\";
                saveFileDialog1.Filter = "Documento Word 2003 (*.doc)|*.doc|Texto (*.txt)|*.txt";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                    string strFile = saveFileDialog1.FileName;

                    //Cria uma string para exportar
                    StreamWriter meustream = new StreamWriter(strFile);

                    meustream.WriteLine(texto);

                    meustream.Close();

                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                richTextValoresGerados.SelectAll();
                Application.DoEvents();

                //string v = richTextBoxResultadosEstimacao.SelectedText;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                richTextValoresGerados.SelectAll();

                string texto = richTextValoresGerados.SelectedText;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = "C:\\";
                saveFileDialog1.Filter = "Documento Word 2003 (*.doc)|*.doc|Texto (*.txt)|*.txt";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                    string strFile = saveFileDialog1.FileName;

                    //Cria uma string para exportar
                    StreamWriter meustream = new StreamWriter(strFile);

                    meustream.WriteLine(texto);

                    meustream.Close();

                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exportarParaTXTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBoxResultadosEstimacao.SelectAll();

                string texto = richTextBoxResultadosEstimacao.SelectedText;

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = "C:\\";
                saveFileDialog1.Filter = "Documento Word 2003 (*.doc)|*.doc|Texto (*.txt)|*.txt";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                    string strFile = saveFileDialog1.FileName;

                    //Cria uma string para exportar
                    StreamWriter meustream = new StreamWriter(strFile);

                    meustream.WriteLine(texto);

                    meustream.Close();


                    Cursor.Current = Cursors.Default;
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
                    FormEstatisticasDescritivas frm = new FormEstatisticasDescritivas();
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
                    FormTabelaFrequencias frm = new FormTabelaFrequencias();
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
                    FormTabelasCruzadas frm = new FormTabelasCruzadas();
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
                    FormCorrelacoes frm = new FormCorrelacoes();
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
        
        #region Operações para o thread

        private delegate void LimparVariaveisGeradasCall(DataGridView dtview);
        private void LimparVariaveisGeradas(DataGridView dtview)
        {
            blr.LimparTabelaDasVariaveisGeradas(ref this.m_dt_tabela_dados);
            dtview.DataSource = this.m_dt_tabela_dados;
        }

        private delegate void AtualizarDataGridViewCall(DataGridView dtview, DataTable dt);
        private void AtualizarDataGridView(DataGridView dtview, DataTable dt)
        {
            dtview.DataSource = dt;
            this.m_dt_tabela_dados = dt;
            this.AtualizaTabelaDadosCalculadora();
            //this.AtualizaTabelaDados();
        }

        private delegate void ApresentaJanelaRichTextCall(RichTextBox rtext, string texto, bool limpar);
        private void ApresentaJanelaRichText(System.Windows.Forms.RichTextBox rtext, string texto, bool limpar)
        {
            if (limpar)
            {
                rtext.Text = texto;
            }
            else
            {
                rtext.Text += texto;
            }
        }

        private delegate void MostraLabelCall(Label lbl, string texto);
        private void MostraLabel(Label lbl, string texto)
        {
            lbl.Text = texto;
        }

        private delegate void MostraTabPageCall(TabPage tab, bool mostra_tab);
        private void MostraTabPage(TabPage tab, bool mostra_tab)
        {
            if (!this.tabControl1.TabPages.Contains(tab))
            {
                this.tabControl1.TabPages.Add(tab);
            }
            if (mostra_tab) this.tabControl1.SelectedTab = tab;
        }

        private delegate void InterrompeThreadPorEscolhaCall();
        private void InterrompeThreadPorEscolha()
        {
            this.m_thread_estimacoes.Abort();
            this.m_thread_progress_bar.Abort();

            this.btnEstimarModelo.Enabled =
                this.btnEstimarModelo1.Enabled = true;

            this.btnInterromper1.Enabled =
                this.btnInterromper2.Enabled = false;

            this.lblProgressBar.Text = "Estimação do modelo interrompida pelo usuário.";
            this.progressBar1.Value = 0;

            MessageBox.Show("Estimação interrompida pelo usuário.", "Estimações", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private delegate void InterrompeThreadPorFalhaCall(string mensagem_erro);
        private void InterrompeThreadPorFalha(string mensagem_erro)
        {
             
             this.m_thread_estimacoes.Abort();
             this.m_thread_progress_bar.Abort(); //está dando erro

             this.btnEstimarModelo.Enabled =
                 this.btnEstimarModelo1.Enabled = true;

             this.btnInterromper1.Enabled =
                 this.btnInterromper2.Enabled = false;

             this.progressBar1.Value = 0;
            
            if (!m_interrupcao_usuario)
            {
                this.lblProgressBar.Text = "Estimação do modelo de regressão falhou.";
                MessageBox.Show(mensagem_erro, "Falha na estimação", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                m_interrupcao_usuario = false;
            }
        }

        private delegate void MostraBotoesExecucaoCall();
        private void MostraBotoesExecucao()
        {
            this.btnEstimarModelo1.Enabled =
                this.btnEstimarModelo.Enabled = true;

            this.btnInterromper1.Enabled =
                this.btnInterromper2.Enabled = false;
        }

       
        
        private void FormRegressaoGMMConley_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.m_thread_estimacoes != null)
            {
                this.m_thread_estimacoes.Abort();
                this.m_thread_estimacoes = null;
            }

            if (this.m_thread_progress_bar != null)
            {
                this.m_thread_progress_bar.Abort();
                this.m_thread_progress_bar = null;
            }
        }

        private void btnInterromper2_Click(object sender, EventArgs e)
        {
            this.InterromperExecucao();
        }

        private void btnInterromper1_Click(object sender, EventArgs e)
        {
            this.InterromperExecucao();
        }

        private bool m_interrupcao_usuario = false;
        private void InterromperExecucao()
        {
            try
            {
                DialogResult disp = MessageBox.Show("Deseja abortar a estimação?", "Interromper Estimação", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (disp == DialogResult.Yes)
                {
                    //throw new Exception("Interrupção pelo usuário.");

                    m_interrupcao_usuario = true;
                    this.btnEstimarModelo.Invoke(new InterrompeThreadPorEscolhaCall(this.InterrompeThreadPorEscolha),
                        new object[] { });
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        #endregion

        #region Importação e exportação de matrizes esparsas

        private void exportarMatrizDeVizinhançaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.m_W_sparsa_from_dists_existente || this.m_W_sparsa_from_arquivo_existente)
                {
                    if (this.m_W_sparsa_from_dists_existente)
                    {
                        FormSalvamentoMatrizEsparsa frm = new FormSalvamentoMatrizEsparsa(false);
                        frm.MdiParent = this.MdiParent;
                        frm.WsparsaFromDists = this.m_W_sparsa_from_dists;
                        if (this.rdbMatrizVizinhancaNormalizada.Checked) frm.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Normalizada;
                        else frm.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Original;
                        frm.DtDados = m_dt_tabela_dados;
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                        }
                    }
                    else
                    {
                        FormSalvamentoMatrizEsparsa frm = new FormSalvamentoMatrizEsparsa(false);
                        frm.MdiParent = this.MdiParent;
                        frm.WsparsaFromDists = this.m_W_sparsa_from_arquivo;
                        if (this.rdbMatrizVizinhancaNormalizada.Checked) frm.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Normalizada;
                        else frm.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Original;
                        frm.DtDados = m_dt_tabela_dados;
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                        }
                    }
                }
                else
                {
                    if (this.m_dados_concatenados)
                    {
                        FormSalvamentoMatrizEsparsa frm = new FormSalvamentoMatrizEsparsa(false);
                        frm.MdiParent = this.MdiParent;
                        frm.Shape = this.m_shape;
                        frm.DtDados = m_dt_tabela_dados;
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                        }
                    }
                    else
                    {
                        if (this.m_vizinhanca_definida)
                        {
                            FormSalvamentoMatrizEsparsa frm = new FormSalvamentoMatrizEsparsa(false);
                            frm.MdiParent = this.MdiParent;
                            frm.Shape = this.m_shape;
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                            }
                        }
                        else
                        {
                            throw new Exception("Matriz de vizinhança ainda não foi definida.");
                        }
                    }
                }                
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool m_W_sparsa_from_arquivo_existente = false;
        private clsMatrizEsparsa m_W_sparsa_from_arquivo = new clsMatrizEsparsa();

        private void importarMatrizDeVizinhançaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    FormSalvamentoMatrizEsparsa frm = new FormSalvamentoMatrizEsparsa(true);
                    frm.MdiParent = this.MdiParent;
                    frm.DtDados = m_dt_tabela_dados;
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        m_dados_concatenados = false;
                        m_W_sparsa_from_dists_existente = false;

                        m_W_sparsa_from_arquivo_existente = true;
                        m_W_sparsa_from_arquivo = frm.WsparsaImportada;

                        if (!m_usa_W_sparsa_predefinida)
                        {
                            this.grbTipoMatrizVizinhanca.Enabled = true;
                        }

                        if (frm.TipoMatrizVizinhanca == TipoMatrizVizinhanca.Normalizada) this.rdbMatrizVizinhancaNormalizada.Checked = true;
                        else this.rdbMatrizVizinhancaOriginal.Checked = true;

                        this.ckbWXcovariaveis.Enabled = true;
                        this.rdbModelosSAR_viaMLE.Enabled = true;
                        this.rdbModelosSEM_viaMLE.Enabled = true;
                        this.rdbModeloSAC_viaMLE.Enabled = true;

                        this.rdbSARKelejianPrucha.Enabled = true;
                        this.rdbModelosSEM_viaMLE.Checked = true;
                        this.grbTipoMatrizVizinhanca.Enabled = false;
                        this.grbCalculoLogDet.Enabled = true;

                        MessageBox.Show("Matriz de vizinhança importada com sucesso. A partir de agora, todos os cálculos serão feitos considerando-se essa nova matriz.",
                                        "Matriz de vizinhança", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        #endregion

        #region Atualizar tabela no formulário de mapas

        private void btnAtualizarTabelaExterna_Click(object sender, EventArgs e)
        {
            try
            {
                m_gridview_externo.DataSource = ((DataTable)this.dataGridView1.DataSource).Copy();

                this.DataSetExterno.Tables.Clear();
                this.DataSetExterno.Tables.Add(((DataTable)this.dataGridView1.DataSource).Copy());

                lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Abrir tabela de dados em Excel

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            try
            {
                if (((DataTable)dataGridView1.DataSource).Columns.Count > 0 && ((DataTable)dataGridView1.DataSource).Rows.Count > 0)
                {
                    DataTable dsTemp = (DataTable)dataGridView1.DataSource;
                    //dsTemp.Tables[0].Columns.Remove("Mapa"+strIDmapa);
                    //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    //saveFileDialog1.InitialDirectory = "C:\\";
                    //saveFileDialog1.Filter = "Excel (*.xls)|*.xls|Access (*.mdb)|*.mdb|XML (*.xml)|*.xml|Texto (*.txt)|*.txt";
                    //saveFileDialog1.FilterIndex = 1;
                    //saveFileDialog1.RestoreDirectory = true;
                    //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;

                        BLExportacaoAberturaExcel ble = new BLExportacaoAberturaExcel();
                        ble.ExportaToExcel(dsTemp, "tabela", "tabela");

                        Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

    }
}
