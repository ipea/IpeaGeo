using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace IpeaGeo
{
    public partial class frmScan : Form
    {
        public frmScan()
        {
            InitializeComponent();
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
        private int numeroClusters;
        public int NumeroDeClusters
        {
            get
            {
                return numeroClusters;
            }
            set
            {
                numeroClusters = value;
            }
        }
        private double[] vetPvalor;
        public double[] vetorPvalor
        {
            get
            {
                return vetPvalor;
            }
            set
            {
                vetPvalor = value;
            }
        }
        private double[] vetMonteCarlo;
        public double[] MonteCarlo
        {
            get
            {
                return vetMonteCarlo;
            }
            set
            {
                vetMonteCarlo = value;
            }
        }

        private SharpMap.Layers.VectorLayer layMapa;
        public SharpMap.Layers.VectorLayer LayerDoMapa
        {
            get
            {
                return layMapa;
            }
            set
            {
                layMapa = value;
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
        private string strID;
        public string IdentificadorBase
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

        private string varBase;
        public string VariavelBase
        {
            get
            {
                return varBase;
            }
            set
            {
                varBase = value;
            }
        }
        private string varEvento;
        public string VariavelEvento
        {
            get
            {
                return varEvento;
            }
            set
            {
                varEvento = value;
            }
        }
        private string strDistribuicao;
        public string Distribuicao
        {
            get
            {
                return strDistribuicao;
            }
            set
            {
                strDistribuicao = value;
            }
        }

        private string strSimulacoes;
        public string NumeroDeSimulacoes
        {
            get
            {
                return strSimulacoes;
            }
            set
            {
                strSimulacoes = value;
            }
        }
        private string strPontosGrid;
        public string NumeroDePontosGrid
        {
            get
            {
                return strPontosGrid;
            }
            set
            {
                strPontosGrid = value;
            }
        }
        private string strRaioMaximo;
        public string RaioMaximo
        {
            get
            {
                return strRaioMaximo;
            }
            set
            {
                strRaioMaximo = value;
            }
        }
        private string strRaioMinimo;
        public string RaioMinimo
        {
            get
            {
                return strRaioMinimo;
            }
            set
            {
                strRaioMinimo = value;
            }
        }
        private string strProporcaoMaxima;
        public string ProporcaoMaxima
        {
            get
            {
                return strProporcaoMaxima;
            }
            set
            {
                strProporcaoMaxima = value;
            }
        }
     
        private Brush[,] coresVetor = new Brush[110, 2];
        private Color[,] coresVetor2 = new Color[110, 2];
        private void frmScan_Load(object sender, EventArgs e)
        {
            try
            {

                lblStatus.Text = "";
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

                //Adiciona as variáveis
                for (int i = 0; i < dTable.Columns.Count; i++)
                {
                    if (dTable.Columns[i].ToString() != "Mapa" + strIDmapa && dTable.Columns[i].GetType() != Type.GetType("System.String"))
                    {
                        cmbBase.Items.Add(dTable.Columns[i].ToString());
                        cmbEvento.Items.Add(dTable.Columns[i].ToString());
                    }
                }

                //Inicializa o valor padrão da comboBox
                cmbCores.SelectedIndex = 0;
                cmbBase.SelectedIndex = 0;
                cmbEvento.SelectedIndex = 0;

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

        private void cmbCores_DrawItem_1(object sender, DrawItemEventArgs e)
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //Cores
                Brush[] cores = new Brush[(int)numCluster.Value + 1];
                Color[] coresRGB = new Color[(int)numCluster.Value + 1];


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
                double nClasses = Convert.ToDouble(numCluster.Value + 1);

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

                //Habilita a progress bar
                progressBar1.Visible = true;
                clsStatisticScan statScan = new clsStatisticScan();

                //Cria o vetor com o LLR
                double[] maxLLR = new double[(int)numCluster.Value];

                //Inicializa o vetor LLR
                for (int i = 0; i < (int)numCluster.Value; i++) maxLLR[i] = -999999;

                //Gera os clusters
                double[] vPvalor = new double[(int)numCluster.Value];
                if (strDistribuicao.ToUpper() == "BERNOULLI") classePoligonos = statScan.EstatisticaScanBinomial(dTable, strEnderecoMapa, (int)numCluster.Value, cmbEvento.SelectedItem.ToString(), cmbBase.SelectedItem.ToString(), ref progressBar1, ref lblStatus, ref maxLLR, (double)numProporcao.Value, (double)numRaioMin.Value, (double)numRaioMax.Value, (int)numGrid.Value, (int)numSimulacoes.Value, strIDmapa, strID, ref vPvalor, ref vetMonteCarlo);
                if (strDistribuicao.ToUpper() == "POISSON") classePoligonos = statScan.EstatisticaScanPoisson(dTable, strEnderecoMapa, (int)numCluster.Value, cmbEvento.SelectedItem.ToString(), cmbBase.SelectedItem.ToString(), ref progressBar1, ref lblStatus, ref maxLLR, (double)numProporcao.Value, (double)numRaioMin.Value, (double)numRaioMax.Value, (int)numGrid.Value, (int)numSimulacoes.Value, strIDmapa, strID, ref vPvalor, ref vetMonteCarlo);

                //Guarda p Pvalor
                vetPvalor = vPvalor;

                //Guarda as variaveis
                varBase = cmbBase.SelectedItem.ToString();
                varEvento = cmbEvento.SelectedItem.ToString();

                //Guarda o número de clusters
                numeroClusters = (int)numCluster.Value;

                //Passar o boolean para o formulário frmMapa gerar o relatório
                blGeraRelatorio = chkRelatorio.Checked;

                strPontosGrid = numGrid.Value.ToString();
                strRaioMaximo = numRaioMax.Value.ToString();
                strRaioMinimo = numRaioMin.Value.ToString();
                strProporcaoMaxima = numProporcao.Value.ToString();
                strSimulacoes = numSimulacoes.Value.ToString();

                Cursor.Current = Cursors.Default;

                //OK
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
