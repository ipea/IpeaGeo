using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Collections;

namespace IpeaGEO
{
    class clsIndicesLocaisDeDependenciaEspacial
    {

        const double OneOverRootTwoPi = 0.398942280401433;

        // probability density for a standard Gaussian distribution
        private double NormalDensity(double x)
        {
            return OneOverRootTwoPi * Math.Exp(-x * x / 2);
        }

        // the InverseCumulativeNormal function via the Beasley-Springer/Moro approximation
        private double InverseCumulativeNormal(double u)
        {


            double[] a = new double[4]{2.50662823884,-18.61500062529,41.39119773534,-25.44106049637};

            double[] b =new double[4]{-8.47351093090,23.08336743743,-21.06224101826,3.13082909833};

            double[] c = new double[9]{0.3374754822726147,0.9761690190917186,0.1607979714918209,0.0276438810333863,0.0038405729373609,0.0003951896511919,0.0000321767881768,0.0000002888167364,0.0000003960315187};


            
            double x=u-0.5;
            double r;

            if (Math.Abs(x)<0.42) // Beasley-Springer
            {
                double y=x*x;
                
                r=x*(((a[3]*y+a[2])*y+a[1])*y+a[0])/
                        ((((b[3]*y+b[2])*y+b[1])*y+b[0])*y+1.0);
                       
            }
            else // Moro
            {
            
                r=u;
            
                if (x>0.0) 
                    r=1.0-u;
                
                r=Math.Log(-Math.Log(r));
                
                r=c[0]+r*(c[1]+r*(c[2]+r*(c[3]+r*(c[4]+r*(c[5]+r*(c[6]+
                        r*(c[7]+r*c[8])))))));
                
                if (x<0.0) 
                    r=-r;
            
            }

            return r;
        }


        // standard normal cumulative distribution function
        private double CumulativeNormal(double x)
        {
            double[] a = new double[5] { 0.319381530, -0.356563782, 1.781477937, -1.821255978, 1.330274429 };

            double result;

            if (x < -7.0)
                result = NormalDensity(x) / Math.Sqrt(1.0 + x * x);

            else
            {
                if (x > 7.0)
                    result = 1.0 - CumulativeNormal(-x);
                else
                {
                    double tmp = 1.0 / (1.0 + 0.2316419 * Math.Abs(x));

                    result = 1 - NormalDensity(x) *
                             (tmp * (a[0] + tmp * (a[1] + tmp * (a[2] + tmp * (a[3] + tmp * a[4])))));

                    if (x <= 0.0)
                        result = 1.0 - result;

                }
            }

            return result;
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

        private double Variancia(double[,] mDados,double dblMedia ,int iColuna)
        {
            double dblVariancia=0;
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                dblVariancia += Math.Pow(mDados[i, iColuna] - dblMedia, 2);
            }
            return (dblVariancia / (mDados.GetLength(0) - 1.0));
        }

        private double Significancia(double mValor)
        {
            double dblSig = 0;
            if (mValor < 0)
            {
                 dblSig = CumulativeNormal(mValor);
            }
            else
            {
                dblSig = 1 - CumulativeNormal(mValor);
            }
            return (dblSig);
        }

