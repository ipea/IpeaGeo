using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace IpeaGEO
{
    class clsIndicesGlobaisDeDependenciaEspacial
    {
        private double EncontraPvalor(double[] rndDados, double dblEstat)
        {
            int i = 0;
            if (dblEstat < 0)
            {
                for (i = rndDados.Length-1; i >=0; i--)
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
                return (Math.Abs( 1.00 - ((double)i / (double)rndDados.Length)));
            }
        }

        private double SomaColuna(double[,] mDados, int coluna)
        {
            double dblSoma = 0;
            for (int k = 0; k < mDados.GetLength(0); k++)
            {
                dblSoma += mDados[k, coluna];
            }
            return (dblSoma);
        }

        /// <summary>
        /// Indice de Geary
        /// </summary>
        /// <param name="dsTable">Base de dados</param>
        /// <param name="strIdMapa">ID no mapa</param>
        /// <param name="mDados">Matriz de dados</param>
        /// <param name="mShape">Estrutura do shape</param>
        /// <returns></returns>
        public double[] IndiceDeGeary(DataTable dsTable, string strIdMapa, double[,] mDados, clsIpeaShape mShape)
        {
            int n = mDados.GetLength(0);
            //Indice global de Geary
            double[] mGeary = new double[mDados.GetLength(1)];

            //Variável

            for (int j = 0; j < mDados.GetLength(1); j++)
            {
                double dblSomaQuadrados = 0;
                double somaPeso = 0;
                //Polígono
                for (int i = 0; i < mDados.GetLength(0); i++)
                {
                    int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);
                    double desvio1 = mDados[i, j];
                    double desvio2 = desvio1 * desvio1;

                    //Vizinho
                    for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                    {
                        double peso = mShape[iMapa].ListaPesosVizinhos[v];
                        int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                        int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                        mGeary[j] += Math.Pow(mDados[i, j] - mDados[iVizinhoBase, j], 2) * peso;
                        somaPeso += peso;
                    }
                    dblSomaQuadrados += desvio2;
                }

                mGeary[j] = ((mDados.GetLength(0)-1) * mGeary[j]) / (dblSomaQuadrados * somaPeso);
            }

            return (mGeary);
        }


        public double[] pValorIndiceDeGeary(DataTable dsTable, string strIdMapa, double[,] mDados, clsIpeaShape mShape, int numSimulacoes, double[] dblGeary, ref ProgressBar pBar)
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
                double[] dblGearySimulados = new double[numSimulacoes];

                for (int s = 0; s < numSimulacoes; s++)
                {
                    int n = mDados.GetLength(0);
                    //Indice global de Geary
                    double[] mGeary = new double[mDados.GetLength(1)];

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
                        int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);

                        double desvio1 = mDados[intDadosTemp[i], j];
                        double desvio2 = desvio1 * desvio1;

                        //Vizinho
                        for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                        {
                            double peso = mShape[iMapa].ListaPesosVizinhos[v];
                            int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                            int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                            mGeary[j] += Math.Pow(desvio1 - mDados[iVizinhoBase, j],2) * peso;
                            somaPeso += peso;

                            //Incrementa a progress bar
                            pBar.Increment(1);
                            Application.DoEvents();
                        }
                        dblSomaQuadrados += desvio2;
                    }
                    dblGearySimulados[s] = ((mDados.GetLength(0) - 1) * mGeary[j]) / (dblSomaQuadrados * somaPeso);
                }
                Array.Sort(dblGearySimulados);
                pValor[j] = EncontraPvalor(dblGearySimulados, dblGeary[j]);
            }
            return (pValor);
        }


        /// <summary>
        /// Indice de Moran GERAL
        /// </summary>
        /// <param name="dsTable">Base de dados</param>
        /// <param name="strIdMapa">ID no mapa</param>
        /// <param name="mDados">Matriz de dados</param>
        /// <param name="mShape">Estrutura do shape</param>
        /// <returns></returns>
        public double[] IndiceDeMoranGlobal(DataTable dsTable, string strIdMapa, double[,] mDados, clsIpeaShape mShape)
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
                        double peso = mShape[iMapa].ListaPesosVizinhos[v];
                        int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                        int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                        mMoran[j] += (desvio1 * (mDados[iVizinhoBase, j] - mMedias[j])) * peso;
                        somaPeso += peso;
                    }
                    dblSomaQuadrados += desvio2;
                }

                mMoran[j] = (mDados.GetLength(0) * mMoran[j]) / (dblSomaQuadrados*somaPeso);
            }

            return (mMoran);
        }



        public double[] pValorIndiceDeMoranGlobal(DataTable dsTable, string strIdMapa, double[,] mDados, clsIpeaShape mShape, int numSimulacoes,double[] dblMoran,ref ProgressBar pBar)
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
                        int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);


                        double desvio1 = mDados[intDadosTemp[i], j] - mMedias[j];
                        double desvio2 = desvio1 * desvio1;

                        //Vizinho
                        for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                        {
                            double peso = mShape[iMapa].ListaPesosVizinhos[v];
                            int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                            int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                            mMoran[j] += (desvio1 * (mDados[iVizinhoBase, j] - mMedias[j])) * peso;
                            somaPeso += peso;

                            //Incrementa a progress bar
                            pBar.Increment(1);
                            Application.DoEvents();
                        }
                        dblSomaQuadrados += desvio2;
                    }
                    dblMoranSimulados[s] = (mDados.GetLength(0) * mMoran[j]) / (dblSomaQuadrados * somaPeso);
                }
                Array.Sort(dblMoranSimulados);
                pValor[j] = EncontraPvalor(dblMoranSimulados, dblMoran[j]);
            }
            return (pValor);
        }

       

        /// <summary>
        /// Indice de Moran simples
        /// </summary>
        /// <param name="dsTable">Base de dados</param>
        /// <param name="strIdMapa">ID no mapa</param>
        /// <param name="mDados">Matriz de dados</param>
        /// <param name="mShape">Estrutura do shape</param>
        /// <returns></returns>
        public double[] IndiceDeMoranGlobalSimples(DataTable dsTable, string strIdMapa, double[,] mDados, clsIpeaShape mShape)
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
                        double peso = 1.0;
                        int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                        int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                        mMoran[j] += (desvio1 * (mDados[iVizinhoBase, j] - mMedias[j])) * peso;
                        somaPeso += peso;
                    }
                    dblSomaQuadrados += desvio2;
                }

                mMoran[j] = (mMoran[j]) / (dblSomaQuadrados);
            }

            return (mMoran);
        }

        public double[] pValorIndiceDeMoranGlobalSimples(DataTable dsTable, string strIdMapa, double[,] mDados, clsIpeaShape mShape, int numSimulacoes, double[] dblMoran, ref ProgressBar pBar)
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
                        int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);


                        double desvio1 = mDados[intDadosTemp[i], j] - mMedias[j];
                        double desvio2 = desvio1 * desvio1;

                        //Vizinho
                        for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                        {
                            double peso = 1.0;
                            int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                            int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                            mMoran[j] += (desvio1 * (mDados[iVizinhoBase, j] - mMedias[j])) * peso;
                            somaPeso += peso;
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

        /// <summary>
        /// Indice Getis Ord Gi Global
        /// </summary>
        /// <param name="dsTable">Base de dados</param>
        /// <param name="strIdMapa">ID no mapa</param>
        /// <param name="mDados">Matriz de dados</param>
        /// <param name="mShape">Estrutura do shape</param>
        /// <returns></returns>
        public double[] IndiceGetisOrdGiGlobal(DataTable dsTable, string strIdMapa, double[,] mDados, clsIpeaShape mShape)
        {
            int n = mDados.GetLength(0);
            //IndiceGetis Ord Gi
            double[] mGetis = new double[mDados.GetLength(1)];

            //Variável
            for (int j = 0; j < mDados.GetLength(1); j++)
            {
                double somaProduto = 0.0;
                //Polígono
                for (int i = 0; i < mDados.GetLength(0); i++)
                {
                    int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);

                    //Vizinho
                    for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                    {
                        double peso = mShape[iMapa].ListaPesosVizinhos[v];
                        int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                        int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                        mGetis[j] += mDados[i,j] * (mDados[iVizinhoBase, j]) * peso;
                        somaProduto += mDados[i, j] * (mDados[iVizinhoBase, j]);
                    }
                }

                mGetis[j] = (mGetis[j]) / (somaProduto);
            }
            return (mGetis);
        }

        public double[] pValorGetisOrdGiGlobal(DataTable dsTable, string strIdMapa, double[,] mDados, clsIpeaShape mShape, int numSimulacoes, double[] dblGetis, ref ProgressBar pBar)
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
                double[] dblGetisSimulados = new double[numSimulacoes];

                for (int s = 0; s < numSimulacoes; s++)
                {
                    int n = mDados.GetLength(0);

                    //Indice Getis Ord Gi
                    double[] mGetis = new double[mDados.GetLength(1)];

                    //Gera o número aleatório
                    double[] rndAleatorio = new double[mDados.GetLength(0)];

                    for (int r = 0; r < rndAleatorio.Length; r++) rndAleatorio[r] = rnd.NextDouble();
                    Array.Sort(rndAleatorio, intDadosTemp);

                    //Variáveis a serem utilizadas
                    double somaProduto = 0;

                    //Polígono
                    for (int i = 0; i < mDados.GetLength(0); i++)
                    {
                        int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);

                        //Vizinho
                        for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                        {
                            double peso = mShape[iMapa].ListaPesosVizinhos[v];
                            int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                            int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                            mGetis[j] += mDados[i, j] * (mDados[iVizinhoBase, j]) * peso;
                            somaProduto += mDados[i, j] * (mDados[iVizinhoBase, j]);

                            //Incrementa a progress bar
                            pBar.Increment(1);
                            Application.DoEvents();
                        }
                    }
                    dblGetisSimulados[s] =(mGetis[j]) / (somaProduto);
                }
                Array.Sort(dblGetisSimulados);
                pValor[j] = EncontraPvalor(dblGetisSimulados, dblGetis[j]);
            }
            return (pValor);
        }


        public double[] IndiceDeTango(DataTable dsTable, string strIdMapa, double[,] mDados, clsIpeaShape mShape)
        {
            int n = mDados.GetLength(0);
            //Indice global de Tango
            double[] mTango = new double[mDados.GetLength(1)-1];

            //Variável

            for (int j = 0; j < mTango.Length; j++)
            {
                //Polígono
                double[] dblEsperado = new double[mDados.GetLength(0)];
                double[] dblObservado = new double[mDados.GetLength(0)];

                double dblSomaCasos = SomaColuna(mDados, j);
                double dblSomaPop = SomaColuna(mDados, mDados.GetLength(1) - 1);

                for (int k = 0; k < dblEsperado.Length; k++)
                {
                    dblObservado[k] = mDados[k, j] / dblSomaPop;
                    dblEsperado[k] = mDados[k, mDados.GetLength(1) - 1] * (dblSomaCasos / (dblSomaPop * dblSomaPop));
                }

                for (int i = 0; i < mDados.GetLength(0); i++)
                {
                    int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);
                    double ri = dblObservado[i];
                    double pi = dblEsperado[i];

                    //Vizinho
                    for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                    {
                        double peso = mShape[iMapa].ListaPesosVizinhos[v];
                        int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                        int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                        double rj = dblObservado[iVizinhoBase];
                        double pj = dblEsperado[iVizinhoBase];
                        mTango[j] += peso*(ri-pi)*(rj-pj);
                    }
                }
            }

            return (mTango);
        }


        public double[] pValorIndiceDeTango(DataTable dsTable, string strIdMapa,double[,] mDados, clsIpeaShape mShape, int numSimulacoes, double[] dblTango, ref ProgressBar pBar)
        {
            double[] pValor = new double[mDados.GetLength(1)-1];
            Random rnd = new Random();
            pBar.Maximum = mDados.GetLength(1) * mDados.GetLength(0);
            //Guarda os dados que serão randomizados
            int[] intDadosTemp = new int[mDados.GetLength(0)];
            for (int r = 0; r < intDadosTemp.Length; r++) intDadosTemp[r] = r;

            //Variável
            for (int j = 0; j < pValor.Length; j++)
            {
                double[] dblTangoSimulados = new double[numSimulacoes];

                //Polígono
                double[] dblEsperado = new double[mDados.GetLength(0)];
                double[] dblObservado = new double[mDados.GetLength(0)];

                double dblSomaCasos = SomaColuna(mDados, j);
                double dblSomaPop = SomaColuna(mDados, mDados.GetLength(1) - 1);

                for (int k = 0; k < dblEsperado.Length; k++)
                {
                    dblObservado[k] = mDados[k, j] / dblSomaPop;
                    dblEsperado[k] = mDados[k, mDados.GetLength(1) - 1] * (dblSomaCasos / (dblSomaPop * dblSomaPop));
                }

                for (int s = 0; s < numSimulacoes; s++)
                {
                    int n = mDados.GetLength(0);
                    //Indice global de Tango
                    double[] mTango = new double[mDados.GetLength(1)-1];

                    //Gera o número aleatório
                    double[] rndAleatorio = new double[mDados.GetLength(0)];

                    for (int r = 0; r < rndAleatorio.Length; r++) rndAleatorio[r] = rnd.NextDouble();
                    Array.Sort(rndAleatorio, intDadosTemp);

                    //Polígono
                    for (int i = 0; i < mDados.GetLength(0); i++)
                    {
                        int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);
                        double ri = dblObservado[intDadosTemp[i]];
                        double pi = dblEsperado[intDadosTemp[i]];

                        //Vizinho
                        for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                        {
                            double peso = mShape[iMapa].ListaPesosVizinhos[v];
                            int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                            int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                            double rj = dblObservado[iVizinhoBase];
                            double pj = dblEsperado[iVizinhoBase];
                            dblTangoSimulados[s] += peso * (ri - pi) * (rj - pj);

                            //Incrementa a progress bar
                            pBar.Increment(1);
                            Application.DoEvents();
                        }
                       }
                }
                Array.Sort(dblTangoSimulados);
                pValor[j] = EncontraPvalor(dblTangoSimulados, dblTango[j]);
            }
            return (pValor);
        }

        public double[] IndiceDeRogerson(DataTable dsTable, string strIdMapa, double[,] mDados, clsIpeaShape mShape)
        {
            int n = mDados.GetLength(0);
            //Indice global de Rogerson
            double[] mRogerson = new double[mDados.GetLength(1)-1];

            //Variável

            for (int j = 0; j < mRogerson.Length; j++)
            {
                //Polígono
                double[] dblEsperado = new double[mDados.GetLength(0)];
                double[] dblObservado = new double[mDados.GetLength(0)];

                double dblSomaCasos = SomaColuna(mDados, j);
                double dblSomaPop = SomaColuna(mDados, mDados.GetLength(1) - 1);

                for (int k = 0; k < dblEsperado.Length; k++)
                {
                    dblObservado[k] = mDados[k, j] / dblSomaPop;
                    dblEsperado[k] = mDados[k, mDados.GetLength(1) - 1] * (dblSomaCasos / (dblSomaPop * dblSomaPop));
                }

                for (int i = 0; i < mDados.GetLength(0); i++)
                {
                    int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);
                    double ri = dblObservado[i];
                    double pi = dblEsperado[i];

                    //Vizinho
                    for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                    {
                        double peso = mShape[iMapa].ListaPesosVizinhos[v];
                        int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                        int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                        double rj = dblObservado[iVizinhoBase];
                        double pj = dblEsperado[iVizinhoBase];
                        mRogerson[j] += (peso * (ri - pi) * (rj - pj))/(Math.Sqrt(pi*pj));
                    }
                }
            }

            return (mRogerson);
        }


        public double[] pValorIndiceDeRogerson(DataTable dsTable, string strIdMapa, double[,] mDados, clsIpeaShape mShape, int numSimulacoes, double[] dblTango, ref ProgressBar pBar)
        {
            double[] pValor = new double[mDados.GetLength(1)-1];
            Random rnd = new Random();
            pBar.Maximum = mDados.GetLength(1) * mDados.GetLength(0);
            //Guarda os dados que serão randomizados
            int[] intDadosTemp = new int[mDados.GetLength(0)];
            for (int r = 0; r < intDadosTemp.Length; r++) intDadosTemp[r] = r;

            //Variável
            for (int j = 0; j < pValor.Length ; j++)
            {
                double[] dblRogersonSimulados = new double[numSimulacoes];

                //Polígono
                double[] dblEsperado = new double[mDados.GetLength(0)];
                double[] dblObservado = new double[mDados.GetLength(0)];

                double dblSomaCasos = SomaColuna(mDados, j);
                double dblSomaPop = SomaColuna(mDados, mDados.GetLength(1) - 1);

                for (int k = 0; k < dblEsperado.Length; k++)
                {
                    dblObservado[k] = mDados[k, j] / dblSomaPop;
                    dblEsperado[k] = mDados[k, mDados.GetLength(1) - 1] * (dblSomaCasos / (dblSomaPop * dblSomaPop));
                }

                for (int s = 0; s < numSimulacoes; s++)
                {
                    int n = mDados.GetLength(0);
                    //Indice global de Rogerson
                    double[] mTango = new double[mDados.GetLength(1)-1];

                    //Gera o número aleatório
                    double[] rndAleatorio = new double[mDados.GetLength(0)];

                    for (int r = 0; r < rndAleatorio.Length; r++) rndAleatorio[r] = rnd.NextDouble();
                    Array.Sort(rndAleatorio, intDadosTemp);

                    //Polígono
                    for (int i = 0; i < mDados.GetLength(0); i++)
                    {
                        int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);
                        double ri = dblObservado[intDadosTemp[i]];
                        double pi = dblEsperado[intDadosTemp[i]];

                        //Vizinho
                        for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                        {
                            double peso = mShape[iMapa].ListaPesosVizinhos[v];
                            int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                            int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                            double rj = dblObservado[iVizinhoBase];
                            double pj = dblEsperado[iVizinhoBase];
                            dblRogersonSimulados[s] += (peso * (ri - pi) * (rj - pj)) / (Math.Sqrt(pi * pj));

                            //Incrementa a progress bar
                            pBar.Increment(1);
                            Application.DoEvents();
                        }
                    }
                }
                Array.Sort(dblRogersonSimulados);
                pValor[j] = EncontraPvalor(dblRogersonSimulados, dblTango[j]);
            }
            return (pValor);
        }

    }
}
