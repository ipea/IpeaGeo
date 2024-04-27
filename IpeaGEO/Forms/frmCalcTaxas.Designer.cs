namespace IpeaGEO.Forms
{
    partial class frmCalcTaxas
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.grbImportacaoDosArquivos = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnAbrirTabelaDados = new System.Windows.Forms.Button();
            this.btnAbrirArquivoShape = new System.Windows.Forms.Button();
            this.tabPage0 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput1 = new IpeaGEO.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.userControlRichTextOutput2 = new IpeaGEO.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBase = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbEvento = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnExecutar = new System.Windows.Forms.Button();
            this.ckbDefinicaoManual = new System.Windows.Forms.CheckBox();
            this.chkAleatorio = new System.Windows.Forms.CheckBox();
            this.chkGuarda = new System.Windows.Forms.CheckBox();
            this.chkRelatorio = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbCores = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbMetodo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numClasses = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbVariavel = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.grbImportacaoDosArquivos.SuspendLayout();
            this.tabPage0.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClasses)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage0);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(728, 495);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer3);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(720, 469);
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
            this.splitContainer3.Size = new System.Drawing.Size(714, 463);
            this.splitContainer3.SplitterDistance = 411;
            this.splitContainer3.TabIndex = 0;
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridView1.Size = new System.Drawing.Size(714, 411);
            this.dataGridView1.TabIndex = 1;
            // 
            // grbImportacaoDosArquivos
            // 
            this.grbImportacaoDosArquivos.Controls.Add(this.button1);
            this.grbImportacaoDosArquivos.Controls.Add(this.btnAbrirTabelaDados);
            this.grbImportacaoDosArquivos.Controls.Add(this.btnAbrirArquivoShape);
            this.grbImportacaoDosArquivos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbImportacaoDosArquivos.Location = new System.Drawing.Point(0, 0);
            this.grbImportacaoDosArquivos.Name = "grbImportacaoDosArquivos";
            this.grbImportacaoDosArquivos.Size = new System.Drawing.Size(714, 48);
            this.grbImportacaoDosArquivos.TabIndex = 0;
            this.grbImportacaoDosArquivos.TabStop = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(618, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "&Concatenar";
            this.button1.UseVisualStyleBackColor = true;
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
            this.tabPage0.Controls.Add(this.groupBox2);
            this.tabPage0.Controls.Add(this.groupBox1);
            this.tabPage0.ImageIndex = 2;
            this.tabPage0.Location = new System.Drawing.Point(4, 22);
            this.tabPage0.Name = "tabPage0";
            this.tabPage0.Size = new System.Drawing.Size(720, 469);
            this.tabPage0.TabIndex = 3;
            this.tabPage0.Text = "Especificações";
            this.tabPage0.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.userControlRichTextOutput1);
            this.tabPage2.ImageIndex = 9;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(720, 469);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Resultados";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.ImageIndex = 4;
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(720, 469);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Gráficos";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.userControlRichTextOutput2);
            this.tabPage4.ImageIndex = 7;
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(720, 469);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "Variáveis Geradas";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput1
            // 
            this.userControlRichTextOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput1.Location = new System.Drawing.Point(3, 3);
            this.userControlRichTextOutput1.Name = "userControlRichTextOutput1";
            this.userControlRichTextOutput1.Size = new System.Drawing.Size(714, 463);
            this.userControlRichTextOutput1.TabIndex = 0;
            this.userControlRichTextOutput1.Texto = "";
            // 
            // userControlRichTextOutput2
            // 
            this.userControlRichTextOutput2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput2.Location = new System.Drawing.Point(0, 0);
            this.userControlRichTextOutput2.Name = "userControlRichTextOutput2";
            this.userControlRichTextOutput2.Size = new System.Drawing.Size(720, 469);
            this.userControlRichTextOutput2.TabIndex = 1;
            this.userControlRichTextOutput2.Texto = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbBase);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbEvento);
            this.groupBox1.Location = new System.Drawing.Point(8, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(704, 75);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Variáveis";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Base";
            // 
            // cmbBase
            // 
            this.cmbBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBase.FormattingEnabled = true;
            this.cmbBase.Location = new System.Drawing.Point(74, 44);
            this.cmbBase.Name = "cmbBase";
            this.cmbBase.Size = new System.Drawing.Size(332, 21);
            this.cmbBase.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Evento";
            // 
            // cmbEvento
            // 
            this.cmbEvento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEvento.FormattingEnabled = true;
            this.cmbEvento.Location = new System.Drawing.Point(74, 17);
            this.cmbEvento.Name = "cmbEvento";
            this.cmbEvento.Size = new System.Drawing.Size(332, 21);
            this.cmbEvento.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnExecutar);
            this.groupBox2.Controls.Add(this.ckbDefinicaoManual);
            this.groupBox2.Controls.Add(this.chkAleatorio);
            this.groupBox2.Controls.Add(this.chkGuarda);
            this.groupBox2.Controls.Add(this.chkRelatorio);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cmbCores);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cmbMetodo);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numClasses);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbVariavel);
            this.groupBox2.Location = new System.Drawing.Point(8, 95);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(704, 219);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parâmetros";
            // 
            // btnExecutar
            // 
            this.btnExecutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecutar.Location = new System.Drawing.Point(614, 156);
            this.btnExecutar.Name = "btnExecutar";
            this.btnExecutar.Size = new System.Drawing.Size(75, 23);
            this.btnExecutar.TabIndex = 0;
            this.btnExecutar.Text = "Executar";
            this.btnExecutar.UseVisualStyleBackColor = true;
            // 
            // ckbDefinicaoManual
            // 
            this.ckbDefinicaoManual.AutoSize = true;
            this.ckbDefinicaoManual.Location = new System.Drawing.Point(73, 160);
            this.ckbDefinicaoManual.Name = "ckbDefinicaoManual";
            this.ckbDefinicaoManual.Size = new System.Drawing.Size(193, 17);
            this.ckbDefinicaoManual.TabIndex = 11;
            this.ckbDefinicaoManual.Text = "Define cortes e cores manualmente";
            this.ckbDefinicaoManual.UseVisualStyleBackColor = true;
            // 
            // chkAleatorio
            // 
            this.chkAleatorio.AutoSize = true;
            this.chkAleatorio.Location = new System.Drawing.Point(384, 127);
            this.chkAleatorio.Name = "chkAleatorio";
            this.chkAleatorio.Size = new System.Drawing.Size(101, 17);
            this.chkAleatorio.TabIndex = 10;
            this.chkAleatorio.Text = "Cores aleatórias";
            this.chkAleatorio.UseVisualStyleBackColor = true;
            // 
            // chkGuarda
            // 
            this.chkGuarda.AutoSize = true;
            this.chkGuarda.Checked = true;
            this.chkGuarda.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGuarda.Location = new System.Drawing.Point(190, 127);
            this.chkGuarda.Name = "chkGuarda";
            this.chkGuarda.Size = new System.Drawing.Size(128, 17);
            this.chkGuarda.TabIndex = 8;
            this.chkGuarda.Text = "Guardar classificação";
            this.chkGuarda.UseVisualStyleBackColor = true;
            // 
            // chkRelatorio
            // 
            this.chkRelatorio.AutoSize = true;
            this.chkRelatorio.Checked = true;
            this.chkRelatorio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRelatorio.Location = new System.Drawing.Point(73, 127);
            this.chkRelatorio.Name = "chkRelatorio";
            this.chkRelatorio.Size = new System.Drawing.Size(92, 17);
            this.chkRelatorio.TabIndex = 3;
            this.chkRelatorio.Text = "Gerar relatório";
            this.chkRelatorio.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 103);
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
            this.cmbCores.Location = new System.Drawing.Point(73, 100);
            this.cmbCores.Name = "cmbCores";
            this.cmbCores.Size = new System.Drawing.Size(333, 21);
            this.cmbCores.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Método";
            // 
            // cmbMetodo
            // 
            this.cmbMetodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMetodo.FormattingEnabled = true;
            this.cmbMetodo.Items.AddRange(new object[] {
            "Quantil",
            "Equal",
            "Jenks (Natural Breaks)",
            "Geométrico",
            "Valores Únicos"});
            this.cmbMetodo.Location = new System.Drawing.Point(73, 45);
            this.cmbMetodo.Name = "cmbMetodo";
            this.cmbMetodo.Size = new System.Drawing.Size(333, 21);
            this.cmbMetodo.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Classes";
            // 
            // numClasses
            // 
            this.numClasses.BackColor = System.Drawing.Color.White;
            this.numClasses.Location = new System.Drawing.Point(73, 74);
            this.numClasses.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numClasses.Name = "numClasses";
            this.numClasses.ReadOnly = true;
            this.numClasses.Size = new System.Drawing.Size(333, 20);
            this.numClasses.TabIndex = 2;
            this.numClasses.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numClasses.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Método";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // cmbVariavel
            // 
            this.cmbVariavel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVariavel.FormattingEnabled = true;
            this.cmbVariavel.Location = new System.Drawing.Point(73, 18);
            this.cmbVariavel.Name = "cmbVariavel";
            this.cmbVariavel.Size = new System.Drawing.Size(333, 21);
            this.cmbVariavel.TabIndex = 0;
            // 
            // frmCalcTaxas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 495);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmCalcTaxas";
            this.Text = "Suavização de Taxas";
            this.Load += new System.EventHandler(this.frmCalcTaxas_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.grbImportacaoDosArquivos.ResumeLayout(false);
            this.tabPage0.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClasses)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox grbImportacaoDosArquivos;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnAbrirTabelaDados;
        private System.Windows.Forms.Button btnAbrirArquivoShape;
        private System.Windows.Forms.TabPage tabPage0;
        private System.Windows.Forms.TabPage tabPage2;
        private IpeaGEO.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private IpeaGEO.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbEvento;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnExecutar;
        private System.Windows.Forms.CheckBox ckbDefinicaoManual;
        private System.Windows.Forms.CheckBox chkAleatorio;
        private System.Windows.Forms.CheckBox chkGuarda;
        private System.Windows.Forms.CheckBox chkRelatorio;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbCores;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbMetodo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numClasses;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbVariavel;
    }
}