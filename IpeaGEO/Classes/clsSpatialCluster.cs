using System;
using System.Collections;
using System.IO;
using IpeaMatrix;

namespace IpeaGeo
{
    class clsSpatialCluster
    {           
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
     
        Random rnd = new Random(2104);
        
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

            FileInfo t0 = new FileInfo(@"F:\IpeaGEO e Componentes\IpeaGEO1.txt");
            StreamWriter Tex0 = t0.CreateText();
            Tex0.WriteLine("ID_\tCLUSTER");
            for (int i = 0; i < mShapeInicial.Count; i++)
            {
                Tex0.WriteLine(mShapeInicial[i].ID.ToString() + "\t" + mShapeInicial[i].IndiceCluster.ToString());
            }
            Tex0.Close();

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
