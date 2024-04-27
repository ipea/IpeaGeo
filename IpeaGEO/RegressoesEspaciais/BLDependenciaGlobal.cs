using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data;
using System.Windows.Forms;
//using System.IO;

namespace IpeaGeo.RegressoesEspaciais
{
    public class BLDependenciaGlobal : clsLinearRegressionModelsMLE
    {
        protected clsMatrizEsparsa m_W_esparsacomp = new clsMatrizEsparsa();
        public BLDependenciaGlobal()
        {
        }

        public BLDependenciaGlobal(clsIpeaShape shape, TipoMatrizVizinhanca tipo_matriz)
        {
            this.TipoMatrizVizinhanca = tipo_matriz;
            this.Shape = shape;

            if (m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Normalizada)
            {
                this.MatrizWesparsaFromVizinhosNorm();
            }

            if (m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Original)
            {
                this.MatrizWesparsaFromVizinhos();
            }
        }

        public BLDependenciaGlobal(clsMatrizEsparsa matriz_W)
        {
            m_W_esparsa = matriz_W;
        }

        public double[] TesteMoranGlobal(double[,] y, ref ProgressBar pbar)
        {
            //Elementos do vetor de resultado:
            //Primeiro: esperança do Indice de Moran Global
            //Segundo: variância do Índice de Moran Global
            //Terceiro: valor da estatística de teste que possui uma dist normal padrão
            //Quarto: p-valor do Teste de Moran Bilateral segundo uma distribuição Normal

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();

            clsMatrizEsparsa M;
            if (m_W_esparsa.FormatoMatrizEsparsa == TipoMatrizEsparsa.TripletForm)
                M = m_W_esparsa.Clone();
            else
                M = fme.CompressColumn2TripletForm(m_W_esparsa);

            double[] res = new double[4];
            double moran = 0;
            moran = IndiceMoranGlobal(y);
            res[0] = -1 / (double)(y.GetLength(0) - 1);

            double s1 = 0;
            double s0 = 0;
            double[] somaLinhas = new double[y.GetLength(0)];
            double[] somaColunas = new double[y.GetLength(0)];
            
            int w = M.nzmax;
            int i1 = 0;
            int j1 = 0;
            //double wij = new double();
            //double wji = new double();

            // inclusão1 gabriela 21 de outubro de 2011
            pbar.Maximum = w;
            // fim inclusão1 gabriela 21 de outubro de 2011

            for (int i = 0; i < w; i++)
            {
                // inclusão2 gabriela 21 de outubro de 2011
                //Incrementa a progress 9bar
                pbar.Increment(1);
                Application.DoEvents();
                // fim inclusão2 gabriela 21 de outubro de 2011

                i1 = M.row_indices[i];
                j1 = M.col_indices[i];
               
                somaLinhas[M.row_indices[i]] = somaLinhas[M.row_indices[i]] + M.x[i];
                somaColunas[M.col_indices[i]] = somaColunas[M.col_indices[i]] + M.x[i];
                s0 += M.x[i];
            }

            clsMatrizEsparsa WW = (fme.MatrizSoma(fme.MatrizTransp(m_W_esparsa), m_W_esparsa, 1.0, 1.0)).LimparElementosZero();
            clsMatrizEsparsa WWT = fme.CompressColumn2TripletForm(WW);
            double[] elementos_x = WW.x;
            for (int i = 0; i < elementos_x.GetLength(0); i++)
            {
                s1 += Math.Pow(elementos_x[i], 2.0);
            }
            s1 = s1 / 2.0;

            // tentativa - fim 


            double s2 = 0;
            for (int i = 0; i < y.GetLength(0); i++)
            {
                s2 = s2 + Math.Pow(somaLinhas[i] + somaColunas[i], 2);
            }

            double varN = (((Math.Pow(y.GetLength(0), 2)) * s1) - (y.GetLength(0) * s2) + (3 * (Math.Pow(s0, 2)))) / (Math.Pow(s0, 2) * (Math.Pow(y.GetLength(0), 2) - 1));
            res[1] = varN - Math.Pow(res[0], 2);
            res[2] = (moran - res[0]) / (Math.Sqrt(res[1]));

            MathNormaldist normdist = new MathNormaldist(0, 1);

            if (res[2] > 0)
                res[3] = 2 * (1 - normdist.cdf(res[2]));
            else
                res[3] = 2 * (normdist.cdf(res[2]));

            return res;
        }

        public double IndiceMoranGlobal(double[,] y)
        {
            double res = 0.0;

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();

            double[,] z = new double[y.GetLength(0), 1];
            double media_y = clt.Mean(y);
            for (int i = 0; i < z.GetLength(0); i++)
            {
                z[i, 0] = y[i, 0] - media_y;
            }

            double den1 = (clt.MatrizMult(clt.MatrizTransp(z), z))[0, 0];

            double[,] aux1 = fme.MatrizMult(m_W_esparsa, z);
            double num1 = (clt.MatrizMult(clt.MatrizTransp(z), aux1))[0, 0];

            res = num1 / den1;

            return res;
        }

