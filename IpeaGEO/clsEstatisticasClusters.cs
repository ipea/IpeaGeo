using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using IpeaMatrix;

namespace IpeaGEO
{
    class clsEstatisticasClusters
    {
        #region Operadores do Cluster Tree 

        /// <summary>
        /// Identifica se é uma nova classe
        /// </summary>
        /// <param name="iClasse">Número da classe</param>
        /// <param name="arLista">Arraylist de classes</param>
        /// <returns></returns>
        private bool novaClasse(int iClasse, ArrayList arLista)
        {
            for (int i = 0; i < arLista.Count; i++)
            {
                if (Convert.ToInt32(arLista[i]) == iClasse) return (false);
            }
            return (true);
        }

        /// <summary>
        /// Cria novo id para classe
        /// </summary>
        /// <param name="iClasse">Id antigo</param>
        /// <param name="arLista">Array de classes</param>
        /// <returns></returns>
        private int idClasse(int iClasse, ArrayList arLista)
        {
            for (int i = 0; i < arLista.Count; i++)
            {
                if (Convert.ToInt32(arLista[i]) == iClasse) return (i);
            }
            return (-1);
        }

        /// <summary>
        /// Converte uma determinada coluna do Cluster Tree eum um vetor de inetiros com as classes indo de zero a C
        /// </summary>
        /// <param name="mClusterTree">Cluster Tree</param>
        /// <param name="iColuna">Coluna que deseja-se transformar</param>
        /// <returns></returns>
        public int[] ConverteClusterTree(double[,] mClusterTree, int iColuna)
        {
            ArrayList arLista = new ArrayList();
            int[] iSaida = new int[mClusterTree.GetLength(0)];
            for (int i = 0; i < iSaida.Length; i++)
            {
                iSaida[i] = (int)mClusterTree[i, iColuna];
                if (novaClasse(iSaida[i], arLista) == true)
                {
                    arLista.Add(iSaida[i]);
                }
            }
            for (int i = 0; i < iSaida.Length; i++)
            {
                iSaida[i] = idClasse(iSaida[i], arLista);
            }

            return (iSaida);

        }

        private int[] JuncaoDeConglomerados(double[,] mTree, int iColuna)
        {
            int[] iJuncao = new int[2];
            iJuncao[0] = -1;
            iJuncao[1] = -2;

            if (iColuna != mTree.GetLength(1) - 1)
            {
                int iColuna2 = iColuna + 1;
                int[] iCluster1 = ConverteClusterTree(mTree, iColuna);
                int[] iCluster2 = ConverteClusterTree(mTree, iColuna2);

                for (int i = 0; i < iCluster1.Length; i++)
                {
                    if (iCluster1[i] != iCluster2[i])
                    {
                        iJuncao[0] = iCluster1[i];
                        iJuncao[1] = iCluster2[i];
                        break;
                    }
                }
                return (iJuncao);
            }
            return (iJuncao);
        }

        #endregion

        #region Estatísticas para os conglomerados 

        /// <summary>
        /// Calcula a média para cada variável e para cada conglomerado.
        /// </summary>
        /// <param name="mDados">Matriz de dados</param>
        /// <param name="iVetor">Vetor de conglomerados</param>
        /// <param name="nClusters">Número de conglomerados</param>
        /// <returns></returns>
        public double[,] MediaDentroDosConglomerados(double[,] mDados, int[] iVetor, int nClusters)
        {
            double[,] iSoma = new double[nClusters,mDados.GetLength(1)];
            int[] iFrequency = new int[nClusters];

            for (int i = 0; i < iVetor.Length; i++)
            {
                iFrequency[iVetor[i]]++;
                for (int j = 0; j < mDados.GetLength(1); j++)
                {
                    iSoma[iVetor[i],j] += mDados[i, j];
                }
            }

            for (int j = 0; j < mDados.GetLength(1); j++)
            {
                for (int i = 0; i < nClusters; i++)
                {
                    iSoma[i, j] /= (double)iFrequency[i];
                }
            }

            return (iSoma);
        }

        /// <summary>
        /// Calcula a média para cada variável;
        /// </summary>
        /// <param name="mDados">Matriz de dados</param>
        /// <returns></returns>
        public double[] VetorDeMedias(double[,] mDados)
        {
            double[] iSoma = new double[mDados.GetLength(1)];

            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                for (int j = 0; j < mDados.GetLength(1); j++)
                {
                    iSoma[j] += mDados[i, j];
                }
            }

            for (int j = 0; j < mDados.GetLength(1); j++)
            {
                iSoma[j] /= (double) mDados.GetLength(0);
            }

            return (iSoma);
        }

        /// <summary>
        /// Calcula o número de elementos em cada conglomerado
        /// </summary>
        /// <param name="iVetor">Vetor com os conglomerados</param>
        /// <param name="nClusters">Número de conglomerados</param>
        /// <returns></returns>
        public int[] NumeroDeElementosPorConglomerados(int[] iVetor, int nClusters)
        {
            int[] iFrequency = new int[nClusters];
            
            for (int i = 0; i < iVetor.Length; i++)
            {
                iFrequency[iVetor[i]]++;
            }
            return (iFrequency);
        }

        

        /// <summary>
        /// Calcula a soma dos quadrados dentro de cada conglomerado.
        /// </summary>
        /// <param name="mDados">Matriz de dados</param>
        /// <param name="iVetor">Vetor de conglomerados</param>
        /// <param name="nClusters">Número de conglomerados</param>
        /// <returns></returns>
        public double[] SomaDosQuadradosNosConglomerados(double[,] mDados, int[] iVetor, int nClusters)
        {
            double[] iSQ = new double[nClusters];
            double[,] dMedias = new double[nClusters,mDados.GetLength(1)];
            dMedias = MediaDentroDosConglomerados(mDados, iVetor, nClusters);

            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                for (int j = 0; j < mDados.GetLength(1); j++)
                {
                    iSQ[iVetor[i]] += Math.Pow(mDados[i, j] - dMedias[iVetor[i], j], 2);
                }
            }
        
            return (iSQ);
        }

        /// <summary>
        /// Calcula a soma de quadrados Total.
        /// </summary>
        /// <param name="mDados">Matriz de dados</param>
        /// <returns></returns>
        public double SomaDosQuadradosTotal(double[,] mDados)
        {
            double iSQ = 0.0;
            double[] dMedias = new double[mDados.GetLength(1)];
            dMedias=VetorDeMedias(mDados);
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                for (int j = 0; j < mDados.GetLength(1); j++)
                {
                    iSQ += Math.Pow(mDados[i, j] - dMedias[j], 2);
                }
            }

            return (iSQ);
        }

        #endregion

        #region Ferramentas de análise multivariada 

        public double EntropiaParametrica(double[,] mDados)
        {
            clsUtilTools clt = new clsUtilTools();
            double[,] covm = clt.CovSampleMatrix(mDados);
            clt.DecomposicaoLU(ref covm);
            double dblDeterminante =0;
            for (int i = 0; i < covm.GetLength(0); i++)
            {
                dblDeterminante *= covm[i, i];
            }

            return (Math.Log(Math.Sqrt(Math.Pow(2 * Math.PI * Math.E, covm.GetLength(0)) * Math.Abs(dblDeterminante))));
        }


        #endregion

    }
}
