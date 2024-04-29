using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IpeaGeo;
using System.Data;
using Combinatorial;
using System.Collections;

namespace IpeaGeo.BLogic
{
    public class BLIndicesSegregacaoSpacial
    {
        private DataTable m_dt = new DataTable();
        public DataTable Dados
        {
            set { this.m_dt = value; }
        }

        private string[] m_lista_vars_xi = new string[0];
        public string[] ListaVariaveisXi
        {
            set { this.m_lista_vars_xi = value; }
            get
            {
                return m_lista_vars_xi;
            }
        }

        private string[] m_lista_vars_yi = new string[0];
        public string[] ListaVariaveisYi
        {
            set { this.m_lista_vars_yi = value; }
            get
            {
                return m_lista_vars_yi;
            }
        }

        private string[] m_combina_vars_xi = new string[0];
        public string[] ListaCombinaVariaveisXi
        {
            get
            {
                return m_combina_vars_xi;
            }
        }

        private string[] m_combina_vars_yi = new string[0];
        public string[] ListaCombinaVariaveisYi
        {
            get
            {
                return m_combina_vars_yi;
            }
        }

        private string m_var_tiy = "";
        public string VariavelTiY
        {
            set { this.m_var_tiy = value; }
        }

        private string m_var_ti = "";
        public string VariavelTi
        {
            set { this.m_var_ti = value; }
        }

        private clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {            
            set { this.m_shape = value; }
            //set { this.m_shape = (clsIpeaShape)value.Clone(); }
        }

        public BLIndicesSegregacaoSpacial()
        {
        }

        public double[,] GeraIndiceSegregacaoMultiGroup(TipoIndiceSegregacaoMultiGroup tipo_indice)
        {
            double[,] v = new double[this.m_lista_vars_xi.GetLength(0), 1];

            for (int i = 0; i < v.GetLength(0); i++)
                v[i, 0] = this.GeraIndiceSegregacaoMultiGroup(tipo_indice, this.m_lista_vars_xi[i], this.m_lista_vars_yi[i]);

            return v;
        }
        
        public double GeraIndiceSegregacaoMultiGroup(TipoIndiceSegregacaoMultiGroup tipo_indice, string variavel_xi, string variavel_yi)
        {
            clsIndicesDeSegregacao clindices = new clsIndicesDeSegregacao();

            double valor_indice = 0.0;

            switch (tipo_indice)
            {
                case TipoIndiceSegregacaoMultiGroup.Dissimilarity:
                    valor_indice = clindices.MultigroupDissimilarity(m_shape, this.m_dt, m_lista_vars_xi, this.m_var_ti);
                    break;
                case TipoIndiceSegregacaoMultiGroup.CoeficienteDeGini:
                    valor_indice = clindices.Gini_Coefficient(m_shape, this.m_dt, m_lista_vars_xi, this.m_var_ti);
                    break;
                case TipoIndiceSegregacaoMultiGroup.InformationTheory:
                    valor_indice = clindices.Information_Theory(m_shape, this.m_dt, m_lista_vars_xi, this.m_var_ti);
                    break;
                case TipoIndiceSegregacaoMultiGroup.NormalizedExposure:
                    valor_indice = clindices.Normalized_Exposure(m_shape, this.m_dt, m_lista_vars_xi, this.m_var_ti);
                    break;
                case TipoIndiceSegregacaoMultiGroup.RelativeDiversity:
                    valor_indice = clindices.Relative_Diversity(m_shape, this.m_dt, m_lista_vars_xi, this.m_var_ti);
                    break;
                case TipoIndiceSegregacaoMultiGroup.SquaredcoefficientOfVariation:
                    valor_indice = clindices.Squared_Coefficient_of_Variation(m_shape, this.m_dt, m_lista_vars_xi, this.m_var_ti);
                    break;
              
                default:
                    valor_indice = clindices.Spatial_Proximity_Index(m_shape, this.m_dt, variavel_xi, variavel_yi);
                    break;
            }

            return valor_indice;
        }

