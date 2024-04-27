namespace IpeaGeo.Modelagem
{
    partial class FormDistribuicoesParametrics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDistribuicoesParametrics));
            IpeaGeo.RegressoesEspaciais.clsIpeaShape clsIpeaShape1 = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.userControlDataGrid1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlDataGrid();
            this.tabPage0 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.userControlSelecaoVariaveis1 = new IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis();
            this.userControlSelecaoVariaveis2 = new IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.ckbLimpaJanelaOutput = new System.Windows.Forms.CheckBox();
            this.btnExecutar = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.userControlRichTextOutput2 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblProgressBar = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this.progressBar1);
            this.splitContainer1.Panel2.Controls.Add(this.lblProgressBar);
            this.splitContainer1.Panel2.Controls.Add(this.btnOK);
            this.splitContainer1.Panel2.Controls.Add(this.btnClose);
            this.splitContainer1.Size = new System.Drawing.Size(867, 548);
            this.splitContainer1.SplitterDistance = 512;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage0);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(867, 512);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.userControlDataGrid1);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(859, 485);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tabela de Dados";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // userControlDataGrid1
            // 
            this.userControlDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDataGrid1.ListaVarsNumericas = new string[0];
            this.userControlDataGrid1.ListaVarsTotais = new string[0];
            this.userControlDataGrid1.Location = new System.Drawing.Point(3, 3);
            this.userControlDataGrid1.Name = "userControlDataGrid1";
            clsIpeaShape1.CoordenadasEmRadianos = false;
            clsIpeaShape1.Count = 0;
            clsIpeaShape1.HoraCriacao = new System.DateTime(2012, 1, 9, 14, 37, 24, 917);
            clsIpeaShape1.MatrizAllDistances = null;
            clsIpeaShape1.Nome = "";
            clsIpeaShape1.OrdemVizinhanca = 1;
            clsIpeaShape1.Poligonos = new IpeaGeo.RegressoesEspaciais.clsIpeaPoligono[0];
            clsIpeaShape1.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
            clsIpeaShape1.TipoDistancia = false;
            clsIpeaShape1.TipoVizinhanca = "";
            this.userControlDataGrid1.Shape = clsIpeaShape1;
            this.userControlDataGrid1.Size = new System.Drawing.Size(853, 479);
            this.userControlDataGrid1.TabControl = null;
            this.userControlDataGrid1.TabIndex = 0;
            this.userControlDataGrid1.UserControlPropScoreMatching = null;
            this.userControlDataGrid1.UserControlRegInstrumentos = null;
            this.userControlDataGrid1.UserControlSelecao2BlocosVariaveis = null;
            this.userControlDataGrid1.UserControlSelecaoVariaveis = null;
            // 
            // tabPage0
            // 
            this.tabPage0.Controls.Add(this.splitContainer2);
            this.tabPage0.ImageIndex = 2;
            this.tabPage0.Location = new System.Drawing.Point(4, 23);
            this.tabPage0.Name = "tabPage0";
            this.tabPage0.Size = new System.Drawing.Size(859, 485);
            this.tabPage0.TabIndex = 3;
            this.tabPage0.Text = "Especificações";
            this.tabPage0.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(859, 485);
            this.splitContainer2.SplitterDistance = 672;
            this.splitContainer2.TabIndex = 0;
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
            this.splitContainer4.Panel1.Controls.Add(this.userControlSelecaoVariaveis1);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.userControlSelecaoVariaveis2);
            this.splitContainer4.Size = new System.Drawing.Size(672, 485);
            this.splitContainer4.SplitterDistance = 223;
            this.splitContainer4.TabIndex = 0;
            // 
            // userControlSelecaoVariaveis1
            // 
            this.userControlSelecaoVariaveis1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSelecaoVariaveis1.Location = new System.Drawing.Point(0, 0);
            this.userControlSelecaoVariaveis1.Name = "userControlSelecaoVariaveis1";
            this.userControlSelecaoVariaveis1.Size = new System.Drawing.Size(672, 223);
            this.userControlSelecaoVariaveis1.TabIndex = 0;
            this.userControlSelecaoVariaveis1.VariaveisDB = null;
            this.userControlSelecaoVariaveis1.VariaveisIndependentes = new string[0];
            this.userControlSelecaoVariaveis1.VariaveisList = null;
            // 
            // userControlSelecaoVariaveis2
            // 
            this.userControlSelecaoVariaveis2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSelecaoVariaveis2.Location = new System.Drawing.Point(0, 0);
            this.userControlSelecaoVariaveis2.Name = "userControlSelecaoVariaveis2";
            this.userControlSelecaoVariaveis2.Size = new System.Drawing.Size(672, 258);
            this.userControlSelecaoVariaveis2.TabIndex = 1;
            this.userControlSelecaoVariaveis2.VariaveisDB = null;
            this.userControlSelecaoVariaveis2.VariaveisIndependentes = new string[0];
            this.userControlSelecaoVariaveis2.VariaveisList = null;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.ckbLimpaJanelaOutput);
            this.groupBox2.Controls.Add(this.btnExecutar);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 485);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Opções";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(8, 360);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(169, 17);
            this.checkBox1.TabIndex = 22;
            this.checkBox1.Text = "Limpar janela de consolidação";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // ckbLimpaJanelaOutput
            // 
            this.ckbLimpaJanelaOutput.AutoSize = true;
            this.ckbLimpaJanelaOutput.Checked = true;
            this.ckbLimpaJanelaOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbLimpaJanelaOutput.Location = new System.Drawing.Point(8, 337);
            this.ckbLimpaJanelaOutput.Name = "ckbLimpaJanelaOutput";
            this.ckbLimpaJanelaOutput.Size = new System.Drawing.Size(154, 17);
            this.ckbLimpaJanelaOutput.TabIndex = 21;
            this.ckbLimpaJanelaOutput.Text = "Limpar janela de resultados";
            this.ckbLimpaJanelaOutput.UseVisualStyleBackColor = true;
            // 
            // btnExecutar
            // 
            this.btnExecutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecutar.Location = new System.Drawing.Point(84, 442);
            this.btnExecutar.Name = "btnExecutar";
            this.btnExecutar.Size = new System.Drawing.Size(75, 23);
            this.btnExecutar.TabIndex = 5;
            this.btnExecutar.Text = "&Executar";
            this.btnExecutar.UseVisualStyleBackColor = true;
            this.btnExecutar.Click += new System.EventHandler(this.btnExecutar_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.userControlRichTextOutput1);
            this.tabPage2.ImageIndex = 11;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(859, 485);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Resultados";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput1
            // 
            this.userControlRichTextOutput1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput1.Location = new System.Drawing.Point(3, 3);
            this.userControlRichTextOutput1.Name = "userControlRichTextOutput1";
            this.userControlRichTextOutput1.Size = new System.Drawing.Size(853, 479);
            this.userControlRichTextOutput1.TabIndex = 0;
            this.userControlRichTextOutput1.Texto = "";
            // 
            // tabPage3
            // 
            this.tabPage3.ImageIndex = 4;
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(859, 485);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Gráficos";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.userControlRichTextOutput2);
            this.tabPage4.ImageIndex = 11;
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(859, 485);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "Consolidação";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // userControlRichTextOutput2
            // 
            this.userControlRichTextOutput2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextOutput2.Location = new System.Drawing.Point(0, 0);
            this.userControlRichTextOutput2.Name = "userControlRichTextOutput2";
            this.userControlRichTextOutput2.Size = new System.Drawing.Size(859, 485);
            this.userControlRichTextOutput2.TabIndex = 1;
            this.userControlRichTextOutput2.Texto = "";
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
            this.imageList1.Images.SetKeyName(10, "application_view_detail.png");
            this.imageList1.Images.SetKeyName(11, "report.png");
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(412, 9);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(296, 10);
            this.progressBar1.TabIndex = 10;
            this.progressBar1.Visible = false;
            // 
            // lblProgressBar
            // 
            this.lblProgressBar.AutoSize = true;
            this.lblProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgressBar.ForeColor = System.Drawing.Color.Black;
            this.lblProgressBar.Location = new System.Drawing.Point(136, 7);
            this.lblProgressBar.Name = "lblProgressBar";
            this.lblProgressBar.Size = new System.Drawing.Size(197, 13);
            this.lblProgressBar.TabIndex = 9;
            this.lblProgressBar.Text = "Rotinas para análise de dados espaciais";
            this.lblProgressBar.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(764, 1);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(28, 1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Fechar";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormDistribuicoesParametrics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 548);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(860, 560);
            this.Name = "FormDistribuicoesParametrics";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ajuste de Distribuições Paramétricas";
            this.Load += new System.EventHandler(this.FormBaseModelagem_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage0.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tabPage0;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblProgressBar;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis userControlSelecaoVariaveis1;
        private IpeaGeo.RegressoesEspaciais.UserControlSelecaoVariaveis userControlSelecaoVariaveis2;
        private System.Windows.Forms.Button btnExecutar;
        private System.Windows.Forms.TabPage tabPage4;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlRichTextOutput userControlRichTextOutput2;
        private RegressoesEspaciais.UserControls.UserControlDataGrid userControlDataGrid1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox ckbLimpaJanelaOutput;
    }
}