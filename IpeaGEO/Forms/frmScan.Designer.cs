namespace IpeaGeo
{
    partial class frmScan
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBase = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbEvento = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.numGrid = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbCores = new System.Windows.Forms.ComboBox();
            this.numRaioMin = new System.Windows.Forms.NumericUpDown();
            this.numRaioMax = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numSimulacoes = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numProporcao = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numCluster = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancela = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.chkRelatorio = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRaioMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRaioMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSimulacoes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numProporcao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCluster)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbBase);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbEvento);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 85);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Variáveis";
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
            this.cmbBase.Location = new System.Drawing.Point(54, 44);
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
            this.cmbEvento.Location = new System.Drawing.Point(54, 17);
            this.cmbEvento.Name = "cmbEvento";
            this.cmbEvento.Size = new System.Drawing.Size(332, 21);
            this.cmbEvento.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.numGrid);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.cmbCores);
            this.groupBox2.Controls.Add(this.numRaioMin);
            this.groupBox2.Controls.Add(this.numRaioMax);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.numSimulacoes);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numProporcao);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.numCluster);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(13, 103);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(402, 183);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Parâmetros";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(116, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Número de pontos Grid";
            // 
            // numGrid
            // 
            this.numGrid.Location = new System.Drawing.Point(146, 102);
            this.numGrid.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numGrid.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numGrid.Name = "numGrid";
            this.numGrid.Size = new System.Drawing.Size(239, 20);
            this.numGrid.TabIndex = 16;
            this.numGrid.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 157);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Cores";
            // 
            // cmbCores
            // 
            this.cmbCores.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbCores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCores.FormattingEnabled = true;
            this.cmbCores.Location = new System.Drawing.Point(146, 154);
            this.cmbCores.Name = "cmbCores";
            this.cmbCores.Size = new System.Drawing.Size(239, 21);
            this.cmbCores.TabIndex = 14;
            this.cmbCores.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cmbCores_DrawItem_1);
            // 
            // numRaioMin
            // 
            this.numRaioMin.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numRaioMin.Location = new System.Drawing.Point(146, 47);
            this.numRaioMin.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numRaioMin.Name = "numRaioMin";
            this.numRaioMin.Size = new System.Drawing.Size(62, 20);
            this.numRaioMin.TabIndex = 13;
            // 
            // numRaioMax
            // 
            this.numRaioMax.DecimalPlaces = 2;
            this.numRaioMax.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numRaioMax.Location = new System.Drawing.Point(324, 47);
            this.numRaioMax.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numRaioMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRaioMax.Name = "numRaioMax";
            this.numRaioMax.Size = new System.Drawing.Size(61, 20);
            this.numRaioMax.TabIndex = 12;
            this.numRaioMax.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(136, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Simulações de Monte Carlo";
            // 
            // numSimulacoes
            // 
            this.numSimulacoes.Location = new System.Drawing.Point(146, 128);
            this.numSimulacoes.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numSimulacoes.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSimulacoes.Name = "numSimulacoes";
            this.numSimulacoes.Size = new System.Drawing.Size(239, 20);
            this.numSimulacoes.TabIndex = 9;
            this.numSimulacoes.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Proporção da base (Max)";
            // 
            // numProporcao
            // 
            this.numProporcao.DecimalPlaces = 2;
            this.numProporcao.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numProporcao.Location = new System.Drawing.Point(146, 76);
            this.numProporcao.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numProporcao.Name = "numProporcao";
            this.numProporcao.Size = new System.Drawing.Size(239, 20);
            this.numProporcao.TabIndex = 7;
            this.numProporcao.Value = new decimal(new int[] {
            5000,
            0,
            0,
            131072});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(227, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Raio máximo (Km)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Raio mínimo (Km)";
            // 
            // numCluster
            // 
            this.numCluster.Location = new System.Drawing.Point(146, 20);
            this.numCluster.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCluster.Name = "numCluster";
            this.numCluster.Size = new System.Drawing.Size(239, 20);
            this.numCluster.TabIndex = 1;
            this.numCluster.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Número de conglomerados";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(258, 327);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancela
            // 
            this.btnCancela.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancela.Location = new System.Drawing.Point(339, 327);
            this.btnCancela.Name = "btnCancela";
            this.btnCancela.Size = new System.Drawing.Size(75, 23);
            this.btnCancela.TabIndex = 3;
            this.btnCancela.Text = "Cancelar";
            this.btnCancela.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 327);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(240, 23);
            this.progressBar1.TabIndex = 4;
            this.progressBar1.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(11, 311);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(35, 13);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "label9";
            // 
            // chkRelatorio
            // 
            this.chkRelatorio.AutoSize = true;
            this.chkRelatorio.Checked = true;
            this.chkRelatorio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRelatorio.Location = new System.Drawing.Point(15, 292);
            this.chkRelatorio.Name = "chkRelatorio";
            this.chkRelatorio.Size = new System.Drawing.Size(92, 17);
            this.chkRelatorio.TabIndex = 6;
            this.chkRelatorio.Text = "Gerar relatório";
            this.chkRelatorio.UseVisualStyleBackColor = true;
            // 
            // frmScan
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancela;
            this.ClientSize = new System.Drawing.Size(427, 359);
            this.Controls.Add(this.chkRelatorio);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnCancela);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmScan";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Estatística Scan";
            this.Load += new System.EventHandler(this.frmScan_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRaioMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRaioMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSimulacoes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numProporcao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCluster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbEvento;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numCluster;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numProporcao;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numSimulacoes;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancela;
        private System.Windows.Forms.NumericUpDown numRaioMin;
        private System.Windows.Forms.NumericUpDown numRaioMax;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbCores;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numGrid;
        private System.Windows.Forms.CheckBox chkRelatorio;
    }
}