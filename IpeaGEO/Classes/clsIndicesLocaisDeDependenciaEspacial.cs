using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;
using MathNet.Numerics.Distributions;

namespace IpeaGeo
{
    class clsIndicesLocaisDeDependenciaEspacial : RegressoesEspaciais.clsLinearRegressionModelsMLE
    {
        Normal normal = new Normal();

        public clsIndicesLocaisDeDependenciaEspacial(RegressoesEspaciais.TipoMatrizVizinhanca tipo_matriz)
        {
            this.TipoMatrizVizinhanca = tipo_matriz;
        }

        const double OneOverRootTwoPi = 0.398942280401433;

        private double SomaColuna(double[,] mDados, int coluna)
        {
            double dblSoma = 0;
            for (int k = 0; k < mDados.GetLength(0); k++)
            {
                dblSoma += mDados[k, coluna];
            }
            
            return (dblSoma);
        }

        private double Significancia(double mValor)
        {
            double dblSig = 0;
            if (mValor < 0)
            {
                dblSig = normal.CumulativeDistribution(mValor);
            }
            else
            {
                dblSig = 1 - normal.CumulativeDistribution(mValor);
            }
            dblSig = 2 * dblSig;
            
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
        public void LISA(ref DataTable dsTable, string strIdMapa, double[,] mDados, string strVariavel, int iVariavel, RegressoesEspaciais.clsIpeaShape mShape, double dblSignificancia, ref ArrayList arEspalhamento, ref ArrayList arPintaMapa, ref ProgressBar pbBar, ref Label lblStatus)
        {
            pbBar.Value = 0;
            pbBar.Maximum = mDados.GetLength(0);
            pbBar.Refresh();
            Application.DoEvents();

            //Retira as variáveis caso pelo menos uma delas já exista, para não manter juntas variáveis com interpretações complementares, mas vindas de análises diferentes
            string nome1 = strVariavel + "_LI";
            string nome2 = strVariavel + "_LS";
            string nome3 = strVariavel + "_LC";
            string nome4 = strVariavel + "_LN";
            string nome5 = strVariavel + "_LM"; // não será criada aqui, embora seja removida, porque está sendo criada no frmapa.
            
            if (dsTable.Columns.Contains(nome1) == true | dsTable.Columns.Contains(nome2) == true | dsTable.Columns.Contains(nome3) == true | dsTable.Columns.Contains(nome4) == true | dsTable.Columns.Contains(nome5) == true)
            {
                dsTable.Columns.Remove(nome1);
                dsTable.Columns.Remove(nome2);
                dsTable.Columns.Remove(nome3);
                dsTable.Columns.Remove(nome4);
                dsTable.Columns.Remove(nome5);  
            }

            //Adicionando as novas variáveis a base
            dsTable.Columns.Add(nome1, Type.GetType("System.Double"));// colocando as variáveis numéricas como tal, e não como string.
            dsTable.Columns.Add(nome2, Type.GetType("System.Double"));
            dsTable.Columns.Add(nome3, Type.GetType("System.String"));
            dsTable.Columns.Add(nome4, Type.GetType("System.Double"));

            //// Inicio alteração gabriela: dezembro de 2011
            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();

            this.Shape = mShape;

            if (m_tipo_matriz_vizinhanca == RegressoesEspaciais.TipoMatrizVizinhanca.Normalizada)
            {
                this.MatrizWesparsaFromVizinhosNorm();
            }

            if (m_tipo_matriz_vizinhanca == RegressoesEspaciais.TipoMatrizVizinhanca.Original)
            {
                this.MatrizWesparsaFromVizinhos();
            }

            double[,] y = clt.GetMatrizFromDataTable(dsTable, strVariavel);
            double[,] z = new double[y.GetLength(0), 1];
            double media_y = clt.Mean(y);
            for (int i = 0; i < z.GetLength(0); i++)
            {
                z[i, 0] = y[i, 0] - media_y;
            }
            double m2 = clt.Sum(clt.MatrizDotPower(z, 2)) / z.GetLength(0);
            double m4 = clt.Sum(clt.MatrizDotPower(z, 4)) / z.GetLength(0);

            double dblVariancia = (clt.Varianciac(y)[0, 0]) * (z.GetLength(0) / (double)(z.GetLength(0)-1)); 

            double[,] zs2 = clt.MatrizDiv(z, m2);
            double[,] lagz = fme.MatrizMult(m_W_esparsa, z);
            double[,] dblLISA = clt.MatrizDotMult(zs2, lagz);
            double[,] dblSomaPeso = fme.MatrizMult(m_W_esparsa, clt.Ones(z.GetLength(0), 1));
            double[,] dblSomaPeso2 = fme.MatrizMult(fme.MatrizDotPower(m_W_esparsa, 2), clt.Ones(z.GetLength(0), 1));
            double[,] dblCrossProd = clt.MatrizSubtracao(clt.Ones(z.GetLength(0),1), dblSomaPeso2); // seguindo operação do R

            int[] iPinta = new int[mDados.GetLength(0)];
            double[,] dblEspalhamento1 = clt.MatrizDiv(z, Math.Sqrt(dblVariancia));
            double[,] dblEspalhamento2 = clt.MatrizDiv(lagz,Math.Sqrt(dblVariancia));
            double[,] dblEspalhamento=clt.Concateh(dblEspalhamento1,dblEspalhamento2);

            double[,] dblLisaE = clt.MatrizDiv(dblSomaPeso, -(z.GetLength(0)-1));
            double b2 = (m4 / (m2 * m2));
            double A= (z.GetLength(0)-b2)/(z.GetLength(0)-1);
            double B = (2 * b2 - z.GetLength(0))/((z.GetLength(0) - 1) * (z.GetLength(0) - 2));
            double[,] C =clt.MatrizDiv(clt.MatrizDotPower(dblSomaPeso,2),Math.Pow(z.GetLength(0)-1,2));
 
            double[,] dblLisaV = clt.MatrizSubtracao(clt.MatrizSoma(clt.MatrizMult(A,dblSomaPeso2), clt.MatrizMult(B,dblCrossProd)), C);

            double[,] dblLISAn = clt.MatrizDotMult((clt.MatrizSubtracao(dblLISA,dblLisaE)),(clt.MatrizDotPower(dblLisaV,(-0.5))));
            double[,] dblLISAs= new double[z.GetLength(0),1];

            for (int i = 0; i < z.GetLength(0); i++)
            {
                int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);
                dblLISAs[i,0] = Significancia(dblLISAn[i,0]);
                //Guarda os valores na base de dados

                dsTable.Rows[i][nome1] = dblLISA[i,0].ToString();
                dsTable.Rows[i][nome2] = dblLISAs[i,0].ToString();
                dsTable.Rows[i][nome4] = dblLISAn[i,0].ToString();
                if (z[i,0] > 0 && lagz[i,0] > 0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Alto";
                    if (dblLISAs[i,0] <= dblSignificancia)
                    {
                        iPinta[iMapa] = 1;
                    }
                }
                else if (z[i,0] > 0 && lagz[i,0] < 0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Baixo";
                    if (dblLISAs[i,0] <= dblSignificancia)
                    {
                        iPinta[iMapa] = 2;
                    }
                }
                else if (z[i,0] < 0 && lagz[i,0] < 0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Baixo";
                    if (dblLISAs[i,0] <= dblSignificancia)
                    {
                        iPinta[iMapa] = 3;
                    }
                }
                else if (z[i,0] < 0 && lagz[i,0] > 0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Alto";
                    if (dblLISAs[i,0] <= dblSignificancia)
                    {
                        iPinta[iMapa] = 4;
                    }
                }
                else
                {
                    dsTable.Rows[i][nome3] = "NaN";
                    iPinta[iMapa] = 0;
                }
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
        public void Getis_Ord_Gi(ref DataTable dsTable, string strIdMapa, double[,] mDados, string strVariavel, int iVariavel, RegressoesEspaciais.clsIpeaShape mShape, RegressoesEspaciais.clsIpeaShape shapeInclude, double dblSignificancia, ref ArrayList arPintaMapa, ref ProgressBar pbBar, ref Label lblStatus)
        {
            pbBar.Value = 0;
            pbBar.Maximum = mDados.GetLength(0);
            pbBar.Refresh();
            Application.DoEvents();

            //Retira as variáveis caso pelo menos uma delas já exista, para não manter juntas variáveis com interpretações complementares, mas vindas de análises diferentes
            string nome1 = strVariavel + "_G*I";
            string nome2 = strVariavel + "_G*S";
            string nome3 = strVariavel + "_G*C";
            string nome4 = strVariavel + "_G*M"; // não será criada aqui, embora seja removida, porque está sendo criada no frmapa.
            if (dsTable.Columns.Contains(nome1) == true | dsTable.Columns.Contains(nome2) == true | dsTable.Columns.Contains(nome3) == true | dsTable.Columns.Contains(nome4) == true)
            {
                dsTable.Columns.Remove(nome1);
                dsTable.Columns.Remove(nome2);
                dsTable.Columns.Remove(nome3);
                dsTable.Columns.Remove(nome4);
            }

            //Adicionando as novas variáveis a base
            dsTable.Columns.Add(nome1, Type.GetType("System.Double"));// colocando as variáveis numéricas como tal, e não como string.
            dsTable.Columns.Add(nome2, Type.GetType("System.Double"));
            dsTable.Columns.Add(nome3, Type.GetType("System.String"));

            // Modificação Gabriela dezembro de 2011
            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();

            this.Shape = shapeInclude; // posso fazer isso pois não há função que use o Shape
            if (m_tipo_matriz_vizinhanca == RegressoesEspaciais.TipoMatrizVizinhanca.Normalizada)
            {
                this.MatrizWesparsaFromVizinhosNormGetis();
            }

            if (m_tipo_matriz_vizinhanca == RegressoesEspaciais.TipoMatrizVizinhanca.Original)
            {
                this.MatrizWesparsaFromVizinhosGetis();
            }

            double[,] y = clt.GetMatrizFromDataTable(dsTable, strVariavel);
            int n = y.GetLength(0);
            double[,] z = new double[y.GetLength(0), 1];
            double media_y = clt.Mean(y);
            for (int i = 0; i < z.GetLength(0); i++)
            {
                z[i, 0] = y[i, 0] - media_y;
            }

            double dblVariancia = (clt.Varianciac(y)[0, 0]); 

            double[,] lagy = fme.MatrizMult(m_W_esparsa, y);
            double[,] lagz = fme.MatrizMult(m_W_esparsa, z);
            double[,] dblSomaPeso = fme.MatrizMult(m_W_esparsa, clt.Ones(y.GetLength(0), 1));
            double[,] dblSomaPeso2 = fme.MatrizMult(fme.MatrizDotPower(m_W_esparsa, 2), clt.Ones(y.GetLength(0), 1));
            double[,] dblGIden=clt.MatrizSubtracao(lagy, clt.MatrizMult( media_y, dblSomaPeso));
            double[,] dblGInum=clt.MatrizDotPower(clt.MatrizMult((dblVariancia /(double)(n-1)),clt.MatrizSubtracao(clt.MatrizMult(n,dblSomaPeso2),clt.MatrizDotPower(dblSomaPeso,2))),0.5);
            double[,] dblGI= clt.MatrizDotMult(dblGIden,clt.MatrizDotPower(dblGInum,-1));

            int[] iPinta = new int[y.GetLength(0)];
            double[,] dblGIs = new double[y.GetLength(0), 1];
            for (int i = 0; i < y.GetLength(0); i++)
            {
                int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);
                dblGIs[i, 0] = Significancia(dblGI[i, 0]);

                //Guarda os valores na base de dados
                dsTable.Rows[i][nome1] = dblGI[i,0].ToString();
                dsTable.Rows[i][nome2] = dblGIs[i,0].ToString();
                if (z[i, 0] > 0 && lagz[i, 0] > 0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Alto";
                    if (dblGIs[i,0] <= dblSignificancia) iPinta[iMapa] = 1;
                }
                else if (z[i,0] > 0 && lagz[i,0] < 0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Baixo";
                    if (dblGIs[i,0] <= dblSignificancia) iPinta[iMapa] = 2;
                }
                else if (z[i,0] < 0 && lagz[i,0] < 0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Baixo";
                    if (dblGIs[i,0] <= dblSignificancia) iPinta[iMapa] = 3;
                }
                else if (z[i,0] < 0 && lagz[i,0] > 0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Alto";
                    if (dblGIs[i,0] <= dblSignificancia)
                    {
                        iPinta[iMapa] = 4;
                    }
                }
                else
                {
                    dsTable.Rows[i][nome3] = "NaN";
                    iPinta[iMapa] = 0;
                }
            }

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
        public void Getis_Ord_Gi2(ref DataTable dsTable, string strIdMapa, double[,] mDados, string strVariavel, int iVariavel, RegressoesEspaciais.clsIpeaShape mShape, double dblSignificancia, ref ArrayList arPintaMapa, ref ProgressBar pbBar, ref Label lblStatus)
        {
            pbBar.Value = 0;
            pbBar.Maximum = mDados.GetLength(0);
            pbBar.Refresh();
            Application.DoEvents();

            //Retira as variáveis caso pelo menos uma delas já exista, para não manter juntas variáveis com interpretações complementares, mas vindas de análises diferentes
            string nome1 = strVariavel + "_GI";
            string nome2 = strVariavel + "_GS";
            string nome3 = strVariavel + "_GC";
            string nome4 = strVariavel + "_GM"; // não será criada aqui, embora seja removida, porque está sendo criada no frmapa.
            if (dsTable.Columns.Contains(nome1) == true | dsTable.Columns.Contains(nome2) == true | dsTable.Columns.Contains(nome3) == true | dsTable.Columns.Contains(nome4) == true)
            {
                dsTable.Columns.Remove(nome1);
                dsTable.Columns.Remove(nome2);
                dsTable.Columns.Remove(nome3);
                dsTable.Columns.Remove(nome4);
            }

            //Adicionando as novas variáveis a base
            dsTable.Columns.Add(nome1, Type.GetType("System.Double"));// colocando as variáveis numéricas como tal, e não como string.
            dsTable.Columns.Add(nome2, Type.GetType("System.Double"));
            dsTable.Columns.Add(nome3, Type.GetType("System.String"));

            // Alterações Gabriela: Dezembro de 2011;
            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();

            this.Shape = mShape;

            if (m_tipo_matriz_vizinhanca == RegressoesEspaciais.TipoMatrizVizinhanca.Normalizada)
            {
                this.MatrizWesparsaFromVizinhosNorm();
            }

            if (m_tipo_matriz_vizinhanca == RegressoesEspaciais.TipoMatrizVizinhanca.Original)
            {
                this.MatrizWesparsaFromVizinhos();
            }

            double[,] y = clt.GetMatrizFromDataTable(dsTable, strVariavel);
            int n = y.GetLength(0);
            double[,] dblMedia_i = new double[y.GetLength(0), 1];
            double media_y = clt.Mean(y)*(y.GetLength(0)/(double)(y.GetLength(0)-1));
            double sum_y = clt.Sum(clt.MatrizDotPower(y,2))/(double)(y.GetLength(0)-1);
            double[,] dblVariancia_i = new double[y.GetLength(0),1];
            double[,] z = new double[y.GetLength(0), 1];

            for (int i = 0; i < y.GetLength(0); i++)
            {
                z[i, 0] = y[i,0] - (media_y*((n-1)/(double)n));
                dblMedia_i[i, 0] = media_y-(y[i, 0]/(double)(y.GetLength(0)-1)) ;
                dblVariancia_i[i,0]= sum_y-(Math.Pow(y[i, 0],2)/(double)(y.GetLength(0)-1))-Math.Pow(dblMedia_i[i, 0],2);
            }

            //double[,] zs2 = clt.MatrizDiv(z, m2);
            double[,] lagy = fme.MatrizMult(m_W_esparsa, y);
            double[,] lagz = fme.MatrizMult(m_W_esparsa, z);
            double[,] dblSomaPeso = fme.MatrizMult(m_W_esparsa, clt.Ones(y.GetLength(0), 1));
            double[,] dblSomaPeso2 = fme.MatrizMult(fme.MatrizDotPower(m_W_esparsa, 2), clt.Ones(y.GetLength(0), 1));
            double[,] dblGIden=clt.MatrizSubtracao(lagy, clt.MatrizDotMult(dblSomaPeso,dblMedia_i));
            double[,] dblGInum=clt.MatrizDotPower(clt.MatrizDotMult(clt.MatrizDiv(dblVariancia_i,(double)(n-2)),clt.MatrizSubtracao(clt.MatrizMult((n-1),dblSomaPeso2),clt.MatrizDotPower(dblSomaPeso,2))),0.5);
            double[,] dblGI= clt.MatrizDotMult(dblGIden,clt.MatrizDotPower(dblGInum,-1));

            int[] iPinta = new int[y.GetLength(0)];
            double[,] dblGIs = new double[y.GetLength(0), 1];
            for (int i = 0; i < y.GetLength(0); i++)
            {
                int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);
                dblGIs[i, 0] = Significancia(dblGI[i, 0]);

                //Guarda os valores na base de dados
                dsTable.Rows[i][nome1] = dblGI[i, 0].ToString();
                dsTable.Rows[i][nome2] = dblGIs[i, 0].ToString();
                if (z[i, 0] > 0 && lagz[i, 0] > 0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Alto";
                    if (dblGIs[i, 0] <= dblSignificancia) iPinta[iMapa] = 1;
                }
                else if (z[i, 0] > 0 && lagz[i, 0] < 0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Baixo";
                    if (dblGIs[i, 0] <= dblSignificancia) iPinta[iMapa] = 2;
                }
                else if (z[i, 0] < 0 && lagz[i, 0] < 0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Baixo";
                    if (dblGIs[i, 0] <= dblSignificancia) iPinta[iMapa] = 3;
                }
                else if (z[i, 0] < 0 && lagz[i, 0] > 0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Alto";
                    if (dblGIs[i, 0] <= dblSignificancia)
                    {
                        iPinta[iMapa] = 4;
                    }
                }
                else
                {
                    dsTable.Rows[i][nome3] = "NaN";
                    iPinta[iMapa] = 0;
                }
            }

            //pbBar.Increment(1);
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
        public void Escore(ref DataTable dsTable, string strIdMapa, double[,] mDados, string strVariavel, int iVariavel, RegressoesEspaciais.clsIpeaShape mShape, double dblSignificancia, ref ArrayList arPintaMapa, ref ProgressBar pbBar, ref Label lblStatus)
        {
            pbBar.Value = 0;
            pbBar.Maximum = mDados.GetLength(0);
            pbBar.Refresh();
            Application.DoEvents();

            //Retira as variáveis caso pelo menos uma delas já exista, para não manter juntas variáveis com interpretações complementares, mas vindas de análises diferentes
            string nome1 = strVariavel + "_EI";
            string nome2 = strVariavel + "_ES";
            string nome3 = strVariavel + "_EC";
            string nome4 = strVariavel + "_EN";
            string nome5 = strVariavel + "_EM"; // não será criada aqui, embora seja removida, porque está sendo criada no frmapa.
            
            if (dsTable.Columns.Contains(nome1) == true | dsTable.Columns.Contains(nome2) == true | dsTable.Columns.Contains(nome3) == true | dsTable.Columns.Contains(nome4) == true | dsTable.Columns.Contains(nome5) == true)
            {
                dsTable.Columns.Remove(nome1);
                dsTable.Columns.Remove(nome2);
                dsTable.Columns.Remove(nome3);
                dsTable.Columns.Remove(nome4);
                dsTable.Columns.Remove(nome5);
            }

            //Adicionando as novas variáveis a base
            dsTable.Columns.Add(nome1, Type.GetType("System.Double")); // colocando as variáveis numéricas como tal, e não como string.
            dsTable.Columns.Add(nome2, Type.GetType("System.Double"));
            dsTable.Columns.Add(nome3, Type.GetType("System.String"));
            dsTable.Columns.Add(nome4, Type.GetType("System.Double"));

            double dblSomaCasos = SomaColuna(mDados, iVariavel);
            double dblSomaPop = SomaColuna(mDados, mDados.GetLength(1) - 1);

            //Encontra a média
            double dblMedia = SomaColuna(mDados, iVariavel) / mDados.GetLength(0);

            // Início alterações em dezembro de 2011
            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            clsUtilTools clt = new clsUtilTools();

            this.Shape = mShape;

            if (m_tipo_matriz_vizinhanca == RegressoesEspaciais.TipoMatrizVizinhanca.Normalizada)
            {
                this.MatrizWesparsaFromVizinhosNorm();
            }

            if (m_tipo_matriz_vizinhanca == RegressoesEspaciais.TipoMatrizVizinhanca.Original)
            {
                this.MatrizWesparsaFromVizinhos();
            }

            double[,] dblObservado = new double[mDados.GetLength(0),1]; //conterá a proporção observada de casos
            double[,] dblEsperado = new double[mDados.GetLength(0),1]; //conterá a proporção observada de população

            //Padroniza os dados
            double[,] dblValor = new double[mDados.GetLength(0),1]; //conterá diferenças entre cada valor de casos observado e a média de casos
            for (int k = 0; k < mDados.GetLength(0); k++)
            {
                dblObservado[k,0] = mDados[k, iVariavel] / dblSomaCasos; //ri
                dblEsperado[k,0] = mDados[k, mDados.GetLength(1) - 1] / (dblSomaPop); //pi
                dblValor[k,0] = (mDados[k, iVariavel] - dblMedia);
            }

            double[,] diff = clt.MatrizSubtracao(dblObservado, dblEsperado);
            double[,] dblESCORE = clt.MatrizMult(mDados.GetLength(0), fme.MatrizMult(m_W_esparsa, diff));
            double[,] dblVariancia2 = fme.MatrizMult(m_W_esparsa, dblEsperado);
            double[,] dblVariancia1 = fme.MatrizMult(fme.MatrizDotPower(m_W_esparsa, 2), dblEsperado);
            double[,] dblVizinhos = fme.MatrizMult(m_W_esparsa, dblValor);
            double[,] dblESCOREv = clt.MatrizMult(mDados.GetLength(0), clt.MatrizSoma(dblVariancia1, clt.MatrizDotPower(dblVariancia2, 2)));
            double[,] dblESCOREn = clt.MatrizDotMult(dblESCORE, clt.MatrizDotPower(dblESCOREv,-0.5));
            int[] iPinta = new int[mDados.GetLength(0)];

            double[,] dblESCOREs = new double[mDados.GetLength(0), 1];
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                int iMapa = Convert.ToInt32(dsTable.Rows[i]["Mapa" + strIdMapa]);
                dblESCOREs[i, 0] = Significancia(dblESCOREn[i, 0]);

                //Guarda os valores na base de dados
                dsTable.Rows[i][nome1] = dblESCORE[i,0].ToString();
                dsTable.Rows[i][nome2] = dblESCOREs[i, 0].ToString();
                dsTable.Rows[i][nome4] = dblESCOREv[i, 0].ToString();
                if (dblValor[i, 0] > 0 && dblVizinhos[i, 0] > 0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Alto";
                    if (dblESCOREs[i, 0] <= dblSignificancia) iPinta[iMapa] = 1;
                }
                else if (dblValor[i, 0] > 0 && dblVizinhos[i, 0] < 0)
                {
                    dsTable.Rows[i][nome3] = "Alto-Baixo";
                    if (dblESCOREs[i, 0] <= dblSignificancia) iPinta[iMapa] = 2;
                }
                else if (dblValor[i, 0] < 0 && dblVizinhos[i, 0] < 0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Baixo";
                    if (dblESCOREs[i, 0] <= dblSignificancia) iPinta[iMapa] = 3;
                }
                else if (dblValor[i, 0] < 0 && dblVizinhos[i, 0] > 0)
                {
                    dsTable.Rows[i][nome3] = "Baixo-Alto";
                    if (dblESCOREs[i, 0] <= dblSignificancia)
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
