namespace IpeaGeo.RegressoesEspaciais
{
    partial class FormEstatisticasDescritivas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEstatisticasDescritivas));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.userControlSelecaoVariaveis1 = new IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.userControlSelecaoVariaveis2 = new IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis();
            this.userControlSelecaoVariaveis3 = new IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudNumCasasDecimais = new System.Windows.Forms.NumericUpDown();
            this.btnCalcular = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdbDadosAmostrais = new System.Windows.Forms.RadioButton();
            this.rdbDadosPopulacionais = new System.Windows.Forms.RadioButton();
            this.nudPercentil4 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudPercentil3 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudPercentil2 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudPercentil1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.btnFechar = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumCasasDecimais)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPercentil4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPercentil3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPercentil2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPercentil1)).BeginInit();
            this.tabPage2.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this.btnFechar);
            this.splitContainer1.Size = new System.Drawing.Size(1028, 618);
            this.splitContainer1.SplitterDistance = 583;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList2;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1028, 583);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer2);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1020, 556);
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
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(1014, 550);
            this.splitContainer2.SplitterDistance = 771;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.userControlSelecaoVariaveis1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Size = new System.Drawing.Size(771, 550);
            this.splitContainer3.SplitterDistance = 173;
            this.splitContainer3.TabIndex = 0;
            // 
            // userControlSelecaoVariaveis1
            // 
            this.userControlSelecaoVariaveis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSelecaoVariaveis1.Location = new System.Drawing.Point(0, 0);
            this.userControlSelecaoVariaveis1.Name = "userControlSelecaoVariaveis1";
            this.userControlSelecaoVariaveis1.Size = new System.Drawing.Size(771, 173);
            this.userControlSelecaoVariaveis1.TabIndex = 0;
            this.userControlSelecaoVariaveis1.VariaveisDB = null;
            this.userControlSelecaoVariaveis1.VariaveisIndependentes = new string[0];
            this.userControlSelecaoVariaveis1.VariaveisList = null;
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
            this.splitContainer4.Panel1.Controls.Add(this.userControlSelecaoVariaveis2);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.userControlSelecaoVariaveis3);
            this.splitContainer4.Size = new System.Drawing.Size(771, 373);
            this.splitContainer4.SplitterDistance = 199;
            this.splitContainer4.TabIndex = 0;
            // 
            // userControlSelecaoVariaveis2
            // 
            this.userControlSelecaoVariaveis2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSelecaoVariaveis2.Location = new System.Drawing.Point(0, 0);
            this.userControlSelecaoVariaveis2.Name = "userControlSelecaoVariaveis2";
            this.userControlSelecaoVariaveis2.Size = new System.Drawing.Size(771, 199);
            this.userControlSelecaoVariaveis2.TabIndex = 1;
            this.userControlSelecaoVariaveis2.VariaveisDB = null;
            this.userControlSelecaoVariaveis2.VariaveisIndependentes = new string[0];
            this.userControlSelecaoVariaveis2.VariaveisList = null;
            // 
            // userControlSelecaoVariaveis3
            // 
            this.userControlSelecaoVariaveis3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSelecaoVariaveis3.Location = new System.Drawing.Point(0, 0);
            this.userControlSelecaoVariaveis3.Name = "userControlSelecaoVariaveis3";
            this.userControlSelecaoVariaveis3.Size = new System.Drawing.Size(771, 170);
            this.userControlSelecaoVariaveis3.TabIndex = 1;
            this.userControlSelecaoVariaveis3.VariaveisDB = null;
            this.userControlSelecaoVariaveis3.VariaveisIndependentes = new string[0];
            this.userControlSelecaoVariaveis3.VariaveisList = null;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudNumCasasDecimais);
            this.groupBox1.Controls.Add(this.btnCalcular);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.nudPercentil4);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nudPercentil3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nudPercentil2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.nudPercentil1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(239, 550);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Opções";
            // 
            // nudNumCasasDecimais
            // 
            this.nudNumCasasDecimais.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudNumCasasDecimais.Location = new System.Drawing.Point(146, 237);
            this.nudNumCasasDecimais.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nudNumCasasDecimais.Name = "nudNumCasasDecimais";
            this.nudNumCasasDecimais.Size = new System.Drawing.Size(51, 20);
            this.nudNumCasasDecimais.TabIndex = 10;
            this.nudNumCasasDecimais.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudNumCasasDecimais.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // btnCalcular
            // 
            this.btnCalcular.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCalcular.Location = new System.Drawing.Point(122, 507);
            this.btnCalcular.Name = "btnCalcular";
            this.btnCalcular.Size = new System.Drawing.Size(75, 23);
            this.btnCalcular.TabIndex = 2;
            this.btnCalcular.Text = "&Calcular";
            this.btnCalcular.UseVisualStyleBackColor = true;
            this.btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 239);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Núm. casas decimais";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.rdbDadosAmostrais);
            this.groupBox2.Controls.Add(this.rdbDadosPopulacionais);
            this.groupBox2.Location = new System.Drawing.Point(21, 268);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 75);
            this.groupBox2.TabIndex = 8;
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
            // nudPercentil4
            // 
            this.nudPercentil4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudPercentil4.DecimalPlaces = 4;
            this.nudPercentil4.Location = new System.Drawing.Point(112, 440);
            this.nudPercentil4.Name = "nudPercentil4";
            this.nudPercentil4.Size = new System.Drawing.Size(85, 20);
            this.nudPercentil4.TabIndex = 7;
            this.nudPercentil4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudPercentil4.Value = new decimal(new int[] {
            95,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 442);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Percentil 4 (%)";
            // 
            // nudPercentil3
            // 
            this.nudPercentil3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudPercentil3.DecimalPlaces = 4;
            this.nudPercentil3.Location = new System.Drawing.Point(112, 414);
            this.nudPercentil3.Name = "nudPercentil3";
            this.nudPercentil3.Size = new System.Drawing.Size(85, 20);
            this.nudPercentil3.TabIndex = 5;
            this.nudPercentil3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudPercentil3.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 416);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Percentil 3 (%)";
            // 
            // nudPercentil2
            // 
            this.nudPercentil2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudPercentil2.DecimalPlaces = 4;
            this.nudPercentil2.Location = new System.Drawing.Point(112, 388);
            this.nudPercentil2.Name = "nudPercentil2";
            this.nudPercentil2.Size = new System.Drawing.Size(85, 20);
            this.nudPercentil2.TabIndex = 3;
            this.nudPercentil2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudPercentil2.Value = new decimal(new int[] {
            85,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 390);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Percentil 2 (%)";
            // 
            // nudPercentil1
            // 
            this.nudPercentil1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudPercentil1.DecimalPlaces = 4;
            this.nudPercentil1.Location = new System.Drawing.Point(112, 362);
            this.nudPercentil1.Name = "nudPercentil1";
            this.nudPercentil1.Size = new System.Drawing.Size(85, 20);
            this.nudPercentil1.TabIndex = 1;
            this.nudPercentil1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudPercentil1.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 364);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Percentil 1 (%)";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.userControlRichTextOutput1);
            this.tabPage2.ImageIndex = 2;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1020, 556);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Resultados";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput1
            // 
            this.userControlRichTextOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput1.Location = new System.Drawing.Point(3, 3);
            this.userControlRichTextOutput1.Name = "userControlRichTextOutput1";
            this.userControlRichTextOutput1.Size = new System.Drawing.Size(1014, 550);
            this.userControlRichTextOutput1.TabIndex = 0;
            this.userControlRichTextOutput1.Texto = "";
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "application_cascade.png");
            this.imageList2.Images.SetKeyName(1, "application_form.png");
            this.imageList2.Images.SetKeyName(2, "application_lightning.png");
            this.imageList2.Images.SetKeyName(3, "page_white_wrench.png");
            this.imageList2.Images.SetKeyName(4, "application_view_columns.png");
            // 
            // btnFechar
            // 
            this.btnFechar.Location = new System.Drawing.Point(29, 0);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(75, 23);
            this.btnFechar.TabIndex = 3;
            this.btnFechar.Text = "&Fechar";
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // FormEstatisticasDescritivas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 618);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1036, 645);
            this.Name = "FormEstatisticasDescritivas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Estatísticas Descritivas";
            this.Load += new System.EventHandler(this.FormEstatisticasDescritivas_Load);
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
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumCasasDecimais)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPercentil4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPercentil3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPercentil2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPercentil1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.Button btnCalcular;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.GroupBox groupBox1;
        private UserControlSelecaoVariaveis userControlSelecaoVariaveis1;
        private UserControlSelecaoVariaveis userControlSelecaoVariaveis2;
        private UserControlSelecaoVariaveis userControlSelecaoVariaveis3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudPercentil1;
        private System.Windows.Forms.NumericUpDown nudPercentil4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudPercentil3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudPercentil2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudNumCasasDecimais;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbDadosAmostrais;
        private System.Windows.Forms.RadioButton rdbDadosPopulacionais;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput1;
    }
}