using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IpeaGeo.RegressoesEspaciais
{
    public partial class FormCalculadora : Form
    {
        public FormCalculadora()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private bool m_precisa_matrizW_predefinida = false;
        public bool PrecisaMatrizWPredefinida
        {
            get { return m_precisa_matrizW_predefinida; }
            set { m_precisa_matrizW_predefinida = value; }
        }

        private clsMatrizEsparsa m_Wpredefinida;
        public clsMatrizEsparsa MatrizWPredefinida
        {
            get { return m_Wpredefinida; }
            set { m_Wpredefinida = value; }
        }

        private clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            set { this.m_shape = value; }
            get
            {
                return this.m_shape;
            }
        }

        private bool m_usa_W_sparsa_from_distancias = false;
        public bool UsaMatrizEsparsaFromDistancias
        {
            get
            {
                return m_usa_W_sparsa_from_distancias;
            }
            set
            {
            	this.m_usa_W_sparsa_from_distancias = value;
            }
        }

        private clsMatrizEsparsa m_W_esparsa_from_distancias = new clsMatrizEsparsa();
        public clsMatrizEsparsa MatrizEsparsaFromDistancias
        {
            get
            {
                return m_W_esparsa_from_distancias;
            }
            set
            {
                m_W_esparsa_from_distancias = value;
            }
        }

        private DataTable m_dt_dados = new DataTable();
        public DataTable Dados
        {
            get { return m_dt_dados.Copy(); }
            set { this.m_dt_dados = value.Copy(); }
        }

        private bool m_dados_concatenados = false;
        public bool DadosConcatenados
        {
            set 
            { 
                m_dados_concatenados = value; 
            }
        }

        private bool m_dados_carregados = false;

        private bool m_ativa_exclusao_variaveis = false;
        public bool AtivaExclusaoVariaveis
        {
            set { m_ativa_exclusao_variaveis = value; }
        }

        private bool m_ativa_medidas_poligonos = false;
        public bool AtivaMedidasPoligonos
        {
            set { m_ativa_medidas_poligonos = value; }
        }

        private void FormCalculadora_Load(object sender, EventArgs e)
        {
            cmbVariavelMatrizDistancias.Enabled = false;

            if (this.m_dados_concatenados)
            {
                if (!this.tabControl1.TabPages.Contains(tabPage4))
                    this.tabControl1.TabPages.Add(tabPage4);
            }
            else
            {
                if (this.tabControl1.TabPages.Contains(tabPage4))
                    this.tabControl1.TabPages.Remove(tabPage4);
            }

            if (this.tabControl1.TabPages.Contains(tabPage5))
                this.tabControl1.TabPages.Remove(tabPage5);

            if (this.tabControl1.TabPages.Contains(tabPage6))
                this.tabControl1.TabPages.Remove(tabPage6);

            clsUtilTools clt = new clsUtilTools();

            string[] vars = clt.RetornaColunasNumericas(m_dt_dados);

            listBox1.Items.Clear();
            listBox1.Items.AddRange(vars);
            listBox1.SelectedIndex = 0;

            listBox4.Items.Clear();
            listBox4.Items.AddRange(vars);
            listBox4.SelectedIndex = 0;

            listBox5.Items.Clear();
            listBox5.Items.AddRange(vars);
            listBox5.SelectedIndex = 0;

            string[] all_vars = clt.RetornaTodasColunas(m_dt_dados);

            listBox7.Items.Clear();
            listBox7.Items.AddRange(all_vars);
            listBox7.SelectedIndex = 0;

            string[] funcoes = new string[] { "Logaritmo", "Logaritmo10", "Exponencial", "Raiz quadrada", "Somatório", "Quadrado", "Cubo", "Valor absoluto",
                                "Seno", "Cosseno", "Tangente", "Arccosseno", "Arcseno",
                                "Arctangente", "Cosseno hiperbólico", "Seno hiperbólico", "Tangente hiperbólica", 
                                "Lag espacial", "Transformação Box-Cox"};

            listBox2.Items.Clear();
            listBox2.Items.AddRange(funcoes);
            listBox2.SelectedIndex = 0;

            m_dados_carregados = true;

            if (m_ativa_exclusao_variaveis)
            {
                this.tabControl1.SelectedTab = tabPage3;
            }

            if (m_ativa_medidas_poligonos)
            {
                this.tabControl1.SelectedTab = tabPage4;
            }

            string[] funcoes_bivariadas = new string[] { "Soma", "Subtração", "Divisão", "Multiplicação", "Potência", "Máximo", "Mínimo" };

            listBox3.Items.Clear();
            listBox3.Items.AddRange(funcoes_bivariadas);
            listBox3.SelectedIndex = 0;
        }

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            try
            {
                string nome_var = this.textBox1.Text.Trim();
                this.textBox1.Text = nome_var;

                string variavel = this.listBox1.SelectedItem.ToString();
                string funcao = this.listBox2.SelectedItem.ToString();

                BLExecutarCalculadora ble = new BLExecutarCalculadora();

                if (funcao == "Lag espacial")
                {
                    if (!m_precisa_matrizW_predefinida)
                    {
                        if (m_dados_concatenados)
                        {
                            ble.GeraMatrizVizinhanca(this.m_shape);
                        }
                        else
                        {
                            if (this.m_usa_W_sparsa_from_distancias)
                            {
                                ble.UsaMatrizEsparsaFromDistancias = true;
                                ble.MatrizEsparsaFromDistancias = m_W_esparsa_from_distancias;
                            }
                            else
                            {
                                throw new Exception("Matriz de vizinhança não está definida. Não é possível calcular o lag espacial.");
                            }
                        }
                    }
                    else
                    {
                        if (m_Wpredefinida != null)
                        {
                            ble.UsaMatrizEsparsaFromDistancias = true;
                            ble.MatrizEsparsaFromDistancias = m_Wpredefinida;
                        }
                        else
                        {
                            throw new Exception("Matriz de vizinhança não está definida. Não é possível calcular o lag espacial.");
                        }
                    }
                }

                int[] filtro = this.DefinicaoObservacoesFiltradas();

                ble.UsaLambdaOtimoBoxCox = !this.checkBox8.Checked;
                ble.ParametroFuncaoUnivariada = Convert.ToDouble(this.nudParametroFuncaoUnivariada.Value);

                if (!this.checkBox1.Checked)
                {
                    ble.ExecutarFuncaoUnivariada(this.m_dt_dados, funcao, variavel, nome_var, filtro);
                }
                else
                {
                    ble.ExecutarFuncaoUnivariada(this.m_dt_dados, Convert.ToDouble(this.numericUpDown1.Value), nome_var, filtro);
                }

                if (!this.checkBox8.Checked)
                {
                    this.nudParametroFuncaoUnivariada.Enabled = true;
                    this.nudParametroFuncaoUnivariada.Value = Convert.ToDecimal(ble.ParametroFuncaoUnivariada);
                    this.nudParametroFuncaoUnivariada.Enabled = false;
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void NomeAutomaticoVariavel()
        {
            string variavel = this.listBox1.SelectedItem.ToString();
            string funcao = this.listBox2.SelectedItem.ToString();

            string prefix = "L";

            nudParametroFuncaoUnivariada.Enabled = false;
            checkBox8.Enabled = false;

            switch (funcao)
            {
                case "Logaritmo":
                    prefix = "L";
                    break;
                case "Logaritmo10":
                    prefix = "L10";
                    break;
                case "Exponencial":
                    prefix = "Exp";
                    break;
                case "Raiz quadrada":
                    prefix = "Root";
                    break;
                case "Somatório":
                    prefix = "Sum(∑)";
                    break;
                case "Quadrado":
                    prefix = "Q2";
                    break;
                case "Cubo":
                    prefix = "Q3";
                    break;
                case "Valor absoluto":
                    prefix = "Abs";
                    break;
                case "Seno":
                    prefix = "Sin";
                    break;
                case "Cosseno":
                    prefix = "Cos";
                    break;
                case "Tangente":
                    prefix = "Tan";
                    break;
                case "Arccosseno":
                    prefix = "Acos";
                    break;
                case "Arcseno":
                    prefix = "Asen";
                    break;
                case "Arctangente":
                    prefix = "Atan";
                    break;
                case "Cosseno hiperbólico":
                    prefix = "Cosh";
                    break;
                case "Seno hiperbólico":
                    prefix = "Senh";
                    break;
                case "Tangente hiperbólica":
                    prefix = "Tanh";
                    break;
                case "Lag espacial":
                    prefix = "W";
                    break;
                case "Transformação Box-Cox":
                    {
                        prefix = "BC";
                        nudParametroFuncaoUnivariada.Enabled = true;
                        checkBox8.Enabled = true;
                    }
                    break;
                default:
                    break;
            }

            this.textBox1.Text = prefix + "_" + variavel;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_dados_carregados)
                {
                    this.checkBox8.Checked = true;
                    this.nudParametroFuncaoUnivariada.Enabled = true;
                    this.nudParametroFuncaoUnivariada.Value = Convert.ToDecimal(0.0);
                    this.lblCorrOtimaBoxCox.Text = "";

                    NomeAutomaticoVariavel();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                NomeAutomaticoVariavel();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private int[] DefinicaoObservacoesFiltradas()
        {
            int[] res = new int[m_dt_dados.Rows.Count];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                res[i] = 1;
            }

            if (ckbAplicarFiltro.Checked)
            {
                object valor1 = new object();
                object valor2 = new object();

                string tipo_variavel = "numero";
                if (dateValor1Comparacao.Visible) tipo_variavel = "data";
                if (txtValor1Comparacao.Visible) tipo_variavel = "texto";

                string relacao = this.listBox8.SelectedItem.ToString();
                string variavel = this.listBox6.SelectedItem.ToString();

                switch (tipo_variavel)
                {
                	case "numero":
                		valor1 = Convert.ToDouble(nudValor1Comparacao.Value);
                        valor2 = Convert.ToDouble(nudValor2Comparacao.Value);
                		break;
                    case "data":
                        valor1 = dateValor1Comparacao.Value;
                        valor2 = dateValor2Comparacao.Value;
                        break;
                    case "texto":
                        valor1 = txtValor1Comparacao.Text;
                        valor2 = txtValor2Comparacao.Text;
                        break;
                    default: 
                        break;
                }

                BLExecutarCalculadora blc = new BLExecutarCalculadora();
                res = blc.DefinicaoObservacoesFiltradas(m_dt_dados, variavel, tipo_variavel, relacao, valor1, valor2);
            }

            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string nome_var = this.textBox2.Text.Trim();
                this.textBox2.Text = nome_var;

                string variavel1;
                string variavel2;
                string funcao = this.listBox3.SelectedItem.ToString();

                BLExecutarCalculadora ble = new BLExecutarCalculadora();

                int[] filtro = this.DefinicaoObservacoesFiltradas();

                if (!checkBox2.Checked && !checkBox3.Checked)
                {
                    variavel1 = this.listBox4.SelectedItem.ToString();
                    variavel2 = this.listBox5.SelectedItem.ToString();
                    ble.ExecutarFuncaoBivariada(this.m_dt_dados, funcao, variavel1, variavel2, nome_var, filtro);
                }
                else
                {
                    if (!checkBox2.Checked && checkBox3.Checked)
                    {
                        variavel1 = this.listBox4.SelectedItem.ToString();
                        ble.ExecutarFuncaoBivariada(this.m_dt_dados, funcao, variavel1, 
                            Convert.ToDouble(this.numericUpDown3.Value), nome_var, filtro);
                    }

                    if (checkBox2.Checked && !checkBox3.Checked)
                    {
                        variavel2 = this.listBox5.SelectedItem.ToString();
                        ble.ExecutarFuncaoBivariada(this.m_dt_dados, funcao, 
                            Convert.ToDouble(this.numericUpDown2.Value), variavel2, nome_var, filtro);
                    }

                    if (checkBox2.Checked && checkBox3.Checked)
                    {
                        ble.ExecutarFuncaoBivariada(this.m_dt_dados, funcao, Convert.ToDouble(this.numericUpDown2.Value),
                            Convert.ToDouble(this.numericUpDown3.Value), nome_var, filtro);
                    }
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int n = this.listBox7.SelectedItems.Count;
                string[] variaveis = new string[n];
                for (int i = 0; i < n; i++) variaveis[i] = this.listBox7.SelectedItems[i].ToString();

                if (n >= m_dt_dados.Columns.Count) throw new Exception("Não é possível excluir todas as colunas da tabela de dados.");

                DialogResult disp = MessageBox.Show("Tem certeza que deseja excluir as variáveis selecionadas?",
                     "Exclusão de variáveis", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (disp == DialogResult.Yes)
                {
                    BLExecutarCalculadora ble = new BLExecutarCalculadora();

                    ble.ExcluiVariaveis(m_dt_dados, variaveis);

                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                this.numericUpDown1.Enabled = true;
                this.listBox1.Enabled = this.listBox2.Enabled = false;
                this.textBox1.Text = "Coluna1";
            }
            else
            {
                this.numericUpDown1.Enabled = false;
                this.listBox1.Enabled = this.listBox2.Enabled = true;
                NomeAutomaticoVariavel();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                this.numericUpDown2.Enabled = true;
                this.listBox4.Enabled = false;
            }
            else
            {
                this.numericUpDown2.Enabled = false;
                this.listBox4.Enabled = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                this.numericUpDown3.Enabled = true;
                this.listBox5.Enabled = false;
            }
            else
            {
                this.numericUpDown3.Enabled = false;
                this.listBox5.Enabled = true;
            }
        }

        private bool m_janela_if_ativada = false;
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.tabControl1.TabPages.Contains(tabPage5))
                {
                    this.tabControl1.TabPages.Add(tabPage5);
                }
                this.tabControl1.SelectedTab = tabPage5;

                clsUtilTools clt = new clsUtilTools();

                string[] all_vars = clt.RetornaTodasColunas(m_dt_dados);

                listBox6.Items.Clear();
                listBox6.Items.AddRange(all_vars);
                listBox6.SelectedIndex = 0;

                this.ckbAplicarFiltro.Checked = true;

                string[] relacionamentos = new string[] { "Igual (=)", "Menor (<)", "Menor ou igual (<=)", "Maior (>)", 
                                                            "Maior or igual (>=)", "Entre (inclusive)", "Entre (exclusive)"};

                listBox8.Items.Clear();
                listBox8.Items.AddRange(relacionamentos);
                listBox8.SelectedIndex = 0;

                m_janela_if_ativada = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                grbDistanciaToPoligono.Enabled = true;

                clsUtilTools clt = new clsUtilTools();

                string[] variaveis = clt.RetornaTodasColunas(m_dt_dados);
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(variaveis);
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                grbDistanciaToPoligono.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string variavel = comboBox1.SelectedItem.ToString();

                string[] valores = new string[m_dt_dados.Rows.Count];
                for (int i = 0; i < valores.GetLength(0); i++)
                {
                    valores[i] = m_dt_dados.Rows[i][variavel].ToString();
                }

                comboBox2.Items.Clear();
                comboBox2.Items.AddRange(valores);
                comboBox2.SelectedIndex = 0;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void listBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (m_janela_if_ativada)
                {
                    if (this.listBox8.SelectedItem.ToString() == "Entre (inclusive)"
                        || this.listBox8.SelectedItem.ToString() == "Entre (exclusive)")
                    {
                        this.nudValor2Comparacao.Enabled = true;
                        this.dateValor2Comparacao.Enabled = true;
                        this.txtValor2Comparacao.Enabled = true;
                    }
                    else
                    {
                        this.nudValor2Comparacao.Enabled =
                        this.dateValor2Comparacao.Enabled =
                        this.txtValor2Comparacao.Enabled = false;
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (m_janela_if_ativada)
                {
                    string var = this.listBox6.SelectedItem.ToString();

                    if (m_dt_dados.Columns[var].DataType == typeof(DateTime))
                    {
                        nudValor1Comparacao.Visible = nudValor2Comparacao.Visible = false;
                        txtValor2Comparacao.Visible = txtValor1Comparacao.Visible = false;
                        dateValor1Comparacao.Visible = dateValor2Comparacao.Visible = true;
                    }
                    else
                    {
                        if (m_dt_dados.Columns[var].DataType == typeof(string) ||
                            m_dt_dados.Columns[var].DataType == typeof(String) ||
                            m_dt_dados.Columns[var].DataType == typeof(object) ||
                            m_dt_dados.Columns[var].DataType == typeof(Object))
                        {
                            nudValor1Comparacao.Visible = nudValor2Comparacao.Visible = false;
                            txtValor2Comparacao.Visible = txtValor1Comparacao.Visible = true;
                            dateValor1Comparacao.Visible = dateValor2Comparacao.Visible = false;
                        }
                        else
                        {
                            nudValor1Comparacao.Visible = nudValor2Comparacao.Visible = true;
                            txtValor2Comparacao.Visible = txtValor1Comparacao.Visible = false;
                            dateValor1Comparacao.Visible = dateValor2Comparacao.Visible = false;
                        }
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnMedidasPoligonos_Click(object sender, EventArgs e)
        {
            try
            {
                BLExecutarCalculadora blc = new BLExecutarCalculadora();

                if (this.m_shape != null && this.m_shape.Count > 0)
                {
                    if (checkBox4.Checked || checkBox5.Checked || checkBox7.Checked || checkBox6.Checked || ckbBoundingBox.Checked)
                    {
                        int[] filtro = this.DefinicaoObservacoesFiltradas();

                        if (!checkBox6.Checked)
                        {
                            Cursor = Cursors.WaitCursor;
                            blc.AdicionaMedidasPoligonos(this.m_dt_dados, this.m_shape, checkBox4.Checked, checkBox5.Checked, checkBox7.Checked, checkBox6.Checked, ckbBoundingBox.Checked,
                                "", "", filtro);
                        }
                        else
                        {
                            Cursor = Cursors.WaitCursor;
                            blc.AdicionaMedidasPoligonos(this.m_dt_dados, this.m_shape, checkBox4.Checked, checkBox5.Checked, checkBox7.Checked, checkBox6.Checked, ckbBoundingBox.Checked,
                                this.comboBox1.SelectedItem.ToString(), this.comboBox2.SelectedItem.ToString(), filtro);
                        }

                        #region gera matriz de distâncias

                        if (ckbGeraMatrizDistancias.Checked)
                        {
                            string variavel = cmbVariavelMatrizDistancias.SelectedItem.ToString();

                            DataTable dt_dists = new DataTable();

                            blc.GeraMatrizDistanciasPoligonos(this.m_dt_dados, variavel, this.m_shape, out dt_dists);
                            
                            this.userControlDataGrid1.TabelaDados = dt_dists;
                            this.userControlDataGrid1.HabilitaControlesContextMenu();

                            if (!this.tabControl1.TabPages.Contains(tabPage6))
                            {
                                this.tabControl1.TabPages.Add(tabPage6);
                            }
                            tabControl1.SelectedTab = tabPage6;
                        }

                        #endregion

                        Cursor = Cursors.Default;

                        if (!ckbGeraMatrizDistancias.Checked)
                        {
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Selecione pelo menos uma das opções para medidas dos polígonos.");
                    }
                }
                else
                {
                    MessageBox.Show("Arquivo shape não está definido.");
                }
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                if (!this.checkBox8.Checked)
                {
                    BLExecutarCalculadora blc = new BLExecutarCalculadora();
                    double[,] v = new double[this.m_dt_dados.Rows.Count, 1];
                    for (int i = 0; i < v.GetLength(0); i++)
                    {
                        v[i, 0] = Convert.ToDouble(this.m_dt_dados.Rows[i][this.listBox1.SelectedItem.ToString()]);
                    }
                    double corr_max = 0.0;
                    double lambda_max = blc.LambdaOtimoBoxCox(v, -5.0, 5.0, ref corr_max);
                    this.nudParametroFuncaoUnivariada.Enabled = true;
                    this.nudParametroFuncaoUnivariada.Value = Convert.ToDecimal(lambda_max);
                    this.nudParametroFuncaoUnivariada.Enabled = false;

                    IpeaGeo.RegressoesEspaciais.clsUtilTools clt = new clsUtilTools();

                    lblCorrOtimaBoxCox.Text = "Correlação ótima para seleção do lambda do Box-Cox: " + clt.Double2Texto(corr_max, 4);
                }

                if (this.checkBox8.Checked) this.nudParametroFuncaoUnivariada.Enabled = true;
                else this.nudParametroFuncaoUnivariada.Enabled = false;

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ckbGeraMatrizDistancias_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbGeraMatrizDistancias.Checked)
            {
                this.cmbVariavelMatrizDistancias.Enabled = true;

                clsUtilTools clt = new clsUtilTools();

                string[] variaveis = clt.RetornaTodasColunas(m_dt_dados);
                this.cmbVariavelMatrizDistancias.Items.Clear();
                cmbVariavelMatrizDistancias.Items.AddRange(variaveis);
                cmbVariavelMatrizDistancias.SelectedIndex = 0;
            }
            else
            {
                this.cmbVariavelMatrizDistancias.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            this.DialogResult = DialogResult.OK;
        }


    }
}
