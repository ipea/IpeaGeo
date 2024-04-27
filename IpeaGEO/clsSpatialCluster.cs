using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using SharpMap.Geometries;
using SharpMap.Data;
using SharpMap.Data.Providers;
using System.Drawing;
using System.Collections;
using IpeaMatrix;
using System.IO;



namespace IpeaGEO
{
    class clsSpatialCluster
    {

        #region Distância Estatística entre as observações e algoritmo de Clusterização


        /// <summary>
        /// Calcula a distância de Minkowsky
        /// </summary>
        /// <param name="ilambda">Parâmetro Lambda</param>
        /// <param name="dblPesos">Vetor de pesos</param>
        /// <param name="mDados">Matriz de dados</param>
        /// <returns></returns>
        private Matrix DistanciaMinkowsky(int ilambda, double[] dblPesos, Matrix mDados)
        {
            //Normalizando os pesos
            double soma=0;
            for (int i = 0; i < dblPesos.Length; i++) soma += dblPesos[i];
            for (int i = 0; i < dblPesos.Length; i++) dblPesos[i] /= soma;

            //Criando a matriz de distâncias
            Matrix mDistancias = new Matrix(mDados.rows,mDados.rows);

            for (int i = 0; i < mDados.cols-1; i++)
            {
                for (int j = i + 1; j < mDados.cols; j++)
                {
                    for (int p = 0; p < dblPesos.Length; p++) mDistancias[i, j] += dblPesos[p] * Math.Pow(Math.Abs(mDados[i, p] - mDados[j, p]), ilambda);
                    mDistancias[i, j] = Math.Pow(mDistancias[i, j], 1.00 / Convert.ToDouble(ilambda));
                    mDistancias[j, i] = mDistancias[i, j];
                }
            }
            return (mDistancias);
        }

        private void NovaDistanciaAposJuncao(ref Matrix mDistancias,ref ArrayList[] strJuncoes, int A1, int A2, double delta1, double delta2, double delta3, double delta4)
        {
            
            //Criando a matriz de distâncias
            ArrayList[] strJuncoesNova = new ArrayList[mDistancias.rows-1];
            for (int i = 0; i < strJuncoesNova.Length; i++) strJuncoesNova[i] = new ArrayList();

            //Minimo
            int iLinha = Math.Min(A1, A2);
            int iFora = Math.Max(A1, A2);

            int iContadorVetor=0;

            //Passo 1: Clonando a matriz de distâncias
            Matrix mDistanciasNova = mDistancias.Clone();

            //Passo 2: Atualizando a linha e a coluna iLinha
            for (int i = 0; i < mDistancias.rows; i++)
            {
                mDistanciasNova[i, iLinha] = delta1 * mDistancias[i, A1] + delta2 * mDistancias[i, A2] + delta3 * mDistancias[A1, A2] + delta4 * Math.Abs(mDistancias[i, A1] - mDistancias[i, A2]);
                mDistanciasNova[iLinha, i] = mDistanciasNova[i, iLinha];

                //Passo 3: Atualizando o vetor strJuncoes
                if (i == iLinha)
                {
                    strJuncoesNova[iContadorVetor] = strJuncoes[i];
                    for(int k=0;k<strJuncoes[iFora].Count;k++) strJuncoesNova[iContadorVetor].Add((object)strJuncoes[iFora][k]);
                    iContadorVetor++;
                }
                else if(i!=iLinha && i!=iFora)
                {
                   strJuncoesNova[iContadorVetor] = strJuncoes[i];
                   iContadorVetor++;
                }
    
                //FileInfo t = new FileInfo(@"C:\Pedro\IpeaGEO.txt");
                //StreamWriter Tex = t.CreateText();
                //Tex.WriteLine("Contador:\t"+iContadorVetor.ToString());
                //Tex.Close();
            }

            //Passo 4: Retira-se a linha iFora e a coluna iFora da matriz mDistanciasNova
            mDistanciasNova = mDistanciasNova.Remove(iFora, iFora);

            //Vetor antigo fica igual o novo
            strJuncoes = strJuncoesNova;

            //Atualiza a matriz de distâncias
            mDistancias = mDistanciasNova;

        }