        public double IndiceMoranGeralPValor(double[,] coluna, int n_simulacoes, double Moran_hat, ref ProgressBar pbar)
        {
            clsUtilTools clt = new clsUtilTools();
            Troschuetz.Random.ContinuousUniformDistribution ranuni = new Troschuetz.Random.ContinuousUniformDistribution();

            // inclusão1 gabriela 21 de outubro de 2011
            pbar.Maximum = n_simulacoes;
            // fim inclusão1 gabriela 21 de outubro de 2011

            object[,] y = new object[coluna.GetLength(0), 1];
            object[,] u = new object[y.GetLength(0), 1];
            object[,] x = new object[y.GetLength(0), 2];
            double[,] xd = new double[y.GetLength(0), 1];

            double[,] amostra = new double[n_simulacoes, 1];

            for (int i = 0; i < y.GetLength(0); i++)
            {
                y[i, 0] = coluna[i, 0];
            }

            for (int k = 0; k < n_simulacoes; k++)
            {
                // inclusão2 gabriela 21 de outubro de 2011
                //Incrementa a progress 9bar
                pbar.Increment(1);
                Application.DoEvents();
                // fim inclusão2 gabriela 21 de outubro de 2011

                for (int i = 0; i < u.GetLength(0); i++)
                {
                    u[i, 0] = ranuni.NextDouble();
                }
                x = clt.Concateh(u, y);
                clt.SortByColumn(ref x, x, 0);

                for (int i = 0; i < xd.GetLength(0); i++)
                {
                    xd[i, 0] = Convert.ToDouble(x[i, 1]);
                }

                amostra[k, 0] = this.IndiceMoranGlobal(xd);
            }

            double[] rndDados = new double[amostra.GetLength(0)];
            for (int i = 0; i < rndDados.GetLength(0); i++)
            {
                rndDados[i] = amostra[i, 0];
            }
            Array.Sort(rndDados);

            double pvalor = EncontraPvalor(rndDados, Moran_hat);

            return pvalor;
        }

        // Modificado por Gabriela 25 de novembro, pois o formato anterior só valia
        // para os índices simétricos em torno de zero.
        private double EncontraPvalor(double[] rndDados, double dblEstat)
        {
            int i = 0;
            for (i = 0; i < rndDados.Length; i++)
            {
                if (dblEstat < rndDados[i]) break;
            }

            if (i < (rndDados.Length / 2))
            {
                return (2 * Math.Abs(((double)i / (double)rndDados.Length)));
            }
            else
            {
                return (2 * Math.Abs(1.00 - ((double)i / (double)rndDados.Length)));
            }

            //if (dblEstat < 0)
            //{
            //    for (i = rndDados.Length - 1; i >= 0; i--)
            //    {
            //        if (dblEstat > rndDados[i]) break;
            //    }
            //    return (Math.Abs(((double)i / (double)rndDados.Length)));
            //}
            //else
            //{
            //    for (i = 0; i < rndDados.Length; i++)
            //    {
            //        if (dblEstat < rndDados[i]) break;
            //    }
            //    return (Math.Abs(1.00 - ((double)i / (double)rndDados.Length)));
            //}
            
        }

        public double IndiceGearyGlobal(double[,] y)
        {
            double res = 0.0;

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();


            double[,] z = new double[y.GetLength(0), 1];
            double media_y = clt.Mean(y);
            for (int i = 0; i < z.GetLength(0); i++)
            {
                z[i, 0] = y[i, 0] - media_y;
            }

            double den1 = (clt.MatrizMult(clt.MatrizTransp(z), z))[0, 0];

            clsMatrizEsparsa M;
            if (m_W_esparsa.FormatoMatrizEsparsa == TipoMatrizEsparsa.TripletForm)
                M = m_W_esparsa.Clone();
            else
                M = fme.CompressColumn2TripletForm(m_W_esparsa);

            int w = M.nzmax;
            double num = 0;
            double num2 = 0;
            double somaPeso = 0;

            for (int i = 0; i < w; i++)
            {
                num2 = Math.Pow(y[M.row_indices[i], 0] - y[M.col_indices[i], 0], 2);
                num += M.x[i] * num2;
                somaPeso += M.x[i];
            }

            den1 = 2 * somaPeso * den1;
            num = (y.GetLength(0) - 1) * num;

            res = num / den1;

            return res;
        }


