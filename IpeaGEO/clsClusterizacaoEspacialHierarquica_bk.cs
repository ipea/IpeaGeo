using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Collections;

namespace IpeaGEO
{
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

    public enum TipoDadosClusterizacao : int
    {
        Continuos = 1,
        Binarios = 2,
        Categoricos = 3,
        Mistos = 4
    };

    public class clsClusterizacaoEspacialHierarquica
    {
        private delegate double FuncaoDistancia(double[,] v1, double[,] v2);
        private FuncaoDistancia m_funcao_distancia;

        public clsClusterizacaoEspacialHierarquica()
        {
            this.m_funcao_distancia = new FuncaoDistancia(this.DistanciaContinuaPoligonos);
        }

        #region Variáveis internas

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
                    this.m_funcao_distancia = new FuncaoDistancia(this.DistanciaBinariaPoligonos);
            }
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
                if (this.m_tipo_distancia_continua == TipoDistanciaContinua.VarianceCorrected)
                {
                    if (this.dados != null)
                        this.ConstroiVarianciasVariaveis();
                }

                if (this.m_tipo_distancia_continua == TipoDistanciaContinua.CovarianceCorrected)
                {
                    if (this.dados != null)
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
            set { this.m_estrutura_shape = (clsIpeaShape)value.Clone(); }
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

                if (this.m_tipo_distancia_continua == TipoDistanciaContinua.VarianceCorrected)
                {
                    if (this.dados != null)
                        this.ConstroiVarianciasVariaveis();
                }

                if (this.m_tipo_distancia_continua == TipoDistanciaContinua.CovarianceCorrected)
                {
                    if (this.dados != null)
                        this.ControiInvCovarianciasVariaveis();
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

        #endregion

        #region Critérios de seleção do número de clusters

        private void GeraCCC(ref double ccc, ref double expected_r2, int q, double r2)
        {
            int p = this.nvars;
            int pstar = (int)Math.Min(q, p + 1);
            double c = 0.0;
            double upstar = 0.5;
            double vstar = 0.0;
            int n = this.nobs;

            while (upstar < 1.0)
            {
                pstar--;
                if (pstar <= 1) break;
                vstar = 1.0;
                for (int i = 0; i <= pstar - 1; i++)
                    vstar *= m_ordered_sj[i];

                c = Math.Pow(vstar / (double)q, 1.0 / ((double)pstar));
                upstar = m_ordered_sj[pstar - 1] / c;
            }

            double[] uj = new double[p];
            vstar = 1.0;
            for (int i = 0; i <= pstar - 1; i++)
                vstar *= m_ordered_sj[i];

            c = Math.Pow(vstar / ((double)q), 1.0 / ((double)pstar));

            for (int i = 0; i <= p - 1; i++)
                uj[i] = m_ordered_sj[i] / c;

            double soma1 = 0.0;
            double soma2 = 0.0;

            for (int i = 0; i <= pstar - 1; i++)
            {
                soma1 += 1.0 / (((double)n) + uj[i]);
                soma2 += Math.Pow(uj[i], 2.0);
            }

            for (int i = pstar; i <= p - 1; i++)
            {
                soma1 += Math.Pow(uj[i], 2.0) / (((double)n) + uj[i]);
                soma2 += Math.Pow(uj[i], 2.0);
            }

            expected_r2 = 1.0 - (soma1 / soma2) * (Math.Pow((double)n - (double)q, 2.0) / (double)n) * (1.0 + (4.0 / (double)n));

            ccc = Math.Log((1.0 - expected_r2) / (1.0 - r2)) * (Math.Sqrt((double)pstar * (double)n / 2.0) / (Math.Pow(0.001 + expected_r2, 1.2)));
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

            double[,] matrix_V = clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(matrix_X), matrix_X), 1.0 / (double)(this.nobs - 1));

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
                    W1 += Math.Pow(m1[i, j] - v1[0, j], 2.0);
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

        #region Clustering algorithms

        #region Preenche a matriz com todas as distâncias

        private MatrizArquivo m_distancias;
        private void PreencheTodasDistanciasContinuas(ref ProgressBar prBar)
        {
            this.m_distancias = new MatrizArquivo(this.nobs, this.nobs);
            clsUtilTools clt = new clsUtilTools();

            prBar.Maximum = this.nobs;
            prBar.Value = 0;

            for (int i = 0; i < this.nobs; i++)
            {
                prBar.Value = i;
                Application.DoEvents();

                if (this.m_tipo_metodo_clusterizacao == MetodoClusterizacao.Ward)
                {
                    for (int j = i + 1; j < this.nobs; j++)
                        this.m_distancias[i, j] = 0.5 * this.m_funcao_distancia(clt.SubRowArrayDouble(this.dados, i), clt.SubRowArrayDouble(this.dados, j));
                }
                else
                {
                    for (int j = i + 1; j < this.nobs; j++)
                        this.m_distancias[i, j] = this.m_funcao_distancia(clt.SubRowArrayDouble(this.dados, i), clt.SubRowArrayDouble(this.dados, j));
                }
            }

            prBar.Value = 0;
            prBar.Refresh();
            Application.DoEvents();
        }

        #endregion

        #region Funções auxiliares

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
            if (this.dados == null) throw new Exception("Matriz de dados não definida para clusterização espacial");
            if (this.m_estrutura_shape == null) throw new Exception("Estrutura do shape não definida para clusterização espacial");

            //------------ preenche a matriz com todas as distâncias entre observações ------------------------------------------//

            lblEvolucao.Text = "Geração da matriz de distâncias entre observações iniciais";
            lblEvolucao.Refresh();

            this.PreencheTodasDistanciasContinuas(ref prBar);

            lblEvolucao.Text = "Evolução da formação da árvore de clusters";
            lblEvolucao.Refresh();

            int min_nclus_tree = this.m_num_min_clusters_arvore;
            int max_nclus_tree = this.m_num_max_clusters_arvore;

            temp_shape = this.m_estrutura_shape.Clone();
            clsUtilTools utl = new clsUtilTools();

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

                    break;
                }

