namespace IpeaGeo
{
    partial class FormAberturaTabelaDados
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAberturaTabelaDados));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdbArquivosSAS = new System.Windows.Forms.RadioButton();
            this.rdbArquivosTXT = new System.Windows.Forms.RadioButton();
            this.rdbArquivosExcelMDB = new System.Windows.Forms.RadioButton();
            this.grbArquivosExcelMDB = new System.Windows.Forms.GroupBox();
            this.cmbTabelasNoArquivo = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.grbArquivosTXT = new System.Windows.Forms.GroupBox();
            this.ckbDelimitadoPontoVirgula = new System.Windows.Forms.CheckBox();
            this.ckbNomesPrimeiraLinha = new System.Windows.Forms.CheckBox();
            this.ckbFormatoNumeroPortugues = new System.Windows.Forms.CheckBox();
            this.ckbDelimitadoTab = new System.Windows.Forms.CheckBox();
            this.ckbDelimitadoCaracter = new System.Windows.Forms.CheckBox();
            this.txtCaracterDelimitacao = new System.Windows.Forms.TextBox();
            this.ckbDelimitadoVirgula = new System.Windows.Forms.CheckBox();
            this.btnImportarTabela = new System.Windows.Forms.Button();
            this.lblArquivoDeDados = new System.Windows.Forms.Label();
            this.btnArquivoDados = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.grbArquivosExcelMDB.SuspendLayout();
            this.grbArquivosTXT.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.grbArquivosExcelMDB);
            this.groupBox1.Controls.Add(this.grbArquivosTXT);
            this.groupBox1.Controls.Add(this.btnImportarTabela);
            this.groupBox1.Controls.Add(this.lblArquivoDeDados);
            this.groupBox1.Controls.Add(this.btnArquivoDados);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(561, 485);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdbArquivosSAS);
            this.groupBox4.Controls.Add(this.rdbArquivosTXT);
            this.groupBox4.Controls.Add(this.rdbArquivosExcelMDB);
            this.groupBox4.Location = new System.Drawing.Point(26, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(509, 113);
            this.groupBox4.TabIndex = 35;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Tipo de arquivo a ser importado";
            // 
            // rdbArquivosSAS
            // 
            this.rdbArquivosSAS.AutoSize = true;
            this.rdbArquivosSAS.Location = new System.Drawing.Point(17, 75);
            this.rdbArquivosSAS.Name = "rdbArquivosSAS";
            this.rdbArquivosSAS.Size = new System.Drawing.Size(152, 17);
            this.rdbArquivosSAS.TabIndex = 2;
            this.rdbArquivosSAS.Text = "Arquivos do programa SAS";
            this.rdbArquivosSAS.UseVisualStyleBackColor = true;
            this.rdbArquivosSAS.CheckedChanged += new System.EventHandler(this.rdbArquivosSAS_CheckedChanged);
            // 
            // rdbArquivosTXT
            // 
            this.rdbArquivosTXT.AutoSize = true;
            this.rdbArquivosTXT.Location = new System.Drawing.Point(17, 52);
            this.rdbArquivosTXT.Name = "rdbArquivosTXT";
            this.rdbArquivosTXT.Size = new System.Drawing.Size(224, 17);
            this.rdbArquivosTXT.TabIndex = 1;
            this.rdbArquivosTXT.Text = "Arquivos do tipo texto (CSV, TXT ou DAT)";
            this.rdbArquivosTXT.UseVisualStyleBackColor = true;
            this.rdbArquivosTXT.CheckedChanged += new System.EventHandler(this.rdbArquivosTXT_CheckedChanged);
            // 
            // rdbArquivosExcelMDB
            // 
            this.rdbArquivosExcelMDB.AutoSize = true;
            this.rdbArquivosExcelMDB.Checked = true;
            this.rdbArquivosExcelMDB.Location = new System.Drawing.Point(17, 29);
            this.rdbArquivosExcelMDB.Name = "rdbArquivosExcelMDB";
            this.rdbArquivosExcelMDB.Size = new System.Drawing.Size(137, 17);
            this.rdbArquivosExcelMDB.TabIndex = 0;
            this.rdbArquivosExcelMDB.TabStop = true;
            this.rdbArquivosExcelMDB.Text = "Arquivos Excel ou MDB";
            this.rdbArquivosExcelMDB.UseVisualStyleBackColor = true;
            this.rdbArquivosExcelMDB.CheckedChanged += new System.EventHandler(this.rdbArquivosExcelMDB_CheckedChanged);
            // 
            // grbArquivosExcelMDB
            // 
            this.grbArquivosExcelMDB.Controls.Add(this.cmbTabelasNoArquivo);
            this.grbArquivosExcelMDB.Controls.Add(this.label12);
            this.grbArquivosExcelMDB.Location = new System.Drawing.Point(26, 243);
            this.grbArquivosExcelMDB.Name = "grbArquivosExcelMDB";
            this.grbArquivosExcelMDB.Size = new System.Drawing.Size(509, 63);
            this.grbArquivosExcelMDB.TabIndex = 34;
            this.grbArquivosExcelMDB.TabStop = false;
            this.grbArquivosExcelMDB.Text = "Importação de arquivos Excel ou MDB";
            // 
            // cmbTabelasNoArquivo
            // 
            this.cmbTabelasNoArquivo.FormattingEnabled = true;
            this.cmbTabelasNoArquivo.Location = new System.Drawing.Point(113, 24);
            this.cmbTabelasNoArquivo.Name = "cmbTabelasNoArquivo";
            this.cmbTabelasNoArquivo.Size = new System.Drawing.Size(373, 21);
            this.cmbTabelasNoArquivo.TabIndex = 19;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(93, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Tabela no arquivo";
            // 
            // grbArquivosTXT
            // 
            this.grbArquivosTXT.Controls.Add(this.ckbDelimitadoPontoVirgula);
            this.grbArquivosTXT.Controls.Add(this.ckbNomesPrimeiraLinha);
            this.grbArquivosTXT.Controls.Add(this.ckbFormatoNumeroPortugues);
            this.grbArquivosTXT.Controls.Add(this.ckbDelimitadoTab);
            this.grbArquivosTXT.Controls.Add(this.ckbDelimitadoCaracter);
            this.grbArquivosTXT.Controls.Add(this.txtCaracterDelimitacao);
            this.grbArquivosTXT.Controls.Add(this.ckbDelimitadoVirgula);
            this.grbArquivosTXT.Location = new System.Drawing.Point(26, 312);
            this.grbArquivosTXT.Name = "grbArquivosTXT";
            this.grbArquivosTXT.Size = new System.Drawing.Size(509, 113);
            this.grbArquivosTXT.TabIndex = 33;
            this.grbArquivosTXT.TabStop = false;
            this.grbArquivosTXT.Text = "Importação de arquivos do tipo texto (CSV, TXT ou DAT)";
            // 
            // ckbDelimitadoPontoVirgula
            // 
            this.ckbDelimitadoPontoVirgula.AutoSize = true;
            this.ckbDelimitadoPontoVirgula.Location = new System.Drawing.Point(17, 55);
            this.ckbDelimitadoPontoVirgula.Name = "ckbDelimitadoPontoVirgula";
            this.ckbDelimitadoPontoVirgula.Size = new System.Drawing.Size(190, 17);
            this.ckbDelimitadoPontoVirgula.TabIndex = 25;
            this.ckbDelimitadoPontoVirgula.Text = "Delimitado por ponto e vírgula (\";\")";
            this.ckbDelimitadoPontoVirgula.UseVisualStyleBackColor = true;
            this.ckbDelimitadoPontoVirgula.CheckedChanged += new System.EventHandler(this.ckbDelimitadoPontoVirgula_CheckedChanged);
            // 
            // ckbNomesPrimeiraLinha
            // 
            this.ckbNomesPrimeiraLinha.AutoSize = true;
            this.ckbNomesPrimeiraLinha.Checked = true;
            this.ckbNomesPrimeiraLinha.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbNomesPrimeiraLinha.Location = new System.Drawing.Point(269, 79);
            this.ckbNomesPrimeiraLinha.Name = "ckbNomesPrimeiraLinha";
            this.ckbNomesPrimeiraLinha.Size = new System.Drawing.Size(203, 17);
            this.ckbNomesPrimeiraLinha.TabIndex = 27;
            this.ckbNomesPrimeiraLinha.Text = "Nomes das variáveis na primeira linha";
            this.ckbNomesPrimeiraLinha.UseVisualStyleBackColor = true;
            // 
            // ckbFormatoNumeroPortugues
            // 
            this.ckbFormatoNumeroPortugues.AutoSize = true;
            this.ckbFormatoNumeroPortugues.Location = new System.Drawing.Point(17, 79);
            this.ckbFormatoNumeroPortugues.Name = "ckbFormatoNumeroPortugues";
            this.ckbFormatoNumeroPortugues.Size = new System.Drawing.Size(189, 17);
            this.ckbFormatoNumeroPortugues.TabIndex = 32;
            this.ckbFormatoNumeroPortugues.Text = "Formato de números em português";
            this.ckbFormatoNumeroPortugues.UseVisualStyleBackColor = true;
            // 
            // ckbDelimitadoTab
            // 
            this.ckbDelimitadoTab.AutoSize = true;
            this.ckbDelimitadoTab.Location = new System.Drawing.Point(17, 29);
            this.ckbDelimitadoTab.Name = "ckbDelimitadoTab";
            this.ckbDelimitadoTab.Size = new System.Drawing.Size(111, 17);
            this.ckbDelimitadoTab.TabIndex = 22;
            this.ckbDelimitadoTab.Text = "Delimitado por tab";
            this.ckbDelimitadoTab.UseVisualStyleBackColor = true;
            this.ckbDelimitadoTab.CheckedChanged += new System.EventHandler(this.ckbDelimitadoTab_CheckedChanged);
            // 
            // ckbDelimitadoCaracter
            // 
            this.ckbDelimitadoCaracter.AutoSize = true;
            this.ckbDelimitadoCaracter.Location = new System.Drawing.Point(269, 55);
            this.ckbDelimitadoCaracter.Name = "ckbDelimitadoCaracter";
            this.ckbDelimitadoCaracter.Size = new System.Drawing.Size(135, 17);
            this.ckbDelimitadoCaracter.TabIndex = 23;
            this.ckbDelimitadoCaracter.Text = "Delimitado por caracter";
            this.ckbDelimitadoCaracter.UseVisualStyleBackColor = true;
            this.ckbDelimitadoCaracter.CheckedChanged += new System.EventHandler(this.ckbDelimitadoCaracter_CheckedChanged);
            // 
            // txtCaracterDelimitacao
            // 
            this.txtCaracterDelimitacao.Enabled = false;
            this.txtCaracterDelimitacao.Location = new System.Drawing.Point(410, 53);
            this.txtCaracterDelimitacao.Name = "txtCaracterDelimitacao";
            this.txtCaracterDelimitacao.Size = new System.Drawing.Size(39, 20);
            this.txtCaracterDelimitacao.TabIndex = 26;
            // 
            // ckbDelimitadoVirgula
            // 
            this.ckbDelimitadoVirgula.AutoSize = true;
            this.ckbDelimitadoVirgula.Location = new System.Drawing.Point(269, 29);
            this.ckbDelimitadoVirgula.Name = "ckbDelimitadoVirgula";
            this.ckbDelimitadoVirgula.Size = new System.Drawing.Size(151, 17);
            this.ckbDelimitadoVirgula.TabIndex = 24;
            this.ckbDelimitadoVirgula.Text = "Delimitado por vírgula (\",\")";
            this.ckbDelimitadoVirgula.UseVisualStyleBackColor = true;
            this.ckbDelimitadoVirgula.CheckedChanged += new System.EventHandler(this.ckbDelimitadoVirgula_CheckedChanged);
            // 
            // btnImportarTabela
            // 
            this.btnImportarTabela.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportarTabela.Enabled = false;
            this.btnImportarTabela.Location = new System.Drawing.Point(435, 438);
            this.btnImportarTabela.Name = "btnImportarTabela";
            this.btnImportarTabela.Size = new System.Drawing.Size(100, 23);
            this.btnImportarTabela.TabIndex = 21;
            this.btnImportarTabela.Text = "&Importar tabela";
            this.btnImportarTabela.UseVisualStyleBackColor = true;
            this.btnImportarTabela.Click += new System.EventHandler(this.btnImportarTabela_Click);
            // 
            // lblArquivoDeDados
            // 
            this.lblArquivoDeDados.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblArquivoDeDados.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArquivoDeDados.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblArquivoDeDados.Location = new System.Drawing.Point(26, 175);
            this.lblArquivoDeDados.Name = "lblArquivoDeDados";
            this.lblArquivoDeDados.Size = new System.Drawing.Size(509, 54);
            this.lblArquivoDeDados.TabIndex = 6;
            this.lblArquivoDeDados.Text = "Escolha o arquivo de dados";
            this.lblArquivoDeDados.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnArquivoDados
            // 
            this.btnArquivoDados.Location = new System.Drawing.Point(26, 142);
            this.btnArquivoDados.Name = "btnArquivoDados";
            this.btnArquivoDados.Size = new System.Drawing.Size(100, 23);
            this.btnArquivoDados.TabIndex = 5;
            this.btnArquivoDados.Text = "&Arquivo";
            this.btnArquivoDados.UseVisualStyleBackColor = true;
            this.btnArquivoDados.Click += new System.EventHandler(this.btnArquivoDados_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(26, 438);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Cancelar";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormAberturaTabelaDados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 485);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormAberturaTabelaDados";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Importação da Tabela de Dados";
            this.Load += new System.EventHandler(this.FormAberturaTabelaDados_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.grbArquivosExcelMDB.ResumeLayout(false);
            this.grbArquivosExcelMDB.PerformLayout();
            this.grbArquivosTXT.ResumeLayout(false);
            this.grbArquivosTXT.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblArquivoDeDados;
        private System.Windows.Forms.Button btnArquivoDados;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cmbTabelasNoArquivo;
        private System.Windows.Forms.Button btnImportarTabela;
        private System.Windows.Forms.CheckBox ckbDelimitadoCaracter;
        private System.Windows.Forms.CheckBox ckbDelimitadoTab;
        private System.Windows.Forms.CheckBox ckbDelimitadoPontoVirgula;
        private System.Windows.Forms.CheckBox ckbDelimitadoVirgula;
        private System.Windows.Forms.TextBox txtCaracterDelimitacao;
        private System.Windows.Forms.CheckBox ckbNomesPrimeiraLinha;
        private System.Windows.Forms.CheckBox ckbFormatoNumeroPortugues;
        private System.Windows.Forms.GroupBox grbArquivosTXT;
        private System.Windows.Forms.GroupBox grbArquivosExcelMDB;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rdbArquivosSAS;
        private System.Windows.Forms.RadioButton rdbArquivosTXT;
        private System.Windows.Forms.RadioButton rdbArquivosExcelMDB;
    }
}