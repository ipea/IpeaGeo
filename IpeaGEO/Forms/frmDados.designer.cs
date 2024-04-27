namespace IpeaGeo.Forms
{
    partial class frmDados
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
            this.spc_1 = new System.Windows.Forms.SplitContainer();
            this.btnPesquisa = new System.Windows.Forms.Button();
            this.cmbListaTemas = new System.Windows.Forms.ComboBox();
            this.txtPesquisar = new System.Windows.Forms.TextBox();
            this.spc_2 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnImportar = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.spc_1)).BeginInit();
            this.spc_1.Panel1.SuspendLayout();
            this.spc_1.Panel2.SuspendLayout();
            this.spc_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spc_2)).BeginInit();
            this.spc_2.Panel1.SuspendLayout();
            this.spc_2.Panel2.SuspendLayout();
            this.spc_2.SuspendLayout();
            this.SuspendLayout();
            // 
            // spc_1
            // 
            this.spc_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spc_1.Location = new System.Drawing.Point(0, 0);
            this.spc_1.Name = "spc_1";
            this.spc_1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spc_1.Panel1
            // 
            this.spc_1.Panel1.Controls.Add(this.btnPesquisa);
            this.spc_1.Panel1.Controls.Add(this.cmbListaTemas);
            this.spc_1.Panel1.Controls.Add(this.txtPesquisar);
            this.spc_1.Panel1.Controls.Add(this.spc_2);
            this.spc_1.Panel1.Controls.Add(this.menuStrip1);
            // 
            // spc_1.Panel2
            // 
            this.spc_1.Panel2.Controls.Add(this.btnClose);
            this.spc_1.Panel2.Controls.Add(this.btnImportar);
            this.spc_1.Panel2.Controls.Add(this.progressBar1);
            this.spc_1.Size = new System.Drawing.Size(794, 487);
            this.spc_1.SplitterDistance = 419;
            this.spc_1.TabIndex = 0;
            // 
            // btnPesquisa
            // 
            this.btnPesquisa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPesquisa.Location = new System.Drawing.Point(672, 1);
            this.btnPesquisa.Name = "btnPesquisa";
            this.btnPesquisa.Size = new System.Drawing.Size(116, 23);
            this.btnPesquisa.TabIndex = 4;
            this.btnPesquisa.Text = "Pesquisar";
            this.btnPesquisa.UseVisualStyleBackColor = true;
            this.btnPesquisa.Click += new System.EventHandler(this.btnPesquisa_Click);
            // 
            // cmbListaTemas
            // 
            this.cmbListaTemas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbListaTemas.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbListaTemas.ForeColor = System.Drawing.SystemColors.MenuText;
            this.cmbListaTemas.FormattingEnabled = true;
            this.cmbListaTemas.Location = new System.Drawing.Point(555, 3);
            this.cmbListaTemas.Name = "cmbListaTemas";
            this.cmbListaTemas.Size = new System.Drawing.Size(112, 21);
            this.cmbListaTemas.TabIndex = 3;
            this.cmbListaTemas.Tag = "";
            this.cmbListaTemas.Click += new System.EventHandler(this.cmbListaTemas_Click);
            // 
            // txtPesquisar
            // 
            this.txtPesquisar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPesquisar.ForeColor = System.Drawing.Color.Silver;
            this.txtPesquisar.Location = new System.Drawing.Point(267, 3);
            this.txtPesquisar.Name = "txtPesquisar";
            this.txtPesquisar.Size = new System.Drawing.Size(282, 20);
            this.txtPesquisar.TabIndex = 2;
            this.txtPesquisar.Text = "Digite sua pesquisa";
            this.txtPesquisar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtPesquisar_MouseClick);
            // 
            // spc_2
            // 
            this.spc_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spc_2.Location = new System.Drawing.Point(0, 24);
            this.spc_2.Name = "spc_2";
            // 
            // spc_2.Panel1
            // 
            this.spc_2.Panel1.Controls.Add(this.treeView1);
            // 
            // spc_2.Panel2
            // 
            this.spc_2.Panel2.Controls.Add(this.checkedListBox1);
            this.spc_2.Size = new System.Drawing.Size(794, 395);
            this.spc_2.SplitterDistance = 263;
            this.spc_2.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(263, 395);
            this.treeView1.TabIndex = 0;
            this.treeView1.UseWaitCursor = true;
            this.treeView1.CursorChanged += new System.EventHandler(this.frmDados_Load);
            this.treeView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseMove);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(0, 0);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(527, 394);
            this.checkedListBox1.TabIndex = 0;
            //this.checkedListBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.checkedListBox1_MouseMove);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(794, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            //this.menuStrip1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.menuStrip1_MouseMove);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(542, 28);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(113, 33);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Fechar\r\n";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnImportar
            // 
            this.btnImportar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportar.Location = new System.Drawing.Point(672, 29);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(113, 33);
            this.btnImportar.TabIndex = 1;
            this.btnImportar.Text = "Importar";
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar1.Location = new System.Drawing.Point(3, 37);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(260, 15);
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Visible = false;
            // 
            // frmDados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 487);
            this.Controls.Add(this.spc_1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmDados";
            this.Text = "Teste Dados";
            this.Load += new System.EventHandler(this.frmDados_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmDados_KeyUp);
            //this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmDados_MouseMove);
            this.spc_1.Panel1.ResumeLayout(false);
            this.spc_1.Panel1.PerformLayout();
            this.spc_1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spc_1)).EndInit();
            this.spc_1.ResumeLayout(false);
            this.spc_2.Panel1.ResumeLayout(false);
            this.spc_2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spc_2)).EndInit();
            this.spc_2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spc_1;
        private System.Windows.Forms.Button btnPesquisa;
        private System.Windows.Forms.TextBox txtPesquisar;
        private System.Windows.Forms.SplitContainer spc_2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cmbListaTemas;
    }
}