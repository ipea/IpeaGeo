namespace IpeaGeo
{
    partial class FormGeracaoDummies
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGeracaoDummies));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.userControlSelecaoVariaveis1 = new IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis();
            this.btnGerar = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnGerarIntervalos = new System.Windows.Forms.Button();
            this.nudNumIntervalos = new System.Windows.Forms.NumericUpDown();
            this.rdbNumIntervalos = new System.Windows.Forms.RadioButton();
            this.rdbDecis = new System.Windows.Forms.RadioButton();
            this.rdbQuartil = new System.Windows.Forms.RadioButton();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.NumIntervalo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LimSuperior = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NumObservacoes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnGerarDummiesQuant = new System.Windows.Forms.Button();
            this.btnAtualizarIntervalos = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumIntervalos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
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
            this.splitContainer1.Panel2.Controls.Add(this.btnCancelar);
            this.splitContainer1.Size = new System.Drawing.Size(876, 543);
            this.splitContainer1.SplitterDistance = 498;
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
            this.tabControl1.Size = new System.Drawing.Size(876, 498);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer2);
            this.tabPage1.ImageIndex = 2;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(868, 471);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Variáveis Categóricas";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.userControlSelecaoVariaveis1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnGerar);
            this.splitContainer2.Size = new System.Drawing.Size(862, 465);
            this.splitContainer2.SplitterDistance = 383;
            this.splitContainer2.TabIndex = 0;
            // 
            // userControlSelecaoVariaveis1
            // 
            this.userControlSelecaoVariaveis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSelecaoVariaveis1.Location = new System.Drawing.Point(0, 0);
            this.userControlSelecaoVariaveis1.Name = "userControlSelecaoVariaveis1";
            this.userControlSelecaoVariaveis1.Size = new System.Drawing.Size(862, 383);
            this.userControlSelecaoVariaveis1.TabIndex = 0;
            this.userControlSelecaoVariaveis1.VariaveisDB = null;
            this.userControlSelecaoVariaveis1.VariaveisIndependentes = new string[0];
            this.userControlSelecaoVariaveis1.VariaveisList = null;
            // 
            // btnGerar
            // 
            this.btnGerar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGerar.Location = new System.Drawing.Point(725, 36);
            this.btnGerar.Name = "btnGerar";
            this.btnGerar.Size = new System.Drawing.Size(114, 23);
            this.btnGerar.TabIndex = 2;
            this.btnGerar.Text = "&Gerar Dummies";
            this.btnGerar.UseVisualStyleBackColor = true;
            this.btnGerar.Click += new System.EventHandler(this.btnGerar_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(868, 471);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Visualização dos Dados";
            this.tabPage2.UseVisualStyleBackColor = true;
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
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
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
            this.dataGridView1.Size = new System.Drawing.Size(862, 465);
            this.dataGridView1.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer3);
            this.tabPage3.ImageIndex = 4;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(868, 471);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Variáveis Quantitativas";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer3.Size = new System.Drawing.Size(868, 471);
            this.splitContainer3.SplitterDistance = 355;
            this.splitContainer3.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer4.Size = new System.Drawing.Size(355, 471);
            this.splitContainer4.SplitterDistance = 288;
            this.splitContainer4.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(355, 288);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Seleção da variável quantitativa";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 16);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(349, 269);
            this.listBox1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnGerarIntervalos);
            this.groupBox2.Controls.Add(this.nudNumIntervalos);
            this.groupBox2.Controls.Add(this.rdbNumIntervalos);
            this.groupBox2.Controls.Add(this.rdbDecis);
            this.groupBox2.Controls.Add(this.rdbQuartil);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(355, 179);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Intervalos iniciais";
            // 
            // btnGerarIntervalos
            // 
            this.btnGerarIntervalos.Location = new System.Drawing.Point(209, 127);
            this.btnGerarIntervalos.Name = "btnGerarIntervalos";
            this.btnGerarIntervalos.Size = new System.Drawing.Size(114, 23);
            this.btnGerarIntervalos.TabIndex = 4;
            this.btnGerarIntervalos.Text = "Gerar &Intervalos";
            this.btnGerarIntervalos.UseVisualStyleBackColor = true;
            this.btnGerarIntervalos.Click += new System.EventHandler(this.btnGerarIntervalos_Click);
            // 
            // nudNumIntervalos
            // 
            this.nudNumIntervalos.Enabled = false;
            this.nudNumIntervalos.Location = new System.Drawing.Point(209, 81);
            this.nudNumIntervalos.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudNumIntervalos.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudNumIntervalos.Name = "nudNumIntervalos";
            this.nudNumIntervalos.Size = new System.Drawing.Size(114, 20);
            this.nudNumIntervalos.TabIndex = 3;
            this.nudNumIntervalos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudNumIntervalos.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // rdbNumIntervalos
            // 
            this.rdbNumIntervalos.AutoSize = true;
            this.rdbNumIntervalos.Location = new System.Drawing.Point(30, 81);
            this.rdbNumIntervalos.Name = "rdbNumIntervalos";
            this.rdbNumIntervalos.Size = new System.Drawing.Size(173, 17);
            this.rdbNumIntervalos.TabIndex = 2;
            this.rdbNumIntervalos.Text = "Escolha o número de intervalos";
            this.rdbNumIntervalos.UseVisualStyleBackColor = true;
            this.rdbNumIntervalos.CheckedChanged += new System.EventHandler(this.rdbNumIntervalos_CheckedChanged);
            // 
            // rdbDecis
            // 
            this.rdbDecis.AutoSize = true;
            this.rdbDecis.Location = new System.Drawing.Point(30, 58);
            this.rdbDecis.Name = "rdbDecis";
            this.rdbDecis.Size = new System.Drawing.Size(52, 17);
            this.rdbDecis.TabIndex = 1;
            this.rdbDecis.Text = "Decis";
            this.rdbDecis.UseVisualStyleBackColor = true;
            // 
            // rdbQuartil
            // 
            this.rdbQuartil.AutoSize = true;
            this.rdbQuartil.Checked = true;
            this.rdbQuartil.Location = new System.Drawing.Point(30, 35);
            this.rdbQuartil.Name = "rdbQuartil";
            this.rdbQuartil.Size = new System.Drawing.Size(58, 17);
            this.rdbQuartil.TabIndex = 0;
            this.rdbQuartil.TabStop = true;
            this.rdbQuartil.Text = "Quartis";
            this.rdbQuartil.UseVisualStyleBackColor = true;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.IsSplitterFixed = true;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.btnGerarDummiesQuant);
            this.splitContainer5.Panel2.Controls.Add(this.btnAtualizarIntervalos);
            this.splitContainer5.Size = new System.Drawing.Size(509, 471);
            this.splitContainer5.SplitterDistance = 292;
            this.splitContainer5.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridView2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(509, 292);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Ajuste dos intervalos";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NumIntervalo,
            this.LimSuperior,
            this.NumObservacoes});
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 16);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(503, 273);
            this.dataGridView2.TabIndex = 0;
            // 
            // NumIntervalo
            // 
            this.NumIntervalo.HeaderText = "Número do intervalo";
            this.NumIntervalo.MinimumWidth = 120;
            this.NumIntervalo.Name = "NumIntervalo";
            this.NumIntervalo.ReadOnly = true;
            this.NumIntervalo.Width = 120;
            // 
            // LimSuperior
            // 
            this.LimSuperior.HeaderText = "Limite superior do intervalo";
            this.LimSuperior.MinimumWidth = 80;
            this.LimSuperior.Name = "LimSuperior";
            this.LimSuperior.Width = 200;
            // 
            // NumObservacoes
            // 
            this.NumObservacoes.HeaderText = "Número de observações";
            this.NumObservacoes.MinimumWidth = 140;
            this.NumObservacoes.Name = "NumObservacoes";
            this.NumObservacoes.ReadOnly = true;
            this.NumObservacoes.Width = 140;
            // 
            // btnGerarDummiesQuant
            // 
            this.btnGerarDummiesQuant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGerarDummiesQuant.Enabled = false;
            this.btnGerarDummiesQuant.Location = new System.Drawing.Point(369, 123);
            this.btnGerarDummiesQuant.Name = "btnGerarDummiesQuant";
            this.btnGerarDummiesQuant.Size = new System.Drawing.Size(114, 23);
            this.btnGerarDummiesQuant.TabIndex = 6;
            this.btnGerarDummiesQuant.Text = "Gerar &Dummies";
            this.btnGerarDummiesQuant.UseVisualStyleBackColor = true;
            this.btnGerarDummiesQuant.Click += new System.EventHandler(this.btnGerarDummiesQuant_Click);
            // 
            // btnAtualizarIntervalos
            // 
            this.btnAtualizarIntervalos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizarIntervalos.Enabled = false;
            this.btnAtualizarIntervalos.Location = new System.Drawing.Point(369, 31);
            this.btnAtualizarIntervalos.Name = "btnAtualizarIntervalos";
            this.btnAtualizarIntervalos.Size = new System.Drawing.Size(114, 23);
            this.btnAtualizarIntervalos.TabIndex = 5;
            this.btnAtualizarIntervalos.Text = "&Atualizar Intervalos";
            this.btnAtualizarIntervalos.UseVisualStyleBackColor = true;
            this.btnAtualizarIntervalos.Click += new System.EventHandler(this.btnAtualizarIntervalos_Click);
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
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(732, 7);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(114, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Atualizar &Tabela";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(34, 7);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(114, 23);
            this.btnCancelar.TabIndex = 0;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Número do intervalo";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 120;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Limite superior do intervalo";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 80;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 200;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Número de observações";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 140;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 140;
            // 
            // FormGeracaoDummies
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 543);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(884, 570);
            this.Name = "FormGeracaoDummies";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Geração de Variáveis Dummy";
            this.Load += new System.EventHandler(this.FormGeracaoDummies_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumIntervalos)).EndInit();
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis userControlSelecaoVariaveis1;
        private System.Windows.Forms.Button btnGerar;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nudNumIntervalos;
        private System.Windows.Forms.RadioButton rdbNumIntervalos;
        private System.Windows.Forms.RadioButton rdbDecis;
        private System.Windows.Forms.RadioButton rdbQuartil;
        private System.Windows.Forms.Button btnGerarIntervalos;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button btnAtualizarIntervalos;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumIntervalo;
        private System.Windows.Forms.DataGridViewTextBoxColumn LimSuperior;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumObservacoes;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnGerarDummiesQuant;
    }
}