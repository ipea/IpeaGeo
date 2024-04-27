using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IpeaGeo.Modelagem
{
    public partial class FormQuickHelp : Form
    {
        public FormQuickHelp()
        {
            InitializeComponent();
        }

        private void FormQuickHelp_Load(object sender, EventArgs e)
        {

        }

        public string TipoModelo
        {
            set
            {
                this.Text = "Ajuda Rápida - " + value;
            }
        }

        public string TextoAjuda
        {
            set
            {
                userControlRichTextOutput1.Texto = value;
            }
        }
    }
}
