using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;
using System.IO;
using ZedGraph;

namespace IpeaGEO
{
    public partial class frmCluster : Form
    {
        public frmCluster()
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
            //Cria as colunas do DataGridView
            DataGridViewTextBoxColumn txtbox = new DataGridViewTextBoxColumn();
            txtbox = new DataGridViewTextBoxColumn();
            //txtbox.Width = 260;
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
            dataGridView1.Columns.Insert(1, chkbox);
            DataGridViewCheckBoxColumn chkbx_key = new DataGridViewCheckBoxColumn();
            chkbx_key = new DataGridViewCheckBoxColumn();
            chkbx_key.Width = 40;
            chkbx_key.FlatStyle = FlatStyle.Popup;
            chkbx_key.HeaderText = "Incluir";
            chkbx_key.DisplayIndex = 2;
            chkbx_key.TrueValue = true;
            dataGridView1.Columns.Insert(2, chkbx_key);

            //Adiciona as variáveis
            dataGridView1.Rows.Add(iVariaveis);
            for (int i = 0; i < strVariaveis.Length ; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = strVariaveis[i];
                dataGridView1.Rows[i].Cells[0].ToolTipText = strVariaveis[i];
                dataGridView1.Rows[i].Cells[2].ToolTipText = strVariaveis[i];
                dataGridView1.Rows[i].Cells[1].Value = "Contínuas";
                dataGridView1.Rows[i].Cells[2].Value = false;
            }
        }
        private void PopulaDataGridViewTree(ref DataGridView dataGridView1, double[,] mDados, int iMaximo)
        {
            dataGridView1.Columns.Add(strID, "Variável Identificadora");
            for (int i = 0; i < mDados.GetLength(1); i++)
            {
                int iCluster=iMaximo-i;
                string strNome = "Cluster " + iCluster.ToString();
                string strNome2 = "Var" + i.ToString();

                dataGridView1.Columns.Add(strNome2, strNome);
            }
            Object[] mLinha = new Object[mDados.GetLength(1)+1];

            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                int iposicao=shapeAlex[i].PosicaoNoDataTable;
                mLinha[0]=(Object)dTable.Rows[iposicao][strID];
                for(int j=0;j<mDados.GetLength(1);j++) mLinha[j+1] = (Object)mDados[i, j];
                dataGridView1.Rows.Add(mLinha);
            }
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;

        }
        private void frmCluster_Load(object sender, EventArgs e)
        {
            
            //checkedListBox1.Items.AddRange(strVariaveis);
            PopulaDataGridView(ref dataGridView2, strVariaveis.Length);
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

            //dai vc seta a fonte pro combobox
            this.cmbCores.DataSource = colorList;
            this.cmbCores.DisplayMember = "Nome";
            this.cmbCores.ValueMember = "Nome";
        }
   

        private void btnCancela_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                blRelatorio = chkRelatorio.Checked;

                progressBar1.Visible = true;
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                #region Cores

                //Cores
                Brush[] cores = new Brush[(int)numCluster.Value];
                Color[] coresRGB = new Color[(int)numCluster.Value];
                strCoresRGB = new string[cores.Length];

                if (chkAleatorio.Checked == false)
                {
                    //Item escolhido do ComboBox de cores
                    int iTem = cmbCores.SelectedIndex;

                    //Inicializa as cores
                    cores[0] = coresVetor[iTem, 0];
                    cores[cores.Length - 1] = coresVetor[iTem, 1];

                    //Converte para COLOR
                    Color[] colors = new Color[2];
                    colors[0] = coresVetor2[iTem, 0];
                    colors[1] = coresVetor2[iTem, 1];


                    //Cria o objeto cor
                    Color mCor0 = new Color();
                    //Set the color
                    mCor0 = Color.FromArgb(colors[0].ToArgb());
                    strCoresRGB[0] = System.Drawing.ColorTranslator.ToHtml(mCor0);
                    //Cria o objeto cor
                    Color mCor1 = new Color();
                    mCor1 = Color.FromArgb(colors[1].ToArgb());
                    strCoresRGB[cores.Length - 1] = System.Drawing.ColorTranslator.ToHtml(mCor1);

                    //Valores RGB
                    double R0 = Convert.ToDouble(colors[0].R);
                    double G0 = Convert.ToDouble(colors[0].G);
                    double B0 = Convert.ToDouble(colors[0].B);
                    double R1 = Convert.ToDouble(colors[1].R);
                    double G1 = Convert.ToDouble(colors[1].G);
                    double B1 = Convert.ToDouble(colors[1].B);

                    //Número de classes
                    double nClasses = Convert.ToDouble(numCluster.Value);

                    for (int i = 1; i < cores.Length - 1; i++)
                    {
                        double fator1 = 1 - (Convert.ToDouble(i + 1) / nClasses);
                        double fator2 = 1 - (Convert.ToDouble(nClasses - i - 1) / nClasses);

                        //Convert o Brush para Color
                        double Rf = R0 * fator1 + R1 * fator2;
                        double Gf = G0 * fator1 + G1 * fator2;
                        double Bf = B0 * fator1 + B1 * fator2;

                        //Cria o objeto cor
                        Color MyColor = new Color();

                        //Set the color
                        MyColor = Color.FromArgb((int)Rf, (int)Gf, (int)Bf);

                        //Guarda a cor
                        cores[i] = new SolidBrush(MyColor);
                        coresRGB[i] = MyColor;
                    }

                    //Guarda cores
                    classeCor = cores;

                    //Converte para RGB
                    for (int k = 0; k < cores.Length; k++)
                    {
                        strCoresRGB[k] = System.Drawing.ColorTranslator.ToHtml((Color)coresRGB[k]);
                    }
                }
                else
                {
                    Random rnd = new Random();
                    //Gerando vetor de cores aleatórias
                    for (int l = 0; l < cores.Length; l++)
                    {
                        int r = rnd.Next(0, 256);
                        int g = rnd.Next(0, 256);
                        int b = rnd.Next(0, 256);
                        Color rndColor = Color.FromArgb(r, g, b);
                        cores[l] = new SolidBrush(rndColor);
                        coresRGB[l] = rndColor;
                    }

                    //Guarda cores
                    classeCor = cores;

                    //Converte para RGB
                    for (int k = 0; k < cores.Length; k++)
                    {
                        strCoresRGB[k] = System.Drawing.ColorTranslator.ToHtml((Color)coresRGB[k]);
                    }

                }
                #endregion

                //OK
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                #region Informações para o relatório

                strDistancia = cmbDistancia.SelectedItem.ToString();
                strMetodo = cmbMetodo.SelectedItem.ToString();
                strFatorMinkowsky = numMinkowsky.Value.ToString();
                strNumCluster = numCluster.Value.ToString();
                //stringEML = numEML.ToString();

                #endregion

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbCores_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Pega o item a ser pintado
            GradientColor selectedItem = (GradientColor)this.cmbCores.Items[e.Index];

            // Projeta o tamanho do fundo 
            Rectangle rectangle = new Rectangle(0, e.Bounds.Top, this.Bounds.Width, e.Bounds.Height);

            //Desenha no combobox
            LinearGradientBrush backBrush = new LinearGradientBrush(rectangle, selectedItem.Color1, selectedItem.Color2, LinearGradientMode.Horizontal);

            e.Graphics.FillRectangle(backBrush, rectangle);
        }

        private void cmbDistancia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDistancia.SelectedItem.ToString() == "Minkowsky")
            {
                numMinkowsky.Enabled = true;
            }
            else
            {
                numMinkowsky.Enabled = false;
            }
        }

       

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            //Captura somente os Items selecionados
            int cSelected = 0;
            for (int i = 0; i < checkedListBox1.Items.Count; i++) if (checkedListBox1.GetItemChecked(i) == true) cSelected++;
            string[] strSelecionadas = new string[cSelected];
            cSelected = 0;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i) == true)
                {
                    strSelecionadas[cSelected] = checkedListBox1.Items[i].ToString();
                    cSelected++;
                }
            }

            //Guarda as variáveis selecionadas
            strVariaveisSelecionadas = strSelecionadas;
             * */
        }

        private void chkAleatorio_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAleatorio.Checked == true)
            {
                cmbCores.Enabled = false;
            }
            else
            {
                cmbCores.Enabled = true;
            }
        }

        private void cmbMetodo_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbTipoVariaveis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipoVariaveis.SelectedItem.ToString() == "Contínuas")
            {
                cmbDistancia.Enabled = true;
                cmbDistanciaBinaria.Enabled = false;
                cmbDistanciaCategorica.Enabled = false;
                cmbDistanciaOrdinaria.Enabled = false;

       
                for (int i = 0; i < strVariaveis.Length; i++) if(dataGridView2.Rows.Count>0) dataGridView2.Rows[i].Cells[1].Value = "Contínuas";
                

            }
            else if (cmbTipoVariaveis.SelectedItem.ToString() == "Binárias")
            {
                cmbDistancia.Enabled = false;
                cmbDistanciaBinaria.Enabled = true;
                cmbDistanciaCategorica.Enabled = false;
                cmbDistanciaOrdinaria.Enabled = false;

                numPesoBinaria.Enabled = true;
                numPesoCategorica.Enabled = false;
                numPesoOrdinais.Enabled = false;

                for (int i = 0; i < strVariaveis.Length; i++) if (dataGridView2.Rows.Count > 0) dataGridView2.Rows[i].Cells[1].Value = "Binárias";
            }
            else if (cmbTipoVariaveis.SelectedItem.ToString() == "Categóricas")
            {
                cmbDistancia.Enabled = false;
                cmbDistanciaBinaria.Enabled = false;
                cmbDistanciaCategorica.Enabled = true;
                cmbDistanciaOrdinaria.Enabled = false;

                numPesoBinaria.Enabled = false;
                numPesoCategorica.Enabled = true;
                numPesoOrdinais.Enabled = false;

                for (int i = 0; i < strVariaveis.Length; i++) if (dataGridView2.Rows.Count > 0) dataGridView2.Rows[i].Cells[1].Value = "Categóricas";
            }
            else if (cmbTipoVariaveis.SelectedItem.ToString() == "Ordinais")
            {
                cmbDistancia.Enabled = false;
                cmbDistanciaBinaria.Enabled = false;
                cmbDistanciaCategorica.Enabled = false;
                cmbDistanciaOrdinaria.Enabled = true;

                numPesoBinaria.Enabled = false;
                numPesoCategorica.Enabled = false;
                numPesoOrdinais.Enabled = true;

                for (int i = 0; i < strVariaveis.Length; i++) if (dataGridView2.Rows.Count > 0) dataGridView2.Rows[i].Cells[1].Value = "Ordinais";
            }
            else
            {
            }

                



        }

        private void cmbDistancia_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbDistancia.SelectedItem.ToString() == "Minkowsky")
            {
                numMinkowsky.Enabled = true;
            }
            else
            {
                numMinkowsky.Enabled = false;
            }
        }

        private void cmbCores_DrawItem_1(object sender, DrawItemEventArgs e)
        {
            // Pega o item a ser pintado
            GradientColor selectedItem = (GradientColor)this.cmbCores.Items[e.Index];

            // Projeta o tamanho do fundo 
            Rectangle rectangle = new Rectangle(0, e.Bounds.Top, this.Bounds.Width, e.Bounds.Height);

            //Desenha no combobox
            LinearGradientBrush backBrush = new LinearGradientBrush(rectangle, selectedItem.Color1, selectedItem.Color2, LinearGradientMode.Horizontal);

            e.Graphics.FillRectangle(backBrush, rectangle);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true) numMaxPolygon.Enabled = true;
            else numMaxPolygon.Enabled = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true) percMaxPolygon.Enabled = true;
            else percMaxPolygon.Enabled = false;
        }

        #region Cria matriz de dados ordinais e categóricos a partir dos dados originais

        private double[,] GeraMatrizDadosCategorias(double[,] mat)
        {
            double[,] res = new double[0, 0];

            //TODO: Conversão de binárias e categóricas de zero a 1

            return res;
        }

        #endregion

        #region Execução da geração de conglomerados

        private string iNomeDataGridView(DataGridView dataGridView2,string strNome)
        {
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                if (dataGridView2.Rows[i].Cells[0].Value.ToString() == strNome && (bool)dataGridView2.Rows[i].Cells[2].Value)
                {
                    return (dataGridView2.Rows[i].Cells[1].Value.ToString());
                }
           }
            return ("Nada");
        }
        clsClusterizacaoEspacialHierarquica clsAlexCluster = new clsClusterizacaoEspacialHierarquica();
        private void GerarCongromerados()
        {
            //Captura somente os Items selecionados
            int cSelected = 0;
            for (int i = 0; i < strVariaveis.Length; i++) if ((bool)dataGridView2.Rows[i].Cells[2].Value == true) cSelected++;
            string[] strSelecionadas = new string[cSelected];
            cSelected = 0;
            for (int i = 0; i < strVariaveis.Length; i++)
            {
                if ((bool)dataGridView2.Rows[i].Cells[2].Value == true)
                {
                    strSelecionadas[cSelected] = dataGridView2.Rows[i].Cells[0].Value.ToString();
                    cSelected++;
                }
            }
            if (cSelected == 0)
            {
                MessageBox.Show("Pelo menos uma variável deve ser selecionada.", "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //Guarda as variáveis selecionadas
                strVariaveisSelecionadas = strSelecionadas;

                //Guarda a distancia
                strDistancia = cmbDistancia.SelectedItem.ToString();

                //Inicializa a classe de clusters espaciais
                clsSpatialCluster spCluster = new clsSpatialCluster();

                //Fator de Minkowsky
                double iMinkowsky = 0;
                iMinkowsky = (double)numMinkowsky.Value;
                
                #region Conglomerados Hierarquicos Espaciais

                if (blEspacial == true)
                {
                    //Habilita o label
                    label7.Text = "Inicializando procedimento...";
                    Application.DoEvents();

                    int nvars_continuas = 0;
                    int nvars_binarias = 0;
                    int nvars_categoricas = 0;
                    int nvars_ordinais = 0;
                    int nobs = dTable.Rows.Count;

                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        if (dataGridView2.Rows[i].Cells[1].Value.ToString() == "Binárias" && Convert.ToBoolean(dataGridView2.Rows[i].Cells[2].Value)) 
                        {
                            nvars_binarias++;
                        }
                        if (dataGridView2.Rows[i].Cells[1].Value.ToString() == "Contínuas" && Convert.ToBoolean(dataGridView2.Rows[i].Cells[2].Value))
                        {
                            nvars_continuas++;
                        }
                        if (dataGridView2.Rows[i].Cells[1].Value.ToString() == "Categóricas" && Convert.ToBoolean(dataGridView2.Rows[i].Cells[2].Value)) 
                        {
                            nvars_categoricas++;
                        }
                        if (dataGridView2.Rows[i].Cells[1].Value.ToString() == "Ordinais" && Convert.ToBoolean(dataGridView2.Rows[i].Cells[2].Value))
                        {
                            nvars_ordinais++;
                        }
                    }

                    //Matrizes com os diferentes tipos de dados
                    double[,] mDadosContinuos = new double[0, 0];
                    double[,] mDadosBinarios = new double[0, 0];
                    double[,] mDadosCategoricos = new double[0, 0];
                    double[,] mDadosOrdinais = new double[0, 0];

                    if (nvars_continuas > 0)    mDadosContinuos = new double[nobs, nvars_continuas];
                    if (nvars_binarias > 0) mDadosBinarios = new double[nobs, nvars_binarias];
                    if (nvars_categoricas > 0) mDadosCategoricos = new double[nobs, nvars_categoricas];
                    if (nvars_ordinais > 0) mDadosOrdinais = new double[nobs, nvars_ordinais];

                    //Guarda os dados na Matriz
                    double[,] mDados = new double[dTable.Rows.Count, strSelecionadas.Length];
                    int iDummyBinaria=0;
                    int iDummyContinua=0;
                    int iDummyCategorica=0;
                    int iDummyOrdinais=0;

                    for (int j = 0; j < dTable.Rows.Count; j++)
                    {
                        iDummyBinaria = 0;
                        iDummyCategorica = 0;
                        iDummyContinua = 0;
                        iDummyOrdinais = 0;

                        for (int i = 0; i < strSelecionadas.Length; i++)
                        {
                            if (double.IsNaN(Convert.ToDouble(dTable.Rows[j][strSelecionadas[i]])) == false)
                            {
                                if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Binárias")
                                {
                                    mDadosBinarios[j, iDummyBinaria] = Convert.ToDouble(dTable.Rows[j][strSelecionadas[i]]);
                                    iDummyBinaria++;
                                }
                                else if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Contínuas")
                                {
                                    mDadosContinuos[j, iDummyContinua] = Convert.ToDouble(dTable.Rows[j][strSelecionadas[i]]);
                                    iDummyContinua++;
                                }
                                else if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Categóricas")
                                {
                                    mDadosCategoricos[j, iDummyCategorica] = Convert.ToDouble(dTable.Rows[j][strSelecionadas[i]]);
                                    iDummyCategorica++;
                                }
                                else if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Binárias")
                                {
                                    mDadosOrdinais[j, iDummyOrdinais] = Convert.ToDouble(dTable.Rows[j][strSelecionadas[i]]);
                                    iDummyOrdinais++;
                                }
                                 //mDados[j, i] = Convert.ToDouble(dTable.Rows[j][strSelecionadas[i]]);
                            }
                            else
                            {
                                if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Binárias")
                                {
                                    mDadosBinarios[j, iDummyBinaria] = double.NaN ;
                                    iDummyBinaria++;
                                }
                                else if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Contínuas")
                                {
                                    mDadosContinuos[j, iDummyContinua] = double.NaN;
                                    iDummyContinua++;
                                }
                                else if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Categóricas")
                                {
                                    mDadosCategoricos[j, iDummyBinaria] = double.NaN;
                                    iDummyCategorica++;
                                }
                                else if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Binárias")
                                {
                                    mDadosOrdinais[j, iDummyOrdinais] = double.NaN;
                                    iDummyOrdinais++;
                                }
                                //mDados[j, i] = double.NaN;
                            }
                        }
                    }
                    int[] iVetor = new int[mDados.GetLength(0)];
                    if (shapeAlex.TipoVizinhanca == "")
                    {
                        //Modifica o label
                        label7.Text = "Gerando a matriz de vizinhanças...";
                        Application.DoEvents();

                        //Cria a vizinhnaça
                        clsIpeaShape cps = new clsIpeaShape();
                        int tipo_vizinhanca = -1;
                        if (cmbVizinhanca.SelectedItem.ToString() == "Queen") tipo_vizinhanca = 1;
                        else tipo_vizinhanca = 2;

                        if (tipo_vizinhanca == 1) shapeAlex.TipoVizinhanca = "Queen";
                        if (tipo_vizinhanca == 2) shapeAlex.TipoVizinhanca = "Rook";

                        cps.DefinicaoVizinhos(ref shapeAlex, tipo_vizinhanca, ref progressBar1);
                    }

                   
                    clsUtilTools clsUtil = new clsUtilTools();

                    //Guarda o Shape
                    clsAlexCluster.EstruturaShape = shapeAlex;

                    //Modifica o label
                    label7.Text = "Gerando conglomerados espaciais...";
                    Application.DoEvents();

                    //Define o tipo de variável dos dados
                    switch (this.cmbTipoVariaveis.SelectedItem.ToString())
                    {
                        case "Contínuas":
                            clsAlexCluster.TipoDadosClusterizacao = TipoDadosClusterizacao.Continuos;
                            break;
                        case "Binários":
                            clsAlexCluster.TipoDadosClusterizacao = TipoDadosClusterizacao.Binarios;
                            break;
                        case "Ordinais":
                            clsAlexCluster.TipoDadosClusterizacao = TipoDadosClusterizacao.Ordinais;
                            break;
                        case "Categóricos":
                            clsAlexCluster.TipoDadosClusterizacao = TipoDadosClusterizacao.Categoricos;
                            break;
                        default:
                            clsAlexCluster.TipoDadosClusterizacao = TipoDadosClusterizacao.Mistos;
                            break;
                    }

                    //Define o método de dissimilaridade
                    switch (this.cmbMetodo.SelectedItem.ToString())
                    {
                        case "Ward":
                            clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.Ward;
                            break;
                        case "Sigle Linkage":
                            clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.SingleLinkage;
                            break;
                        case "Complete Linkage":
                            clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.CompleteLinkage;
                            break;
                        case "Average Linkage":
                            clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.AverageLinkage;
                            break;
                        case "Average Linkage (Weighted)":
                            clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.AverageLinkageWeigthed;
                            break;
                        case "Median":
                            clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.Median;
                            break;
                        case "Centroid":
                            clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.Centroid;
                            break;
                        default:
                            clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.Ward;
                            break;
                    }

                    //Entrada das matrizes de dados
                    switch (clsAlexCluster.TipoDadosClusterizacao)
                    {
                        case TipoDadosClusterizacao.Continuos:
                            clsAlexCluster.DadosClusterizacao = clsUtil.ArrayDoubleClone(mDadosContinuos);
                            //mDadosContinuos = clsUtil.ArrayDoubleClone(mDados);
                            clsAlexCluster.DadosContinuosClusterizacao = clsUtil.ArrayDoubleClone(mDadosContinuos);
                            break;
                        case TipoDadosClusterizacao.Binarios:
                            clsAlexCluster.DadosClusterizacao = clsUtil.ArrayDoubleClone(mDadosBinarios);
                            //mDadosBinarios = clsUtil.ArrayDoubleClone(mDados);
                            clsAlexCluster.DadosBinariosClusterizacao = clsUtil.ArrayDoubleClone(mDadosBinarios);
                            break;
                        case TipoDadosClusterizacao.Categoricos:
                            clsAlexCluster.DadosClusterizacao = clsUtil.ArrayDoubleClone(mDadosCategoricos);
                            //mDadosCategoricos = clsUtil.ArrayDoubleClone(mDados);
                            clsAlexCluster.DadosCategoricosClusterizacao = clsUtil.ArrayDoubleClone(mDadosCategoricos);
                            break;
                        case TipoDadosClusterizacao.Ordinais:
                            clsAlexCluster.DadosClusterizacao = clsUtil.ArrayDoubleClone(mDadosOrdinais);
                            //mDadosOrdinais = clsUtil.ArrayDoubleClone(mDados);
                            clsAlexCluster.DadosCategoricosClusterizacao = clsUtil.ArrayDoubleClone(mDadosOrdinais);
                            break;
                        default:
                            clsAlexCluster.DadosContinuosClusterizacao = mDadosContinuos;
                            clsAlexCluster.DadosBinariosClusterizacao = mDadosBinarios;
                            clsAlexCluster.DadosCategoricosClusterizacao = mDadosBinarios;
                            clsAlexCluster.PesoDistanciaBinariaDadosMistos = (double)numPesoBinaria.Value;
                            clsAlexCluster.PesoDistanciaCategoricaDadosMistos = (double)numPesoCategorica.Value;
                            break;
                    }

                    //Define a distância para variáveis continúas
                    if (clsAlexCluster.TipoDadosClusterizacao == TipoDadosClusterizacao.Continuos
                            || clsAlexCluster.TipoDadosClusterizacao == TipoDadosClusterizacao.Mistos)
                    {
                        switch (cmbDistancia.SelectedItem.ToString())
                        {
                            case "Variance Corrected":
                                clsAlexCluster.TipoDistanciaContinua = TipoDistanciaContinua.VarianceCorrected;
                                break;
                            case "Mahalanobis":
                                clsAlexCluster.TipoDistanciaContinua = TipoDistanciaContinua.CovarianceCorrected;
                                break;
                            case "Manhattan":
                                clsAlexCluster.TipoDistanciaContinua = TipoDistanciaContinua.L1Norm;
                                break;
                            case "Minkowsky":
                                clsAlexCluster.TipoDistanciaContinua = TipoDistanciaContinua.LpNorm;
                                clsAlexCluster.ParameterLpNorm = (double)numMinkowsky.Value;
                                break;
                            default:
                                clsAlexCluster.TipoDistanciaContinua = TipoDistanciaContinua.Euclidiana;
                                break;
                        }
                    }

                    //Define a distância para variáveis binárias
                    if (clsAlexCluster.TipoDadosClusterizacao == TipoDadosClusterizacao.Binarios
                            || clsAlexCluster.TipoDadosClusterizacao == TipoDadosClusterizacao.Mistos)
                    {
                        switch (this.cmbDistanciaBinaria.SelectedItem.ToString())
                        {
                            case "Dice":
                                clsAlexCluster.TipoDistanciaBinaria = TipoDistanciaBinaria.Dice;
                                break;
                            case "Jaccard":
                                clsAlexCluster.TipoDistanciaBinaria = TipoDistanciaBinaria.Jaccard;
                                break;
                            case "Kulczynski":
                                clsAlexCluster.TipoDistanciaBinaria = TipoDistanciaBinaria.Kulczynski;
                                break;
                            case "RusselRao":
                                clsAlexCluster.TipoDistanciaBinaria = TipoDistanciaBinaria.RusselRao;
                                break;
                            case "SimpleMatching":
                                clsAlexCluster.TipoDistanciaBinaria = TipoDistanciaBinaria.SimpleMatching;
                                break;
                            default:
                                clsAlexCluster.TipoDistanciaBinaria = TipoDistanciaBinaria.Tanimoto;
                                break;
                        }
                    }

                    //Define a distância para variáveis categóricas
                    if (clsAlexCluster.TipoDadosClusterizacao == TipoDadosClusterizacao.Categoricos
                            || clsAlexCluster.TipoDadosClusterizacao == TipoDadosClusterizacao.Mistos)
                    {
                        switch (this.cmbDistanciaCategorica.SelectedItem.ToString())
                        {
                            case "SimpleMatching":
                                clsAlexCluster.TipoDistanciaCategorica = TipoDistanciaCategorica.SimpleMatching;
                                break;
                            default:
                                clsAlexCluster.TipoDistanciaCategorica = TipoDistanciaCategorica.SimpleMatching;
                                break;
                        }
                    }
                     
                    //Números máximo, mínimo de clusters na árvore 
                    clsAlexCluster.NumMaxClustersArvore = (int)numMaxClusters.Value;
                    clsAlexCluster.NumMinClustersArvore = (int)numMinClusters.Value;
                   
                    //Restrições para os tamanhos máximos dos clusters
                    clsAlexCluster.LimitaTamanhoMaximoFreq = false;
                    clsAlexCluster.LimitaTamanhoMaximoPerc = false;

                    if (this.radioButton2.Checked)
                    {
                        clsAlexCluster.LimitaTamanhoMaximoFreq = true;
                        clsAlexCluster.TamanhoMaximoClusters = (int)this.numMaxPolygon.Value;
                    }

                    if (this.radioButton3.Checked)
                    {
                        clsAlexCluster.LimitaTamanhoMaximoPerc = true;
                        clsAlexCluster.PercentualMaximoClusters = (double)this.percMaxPolygon.Value;
                    }

                    //Faz a análise de conglomerados
                    clsAlexCluster.GeraArvoreContinuousHierarchicalClustering(ref progressBar1, ref label7);

                    //Coluna com os dados dos conglomerados
                    int coluna = clsAlexCluster.NumMaxClustersArvore - (int)numCluster.Value;

                    //Guarda o clusterTree no dataGridView
                    PopulaDataGridViewTree(ref dataGridView1, clsAlexCluster.ClusterTree, clsAlexCluster.NumMaxClustersArvore);
                    dataGridView1.Refresh();

                    //Converte para a forma de vetor
                    iVetor = clsUtil.ConverteClusterTree(clsAlexCluster.ClusterTree, coluna);


                    //Guarda os conglomerados
                    classePoligonos = iVetor;

                    #region Métodos para a escolha do número de conglomerados
                    //Modifica o label
                    label7.Text = "Gerando dados para a escolha do número de conglomerados...";
                    Application.DoEvents();

                    //CCC
                    dblCCC = clsAlexCluster.SequenciaCCC;

                    //Pseudo T
                    dblPseudoT = clsAlexCluster.SequenciaPseudoT;

                    //Pseudo F
                    dblPseudoF = clsAlexCluster.SequenciaPseudoF;

                    //R-Square
                    dblRSquare = clsAlexCluster.SequenciaR2;

                    //R-Square Partial
                    dblRSquarePartial = clsAlexCluster.SequenciaPartialR2;

                    //R-Square Expected
                    dblRSquareExpected = clsAlexCluster.SequenciaExpectedR2;

                    #region Gera Gráficos

                    // Get a reference to the GraphPane
                    GraphPane myPane = new GraphPane(new RectangleF(0, 0, 1059, 513), "Escolha do tamanho ótimo do número de conglomerados.", "Número de conglomerados", "Pseudo T");

                    // Set the titles and axis labels
                    myPane.Title.Text = "Escolha do tamanho ótimo do número de conglomerados.";
                    myPane.XAxis.Title.Text = "Número de conglomerados";
                    myPane.YAxis.Title.Text = "Pseudo T";
                    myPane.Y2Axis.Title.Text = "R-Square";

                    //List of points
                    PointPairList ptList = new PointPairList();
                    PointPairList rsList = new PointPairList();
                    PointPairList pfList = new PointPairList();
                    PointPairList cccList = new PointPairList();

                    // Fabricate some data values
                    int iObs = Convert.ToInt32(numCluster.Value) * 2;
                    int iTotal = dblRSquare.Length - 1;
                    for (int i = 0; i < dblRSquare.Length /*iObs*/; i++)
                    {
                        rsList.Add(i + 2, dblRSquare[iTotal]);
                        ptList.Add(i + 2, dblPseudoT[iTotal]);
                        cccList.Add(i + 2, dblCCC[iTotal]);
                        pfList.Add(i + 2, dblPseudoF[iTotal]);
                        iTotal--;
                    }

                    // Generate a red curve with diamond symbols, and "PseudoT" in the legend
                    LineItem myCurve = myPane.AddCurve("Pseudo T", ptList, Color.Red, SymbolType.Diamond);
                    // Fill the symbols with white
                    myCurve.Symbol.Fill = new Fill(Color.White);

                    // Generate a blue curve with circle symbols, and "RSquare" in the legend
                    myCurve = myPane.AddCurve("R-Square", rsList, Color.Blue, SymbolType.Circle);
                    // Fill the symbols with white
                    myCurve.Symbol.Fill = new Fill(Color.White);
                    // Associate this curve with the Y2 axis
                    myCurve.IsY2Axis = true;

                    // Generate a green curve with square symbols, and "PseudoF" in the legend
                    myCurve = myPane.AddCurve("Pseudo F", pfList, Color.Green, SymbolType.Square);
                    // Fill the symbols with white
                    myCurve.Symbol.Fill = new Fill(Color.White);
                    // Associate this curve with the second Y axis
                    myCurve.YAxisIndex = 1;

                    // Generate a Black curve with triangle symbols, and "CCC" in the legend
                    myCurve = myPane.AddCurve("CCC", cccList, Color.Black, SymbolType.Triangle);
                    // Fill the symbols with white
                    myCurve.Symbol.Fill = new Fill(Color.White);
                    // Associate this curve with the Y2 axis
                    myCurve.IsY2Axis = true;
                    // Associate this curve with the second Y2 axis
                    myCurve.YAxisIndex = 1;

                    // Show the x axis grid
                    myPane.XAxis.MajorGrid.IsVisible = true;

                    // Make the Y axis scale red
                    myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
                    myPane.YAxis.Title.FontSpec.FontColor = Color.Red;
                    // turn off the opposite tics so the Y tics don't show up on the Y2 axis
                    myPane.YAxis.MajorTic.IsOpposite = false;
                    myPane.YAxis.MinorTic.IsOpposite = false;
                    // Don't display the Y zero line
                    myPane.YAxis.MajorGrid.IsZeroLine = false;

                    // Enable the Y2 axis display
                    myPane.Y2Axis.IsVisible = true;
                    // Make the Y2 axis scale blue
                    myPane.Y2Axis.Scale.FontSpec.FontColor = Color.Blue;
                    myPane.Y2Axis.Title.FontSpec.FontColor = Color.Blue;
                    // turn off the opposite tics so the Y2 tics don't show up on the Y axis
                    myPane.Y2Axis.MajorTic.IsOpposite = false;
                    myPane.Y2Axis.MinorTic.IsOpposite = false;
                    // Display the Y2 axis grid lines
                    myPane.Y2Axis.MajorGrid.IsVisible = true;
                    // Align the Y2 axis labels so they are flush to the axis
                    myPane.Y2Axis.Scale.Align = AlignP.Inside;

                    // Create a second Y Axis, green
                    YAxis yAxis3 = new YAxis("Pseudo F");
                    myPane.YAxisList.Add(yAxis3);
                    yAxis3.Scale.FontSpec.FontColor = Color.Green;
                    yAxis3.Title.FontSpec.FontColor = Color.Green;
                    yAxis3.Color = Color.Green;
                    // turn off the opposite tics so the Y2 tics don't show up on the Y axis
                    yAxis3.MajorTic.IsInside = false;
                    yAxis3.MinorTic.IsInside = false;
                    yAxis3.MajorTic.IsOpposite = false;
                    yAxis3.MinorTic.IsOpposite = false;
                    // Align the Y2 axis labels so they are flush to the axis
                    yAxis3.Scale.Align = AlignP.Inside;

                    Y2Axis yAxis4 = new Y2Axis("CCC");
                    yAxis4.IsVisible = true;
                    myPane.Y2AxisList.Add(yAxis4);
                    // turn off the opposite tics so the Y2 tics don't show up on the Y axis
                    yAxis4.MajorTic.IsInside = false;
                    yAxis4.MinorTic.IsInside = false;
                    yAxis4.MajorTic.IsOpposite = false;
                    yAxis4.MinorTic.IsOpposite = false;
                    // Align the Y2 axis labels so they are flush to the axis
                    yAxis4.Scale.Align = AlignP.Inside;

                    // Fill the axis background with a gradient
                    myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);

                    myPane.AxisChange();

                    myPane.Border.IsVisible = false;

                    //Passa o gráfico
                    zedGraphControl1.GraphPane = myPane;
                    zedGraphControl1.Refresh();



                    #endregion


                    #endregion

                    label7.Text = "";
                    progressBar1.Visible = false;
                }

                #endregion




                this.Cursor = Cursors.Default;
                Application.DoEvents();
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            int[] iVetor = new int[shapeAlex.Count];
            int coluna = clsAlexCluster.NumMaxClustersArvore - (int)numCluster.Value;
            clsUtilTools clsUtil = new clsUtilTools();
            //Converte para a forma de vetor
            iVetor = clsUtil.ConverteClusterTree(clsAlexCluster.ClusterTree, coluna);


            //Guarda os conglomerados
            classePoligonos = iVetor;
            this.Cursor = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.GerarCongromerados();
                this.btnRedefinirClusters.Enabled = true;
                this.Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chkAleatorio_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkAleatorio.Checked) cmbCores.Enabled = false;
            else cmbCores.Enabled = true;
        }

       
    }
}
