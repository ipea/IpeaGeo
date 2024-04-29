using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using IpeaGeo.Forms;
using IpeaGeo.Modelagem;
using IpeaGeo.RegressoesEspaciais;
using SharpMap.Data;
using ZedGraph;
using SharpMap;

namespace IpeaGeo
{
    public partial class frmMapa : Form
    {      
        clsClusterizacaoEspacialHierarquica clsAlexCluster = new clsClusterizacaoEspacialHierarquica();
        
        public frmMapa()
        {
            InitializeComponent();
        }
        
        private ArrayList linhas_clicadas = new ArrayList();
        private Brush[] coresLocal;
        private int[] iVetor;

        #region Métodos para a escolha do número de conglomerados 

        private double[] dblPseudoT, dblPseudoF, dblRSquare, dblRSquarePartial,
                dblRSquareExpected, dblCCC;
        
        #endregion

        #region variáveis internas

        private clsIpeaShape shapeAlex;
             
        //Pasta com os dados
        public string strPastaDados;

        //Layer do mapa
        public SharpMap.Layers.VectorLayer mLayer;

        //Endereço do shapefile
        public string strEnderecoMapa;

        //Endereço da base
        public string strEnderecoBase;

        //Extensão da base
        public string strExtensao;

        //Variaveis no mapa
        public string[] strVariaveisMapa;
        
        //Declara o formulario PAI.
        private IPEAGEOMDIParent m_mdiparent;

        //Número de classes do mapa
        private double[] dblClasses;

        //Variaveis identificadoras
        private string strIDmapa;
        private string strIDbase;

        public string appPath = Path.GetDirectoryName(Application.ExecutablePath);

        private SharpMap.Map map;
        private DataTable m_tabela_shape;

        #endregion
        
        #region método load

        public bool HabilitaFuncoesBasesDadosIPEA
        {
            set
            {
                toolStripButton11.Enabled =
                    toolStripButton9.Enabled = value;
            }
        }

        private double m_zoom_value_extended = 10.0;

        private static bool lendo; 
        private void frmMapa_Load(object sender, EventArgs e)
        {
            try
            {
                propriedadesDosElementosSelecionadosToolStripMenuItem.Enabled = false;
                if (this.xpTabControl1.TabPages.Contains(tabPage1))
                    this.xpTabControl1.TabPages.Remove(tabPage1);
                
                if (this.xpTabControl2.TabPages.Contains(tabPage4))
                    this.xpTabControl2.TabPages.Remove(tabPage4);

                xpTabControl1.SelectedTab = tabPage5;

                toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
                adDados.ImageScaling = ToolStripItemImageScaling.SizeToFit;

                toolStripMapasTematicos.Enabled = false;

                mapaTematico.Visible = false;

                map = new SharpMap.Map(mapImage1.Size);
                map.BackColor = Color.White;
                mapImage1.Map = map;
                
                //Parametros tooltip
                toolTip1.AutoPopDelay = 5000;
                toolTip1.InitialDelay = 1000;
                toolTip1.ReshowDelay = 500;

                //Cursor de espera
                Cursor.Current = Cursors.WaitCursor;

                clsMapa classeMapa = new clsMapa();
                Hashtable[] ht_caracteristicas = new Hashtable[0];
                Classes.clsArmazenamentoDados salvar = new Classes.clsArmazenamentoDados();

                string tipo_geometria = "";
                classeMapa.loadingMapa(ref mLayer, ref mapImage1, ref map, strEnderecoMapa, this.Name, ref shapeAlex, ref ht_caracteristicas, ref tipo_geometria);
                if (salvar.Leitura_efetuada == true)
                {
                    m_zoom_value_extended = salvar.zoom_m;
                    lendo = true;
                }
                else
                {
                    m_zoom_value_extended = this.map.Zoom; 
                }
                this.m_tabela_shape = classeMapa.TabelaDados;

                m_variaveis_dados_layers.Add(this.Name, m_tabela_shape);
                m_caracteristicas_itens_layers.Add(this.Name, ht_caracteristicas);
                m_poligonos_selecionados.Add(this.Name, new ArrayList());
                m_shape_Alex_layers.Add(this.Name, shapeAlex);
                m_path_layers_importados.Add(this.Name, strEnderecoMapa);
                m_tipos_geometrias_layers_importados.Add(this.Name, tipo_geometria);

                if (salvar.Leitura_efetuada == true)
                {
                    mapImage1.Map.Zoom = salvar.zoom_m;
                }
                else
                {
                    this.mapImage1.Map.ZoomToExtents();
                }
                this.mapImage1.Refresh();       

                //Cursor padrão
                Cursor.Current = Cursors.Default;

                AtivarListaTabelasDados();

                //Adicionando um shape à lista de shapes
                TreeNode novo_node = new TreeNode();
                novo_node.Name = this.Name;
                novo_node.Text = this.Name + " (principal)";
                treeView1.Nodes.Add(novo_node);
                treeView1.Nodes[this.Name].Checked = true;
                toolStripButton11.Enabled = true;
                m_nome_top_layer = this.Name;
            }
            catch (Exception er)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region manipulação de listviews e split containers

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            try
            {
                this.mapImage1.Map.ZoomToExtents();
                this.mapImage1.Refresh();
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region concatenção de dados no shape

        private DataSet m_ds_dados_painel_espacial = new DataSet();
        private DataSet m_ds_dados_originais_painei_espacial = new DataSet();
        private string m_variavel_periodos_painel = "";
        private string m_variavel_unidades_painel = "";
        private object[,] m_freqs_periodos_painel_espacial = new object[0, 0];
        private bool m_usa_dados_painel_espacial = false;
        private string m_periodo_painel_espacial = "";
        private bool m_atualiza_list_box_painel_espacial = false;

        private static string[] dataFiles;
        public string[] datafiles_lido = dataFiles; //= new string[1];

        private void ConcatenarDadosToShape(bool painel_espacial)
        {
            try
            {
                m_mdiparent = (IpeaGeo.IPEAGEOMDIParent)this.MdiParent;

                bool erro = false;

                string FileName = "";
                FormConcatenacaoTabelaToShape frm = new FormConcatenacaoTabelaToShape();
                frm.TabelaShape = this.m_tabela_shape;
                frm.Shape.ConvertFromIpeaGEOShape(this.shapeAlex);
                frm.EnderecoMapa = strEnderecoMapa;
                frm.UsaDadosPainelEspacial = painel_espacial;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    //IPEAGEOMDIParent parent = new IPEAGEOMDIParent();
                    //parent.ClearXML();

                    if (frm.UsaDadosPainelEspacial)
                    {
                        if (!this.xpTabControl1.TabPages.Contains(tabPage1))
                            this.xpTabControl1.TabPages.Add(tabPage1);

                        m_ds_dados_painel_espacial = frm.DsDadosPainelEspacial;
                        m_freqs_periodos_painel_espacial = frm.FrequenciaPeriodosPainel;
                        m_variavel_periodos_painel = frm.VariavelPeriodosPainel;
                        m_variavel_unidades_painel = frm.VariavelUnidadesPainel;

                        grbPeriodosPainelEspacial.Text = "Períodos do painel espacial [" + m_variavel_periodos_painel + "]";
                        listBoxPeriodosPainelEspacial.Items.Clear();
                        string[] periodos = new string[m_freqs_periodos_painel_espacial.GetLength(0)];
                        for (int i = 0; i < periodos.GetLength(0); i++) periodos[i] = m_freqs_periodos_painel_espacial[i, 0].ToString();

                        m_atualiza_list_box_painel_espacial = false;
                        listBoxPeriodosPainelEspacial.Items.AddRange(periodos);
                        listBoxPeriodosPainelEspacial.SelectedIndex = 0;
                        m_atualiza_list_box_painel_espacial = true;

                        txtPainelPeriodoApresentado.Text = Convert.ToString(m_freqs_periodos_painel_espacial[0, 0]);
                        m_periodo_painel_espacial = Convert.ToString(m_freqs_periodos_painel_espacial[0, 0]);

                        m_ds_dados_originais_painei_espacial = new DataSet();
                        for (int i = 0; i < m_ds_dados_painel_espacial.Tables.Count; i++)
                            m_ds_dados_originais_painei_espacial.Tables.Add(((DataTable)m_ds_dados_painel_espacial.Tables[i]).Copy());

                        regressãoComDadosDePainelToolStripMenuItem.Enabled = true;
                        dadosDePainelToolStripMenuItem1.Enabled = true;
                    }
                    else if (this.xpTabControl1.TabPages.Contains(tabPage1))
                        this.xpTabControl1.TabPages.Remove(tabPage1);

                    FileName = frm.FileName;

                    dataFiles = new string[1];
                    datafiles_lido = new string[1];
                    dataFiles[0] = Path.GetFileName(FileName);
                    datafiles_lido[0] = dataFiles[0];

                    //Guarda o endereço da base
                    strEnderecoBase = FileName;

                    //Extensão
                    strExtensao = Path.GetExtension(FileName).ToUpper();

                    Cursor.Current = Cursors.WaitCursor;

                    //Habilita a exportação
                    toolExportaDados.Enabled = true;
                    toolStripButton10.Enabled = true;
                    toolRefresh.Enabled = true;

                    //Guarda a base de dados conectada
                    dsDados.Tables.Clear();
                    dsDados.Tables.Add(frm.TabelaDadosConcatenados);

                    m_tabela_dados_original = frm.TabelaDadosConcatenados.Copy();

                    int row_poligono = 0;
                    for (int i = 0; i < m_tabela_dados_original.Rows.Count; i++)
                    {
                        row_poligono = Convert.ToInt32(m_tabela_dados_original.Rows[i]["Mapa" + frm.IDmapa]);
                        shapeAlex[row_poligono].ID = m_tabela_dados_original.Rows[i]["shape_" + frm.IDmapa].ToString();
                        shapeAlex[row_poligono].PosicaoNoDataTable = i;
                    }

                    //Guarda as variaveis que existem no mapa
                    strVariaveisMapa = frm.VariaveisNoShape;

                    //Guarda ID mapa
                    strIDmapa = frm.IDmapa;

                    //Guarda ID Base
                    strIDbase = frm.IDbase;

                    //Habilita as funções que só funcionam após o join
                    toolStripButton7.Enabled = true;
                    toolStripButton6.Enabled = true;
                    toolStripButton8.Enabled = true;

                    //Habilita o botão "saveToolStripButton" para salvar o trabalho após o join
                    saveToolStripButton.Enabled = true;

                    segundavez = true;

                    //Habilita os botões de análise
                    ToolspatialEstat.Enabled = true;
                    toolStripCalculadora.Enabled = true;

                    mapaTematico.Enabled = true;
                    this.toolStripMapasTematicos.Enabled = true;
                    tlInformacao.Enabled = true;

                    //Guarda as bases contidas na pasta de interesse
                    FileInfo finfo1 = new FileInfo(FileName);
                    strPastaDados = finfo1.DirectoryName;
                    int dummy = 1;
                    foreach (string fileName in Directory.GetFiles(strPastaDados, "*.*", SearchOption.TopDirectoryOnly))
                    {
                        FileInfo finfo = new FileInfo(fileName);
                        string strExt = finfo.Extension.ToLower();
                        if ((strExt == ".mdb" || strExt == ".xls" || strExt == ".sas7bdat" || strExt == ".txt" || strExt == ".dat"))
                            dummy++;
                    }

                    //Se a base tiver missings, mostrar ao usuário a opção de fechar ou aceitar zeros
                    bool aviso = false;
                    for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < dsDados.Tables[0].Columns.Count; j++)
                            if (dsDados.Tables[0].Rows[i][j].ToString() == "" || dsDados.Tables[0].Rows[i][j].ToString() == "." || dsDados.Tables[0].Rows[i][j].ToString() == " ")
                            {
                                aviso = true;
                                break;
                            }
                        
                        if (aviso) break;
                    }

                    //Guarda o dataTable
                    dataGridView1.DataSource = dsDados.Tables[0];
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.Refresh();

                    //Lista as variáveis
                    listBox1.Items.Clear();
                    for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++) listBox1.Items.Add(dsDados.Tables[0].Columns[i].ToString());

                    //Tab para os períodos do painel espacial
                    if (frm.UsaDadosPainelEspacial)
                    {
                        if (!xpTabControl1.TabPages.Contains(tabPage1))
                            xpTabControl1.TabPages.Add(tabPage1);
                        
                        xpTabControl1.SelectedTab = tabPage1;
                    }
                    else if (xpTabControl1.TabPages.Contains(tabPage1))
                            xpTabControl1.TabPages.Remove(tabPage1);

                    Cursor.Current = Cursors.Default;

                    if (!this.xpTabControl2.TabPages.Contains(tabPage4))
                        this.xpTabControl2.TabPages.Add(tabPage4);

                    m_usa_dados_painel_espacial = painel_espacial;

                    Cursor = Cursors.WaitCursor;
                    LimpaMapaTematico(true);
                    Cursor = Cursors.Default;
                    this.txtVarMapaTematico.Text = " ";

                    //sinaliza alteração no mapa temático
                    mapa_alterado = true;
                }
                else if (erro == false)
                {
                    Application.DoEvents();

                    #region Copia_do_Load

                    //ImageList
                    System.Windows.Forms.ImageList imagesLarge1 = new ImageList();

                    imagesLarge1.ImageSize = new Size(45, 45);

                    //ImageList2
                    System.Windows.Forms.ImageList imagesSmall1 = new ImageList();

                    //Adicio
                    imagesSmall1.ImageSize = new Size(20, 20);

                    #endregion;
                }

