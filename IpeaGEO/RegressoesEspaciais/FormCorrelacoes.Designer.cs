namespace IpeaGeo.RegressoesEspaciais
{
    partial class FormCorrelacoes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCorrelacoes));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.userControlSelecaoVariaveis1 = new IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdbDadosAmostrais = new System.Windows.Forms.RadioButton();
            this.rdbDadosPopulacionais = new System.Windows.Forms.RadioButton();
            this.ckbCorrSpearman = new System.Windows.Forms.CheckBox();
            this.ckbMatrizCorrPearson = new System.Windows.Forms.CheckBox();
            this.ckbCovMatrix = new System.Windows.Forms.CheckBox();
            this.nudNumCasasDecimais = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.btnFechar = new System.Windows.Forms.Button();
            this.btnCalcular = new System.Windows.Forms.Button();
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
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumCasasDecimais)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "application_form.png");
            this.imageList1.Images.SetKeyName(1, "application_lightning.png");
            this.imageList1.Images.SetKeyName(2, "application_double.png");
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
            this.splitContainer1.Panel2.Controls.Add(this.btnFechar);
            this.splitContainer1.Panel2.Controls.Add(this.btnCalcular);
            this.splitContainer1.Size = new System.Drawing.Size(904, 644);
            this.splitContainer1.SplitterDistance = 604;
            this.splitContainer1.TabIndex = 1;
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
            this.tabControl1.Size = new System.Drawing.Size(904, 604);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer2);
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(896, 577);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Especificações";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(890, 571);
            this.splitContainer2.SplitterDistance = 676;
            this.splitContainer2.TabIndex = 2;
            // 
            // userControlSelecaoVariaveis1
            // 
            this.userControlSelecaoVariaveis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSelecaoVariaveis1.Location = new System.Drawing.Point(0, 0);
            this.userControlSelecaoVariaveis1.Name = "userControlSelecaoVariaveis1";
            this.userControlSelecaoVariaveis1.Size = new System.Drawing.Size(676, 571);
            this.userControlSelecaoVariaveis1.TabIndex = 1;
            this.userControlSelecaoVariaveis1.VariaveisDB = null;
            this.userControlSelecaoVariaveis1.VariaveisIndependentes = new string[0];
            this.userControlSelecaoVariaveis1.VariaveisList = null;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.ckbCorrSpearman);
            this.groupBox1.Controls.Add(this.ckbMatrizCorrPearson);
            this.groupBox1.Controls.Add(this.ckbCovMatrix);
            this.groupBox1.Controls.Add(this.nudNumCasasDecimais);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(210, 571);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Opções";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.rdbDadosAmostrais);
            this.groupBox2.Controls.Add(this.rdbDadosPopulacionais);
            this.groupBox2.Location = new System.Drawing.Point(13, 405);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(184, 75);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            // 
            // rdbDadosAmostrais
            // 
            this.rdbDadosAmostrais.AutoSize = true;
            this.rdbDadosAmostrais.Location = new System.Drawing.Point(15, 19);
            this.rdbDadosAmostrais.Name = "rdbDadosAmostrais";
            this.rdbDadosAmostrais.Size = new System.Drawing.Size(103, 17);
            this.rdbDadosAmostrais.TabIndex = 1;
            this.rdbDadosAmostrais.Text = "Dados amostrais";
            this.rdbDadosAmostrais.UseVisualStyleBackColor = true;
            // 
            // rdbDadosPopulacionais
            // 
            this.rdbDadosPopulacionais.AutoSize = true;
            this.rdbDadosPopulacionais.Checked = true;
            this.rdbDadosPopulacionais.Location = new System.Drawing.Point(15, 42);
            this.rdbDadosPopulacionais.Name = "rdbDadosPopulacionais";
            this.rdbDadosPopulacionais.Size = new System.Drawing.Size(124, 17);
            this.rdbDadosPopulacionais.TabIndex = 0;
            this.rdbDadosPopulacionais.TabStop = true;
            this.rdbDadosPopulacionais.Text = "Dados populacionais";
            this.rdbDadosPopulacionais.UseVisualStyleBackColor = true;
            // 
            // ckbCorrSpearman
            // 
            this.ckbCorrSpearman.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbCorrSpearman.AutoSize = true;
            this.ckbCorrSpearman.Checked = true;
            this.ckbCorrSpearman.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbCorrSpearman.Location = new System.Drawing.Point(20, 360);
            this.ckbCorrSpearman.Name = "ckbCorrSpearman";
            this.ckbCorrSpearman.Size = new System.Drawing.Size(148, 17);
            this.ckbCorrSpearman.TabIndex = 15;
            this.ckbCorrSpearman.Text = "Correlações de Spearman";
            this.ckbCorrSpearman.UseVisualStyleBackColor = true;
            // 
            // ckbMatrizCorrPearson
            // 
            this.ckbMatrizCorrPearson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbMatrizCorrPearson.AutoSize = true;
            this.ckbMatrizCorrPearson.Checked = true;
            this.ckbMatrizCorrPearson.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbMatrizCorrPearson.Location = new System.Drawing.Point(20, 337);
            this.ckbMatrizCorrPearson.Name = "ckbMatrizCorrPearson";
            this.ckbMatrizCorrPearson.Size = new System.Drawing.Size(139, 17);
            this.ckbMatrizCorrPearson.TabIndex = 14;
            this.ckbMatrizCorrPearson.Text = "Correlações de Pearson";
            this.ckbMatrizCorrPearson.UseVisualStyleBackColor = true;
            // 
            // ckbCovMatrix
            // 
            this.ckbCovMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbCovMatrix.AutoSize = true;
            this.ckbCovMatrix.Checked = true;
            this.ckbCovMatrix.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbCovMatrix.Location = new System.Drawing.Point(20, 314);
            this.ckbCovMatrix.Name = "ckbCovMatrix";
            this.ckbCovMatrix.Size = new System.Drawing.Size(132, 17);
            this.ckbCovMatrix.TabIndex = 13;
            this.ckbCovMatrix.Text = "Matriz de covariâncias";
            this.ckbCovMatrix.UseVisualStyleBackColor = true;
            // 
            // nudNumCasasDecimais
            // 
            this.nudNumCasasDecimais.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudNumCasasDecimais.Location = new System.Drawing.Point(132, 519);
            this.nudNumCasasDecimais.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nudNumCasasDecimais.Name = "nudNumCasasDecimais";
            this.nudNumCasasDecimais.Size = new System.Drawing.Size(51, 20);
            this.nudNumCasasDecimais.TabIndex = 12;
            this.nudNumCasasDecimais.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudNumCasasDecimais.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 521);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Núm. casas decimais";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.userControlRichTextOutput1);
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(896, 577);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Resultados";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput1
            // 
            this.userControlRichTextOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput1.Location = new System.Drawing.Point(3, 3);
            this.userControlRichTextOutput1.Name = "userControlRichTextOutput1";
            this.userControlRichTextOutput1.Size = new System.Drawing.Size(890, 571);
            this.userControlRichTextOutput1.TabIndex = 0;
            this.userControlRichTextOutput1.Texto = "";
            // 
            // btnFechar
            // 
            this.btnFechar.Location = new System.Drawing.Point(39, 4);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(75, 23);
            this.btnFechar.TabIndex = 4;
            this.btnFechar.Text = "&Fechar";
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // btnCalcular
            // 
            this.btnCalcular.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCalcular.Location = new System.Drawing.Point(791, 4);
            this.btnCalcular.Name = "btnCalcular";
            this.btnCalcular.Size = new System.Drawing.Size(75, 23);
            this.btnCalcular.TabIndex = 0;
            this.btnCalcular.Text = "&Calcular";
            this.btnCalcular.UseVisualStyleBackColor = true;
            this.btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            // 
            // FormCorrelacoes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 644);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(912, 671);
            this.Name = "FormCorrelacoes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Correlações";
            this.Load += new System.EventHandler(this.FormCorrelacoes_Load);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumCasasDecimais)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private UserControlSelecaoVariaveis userControlSelecaoVariaveis1;
        private System.Windows.Forms.TabPage tabPage2;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput1;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.Button btnCalcular;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ckbCorrSpearman;
        private System.Windows.Forms.CheckBox ckbMatrizCorrPearson;
        private System.Windows.Forms.CheckBox ckbCovMatrix;
        private System.Windows.Forms.NumericUpDown nudNumCasasDecimais;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbDadosAmostrais;
        private System.Windows.Forms.RadioButton rdbDadosPopulacionais;
    }
}