namespace IpeaGeo.Modelagem.ComponentesPrincipais
{
    partial class FormComponentesPrincipais
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormComponentesPrincipais));
            IpeaGeo.RegressoesEspaciais.clsIpeaShape clsIpeaShape1 = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.userControlDataGrid1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlDataGrid();
            this.tabPage0 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.userControlSelecaoVariaveis1 = new IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.ckbIncluirNovasVariaveisTabelaDados = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbcorrelcompvar = new System.Windows.Forms.CheckBox();
            this.ckbapresentacorrelacao = new System.Windows.Forms.CheckBox();
            this.ckbapresentavarcovar = new System.Windows.Forms.CheckBox();
            this.ckbescorecomponentes = new System.Windows.Forms.CheckBox();
            this.ckbBartlet = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbtipomatriz = new System.Windows.Forms.ComboBox();
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
            this.splitContainer1.SplitterDistance = 620;
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
            this.tabControl1.Size = new System.Drawing.Size(1070, 620);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.userControlDataGrid1);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1062, 593);
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
            clsIpeaShape1.HoraCriacao = new System.DateTime(2012, 1, 9, 14, 20, 51, 707);
            clsIpeaShape1.MatrizAllDistances = null;
            clsIpeaShape1.Nome = "";
            clsIpeaShape1.OrdemVizinhanca = 1;
            clsIpeaShape1.Poligonos = new IpeaGeo.RegressoesEspaciais.clsIpeaPoligono[0];
            clsIpeaShape1.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
            clsIpeaShape1.TipoDistancia = false;
            clsIpeaShape1.TipoVizinhanca = "";
            this.userControlDataGrid1.Shape = clsIpeaShape1;
            this.userControlDataGrid1.Size = new System.Drawing.Size(1062, 593);
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
            this.tabPage0.Size = new System.Drawing.Size(1062, 593);
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
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Panel2.Controls.Add(this.numericUpDown1);
            this.splitContainer2.Panel2.Controls.Add(this.ckbIncluirNovasVariaveisTabelaDados);
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Panel2.Controls.Add(this.ckbescorecomponentes);
            this.splitContainer2.Panel2.Controls.Add(this.ckbBartlet);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Panel2.Controls.Add(this.cmbtipomatriz);
            this.splitContainer2.Panel2.Controls.Add(this.btnExecutar);
            this.splitContainer2.Size = new System.Drawing.Size(1056, 587);
            this.splitContainer2.SplitterDistance = 751;
            this.splitContainer2.TabIndex = 0;
            // 
            // userControlSelecaoVariaveis1
            // 
            this.userControlSelecaoVariaveis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSelecaoVariaveis1.Location = new System.Drawing.Point(0, 0);
            this.userControlSelecaoVariaveis1.Name = "userControlSelecaoVariaveis1";
            this.userControlSelecaoVariaveis1.Size = new System.Drawing.Size(751, 587);
            this.userControlSelecaoVariaveis1.TabIndex = 0;
            this.userControlSelecaoVariaveis1.VariaveisDB = new string[0];
            this.userControlSelecaoVariaveis1.VariaveisIndependentes = new string[0];
            this.userControlSelecaoVariaveis1.VariaveisList = null;
            this.userControlSelecaoVariaveis1.Load += new System.EventHandler(this.userControlSelecaoVariaveis1_Load);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Num. Componentes Gravados na Tabela";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(224, 146);
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
            this.ckbIncluirNovasVariaveisTabelaDados.Location = new System.Drawing.Point(21, 118);
            this.ckbIncluirNovasVariaveisTabelaDados.Name = "ckbIncluirNovasVariaveisTabelaDados";
            this.ckbIncluirNovasVariaveisTabelaDados.Size = new System.Drawing.Size(195, 17);
            this.ckbIncluirNovasVariaveisTabelaDados.TabIndex = 13;
            this.ckbIncluirNovasVariaveisTabelaDados.Text = "Mostrar escores na tabela de dados";
            this.ckbIncluirNovasVariaveisTabelaDados.UseVisualStyleBackColor = true;
            this.ckbIncluirNovasVariaveisTabelaDados.CheckedChanged += new System.EventHandler(this.ckbIncluirNovasVariaveisTabelaDados_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckbcorrelcompvar);
            this.groupBox1.Controls.Add(this.ckbapresentacorrelacao);
            this.groupBox1.Controls.Add(this.ckbapresentavarcovar);
            this.groupBox1.Location = new System.Drawing.Point(18, 191);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 106);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Opções de Output";
            // 
            // ckbcorrelcompvar
            // 
            this.ckbcorrelcompvar.AutoSize = true;
            this.ckbcorrelcompvar.Location = new System.Drawing.Point(19, 74);
            this.ckbcorrelcompvar.Name = "ckbcorrelcompvar";
            this.ckbcorrelcompvar.Size = new System.Drawing.Size(205, 17);
            this.ckbcorrelcompvar.TabIndex = 10;
            this.ckbcorrelcompvar.Text = "Correlação - Componentes x Variáveis";
            this.ckbcorrelcompvar.UseVisualStyleBackColor = true;
            // 
            // ckbapresentacorrelacao
            // 
            this.ckbapresentacorrelacao.AutoSize = true;
            this.ckbapresentacorrelacao.Location = new System.Drawing.Point(19, 51);
            this.ckbapresentacorrelacao.Name = "ckbapresentacorrelacao";
            this.ckbapresentacorrelacao.Size = new System.Drawing.Size(123, 17);
            this.ckbapresentacorrelacao.TabIndex = 9;
            this.ckbapresentacorrelacao.Text = "Matriz de Correlação";
            this.ckbapresentacorrelacao.UseVisualStyleBackColor = true;
            // 
            // ckbapresentavarcovar
            // 
            this.ckbapresentavarcovar.AutoSize = true;
            this.ckbapresentavarcovar.Location = new System.Drawing.Point(19, 28);
            this.ckbapresentavarcovar.Name = "ckbapresentavarcovar";
            this.ckbapresentavarcovar.Size = new System.Drawing.Size(184, 17);
            this.ckbapresentavarcovar.TabIndex = 8;
            this.ckbapresentavarcovar.Text = "Matriz de Variância e Covariância";
            this.ckbapresentavarcovar.UseVisualStyleBackColor = true;
            // 
            // ckbescorecomponentes
            // 
            this.ckbescorecomponentes.AutoSize = true;
            this.ckbescorecomponentes.Location = new System.Drawing.Point(21, 72);
            this.ckbescorecomponentes.Name = "ckbescorecomponentes";
            this.ckbescorecomponentes.Size = new System.Drawing.Size(204, 17);
            this.ckbescorecomponentes.TabIndex = 11;
            this.ckbescorecomponentes.Text = "Calcula os Escores dos Componentes";
            this.ckbescorecomponentes.UseVisualStyleBackColor = true;
            this.ckbescorecomponentes.CheckedChanged += new System.EventHandler(this.ckbescorecomponentes_CheckedChanged);
            // 
            // ckbBartlet
            // 
            this.ckbBartlet.AutoSize = true;
            this.ckbBartlet.Location = new System.Drawing.Point(21, 95);
            this.ckbBartlet.Name = "ckbBartlet";
            this.ckbBartlet.Size = new System.Drawing.Size(101, 17);
            this.ckbBartlet.TabIndex = 12;
            this.ckbBartlet.Text = "Teste de Bartlet";
            this.ckbBartlet.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Matriz para cálculos";
            // 
            // cmbtipomatriz
            // 
            this.cmbtipomatriz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbtipomatriz.FormattingEnabled = true;
            this.cmbtipomatriz.Items.AddRange(new object[] {
            "Matriz de Variância e Covariância",
            "Matriz de Correlação"});
            this.cmbtipomatriz.Location = new System.Drawing.Point(18, 29);
            this.cmbtipomatriz.Name = "cmbtipomatriz";
            this.cmbtipomatriz.Size = new System.Drawing.Size(265, 21);
            this.cmbtipomatriz.TabIndex = 6;
            // 
            // btnExecutar
            // 
            this.btnExecutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecutar.Location = new System.Drawing.Point(201, 541);
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
            this.tabPage2.ImageIndex = 10;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1062, 593);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Resultados";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput1
            // 
            this.userControlRichTextOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput1.Location = new System.Drawing.Point(3, 3);
            this.userControlRichTextOutput1.Name = "userControlRichTextOutput1";
            this.userControlRichTextOutput1.Size = new System.Drawing.Size(1056, 587);
            this.userControlRichTextOutput1.TabIndex = 0;
            this.userControlRichTextOutput1.Texto = "";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.zedGraphControl1);
            this.tabPage3.ImageIndex = 4;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1062, 593);
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
            this.zedGraphControl1.Size = new System.Drawing.Size(1062, 593);
            this.zedGraphControl1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.userControlRichTextOutput2);
            this.tabPage4.ImageIndex = 11;
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1062, 593);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "Escores";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput2
            // 
            this.userControlRichTextOutput2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput2.Location = new System.Drawing.Point(3, 3);
            this.userControlRichTextOutput2.Name = "userControlRichTextOutput2";
            this.userControlRichTextOutput2.Size = new System.Drawing.Size(1056, 587);
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
            this.imageList1.Images.SetKeyName(10, "report.png");
            this.imageList1.Images.SetKeyName(11, "application_view_detail.png");
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(400, 11);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(461, 10);
            this.progressBar1.TabIndex = 12;
            this.progressBar1.Visible = false;
            // 
            // lblProgressBar
            // 
            this.lblProgressBar.AutoSize = true;
            this.lblProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressBar.ForeColor = System.Drawing.Color.Black;
            this.lblProgressBar.Location = new System.Drawing.Point(141, 8);
            this.lblProgressBar.Name = "lblProgressBar";
            this.lblProgressBar.Size = new System.Drawing.Size(195, 13);
            this.lblProgressBar.TabIndex = 11;
            this.lblProgressBar.Text = "Rotinas para análise de regressão linear";
            this.lblProgressBar.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(964, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&Atualizar";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Visible = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(32, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "&Fechar";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormComponentesPrincipais
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 657);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormComponentesPrincipais";
            this.Text = "Componentes Principais";
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbtipomatriz;
        private System.Windows.Forms.CheckBox ckbescorecomponentes;
        private System.Windows.Forms.CheckBox ckbcorrelcompvar;
        private System.Windows.Forms.CheckBox ckbapresentacorrelacao;
        private System.Windows.Forms.CheckBox ckbapresentavarcovar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.CheckBox ckbIncluirNovasVariaveisTabelaDados;
        private System.Windows.Forms.CheckBox ckbBartlet;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList imageList1;
        private RegressoesEspaciais.UserControls.UserControlDataGrid userControlDataGrid1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblProgressBar;
    }
}