        public double[] TesteGearyGlobal(double[,] y, ref ProgressBar pbar)
        {
            //Elementos do vetor de resultado:
            //Primeiro: esperança do Indice de Moran Global
            //Segundo: variância do Índice de Moran Global
            //Terceiro: valor da estatística de teste que possui uma dist normal padrão
            //Quarto: p-valor do Teste de Moran Bilateral segundo uma distribuição Normal

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();


            clsMatrizEsparsa M;
            if (m_W_esparsa.FormatoMatrizEsparsa == TipoMatrizEsparsa.TripletForm)
                M = m_W_esparsa.Clone();
            else
                M = fme.CompressColumn2TripletForm(m_W_esparsa);

            double[] res = new double[4];
            double geary = 0;
            geary = IndiceGearyGlobal(y);
            res[0] = 1;

            double s1 = 0;
            double s0 = 0;
            double[] somaLinhas = new double[y.GetLength(0)];
            double[] somaColunas = new double[y.GetLength(0)];

            int w = M.nzmax;
            int i1 = 0;
            int j1 = 0;
            //double wij = new double();
            //double wji = new double();

            // inclusão1 gabriela 19 de outubro de 2011
            pbar.Maximum = w;
            // fim inclusão1 gabriela 19 de outubro de 2011

            for (int i = 0; i < w; i++)
            {
                // inclusão2 gabriela 19 de outubro de 2011
                //Incrementa a progress bar

                pbar.Increment(1);
                Application.DoEvents();
                // fim inclusão2 gabriela 19 de outubro de 2011
                i1 = M.row_indices[i];
                j1 = M.col_indices[i];
                //wij = M.x[i];

                //for (int j = 0; j < Shape[j1].NumeroVizinhos; j++)
                //{
                //    if (Shape[j1].ListaIndicesVizinhos[j] == i1)
                //    {
                //        wji = Shape[j1].ListaPesosVizinhos[j];
                //        break; // inclusão 20 de outubro de 2011
                //    }
                //}
                //s1 = s1 + (Math.Pow(wij + wji, 2));
                somaLinhas[M.row_indices[i]] = somaLinhas[M.row_indices[i]] + M.x[i];
                somaColunas[M.col_indices[i]] = somaColunas[M.col_indices[i]] + M.x[i];
                s0 += M.x[i];
            }
            //s1 = s1 / ((double)2);

            // tentativa - inicio

            clsMatrizEsparsa WW = (fme.MatrizSoma(fme.MatrizTransp(m_W_esparsa), m_W_esparsa, 1.0, 1.0)).LimparElementosZero();
            clsMatrizEsparsa WWT = fme.CompressColumn2TripletForm(WW);
            double[] elementos_x = WW.x;
            for (int i = 0; i < elementos_x.GetLength(0); i++)
            {
                s1 += Math.Pow(elementos_x[i], 2.0);
            }
            s1 = s1 / 2.0;

            // tentativa - fim 

            double s2 = 0;
            for (int i = 0; i < y.GetLength(0); i++)
            {
                s2 = s2 + Math.Pow(somaLinhas[i] + somaColunas[i], 2);
            }

            double varN = ((((2 * s1) + s2) * (y.GetLength(0) - 1)) - (4 * (Math.Pow(s0, 2)))) / (2 * (y.GetLength(0) + 1) * Math.Pow(s0, 2)); // res[1] = varN - Math.Pow(res[0], 2); rever!!!
            res[1] = varN;
            res[2] = (geary - res[0]) / (Math.Sqrt(res[1]));

            MathNormaldist normdist = new MathNormaldist(0, 1);

            if (res[2] > 0)
                res[3] = 2 * (1 - normdist.cdf(res[2]));
            else
                res[3] = 2 * (normdist.cdf(res[2]));

            return res;
        }

        public object[] TesteTangoGlobal(double[,] mDados, ref ProgressBar pbar)
        {
            //Elementos do vetor de resultado:
            //Primeiro: esperança do Indice de Tango Global
            //Segundo: variância do Índice de Tango Global
            //Terceiro: p-valor do Teste de Tango Bilateral segundo uma distribuição Qui-Quadrado (Artigo Lance Waller)
            //Quarto: p-valor do Teste de Tango Bilateral segundo uma distribuição Qui-Quadrado (Artigo Rogerson)

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();

            object[] res = new object[4];
            //double tango = 0;
            //tango = IndiceDeTango(y); não chamei direto a função porque irei precisar de alguns elementos que são utilizados no cálculo do Tango

            clsMatrizEsparsa M;
            if (m_W_esparsa.FormatoMatrizEsparsa == TipoMatrizEsparsa.CompressColumn)
                M = m_W_esparsa.Clone();
            else
                M = fme.TripletForm2CompressColumn(m_W_esparsa);

            int n = mDados.GetLength(0);

            //Polígono
            double[,] dblEsperado = new double[mDados.GetLength(0), 1];
            double[,] dblObservado = new double[mDados.GetLength(0), 1];

            double dblSomaCasos = SomaColuna(mDados, 0); //Total dos casos
            double dblSomaPop = SomaColuna(mDados, 1); //Total da populacao

            for (int k = 0; k < dblEsperado.Length; k++)
            {
                dblObservado[k, 0] = mDados[k, 0] / dblSomaCasos;
                dblEsperado[k, 0] = mDados[k, 1] / (dblSomaPop);
            }

            double[,] diff = new double[mDados.GetLength(0), 1];

            diff = clt.MatrizSubtracao(dblObservado, dblEsperado);
            double[,] mtangoA = fme.MatrizMult(M, diff);
            double[,] mtango = clt.MatrizMult(clt.MatrizTransp(diff), mtangoA);
            double mTango = mtango[0, 0];
            //double[,] mat1=clt.MatrizMultMtranspM(dblEsperado);
            //double[,] mat2=clt.MatrizDiagonal(dblEsperado);
            //double[,] mat3=clt.MatrizSubtracao(mat2,mat1);


            //Criando uma matriz diagonal ESPARSA com dblEsperado
            clsMatrizEsparsa diag = new clsMatrizEsparsa(n, n, n, true);

            diag.x = new double[n];
            diag.p = new int[n];
            diag.i = new int[n];

            for (int i = 0; i < n; i++)
            {
                diag.x[i] = dblEsperado[i, 0];
                diag.i[i] = i;
                diag.p[i] = i;
            }

            double tracoavp = 0.0;
            double tracoavp2 = 0.0;
            double tracoavp3 = 0.0;

            M.LimparElementosZero();
            diag = fme.TripletForm2CompressColumn(diag);

            clsMatrizEsparsa ADp = (fme.MatrizMult(M, diag)).LimparElementosZero();
            clsMatrizEsparsa ADp2 = (fme.MatrizMult(ADp, ADp)).LimparElementosZero();
            clsMatrizEsparsa ADpA = (fme.MatrizMult(ADp, M)).LimparElementosZero();
            clsMatrizEsparsa ADp3 = (fme.MatrizMult(ADp2, ADp)).LimparElementosZero();
            clsMatrizEsparsa ADp2A = (fme.MatrizMult(ADp2, M)).LimparElementosZero();