        private void NovaDistanciaAposJuncaoAverageLinkageWeighted(ref Matrix mDistancias, ref ArrayList[] strJuncoes, int A1, int A2)
        {
            //Deltas
            double delta1 = double.NaN, delta2 = double.NaN, delta3 = double.NaN, delta4 = double.NaN;

            //Criando a matriz de distâncias
            ArrayList[] strJuncoesNova = new ArrayList[mDistancias.rows - 1];
            for (int i = 0; i < strJuncoesNova.Length; i++) strJuncoesNova[i] = new ArrayList();

            //Minimo
            int iLinha = Math.Min(A1, A2);
            int iFora = Math.Max(A1, A2);

            int iContadorVetor = 0;

            //Passo 1: Clonando a matriz de distâncias
            Matrix mDistanciasNova = mDistancias.Clone();

            //Passo 2: Atualizando a linha e a coluna iLinha
            for (int i = 0; i < mDistancias.rows; i++)
            {
                delta1 = (double)strJuncoes[iLinha].Count / ((double)strJuncoes[iLinha].Count + (double)strJuncoes[iFora].Count);
                delta2 = (double)strJuncoes[iFora].Count / ((double)strJuncoes[iLinha].Count + (double)strJuncoes[iFora].Count);
                delta3 = 0.0;
                delta4 = 0.0;

                mDistanciasNova[i, iLinha] = delta1 * mDistancias[i, iLinha] + delta2 * mDistancias[i, iFora] + delta3 * mDistancias[iLinha,iFora] + delta4 * Math.Abs(mDistancias[i, iLinha] - mDistancias[i, iFora]);
                mDistanciasNova[iLinha, i] = mDistanciasNova[i, iLinha];

                //Passo 3: Atualizando o vetor strJuncoes
                if (i == iLinha)
                {
                    strJuncoesNova[iContadorVetor] = strJuncoes[i];
                    for (int k = 0; k < strJuncoes[iFora].Count; k++) strJuncoesNova[iContadorVetor].Add((object)strJuncoes[iFora][k]);
                    iContadorVetor++;
                }
                else if (i != iLinha && i != iFora)
                {
                    strJuncoesNova[iContadorVetor] = strJuncoes[i];
                    iContadorVetor++;
                }
            }

            //Passo 4: Retira-se a linha iFora e a coluna iFora da matriz mDistanciasNova
            mDistanciasNova = mDistanciasNova.Remove(iFora, iFora);

            //Vetor antigo fica igual o novo
            strJuncoes = strJuncoesNova;

            //Atualiza a matriz de distâncias
            mDistancias = mDistanciasNova;

        }

        private void NovaDistanciaAposJuncaoCentroid(ref Matrix mDistancias, ref ArrayList[] strJuncoes, int A1, int A2)
        {
            //Deltas
            double delta1 = double.NaN, delta2 = double.NaN, delta3 = double.NaN, delta4 = double.NaN;

            //Criando a matriz de distâncias
            ArrayList[] strJuncoesNova = new ArrayList[mDistancias.rows - 1];
            for (int i = 0; i < strJuncoesNova.Length; i++) strJuncoesNova[i] = new ArrayList();

            //Minimo
            int iLinha = Math.Min(A1, A2);
            int iFora = Math.Max(A1, A2);

            int iContadorVetor = 0;

            //Passo 1: Clonando a matriz de distâncias
            Matrix mDistanciasNova = mDistancias.Clone();

            //Passo 2: Atualizando a linha e a coluna iLinha
            for (int i = 0; i < mDistancias.rows; i++)
            {
                double n1 = (double)strJuncoes[iLinha].Count;
                double n2 = (double)strJuncoes[iFora].Count;

                delta1 = n1 / (n1 + n2);
                delta2 = n2 / (n1 + n2);
                delta3 = -((n1*n2)/Math.Pow(n1+n2,2));
                delta4 = 0.0;

                mDistanciasNova[i, iLinha] = delta1 * mDistancias[i, iLinha] + delta2 * mDistancias[i, iFora] + delta3 * mDistancias[iLinha, iFora] + delta4 * Math.Abs(mDistancias[i, iLinha] - mDistancias[i, iFora]);
                mDistanciasNova[iLinha, i] = mDistanciasNova[i, iLinha];

                //Passo 3: Atualizando o vetor strJuncoes
                if (i == iLinha)
                {
                    strJuncoesNova[iContadorVetor] = strJuncoes[i];
                    for (int k = 0; k < strJuncoes[iFora].Count; k++) strJuncoesNova[iContadorVetor].Add((object)strJuncoes[iFora][k]);
                    iContadorVetor++;
                }
                else if (i != iLinha && i != iFora)
                {
                    strJuncoesNova[iContadorVetor] = strJuncoes[i];
                    iContadorVetor++;
                }
            }

            //Passo 4: Retira-se a linha iFora e a coluna iFora da matriz mDistanciasNova
            mDistanciasNova = mDistanciasNova.Remove(iFora, iFora);

            //Vetor antigo fica igual o novo
            strJuncoes = strJuncoesNova;

            //Atualiza a matriz de distâncias
            mDistancias = mDistanciasNova;

        }

