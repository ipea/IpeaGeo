namespace IpeaGeo.RegressoesEspaciais.UserControls
{
    partial class UserControlRichTextOutput
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.richTextBoxResultadosEstimacao = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.salvarTXTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarParaTXTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarSeleçãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tamanhoDaFonteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxResultadosEstimacao
            // 
            this.richTextBoxResultadosEstimacao.ContextMenuStrip = this.contextMenuStrip2;
            this.richTextBoxResultadosEstimacao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxResultadosEstimacao.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxResultadosEstimacao.Location = new System.Drawing.Point(0, 0);
            this.richTextBoxResultadosEstimacao.Name = "richTextBoxResultadosEstimacao";
            this.richTextBoxResultadosEstimacao.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.richTextBoxResultadosEstimacao.Size = new System.Drawing.Size(823, 535);
            this.richTextBoxResultadosEstimacao.TabIndex = 1;
            this.richTextBoxResultadosEstimacao.Text = "";
            this.richTextBoxResultadosEstimacao.WordWrap = false;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salvarTXTToolStripMenuItem,
            this.exportarParaTXTToolStripMenuItem,
            this.exportarSeleçãoToolStripMenuItem,
            this.tamanhoDaFonteToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(194, 114);
            // 
            // salvarTXTToolStripMenuItem
            // 
            this.salvarTXTToolStripMenuItem.Name = "salvarTXTToolStripMenuItem";
            this.salvarTXTToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.salvarTXTToolStripMenuItem.Text = "Selecionar tudo";
            this.salvarTXTToolStripMenuItem.Click += new System.EventHandler(this.salvarTXTToolStripMenuItem_Click);
            // 
            // exportarParaTXTToolStripMenuItem
            // 
            this.exportarParaTXTToolStripMenuItem.Name = "exportarParaTXTToolStripMenuItem";
            this.exportarParaTXTToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.exportarParaTXTToolStripMenuItem.Text = "Exportar toda a janela";
            this.exportarParaTXTToolStripMenuItem.Click += new System.EventHandler(this.exportarParaTXTToolStripMenuItem_Click);
            // 
            // exportarSeleçãoToolStripMenuItem
            // 
            this.exportarSeleçãoToolStripMenuItem.Name = "exportarSeleçãoToolStripMenuItem";
            this.exportarSeleçãoToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.exportarSeleçãoToolStripMenuItem.Text = "Exportar seleção";
            this.exportarSeleçãoToolStripMenuItem.Click += new System.EventHandler(this.exportarSeleçãoToolStripMenuItem_Click);
            // 
            // tamanhoDaFonteToolStripMenuItem
            // 
            this.tamanhoDaFonteToolStripMenuItem.Name = "tamanhoDaFonteToolStripMenuItem";
            this.tamanhoDaFonteToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.tamanhoDaFonteToolStripMenuItem.Text = "Tamanho da fonte";
            this.tamanhoDaFonteToolStripMenuItem.Click += new System.EventHandler(this.tamanhoDaFonteToolStripMenuItem_Click);
            // 
            // UserControlRichTextOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextBoxResultadosEstimacao);
            this.Name = "UserControlRichTextOutput";
            this.Size = new System.Drawing.Size(823, 535);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxResultadosEstimacao;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem salvarTXTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportarParaTXTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportarSeleçãoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tamanhoDaFonteToolStripMenuItem;
    }
}
