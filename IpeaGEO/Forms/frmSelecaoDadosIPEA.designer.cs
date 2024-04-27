namespace IpeaGeo.Forms
{
     partial class frmSelecaoDadosIPEA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelecaoDadosIPEA));
            this.spc_1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.spc_2 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnPesquisa = new System.Windows.Forms.Button();
            this.txtPesquisar = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnImportar = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.spc_1)).BeginInit();
            this.spc_1.Panel1.SuspendLayout();
            this.spc_1.Panel2.SuspendLayout();
            this.spc_1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spc_2)).BeginInit();
            this.spc_2.Panel1.SuspendLayout();
            this.spc_2.Panel2.SuspendLayout();
            this.spc_2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spc_1
            // 
            this.spc_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spc_1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spc_1.Location = new System.Drawing.Point(0, 0);
            this.spc_1.Name = "spc_1";
            this.spc_1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spc_1.Panel1
            // 
            this.spc_1.Panel1.Controls.Add(this.tabControl1);
            this.spc_1.Panel1.Controls.Add(this.btnPesquisa);
            this.spc_1.Panel1.Controls.Add(this.txtPesquisar);
            this.spc_1.Panel1.Controls.Add(this.menuStrip1);
            // 
            // spc_1.Panel2
            // 
            this.spc_1.Panel2.Controls.Add(this.btnClose);
            this.spc_1.Panel2.Controls.Add(this.btnImportar);
            this.spc_1.Panel2.Controls.Add(this.progressBar1);
            this.spc_1.Size = new System.Drawing.Size(802, 498);
            this.spc_1.SplitterDistance = 452;
            this.spc_1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(802, 428);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.spc_2);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(794, 401);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Seleção Dados";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // spc_2
            // 
            this.spc_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spc_2.Location = new System.Drawing.Point(3, 3);
            this.spc_2.Name = "spc_2";
            // 
            // spc_2.Panel1
            // 
            this.spc_2.Panel1.Controls.Add(this.treeView1);
            // 
            // spc_2.Panel2
            // 
            this.spc_2.Panel2.Controls.Add(this.richTextBox1);
            this.spc_2.Size = new System.Drawing.Size(788, 395);
            this.spc_2.SplitterDistance = 260;
            this.spc_2.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(260, 395);
            this.treeView1.TabIndex = 0;
            this.treeView1.UseWaitCursor = true;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect_1);
            this.treeView1.CursorChanged += new System.EventHandler(this.frmDados_Load);
            this.treeView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseMove);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(524, 395);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer1);
            this.tabPage2.ImageIndex = 0;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(794, 401);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Pesquisar";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox2);
            this.splitContainer1.Size = new System.Drawing.Size(788, 395);
            this.splitContainer1.SplitterDistance = 260;
            this.splitContainer1.TabIndex = 2;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.Size = new System.Drawing.Size(260, 395);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox2.Location = new System.Drawing.Point(0, 0);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(524, 395);
            this.richTextBox2.TabIndex = 0;
            this.richTextBox2.Text = "";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "preview.png");
            this.imageList1.Images.SetKeyName(1, "");
            // 
            // btnPesquisa
            // 
            this.btnPesquisa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPesquisa.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnPesquisa.Location = new System.Drawing.Point(680, 1);
            this.btnPesquisa.Name = "btnPesquisa";
            this.btnPesquisa.Size = new System.Drawing.Size(116, 23);
            this.btnPesquisa.TabIndex = 4;
            this.btnPesquisa.Text = "Pesquisar";
            this.btnPesquisa.UseVisualStyleBackColor = true;
            this.btnPesquisa.Click += new System.EventHandler(this.btnPesquisa_Click);
            // 
            // txtPesquisar
            // 
            this.txtPesquisar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPesquisar.ForeColor = System.Drawing.Color.Silver;
            this.txtPesquisar.Location = new System.Drawing.Point(267, 3);
            this.txtPesquisar.Name = "txtPesquisar";
            this.txtPesquisar.Size = new System.Drawing.Size(407, 20);
            this.txtPesquisar.TabIndex = 2;
            this.txtPesquisar.Text = "Digite sua pesquisa";
            this.txtPesquisar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtPesquisar_MouseClick);
            this.txtPesquisar.TextChanged += new System.EventHandler(this.txtPesquisar_TextChanged);
            this.txtPesquisar.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPesquisar_KeyUp);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(802, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(532, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(113, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "&Fechar\r\n";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnImportar
            // 
            this.btnImportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportar.Location = new System.Drawing.Point(665, 7);
            this.btnImportar.Name = "btnImportar";
            this.btnImportar.Size = new System.Drawing.Size(113, 23);
            this.btnImportar.TabIndex = 1;
            this.btnImportar.Text = "&Carregar";
            this.btnImportar.UseVisualStyleBackColor = true;
            this.btnImportar.Click += new System.EventHandler(this.btnImportar_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar1.Location = new System.Drawing.Point(21, 13);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(260, 12);
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Visible = false;
            // 
            // frmSelecaoDadosIPEA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 498);
            this.Controls.Add(this.spc_1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(810, 525);
            this.Name = "frmSelecaoDadosIPEA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Seleção dos Dados Disponibilizados pelo IPEA";
            this.Load += new System.EventHandler(this.frmDados_Load);
            this.spc_1.Panel1.ResumeLayout(false);
            this.spc_1.Panel1.PerformLayout();
            this.spc_1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spc_1)).EndInit();
            this.spc_1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.spc_2.Panel1.ResumeLayout(false);
            this.spc_2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spc_2)).EndInit();
            this.spc_2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spc_1;
        private System.Windows.Forms.Button btnPesquisa;
        private System.Windows.Forms.TextBox txtPesquisar;
        private System.Windows.Forms.SplitContainer spc_2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button btnImportar;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timer1;
    }
}