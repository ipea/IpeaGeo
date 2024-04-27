using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Globalization;

namespace IpeaGeo.RegressoesEspaciais
{
    public partial class FormConcatenacaoTabelaToShape : Form
    {
        public FormConcatenacaoTabelaToShape()
        {
            InitializeComponent();
        }

        #region variáveis internas
        
        private object[,] m_freqs_periodos_painel = new object[0, 0];
        public object[,] FrequenciaPeriodosPainel
        {
            get { return m_freqs_periodos_painel; }
        }

        private string m_variavel_periodos_painel = "";
        public string VariavelPeriodosPainel
        {
            get { return m_variavel_periodos_painel; }
        }

        private string m_variavel_unidades_painel = "";
        public string VariavelUnidadesPainel
        {
            get { return m_variavel_unidades_painel; }
        }

        private DataSet m_ds_dados_painel_espacial = new DataSet();
        public DataSet DsDadosPainelEspacial
        {
            get { return m_ds_dados_painel_espacial; }
            set { this.m_ds_dados_painel_espacial = value; }
        }

        private bool m_usa_dados_painel_espacial = false;
        public bool UsaDadosPainelEspacial
        {
            get { return this.m_usa_dados_painel_espacial; }
            set 
            { 
                m_usa_dados_painel_espacial = value;
                if (value)
                {
                    this.Text = "Concatenação da Tabela de Dados com o Arquivo Shape - Painel Espacial";

                    ckbUtilizarApenasDadosShape.Checked = false;
                    ckbUtilizarApenasDadosShape.Enabled = false;
                    btnConcatenarTabelas.Enabled = false;
                }
                else
                {
                    ckbUtilizarApenasDadosShape.Checked = false;
                    ckbUtilizarApenasDadosShape.Enabled = true;
                    btnConcatenarTabelas.Enabled = true;
                }
            }
        }

        private string m_id_base = "";
        public string IDbase
        {
            get { return m_id_base; }
        }

        private string m_id_mapa = "";
        public string IDmapa
        {
            get
            {
                return m_id_mapa;
            }
        }

        private string[] m_variaveis_no_shape = new string[0];
        public string[] VariaveisNoShape
        {
            get { return m_variaveis_no_shape; }
            set { m_variaveis_no_shape = value; }
        }

        private string m_file_name = "";
        public string FileName
        {
            get
            {
                return m_file_name;
            }
            set
            {
                m_file_name = value;
            }
        }

        private clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            get
            {
                return m_shape;
            }
            set
            {
                m_shape = value;
            }
        }

        private bool cidades = false;

        public bool CidadesPublic
        {
            get
            {
                return cidades;
            }
            set
            {
                cidades = value;
            }
        }

        private string pastapadrao = "";

        public string PastaPublic
        {
            get
            {
                return pastapadrao;
            }
            set
            {
                pastapadrao = value;
            }
        }

        private string cidnome = "";

        public string CidadesNOME
        {
            get
            {
                return cidnome;
            }
            set
            {
                cidnome = value;
            }
        }

        private DataTable m_dt_concatenados = new DataTable();
        public DataTable TabelaDadosConcatenados
        {
            get
            {
                DataTable dt = new DataTable();
                string id_shape = this.lblVariavelArquivoShape.Text;

                if (!m_usa_dados_painel_espacial)
                {
                    dt = m_dt_concatenados.Copy();

                    if (dt.Columns.Contains("Mapa" + id_shape)) { dt.Columns.Remove("Mapa" + id_shape); }
                    dt.Columns.Add("Mapa" + id_shape, typeof(Int32));

                    if (dt.Columns.Contains("sequencial_tabela_shape"))
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["Mapa" + id_shape] = dt.Rows[i]["sequencial_tabela_shape"];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["Mapa" + id_shape] = i;
                        }
                    }
                    dt.Columns["Mapa" + id_shape].Caption = "Sequencial das observações na base shape";

                    if (dt.Columns.Contains("Mapa" + id_shape)) dt.Columns["Mapa" + id_shape].ReadOnly = true;
                    if (dt.Columns.Contains("shape_" + id_shape)) dt.Columns["shape_" + id_shape].ReadOnly = true;

                    if (dt.Columns.Contains("sequencial_tabela_shape"))
                    {
                        dt.Columns.Remove("sequencial_tabela_shape");
                    }
                    if (dt.Columns.Contains("sequencial_tabela_dados"))
                    {
                        dt.Columns.Remove("sequencial_tabela_dados");
                    }
                }
                else
                {
                    AjustaDadosPainelEspacial();

                    dt = (DataTable)m_ds_dados_painel_espacial.Tables[0].Copy();
                }

