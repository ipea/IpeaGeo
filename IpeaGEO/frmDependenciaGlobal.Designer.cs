namespace IpeaGEO
{
    partial class frmDependenciaGlobal
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
            this.btnCancelar = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbQuantitativo = new System.Windows.Forms.GroupBox();
            this.chkGetis = new System.Windows.Forms.CheckBox();
            this.chkGeary = new System.Windows.Forms.CheckBox();
            this.chkMoran = new System.Windows.Forms.CheckBox();
            this.chkMoranSimples = new System.Windows.Forms.CheckBox();
            this.gbQualitativo = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPop = new System.Windows.Forms.ComboBox();
            this.chkRogerson = new System.Windows.Forms.CheckBox();
            this.chkTango = new System.Windows.Forms.CheckBox();
            this.gbParametros = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkNormal = new System.Windows.Forms.CheckBox();
            this.chkRelatorio = new System.Windows.Forms.CheckBox();
            this.numSimula = new System.Windows.Forms.NumericUpDown();
            this.Simulações = new System.Windows.Forms.Label();
            this.cmbVizinhanca = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelProgress = new System.Windows.Forms.Label();
            this.gbQuantitativo.SuspendLayout();
            this.gbQualitativo.SuspendLayout();
            this.gbParametros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSimula)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(365, 316);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(446, 316);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.HorizontalScrollbar = true;
            this.checkedListBox1.Location = new System.Drawing.Point(5, 25);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(204, 274);
            this.checkedListBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Variáveis quantitativas:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // gbQuantitativo
            // 
            this.gbQuantitativo.Controls.Add(this.chkGetis);
            this.gbQuantitativo.Controls.Add(this.chkGeary);
            this.gbQuantitativo.Controls.Add(this.chkMoran);
            this.gbQuantitativo.Controls.Add(this.chkMoranSimples);
            this.gbQuantitativo.Location = new System.Drawing.Point(215, 25);
            this.gbQuantitativo.Name = "gbQuantitativo";
            this.gbQuantitativo.Size = new System.Drawing.Size(156, 117);
            this.gbQuantitativo.TabIndex = 1;
            this.gbQuantitativo.TabStop = false;
            this.gbQuantitativo.Text = "Indicadores quantitativos";
            // 
            // chkGetis
            // 
            this.chkGetis.AutoSize = true;
            this.chkGetis.Location = new System.Drawing.Point(10, 88);
            this.chkGetis.Name = "chkGetis";
            this.chkGetis.Size = new System.Drawing.Size(87, 17);
            this.chkGetis.TabIndex = 7;
            this.chkGetis.Text = "Getis-Ord Gi*";
            this.chkGetis.UseVisualStyleBackColor = true;
            // 
            // chkGeary
            // 
            this.chkGeary.AutoSize = true;
            this.chkGeary.Location = new System.Drawing.Point(10, 65);
            this.chkGeary.Name = "chkGeary";
            this.chkGeary.Size = new System.Drawing.Size(54, 17);
            this.chkGeary.TabIndex = 6;
            this.chkGeary.Text = "Geary";
            this.chkGeary.UseVisualStyleBackColor = true;
            // 
            // chkMoran
            // 
            this.chkMoran.AutoSize = true;
            this.chkMoran.Location = new System.Drawing.Point(10, 42);
            this.chkMoran.Name = "chkMoran";
            this.chkMoran.Size = new System.Drawing.Size(82, 17);
            this.chkMoran.TabIndex = 5;
            this.chkMoran.Text = "Moran geral";
            this.chkMoran.UseVisualStyleBackColor = true;
            // 
            // chkMoranSimples
            // 
            this.chkMoranSimples.AutoSize = true;
            this.chkMoranSimples.Location = new System.Drawing.Point(10, 19);
            this.chkMoranSimples.Name = "chkMoranSimples";
            this.chkMoranSimples.Size = new System.Drawing.Size(93, 17);
            this.chkMoranSimples.TabIndex = 4;
            this.chkMoranSimples.Text = "Moran simples";
            this.chkMoranSimples.UseVisualStyleBackColor = true;
            // 
            // gbQualitativo
            // 
            this.gbQualitativo.Controls.Add(this.groupBox2);
            this.gbQualitativo.Controls.Add(this.label2);
            this.gbQualitativo.Controls.Add(this.cmbPop);
            this.gbQualitativo.Controls.Add(this.chkRogerson);
            this.gbQualitativo.Controls.Add(this.chkTango);
            this.gbQualitativo.Location = new System.Drawing.Point(383, 25);
            this.gbQualitativo.Name = "gbQualitativo";
            this.gbQualitativo.Size = new System.Drawing.Size(138, 117);
            this.gbQualitativo.TabIndex = 3;
            this.gbQualitativo.TabStop = false;
            this.gbQualitativo.Text = "Indicadores qualitativos";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(10, 58);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(108, 10);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "População:";
            // 
            // cmbPop
            // 
            this.cmbPop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPop.FormattingEnabled = true;
            this.cmbPop.Location = new System.Drawing.Point(10, 90);
            this.cmbPop.Name = "cmbPop";
            this.cmbPop.Size = new System.Drawing.Size(118, 21);
            this.cmbPop.TabIndex = 4;
            // 
            // chkRogerson
            // 
            this.chkRogerson.AutoSize = true;
            this.chkRogerson.Location = new System.Drawing.Point(10, 42);
            this.chkRogerson.Name = "chkRogerson";
            this.chkRogerson.Size = new System.Drawing.Size(72, 17);
            this.chkRogerson.TabIndex = 3;
            this.chkRogerson.Text = "Rogerson";
            this.chkRogerson.UseVisualStyleBackColor = true;
            // 
            // chkTango
            // 
            this.chkTango.AutoSize = true;
            this.chkTango.Location = new System.Drawing.Point(10, 19);
            this.chkTango.Name = "chkTango";
            this.chkTango.Size = new System.Drawing.Size(57, 17);
            this.chkTango.TabIndex = 2;
            this.chkTango.Text = "Tango";
            this.chkTango.UseVisualStyleBackColor = true;
            this.chkTango.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // gbParametros
            // 
            this.gbParametros.Controls.Add(this.groupBox1);
            this.gbParametros.Controls.Add(this.chkNormal);
            this.gbParametros.Controls.Add(this.chkRelatorio);
            this.gbParametros.Controls.Add(this.numSimula);
            this.gbParametros.Controls.Add(this.Simulações);
            this.gbParametros.Controls.Add(this.cmbVizinhanca);
            this.gbParametros.Controls.Add(this.label3);
            this.gbParametros.Location = new System.Drawing.Point(215, 148);
            this.gbParametros.Name = "gbParametros";
            this.gbParametros.Size = new System.Drawing.Size(306, 152);
            this.gbParametros.TabIndex = 6;
            this.gbParametros.TabStop = false;
            this.gbParametros.Text = "Parâmetros";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(10, 78);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 10);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // chkNormal
            // 
            this.chkNormal.AutoSize = true;
            this.chkNormal.Checked = true;
            this.chkNormal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNormal.Location = new System.Drawing.Point(7, 61);
            this.chkNormal.Name = "chkNormal";
            this.chkNormal.Size = new System.Drawing.Size(75, 17);
            this.chkNormal.TabIndex = 18;
            this.chkNormal.Text = "Normalizar";
            this.chkNormal.UseVisualStyleBackColor = true;
            // 
            // chkRelatorio
            // 
            this.chkRelatorio.AutoSize = true;
            this.chkRelatorio.Checked = true;
            this.chkRelatorio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRelatorio.Location = new System.Drawing.Point(6, 133);
            this.chkRelatorio.Name = "chkRelatorio";
            this.chkRelatorio.Size = new System.Drawing.Size(89, 17);
            this.chkRelatorio.TabIndex = 17;
            this.chkRelatorio.Text = "Gera relatório";
            this.chkRelatorio.UseVisualStyleBackColor = true;
            // 
            // numSimula
            // 
            this.numSimula.Location = new System.Drawing.Point(6, 107);
            this.numSimula.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numSimula.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numSimula.Name = "numSimula";
            this.numSimula.Size = new System.Drawing.Size(290, 20);
            this.numSimula.TabIndex = 3;
            this.numSimula.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // Simulações
            // 
            this.Simulações.AutoSize = true;
            this.Simulações.Location = new System.Drawing.Point(3, 91);
            this.Simulações.Name = "Simulações";
            this.Simulações.Size = new System.Drawing.Size(114, 13);
            this.Simulações.TabIndex = 2;
            this.Simulações.Text = "Número de simulações";
            // 
            // cmbVizinhanca
            // 
            this.cmbVizinhanca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVizinhanca.FormattingEnabled = true;
            this.cmbVizinhanca.Items.AddRange(new object[] {
            "Queen",
            "Rook"});
            this.cmbVizinhanca.Location = new System.Drawing.Point(6, 36);
            this.cmbVizinhanca.Name = "cmbVizinhanca";
            this.cmbVizinhanca.Size = new System.Drawing.Size(290, 21);
            this.cmbVizinhanca.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Vizinhança";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(5, 318);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(354, 23);
            this.progressBar1.TabIndex = 7;
            this.progressBar1.Visible = false;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(9, 302);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(0, 13);
            this.labelProgress.TabIndex = 8;
            // 
            // frmDependenciaGlobal
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(534, 348);
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.gbParametros);
            this.Controls.Add(this.gbQualitativo);
            this.Controls.Add(this.gbQuantitativo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDependenciaGlobal";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Índice de dependência espacial global";
            this.Load += new System.EventHandler(this.frmDependenciaGlobal_Load);
            this.gbQuantitativo.ResumeLayout(false);
            this.gbQuantitativo.PerformLayout();
            this.gbQualitativo.ResumeLayout(false);
            this.gbQualitativo.PerformLayout();
            this.gbParametros.ResumeLayout(false);
            this.gbParametros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSimula)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbQuantitativo;
        private System.Windows.Forms.GroupBox gbQualitativo;
        private System.Windows.Forms.GroupBox gbParametros;
        private System.Windows.Forms.NumericUpDown numSimula;
        private System.Windows.Forms.Label Simulações;
        private System.Windows.Forms.ComboBox cmbVizinhanca;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkRelatorio;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkNormal;
        private System.Windows.Forms.CheckBox chkGetis;
        private System.Windows.Forms.CheckBox chkGeary;
        private System.Windows.Forms.CheckBox chkMoran;
        private System.Windows.Forms.CheckBox chkMoranSimples;
        private System.Windows.Forms.CheckBox chkRogerson;
        private System.Windows.Forms.CheckBox chkTango;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbPop;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}