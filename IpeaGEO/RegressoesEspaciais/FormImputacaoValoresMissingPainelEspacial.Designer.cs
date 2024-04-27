namespace IpeaGeo.RegressoesEspaciais
{
    partial class FormImputacaoValoresMissingPainelEspacial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImputacaoValoresMissingPainelEspacial));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudImputacaoValorFixo = new System.Windows.Forms.NumericUpDown();
            this.rdbValorFixo = new System.Windows.Forms.RadioButton();
            this.rdbMediaGeralPeriodo = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImputacaoValorFixo)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnOK);
            this.splitContainer1.Size = new System.Drawing.Size(391, 143);
            this.splitContainer1.SplitterDistance = 103;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudImputacaoValorFixo);
            this.groupBox1.Controls.Add(this.rdbValorFixo);
            this.groupBox1.Controls.Add(this.rdbMediaGeralPeriodo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(391, 103);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // nudImputacaoValorFixo
            // 
            this.nudImputacaoValorFixo.DecimalPlaces = 6;
            this.nudImputacaoValorFixo.Enabled = false;
            this.nudImputacaoValorFixo.Location = new System.Drawing.Point(276, 53);
            this.nudImputacaoValorFixo.Maximum = new decimal(new int[] {
            -559939584,
            902409669,
            54,
            0});
            this.nudImputacaoValorFixo.Minimum = new decimal(new int[] {
            -1593835520,
            466537709,
            54210,
            -2147483648});
            this.nudImputacaoValorFixo.Name = "nudImputacaoValorFixo";
            this.nudImputacaoValorFixo.Size = new System.Drawing.Size(95, 20);
            this.nudImputacaoValorFixo.TabIndex = 5;
            this.nudImputacaoValorFixo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudImputacaoValorFixo.ThousandsSeparator = true;
            // 
            // rdbValorFixo
            // 
            this.rdbValorFixo.AutoSize = true;
            this.rdbValorFixo.Location = new System.Drawing.Point(22, 53);
            this.rdbValorFixo.Name = "rdbValorFixo";
            this.rdbValorFixo.Size = new System.Drawing.Size(236, 17);
            this.rdbValorFixo.TabIndex = 1;
            this.rdbValorFixo.Text = "Substituir os valores missing por um valor fixo";
            this.rdbValorFixo.UseVisualStyleBackColor = true;
            this.rdbValorFixo.CheckedChanged += new System.EventHandler(this.rdbValorFixo_CheckedChanged);
            // 
            // rdbMediaGeralPeriodo
            // 
            this.rdbMediaGeralPeriodo.AutoSize = true;
            this.rdbMediaGeralPeriodo.Checked = true;
            this.rdbMediaGeralPeriodo.Location = new System.Drawing.Point(22, 30);
            this.rdbMediaGeralPeriodo.Name = "rdbMediaGeralPeriodo";
            this.rdbMediaGeralPeriodo.Size = new System.Drawing.Size(291, 17);
            this.rdbMediaGeralPeriodo.TabIndex = 0;
            this.rdbMediaGeralPeriodo.TabStop = true;
            this.rdbMediaGeralPeriodo.Text = "Substituir os valores missing pela média geral no período";
            this.rdbMediaGeralPeriodo.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(296, 7);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // FormImputacaoValoresMissingPainelEspacial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 143);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImputacaoValoresMissingPainelEspacial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Imputação dos Valores Missing para Painel Espacial";
            this.Load += new System.EventHandler(this.FormImputacaoValoresMissingPainelEspacial_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImputacaoValorFixo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbValorFixo;
        private System.Windows.Forms.RadioButton rdbMediaGeralPeriodo;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.NumericUpDown nudImputacaoValorFixo;
    }
}