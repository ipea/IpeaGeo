using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Collections;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using System.Threading;

namespace IpeaGeo.RegressoesEspaciais
{
    #region Enumerações

    public enum TipoEstatisticaAmostra : int
    {
        N = 1,
        Soma = 2,
        Media = 3,
        VarianciaPopulacional = 4,
        DesvioPadraoPopulacional = 5,
        Minimo = 6,
        Maximo = 7
    };

    #endregion

    #region Funções matriciais e utilitarios

    class clsUtilTools
    {
        #region Contagem de numero de observaçoes para um vetor de intervalos

        /// <summary>
        /// Conta o número de elementos em uma matriz, em intervalos de dados especificados em um vetor de dados. A contagem considera intervalo fechado para o máximo do intervalo.
        /// </summary>
        /// <param name="dados">Matriz de dados.</param>
        /// <param name="vetor_intervalos">Vetor coluna com os valores dos intervalos.</param>
        /// <param name="categoria_intervalo">Retorna uma matriz, de mesma dimensão da matriz dados, contendo o ordinal do intervalo para o qual a observação pertence.</param>
        public int[] ContaFrequenciaEmIntervalos(double[,] dados, double[,] vetor_intervalos, out object[,] categoria_intervalo)
        {
            int[] contagem = new int[vetor_intervalos.GetLength(0) + 1];

            categoria_intervalo = new object[dados.GetLength(0), dados.GetLength(1)];

            double[,] vetor_ext = new double[vetor_intervalos.GetLength(0) + 2, 1];
            vetor_ext[0, 0] = this.Min(dados) - 1;
            vetor_ext[vetor_ext.GetLength(0) - 1, 0] = this.Max(dados) + 1;
            for (int i = 0; i < vetor_intervalos.GetLength(0); i++)
            {
                vetor_ext[i + 1, 0] = vetor_intervalos[i, 0];
            }

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                for (int j = 0; j < dados.GetLength(1); j++)
                {
                    for (int k = 0; k < vetor_ext.GetLength(0) - 1; k++)
                    {
                        if (dados[i, j] <= vetor_ext[k + 1, 0] && dados[i, j] > vetor_ext[k, 0])
                        {
                            categoria_intervalo[i, j] = k + 1;
                            contagem[k]++;
                            break;
                        }
                    }
                }
            }

            return contagem;
        }

        /// <summary>
        /// Conta o número de elementos em uma matriz, em intervalos de dados especificados em um vetor de dados. A contagem considera intervalo fechado para o máximo do intervalo.
        /// </summary>
        /// <param name="dados">Matriz de dados.</param>
        /// <param name="vetor_intervalos">Vetor coluna com os valores dos intervalos.</param>
        public int[] ContaFrequenciaEmIntervalos(double[,] dados, double[,] vetor_intervalos)
        {
            int[] contagem = new int[vetor_intervalos.GetLength(0)+1];

            double[,] vetor_ext = new double[vetor_intervalos.GetLength(0) + 2, 1];
            vetor_ext[0, 0] = this.Min(dados)-1;
            vetor_ext[vetor_ext.GetLength(0) - 1, 0] = this.Max(dados)+1;
            for (int i = 0; i < vetor_intervalos.GetLength(0); i++)
            {
                vetor_ext[i + 1, 0] = vetor_intervalos[i, 0];
            }

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                for (int j = 0; j < dados.GetLength(1); j++)
                {
                    for (int k = 0; k < vetor_ext.GetLength(0) - 1; k++)
                    {
                        if (dados[i, j] <= vetor_ext[k + 1, 0] && dados[i, j] > vetor_ext[k, 0])
                        {
                            contagem[k]++;
                            break;
                        }
                    }
                }
            }

