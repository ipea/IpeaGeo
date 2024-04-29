using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    class BLogicDecisaoMulticriterios : BLogicBaseModelagem
    {
        #region Alterações Pedro

        private string[] strVariaveisSelecionadas;
        private double[] dblPesos;

        /// <summary>
        /// Método para o Cálculo do índice algébrico,
        /// </summary>
        /// <param name="dtDados">Base de dados.</param>
        /// <param name="strVariaveis">Vetor de variáveis selecionadas</param>
        /// <param name="pesos">Vetor com os pesos.</param>
        /// <param name="padronizados">Vetor bool indicando se a variável deve ser padronizada ou não.</param>
        public double[] IndiceAlgebrico(ref DataTable dtDados, string[] strVariaveis, double[] pesos, bool[] padronizados)
        {
            //Passa as variáveis:
            strVariaveisSelecionadas = strVariaveis;

            //Passa os pesos:
            dblPesos = pesos;

            //Cria a matriz de dados.
            double[,] mDados = m_clt.GetMatrizFromDataTable(dtDados, strVariaveisSelecionadas);

            //Normaliza as variáveis
            double[,] media = m_clt.Meanc(mDados);
            double[,] var = m_clt.Varianciac(mDados);
            for (int i = 0; i < padronizados.Length; i++)
            {
                if (padronizados[i])
                {
                    for (int j = 0; j < mDados.GetLength(0); j++)
                    {
                        mDados[j, i] = (mDados[j, i] - media[0, i]) / Math.Sqrt(var[0, i]);
                    }
                }
            }
            
            double[] indice = new double[mDados.GetLength(0)];
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                double soma = 0;
                double somaPeso = 0;
                for (int j = 0; j < mDados.GetLength(1); j++)
                {
                    soma += mDados[i, j] * pesos[j];
                    somaPeso += pesos[j];
                }
                indice[i] = soma / somaPeso;
            }

            //Normaliza
            double min = m_clt.Min(indice);
            double max = m_clt.Max(indice);
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                indice[i] = (indice[i] - min) / (max - min);
            }

            return (indice);
        }

        /// <summary>
        /// Método para o Cálculo do índice Componentes principais,
        /// </summary>
        /// <param name="dtDados">Base de dados.</param>
        /// <param name="strVariaveis">Vetor de variáveis selecionadas</param>
        /// <param name="pesos">Vetor com os pesos.</param>
        /// <param name="padronizados">Vetor bool indicando se a variável deve ser padronizada ou não.</param>
        public double[] IndiceComponentesPrincipais(ref DataTable dtDados, string[] strVariaveis, bool[] padronizados, out double[] autoValores)
        {
            //Passa as variáveis:
            strVariaveisSelecionadas = strVariaveis;

            //Cria a matriz de dados.
            double[,] mDados = m_clt.GetMatrizFromDataTable(dtDados, strVariaveisSelecionadas);

            //Normaliza as variáveis
            double[,] media = m_clt.Meanc(mDados);
            double[,] var = m_clt.Varianciac(mDados);
            for (int i = 0; i < padronizados.Length; i++)
            {
                if (padronizados[i])
                {
                    for (int j = 0; j < mDados.GetLength(0); j++)
                    {
                        mDados[j, i] = (mDados[j, i] - media[0, i]) / Math.Sqrt(var[0, i]);
                    }
                }
            }

            Pearsn m_estatistica7 = new Pearsn();
            Jacobi PrincipalComponents = new Jacobi();
            double[,] matriz_de_Variancia_Covariancia = new double[mDados.GetLength(1), mDados.GetLength(1)];
            matriz_de_Variancia_Covariancia = m_clt.Matriz_De_Covariancia(mDados);
            PrincipalComponents.jacobi(matriz_de_Variancia_Covariancia);
            double[,] Autovetor = PrincipalComponents.Eigenvectors;
            double[] indice = new double[mDados.GetLength(0)];
            double[] Primeiro_Autovetor = new double[mDados.GetLength(1)];
            
            for (int i = 0; i < mDados.GetLength(1); i++)
            {
                Primeiro_Autovetor[i] = Autovetor[i, 0];
            }

            for (int linha = 0; linha < mDados.GetLength(0); linha++)
            {
                for (int coluna = 0; coluna < mDados.GetLength(1); coluna++)
                {
                    indice[linha] += mDados[linha, coluna] * Primeiro_Autovetor[coluna];
                }
            }

            double[,] autoVetores = PrincipalComponents.Eigenvectors;
            autoValores = PrincipalComponents.Eigenvalues;
            double soma = 0;
            for (int i = 0; i < autoValores.Length; i++)
            {
                soma += autoValores[i];
            }
            for (int i = 0; i < autoValores.Length; i++)
            {
                autoValores[i] = autoValores[i] / soma;
            }

            //Normaliza
            double min = m_clt.Min(indice);
            double max = m_clt.Max(indice);
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                indice[i] = (indice[i] - min) / (max - min);
            }

            return (indice);
        }
        
        public string imprimirComponentesPrincipais(double[] dblIndice, string[] strVariavelID, string strNomeDoMetodo, string strVariavelIdentificadora, string[] strVariaveisSelecionadas, double[] dblPesos, bool[] blPadronizado)
        {
            string outText = "\n============================================================================================================================\n\n";

            outText += "Cálculo de índices de apoio a decisão com multicritérios \n";
            outText += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            outText += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";
            outText += "Método: " + strNomeDoMetodo + "\n";
            outText += "Variável identificadora: " + strVariavelIdentificadora + "\n";
            outText += "Número de variáveis: " + m_clt.Double2Texto(strVariaveisSelecionadas.Length, 0) + "\n";

            outText += "\n============================================================================================================================\n\n";

            int max1 = maximoLetras(strVariaveisSelecionadas);
            int max2 = maximoLetras(strVariavelID);
            int max = Math.Max(max1, max2);
            max = Math.Max(max, 23);

            outText += "Variáveis" + PreencheEspacos(max - 9) + "\tPeso" + PreencheEspacos(20) + "\tPadronizado\r\n\n";
            for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
            {
                string strPadr = "Não";
                if (blPadronizado[i]) strPadr = "Sim";
                outText += strVariaveisSelecionadas[i] + PreencheEspacos(max - strVariaveisSelecionadas[i].Length) + "\t" + dblPesos[i].ToString() + PreencheEspacos(24 - dblPesos[i].ToString().Length) + "\t" + strPadr + "\r\n\n";
            }
            outText += "\n============================================================================================================================\n\n";
            outText += "Variável Identificadora" + PreencheEspacos(max - 23) + "\tÍndice Componentes Principais\r\n\n";
            for (int i = 0; i < dblIndice.Length; i++)
            {
                outText += strVariavelID[i] + PreencheEspacos(max - strVariavelID[i].Length) + "\t" + dblIndice[i].ToString() + "\r\n\n";
            }
            return (outText);
        }

        private int maximoLetras(string[] vetor)
        {
            int imaximo = int.MinValue;
            for (int i = 0; i < vetor.Length; i++)
            {
                if (imaximo < vetor[i].Length)
                {
                    imaximo = vetor[i].Length;
                }
            }
            return (imaximo);
        }

        public string imprimirAlgebrico(double[] dblIndice, string[] strVariavelID, string strNomeDoMetodo, string strVariavelIdentificadora, string[] strVariaveisSelecionadas, double[] dblPesos, bool[] blPadronizado)
        {
            string outText = "\n============================================================================================================================\n\n";

            outText += "Cálculo de índices de apoio a decisão com multicritérios \n";
            outText += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            outText += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";
            outText += "Método: " + strNomeDoMetodo + "\n";
            outText += "Variável identificadora: " + strVariavelIdentificadora + "\n";
            outText += "Número de variáveis: " + m_clt.Double2Texto(strVariaveisSelecionadas.Length, 0) + "\n";

            outText += "\n============================================================================================================================\n\n";

            int max1 = maximoLetras(strVariaveisSelecionadas);
            int max2 = maximoLetras(strVariavelID);
            int max = Math.Max(max1, max2);
            max = Math.Max(max, 23);

            outText += "Variáveis" + PreencheEspacos(max - 9) + "\tPeso\tPadronizado\r\n\n";
            for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
            {
                string strPadr = "Não";
                if (blPadronizado[i]) strPadr = "Sim";
                outText += strVariaveisSelecionadas[i] + PreencheEspacos(max - strVariaveisSelecionadas[i].Length) + "\t" + dblPesos[i].ToString() + "\t" + strPadr + "\r\n\n";
            }
            outText += "\n============================================================================================================================\n\n";
            outText += "Variável Identificadora" + PreencheEspacos(max - 23) + "\tÍndice Algébrico\r\n\n";
            for (int i = 0; i < dblIndice.Length; i++)
            {
                outText += strVariavelID[i] + PreencheEspacos(max - strVariavelID[i].Length) + "\t" + dblIndice[i].ToString() + "\r\n\n";
            }
            return (outText);
        }

        /// <summary>
        /// Cálculo do índice usando Análise Hierárquica Clássica
        /// </summary>
        /// <param name="dtDados">DataTable com os dados.</param>
        /// <param name="dtView">DataGridView</param>
        /// <param name="strVariaveis">Variáveis selecionadas</param>
        /// <param name="lambdaMAX">Valor do LambdaMAX para o calculo da inconsistencia</param>
        /// <param name="RC">Indice de inconsistencia RC</param>
        /// <param name="RI">Indice de inconsistencia RI</param>
        /// <returns></returns>
        public double[] IndiceAnaliseHierarquicaClassica(ref DataTable dtDados, DataGridView dtView, string[] strVariaveis, ref double[,] pesosNormalizados, out double lambda, out double IC, out double RC)
        {
            //Passo 1: Captura os pesos
            double[,] pesos = new double[strVariaveis.Length, strVariaveis.Length];
            int linha = 0;
            int coluna = 1;
            int colunaInicio = 1;

            for (int i = 0; i < dtView.RowCount; i++)
            {
                double valor = Convert.ToDouble(dtView.Rows[i].Cells[1].Value);
                if (Convert.ToDouble(valor) < 0)
                {
                    pesos[linha, coluna] = Math.Abs(Convert.ToDouble(dtView.Rows[i].Cells[1].Value));
                    pesos[coluna, linha] = 1.0 / pesos[linha, coluna];
                }
                else if (Convert.ToDouble(valor) > 0)
                {
                    pesos[linha, coluna] = 1.0 / Math.Abs(Convert.ToDouble(dtView.Rows[i].Cells[1].Value));
                    pesos[coluna, linha] = 1.0 / pesos[linha, coluna];
                }
                else
                {
                    pesos[linha, coluna] = pesos[coluna, linha] = 1;
                }
                coluna++;
                if (coluna == strVariaveis.Length)
                {
                    linha++;
                    coluna = colunaInicio + 1;
                }
                if (i < pesos.GetLength(0))
                {
                    pesos[i, i] = 1;
                }
            }

            //Passo 2: Encontra os autovalores da matriz normalizada aproximados.
            //Passo 2.1: Normaliza as colunas.
            //double[,] pesosNormalizados = new double[pesos.GetLength(0), pesos.GetLength(1)];
            double[] somas = new double[pesos.GetLength(0)];
            for (int i = 0; i < somas.GetLength(0); i++)
            {
                for (int j = 0; j < somas.GetLength(0); j++)
                {
                    somas[i] += pesos[j, i];
                }
            }
            for (int i = 0; i < somas.GetLength(0); i++)
            {
                for (int j = 0; j < somas.GetLength(0); j++)
                {
                    pesosNormalizados[j, i] = pesos[j, i] / somas[i];
                }
            }
            //Passo 2.2: Calcula autovalores aproximados
            double[,] autoValores = new double[pesos.GetLength(0), 1];
            autoValores = m_clt.Meanl(pesosNormalizados);
            //Cria a matriz de dados.
            double[,] mDados = m_clt.GetMatrizFromDataTable(dtDados, strVariaveis);
            double[] indice = new double[mDados.GetLength(0)];

            //Passo 3: Calcula os indices
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                for (int j = 0; j < mDados.GetLength(1); j++)
                {
                    indice[i] = mDados[i, j] * autoValores[j, 0];
                }
            }

            //Calcula os indices de inconsistencia
            double[,] lambdaMAX = m_clt.MatrizMult(pesos, autoValores);
            lambda = 0;
            for (int i = 0; i < lambdaMAX.GetLength(0); i++)
            {
                lambda += (lambdaMAX[i, 0] / autoValores[i, 0]);
            }
            lambda = lambda / Convert.ToDouble(pesos.GetLength(0));

            IC = (lambda - Convert.ToDouble(pesos.GetLength(0))) / (Convert.ToDouble(pesos.GetLength(0)) - 1);

            if (pesos.GetLength(0) <= 2)
            {
                RC = IC;
            }
            else if (pesos.GetLength(0) <= 3)
            {
                RC = IC / 0.58;
            }
            else if (pesos.GetLength(0) <= 4)
            {
                RC = IC / 0.9;
            }
            else if (pesos.GetLength(0) <= 5)
            {
                RC = IC / 1.12;
            }
            else if (pesos.GetLength(0) <= 6)
            {
                RC = IC / 1.24;
            }
            else
            {
                RC = IC / 1.32;
            }

            //Normaliza
            double min = m_clt.Min(indice);
            double max = m_clt.Max(indice);
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                indice[i] = (indice[i] - min) / (max - min);
            }

            return (indice);
        }

        public string imprimirHierarquicoClassico(double[] dblIndice, string[] strVariavelID, string strNomeDoMetodo, string strVariavelIdentificadora, string[] strVariaveisSelecionadas, double[,] dblPesos, double lambdaMax, double IC, double RC)
        {
            string outText = "\n============================================================================================================================\n\n";

            outText += "Cálculo de índices de apoio a decisão com multicritérios \n";
            outText += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            outText += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";
            outText += "Método: " + strNomeDoMetodo + "\n";
            outText += "Variável identificadora: " + strVariavelIdentificadora + "\n";
            outText += "Número de variáveis: " + m_clt.Double2Texto(strVariaveisSelecionadas.Length, 0) + "\n";

            outText += "\n============================================================================================================================\n\n";

            int max1 = maximoLetras(strVariaveisSelecionadas);
            int max2 = maximoLetras(strVariavelID);
            int max = Math.Max(max1, max2);
            max = Math.Max(max, 23);

            outText += "Variáveis" + PreencheEspacos(max - 9) + "\tPeso\r\n\n";
            for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
            {
                outText += strVariaveisSelecionadas[i] + PreencheEspacos(max - strVariaveisSelecionadas[i].Length) + "\t" + dblPesos[i, 0].ToString() + "\r\n\n";
            }

            outText += "\n============================================================================================================================\n\n";
            outText += "Índice de consistência\r\n\n";
            outText += "Lambda Max" + PreencheEspacos(22 - 10) + lambdaMax.ToString() + "\t\r\n\n";
            outText += "IC" + PreencheEspacos(22 - 2) + IC.ToString() + "\t\r\n\n";
            outText += "RC" + PreencheEspacos(22 - 2) + RC.ToString() + "\t\r\n\n";

            outText += "\n============================================================================================================================\n\n";
            outText += "Variável Identificadora" + PreencheEspacos(max - 23) + "\t" + strNomeDoMetodo + "\r\n\n";
            for (int i = 0; i < dblIndice.Length; i++)
            {
                outText += strVariavelID[i] + PreencheEspacos(max - strVariavelID[i].Length) + "\t" + dblIndice[i].ToString() + "\r\n\n";
            }
            
            return (outText);
        }

        /// <summary>
        /// Cálculo do índice usando Análise Hierárquica Multiplicativa
        /// </summary>
        /// <param name="dtDados">DataTable com os dados.</param>
        /// <param name="dtView">DataGridView</param>
        /// <param name="strVariaveis">Variáveis selecionadas</param>
        /// <param name="lambdaMAX">Valor do LambdaMAX para o calculo da inconsistencia</param>
        /// <param name="RC">Indice de inconsistencia RC</param>
        /// <param name="RI">Indice de inconsistencia RI</param>
        /// <returns></returns>
        public double[] IndiceAnaliseHierarquicaMultiplicativa(ref DataTable dtDados, DataGridView dtView, string[] strVariaveis, ref double[,] pesosNormalizados, out double lambda, out double IC, out double RC)
        {
            //Passo 1: Captura os pesos
            double[,] pesos = new double[strVariaveis.Length, strVariaveis.Length];
            int linha = 0;
            int coluna = 1;
            int colunaInicio = 1;

            for (int i = 0; i < dtView.RowCount; i++)
            {
                double valor = Convert.ToDouble(dtView.Rows[i].Cells[1].Value);
                if (Convert.ToDouble(valor) < 0)
                {
                    pesos[linha, coluna] = Math.Abs(Convert.ToDouble(dtView.Rows[i].Cells[1].Value));
                    pesos[coluna, linha] = 1.0 / pesos[linha, coluna];
                }
                else if (Convert.ToDouble(valor) > 0)
                {
                    pesos[linha, coluna] = 1.0 / Math.Abs(Convert.ToDouble(dtView.Rows[i].Cells[1].Value));
                    pesos[coluna, linha] = 1.0 / pesos[linha, coluna];
                }
                else
                {
                    pesos[linha, coluna] = pesos[coluna, linha] = 1;
                }
                coluna++;
                if (coluna == strVariaveis.Length)
                {
                    linha++;
                    coluna = colunaInicio + 1;
                }
                if (i < pesos.GetLength(0))
                {
                    pesos[i, i] = 1;
                }
            }

            //Passo 2: Encontra os autovalores da matriz normalizada aproximados.
            //Passo 2.1: Normaliza as colunas.
            //double[,] pesosNormalizados = new double[pesos.GetLength(0), pesos.GetLength(1)];
            double[] somas = new double[pesos.GetLength(0)];
            for (int i = 0; i < somas.GetLength(0); i++)
            {
                for (int j = 0; j < somas.GetLength(0); j++)
                {
                    somas[i] += pesos[j, i];
                }
            }
            for (int i = 0; i < somas.GetLength(0); i++)
            {
                for (int j = 0; j < somas.GetLength(0); j++)
                {
                    pesosNormalizados[j, i] = pesos[j, i] / somas[i];
                }
            }
            
            //Passo 2.2: Calcula autovalores aproximados
            double[,] autoValores = new double[pesos.GetLength(0), 1];
            autoValores = m_clt.GeometricMeanl(pesosNormalizados);
            //Normaliza os autovalores
            double soma = 0;
            for (int i = 0; i < autoValores.GetLength(0); i++)
            {
                soma += autoValores[i, 0];
            }
            for (int i = 0; i < autoValores.GetLength(0); i++)
            {
                autoValores[i, 0] = autoValores[i, 0] / soma;
            }

            //Cria a matriz de dados.
            double[,] mDados = m_clt.GetMatrizFromDataTable(dtDados, strVariaveis);
            double[] indice = new double[mDados.GetLength(0)];

            //Passo 3: Calcula os indices
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                for (int j = 0; j < mDados.GetLength(1); j++)
                {
                    indice[i] = mDados[i, j] * autoValores[j, 0];
                }
            }

            //Calcula os indices de inconsistencia
            double[,] lambdaMAX = m_clt.MatrizMult(pesos, autoValores);
            lambda = 0;
            for (int i = 0; i < lambdaMAX.GetLength(0); i++)
            {
                lambda += (lambdaMAX[i, 0] / autoValores[i, 0]);
            }
            lambda = lambda / Convert.ToDouble(pesos.GetLength(0));

            IC = (lambda - Convert.ToDouble(pesos.GetLength(0))) / (Convert.ToDouble(pesos.GetLength(0)) - 1);

            if (pesos.GetLength(0) <= 2)
            {
                RC = IC;
            }
            else if (pesos.GetLength(0) <= 3)
            {
                RC = IC / 0.58;
            }
            else if (pesos.GetLength(0) <= 4)
            {
                RC = IC / 0.9;
            }
            else if (pesos.GetLength(0) <= 5)
            {
                RC = IC / 1.12;
            }
            else if (pesos.GetLength(0) <= 6)
            {
                RC = IC / 1.24;
            }
            else
            {
                RC = IC / 1.32;
            }

            //Normaliza
            double min = m_clt.Min(indice);
            double max = m_clt.Max(indice);
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                indice[i] = (indice[i] - min) / (max - min);
            }

            return (indice);
        }

        public string imprimirHierarquicoMultiplicativo(double[] dblIndice, string[] strVariavelID, string strNomeDoMetodo, string strVariavelIdentificadora, string[] strVariaveisSelecionadas, double[,] dblPesos, double lambdaMax, double IC, double RC)
        {
            string outText = "\n============================================================================================================================\n\n";

            outText += "Cálculo de índices de apoio a decisão com multicritérios \n";
            outText += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            outText += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";
            outText += "Método: " + strNomeDoMetodo + "\n";
            outText += "Variável identificadora: " + strVariavelIdentificadora + "\n";
            outText += "Número de variáveis: " + m_clt.Double2Texto(strVariaveisSelecionadas.Length, 0) + "\n";

            outText += "\n============================================================================================================================\n\n";

            int max1 = maximoLetras(strVariaveisSelecionadas);
            int max2 = maximoLetras(strVariavelID);
            int max = Math.Max(max1, max2);
            max = Math.Max(max, 23);

            outText += "Variáveis" + PreencheEspacos(max - 9) + "\tPeso\r\n\n";
            for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
            {
                outText += strVariaveisSelecionadas[i] + PreencheEspacos(max - strVariaveisSelecionadas[i].Length) + "\t" + dblPesos[i, 0].ToString() + "\r\n\n";
            }

            outText += "\n============================================================================================================================\n\n";
            outText += "Índice de consistência\r\n\n";
            outText += "Lambda Max" + PreencheEspacos(22 - 10) + lambdaMax.ToString() + "\t\r\n\n";
            outText += "IC" + PreencheEspacos(22 - 2) + IC.ToString() + "\t\r\n\n";
            outText += "RC" + PreencheEspacos(22 - 2) + RC.ToString() + "\t\r\n\n";

            outText += "\n============================================================================================================================\n\n";
            outText += "Variável Identificadora" + PreencheEspacos(max - 23) + "\t" + strNomeDoMetodo + "\r\n\n";
            for (int i = 0; i < dblIndice.Length; i++)
            {
                outText += strVariavelID[i] + PreencheEspacos(max - strVariavelID[i].Length) + "\t" + dblIndice[i].ToString() + "\r\n\n";
            }
            return (outText);
        }

        /// <summary>
        /// Cálculo do índice usando Análise Hierárquica B-G
        /// </summary>
        /// <param name="dtDados">DataTable com os dados.</param>
        /// <param name="dtView">DataGridView</param>
        /// <param name="strVariaveis">Variáveis selecionadas</param>
        /// <param name="lambdaMAX">Valor do LambdaMAX para o calculo da inconsistencia</param>
        /// <param name="RC">Indice de inconsistencia RC</param>
        /// <param name="RI">Indice de inconsistencia RI</param>
        /// <returns></returns>
        public double[] IndiceAnaliseHierarquicaBG(ref DataTable dtDados, DataGridView dtView, string[] strVariaveis, ref double[,] pesosNormalizados, out double lambda, out double IC, out double RC)
        {
            //Passo 1: Captura os pesos
            double[,] pesos = new double[strVariaveis.Length, strVariaveis.Length];
            int linha = 0;
            int coluna = 1;
            int colunaInicio = 1;

            for (int i = 0; i < dtView.RowCount; i++)
            {
                double valor = Convert.ToDouble(dtView.Rows[i].Cells[1].Value);
                if (Convert.ToDouble(valor) < 0)
                {
                    pesos[linha, coluna] = Math.Abs(Convert.ToDouble(dtView.Rows[i].Cells[1].Value));
                    pesos[coluna, linha] = 1.0 / pesos[linha, coluna];
                }
                else if (Convert.ToDouble(valor) > 0)
                {
                    pesos[linha, coluna] = 1.0 / Math.Abs(Convert.ToDouble(dtView.Rows[i].Cells[1].Value));
                    pesos[coluna, linha] = 1.0 / pesos[linha, coluna];
                }
                else
                {
                    pesos[linha, coluna] = pesos[coluna, linha] = 1;
                }
                coluna++;
                if (coluna == strVariaveis.Length)
                {
                    linha++;
                    coluna = colunaInicio + 1;
                }
                if (i < pesos.GetLength(0))
                {
                    pesos[i, i] = 1;
                }
            }

            //Passo 2: Encontra os autovalores da matriz normalizada aproximados.
            //Passo 2.1: Normaliza as colunas.
            //Usa a abordagem B-G (Multiplica a coluna por soma/max(a)
            double[,] maximos = m_clt.Maxc(pesos);
            for (int i = 0; i < pesosNormalizados.GetLength(0); i++)
            {
                for (int j = 0; j < pesosNormalizados.GetLength(0); j++)
                {
                    pesosNormalizados[j, i] = pesos[j, i] / maximos[0,i];
                }
            }

            //Passo 2.2: Calcula autovalores aproximados
            double[,] autoValores = new double[pesos.GetLength(0), 1];
            autoValores = m_clt.Meanl(pesosNormalizados);

            //Normaliza autovalores
            autoValores = m_clt.Normalizac(pesosNormalizados);

            //Cria a matriz de dados.
            double[,] mDados = m_clt.GetMatrizFromDataTable(dtDados, strVariaveis);
            double[] indice = new double[mDados.GetLength(0)];

            //Passo 3: Calcula os indices
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                for (int j = 0; j < mDados.GetLength(1); j++)
                {
                    indice[i] = mDados[i, j] * autoValores[j, 0];
                }
            }

            //Calcula os indices de inconsistencia
            double[,] lambdaMAX = m_clt.MatrizMult(pesos, autoValores);
            lambda = 0;
            for (int i = 0; i < lambdaMAX.GetLength(0); i++)
            {
                lambda += (lambdaMAX[i, 0] / autoValores[i, 0]);
            }
            lambda = lambda / Convert.ToDouble(pesos.GetLength(0));

            IC = (lambda - Convert.ToDouble(pesos.GetLength(0))) / (Convert.ToDouble(pesos.GetLength(0)) - 1);

            if (pesos.GetLength(0) <= 2)
            {
                RC = IC;
            }
            else if (pesos.GetLength(0) <= 3)
            {
                RC = IC / 0.58;
            }
            else if (pesos.GetLength(0) <= 4)
            {
                RC = IC / 0.9;
            }
            else if (pesos.GetLength(0) <= 5)
            {
                RC = IC / 1.12;
            }
            else if (pesos.GetLength(0) <= 6)
            {
                RC = IC / 1.24;
            }
            else
            {
                RC = IC / 1.32;
            }

            //Normaliza
            double min = m_clt.Min(indice);
            double max = m_clt.Max(indice);
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                indice[i] = (indice[i] - min) / (max - min);
            }

            return (indice);
        }

        public string imprimirHierarquicoBG(double[] dblIndice, string[] strVariavelID, string strNomeDoMetodo, string strVariavelIdentificadora, string[] strVariaveisSelecionadas, double[,] dblPesos, double lambdaMax, double IC, double RC)
        {
            string outText = "\n============================================================================================================================\n\n";

            outText += "Cálculo de índices de apoio a decisão com multicritérios \n";
            outText += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            outText += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";
            outText += "Método: " + strNomeDoMetodo + "\n";
            outText += "Variável identificadora: " + strVariavelIdentificadora + "\n";
            outText += "Número de variáveis: " + m_clt.Double2Texto(strVariaveisSelecionadas.Length, 0) + "\n";

            outText += "\n============================================================================================================================\n\n";

            int max1 = maximoLetras(strVariaveisSelecionadas);
            int max2 = maximoLetras(strVariavelID);
            int max = Math.Max(max1, max2);
            max = Math.Max(max, 23);

            outText += "Variáveis" + PreencheEspacos(max - 9) + "\tPeso\r\n\n";
            for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
            {

                outText += strVariaveisSelecionadas[i] + PreencheEspacos(max - strVariaveisSelecionadas[i].Length) + "\t" + dblPesos[i, 0].ToString() + "\r\n\n";
            }

            outText += "\n============================================================================================================================\n\n";
            outText += "Índice de consistência\r\n\n";
            outText += "Lambda Max" + PreencheEspacos(22 - 10) + lambdaMax.ToString() + "\t\r\n\n";
            outText += "IC" + PreencheEspacos(22 - 2) + IC.ToString() + "\t\r\n\n";
            outText += "RC" + PreencheEspacos(22 - 2) + RC.ToString() + "\t\r\n\n";

            outText += "\n============================================================================================================================\n\n";
            outText += "Variável Identificadora" + PreencheEspacos(max - 23) + "\t" + strNomeDoMetodo + "\r\n\n";
            for (int i = 0; i < dblIndice.Length; i++)
            {
                outText += strVariavelID[i] + PreencheEspacos(max - strVariavelID[i].Length) + "\t" + dblIndice[i].ToString() + "\r\n\n";
            }
            return (outText);
        }

        private double[,] calculaMedidaTendenciaCentral(DataTable dtDados, string strVariavel, string strTipoMedida, bool blPadroniza)
        {
            double[,] dblResultado = new double[1, 1];
            double[,] dblDados = m_clt.GetMatrizFromDataTable(dtDados, strVariavel);
            if (blPadroniza)
            {
                dblDados = m_clt.PadronizaDados(dblDados);
                if (strTipoMedida == "Média")
                {
                    dblResultado = m_clt.Meanc(dblDados);
                }
                else if (strTipoMedida == "Mediana")
                {
                    dblResultado = m_clt.Medianc(dblDados);
                }
                else if (strTipoMedida == "Trimean")
                {
                    dblResultado = m_clt.Trimean(dblDados);
                }
                else if (strTipoMedida == "Midrange")
                {
                    dblResultado = m_clt.Midrange(dblDados);
                }
            }
            else
            {
                if (strTipoMedida == "Média")
                {
                    dblResultado = m_clt.Meanc(dblDados);
                }
                else if (strTipoMedida == "Mediana")
                {
                    dblResultado = m_clt.Medianc(dblDados);
                }
                else if (strTipoMedida == "Trimean")
                {
                    dblResultado = m_clt.Trimean(dblDados);
                }
                else if (strTipoMedida == "Midrange")
                {
                    dblResultado = m_clt.Midrange(dblDados);
                }
            }

            return (dblResultado);
        }

        /// <summary>
        /// Cálculo do índice usando Análise Hierárquica Referenciado
        /// </summary>
        /// <param name="dtDados">DataTable com os dados.</param>
        /// <param name="dtView">DataGridView</param>
        /// <param name="strVariaveis">Variáveis selecionadas</param>
        /// <param name="lambdaMAX">Valor do LambdaMAX para o calculo da inconsistencia</param>
        /// <param name="RC">Indice de inconsistencia RC</param>
        /// <param name="RI">Indice de inconsistencia RI</param>
        /// <returns></returns>
        public double[] IndiceAnaliseHierarquicaReferenciado(ref DataTable dtDados, DataGridView dtView, string[] strVariaveis, ref double[,] pesosNormalizados, out double lambda, out double IC, out double RC)
        {
            //Passo 1: Captura os pesos
            double[,] pesos = new double[strVariaveis.Length, strVariaveis.Length];
            double[,] dblMedidas = new double[dtView.RowCount, 1];
            for (int i = 0; i < dtView.RowCount; i++)
            {
                double[,] resultado = calculaMedidaTendenciaCentral(dtDados, dtView.Rows[i].Cells[0].Value.ToString(), dtView.Rows[i].Cells[1].Value.ToString(), Convert.ToBoolean(dtView.Rows[i].Cells[2].Value));
                dblMedidas[i, 0] = resultado[0,0];
            }

            for (int i = 0; i < dtView.RowCount; i++)
            {
                for (int j = 0; j < dtView.RowCount; j++)
                {
                    pesos[i, j] = dblMedidas[i, 0] / dblMedidas[j, 0];
                }
            }
            
            //Passo 2: Encontra os autovalores da matriz normalizada aproximados.
            //Passo 2.1: Normaliza as colunas.
            //double[,] pesosNormalizados = new double[pesos.GetLength(0), pesos.GetLength(1)];
            double[] somas = new double[pesos.GetLength(0)];
            for (int i = 0; i < somas.GetLength(0); i++)
            {
                for (int j = 0; j < somas.GetLength(0); j++)
                {
                    somas[i] += pesos[j, i];
                }
            }
            for (int i = 0; i < somas.GetLength(0); i++)
            {
                for (int j = 0; j < somas.GetLength(0); j++)
                {
                    pesosNormalizados[j, i] = pesos[j, i] / somas[i];
                }
            }

            //Passo 2.2: Calcula autovalores aproximados
            double[,] autoValores = new double[pesos.GetLength(0), 1];
            autoValores = m_clt.Meanl(pesosNormalizados);
            //Cria a matriz de dados.
            double[,] mDados = m_clt.GetMatrizFromDataTable(dtDados, strVariaveis);
            double[] indice = new double[mDados.GetLength(0)];

            //Passo 3: Calcula os indices
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                for (int j = 0; j < mDados.GetLength(1); j++)
                {
                    indice[i] = mDados[i, j] * autoValores[j, 0];
                }
            }

            //Calcula os indices de inconsistencia
            double[,] lambdaMAX = m_clt.MatrizMult(pesos, autoValores);
            lambda = 0;
            for (int i = 0; i < lambdaMAX.GetLength(0); i++)
            {
                lambda += (lambdaMAX[i, 0] / autoValores[i, 0]);
            }
            lambda = lambda / Convert.ToDouble(pesos.GetLength(0));

            IC = (lambda - Convert.ToDouble(pesos.GetLength(0))) / (Convert.ToDouble(pesos.GetLength(0)) - 1);

            if (pesos.GetLength(0) <= 2)
            {
                RC = IC;
            }
            else if (pesos.GetLength(0) <= 3)
            {
                RC = IC / 0.58;
            }
            else if (pesos.GetLength(0) <= 4)
            {
                RC = IC / 0.9;
            }
            else if (pesos.GetLength(0) <= 5)
            {
                RC = IC / 1.12;
            }
            else if (pesos.GetLength(0) <= 6)
            {
                RC = IC / 1.24;
            }
            else
            {
                RC = IC / 1.32;
            }

            //Normaliza
            double min = m_clt.Min(indice);
            double max = m_clt.Max(indice);
            for (int i = 0; i < mDados.GetLength(0); i++)
            {
                indice[i] = (indice[i] - min) / (max - min);
            }

            return (indice);
        }

        public string imprimirHierarquicoReferenciado(double[] dblIndice, string[] strVariavelID, string strNomeDoMetodo, string strVariavelIdentificadora, string[] strVariaveisSelecionadas, double[,] dblPesos, double lambdaMax, double IC, double RC, string[] strEstatistica, string[] strPadroniza)
        {
            string outText = "\n============================================================================================================================\n\n";

            outText += "Cálculo de índices de apoio a decisão com multicritérios \n";
            outText += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            outText += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";
            outText += "Método: " + strNomeDoMetodo + "\n";
            outText += "Variável identificadora: " + strVariavelIdentificadora + "\n";
            outText += "Número de variáveis: " + m_clt.Double2Texto(strVariaveisSelecionadas.Length, 0) + "\n";

            outText += "\n============================================================================================================================\n\n";

            int max1 = maximoLetras(strVariaveisSelecionadas);
            int max2 = maximoLetras(strVariavelID);
            int max = Math.Max(max1, max2);
            max = Math.Max(max, 23);

            outText += "Variáveis" + PreencheEspacos(max - 9) + "\tPeso"+ PreencheEspacos(20)+"\tEstatística\tPadronizado\r\n\n";
            for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
            {
                outText += strVariaveisSelecionadas[i] + PreencheEspacos(max - strVariaveisSelecionadas[i].Length) + "\t" + dblPesos[i, 0].ToString() + PreencheEspacos(24 - dblPesos[i, 0].ToString().Length) + "\t" + strEstatistica[i] + PreencheEspacos(11 - strEstatistica[i].Length) + "\t" + strPadroniza[i] + "\r\n\n";
            }

            outText += "\n============================================================================================================================\n\n";
            outText += "Índice de consistência\r\n\n";
            outText += "Lambda Max" + PreencheEspacos(22 - 10) + lambdaMax.ToString() + "\t\r\n\n";
            outText += "IC" + PreencheEspacos(22 - 2) + IC.ToString() + "\t\r\n\n";
            outText += "RC" + PreencheEspacos(22 - 2) + RC.ToString() + "\t\r\n\n";

            outText += "\n============================================================================================================================\n\n";
            outText += "Variável Identificadora" + PreencheEspacos(max - 23) + "\t" + strNomeDoMetodo + "\r\n\n";
            for (int i = 0; i < dblIndice.Length; i++)
            {
                outText += strVariavelID[i] + PreencheEspacos(max - strVariavelID[i].Length) + "\t" + dblIndice[i].ToString() + "\r\n\n";
            }
            
            return (outText);
        }

        #region Métodos Prométheé
        
        public double[] IndicePromethee1(ref ProgressBar progressBar1, ref DataTable m_dt_tabela_dados, DataGridView dataGridView1, string[] strVariaveisSelecionadas, out double[] variancias)
        {
            //Passo 1: Cria a matriz de decisão
            string[,] matDecisao = new string[strVariaveisSelecionadas.Length, 6];
            for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
            {
                try
                {
                    matDecisao[i, 0] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    matDecisao[i, 1] = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    matDecisao[i, 2] = Convert.ToString(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    matDecisao[i, 3] = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    matDecisao[i, 4] = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    matDecisao[i, 5] = dataGridView1.Rows[i].Cells[5].Value.ToString();
                }
                catch
                {
                    matDecisao[i, 0] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    matDecisao[i, 1] = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    matDecisao[i, 2] = Convert.ToString(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    matDecisao[i, 3] = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    matDecisao[i, 4] = Convert.ToString(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    matDecisao[i, 0] = "Verdadeiro Critério";
                    matDecisao[i, 1] = "Maximizar";
                }
            }

            //Passo 2: Cria a matriz de dados 
            double[,] mDados = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, strVariaveisSelecionadas);

            //Passo 3: Calcula o índice 
            double[] dblVariancias = new double[strVariaveisSelecionadas.Length];
            double[] indResultado = Promethee_I(mDados, matDecisao, ref progressBar1, out dblVariancias);
            variancias = dblVariancias;
            
            return(indResultado);
        }

        public string imprimirPromethee1(DataGridView dataGridView1, DataTable m_dt_tabela_dados, double[] dblIndice, string[] strVariavelID, string strNomeDoMetodo, string strVariavelIdentificadora, string[] strVariaveisSelecionadas, double[] dblPesos,double[] variancias)
        {
            string outText = "\n============================================================================================================================\n\n";

            outText += "Cálculo de índices de apoio a decisão com multicritérios \n";
            outText += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            outText += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";
            outText += "Método: " + strNomeDoMetodo + "\n";
            outText += "Variável identificadora: " + strVariavelIdentificadora + "\n";
            outText += "Número de variáveis: " + m_clt.Double2Texto(strVariaveisSelecionadas.Length, 0) + "\n";

            outText += "\n============================================================================================================================\n\n";

            int max1 = maximoLetras(strVariaveisSelecionadas);
            int max2 = maximoLetras(strVariavelID);
            int max = Math.Max(max1, max2);
            max = Math.Max(max, 23);

            //COLOCAR AQUI A DIRECAO, OS LIMITES DE PREFRENCIA...
            outText += "Variáveis" + PreencheEspacos(max - 9) + "Peso" + PreencheEspacos(5) + "Direção" + PreencheEspacos(3) + "Limite de Indiferença" + PreencheEspacos(1) + "Limite de preferência" + PreencheEspacos(1) + "Função\r\n\n";
            for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
            {
                string strLimiteIndiferenca=dataGridView1.Rows[i].Cells[4].Value.ToString();
                string strLimitePreferencia=dataGridView1.Rows[i].Cells[3].Value.ToString() ;

                if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Verdadeiro Critério")
                {
                    strLimiteIndiferenca = "----------";
                    strLimitePreferencia = "----------";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Quase Critério")
                {
                    //strLimiteIndiferenca = "";
                    strLimitePreferencia = "----------";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Pseudocritério com preferência linear")
                {
                    strLimiteIndiferenca = "----------";
                    //strLimitePreferencia = "----------";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Critério de Nível")
                {
                    //strLimiteIndiferenca = "";
                    //strLimitePreferencia = "";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Critério com preferência linear")
                {
                    //strLimiteIndiferenca = "";
                    //strLimitePreferencia = "";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Critério Gaussiano")
                {
                    strLimiteIndiferenca = variancias[i].ToString();
                    strLimitePreferencia = variancias[i].ToString();
                }

                outText += strVariaveisSelecionadas[i] + PreencheEspacos(max - strVariaveisSelecionadas[i].Length) + dblPesos[i].ToString() + PreencheEspacos(9 - dblPesos[i].ToString().Length) + dataGridView1.Rows[i].Cells[5].Value.ToString() + PreencheEspacos(1) +strLimiteIndiferenca + PreencheEspacos(22 -strLimiteIndiferenca.Length ) + strLimitePreferencia + PreencheEspacos(22 - strLimitePreferencia.Length) + dataGridView1.Rows[i].Cells[2].Value.ToString() + "\r\n\n";
            }

            outText += "\n============================================================================================================================\n\n";
            outText += "Variável Identificadora" + PreencheEspacos(max - 23) + "\t" + strNomeDoMetodo + "\r\n\n";
            for (int i = 0; i < dblIndice.Length; i++)
            {
                outText += strVariavelID[i] + PreencheEspacos(max - strVariavelID[i].Length) + "\t" + dblIndice[i].ToString() + "\r\n\n";
            }
            return (outText);
        }

        public double[] IndicePromethee2(ref ProgressBar progressBar1, ref DataTable m_dt_tabela_dados, DataGridView dataGridView1, string[] strVariaveisSelecionadas, out double[] variancias)
        {
            //Passo 1: Cria a matriz de decisão
            string[,] matDecisao = new string[strVariaveisSelecionadas.Length, 6];
            for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
            {
                try
                {
                    matDecisao[i, 0] = dataGridView1.Rows[i].Cells[0].Value.ToString(); //Nome da variável
                    matDecisao[i, 1] = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value.ToString()); //Peso
                    matDecisao[i, 2] = Convert.ToString(dataGridView1.Rows[i].Cells[4].Value.ToString()); //Limite Inferior
                    matDecisao[i, 3] = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value.ToString()); //Limite Superior
                    matDecisao[i, 4] = dataGridView1.Rows[i].Cells[2].Value.ToString(); //Tipo de função
                    matDecisao[i, 5] = dataGridView1.Rows[i].Cells[5].Value.ToString(); //Direção
                }
                catch
                {
                    matDecisao[i, 0] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    matDecisao[i, 1] = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    matDecisao[i, 2] = Convert.ToString(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    matDecisao[i, 3] = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    matDecisao[i, 4] = Convert.ToString(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    matDecisao[i, 0] = "Verdadeiro Critério";
                    matDecisao[i, 1] = "Maximizar";
                }

            }

            //Passo 2: Cria a matriz de dados 
            double[,] mDados = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, strVariaveisSelecionadas);

            //Passo 3: Calcula o índice 
            double[] dblVariancias = new double[strVariaveisSelecionadas.Length];
            double[] indResultado = Promethee_II(mDados, matDecisao, ref progressBar1, out dblVariancias);
            variancias = dblVariancias;
            
            return (indResultado);
        }

        public double[] IndicePromethee3(ref ProgressBar progressBar1, ref DataTable m_dt_tabela_dados, DataGridView dataGridView1, string[] strVariaveisSelecionadas, double alfa, out double[] variancias)
        {
            //Passo 1: Cria a matriz de decisão
            string[,] matDecisao = new string[strVariaveisSelecionadas.Length, 6];
            for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
            {
                try
                {
                    matDecisao[i, 0] = dataGridView1.Rows[i].Cells[0].Value.ToString(); //Nome da variável
                    matDecisao[i, 1] = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value.ToString()); //Peso
                    matDecisao[i, 2] = Convert.ToString(dataGridView1.Rows[i].Cells[4].Value.ToString()); //Limite Inferior
                    matDecisao[i, 3] = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value.ToString()); //Limite Superior
                    matDecisao[i, 4] = dataGridView1.Rows[i].Cells[2].Value.ToString(); //Tipo de função
                    matDecisao[i, 5] = dataGridView1.Rows[i].Cells[5].Value.ToString(); //Direção
                }
                catch
                {
                    matDecisao[i, 0] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    matDecisao[i, 1] = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    matDecisao[i, 2] = Convert.ToString(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    matDecisao[i, 3] = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    matDecisao[i, 4] = Convert.ToString(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    matDecisao[i, 0] = "Verdadeiro Critério";
                    matDecisao[i, 1] = "Maximizar";
                }
            }

            //Passo 2: Cria a matriz de dados 
            double[,] mDados = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, strVariaveisSelecionadas);

            //Passo 3: Calcula o índice 
            double[] dblVariancias = new double[strVariaveisSelecionadas.Length];
            double[] indResultado = Promethee_III(mDados, matDecisao, ref progressBar1,alfa, out dblVariancias);
            variancias = dblVariancias;
            return (indResultado);
        }

        public string imprimirPromethee3(DataGridView dataGridView1, DataTable m_dt_tabela_dados, double[] dblIndice, string[] strVariavelID, string strNomeDoMetodo, string strVariavelIdentificadora, string[] strVariaveisSelecionadas, double[] dblPesos, double alfa, double[] variancias)
        {
            string outText = "\n============================================================================================================================\n\n";

            outText += "Cálculo de índices de apoio a decisão com multicritérios \n";
            outText += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            outText += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";
            outText += "Método: " + strNomeDoMetodo + "\n";
            outText += "Variável identificadora: " + strVariavelIdentificadora + "\n";
            outText += "Número de variáveis: " + m_clt.Double2Texto(strVariaveisSelecionadas.Length, 0) + "\n";
            outText += "Número de desvios-padrão: " + alfa.ToString()+ "\n";

            outText += "\n============================================================================================================================\n\n";

            int max1 = maximoLetras(strVariaveisSelecionadas);
            int max2 = maximoLetras(strVariavelID);
            int max = Math.Max(max1, max2);
            max = Math.Max(max, 23);

            //COLOCAR AQUI A DIRECAO, OS LIMITES DE PREFRENCIA...
            outText += "Variáveis" + PreencheEspacos(max - 9) + "Peso" + PreencheEspacos(5) + "Direção" + PreencheEspacos(3) + "Limite de Indiferença" + PreencheEspacos(1) + "Limite de preferência" + PreencheEspacos(1) + "Função\r\n\n";
            for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
            {
                string strLimiteIndiferenca = dataGridView1.Rows[i].Cells[4].Value.ToString();
                string strLimitePreferencia = dataGridView1.Rows[i].Cells[3].Value.ToString();

                if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Verdadeiro Critério")
                {
                    strLimiteIndiferenca = "----------";
                    strLimitePreferencia = "----------";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Quase Critério")
                {
                    //strLimiteIndiferenca = "";
                    strLimitePreferencia = "----------";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Pseudocritério com preferência linear")
                {
                    strLimiteIndiferenca = "----------";
                    //strLimitePreferencia = "----------";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Critério de Nível")
                {
                    //strLimiteIndiferenca = "";
                    //strLimitePreferencia = "";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Critério com preferência linear")
                {
                    //strLimiteIndiferenca = "";
                    //strLimitePreferencia = "";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Critério Gaussiano")
                {
                    strLimiteIndiferenca = variancias[i].ToString();
                    strLimitePreferencia = variancias[i].ToString();
                }

                outText += strVariaveisSelecionadas[i] + PreencheEspacos(max - strVariaveisSelecionadas[i].Length) + dblPesos[i].ToString() + PreencheEspacos(9 - dblPesos[i].ToString().Length) + dataGridView1.Rows[i].Cells[5].Value.ToString() + PreencheEspacos(1) + strLimiteIndiferenca + PreencheEspacos(22 - strLimiteIndiferenca.Length) + strLimitePreferencia + PreencheEspacos(22 - strLimitePreferencia.Length) + dataGridView1.Rows[i].Cells[2].Value.ToString() + "\r\n\n";
            }

            outText += "\n============================================================================================================================\n\n";
            outText += "Variável Identificadora" + PreencheEspacos(max - 23) + "\t" + strNomeDoMetodo + "\r\n\n";
            for (int i = 0; i < dblIndice.Length; i++)
            {
                outText += strVariavelID[i] + PreencheEspacos(max - strVariavelID[i].Length) + "\t" + dblIndice[i].ToString() + "\r\n\n";
            }
            return (outText);
        }

        public double[] IndicePromethee4(ref ProgressBar progressBar1, ref DataTable m_dt_tabela_dados, DataGridView dataGridView1, string[] strVariaveisSelecionadas, out double[] variancias)
        {
            //Passo 1: Cria a matriz de decisão
            string[,] matDecisao = new string[strVariaveisSelecionadas.Length, 6];
            for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
            {
                try
                {
                    matDecisao[i, 0] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    matDecisao[i, 1] = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    matDecisao[i, 2] = Convert.ToString(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    matDecisao[i, 3] = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    matDecisao[i, 4] = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    matDecisao[i, 5] = dataGridView1.Rows[i].Cells[5].Value.ToString();
                }
                catch
                {
                    matDecisao[i, 0] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    matDecisao[i, 1] = Convert.ToString(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    matDecisao[i, 2] = Convert.ToString(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    matDecisao[i, 3] = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    matDecisao[i, 4] = Convert.ToString(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    matDecisao[i, 0] = "Verdadeiro Critério";
                    matDecisao[i, 1] = "Maximizar";
                }
            }

            //Passo 2: Cria a matriz de dados 
            double[,] mDados = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, strVariaveisSelecionadas);

            //Passo 3: Calcula o índice 
            double[] dblVariancias = new double[strVariaveisSelecionadas.Length];
            double[] indResultado = Promethee_IV(mDados, matDecisao, ref progressBar1, out dblVariancias);
            variancias = dblVariancias;

            return (indResultado);
        }

        public string imprimirPromethee4(DataGridView dataGridView1, DataTable m_dt_tabela_dados, double[] dblIndice, string[] strVariavelID, string strNomeDoMetodo, string strVariavelIdentificadora, string[] strVariaveisSelecionadas, double[] dblPesos, double[] dblVariancias)
        {
            string outText = "\n============================================================================================================================\n\n";

            outText += "Cálculo de índices de apoio a decisão com multicritérios \n";
            outText += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            outText += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";
            outText += "Método: " + strNomeDoMetodo + "\n";
            outText += "Variável identificadora: " + strVariavelIdentificadora + "\n";
            outText += "Número de variáveis: " + m_clt.Double2Texto(strVariaveisSelecionadas.Length, 0) + "\n";

            outText += "\n============================================================================================================================\n\n";

            int max1 = maximoLetras(strVariaveisSelecionadas);
            int max2 = maximoLetras(strVariavelID);
            int max = Math.Max(max1, max2);
            max = Math.Max(max, 23);

            //COLOCAR AQUI A DIRECAO, OS LIMITES DE PREFRENCIA...
            outText += "Variáveis" + PreencheEspacos(max - 9) + "Peso" + PreencheEspacos(5) + "Direção" + PreencheEspacos(3) + "Limite de Indiferença" + PreencheEspacos(1) + "Limite de preferência" + PreencheEspacos(1) + "Função\r\n\n";
            for (int i = 0; i < strVariaveisSelecionadas.Length; i++)
            {
                string strLimiteIndiferenca = dataGridView1.Rows[i].Cells[4].Value.ToString();
                string strLimitePreferencia = dataGridView1.Rows[i].Cells[3].Value.ToString();

                if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Verdadeiro Critério")
                {
                    strLimiteIndiferenca = "----------";
                    strLimitePreferencia = "----------";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Quase Critério")
                {
                    //strLimiteIndiferenca = "";
                    strLimitePreferencia = "----------";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Pseudocritério com preferência linear")
                {
                    strLimiteIndiferenca = "----------";
                    //strLimitePreferencia = "----------";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Critério de Nível")
                {
                    //strLimiteIndiferenca = "";
                    //strLimitePreferencia = "";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Critério com preferência linear")
                {
                    //strLimiteIndiferenca = "";
                    //strLimitePreferencia = "";
                }
                else if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Critério Gaussiano")
                {
                    strLimiteIndiferenca = dblVariancias[i].ToString();
                    strLimitePreferencia = dblVariancias[i].ToString();
                }

                outText += strVariaveisSelecionadas[i] + PreencheEspacos(max - strVariaveisSelecionadas[i].Length) + dblPesos[i].ToString() + PreencheEspacos(9 - dblPesos[i].ToString().Length) + dataGridView1.Rows[i].Cells[5].Value.ToString() + PreencheEspacos(1) + strLimiteIndiferenca + PreencheEspacos(22 - strLimiteIndiferenca.Length) + strLimitePreferencia + PreencheEspacos(22 - strLimitePreferencia.Length) + dataGridView1.Rows[i].Cells[2].Value.ToString() + "\r\n\n";
            }

            outText += "\n============================================================================================================================\n\n";
            outText += "Variável Identificadora" + PreencheEspacos(max - 23) + "\t" + strNomeDoMetodo + "\r\n\n";
            for (int i = 0; i < dblIndice.Length; i++)
            {
                outText += strVariavelID[i] + PreencheEspacos(max - strVariavelID[i].Length) + "\t" + dblIndice[i].ToString() + "\r\n\n";
            }
            return (outText);
        }

        #endregion
        
        #region Ferramentas
        
        public double Minimo(double[] data)
        {
            int error = 0;
            double minimo = double.MaxValue;

            double[] sorted = new double[data.Length];
            data.CopyTo(sorted, 0);
            Array.Sort(sorted);

            for (int i = 0; i < sorted.Length; i++)
            {
                if (double.IsNaN(sorted[i]) == false)
                {
                    minimo = sorted[i];
                    break;
                }
                else
                {
                    error++;
                }
            }
            
            return minimo;
        }

        public double Maximo(double[] data)
        {
            int error = 0;
            double maximo = double.MinValue;

            double[] sorted = new double[data.Length];
            data.CopyTo(sorted, 0);
            Array.Sort(sorted);
            for (int i = sorted.Length - 1; i > -1; i--)
            {
                if (double.IsNaN(sorted[i]) == false)
                {
                    maximo = sorted[i];
                    break;
                }
                else
                {
                    error++;
                }
            }
            return maximo;
        }
        
        public double Median(double[] Vetor)
        {
            double[] sorted = new double[Vetor.Length];
            Vetor.CopyTo(sorted, 0);
            Array.Sort(sorted);

            if (sorted.Length % 2 == 0)
            {
                return ((sorted[(Vetor.Length / 2) - 1] + sorted[Vetor.Length / 2]) / 2);
            }
            else
            {
                return (sorted[(Vetor.Length - 1) / 2]);
            }
        }

        public double LowerQuartile(DataTable dt, string Coluna)
        {
            int erros = 0;
            int index = dt.Columns.IndexOf(Coluna);
            for (int linha = 0; linha < dt.Rows.Count; linha++)
            {
                if (dt.Rows[linha][index] == DBNull.Value)
                {
                    erros++;
                }
            }

            double[] meuArray2 = new double[(dt.Rows.Count - erros) * (dt.Rows.Count - 1 - erros)];
            int Posicao = 0;
            for (int linha = 0; linha < dt.Rows.Count; linha++)
            {
                for (int linha2 = 0; linha2 < dt.Rows.Count; linha2++)
                {
                    try
                    {
                        if (linha == linha2)
                        {
                        }
                        else
                        {
                            meuArray2[Posicao] = Math.Abs(Convert.ToDouble(dt.Rows[linha][index]) - Convert.ToDouble(dt.Rows[linha2][index]));
                            Posicao++;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            Array.Sort(meuArray2);

            if (meuArray2.Length % 2 == 0)
            {
                double[] lowerHalf = new double[meuArray2.Length / 2];
                Array.Copy(meuArray2, lowerHalf, meuArray2.Length / 2);

                // Find the median of the lower half.
                return Median(lowerHalf);
            }
            else
            {
                double[] lowerHalf = new double[(meuArray2.Length - 1) / 2];
                Array.Copy(meuArray2, 0, lowerHalf, 0, (meuArray2.Length - 1) / 2);

                // Find the median of the lower half.
                return Median(lowerHalf);
            }
        }

        public double UpperQuartile(DataTable dt, string Coluna)
        {
            int erros = 0;
            int index = dt.Columns.IndexOf(Coluna);
            for (int linha = 0; linha < dt.Rows.Count; linha++)
            {
                if (dt.Rows[linha][index] == DBNull.Value)
                {
                    erros++;
                }
            }

            double[] meuArray2 = new double[(dt.Rows.Count - erros) * (dt.Rows.Count - 1 - erros)];
            int Posicao = 0;
            for (int linha = 0; linha < dt.Rows.Count; linha++)
            {
                for (int linha2 = 0; linha2 < dt.Rows.Count; linha2++)
                {
                    try
                    {
                        if (linha == linha2)
                        {
                        }
                        else
                        {
                            meuArray2[Posicao] = Math.Abs(Convert.ToDouble(dt.Rows[linha][index]) - Convert.ToDouble(dt.Rows[linha2][index]));
                            Posicao++;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            double[] sorted = new double[meuArray2.Length];
            meuArray2.CopyTo(sorted, 0);
            Array.Sort(sorted);

            if (sorted.Length % 2 == 0)
            {
                double[] upperHalf = new double[meuArray2.Length / 2];
                Array.Copy(meuArray2, (meuArray2.Length) / 2, upperHalf, 0, meuArray2.Length / 2);

                // Find the median of the upper half.
                return Median(upperHalf);
            }
            else
            {
                double[] upperHalf = new double[(meuArray2.Length - 1) / 2];
                Array.Copy(meuArray2, (meuArray2.Length + 1) / 2, upperHalf, 0, (meuArray2.Length - 1) / 2);

                // Find the median of the upper half.
                return Median(upperHalf);
            }
        }

        private double[] EncontraVetor(double[,] Matriz_de_Entrada, int Coluna)
        {
            double[] Resultado = new double[Matriz_de_Entrada.GetLength(0)];

            for (int i = 0; i < Matriz_de_Entrada.GetLength(0); i++)
            {
                Resultado[i] = Matriz_de_Entrada[i, Coluna];
            }
            return Resultado;
        }

        #endregion

        #region Funções de preferência relativa

        private double Verdadeiro_Criterio(double diferencaAbsoluta)
        {
            if (diferencaAbsoluta == double.NaN)
            {
                return double.NaN;
            }
            else
            {
                if (diferencaAbsoluta != 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        private double Quase_Criterio(double diferencaAbsoluta, double limite_de_indiferenca_Q)
        {
            if (diferencaAbsoluta == double.NaN)
            {
                return double.NaN;
            }
            else
            {
                if (diferencaAbsoluta > limite_de_indiferenca_Q)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        private double Pseudo_Criterio_Com_Preferencia_Linear(double diferencaAbsoluta, double limite_de_preferencia_P)
        {
            if (diferencaAbsoluta == double.NaN)
            {
                return double.NaN;
            }
            else
            {
                if (diferencaAbsoluta > limite_de_preferencia_P)
                {
                    return 1;
                }
                else
                {
                    return (diferencaAbsoluta / limite_de_preferencia_P);
                }
            }
        }

        private double Criterio_de_Nivel(double diferencaAbsoluta, double limite_de_indiferenca_Q, double limite_de_preferencia_P)
        {
            if (diferencaAbsoluta == double.NaN)
            {
                return double.NaN;
            }
            else
            {
                if (diferencaAbsoluta > limite_de_preferencia_P)
                {
                    return 1;
                }
                else if ((limite_de_indiferenca_Q < diferencaAbsoluta) && (diferencaAbsoluta <= limite_de_preferencia_P))
                {
                    return .5;
                }
                else
                {
                    return 0;
                }
            }
        }

        private double Criterio_com_Preferencia_Linear(double diferencaAbsoluta, double limite_de_indiferenca_Q, double limite_de_preferencia_P)
        {
            if (diferencaAbsoluta == double.NaN)
            {
                return double.NaN;
            }
            else
            {
                if (diferencaAbsoluta > limite_de_preferencia_P)
                {
                    return 1;
                }
                else if ((limite_de_indiferenca_Q < diferencaAbsoluta) && (diferencaAbsoluta <= limite_de_preferencia_P))
                {
                    return ((diferencaAbsoluta - limite_de_indiferenca_Q) / (limite_de_preferencia_P - limite_de_indiferenca_Q));
                }
                else
                {
                    return 0;
                }
            }
        }

        private double Criterio_Gaussiano(double[] Vetor, double diferencaAbsoluta, int iColuna, ref double[] variancia)
        {
            double Var;
            double Resultado;
            estatistica estat = new estatistica();
            estat.avevar(Vetor, (ulong)Vetor.Length);
            Var = estat.Variance;
            variancia[iColuna] = Var;
            if (diferencaAbsoluta == double.NaN)
            {
                return double.NaN;
            }
            else
            {
                Resultado = 1 - (Math.Exp((-(Math.Pow(diferencaAbsoluta, 2)) / 2 * Var)));

                return Resultado;
            }
        }

        #endregion

        #region Indices

        public double[] Promethee_I(double[,] matriz_de_entrada, string[,] Matriz_De_Decisao, ref ProgressBar pBar, out double[] dblVariancias)
        {
            int N = matriz_de_entrada.GetLength(0);
            int Coluna = matriz_de_entrada.GetLength(1);
            double[] Resultado = new double[N];

            double[,] Matriz_De_Diferencas = new double[N * (N - 1), Coluna];
            pBar.Minimum=0;
            pBar.Maximum=Coluna*N*N;
            pBar.Value=0;
            dblVariancias = new double[Coluna];
            int[,] Pares_Ordenados = new int[N * (N - 1), 2];
            for (int coluna = 0; coluna < (Coluna); coluna++)
            {
                int Posicao = 0;
                for (int linha1 = 0; linha1 < N; linha1++)
                {
                    for (int linha2 = 0; linha2 < N; linha2++)
                    {
                        if (linha1 == linha2)
                        {
                        }
                        else
                        {
                            if (Matriz_De_Decisao[coluna, 4] == "Verdadeiro Critério")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Verdadeiro_Criterio(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Verdadeiro_Criterio(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }

                            else if (Matriz_De_Decisao[coluna, 4] == "Quase Critério")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Quase_Criterio(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Quase_Criterio(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                            else if (Matriz_De_Decisao[coluna, 4] == "Pseudocritério com preferência Linear")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Pseudo_Criterio_Com_Preferencia_Linear(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Pseudo_Criterio_Com_Preferencia_Linear(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                            else if (Matriz_De_Decisao[coluna, 4] == "Critério de Nível")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_de_Nivel(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_de_Nivel(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                            else if (Matriz_De_Decisao[coluna, 4] == "Critério com preferência linear")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_com_Preferencia_Linear(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_com_Preferencia_Linear(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                            else if (Matriz_De_Decisao[coluna, 4] == "Critério Gaussiano")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_Gaussiano(EncontraVetor(matriz_de_entrada, coluna), (Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna])),coluna,ref dblVariancias);
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_Gaussiano(EncontraVetor(matriz_de_entrada, coluna), (Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna])), coluna, ref dblVariancias);
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                        }
                        pBar.Increment(1);
                        Application.DoEvents();
                    }
                }
            }
            
            double SomaPeso = 0;
            int Linhas_Matriz_de_Decisao = Matriz_De_Decisao.GetLength(0);
            for (int i = 0; i < Linhas_Matriz_de_Decisao; i++)
            {
                try
                {
                    SomaPeso += Convert.ToDouble(Matriz_De_Decisao[i, 1]);
                }
                catch
                {

                }
            }

            double[,] Fluxo_de_superacao = new double[N, N];
            int Coluna_Mat_Diferencas = Matriz_De_Diferencas.GetLength(1);
            for (int linha = 0; linha < N * (N - 1); linha++)
            {
                for (int coluna = 0; coluna < (Coluna_Mat_Diferencas); coluna++)
                {
                    if (Pares_Ordenados[linha, 0] == Pares_Ordenados[linha, 1])
                    {
                        Fluxo_de_superacao[Pares_Ordenados[linha, 0], Pares_Ordenados[linha, 1]] = 0;
                    }
                    else
                    {
                        Fluxo_de_superacao[Pares_Ordenados[linha, 0], Pares_Ordenados[linha, 1]] += (Convert.ToDouble(Matriz_De_Decisao[coluna, 1]) * Matriz_De_Diferencas[linha, coluna]);
                    }
                }
                
                //Guarda os pesos.

                Fluxo_de_superacao[Pares_Ordenados[linha, 0], Pares_Ordenados[linha, 1]] /= SomaPeso;
            }

            double[,] Avaliacao_Phi = new double[N, 2];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                    }
                    else
                    {
                        if (double.IsNaN(Fluxo_de_superacao[i, j]) == true && double.IsNaN(Fluxo_de_superacao[i, j]) == true)
                        {
                        }
                        else if (double.IsNaN(Fluxo_de_superacao[i, j]) == true && double.IsNaN(Fluxo_de_superacao[i, j]) == false)
                        {
                            Avaliacao_Phi[i, 1] += Fluxo_de_superacao[j, i];
                        }
                        else if (double.IsNaN(Fluxo_de_superacao[i, j]) == false && double.IsNaN(Fluxo_de_superacao[i, j]) == true)
                        {
                            Avaliacao_Phi[i, 0] += Fluxo_de_superacao[i, j];
                        }
                        else
                        {
                            Avaliacao_Phi[i, 0] += Fluxo_de_superacao[i, j];
                            Avaliacao_Phi[i, 1] += Fluxo_de_superacao[j, i];
                        }
                    }
                }
            }

            for (int linha1 = 0; linha1 < Avaliacao_Phi.GetLength(0); linha1++)
            {
                for (int linha2 = 0; linha2 < Avaliacao_Phi.GetLength(0); linha2++)
                {
                    if (linha1 == linha2)
                    {
                    }
                    else
                    {
                        if (((Avaliacao_Phi[linha1, 0] >= Avaliacao_Phi[linha2, 0]) && (Avaliacao_Phi[linha1, 1] <= Avaliacao_Phi[linha2, 1])))
                        {
                            Resultado[linha1] += 1;
                        }
                    }
                }
            }

            for (int i = 0; i < N; i++)
            {
                Resultado[i] /= (N - 1);
            }

            //Obter a Pré-Ordem!!!!

            pBar.Value = 0;
            pBar.Refresh();
            pBar.Update();
            Application.DoEvents();
            return Resultado;
        }

        public double[] Promethee_II(double[,] matriz_de_entrada, string[,] Matriz_De_Decisao, ref ProgressBar pBar, out double[] dblVariancias)
        {
            int N = matriz_de_entrada.GetLength(0);
            int Coluna = matriz_de_entrada.GetLength(1);
            double[] Resultado = new double[N];

            double[,] Matriz_De_Diferencas = new double[N * (N - 1), Coluna];
            pBar.Minimum = 0;
            pBar.Maximum = Coluna * N * N;
            pBar.Value = 0;
            dblVariancias = new double[Coluna];
            int[,] Pares_Ordenados = new int[N * (N - 1), 2];
            for (int coluna = 0; coluna < (Coluna); coluna++)
            {
                int Posicao = 0;
                for (int linha1 = 0; linha1 < N; linha1++)
                {
                    for (int linha2 = 0; linha2 < N; linha2++)
                    {
                        if (linha1 == linha2)
                        {
                        }
                        else
                        {
                            if (Matriz_De_Decisao[coluna, 4] == "Verdadeiro Critério")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Verdadeiro_Criterio(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Verdadeiro_Criterio(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }

                            else if (Matriz_De_Decisao[coluna, 4] == "Quase Critério")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Quase_Criterio(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Quase_Criterio(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                            else if (Matriz_De_Decisao[coluna, 4] == "Pseudocritério com preferência Linear")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Pseudo_Criterio_Com_Preferencia_Linear(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Pseudo_Criterio_Com_Preferencia_Linear(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                            else if (Matriz_De_Decisao[coluna, 4] == "Critério de Nível")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_de_Nivel(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_de_Nivel(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                            else if (Matriz_De_Decisao[coluna, 4] == "Critério com preferência linear")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_com_Preferencia_Linear(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_com_Preferencia_Linear(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }

                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                            else if (Matriz_De_Decisao[coluna, 4] == "Critério Gaussiano")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_Gaussiano(EncontraVetor(matriz_de_entrada, coluna), (Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna])), coluna, ref dblVariancias);
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_Gaussiano(EncontraVetor(matriz_de_entrada, coluna), (Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna])), coluna, ref dblVariancias);
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                        }
                        pBar.Increment(1);
                        Application.DoEvents();
                    }
                }
            }
            
            double SomaPeso = 0;
            int Linhas_Matriz_de_Decisao = Matriz_De_Decisao.GetLength(0);
            for (int i = 0; i < Linhas_Matriz_de_Decisao; i++)
            {
                try
                {
                    SomaPeso += Convert.ToDouble(Matriz_De_Decisao[i, 1]);
                }
                catch
                {

                }
            }

            double[,] Fluxo_de_superacao = new double[N, N];
            int Coluna_Mat_Diferencas = Matriz_De_Diferencas.GetLength(1);
            for (int linha = 0; linha < N * (N - 1); linha++)
            {
                for (int coluna = 0; coluna < (Coluna_Mat_Diferencas); coluna++)
                {
                    if (Pares_Ordenados[linha, 0] == Pares_Ordenados[linha, 1])
                    {
                        Fluxo_de_superacao[Pares_Ordenados[linha, 0], Pares_Ordenados[linha, 1]] = 0;
                    }
                    else
                    {
                        Fluxo_de_superacao[Pares_Ordenados[linha, 0], Pares_Ordenados[linha, 1]] += (Convert.ToDouble(Matriz_De_Decisao[coluna, 1]) * Matriz_De_Diferencas[linha, coluna]);
                    }
                }

                Fluxo_de_superacao[Pares_Ordenados[linha, 0], Pares_Ordenados[linha, 1]] /= SomaPeso;
            }

            double[,] Avaliacao_Phi = new double[N, 2];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                    }
                    else
                    {
                        if (double.IsNaN(Fluxo_de_superacao[i, j]) == true && double.IsNaN(Fluxo_de_superacao[i, j]) == true)
                        {

                        }
                        else if (double.IsNaN(Fluxo_de_superacao[i, j]) == true && double.IsNaN(Fluxo_de_superacao[i, j]) == false)
                        {

                            Avaliacao_Phi[i, 1] += Fluxo_de_superacao[j, i];
                        }
                        else if (double.IsNaN(Fluxo_de_superacao[i, j]) == false && double.IsNaN(Fluxo_de_superacao[i, j]) == true)
                        {
                            Avaliacao_Phi[i, 0] += Fluxo_de_superacao[i, j];

                        }
                        else
                        {
                            Avaliacao_Phi[i, 0] += Fluxo_de_superacao[i, j];
                            Avaliacao_Phi[i, 1] += Fluxo_de_superacao[j, i];
                        }
                    }
                }
            }
            for (int i = 0; i < N; i++)
            {
                Resultado[i] = (Avaliacao_Phi[i, 0] - Avaliacao_Phi[i, 1]) / (N - 1);
            }

            double Max = Maximo(Resultado);
            double Min = Minimo(Resultado);
            for (int i = 0; i < N; i++)
            {
                Resultado[i] = 1 - ((Max - Resultado[i]) / (Max - Min));
            }

            pBar.Value = 0;
            pBar.Refresh();
            pBar.Update();
            Application.DoEvents();
            return Resultado;
        }

        public double[] Promethee_III(double[,] matriz_de_entrada, string[,] Matriz_De_Decisao, ref ProgressBar pBar, double alfa, out double[] dblVariancias)
        {
            int N = matriz_de_entrada.GetLength(0);
            int Coluna = matriz_de_entrada.GetLength(1);
            double[] Resultado = new double[N];

            double[,] Matriz_De_Diferencas = new double[N * (N - 1), Coluna];
            pBar.Minimum = 0;
            pBar.Maximum = Coluna * N * N;
            pBar.Value = 0;
            dblVariancias = new double[Coluna];
            int[,] Pares_Ordenados = new int[N * (N - 1), 2];
            for (int coluna = 0; coluna < (Coluna); coluna++)
            {
                int Posicao = 0;
                for (int linha1 = 0; linha1 < N; linha1++)
                {
                    for (int linha2 = 0; linha2 < N; linha2++)
                    {
                        if (linha1 == linha2)
                        {
                        }
                        else
                        {

                            if (Matriz_De_Decisao[coluna, 4] == "Verdadeiro Critério")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Verdadeiro_Criterio(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Verdadeiro_Criterio(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }

                            else if (Matriz_De_Decisao[coluna, 4] == "Quase Critério")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Quase_Criterio(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Quase_Criterio(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                            else if (Matriz_De_Decisao[coluna, 4] == "Pseudocritério com preferência Linear")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Pseudo_Criterio_Com_Preferencia_Linear(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Pseudo_Criterio_Com_Preferencia_Linear(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                            else if (Matriz_De_Decisao[coluna, 4] == "Critério de Nível")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_de_Nivel(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_de_Nivel(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                            else if (Matriz_De_Decisao[coluna, 4] == "Critério com preferência linear")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_com_Preferencia_Linear(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_com_Preferencia_Linear(Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna]), Convert.ToDouble(Matriz_De_Decisao[coluna, 2]), Convert.ToDouble(Matriz_De_Decisao[coluna, 3]));
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }

                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                            else if (Matriz_De_Decisao[coluna, 4] == "Critério Gaussiano")
                            {
                                if (Matriz_De_Decisao[coluna, 5] == "Maximizar" && ((matriz_de_entrada[linha1, coluna] > (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_Gaussiano(EncontraVetor(matriz_de_entrada, coluna), (Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna])), coluna, ref dblVariancias);
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if (Matriz_De_Decisao[coluna, 5] == "Minimizar" && ((matriz_de_entrada[linha1, coluna] < (matriz_de_entrada[linha2, coluna]))))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = Criterio_Gaussiano(EncontraVetor(matriz_de_entrada, coluna), (Math.Abs(matriz_de_entrada[linha1, coluna] - matriz_de_entrada[linha2, coluna])), coluna, ref dblVariancias);
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else if ((double.IsNaN(matriz_de_entrada[linha1, coluna]) == true) || (double.IsNaN(matriz_de_entrada[linha2, coluna]) == true))
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = double.NaN;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                else
                                {
                                    Matriz_De_Diferencas[Posicao, coluna] = 0;
                                    Pares_Ordenados[Posicao, 0] = linha1;
                                    Pares_Ordenados[Posicao, 1] = linha2;
                                }
                                Posicao++;
                            }
                        }
                        pBar.Increment(1);
                        Application.DoEvents();
                    }
                }
            }
            double SomaPeso = 0;
            int Linhas_Matriz_de_Decisao = Matriz_De_Decisao.GetLength(0);
            for (int i = 0; i < Linhas_Matriz_de_Decisao; i++)
            {
                try
                {
                    SomaPeso += Convert.ToDouble(Matriz_De_Decisao[i, 1]);
                }
                catch
                {
                }
            }

            double[,] Fluxo_de_superacao = new double[N, N];
            int Coluna_Mat_Diferencas = Matriz_De_Diferencas.GetLength(1);
            for (int linha = 0; linha < N * (N - 1); linha++)
            {
                for (int coluna = 0; coluna < (Coluna_Mat_Diferencas); coluna++)
                {
                    if (Pares_Ordenados[linha, 0] == Pares_Ordenados[linha, 1])
                    {
                        Fluxo_de_superacao[Pares_Ordenados[linha, 0], Pares_Ordenados[linha, 1]] = 0;
                    }
                    else
                    {
                        Fluxo_de_superacao[Pares_Ordenados[linha, 0], Pares_Ordenados[linha, 1]] += (Convert.ToDouble(Matriz_De_Decisao[coluna, 1]) * Matriz_De_Diferencas[linha, coluna]);
                    }
                }

                Fluxo_de_superacao[Pares_Ordenados[linha, 0], Pares_Ordenados[linha, 1]] /= SomaPeso;
            }

            double[,] Avaliacao_Phi = new double[N, 2];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (i == j)
                    {
                    }
                    else
                    {
                        if (double.IsNaN(Fluxo_de_superacao[i, j]) == true && double.IsNaN(Fluxo_de_superacao[i, j]) == true)
                        {
                        }
                        else if (double.IsNaN(Fluxo_de_superacao[i, j]) == true && double.IsNaN(Fluxo_de_superacao[i, j]) == false)
                        {
                            Avaliacao_Phi[i, 1] += Fluxo_de_superacao[j, i];
                        }
                        else if (double.IsNaN(Fluxo_de_superacao[i, j]) == false && double.IsNaN(Fluxo_de_superacao[i, j]) == true)
                        {
                            Avaliacao_Phi[i, 0] += Fluxo_de_superacao[i, j];
                        }
                        else
                        {
                            Avaliacao_Phi[i, 0] += Fluxo_de_superacao[i, j];
                            Avaliacao_Phi[i, 1] += Fluxo_de_superacao[j, i];
                        }
                    }
                }
            }

            for (int i = 0; i < N; i++)
            {
                Resultado[i] = (Avaliacao_Phi[i, 0] - Avaliacao_Phi[i, 1]) / (N - 1);
            }

            double sigma2 = Variancia(Resultado);
            double[] xa = new double[Resultado.Length];
            double[] ya = new double[Resultado.Length];
            for (int i = 0; i < Resultado.Length; i++)
            {
                xa[i] = Resultado[i] - alfa * Math.Sqrt(sigma2);
                ya[i] = Resultado[i] + alfa * Math.Sqrt(sigma2);
            }

            //Faz as comparações
            //aPb (a é preferível a b) se e somente se xa>yb,
            //aIb (a é indiferente a b) se e somente se xa≤yb e xb≤ya
            for (int i = 0; i < Resultado.Length; i++)
            {
                for (int j = 0; j < Resultado.Length; j++)
                {
                    if (xa[i] > ya[j]) Resultado[i]++;
                }
            }
            double NN=Convert.ToDouble(Resultado.Length);
            for (int i = 0; i < Resultado.Length; i++)
            {
                Resultado[i] = Resultado[i] / (NN * NN);
            }

            double Max = Maximo(Resultado);
            double Min = Minimo(Resultado);
            for (int i = 0; i < N; i++)
            {
                Resultado[i] = 1 - ((Max - Resultado[i]) / (Max - Min));
            }

            pBar.Value = 0;
            pBar.Refresh();
            pBar.Update();
            Application.DoEvents();
            return Resultado;
        }

        public delegate double Function(double x, double a); 

        public double[] Promethee_IV(double[,] matriz_de_entrada, string[,] Matriz_De_Decisao, ref ProgressBar pBar, out double[] dblVariancias)
        {
            int N = matriz_de_entrada.GetLength(0);
            int Coluna = matriz_de_entrada.GetLength(1);
            double[] Resultado = new double[N];
            double[,] Matriz_De_Diferencas = new double[N * (N - 1), Coluna];
            pBar.Minimum = 0;
            pBar.Maximum = Coluna * N;
            pBar.Value = 0;
            dblVariancias = new double[Coluna];

            //Calcula a soma dos pesos
            double SomaPeso = 0;
            for (int i = 0; i < Matriz_De_Decisao.GetLength(0); i++)
            {
                    SomaPeso += Convert.ToDouble(Matriz_De_Decisao[i, 1]);
            }
            
            //Encontra os limites de integração:
            double[,] a = m_clt.Minc(matriz_de_entrada);
            double[,] b = m_clt.Maxc(matriz_de_entrada);
            
            for (int linha1 = 0; linha1 < N; linha1++)
            {
                double blMinimiza = 1.0;
                for (int coluna = 0; coluna < (Coluna); coluna++)
                {
                    if (Matriz_De_Decisao[coluna, 4] == "Verdadeiro Critério")
                    {
                        if (Matriz_De_Decisao[coluna, 5] == "Minimizar") blMinimiza = -1.0;
                        double limite = (Math.Max(Math.Abs(a[0, coluna]), Math.Abs(b[0, coluna]))) * 2;

                        //Programa a função usando Lambda expressions 
                        //Func<double, double> F;
                        //F = (x) =>  x + 2;
                        //http://www.elguille.info/NET/futuro/firmas_octavio_ExpresionesLambda_EN.htm
                        //http://dotnet.learningtree.com/2011/06/17/lambda-expressions-in-c/

                        //Func<double, double> F;
                        //F = (x) =>
                        //    {
                        //        if (x <= 0)
                        //        {
                        //            return (0);
                        //        }
                        //        else
                        //        {
                        //            return (1);
                        //        }
                        //    };
                        //DefiniteIntegral integralTeste = new DefiniteIntegral(F, new Integral(-2, 5+2));
                        //double phiTeste = integralTeste.Approximate(DefiniteIntegral.ApproximationMethod.Simpson, 1000);

                        //Define o integrando
                        double constante=matriz_de_entrada[linha1, coluna];
                        Func<double, double> Verdadeiro_Criterio;
                        Verdadeiro_Criterio= (diferenca) =>
                        {
                            double phiMais = (constante - diferenca) * blMinimiza;
                            double Pab = 0;
                            if (phiMais > 0) Pab = 1;
                            double phiMenos = (diferenca-constante) * blMinimiza;
                            double Pba = 0;
                            if (phiMenos > 0) Pba = 1;
                            diferenca = Pab-Pba;
                            return (diferenca);
                        };
                        DefiniteIntegral integral = new DefiniteIntegral(Verdadeiro_Criterio, new Integral(-limite, 2*Math.Abs(limite))); 
                        double phi=integral.Approximate(ApproximationMethod.Simpson,1000);
                        Resultado[linha1] += phi * Convert.ToDouble(Matriz_De_Decisao[coluna, 1]);
                    }
                    else if (Matriz_De_Decisao[coluna, 4] == "Quase Critério")
                    {
                        if (Matriz_De_Decisao[coluna, 5] == "Minimizar") blMinimiza = -1.0;
                        double limite = (Math.Max(Math.Abs(a[0, coluna]), Math.Abs(b[0, coluna]))) * 2;
                        //Define o integrando
                        double constante = matriz_de_entrada[linha1, coluna];
                        Func<double, double> Quase_Criterio;
                        Quase_Criterio = (diferenca) =>
                        {
                            double phiMais = (constante - diferenca) * blMinimiza;
                            double Pab = 0;
                            if (phiMais > Convert.ToDouble(Matriz_De_Decisao[coluna, 2])) Pab = 1;
                            double phiMenos = (diferenca - constante) * blMinimiza;
                            double Pba = 0;
                            if (phiMenos > Convert.ToDouble(Matriz_De_Decisao[coluna, 2])) Pba = 1;
                            diferenca = Pab - Pba;
                            return (diferenca);
                        };
                        DefiniteIntegral integral = new DefiniteIntegral(Quase_Criterio, new Integral(-limite, 2 * Math.Abs(limite)));
                        double phi = integral.Approximate(ApproximationMethod.Simpson, 1000);
                        Resultado[linha1] += phi * Convert.ToDouble(Matriz_De_Decisao[coluna, 1]);

                    }
                    else if (Matriz_De_Decisao[coluna, 4] == "Pseudocritério com preferência linear")
                    {
                        if (Matriz_De_Decisao[coluna, 5] == "Minimizar") blMinimiza = -1.0;
                        double limite = (Math.Max(Math.Abs(a[0, coluna]), Math.Abs(b[0, coluna]))) * 2;
                        //Define o integrando
                        double constante = matriz_de_entrada[linha1, coluna];
                        Func<double, double> Pseudocriterio_com_preferencia_Linear;
                        Pseudocriterio_com_preferencia_Linear = (diferenca) =>
                        {
                            double phiMais = (constante - diferenca) * blMinimiza;
                            double Pab = 0;
                            if (phiMais > Convert.ToDouble(Matriz_De_Decisao[coluna, 3]))
                            {
                                Pab = 1;
                            }
                            else if (phiMais <= Convert.ToDouble(Matriz_De_Decisao[coluna, 3]) && phiMais>0)
                            {
                                Pab = phiMais / Convert.ToDouble(Matriz_De_Decisao[coluna, 3]);
                            }
                            else
                            {
                                Pab = 0;
                            }
                            double phiMenos = (diferenca - constante) * blMinimiza;
                            double Pba = 0;
                            if (phiMenos > Convert.ToDouble(Matriz_De_Decisao[coluna, 3]))
                            {
                                Pba = 1;
                            }
                            else if (phiMenos <= Convert.ToDouble(Matriz_De_Decisao[coluna, 3]) && phiMenos > 0)
                            {
                                Pba = phiMenos / Convert.ToDouble(Matriz_De_Decisao[coluna, 3]);
                            }
                            else
                            {
                                Pba = 0;
                            }
                            diferenca = Pab - Pba;
                            return (diferenca);
                        };
                        DefiniteIntegral integral = new DefiniteIntegral(Pseudocriterio_com_preferencia_Linear, new Integral(-limite, 2 * Math.Abs(limite)));
                        double phi = integral.Approximate(ApproximationMethod.Simpson, 1000);
                        Resultado[linha1] += phi * Convert.ToDouble(Matriz_De_Decisao[coluna, 1]);

                    }
                    else if (Matriz_De_Decisao[coluna, 4] == "Critério de Nível")
                    {
                        if (Matriz_De_Decisao[coluna, 5] == "Minimizar") blMinimiza = -1.0;
                        double limite = (Math.Max(Math.Abs(a[0, coluna]), Math.Abs(b[0, coluna]))) * 2;
                        //Define o integrando
                        double constante = matriz_de_entrada[linha1, coluna];
                        Func<double, double> Criterio_de_Nivel;
                        Criterio_de_Nivel = (diferenca) =>
                        {
                            double phiMais = (constante - diferenca) * blMinimiza;
                            double Pab = 0;
                            if (phiMais > Convert.ToDouble(Matriz_De_Decisao[coluna, 3]))
                            {
                                Pab = 1;
                            }
                            else if (phiMais <= Convert.ToDouble(Matriz_De_Decisao[coluna, 3]) && phiMais > Convert.ToDouble(Matriz_De_Decisao[coluna, 2]))
                            {
                                Pab = 0.5;
                            }
                            else
                            {
                                Pab = 0;
                            }
                            double phiMenos = (diferenca - constante) * blMinimiza;
                            double Pba = 0;
                            if (phiMenos > Convert.ToDouble(Matriz_De_Decisao[coluna, 3]))
                            {
                                Pba = 1;
                            }
                            else if (phiMenos <= Convert.ToDouble(Matriz_De_Decisao[coluna, 3]) && phiMenos > Convert.ToDouble(Matriz_De_Decisao[coluna, 2]))
                            {
                                Pba = 0.5;
                            }
                            else
                            {
                                Pba = 0;
                            }
                            diferenca = Pab - Pba;
                            return (diferenca);
                        };
                        DefiniteIntegral integral = new DefiniteIntegral(Criterio_de_Nivel, new Integral(-limite, 2 * Math.Abs(limite)));
                        double phi = integral.Approximate(ApproximationMethod.Simpson, 1000);
                        Resultado[linha1] += phi * Convert.ToDouble(Matriz_De_Decisao[coluna, 1]);
                    }
                    else if (Matriz_De_Decisao[coluna, 4] == "Critério com preferência linear")
                    {
                        if (Matriz_De_Decisao[coluna, 5] == "Minimizar") blMinimiza = -1.0;
                        double limite = (Math.Max(Math.Abs(a[0, coluna]), Math.Abs(b[0, coluna]))) * 2;
                        
                        //Define o integrando
                        double constante = matriz_de_entrada[linha1, coluna];
                        Func<double, double> Criterio_com_preferencia_linear;
                        Criterio_com_preferencia_linear = (diferenca) =>
                        {
                            double phiMais = (constante - diferenca) * blMinimiza;
                            double Pab = 0;
                            if (phiMais > Convert.ToDouble(Matriz_De_Decisao[coluna, 3]))
                            {
                                Pab = 1;
                            }
                            else if (phiMais <= Convert.ToDouble(Matriz_De_Decisao[coluna, 3]) && phiMais > Convert.ToDouble(Matriz_De_Decisao[coluna, 2]))
                            {
                                Pab = (phiMais - Convert.ToDouble(Matriz_De_Decisao[coluna, 2])) / (Convert.ToDouble(Matriz_De_Decisao[coluna, 3]) - Convert.ToDouble(Matriz_De_Decisao[coluna, 2]));
                            }
                            else
                            {
                                Pab = 0;
                            }
                            double phiMenos = (diferenca - constante) * blMinimiza;
                            double Pba = 0;
                            if (phiMenos > Convert.ToDouble(Matriz_De_Decisao[coluna, 3]))
                            {
                                Pba = 1;
                            }
                            else if (phiMenos <= Convert.ToDouble(Matriz_De_Decisao[coluna, 3]) && phiMenos > Convert.ToDouble(Matriz_De_Decisao[coluna, 2]))
                            {
                                Pba = (phiMenos - Convert.ToDouble(Matriz_De_Decisao[coluna, 2])) / (Convert.ToDouble(Matriz_De_Decisao[coluna, 3]) - Convert.ToDouble(Matriz_De_Decisao[coluna, 2]));
                            }
                            else
                            {
                                Pba = 0;
                            }
                            diferenca = Pab - Pba;
                            return (diferenca);
                        };
                        DefiniteIntegral integral = new DefiniteIntegral(Criterio_com_preferencia_linear, new Integral(-limite, 2 * Math.Abs(limite)));
                        double phi = integral.Approximate(ApproximationMethod.Simpson, 1000);
                        Resultado[linha1] += phi * Convert.ToDouble(Matriz_De_Decisao[coluna, 1]);
                    }
                    else if (Matriz_De_Decisao[coluna, 4] == "Critério Gaussiano")
                    {
                        if (Matriz_De_Decisao[coluna, 5] == "Minimizar") blMinimiza = -1.0;
                        double limite = (Math.Max(Math.Abs(a[0, coluna]), Math.Abs(b[0, coluna]))) * 2;
                        double sigma2 = Math.Pow(2 * Math.Max(Math.Abs(a[0, coluna]), Math.Abs(b[0, coluna])), 2) / 12.0;
                        dblVariancias[coluna] = sigma2;
                        
                        //Define o integrando
                        double constante = matriz_de_entrada[linha1, coluna];
                        Func<double, double> Criterio_Gaussiano;
                        Criterio_Gaussiano = (diferenca) =>
                        {
                            double phiMais = (constante - diferenca) * blMinimiza;
                            double Pab = 1-Math.Exp(-phiMais*phiMais/(2*sigma2));
                            
                            double phiMenos = (diferenca - constante) * blMinimiza;
                            double Pba = 1 - Math.Exp(-phiMais * phiMenos / (2 * sigma2));
                            diferenca = Pab - Pba;
                            return (diferenca);
                        };
                        DefiniteIntegral integral = new DefiniteIntegral(Criterio_Gaussiano, new Integral(-limite, 2 * Math.Abs(limite)));
                        double phi = integral.Approximate(ApproximationMethod.Simpson, 1000);
                        Resultado[linha1] += phi * Convert.ToDouble(Matriz_De_Decisao[coluna, 1]);
                    }
                    pBar.Increment(1);
                    Application.DoEvents();
                }
                Resultado[linha1] = Resultado[linha1] / SomaPeso;
            }

            double Max = Maximo(Resultado);
            double Min = Minimo(Resultado);
            for (int i = 0; i < N; i++)
            {
                Resultado[i] = 1 - ((Max - Resultado[i]) / (Max - Min));
            }

            pBar.Value = 0;
            pBar.Refresh();
            pBar.Update();
            Application.DoEvents();
            return(Resultado);
        }

        public double Variancia(double[] dados)
        {
            double soma = 0;
            double soma2 = 0;

            for (int i = 0; i < dados.Length; i++)
            {
                soma += dados[i];
                soma2 += dados[i] * dados[i];
            }
            return ((soma2 / Convert.ToDouble(dados.Length)) - Math.Pow((soma / Convert.ToDouble(dados.Length)), 2));
        }
        
        #endregion

        #endregion

        #region enumerações

        public enum Metodo : int
        {
            Albebrico = 1,
            HierarquicoClassico = 2,
            HierarquicoMultiplicativo = 3,
            Geral=4
        };
        
        #endregion

        #region Variaveis e classes internas

        protected static clsUtilTools m_clt = new clsUtilTools();

            #region Classe interna elementoArvore

            [Serializable]
            public class elementoArvore : ISerializable
            {
                #region Variaveis internas

                    #region Variáveis da árvore
                  
                    private int nivelMax;

                    private Metodo m_metodo;
                    public Metodo metodo
                    {
                        get { return m_metodo; }
                        set { m_metodo = value; }
                    }

                    private elementoArvore[][] m_niveis;
                    public elementoArvore[][] niveis
                    {
                        get { return m_niveis; }
                        set { m_niveis = value; }
                    }

                    private elementoArvore m_elementoSelecionado;
                    public elementoArvore elementoSelecionado
                    {
                        get { return m_elementoSelecionado; }
                        set { m_elementoSelecionado = value; }
                    }

                    private elementoArvore[] m_criteriosFinais;
                    public elementoArvore[] criteriosFinais
                    {
                        get { return m_criteriosFinais; }
                        set { m_criteriosFinais = value; }
                    }

                    private string[] m_alternativas;
                    public string[] alternativas
                    {
                        get { return m_alternativas; }
                        set { m_alternativas = value; }
                    }

                    private string m_outText;
                    public string outText
                    {
                        get { return this.m_outText; }
                        set { this.m_outText = value; }
                    }

                    #endregion

                    #region Variáveis do nó
                    
                    private double m_razaoConsistencia;
                    public double razaoConsistencia
                    {
                        get { return m_razaoConsistencia; }
                        set { m_razaoConsistencia = value; }
                    }
                    
                    private elementoArvore m_pai;
                    public elementoArvore pai
                    {
                        get { return m_pai; }
                        set { m_pai = value; }
                    }
                
                    private int m_numeroDescendentes;
                    public int numeroDescendentes
                    {
                        get { return m_numeroDescendentes; }
                        set { m_numeroDescendentes = value; }
                    }

                    private string m_fullPath;
                    public string fullPath
                    {
                        get { return m_fullPath; }
                        set { m_fullPath = value; }
                    }

                    private double m_pesoAcumulado;
                    public double pesoAcumulado
                    {
                        get { return m_pesoAcumulado; }
                        set { m_pesoAcumulado = value; }
                    }

                    private double m_peso;
                    public double peso
                    {
                        get { return m_peso; }
                        set { m_peso = value; }
                    }
                    
                    private string m_nome;
                    public string nome
                    {
                        get { return m_nome; }
                        set { m_nome = value; }
                    }

                    private elementoArvore[] m_filhos;
                    public elementoArvore[] filhos
                    {
                        get { return m_filhos; }
                        set { m_filhos = value; }
                    }

                    private double[,] m_matrizComparacao;
                    public double[,] matrizComparacao
                    {
                        get { return this.m_matrizComparacao; }
                        set { this.m_matrizComparacao = value; }
                    }

                    private double[,] m_matrizComparacaoMult;
                    public double[,] matrizComparacaoMult
                    {
                        get { return this.m_matrizComparacaoMult; }
                        set { this.m_matrizComparacaoMult = value; }
                    }

                    private bool m_isAlternativa;
                    public bool isAlternativa
                    {
                        get { return m_isAlternativa; }
                        set { m_isAlternativa = value; }
                    }

                    private int m_nivel;
                    public int nivel
                    {
                        get { return m_nivel; }
                        set { m_nivel = value; }
                    }

                    #endregion

                #endregion

                #region Métodos

                    #region construtores

                    private void construtorBase(string nome, Metodo metodo)
                    {
                        isAlternativa = false;
                        this.nome = nome;
                        m_fullPath = nome;
                        m_matrizComparacao = new double[0, 0];
                        filhos = new elementoArvore[0];
                        m_alternativas = new string[0];
                        m_criteriosFinais = new elementoArvore[0];
                        elementoSelecionado = null;
                        m_nivel = 0;
                        niveis = new elementoArvore[0][];
                        numeroDescendentes = 0;
                        pai = null;
                        nivelMax = 0;
                        peso = 1.0;
                        razaoConsistencia = 0.0;
                        outText = "";
                        this.metodo = metodo;
                    }

                    public elementoArvore(string nome, Metodo metodo)
                    {
                        construtorBase(nome, metodo);
                    }

                    public elementoArvore(string nome, int nivel, elementoArvore pai, Metodo metodo)
                    {
                        construtorBase(nome, metodo);
                        this.nivel = nivel;
                        this.pai = pai;
                    }

                    #endregion

                    #region adição de elementos

                    public void addAlternativa(string alternativaNova)
                    {
                        string[] alternativasOld = this.alternativas;
                        this.alternativas = new string[alternativasOld.GetLength(0) + 1];

                        for (int i = 0; i < alternativasOld.GetLength(0); i++)
                            this.alternativas[i] = alternativasOld[i];

                        this.alternativas[alternativasOld.GetLength(0)] = alternativaNova;

                        for (int i = 0; i < criteriosFinais.GetLength(0); i++)
                        {
                            criteriosFinais[i].addFilho(alternativaNova);
                            criteriosFinais[i].filhos[alternativas.GetLength(0) - 1].isAlternativa = true;
                        }
                    }

                    public void addCriterioFinal(elementoArvore novoCriterioFinal)
                    {
                        elementoArvore[] criteriosFinaisOld = criteriosFinais;
                        criteriosFinais = new elementoArvore[criteriosFinaisOld.GetLength(0) + 1];

                        for (int i = 0; i < criteriosFinaisOld.GetLength(0); i++)
                            criteriosFinais[i] = criteriosFinaisOld[i];

                        criteriosFinais[criteriosFinaisOld.GetLength(0)] = novoCriterioFinal;
                    }

                    public void addFilho(string nomeNovoFilho)
                    {
                        elementoArvore[] filhosOld = filhos;
                        filhos = new elementoArvore[filhosOld.GetLength(0) + 1];

                        for (int i = 0; i < filhosOld.GetLength(0); i++)
                            filhos[i] = filhosOld[i];

                        elementoArvore novoFilho = new elementoArvore(nomeNovoFilho, this.nivel + 1, this, this.metodo);
                        novoFilho.fullPath = this.fullPath + "\\" + novoFilho.nome;
                        filhos[filhos.GetLength(0) - 1] = novoFilho;

                        m_matrizComparacao = new double[filhos.GetLength(0), filhos.GetLength(0)];
                        for (int i = 0; i < filhos.GetLength(0); i++)
                            for (int j = 0; j < filhos.GetLength(0); j++)
                                m_matrizComparacao[i, j] = 1.0;

                        addDescendente(novoFilho.nivel);
                        isAlternativa = false;
                    }

                    public void addFilho(string nomeNovoFilho, double initMatriz)
                    {
                        addFilho(nomeNovoFilho);

                        for (int i = 0; i < matrizComparacao.GetLength(0); i++)
                        {
                            matrizComparacao[i, i] = initMatriz;
                            for (int j = i + 1; j < matrizComparacao.GetLength(1); j++)
                            {
                                matrizComparacao[i, j] = initMatriz;
                                matrizComparacao[j, i] = initMatriz;
                            }
                        }
                    }

                    #endregion

                    #region remoção de elementos

                    public bool removerFilho(elementoArvore no)
                    {
                        bool removeuFilho = false;

                        if (no.pai != this)
                            return false;

                        elementoArvore[] filhosNovo = new elementoArvore[this.filhos.GetLength(0) - 1];

                        for (int i = 0, j = 0; j < filhos.GetLength(0); j++, i++)
                            if ((filhos[j] == no)&&(!removeuFilho))
                            {
                                i--;
                                removeuFilho = true;
                            }
                            else
                                if(i < filhosNovo.GetLength(0))
                                    filhosNovo[i] = filhos[j];

                        m_filhos = filhosNovo;

                        m_matrizComparacao = new double[filhos.GetLength(0), filhos.GetLength(0)];
                        for (int i = 0; i < filhos.GetLength(0); i++)
                            for (int j = 0; j < filhos.GetLength(0); j++)
                                m_matrizComparacao[i, j] = 1.0;

                        removerDescendente();

                        return removeuFilho;
                    }

                    public bool removerFilho(string nomeFilho)
                    {
                        for (int i = 0; i < filhos.GetLength(0); i++)
                            if (filhos[i].nome == nomeFilho)
                                return removerFilho(filhos[i]);

                        return false;
                    }

                    public void removerElemento(elementoArvore no)
                    {
                        no.pai.removerFilho(no);
                    }

                    public bool removerAlternativa(string nomeAlternativa)
                    {
                        bool removeuAlternativa = false;

                        string[] alternativasOld = this.alternativas;
                        this.alternativas = new string[alternativasOld.GetLength(0) - 1];

                        for (int i = 0, j = 0; i < this.alternativas.GetLength(0); j++, i++)
                            if ((alternativasOld[j] == nomeAlternativa) && (!removeuAlternativa))
                            {
                                i--;
                                removeuAlternativa = true;
                            }
                            else
                                this.alternativas[i] = alternativasOld[j];

                        for (int i = 0; i < criteriosFinais.GetLength(0); i++)
                        {
                            if (!criteriosFinais[i].removerFilho(nomeAlternativa))
                                removeuAlternativa = false;
                        }

                        return removeuAlternativa;
                    }

                    #endregion

                    #region edição da árvore

                    public bool mudarMetodo(Metodo metodo)
                    {
                        if (this.metodo == metodo)
                            return false;

                        if (this.metodo == Metodo.HierarquicoClassico)
                            if (metodo == Metodo.HierarquicoMultiplicativo)
                                this.classico2multiplicativoArvore();

                        if (this.metodo == Metodo.HierarquicoMultiplicativo)
                            if (metodo == Metodo.HierarquicoClassico)
                                this.multiplicativo2classicoArvore();
                                               
                        return true;
                    }

                    private void classico2multiplicativoArvore()
                    {
                        this.classico2multiplicativo();

                        for (int i = 0; i < this.filhos.GetLength(0); i++)
                            this.filhos[i].classico2multiplicativoArvore();
                    }

                    private void multiplicativo2classico()
                    {
                        this.metodo = Metodo.HierarquicoClassico;

                        for (int i = 0; i < matrizComparacao.GetLength(0); i++)
                            for (int j = i; j < matrizComparacao.GetLength(1); j++)
                            {
                                matrizComparacao[i, j] = BLogicDecisaoMulticriterios.lootsma2saaty(matrizComparacao[i, j]);
                                matrizComparacao[j, i] = 1.0 / matrizComparacao[i, j];
                            }                               
                    }

                    private void multiplicativo2classicoArvore()
                    {
                        this.multiplicativo2classico();

                        for (int i = 0; i < this.filhos.GetLength(0); i++)
                            this.filhos[i].multiplicativo2classicoArvore();
                    }

                    private void classico2multiplicativo()
                    {
                        this.metodo = Metodo.HierarquicoMultiplicativo;

                        for (int i = 0; i < matrizComparacao.GetLength(0); i++)
                            for (int j = i; j < matrizComparacao.GetLength(1); j++)
                            {
                                matrizComparacao[i, j] = BLogicDecisaoMulticriterios.saaty2lootsma(matrizComparacao[i, j]);
                                matrizComparacao[j, i] = (-1.0)*matrizComparacao[i, j]; 
                            }
                    }

                    public void inicializaMatrizComparacao(double init)
                    {
                        for (int i = 0; i < matrizComparacao.GetLength(0); i++)
                        {
                            matrizComparacao[i, i] = init;
                            for (int j = i + 1; j < matrizComparacao.GetLength(1); j++)
                            {
                                matrizComparacao[i, j] = init;
                                matrizComparacao[j, i] = init;
                            }
                        }

                        for (int i = 0; i < filhos.GetLength(0); i++)
                            filhos[i].inicializaMatrizComparacao(init);
                    }

                    public elementoArvore retornaElemento(string[] fullPath)
                    {
                        elementoArvore elemento;

                        if (fullPath[0] == this.nome)
                            elemento = this;
                        else
                            return null;

                        for (int i = 0; i < fullPath.GetLength(0) - 1; i++)
                        {
                            if (fullPath[i] != elemento.nome)
                                return null;
                            else
                            {
                                bool achouFilho = false;
                                for (int j = 0; j < elemento.filhos.GetLength(0); j++)
                                {
                                    if (elemento.filhos[j].nome == fullPath[i + 1])
                                    {
                                        elemento = elemento.filhos[j];
                                        achouFilho = true;
                                        break;
                                    }
                                }
                                if (!achouFilho)
                                    return null;
                            }
                        }

                        return elemento;
                    }

                    public int atualizaNiveis(elementoArvore arvore, int x, int y)
                    {
                        arvore.niveis[x][y] = this;
                        int j = 0;

                        for (int i = 0; i < this.filhos.GetLength(0); i++)
                        {
                            j += Math.Max(filhos[i].atualizaNiveis(arvore, x + 1, y + j), 1);
                        }

                        return j;
                    }

                    private void addDescendente(int nivel)
                    {
                        numeroDescendentes++;

                        if (pai != null)
                            pai.addDescendente(nivel);
                        else
                            if (nivel > nivelMax)
                                nivelMax = nivel;
                    }

                    private void removerDescendente()
                    {
                        numeroDescendentes--;

                        if (pai != null)
                            pai.removerDescendente();
                    }
                   
                    private void lerArvore(elementoArvore arvore)
                    {
                        this.nivelMax = arvore.nivelMax;
                        this.niveis = arvore.niveis;
                        this.elementoSelecionado = arvore.elementoSelecionado;
                        this.criteriosFinais = arvore.criteriosFinais;
                        this.alternativas = arvore.alternativas;
                        this.outText = arvore.outText;
                        this.metodo = arvore.metodo;

                        this.pai = arvore.pai;
                        this.numeroDescendentes = arvore.numeroDescendentes;
                        this.fullPath = arvore.fullPath;
                        this.pesoAcumulado = arvore.pesoAcumulado;
                        this.peso = arvore.peso;
                        this.nome = arvore.nome;
                        this.filhos = arvore.filhos;
                        this.matrizComparacao = arvore.matrizComparacao;
                        this.isAlternativa = arvore.isAlternativa;
                        this.nivel = arvore.nivel;
                    }
                    
                    #endregion

                    #region I/O

                    public void imprimirArvore()
                    {
                        niveis = new elementoArvore[nivelMax + 1][];
                        for (int i = 0; i < niveis.GetLength(0); i++)
                        {
                            niveis[i] = new elementoArvore[numeroDescendentes + 1];
                            for (int j = 0; j < niveis[i].GetLength(0); j++)
                                niveis[i][j] = new elementoArvore("", metodo);
                        }

                        atualizaNiveis(this,0,0);

                        int tamMax = 6;

                        for(int i = 0 ; i < niveis.GetLength(0) ; i++)
                            for (int j = 0; j < niveis[i].GetLength(0); j++)
                            {
                                if (niveis[i][j].nome.Length > tamMax)
                                    tamMax = niveis[i][j].nome.Length;
                            }

                        string outText = "\n\n";

                        outText += "Estrutura hierárquica e pesos\n\n";
          
                        for (int i = 0; i < niveis.GetLength(0); i++)
                        {
                            for (int j = 0; j < niveis[i].GetLength(0); j++)
                            {
                                outText += niveis[i][j].nome + PreencheEspacos(tamMax - niveis[i][j].nome.Length + 2);
                            }

                            outText += "\n";

                            for (int j = 0; j < niveis[i].GetLength(0); j++)
                            { 
                                if(niveis[i][j].nome == "")
                                    outText += PreencheEspacos(tamMax + 2);
                                else
                                    outText += m_clt.Double2Texto(niveis[i][j].peso, 4) + PreencheEspacos(tamMax - 6 + 2);
                            }

                            outText += "\n\n";
                        }
                    

                        outText += "============================================================================================================================\n";
                        this.outText += outText;
                    }

                    public void imprimeRazaoConsistencia()
                    {
                        string outText = "\n";

                        outText += "Razão de consistencia (consistency ratio - CR) das matrizes de comparacao\n\n";

                        imprimeRazaoConsistenciaNo(ref outText);

                        outText += "\n***\nObs.: Razão de consistência aceitável para uma matriz de ordem n\nn = 3 : CR < 0,05\nn = 4 : CR < 0,08\nn > 4 : CR < 0,10\n***\n\n";

                        outText += "============================================================================================================================\n";
                        this.outText += outText;
                    }

                    private void imprimeRazaoConsistenciaNo(ref string outText)
                    {
                        if (this.matrizComparacao.GetLength(0) > 2)
                            outText += this.nome + ": " + m_clt.Double2Texto(this.razaoConsistencia, 3) + "\n";

                        for (int i = 0; i < this.filhos.GetLength(0); i++)
                            if (filhos[i].filhos.GetLength(0) > 0)
                                filhos[i].imprimeRazaoConsistenciaNo(ref outText);                       
                    }

                    public void importarArvore(string fileName)
                    {
                        FileStream file = new FileStream(fileName, FileMode.Open);
                        BinaryFormatter formatter = new BinaryFormatter();

                        elementoArvore arvore = (elementoArvore)formatter.Deserialize(file);

                        this.lerArvore(arvore);

                        file.Close();
                    }

                    public void exportarArvore(string fileName)
                    {
                        FileStream file = new FileStream(fileName, FileMode.Create);
                        BinaryFormatter formatter = new BinaryFormatter();

                        formatter.Serialize(file, this);

                        file.Close();
                    }

                    [SecurityPermission(SecurityAction.LinkDemand,
                    Flags = SecurityPermissionFlag.SerializationFormatter)]
                    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
                    {
                        if (info == null) throw new ArgumentNullException("info");
                        
                        info.AddValue("niveis", this.niveis);
                        info.AddValue("criteriosFinais", this.criteriosFinais);
                        info.AddValue("filhos", this.filhos);
                        
                        info.AddValue("elementoSelecionado", this.elementoSelecionado);
                        info.AddValue("pai", this.pai);
                        info.AddValue("metodo", this.metodo);
                        
                        info.AddValue("matrizComparecao", this.matrizComparacao);
                        info.AddValue("alternativas", this.alternativas);
                        
                        info.AddValue("nivelMax", this.nivelMax);                                                      
                        info.AddValue("outText", this.outText);                        
                        info.AddValue("numeroDescendentes", this.numeroDescendentes);
                        info.AddValue("fullPath",this.fullPath);
                        info.AddValue("pesoAcumulado",this.pesoAcumulado);
                        info.AddValue("peso",this.peso);
                        info.AddValue("nome",this.nome);                      
                        info.AddValue("isAlternativa",this.isAlternativa);
                        info.AddValue("nivel", this.nivel);
                    }

                    #endregion

                    #region cálculos

                    public void calculaConsistenciaArvore()
                    {
                        this.calculaConsistenciaNo();

                        for (int i = 0; i < this.filhos.GetLength(0); i++)
                            if (filhos[i].filhos.GetLength(0) > 0)
                                filhos[i].calculaConsistenciaArvore();
                    }

                    public double calculaConsistenciaNo()
                    {
                        double CI;
                        double RI;
                        double RC;
                        IpeaGeo.RegressoesEspaciais.Complex[] autoValores;
                        double[,] autoValoresReais;
                        double[,] matrizAux = new double[matrizComparacao.GetLength(0), matrizComparacao.GetLength(1)];
                        double autoValorMax;

                        for (int i = 0; i < matrizComparacao.GetLength(0); i++)
                            for (int j = i; j < matrizComparacao.GetLength(1); j++)
                                if (this.metodo == Metodo.HierarquicoMultiplicativo)
                                {
                                    matrizAux[i, j] = BLogicDecisaoMulticriterios.lootsma2saaty(matrizComparacao[i, j]);
                                    matrizAux[j, i] = 1.0 / matrizAux[i, j];
                                }
                                else
                                {
                                    matrizAux[i, j] = matrizComparacao[i, j];
                                    matrizAux[j, i] = matrizComparacao[j, i];
                                }

                        autoValores = new IpeaGeo.RegressoesEspaciais.Complex[matrizAux.GetLength(0)];
                        autoValoresReais = new double[matrizAux.GetLength(0), 1];

                        m_clt.AutovaloresMatrizAssimetrica(matrizAux, ref autoValores);

                        for(int i = 0 ; i < autoValoresReais.GetLength(0) ; i++)
                            autoValoresReais[i,0] = autoValores[i].Real;

                        autoValorMax = m_clt.Max(autoValoresReais);

                        CI = (autoValorMax - matrizAux.GetLength(0)) / (matrizAux.GetLength(0) - 1);

                        // Random index calculado de acordo com a abordagem de Saaty (2008, Relative Measurement and Its Generalization in Decision Making Why Pairwise Comparisons are Central in Mathematics for the Measurement of Intangible Factors The Analytic Hierarchy/Network Process

                        switch (matrizAux.GetLength(0))
                        {
                            case 3:
                                RI = 0.52;
                                break;
                            case 4:
                                RI = 0.89;
                                break;
                            case 5:
                                RI = 1.11;
                                break;
                            case 6:
                                RI = 1.25;
                                break;
                            case 7:
                                RI = 1.35;
                                break;
                            case 8:
                                RI = 1.40;
                                break;
                            case 9:
                                RI = 1.45;
                                break;
                            case 10:
                                RI = 1.49;
                                break;
                            case 11:
                                RI = 1.52;
                                break;
                            case 12:
                                RI = 1.54;
                                break;
                            case 13:
                                RI = 1.56;
                                break;
                            case 14:
                                RI = 1.58;
                                break;
                            case 15:
                                RI = 1.59;
                                break;
                            default:
                                RI = 1.6;
                                break;
                        }

                        if (matrizAux.GetLength(0) < 3)
                            RC = 0;
                        else
                            RC = CI / RI;


                        this.razaoConsistencia = RC;

                        return RC;
                    }

                    public void calculaPreferenciaMultiplicativaArvore()
                    {
                        this.calculaPreferenciaMultiplicativa();

                        for (int i = 0; i < this.filhos.GetLength(0); i++)
                            if (filhos[i].filhos.GetLength(0) > 0)
                                filhos[i].calculaPreferenciaMultiplicativaArvore();
                    }

                    public void calculaPreferenciaMultiplicativa()
                    {
                        matrizComparacaoMult = new double[matrizComparacao.GetLength(0), matrizComparacao.GetLength(1)];

                        for (int i = 0; i < matrizComparacao.GetLength(0); i++)
                        {
                            for (int j = 0; j < matrizComparacao.GetLength(1); j++)
                            {
                                matrizComparacaoMult[i, j] = Math.Pow(2, matrizComparacao[i, j]);
                            }
                        }
                    }

                    public double[,] calculaVetorNormalizado()
                    {
                        double[,] u = new double[matrizComparacao.GetLength(0), 1];
                        double[,] v = new double[matrizComparacao.GetLength(0), 1];
                        double[,] vNorm = new double[matrizComparacao.GetLength(0), 1];
                        double totalSoma;

                        double[,] A = matrizCoeficientes();

                        double[,] beta0 = matrizB0();

                        u = m_clt.MatrizMult(m_clt.MatrizInversa(A), beta0);

                        for (int i = 0; i < v.GetLength(0); i++)
                            v[i,0] = Math.Exp(u[i, 0]);

                        totalSoma = 0;
                        for (int i = 0; i < v.GetLength(0); i++)
                            totalSoma += v[i,0];

                        for (int i = 0; i < v.GetLength(0); i++)
                            vNorm[i, 0] = v[i, 0] / totalSoma;

                        return vNorm;
                    }

                    private double[,] matrizB0()
                    {
                        int n = matrizComparacaoMult.GetLength(0);

                        double[,] beta0 = new double[n, 1];

                        for (int i = 0; i < n - 1; i++)
                        {
                            beta0[i, 0] = 0;
                            for (int j = 0; j < n; j++)
                                if (j != i)
                                    beta0[i, 0] += Math.Log(matrizComparacaoMult[i, j]);
                        }

                        beta0[n - 1, 0] = 10.0;

                        return beta0;
                    }

                    private double[,] matrizCoeficientes()
                    {
                        int n = matrizComparacao.GetLength(0);

                        double[,] A = new double[n, n];

                        for (int i = 0; i < n; i++)
                            for (int j = 0; j < n; j++)
                                if (i == n - 1)
                                    A[i, j] = 1.0;
                                else
                                    if (j == i)
                                        A[i, j] = n - 1.0;
                                    else
                                        A[i, j] = -1.0;

                        return A;
                    }

                    #endregion

                #endregion
            }

            private elementoArvore m_arvore;
            public elementoArvore arvore
            {
                get { return m_arvore; }
            }

            #endregion

            #region Classe interna Resultado

            public class Resultado
            {
                #region Variaveis internas

                private string m_outText;
                public string outText
                {
                    get { return m_outText; }
                    set { m_outText = value; }
                }

                private string m_nomeMetodo;
                public string nomeMetodo
                {
                    get { return m_nomeMetodo; }
                    set { m_nomeMetodo = value; }
                }

                private string[,] m_nomeCriterios;
                private string m_nomeVariavelIdentificadora;
                public string nomeVariavelIdentificadora
                {
                    get { return m_nomeVariavelIdentificadora; }
                }

                private string[,] m_valoresVariavelIdentificadora;
                public string[,] valoresVariavelIdentificadora
                {
                    get { return m_valoresVariavelIdentificadora; }
                    set { m_valoresVariavelIdentificadora = value; }
                }

                double[,] m_indices;
                public double[,] indices
                {
                    get { return m_indices; }
                    set { m_indices = value; }
                }

                double[,] m_valorMax;
                double[,] m_valorMin;
                private double[,] m_pesos;

                #endregion

                #region Metodos

                public Resultado(string[,] nomeCriterios, string nomeVariavelIdentificadora, string[,] valoresVariavelIdentificadora, double[,] pesos)
                {
                    this.m_nomeCriterios = (string[,])nomeCriterios.Clone();
                    this.m_nomeVariavelIdentificadora = (string)nomeVariavelIdentificadora;
                    this.m_valoresVariavelIdentificadora = (string[,])valoresVariavelIdentificadora;
                    this.m_pesos = pesos;

                    this.outText = "";
                    this.m_indices = new double[valoresVariavelIdentificadora.GetLength(0), 1];
                    this.m_valorMax = new double[nomeCriterios.GetLength(0), 1];
                    this.m_valorMin = new double[nomeCriterios.GetLength(0), 1];
                }

                public Resultado()
                {
                    indices = new double[0, 0];
                }

                public void imprimirHierarquico()
                {
                    string outText = "\n============================================================================================================================\n\n";
                    int tamMax;

                    outText += "Cálculo de índices de apoio a decisão com multicritérios \n";
                    outText += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                    outText += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";
                    outText += "Método: " + this.nomeMetodo + "\n";
                    outText += "Número de alternativas: " + m_clt.Double2Texto(this.indices.GetLength(0), 0) + "\n\n";

                    tamMax = 23;

                    for (int i = 0; i < this.valoresVariavelIdentificadora.GetLength(0); i++)
                        if (valoresVariavelIdentificadora[i, 0].Length > tamMax)
                            tamMax = valoresVariavelIdentificadora[i, 0].Length;

                    outText += "Variável identificadora  " + PreencheEspacos(tamMax - 23) + "Índice\n";
                    for (int i = 0; i < this.valoresVariavelIdentificadora.GetLength(0); i++)
                        outText += valoresVariavelIdentificadora[i, 0] + "  " + PreencheEspacos(tamMax - valoresVariavelIdentificadora[i, 0].Length) + m_clt.Double2Texto(indices[i, 0], 6) + "\n";
                    outText += "\n\n";

                    outText += "============================================================================================================================\n";

                    this.outText = outText;
                }

                #endregion
            }

            private Resultado m_resultado;
            public Resultado resultado
            {
                get { return this.m_resultado; }
                set { m_resultado = value; }
            }

            #endregion

            #region Classe interna Entrada

            public class Entrada
            {
                #region Variaveis internas

                private Metodo m_metodo;
                public Metodo metodo
                {
                    get { return m_metodo; }
                    set { m_metodo = value; }
                }

                private string[,] m_nomeCriterios;
                public string[,] nomeCriterios
                {
                    get { return m_nomeCriterios; }
                    set { m_nomeCriterios = value; }
                }

                private double[][] m_valoresCriterios;
                public double[][] valoresCriterios
                {
                    get { return m_valoresCriterios; }
                    set { m_valoresCriterios = value; }
                }
                
                private double[,] m_pesos;
                public double[,] pesos
                {
                    get { return m_pesos; }
                    set { m_pesos = value; }
                }

                #endregion

                #region Metodos

                public Entrada(string[,] nomeCriterios, double[,] pesos, double[][] valoresCriterios, Metodo metodo)
                {
                    set(nomeCriterios, pesos, valoresCriterios);
                    this.metodo = metodo;
                }

                public Entrada(Metodo metodo)
                {
                    this.metodo = metodo;
                }

                public void set(string[,] nomeCriterios, double[,] pesos, double[][] valoresCriterios)
                {
                    this.nomeCriterios = (string[,])nomeCriterios.Clone();
                    this.pesos = (double[,])pesos.Clone();
                    this.valoresCriterios = (double[][])valoresCriterios.Clone();
                }

                #endregion
            }

            private Entrada m_entrada;
            public Entrada entrada
            {
                get { return this.m_entrada; }
                set { m_entrada = value; }
            }

            #endregion

        #endregion

        #region Metodos

            #region construtores

            public BLogicDecisaoMulticriterios()
            {

            }

            public BLogicDecisaoMulticriterios(string[,] nomesCriterios, double[,] pesos, double[][] valoresCriterios, string nomeVariavelIdentificadora, string[,] valoresVariavelIdentificadora, Metodo metodo)
            {
                m_resultado = new Resultado(nomesCriterios, nomeVariavelIdentificadora, valoresVariavelIdentificadora, pesos);
                m_entrada = new Entrada(nomesCriterios, pesos, valoresCriterios, metodo);
            }

            public BLogicDecisaoMulticriterios(string nome, Metodo metodo)
            {
                this.m_arvore = new elementoArvore(nome, metodo);

                this.resultado = new Resultado();

                this.entrada = new Entrada(metodo);
            }

            #endregion

            #region métodos "executar"
            
            public void executarHierarquicoClassico()
            {
                this.resultado.nomeMetodo = "Hierárquico Clássico";

                calcularPesosArvoreClassico(this.arvore);

                this.arvore.pesoAcumulado = 1.0;
                calcularPesosAcumuladosClassico(this.arvore);

                calcularIndicesHierarquicoClassico();
            }

            public void executarHierarquicoMultiplicativo()
            {
                this.resultado.nomeMetodo = "Hierárquico Multiplicativo";

                this.arvore.calculaPreferenciaMultiplicativaArvore();

                this.calcularPesosArvoreMultiplicativo(this.arvore);

                this.calcularIndicesHierarquicoMultiplicativo();
            }

            #endregion
      
            #region métodos do modelo Hierarquico

            public bool mudarMetodo(Metodo metodo)
            {
                if (arvore == null)
                    return false;

                if (arvore.metodo == metodo)
                    return false;

                entrada.metodo = metodo;

                return arvore.mudarMetodo(metodo);
            }

                #region classico

                private void calcularIndicesHierarquicoClassico()
                {
                    resultado.indices = new double[arvore.alternativas.GetLength(0), 1];
                    resultado.valoresVariavelIdentificadora = new string[arvore.alternativas.GetLength(0), 1];

                    for (int i = 0; i < resultado.indices.GetLength(0); i++)
                    {
                        resultado.indices[i, 0] = 0.0;
                        resultado.valoresVariavelIdentificadora[i, 0] = arvore.alternativas[i];

                        for (int j = 0; j < arvore.criteriosFinais.GetLength(0); j++)
                            resultado.indices[i, 0] += arvore.criteriosFinais[j].filhos[i].pesoAcumulado;
                    }
                }

                private void calcularPesosAcumuladosClassico(elementoArvore arvore)
                {
                    for (int i = 0; i < arvore.filhos.GetLength(0); i++)
                    {
                        arvore.filhos[i].pesoAcumulado = arvore.pesoAcumulado * arvore.filhos[i].peso;
                        calcularPesosAcumuladosClassico(arvore.filhos[i]);
                    }
                }
           
                private void calcularPesosArvoreClassico(elementoArvore arvore)
                {
                    calcularPesosNoClassico(arvore);

                    for (int i = 0; i < arvore.filhos.GetLength(0); i++)
                        if (arvore.filhos[i].isAlternativa)
                            return;
                        else
                            calcularPesosArvoreClassico(arvore.filhos[i]);
                }

                private void calcularPesosNoClassico(elementoArvore no)
                {
                    double[,] matrizComparacaoNorm = new double[no.matrizComparacao.GetLength(0), no.matrizComparacao.GetLength(1)];
                    double[] somaColunas = new double[no.matrizComparacao.GetLength(1)];

                    for (int i = 0; i < somaColunas.GetLength(0); i++)
                    {
                        somaColunas[i] = 0;
                        for (int j = 0; j < no.matrizComparacao.GetLength(0); j++)
                            somaColunas[i] += no.matrizComparacao[j, i];
                    }

                    for (int i = 0; i < matrizComparacaoNorm.GetLength(0); i++)
                        for (int j = 0; j < matrizComparacaoNorm.GetLength(1); j++)
                            matrizComparacaoNorm[i, j] = no.matrizComparacao[i, j] / somaColunas[j];

                    for (int i = 0; i < no.filhos.GetLength(0); i++)
                    {
                        no.filhos[i].peso = 0;
                        for (int j = 0; j < matrizComparacaoNorm.GetLength(1); j++)
                            no.filhos[i].peso += matrizComparacaoNorm[i, j];

                        no.filhos[i].peso /= matrizComparacaoNorm.GetLength(1);
                    }
                }

                #endregion

                #region multiplicativo

                private void calcularIndicesHierarquicoMultiplicativo()
                {
                    double[,] indices = new double[this.arvore.alternativas.GetLength(0), 1];
                    resultado.indices = new double[this.arvore.alternativas.GetLength(0), 1];
                    resultado.valoresVariavelIdentificadora = new string[arvore.alternativas.GetLength(0), 1];
                    double somaTotal;

                    for(int i = 0 ; i < indices.GetLength(0) ; i++)
                        indices[i,0] = calcularPesosAcumuladosMultiplicativo(this.arvore,i);

                    somaTotal = 0.0;
                    for (int i = 0; i < indices.GetLength(0); i++)
                        somaTotal += indices[i, 0];

                    for (int i = 0; i < indices.GetLength(0); i++)
                    {
                        this.resultado.indices[i, 0] = indices[i, 0] / somaTotal;
                        this.resultado.valoresVariavelIdentificadora[i, 0] = arvore.alternativas[i];
                    }                       
                }

                private double calcularPesosAcumuladosMultiplicativo(elementoArvore no, int alternativa)
                {
                    if (no.filhos.GetLength(0) < 1)
                        return double.NaN;

                    if (no.filhos[0].isAlternativa)
                        return no.filhos[alternativa].peso;

                    double peso = 1;

                    for (int i = 0; i < no.filhos.GetLength(0); i++)
                        peso *= Math.Pow(calcularPesosAcumuladosMultiplicativo(no.filhos[i], alternativa),no.filhos[i].peso);

                    return peso;
                }

                private void calcularPesosArvoreMultiplicativo(elementoArvore arvore)
                {
                    calcularPesosNoMultiplicativo(arvore);

                    for (int i = 0; i < arvore.filhos.GetLength(0); i++)
                        if (arvore.filhos[i].isAlternativa)
                            return;
                        else
                            calcularPesosArvoreMultiplicativo(arvore.filhos[i]);
                }

                private void calcularPesosNoMultiplicativo(elementoArvore no)
                {
                    double[,] vetNorm = no.calculaVetorNormalizado();

                    for (int i = 0; i < no.filhos.GetLength(0); i++)
                        no.filhos[i].peso = vetNorm[i, 0];
                }

                #endregion

            #endregion

            #region métodos estáticos

            static public double lootsma2saaty(double a)
            {
                double b;

                if (a >= 8.0) b = 9.0;
                else if (a >= 7.0) b = 8.0;
                else if (a >= 6.0) b = 7.0;
                else if (a >= 5.0) b = 6.0;
                else if (a >= 4.0) b = 5.0;
                else if (a >= 3.0) b = 4.0;
                else if (a >= 2.0) b = 3.0;
                else if (a >= 1.0) b = 2.0;
                else if (a >= 0.0) b = 1.0;
                else if (a >= -1.0) b = 1.0/2.0;
                else if (a >= -2.0) b = 1.0/3.0;
                else if (a >= -3.0) b = 1.0/4.0;
                else if (a >= -4.0) b = 1.0/5.0;
                else if (a >= -5.0) b = 1.0/6.0;
                else if (a >= -6.0) b = 1.0/7.0;
                else if (a >= -7.0) b = 1.0/8.0;
                else b = 1.0/9.0;

                return b;
            }

            static public double saaty2lootsma(double a)
            {
                double b;

                if (a >= 9.0) b = 8.0;
                else if (a >= 8.0) b = 7.0;
                else if (a >= 7.0) b = 6.0;
                else if (a >= 6.0) b = 5.0;
                else if (a >= 5.0) b = 4.0;
                else if (a >= 4.0) b = 3.0;
                else if (a >= 3.0) b = 2.0;
                else if (a >= 2.0) b = 1.0;
                else if (a >= 1.0) b = 0.0;
                else if (a >= (1.0 / 2.1)) b = -1.0;
                else if (a >= (1.0 / 3.1)) b = -2.0;
                else if (a >= (1.0 / 4.1)) b = -3.0;
                else if (a >= (1.0 / 5.1)) b = -4.0;
                else if (a >= (1.0 / 6.1)) b = -5.0;
                else if (a >= (1.0 / 7.1)) b = -6.0;
                else if (a >= (1.0 / 8.1)) b = -7.0;
                else if (a > 0) b = -8.0;
                else b = 0.0;
                
                return b;
            }

            static public string double2saaty(double a)
            {
                double b;
                string c;

                c = double2saaty(a, out b);

                return c;
            }

            static public string double2saaty(double a, out double b)
            {
                string c;

                if (a >= 9.0)
                {
                    b = 9.0;
                    c = "9";
                }
                else if (a >= 8.0)
                {
                    b = 8.0;
                    c = "8";
                }
                else if (a >= 7.0)
                {
                    b = 7.0;
                    c = "7";
                }
                else if (a >= 6.0)
                {
                    b = 6.0;
                    c = "6";
                }
                else if (a >= 5.0)
                {
                    b = 5.0;
                    c = "5";
                }
                else if (a >= 4.0)
                {
                    b = 4.0;
                    c = "4";
                }
                else if (a >= 3.0)
                {
                    b = 3.0;
                    c = "3";
                }
                else if (a >= 2.0)
                {
                    b = 2.0;
                    c = "2";
                }
                else if (a >= 1.0)
                {
                    b = 1.0;
                    c = "1";
                }
                else if (a >= (1.0 / 2.0))
                {
                    b = (1.0 / 2.0);
                    c = "1/2";
                }
                else if (a >= (1.0 / 3.0))
                {
                    b = (1.0 / 3.0);
                    c = "1/3";
                }
                else if (a >= (1.0 / 4.0))
                {
                    b = (1.0 / 4.0);
                    c = "1/4";
                }
                else if (a >= (1.0 / 5.0))
                {
                    b = (1.0 / 5.0);
                    c = "1/5";
                }
                else if (a >= (1.0 / 6.0))
                {
                    b = (1.0 / 6.0);
                    c = "1/6";
                }
                else if (a >= (1.0 / 7.0))
                {
                    b = (1.0 / 7.0);
                    c = "1/7";
                }
                else if (a >= (1.0 / 8.0))
                {
                    b = (1.0 / 8.0);
                    c = "1/8";
                }
                else if (a > 0)
                {
                    b = (1.0 / 9.0);
                    c = "1/9";
                }
                else
                {
                    b = 1.0;
                    c = "1";
                }
                return c;
            }

            static public string double2Lootsma(double a, out double b)
            {
                string c;

                if (a >= 8.0)
                {
                    b = 8.0;
                    c = "8";
                }
                else if (a >= 7.0)
                {
                    b = 7.0;
                    c = "7";
                }
                else if (a >= 6.0)
                {
                    b = 6.0;
                    c = "6";
                }
                else if (a >= 5.0)
                {
                    b = 5.0;
                    c = "5";
                }
                else if (a >= 4.0)
                {
                    b = 4.0;
                    c = "4";
                }
                else if (a >= 3.0)
                {
                    b = 3.0;
                    c = "3";
                }
                else if (a >= 2.0)
                {
                    b = 2.0;
                    c = "2";
                }
                else if (a >= 1.0)
                {
                    b = 1.0;
                    c = "1";
                }
                else if (a >= 0.0)
                {
                    b = 0.0;
                    c = "0";
                }
                else if (a >= -1.0)
                {
                    b = -1.0;
                    c = "-1";
                }
                else if (a >= -2.0)
                {
                    b = -2.0;
                    c = "-2";
                }
                else if (a >= -3.0)
                {
                    b = (-3.0);
                    c = "-3";
                }
                else if (a >= -4.0)
                {
                    b = -4.0;
                    c = "-4";
                }
                else if (a >= -5.0)
                {
                    b = -5.0;
                    c = "-5";
                }
                else if (a >= -6.0)
                {
                    b = -6.0;
                    c = "-6";
                }
                else if (a >= -7.0)
                {
                    b = -7.0;
                    c = "-7";
                }
                else
                {
                    b = -8.0;
                    c = "-8";
                }
                return c;
            }

            static public string double2Lootsma(double a)
            {
                string c;
                double b;

                c = double2Lootsma(a, out b);

                return c;
            }

            static public bool clear(ref BLogicDecisaoMulticriterios bldm)
            {
                if (bldm.entrada.metodo == Metodo.HierarquicoClassico || bldm.entrada.metodo == Metodo.HierarquicoMultiplicativo)
                    bldm = new BLogicDecisaoMulticriterios(bldm.arvore.nome, bldm.entrada.metodo);
                else
                    if (bldm.entrada.metodo == Metodo.Albebrico)
                        bldm = new BLogicDecisaoMulticriterios(bldm.entrada.nomeCriterios, bldm.entrada.pesos, bldm.entrada.valoresCriterios, bldm.resultado.nomeVariavelIdentificadora, bldm.resultado.valoresVariavelIdentificadora, bldm.entrada.metodo);
                    else
                        return false;

                return true;
            }

            static public bool importarArvore(ref BLogicDecisaoMulticriterios bldm, string fileName)
            {
                bool importouArvore = false;

                bldm = new BLogicDecisaoMulticriterios(bldm.arvore.nome, bldm.entrada.metodo);

                bldm.arvore.importarArvore(fileName);

                importouArvore = true;
                              
                return importouArvore;
            }

            #endregion

            #region Demais métodos
            
            #endregion

        #endregion
    }
}