                //-----------------------------------------------------------------------------------------------------------------------//

                if (nclus <= this.m_num_min_clusters_arvore) break;

                k1 = this.m_lista_clusters_atuais[0];
                k2 = this.m_lista_clusters_atuais[1];
                distance = 0.0;
                min_distance = this.DistanceClusters(k1, k2);

                for (int i = 0; i < nclus; i++)
                {
                    indices_vizinhos = temp_shape[this.m_lista_clusters_atuais[i]].ListaIndicesVizinhos;

                    for (int j = 0; j < indices_vizinhos.GetLength(0); j++)
                    {
                        if (this.m_lista_clusters_atuais[i] < indices_vizinhos[j])
                        {
                            distance = this.m_distancias[this.m_lista_clusters_atuais[i], indices_vizinhos[j]];

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

                //------------------------- redefinição lista de polígonos no cluster -------------------------//

                nNk = temp_shape[k1].IndicesPoligonosNoCluster.GetLength(0);
                nNl = temp_shape[k2].IndicesPoligonosNoCluster.GetLength(0);
                wWk = this.m_W_to_nclus_criterion[k1];
                wWl = this.m_W_to_nclus_criterion[k2];

                nP = nNk;
                nQ = nNl;

                temp_shape[k1].AddIndicesPoligonosNoCluster(temp_shape[k2].IndicesPoligonosNoCluster);
                temp_shape[k2].ClearIndicesPoligonosNoCluster();

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
                }
            }

            this.m_distancias.DisposeMatriz();
            this.m_distancias = null;

            prBar.Value = 0;
            prBar.Refresh();
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

            for (int i = 0; i < n; i++)
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
            for (int j = 0; j < this.dados.GetLength(1); j++)
                this.m_variancias_variaveis[j] = clt.VarianciaColumnMatrix(clt.SubColumnArrayDouble(this.dados, j));
        }

