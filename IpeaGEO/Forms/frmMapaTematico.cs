using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo
{
    public partial class frmMapaTematico : Form
    {
        public frmMapaTematico()
        {
            InitializeComponent();
        }

        Classes.clsArmazenamentoDados salvar = new Classes.clsArmazenamentoDados();

        #region variáveis internas

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

        private bool blGeraRelatorio;
        public bool GeraRelatorio
        {
            get
            {
                return blGeraRelatorio;
            }
            set
            {
                blGeraRelatorio = value;
            }
        }

        private int numeroClasses;
        public int numeroClasses_
        {
            get
            {
                return numeroClasses;
            }
            set
            {
                numeroClasses = value;
            }
        }

        private bool blGuardaClassificacao;
        public bool GuardaClassificacao
        {
            get
            {
                return blGuardaClassificacao;
            }
            set
            {
                blGuardaClassificacao = value;
            }
        }

        private string strVariavelMapa;
        public string Variavel
        {
            get
            {
                return strVariavelMapa;
            }
            set
            {
                strVariavelMapa = value;
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

        private string[] Legenda;
        public string[] Legenda_MapaTem
        {
            get
            {
                return Legenda;
            }
            set
            {
                Legenda = value;
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

        private string strMetodo;
        public string Metodologia
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

        private static Color[] coresRGB;
        public Color[] CoresRGBColors
        {
            get { return coresRGB; }
            set { coresRGB = value; }
        }

        private double[] classeMapaVetor;
        public double[] ClasseDoMapa
        {
            get
            {
                return classeMapaVetor;
            }
            set
            {
                classeMapaVetor = value;
            }
        }

        private string[] classeMapaCatVetor;
        public string[] strClasseDoMapa
        {
            get
            {
                return classeMapaCatVetor;
            }
            set
            {
                classeMapaCatVetor = value;
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
        
        private bool bool_quantitativo;
        public bool BoolQuantitativo
        {
            get
            {
                return bool_quantitativo;
            }
            set
            {
                bool_quantitativo = value;
            }
        }

        private string var_selecionada;
        public string VariavelEscolhida
        {
            get
            {
                return var_selecionada;
            }
            set
            {
                var_selecionada = value;
            }
        }
        
        private Brush[,] coresVetor = new Brush[110, 2];
        private Color[,] coresVetor2 = new Color[110, 2];

        #endregion

        private void frmMapaTematico_Load(object sender, EventArgs e)
        {
            try
            {
                this.btnEditarCortes.Enabled = false;

                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    try
                    {
                        Classes.clsArmazenamentoDados salvar = new Classes.clsArmazenamentoDados();

                        if (salvar.Leitura_efetuada == true)
                        {
                            double var = Convert.ToDouble(dTable.Rows[i][salvar.variaveis_tem]);
                        }
                        else
                        {
                            double var = Convert.ToDouble(dTable.Rows[i][cmbVariavel.SelectedItem.ToString()]);
                        }
                    }
                    catch
                    {
                        break;
                    }
                }

                if (tabControl1.TabPages.Contains(this.tabPage2))
                {
                    tabControl1.TabPages.Remove(tabPage2);
                }

                //Cores
                Color[] vetorCores = new Color[11];
                vetorCores[0] = Color.Black;
                vetorCores[1] = Color.Yellow;
                vetorCores[2] = Color.Green;
                vetorCores[3] = Color.Blue;
                vetorCores[4] = Color.Red;
                vetorCores[5] = Color.Purple;
                vetorCores[6] = Color.Cyan;
                vetorCores[7] = Color.Coral;
                vetorCores[8] = Color.Khaki;
                vetorCores[9] = Color.Brown;
                vetorCores[10] = Color.White;

                //Cores
                Brush[] vetorBrush = new Brush[11];
                vetorBrush[0] = Brushes.Black;
                vetorBrush[1] = Brushes.Yellow;
                vetorBrush[2] = Brushes.Green;
                vetorBrush[3] = Brushes.Blue;
                vetorBrush[4] = Brushes.Red;
                vetorBrush[5] = Brushes.Purple;
                vetorBrush[6] = Brushes.Cyan;
                vetorBrush[7] = Brushes.Coral;
                vetorBrush[8] = Brushes.Khaki;
                vetorBrush[9] = Brushes.Brown;
                vetorBrush[10] = Brushes.White;

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
                //int iConta=0;

                //Adiciona as variáveis
                if (bool_quantitativo)
                {
                    for (int i = 0; i < dTable.Columns.Count; i++)
                    {
                        string strTipo = dTable.Columns[i].DataType.ToString();

                        if (strTipo != "System.String")
                        {
                            if (dTable.Columns[i].ToString() != "Mapa" + strIDmapa)
                            {
                                cmbVariavel.Items.Add(dTable.Columns[i].ToString());
                            }
                        }
                    }
                }
                else 
                {
                    numClasses.Enabled = false;

                    for (int i = 0; i < dTable.Columns.Count; i++)
                    {                        
                        if (dTable.Columns[i].ToString() != "Mapa" + strIDmapa)
                        {
                            cmbVariavel.Items.Add(dTable.Columns[i].ToString());
                        }
                    }                
                }

                //Adiciona os métodos
                if (bool_quantitativo)
                {
                    cmbMetodo.Items.Add("Quantil");
                    cmbMetodo.Items.Add("Equal");
                    cmbMetodo.Items.Add("Jenks (Natural Breaks)");
                    //cmbMetodo.Items.Add("Geométrico");
                    cmbMetodo.Items.Add("Valores Únicos");
                }
                else
                {
                    cmbMetodo.Items.Add("Valores Únicos");
                    cmbMetodo.Enabled = false;
                    label2.Visible =
                        cmbDesvio.Visible =
                        numClasses.Visible = false;
                }
                
                //Inicializa o valor padrão da comboBox
                cmbCores.SelectedIndex = 0;
                cmbMetodo.SelectedIndex = 0;
                cmbVariavel.SelectedIndex = 0;
                cmbDesvio.SelectedIndex = 0;

                //combo box de classes
                numClasses.Maximum = numeroClasses;
            }
            catch (Exception ex)
            {
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
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmbCores_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                // Pega o item a ser pintado
                GradientColor selectedItem = (GradientColor)this.cmbCores.Items[e.Index];

                // Projeta o tamanho do fundo 
                Rectangle rectangle = new Rectangle(0, e.Bounds.Top, this.Bounds.Width, e.Bounds.Height);

                //Desenha no combobox
                LinearGradientBrush backBrush = new LinearGradientBrush(rectangle, selectedItem.Color1, selectedItem.Color2, LinearGradientMode.Horizontal);

                e.Graphics.FillRectangle(backBrush, rectangle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private Brush[] cores;
        private Color[] colors;
        public string MetodoEscolhido;
        public int cor;
        
        private void btnExecutar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                clear = false;

                Classes.clsArmazenamentoDados armazena = new Classes.clsArmazenamentoDados();
                if (armazena.Leitura_efetuada == true)
                {
                    this.VariavelEscolhida = armazena.variaveis_tem;
                    MetodoEscolhido = armazena.metodo_tem;
                    cor = armazena.cor_tem;
                }
                else
                {
                    this.VariavelEscolhida = cmbVariavel.SelectedItem.ToString();
                    MetodoEscolhido = cmbMetodo.SelectedItem.ToString();
                    cor = cmbCores.SelectedIndex;
                }

                int value;
                if (armazena.Leitura_efetuada == true)
                { 
                    value = armazena.cores_tem.Length;
                }
                else
                { value = (int)numClasses.Value; }

                double[] desvios = new double[7] { 1, 0.8, 0.6, 0.5, 0.4, 0.2, 0.1 };
                //Guarda as classes do mapa
                double[] dblClasseMapa = new double[value /*(int)numClasses.Value*/];

                //Tipo do dados
                string strTipo = dTable.Columns[VariavelEscolhida].DataType.ToString();
                if (bool_quantitativo)
                {
                    if (strTipo != "System.String")
                    {
                        clsMapa classeMapa = new clsMapa();

                        if (MetodoEscolhido == "Quantil")
                        {
                            classePoligonos = classeMapa.criaQuantis(dTable, VariavelEscolhida, strIDmapa, strID, value /*(int)numClasses.Value*/, ref dblClasseMapa);

                        }
                        else if (MetodoEscolhido == "Jenks (Natural Breaks)")
                        {
                            classePoligonos = classeMapa.criaJenks(dTable, VariavelEscolhida, strIDmapa, strID, value /*(int)numClasses.Value*/, ref dblClasseMapa);
                        }
                        //TODO: COLOCAR ESTE MÉTODO CORRIGIDO NA VERSÃO 2.0
                        else if (MetodoEscolhido == "Desvio Padrão")
                        {
                            int numero_class = 0;
                            classePoligonos = classeMapa.criaDesvios(dTable, VariavelEscolhida, strIDmapa, strID, desvios[cmbDesvio.SelectedIndex], ref dblClasseMapa, ref numero_class);
                            numClasses.Value = numero_class;
                        }
                        //else if (MetodoEscolhido == "Geométrico")
                        //{
                        //    classePoligonos = classeMapa.criaGeometric(dTable, VariavelEscolhida, strIDmapa, strID, value /*(int)numClasses.Value*/, ref dblClasseMapa);
                        //}

                        else if (MetodoEscolhido == "Equal")
                        {
                            classePoligonos = classeMapa.criaEqual(dTable, VariavelEscolhida, strIDmapa, strID, value /*(int)numClasses.Value*/, ref dblClasseMapa);
                        }
                        else if (MetodoEscolhido == "Valores Únicos")
                        {
                            int numero_classes = 0;
                            ArrayList valores_diferentes = new ArrayList();
                            for (int i = 0; i < dTable.Rows.Count; i++)
                            {
                                if (!valores_diferentes.Contains(dTable.Rows[i][VariavelEscolhida]))
                                {
                                    valores_diferentes.Add(dTable.Rows[i][VariavelEscolhida]);
                                }
                            }
                            numero_classes = valores_diferentes.Count;
                            numClasses.Value = numero_classes;
                            dblClasseMapa = new double[numero_classes];

                            classePoligonos = classeMapa.criaValoresUnicos(dTable, VariavelEscolhida, strIDmapa, strID, numero_classes, ref dblClasseMapa);
                        }

                        //Tipo de mapa temático
                        strMetodo = MetodoEscolhido;

                        //Guarda a variável
                        strVariavelMapa = VariavelEscolhida;

                        //Guarda a classe
                        //classeMapaVetor = ClasseDoMapa;
                        classeMapaVetor = dblClasseMapa;
                        if (!classe_manual)
                        {
                            classeMapaVetor = dblClasseMapa;
                        }

                        //Cores

                        cores = new Brush[(int)numClasses.Value];
                        coresRGB = new Color[(int)numClasses.Value];
                        strCoresRGB = new string[cores.Length];
                        Legenda = new string[(int)numClasses.Value];

                        if (chkAleatorio.Checked == false)
                        {
                            if (armazena.Leitura_efetuada == false)
                            {                               

                                //Item escolhido do ComboBox de cores
                                int iTem = cor/*cmbCores.SelectedIndex*/;

                                //Inicializa as cores
                                cores[0] = coresVetor[iTem, 0];
                                cores[cores.Length - 1] = coresVetor[iTem, 1];

                                //Converte para COLOR
                                colors = new Color[2];
                                colors[0] = coresRGB[0] = coresVetor2[iTem, 0];
                                colors[1] = coresRGB[coresRGB.Length - 1] = coresVetor2[iTem, 1];

                                //Cores para HTML
                                strCoresRGB = new string[cores.Length];
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
                                double nClasses = Convert.ToDouble(numClasses.Value);

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
                                    //ToDoCORES
                                    cores[i] = new SolidBrush(MyColor);
                                    coresRGB[i] = MyColor;
                                }
                            

                                //Guarda cores
                                classeCor = cores;
                            }

                            // abre as cores do xml salvo 
                            if (armazena.Leitura_efetuada == true)
                            {
                                cores = new Brush[armazena.cores_tem.Length];
                                coresRGB = new Color[armazena.cores_tem.Length];
                                strCoresRGB = new string[cores.Length];

                                Legenda = new string[armazena.cores_tem.Length];

                                coresRGB = armazena.cores_tem;

                                for (int i = 0; i < coresRGB.Length; i++)
                                {
                                    cores[i] = new SolidBrush(coresRGB[i]);
                                }

                                classeCor = cores;
                            }

                            //Converte para RGB
                            for (int k = 1; k < cores.Length - 1; k++)
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

                        //Passar o boolean para o formulário frmMapa gerar o relatório
                        blGeraRelatorio = chkRelatorio.Checked;

                        //Guardar a classificação no DATASET do formulário frmMapa
                        blGuardaClassificacao = chkGuarda.Checked;

                        //Legenda = new string[(int)numClasses.Value];
                        for (int i = 0; i < (int)numClasses.Value; i++)
                        {
                            Legenda[i] = "Classe " + i;
                        }

                        button1.Enabled = true;
                        this.btnEditarCortes.Enabled = true;
                        if (armazena.Leitura_efetuada == false)
                        {
                            MessageBox.Show("Mapa gerado.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("A variável precisa ser numérica.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {                      
                    clsMapa classeMapa = new clsMapa();

                    if (MetodoEscolhido == "Valores Únicos")
                    {
                        /*-----------------------------------------------------------------------------------------------------------*/
                        /*-- criando uma coluna auxiliar temporário, com valores numéricos para utilizarmos a programação do Pedro --*/
                        /*-----------------------------------------------------------------------------------------------------------*/

                        string col_selected = VariavelEscolhida;
                        var_selecionada = VariavelEscolhida;
                        string nome_col_temp = ("Coluna_temp_" + 1).Trim();
                        for (int i = 1; i <= 1000; i++)
                        {
                            nome_col_temp = ("Coluna_temp_" + i).Trim();
                            if (!dTable.Columns.Contains(nome_col_temp))
                            {
                                break;
                            }
                        }
                        dTable.Columns.Add(nome_col_temp, typeof(double));

                        clsUtilTools clt = new clsUtilTools();
                        object[,] cats = clt.DataTableToObjectMatrix(dTable, VariavelEscolhida);
                        clt.FrequencyTable(ref cats, cats);
                        string[] scats = new string[cats.GetLength(0)];
                        for (int i = 0; i < scats.GetLength(0); i++)
                        {
                            scats[i] = cats[i, 0].ToString();
                        }

                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            for (int j = 0; j < scats.GetLength(0); j++)
                            {
                                if (dTable.Rows[i][col_selected].ToString() == scats[j])
                                {
                                    dTable.Rows[i][nome_col_temp] = (double)j;
                                    break;
                                }
                            }
                        }

                        int numero_classes = 0;

                        numero_classes = cats.GetLength(0);
                        numClasses.Value = numero_classes;
                        dblClasseMapa = new double[numero_classes];
                                                    
                        classePoligonos = classeMapa.criaValoresUnicos(dTable, nome_col_temp, strIDmapa, strID, numero_classes, ref dblClasseMapa);
                            
                        dTable.Columns.Remove(nome_col_temp);

                        //Guarda a classe
                            strClasseDoMapa = scats;

                        //Gera vetor com as legendas
                            Legenda = new string[(int)numClasses.Value];
                            for (int i = 0; i < (int)numClasses.Value; i++)
                            {
                                Legenda[i] = scats[i];
                            }
                    }

                    //Tipo de mapa temático
                    strMetodo = MetodoEscolhido;

                    //Guarda a variável
                    strVariavelMapa = VariavelEscolhida;

                    if (!escolha_manual)
                    {
                        classeMapaVetor = dblClasseMapa;
                    }

                    //Cores
                    cores = new Brush[(int)numClasses.Value];
                    coresRGB = new Color[(int)numClasses.Value];
                    strCoresRGB = new string[cores.Length];

                    if (chkAleatorio.Checked == false)
                    {
                        if (salvar.Leitura_efetuada == false)
                        {
                            //Item escolhido do ComboBox de cores
                            int iTem = cmbCores.SelectedIndex;

                            //Inicializa as cores
                            cores[0] = coresVetor[iTem, 0];
                            cores[cores.Length - 1] = coresVetor[iTem, 1];

                            //Converte para COLOR
                            colors = new Color[2];
                            colors[0] = coresRGB[0] = coresVetor2[iTem, 0];
                            colors[1] = coresRGB[coresRGB.Length - 1] = coresVetor2[iTem, 1];

                            //Cores para HTML
                            strCoresRGB = new string[cores.Length];
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
                            double nClasses = Convert.ToDouble(numClasses.Value);

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
                        }

                        else 
                        {
                            cores = new Brush[armazena.cores_tem.Length];
                            coresRGB = new Color[armazena.cores_tem.Length];
                            strCoresRGB = new string[cores.Length];

                            coresRGB = armazena.cores_tem;

                            for (int i = 0; i < coresRGB.Length; i++)
                            {
                                cores[i] = new SolidBrush(coresRGB[i]);
                            }

                            classeCor = cores;
                        }
                        //Converte para RGB
                        for (int k = 1; k < cores.Length - 1; k++)
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

                    //Passar o boolean para o formulário frmMapa gerar o relatório
                    blGeraRelatorio = chkRelatorio.Checked;

                    //Guardar a classificação no DATASET do formulário frmMapa
                    blGuardaClassificacao = chkGuarda.Checked;
                      
                    button1.Enabled = true;
                    this.btnEditarCortes.Enabled = true;
                    if (armazena.Leitura_efetuada == false)
                    {
                        MessageBox.Show("Mapa gerado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                //ToDoLegendas
                GuardaLegendasIniciais();

                this.Cursor = Cursors.Default;


            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmbMetodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbMetodo.SelectedItem.ToString() == "Desvio Padrão")
                {
                    numClasses.Visible = false;
                    cmbDesvio.Visible = true;
                    label2.Text = "Desvios";
                }
                else
                {
                    if (bool_quantitativo)
                    {
                        numClasses.Visible = true;
                    }
                    cmbDesvio.Visible = false;
                    label2.Text = "Classes";
                }

                ReinicializarEdicao();

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chkAleatorio_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkAleatorio.Checked) cmbCores.Enabled = false;
                else cmbCores.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool escolha_manual = false;
        private String[] info_mapa = new String[2];
        public string metodo = "";
        private int cor_selec;
        public int indice;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                erromanual = false;
                //Checar se as classes são plausíveis
                double[] classesmanuais = new double[dataGridView1.Rows.Count];

                if (dataGridView1.Rows.Count > 0)
                {
                    m_legendas_manuais = new string[dataGridView1.Rows.Count];
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        classesmanuais[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);

                        m_legendas_manuais[i] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    }
                    catch
                    {
                        erromanual = true;
                        MessageBox.Show("As classes devem ser numéricas", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                }

                for (int i = 0; i < classesmanuais.Length - 1; i++)
                {
                    if (classesmanuais[i] > classesmanuais[i + 1])
                    {
                        erromanual = true;
                        MessageBox.Show("As classes devem estar em ordem crescente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                }

                if (!erromanual)
                {
                    if (classe_manual)
                    {
                        double[] desvios = new double[7] { 1, 0.8, 0.6, 0.5, 0.4, 0.2, 0.1 };
                        //Guarda as classes do mapa
                        double[] dblClasseMapa = new double[(int)numClasses.Value];

                        //Tipo do dados
                        string strTipo = dTable.Columns[cmbVariavel.SelectedItem.ToString()].DataType.ToString();

                        //Guarda a legenda
                        //Legenda = new string[classeMapaVetor.Length];

                        if (bool_quantitativo)
                        {
                            if (strTipo != "System.String")
                            {
                                clsMapa classeMapa = new clsMapa();

                                if (classe_manual)
                                {
                                    for (int i = 0; i < classeMapaVetor.Length; i++)
                                    {
                                        classeMapaVetor[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                                        //Legenda[i] = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value);                                        
                                    }                                    
                                    classePoligonos = classeMapa.criaManual(dTable, cmbVariavel.SelectedItem.ToString(), strIDmapa, strID, (int)numClasses.Value, ref classeMapaVetor);
                                }

                                //Tipo de mapa temático
                                strMetodo = cmbMetodo.SelectedItem.ToString();

                                if (classe_manual)
                                {
                                    strMetodo = "Manual";
                                }
                                //Guarda a variável
                                strVariavelMapa = cmbVariavel.SelectedItem.ToString();

                                //Guarda a classe. Se a escolha foi manual, nem entramos aqui.
                                classeMapaVetor = ClasseDoMapa;
                                if (!classe_manual)
                                {
                                    classeMapaVetor = dblClasseMapa;
                                }

                                //Passar o boolean para o formulário frmMapa gerar o relatório
                                blGeraRelatorio = chkRelatorio.Checked;

                                //Guardar a classificação no DATASET do formulário frmMapa
                                blGuardaClassificacao = chkGuarda.Checked;
                            }
                            else
                            {
                                MessageBox.Show("A variável precisa ser numérica.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            if (strTipo == "System.String")
                            {
                                clsMapa classeMapa = new clsMapa();

                                if (ckbDefinicaoManual)
                                {
                                    for (int i = 0; i < classeMapaVetor.Length; i++)
                                    {
                                        classeMapaVetor[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                                        Legenda[i] = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value);
                                    }
                                    strMetodo = "Manual";
                                    //classePoligonos = classeMapa.criaManual(dTable, cmbVariavel.SelectedItem.ToString(), strIDmapa, strID, (int)numClasses.Value, ref classeMapaVetor);
                                }

                                //Tipo de mapa temático
                                strMetodo = cmbMetodo.SelectedItem.ToString();

                                //Guarda a variável
                                strVariavelMapa = cmbVariavel.SelectedItem.ToString();

                                //Guarda a classe. Se a escolha foi manual, nem entramos aqui.
                                classeMapaVetor = ClasseDoMapa;
                                if (!escolha_manual)
                                {
                                    classeMapaVetor = dblClasseMapa;
                                }

                                //Passar o boolean para o formulário frmMapa gerar o relatório
                                blGeraRelatorio = chkRelatorio.Checked;

                                //Guardar a classificação no DATASET do formulário frmMapa
                                blGuardaClassificacao = chkGuarda.Checked;
                            }                        
                        }

                        //Passar o boolean para o formulário frmMapa gerar o relatório
                        blGeraRelatorio = chkRelatorio.Checked;

                        //Guardar a classificação no DATASET do formulário frmMapa
                        blGuardaClassificacao = chkGuarda.Checked;

                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        //Passar o boolean para o formulário frmMapa gerar o relatório
                        blGeraRelatorio = chkRelatorio.Checked;

                        //Guardar a classificação no DATASET do formulário frmMapa
                        blGuardaClassificacao = chkGuarda.Checked;

                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                }

                //Cor alterada
                if (mudouCores)
                {
                    for (int i = 0; i < coresRGB.Length; i++)
                    {
                        cores[i] = new SolidBrush(dataGridView1.Rows[i].Cells[0].Style.BackColor);
                        strCoresRGB[i] = System.Drawing.ColorTranslator.ToHtml(dataGridView1.Rows[i].Cells[0].Style.BackColor);
                        coresRGB[i] = dataGridView1.Rows[i].Cells[0].Style.BackColor;
                    }
                }
                Classes.clsArmazenamentoDados armazena = new Classes.clsArmazenamentoDados();

                //Legendas manuais
                if (m_legendas_manuais.GetLength(0) > 0)
                {
                    Legenda = new string[m_legendas_manuais.GetLength(0)];
                    for (int i = 0; i < Legenda.GetLength(0); i++) Legenda[i] = m_legendas_manuais[i];
                }


                if (armazena.Leitura_efetuada == false)
                {
                    //salvar.CapturaDados(coresRGB, m_legendas_manuais, cmbVariavel.SelectedItem.ToString(), cmbMetodo.SelectedItem.ToString(), cmbCores.SelectedIndex);
                    info_mapa[0] = cmbVariavel.SelectedItem.ToString();
                    metodo = info_mapa[1] = cmbMetodo.SelectedItem.ToString();
                    indice = cor_selec = cmbCores.SelectedIndex;
                }
                else
                {
                    m_legendas_manuais = armazena.legendas_tem;
                }

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                //ckbDefinicaoManual.Checked = false;
            }
        }

        private void cmbCores_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ReinicializarEdicao();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmbVariavel_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                try
                {
                    double var = Convert.ToDouble(dTable.Rows[i][cmbVariavel.SelectedItem.ToString()]);
                    //ckbDefinicaoManual.Enabled = true;
                }
                catch
                {
                    //ckbDefinicaoManual.Enabled = false;
                    break;
                }
            }

            ReinicializarEdicao();
        }

        private string[] m_legendas_manuais = new string[0];
        public string[] legendas = new string[0];

        private void GuardaLegendasIniciais()
        {
            try
            {
                Classes.clsArmazenamentoDados salvar = new Classes.clsArmazenamentoDados();
                
                int num_classes;
                
                if (salvar.Leitura_efetuada == true)
                { num_classes = cores.Length; }
                else
                { num_classes = Convert.ToInt32(this.numClasses.Value); }

                //Caso o usuário tenha selecionado manual antes de executar algum outro método, criar um vetor de classes de exemplo

                if (bool_quantitativo) 
                {
                    if (classeMapaVetor == null)
                    {
                        classeMapaVetor = new double[num_classes];
                        for (int i = 0; i < num_classes; i++)
                        {
                            classeMapaVetor[i] = 0;
                        }
                        double valormax = 0;
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            if (valormax < Convert.ToDouble(dTable.Rows[i][cmbVariavel.SelectedItem.ToString()]))
                            {
                                valormax = Convert.ToDouble(dTable.Rows[i][cmbVariavel.SelectedItem.ToString()]);
                            }
                        }
                        classeMapaVetor[num_classes - 1] = valormax;
                    }

                    clsUtilTools clsAlex = new clsUtilTools();

                    double[,] dados = new double[dTable.Rows.Count, 1];
                    double[,] cats = new double[num_classes, 1];
                    double[,] dadosCats = new double[dTable.Rows.Count, 1];

                    m_legendas_manuais = new string[num_classes];

                    //ToDoLegenda
                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        dados[i, 0] = Convert.ToDouble(dTable.Rows[i][cmbVariavel.SelectedItem.ToString()]);
                        dadosCats[i, 0] = Convert.ToDouble(classePoligonos[i]);
                    }
                    
                    clsAlex.FrequencyTable(ref cats, dadosCats);
                    double minimo = clsAlex.Min(dados);
                    double maximo = clsAlex.Max(dados);

                    for (int i = 0; i < num_classes; i++)
                    {
                        if (i == 0)
                        {
                            m_legendas_manuais[i] = "Valores no intervalo [" + minimo.ToString() + "; " + classeMapaVetor[0] + "]";
                        }
                        else if (i > 0 || i < num_classes - 1)
                        {
                            m_legendas_manuais[i] = "Valores no intervalo (" + classeMapaVetor[i - 1] + "; " + classeMapaVetor[i] + "]";
                        }
                        else
                        {
                            m_legendas_manuais[i] = "Valores no intervalo (" + classeMapaVetor[num_classes - 1] + "; " + maximo.ToString() + "]";
                        }
                    }
                    legendas = m_legendas_manuais;

                }

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ckbDefinicaoManual = false;
        private bool blSegunda = false;
        private bool editaNumObs = false;  

        private void btnEditarCortes_Click(object sender, EventArgs e)
        {
            try
            {
                if (!tabControl1.TabPages.Contains(tabPage2))
                    tabControl1.TabPages.Add(tabPage2);

                ckbDefinicaoManual = true;
                if (!tabControl1.TabPages.Contains(this.tabPage2) || dataGridView1.RowCount > 0/*&& ckbDefinicaoManual.Checked*/)
                {
                    if (dataGridView1.RowCount == 0) tabControl1.TabPages.Add(tabPage2);
                    tabControl1.SelectedTab = tabPage2;
                    escolha_manual = true;
                }
                else
                {
                    escolha_manual = false;
                }

                int num_classes = Convert.ToInt32(this.numClasses.Value);
                tabControl1.SelectedTab = tabPage2;
                //Caso o usuário tenha selecionado manual antes de executar algum outro método, criar um vetor de classes de exemplo
                if (classeMapaVetor == null)
                {
                    classeMapaVetor = new double[num_classes];
                    for (int i = 0; i < num_classes; i++)
                    {
                        classeMapaVetor[i] = 0;
                    }
                    double valormax = 0;
                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        if (valormax < Convert.ToDouble(dTable.Rows[i][cmbVariavel.SelectedItem.ToString()]))
                        {
                            valormax = Convert.ToDouble(dTable.Rows[i][cmbVariavel.SelectedItem.ToString()]);
                        }
                    }
                    classeMapaVetor[num_classes - 1] = valormax;

                    button1.Enabled = true;
                }
                
                while (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.RemoveAt(0);
                    blSegunda = true;
                }

                dataGridView1.Rows.Clear();
                int teste = dataGridView1.Rows.Count;
                if (dataGridView1.ColumnCount == 0)
                {
                    //Adiciona cor
                    DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                    txtCor = new DataGridViewTextBoxColumn();
                    txtCor.Width = 38;
                    txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                    txtCor.ReadOnly = true;
                    txtCor.HeaderText = "Cor";
                    txtCor.DisplayIndex = 2;
                    txtCor.DefaultCellStyle.BackColor = coresRGB[0];
                    txtCor.DefaultCellStyle.SelectionBackColor = Color.Transparent;
                    txtCor.SortMode = DataGridViewColumnSortMode.NotSortable;

                    dataGridView1.Columns.Insert(0, txtCor);

                    //Adiciona Legenda
                    DataGridViewTextBoxColumn txtLegData = new DataGridViewTextBoxColumn();
                    txtLegData = new DataGridViewTextBoxColumn();
                    txtLegData.Width = 400;
                    txtLegData.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                    txtLegData.ReadOnly = false;
                    txtLegData.HeaderText = "Legenda";
                    txtLegData.DisplayIndex = 2;
                    txtLegData.SortMode = DataGridViewColumnSortMode.NotSortable;

                    dataGridView1.Columns.Insert(1, txtLegData);

                    //Adiciona Limite Superior
                    DataGridViewTextBoxColumn txtLimiteSuperior = new DataGridViewTextBoxColumn();
                    txtLimiteSuperior = new DataGridViewTextBoxColumn();
                    txtLimiteSuperior.Width = 104;
                    txtLimiteSuperior.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                    txtLimiteSuperior.ReadOnly = false;
                    txtLimiteSuperior.HeaderText = "Limite Superior";
                    txtLimiteSuperior.DisplayIndex = 2;
                    txtLimiteSuperior.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    txtLimiteSuperior.SortMode = DataGridViewColumnSortMode.NotSortable;

                    dataGridView1.Columns.Insert(2, txtLimiteSuperior);

                    //Adiciona Numero de observacoes
                    DataGridViewTextBoxColumn txtNumObsData = new DataGridViewTextBoxColumn();
                    txtNumObsData = new DataGridViewTextBoxColumn();
                    txtNumObsData.Width = 100;
                    txtNumObsData.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                    txtNumObsData.ReadOnly = true;
                    txtNumObsData.HeaderText = "Número de observações";
                    txtNumObsData.DisplayIndex = 2;
                    txtNumObsData.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    txtNumObsData.SortMode = DataGridViewColumnSortMode.NotSortable;

                    dataGridView1.Columns.Insert(3, txtNumObsData);
                }

                clsUtilTools clsAlex = new clsUtilTools();

                double[,] dados = new double[dTable.Rows.Count, 1];
                double[,] cats = new double[num_classes, 1];
                double[,] dadosCats = new double[dTable.Rows.Count, 1];

                if (bool_quantitativo)
                {
                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        dados[i, 0] = Convert.ToDouble(dTable.Rows[i][cmbVariavel.SelectedItem.ToString()]);
                        dadosCats[i, 0] = Convert.ToDouble(classePoligonos[i]);
                    }

                    clsAlex.FrequencyTable(ref cats, dadosCats);
                    double minimo = clsAlex.Min(dados);
                    double maximo = clsAlex.Max(dados);

                    for (int i = 0; i < num_classes; i++) dataGridView1.Rows.Add();
                    for (int i = 0; i < num_classes; i++)
                    {
                        //if (dataGridView1.Rows.Count < num_classes) dataGridView1.Rows.Add();
                        if (i == 0)
                        {
                            dataGridView1.Rows[i].Cells[0].Style.BackColor = coresRGB[0];

                            dataGridView1.Rows[i].Cells[1].Value = /*Legenda[i] =*/ "Valores no intervalo [" + minimo.ToString() + "; " + classeMapaVetor[0] + "]";
                        }
                        else if (i > 0 || i < num_classes - 1)
                        {
                            dataGridView1.Rows[i].Cells[0].Style.BackColor = coresRGB[i];

                            dataGridView1.Rows[i].Cells[1].Value =/* Legenda[i] =*/ "Valores no intervalo (" + classeMapaVetor[i - 1] + "; " + classeMapaVetor[i] + "]";
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[0].Style.BackColor = coresRGB[i];

                            dataGridView1.Rows[i].Cells[1].Value =/* Legenda[i] =*/ "Valores no intervalo (" + classeMapaVetor[num_classes - 1] + "; " + maximo.ToString() + "]";
                        }

                        dataGridView1.Rows[i].Cells[2].Value = classeMapaVetor[i];
                        dataGridView1.Rows[i].Cells[3].Value = cats[i, 1];
                    }

                    editaNumObs = true;
                    //button1.Focus();
                    dataGridView1.Invalidate();
                    dataGridView1.Refresh();
                    dataGridView1.Focus();
                    dataGridView1.Update();
                    Application.DoEvents();
                }
                else
                {
                    //ToDoVariavel
                    string col_selected = cmbVariavel.SelectedItem.ToString();
                    string nome_col_temp = ("Coluna_temp_" + 1).Trim();
                    for (int i = 1; i <= 100000; i++)
                    {
                        nome_col_temp = ("Coluna_temp_" + i).Trim();
                        if (!dTable.Columns.Contains(nome_col_temp))
                        {
                            break;
                        }
                    }
                    dTable.Columns.Add(nome_col_temp, typeof(double));

                    clsUtilTools clt = new clsUtilTools();
                    object[,] categ = clt.DataTableToObjectMatrix(dTable, cmbVariavel.SelectedItem.ToString());
                    clt.FrequencyTable(ref categ, categ);
                    string[] scats = new string[categ.GetLength(0)];
                    for (int i = 0; i < scats.GetLength(0); i++)
                    {
                        scats[i] = categ[i, 0].ToString();
                    }

                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        for (int j = 0; j < scats.GetLength(0); j++)
                        {
                            if (dTable.Rows[i][col_selected].ToString() == scats[j])
                            {
                                dTable.Rows[i][nome_col_temp] = (double)j;
                                break;
                            }
                        }
                    }

                    int numero_classes = 0;

                    numero_classes = categ.GetLength(0);
                    numClasses.Value = numero_classes;                    

                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        dados[i, 0] = Convert.ToDouble(dTable.Rows[i][nome_col_temp]);
                        dadosCats[i, 0] = Convert.ToDouble(classePoligonos[i]);
                    }

                    clsAlex.FrequencyTable(ref cats, dadosCats);
                    for (int i = 0; i < num_classes; i++) dataGridView1.Rows.Add();
                    for (int i = 0; i < num_classes; i++)
                    {
                        dataGridView1.Rows[i].Cells[0].Style.BackColor = coresRGB[i];

                        dataGridView1.Rows[i].Cells[1].Value = Legenda[i] = categ[i, 0].ToString();
                        dataGridView1.Rows[i].Cells[3].Value = Legenda[i] = categ[i, 1].ToString();
                        dataGridView1.Rows[i].Cells[2].Value = cats[i, 0];                         
                    }
                    dTable.Columns.Remove(nome_col_temp);                    

                    //dataGridView1.Columns.RemoveAt(2);
                    editaNumObs = true;
                    //button1.Focus();
                    dataGridView1.Invalidate();
                    dataGridView1.Refresh();
                    dataGridView1.Focus();
                    dataGridView1.Update();
                    Application.DoEvents();                
                }

                dataGridView1.Rows[num_classes - 1].Cells[2].ReadOnly = true;

                if (!bool_quantitativo)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        dataGridView1.Rows[i].Cells[1].ReadOnly = true;
                        dataGridView1.Rows[i].Cells[2].ReadOnly = true;
                    }  
                }

                dataGridView1.ClearSelection();

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool erromanual = false;
        private bool classe_manual = false;

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2 && editaNumObs)
                {
                    classe_manual = true;
                    //Averigua se as classes são disjuntas, crescentes e união igual total
                    double[] classesmanuais = new double[dataGridView1.Rows.Count];
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        try
                        {
                            classesmanuais[i] = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                        }
                        catch
                        {
                            erromanual = true;
                            MessageBox.Show("As classes devem ser numéricas");
                            break;
                        }
                    }

                    if (!blSegunda)
                    {
                        for (int i = 0; i < classesmanuais.Length - 1; i++)
                        {
                            if (classesmanuais[i] > classesmanuais[i + 1])
                            {
                                erromanual = true;
                                MessageBox.Show("As classes devem ser ordenadas");
                                break;
                            }
                        }
                    }

                    if (!erromanual)
                    {
                        //Cria vetor de classes.
                        clsMapa classeMapa = new clsMapa();
                        classePoligonos = classeMapa.criaManual(dTable, cmbVariavel.SelectedItem.ToString(), 
                            strIDmapa, strID, (int)numClasses.Value, ref classeMapaVetor);

                        clsUtilTools clsAlex = new clsUtilTools();
                        double[,] dadosCats = new double[dTable.Rows.Count, 1];
                        double[,] cats = new double[(int)numClasses.Value, 1];
                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            dadosCats[i, 0] = Convert.ToDouble(classePoligonos[i]);
                        }
                        clsAlex.FrequencyTable(ref cats, dadosCats);
                        for (int i = 0; i < (int)numClasses.Value; i++)
                        {
                            dataGridView1.Rows[i].Cells[3].Value = cats[i, 1];
                        }
                        dataGridView1.Refresh();
                        dataGridView1.Update();
                        Application.DoEvents();
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ckbInverterEscalaCores_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Color[] cTemp = new Color[dataGridView1.RowCount];
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    cTemp[i] = dataGridView1.Rows[i].Cells[0].Style.BackColor;
                }

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1.Rows[dataGridView1.RowCount - i - 1].Cells[0].Style.BackColor = cTemp[i];
                }
                dataGridView1.Refresh();
                dataGridView1.Update();
                mudouCores = true;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ReinicializarEdicao()
        {
            if (tabControl1.TabPages.Contains(tabPage2)) tabControl1.TabPages.Remove(tabPage2);

            btnEditarCortes.Enabled = false;            
            ckbDefinicaoManual = false;
            blSegunda = false;
            editaNumObs = false;
            mudouCores = false;
        }

        private bool mudouCores = false;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    ColorDialog MyDialog = new ColorDialog();

                    // Keeps the user from selecting a custom color.
                    MyDialog.AllowFullOpen = true;

                    // Allows the user to get help. (The default is false.)
                    MyDialog.ShowHelp = true;

                    // Sets the initial color select to the current text color,
                    // so that if the user cancels out, the original color is restored.
                    MyDialog.Color = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;

                    // Open color selection dialog box
                    MyDialog.ShowDialog();

                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = MyDialog.Color;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = MyDialog.Color;
                    mudouCores = true;

                    button1.Focus();
                    dataGridView1.Refresh();
                    dataGridView1.Update();
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void numClasses_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ReinicializarEdicao();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Focus();
                dataGridView1.Refresh();
                dataGridView1.Update();
                dataGridView1.Invalidate();

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void XML()
        {
            frmMapaTematico_Load(this, new EventArgs());
            btnExecutar_Click(this, new EventArgs());
            button1_Click(this, new EventArgs());
        }
        //public void SalvarXML()
        //{
        //    Classes.clsArmazenamentoDados salvar = new Classes.clsArmazenamentoDados();

        //    if (coresRGB != null && m_legendas_manuais != null && info_mapa != null)
        //    {
        //        //salvar.CapturaDados(coresRGB, m_legendas_manuais, info_mapa[0], info_mapa[1], cor_selec, clear);
        //        //clear = false;
        //    }
        //}

        private static bool clear;
        //public void ClearXML()
        //{
        //    clear = true;
        //}
    }
}
