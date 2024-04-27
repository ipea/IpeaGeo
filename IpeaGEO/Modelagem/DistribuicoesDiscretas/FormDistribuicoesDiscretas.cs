using System;
using System.Data;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public partial class FormDistribuicoesDiscretas : Form
    {
        #region Variáveis internas

        public void HabilitarDadosExternos()
        {
            this.btnOK.Visible =
                btnOK.Enabled = true;
        }

        private DataTable m_dt_tabela_dados = new DataTable();
        public DataTable TabelaDeDados
        {
            get { return this.m_dt_tabela_dados; }
            set
            {
                this.m_dt_tabela_dados = value;

                AtualizaTabelaDados(true);

                if (value.Rows.Count > 0 && value.Columns.Count > 0)
                {
                    this.btnExecutar.Enabled = true;
                }
            }
        }

        private DataTable m_dt_tabela_shape = new DataTable();
        public DataTable DadosShape
        {
            get { return m_dt_tabela_shape; }
            set { m_dt_tabela_shape = value; }
        }

        public DataTable Dados
        {
            get { return m_dt_tabela_dados; }
            set { m_dt_tabela_dados = value; }
        }

        private clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            get { return m_shape; }
            set { m_shape = value; }
        }

        //private clsMapa m_mapa = new clsMapa();
        //public clsMapa Mapa
        //{
        //    get { return m_mapa; }
        //    set { m_mapa = value; }
        //}

        #endregion

        public FormDistribuicoesDiscretas()
        {
            InitializeComponent();
        }

        private BLogicDistribuicoesDiscretas m_blc = new BLogicDistribuicoesDiscretas();

        private void AtualizaTabelaDados(bool atualizaUControl)
        {
            if (atualizaUControl && m_dt_tabela_dados.Columns.Count > 0 && m_dt_tabela_dados.Rows.Count > 0)
            {
                this.userControlDataGrid1.TabelaDados = m_dt_tabela_dados;
            }
            else
            {
                if (this.userControlDataGrid1.TabelaDados.Columns.Count > 0 && this.userControlDataGrid1.TabelaDados.Rows.Count > 0)
                {
                    m_dt_tabela_dados = this.userControlDataGrid1.TabelaDados;
                }
            }

            clsUtilTools clt = new clsUtilTools();

            string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);
            userControlSelecaoVariaveis1.ZeraControle();
            userControlSelecaoVariaveis1.VariaveisList = variaveis_numericas;
            userControlSelecaoVariaveis1.VariaveisDB = variaveis_numericas;

            string[] lista_estatisticas = this.m_blc.ListaDistribuicoesDiscretas;
            userControlSelecaoVariaveis2.ZeraControle();
            userControlSelecaoVariaveis2.VariaveisList = lista_estatisticas;
            userControlSelecaoVariaveis2.VariaveisDB = lista_estatisticas;

            if (!tabControl1.TabPages.Contains(tabPage0))
            {
                tabControl1.TabPages.Add(tabPage0);
                this.tabControl1.SelectedTab = tabPage0;
            }

            if (!tabControl1.TabPages.Contains(tabPage2))
            {
                tabControl1.TabPages.Add(tabPage2);
            }
            
            if (!tabControl1.TabPages.Contains(tabPage4))
            {
                tabControl1.TabPages.Add(tabPage4);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }

        private void FormBaseModelagem_Load(object sender, EventArgs e)
        {
            try
            {   
                // Variáveis sendo passadas para o UserControl
                userControlDataGrid1.TabelaDados = this.m_dt_tabela_dados;
                userControlDataGrid1.FuncaoFromFormulario = new RegressoesEspaciais.UserControls.UserControlDataGrid.FunctionFromFormulario(this.AtualizaTabelaDados);
                userControlDataGrid1.MostraOpcaoImportadaoDados = true;
                userControlDataGrid1.UserControlSelecaoVariaveis = this.userControlSelecaoVariaveis1;

                userControlSelecaoVariaveis1.LabelListBoxEsquerda = "Variáveis numéricas na tabela";
                userControlSelecaoVariaveis1.LabelListBoxDireita = "Variáveis selecionadas";
                userControlSelecaoVariaveis1.PermiteSelecaoMultipla = true;

                userControlSelecaoVariaveis2.LabelListBoxEsquerda = "Distribuições disponíveis";
                userControlSelecaoVariaveis2.LabelListBoxDireita = "Distribuições selecionadas";
                userControlSelecaoVariaveis2.PermiteSelecaoMultipla = true;
                
                if (this.tabControl1.TabPages.Contains(this.tabPage2))
                    this.tabControl1.TabPages.Remove(this.tabPage2);

                if (this.tabControl1.TabPages.Contains(this.tabPage3))
                    this.tabControl1.TabPages.Remove(this.tabPage3);

                if (this.tabControl1.TabPages.Contains(this.tabPage4))
                    this.tabControl1.TabPages.Remove(this.tabPage4);

                if (m_dt_tabela_dados.Rows.Count <= 0 || m_dt_tabela_dados.Columns.Count <= 0)
                {
                    if (this.tabControl1.TabPages.Contains(this.tabPage0))
                        this.tabControl1.TabPages.Remove(this.tabPage0);
                }
                else
                {
                    this.tabControl1.SelectedTab = tabPage0;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                EstimarParametros();

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EstimarParametros()
        {
            m_blc.TabelaDados = m_dt_tabela_dados;
            m_blc.VariaveisDependentes = userControlSelecaoVariaveis1.VariaveisIndependentes;
            m_blc.DistribuicoesEscolhidas = userControlSelecaoVariaveis2.VariaveisIndependentes;

            m_blc.LimpaJanelaResultados = ckbLimpaJanelaOutput.Checked;
            m_blc.LimpaJanelaConsolidacao = checkBox1.Checked;

            m_blc.EstimaDistribuicao();

            this.userControlRichTextOutput1.Texto = m_blc.ResultadoEstimacao;
            this.userControlRichTextOutput2.Texto = m_blc.ResultadoEstimacao2;

            if (!this.tabControl1.TabPages.Contains(this.tabPage2))
                this.tabControl1.TabPages.Add(this.tabPage2);

            this.tabControl1.SelectedTab = tabPage2;
        }
    }
}
