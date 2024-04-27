using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Threading;
using System.Collections;

namespace IpeaGeo.RegressoesEspaciais.UserControls
{
    public partial class UserControlDataGrid : UserControl
    {
        #region inicialização do controle

        public UserControlDataGrid()
        {
            InitializeComponent();

            DesabilitaControlesContextMenu();
        }
        
        private void UserControlDataGrid_Load(object sender, EventArgs e)
        {
            toolStripSplitButton5.Enabled = false;
            
            this.importarArquivoShapeToolStripMenuItem.Enabled =
            toolStripMenuItem4.Enabled =
                toolStripMenuItem5.Enabled = false;
            
            concatenarShapeETabelaDeDadosToolStripMenuItem.Enabled = false;
        }

        #endregion

        #region variáveis internas

        public bool HabilitarImportacaoDados
        {
            set
            {
                importarTabelaDeDadosToolStripMenuItem.Enabled = value;
                importarArquivoDeDadosToolStripMenuItem.Enabled = value;

                if (!value)
                {
                    HabilitaControlesContextMenu();
                }
            }
        }

        public bool HabilitarImportacaoShape
        {
            set
            {
                toolStripSplitButton5.Enabled = true;

                importarArquivoShapeToolStripMenuItem.Enabled =
                toolStripMenuItem4.Enabled = value;
            }
        }

        private bool m_uc_inicializado = false;

        private TabControl m_tabControl;
        public TabControl TabControl
        {
            get { return m_tabControl; }
            set { m_tabControl = value; }
        }

        private void AtualizaTabelaDados()
        {
            clsUtilTools clt = new clsUtilTools();

            if (m_usercontrol_reg_instrumentos != null)
            {
                string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);

                this.m_usercontrol_reg_instrumentos.ZeraControle();
                this.m_usercontrol_reg_instrumentos.VariaveisDB = clt.RetornaColunasNumericas((DataTable)this.dataGridView1.DataSource);
                this.m_usercontrol_reg_instrumentos.VariaveisList = clt.RetornaColunasNumericas((DataTable)this.dataGridView1.DataSource);
            }

            if (m_usercontrol_selecao2blocos_variaveis != null)
            {
                string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);

                this.m_usercontrol_selecao2blocos_variaveis.ZeraControle();
                this.m_usercontrol_selecao2blocos_variaveis.VariaveisDB = clt.RetornaColunasNumericas((DataTable)this.dataGridView1.DataSource);
                this.m_usercontrol_selecao2blocos_variaveis.VariaveisList = clt.RetornaColunasNumericas((DataTable)this.dataGridView1.DataSource);
            }

            if (m_usercontrol_selecao_variaveis != null)
            {
                string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);

