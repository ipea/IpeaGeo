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

namespace IpeaGEO.Forms
{
    public partial class frmCriptografia : Form
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

        public frmCriptografia()
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


        private string[,] ascii = {{"0","control"},
{"1","control"},
{"2","control"},
{"3","control"},
{"4","control"},
{"5","control"},
{"6","control"},
{"7","control"},
{"8","control"},
{"9","\t"},
{"10","\n"},
{"11","\v"},
{"12","\f"},
{"13","\r"},
{"14","control"},
{"15","control"},
{"16","control"},
{"17","control"},
{"18","control"},
{"19","control"},
{"20","control"},
{"21","control"},
{"22","control"},
{"23","control"},
{"24","control"},
{"25","control"},
{"26","control"},
{"27","control"},
{"28","control"},
{"29","control"},
{"30","control"},
{"31","control"},
{"32","space"},
{"33","!"},
{"34","\""},
{"35","#"},
{"36","$"},
{"37","%"},
{"38","&"},
{"39","'"},
{"40","("},
{"41",")"},
{"42","*"},
{"43","+"},
{"44",","},
{"45","-"},
{"46","."},
{"47","/"},
{"48","0"},
{"49","1"},
{"50","2"},
{"51","3"},
{"52","4"},
{"53","5"},
{"54","6"},
{"55","7"},
{"56","8"},
{"57","9"},
{"58",":"},
{"59",";"},
{"60","<"},
{"61","="},
{"62",">"},
{"63","?"},
{"64","@"},
{"65","A"},
{"66","B"},
{"67","C"},
{"68","D"},
{"69","E"},
{"70","F"},
{"71","G"},
{"72","H"},
{"73","I"},
{"74","J"},
{"75","K"},
{"76","L"},
{"77","M"},
{"78","N"},
{"79","O"},
{"80","P"},
{"81","Q"},
{"82","R"},
{"83","S"},
{"84","T"},
{"85","U"},
{"86","V"},
{"87","W"},
{"88","X"},
{"89","Y"},
{"90","Z"},
{"91","["},
{"92","\\"},
{"93","]"},
{"94","^"},
{"95","_"},
{"96","`"},
{"97","a"},
{"98","b"},
{"99","c"},
{"100","d"},
{"101","e"},
{"102","f"},
{"103","g"},
{"104","h"},
{"105","i"},
{"106","j"},
{"107","k"},
{"108","l"},
{"109","m"},
{"110","n"},
{"111","o"},
{"112","p"},
{"113","q"},
{"114","r"},
{"115","s"},
{"116","t"},
{"117","u"},
{"118","v"},
{"119","w"},
{"120","x"},
{"121","y"},
{"122","z"},
{"123","{"},
{"124","|"},
{"125","}"},
{"126","~"},
{"127","control"}};

        private double[] primes = { 3, 5, 7, 11, 13, 17, 19, 23, 29 };


        private string ConverterParaCodigo(string dado)
        {
            string resultado = "";

            for (int i = 0; i < ascii.GetLength(0); i++)
            {
                if (dado == ascii[i, 1].ToString())
                {
                    resultado = ascii[i, 0];
                    break;
                }
            }

            return resultado;
        }

        private string ConverterDeCodigo(string dado)
        {
            string resultado = "";

            for (int i = 0; i < ascii.GetLength(0); i++)
            {
                if (dado == ascii[i, 0].ToString())
                {
                    resultado = ascii[i, 1];
                    break;
                }
            }


            return resultado;
        }      

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable tabela = new DataTable();
            tabela = (System.Data.DataTable)dataGridView1.DataSource;

            DataTable tabela2 = new DataTable();
            tabela2 = tabela.Copy();

            Random aleatorio = new Random();
            int a1 = aleatorio.Next(primes.Length);
            int a2 = aleatorio.Next(primes.Length);

            double primo1 = primes[a1];
            double primo2 = primes[a2];

            double n = primo1 * primo2;

            //Função totiente
            double totient = (primo1 - 1.0) * (primo2 - 1.0);

            double _e_ = 0;

            for (int l = 2; l < totient; l++)
            {
                int modulo = (int)totient % l;

                if (modulo != 0)
                {
                    _e_ = l; 
                    break;
                }
            }

            //_e_ já é a chave publica


            //Falta a chave privada tal que e.d - 1 pode ser dividido pelo totiente
            double _d_ = 0;
            for (int i = 2; i < totient; i++)
            {
                double ed_1 = (_e_ * i) - 1.0;
                int modulo = (int)totient % (int)ed_1;
                if (modulo == 0)
                {
                    _d_ = i;
                    break;
                }
            }


            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                for (int j = 0; j < tabela.Columns.Count; j++)
                {
                    string dado = tabela.Rows[i][j].ToString();
                    string resultado = "";
                    for (int k = 0; k < dado.Length; k++)
                    {
                        string subdado = dado.Substring(k, 1);
                        string subresultado = ConverterParaCodigo(subdado);

                        resultado += subresultado;

                    }
                    tabela2.Rows[i][j] = resultado;

                    //Agora vamos criptografar esse numero
                    double res = Convert.ToDouble(resultado);

                    double c = (Math.Pow(res, _e_) % n);

                    tabela2.Rows[i][j] = c.ToString();


                    //Voltando
                    double mensagem = (Math.Pow(c, _d_) % n);

                    //Esta mensagem está em ASCII




                }
            }
        }
    }
}