        private void NovaDistanciaAposJuncaoWard(ref Matrix mDistancias, ref ArrayList[] strJuncoes, int A1, int A2)
        {
            //Deltas
            double delta1 = double.NaN, delta2 = double.NaN, delta3 = double.NaN, delta4 = double.NaN;

            //Criando a matriz de distâncias
            ArrayList[] strJuncoesNova = new ArrayList[mDistancias.rows - 1];
            for (int i = 0; i < strJuncoesNova.Length; i++) strJuncoesNova[i] = new ArrayList();

            //Minimo
            int iLinha = Math.Min(A1, A2);
            int iFora = Math.Max(A1, A2);

            int iContadorVetor = 0;

            //Passo 1: Clonando a matriz de distâncias
            Matrix mDistanciasNova = mDistancias.Clone();

            //Passo 2: Atualizando a linha e a coluna iLinha
            for (int i = 0; i < mDistancias.rows; i++)
            {
                double n1 = (double)strJuncoes[iLinha].Count;
                double n2 = (double)strJuncoes[iFora].Count;
                double n3 = (double)strJuncoes[i].Count;


                delta1 = (n1 + n3) / (n1 + n2 + n3);
                delta2 = (n2 + n3) / (n1 + n2 + n3);
                delta3 = -((n3) / (n1 + n2 + n3));
                delta4 = 0.0;

                mDistanciasNova[i, iLinha] = delta1 * mDistancias[i, iLinha] + delta2 * mDistancias[i, iFora] + delta3 * mDistancias[iLinha, iFora] + delta4 * Math.Abs(mDistancias[i, iLinha] - mDistancias[i, iFora]);
                mDistanciasNova[iLinha, i] = mDistanciasNova[i, iLinha];

                //Passo 3: Atualizando o vetor strJuncoes
                if (i == iLinha)
                {
                    strJuncoesNova[iContadorVetor] = strJuncoes[i];
                    for (int k = 0; k < strJuncoes[iFora].Count; k++) strJuncoesNova[iContadorVetor].Add((object)strJuncoes[iFora][k]);
                    iContadorVetor++;
                }
                else if (i != iLinha && i != iFora)
                {
                    strJuncoesNova[iContadorVetor] = strJuncoes[i];
                    iContadorVetor++;
                }
            }

            //Passo 4: Retira-se a linha iFora e a coluna iFora da matriz mDistanciasNova
            mDistanciasNova = mDistanciasNova.Remove(iFora, iFora);

            //Vetor antigo fica igual o novo
            strJuncoes = strJuncoesNova;

            //Atualiza a matriz de distâncias
            mDistancias = mDistanciasNova;

        }

        private void ElementosMaisProximos(ref int A1, ref int A2, Matrix mDistancias)
        {
            double minDistancia = double.MaxValue;

            for (int i = 0; i < mDistancias.cols; i++)
            {
                for (int j = i + 1; j < mDistancias.cols; j++)
                {
                    if (mDistancias[i, j] < minDistancia)
                    {
                        minDistancia = mDistancias[i, j];
                        A1 = i;
                        A2 = j;
                    }
                }
            }

        }

