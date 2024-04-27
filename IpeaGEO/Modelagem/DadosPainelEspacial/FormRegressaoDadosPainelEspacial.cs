﻿using System;
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

using IpeaGeo.Modelagem;

namespace IpeaGeo.RegressoesEspaciais
{
    public partial class FormRegressaoDadosPainelEspacial : FormStatisticalAnalysis
    {
        #region variáveis internas

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

        public new string PeriodoFocoPainel
        {
            set { m_periodo_foco = value; }
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

                AtualizaTabelaDados();

                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    this.btnEstimarModelo.Enabled = 
                        this.btnEstimarModelo1.Enabled= true;
                }
            }
        }

        public new string LabelTabelaDados
        {
            set
            {
                m_label_tabela_dados = value;
                this.Text = "Regressão Linear com Dados de Painel Espacial - " + value;
            }
        }

        #endregion

        #region Carrega formulário

        public FormRegressaoDadosPainelEspacial()
        {
            InitializeComponent();
        }

        private bool m_shape_carregado_do_mapa = false;
        private DataTable m_dt_dados_originais = new DataTable();

        public override void HabilitarDadosExternos()
        {
            btnAtualizarTabelaExterna.Visible = true;
            btnAtualizarTabelaExterna.Enabled = true;

            btnAbrirArquivoShape.Enabled =
                btnImportarTabela.Enabled = false;

            m_shape_carregado_do_mapa = true;
        }

        public new DataGridView GridViewExterno
        {
            set { m_gridview_externo = value; }
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
            if (this.m_vizinhanca_definida == true) this.btnImportarTabela.Enabled = true;
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
            if (this.m_vizinhanca_definida == true) this.btnImportarTabela.Enabled = true;

            if (!m_usa_W_sparsa_predefinida)
            {
                m_dados_concatenados = false;
                this.rdbModelosSAR_viaMLE.Enabled = false;
                this.rdbModelosSEM_viaMLE.Enabled = false;
                this.rdbModeloSAC_viaMLE.Enabled = false;

                this.ckbWXcovariaveis.Enabled = false;

                this.grbTipoMatrizVizinhanca.Enabled = false;
                this.grbCalculoLogDet.Enabled = false;
            }

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

                this.btnEstimarModelo.Enabled =
                    this.btnEstimarModelo1.Enabled = true;
                
                this.rdbModelosSAR_viaMLE.Enabled = true;
                this.rdbModelosSEM_viaMLE.Enabled = true;
                this.rdbModeloSAC_viaMLE.Enabled = true;
            }
        }

        private void FormRegressaoGMMConley_Load(object sender, EventArgs e)
        {
            if (!m_usa_W_sparsa_predefinida) lblAvisoMatrizWPredefinida.Text = "";

            if (this.tabControl1.TabPages.Contains(tabPage2))
                this.tabControl1.TabPages.Remove(tabPage2);

            if (this.tabControl1.TabPages.Contains(tabPage3))
                this.tabControl1.TabPages.Remove(tabPage3);

            if (this.tabControl1.TabPages.Contains(tabPage6))
                this.tabControl1.TabPages.Remove(tabPage6);

            this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = false;
            this.grbCoordenadasGeograficas.Enabled = false;
            this.grbSelecaoBandwidth.Enabled = false;

            btnImportarTabela.Enabled = false;

            if (m_dt_tabela_dados.Rows.Count <= 0 || m_dt_tabela_dados.Columns.Count <= 0)
            {
                this.btnEstimarModelo.Enabled =
                    this.btnEstimarModelo1.Enabled = false;
            }
        }

        #endregion

        #region Eventos

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

                this.ckbIterationTillConvergence.Enabled = false;

                this.ckbWXcovariaveis.Enabled = false;
            }
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

                this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = false;
                this.grbCoordenadasGeograficas.Enabled = false;
                this.grbSelecaoBandwidth.Enabled = false;
                this.grbCalculoLogDet.Enabled = true;

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

                this.userControlRegressaoInstrumentos1.SelecaoInstrumentosDisponivel = false;
                this.grbCoordenadasGeograficas.Enabled = false;
                this.grbSelecaoBandwidth.Enabled = false;
                this.grbCalculoLogDet.Enabled = true;

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

        private void EstimaModelo()
        {
            try
            {
                #region checando presença de valores inválidos nos dados

                int[] indicadores_val_invalidos;

                for (int i = 0; i < userControlRegressaoInstrumentos1.VariavelDependente.GetLength(0); i++)
                {
                    if (clt.ChecaValoresDoubleInvalidos(this.m_dt_tabela_dados, userControlRegressaoInstrumentos1.VariavelDependente[i], out indicadores_val_invalidos))
                    {
                        MessageBox.Show("Há valores numéricos inválidos na variável " + userControlRegressaoInstrumentos1.VariavelDependente[i] + ". Cheque a sua base de dados.");
                        return;
                    }
                }

                for (int i = 0; i < userControlRegressaoInstrumentos1.VariaveisIndependentes.GetLength(0); i++)
                {
                    if (clt.ChecaValoresDoubleInvalidos(this.m_dt_tabela_dados, userControlRegressaoInstrumentos1.VariaveisIndependentes[i], out indicadores_val_invalidos))
                    {
                        MessageBox.Show("Há valores numéricos inválidos na variável " + userControlRegressaoInstrumentos1.VariaveisIndependentes[i] + ". Cheque a sua base de dados.");
                        return;
                    }
                }

                #endregion

                #region checando multicolinearidade perfeita das regressores

                blsp.VariaveisIndependentes = userControlRegressaoInstrumentos1.VariaveisIndependentes;
                blsp.TabelaDados = this.m_dt_tabela_dados;

                string mensagem_colinear;

                if (blsp.ChecarMulticolinearidade(out mensagem_colinear))
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

                blsp.CutOffCoordenadaX = Convert.ToDouble(this.nudBandwidthX.Value);
                blsp.CutOffCoordenadaY = Convert.ToDouble(this.nudBandwidthY.Value);

                blsp.VariavelCoordenadaX = this.lstCoordenadasX.SelectedItem.ToString();
                blsp.VariavelCoordenadaY = this.lstCoordenadasY.SelectedItem.ToString();

                blsp.CutOffCoordenadaX = Convert.ToDouble(this.nudBandwidthX.Value);
                blsp.CutOffCoordenadaY = Convert.ToDouble(this.nudBandwidthY.Value);

                blsp.VariavelCoordenadaX = this.lstCoordenadasX.SelectedItem.ToString();
                blsp.VariavelCoordenadaY = this.lstCoordenadasY.SelectedItem.ToString();

                //blsp.BandWidthKernelHAC = Convert.ToDouble(this.nudBandwidthKernelHAC.Value);
                blsp.VariavelCoordenadaX = this.lstCoordenadasX.SelectedItem.ToString();
                blsp.VariavelCoordenadaY = this.lstCoordenadasY.SelectedItem.ToString();

                //blsp.UsaNumVizinhosParaDistancia = this.ckbNumVizinhosDistancia.Checked;
                //blsp.NumVizinhosParaDistancia = Convert.ToInt32(nudNumVizinhosDistancia.Value);

                //if (this.rdbKelejianPruchaResHomocedasticos.Checked) blsp.TipoCorrecaoCovMatrix = TipoCorrecaoMatrizCovariancia.SemCorrecao;
                //if (this.rdbKelejianPruchaResHeteroscedasticos.Checked) blsp.TipoCorrecaoCovMatrix = TipoCorrecaoMatrizCovariancia.Heteroscedasticidade;
                //if (this.rdbKelejianPruchaHAC.Checked) blsp.TipoCorrecaoCovMatrix = TipoCorrecaoMatrizCovariancia.HAC;

                //if (this.rdbBarlletKernelHAC.Checked) blsp.TipoKernelCorrecaoHAC = TipoKernelCorrecaoHAC.Barlett;
                //if (this.rdbEpanechnikovKernelHAC.Checked) blsp.TipoKernelCorrecaoHAC = TipoKernelCorrecaoHAC.Epanechnikov;
                //if (this.rdbBisquareKernelHAC.Checked) blsp.TipoKernelCorrecaoHAC = TipoKernelCorrecaoHAC.Biquadrado;

                if (rdbPooledOLS.Checked) blsp.TipoEstimacaoPainel = Modelagem.TipoEstimacaoDadosPainel.PooledOLS;
                if (rdbFixedEffects.Checked) blsp.TipoEstimacaoPainel = Modelagem.TipoEstimacaoDadosPainel.EfeitosFixos;
                if (rdbRandomEffects.Checked) blsp.TipoEstimacaoPainel = Modelagem.TipoEstimacaoDadosPainel.EfeitosAleatorios;
                if (rdbFirstDiferencesEstimator.Checked) blsp.TipoEstimacaoPainel = Modelagem.TipoEstimacaoDadosPainel.PrimeirasDiferencas;

                blsp.VarUnidadeObservacional = cmbVariavelUnidadesObservacionais.SelectedItem.ToString();
                blsp.VarUnidadeTemporal = cmbVariavelPeriodosTempo.SelectedItem.ToString();

                #endregion

                m_thread_estimacoes = new Thread(new ThreadStart(this.EstimacaoModelosPainelEspacial));
                m_thread_estimacoes.Name = "Estimação modelo";
                m_thread_estimacoes.Start();
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

        private IpeaGeo.Modelagem.BLogicRegressaoLinearDadosPainelEspacial blsp = new Modelagem.BLogicRegressaoLinearDadosPainelEspacial();
        private clsUtilTools clt = new clsUtilTools();

        private void EstimacaoModelosPainelEspacial()
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

                blsp.VariaveisDependentes = userControlRegressaoInstrumentos1.VariavelDependente;
                blsp.VariaveisIndependentes = userControlRegressaoInstrumentos1.VariaveisIndependentes;
                blsp.VariaveisInstrumentais = userControlRegressaoInstrumentos1.VariaveisInstrumentais;

                if (blsp.VariaveisDependentes.GetLength(0) < 1)
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

                if (blsp.VariaveisIndependentes.GetLength(0) < 1)
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

                blsp.IncluiIntercepto = this.ckbIncluiIntercepto.Checked;
                blsp.ApresentaCovMatrixBetaHat = this.ckbApresentaCovMatrixBetaHat.Checked;
                blsp.TabelaDados = this.m_dt_tabela_dados;
                blsp.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo;
                blsp.AdicionaNovaVariaveis = this.ckbIncluirNovasVariaveisTabelaDados.Checked;

                blsp.UsaDummiesTemporais = ckbIncluiDummiesTemporais.Checked;
                blsp.UsaTendenciaTemporalLinear = ckbIncluiTendenciaTemporalLinear.Checked;
                blsp.UsaTendenciaTemporalQuadratica = ckbIncluiTendenciaTemporalQuadratica.Checked;
                blsp.UsaTendenciaTemporalCubica = ckbTendenciaTemporalCubica.Checked;

                #region opções para cálculo do log-determinante de matrizes esparsas

                if (rdbLogDetLUEsparsa.Checked)
                {
                    blsp.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.MatrizEsparsaDecomposicaoLU;
                }

                if (this.rdbLogDetMatrizDensa.Checked)
                {
                    blsp.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.MatrizDensa;

                    int n = m_dt_tabela_dados.Rows.Count;

                    if (this.rdbModelosSAR_viaMLE.Checked || this.rdbModelosSEM_viaMLE.Checked || this.rdbModeloSAC_viaMLE.Checked)
                    {
                        if (n <= 2000)
                        {
                            blsp.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.MatrizDensa;
                        }
                        else
                        {
                            blsp.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.MatrizEsparsaDecomposicaoLU;
                            MessageBox.Show("Tabela de dados contém mais de 2000 observações. Será utilizado cálculo do determinante via decomposição LU para matrizes esparsas.",
                                "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.rdbLogDetLUEsparsa.Checked = true;
                        }
                    }
                }

                if (this.rdbLogDetMonteCarlo.Checked)
                {
                    blsp.TipoCalculoLogDetW = TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo;
                }

                #endregion

                #region modelos de painel não-espacial 

                if (rdbSemDependenciaEspacial.Checked)
                {
                    blsp.EstimarModelo();
                }

                #endregion

                #region modelos SAR (via MLE), SEM (via MLE) e SAC (via MLE)

                 
                #region SAR via MLE

                if (this.rdbModelosSAR_viaMLE.Checked)
                {
                    if (this.rdbMatrizVizinhancaNormalizada.Checked) blsp.TipoMatrizVizinhanca = IpeaGeo.TipoMatrizVizinhanca.Normalizada;
                    if (this.rdbMatrizVizinhancaOriginal.Checked) blsp.TipoMatrizVizinhanca = IpeaGeo.TipoMatrizVizinhanca.Original;

                    if (!this.m_W_sparsa_from_dists_existente && !this.m_W_sparsa_from_arquivo_existente && !this.m_usa_W_sparsa_predefinida)
                    {
                        blsp.Shape = this.m_shape;
                        blsp.GeraMatrizVizinhanca();
                        blsp.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.ArquivoShape;
                    }
                    else
                    {
                        if (this.m_W_sparsa_from_dists_existente)
                        {
                            blsp.MatrizEsparsaFromDistancias = m_W_sparsa_from_dists;
                            blsp.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizFromDistancias;
                        }
                        if (this.m_W_sparsa_from_arquivo_existente)
                        {
                            blsp.MatrizEsparsaFromDistancias = m_W_sparsa_from_arquivo;
                            blsp.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizFromArquivo;
                        }
                        if (this.m_usa_W_sparsa_predefinida)
                        {
                            blsp.MatrizEsparsaFromDistancias = m_W_sparsa_predefinida;
                            blsp.TipoOrigemMatrizVizinhanca = TipoOrigemMatrizVizinhanca.MatrizPreDefinida;
                            blsp.TipoMatrizVizinhancaPredefinida = m_tipo_matriz_W_predefinida;
                        }
                    }

                    if (this.ckbMulticolinearidade.Checked)
                    {
                        if (!this.tabControl1.TabPages.Contains(tabPage2))
                        {
                            this.richTextBoxResultadosEstimacao.Text += blsp.AnaliseMulticolinearidade();
                        }
                        else
                        {
                            this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                    new object[] { richTextBoxResultadosEstimacao, blsp.AnaliseMulticolinearidade(), false });
                        }
                    }

                    blsp.EstimaModelosPainelEspacial();
                    blsp.EstimarModelo();
                    blsp.EstimacaoBemSucedida = true;
                }

                #endregion

                #endregion

                #region saída dos resultados

                if (!this.tabControl1.TabPages.Contains(tabPage2))
                {
                    this.richTextBoxResultadosEstimacao.Text += blsp.ResultadoEstimacao + "\n";
                    this.richTextEfeitosFixos.Text += blsp.TextoEfeitosFixos + "\n";

                    if (this.ckbLimpaJanelaNovasVariaveis.Checked)
                    {
                        this.richTextValoresGerados.Text = blsp.VariaveisGeradas + "\n";
                    }
                    else
                    {
                        this.richTextValoresGerados.Text += blsp.VariaveisGeradas + "\n";
                    }
                }
                else
                {
                    this.richTextBoxResultadosEstimacao.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                        new object[] { this.richTextBoxResultadosEstimacao, blsp.ResultadoEstimacao + "\n", false });

                    //this.richTextBoxResultadosEstimacao.Text += blr.ResultadoEstimacao + "\n";

                    if (this.ckbLimpaJanelaNovasVariaveis.Checked)
                    {
                        //this.richTextValoresGerados.Text = blr.VariaveisGeradas + "\n";

                        this.richTextValoresGerados.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                            new object[] { this.richTextValoresGerados, blsp.VariaveisGeradas + "\n", true });

                        if (blsp.TipoEstimacaoPainel == IpeaGeo.Modelagem.TipoEstimacaoDadosPainel.EfeitosFixos
                            || blsp.TipoEstimacaoPainel == Modelagem.TipoEstimacaoDadosPainel.PrimeirasDiferencas)
                        {
                            this.tabControl1.Invoke(new MostraTabPageCall(this.MostraTabPage),
                                new object[] { this.tabPage6, false });

                            this.richTextEfeitosFixos.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                new object[] { this.richTextEfeitosFixos, blsp.TextoEfeitosFixos + "\n", true });
                        }
                    }
                    else
                    {
                        //this.richTextValoresGerados.Text += blr.VariaveisGeradas + "\n";

                        this.richTextValoresGerados.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                            new object[] { this.richTextValoresGerados, blsp.VariaveisGeradas + "\n", false });

                        if (blsp.TipoEstimacaoPainel == IpeaGeo.Modelagem.TipoEstimacaoDadosPainel.EfeitosFixos
                            || blsp.TipoEstimacaoPainel == Modelagem.TipoEstimacaoDadosPainel.PrimeirasDiferencas)
                        {
                            this.tabControl1.Invoke(new MostraTabPageCall(this.MostraTabPage),
                                new object[] { this.tabPage6, false });

                            this.richTextEfeitosFixos.Invoke(new ApresentaJanelaRichTextCall(this.ApresentaJanelaRichText),
                                new object[] { this.richTextEfeitosFixos, blsp.TextoEfeitosFixos + "\n", false });
                        }
                    }
                }

                this.tabControl1.Invoke(new MostraTabPageCall(this.MostraTabPage),
                    new object[] { this.tabPage2, true });

                this.tabControl1.Invoke(new MostraTabPageCall(this.MostraTabPage),
                    new object[] { this.tabPage3, false });

                if (blsp.TipoEstimacaoPainel == IpeaGeo.Modelagem.TipoEstimacaoDadosPainel.EfeitosFixos
                    || blsp.TipoEstimacaoPainel == Modelagem.TipoEstimacaoDadosPainel.PrimeirasDiferencas)
                {
                    this.tabControl1.Invoke(new MostraTabPageCall(this.MostraTabPage),
                        new object[] { this.tabPage6, false });
                }
                else
                {
                    this.tabControl1.Invoke(new EsconderTabPageCall(this.EsconderTabPage),
                        new object[] { this.tabPage6 });
                }

                if (this.ckbIncluirNovasVariaveisTabelaDados.Checked)
                {
                    this.dataGridView1.Invoke(new AtualizarDataGridViewCall(this.AtualizarDataGridView),
                        new object[] { this.dataGridView1, blsp.TabelaDados });
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
            }
            catch (Exception er)
            {
                this.btnEstimarModelo.Invoke(new InterrompeThreadPorFalhaCall(this.InterrompeThreadPorFalha),
                    new object[] { er.Message });
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
            set { m_ordem_matriz_W_predefinida = value; }
        }

        public string TipoMatrizVizinhancaPredefinida
        {
            set { m_tipo_matriz_W_predefinida = value; }
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
                    
                    m_vizinhanca_definida = false;
                    m_dados_concatenados = false;
                    this.rdbModelosSAR_viaMLE.Enabled = false;
                    this.rdbModelosSEM_viaMLE.Enabled = false;
                    this.rdbModeloSAC_viaMLE.Enabled = false;

                    this.ckbWXcovariaveis.Enabled = false;
                }

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

        private string m_endereco_mapa = "";

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

                    m_endereco_mapa = FileName;

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
                    }

                    m_vizinhanca_definida = false;
                    m_dados_concatenados = false;
                    this.rdbModelosSAR_viaMLE.Enabled = false;
                    this.rdbModelosSEM_viaMLE.Enabled = false;
                    this.rdbModeloSAC_viaMLE.Enabled = false;

                    this.ckbWXcovariaveis.Enabled = false;

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
                this.lblProgressBar.Text = "Importando arquivo em formato shape";

                this.OpenShapeFile();

                this.lblProgressBar.Text = "Arquivo shape importado com sucesso";
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
                        for (int i = 0; i < v_aux.GetLength(0); i++) var_id[i] = Convert.ToString(v_aux[i, 0]);

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

                    if (!this.btnAtualizarTabelaExterna.Visible)
                    {
                        this.btnImportarTabela.Enabled = true;
                    }
                    else
                    {
                        if (!tabControl1.TabPages.Contains(tabPage1))
                        {
                            tabControl1.TabPages.Add(tabPage1);
                        }
                        tabControl1.SelectedTab = tabPage1;
                    }

                    if (m_shape_carregado_do_mapa)
                    {
                        //ConcatenarDados("Estrutura de vizinhança criada com sucesso");

                        lblProgressBar.Text = "Estrutura de vizinhança criada com sucesso";
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

        private string m_variavel_periodos_painel = "";
        private string m_variavel_unidades_painel = "";
        private object[,] m_freqs_periodos_painel_espacial = new object[0, 0];
        private DataSet m_ds_dados_originais_painel_espacial = new DataSet();

        private void ConcatenarDadosPainelEspacial()
        {
            FormConcatenacaoTabelaToShape frm = new FormConcatenacaoTabelaToShape();
            frm.TabelaShape = this.m_tabela_shape;
            frm.Shape = this.m_shape;
            frm.UsaDadosPainelEspacial = true;
            frm.EnderecoMapa = m_endereco_mapa;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                m_ds_dados_painel_espacial = frm.DsDadosPainelEspacial;
                m_freqs_periodos_painel_espacial = frm.FrequenciaPeriodosPainel;
                m_variavel_periodos_painel = frm.VariavelPeriodosPainel;
                m_variavel_unidades_painel = frm.VariavelUnidadesPainel;

                m_ds_dados_originais_painel_espacial = new DataSet();
                for (int i = 0; i < m_ds_dados_painel_espacial.Tables.Count; i++)
                {
                    m_ds_dados_originais_painel_espacial.Tables.Add(((DataTable)m_ds_dados_painel_espacial.Tables[i]).Copy());
                }

                ConcatenarDados();

                AtualizaTabelaDados();
            }
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

                if (Math.Floor((double)obs_dados / (double)obs_shape) != (double)obs_dados / (double)obs_shape) MessageBox.Show("O número de observações na tabela de dados não é compatível com o número de observações no arquivo shape.",
                    "Falha na concatenação", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (Math.Floor((double)obs_dados / (double)obs_shape) == (double)obs_dados / (double)obs_shape)
                {
                    m_dados_concatenados = true;

                    this.ckbWXcovariaveis.Enabled = true;
                    this.rdbModelosSAR_viaMLE.Enabled = true;
                    this.rdbModelosSEM_viaMLE.Enabled = true;
                    this.rdbModeloSAC_viaMLE.Enabled = true;

                    this.rdbModelosSEM_viaMLE.Checked = true;

                    if (!m_usa_W_sparsa_predefinida)
                    {
                        this.grbTipoMatrizVizinhanca.Enabled = true;
                    }
                    this.grbCalculoLogDet.Enabled = true;

                    this.tabControl1.SelectedTab = tabPage1;

                    this.m_W_sparsa_from_dists_existente = false;
                    this.m_W_sparsa_from_arquivo_existente = false;
                    
                    m_dt_tabela_dados = ((DataTable)m_ds_dados_painel_espacial.Tables[0]).Copy();
                    for (int i = 1; i < m_ds_dados_painel_espacial.Tables.Count; i++) m_dt_tabela_dados.Merge(((DataTable)m_ds_dados_painel_espacial.Tables[i]).Copy());

                    m_dt_dados_originais = m_dt_tabela_dados.Copy();

                    this.dataGridView1.DataSource = m_dt_tabela_dados;
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
                ConcatenarDadosPainelEspacial();

                //ImportarArquivoDeDados();
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
            blsp.LimparTabelaDasVariaveisGeradas(ref this.m_dt_tabela_dados);
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

        private delegate void EsconderTabPageCall(TabPage tab);
        private void EsconderTabPage(TabPage tab)
        {
            if (this.tabControl1.TabPages.Contains(tab))
            {
                this.tabControl1.TabPages.Remove(tab);
            }
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
            this.m_thread_progress_bar.Abort();

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

        private Thread m_thread_estimacoes;
        private Thread m_thread_progress_bar;

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

        #region tool strip menu para o richtext box de efeitos fixos

        private void selecionarTudoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.richTextEfeitosFixos.SelectAll();
                Application.DoEvents();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exportarTodaAJanelaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.richTextEfeitosFixos.SelectAll();

                string texto = this.richTextEfeitosFixos.SelectedText;

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

        private void exportarSeleçãoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string texto = this.richTextEfeitosFixos.SelectedText;

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

        private void tamanhoDaFonteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                FormTamanhoFonte frm = new FormTamanhoFonte();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    this.richTextEfeitosFixos.SelectAll();
                    this.richTextEfeitosFixos.SelectionFont = new Font("Courier New", frm.TamanhoFonte);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                ArrayList lista = blsp.possivel_lista_vars_periodos(m_dt_dados_originais, this.cmbVariavelUnidadesObservacionais.SelectedItem.ToString());

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
    }
}
