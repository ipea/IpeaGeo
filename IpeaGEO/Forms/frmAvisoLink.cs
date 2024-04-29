using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace IpeaGeo.Forms
{
    //criado para ser usado quando se quer colocar links nas mensagens de aviso (messagebox nao deixa ter links)
    public partial class frmAvisoLink : Form
    {
        public frmAvisoLink()
        {
            InitializeComponent();
        }
        
        //funcao para fechar a janela de AvisoLink
        private void btnAvisoLink_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        //funcao para passar o url da pagina da web para o link da janela de AvisoLink
        public string Link (string texto_link)
        {
            llbAvisoLink.Text = texto_link;
            return texto_link;
        }
        
        //funcao para passar o url da pagina da web para o link da janela de AvisoLink
        public string Label(string texto_label)
        {
            lbAvisoLink.Text = texto_label;
            return texto_label;
        }
        
        private void llbAvisoLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //quando o usuario clica no link a pagina da web abre
                System.Diagnostics.Process.Start(llbAvisoLink.Text);
            }
            catch(Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }      
    }
}
