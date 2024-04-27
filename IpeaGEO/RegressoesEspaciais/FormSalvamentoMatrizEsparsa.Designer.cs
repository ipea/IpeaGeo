namespace IpeaGeo.RegressoesEspaciais
{
    partial class FormSalvamentoMatrizEsparsa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSalvamentoMatrizEsparsa));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblResultados = new System.Windows.Forms.Label();
            this.btnImportarMatrizFromArquivo = new System.Windows.Forms.Button();
            this.btnVisualizar = new System.Windows.Forms.Button();
            this.grbTipoMatrizVizinhanca = new System.Windows.Forms.GroupBox();
            this.rdbMatrizVizinhancaNormalizada = new System.Windows.Forms.RadioButton();
            this.rdbMatrizVizinhancaOriginal = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdbListaVizinhos = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.rdbTripleFormat = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ckbNomeVarFirstLine = new System.Windows.Forms.CheckBox();
            this.rdbCountersStartingFromZero = new System.Windows.Forms.RadioButton();
            this.lstVariavelIdentificadora = new System.Windows.Forms.ListBox();
            this.rdbIdFromDataTable = new System.Windows.Forms.RadioButton();
            this.rdbCountersStartingFromOne = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnMatrizEsparsa = new System.Windows.Forms.Button();
            this.lblArquivo = new System.Windows.Forms.Label();
            this.btnExportar1 = new System.Windows.Forms.Button();
            this.btnArquivo1 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExportar = new System.Windows.Forms.Button();
            this.btnImportar = new System.Windows.Forms.Button();
            this.lblNomeArquivo = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richTextBoxResultadosEstimacao = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grbTipoMatrizVizinhanca.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabPage3.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this.btnOK);
            this.splitContainer1.Panel2.Controls.Add(this.btnClose);
            this.splitContainer1.Size = new System.Drawing.Size(889, 560);
            this.splitContainer1.SplitterDistance = 531;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(889, 531);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.ImageIndex = 3;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(881, 504);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Especificações";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblResultados);
            this.groupBox1.Controls.Add(this.btnImportarMatrizFromArquivo);
            this.groupBox1.Controls.Add(this.btnVisualizar);
            this.groupBox1.Controls.Add(this.grbTipoMatrizVizinhanca);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(875, 498);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Definições para a exportação de arquivos";
            // 
            // lblResultados
            // 
            this.lblResultados.BackColor = System.Drawing.Color.Transparent;
            this.lblResultados.ForeColor = System.Drawing.SystemColors.Desktop;
            this.lblResultados.Location = new System.Drawing.Point(26, 458);
            this.lblResultados.Name = "lblResultados";
            this.lblResultados.Size = new System.Drawing.Size(636, 31);
            this.lblResultados.TabIndex = 17;
            this.lblResultados.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnImportarMatrizFromArquivo
            // 
            this.btnImportarMatrizFromArquivo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportarMatrizFromArquivo.Location = new System.Drawing.Point(773, 462);
            this.btnImportarMatrizFromArquivo.Name = "btnImportarMatrizFromArquivo";
            this.btnImportarMatrizFromArquivo.Size = new System.Drawing.Size(75, 23);
            this.btnImportarMatrizFromArquivo.TabIndex = 16;
            this.btnImportarMatrizFromArquivo.Text = "&Importar";
            this.btnImportarMatrizFromArquivo.UseVisualStyleBackColor = true;
            this.btnImportarMatrizFromArquivo.Visible = false;
            this.btnImportarMatrizFromArquivo.Click += new System.EventHandler(this.btnImportarMatrizFromArquivo_Click);
            // 
            // btnVisualizar
            // 
            this.btnVisualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVisualizar.Location = new System.Drawing.Point(774, 462);
            this.btnVisualizar.Name = "btnVisualizar";
            this.btnVisualizar.Size = new System.Drawing.Size(75, 23);
            this.btnVisualizar.TabIndex = 15;
            this.btnVisualizar.Text = "&Visualizar";
            this.btnVisualizar.UseVisualStyleBackColor = true;
            this.btnVisualizar.Click += new System.EventHandler(this.btnVisualizar_Click);
            // 
            // grbTipoMatrizVizinhanca
            // 
            this.grbTipoMatrizVizinhanca.Controls.Add(this.rdbMatrizVizinhancaNormalizada);
            this.grbTipoMatrizVizinhanca.Controls.Add(this.rdbMatrizVizinhancaOriginal);
            this.grbTipoMatrizVizinhanca.Location = new System.Drawing.Point(601, 352);
            this.grbTipoMatrizVizinhanca.Name = "grbTipoMatrizVizinhanca";
            this.grbTipoMatrizVizinhanca.Size = new System.Drawing.Size(268, 104);
            this.grbTipoMatrizVizinhanca.TabIndex = 14;
            this.grbTipoMatrizVizinhanca.TabStop = false;
            this.grbTipoMatrizVizinhanca.Text = "Tipo de matriz de vizinhança";
            // 
            // rdbMatrizVizinhancaNormalizada
            // 
            this.rdbMatrizVizinhancaNormalizada.AutoSize = true;
            this.rdbMatrizVizinhancaNormalizada.Checked = true;
            this.rdbMatrizVizinhancaNormalizada.Location = new System.Drawing.Point(22, 58);
            this.rdbMatrizVizinhancaNormalizada.Name = "rdbMatrizVizinhancaNormalizada";
            this.rdbMatrizVizinhancaNormalizada.Size = new System.Drawing.Size(112, 17);
            this.rdbMatrizVizinhancaNormalizada.TabIndex = 1;
            this.rdbMatrizVizinhancaNormalizada.TabStop = true;
            this.rdbMatrizVizinhancaNormalizada.Text = "Matriz normalizada";
            this.rdbMatrizVizinhancaNormalizada.UseVisualStyleBackColor = true;
            this.rdbMatrizVizinhancaNormalizada.CheckedChanged += new System.EventHandler(this.rdbMatrizVizinhancaNormalizada_CheckedChanged);
            // 
            // rdbMatrizVizinhancaOriginal
            // 
            this.rdbMatrizVizinhancaOriginal.AutoSize = true;
            this.rdbMatrizVizinhancaOriginal.Location = new System.Drawing.Point(22, 35);
            this.rdbMatrizVizinhancaOriginal.Name = "rdbMatrizVizinhancaOriginal";
            this.rdbMatrizVizinhancaOriginal.Size = new System.Drawing.Size(89, 17);
            this.rdbMatrizVizinhancaOriginal.TabIndex = 0;
            this.rdbMatrizVizinhancaOriginal.Text = "Matriz original";
            this.rdbMatrizVizinhancaOriginal.UseVisualStyleBackColor = true;
            this.rdbMatrizVizinhancaOriginal.CheckedChanged += new System.EventHandler(this.rdbMatrizVizinhancaOriginal_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdbListaVizinhos);
            this.groupBox4.Controls.Add(this.radioButton4);
            this.groupBox4.Controls.Add(this.rdbTripleFormat);
            this.groupBox4.Location = new System.Drawing.Point(601, 28);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(268, 318);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Formato do arquivo";
            // 
            // rdbListaVizinhos
            // 
            this.rdbListaVizinhos.AutoSize = true;
            this.rdbListaVizinhos.Enabled = false;
            this.rdbListaVizinhos.Location = new System.Drawing.Point(22, 51);
            this.rdbListaVizinhos.Name = "rdbListaVizinhos";
            this.rdbListaVizinhos.Size = new System.Drawing.Size(103, 17);
            this.rdbListaVizinhos.TabIndex = 2;
            this.rdbListaVizinhos.Text = "Lista de vizinhos";
            this.rdbListaVizinhos.UseVisualStyleBackColor = true;
            this.rdbListaVizinhos.Visible = false;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Enabled = false;
            this.radioButton4.Location = new System.Drawing.Point(22, 74);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(128, 17);
            this.radioButton4.TabIndex = 1;
            this.radioButton4.Text = "Formato GAL (Geoda)";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.Visible = false;
            // 
            // rdbTripleFormat
            // 
            this.rdbTripleFormat.AutoSize = true;
            this.rdbTripleFormat.Checked = true;
            this.rdbTripleFormat.Location = new System.Drawing.Point(22, 28);
            this.rdbTripleFormat.Name = "rdbTripleFormat";
            this.rdbTripleFormat.Size = new System.Drawing.Size(91, 17);
            this.rdbTripleFormat.TabIndex = 0;
            this.rdbTripleFormat.TabStop = true;
            this.rdbTripleFormat.Text = "Formato triplet";
            this.rdbTripleFormat.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ckbNomeVarFirstLine);
            this.groupBox3.Controls.Add(this.rdbCountersStartingFromZero);
            this.groupBox3.Controls.Add(this.lstVariavelIdentificadora);
            this.groupBox3.Controls.Add(this.rdbIdFromDataTable);
            this.groupBox3.Controls.Add(this.rdbCountersStartingFromOne);
            this.groupBox3.Location = new System.Drawing.Point(6, 28);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(589, 428);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Variável de identificação";
            // 
            // ckbNomeVarFirstLine
            // 
            this.ckbNomeVarFirstLine.AutoSize = true;
            this.ckbNomeVarFirstLine.Checked = true;
            this.ckbNomeVarFirstLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbNomeVarFirstLine.Location = new System.Drawing.Point(23, 396);
            this.ckbNomeVarFirstLine.Name = "ckbNomeVarFirstLine";
            this.ckbNomeVarFirstLine.Size = new System.Drawing.Size(251, 17);
            this.ckbNomeVarFirstLine.TabIndex = 4;
            this.ckbNomeVarFirstLine.Text = "Nome das variáveis na primeira linha do arquivo";
            this.ckbNomeVarFirstLine.UseVisualStyleBackColor = true;
            // 
            // rdbCountersStartingFromZero
            // 
            this.rdbCountersStartingFromZero.AutoSize = true;
            this.rdbCountersStartingFromZero.Checked = true;
            this.rdbCountersStartingFromZero.Location = new System.Drawing.Point(23, 28);
            this.rdbCountersStartingFromZero.Name = "rdbCountersStartingFromZero";
            this.rdbCountersStartingFromZero.Size = new System.Drawing.Size(181, 17);
            this.rdbCountersStartingFromZero.TabIndex = 3;
            this.rdbCountersStartingFromZero.TabStop = true;
            this.rdbCountersStartingFromZero.Text = "Utilizar contadores (0, 1, 2, 3, ... )";
            this.rdbCountersStartingFromZero.UseVisualStyleBackColor = true;
            // 
            // lstVariavelIdentificadora
            // 
            this.lstVariavelIdentificadora.Enabled = false;
            this.lstVariavelIdentificadora.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstVariavelIdentificadora.FormattingEnabled = true;
            this.lstVariavelIdentificadora.HorizontalScrollbar = true;
            this.lstVariavelIdentificadora.ItemHeight = 14;
            this.lstVariavelIdentificadora.Location = new System.Drawing.Point(23, 111);
            this.lstVariavelIdentificadora.Name = "lstVariavelIdentificadora";
            this.lstVariavelIdentificadora.ScrollAlwaysVisible = true;
            this.lstVariavelIdentificadora.Size = new System.Drawing.Size(541, 270);
            this.lstVariavelIdentificadora.TabIndex = 2;
            // 
            // rdbIdFromDataTable
            // 
            this.rdbIdFromDataTable.AutoSize = true;
            this.rdbIdFromDataTable.Enabled = false;
            this.rdbIdFromDataTable.Location = new System.Drawing.Point(23, 74);
            this.rdbIdFromDataTable.Name = "rdbIdFromDataTable";
            this.rdbIdFromDataTable.Size = new System.Drawing.Size(256, 17);
            this.rdbIdFromDataTable.TabIndex = 1;
            this.rdbIdFromDataTable.Text = "Utilizar variável identificadora na tabela de dados";
            this.rdbIdFromDataTable.UseVisualStyleBackColor = true;
            this.rdbIdFromDataTable.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rdbCountersStartingFromOne
            // 
            this.rdbCountersStartingFromOne.AutoSize = true;
            this.rdbCountersStartingFromOne.Location = new System.Drawing.Point(23, 51);
            this.rdbCountersStartingFromOne.Name = "rdbCountersStartingFromOne";
            this.rdbCountersStartingFromOne.Size = new System.Drawing.Size(169, 17);
            this.rdbCountersStartingFromOne.TabIndex = 0;
            this.rdbCountersStartingFromOne.Text = "Utilizar contadores (1, 2, 3, ... )";
            this.rdbCountersStartingFromOne.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer3);
            this.tabPage2.ImageIndex = 0;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(881, 504);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Visualização";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.richTextBox1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.btnMatrizEsparsa);
            this.splitContainer3.Panel2.Controls.Add(this.lblArquivo);
            this.splitContainer3.Panel2.Controls.Add(this.btnExportar1);
            this.splitContainer3.Panel2.Controls.Add(this.btnArquivo1);
            this.splitContainer3.Size = new System.Drawing.Size(875, 498);
            this.splitContainer3.SplitterDistance = 456;
            this.splitContainer3.TabIndex = 0;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.richTextBox1.Size = new System.Drawing.Size(875, 456);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // btnMatrizEsparsa
            // 
            this.btnMatrizEsparsa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMatrizEsparsa.Enabled = false;
            this.btnMatrizEsparsa.Location = new System.Drawing.Point(773, 7);
            this.btnMatrizEsparsa.Name = "btnMatrizEsparsa";
            this.btnMatrizEsparsa.Size = new System.Drawing.Size(75, 23);
            this.btnMatrizEsparsa.TabIndex = 7;
            this.btnMatrizEsparsa.Text = "&Matriz ...";
            this.btnMatrizEsparsa.UseVisualStyleBackColor = true;
            this.btnMatrizEsparsa.Click += new System.EventHandler(this.btnMatrizEsparsa_Click);
            // 
            // lblArquivo
            // 
            this.lblArquivo.BackColor = System.Drawing.Color.LightGray;
            this.lblArquivo.Location = new System.Drawing.Point(119, 3);
            this.lblArquivo.Name = "lblArquivo";
            this.lblArquivo.Size = new System.Drawing.Size(636, 31);
            this.lblArquivo.TabIndex = 6;
            this.lblArquivo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnExportar1
            // 
            this.btnExportar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportar1.Location = new System.Drawing.Point(773, 7);
            this.btnExportar1.Name = "btnExportar1";
            this.btnExportar1.Size = new System.Drawing.Size(75, 23);
            this.btnExportar1.TabIndex = 5;
            this.btnExportar1.Text = "&Exportar";
            this.btnExportar1.UseVisualStyleBackColor = true;
            this.btnExportar1.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // btnArquivo1
            // 
            this.btnArquivo1.Location = new System.Drawing.Point(25, 7);
            this.btnArquivo1.Name = "btnArquivo1";
            this.btnArquivo1.Size = new System.Drawing.Size(75, 23);
            this.btnArquivo1.TabIndex = 4;
            this.btnArquivo1.Text = "&Arquivo ...";
            this.btnArquivo1.UseVisualStyleBackColor = true;
            this.btnArquivo1.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBox2);
            this.tabPage3.ImageIndex = 4;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(881, 504);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Info Matriz Esparsa";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBox2
            // 
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox2.Location = new System.Drawing.Point(0, 0);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.richTextBox2.Size = new System.Drawing.Size(881, 504);
            this.richTextBox2.TabIndex = 2;
            this.richTextBox2.Text = "";
            this.richTextBox2.WordWrap = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "application_lightning.png");
            this.imageList1.Images.SetKeyName(1, "page_2_copy.png");
            this.imageList1.Images.SetKeyName(2, "map.png");
            this.imageList1.Images.SetKeyName(3, "drive_disk.png");
            this.imageList1.Images.SetKeyName(4, "page_white_world.png");
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(780, -1);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(32, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "&Cancelar";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExportar
            // 
            this.btnExportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportar.Location = new System.Drawing.Point(777, 6);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(75, 23);
            this.btnExportar.TabIndex = 5;
            this.btnExportar.Text = "&Exportar";
            this.btnExportar.UseVisualStyleBackColor = true;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // btnImportar
            // 
            this.btnImportar.Location = new System.Drawing.Point(25, 6);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(75, 23);
            this.btnImportar.TabIndex = 6;
            this.btnImportar.Text = "&Arquivo ...";
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // lblNomeArquivo
            // 
            this.lblNomeArquivo.BackColor = System.Drawing.Color.LightGray;
            this.lblNomeArquivo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblNomeArquivo.Location = new System.Drawing.Point(118, 6);
            this.lblNomeArquivo.Name = "lblNomeArquivo";
            this.lblNomeArquivo.Size = new System.Drawing.Size(641, 23);
            this.lblNomeArquivo.TabIndex = 7;
            this.lblNomeArquivo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(875, 454);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // richTextBoxResultadosEstimacao
            // 
            this.richTextBoxResultadosEstimacao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxResultadosEstimacao.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxResultadosEstimacao.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxResultadosEstimacao.Name = "richTextBoxResultadosEstimacao";
            this.richTextBoxResultadosEstimacao.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.richTextBoxResultadosEstimacao.Size = new System.Drawing.Size(869, 435);
            this.richTextBoxResultadosEstimacao.TabIndex = 1;
            this.richTextBoxResultadosEstimacao.Text = "";
            this.richTextBoxResultadosEstimacao.WordWrap = false;
            // 
            // FormSalvamentoMatrizEsparsa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 560);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(803, 532);
            this.Name = "FormSalvamentoMatrizEsparsa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Importação e Salvamento da Matriz de Vizinhança";
            this.Load += new System.EventHandler(this.FormSalvamentoMatrizEsparsa_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.grbTipoMatrizVizinhanca.ResumeLayout(false);
            this.grbTipoMatrizVizinhanca.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdbIdFromDataTable;
        private System.Windows.Forms.RadioButton rdbCountersStartingFromOne;
        private System.Windows.Forms.ListBox lstVariavelIdentificadora;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton rdbTripleFormat;
        private System.Windows.Forms.RadioButton rdbCountersStartingFromZero;
        private System.Windows.Forms.GroupBox grbTipoMatrizVizinhanca;
        private System.Windows.Forms.RadioButton rdbMatrizVizinhancaNormalizada;
        private System.Windows.Forms.RadioButton rdbMatrizVizinhancaOriginal;
        private System.Windows.Forms.RadioButton rdbListaVizinhos;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnVisualizar;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.Label lblNomeArquivo;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox richTextBoxResultadosEstimacao;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button btnExportar1;
        private System.Windows.Forms.Button btnArquivo1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label lblArquivo;
        private System.Windows.Forms.Button btnImportarMatrizFromArquivo;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Label lblResultados;
        private System.Windows.Forms.Button btnMatrizEsparsa;
        private System.Windows.Forms.CheckBox ckbNomeVarFirstLine;
    }
}