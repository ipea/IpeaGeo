using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IpeaGEO
{
    public partial class frmDependenciaGlobal : Form
    {
        public frmDependenciaGlobal()
        {
            InitializeComponent();
        }

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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmDependenciaGlobal_Load(object sender, EventArgs e)
        {
            //Adiciona as variáveis
            checkedListBox1.Items.AddRange(strVariaveisQuantitativas);
            cmbVizinhanca.SelectedIndex = 0;
            cmbPop.Items.AddRange(strVariaveisQuantitativas);
            cmbPop.SelectedIndex = 0;
        }

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
                if (chkRogerson.Checked || chkTango.Checked)
                {

                    for (int i = 0; i < checkedListBox1.Items.Count; i++) if (checkedListBox1.GetItemChecked(i) == true) cSelected1++;
                    string[] strSelecionadasQuanti = new string[cSelected1 + 1];
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

                if (cSelected1 == 0)
                {
                    MessageBox.Show("Pelo menos uma variável deve ser selecionada.", "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    #region Encontra a matriz de vizinhança
                    if (shapeAlex.TipoVizinhanca == "")
                    {
                        //Modifica o label
                        labelProgress.Text = "Gerando a matriz de vizinhanças...";
                        Application.DoEvents();

                        //Cria a vizinhnaça
                        clsIpeaShape cps = new clsIpeaShape();
                        int tipo_vizinhanca = -1;
                        if (cmbVizinhanca.SelectedItem.ToString() == "Queen" && chkNormal.Checked == false)
                        {
                            tipo_vizinhanca = 1;
                            shapeAlex.TipoVizinhanca = "Queen";
                        }
                        else if (cmbVizinhanca.SelectedItem.ToString() == "Rook" && chkNormal.Checked == false)
                        {
                            tipo_vizinhanca = 2;
                            shapeAlex.TipoVizinhanca = "Rook";
                        }
                        else if (cmbVizinhanca.SelectedItem.ToString() == "Queen" && chkNormal.Checked == true)
                        {
                            tipo_vizinhanca = 3;
                            shapeAlex.TipoVizinhanca = "Queen Normalizada";
                        }
                        else if (cmbVizinhanca.SelectedItem.ToString() == "Rook" && chkNormal.Checked == true)
                        {
                            tipo_vizinhanca = 4;
                            shapeAlex.TipoVizinhanca = "Rook Normalizada";
                        }

                        cps.DefinicaoVizinhos(ref shapeAlex, tipo_vizinhanca, ref progressBar1);
                    }

                    #endregion

                    //Habilita o label
                    labelProgress.Text = "Inicializando procedimento...";
                    Application.DoEvents();

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

                        double[] dblPvalorMoran = new double[mDadosQuanti.GetLength(1)];
                        double[] dblMoran = new double[mDadosQuanti.GetLength(1)];
                        if (chkMoran.Checked)
                        {

                            //Indice de Moran
                            labelProgress.Text = "Calculando indice de moran geral...";
                            progressBar1.Value = 0;
                            Application.DoEvents();

                            dblMoran = clsIndicesGlobais.IndiceDeMoranGlobal(dTable, strIDmapa, mDadosQuanti, shapeAlex);

                            //Indice de Moran
                            labelProgress.Text = "Simulações de Monte Carlo para o indice de moran geral...";
                            Application.DoEvents();

                            dblPvalorMoran = clsIndicesGlobais.pValorIndiceDeMoranGlobal(dTable, strIDmapa, mDadosQuanti, shapeAlex, (int)numSimula.Value, dblMoran, ref progressBar1);
                        }

                        double[] dblMoranSimples = new double[mDadosQuanti.GetLength(1)];
                        double[] dblPvalorMoranSimples = new double[mDadosQuanti.GetLength(1)];
                        if (chkMoranSimples.Checked)
                        {
                            //Indice de Moran simples
                            labelProgress.Text = "Calculando indice de Moran simples...";
                            progressBar1.Value = 0;
                            Application.DoEvents();

                            dblMoranSimples = clsIndicesGlobais.IndiceDeMoranGlobalSimples(dTable, strIDmapa, mDadosQuanti, shapeAlex);

                            labelProgress.Text = "Simulações de Monte Carlo para o indice de Moran simples...";
                            Application.DoEvents();

                            dblPvalorMoranSimples = clsIndicesGlobais.pValorIndiceDeMoranGlobalSimples(dTable, strIDmapa, mDadosQuanti, shapeAlex, (int)numSimula.Value, dblMoranSimples, ref progressBar1);
                        }

                        double[] dblGeary = new double[mDadosQuanti.GetLength(1)];
                        double[] dblPvalorGeary = new double[mDadosQuanti.GetLength(1)];
                        if (chkGeary.Checked)
                        {
                            //Indice de Geary
                            labelProgress.Text = "Calculando indice de Geary...";
                            progressBar1.Value = 0;
                            Application.DoEvents();

                            dblGeary = clsIndicesGlobais.IndiceDeGeary(dTable, strIDmapa, mDadosQuanti, shapeAlex);

                            labelProgress.Text = "Simulações de Monte Carlo para o indice de Geary...";
                            Application.DoEvents();

                            dblPvalorGeary = clsIndicesGlobais.pValorIndiceDeGeary(dTable, strIDmapa, mDadosQuanti, shapeAlex, (int)numSimula.Value, dblGeary, ref progressBar1);
                        }

                        double[] dblGetis = new double[mDadosQuanti.GetLength(1)];
                        double[] dblPvalorGetis = new double[mDadosQuanti.GetLength(1)];
                        if (chkGetis.Checked)
                        {
                            //Indice de Getis
                            labelProgress.Text = "Calculando indice de Getis-Ord Gi*...";
                            progressBar1.Value = 0;
                            Application.DoEvents();

                            dblGetis = clsIndicesGlobais.IndiceGetisOrdGiGlobal(dTable, strIDmapa, mDadosQuanti, shapeAlex);

                            labelProgress.Text = "Simulações de Monte Carlo para o indice de Getis-Ord Gi*...";
                            Application.DoEvents();

                            dblPvalorGetis = clsIndicesGlobais.pValorGetisOrdGiGlobal(dTable, strIDmapa, mDadosQuanti, shapeAlex, (int)numSimula.Value, dblGeary, ref progressBar1);
                        }

                    #endregion

                        #region Indicadores Qualitativos

                        double[] dblTango = new double[mDadosQuanti.GetLength(1) - 1];
                        double[] dblPvalorTango = new double[mDadosQuanti.GetLength(1) - 1];
                        if (chkTango.Checked)
                        {
                            //Indice de Tango
                            labelProgress.Text = "Calculando indice de Tango...";
                            progressBar1.Value = 0;
                            Application.DoEvents();

                            dblTango = clsIndicesGlobais.IndiceDeTango(dTable, strIDmapa, mDadosQuanti, shapeAlex);

                            labelProgress.Text = "Simulações de Monte Carlo para o indice de Tango...";
                            Application.DoEvents();

                            dblPvalorTango = clsIndicesGlobais.pValorIndiceDeTango(dTable, strIDmapa, mDadosQuanti, shapeAlex, (int)numSimula.Value, dblGeary, ref progressBar1);
                        }

                        double[] dblRogerson = new double[mDadosQuanti.GetLength(1) - 1];
                        double[] dblPvalorRogerson = new double[mDadosQuanti.GetLength(1) - 1];
                        if (chkRogerson.Checked)
                        {
                            //Indice de Rogerson
                            labelProgress.Text = "Calculando indice de Rogerson...";
                            progressBar1.Value = 0;
                            Application.DoEvents();

                            dblRogerson = clsIndicesGlobais.IndiceDeRogerson(dTable, strIDmapa, mDadosQuanti, shapeAlex);

                            labelProgress.Text = "Simulações de Monte Carlo para o indice de Rogerson...";
                            Application.DoEvents();

                            dblPvalorRogerson = clsIndicesGlobais.pValorIndiceDeRogerson(dTable, strIDmapa, mDadosQuanti, shapeAlex, (int)numSimula.Value, dblGeary, ref progressBar1);
                        }


                        #endregion

                        #region Passando informações para o relatório

                        strPOPULACAO = cmbPop.SelectedItem.ToString();
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
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Cursor = Cursors.Default;
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