        public int fatorial(int n)
        {
            int fatorial = 1;
            for (int i = 1; i <= n; i++)
            {
                fatorial = fatorial * i;
            }
            return fatorial;
        }

        //Gera o numero de combinacoes N, P a P.

        public int Combinacoes_NP(int n, int p)
        {
            int N_fatorial = fatorial(n);
            int P_fatorial = fatorial(p);
            int NP_fatorial = fatorial(n - p);
            int combinacoes = N_fatorial / (P_fatorial * NP_fatorial);

            return (combinacoes);
        }

        public double[,] GeraIndiceSegregacaoTwoGroup(TipoIndiceSegregacaoTwoGroup tipo_indice)
        {
            //ArrayList  thisComb = new ArrayList();
            Combinations combs = new Combinations(m_lista_vars_xi, 2);

            int numero_combinacoes = Combinacoes_NP(m_lista_vars_xi.Length, 2);
            string[] colunaX = new string[numero_combinacoes];
            string[] colunaY = new string[numero_combinacoes];

            int j = 0;
            while (combs.MoveNext())
            {
                Array thisComb = (Array)combs.Current;

                colunaX[j] = thisComb.GetValue(0).ToString();
                colunaY[j] = thisComb.GetValue(1).ToString();
                j++;
            }

            m_combina_vars_xi = colunaX;
            m_combina_vars_yi = colunaY;

            double[,] v = new double[numero_combinacoes,1];
            
            for (int i = 0; i < numero_combinacoes; i++)
            {
                double teste = this.GeraIndiceSegregacaoTwoGroup(tipo_indice, colunaX[i], colunaY[i]);
                v[i, 0] = teste;
            }

            return v;
        }
       
        public double GeraIndiceSegregacaoTwoGroup(TipoIndiceSegregacaoTwoGroup tipo_indice, string variavel_xi, string variavel_yi)
        {

            clsIndicesDeSegregacao clindices = new clsIndicesDeSegregacao();

            double valor_indice = 0.0;

            switch (tipo_indice)
            {
                case TipoIndiceSegregacaoTwoGroup.DissimilarityAdjustedForContiguityBetweenXY:
                    valor_indice = clindices.Dissimilarity_adjusted_for_tract_contiguity(m_shape,1, this.m_dt, variavel_xi, variavel_yi, this.m_var_ti);
                    break;
                case TipoIndiceSegregacaoTwoGroup.DistanceDecayInteractionIndex:
                    valor_indice = clindices.Distance_Decay_Interation_Index(m_shape, this.m_dt, this.m_var_ti, variavel_yi, variavel_xi);
                    break;
                case TipoIndiceSegregacaoTwoGroup.IndexOfDissimilarityBetweenXY:
                    valor_indice = clindices.Index_of_Dissimilarity(this.m_dt, variavel_xi, variavel_yi);
                    break;
                case TipoIndiceSegregacaoTwoGroup.InteractionIndex:
                    valor_indice = clindices.Interaction_Index(this.m_dt, variavel_xi, variavel_yi, this.m_var_ti);
                    break;
                case TipoIndiceSegregacaoTwoGroup.MeanProximityBetweenMembersOfGroupsXY :
                    valor_indice = clindices.Mean_Proximity_Between_Members_of_Group_X_and_members_of_group_Y(this.m_shape, this.m_dt, variavel_xi, variavel_yi);
                    break;
                case TipoIndiceSegregacaoTwoGroup.MeanProximityBetweenMembersOfOneGroupXAndGroupY:
                    valor_indice = clindices.Mean_Proximity_Between_Members_of_one_group_X_and_members_of_group_Y(m_shape, this.m_dt, variavel_xi, variavel_yi);
                    break;
                case TipoIndiceSegregacaoTwoGroup.RelativeClusteringIndex:
                    valor_indice = clindices.Relative_Clustering_Index(this.m_shape, this.m_dt, variavel_xi, variavel_yi);
                    break;
                case TipoIndiceSegregacaoTwoGroup.SpatialProximityIndex:
                    valor_indice = clindices.Spatial_Proximity_Index(m_shape, this.m_dt, variavel_xi, variavel_yi);
                    break;
                
                   default:
                    valor_indice = clindices.Spatial_Proximity_Index(m_shape, this.m_dt, variavel_xi, variavel_yi);
                    break;
            }

            return valor_indice;
        }