        public ArrayList[] GeraConglomerados(int numClusters, int ilambda, double[] dblPesos, Matrix mDados,double delta1, double delta2, double delta3, double delta4)
        {
            //Encontra a matriz de distâncias
            Matrix mDistancias = DistanciaMinkowsky(ilambda, dblPesos, mDados);

            #region Exemplo 11.5 
            /*
            Matrix mDistancias = new Matrix(8, 8);
            for (int i = 0; i < 8; i++) mDistancias[i, i] = 0.0;
            mDistancias[0, 1] = 10;
            mDistancias[1, 0] = 10;

            mDistancias[0, 2] = 53;
            mDistancias[2, 0] = 53;

            mDistancias[0, 3] = 73;
            mDistancias[3, 0] = 73;

            mDistancias[0, 4] = 50;
            mDistancias[4, 0] = 50;

            mDistancias[0, 5] = 98;
            mDistancias[5, 0] = 98;

            mDistancias[0, 6] = 41;
            mDistancias[6, 0] = 41;

            mDistancias[0, 7] = 65;
            mDistancias[7, 0] = 65;

            mDistancias[1, 2] = 25;
            mDistancias[2, 1] = 25;

            mDistancias[1, 3] = 41;
            mDistancias[3, 1] = 41;

            mDistancias[1, 4] = 20;
            mDistancias[4, 1] = 20;

            mDistancias[1, 5] = 80;
            mDistancias[5, 1] = 80;

            mDistancias[1, 6] = 37;
            mDistancias[6, 1] = 37;
            
            mDistancias[1, 7] = 65;
            mDistancias[7, 1] = 65;

            mDistancias[2, 3] = 2;
            mDistancias[3, 2] = 2;

            mDistancias[2, 4] = 1;
            mDistancias[4, 2] = 1;

            mDistancias[2, 5] = 25;
            mDistancias[5, 2] = 25;
            
            mDistancias[2, 6] = 18;
            mDistancias[6, 2] = 18;
            
            mDistancias[2, 7] = 34;
            mDistancias[7, 2] = 34;

            mDistancias[3, 4] = 5;
            mDistancias[4, 3] = 5;

            mDistancias[3, 5] = 17;
            mDistancias[5, 3] = 17;
            
            mDistancias[3, 6] = 20;
            mDistancias[6, 3] = 20;

            mDistancias[3, 7] = 32;
            mDistancias[7, 3] = 32;

            mDistancias[4, 5] = 36;
            mDistancias[5, 4] = 36;

            mDistancias[4, 6] = 25;
            mDistancias[6, 4] = 25;

            mDistancias[4, 7] = 45;
            mDistancias[7, 4] = 45;

            mDistancias[5, 6] = 13;
            mDistancias[6, 5] = 13;

            mDistancias[5, 7] = 9;
            mDistancias[7, 5] = 9;
            
            mDistancias[6, 7] = 4;
            mDistancias[7, 6] = 4;
            */
            #endregion

            //Inicializa o vetor de junções
            ArrayList[] strJuncoes = new ArrayList[mDados.rows];
            for (int i = 0; i < mDistancias.rows; i++)
            {
                strJuncoes[i] = new ArrayList();
                strJuncoes[i].Add((object)i);
            }
            do 
            {
                int A1 = 0;
                int A2 = 0;
                //Encontra os elementos mais próximos
                ElementosMaisProximos(ref A1, ref A2, mDistancias);

                //Coloca esses dois elementos juntos
                NovaDistanciaAposJuncao(ref mDistancias,ref strJuncoes, A1, A2, delta1, delta2, delta3, delta4);

            }while(strJuncoes.Length>numClusters);

            return (strJuncoes);
        }

