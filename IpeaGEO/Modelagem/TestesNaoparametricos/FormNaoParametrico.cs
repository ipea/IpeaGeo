using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using IpeaGeo.RegressoesEspaciais;
using MathNet.Numerics.Distributions;
using ZedGraph;

namespace IpeaGeo.Modelagem
{
    public partial class FormNonParametric : FormStatisticalAnalysis
    {
        #region Variáveis internas

        public override void HabilitarDadosExternos()
        {
            btnOK.Visible = btnOK.Enabled = true;
        }

        public new string LabelTabelaDados
        {
            set
            {
                m_label_tabela_dados = value;
                this.Text = "Testes Não paramétricos - " + value;
            }
        }

        public new DataTable TabelaDeDados
        {
            get { return m_dt_tabela_dados; }
            set
            {
                m_dt_tabela_dados = value;
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

        public FormNonParametric()
        {
            InitializeComponent();
        }

        #region Atualiza a tabela de dados

        private void AtualizaTabelaDados(bool atualizaUControl)
        {
            if (atualizaUControl && m_dt_tabela_dados.Columns.Count > 0 && m_dt_tabela_dados.Rows.Count > 0)
                this.userControlDataGrid1.TabelaDados = m_dt_tabela_dados;
            else if (this.userControlDataGrid1.TabelaDados.Columns.Count > 0 && this.userControlDataGrid1.TabelaDados.Rows.Count > 0)
                m_dt_tabela_dados = this.userControlDataGrid1.TabelaDados;

            clsUtilTools clt = new clsUtilTools();
            if (m_dt_tabela_dados.Columns.Count > 0)
            {
                string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);
                this.userControlSelecaoDoisBlocosVariaveis1.ZeraControle();
                this.userControlSelecaoDoisBlocosVariaveis1.VariaveisDB = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
                this.userControlSelecaoDoisBlocosVariaveis1.VariaveisList = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
            } // if

            if (!this.tabControl1.TabPages.Contains(tabPage0))
                tabControl1.TabPages.Add(tabPage0);

            this.tabControl1.SelectedTab = tabPage0;
        } // AtualizaTabelaDados()

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        } // btnClose_Click()

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.DataSetExterno.Tables.Clear();
                lblProgressBar.Text = "Tabela atualizada no formulário de mapas";
            } // try
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // btnOk_Click()

