using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace IpeaGEO
{
    public partial class frmMapaTematico : Form
    {
        public frmMapaTematico()
        {
            InitializeComponent();
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

        private Brush[,] coresVetor = new Brush[110, 2];
        private Color[,] coresVetor2 = new Color[110, 2];


        private void frmMapaTematico_Load(object sender, EventArgs e)
        {

            

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
            int contador=0;

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
                        
            //Adiciona as variáveis
            for (int i = 0; i < dTable.Columns.Count; i++)
            {
                if (dTable.Columns[i].ToString() != "Mapa" + strIDmapa)
                {
                    cmbVariavel.Items.Add(dTable.Columns[i].ToString());
                }
            }

            //Inicializa o valor padrão da comboBox
            cmbCores.SelectedIndex = 0;
            cmbMetodo.SelectedIndex = 0;
            cmbVariavel.SelectedIndex = 0;
            cmbDesvio.SelectedIndex = 0;
        }

        private void btnCancela_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                double[] desvios = new double[7] { 1, 0.8, 0.6, 0.5, 0.4, 0.2, 0.1 };
                //Guarda as classes do mapa
                double[] dblClasseMapa = new double[(int)numClasses.Value];

                //Tipo do dados
                string strTipo = dTable.Columns[cmbVariavel.SelectedItem.ToString()].DataType.ToString();

                if (strTipo != "System.String")
                {
                    clsMapa classeMapa = new clsMapa();
                    if (cmbMetodo.SelectedItem.ToString() == "Quantil")
                    {
                        classePoligonos = classeMapa.criaQuantis(dTable, cmbVariavel.SelectedItem.ToString(), strIDmapa, strID, (int)numClasses.Value, ref dblClasseMapa);
                        
                    }
                    else if (cmbMetodo.SelectedItem.ToString() == "Jenks")
                    {
                        classePoligonos = classeMapa.criaJenks(dTable, cmbVariavel.SelectedItem.ToString(), strIDmapa, strID, (int)numClasses.Value, ref dblClasseMapa);
                    }
                    else if (cmbMetodo.SelectedItem.ToString() == "Desvio Padrão")
                    {

                        classePoligonos = classeMapa.criaDesvios(dTable, cmbVariavel.SelectedItem.ToString(), strIDmapa, strID, desvios[cmbDesvio.SelectedIndex] , ref dblClasseMapa);
                    }
                    else if (cmbMetodo.SelectedItem.ToString() == "Geométrico")
                    {
                        classePoligonos = classeMapa.criaGeometric(dTable, cmbVariavel.SelectedItem.ToString(), strIDmapa, strID, (int)numClasses.Value, ref dblClasseMapa);
                    }

                    else if (cmbMetodo.SelectedItem.ToString() == "Equal")
                    {
                        classePoligonos = classeMapa.criaEqual(dTable, cmbVariavel.SelectedItem.ToString(), strIDmapa, strID, (int)numClasses.Value, ref dblClasseMapa);
                    }

                    //Tipo de mapa temático
                    strMetodo = cmbMetodo.SelectedItem.ToString();

                    //Guarda a variável
                    strVariavelMapa = cmbVariavel.SelectedItem.ToString();

                    //Guarda a classe
                    classeMapaVetor = dblClasseMapa;

                    //Cores
                    //Cores
                    Brush[] cores = new Brush[(int)numClasses.Value];
                    Color[] coresRGB = new Color[(int)numClasses.Value];
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

                    //OK
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("A variável precisa ser numérica.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void cmbMetodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMetodo.SelectedItem.ToString() == "Desvio Padrão")
            {
                numClasses.Visible = false;
                cmbDesvio.Visible = true;
                label2.Text = "Desvios";
            }
            else
            {
                numClasses.Visible = true;
                cmbDesvio.Visible = false;
                label2.Text = "Classes";
            }
            Application.DoEvents();
        }

        private void chkAleatorio_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAleatorio.Checked) cmbCores.Enabled = false;
            else cmbCores.Enabled = true;
        }
    }
}