        private double[,] m_matriz_inv_covariancia = new double[0, 0];
        private void ControiInvCovarianciasVariaveis()
        {
            clsUtilTools clt = new clsUtilTools();
            double[,] covm = clt.CovSampleMatrix(this.dados);
            this.m_matriz_inv_covariancia = clt.MatrizInversa(covm);

            double[,] teste = clt.MatrizMult(this.m_matriz_inv_covariancia, covm);
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

                    return aux[0, 0];
                //return Math.Sqrt(aux[0, 0]);

                default:
                    return 0.0;
            }
        }

        #endregion

        #region Distâncias binárias entre polígonos

        private double DistanciaBinariaPoligonos(double[,] v1, double[,] v2)
        {
            double a1 = 0.0, a2 = 0.0, a3 = 0.0, a4 = 0.0;
            switch (this.m_tipo_distancia_binaria)
            {
                case TipoDistanciaBinaria.Jaccard:
                    for (int i = 0; i < this.nvars; i++)
                    {
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a1++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a2++;
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a3++;
                    }
                    return a1 / (a1 + a2 + a3);
                case TipoDistanciaBinaria.Tanimoto:
                    for (int i = 0; i < this.nvars; i++)
                    {
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a1++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a2++;
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a3++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a4++;
                    }
                    return (a1 + a4) / (a1 + 2.0 * (a2 + a3) + a4);
                case TipoDistanciaBinaria.SimpleMatching:
                    for (int i = 0; i < this.nvars; i++)
                    {
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a1++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a4++;
                    }
                    return (a1 + a4) / (double)this.nvars;
                case TipoDistanciaBinaria.RusselRao:
                    for (int i = 0; i < this.nvars; i++)
                    {
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a1++;
                    }
                    return a1 / (double)this.nvars;
                case TipoDistanciaBinaria.Dice:
                    for (int i = 0; i < this.nvars; i++)
                    {
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a1++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a2++;
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a3++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a4++;
                    }
                    return 2.0 * a1 / (2 * a1 + a2 + a3);
                case TipoDistanciaBinaria.Kulczynski:
                    for (int i = 0; i < this.nvars; i++)
                    {
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a1++;
                        if (v1[0, i] <= this.m_corte_variavel_binaria && v2[0, i] > this.m_corte_variavel_binaria) a2++;
                        if (v1[0, i] > this.m_corte_variavel_binaria && v2[0, i] <= this.m_corte_variavel_binaria) a3++;
                    }
                    return a1 / (a2 + a3);
                default:
                    return 0.0;
            }
        }

        #endregion

        #region Métodos de clusterização

        private double soma_Pg = 0.0;

        private double CalculaValorW(int k1)
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
                //W1 += this.DistanciaContinuaPoligonos(utl.SubRowArrayDouble(m1, i), v1);

                W1 += this.m_funcao_distancia(utl.SubRowArrayDouble(m1, i), v1);
            }

            return W1;
        }

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
            /*
            int n1 = 0;
            int n2 = 0;
            for (int i = 0; i < this.nobs; i++)
            {
                if (this.m_indice_clusters_atuais[i] == k1) n1++;
                if (this.m_indice_clusters_atuais[i] == k2) n2++;
            }

            double[,] m1 = new double[n1, this.nvars];
            double[,] m2 = new double[n2, this.nvars];

            int ind1 = 0;
            int ind2 = 0;

            for (int i = 0; i < this.nobs; i++)
            {
                if (this.m_indice_clusters_atuais[i] == k1)
                {
                    for (int j = 0; j < this.nvars; j++)
                    {
                        m1[ind1, j] = this.dados[i, j];
                    }
                    ind1++;
                }
                if (ind1 == n1) break;
            }

            for (int i = 0; i < this.nobs; i++)
            {            
                if (this.m_indice_clusters_atuais[i] == k2)
                {
                    for (int j = 0; j < this.nvars; j++)
                    {
                        m2[ind2, j] = this.dados[i, j];
                    }
                    ind2++;
                }
                if (ind2 == n2) break;
            }
            */
            #endregion

            switch (this.m_tipo_metodo_clusterizacao)
            {
                case MetodoClusterizacao.Ward:
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

        private double UpdateDissimilaridade(double distRP, double distRQ, double distPQ, int nQ, int nP, int nR)
        {
            switch (this.m_tipo_metodo_clusterizacao)
            {
                case MetodoClusterizacao.SingleLinkage:
                    return 0.5 * distRP + 0.5 * distRQ - 0.5 * Math.Abs(distRP - distRQ);

                case MetodoClusterizacao.CompleteLinkage:
                    return 0.5 * distRP + 0.5 * distRQ + 0.5 * Math.Abs(distRP - distRQ);

                case MetodoClusterizacao.AverageLinkage:
                    return 0.5 * distRP + 0.5 * distRQ;

                case MetodoClusterizacao.AverageLinkageWeigthed:
                    return ((double)nP / (double)(nP + nQ)) * distRP + ((double)nQ / (double)(nP + nQ)) * distRQ;

                case MetodoClusterizacao.Centroid:
                    return ((double)nP / (double)(nP + nQ)) * distRP
                        + ((double)nQ / (double)(nP + nQ)) * distRQ
                        - ((double)(nP * nQ) / Math.Pow((double)(nP + nQ), 2.0)) * distPQ;

                case MetodoClusterizacao.Median:
                    return 0.5 * distRP + 0.5 * distRQ - 0.25 * distPQ;

                case MetodoClusterizacao.Ward:
                    return ((double)(nR + nP) / (double)(nP + nQ + nR)) * distRP
                        + ((double)(nR + nQ) / (double)(nP + nQ + nR)) * distRQ
                        - ((double)nR / (double)(nP + nQ + nR)) * distPQ;

                default:
                    return 0.0;
            }
        }

        #endregion

        #endregion
    }
}
