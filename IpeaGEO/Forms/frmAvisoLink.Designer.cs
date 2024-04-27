namespace IpeaGeo.Forms
{
    partial class frmAvisoLink
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
            this.lbAvisoLink = new System.Windows.Forms.Label();
            this.llbAvisoLink = new System.Windows.Forms.LinkLabel();
            this.btnAvisoLink = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbAvisoLink
            // 
            this.lbAvisoLink.AutoSize = true;
            this.lbAvisoLink.Location = new System.Drawing.Point(12, 33);
            this.lbAvisoLink.Name = "lbAvisoLink";
            this.lbAvisoLink.Size = new System.Drawing.Size(35, 13);
            this.lbAvisoLink.TabIndex = 0;
            this.lbAvisoLink.Text = "label1";
            // 
            // llbAvisoLink
            // 
            this.llbAvisoLink.AutoSize = true;
            this.llbAvisoLink.Location = new System.Drawing.Point(13, 50);
            this.llbAvisoLink.Name = "llbAvisoLink";
            this.llbAvisoLink.Size = new System.Drawing.Size(55, 13);
            this.llbAvisoLink.TabIndex = 1;
            this.llbAvisoLink.TabStop = true;
            this.llbAvisoLink.Text = "linkLabel1";
            this.llbAvisoLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbAvisoLink_LinkClicked);
            // 
            // btnAvisoLink
            // 
            this.btnAvisoLink.Location = new System.Drawing.Point(138, 94);
            this.btnAvisoLink.Name = "btnAvisoLink";
            this.btnAvisoLink.Size = new System.Drawing.Size(97, 23);
            this.btnAvisoLink.TabIndex = 2;
            this.btnAvisoLink.Text = "Fechar";
            this.btnAvisoLink.UseVisualStyleBackColor = true;
            this.btnAvisoLink.Click += new System.EventHandler(this.btnAvisoLink_Click);
            // 
            // frmAvisoLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 129);
            this.Controls.Add(this.btnAvisoLink);
            this.Controls.Add(this.llbAvisoLink);
            this.Controls.Add(this.lbAvisoLink);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(386, 167);
            this.MinimumSize = new System.Drawing.Size(386, 167);
            this.Name = "frmAvisoLink";
            this.Text = "frmAvisoLink";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbAvisoLink;
        private System.Windows.Forms.LinkLabel llbAvisoLink;
        private System.Windows.Forms.Button btnAvisoLink;
    }
}