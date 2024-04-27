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
using IpeaGeo;
using System.Data.SqlClient;

namespace IpeaGeo.Forms
{
    public partial class frmMultiMapas : Form
    {
        public frmMultiMapas()
        {
            InitializeComponent();
        }

        private Brush[,] coresVetor = new Brush[110, 2];
        private Color[,] coresVetor2 = new Color[110, 2];

        private string endereco;
        public string endereco_
        {
            get
            {
                return endereco;
            }
            set
            {
                endereco = value;
            }
        }

        public Brush[,] coresVetor_
        {
            get
            {
                return coresVetor;
            }
            set
            {
                coresVetor = value;
            }
        }
        public Color[,] coresVetor2_
        {
            get
            {
                return coresVetor2;
            }
            set
            {
                coresVetor2 = value;
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
        private string[] varsMapa;
        public string[] varsMapa_
        {
            get
            {
                return varsMapa;
            }
            set
            {
                varsMapa = value;
            }
        }
        private string[] metodosMapa;
        public string[] metodosMapa_
        {
            get
            {
                return metodosMapa;
            }
            set
            {
                metodosMapa = value;
            }
        }
        private int[] classesMapa;
        public int[] classesMapa_
        {
            get
            {
                return classesMapa;
            }
            set
            {
                classesMapa = value;
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
        private void PopulaDataGridView(ref DataGridView dataGridView1, string[] strVariaveis)
        {
            //Cria as colunas do DataGridView
            DataGridViewTextBoxColumn txtbox = new DataGridViewTextBoxColumn();
            txtbox = new DataGridViewTextBoxColumn();
            //txtbox.Width = 260;
            txtbox.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
            txtbox.ReadOnly = true;
            txtbox.HeaderText = "Variável";
            //txtbox.DisplayIndex = 2;
            dataGridView1.Columns.Insert(0, txtbox);

            DataGridViewCheckBoxColumn ckbvar = new DataGridViewCheckBoxColumn();
            ckbvar.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
            ckbvar.Width = 60;
            ckbvar.FlatStyle = FlatStyle.Popup;
            ckbvar.HeaderText = "Selecionar";
            //ckbvar.DisplayIndex = 2;
            ckbvar.TrueValue = true;
            dataGridView1.Columns.Insert(1, ckbvar);

            txtbox = new DataGridViewTextBoxColumn();
            txtbox = new DataGridViewTextBoxColumn();
            //txtbox.Width = 260;
            txtbox.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
            txtbox.ReadOnly = true;
            txtbox.HeaderText = "Título";
            //txtbox.DisplayIndex = 2;
            dataGridView1.Columns.Insert(2, txtbox);

            txtbox = new DataGridViewTextBoxColumn();
            txtbox = new DataGridViewTextBoxColumn();
            //txtbox.Width = 260;
            txtbox.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
            txtbox.ReadOnly = true;
            txtbox.HeaderText = "Fonte";
            //txtbox.DisplayIndex = 2;
            dataGridView1.Columns.Insert(3, txtbox);

            DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
            combo.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
            combo.FlatStyle = FlatStyle.Popup;
            combo.HeaderText = "Metodologia";
            string[] y = new string[6];            
            y[0] = "Quantil";
            y[1] = "Equal";
            y[2] = "Jenks";
            y[3] = "Desvio Padrão";
            y[4] = "Geométrico";
            y[5] = "Valores Únicos";
            combo.Items.AddRange(y);  
            dataGridView1.Columns.Insert(4, combo);

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
            comboBox1.DataSource = colorList;
            comboBox1.DisplayMember = "Nome";
            comboBox1.ValueMember = "Nome";  

            //Inicializa o valor padrão da comboBox
            comboBox1.SelectedIndex = 0;
            
            combo = new DataGridViewComboBoxColumn();
            combo.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
            combo.FlatStyle = FlatStyle.Popup;
            combo.HeaderText = "Classes";
            string[] yy = new string[20];
            yy[0] = "1";
            yy[1] = "2";
            yy[2] = "3";
            yy[3] = "4";
            yy[4] = "5";
            yy[5] = "6";
            yy[6] = "7";
            yy[7] = "8";
            yy[8] = "9";
            yy[9] = "10";
            yy[10] = "11";
            yy[11] = "12";
            yy[12] = "13";
            yy[13] = "14";
            yy[14] = "15";
            yy[15] = "16";
            yy[16] = "17";
            yy[17] = "18";
            yy[18] = "19";
            yy[19] = "20";            
            combo.Items.AddRange(yy);
            dataGridView1.Columns.Insert(5, combo);

            //txtbox = new DataGridViewTextBoxColumn();
            //txtbox = new DataGridViewTextBoxColumn();
            //txtbox.Width = 260;
            //txtbox.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
            //txtbox.ReadOnly = true;
            //txtbox.HeaderText = "Classes";
            //txtbox.DisplayIndex = 2;
            //dataGridView1.Columns.Insert(5, txtbox);

            //Adiciona as variáveis
            dataGridView1.Rows.Add(strVariaveis.Length);
            for (int i = 0; i < strVariaveis.Length; i++)
            {
                dataGridView1.Rows[i].Cells[1].Value = false;
                dataGridView1.Rows[i].Cells[0].ToolTipText = strVariaveis[i];
                dataGridView1.Rows[i].Cells[0].Value = strVariaveis[i];

                dataGridView1.Rows[i].Cells[2].Value = strVariaveis[i];
                dataGridView1.Rows[i].Cells[3].Value = "Inserir fonte";

                dataGridView1.Rows[i].Cells[4].Value = "Quantil";
                dataGridView1.Rows[i].Cells[5].Value = "5";
                
            }
            
        }




        private void frmMultiMapas_Load(object sender, EventArgs e)
        {
            
            //Preenchendo os objetos do formulário
            string[] strvariaveis = new string[dTable.Columns.Count];
            

            for (int i = 0; i < dTable.Columns.Count; i++)
            {
                strvariaveis[i] = dTable.Columns[i].ColumnName; 
            }
            
            //checkedListBox1.Items.AddRange(strvariaveis);
            

            PopulaDataGridView(ref dataGridView1, strvariaveis);




        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //Executando os mapas para cada by e variável
                //int numBy = 0;
                int numVar = 0;
                //int contador = 0;

                //numBy = checkedListBox1.SelectedItems.Count;
                /*
                if (numBy > 0)
                {
                    string[] nomesBy = new string[numBy];
                    for (int i = 0; i < numBy; i++)
                    {
                        nomesBy[i] = checkedListBox1.SelectedItems[contador].ToString();
                        contador++;
                    }
                }
                else
                {
                    //MessageBox.Show("Selecionar pelo menos um by");
                }
                */
                
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    bool var = (bool)dataGridView1.Rows[i].Cells[1].Value;
                    if (var)
                    { 
                        numVar++; 
                    }               
                }
                if (numVar > 0)
                {
                    string[] nomesVar = new string[numVar];
                }
                else
                {
                    MessageBox.Show("Selecionar pelo menos uma variável");
                }
                int[] indicesvar = new int[numVar];
                int count = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    bool var = (bool)dataGridView1.Rows[i].Cells[1].Value;
                    if (var)
                    {
                        indicesvar[count] = i;
                        count++;
                    }
                }

                //TODO:
                //Iniciar os dois for, para cada by e para cada variavel
                
                //Fazendo o for somente por variavel//////////////////////////////////////////////////////
                //Gerar um mapa para cada variavel e exportar////////////////////////////////////////////
                

                varsMapa = new string[numVar];
                classesMapa = new int[numVar];
                metodosMapa = new string[numVar];

                for (int vars = 0; vars < numVar; vars++)                
                {                    

                    //Variáveis de interesse

                    int numClasses = Convert.ToInt32(dataGridView1.Rows[indicesvar[vars]].Cells[5].Value);
                    string cmbVariavel = dataGridView1.Rows[indicesvar[vars]].Cells[0].Value.ToString();
                    string cmbMetodo = dataGridView1.Rows[indicesvar[vars]].Cells[4].Value.ToString();



                    varsMapa[vars] = dataGridView1.Rows[indicesvar[vars]].Cells[0].Value.ToString();
                    classesMapa[vars] = Convert.ToInt32(dataGridView1.Rows[indicesvar[vars]].Cells[5].Value);
                    metodosMapa[vars] = dataGridView1.Rows[indicesvar[vars]].Cells[4].Value.ToString();


                    /*
                    //Cópia do mapa temático////////////////////////////////////////////////////////////////////
                    this.Cursor = Cursors.WaitCursor;
                    double[] desvios = new double[7] { 1, 0.8, 0.6, 0.5, 0.4, 0.2, 0.1 };
                    //Guarda as classes do mapa
                    double[] dblClasseMapa = new double[(int)numClasses];

                    //Tipo do dados
                    string strTipo = dTable.Columns[cmbVariavel].DataType.ToString();

                    if (strTipo != "System.String")
                    {
                        clsMapa classeMapa = new clsMapa();
                        if (cmbMetodo == "Quantil")
                        {
                            classePoligonos = classeMapa.criaQuantis(dTable, cmbVariavel, strIDmapa, strID, (int)numClasses, ref dblClasseMapa);

                        }
                        else if (cmbMetodo == "Jenks")
                        {
                            classePoligonos = classeMapa.criaJenks(dTable, cmbVariavel, strIDmapa, strID, (int)numClasses, ref dblClasseMapa);
                        }
                        else if (cmbMetodo == "Desvio Padrão")
                        {
                            int numero_class = 0;
                            classePoligonos = classeMapa.criaDesvios(dTable, cmbVariavel, strIDmapa, strID, numClasses, ref dblClasseMapa, ref numero_class);
                            numClasses = numero_class;
                        }
                        else if (cmbMetodo == "Geométrico")
                        {
                            classePoligonos = classeMapa.criaGeometric(dTable, cmbVariavel, strIDmapa, strID, (int)numClasses, ref dblClasseMapa);
                        }

                        else if (cmbMetodo == "Equal")
                        {
                            classePoligonos = classeMapa.criaEqual(dTable, cmbVariavel, strIDmapa, strID, (int)numClasses, ref dblClasseMapa);
                        }
                        else if (cmbMetodo == "Valores Únicos")
                        {
                            int numero_classes = 0;
                            ArrayList valores_diferentes = new ArrayList();
                            for (int i = 0; i < dTable.Rows.Count; i++)
                            {
                                if (!valores_diferentes.Contains(dTable.Rows[i][cmbVariavel]))
                                {
                                    valores_diferentes.Add(dTable.Rows[i][cmbVariavel]);
                                }
                            }
                            numero_classes = valores_diferentes.Count;
                            numClasses = numero_classes;
                            dblClasseMapa = new double[numero_classes];


                            classePoligonos = classeMapa.criaValoresUnicos(dTable, cmbVariavel, strIDmapa, strID, numero_classes, ref dblClasseMapa);
                        }                        

                        //Tipo de mapa temático
                        strMetodo = cmbMetodo;

                        //Guarda a variável
                        strVariavelMapa = cmbVariavel;

                        //Guarda a classe
                        classeMapaVetor = ClasseDoMapa;                        

                        //Cores
                        //Cores*/                   
               
                        Brush[] cores = new Brush[(int)numClasses];
                        Color[] coresRGB = new Color[(int)numClasses];
                        strCoresRGB = new string[cores.Length];
                    
                        if (checkBox1.Checked == false)
                        {
                            //Item escolhido do ComboBox de cores
                            int iTem = comboBox1.SelectedIndex;

                            //Inicializa as cores
                            cores[0] = coresVetor[iTem, 0];
                            cores[cores.Length - 1] = coresVetor[iTem, 1];

                            //Converte para COLOR
                            Color[] colors = new Color[2];
                            colors[0] = coresVetor2[iTem, 0];
                            colors[1] = coresVetor2[iTem, 1];

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
                            double nClasses = Convert.ToDouble(numClasses);

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
                }

                frmMapa mm = new frmMapa();
                mm.variaveis_multi_mapas = varsMapa;
                mm.numclasses_multi_mapas = classesMapa;
                mm.metodo_multi_mapas = metodosMapa;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                saveFileDialog.Filter = "JPEG (*.jpeg)|*.jpeg|PDF (*.pdf)|*.pdf";
                saveFileDialog.FileName = "";
                
                string FileName = "";
                
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    endereco = saveFileDialog.FileName;
                    mm.endereco_multi_mapas = endereco;
                }
            }
            catch { }
        }

        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                // Pega o item a ser pintado
                GradientColor selectedItem = (GradientColor)this.comboBox1.Items[e.Index];

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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked) comboBox1.Enabled = false;
                else comboBox1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[1].Value = true;
                }
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[1].Value = false;
                }
            }
            
        }
    }
}