        private void FormBaseModelagem_Load(object sender, EventArgs e)
        {
            try
            {
                //==================== Variáveis sendo passadas para o UserControl

                userControlDataGrid1.TabelaDados = this.m_dt_tabela_dados;
                userControlDataGrid1.FuncaoFromFormulario = new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionFromFormulario(this.AtualizaTabelaDados);
                userControlDataGrid1.MostraOpcaoImportadaoDados = true;
                userControlDataGrid1.UserControlSelecao2BlocosVariaveis = this.userControlSelecaoDoisBlocosVariaveis1;


                this.userControlSelecaoDoisBlocosVariaveis1.SelecaoInstrumentosDisponivel = true;

                AtualizaTabelaDados(this.userControlSelecaoDoisBlocosVariaveis1.SelecaoInstrumentosDisponivel = true);

                this.userControlSelecaoDoisBlocosVariaveis1.LabelVariaveisBlocoSuperior = "Variáveis para cálculo dos Testes";
                this.userControlSelecaoDoisBlocosVariaveis1.LabelVariaveisBlocoInferior = "Variáveis de definição de classes ou fator";

                if (tabControl1.TabPages.Contains(tabPage2))
                    tabControl1.TabPages.Remove(tabPage2);

                if (tabControl1.TabPages.Contains(tabPage3))
                    tabControl1.TabPages.Remove(tabPage3);

                if (tabControl1.TabPages.Contains(tabPage4))
                    tabControl1.TabPages.Remove(tabPage4);

                if (m_dt_tabela_dados.Rows.Count <= 0 || m_dt_tabela_dados.Columns.Count <= 0)
                    if (tabControl1.TabPages.Contains(tabPage0))
                        tabControl1.TabPages.Remove(tabPage0);

                cmbTipoDist.Text = "Normal";
            } // try
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // FormBaseModelagem_Load()

        #region toolstrips menus

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblProgressBar.Text = "Importando arquivo em formato shape";

                clsUtilArquivos cla = new clsUtilArquivos();
                cla.ImportarArquivoShape(ref m_shape, ref m_dt_tabela_shape);

                this.lblProgressBar.Text = "Arquivo shape importado com sucesso";
            } // try
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // toolStripMenuItem4_Click()

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                clsUtilArquivos cla = new clsUtilArquivos();
                if (cla.ImportarTabelaDados(ref m_dt_tabela_dados))
                    AtualizaTabelaDados(true);
            } // try
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // toolStripMenuItem5_Click()

        private void exportarDadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                clsUtilArquivos cla = new clsUtilArquivos();
            } // try
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // exportarDadosToolStripMenuItem_Click()

        private void calculadoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormCalculadora frm = new IpeaGeo.RegressoesEspaciais.FormCalculadora();
                    frm.MdiParent = this.MdiParent;
                    frm.Dados = this.m_dt_tabela_dados;

                    if (frm.ShowDialog() == DialogResult.OK)
                        this.m_dt_tabela_dados = frm.Dados;
                } // if
                else MessageBox.Show("Tabela de dados está vazia.");
            } // try
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // calculadoraToolStripMenuItem_Click()

        private void excluirVariáveisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormCalculadora frm = new IpeaGeo.RegressoesEspaciais.FormCalculadora();
                    frm.MdiParent = this.MdiParent;
                    frm.Dados = this.m_dt_tabela_dados;
                    frm.AtivaExclusaoVariaveis = true;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.m_dt_tabela_dados = frm.Dados;
                        this.AtualizaTabelaDados(true);
                    } // if
                } // if
                else MessageBox.Show("Tabela de dados está vazia");
            } // try
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // excluirVariáveisToolStripMenuItem_Click()

        private void estatísticasDescritivasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormEstatisticasDescritivas frm = new IpeaGeo.RegressoesEspaciais.FormEstatisticasDescritivas();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.m_dt_tabela_dados;
                    frm.Show();
                } // if
                else MessageBox.Show("Tabela de dados está vazia.");
            } // try
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // estatísticasDescritivasToolStripMenuItem_Click()

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormTabelaFrequencias frm = new IpeaGeo.RegressoesEspaciais.FormTabelaFrequencias();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.m_dt_tabela_dados;
                    frm.Show();
                } // if
                else MessageBox.Show("Tabela de dados está vazia.");
            } // try
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // toolStripMenuItem9_Click()

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormTabelasCruzadas frm = new IpeaGeo.RegressoesEspaciais.FormTabelasCruzadas();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.m_dt_tabela_dados;
                    frm.Show();
                } // if
                else MessageBox.Show("Tabela de dados está vazia.");
            } //try
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } //catch
        } // toolStripMenuItem10_Click()

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormCorrelacoes frm = new IpeaGeo.RegressoesEspaciais.FormCorrelacoes();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.m_dt_tabela_dados;
                    frm.Show();
                } // if
                else MessageBox.Show("Tabela de dados está vazia.");
            } // try
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // toolStripMenuItem11_Click()

        #endregion

        #region Executa testes não-paramétricos

        private static string tipodist_;
        public string tipodist = tipodist_;
        private void ExecutaTestes()
        {            
            clsUtilTools clt = new clsUtilTools();
            BLogicNonParametricTests blnp = new BLogicNonParametricTests();
            BLogicBaseModelagem blbm = new BLogicBaseModelagem();
            
            blnp.VariaveisDependentes = userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior;
            blnp.TabelaDados = this.m_dt_tabela_dados;
            blnp.VariaveisIndependentes = userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoInferior;
            blnp.TabelaDados = this.m_dt_tabela_dados;

            double[,] dados = clt.GetMatrizFromDataTable(m_dt_tabela_dados, userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior);
            double[,] dados1 = clt.GetMatrizFromDataTable(m_dt_tabela_dados, userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoInferior);

            double pvalor_ks1 = 0.0;
            double tstat_ks1 = 0.0;
            double[] pvalor_ks = new double[dados1.GetLength(1)];
            double[] tstat_ks = new double[dados1.GetLength(1)];
            double[,] tstat_ks2 = new double[0, 0];
            double[,] pvalor_ks2 = new double[0, 0];
            double[,] par1 = new double[0, 0];
            double[,] par2 = new double[0, 0];
            double par11 = 0.0;
            double par22 = 0.0;
            double verificacao = 0.0;

            double[,] cdf_emp = new double[0, 0];
            double[,] cdf_teorica = new double[0, 0];
            double[,] sorted_dados = new double[0, 0];
            double[,] inv_cdf = new double[0, 0];
            double[,] tabela = new double[0, 0];
            object[,] tabelapoisson = new object[0, 0];

            double posto_mw = 0.0;
            double estat_mw = 0.0;
            double dp_mw = 0.0;
            double pvalue_mw = 0.0;
            double estat_kw = 0.0;
            double estat_kw_corrigida = 0.0;
            double pvalue_kw = 0.0;
            double[,] diferenca_sig = new double[0, 0];
            double estat_wil = 0.0;
            double pvalue_wil = 0.0;
            bool ver_empates = false;
           
            string[] nomes = new string[0];
            double[] vetorsomadequadrados = new double[0];
            double[] vetorgl = new double[0];
            string[] vetorqdmedio = new string[0];
            string[] vetorF = new string[0];
            string[] vetorpvalue = new string[0];
            double[,] tabclasse = new double[0, 0];

            if (cmbTipoDist.Text == "Normal")
                blnp.TipoDistribuicao = TipoDistribuicao.Normal;

            if (cmbTipoDist.Text == "Exponencial")
                blnp.TipoDistribuicao = TipoDistribuicao.Exponencial;

            if (cmbTipoDist.Text == "Uniforme")
                blnp.TipoDistribuicao = TipoDistribuicao.Uniform;

            if (cmbTipoDist.Text == "Poisson")
                blnp.TipoDistribuicao = TipoDistribuicao.Poisson;
         
            if (ckbPPplot.Checked == true)
            {
                tabControl1.TabPages.Remove(tabPage3);
                GraphPane myPane = zedGraphControl1.GraphPane;
                PointPairList list1 = new PointPairList();
                PointPairList reta = new PointPairList();
                tabControl1.TabPages.Add(tabPage3);

                zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl1.GraphPane.GraphObjList.Clear();

                if (cmbTipoDist.Text == "Normal")
                {
                    double[,] variavelpp = clt.GetMatrizFromDataTable(m_dt_tabela_dados, userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior);
                    double mediaX = (clt.Meanc(variavelpp))[0, 0];
                    double varianX = clt.Varianciac(variavelpp)[0, 0];
                    double[,] qqnormalv = new double[variavelpp.GetLength(0), 1];
                    double[,] qqdadosv = new double[variavelpp.GetLength(0), 1];
                    double[] qqnormal = new double[variavelpp.GetLength(0)];
                    double[] qqdados = new double[variavelpp.GetLength(0)];
                    double[] retaponto = new double[variavelpp.GetLength(0)];
                    retaponto[0] = 0;

                    BLogicNonParametricTests bnp = new BLogicNonParametricTests();
                    Normal norm = new Normal(mediaX, Math.Sqrt(varianX));
                    double[,] sdados = clt.SortcDoubleArray(variavelpp);

                    int n = sdados.GetLength(0);

                    double[,] emp_dist = new double[n, 1];
                    for (int i = 0; i < n; i++)
                    {
                        qqdadosv[i, 0] = ((double)i / (double)n);
                        qqnormalv[i, 0] = norm.CumulativeDistribution(sdados[i, 0]);
                    } // for

                    for (int i = 0; i < qqdadosv.GetLength(0); i++)
                    {
                        qqnormal[i] = qqnormalv[i, 0];
                        qqdados[i] = qqdadosv[i, 0];
                    } //for

                    list1.Add(qqdados, qqnormal);

                    for (int i = 0; i < variavelpp.GetLength(0); i++)
                        if (i > 0)
                            retaponto[i] = (retaponto[i - 1]) + ((double)clt.Max(qqnormal) / variavelpp.GetLength(0));

                    reta.Add(retaponto, retaponto);

                    LineItem myCurve2 = myPane.AddCurve("Normal", reta, Color.Black, SymbolType.None);
                    myCurve2.Line.IsVisible = true;

                    myPane.YAxis.Scale.Min = (double)clt.Min(qqnormalv) - (((double)clt.Max(qqnormalv) - (double)clt.Min(qqnormalv)) / (double)qqnormalv.GetLength(0));
                    myPane.YAxis.Scale.Max = (double)clt.Max(qqnormalv) + (((double)clt.Max(qqnormalv) - (double)clt.Min(qqnormalv)) / (double)qqnormalv.GetLength(0));
                    myPane.XAxis.Scale.Min = (double)clt.Min(qqdadosv) - (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));
                    myPane.XAxis.Scale.Max = (double)clt.Max(qqdadosv) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));

                    LineItem myCurve1 = myPane.AddCurve("titulo", list1, Color.Blue, SymbolType.Circle);
                    myCurve1.Symbol.Fill.Color = Color.Blue;
                    myCurve1.Label.IsVisible = false;

                    myPane.YAxis.Title.Text = "";
                    myPane.XAxis.Title.Text = Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                    myPane.Title.Text = "PP-Plot da variável " + Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                } // if

                if (cmbTipoDist.Text == "Exponencial")
                {
                    double[,] variavelpp = clt.GetMatrizFromDataTable(m_dt_tabela_dados, userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior);
                    double mediaX = (clt.Meanc(variavelpp))[0, 0];
                    double varianX = clt.Varianciac(variavelpp)[0, 0];
                    double[,] qqexponv = new double[variavelpp.GetLength(0), 1];
                    double[,] qqdadosv = new double[variavelpp.GetLength(0), 1];
                    double[] qqexpon = new double[variavelpp.GetLength(0)];
                    double[] qqdados = new double[variavelpp.GetLength(0)];
                    double[] retaponto = new double[variavelpp.GetLength(0)];
                    retaponto[0] = 0;

                    BLogicNonParametricTests bnp = new BLogicNonParametricTests();
                    Exponential exp = new Exponential((1 / mediaX));

                    double[,] sdados = clt.SortcDoubleArray(variavelpp);
                    int n = sdados.GetLength(0);

                    double[,] emp_dist = new double[n, 1];
                    for (int i = 0; i < n; i++)
                    {
                        qqdadosv[i, 0] = ((double)i / (double)n);
                        qqexponv[i, 0] = exp.CumulativeDistribution(sdados[i, 0]);
                    } // for

                    for (int i = 0; i < qqdadosv.GetLength(0); i++)
                    {
                        qqexpon[i] = qqexponv[i, 0];
                        qqdados[i] = qqdadosv[i, 0];
                    } // for

                    list1.Add(qqdados, qqexpon);

                    for (int i = 0; i < variavelpp.GetLength(0); i++)
                        if (i > 0)
                            retaponto[i] = (retaponto[i - 1]) + ((double)clt.Max(qqexpon) / variavelpp.GetLength(0));

                    reta.Add(retaponto, retaponto);

                    LineItem myCurve2 = myPane.AddCurve("Exponencial", reta, Color.Black, SymbolType.None);
                    myCurve2.Line.IsVisible = true;

                    myPane.YAxis.Scale.Min = (double)clt.Min(qqexponv) - (((double)clt.Max(qqexponv) - (double)clt.Min(qqexponv)) / (double)qqexponv.GetLength(0));
                    myPane.YAxis.Scale.Max = (double)clt.Max(qqexponv) + (((double)clt.Max(qqexponv) - (double)clt.Min(qqexponv)) / (double)qqexponv.GetLength(0));
                    myPane.XAxis.Scale.Min = (double)clt.Min(qqdadosv) - (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));
                    myPane.XAxis.Scale.Max = (double)clt.Max(qqdadosv) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));

                    LineItem myCurve1 = myPane.AddCurve("tituloo", list1, Color.Blue, SymbolType.Circle);
                    myCurve1.Symbol.Fill.Color = Color.Blue;
                    myCurve1.Label.IsVisible = false;

                    myPane.YAxis.Title.Text = "";
                    myPane.XAxis.Title.Text = Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                    myPane.Title.Text = "PP-Plot da variável " + Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                } // if

                if (cmbTipoDist.Text == "Uniforme")
                {
                    double[,] variavelpp = clt.GetMatrizFromDataTable(m_dt_tabela_dados, userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior);
                    double mediaX = (clt.Meanc(variavelpp))[0, 0];
                    double varianX = clt.Varianciac(variavelpp)[0, 0];
                    double[,] qqunifv = new double[variavelpp.GetLength(0), 1];
                    double[,] qqdadosv = new double[variavelpp.GetLength(0), 1];
                    double[] qqunif = new double[variavelpp.GetLength(0)];
                    double[] qqdados = new double[variavelpp.GetLength(0)];
                    double[] retaponto = new double[variavelpp.GetLength(0)];
                    retaponto[0] = 0;

                    BLogicNonParametricTests bnp = new BLogicNonParametricTests();

                    double[,] sdados = clt.SortcDoubleArray(variavelpp);

                    int n = sdados.GetLength(0);
                    double[] vargrafico = new double[variavelpp.GetLength(0)];

                    for (int i = 0; i < variavelpp.GetLength(0); i++)
                        vargrafico[i] = variavelpp[i, 0];

                    double a = clt.Min(vargrafico);
                    double b = clt.Max(vargrafico);
                    double dif = b - a;

                    double[,] emp_dist = new double[n, 1];
                    for (int i = 0; i < n; i++)
                    {
                        qqdadosv[i, 0] = ((double)i / (double)n);
                        qqunifv[i, 0] = (sdados[i, 0] - a) / dif;
                    } // for

                    for (int i = 0; i < qqdadosv.GetLength(0); i++)
                    {
                        qqunif[i] = qqunifv[i, 0];
                        qqdados[i] = qqdadosv[i, 0];
                    } // for

                    list1.Add(qqdados, qqunif);

                    for (int i = 0; i < variavelpp.GetLength(0); i++)
                        if (i > 0)
                            retaponto[i] = (retaponto[i - 1]) + ((double)clt.Max(qqunif) / variavelpp.GetLength(0));

                    reta.Add(retaponto, retaponto);

                    LineItem myCurve2 = myPane.AddCurve("Uniforme", reta, Color.Black, SymbolType.None);
                    myCurve2.Line.IsVisible = true;

                    myPane.YAxis.Scale.Min = (double)clt.Min(qqunifv) - (((double)clt.Max(qqunifv) - (double)clt.Min(qqunifv)) / (double)qqunifv.GetLength(0));
                    myPane.YAxis.Scale.Max = (double)clt.Max(qqunifv) + (((double)clt.Max(qqunifv) - (double)clt.Min(qqunifv)) / (double)qqunifv.GetLength(0));
                    myPane.XAxis.Scale.Min = (double)clt.Min(qqdadosv) - (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));
                    myPane.XAxis.Scale.Max = (double)clt.Max(qqdadosv) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));

                    LineItem myCurve1 = myPane.AddCurve("tituloo", list1, Color.Blue, SymbolType.Circle);
                    myCurve1.Symbol.Fill.Color = Color.Blue;
                    myCurve1.Label.IsVisible = false;

                    myPane.YAxis.Title.Text = "";
                    myPane.XAxis.Title.Text = Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                    myPane.Title.Text = "PP-Plot da variável " + Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                } // if

                if (cmbTipoDist.Text == "Poisson")
                {
                    double[,] variavelpp = clt.GetMatrizFromDataTable(m_dt_tabela_dados, userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior);
                    double mediaX = (clt.Meanc(variavelpp))[0, 0];
                    double varianX = clt.Varianciac(variavelpp)[0, 0];
                    double[,] qqpoissonv = new double[variavelpp.GetLength(0), 1];
                    double[,] qqdadosv = new double[variavelpp.GetLength(0), 1];
                    double[] qqpoisson = new double[variavelpp.GetLength(0)];
                    double[] qqdados = new double[variavelpp.GetLength(0)];
                    double[] retaponto = new double[variavelpp.GetLength(0)];
                    retaponto[0] = 0;

                    BLogicNonParametricTests bnp = new BLogicNonParametricTests();
                    Poisson poisson = new Poisson(mediaX);

                    double[,] sdados = clt.SortcDoubleArray(variavelpp);
                    int n = sdados.GetLength(0);
                    double[,] emp_dist = new double[n, 1];
                    for (int i = 0; i < n; i++)
                    {
                        qqdadosv[i, 0] = ((double)i / (double)n);
                        qqpoissonv[i, 0] = poisson.CumulativeDistribution(sdados[i, 0]);
                    } // for

                    for (int i = 0; i < qqdadosv.GetLength(0); i++)
                    {
                        qqpoisson[i] = qqpoissonv[i, 0];
                        qqdados[i] = qqdadosv[i, 0];
                    } // for

                    list1.Add(qqdados, qqpoisson);
                    for (int i = 0; i < variavelpp.GetLength(0); i++)
                        if (i > 0)
                            retaponto[i] = (retaponto[i - 1]) + ((double)clt.Max(qqpoisson) / variavelpp.GetLength(0));

                    reta.Add(retaponto, retaponto);

                    LineItem myCurve2 = myPane.AddCurve("Poisson", reta, Color.Black, SymbolType.None);
                    myCurve2.Line.IsVisible = true;

                    myPane.YAxis.Scale.Min = (double)clt.Min(qqpoissonv) - (((double)clt.Max(qqpoissonv) - (double)clt.Min(qqpoissonv)) / (double)qqpoissonv.GetLength(0));
                    myPane.YAxis.Scale.Max = (double)clt.Max(qqpoissonv) + (((double)clt.Max(qqpoissonv) - (double)clt.Min(qqpoissonv)) / (double)qqpoissonv.GetLength(0));
                    myPane.XAxis.Scale.Min = (double)clt.Min(qqdadosv) - (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));
                    myPane.XAxis.Scale.Max = (double)clt.Max(qqdadosv) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));

                    LineItem myCurve1 = myPane.AddCurve("título", list1, Color.Blue, SymbolType.Circle);
                    myCurve1.Symbol.Fill.Color = Color.Blue;
                    myCurve1.Label.IsVisible = false;

                    myPane.YAxis.Title.Text = "";
                    myPane.XAxis.Title.Text = Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                    myPane.Title.Text = "PP-Plot da variável " + Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                } // if
            } // if 

            if (ckbQQplot.Checked == true)
            {
                tabControl1.TabPages.Remove(tabPage3);
                GraphPane myPane = zedGraphControl1.GraphPane;
                PointPairList list1 = new PointPairList();
                PointPairList reta = new PointPairList();
                tabControl1.TabPages.Add(tabPage3);

                zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl1.GraphPane.GraphObjList.Clear();

                if (cmbTipoDist.Text == "Normal")
                {
                    double[,] variavelpp = clt.GetMatrizFromDataTable(m_dt_tabela_dados, userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior);
                    double mediaX = (clt.Meanc(variavelpp))[0, 0];
                    double varianX = clt.Varianciac(variavelpp)[0, 0];
                    double[,] qqnormalv = new double[variavelpp.GetLength(0), 1];
                    double[,] qqdadosv = new double[variavelpp.GetLength(0), 1];
                    double[] qqnormal = new double[variavelpp.GetLength(0)];
                    double[] qqdados = new double[variavelpp.GetLength(0)];
                    double[] retaponto = new double[variavelpp.GetLength(0)];

                    BLogicNonParametricTests bnp = new BLogicNonParametricTests();
                    Normal norm = new Normal(mediaX, Math.Sqrt(varianX));
                    //bnp.TipoDistribuicao

                    bnp.QQ_plot_1Variavel(variavelpp, out qqdadosv, out qqnormalv);

                    for (int i = 0; i < qqdadosv.GetLength(0); i++)
                    {
                        qqnormal[i] = qqnormalv[i, 0];
                        qqdados[i] = qqdadosv[i, 0];
                    } // for

                    retaponto[0] = (double)clt.Min(qqdadosv);
                    list1.Add(qqdados, qqnormal);

                    for (int i = 0; i < variavelpp.GetLength(0); i++)
                        if (i > 0)
                            retaponto[i] = (retaponto[i - 1]) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / variavelpp.GetLength(0));

                    reta.Add(retaponto, retaponto);

                    LineItem myCurve2 = myPane.AddCurve("Normal", reta, Color.Black, SymbolType.None);
                    myCurve2.Line.IsVisible = true;

                    myPane.YAxis.Scale.Min = (double)clt.Min(qqnormalv) - (((double)clt.Max(qqnormalv) - (double)clt.Min(qqnormalv)) / (double)qqnormalv.GetLength(0));
                    myPane.YAxis.Scale.Max = (double)clt.Max(qqnormalv) + (((double)clt.Max(qqnormalv) - (double)clt.Min(qqnormalv)) / (double)qqnormalv.GetLength(0));
                    myPane.XAxis.Scale.Min = (double)clt.Min(qqdadosv) - (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));
                    myPane.XAxis.Scale.Max = (double)clt.Max(qqdadosv) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));

                    LineItem myCurve1 = myPane.AddCurve("título", list1, Color.Blue, SymbolType.Circle);
                    myCurve1.Symbol.Fill.Color = Color.Blue;
                    myCurve1.Label.IsVisible = false;

                    myPane.YAxis.Title.Text = "";
                    myPane.XAxis.Title.Text = Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                    myPane.Title.Text = "QQ-Plot da variável " + Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                } // if

                if (cmbTipoDist.Text == "Exponencial")
                {
                    double[,] variavelpp = clt.GetMatrizFromDataTable(m_dt_tabela_dados, userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior);
                    double mediaX = (clt.Meanc(variavelpp))[0, 0];
                    //double varianX = clt.Varianciac(variavelpp)[0, 0];
                    double[,] qqexpv = new double[variavelpp.GetLength(0), 1];
                    double[,] qqdadosv = new double[variavelpp.GetLength(0), 1];
                    double[] qqexp = new double[variavelpp.GetLength(0)];
                    double[] qqdados = new double[variavelpp.GetLength(0)];
                    double[] retaponto = new double[variavelpp.GetLength(0)];

                    BLogicNonParametricTests bnp = new BLogicNonParametricTests();
                    Exponential exp = new Exponential(1.0 / mediaX);

                    bnp.QQ_plot_1Variavel(variavelpp, out qqdadosv, out qqexpv);

                    for (int i = 0; i < qqdadosv.GetLength(0); i++)
                    {
                        qqexp[i] = qqexpv[i, 0];
                        qqdados[i] = qqdadosv[i, 0];
                    } // for

                    retaponto[0] = (double)clt.Min(qqdadosv);
                    list1.Add(qqdados, qqexp);

                    for (int i = 0; i < variavelpp.GetLength(0); i++)
                        if (i > 0)
                            retaponto[i] = (retaponto[i - 1]) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / variavelpp.GetLength(0));

                    reta.Add(retaponto, retaponto);

                    LineItem myCurve2 = myPane.AddCurve("Exponencial", reta, Color.Black, SymbolType.None);
                    myCurve2.Line.IsVisible = true;

                    myPane.YAxis.Scale.Min = (double)clt.Min(qqexpv) - (((double)clt.Max(qqexpv) - (double)clt.Min(qqexpv)) / (double)qqexpv.GetLength(0));
                    myPane.YAxis.Scale.Max = (double)clt.Max(qqexpv) + (((double)clt.Max(qqexpv) - (double)clt.Min(qqexpv)) / (double)qqexpv.GetLength(0));
                    myPane.XAxis.Scale.Min = (double)clt.Min(qqdadosv) - (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));
                    myPane.XAxis.Scale.Max = (double)clt.Max(qqdadosv) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));

                    LineItem myCurve1 = myPane.AddCurve("título", list1, Color.Blue, SymbolType.Circle);
                    myCurve1.Symbol.Fill.Color = Color.Blue;
                    myCurve1.Label.IsVisible = false;

                    myPane.YAxis.Title.Text = "";
                    myPane.XAxis.Title.Text = Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                    myPane.Title.Text = "QQ-Plot da variável " + Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                } // if

                if (cmbTipoDist.Text == "Uniforme")
                {
                    double[,] variavelpp = clt.GetMatrizFromDataTable(m_dt_tabela_dados, userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior);
                    //double mediaX = (clt.Meanc(variavelpp))[0, 0];
                    //double varianX = clt.Varianciac(variavelpp)[0, 0];
                    double[,] qquniforv = new double[variavelpp.GetLength(0), 1];
                    double[,] qqdadosv = new double[variavelpp.GetLength(0), 1];
                    double[] qqunifor = new double[variavelpp.GetLength(0)];
                    double[] qqdados = new double[variavelpp.GetLength(0)];
                    double[] retaponto = new double[variavelpp.GetLength(0)];

                    BLogicNonParametricTests bnp = new BLogicNonParametricTests();

                    bnp.QQ_plot_1Variavel(variavelpp, out qqdadosv, out qquniforv);

                    for (int i = 0; i < qqdadosv.GetLength(0); i++)
                    {
                        qqunifor[i] = qquniforv[i, 0];
                        qqdados[i] = qqdadosv[i, 0];
                    } // for

                    retaponto[0] = (double)clt.Min(qqdadosv);
                    list1.Add(qqdados, qqunifor);

                    for (int i = 0; i < variavelpp.GetLength(0); i++)
                        if (i > 0)
                            retaponto[i] = (retaponto[i - 1]) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / variavelpp.GetLength(0));

                    reta.Add(retaponto, retaponto);

                    LineItem myCurve2 = myPane.AddCurve("Uniforme", reta, Color.Black, SymbolType.None);
                    myCurve2.Line.IsVisible = true;

                    myPane.YAxis.Scale.Min = (double)clt.Min(qquniforv) - (((double)clt.Max(qquniforv) - (double)clt.Min(qquniforv)) / (double)qquniforv.GetLength(0));
                    myPane.YAxis.Scale.Max = (double)clt.Max(qquniforv) + (((double)clt.Max(qquniforv) - (double)clt.Min(qquniforv)) / (double)qquniforv.GetLength(0));
                    myPane.XAxis.Scale.Min = (double)clt.Min(qqdadosv) - (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));
                    myPane.XAxis.Scale.Max = (double)clt.Max(qqdadosv) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));

                    LineItem myCurve1 = myPane.AddCurve("título", list1, Color.Blue, SymbolType.Circle);
                    myCurve1.Symbol.Fill.Color = Color.Blue;
                    myCurve1.Label.IsVisible = false;

                    myPane.YAxis.Title.Text = "";
                    myPane.XAxis.Title.Text = Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                    myPane.Title.Text = "QQ-Plot da variável " + Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                } // if

                if (cmbTipoDist.Text == "Poisson")
                {
                    double[,] variavelpp = clt.GetMatrizFromDataTable(m_dt_tabela_dados, userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior);
                    double mediaX = (clt.Meanc(variavelpp))[0, 0];
                    double varianX = clt.Varianciac(variavelpp)[0, 0];
                    double[,] qqpoissonv = new double[variavelpp.GetLength(0), 1];
                    double[,] qqdadosv = new double[variavelpp.GetLength(0), 1];
                    double[] qqpoisson = new double[variavelpp.GetLength(0)];
                    double[] qqdados = new double[variavelpp.GetLength(0)];
                    double[] retaponto = new double[variavelpp.GetLength(0)];

                    BLogicNonParametricTests bnp = new BLogicNonParametricTests();
                    Poisson poisson = new Poisson(mediaX);

                    bnp.QQ_plot_1Variavel(variavelpp, out qqdadosv, out qqpoissonv);
                    for (int i = 0; i < qqdadosv.GetLength(0); i++)
                    {
                        qqpoisson[i] = qqpoissonv[i, 0];
                        qqdados[i] = qqdadosv[i, 0];
                    } // for

                    retaponto[0] = (double)clt.Min(qqdadosv);
                    list1.Add(qqdados, qqpoisson);

                    for (int i = 0; i < variavelpp.GetLength(0); i++)
                        if (i > 0)
                            retaponto[i] = (retaponto[i - 1]) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / variavelpp.GetLength(0));

                    reta.Add(retaponto, retaponto);
                    LineItem myCurve2 = myPane.AddCurve("Poisson", reta, Color.Black, SymbolType.None);
                    myCurve2.Line.IsVisible = true;

                    myPane.YAxis.Scale.Min = (double)clt.Min(qqpoissonv) - (((double)clt.Max(qqpoissonv) - (double)clt.Min(qqpoissonv)) / (double)qqpoissonv.GetLength(0));
                    myPane.YAxis.Scale.Max = (double)clt.Max(qqpoissonv) + (((double)clt.Max(qqpoissonv) - (double)clt.Min(qqpoissonv)) / (double)qqpoissonv.GetLength(0));
                    myPane.XAxis.Scale.Min = (double)clt.Min(qqdadosv) - (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));
                    myPane.XAxis.Scale.Max = (double)clt.Max(qqdadosv) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));

                    LineItem myCurve1 = myPane.AddCurve("título", list1, Color.Blue, SymbolType.Circle);
                    myCurve1.Symbol.Fill.Color = Color.Blue;
                    myCurve1.Label.IsVisible = false;

                    myPane.YAxis.Title.Text = "";
                    myPane.XAxis.Title.Text = Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                    myPane.Title.Text = "QQ-Plot da variável " + Convert.ToString(userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0]);
                } // if 
            } // if

            #region gerando o output para resultado das estimações

            string out_text = "============================================================================================================================\n\n";

            out_text += "Testes Não Paramétricos \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
            out_text += "Variáveis  testadas: \n";
            out_text += "Variáveis  dependentes: ";
            for (int i = 0; i < dados.GetLength(1); i++)
                out_text += userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + " ";

            out_text += " Variáveis  independentes: ";

            for (int i = 0; i < dados1.GetLength(1); i++)
                out_text += userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoInferior[i] + " ";

            out_text += "\n";
            out_text += "Número de observações: " + dados.GetLength(0) + "\n";
            out_text += "\n";

            if (ckbAKS.Checked)
            {
                tipodist_ = cmbTipoDist.Text;
                //blnp.KS_Test2(dados, dados1);

                if (dados1.GetLength(0) < 2)
                {
                    blnp.KS_Test(dados, out tstat_ks, out pvalor_ks);

                    for (int i = 0; i < dados.GetLength(1); i++)
                    {
                        out_text += "\n===================  Kolmogorov Smirnov - " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + " " + cmbTipoDist.Text + " \n\n";

                        switch (cmbTipoDist.Text)
                        {
                            case "Normal":
                                out_text += "O resultado apresentado para teste não paramétrico de ajuste de distribuição KS para testar se a distribuição é " + cmbTipoDist.Text + "\n";
                                out_text += "H0: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") ~ N(" + clt.Double2Texto(clt.Mean(clt.SubColumnArrayDouble(dados, i)), 1) + ";" + clt.Double2Texto((clt.Despadc(clt.SubColumnArrayDouble(dados, i))[0, 0]) * Math.Sqrt((dados.GetLength(0)) / (dados.GetLength(0) - 1.0)), 2) + ").\n";
                                out_text += "H1: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") !~ N(" + clt.Double2Texto(clt.Mean(clt.SubColumnArrayDouble(dados, i)), 1) + ";" + clt.Double2Texto((clt.Despadc(clt.SubColumnArrayDouble(dados, i))[0, 0]) * Math.Sqrt((dados.GetLength(0)) / (dados.GetLength(0) - 1.0)), 2) + ").\n\n";
                                out_text += "Estatística do Teste: " + clt.Double2Texto(tstat_ks[i], 6) + "\n";
                                out_text += "P-valor: " + clt.Double2Texto(pvalor_ks[i], 6) + "\n";
                                break;

                            case "Exponencial":
                                out_text += "O resultado apresentado para teste não paramétrico de ajuste de distribuição KS para testar se a distribuição é " + cmbTipoDist.Text + "\n";
                                out_text += "H0: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") ~ Exp(" + clt.Double2Texto(clt.Mean(clt.SubColumnArrayDouble(dados, i)), 1) + ").\n";
                                out_text += "H1: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") !~ Exp(" + clt.Double2Texto(clt.Mean(clt.SubColumnArrayDouble(dados, i)), 1) + ").\n\n";
                                out_text += "Estatística do Teste: " + clt.Double2Texto(tstat_ks[i], 6) + "\n";
                                out_text += "P-valor: " + clt.Double2Texto(pvalor_ks[i], 6) + "\n";
                                break;

                            case "Uniforme":
                                out_text += "O resultado apresentado para teste não paramétrico de ajuste de distribuição KS para testar se a distribuição é " + cmbTipoDist.Text + "\n";
                                out_text += "H0: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") ~ U(" + clt.Double2Texto(clt.Min(clt.SubColumnArrayDouble(dados, i)), 1) + ";" + clt.Double2Texto(clt.Maxc(clt.SubColumnArrayDouble(dados, i))[0, 0], 2) + ").\n";
                                out_text += "H1: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") !~ U(" + clt.Double2Texto(clt.Min(clt.SubColumnArrayDouble(dados, i)), 1) + ";" + clt.Double2Texto(clt.Maxc(clt.SubColumnArrayDouble(dados, i))[0, 0], 2) + ").\n\n";
                                out_text += "Estatística do Teste: " + clt.Double2Texto(tstat_ks[i], 6) + "\n";
                                out_text += "P-valor: " + clt.Double2Texto(pvalor_ks[i], 6) + "\n\n\n\n";
                                break;

                            case "Poisson":
                                out_text += "O resultado apresentado para teste não paramétrico de ajuste de distribuição KS para testar se a distribuição é " + cmbTipoDist.Text + "\n";
                                out_text += "H0: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") ~ Pois(" + clt.Double2Texto(clt.Mean(clt.SubColumnArrayDouble(dados, i)), 1) + ").\n";
                                out_text += "H1: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") !~ Pois(" + clt.Double2Texto(clt.Mean(clt.SubColumnArrayDouble(dados, i)), 1) + ").\n\n";
                                out_text += "Estatística do Teste: " + clt.Double2Texto(tstat_ks[i], 6) + "\n";
                                out_text += "P-valor: " + clt.Double2Texto(pvalor_ks[i], 6) + "\n";
                                break;
                        }
                    }
                }
                else
                {
                    blnp.KS_Test2(dados, dados1, out tstat_ks2, out pvalor_ks2, out par1, out par2);

                    for (int i = 0; i < dados.GetLength(1); i++)
                    {
                        out_text += "\n===================  Kolmogorov Smirnov - " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + " " + cmbTipoDist.Text + " \n\n";

                        for (int j = 0; j < tstat_ks2.GetLength(0); j++)

                            switch (cmbTipoDist.Text)
                            {
                                case "Normal":
                                    out_text += "O resultado apresentado para teste não paramétrico de ajuste de distribuição KS para testar se a distribuição é " + cmbTipoDist.Text + ", fator " + clt.Double2Texto(tstat_ks2[j, 0]) + ":" + "\n";
                                    out_text += "H0: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") ~ N(" + clt.Double2Texto(par1[j, i], 2) + ";" + clt.Double2Texto(par2[j, i], 2) + ").\n";
                                    out_text += "H1: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") !~ N(" + clt.Double2Texto(par1[j, i], 2) + ";" + clt.Double2Texto(par2[j, i], 2) + ").\n\n";
                                    out_text += "Estatística do Teste: " + clt.Double2Texto(tstat_ks2[j, (i + 1)], 6) + "\n";
                                    out_text += "P-valor: " + clt.Double2Texto(pvalor_ks2[j, (i + 1)], 6) + "\n\n\n";
                                    break;

                                case "Exponencial":
                                    out_text += "O resultado apresentado para teste não paramétrico de ajuste de distribuição KS para testar se a distribuição é " + cmbTipoDist.Text + ", fator " + clt.Double2Texto(tstat_ks2[j, 0]) + ":" + "\n";
                                    out_text += "H0: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") ~ Exp(" + clt.Double2Texto(par1[j, i], 1) + ").\n";
                                    out_text += "H1: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") !~ Exp(" + clt.Double2Texto(par1[j, i], 1) + ").\n\n";
                                    out_text += "Estatística do Teste: " + clt.Double2Texto(tstat_ks2[j, (i + 1)], 6) + "\n";
                                    out_text += "P-valor: " + clt.Double2Texto(pvalor_ks2[j, (i + 1)], 6) + "\n\n\n";
                                    break;

                                case "Uniforme":
                                    out_text += "O resultado apresentado para teste não paramétrico de ajuste de distribuição KS para testar se a distribuição é " + cmbTipoDist.Text + ", fator " + clt.Double2Texto(tstat_ks2[j, 0]) + ":" + "\n";
                                    out_text += "H0: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") ~ U(" + clt.Double2Texto(par1[j, i], 1) + ";" + clt.Double2Texto(par2[j, i], 2) + ").\n";
                                    out_text += "H1: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") !~ U(" + clt.Double2Texto(par1[j, i], 1) + ";" + clt.Double2Texto(par2[j, i], 2) + ").\n\n";
                                    out_text += "Estatística do Teste: " + clt.Double2Texto(tstat_ks2[j, (i + 1)], 6) + "\n";
                                    out_text += "P-valor: " + clt.Double2Texto(pvalor_ks2[j, (i + 1)], 6) + "\n\n\n";
                                    break;

                                case "Poisson":
                                    out_text += "O resultado apresentado para teste não paramétrico de ajuste de distribuição KS para testar se a distribuição é " + cmbTipoDist.Text + ", fator " + clt.Double2Texto(tstat_ks2[j, 0]) + ":" + "\n";
                                    out_text += "H0: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") ~ Pois(" + clt.Double2Texto(par1[j, i], 1) + ").\n";
                                    out_text += "H1: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") !~ Pois(" + clt.Double2Texto(par1[j, i], 1) + ").\n\n";
                                    out_text += "Estatística do Teste: " + clt.Double2Texto(tstat_ks2[j, (i + 1)], 6) + "\n";
                                    out_text += "P-valor: " + clt.Double2Texto(pvalor_ks2[j, (i + 1)], 6) + "\n\n\n";
                                    break;
                            }
                    }
                }
            }

            if (ckbQQajust.Checked)
            {
                double[,] temp_dados = new double[0, 0];
                for (int i = 0; i < dados.GetLength(1); i++)
                {
                    temp_dados = clt.SubColumnArrayDouble(dados, i);
                    out_text += "\n===================  Teste de Ajuste Qui-Quadrado - " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i]+ " " + cmbTipoDist.Text /*+ cmbTipoDist.Text +*/ + "\n\n";
                    blnp.Teste_AjusteQQuadrado(temp_dados, out tstat_ks1, out pvalor_ks1, out tabela, out par11, out par22, out verificacao, out tabelapoisson);
                    out_text += "O resultado apresentado para teste não paramétrico de ajuste qui-quadrado para testar se os dados tem distribuição " + cmbTipoDist.Text + "\n";

                    if (cmbTipoDist.Text == "Exponencial" || cmbTipoDist.Text == "Poisson")
                    {
                        out_text += "H0: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i]+") ~ "  + cmbTipoDist.Text+"(" + clt.Double2Texto(par11, 1) + ").\n";
                        out_text += "H1: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i]+") !~ "  + cmbTipoDist.Text+"(" + clt.Double2Texto(par11, 1) + ").\n\n";
                    }
                    else
                    {
                        out_text += "H0: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") ~ "  + cmbTipoDist.Text+"(" + clt.Double2Texto(par11, 1) + " ; " + clt.Double2Texto(par22, 1) + ").\n";
                        out_text += "H1: f(" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[i] + ") !~ " + cmbTipoDist.Text + "(" + clt.Double2Texto(par11, 1) + " ; " + clt.Double2Texto(par22, 1) + ").\n";
                    }
                    out_text += "Estatística do Teste: " + clt.Double2Texto(tstat_ks1, 6) + "\n";
                    out_text += "P-valor: " + clt.Double2Texto(pvalor_ks1, 6) + "\n\n";
                    out_text += "\n===================Tabela de Frequencia\n";
                    string[] variaveis = new string[tabela.GetLength(1)];
                    variaveis[0] = "Intervalos";
                    variaveis[1] = "Frequencia Observada";
                    variaveis[2] = "Frequencia Esperada";

                    if (cmbTipoDist.Text == "Poisson")
                        out_text += blbm.GeraTabelaNovasVariaveis(tabelapoisson, variaveis);
                    else
                    {
                        out_text += blbm.GeraTabelaNovasVariaveis(tabela, variaveis);
                        out_text += "\n Os dados foram categorizados segundo o critério de Decis.";
                    }
                    if (verificacao >= 1) out_text += "\n OBS.: Aproximação do qui-quadrado pode estar incorreta devido aos valores das frequências esperadas.";
                }
            } // if
            
                if (ckbMW.Checked)
                {
                    blnp.Mann_Whitney(dados, dados1, out posto_mw, out estat_mw, out dp_mw, out pvalue_mw);
                    out_text += "\n========================== Teste Mann- Whintney  - " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0] + " " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoInferior[0] + " \n\n";
                    out_text += "O resultado apresenta o teste não paramétrico Mann-Whitney para comparação de médias de duas amostras.\n";
                    out_text += "O teste é feito por meio da comparação dos postos dos dois grupos. A estatística Z é calculada com correção de continuidade.\n";
                    out_text += "H0: m1 =  m2 \n";
                    out_text += "H1: m1 !=  m2 \n\n";
                    out_text += "Soma dos postos: " + clt.Double2Texto(posto_mw, 6) + "\n";
                    out_text += "Estatística(m1): " + clt.Double2Texto(estat_mw, 6) + "\n";
                    out_text += "Desvio padrão: " + clt.Double2Texto(dp_mw, 6) + "\n";

                    if (estat_mw > 0.0)
                        out_text += "P-valor: >  Z " + clt.Double2Texto(pvalue_mw, 6) + "\n";
                    else
                        out_text += "P-valor: <  Z " + clt.Double2Texto(pvalue_mw, 6) + "\n";

                    out_text += "P-valor: > |Z| " + clt.Double2Texto(2 * pvalue_mw, 6) + "\n";
                } // if

                if (ckbKW.Checked)
                {
                    blnp.Kruskal_Wallis(dados, dados1, out estat_kw, out estat_kw_corrigida, out pvalue_kw, out diferenca_sig);
                    out_text += "\n========================== Teste Kruskal Wallis  - " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0] + " " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoInferior[0] + " \n\n";
                    out_text += "O resultado apresenta o teste não paramétrico Kruskal Wallis de comparação de médias para 2 grupos ou mais.\n";
                    out_text += "H0: μ_i =  μ_k \n";
                    out_text += "H1: μ_i !=  μ_k,  i!=k \n\n";
                    out_text += "Estatística: " + clt.Double2Texto(estat_kw, 6) + "\n";
                    out_text += "Estatística com correção de empates: " + clt.Double2Texto(estat_kw_corrigida, 6) + "\n";
                    out_text += "P-valor: " + clt.Double2Texto(pvalue_kw, 6) + "\n\n";
                    out_text += "\n========= Tabela de diferenças\n";
                    string[] variaveis = new string[diferenca_sig.GetLength(1)];
                    variaveis[0] = "Grupo1";
                    variaveis[1] = "Grupo2";
                    variaveis[2] = "Limite";
                    variaveis[3] = "Diferença";
                    variaveis[4] = "Significativo";
                    out_text += blbm.GeraTabelaNovasVariaveis(diferenca_sig, variaveis);
                } // if

                if (ckbWil.Checked)
                {
                    blnp.Wilcoxon(dados, dados1, out estat_wil, out pvalue_wil, out  ver_empates);
                    out_text += "\n========================== Teste Wilcoxon  - " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0] + " " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoInferior[0] + " \n\n";
                    out_text += "O resultado apresenta o teste não paramétrico Wilcoxon de comparação de médias para 2 grupos.\n";
                    out_text += "H0: μ_i =  μ_k \n";
                    out_text += "H1: μ_i !=  μ_k,  i!=k \n\n";
                    out_text += "Estatística: " + clt.Double2Texto(estat_wil, 6) + "\n";
                    out_text += "P-valor: " + clt.Double2Texto(pvalue_wil, 6) + "\n\n";
                    if (ver_empates == true) out_text += "P-valor calculado com correção de empates \n\n";
                } // if

                bool calcularVariancia = false;
                double stat_t = 0.0;
                double pvalor_t = 0.0;
                double var1 = 0.0;
                double var2 = 0.0;
                try
                {
                    if (txbVar1.Text != "") var1 = Convert.ToDouble(txbVar1.Text);
                    else calcularVariancia = true;
                } // try
                catch
                {
                    MessageBox.Show("Variância 1 especificada não numérica", "Comparação de médias", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } // catch

                try
                {
                    if (txbVar2.Text != "") var2 = Convert.ToDouble(txbVar2.Text);
                    else calcularVariancia = true;
                } // try
                catch
                {
                    MessageBox.Show("Variância 2 especificada não numérica", "Comparação de médias", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } // catch

                if (rdbTestT.Checked)
                {
                    try
                    {
                        clt.FrequencyTable(ref tabclasse, dados1);
                        int k = tabclasse.GetLength(0);

                        int j = blnp.VariaveisDependentes.GetLength(0);

                        if (j < 2)
                        {
                            if (k < 3)
                            {
                                if (cmbTipoTestT.SelectedItem == null)
                                    MessageBox.Show("Defina um tipo de Amostra: Independentes ou Pareadas", "Comparação de médias", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                else
                                {
                                    if (cmbTipoTestT.SelectedItem.ToString() == "Amostras Pareadas")
                                        if (ckbVar.Checked)
                                        {
                                            if (calcularVariancia)
                                                blnp.TesteT_Dependentes(dados, dados1, out stat_t, out pvalor_t);
                                            else
                                                blnp.TesteT_Dependentes(dados, dados1, out stat_t, out pvalor_t, var1);

                                            out_text += "\n========================== Comparação de médias pareadas com variâncias conhecidas- " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0] + " " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoInferior[0] + " \n\n";
                                            out_text += "H0: μ_1 =  μ_2 \n";
                                            out_text += "H1: μ_1 !=  μ_2\n\n";
                                            out_text += "Estatística Z: " + clt.Double2Texto(stat_t, 6) + "\n";
                                            out_text += "P-valor: " + clt.Double2Texto(pvalor_t, 6) + "\n\n";
                                        } // if
                                        else
                                        {
                                            blnp.TesteT_Dependentes(dados, dados1, out stat_t, out pvalor_t);
                                            out_text += "\n========================== Comparação de médias pareadas com variâncias desconhecidas - " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0] + " " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoInferior[0] + " \n\n";
                                            out_text += "H0: μ_1 =  μ_2 \n";
                                            out_text += "H1: μ_1 !=  μ_2\n\n";
                                            out_text += "Estatística Z: " + clt.Double2Texto(stat_t, 6) + "\n";
                                            out_text += "P-valor: " + clt.Double2Texto(pvalor_t, 6) + "\n\n";
                                        } // else

                                    if (cmbTipoTestT.SelectedItem.ToString() == "Amostras Independentes")
                                        if (ckbVar.Checked)
                                        {
                                            if (calcularVariancia)
                                                blnp.TesteT_Independente(dados, dados1, out stat_t, out pvalor_t, true);
                                            else
                                                blnp.TesteT_Independente(dados, dados1, out stat_t, out pvalor_t, var1, var2);

                                            out_text += "\n========================== Comparação de médias independentes com variâncias conhecidas - " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0] + " " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoInferior[0] + " \n\n";
                                            out_text += "H0: μ_1 =  μ_2 \n";
                                            out_text += "H1: μ_1 !=  μ_2\n\n";
                                            out_text += "Estatística Z: " + clt.Double2Texto(stat_t, 6) + "\n";
                                            out_text += "P-valor: " + clt.Double2Texto(pvalor_t, 6) + "\n\n";
                                        } // if
                                        else
                                        {
                                            if (ckbVarIguais.Checked)
                                            {
                                                blnp.TesteT_Independente(dados, dados1, out stat_t, out pvalor_t, true);
                                                out_text += "\n========================== Comparação de médias independentes com variâncias desconhecidas e iguais - " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0] + " " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoInferior[0] + " \n\n";
                                                out_text += "H0: μ_1 =  μ_2 \n";
                                                out_text += "H1: μ_1 !=  μ_2\n\n";
                                                out_text += "Estatística T: " + clt.Double2Texto(stat_t, 6) + "\n";
                                                out_text += "P-valor: " + clt.Double2Texto(pvalor_t, 6) + "\n\n";
                                            } // if
                                            else
                                            {
                                                blnp.TesteT_Independente(dados, dados1, out stat_t, out pvalor_t);
                                                out_text += "\n========================== Comparação de médias independentes com variâncias desconhecidas e diferentes -" + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoSuperior[0] + " " + userControlSelecaoDoisBlocosVariaveis1.VariaveisBlocoInferior[0] + " \n\n";
                                                out_text += "H0: μ_1 =  μ_2 \n";
                                                out_text += "H1: μ_1 !=  μ_2\n\n";
                                                out_text += "Estatística T: " + clt.Double2Texto(stat_t, 6) + "\n";
                                                out_text += "P-valor: " + clt.Double2Texto(pvalor_t, 6) + "\n\n";
                                            } // else
                                        } // else
                                } // else
                            } // if
                            else MessageBox.Show("Para comparação de médias de 3 ou mais grupos/classes utilize a ANOVA", "Comparação de médias", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        } // if
                        else MessageBox.Show("Só é possivel comparar a média de uma variável", "Comparação de médias", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    } // try
                    catch (Exception er)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show(er.Message, "Comparação de médias", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    } // catch
                } // if

                if (rdbAnova.Checked)
                {
                    try
                    {
                        clt.FrequencyTable(ref tabclasse, dados1);
                        int k = tabclasse.GetLength(0);
                        int j = blnp.VariaveisDependentes.GetLength(0);

                        if (j < 2)
                        {
                            if (k > 2)
                            {
                                blnp.TesteAnova(dados, dados1, out nomes, out vetorsomadequadrados, out vetorgl, out vetorqdmedio, out vetorF, out vetorpvalue);

                                out_text += "\n========================== Comparação de médias para 3 ou mais populações ==========================\n " + " \n\n";
                                out_text += "H0: μ1 = μ2 = ... = μn  \n";
                                out_text += "H1: Pelo menos uma das médias μi é diferente das demais\n\n";

                                out_text += "\n";
                                out_text += blbm.GeraANOVA(nomes, vetorsomadequadrados, vetorgl, vetorqdmedio, vetorF, vetorpvalue);
                            } // if 
                            else MessageBox.Show("É necessário ter no mínimo 3 grupos/classes", "Comparação de médias para 3 ou mais populações", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        } // if
                        else MessageBox.Show("Só é possivel comparar a média de uma variável", "Comparação de médias para 3 ou mais populações", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    } // try
                    catch (Exception er)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show(er.Message, "Comparação de médias", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    } // catch
                } // if

                if (ckbLimpaJanelaOutput.Checked)
                    userControlRichTextOutput1.Texto = out_text;
                else userControlRichTextOutput1.Texto += out_text;

                if (!tabControl1.TabPages.Contains(tabPage2))
                    tabControl1.TabPages.Add(tabPage2);

            #endregion
           
        }// ExecutaTestes()

        #endregion

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                this.ExecutaTestes();
                this.tabControl1.SelectedTab = tabPage2;

                Cursor = Cursors.Default;
            } // try
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Teste não paramétrico", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // catch
        } // btnExecutar_Click()
        //}

        private bool marcar = false;
        private void rdbTestT_CheckedChanged_1(object sender, EventArgs e)
        {
            marcar = rdbTestT.Checked;
        }

        private void rdbTestT_Click_1(object sender, EventArgs e)
        {
            if (rdbTestT.Checked && marcar == false)
            {
                rdbTestT.Checked = false;
            }
            else
            {
                rdbTestT.Checked = true;
                marcar = false;
            }
        }

        private void rdbAnova_CheckedChanged_1(object sender, EventArgs e)
        {
            marcar = rdbAnova.Checked;
        }

        private void rdbAnova_Click_1(object sender, EventArgs e)
        {
            if (rdbAnova.Checked && marcar == false)
            {
                rdbAnova.Checked = false;
            }
            else
            {
                rdbAnova.Checked = true;
                marcar = false;
            }
        }
    }

    //private void cmbTipoTestT_SelectedIndexChanged_1(object sender, EventArgs e)
}
