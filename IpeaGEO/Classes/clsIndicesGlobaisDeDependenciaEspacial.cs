using System;
using System.Data;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo
{
    class clsIndicesGlobaisDeDependenciaEspacial
    {
        public double EncontraPvalor(double[] rndDados, double dblEstat)
        {
            int i = 0;
            if (dblEstat < 0)
            {
                for (i = rndDados.Length - 1; i >= 0; i--)
                {
                    if (dblEstat > rndDados[i]) break;
                }
                return (Math.Abs(((double)i / (double)rndDados.Length)));
            }
            else
            {
                for (i = 0; i < rndDados.Length; i++)
                {
                    if (dblEstat < rndDados[i]) break;
                }
                return (Math.Abs(1.00 - ((double)i / (double)rndDados.Length)));
            }
        }

        /// <summary>
        /// Indice de Moran simples
        /// </summary>
        /// <param name="dsTable">Base de dados</param>
        /// <param name="strIdMapa">ID no mapa</param>
        /// <param name="mDados">Matriz de dados</param>
        /// <param name="mShape">Estrutura do shape</param>
        /// <returns></returns>
        public double[] IndiceDeMoranGlobalSimples(DataTable dsTable, string strIdMapa, double[,] mDados, IpeaGeo.RegressoesEspaciais.clsIpeaShape mShape)
        {
            int n = mDados.GetLength(0);
            //Indice global de Moran
            double[] mMoran = new double[mDados.GetLength(1)];
            double[] mMedias = new double[mDados.GetLength(1)];

            //Classe de ferramentas
            clsUtilTools clsUtil = new clsUtilTools();
            mMedias = clsUtil.VetorMedias(mDados);

            //Variável
            for (int j = 0; j < mDados.GetLength(1); j++)
            {
                double dblSomaQuadrados = 0;
                double somaPeso = 0;
                
                //Polígono
                for (int i = 0; i < mDados.GetLength(0); i++)
                {
                    int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);
                    double desvio1 = mDados[i, j] - mMedias[j];
                    double desvio2 = desvio1 * desvio1;

                    //Vizinho
                    for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                    {
                        if (mShape[iMapa].NumeroVizinhos < mShape.Count - 2)
                        {
                            double peso = 1.0;
                            int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                            int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                            mMoran[j] += (desvio1 * (mDados[iVizinhoBase, j] - mMedias[j])) * peso;
                            somaPeso += peso;
                        }
                    }
                    dblSomaQuadrados += desvio2;
                }

                mMoran[j] = (mMoran[j]) / (dblSomaQuadrados);
            }

            return (mMoran);
        }

        public double[] pValorIndiceDeMoranGlobalSimples(DataTable dsTable, string strIdMapa, double[,] mDados, IpeaGeo.RegressoesEspaciais.clsIpeaShape mShape, 
            int numSimulacoes, double[] dblMoran, ref ProgressBar pBar)
        {
            double[] pValor = new double[mDados.GetLength(1)];
            Random rnd = new Random();
            pBar.Maximum = mDados.GetLength(1) * mDados.GetLength(0);
            
            //Guarda os dados que serão randomizados
            int[] intDadosTemp = new int[mDados.GetLength(0)];
            for (int r = 0; r < intDadosTemp.Length; r++) intDadosTemp[r] = r;

            //Variável
            for (int j = 0; j < mDados.GetLength(1); j++)
            {
                double[] dblMoranSimulados = new double[numSimulacoes];

                for (int s = 0; s < numSimulacoes; s++)
                {
                    int n = mDados.GetLength(0);
                    //Indice global de Moran
                    double[] mMoran = new double[mDados.GetLength(1)];
                    double[] mMedias = new double[mDados.GetLength(1)];

                    //Classe de ferramentas
                    clsUtilTools clsUtil = new clsUtilTools();
                    mMedias = clsUtil.VetorMedias(mDados);

                    //Gera o número aleatório
                    double[] rndAleatorio = new double[mDados.GetLength(0)];

                    for (int r = 0; r < rndAleatorio.Length; r++) rndAleatorio[r] = rnd.NextDouble();
                    Array.Sort(rndAleatorio, intDadosTemp);
                    
                    //Variáveis a serem utilizadas
                    double dblSomaQuadrados = 0;
                    double somaPeso = 0;
                    //Polígono
                    for (int i = 0; i < mDados.GetLength(0); i++)
                    {
                        int iMapa = Convert.ToInt32(dsTable.Rows[intDadosTemp[i]]["Mapa" + strIdMapa]);

                        double desvio1 = mDados[intDadosTemp[i], j] - mMedias[j];
                        double desvio2 = desvio1 * desvio1;

                        //Vizinho
                        for (int v = 0; v < mShape[i].NumeroVizinhos; v++)
                        {
                            if (mShape[iMapa].NumeroVizinhos < mShape.Count - 2)
                            {
                                double peso = 1.0;
                                int iVizinhoMapa = mShape[i].ListaIndicesVizinhos[v];
                                int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                                mMoran[j] += (desvio1 * (mDados[iVizinhoBase, j] - mMedias[j])) * peso;
                                somaPeso += peso;
                            }
                            
                            //Incrementa a progress bar
                            pBar.Increment(1);
                            Application.DoEvents();
                        }
                        dblSomaQuadrados += desvio2;
                    }
                    dblMoranSimulados[s] = (mMoran[j]) / (dblSomaQuadrados);
                }
                Array.Sort(dblMoranSimulados);
                pValor[j] = EncontraPvalor(dblMoranSimulados, dblMoran[j]);
            }
            return (pValor);
        }
    }
}
