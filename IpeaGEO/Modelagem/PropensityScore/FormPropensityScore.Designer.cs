namespace IpeaGeo.Modelagem
{
    partial class FormPropensityScore
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPropensityScore));
            IpeaGeo.RegressoesEspaciais.clsIpeaShape clsIpeaShape1 = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.userControlDataGrid1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlDataGrid();
            this.tabPage0 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.userControlPropensityScoreMatching1 = new IpeaGeo.UserControlPropensityScoreMatching();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDown_numeroTratamentos = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbOutcome = new System.Windows.Forms.ComboBox();
            this.ckbIncluirNovasVariaveisTabelaDados = new System.Windows.Forms.CheckBox();
            this.ckbLimpaJanelaNovasVariaveis = new System.Windows.Forms.CheckBox();
            this.ckbLimpaJanelaOutput = new System.Windows.Forms.CheckBox();
            this.btnPSM = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxAT = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbEstrato = new System.Windows.Forms.ComboBox();
            this.gbxBandWidth = new System.Windows.Forms.GroupBox();
            this.rdbBandwidthA = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBoxBandwidth = new System.Windows.Forms.TextBox();
            this.rdbStratification = new System.Windows.Forms.RadioButton();
            this.comboBoxListaKernel = new System.Windows.Forms.ComboBox();
            this.rdbKernel = new System.Windows.Forms.RadioButton();
            this.rdbNearestNeighbo = new System.Windows.Forms.RadioButton();
            this.grbGMM = new System.Windows.Forms.GroupBox();
            this.rdbCompLogLog = new System.Windows.Forms.RadioButton();
            this.rdbLinkLogit = new System.Windows.Forms.RadioButton();
            this.rdbProbit = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput2 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
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
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numeroTratamentos)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.gbxBandWidth.SuspendLayout();
            this.grbGMM.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer1.Size = new System.Drawing.Size(1070, 686);
            this.splitContainer1.SplitterDistance = 650;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage0);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1070, 650);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.userControlDataGrid1);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1062, 623);
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
            clsIpeaShape1.HoraCriacao = new System.DateTime(2012, 1, 10, 8, 52, 37, 969);
            clsIpeaShape1.MatrizAllDistances = null;
            clsIpeaShape1.Nome = "";
            clsIpeaShape1.OrdemVizinhanca = 1;
            clsIpeaShape1.Poligonos = new IpeaGeo.RegressoesEspaciais.clsIpeaPoligono[0];
            clsIpeaShape1.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
            clsIpeaShape1.TipoDistancia = false;
            clsIpeaShape1.TipoVizinhanca = "";
            this.userControlDataGrid1.Shape = clsIpeaShape1;
            this.userControlDataGrid1.Size = new System.Drawing.Size(1056, 617);
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
            this.tabPage0.Size = new System.Drawing.Size(1062, 623);
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
            this.splitContainer2.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(1062, 623);
            this.splitContainer2.SplitterDistance = 748;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.userControlPropensityScoreMatching1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(748, 623);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Regressão binária para cálculo do propensity score";
            // 
            // userControlPropensityScoreMatching1
            // 
            this.userControlPropensityScoreMatching1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlPropensityScoreMatching1.Location = new System.Drawing.Point(3, 16);
            this.userControlPropensityScoreMatching1.Name = "userControlPropensityScoreMatching1";
            this.userControlPropensityScoreMatching1.Size = new System.Drawing.Size(742, 604);
            this.userControlPropensityScoreMatching1.TabIndex = 0;
            this.userControlPropensityScoreMatching1.VariaveisDB = null;
            this.userControlPropensityScoreMatching1.VariaveisIndependentes = new string[0];
            this.userControlPropensityScoreMatching1.VariaveisInstrumentais = new string[0];
            this.userControlPropensityScoreMatching1.VariaveisList = null;
            this.userControlPropensityScoreMatching1.VariavelDependente = new string[0];
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDown_numeroTratamentos);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmbOutcome);
            this.groupBox2.Controls.Add(this.ckbIncluirNovasVariaveisTabelaDados);
            this.groupBox2.Controls.Add(this.ckbLimpaJanelaNovasVariaveis);
            this.groupBox2.Controls.Add(this.ckbLimpaJanelaOutput);
            this.groupBox2.Controls.Add(this.btnPSM);
            this.groupBox2.Controls.Add(this.groupBox1);
            this.groupBox2.Controls.Add(this.grbGMM);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(310, 623);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Opções";
            // 
            // numericUpDown_numeroTratamentos
            // 
            this.numericUpDown_numeroTratamentos.Location = new System.Drawing.Point(185, 92);
            this.numericUpDown_numeroTratamentos.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_numeroTratamentos.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_numeroTratamentos.Name = "numericUpDown_numeroTratamentos";
            this.numericUpDown_numeroTratamentos.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown_numeroTratamentos.TabIndex = 29;
            this.numericUpDown_numeroTratamentos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_numeroTratamentos.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_numeroTratamentos.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Numero de Tratamentos";
            this.label4.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Variável de desempenho do tratamento";
            // 
            // cmbOutcome
            // 
            this.cmbOutcome.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOutcome.FormattingEnabled = true;
            this.cmbOutcome.Location = new System.Drawing.Point(18, 47);
            this.cmbOutcome.Name = "cmbOutcome";
            this.cmbOutcome.Size = new System.Drawing.Size(216, 21);
            this.cmbOutcome.TabIndex = 25;
            this.cmbOutcome.Click += new System.EventHandler(this.cmbOutcome_Click);
            // 
            // ckbIncluirNovasVariaveisTabelaDados
            // 
            this.ckbIncluirNovasVariaveisTabelaDados.AutoSize = true;
            this.ckbIncluirNovasVariaveisTabelaDados.Location = new System.Drawing.Point(18, 508);
            this.ckbIncluirNovasVariaveisTabelaDados.Name = "ckbIncluirNovasVariaveisTabelaDados";
            this.ckbIncluirNovasVariaveisTabelaDados.Size = new System.Drawing.Size(232, 17);
            this.ckbIncluirNovasVariaveisTabelaDados.TabIndex = 24;
            this.ckbIncluirNovasVariaveisTabelaDados.Text = "Mostrar novas variáveis na tabela de dados";
            this.ckbIncluirNovasVariaveisTabelaDados.UseVisualStyleBackColor = true;
            // 
            // ckbLimpaJanelaNovasVariaveis
            // 
            this.ckbLimpaJanelaNovasVariaveis.AutoSize = true;
            this.ckbLimpaJanelaNovasVariaveis.Checked = true;
            this.ckbLimpaJanelaNovasVariaveis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbLimpaJanelaNovasVariaveis.Location = new System.Drawing.Point(18, 485);
            this.ckbLimpaJanelaNovasVariaveis.Name = "ckbLimpaJanelaNovasVariaveis";
            this.ckbLimpaJanelaNovasVariaveis.Size = new System.Drawing.Size(252, 17);
            this.ckbLimpaJanelaNovasVariaveis.TabIndex = 23;
            this.ckbLimpaJanelaNovasVariaveis.Text = "Limpar janela novas variáveis a cada estimação";
            this.ckbLimpaJanelaNovasVariaveis.UseVisualStyleBackColor = true;
            // 
            // ckbLimpaJanelaOutput
            // 
            this.ckbLimpaJanelaOutput.AutoSize = true;
            this.ckbLimpaJanelaOutput.Checked = true;
            this.ckbLimpaJanelaOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbLimpaJanelaOutput.Location = new System.Drawing.Point(18, 462);
            this.ckbLimpaJanelaOutput.Name = "ckbLimpaJanelaOutput";
            this.ckbLimpaJanelaOutput.Size = new System.Drawing.Size(177, 17);
            this.ckbLimpaJanelaOutput.TabIndex = 22;
            this.ckbLimpaJanelaOutput.Text = "Limpar output a cada estimação";
            this.ckbLimpaJanelaOutput.UseVisualStyleBackColor = true;
            // 
            // btnPSM
            // 
            this.btnPSM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPSM.Location = new System.Drawing.Point(211, 570);
            this.btnPSM.Name = "btnPSM";
            this.btnPSM.Size = new System.Drawing.Size(75, 23);
            this.btnPSM.TabIndex = 12;
            this.btnPSM.Text = "&Estimar";
            this.btnPSM.UseVisualStyleBackColor = true;
            this.btnPSM.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxAT);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbEstrato);
            this.groupBox1.Controls.Add(this.gbxBandWidth);
            this.groupBox1.Controls.Add(this.rdbStratification);
            this.groupBox1.Controls.Add(this.comboBoxListaKernel);
            this.groupBox1.Controls.Add(this.rdbKernel);
            this.groupBox1.Controls.Add(this.rdbNearestNeighbo);
            this.groupBox1.Location = new System.Drawing.Point(18, 240);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(275, 202);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tipo de matching";
            // 
            // cbxAT
            // 
            this.cbxAT.FormattingEnabled = true;
            this.cbxAT.Items.AddRange(new object[] {
            "ATT",
            "ATE"});
            this.cbxAT.Location = new System.Drawing.Point(193, 19);
            this.cbxAT.Name = "cbxAT";
            this.cbxAT.Size = new System.Drawing.Size(68, 21);
            this.cbxAT.TabIndex = 30;
            this.cbxAT.Text = "ATT";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Variável bloco (estrato)";
            this.label2.Visible = false;
            // 
            // cmbEstrato
            // 
            this.cmbEstrato.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstrato.FormattingEnabled = true;
            this.cmbEstrato.Location = new System.Drawing.Point(20, 158);
            this.cmbEstrato.Name = "cmbEstrato";
            this.cmbEstrato.Size = new System.Drawing.Size(216, 21);
            this.cmbEstrato.TabIndex = 26;
            this.cmbEstrato.Visible = false;
            this.cmbEstrato.Click += new System.EventHandler(this.cmbEstrato_Click);
            // 
            // gbxBandWidth
            // 
            this.gbxBandWidth.Controls.Add(this.rdbBandwidthA);
            this.gbxBandWidth.Controls.Add(this.label3);
            this.gbxBandWidth.Controls.Add(this.txtBoxBandwidth);
            this.gbxBandWidth.Location = new System.Drawing.Point(13, 71);
            this.gbxBandWidth.Name = "gbxBandWidth";
            this.gbxBandWidth.Size = new System.Drawing.Size(234, 45);
            this.gbxBandWidth.TabIndex = 25;
            this.gbxBandWidth.TabStop = false;
            this.gbxBandWidth.Text = "Bandwidth";
            this.gbxBandWidth.Visible = false;
            // 
            // rdbBandwidthA
            // 
            this.rdbBandwidthA.AutoSize = true;
            this.rdbBandwidthA.Location = new System.Drawing.Point(7, 18);
            this.rdbBandwidthA.Name = "rdbBandwidthA";
            this.rdbBandwidthA.Size = new System.Drawing.Size(79, 17);
            this.rdbBandwidthA.TabIndex = 25;
            this.rdbBandwidthA.Text = "Automático";
            this.rdbBandwidthA.UseVisualStyleBackColor = true;
            this.rdbBandwidthA.CheckedChanged += new System.EventHandler(this.rdbBandwidthA_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(89, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Manual";
            // 
            // txtBoxBandwidth
            // 
            this.txtBoxBandwidth.Enabled = false;
            this.txtBoxBandwidth.Location = new System.Drawing.Point(134, 16);
            this.txtBoxBandwidth.Name = "txtBoxBandwidth";
            this.txtBoxBandwidth.Size = new System.Drawing.Size(91, 20);
            this.txtBoxBandwidth.TabIndex = 15;
            // 
            // rdbStratification
            // 
            this.rdbStratification.AutoSize = true;
            this.rdbStratification.Location = new System.Drawing.Point(18, 122);
            this.rdbStratification.Name = "rdbStratification";
            this.rdbStratification.Size = new System.Drawing.Size(83, 17);
            this.rdbStratification.TabIndex = 21;
            this.rdbStratification.Text = "Stratification";
            this.rdbStratification.UseVisualStyleBackColor = true;
            this.rdbStratification.CheckedChanged += new System.EventHandler(this.rdbStratification_CheckedChanged);
            // 
            // comboBoxListaKernel
            // 
            this.comboBoxListaKernel.Enabled = false;
            this.comboBoxListaKernel.FormattingEnabled = true;
            this.comboBoxListaKernel.Items.AddRange(new object[] {
            "Gaussiano",
            "Epanechnikov",
            "Biweigth",
            "Triangular",
            "Retangular"});
            this.comboBoxListaKernel.Location = new System.Drawing.Point(79, 47);
            this.comboBoxListaKernel.Name = "comboBoxListaKernel";
            this.comboBoxListaKernel.Size = new System.Drawing.Size(168, 21);
            this.comboBoxListaKernel.TabIndex = 20;
            this.comboBoxListaKernel.Visible = false;
            // 
            // rdbKernel
            // 
            this.rdbKernel.AutoSize = true;
            this.rdbKernel.Location = new System.Drawing.Point(18, 48);
            this.rdbKernel.Name = "rdbKernel";
            this.rdbKernel.Size = new System.Drawing.Size(55, 17);
            this.rdbKernel.TabIndex = 19;
            this.rdbKernel.TabStop = true;
            this.rdbKernel.Text = "Kernel";
            this.rdbKernel.UseVisualStyleBackColor = true;
            this.rdbKernel.CheckedChanged += new System.EventHandler(this.rdbKernel_CheckedChanged);
            // 
            // rdbNearestNeighbo
            // 
            this.rdbNearestNeighbo.AutoSize = true;
            this.rdbNearestNeighbo.Location = new System.Drawing.Point(18, 24);
            this.rdbNearestNeighbo.Name = "rdbNearestNeighbo";
            this.rdbNearestNeighbo.Size = new System.Drawing.Size(132, 17);
            this.rdbNearestNeighbo.TabIndex = 4;
            this.rdbNearestNeighbo.Text = "Nearest Neighborhood";
            this.rdbNearestNeighbo.UseVisualStyleBackColor = true;
            // 
            // grbGMM
            // 
            this.grbGMM.Controls.Add(this.rdbCompLogLog);
            this.grbGMM.Controls.Add(this.rdbLinkLogit);
            this.grbGMM.Controls.Add(this.rdbProbit);
            this.grbGMM.Location = new System.Drawing.Point(18, 124);
            this.grbGMM.Name = "grbGMM";
            this.grbGMM.Size = new System.Drawing.Size(275, 104);
            this.grbGMM.TabIndex = 10;
            this.grbGMM.TabStop = false;
            this.grbGMM.Text = "Funções de ligação";
            // 
            // rdbCompLogLog
            // 
            this.rdbCompLogLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rdbCompLogLog.AutoSize = true;
            this.rdbCompLogLog.Location = new System.Drawing.Point(18, 71);
            this.rdbCompLogLog.Name = "rdbCompLogLog";
            this.rdbCompLogLog.Size = new System.Drawing.Size(131, 17);
            this.rdbCompLogLog.TabIndex = 6;
            this.rdbCompLogLog.Text = "Complementary log-log";
            this.rdbCompLogLog.UseVisualStyleBackColor = true;
            // 
            // rdbLinkLogit
            // 
            this.rdbLinkLogit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rdbLinkLogit.AutoSize = true;
            this.rdbLinkLogit.Location = new System.Drawing.Point(18, 25);
            this.rdbLinkLogit.Name = "rdbLinkLogit";
            this.rdbLinkLogit.Size = new System.Drawing.Size(48, 17);
            this.rdbLinkLogit.TabIndex = 4;
            this.rdbLinkLogit.Text = "Logit";
            this.rdbLinkLogit.UseVisualStyleBackColor = true;
            // 
            // rdbProbit
            // 
            this.rdbProbit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rdbProbit.AutoSize = true;
            this.rdbProbit.Location = new System.Drawing.Point(18, 48);
            this.rdbProbit.Name = "rdbProbit";
            this.rdbProbit.Size = new System.Drawing.Size(52, 17);
            this.rdbProbit.TabIndex = 3;
            this.rdbProbit.Text = "Probit";
            this.rdbProbit.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.userControlRichTextOutput1);
            this.tabPage2.ImageIndex = 9;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1062, 623);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Resultados";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput1
            // 
            this.userControlRichTextOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput1.Location = new System.Drawing.Point(3, 3);
            this.userControlRichTextOutput1.Name = "userControlRichTextOutput1";
            this.userControlRichTextOutput1.Size = new System.Drawing.Size(1056, 617);
            this.userControlRichTextOutput1.TabIndex = 0;
            this.userControlRichTextOutput1.Texto = "";
            // 
            // tabPage3
            // 
            this.tabPage3.ImageIndex = 4;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1062, 623);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Gráficos";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.userControlRichTextOutput2);
            this.tabPage4.ImageIndex = 7;
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1062, 623);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "Variáveis Geradas";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput2
            // 
            this.userControlRichTextOutput2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput2.Location = new System.Drawing.Point(0, 0);
            this.userControlRichTextOutput2.Name = "userControlRichTextOutput2";
            this.userControlRichTextOutput2.Size = new System.Drawing.Size(1062, 623);
            this.userControlRichTextOutput2.TabIndex = 2;
            this.userControlRichTextOutput2.Texto = "";
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
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(412, 9);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(412, 10);
            this.progressBar1.TabIndex = 10;
            this.progressBar1.Visible = false;
            // 
            // lblProgressBar
            // 
            this.lblProgressBar.AutoSize = true;
            this.lblProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressBar.ForeColor = System.Drawing.Color.Black;
            this.lblProgressBar.Location = new System.Drawing.Point(136, 7);
            this.lblProgressBar.Name = "lblProgressBar";
            this.lblProgressBar.Size = new System.Drawing.Size(204, 13);
            this.lblProgressBar.TabIndex = 9;
            this.lblProgressBar.Text = "Estimação via propensity scores matching";
            this.lblProgressBar.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(967, 1);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&Atualizar";
            this.btnOK.UseVisualStyleBackColor = true;
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
            // FormPropensityScore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 686);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(991, 620);
            this.Name = "FormPropensityScore";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Propensity Score Matching";
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
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_numeroTratamentos)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbxBandWidth.ResumeLayout(false);
            this.gbxBandWidth.PerformLayout();
            this.grbGMM.ResumeLayout(false);
            this.grbGMM.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
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
        private UserControlPropensityScoreMatching userControlPropensityScoreMatching1;
        private System.Windows.Forms.GroupBox grbGMM;
        private System.Windows.Forms.RadioButton rdbCompLogLog;
        private System.Windows.Forms.RadioButton rdbLinkLogit;
        private System.Windows.Forms.RadioButton rdbProbit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbNearestNeighbo;
        private System.Windows.Forms.Button btnPSM;
        private System.Windows.Forms.CheckBox ckbIncluirNovasVariaveisTabelaDados;
        private System.Windows.Forms.CheckBox ckbLimpaJanelaNovasVariaveis;
        private System.Windows.Forms.CheckBox ckbLimpaJanelaOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbOutcome;
        private System.Windows.Forms.TabPage tabPage4;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown_numeroTratamentos;
        private System.Windows.Forms.TextBox txtBoxBandwidth;
        private System.Windows.Forms.ComboBox comboBoxListaKernel;
        private System.Windows.Forms.RadioButton rdbKernel;
        private RegressoesEspaciais.UserControls.UserControlDataGrid userControlDataGrid1;
        private System.Windows.Forms.RadioButton rdbStratification;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox gbxBandWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox rdbBandwidthA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbEstrato;
        private System.Windows.Forms.ComboBox cbxAT;
    }
}