            double pAp = (clt.MatrizMult(clt.MatrizTransp(dblEsperado), fme.MatrizMult(M, dblEsperado)))[0,0];
            double pADpAp = (clt.MatrizMult(clt.MatrizTransp(dblEsperado), fme.MatrizMult(ADpA, dblEsperado)))[0, 0];
            double pADp2Ap = (clt.MatrizMult(clt.MatrizTransp(dblEsperado), fme.MatrizMult(ADp2A, dblEsperado)))[0, 0];

            tracoavp = fme.traco(ADp) - pAp;
            tracoavp2 = fme.traco(ADp2) - 2.0 * pADpAp + (pAp * pAp);
            tracoavp3 = fme.traco(ADp3) - 3.0 * pADp2Ap + 3.0 * (pADpAp * pAp) - Math.Pow(pAp, 3.0);

            res[0] = tracoavp / dblSomaCasos;

            double varN = 2 * (tracoavp2 / Math.Pow(dblSomaCasos, 2));
            res[1] = varN;
            double Tstat = (mTango - (double)res[0]) /(Math.Sqrt((double) res[1]));

            double skti = Math.Sqrt(2 * Math.Sqrt(2) * (tracoavp3 / Math.Pow(tracoavp2, 1.5)));
            double dfti = 8 / (Math.Pow(skti, 2));

            MathChisqdist chidist = new MathChisqdist(dfti);
            double ChiStatLance = dfti + (Tstat * Math.Sqrt((2 * skti)));
            if (ChiStatLance < 0)
                res[2] = " Aproximação não válida";
            else
                res[2] = (1 - chidist.cdf((ChiStatLance)));

            res[2] = (1 - chidist.cdf((ChiStatLance)));

            double ChistatRogerson= dfti+(Tstat * Math.Sqrt((2*dfti)));
            if (ChistatRogerson < 0)
                res[3] = "Aproximação não válida";
            else
                res[3] = (1 - chidist.cdf(ChistatRogerson));


            return res;
        }


        public object[] TesteRogersonGlobal(double[,] mDados, ref ProgressBar pbar)
        {
            //Elementos do vetor de resultado:
            //Primeiro: esperança do Indice de Tango Global
            //Segundo: variância do Índice de Tango Global
            //Terceiro: p-valor do Teste de Tango Bilateral segundo uma distribuição Qui-Quadrado (Artigo Lance Waller)
            //Quarto: p-valor do Teste de Tango Bilateral segundo uma distribuição Qui-Quadrado (Artigo Rogerson)

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();

            object[] res = new object[4];
            //double tango = 0;
            //tango = IndiceDeTango(y); não chamei direto a função porque irei precisar de alguns elementos que são utilizados no cálculo do Tango

            clsMatrizEsparsa M;
            if (m_W_esparsa.FormatoMatrizEsparsa == TipoMatrizEsparsa.CompressColumn)
                M = m_W_esparsa.Clone();
            else
                M = fme.TripletForm2CompressColumn(m_W_esparsa);

            int n = mDados.GetLength(0);


            //Polígono
            double[,] dblEsperado = new double[mDados.GetLength(0), 1];
            double[,] dblObservado = new double[mDados.GetLength(0), 1];

            double dblSomaCasos = SomaColuna(mDados, 0); //Total dos casos
            double dblSomaPop = SomaColuna(mDados, 1); //Total da populacao

            for (int k = 0; k < dblEsperado.Length; k++)
            {
                dblObservado[k, 0] = mDados[k, 0] / dblSomaCasos;
                dblEsperado[k, 0] = (mDados[k, 1] + 0.000001) / (dblSomaPop); // A variável está sendo somada por um número epsilon, para não
                // ocorrer bug na divisão pelo valor esperado, quando ele for igual a zero.
            }

            double[,] diff = new double[mDados.GetLength(0), 1];

            diff = clt.MatrizDotMult(clt.MatrizSubtracao(dblObservado, dblEsperado), clt.MatrizDotPower(dblEsperado, -0.5));

            double[,] mrogA = fme.MatrizMult(M, diff);// ver se ele vai conseguir guardar isso. Aqui ja começa a dar NaN, conferir.
            double[,] mrog = clt.MatrizMult(clt.MatrizTransp(diff), mrogA);
            double mRog = mrog[0, 0];


            //double[,] mat1=clt.MatrizMultMtranspM(dblEsperado);
            //double[,] mat2=clt.MatrizDiagonal(dblEsperado);
            //double[,] mat3=clt.MatrizSubtracao(mat2,mat1);

            //Criando uma matriz diagonal ESPARSA com dblEsperado
            clsMatrizEsparsa diag = new clsMatrizEsparsa(n, n, n, true);

            diag.x = new double[n];
            diag.p = new int[n];
            diag.i = new int[n];

            for (int i = 0; i < n; i++)
            {
                diag.x[i] = dblEsperado[i,0];
                diag.i[i] = i;
                diag.p[i] = i;
            }
          
            //double[,] vp = clt.MatrizMult(dblEsperado, clt.MatrizTransp(dblEsperado));

