namespace IpeaGeo
{
    partial class frmDependenciaLocal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDependenciaLocal));
            this.labelProgress = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.gbParametros = new System.Windows.Forms.GroupBox();
            this.chkNormal = new System.Windows.Forms.CheckBox();
            this.cmbVizinhanca = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkRelatorio = new System.Windows.Forms.CheckBox();
            this.gbQualitativo = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPop = new System.Windows.Forms.ComboBox();
            this.chkEscore = new System.Windows.Forms.CheckBox();
            this.gbQuantitativo = new System.Windows.Forms.GroupBox();
            this.chkGetis2 = new System.Windows.Forms.CheckBox();
            this.chkGetis = new System.Windows.Forms.CheckBox();
            this.chkLISA = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.gbCores = new System.Windows.Forms.GroupBox();
            this.txtBaixoAlto = new System.Windows.Forms.TextBox();
            this.txtAltoBaixo = new System.Windows.Forms.TextBox();
            this.txtBaixoBaixo = new System.Windows.Forms.TextBox();
            this.txtAltoAlto = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.gbConfia = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.rdSidak = new System.Windows.Forms.RadioButton();
            this.rdBonferroni = new System.Windows.Forms.RadioButton();
            this.gbParametros.SuspendLayout();
            this.gbQualitativo.SuspendLayout();
            this.gbQuantitativo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.gbCores.SuspendLayout();
            this.gbConfia.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(12, 362);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(0, 13);
            this.labelProgress.TabIndex = 17;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar1.Location = new System.Drawing.Point(113, 408);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(565, 10);
            this.progressBar1.TabIndex = 16;
            this.progressBar1.Visible = false;
            // 
            // gbParametros
            // 
            this.gbParametros.Controls.Add(this.chkNormal);
            this.gbParametros.Controls.Add(this.cmbVizinhanca);
            this.gbParametros.Controls.Add(this.label3);
            this.gbParametros.Location = new System.Drawing.Point(448, 190);
            this.gbParametros.Name = "gbParametros";
            this.gbParametros.Size = new System.Drawing.Size(335, 86);
            this.gbParametros.TabIndex = 15;
            this.gbParametros.TabStop = false;
            this.gbParametros.Text = "Parâmetros";
            // 
            // chkNormal
            // 
            this.chkNormal.AutoSize = true;
            this.chkNormal.Checked = true;
            this.chkNormal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNormal.Location = new System.Drawing.Point(248, 63);
            this.chkNormal.Name = "chkNormal";
            this.chkNormal.Size = new System.Drawing.Size(75, 17);
            this.chkNormal.TabIndex = 18;
            this.chkNormal.Text = "Normalizar";
            this.chkNormal.UseVisualStyleBackColor = true;
            // 
            // cmbVizinhanca
            // 
            this.cmbVizinhanca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVizinhanca.Enabled = false;
            this.cmbVizinhanca.FormattingEnabled = true;
            this.cmbVizinhanca.Items.AddRange(new object[] {
            "Queen",
            "Rook"});
            this.cmbVizinhanca.Location = new System.Drawing.Point(6, 36);
            this.cmbVizinhanca.Name = "cmbVizinhanca";
            this.cmbVizinhanca.Size = new System.Drawing.Size(317, 21);
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
            // chkRelatorio
            // 
            this.chkRelatorio.AutoSize = true;
            this.chkRelatorio.Location = new System.Drawing.Point(682, 362);
            this.chkRelatorio.Name = "chkRelatorio";
            this.chkRelatorio.Size = new System.Drawing.Size(89, 17);
            this.chkRelatorio.TabIndex = 17;
            this.chkRelatorio.Text = "Gera relatório";
            this.chkRelatorio.UseVisualStyleBackColor = true;
            // 
            // gbQualitativo
            // 
            this.gbQualitativo.Controls.Add(this.label2);
            this.gbQualitativo.Controls.Add(this.cmbPop);
            this.gbQualitativo.Controls.Add(this.chkEscore);
            this.gbQualitativo.Location = new System.Drawing.Point(611, 12);
            this.gbQualitativo.Name = "gbQualitativo";
            this.gbQualitativo.Size = new System.Drawing.Size(172, 80);
            this.gbQualitativo.TabIndex = 14;
            this.gbQualitativo.TabStop = false;
            this.gbQualitativo.Text = "Indicadores de Conglomerados";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "População:";
            // 
            // cmbPop
            // 
            this.cmbPop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPop.Enabled = false;
            this.cmbPop.FormattingEnabled = true;
            this.cmbPop.Location = new System.Drawing.Point(6, 56);
            this.cmbPop.Name = "cmbPop";
            this.cmbPop.Size = new System.Drawing.Size(146, 21);
            this.cmbPop.TabIndex = 4;
            // 
            // chkEscore
            // 
            this.chkEscore.AutoSize = true;
            this.chkEscore.Location = new System.Drawing.Point(10, 19);
            this.chkEscore.Name = "chkEscore";
            this.chkEscore.Size = new System.Drawing.Size(59, 17);
            this.chkEscore.TabIndex = 2;
            this.chkEscore.Text = "Escore";
            this.chkEscore.UseVisualStyleBackColor = true;
            this.chkEscore.CheckedChanged += new System.EventHandler(this.chkEscore_CheckedChanged);
            // 
            // gbQuantitativo
            // 
            this.gbQuantitativo.Controls.Add(this.chkGetis2);
            this.gbQuantitativo.Controls.Add(this.chkGetis);
            this.gbQuantitativo.Controls.Add(this.chkLISA);
            this.gbQuantitativo.Location = new System.Drawing.Point(447, 12);
            this.gbQuantitativo.Name = "gbQuantitativo";
            this.gbQuantitativo.Size = new System.Drawing.Size(162, 80);
            this.gbQuantitativo.TabIndex = 10;
            this.gbQuantitativo.TabStop = false;
            this.gbQuantitativo.Text = "Indicadores de Dependência";
            // 
            // chkGetis2
            // 
            this.chkGetis2.AutoSize = true;
            this.chkGetis2.Location = new System.Drawing.Point(10, 56);
            this.chkGetis2.Name = "chkGetis2";
            this.chkGetis2.Size = new System.Drawing.Size(83, 17);
            this.chkGetis2.TabIndex = 8;
            this.chkGetis2.Text = "Getis-Ord Gi";
            this.chkGetis2.UseVisualStyleBackColor = true;
            this.chkGetis2.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // chkGetis
            // 
            this.chkGetis.AutoSize = true;
            this.chkGetis.Location = new System.Drawing.Point(10, 37);
            this.chkGetis.Name = "chkGetis";
            this.chkGetis.Size = new System.Drawing.Size(87, 17);
            this.chkGetis.TabIndex = 7;
            this.chkGetis.Text = "Getis-Ord Gi*";
            this.chkGetis.UseVisualStyleBackColor = true;
            this.chkGetis.CheckedChanged += new System.EventHandler(this.chkGetis_CheckedChanged);
            // 
            // chkLISA
            // 
            this.chkLISA.AutoSize = true;
            this.chkLISA.Location = new System.Drawing.Point(10, 19);
            this.chkLISA.Name = "chkLISA";
            this.chkLISA.Size = new System.Drawing.Size(49, 17);
            this.chkLISA.TabIndex = 4;
            this.chkLISA.Text = "LISA";
            this.chkLISA.UseVisualStyleBackColor = true;
            this.chkLISA.CheckedChanged += new System.EventHandler(this.chkLISA_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Variáveis quantitativas:";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.HorizontalScrollbar = true;
            this.checkedListBox1.Location = new System.Drawing.Point(8, 23);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(434, 334);
            this.checkedListBox1.TabIndex = 12;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(25, 402);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 11;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(691, 402);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "&Executar";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 4;
            this.numericUpDown1.Location = new System.Drawing.Point(6, 31);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(314, 20);
            this.numericUpDown1.TabIndex = 18;
            this.numericUpDown1.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Nível de confiança";
            // 
            // gbCores
            // 
            this.gbCores.Controls.Add(this.txtBaixoAlto);
            this.gbCores.Controls.Add(this.txtAltoBaixo);
            this.gbCores.Controls.Add(this.txtBaixoBaixo);
            this.gbCores.Controls.Add(this.txtAltoAlto);
            this.gbCores.Controls.Add(this.label8);
            this.gbCores.Controls.Add(this.label7);
            this.gbCores.Controls.Add(this.label6);
            this.gbCores.Controls.Add(this.label5);
            this.gbCores.Location = new System.Drawing.Point(448, 282);
            this.gbCores.Name = "gbCores";
            this.gbCores.Size = new System.Drawing.Size(335, 73);
            this.gbCores.TabIndex = 20;
            this.gbCores.TabStop = false;
            this.gbCores.Text = "Cores";
            // 
            // txtBaixoAlto
            // 
            this.txtBaixoAlto.BackColor = System.Drawing.Color.Purple;
            this.txtBaixoAlto.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtBaixoAlto.Location = new System.Drawing.Point(262, 42);
            this.txtBaixoAlto.Name = "txtBaixoAlto";
            this.txtBaixoAlto.ReadOnly = true;
            this.txtBaixoAlto.Size = new System.Drawing.Size(57, 20);
            this.txtBaixoAlto.TabIndex = 7;
            this.txtBaixoAlto.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtBaixoAlto_MouseDown);
            // 
            // txtAltoBaixo
            // 
            this.txtAltoBaixo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.txtAltoBaixo.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtAltoBaixo.Location = new System.Drawing.Point(70, 42);
            this.txtAltoBaixo.Name = "txtAltoBaixo";
            this.txtAltoBaixo.ReadOnly = true;
            this.txtAltoBaixo.Size = new System.Drawing.Size(57, 20);
            this.txtAltoBaixo.TabIndex = 6;
            this.txtAltoBaixo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtAltoBaixo_MouseDown);
            // 
            // txtBaixoBaixo
            // 
            this.txtBaixoBaixo.BackColor = System.Drawing.Color.Blue;
            this.txtBaixoBaixo.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtBaixoBaixo.Location = new System.Drawing.Point(262, 17);
            this.txtBaixoBaixo.Name = "txtBaixoBaixo";
            this.txtBaixoBaixo.ReadOnly = true;
            this.txtBaixoBaixo.Size = new System.Drawing.Size(57, 20);
            this.txtBaixoBaixo.TabIndex = 5;
            this.txtBaixoBaixo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtBaixoBaixo_MouseDown);
            // 
            // txtAltoAlto
            // 
            this.txtAltoAlto.BackColor = System.Drawing.Color.Red;
            this.txtAltoAlto.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtAltoAlto.Location = new System.Drawing.Point(70, 17);
            this.txtAltoAlto.Name = "txtAltoAlto";
            this.txtAltoAlto.ReadOnly = true;
            this.txtAltoAlto.Size = new System.Drawing.Size(57, 20);
            this.txtAltoAlto.TabIndex = 4;
            this.txtAltoAlto.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtAltoAlto_MouseDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(194, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Baixo-Alto";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(194, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Baixo-Baixo";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Alto-Baixo";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Alto-Alto";
            // 
            // gbConfia
            // 
            this.gbConfia.Controls.Add(this.radioButton1);
            this.gbConfia.Controls.Add(this.label9);
            this.gbConfia.Controls.Add(this.rdSidak);
            this.gbConfia.Controls.Add(this.rdBonferroni);
            this.gbConfia.Controls.Add(this.numericUpDown1);
            this.gbConfia.Controls.Add(this.label4);
            this.gbConfia.Location = new System.Drawing.Point(448, 98);
            this.gbConfia.Name = "gbConfia";
            this.gbConfia.Size = new System.Drawing.Size(335, 86);
            this.gbConfia.TabIndex = 21;
            this.gbConfia.TabStop = false;
            this.gbConfia.Text = "Confiabilidade";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(118, 60);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(65, 17);
            this.radioButton1.TabIndex = 23;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Nenhum";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 62);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Método de correção:";
            // 
            // rdSidak
            // 
            this.rdSidak.AutoSize = true;
            this.rdSidak.Location = new System.Drawing.Point(185, 60);
            this.rdSidak.Name = "rdSidak";
            this.rdSidak.Size = new System.Drawing.Size(52, 17);
            this.rdSidak.TabIndex = 21;
            this.rdSidak.Text = "Sidák";
            this.rdSidak.UseVisualStyleBackColor = true;
            // 
            // rdBonferroni
            // 
            this.rdBonferroni.AutoSize = true;
            this.rdBonferroni.Location = new System.Drawing.Point(239, 60);
            this.rdBonferroni.Name = "rdBonferroni";
            this.rdBonferroni.Size = new System.Drawing.Size(73, 17);
            this.rdBonferroni.TabIndex = 20;
            this.rdBonferroni.Text = "Bonferroni";
            this.rdBonferroni.UseVisualStyleBackColor = true;
            // 
            // frmDependenciaLocal
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(787, 437);
            this.Controls.Add(this.gbConfia);
            this.Controls.Add(this.gbCores);
            this.Controls.Add(this.chkRelatorio);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDependenciaLocal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Índices de Dependência Espacial Local";
            this.Load += new System.EventHandler(this.frmDependenciaLocal_Load);
            this.gbParametros.ResumeLayout(false);
            this.gbParametros.PerformLayout();
            this.gbQualitativo.ResumeLayout(false);
            this.gbQualitativo.PerformLayout();
            this.gbQuantitativo.ResumeLayout(false);
            this.gbQuantitativo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.gbCores.ResumeLayout(false);
            this.gbCores.PerformLayout();
            this.gbConfia.ResumeLayout(false);
            this.gbConfia.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox gbParametros;
        private System.Windows.Forms.CheckBox chkNormal;
        private System.Windows.Forms.CheckBox chkRelatorio;
        private System.Windows.Forms.ComboBox cmbVizinhanca;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbQualitativo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbPop;
        private System.Windows.Forms.CheckBox chkEscore;
        private System.Windows.Forms.GroupBox gbQuantitativo;
        private System.Windows.Forms.CheckBox chkGetis;
        private System.Windows.Forms.CheckBox chkLISA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbCores;
        private System.Windows.Forms.TextBox txtBaixoAlto;
        private System.Windows.Forms.TextBox txtAltoBaixo;
        private System.Windows.Forms.TextBox txtBaixoBaixo;
        private System.Windows.Forms.TextBox txtAltoAlto;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox gbConfia;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton rdSidak;
        private System.Windows.Forms.RadioButton rdBonferroni;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.CheckBox chkGetis2;
    }
}