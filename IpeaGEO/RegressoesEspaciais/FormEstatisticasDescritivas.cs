using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace IpeaGeo.RegressoesEspaciais
{
    public partial class FormEstatisticasDescritivas : Form
    {
        public FormEstatisticasDescritivas()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private DataTable m_dt_dados = new DataTable();
        public DataTable TabelaDados
        {
            set
            {
            	m_dt_dados = value;

                clsUtilTools clt = new clsUtilTools();

                string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_dados);
                userControlSelecaoVariaveis1.ZeraControle();
                userControlSelecaoVariaveis1.VariaveisList = variaveis_numericas;
                userControlSelecaoVariaveis1.VariaveisDB = variaveis_numericas;

                string[] lista_estatisticas = this.m_ble.ListaTodasEstatisticas;
                userControlSelecaoVariaveis2.ZeraControle();
                userControlSelecaoVariaveis2.VariaveisList = lista_estatisticas;
                userControlSelecaoVariaveis2.VariaveisDB = lista_estatisticas;

                string[] variaveis = clt.RetornaTodasColunas(m_dt_dados);
                userControlSelecaoVariaveis3.ZeraControle();
                userControlSelecaoVariaveis3.VariaveisList = variaveis;
                userControlSelecaoVariaveis3.VariaveisDB = variaveis;
            }
            get { return m_dt_dados; }
        }

        private void FormEstatisticasDescritivas_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.tabControl1.TabPages.Contains(this.tabPage2))
                    this.tabControl1.TabPages.Remove(this.tabPage2);

                userControlSelecaoVariaveis1.LabelListBoxEsquerda = "Variáveis numéricas na tabela";
                userControlSelecaoVariaveis1.LabelListBoxDireita = "Variáveis selecionadas";
                userControlSelecaoVariaveis1.PermiteSelecaoMultipla = true;

                userControlSelecaoVariaveis2.LabelListBoxEsquerda = "Estatísticas disponíveis";
                userControlSelecaoVariaveis2.LabelListBoxDireita = "Estatísticas selecionadas";
                userControlSelecaoVariaveis2.PermiteSelecaoMultipla = true;

                userControlSelecaoVariaveis3.LabelListBoxEsquerda = "Variáveis na tabela";
                userControlSelecaoVariaveis3.LabelListBoxDireita = "Variáveis de agrupamento";
                userControlSelecaoVariaveis3.PermiteSelecaoMultipla = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        BLEstatisticasDescritivas m_ble = new BLEstatisticasDescritivas();

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                clsUtilTools clt = new clsUtilTools();

                this.m_ble.TabelaDados = this.m_dt_dados;
                this.m_ble.VariaveisCategoricas = clt.EliminaStringsDuplicadas(this.userControlSelecaoVariaveis3.VariaveisIndependentes);
                this.m_ble.VariaveisContinuas = clt.EliminaStringsDuplicadas(this.userControlSelecaoVariaveis1.VariaveisIndependentes);
                this.m_ble.EstatisticasEscolhidas = clt.EliminaStringsDuplicadas(this.userControlSelecaoVariaveis2.VariaveisIndependentes);

                if (m_ble.VariaveisContinuas.GetLength(0) <= 0)
                    throw new Exception("Selecione pelo menos uma variável numérica para geração das estatísticas descritivas.");

                if (m_ble.EstatisticasEscolhidas.GetLength(0) <= 0)
                    throw new Exception("Selecione pelo menos uma estatística.");

                if (m_ble.VariaveisCategoricas.GetLength(0) <= 0)
                    m_ble.EstatisticasPorCategorias = false;
                else
                    m_ble.EstatisticasPorCategorias = true;

                if (this.rdbDadosAmostrais.Checked) this.m_ble.DadosPopulacionais = false;
                else m_ble.DadosPopulacionais = true;

                m_ble.PercPercentil1 = Convert.ToDouble(this.nudPercentil1.Value);
                m_ble.PercPercentil2 = Convert.ToDouble(this.nudPercentil2.Value);
                m_ble.PercPercentil3 = Convert.ToDouble(this.nudPercentil3.Value);
                m_ble.PercPercentil4 = Convert.ToDouble(this.nudPercentil4.Value);
                m_ble.NumDecimais = Convert.ToInt32(this.nudNumCasasDecimais.Value);

                this.m_ble.GeraEstatisticas();

                this.userControlRichTextOutput1.Texto = m_ble.OutputText;

                if (!this.tabControl1.TabPages.Contains(this.tabPage2))
                {
                    this.tabControl1.TabPages.Add(this.tabPage2);
                }
                this.tabControl1.SelectedTab = tabPage2;

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
