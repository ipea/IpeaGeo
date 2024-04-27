using System;

using IpeaGeo.RegressoesEspaciais;
using MathNet.Numerics.Distributions;

namespace IpeaGeo.Modelagem
{
    public class BlogicAnaliseFatorial : BLogicBaseModelagem
    {

        #region variáveis internas

        private double[] percentuais = new double[0];
        public double[] m_percentuais
        {
            get { return percentuais; }
            set { percentuais = value; }
        }

        private string m_CombOx = "";
        public string cmbTIPOMatz
        {
            get { return m_CombOx; }
            set { m_CombOx = value; }
        }

        private string m_tipoestimacao = "";
        public string cmbtipoestimacao
        {
            get { return m_tipoestimacao; }
            set { m_tipoestimacao = value; }
        }

        private string m_tipescore = "";
        public string cmbtipoescore
        {
            get { return m_tipescore; }
            set { m_tipescore = value; }
        }

        private string m_tiporotation = "";
        public string cmbtiporotation
        {
            get { return m_tiporotation; }
            set { m_tiporotation = value; }
        }

        private bool m_ckbapresentavarcovar = false;
        public bool ckbvarcovar
        {
            get { return m_ckbapresentavarcovar; }
            set { m_ckbapresentavarcovar = value; }
        }

        private bool m_ckbapresentainversacorrel = false;
        public bool ckbapresentainversacorrel
        {
            get { return m_ckbapresentainversacorrel; }
            set { m_ckbapresentainversacorrel = value; }
        }

        private bool m_ckbrotacao = false;
        public bool ckbrotacao
        {
            get { return m_ckbrotacao; }
            set { m_ckbrotacao = value; }
        }

        private bool m_ckbmatrizresidual = false;
        public bool ckbmatrizresidual
        {
            get { return m_ckbmatrizresidual; }
            set { m_ckbmatrizresidual = value; }
        }

        private bool m_ckbbartlet = false;
        public bool ckbbartlet
        {
            get { return m_ckbbartlet; }
            set { m_ckbbartlet = value; }
        }

        private bool m_ckbescore = false;
        public bool ckbescore
        {
            get { return m_ckbescore; }
            set { m_ckbescore = value; }
        }

        private int m_numcomponentes = 0;
        public int numcomponentes
        {
            get { return m_numcomponentes; }
            set { m_numcomponentes = value; }
        }

        private bool m_ckbapresentacorrel = false;
        public bool ckbcorrel
        {
            get { return m_ckbapresentacorrel; }
            set { m_ckbapresentacorrel = value; }
        }

        private bool m_ckbestimacoef = false;
        public bool ckbestimacoef
        {
            get { return m_ckbestimacoef; }
            set { m_ckbestimacoef = value; }
        }

        #endregion

        #region funções de estimação

