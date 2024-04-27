namespace IpeaGeo.RegressoesEspaciais
{
    partial class FormTabelasCruzadas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTabelasCruzadas));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.userControlSelecaoVariaveis1 = new IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbPercentuaisTotal = new System.Windows.Forms.CheckBox();
            this.ckbPercentualColunas = new System.Windows.Forms.CheckBox();
            this.ckbPercentualLinhas = new System.Windows.Forms.CheckBox();
            this.ckbFreqAbsoluta = new System.Windows.Forms.CheckBox();
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
            this.splitContainer1.Size = new System.Drawing.Size(905, 565);
            this.splitContainer1.SplitterDistance = 525;
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
            this.tabControl1.Size = new System.Drawing.Size(905, 525);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer2);
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(897, 498);
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
            this.splitContainer2.Size = new System.Drawing.Size(891, 492);
            this.splitContainer2.SplitterDistance = 676;
            this.splitContainer2.TabIndex = 2;
            // 
            // userControlSelecaoVariaveis1
            // 
            this.userControlSelecaoVariaveis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSelecaoVariaveis1.Location = new System.Drawing.Point(0, 0);
            this.userControlSelecaoVariaveis1.Name = "userControlSelecaoVariaveis1";
            this.userControlSelecaoVariaveis1.Size = new System.Drawing.Size(676, 492);
            this.userControlSelecaoVariaveis1.TabIndex = 1;
            this.userControlSelecaoVariaveis1.VariaveisDB = null;
            this.userControlSelecaoVariaveis1.VariaveisIndependentes = new string[0];
            this.userControlSelecaoVariaveis1.VariaveisList = null;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckbPercentuaisTotal);
            this.groupBox1.Controls.Add(this.ckbPercentualColunas);
            this.groupBox1.Controls.Add(this.ckbPercentualLinhas);
            this.groupBox1.Controls.Add(this.ckbFreqAbsoluta);
            this.groupBox1.Controls.Add(this.nudNumCasasDecimais);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(211, 492);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Opções";
            // 
            // ckbPercentuaisTotal
            // 
            this.ckbPercentuaisTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbPercentuaisTotal.AutoSize = true;
            this.ckbPercentuaisTotal.Checked = true;
            this.ckbPercentuaisTotal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbPercentuaisTotal.Location = new System.Drawing.Point(24, 365);
            this.ckbPercentuaisTotal.Name = "ckbPercentuaisTotal";
            this.ckbPercentuaisTotal.Size = new System.Drawing.Size(137, 17);
            this.ckbPercentuaisTotal.TabIndex = 16;
            this.ckbPercentuaisTotal.Text = "Percentuais do total (%)";
            this.ckbPercentuaisTotal.UseVisualStyleBackColor = true;
            // 
            // ckbPercentualColunas
            // 
            this.ckbPercentualColunas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbPercentualColunas.AutoSize = true;
            this.ckbPercentualColunas.Checked = true;
            this.ckbPercentualColunas.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbPercentualColunas.Location = new System.Drawing.Point(24, 342);
            this.ckbPercentualColunas.Name = "ckbPercentualColunas";
            this.ckbPercentualColunas.Size = new System.Drawing.Size(149, 17);
            this.ckbPercentualColunas.TabIndex = 15;
            this.ckbPercentualColunas.Text = "Percentuais na coluna (%)";
            this.ckbPercentualColunas.UseVisualStyleBackColor = true;
            // 
            // ckbPercentualLinhas
            // 
            this.ckbPercentualLinhas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbPercentualLinhas.AutoSize = true;
            this.ckbPercentualLinhas.Checked = true;
            this.ckbPercentualLinhas.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbPercentualLinhas.Location = new System.Drawing.Point(24, 319);
            this.ckbPercentualLinhas.Name = "ckbPercentualLinhas";
            this.ckbPercentualLinhas.Size = new System.Drawing.Size(139, 17);
            this.ckbPercentualLinhas.TabIndex = 14;
            this.ckbPercentualLinhas.Text = "Percentuais na linha (%)";
            this.ckbPercentualLinhas.UseVisualStyleBackColor = true;
            // 
            // ckbFreqAbsoluta
            // 
            this.ckbFreqAbsoluta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbFreqAbsoluta.AutoSize = true;
            this.ckbFreqAbsoluta.Checked = true;
            this.ckbFreqAbsoluta.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbFreqAbsoluta.Location = new System.Drawing.Point(24, 296);
            this.ckbFreqAbsoluta.Name = "ckbFreqAbsoluta";
            this.ckbFreqAbsoluta.Size = new System.Drawing.Size(132, 17);
            this.ckbFreqAbsoluta.TabIndex = 13;
            this.ckbFreqAbsoluta.Text = "Frequências absolutas";
            this.ckbFreqAbsoluta.UseVisualStyleBackColor = true;
            // 
            // nudNumCasasDecimais
            // 
            this.nudNumCasasDecimais.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudNumCasasDecimais.Location = new System.Drawing.Point(134, 440);
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
            this.label5.Location = new System.Drawing.Point(21, 442);
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
            this.tabPage2.Size = new System.Drawing.Size(897, 498);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Resultados";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput1
            // 
            this.userControlRichTextOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput1.Location = new System.Drawing.Point(3, 3);
            this.userControlRichTextOutput1.Name = "userControlRichTextOutput1";
            this.userControlRichTextOutput1.Size = new System.Drawing.Size(891, 492);
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
            this.btnCalcular.Location = new System.Drawing.Point(792, 4);
            this.btnCalcular.Name = "btnCalcular";
            this.btnCalcular.Size = new System.Drawing.Size(75, 23);
            this.btnCalcular.TabIndex = 0;
            this.btnCalcular.Text = "&Calcular";
            this.btnCalcular.UseVisualStyleBackColor = true;
            this.btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            // 
            // FormTabelasCruzadas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 565);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(913, 592);
            this.Name = "FormTabelasCruzadas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tabelas Cruzadas";
            this.Load += new System.EventHandler(this.FormTabelasCruzadas_Load);
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
        private System.Windows.Forms.CheckBox ckbPercentualColunas;
        private System.Windows.Forms.CheckBox ckbPercentualLinhas;
        private System.Windows.Forms.CheckBox ckbFreqAbsoluta;
        private System.Windows.Forms.NumericUpDown nudNumCasasDecimais;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox ckbPercentuaisTotal;
    }
}