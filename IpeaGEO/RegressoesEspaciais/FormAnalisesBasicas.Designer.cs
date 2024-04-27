namespace IpeaGeo.RegressoesEspaciais
{
    partial class FormAnalisesBasicas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAnalisesBasicas));
            IpeaGeo.RegressoesEspaciais.clsIpeaShape clsIpeaShape1 = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.userControlDataGrid1 = new IpeaGeo.RegressoesEspaciais.UserControls.UserControlDataGrid();
            this.lblArquivoImportado = new System.Windows.Forms.Label();
            this.btnFechar = new System.Windows.Forms.Button();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.userControlDataGrid1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblArquivoImportado);
            this.splitContainer1.Panel2.Controls.Add(this.btnFechar);
            this.splitContainer1.Size = new System.Drawing.Size(937, 613);
            this.splitContainer1.SplitterDistance = 578;
            this.splitContainer1.TabIndex = 0;
            // 
            // userControlDataGrid1
            // 
            this.userControlDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDataGrid1.ListaVarsNumericas = new string[0];
            this.userControlDataGrid1.ListaVarsTotais = new string[0];
            this.userControlDataGrid1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataGrid1.Name = "userControlDataGrid1";
            clsIpeaShape1.CoordenadasEmRadianos = false;
            clsIpeaShape1.Count = 0;
            clsIpeaShape1.HoraCriacao = new System.DateTime(2013, 9, 25, 17, 39, 54, 187);
            clsIpeaShape1.MatrizAllDistances = null;
            clsIpeaShape1.Nome = "";
            clsIpeaShape1.OrdemVizinhanca = 1;
            clsIpeaShape1.Poligonos = new IpeaGeo.RegressoesEspaciais.clsIpeaPoligono[0];
            clsIpeaShape1.TipoContiguidade = IpeaGeo.RegressoesEspaciais.TipoContiguidade.Rook;
            clsIpeaShape1.TipoDistancia = false;
            clsIpeaShape1.TipoVizinhanca = "";
            this.userControlDataGrid1.Shape = clsIpeaShape1;
            this.userControlDataGrid1.Size = new System.Drawing.Size(937, 578);
            this.userControlDataGrid1.TabControl = null;
            this.userControlDataGrid1.TabIndex = 0;
            this.userControlDataGrid1.UserControlPropScoreMatching = null;
            this.userControlDataGrid1.UserControlRegInstrumentos = null;
            this.userControlDataGrid1.UserControlSelecao2BlocosVariaveis = null;
            this.userControlDataGrid1.UserControlSelecaoVariaveis = null;
            // 
            // lblArquivoImportado
            // 
            this.lblArquivoImportado.AutoSize = true;
            this.lblArquivoImportado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArquivoImportado.Location = new System.Drawing.Point(142, 7);
            this.lblArquivoImportado.Name = "lblArquivoImportado";
            this.lblArquivoImportado.Size = new System.Drawing.Size(41, 13);
            this.lblArquivoImportado.TabIndex = 5;
            this.lblArquivoImportado.Text = "label1";
            // 
            // btnFechar
            // 
            this.btnFechar.Location = new System.Drawing.Point(29, 2);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(75, 23);
            this.btnFechar.TabIndex = 4;
            this.btnFechar.Text = "&Fechar";
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
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
            // FormAnalisesBasicas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 613);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAnalisesBasicas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Análises Descritivas Básicas";
            this.Load += new System.EventHandler(this.FormAnalisesBasicas_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnFechar;
        private IpeaGeo.RegressoesEspaciais.UserControls.UserControlDataGrid userControlDataGrid1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Label lblArquivoImportado;
    }
}