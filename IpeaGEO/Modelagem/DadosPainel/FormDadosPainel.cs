using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Data.OleDb;
using System.Threading;

namespace IpeaGEO.Modelagem
{
    public partial class FormDadosPainel : Form
    {
        #region Variáveis internas

        private DataTable m_dt_tabela_shape = new DataTable();
        public DataTable DadosShape
        {
            get { return m_dt_tabela_shape; }
            set { m_dt_tabela_shape = value; }
        }

        private DataTable m_dt_tabela_dados = new DataTable();
        public DataTable TabelaDeDados
        {
            get { return m_dt_tabela_dados; }
            set { m_dt_tabela_dados = value; }
        }

        private clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            get { return m_shape; }
            set { m_shape = value; }
        }

        private string m_variavel_unidade_observacional = "";
        private string m_variavel_unidade_tempo = "";

        private bool m_vizinhanca_definida = false;
        private bool m_dados_concatenados = false;
        private bool m_tabela_importada = false;

        #endregion

        public FormDadosPainel()
        {
            InitializeComponent();
        }

        private void AtualizaTabelaDados()
        {
            clsUtilTools clt = new clsUtilTools();

            this.dataGridView1.DataSource = m_dt_tabela_dados;

            string[] todas_variaveis = clt.RetornaTodasColunas(m_dt_tabela_dados);
            string[] num_variaveis = clt.RetornaColunasNumericas(m_dt_tabela_dados);

            this.cmbVariavelPeriodosTempo.Items.Clear();
            this.cmbVariavelPeriodosTempo.Items.AddRange(todas_variaveis);
            this.cmbVariavelPeriodosTempo.SelectedIndex = 0;

            this.cmbVariavelUnidadesObservacionais.Items.Clear();
            this.cmbVariavelUnidadesObservacionais.Items.AddRange(todas_variaveis);
            this.cmbVariavelUnidadesObservacionais.SelectedIndex = 0;
            
            this.userControlRegressaoInstrumentos1.ZeraControle();
            this.userControlRegressaoInstrumentos1.VariaveisDB = num_variaveis;
            this.userControlRegressaoInstrumentos1.VariaveisList = num_variaveis;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormBaseModelagem_Load(object sender, EventArgs e)
        {

        }

        #region Open tabela de dados e tabela shape

        private void btnAbrirArquivoShape_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblProgressBar.Text = "Importando arquivo em formato shape";

                clsUtilArquivos cla = new clsUtilArquivos();
                cla.ImportarArquivoShape(ref m_shape, ref m_dt_tabela_shape);

                this.lblProgressBar.Text = "Arquivo shape importado com sucesso";
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAbrirTabelaDados_Click(object sender, EventArgs e)
        {
            try
            {
                clsUtilArquivos cla = new clsUtilArquivos();
                if (cla.ImportarTabelaDados(ref m_dt_tabela_dados))
                {
                    AtualizaTabelaDados();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region toolstrips menus

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblProgressBar.Text = "Importando arquivo em formato shape";

                clsUtilArquivos cla = new clsUtilArquivos();
                cla.ImportarArquivoShape(ref m_shape, ref m_dt_tabela_shape);

                this.lblProgressBar.Text = "Arquivo shape importado com sucesso";
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            try
            {
                clsUtilArquivos cla = new clsUtilArquivos();
                if (cla.ImportarTabelaDados(ref m_dt_tabela_dados))
                {
                    AtualizaTabelaDados();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exportarDadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                clsUtilArquivos cla = new clsUtilArquivos();
                cla.ExportarTabela((DataTable)this.dataGridView1.DataSource, this.Name);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void calculadoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGEO.RegressoesEspaciais.FormCalculadora frm = new IpeaGEO.RegressoesEspaciais.FormCalculadora();
                    frm.MdiParent = this.MdiParent;
                    frm.Dados = this.m_dt_tabela_dados;

                    //if (m_dados_concatenados)
                    //{
                    //    frm.DadosConcatenados = true;
                    //    frm.Shape = m_shape;
                    //}
                    //else
                    //{
                    //    if (m_W_sparsa_from_dists_existente)
                    //    {
                    //        frm.UsaMatrizEsparsaFromDistancias = true;
                    //        frm.MatrizEsparsaFromDistancias = m_W_sparsa_from_dists;
                    //    }
                    //    if (m_W_sparsa_from_arquivo_existente)
                    //    {
                    //        frm.UsaMatrizEsparsaFromDistancias = true;
                    //        frm.MatrizEsparsaFromDistancias = m_W_sparsa_from_arquivo;
                    //    }
                    //}

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.m_dt_tabela_dados = frm.Dados;

                        //this.AtualizaTabelaDadosCalculadora();
                    }
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    //FormCalculadora frm = new FormCalculadora();
                    //frm.MdiParent = this.MdiParent;
                    //frm.Dados = this.m_dt_tabela_dados;
                    //frm.AtivaMedidasPoligonos = true;
                    //if (m_dados_concatenados)
                    //{
                    //    frm.DadosConcatenados = true;
                    //    frm.Shape = m_shape;
                    //}

                    //if (frm.ShowDialog() == DialogResult.OK)
                    //{
                    //    this.m_dt_tabela_dados = frm.Dados;

                    //    this.AtualizaTabelaDadosCalculadora();
                    //}
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.ToString(), "Erro.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void excluirVariáveisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGEO.RegressoesEspaciais.FormCalculadora frm = new IpeaGEO.RegressoesEspaciais.FormCalculadora();
                    frm.MdiParent = this.MdiParent;
                    frm.Dados = this.m_dt_tabela_dados;
                    frm.AtivaExclusaoVariaveis = true;
                    //if (m_dados_concatenados)
                    //{
                    //    frm.DadosConcatenados = true;
                    //    frm.Shape = m_shape;
                    //}

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.m_dt_tabela_dados = frm.Dados;

                        this.AtualizaTabelaDados();
                    }
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void estatísticasDescritivasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGEO.RegressoesEspaciais.FormEstatisticasDescritivas frm = new IpeaGEO.RegressoesEspaciais.FormEstatisticasDescritivas();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.m_dt_tabela_dados;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGEO.RegressoesEspaciais.FormTabelaFrequencias frm = new IpeaGEO.RegressoesEspaciais.FormTabelaFrequencias();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.m_dt_tabela_dados;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGEO.RegressoesEspaciais.FormTabelasCruzadas frm = new IpeaGEO.RegressoesEspaciais.FormTabelasCruzadas();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.m_dt_tabela_dados;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGEO.RegressoesEspaciais.FormCorrelacoes frm = new IpeaGEO.RegressoesEspaciais.FormCorrelacoes();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.m_dt_tabela_dados;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {

        }

        private void exportarMatrizDeVizinhançaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void importarMatrizDeVizinhançaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        #endregion

        private void cmbVariavelUnidadesObservacionais_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.m_variavel_unidade_observacional = this.cmbVariavelUnidadesObservacionais.SelectedItem.ToString();
        }

        private void cmbVariavelPeriodosTempo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.m_variavel_unidade_tempo = this.cmbVariavelPeriodosTempo.SelectedItem.ToString();
        }

        #region concatenação da tabela de shape com a tabela de dados

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGEO.RegressoesEspaciais.FormConcatenacaoTabelaToShape frm = new IpeaGEO.RegressoesEspaciais.FormConcatenacaoTabelaToShape();
                frm.MdiParent = this.MdiParent;
                frm.TabelaDados = this.m_dt_tabela_dados;
                frm.TabelaShape = this.m_dt_tabela_shape;
                frm.Shape = this.m_shape;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    TabelaDeDados = frm.TabelaDadosConcatenados;

                    ConcatenarDados();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private clsMatrizEsparsa m_W_sparsa_from_dists = new clsMatrizEsparsa();
        private bool m_W_sparsa_from_dists_existente = false;

        private bool m_W_sparsa_from_arquivo_existente = false;
        private clsMatrizEsparsa m_W_sparsa_from_arquivo = new clsMatrizEsparsa();

        private void ConcatenarDados()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int obs_shape = m_shape.Count;
                int obs_dados = this.m_dt_tabela_dados.Rows.Count;

                if (obs_dados > obs_shape) MessageBox.Show("O número de observações na tabela de dados é maior do que o número de observações no arquivo shape.",
                    "Falha na concatenação", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (obs_dados < obs_shape) MessageBox.Show("O número de observações na tabela de dados é menor do que o número de observações no arquivo shape.",
                    "Falha na concatenação", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (obs_dados == obs_shape)
                {
                    //MessageBox.Show("Tabela de dados e arquivo shape concatenados. A concatenação assume que os elementos na tabela de dados está na mesma ordem que os elementos no arquivo shape.",
                    //"Concatenação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    m_dados_concatenados = true;

                    //this.ckbWXcovariaveis.Enabled = true;
                    //this.rdbModelosSAR_viaMLE.Enabled = true;
                    //this.rdbModelosSEM_viaMLE.Enabled = true;
                    //this.rdbModeloSAC_viaMLE.Enabled = true;

                    //this.rdbSEM_FGLS.Enabled =
                    //    this.rdbSEM_OLS.Enabled = true;

                    //this.rdbSARKelejianPrucha.Enabled = true;
                    //this.rdbModelosSEM_viaMLE.Checked = true;
                    //this.grbTipoMatrizVizinhanca.Enabled = true;
                    //this.grbCalculoLogDet.Enabled = true;

                    //this.tabControl1.SelectedTab = tabPage1;

                    this.m_W_sparsa_from_dists_existente = false;
                    this.m_W_sparsa_from_arquivo_existente = false;
                }

                lblProgressBar.Text = "Tabelas concatenadas com sucesso";

                this.Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
    }
}