        public void EstimarAnaliseFatorial(ref double[] percentuais)
        {
            clsUtilTools clt = new clsUtilTools();


            double[,] X = clt.GetMatrizFromDataTable(m_dt_tabela_dados, VariaveisIndependentes);
            double[,] varcovar = new double[X.GetLength(1), X.GetLength(1)];
            double[,] correlmatrix = new double[X.GetLength(1), X.GetLength(1)];

            int p = X.GetLength(1);
            double[,] matrizbase = new double[p, p];
            matrizbase = clt.CorrSampleMatrix(X);

            double[,] autovalores = new double[p, 1];
            double[,] autovetor = new double[p, p];
            double[] autovalorD = new double[p];
            clt.AutovaloresMatrizSimetrica(matrizbase, ref autovetor, ref autovalorD);

            double[,] autovetorcorte = new double[X.GetLength(1), numcomponentes];
            double[] autovalorDcorte = new double[numcomponentes];

            for (int i = 0; i < autovalorD.GetLength(0); i++)
                if (autovalorD[i] == 0.0)
                    throw new Exception("As variáveis escolhidas formam uma matriz singular. Retire ao menos uma das variáveis que é combinação linear das demais.");

            for (int i = 0; i < numcomponentes; i++)
            {
                autovalorDcorte[i] = autovalorD[i];
                for (int j = 0; j < X.GetLength(1); j++)
                    autovetorcorte[j, i] = autovetor[j, i];
            } // for

            if (ckbcorrel || ckbbartlet)
                correlmatrix = clt.CorrSampleMatrix(X);

        #endregion

            #region Estimação de Coeficientes;

            double[,] matrizXnormalizada = new double[X.GetLength(0), X.GetLength(1)];
            double[,] rij = new double[autovetorcorte.GetLength(0), autovetorcorte.GetLength(1)];
            double[,] ksichapeu = new double[p, p];
            double[,] diagonalksichapeu = new double[p, 1];
            double[,] hi2 = new double[p, 1];

            #region componentes Principais
            if (cmbtipoestimacao == "Componentes Principais")
            {
                for (int i = 0; i < autovetorcorte.GetLength(0); i++)
                    for (int j = 0; j < autovetorcorte.GetLength(1); j++)
                        rij[i, j] = ((autovetorcorte[i, j]) * (Math.Sqrt(autovalorDcorte[j]))) / (Math.Sqrt(matrizbase[i, i]));

                ksichapeu = clt.MatrizSubtracao(matrizbase, clt.MatrizMult(rij, clt.MatrizTransp(rij)));

                for (int i = 0; i < p; i++)
                    diagonalksichapeu[i, 0] = ksichapeu[i, i];

                for (int i = 0; i < p; i++)
                    for (int j = 0; j < numcomponentes; j++)
                        hi2[i, 0] += Math.Pow(rij[i, j], 2);
            } // if

            #endregion

            int nummaxiterações = 100;
            double[,] comunalidades_iniciais = clt.Ones(VariaveisIndependentes.GetLength(0), 1);

            #region Fatores Principais
            bool erro_comunalidade_maior_que_1 = false;
            int numero_iterações_fatoresprincipais = 0;
            bool priori_comunalidades = false;

            if (cmbtipoestimacao == "Fatores Principais")
            {
                double[,] newX = X;
                double[,] newY = new double[X.GetLength(0), 1];
                double[,] um1 = clt.Ones(X.GetLength(0), 1);
                double[,] matrizbasefatoresprincipais = clt.ArrayDoubleClone(matrizbase);
                double[,] hi2antigo = new double[X.GetLength(1), 1];
                double[,] delta = new double[X.GetLength(1), 1];
                double somadelta;

                for (int i = 0; i < X.GetLength(1); i++)
                {
                    newY = clt.SubColumnArrayDouble(X, i);
                    newX = clt.DeleteCol(X, i);
                    newX = clt.Concateh(um1, newX);

                    double[,] XtX = clt.MatrizMult(clt.MatrizTransp(newX), newX);
                    double[,] invXtX = clt.MatrizInversa(XtX);
                    double[,] Xty = clt.MatrizMult(clt.MatrizTransp(newX), newY);
                    double[,] beta = clt.MatrizMult(invXtX, Xty);
                    double[,] y_hat = clt.MatrizMult(newX, beta);
                    double[,] erro = clt.MatrizSubtracao(newY, y_hat);
                    double SQE = (clt.MatrizMult(clt.MatrizTransp(erro), erro))[0, 0];
                    double ybarravalor = clt.Meanc(newY)[0, 0];
                    double[,] ybarra = new double[newX.GetLength(0), 1];

                    for (int j = 0; j < newX.GetLength(0); j++)
                        ybarra[j, 0] = ybarravalor;

                    double[,] Miii = clt.MatrizSubtracao(y_hat, ybarra);
                    double SQM = (clt.MatrizMult(clt.MatrizTransp(Miii), Miii))[0, 0];
                    double SQT = SQE + SQM;
                    double R2 = SQM / SQT;

                    matrizbasefatoresprincipais[i, i] = R2;
                    comunalidades_iniciais[i, 0] = R2;
                } // for

                clt.AutovaloresMatrizSimetrica(matrizbasefatoresprincipais, ref autovetor, ref autovalorD);
                for (int t = 0; t < autovalorD.GetLength(0); t++)
                {
                    if (autovalorD[t] <= 0.0)
                    {
                        for (int k = 0; k < matrizbasefatoresprincipais.GetLength(0); k++)
                        {
                            matrizbasefatoresprincipais[k, k] = 1;
                            comunalidades_iniciais[k, 0] = 1;
                            priori_comunalidades = true;
                        } // for
                        break;
                    } // if
                } // for

                for (int i = 0; i < nummaxiterações; i++)
                {
                    clt.AutovaloresMatrizSimetrica(matrizbasefatoresprincipais, ref autovetor, ref autovalorD);

                    for (int f = 0; f < numcomponentes; f++)
                    {
                        autovalorDcorte[f] = autovalorD[f];
                        for (int j = 0; j < X.GetLength(1); j++)
                            autovetorcorte[j, f] = autovetor[j, f];
                    } // for

                    for (int f = 0; f < autovetorcorte.GetLength(0); f++)
                        for (int j = 0; j < autovetorcorte.GetLength(1); j++)
                            rij[f, j] = ((autovetorcorte[f, j]) * (Math.Sqrt(autovalorDcorte[j]))) / (Math.Sqrt(matrizbase[f, f]));

                    hi2 = clt.Zeros(p, 1);
                    for (int f = 0; f < p; f++)
                        for (int j = 0; j < numcomponentes; j++)
                            hi2[f, 0] += Math.Pow(rij[f, j], 2);

                    for (int f = 0; f < X.GetLength(1); f++)
                        matrizbasefatoresprincipais[f, f] = hi2[f, 0];

                    if (i > 0)
                    {
                        delta = clt.MatrizSubtracao(hi2, hi2antigo);
                        somadelta = clt.MatrizMult(clt.MatrizTransp(delta), delta)[0, 0];
                        if (somadelta < 0.00001) break;
                    } // if

                    hi2antigo = clt.ArrayDoubleClone(hi2);
                    numero_iterações_fatoresprincipais++;

                    for (int y = 0; y < hi2antigo.GetLength(0); y++)
                        if (hi2antigo[y, 0] > 1.0)
                            erro_comunalidade_maior_que_1 = true;

                    if (erro_comunalidade_maior_que_1) break;
                } // for

                double[,] teste1 = clt.MatrizMult(rij, clt.MatrizTransp(rij));
                ksichapeu = clt.MatrizSubtracao(matrizbase, teste1);

                for (int i = 0; i < p; i++)
                    diagonalksichapeu[i, 0] = ksichapeu[i, i];
            } // if

            #endregion


            #endregion

            #region Estimação dos Escores
            double[,] ksichapeudiagonalizado = clt.MatrizDiagonal(diagonalksichapeu);

            double[,] temp1a = new double[numcomponentes, X.GetLength(1)];
            double[,] temp2a = new double[numcomponentes, numcomponentes];
            double[,] temp3a = new double[numcomponentes, X.GetLength(0)];
            double[,] escore = new double[numcomponentes, X.GetLength(0)];
            double[,] temp4a = new double[X.GetLength(1), X.GetLength(1)];
            matrizXnormalizada = clt.Standardizec(X);

            if (cmbtipoescore == "Mínimos Quadrados Ponderados")
            {
                temp1a = clt.MatrizMult(clt.MatrizTransp(rij), clt.MatrizInversa(ksichapeudiagonalizado));
                temp2a = clt.MatrizMult(temp1a, rij);
                temp3a = clt.MatrizMult(temp1a, clt.MatrizTransp(matrizXnormalizada));
                escore = clt.MatrizMult(clt.MatrizInversa(temp2a), temp3a);
                double[,] temp52a = clt.MatrizMult(clt.MatrizInversa(temp2a), temp1a);
                double[,] temp55a = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(rij), clt.MatrizInversa(ksichapeudiagonalizado)), rij)), clt.MatrizMult(clt.MatrizTransp(rij), clt.MatrizInversa(ksichapeudiagonalizado)));
                escore = clt.MatrizTransp(escore);
            } // if

            if (cmbtipoescore == "Regressão")
            {
                temp4a = clt.MatrizSoma(clt.MatrizMult(rij, clt.MatrizTransp(rij)), ksichapeudiagonalizado);
                escore = clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(rij), clt.MatrizInversa(temp4a)), clt.MatrizTransp(matrizXnormalizada));
                escore = clt.MatrizTransp(escore);
            } // if

            #endregion

            #region Matriz Residual
            double[,] matrizresidual = new double[p, p];
            double[,] temp1 = clt.MatrizSoma(clt.MatrizMult(rij, clt.MatrizTransp(rij)), ksichapeudiagonalizado);

            matrizresidual = clt.MatrizSubtracao(matrizbase, temp1);

            #endregion

            #region percentuais

            double soma = 0;
            string[] labelsautovalores = new string[p];

            percentuais = new double[X.GetLength(1)];

            for (int i = 0; i < X.GetLength(1); i++)
                soma += autovalorD[i];

            for (int i = 0; i < X.GetLength(1); i++)
                percentuais[i] = autovalorD[i] / soma;

            double[] p_acumulado = new double[X.GetLength(1)];
            p_acumulado[0] = percentuais[0];

            for (int i = 1; i < X.GetLength(1); i++)
                p_acumulado[i] = percentuais[i] + p_acumulado[i - 1];

            double[,] tabela1 = new double[X.GetLength(1), 3];
            string[] n_tabela1 = new string[3];
            n_tabela1[0] = "Autovalor";
            n_tabela1[1] = "Proporção da Variação";
            n_tabela1[2] = "Proporção Acumulada";

            for (int i = 0; i < X.GetLength(1); i++)
            {
                tabela1[i, 0] = autovalorD[i];
                tabela1[i, 1] = percentuais[i];
                tabela1[i, 2] = p_acumulado[i];
            } // for

            #endregion

            #region teste bartlet

            double T1 = 0;
            double T2 = 0;
            double Tb = 0;
            double GLqui = 0;
            double pvaluebartlet = 0;
            double[] autovalorbarlet = new double[p];
            double[,] autovetorbarlet = new double[p, p];

            if (ckbbartlet)
            {
                clt.AutovaloresMatrizSimetrica(correlmatrix, ref autovetorbarlet, ref autovalorbarlet);

                T1 = (-1) * (X.GetLength(0) - ((1.0 / 6.0) * (2 * (X.GetLength(1) + 11))));

                for (int i = 0; i < X.GetLength(1); i++)
                    T2 += Math.Log(autovalorbarlet[i], Math.E);
                Tb = T1 * T2;

                GLqui = (1.0 / 2.0) * (X.GetLength(1)) * (X.GetLength(1) - 1);

                ChiSquared chidist = new ChiSquared(GLqui);
                pvaluebartlet = 1.0 - chidist.CumulativeDistribution(Tb);
            } // if

            #endregion

            #region Rotações

            double al = 1;
            double[,] T = new double[numcomponentes, numcomponentes];
            double[,] A = clt.ArrayDoubleClone(rij);
            T = clt.Identity(numcomponentes);
            double[,] L = clt.MatrizMult(A, T);
            double Q = 0;
            double[,] Gq = new double[0, 0];
            double[,] M = new double[numcomponentes, numcomponentes];
            double[,] S = new double[numcomponentes, numcomponentes];
            double[,] Gp = new double[numcomponentes, numcomponentes];
            double[,] Xis = new double[numcomponentes, numcomponentes];
            double[,] Tt = new double[numcomponentes, numcomponentes];
            double s;
            double[,] u = new double[0, 0];
            double[,] d = new double[0, 0];
            double[,] v = new double[numcomponentes, 1];
            double[,] Lh = new double[rij.GetLength(0), rij.GetLength(1)];

            if (ckbrotacao)
            {

                if (cmbtiporotation == "Quartimax" || cmbtiporotation == "Varimax" || cmbtiporotation == "Entropia Mínima")
                {
                    #region Rotação Orthogonal
                    //start GPForth;
                    this.vgq(L, ref Q, ref Gq, cmbtiporotation);
                    double ft = Q;
                    double[,] G = clt.MatrizMult(clt.MatrizTransp(A), Gq);

                    for (int i = 0; i < 501; i++)
                    {
                        M = clt.MatrizMult(clt.MatrizTransp(T), G);
                        S = clt.MatrizMult(0.5, (clt.MatrizSoma(M, clt.MatrizTransp(M))));
                        Gp = clt.MatrizSubtracao(G, clt.MatrizMult(T, S));
                        s = Math.Sqrt(clt.Trace(clt.MatrizMult(clt.MatrizTransp(Gp), Gp)));

                        if (s < 0.00001) break;

                        al = 2.0 * al;

                        for (int j = 0; j < 11; j++)
                        {
                            Xis = clt.MatrizSubtracao(T, clt.MatrizMult(al, Gp));
                            clt.SingularValueDecomposition(ref d, ref u, ref v, Xis);
                            Tt = clt.MatrizMult(u, clt.MatrizTransp(v));
                            L = clt.MatrizMult(A, Tt);
                            this.vgq(L, ref Q, ref Gq, cmbtiporotation);
                            if (Q < ft - (0.5 * s * s * al)) break;
                            else al = al / 2.0;
                        } // for

                        T = clt.ArrayDoubleClone(Tt);
                        ft = Q;
                        G = clt.MatrizMult(clt.MatrizTransp(A), Gq);
                    } // for

                    Lh = clt.MatrizMult(A, T);
                    #endregion
                } // if
                else
                {
                    #region Rotação Oblique

                    /*start GPFoblq;*/
                    double[,] Ti = clt.MatrizInversa(T);
                    L = clt.MatrizMult(A, clt.MatrizTransp(Ti));
                    this.vgq(L, ref Q, ref Gq, cmbtiporotation);
                    double ft = Q;
                    double[,] G = clt.MatrizMult(-1.0, clt.MatrizTransp((clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(L), Gq), Ti))));
                    for (int i = 0; i < 501; i++)
                    {
                        double[,] TG = clt.MatrizDotMult(T, G);
                        double[,] temp222 = clt.MatrizTransp(clt.Meanc(TG));
                        double[,] temp2222 = (clt.MatrizMult(T, clt.MatrizDiagonal((clt.MatrizMult(TG.GetLength(0), temp222)))));
                        Gp = clt.MatrizSubtracao(G, temp2222);
                        s = Math.Sqrt(clt.Trace(clt.MatrizMult(clt.MatrizTransp(Gp), Gp)));
                        if (s < 0.00001) break;

                        al = 2.0 * al;
                        for (int j = 0; j < 11; j++)
                        {
                            Xis = clt.MatrizSubtracao(T, clt.MatrizMult(al, Gp));
                            double[,] X2 = clt.MatrizDotMult(Xis, Xis);
                            double[,] somacolunas = (clt.MatrizMult(X2.GetLength(0), clt.MatrizTransp(clt.Meanc(X2))));
                            double[,] temp = clt.MatrizDotPower(somacolunas, 0.5);
                            for (int ii = 0; ii < temp.GetLength(0); ii++)
                                for (int jj = 0; jj < temp.GetLength(1); jj++)
                                    v[ii, jj] = 1.0 / temp[ii, jj];
                            Tt = clt.MatrizMult(Xis, clt.MatrizDiagonal(v));
                            Ti = clt.MatrizInversa(Tt);
                            L = clt.MatrizMult(A, clt.MatrizTransp(Ti));
                            this.vgq(L, ref Q, ref Gq, cmbtiporotation);
                            if (Q < ft - (0.5 * s * s * al)) break;
                            else al = al / 2.0;
                        } // for

                        T = clt.ArrayDoubleClone(Tt);
                        ft = Q;
                        G = clt.MatrizMult(-1.0, clt.MatrizTransp((clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(L), Gq), Ti))));
                    } // for
                    Lh = clt.MatrizMult(A, T);
                    #endregion
                } // else
            } // if

            #endregion

            #region gerando o output para resultado das estimações

            object[,] ncomp_princ = new object[X.GetLength(1), 1];

            for (int i = 0; i < X.GetLength(1); i++)
                ncomp_princ[i, 0] = VariaveisIndependentes[i];

            object[,] comp_princ = clt.Concateh(ncomp_princ, autovetorcorte);

            string[] nomes_componentes2 = new string[autovalorDcorte.GetLength(0) + 1];
            nomes_componentes2[0] = "Variável";
            for (int i = 0; i < (autovalorDcorte.GetLength(0)); i++)
                nomes_componentes2[i + 1] = "Autovetor " + (i + 1);

            string out_text = "========================================================================================================================\n\n";

            out_text += "Análise Fatorial \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
            out_text += "Número de Variáveis: " + X.GetLength(1) + "\n";
            out_text += "\n\n";

            out_text += "========== Analise Fatorial\n\n";
            out_text += GeraTabelaNovasVariaveis(comp_princ, nomes_componentes2);

            out_text += "\n\n";
            out_text += "========== Autovalores e Percentual de Variância Explicada";
            out_text += "\n\n";

            out_text += GeraTabelaNovasVariaveis_tabelanumerica(tabela1, n_tabela1);

            if (ckbvarcovar)
            {
                out_text += "\n\n";
                out_text += "========== Matriz de Variância e Covariância das Variáveis";
                out_text += "\n\n";
                varcovar = clt.CovSampleMatrix(X);
                out_text += GeraTabelaCovMatrix(varcovar, VariaveisIndependentes, false);
            } // if

            if (ckbcorrel)
            {
                out_text += "\n\n";
                out_text += "========== Matriz de Correlação das Variáveis";
                out_text += "\n\n";
                out_text += GeraTabelaCovMatrix(correlmatrix, VariaveisIndependentes, false);
            } // if

            if (ckbapresentainversacorrel)
            {
                string[] nomesvazios = new string[X.GetLength(1)];
                for (int i = 0; i < X.GetLength(1); i++)
                    nomesvazios[i] = "";

                double[,] inversacorrel = new double[X.GetLength(1), X.GetLength(1)];
                inversacorrel = clt.MatrizInversa(correlmatrix);
                out_text += "\n\n";
                out_text += "========== Inversa da Matriz de Correlação das Variáveis";
                out_text += "\n\n";
                out_text += GeraTabelaCovMatrix(inversacorrel, nomesvazios, false);
            } // if

            if (ckbbartlet)
            {
                out_text += "\n\n";
                out_text += "========== Teste de Bartlet - testa se as variáveis são independentes";
                out_text += "\n\n";
                out_text += "H0: Matriz de correlação é matriz identidade";
                out_text += "\n\n";

                out_text += "Estatística T do teste: " + clt.Double2Texto(Tb, 6);
                out_text += "\n";
                out_text += "P-valor: " + clt.Double2Texto(pvaluebartlet, 6);
            } // if

            out_text += "\n\n";
            out_text += "========== Comunalidades";
            out_text += "\n\n";
            string[] nomes_comunalidade = new string[3];
            nomes_comunalidade[0] = "Variável (zi)";
            nomes_comunalidade[1] = "Inicial";
            nomes_comunalidade[2] = "Estimada";
            object[,] saida_comunalidades = new object[VariaveisIndependentes.GetLength(0), 1];
            for (int i = 0; i < VariaveisIndependentes.GetLength(0); i++)
                saida_comunalidades[i, 0] = VariaveisIndependentes[i];

            saida_comunalidades = clt.Concateh(saida_comunalidades, comunalidades_iniciais);
            saida_comunalidades = clt.Concateh(saida_comunalidades, hi2);

            out_text += GeraTabelaNovasVariaveis(saida_comunalidades, nomes_comunalidade);
            out_text += "\n";
            if (cmbtipoestimacao == "Fatores Principais")
            {
                out_text += "Número de iterações: " + numero_iterações_fatoresprincipais;
                out_text += "\n\n";
                if (priori_comunalidades)
                    out_text += "O uso do método SMC para inicialização das comunalidades reportou autovalores negativos.\nNesse caso, atribuiu-se valor 1 às comunalidades iniciais.";
                else
                    out_text += "As comunalidades iniciais do processo iterativo foram encontradas com o uso do coeficiente de determinação da regressão das variáveis.\n\n";
            } // if

            string[] nomes_componentes = new string[autovalorDcorte.GetLength(0) + 3];
            object[,] saidacorrelvarcomp = new object[rij.GetLength(0), 1];
            if (ckbestimacoef)
            {
                nomes_componentes[0] = "Variável (zi)";
                for (int i = 0; i < (autovalorDcorte.GetLength(0)); i++)
                    nomes_componentes[i + 1] = "Fator " + (i + 1);

                nomes_componentes[autovalorDcorte.GetLength(0) + 1] = "Comunalidade";
                nomes_componentes[autovalorDcorte.GetLength(0) + 2] = "Unicidade";

                for (int i = 0; i < VariaveisIndependentes.GetLength(0); i++)
                    saidacorrelvarcomp[i, 0] = VariaveisIndependentes[i];

                saidacorrelvarcomp = clt.Concateh(saidacorrelvarcomp, rij);
                saidacorrelvarcomp = clt.Concateh(saidacorrelvarcomp, hi2);
                saidacorrelvarcomp = clt.Concateh(saidacorrelvarcomp, diagonalksichapeu);

                out_text += "\n\n";
                out_text += "========== Estimação de Coeficientes sem Rotação";
                out_text += "\n\n";

                out_text += GeraTabelaNovasVariaveis(saidacorrelvarcomp, nomes_componentes);
            } // if

            if (erro_comunalidade_maior_que_1)
            {
                out_text += "\n\n";
                out_text += "Erro! Comunalidade estimada maior que 1. Caso de Heywood (Rencher, 2002).\n";
                out_text += "O processo iterativo para estimação de coeficientes via Fatores Principais foi abortado.";
                out_text += "\n\n";
            } // if

            if (ckbrotacao & ckbestimacoef)
            {
                string[] nomes_componentesrot = new string[autovalorDcorte.GetLength(0) + 1];
                object[,] saidacorrelvarcomprot = new object[rij.GetLength(0), 1];

                nomes_componentesrot[0] = "Variável (zi)";
                for (int i = 0; i < (autovalorDcorte.GetLength(0)); i++)
                    nomes_componentesrot[i + 1] = "Fator " + (i + 1);

                for (int i = 0; i < VariaveisIndependentes.GetLength(0); i++)
                    saidacorrelvarcomprot[i, 0] = VariaveisIndependentes[i];

                saidacorrelvarcomprot = clt.Concateh(saidacorrelvarcomprot, Lh);

                out_text += "\n\n";
                out_text += "========== Estimação de Coeficientes com Rotação";
                out_text += "\n\n";

                out_text += GeraTabelaNovasVariaveis(saidacorrelvarcomprot, nomes_componentesrot);
            } // if

            string[] nomesmatrizresidual = new string[matrizresidual.GetLength(1)];
            for (int i = 0; i < matrizresidual.GetLength(1); i++)
                nomesmatrizresidual[i] = "";

            if (ckbmatrizresidual)
            {
                out_text += "\n\n";
                out_text += "========== Matriz Residual";
                out_text += "\n\n";
                out_text += GeraTabelaMatrix(matrizresidual);
            } // if

            this.m_output_text = out_text;

            #endregion

            #region adicionando variveis base de dados

            double[,] observacoes = new double[X.GetLength(0), 1];
            double[,] variaveis_geradas = new double[0, 0];
            string[] nomes_variaveis = new string[numcomponentes + 1];

            if (ckbescore)
            {
                for (int i = 0; i < X.GetLength(0); i++)
                {
                    observacoes[i, 0] = (double)i + 1.0;
                }

                nomes_variaveis[0] = "Observacao";

                for (int i = 0; i < (autovalorDcorte.GetLength(0)); i++)
                    nomes_variaveis[i + 1] = "Fator " + (i + 1);

                variaveis_geradas = clt.Concateh(observacoes, escore);

                m_output_variaveis_geradas = "============================================================================================================================\n\n";
                m_output_variaveis_geradas += "Escores dos Fatores\n\n";
                m_output_variaveis_geradas += "Estimação via " + cmbtipoescore + "\n";
                m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
                m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);
                AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);
            } // if
            #endregion
        } // EstimarAnaliseFatorial()

        public void vgq(double[,] L, ref double Q, ref double[,] Gq, string Tiporotacao)
        {
            clsUtilTools clt = new clsUtilTools();

            if (Tiporotacao == "Quartimax")
            {
                double[,] L2 = clt.MatrizDotMult(L, L);
                Q = -0.25 * (clt.Trace(clt.MatrizMult(clt.MatrizTransp(L2), L2)));
                Gq = clt.MatrizMult(-1.0, clt.MatrizDotMult(clt.MatrizDotMult(L, L), L));
            } // if

            if (Tiporotacao == "Varimax")
            {
                double[,] L2 = clt.MatrizDotMult(L, L);
                double[,] cm = clt.Meanc(L2);
                double[,] cmm = clt.ArrayDoubleClone(cm);
                for (int i = 1; i < L2.GetLength(0); i++)
                    cmm = clt.Concatev(cmm, cm);
                
                double[,] QL = clt.MatrizSubtracao(L2, cmm);
                Q = -0.25 * (clt.Trace(clt.MatrizMult(clt.MatrizTransp(QL), QL)));
                Gq = clt.MatrizMult(-1.0, clt.MatrizDotMult(L, QL));
            } // if

            if (Tiporotacao == "Entropia Mínima")
            {
                double[,] L2 = clt.MatrizDotMult(L, L);
                Q = (-0.5 * clt.Sum(clt.MatrizDotMult(L2, clt.LogMatriz(L2))));
                Gq = clt.MatrizMult(-1.0, clt.MatrizSoma(clt.MatrizDotMult(L, clt.LogMatriz(L2)), L));
            } // if

            if (Tiporotacao == "Quartimin" | Tiporotacao == "Bi-quartimin" | Tiporotacao == "Covarimin")
            {
                double gamma = 0;
                if (Tiporotacao == "Quartimin") gamma = 0;
                if (Tiporotacao == "Bi-quartimin") gamma = 0.5;
                if (Tiporotacao == "Covarimin") gamma = 1;
                
                double[,] L2 = clt.MatrizDotMult(L, L);
                int k = L.GetLength(1);
                int p = L.GetLength(0);
                double p1 = 1.0 / p;
                double[,] J = new double[p, p];
                for (int i = 0; i < p; i++)
                    for (int j = 0; j < p; j++)
                        J[i, j] = p1;
                
                double[,] N = clt.MatrizSubtracao(clt.Ones(k, k), clt.Identity(k));
                Q = 0.25 * (clt.Sum(clt.MatrizDotMult(L2, clt.MatrizMult(clt.MatrizMult(clt.MatrizSubtracao(clt.Identity(p), clt.MatrizMult(gamma, J)), L2), N))));
                Gq = clt.MatrizDotMult(L, clt.MatrizMult(clt.MatrizMult(clt.MatrizSubtracao(clt.Identity(p), clt.MatrizMult(gamma, J)), L2), N));
            } // if
        } // vgq()
    } // class
} // namespace
