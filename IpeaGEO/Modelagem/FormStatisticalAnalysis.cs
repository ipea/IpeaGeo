using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IpeaGeo.Modelagem
{
    public abstract class FormStatisticalAnalysis : Form
    {
        protected DataSet m_ds_dados_painel_espacial = new DataSet();
        public DataSet DadosPainelEspacial
        {
            get { return m_ds_dados_painel_espacial; }
            set { m_ds_dados_painel_espacial = value; }
        }

        protected DataSet m_dataset_externo = new DataSet();
        public DataSet DataSetExterno 
        {
            get { return m_dataset_externo; }
            set { m_dataset_externo = value; }
        }

        protected DataGridView m_gridview_externo = new DataGridView();
        public DataGridView GridViewExterno
        {
            set { m_gridview_externo = value; }
        }

        protected string m_label_tabela_dados = "";
        public string LabelTabelaDados
        {
            set { m_label_tabela_dados = value; }
        }

        protected object[,] m_lista_periodos_painel_espacial = new object[0, 0];
        public object[,] ListaPeriodosPainelEspacial
        {
            get { return m_lista_periodos_painel_espacial; }
            set { m_lista_periodos_painel_espacial = value; }
        }

        protected string m_periodo_foco = "";
        public string PeriodoFocoPainel
        {
            set { m_periodo_foco = value; }
        }

        protected DataTable m_dt_tabela_dados = new DataTable();
        public DataTable TabelaDeDados
        {
            get { return m_dt_tabela_dados; }
            set { m_dt_tabela_dados = value; }
        }

        protected string m_variavel_periodos = "";
        public string VariavelPeriodosPainel
        {
            set { m_variavel_periodos = value; }
        }

        protected string m_variavel_unidades = "";
        public string VariavelUnidadesPainel
        {
            set { m_variavel_unidades = value; }
        }

        abstract public void HabilitarDadosExternos();
    }
}