        public ArrayList[] GeraConglomeradosAverageLinkageWeighted(int numClusters, int ilambda, double[] dblPesos, Matrix mDados)
        {
            //Encontra a matriz de distâncias
            Matrix mDistancias = DistanciaMinkowsky(ilambda, dblPesos, mDados);

            //Inicializa o vetor de junções
            ArrayList[] strJuncoes = new ArrayList[mDados.rows];
            for (int i = 0; i < mDistancias.rows; i++)
            {
                strJuncoes[i] = new ArrayList();
                strJuncoes[i].Add((object)i);
            }
            do
            {
                int A1 = 0;
                int A2 = 0;
                //Encontra os elementos mais próximos
                ElementosMaisProximos(ref A1, ref A2, mDistancias);

                //Coloca esses dois elementos juntos
                NovaDistanciaAposJuncaoAverageLinkageWeighted(ref mDistancias, ref strJuncoes, A1, A2);

            } while (strJuncoes.Length > numClusters);

            return (strJuncoes);
        }

        public ArrayList[] GeraConglomeradosCentroid(int numClusters, int ilambda, double[] dblPesos, Matrix mDados)
        {
            //Encontra a matriz de distâncias
            Matrix mDistancias = DistanciaMinkowsky(ilambda, dblPesos, mDados);

            //Inicializa o vetor de junções
            ArrayList[] strJuncoes = new ArrayList[mDados.rows];
            for (int i = 0; i < mDistancias.rows; i++)
            {
                strJuncoes[i] = new ArrayList();
                strJuncoes[i].Add((object)i);
            }
            do
            {
                int A1 = 0;
                int A2 = 0;
                //Encontra os elementos mais próximos
                ElementosMaisProximos(ref A1, ref A2, mDistancias);

                //Coloca esses dois elementos juntos
                NovaDistanciaAposJuncaoCentroid(ref mDistancias, ref strJuncoes, A1, A2);

            } while (strJuncoes.Length > numClusters);

            return (strJuncoes);
        }

       

        public ArrayList[] GeraConglomeradosWard(int numClusters, int ilambda, double[] dblPesos, Matrix mDados)
        {
            //Encontra a matriz de distâncias
            Matrix mDistancias = DistanciaMinkowsky(ilambda, dblPesos, mDados);

            //Inicializa o vetor de junções
            ArrayList[] strJuncoes = new ArrayList[mDados.rows];
            for (int i = 0; i < mDistancias.rows; i++)
            {
                strJuncoes[i] = new ArrayList();
                strJuncoes[i].Add((object)i);
            }
            do
            {
                int A1 = 0;
                int A2 = 0;
                //Encontra os elementos mais próximos
                ElementosMaisProximos(ref A1, ref A2, mDistancias);

                //Coloca esses dois elementos juntos
                NovaDistanciaAposJuncaoWard(ref mDistancias, ref strJuncoes, A1, A2);

            } while (strJuncoes.Length > numClusters);

            return (strJuncoes);
        }
        #endregion
                
        #region Algoritmo Genético para conglomerados espaciais

        private int NumeroPoligonosSemConglomerados(clsIpeaShape mShape)
        {
            int iConta=0;
            for (int i = 0; i < mShape.Count; i++)
            {
                if (mShape[i].IndiceCluster == -1) iConta++;
            }
            return (iConta);
        }
        private int[] VetorPoligonosSemConglomerados(clsIpeaShape mShape)
        {
            int[] iConta = new int[NumeroPoligonosSemConglomerados(mShape)];
            int iNcremeneto=0;
            for (int i = 0; i < mShape.Count; i++)
            {
                if (mShape[i].IndiceCluster == -1)
                {
                    iConta[iNcremeneto] = i;
                    iNcremeneto++;
                }
            }
            return (iConta);
        }
       

