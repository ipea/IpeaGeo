using System;
using System.Data;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    class BLogicRegressaoLinearDadosPainelEspacial : BLogicRegressaoLinearDadosPainel
    {
        public BLogicRegressaoLinearDadosPainelEspacial()
            : base()
        {
        }
        
        #region Matriz de vizinhança
        
        protected string m_tipo_matriz_vizinhanca_predefinida = "";
        public string TipoMatrizVizinhancaPredefinida
        {
            set { m_tipo_matriz_vizinhanca_predefinida = value; }
        }

        public RegressoesEspaciais.clsMatrizEsparsa MatrizEsparsaFromDistancias
        {
            set { blcss.Wesparsa = value; }
        }

        private bool m_estimacao_bem_sucedida = false;
        public bool EstimacaoBemSucedida
        {
            set { m_estimacao_bem_sucedida = value; }
        }

        protected RegressoesEspaciais.BLModelosCrossSectionSpaciais blcss = new RegressoesEspaciais.BLModelosCrossSectionSpaciais();
        protected RegressoesEspaciais.clsModelosRegressaoEspacial clsms = new RegressoesEspaciais.clsModelosRegressaoEspacial();

        protected TipoMatrizVizinhanca m_tipo_matriz_anterior = TipoMatrizVizinhanca.Normalizada;

        public void GeraMatrizVizinhanca()
        {
            if (m_tipo_matriz_vizinhanca == IpeaGeo.TipoMatrizVizinhanca.Normalizada) clsms.TipoMatrizVizinhanca = RegressoesEspaciais.TipoMatrizVizinhanca.Normalizada;
            if (m_tipo_matriz_vizinhanca == IpeaGeo.TipoMatrizVizinhanca.Original) clsms.TipoMatrizVizinhanca = RegressoesEspaciais.TipoMatrizVizinhanca.Original;

            if (!m_estimacao_bem_sucedida
                   || clsms.Shape == null
                   || clsms.Shape.Count <= 0
                   || clsms.Shape.HoraCriacao != this.Shape.HoraCriacao
                   || m_tipo_matriz_anterior != m_tipo_matriz_vizinhanca
                   || m_tipo_calculo_logdet_anterior != m_tipo_logdet
                   || clsms.Shape.TipoVizinhanca != this.Shape.TipoVizinhanca)
            {
                clsms.Shape = this.Shape;

                if (m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Normalizada)
                {
                    clsms.MatrizWesparsaFromVizinhosNorm();
                }

                if (m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Original)
                {
                    clsms.MatrizWesparsaFromVizinhos();
                }

                m_W_esparsa = clsms.Wesparsa;

                m_tipo_calculo_logdet_anterior = m_tipo_logdet;
                m_tipo_matriz_anterior = m_tipo_matriz_vizinhanca;
            }
            else
            {
                m_W_esparsa = clsms.Wesparsa;
            }
        }

        #endregion

        #region variáveis internas

        private RegressoesEspaciais.clsUtilOptimization opt = new RegressoesEspaciais.clsUtilOptimization();
        private RegressoesEspaciais.clsUtilTools m_clt = new RegressoesEspaciais.clsUtilTools();
        private RegressoesEspaciais.clsFuncoesMatrizEsparsa fme = new RegressoesEspaciais.clsFuncoesMatrizEsparsa();
        private RegressoesEspaciais.clsMatrizEsparsa m_sparse_ident = new RegressoesEspaciais.clsMatrizEsparsa();
        private RegressoesEspaciais.clsLogDetMatrizEsparsa m_ldet = new RegressoesEspaciais.clsLogDetMatrizEsparsa();

        #endregion 

        #region Estimação de modelos espaciais para dados de painel

        #region Inicialização dos polinomios de logdet da matriz de vizinhança

        private double m_xmin = -1.0;
        private double m_xmax = 0.99;

        protected void InicializaPolinomiosLogDetWEsparsa()
        {
            m_nobs = m_shape.Count;

            if (this.m_tipo_logdet == RegressoesEspaciais.TipoCalculoLogDetWMatrix.MatrizDensa)
            {
                this.m_W_esparsa.CalcularAutovaloresW();
            }

            m_xmin = -1.0;
            m_xmax = 0.99;

            m_sparse_ident = fme.Identity(m_nobs);

            if ((this.m_nobs <= 300) || (this.m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Original && this.m_nobs <= 2000))
            {
                double lambda_min = 0.0;
                double lambda_max = 0.0;

                if (this.m_tipo_logdet != RegressoesEspaciais.TipoCalculoLogDetWMatrix.MatrizDensa)
                {
                    this.m_W_esparsa.CalcularAutovaloresW();
                }

                lambda_min = (1.0 / this.m_W_esparsa.Autovalores[this.m_W_esparsa.Autovalores.GetLength(0) - 1]) + 0.01;
                lambda_max = (1.0 / this.m_W_esparsa.Autovalores[0]) - 0.01;

                m_xmin = lambda_min;
                m_xmax = lambda_max;
            }

            if (this.m_tipo_logdet == RegressoesEspaciais.TipoCalculoLogDetWMatrix.MatrizDensa)
            {
                this.m_W_esparsa.GeraTabelaLogDetAutovalores(m_xmin, m_xmax);
            }
            else
            {
                this.m_W_esparsa.GeraTabelaLogDetLU(m_xmin, m_xmax);
            }
        }

        #endregion
        
        public void EstimaModelosPainelEspacial()
        {
            m_estimacao_bem_sucedida = false;

            InicializaPolinomiosLogDetWEsparsa();

            this.OrganizaPainelBalanceado();

            #region preparando os dados

            double[,] X_original = m_clt.ArrayDoubleClone(m_X);

            if (m_usa_intercepto && m_tipo_estimacao_painel != TipoEstimacaoDadosPainel.EfeitosFixos
                && m_tipo_estimacao_painel != TipoEstimacaoDadosPainel.PrimeirasDiferencas)
            {
                m_X = m_clt.Concateh(m_clt.Ones(m_X.GetLength(0), 1), m_X);
            }

            if (m_usa_dummies_temporais)
            {
                this.GeraDummiesTemporais(out m_dummies_temporais, out m_nomes_dummies_temporais);

                if (m_usa_intercepto || m_tipo_estimacao_painel != TipoEstimacaoDadosPainel.EfeitosFixos
                       || m_tipo_estimacao_painel != TipoEstimacaoDadosPainel.PrimeirasDiferencas)
                {
                    m_dummies_temporais = m_clt.RemoveColumnArrayDouble(m_dummies_temporais, m_dummies_temporais.GetLength(1) - 1);
                    m_nomes_dummies_temporais = m_clt.RemoveElementoArrayString(m_nomes_dummies_temporais, m_nomes_dummies_temporais.GetLength(0) - 1);
                }

                m_X = m_clt.Concateh(m_X, m_dummies_temporais);
                X_original = m_clt.Concateh(X_original, m_dummies_temporais);

                m_numero_dummies_temporais = m_dummies_temporais.GetLength(1);
            }
            else
            {
                m_numero_dummies_temporais = 0;
                m_nomes_dummies_temporais = new string[0];
                m_dummies_temporais = new double[0, 0];
            }

            if (m_usa_tendencia_temporal_linear || m_usa_tendencia_temporal_quadratica || m_usa_tendencia_temporal_cubica)
            {
                this.GeraTendenciaTemporal(out m_tendencia_temporal, out m_nomes_tendencia, m_usa_tendencia_temporal_linear, m_usa_tendencia_temporal_quadratica, m_usa_tendencia_temporal_cubica);

                m_X = m_clt.Concateh(m_X, m_tendencia_temporal);
                X_original = m_clt.Concateh(X_original, m_tendencia_temporal);
                m_numero_tendencias_temporais = m_tendencia_temporal.GetLength(1);
            }
            else
            {
                m_numero_tendencias_temporais = 0;
                m_tendencia_temporal = new double[0, 0];
                m_nomes_tendencia = new string[0];
            }

            #endregion 

            #region checando multicolinearidade depois da adição de dummies e tendencias temporais

            if (m_usa_dummies_temporais || m_usa_tendencia_temporal_linear || m_usa_tendencia_temporal_quadratica || m_usa_tendencia_temporal_cubica)
            {
                string[] nomes = m_clt.Concate(m_clt.Concate(m_variaveis_independentes, m_nomes_dummies_temporais), m_nomes_tendencia);
                DataTable dt_temp = m_clt.DataTableFromMatriz(X_original, nomes);

                BLogicRegressaoLinear blr = new BLogicRegressaoLinear();

                blr.VariaveisIndependentes = nomes;
                blr.TabelaDados = dt_temp;
                blr.intercepto = (m_usa_intercepto && m_tipo_estimacao_painel != TipoEstimacaoDadosPainel.EfeitosFixos
                    && m_tipo_estimacao_painel != TipoEstimacaoDadosPainel.PrimeirasDiferencas);

                string mensagem_colinear = "";

                if (blr.ChecarMulticolinearidade(out mensagem_colinear))
                {
                    throw new Exception(mensagem_colinear + " Cheque a sua base de dados ou mude a especificação do modelo.");
                }
            }

            #endregion

            EstimaModeloSEMPooledReg();
        }

        #region estimação de um modelo SEM para painel espacial pooled regression

        protected double minusloglikSEMPainel(double lambda)
        {
            RegressoesEspaciais.clsMatrizEsparsa P = fme.MatrizSoma(m_sparse_ident, m_W_esparsa, 1.0, -lambda);

            double[,] Xtilde = new double[m_X.GetLength(0), m_X.GetLength(1)];
            double[,] ytilde = new double[m_Y.GetLength(0), 1];

            double[,] Xtemp;
            double[,] ytemp;

            double[,] XX;
            double[,] Xy;
            double[,] XXinv;

            for (int t = 0; t < m_num_periodos; t++)
            {
                Xtemp = painel_periodo(m_X, t);
                ytemp = painel_periodo(m_Y, t);

                Xtemp = fme.MatrizMult(P, Xtemp);
                ytemp = fme.MatrizMult(P, ytemp);

                for (int i = 0; i < m_num_periodos; i++)
                {
                    ytilde[t * m_num_periodos + i, 0] = ytemp[i, 0];
                    for (int j = 0; j < m_X.GetLength(1); j++) Xtilde[t * m_num_periodos + i, j] = Xtemp[i, j];
                }
            }

            XX = m_clt.MatrizMult(m_clt.MatrizTransp(Xtilde), Xtilde);
            Xy = m_clt.MatrizMult(m_clt.MatrizTransp(Xtilde), ytilde);
            XXinv = m_clt.MatrizInversa(XX);

            m_beta_hat = m_clt.MatrizMult(XXinv, Xy); 

            double[,] v = m_clt.MatrizSubtracao(ytilde, m_clt.MatrizMult(Xtilde, m_beta_hat));
            double vpv = (m_clt.MatrizMult(m_clt.MatrizTransp(v), v))[0, 0];

            double llike = (((double)m_nobs) / 2.0) * Math.Log(vpv)
                            - ((double)m_num_periodos)*m_W_esparsa.Spline.interp(lambda);

            return llike;
        }

        public void EstimaModeloSEMPooledReg()
        {
            double rho_new = 0.0;
            double fval = 0.0;

            clsUtilOptimization.FunctionUnivariate func = new clsUtilOptimization.FunctionUnivariate(this.minusloglikSEMPainel);
            rho_new = opt.MinBrentSearch(func, m_xmin, m_xmax, ref fval);

            double minus_loglik = minusloglikSEMPainel(rho_new);
        }

        #endregion 

        #endregion
    }
}
