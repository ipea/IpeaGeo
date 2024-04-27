namespace IpeaGeo
{
    partial class frmMapaTematico
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMapaTematico));
            this.btnExecutar = new System.Windows.Forms.Button();
            this.btnCancela = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnEditarCortes = new System.Windows.Forms.Button();
            this.chkAleatorio = new System.Windows.Forms.CheckBox();
            this.chkGuarda = new System.Windows.Forms.CheckBox();
            this.chkRelatorio = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbCores = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbMetodo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numClasses = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbVariavel = new System.Windows.Forms.ComboBox();
            this.cmbDesvio = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ckbInverterEscalaCores = new System.Windows.Forms.CheckBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClasses)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExecutar
            // 
            this.btnExecutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecutar.Location = new System.Drawing.Point(543, 186);
            this.btnExecutar.Name = "btnExecutar";
            this.btnExecutar.Size = new System.Drawing.Size(75, 23);
            this.btnExecutar.TabIndex = 0;
            this.btnExecutar.Text = "Executar";
            this.btnExecutar.UseVisualStyleBackColor = true;
            this.btnExecutar.Click += new System.EventHandler(this.btnExecutar_Click);
            // 
            // btnCancela
            // 
            this.btnCancela.Location = new System.Drawing.Point(41, 2);
            this.btnCancela.Name = "btnCancela";
            this.btnCancela.Size = new System.Drawing.Size(75, 23);
            this.btnCancela.TabIndex = 1;
            this.btnCancela.Text = "Cancelar";
            this.btnCancela.UseVisualStyleBackColor = true;
            this.btnCancela.Click += new System.EventHandler(this.btnCancela_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnEditarCortes);
            this.groupBox1.Controls.Add(this.btnExecutar);
            this.groupBox1.Controls.Add(this.chkAleatorio);
            this.groupBox1.Controls.Add(this.chkGuarda);
            this.groupBox1.Controls.Add(this.chkRelatorio);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmbCores);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbMetodo);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numClasses);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbVariavel);
            this.groupBox1.Controls.Add(this.cmbDesvio);
            this.groupBox1.Location = new System.Drawing.Point(7, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(645, 303);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parâmetros";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(222, 256);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(310, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Apresentar e editar os cortes, as cores e as legendas";
            // 
            // btnEditarCortes
            // 
            this.btnEditarCortes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditarCortes.Location = new System.Drawing.Point(543, 251);
            this.btnEditarCortes.Name = "btnEditarCortes";
            this.btnEditarCortes.Size = new System.Drawing.Size(75, 23);
            this.btnEditarCortes.TabIndex = 12;
            this.btnEditarCortes.Text = "&Editar";
            this.btnEditarCortes.UseVisualStyleBackColor = true;
            this.btnEditarCortes.Click += new System.EventHandler(this.btnEditarCortes_Click);
            // 
            // chkAleatorio
            // 
            this.chkAleatorio.AutoSize = true;
            this.chkAleatorio.Location = new System.Drawing.Point(391, 120);
            this.chkAleatorio.Name = "chkAleatorio";
            this.chkAleatorio.Size = new System.Drawing.Size(101, 17);
            this.chkAleatorio.TabIndex = 10;
            this.chkAleatorio.Text = "Cores aleatórias";
            this.chkAleatorio.UseVisualStyleBackColor = true;
            this.chkAleatorio.CheckedChanged += new System.EventHandler(this.chkAleatorio_CheckedChanged);
            // 
            // chkGuarda
            // 
            this.chkGuarda.AutoSize = true;
            this.chkGuarda.Checked = true;
            this.chkGuarda.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGuarda.Location = new System.Drawing.Point(239, 120);
            this.chkGuarda.Name = "chkGuarda";
            this.chkGuarda.Size = new System.Drawing.Size(128, 17);
            this.chkGuarda.TabIndex = 8;
            this.chkGuarda.Text = "Guardar classificação";
            this.chkGuarda.UseVisualStyleBackColor = true;
            // 
            // chkRelatorio
            // 
            this.chkRelatorio.AutoSize = true;
            this.chkRelatorio.Location = new System.Drawing.Point(517, 120);
            this.chkRelatorio.Name = "chkRelatorio";
            this.chkRelatorio.Size = new System.Drawing.Size(92, 17);
            this.chkRelatorio.TabIndex = 3;
            this.chkRelatorio.Text = "Gerar relatório";
            this.chkRelatorio.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Cores";
            // 
            // cmbCores
            // 
            this.cmbCores.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbCores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCores.FormattingEnabled = true;
            this.cmbCores.Location = new System.Drawing.Point(88, 87);
            this.cmbCores.Name = "cmbCores";
            this.cmbCores.Size = new System.Drawing.Size(530, 21);
            this.cmbCores.TabIndex = 6;
            this.cmbCores.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbCores_DrawItem);
            this.cmbCores.SelectedIndexChanged += new System.EventHandler(this.cmbCores_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Método";
            // 
            // cmbMetodo
            // 
            this.cmbMetodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMetodo.FormattingEnabled = true;
            this.cmbMetodo.Location = new System.Drawing.Point(88, 57);
            this.cmbMetodo.Name = "cmbMetodo";
            this.cmbMetodo.Size = new System.Drawing.Size(530, 21);
            this.cmbMetodo.TabIndex = 4;
            this.cmbMetodo.SelectedIndexChanged += new System.EventHandler(this.cmbMetodo_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Classes";
            // 
            // numClasses
            // 
            this.numClasses.BackColor = System.Drawing.Color.White;
            this.numClasses.Location = new System.Drawing.Point(88, 117);
            this.numClasses.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numClasses.Name = "numClasses";
            this.numClasses.ReadOnly = true;
            this.numClasses.Size = new System.Drawing.Size(108, 20);
            this.numClasses.TabIndex = 2;
            this.numClasses.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numClasses.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numClasses.ValueChanged += new System.EventHandler(this.numClasses_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Variável";
            // 
            // cmbVariavel
            // 
            this.cmbVariavel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVariavel.FormattingEnabled = true;
            this.cmbVariavel.Location = new System.Drawing.Point(88, 30);
            this.cmbVariavel.Name = "cmbVariavel";
            this.cmbVariavel.Size = new System.Drawing.Size(530, 21);
            this.cmbVariavel.TabIndex = 0;
            this.cmbVariavel.SelectedIndexChanged += new System.EventHandler(this.cmbVariavel_SelectedIndexChanged);
            // 
            // cmbDesvio
            // 
            this.cmbDesvio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDesvio.FormattingEnabled = true;
            this.cmbDesvio.Items.AddRange(new object[] {
            "1 desvio padrão",
            "0,8 desvio padrão",
            "0,6 desvio padrão",
            "0,5 desvio padrão",
            "0,4 desvio padrão",
            "0,2 desvio padrão",
            "0,1 desvio padrão"});
            this.cmbDesvio.Location = new System.Drawing.Point(88, 116);
            this.cmbDesvio.Name = "cmbDesvio";
            this.cmbDesvio.Size = new System.Drawing.Size(108, 21);
            this.cmbDesvio.TabIndex = 9;
            this.cmbDesvio.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(668, 346);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.Click += new System.EventHandler(this.tabControl1_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(660, 319);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Principal";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.ImageIndex = 2;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(660, 319);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Definição Manual";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Enter += new System.EventHandler(this.tabPage2_Enter);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ckbInverterEscalaCores);
            this.splitContainer2.Size = new System.Drawing.Size(654, 313);
            this.splitContainer2.SplitterDistance = 284;
            this.splitContainer2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 4;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(654, 284);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // ckbInverterEscalaCores
            // 
            this.ckbInverterEscalaCores.AutoSize = true;
            this.ckbInverterEscalaCores.Location = new System.Drawing.Point(28, 3);
            this.ckbInverterEscalaCores.Name = "ckbInverterEscalaCores";
            this.ckbInverterEscalaCores.Size = new System.Drawing.Size(140, 17);
            this.ckbInverterEscalaCores.TabIndex = 4;
            this.ckbInverterEscalaCores.Text = "Inverter escala de cores";
            this.ckbInverterEscalaCores.UseVisualStyleBackColor = true;
            this.ckbInverterEscalaCores.CheckedChanged += new System.EventHandler(this.ckbInverterEscalaCores_CheckedChanged);
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
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Panel2.Controls.Add(this.btnCancela);
            this.splitContainer1.Size = new System.Drawing.Size(668, 382);
            this.splitContainer1.SplitterDistance = 346;
            this.splitContainer1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(554, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmMapaTematico
            // 
            this.AcceptButton = this.btnExecutar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 382);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMapaTematico";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mapas Temáticos";
            this.Load += new System.EventHandler(this.frmMapaTematico_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClasses)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExecutar;
        private System.Windows.Forms.Button btnCancela;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numClasses;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cmbVariavel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbMetodo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbCores;
        private System.Windows.Forms.CheckBox chkRelatorio;
        private System.Windows.Forms.CheckBox chkGuarda;
        private System.Windows.Forms.ComboBox cmbDesvio;
        private System.Windows.Forms.CheckBox chkAleatorio;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.CheckBox ckbInverterEscalaCores;
        private System.Windows.Forms.Button btnEditarCortes;
        private System.Windows.Forms.Label label5;
    }
}