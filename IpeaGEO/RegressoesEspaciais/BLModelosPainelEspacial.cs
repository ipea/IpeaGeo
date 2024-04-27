using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace IpeaGeo.RegressoesEspaciais
{
    public class BLModelosPainelEspacial : clsModelosRegressaoEspacial
    {
        public BLModelosPainelEspacial()
        {
        }
        
        #region variáveis

        private delegate double funcao_kernel(double d, double d_max);
        private funcao_kernel m_kernel;

        protected bool m_usa_num_vizinhos_para_distancia = false;
        public bool UsaNumVizinhosParaDistancia
        {
            get
            {
                return m_usa_num_vizinhos_para_distancia;
            }
            set
            {
                this.m_usa_num_vizinhos_para_distancia = value;
            }
        }

        protected int m_num_vizinhos_para_distancia = 10;
        public int NumVizinhosParaDistancia
        {
            get
            {
                return m_num_vizinhos_para_distancia;
            }
            set
            {
                this.m_num_vizinhos_para_distancia = value;
            }
        }

        protected double m_bandkwidth_kernel_HAC = 0.0;
        public double BandWidthKernelHAC
        {
            get { return this.m_bandkwidth_kernel_HAC; }
            set { this.m_bandkwidth_kernel_HAC = value; }
        }

        protected TipoKernelCorrecaoHAC m_tipo_kernel_correcao_HAC = TipoKernelCorrecaoHAC.Barlett;
        public TipoKernelCorrecaoHAC TipoKernelCorrecaoHAC
        {
            get { return this.m_tipo_kernel_correcao_HAC; }
            set
            {
                this.m_tipo_kernel_correcao_HAC = value;
                switch (value)
                {
                    case TipoKernelCorrecaoHAC.Barlett:
                        m_kernel = new funcao_kernel(Barlett);
                        break;
                    case TipoKernelCorrecaoHAC.Epanechnikov:
                        m_kernel = new funcao_kernel(Epanechnikov);
                        break;
                    case TipoKernelCorrecaoHAC.Biquadrado:
                        m_kernel = new funcao_kernel(Biquadrado);
                        break;
                    default:
                        m_kernel = new funcao_kernel(Barlett);
                        break;
                }
            }
        }

        protected TipoCorrecaoMatrizCovariancia m_tipo_correcao_cov_matrix = TipoCorrecaoMatrizCovariancia.SemCorrecao;
        public TipoCorrecaoMatrizCovariancia TipoCorrecaoCovMatrix
        {
            get { return this.m_tipo_correcao_cov_matrix; }
            set { this.m_tipo_correcao_cov_matrix = value; }
        }

        protected DataTable m_dt_tabela_dados = new DataTable();
        protected string[] m_variaveis_dependentes = new string[0];
        protected string[] m_variaveis_independentes = new string[0];
        protected string[] m_variaveis_instrumentais = new string[0];
        protected string[] m_variaveis_exogenas = new string[0];
        protected string[] m_variaveis_endogenas = new string[0];

        protected bool m_usa_intercepto = true;
        protected string m_output_text = "";
        protected string m_output_variaveis_geradas = "";
        protected bool m_adiciona_novas_variaveis = false;

        public bool AdicionaNovaVariaveis
        {
            get
            {
                return m_adiciona_novas_variaveis;
            }
            set
            {
                m_adiciona_novas_variaveis = value;
            }
        }

        protected bool m_apresenta_covmatrix_beta_hat = false;
        protected TipoCalculoLogDetWMatrix m_tipo_logdet = TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo;

        public TipoCalculoLogDetWMatrix TipoCalculoLogDetW
        {
            get
            {
                return this.m_tipo_logdet;
            }
            set
            {
                this.m_tipo_logdet = value;
            }
        }

        public bool ApresentaCovMatrixBetaHat { set { this.m_apresenta_covmatrix_beta_hat = value; } }

        protected string m_variavel_coord_X = "";
        protected string m_variavel_coord_Y = "";
        protected double m_cutoff_X = 0.0;
        protected double m_cutoff_Y = 0.0;

        public string VariavelCoordenadaX { set { this.m_variavel_coord_X = value; } }
        public string VariavelCoordenadaY { set { this.m_variavel_coord_Y = value; } }
        public double CutOffCoordenadaX { set { this.m_cutoff_X = value; } }
        public double CutOffCoordenadaY { set { this.m_cutoff_Y = value; } }

        protected clsLinearRegressionModelsMLE m_geomle = new clsLinearRegressionModelsMLE();

        public clsMatrizEsparsa MatrizEsparsaFromDistancias
        {
            get
            {
                return m_geomle.Wesparsa;
            }
            set
            {
                m_geomle.Wesparsa = value;
            }
        }

        protected clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            get { return this.m_shape.Clone(); }
            set { this.m_shape = value.Clone(); }
        }

        protected TipoMatrizVizinhanca m_tipo_matriz_vizinhanca = TipoMatrizVizinhanca.Normalizada;
        public TipoMatrizVizinhanca TipoMatrizVizinhanca
        {
            set { this.m_tipo_matriz_vizinhanca = value; }
            get { return this.m_tipo_matriz_vizinhanca; }
        }

        public string VariaveisGeradas
        {
            get { return this.m_output_variaveis_geradas; }
        }

        public string ResultadoEstimacao
        {
            get { return this.m_output_text; }
        }

        public bool IncluiIntercepto
        {
            get { return this.m_usa_intercepto; }
            set { this.m_usa_intercepto = value; }
        }

        public DataTable TabelaDados
        {
            set { this.m_dt_tabela_dados = value.Copy(); }
            get { return this.m_dt_tabela_dados.Copy(); }
        }

        public string[] VariaveisDependentes
        {
            set { this.m_variaveis_dependentes = value; }
            get { return this.m_variaveis_dependentes; }
        }

        public string[] VariaveisIndependentes
        {
            set { this.m_variaveis_independentes = value; }
            get { return this.m_variaveis_independentes; }
        }

        public string[] VariaveisExogenas
        {
            get { return this.m_variaveis_exogenas; }
        }

        public string[] VariaveisInstrumentais
        {
            set { this.m_variaveis_instrumentais = value; }
            get { return this.m_variaveis_instrumentais; }
        }

        public double GeraMaximaDistancia(DataTable dt, string variavel)
        {
            clsUtilTools clt = new clsUtilTools();
            double[,] w = clt.GetMatrizFromDataTable(dt, variavel);

            return Math.Abs(clt.Max(w) - clt.Min(w));
        }

        #endregion
    }
}