            //// Matriz diagonal esparsa menos vp
            //vp = clt.MatrizMult(-1, vp);
            //for (int i = 0; i < n; i++)
            //{
            //    vp[i,i] = diag.x[i] + vp[i, i];
            //}

            //double[,] vp = clt.MatrizSubtracao(clt.MatrizDiagonal(dblEsperado), clt.MatrizMult(dblEsperado, clt.MatrizTransp(dblEsperado)));
            //double[,] teste = clt.MatrizSubtracao(vp2_p1, vp);
            //double somateste = clt.Sum(teste);

            // conferir vp2_p1 com vp
            clsMatrizEsparsa W = m_W_esparsa.Clone();
            if (m_W_esparsa.FormatoMatrizEsparsa == TipoMatrizEsparsa.CompressColumn)
                W = fme.CompressColumn2TripletForm(m_W_esparsa);
            
            for (int i = 0; i < M.x.GetLength(0); i++)
            {
                
                double pi = dblEsperado[W.i[i], 0];
                double pj = dblEsperado[W.p[i], 0];
                W.x[i] = W.x[i]/(Math.Sqrt((pi*pj)));
            }

            W = fme.TripletForm2CompressColumn(W);

            //double[,] wvp = fme.MatrizMult(W, vp);
            //double[,] wvp2 = clt.MatrizMult(wvp, wvp);
            //double[,] wvp3 = clt.MatrizMult(wvp2, wvp);

            double tracoavp = 0.0;
            double tracoavp2 = 0.0;
            double tracoavp3 = 0.0;

            W.LimparElementosZero();
            diag = fme.TripletForm2CompressColumn(diag);

            clsMatrizEsparsa ADp = (fme.MatrizMult(W, diag)).LimparElementosZero();
            clsMatrizEsparsa ADp2 = (fme.MatrizMult(ADp, ADp)).LimparElementosZero();
            clsMatrizEsparsa ADpA = (fme.MatrizMult(ADp, W)).LimparElementosZero();
            clsMatrizEsparsa ADp3 = (fme.MatrizMult(ADp2, ADp)).LimparElementosZero();
            clsMatrizEsparsa ADp2A = (fme.MatrizMult(ADp2, W)).LimparElementosZero();

            double pAp = (clt.MatrizMult(clt.MatrizTransp(dblEsperado), fme.MatrizMult(W, dblEsperado)))[0, 0];
            double pADpAp = (clt.MatrizMult(clt.MatrizTransp(dblEsperado), fme.MatrizMult(ADpA, dblEsperado)))[0, 0];
            double pADp2Ap = (clt.MatrizMult(clt.MatrizTransp(dblEsperado), fme.MatrizMult(ADp2A, dblEsperado)))[0, 0];

            tracoavp = fme.traco(ADp) - pAp;
            tracoavp2 = fme.traco(ADp2) - 2.0 * pADpAp + (pAp * pAp);
            tracoavp3 = fme.traco(ADp3) - 3.0 * pADp2Ap + 3.0 * (pADpAp * pAp) - Math.Pow(pAp, 3.0);


            res[0] = tracoavp / dblSomaCasos;
            double varN = 2 * (tracoavp2 / Math.Pow(dblSomaCasos, 2));
            res[1] = varN;
            double Rstat = (mRog - (double) res[0]) / (Math.Sqrt((double) res[1]));

            double skti = Math.Sqrt(2 * Math.Sqrt(2.0) * (tracoavp3 / Math.Pow(tracoavp2, 1.5)));
            double dfti = 8 / (Math.Pow(skti, 2));

            double ChiStatLance = dfti + (Rstat * Math.Sqrt((2 * skti)));

            MathChisqdist chidist = new MathChisqdist(dfti);
            if (ChiStatLance < 0)
                res[3] = "Aproximação não válida";
            else
                res[2] = (1 - chidist.cdf((ChiStatLance)));

            double ChistatRogerson = dfti + (Rstat * Math.Sqrt((2 * dfti)));
            if (ChistatRogerson < 0)
                res[3] = "Aproximação não válida";
            else
                res[3] = (1 - chidist.cdf(ChistatRogerson));


            return res;

        }









        public double pValorIndiceDeGeary(double[,] coluna, int n_simulacoes, double Geary_hat, ref ProgressBar pbar)
        {
            clsUtilTools clt = new clsUtilTools();
            // inclusão2 gabriela 18 de outubro de 2011
            pbar.Maximum = n_simulacoes;
            // fim inclusão2 gabriela 18 de outubro de 2011
            Troschuetz.Random.ContinuousUniformDistribution ranuni = new Troschuetz.Random.ContinuousUniformDistribution();

            object[,] y = new object[coluna.GetLength(0), 1];
            object[,] u = new object[y.GetLength(0), 1];
            object[,] x = new object[y.GetLength(0), 2];
            double[,] xd = new double[y.GetLength(0), 1];

            double[,] amostra = new double[n_simulacoes, 1];

            for (int i = 0; i < y.GetLength(0); i++)
            {
                y[i, 0] = coluna[i, 0];
            }

            for (int k = 0; k < n_simulacoes; k++)
            {
                // inclusão3 gabriela 18 de outubro de 2011
                //Incrementa a progress bar
                pbar.Increment(1);
                Application.DoEvents();
                // fim inclusão3 gabriela 18 de outubro de 2011
                for (int i = 0; i < u.GetLength(0); i++)
                {
                    u[i, 0] = ranuni.NextDouble();
                }
                x = clt.Concateh(u, y);
                clt.SortByColumn(ref x, x, 0);

                for (int i = 0; i < xd.GetLength(0); i++)
                {
                    xd[i, 0] = Convert.ToDouble(x[i, 1]);
                }

                amostra[k, 0] = this.IndiceGearyGlobal(xd);

            }

            double[] rndDados = new double[amostra.GetLength(0)];
            for (int i = 0; i < rndDados.GetLength(0); i++)
            {
                rndDados[i] = amostra[i, 0];
            }
            Array.Sort(rndDados);

            double pvalor = EncontraPvalor(rndDados, Geary_hat);
            if (pvalor > 0.5) pvalor = 1.0 - pvalor;

            return pvalor;

        }

