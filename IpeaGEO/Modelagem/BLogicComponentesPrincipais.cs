using System;

using IpeaGeo.RegressoesEspaciais;
using MathNet.Numerics.Distributions;

namespace IpeaGeo.Modelagem
{
    public class BLogicComponentesPrincipais : BLogicBaseModelagem
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

        private bool m_ckbapresentavarcovar = false;
        public bool ckbvarcovar
        {
            get { return m_ckbapresentavarcovar; }
            set { m_ckbapresentavarcovar = value; }
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

        private bool m_ckbapresentacorrelvarcomp = false;
        public bool ckbcorrelvarcomp
        {
            get { return m_ckbapresentacorrelvarcomp; }
            set { m_ckbapresentacorrelvarcomp = value; }
        }

        #endregion

        #region funções de estimação

        public void EstimarComponentesPrincipais(ref double[] percentuais)
        {
            clsUtilTools clt = new clsUtilTools();

            double[,] X = clt.GetMatrizFromDataTable(m_dt_tabela_dados, VariaveisIndependentes);
            double[,] varcovar = new double[X.GetLength(1), X.GetLength(1)];
            double[,] correlmatrix = new double[X.GetLength(1), X.GetLength(1)];

            int p = X.GetLength(1);
            double[,] matrizbase = new double[p, p];

            if (m_CombOx == "Matriz de Correlação")
                matrizbase = clt.CorrSampleMatrix(X);
            else if (m_CombOx == "Matriz de Variância e Covariância")
                matrizbase = clt.CovSampleMatrix(X);

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

            double[,] rij = new double[autovetorcorte.GetLength(0), autovetorcorte.GetLength(1)];
            #region Correlação entre componentes e Variáveis selecionadas;

            for (int i = 0; i < autovetorcorte.GetLength(0); i++)
                for (int j = 0; j < autovetorcorte.GetLength(1); j++)
                    rij[i, j] = ((autovetorcorte[i, j]) * (Math.Sqrt(autovalorDcorte[j]))) / (Math.Sqrt(matrizbase[i, i]));

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

            #region teste bartlet

            double T1 = 0;
            double T2 = 0;
            double T = 0;
            double GLqui = 0;
            double pvaluebartlet = 0;
            double[] autovalorbarlet = new double[p];
            double[,] autovetorbarlet = new double[p, p];

            if (ckbbartlet)
            {
                if ((m_CombOx == "Matriz de Variância e Covariância"))
                    clt.AutovaloresMatrizSimetrica(correlmatrix, ref autovetorbarlet, ref autovalorbarlet);
                else autovalorbarlet = autovalorD;

                T1 = (-1) * (X.GetLength(0) - ((1.0 / 6.0) * (2 * (X.GetLength(1) + 11))));

                for (int i = 0; i < X.GetLength(1); i++)
                    T2 += Math.Log(autovalorbarlet[i], Math.E);
                
                T = T1 * T2;

                GLqui = (1.0 / 2.0) * (X.GetLength(1)) * (X.GetLength(1) - 1);

                ChiSquared chidist = new ChiSquared(GLqui);
                pvaluebartlet = 1.0 - chidist.CumulativeDistribution(T);
            } // if

            #endregion

            object[,] ncomp_princ = new object[X.GetLength(1), 1];
            for (int i = 0; i < X.GetLength(1); i++)
                ncomp_princ[i, 0] = VariaveisIndependentes[i];

            object[,] comp_princ = clt.Concateh(ncomp_princ, autovetorcorte);

            string[] nomes_componentes2 = new string[autovalorDcorte.GetLength(0) + 1];
            nomes_componentes2[0] = "Variável";
            for (int i = 0; i < (autovalorDcorte.GetLength(0)); i++)
                nomes_componentes2[i + 1] = "Componente " + (i + 1);

            #region gerando o output para resultado das estimações

            string out_text = "========================================================================================================================\n\n";

            out_text += "Componentes Principais \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
            out_text += "Número de Variáveis: " + X.GetLength(1) + "\n";
            out_text += "\n\n";

            out_text += "========== Componentes Principais\n\n";

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

            if (ckbbartlet)
            {
                out_text += "\n\n";
                out_text += "========== Teste de Bartlet - testa se as variáveis são independentes";
                out_text += "\n\n";
                out_text += "H0: Matriz de correlação é matriz identidade";
                out_text += "\n\n";

                out_text += "Estatística T do teste: " + clt.Double2Texto(T, 6);
                out_text += "\n";
                out_text += "P-valor: " + clt.Double2Texto(pvaluebartlet, 6);
            } // if

            string[] nomes_componentes = new string[autovalorDcorte.GetLength(0) + 1];
            object[,] saidacorrelvarcomp = new object[rij.GetLength(0), 1];
            if (ckbcorrelvarcomp)
            {
                nomes_componentes[0] = "";
                for (int i = 0; i < (autovalorDcorte.GetLength(0)); i++)
                    nomes_componentes[i + 1] = "Componente " + (i + 1);

                for (int i = 0; i < VariaveisIndependentes.GetLength(0); i++)
                    saidacorrelvarcomp[i, 0] = VariaveisIndependentes[i];

                saidacorrelvarcomp = clt.Concateh(saidacorrelvarcomp, rij);

                out_text += "\n\n";
                out_text += "========== Matriz de Correlação das Variáveis com os Componentes";
                out_text += "\n\n";

                out_text += GeraTabelaNovasVariaveis(saidacorrelvarcomp, nomes_componentes);
            } // if

            double[,] escore = new double[X.GetLength(0), numcomponentes];
            double[,] matrizXnormalizada = new double[X.GetLength(0), X.GetLength(1)];
            if (ckbescore)
            {
                if (m_CombOx == "Matriz de Correlação")
                    matrizXnormalizada = clt.Standardizec(X);
                else
                {
                    double[,] mediax = clt.Meanc(X);
                    for (int i = 0; i < X.GetLength(0); i++)
                        for (int j = 0; j < X.GetLength(1); j++)
                            matrizXnormalizada[i, j] = X[i, j] - mediax[0, j];
                } // else

                escore = clt.MatrizMult(matrizXnormalizada, autovetorcorte);
            } // if

            this.m_output_text = out_text;

            #endregion

            #region adicionando variveis base de dados
            
            double[,] observacoes = new double[X.GetLength(0), 1];

            for (int i = 0; i < X.GetLength(0); i++)
                observacoes[i, 0] = (double)i + 1.0;

            double[,] variaveis_geradas = clt.Concateh(observacoes, escore);
            string[] nomes_variaveis = new string[numcomponentes + 1];

            nomes_variaveis[0] = "Observacao";

            for (int i = 0; i < (autovalorDcorte.GetLength(0)); i++)
                nomes_variaveis[i + 1] = "Componente " + (i + 1);

            m_output_variaveis_geradas = "============================================================================================================================\n\n";
            m_output_variaveis_geradas += "Escores das Componentes Principais\n\n";

            if (m_CombOx == "Matriz de Correlação")
                m_output_variaveis_geradas += "*Os escores apresentados foram calculados com as variáveis padronizadas devido ao uso da matriz de correlação como matriz base\n\n";

            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);
            AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);

            #endregion
        }  // EstimarComponentesPrincipais()

        #endregion 
    } // class
} // namespace
