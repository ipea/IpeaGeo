namespace IpeaGEO
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancela = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.chkAleatorio = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClasses)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(209, 172);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancela
            // 
            this.btnCancela.Location = new System.Drawing.Point(290, 172);
            this.btnCancela.Name = "btnCancela";
            this.btnCancela.Size = new System.Drawing.Size(75, 23);
            this.btnCancela.TabIndex = 1;
            this.btnCancela.Text = "Cancelar";
            this.btnCancela.UseVisualStyleBackColor = true;
            this.btnCancela.Click += new System.EventHandler(this.btnCancela_Click);
            // 
            // groupBox1
            // 
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
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(353, 154);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parâmetros";
            // 
            // chkGuarda
            // 
            this.chkGuarda.AutoSize = true;
            this.chkGuarda.Checked = true;
            this.chkGuarda.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGuarda.Location = new System.Drawing.Point(107, 127);
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
            this.chkRelatorio.Location = new System.Drawing.Point(9, 127);
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
            this.cmbCores.Size = new System.Drawing.Size(274, 21);
            this.cmbCores.TabIndex = 6;
            this.cmbCores.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbCores_DrawItem);
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
            "Jenks",
            "Geométrico",
            "Desvio Padrão"});
            this.cmbMetodo.Location = new System.Drawing.Point(73, 45);
            this.cmbMetodo.Name = "cmbMetodo";
            this.cmbMetodo.Size = new System.Drawing.Size(274, 21);
            this.cmbMetodo.TabIndex = 4;
            this.cmbMetodo.SelectedIndexChanged += new System.EventHandler(this.cmbMetodo_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Classes";
            // 
            // numClasses
            // 
            this.numClasses.Location = new System.Drawing.Point(73, 74);
            this.numClasses.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numClasses.Name = "numClasses";
            this.numClasses.Size = new System.Drawing.Size(274, 20);
            this.numClasses.TabIndex = 2;
            this.numClasses.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Variável";
            // 
            // cmbVariavel
            // 
            this.cmbVariavel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVariavel.FormattingEnabled = true;
            this.cmbVariavel.Location = new System.Drawing.Point(73, 18);
            this.cmbVariavel.Name = "cmbVariavel";
            this.cmbVariavel.Size = new System.Drawing.Size(274, 21);
            this.cmbVariavel.TabIndex = 0;
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
            this.cmbDesvio.Location = new System.Drawing.Point(73, 73);
            this.cmbDesvio.Name = "cmbDesvio";
            this.cmbDesvio.Size = new System.Drawing.Size(274, 21);
            this.cmbDesvio.TabIndex = 9;
            this.cmbDesvio.Visible = false;
            // 
            // chkAleatorio
            // 
            this.chkAleatorio.AutoSize = true;
            this.chkAleatorio.Location = new System.Drawing.Point(246, 127);
            this.chkAleatorio.Name = "chkAleatorio";
            this.chkAleatorio.Size = new System.Drawing.Size(101, 17);
            this.chkAleatorio.TabIndex = 10;
            this.chkAleatorio.Text = "Cores aleatórias";
            this.chkAleatorio.UseVisualStyleBackColor = true;
            this.chkAleatorio.CheckedChanged += new System.EventHandler(this.chkAleatorio_CheckedChanged);
            // 
            // frmMapaTematico
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 201);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancela);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMapaTematico";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mapa temático";
            this.Load += new System.EventHandler(this.frmMapaTematico_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numClasses)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancela;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numClasses;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbVariavel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbMetodo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbCores;
        private System.Windows.Forms.CheckBox chkRelatorio;
        private System.Windows.Forms.CheckBox chkGuarda;
        private System.Windows.Forms.ComboBox cmbDesvio;
        private System.Windows.Forms.CheckBox chkAleatorio;
    }
}