                return dt;
            }
        }

        private void AjustaDadosPainelEspacial()
        {
            DataTable dt = new DataTable();
            string id_shape = this.lblVariavelArquivoShape.Text;

            for (int k = 0; k < m_ds_dados_painel_espacial.Tables.Count; k++)
            {
                dt = (DataTable)m_ds_dados_painel_espacial.Tables[k];
                
                if (dt.Columns.Contains("Mapa" + id_shape)) { dt.Columns.Remove("Mapa" + id_shape); }
                dt.Columns.Add("Mapa" + id_shape, typeof(Int32));

                if (dt.Columns.Contains("sequencial_tabela_shape"))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Mapa" + id_shape] = dt.Rows[i]["sequencial_tabela_shape"];
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Mapa" + id_shape] = i;
                    }
                }
                dt.Columns["Mapa" + id_shape].Caption = "Sequencial das observações na base shape";

                if (dt.Columns.Contains("Mapa" + id_shape)) dt.Columns["Mapa" + id_shape].ReadOnly = true;
                if (dt.Columns.Contains("shape_" + id_shape)) dt.Columns["shape_" + id_shape].ReadOnly = true;

                if (dt.Columns.Contains("sequencial_tabela_shape"))
                {
                    dt.Columns.Remove("sequencial_tabela_shape");
                }
                if (dt.Columns.Contains("sequencial_tabela_dados"))
                {
                    dt.Columns.Remove("sequencial_tabela_dados");
                }
            }
        }

        private string m_endereco_mapa = "";
        public string EnderecoMapa
        {
            get { return m_endereco_mapa; }
            set { m_endereco_mapa = value; }
        }

        private DataTable m_dt_dados = new DataTable();
        public DataTable TabelaDados
        {
            set
            {
                m_dt_dados = value.Copy();
                clsUtilTools clt = new clsUtilTools();

                //string[] all_variables = clt.RetornaTodasColunas(m_dt_dados);
                string[] all_variables = clt.RetornaUniqueColunas(m_dt_dados);

                this.listBox1.Items.Clear();
                this.listBox1.Items.AddRange(all_variables);
                this.listBox1.SelectedIndex = 0;

                this.cmbTabelasNoArquivo.Enabled = false;
                this.lblArquivoDeDados.Text = "Arquivo de dados já previamente importado";
                this.btnAbrirTabelaDados.Enabled = false;
                this.btnImportarTabela.Enabled = false;

                this.IncluirTabPage(tabPage3, false);
                this.userControlDataGrid1.TabelaDados = m_dt_dados;

                string[] var_numericas = clt.RetornaColunasNumericas(m_dt_dados);
                if (var_numericas.GetLength(0) > 0)
                {
                    lstCoordenadasX.Items.Clear();
                    lstCoordenadasX.Items.AddRange(var_numericas);
                    lstCoordenadasX.SelectedIndex = 0;

                    lstCoordenadasY.Items.Clear();
                    lstCoordenadasY.Items.AddRange(var_numericas);
                    lstCoordenadasY.SelectedIndex = 0;

                    AjustaMaximasCoordenadas();
                    m_form_iniciado = true;
                }
            }
            get
            {
                return this.m_dt_dados;
            }
        }

        private DataTable m_dt_shape = new DataTable();
        public DataTable TabelaShape
        {
            set
            {
                m_dt_shape = value.Copy();
                clsUtilTools clt = new clsUtilTools();

                //string[] all_variables = clt.RetornaTodasColunas(value);
                string[] all_variables = clt.RetornaUniqueColunas(value);

                m_variaveis_no_shape = clt.RetornaTodasColunas(value);

                this.listBox2.Items.Clear();
                this.listBox2.Items.AddRange(all_variables);
                this.listBox2.SelectedIndex = 0;

                this.IncluirTabPage(tabPage4, false);
                this.userControlDataGrid2.TabelaDados = m_dt_shape;
            }
            get
            {
                return this.m_dt_shape;
            }
        }

        #endregion

        #region inicialização do formulário

        public string appPath = Path.GetDirectoryName(Application.ExecutablePath);

        private void FormConcatenacaoTabelaToShape_Load(object sender, EventArgs e)
         {
            cmbTabelasNoArquivo.Enabled = true;

            ckbDelimitadoCaracter.Enabled =
                ckbDelimitadoPontoVirgula.Enabled =
                ckbDelimitadoTab.Enabled =
                ckbDelimitadoVirgula.Enabled =
                ckbNomesPrimeiraLinha.Enabled =
                ckbFormatoNumeroPortugues.Enabled = false;

            this.AjusteAutomaticoParametrosDecaimento();

            clsUtilTools clt = new clsUtilTools();

            if (m_dt_dados.Rows.Count > 0 && m_dt_dados.Columns.Count > 0)
            {
                string[] variaveis = clt.RetornaTodasColunas(m_dt_dados);
                userControlSelecaoVariaveis3.ZeraControle();
                userControlSelecaoVariaveis3.VariaveisList = variaveis;
                userControlSelecaoVariaveis3.VariaveisDB = variaveis;

                userControlSelecaoVariaveis3.LabelListBoxEsquerda = "Variáveis na tabela";
                userControlSelecaoVariaveis3.LabelListBoxDireita = "Variáveis de agrupamento";
                userControlSelecaoVariaveis3.PermiteSelecaoMultipla = true;
            }

            RemoverTabPage(this.tabPage2);

            if (this.m_dt_dados.Rows.Count <= 0 || m_dt_dados.Columns.Count <= 0)
            {
                RemoverTabPage(this.tabPage3);
            }

            if (this.m_dt_shape.Rows.Count <= 0 || m_dt_shape.Columns.Count <= 0)
            {
                RemoverTabPage(this.tabPage4);
            }

            RemoverTabPage(this.tabPage5);
            RemoverTabPage(this.tabPage6);
            RemoverTabPage(this.tabPage7);
            RemoverTabPage(this.tabPage8);
            RemoverTabPage(this.tabPage9);

            this.btnImportarTabela.Enabled = false;
            this.txtCaracterDelimitacao.Enabled = false;
            this.cmbTabelasNoArquivo.Enabled = true;
            this.btnConcatenarTabelas.Enabled = !m_usa_dados_painel_espacial;

            #region importação de tabelas já pré-definidas

            if (cidades == true)
            {
                try
                {
                    this.lblArquivoDeDados.Text = "";

                    string FileName = "";
                    frmMapa mapa = new frmMapa();

                    if (mapa.tab_mod_efetuada == false)
                    {
                        if (CidadesNOME == "mesoregiao") { FileName = appPath + "\\Bases\\IBGE_CIDADES_MESOREGIAO.xls"; }
                        if (CidadesNOME == "microregiao") { FileName = appPath + "\\Bases\\IBGE_CIDADES_MICROREGIAO.xls"; }
                        if (CidadesNOME == "municipio") { FileName = appPath + "\\Bases\\CIDADES_IPEAGEO.xls"; }
                        if (CidadesNOME == "municipio5564") { FileName = appPath + "\\Bases\\IBGE_CIDADES_5564.xls"; }
                        if (CidadesNOME == "sao_francisco") { FileName = appPath + "\\Bases\\CIDADES_SAO_FRANCISCO.xls"; }
                        if (CidadesNOME == "municipio5507") { FileName = appPath + "\\Bases\\CIDADES_IPEAGEO.xls"; }

                        if (CidadesNOME == "AC_Mun97_region") { FileName = appPath + "\\Bases\\AC_Mun97_region.xls"; }
                        if (CidadesNOME == "AL_Mun97_region") { FileName = appPath + "\\Bases\\AL_Mun97_region.xls"; }
                        if (CidadesNOME == "AP_Mun97_region") { FileName = appPath + "\\Bases\\AP_Mun97_region.xls"; }
                        if (CidadesNOME == "AM_Mun97_region") { FileName = appPath + "\\Bases\\AM_Mun97_region.xls"; }
                        if (CidadesNOME == "BA_Mun97_region") { FileName = appPath + "\\Bases\\BA_Mun97_region.xls"; }
                        if (CidadesNOME == "CE_Mun97_region") { FileName = appPath + "\\Bases\\CE_Mun97_region.xls"; }
                        if (CidadesNOME == "DF_Mun97_region") { FileName = appPath + "\\Bases\\DF_Mun97_region.xls"; }
                        if (CidadesNOME == "ES_Mun97_region") { FileName = appPath + "\\Bases\\ES_Mun97_region.xls"; }
                        if (CidadesNOME == "GO_Mun97_region") { FileName = appPath + "\\Bases\\GO_Mun97_region.xls"; }
                        if (CidadesNOME == "MA_Mun97_region") { FileName = appPath + "\\Bases\\MA_Mun97_region.xls"; }
                        if (CidadesNOME == "MT_Mun97_region") { FileName = appPath + "\\Bases\\MT_Mun97_region.xls"; }
                        if (CidadesNOME == "MS_Mun97_region") { FileName = appPath + "\\Bases\\MS_Mun97_region.xls"; }
                        if (CidadesNOME == "MG_Mun97_region") { FileName = appPath + "\\Bases\\MG_Mun97_region.xls"; }
                        if (CidadesNOME == "PA_Mun97_region") { FileName = appPath + "\\Bases\\PA_Mun97_region.xls"; }
                        if (CidadesNOME == "PB_Mun97_region") { FileName = appPath + "\\Bases\\PB_Mun97_region.xls"; }
                        if (CidadesNOME == "PR_Mun97_region") { FileName = appPath + "\\Bases\\PR_Mun97_region.xls"; }
                        if (CidadesNOME == "PE_Mun97_region") { FileName = appPath + "\\Bases\\PE_Mun97_region.xls"; }
                        if (CidadesNOME == "PI_Mun97_region") { FileName = appPath + "\\Bases\\PI_Mun97_region.xls"; }
                        if (CidadesNOME == "RJ_Mun97_region") { FileName = appPath + "\\Bases\\RJ_Mun97_region.xls"; }
                        if (CidadesNOME == "RN_Mun97_region") { FileName = appPath + "\\Bases\\RN_Mun97_region.xls"; }
                        if (CidadesNOME == "RS_Mun97_region") { FileName = appPath + "\\Bases\\RS_Mun97_region.xls"; }
                        if (CidadesNOME == "RO_Mun97_region") { FileName = appPath + "\\Bases\\RO_Mun97_region.xls"; }
                        if (CidadesNOME == "RR_Mun97_region") { FileName = appPath + "\\Bases\\RR_Mun97_region.xls"; }
                        if (CidadesNOME == "SC_Mun97_region") { FileName = appPath + "\\Bases\\SC_Mun97_region.xls"; }
                        if (CidadesNOME == "SP_Mun97_region") { FileName = appPath + "\\Bases\\SP_Mun97_region.xls"; }
                        if (CidadesNOME == "SE_Mun97_region") { FileName = appPath + "\\Bases\\SE_Mun97_region.xls"; }
                        if (CidadesNOME == "TO_Mun97_region") { FileName = appPath + "\\Bases\\TO_Mun97_region.xls"; }
                        if (CidadesNOME == "BR_MUN1997_CO_region") { FileName = appPath + "\\Bases\\BR_MUN1997_CO_region.xls"; }
                        if (CidadesNOME == "BR_MUN1997_N_region") { FileName = appPath + "\\Bases\\BR_MUN1997_N_region.xls"; }
                        if (CidadesNOME == "BR_MUN1997_NE_region") { FileName = appPath + "\\Bases\\BR_MUN1997_NE_region.xls"; }
                        if (CidadesNOME == "BR_MUN1997_S_region") { FileName = appPath + "\\Bases\\BR_MUN1997_S_region.xls"; }
                        if (CidadesNOME == "BR_MUN1997_SE_region") { FileName = appPath + "\\Bases\\BR_MUN1997_SE_region.xls"; }
                        if (CidadesNOME == "IDH_2010") { FileName = appPath + "\\Bases\\IDH_2010.xls"; }
                        if (CidadesNOME == "IDH_2000") { FileName = appPath + "\\Bases\\IDH_2000.xls"; }
                        if (CidadesNOME == "IDH_1991") { FileName = appPath + "\\Bases\\IDH_1991.xls"; }
                    }
                    else
                    {
                        Classes.clsArmazenamentoDados salvar = new Classes.clsArmazenamentoDados();
                        FileName = salvar.Base_(FileName);
                    }
                    
                    //m_file_name = FileName;
                    strExtensao = Path.GetExtension(FileName).ToUpper();
                    strEnderecoBase = FileName;

                    if (strExtensao == ".XLS")
                    
                    {
                        //Inicia conexão com o OLEDB
                        string m_cnnstring = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strEnderecoBase + ";Extended Properties=Excel 8.0;";
                        m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
                        m_cnn.Open();
                        System.Data.DataTable dt = null;
                        dt = m_cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        //Limpa os nomes retirando cifrao e unerline. Adiciona nomes na nova data table new_coll
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            string nome_celula = Convert.ToString(dt.Rows[i]["TABLE_NAME"]);
                            nome_celula = nome_celula.Replace("'","");
                            int indice_cifrao = nome_celula.IndexOf("$");
                            int indice_hash = nome_celula.IndexOf("#");
                            if (indice_cifrao == -1 || indice_hash != -1) { dt.Rows.RemoveAt(i); }                     
                        } 
                        //Limpa itens da box
                        this.cmbTabelasNoArquivo.Items.Clear();

                        string aba_copiada = "[~~]";
                        //Adiciona nomes de tabelas na box para escolha e retira nomes duplicados 
                        foreach (DataRow row in /*new_coll.Rows*/dt.Rows)
                        {
                            //this.cmbTabelasNoArquivo.Items.Add(row["TABLE_CATALOG"].ToString());
                            string row_1 = Convert.ToString(row["TABLE_NAME"]);
                            if (row_1.IndexOf(aba_copiada/*"'_"*/) >= 0)
                            {
                            }
                            else
                            {
                                this.cmbTabelasNoArquivo.Items.Add(row["TABLE_NAME"].ToString().Replace("'","").Replace("$",""));
                                String.Format("{0}$", this.cmbTabelasNoArquivo.Items);
                            }
                            aba_copiada = row_1;
                        }

                        if (this.cmbTabelasNoArquivo.Items.Count > 0)
                        {
                            this.btnImportarTabela.Enabled = true;
                            this.cmbTabelasNoArquivo.SelectedIndex = 0;
                        }
                        else
                        {
                            this.btnImportarTabela.Enabled = false;
                        }

                        Classes.clsArmazenamentoDados armazena = new Classes.clsArmazenamentoDados();
                        if (armazena.Leitura_efetuada == true)
                        {
                            this.ImportarTabela();
                            btnConcatenarTabelas.PerformClick();
                            btnOK.PerformClick();
                        }
                    }
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            #endregion 
        }

        #endregion

        #region eventos

        private void rdbImputacaoZero_CheckedChanged(object sender, EventArgs e)
        {
            btnImputacaoExecutarGeral.Enabled = false;
            btnAtualizarEspecificacoes.Enabled = true;
            EnableBotaoOpcoes();
        }

        private void rdbImputacaoMediaGeral_CheckedChanged(object sender, EventArgs e)
        {
            btnImputacaoExecutarGeral.Enabled = false;
            btnAtualizarEspecificacoes.Enabled = true;
            EnableBotaoOpcoes();
        }

        private void nudPropDistMaxima_ValueChanged(object sender, EventArgs e)
        {
            m_alteracao_max_dist = false;
            if (m_alteracao_prop_dist)
            {
                nudDistEuclidianaMaxima.Value = Convert.ToDecimal(m_max_distancia_Euclidiana
                    * Convert.ToDouble(this.nudPropDistMaxima.Value) / 100.0);
            }
            m_alteracao_max_dist = true;
        }

        private void nudDistEuclidianaMaxima_ValueChanged(object sender, EventArgs e)
        {
            m_alteracao_prop_dist = false;
            if (m_alteracao_max_dist)
            {
                nudPropDistMaxima.Value = Convert.ToDecimal((Convert.ToDouble(this.nudDistEuclidianaMaxima.Value) / m_max_distancia_Euclidiana) * 100.0);
            }
            m_alteracao_prop_dist = true;
        }

        private void lstCoordenadasX_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_form_iniciado)
            {
                Cursor = Cursors.WaitCursor;

                this.AjustaMaximasCoordenadas();
                AjusteAutomaticoParametrosDecaimento();

                Cursor = Cursors.Default;
            }
        }

        private void lstCoordenadasY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_form_iniciado)
            {
                Cursor = Cursors.WaitCursor;

                this.AjustaMaximasCoordenadas();
                AjusteAutomaticoParametrosDecaimento();

                Cursor = Cursors.Default;
            }
        }

        private void rdbDecaimentoExponencial_CheckedChanged(object sender, EventArgs e)
        {
            AjusteAutomaticoParametrosDecaimento();
        }

        private void rdbDecaimentoUniforme_CheckedChanged(object sender, EventArgs e)
        {
            AjusteAutomaticoParametrosDecaimento();
        }

        private void rdbDecaimentoGaussiano_CheckedChanged(object sender, EventArgs e)
        {
            AjusteAutomaticoParametrosDecaimento();
        }

        private void rdbDecaimentoQuadratico_CheckedChanged(object sender, EventArgs e)
        {
            AjusteAutomaticoParametrosDecaimento();
        }

        private void rdbDecaimentoLinear_CheckedChanged(object sender, EventArgs e)
        {
            AjusteAutomaticoParametrosDecaimento();
        }

        private void nudParm1FuncaoDecaimento_ValueChanged(object sender, EventArgs e)
        {
        }

        private void btnAtualizarEspecificacoes_Click(object sender, EventArgs e)
         {
            Cursor = Cursors.WaitCursor;
            btnImputacaoExecutarGeral.Enabled = true;
            btnAtualizarEspecificacoes.Enabled = false;
            AtualizaOpcoesImputacaoMissing();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblVariavelTabelaDados.Text = this.listBox1.SelectedItem.ToString();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblVariavelArquivoShape.Text = this.listBox2.SelectedItem.ToString();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.Cancel;
        }

        private void PreparandoTabelaArquivoShape()
        {
            try
            {
                if (lblVariavelArquivoShape.Text == "") throw new Exception("Escolha uma variável identificadora para o arquivo shape.");

                this.FileName = m_endereco_mapa;
                DataTable dt = this.TabelaShape.Copy();

                if (dt.Columns.Contains("shape_" + lblVariavelArquivoShape.Text))
                    dt.Columns.Remove("shape_" + lblVariavelArquivoShape.Text);

                if (dt.Columns.Contains("Mapa" + lblVariavelArquivoShape.Text))
                    dt.Columns.Remove("Mapa" + lblVariavelArquivoShape.Text);

                dt.Columns.Add("Mapa" + lblVariavelArquivoShape.Text, typeof(Int32));
                dt.Columns.Add("shape_" + lblVariavelArquivoShape.Text, dt.Columns[lblVariavelArquivoShape.Text].DataType);

                this.m_id_mapa = lblVariavelArquivoShape.Text;
                this.m_id_base = lblVariavelArquivoShape.Text;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Mapa" + lblVariavelArquivoShape.Text] = i;
                    dt.Rows[i]["shape_" + lblVariavelArquivoShape.Text] = dt.Rows[i][lblVariavelArquivoShape.Text];
                }

                m_dt_concatenados = dt.Copy();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ckbUtilizarApenasDadosShape.Checked)
            {
                PreparandoTabelaArquivoShape();
            }
            else
            {
                if (!m_dados_ja_imputados)
                {
                    ExecutarImputacaoGeral();
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            if (!this.tabControl1.TabPages.Contains(this.tabPage2))
                this.tabControl1.TabPages.Add(this.tabPage2);
            this.tabControl1.SelectedTab = tabPage2;
        }

        private void btnDadosMissing_Click(object sender, EventArgs e)
        {
            IncluirTabPage(this.tabPage6, true);
        }

        private void rdbImputacaoMediaVizinhos_CheckedChanged(object sender, EventArgs e)
        {
            btnImputacaoExecutarGeral.Enabled = false;
            btnAtualizarEspecificacoes.Enabled = true;
            if (rdbImputacaoMediaVizinhos.Checked)
            {
                IncluirTabPage(this.tabPage8, false);
                this.RemoverTabPage(this.tabPage7);
            }
            EnableBotaoOpcoes();
        }

        private void btnImputacaoMediaSubGrupo_CheckedChanged(object sender, EventArgs e)
        {
            btnImputacaoExecutarGeral.Enabled = false;
            btnAtualizarEspecificacoes.Enabled = true;
            if (this.btnImputacaoMediaSubGrupo.Checked)
            {
                IncluirTabPage(this.tabPage7, false);
                this.RemoverTabPage(this.tabPage8);

                userControlSelecaoVariaveis3.UnselectAll();
            }
            EnableBotaoOpcoes();
        }

        private void rdbImputacaoValorFixo_CheckedChanged(object sender, EventArgs e)
        {
            
            btnAtualizarEspecificacoes.Enabled = true;
            if (!this.rdbImputacaoValorFixo.Checked)
            {
                this.nudImputacaoValorFixo.Enabled = false;
            }
            else
            {
                this.nudImputacaoValorFixo.Enabled = true;
            }
            EnableBotaoOpcoes();
        }


        private static String[] xml_concat = new String[3];

        private void btnConcatenarTabelas_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if ((m_dt_dados.Rows.Count <= 0 || m_dt_dados.Columns.Count <= 0) && !ckbUtilizarApenasDadosShape.Checked)
                {
                    throw new Exception("Antes da concatenação, importe o arquivo de dados ou selecione a opção de utilização dos dados do arquivo shape"); 
                }

                if (this.m_usa_dados_painel_espacial && cmbVariavelPeriodoPainelEspacial.SelectedIndex < 0) throw new Exception("Selecione uma variável de identificação do período para o painel espacial."); 

                BLConcatenacaoArquivos bla = new BLConcatenacaoArquivos();
                Classes.clsArmazenamentoDados armazena = new Classes.clsArmazenamentoDados();

                if (!this.m_usa_dados_painel_espacial)
                {                    
                    if (armazena.Leitura_efetuada == true)
                    {
                        this.lblVariavelArquivoShape.Text = armazena.lblShape_concat;
                        this.lblVariavelTabelaDados.Text = armazena.lblDados_concat;

                        m_dt_concatenados = bla.ConcatenaShapeToDados(m_dt_shape, this.lblVariavelArquivoShape.Text,
                                              m_dt_dados, this.lblVariavelTabelaDados.Text, this.ckbAnexarDadosShape.Checked);
                    }
                    else
                    {
                        m_dt_concatenados = bla.ConcatenaShapeToDados(m_dt_shape, this.lblVariavelArquivoShape.Text,
                                                  m_dt_dados, this.lblVariavelTabelaDados.Text, this.ckbAnexarDadosShape.Checked);
                    }

                    m_id_mapa = this.lblVariavelArquivoShape.Text;
                    m_id_base = this.lblVariavelTabelaDados.Text;

                    this.IncluirTabPage(tabPage5, true);
                    this.userControlDataGrid3.TabelaDados = m_dt_concatenados;

                    m_array_missing_vars = bla.IdentificarVariaveisNumericasMissing(m_dt_concatenados);

                    userControlRichTextOutput1.Texto += bla.OutputText;

                    if (m_array_missing_vars.Count > 0)
                    {
                        this.btnDadosMissing.Enabled = true;
                        this.IncluirTabPage(tabPage6, true);
                        this.IncluirTabPage(tabPage9, false);

                        listBox3.Items.Clear();
                        listBox3.Items.AddRange(m_array_missing_vars.NomesVariaveisMissing);
                        m_ativa_imputacao_missing = true;
                        listBox3.SelectedIndex = 0;

                        userControlRichTextOutput1.Texto += "\n\n";
                        userControlRichTextOutput1.Texto += "=======================================================================================\n\n";
                        userControlRichTextOutput1.Texto += "Variáveis numéricas com valores missing \n";
                        for (int i = 0; i < m_array_missing_vars.Count; i++)
                        {
                            userControlRichTextOutput1.Texto += "\n";
                            userControlRichTextOutput1.Texto += m_array_missing_vars[i].NomeVariavelNum;
                        }
                        userControlRichTextOutput1.Texto += "\n";

                        bla.GeraTabelaDadosMissing(m_array_missing_vars, m_dt_concatenados);
                        userControlDataGrid4.TabelaDados = bla.TabelaDadosMissing;
                    }
                    else
                    {
                        userControlRichTextOutput1.Texto += "\n\n";
                        userControlRichTextOutput1.Texto += "Não há variáveis numéricas com observações missing\n\n";

                        m_dados_ja_imputados = true;
                    }

                    m_usa_dados_painel_espacial = false;

                    btnOK.Enabled = true;
                    btnRelatorio.Enabled = true;
                }
                else
                {
                    DataSet ds_concatenados = new DataSet();
                    string relatorio = "";

                    m_dt_concatenados = bla.DadosPainelConcatenarShapeToDados(m_dt_shape, this.lblVariavelArquivoShape.Text,
                                              m_ds_dados_painel_espacial, this.lblVariavelTabelaDados.Text, this.ckbAnexarDadosShape.Checked, m_freqs_periodos_painel, 
                                              ref ds_concatenados, ref relatorio);

                    m_id_mapa = this.lblVariavelArquivoShape.Text;
                    m_id_base = this.lblVariavelTabelaDados.Text;

                    this.IncluirTabPage(tabPage5, true);
                    this.userControlDataGrid3.TabelaDados = m_dt_concatenados;

                    m_array_missing_vars = bla.IdentificarVariaveisNumericasMissing(m_dt_concatenados);

                    if (m_array_missing_vars.Count > 0)
                    {
                        FormImputacaoValoresMissingPainelEspacial frm = new FormImputacaoValoresMissingPainelEspacial();
                        frm.Dados = m_dt_concatenados;
                        if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {

                        }
                    }

                    m_dados_ja_imputados = true;

                    userControlRichTextOutput1.Texto = relatorio;

                    this.m_usa_dados_painel_espacial = true;

                    btnOK.Enabled = true;
                    btnRelatorio.Enabled = true;

                    m_ds_dados_painel_espacial = ds_concatenados;
                    m_dt_dados = (DataTable)ds_concatenados.Tables[0];
                }

                if (m_variavel_periodos_painel != "" && m_variavel_periodos_painel != " ")
                {
                    m_variavel_unidades_painel = lblVariavelTabelaDados.Text;
                }

                Cursor = Cursors.Default;
                if (armazena.Leitura_efetuada == false)
                {
                    if (cmbTabelasNoArquivo.SelectedItem != null)
                    {
                        xml_concat[0] = cmbTabelasNoArquivo.SelectedItem.ToString();
                    }
                    xml_concat[1] = lblVariavelTabelaDados.Text;
                    xml_concat[2] = lblVariavelArquivoShape.Text;
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnImputacaoExecutarGeral_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                ExecutarImputacaoGeral();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnImputacaoOpcoes_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnImputacaoMediaSubGrupo.Checked)
                {
                    tabControl1.SelectedTab = tabPage7;
                }

                if (rdbImputacaoMediaVizinhos.Checked)
                {
                    tabControl1.SelectedTab = tabPage8;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExecutarMediaSubGrupos_Click(object sender, EventArgs e)
        {
            try
            {
                AtualizaOpcoesImputacaoMissing();
                this.tabControl1.SelectedTab = tabPage6;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                AtualizaOpcoesImputacaoMissing();
                this.tabControl1.SelectedTab = tabPage6;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void rdbVizinhancaFuncaoDecaimento_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbVizinhancaFuncaoDecaimento.Checked)
            {
                grbSelecaoBandwidth.Enabled = true;
                grbVizinhancaComDecaimento.Enabled = true;
                grbVizinhancaContiguidade.Enabled = false;
                grbVizinhosMaisProximos.Enabled = false;
            }
        }

        private void rdbVizinhancaContiguidade_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbVizinhancaContiguidade.Checked)
            {
                grbSelecaoBandwidth.Enabled = false;
                grbVizinhancaComDecaimento.Enabled = false;
                grbVizinhancaContiguidade.Enabled = true;
                grbVizinhosMaisProximos.Enabled = false;
            }
        }

        private void btnVizinhosMaisProximos_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbVizinhosMaisProximos.Checked)
            {
                grbSelecaoBandwidth.Enabled = true;
                grbVizinhancaComDecaimento.Enabled = false;
                grbVizinhancaContiguidade.Enabled = false;
                grbVizinhosMaisProximos.Enabled = true;
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizaVariavelMissing();
            btnImputacaoExecutarGeral.Enabled = false;
            btnAtualizarEspecificacoes.Enabled = true;
        }

        #endregion

        #region escolha e importação da tabela de dados

        //================================ Conexão MDB ======================================================//
        private System.Data.OleDb.OleDbConnection m_cnn = new System.Data.OleDb.OleDbConnection();
        private System.Data.OleDb.OleDbDataAdapter m_dap = new System.Data.OleDb.OleDbDataAdapter();
        private string strExtensao = "";
        private string strEnderecoBase = "";
        private DataSet dsDados = new DataSet();
        bool abrir = false;

        private void AbrirTabelaDados()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                this.lblArquivoDeDados.Text = "";

                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (rdbArquivosSAS.Checked)
                {
                    openFileDialog.Filter = "SAS (*.sas7bdat)|*.sas7bdat";
                }
                else if (rdbArquivosExcelMDB.Checked)
                {
                    openFileDialog.Filter = "Excel (*.xls)|*.xls|Excel (*.xlsx)|*.xlsx|Access (*.mdb)|*.mdb";
                }
                else
                {
                    openFileDialog.Filter = "Delimited File (*.txt)|*.txt|Delimited File (*.dat)|*.dat|Comma Separated (*.csv)|*.csv";
                }

                string FileName = "";
                
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    abrir = true;
                }
                if (abrir == true)
                {
                    FileName = openFileDialog.FileName;
                    m_file_name = FileName;
                    strExtensao = Path.GetExtension(FileName).ToUpper();
                    strEnderecoBase = FileName;

                    if (strExtensao == ".XLS")
                    {
                        //Inicia conexão com o OLEDB
                        string m_cnnstring = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strEnderecoBase + ";Extended Properties=Excel 8.0;";
                        m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
                        m_cnn.Open();
                        System.Data.DataTable dt = null;

                        dt = m_cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        //Eliminando linhas duplicadas com cifrão no final                    
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            string nome_celula = Convert.ToString(dt.Rows[i]["TABLE_NAME"]);
                            int indice_cifrao = nome_celula.IndexOf("$");
                            if (indice_cifrao == -1)
                            {
                                dt.Rows.RemoveAt(i);
                            }
                        }

                        //string[] restrictions = new string[dt.Rows.Count];
                        this.cmbTabelasNoArquivo.Items.Clear();
                        foreach (DataRow row in dt.Rows)
                        {
                            string row_1 = Convert.ToString(row["TABLE_NAME"]);
                            if (row_1.IndexOf("'_") > 0)
                            {
                            }
                            else
                            {
                                this.cmbTabelasNoArquivo.Items.Add(row["TABLE_NAME"].ToString().Replace("'", "").Replace("$", ""));
                                String.Format("{0}$", this.cmbTabelasNoArquivo.Items);
                            }
                        }
                        if (this.cmbTabelasNoArquivo.Items.Count > 0)
                        {
                            this.btnImportarTabela.Enabled = true;
                            this.cmbTabelasNoArquivo.SelectedIndex = 0;
                        }
                        else
                        {
                            this.btnImportarTabela.Enabled = false;
                        }
                    }
                    if (strExtensao == ".XLSX")
                    {
                        //Inicia conexão com o OLEDB
                        string m_cnnstring = "Provider=Microsoft.ace.OLEDB.12.0;" + "Data Source=" + strEnderecoBase + ";Extended Properties=Excel 12.0 Xml;";
                        m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
                        m_cnn.Open();
                        System.Data.DataTable dt = null;

                        dt = m_cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        //Eliminando linhas duplicadas com cifrão no final                    
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            string nome_celula = Convert.ToString(dt.Rows[i]["TABLE_NAME"]);
                            int indice_cifrao = nome_celula.IndexOf("$");
                            if (indice_cifrao == -1)
                            {
                                dt.Rows.RemoveAt(i);
                            }
                        }

                        //string[] restrictions = new string[dt.Rows.Count];
                        this.cmbTabelasNoArquivo.Items.Clear();
                        foreach (DataRow row in dt.Rows)
                        {
                            string row_1 = Convert.ToString(row["TABLE_NAME"]);
                            if (row_1.IndexOf("'_") > 0)
                            {
                            }
                            else
                            {
                                this.cmbTabelasNoArquivo.Items.Add(row["TABLE_NAME"].ToString());
                            }
                        }
                        if (this.cmbTabelasNoArquivo.Items.Count > 0)
                        {
                            this.btnImportarTabela.Enabled = true;
                            this.cmbTabelasNoArquivo.SelectedIndex = 0;
                        }
                        else
                        {
                            this.btnImportarTabela.Enabled = false;
                        }
                    }

                    if (strExtensao == ".TXT" || strExtensao == ".DAT" || strExtensao == ".CSV")
                    {
                        ckbDelimitadoTab.Enabled =
                           ckbDelimitadoVirgula.Enabled =
                           ckbNomesPrimeiraLinha.Enabled =
                           ckbDelimitadoPontoVirgula.Enabled =
                           ckbDelimitadoCaracter.Enabled =
                           this.ckbFormatoNumeroPortugues.Enabled = true;

                        if (strExtensao == ".TXT" || strExtensao == ".DAT")
                        {
                            ckbDelimitadoTab.Checked = true;
                            ckbNomesPrimeiraLinha.Checked = true;
                        }

                        if (strExtensao == ".CSV")
                        {
                            this.ckbDelimitadoPontoVirgula.Checked = true;
                            ckbNomesPrimeiraLinha.Checked = true;
                        }

                        this.btnImportarTabela.Enabled = true;
                    }
                    else
                    {
                        ckbDelimitadoTab.Enabled =
                            ckbDelimitadoVirgula.Enabled =
                            ckbNomesPrimeiraLinha.Enabled =
                            ckbDelimitadoPontoVirgula.Enabled =
                            ckbDelimitadoCaracter.Enabled =
                            this.ckbFormatoNumeroPortugues.Enabled = false;

                        this.btnImportarTabela.Enabled = true;
                    }

                    if (strExtensao == ".SAS7BDAT")
                    {
                        //txtEndereco.Text = strEnderecoBase;
                        //label2.Text = "";
                        cmbTabelasNoArquivo.Enabled = false;
                        //Endereço da pasta com o arquivo
                        string strPath = Path.GetDirectoryName(strEnderecoBase);
                        //Nome do arquivo
                        string strAqruivo = Path.GetFileNameWithoutExtension(strEnderecoBase);
                        //String de conexão
                        string m_cnnstring = "Provider=sas.LocalProvider; Data Source=" + strPath;
                        m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
                        m_cnn.Open();

                        System.Data.DataTable dt = null;

                        dt = m_cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        //string[] restrictions = new string[dt.Rows.Count];
                        this.cmbTabelasNoArquivo.Items.Clear();
                        foreach (DataRow row in dt.Rows)
                        {
                            string row_1 = Convert.ToString(row["TABLE_NAME"]);
                            if (row_1.IndexOf("'_") > 0)
                            {
                            }
                            else
                            {
                                this.cmbTabelasNoArquivo.Items.Add(row["TABLE_NAME"].ToString());
                            }
                        }
                        if (this.cmbTabelasNoArquivo.Items.Count > 0)
                        {
                            this.btnImportarTabela.Enabled = true;
                            this.cmbTabelasNoArquivo.SelectedIndex = 0;
                        }
                        else
                        {
                            this.btnImportarTabela.Enabled = false;
                        }

                        m_cnn.Close();
                    }

                    if (strExtensao == ".MDB")
                    {
                        //Inicia conexão com o MDB
                        string m_cnnstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strEnderecoBase;
                        m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
                        m_cnn.Open();
                        string[] restrictions = new string[4];
                        restrictions[3] = "Table";

                        //Mostra as tabelas de usuario.
                        DataTable userTables = null;
                        userTables = m_cnn.GetSchema("Tables", restrictions);
                        cmbTabelasNoArquivo.Items.Clear();
                        for (int i = 0; i < userTables.Rows.Count; i++)
                        {
                            cmbTabelasNoArquivo.Items.Add(userTables.Rows[i][2].ToString());
                        }

                        System.Data.DataTable dt = null;
                        dt = m_cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                        //string[] restrictions = new string[dt.Rows.Count];
                        this.cmbTabelasNoArquivo.Items.Clear();
                        foreach (DataRow row in dt.Rows)
                        {
                            string row_1 = Convert.ToString(row["TABLE_NAME"]);
                            if (row_1.IndexOf("'_") > 0)
                            {
                            }
                            else
                            {
                                this.cmbTabelasNoArquivo.Items.Add(row["TABLE_NAME"].ToString());
                            }
                        }
                        if (this.cmbTabelasNoArquivo.Items.Count > 0)
                        {
                            this.btnImportarTabela.Enabled = true;
                            this.cmbTabelasNoArquivo.SelectedIndex = 0;
                        }
                        else
                        {
                            this.btnImportarTabela.Enabled = false;
                        }
                    }

                    this.lblArquivoDeDados.Text = FileName;
                }

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAbrirTabelaDados_Click(object sender, EventArgs e)
        {
            this.AbrirTabelaDados();
        }

        private void ImportarTabela()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (this.cmbTabelasNoArquivo.Items.Count <= 0 && strExtensao != ".TXT" && strExtensao != ".DAT" && strExtensao != ".CSV")
                {
                    throw new Exception("Selecione um arquivo de dados.");
                }
                Classes.clsArmazenamentoDados armazena = new Classes.clsArmazenamentoDados();

                dsDados.Reset();

                if (strExtensao == ".SAS7BDAT")
                {
                    string strAqruivo = Path.GetFileNameWithoutExtension(strEnderecoBase);
                    OleDbCommand sasCommand = m_cnn.CreateCommand();
                    sasCommand.CommandType = CommandType.TableDirect;
                    sasCommand.CommandText = strAqruivo;
                    m_dap = new System.Data.OleDb.OleDbDataAdapter(sasCommand);
                    m_dap.Fill(dsDados, "Table1");

                    m_cnn.Close();

                    m_dt_dados = dsDados.Tables["Table1"];
                }
                
                if (strExtensao == ".MDB")
                {
                    m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM " + this.cmbTabelasNoArquivo.SelectedItem.ToString(), m_cnn);
                    m_cnn.Close();

                    m_dt_dados = dsDados.Tables["Table1"];
                }
                
                if (strExtensao == ".TXT" || strExtensao == ".DAT" || strExtensao == ".CSV")
                {
                    DataTable dt_saida = new DataTable();

                    clsUtilTools clt = new clsUtilTools();

                    clt.LerArquivoTextoDelimited(strEnderecoBase, ref dt_saida, ckbDelimitadoPontoVirgula.Checked,
                        ckbDelimitadoVirgula.Checked, ckbDelimitadoCaracter.Checked, txtCaracterDelimitacao.Text,
                        ckbNomesPrimeiraLinha.Checked, ckbFormatoNumeroPortugues.Checked);

                    m_dt_dados = dt_saida;
                }

                else if (strExtensao == ".XLS")
                {
                    m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + this.cmbTabelasNoArquivo.SelectedItem.ToString() + "$]", m_cnn);
                    m_dap.Fill(dsDados, "Table1");

                    m_cnn.Close();

                    m_dt_dados = dsDados.Tables["Table1"];
                }
                else if (strExtensao == ".XLSX")
                {
                    m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + this.cmbTabelasNoArquivo.SelectedItem.ToString() + "]", m_cnn);
                    m_dap.Fill(dsDados, "Table1");

                    m_cnn.Close();

                    m_dt_dados = dsDados.Tables["Table1"];
                }

                if (m_dt_dados.Rows.Count > 0 && m_dt_dados.Columns.Count > 0)
                {
                    clsUtilTools clt = new clsUtilTools();

                    string[] all_variables = clt.RetornaUniqueColunas(m_dt_dados);

                    if (all_variables.GetLength(0) > 0)
                    {
                        this.listBox1.Items.Clear();
                        this.listBox1.Items.AddRange(all_variables);
                        this.listBox1.SelectedIndex = 0;

                        this.IncluirTabPage(tabPage3, false);
                        this.userControlDataGrid1.TabelaDados = m_dt_dados;

                        string[] var_numericas = clt.RetornaColunasNumericas(m_dt_dados);
                        string[] var_num_naonula = new string[0];

                        if (var_numericas.GetLength(0) > 0)
                        {
                            lstCoordenadasX.Items.Clear();
                            lstCoordenadasX.Items.AddRange(var_numericas);
                            lstCoordenadasX.SelectedIndex = 0;

                            lstCoordenadasY.Items.Clear();
                            lstCoordenadasY.Items.AddRange(var_numericas);
                            lstCoordenadasY.SelectedIndex = 0;

                            AjustaMaximasCoordenadas();
                            m_form_iniciado = true;
                        }

                        AtualizaTabelaDados();

                        this.AjusteAutomaticoParametrosDecaimento();

                        if (m_dt_dados.Rows.Count > 0 && m_dt_dados.Columns.Count > 0)
                        {
                            string[] variaveis = clt.RetornaTodasColunas(m_dt_dados);
                            userControlSelecaoVariaveis3.ZeraControle();
                            userControlSelecaoVariaveis3.VariaveisList = variaveis;
                            userControlSelecaoVariaveis3.VariaveisDB = variaveis;

                            userControlSelecaoVariaveis3.LabelListBoxEsquerda = "Variáveis na tabela";
                            userControlSelecaoVariaveis3.LabelListBoxDireita = "Variáveis de agrupamento";
                            userControlSelecaoVariaveis3.PermiteSelecaoMultipla = true;
                        }

                        if (this.m_usa_dados_painel_espacial)
                        {
                            this.grbImportacaoDadosPainel.Enabled = true;
                            this.cmbVariavelPeriodoPainelEspacial.Enabled = true;

                            string[] todas_variables = clt.RetornaTodasColunas(m_dt_dados);

                            cmbVariavelPeriodoPainelEspacial.Items.Clear();
                            cmbVariavelPeriodoPainelEspacial.Items.AddRange(todas_variables);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Não existem colunas com valores únicos na tabela de dados. Importe uma outra tabela ou habilite a opção de importação de dados de painel espacial",
                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                Cursor = Cursors.Default;
            }
              catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnImportarTabela_Click(object sender, EventArgs e)
        {
            ImportarTabela();
            if (!this.m_usa_dados_painel_espacial) this.btnConcatenarTabelas.Enabled = true;
            Cursor = Cursors.Default;
        }

        private void AtualizaTabelaDados()
        {
            try
            {
                IncluirTabPage(this.tabPage3, false);
                this.userControlDataGrid1.TabelaDados = m_dt_dados;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region executa imputação de valores missing

        private bool m_ativa_imputacao_missing = false;
        private clsArrayVariaveisNumMissing m_array_missing_vars = new clsArrayVariaveisNumMissing();
        private clsVariaveisNumMissing m_variavel_missing = new clsVariaveisNumMissing();

        private void AtualizaVariavelMissing()
        {
            try
            {
                if (m_ativa_imputacao_missing)
                {
                    string variavel = listBox3.SelectedItem.ToString();
                    m_variavel_missing = m_array_missing_vars[variavel];

                    this.userControlRichTextOutput3.Texto = m_variavel_missing.ToText;

                    switch (m_variavel_missing.TipoImputacao)
                    {
                        case TipoImputacaoNumMissing.Zero:
                            nudImputacaoValorFixo.Enabled = true;
                            nudImputacaoValorFixo.Value = 0;
                            nudImputacaoValorFixo.Enabled = false;
                            rdbImputacaoZero.Checked = true;
                            break;
                        case TipoImputacaoNumMissing.ValorFixo:
                            nudImputacaoValorFixo.Enabled = true;
                            nudImputacaoValorFixo.Value = Convert.ToDecimal(m_variavel_missing.ValorFixo);
                            rdbImputacaoValorFixo.Checked = true;
                            break;
                        case TipoImputacaoNumMissing.MediaVizinhos:
                            nudImputacaoValorFixo.Enabled = true;
                            nudImputacaoValorFixo.Value = 0;
                            nudImputacaoValorFixo.Enabled = false;
                            rdbImputacaoMediaVizinhos.Checked = true;
                            this.IncluirTabPage(tabPage8, false);
                            if (m_variavel_missing.TipoVizinhancaParaImputacao == TipoImputacaoVizinhanca.FuncaoDecaimento)
                            {
                                rdbVizinhancaFuncaoDecaimento.Checked = true;
                                lstCoordenadasX.SelectedItem = m_variavel_missing.NomeVariavelCoordX;
                                lstCoordenadasY.SelectedItem = m_variavel_missing.NomeVariavelCoordY;
                                nudDistEuclidianaMaxima.Value = Convert.ToDecimal(m_variavel_missing.DistEuclidianaCorte);
                                if (m_variavel_missing.TipoFuncaoDecaimento == TipoFuncaoDecaimento.Exponencial) rdbDecaimentoExponencial.Checked = true;
                                if (m_variavel_missing.TipoFuncaoDecaimento == TipoFuncaoDecaimento.Gaussiana) rdbDecaimentoGaussiano.Checked = true;
                                if (m_variavel_missing.TipoFuncaoDecaimento == TipoFuncaoDecaimento.Linear) rdbDecaimentoLinear.Checked = true;
                                if (m_variavel_missing.TipoFuncaoDecaimento == TipoFuncaoDecaimento.Quadratico) rdbDecaimentoQuadratico.Checked = true;
                                if (m_variavel_missing.TipoFuncaoDecaimento == TipoFuncaoDecaimento.Uniforme) rdbDecaimentoUniforme.Checked = true;
                                nudParm1FuncaoDecaimento.Value = Convert.ToDecimal(m_variavel_missing.ParametroDecaimento);
                            }
                            if (m_variavel_missing.TipoVizinhancaParaImputacao == TipoImputacaoVizinhanca.MatrizContiguidade)
                            {
                                rdbVizinhancaContiguidade.Checked = true;
                                if (m_variavel_missing.TipoContiguidade == TipoContiguidade.Queen) { rdbQueen.Checked = true; }
                                else { rdbRook.Checked = true; }
                            }
                            if (m_variavel_missing.TipoVizinhancaParaImputacao == TipoImputacaoVizinhanca.VizinhosMaisProximos)
                            {
                                rdbVizinhosMaisProximos.Checked = true;
                                nudNumVizinhosDistancia.Value = Convert.ToDecimal(m_variavel_missing.NumVizinhosImputacao);
                            }
                            break;
                        case TipoImputacaoNumMissing.MediaGeral:
                            nudImputacaoValorFixo.Enabled = true;
                            nudImputacaoValorFixo.Value = 0;
                            nudImputacaoValorFixo.Enabled = false;
                            rdbImputacaoMediaGeral.Checked = true;
                            break;
                        case TipoImputacaoNumMissing.MediaCategoria:
                            nudImputacaoValorFixo.Enabled = true;
                            nudImputacaoValorFixo.Value = 0;
                            nudImputacaoValorFixo.Enabled = false;
                            btnImputacaoMediaSubGrupo.Checked = true;
                            this.IncluirTabPage(tabPage7, false);
                            if (m_variavel_missing.NomesVariaveisCategoricas.GetLength(0) > 0)
                            {
                                userControlSelecaoVariaveis3.VariaveisIndependentes = m_variavel_missing.NomesVariaveisCategoricas;
                            }
                            else
                            {
                                userControlSelecaoVariaveis3.UnselectAll();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool m_dados_ja_imputados = false;

        private void ExecutarImputacaoGeral()
        {
            try
            {
                string variavel = "";
                int indice = 0;
                object[,] vcategorias = new object[0, 0];
                double[] coord_x = new double[0];
                double[] coord_y = new double[0];
                for (int i = 0; i < m_array_missing_vars.Count; i++)
                {
                    if (m_array_missing_vars[i].TipoImputacao == TipoImputacaoNumMissing.MediaCategoria)
                    {
                        vcategorias = new object[m_dt_concatenados.Rows.Count, m_array_missing_vars[i].NomesVariaveisCategoricas.GetLength(0)];
                        for (int k = 0; k < vcategorias.GetLength(0); k++)
                        {
                            for (int j = 0; j < vcategorias.GetLength(1); j++)
                            {
                                vcategorias[k, j] = m_dt_concatenados.Rows[k][m_array_missing_vars[i].NomesVariaveisCategoricas[j]];
                            }
                        }
                        m_array_missing_vars[i].ValoresCategorias = vcategorias;
                    }

                    if (m_array_missing_vars[i].TipoImputacao == TipoImputacaoNumMissing.MediaVizinhos)
                    {
                        if (m_array_missing_vars[i].TipoVizinhancaParaImputacao == TipoImputacaoVizinhanca.FuncaoDecaimento
                            || m_array_missing_vars[i].TipoVizinhancaParaImputacao == TipoImputacaoVizinhanca.VizinhosMaisProximos)
                        {
                            coord_x = new double[m_dt_concatenados.Rows.Count];
                            coord_y = new double[m_dt_concatenados.Rows.Count];
                            for (int k = 0; k < coord_y.GetLength(0); k++)
                            {
                                coord_x[k] = Convert.ToDouble(m_dt_concatenados.Rows[k][m_array_missing_vars[i].NomeVariavelCoordX]);
                                if (double.IsInfinity(coord_x[k]) || double.IsNaN(coord_x[k]) || double.IsNegativeInfinity(coord_x[k])
                                    || double.IsPositiveInfinity(coord_x[k]))
                                {
                                    throw new Exception("Variável com coordenadas X não pode contêr valores inválidos. Escolha outra variável ou cheque os seus dados.");
                                }

                                coord_y[k] = Convert.ToDouble(m_dt_concatenados.Rows[k][m_array_missing_vars[i].NomeVariavelCoordY]);
                                if (double.IsInfinity(coord_y[k]) || double.IsNaN(coord_y[k]) || double.IsNegativeInfinity(coord_y[k])
                                    || double.IsPositiveInfinity(coord_y[k]))
                                {
                                    throw new Exception("Variável com coordenadas Y não pode contêr valores inválidos. Escolha outra variável ou cheque os seus dados.");
                                }

                                m_array_missing_vars[i].VetorVariavelX = coord_x;
                                m_array_missing_vars[i].VetorVariavelY = coord_y;
                            }
                        }

                        if (m_array_missing_vars[i].TipoVizinhancaParaImputacao == TipoImputacaoVizinhanca.MatrizContiguidade)
                        {
                            m_array_missing_vars[i].Shape = m_shape;
                        }
                    }

                    m_array_missing_vars[i].ExecutaImputacao();
                    variavel = m_array_missing_vars[i].NomeVariavelNum;

                    for (int k = 0; k < m_array_missing_vars[i].IndicesValoresMissing.Count; k++)
                    {
                        indice = Convert.ToInt32(m_array_missing_vars[i].IndicesValoresMissing[k]);
                        m_dt_concatenados.Rows[indice][variavel] = m_array_missing_vars[i].ValoresVariavel[indice];
                    }

                    userControlRichTextOutput1.Texto += "\n";
                    userControlRichTextOutput1.Texto += "=======================================================================================\n\n";
                    userControlRichTextOutput1.Texto += m_array_missing_vars[i].ToText;
                }

                userControlDataGrid3.TabelaDados = m_dt_concatenados;
                m_dados_ja_imputados = true;

                variavel = listBox3.SelectedItem.ToString();
                userControlRichTextOutput3.Texto = m_array_missing_vars[variavel].ToText;

                BLConcatenacaoArquivos bla = new BLConcatenacaoArquivos();

                bla.GeraTabelaDadosMissing(m_array_missing_vars, m_dt_concatenados);
                userControlDataGrid4.TabelaDados = bla.TabelaDadosMissing;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AtualizaOpcoesImputacaoMissing()
        {
            try
            {
                string variavel = listBox3.SelectedItem.ToString();

                TipoImputacaoNumMissing tipo = TipoImputacaoNumMissing.Zero;
                if (rdbImputacaoMediaGeral.Checked) tipo = TipoImputacaoNumMissing.MediaGeral;
                if (rdbImputacaoMediaVizinhos.Checked) tipo = TipoImputacaoNumMissing.MediaVizinhos;
                if (rdbImputacaoValorFixo.Checked) tipo = TipoImputacaoNumMissing.ValorFixo;
                if (btnImputacaoMediaSubGrupo.Checked) tipo = TipoImputacaoNumMissing.MediaCategoria;

                m_array_missing_vars[variavel].TipoImputacao = tipo;
                m_array_missing_vars[variavel].StatusImputacao = "";

                switch (tipo)
                {
                    case TipoImputacaoNumMissing.MediaVizinhos:
                        m_array_missing_vars[variavel].DistEuclidianaCorte = Convert.ToDouble(nudDistEuclidianaMaxima.Value);
                        m_array_missing_vars[variavel].NomeVariavelCoordX = lstCoordenadasX.SelectedItem.ToString();
                        m_array_missing_vars[variavel].NomeVariavelCoordY = lstCoordenadasY.SelectedItem.ToString();
                        m_array_missing_vars[variavel].NumVizinhosImputacao = Convert.ToInt32(nudNumVizinhosDistancia.Value);
                        m_array_missing_vars[variavel].ParametroDecaimento = Convert.ToDouble(nudParm1FuncaoDecaimento.Value);

                        //===================== especificando a função de decaimento
                        if (rdbDecaimentoExponencial.Checked) m_array_missing_vars[variavel].TipoFuncaoDecaimento = TipoFuncaoDecaimento.Exponencial;
                        if (this.rdbDecaimentoGaussiano.Checked) m_array_missing_vars[variavel].TipoFuncaoDecaimento = TipoFuncaoDecaimento.Gaussiana;
                        if (this.rdbDecaimentoLinear.Checked) m_array_missing_vars[variavel].TipoFuncaoDecaimento = TipoFuncaoDecaimento.Linear;
                        if (this.rdbDecaimentoQuadratico.Checked) m_array_missing_vars[variavel].TipoFuncaoDecaimento = TipoFuncaoDecaimento.Quadratico;
                        if (this.rdbDecaimentoUniforme.Checked) m_array_missing_vars[variavel].TipoFuncaoDecaimento = TipoFuncaoDecaimento.Uniforme;

                        //===================== especificando o tipo de contiguidade
                        if (rdbQueen.Checked) m_array_missing_vars[variavel].TipoContiguidade = TipoContiguidade.Queen;
                        if (this.rdbRook.Checked) m_array_missing_vars[variavel].TipoContiguidade = TipoContiguidade.Rook;

                        //===================== especificando o tipo de imputacao via vizinhos mais próximos
                        if (this.rdbVizinhancaFuncaoDecaimento.Checked) m_array_missing_vars[variavel].TipoVizinhancaParaImputacao = TipoImputacaoVizinhanca.FuncaoDecaimento;
                        if (this.rdbVizinhosMaisProximos.Checked) m_array_missing_vars[variavel].TipoVizinhancaParaImputacao = TipoImputacaoVizinhanca.VizinhosMaisProximos;
                        if (this.rdbVizinhancaContiguidade.Checked) m_array_missing_vars[variavel].TipoVizinhancaParaImputacao = TipoImputacaoVizinhanca.MatrizContiguidade;
                        break;
                    case TipoImputacaoNumMissing.MediaCategoria:
                        if (userControlSelecaoVariaveis3.VariaveisIndependentes.GetLength(0) > 0)
                        {
                            m_array_missing_vars[variavel].NomesVariaveisCategoricas = userControlSelecaoVariaveis3.VariaveisIndependentes;
                        }
                        else
                        {
                            m_array_missing_vars[variavel].NomesVariaveisCategoricas = new string[0];
                        }
                        break;
                    case TipoImputacaoNumMissing.MediaGeral:
                        break;
                    case TipoImputacaoNumMissing.ValorFixo:
                        m_array_missing_vars[variavel].ValorFixo = Convert.ToDouble(nudImputacaoValorFixo.Value);
                        break;
                    case TipoImputacaoNumMissing.Zero:
                        break;
                    default:
                        break;
                }

                userControlRichTextOutput3.Texto = m_array_missing_vars[variavel].ToText;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region funções auxiliares

        private void RemoverTabPage(TabPage tp)
        {
            if (this.tabControl1.TabPages.Contains(tp))
                this.tabControl1.TabPages.Remove(tp);
        }

        private void IncluirTabPage(TabPage tp, bool seleciona_tabpage)
        {
            if (!this.tabControl1.TabPages.Contains(tp))
            {
                this.tabControl1.TabPages.Add(tp);
            }
            if (seleciona_tabpage)
            {
                this.tabControl1.SelectedTab = tp;
            }
        }

        private bool m_form_iniciado = false;

        private void EnableBotaoOpcoes()
        {
            if (btnImputacaoMediaSubGrupo.Checked)
            {
                btnImputacaoOpcoes.Enabled = true;
            }
            if (rdbImputacaoMediaVizinhos.Checked) btnImputacaoOpcoes.Enabled = true;
            if (rdbImputacaoMediaGeral.Checked)
            {
                this.RemoverTabPage(tabPage7);
                this.RemoverTabPage(tabPage8);
                btnImputacaoOpcoes.Enabled = false;
            }
            if (rdbImputacaoValorFixo.Checked)
            {
                this.RemoverTabPage(tabPage7);
                this.RemoverTabPage(tabPage8);
                btnImputacaoOpcoes.Enabled = false;
            }
            if (rdbImputacaoZero.Checked)
            {
                this.RemoverTabPage(tabPage7);
                this.RemoverTabPage(tabPage8);
                btnImputacaoOpcoes.Enabled = false;
            }
        }

        private void AjustaMaximasCoordenadas()
        {
            m_alteracao_prop_dist = false;
            m_alteracao_max_dist = false;

            BLModelosCrossSectionSpaciaisLineares gmm = new BLModelosCrossSectionSpaciaisLineares();
            BLMatrizDistanciasParametricas blp = new BLMatrizDistanciasParametricas();

            double xmax = gmm.GeraMaximaDistancia(this.m_dt_dados, this.lstCoordenadasX.SelectedItem.ToString());
            double ymax = gmm.GeraMaximaDistancia(this.m_dt_dados, this.lstCoordenadasY.SelectedItem.ToString());
            double dmax = blp.GeraMaxDistancia(this.m_dt_dados,
                this.lstCoordenadasX.SelectedItem.ToString(), this.lstCoordenadasY.SelectedItem.ToString());

            clsUtilTools clt = new clsUtilTools();

            lblMaxCoordenadaX.Text = clt.Double2Texto(xmax, 8);
            lblMaximaCoordenadaY.Text = clt.Double2Texto(ymax, 8);
            lblMaxDistEuclidiana.Text = clt.Double2Texto(dmax, 8);

            nudDistEuclidianaMaxima.Maximum = Convert.ToDecimal(dmax);
            nudDistEuclidianaMaxima.Value = Convert.ToDecimal(dmax * Convert.ToDouble(this.nudPropDistMaxima.Value) / 100.0);

            m_max_distancia_Euclidiana = dmax;

            m_alteracao_prop_dist = true;
            m_alteracao_max_dist = true;
        }

        private bool m_alteracao_prop_dist = false;
        private bool m_alteracao_max_dist = false;
        private double m_max_distancia_Euclidiana = 0.0;

        private void AjusteAutomaticoParametrosDecaimento()
        {
            if (rdbDecaimentoUniforme.Checked)
            {
                this.nudParm1FuncaoDecaimento.Value = Convert.ToDecimal(1.0);
                this.nudParm1FuncaoDecaimento.Enabled = false;
            }
            else
            {
                this.nudParm1FuncaoDecaimento.Enabled = true;
                if (rdbDecaimentoExponencial.Checked)
                {
                    double p;
                    if (Convert.ToDouble(this.nudDistEuclidianaMaxima.Value) > 0.0)
                    {
                        p = Math.Log(10.0) / Convert.ToDouble(this.nudDistEuclidianaMaxima.Value);
                    }
                    else
                    {
                        p = 1.0;
                    }
                    this.nudParm1FuncaoDecaimento.Value = Convert.ToDecimal(p);
                }
                if (rdbDecaimentoLinear.Checked)
                {
                    this.nudParm1FuncaoDecaimento.Value = this.nudDistEuclidianaMaxima.Value;
                }
                if (this.rdbDecaimentoQuadratico.Checked)
                {
                    this.nudParm1FuncaoDecaimento.Value = this.nudDistEuclidianaMaxima.Value;
                }
                if (this.rdbDecaimentoGaussiano.Checked)
                {
                    this.nudParm1FuncaoDecaimento.Value = Convert.ToDecimal(Convert.ToDouble(this.nudDistEuclidianaMaxima.Value) / 2.0);
                }
            }
        }

        #endregion

        #region controles

        private bool m_posicao_anterior_btnOk;
        private bool m_posicao_anterior_btnRelatorio;
        private bool m_posicao_anterior_btnConcatenar;

        private void ckbUtilizarApenasDadosShape_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckbUtilizarApenasDadosShape.Checked)
                {
                    ckbAnexarDadosShape.Checked = false;
                    groupBox9.Enabled = false;

                    cmbTabelasNoArquivo.Enabled = btnAbrirTabelaDados.Enabled
                        = ckbAnexarDadosShape.Enabled = btnImportarTabela.Enabled = false;

                    m_posicao_anterior_btnRelatorio = btnRelatorio.Enabled;
                    m_posicao_anterior_btnOk = btnOK.Enabled;
                    m_posicao_anterior_btnConcatenar = btnConcatenarTabelas.Enabled;

                    btnOK.Enabled = true;
                    btnConcatenarTabelas.Enabled = false;
                    btnRelatorio.Enabled = false;

                    MessageBox.Show("Escolha uma coluna da tabela do arquivo shape, que será utilizada como variável identificadora.",
                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cmbTabelasNoArquivo.Enabled = btnAbrirTabelaDados.Enabled
                        = ckbAnexarDadosShape.Enabled = btnImportarTabela.Enabled = true;

                    ckbAnexarDadosShape.Checked = false;
                    groupBox9.Enabled = true;

                    btnOK.Enabled = m_posicao_anterior_btnOk;
                    btnConcatenarTabelas.Enabled = m_posicao_anterior_btnConcatenar;
                    btnRelatorio.Enabled = m_posicao_anterior_btnRelatorio;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ckbDelimitadoTab_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbDelimitadoTab.Checked)
            {
                ckbDelimitadoTab.Checked = true;
                ckbDelimitadoPontoVirgula.Checked = false;
                ckbDelimitadoVirgula.Checked = false;
                ckbDelimitadoCaracter.Checked = false;

                txtCaracterDelimitacao.Enabled = false;
            }
            AjustaDelimitacao();
        }

        private void AjustaDelimitacao()
        {
            try
            {
                if (ckbDelimitadoCaracter.Checked || ckbDelimitadoPontoVirgula.Checked ||
                    ckbDelimitadoTab.Checked || ckbDelimitadoVirgula.Checked)
                {
                    cmbTabelasNoArquivo.Enabled = false;
                    ckbNomesPrimeiraLinha.Enabled = true;
                    ckbNomesPrimeiraLinha.Checked = true;
                }
                else
                {
                    if (strExtensao == ".TXT" || strExtensao == ".DAT")
                    {
                        ckbDelimitadoTab.Checked = true;
                    }
                    else
                    {
                        cmbTabelasNoArquivo.Enabled = true;
                        txtCaracterDelimitacao.Enabled = false;
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ckbDelimitadoVirgula_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbDelimitadoVirgula.Checked)
            {
                ckbDelimitadoTab.Checked = false;
                ckbDelimitadoPontoVirgula.Checked = false;
                ckbDelimitadoVirgula.Checked = true;
                ckbDelimitadoCaracter.Checked = false;

                txtCaracterDelimitacao.Enabled = false;
            }
            AjustaDelimitacao();
        }

        private void ckbDelimitadoPontoVirgula_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbDelimitadoPontoVirgula.Checked)
            {
                ckbDelimitadoTab.Checked = false;
                ckbDelimitadoPontoVirgula.Checked = true;
                ckbDelimitadoVirgula.Checked = false;
                ckbDelimitadoCaracter.Checked = false;

                txtCaracterDelimitacao.Enabled = false;
            }
            AjustaDelimitacao();
        }

        private void ckbDelimitadoCaracter_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckbDelimitadoCaracter.Checked)
            {
                ckbDelimitadoTab.Checked = false;
                ckbDelimitadoPontoVirgula.Checked = false;
                ckbDelimitadoVirgula.Checked = false;
                ckbDelimitadoCaracter.Checked = true;

                txtCaracterDelimitacao.Enabled = true;
            }
            AjustaDelimitacao();
        }

        #endregion

        #region eventos

        private void cmbVariavelPeriodoPainelEspacial_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                string nome_var_periodo = cmbVariavelPeriodoPainelEspacial.SelectedItem.ToString();

                string mensagem = "";
                m_ds_dados_painel_espacial = new DataSet();
                string[] ids_unicos = new string[0];

                BLConcatenacaoArquivos bla = new BLConcatenacaoArquivos();

                object[,] freqs = bla.FrequenciasTabelaDadosPainelEspacial(this.m_dt_dados, nome_var_periodo, m_dt_shape, ref mensagem, ref m_ds_dados_painel_espacial, ref ids_unicos);

                m_freqs_periodos_painel = freqs;
                m_variavel_periodos_painel = nome_var_periodo;

                listBox1.Items.Clear();
                listBox1.Items.AddRange(ids_unicos);
                //listBox1.SelectedIndex = 0; 

                if (mensagem != "")
                {
                    MessageBox.Show(mensagem, "Problemas na seleção na variável de período", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                btnOK.Enabled = false;
                btnConcatenarTabelas.Enabled = true;

                try
                {
                    string[] lista1 = new string[listBox1.Items.Count];
                    for (int i = 0; i < listBox1.Items.Count; i++) lista1[i] = listBox1.Items[i].ToString();

                    clsUtilTools clt = new clsUtilTools();

                    string[] lista2 = new string[listBox2.Items.Count];
                    for (int i = 0; i < listBox2.Items.Count; i++)
                    {
                        lista2[i] = listBox2.Items[i].ToString();
                        if (clt.ChecaStringEmLista(lista2[i], lista1))
                        {
                            listBox1.SelectedItem = lista2[i];
                            listBox2.SelectedItem = lista2[i];
                            break;
                        }
                    }
                }
                catch
                {
                }

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Problemas na seleção na variável de período", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FormConcatenacaoTabelaToShape_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                System.GC.Collect();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void rdbArquivosExcelMDB_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdbArquivosExcelMDB.Checked)
                {
                    cmbTabelasNoArquivo.Enabled = true;

                    ckbDelimitadoCaracter.Enabled =
                        ckbDelimitadoPontoVirgula.Enabled =
                        ckbDelimitadoTab.Enabled =
                        ckbDelimitadoVirgula.Enabled =
                        ckbNomesPrimeiraLinha.Enabled =
                        ckbFormatoNumeroPortugues.Enabled = false;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void rdbArquivosTXT_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rdbArquivosTXT.Checked)
                {
                    cmbTabelasNoArquivo.Enabled = false;

                    ckbDelimitadoCaracter.Enabled =
                        ckbDelimitadoPontoVirgula.Enabled =
                        ckbDelimitadoTab.Enabled =
                        ckbDelimitadoVirgula.Enabled =
                        ckbNomesPrimeiraLinha.Enabled =
                        ckbFormatoNumeroPortugues.Enabled = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void rdbArquivosSAS_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rdbArquivosSAS.Checked)
                {
                    cmbTabelasNoArquivo.Enabled = false;

                    ckbDelimitadoCaracter.Enabled =
                        ckbDelimitadoPontoVirgula.Enabled =
                        ckbDelimitadoTab.Enabled =
                        ckbDelimitadoVirgula.Enabled =
                        ckbNomesPrimeiraLinha.Enabled =
                        ckbFormatoNumeroPortugues.Enabled = false;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        //olhar se é necessario 
        //public void SalvarXML()
        //{
        //    Classes.clsArmazenamentoDados salvar = new Classes.clsArmazenamentoDados();

        //    if (xml_concat[0] != null && xml_concat[1] != null && xml_concat[2] != null)
        //    {
        //        salvar.CapturaDados(xml_concat[0], xml_concat[1], xml_concat[2]);
        //    }
        //}

        public void LerXML()
        {
            FormConcatenacaoTabelaToShape_Load(this, new EventArgs());
            btnImportarTabela_Click(this, new EventArgs());
            btnConcatenarTabelas_Click(this, new EventArgs());
            btnOK_Click(this, new EventArgs());
        }

        ////olhar se é necessario
        //public void ClearXML()
        //{
        //    xml_concat = null;
        //}
    }
}