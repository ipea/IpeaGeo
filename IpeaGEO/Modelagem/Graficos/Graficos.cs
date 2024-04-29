using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

using IpeaGeo.RegressoesEspaciais;
using MathNet.Numerics.Distributions;

namespace IpeaGeo.Modelagem.Graficos
{
    public partial class Graficos : Form
    {
        BLogicComponentesPrincipais comprinc = new BLogicComponentesPrincipais();

        public Graficos()
        {
            InitializeComponent();
            tabControl1.SelectedIndex = 1;
        }

        clsUtilTools clt = new clsUtilTools();
        
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

        private void AtualizaTabelaDados(bool atualizaControles)
        {
            this.userControlDataGrid1.TabelaDados = m_dt_tabela_dados;

            clsUtilTools clt = new clsUtilTools();
         
            string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);
            string[] todasvariaveis = clt.RetornaTodasColunas(m_dt_tabela_dados);                      
        }
        
        private void btnAbrirTabelaDados_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EstimaModelo()
        {          
            tabControl1.TabPages.Remove(tabPage3);
            GraphPane myPane = zedGraphControl1.GraphPane;

            if (LstX.SelectedIndex < 0 || (LstY.Enabled && LstY.SelectedIndex <0))
            {
                throw new Exception("Escolha a variável a ser plotada.");
            }

            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();

            if (!this.tabControl1.TabPages.Contains(tabPage3))
            {
                tabControl1.TabPages.Add(tabPage3);
            }

            #region Títulos

            if (txtTituloGrafico.Text == "(Insira o título do gráfico)")
            {
                if (LstY.Enabled)
                {
                    myPane.Title.Text = Convert.ToString(LstX.SelectedItem) + " x " + Convert.ToString(LstY.SelectedItem);
                }
                else
                {
                    myPane.Title.Text = "Gráfico de " + Convert.ToString(LstX.SelectedItem);
                }
            }
            else
            {
                myPane.Title.Text = txtTituloGrafico.Text;
            }

            if (txtEixoX.Text == "(Insira o nome do eixo X)")
            {               
                myPane.XAxis.Title.Text = Convert.ToString(LstX.SelectedItem); ;
            }
            else
            {
                myPane.XAxis.Title.Text = txtEixoX.Text;
            }

            if (txtEixoY.Text == "(Insira o nome do eixo Y)")
            {                
                myPane.YAxis.Title.Text = Convert.ToString(LstY.SelectedItem); ;
            }
            else
            {
                myPane.YAxis.Title.Text = txtEixoY.Text;
            }

            #endregion            

            #region Variaveis para gráficos categóricos
            //Variáveis utilizadas apenas para gráficos de variáveis categóricas
            //if (RBcategorica.Checked)
            //{

            object[,] varescolhidaX = clt.GetObjMatrizFromDataTable(m_dt_tabela_dados, Convert.ToString(LstX.SelectedItem));
            object[,] freq_out = new object[0, 0];
        
            clt.FrequencyTable(ref freq_out, varescolhidaX);

            string[] X = new string[freq_out.GetLength(0)];
            double[] Y = new double[freq_out.GetLength(0)];
            //double[] Xhist = new double[freq_out.GetLength(0)];

            for (int i = 0; i < freq_out.GetLength(0); i++)
            {
                X[i] = Convert.ToString(freq_out[i, 0]);
                Y[i] = Convert.ToDouble(freq_out[i, 1]);
                //                    Xhist[i] = Convert.ToDouble(freq_out[i, 0]);
            }

            #region Vetor de cores principais
            Color[] cores = new Color[10];
            cores[0] = Color.Red;
            cores[1] = Color.Gold;
            cores[2] = Color.RoyalBlue;
            cores[3] = Color.Olive;
            cores[4] = Color.Gray;
            cores[5] = Color.Orange;
            cores[6] = Color.LightSteelBlue;
            cores[7] = Color.Wheat;
            cores[8] = Color.Violet;
            cores[9] = Color.Black;

            #endregion

            //}

            #endregion  

            #region Gráfico de barras

            if (RBBarras.Checked)
            {             
                BarItem myBar = myPane.AddBar("Curve 1", null, Y, Color.Red);
                myBar.Bar.Fill = new Fill(Color.Red, Color.White, Color.Red);

                myPane.XAxis.MajorTic.IsBetweenLabels = true;

                // Set the XAxis labels

                myPane.XAxis.Scale.TextLabels = X;
                // Set the XAxis to Text type
                myPane.XAxis.Type = AxisType.Text;

                myPane.XAxis.IsVisible = true;
                myPane.YAxis.IsVisible = true;
                
                // Fill the Axis and Pane backgrounds
            }

            #endregion

            #region Gráfico de Pizza;
            
            if (RBPizza.Checked)
            {                
                for (int i = 0; i < X.GetLength(0); i++)
                {
                    PieItem pieSlice = myPane.AddPieSlice(Y[i], cores[i], 0F, X[i]);
                    //pieSlice.Border.Color = Color.White;                    
                }                               

                // optional depending on whether you want labels within the graph legend
                myPane.Legend.IsVisible = true;
                
                zedGraphControl1.AxisChange();
                zedGraphControl1.Invalidate();
            }

            #endregion

            #region Gráfico de linha e dispersão

            if (RBLinhas.Checked || RBDispers.Checked || RBNormplotpp.Checked || RBHistograma.Checked || RBNormplotqq.Checked)
            {
                string[] nomesvar = new string[2];
                nomesvar[0] = Convert.ToString(LstX.SelectedItem);
                nomesvar[1] = Convert.ToString(LstY.SelectedItem);
                PointPairList list1 = new PointPairList();
                PointPairList reta = new PointPairList();

                if (RBLinhas.Checked || RBDispers.Checked)
                {
                    object[,] pontoscurvaobj = clt.GetObjMatrizFromDataTable(m_dt_tabela_dados, nomesvar);
                    object[,] pontossortedX = new object[0,0];
                    clt.SortByColumn(ref pontossortedX, pontoscurvaobj, 0);
                    double[,] pontoscurva = new double[pontoscurvaobj.GetLength(0), pontoscurvaobj.GetLength(1)];

                    for (int i = 0; i < pontossortedX.GetLength(0); i++)
                    {
                        for (int j = 0; j < pontossortedX.GetLength(1); j++)
                        {
                            pontoscurva[i, j] = (double)pontossortedX[i, j];                        
                        }
                    }

                    for (int i = 0; i < pontoscurva.GetLength(0); i++)
                    {
                        list1.Add(pontoscurva[i, 0], pontoscurva[i, 1]);
                    }

                    if (txtEixoX.Text == "(Insira o nome do eixo X)")
                    {
                        myPane.XAxis.Title.Text = Convert.ToString(LstX.SelectedItem);
                    }
                    else
                    {
                        myPane.XAxis.Title.Text = txtEixoX.Text;
                    }

                    if (txtEixoY.Text == "(Insira o nome do eixo Y)")
                    {
                        myPane.YAxis.Title.Text = Convert.ToString(LstY.SelectedItem);
                    }
                    else
                    {
                        myPane.YAxis.Title.Text = txtEixoY.Text;
                    }

                    myPane.YAxis.Scale.Min = (double)clt.Minc(pontoscurva)[0, 1] - (((double)clt.Maxc(pontoscurva)[0,1] - (double)clt.Minc(pontoscurva)[0,1]) / (double)pontoscurva.GetLength(0));
                    myPane.YAxis.Scale.Max = (double)clt.Maxc(pontoscurva)[0, 1] + (((double)clt.Maxc(pontoscurva)[0, 1] - (double)clt.Minc(pontoscurva)[0, 1]) / (double)pontoscurva.GetLength(0));
                    myPane.XAxis.Scale.Min = (double)clt.Minc(pontoscurva)[0, 0] - (((double)clt.Maxc(pontoscurva)[0, 0] - (double)clt.Minc(pontoscurva)[0, 0]) / (double)pontoscurva.GetLength(0));
                    myPane.XAxis.Scale.Max = (double)clt.Maxc(pontoscurva)[0, 0] + (((double)clt.Maxc(pontoscurva)[0, 0] - (double)clt.Minc(pontoscurva)[0, 0]) / (double)pontoscurva.GetLength(0)); 
                }

                #region PPplot
                
                if (RBNormplotpp.Checked)
                {
                    double[,] variavelpp = clt.GetMatrizFromDataTable(m_dt_tabela_dados, Convert.ToString(LstX.SelectedItem));
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
                        qqnormalv[i,0] = norm.CumulativeDistribution(sdados[i,0]);
                    }

                    for (int i = 0; i < qqdadosv.GetLength(0); i++)
                    {
                        qqnormal[i] = qqnormalv[i, 0];
                        qqdados[i] = qqdadosv[i, 0];
                    }

                    list1.Add(qqdados, qqnormal);

                    for (int i = 0; i < variavelpp.GetLength(0); i++)
                    {
                        if (i > 0)
                        {
                            retaponto[i] = (retaponto[i - 1]) + ((double)clt.Max(qqnormal) / variavelpp.GetLength(0));
                        }
                    }

                    reta.Add(retaponto, retaponto);

                    LineItem myCurve2 = myPane.AddCurve("Normal", reta, Color.Black, SymbolType.None);
                    myCurve2.Line.IsVisible = true;

                    myPane.YAxis.Scale.Min = (double)clt.Min(qqnormalv) - (((double)clt.Max(qqnormalv) - (double)clt.Min(qqnormalv))/(double) qqnormalv.GetLength(0));
                    myPane.YAxis.Scale.Max = (double)clt.Max(qqnormalv) + (((double)clt.Max(qqnormalv) - (double)clt.Min(qqnormalv)) / (double)qqnormalv.GetLength(0));
                    myPane.XAxis.Scale.Min = (double)clt.Min(qqdadosv) - (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));
                    myPane.XAxis.Scale.Max = (double)clt.Max(qqdadosv) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));
                }

                #endregion

                #region QQplt
                
                if (RBNormplotqq.Checked)
                {
                    double[,] variavelpp = clt.GetMatrizFromDataTable(m_dt_tabela_dados, Convert.ToString(LstX.SelectedItem));
                    double mediaX = (clt.Meanc(variavelpp))[0, 0];
                    double varianX = clt.Varianciac(variavelpp)[0, 0];
                    double[,] qqnormalv = new double[variavelpp.GetLength(0),1];
                    double[,] qqdadosv = new double[variavelpp.GetLength(0),1];
                    double[] qqnormal = new double[variavelpp.GetLength(0)];
                    double[] qqdados = new double[variavelpp.GetLength(0)];
                    //double[] retapontox = new double[variavelpp.GetLength(0)];
                    double[] retaponto = new double[variavelpp.GetLength(0)];

                    BLogicNonParametricTests bnp = new BLogicNonParametricTests();
                    Normal norm = new Normal(mediaX, Math.Sqrt(varianX));

                    bnp.QQ_plot_1Variavel(variavelpp,out qqdadosv,out qqnormalv);

                    for (int i = 0; i < qqdadosv.GetLength(0); i++)
                    {
                         qqnormal[i] = qqnormalv[i,0];
                         qqdados[i] = qqdadosv[i, 0];
                    }

                    //retapontox[0] = (double)clt.Min(qqdadosv);
                    retaponto[0] = (double)clt.Min(qqdadosv);

                    list1.Add(qqdados, qqnormal);

                    for (int i = 0; i < variavelpp.GetLength(0); i++)
                    {                        
                        if (i > 0)
                        {
                            //retapontox[i] = (retapontox[i - 1]) + ((double)clt.Max(qqdadosv) / variavelpp.GetLength(0));
                            retaponto[i] = (retaponto[i - 1]) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / variavelpp.GetLength(0));

                            //reta.Add(retapontoy, retapontoy);
                        }
                    }

                    reta.Add(retaponto, retaponto);

                    LineItem myCurve2 = myPane.AddCurve("Normal", reta, Color.Black, SymbolType.None);
                    myCurve2.Line.IsVisible = true;

                    myPane.YAxis.Scale.Min = (double)clt.Min(qqnormalv) - (((double)clt.Max(qqnormalv) - (double)clt.Min(qqnormalv)) / (double)qqnormalv.GetLength(0));
                    myPane.YAxis.Scale.Max = (double)clt.Max(qqnormalv) + (((double)clt.Max(qqnormalv) - (double)clt.Min(qqnormalv)) / (double)qqnormalv.GetLength(0));
                    myPane.XAxis.Scale.Min = (double)clt.Min(qqdadosv) - (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));
                    myPane.XAxis.Scale.Max = (double)clt.Max(qqdadosv) + (((double)clt.Max(qqdadosv) - (double)clt.Min(qqdadosv)) / (double)qqdadosv.GetLength(0));
                }

                #endregion
                
                #region Histograma
                
                if (RBHistograma.Checked)
                {
                    double[,] variavelpp = clt.GetMatrizFromDataTable(m_dt_tabela_dados, Convert.ToString(LstX.SelectedItem));                    
 
                    //Calcula o número de classes segundo Regra de Sturges numclass = 1 + 3.3 log(n)

                    int numclassesSturges = new int();
                    double amplitudehist = new double();
                    amplitudehist = ((clt.Maxc(variavelpp)[0, 0]) - (clt.Minc(variavelpp)[0, 0]));
                    double amplitude_classe = new double();
                    if (rbAmplitudeManual.Checked && Convert.ToDouble(numericUpDownAmplitudeManual.Value) > 0)
                    {
                        amplitude_classe = Convert.ToDouble(numericUpDownAmplitudeManual.Value);
                        numclassesSturges = (int)((clt.Max(variavelpp) - clt.Min(variavelpp)) / (amplitude_classe));
                    }
                    else 
                    {
                        if (rbNumClasses.Checked && Convert.ToDouble(numericUpDownNumClasses.Value) > 0)
                        {
                            numclassesSturges = Convert.ToInt32(numericUpDownNumClasses.Value);                            
                        }
                        else
                        {
                            if(rbNotimoclasses.Checked)
                            {
                                
                                numclassesSturges = (int)(1.0 + 3.3 * Math.Log(variavelpp.GetLength(0)));
                            }
                            else
                            {
                               MessageBox.Show("Selecione uma opção para o Histograma.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);                              
                            }
                        }
                    }

                    if (numclassesSturges > 100)
                    {
                        throw new Exception("Número de classes muito grande. Ajuste novamente a amplitude de seu histograma.");
                    }
                    
                    double incrementohist = (double)(amplitudehist / (double)numclassesSturges);
                    double[] Yhist = new double[numclassesSturges];
                    double[] Xhist = new double[numclassesSturges + 1];
                    
                    for (int i = 0; i < Xhist.GetLength(0); i++)
                    { 
                        Xhist[i] = ((clt.Minc(variavelpp)[0, 0]) + (incrementohist * ((double)i)));               
                    }

                    double temp = new double();
                    for (int i = 0; i < variavelpp.GetLength(0); i++)
                    {
                        for (int j = 0; j < numclassesSturges; j++)
                        {
                            temp = ((clt.Minc(variavelpp)[0, 0]) + (incrementohist * (((double)j)+1.0)));
                            if (variavelpp[i,0] <= temp)
                            {
                                Yhist[j] += 1;
                                break;
                            }
                        }
                    }

                    /*cria frequencia relativa*/
                    double n = variavelpp.GetLength(0);
                    if (cbFrequencia.SelectedItem.ToString() == "Frequência Relativa")
                    {
                        for (int i = 0; i < numclassesSturges; i++)
                        {
                            Yhist[i] = Yhist[i] / n;                       
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
                }

                #endregion

                LineItem myCurve1 = myPane.AddCurve("tituloo", list1, Color.Blue, SymbolType.Circle);
                myCurve1.Symbol.Fill.Color = Color.Blue;
                myCurve1.Label.IsVisible = false;

                //Gráfico de dispersão. Tira a linha que liga os pontos
                if (RBDispers.Checked)
                {
                    myCurve1.Line.IsVisible = false;                    
                }
            }
            
            #endregion

            //myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 90F);
            myPane.Chart.Fill = new Fill(Color.White, Color.White, 90F);
            myPane.Fill = new Fill(Color.White);
                                 
            zedGraphControl1.AxisChange();
                         
            zedGraphControl1.Update();
            zedGraphControl1.Refresh();           

            tabControl1.SelectedTab = tabPage3;        
        }              
        
        private void FormComponentesPrincipais_Load(object sender, EventArgs e)
        {
            // Variáveis sendo passadas para o UserControl
            userControlDataGrid1.TabelaDados = this.m_dt_tabela_dados;
            userControlDataGrid1.FuncaoFromFormulario = new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionFromFormulario(this.AtualizaTabelaDados);
            userControlDataGrid1.MostraOpcaoImportadaoDados = true;
            
            if (RBnumerica.Checked)
            {
                RBLinhas.Enabled = true;
                RBHistograma.Enabled = true;
                RBDispers.Enabled = true;
                RBNormplotpp.Enabled = true;
                RBNormplotqq.Enabled = true;
                string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);
                string[] todasvariaveis = clt.RetornaTodasColunas(m_dt_tabela_dados);
                LstX.Items.Clear();
                //LstX.SelectedIndex = 0;
                LstX.Items.AddRange(variaveis_numericas);
                LstY.Items.Clear();
                LstY.Enabled = true;
                RBBarras.Enabled = false;
                RBPizza.Enabled = false;
                RBBarras.Checked = false;
                RBPizza.Checked = false;
                //LstY.SelectedIndex = 0;
                LstY.Items.AddRange(variaveis_numericas);

                //Preenche o campo de título e eixos
                txtTituloGrafico.Text = "(Insira o título do gráfico)";
                txtEixoX.Text = "(Insira o nome do eixo X)";
                txtEixoY.Text = "(Insira o nome do eixo Y)";
            }

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
        }

        private void RBnumerica_CheckedChanged(object sender, EventArgs e)
        {
            if (RBnumerica.Checked)
            {
                RBLinhas.Enabled = true;
                RBHistograma.Enabled = true;
                RBDispers.Enabled = true;
                RBNormplotpp.Enabled = true;
                RBNormplotqq.Enabled = true;
                string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);
                string[] todasvariaveis = clt.RetornaTodasColunas(m_dt_tabela_dados);
                LstX.Items.Clear();
                //LstX.SelectedIndex = 0;
                LstX.Items.AddRange(variaveis_numericas);
                LstY.Items.Clear();
                LstY.Enabled = true;
                RBBarras.Enabled = false;
                RBPizza.Enabled = false;
                RBBarras.Checked = false;
                RBPizza.Checked = false;  
                //LstY.SelectedIndex = 0;
                LstY.Items.AddRange(variaveis_numericas);
                RBLinhas.Checked = true;                
            }
        }

        private void RBcategorica_CheckedChanged(object sender, EventArgs e)
        {
            RBBarras.Enabled = true;
            RBPizza.Enabled = true;
            string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);
            string[] todasvariaveis = clt.RetornaTodasColunas(m_dt_tabela_dados);
            LstX.Items.Clear();
            //LstX.SelectedIndex = 0;
            LstX.Items.AddRange(todasvariaveis);
            LstY.Items.Clear();
            //LstY.SelectedIndex = 0;            
            RBLinhas.Enabled = false;
            RBHistograma.Enabled = false;
            RBDispers.Enabled = false;
            RBNormplotpp.Enabled = false;
            RBNormplotqq.Enabled = false;
            RBLinhas.Checked = false;
            RBHistograma.Checked = false;
            RBDispers.Checked = false;
            RBNormplotpp.Checked = false;
            RBNormplotqq.Checked = false;
            LstY.Enabled = false;
            RBBarras.Checked = true;
        }

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                this.EstimaModelo();

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Gráficos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RBLinhas_CheckedChanged(object sender, EventArgs e)
        {
            LstY.Enabled = true;
        }

        private void RBDispers_CheckedChanged(object sender, EventArgs e)
        {
            LstY.Enabled = true;
        }
        
        private void RBNormplot_CheckedChanged(object sender, EventArgs e)
        {
            LstY.Enabled = false;
        }

        private void RBHistograma_CheckedChanged(object sender, EventArgs e)
        {
            LstY.Enabled = false;
            if (RBHistograma.Checked)
            {
                rbAmplitudeManual.Enabled = true;
                rbNumClasses.Enabled = true;
                rbNotimoclasses.Enabled = true;
                rbNotimoclasses.Checked = true;
                cbFrequencia.Enabled = true;
                cbFrequencia.SelectedItem = "Frequência Absoluta";
            }
            else
            {
                rbAmplitudeManual.Enabled = false;
                rbNotimoclasses.Enabled = false;
                rbNumClasses.Enabled = false;
                rbNumClasses.Checked = false;
                rbAmplitudeManual.Checked = false;
                rbNotimoclasses.Checked = false;
                cbFrequencia.Enabled = false;              
            }
        }

        private void RBNormplotqq_CheckedChanged(object sender, EventArgs e)
        {
            LstY.Enabled = false;
        }

        private void btnDadosGrafico_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.tabControl1.TabPages.Contains(tabPage4))
                {
                    this.tabControl1.TabPages.Add(tabPage4);
                }
                this.tabControl1.SelectedTab = tabPage4;

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtTituloGrafico_TextChanged(object sender, EventArgs e)
        {           
        }

        private void rdbAmplitudeManual_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAmplitudeManual.Checked)
            {
                numericUpDownAmplitudeManual.Enabled = true;
            }
            else
            {
                numericUpDownAmplitudeManual.Enabled = false;
            }
        }

        private void rbNumClasses_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNumClasses.Checked)
            {
                numericUpDownNumClasses.Enabled = true;
            }
            else
            {
                numericUpDownNumClasses.Enabled = false;
            }
        }

        private void rbNotimoclasses_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNotimoclasses.Checked)
            {
                numericUpDownNumClasses.Value = 0;
                numericUpDownAmplitudeManual.Value = 0;
            }
        }
        
        int a = 0;
        int b = 0;
        int c = 0;
        
        private void txtTituloGrafico_Click(object sender, EventArgs e)
        {
            if (a == 0)
            {
                txtTituloGrafico.Text = "";
                a = 1;
            }
            txtTituloGrafico.ForeColor = Color.Black;
        }

        private void txtEixoX_Click(object sender, EventArgs e)
        {
            if (b == 0)
            {
                txtEixoX.Text = "";
                b = 1;
            }
            txtEixoX.ForeColor = Color.Black;
        }

        private void txtEixoY_Click(object sender, EventArgs e)
        {
            if (c == 0)
            {
                txtEixoY.Text = "";
                c = 1;
            }
            txtEixoY.ForeColor = Color.Black;
        }

        private void txtTituloGrafico_Leave(object sender, EventArgs e)
        {
            int t = txtTituloGrafico.Text.Length;
            for (int i = 0; i < t; i++)
            {
                if (txtTituloGrafico.Text.IndexOf(" ") == 0)
                {
                    txtTituloGrafico.Text = txtTituloGrafico.Text.Remove(0, 1);
                }
            }
            
            if (txtTituloGrafico.Text == "")
            {
                txtTituloGrafico.ForeColor = Color.Silver;
                txtTituloGrafico.Text = "(Insira o título do gráfico)";
                a = 0;
            }            
        }

        private void txtEixoX_Leave(object sender, EventArgs e)
        {
            int t = txtEixoX.Text.Length;
            for (int i = 0; i < t; i++)
            {
                if (txtEixoX.Text.IndexOf(" ") == 0)
                {
                    txtEixoX.Text = txtEixoX.Text.Remove(0, 1);
                }
            }
            if (txtEixoX.Text == "")
            {
                txtEixoX.ForeColor = Color.Silver;
                txtEixoX.Text = "(Insira o título do gráfico)";
                b = 0;
            }
        }

        private void txtEixoY_Leave(object sender, EventArgs e)
        {
            int t = txtEixoY.Text.Length;
            for (int i = 0; i < t; i++)
            {
                if (txtEixoY.Text.IndexOf(" ") == 0)
                {
                    txtEixoY.Text = txtEixoY.Text.Remove(0, 1);
                }
            }
            if (txtEixoY.Text == "")
            {
                txtEixoY.ForeColor = Color.Silver;
                txtEixoY.Text = "(Insira o título do gráfico)";
                c = 0;
            }
        }
    }
}
