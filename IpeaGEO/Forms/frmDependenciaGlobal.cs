using System;
using System.Data;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo
{

    public partial class frmDependenciaGlobal : Form
    {

        public frmDependenciaGlobal()
        {
            InitializeComponent();
        }

        #region variáveis internas

        protected string m_output_text = "";
        private IpeaGeo.RegressoesEspaciais.clsIpeaShape shapeAlex = new RegressoesEspaciais.clsIpeaShape();
        public IpeaGeo.RegressoesEspaciais.clsIpeaShape EstruturaShape
        {
            get
            {
                return shapeAlex;
            }
            set
            {
                shapeAlex = value;
            }
        }
        private string[] strVariaveisQualitativas;
        public string[] VariaveisQualitativas
        {
            get
            {
                return strVariaveisQualitativas;
            }
            set
            {
                strVariaveisQualitativas = value;
            }
        }
        private string[] strVariaveisQuantitativas;
        public string[] VariaveisQuantitativas
        {
            get
            {
                return strVariaveisQuantitativas;
            }
            set
            {
                strVariaveisQuantitativas = value;
            }
        }

        private string[] strVariaveisSelecionadasQuantitativas;
        public string[] VariaveisSelecionadasQuantitativas
        {
            get
            {
                return strVariaveisSelecionadasQuantitativas;
            }
            set
            {
                strVariaveisSelecionadasQuantitativas = value;
            }
        }

        private string[] strVariaveisSelecionadasQualitativas;
        public string[] VariaveisSelecionadasQualitativas
        {
            get
            {
                return strVariaveisSelecionadasQualitativas;
            }
            set
            {
                strVariaveisSelecionadasQualitativas = value;
            }
        }

        private int[] classePoligonos;
        public int[] vetorPoligonos
        {
            get
            {
                return classePoligonos;
            }
            set
            {
                classePoligonos = value;
            }
        }
        private string strIDmapa;
        public string IdentificadorMapa
        {
            get
            {
                return strIDmapa;
            }
            set
            {
                strIDmapa = value;
            }
        }

        private bool blRelatorio;
        public bool GeraRelatorio
        {
            get
            {
                return blRelatorio;
            }
            set
            {
                blRelatorio = value;
            }
        }

        private string strID;
        public string IdentificadorDados
        {
            get
            {
                return strID;
            }
            set
            {
                strID = value;
            }
        }
        private DataTable dTable;
        public DataTable DataTableDados
        {
            get
            {
                return dTable;
            }
            set
            {
                dTable = value;
            }
        }

        private string strEnderecoMapa;
        public string EnderecoMapa
        {
            get
            {
                return strEnderecoMapa;
            }
            set
            {
                strEnderecoMapa = value;
            }
        }

        private double[] dblIMORAN;
        public double[] IndiceMoran
        {
            get
            {
                return dblIMORAN;
            }
            set
            {
                dblIMORAN = value;
            }
        }
        private double[] dblpIMORAN;
        public double[] pValorIndiceMoran
        {
            get
            {
                return dblpIMORAN;
            }
            set
            {
                dblpIMORAN = value;
            }
        }

        private double[] dblIMORANSIMPLES;
        public double[] IndiceMoranSimples
        {
            get
            {
                return dblIMORANSIMPLES;
            }
            set
            {
                dblIMORANSIMPLES = value;
            }
        }
        private double[] dblpIMORANSIMPLES;
        public double[] pValorIndiceMoranSimples
        {
            get
            {
                return dblpIMORANSIMPLES;
            }
            set
            {
                dblpIMORANSIMPLES = value;
            }
        }

        private double[] dblGearyI;
        public double[] IndiceGeary
        {
            get
            {
                return dblGearyI;
            }
            set
            {
                dblGearyI = value;
            }
        }

        private double[] dblpGearyI;
        public double[] pValorIndiceGeary
        {
            get
            {
                return dblpGearyI;
            }
            set
            {
                dblpGearyI = value;
            }
        }

        private double[] dblGetisI;
        public double[] IndiceGetis
        {
            get
            {
                return dblGetisI;
            }
            set
            {
                dblGetisI = value;
            }
        }

        private double[] dblpGetisI;
        public double[] pValorIndiceGetis
        {
            get
            {
                return dblpGetisI;
            }
            set
            {
                dblpGetisI = value;
            }
        }

        private double[] dblTangoI;
        public double[] IndiceTango
        {
            get
            {
                return dblTangoI;
            }
            set
            {
                dblTangoI = value;
            }
        }

        private double[] dblpTangoI;
        public double[] pValorIndiceTango
        {
            get
            {
                return dblpTangoI;
            }
            set
            {
                dblpTangoI = value;
            }
        }

        private double[] dblRogersonI;
        public double[] IndiceRogerson
        {
            get
            {
                return dblRogersonI;
            }
            set
            {
                dblRogersonI = value;
            }
        }

        private double[] dblpRogersonI;
        public double[] pValorIndiceRogerson
        {
            get
            {
                return dblpRogersonI;
            }
            set
            {
                dblpRogersonI = value;
            }
        }
        private string strPOPULACAO;
        public string VariavelPopulacao
        {
            get
            {
                return strPOPULACAO;
            }
            set
            {
                strPOPULACAO = value;
            }
        }
        private string strTipoPeso;
        public string TipoDoPeso
        {
            get
            {
                return strTipoPeso;
            }
            set
            {
                strTipoPeso = value;
            }
        }
        private int intNumeroSim;
        public int NumeroDeSimulacoes
        {
            get
            {
                return intNumeroSim;
            }
            set
            {
                intNumeroSim = value;
            }
        }

        private bool m_matriz_W_pre_definida = false;
        public bool MatrizWPreDefinida
        {
            get { return m_matriz_W_pre_definida; }
            set { m_matriz_W_pre_definida = value; }
        }

        private IpeaGeo.RegressoesEspaciais.clsMatrizEsparsa m_W_esparsa;
        public IpeaGeo.RegressoesEspaciais.clsMatrizEsparsa MatrizWEsparsa
        {
            get { return m_W_esparsa; }
            set { m_W_esparsa = value; }
        }

        private bool m_matriz_W_normalizada = false;
        public bool MatrizWNormalizada
        {
            get { return m_matriz_W_normalizada; }
            set { m_matriz_W_normalizada = value; }
        }

        private int m_ordem_vizinhanca = 0;
        public int OrdemVizinhanca
        {
            set { m_ordem_vizinhanca = value; }
        }

        private string m_tipo_matriz_vizinhanca = "";
        public string TipoMatrizVizinhanca
        {
            set { m_tipo_matriz_vizinhanca = value; }
        }

        #endregion

        private void frmDependenciaGlobal_Load(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.TabPages.Contains(tabPage2))
                {
                    tabControl1.TabPages.Remove(tabPage2);
                }

                if (m_matriz_W_pre_definida && m_tipo_matriz_vizinhanca != "")
                {
                    cmbVizinhanca.Items.Clear();

                    cmbVizinhanca.Enabled = false;
                    chkNormal.Checked = m_matriz_W_normalizada;
                    chkNormal.Enabled = false;

                    lblTipoMatrizVizinhanca.Visible = true;
                    lblTipoMatrizVizinhanca.Text = "Tipo de vizinhança: " + m_tipo_matriz_vizinhanca;

                    if (m_ordem_vizinhanca > 0)
                    {
                        lblOrdemMatrizVizinhanca.Visible = true;
                        lblOrdemMatrizVizinhanca.Text = "Ordem da matriz de vizinhança: " + m_ordem_vizinhanca;
                    }

                    shapeAlex.TipoVizinhanca = m_tipo_matriz_vizinhanca;
                }
                else
                {
                    lblOrdemMatrizVizinhanca.Visible = false;
                    lblTipoMatrizVizinhanca.Visible = false;

                    cmbVizinhanca.SelectedIndex = 0;
                }

                //Adiciona as variáveis
                checkedListBox1.Items.AddRange(strVariaveisQuantitativas);

                cmbPop.Items.Clear();
                //cmbPop.Items.AddRange(strVariaveisQuantitativas);
                //cmbPop.SelectedIndex = 0;

                if (m_tipo_matriz_vizinhanca == "") cmbVizinhanca.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private double[] dblPvalorMoran;
        private double[] dblMoran;
        private double[] dblEspMoran;
        private double[] dblVarMoran;
        private double[] dblPAnaliticoMoran;

        private double[] dblPvalorGeary;
        private double[] dblGeary;
        private double[] dblEspGeary;
        private double[] dblVarGeary;
        private double[] dblPAnaliticoGeary;

        private double[] dblPvalorGetis;
        private double[] dblGetis;
        private double[] dblEspGetis;
        private double[] dblVarGetis;
        private double[] dblPAnaliticoGetis;


        private double[] dblTango;
        private double[] dblPvalorTango;
        private object[] dblEspTango;
        private object[] dblVarTango;
        private object[] dblPAnaliticoTangoLance;
        private object[] dblPAnaliticoTangoRogerson;

        private double[] dblRogerson;
        private double[] dblPvalorRogerson;
        private object[] dblEspRogerson;
        private object[] dblVarRogerson;
        private object[] dblPAnaliticoRogersonLance;
        private object[] dblPAnaliticoRogersonRogerson;
        
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                blRelatorio = chkRelatorio.Checked;
                progressBar1.Visible = true;
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                #region Guarda as variáveis
                int cSelected1 = 0;
                //Variáveis QUANTITATIVAS

                bool erro_de_pop = false;

                if (chkRogerson.Checked || chkTango.Checked)
                {

                    for (int i = 0; i < checkedListBox1.Items.Count; i++) if (checkedListBox1.GetItemChecked(i) == true) cSelected1++;
                    string[] strSelecionadasQuanti = new string[cSelected1 + 1];
                    //string[] strSelecionadasQuanti = new string[cSelected1];
                    cSelected1 = 0;
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        if (checkedListBox1.GetItemChecked(i) == true)
                        {
                            strSelecionadasQuanti[cSelected1] = checkedListBox1.Items[i].ToString();
                            cSelected1++;
                        }
                    }
                    strSelecionadasQuanti[cSelected1] = cmbPop.SelectedItem.ToString();
                    strVariaveisSelecionadasQuantitativas = strSelecionadasQuanti;

                    //Checar se a variável de população é de fato maior que a soma de todos os casos selecionados
                    //Somente habilitar a geração de indices se a população for maior que as variaveis.

                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        double soma_vars = 0;
                        for (int j = 0; j < strVariaveisSelecionadasQuantitativas.Length - 1; j++)
                        {
                            soma_vars += Convert.ToDouble(dTable.Rows[i][strVariaveisSelecionadasQuantitativas[j]]);
                        }
                        double pop = Convert.ToDouble(dTable.Rows[i][cmbPop.SelectedItem.ToString()]);
                        double diff = pop - soma_vars;

                        if (diff < 0)
                        {
                            erro_de_pop = true;
                            break;
                        }
                    }

                    if (erro_de_pop)
                    {
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("A variável de população deve ser maior que a soma das variáveis de caso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {

                    for (int i = 0; i < checkedListBox1.Items.Count; i++) if (checkedListBox1.GetItemChecked(i) == true) cSelected1++;
                    string[] strSelecionadasQuanti = new string[cSelected1];
                    cSelected1 = 0;
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        if (checkedListBox1.GetItemChecked(i) == true)
                        {
                            strSelecionadasQuanti[cSelected1] = checkedListBox1.Items[i].ToString();
                            cSelected1++;
                        }
                    }
                    strVariaveisSelecionadasQuantitativas = strSelecionadasQuanti;
                }

                #endregion

                // Alteração Gabriela 07 de outubro de 2011
                dblMoran = null; // Zerando as variáveis para não permanecerem da simulação anterior e dar erro na geração de outpus (Gabriela 7 de outubro)             
                dblGeary = null;
                dblGetis = null;
                dblTango = null;
                dblRogerson = null;
                // Fim da alteração Gabriela 07 de outubro

                if (cSelected1 == 0)
                {
                    throw new Exception ("Pelo menos uma variável deve ser selecionada");
                }
                else if (cSelected1 != 0 && !erro_de_pop)
                {
                    #region Encontra a matriz de vizinhança

                    IpeaGeo.RegressoesEspaciais.clsIpeaShape shp_dados = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
                    if (!m_matriz_W_pre_definida)
                    {
                        //Modifica o label
                        labelProgress.Text = "Gerando a matriz de vizinhanças...";
                        Application.DoEvents();

                        //Cria a vizinhnaça
                        IpeaGeo.RegressoesEspaciais.clsIpeaShape cps = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
                        int tipo_vizinhanca = -1;
                        if (cmbVizinhanca.SelectedItem.ToString() == "Queen" && chkNormal.Checked == false)
                        {
                            tipo_vizinhanca = 1;
                            shapeAlex.TipoVizinhanca = "Queen";
                            m_tipo_matriz_vizinhanca = "Queen";
                        }
                        else if (cmbVizinhanca.SelectedItem.ToString() == "Rook" && chkNormal.Checked == false)
                        {
                            tipo_vizinhanca = 2;
                            shapeAlex.TipoVizinhanca = "Rook";
                            m_tipo_matriz_vizinhanca = "Rook";
                        }
                        else if (cmbVizinhanca.SelectedItem.ToString() == "Queen" && chkNormal.Checked == true)
                        {
                            tipo_vizinhanca = 3;
                            shapeAlex.TipoVizinhanca = "Queen Normalizada";
                            m_tipo_matriz_vizinhanca = "Queen Normalizada";
                        }
                        else if (cmbVizinhanca.SelectedItem.ToString() == "Rook" && chkNormal.Checked == true)
                        {
                            tipo_vizinhanca = 4;
                            shapeAlex.TipoVizinhanca = "Rook Normalizada";
                            m_tipo_matriz_vizinhanca = "Rook Normalizada";
                        }

                        cps.DefinicaoVizinhos(ref shapeAlex, tipo_vizinhanca, ref progressBar1);
                        cps.AdicionaVizinhoProximo(ref shapeAlex);

                        IpeaGeo.RegressoesEspaciais.clsLinearRegressionModelsMLE m_geomle = new RegressoesEspaciais.clsLinearRegressionModelsMLE();
                        m_geomle.Shape = shapeAlex;
                        m_geomle.MatrizWesparsaFromVizinhosComPesos();

                        this.m_W_esparsa = m_geomle.Wesparsa;
                    }

                    #endregion

                    //Habilita o label
                    labelProgress.Text = "Inicializando procedimento...";
                    Application.DoEvents();

                    IpeaGeo.RegressoesEspaciais.BLDependenciaGlobal bld;
                    if (chkNormal.Checked)
                    {
                        //bld = new IpeaGEO.RegressoesEspaciais.BLDependenciaGlobal(shapeAlex,
                        //    IpeaGEO.RegressoesEspaciais.TipoMatrizVizinhanca.Normalizada);

                        bld = new IpeaGeo.RegressoesEspaciais.BLDependenciaGlobal(m_W_esparsa);
                    }
                    else
                    {
                        //bld = new IpeaGEO.RegressoesEspaciais.BLDependenciaGlobal(shapeAlex,
                        //    IpeaGEO.RegressoesEspaciais.TipoMatrizVizinhanca.Original);

                        bld = new IpeaGeo.RegressoesEspaciais.BLDependenciaGlobal(m_W_esparsa);
                    }

                    #region Indicadores Quantitativos

                    //Procedimento Variáveis QUANTITATIVAS
                    if (cSelected1 > 0)
                    {
                        double[,] mDadosQuanti = new double[dTable.Rows.Count, strVariaveisSelecionadasQuantitativas.Length];
                        for (int i = 0; i < strVariaveisSelecionadasQuantitativas.Length; i++)
                        {
                            for (int j = 0; j < dTable.Rows.Count; j++)
                            {
                                if (double.IsNaN(Convert.ToDouble(dTable.Rows[j][strVariaveisSelecionadasQuantitativas[i]])) == false)
                                {
                                    mDadosQuanti[j, i] = Convert.ToDouble(dTable.Rows[j][strVariaveisSelecionadasQuantitativas[i]]);
                                }
                                else
                                {
                                    mDadosQuanti[j, i] = double.NaN;
                                }
                            }
                        }

                        clsIndicesGlobaisDeDependenciaEspacial clsIndicesGlobais = new clsIndicesGlobaisDeDependenciaEspacial();
                        dblPvalorMoran = new double[mDadosQuanti.GetLength(1)];
                        dblMoran = new double[mDadosQuanti.GetLength(1)];
                        if (chkRogerson.Checked || chkTango.Checked)
                        {
                            dblPvalorMoran = new double[mDadosQuanti.GetLength(1) - 1];
                            dblMoran = new double[mDadosQuanti.GetLength(1) - 1];
                        }
                        if (chkMoran.Checked)
                        {
                            //Indice de Moran
                            clsUtilTools clt = new clsUtilTools();
                            double[,] y = new double[0, 0];
                            double Moran_teste = 0.0;
                            double p_valor_Moran_teste = 0.0;
                            double[] teste_normalidade_moran = new double[4];
                            dblMoran = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];
                            dblPvalorMoran = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];
                            dblEspMoran = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];
                            dblVarMoran = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];
                            dblPAnaliticoMoran = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];

                            for (int k = 0; k < strVariaveisSelecionadasQuantitativas.GetLength(0); k++)
                            {
                                y = clt.GetMatrizFromDataTable(dTable, strVariaveisSelecionadasQuantitativas[k]);
                                Moran_teste = bld.IndiceMoranGlobal(y);

                                progressBar1.Value = 0;
                                Application.DoEvents();
                                labelProgress.Text = "Simulações de Monte Carlo para o Índice de Moran...";
                                Application.DoEvents();

                                p_valor_Moran_teste = bld.IndiceMoranGeralPValor(y, (int)numSimula.Value, Moran_teste, ref progressBar1);

                                // inclusão1 gabriela 21 de outubro de 2011
                                progressBar1.Value = 0;
                                labelProgress.Text = "Teste de hipótese para dependência espacial da variável " + strVariaveisSelecionadasQuantitativas[k] + " ...";
                                // fim inclusão1 gabriela 21 de outubro de 2011

                                teste_normalidade_moran = bld.TesteMoranGlobal(y, ref progressBar1);

                                // inclusão2 gabriela 19 de outubro de 2011
                                progressBar1.Value = 0;
                                Application.DoEvents();
                                // fim inclusão2 gabriela 19 de outubro de 2011

                                dblMoran[k] = Moran_teste;
                                dblPvalorMoran[k] = p_valor_Moran_teste;
                                dblEspMoran[k] = teste_normalidade_moran[0];
                                dblVarMoran[k] = teste_normalidade_moran[1];
                                dblPAnaliticoMoran[k] = teste_normalidade_moran[3];
                            }

                            //------------------ Fim do teste Alex : 4 agosto de 2011
                            
                            output_Resultado_indicesglobais();

                            if (chkRogerson.Checked || chkTango.Checked)
                            {
                                double[] testepvalor = new double[mDadosQuanti.GetLength(1) - 1];
                                double[] testeindice = new double[mDadosQuanti.GetLength(1) - 1];
                                for (int i = 0; i < testeindice.Length; i++)
                                {
                                    testepvalor[i] = dblPvalorMoran[i];
                                    testeindice[i] = dblMoran[i];
                                }

                                dblPvalorMoran = new double[mDadosQuanti.GetLength(1) - 1];
                                dblMoran = new double[mDadosQuanti.GetLength(1) - 1];

                                dblMoran = testeindice;
                                dblPvalorMoran = testepvalor;
                            }
                        }

                        double[] dblMoranSimples = new double[mDadosQuanti.GetLength(1)];
                        double[] dblPvalorMoranSimples = new double[mDadosQuanti.GetLength(1)];
                        if (chkRogerson.Checked || chkTango.Checked)
                        {
                            dblMoranSimples = new double[mDadosQuanti.GetLength(1) - 1];
                            dblPvalorMoranSimples = new double[mDadosQuanti.GetLength(1) - 1];
                        }
                        if (chkMoranSimples.Checked)
                        {
                            //Indice de Moran simples
                            labelProgress.Text = "Calculando Índice de Moran simples...";
                            progressBar1.Value = 0;
                            Application.DoEvents();

                            dblMoranSimples = clsIndicesGlobais.IndiceDeMoranGlobalSimples(dTable, strIDmapa, mDadosQuanti, shapeAlex);

                            labelProgress.Text = "Simulações de Monte Carlo para o Índice de Moran simples...";
                            Application.DoEvents();

                            dblPvalorMoranSimples = clsIndicesGlobais.pValorIndiceDeMoranGlobalSimples(dTable, strIDmapa, mDadosQuanti, shapeAlex, (int)numSimula.Value, dblMoranSimples, ref progressBar1);

                            if (chkRogerson.Checked || chkTango.Checked)
                            {
                                double[] testepvalor = new double[mDadosQuanti.GetLength(1) - 1];
                                double[] testeindice = new double[mDadosQuanti.GetLength(1) - 1];
                                for (int i = 0; i < testeindice.Length; i++)
                                {
                                    testepvalor[i] = dblPvalorMoranSimples[i];
                                    testeindice[i] = dblMoranSimples[i];
                                }

                                dblPvalorMoranSimples = new double[mDadosQuanti.GetLength(1) - 1];
                                dblMoranSimples = new double[mDadosQuanti.GetLength(1) - 1];

                                dblMoranSimples = testeindice;
                                dblPvalorMoranSimples = testepvalor;
                            }
                        }

                        dblGeary = new double[mDadosQuanti.GetLength(1)];
                        dblPvalorGeary = new double[mDadosQuanti.GetLength(1)];
                        if (chkRogerson.Checked || chkTango.Checked)
                        {
                            dblGeary = new double[mDadosQuanti.GetLength(1) - 1];
                            dblPvalorGeary = new double[mDadosQuanti.GetLength(1) - 1];
                        }
                        if (chkGeary.Checked)
                        {                            
                            clsUtilTools clt = new clsUtilTools();
                            double[,] y = new double[0, 0];
                            double Geary_teste = 0.0;
                            double p_valor_Geary_teste = 0.0;
                            double[] teste_normalidade_geary = new double[4];

                            dblGeary = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];
                            dblPvalorGeary = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];
                            dblEspGeary = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];
                            dblVarGeary = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];
                            dblPAnaliticoGeary = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];
                            
                            for (int k = 0; k < strVariaveisSelecionadasQuantitativas.GetLength(0); k++)
                            {
                                y = clt.GetMatrizFromDataTable(dTable, strVariaveisSelecionadasQuantitativas[k]);
                                Geary_teste = bld.IndiceGearyGlobal(y);
                                // inclusão1 gabriela 18 de outubro de 2011
                                progressBar1.Value = 0;
                                Application.DoEvents();
                                labelProgress.Text = "Simulações de Monte Carlo para o Índice de Geary da variável " + strVariaveisSelecionadasQuantitativas[k] + " ...";
                                Application.DoEvents();
                                // fim inclusão1 gabriela 18 de outubro de 2011
                                p_valor_Geary_teste = bld.pValorIndiceDeGeary(y, (int)numSimula.Value, Geary_teste, ref progressBar1);
                                // inclusão1 gabriela 19 de outubro de 2011
                                progressBar1.Value = 0;
                                labelProgress.Text = "Teste de hipótese para dependência espacial da variável " + strVariaveisSelecionadasQuantitativas[k] + " ...";
                                // fim inclusão1 gabriela 19 de outubro de 2011
                                teste_normalidade_geary = bld.TesteGearyGlobal(y, ref progressBar1);
                                // inclusão2 gabriela 19 de outubro de 2011
                                progressBar1.Value = 0;
                                Application.DoEvents();
                                // fim inclusão2 gabriela 19 de outubro de 2011
                                dblGeary[k] = Geary_teste;
                                dblPvalorGeary[k] = p_valor_Geary_teste;
                                dblEspGeary[k] = teste_normalidade_geary[0];
                                dblVarGeary[k] = teste_normalidade_geary[1];
                                dblPAnaliticoGeary[k] = teste_normalidade_geary[3];
                            }                    

                            output_Resultado_indicesglobais();

                            if (chkRogerson.Checked || chkTango.Checked)
                            {
                                double[] testepvalor = new double[mDadosQuanti.GetLength(1) - 1];
                                double[] testeindice = new double[mDadosQuanti.GetLength(1) - 1];
                                for (int i = 0; i < testeindice.Length; i++)
                                {
                                    testepvalor[i] = dblPvalorGeary[i];
                                    testeindice[i] = dblGeary[i];
                                }

                                dblPvalorGeary = new double[mDadosQuanti.GetLength(1) - 1];
                                dblGeary = new double[mDadosQuanti.GetLength(1) - 1];

                                dblGeary = testeindice;
                                dblPvalorGeary = testepvalor;
                            }
                        }

                        dblGetis = new double[mDadosQuanti.GetLength(1)];
                        dblPvalorGetis = new double[mDadosQuanti.GetLength(1)];
                        if (chkRogerson.Checked || chkTango.Checked)
                        {
                            dblGetis = new double[mDadosQuanti.GetLength(1) - 1];
                            dblPvalorGetis = new double[mDadosQuanti.GetLength(1) - 1];
                        }
                        if (chkGetis.Checked)
                        {
                            Application.DoEvents();
                            
                            clsUtilTools clt = new clsUtilTools();
                            double[,] y = new double[0, 0];
                            double Getis_teste = 0.0;
                            double p_valor_Getis_teste = 0.0;

                            double[] teste_normalidade_getis = new double[4];
                            dblGetis = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];
                            dblPvalorGetis = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];
                            dblEspGetis = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];
                            dblVarGetis = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];
                            dblPAnaliticoGetis = new double[strVariaveisSelecionadasQuantitativas.GetLength(0)];

                            for (int k = 0; k < strVariaveisSelecionadasQuantitativas.GetLength(0); k++)
                            {
                                y = clt.GetMatrizFromDataTable(dTable, strVariaveisSelecionadasQuantitativas[k]);
                                Getis_teste = bld.IndiceGetisOrdGiGlobal(y);
                                teste_normalidade_getis = bld.TesteGetisGlobal(y, ref progressBar1);
                                progressBar1.Value = 0;
                                Application.DoEvents();
                                labelProgress.Text = "Simulações de Monte Carlo para o Índice de Getis-Ord Gi...";
                                Application.DoEvents();
                                
                                p_valor_Getis_teste = bld.pValorGetisOrdGi(y, (int)numSimula.Value, Getis_teste, ref progressBar1);

                                // inclusão1 gabriela 19 de outubro de 2011
                                progressBar1.Value = 0;
                                labelProgress.Text = "Teste de hipótese para dependência espacial da variável " + strVariaveisSelecionadasQuantitativas[k] + " ...";
                                // fim inclusão1 gabriela 19 de outubro de 2011
                                
                                dblGetis[k] = Getis_teste;
                                dblPvalorGetis[k] = p_valor_Getis_teste;
                                dblEspGetis[k] = teste_normalidade_getis[0];
                                dblVarGetis[k] = teste_normalidade_getis[1];
                                dblPAnaliticoGetis[k] = teste_normalidade_getis[3];
                            }

                            //------------------ Fim do teste Gabriela : 11 de agosto de 2011                            

                            output_Resultado_indicesglobais();

                            if (chkRogerson.Checked || chkTango.Checked)
                            {
                                double[] testepvalor = new double[mDadosQuanti.GetLength(1) - 1];
                                double[] testeindice = new double[mDadosQuanti.GetLength(1) - 1];
                                for (int i = 0; i < testeindice.Length; i++)
                                {
                                    testepvalor[i] = dblPvalorGetis[i];
                                    testeindice[i] = dblGetis[i];
                                }

                                dblPvalorGetis = new double[mDadosQuanti.GetLength(1) - 1];
                                dblGetis = new double[mDadosQuanti.GetLength(1) - 1];

                                dblGetis = testeindice;
                                dblPvalorGetis = testepvalor;
                            }
                        }

                        #endregion

                        #region Indicadores Qualitativos

                        dblTango = new double[mDadosQuanti.GetLength(1)];
                        dblPvalorTango = new double[mDadosQuanti.GetLength(1)];
                        if (chkTango.Checked)
                        {
                            //Indice de Tango
                            labelProgress.Text = "Calculando Índice de Tango...";
                            progressBar1.Value = 0;
                            Application.DoEvents();

                            //=================================== Modificação Gabriela 09.11.2011

                            clsUtilTools clt = new clsUtilTools();
                            double[,] y = new double[0, 0];
                            double Tango_teste = 0.0;
                            double p_valor_Tango_teste = 0.0;
                            int Total = strVariaveisSelecionadasQuantitativas.GetLength(0);
                            object[] teste_normalidade_tango = new object[4];
                            dblTango = new double[Total-1];
                            dblPvalorTango = new double[Total-1];
                            dblEspTango = new object[Total-1];
                            dblVarTango = new object[Total-1];
                            dblPAnaliticoTangoLance = new object[Total-1];
                            dblPAnaliticoTangoRogerson = new object[Total - 1];

                            for (int k = 0; k < (Total - 1); k++)
                            {
                                y = clt.GetMatrizFromDataTable(dTable, strVariaveisSelecionadasQuantitativas[k]);
                                double[,] pop = clt.GetMatrizFromDataTable(dTable, strVariaveisSelecionadasQuantitativas[(Total-1)]);
                                y = clt.Concateh(y, pop);
                                Tango_teste = bld.IndiceDeTango(y);
                                progressBar1.Value = 0;
                                Application.DoEvents();
                                labelProgress.Text = "Simulações de Monte Carlo para o Índice de Tango...";
                                Application.DoEvents();


                                p_valor_Tango_teste = bld.pValorTango(y, (int)numSimula.Value, Tango_teste, ref progressBar1);

                                // inclusão1 gabriela 19 de outubro de 2011
                                progressBar1.Value = 0;
                                labelProgress.Text = "Teste de hipótese para dependência espacial da variável " + strVariaveisSelecionadasQuantitativas[k] + " ...";

                                if (ckRogersonTangoUsaQuiQuadrado.Checked == true)
                                {
                                    teste_normalidade_tango = bld.TesteTangoGlobal(y, ref progressBar1);
                                    dblEspTango[k] = teste_normalidade_tango[0];
                                    dblVarTango[k] = teste_normalidade_tango[1];
                                    dblPAnaliticoTangoLance[k] = teste_normalidade_tango[2];
                                    dblPAnaliticoTangoRogerson[k] = teste_normalidade_tango[3];
                                }
                                dblTango[k] = Tango_teste;
                                dblPvalorTango[k] = p_valor_Tango_teste;

                            }           

                            output_Resultado_indicesglobais();

                            //=================================== Fim Modificação Gabriela 09.11.2011
                        }

                        dblRogerson = new double[mDadosQuanti.GetLength(1)];
                        dblPvalorRogerson = new double[mDadosQuanti.GetLength(1)];
                        if (chkRogerson.Checked)
                        {
                            //Indice de Rogerson
                            labelProgress.Text = "Calculando Índice de Rogerson...";
                            progressBar1.Value = 0;
                            Application.DoEvents();

                            //=================================== Modificação Gabriela 09.11.2011

                            clsUtilTools clt = new clsUtilTools();
                            double[,] y = new double[0, 0];
                            double Rogerson_teste = 0.0;
                            double p_valor_Rogerson_teste = 0.0;
                            int Total = strVariaveisSelecionadasQuantitativas.GetLength(0);
                            object[] teste_normalidade_rogerson = new object[4];
                            dblRogerson = new double[Total-1];
                            dblPvalorRogerson = new double[Total-1];
                            dblEspRogerson = new object[Total - 1];
                            dblVarRogerson = new object[Total - 1];
                            dblPAnaliticoRogersonLance = new object[Total - 1];
                            dblPAnaliticoRogersonRogerson = new object[Total - 1];

                            for (int k = 0; k < (Total - 1); k++)
                            {
                                y = clt.GetMatrizFromDataTable(dTable, strVariaveisSelecionadasQuantitativas[k]);
                                double[,] pop = clt.GetMatrizFromDataTable(dTable, strVariaveisSelecionadasQuantitativas[(Total-1)]);
                                y = clt.Concateh(y, pop);
                                Rogerson_teste = bld.IndiceDeRogerson(y);

                                progressBar1.Value = 0;
                                Application.DoEvents();
                                labelProgress.Text = "Simulações de Monte Carlo para o Índice de Rogerson...";
                                Application.DoEvents();

                                p_valor_Rogerson_teste = bld.pValorRogerson(y, (int)numSimula.Value, Rogerson_teste, ref progressBar1);

                                // inclusão1 gabriela 19 de outubro de 2011
                                progressBar1.Value = 0;
                                labelProgress.Text = "Teste de hipótese para dependência espacial da variável " + strVariaveisSelecionadasQuantitativas[k] + " ...";
                                if (ckRogersonTangoUsaQuiQuadrado.Checked == true)
                                {
                                    teste_normalidade_rogerson = bld.TesteRogersonGlobal(y, ref progressBar1);
                                    dblEspRogerson[k] = teste_normalidade_rogerson[0];
                                    dblVarRogerson[k] = teste_normalidade_rogerson[1];
                                    dblPAnaliticoRogersonLance[k] = teste_normalidade_rogerson[2];
                                    dblPAnaliticoRogersonRogerson[k] = teste_normalidade_rogerson[3];
                                }
                                dblRogerson[k] = Rogerson_teste;
                                dblPvalorRogerson[k] = p_valor_Rogerson_teste;
                            }                                     

                            output_Resultado_indicesglobais();
                            
                            //=================================== Fim Modificação Gabriela 09.11.2011
                        }

                        if (chkRogerson.Checked || chkTango.Checked)
                        {
                            string[] testevar = new string[strVariaveisSelecionadasQuantitativas.Length - 1];

                            for (int i = 0; i < strVariaveisSelecionadasQuantitativas.Length - 1; i++)
                            {
                                testevar[i] = strVariaveisSelecionadasQuantitativas[i];

                            }

                            strVariaveisSelecionadasQuantitativas = new string[strVariaveisSelecionadasQuantitativas.Length - 1];
                            strVariaveisSelecionadasQuantitativas = testevar;
                        }

                        #endregion

                        #region Passando informações para o relatório

                        if (cmbPop.SelectedIndex >= 0)
                        {
                            strPOPULACAO = cmbPop.SelectedItem.ToString();
                        }
                        else
                        {
                            strPOPULACAO = "Não se aplica";
                        }

                        intNumeroSim = (int)numSimula.Value;
                        strTipoPeso = shapeAlex.TipoVizinhanca;

                        dblGearyI = dblGeary;
                        dblpGearyI = dblPvalorGeary;

                        dblGetisI = dblGetis;
                        dblpGetisI = dblPvalorGetis;

                        dblIMORAN = dblMoran;
                        dblpIMORAN = dblPvalorMoran;

                        dblIMORANSIMPLES = dblMoranSimples;
                        dblpIMORANSIMPLES = dblPvalorMoranSimples;

                        dblTangoI = dblTango;
                        dblpTangoI = dblPvalorTango;

                        dblRogersonI = dblRogerson;
                        dblpRogersonI = dblPvalorRogerson;

                        blRelatorio = chkRelatorio.Checked;

                        #endregion
                    }

                    //OK
                    if (chkRelatorio.Checked)
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        if (!tabControl1.TabPages.Contains(tabPage2))
                        {
                            tabControl1.TabPages.Add(tabPage2);
                        }
                        tabControl1.SelectedTab = tabPage2;
                    }

                    this.Cursor = Cursors.Default;
                    Application.DoEvents();
                    progressBar1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTango.Checked || chkRogerson.Checked)
            {
                cmbPop.Enabled = true;
                ckRogersonTangoUsaQuiQuadrado.Enabled = true;

                cmbPop.Items.Clear();
                cmbPop.Items.AddRange(strVariaveisQuantitativas);
                cmbPop.SelectedIndex = 0;
            }

            if (!chkTango.Checked && !chkRogerson.Checked)
            {
                ckRogersonTangoUsaQuiQuadrado.Enabled = false;
                cmbPop.Items.Clear();
                cmbPop.Enabled = false;
            }

            try
            {
                if (chkTango.Checked == true)
                {
                    btnOk.Enabled = true;
                }
                else
                {
                    if (chkGeary.Checked == false && chkGetis.Checked == false && chkRogerson.Checked == false && chkMoran.Checked == false)
                    {
                        btnOk.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void chkRogerson_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTango.Checked || chkRogerson.Checked)
            {
                cmbPop.Enabled = true;
                ckRogersonTangoUsaQuiQuadrado.Enabled = true;

                cmbPop.Items.Clear();
                cmbPop.Items.AddRange(strVariaveisQuantitativas);
                cmbPop.SelectedIndex = 0;
            }

            if (!chkTango.Checked && !chkRogerson.Checked)
            {
                ckRogersonTangoUsaQuiQuadrado.Enabled = false;
                cmbPop.Items.Clear();
                cmbPop.Enabled = false;
            }

            try
            {
                if (chkRogerson.Checked == true)
                {
                    btnOk.Enabled = true;
                }
                else
                {
                    if (chkGeary.Checked == false && chkGetis.Checked == false && chkTango.Checked == false && chkMoran.Checked == false)
                    {
                        btnOk.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chkMoran_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkMoran.Checked == true)
                {
                    btnOk.Enabled = true;
                }
                else
                {
                    if (chkGeary.Checked == false && chkGetis.Checked == false && chkRogerson.Checked == false && chkTango.Checked == false)
                    {
                        btnOk.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chkGeary_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkGeary.Checked == true)
                {
                    btnOk.Enabled = true;
                }
                else
                {
                    if (chkMoran.Checked == false && chkGetis.Checked == false && chkRogerson.Checked == false && chkTango.Checked == false)
                    {
                        btnOk.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chkGetis_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkGetis.Checked == true)
                {
                    btnOk.Enabled = true;
                }
                else
                {
                    if (chkMoran.Checked == false && chkGeary.Checked == false && chkRogerson.Checked == false && chkTango.Checked == false)
                    {
                        btnOk.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void chkMoran_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            tip = new ToolTip();
            tip.Show(@"O Índice de Moran mede a correlação espacial de uma variável y e é similar em
vários aspectos ao índice de correlação de pearson.
Seu valor varia de -1 a 1. Valores próximos de zero indicam a inexistência de autocorrelação espacial
significativa entre os valores dos objetos e seus vizinhos.
Valores positivos para o índice, indicam autocorrelação espacial positiva, ou
seja, o valor do atributo de um objeto tende a ser semelhante aos valores dos seus
vizinhos. Valores negativos para o índice, por sua vez, indicam autocorrelação negativa.", this, hlpevent.MousePos.X, hlpevent.MousePos.Y);

        }

        private ToolTip tip = new ToolTip();

        private void chkGeary_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            tip = new ToolTip();
            tip.Show(@"O C de Geary é uma medida de autocorrelação. O valor do C de Geary varia
entre 0 e 2. Quando o índice é igual a 1 significa ausência de autocorrelação espacial.
Valores pequenos menores do que 1 indicam autocorrelação espacial positiva, já
valores maiores do que 1 indicam autocorrelação negativa.
C de Geary é inversamente proporcional ao I de Moran, mas não identicamente.
Enquanto o I de Moran é uma medida global o C de Geary é mais sensitivo
a medidas locais, apesar de ser formalmente considerado um índice global.", this, hlpevent.MousePos.X, hlpevent.MousePos.Y);
        }

        private void chkGetis_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            tip = new ToolTip();
            tip.Show(@"O índice global Getis-Ord mostra o quão concentrado estão os dados
para uma determinada área de estudo. A hipótese nula do índice global Getis-Ord é de que
não há conglomerados espaciais entre os valores. Quando o valor escore z é alto e o p-valor
pequeno, a hipótese nula não deverá ser aceita. Caso o escore z seja significativo e positivo,
há indícios de que os valores altos estão aglomerados, já se o escore z for significativo mas negativo
há indícios de que os valores baixos estão aglomerados.", this, hlpevent.MousePos.X, hlpevent.MousePos.Y);
        }

        private void chkTango_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            tip = new ToolTip();
            tip.Show(@"A estatística de Tango é uma média ponderada
das covariâncias dos desvios entre frequências observadas e esperadas,
para todos os pares de pontos.", this, hlpevent.MousePos.X, hlpevent.MousePos.Y);
        }

        private void chkRogerson_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            tip = new ToolTip();
            tip.Show(@"A estatística R de Rogerson é um caso especial
da estatística de Tango, onde os pesos nesta são divididos pela raiz
quadrada do produto entre as frequências esperadas de cada par de pontos.", this, hlpevent.MousePos.X, hlpevent.MousePos.Y);
        }

        private void frmDependenciaGlobal_MouseMove(object sender, MouseEventArgs e)
        {
            tip.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void userControlRichTextOutput1_Load(object sender, EventArgs e)
        {
        }

        #region output rsultados

        private string tabcontrol1 = "";
        public string ResultadoEstimacao2
        {
            get { return tabcontrol1; }
            set { this.tabcontrol1 = value; }
        }

        public void output_Resultado_indicesglobais()
        {
            progressBar1.Visible = true;
            clsUtilTools clt = new clsUtilTools();
            double[,] mDadosQuanti = new double[dTable.Rows.Count, strVariaveisSelecionadasQuantitativas.Length];
            for (int i = 0; i < strVariaveisSelecionadasQuantitativas.Length; i++)
            {
                for (int j = 0; j < dTable.Rows.Count; j++)
                {
                    if (double.IsNaN(Convert.ToDouble(dTable.Rows[j][strVariaveisSelecionadasQuantitativas[i]])) == false)
                    {
                        mDadosQuanti[j, i] = Convert.ToDouble(dTable.Rows[j][strVariaveisSelecionadasQuantitativas[i]]);
                    }
                    else
                    {
                        mDadosQuanti[j, i] = double.NaN;
                    }
                }
            }

            clsIndicesGlobaisDeDependenciaEspacial clsIndicesGlobais = new clsIndicesGlobaisDeDependenciaEspacial();

            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                if (chkMoran.Checked & (dblMoran != null))
                {
                    string out_text = "==============================================================================================================\n\n";

                    out_text += "Resultado do Índice de dependência Global - Moran \n\n";
                    out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                    out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                    out_text += "Variável dos dados: " + strVariaveisSelecionadasQuantitativas[i] + "\n\n";

                    out_text += "Numero de observacoes: " + dTable.Rows.Count + "\n";
                    out_text += "Índice de Moran: " + clt.Double2Texto(dblMoran[i], 6) + "\n";
                    out_text += "Esperança: " + clt.Double2Texto(dblEspMoran[i], 6) + "\n";
                    out_text += "Variância: " + clt.Double2Texto(dblVarMoran[i], 6) + "\n\n";
                    out_text += "P-valor Empírico via Simulação Monte Carlo: " + clt.Double2Texto(dblPvalorMoran[i], 6) + "\n";
                    out_text += "P-valor Analítico segundo uma distribuição Normal: " + clt.Double2Texto(dblPAnaliticoMoran[i], 6) + "\n\n";

                    out_text += "Interpretação do Índice de Moran:" + "\n";

                    out_text += "Seu valor varia de -1 a 1. Valores próximos de zero, indicam a inexistência de autocorrelação espacial" + "\n" +
                                "signicativa entre os valores dos objetos e seus vizinhos. Valores positivos para o índice, indicam " + "\n" +
                                "autocorrelação espacial positiva, ou seja, o valor do atributo de um objeto tende a ser semelhante " + "\n" +
                                "aos valores dos seus vizinhos. Valores negativos para o índice, por sua vez, indicam autocorrelação negativa." + "\n\n";

                    m_output_text += out_text;

                    userControlRichTextOutput1.Texto = m_output_text;

                    //progressBar1.Visible = false;
                    progressBar1.Value = 0;
                    Application.DoEvents();
                    //labelProgress.Visible = false;
                    labelProgress.Text = "Processo de estimação finalizado.";

                }

                if (chkGeary.Checked & (dblGeary != null))
                {
                    string out_text = "==============================================================================================================\n\n";

                    out_text += "Resultado do Índice de dependência Global - Geary \n\n";
                    out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                    out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                    out_text += "Variável dos dados: " + strVariaveisSelecionadasQuantitativas[i] + "\n\n";

                    out_text += "Numero de observacoes: " + dTable.Rows.Count + "\n";
                    out_text += "Índice de Geary: " + clt.Double2Texto(dblGeary[i], 6) + "\n";
                    out_text += "Esperança: " + clt.Double2Texto(dblEspGeary[i], 6) + "\n";
                    out_text += "Variância: " + clt.Double2Texto(dblVarGeary[i], 6) + "\n\n";
                    out_text += "P-valor Empírico via Simulação Monte Carlo: " + clt.Double2Texto(dblPvalorGeary[i], 6) + "\n";
                    out_text += "P-valor Analítico segundo uma distribuição Normal: " + clt.Double2Texto(dblPAnaliticoGeary[i], 6) + "\n\n";

                    out_text += "Interpretação do Índice de Geary:" + "\n";
                    out_text += "O C de Geary é uma medida de autocorrelação. O valor do C de Geary varia entre 0 e 2. Quando o índice é" + "\n" +
                                "igual a 1 signfica ausência de autocorrelação espacial. Valores pequenos menores do que 1 indicam" + "\n" +
                                "autocorrelacão espacial positiva, já valores maiores do que 1 indicam autocorrelacão negativa." + "\n\n";

                    m_output_text += out_text;

                    userControlRichTextOutput1.Texto = m_output_text;

                    //progressBar1.Visible = false;
                    progressBar1.Value = 0;
                    Application.DoEvents();
                    //labelProgress.Visible = false;
                    labelProgress.Text = "Processo de estimação finalizado.";
                }

                if (chkGetis.Checked & (dblGetis != null))
                {
                    string out_text = "==============================================================================================================\n\n";

                    out_text += "Resultado do Índice de dependência Global - Getis-Ord Gi \n\n";
                    out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                    out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                    out_text += "Variável dos dados: " + strVariaveisSelecionadasQuantitativas[i] + "\n\n";


                    out_text += "Numero de observacoes: " + dTable.Rows.Count + "\n";
                    out_text += "Índice de Getis-Ord Gi: " + clt.Double2Texto(dblGetis[i], 6) + "\n";
                    out_text += "Esperança: " + clt.Double2Texto(dblEspGetis[i], 6) + "\n";
                    out_text += "Variância: " + clt.Double2Texto(dblVarGetis[i], 6) + "\n\n";
                    out_text += "P-valor Empírico via Simulação Monte Carlo: " + clt.Double2Texto(dblPvalorGetis[i], 6) + "\n";
                    out_text += "P-valor Analítico segundo uma distribuição Normal: " + clt.Double2Texto(dblPAnaliticoGetis[i], 6) + "\n\n";

                    out_text += "Interpretação do Índice de Getis-Ord Gi:" + "\n";
                    out_text += "O índice global Getis-Ord mostra o quão concentrado estão os dados para uma determinada área de estudo." + "\n" +
                                 "A hipótese nula do índice global Getis-Ord é de que não há conglomerados espaciais entre os valores." + "\n" +
                                 "Quando o valor escore z é alto e o p-valor pequeno, a hipótese nula não deverá ser aceita. Caso o escore z" + "\n" +
                                 "seja significativo e positivo, há indícios de que os valores altos estão aglomerados, já se o escore z" + "\n" +
                                 "for significativo mas negativo há indícios de que os valores baixos estão aglomerados." + "\n\n";
                    
                    m_output_text += out_text;

                    userControlRichTextOutput1.Texto = m_output_text;

                    //progressBar1.Visible = false;
                    progressBar1.Value = 0;
                    Application.DoEvents();
                    //labelProgress.Visible = false;
                    labelProgress.Text = "Processo de estimação finalizado.";
                }

                if (chkTango.Checked & (dblTango != null))
                {
                    string out_text = "==============================================================================================================\n\n";

                    out_text += "Resultado do Índice de dependência Global - Tango \n\n";
                    out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                    out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                    out_text += "Variável dos dados: " + strVariaveisSelecionadasQuantitativas[i] + "\n\n";


                    out_text += "Numero de observacoes: " + dTable.Rows.Count + "\n";
                    out_text += "Índice de Tango: " + clt.Double2Texto(dblTango[i], 6) + "\n";
                    out_text += "P-valor Empírico via Simulação Monte Carlo: " + clt.Double2Texto(dblPvalorTango[i], 6) + "\n";

                    if (ckRogersonTangoUsaQuiQuadrado.Checked == true)
                    {
                        out_text += "Esperança: " + clt.Double2Texto(dblEspTango[i], 6) + "\n";
                        out_text += "Variância: " + clt.Double2Texto(dblVarTango[i], 6) + "\n\n";
                        out_text += "P-valor Analítico segundo uma distribuição Qui-quadrado[1]: " + clt.Double2Texto(dblPAnaliticoTangoLance[i], 6) + "\n";
                        out_text += "P-valor Analítico segundo uma distribuição Qui-quadrado[2]: " + clt.Double2Texto(dblPAnaliticoTangoRogerson[i], 6) + "\n\n";
                    }
                    
                    out_text += "Interpretação do Índice de Tango:" + "\n";

                    out_text += "A estatística de Tango é uma média ponderada das covariâncias dos desvios entre frequências" + "\n" +
                                 "observadas e esperadas, para todos os pares de pontos." + "\n\n";

                    if (ckRogersonTangoUsaQuiQuadrado.Checked == true)
                    {
                        out_text += "Referências:" + "\n";

                        out_text += "[1] Waller, L. A. ; Gotway, C. A. (2004). Applied Spatial Statistics for Public Health Data. New York: John Wiley and Sons." + "\n";
                        out_text += "[2] Rogerson P. A. The detection of clusters using a spatial version of the chi-square goodness-of-fit statistic. Geographical Analysis. 1999;31:130–147." + "\n\n";
                    }
                    m_output_text += out_text;

                    userControlRichTextOutput1.Texto = m_output_text;

                    //progressBar1.Visible = false;
                    progressBar1.Value = 0;
                    Application.DoEvents();
                    //labelProgress.Visible = false;
                    labelProgress.Text = "Processo de estimação finalizado.";
                }

                if (chkRogerson.Checked & (dblRogerson != null))
                {
                    string out_text = "==============================================================================================================\n\n";

                    out_text += "Resultado do Índice de dependência Global - Rogerson \n\n";
                    out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                    out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                    out_text += "Variável dos dados: " + strVariaveisSelecionadasQuantitativas[i] + "\n\n";


                    out_text += "Numero de observacoes: " + dTable.Rows.Count + "\n";
                    out_text += "Índice de Rogerson: " + clt.Double2Texto(dblRogerson[i], 6) + "\n";
                    out_text += "P-valor Empírico via Simulação Monte Carlo: " + clt.Double2Texto(dblPvalorRogerson[i], 6) + "\n";

                    if (ckRogersonTangoUsaQuiQuadrado.Checked == true)
                    {
                        out_text += "Esperança: " + clt.Double2Texto(dblEspRogerson[i], 6) + "\n";
                        out_text += "Variância: " + clt.Double2Texto(dblVarRogerson[i], 6) + "\n\n";
                        out_text += "P-valor Analítico segundo uma distribuição Qui-quadrado[1]: " + clt.Double2Texto(dblPAnaliticoRogersonLance[i], 6) + "\n";
                        out_text += "P-valor Analítico segundo uma distribuição Qui-quadrado[2]: " + clt.Double2Texto(dblPAnaliticoRogersonRogerson[i], 6) + "\n\n";
                    }                 
                    
                    out_text += "Interpretação do Índice de Rogerson:" + "\n";

                    out_text += "A estatística R de Rogerson é um caso especial da estatística de Tango, onde os pesos nesta " + "\n" +
                                 "são divididos pela raiz quadrada do produto entre as frequências esperadas de cada par de pontos." + "\n\n";

                    if (ckRogersonTangoUsaQuiQuadrado.Checked == true)
                    {
                        out_text += "Referências:" + "\n";

                        out_text += "[1] Waller, L. A. ; Gotway, C. A. (2004). Applied Spatial Statistics for Public Health Data. New York: John Wiley and Sons." + "\n";
                        out_text += "[2] Rogerson P. A. The detection of clusters using a spatial version of the chi-square goodness-of-fit statistic. Geographical Analysis. 1999;31:130–147." + "\n\n";
                    }

                    m_output_text += out_text;

                    userControlRichTextOutput1.Texto = m_output_text;

                    //progressBar1.Visible = false;
                    progressBar1.Value = 0;
                    Application.DoEvents();
                    //labelProgress.Visible = false;
                    labelProgress.Text = "Processo de estimação finalizado.";
                }
                progressBar1.Visible = false;
            }
            if (ckbLimpaJanelaOutput.Checked)
            {
                m_output_text = "";
            }
        }
    }

    #endregion
}