                this.AtivarListaTabelasDados();
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void adDados_Click_1(object sender, EventArgs e)
        {
            label1.Visible = false;
            try
            {
                ConcatenarDadosToShape(false);
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            try
            {
                ConcatenarDadosToShape(true);
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region manipulação dos toostrips 

        private DataTable m_dt_tabela_shape = new DataTable();

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            try
            {
                frmMapa mapa = new frmMapa();

                string FileName = this.strEnderecoMapa;
                Cursor.Current = Cursors.WaitCursor;

                mapa.Text = Path.GetFileNameWithoutExtension(FileName);
                mapa.Name = Path.GetFileNameWithoutExtension(FileName);
                mapa.MdiParent = this.MdiParent;
                mapa.strEnderecoMapa = FileName;

                //Coloca as variaveis do mapa
                clsMapa clsMapa = new clsMapa();
                mapa.strVariaveisMapa = clsMapa.informacaoVariaveis(FileName, 0);
                mapa.Show();

                m_dt_tabela_shape = clsMapa.TabelaDados;
                Cursor.Current = Cursors.Default;    
                this.Close();

                //sinaliza alteração no mapa temático
                mapa_alterado = true;
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
            }
        }
        
        private void ExportaDados()
        {
            try
            {
                ExportData ed = new ExportData();
                ed.ExportarDados(dataGridView1, this.Name);
                //sinaliza alteração no mapa temático
                if (ed.fechamento == true)
                {
                    mapa_alterado = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolExportaDados_Click_1(object sender, EventArgs e)
        {
            label1.Visible = false;
            try
            {
                salvamento = salvamento_efetuado = false;
                this.ExportaDados();
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void moveMapa_Click(object sender, EventArgs e)
        {
            xpTabControl2.SelectTab(tabPage3);
            label1.Visible = false;
            try
            {
                tlInformacao.Checked = false;
                toolStripButton4.Checked = false;
                toolStripButton3.Checked = false;
                toolStripButton7.Checked = false;
                toolStripButton2.Checked = false;
                toolStripButton5.Checked = false;
                toolStripButton6.Checked = false;
                toolStripButton8.Checked = false;

                mapImage1.ActiveTool = (moveMapa.Checked) ? SharpMap.Forms.MapImage.Tools.Pan : SharpMap.Forms.MapImage.Tools.None;
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            xpTabControl2.SelectTab(tabPage3);
            label1.Visible = false;
            try
            {
                tlInformacao.Checked = false;
                moveMapa.Checked = false;
                toolStripButton2.Checked = false;
                toolStripButton4.Checked = false;
                toolStripButton7.Checked = false;
                toolStripButton5.Checked = false;
                toolStripButton6.Checked = false;
                toolStripButton8.Checked = false;

                mapImage1.ActiveTool = (toolStripButton3.Checked) ? SharpMap.Forms.MapImage.Tools.ZoomIn : SharpMap.Forms.MapImage.Tools.None;

                //sinaliza alteração no mapa temático
                mapa_alterado = true;
            }
            catch (Exception)
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            xpTabControl2.SelectTab(tabPage3);
            label1.Visible = false;
            try
            {
                tlInformacao.Checked = false;
                moveMapa.Checked = false;
                toolStripButton3.Checked = false;
                toolStripButton6.Checked = false;
                toolStripButton2.Checked = false;
                toolStripButton7.Checked = false;
                toolStripButton5.Checked = false;
                toolStripButton8.Checked = false;

                mapImage1.ActiveTool = (toolStripButton4.Checked) ? SharpMap.Forms.MapImage.Tools.ZoomOut : SharpMap.Forms.MapImage.Tools.None;

                //sinaliza alteração no mapa temático
                mapa_alterado = true;
            }
            catch (Exception)
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            xpTabControl2.SelectTab(tabPage3);
            label1.Visible = false;
            try
            {
                this.mapImage1.Map.ZoomToExtents();
                this.mapImage1.Refresh();

                //sinaliza alteração no mapa temático
                mapa_alterado = true;
                escala();
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
            }
        }

        #endregion

        #region mapa temático

        private void mapaTematico_Click(object sender, EventArgs e)
        {
            try
            {
                //Formulário Pai
                m_mdiparent = (IpeaGeo.IPEAGEOMDIParent)this.MdiParent;

                //Abre a conexão
                frmMapaTematico frmMapa = new frmMapaTematico();

                //Aplica os Idenfificadores
                frmMapa.IdentificadorDados = strIDbase;
                frmMapa.IdentificadorMapa = strIDmapa;
                frmMapa.numeroClasses_ = shapeAlex.Count;

                //Aplica o DataTable
                DataTable dTable = dsDados.Tables[0];
                frmMapa.DataTableDados = dTable;

                //Abre o Dialog
                frmMapa.ShowDialog();

                if (frmMapa.DialogResult == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    //Captura o mapa
                    SharpMap.Map mMapa = mapImage1.Map;

                    //Captura o layer
                    SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[this.Name];

                    //Cria o vetor temático
                    int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                    //Guarda as classes
                    dblClasses = frmMapa.ClasseDoMapa;

                    //Guarda o vetor temático
                    iVetor = frmMapa.vetorPoligonos;

                    //Inicializa as cores
                    Brush[] cores = frmMapa.CoresParaMapa;                     

                    //Guarda os vetores
                    ArrayList vetorID = new ArrayList();
                    int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];

                    for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                    {
                        vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                        vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                    }                    

                    //Pinta o mapa
                    mTheme meuTema = new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice);
                    layMapa.Theme = new SharpMap.Rendering.Thematics.CustomTheme(meuTema.GetStyle);
                    if (frmMapa.GuardaClassificacao == true)
                    {
                        if (dsDados.Tables[0].Columns.Contains("MapaTematico") == false) dsDados.Tables[0].Columns.Add("MapaTematico");
                        for (int i = 0; i < iVetor.Length; i++)
                            dsDados.Tables[0].Rows[i]["MapaTematico"] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]];
                    }

                    //Refresh
                    mMapa.ZoomToExtents();

                    //Refresh o mapa
                    mapImage1.Refresh();

                    //Gera relatório
                    if (frmMapa.GeraRelatorio == true)
                    {
                        //this.Dock = DockStyle.Fill;
                        //this.Dock = DockStyle.None;

                        //Coloca o fundo branco
                        mapImage1.Map.BackColor = Color.White;

                        //Gera o mapa
                        System.Drawing.Bitmap img = new System.Drawing.Bitmap(mapImage1.Width, mapImage1.Height);
                        mapImage1.DrawToBitmap(img, mapImage1.ClientRectangle);
                         
                        //Salva o mapa
                        img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);
                        string[] legenda = frmMapa.Legenda_MapaTem;

                        clsReport classeReport = new clsReport();
                        string html = null;
                        if (legenda != null)
                        {
                            html = classeReport.MapaTematicoRelatorio(strEnderecoBase, strEnderecoMapa, 
                                Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count, frmMapa.Metodologia,
                                frmMapa.ClasseDoMapa, frmMapa.CoresRGB, frmMapa.Variavel, legenda);
                        }
                        else 
                        {
                            html = classeReport.MapaTematicoRelatorio(strEnderecoBase, strEnderecoMapa, 
                                Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count, frmMapa.Metodologia, 
                                frmMapa.ClasseDoMapa, frmMapa.CoresRGB, frmMapa.Variavel);
                        }

                        //Abre o relatório
                        frmRelatorio frmRelatorio = new frmRelatorio();
                        frmRelatorio.isThematicMap = true;
                        frmRelatorio.legendaMapaTematico = legenda;
                        frmRelatorio.enderecoBase = strEnderecoBase;
                        frmRelatorio.enderecoMapa = strEnderecoMapa;
                        frmRelatorio.endMapa = Application.StartupPath + "\\Mapa.jpeg";
                        frmRelatorio.shapeCount = shapeAlex.Count;
                        frmRelatorio.metodo = frmMapa.Metodologia;
                        frmRelatorio.classesMapaReg = frmMapa.ClasseDoMapa;
                        frmRelatorio.coresMapaReg = frmMapa.CoresRGB;
                        frmRelatorio.varMapa = frmMapa.Variavel;

                        frmRelatorio.MdiParent = this.MdiParent;
                        frmRelatorio.codigoHTML = html;
                        frmRelatorio.Text = "Relatório " + this.Text + ": " + frmMapa.Variavel;

                        string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count + 1, 2];
                        strClusterMapa[0, 0] = strIDbase;
                        strClusterMapa[0, 1] = "Classe";

                        for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                        {
                            string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                            string stCl = dsDados.Tables[0].Rows[i]["MapaTematico"].ToString();
                            strClusterMapa[i + 1, 0] = stId;
                            strClusterMapa[i + 1, 1] = stCl;
                        }

                        frmRelatorio.variaveisMapa = strClusterMapa;
                        frmRelatorio.Show();

                        //sinaliza alteração no mapa temático
                        mapa_alterado = true;
                    }

                    //Refresh
                    mMapa.ZoomToExtents();

                    //Refresh o mapa
                    mapImage1.Refresh();
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region índice de Moran

        private void índiceDeMoranToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Abre a conexão
                frmDependenciaGlobal frmDependenciaGlobal = new frmDependenciaGlobal();

                //Guarda a base de dados
                frmDependenciaGlobal.DataTableDados = (DataTable)dsDados.Tables[0];

                //Guarda o ID do mapa
                frmDependenciaGlobal.IdentificadorMapa = strIDmapa;

                //Guarda o ID da base
                frmDependenciaGlobal.IdentificadorDados = strIDbase;

                //Guarda o endereço do mapa
                frmDependenciaGlobal.EnderecoMapa = strEnderecoMapa;

                //Guarda a estrutura do shape
                frmDependenciaGlobal.EstruturaShape.ConvertFromIpeaGEOShape(shapeAlex);

                //Se matriz de vizinhança já especificada
                if (m_matriz_W_esparsa_existente)
                {
                    frmDependenciaGlobal.MatrizWPreDefinida = m_matriz_W_esparsa_existente;
                    frmDependenciaGlobal.MatrizWEsparsa = m_matriz_W_esparsa;
                    frmDependenciaGlobal.TipoMatrizVizinhanca = m_tipo_matriz_vizinhanca;
                    frmDependenciaGlobal.OrdemVizinhanca = m_ordem_matriz_vizinhanca;
                    frmDependenciaGlobal.MatrizWNormalizada = m_matriz_W_normalizada;
                }

                //Guarda as variáveis
                int intQuantitiativas = 0;
                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                {
                    //Guarda as informações
                    Type tipo = dsDados.Tables[0].Columns[i].DataType;
                    //Salva o tipo de interesse
                    string strTipo = tipo.ToString();

                    if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16" 
                        || strTipo == "System.Int64" || strTipo == "System.Double")
                    {
                        intQuantitiativas++;
                    }
                }
                string[] strVariaveisQuantitativas = new string[intQuantitiativas];
                string[] strVariaveisQualitativas = new string[dsDados.Tables[0].Columns.Count - intQuantitiativas];
                int dummyQuali = 0;
                int dummyQuanti = 0;

                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                {
                    //Guarda as informações
                    Type tipo = dsDados.Tables[0].Columns[i].DataType;
                    //Salva o tipo de interesse
                    string strTipo = tipo.ToString();

                    if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16" 
                        || strTipo == "System.Int64" || strTipo == "System.Double")
                    {
                        strVariaveisQuantitativas[dummyQuanti] = dsDados.Tables[0].Columns[i].ColumnName;
                        dummyQuanti++;
                    }
                    else
                    {
                        strVariaveisQualitativas[dummyQuali] = dsDados.Tables[0].Columns[i].ColumnName;
                        dummyQuali++;
                    }
                }

                frmDependenciaGlobal.VariaveisQualitativas = strVariaveisQualitativas;
                frmDependenciaGlobal.VariaveisQuantitativas = strVariaveisQuantitativas;

                //Abre o Dialog
                frmDependenciaGlobal.ShowDialog();

                if (frmDependenciaGlobal.DialogResult == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Application.DoEvents();
                     
                    #region Gera o Relatório

                    if (frmDependenciaGlobal.GeraRelatorio)
                    {
                        clsReport classeReport = new clsReport();
                        clsReport.DependenciaGlobal dg = new clsReport.DependenciaGlobal();

                        dg.strBase = strEnderecoBase; dg.strMapa = strEnderecoMapa;
                        dg.numPoligonos = shapeAlex.Count; dg.strTipoVizinhanca = shapeAlex.TipoVizinhanca;
                        dg.strVariaveisSelecionadasQuantitativas = frmDependenciaGlobal.VariaveisSelecionadasQuantitativas;
                        dg.strVariavelPopulacao = frmDependenciaGlobal.VariavelPopulacao;
                        dg.intNumeroDeSimulacoes = frmDependenciaGlobal.NumeroDeSimulacoes;
                        dg.strTipoDoPeso = frmDependenciaGlobal.TipoDoPeso;
                        dg.dblIndiceGeary = frmDependenciaGlobal.IndiceGeary;
                        dg.dblpValorIndiceGeary = frmDependenciaGlobal.pValorIndiceGeary;
                        dg.dblIndiceGetis = frmDependenciaGlobal.IndiceGetis;
                        dg.dblpValorIndiceGetis = frmDependenciaGlobal.pValorIndiceGetis;
                        dg.dblIndiceMoran = frmDependenciaGlobal.IndiceMoran;
                        dg.dblpValorIndiceMoran = frmDependenciaGlobal.pValorIndiceMoran;
                        dg.dblIndiceMoranSimples = frmDependenciaGlobal.IndiceMoranSimples;
                        dg.dblpValorIndiceMoranSimples = frmDependenciaGlobal.pValorIndiceMoranSimples;
                        dg.dblIndiceRogerson = frmDependenciaGlobal.IndiceRogerson;
                        dg.dblpValorIndiceRogerson = frmDependenciaGlobal.pValorIndiceRogerson;
                        dg.dblIndiceTango = frmDependenciaGlobal.IndiceTango;
                        dg.dblpValorIndiceTango = frmDependenciaGlobal.pValorIndiceTango;

                        string html = classeReport.IndicesDeDependenciaEspacialGlobais(dg);

                        //Abre o relatório
                        frmRelatorio frmRelatorio = new frmRelatorio();
                        frmRelatorio.MdiParent = this.MdiParent;
                        frmRelatorio.codigoHTML = html;
                        frmRelatorio.isGlobalDependence = true;
                        frmRelatorio.moran = frmDependenciaGlobal.IndiceMoran;
                        frmRelatorio.moranP = frmDependenciaGlobal.pValorIndiceMoran;
                        frmRelatorio.moranSimp = frmDependenciaGlobal.IndiceMoranSimples;
                        frmRelatorio.moranSimpP = frmDependenciaGlobal.pValorIndiceMoranSimples;
                        frmRelatorio.geary = frmDependenciaGlobal.IndiceGeary;
                        frmRelatorio.gearyP = frmDependenciaGlobal.pValorIndiceGeary;
                        frmRelatorio.getis = frmDependenciaGlobal.IndiceGetis;
                        frmRelatorio.getisP = frmDependenciaGlobal.pValorIndiceGetis;
                        frmRelatorio.rogerson = frmDependenciaGlobal.IndiceRogerson;
                        frmRelatorio.rogersonP = frmDependenciaGlobal.pValorIndiceRogerson;
                        frmRelatorio.tango = frmDependenciaGlobal.IndiceTango;
                        frmRelatorio.tangoP = frmDependenciaGlobal.pValorIndiceTango;

                        frmRelatorio.enderecoBase = strEnderecoBase;
                        frmRelatorio.enderecoMapa = strEnderecoMapa;
                        frmRelatorio.shapeCount = shapeAlex.Count;
                        frmRelatorio.tipoVizinhanca = shapeAlex.TipoVizinhanca;
                        frmRelatorio.tipoDePeso = frmDependenciaGlobal.TipoDoPeso;
                        frmRelatorio.variaveisSelecionadasQuant = frmDependenciaGlobal.VariaveisSelecionadasQuantitativas;
                        frmRelatorio.variavelPopulacao = frmDependenciaGlobal.VariavelPopulacao;
                        frmRelatorio.numeroSimulacoes = frmDependenciaGlobal.NumeroDeSimulacoes;

                        frmRelatorio.Text = "Relatório " + this.Text + ": Índices de dependência espacial globais";
                        frmRelatorio.Show();
                    }

                    #endregion

                    Cursor.Current = Cursors.Default;

                    //sinaliza alteração no mapa temático
                    mapa_alterado = true;
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region análise LISA

        private void lISAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Abre a conexão
                frmDependenciaLocal frmDependenciaLocal = new frmDependenciaLocal();

                //Guarda a base de dados
                frmDependenciaLocal.DataTableDados = dsDados.Tables[0];

                //Guarda o ID do mapa
                frmDependenciaLocal.IdentificadorMapa = strIDmapa;

                //Guarda o ID da base
                frmDependenciaLocal.IdentificadorDados = strIDbase;

                //Guarda o endereço do mapa
                frmDependenciaLocal.EnderecoMapa = strEnderecoMapa;

                //Guarda a estrutura do shape
                frmDependenciaLocal.EstruturaShape = shapeAlex;

                //Guarda as variáveis
                int intQuantitiativas = 0;
                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                {
                    //Guarda as informações
                    Type tipo = dsDados.Tables[0].Columns[i].DataType;
                    //Salva o tipo de interesse
                    string strTipo = tipo.ToString();

                    if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16"
                        || strTipo == "System.Int64" || strTipo == "System.Double")
                    {
                        intQuantitiativas++;
                    }
                }
                string[] strVariaveisQuantitativas = new string[intQuantitiativas];
                string[] strVariaveisQualitativas = new string[dsDados.Tables[0].Columns.Count - intQuantitiativas];
                int dummyQuali = 0;
                int dummyQuanti = 0;

                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++)
                {
                    //Guarda as informações
                    Type tipo = dsDados.Tables[0].Columns[i].DataType;
                    //Salva o tipo de interesse
                    string strTipo = tipo.ToString();

                    if (strTipo == "System.Byte" || strTipo == "System.Int32" || strTipo == "System.Int16"
                        || strTipo == "System.Int64" || strTipo == "System.Double")
                    {
                        strVariaveisQuantitativas[dummyQuanti] = dsDados.Tables[0].Columns[i].ColumnName;
                        dummyQuanti++;
                    }
                    else
                    {
                        strVariaveisQualitativas[dummyQuali] = dsDados.Tables[0].Columns[i].ColumnName;
                        dummyQuali++;
                    }
                }

                frmDependenciaLocal.VariaveisQualitativas = strVariaveisQualitativas;
                frmDependenciaLocal.VariaveisQuantitativas = strVariaveisQuantitativas;

                //Abre o Dialog
                frmDependenciaLocal.ShowDialog();

                if (frmDependenciaLocal.DialogResult == DialogResult.OK)
                {
                    //this.Dock = DockStyle.Fill;
                    //this.Dock = DockStyle.None;

                    Cursor.Current = Cursors.WaitCursor;
                    Application.DoEvents();

                    //Captura o mapa
                    SharpMap.Map mMapa = mapImage1.Map;

                    //Captura o layer
                    SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[this.Name];

                    //Cria o vetor temático
                    int[] iVetorLisa = new int[dsDados.Tables[0].Rows.Count];
                    int[] iVetorEscore = new int[dsDados.Tables[0].Rows.Count];
                    int[] iVetorGetis = new int[dsDados.Tables[0].Rows.Count];
                    int[] iVetorGetis2 = new int[dsDados.Tables[0].Rows.Count];
                    string[] iBaseMapa = new string[dsDados.Tables[0].Rows.Count];
                    for (int i = 0; i < shapeAlex.Count; i++)
                        iBaseMapa[i] = dsDados.Tables[0].Rows[shapeAlex[i].PosicaoNoDataTable][strIDbase].ToString();

                    ArrayList arEscore = frmDependenciaLocal.ESCOREmapa;
                    string[] strMapaEscore = new string[frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length];
                    ArrayList arLisa = frmDependenciaLocal.LISAmapa;
                    string[] strMapaLisa = new string[frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length];
                    ArrayList arGetis = frmDependenciaLocal.GETISmapa;
                    string[] strMapaGetis = new string[frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length];
                    ArrayList arGetis2 = frmDependenciaLocal.GETIS2mapa;
                    string[] strMapaGetis2 = new string[frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length];
                    ArrayList arEspalhamento = frmDependenciaLocal.ESPALHAMENTOmapa;
                    string[] strMapaEspalhamento = new string[frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length];

                    for (int v = 0; v < frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length; v++)
                    {
                        //Inicializa as cores
                        Brush[] cores = coresLocal = frmDependenciaLocal.CoresParaMapa;
                        if (v == frmDependenciaLocal.VariaveisSelecionadasQuantitativas.Length - 1)
                        {
                            m_cores_mapa_tematico = new Brush[cores.GetLength(0)];
                            for (int i = 0; i < m_cores_mapa_tematico.GetLength(0); i++) m_cores_mapa_tematico[i] = (Brush)cores[i];
                        }

                        if (arLisa.Count > 0)
                        {
                            iVetorLisa = (int[])arLisa[v];

                            //Retira a variável caso já exista
                            string nome = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v] + "_LM";
                            if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);

                            //Adiciona a variavel no mapa
                            dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                            //Guarda os vetores
                            ArrayList vetorID = new ArrayList();
                            int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];

                            Hashtable[] ht = new Hashtable[0];
                            ht = (Hashtable[])(m_caracteristicas_itens_layers[this.Name]);
                            
                            for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                            {
                                vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                                vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                                if (iVetorLisa[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0)
                                    dsDados.Tables[0].Rows[i][nome] = iVetorLisa[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();

                                // guardando os resultados no hashtable de características dos polígonos
                                (ht[i])["Fill"] = (Brush)cores[iVetorLisa[i]]; 
                            }
                            LayerPrincipalNoTopo();

                            //Pinta o mapa
                            iVetor = iVetorLisa;

                            SharpMap.Rendering.Thematics.CustomTheme iTheme =
                                new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetorLisa, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                            layMapa.Theme = iTheme;

                            //Refresh
                            mMapa.ZoomToExtents();

                            //Coloca o fundo branco
                            mapImage1.Map.BackColor = Color.White;

                            //Refresh o mapa
                            mapImage1.Refresh();

                            //this.Dock = DockStyle.Fill;
                            //this.Dock = DockStyle.None;
                            
                            //Gera o mapa
                            System.Drawing.Bitmap img = new System.Drawing.Bitmap(mapImage1.Width, mapImage1.Height);
                            mapImage1.DrawToBitmap(img, mapImage1.ClientRectangle);

                            //Nome do MAPA
                            strMapaLisa[v] = Application.StartupPath + "\\MapaLisa_" + v.ToString() + ".jpeg";

                            //Salva o mapa
                            img.Save(strMapaLisa[v], ImageFormat.Jpeg);

                            //Coloca a variável que está sendo representada no mapa
                            this.txtVarMapaTematico.Text = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v].ToString();

                            //Gera espalhamento de MORAN
                            GraphPane myPane = new GraphPane(new RectangleF(0, 0, 824, 631), "Gráfico de espalhamento de Moran.",
                                frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v],
                                "Média dos vizinhos " + frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v]);

                            // Enter some calculated data constants
                            PointPairList list1 = new PointPairList();
                            double[,] dblEspalha = (double[,])arEspalhamento[v];
                            for (int k = 0; k < dblEspalha.GetLength(0); k++)
                                list1.Add(dblEspalha[k, 0], dblEspalha[k, 1]);

                            // Generate a red curve with diamond symbols, and "Gas Data" in the legend
                            LineItem myCurve = myPane.AddCurve("Espalhamento de Moran", list1, Color.Red, SymbolType.Diamond);

                            myCurve.Symbol.Size = 12;
                            // Set up a red-blue color gradient to be used for the fill
                            myCurve.Symbol.Fill = new Fill(Color.Red, Color.Blue);
                            // Turn off the symbol borders
                            myCurve.Symbol.Border.IsVisible = false;
                            // Turn off the line, so the curve will by symbols only
                            myCurve.Line.IsVisible = false;

                            //Alto-Alto
                            TextObj text1 = new TextObj("Alto-Alto", 2.0, 2.0, CoordType.AxisXYScale);
                            text1.FontSpec.FontColor = Color.Black;
                            text1.FontSpec.Border.IsVisible = false;
                            text1.FontSpec.Fill.IsVisible = false;
                            text1.FontSpec.Size = 14;
                            myPane.GraphObjList.Add(text1);

                            //Alto-Baixo
                            TextObj text2 = new TextObj("Alto-Baixo", 2.0, -2.0, CoordType.AxisXYScale);
                            text2.FontSpec.FontColor = Color.Black;
                            text2.FontSpec.Border.IsVisible = false;
                            text2.FontSpec.Fill.IsVisible = false;
                            text2.FontSpec.Size = 14;
                            myPane.GraphObjList.Add(text2);

                            //Baixo-Alto
                            TextObj text3 = new TextObj("Baixo-Alto", -2.0, 2.0, CoordType.AxisXYScale);
                            text3.FontSpec.FontColor = Color.Black;
                            text3.FontSpec.Border.IsVisible = false;
                            text3.FontSpec.Fill.IsVisible = false;
                            text3.FontSpec.Size = 14;
                            myPane.GraphObjList.Add(text3);

                            //Baixo-Baixo
                            TextObj text4 = new TextObj("Baixo-Baixo", -2.0, -2.0, CoordType.AxisXYScale);
                            text4.FontSpec.FontColor = Color.Black;
                            text4.FontSpec.Border.IsVisible = false;
                            text4.FontSpec.Fill.IsVisible = false;
                            text4.FontSpec.Size = 14;
                            myPane.GraphObjList.Add(text4);

                            // Show the X and Y grids
                            myPane.XAxis.MajorGrid.IsVisible = true;
                            myPane.YAxis.MajorGrid.IsVisible = true;

                            // Set the x and y scale and title font sizes to 14
                            myPane.XAxis.Scale.FontSpec.Size = 14;
                            myPane.XAxis.Title.FontSpec.Size = 14;
                            myPane.YAxis.Scale.FontSpec.Size = 14;
                            myPane.YAxis.Title.FontSpec.Size = 14;
                            // Set the GraphPane title font size to 16
                            myPane.Title.FontSpec.Size = 16;
                            // Turn off the legend
                            myPane.Legend.IsVisible = false;

                            // Fill the axis background with a color gradient
                            myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 90F);
                            myPane.AxisChange();
                            strMapaEspalhamento[v] = Application.StartupPath + "\\Espalhamento_" + v.ToString() + ".jpeg";

