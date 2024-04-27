namespace IpeaGEO
{
    partial class frmMapa
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMapa));
            SharpMap.Map map1 = new SharpMap.Map();
            System.Drawing.Drawing2D.Matrix matrix1 = new System.Drawing.Drawing2D.Matrix();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.adDados = new System.Windows.Forms.ToolStripButton();
            this.toolExportaDados = new System.Windows.Forms.ToolStripButton();
            this.toolRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.moveMapa = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.geoReferenciamento = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tlInformacao = new System.Windows.Forms.ToolStripButton();
            this.mapaTematico = new System.Windows.Forms.ToolStripButton();
            this.ToolspatialEstat = new System.Windows.Forms.ToolStripSplitButton();
            this.tlMultivariada = new System.Windows.Forms.ToolStripMenuItem();
            this.análiseDeConglomeradosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.índicesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ponderadoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.componentesPrincipaisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prometheéToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.dependênciaEspacialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.índiceDeMoranToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lISAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelosLinearesEspaciaisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.estatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bernoulliToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.poissonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.conglomeradosEspaciaisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hierarquicoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entropiaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.dsDados = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.xpTabControl1 = new VSControls.XPTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selecionarPastaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.iconesPequenosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iconesGranesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selecionaTodosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.limpaTodosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xpTabControl2 = new VSControls.XPTabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.mapImage1 = new SharpMap.Forms.MapImage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dsDados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.xpTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.xpTabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapImage1)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.saveToolStripButton,
            this.adDados,
            this.toolExportaDados,
            this.toolRefresh,
            this.toolStripSeparator2,
            this.moveMapa,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton5,
            this.geoReferenciamento,
            this.toolStripSeparator1,
            this.tlInformacao,
            this.mapaTematico,
            this.ToolspatialEstat,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(792, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newToolStripButton.Text = "&Limpa o mapa";
            this.newToolStripButton.Click += new System.EventHandler(this.newToolStripButton_Click_1);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Salva o mapa";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click_1);
            // 
            // adDados
            // 
            this.adDados.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.adDados.Image = ((System.Drawing.Image)(resources.GetObject("adDados.Image")));
            this.adDados.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.adDados.Name = "adDados";
            this.adDados.Size = new System.Drawing.Size(23, 22);
            this.adDados.Text = "Adiciona dados";
            this.adDados.Click += new System.EventHandler(this.adDados_Click_1);
            // 
            // toolExportaDados
            // 
            this.toolExportaDados.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolExportaDados.Enabled = false;
            this.toolExportaDados.Image = ((System.Drawing.Image)(resources.GetObject("toolExportaDados.Image")));
            this.toolExportaDados.ImageTransparentColor = System.Drawing.Color.White;
            this.toolExportaDados.Name = "toolExportaDados";
            this.toolExportaDados.Size = new System.Drawing.Size(23, 22);
            this.toolExportaDados.Text = "Exporta dados";
            this.toolExportaDados.Click += new System.EventHandler(this.toolExportaDados_Click_1);
            // 
            // toolRefresh
            // 
            this.toolRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolRefresh.Enabled = false;
            this.toolRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolRefresh.Image")));
            this.toolRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolRefresh.Name = "toolRefresh";
            this.toolRefresh.Size = new System.Drawing.Size(23, 22);
            this.toolRefresh.Text = "Atualiza tabela e mapa";
            this.toolRefresh.Click += new System.EventHandler(this.toolRefresh_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // moveMapa
            // 
            this.moveMapa.CheckOnClick = true;
            this.moveMapa.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveMapa.Image = ((System.Drawing.Image)(resources.GetObject("moveMapa.Image")));
            this.moveMapa.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveMapa.Name = "moveMapa";
            this.moveMapa.Size = new System.Drawing.Size(23, 22);
            this.moveMapa.Text = "Move o mapa";
            this.moveMapa.Click += new System.EventHandler(this.moveMapa_Click_1);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.CheckOnClick = true;
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Adiciona zoom";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click_1);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.CheckOnClick = true;
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "Remove zoom";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click_1);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "Padrão";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click_1);
            // 
            // geoReferenciamento
            // 
            this.geoReferenciamento.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.geoReferenciamento.Image = ((System.Drawing.Image)(resources.GetObject("geoReferenciamento.Image")));
            this.geoReferenciamento.ImageTransparentColor = System.Drawing.Color.White;
            this.geoReferenciamento.Name = "geoReferenciamento";
            this.geoReferenciamento.Size = new System.Drawing.Size(23, 22);
            this.geoReferenciamento.Text = "Georeferenciamento";
            this.geoReferenciamento.Click += new System.EventHandler(this.geoReferenciamento_Click_1);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tlInformacao
            // 
            this.tlInformacao.CheckOnClick = true;
            this.tlInformacao.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlInformacao.Enabled = false;
            this.tlInformacao.Image = ((System.Drawing.Image)(resources.GetObject("tlInformacao.Image")));
            this.tlInformacao.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlInformacao.Name = "tlInformacao";
            this.tlInformacao.Size = new System.Drawing.Size(23, 22);
            this.tlInformacao.Text = "Informação sobre o polígono";
            this.tlInformacao.Click += new System.EventHandler(this.tlInformacao_Click_1);
            // 
            // mapaTematico
            // 
            this.mapaTematico.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mapaTematico.Enabled = false;
            this.mapaTematico.Image = ((System.Drawing.Image)(resources.GetObject("mapaTematico.Image")));
            this.mapaTematico.ImageTransparentColor = System.Drawing.Color.White;
            this.mapaTematico.Name = "mapaTematico";
            this.mapaTematico.Size = new System.Drawing.Size(23, 22);
            this.mapaTematico.Text = "Mapa temático";
            this.mapaTematico.Click += new System.EventHandler(this.mapaTematico_Click_1);
            // 
            // ToolspatialEstat
            // 
            this.ToolspatialEstat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolspatialEstat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlMultivariada,
            this.toolStripSeparator4,
            this.dependênciaEspacialToolStripMenuItem,
            this.modelosLinearesEspaciaisToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripSeparator3,
            this.estatToolStripMenuItem,
            this.conglomeradosEspaciaisToolStripMenuItem});
            this.ToolspatialEstat.Enabled = false;
            this.ToolspatialEstat.Image = ((System.Drawing.Image)(resources.GetObject("ToolspatialEstat.Image")));
            this.ToolspatialEstat.ImageTransparentColor = System.Drawing.Color.White;
            this.ToolspatialEstat.Name = "ToolspatialEstat";
            this.ToolspatialEstat.Size = new System.Drawing.Size(32, 22);
            this.ToolspatialEstat.Text = "Funções";
            // 
            // tlMultivariada
            // 
            this.tlMultivariada.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.análiseDeConglomeradosToolStripMenuItem,
            this.índicesToolStripMenuItem});
            this.tlMultivariada.Enabled = false;
            this.tlMultivariada.Name = "tlMultivariada";
            this.tlMultivariada.Size = new System.Drawing.Size(234, 22);
            this.tlMultivariada.Text = "Análise Multivariada";
            // 
            // análiseDeConglomeradosToolStripMenuItem
            // 
            this.análiseDeConglomeradosToolStripMenuItem.Name = "análiseDeConglomeradosToolStripMenuItem";
            this.análiseDeConglomeradosToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.análiseDeConglomeradosToolStripMenuItem.Text = "Análise de conglomerados";
            this.análiseDeConglomeradosToolStripMenuItem.Click += new System.EventHandler(this.análiseDeConglomeradosToolStripMenuItem_Click_1);
            // 
            // índicesToolStripMenuItem
            // 
            this.índicesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ponderadoToolStripMenuItem,
            this.componentesPrincipaisToolStripMenuItem,
            this.prometheéToolStripMenuItem});
            this.índicesToolStripMenuItem.Name = "índicesToolStripMenuItem";
            this.índicesToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.índicesToolStripMenuItem.Text = "Índices";
            // 
            // ponderadoToolStripMenuItem
            // 
            this.ponderadoToolStripMenuItem.Name = "ponderadoToolStripMenuItem";
            this.ponderadoToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.ponderadoToolStripMenuItem.Text = "Ponderado";
            // 
            // componentesPrincipaisToolStripMenuItem
            // 
            this.componentesPrincipaisToolStripMenuItem.Name = "componentesPrincipaisToolStripMenuItem";
            this.componentesPrincipaisToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.componentesPrincipaisToolStripMenuItem.Text = "Componentes Principais";
            // 
            // prometheéToolStripMenuItem
            // 
            this.prometheéToolStripMenuItem.Name = "prometheéToolStripMenuItem";
            this.prometheéToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.prometheéToolStripMenuItem.Text = "Prometheé";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(231, 6);
            // 
            // dependênciaEspacialToolStripMenuItem
            // 
            this.dependênciaEspacialToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.índiceDeMoranToolStripMenuItem,
            this.lISAToolStripMenuItem});
            this.dependênciaEspacialToolStripMenuItem.Name = "dependênciaEspacialToolStripMenuItem";
            this.dependênciaEspacialToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.dependênciaEspacialToolStripMenuItem.Text = "Dependência Espacial";
            // 
            // índiceDeMoranToolStripMenuItem
            // 
            this.índiceDeMoranToolStripMenuItem.Name = "índiceDeMoranToolStripMenuItem";
            this.índiceDeMoranToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.índiceDeMoranToolStripMenuItem.Text = "Índices Globais";
            this.índiceDeMoranToolStripMenuItem.Click += new System.EventHandler(this.índiceDeMoranToolStripMenuItem_Click_1);
            // 
            // lISAToolStripMenuItem
            // 
            this.lISAToolStripMenuItem.Name = "lISAToolStripMenuItem";
            this.lISAToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.lISAToolStripMenuItem.Text = "Índices Locais";
            this.lISAToolStripMenuItem.Click += new System.EventHandler(this.lISAToolStripMenuItem_Click_1);
            // 
            // modelosLinearesEspaciaisToolStripMenuItem
            // 
            this.modelosLinearesEspaciaisToolStripMenuItem.Enabled = false;
            this.modelosLinearesEspaciaisToolStripMenuItem.Name = "modelosLinearesEspaciaisToolStripMenuItem";
            this.modelosLinearesEspaciaisToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.modelosLinearesEspaciaisToolStripMenuItem.Text = "Modelos Lineares Espaciais";
            this.modelosLinearesEspaciaisToolStripMenuItem.Click += new System.EventHandler(this.modelosLinearesEspaciaisToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Enabled = false;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(234, 22);
            this.toolStripMenuItem1.Text = "Indíces de segregação espacial";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click_1);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(231, 6);
            // 
            // estatToolStripMenuItem
            // 
            this.estatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bernoulliToolStripMenuItem,
            this.poissonToolStripMenuItem});
            this.estatToolStripMenuItem.Name = "estatToolStripMenuItem";
            this.estatToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.estatToolStripMenuItem.Text = "Estatística Scan";
            // 
            // bernoulliToolStripMenuItem
            // 
            this.bernoulliToolStripMenuItem.Name = "bernoulliToolStripMenuItem";
            this.bernoulliToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.bernoulliToolStripMenuItem.Text = "Bernoulli";
            this.bernoulliToolStripMenuItem.Click += new System.EventHandler(this.bernoulliToolStripMenuItem_Click_1);
            // 
            // poissonToolStripMenuItem
            // 
            this.poissonToolStripMenuItem.Name = "poissonToolStripMenuItem";
            this.poissonToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.poissonToolStripMenuItem.Text = "Poisson";
            this.poissonToolStripMenuItem.Click += new System.EventHandler(this.poissonToolStripMenuItem_Click_1);
            // 
            // conglomeradosEspaciaisToolStripMenuItem
            // 
            this.conglomeradosEspaciaisToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hierarquicoToolStripMenuItem,
            this.entropiaToolStripMenuItem});
            this.conglomeradosEspaciaisToolStripMenuItem.Name = "conglomeradosEspaciaisToolStripMenuItem";
            this.conglomeradosEspaciaisToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.conglomeradosEspaciaisToolStripMenuItem.Text = "Conglomerados Espaciais";
            // 
            // hierarquicoToolStripMenuItem
            // 
            this.hierarquicoToolStripMenuItem.Name = "hierarquicoToolStripMenuItem";
            this.hierarquicoToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.hierarquicoToolStripMenuItem.Text = "Hierárquico";
            this.hierarquicoToolStripMenuItem.Click += new System.EventHandler(this.hierarquicoToolStripMenuItem_Click_1);
            // 
            // entropiaToolStripMenuItem
            // 
            this.entropiaToolStripMenuItem.Enabled = false;
            this.entropiaToolStripMenuItem.Name = "entropiaToolStripMenuItem";
            this.entropiaToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.entropiaToolStripMenuItem.Text = "Entropia";
            this.entropiaToolStripMenuItem.Click += new System.EventHandler(this.entropiaToolStripMenuItem_Click_1);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // dsDados
            // 
            this.dsDados.DataSetName = "NewDataSet";
            this.dsDados.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable1});
            // 
            // dataTable1
            // 
            this.dataTable1.TableName = "Table1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.xpTabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.xpTabControl2);
            this.splitContainer1.Panel2MinSize = 75;
            this.splitContainer1.Size = new System.Drawing.Size(792, 541);
            this.splitContainer1.SplitterDistance = 226;
            this.splitContainer1.TabIndex = 2;
            // 
            // xpTabControl1
            // 
            this.xpTabControl1.Controls.Add(this.tabPage1);
            this.xpTabControl1.Controls.Add(this.tabPage2);
            this.xpTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xpTabControl1.Name = "xpTabControl1";
            this.xpTabControl1.SelectedIndex = 0;
            this.xpTabControl1.Size = new System.Drawing.Size(226, 541);
            this.xpTabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(218, 515);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Bases";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(212, 509);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.MouseEnter += new System.EventHandler(this.listView1_MouseEnter);
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            this.listView1.MouseLeave += new System.EventHandler(this.listView1_MouseLeave);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selecionarPastaToolStripMenuItem,
            this.toolStripSeparator5,
            this.iconesPequenosToolStripMenuItem,
            this.iconesGranesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(165, 76);
            // 
            // selecionarPastaToolStripMenuItem
            // 
            this.selecionarPastaToolStripMenuItem.Name = "selecionarPastaToolStripMenuItem";
            this.selecionarPastaToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.selecionarPastaToolStripMenuItem.Text = "Selecionar pasta";
            this.selecionarPastaToolStripMenuItem.Click += new System.EventHandler(this.selecionarPastaToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(161, 6);
            // 
            // iconesPequenosToolStripMenuItem
            // 
            this.iconesPequenosToolStripMenuItem.Name = "iconesPequenosToolStripMenuItem";
            this.iconesPequenosToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.iconesPequenosToolStripMenuItem.Text = "Lista";
            this.iconesPequenosToolStripMenuItem.Click += new System.EventHandler(this.iconesPequenosToolStripMenuItem_Click);
            // 
            // iconesGranesToolStripMenuItem
            // 
            this.iconesGranesToolStripMenuItem.Name = "iconesGranesToolStripMenuItem";
            this.iconesGranesToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.iconesGranesToolStripMenuItem.Text = "Icones";
            this.iconesGranesToolStripMenuItem.Click += new System.EventHandler(this.iconesGranesToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(218, 515);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Variáveis";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.contextMenuStrip2;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(3, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox1.Size = new System.Drawing.Size(212, 498);
            this.listBox1.TabIndex = 0;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selecionaTodosToolStripMenuItem,
            this.limpaTodosToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(161, 48);
            // 
            // selecionaTodosToolStripMenuItem
            // 
            this.selecionaTodosToolStripMenuItem.Name = "selecionaTodosToolStripMenuItem";
            this.selecionaTodosToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.selecionaTodosToolStripMenuItem.Text = "Seleciona todos";
            this.selecionaTodosToolStripMenuItem.Click += new System.EventHandler(this.selecionaTodosToolStripMenuItem_Click);
            // 
            // limpaTodosToolStripMenuItem
            // 
            this.limpaTodosToolStripMenuItem.Name = "limpaTodosToolStripMenuItem";
            this.limpaTodosToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.limpaTodosToolStripMenuItem.Text = "Limpa todos";
            this.limpaTodosToolStripMenuItem.Click += new System.EventHandler(this.limpaTodosToolStripMenuItem_Click);
            // 
            // xpTabControl2
            // 
            this.xpTabControl2.Controls.Add(this.tabPage3);
            this.xpTabControl2.Controls.Add(this.tabPage4);
            this.xpTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xpTabControl2.Location = new System.Drawing.Point(0, 0);
            this.xpTabControl2.Name = "xpTabControl2";
            this.xpTabControl2.SelectedIndex = 0;
            this.xpTabControl2.Size = new System.Drawing.Size(562, 541);
            this.xpTabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.mapImage1);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(554, 515);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Mapa";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // mapImage1
            // 
            this.mapImage1.ActiveTool = SharpMap.Forms.MapImage.Tools.None;
            this.mapImage1.BackColor = System.Drawing.Color.White;
            this.mapImage1.Cursor = System.Windows.Forms.Cursors.Cross;
            this.mapImage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapImage1.Location = new System.Drawing.Point(3, 3);
            map1.BackColor = System.Drawing.Color.Transparent;
            map1.Center = null;
            map1.Layers = null;
            map1.MapTransform = matrix1;
            map1.MaximumZoom = 1.7976931348623157E+308;
            map1.MinimumZoom = 0;
            map1.PixelAspectRatio = 1;
            map1.Size = new System.Drawing.Size(100, 50);
            map1.Zoom = 1;
            this.mapImage1.Map = map1;
            this.mapImage1.Name = "mapImage1";
            this.mapImage1.QueryLayerIndex = 0;
            this.mapImage1.Size = new System.Drawing.Size(548, 509);
            this.mapImage1.TabIndex = 2;
            this.mapImage1.TabStop = false;
            this.mapImage1.MouseMove += new SharpMap.Forms.MapImage.MouseEventHandler(this.mapImage1_MouseMove);
            this.mapImage1.MouseDown += new SharpMap.Forms.MapImage.MouseEventHandler(this.mapImage1_MouseDown_1);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dataGridView1);
            this.tabPage4.Location = new System.Drawing.Point(4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(554, 515);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Tabela";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(548, 509);
            this.dataGridView1.TabIndex = 0;
            // 
            // frmMapa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMapa";
            this.Text = "frmMapa";
            this.Load += new System.EventHandler(this.frmMapa_Load);
            this.MaximizedBoundsChanged += new System.EventHandler(this.frmMapa_MaximizedBoundsChanged_1);
            this.Resize += new System.EventHandler(this.frmMapa_Resize_1);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dsDados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.xpTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.xpTabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mapImage1)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton adDados;
        private System.Windows.Forms.ToolStripButton moveMapa;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton tlInformacao;
        private System.Windows.Forms.ToolStripSplitButton ToolspatialEstat;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Data.DataSet dsDados;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Data.DataTable dataTable1;
        private System.Windows.Forms.ToolStripButton mapaTematico;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolExportaDados;
        private System.Windows.Forms.ToolStripMenuItem estatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dependênciaEspacialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem índiceDeMoranToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lISAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modelosLinearesEspaciaisToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem bernoulliToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem poissonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem conglomeradosEspaciaisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hierarquicoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem entropiaToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton geoReferenciamento;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem tlMultivariada;
        private System.Windows.Forms.ToolStripMenuItem análiseDeConglomeradosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem índicesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ponderadoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem componentesPrincipaisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prometheéToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private VSControls.XPTabControl xpTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox listBox1;
        private VSControls.XPTabControl xpTabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private SharpMap.Forms.MapImage mapImage1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selecionarPastaToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem iconesPequenosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iconesGranesToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolRefresh;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem selecionaTodosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem limpaTodosToolStripMenuItem;
    }
}