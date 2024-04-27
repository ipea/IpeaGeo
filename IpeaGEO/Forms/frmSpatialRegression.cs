using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;
using ZedGraph;

namespace IpeaGeo
{
    public partial class frmSpatialRegression : Form
    {

        #region Métodos Get Set

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
           
            set
            {
                strVariaveis = value;
            }
        }
        private string[] strVariaveisSelecionadas;
        public string[] VariaveisSelecionadas
        {
            
            set
            {
                strVariaveisSelecionadas = value;
            }
        }
        private string[] strVariaveisSelecionadasInstrumentos;
        public string[] VariaveisSelecionadasInstrumentos
        {
            
            set
            {
                strVariaveisSelecionadasInstrumentos = value;
            }
        }
        private int[] classePoligonos;
        public int[] vetorPoligonos
        {
            
            set
            {
                classePoligonos = value;
            }
        }
        private string strIDmapa;
        public string IdentificadorMapa
        {
            
            set
            {
                strIDmapa = value;
            }
        }
     
        private bool blRelatorio;
        public bool GeraRelatorio
        {
           
            set
            {
                blRelatorio = value;
            }
        }

        private string strID;
        public string IdentificadorDados
        {
            
            set
            {
                strID = value;
            }
        }
        private DataTable dTable;
        public DataTable DataTableDados
        {

            set
            {
                dTable = value;
            }
        }

        private string strEnderecoMapa;
        public string EnderecoMapa
        {
           
            set
            {
                strEnderecoMapa = value;
            }
        }
        private string strDistancia;
        public string Distancia
        {
           
            set
            {
                strDistancia = value;
            }
        }

