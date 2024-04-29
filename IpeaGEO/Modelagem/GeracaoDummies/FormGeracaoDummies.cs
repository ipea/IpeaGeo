using System;
using System.Data;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo
{
    public partial class FormGeracaoDummies : Form
    {
        public FormGeracaoDummies()
        {
            InitializeComponent();
        }

        private DataTable m_dt_dados = new DataTable();
        public DataTable TabelaDados
        {                        
            get { return m_dt_dados; }
            set 
            { 
                m_dt_dados = value;

                clsUtilTools clt = new clsUtilTools();

                string[] variaveis = clt.RetornaTodasColunas(m_dt_dados);

                userControlSelecaoVariaveis1.PermiteSelecaoMultipla = true;
                userControlSelecaoVariaveis1.ZeraControle();
                userControlSelecaoVariaveis1.VariaveisList = variaveis;
                userControlSelecaoVariaveis1.VariaveisDB = variaveis;

                string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_dados);

                listBox1.Items.Clear();
                listBox1.Items.AddRange(variaveis_numericas);
                listBox1.SelectedIndex = 0;
            }
        }

        private void FormGeracaoDummies_Load(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Contains(tabPage2))
                this.tabControl1.TabPages.Remove(tabPage2);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tabela de dados será atualizada com inclusão de variáveis dummies geradas.", "Atualização", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                string[] variaveis_selecionadas = userControlSelecaoVariaveis1.VariaveisIndependentes;

                IpeaGeo.Modelagem.BLogicGeracaoDummies blg = new IpeaGeo.Modelagem.BLogicGeracaoDummies();

                blg.GerarDummies(ref m_dt_dados, variaveis_selecionadas);

                dataGridView1.DataSource = m_dt_dados;

                if (!tabControl1.TabPages.Contains(tabPage2))
                    this.tabControl1.TabPages.Add(tabPage2);

                tabControl1.SelectedTab = tabPage2;

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void rdbNumIntervalos_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbNumIntervalos.Checked) nudNumIntervalos.Enabled = true;
            else nudNumIntervalos.Enabled = false;
        }

        private string m_variavel_quant = "";

        private void btnGerarIntervalos_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                clsUtilTools clt = new clsUtilTools();

                string variavel = listBox1.SelectedItem.ToString();

                this.m_variavel_quant = variavel;

                double[,] dados = clt.GetMatrizFromDataTable(this.m_dt_dados, variavel);

                #region quartis

                if (rdbQuartil.Checked)
                {
                    double[,] q = new double[3,1];
                    int[] nq = new int[4];

                    q[0,0] = clt.Percentil(dados, 25.0);
                    q[1,0] = clt.Percentil(dados, 50.0);
                    q[2,0] = clt.Percentil(dados, 75.0);
                    double maxim = clt.Max(dados);

                    nq = clt.ContaFrequenciaEmIntervalos(dados, q);

                    while (dataGridView2.Rows.Count > 1)
                    {
                        dataGridView2.Rows.RemoveAt(0);
                    }

                    dataGridView2.Rows.Clear();

                    for (int i = 0; i < 4; i++) dataGridView2.Rows.Add();

                    for (int i =0; i<3; i++)
                    {
                        dataGridView2.Rows[i].Cells[0].Value = i + 1;
                        dataGridView2.Rows[i].Cells[1].Value = (q[i,0]);
                        dataGridView2.Rows[i].Cells[2].Value = nq[i];
                    }
                    dataGridView2.Rows[3].Cells[0].Value = 4;
                    dataGridView2.Rows[3].Cells[1].Value = (maxim);
                    dataGridView2.Rows[3].Cells[2].Value = nq[3];

                    dataGridView1.Invalidate();
                    dataGridView1.Refresh();
                    dataGridView1.Focus();
                    dataGridView1.Update();
                    Application.DoEvents();
                }

                #endregion
                
                #region Decis

                if (this.rdbDecis.Checked)
                {
                    double[,] q = new double[9, 1];
                    int[] nq = new int[10];

                    for (int i = 0; i < q.GetLength(0); i++)
                    {
                        q[i, 0] = clt.Percentil(dados, ((double)(i+1))*10.0);
                    }
                    double maxim = clt.Max(dados);

                    nq = clt.ContaFrequenciaEmIntervalos(dados, q);

                    while (dataGridView2.Rows.Count > 1)
                    {
                        dataGridView2.Rows.RemoveAt(0);
                    }

                    dataGridView2.Rows.Clear();

                    for (int i = 0; i < 10; i++) dataGridView2.Rows.Add();

                    for (int i = 0; i < 9; i++)
                    {
                        dataGridView2.Rows[i].Cells[0].Value = i + 1;
                        dataGridView2.Rows[i].Cells[1].Value = (q[i, 0]);
                        dataGridView2.Rows[i].Cells[2].Value = nq[i];
                    }
                    dataGridView2.Rows[9].Cells[0].Value = 10;
                    dataGridView2.Rows[9].Cells[1].Value = (maxim);
                    dataGridView2.Rows[9].Cells[2].Value = nq[9];

                    dataGridView1.Invalidate();
                    dataGridView1.Refresh();
                    dataGridView1.Focus();
                    dataGridView1.Update();
                    Application.DoEvents();
                }

                #endregion

                #region numero intervalos dado pelo usuario
                
                if (this.rdbNumIntervalos.Checked)
                {
                    int nint = (int)nudNumIntervalos.Value;

                    double[,] q = new double[nint-1, 1];
                    int[] nq = new int[nint];

                    for (int i = 0; i < q.GetLength(0); i++)
                    {
                        q[i, 0] = clt.Percentil(dados, ((double)(i + 1)) * (100.0)/((double)(nint)));
                    }
                    double maxim = clt.Max(dados);

                    nq = clt.ContaFrequenciaEmIntervalos(dados, q);

                    while (dataGridView2.Rows.Count > 1)
                    {
                        dataGridView2.Rows.RemoveAt(0);
                    }

                    dataGridView2.Rows.Clear();

                    for (int i = 0; i < nint; i++) dataGridView2.Rows.Add();

                    for (int i = 0; i < nint-1; i++)
                    {
                        dataGridView2.Rows[i].Cells[0].Value = i + 1;
                        dataGridView2.Rows[i].Cells[1].Value = q[i, 0];
                        dataGridView2.Rows[i].Cells[2].Value = nq[i];
                    }
                    dataGridView2.Rows[nint-1].Cells[0].Value = nint;
                    dataGridView2.Rows[nint-1].Cells[1].Value = (maxim);
                    dataGridView2.Rows[nint-1].Cells[2].Value = nq[nint-1];

                    dataGridView1.Invalidate();
                    dataGridView1.Refresh();
                    dataGridView1.Focus();
                    dataGridView1.Update();
                    Application.DoEvents();
                }

                #endregion

                this.btnAtualizarIntervalos.Enabled = true;
                this.btnGerarDummiesQuant.Enabled = true;

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAtualizarIntervalos_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                clsUtilTools clt = new clsUtilTools();

                string variavel = this.m_variavel_quant;

                double[,] dados = clt.GetMatrizFromDataTable(this.m_dt_dados, variavel);

                double[,] cortes = new double[dataGridView2.Rows.Count - 1, 1];

                #region checando se valores dos cortes estão em ordem crescente
                
                double vl = 0.0;
                bool teste = false;

                for (int i = 0; i < cortes.GetLength(0); i++)
                {
                    teste = Double.TryParse(dataGridView2.Rows[i].Cells[1].Value.ToString(), out vl);

                    if (!teste) throw new Exception("Não foi possível converter o valor da célula para número.");

                    cortes[i, 0] = vl;
                    if (i > 0 && cortes[i, 0] <= cortes[i - 1, 0]) throw new Exception("Valores dos cortes devem estar em ordem estritamente crescente.");
                }

                #endregion

                #region preenchendo os valores de quantidades em cada intervalo 

                double[,] cortes_int = new double[cortes.GetLength(0) - 1, 1];
                for (int i = 0; i < cortes_int.GetLength(0); i++) cortes_int[i, 0] = cortes[i, 0];

                int[] nq = clt.ContaFrequenciaEmIntervalos(dados, cortes_int);

                for (int i = 0; i < this.dataGridView2.Rows.Count - 1; i++) dataGridView2.Rows[i].Cells[2].Value = nq[i];

                #endregion

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGerarDummiesQuant_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                clsUtilTools clt = new clsUtilTools();
                
                #region checando se valores dos cortes estão em ordem crescente

                string variavel = this.m_variavel_quant;
                double[,] cortes = new double[dataGridView2.Rows.Count - 1, 1];
                double[,] dados = clt.GetMatrizFromDataTable(this.m_dt_dados, variavel);

                double vl = 0.0;
                bool teste = false;

                for (int i = 0; i < cortes.GetLength(0); i++)
                {
                    teste = Double.TryParse(dataGridView2.Rows[i].Cells[1].Value.ToString(), out vl);

                    if (!teste) throw new Exception("Não foi possível converter o valor da célula para número.");

                    cortes[i, 0] = vl;
                    if (i > 0 && cortes[i, 0] <= cortes[i - 1, 0]) throw new Exception("Valores dos cortes devem estar em ordem estritamente crescente.");
                }

                #endregion

                #region categorias dos intervalos

                double[,] cortes_int = new double[cortes.GetLength(0) - 1, 1];
                for (int i = 0; i < cortes_int.GetLength(0); i++) cortes_int[i, 0] = cortes[i, 0];

                object[,] categorias = new object[0, 0];

                int[] nq = clt.ContaFrequenciaEmIntervalos(dados, cortes_int, out categorias);

                for (int i = 0; i < this.dataGridView2.Rows.Count - 1; i++) dataGridView2.Rows[i].Cells[2].Value = nq[i];

                #endregion

                string[] variaveis_selecionadas = new string[1];
                variaveis_selecionadas[0] = m_variavel_quant + "_intervalo";
                Type[] tipo_variavel = new Type[1];
                tipo_variavel[0] = typeof(int);

                clt.AdicionaColunasToDataTable(ref m_dt_dados, categorias, variaveis_selecionadas, tipo_variavel);

                IpeaGeo.Modelagem.BLogicGeracaoDummies blg = new IpeaGeo.Modelagem.BLogicGeracaoDummies();

                blg.GerarDummies(ref m_dt_dados, variaveis_selecionadas);

                m_dt_dados.Columns.Remove(variaveis_selecionadas[0]);

                dataGridView1.DataSource = m_dt_dados;

                if (!tabControl1.TabPages.Contains(tabPage2))
                    this.tabControl1.TabPages.Add(tabPage2);

                tabControl1.SelectedTab = tabPage2;

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
