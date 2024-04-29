using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public partial class RegressaoGLM : Form
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

                this.Text = "Modelos Lineares Generalizados - " + value;
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

        public RegressaoGLM()
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
                Cursor = Cursors.WaitCursor;

                // preenche comboboxes 
                cmbDistribuicao.Items.Clear();
                cmbDistribuicao.Items.Add("Normal");
                cmbDistribuicao.Items.Add("Poisson");
                cmbDistribuicao.Items.Add("Bernoulli");
                cmbDistribuicao.Items.Add("Gamma");
                cmbDistribuicao.Items.Add("Geométrica");
                cmbDistribuicao.Items.Add("Binomial Negativa");
                //cmbDistribuicao.Items.Add("Gaussiana Inversa");
                cmbDistribuicao.SelectedIndex = 0;

                cmbFuncaoLigacao.Items.Add("Identidade");
                cmbFuncaoLigacao.Items.Add("Logaritmica");
                cmbFuncaoLigacao.Items.Add("Logit");
                cmbFuncaoLigacao.Items.Add("Probit");
                cmbFuncaoLigacao.Items.Add("Power");
                cmbFuncaoLigacao.Items.Add("Complementary log-log");
                cmbFuncaoLigacao.SelectedIndex = 0;

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

                numericUpDown1.Visible = false;
                label3.Visible = false;
                btnOK.Enabled = false;
                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                this.EstimaModelo();

                btnOK.Enabled = true;
                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Modelos Lineares Generalizados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private double[,] num_cat = new double[0,0];
        private void EstimaModelo()
        {
            try
            {
                if (ckbLimpaJanelaOutput.Checked)
                {
                    this.userControlRichTextOutput1.Texto = "";
                }

                if (ckbLimpaJanelaNovasVariaveis.Checked)
                {
                    this.userControlRichTextOutput2.Texto = "";
                }

                clsUtilTools clt = new clsUtilTools();

                BLogicRegressaoGLM blglm = new BLogicRegressaoGLM();

                blglm.VariaveisDependentes = new string[userControlRegressaoInstrumentos1.VariavelDependente.GetLength(0)];
                for (int i = 0; i < userControlRegressaoInstrumentos1.VariavelDependente.GetLength(0); i++)
                {
                    blglm.VariaveisDependentes[i] = userControlRegressaoInstrumentos1.VariavelDependente[i];
                }

                blglm.VariaveisIndependentes = userControlRegressaoInstrumentos1.VariaveisIndependentes;
                blglm.TabelaDados = this.m_dt_tabela_dados;
                blglm.AdicionaNovaVariaveis = ckbIncluirNovasVariaveisTabelaDados.Checked;
                blglm.IncluiIntercepto = ckbIncluiIntercepto.Checked;
                double[,] Y = clt.GetMatrizFromDataTable(m_dt_tabela_dados, blglm.VariaveisDependentes);
                string[] variavel_dependente = blglm.VariaveisDependentes;
                string variavel_dep_aux =  variavel_dependente[0];
                decimal m_k_binomialneg = numericUpDown1.Value;
                decimal m_alphapower = numericUpDown2.Value;

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

                if (blglm.ChecarMulticolinearidade(out mensagem_colinear))
                {
                    MessageBox.Show("Multicolinearidade dos regressores", mensagem_colinear + " Cheque a sua base de dados ou mude a especificação do modelo.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                #endregion

                switch (cmbDistribuicao.SelectedItem.ToString())
                {
                    case "Normal":
                        blglm.Distribuicao = BLogicRegressaoGLM.DistribuicoesGLM.Normal;
                        break;
                    case "Poisson":
                        blglm.Distribuicao = BLogicRegressaoGLM.DistribuicoesGLM.Poisson;

                        for (int i = 0; i < Y.GetLength(0); i++)
                        {
                            if (Y[i, 0] < 0)
                            {
                                MessageBox.Show("A variável dependente deve ser positiva.","Modelos Lineares Generalizados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }                        
                        }
                        break;

                    case "Bernoulli":
                        blglm.Distribuicao = BLogicRegressaoGLM.DistribuicoesGLM.Bernoulli;

                        clt.FrequencyTable(ref num_cat, Y);

                        if (num_cat.GetLength(0) != 2)
                        {
                            Cursor = Cursors.Default;
                            MessageBox.Show("O número de categorias da variável dependente é diferente de 2.", "Modelos Lineares Generalizados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        break;
                        
                    case "Gamma":
                        blglm.Distribuicao = BLogicRegressaoGLM.DistribuicoesGLM.Gamma;

                        for (int i = 0; i < Y.GetLength(0); i++)
                        {
                            if (Y[i, 0] < 0)
                            {
                                MessageBox.Show("A variável dependente deve ser positiva.", "Modelos Lineares Generalizados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        break;
                        
                    case "Geométrica":
                        blglm.Distribuicao = BLogicRegressaoGLM.DistribuicoesGLM.Geometrica;

                        for (int i = 0; i < Y.GetLength(0); i++)
                        {
                            if (Y[i, 0] < 0)
                            {
                                MessageBox.Show("A variável dependente deve ser positiva.", "Modelos Lineares Generalizados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                        }
                        break;
                        
                    case "Gaussiana Inversa":
                        blglm.Distribuicao = BLogicRegressaoGLM.DistribuicoesGLM.InverseGaussian;

                        for (int i = 0; i < Y.GetLength(0); i++)
                        {
                            if (Y[i, 0] < 0)
                            {
                                MessageBox.Show("A variável dependente deve ser positiva.", "Modelos Lineares Generalizados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        break;
                        
                    case "Binomial Negativa":
                        blglm.Distribuicao = BLogicRegressaoGLM.DistribuicoesGLM.BinomialNegativa;
                        for (int i = 0; i < Y.GetLength(0); i++)
                        {
                            if (Y[i, 0] < 0)
                            {
                                MessageBox.Show("A variável dependente deve ser positiva.", "Modelos Lineares Generalizados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        break;
                        
                    default:
                        break;
                }

                switch (cmbFuncaoLigacao.SelectedItem.ToString())
                {
                    case "Logaritmica":
                        blglm.FuncaoLigacaoModelo = BLogicRegressaoGLM.FuncoesLigacaoGLM.Logaritmica;
                        break;
                    case "Identidade":
                        blglm.FuncaoLigacaoModelo = BLogicRegressaoGLM.FuncoesLigacaoGLM.Identidade;
                        break;
                    case "Logit":
                        blglm.FuncaoLigacaoModelo = BLogicRegressaoGLM.FuncoesLigacaoGLM.Logit;
                        break;
                    case "Complementary log-log":
                        blglm.FuncaoLigacaoModelo = BLogicRegressaoGLM.FuncoesLigacaoGLM.ComplentaryLogLog;
                        break;
                    case "Power":
                        blglm.FuncaoLigacaoModelo = BLogicRegressaoGLM.FuncoesLigacaoGLM.Power;
                        break;
                    case "Binomial Negativa":
                        blglm.FuncaoLigacaoModelo = BLogicRegressaoGLM.FuncoesLigacaoGLM.BinomialNegativa;
                        break;
                    case "Probit":
                        blglm.FuncaoLigacaoModelo = BLogicRegressaoGLM.FuncoesLigacaoGLM.Probit;
                        break;
                    default:
                        blglm.FuncaoLigacaoModelo = BLogicRegressaoGLM.FuncoesLigacaoGLM.Identidade;
                        break;
                }

                bool estima_phi = false;
                if (ckbSobreDispersao.Checked) estima_phi = true;

                m_dt_tabela_dados = (DataTable)this.userControlDataGrid1.Datagridview.DataSource;
                double[,] residuos_brutos = new double[Y.GetLength(0), Y.GetLength(1)];
                IpeaGeo.Modelagem.BLogicGeracaoDummies blg = new IpeaGeo.Modelagem.BLogicGeracaoDummies();

                if (cmbDistribuicao.SelectedItem.ToString() == "Bernoulli")
                {
                    if (num_cat.GetLength(0) == 2)
                    {
                        blg.GerarDummies(ref m_dt_tabela_dados, variavel_dependente);

                        DialogResult disp = MessageBox.Show("Deseja utilizar a categoria " + num_cat[0, 0].ToString() + " como 'sucesso' (valor 1 na variável de Bernoulli)? Caso deseje usar a outra categoria selecione 'Não'.", "Modelos Lineares Generalizados", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (disp == DialogResult.Yes)
                        {
                            blglm.VariaveisDependentes[0] = variavel_dependente[0] + "_" + num_cat[0, 0].ToString();                        
                        }

                        else
                        {
                            blglm.VariaveisDependentes[0] = variavel_dependente[0] + "_" + num_cat[1, 0].ToString();
                        }
                    }
                }

                blglm.TabelaDados = this.m_dt_tabela_dados;

                blglm.EstimaModeloGLM(ref residuos_brutos, estima_phi, Convert.ToDouble(m_k_binomialneg), Convert.ToDouble(m_alphapower));

                this.userControlRichTextOutput1.Texto += blglm.ResultadoEstimacao + "\n\n";
                this.userControlRichTextOutput2.Texto = blglm.VariaveisGeradas + "\n\n";

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
                    amplitudehist = ((clt.Maxc(residuos_brutos)[0, 0]) - (clt.Minc(residuos_brutos)[0, 0]));
                    numclassesSturges = (int)(1.0 + 3.3 * Math.Log(residuos_brutos.GetLength(0)));

                    double incrementohist = (double)(amplitudehist / (double)numclassesSturges);
                    double[] Yhist = new double[numclassesSturges];
                    double[] Xhist = new double[numclassesSturges + 1];
                    for (int i = 0; i < Xhist.GetLength(0); i++)
                    {
                        Xhist[i] = ((clt.Minc(residuos_brutos)[0, 0]) + (incrementohist * ((double)i)));
                    }
                    double temp = new double();
                    
                    for (int i = 0; i < residuos_brutos.GetLength(0); i++)
                    {
                        for (int j = 0; j < numclassesSturges; j++)
                        {
                            temp = ((clt.Minc(residuos_brutos)[0, 0]) + (incrementohist * (((double)j) + 1.0)));
                            if (residuos_brutos[i, 0] <= temp)
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

                    zedGraphControl1.AxisChange();
                    zedGraphControl1.Update();
                    zedGraphControl1.Refresh();

                    if (!this.tabControl1.TabPages.Contains(tabPage3))
                    {
                        tabControl1.TabPages.Add(tabPage3);
                    }

                    #endregion

                    #region PxP Plot dos erros

                    if (cmbDistribuicao.SelectedItem.ToString() == "Normal")
                    {
                        //double mediaX = (clt.Meanc(erro))[0, 0];
                        //double varianX = clt.Varianciac(erro)[0, 0];
                        double[,] acumuladanormal = new double[residuos_brutos.GetLength(0), 1];
                        double[,] acumuladadados = new double[residuos_brutos.GetLength(0), 1];
                        double[] retaponto = new double[residuos_brutos.GetLength(0)];
                        retaponto[0] = 0;

                        //MathNormaldist norm = new MathNormaldist(mediaX, Math.Sqrt(varianX));
                        residuos_brutos = clt.SortcDoubleArray(residuos_brutos);

                        BLogicNonParametricTests blnonparametric = new BLogicNonParametricTests();
                        blnonparametric.PP_plot_1Variavel(residuos_brutos, out acumuladadados, out acumuladanormal);

                        for (int i = 0; i < residuos_brutos.GetLength(0); i++)
                        {
                            //acumuladanormal[i] = norm.cdf(erro[i, 0]);
                            //acumuladadados[i] = ((double)i / erro.GetLength(0));
                            listPP.Add(acumuladadados[i, 0], acumuladanormal[i, 0]);
                            if (i > 0)
                            {
                                retaponto[i] = (retaponto[i - 1]) + (1.0 / residuos_brutos.GetLength(0));
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
                    }
                    
                    /*
                    if (cmbDistribuicao.SelectedItem.ToString() == "Poisson")
                    {
                        PointPairList list1 = new PointPairList();
                        PointPairList reta = new PointPairList();                   

                        zedGraphControl1.GraphPane.CurveList.Clear();
                        zedGraphControl1.GraphPane.GraphObjList.Clear();
                        double mediaX = (clt.Meanc(residuos_brutos))[0, 0];
                        double varianX = clt.Varianciac(residuos_brutos)[0, 0];
                        double[,] qqpoissonv = new double[residuos_brutos.GetLength(0), 1];
                        double[,] qqdadosv = new double[residuos_brutos.GetLength(0), 1];
                        double[] qqpoisson = new double[residuos_brutos.GetLength(0)];
                        double[] qqdados = new double[residuos_brutos.GetLength(0)];
                        double[] retaponto = new double[residuos_brutos.GetLength(0)];
                        retaponto[0] = 0;

                        BLogicNonParametricTests bnp = new BLogicNonParametricTests();
                        MathPoissondist poisson = new MathPoissondist(mediaX);
                        //bnp.PP_plot_1Variavel(variavelpp, out qqdadosv, out  qqnormalv);

                        double[,] sdados = clt.SortcDoubleArray(residuos_brutos);

                        int n = sdados.GetLength(0);

                        double[,] emp_dist = new double[n, 1];
                        for (int i = 0; i < n; i++)
                        {
                            qqdadosv[i, 0] = ((double)i / (double)n);
                            qqpoissonv[i, 0] = poisson.cdf(sdados[i, 0]);
                        }

                        for (int i = 0; i < qqdadosv.GetLength(0); i++)
                        {
                            qqpoisson[i] = qqpoissonv[i, 0];
                            qqdados[i] = qqdadosv[i, 0];
                        }

                        list1.Add(qqdados, qqpoisson);

                        for (int i = 0; i < residuos_brutos.GetLength(0); i++)
                        {
                            if (i > 0)
                            {
                                retaponto[i] = (retaponto[i - 1]) + ((double)clt.Max(qqpoisson) / residuos_brutos.GetLength(0));
                            }
                        }

                        reta.Add(retaponto, retaponto);

                        LineItem myCurve2 = myPane2.AddCurve("Poisson", reta, Color.Black, SymbolType.None);
                        myCurve2.Line.IsVisible = true;

                        myPane2.YAxis.Scale.Min = (double)clt.Min(qqpoissonv) - (((double)clt.Max(qqpoissonv) - (double)clt.Min(qqpoissonv)) / (double)qqpoissonv.GetLength(0));
                        myPane2.YAxis.Scale.Max = (double)clt.Max(qqpoissonv) + (((double)clt.Max(qqpoissonv) - (double)clt.Min(qqpoissonv)) / (double)qqpoissonv.GetLength(0));
                        myPane2.XAxis.Scale.Min = (double)clt.Min(qqdadosv) - (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));
                        myPane2.XAxis.Scale.Max = (double)clt.Max(qqdadosv) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));

                        LineItem myCurve1 = myPane2.AddCurve("título", list1, Color.Blue, SymbolType.Circle);
                        myCurve1.Symbol.Fill.Color = Color.Blue;
                        myCurve1.Label.IsVisible = false;

                        myPane2.YAxis.Title.Text = "F(x) Poisson";
                        myPane2.XAxis.Title.Text = "F(x) Erro";
                        myPane2.Title.Text = "PxP Plot dos erros";
                    }*/

                    zedGraphControl1.AxisChange();

                    zedGraphControl1.Update();
                    zedGraphControl1.Refresh();

                    #endregion
                }

                #endregion

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

                if (blglm.AdicionaNovaVariaveis)
                {
                    this.userControlDataGrid1.TabelaDados = blglm.TabelaDados;
                }

                if (this.ckbMulticolinearidade.Checked)
                {
                    this.userControlRichTextOutput1.Texto += blglm.AnaliseMulticolinearidade();
                }
                else
                {
                    if (this.ckbApresentaCovMatrixBetaHat.Checked)
                        this.userControlRichTextOutput1.Texto += blglm.Analisedecorrelacao();
                }

                //deletar colunas criadas
                if (num_cat.GetLength(0) == 2 && cmbDistribuicao.SelectedItem.ToString() == "Bernoulli")
                {
                    m_dt_tabela_dados.Columns.Remove(variavel_dep_aux + "_" + num_cat[0, 0]);
                    m_dt_tabela_dados.Columns.Remove(variavel_dep_aux + "_" + num_cat[1, 0]);                     
                }
            }
            catch (Exception er)
            {
                if (num_cat.GetLength(0) == 2 && cmbDistribuicao.SelectedItem.ToString() == "Bernoulli")
                {
                    BLogicRegressaoGLM blglm = new BLogicRegressaoGLM();
                    string[] variavel_dependente = blglm.VariaveisDependentes;
                    string variavel_dep_aux = variavel_dependente[0];
                    m_dt_tabela_dados.Columns.Remove(variavel_dep_aux + "_" + num_cat[0, 0]);
                    m_dt_tabela_dados.Columns.Remove(variavel_dep_aux + "_" + num_cat[1, 0]);
                }
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Modelos Lineares Generalizados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        
        private void cmbDistribuicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDistribuicao.SelectedItem.ToString() == "Binomial Negativa")
                {
                    numericUpDown1.Visible = true;
                    label3.Visible = true;
                    label3.Text = "Parâmetro k de dispersão" + "\n" + "da Binomial Negativa";
                }
                else
                {
                    numericUpDown1.Visible = false;
                    label3.Visible = false;
                }               
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Modelos Lineares Generalizados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
            }
        }

        private void cmbFuncaoLigacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFuncaoLigacao.SelectedItem.ToString() == "Power")
            {
                numericUpDown2.Visible = true;
                label4.Visible = true;
            }
            else
            {
                numericUpDown2.Visible = false;
                label4.Visible = false;
            }
        }
    }
}