        /// <summary>
        /// LISA
        /// </summary>
        /// <param name="dsTable">Base de dados</param>
        /// <param name="strIdMapa">ID do MAPA</param>
        /// <param name="mDados">Matriz de dados</param>
        /// <param name="strVariavel">Nome da variável</param>
        /// <param name="iVariavel">Posição da variável na matriz mDados</param>
        /// <param name="mShape">Estrutura do Shape</param>
        /// <param name="dblSignificancia">Sigificância</param>
        /// <param name="arEspalhamento">Array onde será adicionado o espalhamento</param>
        /// <param name="arPintaMapa">Array com os vetores INT para pintar o mapa</param>
        public void LISA(ref DataTable dsTable, string strIdMapa, double[,] mDados,string strVariavel,int iVariavel, clsIpeaShape mShape,double dblSignificancia, ref ArrayList arEspalhamento,ref ArrayList arPintaMapa,ref ProgressBar pbBar,ref Label lblStatus)
        {
            pbBar.Value = 0;
            pbBar.Maximum=mDados.GetLength(0);
            pbBar.Refresh();
            Application.DoEvents();
            

            //Adicionando as novas variáveis a base
            string nome1 = strVariavel + "_LI";
            string nome2 = strVariavel + "_LS";
            string nome3 = strVariavel + "_LC";
            string nome4 = strVariavel + "_LN";
            if (dsTable.Columns.Contains(nome4) == false) dsTable.Columns.Add(nome4, Type.GetType("System.String"));
            if (dsTable.Columns.Contains(nome1) == false) dsTable.Columns.Add(nome1, Type.GetType("System.String"));
            if (dsTable.Columns.Contains(nome2) == false) dsTable.Columns.Add(nome2, Type.GetType("System.String"));
            if (dsTable.Columns.Contains(nome3) == false) dsTable.Columns.Add(nome3,Type.GetType("System.String"));

            //Encontra a média
            double dblMedia = SomaColuna(mDados, iVariavel) / mDados.GetLength(0);
            double dblVariancia =Variancia(mDados,dblMedia,iVariavel);


            //Encontra soma dos quadrados dos desvios
            double dblZ2 = 0;
            double dblZ4 = 0;

            double[] dblValor = new double[mDados.GetLength(0)];
            for(int i=0;i<mDados.GetLength(0);i++) 
            {
                dblZ2 += Math.Pow((mDados[i, iVariavel] - dblMedia), 2);
                dblZ4 += Math.Pow((mDados[i, iVariavel] - dblMedia), 4);
                dblValor[i] = (mDados[i, iVariavel] - dblMedia);
            }
            double m2 = dblZ2 / mDados.GetLength(0);
            double m4 = dblZ4 / mDados.GetLength(0);


            //Criando o espalhamento e o LISA
            lblStatus.Text = "Gerando o índice de dependência local LISA...";
            double[] dblVizinhos = new double[mDados.GetLength(0)];
            double[] dblLISA = new double[mDados.GetLength(0)];
            double[] dblLISAs = new double[mDados.GetLength(0)];
            double[] dblLISAn = new double[mDados.GetLength(0)];

            int[] iPinta = new int[mDados.GetLength(0)];
            
            double[,] dblEspalhamento= new double[mDados.GetLength(0),2];

            for (int i = 0; i < dblValor.Length; i++)
            {

                int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);
                double dblSomaPeso = 0;
                double dblSomaPeso2 = 0;
                double dblProdutoEntreVizinhos = 0;
                //Vizinho
                for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                {
                    double peso = mShape[iMapa].ListaPesosVizinhos[v];
                    int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                    int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                    dblLISA[i] +=  dblValor[iVizinhoBase]* peso;
                    dblSomaPeso += peso;
                    dblSomaPeso2 += peso*peso;
                    for (int v2 = v + 1; v2 < mShape[iMapa].NumeroVizinhos - 1; v2++) dblProdutoEntreVizinhos += peso * mShape[iMapa].ListaPesosVizinhos[v2];


                }
                dblVizinhos[i] = dblLISA[i];
                dblEspalhamento[i, 0] = dblValor[i] / Math.Sqrt(dblVariancia);
                dblEspalhamento[i, 1] = dblVizinhos[i] / Math.Sqrt(dblVariancia);
                
                double n=mDados.GetLength(0);
                dblLISA[i]*=(mDados.GetLength(0)*dblValor[i]/dblZ2);
                double dblLisaE=-dblSomaPeso/(n-1);
                double b2=(m4/(m2*m2));
                double dblLisaV = ((dblSomaPeso2 * (n - b2)) / (n - 1)) + (2 * dblProdutoEntreVizinhos * ((2 * b2 - n) / ((n - 1)*(n-2)))) - (dblLisaE * dblLisaE);


                dblLISAn[i] = (dblLISA[i] - dblLisaE) / (Math.Sqrt(dblLisaV));
                dblLISAs[i] = Significancia(dblLISAn[i]);

                //Guarda os valores na base de dados
                dsTable.Rows[i][nome1] = dblLISA[i].ToString();
                dsTable.Rows[i][nome2] = dblLISAs[i].ToString();
                dsTable.Rows[i][nome4] = dblLISAn[i].ToString();
                if(dblValor[i]>0 && dblVizinhos[i]>0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Alto";
                    if (dblLISAs[i] <= dblSignificancia)
                    {
                        iPinta[iMapa] = 1;
                    }
                }
                else if(dblValor[i]>0 && dblVizinhos[i]<0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Baixo";
                    if(dblLISAs[i]<=dblSignificancia)
                    {
                        iPinta[iMapa] = 2;
                    }
                }
                else if(dblValor[i]<0 && dblVizinhos[i]<0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Baixo";
                    if(dblLISAs[i]<=dblSignificancia)
                    {
                        iPinta[iMapa] = 3;
                    }
                }
                else if (dblValor[i] < 0 && dblVizinhos[i] > 0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Alto";
                    if(dblLISAs[i]<=dblSignificancia)
                    {
                        iPinta[iMapa] = 4;
                    }
                }
                else 
                {
                    dsTable.Rows[i][nome3] = "NaN";
                    iPinta[iMapa] = 0;
                }

                pbBar.Increment(1);
            }
            arEspalhamento.Add(dblEspalhamento);
            arPintaMapa.Add(iPinta);
            lblStatus.Text = "";
            Application.DoEvents();
        }

