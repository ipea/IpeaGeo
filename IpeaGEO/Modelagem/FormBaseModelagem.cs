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

namespace IpeaGeo.Modelagem
{
    public partial class FormBaseModelagem : Form
    {
        #region Variáveis internas

        private DataTable m_dt_tabela_shape = new DataTable();
        public DataTable DadosShape
        {
            get { return m_dt_tabela_shape; }
            set { m_dt_tabela_shape = value; }
        }

        private DataTable m_dt_tabela_dados = new DataTable();
        public DataTable Dados
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

        #endregion

        public FormBaseModelagem()
        {
            InitializeComponent();
        }

        private void AtualizaTabelaDados()
        {
            this.dataGridView1.DataSource = m_dt_tabela_dados;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {              
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
                    IpeaGeo.RegressoesEspaciais.FormCalculadora frm = new IpeaGeo.RegressoesEspaciais.FormCalculadora();
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
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void excluirVariáveisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormCalculadora frm = new IpeaGeo.RegressoesEspaciais.FormCalculadora();
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
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void estatísticasDescritivasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormEstatisticasDescritivas frm = new IpeaGeo.RegressoesEspaciais.FormEstatisticasDescritivas();
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
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormTabelaFrequencias frm = new IpeaGeo.RegressoesEspaciais.FormTabelaFrequencias();
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
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormTabelasCruzadas frm = new IpeaGeo.RegressoesEspaciais.FormTabelasCruzadas();
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
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormCorrelacoes frm = new IpeaGeo.RegressoesEspaciais.FormCorrelacoes();
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
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
    }
}
