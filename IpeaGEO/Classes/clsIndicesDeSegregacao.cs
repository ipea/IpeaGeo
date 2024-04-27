using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IpeaGeo
{
    #region Enumerações

    public enum TipoIndiceSegregacaoOneGroup : int
    {
        SegregationIndex = 1,
        SegregationIndexAdjustedContiguity = 2,
        IndicadorEntropia = 3,
        CoeficienteGini = 4,
        IndiceAtkisnon = 5,
        IsolationIndex = 6,
        CorrelationRatio = 7,
        AbsoulteClustering = 8,
        MeanProximityBetweenMembersOfGroupX = 9,
        MeanProximityBetweenOfOneGroupExp = 10,
        DistanceDecayIsolationIndex = 11
    };

    public enum TipoIndiceSegregacaoTwoGroup : int
    {
        IndexOfDissimilarityBetweenXY = 1,
        DissimilarityAdjustedForContiguityBetweenXY = 2,
        InteractionIndex = 3,
        MeanProximityBetweenMembersOfGroupsXY = 4,
        MeanProximityBetweenMembersOfOneGroupXAndGroupY = 5,
        SpatialProximityIndex = 6,
        RelativeClusteringIndex = 7,
        DistanceDecayInteractionIndex = 8
    };

    public enum TipoIndiceSegregacaoMultiGroup : int
    {
        Dissimilarity = 1,
        CoeficienteDeGini = 2,
        InformationTheory = 3,
        NormalizedExposure = 4,
        RelativeDiversity = 5,
        SquaredcoefficientOfVariation = 6
    };



    #endregion

    class clsIndicesDeSegregacao
    {
        private double T = 0.0;
        private double P = 0.0;
        private double X = 0.0;
        private double Y = 0.0;
        private double I = 0.0;
        private double A = 0.0;

        #region Indices One Group;

        #region Indices de Segregacao One Group Eveness

        /// <summary>
        /// Calcula o índice de Segregacao " One-Group Eveness Segregation Index" 
        /// </summary>
        /// <param name="tabela">Tabela de entrada dos dados</param>
        /// <param name="strTotal">Variável com o total da população na região</param>
        /// <param name="strGrupo">Variável com o total do grupo</param>
        /// <returns></returns>
        public double Eveness_Segregation_Index(DataTable tabela, string strTotal, string strGrupo)
        {
            double IS = 0;
            double T = 0;
            double X = 0;

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                T += (double)tabela.Rows[i][strTotal];
                X += (double)tabela.Rows[i][strGrupo];
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double xi = (double)tabela.Rows[i][strGrupo];
                double ti = (double)tabela.Rows[i][strTotal];

                IS += Math.Abs((xi / X) - ((ti - xi) / (T - X)));
            }

            return (IS / 2);
        }

        /// <summary>
        /// Calcula o índice de Segregacao " One-Group Eveness Segregation Index adjusted for tract contiguity" 
        /// </summary>
        /// <param name="mShape">shapefile</param>
        /// <param name="tipo_vizinhanca">1 para Queen ou 2 para Rook</param>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="strTotal">Variável com o total da população na região</param>
        /// <param name="strGrupo">Variável com o total do grupo</param>
        /// <returns></returns>
        public double Eveness_Segregation_Index_Adjusted_for_tracked_Contiguity(clsIpeaShape mShape,
            int tipo_vizinhanca, DataTable tabela, string strTotal, string strGrupo)
        {
            if (mShape.TipoVizinhanca == "" && (tipo_vizinhanca == 1 || tipo_vizinhanca == 2))
            {
                //Cria a vizinhnaça
                clsIpeaShape cps = new clsIpeaShape();

                tipo_vizinhanca = 1;

                if (tipo_vizinhanca == 1) mShape.TipoVizinhanca = "Queen";
                if (tipo_vizinhanca == 2) mShape.TipoVizinhanca = "Rook";

                cps.DefinicaoVizinhos(ref mShape, tipo_vizinhanca);
            }

            double IS = Eveness_Segregation_Index(tabela, strTotal, strGrupo);
            double corretor = 0;
            double somavizinho = 0;

            for (int i = 0; i < mShape.Count; i++)
            {
                int ibase = mShape[i].PosicaoNoDataTable;
                double xi = (double)tabela.Rows[ibase][strGrupo];
                double ti = (double)tabela.Rows[ibase][strTotal];

                for (int j = 0; j < mShape[i].NumeroVizinhos; j++)
                {
                    int jShape = mShape[i].ListaIndicesVizinhos[j];
                    int jbase = mShape[jShape].PosicaoNoDataTable;

                    double xj = (double)tabela.Rows[jbase][strGrupo];
                    double tj = (double)tabela.Rows[jbase][strTotal];

                    corretor += Math.Abs((xi / ti) - (xj / tj));
                    somavizinho++;
                }
            }

            return (IS - (corretor / somavizinho));
        }

        /// <summary>
        /// Calcula o Índicador de Entropia ou Índice de Informação
        /// </summary>
        /// <param name="tabela">Tabela de Dados</param>
        /// <param name="strTotal">Variável com o total da população na região</param>
        /// <param name="strGrupo">Variável com o total do grupo</param>
        /// <returns></returns>
        public double Entropy_or_Information_Index(DataTable tabela, string strTotal, string strGrupo)
        {
            double Entropia = 0;
            double E = 0;
            double ei = 0;
            double T = 0;
            double X = 0;

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                T += (double)tabela.Rows[i][strTotal];
                X += (double)tabela.Rows[i][strGrupo];
            }

            E = (((X / T) * (Math.Log(T / X))) + (1 - (X / T)) * Math.Log((T / (T - X))));

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double xi = (double)tabela.Rows[i][strGrupo];
                double ti = (double)tabela.Rows[i][strTotal];

                ei = (((xi / ti) * (Math.Log(ti / xi))) + (1 - (xi / ti)) * Math.Log((ti / (ti - xi))));

                Entropia += (ti * (E - ei) / (E * T));
            }

            return (Entropia);
        }

        /// <summary>
        /// Retorno o coeficiente de Gini
        /// </summary>
        /// <param name="tabela">tabela de dados</param>
        /// <param name="strTotal">Variável com o total da população na região</param>
        /// <param name="strGrupo">Variável com o total do grupo</param>
        /// <returns></returns>
        public double Indice_de_Gini(DataTable tabela, string strTotal, string strGrupo)
        {
            double gini = 0;
            double T = 0;
            double X = 0;

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                T += (double)tabela.Rows[i][strTotal];
                X += (double)tabela.Rows[i][strGrupo];
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double xi = (double)tabela.Rows[i][strGrupo];
                double ti = (double)tabela.Rows[i][strTotal];

                for (int j = 0; j < tabela.Rows.Count; j++)
                {
                    double xj = (double)tabela.Rows[j][strGrupo];
                    double tj = (double)tabela.Rows[j][strTotal];

                    gini += (ti * tj) * (Math.Abs((xi / ti) - (xj / tj)) / (2 * T * T * (X / T) * (1 - (X / T))));
                }
            }

            return (gini);
        }

        /// <summary>
        /// Retorna o índice "Atkinson Index"
        /// </summary>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="strTotal">Variável com o total da população na região</param>
        /// <param name="strGrupo">Variável com o total do grupo</param>
        /// <param name="b">Shape parameter varies between 0 and 1</param>
        /// <returns></returns>
        public double Atkinson_Index(DataTable tabela, string strTotal, string strGrupo, double b)
        {
            double indice = 0;
            double A = 0;
            double a1 = 0;
            double a2 = 0;
            double T = 0;
            double X = 0;

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                T += (double)tabela.Rows[i][strTotal]; //População total X/T = P
                X += (double)tabela.Rows[i][strGrupo]; //Total de casos
            }

            a1 = (((X / T) / (1 - (X / T))));

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double xi = (double)tabela.Rows[i][strGrupo];
                double ti = (double)tabela.Rows[i][strTotal];

                double parte1 = Math.Pow(1 - (xi / ti), (1 - b));
                double parte2 = Math.Pow((xi / ti), b);
                double parte3 = parte1 * parte2 * ti / X;
                a2 += parte3;
            }

            a2 = Math.Pow(Math.Abs(a2), (1.0 / (1.0 - b)));

            indice = 1 - (a1 * a2);
            return (indice);
        }

        #endregion

        #region Indices de Segregacao One Group Exposition

        /// <summary>
        /// Calcula o indicador de Exposição "Isolation Index"
        /// </summary>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="strTotal">Variável com o total da população na região</param>
        /// <param name="strGrupo">Variável com o total do grupo</param>
        /// <returns></returns>
        public double Isolation_Index(DataTable tabela, string strTotal, string strGrupo)
        {
            double Isolation = 0;
            double T = 0;
            double X = 0;

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                T += (double)tabela.Rows[i][strTotal];
                X += (double)tabela.Rows[i][strGrupo];
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double xi = (double)tabela.Rows[i][strGrupo];
                double ti = (double)tabela.Rows[i][strTotal];

                Isolation += (xi / X) * (xi / ti);

            }

            return (Isolation);
        }

        /// <summary>
        /// Retorna o indicador Correlation Ratio or Eta Squared
        /// </summary>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="strTotal">Variável com o total da população na região</param>
        /// <param name="strGrupo">Variável com o total do grupo</param>
        /// <returns></returns>
        public double Correlation_Ratio_or_Eta_Squared(DataTable tabela, string strTotal, string strGrupo)
        {
            double Eta2 = 0;
            double T = 0;
            double X = 0;

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                T += (double)tabela.Rows[i][strTotal];
                X += (double)tabela.Rows[i][strGrupo];
            }

            Eta2 = ((Isolation_Index(tabela, strTotal, strGrupo) - (X / T)) / (1 - (X / T)));

            return (Eta2);
        }






        #endregion

        #region Indices de Segregacao One Group Clustering


        /// <summary>
        /// Retorna o indicador One group Absolute Clustering Index
        /// </summary>
        /// <param name="mShape">Shape dos dados</param>
        /// <param name="tipo_vizinhanca">1 para Queen ou 2 para Rook</param>
        /// <param name="tabela">Tabela de entrada dos dados</param>
        /// <param name="strTotal">Variável com o total da população na região</param>
        /// <param name="strGrupo">Variável com o total do grupo</param>
        /// <returns></returns>


        public double Absolute_Clustering_index(clsIpeaShape mShape, int tipo_vizinhanca, DataTable tabela, string strTotal, string strGrupo)
        {
            if (mShape.TipoVizinhanca == "" && (tipo_vizinhanca == 1 || tipo_vizinhanca == 2))
            {
                //Cria a vizinhnaça
                clsIpeaShape cps = new clsIpeaShape();

                tipo_vizinhanca = 1;

                if (tipo_vizinhanca == 1) mShape.TipoVizinhanca = "Queen";
                if (tipo_vizinhanca == 2) mShape.TipoVizinhanca = "Rook";

                cps.DefinicaoVizinhos(ref mShape, tipo_vizinhanca);
            }

            double IS = Eveness_Segregation_Index(tabela, strTotal, strGrupo);
            double corretor = 0;
            double somavizinho = 0;
            double X = 0;
            double T = 0;
            double municipios = 0;
            double ACL2 = 0;
            double ACL1 = 0;
            double ACL3 = 0;


            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                T += (double)tabela.Rows[i][strTotal];
                X += (double)tabela.Rows[i][strGrupo];
                municipios++;

            }
            double xj = 0;
            double tj = 0;

            for (int i = 0; i < mShape.Count; i++)
            {

                int ibase = mShape[i].PosicaoNoDataTable;
                double xi = (double)tabela.Rows[ibase][strGrupo];
                double ti = (double)tabela.Rows[ibase][strTotal];
                double A = (xi / X);

                for (int j = 0; j < mShape[i].NumeroVizinhos; j++)
                {
                    int jShape = mShape[i].ListaIndicesVizinhos[j];
                    int jbase = mShape[jShape].PosicaoNoDataTable;

                    xj += (double)tabela.Rows[jbase][strGrupo];
                    tj += (double)tabela.Rows[jbase][strTotal];

                    somavizinho++;
                }
                ACL1 += (A * xj);
                ACL3 += (A * tj);
                xj = 0;
                tj = 0;
            }
            ACL2 = (X * somavizinho / (municipios * municipios));
            return ((ACL1 - ACL2) / (ACL3 - ACL2));

        }

        /// <summary>
        /// Retorna o índice "Mean Proximity between members of group X"
        /// </summary>
        /// <param name="mShape">Shape dos dados</param>
        /// <param name="latitude">Variável Latitude no Shape</param>
        /// <param name="longitude">Variável Longitude no Shape</param>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="strTotal">Variável com o total da população na região</param>
        /// <param name="strGrupo">Variável com o total do grupo</param>
        /// <returns></returns>
        public double Mean_Proximity_Between_Members_of_Group_X(clsIpeaShape mShape, DataTable tabela, string strTotal, string strGrupo)
        {
            clsAreaPerimetroCentroide funcao = new clsAreaPerimetroCentroide();
            double[,] dij = new double[tabela.Rows.Count, tabela.Rows.Count];
            double X = 0;
            double A1 = 0;
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                X += (double)tabela.Rows[i][strGrupo];
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {

                double xi = (double)tabela.Rows[i][strGrupo];

                for (int j = i + 1; j < tabela.Rows.Count - 1; j++)
                {
                    if (!mShape.TipoDistancia)
                    {
                        mShape.GerarMatrizTodasDistancias();
                    }

                    dij[i, j] = mShape.MatrizAllDistances[i, j];

                    double xj = (double)tabela.Rows[j][strGrupo];

                    if (!double.IsNaN(dij[i, j]))
                    {
                        A1 += (xi * xj * dij[i, j]) / (X * X);
                    }

                }

            }

            double A2 = A1 * 2;



            double MPBMGX = A2;

            return (MPBMGX);

        }

        /// <summary>
        /// Retorna o indicador "Mean Proximity Between Members of one group(exp)"
        /// </summary>
        /// <param name="mShape">Shape dos dados</param>
        /// <param name="latitude">Variável Latitude no Shape</param>
        /// <param name="longitude">Variável Longitude no Shape</param>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="strTotal">Variável com o total da população na região</param>
        /// <param name="strGrupo">Variável com o total do grupo</param>
        /// <returns></returns>
        public double Mean_Proximity_Between_Members_of_one_group_exp(clsIpeaShape mShape, DataTable tabela, string strTotal, string strGrupo)
        {
            clsAreaPerimetroCentroide funcao = new clsAreaPerimetroCentroide();
            double[,] dij = new double[tabela.Rows.Count, tabela.Rows.Count];
            double X = 0;
            double A1 = 0;
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                X += (double)tabela.Rows[i][strGrupo];
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double xi = (double)tabela.Rows[i][strGrupo];

                for (int j = i + 1; j < tabela.Rows.Count - 1; j++)
                {

                    if (!mShape.TipoDistancia)
                    {
                        mShape.GerarMatrizTodasDistancias();
                    }

                    dij[i, j] = mShape.MatrizAllDistances[i, j];
                    double xj = (double)tabela.Rows[j][strGrupo];

                    if (!double.IsNaN(dij[i, j]))
                    {
                        A1 += (xi * xj * (Math.Exp(-dij[i, j]))) / (X * X);
                    }

                }

            }
            double A2 = A1 * 2;
            double MPBMGXE = A2;
            return (MPBMGXE);

        }
        //TODO: Indice NaN
        /// <summary>
        /// Retorna o indicador Distance Decay isolation Index"
        /// </summary>
        /// <param name="mShape">Shape dos dados</param>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="strTotal">Variável com o total da população na região</param>
        /// <param name="strGrupo">Variável com o total do grupo</param>
        /// <returns></returns>
        public double Distance_Decay_Isolation_Index(clsIpeaShape mShape, DataTable tabela, string strTotal, string strGrupo)
        {
            clsAreaPerimetroCentroide funcao = new clsAreaPerimetroCentroide();

            double X = 0;
            double A1 = 0;
            double A2 = 0;
            double A3 = 0;
            int guarda_i = 0;
            double[,] dij = new double[tabela.Rows.Count, tabela.Rows.Count];

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                X += (double)tabela.Rows[i][strGrupo];
            }


            double[,] kij = new double[tabela.Rows.Count, tabela.Rows.Count];
            double[] denominador_kij = new double[tabela.Rows.Count];

            if (!mShape.TipoDistancia)
            {
                mShape.GerarMatrizTodasDistancias();
            }


            for (int j = 0; j < tabela.Rows.Count; j++)
            {
                double tj = (double)tabela.Rows[j][strTotal];

                for (int i = 0; i < tabela.Rows.Count; i++)
                {
                    double ti = (double)tabela.Rows[i][strTotal];
                    dij[i, j] = mShape.MatrizAllDistances[i, j]; ;

                    if (!double.IsNaN(dij[i, j]))
                    {
                        double nome = ti * Math.Exp(-dij[i, j]);
                        A1 += nome;
                    }
                }
                denominador_kij[j] = A1;
                A1 = 0;
            }




            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                A2 = 0;
                double xi = (double)tabela.Rows[i][strGrupo];
                //guarda_i = i;

                for (int j = 0; j < tabela.Rows.Count; j++)
                {
                    double tj = (double)tabela.Rows[j][strTotal];
                    double xj = (double)tabela.Rows[j][strGrupo];
                    A1 = 0;

                    kij[i, j] = (Math.Exp(-dij[i, j]) / (denominador_kij[j]));

                    if (!double.IsNaN(kij[i, j]))
                    {
                        A2 += (xj * kij[i, j]);
                    }
                }
                A3 += (xi / X) * A2;

                //i = guarda_i;

            }


            //Codigo antigo

            /*



            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                for (int j = 0; j < tabela.Rows.Count; j++)
                {
                    double tj = (double)tabela.Rows[j][strTotal];


                    dij[i, j] = funcao.distancia(mShape[i].YCentroide, mShape[i].XCentroide, mShape[j].YCentroide, mShape[j].XCentroide, false);

                    if (!double.IsNaN(dij[i, j]))
                    {
                        double q = tj * Math.Exp(-dij[i, j]) / A1;
                        kij[i, j] = q;
                    }

                }

            }
            double B1 = 0;
            double B2 = 0;
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                for (int j = 0; j < tabela.Rows.Count; j++)
                {
                    double tj = (double)tabela.Rows[j][strTotal];
                    double xj = (double)tabela.Rows[j][strGrupo];

                    B1 += ((kij[i,j] * xj)/tj);
                           
                }
                double xi = (double)tabela.Rows[i][strGrupo];
                B2 += (xi / X) * B1;
                B1 = 0;
            }*/

            return (A3);

        }



        #endregion

        #endregion;

        #region  Indices Two Group;

        #region Indices de Segregacao Two Group Evenness;
        /// <summary>
        /// Retorna o índice "Index of Dissimilarity entre os grupos X e Y"
        /// </summary>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="nomeX">Variável identificadora grupo X</param>
        /// <param name="nomeY">Variável identificadora grupo Y</param>
        /// <returns></returns>
        public double Index_of_Dissimilarity(DataTable tabela, string nomeX, string nomeY)
        {
            double IDi = 0;
            double Y = 0;
            double X = 0;

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                Y += (double)tabela.Rows[i][nomeY];
                X += (double)tabela.Rows[i][nomeX];
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double xi = (double)tabela.Rows[i][nomeX];
                double yi = (double)tabela.Rows[i][nomeY];

                IDi += Math.Abs((xi / X) - (yi / Y));

            }

            return (IDi / 2);
        }


        /// <summary>
        /// Retorna o índice "Dissimilarity Adjusted for Tract Contiguity" entre os grupos X e Y
        /// </summary>
        /// <param name="mShape">Shape</param>
        /// <param name="tipo_vizinhanca">Tipo de vizinhança, 1 para Queen e 2 para Rook</param>
        /// <param name="tabela">Tabela com os dados</param>
        /// <param name="nomeX">Variável indicadora do Grupo X</param>
        /// <param name="nomeY">Variável Indicadora do Grupo Y</param>
        /// <param name="strTotal">Variável com o total da população na região</param>
        /// <returns></returns>
        public double Dissimilarity_adjusted_for_tract_contiguity(clsIpeaShape mShape, int tipo_vizinhanca, DataTable tabela, string nomeX, string nomeY, string strTotal)
        {
            if (mShape.TipoVizinhanca == "" && (tipo_vizinhanca == 1 || tipo_vizinhanca == 2))
            {
                //Cria a vizinhnaça
                clsIpeaShape cps = new clsIpeaShape();

                tipo_vizinhanca = 1;

                if (tipo_vizinhanca == 1) mShape.TipoVizinhanca = "Queen";
                if (tipo_vizinhanca == 2) mShape.TipoVizinhanca = "Rook";

                cps.DefinicaoVizinhos(ref mShape, tipo_vizinhanca);
            }

            double IDi = Eveness_Segregation_Index(tabela, nomeX, nomeY);
            double corretor = 0;
            double somavizinho = 0;

            for (int i = 0; i < mShape.Count; i++)
            {

                int ibase = mShape[i].PosicaoNoDataTable;
                double xi = (double)tabela.Rows[ibase][nomeX];
                double ti = (double)tabela.Rows[ibase][strTotal];

                for (int j = 0; j < mShape[i].NumeroVizinhos; j++)
                {
                    int jShape = mShape[i].ListaIndicesVizinhos[j];
                    int jbase = mShape[jShape].PosicaoNoDataTable;



                    double xj = (double)tabela.Rows[jbase][nomeX];
                    double tj = (double)tabela.Rows[jbase][strTotal];


                    corretor += Math.Abs((xi / ti) - (xj / tj));
                    somavizinho++;

                }

            }

            return (IDi - (corretor / somavizinho));

        }
        #endregion;

        #region Indices de Segregacao Two Group Exposition;

        public double Interaction_Index(DataTable tabela, string nomeX, string nomeY, string strTotal)
        {
            double Interaction = 0;
            double T = 0;
            double X = 0;
            double Y = 0;


            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                X += (double)tabela.Rows[i][nomeX];
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double xi = (double)tabela.Rows[i][nomeX];
                double yi = (double)tabela.Rows[i][nomeY];
                double ti = (double)tabela.Rows[i][strTotal];

                Interaction += (xi / X) * (yi / ti);

            }

            return (Interaction);
        }










        #endregion;

        #region Indices de Segregacao Two Group Clustering;

        private double emrad = (Math.PI) / 180;

        public double Mean_Proximity_Between_Members_of_Group_X_and_members_of_group_Y(clsIpeaShape mShape, DataTable tabela, string nomeX, string nomeY)
        {
            clsAreaPerimetroCentroide funcao = new clsAreaPerimetroCentroide();
            double[,] dij = new double[tabela.Rows.Count, tabela.Rows.Count];
            double X = 0;
            double Y = 0;
            double A1 = 0;
            if (!mShape.TipoDistancia)
            {
                mShape.GerarMatrizTodasDistancias();
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                X += (double)tabela.Rows[i][nomeX];
                Y += (double)tabela.Rows[i][nomeY];
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double xi = (double)tabela.Rows[i][nomeX];

                for (int j = 0; j < tabela.Rows.Count; j++)
                {
                    dij[i, j] = mShape.MatrizAllDistances[i, j];

                    double yj = Convert.ToDouble(tabela.Rows[j][nomeY]);
                    if (!double.IsNaN(dij[i, j]))
                    {
                        A1 += (xi * yj * dij[i, j]);
                    }
                }

            }

            double MPBMGX = A1 / (X * Y);
            return (MPBMGX);

        }

        public double Mean_Proximity_Between_Members_of_one_group_X_and_members_of_group_Y(clsIpeaShape mShape, DataTable tabela, string nomeX, string nomeY)
        {
            clsAreaPerimetroCentroide funcao = new clsAreaPerimetroCentroide();
            double[,] dij = new double[tabela.Rows.Count, tabela.Rows.Count];
            double X = 0;
            double Y = 0;
            double A1 = 0;
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                X += (double)tabela.Rows[i][nomeX];
                Y += (double)tabela.Rows[i][nomeY];
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double xi = (double)tabela.Rows[i][nomeX];

                for (int j = 0; j < tabela.Rows.Count; j++)
                {

                    if (!mShape.TipoDistancia)
                    {
                        mShape.GerarMatrizTodasDistancias();
                    }

                    dij[i, j] = mShape.MatrizAllDistances[i, j];

                    double yj = (double)tabela.Rows[j][nomeY];
                    if (!double.IsNaN(dij[i, j]))
                    {
                        A1 += (xi * yj * Math.Exp(-dij[i, j]));
                    }
                }

            }
            double MPBMGX = A1 / (X * Y);
            return (MPBMGX);

        }



        public double Spatial_Proximity_Index(clsIpeaShape mShape, DataTable tabela, string nomeX, string nomeY)
        {
            clsAreaPerimetroCentroide funcao = new clsAreaPerimetroCentroide();
            double[,] dij = new double[tabela.Rows.Count, tabela.Rows.Count];
            double X = 0;
            double Y = 0;
            double A1 = 0;
            double A2 = 0;
            double A3 = 0;
            double SP = 0;
            if (!mShape.TipoDistancia)
            {
                mShape.GerarMatrizTodasDistancias();
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                X += (double)tabela.Rows[i][nomeX];
                Y += (double)tabela.Rows[i][nomeY];
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double xi = (double)tabela.Rows[i][nomeX];
                double yi = (double)tabela.Rows[i][nomeY];


                for (int j = 0; j < tabela.Rows.Count; j++)
                {
                    dij[i, j] = mShape.MatrizAllDistances[i, j];
                    double yj = (double)tabela.Rows[j][nomeY];
                    double xj = (double)tabela.Rows[j][nomeX];
                    if (!double.IsNaN(dij[i, j]))
                    {
                        A1 += ((xi + yi) * (xj + yj) * Math.Exp(-dij[i, j]));
                    }
                }

            }
            double P00 = A1 / (X * Y);


            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double xi = (double)tabela.Rows[i][nomeX];

                for (int j = 0; j < tabela.Rows.Count; j++)
                {
                    dij[i, j] = mShape.MatrizAllDistances[i, j];
                    double xj = (double)tabela.Rows[j][nomeX];
                    if (!double.IsNaN(dij[i, j]))
                    {
                        A2 += (xi * xj * Math.Exp(-dij[i, j]));
                    }
                }

            }
            double Pxx = A2 / (X * X);

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double yi = (double)tabela.Rows[i][nomeY];

                for (int j = 0; j < tabela.Rows.Count; j++)
                {
                    dij[i, j] = mShape.MatrizAllDistances[i, j];
                    double yj = (double)tabela.Rows[j][nomeY];
                    if (!double.IsNaN(dij[i, j]))
                    {
                        A3 += (yi * yj * Math.Exp(-dij[i, j]));
                    }
                }

            }
            double Pyy = A3 / (Y * Y);


            SP = (((X * Pxx) + (Y * Pyy)) / ((X + Y) * P00));


            return (SP);

        }


        public double Relative_Clustering_Index(clsIpeaShape mShape, DataTable tabela, string nomeX, string nomeY)
        {
            clsAreaPerimetroCentroide funcao = new clsAreaPerimetroCentroide();
            double[,] dij = new double[tabela.Rows.Count, tabela.Rows.Count];
            double X = 0;
            double Y = 0;
            double A1 = 0;
            double A2 = 0;
            double A3 = 0;
            double RCI = 0;
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                X += (double)tabela.Rows[i][nomeX];
                Y += (double)tabela.Rows[i][nomeY];
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double xi = (double)tabela.Rows[i][nomeX];

                for (int j = 0; j < tabela.Rows.Count; j++)
                {

                    if (!mShape.TipoDistancia)
                    {
                        mShape.GerarMatrizTodasDistancias();
                    }

                    dij[i, j] = mShape.MatrizAllDistances[i, j];
                    double xj = (double)tabela.Rows[j][nomeX];
                    if (!double.IsNaN(dij[i, j]))
                    {
                        A2 += (xi * xj * Math.Exp(-dij[i, j]));
                    }
                }

            }
            double Pxx = A2 / (X * X);

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                double yi = (double)tabela.Rows[i][nomeY];

                for (int j = 0; j < tabela.Rows.Count; j++)
                {

                    if (!mShape.TipoDistancia)
                    {
                        mShape.GerarMatrizTodasDistancias();
                    }

                    dij[i, j] = mShape.MatrizAllDistances[i, j];
                    double yj = (double)tabela.Rows[j][nomeY];
                    if (!double.IsNaN(dij[i, j]))
                    {
                        A3 += (yi * yj * Math.Exp(-dij[i, j]));
                    }
                }

            }
            double Pyy = A3 / (Y * Y);


            RCI = (Pxx / Pyy) - 1;


            return (RCI);

        }
        //TODO: Checar a formula. B2 tende a infinito pois B1 tende a zero.
        public double Distance_Decay_Interation_Index(clsIpeaShape mShape, DataTable tabela, string strTotal, string nomeX, string nomeY)
        {
            clsAreaPerimetroCentroide funcao = new clsAreaPerimetroCentroide();

            double X = 0;
            double A1 = 0;
            double[,] dij = new double[tabela.Rows.Count, tabela.Rows.Count];
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                X += (double)tabela.Rows[i][nomeX];
            }


            double[,] kij = new double[tabela.Rows.Count, tabela.Rows.Count];

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                for (int j = i + 1; j < tabela.Rows.Count; j++)
                {
                    double ti = (double)tabela.Rows[i][strTotal];
                    double tj = (double)tabela.Rows[j][strTotal];
                    double[] centroidei = new double[2];
                    centroidei[0] = mShape[i].YCentroide * emrad;
                    centroidei[1] = mShape[i].XCentroide * emrad;
                    double[] centroidej = new double[2];
                    centroidej[0] = mShape[j].YCentroide * emrad;
                    centroidej[1] = mShape[j].XCentroide * emrad;


                    if (!mShape.TipoDistancia)
                    {
                        mShape.GerarMatrizTodasDistancias();
                    }

                    dij[i, j] = mShape.MatrizAllDistances[i, j];

                    if (!double.IsNaN(dij[i, j]))
                    {
                        double nome = ti * Math.Exp(-dij[i, j]);
                        A1 += nome;
                    }

                }

            }
            A1 = A1 * 2;

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                for (int j = 0; j < tabela.Rows.Count; j++)
                {
                    double tj = (double)tabela.Rows[j][strTotal];

                    dij[i, j] = mShape.MatrizAllDistances[i, j];

                    if (!double.IsNaN(dij[i, j]))
                    {
                        double q = tj * Math.Exp(-dij[i, j]) / A1;
                        kij[i, j] = q;
                    }

                }

            }
            double B1 = 0;
            double B2 = 0;
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                for (int j = i + 1; j < tabela.Rows.Count; j++)
                {
                    double tj = (double)tabela.Rows[j][strTotal];
                    double xj = (double)tabela.Rows[j][nomeX];
                    double yj = (double)tabela.Rows[j][nomeY];

                    double conta = ((kij[i, j] * yj) / tj);
                    if (!double.IsInfinity(conta))
                    {
                        B1 += conta;
                    }

                }
                double xi = (double)tabela.Rows[i][nomeX];
                B2 += (xi * B1) / (X);
                B1 = 0;
            }
            B2 = B2 * 2;
            return (B2);

        }

        #endregion;


        #endregion;

        #region Indices Multigroup;


        #region Multigroup Eveness;
        /// <summary>
        /// Retorna o indicador Multigroup "Dissimilarity"
        /// </summary>
        /// <param name="shape">Shape com entrada de dados</param>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="nomes">Vetor Com o nome das Variáveis dos Grupos </param>
        /// <param name="total">Variável com o total da população</param>
        /// <returns></returns>
        public double MultigroupDissimilarity(clsIpeaShape shape, DataTable tabela, string[] nomes, string total)
        {
            double[,] PIjm = new double[tabela.Rows.Count, nomes.Length];
            double[] PIm = new double[nomes.Length];
            double T = 0;
            double I = 0;
            double D = 0;
            double tmp1 = 0;

            for (int i = 0; i < shape.Count; i++)
            {
                int ilinha = shape[i].PosicaoNoDataTable;
                for (int j = 0; j < nomes.Length; j++)
                {
                    PIjm[i, j] = Convert.ToDouble(tabela.Rows[ilinha][nomes[j]]) / Convert.ToDouble(tabela.Rows[ilinha][total]);
                    PIm[j] += Convert.ToDouble(tabela.Rows[ilinha][nomes[j]]);

                }
                T += Convert.ToDouble(tabela.Rows[ilinha][total]);
            }

            for (int i = 0; i < PIm.Length; i++)
            {
                PIm[i] /= T;
                I += PIm[i] * (1 - PIm[i]);
            }

            for (int j = 0; j < PIjm.GetLength(0); j++)
            {
                int jlinha = shape[j].PosicaoNoDataTable;
                double tj = Convert.ToDouble(tabela.Rows[jlinha][total]);
                for (int m = 0; m < PIjm.GetLength(1); m++)
                {
                    tmp1 += (tj * Math.Abs(PIjm[j, m] - PIm[m]));
                }
            }

            D = ((1 / (2 * T * I)) * tmp1);

            return (D);
        }


        /// <summary>
        /// Retorna o indicador Multigroup "Gini Coefficient"
        /// </summary>
        /// <param name="shape">Shape com entrada de dados</param>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="nomes">Vetor Com o nome das Variáveis dos Grupos </param>
        /// <param name="total">Variável com o total da população</param>
        /// <returns></returns>
        public double Gini_Coefficient(clsIpeaShape shape, DataTable tabela, string[] nomes, string total)
        {
            double[,] PIjm = new double[tabela.Rows.Count, nomes.Length];
            double[] PIm = new double[nomes.Length];
            double T = 0;
            double I = 0;
            double Gini = 0;
            double tmp1 = 0;

            for (int i = 0; i < shape.Count; i++)
            {
                int ilinha = shape[i].PosicaoNoDataTable;
                for (int j = 0; j < nomes.Length; j++)
                {
                    PIjm[i, j] = Convert.ToDouble(tabela.Rows[ilinha][nomes[j]]) / Convert.ToDouble(tabela.Rows[ilinha][total]);
                    PIm[j] += Convert.ToDouble(tabela.Rows[ilinha][nomes[j]]);

                }
                T += Convert.ToDouble(tabela.Rows[ilinha][total]);
            }

            for (int i = 0; i < PIm.Length; i++)
            {
                PIm[i] /= T;
                I += PIm[i] * (1 - PIm[i]);
            }





            for (int i = 0; i < PIjm.GetLength(0); i++)
            {
                int ilinha = shape[i].PosicaoNoDataTable;
                double ti = Convert.ToDouble(tabela.Rows[ilinha][total]);
                for (int j = 0; j < PIjm.GetLength(0); j++)
                {
                    int jlinha = shape[j].PosicaoNoDataTable;
                    double tj = Convert.ToDouble(tabela.Rows[jlinha][total]);

                    for (int m = 0; m < PIjm.GetLength(1); m++)
                    {
                        tmp1 += (ti * tj * Math.Abs(PIjm[i, m] - PIjm[j, m]));
                    }
                }
            }
            Gini = ((1 / (2 * T * T * I)) * tmp1);

            return (Gini);
        }


        /// <summary>
        /// Retorna o indicador Multigroup "Information Theory"
        /// </summary>
        /// <param name="shape">Shape com entrada de dados</param>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="nomes">Vetor Com o nome das Variáveis dos Grupos </param>
        /// <param name="total">Variável com o total da população</param>
        /// <returns></returns>
        public double Information_Theory(clsIpeaShape shape, DataTable tabela, string[] nomes, string total)
        {
            double[,] PIjm = new double[tabela.Rows.Count, nomes.Length];
            double[] PIm = new double[nomes.Length];
            double T = 0;
            double E = 0;
            double IT = 0;
            double tmp1 = 0;

            for (int i = 0; i < shape.Count; i++)
            {
                int ilinha = shape[i].PosicaoNoDataTable;
                for (int j = 0; j < nomes.Length; j++)
                {
                    PIjm[i, j] = Convert.ToDouble(tabela.Rows[ilinha][nomes[j]]) / Convert.ToDouble(tabela.Rows[ilinha][total]);
                    PIm[j] += Convert.ToDouble(tabela.Rows[ilinha][nomes[j]]);

                }
                T += Convert.ToDouble(tabela.Rows[ilinha][total]);
            }

            for (int i = 0; i < PIm.Length; i++)
            {
                PIm[i] /= T;
                E += PIm[i] * Math.Log(1 / PIm[i]);
            }

            for (int j = 0; j < PIjm.GetLength(0); j++)
            {
                int jlinha = shape[j].PosicaoNoDataTable;
                double tj = Convert.ToDouble(tabela.Rows[jlinha][total]);
                for (int m = 0; m < PIjm.GetLength(1); m++)
                {
                    tmp1 += (tj * PIjm[j, m] * Math.Log(PIjm[j, m] / PIm[m]));
                }
            }

            IT = ((1 / (T * E)) * tmp1);

            return (IT);
        }

        #endregion;


        #region Multigroup Exposition;

        /// <summary>
        /// Retorna o indicador Multigroup "Normalized_Exposure"
        /// </summary>
        /// <param name="shape">Shape com entrada de dados</param>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="nomes">Vetor Com o nome das Variáveis dos Grupos </param>
        /// <param name="total">Variável com o total da população</param>
        /// <returns></returns>
        public double Normalized_Exposure(clsIpeaShape shape, DataTable tabela, string[] nomes, string total)
        {
            double[,] PIjm = new double[tabela.Rows.Count, nomes.Length];
            double[] PIm = new double[nomes.Length];
            double T = 0;
            double E = 0;
            double P = 0;
            double tmp1 = 0;

            for (int i = 0; i < shape.Count; i++)
            {
                int ilinha = shape[i].PosicaoNoDataTable;
                for (int j = 0; j < nomes.Length; j++)
                {
                    PIjm[i, j] = Convert.ToDouble(tabela.Rows[ilinha][nomes[j]]) / Convert.ToDouble(tabela.Rows[ilinha][total]);
                    PIm[j] += Convert.ToDouble(tabela.Rows[ilinha][nomes[j]]);

                }
                T += Convert.ToDouble(tabela.Rows[ilinha][total]);
            }

            for (int i = 0; i < PIm.Length; i++)
            {
                PIm[i] /= T;
            }

            for (int j = 0; j < PIjm.GetLength(0); j++)
            {
                int jlinha = shape[j].PosicaoNoDataTable;
                double tj = Convert.ToDouble(tabela.Rows[jlinha][total]);
                for (int m = 0; m < PIjm.GetLength(1); m++)
                {
                    tmp1 += (tj * (Math.Pow((PIjm[j, m] - PIm[m]), 2) / (1 - PIjm[j, m])));
                }
            }

            P = (tmp1 / T);

            return (P);
        }


        /// <summary>
        /// Retorna o indicador Multigroup "Relative Diversity"
        /// </summary>
        /// <param name="shape">Shape com entrada de dados</param>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="nomes">Vetor Com o nome das Variáveis dos Grupos </param>
        /// <param name="total">Variável com o total da população</param>
        /// <returns></returns>
        public double Relative_Diversity(clsIpeaShape shape, DataTable tabela, string[] nomes, string total)
        {
            double[,] PIjm = new double[tabela.Rows.Count, nomes.Length];
            double[] PIm = new double[nomes.Length];
            double T = 0;
            double I = 0;
            double RD = 0;
            double tmp1 = 0;

            for (int i = 0; i < shape.Count; i++)
            {
                int ilinha = shape[i].PosicaoNoDataTable;
                for (int j = 0; j < nomes.Length; j++)
                {
                    PIjm[i, j] = Convert.ToDouble(tabela.Rows[ilinha][nomes[j]]) / Convert.ToDouble(tabela.Rows[ilinha][total]);
                    PIm[j] += Convert.ToDouble(tabela.Rows[ilinha][nomes[j]]);

                }
                T += Convert.ToDouble(tabela.Rows[ilinha][total]);
            }

            for (int i = 0; i < PIm.Length; i++)
            {
                PIm[i] /= T;
                I += PIm[i] * (1 - PIm[i]);
            }

            for (int j = 0; j < PIjm.GetLength(0); j++)
            {
                int jlinha = shape[j].PosicaoNoDataTable;
                double tj = Convert.ToDouble(tabela.Rows[jlinha][total]);
                for (int m = 0; m < PIjm.GetLength(1); m++)
                {
                    tmp1 += (tj * Math.Pow((PIjm[j, m] - PIm[m]), 2));
                }
            }

            RD = (tmp1 / (T * I));

            return (RD);
        }

        /// <summary>
        /// Retorna o indicador Multigroup "Squared Coefficient of Variation"
        /// </summary>
        /// <param name="shape">Shape com entrada de dados</param>
        /// <param name="tabela">Tabela de dados</param>
        /// <param name="nomes">Vetor Com o nome das Variáveis dos Grupos </param>
        /// <param name="total">Variável com o total da população</param>
        /// <returns></returns>
        public double Squared_Coefficient_of_Variation(clsIpeaShape shape, DataTable tabela, string[] nomes, string total)
        {
            double[,] PIjm = new double[tabela.Rows.Count, nomes.Length];
            double[] PIm = new double[nomes.Length];
            double T = 0;
            double I = 0;
            double C = 0;
            double tmp1 = 0;

            for (int i = 0; i < shape.Count; i++)
            {
                int ilinha = shape[i].PosicaoNoDataTable;
                for (int j = 0; j < nomes.Length; j++)
                {
                    PIjm[i, j] = Convert.ToDouble(tabela.Rows[ilinha][nomes[j]]) / Convert.ToDouble(tabela.Rows[ilinha][total]);
                    PIm[j] += Convert.ToDouble(tabela.Rows[ilinha][nomes[j]]);

                }
                T += Convert.ToDouble(tabela.Rows[ilinha][total]);
            }

            for (int i = 0; i < PIm.Length; i++)
            {
                PIm[i] /= T;
            }

            for (int j = 0; j < PIjm.GetLength(0); j++)
            {
                int jlinha = shape[j].PosicaoNoDataTable;
                double tj = Convert.ToDouble(tabela.Rows[jlinha][total]);
                for (int m = 0; m < PIjm.GetLength(1); m++)
                {
                    tmp1 += ((tj * Math.Pow((PIjm[j, m] - PIm[m]), 2)) / (PIm[m]));
                }
            }

            C = (tmp1 / (T * (PIjm.GetLength(1))));

            return (C);
        }

        #endregion;

        #endregion;

    }
}