            return contagem;
        }
        
        #endregion

        #region Funções auxiliares

        public string[] RetornaIntersecaoDuasListas(string[] v1, string[] v2)
        {
            ArrayList l1 = new ArrayList();
            ArrayList l = new ArrayList();
            for (int i = 0; i < v1.GetLength(0); i++)
            {
                l1.Add(v1[i]);
            }

            for (int i = 0; i < v2.GetLength(0); i++)
            {
                if (l1.Contains(v2[i])) l.Add(v2[i]);
            }

            string[] res = new string[l.Count];
            for (int i = 0; i < l.Count; i++) res[i] = Convert.ToString(l[i]);

            return res;
        }

        public bool ExportToTXT(string conteudo, string fileName)
        {
            try
            {
                StreamWriter stream = new StreamWriter(fileName);
                stream.Write(conteudo);
                stream.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Processo de exportação para arquivo txt falhou." + ex.InnerException.ToString());
            }
        }

        #endregion
        
        #region importação de arquivos texto

        public void LerArquivoTextoDelimited(string arquivo, ref DataTable dt, 
            bool delimitado_pontovirgula, bool delimitado_virgula, bool delimitado_caracter, 
            string caracter_delimitacao, bool nomes_primeira_linha, bool formato_numero_portugues)
        {
            StringBuilder sb = new StringBuilder();
            dt = new DataTable();
            bool primeira_linha = true;

            char[] s = new char[] { '\t' };
            if (delimitado_pontovirgula) s = new char[] { ';' };
            if (delimitado_virgula) s = new char[] { ',' };
            if (delimitado_caracter)
            {
                s[0] = Convert.ToChar(caracter_delimitacao.Substring(0, 1));
            }

            string[] dados = new string[0];
            string nome_nova_variavel = "";

            clsUtilTools clt = new clsUtilTools();

            using (StreamReader sr = File.OpenText(arquivo))
            {
                String input;
                while ((input = sr.ReadLine()) != null)
                {
                    if (primeira_linha)
                    {
                        dados = input.Split(s);
                        if (nomes_primeira_linha)
                        {
                            for (int i = 0; i < dados.GetLength(0); i++)
                            {
                                nome_nova_variavel = clt.nome_nova_coluna(dt, dados[i]);
                                dt.Columns.Add(nome_nova_variavel, typeof(string));
                            }
                        }
                        else
                        {
                            int contador_variavel = 0;
                            for (int i = 0; i < dados.GetLength(0); i++)
                            {
                                nome_nova_variavel = "V_" + contador_variavel;
                                contador_variavel++;
                                dt.Columns.Add(nome_nova_variavel, typeof(string));
                            }

                            dt.Rows.Add(dados);
                        }

                        primeira_linha = false;
                    }
                    else
                    {
                        dados = input.Split(s);
                        dt.Rows.Add(dados);
                    }
                }
            }

            double[,] valores = new double[0, 0];
            ArrayList colunas_numericas = new ArrayList();
            ArrayList lista_valores = new ArrayList();

            for (int k = 0; k < dt.Columns.Count; k++)
            {
                if (ChecaColunaNumerica(dt, k, ref valores, formato_numero_portugues))
                {
                    colunas_numericas.Add(dt.Columns[k].ColumnName);
                    lista_valores.Add(valores);
                }
            }

            DataTable dt_novo = new DataTable();
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                if (colunas_numericas.Contains(dt.Columns[k].ColumnName))
                {
                    dt_novo.Columns.Add(dt.Columns[k].ColumnName, typeof(double));
                }
                else
                {
                    dt_novo.Columns.Add(dt.Columns[k].ColumnName, dt.Columns[k].DataType);
                }
            }

            object[] itens = new object[dt.Columns.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt_novo.Rows.Add(itens);
            }

            int contador = 0;
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                if (colunas_numericas.Contains(dt.Columns[k].ColumnName))
                {
                    valores = (double[,])lista_valores[contador];
                    contador++;
                    for (int i = 0; i < valores.GetLength(0); i++)
                    {
                        dt_novo.Rows[i][dt.Columns[k].ColumnName] = valores[i, 0];
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt_novo.Rows[i][dt.Columns[k].ColumnName] = dt.Rows[i][dt.Columns[k].ColumnName];
                    }
                }
            }

            dt = dt_novo.Copy();
        }

        public bool ChecaColunaNumerica(DataTable dt, int num_coluna, ref double[,] valores, bool formato_numero_portugues)
        {
            bool res = true;
            valores = new double[dt.Rows.Count, 1];
            double v = 0.0;
            string s = "";

            NumberStyles style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol
                                | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowTrailingSign
                                | NumberStyles.AllowTrailingWhite;
            CultureInfo culture1 = CultureInfo.CreateSpecificCulture("en-GB");
            CultureInfo culture2 = CultureInfo.CreateSpecificCulture("en-US");
            CultureInfo culture3 = CultureInfo.CreateSpecificCulture("pt-BR");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                res = false;
                s = dt.Rows[i][num_coluna].ToString();

                if (s.IndexOf(",") != -1 && s.IndexOf(".") == -1)
                {
                    formato_numero_portugues = true;
                }
                if (!formato_numero_portugues && Double.TryParse(s, style, culture1, out v))
                {
                    valores[i, 0] = v;
                    res = true;
                }
                else
                {
                    if (!formato_numero_portugues && Double.TryParse(s, style, culture2, out v))
                    {
                        valores[i, 0] = v;
                        res = true;
                    }
                    else
                    {
                        if (formato_numero_portugues && Double.TryParse(s, style, culture3, out v))
                        {
                            valores[i, 0] = v;
                            res = true;
                        }
                    }
                }
                if (!res)
                {
                    return false;
                }
            }

            return res;
        }

        #endregion

        #region atribuindo nome novo a uma variável, para evitar duplicação de nomes

        /// <summary>
        /// Retorno o nome da nova variávei para entrar na tabela, evitando duplicação. 
        /// Quando o nome já existir, será dado um nome com o _1 no final. Quando já houver
        /// variável com esse sufixo, será um incluído um sufixo incrementado até não haver mais 
        /// a duplicação (_1, _2, ...). 
        /// </summary>
        /// <param name="dt">Lista de nomes inicial, para checagem da duplicação dos nomes das colunas.</param>
        /// <param name="nome">Nome tetantivo da variável nova.</param>
        /// <returns>Retorno no nome sem duplicação.</returns>
        public string nome_nova_coluna(ArrayList nomes_existentes, string nome)
        {
            string res = nome;
            if (!nomes_existentes.Contains(res)) return res;

            if (res.Substring(res.Length - 1, 1) == "_")
            {
                return res + "1"; ;
            }

            int posicao = -1;

            for (int i = res.Length - 1; i >= 0; i--)
            {
                if (res.Substring(i, 1) == "_")
                {
                    posicao = i;
                    break;
                }
            }

            if (posicao >= 0)
            {
                string snumero = res.Substring(posicao + 1, res.Length - posicao);
                int numero = 0;
                if (Int32.TryParse(snumero, out numero))
                {
                    res = res.Substring(0, posicao + 1) + (numero + 1).ToString();
                    return res;
                }
                else
                {
                    return res + "_1";
                }
            }

            return nome_nova_coluna(nomes_existentes, res + "_1");
        }

        /// <summary>
        /// Retorno o nome da nova variávei para entrar na tabela, evitando duplicação. 
        /// Quando o nome já existir, será dado um nome com o _1 no final. Quando já houver
        /// variável com esse sufixo, será um incluído um sufixo incrementado até não haver mais 
        /// a duplicação (_1, _2, ...). 
        /// </summary>
        /// <param name="dt">Tabela inicial, para checagem da duplicação dos nomes das colunas.</param>
        /// <param name="nome">Nome tetantivo da variável nova.</param>
        /// <returns>Retorno no nome sem duplicação.</returns>
        public string nome_nova_coluna(DataTable dt, string nome)
        {
            ArrayList nomes_existentes = new ArrayList();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                nomes_existentes.Add(dt.Columns[i].ColumnName);
            }

            return nome_nova_coluna(nomes_existentes, nome);
        }

        #endregion

        #region datatable para matriz de objetos

        public object[,] DataTableToObjectMatrix(DataTable dt, string colname)
        {
            object[,] res = new object[dt.Rows.Count, 1];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                res[i, 0] = dt.Rows[i][colname];
            }
            return res;
        }

        public object[,] DataTableToObjectMatrix(DataTable dt, string[] colnames)
        {
            object[,] res = new object[dt.Rows.Count, colnames.GetLength(0)];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < colnames.GetLength(0); j++)
                {
                    res[i, j] = dt.Rows[i][colnames[j]];
                }
            }
            return res;
        }

        #endregion

        #region distância entre vetores linha

        public double distancia_entre_vetores_linha(double[,] v1, double[,] v2)
        {
            double res = 0.0;

            for (int i = 0; i < v1.GetLength(1); i++)
            {
                res += Math.Pow(v1[0, i] - v2[0, i], 2.0);                   
            }

            return Math.Sqrt(res);
        }

        #endregion

        #region amostras sem reposição

        public int[] amostra_sem_reposicao(int tamanho_amostra, int tamanho_populacao)
        {
            int[] sequencia = new int[tamanho_populacao];
            for (int i = 0; i < sequencia.GetLength(0); i++)
            {
                sequencia[i] = i;
            }

            return amostra_sem_reposicao(sequencia, tamanho_amostra);
        }

        public int[] amostra_sem_reposicao(int[] sequencia, int num_obs)
        {
            Random rnd = new Random();
            
            int[] res = new int[num_obs];

            object[,] aux1 = new object[sequencia.GetLength(0), 2];
            for (int i = 0; i < aux1.GetLength(0); i++)
            {
                aux1[i, 0] = rnd.NextDouble();
                aux1[i, 1] = sequencia[i];
            }

            this.SortByColumn(ref aux1, aux1, 0);

            for (int i = 0; i < num_obs; i++)
            {
                res[i] = Convert.ToInt32(aux1[i, 1]);
            }

            return res;
        }

        #endregion

        #region adição de novas variáveis a um datatable

        public void AdicionaColunasToDataTable(ref DataTable tabela, double[,] dados, string[] nomes_variaveis)
        {
            Type[] tipos_variaveis = new Type[dados.GetLength(1)];
            for (int i = 0; i < tipos_variaveis.GetLength(0); i++)
            {
                tipos_variaveis[i] = typeof(double);
            }

            object[,] dados_novos = new object[dados.GetLength(0), dados.GetLength(1)];
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                for (int j = 0; j < dados.GetLength(1); j++)
                {
                    dados_novos[i, j] = dados[i, j];
                }
            }

            AdicionaColunasToDataTable(ref tabela, dados_novos, nomes_variaveis, tipos_variaveis);
        }

        public void AdicionaColunasToDataTable(ref DataTable tabela, object[,] dados, string[] nomes_variaveis, Type[] tipos_variaveis)
        {
            if (nomes_variaveis.GetLength(0) != dados.GetLength(1)) throw new Exception("Número de variáveis é diferente do número de colunas");
            if (dados.GetLength(0) != tabela.Rows.Count) throw new Exception("Número de observações na tabela é diferente do número de linhas do datatable.");
            if (tipos_variaveis.GetLength(0) != dados.GetLength(1)) throw new Exception("Número de tipos de variáveis é diferente do número de colunas");

            string[] novos_nomes = new string[nomes_variaveis.GetLength(0)];
            for (int k = 0; k < nomes_variaveis.GetLength(0); k++)
            {
                if (!tabela.Columns.Contains(nomes_variaveis[k]))
                {
                    novos_nomes[k] = nomes_variaveis[k];
                }
                else
                {
                    novos_nomes[k] = IncrementaNomeVariavel(nomes_variaveis[k]);
                    while (tabela.Columns.Contains(novos_nomes[k]))
                    {
                        novos_nomes[k] = IncrementaNomeVariavel(nomes_variaveis[k]);
                    }
                }
            }

            for (int j = 0; j < dados.GetLength(1); j++)
            {
                tabela.Columns.Add(novos_nomes[j], tipos_variaveis[j]);
                for (int i = 0; i < dados.GetLength(0); i++)
                {
                    tabela.Rows[i][novos_nomes[j]] = dados[i, j];
                }
            }
        }

        private string IncrementaNomeVariavel(string nome)
        {
            int final = RetornaNumeroFinalString(nome);
            if (final < 0)
            {
                return nome + "_1";
            }

            string res = nome;
            if (final < 10)
            {
                res = nome.Remove(nome.Length - 1) + (final + 1).ToString();
                return res;
            }

            if (final < 100)
            {
                res = nome.Remove(nome.Length - 2) + (final + 1).ToString();
                return res;
            }

            if (final < 1000)
            {
                res = nome.Remove(nome.Length - 3) + (final + 1).ToString();
                return res;
            }

            return nome;
        }

        private int RetornaNumeroFinalString(string nome)
        {
            int res = -1;

            if (nome.Substring(nome.Length - 2, 1) == "_")
            {
                for (int i = 0; i < 10; i++)
                {
                    if (nome.Substring(nome.Length - 1, 1) == i.ToString())
                    {
                        return i;
                    }
                }
            }

            if (nome.Substring(nome.Length - 3, 1) == "_")
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (nome.Substring(nome.Length - 2, 2) == (i * 10 + j).ToString())
                        {
                            return i*10 + j;
                        }
                    }
                }
            }
            
            if (nome.Substring(nome.Length - 4, 1) == "_")
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        for (int k = 0; k < 10; k++)
                        {
                            if (nome.Substring(nome.Length - 3, 3) == (i * 100 + j*10 + k).ToString())
                            {
                                return (i * 100 + j * 10 + k);
                            }
                        }
                    }
                }
            }

            return res;
        }

        #endregion

        #region função de logaritmo

        public double[,] LogMatriz(double[,] m)
        {
            double[,] res = new double[m.GetLength(0), m.GetLength(1)];
            for (int i = 0; i < m.GetLength(0); i++)
                for (int j = 0; j < m.GetLength(1); j++)
                    res[i, j] = Math.Log(m[i, j]);
            return res;
        }

        #endregion

        #region dobra tamanho do vetor

        /// <summary>
        /// Função para dobrar o número de elementos de um vetor, mantendo os valores originais.
        /// </summary>
        /// <param name="a">Vetor de entrada.</param>
        /// <returns>Novo vetor, com o dobro de valores.</returns>
        public double[] DobraTamanhoVetor(double[] a)
        {
            double[] res = new double[2 * a.GetLength(0)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                res[i] = a[i];
            }
            return res;
        }

        /// <summary>
        /// Função para dobrar o número de elementos de um vetor, mantendo os valores originais.
        /// </summary>
        /// <param name="a">Vetor de entrada.</param>
        /// <returns>Novo vetor, com o dobro de valores.</returns>
        public int[] DobraTamanhoVetor(int[] a)
        {
            int[] res = new int[2*a.GetLength(0)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                res[i] = a[i];
            }
            return res;
        }

        #endregion

        #region subvetor

        /// <summary>
        /// Retorna elementos a partir de um vetor original.
        /// </summary>
        /// <param name="v">Vetor original.</param>
        /// <param name="i0">Índice do vetor, a partir do qual os elementos serão retornados.</param>
        /// <returns>Vetor de saída.</returns>
        public int[] SubVector(int[] v, int i0)
        {
            return SubVector(v, i0, v.GetLength(0) - 1);
        }

        /// <summary>
        /// Retorna elementos a partir de um vetor original.
        /// </summary>
        /// <param name="v">Vetor original.</param>
        /// <param name="i0">Índice do primeiro elemento a ser retornado.</param>
        /// <param name="i1">Índice do último elemento a ser retornado.</param>
        /// <returns></returns>
        public int[] SubVector(int[] v, int i0, int i1)
        {
            int[] res = new int[i1 - i0 + 1];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                res[i] = v[i + i0];
            }
            return res;
        }

        #endregion

        #region tratamento de observações missing ou nulas nos datatable

        public void DatatableToDoubleMatrixSemMissing(DataTable dt, string[] variaveis,
            out double[,] matriz_dados, out int num_obs_total, out int num_obs_missing, out int num_obs_validas,
            out int[] rows_obs_validas, out int[] indicador_obs_validas)
        {
            for (int i = 0; i < variaveis.GetLength(0); i++)
            {
                if (!IsNumeric(dt.Columns[variaveis[i]], typeof(double)))
                {
                    throw new Exception("Coluna " + variaveis[i] + " não corresponde a uma variável numérica.");
                }
            }                     

            num_obs_missing = 0;
            num_obs_total = dt.Rows.Count;
            num_obs_validas = 0;

            matriz_dados = new double[0, 0];
            rows_obs_validas = new int[0];

            indicador_obs_validas = new int[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                indicador_obs_validas[i] = 1;
                for (int j = 0; j < variaveis.GetLength(0); j++)
                {
                    if (dt.Rows[i][variaveis[j]] is DBNull 
                        || Double.IsInfinity(Convert.ToDouble(dt.Rows[i][variaveis[j]]))
                        || Double.IsNaN(Convert.ToDouble(dt.Rows[i][variaveis[j]]))
                        || Double.IsNegativeInfinity(Convert.ToDouble(dt.Rows[i][variaveis[j]]))
                        || Double.IsPositiveInfinity(Convert.ToDouble(dt.Rows[i][variaveis[j]])))
                    {
                        indicador_obs_validas[i] = 0;
                        break;
                    }
                }
                if (indicador_obs_validas[i] == 1)
                {
                    num_obs_validas++;
                }
            }
            num_obs_missing = num_obs_total - num_obs_validas;

            matriz_dados = new double[num_obs_validas, variaveis.GetLength(0)];
            rows_obs_validas = new int[num_obs_validas];
            int row = 0;
            for (int i = 0; i < num_obs_total; i++)
            {
                if (indicador_obs_validas[i] == 1)
                {
                    rows_obs_validas[row] = i;
                    for (int j = 0; j < variaveis.GetLength(0); j++)
                    {
                        matriz_dados[row, j] = Convert.ToDouble(dt.Rows[i][variaveis[j]]);
                    }
                    row++;
                }
            }
        }

        #endregion

        #region tratamento de observações missing ou nulas em um double
        public void DoubleToDoubleSemMissing(double[,] dt, out double[,] matriz_dados)
        {

            int n = dt.GetLength(0);
            double[] validas = new double[n];

            int n_valido = 0;
            for (int i = 0; i < n; i++)
            {
                if (double.IsNaN(dt[i, 0]))
                {
                    validas[i] = 0;
                }
                else
                {
                    validas[i] = 1;
                    n_valido += 1;
                }
            }

            matriz_dados = new double[n_valido,1];


            int contador = 0;
            for (int i = 0; i < n; i++)
            {

                if (validas[i] == 1)
                {
                    matriz_dados[contador,0] = dt[i,0];
                    contador += 1;
                }
            }

        }

        #endregion

        #region funções envolvendo categorias

        /// <summary>
        /// Checa se uma determinada string está em um vetor de strings.
        /// </summary>
        /// <param name="s">String a ser procurada.</param>
        /// <param name="v">Vetor de strings.</param>
        /// <returns>Retorna true se string s está no vetor v, e false caso contrário.</returns>
        public bool ChecaStringEmLista(string s, string[] v)
        {
            for (int i = 0; i < v.GetLength(0); i++)
            {
                if (s == v[i]) return true;
            }
            return false;
        }


        /// <summary>
        /// Retorna true se o número de categorias está dentro do limite máximo, ou false caso contrário.
        /// </summary>
        public bool ChecaLimiteCategorias(int num_max_categorias, object[,] dados)
        {
            ArrayList lista = new ArrayList();
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (!lista.Contains(dados[i, 0]))
                {
                    lista.Add(dados[i, 0]);
                    if (lista.Count > num_max_categorias) return false;
                }
            }
            return true;
        }

        //TODO: Novo overload da funcao checaLimiteCategorias (Caue)
        public bool ChecaLimiteCategorias(int num_max_categorias, object[,] dados, out object[] categorias)
        {
            ArrayList lista = new ArrayList();
            categorias = new object[num_max_categorias];

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (!lista.Contains(dados[i, 0]))
                {
                    lista.Add(dados[i, 0]);
                    
                    if (lista.Count > num_max_categorias) return false;
                    else categorias[lista.Count - 1] = dados[i, 0];
                }
            }
            return true;
        }

        #endregion

        #region checa para a presença de valores double inválidos

        /// <summary>
        /// Identificar linhas com valores double inválidos.
        /// </summary>
        /// <param name="v">Matriz de double[,].</param>
        /// <param name="indicadores_val_invalidos">Vetor int[], com 0 se a linha estiver válida ou 1 caso a linha de v contenha valores inválidos.</param>
        /// <returns>Retorna true se matriz v contém valores inválidos.</returns>
        private bool IndicadoresRowsMissingDouble(double[,] v, out int[] indicadores_val_invalidos)
        {
            bool res = false;
            indicadores_val_invalidos = new int[v.GetLength(0)];
            for (int i = 0; i < v.GetLength(0); i++)
            {
                for (int j = 0; j < v.GetLength(1); j++)
                {
                    if (double.IsInfinity(v[i, j]) || double.IsNaN(v[i, j]) || double.IsNegativeInfinity(v[i, j]) || double.IsPositiveInfinity(v[i, j]))
                    {
                        indicadores_val_invalidos[i] = 1;
                        res = true; 
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Identificar linhas com valores double inválidos.
        /// </summary>
        /// <param name="dt">Datatable com os dados.</param>
        /// <param name="variavel">Variável no datatable a ser checada.</param>
        /// <param name="indicadores_val_invalidos">Vetor int[], com 0 se a linha estiver válida ou 1 caso a linha de v contenha valores inválidos.</param>
        /// <returns>Retorna true se matriz v contém valores inválidos.</returns>
        public bool ChecaValoresDoubleInvalidos(DataTable dt, string variavel, out int[] indicadores_val_invalidos)
        {
            double[,] v = this.GetMatrizFromDataTable(dt, variavel);
            return IndicadoresRowsMissingDouble(v, out indicadores_val_invalidos);
        }

        #endregion

        #region funções de importação e exportaçaõ de arquivos

        /// <summary>
        /// Exporta uma variável texto para um arquivo texto.
        /// </summary>
        /// <param name="conteudo">Variável texto a ser salva.</param>
        /// <param name="fileName">Nome do arquivo.</param>
        /// <returns>Retorna true se a exportação foi bem sucedida.</returns>
        public bool ExportarArquivoTexto(string conteudo, string fileName)
        {
            try
            {
                StreamWriter stream = new StreamWriter(fileName);
                stream.Write(conteudo);
                stream.Close();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retorna uma string com as informações de um determinado arquivo.
        /// </summary>
        /// <param name="fileName">Nome do arquivo.</param>
        /// <returns>Retorna data de criação, data de modificação e data de último acesso.</returns>
        private string InformacoesArquivo(string fileName)
        {
            string information = fileName + " exists\r\n\r\n";

            information += "Created: " +
                File.GetCreationTime(fileName) + "\r\n";

            information += "Last modified: " +
                File.GetLastWriteTime(fileName) + "\r\n";

            information += "Last accessed: " +
                File.GetLastAccessTime(fileName) + "\r\n";

            return information;
        }

        /// <summary>
        /// Lê um arquivo texto e adiciona as informações para uma variável string.
        /// </summary>
        /// <param name="conteudo">Retorno o conteúdo do arquivo texto.</param>
        /// <param name="fileInformacoes">Retorna as informações do arquivo.</param>
        /// <param name="fileName">Nome do arquivo.</param>
        /// <returns>Retorno true se importação foi bem sucedida.</returns>
        public bool ImportarArquivoTexto(ref string conteudo, ref string fileInformacoes, string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    fileInformacoes = this.InformacoesArquivo(fileName);

                    try
                    {
                        StreamReader stream = new StreamReader(fileName);
                        conteudo = stream.ReadToEnd();
                        stream.Close();
                    }

                    catch (IOException)
                    {
                        MessageBox.Show("File Error", "File Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        public DataTable DataTableNumberToTexto2(DataTable dt)
        {
            DataTable res = new DataTable();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                res.Columns.Add(dt.Columns[i].ColumnName, Type.GetType("System.String"));
                res.Columns[i].Caption = dt.Columns[i].Caption;
            }

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataRow linha = res.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    linha[i] = dt.Rows[j][i].ToString();

                }
                res.Rows.Add(linha);
            }
            return res;
        }

        #region deletando strings

        /// <summary>
        /// Deleta uma string de um vetor de strings.
        /// </summary>
        /// <param name="a">Vetor de strings.</param>
        /// <param name="indice">Índice do elemento a ser deletado.</param>
        /// <returns>Retorna um novo vetor, com um elemento a menos.</returns>
        public string[] DeleteStringFromArray(string[] a, int indice)
        {
            string[] res = new string[a.GetLength(0) - 1];
            if (indice < 0 || indice > a.GetLength(0) - 1) return a;
            if (a.GetLength(0)<=1) return new string[0];
            if (indice == 0)
            {
                res = new string[a.GetLength(0) - 1];
                for (int i = 0; i < res.GetLength(0); i++)
                    res[i] = a[i + 1];
                return res;
            }
            if (indice == a.GetLength(0) - 1)
            {
                res = new string[a.GetLength(0) - 1];
                for (int i = 0; i < res.GetLength(0); i++)
                    res[i] = a[i];
                return res;
            }
            for (int i = 0; i < indice; i++)
            {
                res[i] = a[i];
            }
            for (int i = indice + 1; i < a.GetLength(0); i++)
            {
                res[i - 1] = a[i];
            }
            return res;
        }

        /// <summary>
        /// Deleta uma linha de uma matriz.
        /// </summary>
        /// <param name="a">Matriz original.</param>
        /// <param name="col">Índice da linha a ser excluída.</param>
        /// <returns>Retorna uma matriz com uma linha a menos.</returns>
        public double[,] DeleteRow(double[,] a, int row)
        {
            if (row < 0 || row > a.GetLength(0) - 1) return this.ArrayDoubleClone(a);
            if (a.GetLength(0) == 1) return new double[0, 0];
            if (row == 0)
            {
                if (a.GetLength(0) > 1)
                {
                    double[,] res = new double[a.GetLength(0)-1, a.GetLength(1)];
                    for (int i = 0; i < res.GetLength(0); i++)
                        for (int j = 0; j < res.GetLength(1); j++)
                            res[i, j] = a[i+1, j];
                    return res;
                }
                return new double[0, 0];
            }
            if (row == a.GetLength(0) - 1)
            {
                if (a.GetLength(0) > 1)
                {
                    double[,] res = new double[a.GetLength(0)-1, a.GetLength(1)];
                    for (int i = 0; i < res.GetLength(0); i++)
                        for (int j = 0; j < res.GetLength(1); j++)
                            res[i, j] = a[i, j];
                    return res;
                }
                return new double[0, 0];
            }
            if (row > 0 && row < a.GetLength(0) - 1)
            {
                double[,] res = new double[a.GetLength(0)-1, a.GetLength(1)];
                for (int i = 0; i < res.GetLength(1); i++)
                {
                    for (int j = 0; j < row; j++)
                    {
                        res[j,i] = a[j,i];
                    }
                    for (int j = row + 1; j < a.GetLength(0); j++)
                    {
                        res[j - 1, i] = a[j, i];
                    }
                }
                return res;
            }
            return this.ArrayDoubleClone(a);
        }

        /// <summary>
        /// Deleta uma coluna de uma matriz.
        /// </summary>
        /// <param name="a">Matriz original.</param>
        /// <param name="col">Índice da coluna a ser excluída.</param>
        /// <returns>Retorna uma matriz com uma coluna a menos.</returns>
        public object[,] DeleteCol(object[,] a, int col)
        {
            if (col < 0 || col > a.GetLength(1) - 1) return this.ArrayDoubleClone(a);
            if (a.GetLength(1) == 1) return new object[0, 0];
            if (col == 0)
            {
                if (a.GetLength(1) > 1)
                {
                    object[,] res = new object[a.GetLength(0), a.GetLength(1) - 1];
                    for (int i = 0; i < res.GetLength(0); i++)
                        for (int j = 0; j < res.GetLength(1); j++)
                            res[i, j] = a[i, j + 1];
                    return res;
                }
                return new object[0, 0];
            }
            if (col == a.GetLength(1) - 1)
            {
                if (a.GetLength(1) > 1)
                {
                    object[,] res = new object[a.GetLength(0), a.GetLength(1) - 1];
                    for (int i = 0; i < res.GetLength(0); i++)
                        for (int j = 0; j < res.GetLength(1); j++)
                            res[i, j] = a[i, j];
                    return res;
                }
                return new object[0, 0];
            }
            if (col > 0 && col < a.GetLength(1) - 1)
            {
                object[,] res = new object[a.GetLength(0), a.GetLength(1) - 1];
                for (int i = 0; i < res.GetLength(0); i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        res[i, j] = a[i, j];
                    }
                    for (int j = col + 1; j < a.GetLength(1); j++)
                    {
                        res[i, j - 1] = a[i, j];
                    }
                }
                return res;
            }
            return this.ArrayDoubleClone(a);
        }

        /// <summary>
        /// Deleta uma coluna de uma matriz.
        /// </summary>
        /// <param name="a">Matriz original.</param>
        /// <param name="col">Índice da coluna a ser excluída.</param>
        /// <returns>Retorna uma matriz com uma coluna a menos.</returns>
        public double[,] DeleteCol(double[,] a, int col)
        {
            if (col < 0 || col > a.GetLength(1) - 1) return this.ArrayDoubleClone(a);
            if (a.GetLength(1) == 1) return new double[0, 0];
            if (col == 0)
            {
                if (a.GetLength(1) > 1)
                {
                    double[,] res = new double[a.GetLength(0), a.GetLength(1) - 1];
                    for (int i = 0; i < res.GetLength(0); i++)
                        for (int j = 0; j < res.GetLength(1); j++)
                            res[i, j] = a[i, j + 1];
                    return res;
                }
                return new double[0, 0];
            }
            if (col == a.GetLength(1)-1)
            {
                if (a.GetLength(1) > 1)
                {
                    double[,] res = new double[a.GetLength(0), a.GetLength(1) - 1];
                    for (int i = 0; i < res.GetLength(0); i++)
                        for (int j = 0; j < res.GetLength(1); j++)
                            res[i, j] = a[i, j];
                    return res;
                }
                return new double[0, 0];
            }
            if (col > 0 && col < a.GetLength(1) - 1)
            {
                double[,] res = new double[a.GetLength(0), a.GetLength(1) - 1];
                for (int i = 0; i < res.GetLength(0); i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        res[i, j] = a[i, j];
                    }
                    for (int j = col + 1; j < a.GetLength(1); j++)
                    {
                        res[i, j - 1] = a[i, j];
                    }
                }
                return res;
            }
            return this.ArrayDoubleClone(a);
        }

        #endregion

        #region funções estatísticas de matrizes

        /// <summary>
        /// Retorna o traço de uma matriz.
        /// </summary>
        /// <param name="a">Matriz de entrada.</param>
        /// <returns>Traço da matriz.</returns>
        public double Trace(double[,] a)
        {
            if (a.GetLength(0) != a.GetLength(1)) return double.NaN;
            double r = 0.0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                r += a[i, i];
            }
            return r;
        }

        /// <summary>
        /// Retorna o percentil de uma massa de dados.
        /// </summary>
        /// <param name="dados">Matriz com as observações.</param>
        /// <param name="percentil">Percentil (entre 0% e 100%).</param>
        /// <returns>Percentil.</returns>
        public double Percentil(double[] dados, double percentil)
        {
            double[,] aux = new double[dados.GetLength(0), 1];
            for (int i = 0; i < aux.GetLength(0); i++)
            {
                aux[i, 0] = dados[i];
            }
            return Percentil(aux, percentil);
        }

        /// <summary>
        /// Retorna o percentil de uma massa de dados.
        /// </summary>
        /// <param name="dados">Matriz com as observações.</param>
        /// <param name="percentil">Percentil (entre 0% e 100%).</param>
        /// <returns>Percentil.</returns>
        public double Percentil(double[,] dados, double percentil)
        {
            if (dados.GetLength(0)*dados.GetLength(1) > 1)
            {
                ArrayList a = new ArrayList();
                for (int i = 0; i < dados.GetLength(0); i++)
                {
                    for (int j = 0; j < dados.GetLength(1); j++)
                    {
                        a.Add(dados[i, j]);
                    }
                }
                a.Sort();

                if (percentil >= 100.0) return Convert.ToDouble(a[a.Count - 1]);
                if (percentil <= 0.0) return Convert.ToDouble(a[0]);

                int i1 = Convert.ToInt32(Math.Floor(percentil * (double)a.Count / 100.0 - 0.5));
                int i2 = Convert.ToInt32(Math.Ceiling(percentil * (double)a.Count / 100.0 - 0.5));

                i1 = Convert.ToInt32(Math.Max(0.0, Math.Min((double)i1, (double)a.Count - 1.0)));
                i2 = Convert.ToInt32(Math.Max(0.0, Math.Min((double)i2, (double)a.Count - 1.0)));

                double c1 = Convert.ToDouble(a[i1]);
                double c2 = Convert.ToDouble(a[i2]);

                return c1 + (c2 - c1) * (percentil - 100.0 * ((double)i1 + 0.5) / (double)a.Count) / (100.0 / (double)a.Count);
            }
            else
            {
                return dados[0,0];
            }
        }

        /// <summary>
        /// Retorna uma matriz com as colunas da matriz originais padronizadas.
        /// </summary>
        /// <param name="m">Matriz original.</param>
        /// <returns>Matriz com colunas padronizadas.</returns>
        public double[,] Standardizec(double[,] m)
        {
            double[,] mediac = this.Meanc(m);
            double[,] desvc = this.Despadc(m);
            double[,] r = new double[m.GetLength(0), m.GetLength(1)];
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    r[i, j] = (m[i, j] - mediac[0, j]) / desvc[0, j];
                }
            }
            return r;
        }

        /// <summary>
        /// Retorna uma matriz identidade.
        /// </summary>
        /// <param name="m">Dimensão da matriz identidade.</param>
        /// <returns>Matriz criada.</returns>
        public double[,] Identity(int m)
        {
            double[,] r = new double[m, m];
            for (int i = 0; i < m; i++)
            {
                r[i, i] = 1.0;
            }
            return r;
        }

        #endregion

        #region procura chave primária nas colunas de um datatable

        /// <summary>
        /// Retorna uma combinação (de no máximo quatro colunas) que forma uma chave primária para as observações no datatable. 
        /// </summary>
        /// <param name="dt">Datatable de entrada.</param>
        /// <returns>ArrayList contendo os nomes das colunas que formam a chave primária.</returns>
        public ArrayList RetornaIndicesColunasChavesPrimarias(DataTable dt)
        {
            ArrayList res = new ArrayList();
            ArrayList ind_cols = new ArrayList();

            if (dt.Columns.Count > 0)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ind_cols = new ArrayList();
                    ind_cols.Add(i);
                    if (IsColunasChaveUnica(dt, ind_cols))
                    {
                        return ind_cols;
                    }
                }
            }

            if (dt.Columns.Count > 1)
            {
                for (int i = 0; i < dt.Columns.Count - 1; i++)
                {
                    for (int j = i + 1; j < dt.Columns.Count; j++)
                    {
                        ind_cols = new ArrayList();
                        ind_cols.Add(i);
                        ind_cols.Add(j);
                        if (IsColunasChaveUnica(dt, ind_cols))
                        {
                            return ind_cols;
                        }
                    }
                }
            }

            if (dt.Columns.Count > 2)
            {
                for (int i = 0; i < dt.Columns.Count - 2; i++)
                {
                    for (int j = i + 1; j < dt.Columns.Count - 1; j++)
                    {
                        for (int k = j + 1; k < dt.Columns.Count; k++)
                        {
                            ind_cols = new ArrayList();
                            ind_cols.Add(i);
                            ind_cols.Add(j);
                            ind_cols.Add(k);
                            if (IsColunasChaveUnica(dt, ind_cols))
                            {
                                return ind_cols;
                            }
                        }
                    }
                }
            }

            if (dt.Columns.Count > 3)
            {
                for (int i = 0; i < dt.Columns.Count - 3; i++)
                {
                    for (int j = i + 1; j < dt.Columns.Count - 2; j++)
                    {
                        for (int k = j + 1; k < dt.Columns.Count - 1; k++)
                        {
                            for (int l = k + 1; k < dt.Columns.Count; l++)
                            {
                                ind_cols = new ArrayList();
                                ind_cols.Add(i);
                                ind_cols.Add(j);
                                ind_cols.Add(k);
                                ind_cols.Add(l);
                                if (IsColunasChaveUnica(dt, ind_cols))
                                {
                                    return ind_cols;
                                }
                            }
                        }
                    }
                }
            }

            return res;
        }

        public bool IsColunasChaveUnica(DataTable dt, ArrayList ind_cols)
        {
            ArrayList lista = new ArrayList();
            StringBuilder v = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                v.Clear();
                for (int i = 0; i < ind_cols.Count; i++)
                {
                    v.Append(dr[(int)ind_cols[i]].ToString());
                }
                if (lista.Contains(v.ToString())) return false;
                else lista.Add(v.ToString());
            }
            return true;
        }

        #endregion

        /// <summary>
        /// Gera um datatable a partir de uma matriz de strings.
        /// </summary>
        /// <param name="m">Matriz de strings.</param>
        /// <param name="vs">Vetor com os nomes das colunas.</param>
        /// <returns>Retorna o datatable criado.</returns>
        public DataTable DataTableFromStringMatriz(string[,] m, string[] vs)
        {
            DataTable dt = new DataTable();
            DataColumn dc;
            for (int k = 0; k < vs.GetLength(0); k++)
            {
                dc = new DataColumn(vs[k], typeof(string));
                dt.Columns.Add(dc);
            }

            object[] valores = new object[vs.GetLength(0)];
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int k = 0; k < vs.GetLength(0); k++)
                {
                    valores[k] = m[i, k];
                }
                dt.Rows.Add(valores);
            }
            return dt;
        }

        /// <summary>
        /// Gera um datatable a partir de uma matriz de doubles.
        /// </summary>
        /// <param name="m">Matriz de doubles.</param>
        /// <param name="vs">Vetor com os nomes das colunas.</param>
        /// <returns>Retorna o datatable criado.</returns>
        public DataTable DataTableFromMatriz(double[,] m, string[] vs)
        {
            DataTable dt = new DataTable();
            DataColumn dc;
            for (int k = 0; k < vs.GetLength(0); k++)
            {
                dc = new DataColumn(vs[k], typeof(double));
                dt.Columns.Add(dc);
            }

            object[] valores = new object[vs.GetLength(0)];
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int k = 0; k < vs.GetLength(0); k++)
                {
                    valores[k] = m[i,k];
                }
                dt.Rows.Add(valores);
            }
            return dt;
        }

        /// <summary>
        /// Retorna a posicao de uma string em uma array de strings. Caso a array nao esteja na lista, a funcao retorna -1.
        /// </summary>
        /// <param name="vs">Lista de strings.</param>
        /// <param name="s">String testada.</param>
        /// <returns>Retorna -1 ou a posicao da string na lista.</returns>
        public int PosicaoNaLista(string[] vs, string s)
        {
            for (int i = 0; i < vs.GetLength(0); i++)
            {
                if (vs[i] == s) return i;
            }

            return -1;
        }

        /// <summary>
        /// Função para eliminar string duplicadas em uma array de strings.
        /// </summary>
        /// <param name="vs">Array de strings como imput.</param>
        /// <returns>Retorna array de strings sem duplicação.</returns>
        public string[] EliminaStringsDuplicadas(string[] vs)
        {
            ArrayList va = new ArrayList();
            for (int i = 0; i < vs.GetLength(0); i++)
            {
                if (!va.Contains(vs[i]))
                {
                    va.Add(vs[i]);
                }
            }
            string[] res = new string[va.Count];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                res[i] = va[i].ToString();
            }
            return res;
        }

        #region mudando o separador de decimais de valores numéricos em um datatable

        public DataTable MudaSeparadorDecimais(DataTable dt)
        {
            System.Globalization.NumberFormatInfo m_format = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            System.Globalization.NumberFormatInfo nfi = (System.Globalization.NumberFormatInfo)m_format.Clone();

            nfi.NumberDecimalDigits = 30;

            clsUtilTools clt = new clsUtilTools();

            ArrayList vars_numericas = new ArrayList();
            DataTable dt_novo = new DataTable();

            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (dt.Columns[j].DataType == typeof(int)
                    || dt.Columns[j].DataType == typeof(Int16)
                    || dt.Columns[j].DataType == typeof(Int32)
                    || dt.Columns[j].DataType == typeof(Int64)
                    || dt.Columns[j].DataType == typeof(decimal)
                    || dt.Columns[j].DataType == typeof(Decimal)
                    || dt.Columns[j].DataType == typeof(double)
                    || dt.Columns[j].DataType == typeof(Double)
                    || dt.Columns[j].DataType == typeof(float))
                {
                    vars_numericas.Add(j);
                    dt_novo.Columns.Add(dt.Columns[j].ColumnName, typeof(string));
                }
                else
                {
                    dt_novo.Columns.Add(dt.Columns[j].ColumnName, dt.Columns[j].DataType);
                }
            }

            object[] nova_linha = new object[dt.Columns.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nova_linha = dt.Rows[i].ItemArray;
                for (int j = 0; j < vars_numericas.Count; j++)
                {
                    nova_linha[Convert.ToInt32(vars_numericas[j])] = (Convert.ToDouble(nova_linha[Convert.ToInt32(vars_numericas[j])])).ToString("N", nfi);
                }
                dt_novo.Rows.Add(nova_linha);
            }

            return dt_novo;
        }

        #endregion

        #region funções de números para texto

        public double DoubleFromTexto(object m_texto)
        {
            try
            {
                System.Globalization.NumberFormatInfo m_format = new System.Globalization.CultureInfo("en-US", false).NumberFormat;

                return Double.Parse(m_texto.ToString(), m_format);
            }
            catch
            {
                return double.NaN;
            }
        }

        public string Double2Texto(object m_double)
        {
            try
            {
                if (m_double.GetType() == typeof(double)
                    || m_double.GetType() == typeof(Double))
                {
                    //System.Globalization.NumberFormatInfo m_format = new System.Globalization.CultureInfo("pt-BR", false).NumberFormat;
                    System.Globalization.NumberFormatInfo m_format = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
                    //System.Globalization.NumberFormatInfo m_format = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;

                    double m_temp_double = Convert.ToDouble(m_double);
                    return m_temp_double.ToString(m_format);
                    //return m_temp_double.ToString("N", m_format);
                }

                return m_double.ToString();
            }
            catch
            {
                return "";
            }
        }

        public string Double2Texto(object m_double, int m_decimals)
        {
            try
            {
                if (m_double.GetType() == typeof(double)
                    || m_double.GetType() == typeof(Double))
                {
                    //System.Globalization.NumberFormatInfo m_format = new System.Globalization.CultureInfo("pt-BR", false).NumberFormat;
                    System.Globalization.NumberFormatInfo m_format = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
                    //System.Globalization.NumberFormatInfo m_format = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;

                    m_format.NumberDecimalDigits = m_decimals;
                    double m_temp_double = Convert.ToDouble(m_double);
                    if (double.IsInfinity(m_temp_double)
                        || double.IsNaN(m_temp_double)
                        || double.IsNegativeInfinity(m_temp_double)
                        || double.IsPositiveInfinity(m_temp_double))
                    {
                        return ".";
                    }
                    return m_temp_double.ToString("N", m_format);
                }

                if (m_double.GetType() == typeof(int)
                    || m_double.GetType() == typeof(Int32)
                    || m_double.GetType() == typeof(Int16)
                    || m_double.GetType() == typeof(Int64))
                {
                    return Convert.ToInt32(m_double).ToString();
                }

                return m_double.ToString();
            }
            catch
            {
                return "";
            }
        }

        #endregion

        /// <summary>
        /// Gera uma matriz de object a partir de um datatable.
        /// </summary>
        /// <param name="dt">Datatable de entrada.</param>
        /// <param name="variaveis">Nome das colunas a serem passadas para a matriz de object.</param>
        /// <returns>Matriz de object gerada.</returns>
        public object[,] GetObjMatrizFromDataTable(DataTable dt, string variavel)
        {
            string[] variavel_array = new string[1];
            variavel_array[0] = variavel;

            return GetObjMatrizFromDataTable(dt, variavel_array);
        }

        /// <summary>
        /// Padroniza as colunas de uma matriz (X-mu)/sigma
        /// </summary>
        /// <param name="dados"></param>
        /// <returns></returns>
        public double[,] PadronizaDados(double[,] dados)
        {
            double[,] media = Meanc(dados);
            double[,] var = Varianciac(dados);
            for (int iColuna = 0; iColuna < media.GetLength(1); iColuna++)
            {
                for (int iLinha = 0; iLinha < media.GetLength(1); iLinha++)
                {
                    dados[iLinha, iColuna] = (dados[iLinha, iColuna] - media[iLinha, iColuna]) / Math.Sqrt(var[iLinha, iColuna]);
                }
            }
            return (dados);
        }

        #region geracao de um nome de coluna que ainda não existe no datatable ou na lista de variáveis

        /// <summary>
        /// Retorna um string com um nome para uma coluna, sem que haja outra coluna com o mesmo nome no datatable. O novo nome é criado adicionando-se inteiros ao final do nome sugerido, quando necessário.
        /// </summary>
        /// <param name="dt">Datatable de entrada - serão checados os nomes de colunas nesse datatable.</param>
        /// <param name="nome_sugerido">Nome sugerido.</param>
        /// <returns>Retorno o nome alterado, quando já existir no datatable.</returns>
        public string RetornaNovoNomeSemRepeticao(string[] variaveis, string nome_sugerido)
        {
            ArrayList lista = new ArrayList();
            for (int i = 0; i < variaveis.GetLength(0); i++) lista.Add(variaveis[i]);

            return RetornaNovoNomeSemRepeticao(lista, nome_sugerido);
        }

        /// <summary>
        /// Retorna um string com um nome para uma coluna, sem que haja outra coluna com o mesmo nome no datatable. O novo nome é criado adicionando-se inteiros ao final do nome sugerido, quando necessário.
        /// </summary>
        /// <param name="dt">Datatable de entrada - serão checados os nomes de colunas nesse datatable.</param>
        /// <param name="nome_sugerido">Nome sugerido.</param>
        /// <returns>Retorno o nome alterado, quando já existir no datatable.</returns>
        public string RetornaNovoNomeSemRepeticao(ArrayList lista_vars, string nome_sugerido)
        {
            if (!lista_vars.Contains(nome_sugerido)) return nome_sugerido;

            string res = nome_sugerido;
            int contador = 1000;
            for (int i = 1000; i >= 0; i--)
            {
                if (lista_vars.Contains(res + "_" + i.ToString()))
                {
                    contador = i + 1;
                    return res + "_" + contador.ToString();
                }
            }

            return res + "_1";
        }

        #endregion

        /// <summary>
        /// Gera uma matriz de doubles a partir de um datatable.
        /// </summary>
        /// <param name="dt">Datatable de entrada.</param>
        /// <param name="variavel">Nome da coluna a ser passada para a matriz de doubles.</param>
        /// <returns>Matriz de doubles gerada.</returns>
        public double[,] GetMatrizFromDataTable(DataTable dt, string variavel)
        {
            string[] variavel_array = new string[1];
            variavel_array[0] = variavel;

            return GetMatrizFromDataTable(dt, variavel_array);
        }
        
        /// <summary>
        /// Gera uma matriz de object a partir de um datatable.
        /// </summary>
        /// <param name="dt">Datatable de entrada.</param>
        /// <param name="variaveis">Nome das colunas a serem passadas para a matriz de object.</param>
        /// <returns>Matriz de object gerada.</returns>
        public object[,] GetObjMatrizFromDataTable(DataTable dt, string[] variaveis)
        {
            object[,] m = new object[dt.Rows.Count, variaveis.GetLength(0)];
            for (int i = 0; i < variaveis.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(0); j++)
                {
                    m[j, i] = dt.Rows[j][variaveis[i]];
                }
            }
            return m;
        }

        /// <summary>
        /// Gera uma matriz de doubles a partir de um datatable.
        /// </summary>
        /// <param name="dt">Datatable de entrada.</param>
        /// <param name="variaveis">Nome das colunas a serem passadas para a matriz de doubles.</param>
        /// <returns>Matriz de doubles gerada.</returns>
        public double[,] GetMatrizFromDataTable(DataTable dt, string[] variaveis)
        {
            int num_colunas_numericas = 0;
            for (int i = 0; i < variaveis.GetLength(0); i++)
            {
                DataColumn dc = dt.Columns[variaveis[i]];
                if (IsNumeric(dc, typeof(double)))
                {
                    num_colunas_numericas++;
                }
            }

            if (num_colunas_numericas <= 0)
            {
                return new double[0, 0];
            }

            double[,] m = new double[dt.Rows.Count, num_colunas_numericas];
            int indice = 0;
            for (int i = 0; i < variaveis.GetLength(0); i++)
            {
                DataColumn dc = dt.Columns[variaveis[i]];
                if (IsNumeric(dc, typeof(double)))
                {
                    for (int j = 0; j < m.GetLength(0); j++)
                    {
                        if (dt.Rows[j][dc] is DBNull)
                        {
                            m[j, indice] = double.NaN;
                        }
                        else
                        {
                            m[j, indice] = Convert.ToDouble(dt.Rows[j][dc]);
                        }
                    }

                    indice++;
                }
            }

            return m;
        }

        /// <summary>
        /// Lista todas as colunas em um datatable.
        /// </summary>
        /// <param name="dt">Datatable de entrada.</param>
        /// <returns>Vetor de strings com os nomes das colunas.</returns>
        public string[] RetornaTodasColunas(DataTable dt)
        {
            string[] m_cols = new string[dt.Columns.Count];
            for (int i=0; i<dt.Columns.Count; i++) m_cols[i] = dt.Columns[i].ColumnName;

            return m_cols;
        }

        /// <summary>
        /// Retorna a lista de colunas cujos valores são únicos e não missing. As colunas escolhidas podem ser numéricas ou não. 
        /// </summary>
        /// <param name="dt">Tabela de dados.</param>
        /// <returns>Variáveis únidas</returns>
        public string[] RetornaUniqueColunas(DataTable dt)
        {
            ArrayList lista = new ArrayList();
            ArrayList lista_dup = new ArrayList();

            bool variavel_unica = true;
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                variavel_unica = true;
                lista_dup.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][k] is DBNull || lista_dup.Contains(dt.Rows[i][k]))
                    {
                        variavel_unica = false;
                        break;
                    }
                    else
                    {
                        lista_dup.Add(dt.Rows[i][k]);
                    }
                }

                if (variavel_unica)
                {
                    lista.Add(dt.Columns[k].ColumnName);
                }
            }

            string[] res = new string[lista.Count];
            for (int i = 0; i < res.GetLength(0); i++) res[i] = lista[i].ToString();

            return res;
        }

        /// <summary>
        /// Lista todas as colunas do tipo double em um datatable.
        /// </summary>
        /// <param name="dt">Datatable de entrada.</param>
        /// <returns>Vetor de strings com os nomes das colunas do tipo double.</returns>
        public string[] RetornaColunasDouble(DataTable dt)
        {
            string[] m_colunas = new string[dt.Columns.Count];

            int i = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                if (IsNumeric(dc, typeof(double)))
                {
                    m_colunas[i] = dc.ToString();
                    i++;
                }
            }
            string[] m_colunasNum = new string[i];
            for (int k = 0; k < i; k++)
                m_colunasNum[k] = m_colunas[k];

            return m_colunasNum;
        }

        /// <summary>
        /// Lista todas as colunas numéricas em um datatable.
        /// </summary>
        /// <param name="dt">Datatable de entrada.</param>
        /// <returns>Vetor de strings com os nomes das colunas numéricas.</returns>
        public string[] RetornaColunasNumericas(DataTable dt)
        {
            string[] m_colunas = new string[dt.Columns.Count];

            int i = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                if (IsNumeric(dc, typeof(double)))
                {
                    m_colunas[i] = dc.ToString();
                    i++;
                }
            }
            string[] m_colunasNum = new string[i];
            for (int k = 0; k < i; k++)
                m_colunasNum[k] = m_colunas[k];

            return m_colunasNum;
        }

        /// <summary>
        /// Checa se um determinado Datacolumn é numérico.
        /// </summary>
        /// <param name="dc">Datacolumn de entrada.</param>
        /// <param name="tp">Tipo da variável.</param>
        /// <returns>Retorna true se Datacolumn corresponde a uma variável numérica.</returns>
        private bool IsNumeric(DataColumn dc, Type tp)
        {
            switch (dc.DataType.ToString())
            {
                case "System.Float":
                    return true;
                case "System.Decimal":
                    return true;
                case "System.Double":
                    return true;
                case "System.Int":
                    return true;
                case "System.Int16":
                    return true;
                case "System.Int32":
                    return true;
                case "System.Int64":
                    return true;
                case "System.Single":
                    return true;
                //case "System.DateTime":
                //    return true;
                default:
                    return false;
            }
        }
        
        /// <summary>Média das colunas de uma matriz.</summary>
        public double[] VetorMedias(double[,] a)
        {
            double[] r = new double[a.GetLength(1)];
            for (int j = 0; j < a.GetLength(1); j++)
            {
                r[j] = 0.0;
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    r[j] += a[i, j];
                }
                r[j] = r[j] / Convert.ToDouble(a.GetLength(0));
            }
            return r;
        }

        /// <summary>Média das linhas de uma matriz.</summary>
        public double[,] Meanl(double[,] a)
        {
            double[,] r = new double[a.GetLength(0), 1];
            for (int j = 0; j < a.GetLength(0); j++)
            {
                r[j, 0] = 0.0;
                for (int i = 0; i < a.GetLength(1); i++)
                {
                    r[j, 0] += a[j, i];
                }
                r[j, 0] = r[j, 0] / Convert.ToDouble(a.GetLength(1));
            }
            return r;
        }

        /// <summary>Normaliza as colunas de uma matriz (Soma da coluna igual a 1)</summary>
        public double[,] Normalizac(double[,] a)
        {
            double[,] r = new double[1, a.GetLength(0)];
            for (int j = 0; j < a.GetLength(1); j++)
            {
                r[0, j] = 0.0;
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    r[0, j] += a[i, j];
                }
            }
            for (int j = 0; j < a.GetLength(1); j++)
            {
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    a[i, j] = a[i, j] / r[0, j];
                }
            }
            return a;
        }

        /// <summary>Média geométrica das linhas de uma matriz.</summary>
        public double[,] GeometricMeanl(double[,] a)
        {
            double[,] r = new double[a.GetLength(0), 1];
            for (int j = 0; j < a.GetLength(0); j++)
            {
                r[j, 0] = 1.0;
                for (int i = 0; i < a.GetLength(1); i++)
                {
                    r[j, 0] *= a[j, i];
                }
                r[j, 0] = Math.Pow(r[j, 0], 1.0 / Convert.ToDouble(a.GetLength(1)));
            }
            return r;
        }

        private bool novaClasse(int iClasse, ArrayList arLista)
        {
            for (int i = 0; i < arLista.Count; i++)
            {
                if (Convert.ToInt32(arLista[i]) == iClasse) return (false);
            }
            return (true);
        }

        private int idClasse(int iClasse, ArrayList arLista)
        {
            for (int i = 0; i < arLista.Count; i++)
            {
                if (Convert.ToInt32(arLista[i]) == iClasse) return (i);
            }
            return (-1);
        }

        public int[] ConverteClusterTree(double[,] mClusterTree, int iColuna)
        {
            ArrayList arLista = new ArrayList();
            int[] iSaida = new int[mClusterTree.GetLength(0)];
            for (int i = 0; i < iSaida.Length; i++)
            {
                iSaida[i] = (int)mClusterTree[i, iColuna];
                if (novaClasse(iSaida[i], arLista) == true)
                {
                    arLista.Add(iSaida[i]);
                }
            }
            //FileInfo t = new FileInfo(@"F:\IpeaGEO.txt");
            //StreamWriter Tex = t.CreateText();
            for (int i = 0; i < iSaida.Length; i++)
            {
                iSaida[i] = idClasse(iSaida[i], arLista);
                //Tex.WriteLine(i.ToString() + "\t" + iSaida[i].ToString());
            }
            //Tex.Close();
            return (iSaida);
        }

        /// <summary>Returns the sum of elements of the rows of a matrix</summary>
        public double[,] Sumr(double[,] a)
        {
            double[,] r = new double[a.GetLength(0), 1];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                r[i, 0] = 0.0;
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    r[i, 0] += a[i, j];
                }
            }
            return r;
        }
        
        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="a">Input matrix</param>
        /// <param name="i0">Start row index</param>
        /// <param name="i1">End row index</param>
        public double[] SubMatriz(double[] a, int i0, int i1)
        {
            if ((i0 > i1) || (i0 < 0) || (i0 >= a.GetLength(0) || (i1 < 0) || (i1 >= a.GetLength(0))))
            {
                throw new ArgumentException();
            }
            double[] X = new double[i1 - i0 + 1];
            for (int i = i0; i <= i1; i++)
            {
                X[i - i0] = a[i];             
            }
            return X;
        }

		/// <summary>Returns a sub matrix extracted from the current matrix.</summary>
        /// <param name="a">Input matrix</param>
		/// <param name="i0">Start row index</param>
		/// <param name="i1">End row index</param>
		/// <param name="j0">Start column index</param>
		/// <param name="j1">End column index</param>
		public double[,] SubMatriz(double[,] a, int i0, int i1, int j0, int j1)
		{
			if ((i0 > i1) || (j0 > j1) ||  (i0 < 0) || (i0 >= a.GetLength(0) ||  (i1 < 0) || (i1 >= a.GetLength(0)) ||  
				(j0 < 0) || (j0 >= a.GetLength(1)) ||  (j1 < 0) || (j1 >= a.GetLength(1))))
			{ 
				throw new ArgumentException(); 
			}			
			double[,] X = new double[i1-i0+1,j1-j0+1];
			for (int i = i0; i <= i1; i++)
			{
				for (int j = j0; j <= j1; j++)
				{
					X[i - i0,j - j0] = a[i,j];
				}
			}
			return X;
        }

        /// <summary>Returns the maximum of elements of the vector</summary>
        public double Max(double[] a)
        {
            double m = a[0];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                if (a[i] > m) m = a[i];
            }
            return m;
        }

        /// <summary>Returns the minimum of elements of the vector</summary>
        public double Min(double[] a)
        {
            double m = a[0];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                if (a[i] < m) m = a[i];
            }
            return m;
        }

        /// <summary>Returns the maximum of elements of the matrix</summary>
        public double Max(double[,] a)
        {
            double m = a[0, 0];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] > m) { m = a[i, j]; }
                }
            }
            return m;
        }
        
        /// <summary>Returns the minimum of elements of the matrix</summary>
        public double Min(double[,] a)
        {
            double m = a[0, 0];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] < m) { m = a[i, j]; }
                }
            }
            return m;
        }

        /// <summary>
        /// Retorna uma matriz com todos os elementos iguais a um, com n linhas e m colunas.
        /// </summary>
        public double[,] Ones(int n, int m)
        {
            double[,] res = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    res[i, j] = 1.0;

            return res;
        }

        /// <summary>
        /// Retorna uma matriz com todos os elementos iguais a zero, com n linhas e m colunas.
        /// </summary>
        public double[,] Zeros(int n, int m)
        {
            double[,] res = new double[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    res[i, j] = 0.0;

            return res;
        }

        /// <summary>
        /// Retorna uma matriz quadrada, com dimensão m, com todos os elementos iguais a um.
        /// </summary>
        public double[,] unit(int m)
        {
            double[,] res = new double[m, m];
            for (int i = 0; i < m; i++) res[i, i] = 1.0;

            return res;
        }

        public int columns(double[,] a) { return a.GetLength(1); }
        public int rows(double[,] a) { return a.GetLength(0); }

        /// <summary>
        /// Sort all rows of a matrix, according to a specified column (between 0 and n-1, where
        /// n is the number of columns). 
        /// </summary>
        /// <param name="sortedx">Output sorted matrix by specified columns.</param>
        /// <param name="x">Matrix to be sorted.</param>
        /// <param name="sorting_column">Integers with indexes of sorting columns.</param>
        /// <returns>Returns 1 if algorithm successful or -1 if indexes out of range.</returns>
        public int SortByColumn(ref object[,] sortedx, ref object[,] lista_categorias, 
                                ref int[] frequencia_categorias, ref int numero_categorias,
                                object[,] x, int[] sorting_columns)
        {
            int[] sortcolumn = new int[sorting_columns.GetLength(0)];
            for (int i = 0; i < sortcolumn.GetLength(0); i++)
            {
                sortcolumn[i] = sorting_columns[i];
                if (sortcolumn[i] > x.GetLength(1) - 1) sortcolumn[i] = x.GetLength(1) - 1;
                if (sortcolumn[i] < 0) sortcolumn[i] = 0;
            }

            Rank rk = new Rank();

            //object[,] v = this.SubColumnArrayDouble(x, sortcolumn);

            object[,] v = new object[x.GetLength(0), 1];
            StringBuilder s = new StringBuilder();

            for (int i = 0; i < x.GetLength(0); i++)
            {
                s = new StringBuilder(x[i, sortcolumn[0]].ToString());
                for (int k = 1; k < sortcolumn.GetLength(0); k++)
                {
                    s.Append("_" + x[i, sortcolumn[k]].ToString());
                }
                v[i, 0] = s.ToString();
            }

            ulong n = Convert.ToUInt64(v.GetLength(0));
            ulong[] indx = new ulong[n];
            ulong[] irank = new ulong[n];

            clsSort sort = new clsSort();

            sort.indexx(n, v, indx);
            rk.rank(ref indx, ref irank);

            string[] sv = new string[v.GetLength(0)];

            object[,] res = new object[x.GetLength(0), x.GetLength(1)];
            for (int i = 0; i < x.GetLength(0); i++)
            {
                sv[Convert.ToInt32(irank[i])] = v[i, 0].ToString();
                for (int j = 0; j < x.GetLength(1); j++)
                {
                    res[Convert.ToInt32(irank[i]), j] = x[i, j];
                }
            }

            int n_categorias = 1;
            string s_foco = sv[0];
            for (int i = 1; i < sv.GetLength(0); i++)
            {
                if (sv[i] != s_foco)
                {
                    n_categorias++;
                    s_foco = sv[i];
                }
            }

            object[,] categorias = new object[n_categorias, sorting_columns.GetLength(0)];
            int[] freq_categorias = new int[n_categorias]; 
            s_foco = sv[0];
            int row = 0;

            for (int j = 0; j < sorting_columns.GetLength(0); j++)
            {
                categorias[0, j] = res[0, sorting_columns[j]];
            }
            freq_categorias[0] = 1 + freq_categorias[0];

            for (int i = 1; i < sv.GetLength(0); i++)
            {
                if (sv[i] != s_foco)
                {
                    row++;
                    for (int j = 0; j < sorting_columns.GetLength(0); j++)
                    {
                        categorias[row, j] = res[i, sorting_columns[j]];
                    }
                    s_foco = sv[i];
                    freq_categorias[row] = freq_categorias[row] + 1;
                }
                else
                {
                    freq_categorias[row] = freq_categorias[row] + 1;
                }
            }

            lista_categorias = this.ArrayDoubleClone(categorias);
            frequencia_categorias = freq_categorias;
            numero_categorias = n_categorias;
            sortedx = this.ArrayDoubleClone(res);
            return 1;
        }

        /// <summary>
        /// Sort all rows of a matrix, according to a specified column (between 0 and n-1, where
        /// n is the number of columns). 
        /// </summary>
        /// <param name="sortedx">Output sorted matrix by specified columns.</param>
        /// <param name="x">Matrix to be sorted.</param>
        /// <param name="sorting_column">Integer with index of sorting column.</param>
        /// <returns>Returns 1 if algorithm successful or -1 if indexes out of range.</returns>
        public int SortByColumn(ref object[,] sortedx, object[,] x, int sorting_column)
        {
            int sortcolumn = sorting_column;
            if (sortcolumn > x.GetLength(1) - 1) sortcolumn = x.GetLength(1) - 1;
            if (sortcolumn < 0) sortcolumn = 0;

            Rank rk = new Rank();

            object[,] v = this.SubColumnArrayDouble(x, sortcolumn);

            ulong n = Convert.ToUInt64(v.GetLength(0));
            ulong[] indx = new ulong[n];
            ulong[] irank = new ulong[n];

            clsSort sort = new clsSort();

            sort.indexx(n, v, indx);
            rk.rank(ref indx, ref irank);

            object[,] res = new object[x.GetLength(0), x.GetLength(1)];
            for (int i = 0; i < x.GetLength(0); i++)
            {
                for (int j = 0; j < x.GetLength(1); j++)
                {
                    res[Convert.ToInt32(irank[i]), j] = x[i, j];
                }
            }

            sortedx = this.ArrayDoubleClone(res);
            return 1;
        }

        /// <summary>
        /// Função para retornar a matriz de ranks a partir de uma determinada matriz de entrada.
        /// </summary>
        /// <param name="v">Matriz de doubles de entrada.</param>
        /// <returns>Substitui cada coluna da matriz original, pelos ranks das observações naquela coluna. Os ranks vão de 1 até o número de 
        /// valores distintos na coluna.</returns>
        public double[,] ArrayRanks(double[,] v)
        {
            double[,] r;
            double[,] ftable = new double[0, 0];
            double[,] ranks = new double[v.GetLength(0), v.GetLength(1)];
            int n = v.GetLength(0);

            for (int k = 0; k < v.GetLength(1); k++)
            {
                r = this.SubColumnArrayDouble(v, k);
                this.FrequencyTable(ref ftable, r);

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < ftable.GetLength(0); j++)
                    {
                        if (v[i, k] == ftable[j, 0])
                        {
                            ranks[i, k] = (double)(j + 1);
                            break;
                        }
                    }
                }
            }

            return ranks;
        }







        #region RankEmpates

        /// <summary> Função que rankei levando em consideração os empates
        /// 
        /// </summary> 
        /// <param name="v"> Matriz de dados</param>
        /// <returns> Matriz de dados rankeado</returns>


        public double[,] ArrayRanksEmpate(double[,] v)
        {
            double[,] r;
            double[,] ftable = new double[0, 0];
            double[,] ranks = new double[v.GetLength(0), v.GetLength(1)];
            int n = v.GetLength(0);

            for (int k = 0; k < v.GetLength(1); k++)
            {
                r = this.SubColumnArrayDouble(v, k);
                this.FrequencyTable(ref ftable, r);
                double num = 0.0;
                for (int j = 0; j < ftable.GetLength(0); j++)
                {
                    double acum = 0.0;
                    for (int g = 0; g < ftable[j, 1]; g++)
                    {

                        num += 1;
                        acum += num;

                    }
                    double valor = acum / ftable[j, 1];
                    ftable[j, 1] = valor;

                }


                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < ftable.GetLength(0); j++)
                    {
                        if (v[i, k] == ftable[j, 0])
                        {
                            ranks[i, k] = (double)ftable[j, 1];
                            break;
                        }
                    }
                }
            }

            return ranks;
        }

        /// <summary> Função que rankeia levando em consideração os empates e também retorna a frequência de empates:Função especifica para o teste Kruskal Wallis
        /// 
        /// </summary>
        /// <param name="v"> Matriz de dados</param>
        /// <param name="ranks">Matriz de dados rankeado</param>
        /// <param name="empates">Frequência de empates</param>
        public void ArrayRanksEmpateC(double[,] v, out double[,] ranks, out double[,] empates)
        {
            double[,] r;
            double[,] ftable = new double[0, 0];
            ranks = new double[v.GetLength(0), v.GetLength(1)];
            empates = new double[v.GetLength(0), v.GetLength(1)];
            int n = v.GetLength(0);


            for (int k = 0; k < v.GetLength(1); k++)
            {
                r = this.SubColumnArrayDouble(v, k);
                this.FrequencyTable(ref ftable, r);
                double[,] emp = new double[ftable.GetLength(0), 1];
                double num = 0.0;
                for (int j = 0; j < ftable.GetLength(0); j++)
                {
                    if (ftable[j, 1] == 1)
                    {
                        emp[j, 0] = 0;
                    }
                    else
                    {
                        emp[j, 0] = 1;
                    }

                    double acum = 0.0;
                    for (int g = 0; g < ftable[j, 1]; g++)
                    {

                        num += 1;
                        acum += num;

                    }
                    double valor = acum / ftable[j, 1];
                    ftable[j, 1] = valor;
                }


                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < ftable.GetLength(0); j++)
                    {
                        if (v[i, k] == ftable[j, 0])
                        {
                            ranks[i, k] = (double)ftable[j, 1];
                            empates[i, k] = (double)emp[j, 0];
                            break;
                        }
                    }
                }
            }

        }





        #endregion



        public double Sum(double[,] v)
        {
            double res = 0.0;
            for (int i = 0; i < v.GetLength(0); i++)
            {
                for (int j = 0; j < v.GetLength(1); j++)
                {
                    res += v[i, j];
                }
            }
            return res;
        }

        public void GeraSubRows(ref double[] res, double[] dados, int first_row, int last_row)
        {
            if (first_row < 0 || last_row > dados.GetLength(0) - 1) throw new Exception("Primeira e última linhas não válidas em rotina de sublinhas");
            res = new double[last_row - first_row + 1];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                res[i] = dados[i + first_row];
            }
        }

        public void GeraSubRows(ref double[,] res, double[,] dados, int first_row, int last_row)
        {
            if (first_row < 0 || last_row > dados.GetLength(0)-1) throw new Exception("Primeira e última linhas não válidas em rotina de sublinhas");
            res = new double[last_row - first_row + 1, dados.GetLength(1)];
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                    res[i, j] = dados[i+first_row, j];
            }
        }

        /// <summary>
        /// Retorna os desvios padrões amostrais das observações em cada coluna de uma matriz de dados.
        /// </summary>
        /// <param name="dados">Matriz de dados.</param>
        /// <returns>Vetor linha com os desvios padrões amostrais.</returns>
        public double[,] Despadca(double[,] dados)
        {
            double[,] a = this.Varianciaca(dados);
            for (int i = 0; i < a.GetLength(1); i++)
            {
                a[0, i] = Math.Sqrt(a[0, i]);
            }
            return a;
        }

        /// <summary>
        /// Retorna os desvios padrões populacionais das observações em cada coluna de uma matriz de dados.
        /// </summary>
        /// <param name="dados">Matriz de dados.</param>
        /// <returns>Vetor linha com os desvios padrões.</returns>
        public double[,] Despadc(double[,] dados)
        {
            double[,] a = this.Varianciac(dados);
            for (int i = 0; i < a.GetLength(1); i++)
            {
                a[0, i] = Math.Sqrt(a[0, i]);
            }
            return a;
        }

        /// <summary>
        /// Retorna as variâncias amostrais das observações em cada coluna de uma matriz de dados.
        /// </summary>
        /// <param name="dados">Matriz de dados.</param>
        /// <returns>Vetor linha com as variâncias amostrais.</returns>
        public double[,] Varianciaca(double[,] dados)
        {
            double[,] media = this.Meanc(dados);
            double[,] res2 = new double[dados.GetLength(0), dados.GetLength(1)];
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                for (int j = 0; j < dados.GetLength(1); j++)
                {

                    if (double.IsInfinity(dados[i, j]) || double.IsNaN(dados[i, j])
                        || double.IsNegativeInfinity(dados[i, j]) || double.IsPositiveInfinity(dados[i, j]))
                    {
                        res2[i, j] = double.NaN;
                    }
                    else
                    {

                        res2[i, j] = Math.Pow(dados[i, j] - media[0, j], 2.0);

                    }
                }
            }
            double n = ((double)dados.GetLength(0));
            res2 = this.MatrizMult(this.MatrizMult(this.Meanc(res2),n),(1/(n-(1.0))));

            return (res2);
        }

       
        /// <summary>
        /// Retorna as variâncias populacionais das observações em cada coluna de uma matriz de dados.
        /// </summary>
        /// <param name="dados">Matriz de dados.</param>
        /// <returns>Vetor linha com as variâncias.</returns>
        public double[,] Varianciac(double[,] dados)
        {
            double[,] media = this.Meanc(dados);
            double[,] res2 = new double[dados.GetLength(0), dados.GetLength(1)];
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                for (int j = 0; j < dados.GetLength(1); j++)
                {

                    if (double.IsInfinity(dados[i, j]) || double.IsNaN(dados[i, j])
                        || double.IsNegativeInfinity(dados[i, j]) || double.IsPositiveInfinity(dados[i, j]))
                    {
                        res2[i, j] = double.NaN;
                    }
                    else
                    {

                        res2[i, j] = Math.Pow(dados[i, j] - media[0, j], 2.0);

                    }
                }
            }
            return this.Meanc(res2);
        }

        /// <summary>
        /// Retorna as variâncias amostrais das observações em cada coluna de uma matriz de dados.
        /// </summary>
        /// <param name="dados"></param>
        /// <returns></returns>
        public double VarianciaColumnMatrix(double[,] dados)
        {
            double media = 0.0;
            for (int i = 0; i < dados.GetLength(0); i++) media += dados[i, 0];
            media /= (double)dados.GetLength(0);

            double v = 0.0;
            for (int i = 0; i < dados.GetLength(0); i++) v += Math.Pow((dados[i, 0] - media), 2.0);
            v /= (double)(dados.GetLength(0) - 1);
            return v;
        }

        /// <summary>
        /// Retorna as variâncias amostrais das observações em cada coluna de uma matriz de dados.
        /// </summary>
        /// <param name="dados">Dados com variáveis organizadas em colunas.</param>
        /// <returns>Vetor linha com a variância amostral de cada coluna.</returns>
        public double[,] VarianciasColumnMatrix(double[,] dados)
        {
            if (dados.GetLength(0) <= 1) return new double[1, dados.GetLength(1)];

            double[,] media = new double[1, dados.GetLength(1)];
            for (int k = 0; k < dados.GetLength(1); k++)
            {
                for (int i = 0; i < dados.GetLength(0); i++) media[0, k] += dados[i, k];
                media[0, k] /= (double)dados.GetLength(0);
            }

            double[,] v = new double[1, dados.GetLength(1)];
            for (int k = 0; k < dados.GetLength(1); k++)
            {
                for (int i = 0; i < dados.GetLength(0); i++) v[0,k] += Math.Pow((dados[i, k] - media[0,k]), 2.0);
                v[0,k] /= (double)(dados.GetLength(0) - 1);
            }
            return v;
        }

        public string[] RemoveElementoArrayString(string[] v, int elemento)
        {
            if (elemento < 0 || elemento > v.GetLength(0) - 1) throw new Exception("Dimensão fora do vetor de strings");

            string[] novo = new string[v.GetLength(0) - 1];

            ArrayList temp = new ArrayList();
            for (int i = 0; i < v.GetLength(0); i++) temp.Add(v[i]);

            temp.RemoveAt(elemento);

            for (int i = 0; i < temp.Count; i++) novo[i] = temp[i].ToString();

            return novo;
        }

        public double[,] RemoveColumnArrayDouble(double[,] m, int col)
        {
            double[,] res = new double[m.GetLength(0), m.GetLength(1) - 1];
            ArrayList cols = new ArrayList();
            for (int i = 0; i < m.GetLength(1); i++)
            {
                if (i != col) cols.Add(i);
            }

            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < cols.Count; j++)
                {
                    res[i, j] = m[i, Convert.ToInt32(cols[j])];
                }
            }

            return res;
        }

        public object[,] ConvertMatrizDoubleToObj(double[,] m)
        {
            object[,] r = new object[m.GetLength(0), m.GetLength(1)];
            for (int i = 0; i < r.GetLength(0); i++)
            {
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    r[i, j] = m[i, j];
                }
            }
            return r;
        }

        public double[,] ConvertMatrixObjToDouble(object[,] m)
        {
            double[,] r = new double[m.GetLength(0), m.GetLength(1)];
            for (int i = 0; i < r.GetLength(0); i++)
            {
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    r[i, j] = Convert.ToDouble(m[i, j]);
                }
            }
            return r;
        }

        /// <summary>
        /// Subcolunas de uma matriz.
        /// </summary>
        //public double[,] SubColumnsArrayDouble(double[,] a, int inicio, int final)
        //{
        //    int[] cols = new int[final - inicio + 1];
        //    for (int i = 0; i < cols.GetLength(0); i++) cols[i] = i + inicio;
        //    return SubColumnsArrayDouble(a, cols);
        //}

        /// <summary>
        /// Subcolunas de uma matriz.
        /// </summary>
        //public double[,] SubColumnsArrayDouble(double[,] a, int[] indice)
        //{
        //    if (a.GetLength(0) == 0 || a.GetLength(1) == 0) return new double[0, 0];

        //    for (int i = 0; i < indice.GetLength(0); i++)
        //    {
        //        if (indice[i] < 0 || indice[i] > a.GetLength(1) - 1) throw new Exception("Índice da subcoluna inválido");
        //    }

        //    double[,] r = new double[a.GetLength(0), indice.GetLength(0)];
        //    for (int i = 0; i < r.GetLength(0); i++)
        //        for (int j = 0; j < indice.GetLength(0); j++)
        //            r[i, j] = a[i, indice[j]];

        //    return r;
        //}

        /// <summary>
        /// Subcolunas de uma matriz.
        /// </summary>
        public object[,] SubColumnsArrayObject(object[,] a, int inicio, int final)
        {
            int[] cols = new int[final - inicio + 1];
            for (int i = 0; i < cols.GetLength(0); i++) cols[i] = i + inicio;
            return SubColumnsArrayObject(a, cols);
        }

        /// <summary>
        /// Subcolunas de uma matriz.
        /// </summary>
        public object[,] SubColumnsArrayObject(object[,] a, int[] indice)
        {
            if (a.GetLength(0) == 0 || a.GetLength(1) == 0) return new object[0, 0];

            for (int i = 0; i < indice.GetLength(0); i++)
            {
                if (indice[i] < 0 || indice[i] > a.GetLength(1) - 1) throw new Exception("Índice da subcoluna inválido");
            }

            object[,] r = new object[a.GetLength(0), indice.GetLength(0)];
            for (int i = 0; i < r.GetLength(0); i++)
                for (int j = 0; j < indice.GetLength(0); j++)
                    r[i, j] = a[i, indice[j]];

            return r;
        }

        /// <summary>
        /// Subcoluna de uma matriz.
        /// </summary>
        public object[,] SubColumnArrayObject(object[,] a, int indice)
        {
            if (a.GetLength(0) == 0 || a.GetLength(1) == 0) return new object[0, 0];

            if (indice < 0 || indice > a.GetLength(1) - 1) throw new Exception("Índice da subcoluna inválido");

            object[,] r = new object[a.GetLength(0), 1];
            for (int i = 0; i < r.GetLength(0); i++)
                r[i, 0] = a[i, indice];

            return r;
        }

        /// <summary>
        /// Subcoluna de uma matriz.
        /// </summary>
        public object[,] SubColumnArrayDouble(object[,] a, int indice)
        {
            if (a.GetLength(0) == 0 || a.GetLength(1) == 0) return new object[0, 0];

            if (indice < 0 || indice > a.GetLength(1) - 1) throw new Exception("Índice da subcoluna inválido");

            object[,] r = new object[a.GetLength(0), 1];
            for (int i = 0; i < r.GetLength(0); i++)
                r[i, 0] = a[i, indice];

            return r;
        }

        /// <summary>
        /// Subcoluna de uma matriz.
        /// </summary>
        public double[,] SubColumnArrayDouble(double[,] a, int indice)
        {
            if (a.GetLength(0) == 0 || a.GetLength(1) == 0) return new double[0, 0];

            if (indice < 0 || indice > a.GetLength(1) - 1) throw new Exception("Índice da subcoluna inválido");

            double[,] r = new double[a.GetLength(0), 1];
            for (int i = 0; i < r.GetLength(0); i++)
                r[i, 0] = a[i, indice];

            return r;
        }

        /// <summary>
        /// Sublinha de uma matriz.
        /// </summary>
        public double[,] SubRowArrayDouble(double[,] a, int indice)
        {
            if (a.GetLength(0) == 0 || a.GetLength(1) == 0) return new double[0, 0];

            if (indice < 0 || indice > a.GetLength(0) - 1) throw new Exception("Índice da sublinha inválido");

            double[,] r = new double[1, a.GetLength(1)];
            for (int i = 0; i < r.GetLength(1); i++)
                r[0, i] = a[indice, i];

            return r;
        }

        /// <summary>
        /// Diferença entre duas matrizes.
        /// </summary>
        public double[,] DiffArrayDouble(double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || b.GetLength(1) != a.GetLength(1))
                throw new Exception("Matrizes devem possuir mesma dimensão em rotina de diferença de matrizes");

            double[,] r = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    r[i, j] = a[i, j] - b[i, j];

            return r;
        }

        public double[,] MatrizDiv(double[,] a, double c)
        {
            double[,] res = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j] / c;

            return res;
        }

        public double[,] MatrizDotPower(double[,] a, double c)
        {
            double[,] res = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = Math.Pow(a[i, j], c);

            return res;
        }

        public double[,] MatrizDotMult(double[,] a, double[,] b)
        {
            double[,] res = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j] * b[i, j];

            return res;
        }

        public double[,] MatrizMult(double c, double[,] a)
        {
            double[,] res = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j] * c;

            return res;
        }

        public double[,] MatrizMult(double[,] a, double c)
        {
            double[,] res = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j] * c;

            return res;
        }

        public double[,] MatrizSubtracao(double[,] a, double b)
        {
            double[,] r = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < r.GetLength(0); i++)
            {
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    r[i, j] = a[i, j] - b;
                }
            }

            return r;
        }

        public double[,] MatrizSubtracao(double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1)) throw new Exception("Dimensões das matrizes não estão adequadas para soma das matrizes");
            double[,] r = new double[a.GetLength(0), b.GetLength(1)];

            for (int i = 0; i < r.GetLength(0); i++)
            {
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    r[i, j] = a[i, j] - b[i, j];
                }
            }

            return r;
        }
        
        public double[,] MatrizSoma(double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1)) throw new Exception("Dimensões das matrizes não estão adequadas para soma das matrizes");
            double[,] r = new double[a.GetLength(0), b.GetLength(1)];

            for (int i = 0; i < r.GetLength(0); i++)
            {
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    r[i, j] = a[i, j] + b[i, j];
                }
            }

            return r;
        }

        /// <summary>
        /// Multiplies a matrix by itself transposed.
        /// </summary>
        public double[,] MatrizMultMtranspM(double[,] a)
        {
            double[,] r;
            if (a.GetLength(1) == a.GetLength(0))
            {
                r = new double[a.GetLength(0), a.GetLength(1)];

                for (int i = 0; i < r.GetLength(0); i++)
                {
                    r[i, i] = 0.0;
                    for (int k = 0; k < r.GetLength(0); k++)
                    {
                        r[i, i] += Math.Pow(a[i, k], 2.0);
                    }

                    for (int j = i + 1; j < r.GetLength(1); j++)
                    {
                        r[i, j] = 0.0;
                        for (int k = 0; k < a.GetLength(1); k++)
                        {
                            r[i, j] += a[i, k] * a[j, k];
                        }
                        r[j, i] = r[i, j];
                    }
                }
            }

            if (a.GetLength(1) == 1)
            {
                r = new double[1, 1];
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    r[0, 0] += Math.Pow(a[i, 0], 2.0); 
                }
                return r;
            }

            if (a.GetLength(0) == 1)
            {
                r = new double[a.GetLength(1), a.GetLength(1)];
                for (int i = 0; i < a.GetLength(1); i++)
                {
                    for (int j = 0; j < a.GetLength(1); j++)
                    {
                        r[i, j] = a[0, i] * a[0, j];
                    }
                }
                return r;
            }

            throw new Exception("Matriz de input deve ser quadrada ou matriz coluna ou matriz linha.");
        }

        public double[,] MatrizMult(double[,] a, double[,] b)
        {
            if (a.GetLength(1) != b.GetLength(0)) throw new Exception("Dimensões das matrizes não estão adequadas para multiplicação");
            double[,] r = new double[a.GetLength(0), b.GetLength(1)];

            for (int i = 0; i < r.GetLength(0); i++)
            {
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    r[i, j] = 0.0;
                    for (int k = 0; k < a.GetLength(1); k++)
                        r[i, j] += a[i, k] * b[k, j];
                }
            }

            return r;
        }

        public double[,] MatrizTransp(double[,] a)
        {
            double[,] r = new double[a.GetLength(1), a.GetLength(0)];
            for (int i = 0; i < r.GetLength(0); i++)
                for (int j = 0; j < r.GetLength(1); j++)
                    r[i, j] = a[j, i];

            return r;
        }

        public double[,] CorrSampleMatrix(double[,] col_data)
        {
            double[,] v = this.CovSampleMatrix(col_data);
            double[,] c = new double[v.GetLength(0), v.GetLength(1)];
            for (int i = 0; i < c.GetLength(0); i++)
            {
                c[i, i] = 1.0;
                for (int j = i+1; j < c.GetLength(0); j++)
                {
                    c[i, j] = v[i, j] / Math.Sqrt(v[i,i]*v[j,j]);
                    c[j, i] = c[i, j];
                }
            }
            return c;
        }

        public double[,] CovSampleMatrix(double[,] col_data)
        {
            int nvar = col_data.GetLength(1);
            int nobs = col_data.GetLength(0);

            double[,] medias = this.Meanc(col_data);
            double[,] demeaned = new double[col_data.GetLength(0), col_data.GetLength(1)];

            for (int i = 0; i < nobs; i++)
            {
                for (int j = 0; j < nvar; j++)
                {
                    demeaned[i, j] = col_data[i, j] - medias[0, j];
                }
            }

            double[,] covm = this.MatrizMult(this.MatrizTransp(demeaned), demeaned);

            if (nobs > 1)
            {
                for (int i = 0; i < covm.GetLength(0); i++)
                    for (int j = 0; j < covm.GetLength(1); j++)
                        covm[i, j] = covm[i, j] / (double)(nobs - 1.0);
            }

            return covm;
        }
        
        /// <summary>Norma Euclidiana de uma matriz.</summary>
        public double Norm(double[,] A)
        {
            double f = 0;
            for (int j = 0; j < A.GetLength(1); j++)
            {
                for (int i = 0; i < A.GetLength(0); i++)
                {
                    f += Math.Pow(A[i, j], 2);
                }
            }
            return Math.Sqrt(f);
        }

        /// <summary>Mínimos das colunas de uma matriz.</summary>
        public double[,] Minc(double[,] a)
        {
            double[,] r = new double[1, a.GetLength(1)];
            for (int j = 0; j < a.GetLength(1); j++)
            {
                r[0, j] = a[0, j];
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    if (r[0, j] > a[i, j]) r[0, j] = a[i, j];
                }
            }
            return r;
        }

        /// <summary>Máximos das colunas de uma matriz.</summary>
        public double[,] Maxc(double[,] a)
        {
            double[,] r = new double[1, a.GetLength(1)];
            for (int j = 0; j < a.GetLength(1); j++)
            {
                r[0, j] = a[0,j];
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    if (r[0, j] < a[i, j]) r[0, j] = a[i,j];
                }
            }
            return r;
        }

        /// <summary>
        /// Calcula a Trimean das colunas de uma matriz
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public double[,] Trimean(double[,] a)
        {
            double[,] r = new double[1, a.GetLength(1)];
            double[,] dados;
            for (int j = 0; j < a.GetLength(1); j++)
            {
                dados = this.SubColumnArrayDouble(a, j);
                r[0, j] = (this.Percentil(dados, 25.0) + 2 * this.Percentil(dados, 50.0) + this.Percentil(dados, 75.0))/4;
            }

            return r;
        }

        /// <summary>
        ///  Calcula o Midrange das colunas de uma matriz
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public double[,] Midrange(double[,] a)
        {
            double[,] r = new double[1, a.GetLength(1)];
            double[,] dados;
            for (int j = 0; j < a.GetLength(1); j++)
            {
                dados = this.SubColumnArrayDouble(a, j);
                r[0, j] = (this.Max(dados) + this.Min(dados)) / 2;
            }

            return r;
        }

        public double[,] Medianc(double[,] a)
        {
            double[,] r = new double[1, a.GetLength(1)];
            double[,] dados;

            //double[,] sorted_a = this.SortcDoubleArray(a);
            //int indice_baixo = 0;
            //int indice_alto = 0;

            //if (Math.Floor((double)a.GetLength(0) / 2.0) == (double)a.GetLength(0) / 2.0)
            //{
            //    indice_baixo = (int)Math.Floor((double)a.GetLength(0) / 2.0);
            //    indice_alto = (int)Math.Floor((double)a.GetLength(0) / 2.0) + 1;
            //}
            //else
            //{
            //    indice_baixo = (int)Math.Floor((double)a.GetLength(0) / 2.0) + 1;
            //    indice_alto = (int)Math.Floor((double)a.GetLength(0) / 2.0) + 1;
            //}

            for (int j = 0; j < a.GetLength(1); j++)
            {
                dados = this.SubColumnArrayDouble(a, j);
                r[0, j] = this.Percentil(dados, 50.0);

                //r[0, j] = (sorted_a[indice_baixo-1, j] + sorted_a[indice_alto-1, j])/2.0;
            }

            return r;
        }

        public double Mean(double[,] a)
        {
            double m = 0.0;
            int n = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (double.IsInfinity(a[i, j]) || double.IsNaN(a[i, j])
                        || double.IsNegativeInfinity(a[i, j]) || double.IsPositiveInfinity(a[i, j]))
                    {
                    }
                    else
                    {
                        m += a[i, j];
                        n++;
                    }
                }
            }
            if (n > 0)
            {
                return m / ((double)n);
            }
            return double.NaN;
        }

        public double Mean(double[] a)
        {
            double m = 0.0;
            int n = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                if (double.IsInfinity(a[i]) || double.IsNaN(a[i]) 
                    || double.IsNegativeInfinity(a[i]) || double.IsPositiveInfinity(a[i]))
                {
                }
                else
                {
                    m += a[i];
                    n++;
                }
            }
            if (n > 0)
            {
                return m / ((double)n);
            }
            return double.NaN;
        }

        /// <summary>Média das colunas de uma matriz.</summary>
        public double[,] Meanc(double[,] a)
        {
            double[,] r = new double[1, a.GetLength(1)];
            for (int j = 0; j < a.GetLength(1); j++)
            {
                double m = 0.0;
                int n = 0;
                r[0, j] = 0.0;
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    if (double.IsInfinity(a[i, j]) || double.IsNaN(a[i, j])
                        || double.IsNegativeInfinity(a[i, j]) || double.IsPositiveInfinity(a[i, j]))
                    {
                    }
                    else
                    {

                        m += a[i, j];
                        n++;

                    }
                }


                if (n > 0)
                {
                   r[0, j]= m / ((double)n);
                }
                else
                {
                    r[0, j]= double.NaN;

                }

            }
            return r;
        }

        /// <summary>
        /// Cálculo da matriz de variâncias e covariâncias.
        /// </summary>
        /// <param name="matriz_De_Entrada">Matriz de dados.</param>
        /// <returns></returns>
        public double[,] Matriz_De_Covariancia(double[,] matriz_De_Entrada)
        {
            ulong n = (ulong)matriz_De_Entrada.GetLength(0);


            double[,] matriz_De_Covariancia = new double[matriz_De_Entrada.GetLength(1), matriz_De_Entrada.GetLength(1)];

            for (int Coluna_X = 0; Coluna_X < matriz_De_Entrada.GetLength(1); Coluna_X++)
            {
                for (int Coluna_Y = 0; Coluna_Y < matriz_De_Entrada.GetLength(1); Coluna_Y++)
                {
                    double yt, xt, erro = 0.0, total = 0.0, cov = 0.0;
                    double syy = 0.0, sxy = 0.0, sxx = 0.0, ay = 0.0, ax = 0.0;
                    for (int j = 0; j < (int)n; j++)
                    {
                        if (double.IsNaN(matriz_De_Entrada[j, Coluna_X]) != true && double.IsNaN(matriz_De_Entrada[j, Coluna_Y]) != true)
                        {
                            ax += matriz_De_Entrada[j, Coluna_X];
                            ay += matriz_De_Entrada[j, Coluna_Y];
                        }
                        else
                        {
                            erro += 1;
                        }
                    }

                    total = (double)n - erro;

                    ax /= total;
                    ay /= total;

                    for (int k = 0; k < (int)n; k++)
                    {
                        if (double.IsNaN(matriz_De_Entrada[k, Coluna_X]) != true && double.IsNaN(matriz_De_Entrada[k, Coluna_Y]) != true)
                        {
                            xt = matriz_De_Entrada[k, Coluna_X] - ax;
                            yt = matriz_De_Entrada[k, Coluna_Y] - ay;
                            sxx += xt * xt;
                            syy += yt * yt;
                            sxy += xt * yt;
                        }
                    }

                    cov = (sxy) / (total - 1);
                    matriz_De_Covariancia[Coluna_X, Coluna_Y] = cov;

                }
            }

            return matriz_De_Covariancia;

        }      

        /// <summary>
        /// Concatenação de vetores.
        /// </summary>
        public double[] ConcateArraysDouble(double[] a, double[] b)
        {
            if (a.GetLength(0) == 0)
            {
                double[] rb = new double[b.GetLength(0)];
                for (int i = 0; i < rb.GetLength(0); i++) rb[i] = b[i];
                return rb;
            }

            if (b.GetLength(0) == 0)
            {
                double[] ra = new double[a.GetLength(0)];
                for (int i = 0; i < ra.GetLength(0); i++) ra[i] = a[i];
                return ra;
            }

            double[] res = new double[a.GetLength(0) + b.GetLength(0)];
            for (int j = 0; j < a.GetLength(0); j++)
                res[j] = a[j];

            for (int j = 0; j < b.GetLength(0); j++)
                res[j + a.GetLength(0)] = b[j];

            return res;
        }

        /// <summary>
        /// Concatenação vertical de matrizes.
        /// </summary>
        public object[,] Concatev(object[,] a, object[,] b)
        {
            if (b.GetLength(0) == 0 && b.GetLength(1) == 0) return this.ArrayDoubleClone(a);
            if (a.GetLength(0) == 0 && a.GetLength(1) == 0) return this.ArrayDoubleClone(b);

            if (a.GetLength(1) != b.GetLength(1)) throw new Exception("Matrizes devem possuir o mesmo número de colunas em concatenação vertical");

            object[,] res = new object[a.GetLength(0) + b.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(1); i++)
            {
                for (int j = 0; j < a.GetLength(0); j++)
                    res[j, i] = a[j, i];

                for (int j = 0; j < b.GetLength(0); j++)
                    res[j + a.GetLength(0), i] = b[j, i];
            }

            return res;
        }

        /// <summary>
        /// Concatenação vertical de matrizes.
        /// </summary>
        public double[,] Concatev(double[,] a, double[,] b)
        {
            if (b.GetLength(0) == 0 && b.GetLength(1) == 0) return this.ArrayDoubleClone(a);
            if (a.GetLength(0) == 0 && a.GetLength(1) == 0) return this.ArrayDoubleClone(b);

            if (a.GetLength(1) != b.GetLength(1)) throw new Exception("Matrizes devem possuir o mesmo número de colunas em concatenação vertical");

            double[,] res = new double[a.GetLength(0) + b.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(1); i++)
            {
                for (int j = 0; j < a.GetLength(0); j++)
                    res[j, i] = a[j, i];

                for (int j = 0; j < b.GetLength(0); j++)
                    res[j + a.GetLength(0), i] = b[j, i];
            }

            return res;
        }

        /// <summary>
        /// Concatenação horizontal de matrizes.
        /// </summary>
        public object[,] Concateh(object[,] a, double[,] b)
        {
            if (b.GetLength(0) == 0 && b.GetLength(1) == 0) return this.ArrayObjectClone(a);
            if (a.GetLength(0) == 0 && a.GetLength(1) == 0) return this.ArrayObjectClone(b);

            if (a.GetLength(0) != b.GetLength(0)) throw new Exception("Matrizes devem possuir o mesmo número de linhas em concatenação horizontal");

            object[,] res = new object[a.GetLength(0), a.GetLength(1) + b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j];

                for (int j = 0; j < b.GetLength(1); j++)
                    res[i, a.GetLength(1) + j] = b[i, j];
            }

            return res;
        }

        /// <summary>
        /// Concatenação horizontal de matrizes.
        /// </summary>
        public object[,] Concateh(double[,] a, object[,] b)
        {
            if (b.GetLength(0) == 0 && b.GetLength(1) == 0) return this.ArrayObjectClone(a);
            if (a.GetLength(0) == 0 && a.GetLength(1) == 0) return this.ArrayObjectClone(b);

            if (a.GetLength(0) != b.GetLength(0)) throw new Exception("Matrizes devem possuir o mesmo número de linhas em concatenação horizontal");

            object[,] res = new object[a.GetLength(0), a.GetLength(1) + b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j];

                for (int j = 0; j < b.GetLength(1); j++)
                    res[i, a.GetLength(1) + j] = b[i, j];
            }

            return res;
        }

        /// <summary>
        /// Concatenação horizontal de matrizes.
        /// </summary>
        public object[,] Concateh(object[,] a, object[,] b)
        {
            if (b.GetLength(0) == 0 && b.GetLength(1) == 0) return this.ArrayObjectClone(a);
            if (a.GetLength(0) == 0 && a.GetLength(1) == 0) return this.ArrayObjectClone(b);

            if (a.GetLength(0) != b.GetLength(0)) throw new Exception("Matrizes devem possuir o mesmo número de linhas em concatenação horizontal");

            object[,] res = new object[a.GetLength(0), a.GetLength(1) + b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j];

                for (int j = 0; j < b.GetLength(1); j++)
                    res[i, a.GetLength(1) + j] = b[i, j];
            }

            return res;
        }

        public string[] Concate(string[] a, string[] b)
        {
            string[] r = new string[a.GetLength(0) + b.GetLength(0)];
            ArrayList temp = new ArrayList();
            for (int i = 0; i < a.GetLength(0); i++) temp.Add(a[i]);
            for (int i = 0; i < b.GetLength(0); i++) temp.Add(b[i]);
            for (int i = 0; i < temp.Count; i++) r[i] = temp[i].ToString();

            return r;
        }

        /// <summary>
        /// Concatenação horizontal de matrizes.
        /// </summary>
        public double[,] Concateh(double[,] a, double[,] b)
        {
            if (b.GetLength(0) == 0 && b.GetLength(1) == 0) return this.ArrayDoubleClone(a);
            if (a.GetLength(0) == 0 && a.GetLength(1) == 0) return this.ArrayDoubleClone(b);

            if (a.GetLength(0) != b.GetLength(0)) throw new Exception("Matrizes devem possuir o mesmo número de linhas em concatenação horizontal");

            double[,] res = new double[a.GetLength(0), a.GetLength(1) + b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j];

                for (int j = 0; j < b.GetLength(1); j++)
                    res[i, a.GetLength(1) + j] = b[i, j];
            }

            return res;
        }

        /// <summary>
        /// Clona uma matriz.
        /// </summary>
        public double[,] ArrayDoubleClone(double[] a)
        {
            double[,] res = new double[a.GetLength(0), 1];
            for (int i = 0; i < a.GetLength(0); i++)
                    res[i, 0] = a[i];

            return res;
        }

        /// <summary>
        /// Clona uma matriz.
        /// </summary>
        public object[,] ArrayObjectClone(object[] a)
        {
            object[,] res = new object[a.GetLength(0), 1];
            for (int i = 0; i < a.GetLength(0); i++)
                res[i, 0] = a[i];

            return res;
        }

        /// <summary>
        /// Clona uma matriz.
        /// </summary>
        public object[,] ArrayDoubleClone(object[,] a)
        {
            object[,] res = new object[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j];

            return res;
        }

        /// <summary>
        /// Clona uma matriz.
        /// </summary>
        public object[,] ArrayObjectClone(double[,] a)
        {
            object[,] res = new object[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j];

            return res;
        }

        /// <summary>
        /// Clona uma matriz.
        /// </summary>
        public object[,] ArrayObjectClone(object[,] a)
        {
            object[,] res = new object[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j];

            return res;
        }

        /// <summary>
        /// Clona uma matriz.
        /// </summary>
        public double[,] ArrayDoubleClone(double[,] a)
        {
            double[,] res = new double[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[i, j] = a[i, j];

            return res;
        }

        public int[] sort(int[] a)
        {
            double[,] x = new double[a.GetLength(0), 1];
            for (int i = 0; i < x.GetLength(0); i++) x[i, 0] = Convert.ToDouble(a[i]);

            x = this.SortcDoubleArray(x);

            int[] res = new int[x.GetLength(0)];
            for (int i = 0; i < x.GetLength(0); i++) res[i] = Convert.ToInt32(x[i,0]);

            return res;
        }

        /// <summary>
        /// Retorna uma matriz, com colunas ordenadas independentemente.
        /// </summary>
        public double[,] SortcDoubleArray(double[,] x)
        {
            int problem = 1;
            string message = "";

            clsSort auxc = new clsSort();
            int n = x.GetLength(0);
            ulong ln = Convert.ToUInt64(n);
            double[] a = new double[n];
            for (int i = 0; i < n; i++) { a[i] = x[i, 0]; }
            auxc.sort(ref a, ln, ref problem, ref message);
            double[,] X = this.ArrayDoubleClone(a);
            double[,] aux;
            for (int j = 1; j < x.GetLength(1); j++)
            {
                for (int i = 0; i < n; i++) { a[i] = x[i, j]; }
                auxc.sort(ref a, ln, ref problem, ref message);
                aux = this.ArrayDoubleClone(a);
                X = this.Concateh(X, aux);
            }
            return X;
        }

        /// <summary>
        /// Retorna uma matriz, com colunas ordenadas independentemente.
        /// </summary>
        public object[,] SortcDoubleArray(object[,] x)
        {
            int problem = 1;
            string message = "";

            clsSort auxc = new clsSort();
            int n = x.GetLength(0);
            ulong ln = Convert.ToUInt64(n);
            object[] a = new object[n];
            for (int i = 0; i < n; i++) { a[i] = x[i, 0]; }
            auxc.sort(ref a, ln, ref problem, ref message);
            object[,] X = this.ArrayObjectClone(a);
            object[,] aux;
            for (int j = 1; j < x.GetLength(1); j++)
            {
                for (int i = 0; i < n; i++) { a[i] = x[i, j]; }
                auxc.sort(ref a, ln, ref problem, ref message);
                aux = this.ArrayObjectClone(a);
                X = this.Concateh(X, aux);
            }
            return X;
        }

        /// <summary>
        /// Tabela de frequência para uma variável categórica. 
        /// </summary>
        /// <param name="table">Matriz de saída: primeira coluna corresponde às categorias; segunda coluna corresponde à contagem de cada categoria.</param>
        /// <param name="cats">Vetor coluna com as categorias.</param>
        public void FrequencyTable(ref object[,] table, object[,] cats)
        {
            int nobs = cats.GetLength(0);
            object[,] scats = this.SortcDoubleArray(cats);
            object[,] categorias;

            int index = 1;
            object temp = scats[0, 0];

            clsSort srt = new clsSort();

            for (int i = 1; i < nobs; i++)
            {
                //if (scats[i, 0] != temp)
                if (!srt.EqualTo(scats[i,0], temp))
                {
                    index++;
                    temp = scats[i, 0];
                }
            }

            categorias = new object[index, 1];
            categorias[0, 0] = scats[0, 0];
            index = 0;
            for (int i = 1; i < nobs; i++)
            {
                //if (scats[i, 0] != categorias[index, 0])
                if (!srt.EqualTo(scats[i,0], categorias[index, 0]))
                {
                    index++;
                    categorias[index, 0] = scats[i, 0];
                }
            }

            object[,] counts = new object[categorias.GetLength(0), 1];
            for (int i = 0; i < nobs; i++)
            {
                for (int k = 0; k < categorias.GetLength(0); k++)
                {
                    //if (scats[i, 0] == categorias[k, 0])
                    if (srt.EqualTo(scats[i,0], categorias[k,0]))
                    {
                        counts[k, 0] = Convert.ToDouble(counts[k,0]) + 1.0;
                        break;
                    }
                }
            }
            table = this.Concateh(categorias, counts);
        }

        /// <summary>
        /// Tabela de frequência para uma variável categórica. 
        /// </summary>
        /// <param name="table">Matriz de saída: primeira coluna corresponde às categorias; segunda coluna corresponde à contagem de cada categoria.</param>
        /// <param name="cats">Vetor coluna com as categorias.</param>
        public void FrequencyTable(ref double[,] table, double[,] cats)
        {
            int nobs = cats.GetLength(0);
            double[,] scats = this.SortcDoubleArray(cats);
            double[,] categorias;

            int index = 1;
            double temp = scats[0, 0];

            for (int i = 1; i < nobs; i++)
            {
                if (scats[i,0] != temp)
                {
                    index++;
                    temp = scats[i,0];
                }
            }

            categorias = new double[index, 1];
            categorias[0, 0] = scats[0,0];
            index = 0;
            for (int i = 1; i < nobs; i++)
            {
                if (scats[i,0] != categorias[index,0])
                {
                    index++;
                    categorias[index,0] = scats[i,0];
                }
            }            
            
            double[,] counts = new double[categorias.GetLength(0), 1];
            for (int i = 0; i < nobs; i++)
            {
                for (int k = 0; k < categorias.GetLength(0); k++)
                {
                    if (scats[i,0] == categorias[k,0])
                    {
                        counts[k,0]++;
                        break;
                    }
                }
            }
            table = this.Concateh(categorias, counts);
        }

        /// <summary>
        /// Gera matriz diagonal. Função similar à do matlab. Se D for uma matriz coluna, então a função retorna uma matriz diagonal, 
        /// com elementos iguais ao vetor coluna D. Caso D seja uma matriz quadrada, 
        /// então a função retorna uma matriz coluna, com os elementos da diagonal principal de D.
        /// </summary>
        /// <param name="D">Matriz de entrada.</param>
        /// <returns>Retorna uma matriz coluna ou uma matriz diagonal, dependendo das dimensões de D.</returns>
        public double[,] MatrizDiagonal(double[,] D)
        {
            if (D.GetLength(1) == D.GetLength(0))
            {
                double[,] res1 = new double[D.GetLength(0), 1];
                for (int i = 0; i < res1.GetLength(0); i++)
                {
                    res1[i, 0] = D[i, i];
                }
                return res1;
            }

            double[,] res = new double[D.GetLength(0), D.GetLength(0)];
            for (int i = 0; i < res.GetLength(0); i++)
                res[i, i] = D[i, 0];
            return res;
        }

        public double[,] MatrizInversa(double[,] mat)
        {
            GaussJordan gj = new GaussJordan();
            double[,] b = this.ArrayDoubleClone(mat);

            gj.gaussj(ref b);

            return b;
        }

        /// <summary>
        /// Calcula os autovalores e os autovetores de uma matriz simétrica (retorna autovalores ordenados)
        /// </summary>
        /// <param name="x">Matriz de entrada</param>
        /// <param name="V">Referencia para os autovetores (em colunas)</param>
        /// <param name="D">Referencis para os autovalores ordenados</param>
        public void AutovaloresMatrizSimetrica(double[,] x, ref double[,] V, ref double[] D)
        {
            Jacobi jac = new Jacobi(x);
            V = jac.v;
            D = jac.d;
        }

        public void AutovaloresMatrizAssimetrica(double[,] x, ref Complex[] autovalores)
        {
            clsAutoValorNonSymmetric clns = new clsAutoValorNonSymmetric(x, false, false);
            autovalores = clns.wri;
        }

        /// <summary>
        /// Função para calcular a singular value decomposition de uma matriz de input. A função calcula matrizes W, U, V, tais que
        /// a matriz de entrada A = U x diag(W) x transp(V). 
        /// </summary>
        public void SingularValueDecomposition(ref double[,] W, ref double[,] U, ref double[,] V,
            double[,] A)
        {
            clsSingularValueDecomposition cls = new clsSingularValueDecomposition(ref A);

            W = cls.WMatrix;
            U = cls.UMatrix;
            V = cls.VMatrix;
        }
    }

    #endregion 

    #region Classe para quicksort

    /// <summary>
    /// Given indx[1..n] as output from the routine indexx, returns an array irank[1..n], the
    /// corresponding table of ranks.
    /// </summary>
    internal class Rank
    {
        public Rank()
        {
        }

        public void rank(ref ulong[] indx, ref ulong[] irank)
        {
            ulong j = 0;

            ulong n = Convert.ToUInt64(indx.GetLength(0));
            for (j = 0; j < n; j++) irank[Convert.ToInt32(indx[j]) - 1] = j;
        }
    }

    internal class clsSort
    {
        #region Funções de comparação

        public bool LessOrEqualTo(object v1, object v2)
        {
            if ((v1 is string) && (v2 is string))
            {
                int rcompare = Convert.ToString(v1).CompareTo(Convert.ToString(v2));
                if (rcompare > 0) return false;
                else return true;
            }

            return (this.LessThan(v1, v2) || this.EqualTo(v1, v2));
        }

        public bool GreaterThan(object v1, object v2)
        {
            if ((v1 is string) && (v2 is string))
            {
                int rcompare = Convert.ToString(v1).CompareTo(Convert.ToString(v2));
                if (rcompare <= 0) return false;
                else return true;
            }

            if ((v1 is DateTime) && (v2 is DateTime))
            {
                return (Convert.ToDateTime(v1) > Convert.ToDateTime(v2));
            }

            if (!(v1 is string) && !(v2 is string))
            {
                return (Convert.ToDouble(v1) > Convert.ToDouble(v2));
            }
            else
            {
                double u1;
                double u2;
                string s1 = v1.ToString();
                string s2 = v2.ToString();
                if (double.TryParse(s1, this.m_style, this.m_culture, out u1) &&
                    double.TryParse(s2, this.m_style, this.m_culture, out u2))
                {
                    return (u1 > u2);
                }
                else
                {
                    if (s1.Length == s2.Length)
                    {
                        for (int i = 0; i < s2.Length; i++)
                        {
                            if (Convert.ToChar(s1[i]) > Convert.ToChar(s2[i])) return true;
                        }
                        return false;
                    }
                    if (s1.Length > s2.Length)
                    {
                        if (s1.Substring(0, s2.Length) == s2) return true;
                        for (int i = 0; i < s2.Length; i++)
                        {
                            if (Convert.ToChar(s1[i]) > Convert.ToChar(s2[i])) return true;
                        }
                        return false;
                    }
                    if (s1.Length < s2.Length)
                    {
                        if (s1 == s2.Substring(0, s1.Length)) return false;
                        for (int i = 0; i < s1.Length; i++)
                        {
                            if (Convert.ToChar(s1[i]) > Convert.ToChar(s2[i])) return true;
                        }
                        return false;
                    }
                    return false;
                }
            }
        }

        public bool LessThan(object v1, object v2)
        {
            if ((v1 is string) && (v2 is string))
            {
                int rcompare = Convert.ToString(v1).CompareTo(Convert.ToString(v2));
                if (rcompare >= 0) return false;
                else return true;
            }

            if ((v1 is DateTime) && (v2 is DateTime))
            {
                return (Convert.ToDateTime(v1) < Convert.ToDateTime(v2));
            }

            if (!(v1 is string) && !(v2 is string))
            {
                return (Convert.ToDouble(v1) < Convert.ToDouble(v2));
            }
            else
            {
                double u1;
                double u2;
                string s1 = v1.ToString();
                string s2 = v2.ToString();
                if (double.TryParse(s1, this.m_style, this.m_culture, out u1) &&
                    double.TryParse(s2, this.m_style, this.m_culture, out u2))
                {
                    return (u1 < u2);
                }
                else
                {
                    if (s1.Length == s2.Length)
                    {
                        for (int i = 0; i < s2.Length; i++)
                        {
                            if (Convert.ToChar(s1[i]) < Convert.ToChar(s2[i])) return true;
                        }
                        return false;
                    }
                    if (s1.Length > s2.Length)
                    {
                        if (s1.Substring(0, s2.Length) == s2) return false;
                        for (int i = 0; i < s2.Length; i++)
                        {
                            if (Convert.ToChar(s1[i]) < Convert.ToChar(s2[i])) return true;
                        }
                        return false;
                    }
                    if (s1.Length < s2.Length)
                    {
                        if (s1 == s2.Substring(0, s1.Length)) return true;
                        for (int i = 0; i < s1.Length; i++)
                        {
                            if (Convert.ToChar(s1[i]) < Convert.ToChar(s2[i])) return true;
                        }
                        return false;
                    }
                    return false;
                }
            }
        }

        public bool EqualTo(object v1, object v2)
        {
            if ((v1 is DateTime) && (v2 is DateTime))
            {
                return (Convert.ToDateTime(v1) == Convert.ToDateTime(v2));
            }

            if (!(v1 is string) && !(v2 is string))
            {
                return (Convert.ToDouble(v1) == Convert.ToDouble(v2));
            }
            else
            {
                double u1;
                double u2;
                string s1 = v1.ToString();
                string s2 = v2.ToString();
                if (double.TryParse(s1, this.m_style, this.m_culture, out u1) &&
                    double.TryParse(s2, this.m_style, this.m_culture, out u2))
                {
                    return (u1 == u2);
                }
                else
                {
                    return (s1 == s2);
                }
            }
        }

        #endregion

        private NumberStyles m_style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands 
                                       | NumberStyles.AllowCurrencySymbol | NumberStyles.Number;

		private CultureInfo m_culture = CultureInfo.CurrentCulture;
        //private CultureInfo m_culture = CultureInfo.GetCultureInfo("en-US");
        //private CultureInfo m_culture = CultureInfo.GetCultureInfo("pt-BR");

        public clsSort()
        {
            this.m_style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands
                                     | NumberStyles.AllowCurrencySymbol | NumberStyles.Number;
            //this.m_culture = CultureInfo.GetCultureInfo("en-US");
            this.m_culture = CultureInfo.CurrentCulture;
        }

        #region Sorting arrays de doubles

        private int M = 7;
        private int NSTACK = 500;
        
        public int sort(ref object[] arr, ulong n, ref int problem, ref string message)
        {
            problem = 1;
            message = "Quicksort routine successful";

            ulong i, ir = n, j, k, l = 1;
            int jstack = 0;
            object a, temp;

            ulong[] istack = new ulong[NSTACK];
            for (; ; )
            {
                if (ir - l < (ulong)M)
                {
                    for (j = l + 1; j <= ir; j++)
                    {
                        a = arr[j - 1];
                        for (i = j - 1; i >= 1; i--)
                        {
                            //if (arr[i - 1] <= a) break;

                            if (this.LessOrEqualTo(arr[i - 1], a)) break;
                            arr[i] = arr[i - 1];
                        }
                        arr[i] = a;
                    }
                    if (jstack == 0) break;
                    ir = istack[(jstack--) - 1];
                    l = istack[(jstack--) - 1];
                }
                else
                {
                    k = (l + ir) >> 1;
                    temp = arr[k - 1];
                    arr[k - 1] = arr[l];
                    arr[l] = temp;

                    //if (arr[l] > arr[ir - 1])
                    if (this.GreaterThan(arr[l], arr[ir - 1]))
                    {
                        temp = arr[l];
                        arr[l] = arr[ir - 1];
                        arr[ir - 1] = temp;
                    }

                    //if (arr[l - 1] > arr[ir - 1])
                    if (this.GreaterThan(arr[l - 1], arr[ir - 1]))
                    {
                        temp = arr[l - 1];
                        arr[l - 1] = arr[ir - 1];
                        arr[ir - 1] = temp;
                    }

                    //if (arr[l] > arr[l - 1])
                    if (this.GreaterThan(arr[l], arr[l - 1]))
                    {
                        temp = arr[l];
                        arr[l] = arr[l - 1];
                        arr[l - 1] = temp;
                    }
                    i = l + 1;
                    j = ir;
                    a = arr[l - 1];
                    for (; ; )
                    {
                        //do i++; while (arr[i - 1] < a);
                        do i++; while (this.LessThan(arr[i - 1], a));
                        
                        //do j--; while (arr[j - 1] > a);
                        do j--; while (this.GreaterThan(arr[j - 1], a));

                        if (j < i) break;
                        temp = arr[i - 1];
                        arr[i - 1] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                    arr[l - 1] = arr[j - 1];
                    arr[j - 1] = a;
                    jstack += 2;
                    if (jstack > NSTACK)
                    {
                        message = "NSTACK too small in quicksort routine";
                        problem = 1;
                        return 1;
                    }
                    if (ir - i + 1 >= j - l)
                    {
                        istack[jstack - 1] = ir;
                        istack[jstack - 2] = i;
                        ir = j - 1;
                    }
                    else
                    {
                        istack[jstack - 1] = j - 1;
                        istack[jstack - 2] = l;
                        l = i;
                    }
                }
            }
            return 0;
        }

        #endregion

        #region Sorting arrays de objects

        public int sort(ref double[] arr, ulong n, ref int problem, ref string message)
        {
            problem = 1;
            message = "Quicksort routine successful";

            ulong i, ir = n, j, k, l = 1;
            int jstack = 0;
            double a, temp;

            ulong[] istack = new ulong[NSTACK];
            for (; ; )
            {
                if (ir - l < (ulong)M)
                {
                    for (j = l + 1; j <= ir; j++)
                    {
                        a = arr[j - 1];
                        for (i = j - 1; i >= 1; i--)
                        {
                            if (arr[i - 1] <= a) break;
                            arr[i] = arr[i - 1];
                        }
                        arr[i] = a;
                    }
                    if (jstack == 0) break;
                    ir = istack[(jstack--) - 1];
                    l = istack[(jstack--) - 1];
                }
                else
                {
                    k = (l + ir) >> 1;
                    temp = arr[k - 1];
                    arr[k - 1] = arr[l];
                    arr[l] = temp;
                    if (arr[l] > arr[ir - 1])
                    {
                        temp = arr[l];
                        arr[l] = arr[ir - 1];
                        arr[ir - 1] = temp;
                    }
                    if (arr[l - 1] > arr[ir - 1])
                    {
                        temp = arr[l - 1];
                        arr[l - 1] = arr[ir - 1];
                        arr[ir - 1] = temp;
                    }
                    if (arr[l] > arr[l - 1])
                    {
                        temp = arr[l];
                        arr[l] = arr[l - 1];
                        arr[l - 1] = temp;
                    }
                    i = l + 1;
                    j = ir;
                    a = arr[l - 1];
                    for (; ; )
                    {
                        do i++; while (arr[i - 1] < a);
                        do j--; while (arr[j - 1] > a);
                        if (j < i) break;
                        temp = arr[i - 1];
                        arr[i - 1] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                    arr[l - 1] = arr[j - 1];
                    arr[j - 1] = a;
                    jstack += 2;
                    if (jstack > NSTACK)
                    {
                        message = "NSTACK too small in quicksort routine";
                        problem = 1;
                        return 1;
                    }
                    if (ir - i + 1 >= j - l)
                    {
                        istack[jstack - 1] = ir;
                        istack[jstack - 2] = i;
                        ir = j - 1;
                    }
                    else
                    {
                        istack[jstack - 1] = j - 1;
                        istack[jstack - 2] = l;
                        l = i;
                    }
                }
            }
            return 0;
        }

        #endregion

        #region Indexização de ranks

        //private int M = 7;
        //private int NSTACK = 500;
        //private int NSTACK = 50;

        /// <summary>Indexes an array arr[0..n-1], i.e., outputs the array indx[0..n-1] such that arr[indx[j-1]-1] is
        /// in ascending order for j = 1, 2, . . . ,N. The input quantities n and arr are not changed.</summary>
        /// <param name="n">Ulong variable with size of the vector to be sorted.</param>
        /// <param name="arr">Double vector with data to be sorted.</param>
        /// <param name="indx">Output ulong vector with indexes for sorting.</param>
        public void indexx(ulong n, object[,] arr, ulong[] indx)
        {
            ulong i, indxt, ir = n, itemp, j, k, l = 1;
            int jstack = 0;
            //double a;
            object a;
            ulong[] istack = new ulong[NSTACK];
            for (j = 1; j <= n; j++) indx[j - 1] = j;
            for (; ; )
            {
                if (ir - l < (ulong)M)
                {
                    for (j = l + 1; j <= ir; j++)
                    {
                        indxt = indx[j - 1];
                        a = arr[indxt - 1, 0];
                        for (i = j - 1; i >= 1; i--)
                        {
                            //if (arr[indx[i - 1] - 1] <= a) break;
                            if (this.LessOrEqualTo(arr[indx[i - 1] - 1, 0], a)) break;
                            indx[i] = indx[i - 1];
                        }
                        indx[i] = indxt;
                    }
                    if (jstack == 0) break;
                    ir = istack[jstack-- - 1];
                    l = istack[jstack-- - 1];
                }
                else
                {
                    k = (l + ir) >> 1;
                    itemp = indx[k - 1];
                    indx[k - 1] = indx[l];
                    indx[l] = itemp;
                    //if (arr[indx[l] - 1] > arr[indx[ir - 1] - 1])
                    if (this.GreaterThan(arr[indx[l] - 1,0], arr[indx[ir - 1] - 1,0]))
                    {
                        itemp = indx[l];
                        indx[l] = indx[ir - 1];
                        indx[ir - 1] = itemp;
                    }
                    //if (arr[indx[l - 1] - 1] > arr[indx[ir - 1] - 1])
                    if (this.GreaterThan(arr[indx[l - 1] - 1,0], arr[indx[ir - 1] - 1,0]))
                    {
                        itemp = indx[l - 1];
                        indx[l - 1] = indx[ir - 1];
                        indx[ir - 1] = itemp;
                    }
                    //if (arr[indx[l] - 1] > arr[indx[l - 1] - 1])
                    if (this.GreaterThan(arr[indx[l] - 1,0], arr[indx[l - 1] - 1,0]))
                    {
                        itemp = indx[l];
                        indx[l] = indx[l - 1];
                        indx[l - 1] = itemp;
                    }
                    i = l + 1;
                    j = ir;
                    indxt = indx[l - 1];
                    a = arr[indxt - 1,0];
                    for (; ; )
                    {
                        //do i++; while (arr[indx[i - 1] - 1] < a);
                        do i++; while (this.LessThan(arr[indx[i - 1] - 1,0], a));
                        //do j--; while (arr[indx[j - 1] - 1] > a);
                        do j--; while (this.GreaterThan(arr[indx[j - 1] - 1,0], a));
                        if (j < i) break;
                        itemp = indx[i - 1];
                        indx[i - 1] = indx[j - 1];
                        indx[j - 1] = itemp;
                    }
                    indx[l - 1] = indx[j - 1];
                    indx[j - 1] = indxt;
                    jstack += 2;
                    if (jstack > NSTACK) try { throw new Exception(); }
                        catch (Exception)
                        {
                            //MessageBox.Show("NSTACK too small in indexx.",
                            //	 "Invalid method",MessageBoxButtons.OK, MessageBoxIcon.Warning );
                        }
                    if (ir - i + 1 >= j - l)
                    {
                        istack[jstack - 1] = ir;
                        istack[jstack - 2] = i;
                        ir = j - 1;
                    }
                    else
                    {
                        istack[jstack - 1] = j - 1;
                        istack[jstack - 2] = l;
                        l = i;
                    }
                }
            }
        }

        #endregion 
    }

    #endregion 

    #region Classe para Gauss-Jordan elimination

    public class GaussJordan
    {
        /// <summary>
        /// Linear equation solution by Gauss-Jordan elimination, equation (2.1.1) above. The input matrix
        /// is a[0..n-1][0..n-1]. b[0..n-1][0..m-1] is input containing the m right-hand side vectors.
        /// On output, a is replaced by its matrix inverse, and b is replaced by the corresponding set of
        /// solution vectors.
        /// </summary>
        /// 

        //TODO: corrigi essa função (Caue)
        public void gaussj(ref double[,] a, ref double[,] b)
        {
            // Cria variáveis e matriz do tamanho da tabela usada pelo usuário
            double temp = 0.0;
            int i, icol = 0, irow = 0, j, k, l, ll, n = a.GetLength(0), m = b.GetLength(1);
            double big, dum, pivinv;
            int[] indxc = new int[n];
            int[] indxr = new int[n];
            int[] ipiv = new int[n];

            // Limpa vetor criado acima (insere 0s)
            for (j = 0; j < n; j++)
            { ipiv[j] = 0; }

            // Decomposicao da matriz  
            for (i = 0; i < n; i++)
            {
                big = 0.0;
                for (j = 0; j < n; j++)
                    if (ipiv[j] != 1)
                        for (k = 0; k < n; k++)
                        {
                            if (ipiv[k] == 0)
                            {
                                if (Math.Abs(a[j, k]) >= big)
                                {
                                    //big=Math.Abs(a[j,k]);
                                    big = Math.Abs(a[j, k]);
                                    irow = j;
                                    icol = k;
                                }
                            }
                        }
                ++(ipiv[icol]);
                if (irow != icol)
                {
                    for (l = 0; l < n; l++)
                    {
                        //SWAP(a[irow,l],a[icol,l]);
                        temp = a[irow, l];
                        a[irow, l] = a[icol, l];
                        a[icol, l] = temp;
                    }
                    for (l = 0; l < m; l++)
                    {
                        //SWAP(b[irow,l],b[icol,l]);
                        temp = b[irow, l];
                        b[irow, l] = b[icol, l];
                        b[icol, l] = temp;
                    }
                }
                indxr[i] = irow;
                indxc[i] = icol;

                // Calcula o determinante para descobrir se a matriz e singular ou nao
                IpeaMatrix.Matrix matriz_a = new IpeaMatrix.Matrix(a);
                
                if(a[icol,icol] == 0 && IpeaMatrix.Mfunc.Det(matriz_a) == 0)
                {
                   throw new Exception("Matriz singular na rotina de inversão."); 
                }
                
                // A inversa da matriz diagonal e 1/elementos da diagonal
                pivinv = 1.0 / a[icol, icol];
                a[icol, icol] = 1.0;
                for (l = 0; l < n; l++) 
                    a[icol, l] *= pivinv;
                for (l = 0; l < m; l++) 
                    b[icol, l] *= pivinv;
                for (ll = 0; ll < n; ll++)
                    if (ll != icol)
                    {
                        dum = a[ll, icol];
                        a[ll, icol] = 0.0;
                        for (l = 0; l < n; l++) 
                            a[ll, l] -= a[icol, l] * dum;
                        for (l = 0; l < m; l++) 
                            b[ll, l] -= b[icol, l] * dum;
                    }
            }
            for (l = n - 1; l >= 0; l--)
            {
                if (indxr[l] != indxc[l])
                    for (k = 0; k < n; k++)
                    {
                        //SWAP(a[k,indxr[l]],a[k,indxc[l]]);
                        temp = a[k, indxr[l]];
                        a[k, indxr[l]] = a[k, indxc[l]];
                        a[k, indxc[l]] = temp;
                    }
            }
        }

        /// <summary>
        /// Overloaded version with no right-hand sides. Replaces a by its inverse.
        /// </summary>
        public void gaussj(ref double[,] a)
        {
            double[,] b = new double[a.GetLength(0), 1];
            gaussj(ref a, ref b);
        }
    }

    #endregion 
    
    #region Classe para encontrar auto-valores e auto-vetores de matrizes simétricas

    public class Jacobi
    {
        int n;
        double[,] a;
        public double[,] v;
        public double[] d;
        int nrot;
        double EPS;

        private void eigsrt(ref double[] d, ref double[,] v)
        {
	        int k;
	        int n=d.GetLength(0);
	        for (int i=0;i<n-1;i++) {
		        double p=d[k=i];
		        for (int j=i;j<n;j++)
			        if (d[j] >= p) p=d[k=j];
		        if (k != i) {
			        d[k]=d[i];
			        d[i]=p;
			        if (v != null)
				        for (int j=0;j<n;j++) {
					        p=v[j,i];
					        v[j,i]=v[j,k];
					        v[j,k]=p;
				        }
		        }
	        }
        }

        public Jacobi(double[,] aa)
        {
            clsUtilTools clt = new clsUtilTools();

            n = aa.GetLength(0);
            a = clt.ArrayDoubleClone(aa);
            v = new double[n,n];
            d = new double[n];
            nrot = 0;
            EPS = 1.0e-6;

            int i,j,ip,iq;
		    double tresh,theta,tau,t,sm,s,h,g,c;
		    double[] b = new double[n];
            double[] z = new double[n];
		    for (ip=0;ip<n;ip++) {
			    for (iq=0;iq<n;iq++) v[ip,iq]=0.0;
			    v[ip,ip]=1.0;
		    }
		    for (ip=0;ip<n;ip++) {
			    b[ip]=d[ip]=a[ip,ip];
			    z[ip]=0.0;
		    }
		    for (i=1;i<=50;i++) {
			    sm=0.0;
			    for (ip=0;ip<n-1;ip++) {
				    for (iq=ip+1;iq<n;iq++)
					    sm += Math.Abs(a[ip,iq]);
			    }
			    if (sm == 0.0) {
				    eigsrt(ref d, ref v);
				    return;
			    }
			    if (i < 4)
				    tresh=0.2*sm/(n*n);
			    else
				    tresh=0.0;
			    for (ip=0;ip<n-1;ip++) {
				    for (iq=ip+1;iq<n;iq++) {
					    g=100.0*Math.Abs(a[ip,iq]);
					    if (i > 4 && g <= EPS*Math.Abs(d[ip]) && g <= EPS*Math.Abs(d[iq]))
							    a[ip,iq]=0.0;
					    else if (Math.Abs(a[ip,iq]) > tresh) {
						    h=d[iq]-d[ip];
						    if (g <= EPS*Math.Abs(h))
							    t=(a[ip,iq])/h;
						    else {
							    theta=0.5*h/(a[ip,iq]);
							    t=1.0/(Math.Abs(theta)+Math.Sqrt(1.0+theta*theta));
							    if (theta < 0.0) t = -t;
						    }
						    c=1.0/Math.Sqrt(1+t*t);
						    s=t*c;
						    tau=s/(1.0+c);
						    h=t*a[ip,iq];
						    z[ip] -= h;
						    z[iq] += h;
						    d[ip] -= h;
						    d[iq] += h;
						    a[ip,iq]=0.0;
						    for (j=0;j<ip;j++)
							    rot(ref a,s,tau,j,ip,j,iq);
						    for (j=ip+1;j<iq;j++)
							    rot(ref a,s,tau,ip,j,j,iq);
						    for (j=iq+1;j<n;j++)
							    rot(ref a,s,tau,ip,j,iq,j);
						    for (j=0;j<n;j++)
							    rot(ref v,s,tau,j,ip,j,iq);
						    ++nrot;
					    }
				    }
			    }
			    for (ip=0;ip<n;ip++) {
				    b[ip] += z[ip];
				    d[ip]=b[ip];
				    z[ip]=0.0;
			    }
		    }
		    throw new Exception("Too many iterations in routine jacobi");
        }

        private void rot(ref double[,] a, double s, double tau, int i, int j, int k, int l)
	    {
		    double g=a[i,j];
		    double h=a[k,l];
		    a[i,j]=g-s*(h+g*tau);
		    a[k,l]=h+s*(g-h*tau);
	    }
    }

    #endregion 

    #region Classe para encontrar a decomposição de Cholesky

    public class Cholesky
    {
        private int n;
        private double[,] el;

        /// <summary>
        /// Retorna a decomposição de Cholesky L da matriz de entrada A, onde A = LxLT. 
        /// </summary>
        public double[,] CholMatrix
        {
            get
            {
                clsUtilTools clt = new clsUtilTools();   
                return clt.ArrayDoubleClone(this.el); 
            }
        }

        /// <summary>
        /// Constructor. Given a positive-definite symmetric matrix a[0..n-1][0..n-1], construct
        /// and store its Cholesky decomposition, A = L x L' .
        /// </summary>
        /// <param name="a">Input matrix.</param>
        public Cholesky(ref double[,] a)
        {
            clsUtilTools clt = new clsUtilTools();
            //TODO: cholesky failed
            n = a.GetLength(0);
            el = clt.ArrayDoubleClone(a); 

            int i, j, k;
            double sum;
            if (el.GetLength(1) != n) throw new Exception("need square matrix");
            for (i = 0; i < n; i++)
            {
                for (j = i; j < n; j++)
                {
                    for (sum = el[i, j], k = i - 1; k >= 0; k--) sum -= el[i, k] * el[j, k];
                    if (i == j)
                    {
                        if (sum <= 0.0)
                            throw new Exception("Cholesky failed");
                        el[i, i] = Math.Sqrt(sum);
                    }
                    else el[j, i] = sum / el[i, i];
                }
            }
            for (i = 0; i < n; i++) for (j = 0; j < i; j++) el[j, i] = 0.0;
        }

        /// <summary>
        /// Solve the set of n linear equations Ax = b, where a is a positive-definite symmetric matrix
        /// whose Cholesky decomposition has been stored. b[0..n-1] is input as the right-hand side
        /// vector. The solution vector is returned in x[0..n-1].
        /// </summary>
        /// <param name="b">Right-hand-side coefficients.</param>
        /// <param name="x">Solution coefficients.</param>
        public void solve(ref double[,] b, ref double[,] x)
        {
            int i, k;
            double sum;
            if (b.GetLength(0) != n || x.GetLength(0) != n) throw new Exception("bad lengths in Cholesky");
            for (i = 0; i < n; i++)
            {
                for (sum = b[i,0], k = i - 1; k >= 0; k--) sum -= el[i, k] * x[k,0];
                x[i,0] = sum / el[i, i];
            }
            for (i = n - 1; i >= 0; i--)
            {
                for (sum = x[i,0], k = i + 1; k < n; k++) sum -= el[k, i] * x[k,0];
                x[i,0] = sum / el[i, i];
            }
        }

        public void elmult(ref double[,] y, ref double[,] b)
        {
            int i, j;
            if (b.GetLength(0) != n || y.GetLength(0) != n) throw new Exception("bad lengths");
            for (i = 0; i < n; i++)
            {
                b[i,0] = 0.0;
                for (j = 0; j <= i; j++) b[i, 0] += el[i, j] * y[j, 0];
            }
        }

        public void elsolve(ref double[,] b, ref double[,] y)
        {
            int i, j;
            double sum;
            if (b.GetLength(0) != n || y.GetLength(0) != n) throw new Exception("bad lengths");
            for (i = 0; i < n; i++)
            {
                for (sum = b[i,0], j = 0; j < i; j++) sum -= el[i, j] * y[j,0];
                y[i,0] = sum / el[i, i];
            }
        }

        public void inverse(ref double[,] ainv)
        {
            int i, j, k;
            double sum;
            
            //ainv.resize(n, n);
            ainv = new double[n, n];

            for (i = 0; i < n; i++) for (j = 0; j <= i; j++)
                {
                    sum = (i == j ? 1.0 : 0.0);
                    for (k = i - 1; k >= j; k--) sum -= el[i, k] * ainv[j, k];
                    ainv[j, i] = sum / el[i, i];
                }
            for (i = n - 1; i >= 0; i--) for (j = 0; j <= i; j++)
                {
                    sum = (i < j ? 0.0 : ainv[j, i]);
                    for (k = i + 1; k < n; k++) sum -= el[k, i] * ainv[j, k];
                    ainv[i, j] = ainv[j, i] = sum / el[i, i];
                }
        }

        public double logdet()
        {
            double sum = 0.0;
            for (int i = 0; i < n; i++) sum += Math.Log(el[i, i]);
            return 2.0 * sum;
        }
    }
    #endregion

    #region Classe para singular value decomposition

    public class clsSingularValueDecomposition
    {
        private int m, n;
        private double[,] u, v;
        private double[,] w;

        public double[,] UMatrix
        {
            get { return this.u; }
        }

        public double[,] VMatrix
        {
            get { return this.v; }
        }

        public double[,] WMatrix
        {
            get { return this.w; }
        }

        private double eps, tsh;

        private double thresh = 1.0e-6;
        public double Threshold
        {
            get { return this.thresh; }
            set
            {
                if (value > 0) this.thresh = value;
                else this.thresh = 1.0e-6;
            }
        }

        private double SIGN(double a, double b)
        {
            if (b < 0) return -Math.Abs(a);
            return Math.Abs(a);
        }

        public clsSingularValueDecomposition(ref double[,] a)
        {
            clsUtilTools clt = new clsUtilTools();

            m = a.GetLength(0);
            n = a.GetLength(1);
            u = clt.ArrayDoubleClone(a);
            v = new double[n, n];
            w = new double[n, 1];

            eps = 1.0e-8;

            decompose();
            reorder();
            tsh = 0.5 * Math.Sqrt((double)(m + n + 1.0)) * w[0, 0] * eps;
        }

        /// <summary>
        /// Solve A . x = b for a vector x using the pseudoinverse of A as obtained by SVD.
        /// </summary>
        public void solve(ref double[,] b, ref double[,] x)
        {
            int i, j, jj;
            double s;
            if (b.GetLength(0) != m || x.GetLength(0) != n) throw new Exception("SVD::solve bad sizes");
            double[,] tmp = new double[n, 1];
            tsh = (thresh >= 0.0 ? thresh : 0.5 * Math.Sqrt((double)(m + n + 1.0)) * w[0, 0] * eps);
            for (j = 0; j < n; j++)
            {
                s = 0.0;
                if (w[j, 0] > tsh)
                {
                    for (i = 0; i < m; i++) s += u[i, j] * b[i, 0];
                    s /= w[j, 0];
                }
                tmp[j, 0] = s;
            }
            for (j = 0; j < n; j++)
            {
                s = 0.0;
                for (jj = 0; jj < n; jj++) s += v[j, jj] * tmp[jj, 0];
                x[j, 0] = s;
            }
        }

        /// <summary>
        /// Solves m sets of n equations A.X = B using the pseudoinverse of A. The right-hand sides are
        /// input as b[0..n-1][0..m-1], while x[0..n-1][0..m-1] returns the solutions.
        /// </summary>
        public void solvemat(ref double[,] b, ref double[,] x)
        {
            int i, j, m = b.GetLength(1);
            if (b.GetLength(0) != n || x.GetLength(0) != n || b.GetLength(1) != x.GetLength(1))
                throw new Exception("SVD::solve bad sizes");

            double[,] xx = new double[n,1];
            for (j = 0; j < m; j++)
            {
                for (i = 0; i < n; i++) xx[i,0] = b[i, j];
                solve(ref xx, ref xx);
                for (i = 0; i < n; i++) x[i, j] = xx[i,0];
            }
        }

        public void decompose()
        {
            bool flag;
            int i, its, j, jj, k, l = 0, nm = 0;
            double anorm, c, f, g, h, s, scale, x, y, z;
            double[,] rv1 = new double[n,1];
            g = scale = anorm = 0.0;
            for (i = 0; i < n; i++)
            {
                l = i + 2;
                rv1[i,0] = scale * g;
                g = s = scale = 0.0;
                if (i < m)
                {
                    for (k = i; k < m; k++) scale += Math.Abs(u[k, i]);
                    if (scale != 0.0)
                    {
                        for (k = i; k < m; k++)
                        {
                            u[k, i] /= scale;
                            s += u[k, i] * u[k, i];
                        }
                        f = u[i, i];
                        g = -SIGN(Math.Sqrt(s), f);
                        h = f * g - s;
                        u[i, i] = f - g;
                        for (j = l - 1; j < n; j++)
                        {
                            for (s = 0.0, k = i; k < m; k++) s += u[k, i] * u[k, j];
                            f = s / h;
                            for (k = i; k < m; k++) u[k, j] += f * u[k, i];
                        }
                        for (k = i; k < m; k++) u[k, i] *= scale;
                    }
                }
                w[i, 0] = scale * g;
                g = s = scale = 0.0;
                if (i + 1 <= m && i + 1 != n)
                {
                    for (k = l - 1; k < n; k++) scale += Math.Abs(u[i, k]);
                    if (scale != 0.0)
                    {
                        for (k = l - 1; k < n; k++)
                        {
                            u[i, k] /= scale;
                            s += u[i, k] * u[i, k];
                        }
                        f = u[i, l - 1];
                        g = -SIGN(Math.Sqrt(s), f);
                        h = f * g - s;
                        u[i, l - 1] = f - g;
                        for (k = l - 1; k < n; k++) rv1[k, 0] = u[i, k] / h;
                        for (j = l - 1; j < m; j++)
                        {
                            for (s = 0.0, k = l - 1; k < n; k++) s += u[j, k] * u[i, k];
                            for (k = l - 1; k < n; k++) u[j, k] += s * rv1[k, 0];
                        }
                        for (k = l - 1; k < n; k++) u[i, k] *= scale;
                    }
                }
                anorm = Math.Max(anorm, (Math.Abs(w[i,0]) + Math.Abs(rv1[i,0])));
            }
            for (i = n - 1; i >= 0; i--)
            {
                if (i < n - 1)
                {
                    if (g != 0.0)
                    {
                        for (j = l; j < n; j++)
                            v[j, i] = (u[i, j] / u[i, l]) / g;
                        for (j = l; j < n; j++)
                        {
                            for (s = 0.0, k = l; k < n; k++) s += u[i, k] * v[k, j];
                            for (k = l; k < n; k++) v[k, j] += s * v[k, i];
                        }
                    }
                    for (j = l; j < n; j++) v[i, j] = v[j, i] = 0.0;
                }
                v[i, i] = 1.0;
                g = rv1[i,0];
                l = i;
            }
            for (i = Math.Min(m, n) - 1; i >= 0; i--)
            {
                l = i + 1;
                g = w[i,0];
                for (j = l; j < n; j++) u[i, j] = 0.0;
                if (g != 0.0)
                {
                    g = 1.0 / g;
                    for (j = l; j < n; j++)
                    {
                        for (s = 0.0, k = l; k < m; k++) s += u[k, i] * u[k, j];
                        f = (s / u[i, i]) * g;
                        for (k = i; k < m; k++) u[k, j] += f * u[k, i];
                    }
                    for (j = i; j < m; j++) u[j, i] *= g;
                }
                else for (j = i; j < m; j++) u[j, i] = 0.0;
                ++u[i, i];
            }
            for (k = n - 1; k >= 0; k--)
            {
                for (its = 0; its < 30; its++)
                {
                    flag = true;
                    for (l = k; l >= 0; l--)
                    {
                        nm = l - 1;
                        if (l == 0 || Math.Abs(rv1[l,0]) <= eps * anorm)
                        {
                            flag = false;
                            break;
                        }
                        if (Math.Abs(w[nm,0]) <= eps * anorm) break;
                    }
                    if (flag)
                    {
                        c = 0.0;
                        s = 1.0;
                        for (i = l; i < k + 1; i++)
                        {
                            f = s * rv1[i,0];
                            rv1[i,0] = c * rv1[i,0];
                            if (Math.Abs(f) <= eps * anorm) break;
                            g = w[i,0];
                            h = pythag(f, g);
                            w[i,0] = h;
                            h = 1.0 / h;
                            c = g * h;
                            s = -f * h;
                            for (j = 0; j < m; j++)
                            {
                                y = u[j, nm];
                                z = u[j, i];
                                u[j, nm] = y * c + z * s;
                                u[j, i] = z * c - y * s;
                            }
                        }
                    }
                    z = w[k,0];
                    if (l == k)
                    {
                        if (z < 0.0)
                        {
                            w[k,0] = -z;
                            for (j = 0; j < n; j++) v[j, k] = -v[j, k];
                        }
                        break;
                    }
                    if (its == 29) throw new Exception("no convergence in 30 svdcmp iterations");
                    x = w[l,0];
                    nm = k - 1;
                    y = w[nm,0];
                    g = rv1[nm,0];
                    h = rv1[k,0];
                    f = ((y - z) * (y + z) + (g - h) * (g + h)) / (2.0 * h * y);
                    g = pythag(f, 1.0);
                    f = ((x - z) * (x + z) + h * ((y / (f + SIGN(g, f))) - h)) / x;
                    c = s = 1.0;
                    for (j = l; j <= nm; j++)
                    {
                        i = j + 1;
                        g = rv1[i,0];
                        y = w[i,0];
                        h = s * g;
                        g = c * g;
                        z = pythag(f, h);
                        rv1[j,0] = z;
                        c = f / z;
                        s = h / z;
                        f = x * c + g * s;
                        g = g * c - x * s;
                        h = y * s;
                        y *= c;
                        for (jj = 0; jj < n; jj++)
                        {
                            x = v[jj, j];
                            z = v[jj, i];
                            v[jj, j] = x * c + z * s;
                            v[jj, i] = z * c - x * s;
                        }
                        z = pythag(f, h);
                        w[j,0] = z;
                        //if (z) {
                        if (z != 0.0)
                        {
                            z = 1.0 / z;
                            c = f * z;
                            s = h * z;
                        }
                        f = c * g + s * y;
                        x = c * y - s * g;
                        for (jj = 0; jj < m; jj++)
                        {
                            y = u[jj, j];
                            z = u[jj, i];
                            u[jj, j] = y * c + z * s;
                            u[jj, i] = z * c - y * s;
                        }
                    }
                    rv1[l,0] = 0.0;
                    rv1[k,0] = f;
                    w[k,0] = x;
                }
            }
        }

        private void reorder()
        {
            int i, j, k, s, inc = 1;
            double sw;
            double[,] su = new double[m,1];
            double[,] sv = new double[n,1];
            do { inc *= 3; inc++; } while (inc <= n);
            do
            {
                inc /= 3;
                for (i = inc; i < n; i++)
                {
                    sw = w[i,0];
                    for (k = 0; k < m; k++) su[k,0] = u[k, i];
                    for (k = 0; k < n; k++) sv[k,0] = v[k, i];
                    j = i;
                    while (w[j - inc, 0] < sw)
                    {
                        w[j, 0] = w[j - inc, 0];
                        for (k = 0; k < m; k++) u[k, j] = u[k, j - inc];
                        for (k = 0; k < n; k++) v[k, j] = v[k, j - inc];
                        j -= inc;
                        if (j < inc) break;
                    }
                    w[j, 0] = sw;
                    for (k = 0; k < m; k++) u[k, j] = su[k, 0];
                    for (k = 0; k < n; k++) v[k, j] = sv[k, 0];
                }
            } while (inc > 1);

            for (k = 0; k < n; k++)
            {
                s = 0;
                for (i = 0; i < m; i++) if (u[i, k] < 0.0) s++;
                for (j = 0; j < n; j++) if (v[j, k] < 0.0) s++;
                if (s > (m + n) / 2)
                {
                    for (i = 0; i < m; i++) u[i, k] = -u[i, k];
                    for (j = 0; j < n; j++) v[j, k] = -v[j, k];
                }
            }
        }

        private double pythag(double a, double b)
        {
            double absa = Math.Abs(a), absb = Math.Abs(b);
            return (absa > absb ? absa * Math.Sqrt(1.0 + SQR(absb / absa)) :
                (absb == 0.0 ? 0.0 : absb * Math.Sqrt(1.0 + SQR(absa / absb))));
        }

        private double SQR(double a)
        {
            return a * a;
        }
    }

    #endregion

    #region Classes para interpolação

    #region Base_interp

    public class Base_interp
    {
        public Base_interp(double[] x, double[] y, int m)
        {
            clsUtilTools clt = new clsUtilTools();
            n = x.GetLength(0);
            mm = m;
            jsav = 0;
            cor = 0;
            xx = x;
            yy = y;
            dj = Math.Min(1, Convert.ToInt32(Math.Pow((double)n, 0.25)));
        }

	    protected int n, mm, jsav, cor, dj;
	    protected double[] xx, yy;

	    public double interp(double x) 
        {
		    int jlo = cor != 0 ? hunt(x) : locate(x);
		    return rawinterp(jlo,x);
	    }

        protected int locate(double x)
        {
	        int ju,jm,jl;
	        if (n < 2 || mm < 2 || mm > n) throw new Exception("locate size error");
	        bool ascnd=(xx[n-1] >= xx[0]);
	        jl=0;
	        ju=n-1;
	        while (ju-jl > 1) 
            {
		        jm = (ju+jl) >> 1;
		        if (x >= xx[jm] == ascnd)
			        jl=jm;
		        else
			        ju=jm;
	        }
	        cor = Math.Abs(jl-jsav) > dj ? 0 : 1;
	        jsav = jl;
	        return Math.Max(0,Math.Min(n-mm,jl-((mm-2)>>1)));
        }

        protected virtual double rawinterp(int jlo, double x)
        {
            return 0.0;
        }

        protected int hunt(double x)
        {
	        int jl=jsav, jm, ju, inc=1;
	        if (n < 2 || mm < 2 || mm > n) throw new Exception("hunt size error");
	        bool ascnd=(xx[n-1] >= xx[0]);
	        if (jl < 0 || jl > n-1) {
		        jl=0;
		        ju=n-1;
	        } else {
		        if (x >= xx[jl] == ascnd) {
			        for (;;) {
				        ju = jl + inc;
				        if (ju >= n-1) { ju = n-1; break;}
				        else if (x < xx[ju] == ascnd) break;
				        else {
					        jl = ju;
					        inc += inc;
				        }
			        }
		        } else {
			        ju = jl;
			        for (;;) {
				        jl = jl - inc;
				        if (jl <= 0) { jl = 0; break;}
				        else if (x >= xx[jl] == ascnd) break;
				        else {
					        ju = jl;
					        inc += inc;
				        }
			        }
		        }
	        }
	        while (ju-jl > 1) {
		        jm = (ju+jl) >> 1;
		        if (x >= xx[jm] == ascnd)
			        jl=jm;
		        else
			        ju=jm;
	        }
	        cor = Math.Abs(jl-jsav) > dj ? 0 : 1;
	        jsav = jl;
	        return Math.Max(0,Math.Min(n-mm,jl-((mm-2)>>1)));
        }
    }

    #endregion

    #region Poly_interp

    public class Poly_interp : Base_interp
    {
	    private double dy;

	    public Poly_interp(double[] xv, double[] yv, int m)
		    : base(xv, yv, m)
        {
            dy = 0.0;
        }

        protected override double rawinterp(int jl, double x)
        {
            clsUtilTools clt = new clsUtilTools();
	        int i,m,ns=0;
	        double y,den,dif,dift,ho,hp,w;
	        double[] xa = clt.SubMatriz(xx, jl, xx.GetLength(0)-1); 
            double[] ya = clt.SubMatriz(yy, jl, yy.GetLength(0)-1); 
	        double[] c = new double[mm];
            double[] d = new double[mm];
	        dif=Math.Abs(x-xa[0]);
	        for (i=0;i<mm;i++) {
		        if ((dift=Math.Abs(x-xa[i])) < dif) {
			        ns=i;
			        dif=dift;
		        }
		        c[i]=ya[i];
		        d[i]=ya[i];
	        }
	        y=ya[ns--];
	        for (m=1;m<mm;m++) {
		        for (i=0;i<mm-m;i++) {
			        ho=xa[i]-x;
			        hp=xa[i+m]-x;
			        w=c[i+1]-d[i];
			        if ((den=ho-hp) == 0.0) throw new Exception("Poly_interp error");
			        den=w/den;
			        d[i]=hp*den;
			        c[i]=ho*den;
		        }
		        y += (dy=(2*(ns+1) < (mm-m) ? c[ns+1] : d[ns--]));
	        }
	        return y;
        }
    }

    #endregion

    #region Rat_interp

    public class Rat_interp : Base_interp
    {
	    private double dy;
	    public Rat_interp(double[] xv, double[] yv, int m)
		    : base(xv,yv,m)
        {
            dy = 0.0;
        }

        protected override double rawinterp(int jl, double x)
        {
            clsUtilTools clt = new clsUtilTools();
	        double TINY=1.0e-99;
	        int m,i,ns=0;
	        double y,w,t,hh,h,dd;

            double[] xa = clt.SubMatriz(xx, jl, xx.GetLength(0)-1); 
            double[] ya = clt.SubMatriz(yy, jl, yy.GetLength(0)-1); 
	        double[] c = new double[mm];
            double[] d = new double[mm];           
            
            hh=Math.Abs(x-xa[0]);
	        for (i=0;i<mm;i++) {
		        h=Math.Abs(x-xa[i]);
		        if (h == 0.0) {
			        dy=0.0;
			        return ya[i];
		        } else if (h < hh) {
			        ns=i;
			        hh=h;
		        }
		        c[i]=ya[i];
		        d[i]=ya[i]+TINY;
	        }
	        y=ya[ns--];
	        for (m=1;m<mm;m++) {
		        for (i=0;i<mm-m;i++) {
			        w=c[i+1]-d[i];
			        h=xa[i+m]-x;
			        t=(xa[i]-x)*d[i]/h;
			        dd=t-c[i+1];
			        if (dd == 0.0) throw new Exception("Error in routine ratint");
			        dd=w/dd;
			        d[i]=c[i+1]*dd;
			        c[i]=t*dd;
		        }
		        y += (dy=(2*(ns+1) < (mm-m) ? c[ns+1] : d[ns--]));
	        }
	        return y;
        }
    }

    #endregion

    #region  Spline_interp

    public class Spline_interp : Base_interp
    {
	    private double[] y2;
    	
	    public Spline_interp(double[] xv, double[] yv)
	        : base(xv, yv, 2) 
	    {
            double yp1=1.0e99;
            double ypn=1.0e99;
            y2 = new double[xv.GetLength(0)];

            sety2(xv, yv, yp1, ypn);
        }

        private void sety2(double[] xv, double[] yv, double yp1, double ypn)
        {
	        int i,k;
	        double p,qn,sig,un;
	        int n=y2.GetLength(0);
	        double[] u = new double[n-1];
	        if (yp1 > 0.99e99)
		        y2[0]=u[0]=0.0;
	        else {
		        y2[0] = -0.5;
		        u[0]=(3.0/(xv[1]-xv[0]))*((yv[1]-yv[0])/(xv[1]-xv[0])-yp1);
	        }
	        for (i=1;i<n-1;i++) {
		        sig=(xv[i]-xv[i-1])/(xv[i+1]-xv[i-1]);
		        p=sig*y2[i-1]+2.0;
		        y2[i]=(sig-1.0)/p;
		        u[i]=(yv[i+1]-yv[i])/(xv[i+1]-xv[i]) - (yv[i]-yv[i-1])/(xv[i]-xv[i-1]);
		        u[i]=(6.0*u[i]/(xv[i+1]-xv[i-1])-sig*u[i-1])/p;
	        }
	        if (ypn > 0.99e99)
		        qn=un=0.0;
	        else {
		        qn=0.5;
		        un=(3.0/(xv[n-1]-xv[n-2]))*(ypn-(yv[n-1]-yv[n-2])/(xv[n-1]-xv[n-2]));
	        }
	        y2[n-1]=(un-qn*u[n-2])/(qn*y2[n-2]+1.0);
	        for (k=n-2;k>=0;k--)
		        y2[k]=y2[k]*y2[k+1]+u[k];
        }

        protected override double rawinterp(int jl, double x)
        {
	        int klo=jl,khi=jl+1;
	        double y,h,b,a;
	        h=xx[khi]-xx[klo];
	        if (h == 0.0) throw new Exception("Bad input to routine spline");
	        a=(xx[khi]-x)/h;
	        b=(x-xx[klo])/h;
	        y=a*yy[klo]+b*yy[khi]+((a*a*a-a)*y2[klo]
		        +(b*b*b-b)*y2[khi])*(h*h)/6.0;
	        return y;
        }
    }

    #endregion

    #endregion

    #region Classes para a LU decomposition

    public class clsLUDecomposition
    {
        private int n;
        private double[,] lu; //Stores the decomposition.
        private int[] indx; //Stores the permutation.
        private double d; //Used by det.
        private double[,] aref;

        public clsLUDecomposition(double[,] a)
        {
            LUdcmp(ref a);
        }
        
        /// <summary>
        /// Função para decomposição LU
        /// </summary>
        private void LUdcmp(ref double[,] a)
        {
            aref = (double[,])a.Clone();
            n = a.GetLength(0);
            d = 0.0;
            lu = (double[,])a.Clone();
            indx = new int[n];

            double TINY = 1.0e-40;
            int i, imax = 0, j, k;
            double big, temp;
            double[] vv = new double[n];
            d = 1.0;
            for (i = 0; i < n; i++)
            {
                big = 0.0;
                for (j = 0; j < n; j++)
                    if ((temp = Math.Abs(lu[i, j])) > big) big = temp;
                if (big == 0.0) 
                    throw new Exception("Matriz singular na decomposição LU.");
                vv[i] = 1.0 / big;
            }
            for (k = 0; k < n; k++)
            {
                big = 0.0;
                for (i = k; i < n; i++)
                {
                    temp = vv[i] * Math.Abs(lu[i, k]);
                    if (temp > big)
                    {
                        big = temp;
                        imax = i;
                    }
                }
                if (k != imax)
                {
                    for (j = 0; j < n; j++)
                    {
                        temp = lu[imax, j];
                        lu[imax, j] = lu[k, j];
                        lu[k, j] = temp;
                    }
                    d = -d;
                    vv[imax] = vv[k];
                }
                indx[k] = imax;
                if (lu[k, k] == 0.0) lu[k, k] = TINY;
                for (i = k + 1; i < n; i++)
                {
                    temp = lu[i, k] /= lu[k, k];
                    for (j = k + 1; j < n; j++)
                        lu[i, j] -= temp * lu[k, j];
                }
            }
        }

        public void solve(ref double[,] b, ref double[,] x) 
        {
            int i,ii=0,ip,j;
            double sum;
            if (b.GetLength(0) != n || x.GetLength(0) != n)
                throw new Exception("LUdcmp::solve bad sizes");
            for (i=0;i<n;i++) x[i,0] = b[i,0];
            for (i=0;i<n;i++)
            {
                ip=indx[i];
                sum=x[ip,0];
                x[ip,0]=x[i,0];
                if (ii != 0)
                    for (j = ii - 1; j < i; j++) sum -= lu[i, j] * x[j, 0];
                else if (sum != 0.0)
                    ii = i + 1;
                x[i,0]=sum;
            }
            for (i=n-1;i>=0;i--)
            {
                sum=x[i,0];
                for (j=i+1;j<n;j++) sum -= lu[i,j]*x[j,0];
                x[i,0]=sum/lu[i,i];
            }
        }
    }

    #endregion

    #region Classe para Exportação para planilha Excel
    
    # region Summary

    /// <summary>
    /// Exports datatable to CSV or Excel format.
    /// This uses DataSet's XML features and XSLT for exporting.
    /// 
    /// C#.Net Example to be used in WebForms
    /// ------------------------------------- 
    /// using MyLib.ExportData;
    /// 
    /// private void btnExport_Click(object sender, System.EventArgs e)
    /// {
    ///   try
    ///   {
    ///     // Declarations
    ///     DataSet dsUsers =  ((DataSet) Session["dsUsers"]).Copy( );
    ///     MyLib.ExportData.Export oExport = new MyLib.ExportData.Export("Web"); 
    ///     string FileName = "UserList.csv";
    ///     int[] ColList = {2, 3, 4, 5, 6};
    ///     oExport.ExportDetails(dsUsers.Tables[0], ColList, Export.ExportFormat.CSV, FileName);
    ///   }
    ///   catch(Exception Ex)
    ///   {
    ///     lblError.Text = Ex.Message;
    ///   }
    /// }	
    ///  
    /// VB.Net Example to be used in WindowsForms
    /// ----------------------------------------- 
    /// Imports MyLib.ExportData
    /// 
    /// Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    /// 
    ///	  Try	
    ///	  
    ///     'Declarations
    /// 	Dim dsUsers As DataSet = (CType(Session("dsUsers"), DataSet)).Copy()
    /// 	Dim oExport As New MyLib.ExportData.Export("Win")
    /// 	Dim FileName As String = "C:\\UserList.xls"
    /// 	Dim ColList() As Integer = New Integer() {2, 3, 4, 5, 6}			
    ///     oExport.ExportDetails(dsUsers.Tables(0), ColList, Export.ExportFormat.CSV, FileName)	 
    ///     
    ///   Catch Ex As Exception
    /// 	lblError.Text = Ex.Message
    ///   End Try
    ///   
    /// End Sub
    ///     
    /// </summary>

    # endregion // Summary

    public class clsExportExcel
    {
        public enum ExportFormat : int { CSV = 1, Excel = 2 }; // Export format enumeration			
        //System.Web.HttpResponse response;
        private string appType;

        public clsExportExcel()
        {
            appType = "Web";
            //response = System.Web.HttpContext.Current.Response;
        }

        public clsExportExcel(string ApplicationType)
        {
            //appType = ApplicationType;
            //if (appType != "Web" && appType != "Win") throw new Exception("Provide valid application format (Web/Win)");
            //if (appType == "Web") response = System.Web.HttpContext.Current.Response;
        }

        #region ExportDetails OverLoad : Type#1

        // Function  : ExportDetails 
        // Arguments : DetailsTable, FormatType, FileName
        // Purpose	 : To get all the column headers in the datatable and 
        //			   exorts in CSV / Excel format with all columns

        public void ExportDetails(DataTable DetailsTable, ExportFormat FormatType, string FileName)
        {
            try
            {
                if (DetailsTable.Rows.Count == 0)
                    throw new Exception("There are no details to export.");

                // Create Dataset
                DataSet dsExport = new DataSet("Export");

                //DataTable dtExport = DetailsTable.Copy();

                clsUtilTools clt = new clsUtilTools();
                DataTable dtExport = clt.MudaSeparadorDecimais(DetailsTable.Copy());

                dtExport.TableName = "Values";
                dsExport.Tables.Add(dtExport);

                // Getting Field Names
                string[] sHeaders = new string[dtExport.Columns.Count];
                string[] sFileds = new string[dtExport.Columns.Count];

                for (int i = 0; i < dtExport.Columns.Count; i++)
                {
                    //sHeaders[i] = ReplaceSpclChars(dtExport.Columns[i].ColumnName);
                    sHeaders[i] = dtExport.Columns[i].ColumnName;
                    sFileds[i] = ReplaceSpclChars(dtExport.Columns[i].ColumnName);
                }

                //if (appType == "Web")
                    //Export_with_XSLT_Web(dsExport, sHeaders, sFileds, FormatType, FileName);
                //else if (appType == "Win")
                    Export_with_XSLT_Windows(dsExport, sHeaders, sFileds, FormatType, FileName);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion // ExportDetails OverLoad : Type#1

        #region ExportDetails OverLoad : Type#2

        // Function  : ExportDetails 
        // Arguments : DetailsTable, ColumnList, FormatType, FileName		
        // Purpose	 : To get the specified column headers in the datatable and
        //			   exorts in CSV / Excel format with specified columns

        public void ExportDetails(DataTable DetailsTable, int[] ColumnList, ExportFormat FormatType, string FileName)
        {
            try
            {
                if (DetailsTable.Rows.Count == 0)
                    throw new Exception("There are no details to export");

                // Create Dataset
                DataSet dsExport = new DataSet("Export");

                //DataTable dtExport = DetailsTable.Copy();

                clsUtilTools clt = new clsUtilTools();
                DataTable dtExport = clt.MudaSeparadorDecimais(DetailsTable.Copy());

                dtExport.TableName = "Values";
                dsExport.Tables.Add(dtExport);

                if (ColumnList.Length > dtExport.Columns.Count)
                    throw new Exception("ExportColumn List should not exceed Total Columns");

                // Getting Field Names
                string[] sHeaders = new string[ColumnList.Length];
                string[] sFileds = new string[ColumnList.Length];

                for (int i = 0; i < ColumnList.Length; i++)
                {
                    if ((ColumnList[i] < 0) || (ColumnList[i] >= dtExport.Columns.Count))
                        throw new Exception("ExportColumn Number should not exceed Total Columns Range");

                    sHeaders[i] = dtExport.Columns[ColumnList[i]].ColumnName;
                    sFileds[i] = ReplaceSpclChars(dtExport.Columns[ColumnList[i]].ColumnName);
                }

                //if (appType == "Web")
                    //Export_with_XSLT_Web(dsExport, sHeaders, sFileds, FormatType, FileName);
                //else if (appType == "Win")
                    Export_with_XSLT_Windows(dsExport, sHeaders, sFileds, FormatType, FileName);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion // ExportDetails OverLoad : Type#2

        #region ExportDetails OverLoad : Type#3

        // Function  : ExportDetails 
        // Arguments : DetailsTable, ColumnList, Headers, FormatType, FileName	
        // Purpose	 : To get the specified column headers in the datatable and	
        //			   exorts in CSV / Excel format with specified columns and 
        //			   with specified headers

        public void ExportDetails(DataTable DetailsTable, int[] ColumnList, string[] Headers, ExportFormat FormatType,
            string FileName)
        {
            try
            {
                if (DetailsTable.Rows.Count == 0)
                    throw new Exception("There are no details to export");

                // Create Dataset
                DataSet dsExport = new DataSet("Export");

                //DataTable dtExport = DetailsTable.Copy();

                clsUtilTools clt = new clsUtilTools();
                DataTable dtExport = clt.MudaSeparadorDecimais(DetailsTable.Copy());

                dtExport.TableName = "Values";
                dsExport.Tables.Add(dtExport);

                if (ColumnList.Length != Headers.Length)
                    throw new Exception("ExportColumn List and Headers List should be of same length");
                else if (ColumnList.Length > dtExport.Columns.Count || Headers.Length > dtExport.Columns.Count)
                    throw new Exception("ExportColumn List should not exceed Total Columns");

                // Getting Field Names
                string[] sFileds = new string[ColumnList.Length];

                for (int i = 0; i < ColumnList.Length; i++)
                {
                    if ((ColumnList[i] < 0) || (ColumnList[i] >= dtExport.Columns.Count))
                        throw new Exception("ExportColumn Number should not exceed Total Columns Range");

                    sFileds[i] = ReplaceSpclChars(dtExport.Columns[ColumnList[i]].ColumnName);
                }

                //if (appType == "Web")
                    //Export_with_XSLT_Web(dsExport, Headers, sFileds, FormatType, FileName);
                //else if (appType == "Win")
                    Export_with_XSLT_Windows(dsExport, Headers, sFileds, FormatType, FileName);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion // ExportDetails OverLoad : Type#3

        #region Export_with_XSLT_Web

        // Function  : Export_with_XSLT_Web 
        // Arguments : dsExport, sHeaders, sFileds, FormatType, FileName
        // Purpose   : Exports dataset into CSV / Excel format

        /*
        private void Export_with_XSLT_Web(DataSet dsExport, string[] sHeaders, string[] sFileds, ExportFormat FormatType, string FileName)
        {
            try
            {
                // Appending Headers
                response.Clear();
                response.Buffer = true;

                if (FormatType == ExportFormat.CSV)
                {
                    response.ContentType = "text/csv";
                    response.AppendHeader("content-disposition", "attachment; filename=" + FileName);
                }
                else
                {
                    response.ContentType = "application/vnd.ms-excel";
                    response.AppendHeader("content-disposition", "attachment; filename=" + FileName);
                }

                // XSLT to use for transforming this dataset.						
                MemoryStream stream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);

                CreateStylesheet(writer, sHeaders, sFileds, FormatType);
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                XmlDataDocument xmlDoc = new XmlDataDocument(dsExport);
                //dsExport.WriteXml("Data.xml");
                XslTransform xslTran = new XslTransform();
                xslTran.Load(new XmlTextReader(stream), null, null);

                System.IO.StringWriter sw = new System.IO.StringWriter();
                xslTran.Transform(xmlDoc, null, sw, null);
                //xslTran.Transform(System.Web.HttpContext.Current.Server.MapPath("Data.xml"), null, sw, null);

                //Writeout the Content				
                response.Write(sw.ToString());
                sw.Close();
                writer.Close();
                stream.Close();
                response.End();
            }
            catch (ThreadAbortException Ex)
            {
                string ErrMsg = Ex.Message;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        */

        #endregion // Export_with_XSLT

        #region Export_with_XSLT_Windows

        // Function  : Export_with_XSLT_Windows 
        // Arguments : dsExport, sHeaders, sFileds, FormatType, FileName
        // Purpose   : Exports dataset into CSV / Excel format

        private void Export_with_XSLT_Windows(DataSet dsExport, string[] sHeaders, string[] sFileds, ExportFormat FormatType, string FileName)
        {
            try
            {
                //DataSet ds_novo = new DataSet();
                //for (int i = 0; i < dsExport.Tables.Count; i++)
                //{
                //    ds_novo.Tables.Add(MudaSeparadorDecimais((DataTable)dsExport.Tables[i]));
                //}

                // XSLT to use for transforming this dataset.						
                MemoryStream stream = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);

                CreateStylesheet(writer, sHeaders, sFileds, FormatType);
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                XmlDataDocument xmlDoc = new XmlDataDocument(dsExport);
                //XmlDataDocument xmlDoc = new XmlDataDocument(ds_novo);

                XslTransform xslTran = new XslTransform();
                xslTran.Load(new XmlTextReader(stream), null, null);

                System.IO.StringWriter sw = new System.IO.StringWriter();
                xslTran.Transform(xmlDoc, null, sw, null);

                //Writeout the Content									
                StreamWriter strwriter = new StreamWriter(FileName);
                strwriter.WriteLine(sw.ToString());
                strwriter.Close();

                sw.Close();
                writer.Close();
                stream.Close();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion // Export_with_XSLT

        #region CreateStylesheet

        // Function  : WriteStylesheet 
        // Arguments : writer, sHeaders, sFileds, FormatType
        // Purpose   : Creates XSLT file to apply on dataset's XML file 

        private void CreateStylesheet(XmlTextWriter writer, string[] sHeaders, string[] sFileds, ExportFormat FormatType)
        {
            try
            {
                // xsl:stylesheet
                string ns = "http://www.w3.org/1999/XSL/Transform";
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("xsl", "stylesheet", ns);
                writer.WriteAttributeString("version", "1.0");
                writer.WriteStartElement("xsl:output");
                writer.WriteAttributeString("method", "text");
                writer.WriteAttributeString("version", "4.0");
                writer.WriteEndElement();

                // xsl-template
                writer.WriteStartElement("xsl:template");
                writer.WriteAttributeString("match", "/");

                // xsl:value-of for headers
                for (int i = 0; i < sHeaders.Length; i++)
                {
                    writer.WriteString(";");
                    writer.WriteStartElement("xsl:value-of");
                    writer.WriteAttributeString("select", "'" + sHeaders[i] + "'");
                    writer.WriteEndElement(); // xsl:value-of
                    //writer.WriteString(";");
                    //if (i != sFileds.Length - 1) writer.WriteString((FormatType == ExportFormat.Excel) ? ";" : "	");
                }

                // xsl:for-each
                writer.WriteStartElement("xsl:for-each");
                writer.WriteAttributeString("select", "Export/Values");
                writer.WriteString("\r\n");

                // xsl:value-of for data fields
                for (int i = 0; i < sFileds.Length; i++)
                {
                    writer.WriteString(";");
                    writer.WriteStartElement("xsl:value-of");
                    writer.WriteAttributeString("select", sFileds[i]);
                    writer.WriteEndElement(); // xsl:value-of
                    //writer.WriteString(";");
                    //if (i != sFileds.Length - 1) writer.WriteString((FormatType == ExportFormat.Excel) ? ";" : "	");
                }

                writer.WriteEndElement(); // xsl:for-each
                writer.WriteEndElement(); // xsl-template
                writer.WriteEndElement(); // xsl:stylesheet
                writer.WriteEndDocument();

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        #endregion // WriteStylesheet

        #region ReplaceSpclChars

        // Function  : ReplaceSpclChars 
        // Arguments : fieldName
        // Purpose   : Replaces special characters with XML codes 

        private string ReplaceSpclChars(string fieldName)
        {
            //			space 	-> 	_x0020_
            //			%		-> 	_x0025_
            //			#		->	_x0023_
            //			&		->	_x0026_
            //			/		->	_x002F_

            fieldName = fieldName.Replace(" ", "_x0020_");
            fieldName = fieldName.Replace("%", "_x0025_");
            fieldName = fieldName.Replace("#", "_x0023_");
            fieldName = fieldName.Replace("&", "_x0026_");
            fieldName = fieldName.Replace("/", "_x002F_");
            return fieldName;
        }

        #endregion // ReplaceSpclChars
    }

    #endregion
}