        Random rnd = new Random(2125);
        private void PreencheComConglomerados(ref clsIpeaShape mShape,int iPreenche,int indiceCluster)
        {
            //Escolhe-se um poligono inicial aleatoriamente
            int iInicio = iPreenche;

            //Salva o indice do cluster desse poligono
            mShape[iInicio].IndiceCluster = indiceCluster;

            //Inicializa o contador (1 poligono já foi preenchido)
            int iConta=1;

            //Arraylist dos vizinhos
            ArrayList arVizinhos = new ArrayList();

            //Guarda os vizinhos do primeiro
            int[] iVizinhos = mShape[iInicio].ListaIndicesVizinhos;
            for (int i = 0; i < mShape[iInicio].ListaIndicesVizinhos.Length; i++) if (arVizinhos.Contains(iVizinhos[i]) == false && mShape[iVizinhos[i]].IndiceCluster == -1) arVizinhos.Add(iVizinhos[i]);
            
            //Começa a preencher
            do
            {
                if (arVizinhos.Count > 0)
                {
                    //Sorteia um vizinho para preencher
                    int intEscolhido = rnd.Next(0, arVizinhos.Count);
                    int iEscolhe = (int)arVizinhos[intEscolhido];

                    //Verifica se esse vizinho já foi preenchido
                    if (mShape[iEscolhe].IndiceCluster == -1)
                    {
                        //Salva o indice do cluster desse poligono
                        mShape[iEscolhe].IndiceCluster = indiceCluster;

                        //Adiciona vizinhos
                        int[] iVizinhos2 = mShape[iEscolhe].ListaIndicesVizinhos;
                        for (int i = 0; i < mShape[iEscolhe].ListaIndicesVizinhos.Length; i++) if (arVizinhos.Contains(iVizinhos2[i]) == false && mShape[iVizinhos2[i]].IndiceCluster == -1) arVizinhos.Add(iVizinhos2[i]);

                        //Retira da lista o poligono preenchido
                        arVizinhos.Remove(intEscolhido);

                        //Incrementa o contador
                        iConta++;
                    }
                    else
                    {
                        //Deleta vizinho já preenchido
                        arVizinhos.Remove(iEscolhe);
                    }
                }
            } while (arVizinhos.Count > 0 && iConta < iPreenche);

        }

        public clsIpeaShape PopulacaoInicial(clsIpeaShape mShape, int nClusters)
        {
            clsIpeaShape mShapeInicial = mShape.Clone();

            //Gera mapa inicial
            for (int i = 0; i < nClusters; i++)
            {
                //Número de conglomerados que faltam a ser preenchidos
                int iFaltamASerPreenchidos = NumeroPoligonosSemConglomerados(mShapeInicial)-nClusters;

                //Número de poligonos que deverão ser preenchidos
                int iPreenche = rnd.Next(1, iFaltamASerPreenchidos);

                //Preenche os poligonos
                PreencheComConglomerados(ref mShapeInicial, iPreenche, i);
            }

            //TODO:DELETAR ESSE CODIGO
            FileInfo t0 = new FileInfo(@"F:\IpeaGEO e Componentes\IpeaGEO1.txt");
            StreamWriter Tex0 = t0.CreateText();
            Tex0.WriteLine("ID_\tCLUSTER");
            for (int i = 0; i < mShapeInicial.Count; i++)
            {
                Tex0.WriteLine(mShapeInicial[i].ID.ToString() + "\t" + mShapeInicial[i].IndiceCluster.ToString());
            }
            Tex0.Close();

            //Concerta Brancos
            int[] iBrancos = VetorPoligonosSemConglomerados(mShapeInicial);
            if (iBrancos.Length > 0)
            {
                for (int i = 0; i < iBrancos.Length; i++)
                {
                    //Vizinhos do poligono sem conglomerado
                    int[] iVizinhos = mShapeInicial[iBrancos[i]].ListaIndicesVizinhos;

                    //Indice do vizinho sorteado
                    int iVizinhoAleatorio = rnd.Next(0, iVizinhos.Length);

                    //Iguala ao cluster do vizinho sorteado
                    mShapeInicial[iBrancos[i]].IndiceCluster = mShapeInicial[iVizinhos[iVizinhoAleatorio]].IndiceCluster;
                }
            }

            //TODO:DELETAR ESSE CODIGO
            FileInfo t = new FileInfo(@"F:\IpeaGEO e Componentes\IpeaGEO2.txt");
            StreamWriter Tex = t.CreateText();
            Tex.WriteLine("ID_\tCLUSTER");
            for (int i = 0; i < mShapeInicial.Count; i++)
            {
                Tex.WriteLine(mShapeInicial[i].ID.ToString() + "\t" + mShapeInicial[i].IndiceCluster.ToString());
            }
            Tex.Close();

            return (mShapeInicial);
        }


        #endregion
    }
}