        public double IndiceGetisOrdGiGlobal(double[,] y)
        {
            double res = 0.0;

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();

            double[,] aux1 = fme.MatrizMult(m_W_esparsa, y);
            double num1 = (clt.MatrizMult(clt.MatrizTransp(y), aux1))[0, 0];

            //double den1 = 0;

            //for (int i = 0; i < y.GetLength(0); i++)
            //{
            //    for (int k = 0; k < y.GetLength(0); k++)
            //    {
            //        if (k != i) den1 += y[i, 0] * y[k, 0];
            //    }
            //}

            double denteste = 0; // Esse denominador está ligeiramente diferente do calculado  com o for de cima para variáveis com
            // muitas casas decimais devido a erros de aproximação.

            for (int i = 0; i < y.GetLength(0); i++)
            {
                for (int k = (i + 1); k < y.GetLength(0); k++)
                {
                    denteste += y[i, 0] * y[k, 0];

                }
            }


            denteste = 2.0 * denteste;

            res = num1 / denteste;

            return res;
        }

        public double pValorGetisOrdGi(double[,] coluna, int n_simulacoes, double GetisOrdGi_hat, ref ProgressBar pbar)
        {
            clsUtilTools clt = new clsUtilTools();
            Troschuetz.Random.ContinuousUniformDistribution ranuni = new Troschuetz.Random.ContinuousUniformDistribution();

            // inclusão2 gabriela 18 de outubro de 2011
            pbar.Maximum = n_simulacoes;
            // fim inclusão2 gabriela 18 de outubro de 2011

            object[,] y = new object[coluna.GetLength(0), 1];
            object[,] u = new object[y.GetLength(0), 1];
            object[,] x = new object[y.GetLength(0), 2];
            double[,] xd = new double[y.GetLength(0), 1];

            double[,] amostra = new double[n_simulacoes, 1];

            for (int i = 0; i < y.GetLength(0); i++)
            {
                y[i, 0] = coluna[i, 0];
            }

            for (int k = 0; k < n_simulacoes; k++)
            {
                // inclusão3 gabriela 18 de outubro de 2011
                //Incrementa a progress bar
                pbar.Increment(1);
                Application.DoEvents();
                // fim inclusão3 gabriela 18 de outubro de 2011

                for (int i = 0; i < u.GetLength(0); i++)
                {
                    u[i, 0] = ranuni.NextDouble();
                }
                x = clt.Concateh(u, y);
                clt.SortByColumn(ref x, x, 0);

                for (int i = 0; i < xd.GetLength(0); i++)
                {
                    xd[i, 0] = Convert.ToDouble(x[i, 1]);
                }

                amostra[k, 0] = this.IndiceGetisOrdGiGlobal(xd);
            }

            double[] rndDados = new double[amostra.GetLength(0)];
            for (int i = 0; i < rndDados.GetLength(0); i++)
            {
                rndDados[i] = amostra[i, 0];
            }
            Array.Sort(rndDados);

            double pvalor = EncontraPvalor(rndDados, GetisOrdGi_hat);

            return pvalor;
        }