        private string strMetodo;
        public string Metodo
        {
            
            set
            {
                strMetodo = value;
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
        private int[] iVetor;
        public int[] Residuos_Mapa
        {
            get
            {
                return iVetor;
            }
            set
            {
                iVetor = value;
            }
        }

        private string Metodo_mapa;
        public string Metodo_selecionado
        {
            get
            {
                return Metodo_mapa;
            }
            set
            {
                Metodo_mapa = value;
            }
        }

        private string var_sel;
        public string Variavel_selecionada_mapa
        {
            get
            {
                return var_sel;
            }
            set
            {
                var_sel = value;
            }
        }

        private double[] limites;
        public double[] Limites_Classes
        {
            get
            {
                return limites;
            }
            set
            {
                limites = value;
            }
        }

        private string[] RGB;
        public string[] CoresRGB
        {
            get
            {
                return RGB;
            }
            set
            {
                RGB = value;
            }
        }

        public DataTable Tabela
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
        private string[] Nomes_Var;
        public string[] sBetas
        {
            get
            {
                return Nomes_Var;
            }
            set
            {
                Nomes_Var = value;
            }
        }
        private double[] Valores_betas;
        public double[] dBetas
        {
            get
            {
                return Valores_betas;
            }
            set
            {
                Valores_betas = value;
            }
        }
        private double[] t_betas;
        public double[] t
        {
            get
            {
                return t_betas;
            }
            set
            {
                t_betas = value;
            }
        }

        private double[] p_betas;
        public double[] pvalores
        {
            get
            {
                return p_betas;
            }
            set
            {
                p_betas = value;
            }
        }

        private double[] desv;
        public double[] desvios_betas
        {
            get
            {
                return desv;
            }
            set
            {
                desv = value;
            }
        }

        private double prov_aic;
        public double aic
        {
            get
            {
                return prov_aic;
            }
            set
            {
                prov_aic = value;
            }
        }
        private double prov_bic;
        public double bic
        {
            get
            {
                return prov_bic;
            }
            set
            {
                prov_bic = value;
            }
        }
        private double prov_loglik;
        public double loglikeli
        {
            get
            {
                return prov_loglik;
            }
            set
            {
                prov_loglik = value;
            }
        }
        private double prov_sigma2;
        public double sigma_2
        {
            get
            {
                return prov_sigma2;
            }
            set
            {
                prov_sigma2 = value;
            }
        }

        private double prov_rho;
        public double rho
        {
            get
            {
                return prov_rho;
            }
            set
            {
                prov_rho = value;
            }
        }

        private double prov_rho_t;
        public double rho_t
        {
            get
            {
                return prov_rho_t;
            }
            set
            {
                prov_rho_t = value;
            }
        }

        private double prov_rho_p;
        public double rho_p
        {
            get
            {
                return prov_rho_p;
            }
            set
            {
                prov_rho_p = value;
            }
        }

        private double prov_rho_desv;
        public double rho_desv
        {
            get
            {
                return prov_rho_desv;
            }
            set
            {
                prov_rho_desv = value;
            }
        }

        private string metreg;
        public string Metodo_Regressao
        {
            get
            {
                return metreg;
            }
            set
            {
                metreg = value;
            }
        }

        private string var_para_mapa;
        public string Variavel_Mapa
        {
            get
            {
                return var_para_mapa;
            }
            set
            {
                var_para_mapa = value;
            }
        }

        private bool isso_eh_regressao;
        public bool Regressao_Feita
        {
            get
            {
                return isso_eh_regressao;
            }
            set
            {
                isso_eh_regressao = value;
            }
        }

        public DataTable DataTable_Residuos
        {
            get
            {
                return residuos;
            }
            set
            {
                residuos = value;
            }
        }


        private Brush[,] coresVetor = new Brush[110, 2];
        private Color[,] coresVetor2 = new Color[110, 2];
        #endregion 


        public frmSpatialRegression()
        {
            InitializeComponent();
        }

        private void frmSpatialRegression_Load(object sender, EventArgs e)
        {
            try
            {

                //Popula listbox

                ckbX.Items.AddRange(strVariaveis);
                ckbInstrumentos.Items.AddRange(strVariaveis);
                comboBoxDependente.Items.AddRange(strVariaveis);
                comboBoxDependente.SelectedIndex = 0;
                //comboBox6.SelectedIndex = 0;
                this.label2.Visible = true;

                //Habilita a cmbVizinhanca
                if (shapeAlex.TipoVizinhanca == "") cmbVizinhanca.Enabled = true;

                //Popula metodos
                string[] metodos = { "SAR", "SEM", "GMM", "GMM (Conley)", "2SLS", "OLS", "OLS (Conley)" };
                comboBox2.Items.AddRange(metodos);

                string[] metodosmapa = { "Quantil", "Equal", "Jenks", "Geométrico", "Desvio Padrão" };
                comboBox1.Items.AddRange(metodosmapa);

                //Popula variaveis para centroide
                comboBox4.Items.AddRange(strVariaveis);
                comboBox5.Items.AddRange(strVariaveis);

                string[] itemsmapa = { "Resíduos", "Valores Estimados" };
                comboBox3.Items.AddRange(itemsmapa);

                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                comboBox3.SelectedIndex = 0;
                cmbVizinhanca.SelectedIndex = 0;




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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Gerando os datatables
        private DataTable betas = new DataTable();
        private DataTable outras = new DataTable();
        private DataTable residuos = new DataTable();

        private void EstimaRegressao()
        {
            try
            {

                /*
                //Testando exibição de dataset
                DataSet teste = new DataSet();
                DataTable dados1 = new DataTable("1");
                DataTable dados2 = new DataTable("2");
                dados1.Columns.Add("Coluna1");
                dados1.Columns.Add("Col2");
                dados2.Columns.Add("Col1");
                dados2.Columns.Add("col2");

                DataRow linhar = dados1.NewRow();
                linhar[0] = 1;
                linhar[1] = 1000;

                DataRow linh2 = dados2.NewRow();
                linh2[0] = 23;
                linh2[1] = "Teste";

                dados1.Rows.Add(linhar);
                dados2.Rows.Add(linh2);

                teste.Tables.Add(dados1);
                teste.Tables.Add(dados2);
                dataGridView1.DataSource = teste;
              
             
                //Fazer o datagridview exibir a table "betas"
                BindingSource bs = new BindingSource();
                bs.DataSource = teste;
                dataGridView1.DataSource = bs;

                dataGridView1.Update();
                dataGridView1.Refresh();

                int u = 0;
                */


                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                //Verificar se há matriz de vizinhanças

                if (shapeAlex.TipoVizinhanca == "")
                {
                    //Modifica o label
                    label2.Text = "Gerando a matriz de vizinhanças...";
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



                groupBox4.Enabled = true;
                comboBox3.Enabled = true;

                //Conta quantas variaveis independentes foram escolhidas
                int numero1 = ckbX.CheckedItems.Count;
                int numeroZ = ckbInstrumentos.CheckedItems.Count;

                int dummyintercepto = 0;
                if (cbxIntercepto.Checked == false) dummyintercepto++;

                //Criando as matrizes
                double[,] matriz_y = new double[dTable.Rows.Count, 1];
                double[,] matriz_x = new double[dTable.Rows.Count, numero1];
                double[,] matriz_z = new double[dTable.Rows.Count, numeroZ];

                double[,] vetor_betas = new double[numero1 + numeroZ + dummyintercepto, 1];
                double[,] lim_inf_betas = new double[numero1 + numeroZ + dummyintercepto, 1];
                double[,] lim_sup_betas = new double[numero1 + numeroZ + dummyintercepto, 1];
                double[,] vetor_residuos = new double[dTable.Rows.Count, 1];

                double[,] vetor_t = new double[numero1 + numeroZ + dummyintercepto, 1];
                double[,] desvios_padrao_betas = new double[numero1 + numeroZ + dummyintercepto, 1];
                double[,] p_valores = new double[numero1 + numeroZ + dummyintercepto, 1];
                double[,] cov_betas = new double[numero1 + numeroZ + dummyintercepto, numero1 + numeroZ + dummyintercepto];
                double sigma2 = 0;
                double cutoff_x = 0;
                double cutoff_y = 0;
                double j_stat = 0;
                double j_pvalor = 0;

                //Definindo o cutoff


                clsMapa cmm = new clsMapa();

                //valor_escala_x = (cmm.Sharp_Mapa.Envelope.Right) - (cmm.Sharp_Mapa.Envelope.Left);
                //valor_escala_y = (cmm.Sharp_Mapa.Envelope.Top) - (cmm.Sharp_Mapa.Envelope.Bottom);



                if (nudCutoffLatitude.Value != 0)
                {
                    cutoff_x = (double)nudCutoffLatitude.Value;
                }
                else if (nudCutoffLatitude.Value == 0)
                {
                    cutoff_x = 10;
                }

                if (nudCutoffLongitude.Value != 0)
                {
                    cutoff_y = (double)nudCutoffLongitude.Value;
                }
                else if (nudCutoffLongitude.Value == 0)
                {
                    cutoff_y = 10;
                }

                //Matrizes de coordenadas
                double[,] coord_X = new double[dTable.Rows.Count, 1];
                double[,] coord_Y = new double[dTable.Rows.Count, 1];

                if (comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1)
                {

                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        coord_X[i, 0] = Convert.ToDouble(dTable.Rows[i][comboBox5.SelectedIndex]);
                    }


                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        coord_Y[i, 0] = Convert.ToDouble(dTable.Rows[i][comboBox4.SelectedIndex]);
                    }
                }
                //Variáveis Selecionadas Regressão
                strVariaveisSelecionadas = new string[ckbX.CheckedItems.Count];

                for (int i = 0; i < ckbX.CheckedItems.Count; i++)
                {
                    strVariaveisSelecionadas[i] = Convert.ToString(ckbX.CheckedItems[i]);
                }

                //Variáveis Selecionadas Instrumentos
                strVariaveisSelecionadasInstrumentos = new string[ckbInstrumentos.CheckedItems.Count];

                for (int i = 0; i < ckbInstrumentos.CheckedItems.Count; i++)
                {
                    strVariaveisSelecionadasInstrumentos[i] = Convert.ToString(ckbInstrumentos.CheckedItems[i]);
                }

                //Populating these matrixes
                int posicaox = 0;
                int posicaoz = 0;
                for (int i = 0; i < matriz_y.Length; i++)
                {
                    DataRow dr = dTable.Rows[i];
                    Type tp = dr[comboBoxDependente.SelectedIndex].GetType();


                    if (tp == typeof(System.DBNull))
                    {
                        double nulo = double.NaN;
                        matriz_y[i, 0] = nulo;
                    }
                    else if (tp != typeof(System.DBNull))
                    {
                        matriz_y[i, 0] = Convert.ToDouble(dTable.Rows[i][comboBoxDependente.SelectedIndex]);
                    }

                    for (int j = 0; j < strVariaveisSelecionadas.Length; j++)
                    {
                        tp = dr[strVariaveisSelecionadas[j]].GetType();
                        if (tp == typeof(System.DBNull))
                        {
                            double nulo = double.NaN;
                            matriz_x[i, j] = nulo;
                        }
                        else if (tp != typeof(System.DBNull))
                        {
                            matriz_x[i, j] = Convert.ToDouble(dTable.Rows[i][strVariaveisSelecionadas[j]]);
                            posicaox = j;
                        }
                    }

                    for (int j = 0; j < strVariaveisSelecionadasInstrumentos.Length; j++)
                    {
                        tp = dr[strVariaveisSelecionadasInstrumentos[j]].GetType();
                        if (tp == typeof(System.DBNull))
                        {
                            double nulo = double.NaN;
                            matriz_z[i, j] = nulo;
                        }
                        else if (tp != typeof(System.DBNull))
                        {
                            matriz_z[i, j] = Convert.ToDouble(dTable.Rows[i][strVariaveisSelecionadasInstrumentos[j]]);
                            posicaoz = j;
                        }
                    }

                }

                clsUtilTools clsUtil = new clsUtilTools();
                double[,] intercepto = clsUtil.Ones(matriz_x.GetLength(0), 1);

                if (!this.cbxIntercepto.Checked)
                {
                    matriz_x = clsUtil.Concateh(intercepto, matriz_x);
                    matriz_z = clsUtil.Concateh(intercepto, matriz_z);
                }

                clsSpatialGMM clsGMM = new clsSpatialGMM();

                //Minimos quadrados
                //Se OLS (Conley), os instrumentos são iguais às variaveis
                if (comboBox2.SelectedItem.ToString() == "OLS")
                {
                    metreg = "OLS";
                    clsGMM.Simple_OLS_estimation(matriz_y, matriz_x, ref vetor_betas, ref cov_betas, ref desvios_padrao_betas, ref vetor_t, ref p_valores, ref sigma2);
                }

                if (comboBox2.SelectedItem.ToString() == "2SLS")
                {
                    metreg = "2SLS";
                    clsGMM.Simple_2SLS_estimation(matriz_y, matriz_x, matriz_z, ref vetor_betas, ref cov_betas, ref desvios_padrao_betas, ref vetor_t, ref p_valores, ref sigma2);
                }

                if (comboBox2.SelectedItem.ToString() == "OLS (Conley)")
                {
                    metreg = "OLS (Conley)";
                    ckbInstrumentos.Enabled = false;
                    strVariaveisSelecionadasInstrumentos = new string[ckbX.CheckedItems.Count];
                    for (int i = 0; i < ckbX.CheckedItems.Count; i++)
                    {
                        strVariaveisSelecionadasInstrumentos[i] = Convert.ToString(ckbX.CheckedItems[i]);
                    }
                    clsGMM.Simple_2SLS_estimation(matriz_y, matriz_x, matriz_z, ref vetor_betas, ref cov_betas, ref desvios_padrao_betas, ref vetor_t, ref p_valores, ref sigma2);
                }

                //Generalized method of moments

                if (comboBox2.SelectedItem.ToString() == "GMM")
                {
                    metreg = "GMM";
                    //clsGMM.Limited_info_GMM_estimation(matriz_y, matriz_x, matriz_z, ref vetor_betas, ref cov_betas, ref desvios_padrao_betas, ref vetor_t, ref p_valores, ref j_stat, ref j_pvalor, ref sigma2);
                }

                if (comboBox2.SelectedItem.ToString() == "GMM (Conley)")
                {
                    metreg = "GMM (Conley)";
                    //clsGMM.Limited_info_spatial_GMM_estimation(matriz_y, matriz_x, matriz_z, coord_X, coord_Y, cutoff_x, cutoff_y, ref vetor_betas, ref cov_betas, ref desvios_padrao_betas, ref vetor_t, ref p_valores, ref j_stat, ref j_pvalor, ref sigma2);
                }

                //SAR e SEM

                //clsLinearRegressionModelsMLE clsMLE = new clsLinearRegressionModelsMLE();
                //clsMLE.Shape = shapeAlex;
                //clsMLE.Y = matriz_y;
                //clsMLE.X = matriz_x;
                //clsMLE.Z = matriz_z;

                double aic = 0;
                double bic = 0;
                double log_likelihood = 0;
                double rho = 0;
                double lim_inf_rho = 0;
                double lim_sup_rho = 0;
                double rho_pvalue = 0;
                double rho_std = 0;
                double rho_t = 0;
                double[,] res_pond = new double[0, 0];
                
              //Gerando vetor de residuos, caso o modelo não seja SAR ou SEM
                clsUtilTools clsut = new clsUtilTools();
                double[,] predito = clsut.MatrizMult(matriz_x, vetor_betas);
                vetor_residuos = clsut.MatrizSubtracao(matriz_y, predito);
                



                if (comboBox2.SelectedItem.ToString() == "SAR")
                {
                    label2.Text = "Modelo SAR";
                    Application.DoEvents();
                    metreg = "SAR";

                    //clsMLE.TipoModeloRegressaoEspacial = TipoModeloEspacial.SAR;

                    //clsMLE.EstimateModeloSAR();
                    //vetor_betas = clsMLE.BetaHat;
                    //lim_inf_betas = clsMLE.BetaLimInfCI;
                    //lim_sup_betas = clsMLE.BetaLimSupCI;
                    //vetor_t = clsMLE.BetaTStat;
                    //desvios_padrao_betas = clsMLE.BetaStdError;
                    //p_valores = clsMLE.BetaPValue;
                    //aic = clsMLE.AIC;
                    //bic = clsMLE.BIC;
                    //log_likelihood = clsMLE.LogLik;
                    //vetor_residuos = clsMLE.Residuos;
                    //res_pond = clsMLE.ResiduosDefasados;
                    //rho = clsMLE.RhoHat;
                    //lim_inf_rho = clsMLE.RhoLimInfCI;
                    //lim_sup_rho = clsMLE.RhoLimSupCI;
                    //rho_pvalue = clsMLE.RhoPValue;
                    //rho_std = clsMLE.RhoStdError;
                    //rho_t = clsMLE.RhoTStat;
                    //sigma2 = clsMLE.Sigma2Hat;


                    label2.Text = "";
                    Application.DoEvents();
                }

                if (comboBox2.SelectedItem.ToString() == "SEM")
                {
                    label2.Text = "Modelo SEM";
                    Application.DoEvents();
                    metreg = "SEM";

                    //clsMLE.TipoModeloRegressaoEspacial = TipoModeloEspacial.SEM;

                    //clsMLE.EstimateModeloSEM();
                    //vetor_betas = clsMLE.BetaHat;
                    //lim_inf_betas = clsMLE.BetaLimInfCI;
                    //lim_sup_betas = clsMLE.BetaLimSupCI;
                    //vetor_t = clsMLE.BetaTStat;
                    //desvios_padrao_betas = clsMLE.BetaStdError;
                    //p_valores = clsMLE.BetaPValue;
                    //aic = clsMLE.AIC;
                    //bic = clsMLE.BIC;
                    //log_likelihood = clsMLE.LogLik;
                    //vetor_residuos = clsMLE.Residuos;
                    //res_pond = clsMLE.ResiduosDefasados;
                    //rho = clsMLE.RhoHat;
                    //lim_inf_rho = clsMLE.RhoLimInfCI;
                    //lim_sup_rho = clsMLE.RhoLimSupCI;
                    //rho_pvalue = clsMLE.RhoPValue;
                    //rho_std = clsMLE.RhoStdError;
                    //rho_t = clsMLE.RhoTStat;
                    //sigma2 = clsMLE.Sigma2Hat;

                    label2.Text = "";
                    Application.DoEvents();
                }

                //Criando um vetor com os nomes de todas as variaveis independentes selecionadas
                object[,] todas_var = new object[numero1 + numeroZ, 1];
                object[,] var_x = new object[numero1, 1];
                object[,] var_z = new object[numeroZ, 1];
                for (int i = 0; i < numero1; i++)
                {
                    var_x[i, 0] = strVariaveisSelecionadas[i];

                }
                for (int i = 0; i < numeroZ; i++)
                {
                    var_z[i, 0] = strVariaveisSelecionadasInstrumentos[i];
                }

                todas_var = clsUtil.Concatev(var_x, var_z);





                //Gerando o datatable com betas

                betas.Columns.Add("Variaveis", Type.GetType("System.String"));
                betas.Columns.Add("Beta_Estimado", Type.GetType("System.Double"));
                betas.Columns.Add("Desvio_Padrão", Type.GetType("System.Double"));
                betas.Columns.Add("Estatistica t", Type.GetType("System.Double"));
                betas.Columns.Add("P-valor", Type.GetType("System.Double"));
                betas.Columns.Add("Limite_inferior", Type.GetType("System.Double"));
                betas.Columns.Add("Limite_Superior", Type.GetType("System.Double"));

                DataRow novaLinha = betas.NewRow();
                novaLinha[0] = "Intercepto";

                novaLinha[1] = vetor_betas[0, 0];
                novaLinha[2] = desvios_padrao_betas[0, 0];
                novaLinha[3] = vetor_t[0, 0];
                novaLinha[4] = p_valores[0, 0];
                novaLinha[5] = lim_inf_betas[0, 0];
                novaLinha[6] = lim_sup_betas[0, 0];
                betas.Rows.Add(novaLinha);

                for (int i = 1; i < vetor_betas.Length; i++)
                {
                    novaLinha = betas.NewRow();
                    novaLinha[0] = Convert.ToString(todas_var[i - 1, 0]);

                    novaLinha[1] = vetor_betas[i, 0];
                    novaLinha[2] = desvios_padrao_betas[i, 0];
                    novaLinha[3] = vetor_t[i, 0];
                    novaLinha[4] = p_valores[i, 0];
                    novaLinha[5] = lim_inf_betas[i, 0];
                    novaLinha[6] = lim_sup_betas[i, 0];

                    betas.Rows.Add(novaLinha);

                }

                //Inserindo a linha final, com rho
                DataRow linha_rho = betas.NewRow();
                linha_rho[0] = "Rho";
                linha_rho[1] = rho;
                linha_rho[2] = rho_std;
                linha_rho[3] = rho_t;
                linha_rho[4] = rho_pvalue;
                linha_rho[5] = lim_inf_rho;
                linha_rho[6] = lim_sup_rho;
                betas.Rows.Add(linha_rho);


                dataGridView1.DataSource = betas;
                dataGridView1.Refresh();
                Application.DoEvents();

                //Gerar outro datatable, agora com os outros resultados


                outras.Columns.Add("AIC", Type.GetType("System.Double"));
                outras.Columns.Add("BIC", Type.GetType("System.Double"));
                outras.Columns.Add("Log_Likelihood", Type.GetType("System.Double"));
                outras.Columns.Add("Sigma2", Type.GetType("System.Double"));

                DataRow linha_outras = outras.NewRow();
                linha_outras[0] = aic;
                linha_outras[1] = bic;
                linha_outras[2] = log_likelihood;
                linha_outras[3] = sigma2;
                outras.Rows.Add(linha_outras);

                #region Datagrid de output tentativo

                DataTable out_res = new DataTable();
                out_res.Columns.Add("C0", typeof(object));
                out_res.Columns.Add("C1", typeof(object));
                out_res.Columns.Add("C2", typeof(object));
                out_res.Columns.Add("C3", typeof(object));
                out_res.Columns.Add("C4", typeof(object));
                out_res.Columns.Add("C5", typeof(object));
                out_res.Columns.Add("C6", typeof(object));

                DataRow drr = out_res.NewRow();
                drr[0] = "Modelo "+ comboBox2.SelectedItem.ToString();
                out_res.Rows.Add(drr);

                drr = out_res.NewRow();
                drr[0] = "Data: ";
                drr[1] = DateTime.Now.ToLongDateString();
                out_res.Rows.Add(drr);

                drr = out_res.NewRow();
                drr[0] = " ";
                out_res.Rows.Add(drr);

                drr = out_res.NewRow();
                drr[0] = "AIC: ";
                drr[1] = aic.ToString();
                out_res.Rows.Add(drr);

                drr = out_res.NewRow();
                drr[0] = "BIC: ";
                drr[1] = bic.ToString();
                out_res.Rows.Add(drr);

                drr = out_res.NewRow();
                drr[0] = "Log-likelihood: ";
                drr[1] = log_likelihood.ToString();
                out_res.Rows.Add(drr);

                drr = out_res.NewRow();
                drr[0] = "Sigma2: ";
                drr[1] = sigma2.ToString();
                out_res.Rows.Add(drr);

                drr = out_res.NewRow();
                drr[0] = "Probabilidade de cobertura (%): ";
                drr[1] = "95.0";
                out_res.Rows.Add(drr);

                drr = out_res.NewRow();
                drr[0] = " ";
                out_res.Rows.Add(drr);

                drr = out_res.NewRow();
                drr[0] = "Variaveis";
                drr[1] = "Beta_Estimado";
                drr[2] = "Desvio_Padrão";
                drr[3] = "Estatistica t";
                drr[4] = "P-valor";
                drr[5] = "Limite_inferior";
                drr[6] = "Limite_Superior";
                out_res.Rows.Add(drr);

                drr = out_res.NewRow();
                drr[0] = "Intercepto";
                drr[1] = vetor_betas[0, 0];
                drr[2] = desvios_padrao_betas[0, 0];
                drr[3] = vetor_t[0, 0];
                drr[4] = p_valores[0, 0];
                drr[5] = lim_inf_betas[0, 0];
                drr[6] = lim_sup_betas[0, 0];
                out_res.Rows.Add(drr);

                for (int i = 1; i < vetor_betas.Length; i++)
                {
                    drr = out_res.NewRow();
                    drr[0] = Convert.ToString(todas_var[i - 1, 0]);
                    drr[1] = vetor_betas[i, 0];
                    drr[2] = desvios_padrao_betas[i, 0];
                    drr[3] = vetor_t[i, 0];
                    drr[4] = p_valores[i, 0];
                    drr[5] = lim_inf_betas[i, 0];
                    drr[6] = lim_sup_betas[i, 0];
                    out_res.Rows.Add(drr);
                }


                ////Adicionando para as variaveis publicas, a usar no relatorio//////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Nomes_Var = new string[todas_var.Length];
                t_betas = new double[vetor_t.Length];
                p_betas = new double[p_valores.Length];
                desv = new double[desvios_padrao_betas.Length];
                Valores_betas = new double[vetor_betas.Length];

                for (int i = 0; i < todas_var.Length; i++)
                {
                    Nomes_Var[i] = Convert.ToString(todas_var[i, 0]);
                }
                for (int i = 0; i < vetor_betas.Length; i++)
                {
                    Valores_betas[i] = vetor_betas[i, 0];
                    t_betas[i] = vetor_t[i, 0];
                    p_betas[i] = p_valores[i, 0];
                    desv[i] = desvios_padrao_betas[i, 0];
                }
                prov_aic = aic;
                prov_bic = bic;
                prov_loglik = log_likelihood;
                prov_sigma2 = sigma2;
                prov_rho = rho;
                prov_rho_desv = rho_std;
                prov_rho_t = rho_t;
                prov_rho_p = rho_pvalue;
                var_para_mapa = comboBox3.SelectedItem.ToString();
                isso_eh_regressao = true;
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////


                //Inserindo a linha final, com rho
                drr = out_res.NewRow();
                drr[0] = "Rho";
                drr[1] = rho;
                drr[2] = rho_std;
                drr[3] = rho_t;
                drr[4] = rho_pvalue;
                drr[5] = lim_inf_rho;
                drr[6] = lim_sup_rho;
                out_res.Rows.Add(drr);

                drr = out_res.NewRow();
                drr[0] = " ";
                out_res.Rows.Add(drr);

                drr = out_res.NewRow();
                drr[0] = "LR para rho (estatistica teste): ";
                drr[1] = "";
                out_res.Rows.Add(drr);

                drr = out_res.NewRow();
                drr[0] = "LR para rho (p-valor): ";
                drr[1] = "";
                out_res.Rows.Add(drr);

                this.dataGridView2.DataSource = out_res;

                #endregion


                
                //Gerar datatable com o vetor de residuos

                residuos.Columns.Add("Resíduos", Type.GetType("System.Double"));
                if(res_pond.Length>0) residuos.Columns.Add("Resíduos Defasados", Type.GetType("System.Double"));
                for (int i = 0; i < vetor_residuos.Length; i++)
                {
                    DataRow linha = residuos.NewRow();
                    linha[0] = vetor_residuos[i, 0];
                    if (res_pond.Length > 0) linha[1] = res_pond[i, 0];
                    residuos.Rows.Add(linha);

                }
                dataGridView1.DataSource = residuos;






                //Grafico
                GraphPane myPane = zedGraphControl1.GraphPane;

                // Set the titles
                myPane.Title.Text = "Resíduos versus Resíduos Ponderados Geograficamente";
                myPane.XAxis.Title.Text = "Resíduos";
                myPane.YAxis.Title.Text = "Resíduos Ponderados Geograficamente";


                // Populate a PointPairList with a log-based function and some random variability
                PointPairList list = new PointPairList();
                for (int i = 0; i < res_pond.Length; i++)
                {
                    double x = vetor_residuos[i, 0];
                    double y = res_pond[i, 0];
                    list.Add(x, y);
                }

                // Add the curve
                LineItem myCurve = myPane.AddCurve("Observação", list, Color.Black, SymbolType.Diamond);
                // Don't display the line (This makes a scatter plot)
                myCurve.Line.IsVisible = false;
                // Hide the symbol outline
                myCurve.Symbol.Border.IsVisible = false;
                // Fill the symbol interior with color
                myCurve.Symbol.Fill = new Fill(Color.Firebrick);

                // Fill the background of the chart rect and pane
                myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);
                myPane.Fill = new Fill(Color.White, Color.SlateGray, 45.0f);

                zedGraphControl1.AxisChange();



                this.Cursor = Cursors.Default;
                Application.DoEvents();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRegressao_Click(object sender, EventArgs e)
        {
            try
            {
                this.EstimaRegressao();                             

            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Cursor = Cursors.Default;
                Application.DoEvents();
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
        private void comboBoxDependente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ckbX.Items.Clear();
                ckbX.Items.AddRange(strVariaveis);
                ckbX.Items.RemoveAt(comboBoxDependente.SelectedIndex);

                ckbInstrumentos.Items.Clear();
                ckbInstrumentos.Items.AddRange(strVariaveis);
                ckbInstrumentos.Items.RemoveAt(comboBoxDependente.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBox2.SelectedItem.ToString() == "SAR" ||
                    this.comboBox2.SelectedItem.ToString() == "SEM")
                {
                    this.groupBox5.Enabled = false;
                    this.ckbInstrumentos.Enabled = false;
                }
                else
                {
                    this.groupBox5.Enabled = true;
                    this.ckbInstrumentos.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        
        private void tabPage3_Click(object sender, EventArgs e)
        {            
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = betas;
            
            //Application.DoEvents();
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            //dataGridView1.DataSource = betas;
            //comboBox6.SelectedIndex = 0;
            //Application.DoEvents();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {

                isso_eh_regressao = true;

                //É preciso adicionar as colunas de resiudos à tabela dtable
                if (!dTable.Columns.Contains("Resíduos")) dTable.Columns.Add("Resíduos");
                if (!dTable.Columns.Contains("Resíduos defasados")) dTable.Columns.Add("Resíduos defasados");
                int numero = dTable.Rows.Count;

                for (int i = 0; i < numero; i++)
                {

                    dTable.Rows[i]["Resíduos"] = residuos.Rows[i]["Resíduos"];
                    dTable.Rows[i]["Resíduos defasados"] = residuos.Rows[i]["Resíduos defasados"];

                }



                //Criando o vetor tematico
                clsMapa cmapa = new clsMapa();
                int numclasses = 5;
                iVetor = new int[dTable.Rows.Count];

                limites = new double[numclasses];

                //Outras variaveis a mandar para frmMapa
                var_sel = comboBox3.SelectedItem.ToString();

                if (comboBox1.SelectedItem.ToString() == "Jenks")
                {
                    Metodo_mapa = "Jenks";
                    iVetor = cmapa.criaJenks(dTable, var_sel, strIDmapa, strID, numclasses, ref limites);
                }
                if (comboBox1.SelectedItem.ToString() == "Geométrica")
                {
                    Metodo_mapa = "Geométrico";
                    iVetor = cmapa.criaGeometric(dTable, var_sel, strIDmapa, strID, numclasses, ref limites);
                }
                if (comboBox1.SelectedItem.ToString() == "Desvio Padrão")
                {
                    Metodo_mapa = "Desvio Padrão";
                    int numero_class = 0;
                    iVetor = cmapa.criaDesvios(dTable, var_sel, strIDmapa, strID, numclasses, ref limites, ref numero_class);
                }
                if (comboBox1.SelectedItem.ToString() == "Equal")
                {
                    Metodo_mapa = "Equal";
                    iVetor = cmapa.criaEqual(dTable, var_sel, strIDmapa, strID, numclasses, ref limites);
                }
                if (comboBox1.SelectedItem.ToString() == "Quantil")
                {
                    Metodo_mapa = "Quantil";
                    iVetor = cmapa.criaQuantis(dTable, var_sel, strIDmapa, strID, numclasses, ref limites);
                }

                //Cores para o mapa


                #region Cores


                //Cores
                Brush[] cores = new Brush[numclasses];
                Color[] coresRGB = new Color[numclasses];
                string[] strCoresRGB = new string[numclasses];

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


                    //Valores RGB
                    double R0 = Convert.ToDouble(colors[0].R);
                    double G0 = Convert.ToDouble(colors[0].G);
                    double B0 = Convert.ToDouble(colors[0].B);
                    double R1 = Convert.ToDouble(colors[1].R);
                    double G1 = Convert.ToDouble(colors[1].G);
                    double B1 = Convert.ToDouble(colors[1].B);

                    //Número de classes
                    double nClasses = Convert.ToDouble(numclasses);

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
                    //Cria o objeto cor
                    Color mCor0 = new Color();
                    //Set the color
                    mCor0 = Color.FromArgb(colors[0].ToArgb());
                    strCoresRGB[0] = System.Drawing.ColorTranslator.ToHtml(mCor0);
                    //Cria o objeto cor
                    Color mCor1 = new Color();
                    mCor1 = Color.FromArgb(colors[1].ToArgb());
                    strCoresRGB[cores.Length - 1] = System.Drawing.ColorTranslator.ToHtml(mCor1);
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
                RGB = strCoresRGB;
                #endregion




                this.DialogResult = DialogResult.OK;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        

     

     
    



      

      
    }
}