                this.m_usercontrol_selecao_variaveis.ZeraControle();
                this.m_usercontrol_selecao_variaveis.VariaveisDB = clt.RetornaColunasNumericas((DataTable)this.dataGridView1.DataSource);
                this.m_usercontrol_selecao_variaveis.VariaveisList = clt.RetornaColunasNumericas((DataTable)this.dataGridView1.DataSource);
            }

            if (m_usercontrol_prop_score_matching != null)
            {
                string[] variaveis_numericas = clt.RetornaColunasNumericas(m_dt_tabela_dados);

                this.m_usercontrol_prop_score_matching.ZeraControle();
                this.m_usercontrol_prop_score_matching.VariaveisDB = clt.RetornaColunasNumericas((DataTable)this.dataGridView1.DataSource);
                this.m_usercontrol_prop_score_matching.VariaveisList = clt.RetornaColunasNumericas((DataTable)this.dataGridView1.DataSource);
            }

            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].Width = 150;
            }
        }

        private UserControlPropensityScoreMatching m_usercontrol_prop_score_matching;
        public UserControlPropensityScoreMatching UserControlPropScoreMatching
        {
            get { return m_usercontrol_prop_score_matching; }
            set { m_usercontrol_prop_score_matching = value; }
        }

        private UserControlSelecaoVariaveis m_usercontrol_selecao_variaveis;
        public UserControlSelecaoVariaveis UserControlSelecaoVariaveis
        {
            get { return m_usercontrol_selecao_variaveis; }
            set { m_usercontrol_selecao_variaveis = value; }
        }

        private UserControlRegressaoInstrumentos m_usercontrol_reg_instrumentos;
        public UserControlRegressaoInstrumentos UserControlRegInstrumentos
        {
            get { return m_usercontrol_reg_instrumentos; }
            set { m_usercontrol_reg_instrumentos = value; }
        }
        
        private UserControlSelecaoDoisBlocosVariaveis m_usercontrol_selecao2blocos_variaveis;
        public UserControlSelecaoDoisBlocosVariaveis UserControlSelecao2BlocosVariaveis
        {
            get { return m_usercontrol_selecao2blocos_variaveis; }
            set { m_usercontrol_selecao2blocos_variaveis = value; }
        }
        
        private bool m_mostra_opcao_importacao_dados = true;
        public bool MostraOpcaoImportadaoDados
        {
            set 
            { 
                m_mostra_opcao_importacao_dados = value;

                this.contextMenuStrip1.Items[0].Enabled = value;
            }
        }

        private DataTable m_dt_dados_originais = new DataTable();
        private DataTable m_dt_tabela_dados = new DataTable();

        public DataTable TabelaDados
        {
            get
            {
                return m_dt_tabela_dados;
            }
            set
            {
                m_dt_dados_originais = value.Copy();

                m_dt_tabela_dados = value;
                m_dt_tabela_dados.TableName = "Tabela de dados";
                this.dataGridView1.DataSource = m_dt_tabela_dados;

                if (m_uc_inicializado)
                {
                    this.HabilitaControlesContextMenu();
                }

                m_uc_inicializado = true;
            }
        }

        public DataGridView Datagridview
        {
            get { return dataGridView1; }
            set { dataGridView1 = value; }
        }

        #region delegates de funções dos formularios

        public delegate void FunctionConcatenacaoFromFormulario(bool parametro);
        public FunctionConcatenacaoFromFormulario m_funcao_concatenacao;
        public FunctionConcatenacaoFromFormulario FuncaoConcatenacaoFromFormulario
        {
            set { m_funcao_concatenacao = new FunctionConcatenacaoFromFormulario(value); }
        }

        public delegate void FunctionShapeFromFormulario(bool parametro);
        public FunctionShapeFromFormulario m_funcao_shape;
        public FunctionShapeFromFormulario FuncaoShapeFromFormulario
        {
            set { m_funcao_shape = new FunctionShapeFromFormulario(value); }
        }

        public delegate void FunctionFromFormulario(bool atualizaUControl);
        public FunctionFromFormulario m_funcao;
        public FunctionFromFormulario FuncaoFromFormulario
        {
            set { m_funcao = new FunctionFromFormulario(value); }
        }

        #endregion

        private string[] m_array_var_numericas = new string[0];
        private string[] m_array_var_totais = new string[0];

        public string[] ListaVarsNumericas
        {
            get { return m_array_var_numericas; }
            set { m_array_var_numericas = value; }
        }

        public string[] ListaVarsTotais
        {
            get { return m_array_var_totais; }
            set { m_array_var_totais = value; }
        }

        public void ColumnsWidthAutoFitAllCells()
        {
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        public void ColumnsWidthAutoFitHeader()
        {
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            }
        }

        public void ColumnsWidth(int w)
        {
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Width = w;
            }
        }

        private string m_nome_arquivo_importado = "";
        private string m_path_arquivo_importado = "";

        public string NomeArquivoImportado
        {
            get { return m_nome_arquivo_importado; }
        }

        public string PathArquivoImportado
        {
            get { return m_path_arquivo_importado; }
        }

        private bool m_dados_concatenados = false;
        private bool m_shape_importado = false;
        private bool m_dados_importados = false;

        private DataTable m_dt_tabela_shape = new DataTable();
        public DataTable DadosShape
        {
            get { return m_dt_tabela_shape; }
            set { m_dt_tabela_shape = value; }
        }

        private IpeaGeo.RegressoesEspaciais.clsIpeaShape m_shape = new IpeaGeo.RegressoesEspaciais.clsIpeaShape();
        public IpeaGeo.RegressoesEspaciais.clsIpeaShape Shape
        {
            get { return m_shape; }
            set { m_shape = value; }
        }

        #endregion

        #region estatísticas descritivas

        private void estatísticasDescritivasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            EstatisticasDescritivas();
        }

        private void EstatisticasDescritivas()
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    FormEstatisticasDescritivas frm = new FormEstatisticasDescritivas();
                    frm.TabelaDados = (DataTable)dataGridView1.DataSource;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void estatísticasDescritivasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EstatisticasDescritivas();
        }

        #endregion

        #region tabela de frequências

        private void tabelaDeFrequênciasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TabelaFrequencias();
        }

        private void TabelaFrequencias()
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    FormTabelaFrequencias frm = new FormTabelaFrequencias();
                    frm.TabelaDados = (DataTable)dataGridView1.DataSource;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tabelaDeFrequênciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabelaFrequencias();
        }

        #endregion

        #region tabulações cruzadas

        private void tabulaçõesCruzadasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabelasCruzadas();
        }

        private void tabulaçõesCruzadasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TabelasCruzadas();
        }

        private void TabelasCruzadas()
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    FormTabelasCruzadas frm = new FormTabelasCruzadas();
                    frm.TabelaDados = (DataTable)dataGridView1.DataSource;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region correlações

        private void correlaçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Correlacoes();
        }

        private void correlaçõesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Correlacoes();
        }

        private void Correlacoes()
        {
            try
            {
                if (m_dt_tabela_dados.Rows.Count > 0 && m_dt_tabela_dados.Columns.Count > 0)
                {
                    FormCorrelacoes frm = new FormCorrelacoes();
                    frm.TabelaDados = (DataTable)dataGridView1.DataSource;
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia.");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region importar tabela de dados

        private void importarArquivoDeDadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportarArquivoDeDados();
        }

        private void importarTabelaDeDadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportarArquivoDeDados();
        }

        private void ImportarArquivoDeDados()
        {
            try
            {
                clsUtilArquivos cla = new clsUtilArquivos();
                if (cla.ImportarTabelaDados(ref m_dt_tabela_dados, ref m_nome_arquivo_importado, ref m_path_arquivo_importado))
                {
                    this.dataGridView1.DataSource = m_dt_tabela_dados;

                    m_dt_dados_originais = m_dt_tabela_dados.Copy();

                    clsUtilTools clt = new clsUtilTools();

                    this.m_array_var_numericas = clt.RetornaColunasNumericas(this.m_dt_tabela_dados);
                    this.m_array_var_totais = clt.RetornaTodasColunas(this.m_dt_tabela_dados);

                    AtualizaTabelaDados();

                    this.m_funcao(false);

                    HabilitaControlesContextMenu();

                    m_dados_importados = true;
                    m_dados_concatenados = false;

                    if (m_dados_importados && m_shape_importado) concatenarShapeETabelaDeDadosToolStripMenuItem.Enabled = toolStripMenuItem5.Enabled = true;
                    else concatenarShapeETabelaDeDadosToolStripMenuItem.Enabled = toolStripMenuItem5.Enabled = false;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        #endregion

        #region exclusão de variáveis

        private void excluirVariáveisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcluirVariaveis();
        }

        private void ExcluirVariaveis()
        {
            try
            {
                IpeaGeo.RegressoesEspaciais.FormCalculadora frm = new IpeaGeo.RegressoesEspaciais.FormCalculadora();
                //frm.MdiParent = this.MdiParent;
                frm.Dados = (DataTable)dataGridView1.DataSource;
                frm.AtivaExclusaoVariaveis = true;

                //if (m_dados_concatenados)
                //{
                frm.DadosConcatenados = false;
                //frm.Shape = this.shapeAlex;
                //}

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    m_dt_tabela_dados = frm.Dados;

                    this.dataGridView1.DataSource = m_dt_tabela_dados;

                    this.m_funcao(false);

                    //this.AtualizaTabelaDados();
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void excluirVariáveisToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ExcluirVariaveis();
        }

        #endregion

        #region calculadora

        private void calculadoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calculadora();
        }

        private void Calculadora()
        {
            try
            {
                IpeaGeo.RegressoesEspaciais.FormCalculadora frm = new IpeaGeo.RegressoesEspaciais.FormCalculadora();
                //frm.MdiParent = this.MdiParent;
                frm.Dados = (DataTable)dataGridView1.DataSource;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    m_dt_tabela_dados = frm.Dados;

                    this.dataGridView1.DataSource = m_dt_tabela_dados;

                    this.m_funcao(false);

                    //this.AtualizaTabelaDados();
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void calculadoraToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Calculadora();
        }

        #endregion

        #region geração de variáveis dummy

        private void geraçãoDeVariáveisDummyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeracaoVariaveisDummy();
        }

        private void geraçãoDeVariáveisDummyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GeracaoVariaveisDummy();
        }

        private void GeracaoVariaveisDummy()
        {
            try
            {
                FormGeracaoDummies frm = new FormGeracaoDummies();
                frm.TabelaDados = (DataTable)dataGridView1.DataSource;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    this.m_dt_tabela_dados = frm.TabelaDados;

                    this.dataGridView1.DataSource = this.m_dt_tabela_dados;

                    this.m_funcao(false);

                    //this.AtualizaTabelaDados();
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region recuperar tabela de dados original

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            RecuperarDadosOriginais();
        }

        private void RecuperarDadosOriginais()
        {
            try
            {
                m_dt_tabela_dados = m_dt_dados_originais.Copy();

                dataGridView1.DataSource = m_dt_tabela_dados;

                this.m_funcao(false);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void recuperarTabelaOriginalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecuperarDadosOriginais();
        }

        #endregion

        #region controles

        private void larguraAutomáticaDosCabeçalhosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ColumnsWidthAutoFitHeader();
        }

        private void larguraAutomáticaDosConteúdosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ColumnsWidthAutoFitAllCells();
        }
        
        private void larguraAutomáticaDosCabeçalhosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.ColumnsWidthAutoFitHeader();
        }

        private void larguraAutomáticaDosConteúdosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.ColumnsWidthAutoFitAllCells();
        }

        #endregion

        #region gráficos 

        private void apresentaçãoDosGráficosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApresentacaoGraficos();  
        }

        private void apresentaçãoDosGráficosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ApresentacaoGraficos();
        }

        private void ApresentacaoGraficos()
        {
            try
            {
                Modelagem.Graficos.Graficos frg = new Modelagem.Graficos.Graficos();
                frg.TabelaDeDados = this.m_dt_tabela_dados.Copy();
                frg.Show();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region abertura do datatable em planilha Excel

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            AbrirTabelaEmExcel();
        }

        private void AbrirTabelaEmExcel()
        {
            try
            {
                if (((DataTable)dataGridView1.DataSource).Columns.Count > 0 && ((DataTable)dataGridView1.DataSource).Rows.Count > 0)
                {
                    DataTable dsTemp = (DataTable)dataGridView1.DataSource;

                    Cursor = Cursors.WaitCursor;

                    BLExportacaoAberturaExcel ble = new BLExportacaoAberturaExcel();
                    ble.ExportaToExcel(dsTemp, "tabela", "tabela");

                    Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void abrirEmPlanilhaExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirTabelaEmExcel();
        }

        #endregion

        #region habilita e desabilita controles do context menu

        public void DesabilitaControlesContextMenu()
        {
            try
            {
                exportarDadosToolStripMenuItem.Enabled =
                    toolStripMenuItem1.Enabled =
                    toolStripMenuItem2.Enabled =
                    toolStripMenuItem3.Enabled =
                    calculadoraToolStripMenuItem.Enabled =
                    excluirVariáveisToolStripMenuItem.Enabled =
                    geraçãoDeVariáveisDummyToolStripMenuItem.Enabled =
                    estatísticasDescritivasToolStripMenuItem.Enabled =
                    tabelaDeFrequênciasToolStripMenuItem.Enabled =
                    tabulaçõesCruzadasToolStripMenuItem.Enabled =
                    correlaçõesToolStripMenuItem.Enabled = false;
                this.larguraDasColunasToolStripMenuItem.Enabled = false;

                exportarTabelaDeDadosToolStripMenuItem.Enabled =
                    abrirEmPlanilhaExcelToolStripMenuItem.Enabled =
                    recuperarTabelaOriginalToolStripMenuItem.Enabled =
                    correlaçõesToolStripMenuItem1.Enabled =
                    geraçãoDeVariáveisDummyToolStripMenuItem1.Enabled =
                    excluirVariáveisToolStripMenuItem1.Enabled = 
                    tabelaDeFrequênciasToolStripMenuItem1.Enabled =
                    tabulaçõesCruzadasToolStripMenuItem1.Enabled =
                    estatísticasDescritivasToolStripMenuItem1.Enabled =
                    this.larguraDasColunasToolStripMenuItem1.Enabled = 
                    análiseGráficaToolStripMenuItem.Enabled =
                    calculadoraToolStripMenuItem1.Enabled = false;                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void HabilitaControlesContextMenu()
        {
            try
            {
                exportarDadosToolStripMenuItem.Enabled =
                    toolStripMenuItem1.Enabled =
                    toolStripMenuItem2.Enabled =
                    toolStripMenuItem3.Enabled =
                    calculadoraToolStripMenuItem.Enabled =
                    excluirVariáveisToolStripMenuItem.Enabled =
                    geraçãoDeVariáveisDummyToolStripMenuItem.Enabled =
                    estatísticasDescritivasToolStripMenuItem.Enabled =
                    tabelaDeFrequênciasToolStripMenuItem.Enabled =
                    tabulaçõesCruzadasToolStripMenuItem.Enabled =
                    correlaçõesToolStripMenuItem.Enabled =
                    larguraDasColunasToolStripMenuItem.Enabled = true;
                
                exportarTabelaDeDadosToolStripMenuItem.Enabled =
                    abrirEmPlanilhaExcelToolStripMenuItem.Enabled =
                    recuperarTabelaOriginalToolStripMenuItem.Enabled =
                    correlaçõesToolStripMenuItem1.Enabled =
                    geraçãoDeVariáveisDummyToolStripMenuItem1.Enabled =
                    excluirVariáveisToolStripMenuItem1.Enabled =
                    tabelaDeFrequênciasToolStripMenuItem1.Enabled =
                    tabulaçõesCruzadasToolStripMenuItem1.Enabled =
                    estatísticasDescritivasToolStripMenuItem1.Enabled =
                    this.larguraDasColunasToolStripMenuItem1.Enabled =
                    análiseGráficaToolStripMenuItem.Enabled =
                    calculadoraToolStripMenuItem1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion 
        
        #region exportar tabela de dados

        private void exportarDadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportaDados();
        }

        private void ExportaDados()
        {
            try
            {
                ExportData ed = new ExportData();
                ed.ExportarDados(dataGridView1, this.Name);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void exportarTabelaDeDadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportaDados();
        }

        #endregion

        #region importacao do arquivo shape

        private void ImportarShape()
        {
            try
            {
                clsUtilArquivos cla = new clsUtilArquivos();
                cla.ImportarArquivoShape(ref m_shape, ref m_dt_tabela_shape);

                this.m_funcao_shape(false);

                m_shape_importado = true;
                m_dados_concatenados = false;

                if (m_dados_importados && m_shape_importado) concatenarShapeETabelaDeDadosToolStripMenuItem.Enabled = toolStripMenuItem5.Enabled = true;
                else concatenarShapeETabelaDeDadosToolStripMenuItem.Enabled = toolStripMenuItem5.Enabled = false;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void importarArquivoShapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportarShape();
        }

        private void ConcatenarShapeToDados()
        {
            try
            {
                IpeaGeo.RegressoesEspaciais.FormConcatenacaoTabelaToShape frm = new IpeaGeo.RegressoesEspaciais.FormConcatenacaoTabelaToShape();
                frm.TabelaDados = this.m_dt_tabela_dados;
                frm.TabelaShape = this.m_dt_tabela_shape;
                frm.Shape = this.m_shape;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    m_dt_tabela_dados = frm.TabelaDadosConcatenados;

                    ConcatenarDados();

                    this.m_funcao_concatenacao(false);

                    m_dados_concatenados = true;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void concatenarShapeETabelaDeDadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConcatenarShapeToDados();
        }

        private void ConcatenarDados()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                int obs_shape = m_shape.Count;
                int obs_dados = this.m_dt_tabela_dados.Rows.Count;

                if (obs_dados > obs_shape) MessageBox.Show("O número de observações na tabela de dados é maior do que o número de observações no arquivo shape.",
                    "Falha na concatenação", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (obs_dados < obs_shape) MessageBox.Show("O número de observações na tabela de dados é menor do que o número de observações no arquivo shape.",
                    "Falha na concatenação", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (obs_dados == obs_shape)
                {
                    m_dados_concatenados = true;
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception er)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(er.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            ImportarShape();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            ConcatenarShapeToDados();
        }
        
        #endregion
    }
}
