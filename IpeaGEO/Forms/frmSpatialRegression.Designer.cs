namespace IpeaGeo
{
    partial class frmSpatialRegression
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblMetodo = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnRegressao = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.comboBoxDependente = new System.Windows.Forms.ComboBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbInstrumentos = new System.Windows.Forms.CheckedListBox();
            this.lblX = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ckbX = new System.Windows.Forms.CheckedListBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.nudCutoffLatitude = new System.Windows.Forms.NumericUpDown();
            this.nudCutoffLongitude = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbxIntercepto = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbVizinhanca = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cmbCores = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Cores = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.chkRelatorio = new System.Windows.Forms.CheckBox();
            this.chkAleatorio = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCutoffLatitude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCutoffLongitude)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(847, 503);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseClick);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.comboBox3);
            this.tabPage1.Controls.Add(this.btnOK);
            this.tabPage1.Controls.Add(this.lblMetodo);
            this.tabPage1.Controls.Add(this.comboBox2);
            this.tabPage1.Controls.Add(this.btnCancelar);
            this.tabPage1.Controls.Add(this.btnRegressao);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.lblY);
            this.tabPage1.Controls.Add(this.comboBoxDependente);
            this.tabPage1.Controls.Add(this.progressBar1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(839, 477);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Regressão";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(558, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Variável";
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.Enabled = false;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(561, 227);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(215, 21);
            this.comboBox3.TabIndex = 27;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(664, 411);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(112, 23);
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblMetodo
            // 
            this.lblMetodo.AutoSize = true;
            this.lblMetodo.Location = new System.Drawing.Point(558, 34);
            this.lblMetodo.Name = "lblMetodo";
            this.lblMetodo.Size = new System.Drawing.Size(43, 13);
            this.lblMetodo.TabIndex = 10;
            this.lblMetodo.Text = "Método";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(561, 50);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(215, 21);
            this.comboBox2.TabIndex = 9;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(514, 411);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(112, 23);
            this.btnCancelar.TabIndex = 7;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnRegressao
            // 
            this.btnRegressao.Location = new System.Drawing.Point(514, 382);
            this.btnRegressao.Name = "btnRegressao";
            this.btnRegressao.Size = new System.Drawing.Size(262, 23);
            this.btnRegressao.TabIndex = 6;
            this.btnRegressao.Text = "Regressão";
            this.btnRegressao.UseVisualStyleBackColor = true;
            this.btnRegressao.Click += new System.EventHandler(this.btnRegressao_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(363, 451);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(40, 34);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(107, 13);
            this.lblY.TabIndex = 2;
            this.lblY.Text = "Variável Dependente";
            // 
            // comboBoxDependente
            // 
            this.comboBoxDependente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDependente.FormattingEnabled = true;
            this.comboBoxDependente.Location = new System.Drawing.Point(42, 50);
            this.comboBoxDependente.MaxLength = 1000000;
            this.comboBoxDependente.Name = "comboBoxDependente";
            this.comboBoxDependente.Size = new System.Drawing.Size(201, 21);
            this.comboBoxDependente.TabIndex = 1;
            this.comboBoxDependente.SelectedIndexChanged += new System.EventHandler(this.comboBoxDependente_SelectedIndexChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 446);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(342, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckbInstrumentos);
            this.groupBox1.Controls.Add(this.lblX);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ckbX);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Location = new System.Drawing.Point(16, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(475, 427);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Variáveis";
            // 
            // ckbInstrumentos
            // 
            this.ckbInstrumentos.CheckOnClick = true;
            this.ckbInstrumentos.FormattingEnabled = true;
            this.ckbInstrumentos.HorizontalScrollbar = true;
            this.ckbInstrumentos.Location = new System.Drawing.Point(244, 83);
            this.ckbInstrumentos.Name = "ckbInstrumentos";
            this.ckbInstrumentos.Size = new System.Drawing.Size(192, 199);
            this.ckbInstrumentos.TabIndex = 29;
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(24, 67);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(66, 13);
            this.lblX.TabIndex = 5;
            this.lblX.Text = "Regressores";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(241, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Instrumentos";
            // 
            // ckbX
            // 
            this.ckbX.CheckOnClick = true;
            this.ckbX.FormattingEnabled = true;
            this.ckbX.HorizontalScrollbar = true;
            this.ckbX.Location = new System.Drawing.Point(27, 83);
            this.ckbX.Name = "ckbX";
            this.ckbX.Size = new System.Drawing.Size(200, 199);
            this.ckbX.TabIndex = 4;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.nudCutoffLatitude);
            this.groupBox5.Controls.Add(this.nudCutoffLongitude);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.comboBox4);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.comboBox5);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Enabled = false;
            this.groupBox5.Location = new System.Drawing.Point(12, 294);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(448, 127);
            this.groupBox5.TabIndex = 35;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Opções GMM espacial (Conley)";
            // 
            // nudCutoffLatitude
            // 
            this.nudCutoffLatitude.Location = new System.Drawing.Point(23, 91);
            this.nudCutoffLatitude.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudCutoffLatitude.Name = "nudCutoffLatitude";
            this.nudCutoffLatitude.Size = new System.Drawing.Size(143, 20);
            this.nudCutoffLatitude.TabIndex = 41;
            this.nudCutoffLatitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudCutoffLongitude
            // 
            this.nudCutoffLongitude.Location = new System.Drawing.Point(232, 91);
            this.nudCutoffLongitude.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudCutoffLongitude.Name = "nudCutoffLongitude";
            this.nudCutoffLongitude.Size = new System.Drawing.Size(143, 20);
            this.nudCutoffLongitude.TabIndex = 40;
            this.nudCutoffLongitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(229, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "Cut-off longitude";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 75);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 38;
            this.label9.Text = "Cut-off latitude";
            // 
            // comboBox4
            // 
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(232, 41);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(143, 21);
            this.comboBox4.TabIndex = 32;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(229, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Longitude";
            // 
            // comboBox5
            // 
            this.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point(23, 41);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(143, 21);
            this.comboBox5.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "Latitude";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbxIntercepto);
            this.groupBox2.Location = new System.Drawing.Point(514, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(286, 88);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Modelo";
            // 
            // cbxIntercepto
            // 
            this.cbxIntercepto.AutoSize = true;
            this.cbxIntercepto.Location = new System.Drawing.Point(47, 64);
            this.cbxIntercepto.Name = "cbxIntercepto";
            this.cbxIntercepto.Size = new System.Drawing.Size(214, 17);
            this.cbxIntercepto.TabIndex = 12;
            this.cbxIntercepto.Text = "Regressão pela Origem (sem intercepto)";
            this.cbxIntercepto.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmbVizinhanca);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(514, 107);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(286, 76);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Vizinhança";
            // 
            // cmbVizinhanca
            // 
            this.cmbVizinhanca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVizinhanca.Enabled = false;
            this.cmbVizinhanca.FormattingEnabled = true;
            this.cmbVizinhanca.Items.AddRange(new object[] {
            "Queen",
            "Rook"});
            this.cmbVizinhanca.Location = new System.Drawing.Point(47, 42);
            this.cmbVizinhanca.Name = "cmbVizinhanca";
            this.cmbVizinhanca.Size = new System.Drawing.Size(215, 21);
            this.cmbVizinhanca.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Tipo";
            this.label4.Visible = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cmbCores);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.Cores);
            this.groupBox4.Controls.Add(this.comboBox1);
            this.groupBox4.Controls.Add(this.chkRelatorio);
            this.groupBox4.Controls.Add(this.chkAleatorio);
            this.groupBox4.Enabled = false;
            this.groupBox4.Location = new System.Drawing.Point(514, 189);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(286, 187);
            this.groupBox4.TabIndex = 31;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Mapa";
            // 
            // cmbCores
            // 
            this.cmbCores.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbCores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCores.FormattingEnabled = true;
            this.cmbCores.Location = new System.Drawing.Point(46, 132);
            this.cmbCores.Name = "cmbCores";
            this.cmbCores.Size = new System.Drawing.Size(215, 21);
            this.cmbCores.TabIndex = 32;
            this.cmbCores.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbCores_DrawItem_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "Método";
            // 
            // Cores
            // 
            this.Cores.AutoSize = true;
            this.Cores.Location = new System.Drawing.Point(44, 118);
            this.Cores.Name = "Cores";
            this.Cores.Size = new System.Drawing.Size(34, 13);
            this.Cores.TabIndex = 20;
            this.Cores.Text = "Cores";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(47, 84);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(215, 21);
            this.comboBox1.TabIndex = 32;
            // 
            // chkRelatorio
            // 
            this.chkRelatorio.AutoSize = true;
            this.chkRelatorio.Checked = true;
            this.chkRelatorio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRelatorio.Location = new System.Drawing.Point(155, 161);
            this.chkRelatorio.Name = "chkRelatorio";
            this.chkRelatorio.Size = new System.Drawing.Size(89, 17);
            this.chkRelatorio.TabIndex = 24;
            this.chkRelatorio.Text = "Gera relatório";
            this.chkRelatorio.UseVisualStyleBackColor = true;
            // 
            // chkAleatorio
            // 
            this.chkAleatorio.AutoSize = true;
            this.chkAleatorio.Location = new System.Drawing.Point(47, 161);
            this.chkAleatorio.Name = "chkAleatorio";
            this.chkAleatorio.Size = new System.Drawing.Size(102, 17);
            this.chkAleatorio.TabIndex = 23;
            this.chkAleatorio.Text = "Cores Aleatórias";
            this.chkAleatorio.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(839, 477);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Resíduos";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(3, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(814, 420);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.zedGraphControl1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(839, 477);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Plots";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.tabPage3.Click += new System.EventHandler(this.tabPage3_Click);
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl1.Location = new System.Drawing.Point(3, 3);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0;
            this.zedGraphControl1.ScrollMaxX = 0;
            this.zedGraphControl1.ScrollMaxY = 0;
            this.zedGraphControl1.ScrollMaxY2 = 0;
            this.zedGraphControl1.ScrollMinX = 0;
            this.zedGraphControl1.ScrollMinY = 0;
            this.zedGraphControl1.ScrollMinY2 = 0;
            this.zedGraphControl1.Size = new System.Drawing.Size(833, 471);
            this.zedGraphControl1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dataGridView2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(839, 477);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Resultados";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedHeaders;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(3, 14);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.Size = new System.Drawing.Size(807, 414);
            this.dataGridView2.TabIndex = 1;
            // 
            // frmSpatialRegression
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(847, 503);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSpatialRegression";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Regressão Espacial";
            this.Load += new System.EventHandler(this.frmSpatialRegression_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCutoffLatitude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCutoffLongitude)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.ComboBox comboBoxDependente;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.CheckedListBox ckbX;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnRegressao;
        private System.Windows.Forms.Label lblMetodo;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbxIntercepto;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label Cores;
        private System.Windows.Forms.CheckBox chkAleatorio;
        private System.Windows.Forms.CheckBox chkRelatorio;
        public System.Windows.Forms.ComboBox cmbVizinhanca;
        public System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox ckbInstrumentos;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cmbCores;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.NumericUpDown nudCutoffLongitude;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudCutoffLatitude;
    }
}