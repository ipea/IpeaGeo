using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IpeaGeo.RegressoesEspaciais
{
    public partial class FormImputacaoValoresMissingPainelEspacial : Form
    {
        public FormImputacaoValoresMissingPainelEspacial()
        {
            InitializeComponent();
        }

        private DataTable m_dt = new DataTable();
        public DataTable Dados
        {
            get { return m_dt; }
            set { m_dt = value; }
        }

        private void FormImputacaoValoresMissingPainelEspacial_Load(object sender, EventArgs e)
        {
        }

        private void rdbValorFixo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbValorFixo.Checked) nudImputacaoValorFixo.Enabled = true;
            else nudImputacaoValorFixo.Enabled = false;
        }
    }
}
