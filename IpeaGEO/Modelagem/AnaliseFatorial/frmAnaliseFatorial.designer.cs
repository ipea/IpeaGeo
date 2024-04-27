namespace IpeaGeo.Modelagem
{
    partial class frmAnaliseFatorial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAnaliseFatorial));
            IpeaGeo.RegressoesEspaciais.clsIpeaShape clsIpeaShape1 = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.userControlDataGrid1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlDataGrid();
            this.tabPage0 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.userControlSelecaoVariaveis1 = new IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis();
            this.cmbrotation = new System.Windows.Forms.ComboBox();
            this.ckbrotation = new System.Windows.Forms.CheckBox();
            this.cmbmetodoescore = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbmetodoestimacao = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.ckbIncluirNovasVariaveisTabelaDados = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbinversacorrel = new System.Windows.Forms.CheckBox();
            this.ckbmatrizresidual = new System.Windows.Forms.CheckBox();
            this.ckbestimacaocoef = new System.Windows.Forms.CheckBox();
            this.ckbapresentacorrelacao = new System.Windows.Forms.CheckBox();
            this.ckbapresentavarcovar = new System.Windows.Forms.CheckBox();
            this.ckbescorecomponentes = new System.Windows.Forms.CheckBox();
            this.ckbBartlet = new System.Windows.Forms.CheckBox();
            this.btnExecutar = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
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
            this.splitContainer1.Size = new System.Drawing.Size(1070, 657);
            this.splitContainer1.SplitterDistance = 622;
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
            this.tabControl1.Size = new System.Drawing.Size(1070, 622);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.userControlDataGrid1);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1062, 595);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Tabela de Dados";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // userControlDataGrid1
            // 
            this.userControlDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDataGrid1.ListaVarsNumericas = new string[0];
            this.userControlDataGrid1.ListaVarsTotais = new string[0];
            this.userControlDataGrid1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataGrid1.Name = "userControlDataGrid1";
            clsIpeaShape1.CoordenadasEmRadianos = false;
            clsIpeaShape1.Count = 0;
            clsIpeaShape1.HoraCriacao = new System.DateTime(2013, 3, 6, 15, 50, 6, 550);
            clsIpeaShape1.MatrizAllDistances = null;
            clsIpeaShape1.Nome = "";
            clsIpeaShape1.OrdemVizinhanca = 1;
            clsIpeaShape1.Poligonos = new IpeaGeo.RegressoesEspaciais.clsIpeaPoligono[0];
            clsIpeaShape1.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
            clsIpeaShape1.TipoDistancia = false;
            clsIpeaShape1.TipoVizinhanca = "";
            this.userControlDataGrid1.Shape = clsIpeaShape1;
            this.userControlDataGrid1.Size = new System.Drawing.Size(1062, 595);
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
            this.tabPage0.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage0.Size = new System.Drawing.Size(1062, 595);
            this.tabPage0.TabIndex = 0;
            this.tabPage0.Text = "Especificações";
            this.tabPage0.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.userControlSelecaoVariaveis1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.cmbrotation);
            this.splitContainer2.Panel2.Controls.Add(this.ckbrotation);
            this.splitContainer2.Panel2.Controls.Add(this.cmbmetodoescore);
            this.splitContainer2.Panel2.Controls.Add(this.label3);
            this.splitContainer2.Panel2.Controls.Add(this.cmbmetodoestimacao);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Panel2.Controls.Add(this.numericUpDown1);
            this.splitContainer2.Panel2.Controls.Add(this.ckbIncluirNovasVariaveisTabelaDados);
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Panel2.Controls.Add(this.ckbescorecomponentes);
            this.splitContainer2.Panel2.Controls.Add(this.ckbBartlet);
            this.splitContainer2.Panel2.Controls.Add(this.btnExecutar);
            this.splitContainer2.Size = new System.Drawing.Size(1056, 589);
            this.splitContainer2.SplitterDistance = 764;
            this.splitContainer2.TabIndex = 0;
            // 
            // userControlSelecaoVariaveis1
            // 
            this.userControlSelecaoVariaveis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSelecaoVariaveis1.Location = new System.Drawing.Point(0, 0);
            this.userControlSelecaoVariaveis1.Name = "userControlSelecaoVariaveis1";
            this.userControlSelecaoVariaveis1.Size = new System.Drawing.Size(764, 589);
            this.userControlSelecaoVariaveis1.TabIndex = 0;
            this.userControlSelecaoVariaveis1.VariaveisDB = new string[0];
            this.userControlSelecaoVariaveis1.VariaveisIndependentes = new string[0];
            this.userControlSelecaoVariaveis1.VariaveisList = null;
            this.userControlSelecaoVariaveis1.Load += new System.EventHandler(this.userControlSelecaoVariaveis1_Load);
            // 
            // cmbrotation
            // 
            this.cmbrotation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbrotation.Enabled = false;
            this.cmbrotation.FormattingEnabled = true;
            this.cmbrotation.Items.AddRange(new object[] {
            "Quartimax",
            "Varimax",
            "Entropia Mínima",
            "Quartimin",
            "Bi-quartimin",
            "Covarimin"});
            this.cmbrotation.Location = new System.Drawing.Point(19, 163);
            this.cmbrotation.Name = "cmbrotation";
            this.cmbrotation.Size = new System.Drawing.Size(251, 21);
            this.cmbrotation.TabIndex = 20;
            // 
            // ckbrotation
            // 
            this.ckbrotation.AutoSize = true;
            this.ckbrotation.Location = new System.Drawing.Point(19, 140);
            this.ckbrotation.Name = "ckbrotation";
            this.ckbrotation.Size = new System.Drawing.Size(130, 17);
            this.ckbrotation.TabIndex = 21;
            this.ckbrotation.Text = "Rotações dos Fatores";
            this.ckbrotation.UseVisualStyleBackColor = true;
            this.ckbrotation.CheckedChanged += new System.EventHandler(this.ckbrotation_CheckedChanged);
            // 
            // cmbmetodoescore
            // 
            this.cmbmetodoescore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbmetodoescore.Enabled = false;
            this.cmbmetodoescore.FormattingEnabled = true;
            this.cmbmetodoescore.Items.AddRange(new object[] {
            "Mínimos Quadrados Ponderados",
            "Regressão"});
            this.cmbmetodoescore.Location = new System.Drawing.Point(19, 96);
            this.cmbmetodoescore.Name = "cmbmetodoescore";
            this.cmbmetodoescore.Size = new System.Drawing.Size(249, 21);
            this.cmbmetodoescore.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(232, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Método de Estimação da Matriz de Coeficientes";
            // 
            // cmbmetodoestimacao
            // 
            this.cmbmetodoestimacao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbmetodoestimacao.FormattingEnabled = true;
            this.cmbmetodoestimacao.Items.AddRange(new object[] {
            "Componentes Principais",
            "Fatores Principais",
            "Máxima Verossimilhança"});
            this.cmbmetodoestimacao.Location = new System.Drawing.Point(19, 33);
            this.cmbmetodoestimacao.Name = "cmbmetodoestimacao";
            this.cmbmetodoestimacao.Size = new System.Drawing.Size(249, 21);
            this.cmbmetodoestimacao.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 280);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(185, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Núm. de Fatores Gravados na Tabela";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(209, 278);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(59, 20);
            this.numericUpDown1.TabIndex = 14;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // ckbIncluirNovasVariaveisTabelaDados
            // 
            this.ckbIncluirNovasVariaveisTabelaDados.AutoSize = true;
            this.ckbIncluirNovasVariaveisTabelaDados.Enabled = false;
            this.ckbIncluirNovasVariaveisTabelaDados.Location = new System.Drawing.Point(19, 251);
            this.ckbIncluirNovasVariaveisTabelaDados.Name = "ckbIncluirNovasVariaveisTabelaDados";
            this.ckbIncluirNovasVariaveisTabelaDados.Size = new System.Drawing.Size(195, 17);
            this.ckbIncluirNovasVariaveisTabelaDados.TabIndex = 13;
            this.ckbIncluirNovasVariaveisTabelaDados.Text = "Mostrar escores na tabela de dados";
            this.ckbIncluirNovasVariaveisTabelaDados.UseVisualStyleBackColor = true;
            this.ckbIncluirNovasVariaveisTabelaDados.CheckedChanged += new System.EventHandler(this.ckbIncluirNovasVariaveisTabelaDados_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckbinversacorrel);
            this.groupBox1.Controls.Add(this.ckbmatrizresidual);
            this.groupBox1.Controls.Add(this.ckbestimacaocoef);
            this.groupBox1.Controls.Add(this.ckbapresentacorrelacao);
            this.groupBox1.Controls.Add(this.ckbapresentavarcovar);
            this.groupBox1.Location = new System.Drawing.Point(19, 328);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(249, 152);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Opções de Output";
            // 
            // ckbinversacorrel
            // 
            this.ckbinversacorrel.AutoSize = true;
            this.ckbinversacorrel.Location = new System.Drawing.Point(21, 119);
            this.ckbinversacorrel.Name = "ckbinversacorrel";
            this.ckbinversacorrel.Size = new System.Drawing.Size(179, 17);
            this.ckbinversacorrel.TabIndex = 12;
            this.ckbinversacorrel.Text = "Inversa da Matriz de Correlação ";
            this.ckbinversacorrel.UseVisualStyleBackColor = true;
            // 
            // ckbmatrizresidual
            // 
            this.ckbmatrizresidual.AutoSize = true;
            this.ckbmatrizresidual.Location = new System.Drawing.Point(21, 96);
            this.ckbmatrizresidual.Name = "ckbmatrizresidual";
            this.ckbmatrizresidual.Size = new System.Drawing.Size(98, 17);
            this.ckbmatrizresidual.TabIndex = 11;
            this.ckbmatrizresidual.Text = "Matriz Residual";
            this.ckbmatrizresidual.UseVisualStyleBackColor = true;
            // 
            // ckbestimacaocoef
            // 
            this.ckbestimacaocoef.AutoSize = true;
            this.ckbestimacaocoef.Location = new System.Drawing.Point(21, 73);
            this.ckbestimacaocoef.Name = "ckbestimacaocoef";
            this.ckbestimacaocoef.Size = new System.Drawing.Size(151, 17);
            this.ckbestimacaocoef.TabIndex = 10;
            this.ckbestimacaocoef.Text = "Estimação de Coeficientes";
            this.ckbestimacaocoef.UseVisualStyleBackColor = true;
            // 
            // ckbapresentacorrelacao
            // 
            this.ckbapresentacorrelacao.AutoSize = true;
            this.ckbapresentacorrelacao.Location = new System.Drawing.Point(21, 50);
            this.ckbapresentacorrelacao.Name = "ckbapresentacorrelacao";
            this.ckbapresentacorrelacao.Size = new System.Drawing.Size(123, 17);
            this.ckbapresentacorrelacao.TabIndex = 9;
            this.ckbapresentacorrelacao.Text = "Matriz de Correlação";
            this.ckbapresentacorrelacao.UseVisualStyleBackColor = true;
            // 
            // ckbapresentavarcovar
            // 
            this.ckbapresentavarcovar.AutoSize = true;
            this.ckbapresentavarcovar.Location = new System.Drawing.Point(21, 27);
            this.ckbapresentavarcovar.Name = "ckbapresentavarcovar";
            this.ckbapresentavarcovar.Size = new System.Drawing.Size(184, 17);
            this.ckbapresentavarcovar.TabIndex = 8;
            this.ckbapresentavarcovar.Text = "Matriz de Variância e Covariância";
            this.ckbapresentavarcovar.UseVisualStyleBackColor = true;
            // 
            // ckbescorecomponentes
            // 
            this.ckbescorecomponentes.AutoSize = true;
            this.ckbescorecomponentes.Location = new System.Drawing.Point(19, 73);
            this.ckbescorecomponentes.Name = "ckbescorecomponentes";
            this.ckbescorecomponentes.Size = new System.Drawing.Size(251, 17);
            this.ckbescorecomponentes.TabIndex = 11;
            this.ckbescorecomponentes.Text = "Escores dos Fatores Calculados com o Método:";
            this.ckbescorecomponentes.UseVisualStyleBackColor = true;
            this.ckbescorecomponentes.CheckedChanged += new System.EventHandler(this.ckbescorecomponentes_CheckedChanged);
            // 
            // ckbBartlet
            // 
            this.ckbBartlet.AutoSize = true;
            this.ckbBartlet.Location = new System.Drawing.Point(19, 211);
            this.ckbBartlet.Name = "ckbBartlet";
            this.ckbBartlet.Size = new System.Drawing.Size(101, 17);
            this.ckbBartlet.TabIndex = 12;
            this.ckbBartlet.Text = "Teste de Bartlet";
            this.ckbBartlet.UseVisualStyleBackColor = true;
            // 
            // btnExecutar
            // 
            this.btnExecutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecutar.Location = new System.Drawing.Point(188, 543);
            this.btnExecutar.Name = "btnExecutar";
            this.btnExecutar.Size = new System.Drawing.Size(75, 23);
            this.btnExecutar.TabIndex = 5;
            this.btnExecutar.Text = "&Executar";
            this.btnExecutar.UseVisualStyleBackColor = true;
            this.btnExecutar.Click += new System.EventHandler(this.btnExecutar_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.userControlRichTextOutput1);
            this.tabPage2.ImageIndex = 9;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1062, 595);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Resultados";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput1
            // 
            this.userControlRichTextOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput1.Location = new System.Drawing.Point(3, 3);
            this.userControlRichTextOutput1.Name = "userControlRichTextOutput1";
            this.userControlRichTextOutput1.Size = new System.Drawing.Size(1056, 589);
            this.userControlRichTextOutput1.TabIndex = 0;
            this.userControlRichTextOutput1.Texto = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.zedGraphControl1);
            this.tabPage3.ImageIndex = 4;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1062, 595);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Gráficos";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl1.Location = new System.Drawing.Point(0, 0);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(1062, 595);
            this.zedGraphControl1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.userControlRichTextOutput2);
            this.tabPage4.ImageIndex = 6;
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1062, 595);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "Escores";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput2
            // 
            this.userControlRichTextOutput2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput2.Location = new System.Drawing.Point(3, 3);
            this.userControlRichTextOutput2.Name = "userControlRichTextOutput2";
            this.userControlRichTextOutput2.Size = new System.Drawing.Size(1056, 589);
            this.userControlRichTextOutput2.TabIndex = 0;
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
            this.progressBar1.Location = new System.Drawing.Point(405, 10);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(477, 10);
            this.progressBar1.TabIndex = 12;
            this.progressBar1.Visible = false;
            // 
            // lblProgressBar
            // 
            this.lblProgressBar.AutoSize = true;
            this.lblProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressBar.ForeColor = System.Drawing.Color.Black;
            this.lblProgressBar.Location = new System.Drawing.Point(150, 7);
            this.lblProgressBar.Name = "lblProgressBar";
            this.lblProgressBar.Size = new System.Drawing.Size(195, 13);
            this.lblProgressBar.TabIndex = 11;
            this.lblProgressBar.Text = "Rotinas para análise de regressão linear";
            this.lblProgressBar.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(963, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&Atualizar";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Visible = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(31, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Fechar";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmAnaliseFatorial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 657);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAnaliseFatorial";
            this.Text = "Análise Fatorial";
            this.Load += new System.EventHandler(this.FormComponentesPrincipais_Load);
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
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage0;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabPage tabPage2;
        private IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis userControlSelecaoVariaveis1;
        private System.Windows.Forms.TabPage tabPage1;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput1;
        private System.Windows.Forms.Button btnExecutar;
        private System.Windows.Forms.TabPage tabPage3;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.CheckBox ckbescorecomponentes;
        private System.Windows.Forms.CheckBox ckbestimacaocoef;
        private System.Windows.Forms.CheckBox ckbapresentacorrelacao;
        private System.Windows.Forms.CheckBox ckbapresentavarcovar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.CheckBox ckbIncluirNovasVariaveisTabelaDados;
        private System.Windows.Forms.CheckBox ckbBartlet;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ckbmatrizresidual;
        private System.Windows.Forms.ComboBox cmbmetodoestimacao;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbmetodoescore;
        private System.Windows.Forms.CheckBox ckbrotation;
        private System.Windows.Forms.ComboBox cmbrotation;
        private System.Windows.Forms.CheckBox ckbinversacorrel;
        private System.Windows.Forms.ImageList imageList1;
        private RegressoesEspaciais.UserControls.UserControlDataGrid userControlDataGrid1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblProgressBar;
    }
}