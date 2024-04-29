using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using IpeaGeo.BLogic;
using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Forms
{
    public partial class frmIndicadoresSegregacao : Form
    {
        public frmIndicadoresSegregacao()
        {
            InitializeComponent();
        }

        #region Métodos para a escolha do número de conglomerados

        private double[] dblPseudoT;
        public double[] PseudoT
        {
            get
            {
                return dblPseudoT;
            }
            set
            {
                dblPseudoT = value;
            }
        }
        
        private double[] dblPseudoF;
        public double[] PseudoF
        {
            get
            {
                return dblPseudoF;
            }
            set
            {
                dblPseudoF = value;
            }
        }
        
        private double[] dblRSquare;
        public double[] RSquare
        {
            get
            {
                return dblRSquare;
            }
            set
            {
                dblRSquare = value;
            }
        }
        
        private double[] dblRSquarePartial;
        public double[] RSquarePartial
        {
            get
            {
                return dblRSquarePartial;
            }
            set
            {
                dblRSquarePartial = value;
            }
        }
        
        private double[] dblRSquareExpected;
        public double[] RSquareExpected
        {
            get
            {
                return dblRSquareExpected;
            }
            set
            {
                dblRSquareExpected = value;
            }
        }

        private double[] dblCCC;
        public double[] CCC
        {
            get
            {
                return dblCCC;
            }
            set
            {
                dblCCC = value;
            }
        }

        #endregion

        private clsIpeaShape shapeAlex;
        public clsIpeaShape EstruturaShape
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
        
        private string[] strVariaveis;
        public string[] Variaveis
        {
            get
            {
                return strVariaveis;
            }
            set
            {
                strVariaveis = value;
            }
        }

        private bool HouveSegregacao_;
        public bool HouveSegregacao
        {
            get
            {
                return HouveSegregacao_;
            }
            set
            {
                HouveSegregacao_ = value;
            }
        }
        
        private bool relatoriosegregacao_;
        public bool relatoriosegregacao
        {
            get
            {
                return relatoriosegregacao_;
            }
            set
            {
                relatoriosegregacao_ = value;
            }
        }
        
        private string[] strX;
        public string[] strX_
        {
            get
            {
                return strX;
            }
            set
            {
                strX = value;
            }
        }
        
        private string[] strY;
        public string[] strY_
        {
            get
            {
                return strY;
            }
            set
            {
                strY = value;
            }
        }

        private string[] strVariaveisSelecionadas;
        public string[] VariaveisSelecionadas
        {
            get
            {
                return strVariaveisSelecionadas;
            }
            set
            {
                strVariaveisSelecionadas = value;
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
        
        private string strNumCluster;
        public string NumeroDeConglomerados
        {
            get
            {
                return strNumCluster;
            }
            set
            {
                strNumCluster = value;
            }
        }
        
        private bool blEspacial;
        public bool IsSpatialCluster
        {
            get
            {
                return blEspacial;
            }
            set
            {
                blEspacial = value;
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
        
        private string strDistancia;
        public string Distancia
        {
            get
            {
                return strDistancia;
            }
            set
            {
                strDistancia = value;
            }
        }

        private string strMetodo;
        public string Metodo
        {
            get
            {
                return strMetodo;
            }
            set
            {
                strMetodo = value;
            }
        }
        
        private string strFatorMinkowsky;
        public string FatorMinkowsky
        {
            get
            {
                return strFatorMinkowsky;
            }
            set
            {
                strFatorMinkowsky = value;
            }
        }
        
        public string[] strVetorPesos;
        public string[] VetorDePesos
        {
            get
            {
                return strVetorPesos;
            }
            set
            {
                strVetorPesos = value;
            }
        }
        
        public double[,] matrizIndicesSegregacao_;
        public double[,] matrizIndicesSegregacao
        {
            get
            {
                return matrizIndicesSegregacao_;
            }
            set
            {
                matrizIndicesSegregacao_ = value;
            }
        }
        
        public string[] nomesIndicesSegregacao_;
        public string[] nomesIndicesSegregacao
        {
            get
            {
                return nomesIndicesSegregacao_;
            }
            set
            {
                nomesIndicesSegregacao_ = value;
            }
        }
        
        public bool multi_=false;
        public bool multi
        {
            get
            {
                return multi_;
            }
            set
            {
                multi_ = value;
            }
        }
        
        public string[] nomesVariaveis_;
        public string[] nomesVariaveis
        {
            get
            {
                return nomesVariaveis_;
            }
            set
            {
                nomesVariaveis_ = value;
            }
        }

        public struct GradientColor
        {
            public Color Color1;
            public Color Color2;
            public string ColorName;

            public GradientColor(string colorname, Color color1, Color color2)
            {
                ColorName = colorname;
                Color1 = color1;
                Color2 = color2;
            }
            public string Nome
            {
                get { return ColorName; }
            }

        }
        
        private Brush[] classeCor;
        public Brush[] CoresParaMapa
        {
            get
            {
                return classeCor;
            }
            set
            {
                classeCor = value;
            }
        }
        
        private string[] strCoresRGB;
        public string[] CoresRGB
        {
            get
            {
                return strCoresRGB;
            }
            set
            {
                strCoresRGB = value;
            }
        }

        private string stringEML;
        public string strEML
        {
            get
            {
                return stringEML;
            }
        }

        private Brush[,] coresVetor = new Brush[110, 2];
        private Color[,] coresVetor2 = new Color[110, 2];

        private void PopulaDataGridView(ref DataGridView dataGridView1, int iVariaveis)
        {
            try
            {
                //Cria as colunas do DataGridView
                DataGridViewTextBoxColumn txtbox = new DataGridViewTextBoxColumn();
                txtbox = new DataGridViewTextBoxColumn();
                txtbox.Width = 300;
                txtbox.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                txtbox.ReadOnly = true;
                txtbox.HeaderText = "Variável";
                txtbox.DisplayIndex = 2;
                dataGridView1.Columns.Insert(0, txtbox);

                DataGridViewComboBoxColumn chkbox = new DataGridViewComboBoxColumn();
                chkbox = new DataGridViewComboBoxColumn();
                //chkbox.Width = 50;
                chkbox.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                chkbox.FlatStyle = FlatStyle.Popup;
                chkbox.HeaderText = "Tipo de variável";
                chkbox.DisplayIndex = 2;
                chkbox.Items.Add("Contínuas");
                chkbox.Items.Add("Binárias");
                chkbox.Items.Add("Categóricas");
                chkbox.Items.Add("Ordinais");
                //dataGridView1.Columns.Insert(1, chkbox);

                DataGridViewCheckBoxColumn chkbx_key = new DataGridViewCheckBoxColumn();
                chkbx_key = new DataGridViewCheckBoxColumn();
                chkbx_key.Width = 40;
                chkbx_key.FlatStyle = FlatStyle.Popup;
                chkbx_key.HeaderText = "Incluir";
                chkbx_key.DisplayIndex = 2;
                chkbx_key.TrueValue = true;
                dataGridView1.Columns.Insert(1, chkbx_key);

                //Adiciona as variáveis
                dataGridView1.Rows.Add(iVariaveis);
                for (int i = 0; i < strVariaveis.Length; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = strVariaveis[i];
                    dataGridView1.Rows[i].Cells[0].ToolTipText = strVariaveis[i];
                    //dataGridView1.Rows[i].Cells[2].ToolTipText = strVariaveis[i];
                    dataGridView1.Rows[i].Cells[1].Value = false;
                    //dataGridView1.Rows[i].Cells[2].Value = false;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string[] m_tipos_grupos_indicadores = new string[] { "One-group", "Two-group", "Multi-group" };

        private string[] m_indicadores_onegroup_eveness = new string[] {"Índice de Segregação (Segregation Index)", "Índice de Segregação Ajustado por Contiguidade (Contiguity Adjusted Segregation Index)",
            "Indicador de entropia", "Coeficiente de Gini", "Índice de Atkinson"};
        private string[] m_indicadores_onegroup_exposition = new string[] { "Índice de Isolamento (Isolation Index)", "Coeficiente de Correlação (Corelation Ratio)" };
        private string[] m_indicadores_onegroup_clustering = new string[] { "Agrupamento Absoluto (Absolute clustering)", "Proximidade Média entre Membros do Grupo X",
            "Proximidade Média entre Membros de um Grupo (exp)", "Índice de Isolamento do Decaimento da Distância (Distance decay isolation index)"};

        private string[] m_indicadores_twogroup_eveness = new string[] {"Índice de dissimilaridade entre XY (Index of dissimilarity between XY)", 
            "Dissimilaridade Ajustada para Contiguidade entre XY (Dissimilarity adjusted for contiguity between XY)"};
        private string[] m_indicadores_twogroup_exposition = new string[] { "Índice de Interação (Interation index)" };
        private string[] m_indicadores_twogroup_clustering = new string[] { "Proximidade Média entre Membros dos Grupos X e Y (Mean proximity between members of groups X and Y)",
            "Proximidade Média entre Membros de um Grupo X e Grupo Y (Mean proximity between members of one group X and group Y)", "Índice de Proximidade Espacial (Spatial proximity index)", 
            "Índice Relativo de Agrupamento (Relative clustering index)", "Índice de Interação de Decaimento de Distância (Distance decay interaction index)"};

        private string[] m_indicadores_multigroup_eveness = new string[] {"Dissimilaridade (Dissimilarity)", "Coeficiente de Gini", 
            "Teoria de Informação (Information theory)"};
        private string[] m_indicadores_multigroup_exposition = new string[] {"Exposição Normalizada (Normalized exposure)", "Diversidade Relativa (Relative diversity)", 
            "Coeficiente de Variação ao Quadrado (Squared coefficient of variation)"};  

        private void frmCluster_Load(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked == true) relatoriosegregacao_ = true;
                else relatoriosegregacao_ = false;

                this.cmbTipoIndicador.Items.Clear();
                this.cmbTipoIndicador.Items.AddRange(this.m_tipos_grupos_indicadores);
                this.cmbTipoIndicador.SelectedIndex = 0;

                //Filtrando variaveis populacionais (somente permitir numéricas)
                #region Filtro
                int numvar = 0;
                bool xyz = false;
                
                for (int i = 0; i < dTable.Columns.Count; i++)
                {                    
                     Type tipo = dTable.Columns[i].DataType;
                     
                     //Salva o tipo de interesse
                     string strTipo = tipo.ToString();

                     if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16" || strTipo == "System.Int64" || strTipo == "System.Double")
                     {
                         numvar++;
                     }                    
                }

                string[] strVarPop = new string[numvar];
                int count=0;

                for (int i = 0; i < dTable.Columns.Count; i++)
                {
                    Type tipo = dTable.Columns[i].DataType;
                    
                    //Salva o tipo de interesse
                    string strTipo = tipo.ToString();

                    if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16" || strTipo == "System.Int64" || strTipo == "System.Double")
                    {
                        strVarPop[count] = strVariaveis[i];
                        count++;
                    }
                }                

                #endregion;

                strVariaveis = strVarPop;
                cmbVariavelTotalPopulacao.Items.AddRange(strVariaveis);                
                //cmbVariavelTotalPopulacao.SelectedIndex = 0;

                tabControl1.TabPages.Remove(this.tabPage4);
                tabControl1.TabPages.Remove(this.tabPage3);

                //checkedListBox1.Items.AddRange(strVariaveis);
                //PopulaDataGridView(ref dataGridView2, strVariaveis.Length);
                PopulaDataGridView(ref dataGridView2, strVarPop.Length);
                dataGridView2.AllowUserToAddRows = false;
                cmbTipoVariaveis.SelectedIndex = 0;
                cmbDistanciaBinaria.SelectedIndex = 0;
                cmbDistanciaCategorica.SelectedIndex = 0;
                cmbDistanciaOrdinaria.SelectedIndex = 0;

                //Inicializa o valor padrão da comboBox
                cmbDistancia.SelectedIndex = 0;
                cmbMetodo.SelectedIndex = 0;
                cmbCores.SelectedIndex = 0;
                cmbVizinhanca.SelectedIndex = 0;

                //Cores
                Color[] vetorCores = new Color[11];
                vetorCores[0] = Color.White;
                vetorCores[1] = Color.Yellow;
                vetorCores[2] = Color.Green;
                vetorCores[3] = Color.Blue;
                vetorCores[4] = Color.Red;
                vetorCores[5] = Color.Purple;
                vetorCores[6] = Color.Cyan;
                vetorCores[7] = Color.Coral;
                vetorCores[8] = Color.Khaki;
                vetorCores[9] = Color.Brown;
                vetorCores[10] = Color.Black;

                //Cores
                Brush[] vetorBrush = new Brush[11];
                vetorBrush[0] = Brushes.White;
                vetorBrush[1] = Brushes.Yellow;
                vetorBrush[2] = Brushes.Green;
                vetorBrush[3] = Brushes.Blue;
                vetorBrush[4] = Brushes.Red;
                vetorBrush[5] = Brushes.Purple;
                vetorBrush[6] = Brushes.Cyan;
                vetorBrush[7] = Brushes.Coral;
                vetorBrush[8] = Brushes.Khaki;
                vetorBrush[9] = Brushes.Brown;
                vetorBrush[10] = Brushes.Black;

                //cria uma list com os item do combobox
                List<GradientColor> colorList = new List<GradientColor>();
                int contador = 0;

                for (int i = 0; i < vetorCores.Length; i++)
                {
                    for (int j = 0; j < vetorCores.Length; j++)
                    {
                        if (j != i)
                        {
                            string strCor = "Cor" + contador.ToString();
                            colorList.Add(new GradientColor(strCor, vetorCores[i], vetorCores[j]));
                            coresVetor[contador, 0] = vetorBrush[i];
                            coresVetor[contador, 1] = vetorBrush[j];

                            coresVetor2[contador, 0] = vetorCores[i];
                            coresVetor2[contador, 1] = vetorCores[j];

                            contador++;
                        }
                    }
                }

                // seta a fonte pro combobox
                this.cmbCores.DataSource = colorList;
                this.cmbCores.DisplayMember = "Nome";
                this.cmbCores.ValueMember = "Nome";
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancela_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private ArrayList indicesSelecionados(CheckedListBox chkEveness, CheckedListBox chkExposition, CheckedListBox chkClustering)
        {
            ArrayList arLista= new ArrayList();
            for (int i = 0; i < chkEveness.Items.Count; i++)
            {
                if(chkEveness.GetItemChecked(i)) arLista.Add(chkEveness.Items[i].ToString());
            }
            for (int i = 0; i < chkExposition.Items.Count; i++)
            {
                if(chkExposition.GetItemChecked(i)) arLista.Add(chkExposition.Items[i].ToString());
            }
            for (int i = 0; i < chkClustering.Items.Count; i++)
            {
                if(chkClustering.GetItemChecked(i)) arLista.Add(chkClustering.Items[i].ToString());
            }
            return(arLista);
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {            
                DataTable DTIndices = new DataTable();

                //this.dataGridView1 = new DataGridView();
                HouveSegregacao_ = true;

                this.Cursor = Cursors.WaitCursor;

                string strPOP = cmbVariavelTotalPopulacao.SelectedItem.ToString();
                int cSelected = 0;
                for (int i = 0; i < strVariaveis.Length; i++) if ((bool)dataGridView2.Rows[i].Cells[1].Value == true) cSelected++;
                string[] strSelecionadas = new string[cSelected];
                cSelected = 0;
                for (int i = 0; i < strVariaveis.Length; i++)
                {
                    if ((bool)dataGridView2.Rows[i].Cells[1].Value == true)
                    {
                        strSelecionadas[cSelected] = dataGridView2.Rows[i].Cells[0].Value.ToString();
                        cSelected++;
                    }
                }

                bool dummy = false;

                if (strSelecionadas.Length == 0 && cmbTipoIndicador.SelectedItem.ToString() == "One-group")
                {
                    dummy = true;
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Nenhuma variável selecionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (strSelecionadas.Length < 2 && cmbTipoIndicador.SelectedItem.ToString() == "Two-group")
                {
                    dummy = true;
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Apenas uma variável selecionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (strSelecionadas.Length < 2 && cmbTipoIndicador.SelectedItem.ToString() == "Multi-group")
                {
                    dummy = true;
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Apenas uma variável selecionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                //Somente habilitar a geração de indices se a população for maior que as variaveis.
                bool erro_de_pop = false;
                if (cmbTipoIndicador.SelectedItem.ToString() == "One-group")
                {
                    for (int j = 0; j < strSelecionadas.Length; j++)
                    {
                        double soma_vars = 0;
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            soma_vars = Convert.ToDouble(dTable.Rows[i][strSelecionadas[j]]);
                            double pop = Convert.ToDouble(dTable.Rows[i][cmbVariavelTotalPopulacao.SelectedItem.ToString()]);
                            double diff = pop - soma_vars;

                            if (diff < 0)
                            {
                                erro_de_pop = true;
                                break;
                            }
                        }
                    }
                }
                
                if (cmbTipoIndicador.SelectedItem.ToString() == "Two-group")
                {
                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        double soma_vars = 0;
                        for (int j = 0; j < strSelecionadas.Length; j++)
                        {
                            soma_vars += Convert.ToDouble(dTable.Rows[i][strSelecionadas[j]]);
                            double pop = Convert.ToDouble(dTable.Rows[i][cmbVariavelTotalPopulacao.SelectedItem.ToString()]);
                            double diff = pop - soma_vars;

                            if (diff < 0)
                            {
                                erro_de_pop = true;
                                break;
                            }
                        }
                    }
                }


                if (erro_de_pop)
                {
                    dummy = true;
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("A variável de população deve ser maior que a soma das variáveis de caso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (dummy == false)
                {
                    if (shapeAlex.TipoVizinhanca.ToString().Length < 2)
                    {
                        toolStripStatusLabel1.Text = "Gerando matriz de vizinhanças";
                        toolStripStatusLabel1.Visible = true;
                        Application.DoEvents();
                        shapeAlex.GerarMatrizTodasDistancias();
                    }

                    BLIndicesSegregacaoSpacial bli = new BLIndicesSegregacaoSpacial();

                    bli.Dados = this.dTable;
                    bli.Shape = this.shapeAlex;
                    bli.ListaVariaveisXi = strSelecionadas;
                    bli.ListaVariaveisYi = strSelecionadas;

                    bli.VariavelTi = strPOP;

                    clsUtilTools clt = new clsUtilTools();

                    double[,] v = new double[0, 0];
                    double[,] aux = new double[0, 0];

                    ArrayList arIndices = indicesSelecionados(ckbListaIndicadoresEveness, ckbListaIndicadoresExposition, ckbListaIndicadoresClustering);

                    progressBar1.Visible = true;
                    progressBar1.Maximum = arIndices.Count;

                    if (cmbTipoIndicador.SelectedItem.ToString() == "One-group")
                    {
                        //Eveness
                        if (ckbListaIndicadoresEveness.GetItemChecked(0)) { aux = bli.GeraIndiceSegregacaoOneGroup(TipoIndiceSegregacaoOneGroup.SegregationIndex); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Segregação"; Application.DoEvents(); }
                        if (ckbListaIndicadoresEveness.GetItemChecked(1)) { aux = bli.GeraIndiceSegregacaoOneGroup(TipoIndiceSegregacaoOneGroup.SegregationIndexAdjustedContiguity); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Segregação Ajustado por Contiguidade"; Application.DoEvents(); }
                        if (ckbListaIndicadoresEveness.GetItemChecked(2)) { aux = bli.GeraIndiceSegregacaoOneGroup(TipoIndiceSegregacaoOneGroup.IndicadorEntropia); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Indicador Entropia"; Application.DoEvents(); }
                        if (ckbListaIndicadoresEveness.GetItemChecked(3)) { aux = bli.GeraIndiceSegregacaoOneGroup(TipoIndiceSegregacaoOneGroup.CoeficienteGini); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Coeficiente Gini"; Application.DoEvents(); }
                        if (ckbListaIndicadoresEveness.GetItemChecked(4)) { aux = bli.GeraIndiceSegregacaoOneGroup(TipoIndiceSegregacaoOneGroup.IndiceAtkisnon); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Indice Atkinson"; Application.DoEvents(); }

                        //Exposition
                        if (ckbListaIndicadoresExposition.GetItemChecked(0)) { aux = bli.GeraIndiceSegregacaoOneGroup(TipoIndiceSegregacaoOneGroup.IsolationIndex); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Isolamento"; Application.DoEvents(); }
                        if (ckbListaIndicadoresExposition.GetItemChecked(1)) { aux = bli.GeraIndiceSegregacaoOneGroup(TipoIndiceSegregacaoOneGroup.CorrelationRatio); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Coeficiente de Correlação"; Application.DoEvents(); }

                        //Clustering
                        if (ckbListaIndicadoresClustering.GetItemChecked(0)) { aux = bli.GeraIndiceSegregacaoOneGroup(TipoIndiceSegregacaoOneGroup.AbsoulteClustering); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Agrupamento Absoluto"; Application.DoEvents(); }
                        if (ckbListaIndicadoresClustering.GetItemChecked(1)) { aux = bli.GeraIndiceSegregacaoOneGroup(TipoIndiceSegregacaoOneGroup.MeanProximityBetweenMembersOfGroupX); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Proximidade Média"; Application.DoEvents(); }
                        if (ckbListaIndicadoresClustering.GetItemChecked(2)) { aux = bli.GeraIndiceSegregacaoOneGroup(TipoIndiceSegregacaoOneGroup.MeanProximityBetweenOfOneGroupExp); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Proximidade (EXP) Média"; Application.DoEvents(); }
                        if (ckbListaIndicadoresClustering.GetItemChecked(3)) { aux = bli.GeraIndiceSegregacaoOneGroup(TipoIndiceSegregacaoOneGroup.DistanceDecayIsolationIndex); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Distância de Decaimento"; Application.DoEvents(); }

                        //Populando o datatable
                        DTIndices.Columns.Add("Variáveis", System.Type.GetType("System.String"));
                        for (int i = 0; i < v.GetLength(1); i++)
                        {
                            DTIndices.Columns.Add(arIndices[i].ToString(), System.Type.GetType("System.String"));
                        }
                        for (int i = 0; i < v.GetLength(0); i++)
                        {
                            DataRow linha = DTIndices.NewRow();
                            linha[0] = strSelecionadas[i].ToString();
                            for (int j = 0; j < v.GetLength(1); j++)
                            {
                                linha[j + 1] = v[i, j].ToString();
                            }
                            DTIndices.Rows.Add(linha);
                        }
                        dataGridView1.DataSource = DTIndices;
                        dataGridView1.ReadOnly = true;
                        dataGridView1.Refresh();
                        dataGridView1.Update();
                    }

                    else if (cmbTipoIndicador.SelectedItem.ToString() == "Two-group")
                    {
                        //Eveness
                        if (ckbListaIndicadoresEveness.GetItemChecked(0))
                        {
                            aux = bli.GeraIndiceSegregacaoTwoGroup(TipoIndiceSegregacaoTwoGroup.IndexOfDissimilarityBetweenXY);
                            v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Dissimilaridade";
                        }
                        if (ckbListaIndicadoresEveness.GetItemChecked(1))
                        {
                            aux = bli.GeraIndiceSegregacaoTwoGroup(TipoIndiceSegregacaoTwoGroup.DissimilarityAdjustedForContiguityBetweenXY);
                            v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Dissimilaridade Ajustada para Contiguidade";
                        }

                        //Exposition
                        if (ckbListaIndicadoresExposition.GetItemChecked(0))
                        {
                            aux = bli.GeraIndiceSegregacaoTwoGroup(TipoIndiceSegregacaoTwoGroup.InteractionIndex); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Interação";
                        }

                        //Clustering
                        if (ckbListaIndicadoresClustering.GetItemChecked(0))
                        {
                            aux = bli.GeraIndiceSegregacaoTwoGroup(TipoIndiceSegregacaoTwoGroup.MeanProximityBetweenMembersOfGroupsXY); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Proximidade Média"; Application.DoEvents();
                        }
                        if (ckbListaIndicadoresClustering.GetItemChecked(1))
                        {
                            aux = bli.GeraIndiceSegregacaoTwoGroup(TipoIndiceSegregacaoTwoGroup.MeanProximityBetweenMembersOfOneGroupXAndGroupY); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Proximidade Média (EXP)"; Application.DoEvents();
                        }
                        if (ckbListaIndicadoresClustering.GetItemChecked(2))
                        {
                            aux = bli.GeraIndiceSegregacaoTwoGroup(TipoIndiceSegregacaoTwoGroup.SpatialProximityIndex); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Proximidade Espacial"; Application.DoEvents();
                        }
                        if (ckbListaIndicadoresClustering.GetItemChecked(3))
                        {
                            aux = bli.GeraIndiceSegregacaoTwoGroup(TipoIndiceSegregacaoTwoGroup.RelativeClusteringIndex); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de AGrupamento Relativo"; Application.DoEvents();
                        }
                        if (ckbListaIndicadoresClustering.GetItemChecked(4))
                        {
                            aux = bli.GeraIndiceSegregacaoTwoGroup(TipoIndiceSegregacaoTwoGroup.DistanceDecayInteractionIndex); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Distância de Decaimento"; Application.DoEvents();
                        }

                        //Gera dataGridView
                        DataTable tabela2group = new DataTable();
                        tabela2group.Columns.Add("Grupo 1");
                        tabela2group.Columns.Add("Grupo 2");

                        //dataGridView1.Columns.Add("vClasse1", "Grupo 1");
                        //dataGridView1.Columns.Add("vClasse2", "Grupo 2");

                        for (int i = 0; i < v.GetLength(1); i++)
                        {
                            string strNome2 = "Indice" + i.ToString();

                            //dataGridView1.Columns.Add(strNome2, arIndices[i].ToString());
                            tabela2group.Columns.Add(arIndices[i].ToString());
                        }

                        Object[] mLinha = new Object[v.GetLength(1) + 2];
                        strX = bli.ListaCombinaVariaveisXi;
                        strY = bli.ListaCombinaVariaveisYi;

                        for (int i = 0; i < v.GetLength(0); i++)
                        {
                            //mLinha[0] = (Object)strSelecionadas[i];
                            mLinha[0] = (Object)strX[i];
                            mLinha[1] = (Object)strY[i];
                            for (int j = 0; j < v.GetLength(1); j++) mLinha[j + 2] = (Object)v[i, j];
                            //dataGridView1.Rows.Add(mLinha);
                            tabela2group.Rows.Add(mLinha);
                        }

                        dataGridView1.DataSource = tabela2group;
                        dataGridView1.ReadOnly = true;
                        dataGridView1.Refresh();
                        dataGridView1.Update();
                        //dataGridView1.AllowUserToAddRows = false;
                        //dataGridView1.ReadOnly = true;
                    }

                    /*Dissimilarity = 1,
                    CoeficienteDeGini = 2,
                    InformationTheory = 3,
                    NormalizedExposure = 4,
                    RelativeDiversity = 5,
                    SquaredcoefficientOfVariation = 6 */
                    else if (cmbTipoIndicador.SelectedItem.ToString() == "Multi-group")
                    {
                        multi_ = true;

                        //Eveness
                        if (ckbListaIndicadoresEveness.GetItemChecked(0)) { aux = bli.GeraIndiceSegregacaoMultiGroup(TipoIndiceSegregacaoMultiGroup.Dissimilarity); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Índice de Dissimilaridade"; Application.DoEvents(); }
                        if (ckbListaIndicadoresEveness.GetItemChecked(1)) { aux = bli.GeraIndiceSegregacaoMultiGroup(TipoIndiceSegregacaoMultiGroup.CoeficienteDeGini); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Coeficiente de Gini"; Application.DoEvents(); }
                        if (ckbListaIndicadoresEveness.GetItemChecked(2)) { aux = bli.GeraIndiceSegregacaoMultiGroup(TipoIndiceSegregacaoMultiGroup.InformationTheory); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Teoria da Informação"; Application.DoEvents(); }

                        //Exposition
                        if (ckbListaIndicadoresExposition.GetItemChecked(0)) { aux = bli.GeraIndiceSegregacaoMultiGroup(TipoIndiceSegregacaoMultiGroup.NormalizedExposure); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Exposição Normalizada"; Application.DoEvents(); }
                        if (ckbListaIndicadoresExposition.GetItemChecked(1)) { aux = bli.GeraIndiceSegregacaoMultiGroup(TipoIndiceSegregacaoMultiGroup.RelativeDiversity); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Diversidade Relativa"; Application.DoEvents(); }
                        if (ckbListaIndicadoresExposition.GetItemChecked(2)) { aux = bli.GeraIndiceSegregacaoMultiGroup(TipoIndiceSegregacaoMultiGroup.SquaredcoefficientOfVariation); v = clt.Concateh(v, aux); progressBar1.Increment(1); toolStripStatusLabel1.Text = "Gerando Coeficiente de Variação ao Quadrado"; Application.DoEvents(); }

                        //Gera dataGridView
                        DataTable tabela3group = new DataTable();
                        tabela3group.Columns.Add("Grupo");

                        //dataGridView1.Columns.Add("vClasse", "Grupo");
                        for (int i = 0; i < v.GetLength(1); i++)
                        {
                            string strNome2 = "Indice" + i.ToString();

                            //dataGridView1.Columns.Add(strNome2, arIndices[i].ToString());
                            tabela3group.Columns.Add(arIndices[i].ToString());
                        }

                        Object[] mLinha = new Object[v.GetLength(1) + 1];
                        string nome = "Grupo";

                        mLinha[0] = (Object)nome;
                        for (int j = 0; j < v.GetLength(1); j++) mLinha[j + 1] = (Object)v[0, j];
                        //dataGridView1.Rows.Add(mLinha);
                        tabela3group.Rows.Add(mLinha);

                        //dataGridView1.AllowUserToAddRows = false;
                        //dataGridView1.ReadOnly = true;

                        dataGridView1.DataSource = tabela3group;
                        dataGridView1.ReadOnly = true;
                        dataGridView1.Refresh();
                        dataGridView1.Update();
                    }

                    toolStripStatusLabel1.Text = ".";
                    progressBar1.Value = 0;
                    Application.DoEvents();
                    this.Cursor = Cursors.Default;

                    int t = 0;

                    //Programação para enviar variaveis para gerar o relatorio
                    HouveSegregacao_ = true;
                    if (checkBox1.Checked == true) relatoriosegregacao_ = true;
                    matrizIndicesSegregacao_ = v;
                    nomesIndicesSegregacao_ = new string[arIndices.Count];
                    for (int i = 0; i < arIndices.Count; i++)
                    {
                        nomesIndicesSegregacao_[i] = Convert.ToString(arIndices[i]);
                    }
                    nomesVariaveis_ = strSelecionadas;

                    tabControl1.TabIndex = 1;
                    tabControl1.SelectedIndex = 1;
                    Application.DoEvents();

                    #region Informações para o relatório

                    strDistancia = cmbDistancia.SelectedItem.ToString();
                    strMetodo = cmbMetodo.SelectedItem.ToString();
                    strFatorMinkowsky = numMinkowsky.Value.ToString();
                    strNumCluster = numCluster.Value.ToString();
                    //stringEML = numEML.ToString();

                    #endregion
                }
                progressBar1.Visible = false;
                btnOK2.Enabled = true;
                
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        clsClusterizacaoEspacialHierarquica clsAlexCluster = new clsClusterizacaoEspacialHierarquica();

        private void cmbTipoIndicador_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cmbTipoIndicador.SelectedItem.ToString() == "One-group")
                    this.PreencheComboBoxOneGroup();

                if (this.cmbTipoIndicador.SelectedItem.ToString() == "Two-group")
                    this.PreencheComboBoxTwoGroup();

                if (this.cmbTipoIndicador.SelectedItem.ToString() == "Multi-group")
                    this.PreencheComboBoxMultiGroup();

                ckbTudoOuNada.Checked = false;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PreencheComboBoxOneGroup()
        {
            try
            {
                this.ckbListaIndicadoresClustering.Visible = true;
                this.lblIndicadoresClustering.Visible = true;

                this.ckbListaIndicadoresEveness.Items.Clear();
                this.ckbListaIndicadoresEveness.Items.AddRange(this.m_indicadores_onegroup_eveness);

                this.ckbListaIndicadoresExposition.Items.Clear();
                this.ckbListaIndicadoresExposition.Items.AddRange(this.m_indicadores_onegroup_exposition);

                this.ckbListaIndicadoresClustering.Items.Clear();
                this.ckbListaIndicadoresClustering.Items.AddRange(this.m_indicadores_onegroup_clustering);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PreencheComboBoxTwoGroup()
        {
            try
            {
                this.ckbListaIndicadoresClustering.Visible = true;
                this.lblIndicadoresClustering.Visible = true;

                this.ckbListaIndicadoresEveness.Items.Clear();
                this.ckbListaIndicadoresEveness.Items.AddRange(this.m_indicadores_twogroup_eveness);

                this.ckbListaIndicadoresExposition.Items.Clear();
                this.ckbListaIndicadoresExposition.Items.AddRange(this.m_indicadores_twogroup_exposition);

                this.ckbListaIndicadoresClustering.Items.Clear();
                this.ckbListaIndicadoresClustering.Items.AddRange(this.m_indicadores_twogroup_clustering);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PreencheComboBoxMultiGroup()
        {
            try
            {
                this.ckbListaIndicadoresEveness.Items.Clear();
                this.ckbListaIndicadoresEveness.Items.AddRange(this.m_indicadores_multigroup_eveness);

                this.ckbListaIndicadoresExposition.Items.Clear();
                this.ckbListaIndicadoresExposition.Items.AddRange(this.m_indicadores_multigroup_exposition);

                this.ckbListaIndicadoresClustering.Items.Clear();
                this.ckbListaIndicadoresClustering.Visible = false;
                this.lblIndicadoresClustering.Visible = false;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmbVariavelTotalPopulacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbVariavelTotalPopulacao.Items.Count > 0)
                {
                    dataGridView2.ReadOnly = false;

                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        if (dataGridView2.Rows[i].Cells[0].Value.ToString() == cmbVariavelTotalPopulacao.SelectedItem.ToString())
                        {
                            bool test = (bool)dataGridView2.Rows[i].Cells[1].Value;
                            if (test == true)
                            {
                                dataGridView2.Rows[i].Cells[1].Value = false;
                            }
                            dataGridView2.Rows[i].Visible = false;
                        }
                        else
                        {
                            bool test = (bool)dataGridView2.Rows[i].Cells[1].Value;
                            if (test == true)
                            {
                                dataGridView2.Rows[i].Cells[1].Value = false;
                            }
                            dataGridView2.Rows[i].Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ckbTudoOuNada_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckbTudoOuNada.Checked)
                {
                    for (int i = 0; i < ckbListaIndicadoresEveness.Items.Count; i++)
                    {
                        ckbListaIndicadoresEveness.SetItemChecked(i, true);
                    }
                    for (int i = 0; i < ckbListaIndicadoresExposition.Items.Count; i++)
                    {
                        ckbListaIndicadoresExposition.SetItemChecked(i, true);
                    }
                    for (int i = 0; i < ckbListaIndicadoresClustering.Items.Count; i++)
                    {
                        ckbListaIndicadoresClustering.SetItemChecked(i, true);
                    }
                }
                else
                {
                    for (int i = 0; i < ckbListaIndicadoresEveness.Items.Count; i++)
                    {
                        ckbListaIndicadoresEveness.SetItemChecked(i, false);
                    }
                    for (int i = 0; i < ckbListaIndicadoresExposition.Items.Count; i++)
                    {
                        ckbListaIndicadoresExposition.SetItemChecked(i, false);
                    }
                    for (int i = 0; i < ckbListaIndicadoresClustering.Items.Count; i++)
                    {
                        ckbListaIndicadoresClustering.SetItemChecked(i, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked == true) relatoriosegregacao_ = true;
                else relatoriosegregacao_ = false;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dsTemp = (DataTable)dataGridView1.DataSource;
                //dsTemp.Tables[0].Columns.Remove("Mapa"+strIDmapa);
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = "C:\\";
                saveFileDialog1.Filter = "Excel (*.xls)|*.xls|Access (*.mdb)|*.mdb|XML (*.xml)|*.xml";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                    string strFile = saveFileDialog1.FileName;

                    ExportData exporta = new ExportData();

                    if (strExtensao == ".XLS")
                    {
                        exporta.exportToExcel(dsTemp, strFile);                        
                    }
                    else if (strExtensao == ".XML")
                    {
                        dsTemp.WriteXml(strFile);
                    }
                    else if (strExtensao == ".MDB")
                    {
                        //Cria o arquivo MDB
                        exporta.exportaToAccess(dsTemp, strFile, this.Name);
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
