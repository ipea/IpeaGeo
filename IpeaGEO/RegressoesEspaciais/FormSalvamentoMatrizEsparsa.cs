using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace IpeaGeo.RegressoesEspaciais
{
    public partial class FormSalvamentoMatrizEsparsa : Form
    {
        private bool m_formulario_importacao = false;
        
        private clsMatrizEsparsa m_T = new clsMatrizEsparsa();
        private clsMatrizEsparsa m_C = new clsMatrizEsparsa();
        private bool m_matriz_sparsa_normalizada = false;
        private bool m_matriz_sparsa_importada = false;

        public FormSalvamentoMatrizEsparsa(bool formulario_importacao)
        {
            InitializeComponent();

            m_formulario_importacao = formulario_importacao;
            if (!formulario_importacao)
            {
                this.btnArquivo1.Enabled = false;
                this.btnExportar1.Enabled = true;
                this.btnVisualizar.Visible = true;
                this.btnImportarMatrizFromArquivo.Visible = false;
                this.grbTipoMatrizVizinhanca.Enabled = true;
                this.rdbMatrizVizinhancaNormalizada.Checked = true;
                this.btnMatrizEsparsa.Visible = false;
                this.btnMatrizEsparsa.Enabled = false;
                this.btnExportar1.Visible = true;

                if (this.tabControl1.TabPages.Contains(this.tabPage3))
                    this.tabControl1.TabPages.Remove(this.tabPage3);
            }
            else
            {
                this.btnOK.Enabled = false;

                this.btnArquivo1.Enabled = true;
                this.btnExportar1.Enabled = false;
                this.btnVisualizar.Visible = false;
                this.btnImportarMatrizFromArquivo.Visible = true;
                this.grbTipoMatrizVizinhanca.Enabled = true;

                //this.rdbMatrizVizinhancaOriginal.Checked = true;
                this.rdbMatrizVizinhancaNormalizada.Checked = true;

                this.btnMatrizEsparsa.Visible = true;
                this.btnExportar1.Visible = false;
                this.btnMatrizEsparsa.Enabled = false;

                if (this.tabControl1.TabPages.Contains(this.tabPage1))
                    this.tabControl1.TabPages.Remove(this.tabPage1);

                if (this.tabControl1.TabPages.Contains(this.tabPage3))
                    this.tabControl1.TabPages.Remove(this.tabPage3);
            }
        }

        private TipoMatrizVizinhanca m_tipo_matriz = TipoMatrizVizinhanca.Normalizada;
        public TipoMatrizVizinhanca TipoMatrizVizinhanca
        {
            set
            {
            	m_tipo_matriz = value;

                if (m_tipo_matriz == TipoMatrizVizinhanca.Normalizada) rdbMatrizVizinhancaNormalizada.Checked= true;
                if (m_tipo_matriz == TipoMatrizVizinhanca.Original) rdbMatrizVizinhancaOriginal.Checked = true;

                grbTipoMatrizVizinhanca.Enabled = false;
            }
            get
            {
                if (this.m_matriz_sparsa_normalizada) return TipoMatrizVizinhanca.Normalizada;
                else return TipoMatrizVizinhanca.Original;
            }
        }

        private DataTable m_dt_dados = new DataTable();
        public DataTable DtDados
        {
            get
            {
                return m_dt_dados;
            }
            set
            {
            	this.m_dt_dados = value;

                if (!this.m_formulario_importacao)
                {
                    this.rdbIdFromDataTable.Enabled = true;
                    clsUtilTools clt = new clsUtilTools();
                    string[] nomes_variaveis = clt.RetornaTodasColunas(m_dt_dados);
                    this.lstVariavelIdentificadora.Items.Clear();
                    this.lstVariavelIdentificadora.Items.AddRange(nomes_variaveis);
                    this.lstVariavelIdentificadora.SelectedIndex = 0;
                }
            }
        }

        private clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            set
            {
            	this.m_shape = value.Clone();
            }
        }

        private BLModelosCrossSectionSpaciaisLineares blr = new BLModelosCrossSectionSpaciaisLineares();

        private clsMatrizEsparsa m_W_sparsa_from_dists;
        public clsMatrizEsparsa WsparsaFromDists
        {
            get
            {
                return m_W_sparsa_from_dists.Clone();
            }
            set
            {
            	m_W_sparsa_from_dists = value.Clone();
            }
        }

        public clsMatrizEsparsa WsparsaImportada
        {
            get
            {
                return m_C;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbIdFromDataTable.Checked) this.lstVariavelIdentificadora.Enabled = true;
            else lstVariavelIdentificadora.Enabled = false;
        }

        private void Exportar()
        {
            try
            {
                //string saida = GerarFormatoTriplet();

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                if (this.rdbTripleFormat.Checked)
                {
                    saveFileDialog1.Filter = "Triplet format (*.tpf)|*.tpf";
                }
                else
                {
                    if (this.rdbListaVizinhos.Checked)
                    {
                        saveFileDialog1.Filter = "Vizinhos (*.vzs)|*.vzs";
                    }
                }

                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor = Cursors.WaitCursor;

                    clsUtilTools clt = new clsUtilTools();
                    clt.ExportarArquivoTexto(m_conteudo_arquivo, saveFileDialog1.FileName);
                    this.lblArquivo.Text = saveFileDialog1.FileName;

                    Cursor = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            this.lblResultados.Text = "";
            this.Exportar();
            this.lblResultados.Text = "Matriz esparsa exportada com sucesso.";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private string m_conteudo_arquivo = "";
        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                this.lblResultados.Text = "";

                m_conteudo_arquivo = GerarFormatoTriplet();
                this.richTextBox1.Text = m_conteudo_arquivo;

                this.tabControl1.SelectedTab = tabPage2;

                Cursor = Cursors.Default;

                this.lblResultados.Text = "Matriz esparsa pronta para visualização no format triplet.";
            }
            catch (Exception er)
            {
                this.lblResultados.Text = "";
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool m_tipo_matriz_alterada = false;

        private string GerarFormatoTriplet()
        {
            if (m_W_sparsa_from_dists == null || m_tipo_matriz_alterada)
            {
                blr.Shape = this.m_shape;
                if (rdbMatrizVizinhancaNormalizada.Checked) blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Normalizada;
                else blr.TipoMatrizVizinhanca = TipoMatrizVizinhanca.Original;
                blr.GeraMatrizVizinhanca();
                m_W_sparsa_from_dists = blr.MatrizEsparsaFromDistancias;

                m_tipo_matriz_alterada = false;
            }

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsMatrizEsparsa w_temp = fme.CompressColumn2TripletForm(m_W_sparsa_from_dists);

            if (this.rdbCountersStartingFromZero.Checked)
            {
                return fme.ImprimeMatrizEsparsaTripletForm(w_temp, true, this.ckbNomeVarFirstLine.Checked);
            }

            if (this.rdbCountersStartingFromOne.Checked)
            {
                string[] labels = new string[w_temp.m];
                for (int i = 0; i < w_temp.m; i++)
                {
                    labels[i] = (i+1).ToString();
                }
                return fme.ImprimeMatrizEsparsaTripletForm(w_temp, labels, true, this.ckbNomeVarFirstLine.Checked);
            }

            if (this.rdbIdFromDataTable.Checked)
            {
                string[] labels = new string[m_dt_dados.Rows.Count];
                string v = this.lstVariavelIdentificadora.SelectedItem.ToString();
                for (int i = 0; i < this.m_dt_dados.Rows.Count; i++)
                {
                    labels[i] = this.m_dt_dados.Rows[i][v].ToString();
                }
                return fme.ImprimeMatrizEsparsaTripletForm(w_temp, labels, true, this.ckbNomeVarFirstLine.Checked);
            }

            return "";
        }

        private void FormSalvamentoMatrizEsparsa_Load(object sender, EventArgs e)
        {

        }

        private void rdbMatrizVizinhancaOriginal_CheckedChanged(object sender, EventArgs e)
        {
            m_tipo_matriz_alterada = true;
        }

        private void rdbMatrizVizinhancaNormalizada_CheckedChanged(object sender, EventArgs e)
        {
            m_tipo_matriz_alterada = true;
        }

        private void Importar()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Triplet format (*.tpf)|*.tpf|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                clsUtilTools clt = new clsUtilTools();
                string conteudo = "";
                string informacoes = "";

                clt.ImportarArquivoTexto(ref conteudo, ref informacoes, openFileDialog.FileName);

                this.m_conteudo_arquivo = conteudo;
                this.richTextBox1.Text = conteudo;

                if (!this.tabControl1.TabPages.Contains(this.tabPage1))
                    this.tabControl1.TabPages.Add(this.tabPage1);

                this.lblArquivo.Text = openFileDialog.FileName;
            }
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblResultados.Text = "";
                this.Importar();
                this.lblResultados.Text = "Arquivo importado com sucesso.";
                this.btnMatrizEsparsa.Enabled = true;
                this.btnMatrizEsparsa.Visible = true;
            }
            catch (Exception er)
            {
                this.lblResultados.Text = "";
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ImportarMatrizFromArquivo()
        {
            this.lblResultados.Text = "";

            ArrayList ind_i = new ArrayList();
            ArrayList ind_j = new ArrayList();
            ArrayList mat_ij = new ArrayList();
            ArrayList inds = new ArrayList();

            clsUtilTools clt = new clsUtilTools();

            string[] aux1 = this.m_conteudo_arquivo.Split(Convert.ToChar("\n"));
            string[] aux2 = new string[0];
            int indice = 0;

            int inicio_arquivo = 0;
            if (this.ckbNomeVarFirstLine.Checked)
            {
                inicio_arquivo = 1;
            }

            for (int k = inicio_arquivo; k < aux1.GetLength(0); k++)
            {
                indice = 0;
                aux2 = aux1[k].Split(Convert.ToChar("\t"));
                for (int l = 0; l < aux2.GetLength(0); l++)
                {
                    if (aux2[l].Length > 0)
                    {
                        if (indice == 0) ind_i.Add(aux2[l].Trim());
                        if (indice == 1) ind_j.Add(aux2[l].Trim());
                        if (indice == 2) mat_ij.Add(aux2[l].Trim());

                        indice++;
                    }
                }
            }

            if (ind_i.Count != ind_j.Count || ind_i.Count != mat_ij.Count) throw new Exception("Falha na importação da matriz de vizinhança.");
            if (ind_i.Count <= 0) throw new Exception("Falha na importação da matriz de vizinhança. Nenhuma observação foi importada."); 

            int[] mi = new int[ind_i.Count];
            int[] mj = new int[ind_j.Count];
            double[] mx = new double[mat_ij.Count];
            int temp = 0;
            double d = 0.0;
            object[,] aux_T = new object[mi.GetLength(0), 2];

            for (int k = 0; k < mi.GetLength(0); k++)
            {
                if (Int32.TryParse(ind_i[k].ToString(), out temp)) mi[k] = temp;
                else throw new Exception("Falha na importação da matriz de vizinhança. Problemas na conversão para inteiro dos indexadores de linhas da matriz de vizinhança.");

                if (Int32.TryParse(ind_j[k].ToString(), out temp)) mj[k] = temp;
                else throw new Exception("Falha na importação da matriz de vizinhança. Problemas na conversão para inteiro dos indexadores de colunas da matriz de vizinhança.");

                d = clt.DoubleFromTexto(mat_ij[k]);
                if (double.IsInfinity(d) || double.IsNaN(d) || double.IsNegativeInfinity(d) || double.IsPositiveInfinity(d))
                    throw new Exception("Falha na importação da matriz de vizinhança. Problemas na conversão para double dos elementos da matriz de vizinhança.");
                else mx[k] = d;

                aux_T[k, 0] = mi[k] * 1000000 + mj[k];
                aux_T[k, 1] = mx[k];
            }

            int min_mi = mi[0];
            int max_mi = mi[0];
            int min_mj = mj[0];
            int max_mj = mj[0];

            for (int k = 0; k < mi.GetLength(0); k++)
            {
                if (mi[k] < min_mi) min_mi = mi[k];
                if (mi[k] > max_mi) max_mi = mi[k];
                if (mj[k] < min_mj) min_mj = mj[k];
                if (mj[k] > max_mj) max_mj = mj[k];
            }

            int num_obs = this.m_dt_dados.Rows.Count;

            if (rdbCountersStartingFromOne.Checked && (min_mi <= 0 || min_mj <= 0))
                throw new Exception("Falha na importação da matriz de vizinhança. Identificadores de linhas e colunas têm que ter valor mínimo maior ou igual a um.");

            if (rdbCountersStartingFromOne.Checked && (max_mi > num_obs || max_mj > num_obs))
                throw new Exception("Falha na importação da matriz de vizinhança. Identificadores de linhas e colunas têm que ter valor máximo igual ou menor que o número de observações na tabela de dados.");

            if (this.rdbCountersStartingFromZero.Checked && (max_mi >= num_obs || max_mj >= num_obs))
                throw new Exception("Falha na importação da matriz de vizinhança. Identificadores de linhas e colunas têm que ter valor máximo menor que o número de observações na tabela de dados.");

            if (min_mi < 0 || min_mj < 0)
                throw new Exception("Falha na importação da matriz de vizinhança. Identificadores de linhas e colunas têm que ter valor mínimo maior ou igual a zero.");

            /*----------------------------------------------------------------*/
            /*-- criação da matriz esparsa a partir da matriz importada     --*/
            /*----------------------------------------------------------------*/

            int resto = 0;
            int numero = 0;
            object[,] saux_T = new object[0, 0];
            int res = clt.SortByColumn(ref saux_T, aux_T, 0);

            ArrayList checa_indices = new ArrayList();
            numero = Convert.ToInt32(saux_T[0, 0]);
            mi[0] = Math.DivRem(numero, 1000000, out resto);
            mj[0] = resto;
            mx[0] = Convert.ToDouble(saux_T[0, 1]);
            checa_indices.Add(numero);
            int row = 1;
            for (int k = 1; k < aux_T.GetLength(0); k++)
            {
                numero = Convert.ToInt32(saux_T[k, 0]);
                if (!checa_indices.Contains(numero))
                {
                    mi[row] = Math.DivRem(numero, 1000000, out resto);
                    mj[row] = resto;
                    mx[row] = Convert.ToDouble(saux_T[k, 1]);

                    if (this.rdbCountersStartingFromOne.Checked)
                    {
                        mi[row] = mi[row] - 1;
                        mj[row] = mj[row] - 1;
                    }

                    row++;
                }
            }

            double[] norm_mx = new double[mx.GetLength(0)];
            for (int k = 0; k < mx.GetLength(0); k++)
            {
                norm_mx[k] = mx[k];
            }

            if (rdbMatrizVizinhancaNormalizada.Checked)
            {
                this.m_matriz_sparsa_normalizada = true;
                NormalizarMatrizTripletFormat(mi, num_obs, mx, ref norm_mx);
            }
            else
            {
                if (ChecaMatrizNormalizada(mi, num_obs, mx))
                {
                    this.m_matriz_sparsa_normalizada = true;
                    NormalizarMatrizTripletFormat(mi, num_obs, mx, ref norm_mx);
                    rdbMatrizVizinhancaNormalizada.Checked = true;
                }
                else
                {
                    this.m_matriz_sparsa_normalizada = false;
                }
            }

            m_T = new clsMatrizEsparsa(num_obs, num_obs, norm_mx.GetLength(0), norm_mx, true);
            m_T.row_indices = mi;
            m_T.col_indices = mj;
            m_T.x = norm_mx;
            m_T.nz = norm_mx.GetLength(0);
            m_T.nzmax = norm_mx.GetLength(0); 
            
            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            m_C = fme.TripletForm2CompressColumn(m_T);

            this.richTextBox2.Text = m_C.EstatisticasDescritivas();

            if (!this.tabControl1.TabPages.Contains(tabPage3))
                this.tabControl1.TabPages.Add(tabPage3);

            this.lblResultados.Text = "Matriz esparsa carregada com sucesso.";
            this.m_matriz_sparsa_importada = true;

            //MessageBox.Show("Matriz esparsa carregada com sucesso. Assume-se que a ordem dos indexadores dos elementos da matriz seja a mesma das observações na tabela de dados.",
            //    "Matriz esparsa", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.btnOK.Enabled = true;
        }

        private bool ChecaMatrizNormalizada(int[] mi, int nrows, double[] mx)
        {
            double[] soma_x = new double[nrows];
            for (int k = 0; k < mi.GetLength(0); k++)
            {
                soma_x[mi[k]] += mx[k];
            }
            for (int k = 0; k < nrows; k++)
            {
                if (Math.Abs(soma_x[k] - 1.0) > 1.0e-6 && Math.Abs(soma_x[k]) > 1.0e-6)
                {
                    return false;
                }
            }
            return true;
        }

        private void NormalizarMatrizTripletFormat(int[] mi, int nrows, double[] mx, ref double[] norm_mx)
        {
            double[] soma_x = new double[nrows];
            for (int k = 0; k < mi.GetLength(0); k++)
            {
                soma_x[mi[k]] += mx[k];
            }
            norm_mx = new double[mx.GetLength(0)];
            for (int k = 0; k < mi.GetLength(0); k++)
            {
                if (soma_x[mi[k]] != 0.0)
                {
                    norm_mx[k] = mx[k] / soma_x[mi[k]];
                }
                else
                {
                    norm_mx[k] = mx[k];
                }
            }
        }

        private void btnImportarMatrizFromArquivo_Click(object sender, EventArgs e)
        {
            try
            {
                this.ImportarMatrizFromArquivo();

                string mensagem = "Matriz esparsa carregada com sucesso. ";
                mensagem += "Assume-se que a ordem dos indexadores dos elementos da matriz seja a mesma das observações na tabela de dados. ";
                mensagem += "Deseja utilizar a matriz gerada como matriz de vizinhança nas análises estatísticas?";

                DialogResult disp = MessageBox.Show(mensagem, 
                    "Importação da matriz de vizinhança", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (disp == DialogResult.Yes)
                {
                    FecharFormularioOK();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FecharFormularioOK()
        {
            if (m_matriz_sparsa_importada && m_formulario_importacao)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.FecharFormularioOK();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnMatrizEsparsa_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.tabControl1.TabPages.Contains(this.tabPage1))
                    this.tabControl1.TabPages.Add(this.tabPage1);

                this.tabControl1.SelectedTab = this.tabPage1;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
