using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IpeaGEO
{
    public partial class frmIndicesSegregacao : Form
    {
        public frmIndicesSegregacao()
        {
            InitializeComponent();
        }

        #region Métodos Básicos
        private string[] strVariaveis;
        public string[] Variaveis
        {
            set
            {
                strVariaveis = value;
            }
        }

        private DataSet dsDados;
        public DataSet Dados
        {
            set
            {
                dsDados = value;
            }
        }


        private string[] strVariaveisSelecionadas;
        public string[] VariaveisSelecionadas
        {
            get
            {
                return strVariaveisSelecionadas;
            }
        }

        private string[] strIndices;
        public string[] Indices
        {
            get
            {
                return strIndices;
            }
        }

        #endregion 


        private void frmIndicesSegregacao_Load(object sender, EventArgs e)
        {
            cmbArea.Items.AddRange(strVariaveis);
            cmbPopulacaoMajoritaria.Items.AddRange(strVariaveis);
            cmbPopulacaoMiniritaria.Items.AddRange(strVariaveis);
            cmbPopulacaoTotal.Items.AddRange(strVariaveis);

            //cmbArea.SelectedIndex = 0;
            //cmbPopulacaoMajoritaria.SelectedIndex = 0;
            //cmbPopulacaoMiniritaria.SelectedIndex = 0;
            //cmbPopulacaoTotal.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            //OK
            /*
            if (chkUniformidade.Checked == true && (cmbPopulacaoTotal.SelectedItem == "" || cmbPopulacaoMiniritaria.SelectedItem == ""))
            {
                MessageBox.Show("É necessário que as variáveis de população minoritária e população total estejam preenchidas.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (chkExposicao.Checked == true && (cmbPopulacaoTotal.SelectedItem == "" || cmbPopulacaoMiniritaria.SelectedItem == "" || cmbPopulacaoMajoritaria.SelectedItem == ""))
            {
                MessageBox.Show("É necessário que as variáveis de população minoritária, majoritária e população total estejam preenchidas.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (chkConcentracao.Checked == true && (cmbPopulacaoTotal.SelectedItem == "" || cmbPopulacaoMiniritaria.SelectedItem == "" || cmbPopulacaoMajoritaria.SelectedItem == ""))
            {
                MessageBox.Show("É necessário que as variáveis de área, população minoritária, majoritária e população total estejam preenchidas.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (chkCentralizacao.Checked == true && (cmbPopulacaoTotal.SelectedItem == "" || cmbPopulacaoMiniritaria.SelectedItem == "" || cmbPopulacaoMajoritaria.SelectedItem == ""))
            {
                MessageBox.Show("É necessário que as variáveis de área, população minoritária, majoritária e população total estejam preenchidas.", "Aviso.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            */
            clsIndicesDeSegregacao clsIndices = new clsIndicesDeSegregacao();
            if (chkUniformidade.Checked == true)
            {
                double iDissimilaridade = clsIndices.Dissimilarity(dsDados, cmbPopulacaoMiniritaria.SelectedItem.ToString(), cmbPopulacaoTotal.SelectedItem.ToString());
                double iGini = clsIndices.Gini(dsDados, cmbPopulacaoMiniritaria.SelectedItem.ToString(), cmbPopulacaoTotal.SelectedItem.ToString());
                double iEntropy = clsIndices.Entropy(dsDados, cmbPopulacaoMiniritaria.SelectedItem.ToString(), cmbPopulacaoTotal.SelectedItem.ToString());
            }
            if (chkExposicao.Checked == true)
            {
                double iInterecation = clsIndices.Interaction(dsDados, cmbPopulacaoTotal.SelectedItem.ToString(), cmbPopulacaoMiniritaria.SelectedItem.ToString(), cmbPopulacaoMajoritaria.SelectedItem.ToString());
                double iIsolation = clsIndices.Isolation(dsDados, cmbPopulacaoTotal.SelectedItem.ToString(), cmbPopulacaoMiniritaria.SelectedItem.ToString(), cmbPopulacaoMajoritaria.SelectedItem.ToString());
                double iCorrelation = clsIndices.Correlation(dsDados, cmbPopulacaoTotal.SelectedItem.ToString(), cmbPopulacaoMiniritaria.SelectedItem.ToString(), cmbPopulacaoMajoritaria.SelectedItem.ToString());
            }
            if (chkConcentracao.Checked == true)
            {
                double iDelta = clsIndices.Delta(dsDados, cmbPopulacaoMiniritaria.SelectedItem.ToString(), cmbArea.SelectedItem.ToString());
            }
               
            


            #region Informações para o relatório

            

            #endregion

            this.Cursor = Cursors.WaitCursor;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Application.DoEvents();
        }

        private void chkAglomeracao_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