                            //Salva o histograma
                            myPane.GetImage().Save(strMapaEspalhamento[v], ImageFormat.Jpeg);
                        }
                        if (arGetis.Count > 0)
                        {
                            iVetorGetis = (int[])arGetis[v];
                            //Retira a variável caso já exista
                            string nome = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v] + "_G*M";
                            if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);

                            //Adiciona a variavel no mapa
                            dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                            //Guarda os vetores
                            ArrayList vetorID = new ArrayList();
                            int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];

                            Hashtable[] ht = (Hashtable[])(m_caracteristicas_itens_layers[this.Name]);

                            for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                            {
                                vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                                vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                                if (iVetorGetis[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0)
                                    dsDados.Tables[0].Rows[i][nome] = iVetorGetis[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                                
                                // guardando os resultados no hashtable de características dos polígonos
                                (ht[i])["Fill"] = (Brush)cores[iVetorGetis[i]];
                            }
                            LayerPrincipalNoTopo();

                            //Pinta o mapa
                            iVetor = iVetorGetis;
                            SharpMap.Rendering.Thematics.CustomTheme iTheme =
                                new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetorGetis, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                            layMapa.Theme = iTheme;

                            //Refresh
                            mMapa.ZoomToExtents();

                            //Coloca o fundo branco
                            mapImage1.Map.BackColor = Color.White;

                            //Refresh o mapa
                            mapImage1.Refresh();

                            //Gera o mapa
                            System.Drawing.Bitmap img = new System.Drawing.Bitmap(mapImage1.Width, mapImage1.Height);
                            mapImage1.DrawToBitmap(img, mapImage1.ClientRectangle);

                            //Nome do MAPA
                            strMapaGetis[v] = Application.StartupPath + "\\MapaGetisAst_" + v.ToString() + ".jpeg";

                            //Salva o mapa
                            img.Save(strMapaGetis[v], ImageFormat.Jpeg);

                            //Coloca a variável que está sendo representada no mapa
                            this.txtVarMapaTematico.Text = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v].ToString();
                        }

                        if (arGetis2.Count > 0)
                        {
                            iVetorGetis2 = (int[])arGetis2[v];
                            //Retira a variável caso já exista
                            string nome = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v] + "_GM";
                            if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);

                            //Adiciona a variavel no mapa
                            dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                            //Guarda os vetores
                            ArrayList vetorID = new ArrayList();
                            int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];

                            Hashtable[] ht = (Hashtable[])(m_caracteristicas_itens_layers[this.Name]);

                            for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                            {
                                vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                                vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                                if (iVetorGetis2[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0)
                                    dsDados.Tables[0].Rows[i][nome] = iVetorGetis2[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                                
                                // guardando os resultados no hashtable de características dos polígonos
                                (ht[i])["Fill"] = (Brush)cores[iVetorGetis2[i]];
                            }
                            LayerPrincipalNoTopo();

                            //Pinta o mapa
                            iVetor = iVetorGetis2;
                            SharpMap.Rendering.Thematics.CustomTheme iTheme =
                                new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetorGetis2, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                            layMapa.Theme = iTheme;

                            //Refresh
                            mMapa.ZoomToExtents();

                            //Coloca o fundo branco
                            mapImage1.Map.BackColor = Color.White;

                            //Refresh o mapa
                            mapImage1.Refresh();

                            //Gera o mapa
                            System.Drawing.Bitmap img = new System.Drawing.Bitmap(mapImage1.Width, mapImage1.Height);
                            mapImage1.DrawToBitmap(img, mapImage1.ClientRectangle);

                            //Nome do MAPA
                            strMapaGetis2[v] = Application.StartupPath + "\\MapaGetis_" + v.ToString() + ".jpeg";

                            //Salva o mapa
                            img.Save(strMapaGetis2[v], ImageFormat.Jpeg);

                            //Coloca a variável que está sendo representada no mapa
                            this.txtVarMapaTematico.Text = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v].ToString();
                        }

                        if (arEscore.Count > 0)
                        {
                            iVetorEscore = (int[])arEscore[v];
                            //Retira a variável caso já exista
                            string nome = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v] + "_EM";
                            if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);

                            //Adiciona a variavel no mapa
                            dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));

                            //Guarda os vetores
                            ArrayList vetorID = new ArrayList();
                            int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];

                            Hashtable[] ht = (Hashtable[])(m_caracteristicas_itens_layers[this.Name]);

                            for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                            {
                                vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                                vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                                if (iVetorEscore[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0)
                                    dsDados.Tables[0].Rows[i][nome] = iVetorEscore[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                                
                                // guardando os resultados no hashtable de características dos polígonos
                                (ht[i])["Fill"] = (Brush)cores[iVetorEscore[i]];
                            }
                            LayerPrincipalNoTopo();

                            //Pinta o mapa
                            iVetor = iVetorEscore;
                            SharpMap.Rendering.Thematics.CustomTheme iTheme
                                = new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetorEscore, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                            layMapa.Theme = iTheme;

                            //Coloca o fundo branco
                            mapImage1.Map.BackColor = Color.White;

                            //Refresh
                            mMapa.ZoomToExtents();

                            //Refresh o mapa
                            mapImage1.Refresh();

                            //Gera o mapa
                            System.Drawing.Bitmap img = new System.Drawing.Bitmap(mapImage1.Width, mapImage1.Height);
                            mapImage1.DrawToBitmap(img, mapImage1.ClientRectangle);

                            //Nome do MAPA
                            strMapaEscore[v] = Application.StartupPath + "\\MapaEscore_" + v.ToString() + ".jpeg";

                            //Salva o mapa
                            img.Save(strMapaEscore[v], ImageFormat.Jpeg);

                            //Coloca a variável que está sendo representada no mapa
                            this.txtVarMapaTematico.Text = frmDependenciaLocal.VariaveisSelecionadasQuantitativas[v].ToString();
                        }
                    }

                    #region Gera o Relatório

                    if (frmDependenciaLocal.GeraRelatorio == true)
                    {
                        clsReport classeReport = new clsReport();
                        clsReport.DependenciaLocal dl = new clsReport.DependenciaLocal();

                        dl.strBase = strEnderecoBase; dl.strMapa = strEnderecoMapa;
                        dl.numPoligonos = shapeAlex.Count; dl.strMapaImagemLisa = strMapaLisa;
                        dl.strMapaImagemGetis = strMapaGetis; dl.strMapaImagemGetis2 = strMapaGetis2;
                        dl.strMapaImagemEscore = strMapaEscore; dl.strEspalha = strMapaEspalhamento;
                        dl.strTipoVizinhnaca = frmDependenciaLocal.TipoDeVizinhanca;
                        dl.strTipoCorrecao = frmDependenciaLocal.TipoDeCorrecao;
                        dl.strConfiabilidade = frmDependenciaLocal.Confiabilidade;
                        dl.strPopulacao = frmDependenciaLocal.Populacao;
                        dl.strVariaveisSelecionadas = frmDependenciaLocal.VariaveisSelecionadasQuantitativas;
                        dl.strCores = frmDependenciaLocal.CoresRGB;

                        string html = classeReport.IndiceDeDependenciaLocal(dl);

                        //Abre o relatório
                        frmRelatorio frmRelatorio = new frmRelatorio();
                        frmRelatorio.MdiParent = this.MdiParent;
                        frmRelatorio.codigoHTML = html;
                        frmRelatorio.Text = "Relatório " + this.Text + ": Índices de dependência espacial locais";

                        //Variáveis
                        frmRelatorio.isLocalDependence = true;
                        frmRelatorio.enderecoBase = strEnderecoBase;
                        frmRelatorio.enderecoMapa = strEnderecoMapa;
                        frmRelatorio.shapeCount = shapeAlex.Count;
                        frmRelatorio.strMapaLisa = strMapaLisa;
                        frmRelatorio.strMapaGetis = strMapaGetis;
                        frmRelatorio.strMapaGetis2 = strMapaGetis2;
                        frmRelatorio.strMapaEscore = strMapaEscore;
                        frmRelatorio.strMapaEspalhamento = strMapaEspalhamento;
                        frmRelatorio.tipoVizinhanca = frmDependenciaLocal.TipoDeVizinhanca;
                        frmRelatorio.tipoCorrecao = frmDependenciaLocal.TipoDeCorrecao;
                        frmRelatorio.populacao = frmDependenciaLocal.Populacao;
                        frmRelatorio.confiabilidade = frmDependenciaLocal.Confiabilidade;
                        frmRelatorio.coresMapaReg = frmDependenciaLocal.CoresRGB;
                        frmRelatorio.variaveisSelecionadasQuant = frmDependenciaLocal.VariaveisSelecionadasQuantitativas;

                        frmRelatorio.mapaLisa = arLisa;
                        frmRelatorio.mapaEscore = arEscore;
                        frmRelatorio.mapaEspalhamento = arEspalhamento;
                        frmRelatorio.mapaGetis = arGetis;
                        frmRelatorio.mapaGetis2 = arGetis2;
                        frmRelatorio.id = iBaseMapa;

                        frmRelatorio.Show();
                    }

                    mMapa.ZoomToExtents();
                    this.mapImage1.Refresh();

                    #endregion
                    
                    //Preenche a legenda do Mapa
                    if (dataGridView2.ColumnCount == 0)
                    {
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                            if (row.Index != 0) dataGridView1.Rows.Remove(row);
                        
                        //Adiciona cor
                        DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                        txtCor = new DataGridViewTextBoxColumn();
                        txtCor.Width = 36;
                        txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor.ReadOnly = true;
                        txtCor.HeaderText = "Cor";
                        txtCor.DisplayIndex = 2;
                        txtCor.DefaultCellStyle.BackColor = Color.White;
                        dataGridView2.Columns.Insert(0, txtCor);

                        //Adiciona Legenda
                        DataGridViewTextBoxColumn txtLegData = new DataGridViewTextBoxColumn();
                        txtLegData = new DataGridViewTextBoxColumn();
                        txtLegData.Width = 130;
                        txtLegData.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtLegData.ReadOnly = false;
                        txtLegData.HeaderText = "Legenda";
                        txtLegData.DisplayIndex = 2;
                        dataGridView2.Columns.Insert(1, txtLegData);

                        //Adiciona Limite Superior
                        DataGridViewCheckBoxColumn chkSelecionar = new DataGridViewCheckBoxColumn();
                        chkSelecionar = new DataGridViewCheckBoxColumn();
                        chkSelecionar.Width = 70;
                        chkSelecionar.HeaderText = "Selecionar";
                        chkSelecionar.DisplayIndex = 2;
                        dataGridView2.Columns.Insert(2, chkSelecionar);
                    }

                    string[] legenda = new string[5] { "Não significativo", "Alto-Alto", "Alto-Baixo", "Baixo-Baixo", "Baixo-Alto" };
                    for (int i = 0; i < 5; i++)
                    {
                        if (dataGridView2.Rows.Count < 5) dataGridView2.Rows.Add();
                        dataGridView2.Rows[i].Cells[0].Style.BackColor = new Pen(coresLocal[i]).Color;
                        dataGridView2.Rows[i].Cells[1].Value = legenda[i];
                        dataGridView2.Rows[i].Cells[1].ToolTipText = legenda[i];
                        dataGridView2.Rows[i].Cells[2].Value = false;
                    }

                    dataGridView1.Refresh();
                    
                    Cursor.Current = Cursors.Default;
                    Application.DoEvents();

                    AtualizaPeriodoPainelEspacial();

                    //sinaliza alteração no mapa temático
                    mapa_alterado = true;
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Análise Scan com distribuição de Bernoulli

        private void bernoulliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Abre a conexão
                frmScan frmScanEstat = new frmScan();

                //Guarda a base de dados
                frmScanEstat.DataTableDados = dsDados.Tables[0];

                //Guarda o ID do mapa
                frmScanEstat.IdentificadorMapa = strIDmapa;

                //Guarda o ID da base
                frmScanEstat.IdentificadorBase = strIDbase;

                //Guarda o endereço do mapa
                frmScanEstat.EnderecoMapa = strEnderecoMapa;

                //Define a distribuição
                frmScanEstat.Distribuicao = "BERNOULLI";

                //Abre o Dialog
                frmScanEstat.ShowDialog();

                if (frmScanEstat.DialogResult == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    //Captura o mapa
                    SharpMap.Map mMapa = mapImage1.Map;

                    //Captura o layer
                    SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[this.Name];

                    //Cria o vetor temático
                    int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                    //Guarda o vetor temático
                    iVetor = frmScanEstat.vetorPoligonos;

                    //Inicializa as cores
                    Brush[] cores = frmScanEstat.CoresParaMapa;

                    //Adiciona o pvalor na tabela
                    double[] pValor = frmScanEstat.vetorPvalor;

                    //Retira a variável caso já exista
                    string nome = "ScanB_" + frmScanEstat.VariavelEvento + "_" + frmScanEstat.VariavelBase;
                    if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);
                    string nome2 = "ScanB1_" + frmScanEstat.VariavelEvento + "_" + frmScanEstat.VariavelBase;
                    if (dsDados.Tables[0].Columns.Contains(nome2) == true) dsDados.Tables[0].Columns.Remove(nome2);

                    //Adiciona a variavel no mapa
                    dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));
                    dsDados.Tables[0].Columns.Add(nome2, Type.GetType("System.String"));
                     
                    //Guarda os vetores
                    ArrayList vetorID = new ArrayList();
                    int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                    for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                    {
                        vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                        vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                        int iPosicao = Convert.ToInt32(dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]);
                        if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0 && (int)iVetor[iPosicao] < 5) 
                            dsDados.Tables[0].Rows[i][nome] = pValor[iVetor[iPosicao]].ToString();

                        if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0 && (int)iVetor[iPosicao] < 5) 
                            dsDados.Tables[0].Rows[i][nome2] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                    }

                    //Pinta o mapa
                    SharpMap.Rendering.Thematics.CustomTheme iTheme = 
                        new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                    layMapa.Theme = iTheme;

                    //Refresh
                    mMapa.ZoomToExtents();

                    //Refresh o mapa
                    mapImage1.Refresh();

                    //Gera relatório
                    if (frmScanEstat.GeraRelatorio == true)
                    {
                        //Coloca o fundo branco
                        mapImage1.Map.BackColor = Color.White;

                        //Gera o mapa
                        System.Drawing.Bitmap img = new System.Drawing.Bitmap(mapImage1.Width, mapImage1.Height);
                        mapImage1.DrawToBitmap(img, mapImage1.ClientRectangle);

                        //Salva o mapa
                        img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);
                         
                        double[] vetorMontecarlo = frmScanEstat.MonteCarlo;
                        Array.Sort(vetorMontecarlo);
                        //Cria os intervalos
                        int iRazao = vetorMontecarlo.Length / 10;
                        double iIntervalos = (vetorMontecarlo[vetorMontecarlo.Length - 1] - vetorMontecarlo[0]) / iRazao;
                        double[] dblAbcissa = new double[iRazao];
                        double iDummy = iIntervalos;
                        for (int i = 0; i < iRazao; i++)
                        {
                            dblAbcissa[i] = iDummy;
                            iDummy += iIntervalos;
                        }

                        //Conta a frequencia
                        double[] dblOrdenada = new double[iRazao];
                        int iConta = 0;
                        for (int j = 0; j < vetorMontecarlo.Length; j++)
                        {
                            if (vetorMontecarlo[j] < dblAbcissa[iConta])
                                dblOrdenada[iConta]++;
                            else
                            {
                                dblOrdenada[iConta] /= vetorMontecarlo.Length;
                                if (iConta < dblOrdenada.Length - 1) iConta++;
                                dblOrdenada[iConta]++;
                            }
                        }
                        dblOrdenada[dblOrdenada.Length - 1] /= vetorMontecarlo.Length;

                        //Gera o histograma
                        GraphPane myPane = new GraphPane(new RectangleF(0, 0, 824, 631), 
                            "Densidade : Razão de verossimilhança", "Razão de verossimilhança", "Frequência");

                        // Add a red curve with circle symbols
                        LineItem curve = myPane.AddCurve("LLR", dblAbcissa, dblOrdenada, Color.White, SymbolType.None);

                        curve.Line.Width = 1.5F;
                        curve.Line.IsSmooth = true;
                        curve.Line.SmoothTension = 0.8F;

                        // Fill the area under the curve
                        curve.Line.Fill = new Fill(Color.Blue);

                        // Fill the symbols with white to make them opaque
                        curve.Symbol.Fill = new Fill(Color.Blue);

                        // Fill the axis background with a color gradient
                        myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 45.0F);

                        myPane.AxisChange();

                        //Salva o histograma
                        myPane.GetImage().Save(Application.StartupPath + "\\Histograma.jpeg", ImageFormat.Jpeg);
                         
                        //Gera o relatório
                        clsReport classeReport = new clsReport();
                        string html = classeReport.EstatisticaScanRelatorio(strEnderecoBase, strEnderecoMapa, 
                            Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count, frmScanEstat.Distribuicao, frmScanEstat.vetorPvalor,
                            frmScanEstat.CoresRGB, frmScanEstat.VariavelBase, frmScanEstat.VariavelEvento,
                            frmScanEstat.NumeroDeSimulacoes, frmScanEstat.NumeroDePontosGrid, frmScanEstat.RaioMaximo, 
                            frmScanEstat.RaioMinimo, frmScanEstat.ProporcaoMaxima, Application.StartupPath + "\\Histograma.jpeg");

                        //Abre o relatório
                        frmRelatorio frmRelatorio = new frmRelatorio();
                        frmRelatorio.MdiParent = this.MdiParent;
                        frmRelatorio.codigoHTML = html;
                        frmRelatorio.isScan = true;
                        frmRelatorio.enderecoBase = strEnderecoBase;
                        frmRelatorio.enderecoMapa = strEnderecoMapa;
                        frmRelatorio.endMapa = Application.StartupPath + "\\Mapa.jpeg";
                        frmRelatorio.shapeCount = shapeAlex.Count;
                        frmRelatorio.metodo = frmScanEstat.Distribuicao;
                        frmRelatorio.distribuicao = frmScanEstat.Distribuicao;
                        frmRelatorio.pValor = frmScanEstat.vetorPvalor;
                        frmRelatorio.coresMapaReg = frmScanEstat.CoresRGB;
                        frmRelatorio.variavelBase = frmScanEstat.VariavelBase;
                        frmRelatorio.variavelEvento = frmScanEstat.VariavelEvento;
                        frmRelatorio.strNumeroSimulacoes = frmScanEstat.NumeroDeSimulacoes;
                        frmRelatorio.numeroPontosGrid = frmScanEstat.NumeroDePontosGrid;
                        frmRelatorio.raioMaximo = frmScanEstat.RaioMaximo;
                        frmRelatorio.raioMinimo = frmScanEstat.RaioMinimo;
                        frmRelatorio.proporcaoMaxima = frmScanEstat.ProporcaoMaxima;
                        frmRelatorio.enderecoHistograma = Application.StartupPath + "\\Histograma.jpeg";

                        frmRelatorio.Text = "Relatório Estatística Scan: " + this.Text;

                        string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count + 1, 2];
                        strClusterMapa[0, 0] = strIDbase;
                        strClusterMapa[0, 1] = "Conglomerado";

                        for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                        {
                            string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                            string stCl = dsDados.Tables[0].Rows[i][nome2].ToString();
                            strClusterMapa[i + 1, 0] = stId;
                            strClusterMapa[i + 1, 1] = stCl;
                        }

                        frmRelatorio.histogramaX = dblAbcissa;
                        frmRelatorio.histogramaY = dblOrdenada;
                        frmRelatorio.variaveisMapa = strClusterMapa;

                        frmRelatorio.Show();
                    }
                     
                    Cursor.Current = Cursors.Default;

                    //sinaliza alteração no mapa temático
                    mapa_alterado = true;
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Análise Scan com distribuição de Poisson

        private void poissonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Abre a conexão
                frmScan frmScanEstat = new frmScan();

                //Guarda a base de dados
                frmScanEstat.DataTableDados = dsDados.Tables[0];

                //Guarda o ID do mapa
                frmScanEstat.IdentificadorMapa = strIDmapa;

                //Guarda o ID da base
                frmScanEstat.IdentificadorBase = strIDbase;

                //Define a distribuição
                frmScanEstat.Distribuicao = "POISSON";

                //Guarda o endereço do mapa
                frmScanEstat.EnderecoMapa = strEnderecoMapa;

                //Abre o Dialog
                frmScanEstat.ShowDialog();

                if (frmScanEstat.DialogResult == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    //Captura o mapa
                    SharpMap.Map mMapa = mapImage1.Map;

                    //Captura o layer
                    SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[this.Name];

                    //Cria o vetor temático
                    int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                    //Guarda o vetor temático
                    iVetor = frmScanEstat.vetorPoligonos;

                    //Inicializa as cores
                    Brush[] cores = frmScanEstat.CoresParaMapa;

                    //Adiciona o pvalor na tabela
                    double[] pValor = frmScanEstat.vetorPvalor;

                    //Retira a variável caso já exista
                    string nome = "ScanP_" + frmScanEstat.VariavelEvento + "_" + frmScanEstat.VariavelBase;
                    if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);
                    string nome2 = "ScanP1_" + frmScanEstat.VariavelEvento + "_" + frmScanEstat.VariavelBase;
                    if (dsDados.Tables[0].Columns.Contains(nome2) == true) dsDados.Tables[0].Columns.Remove(nome2);

                    //Adiciona a variavel no mapa
                    dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));
                    dsDados.Tables[0].Columns.Add(nome2, Type.GetType("System.String"));
                     
                    //Guarda os vetores
                    ArrayList vetorID = new ArrayList();
                    int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                    for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                    {
                        vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                        vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                        if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] > 0)
                            dsDados.Tables[0].Rows[i][nome] = pValor[iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] - 1].ToString();

                        if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] > 0)
                            dsDados.Tables[0].Rows[i][nome2] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                    }

                    //Pinta o mapa
                    SharpMap.Rendering.Thematics.CustomTheme iTheme = 
                        new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                    layMapa.Theme = iTheme;

                    //Refresh
                    mMapa.ZoomToExtents();

                    //Refresh o mapa
                    mapImage1.Refresh();

                    //Gera relatório
                    if (frmScanEstat.GeraRelatorio == true)
                    {
                        //Coloca o fundo branco
                        mapImage1.Map.BackColor = Color.White;

                        //Gera o mapa
                        System.Drawing.Bitmap img = new System.Drawing.Bitmap(mapImage1.Width, mapImage1.Height);
                        mapImage1.DrawToBitmap(img, mapImage1.ClientRectangle);

                        //Salva o mapa
                        img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);
                         
                        double[] vetorMontecarlo = frmScanEstat.MonteCarlo;
                        Array.Sort(vetorMontecarlo);
                        //Cria os intervalos
                        int iRazao = vetorMontecarlo.Length / 10;
                        double iIntervalos = (vetorMontecarlo[vetorMontecarlo.Length - 1] - vetorMontecarlo[0]) / iRazao;
                        double[] dblAbcissa = new double[iRazao];
                        double iDummy = iIntervalos;
                        for (int i = 0; i < iRazao; i++)
                        {
                            dblAbcissa[i] = iDummy;
                            iDummy += iIntervalos;
                        }

                        //Conta a frequencia
                        double[] dblOrdenada = new double[iRazao];
                        int iConta = 0;
                        for (int j = 0; j < vetorMontecarlo.Length; j++)
                        {
                            if (vetorMontecarlo[j] < dblAbcissa[iConta])
                                dblOrdenada[iConta]++;
                            else
                            {
                                dblOrdenada[iConta] /= vetorMontecarlo.Length;
                                if (iConta < dblOrdenada.Length - 1) iConta++;
                                dblOrdenada[iConta]++;
                            }
                        }
                        dblOrdenada[dblOrdenada.Length - 1] /= vetorMontecarlo.Length;

                        //Gera o histograma
                        GraphPane myPane = new GraphPane(new RectangleF(0, 0, 824, 631), 
                            "Densidade : Razão de verossimilhança", "Razão de verossimilhança", "Frequência");

                        // Add a red curve with circle symbols
                        LineItem curve = myPane.AddCurve("LLR", dblAbcissa, dblOrdenada, Color.White, SymbolType.None);

                        curve.Line.Width = 1.5F;
                        curve.Line.IsSmooth = true;
                        curve.Line.SmoothTension = 0.8F;

                        // Fill the area under the curve
                        curve.Line.Fill = new Fill(Color.Blue);

                        // Fill the symbols with white to make them opaque
                        curve.Symbol.Fill = new Fill(Color.Blue);

                        // Fill the axis background with a color gradient
                        myPane.Chart.Fill = new Fill(Color.White, Color.FromArgb(255, 255, 166), 45.0F);

                        myPane.AxisChange();

                        //Salva o histograma
                        myPane.GetImage().Save(Application.StartupPath + "\\Histograma.jpeg", ImageFormat.Jpeg);

                        //Gera o relatório
                        clsReport classeReport = new clsReport();
                        string html = classeReport.EstatisticaScanRelatorio(strEnderecoBase, strEnderecoMapa, 
                            Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count, frmScanEstat.Distribuicao, frmScanEstat.vetorPvalor,
                            frmScanEstat.CoresRGB, frmScanEstat.VariavelBase, frmScanEstat.VariavelEvento, 
                            frmScanEstat.NumeroDeSimulacoes, frmScanEstat.NumeroDePontosGrid, frmScanEstat.RaioMaximo, 
                            frmScanEstat.RaioMinimo, frmScanEstat.ProporcaoMaxima, 
                            Application.StartupPath + "\\Histograma.jpeg");

                        //Abre o relatório
                        frmRelatorio frmRelatorio = new frmRelatorio();
                        frmRelatorio.MdiParent = this.MdiParent;
                        frmRelatorio.codigoHTML = html;
                        frmRelatorio.isScan = true;
                        frmRelatorio.enderecoBase = strEnderecoBase;
                        frmRelatorio.enderecoMapa = strEnderecoMapa;
                        frmRelatorio.endMapa = Application.StartupPath + "\\Mapa.jpeg";
                        frmRelatorio.shapeCount = shapeAlex.Count;
                        frmRelatorio.metodo = frmScanEstat.Distribuicao;
                        frmRelatorio.distribuicao = frmScanEstat.Distribuicao;
                        frmRelatorio.pValor = frmScanEstat.vetorPvalor;
                        frmRelatorio.coresMapaReg = frmScanEstat.CoresRGB;
                        frmRelatorio.variavelBase = frmScanEstat.VariavelBase;
                        frmRelatorio.variavelEvento = frmScanEstat.VariavelEvento;
                        frmRelatorio.strNumeroSimulacoes = frmScanEstat.NumeroDeSimulacoes;
                        frmRelatorio.numeroPontosGrid = frmScanEstat.NumeroDePontosGrid;
                        frmRelatorio.raioMaximo = frmScanEstat.RaioMaximo;
                        frmRelatorio.raioMinimo = frmScanEstat.RaioMinimo;
                        frmRelatorio.proporcaoMaxima = frmScanEstat.ProporcaoMaxima;
                        frmRelatorio.enderecoHistograma = Application.StartupPath + "\\Histograma.jpeg";

                        frmRelatorio.Text = "Relatório Estatística Scan: " + this.Text;

                        string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count + 1, 2];
                        strClusterMapa[0, 0] = strIDbase;
                        strClusterMapa[0, 1] = "Conglomerado";

                        for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                        {
                            string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                            string stCl = dsDados.Tables[0].Rows[i][nome2].ToString();
                            strClusterMapa[i + 1, 0] = stId;
                            strClusterMapa[i + 1, 1] = stCl;
                        }

                        frmRelatorio.histogramaX = dblAbcissa;
                        frmRelatorio.histogramaY = dblOrdenada;
                        frmRelatorio.variaveisMapa = strClusterMapa;

                        frmRelatorio.Show();
                    }

                    Cursor.Current = Cursors.Default;

                    //sinaliza alteração no mapa temático
                    mapa_alterado = true;
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region análise de clusters spaciais hierárquicos

        private void hierarquicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Abre a conexão
                frmCluster frmCluster = new frmCluster();

                //Guarda a base de dados
                frmCluster.DataTableDados = dsDados.Tables[0];
               
                //Guarda o ID do mapa
                frmCluster.IdentificadorMapa = strIDmapa;

                //Guarda o ID da base
                frmCluster.IdentificadorDados = strIDbase;

                //Guarda o endereço do mapa
                frmCluster.EnderecoMapa = strEnderecoMapa;

                //Guarda a estrutura do shape
                frmCluster.EstruturaShape = shapeAlex;

                //Define como conglomerado espacial
                frmCluster.IsSpatialCluster = true;

                //Habilita o Label "Vizinhança"
                frmCluster.label4.Visible = true;

                //Modifica o tamanho do GroupBox
                //frmCluster.groupBox1.Size = new Size(240, 256);

                //Guarda as variáveis
                string[] strVariaveis = new string[dsDados.Tables[0].Columns.Count];
                for (int i = 0; i < strVariaveis.Length; i++) strVariaveis[i] = dsDados.Tables[0].Columns[i].ColumnName;
                frmCluster.Variaveis = strVariaveis;

                //Abre o Dialog
                frmCluster.ShowDialog();

                if (frmCluster.DialogResult == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    //Captura o mapa
                    SharpMap.Map mMapa = mapImage1.Map;

                    //Captura o layer
                    SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[this.Name];

                    //Cria o vetor temático
                    iVetor = new int[dsDados.Tables[0].Rows.Count];

                    //Guarda o vetor temático
                    iVetor = frmCluster.vetorPoligonos;

                    //Inicializa as cores
                    Brush[] cores = frmCluster.CoresParaMapa;
                    m_cores_mapa_tematico = new Brush[cores.GetLength(0)];
                    for (int i = 0; i < m_cores_mapa_tematico.GetLength(0); i++) m_cores_mapa_tematico[i] = (Brush)cores[i];

                    //Retira a variável caso já exista
                    string nome = "Cluster_Espacial";
                    string nome1 = "Label_Cluster_Espacial";
                    if (dsDados.Tables[0].Columns.Contains(nome) == true) dsDados.Tables[0].Columns.Remove(nome);
                    if (dsDados.Tables[0].Columns.Contains(nome1) == true) dsDados.Tables[0].Columns.Remove(nome1);

                    //Adiciona a variavel no mapa
                    dsDados.Tables[0].Columns.Add(nome, Type.GetType("System.String"));
                    dsDados.Tables[0].Columns.Add(nome1, Type.GetType("System.String"));
                    

                    //Guarda os vetores
                    ArrayList vetorID = new ArrayList();
                    int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                    
                    Hashtable[] ht = (Hashtable[])(m_caracteristicas_itens_layers[this.Name]);                    

                    for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                    {
                        vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                        vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                        if (iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]] >= 0)
                        {
                            dsDados.Tables[0].Rows[i][nome] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]].ToString();
                            dsDados.Tables[0].Rows[i][nome1] = "Conglomerado " + dsDados.Tables[0].Rows[i][nome];
                        }

                        // guardando os resultados no hashtable de características dos polígonos
                        (ht[i])["Fill"] = (Brush)cores[iVetor[i]];
                    }
                    
                    LayerPrincipalNoTopo();

                    //Pinta o mapa
                    SharpMap.Rendering.Thematics.CustomTheme iTheme = 
                        new SharpMap.Rendering.Thematics.CustomTheme((new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice)).GetStyle);
                    layMapa.Theme = iTheme;

                    //Refresh
                    mMapa.ZoomToExtents();

                    //Refresh o mapa
                    mapImage1.Refresh();

                    clsAlexCluster.NumMaxClustersArvore = (int)frmCluster.numMaxClusters.Value;
                    clsAlexCluster.NumMinClustersArvore = (int)frmCluster.numMinClusters.Value;

                    //Restrições para os tamanhos máximos dos clusters
                    clsAlexCluster.LimitaTamanhoMaximoFreq = false;
                    clsAlexCluster.LimitaTamanhoMaximoPerc = false;

                    #region Gera o Relatório

                    if (frmCluster.GeraRelatorio == true)
                    {
                        //this.Dock = DockStyle.Fill;
                        //this.Dock = DockStyle.None;

                        //Métodos para a escolha do número de conglomerados
                        dblCCC = frmCluster.CCC;
                        dblPseudoF = frmCluster.PseudoF;
                        dblPseudoT = frmCluster.PseudoT;
                        dblRSquare = frmCluster.RSquare;
                        dblRSquareExpected = frmCluster.RSquareExpected;
                        dblRSquarePartial = frmCluster.RSquarePartial;

                        #region Gera Gráficos

                        // Get a reference to the GraphPane
                        GraphPane myPane = new GraphPane(new RectangleF(0, 0, 824, 631), 
                            "Escolha do tamanho ótimo do número de conglomerados.", "Número de conglomerados", "Pseudo T");

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
                        int iObs = Convert.ToInt32(frmCluster.numCluster.Value) * 2;
                        int iTotal = dblRSquare.Length - 1;
                        for (int i = clsAlexCluster.NumMinClustersArvore; i <= clsAlexCluster.NumMaxClustersArvore /*iObs*/; i++)
                        {
                            rsList.Add(i, dblRSquare[iTotal]);
                            ptList.Add(i, dblPseudoT[iTotal]);
                            cccList.Add(i, dblCCC[iTotal]);
                            pfList.Add(i, dblPseudoF[iTotal]);
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

                        //Salva o gráfico
                        myPane.GetImage().Save(Application.StartupPath + "\\NumeroConglomerados.jpeg", ImageFormat.Jpeg);
                         
                        #endregion

                        //Coloca o fundo branco
                        mapImage1.Map.BackColor = Color.White;

                        //Gera o mapa
                        System.Drawing.Bitmap img = new System.Drawing.Bitmap(mapImage1.Width, mapImage1.Height);
                        mapImage1.DrawToBitmap(img, mapImage1.ClientRectangle);

                        //Salva o mapa
                        img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);

                        clsReport classeReport = new clsReport();
                        string html = classeReport.AnaliseDeConglomeradosEspacial(strEnderecoBase, strEnderecoMapa, 
                            Application.StartupPath + "\\Mapa.jpeg", frmCluster.NumeroDeConglomerados, frmCluster.FatorMinkowsky,
                            shapeAlex.Count, frmCluster.Metodo, frmCluster.Distancia, frmCluster.strEML, frmCluster.IsSpatialCluster, 
                            frmCluster.VariaveisSelecionadas, frmCluster.CoresRGB, shapeAlex.TipoVizinhanca, 
                            Application.StartupPath + "\\NumeroConglomerados.jpeg");

                        //Abre o relatório
                        frmRelatorio frmRelatorio = new frmRelatorio();
                        frmRelatorio.MdiParent = this.MdiParent;
                        frmRelatorio.codigoHTML = html;
                        frmRelatorio.Text = "Relatório " + this.Text + ": Análise de conglomerados espaciais";

                        //Métodos para a escolha do número de conglomerados
                        frmRelatorio.ccc = dblCCC;
                        frmRelatorio.pseudoF = dblPseudoF;
                        frmRelatorio.pseudoT = dblPseudoT;
                        frmRelatorio.rSquare = dblRSquare;
                        frmRelatorio.expectedRSquare = dblRSquareExpected;
                        frmRelatorio.partialRSquare = dblRSquarePartial;
                        frmRelatorio.isCluster = true;
                        frmRelatorio.nomeVariaveisMapa = frmCluster.VariaveisSelecionadas;
                        frmRelatorio.fatorMinkowski = frmCluster.FatorMinkowsky;
                        frmRelatorio.numeroConglomerados = frmCluster.NumeroDeConglomerados;
                        frmRelatorio.endCCC = Application.StartupPath + "\\NumeroConglomerados.jpeg";
                        frmRelatorio.tipoVizinhanca = shapeAlex.TipoVizinhanca;
                        frmRelatorio.enderecoMapa = strEnderecoMapa;
                        frmRelatorio.enderecoBase = strEnderecoBase;
                        frmRelatorio.distancia = frmCluster.Distancia;
                        frmRelatorio.metodo = frmCluster.Metodo;
                        frmRelatorio.coresMapaReg = frmCluster.CoresRGB;
                        frmRelatorio.endMapa = Application.StartupPath + "\\Mapa.jpeg";
                        frmRelatorio.shapeCount = shapeAlex.Count;
                         
                        string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count + 1, 2];
                        strClusterMapa[0, 0] = strIDbase;
                        strClusterMapa[0, 1] = "Conglomerado";

                        for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                        {
                            string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                            string stCl = dsDados.Tables[0].Rows[i][nome].ToString();
                            strClusterMapa[i + 1, 0] = stId;
                            strClusterMapa[i + 1, 1] = stCl;
                        }

                        frmRelatorio.variaveisMapa = strClusterMapa;
                        frmRelatorio.Show();
                    }

                    #endregion

                    //Gera legendas e demais
                    //Preenche a legenda do Mapa
                    if (dataGridView2.ColumnCount == 0)
                    {
                        //Adiciona cor
                        DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                        txtCor = new DataGridViewTextBoxColumn();
                        txtCor.Width = 36;
                        txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor.ReadOnly = true;
                        txtCor.HeaderText = "Cor";
                        txtCor.DisplayIndex = 2;
                        txtCor.DefaultCellStyle.BackColor = Color.White;
                        dataGridView2.Columns.Insert(0, txtCor);

                        //Adiciona Legenda
                        DataGridViewTextBoxColumn txtLegData = new DataGridViewTextBoxColumn();
                        txtLegData = new DataGridViewTextBoxColumn();
                        txtLegData.Width = 130;
                        txtLegData.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtLegData.ReadOnly = false;
                        txtLegData.HeaderText = "Legenda";
                        txtLegData.DisplayIndex = 2;
                        dataGridView2.Columns.Insert(1, txtLegData);

                        //Adiciona Limite Superior
                        DataGridViewCheckBoxColumn chkSelecionar = new DataGridViewCheckBoxColumn();
                        chkSelecionar = new DataGridViewCheckBoxColumn();
                        chkSelecionar.Width = 70;
                        //txtLimiteSuperior.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        //txtLimiteSuperior.ReadOnly = false;
                        chkSelecionar.HeaderText = "Selecionar";
                        chkSelecionar.DisplayIndex = 2;
                        //txtLimiteSuperior.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridView2.Columns.Insert(2, chkSelecionar);
                    }
                    dataGridView2.Rows.Clear();
                    for (int i = 0; i < cores.Length; i++)
                    {                        
                        if (dataGridView2.Rows.Count < cores.Length) dataGridView2.Rows.Add();
                        dataGridView2.Rows[i].Cells[0].Style.BackColor = new Pen(cores[i]).Color;
                        dataGridView2.Rows[i].Cells[1].Value = "Conglomerado " + i;
                        dataGridView2.Rows[i].Cells[1].ToolTipText = "Conglomerado " + i;
                        dataGridView2.Rows[i].Cells[2].Value = false;
                    }

                    dataGridView2.ClearSelection();
                     
                    //Refresh
                    mMapa.ZoomToExtents();

                    //Refresh o mapa
                    mapImage1.Refresh();

                    //Coloca a variável que está sendo representada no mapa
                    this.txtVarMapaTematico.Text = "";
                    for (int v = 0; v <  frmCluster.VariaveisSelecionadas.Length; v++)
                    {
                        this.txtVarMapaTematico.Text += frmCluster.VariaveisSelecionadas[v];
                        if (v != frmCluster.VariaveisSelecionadas.Length - 1) { this.txtVarMapaTematico.Text += ", "; }
                    }

                    AtualizaPeriodoPainelEspacial();
                    Cursor.Current = Cursors.Default;

                    //sinaliza alteração no mapa temático
                    mapa_alterado = true;
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region controle de toostrips

        private void RemoverSelecoes()
        {
            toolStripButton7.Checked = false;
            toolStripButton6.Checked = false;

            #region limpando a seleção das legendas

            if (this.Name == this.m_nome_top_layer)
                for (int i = 0; i < dataGridView2.RowCount; i++)
                    if ((bool)dataGridView2.Rows[i].Cells[2].Value == true)
                        dataGridView2.Rows[i].Cells[2].Value = false;

            #endregion

            label1.Visible = false;

            //Atualiza a lista de variaveis
            if (this.Name == this.m_nome_top_layer)
            {
                listBox1.Items.Clear();
                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++) listBox1.Items.Add(dsDados.Tables[0].Columns[i].ToString());
                listBox1.ClearSelected();

                //Atualizando a tabela
                dataGridView1.ClearSelection();
                dataGridView1.DataSource = dsDados.Tables[0].Copy();
                dataGridView1.Refresh();
            }

            //Atualiza seleção de polígonos (hachureados)
            DataTable dt = (DataTable)this.m_variaveis_dados_layers[this.m_nome_top_layer];

            int[] selecao_hach = new int[dt.Rows.Count];
            for (int i = 0; i < selecao_hach.GetLength(0); i++)
                selecao_hach[i] = 0;

            IpeaGeo.Classes.clsManipulaItensNoLayer clm = new Classes.clsManipulaItensNoLayer();
            Hashtable[] ht = (Hashtable[])this.m_caracteristicas_itens_layers[this.m_nome_top_layer];
            string id_chave = "";
            if (this.Name == this.m_nome_top_layer) id_chave = strIDmapa;

            clm.HachurearPoligonos(ref this.map, ref this.mapImage1,
                this.m_nome_top_layer, ref ht, ref dt, id_chave, ref selecao_hach);

            ((ArrayList)m_poligonos_selecionados[m_nome_top_layer]).Clear();
            AjustaEnabledOpcoesLayersSelecionados();
            linhas_clicadas.Clear();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            xpTabControl2.SelectTab(tabPage3);
            label3.Visible = false;
            //Remover as seleções
            try
            {             
                RemoverSelecoes();
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (shapeAlex.TipoVizinhanca == "")
                {
                    //Cria a vizinhnaça
                    clsIpeaShape cps = new clsIpeaShape();
                    int tipo_vizinhanca = -1;
                    tipo_vizinhanca = 1;
                    if (tipo_vizinhanca == 1) shapeAlex.TipoVizinhanca = "Queen";
                    cps.DefinicaoVizinhos(ref shapeAlex, tipo_vizinhanca);
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            xpTabControl2.SelectTab(tabPage3);
            label1.Visible = false;
            timer1.Interval = 1;
            timer1.Start();
            try
            {
                toolStripButton7.Checked = false;
                tlInformacao.Checked = false;
                moveMapa.Checked = false;
                toolStripButton6.Checked = false;
                toolStripButton3.Checked = false;
                toolStripButton4.Checked = false;
                toolStripButton8.Checked = false;
                toolStripButton5.Checked = false;

                mapImage1.ActiveTool = (toolStripButton2.Checked) ? SharpMap.Forms.MapImage.Tools.ZoomWindow : SharpMap.Forms.MapImage.Tools.None;
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            xpTabControl2.SelectTab(tabPage3);
            label1.Text = "Duplo Clique para Finalizar\n\nClique em Atualizar para finalizar a seleção";
            label3.Visible = false;
            try
            {
                if (!toolStripButton6.Checked) label1.Visible = false;

                //Remove as seleções antes de iniciar o processo
                toolStripButton8.Checked = true;
                SharpMap.Map _map = mapImage1.Map;
                int total = grupo.Layers.Count;

                for (int i = total - 1; i >= 0; i--)
                {
                    grupo.Layers.RemoveAt(i);
                    _map.Layers.RemoveAt(i + 1);
                }

                //Adiciona o novo Map
                mapImage1.Map = _map;

                this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;
                toolStripButton8.Checked = false;
                mapImage1.Refresh();

                //Atualiza a lista de variaveis
                listBox1.Items.Clear();
                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++) listBox1.Items.Add(dsDados.Tables[0].Columns[i].ToString());

                toolStripButton7.Checked = false;
                tlInformacao.Checked = false;
                moveMapa.Checked = false;
                toolStripButton2.Checked = false;
                toolStripButton3.Checked = false;
                toolStripButton4.Checked = false;
                label1.Visible = toolStripButton6.Checked;
                mapImage1.ActiveTool = (toolStripButton6.Checked) ? SharpMap.Forms.MapImage.Tools.Measure : SharpMap.Forms.MapImage.Tools.None;
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void uTM18ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.map.SetProjectByWKT("PROJCS[\"NAD_1983_UTM_Zone_18N\",GEOGCS[\"GCS_North_American_1983\",DATUM[\"D_North_American_1983\",SPHEROID[\"GRS_1980\",6378137,298.257222101]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"False_Easting\",500000],PARAMETER[\"False_Northing\",0],PARAMETER[\"Central_Meridian\",-75],PARAMETER[\"Scale_Factor\",0.9996],PARAMETER[\"Latitude_Of_Origin\",0],UNIT[\"Meter\",1]]");
                this.map.ZoomToExtents();
                mapImage1.Update();
                mapImage1.Refresh();

                this.mapImage1.Map.ZoomToExtents();
                this.mapImage1.Refresh();
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void entropiaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (shapeAlex.TipoVizinhanca == "")
                {
                    //Cria a vizinhnaça
                    clsIpeaShape cps = new clsIpeaShape();
                    int tipo_vizinhanca = 1;

                    //"Queen" tipo_vizinhanca = 1;
                    //"Rook " tipo_vizinhanca = 2;

                    if (tipo_vizinhanca == 1) shapeAlex.TipoVizinhanca = "Queen";
                    if (tipo_vizinhanca == 2) shapeAlex.TipoVizinhanca = "Rook";

                    cps.DefinicaoVizinhos(ref shapeAlex, tipo_vizinhanca);
                }

                clsSpatialCluster clsEspacial = new clsSpatialCluster();
                clsEspacial.PopulacaoInicial(shapeAlex, 5);
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            try
            {
                //Abre o formulário
                frmIndicadoresSegregacao frmIndices = new frmIndicadoresSegregacao();

                //Variáveis
                string[] strVariaveis = new string[dsDados.Tables[0].Columns.Count];
                for (int i = 0; i < strVariaveis.Length; i++) strVariaveis[i] = dsDados.Tables[0].Columns[i].ColumnName;
                frmIndices.Variaveis = strVariaveis;
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool segundavez = false;

        #endregion
        
        #region programação nova da selação de base do IPEAGEO

        [Obsolete()]
        private void getExcelData(FormConcatenacaoTabelaToShape frm, string title, string name)
        {
            FileInfo tBase = new FileInfo(String.Format(@"{0}\Bases\{1}.xls", appPath, name));
            bool TemBase = tBase.Exists;

            if (TemBase == false)
            {
                DialogResult result;
                result = MessageBox.Show("Essa base de dados não existe. Deseja fazer seu download?", "Aviso", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string site = "http://www.ipea.gov.br/ipeageo/docs";
                    string LocalBases = String.Format(@"{0}\Bases", appPath);

                    System.Net.WebClient client = new System.Net.WebClient();
                    client.Headers.Add("user-agent", "Only a test!");
                    client.DownloadFile(String.Format("{0}/{1}.xls", site, name), String.Format(@"{0}\{1}.xls", LocalBases, name));
                    Cursor.Current = Cursors.Default;

                    if (!this.xpTabControl2.TabPages.Contains(tabPage4))
                        this.xpTabControl2.TabPages.Add(tabPage4);
                    else { return; }
                }
                frm.CidadesNOME = title;
            }
        }

        private static bool tab_mod;
        public bool tab_mod_efetuada = tab_mod;

        private void SelecionaBaseIPEAGEO()
        {
            try
            {
                m_mdiparent = (IpeaGeo.IPEAGEOMDIParent)this.MdiParent;

                bool erro = false;
                //cidades = true;

                string FileName = "";
                FormConcatenacaoTabelaToShape frm = new FormConcatenacaoTabelaToShape();
                frm.TabelaShape = this.m_tabela_shape;
                frm.CidadesPublic = true;
                frm.PastaPublic = appPath;
                frm.Shape.ConvertFromIpeaGEOShape(this.shapeAlex);

                #warning Reimplementar usando o arquivo XML.
                #region Reimplementar usando XML.
                string[] labels = { "AC_Mun97_region", "AL_Mun97_region", "AP_Mun97_region",
                                      "AM_Mun97_region", "BA_Mun97_region", "CE_Mun97_region",
                                      "DF_Mun97_region", "ES_Mun97_region", "GO_Mun97_region",
                                      "MA_Mun97_region", "MG_Mun97_region", "MS_Mun97_region",
                                      "MT_Mun97_region", "PA_Mun97_region", "PB_Mun97_region",
                                      "PE_Mun97_region", "PI_Mun97_region", "PR_Mun97_region",
                                      "RJ_Mun97_region", "RN_Mun97_region", "RO_Mun97_region",
                                      "RR_Mun97_region", "RS_Mun97_region", "SC_Mun97_region",
                                      "SE_Mun97_region", "SP_Mun97_region", "TO_Mun97_region",
                                      "BR_MUN1997_CO_region", "BR_MUN1997_N_region",
                                      "BR_MUN1997_NE_region", "BR_MUN1997_S_region",
                                      "BR_MUN1997_SE_region", "mesoregiao",
                                      "microregiao", "municipio", "municipio5564",
                                      "sao_francisco", "IDH_1991", "IDH_2000", "IDH_2010" };

                string[] names = { "AC_Mun97_region", "AL_Mun97_region", "AP_Mun97_region",
                                      "AM_Mun97_region", "BA_Mun97_region", "CE_Mun97_region",
                                      "DF_Mun97_region", "ES_Mun97_region", "GO_Mun97_region",
                                      "MA_Mun97_region", "MG_Mun97_region", "MS_Mun97_region",
                                      "MT_Mun97_region", "PA_Mun97_region", "PB_Mun97_region",
                                      "PE_Mun97_region", "PI_Mun97_region", "PR_Mun97_region",
                                      "RJ_Mun97_region", "RN_Mun97_region", "RO_Mun97_region",
                                      "RR_Mun97_region", "RS_Mun97_region", "SC_Mun97_region",
                                      "SE_Mun97_region", "SP_Mun97_region", "TO_Mun97_region",
                                      "BR_MUN1997_CO_region", "BR_MUN1997_N_region",
                                      "BR_MUN1997_NE_region", "BR_MUN1997_S_region",
                                      "BR_MUN1997_SE_region", "IBGE_CIDADES_MESOREGIAO",
                                      "IBGE_CIDADES_MICROREGIAO", "CIDADES_IPEAGEO", 
                                      "IBGE_CIDADES_5564", "CIDADES_SAO_FRANCISCO", 
                                      "IDH_1991", "IDH_2000", "IDH_2010" };

                for (int k = 0; k < labels.Length; k++)
                    if (labels[k] == Name)
                    {
                        getExcelData(frm, labels[k], names[k]);
                        frm.CidadesNOME = labels[k];
                    }
                
                #endregion

                bool conc_tab = false;
                if (tab_mod == true)
                {
                    frm.LerXML();
                    FileName = basefile;
                    conc_tab = true;
                }
                else
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        FileName = frm.FileName;
                        conc_tab = true; 
                    }
                }

                if (conc_tab == true)
                {
                    //FileName = frm.FileName;

                    //Guarda o endereço da base
                    strEnderecoBase = FileName;

                    //Extensão
                    strExtensao = Path.GetExtension(FileName).ToUpper();

                    Cursor.Current = Cursors.WaitCursor;

                    //Habilita a exportação
                    toolExportaDados.Enabled = true;
                    toolStripButton10.Enabled = true;
                    toolRefresh.Enabled = true;

                    //Guarda a base de dados conectada
                    dsDados.Tables.Clear();
                    dsDados.Tables.Add(frm.TabelaDadosConcatenados);

                    m_tabela_dados_original = frm.TabelaDadosConcatenados.Copy();

                    int row_poligono = 0;
                    for (int i = 0; i < m_tabela_dados_original.Rows.Count; i++)
                    {
                        row_poligono = Convert.ToInt32(m_tabela_dados_original.Rows[i]["Mapa" + frm.IDmapa]);
                        shapeAlex[row_poligono].ID = m_tabela_dados_original.Rows[i]["shape_" + frm.IDmapa].ToString();
                        shapeAlex[row_poligono].PosicaoNoDataTable = i;
                    }

                    //Guarda as variaveis que existem no mapa
                    strVariaveisMapa = frm.VariaveisNoShape;

                    //Guarda ID mapa
                    strIDmapa = frm.IDmapa;

                    //Guarda ID Base
                    strIDbase = frm.IDbase;

                    //Habilita as funções que só funcionam após o join
                    toolStripButton7.Enabled = true;
                    toolStripButton6.Enabled = true;
                    toolStripButton8.Enabled = true;

                    segundavez = true;

                    //Habilita o botão "saveToolStripButton" para salvar o trabalho após o join
                    saveToolStripButton.Enabled = true;

                    //Habilita os botões de análise
                    ToolspatialEstat.Enabled = true;
                    toolStripCalculadora.Enabled = true;
                    mapaTematico.Enabled = true;
                    this.toolStripMapasTematicos.Enabled = true;
                    tlInformacao.Enabled = true;

                    //Guarda o dataTable
                    dataGridView1.DataSource = dsDados.Tables[0];
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.Refresh();

                    //Lista as variáveis
                    listBox1.Items.Clear();
                    for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++) listBox1.Items.Add(dsDados.Tables[0].Columns[i].ToString());

                    Cursor.Current = Cursors.Default;

                    if (!this.xpTabControl2.TabPages.Contains(tabPage4))
                    {
                        this.xpTabControl2.TabPages.Add(tabPage4);
                    }
                }
                else if (erro == false)
                {
                    System.Windows.Forms.ImageList imagesLarge1 = new ImageList();
                    imagesLarge1.ImageSize = new Size(45, 45);
                    System.Windows.Forms.ImageList imagesSmall1 = new ImageList();
                    imagesSmall1.ImageSize = new Size(20, 20);
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
                
        #region botão de refresh do mapa

        private string m_nome_top_layer = "";

        private void AtualizaSelecao()
        {
            #region selecionado as linhas na tabela de acordo com a seleção dos polígonos

            if (((ArrayList)m_poligonos_selecionados[this.Name]).Count > 0)
            {
                DataTable dt_temporario = this.dsDados.Tables[0].Clone();

                for (int i = 0; i < this.dsDados.Tables[0].Rows.Count; i++)
                    if (((ArrayList)m_poligonos_selecionados[this.Name]).Contains(i))
                        dt_temporario.Rows.Add(this.dsDados.Tables[0].Rows[i].ItemArray);

                if (listBox1.SelectedItems.Count < listBox1.Items.Count && listBox1.SelectedItems.Count > 0)
                    for (int i = 0; i < listBox1.Items.Count; i++)
                        if (!listBox1.GetSelected(i))
                            dt_temporario.Columns.Remove(listBox1.Items[i].ToString());

                dataGridView1.ClearSelection();
                dataGridView1.DataSource = dt_temporario;
                dataGridView1.Refresh();
            }

            #endregion

            #region selecionando os polígonos (hachureando) de acordo com a selação na tabela de dados

            if (linhas_clicadas.Count > 0 && ((ArrayList)m_poligonos_selecionados[this.Name]).Count == 0 && !toolStripButton7.Checked)
            {
                // Hachureando os polígonos selecionados
                dataGridView1.ClearSelection();
                DataTable dt = (DataTable)this.m_variaveis_dados_layers[this.Name];

                int[] selecao_hach = new int[dt.Rows.Count];
                for (int i = 0; i < selecao_hach.GetLength(0); i++)
                    if (this.linhas_clicadas.Contains(i)) selecao_hach[i] = 1;
                    else selecao_hach[i] = 0;

                IpeaGeo.Classes.clsManipulaItensNoLayer clm = new Classes.clsManipulaItensNoLayer();
                Hashtable[] ht = (Hashtable[])this.m_caracteristicas_itens_layers[this.Name];

                clm.HachurearPoligonos(ref this.map, ref this.mapImage1,
                    this.Name, ref ht, ref dt, strIDmapa, ref selecao_hach);

                ((ArrayList)m_poligonos_selecionados[this.Name]).Clear();
                AjustaEnabledOpcoesLayersSelecionados();
                for (int i=0; i<linhas_clicadas.Count; i++)
                    ((ArrayList)m_poligonos_selecionados[this.Name]).Add((int)linhas_clicadas[i]);
                
                AjustaEnabledOpcoesLayersSelecionados();

                DataTable dt_temporario = this.dsDados.Tables[0].Clone();
                for (int i = 0; i < this.dsDados.Tables[0].Rows.Count; i++)
                    if (((ArrayList)m_poligonos_selecionados[this.Name]).Contains(i))
                        dt_temporario.Rows.Add(this.dsDados.Tables[0].Rows[i].ItemArray);

                if (listBox1.SelectedItems.Count < listBox1.Items.Count && listBox1.SelectedItems.Count > 0)
                    for (int i = 0; i < listBox1.Items.Count; i++)
                        if (!listBox1.GetSelected(i))
                            dt_temporario.Columns.Remove(listBox1.Items[i].ToString());

                dataGridView1.ClearSelection();
                dataGridView1.DataSource = dt_temporario;
                dataGridView1.Refresh();
                linhas_clicadas.Clear();
            }

            #endregion

            if (linhas_clicadas.Count == 0 && (listBox1.SelectedItems.Count == 0 || listBox1.SelectedItems.Count == listBox1.Items.Count) && ((ArrayList)m_poligonos_selecionados[this.Name]).Count == 0)
                RemoverSelecoes();

            #region selecionando as variáveis clicadas na lista de variáveis

            if (linhas_clicadas.Count == 0 && listBox1.SelectedItems.Count > 0 && listBox1.SelectedItems.Count < listBox1.Items.Count && ((ArrayList)m_poligonos_selecionados[this.Name]).Count == 0)
            {
                DataTable dt_temporario = this.dsDados.Tables[0].Copy();
                
                if (listBox1.SelectedItems.Count < listBox1.Items.Count && listBox1.SelectedItems.Count > 0)
                    for (int i = 0; i < listBox1.Items.Count; i++)
                        if (!listBox1.GetSelected(i))
                            dt_temporario.Columns.Remove(listBox1.Items[i].ToString());

                dataGridView1.ClearSelection();
                dataGridView1.DataSource = dt_temporario;
                dataGridView1.Refresh();
            }

            #endregion
        }

        private void toolRefresh_Click(object sender, EventArgs e)
        {
            label3.Text = "Tabela de Dados atualizada";
            timer1.Interval = 5000;
            timer1.Start();
            
            label1.Visible = false;
            label3.Visible = true;

            //sinaliza alteração no mapa temático
            mapa_alterado = true;
            try
            {
                Cursor = Cursors.WaitCursor;
                AtualizaSelecao();
                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region ativa informação do polígono

        private void tlInformacao_Click(object sender, EventArgs e)
        {
            label1.Text = "Clique sobre a área desejada para obter as informações do polígono";
            label1.Visible = true;
            xpTabControl2.SelectTab(tabPage3);
            try
            {
                mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;
                mapImage1.Refresh();

                toolStripButton7.Checked = false;
                toolStripButton2.Checked = false;
                moveMapa.Checked = false;
                toolStripButton6.Checked = false;
                toolStripButton3.Checked = false;
                toolStripButton4.Checked = false;
                toolStripButton8.Checked = false;
                toolStripButton5.Checked = false;

                mapImage1.ActiveTool = (tlInformacao.Checked) ? SharpMap.Forms.MapImage.Tools.ZoomWindow : SharpMap.Forms.MapImage.Tools.None;
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region manipulação do formulário de mapas

        private void frmMapa_Shown(object sender, EventArgs e)
        {
            try
            {
                Classes.clsArmazenamentoDados salvar = new Classes.clsArmazenamentoDados();
                if (lendo == true)
                {
                    this.mapImage1.Map.Zoom = salvar.zoom_m;
                    this.mapImage1.Map.Center = new SharpMap.Geometries.Point(salvar.centro_map[0], salvar.centro_map[1]);
                    lendo = false;
                }
                else
                {
                    this.mapImage1.Map.ZoomToExtents();
                    escala();
                }
                this.mapImage1.Refresh();
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void frmMapa_Resize_1(object sender, EventArgs e)
        {
            try
            {
                this.mapImage1.Map.ZoomToExtents();
                this.mapImage1.Refresh();
                escala();
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region seleção do polígono no layer
        
        private void mapImage1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                label1.Text = "Clique em Atualizar para finalizar a seleção";
                Application.DoEvents();

                //Lista de polígonos viziveis
                double minX = mapImage1.Map.Envelope.Min.X;
                double minY = mapImage1.Map.Envelope.Min.Y;
                double maxX = mapImage1.Map.Envelope.Max.X;
                double maxY = mapImage1.Map.Envelope.Max.Y;

                //Funcao measure está ativada
                if (toolStripButton6.Checked)
                {
                    clsAreaPerimetroCentroide apc = new clsAreaPerimetroCentroide();
                    clsMapa cmap = new clsMapa();
                    
                    //Guarda o poligono do measure
                    List<SharpMap.Geometries.Point> lista = mapImage1.MeasurePoints;
                    SharpMap.Geometries.Polygon poligono = cmap.polygonFromList(lista);

                    //Captura o primeiro Layer
                    SharpMap.Layers.VectorLayer needLayer = (SharpMap.Layers.VectorLayer)map.Layers[this.m_nome_top_layer];

                    SharpMap.Data.FeatureDataTable tableSelected = new FeatureDataTable();

                    double x_temporario = 0.0;
                    double y_temporario = 0.0;
                    bool EstaNoPoligono = false;

                    clsIpeaShape shape_layer = new clsIpeaShape();
                    shape_layer = (this.Name == m_nome_top_layer) ? shapeAlex : (clsIpeaShape)m_shape_Alex_layers[m_nome_top_layer];

                    for (int j = 0; j < shape_layer.Count; j++)
                    {
                        if (shape_layer[j].BoundingBoxXMin >= minX && shape_layer[j].BoundingBoxXMax <= maxX)
                        {
                            if (shape_layer[j].BoundingBoxYMin >= minY && shape_layer[j].BoundingBoxYMax <= maxY)
                            {
                                //Captura os pontos do poligono no Shape j
                                SharpMap.Geometries.Point mPontos;
                                x_temporario = (shape_layer[j].BoundingBoxXMax + shape_layer[j].BoundingBoxXMin) / 2.0;
                                y_temporario = (shape_layer[j].BoundingBoxYMax + shape_layer[j].BoundingBoxYMin) / 2.0;

                                mPontos = new SharpMap.Geometries.Point(x_temporario, y_temporario);

                                //Verifica se o ponto está no polígono
                                EstaNoPoligono = cmap.isPointInPolygon(mPontos, poligono);

                                if (EstaNoPoligono)
                                {
                                    if (((ArrayList)m_poligonos_selecionados[this.m_nome_top_layer]).Contains(j)) ((ArrayList)m_poligonos_selecionados[this.m_nome_top_layer]).Remove(j);
                                    else ((ArrayList)m_poligonos_selecionados[this.m_nome_top_layer]).Add(j);
                                }
                                AjustaEnabledOpcoesLayersSelecionados();
                            }
                        }
                    }

                    // hachureando os polígonos selecionados
                    DataTable dt = (DataTable)this.m_variaveis_dados_layers[this.m_nome_top_layer];

                    int[] selecao_hach = new int[dt.Rows.Count];
                    for (int i = 0; i < selecao_hach.GetLength(0); i++)
                    {
                        if (((ArrayList)m_poligonos_selecionados[this.m_nome_top_layer]).Contains(i)) selecao_hach[i] = 1;
                        else selecao_hach[i] = 0;
                    }

                    IpeaGeo.Classes.clsManipulaItensNoLayer clm = new Classes.clsManipulaItensNoLayer();
                    Hashtable[] ht = (Hashtable[])this.m_caracteristicas_itens_layers[this.Name];
                    string id_chave = "";
                    if (this.Name == this.m_nome_top_layer) id_chave = strIDmapa;

                    clm.HachurearPoligonos(ref this.map, ref this.mapImage1,
                        this.m_nome_top_layer, ref ht, ref dt, id_chave, ref selecao_hach);

                    //Adiciona o novo Map
                    mapImage1.Map = map;

                    this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;
                    toolStripButton6.Checked = false;
                    mapImage1.Refresh();
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private SharpMap.Layers.LayerGroup grupo = new SharpMap.Layers.LayerGroup("Todos_Layers");

        private void mapImage1_MouseDown_1(SharpMap.Geometries.Point WorldPos, MouseEventArgs ImagePos)
        {
            try
            {
                #region Se o usuário habilitar a pesquisa

                if (tlInformacao.Checked == true)
                {
                    //Desabilita qualquer função de ZOOM ou PAN
                    this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;

                    //Passo 1: Obtém o ponto clicado
                    double x = WorldPos.X;
                    double y = WorldPos.Y;

                    //Passo 2: Obtém o polígono escolhido
                    clsMapa ClasseMapa = new clsMapa();
                    int iPoligono = -1;
                    int iPoligonoBase = -1;

                    if (iPoligono < 0 || iPoligono >= shapeAlex.Count)
                        iPoligono = ClasseMapa.identifyPointInPoligono(shapeAlex, x, y);

                    //Captura os dados
                    DataSet dsTemp = new DataSet();
                    dsTemp = dsDados.Copy();
                    if (listBox1.SelectedItems.Count == listBox1.Items.Count || listBox1.SelectedItems.Count == 0)
                    {
                        dataGridView1.DataSource = dsDados.Tables[0];
                        dataGridView1.Refresh();
                    }
                    else
                    {
                        for (int i = 0; i < listBox1.Items.Count; i++)
                            if (!listBox1.GetSelected(i))
                                dsTemp.Tables[0].Columns.Remove(listBox1.Items[i].ToString());
                        
                        dataGridView1.DataSource = dsTemp.Tables[0];
                        dataGridView1.Refresh();
                    }

                    DataRow dRow = dsTemp.Tables[0].Rows[0];
                    for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                        if (dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa].ToString() == iPoligono.ToString())
                        {
                            dRow = dsTemp.Tables[0].Rows[i];
                            iPoligonoBase = i;
                            break;
                        }
                    
                    string strInfor = "";

                    if (-1 < iPoligono && iPoligono < shapeAlex.Count)
                    {
                        for (int i = 0; i < dRow.Table.Columns.Count; i++)
                            strInfor += dRow.Table.Columns[i] + ": " + dRow.Table.Rows[iPoligonoBase][i].ToString() + "\n";

                        toolTip1.Show(strInfor, this, ImagePos.X, ImagePos.Y, 5000);
                    }
                }

                #endregion

                SharpMap.Map _map = mapImage1.Map;
                

                #region Se a ferramenta é a de seleção individual de polígonos

                if (_map.Layers.Count > 0 && toolStripButton7.Checked)
                {
                    //Captura o primeiro Layer
                    SharpMap.Layers.VectorLayer needLayer = (SharpMap.Layers.VectorLayer)_map.Layers[this.m_nome_top_layer];

                    //Define a classe de funções para o mapa
                    clsMapa clsFuncoesMapa = new clsMapa();

                    //Passo 1: Obtém o ponto clicado
                    double x = WorldPos.X;
                    double y = WorldPos.Y;

                    int iPoligono = -1;
                     
                    clsMapa ClasseMapa = new clsMapa();

                    if (this.Name == this.m_nome_top_layer)
                    {
                        if (iPoligono < 0 || iPoligono >= shapeAlex.Count)
                            iPoligono = ClasseMapa.identifyPointInPoligono(shapeAlex, x, y);
                    }
                    else
                    {
                        clsIpeaShape shape_layer = (clsIpeaShape)m_shape_Alex_layers[m_nome_top_layer];
                        if (iPoligono < 0 || iPoligono >= shape_layer.Count)
                            iPoligono = ClasseMapa.identifyPointInPoligono(shape_layer, x, y);
                    }
                    
                    if (((ArrayList)m_poligonos_selecionados[this.m_nome_top_layer]).Contains(iPoligono)) ((ArrayList)m_poligonos_selecionados[this.m_nome_top_layer]).Remove(iPoligono);
                    else ((ArrayList)m_poligonos_selecionados[this.m_nome_top_layer]).Add(iPoligono);
                    
                    AjustaEnabledOpcoesLayersSelecionados();

                    // hachureando os polígonos selecionados
                    DataTable dt = (DataTable)this.m_variaveis_dados_layers[this.m_nome_top_layer];
                    int[] selecao_hach = new int[dt.Rows.Count];
                    for (int i = 0; i < selecao_hach.GetLength(0); i++)
                    {
                        if (((ArrayList)m_poligonos_selecionados[this.m_nome_top_layer]).Contains(i)) selecao_hach[i] = 1;
                        else selecao_hach[i] = 0;
                    }

                    IpeaGeo.Classes.clsManipulaItensNoLayer clm = new Classes.clsManipulaItensNoLayer();
                    Hashtable[] ht = (Hashtable[])this.m_caracteristicas_itens_layers[this.m_nome_top_layer];
                    string id_chave = "";
                    if (this.Name == this.m_nome_top_layer) id_chave = strIDmapa;

                    clm.HachurearPoligonos(ref this.map, ref this.mapImage1,
                        this.m_nome_top_layer, ref ht, ref dt, id_chave, ref selecao_hach);
                    
                }

                #endregion
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void mapImage1_MouseUp(SharpMap.Geometries.Point WorldPos, MouseEventArgs ImagePos)
        {
            escala();
        }

        #endregion

        #region índices de Segregacao espacial

        private void índicesDeSegregacaoEspacialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Abre a conexão    
                frmIndicadoresSegregacao frmCluster = new frmIndicadoresSegregacao();

                //Guarda a base de dados
                frmCluster.DataTableDados = dsDados.Tables[0];

                //Guarda o ID do mapa
                frmCluster.IdentificadorMapa = strIDmapa;

                //Guarda o ID da base
                frmCluster.IdentificadorDados = strIDbase;

                //Guarda o endereço do mapa
                frmCluster.EnderecoMapa = strEnderecoMapa;

                //Guarda a estrutura do shape
                frmCluster.EstruturaShape = shapeAlex;

                //Define como conglomerado espacial
                frmCluster.IsSpatialCluster = true;

                //Habilita o Label "Vizinhança"
                frmCluster.label4.Visible = true;

                //Modifica o tamanho do GroupBox
                //frmCluster.groupBox1.Size = new Size(240, 256);

                //Guarda as variáveis
                string[] strVariaveis = new string[dsDados.Tables[0].Columns.Count];
                for (int i = 0; i < strVariaveis.Length; i++) strVariaveis[i] = dsDados.Tables[0].Columns[i].ColumnName;
                frmCluster.Variaveis = strVariaveis;

                //Abre o Dialog
                frmCluster.ShowDialog();

                double[,] indices = frmCluster.matrizIndicesSegregacao;
                string[] nomes_variaveis = frmCluster.nomesVariaveis;
                string[] nomes_indices = frmCluster.nomesIndicesSegregacao;
                string[] nomesX = frmCluster.strX_;
                string[] nomesY = frmCluster.strY_;

                bool multi = frmCluster.multi;

                bool gerarelatorio = frmCluster.relatoriosegregacao;

                if (frmCluster.DialogResult == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (gerarelatorio == true)
                    {
                        #region Gera o Relatório

                        frmIndicadoresSegregacao indseg = new frmIndicadoresSegregacao();
                        clsReport classeReport = new clsReport();
                        string html = classeReport.SegregacaoRelatorio(indices, nomes_variaveis, nomes_indices, nomesX, nomesY, multi);

                        //Abre o relatório
                        frmRelatorio frmRelatorio = new frmRelatorio();
                        frmRelatorio.isSegregation = true;
                        frmRelatorio.segregacaoValores = indices;
                        frmRelatorio.segregacaoIndices = nomes_indices;
                        frmRelatorio.segregacaoVariaveis = nomes_variaveis;

                        frmRelatorio.MdiParent = this.MdiParent;
                        frmRelatorio.codigoHTML = html;
                        frmRelatorio.Text = "Relatório " + this.Text + ": Índices de Segregacao";

                        frmRelatorio.Show();

                        #endregion
                    }

                    AtualizaPeriodoPainelEspacial();
                    Cursor.Current = Cursors.Default;

                    //sinaliza alteração no mapa temático
                    mapa_alterado = true;
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region controles de tab

        private void tabPage3_Click(object sender, EventArgs e)
        {
            try
            {
                this.mapImage1.Map.ZoomToExtents();
                this.mapImage1.Refresh();
                
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void xpTabControl2_TabIndexChanged(object sender, EventArgs e)
        {            
            try
            {
                if (dsDados.Tables[0].IsInitialized && xpTabControl2.SelectedIndex == 1) toolStripButton7.Checked = false;
                if (dsDados.Tables[0].IsInitialized && xpTabControl2.SelectedIndex == 0) toolStripButton7.Checked = true;
                this.mapImage1.Map.ZoomToExtents();
                this.mapImage1.Update();
                this.map.ZoomToExtents();
                this.mapImage1.Refresh();             
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void tabPage4_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (toolStripButton7.Checked) toolStripButton7.Checked = false;
                this.mapImage1.Map.ZoomToExtents();
                this.mapImage1.Refresh();
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void xpTabControl2_Click(object sender, EventArgs e)
        {
            try
            {
                label1.Visible = false;
                label3.Visible = false;
                if (dsDados.Tables[0].Rows.Count > 0 && xpTabControl2.SelectedIndex == 1) toolStripButton7.Checked = false;
                if (dsDados.Tables[0].Rows.Count > 0 && xpTabControl2.SelectedIndex == 0) toolStripButton7.Checked = true;
                this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;
                this.toolStripButton2.Checked = false;
                this.toolStripButton3.Checked = false;
                this.toolStripButton4.Checked = false;
                this.toolStripButton6.Checked = false;
                this.mapImage1.Refresh();
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }           
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            xpTabControl2.SelectTab(tabPage3);
            label3.Visible = false;
            try
            {
                label1.Text = "Clique em Atualizar para finalizar a seleção";
                label1.Visible = true;

                toolStripButton6.Checked = false;
                tlInformacao.Checked = false;
                moveMapa.Checked = false;
                toolStripButton2.Checked = false;
                toolStripButton3.Checked = false;
                toolStripButton4.Checked = false;

                mapImage1.ActiveTool = (toolStripButton7.Checked) ? SharpMap.Forms.MapImage.Tools.None : SharpMap.Forms.MapImage.Tools.None;
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region controle de data grid

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                int index = dataGridView1.CurrentCell.RowIndex;

                if (linhas_clicadas.Contains(index)) linhas_clicadas.Remove(index);
                else linhas_clicadas.Add(index);
                
                for (int i = 0; i < linhas_clicadas.Count; i++)
                    dataGridView1.Rows[(int)linhas_clicadas[i]].Selected = true;
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
        
        #region manipulação da tabela de dados

        private void calculadoraToolStrip()
        {
            try
            {
                IpeaGeo.RegressoesEspaciais.FormCalculadora frm = new IpeaGeo.RegressoesEspaciais.FormCalculadora();
                frm.Dados = (DataTable)this.dsDados.Tables[0];
                frm.DadosConcatenados = true;
                frm.Shape.ConvertFromIpeaGEOShape(this.shapeAlex);
                frm.PrecisaMatrizWPredefinida = true;
                if (m_matriz_W_esparsa_existente)
                {
                    frm.MatrizWPredefinida = m_matriz_W_esparsa;
                }
                else frm.MatrizWPredefinida = null;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataTable dt = frm.Dados;
                    dt.TableName = dsDados.Tables[0].TableName;

                    this.dsDados.Tables.RemoveAt(0);
                    this.dsDados.Tables.Add(dt.Copy());

                    this.dataGridView1.DataSource = this.dsDados.Tables[0];

                    //sinaliza alteração no mapa temático
                    mapa_alterado = true;
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void calculadoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calculadoraToolStrip();
        }

        private void excluirVariáveisToolStrip()
        {
            try
            {
                IpeaGeo.RegressoesEspaciais.FormCalculadora frm = new IpeaGeo.RegressoesEspaciais.FormCalculadora();
                frm.Dados = (DataTable)this.dsDados.Tables[0];
                frm.AtivaExclusaoVariaveis = true;
                frm.DadosConcatenados = false;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataTable dt = frm.Dados;
                    dt.TableName = dsDados.Tables[0].TableName;

                    this.dsDados.Tables.RemoveAt(0);
                    this.dsDados.Tables.Add(dt.Copy());
                    this.dataGridView1.DataSource = this.dsDados.Tables[0];

                    //sinaliza alteração no mapa temático
                    mapa_alterado = true;
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void excluirVariáveisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            excluirVariáveisToolStrip();
        }

        #region Exportar tabela de dados
        
        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportaDados();
        }

        #endregion

        #endregion

        #region outras variáveis internas

        public string[] variaveis_multi_mapas, metodo_multi_mapas;
        public int[] numclasses_multi_mapas;
        public string endereco_multi_mapas;
        
        #endregion

        #region multimapas

        private void btnMultiMapas_Click(object sender, EventArgs e)
        {
            frmMultiMapas maps = new frmMultiMapas();
            maps.DataTableDados = this.dsDados.Tables[0];
            maps.ShowDialog();

            if (maps.DialogResult == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                //Capturar as variaveis de interesse e pintar o mapa para cada uma delas
                string[] variaveis = maps.varsMapa_;
                string[] metodos = maps.metodosMapa_;
                int[] numclasses = maps.classesMapa_;

                string variaveis_erradas = "Variáveis incompatíveis: ";
                int conta_erradas = 0;
                bool nao_faz = false;

                for (int repeticao = 0; repeticao < variaveis.Length; repeticao++)
                {
                    nao_faz = false;

                    int numClasses = numclasses[repeticao];
                    string cmbVariavel = variaveis[repeticao];
                    string cmbMetodo = metodos[repeticao];

                    int[] classePoligonos = new int[variaveis.Length];
                    string strID = "";

                    // Cópia do mapa temático
                    this.Cursor = Cursors.WaitCursor;
                    double[] desvios = new double[7] { 1, 0.8, 0.6, 0.5, 0.4, 0.2, 0.1 };
                    
                    //Guarda as classes do mapa
                    double[] dblClasseMapa = new double[(int)numClasses];

                    //Tipo do dados
                    string strTipo = dsDados.Tables[0].Columns[cmbVariavel].DataType.ToString();

                    if (strTipo != "System.String")
                    {
                        clsMapa classeMapa = new clsMapa();

                        if (cmbMetodo == "Quantil")
                            classePoligonos = classeMapa.criaQuantis(dsDados.Tables[0], cmbVariavel, strIDmapa, strID, (int)numClasses, ref dblClasseMapa);
                        else if (cmbMetodo == "Jenks")
                            classePoligonos = classeMapa.criaJenks(dsDados.Tables[0], cmbVariavel, strIDmapa, strID, (int)numClasses, ref dblClasseMapa);
                        else if (cmbMetodo == "Desvio Padrão")
                        {
                            int numero_class = 0;
                            classePoligonos = classeMapa.criaDesvios(dsDados.Tables[0], cmbVariavel, strIDmapa, strID, numClasses, ref dblClasseMapa, ref numero_class);
                            numClasses = numero_class;
                        }
                        else if (cmbMetodo == "Geométrico")
                            classePoligonos = classeMapa.criaGeometric(dsDados.Tables[0], cmbVariavel, strIDmapa, strID, (int)numClasses, ref dblClasseMapa);
                        else if (cmbMetodo == "Equal")
                            classePoligonos = classeMapa.criaEqual(dsDados.Tables[0], cmbVariavel, strIDmapa, strID, (int)numClasses, ref dblClasseMapa);
                        else if (cmbMetodo == "Valores Únicos")
                        {
                            int numero_classes = 0;
                            ArrayList valores_diferentes = new ArrayList();
                            for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                                if (!valores_diferentes.Contains(dsDados.Tables[0].Rows[i][cmbVariavel]))
                                    valores_diferentes.Add(dsDados.Tables[0].Rows[i][cmbVariavel]);
                            
                            numero_classes = valores_diferentes.Count;
                            numClasses = numero_classes;
                            dblClasseMapa = new double[numero_classes];

                            classePoligonos = classeMapa.criaValoresUnicos(dsDados.Tables[0], cmbVariavel, strIDmapa, strID, numero_classes, ref dblClasseMapa);
                        }

                        //Tipo de mapa temático
                        string strMetodo = cmbMetodo;

                        //Guarda a variável
                        string strVariavelMapa = cmbVariavel;

                        //Guarda a classe
                        //string classeMapaVetor = ClasseDoMapa;

                        //Cores                                               
                        Color[] coresRGB = new Color[(int)numClasses];
                        Brush[,] coresVetor = maps.coresVetor_;
                        Color[,] coresVetor2 = maps.coresVetor2_;                         

                        //Aqui termina a geração das classes e cores. Precisamos pintar o mapa e exportar
                        //Capturar o mapa
                        frmMapa mapa = new frmMapa();

                        //sinaliza alteração no mapa temático
                        mapa_alterado = true;
                    }
                    else
                    {
                        variaveis_erradas += cmbVariavel + ", ";
                        conta_erradas++;
                        nao_faz = true;
                    }
                     
                    this.Cursor = Cursors.Default;

                    if (!nao_faz)
                    {
                        //Captura o mapa
                        SharpMap.Map mMapa = mapImage1.Map;

                        //Captura o layer
                        SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[this.Name];

                        //Cria o vetor temático
                        int[] iVetor = new int[dsDados.Tables[0].Rows.Count];

                        //Guarda as classes
                        dblClasses = dblClasseMapa;

                        //Guarda o vetor temático
                        iVetor = classePoligonos;

                        //Inicializa as cores
                        //Brush[] cores = frmMapa.CoresParaMapa;                         
                        Brush[] cores = maps.CoresParaMapa;
                        string[] strCoresRGB = maps.CoresRGB;

                        //Guarda os vetores
                        ArrayList vetorID = new ArrayList();
                        int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];

                        for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                        {
                            vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                            vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];
                        }

                        //Pinta o mapa
                        mTheme meuTema = new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice);
                        layMapa.Theme = new SharpMap.Rendering.Thematics.CustomTheme(meuTema.GetStyle);

                        //Refresh
                        mMapa.ZoomToExtents();

                        //Refresh o mapa
                        mapImage1.Refresh();

                        //Gera relatório
                        //Coloca o fundo branco
                        mapImage1.Map.BackColor = Color.White;

                        //Gera o mapa
                        System.Drawing.Bitmap img = new System.Drawing.Bitmap(mapImage1.Width, mapImage1.Height);
                        mapImage1.DrawToBitmap(img, mapImage1.ClientRectangle);

                        string end = maps.endereco_;
                        int indice = end.LastIndexOf(".");
                        string enderecofinal = end.Substring(0, indice);
                        string enderecoimagem = enderecofinal + ".jpeg";
                        string sufixo = end.Substring(indice);
                        enderecofinal = enderecofinal + "_" + variaveis[repeticao] + "_" + cmbMetodo + sufixo;

                        indice = enderecofinal.IndexOf(".jpeg");
                        if (indice != -1)
                        {
                            //Salva o mapa
                            img.Save(enderecofinal, ImageFormat.Jpeg);
                             
                            //Criando um bitmap grande com tamanho maior
                            Bitmap background = new Bitmap(945, 660);
                             
                            //Criando um objeto graphics sobre esta imagem
                            Graphics grfx = Graphics.FromImage(background);
                            Brush pincel = new SolidBrush(Color.White);

                            //Coloca o fundo branco
                            grfx.FillRectangle(pincel, 0, 0, 945, 660);

                            //Insere o mapa
                            grfx.DrawImage(img, 200, 50);                            

                            StringFormat formato = new StringFormat();
                            PointF pontos = new PointF(250, 0);
                            PointF pontos2 = new PointF(250, 20);
                            Font fonte = new Font("Times", 16, FontStyle.Bold);
                            Font fonte2 = new Font("Times", 12);
                            Brush pincel2 = new SolidBrush(Color.Black);

                            //Insere texto na imagem
                            grfx.DrawString("Variável: " + cmbVariavel, fonte, pincel2, pontos);
                            grfx.DrawString("Metodologia: " + cmbMetodo, fonte, pincel2, pontos2);
                             
                            Pen caneta=new Pen(Color.Black);

                            //Insere uma legenda, dependendo do número de classes (mais de 10 e teremos apenas uma escala de cores)
                            int fator_de_aumento = 45;
                            int fator_max = 0;
                            if (dblClasses.Length < 11)
                            {
                                for (int i = 0; i < dblClasses.Length; i++)
                                {
                                    string totalcor = strCoresRGB[i].Substring(1); //Retirei o #
                                    string R = totalcor.Substring(0, 2);
                                    string G = totalcor.Substring(2, 2);
                                    string B = totalcor.Substring(4, 2);

                                    int Rint = Convert.ToInt32(R, 16); //Aqui convertemos da base hexadecimal
                                    int Gint = Convert.ToInt32(G, 16);
                                    int Bint = Convert.ToInt32(B, 16);

                                    Color mycolor = Color.FromArgb(Rint, Gint, Bint);
                                    Brush pincel_legenda = new SolidBrush(mycolor);

                                    int fator = fator_de_aumento * i;

                                    //Precismos pintar um retangulo de cor e colocar, na esquerda, o intervalo de classes
                                    grfx.FillRectangle(pincel_legenda, 10, 580 - fator, 40, 40);

                                    pontos = new PointF(60, 590 - fator);
                                    grfx.DrawString(dblClasses[i].ToString(), fonte2, pincel2, pontos);

                                    fator_max = fator;
                                }

                                pontos = new PointF(10, 545 - fator_max);
                                grfx.DrawString("Legenda (Limites superiores)", fonte2, pincel2, pontos);
                            }
                            else
                            {
                                string totalcor = strCoresRGB[0].Substring(1); //Retirei o #
                                string R = totalcor.Substring(0, 2);
                                string G = totalcor.Substring(2, 2);
                                string B = totalcor.Substring(4, 2);

                                int Rint = Convert.ToInt32(R, 16); //Aqui convertemos da base hexadecimal
                                int Gint = Convert.ToInt32(G, 16);
                                int Bint = Convert.ToInt32(B, 16);

                                Color mycolor1 = Color.FromArgb(Rint, Gint, Bint);

                                totalcor = strCoresRGB[strCoresRGB.Length-1].Substring(1); //Retirei o #
                                R = totalcor.Substring(0, 2);
                                G = totalcor.Substring(2, 2);
                                B = totalcor.Substring(4, 2);

                                Rint = Convert.ToInt32(R, 16); //Aqui convertemos da base hexadecimal
                                Gint = Convert.ToInt32(G, 16);
                                Bint = Convert.ToInt32(B, 16);

                                Color mycolor2 = Color.FromArgb(Rint, Gint, Bint);

                                PointF pontof=new PointF(20,400);
                                SizeF tam=new SizeF(50,200);

                                //RectangleF rect=new RectangleF(pontof,tam);
                                Rectangle rectang = new Rectangle(20, 400, 50, 200);
                                LinearGradientBrush backBrush = new LinearGradientBrush(rectang, mycolor1, mycolor2, LinearGradientMode.Vertical);
                                Pen lapis=new Pen(backBrush);                                 

                                grfx.FillRectangle(backBrush, rectang);
                                pontos = new PointF(20, 370);
                                grfx.DrawString("Legenda", fonte2, pincel2, pontos);

                                pontos = new PointF(80, 400);
                                grfx.DrawString(dblClasses[0].ToString(), fonte2, pincel2, pontos);

                                pontos = new PointF(80, 580);
                                grfx.DrawString(dblClasses[dblClasses.Length-1].ToString(), fonte2, pincel2, pontos);
                            }

                            //Salva a imagem alterada
                            background.Save(enderecofinal, ImageFormat.Jpeg);
                        }

                        indice = enderecofinal.IndexOf(".pdf");
                        if (indice != -1)
                        {
                            //Salvar em pdf
                            IpeaGeo.Classes.clsReportPDF pdf = new IpeaGeo.Classes.clsReportPDF();
                            img.Save(enderecoimagem, ImageFormat.Jpeg);
                            pdf.RelatorioPDF_MapaTematicoMM(enderecofinal, enderecoimagem, cmbMetodo, dblClasses, strCoresRGB, cmbVariavel);
                        }
                    }
                }

                if (conta_erradas == 0)
                    MessageBox.Show("Operação realizada com sucesso");
                else
                {
                    int index = variaveis_erradas.LastIndexOf(",");
                    variaveis_erradas = variaveis_erradas.Remove(index);
                    MessageBox.Show("Operação realizada. " + conta_erradas + " mapa(s) não puderam ser feitos." + "\n" + variaveis_erradas);
                }
            }
        }

        #endregion

        #warning Agrupar os métodos semelhantes
        #region ferramentas de análise

        private void análiseDeComponentesPrincipaisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGeo.Modelagem.ComponentesPrincipais.FormComponentesPrincipais frm = new IpeaGeo.Modelagem.ComponentesPrincipais.FormComponentesPrincipais(); 
                frm.MdiParent = this.MdiParent;
                frm.TabelaDeDados = this.dsDados.Tables[0].Copy();
                frm.HabilitarDadosExternos();
                frm.LabelTabelaDados = this.Text;
                frm.GridViewExterno = dataGridView1;
                frm.DataSetExterno = this.dsDados;
                frm.Show();
                
                AtualizaPeriodoPainelEspacial();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void kMeansToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGeo.Modelagem.K_means frm = new IpeaGeo.Modelagem.K_means();
                frm.MdiParent = this.MdiParent;
                frm.TabelaDeDados = this.dsDados.Tables[0].Copy();
                frm.HabilitarDadosExternos();
                frm.LabelTabelaDados = this.Text;
                frm.GridViewExterno = dataGridView1;
                frm.DataSetExterno = this.dsDados;
                frm.Show();

                AtualizaPeriodoPainelEspacial();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EstatDescritivas()
        {
            try
            {
                if (((DataTable)this.dataGridView1.DataSource).Rows.Count > 0 && ((DataTable)this.dataGridView1.DataSource).Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormEstatisticasDescritivas frm = new IpeaGeo.RegressoesEspaciais.FormEstatisticasDescritivas();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = (DataTable)this.dataGridView1.DataSource;
                    if (frm.TabelaDados.Rows.Count == this.dsDados.Tables[0].Rows.Count)
                        frm.Text = "Estatísticas Descritivas - [" + this.Name + "]";
                    else
                        frm.Text = "Estatísticas Descritivas - [" + this.Name + " (filtrado)]";
                    
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

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            EstatDescritivas();
        }

        private void TabelasFrequencias()
        {
            try
            {
                if (((DataTable)this.dataGridView1.DataSource).Rows.Count > 0 && ((DataTable)this.dataGridView1.DataSource).Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormTabelaFrequencias frm = new FormTabelaFrequencias();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = (DataTable)this.dataGridView1.DataSource;
                    if (frm.TabelaDados.Rows.Count == this.dsDados.Tables[0].Rows.Count)
                        frm.Text = "Tabela de Frequências - [" + this.Name + "]";
                    else
                        frm.Text = "Tabela de Frequências - [" + this.Name + " (filtrado)]";
                    
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

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            TabelasFrequencias();
        }

        private void TabulacoesCruzadas()
        {
            try
            {
                if (((DataTable)this.dataGridView1.DataSource).Rows.Count > 0 && ((DataTable)this.dataGridView1.DataSource).Columns.Count > 0)
                {                   
                    IpeaGeo.RegressoesEspaciais.FormTabelasCruzadas frm = new FormTabelasCruzadas();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = (DataTable)this.dataGridView1.DataSource;
                    if (frm.TabelaDados.Rows.Count == this.dsDados.Tables[0].Rows.Count)
                        frm.Text = "Tabela Cruzadas - [" + this.Name + "]";
                    else
                        frm.Text = "Tabela Cruzadas - [" + this.Name + " (filtrado)]";
                    
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

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            TabulacoesCruzadas();
        }

        private void CorrelacoestoolStrip()
        {
            try
            {
                if (((DataTable)this.dataGridView1.DataSource).Rows.Count > 0 && ((DataTable)this.dataGridView1.DataSource).Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormCorrelacoes frm = new IpeaGeo.RegressoesEspaciais.FormCorrelacoes();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = (DataTable)this.dataGridView1.DataSource;
                    if (frm.TabelaDados.Rows.Count == this.dsDados.Tables[0].Rows.Count)
                        frm.Text = "Correlações - [" + this.Name + "]";
                    else
                        frm.Text = "Correlações - [" + this.Name + " (filtrado)]";
                    
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

        private void toolStripMenuItem5_Click(object sender, EventArgs e)

        {
            CorrelacoestoolStrip();
        }

        private void crossSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGeo.RegressoesEspaciais.FormRegressaoGMMConley frm = new IpeaGeo.RegressoesEspaciais.FormRegressaoGMMConley();
                frm.MdiParent = this.MdiParent;
                frm.TabelaDeDados = this.dsDados.Tables[0].Copy();
                frm.HabilitarDadosExternos();
                frm.LabelTabelaDados = this.Text;
                frm.GridViewExterno = dataGridView1;
                frm.DataSetExterno = this.dsDados;
                
                //Se matriz de vizinhança já especificada
                if (m_matriz_W_esparsa_existente)
                {
                    frm.MatrizWEsparsaPredefinida = m_matriz_W_esparsa;
                    frm.TipoMatrizVizinhancaPredefinida = m_tipo_matriz_vizinhanca;
                    frm.OrdemMatrizVizinhancaPredefinida = m_ordem_matriz_vizinhanca;
                    frm.MatrizWNormalizadaPredefinida = m_matriz_W_normalizada;
                    frm.UsaWSparsaPredefinida = m_matriz_W_esparsa_existente;
                }

                IpeaGeo.RegressoesEspaciais.clsIpeaShape shape_modelagem = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
                shape_modelagem.ConvertFromIpeaGEOShape(this.shapeAlex);

                frm.Shape = shape_modelagem;
                frm.Show();

                AtualizaPeriodoPainelEspacial();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private DataTable m_tabela_dados_original = new DataTable();

        private void recuperarTabelaOriginal()
        { 
            try
            {
                if (!m_usa_dados_painel_espacial)
                {
                    dsDados.Tables.Clear();
                    dsDados.Tables.Add(m_tabela_dados_original);
                    dataGridView1.DataSource = (DataTable)dsDados.Tables[0];
                }
                else
                {
                    m_tabela_dados_original = ((DataTable)m_ds_dados_originais_painei_espacial.Tables[this.m_periodo_painel_espacial]).Copy();

                    dsDados.Tables.Clear();
                    dsDados.Tables.Add(m_tabela_dados_original);

                    dataGridView1.DataSource = (DataTable)dsDados.Tables[0];

                    m_ds_dados_painel_espacial.Tables.Remove(m_periodo_painel_espacial);
                    m_ds_dados_painel_espacial.Tables.Add(m_tabela_dados_original);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }           
        
        }

        private void recuperarTabelaOriginalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            recuperarTabelaOriginal();
        }

        private void cálculoDeTaxasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGeo.Modelagem.FormCalculoTaxas frm = new IpeaGeo.Modelagem.FormCalculoTaxas();
                frm.MdiParent = this.MdiParent;
                frm.TabelaDeDados = this.dsDados.Tables[0].Copy();
                frm.HabilitarDadosExternos();
                frm.LabelTabelaDados = this.Text;
                frm.GridViewExterno = dataGridView1;
                frm.DataSetExterno = this.dsDados;

                IpeaGeo.RegressoesEspaciais.clsIpeaShape shape_modelagem = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
                shape_modelagem.ConvertFromIpeaGEOShape(this.shapeAlex);

                frm.Shape = this.shapeAlex;
                frm.Show();

                AtualizaPeriodoPainelEspacial();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void regressãoLinearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGeo.Modelagem.FormLinearRegression frm = new IpeaGeo.Modelagem.FormLinearRegression();
                frm.MdiParent = this.MdiParent;
                frm.TabelaDeDados = this.dsDados.Tables[0].Copy();
                frm.HabilitarDadosExternos();
                frm.LabelTabelaDados = this.Text;
                frm.GridViewExterno = dataGridView1;
                frm.DataSetExterno = this.dsDados;
                frm.Show();

                AtualizaPeriodoPainelEspacial();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void regressãoComDadosBináriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGeo.Modelagem.FormRegressaoDadosBinarios frm = new IpeaGeo.Modelagem.FormRegressaoDadosBinarios();
                frm.MdiParent = this.MdiParent;
                frm.TabelaDeDados = this.dsDados.Tables[0].Copy();
                frm.HabilitarDadosExternos();
                frm.LabelTabelaDados = this.Text;
                frm.GridViewExterno = dataGridView1;
                frm.DataSetExterno = this.dsDados;
                frm.Show();

                AtualizaPeriodoPainelEspacial();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void propensityScoreMatchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGeo.Modelagem.FormPropensityScore frm = new IpeaGeo.Modelagem.FormPropensityScore();
                frm.MdiParent = this.MdiParent;
                frm.TabelaDeDados = this.dsDados.Tables[0].Copy();
                frm.HabilitarDadosExternos();
                frm.LabelTabelaDados = this.Text;
                frm.GridViewExterno = dataGridView1;
                frm.DataSetExterno = this.dsDados;
                frm.Show();

                AtualizaPeriodoPainelEspacial();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void descriçãoDosDadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dsDados.Tables[0].Rows.Count > 0 && this.dsDados.Tables[0].Columns.Count > 0)
                {
                    IpeaGeo.RegressoesEspaciais.FormEstatisticasDescritivas frm = new IpeaGeo.RegressoesEspaciais.FormEstatisticasDescritivas();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDados = this.dsDados.Tables[0];
                    frm.Show();

                    AtualizaPeriodoPainelEspacial();
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

        private void distribuiçõesContínuasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGeo.Modelagem.FormDistribuicoesParametrics frm = new IpeaGeo.Modelagem.FormDistribuicoesParametrics();
                frm.MdiParent = this.MdiParent;
                frm.TabelaDeDados = this.dsDados.Tables[0];
                frm.Show();

                AtualizaPeriodoPainelEspacial();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void distribuiçõesDiscretasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGeo.Modelagem.FormDistribuicoesDiscretas frm = new IpeaGeo.Modelagem.FormDistribuicoesDiscretas();
                frm.MdiParent = this.MdiParent;
                frm.TabelaDeDados = this.dsDados.Tables[0];
                frm.Show();

                AtualizaPeriodoPainelEspacial();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region mapas temáticos
        
        private double[] m_cortes_classes_quantitativas = new double[0];
        private string[] m_valores_classes_qualitativas = new string[0];
        private string m_variavel_mapa_tematico = "";
        private string m_tipo_mapa_tematico = "quantitativo";
        private Brush[] m_cores_mapa_tematico = new Brush[0];
        private Color[] m_cores = new Color[0];
        private string[] m_legendas_mapa_tematico = new string[0];
        private string m_metodo = "";
        private int index;
        private void AjustaMapaTematicoPainel()
        {
            try
            {
                if (ckbAtualizarMapaTematico.Checked && m_variavel_mapa_tematico != "" && ((DataTable)dsDados.Tables[0]).Columns.Contains(m_variavel_mapa_tematico))
                {
                    if (m_tipo_mapa_tematico == "quantitativo")
                    {
                        #region atualização do mapa temático para variáveis quantitativas 

                        clsUtilTools clt = new clsUtilTools();
                        double[,] valores = clt.GetMatrizFromDataTable((DataTable)dsDados.Tables[0], m_variavel_mapa_tematico);

                        int[] novoVetor = new int[valores.GetLength(0)];
                        for (int i = 0; i < novoVetor.GetLength(0); i++)
                        {
                            novoVetor[i] = m_cortes_classes_quantitativas.GetLength(0) - 1;
                            for (int k = 0; k < m_cortes_classes_quantitativas.GetLength(0) - 1; k++)
                                if (valores[i, 0] <= m_cortes_classes_quantitativas[k])
                                {
                                    novoVetor[i] = k;
                                    break;
                                }
                        }
                        
                        int[] iVetor = novoVetor;
                        Brush[] cores = m_cores_mapa_tematico;

                        //Guarda os vetores
                        ArrayList vetorID = new ArrayList();
                        int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];

                        Hashtable[] ht = (Hashtable[])(m_caracteristicas_itens_layers[this.Name]);

                        for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                        {
                            vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                            vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];

                            // guardando os resultados no hashtable de características dos polígonos
                            (ht[i])["Fill"] = (Brush)cores[iVetor[i]];
                        }

                        //Captura o mapa
                        SharpMap.Map mMapa = mapImage1.Map;

                        //Captura o layer
                        SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[this.Name];

                        LayerPrincipalNoTopo();

                        //Pinta o mapa
                        mTheme meuTema = new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice);
                        layMapa.Theme = new SharpMap.Rendering.Thematics.CustomTheme(meuTema.GetStyle);

                        if (dsDados.Tables[0].Columns.Contains("MapaTematico") == false) dsDados.Tables[0].Columns.Add("MapaTematico");
                        for (int i = 0; i < iVetor.Length; i++)
                            dsDados.Tables[0].Rows[i]["MapaTematico"] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]];

                        //Refresh
                        mMapa.ZoomToExtents();

                        //Refresh o mapa
                        mapImage1.Refresh();

                        this.iVetor = iVetor;

                        #endregion
                    }
                    else
                    {
                        #region atualização do mapa temático para variáveis qualitativas 

                        clsUtilTools clt = new clsUtilTools();
                        object[,] valores = clt.GetObjMatrizFromDataTable((DataTable)dsDados.Tables[0], m_variavel_mapa_tematico);

                        int[] novoVetor = new int[valores.GetLength(0)];
                        for (int i = 0; i < novoVetor.GetLength(0); i++)
                            for (int k = 0; k < m_valores_classes_qualitativas.GetLength(0); k++)
                                if (valores[i, 0].ToString() == m_valores_classes_qualitativas[k].ToString())
                                {
                                    novoVetor[i] = k;
                                    break;
                                }
                        
                        int[] iVetor = novoVetor;
                        Brush[] cores = m_cores_mapa_tematico;

                        //Guarda os vetores
                        ArrayList vetorID = new ArrayList();
                        int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];

                        Hashtable[] ht = (Hashtable[])(m_caracteristicas_itens_layers[this.Name]);

                        for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                        {
                            vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                            vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];

                            // guardando os resultados no hashtable de características dos polígonos
                            (ht[i])["Fill"] = (Brush)cores[iVetor[i]];
                        }

                        //Captura o mapa
                        SharpMap.Map mMapa = mapImage1.Map;

                        //Captura o layer
                        SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[this.Name];

                        LayerPrincipalNoTopo();

                        //Pinta o mapa
                        mTheme meuTema = new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice);
                        layMapa.Theme = new SharpMap.Rendering.Thematics.CustomTheme(meuTema.GetStyle);

                        if (dsDados.Tables[0].Columns.Contains("MapaTematico") == false) dsDados.Tables[0].Columns.Add("MapaTematico");
                        for (int i = 0; i < iVetor.Length; i++)
                            dsDados.Tables[0].Rows[i]["MapaTematico"] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]];

                        //Refresh
                        mMapa.ZoomToExtents();

                        //Refresh o mapa
                        mapImage1.Refresh();

                        this.iVetor = iVetor;

                        #endregion
                    }
                }
                else LimpaMapaTematico(false);
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #region Mapa temático para variáveis quantitativas 

        bool mapa_tem;

        private void variáveisQuantitativasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Quantitativo
                bool quantitativo;
                if (tab_mod == false)
                {
                    quantitativo = mapa_tem = true;
                }
                else 
                {
                    quantitativo = mapa_tem;
                }

                //Formulário Pai
                m_mdiparent = (IpeaGeo.IPEAGEOMDIParent)this.MdiParent;

                //Abre a conexão
                frmMapaTematico frmMapa = new frmMapaTematico();

                //Aplica os Idenfificadores
                frmMapa.IdentificadorDados = strIDbase;
                frmMapa.IdentificadorMapa = strIDmapa;
                frmMapa.numeroClasses_ = shapeAlex.Count;
                frmMapa.BoolQuantitativo = quantitativo;
                
                //Aplica o DataTable
                DataTable dTable = dsDados.Tables[0];

                frmMapa.DataTableDados = dTable;
                Classes.clsArmazenamentoDados armazena = new Classes.clsArmazenamentoDados();
                if (armazena.Leitura_efetuada == true)
                    frmMapa.XML();
                else frmMapa.ShowDialog();

                if (frmMapa.DialogResult == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    
                    m_tipo_mapa_tematico = "quantitativo";
                    m_variavel_mapa_tematico = frmMapa.VariavelEscolhida;

                    //Captura o mapa
                    SharpMap.Map mMapa = mapImage1.Map;

                    //Captura o layer
                    SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[this.Name];

                    //Cria o vetor temático
                    iVetor = new int[dsDados.Tables[0].Rows.Count];

                    //Guarda as classes
                    dblClasses = frmMapa.ClasseDoMapa;
                    m_cortes_classes_quantitativas = dblClasses;

                    //Guarda o vetor temático
                    iVetor = frmMapa.vetorPoligonos;

                    //Guarda a legenda do mapa tematico
                    m_legendas_mapa_tematico = frmMapa.legendas;

                    //Guarda Metodo utilizado 
                    m_metodo = frmMapa.metodo;

                    //Guarda indice da cor selecionada
                    index = frmMapa.indice;

                    //Inicializa as cores
                    Brush[] cores = frmMapa.CoresParaMapa;
                    m_cores_mapa_tematico = cores;
                    m_cores = frmMapa.CoresRGBColors;

                    //Guarda os vetores
                    ArrayList vetorID = new ArrayList();
                    int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];

                    Hashtable[] ht = (Hashtable[])(m_caracteristicas_itens_layers[this.Name]);

                    for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                    {
                        vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                        vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];

                        // guardando os resultados no hashtable de características dos polígonos
                        (ht[i])["Fill"] = (Brush)cores[iVetor[i]];
                    }

                    //LayerPrincipalNoTopo();

                    //Pinta o mapa
                    mTheme meuTema = new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice);
                    layMapa.Theme = new SharpMap.Rendering.Thematics.CustomTheme(meuTema.GetStyle);
                    if (frmMapa.GuardaClassificacao == true)
                    {
                        if (dsDados.Tables[0].Columns.Contains("MapaTematico") == false) dsDados.Tables[0].Columns.Add("MapaTematico");
                        for (int i = 0; i < iVetor.Length; i++)
                            dsDados.Tables[0].Rows[i]["MapaTematico"] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]];
                    }

                    //Refresh
                    mMapa.ZoomToExtents();

                    //Refresh o mapa
                    mapImage1.Refresh();

                    //Gera relatório
                    if (frmMapa.GeraRelatorio == true)
                    {   
                        //Gera o mapa
                        System.Drawing.Bitmap img = new System.Drawing.Bitmap(mapImage1.Width, mapImage1.Height);
                        mapImage1.DrawToBitmap(img, mapImage1.ClientRectangle);
                        
                        //this.Dock = DockStyle.Fill;
                        //this.Dock = DockStyle.None;

                        //Coloca o fundo branco
                        mapImage1.Map.BackColor = Color.White;

                        //Salva o mapa
                        img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);
                        string[] legenda = frmMapa.Legenda_MapaTem;

                        clsReport classeReport = new clsReport();
                        string html = null;
                        if (legenda != null)
                        {
                            html = classeReport.MapaTematicoRelatorio(strEnderecoBase, strEnderecoMapa,
                                Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count, frmMapa.Metodologia,
                                frmMapa.ClasseDoMapa, frmMapa.CoresRGB, frmMapa.Variavel, legenda);
                        }
                        else
                        {
                            html = classeReport.MapaTematicoRelatorio(strEnderecoBase, strEnderecoMapa,
                                Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count, frmMapa.Metodologia,
                                frmMapa.ClasseDoMapa, frmMapa.CoresRGB, frmMapa.Variavel);
                        }

                        //Abre o relatório
                        frmRelatorio frmRelatorio = new frmRelatorio();
                        frmRelatorio.isThematicMap = true;
                        frmRelatorio.legendaMapaTematico = legenda;
                        frmRelatorio.enderecoBase = strEnderecoBase;
                        frmRelatorio.enderecoMapa = strEnderecoMapa;
                        frmRelatorio.endMapa = Application.StartupPath + "\\Mapa.jpeg";
                        frmRelatorio.shapeCount = shapeAlex.Count;
                        frmRelatorio.metodo = frmMapa.Metodologia;
                        frmRelatorio.classesMapaReg = frmMapa.ClasseDoMapa;
                        frmRelatorio.coresMapaReg = frmMapa.CoresRGB;
                        frmRelatorio.varMapa = frmMapa.Variavel;

                        frmRelatorio.MdiParent = this.MdiParent;
                        frmRelatorio.codigoHTML = html;
                        frmRelatorio.Text = "Relatório " + this.Text + ": " + frmMapa.Variavel;

                        string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count + 1, 2];
                        strClusterMapa[0, 0] = strIDbase;
                        strClusterMapa[0, 1] = "Classe";

                        for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                        {
                            string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                            string stCl = dsDados.Tables[0].Rows[i]["MapaTematico"].ToString();
                            strClusterMapa[i + 1, 0] = stId;
                            strClusterMapa[i + 1, 1] = stCl;
                        }

                        frmRelatorio.variaveisMapa = strClusterMapa;
                        frmRelatorio.Show();
                    }

                    //Refresh
                    mMapa.ZoomToExtents();

                    //Refresh o mapa
                    mapImage1.Refresh();
                    Cursor.Current = Cursors.Default;

                    //Preenche a legenda do Mapa
                    if (dataGridView2.ColumnCount == 0)
                    {
                        //Adiciona cor
                        DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                        txtCor = new DataGridViewTextBoxColumn();
                        txtCor.Width = 36;
                        txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor.ReadOnly = true;
                        txtCor.HeaderText = "Cor";
                        txtCor.DisplayIndex = 2;
                        txtCor.DefaultCellStyle.BackColor = Color.White;
                        dataGridView2.Columns.Insert(0, txtCor);

                        //Adiciona Legenda
                        DataGridViewTextBoxColumn txtLegData = new DataGridViewTextBoxColumn();
                        txtLegData = new DataGridViewTextBoxColumn();
                        txtLegData.Width = 130;
                        txtLegData.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtLegData.ReadOnly = false;
                        txtLegData.HeaderText = "Legenda";
                        txtLegData.DisplayIndex = 2;
                        dataGridView2.Columns.Insert(1, txtLegData);

                        //Adiciona Limite Superior
                        DataGridViewCheckBoxColumn chkSelecionar = new DataGridViewCheckBoxColumn();
                        chkSelecionar = new DataGridViewCheckBoxColumn();
                        chkSelecionar.Width = 70;
                        chkSelecionar.HeaderText = "Selecionar";
                        chkSelecionar.DisplayIndex = 2;
                        dataGridView2.Columns.Insert(2, chkSelecionar);
                    }

                    if (frmMapa.Legenda_MapaTem[0] == null)
                        for (int i = 0; i < dblClasses.Length; i++)
                            frmMapa.Legenda_MapaTem[i] = "Classe " + i;

                    if (dataGridView2.Rows.Count > frmMapa.Legenda_MapaTem.GetLength(0))
                        dataGridView2.Rows.Clear();

                    for (int i = 0; i < dblClasses.Length; i++)
                    {
                        if (dataGridView2.Rows.Count < dblClasses.Length) dataGridView2.Rows.Add();
                        dataGridView2.Rows[i].Cells[0].Style.BackColor = frmMapa.CoresRGBColors[i];
                        dataGridView2.Rows[i].Cells[1].Value = frmMapa.Legenda_MapaTem[i];
                        dataGridView2.Rows[i].Cells[1].ToolTipText = frmMapa.Legenda_MapaTem[i];
                        dataGridView2.Rows[i].Cells[2].Value = false;
                    }

                    xpTabControl2.SelectedTab = tabPage3;
                    xpTabControl1.SelectedTab = tabPage5;
                }

                txtVarMapaTematico.Text = frmMapa.VariavelEscolhida;
                dataGridView2.ClearSelection();
                RemoverSelecoes();

                Classes.clsArmazenamentoDados salvar = new Classes.clsArmazenamentoDados();
                if (salvar.Leitura_efetuada == true)
                {
                    this.mapImage1.Map.Zoom = salvar.zoom_m;
                    this.mapImage1.Map.Center = new SharpMap.Geometries.Point(salvar.centro_map[0], salvar.centro_map[1]);
                    this.mapImage1.Refresh();
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion 

        #region Mapa temático para variáveis categóricas 

        private void variáveisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                bool quantitativo = mapa_tem = false;
                //Formulário Pai
                m_mdiparent = (IpeaGeo.IPEAGEOMDIParent)this.MdiParent;

                //Abre a conexão
                frmMapaTematico frmMapa = new frmMapaTematico();

                //Aplica os Idenfificadores
                frmMapa.IdentificadorDados = strIDbase;
                frmMapa.IdentificadorMapa = strIDmapa;
                frmMapa.numeroClasses_ = shapeAlex.Count;
                frmMapa.BoolQuantitativo = quantitativo;

                //Aplica o DataTable
                DataTable dTable = dsDados.Tables[0];

                frmMapa.DataTableDados = dTable;

                //Abre o Dialog
                frmMapa.ShowDialog();

                if (frmMapa.DialogResult == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    m_tipo_mapa_tematico = "qualitativo";
                    m_variavel_mapa_tematico = frmMapa.VariavelEscolhida;                    

                    //Captura o mapa
                    SharpMap.Map mMapa = mapImage1.Map;

                    //Captura o layer
                    SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[this.Name];

                    //Cria o vetor temático
                    iVetor = new int[dsDados.Tables[0].Rows.Count];

                    //Guarda as classes
                    dblClasses = frmMapa.ClasseDoMapa;

                    m_valores_classes_qualitativas = frmMapa.strClasseDoMapa;

                    //Guarda o vetor temático
                    iVetor = frmMapa.vetorPoligonos;

                    //Guarda a legenda do mapa tematico
                    m_legendas_mapa_tematico = frmMapa.Legenda_MapaTem;

                    //Guarda Metodo utilizado 
                    m_metodo = frmMapa.metodo;

                    //Guarda indice da cor selecionada
                    index = frmMapa.indice;

                    //Inicializa as cores
                    Brush[] cores = frmMapa.CoresParaMapa;
                    m_cores_mapa_tematico = cores;
                    m_cores = frmMapa.CoresRGBColors;
                    
                    Hashtable[] ht = (Hashtable[])(m_caracteristicas_itens_layers[this.Name]);
                    
                    //Guarda os vetores
                    ArrayList vetorID = new ArrayList();
                    int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];

                    for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                    {
                        vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                        vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];

                        // guardando os resultados no hashtable de características dos polígonos
                        (ht[i])["Fill"] = (Brush)cores[iVetor[i]];
                    }
                    
                    LayerPrincipalNoTopo();

                    //Pinta o mapa
                    mTheme meuTema = new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice);
                    layMapa.Theme = new SharpMap.Rendering.Thematics.CustomTheme(meuTema.GetStyle);
                    if (frmMapa.GuardaClassificacao == true)
                    {
                        if (dsDados.Tables[0].Columns.Contains("MapaTematico") == false) dsDados.Tables[0].Columns.Add("MapaTematico");
                        for (int i = 0; i < iVetor.Length; i++)
                            dsDados.Tables[0].Rows[i]["MapaTematico"] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]];
                    }

                    //Refresh
                    mMapa.ZoomToExtents();

                    //Refresh o mapa
                    mapImage1.Refresh();

                    //Gera relatório
                    if (frmMapa.GeraRelatorio == true)
                    {
                        //this.Dock = DockStyle.Fill;
                        //this.Dock = DockStyle.None;

                        //Coloca o fundo branco
                        mapImage1.Map.BackColor = Color.White;

                        //Gera o mapa
                        System.Drawing.Bitmap img = new System.Drawing.Bitmap(mapImage1.Width, mapImage1.Height);
                        mapImage1.DrawToBitmap(img, mapImage1.ClientRectangle);

                        //Salva o mapa
                        img.Save(Application.StartupPath + "\\Mapa.jpeg", ImageFormat.Jpeg);
                        string[] legenda = frmMapa.Legenda_MapaTem;

                        clsReport classeReport = new clsReport();
                        string html = null;
                        if (legenda[0] != null)
                        {
                            html = classeReport.MapaTematicoQualiRelatorio(strEnderecoBase, strEnderecoMapa,
                                Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count, frmMapa.Metodologia,
                                frmMapa.strClasseDoMapa, frmMapa.CoresRGB, frmMapa.Variavel, legenda);
                        }
                        else
                        {
                            html = classeReport.MapaTematicoQualiRelatorio(strEnderecoBase, strEnderecoMapa,
                                Application.StartupPath + "\\Mapa.jpeg", shapeAlex.Count, frmMapa.Metodologia,
                                frmMapa.strClasseDoMapa, frmMapa.CoresRGB, frmMapa.Variavel);
                        }

                        //Abre o relatório
                        frmRelatorio frmRelatorio = new frmRelatorio();
                        frmRelatorio.isThematicMap = true;
                        frmRelatorio.legendaMapaTematico = legenda;

                        frmRelatorio.enderecoBase = strEnderecoBase;
                        frmRelatorio.enderecoMapa = strEnderecoMapa;
                        frmRelatorio.endMapa = Application.StartupPath + "\\Mapa.jpeg";
                        frmRelatorio.shapeCount = shapeAlex.Count;
                        frmRelatorio.metodo = frmMapa.Metodologia;
                        frmRelatorio.classesMapaReg = frmMapa.ClasseDoMapa;
                        frmRelatorio.coresMapaReg = frmMapa.CoresRGB;
                        frmRelatorio.varMapa = frmMapa.Variavel;

                        frmRelatorio.MdiParent = this.MdiParent;
                        frmRelatorio.codigoHTML = html;
                        frmRelatorio.Text = "Relatório " + this.Text + ": " + frmMapa.Variavel;

                        string[,] strClusterMapa = new string[dsDados.Tables[0].Rows.Count + 1, 2];
                        strClusterMapa[0, 0] = strIDbase;
                        strClusterMapa[0, 1] = "Classe";

                        for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                        {
                            string stId = dsDados.Tables[0].Rows[i][strIDbase].ToString();
                            string stCl = dsDados.Tables[0].Rows[i]["MapaTematico"].ToString();
                            strClusterMapa[i + 1, 0] = stId;
                            strClusterMapa[i + 1, 1] = stCl;
                        }

                        frmRelatorio.variaveisMapa = strClusterMapa;
                        frmRelatorio.Show();                         
                    }

                    //Refresh
                    mMapa.ZoomToExtents();

                    //Refresh o mapa
                    mapImage1.Refresh();
                    Cursor.Current = Cursors.Default;

                    //Preenche a legenda do Mapa
                    if (dataGridView2.ColumnCount == 0)
                    {
                        //Adiciona cor
                        DataGridViewTextBoxColumn txtCor = new DataGridViewTextBoxColumn();
                        txtCor = new DataGridViewTextBoxColumn();
                        txtCor.Width = 36;
                        txtCor.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtCor.ReadOnly = true;
                        txtCor.HeaderText = "Cor";
                        txtCor.DisplayIndex = 2;
                        txtCor.DefaultCellStyle.BackColor = Color.White;
                        dataGridView2.Columns.Insert(0, txtCor);

                        //Adiciona Legenda
                        DataGridViewTextBoxColumn txtLegData = new DataGridViewTextBoxColumn();
                        txtLegData = new DataGridViewTextBoxColumn();
                        txtLegData.Width = 130;
                        txtLegData.AutoSizeMode = (System.Windows.Forms.DataGridViewAutoSizeColumnMode)AutoSizeMode.GrowAndShrink;
                        txtLegData.ReadOnly = false;
                        txtLegData.HeaderText = "Legenda";
                        txtLegData.DisplayIndex = 2;
                        dataGridView2.Columns.Insert(1, txtLegData);

                        //Adiciona Limite Superior
                        DataGridViewCheckBoxColumn chkSelecionar = new DataGridViewCheckBoxColumn();
                        chkSelecionar = new DataGridViewCheckBoxColumn();
                        chkSelecionar.Width = 70;
                        chkSelecionar.HeaderText = "Selecionar";
                        chkSelecionar.DisplayIndex = 2;
                        dataGridView2.Columns.Insert(2, chkSelecionar);
                    }

                    string var_escolhida = frmMapa.VariavelEscolhida;
                    clsUtilTools clt = new clsUtilTools();
                    object[,] cats = clt.DataTableToObjectMatrix(dTable, var_escolhida);
                    clt.FrequencyTable(ref cats, cats);
                    for (int i = 0; i < frmMapa.Legenda_MapaTem.GetLength(0); i++)
                        frmMapa.Legenda_MapaTem[i] = cats[i, 0].ToString();

                    if (dataGridView2.Rows.Count > cats.GetLength(0))
                        dataGridView2.Rows.Clear();

                    dataGridView2.ClearSelection();

                    for (int i = 0; i < dblClasses.Length; i++)
                    {
                        if (dataGridView2.Rows.Count < dblClasses.Length) dataGridView2.Rows.Add();
                        dataGridView2.Rows[i].Cells[0].Style.BackColor = frmMapa.CoresRGBColors[i];
                        dataGridView2.Rows[i].Cells[1].Value = frmMapa.Legenda_MapaTem[i];
                        dataGridView2.Rows[i].Cells[1].ToolTipText = frmMapa.Legenda_MapaTem[i];
                        dataGridView2.Rows[i].Cells[2].Value = false;
                    }
                }

                xpTabControl2.SelectedTab = tabPage3;
                xpTabControl1.SelectedTab = tabPage5;
                txtVarMapaTematico.Text = frmMapa.VariavelEscolhida;
                dataGridView2.ClearSelection();
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region zerar mapa temático

        private void zerarMapaTemáticoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                LimpaMapaTematico(true);
                Cursor = Cursors.Default;
                this.txtVarMapaTematico.Text = " ";

                //frmMapaTematico mapa = new frmMapaTematico();

                //mapa.ClearXML();
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void LimpaMapaTematico(bool zeraGridLegendas)
        {
            if (dataGridView2.Rows.Count <= 0) return;

            Hashtable[] ht = (Hashtable[])(m_caracteristicas_itens_layers[this.Name]);
            SolidBrush brs = new SolidBrush(Color.Transparent);
            Brush[] cores = new Brush[m_cores_mapa_tematico.GetLength(0)];
            for (int k = 0; k < cores.GetLength(0); k++) cores[k] = (Brush)brs;

            //Guarda os vetores
            ArrayList vetorID = new ArrayList();
            int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];

            for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
            {
                vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];

                // guardando os resultados no hashtable de características dos polígonos
                (ht[i])["Fill"] = (Brush)cores[iVetor[i]];
            }

            //Captura o mapa
            SharpMap.Map mMapa = mapImage1.Map;

            //Captura o layer
            SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[this.Name];

            LayerPrincipalNoTopo();

            //Pinta o mapa
            mTheme meuTema = new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice);
            layMapa.Theme = new SharpMap.Rendering.Thematics.CustomTheme(meuTema.GetStyle);

            if (dsDados.Tables[0].Columns.Contains("MapaTematico") == false) dsDados.Tables[0].Columns.Add("MapaTematico");
            for (int i = 0; i < iVetor.Length; i++)
                dsDados.Tables[0].Rows[i]["MapaTematico"] = iVetor[(int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa]];

            //Refresh
            mMapa.ZoomToExtents();

            //Refresh o mapa
            mapImage1.Refresh();

            if (zeraGridLegendas)
            {
                m_variavel_mapa_tematico = "";

                int nrows = dataGridView2.Rows.Count;
                for (int i = nrows - 1; i >= 0; i--)
                    dataGridView2.Rows.RemoveAt(i);
            }
        }

        #endregion 

        #endregion

        #region Abrir tabela em Excel

        private void AbrirTabelaEmExcel()
        {
            try
            {
                if (((DataTable)dataGridView1.DataSource).Columns.Count > 0 && ((DataTable)dataGridView1.DataSource).Rows.Count > 0)
                {
                    DataTable dsTemp = (DataTable)dataGridView1.DataSource;
                    Cursor = Cursors.WaitCursor;
                    BLExportacaoAberturaExcel ble = new BLExportacaoAberturaExcel();
                    ble.ExportaToExcel(dsTemp, "tabela", "tabela");
                    Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            AbrirTabelaEmExcel();
        }

        #endregion

        #region manipulação das legendas do mapa temático

        private Hashtable m_poligonos_selecionados = new Hashtable();
        private Hashtable m_tipos_geometrias_layers_importados = new Hashtable();
        private Hashtable m_path_layers_importados = new Hashtable();

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    dataGridView2.ClearSelection();
                    ColorDialog MyDialog = new ColorDialog();

                    // Keeps the user from selecting a custom color.
                    MyDialog.AllowFullOpen = true;

                    // Allows the user to get help. (The default is false.)
                    MyDialog.ShowHelp = true;

                    // Sets the initial color select to the current text color,
                    // so that if the user cancels out, the original color is restored.
                    MyDialog.Color = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;

                    // Open color selection dialog box
                    MyDialog.ShowDialog();

                    dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = MyDialog.Color;
                    //mudouCores = true;
                    
                    //Captura o mapa
                    SharpMap.Map mMapa = mapImage1.Map;

                    //Captura o layer
                    SharpMap.Layers.VectorLayer layMapa = (SharpMap.Layers.VectorLayer)mMapa.Layers[this.Name];

                    //Guarda as cores atuais
                    Brush[] cores = new Brush[dataGridView2.RowCount];
                    for (int i = 0; i < dataGridView2.RowCount; i++) cores[i] = new SolidBrush(dataGridView2.Rows[i].Cells[0].Style.BackColor);

                    ArrayList vetorID = new ArrayList();
                    int[] vetorIndice = new int[dsDados.Tables[0].Rows.Count];
                    
                    Hashtable[] ht = (Hashtable[])(m_caracteristicas_itens_layers[this.Name]);
                    
                    for (int i = 0; i < dsDados.Tables[0].Rows.Count; i++)
                    {
                        vetorID.Add(dsDados.Tables[0].Rows[i][strIDbase].ToString());
                        vetorIndice[i] = (int)dsDados.Tables[0].Rows[i]["Mapa" + strIDmapa];

                        // guardando os resultados no hashtable de características dos polígonos
                        (ht[i])["Fill"] = (Brush)cores[iVetor[i]];
                    }

                    mTheme meuTema = new mTheme(iVetor, cores, strIDmapa, vetorID, vetorIndice);
                    layMapa.Theme = new SharpMap.Rendering.Thematics.CustomTheme(meuTema.GetStyle);

                    //Refresh o mapa
                    mapImage1.Refresh();

                    dataGridView2.Refresh();
                    dataGridView2.Update();
                    Application.DoEvents();

                    //sinaliza alteração no mapa temático
                    mapa_alterado = true;
                }
                else if (e.ColumnIndex == 2)
                {
                    dataGridView2.ClearSelection();
                    
                    //Muda o valor da célula
                    if ((bool)dataGridView2.Rows[e.RowIndex].Cells[2].Value == false) dataGridView2.Rows[e.RowIndex].Cells[2].Value = true;
                    else if ((bool)dataGridView2.Rows[e.RowIndex].Cells[2].Value == true) dataGridView2.Rows[e.RowIndex].Cells[2].Value = false;

                    //Busca quais classes devem ser selecionadas.
                    ArrayList arClasses = new ArrayList();
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        string teste = dataGridView2.Rows[i].Cells[2].Value.ToString();
                        if ((bool)dataGridView2.Rows[i].Cells[2].Value)
                            arClasses.Add(i);
                    }

                    //Busca os polígonos de interesse
                    ArrayList arPoligonosSelecionados = new ArrayList();
                    for (int i = 0; i < iVetor.Length; i++)
                        for (int j = 0; j < arClasses.Count; j++)
                            if (iVetor[i] == (int)arClasses[j])
                            {
                                arPoligonosSelecionados.Add(i);
                                break;
                            }

                    int[] selecao_hach = new int[iVetor.GetLength(0)];
                    for (int i = 0; i < selecao_hach.GetLength(0); i++)
                        if (arPoligonosSelecionados.Contains(i)) selecao_hach[i] = 1;
                        else selecao_hach[i] = 0;

                    IpeaGeo.Classes.clsManipulaItensNoLayer clm = new Classes.clsManipulaItensNoLayer();
                    Hashtable[] ht = (Hashtable[])this.m_caracteristicas_itens_layers[this.Name];
                    DataTable dt = (DataTable)this.m_variaveis_dados_layers[this.Name];

                    clm.HachurearPoligonos(ref this.map, ref this.mapImage1,
                        this.Name, ref ht, ref dt, strIDmapa, ref selecao_hach);

                    m_poligonos_selecionados[this.Name] = arPoligonosSelecionados;
                    AjustaEnabledOpcoesLayersSelecionados();

                    //sinaliza alteração no mapa temático
                    mapa_alterado = true;
                }
                
                LayerPrincipalNoTopo();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AtivarListaTabelasDados()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                System.Windows.Forms.ImageList imagesSmall1 = new ImageList();
                imagesSmall1.ImageSize = new Size(20, 20);
                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
            }
        }

        private void clustersHierárquicosnãoespaciaisToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.dsDados.Tables[0].Rows.Count > 0 && this.dsDados.Tables[0].Columns.Count > 0)
                {
                    IpeaGeo.Modelagem.Clusters_hierarquicos.FormClustersHierarquicosNaoEspaciais frm = new IpeaGeo.Modelagem.Clusters_hierarquicos.FormClustersHierarquicosNaoEspaciais();
                    frm.MdiParent = this.MdiParent;
                    frm.TabelaDeDados = this.dsDados.Tables[0];
                    frm.Show();

                    AtualizaPeriodoPainelEspacial();
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

        #endregion

        #region funções de análise

        private void AnaliseGraficaToolsTrip()
        {
            try
            {
                IpeaGeo.Modelagem.Graficos.Graficos frm = new IpeaGeo.Modelagem.Graficos.Graficos();
                frm.MdiParent = this.MdiParent;
                frm.TabelaDeDados = this.dsDados.Tables[0].Copy();
                frm.Show();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void análiseGráficaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AnaliseGraficaToolsTrip();
        }

        private void análiseGráficaToolStripMenuItem_Click(object sender, EventArgs e)
        {
           AnaliseGraficaToolsTrip();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            AbrirTabelaEmExcel();
        }

        private void toolStripButton9_Click_1(object sender, EventArgs e)
        {
            label1.Visible = false;
            try
            {
                this.SelecionaBaseIPEAGEO();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void análiseFatorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGeo.Modelagem.frmAnaliseFatorial frm = new IpeaGeo.Modelagem.frmAnaliseFatorial();
                frm.MdiParent = this.MdiParent;
                frm.TabelaDeDados = this.dsDados.Tables[0].Copy();
                frm.HabilitarDadosExternos();
                frm.LabelTabelaDados = this.Text;
                frm.GridViewExterno = dataGridView1;
                frm.DataSetExterno = this.dsDados;
                frm.Show();

                AtualizaPeriodoPainelEspacial();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void métodosDeApoioÀDecisãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGeo.Modelagem.decisaoMulticriterios.FormDecisaoMulticriterios frm = new IpeaGeo.Modelagem.decisaoMulticriterios.FormDecisaoMulticriterios();
                frm.MdiParent = this.MdiParent;
                frm.Dados = this.dsDados.Tables[0].Copy();
                frm.HabilitarDadosExternos();
                frm.LabelTabelaDados = this.Text;
                frm.GridViewExterno = dataGridView1;
                frm.DataSetExterno = this.dsDados;
                IpeaGeo.RegressoesEspaciais.clsIpeaShape shape_modelagem = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
                shape_modelagem.ConvertFromIpeaGEOShape(this.shapeAlex);
                frm.Shape = this.shapeAlex;
                frm.Show();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region tratamento dos layers

        private Hashtable m_variaveis_dados_layers = new Hashtable();
        private Hashtable m_caracteristicas_itens_layers = new Hashtable();
        private Hashtable m_shape_Alex_layers = new Hashtable();

        private void AdicionaLayerVisualizacao()
        {
            m_atualiza_layers = false;

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "ShapeFile (*.shp)|*.shp|All Files (*.*)|*.*";
            string FileName = "";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                FileName = openFileDialog.FileName;
                string FileNameReduced = Path.GetFileNameWithoutExtension(FileName);
                Cursor.Current = Cursors.WaitCursor;

                SharpMap.Layers.VectorLayer layernovo = new SharpMap.Layers.VectorLayer(FileNameReduced);

                clsMapa clm = new clsMapa();
                DataTable variaveis_dt_shape = new DataTable();
                Hashtable[] ht_caracteristicas = new Hashtable[0];
                clsIpeaShape shape_layer = new clsIpeaShape();

                string tipo_geometria = "";

                clm.AdicionaLayerToMap(ref layernovo, ref mapImage1, ref map, ref variaveis_dt_shape, FileName, FileNameReduced, ref ht_caracteristicas, ref shape_layer, ref tipo_geometria);

                m_variaveis_dados_layers.Add(FileNameReduced, variaveis_dt_shape);
                m_caracteristicas_itens_layers.Add(FileNameReduced, ht_caracteristicas);
                m_poligonos_selecionados.Add(FileNameReduced, new ArrayList());
                m_shape_Alex_layers.Add(FileNameReduced, shape_layer);
                m_path_layers_importados.Add(FileNameReduced, FileName);
                m_tipos_geometrias_layers_importados.Add(FileNameReduced, tipo_geometria); 

                m_zoom_value_extended = this.map.Zoom;

                TreeNode novo_node = new TreeNode();
                novo_node.Name = FileNameReduced;
                novo_node.Text = FileNameReduced;
                treeView1.Nodes.Add(novo_node);
                treeView1.Nodes[FileNameReduced].Checked = true;

                Cursor.Current = Cursors.Default;

                //sinaliza alteração no mapa temático
                mapa_alterado = true;
            }
        }

        private void ExcluirLayerVisualizacao()
        {
            m_atualiza_layers = false;

            if (treeView1.SelectedNode != null)
            {
                string nome_layer = treeView1.SelectedNode.Name;

                if (nome_layer == this.Name) throw new Exception("Layer selecionado é o layer de análise e não pode ser excluído."); 

                clsMapa clm = new clsMapa();
                clm.ExcluirLayerExistente(ref mapImage1, ref map, nome_layer);

                if (this.m_variaveis_dados_layers.Contains(nome_layer))
                    this.m_variaveis_dados_layers.Remove(nome_layer);
                
                if (this.m_caracteristicas_itens_layers.Contains(nome_layer))
                    this.m_caracteristicas_itens_layers.Remove(nome_layer);

                if (this.m_poligonos_selecionados.Contains(nome_layer))
                    this.m_poligonos_selecionados.Remove(nome_layer);

                if (this.m_shape_Alex_layers.Contains(nome_layer))
                    this.m_shape_Alex_layers.Remove(nome_layer);

                if (this.m_tipos_geometrias_layers_importados.Contains(nome_layer)) this.m_tipos_geometrias_layers_importados.Remove(nome_layer);
                if (this.m_path_layers_importados.Contains(nome_layer)) this.m_path_layers_importados.Remove(nome_layer);

                treeView1.Nodes[nome_layer].Remove();
            }
            else throw new Exception("Selecione um layer para exclusão."); 
        }

        private void PropridadesDoLayer(bool todos_layers)
        {
            m_atualiza_layers = false;

            if (treeView1.SelectedNode != null)
            {
                string selected_layer = treeView1.SelectedNode.Name;
                frmPropriedadesPoligonosNoLayer frm = new frmPropriedadesPoligonosNoLayer();
                frm.NomeLayerSelecionado = selected_layer;
                frm.TipoGeometriaLayer = Convert.ToString(this.m_tipos_geometrias_layers_importados[selected_layer]);
                frm.VariaveisDadosShape = (DataTable)this.m_variaveis_dados_layers[selected_layer];
                frm.HashTableCaracteristicasItens = (Hashtable[])this.m_caracteristicas_itens_layers[selected_layer];
                frm.ZoomValueExtended = m_zoom_value_extended;
                frm.Mapa = this.map;
                frm.MapImage = this.mapImage1;
                frm.SelecionaTodosElementos = todos_layers;
                if (!todos_layers && ((ArrayList)m_poligonos_selecionados[selected_layer]).Count > 0 && ((ArrayList)m_poligonos_selecionados[selected_layer]).Count < frm.VariaveisDadosShape.Rows.Count)
                    frm.ObservacoesSelecionadas = ((ArrayList)m_poligonos_selecionados[selected_layer]);
                else frm.SelecionaTodosElementos = true;
                
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    Hashtable[] ht = (Hashtable[])frm.HashTableCaracteristicasItens;
                    this.m_caracteristicas_itens_layers[selected_layer] = ht;
                }
            }
            else throw new Exception("Selecione um layer para edição de atributos."); 
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            try
            {
                m_atualiza_layers = false;
                AdicionaLayerVisualizacao();
                m_atualiza_layers = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void excluirLayerDeVisualizaçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                m_atualiza_layers = false;
                ExcluirLayerVisualizacao();
                m_atualiza_layers = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void adicionarLayerDeVisualizaçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                m_atualiza_layers = false;
                AdicionaLayerVisualizacao();
                m_atualiza_layers = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool m_atualiza_layers = false;

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (m_atualiza_layers)
                {
                    if (map.Layers.Count > 1)
                    {
                        ArrayList lista = new ArrayList();
                        for (int i = 0; i < treeView1.Nodes.Count; i++)
                            if (treeView1.Nodes[i].Checked)
                                lista.Add(treeView1.Nodes[i].Name);

                        clsMapa clm = new clsMapa();
                        clm.ReapresentarLayersExistentes(ref mapImage1, ref map, lista);
                    }
                    else
                    {
                        m_atualiza_layers = false;
                        treeView1.Nodes[this.Name].Checked = true;
                        m_atualiza_layers = true;
                    }
                }
                else if (!treeView1.Nodes[this.Name].Checked)
                    treeView1.Nodes[this.Name].Checked = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void propriedadesParaTodosOsElementosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                m_atualiza_layers = false;
                PropridadesDoLayer(true);
                m_atualiza_layers = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void propriedadesDosElementosSelecionadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                m_atualiza_layers = false;
                PropridadesDoLayer(false);
                m_atualiza_layers = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LayerPrincipalNoTopo()
        {
            if (this.Name != this.m_nome_top_layer)
            {
                m_atualiza_layers = false;
                treeView1.Nodes.Remove(treeView1.Nodes[this.Name]);

                TreeNode novo_node = new TreeNode();
                novo_node.Name = this.Name;
                novo_node.Text = this.Name + " (principal)";
                treeView1.Nodes.Insert(0, novo_node);
                treeView1.Nodes[this.Name].Checked = true;

                m_nome_top_layer = treeView1.Nodes[0].Name;
                treeView1.SelectedNode = treeView1.Nodes[this.Name];
                treeView1.Refresh();

                btnAtualizarOrdemLayers.Enabled = true;
                m_atualiza_layers = true;
            }
        }

        private void btnLayerUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode != null && treeView1.Nodes.Count > 1)
                {
                    m_atualiza_layers = false;
                    string selected_layer = treeView1.SelectedNode.Name;
                    bool checked_state = treeView1.Nodes[selected_layer].Checked;
                    int node_position = treeView1.Nodes[selected_layer].Index;
                    int num_nodes = treeView1.Nodes.Count;

                    treeView1.Nodes.Remove(treeView1.Nodes[selected_layer]);

                    TreeNode novo_node = new TreeNode();
                    novo_node.Name = selected_layer;
                    if (selected_layer == this.Name)
                        novo_node.Text = selected_layer + " (principal)";
                    else novo_node.Text = selected_layer;
                    
                    treeView1.Nodes.Insert(Math.Max(0, node_position-1), novo_node);
                    treeView1.Nodes[selected_layer].Checked = checked_state;

                    m_nome_top_layer = treeView1.Nodes[0].Name;
                    treeView1.SelectedNode = treeView1.Nodes[selected_layer];
                    treeView1.Refresh();

                    //btnAtualizarOrdemLayers.Enabled = true;
                    m_atualiza_layers = true;
                }
                AlterarOrdemLayers();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLayerDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode != null && treeView1.Nodes.Count > 1)
                {
                    m_atualiza_layers = false;
                    string selected_layer = treeView1.SelectedNode.Name;
                    bool checked_state = treeView1.Nodes[selected_layer].Checked;
                    int node_position = treeView1.Nodes[selected_layer].Index;
                    int num_nodes = treeView1.Nodes.Count;

                    treeView1.Nodes.Remove(treeView1.Nodes[selected_layer]);

                    TreeNode novo_node = new TreeNode();
                    novo_node.Name = selected_layer;
                    if (selected_layer == this.Name)
                        novo_node.Text = selected_layer + " (principal)";
                    else novo_node.Text = selected_layer;
                    
                    treeView1.Nodes.Insert(node_position + 1, novo_node);
                    treeView1.Nodes[selected_layer].Checked = checked_state;

                    m_nome_top_layer = treeView1.Nodes[0].Name;
                    treeView1.SelectedNode = treeView1.Nodes[selected_layer];
                    treeView1.Refresh();

                    //btnAtualizarOrdemLayers.Enabled = true;
                    m_atualiza_layers = true;
                }
                AlterarOrdemLayers();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void AlterarOrdemLayers()
        {
            SharpMap.Map novo_mapa = new SharpMap.Map(new Size(800, 400));
            novo_mapa.BackColor = Color.White;
            SharpMap.Layers.Layer l1;

            string[] posicoes_invert = new string[treeView1.Nodes.Count];
            for (int i = 0; i < posicoes_invert.GetLength(0); i++)
                posicoes_invert[posicoes_invert.GetLength(0)-i-1] = treeView1.Nodes[i].Name;

            for (int i = 0; i < posicoes_invert.GetLength(0); i++)
            {
                l1 = (SharpMap.Layers.Layer)map.Layers[posicoes_invert[i]];
                novo_mapa.Layers.Add(l1);
            }
            map = novo_mapa;
            mapImage1.Map = map;

            this.mapImage1.Map.ZoomToExtents();
            this.mapImage1.Refresh();

            this.btnAtualizarOrdemLayers.Enabled = false;
        }

        private void btnAtualizarOrdemLayers_Click(object sender, EventArgs e)
        {
            try
            {
                this.AlterarOrdemLayers();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AjustaEnabledOpcoesLayersSelecionados()
        {
            if (((ArrayList)m_poligonos_selecionados[this.m_nome_top_layer]).Count > 0)
                propriedadesDosElementosSelecionadosToolStripMenuItem.Enabled = true;
            else
                propriedadesDosElementosSelecionadosToolStripMenuItem.Enabled = false;
        }

        private void propriedadesDoLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AjustaEnabledOpcoesLayersSelecionados();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region opções para tratamento de painel espacial

        private void listBoxPeriodosPainelEspacial_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string selected_time = this.listBoxPeriodosPainelEspacial.SelectedItem.ToString();
                if (m_atualiza_list_box_painel_espacial && selected_time != "" && selected_time != m_periodo_painel_espacial)
                {
                    Cursor = Cursors.WaitCursor;
                    AtualizaPeriodoPainelEspacial();
                    AjustaMapaTematicoPainel();
                    Cursor = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AtualizaPeriodoPainelEspacial()
        {
            if (m_usa_dados_painel_espacial)
            {
                RemoverSelecoes();

                DataTable dt_aux = dsDados.Tables[0].Copy();
                dt_aux.TableName = m_periodo_painel_espacial;

                m_ds_dados_painel_espacial.Tables.Remove(m_periodo_painel_espacial);
                m_ds_dados_painel_espacial.Tables.Add(dt_aux);

                string selected_time = this.listBoxPeriodosPainelEspacial.SelectedItem.ToString();
                DataTable dt = (DataTable)m_ds_dados_painel_espacial.Tables[selected_time];

                //Guarda a base de dados conectada
                dsDados.Tables.Clear();
                dsDados.Tables.Add(dt.Copy());

                m_tabela_dados_original = ((DataTable)this.m_ds_dados_originais_painei_espacial.Tables[selected_time]).Copy();

                //Guarda o dataTable
                dataGridView1.DataSource = dsDados.Tables[0];
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.Refresh();

                //Lista as variáveis
                listBox1.Items.Clear();
                for (int i = 0; i < dsDados.Tables[0].Columns.Count; i++) listBox1.Items.Add(dsDados.Tables[0].Columns[i].ToString());

                txtPainelPeriodoApresentado.Text = selected_time;
                m_periodo_painel_espacial = selected_time;
            }
        }

        #endregion

        #region definição da matriz de vizinhança

        private bool m_matriz_W_esparsa_existente = false;
        private IpeaGeo.RegressoesEspaciais.clsMatrizEsparsa m_matriz_W_esparsa = new RegressoesEspaciais.clsMatrizEsparsa();
        private bool m_matriz_W_normalizada = false;
        private string m_tipo_matriz_vizinhanca = "";
        private int m_ordem_matriz_vizinhanca = 0;

        private void MatrizVizinhanca()
        {
            DataTable dt_dados = (DataTable)dsDados.Tables[0];

            if (dt_dados.Rows.Count > 0 && dt_dados.Columns.Count > 0)
            {
                FormMatrizVizinhancaComDistancias frm = new FormMatrizVizinhancaComDistancias();
                frm.DefinicaoGeralMatrizVizinhanca = true;
                frm.Shape.ConvertFromIpeaGEOShape(this.shapeAlex);
                frm.Dados = dt_dados;

                if (m_tipos_geometrias_layers_importados[this.Name].ToString() == "Polígonos"
                    || m_tipos_geometrias_layers_importados[this.Name].ToString() == "Multi-Polígonos")
                    frm.HabilitaMatrizContiguidade = true;
                else frm.HabilitaMatrizContiguidade = false;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    m_matriz_W_esparsa_existente = true;
                    m_matriz_W_esparsa = frm.Wesparsa;
                    m_matriz_W_normalizada = frm.MatrizWNormalizada;
                    m_tipo_matriz_vizinhanca = frm.TipoVizinhanca;
                    m_ordem_matriz_vizinhanca = frm.OrdemVizinhanca;

                    MessageBox.Show("Matriz de vizinhança atualizada. A partir de agora, todos os cálculos serão feitos considerando-se essa nova matriz.",
                                    "Matriz de vizinhança", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    toolStripMenuItem9.Enabled = true;
                    exportarMatrizDeVizinhançaToolStripMenuItem.Enabled = true;
                    exportarMatrizDeVizinhançaToolStripMenuItem1.Enabled = true;   
                }
            }
            else MessageBox.Show("Tabela de dados está vazia.");
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            try
            {
                MatrizVizinhanca();
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            try
            {
                MatrizVizinhanca();
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MedidasParaPoligonos()
        {
           try
            {
                DataTable dt = (DataTable)dsDados.Tables[0];
                if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
                {
                    FormCalculadora frm = new FormCalculadora();
                    frm.Dados = dt;
                    frm.AtivaMedidasPoligonos = true;
                    frm.DadosConcatenados = true;
                    frm.Shape.ConvertFromIpeaGEOShape(this.shapeAlex);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        dt = frm.Dados;
                        dt.TableName = dsDados.Tables[0].TableName;

                        this.dsDados.Tables.RemoveAt(0);
                        this.dsDados.Tables.Add(dt.Copy());

                        this.dataGridView1.DataSource = this.dsDados.Tables[0];
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        
        }
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            MedidasParaPoligonos();
        }
        
        private void GeracaoDummies()
        {
            try
            {
                DataTable dt = (DataTable)dsDados.Tables[0];
                if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
                {
                    FormGeracaoDummies frm = new FormGeracaoDummies();
                    frm.TabelaDados = dt.Copy();

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        dt = frm.TabelaDados;
                        dt.TableName = dsDados.Tables[0].TableName;

                        this.dsDados.Tables.RemoveAt(0);
                        this.dsDados.Tables.Add(dt.Copy());

                        this.dataGridView1.DataSource = this.dsDados.Tables[0];

                        //sinaliza alteração no mapa temático
                        mapa_alterado = true;
                    }
                }
                else MessageBox.Show("Tabela de dados está vazia");
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            GeracaoDummies();
        }

        private void ExportarMatrizDeVizinhanca()
        {
            try
            {
                if (m_matriz_W_esparsa_existente)
                {
                    DataTable dt = (DataTable)dsDados.Tables[0];
                    if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
                    {
                        FormSalvamentoMatrizEsparsa frm = new FormSalvamentoMatrizEsparsa(false);
                        frm.WsparsaFromDists = this.m_matriz_W_esparsa;
                        if (m_matriz_W_normalizada) frm.TipoMatrizVizinhanca = IpeaGeo.RegressoesEspaciais.TipoMatrizVizinhanca.Normalizada;
                        else frm.TipoMatrizVizinhanca = IpeaGeo.RegressoesEspaciais.TipoMatrizVizinhanca.Original;
                        frm.DtDados = dt;
                        frm.ShowDialog();
                    }
                    else MessageBox.Show("Tabela de dados está vazia");
                }
                else
                {
                    MessageBox.Show("Matriz de vizinhança ainda não está definida");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exportarMatrizDeVizinhançaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportarMatrizDeVizinhanca();
        }

        private void ImportarMatrizDeVizinhanca()
        {
            try
            {
                DataTable dt = (DataTable)dsDados.Tables[0];
                if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
                {
                    FormSalvamentoMatrizEsparsa frm = new FormSalvamentoMatrizEsparsa(true);
                    frm.DtDados = dt;
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        m_matriz_W_esparsa_existente = true;
                        m_matriz_W_esparsa = frm.WsparsaImportada;

                        if (frm.TipoMatrizVizinhanca == IpeaGeo.RegressoesEspaciais.TipoMatrizVizinhanca.Normalizada) this.m_matriz_W_normalizada = true;
                        else m_matriz_W_normalizada = true;

                        MessageBox.Show("Matriz de vizinhança importada com sucesso. A partir de agora, todos os cálculos serão feitos considerando-se essa nova matriz.",
                                        "Matriz de vizinhança", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else MessageBox.Show("Tabela de dados está vazia.");
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void importarMatrizDeVizinhançaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportarMatrizDeVizinhanca();
        }

        private void exportarMatrizDeVizinhançaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ExportarMatrizDeVizinhanca();
        }

        private void importarMatrizDeVizinhançaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ImportarMatrizDeVizinhanca();
        }

        #endregion

        private enum KindOfStatisticalAnalysis
        {
            SpatialPanel, NonSpatialPanel, NonParametric
        }

        private FormStatisticalAnalysis loadAnalysisForm(KindOfStatisticalAnalysis analysis, bool show)
        {
            FormStatisticalAnalysis frm = null;

            AtualizaPeriodoPainelEspacial();

            switch (analysis)
            {
                case KindOfStatisticalAnalysis.NonParametric:
                    frm = new IpeaGeo.Modelagem.FormNonParametric();
                    frm.TabelaDeDados = this.dsDados.Tables[0].Copy();
                    break;
                case KindOfStatisticalAnalysis.NonSpatialPanel:
                    frm = new FormDadosPainelNaoEspacial();
                    break;
                case KindOfStatisticalAnalysis.SpatialPanel:
                    frm = new IpeaGeo.RegressoesEspaciais.FormRegressaoDadosPainelEspacial();
                    break;
            }

            frm.MdiParent = this.MdiParent;
            frm.HabilitarDadosExternos();
            frm.LabelTabelaDados = this.Text;
            frm.GridViewExterno = dataGridView1;
            frm.DataSetExterno = this.dsDados;
            if (analysis != KindOfStatisticalAnalysis.NonParametric)
            {
                frm.DadosPainelEspacial = m_ds_dados_painel_espacial;
                frm.PeriodoFocoPainel = this.listBoxPeriodosPainelEspacial.SelectedItem.ToString();
                frm.VariavelPeriodosPainel = m_variavel_periodos_painel;
                frm.VariavelUnidadesPainel = m_variavel_unidades_painel;
                frm.ListaPeriodosPainelEspacial = this.m_freqs_periodos_painel_espacial;
            }

            if (show) frm.Show();

            return frm;
        }

        #region regressão com dados de painel

        private void regressãoComDadosDePainelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormStatisticalAnalysis frm = loadAnalysisForm(KindOfStatisticalAnalysis.NonSpatialPanel, true);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region testes não paramétricos

        private void testesNãoParamétricosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormStatisticalAnalysis frm = loadAnalysisForm(KindOfStatisticalAnalysis.NonParametric, true);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region regressão com dados de painel espacial

        private void dadosDePainelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                FormRegressaoDadosPainelEspacial frm = (FormRegressaoDadosPainelEspacial)loadAnalysisForm(KindOfStatisticalAnalysis.SpatialPanel, false);

                //Se matriz de vizinhança já especificada
                if (m_matriz_W_esparsa_existente)
                {
                    frm.MatrizWEsparsaPredefinida = m_matriz_W_esparsa;
                    frm.TipoMatrizVizinhancaPredefinida = m_tipo_matriz_vizinhanca;
                    frm.OrdemMatrizVizinhancaPredefinida = m_ordem_matriz_vizinhanca;
                    frm.MatrizWNormalizadaPredefinida = m_matriz_W_normalizada;
                    frm.UsaWSparsaPredefinida = m_matriz_W_esparsa_existente;
                }

                IpeaGeo.RegressoesEspaciais.clsIpeaShape shape_modelagem = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
                shape_modelagem.ConvertFromIpeaGEOShape(this.shapeAlex);
                frm.Shape = shape_modelagem;

                frm.Show();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Tool Strip Calculadora

        private void exportarTabelaDeDadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportaDados();
        }

        private void recuperarTabelaOriginalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            recuperarTabelaOriginal();

            //sinaliza alteração no mapa temático
            mapa_alterado = true;
        }

        private void abrirDadosEmPlanilhaExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirTabelaEmExcel();
        }

        private void calculadoraToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            calculadoraToolStrip();
        }

        private void medidasParaOsPolígonosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MedidasParaPoligonos();
        }

        private void excluirVariáveisToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            excluirVariáveisToolStrip();
        }

        private void geraçãoDeVariáveisDummyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeracaoDummies();
        }

        private void análiseGráficaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AnaliseGraficaToolsTrip();
        }

        private void estatísticasDescritvasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EstatDescritivas();
        }

        private void tabelasDeFrequênciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabelasFrequencias();
        }
        
        private void tabulaçõesCruzadasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabulacoesCruzadas();
        }

        private void correlaçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CorrelacoestoolStrip();
        }

        private void matrizDeVizinhançaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MatrizVizinhanca();
        }

        private void exportarMatrizDeVizinhançaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ExportarMatrizDeVizinhanca();
        }

        private void importarMatrizDeVizinhançaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ImportarMatrizDeVizinhanca();
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Visible = false;
            escala();
        }

        private void compatibilizaçãoDeVariáveisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGeo.RegressoesEspaciais.FormAreasMinimasComparaveis frm = new IpeaGeo.RegressoesEspaciais.FormAreasMinimasComparaveis();
                frm.Show();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void modelosLinearesGeneralizadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IpeaGeo.Modelagem.RegressaoGLM frm = new IpeaGeo.Modelagem.RegressaoGLM();
                frm.MdiParent = this.MdiParent;
                frm.TabelaDeDados = this.dsDados.Tables[0].Copy();
                frm.HabilitarDadosExternos();
                frm.LabelTabelaDados = this.Text;
                frm.GridViewExterno = dataGridView1;
                frm.DataSetExterno = this.dsDados;
                frm.Show();

                AtualizaPeriodoPainelEspacial();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string basefile;
        public void XML(string nome, Color[] cores, string Base, bool mapa)
        {
            this.Name = nome;
            this.mapa_tem = mapa;
            tab_mod = true;
            tab_mod_efetuada = true;
            basefile = Base;
            SelecionaBaseIPEAGEO();

            if (cores.Length > 0)
            {
                variáveisQuantitativasToolStripMenuItem_Click(this, new EventArgs());
            }
            tab_mod = tab_mod_efetuada = false;
        }

        private static bool salvamento;
        public bool salvamento_efetuado = salvamento;
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            Classes.clsArmazenamentoDados salvar = new Classes.clsArmazenamentoDados();
            ExportData export = new ExportData();

            salvamento = true;
            salvamento_efetuado = true;
            string nome;
            double[] posicao_map = new double[2]; 
            posicao_map[0] = map.Center.X;
            posicao_map[1] = map.Center.Y;
            nome = strEnderecoBase.Substring(strEnderecoBase.LastIndexOf("\\") + 1);
            salvar.CapturaDados(base.Text, dsDados.Tables[0].Namespace.ToString(), nome, map.Zoom, posicao_map, strEnderecoMapa);
            salvar.CapturaDados(strIDbase, strIDmapa, mapa_tem);
            salvar.CapturaDados(m_cores, m_legendas_mapa_tematico, m_variavel_mapa_tematico, m_metodo, index);

            ExportaDados();
            salvamento = salvamento_efetuado = false;
            if (export.fechamento == true)
            {
                mapa_alterado = false;
            }
        }
        public void clear()
        {
            dataFiles = null;
        }
        private bool mapa_alterado;

        private void mapImage1_MouseWheel(object sender, MouseEventArgs e)
        {
            //sinaliza alteração no mapa temático
            mapa_alterado = true;
            //escala();
        }
        private void mapImage1_MapZoomChanged(double zoom)
        {
            escala();
        }
        private void mapImage1_MapZooming(double zoom)
        {
            escala();
        }

        private void frmMapa_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dsDados.Tables[0].Rows.Count > 0 && mapa_alterado == true)
            {
                DialogResult salvartrabalho = MessageBox.Show("Deseja salvar as alterações no mapa?", "Aviso", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (salvartrabalho == DialogResult.Yes)
                {
                    saveToolStripButton.PerformClick();
                    ExportData export = new ExportData();
                    if (export.fechamento == false)
                    {
                        e.Cancel = true;
                    }
                }
                else if (salvartrabalho == DialogResult.No)
                {
                    try
                    {
                        System.GC.Collect();
                    }
                    catch (Exception er)
                    {
                        MessageBox.Show(er.Message);
                    }
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        #region Escala
        double dScale;
        
        private void escala()
        {
            double i;

            i = distance(map.Envelope.TopLeft.Y, map.Envelope.TopLeft.X, map.Envelope.TopRight.Y, map.Envelope.TopRight.X, 'K');

            double map_ratio = i / map.Size.Width;
            dScale = map_ratio * pictureBox2.Size.Width;

            if (dScale >= 50)
            {
                label6.Text = Math.Round(dScale).ToString();
                label5.Text = "(km)";
            }
            else
            {
                label6.Text = Math.Round(dScale * 1000).ToString();
                label5.Text = "(m)";
            }

        }

        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
        private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));

            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;

            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }
        
        #endregion
    }
}