        /// <summary>
        /// Getis-Ord Gi
        /// </summary>
        /// <param name="dsTable">Base de dados</param>
        /// <param name="strIdMapa">ID do MAPA</param>
        /// <param name="mDados">Matriz de dados</param>
        /// <param name="strVariavel">Nome da variável</param>
        /// <param name="iVariavel">Posição da variável na matriz mDados</param>
        /// <param name="mShape">Estrutura do Shape</param>
        /// <param name="dblSignificancia">Sigificância</param>
        /// <param name="arEspalhamento">Array onde será adicionado o espalhamento</param>
        /// <param name="arPintaMapa">Array com os vetores INT para pintar o mapa</param>
        public void Getis_Ord_Gi(ref DataTable dsTable, string strIdMapa, double[,] mDados, string strVariavel, int iVariavel, clsIpeaShape mShape, double dblSignificancia, ref ArrayList arPintaMapa, ref ProgressBar pbBar, ref Label lblStatus)
        {
            pbBar.Value = 0;
            pbBar.Maximum = mDados.GetLength(0);
            pbBar.Refresh();
            Application.DoEvents();



            //Adicionando as novas variáveis a base
            string nome1 = strVariavel + "_GI";
            string nome2 = strVariavel + "_GS";
            string nome3 = strVariavel + "_GC";
            if (dsTable.Columns.Contains(nome1) == false) dsTable.Columns.Add(nome1, Type.GetType("System.String"));
            if (dsTable.Columns.Contains(nome2) == false) dsTable.Columns.Add(nome2, Type.GetType("System.String"));
            if (dsTable.Columns.Contains(nome3) == false) dsTable.Columns.Add(nome3, Type.GetType("System.String"));

            //Encontra a média
            double dblMedia = SomaColuna(mDados, iVariavel) / mDados.GetLength(0);
            double dblVariancia = Variancia(mDados, dblMedia, iVariavel);

            //Padroniza os dados
            double[] dblValor = new double[mDados.GetLength(0)];
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                dblValor[i] = (mDados[i, iVariavel] - dblMedia);
            }

