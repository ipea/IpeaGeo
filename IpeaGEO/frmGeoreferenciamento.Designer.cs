namespace IpeaGEO
{
    partial class frmGeoreferenciamento
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
            this.chkCoordenadas = new System.Windows.Forms.CheckBox();
            this.chkBounding = new System.Windows.Forms.CheckBox();
            this.chkCentroid = new System.Windows.Forms.CheckBox();
            this.btnExporta = new System.Windows.Forms.Button();
            this.btnCancela = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.dataSet1 = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.dataTable2 = new System.Data.DataTable();
            this.dataTable3 = new System.Data.DataTable();
            this.dataTable4 = new System.Data.DataTable();
            this.cmbID = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ckbEstruturaVizinhanca = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdbRook = new System.Windows.Forms.RadioButton();
            this.rdbQueen = new System.Windows.Forms.RadioButton();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable4)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.ckbEstruturaVizinhanca);
            this.groupBox1.Controls.Add(this.chkCoordenadas);
            this.groupBox1.Controls.Add(this.chkBounding);
            this.groupBox1.Controls.Add(this.chkCentroid);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 127);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Opções de georeferenciamento";
            // 
            // chkCoordenadas
            // 
            this.chkCoordenadas.AutoSize = true;
            this.chkCoordenadas.Location = new System.Drawing.Point(18, 76);
            this.chkCoordenadas.Name = "chkCoordenadas";
            this.chkCoordenadas.Size = new System.Drawing.Size(89, 17);
            this.chkCoordenadas.TabIndex = 2;
            this.chkCoordenadas.Text = "Coordenadas";
            this.chkCoordenadas.UseVisualStyleBackColor = true;
            // 
            // chkBounding
            // 
            this.chkBounding.AutoSize = true;
            this.chkBounding.Location = new System.Drawing.Point(18, 53);
            this.chkBounding.Name = "chkBounding";
            this.chkBounding.Size = new System.Drawing.Size(92, 17);
            this.chkBounding.TabIndex = 1;
            this.chkBounding.Text = "Bounding Box";
            this.chkBounding.UseVisualStyleBackColor = true;
            // 
            // chkCentroid
            // 
            this.chkCentroid.AutoSize = true;
            this.chkCentroid.Location = new System.Drawing.Point(18, 30);
            this.chkCentroid.Name = "chkCentroid";
            this.chkCentroid.Size = new System.Drawing.Size(65, 17);
            this.chkCentroid.TabIndex = 0;
            this.chkCentroid.Text = "Centroid";
            this.chkCentroid.UseVisualStyleBackColor = true;
            // 
            // btnExporta
            // 
            this.btnExporta.Location = new System.Drawing.Point(13, 221);
            this.btnExporta.Name = "btnExporta";
            this.btnExporta.Size = new System.Drawing.Size(75, 23);
            this.btnExporta.TabIndex = 1;
            this.btnExporta.Text = "Exporta";
            this.btnExporta.UseVisualStyleBackColor = true;
            this.btnExporta.Click += new System.EventHandler(this.btnExporta_Click);
            // 
            // btnCancela
            // 
            this.btnCancela.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancela.Location = new System.Drawing.Point(302, 221);
            this.btnCancela.Name = "btnCancela";
            this.btnCancela.Size = new System.Drawing.Size(75, 23);
            this.btnCancela.TabIndex = 2;
            this.btnCancela.Text = "Cancela";
            this.btnCancela.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 192);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(365, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            this.dataSet1.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable1,
            this.dataTable2,
            this.dataTable3,
            this.dataTable4});
            // 
            // dataTable1
            // 
            this.dataTable1.TableName = "tblCentroid";
            // 
            // dataTable2
            // 
            this.dataTable2.TableName = "tblBounding";
            // 
            // dataTable3
            // 
            this.dataTable3.TableName = "tblCoordenadas";
            // 
            // dataTable4
            // 
            this.dataTable4.TableName = "tblVizinhanca";
            // 
            // cmbID
            // 
            this.cmbID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbID.FormattingEnabled = true;
            this.cmbID.Location = new System.Drawing.Point(12, 165);
            this.cmbID.Name = "cmbID";
            this.cmbID.Size = new System.Drawing.Size(365, 21);
            this.cmbID.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Variável identificadora";
            // 
            // ckbEstruturaVizinhanca
            // 
            this.ckbEstruturaVizinhanca.AutoSize = true;
            this.ckbEstruturaVizinhanca.Location = new System.Drawing.Point(18, 99);
            this.ckbEstruturaVizinhanca.Name = "ckbEstruturaVizinhanca";
            this.ckbEstruturaVizinhanca.Size = new System.Drawing.Size(123, 17);
            this.ckbEstruturaVizinhanca.TabIndex = 5;
            this.ckbEstruturaVizinhanca.Text = "Matriz de vizinhança";
            this.ckbEstruturaVizinhanca.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.numericUpDown1);
            this.groupBox2.Controls.Add(this.rdbRook);
            this.groupBox2.Controls.Add(this.rdbQueen);
            this.groupBox2.Location = new System.Drawing.Point(160, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 97);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Definição de vizinhos";
            // 
            // rdbRook
            // 
            this.rdbRook.AutoSize = true;
            this.rdbRook.Location = new System.Drawing.Point(17, 42);
            this.rdbRook.Name = "rdbRook";
            this.rdbRook.Size = new System.Drawing.Size(169, 17);
            this.rdbRook.TabIndex = 1;
            this.rdbRook.Text = "Rook       (um lado em comum)";
            this.rdbRook.UseVisualStyleBackColor = true;
            // 
            // rdbQueen
            // 
            this.rdbQueen.AutoSize = true;
            this.rdbQueen.Checked = true;
            this.rdbQueen.Location = new System.Drawing.Point(17, 19);
            this.rdbQueen.Name = "rdbQueen";
            this.rdbQueen.Size = new System.Drawing.Size(169, 17);
            this.rdbQueen.TabIndex = 0;
            this.rdbQueen.TabStop = true;
            this.rdbQueen.Text = "Queen (um vértice em comum)";
            this.rdbQueen.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(58, 65);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(128, 20);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ordem";
            // 
            // frmGeoreferenciamento
            // 
            this.AcceptButton = this.btnExporta;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancela;
            this.ClientSize = new System.Drawing.Size(388, 248);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbID);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnCancela);
            this.Controls.Add(this.btnExporta);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGeoreferenciamento";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Opções de georeferenciamento";
            this.Load += new System.EventHandler(this.frmGeoreferenciamento_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable4)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkCoordenadas;
        private System.Windows.Forms.CheckBox chkBounding;
        private System.Windows.Forms.CheckBox chkCentroid;
        private System.Windows.Forms.Button btnExporta;
        private System.Windows.Forms.Button btnCancela;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Data.DataSet dataSet1;
        private System.Data.DataTable dataTable1;
        private System.Data.DataTable dataTable2;
        private System.Data.DataTable dataTable3;
        private System.Data.DataTable dataTable4;
        private System.Windows.Forms.ComboBox cmbID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ckbEstruturaVizinhanca;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbRook;
        private System.Windows.Forms.RadioButton rdbQueen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}