        public double[,] GeraIndiceSegregacaoOneGroup(TipoIndiceSegregacaoOneGroup tipo_indice)
        {
            double[,] v = new double[this.m_lista_vars_xi.GetLength(0), 1];

            for (int i = 0; i < v.GetLength(0); i++)
                v[i, 0] = this.GeraIndiceSegregacaoOneGroup(tipo_indice, this.m_lista_vars_xi[i]);

            return v;
        }

        public double GeraIndiceSegregacaoOneGroup(TipoIndiceSegregacaoOneGroup tipo_indice, string variavel_xi)
        {
            clsIndicesDeSegregacao clindices = new clsIndicesDeSegregacao();

            double valor_indice = 0.0;

            switch (tipo_indice)
            {
                case TipoIndiceSegregacaoOneGroup.CoeficienteGini:
                    valor_indice = clindices.Indice_de_Gini(this.m_dt, this.m_var_ti, variavel_xi);
                    break;
                case TipoIndiceSegregacaoOneGroup.IndicadorEntropia:
                    valor_indice = clindices.Entropy_or_Information_Index(this.m_dt, this.m_var_ti, variavel_xi);
                    break;
                case TipoIndiceSegregacaoOneGroup.SegregationIndex:
                    valor_indice = clindices.Eveness_Segregation_Index(this.m_dt, this.m_var_ti, variavel_xi);
                    break;
                case TipoIndiceSegregacaoOneGroup.SegregationIndexAdjustedContiguity:
                    valor_indice = clindices.Eveness_Segregation_Index_Adjusted_for_tracked_Contiguity(this.m_shape,
                        1, this.m_dt, this.m_var_ti, variavel_xi);
                    break;
                case TipoIndiceSegregacaoOneGroup.AbsoulteClustering:
                    valor_indice = clindices.Absolute_Clustering_index(this.m_shape,
                        1, this.m_dt, this.m_var_ti, variavel_xi);
                    break;
                case TipoIndiceSegregacaoOneGroup.CorrelationRatio:
                    valor_indice = clindices.Correlation_Ratio_or_Eta_Squared(this.m_dt, this.m_var_ti, variavel_xi);
                    break;
                case TipoIndiceSegregacaoOneGroup.DistanceDecayIsolationIndex:
                    valor_indice = clindices.Distance_Decay_Isolation_Index(this.m_shape, this.m_dt, this.m_var_ti, variavel_xi);
                    break;
                case TipoIndiceSegregacaoOneGroup.IndiceAtkisnon:
                    valor_indice = clindices.Atkinson_Index(this.m_dt, this.m_var_ti, variavel_xi, 0.5);
                    break;
                case TipoIndiceSegregacaoOneGroup.IsolationIndex:
                    valor_indice = clindices.Isolation_Index(this.m_dt, this.m_var_ti, variavel_xi);
                    break;
                case TipoIndiceSegregacaoOneGroup.MeanProximityBetweenMembersOfGroupX:
                    valor_indice = clindices.Mean_Proximity_Between_Members_of_Group_X(this.m_shape, this.m_dt, this.m_var_ti, variavel_xi);
                    break;
                case TipoIndiceSegregacaoOneGroup.MeanProximityBetweenOfOneGroupExp:
                    valor_indice = clindices.Mean_Proximity_Between_Members_of_one_group_exp(this.m_shape, this.m_dt, this.m_var_ti, variavel_xi);
                    break;
                default:
                    valor_indice = clindices.Indice_de_Gini(this.m_dt, this.m_var_ti, variavel_xi);
                    break;
            }

            return valor_indice;
        }
    }
}
