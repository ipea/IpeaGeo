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
    public partial class FormAnalisesBasicas : Form
    {
        public FormAnalisesBasicas()
        {
            InitializeComponent();
        }

        private DataTable m_dt_dados = new DataTable();
        public DataTable TabelaDados
        {
            get
            {
                return m_dt_dados;
            }
            set
            {
            	m_dt_dados = value;
                userControlDataGrid1.TabelaDados = m_dt_dados;
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void AtualizaTabelaDados(bool atualizagrid)
        {
            this.lblArquivoImportado.Text = "Arquivo: " + userControlDataGrid1.NomeArquivoImportado;
        }

        private void FormAnalisesBasicas_Load(object sender, EventArgs e)
        {
            // Variáveis sendo passadas para o UserControl
            userControlDataGrid1.TabelaDados = this.m_dt_dados;
            userControlDataGrid1.FuncaoFromFormulario = new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionFromFormulario(this.AtualizaTabelaDados);
            userControlDataGrid1.MostraOpcaoImportadaoDados = true;

            lblArquivoImportado.Text = "";
        }
    }
}
