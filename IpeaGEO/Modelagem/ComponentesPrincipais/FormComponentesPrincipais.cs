using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem.ComponentesPrincipais
{
    public partial class FormComponentesPrincipais : Form
    {
        BLogicComponentesPrincipais comprinc = new BLogicComponentesPrincipais();

        public FormComponentesPrincipais()
        {
            InitializeComponent();
        }

        #region variáveis internas

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

                this.Text = "Análise de Componentes Principais - " + value;
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

        #endregion

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
        }

        private void EstimaModelo()
        {
            GraphPane myPane = zedGraphControl1.GraphPane;
            clsUtilTools clt = new clsUtilTools();
            BLogicComponentesPrincipais blr = new BLogicComponentesPrincipais();

            zedGraphControl1.GraphPane.CurveList.Clear();

            this.userControlRichTextOutput1.Texto = "";

            //blr.CorrecaoWhite = ckbCorrecaoWhite.Checked;
            //blr.influentes = ckbObservacoesInfluente.Checked;
            //blr.VariaveisDependentes = userControlRegressaoInstrumentos1.VariavelDependente;
            blr.VariaveisIndependentes = userControlSelecaoVariaveis1.VariaveisIndependentes;
            blr.TabelaDados = this.m_dt_tabela_dados;
            blr.AdicionaNovaVariaveis = ckbIncluirNovasVariaveisTabelaDados.Checked;
            
            //if (ckbLimpaJanelaOutput.Checked)
            //{
            //    this.userControlRichTextOutput1.Texto = "";
            //}

            #region checando valores inválidos

            int[] indicadores_val_invalidos;


            
            //for (int i = 0; i < userControlSelecaoVariaveis1.VariaveisIndependentes.GetLength(0); i++)
            //{
            //    if (clt.ChecaValoresDoubleInvalidos(this.m_dt_tabela_dados, userControlSelecaoVariaveis1.VariaveisIndependentes[i], out indicadores_val_invalidos))
            //    {
            //        MessageBox.Show("Há valores double inválidos na variável " + userControlSelecaoVariaveis1.VariaveisIndependentes[i] + ". Cheque a sua base de dados.");
            //        return;
            //    }
            //}

            if (userControlSelecaoVariaveis1.VariaveisIndependentes.GetLength(0) < 2)
            {
                throw new Exception("Escolha mais de uma variável.");
            }
            if (numericUpDown1.Value > userControlSelecaoVariaveis1.VariaveisIndependentes.GetLength(0))
            {
                throw new Exception("O número de componentes deve ser menor ou igual ao número de variáveis.");
            }
           
            
            #endregion

            double[] perc = new double[0];

            blr.cmbTIPOMatz = cmbtipomatriz.SelectedItem.ToString();
            blr.ckbvarcovar = ckbapresentavarcovar.Checked;
            blr.ckbcorrel = ckbapresentacorrelacao.Checked;
            blr.ckbcorrelvarcomp = ckbcorrelcompvar.Checked;
            blr.ckbescore  = ckbescorecomponentes.Checked;
            blr.AdicionaNovaVariaveis = ckbIncluirNovasVariaveisTabelaDados.Checked;
            blr.ckbbartlet = ckbBartlet.Checked;
            blr.numcomponentes = Convert.ToInt32(numericUpDown1.Value);

            blr.EstimarComponentesPrincipais(ref perc);

            if (!this.tabControl1.TabPages.Contains(tabPage2))
            {
                tabControl1.TabPages.Add(tabPage2);
            }
            tabControl1.SelectedTab = tabPage2;
            
            if (!this.tabControl1.TabPages.Contains(tabPage3))
            {
                tabControl1.TabPages.Add(tabPage3);
            }

            if (!this.tabControl1.TabPages.Contains(tabPage4))
            {
                if (ckbescorecomponentes.Checked)
                {
                    tabControl1.TabPages.Add(tabPage4);
                }
            }
            
            if (!ckbescorecomponentes.Checked)
            {
                tabControl1.TabPages.Remove(tabPage4);
            }           

            if (ckbescorecomponentes.Checked)
            { 
            this.userControlRichTextOutput2.Texto = blr.VariaveisGeradas;
            }

            #region Scree PLot

            
            // Set the Titles
            myPane.Title.Text = "Scree Plot";
            myPane.XAxis.Title.Text = "Autovalores";
            myPane.YAxis.Title.Text = "Percentual de Explicação da Variância";
            //myPane.XAxis.Type = AxisType.Text;
            myPane.YAxis.Scale.Min = 0;
            myPane.YAxis.Scale.Max = 1;
            myPane.XAxis.Scale.Min = 0;
            

            // Make up some data arrays based on the Sine function
            double x, y;
            double[] X = new double[perc.GetLength(0)];

            PointPairList list1 = new PointPairList();
            

            for (int i = 0; i < perc.GetLength(0); i++)
            {

                X[i] = i + 1;
                list1.Add(X[i], perc[i]);
            }

            

            // Generate a blue curve with circle
            // symbols, and "Piper" in the legend
            LineItem myCurve1 = myPane.AddCurve("Autovalor", list1, Color.Blue, SymbolType.Circle);

            // Fill the area under the curves
            myCurve1.Line.Fill = new Fill(Color.White, Color.Blue, 45F);

            //// Add a background gradient fill to the axis frame
            //myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 210), -45F);


            // Tell ZedGraph to refigure the
            // axes since the data have changed
            zedGraphControl1.AxisChange();

            zedGraphControl1.Update();
            zedGraphControl1.Refresh();


            #endregion
            
            this.userControlRichTextOutput1.Texto += blr.ResultadoEstimacao + "\n\n";
            //this.userControlRichTextOutput2.Texto = blr.VariaveisGeradas + "\n\n";

            //if (!this.tabControl1.TabPages.Contains(tabPage2))
            //{
            //    tabControl1.TabPages.Add(tabPage2);
            //}
            //tabControl1.SelectedTab = tabPage2;
            
            if (blr.AdicionaNovaVariaveis)
            {
                this.userControlDataGrid1.TabelaDados = blr.TabelaDados;
            }
        }

        private void btnExecutar_Click(object sender, EventArgs e)
        {
             try
            {
                Cursor = Cursors.WaitCursor;

                numericUpDown1.Maximum = userControlSelecaoVariaveis1.VariaveisIndependentes.GetLength(0);

                this.EstimaModelo();

                this.lblProgressBar.Text = "Cálculos efetuados com sucesso";
                this.lblProgressBar.Visible = true;

                Cursor = Cursors.Default;
            }
            catch (Exception er)
             {
                this.lblProgressBar.Text = "Houve problemas nos cálculos para os componentes principais";
                this.lblProgressBar.Visible = true;

                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Componentes Principais", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }             
        }

        private void FormComponentesPrincipais_Load(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = false;

            userControlSelecaoVariaveis1.LabelListBoxEsquerda = "Variáveis numéricas na tabela";
            userControlSelecaoVariaveis1.LabelListBoxDireita = "Variáveis selecionadas";
            userControlSelecaoVariaveis1.PermiteSelecaoMultipla = true;

            // Variáveis sendo passadas para o UserControl
            userControlDataGrid1.TabelaDados = this.m_dt_tabela_dados;
            userControlDataGrid1.FuncaoFromFormulario = new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionFromFormulario(this.AtualizaTabelaDados);
            userControlDataGrid1.MostraOpcaoImportadaoDados = true;
            userControlDataGrid1.UserControlSelecaoVariaveis = this.userControlSelecaoVariaveis1;

            cmbtipomatriz.Text = "Matriz de Variância e Covariância";

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

        private void ckbescorecomponentes_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbescorecomponentes.Checked)
            {
                ckbIncluirNovasVariaveisTabelaDados.Enabled = true;
            }
            else 
            {
                ckbIncluirNovasVariaveisTabelaDados.Enabled = false;
                ckbIncluirNovasVariaveisTabelaDados.Checked = false;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (userControlSelecaoVariaveis1.VariaveisIndependentes.GetLength(0) > 0)
            {
                numericUpDown1.Maximum = userControlSelecaoVariaveis1.VariaveisIndependentes.GetLength(0);                
            }
        }

        private void userControlSelecaoVariaveis1_Load(object sender, EventArgs e)
        {
            try
            {
                numericUpDown1.Refresh();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                    MessageBox.Show("Selecione a opção 'Mostrar escores na tabela de dados', localizada na aba Especificações", "Atualização", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ckbIncluirNovasVariaveisTabelaDados_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbIncluirNovasVariaveisTabelaDados.Checked) numericUpDown1.Enabled = true;
            else numericUpDown1.Enabled = false;
        }
    }
}
