namespace IpeaGeo.Modelagem
{
    partial class FormDadosPainelNaoEspacial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDadosPainelNaoEspacial));
            IpeaGeo.RegressoesEspaciais.clsIpeaShape clsIpeaShape1 = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.userControlDataGrid1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlDataGrid();
            this.tabPage0 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.userControlRegressaoInstrumentos1 = new IpeaGeo.UserControlRegressaoInstrumentos();
            this.cmbVariavelPeriodosTempo = new System.Windows.Forms.ComboBox();
            this.cmbVariavelUnidadesObservacionais = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnOpções = new System.Windows.Forms.Button();
            this.grbGMM = new System.Windows.Forms.GroupBox();
            this.rdbFirstDiferencesEstimator = new System.Windows.Forms.RadioButton();
            this.cktGeneralFGLS = new System.Windows.Forms.CheckBox();
            this.ckbCovMatrizRobusta = new System.Windows.Forms.CheckBox();
            this.rdbRandomEffects = new System.Windows.Forms.RadioButton();
            this.rdbPooledOLS = new System.Windows.Forms.RadioButton();
            this.rdbFixedEffects = new System.Windows.Forms.RadioButton();
            this.btnExecutar = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExecutar2 = new System.Windows.Forms.Button();
            this.ckbTendenciaTemporalCubica = new System.Windows.Forms.CheckBox();
            this.ckbIncluiTendenciaTemporalQuadratica = new System.Windows.Forms.CheckBox();
            this.ckbIncluiTendenciaTemporalLinear = new System.Windows.Forms.CheckBox();
            this.ckbIncluiDummiesTemporais = new System.Windows.Forms.CheckBox();
            this.ckbMulticolinearidade = new System.Windows.Forms.CheckBox();
            this.ckbApresentaCovMatrixBetaHat = new System.Windows.Forms.CheckBox();
            this.ckbIncluiIntercepto = new System.Windows.Forms.CheckBox();
            this.ckbIncluirNovasVariaveisTabelaDados = new System.Windows.Forms.CheckBox();
            this.ckbLimpaJanelaNovasVariaveis = new System.Windows.Forms.CheckBox();
            this.ckbLimpaJanelaOutput = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput2 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput3 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblProgressBar = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grbGMM.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Ipea_GEO.ico");
            this.imageList1.Images.SetKeyName(1, "application_cascade.png");
            this.imageList1.Images.SetKeyName(2, "application_form.png");
            this.imageList1.Images.SetKeyName(3, "chart_curve.png");
            this.imageList1.Images.SetKeyName(4, "chart_bar.png");
            this.imageList1.Images.SetKeyName(5, "photos.png");
            this.imageList1.Images.SetKeyName(6, "map.png");
            this.imageList1.Images.SetKeyName(7, "databases.png");
            this.imageList1.Images.SetKeyName(8, "database.png");
            this.imageList1.Images.SetKeyName(9, "page_refresh.png");
            this.imageList1.Images.SetKeyName(10, "report.png");
            this.imageList1.Images.SetKeyName(11, "application_view_columns.png");
            this.imageList1.Images.SetKeyName(12, "photos.png");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.progressBar1);
            this.splitContainer1.Panel2.Controls.Add(this.lblProgressBar);
            this.splitContainer1.Panel2.Controls.Add(this.btnOK);
            this.splitContainer1.Panel2.Controls.Add(this.btnClose);
            this.splitContainer1.Size = new System.Drawing.Size(1162, 693);
            this.splitContainer1.SplitterDistance = 657;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage0);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1162, 657);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.userControlDataGrid1);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1154, 630);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tabela de Dados";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // userControlDataGrid1
            // 
            this.userControlDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDataGrid1.ListaVarsNumericas = new string[0];
            this.userControlDataGrid1.ListaVarsTotais = new string[0];
            this.userControlDataGrid1.Location = new System.Drawing.Point(3, 3);
            this.userControlDataGrid1.Name = "userControlDataGrid1";
            clsIpeaShape1.CoordenadasEmRadianos = false;
            clsIpeaShape1.Count = 0;
            clsIpeaShape1.HoraCriacao = new System.DateTime(2012, 1, 25, 13, 48, 23, 704);
            clsIpeaShape1.MatrizAllDistances = null;
            clsIpeaShape1.Nome = "";
            clsIpeaShape1.OrdemVizinhanca = 1;
            clsIpeaShape1.Poligonos = new IpeaGeo.RegressoesEspaciais.clsIpeaPoligono[0];
            clsIpeaShape1.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
            clsIpeaShape1.TipoDistancia = false;
            clsIpeaShape1.TipoVizinhanca = "";
            this.userControlDataGrid1.Shape = clsIpeaShape1;
            this.userControlDataGrid1.Size = new System.Drawing.Size(1148, 624);
            this.userControlDataGrid1.TabControl = null;
            this.userControlDataGrid1.TabIndex = 0;
            this.userControlDataGrid1.UserControlPropScoreMatching = null;
            this.userControlDataGrid1.UserControlRegInstrumentos = null;
            this.userControlDataGrid1.UserControlSelecao2BlocosVariaveis = null;
            this.userControlDataGrid1.UserControlSelecaoVariaveis = null;
            // 
            // tabPage0
            // 
            this.tabPage0.Controls.Add(this.splitContainer2);
            this.tabPage0.ImageIndex = 2;
            this.tabPage0.Location = new System.Drawing.Point(4, 23);
            this.tabPage0.Name = "tabPage0";
            this.tabPage0.Size = new System.Drawing.Size(1154, 630);
            this.tabPage0.TabIndex = 3;
            this.tabPage0.Text = "Especificações";
            this.tabPage0.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(1154, 630);
            this.splitContainer2.SplitterDistance = 815;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.userControlRegressaoInstrumentos1);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.cmbVariavelPeriodosTempo);
            this.splitContainer4.Panel2.Controls.Add(this.cmbVariavelUnidadesObservacionais);
            this.splitContainer4.Panel2.Controls.Add(this.label2);
            this.splitContainer4.Panel2.Controls.Add(this.label1);
            this.splitContainer4.Size = new System.Drawing.Size(815, 630);
            this.splitContainer4.SplitterDistance = 562;
            this.splitContainer4.TabIndex = 1;
            // 
            // userControlRegressaoInstrumentos1
            // 
            this.userControlRegressaoInstrumentos1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRegressaoInstrumentos1.Location = new System.Drawing.Point(0, 0);
            this.userControlRegressaoInstrumentos1.Name = "userControlRegressaoInstrumentos1";
            this.userControlRegressaoInstrumentos1.Size = new System.Drawing.Size(815, 562);
            this.userControlRegressaoInstrumentos1.TabIndex = 0;
            this.userControlRegressaoInstrumentos1.VariaveisDB = null;
            this.userControlRegressaoInstrumentos1.VariaveisIndependentes = new string[0];
            this.userControlRegressaoInstrumentos1.VariaveisInstrumentais = new string[0];
            this.userControlRegressaoInstrumentos1.VariaveisList = null;
            this.userControlRegressaoInstrumentos1.VariavelDependente = new string[0];
            // 
            // cmbVariavelPeriodosTempo
            // 
            this.cmbVariavelPeriodosTempo.FormattingEnabled = true;
            this.cmbVariavelPeriodosTempo.Location = new System.Drawing.Point(259, 33);
            this.cmbVariavelPeriodosTempo.Name = "cmbVariavelPeriodosTempo";
            this.cmbVariavelPeriodosTempo.Size = new System.Drawing.Size(521, 21);
            this.cmbVariavelPeriodosTempo.TabIndex = 32;
            // 
            // cmbVariavelUnidadesObservacionais
            // 
            this.cmbVariavelUnidadesObservacionais.FormattingEnabled = true;
            this.cmbVariavelUnidadesObservacionais.Location = new System.Drawing.Point(259, 9);
            this.cmbVariavelUnidadesObservacionais.Name = "cmbVariavelUnidadesObservacionais";
            this.cmbVariavelUnidadesObservacionais.Size = new System.Drawing.Size(521, 21);
            this.cmbVariavelUnidadesObservacionais.TabIndex = 31;
            this.cmbVariavelUnidadesObservacionais.SelectedIndexChanged += new System.EventHandler(this.cmbVariavelUnidadesObservacionais_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(21, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Variável indicadora de períodos de tempo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(21, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Variável indicadora de unidades observacionais";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnOpções);
            this.groupBox2.Controls.Add(this.grbGMM);
            this.groupBox2.Controls.Add(this.btnExecutar);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(335, 630);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Opções";
            // 
            // btnOpções
            // 
            this.btnOpções.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpções.Enabled = false;
            this.btnOpções.Location = new System.Drawing.Point(31, 585);
            this.btnOpções.Name = "btnOpções";
            this.btnOpções.Size = new System.Drawing.Size(75, 23);
            this.btnOpções.TabIndex = 28;
            this.btnOpções.Text = "&Opções";
            this.btnOpções.UseVisualStyleBackColor = true;
            this.btnOpções.Click += new System.EventHandler(this.btnOpções_Click);
            // 
            // grbGMM
            // 
            this.grbGMM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grbGMM.Controls.Add(this.rdbFirstDiferencesEstimator);
            this.grbGMM.Controls.Add(this.cktGeneralFGLS);
            this.grbGMM.Controls.Add(this.ckbCovMatrizRobusta);
            this.grbGMM.Controls.Add(this.rdbRandomEffects);
            this.grbGMM.Controls.Add(this.rdbPooledOLS);
            this.grbGMM.Controls.Add(this.rdbFixedEffects);
            this.grbGMM.Location = new System.Drawing.Point(19, 19);
            this.grbGMM.Name = "grbGMM";
            this.grbGMM.Size = new System.Drawing.Size(292, 203);
            this.grbGMM.TabIndex = 27;
            this.grbGMM.TabStop = false;
            this.grbGMM.Text = "Método de Estimação";
            // 
            // rdbFirstDiferencesEstimator
            // 
            this.rdbFirstDiferencesEstimator.AutoSize = true;
            this.rdbFirstDiferencesEstimator.Location = new System.Drawing.Point(21, 98);
            this.rdbFirstDiferencesEstimator.Name = "rdbFirstDiferencesEstimator";
            this.rdbFirstDiferencesEstimator.Size = new System.Drawing.Size(119, 17);
            this.rdbFirstDiferencesEstimator.TabIndex = 31;
            this.rdbFirstDiferencesEstimator.Text = "Primeiras diferenças";
            this.rdbFirstDiferencesEstimator.UseVisualStyleBackColor = true;
            this.rdbFirstDiferencesEstimator.CheckedChanged += new System.EventHandler(this.rdbFirstDiferencesEstimator_CheckedChanged);
            // 
            // cktGeneralFGLS
            // 
            this.cktGeneralFGLS.AutoSize = true;
            this.cktGeneralFGLS.Location = new System.Drawing.Point(21, 160);
            this.cktGeneralFGLS.Name = "cktGeneralFGLS";
            this.cktGeneralFGLS.Size = new System.Drawing.Size(179, 17);
            this.cktGeneralFGLS.TabIndex = 30;
            this.cktGeneralFGLS.Text = "FGLS geral para a matriz Omega";
            this.cktGeneralFGLS.UseVisualStyleBackColor = true;
            this.cktGeneralFGLS.CheckedChanged += new System.EventHandler(this.cktGeneralFGLS_CheckedChanged);
            // 
            // ckbCovMatrizRobusta
            // 
            this.ckbCovMatrizRobusta.AutoSize = true;
            this.ckbCovMatrizRobusta.Location = new System.Drawing.Point(21, 137);
            this.ckbCovMatrizRobusta.Name = "ckbCovMatrizRobusta";
            this.ckbCovMatrizRobusta.Size = new System.Drawing.Size(231, 17);
            this.ckbCovMatrizRobusta.TabIndex = 29;
            this.ckbCovMatrizRobusta.Text = "Estimação robusta da matriz de covariância";
            this.ckbCovMatrizRobusta.UseVisualStyleBackColor = true;
            this.ckbCovMatrizRobusta.CheckedChanged += new System.EventHandler(this.ckbCovMatrizRobusta_CheckedChanged);
            // 
            // rdbRandomEffects
            // 
            this.rdbRandomEffects.AutoSize = true;
            this.rdbRandomEffects.Location = new System.Drawing.Point(21, 75);
            this.rdbRandomEffects.Name = "rdbRandomEffects";
            this.rdbRandomEffects.Size = new System.Drawing.Size(105, 17);
            this.rdbRandomEffects.TabIndex = 6;
            this.rdbRandomEffects.Text = "Efeitos aleatórios";
            this.rdbRandomEffects.UseVisualStyleBackColor = true;
            this.rdbRandomEffects.CheckedChanged += new System.EventHandler(this.rdbRandomEffects_CheckedChanged);
            // 
            // rdbPooledOLS
            // 
            this.rdbPooledOLS.AutoSize = true;
            this.rdbPooledOLS.Checked = true;
            this.rdbPooledOLS.Location = new System.Drawing.Point(21, 29);
            this.rdbPooledOLS.Name = "rdbPooledOLS";
            this.rdbPooledOLS.Size = new System.Drawing.Size(82, 17);
            this.rdbPooledOLS.TabIndex = 4;
            this.rdbPooledOLS.TabStop = true;
            this.rdbPooledOLS.Text = "Pooled OLS";
            this.rdbPooledOLS.UseVisualStyleBackColor = true;
            this.rdbPooledOLS.CheckedChanged += new System.EventHandler(this.rdbPooledOLS_CheckedChanged);
            // 
            // rdbFixedEffects
            // 
            this.rdbFixedEffects.AutoSize = true;
            this.rdbFixedEffects.Location = new System.Drawing.Point(21, 52);
            this.rdbFixedEffects.Name = "rdbFixedEffects";
            this.rdbFixedEffects.Size = new System.Drawing.Size(175, 17);
            this.rdbFixedEffects.TabIndex = 3;
            this.rdbFixedEffects.Text = "Efeitos fixos (estimador \"within\")";
            this.rdbFixedEffects.UseVisualStyleBackColor = true;
            this.rdbFixedEffects.CheckedChanged += new System.EventHandler(this.rdbFixedEffects_CheckedChanged);
            // 
            // btnExecutar
            // 
            this.btnExecutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecutar.Location = new System.Drawing.Point(231, 585);
            this.btnExecutar.Name = "btnExecutar";
            this.btnExecutar.Size = new System.Drawing.Size(75, 23);
            this.btnExecutar.TabIndex = 22;
            this.btnExecutar.Text = "&Executar";
            this.btnExecutar.UseVisualStyleBackColor = true;
            this.btnExecutar.Click += new System.EventHandler(this.btnExecutar_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.groupBox1);
            this.tabPage6.ImageIndex = 12;
            this.tabPage6.Location = new System.Drawing.Point(4, 23);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(1154, 630);
            this.tabPage6.TabIndex = 6;
            this.tabPage6.Text = "Opções para Estimação";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnExecutar2);
            this.groupBox1.Controls.Add(this.ckbTendenciaTemporalCubica);
            this.groupBox1.Controls.Add(this.ckbIncluiTendenciaTemporalQuadratica);
            this.groupBox1.Controls.Add(this.ckbIncluiTendenciaTemporalLinear);
            this.groupBox1.Controls.Add(this.ckbIncluiDummiesTemporais);
            this.groupBox1.Controls.Add(this.ckbMulticolinearidade);
            this.groupBox1.Controls.Add(this.ckbApresentaCovMatrixBetaHat);
            this.groupBox1.Controls.Add(this.ckbIncluiIntercepto);
            this.groupBox1.Controls.Add(this.ckbIncluirNovasVariaveisTabelaDados);
            this.groupBox1.Controls.Add(this.ckbLimpaJanelaNovasVariaveis);
            this.groupBox1.Controls.Add(this.ckbLimpaJanelaOutput);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1154, 630);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnExecutar2
            // 
            this.btnExecutar2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecutar2.Location = new System.Drawing.Point(1050, 585);
            this.btnExecutar2.Name = "btnExecutar2";
            this.btnExecutar2.Size = new System.Drawing.Size(75, 23);
            this.btnExecutar2.TabIndex = 45;
            this.btnExecutar2.Text = "&Executar";
            this.btnExecutar2.UseVisualStyleBackColor = true;
            this.btnExecutar2.Click += new System.EventHandler(this.btnExecutar2_Click);
            // 
            // ckbTendenciaTemporalCubica
            // 
            this.ckbTendenciaTemporalCubica.AutoSize = true;
            this.ckbTendenciaTemporalCubica.Location = new System.Drawing.Point(49, 142);
            this.ckbTendenciaTemporalCubica.Name = "ckbTendenciaTemporalCubica";
            this.ckbTendenciaTemporalCubica.Size = new System.Drawing.Size(179, 17);
            this.ckbTendenciaTemporalCubica.TabIndex = 44;
            this.ckbTendenciaTemporalCubica.Text = "Inclui tendência temporal cúbica";
            this.ckbTendenciaTemporalCubica.UseVisualStyleBackColor = true;
            this.ckbTendenciaTemporalCubica.CheckedChanged += new System.EventHandler(this.ckbTendenciaTemporalCubica_CheckedChanged);
            // 
            // ckbIncluiTendenciaTemporalQuadratica
            // 
            this.ckbIncluiTendenciaTemporalQuadratica.AutoSize = true;
            this.ckbIncluiTendenciaTemporalQuadratica.Location = new System.Drawing.Point(49, 119);
            this.ckbIncluiTendenciaTemporalQuadratica.Name = "ckbIncluiTendenciaTemporalQuadratica";
            this.ckbIncluiTendenciaTemporalQuadratica.Size = new System.Drawing.Size(197, 17);
            this.ckbIncluiTendenciaTemporalQuadratica.TabIndex = 43;
            this.ckbIncluiTendenciaTemporalQuadratica.Text = "Inclui tendência temporal quadrática";
            this.ckbIncluiTendenciaTemporalQuadratica.UseVisualStyleBackColor = true;
            this.ckbIncluiTendenciaTemporalQuadratica.CheckedChanged += new System.EventHandler(this.ckbIncluiTendenciaTemporalQuadratica_CheckedChanged);
            // 
            // ckbIncluiTendenciaTemporalLinear
            // 
            this.ckbIncluiTendenciaTemporalLinear.AutoSize = true;
            this.ckbIncluiTendenciaTemporalLinear.Location = new System.Drawing.Point(49, 96);
            this.ckbIncluiTendenciaTemporalLinear.Name = "ckbIncluiTendenciaTemporalLinear";
            this.ckbIncluiTendenciaTemporalLinear.Size = new System.Drawing.Size(172, 17);
            this.ckbIncluiTendenciaTemporalLinear.TabIndex = 42;
            this.ckbIncluiTendenciaTemporalLinear.Text = "Inclui tendência temporal linear";
            this.ckbIncluiTendenciaTemporalLinear.UseVisualStyleBackColor = true;
            this.ckbIncluiTendenciaTemporalLinear.CheckedChanged += new System.EventHandler(this.ckbIncluiTendenciaTemporalLinear_CheckedChanged);
            // 
            // ckbIncluiDummiesTemporais
            // 
            this.ckbIncluiDummiesTemporais.AutoSize = true;
            this.ckbIncluiDummiesTemporais.Location = new System.Drawing.Point(49, 73);
            this.ckbIncluiDummiesTemporais.Name = "ckbIncluiDummiesTemporais";
            this.ckbIncluiDummiesTemporais.Size = new System.Drawing.Size(143, 17);
            this.ckbIncluiDummiesTemporais.TabIndex = 41;
            this.ckbIncluiDummiesTemporais.Text = "Inclui dummies temporais";
            this.ckbIncluiDummiesTemporais.UseVisualStyleBackColor = true;
            this.ckbIncluiDummiesTemporais.CheckedChanged += new System.EventHandler(this.ckbIncluiDummiesTemporais_CheckedChanged);
            // 
            // ckbMulticolinearidade
            // 
            this.ckbMulticolinearidade.AutoSize = true;
            this.ckbMulticolinearidade.Location = new System.Drawing.Point(49, 211);
            this.ckbMulticolinearidade.Name = "ckbMulticolinearidade";
            this.ckbMulticolinearidade.Size = new System.Drawing.Size(162, 17);
            this.ckbMulticolinearidade.TabIndex = 40;
            this.ckbMulticolinearidade.Text = "Análise de multicolinearidade";
            this.ckbMulticolinearidade.UseVisualStyleBackColor = true;
            // 
            // ckbApresentaCovMatrixBetaHat
            // 
            this.ckbApresentaCovMatrixBetaHat.AutoSize = true;
            this.ckbApresentaCovMatrixBetaHat.Location = new System.Drawing.Point(49, 188);
            this.ckbApresentaCovMatrixBetaHat.Name = "ckbApresentaCovMatrixBetaHat";
            this.ckbApresentaCovMatrixBetaHat.Size = new System.Drawing.Size(257, 17);
            this.ckbApresentaCovMatrixBetaHat.TabIndex = 39;
            this.ckbApresentaCovMatrixBetaHat.Text = "Apresenta matriz de covariância dos coeficientes";
            this.ckbApresentaCovMatrixBetaHat.UseVisualStyleBackColor = true;
            // 
            // ckbIncluiIntercepto
            // 
            this.ckbIncluiIntercepto.AutoSize = true;
            this.ckbIncluiIntercepto.Checked = true;
            this.ckbIncluiIntercepto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbIncluiIntercepto.Location = new System.Drawing.Point(49, 52);
            this.ckbIncluiIntercepto.Name = "ckbIncluiIntercepto";
            this.ckbIncluiIntercepto.Size = new System.Drawing.Size(101, 17);
            this.ckbIncluiIntercepto.TabIndex = 38;
            this.ckbIncluiIntercepto.Text = "Inclui intercepto";
            this.ckbIncluiIntercepto.UseVisualStyleBackColor = true;
            // 
            // ckbIncluirNovasVariaveisTabelaDados
            // 
            this.ckbIncluirNovasVariaveisTabelaDados.AutoSize = true;
            this.ckbIncluirNovasVariaveisTabelaDados.Location = new System.Drawing.Point(49, 303);
            this.ckbIncluirNovasVariaveisTabelaDados.Name = "ckbIncluirNovasVariaveisTabelaDados";
            this.ckbIncluirNovasVariaveisTabelaDados.Size = new System.Drawing.Size(232, 17);
            this.ckbIncluirNovasVariaveisTabelaDados.TabIndex = 37;
            this.ckbIncluirNovasVariaveisTabelaDados.Text = "Mostrar novas variáveis na tabela de dados";
            this.ckbIncluirNovasVariaveisTabelaDados.UseVisualStyleBackColor = true;
            // 
            // ckbLimpaJanelaNovasVariaveis
            // 
            this.ckbLimpaJanelaNovasVariaveis.AutoSize = true;
            this.ckbLimpaJanelaNovasVariaveis.Checked = true;
            this.ckbLimpaJanelaNovasVariaveis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbLimpaJanelaNovasVariaveis.Location = new System.Drawing.Point(49, 280);
            this.ckbLimpaJanelaNovasVariaveis.Name = "ckbLimpaJanelaNovasVariaveis";
            this.ckbLimpaJanelaNovasVariaveis.Size = new System.Drawing.Size(252, 17);
            this.ckbLimpaJanelaNovasVariaveis.TabIndex = 36;
            this.ckbLimpaJanelaNovasVariaveis.Text = "Limpar janela novas variáveis a cada estimação";
            this.ckbLimpaJanelaNovasVariaveis.UseVisualStyleBackColor = true;
            // 
            // ckbLimpaJanelaOutput
            // 
            this.ckbLimpaJanelaOutput.AutoSize = true;
            this.ckbLimpaJanelaOutput.Checked = true;
            this.ckbLimpaJanelaOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbLimpaJanelaOutput.Location = new System.Drawing.Point(49, 257);
            this.ckbLimpaJanelaOutput.Name = "ckbLimpaJanelaOutput";
            this.ckbLimpaJanelaOutput.Size = new System.Drawing.Size(177, 17);
            this.ckbLimpaJanelaOutput.TabIndex = 35;
            this.ckbLimpaJanelaOutput.Text = "Limpar output a cada estimação";
            this.ckbLimpaJanelaOutput.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.userControlRichTextOutput1);
            this.tabPage2.ImageIndex = 10;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1154, 630);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Resultados";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput1
            // 
            this.userControlRichTextOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput1.Location = new System.Drawing.Point(3, 3);
            this.userControlRichTextOutput1.Name = "userControlRichTextOutput1";
            this.userControlRichTextOutput1.Size = new System.Drawing.Size(1148, 624);
            this.userControlRichTextOutput1.TabIndex = 0;
            this.userControlRichTextOutput1.Texto = "";
            // 
            // tabPage3
            // 
            this.tabPage3.ImageIndex = 4;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1154, 630);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Gráficos";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.userControlRichTextOutput2);
            this.tabPage4.ImageIndex = 11;
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1154, 630);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "Variáveis Geradas";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput2
            // 
            this.userControlRichTextOutput2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput2.Location = new System.Drawing.Point(0, 0);
            this.userControlRichTextOutput2.Name = "userControlRichTextOutput2";
            this.userControlRichTextOutput2.Size = new System.Drawing.Size(1154, 630);
            this.userControlRichTextOutput2.TabIndex = 1;
            this.userControlRichTextOutput2.Texto = "";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.userControlRichTextOutput3);
            this.tabPage5.ImageIndex = 11;
            this.tabPage5.Location = new System.Drawing.Point(4, 23);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1154, 630);
            this.tabPage5.TabIndex = 5;
            this.tabPage5.Text = "Efeitos Idiossincráticos";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput3
            // 
            this.userControlRichTextOutput3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput3.Location = new System.Drawing.Point(0, 0);
            this.userControlRichTextOutput3.Name = "userControlRichTextOutput3";
            this.userControlRichTextOutput3.Size = new System.Drawing.Size(1154, 630);
            this.userControlRichTextOutput3.TabIndex = 2;
            this.userControlRichTextOutput3.Texto = "";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(603, 9);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(221, 10);
            this.progressBar1.TabIndex = 10;
            this.progressBar1.Visible = false;
            // 
            // lblProgressBar
            // 
            this.lblProgressBar.AutoSize = true;
            this.lblProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblProgressBar.Location = new System.Drawing.Point(136, 7);
            this.lblProgressBar.Name = "lblProgressBar";
            this.lblProgressBar.Size = new System.Drawing.Size(235, 13);
            this.lblProgressBar.TabIndex = 9;
            this.lblProgressBar.Text = "Rotinas para análise de dados de painel";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(1054, 1);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&Atualizar";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Visible = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(28, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Fechar";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormDadosPainelNaoEspacial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1162, 693);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1170, 720);
            this.Name = "FormDadosPainelNaoEspacial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Regressão Linear com Dados de Painel";
            this.Load += new System.EventHandler(this.FormBaseModelagem_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage0.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.grbGMM.ResumeLayout(false);
            this.grbGMM.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tabPage0;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblProgressBar;
        private System.Windows.Forms.TabPage tabPage4;
        private UserControlRegressaoInstrumentos userControlRegressaoInstrumentos1;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput2;
        private System.Windows.Forms.Button btnExecutar;
        private System.Windows.Forms.GroupBox grbGMM;
        private System.Windows.Forms.RadioButton rdbRandomEffects;
        private System.Windows.Forms.RadioButton rdbPooledOLS;
        private System.Windows.Forms.RadioButton rdbFixedEffects;
        private System.Windows.Forms.CheckBox ckbCovMatrizRobusta;
        private System.Windows.Forms.CheckBox cktGeneralFGLS;
        private System.Windows.Forms.RadioButton rdbFirstDiferencesEstimator;
        private System.Windows.Forms.TabPage tabPage5;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.ComboBox cmbVariavelPeriodosTempo;
        private System.Windows.Forms.ComboBox cmbVariavelUnidadesObservacionais;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private RegressoesEspaciais.UserControls.UserControlDataGrid userControlDataGrid1;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ckbTendenciaTemporalCubica;
        private System.Windows.Forms.CheckBox ckbIncluiTendenciaTemporalQuadratica;
        private System.Windows.Forms.CheckBox ckbIncluiTendenciaTemporalLinear;
        private System.Windows.Forms.CheckBox ckbIncluiDummiesTemporais;
        private System.Windows.Forms.CheckBox ckbMulticolinearidade;
        private System.Windows.Forms.CheckBox ckbApresentaCovMatrixBetaHat;
        private System.Windows.Forms.CheckBox ckbIncluiIntercepto;
        private System.Windows.Forms.CheckBox ckbIncluirNovasVariaveisTabelaDados;
        private System.Windows.Forms.CheckBox ckbLimpaJanelaNovasVariaveis;
        private System.Windows.Forms.CheckBox ckbLimpaJanelaOutput;
        private System.Windows.Forms.Button btnOpções;
        private System.Windows.Forms.Button btnExecutar2;
    }
}