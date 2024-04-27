using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using ZedGraph;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo
{
    public partial class frmCluster : Form
    {
        public frmCluster()
        {
            InitializeComponent();
            tabControl1.SelectTab(1);
        }

        private bool m_shape_carregado_do_mapa = false;

        public void HabilitarDadosExternos()
        {
            btnAtualizarTabelaExterna.Visible = true;
            btnAtualizarTabelaExterna.Enabled = true;

            btnAbrirArquivoShape.Enabled =
                btnImportarTabela.Enabled =
                btnConcatenarDados.Enabled = false;

            m_shape_carregado_do_mapa = true;
        }

        #region Métodos para a escolha do número de conglomerados

        private double[] dblPseudoT;
        public double[] PseudoT
        {
            get
            {
                return dblPseudoT;
            }
            set
            {
                dblPseudoT = value;
            }
        }
        private double[] dblPseudoF;
        public double[] PseudoF
        {
            get
            {
                return dblPseudoF;
            }
            set
            {
                dblPseudoF = value;
            }
        }
        private double[] dblRSquare;
        public double[] RSquare
        {
            get
            {
                return dblRSquare;
            }
            set
            {
                dblRSquare = value;
            }
        }

        private double[] dblRSquarePartial;
        public double[] RSquarePartial
        {
            get
            {
                return dblRSquarePartial;
            }
            set
            {
                dblRSquarePartial = value;
            }
        }
        private double[] dblRSquareExpected;
        public double[] RSquareExpected
        {
            get
            {
                return dblRSquareExpected;
            }
            set
            {
                dblRSquareExpected = value;
            }
        }
        private double[] dblCCC;
        public double[] CCC
        {
            get
            {
                return dblCCC;
            }
            set
            {
                dblCCC = value;
            }
        }

        #endregion

        #region Variáveis internas

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
            get
            {
                return strVariaveis;
            }
            set
            {
                strVariaveis = value;
            }
        }
        private DataTable tabela_congl;
        public DataTable tabela_congl_
        {
            get
            {
                return tabela_congl;
            }
            set
            {
                tabela_congl = value;
            }
        }
        private string[] strVariaveisSelecionadas;
        public string[] VariaveisSelecionadas
        {
            get
            {
                return strVariaveisSelecionadas;
            }
            set
            {
                strVariaveisSelecionadas = value;
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
        private string strNumCluster;
        public string NumeroDeConglomerados
        {
            get
            {
                return strNumCluster;
            }
            set
            {
                strNumCluster = value;
            }
        }
        private bool blEspacial;
        public bool IsSpatialCluster
        {
            get
            {
                return blEspacial;
            }
            set
            {
                blEspacial = value;
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

                AtualizaTabelaDados();
            }
        }

        private DataGridView m_gridview_externo = new DataGridView();
        public DataGridView GridViewExterno
        {
            set
            {
                m_gridview_externo = value;
            }
        }

        private DataSet m_dataset_externo = new DataSet();
        public DataSet DataSetExterno
        {
            set
            {
                m_dataset_externo = value;
            }
            get
            {
                return m_dataset_externo;
            }
        }

        private void AtualizaTabelaDadosCalculadora()
        {
            clsUtilTools clt = new clsUtilTools();

            this.dataGridView3.DataSource = this.dTable;

            for (int i = 0; i < this.dataGridView3.Columns.Count; i++)
            {
                this.dataGridView3.Columns[i].Width = 180;
            }
        }

        #region Atualizar tabela no formulário de mapas

        private void btnAtualizarTabelaExterna_Click(object sender, EventArgs e)
        {
            try
            {
                m_gridview_externo.DataSource = ((DataTable)this.dataGridView3.DataSource).Copy();

                this.DataSetExterno.Tables.Clear();
                this.DataSetExterno.Tables.Add(((DataTable)this.dataGridView3.DataSource).Copy());

                label7.Text = "Tabela atualizada no formulário de mapas";
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Variáveis da matriz esparsa externa (pré-definida no frmMapas)

        private string m_tipo_matriz_W_predefinida = "";
        private int m_ordem_matriz_W_predefinida = 1;
        private bool m_matriz_W_normalizada = true;
        private bool m_usa_W_sparsa_predefinida = false;
        private clsMatrizEsparsa m_W_sparsa_predefinida = new clsMatrizEsparsa();

        public clsMatrizEsparsa MatrizWEsparsaPredefinida
        {
            get { return m_W_sparsa_predefinida; }
            set
            {
                m_W_sparsa_predefinida = value;
            }
        }

        public bool MatrizWNormalizadaPredefinida
        {
            set { m_matriz_W_normalizada = value; }
        }

        public int OrdemMatrizVizinhancaPredefinida
        {
            set { m_ordem_matriz_W_predefinida = value; }
        }

        public string TipoMatrizVizinhancaPredefinida
        {
            set { m_tipo_matriz_W_predefinida = value; }
        }

        public bool UsaWSparsaPredefinida
        {
            set
            {
                m_usa_W_sparsa_predefinida = value;
                UsaApenasMatrizWPredefinida();
            }
        }

        private void UsaApenasMatrizWPredefinida()
        {

            m_dados_concatenados = true;

            this.tabControl1.SelectedTab = tabPage1;

        }

        #endregion

        #region abrir arquivo shape

        private clsMapa m_mapa;
        private SharpMap.Map m_sharp_mapa;
        private DataTable m_tabela_dados;
        private DataTable m_tabela_shape;

        private bool m_vizinhanca_definida = false;
        private bool m_dados_concatenados = false;
        private bool m_tabela_importada = false;

        private clsIpeaShape m_shape;
        private string m_tipo_vizinhanca = "";

        public clsIpeaShape Shape
        {
            get { return this.m_shape; }
            set 
            { 
                this.m_shape = value;


                if (!m_usa_W_sparsa_predefinida)
                {
                    this.btnConcatenarDados.Enabled = true;
                }
            }
        }

        public DataTable TabelaShape
        {
            get
            {
                return m_tabela_shape;
            }
            set
            {
            	m_tabela_shape = value;
            }
        }

        private void OpenShapeFile()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "ShapeFile (*.shp)|*.shp|All Files (*.*)|*.*";
                string FileName = "";
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    FileName = openFileDialog.FileName;
                    Cursor.Current = Cursors.WaitCursor;

                    m_mapa = new clsMapa();

                    m_mapa.LoadingMapa("mapa teste", FileName, this.ckbCriarEstruturaPoligonos.Checked);

                    this.m_tabela_shape = m_mapa.TabelaDados;

                    this.m_sharp_mapa = m_mapa.Sharp_Mapa;

                    Cursor.Current = Cursors.Default;

                    if (this.m_mapa.VetorShapes.GetLength(0) > 0 && (this.m_mapa.VetorShapes)[0] != null)
                    {
                        m_shape = (this.m_mapa.VetorShapes)[0];

                        this.btnConcatenarDados.Enabled = true;
                    }

                    m_vizinhanca_definida = false;
                    m_dados_concatenados = false;

                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAbrirArquivoShape_Click(object sender, EventArgs e)
        {
            try
            {
                progressBar1.Visible = true;
                this.label7.Text = "Importando arquivo em formato shape";

                this.OpenShapeFile();

                this.label7.Text = "Arquivo shape importado com sucesso";
                progressBar1.Visible = false;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public DataTable TabelaDeDados
        {
            get { return this.dTable; }
            set
            {
                this.dTable = value;

                AtualizaTabelaDados();
            }
        }

        private void btnConcatenarDados_Click(object sender, EventArgs e)
        {
            FormConcatenacaoTabelaToShape frm = new FormConcatenacaoTabelaToShape();
            //frm.MdiParent = this.MdiParent;
            frm.TabelaDados = this.dTable;
            frm.TabelaShape = this.m_tabela_shape;
            frm.Shape = this.m_shape;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                TabelaDeDados = frm.TabelaDadosConcatenados;

                ConcatenarDados();

                if (shapeAlex == null)
                {
                    tabControl1.TabPages.Add(tabPage1);
                    this.strVariaveis = new string[TabelaDeDados.Columns.Count];
                    for (int i = 0; i < TabelaDeDados.Columns.Count; i++) strVariaveis[i] = TabelaDeDados.Columns[i].ColumnName;

                    PopulaDataGridView(ref dataGridView2, strVariaveis.Length);
                }
            }

            //ConcatenarDados();
        }

        private void ConcatenarDados()
        {
            ConcatenarDados("");
        }

        private void ConcatenarDados(string mensagem)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int obs_shape = m_shape.Count;
                int obs_dados = this.dTable.Rows.Count;

                if (obs_dados > obs_shape) MessageBox.Show("O número de observações na tabela de dados é maior do que o número de observações no arquivo shape.",
                    "Falha na concatenação", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (obs_dados < obs_shape) MessageBox.Show("O número de observações na tabela de dados é menor do que o número de observações no arquivo shape.",
                    "Falha na concatenação", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (obs_dados == obs_shape)
                {
                    //MessageBox.Show("Tabela de dados e arquivo shape concatenados. A concatenação assume que os elementos na tabela de dados está na mesma ordem que os elementos no arquivo shape.",
                    //"Concatenação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    m_dados_concatenados = true;

                    this.tabControl1.SelectedTab = tabPage1;

                    //this.m_W_sparsa_from_dists_existente = false;
                    //this.m_W_sparsa_from_arquivo_existente = false;
                }

                if (mensagem == "")
                {
                    label7.Text = "Tabelas concatenadas com sucesso";
                }
                else
                {
                    label7.Text = mensagem;
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        private void AtualizaTabelaDados()
        {
            clsUtilTools clt = new clsUtilTools();

            this.dataGridView3.DataSource = this.dTable;
            this.dataGridView3.Refresh();

            for (int i = 0; i < this.dataGridView3.Columns.Count; i++)
            {
                this.dataGridView3.Columns[i].Width = 150;
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            try
            {
                if (dTable.Rows.Count > 0 && dTable.Columns.Count > 0)
                {
                    FormCalculadora frm = new FormCalculadora();
                    //frm.MdiParent = this.MdiParent;
                    frm.Dados = this.dTable;
                    frm.AtivaMedidasPoligonos = true;
                    if (m_dados_concatenados)
                    {
                        frm.DadosConcatenados = true;
                        frm.Shape = m_shape;
                    }

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.dTable = frm.Dados;

                        this.AtualizaTabelaDadosCalculadora();
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

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            try
            {
                if (((DataTable)dataGridView3.DataSource).Columns.Count > 0 && ((DataTable)dataGridView3.DataSource).Rows.Count > 0)
                {
                    DataTable dsTemp = (DataTable)dataGridView3.DataSource;
                    //dsTemp.Tables[0].Columns.Remove("Mapa"+strIDmapa);
                    //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    //saveFileDialog1.InitialDirectory = "C:\\";
                    //saveFileDialog1.Filter = "Excel (*.xls)|*.xls|Access (*.mdb)|*.mdb|XML (*.xml)|*.xml|Texto (*.txt)|*.txt";
                    //saveFileDialog1.FilterIndex = 1;
                    //saveFileDialog1.RestoreDirectory = true;
                    //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;

                        BLExportacaoAberturaExcel ble = new BLExportacaoAberturaExcel();
                        ble.ExportaToExcel(dsTemp, "tabela", "tabela");

                        Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void calculadoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dTable.Rows.Count > 0 && dTable.Columns.Count > 0)
                {
                    FormCalculadora frm = new FormCalculadora();
                    //frm.MdiParent = this.MdiParent;
                    frm.Dados = this.dTable;
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
                        this.dTable = frm.Dados;

                        this.AtualizaTabelaDadosCalculadora();
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

        private void geraçãoDeVariáveisDummyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dTable.Rows.Count > 0 && dTable.Columns.Count > 0)
                {
                    FormGeracaoDummies frm = new FormGeracaoDummies();
                    //frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.dTable.Copy();

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.dTable = frm.TabelaDados;

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

        private void excluirVariáveisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dTable.Rows.Count > 0 && dTable.Columns.Count > 0)
                {
                    FormCalculadora frm = new FormCalculadora();
                    //frm.MdiParent = this.MdiParent;
                    frm.Dados = this.dTable;
                    frm.AtivaExclusaoVariaveis = true;
                    if (m_dados_concatenados)
                    {
                        frm.DadosConcatenados = true;
                        frm.Shape = m_shape;
                    }

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.dTable = frm.Dados;

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


        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                this.label7.Text = "Importando arquivo em formato shape";

                this.OpenShapeFile();

                this.label7.Text = "Arquivo shape importado com sucesso";
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private DataTable m_dt_dados_originais = new DataTable();

        private void ImportarArquivoDeDados()
        {
            try
            {
                clsUtilArquivos cla = new clsUtilArquivos();
                if (cla.ImportarTabelaDados(ref dTable))
                {
                    this.dataGridView3.DataSource = dTable;

                    m_dt_dados_originais = dTable.Copy();

                    AtualizaTabelaDados();
                    btnConcatenarDados.Enabled = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnImportarTabela_Click(object sender, EventArgs e)
        {
            try
            {
                ImportarArquivoDeDados();
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
                ImportarArquivoDeDados();
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
                ExportData ed = new ExportData();
                ed.ExportarDados(dataGridView3, this.Name);
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
                if (dTable.Rows.Count > 0 && dTable.Columns.Count > 0)
                {
                    FormEstatisticasDescritivas frm = new FormEstatisticasDescritivas();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.dTable;
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
                if (dTable.Rows.Count > 0 && dTable.Columns.Count > 0)
                {
                    FormTabelaFrequencias frm = new FormTabelaFrequencias();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.dTable;
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
                if (dTable.Rows.Count > 0 && dTable.Columns.Count > 0)
                {
                    FormTabelasCruzadas frm = new FormTabelasCruzadas();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.dTable;
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
                if (dTable.Rows.Count > 0 && dTable.Columns.Count > 0)
                {
                    FormCorrelacoes frm = new FormCorrelacoes();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.dTable;
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

        /// <summary>
        /// tabela de dados botoes acabam aqui
        /// </summary>
                   
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
        private string strDistancia;
        public string Distancia
        {
            get
            {
                return strDistancia;
            }
            set
            {
                strDistancia = value;
            }
        }

        private string strMetodo;
        public string Metodo
        {
            get
            {
                return strMetodo;
            }
            set
            {
                strMetodo = value;
            }
        }
        private string strFatorMinkowsky;
        public string FatorMinkowsky
        {
            get
            {
                return strFatorMinkowsky;
            }
            set
            {
                strFatorMinkowsky = value;
            }
        }
        public string[] strVetorPesos;
        public string[] VetorDePesos
        {
            get
            {
                return strVetorPesos;
            }
            set
            {
                strVetorPesos = value;
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
        private string[] strCoresRGB;
        public string[] CoresRGB
        {
            get
            {
                return strCoresRGB;
            }
            set
            {
                strCoresRGB = value;
            }
        }

        private string stringEML;
        public string strEML
        {
            get
            {
                return stringEML;
            }
        }

        private Brush[,] coresVetor = new Brush[110, 2];
        private Color[,] coresVetor2 = new Color[110, 2];

        #endregion

        private void PopulaDataGridView(ref DataGridView dataGridView1, int iVariaveis)
        {
            //Cria as colunas do DataGridView
            DataGridViewTextBoxColumn txtbox = new DataGridViewTextBoxColumn();
            txtbox = new DataGridViewTextBoxColumn();
            //txtbox.Width = 260;
            txtbox.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
            txtbox.ReadOnly = true;
            txtbox.HeaderText = "Variável";
            txtbox.DisplayIndex = 2;
            dataGridView1.Columns.Insert(0, txtbox);

            DataGridViewComboBoxColumn chkbox = new DataGridViewComboBoxColumn();
            chkbox = new DataGridViewComboBoxColumn();
            //chkbox.Width = 50;
            chkbox.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
            chkbox.FlatStyle = FlatStyle.Popup;
            chkbox.HeaderText = "Tipo de variável";
            chkbox.DisplayIndex = 2;
            chkbox.Items.Add("Contínuas");
            chkbox.Items.Add("Binárias");
            chkbox.Items.Add("Categóricas");
            chkbox.Items.Add("Ordinais");
            dataGridView1.Columns.Insert(1, chkbox);
            DataGridViewCheckBoxColumn chkbx_key = new DataGridViewCheckBoxColumn();
            chkbx_key = new DataGridViewCheckBoxColumn();
            chkbx_key.Width = 40;
            chkbx_key.FlatStyle = FlatStyle.Popup;
            chkbx_key.HeaderText = "Incluir";
            chkbx_key.DisplayIndex = 2;
            chkbx_key.TrueValue = true;
            dataGridView1.Columns.Insert(2, chkbx_key);

            //Adiciona as variáveis
            dataGridView1.Rows.Add(iVariaveis);
            for (int i = 0; i < strVariaveis.Length ; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = strVariaveis[i];
                dataGridView1.Rows[i].Cells[0].ToolTipText = strVariaveis[i];
                dataGridView1.Rows[i].Cells[2].ToolTipText = strVariaveis[i];
                dataGridView1.Rows[i].Cells[1].Value = "Contínuas";
                dataGridView1.Rows[i].Cells[2].Value = false;
            }
        }

        private void PopulaDataGridViewTree(ref DataGridView dataGridView1, double[,] mDados, int iMaximo, ref DataTable tabela)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(strID, "Variável Identificadora");
            tabela.Columns.Add("Variável Identificadora", System.Type.GetType("System.String"));
            for (int i = 0; i < mDados.GetLength(1); i++)
            {
                int iCluster = iMaximo - i;
                string strNome = "Cluster " + iCluster.ToString();
                string strNome2 = "Var" + i.ToString();
                tabela.Columns.Add("Cluster " + iCluster.ToString(), System.Type.GetType("System.String"));
                dataGridView1.Columns.Add(strNome2, strNome);
                
            }
            Object[] mLinha = new Object[mDados.GetLength(1) + 1];

            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                DataRow linha = tabela.NewRow();
                int iposicao = shapeAlex[i].PosicaoNoDataTable;
                mLinha[0] = (Object)dTable.Rows[iposicao][strID];
                linha[0] = dTable.Rows[iposicao][strID].ToString();
                for (int j = 0; j < mDados.GetLength(1); j++)
                { 
                    mLinha[j + 1] = (Object)mDados[i, j];
                    linha[j + 1] = mDados[i, j].ToString();
                }
                dataGridView1.Rows.Add(mLinha);
                tabela.Rows.Add(linha);
            }
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }

        private void frmCluster_Load(object sender, EventArgs e)
        {
            try
            {
                //Definindo o numero maximo de clusters
                if (shapeAlex != null)
                {
                    numMaxClusters.Maximum = shapeAlex.Count - 1;


                    //checkedListBox1.Items.AddRange(strVariaveis);
                    //PopulaDataGridView(ref dataGridView2, strVariaveis.Length);
                    PopulaDataGridView(ref dataGridView2, strVariaveis.Length);
                }
                dataGridView2.AllowUserToAddRows = false;
                cmbTipoVariaveis.SelectedIndex = 0;
                cmbDistanciaBinaria.SelectedIndex = 0;
                cmbDistanciaCategorica.SelectedIndex = 0;
                cmbDistanciaOrdinaria.SelectedIndex = 0;

                //Inicializa o valor padrão da comboBox
                cmbDistancia.SelectedIndex = 0;
                cmbMetodo.SelectedIndex = 0;
                cmbCores.SelectedIndex = 0;
                cmbVizinhanca.SelectedIndex = 0;

                //Cores
                Color[] vetorCores = new Color[11];
                vetorCores[0] = Color.White;
                vetorCores[1] = Color.Yellow;
                vetorCores[2] = Color.Green;
                vetorCores[3] = Color.Blue;
                vetorCores[4] = Color.Red;
                vetorCores[5] = Color.Purple;
                vetorCores[6] = Color.Cyan;
                vetorCores[7] = Color.Coral;
                vetorCores[8] = Color.Khaki;
                vetorCores[9] = Color.Brown;
                vetorCores[10] = Color.Black;

                //Cores
                Brush[] vetorBrush = new Brush[11];
                vetorBrush[0] = Brushes.White;
                vetorBrush[1] = Brushes.Yellow;
                vetorBrush[2] = Brushes.Green;
                vetorBrush[3] = Brushes.Blue;
                vetorBrush[4] = Brushes.Red;
                vetorBrush[5] = Brushes.Purple;
                vetorBrush[6] = Brushes.Cyan;
                vetorBrush[7] = Brushes.Coral;
                vetorBrush[8] = Brushes.Khaki;
                vetorBrush[9] = Brushes.Brown;
                vetorBrush[10] = Brushes.Black;

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

                if (shapeAlex != null)
                {
                    if (shapeAlex.TipoVizinhanca == "") cmbVizinhanca.Enabled = true;
                }
                else
                {
                    tabControl1.TabPages.Remove(tabPage1);
                    tabControl1.SelectTab(0);
                    btnConcatenarDados.Enabled = false;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }   
        
        private void btnCancela_Click(object sender, EventArgs e)
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                blRelatorio = chkRelatorio.Checked;

                progressBar1.Visible = true;
                this.Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                #region Cores

                //Cores
                Brush[] cores = new Brush[(int)numCluster.Value];
                Color[] coresRGB = new Color[(int)numCluster.Value];
                strCoresRGB = new string[cores.Length];

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


                    //Cria o objeto cor
                    Color mCor0 = new Color();
                    //Set the color
                    mCor0 = Color.FromArgb(colors[0].ToArgb());
                    strCoresRGB[0] = System.Drawing.ColorTranslator.ToHtml(mCor0);
                    //Cria o objeto cor
                    Color mCor1 = new Color();
                    mCor1 = Color.FromArgb(colors[1].ToArgb());
                    strCoresRGB[cores.Length - 1] = System.Drawing.ColorTranslator.ToHtml(mCor1);

                    //Valores RGB
                    double R0 = Convert.ToDouble(colors[0].R);
                    double G0 = Convert.ToDouble(colors[0].G);
                    double B0 = Convert.ToDouble(colors[0].B);
                    double R1 = Convert.ToDouble(colors[1].R);
                    double G1 = Convert.ToDouble(colors[1].G);
                    double B1 = Convert.ToDouble(colors[1].B);

                    //Número de classes
                    double nClasses = Convert.ToDouble(numCluster.Value);

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
                #endregion

                //OK
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                #region Informações para o relatório

                strDistancia = cmbDistancia.SelectedItem.ToString();
                strMetodo = cmbMetodo.SelectedItem.ToString();
                strFatorMinkowsky = numMinkowsky.Value.ToString();
                strNumCluster = numCluster.Value.ToString();
                //stringEML = numEML.ToString();
                progressBar1.Visible = false;
                #endregion                
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmbTipoVariaveis_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbTipoVariaveis.SelectedItem.ToString() == "Contínuas")
                {
                    cmbDistancia.Enabled = true;
                    cmbDistanciaBinaria.Enabled = false;
                    cmbDistanciaCategorica.Enabled = false;
                    cmbDistanciaOrdinaria.Enabled = false;

                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        dataGridView2.Rows[i].Cells[1].Value = "Contínuas";
                        dataGridView2.Rows[i].Cells[1].ReadOnly = true;
                    }
                }
                else if (cmbTipoVariaveis.SelectedItem.ToString() == "Binárias")
                {
                    cmbDistancia.Enabled = false;
                    cmbDistanciaBinaria.Enabled = true;
                    cmbDistanciaCategorica.Enabled = false;
                    cmbDistanciaOrdinaria.Enabled = false;

                    numPesoBinaria.Enabled = true;
                    numPesoCategorica.Enabled = false;
                    numPesoOrdinais.Enabled = false;

                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        dataGridView2.Rows[i].Cells[1].Value = "Binárias";
                        dataGridView2.Rows[i].Cells[1].ReadOnly = true;
                    }
                }
                else if (cmbTipoVariaveis.SelectedItem.ToString() == "Categóricas")
                {
                    cmbDistancia.Enabled = false;
                    cmbDistanciaBinaria.Enabled = false;
                    cmbDistanciaCategorica.Enabled = true;
                    cmbDistanciaOrdinaria.Enabled = false;

                    numPesoBinaria.Enabled = false;
                    numPesoCategorica.Enabled = true;
                    numPesoOrdinais.Enabled = false;

                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        dataGridView2.Rows[i].Cells[1].Value = "Categóricas";
                        dataGridView2.Rows[i].Cells[1].ReadOnly = true;
                    }
                }
                else if (cmbTipoVariaveis.SelectedItem.ToString() == "Ordinais")
                {
                    cmbDistancia.Enabled = false;
                    cmbDistanciaBinaria.Enabled = false;
                    cmbDistanciaCategorica.Enabled = false;
                    cmbDistanciaOrdinaria.Enabled = true;

                    numPesoBinaria.Enabled = false;
                    numPesoCategorica.Enabled = false;
                    numPesoOrdinais.Enabled = true;

                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        dataGridView2.Rows[i].Cells[1].Value = "Ordinais";
                        dataGridView2.Rows[i].Cells[1].ReadOnly = true;
                    }
                }
                else if (cmbTipoVariaveis.SelectedItem.ToString() == "Mistas")
                {
                    cmbDistancia.Enabled = true;
                    cmbDistanciaBinaria.Enabled = true;
                    cmbDistanciaCategorica.Enabled = true;
                    cmbDistanciaOrdinaria.Enabled = true;

                    numPesoBinaria.Enabled = true;
                    numPesoCategorica.Enabled = true;
                    numPesoOrdinais.Enabled = true;

                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        dataGridView2.Rows[i].Cells[1].Value = "Contínuas";
                        dataGridView2.Rows[i].Cells[1].ReadOnly = false;
                    }                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmbDistancia_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (cmbDistancia.SelectedItem.ToString() == "Minkowsky")
                {
                    numMinkowsky.Enabled = true;
                }
                else
                {
                    numMinkowsky.Enabled = false;
                }
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
                Rectangle rectangle = new Rectangle(0, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);

                //Desenha no combobox
                LinearGradientBrush backBrush = new LinearGradientBrush(rectangle, selectedItem.Color1, selectedItem.Color2, LinearGradientMode.Horizontal);

                e.Graphics.FillRectangle(backBrush, rectangle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButton2.Checked == true) numMaxPolygon.Enabled = true;
                else numMaxPolygon.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButton3.Checked == true) percMaxPolygon.Enabled = true;
                else percMaxPolygon.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #region Execução da geração de conglomerados

        private string iNomeDataGridView(DataGridView dataGridView2,string strNome)
        {
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                if (dataGridView2.Rows[i].Cells[0].Value.ToString() == strNome && (bool)dataGridView2.Rows[i].Cells[2].Value)
                {
                    return (dataGridView2.Rows[i].Cells[1].Value.ToString());
                }
           }
            return ("Nada");
        }

        clsClusterizacaoEspacialHierarquica clsAlexCluster = new clsClusterizacaoEspacialHierarquica();
        private void GerarCongromerados()
        {
            try
            {
                if (numMaxClusters.Value < numMinClusters.Value) throw new Exception("Número máximo tem que ser maior ou igual ao número mínimo de clusters.");

                if (numCluster.Value > numMaxClusters.Value) throw new Exception("Número de clusters não pode ser maior do que o número máximo na árvore.");

                if (numCluster.Value < this.numMinClusters.Value) throw new Exception("Número de clusters não pode ser menor do que o número mínimo na árvore.");

                //Captura somente os Items selecionados
                int cSelected = 0;
                for (int i = 0; i < strVariaveis.Length; i++) if ((bool)dataGridView2.Rows[i].Cells[2].Value == true) cSelected++;
                string[] strSelecionadas = new string[cSelected];
                cSelected = 0;
                for (int i = 0; i < strVariaveis.Length; i++)
                {
                    if ((bool)dataGridView2.Rows[i].Cells[2].Value == true)
                    {
                        strSelecionadas[cSelected] = dataGridView2.Rows[i].Cells[0].Value.ToString(); 
                        cSelected++;
                    }
                }
                if (cSelected == 0)
                {
                    MessageBox.Show("Pelo menos uma variável deve ser selecionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnOK.Enabled = false;
                }
                else
                {
                    try
                    {
                        btnOK.Enabled = true;
                        progressBar1.Visible = true;
                        //Guarda as variáveis selecionadas
                        strVariaveisSelecionadas = strSelecionadas;

                        //Guarda a distancia
                        strDistancia = cmbDistancia.SelectedItem.ToString();

                        //Inicializa a classe de clusters espaciais
                        clsSpatialCluster spCluster = new clsSpatialCluster();

                        //Fator de Minkowsky
                        double iMinkowsky = 0;
                        iMinkowsky = (double)numMinkowsky.Value;

                        #region Conglomerados Hierarquicos Espaciais

                        double[,] iMatriz;
                        double[,] tabcluster;

                        if (blEspacial == true)
                        {
                            //Habilita o label
                            label7.Text = "Inicializando procedimento...";
                            Application.DoEvents();

                            int nvars_continuas = 0;
                            int nvars_binarias = 0;
                            int nvars_categoricas = 0;
                            int nvars_ordinais = 0;
                            int nobs = dTable.Rows.Count;

                            for (int i = 0; i < dataGridView2.RowCount; i++)
                            {
                                if (dataGridView2.Rows[i].Cells[1].Value.ToString() == "Binárias" && Convert.ToBoolean(dataGridView2.Rows[i].Cells[2].Value))
                                {
                                    nvars_binarias++;
                                }
                                if (dataGridView2.Rows[i].Cells[1].Value.ToString() == "Contínuas" && Convert.ToBoolean(dataGridView2.Rows[i].Cells[2].Value))
                                {
                                    nvars_continuas++;
                                }
                                if (dataGridView2.Rows[i].Cells[1].Value.ToString() == "Categóricas" && Convert.ToBoolean(dataGridView2.Rows[i].Cells[2].Value))
                                {
                                    nvars_categoricas++;
                                }
                                if (dataGridView2.Rows[i].Cells[1].Value.ToString() == "Ordinais" && Convert.ToBoolean(dataGridView2.Rows[i].Cells[2].Value))
                                {
                                    nvars_ordinais++;
                                }
                            }

                            //Matrizes com os diferentes tipos de dados
                            double[,] mDadosContinuos = new double[0, 0];
                            double[,] mDadosBinarios = new double[0, 0];
                            double[,] mDadosCategoricos = new double[0, 0];
                            double[,] mDadosOrdinais = new double[0, 0];

                            if (nvars_continuas > 0) mDadosContinuos = new double[nobs, nvars_continuas];
                            if (nvars_binarias > 0) mDadosBinarios = new double[nobs, nvars_binarias];
                            if (nvars_categoricas > 0) mDadosCategoricos = new double[nobs, nvars_categoricas];
                            if (nvars_ordinais > 0) mDadosOrdinais = new double[nobs, nvars_ordinais];

                            //Guarda os dados na Matriz
                            double[,] mDados = new double[dTable.Rows.Count, strSelecionadas.Length];
                            int iDummyBinaria = 0;
                            int iDummyContinua = 0;
                            int iDummyCategorica = 0;
                            int iDummyOrdinais = 0;

                            for (int j = 0; j < dTable.Rows.Count; j++)
                            {
                                iDummyBinaria = 0;
                                iDummyCategorica = 0;
                                iDummyContinua = 0;
                                iDummyOrdinais = 0;

                                for (int i = 0; i < strSelecionadas.Length; i++)
                                {
                                    if (double.IsNaN(Convert.ToDouble(dTable.Rows[j][strSelecionadas[i]])) == false)
                                    {
                                        if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Binárias")
                                        {
                                            mDadosBinarios[j, iDummyBinaria] = Convert.ToDouble(dTable.Rows[j][strSelecionadas[i]]);
                                            iDummyBinaria++;
                                        }
                                        else if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Contínuas")
                                        {
                                            mDadosContinuos[j, iDummyContinua] = Convert.ToDouble(dTable.Rows[j][strSelecionadas[i]]);
                                            iDummyContinua++;
                                        }
                                        else if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Categóricas")
                                        {
                                            mDadosCategoricos[j, iDummyCategorica] = Convert.ToDouble(dTable.Rows[j][strSelecionadas[i]]);
                                            iDummyCategorica++;
                                        }
                                        else if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Ordinais")
                                        {
                                            mDadosOrdinais[j, iDummyOrdinais] = Convert.ToDouble(dTable.Rows[j][strSelecionadas[i]]);
                                            iDummyOrdinais++;
                                        }
                                        //mDados[j, i] = Convert.ToDouble(dTable.Rows[j][strSelecionadas[i]]);
                                    }
                                    else
                                    {
                                        if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Binárias")
                                        {
                                            mDadosBinarios[j, iDummyBinaria] = double.NaN;
                                            iDummyBinaria++;
                                        }
                                        else if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Contínuas")
                                        {
                                            mDadosContinuos[j, iDummyContinua] = double.NaN;
                                            iDummyContinua++;
                                        }
                                        else if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Categóricas")
                                        {
                                            mDadosCategoricos[j, iDummyCategorica] = double.NaN;
                                            iDummyCategorica++;
                                        }
                                        else if (iNomeDataGridView(dataGridView2, strSelecionadas[i]) == "Ordinais")
                                        {
                                            mDadosOrdinais[j, iDummyOrdinais] = double.NaN;
                                            iDummyOrdinais++;
                                        }
                                        //mDados[j, i] = double.NaN;
                                    }
                                }
                            }

                            int[] iVetor = new int[mDados.GetLength(0)];
                            if (shapeAlex.TipoVizinhanca == "")
                            {
                                //Modifica o label
                                label7.Text = "Gerando a matriz de vizinhanças...";
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
                            
                            clsUtilTools clsUtil = new clsUtilTools();

                            //Guarda o Shape
                            clsAlexCluster.EstruturaShape = shapeAlex;

                            //Modifica o label
                            label7.Text = "Gerando conglomerados espaciais...";
                            Application.DoEvents();

                            //Define o tipo de variável dos dados
                            switch (this.cmbTipoVariaveis.SelectedItem.ToString())
                            {
                                case "Contínuas":
                                    clsAlexCluster.TipoDadosClusterizacao = TipoDadosClusterizacao.Continuos;
                                    break;
                                case "Binários":
                                    clsAlexCluster.TipoDadosClusterizacao = TipoDadosClusterizacao.Binarios;
                                    break;
                                case "Ordinais":
                                    clsAlexCluster.TipoDadosClusterizacao = TipoDadosClusterizacao.Ordinais;
                                    break;
                                case "Categóricos":
                                    clsAlexCluster.TipoDadosClusterizacao = TipoDadosClusterizacao.Categoricos;
                                    break;
                                default:
                                    clsAlexCluster.TipoDadosClusterizacao = TipoDadosClusterizacao.Mistos;
                                    break;
                            }

                            //Define o método de dissimilaridade
                            switch (this.cmbMetodo.SelectedItem.ToString())
                            {
                                case "Ward":
                                    clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.Ward;
                                    break;
                                case "Sigle Linkage":
                                    clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.SingleLinkage;
                                    break;
                                case "Complete Linkage":
                                    clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.CompleteLinkage;
                                    break;
                                case "Average Linkage":
                                    clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.AverageLinkage;
                                    break;
                                case "Average Linkage (Weighted)":
                                    clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.AverageLinkageWeigthed;
                                    break;
                                case "Median":
                                    clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.Median;
                                    break;
                                case "Centroid":
                                    clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.Centroid;
                                    break;
                                default:
                                    clsAlexCluster.TipoMetodoClusterizacao = MetodoClusterizacao.Ward;
                                    break;
                            }

                            //Entrada das matrizes de dados
                            switch (clsAlexCluster.TipoDadosClusterizacao)
                            {
                                case TipoDadosClusterizacao.Continuos:
                                    clsAlexCluster.DadosClusterizacao = clsUtil.ArrayDoubleClone(mDadosContinuos);
                                    //mDadosContinuos = clsUtil.ArrayDoubleClone(mDados);
                                    clsAlexCluster.DadosContinuosClusterizacao = clsUtil.ArrayDoubleClone(mDadosContinuos);
                                    break;
                                case TipoDadosClusterizacao.Binarios:
                                    clsAlexCluster.DadosClusterizacao = clsUtil.ArrayDoubleClone(mDadosBinarios);
                                    //mDadosBinarios = clsUtil.ArrayDoubleClone(mDados);
                                    clsAlexCluster.DadosBinariosClusterizacao = clsUtil.ArrayDoubleClone(mDadosBinarios);
                                    break;
                                case TipoDadosClusterizacao.Categoricos:
                                    clsAlexCluster.DadosClusterizacao = clsUtil.ArrayDoubleClone(mDadosCategoricos);
                                    //mDadosCategoricos = clsUtil.ArrayDoubleClone(mDados);
                                    clsAlexCluster.DadosCategoricosClusterizacao = clsUtil.ArrayDoubleClone(mDadosCategoricos);
                                    break;
                                case TipoDadosClusterizacao.Ordinais:
                                    clsAlexCluster.DadosClusterizacao = clsUtil.ArrayDoubleClone(mDadosOrdinais);
                                    //mDadosOrdinais = clsUtil.ArrayDoubleClone(mDados);
                                    clsAlexCluster.DadosCategoricosClusterizacao = clsUtil.ArrayDoubleClone(mDadosOrdinais);
                                    break;
                                default:
                                    clsAlexCluster.DadosContinuosClusterizacao = mDadosContinuos;
                                    clsAlexCluster.DadosBinariosClusterizacao = mDadosBinarios;
                                    clsAlexCluster.DadosCategoricosClusterizacao = mDadosBinarios;
                                    clsAlexCluster.PesoDistanciaBinariaDadosMistos = (double)numPesoBinaria.Value;
                                    clsAlexCluster.PesoDistanciaCategoricaDadosMistos = (double)numPesoCategorica.Value;
                                    break;
                            }

                            //Define a distância para variáveis continúas
                            if (clsAlexCluster.TipoDadosClusterizacao == TipoDadosClusterizacao.Continuos
                                    || clsAlexCluster.TipoDadosClusterizacao == TipoDadosClusterizacao.Mistos)
                            {
                                switch (cmbDistancia.SelectedItem.ToString())
                                {
                                    case "Variance Corrected":
                                        clsAlexCluster.TipoDistanciaContinua = TipoDistanciaContinua.VarianceCorrected;
                                        break;
                                    case "Mahalanobis":
                                        clsAlexCluster.TipoDistanciaContinua = TipoDistanciaContinua.CovarianceCorrected;
                                        break;
                                    case "Manhattan":
                                        clsAlexCluster.TipoDistanciaContinua = TipoDistanciaContinua.L1Norm;
                                        break;
                                    case "Minkowsky":
                                        clsAlexCluster.TipoDistanciaContinua = TipoDistanciaContinua.LpNorm;
                                        clsAlexCluster.ParameterLpNorm = (double)numMinkowsky.Value;
                                        break;
                                    default:
                                        clsAlexCluster.TipoDistanciaContinua = TipoDistanciaContinua.Euclidiana;
                                        break;
                                }
                            }

                            //Define a distância para variáveis binárias
                            if (clsAlexCluster.TipoDadosClusterizacao == TipoDadosClusterizacao.Binarios
                                    || clsAlexCluster.TipoDadosClusterizacao == TipoDadosClusterizacao.Mistos)
                            {
                                switch (this.cmbDistanciaBinaria.SelectedItem.ToString())
                                {
                                    case "Dice":
                                        clsAlexCluster.TipoDistanciaBinaria = TipoDistanciaBinaria.Dice;
                                        break;
                                    case "Jaccard":
                                        clsAlexCluster.TipoDistanciaBinaria = TipoDistanciaBinaria.Jaccard;
                                        break;
                                    case "Kulczynski":
                                        clsAlexCluster.TipoDistanciaBinaria = TipoDistanciaBinaria.Kulczynski;
                                        break;
                                    case "RusselRao":
                                        clsAlexCluster.TipoDistanciaBinaria = TipoDistanciaBinaria.RusselRao;
                                        break;
                                    case "SimpleMatching":
                                        clsAlexCluster.TipoDistanciaBinaria = TipoDistanciaBinaria.SimpleMatching;
                                        break;
                                    default:
                                        clsAlexCluster.TipoDistanciaBinaria = TipoDistanciaBinaria.Tanimoto;
                                        break;
                                }
                            }

                            //Define a distância para variáveis categóricas
                            if (clsAlexCluster.TipoDadosClusterizacao == TipoDadosClusterizacao.Categoricos
                                    || clsAlexCluster.TipoDadosClusterizacao == TipoDadosClusterizacao.Mistos)
                            {
                                switch (this.cmbDistanciaCategorica.SelectedItem.ToString())
                                {
                                    case "SimpleMatching":
                                        clsAlexCluster.TipoDistanciaCategorica = TipoDistanciaCategorica.SimpleMatching;
                                        break;
                                    default:
                                        clsAlexCluster.TipoDistanciaCategorica = TipoDistanciaCategorica.SimpleMatching;
                                        break;
                                }
                            }

                            //Números máximo, mínimo de clusters na árvore 
                            clsAlexCluster.NumMaxClustersArvore = (int)numMaxClusters.Value;
                            clsAlexCluster.NumMinClustersArvore = (int)numMinClusters.Value;

                            //Restrições para os tamanhos máximos dos clusters
                            clsAlexCluster.LimitaTamanhoMaximoFreq = false;
                            clsAlexCluster.LimitaTamanhoMaximoPerc = false;

                            if (this.radioButton2.Checked)
                            {
                                clsAlexCluster.LimitaTamanhoMaximoFreq = true;
                                clsAlexCluster.TamanhoMaximoClusters = (int)this.numMaxPolygon.Value;
                            }

                            if (this.radioButton3.Checked)
                            {
                                clsAlexCluster.LimitaTamanhoMaximoPerc = true;
                                clsAlexCluster.PercentualMaximoClusters = (double)this.percMaxPolygon.Value;
                            }

                            //Faz a análise de conglomerados
                            clsAlexCluster.GeraArvoreContinuousHierarchicalClustering(ref progressBar1, ref label7);

                            //Coluna com os dados dos conglomerados
                            int coluna = clsAlexCluster.NumMaxClustersArvore - (int)numCluster.Value;

                            //Criando datatable a ser exportado
                            DataTable tabela = new DataTable();

                            //Guarda o clusterTree no dataGridView
                            PopulaDataGridViewTree(ref dataGridView1, clsAlexCluster.ClusterTree, clsAlexCluster.NumMaxClustersArvore, ref tabela);
                            dataGridView1.Refresh();
                            tabela_congl = tabela;

                            //Converte para a forma de vetor
                            iVetor = clsUtil.ConverteClusterTree(clsAlexCluster.ClusterTree, coluna);

                            //Guarda os conglomerados
                            classePoligonos = iVetor;

                            iMatriz = new double [iVetor.Length,1];
                            for (int i = 0; i < iVetor.Length; i++)
                            {
                                iMatriz[i, 0] = iVetor[i];
                            }
                            clsUtilTools clsUtil1 = new clsUtilTools();
                            tabcluster = new double[0, 0];

                            clsUtil1.FrequencyTable(ref tabcluster, iMatriz);

                            #region Métodos para a escolha do número de conglomerados
                            //Modifica o label
                            label7.Text = "Gerando dados para a escolha do número de conglomerados...";
                            Application.DoEvents();

                            //CCC
                            dblCCC = clsAlexCluster.SequenciaCCC;

                            //Pseudo T
                            dblPseudoT = clsAlexCluster.SequenciaPseudoT;

                            //Pseudo F
                            dblPseudoF = clsAlexCluster.SequenciaPseudoF;

                            //R-Square
                            dblRSquare = clsAlexCluster.SequenciaR2;

                            //R-Square Partial
                            dblRSquarePartial = clsAlexCluster.SequenciaPartialR2;

                            //R-Square Expected
                            dblRSquareExpected = clsAlexCluster.SequenciaExpectedR2;

                            #region Gera Gráficos

                            // Get a reference to the GraphPane
                            GraphPane myPane = new GraphPane(new RectangleF(0, 0, 1059, 513), "Escolha do tamanho ótimo do número de conglomerados.", "Número de conglomerados", "Pseudo T");

                            // Set the titles and axis labels
                            myPane.Title.Text = "Escolha do tamanho ótimo do número de conglomerados.";
                            myPane.XAxis.Title.Text = "Número de conglomerados";
                            myPane.YAxis.Title.Text = "Pseudo T";
                            myPane.Y2Axis.Title.Text = "R-Square";

                            //List of points
                            PointPairList ptList = new PointPairList();
                            PointPairList rsList = new PointPairList();
                            PointPairList pfList = new PointPairList();
                            PointPairList cccList = new PointPairList();

                            // Fabricate some data values
                            int iObs = Convert.ToInt32(numCluster.Value) * 2;
                            int iTotal = dblRSquare.Length - 1;
                            //for (int i = clsAlexCluster.NumMinClustersArvore; i <= clsAlexCluster.NumMaxClustersArvore /*iObs*/; i++)
                            //{
                            //    rsList.Add(i, dblRSquare[iTotal]);
                            //    ptList.Add(i, dblPseudoT[iTotal]);
                            //    cccList.Add(i, dblCCC[iTotal]);
                            //    pfList.Add(i, dblPseudoF[iTotal]);
                            //    iTotal--;
                            //}
                            for (int i = 0; i < dblRSquare.Length /*iObs*/; i++)
                            {
                                rsList.Add(i + 2, dblRSquare[iTotal]);
                                ptList.Add(i + 2, dblPseudoT[iTotal]);
                                cccList.Add(i + 2, dblCCC[iTotal]);
                                pfList.Add(i + 2, dblPseudoF[iTotal]);
                                iTotal--;
                            }

                            // Generate a red curve with diamond symbols, and "PseudoT" in the legend
                            LineItem myCurve = myPane.AddCurve("Pseudo T", ptList, Color.Red, SymbolType.Diamond);
                            // Fill the symbols with white
                            myCurve.Symbol.Fill = new Fill(Color.White);

                            // Generate a blue curve with circle symbols, and "RSquare" in the legend
                            myCurve = myPane.AddCurve("R-Square", rsList, Color.Blue, SymbolType.Circle);
                            // Fill the symbols with white
                            myCurve.Symbol.Fill = new Fill(Color.White);
                            // Associate this curve with the Y2 axis
                            myCurve.IsY2Axis = true;

                            // Generate a green curve with square symbols, and "PseudoF" in the legend
                            myCurve = myPane.AddCurve("Pseudo F", pfList, Color.Green, SymbolType.Square);
                            // Fill the symbols with white
                            myCurve.Symbol.Fill = new Fill(Color.White);
                            // Associate this curve with the second Y axis
                            myCurve.YAxisIndex = 1;

                            // Generate a Black curve with triangle symbols, and "CCC" in the legend
                            myCurve = myPane.AddCurve("CCC", cccList, Color.Black, SymbolType.Triangle);
                            // Fill the symbols with white
                            myCurve.Symbol.Fill = new Fill(Color.White);
                            // Associate this curve with the Y2 axis
                            myCurve.IsY2Axis = true;
                            // Associate this curve with the second Y2 axis
                            myCurve.YAxisIndex = 1;

                            // Show the x axis grid
                            myPane.XAxis.MajorGrid.IsVisible = true;

                            // Make the Y axis scale red
                            myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
                            myPane.YAxis.Title.FontSpec.FontColor = Color.Red;
                            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
                            myPane.YAxis.MajorTic.IsOpposite = false;
                            myPane.YAxis.MinorTic.IsOpposite = false;
                            // Don't display the Y zero line
                            myPane.YAxis.MajorGrid.IsZeroLine = false;

                            // Enable the Y2 axis display
                            myPane.Y2Axis.IsVisible = true;
                            // Make the Y2 axis scale blue
                            myPane.Y2Axis.Scale.FontSpec.FontColor = Color.Blue;
                            myPane.Y2Axis.Title.FontSpec.FontColor = Color.Blue;
                            // turn off the opposite tics so the Y2 tics don't show up on the Y axis
                            myPane.Y2Axis.MajorTic.IsOpposite = false;
                            myPane.Y2Axis.MinorTic.IsOpposite = false;
                            // Display the Y2 axis grid lines
                            myPane.Y2Axis.MajorGrid.IsVisible = true;
                            // Align the Y2 axis labels so they are flush to the axis
                            myPane.Y2Axis.Scale.Align = AlignP.Inside;

                            // Create a second Y Axis, green
                            YAxis yAxis3 = new YAxis("Pseudo F");
                            myPane.YAxisList.Add(yAxis3);
                            yAxis3.Scale.FontSpec.FontColor = Color.Green;
                            yAxis3.Title.FontSpec.FontColor = Color.Green;
                            yAxis3.Color = Color.Green;
                            // turn off the opposite tics so the Y2 tics don't show up on the Y axis
                            yAxis3.MajorTic.IsInside = false;
                            yAxis3.MinorTic.IsInside = false;
                            yAxis3.MajorTic.IsOpposite = false;
                            yAxis3.MinorTic.IsOpposite = false;
                            // Align the Y2 axis labels so they are flush to the axis
                            yAxis3.Scale.Align = AlignP.Inside;

                            Y2Axis yAxis4 = new Y2Axis("CCC");
                            yAxis4.IsVisible = true;
                            myPane.Y2AxisList.Add(yAxis4);
                            // turn off the opposite tics so the Y2 tics don't show up on the Y axis
                            yAxis4.MajorTic.IsInside = false;
                            yAxis4.MinorTic.IsInside = false;
                            yAxis4.MajorTic.IsOpposite = false;
                            yAxis4.MinorTic.IsOpposite = false;
                            // Align the Y2 axis labels so they are flush to the axis
                            yAxis4.Scale.Align = AlignP.Inside;

                            // Fill the axis background with a gradient
                            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);

                            myPane.AxisChange();

                            myPane.Border.IsVisible = false;

                            //Passa o gráfico
                            zedGraphControl1.GraphPane = myPane;
                            zedGraphControl1.Refresh();



                            #endregion


                            #endregion

                            label7.Text = "";
                            progressBar1.Visible = false;
                        }

                        #endregion


                        /*string out_text = "==============================================================================================================\n\n";
                        out_text += "Resultado do Cluster Espacial Hierarquico  \n\n";
                        out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                        out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
                        out_text += "Número de Observações:" + dTable.Rows.Count.ToString();



                        userControlRichTextOutput1.Texto = out_text;*/

                        this.Cursor = Cursors.Default;
                        Application.DoEvents();
                    }
                    catch (Exception er)
                    {
                        btnOK.Enabled = false;
                        this.Cursor = Cursors.Default;
                        label7.Text = "";
                        progressBar1.Visible = false;
                        MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Application.DoEvents();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int[] iVetor = new int[shapeAlex.Count];
                int coluna = clsAlexCluster.NumMaxClustersArvore - (int)numCluster.Value;
                clsUtilTools clsUtil = new clsUtilTools();

                //Converte para a forma de vetor
                iVetor = clsUtil.ConverteClusterTree(clsAlexCluster.ClusterTree, coluna);

                //Guarda os conglomerados
                classePoligonos = iVetor;
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private frmMapa mainForm = null;
        public frmCluster (Form callingForm)
        {
            mainForm = callingForm as frmMapa; 
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                
                this.Cursor = Cursors.WaitCursor;
                this.GerarCongromerados();
                this.btnRedefinirClusters.Enabled = true;
                //this.btnOK.Enabled = true;
                this.Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chkAleatorio_CheckedChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (chkAleatorio.Checked) cmbCores.Enabled = false;
                else cmbCores.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable dsTemp = (DataTable)dataGridView1.DataSource;
                DataTable dsTemp = tabela_congl_;
                //dsTemp.Tables[0].Columns.Remove("Mapa"+strIDmapa);
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = "C:\\";
                saveFileDialog1.Filter = "Excel (*.xls)|*.xls|Access (*.mdb)|*.mdb|XML (*.xml)|*.xml";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                    string strFile = saveFileDialog1.FileName;

                    ExportData exporta = new ExportData();

                    if (strExtensao == ".XLS")
                    {
                        exporta.exportToExcel(dsTemp, strFile);
                    }
                    else if (strExtensao == ".XML")
                    {
                        dsTemp.WriteXml(strFile);
                    }
                    else if (strExtensao == ".MDB")
                    {
                        //Cria o arquivo MDB
                        exporta.exportaToAccess(dsTemp, strFile, this.Name);
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        
        private void dataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            { 
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void selecionaTudoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < strVariaveis.Length; i++)
                {
                    dataGridView2.Rows[i].Cells[2].Value = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void retiraSeleçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < strVariaveis.Length; i++)
                {
                    dataGridView2.Rows[i].Cells[2].Value = false;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {           
        }

        private void frmCluster_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                MatrizArquivo.LimpaMatrizesArquivosTemporarios();
            }
            catch(Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
