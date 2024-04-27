using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;

namespace IpeaGeo.RegressoesEspaciais
{
    public partial class FormAreasMinimasComparaveis : Form
    {
        #region variáveis internas

        private DataTable m_dt_tabela_dados = new DataTable();
        public DataTable TabelaDeDados
        {
            get { return this.m_dt_tabela_dados; }
            set
            {
                this.m_dt_tabela_dados = value;

                AtualizaTabelaDados();

                if (value.Rows.Count > 0 && value.Columns.Count > 0)
                {
                    this.btnExecutar.Enabled = true;
                }
            }
        }

        private void AtualizaTabelaDados()
        {
            try
            {
                this.userControlDataGrid1.TabelaDados = m_dt_tabela_dados;
                this.userControlDataGrid1.ColumnsWidth(180);

                clsUtilTools clt = new clsUtilTools();

                //this.userControlRegressaoInstrumentos1.VariaveisDB = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
                //this.userControlRegressaoInstrumentos1.VariaveisList = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);

                if (!this.tabControl1.TabPages.Contains(tabPage2))
                {
                    tabControl1.TabPages.Add(tabPage2);
                }

                string[] variaveis = clt.RetornaTodasColunas(m_dt_tabela_dados);
                string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);
                string[] variaveis_unicas = clt.RetornaUniqueColunas(m_dt_tabela_dados);

                if (variaveis_unicas.GetLength(0) <= 0)
                {
                    throw new Exception("Não há variáveis de identificação única na tabela de dados importada. A tabela importada deve ter uma coluna de identificação única do município, de acordo com os códigos do IBGE.");
                }

                cmbIdMunicipio.Items.Clear();
                cmbIdMunicipio.Items.AddRange(variaveis_unicas);
                cmbIdMunicipio.SelectedIndex = 0;

                cmbVariavelPesoDefault.Items.Clear();
                cmbVariavelPesoDefault.Items.Add("População do município");
                cmbVariavelPesoDefault.Items.AddRange(variaveis_numericas);
                cmbVariavelPesoDefault.SelectedIndex = 0;

                lstVariaveisDisponiveis.Items.Clear();
                lstVariaveisDisponiveis.Items.AddRange(variaveis);
                lstVariaveisDisponiveis.SelectedIndex = 0;

                string[] tipos_agregacao = new string[] { "Soma", "Média simples", 
                "Média ponderada", "Máximo", "Mínimo", "Primeiro ordem dos dados", "Último ordem dos dados", "Primeiro ordem alfabética", 
                "Último ordem alfabética", "Primeiro ordem ponderação", "Último ordem ponderação" };

                //string[] tipos_agregacao = new string[] { "Soma", "Média simples", 
                //    "Média ponderada", "Máximo", "Mínimo" };

                TipoAgregacao.Items.Clear();
                TipoAgregacao.Items.AddRange(tipos_agregacao);

                this.VariavelPonderacao.Items.Clear();
                this.VariavelPonderacao.Items.Add("População do município");
                this.VariavelPonderacao.Items.AddRange(variaveis_numericas);

                m_variaveis_originais.Clear();
                m_variaveis_disponiveis.Clear();
                m_variaveis_selecionadas.Clear();

                for (int i = 0; i < variaveis.GetLength(0); i++)
                {
                    m_variaveis_disponiveis.Add(variaveis[i]);
                    m_variaveis_originais.Add(variaveis[i]);
                }

                string[] anos_amcs = new string[] { "1970", "1991", "1997" };

                cmbAnoAMC.Items.Clear();
                cmbAnoAMC.Items.AddRange(anos_amcs);
                cmbAnoAMC.SelectedItem = "1997";

                btnExecutar.Enabled = true;
            }
            catch (Exception er)
            {
                cmbIdMunicipio.Items.Clear();
                cmbVariavelPesoDefault.Items.Clear();
                lstVariaveisDisponiveis.Items.Clear();
                this.VariavelPonderacao.Items.Clear();

                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region importação dos arquivos de depara

        private System.Data.OleDb.OleDbConnection m_cnn = new System.Data.OleDb.OleDbConnection();
        private System.Data.OleDb.OleDbDataAdapter m_dap = new System.Data.OleDb.OleDbDataAdapter();
        private DataSet dsDados = new DataSet();

        private DataTable m_dt_depara_mun70_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun80_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun91_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun93_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun97_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun98_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun99_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun00_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun01_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun02_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun03_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun04_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun05_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun06_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun07_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun08_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun09_to_amc70 = new DataTable();
        private DataTable m_dt_depara_mun10_to_amc70 = new DataTable();

        private DataTable m_dt_depara_mun91_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun93_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun97_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun98_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun99_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun00_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun01_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun02_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun03_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun04_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun05_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun06_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun07_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun08_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun09_to_amc91 = new DataTable();
        private DataTable m_dt_depara_mun10_to_amc91 = new DataTable();

        private DataTable m_dt_depara_mun97_to_amc97 = new DataTable();
        private DataTable m_dt_depara_mun98_to_amc97 = new DataTable();
        private DataTable m_dt_depara_mun99_to_amc97 = new DataTable();
        private DataTable m_dt_depara_mun00_to_amc97 = new DataTable();
        private DataTable m_dt_depara_mun01_to_amc97 = new DataTable();
        private DataTable m_dt_depara_mun02_to_amc97 = new DataTable();
        private DataTable m_dt_depara_mun03_to_amc97 = new DataTable();
        private DataTable m_dt_depara_mun04_to_amc97 = new DataTable();
        private DataTable m_dt_depara_mun05_to_amc97 = new DataTable();
        private DataTable m_dt_depara_mun06_to_amc97 = new DataTable();
        private DataTable m_dt_depara_mun07_to_amc97 = new DataTable();
        private DataTable m_dt_depara_mun08_to_amc97 = new DataTable();
        private DataTable m_dt_depara_mun09_to_amc97 = new DataTable();
        private DataTable m_dt_depara_mun10_to_amc97 = new DataTable();

        private string VariavelIdMunDtAmcs(int ano_municipio)
        {
            if (ano_municipio == 1970) return "codmun70";
            if (ano_municipio == 1980) return "codmun80";
            if (ano_municipio == 1991) return "codmun91";
            if (ano_municipio == 1993) return "codmun93";
            if (ano_municipio == 1997) return "codmun97";
            if (ano_municipio == 1998) return "codmun98";
            if (ano_municipio == 1999) return "codmun99";
            if (ano_municipio == 2000) return "codmun00";
            if (ano_municipio == 2001) return "codmun01";
            if (ano_municipio == 2002) return "codmun02";
            if (ano_municipio == 2003) return "codmun03";
            if (ano_municipio == 2004) return "codmun04";
            if (ano_municipio == 2005) return "codmun05";
            if (ano_municipio == 2006) return "codmun06";
            if (ano_municipio == 2007) return "codmun07";
            if (ano_municipio == 2008) return "codmun08";
            if (ano_municipio == 2009) return "codmun09";
            if (ano_municipio == 2010) return "codmun10";

            return "codmun70";
        }

        private DataTable TabelaDepara(int ano_amc, int ano_municipio)
        {
            if (ano_amc == 1970 && ano_municipio == 1970) return m_dt_depara_mun70_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 1980) return m_dt_depara_mun80_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 1991) return m_dt_depara_mun91_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 1993) return m_dt_depara_mun93_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 1997) return m_dt_depara_mun97_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 1998) return m_dt_depara_mun98_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 1999) return m_dt_depara_mun99_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 2000) return m_dt_depara_mun00_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 2001) return m_dt_depara_mun01_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 2002) return m_dt_depara_mun02_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 2003) return m_dt_depara_mun03_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 2004) return m_dt_depara_mun04_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 2005) return m_dt_depara_mun05_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 2006) return m_dt_depara_mun06_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 2007) return m_dt_depara_mun07_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 2008) return m_dt_depara_mun08_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 2009) return m_dt_depara_mun09_to_amc70;
            if (ano_amc == 1970 && ano_municipio == 2010) return m_dt_depara_mun10_to_amc70;

            if (ano_amc == 1991 && ano_municipio == 1991) return m_dt_depara_mun91_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 1993) return m_dt_depara_mun93_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 1997) return m_dt_depara_mun97_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 1998) return m_dt_depara_mun98_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 1999) return m_dt_depara_mun99_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 2000) return m_dt_depara_mun00_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 2001) return m_dt_depara_mun01_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 2002) return m_dt_depara_mun02_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 2003) return m_dt_depara_mun03_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 2004) return m_dt_depara_mun04_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 2005) return m_dt_depara_mun05_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 2006) return m_dt_depara_mun06_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 2007) return m_dt_depara_mun07_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 2008) return m_dt_depara_mun08_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 2009) return m_dt_depara_mun09_to_amc91;
            if (ano_amc == 1991 && ano_municipio == 2010) return m_dt_depara_mun10_to_amc91;

            if (ano_amc == 1997 && ano_municipio == 1997) return m_dt_depara_mun97_to_amc97;
            if (ano_amc == 1997 && ano_municipio == 1998) return m_dt_depara_mun98_to_amc97;
            if (ano_amc == 1997 && ano_municipio == 1999) return m_dt_depara_mun99_to_amc97;
            if (ano_amc == 1997 && ano_municipio == 2000) return m_dt_depara_mun00_to_amc97;
            if (ano_amc == 1997 && ano_municipio == 2001) return m_dt_depara_mun01_to_amc97;
            if (ano_amc == 1997 && ano_municipio == 2002) return m_dt_depara_mun02_to_amc97;
            if (ano_amc == 1997 && ano_municipio == 2003) return m_dt_depara_mun03_to_amc97;
            if (ano_amc == 1997 && ano_municipio == 2004) return m_dt_depara_mun04_to_amc97;
            if (ano_amc == 1997 && ano_municipio == 2005) return m_dt_depara_mun05_to_amc97;
            if (ano_amc == 1997 && ano_municipio == 2006) return m_dt_depara_mun06_to_amc97;
            if (ano_amc == 1997 && ano_municipio == 2007) return m_dt_depara_mun07_to_amc97;
            if (ano_amc == 1997 && ano_municipio == 2008) return m_dt_depara_mun08_to_amc97;
            if (ano_amc == 1997 && ano_municipio == 2009) return m_dt_depara_mun09_to_amc97;
            if (ano_amc == 1997 && ano_municipio == 2010) return m_dt_depara_mun10_to_amc97;

            return m_dt_depara_mun70_to_amc70;
        }

        private void ImportarArquivoDepara(string nome_arquivo, string nome_planilha, ref DataTable dt)
        {
            dsDados = new DataSet();

            string diretorio_arquivos = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            string arquivo = diretorio_arquivos + @"\bases\DeparaAMCs\" + nome_arquivo;

            string m_cnnstring = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + arquivo + ";Extended Properties=Excel 8.0;";
            m_cnn = new System.Data.OleDb.OleDbConnection(m_cnnstring);
            m_cnn.Open();

            System.Data.DataTable dtt = null;

            dtt = m_cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            m_dap = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" + nome_planilha + "$]", m_cnn);
            m_dap.Fill(dsDados, "Table1");

            m_cnn.Close();
            dt = dsDados.Tables["Table1"].Copy();
        }

        private bool m_AMC97_imported = false;
        private void ImportarTodosDeparasAMC97()
        {
            try
            {
                if (!m_AMC97_imported)
                {
                    Cursor = Cursors.WaitCursor;

                    ImportarArquivoDepara("TABELA_AMC97_MUN97.xls", "AMC97_TO_97_NOVO", ref m_dt_depara_mun97_to_amc97);
                    ImportarArquivoDepara("TABELA_AMC97_MUN98.xls", "AMC97_TO_98_NOVO", ref m_dt_depara_mun98_to_amc97);
                    ImportarArquivoDepara("TABELA_AMC97_MUN99.xls", "AMC97_TO_99_NOVO", ref m_dt_depara_mun99_to_amc97);
                    ImportarArquivoDepara("TABELA_AMC97_MUN00.xls", "AMC97_TO_00_NOVO", ref m_dt_depara_mun00_to_amc97);
                    ImportarArquivoDepara("TABELA_AMC97_MUN01.xls", "AMC97_TO_01_NOVO", ref m_dt_depara_mun01_to_amc97);
                    ImportarArquivoDepara("TABELA_AMC97_MUN02.xls", "AMC97_TO_02_NOVO", ref m_dt_depara_mun02_to_amc97);
                    ImportarArquivoDepara("TABELA_AMC97_MUN03.xls", "AMC97_TO_03_NOVO", ref m_dt_depara_mun03_to_amc97);
                    ImportarArquivoDepara("TABELA_AMC97_MUN04.xls", "AMC97_TO_04_NOVO", ref m_dt_depara_mun04_to_amc97);
                    ImportarArquivoDepara("TABELA_AMC97_MUN05.xls", "AMC97_TO_05_NOVO", ref m_dt_depara_mun05_to_amc97);
                    ImportarArquivoDepara("TABELA_AMC97_MUN06.xls", "AMC97_TO_06_NOVO", ref m_dt_depara_mun06_to_amc97);
                    ImportarArquivoDepara("TABELA_AMC97_MUN07.xls", "AMC97_TO_07_NOVO", ref m_dt_depara_mun07_to_amc97);
                    ImportarArquivoDepara("TABELA_AMC97_MUN08.xls", "AMC97_TO_08_NOVO", ref m_dt_depara_mun08_to_amc97);
                    ImportarArquivoDepara("TABELA_AMC97_MUN09.xls", "AMC97_TO_09_NOVO", ref m_dt_depara_mun09_to_amc97);
                    ImportarArquivoDepara("TABELA_AMC97_MUN10.xls", "AMC97_TO_10_NOVO", ref m_dt_depara_mun10_to_amc97);

                    m_AMC97_imported = true;

                    Cursor = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool m_AMC91_imported = false;
        private void ImportarTodosDeparasAMC91()
        {
            try
            {
                if (!m_AMC91_imported)
                {
                    Cursor = Cursors.WaitCursor;

                    ImportarArquivoDepara("TABELA_AMC91_MUN91.xls", "AMC91_TO_91_NOVO", ref m_dt_depara_mun91_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN93.xls", "AMC91_TO_93_NOVO", ref m_dt_depara_mun93_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN97.xls", "AMC91_TO_97_NOVO", ref m_dt_depara_mun97_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN98.xls", "AMC91_TO_98_NOVO", ref m_dt_depara_mun98_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN99.xls", "AMC91_TO_99_NOVO", ref m_dt_depara_mun99_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN00.xls", "AMC91_TO_00_NOVO", ref m_dt_depara_mun00_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN01.xls", "AMC91_TO_01_NOVO", ref m_dt_depara_mun01_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN02.xls", "AMC91_TO_02_NOVO", ref m_dt_depara_mun02_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN03.xls", "AMC91_TO_03_NOVO", ref m_dt_depara_mun03_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN04.xls", "AMC91_TO_04_NOVO", ref m_dt_depara_mun04_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN05.xls", "AMC91_TO_05_NOVO", ref m_dt_depara_mun05_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN06.xls", "AMC91_TO_06_NOVO", ref m_dt_depara_mun06_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN07.xls", "AMC91_TO_07_NOVO", ref m_dt_depara_mun07_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN08.xls", "AMC91_TO_08_NOVO", ref m_dt_depara_mun08_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN09.xls", "AMC91_TO_09_NOVO", ref m_dt_depara_mun09_to_amc91);
                    ImportarArquivoDepara("TABELA_AMC91_MUN10.xls", "AMC91_TO_10_NOVO", ref m_dt_depara_mun10_to_amc91);

                    m_AMC91_imported = true;

                    Cursor = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool m_AMC70_imported = false;
        private void ImportarTodosDeparasAMC70()
        {
            try
            {
                if (!m_AMC70_imported)
                {
                    Cursor = Cursors.WaitCursor;

                    ImportarArquivoDepara("TABELA_AMC70_MUN70.xls", "AMC70_TO_70_NOVO", ref m_dt_depara_mun70_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN80.xls", "AMC70_TO_80_NOVO", ref m_dt_depara_mun80_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN91.xls", "AMC70_TO_91_NOVO", ref m_dt_depara_mun91_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN93.xls", "AMC70_TO_93_NOVO", ref m_dt_depara_mun93_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN97.xls", "AMC70_TO_97_NOVO", ref m_dt_depara_mun97_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN98.xls", "AMC70_TO_98_NOVO", ref m_dt_depara_mun98_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN99.xls", "AMC70_TO_99_NOVO", ref m_dt_depara_mun99_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN00.xls", "AMC70_TO_00_NOVO", ref m_dt_depara_mun00_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN01.xls", "AMC70_TO_01_NOVO", ref m_dt_depara_mun01_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN02.xls", "AMC70_TO_02_NOVO", ref m_dt_depara_mun02_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN03.xls", "AMC70_TO_03_NOVO", ref m_dt_depara_mun03_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN04.xls", "AMC70_TO_04_NOVO", ref m_dt_depara_mun04_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN05.xls", "AMC70_TO_05_NOVO", ref m_dt_depara_mun05_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN06.xls", "AMC70_TO_06_NOVO", ref m_dt_depara_mun06_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN07.xls", "AMC70_TO_07_NOVO", ref m_dt_depara_mun07_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN08.xls", "AMC70_TO_08_NOVO", ref m_dt_depara_mun08_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN09.xls", "AMC70_TO_09_NOVO", ref m_dt_depara_mun09_to_amc70);
                    ImportarArquivoDepara("TABELA_AMC70_MUN10.xls", "AMC70_TO_10_NOVO", ref m_dt_depara_mun10_to_amc70);

                    m_AMC70_imported = true;

                    Cursor = Cursors.Default;
                }
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region inicialização e fechamento do formulário 

        public FormAreasMinimasComparaveis()
        {
            InitializeComponent();
            int a = dataGridView3.Rows.Count;
            dataGridView3.Rows[a - 1].ReadOnly = true;
        }
        private void FormAreasMinimasComparaveis_Load(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.TabPages.Contains(tabPage2)) tabControl1.TabPages.Remove(tabPage2);
                if (tabControl1.TabPages.Contains(tabPage3)) tabControl1.TabPages.Remove(tabPage3);
                if (tabControl1.TabPages.Contains(tabPage4)) tabControl1.TabPages.Remove(tabPage4);
                if (tabControl1.TabPages.Contains(tabPage5)) tabControl1.TabPages.Remove(tabPage5);
                if (tabControl1.TabPages.Contains(tabPage6)) tabControl1.TabPages.Remove(tabPage6);

                string[] lista_amcs = new string[] { "1970", "1991", "1997" };
                cmbAnoAMC.Items.Clear();
                cmbAnoAMC.Items.AddRange(lista_amcs);
                cmbAnoAMC.SelectedItem = "1997";
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region funções de manipulação da escolha das variáveis

        private ArrayList m_variaveis_originais = new ArrayList();
        private ArrayList m_variaveis_selecionadas = new ArrayList();
        private ArrayList m_variaveis_disponiveis = new ArrayList();

        private void AdicionaVariavel(string[] nomes_variaveis)
        {
            string nome_variavel = "";
            object[] nova_linha = new object[4];

            for (int i = 0; i < nomes_variaveis.GetLength(0); i++)
            {
                nome_variavel = nomes_variaveis[i];

                m_variaveis_selecionadas.Add(nome_variavel);
                m_variaveis_disponiveis.Remove(nome_variavel);

                lstVariaveisDisponiveis.Items.Remove(nome_variavel);

                nova_linha = new object[4];
                nova_linha[0] = nome_variavel;
                if (this.m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(double)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(decimal)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(Decimal)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(float)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(uint)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(Double)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(int)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(Int16)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(Int32)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(Int64))
                {
                    nova_linha[1] = "Soma";
                }
                else
                {
                    nova_linha[1] = "Primeiro ordem dos dados";
                }
                nova_linha[2] = cmbVariavelPesoDefault.SelectedItem.ToString();
                nova_linha[3] = cmbAnoDefaultTabelaDados.SelectedItem.ToString();

                dataGridView3.Rows.Add(nova_linha);
                dataGridView3.Refresh();

            }
        }

        private void RemoverVariavel(string[] nomes_variaveis)
        {
            string nome_variavel = "";
            ArrayList rows = new ArrayList();

            for (int i = 0; i < nomes_variaveis.GetLength(0); i++)
            {
                nome_variavel = nomes_variaveis[i];

                m_variaveis_selecionadas.Remove(nome_variavel);
                m_variaveis_disponiveis.Add(nome_variavel);
                lstVariaveisDisponiveis.Items.Add(nome_variavel);

                for (int k = 0; k < dataGridView3.Rows.Count - 1; k++)
                {
                    if (nome_variavel == dataGridView3.Rows[k].Cells[0].Value.ToString())
                    {
                        rows.Add(k);
                    }
                }
            }

            for (int i = rows.Count - 1; i >= 0; i--)
            {
                dataGridView3.Rows.RemoveAt(Convert.ToInt32(rows[i]));
            }
        }

        private void btnAddVariavelIndependente_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_variaveis_disponiveis.Count > 0
                    && lstVariaveisDisponiveis.SelectedItems.Count <= 0) throw new Exception("Selecione uma variável para ser adicionada.");

                string[] variaveis_selecionadas = new string[lstVariaveisDisponiveis.SelectedItems.Count];
                for (int i = 0; i < variaveis_selecionadas.GetLength(0); i++)
                {
                    variaveis_selecionadas[i] = lstVariaveisDisponiveis.SelectedItems[i].ToString();
                }

                AdicionaVariavel(variaveis_selecionadas);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRemoveVariavelIndependente_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView3.SelectedRows.Count >= 0)
                {
                    ArrayList rows = new ArrayList();

                    for (int i = 0; i < dataGridView3.Rows.Count-1; i++)
                    {
                        if (dataGridView3.SelectedRows.Contains(dataGridView3.Rows[i]))
                        {
                            rows.Add(dataGridView3.Rows[i].Cells[0].Value.ToString());
                        }
                    }

                    string[] remover = new string[rows.Count];
                    for (int i = 0; i < remover.GetLength(0); i++)
                    {
                        remover[i] = rows[i].ToString();
                    }
                    RemoverVariavel(remover);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAddAllVariaveisIndependentes_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_variaveis_disponiveis.Count > 0)
                {
                    string[] variaveis_selecionadas = new string[lstVariaveisDisponiveis.Items.Count];
                    for (int i = 0; i < variaveis_selecionadas.GetLength(0); i++)
                    {
                        variaveis_selecionadas[i] = lstVariaveisDisponiveis.Items[i].ToString();
                    }

                    AdicionaVariavel(variaveis_selecionadas);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRemoveAllVariaveisIndependentes_Click(object sender, EventArgs e)
        {
            try
            {
                string[] remover = new string[m_variaveis_selecionadas.Count];
                for (int i = 0; i < remover.GetLength(0); i++)
                {
                    remover[i] = m_variaveis_selecionadas[i].ToString();
                }
                RemoverVariavel(remover);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LimparListaVariaveis()
        {
            try
            {
                string[] remover = new string[m_variaveis_selecionadas.Count];
                for (int i = 0; i < remover.GetLength(0); i++)
                {
                    remover[i] = m_variaveis_selecionadas[i].ToString();
                }
                RemoverVariavel(remover);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }   
        }

        #endregion

        #region executando a criação da tabela de dados

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (tabControl1.TabPages.Contains(tabPage3)) tabControl1.TabPages.Remove(tabPage3);
                if (tabControl1.TabPages.Contains(tabPage4)) tabControl1.TabPages.Remove(tabPage4);
                if (tabControl1.TabPages.Contains(tabPage5)) tabControl1.TabPages.Remove(tabPage5);
                if (tabControl1.TabPages.Contains(tabPage6)) tabControl1.TabPages.Remove(tabPage6);

                if (cmbAnoAMC.SelectedItem.ToString() == "1970") ImportarTodosDeparasAMC70();
                if (cmbAnoAMC.SelectedItem.ToString() == "1991") ImportarTodosDeparasAMC91();
                if (cmbAnoAMC.SelectedItem.ToString() == "1997") ImportarTodosDeparasAMC97();

                if (this.m_variaveis_selecionadas.Count <= 0)
                {
                    throw new Exception("Selecione as variáveis para serem criadas na nova tabela de dados.");
                }

                MontandoTabelasPorAnoDados();

                if (!this.tabControl1.TabPages.Contains(tabPage3))
                {
                    tabControl1.TabPages.Add(tabPage3);
                }
                tabControl1.SelectedTab = tabPage3;

                if (!this.tabControl1.TabPages.Contains(tabPage5))
                {
                    tabControl1.TabPages.Add(tabPage5);
                }

                userControlDataGrid2.TabelaDados = m_dt_dados_por_amc;
                this.userControlDataGrid2.ColumnsWidth(180);

                userControlDataGrid4.TabelaDados = m_dt_dados_concatenados;
                this.userControlDataGrid4.ColumnsWidth(180);

                MostraDepara(false);
                
                if (!this.tabControl1.TabPages.Contains(tabPage6))
                {
                    tabControl1.TabPages.Add(tabPage6);
                }

                userControlRichTextOutput1.Texto = "//===============================================================================================// \n\n";
                userControlRichTextOutput1.Texto += "PREPARAÇÃO DE DADOS POR ÁREAS MÍNIMAS COMPARÁVEIS \n";
                userControlRichTextOutput1.Texto += DateTime.Now.ToLongDateString() + "\n";
                userControlRichTextOutput1.Texto += DateTime.Now.ToLongTimeString() + "\n\n";

                userControlRichTextOutput1.Texto += m_mensagem.ToString();

                Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        #region Montando um dataset com as tabelas para cada ano separadamente

        private DataTable m_dt_dados_por_amc = new DataTable();
        private DataTable m_dt_dados_concatenados = new DataTable();
        private StringBuilder m_mensagem = new StringBuilder();

        private void ChecaConsistenciaTiposAgregacao()
        {            
            object[] itens = new object[4];
            for (int k = 0; k < dataGridView3.Rows.Count - 1; k++)
            {
                itens = new object[4];
                itens[0] = dataGridView3.Rows[k].Cells[0].Value.ToString();
                itens[1] = dataGridView3.Rows[k].Cells[1].Value.ToString();
                itens[2] = dataGridView3.Rows[k].Cells[2].Value.ToString();
                itens[3] = dataGridView3.Rows[k].Cells[3].Value.ToString();

                string tipo_agregacao = dataGridView3.Rows[k].Cells[1].Value.ToString();
                string nome_variavel = dataGridView3.Rows[k].Cells[0].Value.ToString();

                if (this.m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(double)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(decimal)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(Decimal)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(float)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(uint)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(Double)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(int)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(Int16)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(Int32)
                    || m_dt_tabela_dados.Columns[nome_variavel].DataType == typeof(Int64))
                {
                }
                else
                {
                    if (tipo_agregacao == "Soma" || tipo_agregacao == "Média simples"
                        || tipo_agregacao == "Média ponderada" || tipo_agregacao == "Máximo" || tipo_agregacao == "Mínimo")
                    {
                        dataGridView3.Rows[k].Cells[1].Value = "Primeiro ordem dos dados";
                        MessageBox.Show("A variável '" + nome_variavel + "' não pode ter uma agregação do tipo '" + tipo_agregacao 
                            + "'. O programa mudou o tipo de agregação para 'Primeiro ordem dos dados'", "Aviso", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void MontandoTabelasPorAnoDados()
        {
            Cursor = Cursors.WaitCursor;

            ChecaConsistenciaTiposAgregacao();

            m_mensagem = new StringBuilder();

            clsUtilTools clt = new clsUtilTools();

            BLAreasMinimasComparaveis bla = new BLAreasMinimasComparaveis();

            DataTable dt_sel_variaveis = new DataTable();
            dt_sel_variaveis.Columns.Add("NomeVariavel", typeof(string));
            dt_sel_variaveis.Columns.Add("TipoAgregacao", typeof(string));
            dt_sel_variaveis.Columns.Add("VariavelPonderacao", typeof(string));
            dt_sel_variaveis.Columns.Add("AnoDados", typeof(string));

            int ano_municipio;
            int ano_amcs = Convert.ToInt32(cmbAnoAMC.SelectedItem.ToString());
            string id_mun_dt_dados;
            string id_mun_dt_amcs;
            string tipo_agregacao;
            string variavel_ponderacao;
            string variavel_dados;
            DataTable dt_amcs = new DataTable();
            int num_pares_concatenados = 0;

            DataTable dt_resultados = new DataTable();
            DataTable dt_concatenada = new DataTable();
            
            m_dt_dados_por_amc = new DataTable();
            m_dt_dados_concatenados = new DataTable();

            m_mensagem.Append("Ano base para as AMC's: " + ano_amcs + "\n");
            m_mensagem.Append("Número de observações na tabela de dados: " + this.m_dt_tabela_dados.Rows.Count + "\n\n");

            int num_prog_bar = 0;
            int[] nsprogbar = new int[dataGridView3.Rows.Count];
            for (int k = 0; k < dataGridView3.Rows.Count - 1; k++)
            {
                ano_municipio = Convert.ToInt32(dataGridView3.Rows[k].Cells[3].Value.ToString());
                dt_amcs = TabelaDepara(ano_amcs, ano_municipio);
                if (k < dataGridView3.Rows.Count - 1)
                {
                    num_prog_bar += dt_amcs.Rows.Count;
                }
                else
                {
                    num_prog_bar += (int)Math.Floor((Convert.ToDouble(dt_amcs.Rows.Count) / 2.0));
                }
                nsprogbar[k] = dt_amcs.Rows.Count;
            }

            this.progressBar1.Maximum = num_prog_bar;
            this.progressBar1.Value = 0;

            num_prog_bar = 0;

            ArrayList lista_variaveis_agregadas = new ArrayList();

            object[] itens = new object[4];
            for (int k = 0; k < dataGridView3.Rows.Count - 1; k++)
            {
                itens = new object[4];
                itens[0] = dataGridView3.Rows[k].Cells[0].Value.ToString();
                itens[1] = dataGridView3.Rows[k].Cells[1].Value.ToString();
                itens[2] = dataGridView3.Rows[k].Cells[2].Value.ToString();
                itens[3] = dataGridView3.Rows[k].Cells[3].Value.ToString();

                dt_sel_variaveis.Rows.Add(itens);

                ano_municipio = Convert.ToInt32(dataGridView3.Rows[k].Cells[3].Value.ToString());
                id_mun_dt_dados = cmbIdMunicipio.SelectedItem.ToString();
                id_mun_dt_amcs = VariavelIdMunDtAmcs(ano_municipio);
                variavel_ponderacao = dataGridView3.Rows[k].Cells[2].Value.ToString();
                tipo_agregacao = dataGridView3.Rows[k].Cells[1].Value.ToString();
                variavel_dados = dataGridView3.Rows[k].Cells[0].Value.ToString();
                dt_amcs = TabelaDepara(ano_amcs, ano_municipio);

                if (!m_dt_dados_por_amc.Columns.Contains(variavel_dados))
                {
                    bla.GeraVariavelPorAMCs(m_dt_tabela_dados, dt_amcs, variavel_dados,
                        ano_municipio, 
                        ano_amcs, id_mun_dt_dados, id_mun_dt_amcs,
                        tipo_agregacao, variavel_ponderacao, out dt_resultados, out dt_concatenada, 
                        out num_pares_concatenados, ref this.progressBar1);

                    lista_variaveis_agregadas.Add(variavel_dados);

                    m_mensagem.Append("Nome da variável de dados para a AMC: " + variavel_dados + "\n");
                    m_mensagem.Append("Número de observações na tabela de compatibilização: " + dt_amcs.Rows.Count + "\n");
                    m_mensagem.Append("Ano da informação na variável agregada: " + ano_municipio + "\n");
                    m_mensagem.Append("Tipo de agregação: " + tipo_agregacao + "\n");
                    m_mensagem.Append("Variável de ponderação: " + variavel_ponderacao + "\n");
                    m_mensagem.Append("Número de observações associadas: " + num_pares_concatenados + "\n\n");

                    if (k > 0)
                    {
                        if ((this.m_dt_tabela_dados.Columns[variavel_dados].DataType == typeof(double)
                            || m_dt_tabela_dados.Columns[variavel_dados].DataType == typeof(decimal)
                            || m_dt_tabela_dados.Columns[variavel_dados].DataType == typeof(Decimal)
                            || m_dt_tabela_dados.Columns[variavel_dados].DataType == typeof(float)
                            || m_dt_tabela_dados.Columns[variavel_dados].DataType == typeof(uint)
                            || m_dt_tabela_dados.Columns[variavel_dados].DataType == typeof(Double)
                            || m_dt_tabela_dados.Columns[variavel_dados].DataType == typeof(int)
                            || m_dt_tabela_dados.Columns[variavel_dados].DataType == typeof(Int16)
                            || m_dt_tabela_dados.Columns[variavel_dados].DataType == typeof(Int32)
                            || m_dt_tabela_dados.Columns[variavel_dados].DataType == typeof(Int64)) 
                            && (tipo_agregacao == "Soma" || tipo_agregacao == "Média simples"
                                || tipo_agregacao == "Média ponderada" || tipo_agregacao == "Máximo" || tipo_agregacao == "Mínimo"))
                        {
                            if (!m_dt_dados_por_amc.Columns.Contains(variavel_dados))
                            {
                                m_dt_dados_por_amc.Columns.Add(variavel_dados, typeof(double));
                                for (int i = 0; i < m_dt_dados_por_amc.Rows.Count; i++)
                                {
                                    m_dt_dados_por_amc.Rows[i][variavel_dados] = dt_resultados.Rows[i][variavel_dados];
                                }
                            }

                            if (!m_dt_dados_concatenados.Columns.Contains(variavel_dados))
                            {
                                this.m_dt_dados_concatenados.Columns.Add(variavel_dados, typeof(double));
                                for (int i = 0; i < m_dt_dados_concatenados.Rows.Count; i++)
                                {
                                    m_dt_dados_concatenados.Rows[i][variavel_dados] = dt_concatenada.Rows[i][variavel_dados];
                                }
                            }
                        }
                        else
                        {
                            if (!m_dt_dados_por_amc.Columns.Contains(variavel_dados))
                            {
                                m_dt_dados_por_amc.Columns.Add(variavel_dados, m_dt_tabela_dados.Columns[variavel_dados].DataType);
                                for (int i = 0; i < m_dt_dados_por_amc.Rows.Count; i++)
                                {
                                    m_dt_dados_por_amc.Rows[i][variavel_dados] = dt_resultados.Rows[i][variavel_dados];
                                }
                            }

                            if (!m_dt_dados_concatenados.Columns.Contains(variavel_dados))
                            {
                                this.m_dt_dados_concatenados.Columns.Add(variavel_dados, m_dt_tabela_dados.Columns[variavel_dados].DataType);
                                for (int i = 0; i < m_dt_dados_concatenados.Rows.Count; i++)
                                {
                                    m_dt_dados_concatenados.Rows[i][variavel_dados] = dt_concatenada.Rows[i][variavel_dados];
                                }
                            }
                        }
                    }
                    else
                    {
                        m_dt_dados_por_amc = dt_resultados.Copy();
                        m_dt_dados_concatenados = dt_concatenada.Copy();
                    }
                }

                num_prog_bar += nsprogbar[k];
                progressBar1.Value = num_prog_bar;
            }

            if (ckbEliminaLinhasVazinhas.Checked)
            {
                EliminaLinhasVazias(ref m_dt_dados_por_amc, lista_variaveis_agregadas);
                EliminaLinhasVazias(ref m_dt_dados_concatenados, lista_variaveis_agregadas);
            }

            this.progressBar1.Value = 0;
            Application.DoEvents();

            Cursor = Cursors.Default;
        }

        private void EliminaLinhasVazias(ref DataTable dt, ArrayList variaveis)
        {
            int[] elimina_linha = new int[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                elimina_linha[i] = 1;
                for (int k = 0; k < variaveis.Count; k++)
                {
                    if (!(dt.Rows[i][variaveis[k].ToString()] is DBNull))
                    {
                        elimina_linha[i] = 0;
                        break;
                    }
                }
            }

            int nrows = dt.Rows.Count;
            for (int i = nrows - 1; i >= 0; i--)
            {
                if (elimina_linha[i] == 1)
                {
                    dt.Rows.RemoveAt(i);
                }
            }
        }

        #endregion

        #endregion

        #region controles

        private void btnAbrirTabelaDados_Click(object sender, EventArgs e)
        {
            try
            {
                clsUtilArquivos cla = new clsUtilArquivos();
                if (cla.ImportarTabelaDados(ref m_dt_tabela_dados))
                {
                    LimparListaVariaveis();
                    AtualizaTabelaDados();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmbAnoAMC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string[] anos_dados = new string[0];
                if (cmbAnoAMC.SelectedItem.ToString() == "1970")
                {
                    anos_dados = new string[] {"1970", "1980", "1991", "1993", "1997", "1998", "1999", "2000", "2001", "2002", "2003", 
                                                "2004", "2005", "2006", "2007", "2008", "2009", "2010"};
                }

                if (cmbAnoAMC.SelectedItem.ToString() == "1991")
                {
                    anos_dados = new string[] {"1991", "1993", "1997", "1998", "1999", "2000", "2001", "2002", "2003", 
                                                "2004", "2005", "2006", "2007", "2008", "2009", "2010"};
                } 
                
                if (cmbAnoAMC.SelectedItem.ToString() == "1997")
                {
                    anos_dados = new string[] {"1997", "1998", "1999", "2000", "2001", "2002", "2003", 
                                                "2004", "2005", "2006", "2007", "2008", "2009", "2010"};
                }

                cmbAnoDefaultTabelaDados.Items.Clear();
                cmbAnoDefaultTabelaDados.Items.AddRange(anos_dados);
                cmbAnoDefaultTabelaDados.SelectedIndex = 0;

                AnoDados.Items.Clear();
                AnoDados.Items.AddRange(anos_dados);

                for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
                {
                    dataGridView3.Rows[i].Cells[3].Value = anos_dados[0];
                }

                btnMostrarDepara.Enabled = true;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmbAnoDefaultTabelaDados_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
                {
                    dataGridView3.Rows[i].Cells[3].Value = cmbAnoDefaultTabelaDados.SelectedItem.ToString();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmbVariavelPesoDefault_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView3.Rows.Count - 1; i++)
                {
                    dataGridView3.Rows[i].Cells[2].Value = this.cmbVariavelPesoDefault.SelectedItem.ToString();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region mostrando a tabela de deparas

        private void MostraDepara(bool evidencia)
        {
            if (cmbAnoAMC.SelectedItem.ToString() == "1970") ImportarTodosDeparasAMC70();
            if (cmbAnoAMC.SelectedItem.ToString() == "1991") ImportarTodosDeparasAMC91();
            if (cmbAnoAMC.SelectedItem.ToString() == "1997") ImportarTodosDeparasAMC97();

            if (!tabControl1.TabPages.Contains(tabPage4)) tabControl1.TabPages.Add(tabPage4);
            if (evidencia) tabControl1.SelectedTab = tabPage4;

            if (cmbAnoAMC.SelectedItem.ToString() == "1970")
            {
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1970")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun70_to_amc70;       
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1980")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun80_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1991")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun91_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1993")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun93_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1997")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun97_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1998")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun98_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1999")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun99_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2000")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun00_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2001")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun01_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2002")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun02_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2003")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun03_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2004")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun04_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2005")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun05_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2006")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun06_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2007")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun07_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2008")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun08_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2009")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun09_to_amc70;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2010")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun10_to_amc70;                    
                }
            }

            if (cmbAnoAMC.SelectedItem.ToString() == "1991")
            {
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1991")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun91_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1993")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun93_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1997")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun97_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1998")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun98_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1999")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun99_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2000")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun00_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2001")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun01_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2002")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun02_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2003")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun03_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2004")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun04_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2005")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun05_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2006")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun06_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2007")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun07_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2008")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun08_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2009")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun09_to_amc91;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2010")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun10_to_amc91;                    
                }
            }

            if (cmbAnoAMC.SelectedItem.ToString() == "1997")
            {
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1997")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun97_to_amc97;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1998")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun98_to_amc97;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "1999")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun99_to_amc97;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2000")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun00_to_amc97;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2001")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun01_to_amc97;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2002")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun02_to_amc97;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2003")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun03_to_amc97;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2004")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun04_to_amc97;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2005")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun05_to_amc97;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2006")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun06_to_amc97;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2007")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun07_to_amc97;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2008")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun08_to_amc97;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2009")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun09_to_amc97;                    
                }
                if (cmbAnoDefaultTabelaDados.SelectedItem.ToString() == "2010")
                {
                    userControlDataGrid3.TabelaDados = m_dt_depara_mun10_to_amc97;                    
                }
            }

            this.userControlDataGrid3.ColumnsWidth(190);        
        }

        private void btnMostrarDepara_Click(object sender, EventArgs e)
        {
            try
            {
                MostraDepara(true);
            }
            catch (Exception er)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
    }
}
