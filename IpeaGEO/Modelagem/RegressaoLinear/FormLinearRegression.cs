using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public partial class FormLinearRegression : Form
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

                this.userControlDataGrid1.HabilitarImportacaoDados = false;
            }
        }

        private DataTable m_dt_tabela_shape = new DataTable();
        public DataTable DadosShape
        {
            get { return m_dt_tabela_shape; }
            set { m_dt_tabela_shape = value; }
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

        public FormLinearRegression()
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

                if (tabControl1.TabPages.Contains(tabPage5))
                {
                    tabControl1.TabPages.Remove(tabPage5);
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
            BLogicRegressaoLinear blr = new BLogicRegressaoLinear();

            m_dt_tabela_dados =  (DataTable)this.userControlDataGrid1.Datagridview.DataSource;

            blr.CorrecaoWhite = ckbCorrecaoWhite.Checked;
            blr.influentes = ckbObservacoesInfluente.Checked;
            blr.VariaveisDependentes = userControlRegressaoInstrumentos1.VariavelDependente;
            blr.VariaveisIndependentes = userControlRegressaoInstrumentos1.VariaveisIndependentes;
            blr.TabelaDados = this.m_dt_tabela_dados;
            blr.AdicionaNovaVariaveis = ckbIncluirNovasVariaveisTabelaDados.Checked;
            blr.intercepto = ckbIncluiIntercepto.Checked;
                                  
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

            if (userControlRegressaoInstrumentos1.VariavelDependente.GetLength(0) <= 0)
                throw new Exception("Escolha uma variável dependente.");

            if (userControlRegressaoInstrumentos1.VariaveisIndependentes.GetLength(0) <= 0)
                throw new Exception("Escolha pelo menos uma variável independente.");
            
            #endregion

            #region checando multicolinearidade perfeita das regressores

            string mensagem_colinear;

            if (blr.ChecarMulticolinearidade(out mensagem_colinear))
            {
                MessageBox.Show(/*"Cheque a sua base de dados ou mude a especificação do modelo.",*/ mensagem_colinear, "Multicolinearidade dos regressores", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #endregion

            double[,] X = clt.GetMatrizFromDataTable(m_dt_tabela_dados, blr.VariaveisDependentes);
            double[,] erro = new double[X.GetLength(0),X.GetLength(1)];

            blr.EstimarModeloRegressao(ref erro);
            
            #region Gráficos

            GraphPane myPane = zedGraphControl1.GraphPane;
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();


            GraphPane myPane2 = zedGraphControl2.GraphPane;
            PointPairList listPP = new PointPairList();
            PointPairList retaPP = new PointPairList();
            zedGraphControl2.GraphPane.CurveList.Clear();
            zedGraphControl2.GraphPane.GraphObjList.Clear();


            if (ckbAnaliseGrafica.Checked)
            {
                #region Histograma dos Erros
                //Calcula o número de classes segundo Regra de Sturges numclass = 1 + 3.3 log(n)
                int numclassesSturges = new int();
                double amplitudehist = new double();
                amplitudehist = ((clt.Maxc(erro)[0, 0]) - (clt.Minc(erro)[0, 0]));
                numclassesSturges = (int)(1.0 + 3.3 * Math.Log(erro.GetLength(0)));

                double incrementohist = (double)(amplitudehist / (double)numclassesSturges);
                double[] Yhist = new double[numclassesSturges];
                double[] Xhist = new double[numclassesSturges + 1];
                for (int i = 0; i < Xhist.GetLength(0); i++)
                {
                    Xhist[i] = ((clt.Minc(erro)[0, 0]) + (incrementohist * ((double)i)));
                }
                double temp = new double();
                for (int i = 0; i < erro.GetLength(0); i++)
                {
                    for (int j = 0; j < numclassesSturges; j++)
                    {
                        temp = ((clt.Minc(erro)[0, 0]) + (incrementohist * (((double)j) + 1.0)));
                        if (erro[i, 0] <= temp)
                        {
                            Yhist[j] += 1;
                            break;
                        }
                    }
                }

                for (int i = 0; i < Xhist.GetLength(0) - 1; i++)
                {
                    BoxObj box = new BoxObj(Xhist[i], Yhist[i], (double)(Xhist[i + 1] - Xhist[i]), Yhist[i]);
                    box.IsClippedToChartRect = true;
                    box.Fill.Color = Color.Beige;
                    myPane.GraphObjList.Add(box);
                    myPane.YAxis.Scale.Min = (double)clt.Min(Yhist) - (((double)clt.Max(Yhist) - (double)clt.Min(Yhist)) / (double)Yhist.GetLength(0));
                    myPane.YAxis.Scale.Max = (double)clt.Max(Yhist) + (((double)clt.Max(Yhist) - (double)clt.Min(Yhist)) / (double)Yhist.GetLength(0));
                    myPane.XAxis.Scale.Min = (double)clt.Min(Xhist) - (((double)clt.Max(Xhist) - (double)clt.Min(Xhist)) / (double)Xhist.GetLength(0));
                    myPane.XAxis.Scale.Max = (double)clt.Max(Xhist) + (((double)clt.Max(Xhist) - (double)clt.Min(Xhist)) / (double)Xhist.GetLength(0));
                    
                }

                

                
                myPane.Title.Text = "Histograma dos Erros";
                myPane.XAxis.Title.Text = "Resíduos";

                

                if (!this.tabControl1.TabPages.Contains(tabPage3))
                {
                    tabControl1.TabPages.Add(tabPage3);
                }

                #endregion

                #region PxP Plot dos erros

                    //double mediaX = (clt.Meanc(erro))[0, 0];
                    //double varianX = clt.Varianciac(erro)[0, 0];
                    double[,] acumuladanormal = new double[erro.GetLength(0),1];
                    double[,] acumuladadados = new double[erro.GetLength(0),1];
                    double[] retaponto = new double[erro.GetLength(0)];
                    retaponto[0] = 0;


                    //MathNormaldist norm = new MathNormaldist(mediaX, Math.Sqrt(varianX));
                    erro = clt.SortcDoubleArray(erro);

                    BLogicNonParametricTests blnonparametric = new BLogicNonParametricTests();
                    blnonparametric.PP_plot_1Variavel(erro,out acumuladadados,out acumuladanormal);

                    for (int i = 0; i < erro.GetLength(0); i++)
                    {
                        //acumuladanormal[i] = norm.cdf(erro[i, 0]);
                        //acumuladadados[i] = ((double)i / erro.GetLength(0));
                        listPP.Add(acumuladadados[i,0], acumuladanormal[i,0]);
                        if (i > 0)
                        {
                            retaponto[i] = (retaponto[i - 1]) + (1.0 / erro.GetLength(0));                            
                        }
                    }
                    retaPP.Add(retaponto, retaponto);
                    LineItem myCurve2 = myPane2.AddCurve("Normal", retaPP, Color.Black, SymbolType.None);
                    myCurve2.Line.IsVisible = true;

                    LineItem myCurve1 = myPane2.AddCurve("tituloo", listPP, Color.Blue, SymbolType.Circle);
                    myCurve1.Symbol.Fill.Color = Color.Blue;
                    myCurve1.Label.IsVisible = false;


                    myPane2.YAxis.Scale.Min = 0.0;
                    myPane2.YAxis.Scale.Max = 1.0;
                    myPane2.XAxis.Scale.Min = 0.0;
                    myPane2.XAxis.Scale.Max = 1.0;
                    myPane2.XAxis.Title.Text = "F(x) Erro";
                    myPane2.YAxis.Title.Text = "F(x) Normal";
                    myPane2.Title.Text = "PxP Plot dos erros";

                    if (!this.tabControl1.TabPages.Contains(tabPage5))
                    {
                        tabControl1.TabPages.Add(tabPage5);
                    }

                    zedGraphControl1.AxisChange();

                    zedGraphControl1.Update();
                    zedGraphControl1.Refresh();  

            #endregion


            }


            #endregion
            
            this.userControlRichTextOutput1.Texto += blr.ResultadoEstimacao + "\n\n";
            this.userControlRichTextOutput2.Texto = blr.VariaveisGeradas + "\n\n";

            if (!this.tabControl1.TabPages.Contains(tabPage2))
            {
                tabControl1.TabPages.Add(tabPage2);
            }
            tabControl1.SelectedTab = tabPage2;
            
            if (ckbAnaliseResiduos.Checked)
            {
                if (!this.tabControl1.TabPages.Contains(tabPage4))
                {
                    tabControl1.TabPages.Add(tabPage4);
                }
            }
            else
            {
                tabControl1.TabPages.Remove(tabPage4);
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
                //this.dataGridView1.DataSource = blr.TabelaDados;
                this.userControlDataGrid1.TabelaDados = blr.TabelaDados;
            }
        }

        private void ckbObservacoesInfluente_CheckedChanged(object sender, EventArgs e)
        {            
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

        private void ckbApresentaCovMatrixBetaHat_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void userControlRegressaoInstrumentos1_Load(object sender, EventArgs e)
        {
        }

        private void ckbCorrecaoWhite_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void userControlRichTextOutput1_Load(object sender, EventArgs e)
        {

        }
    }
}
