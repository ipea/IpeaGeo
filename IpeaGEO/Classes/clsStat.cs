using System;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Classes
{
    public class clsStat
    {
        #region Estatística Descritiva
        /// <summary>
        /// Calcula a média da variável
        /// </summary>
        /// <param name="dados">Vetor de dados</param>
        /// <returns>Retorna a média</returns>
        public double media(double[] dados)
        {
            double soma = 0;
            for (int i = 0; i < dados.Length; i++)
            {
                soma += dados[i];
            }
            return ((soma / (double)dados.Length));
        }

        /// <summary>
        /// Calcula a média da variável
        /// </summary>
        /// <param name="dados">Matriz de dados</param>
        /// <param name="col1">Coluna da matriz a qual se deseja calcular a média</param>
        /// <returns>Retorna a média</returns>
        public double media(double[,] dados, int col1)
        {
            double soma = 0;
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                soma += dados[i, col1];
            }
            return ((soma / (double)dados.GetLength(0)));
        }

        /// <summary>
        /// Calcula a variância dos dados do vetor
        /// </summary>
        /// <param name="dados">Vetor de dados</param>
        /// <returns>Retorna a variância</returns>
        public double variancia(double[] dados)
        {
            double soma = 0;
            for (int i = 0; i < dados.Length; i++)
            {
                soma += Math.Pow(dados[i], 2.0);
            }
            return (((soma / ((double)dados.Length - 1.0)) - (((double)dados.Length / ((double)dados.Length - 1.0)) * Math.Pow(media(dados), 2.0))));
        }

        /// <summary>
        /// Calcula a variância da variável
        /// </summary>
        /// <param name="dados">Matriz de dados</param>
        /// <param name="coluna">Coluna da matriz a qual se deseja calcular a variância</param>
        /// <returns>Retorna a variância</returns>
        public double variancia(double[,] dados, int coluna)
        {
            double soma = 0;
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                soma += Math.Pow(dados[i, coluna], 2);
            }
            return (((soma / ((double)dados.GetLength(0) - 1.0)) - (((double)dados.GetLength(0) / ((double)dados.GetLength(0) - 1.0)) * (Math.Pow(media(dados, coluna), 2.0)))));
        }


        /// <summary>
        /// Calcula a assimetria da distribuição dos dados da variável
        /// </summary>
        /// <param name="dados">Vetor de dados</param>
        /// <returns>Retorna a assimetria</returns>
        public double assimetria(double[] dados)
        {
            double soma = 0;
            for (int i = 0; i < dados.Length; i++)
            {
                soma += Math.Pow((dados[i] - media(dados)) / (Math.Pow(variancia(dados), 0.5)), 3.0);
            }
            return (((double)dados.Length / ((double)dados.Length - 2.0)) * (1.0 / ((double)dados.Length - 1.0)) * soma);
        }


        /// <summary>
        /// Calcula a kurtose (achatamento) da distribuição dos dados da variável
        /// </summary>
        /// <param name="dados">Vetor de dados</param>
        /// <returns>Retorna a kurtose</returns>
        public double kurtose(double[] dados)
        {
            double soma = 0;
            for (int i = 0; i < dados.Length; i++)
            {
                soma += Math.Pow((dados[i] - media(dados)) / (Math.Pow(variancia(dados), 0.5)), 4.0);

            }

            double g1 = (((double)dados.Length) * ((double)dados.Length + 1.0)) / (((double)dados.Length - 1.0) * ((double)dados.Length - 2.0) * ((double)dados.Length - 3.0));
            double g2 = (Math.Pow(((double)dados.Length - 1.0), 2.0)) / (((double)dados.Length - 2.0) * ((double)dados.Length - 3.0));

            return ((g1 * soma) - (3.0 * g2));


        }

        /// <summary>
        /// Calcula a covariância entre duas variáveis
        /// </summary>
        /// <param name="x">Vetor de dados da primeira variável</param>
        /// <param name="y">Vetor de dados da segunda variável</param>
        /// <returns>Retorna a covariância entre x e y</returns>
        public double covariancia(double[] x, double[] y)
        {
            double[] xy = new double[x.Length];
            double soma = 0;
            if (x.Length == y.Length)
            {
                for (int i = 0; i < x.Length; i++)
                {
                    soma += ((x[i] - media(x)) * (y[i] - media(y)));
                }

                return (soma / ((double)x.Length - 1));
            }
            else
            {
                return (double.MaxValue);
            }
        }

        /// <summary>
        /// Calcula a covariância entre duas variáveis
        /// </summary>
        /// <param name="x">Matriz de dados</param>
        /// <param name="col1">Coluna na matriz de dados correspondente à primeira variável</param>
        /// <param name="col2">Coluna na matriz de dados correspondente à segunda variável</param>
        /// <returns>Retorna a covariância entre as duas variáveis</returns>
        public double covariancia(double[,] x, int col1, int col2)
        {
            double[] xy = new double[x.GetLength(0)];
            double soma = 0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                soma += ((x[i, col1] - media(x, col1)) * (x[i, col2] - media(x, col2)));

            }

            return (soma / ((double)x.GetLength(0) - 1.0));
        }

        /// <summary>
        /// Calcula a correlação linear entre duas variáveis
        /// </summary>
        /// <param name="x">Vetor de dados da primeira variável</param>
        /// <param name="y">Vetor de dados da segunda variável</param>
        /// <returns>Retorna o índice de correlação linear entre as duas variáveis</returns>
        public double correlacao(double[] x, double[] y)
        {
            if (x.Length == y.Length)
            {

                return (covariancia(x, y) / (Math.Sqrt(variancia(x)) * Math.Sqrt(variancia(y))));
            }
            else
            {
                return (double.MaxValue);

            }
        }

        /// <summary>
        /// Calcula a correlação linear entre duas variáveis
        /// </summary>
        /// <param name="x">Matriz de dados</param>
        /// <param name="col1">Coluna na matriz de dados correspondente à primeira variável</param>
        /// <param name="col2">Coluna na matriz de dados correspondente à segunda variável</param>
        /// <returns>Retorna o índice de correlação linear entre as duas variáveis</returns>
        public double correlacao(double[,] x, int col1, int col2)
        {
            return (covariancia(x, col1, col2) / (Math.Sqrt(variancia(x, col1)) * Math.Sqrt(variancia(x, col2))));

        }
        /// <summary>
        /// Calcula a matriz de covariância dos dados
        /// </summary>
        /// <param name="x">Matriz de dados</param>
        /// <returns>Retorna a matriz de covariância das variáveis da base</returns>
        public double[,] matrizcorv(double[,] x)
        {
            double[,] m = new double[x.GetLength(1), x.GetLength(1)];

            for (int row = 0; row < x.GetLength(0); row++)
            {
                for (int col = 0; col < x.GetLength(1); col++)
                {
                    m[row, col] = covariancia(x, row, col);
                }

            }

            return (m);

        }

        /// <summary>
        /// Calcula a matriz de correlação linear dos dados
        /// </summary>
        /// <param name="x">Matriz de dados</param>
        /// <returns>Retorna a matriz dos índices de correlação linear das variáveis</returns>
        public double[,] matrizcorr(double[,] x)
        {
            double[,] cor = new double[x.GetLength(1), x.GetLength(1)];
            for (int row = 0; row < x.GetLength(0); row++)
            {
                for (int col = 0; col < x.GetLength(1); col++)
                {
                    cor[row, col] = correlacao(x, row, col);

                }

            }
            return (cor);
        }



        #endregion

        #region Índices de Concentração Espacial

        /// <summary>
        /// Calcula o Isard Index, que se baseia na distância absoluta entre a distribuição atual e a distribuição de referência
        /// </summary>
        /// <param name="x">Matriz de dados</param>
        /// <returns>Retorna um vetor com o Isard Index de cada setor</returns>
        public double[] isard(double[,] x)
        {
            double soma_t = 0;
            double[] resultado = new double[x.GetLength(1)];
            double[] soma_s = new double[x.GetLength(1)];
            double[] soma_r = new double[x.GetLength(0)];

            for (int s = 0; s < x.GetLength(1); s++)
            {
                soma_t += soma_s[s];
                for (int r = 0; r < x.GetLength(0); r++)
                {
                    soma_s[s] += x[r, s];
                    soma_r[r] += x[r, s];

                }
            }

            for (int s = 0; s < x.GetLength(1); s++)
            {

                for (int r = 0; r < x.GetLength(0); r++)
                {
                    resultado[s] += Math.Abs((x[r, s] / soma_s[s]) - (soma_r[r] / soma_t));
                }
                resultado[s] /= 2.0;
            }

            return (resultado);

        }

        /// <summary>
        /// Calcula o Herfindhal Index, que se baseia na soma ponderada dos quadrados da "participação" de cada setor da região   
        /// </summary>
        /// <param name="x">Matriz de dados</param>
        /// <returns>Retorna um vetor com o Herfindhal Index correspondente a cada setor</returns>
        public double[] herfindhal(double[,] x)
        {
            double[] soma_s = new double[x.GetLength(1)];
            double[] soma_r = new double[x.GetLength(0)];
            double soma_t = 0;
            double[] resultado = new double[x.GetLength(1)];

            for (int s = 0; s < x.GetLength(1); s++)
            {
                soma_t += soma_s[s];

                for (int r = 0; r < x.GetLength(0); r++)
                {
                    soma_s[s] += x[r, s];
                    soma_r[r] += x[r, s];

                }

            }
            for (int s = 0; s < x.GetLength(1); s++)
            {

                for (int r = 0; r < x.GetLength(0); r++)
                {

                    resultado[s] += (soma_r[r] / soma_t) * (Math.Pow(((x[r, s] * soma_t) / (soma_s[s] * soma_r[r])), 2.0));

                    resultado[s] /= ((double)x.GetLength(0));
                }

            }

            return (resultado);
        }

        /// <summary>
        /// Calcula a entropia de cada setor
        /// </summary>
        /// <param name="x">Matriz de dados</param>
        /// <param name="y">Parâmetro alpha</param>
        /// <returns>Retorna o vetor dos índices de entropia para cada setor</returns>
        public double[] entropia(double[,] x, double y)
        {
            double[] soma_s = new double[x.GetLength(1)];
            double[] soma_r = new double[x.GetLength(0)];
            double soma_t = 0;
            double[] resultado = new double[x.GetLength(1)];

            for (int s = 0; s < x.GetLength(1); s++)
            {
                soma_t += soma_s[s];

                for (int r = 0; r < x.GetLength(0); r++)
                {
                    soma_s[s] += x[r, s];
                    soma_r[r] += x[r, s];

                }

            }
            for (int s = 0; s < x.GetLength(1); s++)
            {

                for (int r = 0; r < x.GetLength(0); r++)
                {

                    resultado[s] += (soma_r[r] / soma_t) * (Math.Pow(((x[r, s] / soma_s[s]) * (soma_t / soma_r[r])), y));
                    resultado[s] += (-1.0);
                    resultado[s] /= (Math.Pow(y, 2.0) - y);

                }

            }
            return (resultado);
        }

        #endregion

        #region Textmining



        /// <summary>
        /// The Levenshtein distance between two strings is given by the minimum number of operations needed to transform one string into the other, 
        /// where an operation is an insertion, deletion, or substitution of a single character.
        /// </summary>
        /// <param name="palavra1">Word to be compared</param>
        /// <param name="palavra2">Word to compare with the first one</param>
        /// <returns>Levenshtein Distance</returns>
        public int levenshteindistance(string palavra1, string palavra2)
        {
            int[,] d = new int[palavra1.Length + 1, palavra2.Length + 1];
            char[] s = new char[palavra1.Length + 1];
            char[] t = new char[palavra2.Length + 1];
            s = palavra1.ToCharArray();
            t = palavra2.ToCharArray();


            for (int i = 0; i <= palavra1.Length; i++)
            {
                d[i, 0] = i;

            }

            for (int j = 0; j <= palavra2.Length; j++)
            {
                d[0, j] = j;

            }

            for (int j = 1; j <= palavra2.Length; j++)
            {
                for (int i = 1; i <= palavra1.Length; i++)
                {


                    if (s[i - 1] == t[j - 1])
                    {
                        d[i, j] = d[i - 1, j - 1];

                    }

                    else
                    {
                        d[i, j] = Math.Min((d[i - 1, j] + 1), (d[i, j - 1] + 1));
                        d[i, j] = Math.Min(d[i, j], (d[i - 1, j - 1] + 1));

                    }

                }

            }

            return d[palavra1.Length, palavra2.Length];

        }


        /// <summary>
        /// Is a "distance" (string metric) between two strings, i.e., 
        /// finite sequence of symbols, given by counting the minimum number of operations needed to transform one string into the other, 
        /// where an operation is defined as an insertion, deletion, or substitution of a single character, or a transposition of two characters.
        /// </summary>
        /// <param name="palavra1">Word to be compared</param>
        /// <param name="palavra2">Word to compare with the first one</param>
        /// <returns>Damerau-Levenshtein Distance</returns>
        public int dameraudistance(string palavra1, string palavra2)
        {
            int[,] d = new int[palavra1.Length + 1, palavra2.Length + 1];
            char[] s = new char[palavra1.Length + 1];
            char[] t = new char[palavra2.Length + 1];
            s = palavra1.ToCharArray();
            t = palavra2.ToCharArray();
            int cost = 0;

            for (int i = 0; i <= palavra1.Length; i++)
            {
                d[i, 0] = i;

            }

            for (int j = 0; j <= palavra2.Length; j++)
            {
                d[0, j] = j;

            }

            for (int i = 1; i <= palavra1.Length; i++)
            {
                for (int j = 1; j <= palavra2.Length; j++)
                {

                    if (s[i - 1] == t[j - 1])
                    {
                        cost = 0;

                    }

                    else
                    {
                        cost = 1;

                    }

                    d[i, j] = Math.Min((d[i - 1, j] + 1), (d[i, j - 1] + 1));
                    d[i, j] = Math.Min(d[i, j], (d[i - 1, j - 1] + cost));

                    if ((i > 1) && (j > 1) && (s[i - 1] == t[j - 2]) && (s[i - 2] == t[j - 1]))
                    {
                        d[i, j] = Math.Min(d[i, j], (d[i - 2, j - 2] + cost));

                    }
                }

            }

            return d[palavra1.Length, palavra2.Length];

        }

        /// <summary>
        ///  Is the number of symbols that are different from the zero-symbol ("0" or " "). 
        /// </summary>
        /// <param name="palavra1"></param>
        /// <returns>Hamming Weight</returns>
        public int hammingweight(string palavra1)
        {
            char[] s = new char[palavra1.Length];
            s = palavra1.ToCharArray();
            int weight = 0;
            for (int i = 0; i < palavra1.Length; i++)
            {
                if (s[i].ToString() == " " || s[i].ToString() == "0")
                {
                    weight = weight;

                }


                else
                {
                    weight = weight + 1;


                }


            }
            return (weight);

        }

        public int hammingweight(int numero1)
        {
            string numero2 = numero1.ToString();
            char[] s = new char[numero2.Length];
            s = numero2.ToCharArray();
            int weight = 0;



            for (int i = 0; i < numero2.Length; i++)
            {
                if (s[i].ToString() == "0")
                {
                    weight = weight;

                }

                else
                {

                    weight = weight + 1;

                }


            }

            return (weight);

        }

        /// <summary>
        /// It measures the minimum number of substitutions required to change one string into the other, 
        /// or the number of errors that transformed one string into the other. They must have an equal length.
        /// </summary>
        /// <param name="palavra1"></param>
        /// <param name="palavra2"></param>
        /// <returns>Hamming Distance</returns>
        public int hammingdistance(string palavra1, string palavra2)
        {

            if (palavra1.Length == palavra2.Length)
            {
                char[] s = new char[palavra1.Length];
                char[] t = new char[palavra2.Length];
                s = palavra1.ToCharArray();
                t = palavra2.ToCharArray();
                int d = 0;

                for (int i = 0; i < palavra1.Length; i++)
                {
                    if (s[i].ToString() == t[i].ToString())
                    {
                        d = d;

                    }

                    else
                    {
                        d = d + 1;

                    }


                }
                return (d);

            }

            else
            {
                return (99999);

            }


        }







        #endregion

        #region K-means

        public double[,] classes(double[,] x)
        {

            int num_clas = 1;
            for (int i = 1; i < x.GetLength(0); i++)
            {
                if (x[i, 1] != x[i - 1, 1])
                {
                    num_clas++;

                }

            }

            double soma = 0;
            double count = 0;
            double media = 0;
            double[] medias = new double[num_clas];
            int m = 0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                count++;
                soma += x[i, 0];
                if (i < x.GetLength(0) - 1)
                {
                    if (x[i, 1] != x[i + 1, 1])
                    {
                        media = soma / count;
                        medias[m] = media;
                        m++;
                        count = 0;
                        soma = 0;
                    }
                }
            }
            media = soma / count;
            medias[m] = media;
            m++;
            count = 0;
            soma = 0;

            int k = 0;
            //int v = 0;

            for (int i = 0; i < x.GetLength(0); i++)
            {
                if (k < medias.Length)
                {
                    if (x[i, 0] < medias[k])
                    {
                        x[i, 1] = k;

                    }
                    else
                    {
                        k++;
                        x[i, 1] = k;
                    }
                }
                else
                {
                    x[i, 1] = k;
                }

            }
            return (x);

        }
        /// <summary>
        /// Retorna matriz de 2 colunas: dados e seus respectivos grupos
        /// </summary>
        /// <param name="x">Vetor de dados</param>
        /// <param name="y">Número de classes</param>
        /// <returns></returns>
        public double[,] kmeans(double[,] x, double y)
        {
            clsUtilTools m_clt = new clsUtilTools();

            m_clt.SortcDoubleArray(x);
            //Array.Sort(x);
            double[,] matriz = new double[x.GetLength(0), 2];

            for (int i = 0; i < x.GetLength(0); i++)
            {
                matriz[i, 0] = x[i, 0];


            }
            for (int i = 0; i < y - 1; i++)
            {
                matriz = classes(matriz);

            }

            return (matriz);
        }

        #endregion
    }
}