            //Criando o espalhamento e o Getis-Ord Gi
            lblStatus.Text = "Gerando o índice de dependência local Getis-Ord Gi*...";
            double[] dblVizinhos = new double[mDados.GetLength(0)];
            double[] dblGI = new double[mDados.GetLength(0)];
            double[] dblGIs = new double[mDados.GetLength(0)];
            int[] iPinta = new int[mDados.GetLength(0)];
            double[,] dblEspalhamento = new double[mDados.GetLength(0), 2];

            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                double somaPeso = 0;
                double somaPeso2 = 0;
                int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);

                //Vizinho
                for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                {
                    double peso = mShape[iMapa].ListaPesosVizinhos[v];
                    int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                    int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                    dblGI[i] += mDados[iVizinhoBase, iVariavel] * peso;
                    somaPeso += peso;
                    somaPeso2 += peso * peso;
                    dblVizinhos[i] += peso * dblValor[iVizinhoBase];
                }
                
                dblGI[i] = (dblGI[i] - (dblMedia * somaPeso)) / Math.Sqrt(dblVariancia * (((mDados.GetLength(0) * somaPeso2) - (somaPeso * somaPeso)) / (mDados.GetLength(0) - 1.0)));
                dblGIs[i] = Significancia(dblGI[i]);
                //Guarda os valores na base de dados
                dsTable.Rows[i][nome1] = dblGI[i].ToString();
                dsTable.Rows[i][nome2] = dblGIs[i].ToString();
                if (dblValor[i] > 0 && dblVizinhos[i] > 0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Alto";
                    if (dblGIs[i] <= dblSignificancia) iPinta[iMapa] = 1;
                }
                else if (dblValor[i] > 0 && dblVizinhos[i] < 0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Baixo";
                    if (dblGIs[i] <= dblSignificancia) iPinta[iMapa] = 2;
                }
                else if (dblValor[i] < 0 && dblVizinhos[i] < 0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Baixo";
                    if (dblGIs[i] <= dblSignificancia) iPinta[iMapa] = 3;
                }
                else if (dblValor[i] < 0 && dblVizinhos[i] > 0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Alto";
                    if(dblGIs[i]<=dblSignificancia)
                    {
                        iPinta[iMapa] = 4;
                    }
                }
                else 
                {
                    dsTable.Rows[i][nome3] = "NaN";
                    iPinta[iMapa] = 0;
                }

                pbBar.Increment(1);
            }
            arPintaMapa.Add(iPinta);
            lblStatus.Text = "";
            Application.DoEvents();
        }



        /// <summary>
        /// Escore
        /// </summary>
        /// <param name="dsTable">Base de dados</param>
        /// <param name="strIdMapa">ID do MAPA</param>
        /// <param name="mDados">Matriz de dados</param>
        /// <param name="strVariavel">Nome da variável</param>
        /// <param name="iVariavel">Posição da variável na matriz mDados</param>
        /// <param name="mShape">Estrutura do Shape</param>
        /// <param name="dblSignificancia">Sigificância</param>
        /// <param name="arEspalhamento">Array onde será adicionado o espalhamento</param>
        /// <param name="arPintaMapa">Array com os vetores INT para pintar o mapa</param>
        public void Escore(ref DataTable dsTable, string strIdMapa, double[,] mDados, string strVariavel, int iVariavel,clsIpeaShape mShape, double dblSignificancia, ref ArrayList arPintaMapa, ref ProgressBar pbBar, ref Label lblStatus)
        {
            pbBar.Value = 0;
            pbBar.Maximum = mDados.GetLength(0);
            pbBar.Refresh();
            Application.DoEvents();


            //Adicionando as novas variáveis a base
            string nome1 = strVariavel + "_EI";
            string nome2 = strVariavel + "_ES";
            string nome3 = strVariavel + "_EC";
            string nome4 = strVariavel + "_EN";
            if (dsTable.Columns.Contains(nome1) == false) dsTable.Columns.Add(nome1, Type.GetType("System.String"));
            if (dsTable.Columns.Contains(nome4) == false) dsTable.Columns.Add(nome4, Type.GetType("System.String"));
            if (dsTable.Columns.Contains(nome2) == false) dsTable.Columns.Add(nome2, Type.GetType("System.String"));
            if (dsTable.Columns.Contains(nome3) == false) dsTable.Columns.Add(nome3, Type.GetType("System.String"));

            double[] dblObservado = new double[mDados.GetLength(0)];
            double[] dblEsperado = new double[mDados.GetLength(0)];

            double dblSomaCasos = SomaColuna(mDados, iVariavel);
            double dblSomaPop = SomaColuna(mDados, mDados.GetLength(1) - 1);

            //Encontra a média
            double dblMedia = SomaColuna(mDados, iVariavel) / mDados.GetLength(0);

            //Padroniza os dados
            double[] dblValor = new double[mDados.GetLength(0)];
            for (int k = 0; k < dblEsperado.Length; k++)
            {
                dblObservado[k] = mDados[k, iVariavel] / dblSomaPop;
                dblEsperado[k] = mDados[k, mDados.GetLength(1) - 1] * (dblSomaCasos / (dblSomaPop * dblSomaPop));
                dblValor[k] = (mDados[k, iVariavel] - dblMedia);
            }

            //Criando o espalhamento e o Getis-Ord Gi
            lblStatus.Text = "Gerando o índice de dependência local Escore...";
            double[] dblVizinhos = new double[mDados.GetLength(0)];
            double[] dblESCORE = new double[mDados.GetLength(0)];
            double[] dblESCOREn = new double[mDados.GetLength(0)];
            double[] dblESCOREv = new double[mDados.GetLength(0)];
            double[] dblESCOREs = new double[mDados.GetLength(0)];

            int[] iPinta = new int[mDados.GetLength(0)];

            double[,] dblEspalhamento = new double[mDados.GetLength(0), 2];
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);
                double dblVariancia1 = 0;
                double dblVariancia2 = 0;

                //Vizinho
                for (int v = 0; v < mShape[iMapa].NumeroVizinhos; v++)
                {
                    double peso = mShape[iMapa].ListaPesosVizinhos[v];
                    int iVizinhoMapa = mShape[iMapa].ListaIndicesVizinhos[v];
                    int iVizinhoBase = mShape[iVizinhoMapa].PosicaoNoDataTable;
                    dblESCORE[i] += (dblObservado[iVizinhoBase]-dblEsperado[iVizinhoBase]) * peso;
                    dblVariancia1 += peso * peso * dblEsperado[iVizinhoBase];
                    dblVariancia2 += peso* dblEsperado[iVizinhoBase];
                    dblVizinhos[i] += dblValor[iVizinhoBase] * peso;
                }
                dblESCORE[i] *= dblESCORE.Length;
                dblESCOREv[i] = dblESCORE.Length * (dblVariancia1 + (dblVariancia2 * dblVariancia2));
                dblESCOREn[i] = dblESCORE[i] / Math.Sqrt(dblESCOREv[i]);

                dblESCOREs[i] = Significancia(dblESCOREn[i]);
                //Guarda os valores na base de dados
                dsTable.Rows[i][nome1] = dblESCORE[i].ToString();
                dsTable.Rows[i][nome2] = dblESCOREs[i].ToString();
                dsTable.Rows[i][nome4] = dblESCOREv[i].ToString();
                if (dblValor[i] > 0 && dblVizinhos[i] > 0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Alto";
                    if (dblESCOREs[i] <= dblSignificancia) iPinta[iMapa] = 1;
                }
                else if (dblValor[i] > 0 && dblVizinhos[i] < 0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Baixo";
                    if (dblESCOREs[i] <= dblSignificancia) iPinta[iMapa] = 2;
                }
                else if (dblValor[i] < 0 && dblVizinhos[i] < 0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Baixo";
                    if (dblESCOREs[i] <= dblSignificancia) iPinta[iMapa] = 3;
                }
                else if (dblValor[i] < 0 && dblVizinhos[i] > 0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Alto";
                    if (dblESCOREs[i] <= dblSignificancia)
                    {
                        iPinta[iMapa] = 4;
                    }
                }
                else
                {
                    dsTable.Rows[iMapa][nome3] = "NaN";
                    iPinta[i] = 0;
                }
                pbBar.Increment(1);
            }
            arPintaMapa.Add(iPinta);
            lblStatus.Text = "";
            Application.DoEvents();
        }

    }
}
