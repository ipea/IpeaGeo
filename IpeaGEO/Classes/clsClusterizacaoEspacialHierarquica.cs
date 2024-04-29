using System;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo
{
    #region Tipos de métodos e distâncias

    public enum MetodoClusterizacao : int
    {
        Ward = 1,
        AverageLinkage = 2,
        Centroid = 3,
        CompleteLinkage = 4,
        EML = 5,
        SingleLinkage = 6,
        Median = 7,
        AverageLinkageWeigthed = 8
    };

    public enum TipoDistanciaContinua : int
    {
        Euclidiana = 1,
        L1Norm = 2,
        LpNorm = 3,
        VarianceCorrected = 4,
        CovarianceCorrected = 5
    };

    public enum TipoDistanciaBinaria : int
    {
        Jaccard = 1,
        Tanimoto = 2,
        SimpleMatching = 3,
        RusselRao = 4,
        Dice = 5,
        Kulczynski = 6
    };

    public enum TipoDistanciaCategorica : int
    {
        SimpleMatching = 1
    };

    public enum TipoDistanciaOrdinal : int
    {
        Euclidiana = 1,
        L1Norm = 2,
        VarianceCorrected = 3,
        L1NormRescaled = 4
    };

    public enum TipoDadosClusterizacao : int
    {
        Continuos = 1,
        Binarios = 2,
        Categoricos = 3,
        Ordinais = 4,
        Mistos = 5
    };

    #endregion 

    public class clsClusterizacaoEspacialHierarquica
    {
        private delegate double FuncaoDistancia(double[,] v1, double[,] v2);
        private FuncaoDistancia m_funcao_distancia;

        public clsClusterizacaoEspacialHierarquica()
        {
            this.m_funcao_distancia = new FuncaoDistancia(this.DistanciaContinuaPoligonos);
        }

        #region Variáveis internas

        private int m_tamanho_maximo_used_cluster = 100;

        private int m_TamanhoMaximoClusters = 100;
        public int TamanhoMaximoClusters
        {
            get { return this.m_TamanhoMaximoClusters; }
            set { this.m_TamanhoMaximoClusters = value; }
        }

        private double m_PercentualMaximoClusters = 10.0;
        public double PercentualMaximoClusters
        {
            get { return this.m_PercentualMaximoClusters; }
            set {this.m_PercentualMaximoClusters = value;}
        }

        private bool m_limita_maximo_tamanho_freq = false;
        public bool LimitaTamanhoMaximoFreq
        {
            get { return this.m_limita_maximo_tamanho_freq; }
            set 
            {
                this.m_limita_maximo_tamanho_freq = value;
                if (value) this.m_limita_maximo_tamanho_perc = false;
            }
        }

        private bool m_limita_maximo_tamanho_perc = false;
        public bool LimitaTamanhoMaximoPerc
        {
            get { return this.m_limita_maximo_tamanho_perc; }
            set 
            { 
                this.m_limita_maximo_tamanho_perc = value;
                if (value) this.m_limita_maximo_tamanho_freq = false;
            }
        }

        private double m_corte_variavel_binaria = 0.0;
	    
        /// <summary>
        /// Gets or sets the cut value for binary variables. If y > cut_value, or 
        /// y less than equal to cut_value. The default is 0.0.
        /// </summary>
        public double CorteVariavelBinaria
        {
            get { return this.m_corte_variavel_binaria; }
            set { this.m_corte_variavel_binaria = value; }
        }

        private int m_numero_minimo_possivel_clusters = 1;
        public int NumeroMinimoPossivelClusters
        {
            get { return this.m_numero_minimo_possivel_clusters; }
        }

        private TipoDadosClusterizacao m_tipo_dados_clusterizacao = TipoDadosClusterizacao.Continuos;
        public TipoDadosClusterizacao TipoDadosClusterizacao
        {
            get { return this.m_tipo_dados_clusterizacao; }
            set 
            { 
                this.m_tipo_dados_clusterizacao = value;
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Continuos)
                    this.m_funcao_distancia = new FuncaoDistancia(this.DistanciaContinuaPoligonos);
                else
                {
                    if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Binarios)
                    {
                        this.m_funcao_distancia = new FuncaoDistancia(this.DistanciaBinariaPoligonos);
                    }
                    else
                    {
                        if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Categoricos)
                        {
                            this.m_funcao_distancia = new FuncaoDistancia(this.DistanciaCategoricaPoligonos);
                        }
                        else
                        {
                            this.m_funcao_distancia = new FuncaoDistancia(this.DistanciaOrdinalPoligonos);
                        }
                    }
                }
            }
        }

        private bool m_ja_calculou_escala_distancia_ordinal = false;
        private TipoDistanciaOrdinal m_tipo_distancia_ordinal = TipoDistanciaOrdinal.L1NormRescaled;
        public TipoDistanciaOrdinal TipoDistanciaOrdinal
        {
            get { return this.m_tipo_distancia_ordinal; }
            set 
            { 
                this.m_tipo_distancia_ordinal = value;
                m_ja_calculou_escala_distancia_ordinal = false;
            }
        }

        private TipoDistanciaCategorica m_tipo_distancia_categorica = TipoDistanciaCategorica.SimpleMatching;
        public TipoDistanciaCategorica TipoDistanciaCategorica
        {
            get { return this.m_tipo_distancia_categorica; }
            set { this.m_tipo_distancia_categorica = value; }
        }

        private TipoDistanciaBinaria m_tipo_distancia_binaria = TipoDistanciaBinaria.Dice;
        public TipoDistanciaBinaria TipoDistanciaBinaria
        {
            get { return this.m_tipo_distancia_binaria; }
            set { this.m_tipo_distancia_binaria = value; }
        }

        private MetodoClusterizacao m_tipo_metodo_clusterizacao = MetodoClusterizacao.Ward;
        public MetodoClusterizacao TipoMetodoClusterizacao
        {
            get { return this.m_tipo_metodo_clusterizacao; }
            set { this.m_tipo_metodo_clusterizacao = value; }
        }

        private TipoDistanciaContinua m_tipo_distancia_continua = TipoDistanciaContinua.Euclidiana;
        public TipoDistanciaContinua TipoDistanciaContinua
        {
            set 
            { 
                this.m_tipo_distancia_continua = value;

                if (this.dados_continuos == null || this.dados_continuos.GetLength(0) == 0 || this.dados_continuos.GetLength(1) == 0)
                {
                    clsUtilTools clt = new clsUtilTools();
                    if (this.dados != null && this.dados.GetLength(0) > 0 && this.dados.GetLength(1) > 0)
                    {
                        this.dados_continuos = clt.ArrayDoubleClone(this.dados);
                    }
                }

                if (this.m_tipo_distancia_continua == TipoDistanciaContinua.VarianceCorrected)
                {
                    if (this.dados_continuos != null && this.dados_continuos.GetLength(0) > 0 && this.dados_continuos.GetLength(1) > 0)
                        this.ConstroiVarianciasVariaveis();
                }

                if (this.m_tipo_distancia_continua == TipoDistanciaContinua.CovarianceCorrected)
                {
                    if (this.dados_continuos != null && this.dados_continuos.GetLength(0) > 0 && this.dados_continuos.GetLength(1) > 0)
                        this.ControiInvCovarianciasVariaveis();
                }
            }
            get 
            { 
                return this.m_tipo_distancia_continua; 
            }
        }

        private double m_parameter_Lp_norm = 2.0;
        public double ParameterLpNorm
        {
            set { this.m_parameter_Lp_norm = value; }
            get { return this.m_parameter_Lp_norm; }
        }

        private double[,] m_cluster_tree;
        public double[,] ClusterTree
        {
            get
            {
                clsUtilTools clt = new clsUtilTools();
                return clt.ArrayDoubleClone(m_cluster_tree);
            }
        }

        private int m_num_max_clusters_arvore = 100;
        public int NumMaxClustersArvore
        {
            get { return this.m_num_max_clusters_arvore; }
            set 
            { 
                this.m_num_max_clusters_arvore = value;
                this.m_cluster_tree = null;
            }
        }
        
        private int m_num_min_clusters_arvore = 1;
        public int NumMinClustersArvore
        {
            get { return this.m_num_min_clusters_arvore; }
            set 
            { 
                this.m_num_min_clusters_arvore = value;
                this.m_cluster_tree = null;
            }
        }
         
        private clsIpeaShape m_estrutura_shape;
        public clsIpeaShape EstruturaShape
        {
            set 
            { 
                //this.m_estrutura_shape = (IpeaGEO.clsIpeaShape)value.Clone(); 
                this.m_estrutura_shape = value.ConvertToIpeaGEOShape();
            }
        }

        private double[,] dados_continuos = new double[0, 0];
        public double[,] DadosContinuosClusterizacao
        {
            set
            {
                clsUtilTools utl = new clsUtilTools();
                this.dados_continuos = utl.ArrayDoubleClone(value);
                this.nobs = this.dados_continuos.GetLength(0);

                if (this.m_tipo_distancia_continua == TipoDistanciaContinua.VarianceCorrected)
                {
                    if (this.dados_continuos != null && this.dados_continuos.GetLength(0) > 0 && this.dados_continuos.GetLength(1) > 0)
                        this.ConstroiVarianciasVariaveis();
                }

                if (this.m_tipo_distancia_continua == TipoDistanciaContinua.CovarianceCorrected)
                {
                    if (this.dados_continuos != null && this.dados_continuos.GetLength(0) > 0 && this.dados_continuos.GetLength(1) > 0)
                        this.ControiInvCovarianciasVariaveis();
                }

                if (this.dados == null || this.dados.GetLength(0) == 0 || this.dados.GetLength(1) == 0)
                {
                    this.dados = utl.ArrayDoubleClone(this.dados_continuos);
                }
            }
        }

        private double[,] dados_binarios = new double[0, 0];
        public double[,] DadosBinariosClusterizacao
        {
            set
            {
                clsUtilTools utl = new clsUtilTools();
                this.dados_binarios = utl.ArrayDoubleClone(value);
                this.nobs = this.dados_binarios.GetLength(0);
            }
        }

        private double[,] dados_categoricos = new double[0, 0];
        public double[,] DadosCategoricosClusterizacao
        {
            set
            {
                clsUtilTools utl = new clsUtilTools();
                this.dados_categoricos = utl.ArrayRanks(value);
                this.nobs = this.dados_categoricos.GetLength(0);
            }
        }

        private double[,] dados_ordinais = new double[0, 0];
        public double[,] DadosOrdinaisClusterizacao
        {
            set
            {
                clsUtilTools utl = new clsUtilTools();
                this.dados_ordinais = utl.ArrayRanks(value);
                this.nobs = this.dados_ordinais.GetLength(0);
            }
        }

        private int nobs = 0;
        private int nvars = 0;
        private double[,] dados;
        public double[,] DadosClusterizacao
        {
            set
            {
                clsUtilTools utl = new clsUtilTools();
                this.dados = utl.ArrayDoubleClone(value);
                this.nobs = this.dados.GetLength(0);
                this.nvars = this.dados.GetLength(1);

                if (this.m_tipo_distancia_continua == TipoDistanciaContinua.VarianceCorrected 
                    && (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Continuos || this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Mistos))
                {
                    if (this.dados_continuos == null || this.dados_continuos.GetLength(0) == 0 || this.dados_continuos.GetLength(1) == 0)
                    {
                        this.dados_continuos = utl.ArrayDoubleClone(this.dados);
                    }
                    if (this.dados_continuos != null && this.dados_continuos.GetLength(0) > 0 && this.dados_continuos.GetLength(1) > 0)
                    {
                        this.ConstroiVarianciasVariaveis();
                    }
                }

                if (this.m_tipo_distancia_continua == TipoDistanciaContinua.CovarianceCorrected 
                    && (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Continuos || this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Mistos))
                {
                    if (this.dados_continuos == null || this.dados_continuos.GetLength(0) == 0 || this.dados_continuos.GetLength(1) == 0)
                    {
                        this.dados_continuos = utl.ArrayDoubleClone(this.dados);
                    }
                    if (this.dados_continuos != null && this.dados_continuos.GetLength(0) > 0 && this.dados_continuos.GetLength(1) > 0)
                    {
                        this.ControiInvCovarianciasVariaveis();
                    }
                }
            }
            get 
            {
                clsUtilTools utl = new clsUtilTools();
                return utl.ArrayDoubleClone(this.dados); 
            }
        }

        private double m_parameter_p_EML_method = 2.0;
        public double ParameterP_EMLmethod
        {
            get { return this.m_parameter_p_EML_method; }
            set { this.m_parameter_p_EML_method = value; }
        }

        private int[] m_lista_clusters_atuais;
        public int[] ListaClusters
        {
            get { return this.m_lista_clusters_atuais; }
        }

        private int[] m_indice_clusters_atuais;
        public int[] IndicesClusters
        {
            get { return this.m_indice_clusters_atuais; }
        }

        private double m_peso_distancia_categorica = 1.0;
        public double PesoDistanciaCategoricaDadosMistos
        {
            get { return this.m_peso_distancia_categorica; }
            set { this.m_peso_distancia_categorica = value; }
        }

        private double m_peso_distancia_binaria = 1.0;
        public double PesoDistanciaBinariaDadosMistos
        {
            get { return this.m_peso_distancia_binaria; }
            set { this.m_peso_distancia_binaria = value; }
        }

        private double m_peso_distancia_ordinal = 1.0;
        public double PesoDistanciaOrdinalDadosMistos
        {
            get { return this.m_peso_distancia_ordinal; }
            set { this.m_peso_distancia_ordinal = value; }
        }

        #endregion 

        #region Critérios de seleção do número de clusters

        private void GeraCCC(ref double ccc, ref double expected_r2, int q, double r2)
        {
                int p = this.nvars;
                int pstar = (int)Math.Min(q, p+1);
	        double c = 0.0;
	        double upstar = 0.5;
	        double vstar = 0.0;
	        int n = this.nobs;

	        while (upstar < 1.0)	
	        {
		        pstar--;
		        if (pstar <= 1) break;
		        vstar = 1.0;
		        for (int i=0; i <= pstar - 1; i++)
			        vstar *= m_ordered_sj[i];
        		
		        c = Math.Pow(vstar / (double)q, 1.0/((double)pstar));
		        upstar = m_ordered_sj[pstar-1] / c;
	        }	

	        double[] uj = new double[p];
	        vstar = 1.0;
	        for (int i=0; i<=pstar-1; i++)
		        vstar *= m_ordered_sj[i];

	        c = Math.Pow(vstar / ((double)q), 1.0/((double)pstar));
        	
	        for (int i=0; i<=p-1; i++)
		        uj[i] = m_ordered_sj[i] / c;

	        double soma1 = 0.0;
	        double soma2 = 0.0;

	        for (int i=0; i<=pstar-1; i++)
	        {
		        soma1 += 1.0 / (((double)n) + uj[i]);
		        soma2 += Math.Pow(uj[i], 2.0);
	        }
        	
	        for (int i=pstar; i<= p-1; i++)
	        {
		        soma1 += Math.Pow(uj[i], 2.0) / (((double)n) + uj[i]);
		        soma2 += Math.Pow(uj[i], 2.0);
	        }

	        expected_r2 = 1.0 - (soma1 / soma2)*(Math.Pow((double)n - (double)q, 2.0) / (double)n) * (1.0 + (4.0 / (double)n));

	        ccc = Math.Log((1.0 - expected_r2)/(1.0 - r2)) * (Math.Sqrt((double)pstar * (double)n / 2.0)/(Math.Pow(0.001 + expected_r2, 1.2)));
        }

        private double[] m_ordered_sj = new double[0];
        private void CalculaOrderedSj()
        {
            clsUtilTools clt = new clsUtilTools();
            double[,] matrix_X = new double[this.dados.GetLength(0), this.dados.GetLength(1)];
            double[,] media_X = clt.Meanc(this.dados);
            for (int i = 0; i < matrix_X.GetLength(0); i++)
                for (int j = 0; j < matrix_X.GetLength(1); j++)
                    matrix_X[i, j] = this.dados[i, j] - media_X[0, j];

            double[,] matrix_V = clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(matrix_X), matrix_X), 1.0 / (double)(this.nobs-1));

            clt.AutovaloresMatrizSimetrica(matrix_V, ref matrix_V, ref this.m_ordered_sj);

            for (int i = 0; i < this.m_ordered_sj.GetLength(0); i++)
                this.m_ordered_sj[i] = Math.Sqrt(this.m_ordered_sj[i]);
        }

        private double m_T_to_nclus_criterion = 0.0;
        private void CalculaValorTtoNclusCriterion()
        {
            clsUtilTools utl = new clsUtilTools();
            this.m_T_to_nclus_criterion = 0.0;
            double[,] v1 = utl.Meanc(this.dados);
            for (int i = 0; i < this.dados.GetLength(0); i++) 
            {
                for (int j = 0; j < this.dados.GetLength(1); j++)
                {
                    this.m_T_to_nclus_criterion += Math.Pow(this.dados[i, j] - v1[0, j], 2.0);
                }
            }
        }

        private double m_Pg_to_nclus_criterion = 0.0;
        private double[] m_W_to_nclus_criterion = new double[0];
        private double m_Bkl_to_nclus_criterion = 0.0;
        private double AtualizaValorWtoNclusCriterion(int k1)
        {
            clsUtilTools utl = new clsUtilTools();
            int n1 = temp_shape[k1].IndicesPoligonosNoCluster.GetLength(0);
            double[,] m1 = new double[n1, this.nvars];

            for (int i = 0; i < m1.GetLength(0); i++)
            {
                for (int j = 0; j < this.nvars; j++)
                {
                    m1[i, j] = this.dados[temp_shape[k1].IndicesPoligonosNoCluster[i], j];
                }
            }
            double W1 = 0.0;
            double[,] v1 = utl.Meanc(m1);
            for (int i = 0; i < m1.GetLength(0); i++) 
            {
                for (int j = 0; j < m1.GetLength(1); j++)
                {
                    W1 += Math.Pow(m1[i,j] - v1[0,j], 2.0);
                }
            }
            return W1;
        }

        private double[] m_sequencia_CCC = new double[0];
        private double[] m_sequencia_pseudo_F = new double[0];
        private double[] m_sequencia_pseudo_t = new double[0];
        private double[] m_sequencia_R2 = new double[0];
        private double[] m_sequencia_partial_R2 = new double[0];
        private double[] m_sequencia_nclusters = new double[0];
        private double[] m_sequencia_expected_R2 = new double[0];
        private double[] m_sequencia_nobs_novo_cluster = new double[0];

        public double[] SequenciaCCC { get { return this.m_sequencia_CCC; } }
        public double[] SequenciaPseudoF { get { return this.m_sequencia_pseudo_F; } }
        public double[] SequenciaPseudoT { get { return this.m_sequencia_pseudo_t; } }
        public double[] SequenciaR2 { get { return this.m_sequencia_R2; } }
        public double[] SequenciaPartialR2 { get { return this.m_sequencia_partial_R2; } }
        public double[] SequenciaNClusters { get { return this.m_sequencia_nclusters; } }
        public double[] SequenciaExpectedR2 { get { return this.m_sequencia_expected_R2; } }
        public double[] SequenciaNobsNovoCluster { get { return this.m_sequencia_nobs_novo_cluster; } }

        #endregion 

        #region Medidas de variabilidade intra-clusters

        private double[,] m_dados_vars_binarias = new double[0, 0];

        private void CalculaMedidaVariabilidadeTotal()
        {
            if (this.m_variancias_variaveis.GetLength(0) == 0) this.ConstroiVarianciasVariaveis();
            if (this.m_matriz_inv_covariancia.GetLength(0) == 0) this.ControiInvCovarianciasVariaveis();

            clsUtilTools clt = new clsUtilTools();
            double[,] media_dados_cluster = clt.Meanc(this.dados);
            double[,] aux_dados = new double[0, 0];
            double[,] median_dados_cluster = clt.Medianc(this.dados);

            m_total_norma_L2 = 0.0;
            m_total_norma_L1 = 0.0;
            m_total_norma_Lp = 0.0;
            m_total_cov_corrected_L2 = 0.0;
            m_total_var_corrected_L2 = 0.0;

            this.m_dados_vars_binarias = new double[nobs, nvars];
            if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Binarios)
            {
                for (int i = 0; i < nobs; i++)
                {
                    for (int j = 0; j < nvars; j++)
                    {
                        if (this.dados[i, j] > this.m_corte_variavel_binaria) this.m_dados_vars_binarias[i, j] = 1.0;
                        else this.m_dados_vars_binarias[i, j] = 0.0;
                    }
                }
            }

            if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Continuos)
            {
                for (int i = 0; i < nobs; i++)
                {
                    for (int j = 0; j < nvars; j++)
                    {
                        if (this.dados[i, j] > median_dados_cluster[0,j]) this.m_dados_vars_binarias[i, j] = 1.0;
                        else this.m_dados_vars_binarias[i, j] = 0.0;
                    }
                }
            }

            double[,] media_dados_binarios = clt.Meanc(this.m_dados_vars_binarias);

            for (int j = 0; j < this.nobs; j++)
            {
                //-------------------- medidas para variáveis contínuas -------------------------------------------//

                aux_dados = clt.DiffArrayDouble(clt.SubRowArrayDouble(this.dados, j), media_dados_cluster);

                for (int m = 0; m < this.nvars; m++)
                {
                    this.m_total_norma_L2 += Math.Pow(aux_dados[0, m], 2.0);
                    this.m_total_norma_L1 += Math.Abs(aux_dados[0, m]);
                    this.m_total_norma_Lp += Math.Pow(Math.Abs(aux_dados[0, m]), this.m_parameter_Lp_norm);
                    this.m_total_var_corrected_L2 += Math.Pow(aux_dados[0, m], 2.0) / this.m_variancias_variaveis[m];
                }

                this.m_total_cov_corrected_L2 +=
                    (clt.MatrizMult(clt.MatrizMult(aux_dados, this.m_matriz_inv_covariancia), clt.MatrizTransp(aux_dados)))[0, 0];

                //--------------------- medidas para variáveis binárias -------------------------------------------//
            }
        }

        private double m_total_norma_L2 = 0.0;
        private double m_total_norma_L1 = 0.0;
        private double m_total_norma_Lp = 0.0;
        private double m_total_cov_corrected_L2 = 0.0;
        private double m_total_var_corrected_L2 = 0.0;

        private void CalculaMedidasVariabilidade(int posicao_indice)
        {
            m_intracluster_norma_L2[posicao_indice] = 0.0;
            m_intracluster_norma_L1[posicao_indice] = 0.0;
            m_intracluster_norma_Lp[posicao_indice] = 0.0;
            m_intracluster_cov_corrected_L2[posicao_indice] = 0.0;
            m_intracluster_var_corrected_L2[posicao_indice] = 0.0;

            if (this.m_variancias_variaveis.GetLength(0) == 0) this.ConstroiVarianciasVariaveis();
            if (this.m_matriz_inv_covariancia.GetLength(0) == 0) this.ControiInvCovarianciasVariaveis();

            double[,] dados_cluster = new double[0, 0];
            int n_dados_cluster = 0;
            int indice = 0;
            clsUtilTools clt = new clsUtilTools();
            double[,] media_dados_cluster = new double[0, 0];
            double[,] aux_dados = new double[0,0];

            for (int i = 0; i < this.m_lista_clusters_atuais.GetLength(0); i++)
            {
                //------------------- separando as observações dentro de cada cluster ------------------------//

                n_dados_cluster = 0;
                for (int k = 0; k < this.nobs; k++)
                {
                    if (this.m_indice_clusters_atuais[k] == this.m_lista_clusters_atuais[i])
                        n_dados_cluster++;
                }

                indice = 0;
                dados_cluster = new double[n_dados_cluster, nvars];
                for (int k = 0; k < this.nobs; k++)
                {
                    if (this.m_indice_clusters_atuais[k] == this.m_lista_clusters_atuais[i])
                    {
                        for (int j = 0; j < this.nvars; j++)
                        {
                            dados_cluster[indice, j] = this.dados[k, j];
                        }
                        indice++;
                    }
                }

                //------------------------ Calculando as medidas de variabilidade ---------------------------//

                media_dados_cluster = clt.Meanc(dados_cluster);

                for (int j = 0; j < n_dados_cluster; j++)
                {
                    //-------------------- medidas para variáveis contínuas -------------------------------------------//

                    aux_dados = clt.DiffArrayDouble(clt.SubRowArrayDouble(dados_cluster, j), media_dados_cluster);

                    for (int m = 0; m < this.nvars; m++)
                    {
                        this.m_intracluster_norma_L2[posicao_indice] += Math.Pow(aux_dados[0, m], 2.0);
                        this.m_intracluster_norma_L1[posicao_indice] += Math.Abs(aux_dados[0, m]);
                        this.m_intracluster_norma_Lp[posicao_indice] += Math.Pow(Math.Abs(aux_dados[0, m]), this.m_parameter_Lp_norm);
                        this.m_intracluster_var_corrected_L2[posicao_indice] += Math.Pow(aux_dados[0, m], 2.0) / this.m_variancias_variaveis[m];
                    }

                    this.m_intracluster_cov_corrected_L2[posicao_indice] += 
                        (clt.MatrizMult(clt.MatrizMult(aux_dados, this.m_matriz_inv_covariancia), clt.MatrizTransp(aux_dados)))[0,0];

                    //--------------------- medidas para variáveis binárias -------------------------------------------//
                }
            }

            this.m_intracluster_norma_L2[posicao_indice] /= this.m_total_norma_L2;
            this.m_intracluster_norma_L1[posicao_indice] /= this.m_total_norma_L1;
            this.m_intracluster_norma_Lp[posicao_indice] /= this.m_total_norma_Lp;
            this.m_intracluster_var_corrected_L2[posicao_indice] /= this.m_total_var_corrected_L2;
            this.m_intracluster_cov_corrected_L2[posicao_indice] /= this.m_total_cov_corrected_L2;
        }

        private double[] m_intracluster_norma_L2 = new double[0];
        private double[] m_intracluster_norma_L1 = new double[0];
        private double[] m_intracluster_norma_Lp = new double[0];
        private double[] m_intracluster_cov_corrected_L2 = new double[0];
        private double[] m_intracluster_var_corrected_L2 = new double[0];

        public double[] IntraclusterVarCorrectedL2 { get { return this.m_intracluster_var_corrected_L2; } }
        public double[] IntraclusterCovCorrectedL2 { get { return this.m_intracluster_cov_corrected_L2; } }
        public double[] IntraclusterNormaL2 { get { return this.m_intracluster_norma_L2; } }
        public double[] IntraclusterNormaL1 { get { return this.m_intracluster_norma_L1; } }
        public double[] IntraclusterNormaLp { get { return this.m_intracluster_norma_Lp; } }

        #endregion

        #region Clustering algorithms

        #region Preenche a matriz com todas as distâncias

        private MatrizArquivo m_distancias;
        private double m_maxima_distancia_matriz_inicial = -10.0;

        private void PreencheTodasDistanciasContinuas(ref ProgressBar prBar)
        {
            this.m_distancias = new MatrizArquivo(this.nobs, this.nobs);
            clsUtilTools clt = new clsUtilTools();
            double nova_distancia = 0.0;

            prBar.Maximum = this.nobs;
            prBar.Value = 0;

            if (this.m_tipo_dados_clusterizacao != TipoDadosClusterizacao.Mistos)
            {
                #region Dados contínuos, binários ou categóricos
                for (int i = 0; i < this.nobs; i++)
                {
                    prBar.Value = i;
                    Application.DoEvents();

                    if (this.m_tipo_metodo_clusterizacao == MetodoClusterizacao.Ward)
                    {
                        for (int j = i + 1; j < this.nobs; j++)
                        {
                            nova_distancia = 0.5 * this.m_funcao_distancia(clt.SubRowArrayDouble(this.dados, i), clt.SubRowArrayDouble(this.dados, j));
                            this.m_distancias[i, j] = nova_distancia;

                            if (nova_distancia > this.m_maxima_distancia_matriz_inicial) this.m_maxima_distancia_matriz_inicial = nova_distancia;
                        }
                    }
                    else
                    {
                        for (int j = i + 1; j < this.nobs; j++)
                        {
                            nova_distancia = this.m_funcao_distancia(clt.SubRowArrayDouble(this.dados, i), clt.SubRowArrayDouble(this.dados, j));
                            this.m_distancias[i, j] = nova_distancia;

                            if (nova_distancia > this.m_maxima_distancia_matriz_inicial) this.m_maxima_distancia_matriz_inicial = nova_distancia;
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region Dados mistos

                for (int i = 0; i < this.nobs; i++)
                {
                    prBar.Value = i;
                    Application.DoEvents();

                    if (this.m_tipo_metodo_clusterizacao == MetodoClusterizacao.Ward)
                    {
                        for (int j = i + 1; j < this.nobs; j++)
                        {
                            nova_distancia = 0.5 * this.DistanciaMistaPoligonos(clt.SubRowArrayDouble(this.dados_continuos, i), clt.SubRowArrayDouble(this.dados_continuos, j),
                                clt.SubRowArrayDouble(this.dados_binarios, i), clt.SubRowArrayDouble(this.dados_binarios, j),
                                clt.SubRowArrayDouble(this.dados_categoricos, i), clt.SubRowArrayDouble(this.dados_categoricos, j),
                                clt.SubRowArrayDouble(this.dados_ordinais, i), clt.SubRowArrayDouble(this.dados_ordinais, j));

                            this.m_distancias[i, j] = nova_distancia;

                            if (nova_distancia > this.m_maxima_distancia_matriz_inicial) this.m_maxima_distancia_matriz_inicial = nova_distancia;
                        }
                    }
                    else
                    {
                        for (int j = i + 1; j < this.nobs; j++)
                        {
                            nova_distancia = this.DistanciaMistaPoligonos(clt.SubRowArrayDouble(this.dados_continuos, i), clt.SubRowArrayDouble(this.dados_continuos, j),
                                clt.SubRowArrayDouble(this.dados_binarios, i), clt.SubRowArrayDouble(this.dados_binarios, j),
                                clt.SubRowArrayDouble(this.dados_categoricos, i), clt.SubRowArrayDouble(this.dados_categoricos, j),
                                clt.SubRowArrayDouble(this.dados_ordinais, i), clt.SubRowArrayDouble(this.dados_ordinais, j));

                            this.m_distancias[i, j] = nova_distancia;

                            if (nova_distancia > this.m_maxima_distancia_matriz_inicial) this.m_maxima_distancia_matriz_inicial = nova_distancia;
                        }
                    }
                }
                #endregion
            }
            
            prBar.Value = 0;
            prBar.Refresh();
            Application.DoEvents();
        }
        
        private void PreencheTodasDistanciasContinuas(ref ToolStripProgressBar prBar)
        {
            this.m_distancias = new MatrizArquivo(this.nobs, this.nobs);
            clsUtilTools clt = new clsUtilTools();
            double nova_distancia = 0.0;

            prBar.Maximum = this.nobs;
            prBar.Value = 0;

            if (this.m_tipo_dados_clusterizacao != TipoDadosClusterizacao.Mistos)
            {
                #region Dados contínuos, binários ou categóricos
                for (int i = 0; i < this.nobs; i++)
                {
                    prBar.Value = i;
                    Application.DoEvents();

                    if (this.m_tipo_metodo_clusterizacao == MetodoClusterizacao.Ward)
                    {
                        for (int j = i + 1; j < this.nobs; j++)
                        {
                            nova_distancia = 0.5 * this.m_funcao_distancia(clt.SubRowArrayDouble(this.dados, i), clt.SubRowArrayDouble(this.dados, j));
                            this.m_distancias[i, j] = nova_distancia;

                            if (nova_distancia > this.m_maxima_distancia_matriz_inicial) this.m_maxima_distancia_matriz_inicial = nova_distancia;
                        }
                    }
                    else
                    {
                        for (int j = i + 1; j < this.nobs; j++)
                        {
                            nova_distancia = this.m_funcao_distancia(clt.SubRowArrayDouble(this.dados, i), clt.SubRowArrayDouble(this.dados, j));
                            this.m_distancias[i, j] = nova_distancia;

                            if (nova_distancia > this.m_maxima_distancia_matriz_inicial) this.m_maxima_distancia_matriz_inicial = nova_distancia;
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region Dados mistos

                for (int i = 0; i < this.nobs; i++)
                {
                    prBar.Value = i;
                    Application.DoEvents();

                    if (this.m_tipo_metodo_clusterizacao == MetodoClusterizacao.Ward)
                    {
                        for (int j = i + 1; j < this.nobs; j++)
                        {
                            nova_distancia = 0.5 * this.DistanciaMistaPoligonos(clt.SubRowArrayDouble(this.dados_continuos, i), clt.SubRowArrayDouble(this.dados_continuos, j),
                                clt.SubRowArrayDouble(this.dados_binarios, i), clt.SubRowArrayDouble(this.dados_binarios, j),
                                clt.SubRowArrayDouble(this.dados_categoricos, i), clt.SubRowArrayDouble(this.dados_categoricos, j),
                                clt.SubRowArrayDouble(this.dados_ordinais, i), clt.SubRowArrayDouble(this.dados_ordinais, j));

                            this.m_distancias[i, j] = nova_distancia;

                            if (nova_distancia > this.m_maxima_distancia_matriz_inicial) this.m_maxima_distancia_matriz_inicial = nova_distancia;
                        }
                    }
                    else
                    {
                        for (int j = i + 1; j < this.nobs; j++)
                        {
                            nova_distancia = this.DistanciaMistaPoligonos(clt.SubRowArrayDouble(this.dados_continuos, i), clt.SubRowArrayDouble(this.dados_continuos, j),
                                clt.SubRowArrayDouble(this.dados_binarios, i), clt.SubRowArrayDouble(this.dados_binarios, j),
                                clt.SubRowArrayDouble(this.dados_categoricos, i), clt.SubRowArrayDouble(this.dados_categoricos, j),
                                clt.SubRowArrayDouble(this.dados_ordinais, i), clt.SubRowArrayDouble(this.dados_ordinais, j));

                            this.m_distancias[i, j] = nova_distancia;

                            if (nova_distancia > this.m_maxima_distancia_matriz_inicial) this.m_maxima_distancia_matriz_inicial = nova_distancia;
                        }
                    }
                }
                #endregion
            }

            prBar.Value = 0;
            //prBar.Refresh();
            Application.DoEvents();
        }

        #endregion

        #region Funções auxiliares

        private void CalculaLimiteMaximoTamanhoClusters()
        {
            //this.m_tamanho_maximo_used_cluster = 1 + this.dados.GetLength(0);
            this.m_tamanho_maximo_used_cluster = 1 + this.nobs;

            if (this.LimitaTamanhoMaximoFreq)
            {
                this.m_tamanho_maximo_used_cluster = this.m_TamanhoMaximoClusters; 
            }

            if (this.LimitaTamanhoMaximoPerc)
            {
                this.m_tamanho_maximo_used_cluster = 
                    (int)Math.Ceiling((double)this.nobs * (this.m_PercentualMaximoClusters / 100.0));

                //(int)Math.Ceiling((double)this.dados.GetLength(0) * (this.m_PercentualMaximoClusters / 100.0));
            }

            // this.m_tamanho_maximo_used_cluster = Math.Max(this.m_tamanho_maximo_used_cluster, 
            //    (int)Math.Ceiling((double)this.dados.GetLength(0) / (double)this.m_num_min_clusters_arvore));
        }

        public double[,] RetornaDadosComClusters(int numero_clusters)
        {
            if (this.m_cluster_tree == null) throw new Exception("Árvore de clusters não construída");
            if (numero_clusters > this.m_num_max_clusters_arvore || numero_clusters < this.m_num_min_clusters_arvore)
                throw new Exception("Número de clusters inválido para a árvore de clusters construída");

            int coluna = this.m_num_max_clusters_arvore - numero_clusters;

            clsUtilTools utl = new clsUtilTools();

            return utl.Concateh(this.dados, utl.SubColumnArrayDouble(this.m_cluster_tree, coluna));
        }

        private clsIpeaShape temp_shape = new clsIpeaShape();
        private double[] vetor_W = new double[0];

        #endregion 

        #region Arvore de clusters hierarquicos

        public void GeraArvoreContinuousHierarchicalClustering(ref ProgressBar prBar, ref Label lblEvolucao)
        {
            clsUtilTools utl = new clsUtilTools();

            #region Cheque sobre os dados da clusterização
            if (this.dados == null || this.dados.GetLength(0) == 0 || this.dados.GetLength(1) == 0)
            {
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Binarios) this.dados = utl.ArrayDoubleClone(this.dados_binarios);
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Continuos) this.dados = utl.ArrayDoubleClone(this.dados_continuos);
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Categoricos) this.dados = utl.ArrayDoubleClone(this.dados_categoricos);
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Ordinais) this.dados = utl.ArrayDoubleClone(this.dados_ordinais);

                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Mistos)
                {
                    this.dados = utl.Concateh(utl.Concateh(this.dados_continuos, this.dados_binarios),
                                              utl.Concateh(this.dados_categoricos, this.dados_ordinais));
                }
            }
            else
            {
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Continuos && this.dados_continuos.GetLength(0) == 0) this.dados_continuos = utl.ArrayDoubleClone(this.dados);
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Binarios && this.dados_binarios.GetLength(0) == 0) this.dados_binarios = utl.ArrayDoubleClone(this.dados);
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Categoricos && this.dados_categoricos.GetLength(0) == 0) this.dados_categoricos = utl.ArrayDoubleClone(this.dados);
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Ordinais && this.dados_ordinais.GetLength(0) == 0) this.dados_ordinais = utl.ArrayDoubleClone(this.dados);
            }
            #endregion

            if (this.dados == null) throw new Exception("Matriz de dados não definida para clusterização espacial");
            if (this.m_estrutura_shape == null) throw new Exception("Estrutura do shape não definida para clusterização espacial");

            this.nobs = this.dados.GetLength(0);
            this.nvars = this.dados.GetLength(1);

            this.CalculaLimiteMaximoTamanhoClusters();

            //------------ preenche a matriz com todas as distâncias entre observações ------------------------------------------//

            lblEvolucao.Text = "Geração da matriz de distâncias entre observações iniciais";
            lblEvolucao.Refresh();

            this.PreencheTodasDistanciasContinuas(ref prBar);

            lblEvolucao.Text = "Evolução da formação da árvore de clusters";
            lblEvolucao.Refresh();

            int min_nclus_tree = this.m_num_min_clusters_arvore;
            int max_nclus_tree = this.m_num_max_clusters_arvore;

            temp_shape = this.m_estrutura_shape.Clone();

            int nNk = 0;
            int nNl = 0;
            double wWk = 0.0;
            double wWl = 0.0;

            double nova_distancia = 0.0;
            double dist_RP = 0.0;
            double dist_RQ = 0.0;
            double dist_PQ = 0.0;
            int nP, nQ, nR;
            int indice_menor, indice_maior;

            //int nk1_temp = 0;
            //int nk2_temp = 0;

            //---------------------------------------------- Initializing cluster list ----------------------------------------//
            this.m_indice_clusters_atuais = new int[this.nobs];
            this.m_lista_clusters_atuais = new int[this.nobs];
            for (int i = 0; i < this.nobs; i++)
            {
                this.m_indice_clusters_atuais[i] = i;
                this.m_lista_clusters_atuais[i] = i;
                temp_shape[i].AddListaAllVizinhos(temp_shape[i].ListaIndicesVizinhos);
                temp_shape[i].IndiceCluster = i;
                temp_shape[i].AddIndicePoligonoNoCluster(i);
            }

            this.soma_Pg = 0.0;
            this.m_Pg_to_nclus_criterion = 0.0;
            this.CalculaValorTtoNclusCriterion();

            this.vetor_W = new double[this.nobs];
            this.m_W_to_nclus_criterion = new double[this.nobs];

            for (int i = 0; i < this.nobs; i++)
            {
                this.vetor_W[i] = 0.0;
                this.m_W_to_nclus_criterion[i] = 0.0;
            }

            this.m_sequencia_CCC = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_nclusters = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_partial_R2 = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_pseudo_F = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_pseudo_t = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_R2 = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_expected_R2 = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_nobs_novo_cluster = new double[max_nclus_tree - min_nclus_tree + 1];

            this.m_intracluster_cov_corrected_L2 = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_intracluster_norma_L1 = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_intracluster_norma_L2 = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_intracluster_norma_Lp = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_intracluster_var_corrected_L2 = new double[max_nclus_tree - min_nclus_tree + 1];

            this.CalculaOrderedSj();
            double ccc = 0.0;
            double expected_r2 = 0.0;

            //----------------------------------------------- Clustering steps ----------------------------------------------------------//
            int nclus = this.nobs;
            int k1 = 0;
            int k2 = 0;
            double distance = 0.0;
            double min_distance = 0.0;

            prBar.Maximum = this.nobs;

            int[] aux_lista = new int[0];
            int indice = 0;

            int[] indices_vizinhos = new int[0];

            double[,] aux_indices;
            bool tem_vizinhos = false;

            int posicao_indice_intraclusters = 0;
            this.CalculaMedidaVariabilidadeTotal();

            for (int iter = 0; iter < this.nobs - 1; iter++)
            {
                prBar.Increment(1);
                Application.DoEvents();

                //----------------------------------------- Checando se ainda há clusters vizinhos --------------------------------------//

                tem_vizinhos = false;
                for (int i = 0; i < this.m_lista_clusters_atuais.GetLength(0); i++)
                {
                    if (temp_shape[this.m_lista_clusters_atuais[i]].ListaIndicesVizinhos.GetLength(0) > 0)
                    {
                        tem_vizinhos = true;
                        break;
                    }
                }

                if (!tem_vizinhos)
                {
                    this.m_num_min_clusters_arvore = nclus;
                    this.m_numero_minimo_possivel_clusters = nclus;

                    utl.GeraSubRows(ref this.m_sequencia_CCC, this.m_sequencia_CCC, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_nclusters, this.m_sequencia_nclusters, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_partial_R2, this.m_sequencia_partial_R2, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_pseudo_F, this.m_sequencia_pseudo_F, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_R2, this.m_sequencia_R2, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_pseudo_t, this.m_sequencia_pseudo_t, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_expected_R2, this.m_sequencia_expected_R2, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_nobs_novo_cluster, this.m_sequencia_nobs_novo_cluster, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);

                    utl.GeraSubRows(ref this.m_intracluster_cov_corrected_L2, this.m_intracluster_cov_corrected_L2, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_intracluster_norma_L1, this.m_intracluster_norma_L1, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_intracluster_norma_L2, this.m_intracluster_norma_L2, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_intracluster_norma_Lp, this.m_intracluster_norma_Lp, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_intracluster_var_corrected_L2, this.m_intracluster_var_corrected_L2, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);

                    break;
                }

                //-----------------------------------------------------------------------------------------------------------------------//

                if (nclus <= this.m_num_min_clusters_arvore) break;

                k1 = this.m_lista_clusters_atuais[0];
                k2 = this.m_lista_clusters_atuais[1];
                distance = 0.0;

                //min_distance = this.DistanceClusters(k1, k2);

                min_distance = 1.0e200;
                if (k1 < k2) { min_distance = this.m_distancias[k1, k2]; }
                if (k2 < k1) { min_distance = this.m_distancias[k2, k1]; }

                //nk1_temp = temp_shape[k1].IndicesPoligonosNoCluster.GetLength(0);
                //nk2_temp = temp_shape[k2].IndicesPoligonosNoCluster.GetLength(0);

                //if ((nk1_temp + nk2_temp) > this.m_tamanho_maximo_used_cluster)
                //{
                //    min_distance += 1.0e200;
                //}

                for (int i = 0; i < nclus; i++)
                {
                    indices_vizinhos = temp_shape[this.m_lista_clusters_atuais[i]].ListaIndicesVizinhos;

                    for (int j = 0; j < indices_vizinhos.GetLength(0); j++)
                    {
                        if (this.m_lista_clusters_atuais[i] < indices_vizinhos[j])
                        {
                            distance = this.m_distancias[this.m_lista_clusters_atuais[i], indices_vizinhos[j]];

                            if (distance < min_distance
                                || ((distance <= min_distance)
                                && (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Binarios)))
                            {
                                //------------------------ controlando para o tamanho dos clusters ----------------------//
                                //nk1_temp = temp_shape[k1].IndicesPoligonosNoCluster.GetLength(0);
                                //nk2_temp = temp_shape[k2].IndicesPoligonosNoCluster.GetLength(0);

                                //if (nk1_temp + nk2_temp <= this.m_tamanho_maximo_used_cluster ||
                                //    nclus <= 2*(int)Math.Ceiling((double)this.dados.GetLength(0) / (double)this.m_tamanho_maximo_used_cluster))
                                //{
                                k1 = this.m_lista_clusters_atuais[i];
                                k2 = indices_vizinhos[j];
                                min_distance = distance;
                                //}
                            }
                        }
                    }
                }

                //------------------ junta clusters com menor distância -------------------//

                nclus--;
                for (int i = 0; i < this.nobs; i++)
                {
                    if (this.m_indice_clusters_atuais[i] == k2) this.m_indice_clusters_atuais[i] = k1;
                }

                indice = 0;
                aux_lista = new int[nclus];
                for (int i = 0; i <= nclus; i++)
                {
                    if (this.m_lista_clusters_atuais[i] != k2)
                    {
                        aux_lista[indice++] = this.m_lista_clusters_atuais[i];
                    }
                }
                this.m_lista_clusters_atuais = aux_lista;

                //----------------------- redefinição de vizinhanças dos clusters ---------------------------//

                temp_shape[k1].AddListaVizinhos(temp_shape[k2].ListaIndicesVizinhos);

                for (int i = 0; i < this.m_lista_clusters_atuais.GetLength(0); i++)
                {
                    if (temp_shape[this.m_lista_clusters_atuais[i]].EstaNaListaVizinhos(k2))
                    {
                        temp_shape[this.m_lista_clusters_atuais[i]].AddVizinho(k1);
                        temp_shape[this.m_lista_clusters_atuais[i]].DeleteVizinho(k2);
                    }
                }

                //------------------------- redefinição lista de polígonos no cluster -------------------------//

                nNk = temp_shape[k1].IndicesPoligonosNoCluster.GetLength(0);
                nNl = temp_shape[k2].IndicesPoligonosNoCluster.GetLength(0);
                wWk = this.m_W_to_nclus_criterion[k1];
                wWl = this.m_W_to_nclus_criterion[k2];

                nP = nNk;
                nQ = nNl;

                temp_shape[k1].AddIndicesPoligonosNoCluster(temp_shape[k2].IndicesPoligonosNoCluster);
                temp_shape[k2].ClearIndicesPoligonosNoCluster();

                //------------------- tira da lista de vizinhos caso o cluster já tenha excedido o tamanho máximo --------------//
                /*
                if (nclus >= 2*(int)Math.Ceiling((double)this.dados.GetLength(0) / (double)this.m_tamanho_maximo_used_cluster))
                {
                    if (nP + nQ > this.m_tamanho_maximo_used_cluster)
                    {
                        for (int i = 0; i < this.m_lista_clusters_atuais.GetLength(0); i++)
                        {
                            if (temp_shape[this.m_lista_clusters_atuais[i]].EstaNaListaVizinhos(k1)
                                && this.m_lista_clusters_atuais[i] != k1)
                            {
                                temp_shape[this.m_lista_clusters_atuais[i]].DeleteVizinho(k1);
                            }
                            if (temp_shape[k1].EstaNaListaVizinhos(this.m_lista_clusters_atuais[i])
                                && this.m_lista_clusters_atuais[i] != k1)
                            {
                                temp_shape[k1].DeleteVizinho(this.m_lista_clusters_atuais[i]);
                            }
                        }
                    }
                }
                */
                //----------------------------- redefine as distâncias entre clusters ------------------------------------------//

                for (int j = 0; j < this.m_lista_clusters_atuais.GetLength(0); j++)
                {
                    if (this.m_lista_clusters_atuais[j] != k1)
                    {
                        indice_menor = Math.Min(k1, this.m_lista_clusters_atuais[j]);
                        indice_maior = Math.Max(k1, this.m_lista_clusters_atuais[j]);

                        dist_RP = this.m_distancias[indice_menor, indice_maior];
                        dist_PQ = this.m_distancias[k1, k2];

                        indice_menor = Math.Min(k2, this.m_lista_clusters_atuais[j]);
                        indice_maior = Math.Max(k2, this.m_lista_clusters_atuais[j]);

                        dist_RQ = this.m_distancias[indice_menor, indice_maior];
                        nR = temp_shape[this.m_lista_clusters_atuais[j]].IndicesPoligonosNoCluster.GetLength(0);

                        nova_distancia = this.UpdateDissimilaridade(dist_RP, dist_RQ, dist_PQ, nQ, nP, nR);

                        indice_menor = Math.Min(k1, this.m_lista_clusters_atuais[j]);
                        indice_maior = Math.Max(k1, this.m_lista_clusters_atuais[j]);

                        //----------------- penalizando clusters muito grandes ---------------------------//

                        if (nP + nQ > this.m_tamanho_maximo_used_cluster)
                        {
                            nova_distancia += (double)this.dados.GetLength(0) * this.m_maxima_distancia_matriz_inicial;
                        }

                        this.m_distancias[indice_menor, indice_maior] = nova_distancia;
                    }
                }

                //------------------------ variáveis para geração dos critérios de seleção do número de clusters ---------------//

                this.m_Bkl_to_nclus_criterion = -(this.m_W_to_nclus_criterion[k1] + this.m_W_to_nclus_criterion[k2]);
                this.m_Pg_to_nclus_criterion -= (this.m_W_to_nclus_criterion[k1] + this.m_W_to_nclus_criterion[k2]);
                this.m_W_to_nclus_criterion[k1] = this.AtualizaValorWtoNclusCriterion(k1);
                this.m_Pg_to_nclus_criterion += this.m_W_to_nclus_criterion[k1];
                this.m_Bkl_to_nclus_criterion += this.m_W_to_nclus_criterion[k1];

                //------------------------ alimenta a árvore de clusters e os critérios de seleção do número de clustes --------------------------------------//

                aux_indices = new double[this.m_indice_clusters_atuais.GetLength(0), 1];
                for (int i = 0; i < this.m_indice_clusters_atuais.GetLength(0); i++) aux_indices[i, 0] = (double)this.m_indice_clusters_atuais[i];

                if (nclus <= max_nclus_tree && nclus >= min_nclus_tree)
                {
                    if (nclus == max_nclus_tree)
                    {
                        this.m_cluster_tree = utl.ArrayDoubleClone(aux_indices);
                    }
                    else
                    {
                        this.m_cluster_tree = utl.Concateh(this.m_cluster_tree, aux_indices);
                    }

                    this.m_sequencia_nclusters[max_nclus_tree - nclus] = (double)nclus;
                    this.m_sequencia_nobs_novo_cluster[max_nclus_tree - nclus] = (double)(nNk + nNl);

                    this.m_sequencia_partial_R2[max_nclus_tree - nclus] = this.m_Bkl_to_nclus_criterion / this.m_T_to_nclus_criterion;
                    if (nclus > 1)
                    {
                        this.m_sequencia_R2[max_nclus_tree - nclus] = 1.0 - (this.m_Pg_to_nclus_criterion / this.m_T_to_nclus_criterion);
                        this.GeraCCC(ref ccc, ref expected_r2, nclus, this.m_sequencia_R2[max_nclus_tree - nclus]);
                        this.m_sequencia_CCC[max_nclus_tree - nclus] = ccc;
                        this.m_sequencia_expected_R2[max_nclus_tree - nclus] = expected_r2;

                        this.m_sequencia_pseudo_F[max_nclus_tree - nclus] =
                            ((this.m_T_to_nclus_criterion - this.m_Pg_to_nclus_criterion)
                            / ((double)nclus - 1.0)) / (this.m_Pg_to_nclus_criterion / ((double)this.nobs - (double)nclus));
                    }
                    else
                    {
                        this.m_sequencia_R2[max_nclus_tree - nclus] = 0.0;
                        this.m_sequencia_CCC[max_nclus_tree - nclus] = 0.0;
                        this.m_sequencia_expected_R2[max_nclus_tree - nclus] = 0.0;

                        this.m_sequencia_pseudo_F[max_nclus_tree - nclus] = double.NaN;
                    }
                    if (nNk + nNl > 2)
                    {
                        this.m_sequencia_pseudo_t[max_nclus_tree - nclus] =
                            this.m_Bkl_to_nclus_criterion / ((wWk + wWl) / ((double)nNk + (double)nNl - 2.0));
                    }
                    else
                    {
                        this.m_sequencia_pseudo_t[max_nclus_tree - nclus] = double.NaN;
                    }

                    this.CalculaMedidasVariabilidade(posicao_indice_intraclusters);
                    posicao_indice_intraclusters++;
                }
            }

            this.m_distancias.DisposeMatriz();

            prBar.Value = 0;
            prBar.Refresh();
            Application.DoEvents();
        }

        public void GeraArvoreContinuousHierarchicalClustering(ref ToolStripProgressBar prBar, ref ToolStripStatusLabel lblEvolucao)
        {
            clsUtilTools utl = new clsUtilTools();

            #region Cheque sobre os dados da clusterização
            if (this.dados == null || this.dados.GetLength(0) == 0 || this.dados.GetLength(1) == 0)
            {
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Binarios) this.dados = utl.ArrayDoubleClone(this.dados_binarios);
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Continuos) this.dados = utl.ArrayDoubleClone(this.dados_continuos);
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Categoricos) this.dados = utl.ArrayDoubleClone(this.dados_categoricos);
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Ordinais) this.dados = utl.ArrayDoubleClone(this.dados_ordinais);

                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Mistos)
                {
                    this.dados = utl.Concateh(utl.Concateh(this.dados_continuos, this.dados_binarios),
                                              utl.Concateh(this.dados_categoricos, this.dados_ordinais));
                }
            }
            else
            {
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Continuos && this.dados_continuos.GetLength(0) == 0) this.dados_continuos = utl.ArrayDoubleClone(this.dados);
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Binarios && this.dados_binarios.GetLength(0) == 0) this.dados_binarios = utl.ArrayDoubleClone(this.dados);
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Categoricos && this.dados_categoricos.GetLength(0) == 0) this.dados_categoricos = utl.ArrayDoubleClone(this.dados);
                if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Ordinais && this.dados_ordinais.GetLength(0) == 0) this.dados_ordinais = utl.ArrayDoubleClone(this.dados);
            }
            #endregion

            if (this.dados == null) throw new Exception("Matriz de dados não definida para clusterização espacial");
            if (this.m_estrutura_shape == null) throw new Exception("Estrutura do shape não definida para clusterização espacial");

            this.nobs = this.dados.GetLength(0);
            this.nvars = this.dados.GetLength(1);

            this.CalculaLimiteMaximoTamanhoClusters();

            //------------ preenche a matriz com todas as distâncias entre observações ------------------------------------------//

            lblEvolucao.Text = "Geração da matriz de distâncias entre observações iniciais";
            //lblEvolucao.Refresh();

            this.PreencheTodasDistanciasContinuas(ref prBar);

            lblEvolucao.Text = "Evolução da formação da árvore de clusters";
            //lblEvolucao.Refresh();

            int min_nclus_tree = this.m_num_min_clusters_arvore;
            int max_nclus_tree = this.m_num_max_clusters_arvore;

            temp_shape = this.m_estrutura_shape.ConvertToIpeaGEOShape();

            int nNk = 0;
            int nNl = 0;
            double wWk = 0.0;
            double wWl = 0.0;

            double nova_distancia = 0.0;
            double dist_RP = 0.0;
            double dist_RQ = 0.0;
            double dist_PQ = 0.0;
            int nP, nQ, nR;
            int indice_menor, indice_maior;

            //int nk1_temp = 0;
            //int nk2_temp = 0;

            //---------------------------------------------- Initializing cluster list ----------------------------------------//
            this.m_indice_clusters_atuais = new int[this.nobs];
            this.m_lista_clusters_atuais = new int[this.nobs];
            for (int i = 0; i < this.nobs; i++)
            {
                this.m_indice_clusters_atuais[i] = i;
                this.m_lista_clusters_atuais[i] = i;
                temp_shape[i].AddListaAllVizinhos(temp_shape[i].ListaIndicesVizinhos);
                temp_shape[i].IndiceCluster = i;
                temp_shape[i].AddIndicePoligonoNoCluster(i);
            }

            this.soma_Pg = 0.0;
            this.m_Pg_to_nclus_criterion = 0.0;
            this.CalculaValorTtoNclusCriterion();

            this.vetor_W = new double[this.nobs];
            this.m_W_to_nclus_criterion = new double[this.nobs];

            for (int i = 0; i < this.nobs; i++)
            {
                this.vetor_W[i] = 0.0;
                this.m_W_to_nclus_criterion[i] = 0.0;
            }

            this.m_sequencia_CCC = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_nclusters = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_partial_R2 = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_pseudo_F = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_pseudo_t = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_R2 = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_expected_R2 = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_sequencia_nobs_novo_cluster = new double[max_nclus_tree - min_nclus_tree + 1];

            this.m_intracluster_cov_corrected_L2 = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_intracluster_norma_L1 = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_intracluster_norma_L2 = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_intracluster_norma_Lp = new double[max_nclus_tree - min_nclus_tree + 1];
            this.m_intracluster_var_corrected_L2 = new double[max_nclus_tree - min_nclus_tree + 1];

            this.CalculaOrderedSj();
            double ccc = 0.0;
            double expected_r2 = 0.0;

            //----------------------------------------------- Clustering steps ----------------------------------------------------------//
            int nclus = this.nobs;
            int k1 = 0;
            int k2 = 0;
            double distance = 0.0;
            double min_distance = 0.0;

            prBar.Maximum = this.nobs;

            int[] aux_lista = new int[0];
            int indice = 0;

            int[] indices_vizinhos = new int[0];

            double[,] aux_indices;
            bool tem_vizinhos = false;

            int posicao_indice_intraclusters = 0;
            this.CalculaMedidaVariabilidadeTotal();

            for (int iter = 0; iter < this.nobs-1; iter++)
            {
                prBar.Increment(1);
                Application.DoEvents();

                //----------------------------------------- Checando se ainda há clusters vizinhos --------------------------------------//

                tem_vizinhos = false;
                for (int i = 0; i < this.m_lista_clusters_atuais.GetLength(0); i++)
                {
                    if (temp_shape[this.m_lista_clusters_atuais[i]].ListaIndicesVizinhos.GetLength(0) > 0)
                    {
                        tem_vizinhos = true;
                        break;
                    }
                }

                if (!tem_vizinhos)
                {
                    this.m_num_min_clusters_arvore = nclus;
                    this.m_numero_minimo_possivel_clusters = nclus;
                    
                    utl.GeraSubRows(ref this.m_sequencia_CCC, this.m_sequencia_CCC, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_nclusters, this.m_sequencia_nclusters, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_partial_R2, this.m_sequencia_partial_R2, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_pseudo_F, this.m_sequencia_pseudo_F, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_R2, this.m_sequencia_R2, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_pseudo_t, this.m_sequencia_pseudo_t, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_expected_R2, this.m_sequencia_expected_R2, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_sequencia_nobs_novo_cluster, this.m_sequencia_nobs_novo_cluster, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);

                    utl.GeraSubRows(ref this.m_intracluster_cov_corrected_L2, this.m_intracluster_cov_corrected_L2, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_intracluster_norma_L1, this.m_intracluster_norma_L1, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_intracluster_norma_L2, this.m_intracluster_norma_L2, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_intracluster_norma_Lp, this.m_intracluster_norma_Lp, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);
                    utl.GeraSubRows(ref this.m_intracluster_var_corrected_L2, this.m_intracluster_var_corrected_L2, 0, this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore);

                    break;
                }

                //-----------------------------------------------------------------------------------------------------------------------//

                if (nclus <= this.m_num_min_clusters_arvore) break;

                k1 = this.m_lista_clusters_atuais[0];
                k2 = this.m_lista_clusters_atuais[1];
                distance = 0.0;

                min_distance = this.DistanceClusters(k1, k2);

                //min_distance = 1.0e200;
                if (k1 < k2) { min_distance = this.m_distancias[k1, k2]; }
                if (k2 < k1) { min_distance = this.m_distancias[k2, k1]; }

                //nk1_temp = temp_shape[k1].IndicesPoligonosNoCluster.GetLength(0);
                //nk2_temp = temp_shape[k2].IndicesPoligonosNoCluster.GetLength(0);

                //if ((nk1_temp + nk2_temp) > this.m_tamanho_maximo_used_cluster)
                //{
                //    min_distance += 1.0e200;
                //}

                for (int i = 0; i < nclus; i++)
                {
                    indices_vizinhos = temp_shape[this.m_lista_clusters_atuais[i]].ListaIndicesVizinhos;
                    

                    for (int j = 0; j < indices_vizinhos.GetLength(0); j++)
                    {
                        if (this.m_lista_clusters_atuais[i] < indices_vizinhos[j])
                        {
                            distance = this.m_distancias[this.m_lista_clusters_atuais[i], indices_vizinhos[j]];

                            if (distance < min_distance 
                                || ((distance <= min_distance)
                                && (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Binarios)))
                            {
                                //------------------------ controlando para o tamanho dos clusters ----------------------//
                                //nk1_temp = temp_shape[k1].IndicesPoligonosNoCluster.GetLength(0);
                                //nk2_temp = temp_shape[k2].IndicesPoligonosNoCluster.GetLength(0);

                                //if (nk1_temp + nk2_temp <= this.m_tamanho_maximo_used_cluster ||
                                //    nclus <= 2*(int)Math.Ceiling((double)this.dados.GetLength(0) / (double)this.m_tamanho_maximo_used_cluster))
                                //{
                                    k1 = this.m_lista_clusters_atuais[i];
                                    k2 = indices_vizinhos[j];
                                    min_distance = distance;
                                //}
                            }
                        }
                    }
                }

                //------------------ junta clusters com menor distância -------------------//

                nclus--;
                for (int i = 0; i < this.nobs; i++)
                {
                    if (this.m_indice_clusters_atuais[i] == k2) this.m_indice_clusters_atuais[i] = k1;
                }

                indice = 0;
                aux_lista = new int[nclus];
                for (int i = 0; i <= nclus; i++)
                {
                    if (this.m_lista_clusters_atuais[i] != k2)
                    {
                        aux_lista[indice++] = this.m_lista_clusters_atuais[i];
                    }
                }
                this.m_lista_clusters_atuais = aux_lista;

                //----------------------- redefinição de vizinhanças dos clusters ---------------------------//

                temp_shape[k1].AddListaVizinhos(temp_shape[k2].ListaIndicesVizinhos);

                for (int i = 0; i < this.m_lista_clusters_atuais.GetLength(0); i++)
                {
                    if (temp_shape[this.m_lista_clusters_atuais[i]].EstaNaListaVizinhos(k2))
                    {
                        temp_shape[this.m_lista_clusters_atuais[i]].AddVizinho(k1);
                        temp_shape[this.m_lista_clusters_atuais[i]].DeleteVizinho(k2);
                    }
                }

                //------------------------- redefinição lista de polígonos no cluster -------------------------//

                nNk = temp_shape[k1].IndicesPoligonosNoCluster.GetLength(0);
                nNl = temp_shape[k2].IndicesPoligonosNoCluster.GetLength(0);
                wWk = this.m_W_to_nclus_criterion[k1];
                wWl = this.m_W_to_nclus_criterion[k2];

                nP = nNk;
                nQ = nNl;

                temp_shape[k1].AddIndicesPoligonosNoCluster(temp_shape[k2].IndicesPoligonosNoCluster);
                temp_shape[k2].ClearIndicesPoligonosNoCluster();

                //------------------- tira da lista de vizinhos caso o cluster já tenha excedido o tamanho máximo --------------//
                /*
                if (nclus >= 2*(int)Math.Ceiling((double)this.dados.GetLength(0) / (double)this.m_tamanho_maximo_used_cluster))
                {
                    if (nP + nQ > this.m_tamanho_maximo_used_cluster)
                    {
                        for (int i = 0; i < this.m_lista_clusters_atuais.GetLength(0); i++)
                        {
                            if (temp_shape[this.m_lista_clusters_atuais[i]].EstaNaListaVizinhos(k1)
                                && this.m_lista_clusters_atuais[i] != k1)
                            {
                                temp_shape[this.m_lista_clusters_atuais[i]].DeleteVizinho(k1);
                            }
                            if (temp_shape[k1].EstaNaListaVizinhos(this.m_lista_clusters_atuais[i])
                                && this.m_lista_clusters_atuais[i] != k1)
                            {
                                temp_shape[k1].DeleteVizinho(this.m_lista_clusters_atuais[i]);
                            }
                        }
                    }
                }
                */
                //----------------------------- redefine as distâncias entre clusters ------------------------------------------//

                for (int j = 0; j < this.m_lista_clusters_atuais.GetLength(0); j++)
                {
                    if (this.m_lista_clusters_atuais[j] != k1)
                    {
                        indice_menor = Math.Min(k1, this.m_lista_clusters_atuais[j]);
                        indice_maior = Math.Max(k1, this.m_lista_clusters_atuais[j]);

                        dist_RP = this.m_distancias[indice_menor, indice_maior];
                        dist_PQ = this.m_distancias[k1, k2];

                        indice_menor = Math.Min(k2, this.m_lista_clusters_atuais[j]);
                        indice_maior = Math.Max(k2, this.m_lista_clusters_atuais[j]);

                        dist_RQ = this.m_distancias[indice_menor, indice_maior];
                        nR = temp_shape[this.m_lista_clusters_atuais[j]].IndicesPoligonosNoCluster.GetLength(0);

                        nova_distancia = this.UpdateDissimilaridade(dist_RP, dist_RQ, dist_PQ, nQ, nP, nR);

                        indice_menor = Math.Min(k1, this.m_lista_clusters_atuais[j]);
                        indice_maior = Math.Max(k1, this.m_lista_clusters_atuais[j]);

                        //----------------- penalizando clusters muito grandes ---------------------------//

                        if (nP + nQ > this.m_tamanho_maximo_used_cluster)
                        {
                            nova_distancia += (double)this.dados.GetLength(0) * this.m_maxima_distancia_matriz_inicial;
                        }

                        this.m_distancias[indice_menor, indice_maior] = nova_distancia;
                    }
                }

                //------------------------ variáveis para geração dos critérios de seleção do número de clusters ---------------//

                this.m_Bkl_to_nclus_criterion = - (this.m_W_to_nclus_criterion[k1] + this.m_W_to_nclus_criterion[k2]);
                this.m_Pg_to_nclus_criterion -= (this.m_W_to_nclus_criterion[k1] + this.m_W_to_nclus_criterion[k2]);
                this.m_W_to_nclus_criterion[k1] = this.AtualizaValorWtoNclusCriterion(k1);
                this.m_Pg_to_nclus_criterion += this.m_W_to_nclus_criterion[k1];
                this.m_Bkl_to_nclus_criterion += this.m_W_to_nclus_criterion[k1];
    
                //------------------------ alimenta a árvore de clusters e os critérios de seleção do número de clustes --------------------------------------//

                aux_indices = new double[this.m_indice_clusters_atuais.GetLength(0), 1];
                for (int i = 0; i < this.m_indice_clusters_atuais.GetLength(0); i++) aux_indices[i, 0] = (double)this.m_indice_clusters_atuais[i];

                if (nclus <= max_nclus_tree && nclus >= min_nclus_tree)
                {
                    if (nclus == max_nclus_tree)
                    {
                        this.m_cluster_tree = utl.ArrayDoubleClone(aux_indices);
                    }
                    else
                    {
                        this.m_cluster_tree = utl.Concateh(this.m_cluster_tree, aux_indices);
                    }

                    this.m_sequencia_nclusters[max_nclus_tree - nclus] = (double)nclus;
                    this.m_sequencia_nobs_novo_cluster[max_nclus_tree - nclus] = (double)(nNk + nNl);

                    this.m_sequencia_partial_R2[max_nclus_tree - nclus] = this.m_Bkl_to_nclus_criterion / this.m_T_to_nclus_criterion;
                    if (nclus > 1)
                    {
                        this.m_sequencia_R2[max_nclus_tree - nclus] = 1.0 - (this.m_Pg_to_nclus_criterion / this.m_T_to_nclus_criterion);
                        this.GeraCCC(ref ccc, ref expected_r2, nclus, this.m_sequencia_R2[max_nclus_tree - nclus]);
                        this.m_sequencia_CCC[max_nclus_tree - nclus] = ccc;
                        this.m_sequencia_expected_R2[max_nclus_tree - nclus] = expected_r2;

                        this.m_sequencia_pseudo_F[max_nclus_tree - nclus] =
                            ((this.m_T_to_nclus_criterion - this.m_Pg_to_nclus_criterion) 
                            / ((double)nclus - 1.0)) / (this.m_Pg_to_nclus_criterion / ((double)this.nobs - (double)nclus));
                    }
                    else
                    {
                        this.m_sequencia_R2[max_nclus_tree - nclus] = 0.0;
                        this.m_sequencia_CCC[max_nclus_tree - nclus] = 0.0;
                        this.m_sequencia_expected_R2[max_nclus_tree - nclus] = 0.0;
 
                        this.m_sequencia_pseudo_F[max_nclus_tree - nclus] = double.NaN;
                    }
                    if (nNk + nNl > 2)
                    {
                        this.m_sequencia_pseudo_t[max_nclus_tree - nclus] =
                            this.m_Bkl_to_nclus_criterion / ((wWk + wWl) / ((double)nNk + (double)nNl - 2.0));
                    }
                    else
                    {
                        this.m_sequencia_pseudo_t[max_nclus_tree - nclus] = double.NaN;
                    }

                    this.CalculaMedidasVariabilidade(posicao_indice_intraclusters);
                    posicao_indice_intraclusters++;
                }
            }

            this.m_distancias.DisposeMatriz();
            this.m_distancias = null;

            prBar.Value = 0;
            //prBar.Refresh();
            Application.DoEvents();
        }

        #endregion 

        #region Sequencia estatísticas dos clusters da árvore

        public double[,] GeraSequenciaEstatisticas(double[,] variaveis_to_medias)
        {
            clsUtilTools clt = new clsUtilTools();

            int nsequencia_clusters = this.m_num_max_clusters_arvore - this.m_num_min_clusters_arvore + 1;

            double[,] max_diff_percentual = new double[nsequencia_clusters, 1];
            double[,] min_diff_percentual = new double[nsequencia_clusters, 1];
            double[,] media_diff_percentual = new double[nsequencia_clusters, 1];
            
            double[,] aux_medias = new double[0, 0];
            double[,] aux_medias_0 = new double[0, 0];

            double aux_diff_media = 0.0;
            double aux_diff_max = 0.0;
            double aux_diff_min = 0.0;
            double[,] n_clusters = new double[nsequencia_clusters, 1];

            int minimo_indice = 1;
            n_clusters[nsequencia_clusters - 1, 0] = 1.0;

            if (this.m_num_min_clusters_arvore > 1)
                minimo_indice = 0;

            for (int k = nsequencia_clusters - 1; k >= minimo_indice; k--)
            {
                n_clusters[nsequencia_clusters - 1 - k, 0] = k + this.m_num_min_clusters_arvore;

                max_diff_percentual[nsequencia_clusters - 1 - k, 0] = 1.0e+200;
                min_diff_percentual[nsequencia_clusters - 1 - k, 0] = 1.0e+200;
                media_diff_percentual[nsequencia_clusters - 1 - k, 0] = 1.0e+200;

                aux_medias_0 = this.GerarEstatisticasClusters(variaveis_to_medias, k + this.m_num_min_clusters_arvore);
                aux_medias = new double[aux_medias_0.GetLength(0), aux_medias_0.GetLength(1) - 2];
                for (int i = 0; i < aux_medias_0.GetLength(0); i++)
                    for (int j = 0; j < aux_medias.GetLength(1); j++)
                        aux_medias[i, j] = aux_medias_0[i, j + 2];

                for (int i = 0; i < aux_medias.GetLength(0) - 1; i++)
                {
                    for (int j = i + 1; j < aux_medias.GetLength(0); j++)
                    {
                        aux_diff_media = this.MediaDiferencaPercentualEntreMediasColunas(clt.SubRowArrayDouble(aux_medias, i), clt.SubRowArrayDouble(aux_medias, j));
                        aux_diff_max = this.MaximoDiferencaPercentualEntreMediasColunas(clt.SubRowArrayDouble(aux_medias, i), clt.SubRowArrayDouble(aux_medias, j));
                        aux_diff_min = this.MinimoDiferencaPercentualEntreMediasColunas(clt.SubRowArrayDouble(aux_medias, i), clt.SubRowArrayDouble(aux_medias, j));

                        if (aux_diff_max < max_diff_percentual[nsequencia_clusters - 1 - k, 0]) max_diff_percentual[nsequencia_clusters - 1 - k, 0] = aux_diff_max;
                        if (aux_diff_min < min_diff_percentual[nsequencia_clusters - 1 - k, 0]) min_diff_percentual[nsequencia_clusters - 1 - k, 0] = aux_diff_min;
                        if (aux_diff_media < media_diff_percentual[nsequencia_clusters - 1 - k, 0]) media_diff_percentual[nsequencia_clusters - 1 - k, 0] = aux_diff_media;
                    }
                }
            }

            double[,] res = clt.Concateh(n_clusters, media_diff_percentual);
            res = clt.Concateh(res, max_diff_percentual);
            res = clt.Concateh(res, min_diff_percentual);

            return res;
        }

        private double MediaDiferencaPercentualEntreMediasColunas(double[,] v1, double[,] v2)
        {
            int n = v1.GetLength(1);
            double[] diffs = new double[n];
            double denominador = 0.0;

            for (int i = 0; i < n; i++)
            {
                denominador = Math.Max(Math.Abs(v1[0, i]), Math.Abs(v2[0, i]));
                denominador = Math.Max(1.0e-6, denominador);
                diffs[i] = Math.Abs(v1[0, i] - v2[0, i]) / denominador;
            }

            double media_distance = 0.0;
            for (int i = 0; i < n; i++)
            {
                media_distance += diffs[i];
            }

            return 100.0 * media_distance / (double)n;
        }

        private double MinimoDiferencaPercentualEntreMediasColunas(double[,] v1, double[,] v2)
        {
            int n = v1.GetLength(1);
            double[] diffs = new double[n];
            double denominador = 0.0;

            for (int i = 0; i < n; i++)
            {
                denominador = Math.Max(Math.Abs(v1[0, i]), Math.Abs(v2[0, i]));
                denominador = Math.Max(1.0e-6, denominador);
                diffs[i] = Math.Abs(v1[0, i] - v2[0, i]) / denominador;
            }

            double min_distance = diffs[0];
            for (int i = 1; i < n; i++)
            {
                if (min_distance > diffs[i]) min_distance = diffs[i];
            }

            return 100.0 * min_distance;
        }

        private double MaximoDiferencaPercentualEntreMediasColunas(double[,] v1, double[,] v2)
        {
            int n = v1.GetLength(1);
            double[] diffs = new double[n];
            double denominador = 0.0;

            for (int i=0; i<n; i++)
            {
                denominador = Math.Max(Math.Abs(v1[0, i]), Math.Abs(v2[0, i]));
                denominador = Math.Max(1.0e-6, denominador);
                diffs[i] = Math.Abs(v1[0, i] - v2[0, i]) / denominador;                
            }

            double max_distance = diffs[0];
            for (int i = 1; i < n; i++)
            {
                if (max_distance < diffs[i]) max_distance = diffs[i];
            }

            return 100.0 * max_distance;
        }

        public double[,] GerarEstatisticasClusters(double[,] variaveis_to_medias, int numero_clusters)
        {
            if (this.m_cluster_tree == null) throw new Exception("Árvore de clusters não construída");
            if (numero_clusters > this.m_num_max_clusters_arvore || numero_clusters < this.m_num_min_clusters_arvore)
                throw new Exception("Número de clusters inválido para a árvore de clusters construída");

            int coluna = this.m_num_max_clusters_arvore - numero_clusters;

            clsUtilTools utl = new clsUtilTools();

            double[,] aux_indices = utl.SubColumnArrayDouble(this.m_cluster_tree, coluna);
            int nclus = numero_clusters;

            //--------------------------------------------- Clustering statistics -----------------------------------------//

            double[,] medias_clusters = new double[nclus, variaveis_to_medias.GetLength(1)];
            double[,] ftable = new double[0, 0];

            utl.FrequencyTable(ref ftable, aux_indices);

            for (int i = 0; i < this.nobs; i++)
            {
                for (int k = 0; k < nclus; k++)
                {
                    if (aux_indices[i, 0] == ftable[k, 0])
                    {
                        for (int j = 0; j < variaveis_to_medias.GetLength(1); j++)
                        {
                            medias_clusters[k, j] += variaveis_to_medias[i, j];
                        }
                    }
                }
            }

            for (int k = 0; k < nclus; k++)
            {
                for (int j = 0; j < variaveis_to_medias.GetLength(1); j++)
                {
                    medias_clusters[k, j] = medias_clusters[k, j] / ftable[k, 1];
                }
            }

            return utl.Concateh(ftable, medias_clusters);
        }

        #endregion 

        #region Distâncias contínuas entre polígonos

        private double[] m_variancias_variaveis = new double[0];
        private void ConstroiVarianciasVariaveis()
        {
            clsUtilTools clt = new clsUtilTools();
            this.m_variancias_variaveis = new double[this.dados.GetLength(1)];

            if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Continuos
                || (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Mistos && this.dados_continuos.GetLength(0) > 0))
            {
                for (int j = 0; j < this.dados.GetLength(1); j++)
                    this.m_variancias_variaveis[j] = clt.VarianciaColumnMatrix(clt.SubColumnArrayDouble(this.dados_continuos, j));
            }
            else
            {
                for (int j = 0; j < this.dados.GetLength(1); j++)
                    this.m_variancias_variaveis[j] = 1.0;
            }
        }

        private double[,] m_matriz_inv_covariancia = new double[0, 0];
        private void ControiInvCovarianciasVariaveis()
        {
            clsUtilTools clt = new clsUtilTools();
            double[,] covm = new double[0,0];

            if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Continuos 
                || (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Mistos && this.dados_continuos.GetLength(0) > 0))
            {
                covm = clt.CovSampleMatrix(this.dados_continuos);

                try
                {
                    this.m_matriz_inv_covariancia = clt.MatrizInversa(covm);
                }
                catch
                {
                    this.m_matriz_inv_covariancia = new double[covm.GetLength(0), covm.GetLength(0)];
                    for (int i = 0; i < covm.GetLength(0); i++)
                    {
                        if (covm[i, i] > 0) this.m_matriz_inv_covariancia[i, i] = 1.0 / covm[i, i];
                    }
                }

                double[,] teste = clt.MatrizMult(this.m_matriz_inv_covariancia, covm);
            }
            else
            {
                this.m_matriz_inv_covariancia = new double[this.dados.GetLength(1), this.dados.GetLength(1)];
                for (int i = 0; i < this.m_matriz_inv_covariancia.GetLength(0); i++)
                {
                    for (int j = 0; j < this.m_matriz_inv_covariancia.GetLength(1); j++)
                    {
                        this.m_matriz_inv_covariancia[i, j] = 0.0;
                    }
                    this.m_matriz_inv_covariancia[i, i] = 1.0;
                }
            }
        }

        private double DistanciaContinuaPoligonos(double[,] v1, double[,] v2)
        {
            double r = 0.0;
            switch (this.m_tipo_distancia_continua)
            {
                case TipoDistanciaContinua.Euclidiana:
                    for (int j = 0; j < v1.GetLength(1); j++)
                        r += Math.Pow((v1[0, j] - v2[0, j]), 2.0);

                    //return Math.Sqrt(r);
                    return r;

                case TipoDistanciaContinua.L1Norm:
                    for (int j = 0; j < v1.GetLength(1); j++)
                        r += Math.Abs(v1[0, j] - v2[0, j]);
                    return r;

                case TipoDistanciaContinua.LpNorm:
                    for (int j = 0; j < v1.GetLength(1); j++)
                        r += Math.Pow(Math.Abs((v1[0, j] - v2[0, j])), this.m_parameter_Lp_norm);

                    //return Math.Pow(r, 1.0 / this.m_parameter_Lp_norm);
                    return r;

                case TipoDistanciaContinua.VarianceCorrected:
                    for (int j = 0; j < v1.GetLength(1); j++)
                        r += Math.Pow((v1[0, j] - v2[0, j]), 2.0) / this.m_variancias_variaveis[j];

                    return r;
                    //return Math.Sqrt(r);

                case TipoDistanciaContinua.CovarianceCorrected:

                    clsUtilTools clt = new clsUtilTools();
                    double[,] d = new double[1, v1.GetLength(1)];
                    for (int j = 0; j < v1.GetLength(1); j++)
                        d[0, j] = v1[0, j] - v2[0, j];

                    double[,] aux = clt.MatrizMult(clt.MatrizMult(d, this.m_matriz_inv_covariancia), clt.MatrizTransp(d));

                    return aux[0,0];
                    //return Math.Sqrt(aux[0, 0]);

                default:
                    return 0.0;                
            }
        }

        #endregion

        #region Distâncias ordinais entre polígonos

        private double[] m_variancia_dados_ordinais = new double[0];
        private double[] m_scaleL1norm_dados_ordinais = new double[0];

        /// <summary>
        /// Função para gerar a distância entre vetores com variáveis ordinais. Assume-se que os valores 
        /// dos elementos do vetor assumem valores de 1 a K, onde K é o número máximo de categorias ordinais. 
        /// </summary>
        /// <param name="v1">Primeiro vetor de variáveis ordinais.</param>
        /// <param name="v2">Segundo vetor de variáveis ordinais.</param>
        /// <returns>Retorna um escalar, com a distância entre os vetores.</returns>
        private double DistanciaOrdinalPoligonos(double[,] v1, double[,] v2)
        {
            double distancia = 0.0;
            double nv = Convert.ToDouble(v1.GetLength(1));
            double a = 0.0;
            double[] medias = new double[0];

            switch (this.m_tipo_distancia_ordinal)
            {
                case TipoDistanciaOrdinal.Euclidiana:
                    for (int j = 0; j < v1.GetLength(1); j++)
                    {
                        a += Math.Pow((v1[0, j] - v2[0, j]), 2.0);
                    }
                    distancia = a;
                    break;
                case TipoDistanciaOrdinal.L1Norm:
                    for (int j = 0; j < v1.GetLength(1); j++)
                    {
                        a += Math.Abs(v1[0, j] - v2[0, j]);
                    }
                    distancia = a;
                    break;
                case TipoDistanciaOrdinal.L1NormRescaled:
                    if (!this.m_ja_calculou_escala_distancia_ordinal)
                    {
                        this.m_scaleL1norm_dados_ordinais = new double[v1.GetLength(1)];
                        medias = new double[v1.GetLength(1)];
                        for (int k = 0; k < v1.GetLength(1); k++)
                        {
                            for (int i = 0; i < this.dados_ordinais.GetLength(0); i++)
                            {
                                medias[k] += this.dados_ordinais[i, k];
                            }
                            medias[k] = medias[k] / (double)this.dados_ordinais.GetLength(0); 
                            for (int i = 0; i < this.dados_ordinais.GetLength(0); i++)
                            {
                                this.m_scaleL1norm_dados_ordinais[k] += Math.Abs(this.dados_ordinais[i, k] - medias[k]);
                            }
                            m_scaleL1norm_dados_ordinais[k] = m_scaleL1norm_dados_ordinais[k] / (double)this.dados_ordinais.GetLength(0); 
                        }
                    }
                    for (int j = 0; j < v1.GetLength(1); j++)
                    {
                        a += Math.Abs(v1[0, j] - v2[0, j]) / this.m_scaleL1norm_dados_ordinais[j];
                    }
                    distancia = a;
                    break;
                case TipoDistanciaOrdinal.VarianceCorrected:
                    if (!this.m_ja_calculou_escala_distancia_ordinal)
                    {
                        this.m_variancia_dados_ordinais = new double[v1.GetLength(1)];
                        medias = new double[v1.GetLength(1)];
                        for (int k = 0; k < v1.GetLength(1); k++)
                        {
                            for (int i = 0; i < this.dados_ordinais.GetLength(0); i++)
                            {
                                medias[k] += this.dados_ordinais[i, k];
                            }
                            medias[k] = medias[k] / (double)this.dados_ordinais.GetLength(0);
                            for (int i = 0; i < this.dados_ordinais.GetLength(0); i++)
                            {
                                this.m_variancia_dados_ordinais[k] += Math.Pow(this.dados_ordinais[i, k] - medias[k], 2.0);
                            }
                            m_variancia_dados_ordinais[k] = m_variancia_dados_ordinais[k] / (double)this.dados_ordinais.GetLength(0);
                        }
                    }
                    for (int j = 0; j < v1.GetLength(1); j++)
                    {
                        a += Math.Pow(v1[0, j] - v2[0, j], 2.0) / this.m_variancia_dados_ordinais[j];
                    }
                    distancia = a;
                    break;
                default:
                    distancia = 0.0;
                    break;
            }
            return distancia;
        }

        #endregion

        #region Distância mista entre polígonos

        private double DistanciaMistaPoligonos(double[,] vcont1, double[,] vcont2,
                                                double[,] vbin1, double[,] vbin2,
                                                double[,] vcat1, double[,] vcat2,
                                                double[,] vord1, double[,] vord2)
        {
            double dist_continua = 0.0;
            if (vcont1.GetLength(1) == vcont2.GetLength(1) && vcont1.GetLength(1) > 0) dist_continua = this.DistanciaContinuaPoligonos(vcont1, vcont2);

            double dist_binaria = 0.0;
            if (vbin1.GetLength(1) == vbin2.GetLength(1) && vbin1.GetLength(1) > 0) dist_binaria = this.DistanciaBinariaPoligonos(vbin1, vbin2);

            double dist_categorica = 0.0;
            if (vcat1.GetLength(1) == vcat2.GetLength(1) && vcat1.GetLength(1) > 0) dist_categorica = this.DistanciaCategoricaPoligonos(vcat1, vcat2);

            double dist_ordinal = 0.0;
            if (vord1.GetLength(1) == vord2.GetLength(1) && vord1.GetLength(1) > 0) dist_ordinal = this.DistanciaOrdinalPoligonos(vord1, vord2);

            return dist_continua + this.PesoDistanciaBinariaDadosMistos * dist_binaria
                                 + this.PesoDistanciaCategoricaDadosMistos * dist_categorica
                                 + this.PesoDistanciaOrdinalDadosMistos * dist_ordinal;
        }

        #endregion

        #region Distâncias categóricas entre polígonos

        private double DistanciaCategoricaPoligonos(double[,] v1, double[,] v2)
        {
            double a1 = 0.0;
            double distancia = 0.0;
            double nv = (double)v1.GetLength(1);

            switch (this.m_tipo_distancia_categorica)
            {
                case TipoDistanciaCategorica.SimpleMatching:
                    for (int i = 0; i < v1.GetLength(1); i++)
                    {
                        if (Convert.ToInt32(Math.Round(v1[0, i])) == Convert.ToInt32(Math.Round(v2[0, i]))) a1++;
                    }
                    //distancia = 1.0 - (a1 / (double)this.nvars);
                    distancia = 1.0 - (a1 / nv);
                    break;
                default:
                    distancia = 0.0;
                    break;
            }
            return distancia;
        }

        #endregion
        
        #region Distâncias binárias entre polígonos

        private double DistanciaBinariaPoligonos(double[,] v1, double[,] v2)
        {
            double a1 = 0.0, a2 = 0.0, a3 = 0.0, a4 = 0.0;
            double distancia = 0.0;
            double r = 0.0;
            double nv = (double)v1.GetLength(1);

            switch (this.m_tipo_distancia_binaria)
            {
                case TipoDistanciaBinaria.Jaccard:
                    for (int i = 0; i < v1.GetLength(1); i++)
                    {
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a1++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a2++;
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] <=this.m_corte_variavel_binaria) a3++;
                    }
                    if (a1 + a2 + a3 > 0)
                    {
                        distancia = (a2 + a3) / (a1 + a2 + a3);
                    }
                    else
                    {
                        distancia = 0.0;
                    }
                    break;
                    //return a1 / (a1 + a2 + a3);
                case TipoDistanciaBinaria.Tanimoto:
                    for (int i = 0; i < v1.GetLength(1); i++)
                    {
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a1++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a2++;
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a3++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a4++;
                    }
                    distancia = (2.0 * (a2 + a3)) / (a1 + 2.0 * (a2 + a3) + a4);
                    break;
                    //return (a1 + a4) / (a1 + 2.0 * (a2 + a3) + a4);
                case TipoDistanciaBinaria.SimpleMatching:
                    for (int i = 0; i < v1.GetLength(1); i++)
                    {
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a1++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a4++;
                    }
                    //distancia = 1.0 - ((a1 + a4) / (double)this.nvars);
                    distancia = 1.0 - ((a1 + a4) / nv);
                    break;
                    //return (a1 + a4) / (double)this.nvars;
                case TipoDistanciaBinaria.RusselRao:
                    for (int i = 0; i < v1.GetLength(1); i++)
                    {
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a1++;
                    }
                    //distancia = 1.0 - (a1 / (double)this.nvars);
                    distancia = 1.0 - (a1 / nv);
                    break;
                    //return a1 / (double)this.nvars;
                case TipoDistanciaBinaria.Dice:
                    for (int i = 0; i < v1.GetLength(1); i++)
                    {
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a1++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a2++;
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a3++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a4++;
                    }
                    if (a1 + a2 + a3 > 0)
                    {
                        distancia = (a2 + a3) / (2.0 * a1 + a2 + a3);
                    }
                    else
                    {
                        distancia = 0.0;
                    }
                    break;
                    //return 2.0 * a1 / (2.0 * a1 + a2 + a3);
                case TipoDistanciaBinaria.Kulczynski:
                    for (int i = 0; i < v1.GetLength(1); i++)
                    {
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a1++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a2++;
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a3++;
                    }
                    if (a1 + a2 > 0 && a1 + a3 > 0)
                    {
                        distancia = 1.0 - 0.5 * ((a1 / (a1 + a2)) + (a1 / (a1 + a3)));
                    }
                    else 
                    {
                        for (int i = 0; i < v1.GetLength(1); i++)
                        {
                            if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a4++;
                        }
                        if (Convert.ToInt32(a4) == v1.GetLength(1))
                        {
                            distancia = 0.0;
                        }
                        else distancia = 1.0;
                    }
                    break;
                    //return a1 / (a2 + a3);

                default:
                    //return 0.0;
                    distancia = 0.0;
                    break;
            }
            return distancia;
        }

        #endregion 

        #region Métodos de clusterização

        private double soma_Pg = 0.0;
       
        private double DistanceClusters(int k1, int k2)
        {
            clsUtilTools utl = new clsUtilTools();

            #region Esquema novo de montagem das bases dos clusters
            
            int n1 = temp_shape[k1].IndicesPoligonosNoCluster.GetLength(0);
            int n2 = temp_shape[k2].IndicesPoligonosNoCluster.GetLength(0);

            double[,] m1 = new double[n1, this.nvars];
            double[,] m2 = new double[n2, this.nvars];

            for (int i = 0; i < m1.GetLength(0); i++)
            {
                for (int j = 0; j < this.nvars; j++)
                {
                    m1[i, j] = this.dados[temp_shape[k1].IndicesPoligonosNoCluster[i], j];
                }
            }

            for (int i = 0; i < m2.GetLength(0); i++)
            {
                for (int j = 0; j < this.nvars; j++)
                {
                    m2[i, j] = this.dados[temp_shape[k2].IndicesPoligonosNoCluster[i], j];
                }
            }
            
            #endregion 

            #region Esquema antigo de montagem das bases dos clusters
            
            //int n1 = 0;
            //int n2 = 0;
            //for (int i = 0; i < this.nobs; i++)
            //{
            //    if (this.m_indice_clusters_atuais[i] == k1) n1++;
            //    if (this.m_indice_clusters_atuais[i] == k2) n2++;
            //}

            //double[,] m1 = new double[n1, this.nvars];
            //double[,] m2 = new double[n2, this.nvars];

            //int ind1 = 0;
            //int ind2 = 0;

            //for (int i = 0; i < this.nobs; i++)
            //{
            //    if (this.m_indice_clusters_atuais[i] == k1)
            //    {
            //        for (int j = 0; j < this.nvars; j++)
            //        {
            //            m1[ind1, j] = this.dados[i, j];
            //        }
            //        ind1++;
            //    }
            //    if (ind1 == n1) break;
            //}

            //for (int i = 0; i < this.nobs; i++)
            //{            
            //    if (this.m_indice_clusters_atuais[i] == k2)
            //    {
            //        for (int j = 0; j < this.nvars; j++)
            //        {
            //            m2[ind2, j] = this.dados[i, j];
            //        }
            //        ind2++;
            //    }
            //    if (ind2 == n2) break;
            //}
            
            #endregion 

            switch (this.m_tipo_metodo_clusterizacao)
            {
                case  MetodoClusterizacao.Ward:
                    {
                        //----------------------------- Ward's minimum-variance method ----------------------------//
                        return this.m_funcao_distancia(utl.Meanc(m1), utl.Meanc(m2)) /
                            ((1.0 / (double)m1.GetLength(0)) + (1.0 / (double)m2.GetLength(0)));
                    }
                case MetodoClusterizacao.AverageLinkage:
                    {
                        //----------------------------- Average linkage method -----------------------------------//
                        double[,] v1 = utl.Meanc(m1);
                        double[,] v2 = utl.Meanc(m2);
                        return this.m_funcao_distancia(v1, v2) + ((vetor_W[k1] / (double)m1.GetLength(0)) + (vetor_W[k2] / (double)m2.GetLength(0)));
                    }
                case MetodoClusterizacao.Centroid:
                    {
                        //----------------------------- Centroid method -------------------------------------------//
                        double[,] v1 = utl.Meanc(m1);
                        double[,] v2 = utl.Meanc(m2);
                        return this.m_funcao_distancia(v1, v2);
                    }
                case MetodoClusterizacao.CompleteLinkage:
                    {
                        //----------------------------- Complete linkage method -----------------------------------//
                        double max_dist = this.m_funcao_distancia(utl.SubRowArrayDouble(m1, 0), utl.SubRowArrayDouble(m2, 0));
                        double dist = 0.0;
                        for (int i = 0; i < m1.GetLength(0); i++)
                        {
                            for (int j = 0; j < m2.GetLength(0); j++)
                            {
                                dist = this.m_funcao_distancia(utl.SubRowArrayDouble(m1, i), utl.SubRowArrayDouble(m2, j));
                                if (max_dist < dist) max_dist = dist;
                            }
                        }
                        return max_dist;
                    }
                case MetodoClusterizacao.EML:
                    {
                        //--------------------------------------- EML method --------------------------------------//
                        n1 = m1.GetLength(0);
                        n2 = m2.GetLength(0);
                        int nm = n1 + n2;

                        double dist = -this.m_parameter_p_EML_method * ((double)nm * Math.Log((double)nm)
                            - (double)n1 * Math.Log((double)n1) - (double)n2 * Math.Log((double)n2));

                        double[,] mm = utl.Concatev(m1, m2);
                        double Wm = 0.0;
                        double[,] vm = utl.Meanc(mm);
                        for (int i = 0; i < mm.GetLength(0); i++) { Wm += this.m_funcao_distancia(utl.SubRowArrayDouble(mm, i), vm); }

                        double Bkl = Wm - vetor_W[k1] - vetor_W[k2];

                        dist += (double)this.nobs * (double)this.nvars * Math.Log(1.0 + (Bkl / soma_Pg));

                        return dist;
                    }
                case MetodoClusterizacao.SingleLinkage:
                    {
                        //------------------------------- Single linkage method -----------------------------------//
                        double min_dist = this.m_funcao_distancia(utl.SubRowArrayDouble(m1, 0), utl.SubRowArrayDouble(m2, 0));
                        double dist = 0.0;
                        for (int i = 0; i < m1.GetLength(0); i++)
                        {
                            for (int j = 0; j < m2.GetLength(0); j++)
                            {
                                dist = this.m_funcao_distancia(utl.SubRowArrayDouble(m1, i), utl.SubRowArrayDouble(m2, j));
                                if (min_dist > dist) min_dist = dist;
                            }
                        }
                        return min_dist;
                    }
                default:
                    return 0.0;
            }
        }
      
        #region Algoritmo antigo
		
        /*
        public int NQContinuousHierarchicalClustering(ref double[,] cluster_database,
            ref double[,] cluster_statistics,
            ref double[,] cluster_tree,
            double[,] database_in, int distance_used, int num_clusters, int min_nclus_tree, int max_nclus_tree, clsIpeaShape estrutura_shape,
            ref ProgressBar prBar)
        {
            clsIpeaShape temp_shape = estrutura_shape.Clone();

            clsUtilTools utl = new clsUtilTools();
            
            this.nobs = database_in.GetLength(0);
            this.nvars = database_in.GetLength(1);

            this.dados = utl.ArrayDoubleClone(database_in);

            //---------------------------------------------- Initializing cluster list ----------------------------------------//
            this.m_indice_clusters_atuais = new int[this.nobs];
            this.m_lista_clusters_atuais = new int[this.nobs];
            for (int i = 0; i < this.nobs; i++)
            {
                this.m_indice_clusters_atuais[i] = i;
                this.m_lista_clusters_atuais[i] = i;
                temp_shape[i].AddListaAllVizinhos(temp_shape[i].ListaIndicesVizinhos);
                temp_shape[i].IndiceCluster = i;
            }
           
            //----------------------------------------------- Clustering steps ------------------------------------------------//
            int nclus = this.nobs;
            int k1 = 0;
            int k2 = 0;
            double distance = 0.0;
            double min_distance = 0.0;

            prBar.Maximum = this.nobs;

            int[] aux_lista = new int[0];
            int indice = 0;

            int[] indices_vizinhos = new int[0];
            
            double[,] aux_indices;

            for (int iter = 0; iter < this.nobs; iter++)
            {
                prBar.Increment(1);
                Application.DoEvents();
                
                if (nclus <= num_clusters) break;
                k1 = this.m_lista_clusters_atuais[0];
                k2 = this.m_lista_clusters_atuais[1];
                distance = 0.0;
                min_distance = this.DistanceClusters(k1, k2, distance_used);
                for (int i = 0; i < nclus; i++)
                {
                    indices_vizinhos = temp_shape[this.m_lista_clusters_atuais[i]].ListaIndicesVizinhos;

                    for (int j = 0; j < indices_vizinhos.GetLength(0); j++)
                    {
                        if (this.m_lista_clusters_atuais[i] < indices_vizinhos[j])
                        {
                            distance = this.DistanceClusters(this.m_lista_clusters_atuais[i], indices_vizinhos[j], distance_used);
                            if (distance < min_distance)
                            {
                                k1 = this.m_lista_clusters_atuais[i];
                                k2 = indices_vizinhos[j];
                                min_distance = distance;
                            }
                        }
                    }
                }
                
                //------------------ junta clusters com menor distância -------------------//

                nclus--;
                for (int i = 0; i < this.nobs; i++)
                {
                    if (this.m_indice_clusters_atuais[i] == k2) this.m_indice_clusters_atuais[i] = k1;
                }

                indice = 0;
                aux_lista = new int[nclus];
                for (int i = 0; i <= nclus; i++)
                {
                    if (this.m_lista_clusters_atuais[i] != k2)
                    {
                        aux_lista[indice++] = this.m_lista_clusters_atuais[i];
                    }
                }
                this.m_lista_clusters_atuais = aux_lista;

                //----------------------- redefinição de vizinhanças dos clusters ---------------------------//

                temp_shape[k1].AddListaVizinhos(temp_shape[k2].ListaIndicesVizinhos);

                for (int i = 0; i < this.m_lista_clusters_atuais.GetLength(0); i++)
                {
                    if (temp_shape[this.m_lista_clusters_atuais[i]].EstaNaListaVizinhos(k2))
                    {
                        temp_shape[this.m_lista_clusters_atuais[i]].AddVizinho(k1);
                        temp_shape[this.m_lista_clusters_atuais[i]].DeleteVizinho(k2);
                    }                    
                }

                aux_indices = new double[this.m_indice_clusters_atuais.GetLength(0), 1];
                for (int i = 0; i < this.m_indice_clusters_atuais.GetLength(0); i++) aux_indices[i,0] = (double)this.m_indice_clusters_atuais[i];

                if (nclus <= max_nclus_tree && nclus >= min_nclus_tree)
                {
                    if (nclus == max_nclus_tree) cluster_tree = utl.ArrayDoubleClone(aux_indices);
                    else cluster_tree = utl.Concateh(cluster_tree, aux_indices);
                }
            }

            nclus = this.m_lista_clusters_atuais.GetLength(0);

            //--------------------------------------------- Clustering results --------------------------------------------//

            aux_indices = new double[this.m_indice_clusters_atuais.GetLength(0), 1];
            for (int i = 0; i < this.m_indice_clusters_atuais.GetLength(0); i++) aux_indices[i, 0] = (double)this.m_indice_clusters_atuais[i];

            cluster_database = utl.Concateh(this.dados, aux_indices);
             
            //--------------------------------------------- Clustering statistics -----------------------------------------//

            double[,] medias_clusters = new double[nclus, this.nvars];
            double[,] ftable = new double[0,0];
            aux_indices = new double[this.m_indice_clusters_atuais.GetLength(0), 1];
            for (int i = 0; i < this.m_indice_clusters_atuais.GetLength(0); i++) aux_indices[i,0] = (double)this.m_indice_clusters_atuais[i];

            utl.FrequencyTable(ref ftable, aux_indices);

            for (int i = 0; i < this.nobs; i++)
            {
                for (int k = 0; k < nclus; k++)
                {
                    if (this.m_indice_clusters_atuais[i] == (int)ftable[k,0])
                    {
                        for (int j = 0; j < this.nvars; j++)
                        {
                            medias_clusters[k, j] += this.dados[i, j];
                        }
                    }
                }
            }

            for (int k = 0; k < nclus; k++)
            {
                for (int j = 0; j < this.nvars; j++)
                {
                    medias_clusters[k, j] = medias_clusters[k, j] / ftable[k, 1];
                }
            }

            cluster_statistics = utl.Concateh(ftable, medias_clusters);

            prBar.Value = 0;
            prBar.Refresh();
            Application.DoEvents();

            return 1;
        }
        */
		
        #endregion

        #endregion 

        #region Métodos de dissimilaridade

        private double m_peso_dissimilaridade_cluster_size = 1.0;

        private double UpdateDissimilaridade(double distRP, double distRQ, double distPQ, int nQ, int nP, int nR)
        {
            double distancia_nova = 0.0;
            double penalty_tamanho_cluster = 1.0;
            if (this.m_tipo_dados_clusterizacao == TipoDadosClusterizacao.Binarios)
            {
                penalty_tamanho_cluster = 1.0 
                    + (this.m_peso_dissimilaridade_cluster_size * (double)(nP + nQ) / (double)this.dados.GetLength(0));
            }

            switch (this.m_tipo_metodo_clusterizacao)
            {
                case MetodoClusterizacao.SingleLinkage:
                    distancia_nova = 0.5 * distRP + 0.5 * distRQ - 0.5 * Math.Abs(distRP - distRQ);
                    break;

                    //return 0.5 * distRP + 0.5 * distRQ - 0.5 * Math.Abs(distRP - distRQ);

                case MetodoClusterizacao.CompleteLinkage:
                    distancia_nova = 0.5 * distRP + 0.5 * distRQ + 0.5 * Math.Abs(distRP - distRQ);
                    break;
                
                    //return 0.5 * distRP + 0.5 * distRQ + 0.5 * Math.Abs(distRP - distRQ);

                case MetodoClusterizacao.AverageLinkage:
                    distancia_nova = 0.5 * distRP + 0.5 * distRQ;
                    break;
                    
                    //return 0.5 * distRP + 0.5 * distRQ;

                case MetodoClusterizacao.AverageLinkageWeigthed:
                    distancia_nova = ((double)nP / (double)(nP + nQ)) * distRP 
                                     + ((double)nQ / (double)(nP + nQ)) * distRQ;
                    break;

                    //return ((double)nP / (double)(nP + nQ)) * distRP
                    //   + ((double)nQ / (double)(nP + nQ)) * distRQ;

                case MetodoClusterizacao.Centroid:
                    distancia_nova = ((double)nP / (double)(nP + nQ)) * distRP
                            + ((double)nQ / (double)(nP + nQ)) * distRQ
                            - ((double)(nP * nQ) / Math.Pow((double)(nP + nQ), 2.0)) * distPQ;
                    break;

                    //return ((double)nP / (double)(nP + nQ)) * distRP 
                    //    + ((double)nQ / (double)(nP + nQ)) * distRQ 
                    //    - ((double)(nP * nQ) / Math.Pow((double)(nP + nQ), 2.0)) * distPQ;

                case MetodoClusterizacao.Median:
                    distancia_nova = 0.5 * distRP + 0.5 * distRQ - 0.25 * distPQ;
                    break;
                    
                    //return 0.5 * distRP + 0.5 * distRQ - 0.25 * distPQ;

                case MetodoClusterizacao.Ward:
                    distancia_nova = ((double)(nR + nP) / (double)(nP + nQ + nR)) * distRP
                                    + ((double)(nR + nQ) / (double)(nP + nQ + nR)) * distRQ
                                    - ((double)nR / (double)(nP + nQ + nR)) * distPQ;
                    break;

                    //return ((double)(nR + nP) / (double)(nP + nQ + nR)) * distRP 
                    //    + ((double)(nR + nQ) / (double)(nP + nQ + nR)) * distRQ 
                    //    - ((double)nR / (double)(nP + nQ + nR)) * distPQ;

                default:
                    distancia_nova = 0.0;
                    break;

                    //return 0.0;
            }
            return distancia_nova * penalty_tamanho_cluster;
        }

        #endregion 

        #endregion
    }
}
