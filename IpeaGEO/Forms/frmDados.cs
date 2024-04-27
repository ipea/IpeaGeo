using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Forms
{
    public partial class frmDados : Form
    {

        private System.Data.OleDb.OleDbConnection m_cnn = new System.Data.OleDb.OleDbConnection();
        private System.Data.OleDb.OleDbDataAdapter m_dap = new System.Data.OleDb.OleDbDataAdapter();
        private string strExtensao = "";
        private string strEnderecoBase = "";
        private DataSet ds_tabelas_csv = new System.Data.DataSet();

        public frmDados()
        {
            InitializeComponent();
        }

        #region Leitura das Tabelas em CSV

        public static void TransferCSVToTable(ref DataTable dt1, string filePath)
        {
            string[] csvRows = System.IO.File.ReadAllLines(filePath);
            string[] fields = null;
            foreach (string csvRow in csvRows)
            {
                fields = csvRow.Split(';');

                if (dt1.Columns.Count == 0)
                {
                    for (int i = 0; i < fields.Length; i++)
                    {
                        dt1.Columns.Add(fields[i]);
                    }
                }
                else
                {
                    DataRow row = dt1.NewRow();
                    row.ItemArray = fields;
                    dt1.Rows.Add(row);
                }
            }


        }



        #endregion

        private void frmDados_Load(object sender, EventArgs e)
        {
            this.txtPesquisar.ForeColor = Color.Silver;

            //impede que os split containers mudem a razao de divisao entre eles//
            spc_1.IsSplitterFixed = true;
            spc_2.IsSplitterFixed = true;

            try
            {
                clsUtilTools mUtils1 = new clsUtilTools();

                //Cria um DataTable
                DataTable dt1 = new DataTable();

                // acha o endereco de onde o aplicativo esta sendo lido
                string strEndereco1 = Application.ExecutablePath;

                FileInfo fiExcel1 = new FileInfo(strEndereco1);

                ArrayList arquivos = new ArrayList();
                arquivos.Add("\\TabelaIpeaGeo1.csv");
                arquivos.Add("\\TabelaIpeaGeo2.csv");
                arquivos.Add("\\TabelaIpeaGeo3.csv");
                arquivos.Add("\\TabelaIpeaGeo4.csv");
                arquivos.Add("\\TabelaIpeaGeo5.csv");
                arquivos.Add("\\TabelaIpeaGeo6.csv");

                ArrayList nomes = new ArrayList();
                nomes.Add("Arqui1");
                nomes.Add("Arqui2");
                nomes.Add("Arqui3");
                nomes.Add("Arqui4");
                nomes.Add("Arqui5");
                nomes.Add("Arqui6");

                string strEndCompleto = "";

                for (int i = 0; i < arquivos.Count; i++)
                {
                    dt1 = new System.Data.DataTable();

                    strEndCompleto = fiExcel1.DirectoryName + (string)arquivos[i];

                    TransferCSVToTable(ref dt1, strEndCompleto);

                    dt1.TableName = (string)nomes[i];

                    ds_tabelas_csv.Tables.Add(dt1);
                }


                #region Combobox

                //--- preenchendo o combobox ---//

                cmbListaTemas.ForeColor = Color.Silver;

                dt1 = (DataTable)ds_tabelas_csv.Tables[0];
                cmbListaTemas.Items.Clear();
                cmbListaTemas.Items.Add("Pesquisar em");
                cmbListaTemas.Items.Add(" ");
                for (int i = 0; i < dt1.Columns.Count - 2; i++)
                {
                    cmbListaTemas.Items.Add((string)dt1.Columns[i].ColumnName);
                }
                cmbListaTemas.SelectedIndex = 0;

                #endregion

                #region Treeview

                //--- preenchendo o treeview ---//

                // DataTable dt2 = (DataTable)ds_tabelas_csv.Tables[2];
                // DataTable dt3 = (DataTable)ds_tabelas_csv.Tables[3];


                treeView1.Nodes.Clear();
                checkedListBox1.Items.Clear();

                for (int i = 0; i < dt1.Columns.Count - 2; i++)
                {
                    treeView1.Nodes.Add((string)dt1.Columns[i].ColumnName);

                    treeView1.Nodes[i].Nodes.Clear();

                    for (int j = 0; j < ds_tabelas_csv.Tables[i + 1].Rows.Count; j++)
                    {
                        treeView1.Nodes[i].Nodes.Add((string)ds_tabelas_csv.Tables[i + 1].Rows[j][1]);
                        checkedListBox1.Items.Add((string)ds_tabelas_csv.Tables[i + 1].Rows[j][1]);
                    }

                }

                #endregion

                #region Checklist Box

                // ---- preenchendo o checklist box --- //
                // checkedListBox1.Items.Clear();

                // for (int i = 0; i < dt2.Rows.Count; i++)
                //{
                //     checkedListBox1.Items.Add((string)dt2.Rows[i][1]);
                // }

                #endregion

                // --- linkando o treeview com o checklist box ---

            }

            catch (Exception er)
            {
                MessageBox.Show(er.Message);

            }

        }


        #region Importar

        private void btnImportar_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;

            progressBar1.Maximum = 100000;
            progressBar1.Minimum = 0;
            for (int i = 0; i < 100000; i++)
            {
                progressBar1.Value = i;
            }


        }

        #endregion




        bool txtBox = false;
        private void txtPesquisar_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            txtPesquisar.ForeColor = Color.Black;

            if (txtBox == false)
            {
                txtPesquisar.Text = "";
                txtBox = true;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //--muda o cursor quando este estiver em cima do tree view--//
        private void treeView1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            //cmbListaTemas.Text = "Pesquisar em";
        }



        private bool m_cmbtemas_iniciado = false;

        private void cmbListaTemas_Click(object sender, EventArgs e)
        {
            try
            {
                if (!m_cmbtemas_iniciado)
                {
                    cmbListaTemas.ForeColor = Color.Black;
                    cmbListaTemas.Items.RemoveAt(0);
                    m_cmbtemas_iniciado = true;
                }
            }
            catch (Exception er)
            {
            }
        }

        private void btnPesquisa_Click(object sender, EventArgs e)
        {
            try
            {
                treeView1.CollapseAll();

                DataTable dt1 = (DataTable)ds_tabelas_csv.Tables[0];
                string pesquisa = (txtPesquisar.Text).ToLower();
                string tentativa = "";

                ArrayList dim1 = new ArrayList();
                ArrayList dim2 = new ArrayList();
                ArrayList st_encontrado = new ArrayList();

                checkedListBox1.Items.Clear();

                for (int i = 0; i < dt1.Columns.Count - 2; i++)
                {
                    for (int j = 0; j < ds_tabelas_csv.Tables[i + 1].Rows.Count; j++)
                    {
                        tentativa = ((string)ds_tabelas_csv.Tables[i + 1].Rows[j][1]);
                        if (EncontrarString(pesquisa, tentativa.ToLower()))
                        {
                            dim1.Add(i);
                            dim2.Add(j);
                            st_encontrado.Add(tentativa);

                            checkedListBox1.Items.Add(tentativa);
                        }
                    }
                }

            }

            catch (Exception er)
            {
            }
        }

        private bool EncontrarString(string pesquisa, string tentativa)
        {
            double valor_corte = 1000;
            if (DistStrings(pesquisa, tentativa) < valor_corte)
                return true;

            return false;
        }

        private double DistStrings(string pesquisa, string tentativa)
        {
            double res = Double.PositiveInfinity;

            double indice = tentativa.IndexOf(pesquisa);

            if (indice >= 0) res = 0;

            return res;
        }

        // --- tecla enter ligada ao botao pesquisar --- 
        private void frmDados_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            {
                if (e.KeyCode == System.Windows.Forms.Keys.Enter)
                {
                    btnPesquisa_Click(sender, e);
                }
            }
        }
    }
}