        public double[] TesteGetisGlobal(double[,] y, ref ProgressBar pbar)
        {
            //Elementos do vetor de resultado:
            //Primeiro: esperança do Indice de Getis Global
            //Segundo: variância do Índice de Getis Global
            //Terceiro: valor da estatística de teste que possui uma dist normal padrão
            //Quarto: p-valor do Teste de Getis Bilateral segundo uma distribuição Normal

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();


            clsMatrizEsparsa M;
            if (m_W_esparsa.FormatoMatrizEsparsa == TipoMatrizEsparsa.TripletForm)
                M = m_W_esparsa.Clone();
            else
                M = fme.CompressColumn2TripletForm(m_W_esparsa);

            double[] res = new double[4];
            double getis = 0;
            getis = this.IndiceGetisOrdGiGlobal(y);

            double s1 = 0;
            double s0 = 0;
            double[] somaLinhas = new double[y.GetLength(0)];
            double[] somaColunas = new double[y.GetLength(0)];


            int w = M.nzmax;
            int i1 = 0;
            int j1 = 0;
            //double wij = new double();
            //double wji = new double();

            // inclusão1 gabriela 19 de outubro de 2011
            pbar.Maximum = w;
            // fim inclusão1 gabriela 19 de outubro de 2011

            for (int i = 0; i < w; i++)
            {
                i1 = M.row_indices[i];
                j1 = M.col_indices[i];
                //wij = M.x[i];

                // inclusão2 gabriela 19 de outubro de 2011
                //Incrementa a progress bar

                pbar.Increment(1);
                Application.DoEvents();
                // fim inclusão2 gabriela 19 de outubro de 2011


                //for (int j = 0; j < Shape[j1].NumeroVizinhos; j++)
                //{
                //    if (Shape[j1].ListaIndicesVizinhos[j] == i1)
                //        wji = Shape[j1].ListaPesosVizinhos[j];
                //}
                //s1 = s1 + (Math.Pow(wij + wji, 2));
                somaLinhas[M.row_indices[i]] = somaLinhas[M.row_indices[i]] + M.x[i];
                somaColunas[M.col_indices[i]] = somaColunas[M.col_indices[i]] + M.x[i];
                s0 += M.x[i];
            }
            //s1 = s1 / ((double)2);

            // tentativa - inicio

            clsMatrizEsparsa WW = (fme.MatrizSoma(fme.MatrizTransp(m_W_esparsa), m_W_esparsa, 1.0, 1.0)).LimparElementosZero();
            clsMatrizEsparsa WWT = fme.CompressColumn2TripletForm(WW);
            double[] elementos_x = WW.x;
            for (int i = 0; i < elementos_x.GetLength(0); i++)
            {
                s1 += Math.Pow(elementos_x[i], 2.0);
            }
            s1 = s1 / 2.0;

            // tentativa - fim 

            double s2 = 0;
            double sy = 0;
            double sy2 = 0;
            double sy3 = 0;
            double sy4 = 0;
            for (int i = 0; i < y.GetLength(0); i++)
            {
                s2 = s2 + Math.Pow(somaLinhas[i] + somaColunas[i], 2);
                sy += y[i, 0];
                sy2 += Math.Pow(y[i, 0], 2);
                sy3 += Math.Pow(y[i, 0], 3);
                sy4 += Math.Pow(y[i, 0], 4);
            }

            res[0] = s0 / (y.GetLength(0) * (y.GetLength(0) - 1));

            double b0 = ((Math.Pow(y.GetLength(0), 2) - (3 * y.GetLength(0)) + 3) * s1) - (y.GetLength(0) * s2) + (3 * (Math.Pow(s0, 2)));
            double b1 = -1 * (((Math.Pow(y.GetLength(0), 2) - y.GetLength(0)) * s1) - (2 * y.GetLength(0) * s2) + (6 * (Math.Pow(s0, 2))));
            double b2 = -1 * (((2 * y.GetLength(0) * s1) - ((y.GetLength(0) + 3) * s2)) + (6 * Math.Pow(s0, 2)));
            double b3 = (4 * (y.GetLength(0) - 1) * s1) - (2 * (y.GetLength(0) + 1) * s2) + (8 * Math.Pow(s0, 2));
            double b4 = s1 - s2 + (Math.Pow(s0, 2));

            double varG = ((b0 * Math.Pow(sy2, 2)) + (b1 * sy4) + (b2 * (Math.Pow(sy, 2)) * sy2) +
                (b3 * sy * sy3) + (b4 * (Math.Pow(sy, 4)))) / (Math.Pow(Math.Pow(sy, 2) - sy2, 2) * y.GetLength(0) * (y.GetLength(0) - 1) * (y.GetLength(0) - 2) * (y.GetLength(0) - 3));
            res[1] = varG - Math.Pow(res[0], 2);
            res[2] = (getis - res[0]) / (Math.Sqrt(res[1]));

            MathNormaldist normdist = new MathNormaldist(0, 1);

            if (res[2] > 0)
                res[3] = 2 * (1 - normdist.cdf(res[2]));
            else
                res[3] = 2 * (normdist.cdf(res[2]));

            return res;
        }


        public double IndiceDeTango(double[,] mDados)
        {

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();

            clsMatrizEsparsa M;
            if (m_W_esparsa.FormatoMatrizEsparsa == TipoMatrizEsparsa.CompressColumn)
                M = m_W_esparsa.Clone();
            else
                M = fme.TripletForm2CompressColumn(m_W_esparsa);

            int n = mDados.GetLength(0);

            //Polígono
            double[,] dblEsperado = new double[mDados.GetLength(0), 1];
            double[,] dblObservado = new double[mDados.GetLength(0), 1];

            double dblSomaCasos = SomaColuna(mDados, 0); //Total dos casos
            double dblSomaPop = SomaColuna(mDados, 1); //Total da populacao

            clsUtilTools clt = new clsUtilTools();

            for (int k = 0; k < dblEsperado.Length; k++)
            {
                dblObservado[k, 0] = mDados[k, 0] / dblSomaCasos;
                dblEsperado[k, 0] = mDados[k, 1] / (dblSomaPop);
            }

            double[,] diff = new double[mDados.GetLength(0), 1];

            diff = clt.MatrizSubtracao(dblObservado, dblEsperado);
            double[,] mtangoA = fme.MatrizMult(M, diff);
            double[,] mtango = clt.MatrizMult(clt.MatrizTransp(diff), mtangoA);
            double mTango = mtango[0, 0];


            return (mTango);
        }


