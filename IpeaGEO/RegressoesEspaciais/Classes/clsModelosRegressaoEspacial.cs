using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace IpeaGeo.RegressoesEspaciais
{
    #region Enumerações
    
    public enum TipoKernelCorrecaoHAC : int
    {
        Barlett,
        Epanechnikov,
        Biquadrado
    };

    public enum TipoFuncaoLigacao : int
    {
        Logaritmo,
        Logit,
        Cloglog,
        Probit,
        Identidade
    };

    public enum TipoCorrecaoMatrizCovariancia : int
    {
        SemCorrecao,
        Heteroscedasticidade,
        HAC
    };
    
    public enum TipoOrigemMatrizVizinhanca : int
    {
        ArquivoShape = 1,
        MatrizFromDistancias = 2,
        MatrizFromArquivo = 3,
        MatrizPreDefinida
    };

    public enum TipoContiguidade : int
    {
        Rook = 1,
        Queen = 2, 
        Distâncias = 3
    };

    public enum TipoModeloEspacial : int
    {
        SAR = 1,
        SEM = 2,
        SAC = 3,
        nao_espacial
    };

    public enum TipoMatrizVizinhanca : int
    {
        Original = 1,
        Normalizada = 2
    };

    public enum TipoCalculoLogDetWMatrix : int
    {
        MatrizDensa,
        SimulacoesMonteCarlo,
        MatrizEsparsaDecomposicaoLU,
        DecomposicaoAutovalores
    };

    #endregion

    public class clsModelosRegressaoEspacial
    {
        public clsModelosRegressaoEspacial()
        {
        }

        #region rotinas para cálculo do log Jacobiano da matriz I - rho x W via eigenvalue decomposition

        protected double[] m_autovalores_W = new double[0];

        protected void CalcularAutovaloresW(clsMatrizEsparsa W)
        {
            clsUtilTools clt = new clsUtilTools();
            Complex[] autovalores = new Complex[0];
            clt.AutovaloresMatrizAssimetrica(W.AsDoubleMatrix(), ref autovalores);
            m_autovalores_W = new double[autovalores.GetLength(0)];
            for (int i = 0; i < m_autovalores_W.GetLength(0); i++)
            {
                m_autovalores_W[i] = autovalores[i].Real;
            }
        }

        protected double LogDetIrhoW(double rho)
        {
            double res = 0.0;
            for (int i = 0; i < m_autovalores_W.GetLength(0); i++)
            {
                res += Math.Log(1.0 - rho * m_autovalores_W[i]);
            }
            return res;
        }

        #endregion

        #region variáveis
        
        protected TipoMatrizVizinhanca m_tipo_matriz_vizinhanca = TipoMatrizVizinhanca.Normalizada;
        public TipoMatrizVizinhanca TipoMatrizVizinhanca
        {
            set { this.m_tipo_matriz_vizinhanca = value; }
            get { return this.m_tipo_matriz_vizinhanca; }
        }

        protected TipoOrigemMatrizVizinhanca m_tipo_origem_matriz_vizinhanca = TipoOrigemMatrizVizinhanca.ArquivoShape;
        public TipoOrigemMatrizVizinhanca TipoOrigemMatrizVizinhanca
        {
            get
            {
                return m_tipo_origem_matriz_vizinhanca;
            }
            set
            {
                m_tipo_origem_matriz_vizinhanca = value;
            }
        }

        protected double m_lambda_com_rho_nulo = 0.0;
        public double LambdaComRhoNulo
        {
            get { return m_lambda_com_rho_nulo; }
        }

        protected double m_rho_com_lambda_nulo = 0.0;
        public double RhoComLambdaNulo
        {
            get { return m_rho_com_lambda_nulo; }
        }

        protected string m_mensagem_iterations_till_convergence = "";
        public string MensagemIterationsTillConvergence
        {
            get { return this.m_mensagem_iterations_till_convergence; }
            set { this.m_mensagem_iterations_till_convergence = value; }
        }

        protected int m_num_iterations = 0;
        public int NumIterations
        {
            get { return this.m_num_iterations; }
            set { this.m_num_iterations = value; }
        }

        protected int m_max_iterations_till_convergence = 2000;
        public int MaxIterationsTillConvergence
        {
            get { return this.m_max_iterations_till_convergence; }
            set { this.m_max_iterations_till_convergence = value; }
        }

        protected double m_tol_iterate_till_convergence = 1.0e-6;
        public double TolIterateTillConvergence
        {
            get { return this.m_tol_iterate_till_convergence; }
            set { this.m_tol_iterate_till_convergence = value; }
        }

        protected bool m_iterate_till_convergence = false;
        public bool IterateTillConvergence
        {
            get { return this.m_iterate_till_convergence; }
            set { this.m_iterate_till_convergence = value; }
        }

        protected double[,] m_W_matriz = new double[0, 0];

        protected double[,] m_variaveis_geradas = new double[0, 0];
        public double[,] VariaveisGeradas { get { return this.m_variaveis_geradas; } }

        protected string[] m_nomes_variaveis_geradas = new string[0];
        public string[] NomesVariaveisGeradas { get { return this.m_nomes_variaveis_geradas; } }

        protected double m_loglik = 0.0;
        protected double m_aic = 0.0;
        protected double m_bic = 0.0;

        public double LogLik { get { return this.m_loglik; } }
        public double AIC { get { return this.m_aic; } }
        public double BIC { get { return this.m_bic; } }

        protected double[,] m_beta_hat = new double[0,0];
        protected double[,] m_beta_stderror = new double[0, 0];
        protected double[,] m_beta_tstat = new double[0, 0];
        protected double[,] m_beta_pvalor = new double[0, 0];
        protected double[,] m_beta_liminf_ci = new double[0, 0];
        protected double[,] m_beta_limsup_ci = new double[0, 0];
        protected double[,] m_beta_hat_cov = new double[0, 0];

        protected double m_sigma2_hat = 0.0;
        protected double m_prob_confidence_intervals = 95.0;
    
        public double[,] BetaHat 
        { 
            get { return this.m_beta_hat; }
            set { this.m_beta_hat = value; }
        }

        public double[,] BetaStdError { get { return this.m_beta_stderror; } }
        public double[,] BetaTStat { get { return this.m_beta_tstat; } }
        public double[,] BetaPValue { get { return this.m_beta_pvalor; } }
        public double[,] BetaLimInfCI { get { return this.m_beta_liminf_ci; } }
        public double[,] BetaLimSupCI { get { return this.m_beta_limsup_ci; } }

        public double[,] BetaHatCovMatrix 
        { 
            get { return this.m_beta_hat_cov; }
            set { this.m_beta_hat_cov = value; }
        }

        public double Sigma2Hat { get { return this.m_sigma2_hat; } }

        protected double m_rho_hat = 0.0;
        protected double m_rho_stderror = 0.0;
        protected double m_rho_tstat = 0.0;
        protected double m_rho_pvalor = 0.0;
        protected double m_rho_liminf_ci = 0.0;
        protected double m_rho_limsup_ci = 0.0;

        public double RhoHat { get { return this.m_rho_hat; } }
        public double RhoStdError { get { return this.m_rho_stderror; } }
        public double RhoTStat { get { return this.m_rho_tstat; } }
        public double RhoPValue { get { return this.m_rho_pvalor; } }
        public double RhoLimInfCI { get { return this.m_rho_liminf_ci; } }
        public double RhoLimSupCI { get { return this.m_rho_limsup_ci; } }

        protected double m_lambda_hat = 0.0;
        protected double m_lambda_stderror = 0.0;
        protected double m_lambda_tstat = 0.0;
        protected double m_lambda_pvalor = 0.0;
        protected double m_lambda_liminf_ci = 0.0;
        protected double m_lambda_limsup_ci = 0.0;

        public double LambdaHat { get { return this.m_lambda_hat; } }
        public double LambdaStdError { get { return this.m_lambda_stderror; } }
        public double LambdaTStat { get { return this.m_lambda_tstat; } }
        public double LambdaPValue { get { return this.m_lambda_pvalor; } }
        public double LambdaLimInfCI { get { return this.m_lambda_liminf_ci; } }
        public double LambdaLimSupCI { get { return this.m_lambda_limsup_ci; } }

        protected double[,] m_residuos = new double[0, 0];
        protected double[,] m_res_versus_w = new double[0, 0];
        protected double[,] m_Y = new double[0, 0];
        protected double[,] m_X = new double[0, 0];
        protected double[,] m_Z = new double[0, 0];
        protected int m_nobs = 0;

        protected double m_likratio_rho_stat = 0.0;
        protected double m_likratio_rho_pvalue = 0.0;

        protected double m_likratio_lambda_stat = 0.0;
        protected double m_likratio_lambda_pvalue = 0.0;

        protected double m_likratio_rho_lambda_stat = 0.0;
        protected double m_likratio_rho_lambda_pvalue = 0.0;
        
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

        public double[,] ResiduosDefasados { get { return this.m_res_versus_w; } }

        protected TipoModeloEspacial m_tipo_modelo = TipoModeloEspacial.SAR;
        public TipoModeloEspacial TipoModeloRegressaoEspacial
        {
            get { return this.m_tipo_modelo; }
            set { this.m_tipo_modelo = value; }
        }

        public double LikelihoodRatioTestStatRho
        {
            get { return this.m_likratio_rho_stat; }
            set { this.m_likratio_rho_stat = value; }
        }

        public double LikelihoodRatioTestPvalueRho
        {
            get { return this.m_likratio_rho_pvalue; }
            set { this.m_likratio_rho_pvalue = value; }
        }

        public double LikelihoodRatioTestStatRhoLambda
        {
            get { return this.m_likratio_rho_lambda_stat; }
            set { this.m_likratio_rho_lambda_stat = value; }
        }

        public double LikelihoodRatioTestPvalueRhoLambda
        {
            get { return this.m_likratio_rho_lambda_pvalue; }
            set { this.m_likratio_rho_lambda_pvalue = value; }
        }

        public double LikelihoodRatioTestStatLambda
        {
            get { return this.m_likratio_lambda_stat; }
            set { this.m_likratio_lambda_stat = value; }
        }

        public double LikelihoodRatioTestPvalueLambda
        {
            get { return this.m_likratio_lambda_pvalue; }
            set { this.m_likratio_lambda_pvalue = value; }
        }

        public double[,] Residuos { get { return this.m_residuos; } }
        public double[,] Y 
        { 
            get { return this.m_Y; }
            set { this.m_Y = value; }
        }
        public double[,] X 
        { 
            get { return this.m_X; }
            set { this.m_X = value; }
        }
        public double[,] Z 
        { 
            get { return this.m_Z; }
            set { this.m_Z = value; }
        }

        protected clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            get { return this.m_shape.Clone(); }
            set { this.m_shape = value.Clone(); }
        }

        protected TipoMatrizVizinhanca m_tipo_vizinhanca_anterior = TipoMatrizVizinhanca.Normalizada;
        protected TipoCalculoLogDetWMatrix m_tipo_calculo_logdet_anterior = TipoCalculoLogDetWMatrix.MatrizEsparsaDecomposicaoLU;

        protected double[,] m_W = new double[0, 0];
        public double[,] Wmatriz
        {
            get { return this.m_W; }
            set { this.m_W = value; }
        }

        protected clsMatrizEsparsa m_W_esparsa = new clsMatrizEsparsa();
        public clsMatrizEsparsa Wesparsa
        {
            get { return this.m_W_esparsa; }
            set { this.m_W_esparsa = value; }
        }

        public int NumNonZeroElementsMatrizW
        {
            get
            {
                int contador = 0;
                for (int i = 0; i < m_W.GetLength(0); i++)
                {
                    for (int j = 0; j < m_W.GetLength(1); j++)
                    {
                        if (this.m_W[i, j] != 0.0) contador++;
                    }
                }

                return contador;
            }
        }

        #endregion

        #region funções auxiliares

        public void LimparTabelaDasVariaveisGeradas(ref DataTable dt)
        {
            if (dt.Columns.Contains("Observacao_")) dt.Columns.Remove("Observacao_");
            if (dt.Columns.Contains("Y_observado_")) dt.Columns.Remove("Y_observado_");
            if (dt.Columns.Contains("Y_predito_")) dt.Columns.Remove("Y_predito_");
            if (dt.Columns.Contains("Prob_predita_")) dt.Columns.Remove("Prob_predita_");
            if (dt.Columns.Contains("Y_predito_uncondicional_")) dt.Columns.Remove("Y_predito_uncondicional_");
            if (dt.Columns.Contains("Y_predito_condicional_")) dt.Columns.Remove("Y_predito_condicional_");
            if (dt.Columns.Contains("Residuo_")) dt.Columns.Remove("Residuo_");
            if (dt.Columns.Contains("Residuo_padronizado_")) dt.Columns.Remove("Residuo_padronizado_");
            if (dt.Columns.Contains("SpatialW_residuo_")) dt.Columns.Remove("SpatialW_residuo_");
            if (dt.Columns.Contains("Epsilon_")) dt.Columns.Remove("Epsilon_");
            if (dt.Columns.Contains("Epsilon_padronizado_")) dt.Columns.Remove("Epsilon_padronizado_");
            if (dt.Columns.Contains("SpatialW_epsilon_")) dt.Columns.Remove("SpatialW_epsilon_");
            if (dt.Columns.Contains("Unidade_observacional_")) dt.Columns.Remove("Unidade_observacional_");
            if (dt.Columns.Contains("Periodo_de_tempo_")) dt.Columns.Remove("Periodo_de_tempo_");
            if (dt.Columns.Contains("DY_observado_")) dt.Columns.Remove("DY_observado_");
            if (dt.Columns.Contains("DY_predito_")) dt.Columns.Remove("DY_predito_");
            if (dt.Columns.Contains("DResiduo_")) dt.Columns.Remove("DResiduo_");
            if (dt.Columns.Contains("EfeitosFixos_")) dt.Columns.Remove("EfeitosFixos_");
        }

        public double[] RowsSumFromDenseW()
        {
            double[] res = new double[this.m_W.GetLength(0)];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < m_W.GetLength(1); j++)
                {
                    res[i] += m_W[i,j];
                }
            }
            return res;
        }

        public void MatrizWesparsaFromVizinhosNorm()
        {
            int nzmax = 0;
            int[] ind_vizinhos = new int[0];
            for (int i = 0; i < this.m_shape.Count; i++)
            {
                ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
                for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
                {
                    if (i != ind_vizinhos[k]) nzmax++;
                }
            }

            clsMatrizEsparsa a = new clsMatrizEsparsa(this.m_shape.Count, this.m_shape.Count, nzmax, true);

            int indice = 0;
            for (int i = 0; i < this.m_shape.Count; i++)
            {
                ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
                for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
                {
                    if (i != ind_vizinhos[k])
                    {
                        a.x[indice] = 1.0;
                        a.i[indice] = i;
                        a.p[indice] = ind_vizinhos[k];
                        indice++;
                    }
                }
            }

            //----------------- calculando a soma de cada linha a partir da matriz esparsa na triplet forma ----------//

            int[] ai = a.i;
            double[] ax = a.x;
            double[] soma_rows = new double[a.m];
            for (int i = 0; i < ax.GetLength(0); i++)
            {
                soma_rows[ai[i]] += ax[i];
            }

            for (int i = 0; i < ax.GetLength(0); i++)
            {
                ax[i] = ax[i] / soma_rows[ai[i]];
            }

            a.x = ax;

            //----------------- convertendo para matriz esparsa na forma compressed-column ---------------------------//

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            m_W_esparsa = fme.TripletForm2CompressColumn(a);

            m_tipo_vizinhanca_anterior = TipoMatrizVizinhanca.Normalizada;
        }

        public void MatrizWesparsaFromVizinhosNormGetis()
        {
            int nzmax = 0;
            int[] ind_vizinhos = new int[0];
            for (int i = 0; i < this.m_shape.Count; i++)
            {
                ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
                for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
                {
                    nzmax++;
                }
            }

            clsMatrizEsparsa a = new clsMatrizEsparsa(this.m_shape.Count, this.m_shape.Count, nzmax, true);

            int indice = 0;
            for (int i = 0; i < this.m_shape.Count; i++)
            {
                ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
                for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
                {
                        a.x[indice] = 1.0;
                        a.i[indice] = i;
                        a.p[indice] = ind_vizinhos[k];
                        indice++;
                }
            }

            //----------------- calculando a soma de cada linha a partir da matriz esparsa na triplet forma ----------//

            int[] ai = a.i;
            double[] ax = a.x;
            double[] soma_rows = new double[a.m];
            for (int i = 0; i < ax.GetLength(0); i++)
            {
                soma_rows[ai[i]] += ax[i];
            }

            for (int i = 0; i < ax.GetLength(0); i++)
            {
                ax[i] = ax[i] / soma_rows[ai[i]];
            }

            a.x = ax;

            //----------------- convertendo para matriz esparsa na forma compressed-column ---------------------------//

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            m_W_esparsa = fme.TripletForm2CompressColumn(a);

            m_tipo_vizinhanca_anterior = TipoMatrizVizinhanca.Normalizada;
        }

        public void MatrizWesparsaFromVizinhosComPesos()
        {
            int nzmax = 0;
            int[] ind_vizinhos = new int[0];
            double[] pesos = new double[0];
            for (int i = 0; i < this.m_shape.Count; i++)
            {
                ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
                for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
                {
                    if (i != ind_vizinhos[k]) nzmax++;
                }
            }

            clsMatrizEsparsa a = new clsMatrizEsparsa(this.m_shape.Count, this.m_shape.Count, nzmax, true);

            int indice = 0;
            for (int i = 0; i < this.m_shape.Count; i++)
            {
                ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
                pesos = this.m_shape[i].ListaPesosVizinhos;
                for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
                {
                    if (i != ind_vizinhos[k])
                    {
                        a.x[indice] = pesos[k];
                        a.i[indice] = i;
                        a.p[indice] = ind_vizinhos[k];
                        indice++;
                    }
                }
            }

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            m_W_esparsa = fme.TripletForm2CompressColumn(a);

            m_tipo_vizinhanca_anterior = TipoMatrizVizinhanca.Original;
        }

        public void MatrizWesparsaFromVizinhos()
        {
            int nzmax = 0;
            int[] ind_vizinhos = new int[0];
            for (int i = 0; i < this.m_shape.Count; i++)
            {
                ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
                for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
                {
                    if (i != ind_vizinhos[k]) nzmax++;
                }
            }

            clsMatrizEsparsa a = new clsMatrizEsparsa(this.m_shape.Count, this.m_shape.Count, nzmax, true);

            int indice = 0;
            for (int i = 0; i < this.m_shape.Count; i++)
            {
                ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
                for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
                {
                    if (i != ind_vizinhos[k])
                    {
                        a.x[indice] = 1.0;
                        a.i[indice] = i;
                        a.p[indice] = ind_vizinhos[k];
                        indice++;
                    }
                }
            }

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            m_W_esparsa = fme.TripletForm2CompressColumn(a);

            m_tipo_vizinhanca_anterior = TipoMatrizVizinhanca.Original;
        }

        public void MatrizWesparsaFromVizinhosGetis()
        {
            int nzmax = 0;
            int[] ind_vizinhos = new int[0];
            for (int i = 0; i < this.m_shape.Count; i++)
            {
                ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
                for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
                {
                    nzmax++;
                }
            }

            clsMatrizEsparsa a = new clsMatrizEsparsa(this.m_shape.Count, this.m_shape.Count, nzmax, true);

            int indice = 0;
            for (int i = 0; i < this.m_shape.Count; i++)
            {
                ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
                for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
                {
                        a.x[indice] = 1.0;
                        a.i[indice] = i;
                        a.p[indice] = ind_vizinhos[k];
                        indice++;
                }
            }

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            m_W_esparsa = fme.TripletForm2CompressColumn(a);

            m_tipo_vizinhanca_anterior = TipoMatrizVizinhanca.Original;
        }

        public void MatrizWFromVizinhos()
        {
            this.m_W = new double[this.m_shape.Count, this.m_shape.Count];
            int[] ind_vizinhos = new int[0];

            for (int i = 0; i < this.m_shape.Count; i++)
            {
                ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
                for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
                {
                    this.m_W[i, ind_vizinhos[k]] = 1.0;
                    this.m_W[ind_vizinhos[k], i] = 1.0;
                }
            }
            for (int i = 0; i < this.m_shape.Count; i++)
            {
                this.m_W[i, i] = 0.0;
            }
        }

        public void MatrizWFromVizinhosNorm()
        {
            this.m_W = new double[this.m_shape.Count, this.m_shape.Count];
            int[] ind_vizinhos = new int[0];

            for (int i = 0; i < this.m_shape.Count; i++)
            {
                ind_vizinhos = this.m_shape[i].ListaIndicesVizinhos;
                for (int k = 0; k < ind_vizinhos.GetLength(0); k++)
                {
                    this.m_W[i, ind_vizinhos[k]] = 1.0 / ((double)ind_vizinhos.GetLength(0));
                    this.m_W[ind_vizinhos[k], i] = 1.0 / ((double)m_shape[ind_vizinhos[k]].ListaIndicesVizinhos.GetLength(0));
                }
            }
            for (int i = 0; i < this.m_shape.Count; i++)
            {
                this.m_W[i, i] = 0.0;
            }
        }

        /// <summary>
        /// Função para gerar os vetores de significância dos coeficientes da regressão, a partir 
        /// da matriz de variância-covariância dos estimadores. Pode-se utilizar essa função para
        /// os diversos estimadores de modelos de regressão.
        /// </summary>
        protected void GeraSignificanciaCoeficientes(bool gera_residuos)
        {
            MathNormaldist normal = new MathNormaldist();

            this.m_beta_stderror = new double[this.m_beta_hat.GetLength(0), 1];
            this.m_beta_tstat = new double[this.m_beta_hat.GetLength(0), 1];
            this.m_beta_pvalor = new double[this.m_beta_hat.GetLength(0), 1];
            this.m_beta_liminf_ci = new double[this.m_beta_hat.GetLength(0), 1];
            this.m_beta_limsup_ci = new double[this.m_beta_hat.GetLength(0), 1];

            for (int i = 0; i < this.m_beta_hat.GetLength(0); i++)
            {
                double prob_cauda = this.m_prob_confidence_intervals / 200.0;
                double cv = normal.invcdf(1.0 - prob_cauda);

                m_beta_stderror[i, 0] = Math.Sqrt(m_beta_hat_cov[i, i]);
                m_beta_tstat[i, 0] = m_beta_hat[i, 0] / m_beta_stderror[i, 0];
                m_beta_pvalor[i, 0] = 2.0 * (1.0 - normal.cdf(Math.Abs(m_beta_tstat[i, 0])));
                m_beta_liminf_ci[i, 0] = m_beta_hat[i, 0] - cv * m_beta_stderror[i, 0];
                m_beta_limsup_ci[i, 0] = m_beta_hat[i, 0] + cv * m_beta_stderror[i, 0];
            }

            if (gera_residuos)
            {
                clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
                clsUtilTools clsut = new clsUtilTools();

                if (m_X.GetLength(1) == m_beta_hat.GetLength(0) && m_X.GetLength(1) > 0)
                {
                    double[,] predito = clsut.MatrizMult(m_X, m_beta_hat);
                    if (this.m_tipo_modelo == TipoModeloEspacial.SAR)
                    {
                        //predito = clsut.MatrizSoma(predito, clsut.MatrizMult(this.m_rho_hat, clsut.MatrizMult(this.m_W, m_Y)));
                        predito = clsut.MatrizSoma(predito, clsut.MatrizMult(this.m_rho_hat, fme.MatrizMult(this.m_W_esparsa, m_Y)));
                    }

                    m_residuos = clsut.MatrizSubtracao(m_Y, predito);
                    //m_res_versus_w = clsut.MatrizMult(m_W, m_residuos);

                    if (m_tipo_modelo != TipoModeloEspacial.nao_espacial)
                    {
                        m_res_versus_w = fme.MatrizMult(m_W_esparsa, m_residuos);
                    }
                }
            }
        }

        /// <summary>
        /// Função para gerar os vetores de significância dos coeficientes da regressão, a partir 
        /// da matriz de variância-covariância dos estimadores. Pode-se utilizar essa função para
        /// os diversos estimadores de modelos de regressão.
        /// </summary>
        protected void GeraSignificanciaCoeficientes()
        {
            GeraSignificanciaCoeficientes(true);
        }

        #endregion

        #region funções kernel

        protected double Biquadrado(double d, double d_max)
        {
            if (Math.Abs(d) < Math.Abs(d_max))
            {
                return Math.Pow(1.0 - Math.Pow(d / d_max, 2.0), 2.0);
            }
            return 0.0;
        }

        protected double Barlett(double d, double d_max)
        {
            if (Math.Abs(d) < Math.Abs(d_max))
            {
                return 1.0 - Math.Abs(d / d_max);
            }
            return 0.0;
        }

        protected double Epanechnikov(double d, double d_max)
        {
            if (Math.Abs(d) < Math.Abs(d_max))
            {
                return 1.0 - Math.Pow(d / d_max, 2.0);
            }
            return 0.0;
        }

        #endregion
    }
}
