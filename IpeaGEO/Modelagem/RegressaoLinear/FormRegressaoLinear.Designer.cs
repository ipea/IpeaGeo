namespace IpeaGEO.Modelagem
{
    partial class FormRegressaoLinear
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegressaoLinear));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportarDadosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calculadoraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.excluirVariáveisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.estatísticasDescritivasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarMatrizDeVizinhançaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importarMatrizDeVizinhançaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grbImportacaoDosArquivos = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnImportarTabela = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbTabelasNoArquivo = new System.Windows.Forms.ComboBox();
            this.btnAbrirTabelaDados = new System.Windows.Forms.Button();
            this.btnAbrirArquivoShape = new System.Windows.Forms.Button();
            this.tabPage0 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput1 = new IpeaGEO.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.userControlRegressaoInstrumentos1 = new IpeaGEO.UserControlRegressaoInstrumentos();
            this.btnExecutar = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.grbImportacaoDosArquivos.SuspendLayout();
            this.tabPage0.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
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
            this.splitContainer1.Size = new System.Drawing.Size(983, 586);
            this.splitContainer1.SplitterDistance = 550;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage0);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(983, 550);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer3);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(975, 523);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tabela de Dados";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.grbImportacaoDosArquivos);
            this.splitContainer3.Size = new System.Drawing.Size(969, 517);
            this.splitContainer3.SplitterDistance = 465;
            this.splitContainer3.TabIndex = 0;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.Size = new System.Drawing.Size(969, 465);
            this.dataGridView1.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem8,
            this.toolStripSeparator1,
            this.exportarDadosToolStripMenuItem,
            this.calculadoraToolStripMenuItem,
            this.toolStripMenuItem6,
            this.excluirVariáveisToolStripMenuItem,
            this.toolStripSeparator3,
            this.estatísticasDescritivasToolStripMenuItem,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11,
            this.toolStripSeparator2,
            this.toolStripMenuItem7,
            this.exportarMatrizDeVizinhançaToolStripMenuItem,
            this.importarMatrizDeVizinhançaToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.contextMenuStrip1.Size = new System.Drawing.Size(261, 330);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItem4.Text = "Importar arquivo shape";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItem5.Text = "Escolher arquivo de dados";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Enabled = false;
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItem8.Text = "Importar tabela";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(257, 6);
            // 
            // exportarDadosToolStripMenuItem
            // 
            this.exportarDadosToolStripMenuItem.Name = "exportarDadosToolStripMenuItem";
            this.exportarDadosToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.exportarDadosToolStripMenuItem.Text = "Exportar dados";
            // 
            // calculadoraToolStripMenuItem
            // 
            this.calculadoraToolStripMenuItem.Name = "calculadoraToolStripMenuItem";
            this.calculadoraToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.calculadoraToolStripMenuItem.Text = "Calculadora";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItem6.Text = "Medidas para os polígonos";
            // 
            // excluirVariáveisToolStripMenuItem
            // 
            this.excluirVariáveisToolStripMenuItem.Name = "excluirVariáveisToolStripMenuItem";
            this.excluirVariáveisToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.excluirVariáveisToolStripMenuItem.Text = "Excluir variáveis";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(257, 6);
            // 
            // estatísticasDescritivasToolStripMenuItem
            // 
            this.estatísticasDescritivasToolStripMenuItem.Name = "estatísticasDescritivasToolStripMenuItem";
            this.estatísticasDescritivasToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.estatísticasDescritivasToolStripMenuItem.Text = "Estatísticas descritivas";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItem9.Text = "Tabelas de frequências";
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItem10.Text = "Tabulações cruzadas";
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItem11.Text = "Correlações";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(257, 6);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItem7.Text = "Matriz de vizinhança pelas distâncias";
            // 
            // exportarMatrizDeVizinhançaToolStripMenuItem
            // 
            this.exportarMatrizDeVizinhançaToolStripMenuItem.Name = "exportarMatrizDeVizinhançaToolStripMenuItem";
            this.exportarMatrizDeVizinhançaToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.exportarMatrizDeVizinhançaToolStripMenuItem.Text = "Exportar matriz de vizinhança";
            // 
            // importarMatrizDeVizinhançaToolStripMenuItem
            // 
            this.importarMatrizDeVizinhançaToolStripMenuItem.Name = "importarMatrizDeVizinhançaToolStripMenuItem";
            this.importarMatrizDeVizinhançaToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.importarMatrizDeVizinhançaToolStripMenuItem.Text = "Importar matriz de vizinhança";
            // 
            // grbImportacaoDosArquivos
            // 
            this.grbImportacaoDosArquivos.Controls.Add(this.button1);
            this.grbImportacaoDosArquivos.Controls.Add(this.btnImportarTabela);
            this.grbImportacaoDosArquivos.Controls.Add(this.label12);
            this.grbImportacaoDosArquivos.Controls.Add(this.cmbTabelasNoArquivo);
            this.grbImportacaoDosArquivos.Controls.Add(this.btnAbrirTabelaDados);
            this.grbImportacaoDosArquivos.Controls.Add(this.btnAbrirArquivoShape);
            this.grbImportacaoDosArquivos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbImportacaoDosArquivos.Location = new System.Drawing.Point(0, 0);
            this.grbImportacaoDosArquivos.Name = "grbImportacaoDosArquivos";
            this.grbImportacaoDosArquivos.Size = new System.Drawing.Size(969, 48);
            this.grbImportacaoDosArquivos.TabIndex = 0;
            this.grbImportacaoDosArquivos.TabStop = false;
            this.grbImportacaoDosArquivos.Enter += new System.EventHandler(this.grbImportacaoDosArquivos_Enter);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(873, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "&Concatenar";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnImportarTabela
            // 
            this.btnImportarTabela.Enabled = false;
            this.btnImportarTabela.Location = new System.Drawing.Point(622, 15);
            this.btnImportarTabela.Name = "btnImportarTabela";
            this.btnImportarTabela.Size = new System.Drawing.Size(100, 23);
            this.btnImportarTabela.TabIndex = 19;
            this.btnImportarTabela.Text = "&Importar tabela";
            this.btnImportarTabela.UseVisualStyleBackColor = true;
            this.btnImportarTabela.Click += new System.EventHandler(this.btnImportarTabela_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(197, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(93, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Tabela no arquivo";
            // 
            // cmbTabelasNoArquivo
            // 
            this.cmbTabelasNoArquivo.FormattingEnabled = true;
            this.cmbTabelasNoArquivo.Location = new System.Drawing.Point(296, 17);
            this.cmbTabelasNoArquivo.Name = "cmbTabelasNoArquivo";
            this.cmbTabelasNoArquivo.Size = new System.Drawing.Size(310, 21);
            this.cmbTabelasNoArquivo.TabIndex = 17;
            // 
            // btnAbrirTabelaDados
            // 
            this.btnAbrirTabelaDados.Location = new System.Drawing.Point(112, 15);
            this.btnAbrirTabelaDados.Name = "btnAbrirTabelaDados";
            this.btnAbrirTabelaDados.Size = new System.Drawing.Size(75, 23);
            this.btnAbrirTabelaDados.TabIndex = 16;
            this.btnAbrirTabelaDados.Text = "&Dados";
            this.btnAbrirTabelaDados.UseVisualStyleBackColor = true;
            this.btnAbrirTabelaDados.Click += new System.EventHandler(this.btnAbrirTabelaDados_Click);
            // 
            // btnAbrirArquivoShape
            // 
            this.btnAbrirArquivoShape.Location = new System.Drawing.Point(21, 15);
            this.btnAbrirArquivoShape.Name = "btnAbrirArquivoShape";
            this.btnAbrirArquivoShape.Size = new System.Drawing.Size(75, 23);
            this.btnAbrirArquivoShape.TabIndex = 13;
            this.btnAbrirArquivoShape.Text = "&Shape";
            this.btnAbrirArquivoShape.UseVisualStyleBackColor = true;
            // 
            // tabPage0
            // 
            this.tabPage0.Controls.Add(this.splitContainer2);
            this.tabPage0.ImageIndex = 2;
            this.tabPage0.Location = new System.Drawing.Point(4, 23);
            this.tabPage0.Name = "tabPage0";
            this.tabPage0.Size = new System.Drawing.Size(975, 523);
            this.tabPage0.TabIndex = 3;
            this.tabPage0.Text = "Especificações";
            this.tabPage0.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.userControlRegressaoInstrumentos1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(975, 523);
            this.splitContainer2.SplitterDistance = 638;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnExecutar);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(333, 523);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Opções";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.userControlRichTextOutput1);
            this.tabPage2.ImageIndex = 9;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(975, 523);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Resultados";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput1
            // 
            this.userControlRichTextOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput1.Location = new System.Drawing.Point(3, 3);
            this.userControlRichTextOutput1.Name = "userControlRichTextOutput1";
            this.userControlRichTextOutput1.Size = new System.Drawing.Size(969, 517);
            this.userControlRichTextOutput1.TabIndex = 0;
            this.userControlRichTextOutput1.Texto = "";
            // 
            // tabPage3
            // 
            this.tabPage3.ImageIndex = 4;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(975, 523);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Gráficos";
            this.tabPage3.UseVisualStyleBackColor = true;
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
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(880, 1);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
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
            // userControlRegressaoInstrumentos1
            // 
            this.userControlRegressaoInstrumentos1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRegressaoInstrumentos1.Location = new System.Drawing.Point(0, 0);
            this.userControlRegressaoInstrumentos1.Name = "userControlRegressaoInstrumentos1";
            this.userControlRegressaoInstrumentos1.Size = new System.Drawing.Size(638, 523);
            this.userControlRegressaoInstrumentos1.TabIndex = 0;
            this.userControlRegressaoInstrumentos1.VariaveisDB = null;
            this.userControlRegressaoInstrumentos1.VariaveisIndependentes = new string[0];
            this.userControlRegressaoInstrumentos1.VariaveisInstrumentais = new string[0];
            this.userControlRegressaoInstrumentos1.VariaveisList = null;
            this.userControlRegressaoInstrumentos1.VariavelDependente = new string[0];
            // 
            // btnExecutar
            // 
            this.btnExecutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecutar.Location = new System.Drawing.Point(234, 479);
            this.btnExecutar.Name = "btnExecutar";
            this.btnExecutar.Size = new System.Drawing.Size(75, 23);
            this.btnExecutar.TabIndex = 3;
            this.btnExecutar.Text = "&Executar";
            this.btnExecutar.UseVisualStyleBackColor = true;
            this.btnExecutar.Click += new System.EventHandler(this.btnExecutar_Click);
            // 
            // FormRegressaoLinear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 586);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(991, 620);
            this.Name = "FormRegressaoLinear";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Análise de Regressão Linear";
            this.Load += new System.EventHandler(this.FormBaseModelagem_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.grbImportacaoDosArquivos.ResumeLayout(false);
            this.grbImportacaoDosArquivos.PerformLayout();
            this.tabPage0.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private IpeaGEO.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tabPage0;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox grbImportacaoDosArquivos;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnAbrirArquivoShape;
        private System.Windows.Forms.Button btnAbrirTabelaDados;
        private System.Windows.Forms.ComboBox cmbTabelasNoArquivo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnImportarTabela;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exportarDadosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calculadoraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem excluirVariáveisToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem estatísticasDescritivasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem exportarMatrizDeVizinhançaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importarMatrizDeVizinhançaToolStripMenuItem;
        private UserControlRegressaoInstrumentos userControlRegressaoInstrumentos1;
        private System.Windows.Forms.Button btnExecutar;
    }
}