        public double pValorTango(double[,] coluna, int n_simulacoes, double Tango_hat, ref ProgressBar pbar)
        {
            clsUtilTools clt = new clsUtilTools();
            Troschuetz.Random.ContinuousUniformDistribution ranuni = new Troschuetz.Random.ContinuousUniformDistribution();

            // inclusão2 gabriela 18 de outubro de 2011
            pbar.Maximum = n_simulacoes;
            // fim inclusão2 gabriela 18 de outubro de 2011

            object[,] y = new object[coluna.GetLength(0), 2];
            object[,] u = new object[y.GetLength(0), 1];
            object[,] x = new object[y.GetLength(0), 3];
            double[,] xd = new double[y.GetLength(0), 2];

            double[,] amostra = new double[n_simulacoes, 1];

            for (int i = 0; i < y.GetLength(0); i++)
            {
                y[i, 0] = coluna[i, 0];
                y[i, 1] = coluna[i, 1];
            }

            for (int k = 0; k < n_simulacoes; k++)
            {
                // inclusão3 gabriela 18 de outubro de 2011
                //Incrementa a progress bar
                pbar.Increment(1);
                Application.DoEvents();
                // fim inclusão3 gabriela 18 de outubro de 2011

                for (int i = 0; i < u.GetLength(0); i++)
                {
                    u[i, 0] = ranuni.NextDouble();
                }
                x = clt.Concateh(u, y);
                clt.SortByColumn(ref x, x, 0);

                for (int i = 0; i < xd.GetLength(0); i++)
                {
                    xd[i, 0] = Convert.ToDouble(x[i, 1]);
                    xd[i, 1] = Convert.ToDouble(x[i, 2]);
                }

                amostra[k, 0] = this.IndiceDeTango(xd);
            }

            double[] rndDados = new double[amostra.GetLength(0)];
            for (int i = 0; i < rndDados.GetLength(0); i++)
            {
                rndDados[i] = amostra[i, 0];
            }
            Array.Sort(rndDados);

            double pvalor = EncontraPvalor(rndDados, Tango_hat);

            return pvalor;
        }

        public double pValorRogerson(double[,] coluna, int n_simulacoes, double Rogerson_hat, ref ProgressBar pbar)
        {
            clsUtilTools clt = new clsUtilTools();
            Troschuetz.Random.ContinuousUniformDistribution ranuni = new Troschuetz.Random.ContinuousUniformDistribution();

            // inclusão2 gabriela 18 de outubro de 2011
            pbar.Maximum = n_simulacoes;
            // fim inclusão2 gabriela 18 de outubro de 2011

            object[,] y = new object[coluna.GetLength(0), 2];
            object[,] u = new object[y.GetLength(0), 1];
            object[,] x = new object[y.GetLength(0), 3];
            double[,] xd = new double[y.GetLength(0), 2];

            double[,] amostra = new double[n_simulacoes, 1];

            for (int i = 0; i < y.GetLength(0); i++)
            {
                y[i, 0] = coluna[i, 0];
                y[i, 1] = coluna[i, 1];
            }

            for (int k = 0; k < n_simulacoes; k++)
            {
                // inclusão3 gabriela 18 de outubro de 2011
                //Incrementa a progress bar
                pbar.Increment(1);
                Application.DoEvents();
                // fim inclusão3 gabriela 18 de outubro de 2011

                for (int i = 0; i < u.GetLength(0); i++)
                {
                    u[i, 0] = ranuni.NextDouble();
                }
                x = clt.Concateh(u, y);
                clt.SortByColumn(ref x, x, 0);

                for (int i = 0; i < xd.GetLength(0); i++)
                {
                    xd[i, 0] = Convert.ToDouble(x[i, 1]);
                    xd[i, 1] = Convert.ToDouble(x[i, 2]);
                }

                amostra[k, 0] = this.IndiceDeRogerson(xd);
            }

            double[] rndDados = new double[amostra.GetLength(0)];
            for (int i = 0; i < rndDados.GetLength(0); i++)
            {
                rndDados[i] = amostra[i, 0];
            }
            Array.Sort(rndDados);

            double pvalor = EncontraPvalor(rndDados, Rogerson_hat);

            return pvalor;
        }



        public double IndiceDeRogerson(double[,] mDados)
        {
            //A primeira linha de mDados é a variável de interesse e a segunda é a população sob risco.

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();

            clsMatrizEsparsa M;
            if (m_W_esparsa.FormatoMatrizEsparsa == TipoMatrizEsparsa.CompressColumn)
                M = m_W_esparsa.Clone();
            else
                M = fme.TripletForm2CompressColumn(m_W_esparsa);

            int n = mDados.GetLength(0);


            //Polígono
            double[,] dblEsperado = new double[mDados.GetLength(0), 1];
            double[,] dblObservado = new double[mDados.GetLength(0), 1];

            double dblSomaCasos = SomaColuna(mDados, 0); //Total dos casos
            double dblSomaPop = SomaColuna(mDados, 1); //Total da populacao

            clsUtilTools clt = new clsUtilTools();

            for (int k = 0; k < dblEsperado.Length; k++)
            {
                dblObservado[k, 0] = mDados[k, 0]/ dblSomaCasos;
                dblEsperado[k, 0] = (mDados[k, 1] + 0.000001) / (dblSomaPop); // A variável está sendo somada por um número epsilon, para não
                                                                              // ocorrer bug na divisão pelo valor esperado, quando ele for igual a zero.
            }

            double[,] diff = new double[mDados.GetLength(0), 1];

            diff = clt.MatrizDotMult(clt.MatrizSubtracao(dblObservado, dblEsperado), clt.MatrizDotPower(dblEsperado, -0.5));
 
            double[,] mrogA = fme.MatrizMult(M, diff);// ver se ele vai conseguir guardar isso. Aqui ja começa a dar NaN, conferir.
            double[,] mrog = clt.MatrizMult(clt.MatrizTransp(diff), mrogA);
            double mRog = mrog[0, 0];


            return (mRog);
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

    }
}