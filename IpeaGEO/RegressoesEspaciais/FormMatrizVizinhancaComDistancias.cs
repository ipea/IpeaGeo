using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IpeaGeo.RegressoesEspaciais
{
    public partial class FormMatrizVizinhancaComDistancias : Form
    {
        #region variáveis internas

        private clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            get { return m_shape; }
            set { m_shape = value; }
        }

        private bool m_definicao_geral_matriz_vizinhanca = false;
        public bool DefinicaoGeralMatrizVizinhanca
        {
            set
            {
                m_definicao_geral_matriz_vizinhanca = value;
                if (m_definicao_geral_matriz_vizinhanca)
                {
                    if (!tabControl1.TabPages.Contains(tabPage3)) tabControl1.TabPages.Add(tabPage3);
                    if (tabControl1.TabPages.Contains(tabPage1)) tabControl1.TabPages.Remove(tabPage1);
                    if (tabControl1.TabPages.Contains(tabPage2)) tabControl1.TabPages.Remove(tabPage2);

                    this.Text = "Definição da matriz de vizinhança";
                    this.rdbVizinhosContiguidade.Checked = true;
                    this.rdbVizinhosDistancia.Checked = false;
                    this.grbVizinhancasContiguidade.Enabled = true;
                }
            }
        }

        private clsMatrizEsparsa m_Wesparsa = new clsMatrizEsparsa();
        public clsMatrizEsparsa Wesparsa
        {
            get
            {
                return this.m_Wesparsa;
            }
        }

        private bool m_matriz_W_normalizada = true;
        public bool MatrizWNormalizada
        {
            get
            {
                return m_matriz_W_normalizada;
            }
        }

        private DataTable m_dt_dados = new DataTable();
        private bool m_form_iniciado = false;
        public DataTable Dados
        {
            get { return m_dt_dados.Copy(); }
            set 
            { 
                this.m_dt_dados = value.Copy(); 

                clsUtilTools clt = new clsUtilTools();
                string[] var_numericas = clt.RetornaColunasNumericas(m_dt_dados);

                if (var_numericas.GetLength(0) > 0)
                {
                    lstCoordenadasX.Items.Clear();
                    lstCoordenadasX.Items.AddRange(var_numericas);
                    lstCoordenadasX.SelectedIndex = 0;

                    lstCoordenadasY.Items.Clear();
                    lstCoordenadasY.Items.AddRange(var_numericas);
                    lstCoordenadasY.SelectedIndex = 0;

                    AjustaMaximasCoordenadas();
                    m_form_iniciado = true;
                }
            }
        }

        private string m_tipo_vizinhanca = "";
        public string TipoVizinhanca
        {
            get { return m_tipo_vizinhanca; }
        }

        private int m_ordem_vizinhanca = 1;
        public int OrdemVizinhanca
        {
            get { return m_ordem_vizinhanca; }
        }

        private bool m_habilita_matriz_contiquidade = true;
        public bool HabilitaMatrizContiguidade
        {
            get { return m_habilita_matriz_contiquidade; }
            set
            {
                m_habilita_matriz_contiquidade = value;

                if (!value)
                {
                    rdbVizinhosDistancia.Checked = true;
                    rdbVizinhosDistancia.Enabled =
                        rdbVizinhosContiguidade.Enabled = false;
                }
            }
        }

        #endregion

        public FormMatrizVizinhancaComDistancias()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {           
            this.DialogResult = DialogResult.Cancel;
        }

        private void FormMatrizVizinhancaComDistancias_Load(object sender, EventArgs e)
        {
            this.AjusteAutomaticoParametrosDecaimento();

            if (m_definicao_geral_matriz_vizinhanca)
            {
                if (!tabControl1.TabPages.Contains(tabPage3)) tabControl1.TabPages.Add(tabPage3);
                if (tabControl1.TabPages.Contains(tabPage1)) tabControl1.TabPages.Remove(tabPage1);
                if (tabControl1.TabPages.Contains(tabPage2)) tabControl1.TabPages.Remove(tabPage2);

                this.Text = "Definição da matriz de vizinhança";
                this.rdbVizinhosContiguidade.Checked = true;
                this.rdbVizinhosDistancia.Checked = false;
                this.grbVizinhancasContiguidade.Enabled = true;
            }
            else
            {
                this.rdbVizinhosContiguidade.Checked = false;
                this.rdbVizinhosDistancia.Checked = true;
                this.grbVizinhancasContiguidade.Enabled = false;

                if (this.tabControl1.TabPages.Contains(tabPage3))
                {
                    this.tabControl1.TabPages.Remove(tabPage3);
                }
                if (this.tabControl1.TabPages.Contains(tabPage2))
                {
                    this.tabControl1.TabPages.Remove(tabPage2);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ExecutarFuncao();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private BLMatrizDistanciasParametricas m_blm = new BLMatrizDistanciasParametricas();

        private void ExecutarFuncao()
        {
            if (rdbDecaimentoUniforme.Checked)
            {
                m_blm.TipoDecaimento = TipoFuncaoDecaimento.Uniforme;
            }
            if (rdbDecaimentoExponencial.Checked)
            {
                m_blm.TipoDecaimento = TipoFuncaoDecaimento.Exponencial;
            }
            if (rdbDecaimentoLinear.Checked)
            {
                m_blm.TipoDecaimento = TipoFuncaoDecaimento.Linear;
            }
            if (this.rdbDecaimentoQuadratico.Checked)
            {
                m_blm.TipoDecaimento = TipoFuncaoDecaimento.Quadratico;
            }
            if (this.rdbDecaimentoGaussiano.Checked)
            {
                m_blm.TipoDecaimento = TipoFuncaoDecaimento.Gaussiana;
            }

            clsUtilTools clt = new clsUtilTools();

            double[,] m_X = clt.GetMatrizFromDataTable(this.m_dt_dados, this.lstCoordenadasX.SelectedItem.ToString());
            double[,] m_Y = clt.GetMatrizFromDataTable(this.m_dt_dados, this.lstCoordenadasY.SelectedItem.ToString());

            m_blm.X = m_X;
            m_blm.Y = m_Y;

            m_blm.Parm1 = Convert.ToDouble(this.nudParm1FuncaoDecaimento.Value);
            m_blm.DistMax = Convert.ToDouble(this.nudDistEuclidianaMaxima.Value);

            if (!this.tabControl1.TabPages.Contains(tabPage2))
            {
                this.tabControl1.TabPages.Add(tabPage2);
            }
            this.tabControl1.SelectedTab = tabPage2;

            DataTable dt = m_blm.FuncaoDecaimento();

            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
            dataGridView1.Update();

            if (dataGridView1.ColumnCount <= 0)
            {
                dataGridView1.DataSource = dt;
            }

            if (dataGridView1.Columns.Contains("observacao"))
            {
                dataGridView1.Columns["observacao"].Width = 80;
                dataGridView1.Columns["observacao"].HeaderText = "Obs.";
            }

            if (dataGridView1.Columns.Contains("distancia"))
            {
                dataGridView1.Columns["distancia"].Width = 160;
                dataGridView1.Columns["distancia"].HeaderText = "Distância";
            }

            if (dataGridView1.Columns.Contains("funcao_decaimento"))
            {
                dataGridView1.Columns["funcao_decaimento"].Width = 150;
                dataGridView1.Columns["funcao_decaimento"].HeaderText = "Função de decaimento";
            }

            if (rdbMatrizVizinhancaNormalizada.Checked)
            {
                m_blm.MatrizWesparsaFromDistanciaNorm();
                m_matriz_W_normalizada = true;
            }
            else
            {
                m_blm.MatrizWesparsaFromDistancia();
                m_matriz_W_normalizada = false;
            }

            this.m_Wesparsa = m_blm.Wesparsa;

            if (ckbLimpaJanelaOutput.Checked)
            {
                this.richTextBoxResultadosEstimacao.Text = m_Wesparsa.EstatisticasDescritivas();
            }
            else
            {
                this.richTextBoxResultadosEstimacao.Text += m_Wesparsa.EstatisticasDescritivas();
            }

            m_tipo_vizinhanca = "Vizinhanças pelas distâncias";
            m_ordem_vizinhanca = 0;

            if (m_shape != null && m_shape.Count == m_dt_dados.Rows.Count)
            {
                clsIpeaShape clp = new clsIpeaShape();
                clp.DefinicaoVizinhosFromMatrizEsparsa(ref m_shape, m_Wesparsa);
            }
        }

        private void lstCoordenadasX_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_form_iniciado)
            {
                Cursor = Cursors.WaitCursor;

                this.AjustaMaximasCoordenadas();
                AjusteAutomaticoParametrosDecaimento();

                Cursor = Cursors.Default;
            }
        }

        private void lstCoordenadasY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_form_iniciado)
            {
                Cursor = Cursors.WaitCursor;

                this.AjustaMaximasCoordenadas();
                AjusteAutomaticoParametrosDecaimento();

                Cursor = Cursors.Default;
            }
        }

        private void AjustaMaximasCoordenadas()
        {
            m_alteracao_prop_dist = false;
            m_alteracao_max_dist = false;

            BLModelosCrossSectionSpaciaisLineares gmm = new BLModelosCrossSectionSpaciaisLineares();
            BLMatrizDistanciasParametricas blp = new BLMatrizDistanciasParametricas();

            double xmax = gmm.GeraMaximaDistancia(this.m_dt_dados, this.lstCoordenadasX.SelectedItem.ToString());
            double ymax = gmm.GeraMaximaDistancia(this.m_dt_dados, this.lstCoordenadasY.SelectedItem.ToString());
            double dmax = blp.GeraMaxDistancia(this.m_dt_dados, 
                this.lstCoordenadasX.SelectedItem.ToString(), this.lstCoordenadasY.SelectedItem.ToString());

            clsUtilTools clt = new clsUtilTools();

            lblMaxCoordenadaX.Text = clt.Double2Texto(xmax, 8);
            lblMaximaCoordenadaY.Text = clt.Double2Texto(ymax, 8);
            lblMaxDistEuclidiana.Text = clt.Double2Texto(dmax, 8);

            nudDistEuclidianaMaxima.Maximum = Convert.ToDecimal(dmax);
            nudDistEuclidianaMaxima.Value = Convert.ToDecimal(dmax * Convert.ToDouble(this.nudPropDistMaxima.Value) / 100.0);

            m_max_distancia_Euclidiana = dmax;

            m_alteracao_prop_dist = true;
            m_alteracao_max_dist = true;
        }

        private bool m_alteracao_prop_dist = false;
        private bool m_alteracao_max_dist = false;
        private double m_max_distancia_Euclidiana = 0.0;

        private void nudPropDistMaxima_ValueChanged(object sender, EventArgs e)
        {
            m_alteracao_max_dist = false;
            if (m_alteracao_prop_dist)
            {
                nudDistEuclidianaMaxima.Value = Convert.ToDecimal(m_max_distancia_Euclidiana 
                    * Convert.ToDouble(this.nudPropDistMaxima.Value) / 100.0);
            }
            m_alteracao_max_dist = true;
        }

        private void nudDistEuclidianaMaxima_ValueChanged(object sender, EventArgs e)
        {
            m_alteracao_prop_dist = false;
            if (m_alteracao_max_dist)
            {
                nudPropDistMaxima.Value = Convert.ToDecimal((Convert.ToDouble(this.nudDistEuclidianaMaxima.Value) / m_max_distancia_Euclidiana) * 100.0);
            }
            m_alteracao_prop_dist = true;
        }

        private void AjusteAutomaticoParametrosDecaimento()
        {
            if (rdbDecaimentoUniforme.Checked)
            {
                this.nudParm1FuncaoDecaimento.Value = Convert.ToDecimal(1.0);
                this.nudParm1FuncaoDecaimento.Enabled = false;
            }
            else
            {
                this.nudParm1FuncaoDecaimento.Enabled = true;
                if (rdbDecaimentoExponencial.Checked)
                {
                    double p = Math.Log(10.0) / Convert.ToDouble(this.nudDistEuclidianaMaxima.Value);
                    this.nudParm1FuncaoDecaimento.Value = Convert.ToDecimal(p);
                }
                if (rdbDecaimentoLinear.Checked)
                {
                    this.nudParm1FuncaoDecaimento.Value = this.nudDistEuclidianaMaxima.Value;
                }
                if (this.rdbDecaimentoQuadratico.Checked)
                {
                    this.nudParm1FuncaoDecaimento.Value = this.nudDistEuclidianaMaxima.Value;
                }
                if (this.rdbDecaimentoGaussiano.Checked)
                {
                    this.nudParm1FuncaoDecaimento.Value = Convert.ToDecimal(Convert.ToDouble(this.nudDistEuclidianaMaxima.Value) / 2.0);
                }
            }
        }

        private void rdbDecaimentoExponencial_CheckedChanged(object sender, EventArgs e)
        {
            AjusteAutomaticoParametrosDecaimento();
        }

        private void rdbDecaimentoUniforme_CheckedChanged(object sender, EventArgs e)
        {
            AjusteAutomaticoParametrosDecaimento();
        }

        private void rdbDecaimentoGaussiano_CheckedChanged(object sender, EventArgs e)
        {
            AjusteAutomaticoParametrosDecaimento();
        }

        private void rdbDecaimentoQuadratico_CheckedChanged(object sender, EventArgs e)
        {
            AjusteAutomaticoParametrosDecaimento();
        }

        private void rdbDecaimentoLinear_CheckedChanged(object sender, EventArgs e)
        {
            AjusteAutomaticoParametrosDecaimento();
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

        private void rdbMatrizVizinhancaOriginal_CheckedChanged(object sender, EventArgs e)
        {
            this.m_matriz_W_normalizada = rdbMatrizVizinhancaNormalizada.Checked;
        }

        private void rdbMatrizVizinhancaNormalizada_CheckedChanged(object sender, EventArgs e)
        {
            this.m_matriz_W_normalizada = rdbMatrizVizinhancaNormalizada.Checked;
        }

        private void rdbVizinhosDistancia_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdbVizinhosDistancia.Checked)
                {
                    this.grbVizinhancasContiguidade.Enabled = false;

                    if (!tabControl1.TabPages.Contains(tabPage1)) tabControl1.TabPages.Add(tabPage1);
                    if (tabControl1.TabPages.Contains(tabPage2)) tabControl1.TabPages.Remove(tabPage2);
                    tabControl1.SelectedTab = tabPage1;
                }
                else
                {
                    if (!tabControl1.TabPages.Contains(tabPage3)) tabControl1.TabPages.Add(tabPage3);
                    if (tabControl1.TabPages.Contains(tabPage1)) tabControl1.TabPages.Remove(tabPage1);
                    if (tabControl1.TabPages.Contains(tabPage2)) tabControl1.TabPages.Remove(tabPage2);

                    this.grbVizinhancasContiguidade.Enabled = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected clsLinearRegressionModelsMLE m_geomle = new clsLinearRegressionModelsMLE();

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                progressBar1.Visible = true;
                if (m_shape != null && m_shape.Count > 0)
                {
                    clsIpeaShape cs = new clsIpeaShape();

                    m_ordem_vizinhanca = Convert.ToInt32(nudOrdemVizinhanca.Value);

                    int tipo_vizinhanca = 2;
                    this.m_tipo_vizinhanca = "Rook";
                    m_matriz_W_normalizada = false;

                    if (this.rdbQueen.Checked)
                    {
                        tipo_vizinhanca = 1;
                        this.m_tipo_vizinhanca = "Queen";
                        m_matriz_W_normalizada = false;
                    }

                    if (this.rdbRookN.Checked)
                    {
                        tipo_vizinhanca = 4;
                        this.m_tipo_vizinhanca = "Rook Normalizada";
                        m_matriz_W_normalizada = true;
                    }

                    if (this.rdbQueenN.Checked)
                    {
                        tipo_vizinhanca = 3;
                        m_tipo_vizinhanca = "Queen Normalizada";
                        m_matriz_W_normalizada = true;
                    }

                    this.lblProgressBar.Text = "Evolução da criação da vizinhança dos polígonos";

                    cs.DefinicaoVizinhos(ref m_shape, tipo_vizinhanca, ref progressBar1);
                    cs.AdicionaVizinhoProximo(ref m_shape);

                    m_geomle.Shape = m_shape;
                    m_geomle.MatrizWesparsaFromVizinhosComPesos();

                    this.m_Wesparsa = m_geomle.Wesparsa;

                    this.userControlRichTextOutput1.Texto = m_Wesparsa.EstatisticasDescritivas();

                    //teste da geração de matrizes de vizinhança 

                    //clsIpeaShape spteste = m_shape.Clone();

                    //cs.DefinicaoVizinhosFromMatrizEsparsa(ref spteste, this.m_Wesparsa);

                    //m_geomle.Shape = spteste;
                    //m_geomle.MatrizWesparsaFromVizinhosComPesos();

                    //clsMatrizEsparsa Wteste = m_geomle.Wesparsa;

                    //userControlRichTextOutput1.Texto += Wteste.EstatisticasDescritivas();

                    //int teste = Math.Max(1, (int)Math.Floor(tipo_vizinhanca / 2.0));
                    //cs.DefinicaoVizinhos(ref m_shape, teste, ref progressBar1);
                    //cs.AdicionaVizinhoProximo(ref m_shape);

                    //m_geomle.Shape = m_shape;
                    //m_geomle.MatrizWesparsaFromVizinhosNorm();

                    //this.m_Wesparsa = m_geomle.Wesparsa;

                    //this.userControlRichTextOutput1.Texto += m_Wesparsa.EstatisticasDescritivas();
                    
                    //fim do teste

                    this.lblProgressBar.Text = "Estrutura de vizinhança criada com sucesso";
                    progressBar1.Visible = false;
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
    }
}
