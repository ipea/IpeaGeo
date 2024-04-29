using System;
using System.Data;
using System.Linq;
using IpeaGeo.RegressoesEspaciais;
using MathNet.Numerics.Distributions;

namespace IpeaGeo.Modelagem
{
    #region enumerações

    public enum TipoDistribuicao : int
    {
        Normal = 1,
        Uniform = 2,
        Exponencial = 3,
        Gamma = 4,
        Beta = 5,
        Poisson = 6
    };

    public enum TipoTeste : int
    {
        Amostras_Independentes = 1,
        Amostras_Dependentes = 2
    };

    #endregion

    public class BLogicNonParametricTests : BLogicBaseModelagem
    {
        #region variaveis internas

        #endregion

        public BLogicNonParametricTests()
        {
        }

        #region variaveis da classe

        private static TipoDistribuicao m_tipo_distribuicao;// = TipoDistribuicao.Normal;
        public TipoDistribuicao TipoDistribuicao
        {
            get { return m_tipo_distribuicao; }
            set { m_tipo_distribuicao = value; }
        }

        //private TipoTeste m_tipo_teste = TipoTeste.Amostras_Independentes;
        //public TipoTeste TipoTeste
        //{
        //    get { return m_tipo_teste; }
        //    set { m_tipo_teste = value; }
        //}

        private DataTable m_dt_tabela_dados = new DataTable();
        public DataTable TabelaDados
        {
            get { return this.m_dt_tabela_dados; }
            set { m_dt_tabela_dados = value; }
        }


        #endregion

        #region QQ-plot

        public void QQ_plot_1Variavel(double[,] dados, out double[,] sorted_dados, out double[,] inv_cdf)
        {
            clsUtilTools clt = new clsUtilTools();

            double[,] sdados = clt.SortcDoubleArray(dados);

            int n = sdados.GetLength(0);

            double[,] emp_dist = new double[n, 1];
            for (int i = 0; i < n; i++)
            {
                emp_dist[i, 0] = ((double)i + 0.5) / (double)n;
            }

            inv_cdf = this.cdf_inversa(emp_dist, sdados);
            sorted_dados = sdados;
        }

        #endregion

        #region PP-plot

        public void PP_plot_1Variavel(double[,] dados, out double[,] cdf_empirica, out double[,] cdf_teorica)
        {
            clsUtilTools clt = new clsUtilTools();

            double[,] sdados = clt.SortcDoubleArray(dados);

            int n = sdados.GetLength(0);

            double[,] emp_dist = new double[n, 1];
            for (int i = 0; i < n; i++)
            {
                emp_dist[i, 0] = ((double)i) / (double)n;
            }

            cdf_teorica = this.cdf_distribuicao(sdados);
            cdf_empirica = emp_dist;
        }

        #endregion

        #region Teste Kolmogorv Smirnov

        public void KS_Test(DataTable dt_dados, string[] variaveis, out double[] test_stat, out double[] p_valor)
        {
            clsUtilTools clt = new clsUtilTools();

            double[,] dados = new double[0, 0];
            int num_obs_total;
            int num_obs_missing;
            int num_obs_validas;
            int[] rows_obs_validas;
            int[] indicador_obs_validas;

            clt.DatatableToDoubleMatrixSemMissing(dt_dados, variaveis, out dados, out num_obs_total, out num_obs_missing, out num_obs_validas, out rows_obs_validas, out indicador_obs_validas);

            this.KS_Test(dados, out test_stat, out p_valor);
        }

        /// <summary>
        /// Função para o teste KS bi-cauldal para várias variáveis (colunas de uma matriz). 
        /// </summary>
        /// <param name="dados">Matriz contendo uma variável por coluna.</param>
        /// <param name="test_stat">Vetor de estatística testes.</param>
        /// <param name="p_valor">Vetor de p-valores.</param>
        public void KS_Test(double[,] dados, out double[] test_stat, out double[] p_valor)
        {
            clsUtilTools clt = new clsUtilTools();
            int ncols = dados.GetLength(1);
            test_stat = new double[ncols];
            p_valor = new double[ncols];
            double[,] temp_dados;
            double temp_tstat = 0.0;
            double temp_pvalor = 0.0;
            for (int j = 0; j < ncols; j++)
            {
                temp_dados = clt.SubColumnArrayDouble(dados, j);
                KS_Test_1Variavel(temp_dados, out temp_tstat, out temp_pvalor);
                test_stat[j] = temp_tstat;
                p_valor[j] = temp_pvalor;
            }
        }
       
        public void KS_Test2(double[,] dados, double[,] dados1, out double[,] test_stat2, out double[,] p_valor2, out double[,] parametro1, out double[,] parametro2)
        {
            clsUtilTools clt = new clsUtilTools();
            int ncols = dados.GetLength(1);
            double[,] temp_dados;
            double temp_tstat = 0.0;
            double temp_pvalor = 0.0;
            double[,] tabclasse = new double[0, 0];
            clt.FrequencyTable(ref tabclasse, dados1);

            test_stat2 = new double[tabclasse.GetLength(0), (ncols + 1)];
            p_valor2 = new double[tabclasse.GetLength(0), (ncols + 1)];
            parametro1 = new double[tabclasse.GetLength(0), (ncols)];
            parametro2 = new double[tabclasse.GetLength(0), (ncols)];
            double[,] cdf = this.cdf_distribuicao(dados);

            // NÍVEIS DA VARIAVEL CATEGORICA

            for (int j = 0; j < ncols; j++)
            {
                temp_dados = clt.SubColumnArrayDouble(dados, j);
                for (int h = 0; h < tabclasse.GetLength(0); h++)
                {

                    int count = 0;
                    double[,] dados_sep = new double[(int)tabclasse[h, 1], 1];
                    test_stat2[h, 0] = tabclasse[h, 0];
                    p_valor2[h, 0] = tabclasse[h, 0];

                    //DIVIDIR OS DADOS POR CATEGORIA
                    for (int i = 0; i < dados.GetLength(0); i++)
                    {
                        if (dados1[i, 0] == tabclasse[h, 0])
                        {
                            dados_sep[count, 0] = temp_dados[i, 0];
                            count += 1;
                        }
                    }

                    FormNonParametric teste = new FormNonParametric();
                    if (teste.tipodist == "Uniforme")
                    {
                        parametro1[h, j] = clt.Min(clt.SubColumnArrayDouble(dados_sep, 0));
                        parametro2[h, j] = clt.Max(clt.SubColumnArrayDouble(dados_sep, 0));
                    }
                    else
                    {
                        parametro1[h, j] = clt.Mean(clt.SubColumnArrayDouble(dados_sep, 0));
                        parametro2[h, j] = (clt.Despadc(clt.SubColumnArrayDouble(dados_sep, 0))[0, 0]) * Math.Sqrt((dados_sep.GetLength(0)) / (dados_sep.GetLength(0) - 1.0));
                    }                   

                    KS_Test_1Variavel(dados_sep, out temp_tstat, out temp_pvalor);
                    test_stat2[h, (j + 1)] = temp_tstat;
                    p_valor2[h, (j + 1)] = temp_pvalor;
                }
            }
        }

        /// <summary>
        /// Função para o teste KS bi-caudal de uma variável apenas (matriz coluna). 
        /// </summary>
        /// <param name="dados">Matriz coluna com a variável a ser testada.</param>
        /// <param name="test_stat">Retorna a estatística teste.</param>
        /// <param name="p_valor">Retorna o pvalor.</param>
        public void KS_Test_1Variavel(double[,] dados_in, out double test_stat, out double p_valor)
        {
            test_stat = 0.0;
            p_valor = 0.0;

            clsUtilTools clt = new clsUtilTools();

            double[,] dados1 = clt.SubColumnArrayDouble(dados_in, 0);
            double[,] dados = new double[0, 0];
            clt.DoubleToDoubleSemMissing(dados1, out dados);

            double[,] sdados = clt.SortcDoubleArray(dados);

            int n = sdados.GetLength(0);

            double[,] emp_dist = new double[n, 1];
            for (int i = 0; i < n; i++)
            {
                emp_dist[i, 0] = ((double)i + 1) / (double)n;
            }

            double[,] cdf = this.cdf_distribuicao(sdados);
            double max_dif = cdf[0, 0] - emp_dist[0, 0];
            double new_dif = 0.0;

            for (int i = 1; i < n; i++)
            {
                new_dif = Math.Max(Math.Abs(cdf[i, 0] - emp_dist[i, 0]), Math.Abs(cdf[i, 0] - emp_dist[i - 1, 0]));
                if (new_dif > max_dif)
                {
                    max_dif = new_dif;
                }
            }

            test_stat = max_dif;

            MathKSdist_N ksd = new MathKSdist_N();
            ksd.ks(test_stat, n, out p_valor);
        }

        #endregion

        #region Cdf inversa

        /// <summary>
        /// 
        /// </summary>
        /// <param name="probs"></param>
        /// <param name="dados"></param>
        /// <returns></returns>
        private double[,] cdf_inversa(double[,] probs, double[,] dados)
        {
            double[,] invcdf = new double[dados.GetLength(0), 1];
            clsUtilTools clt1 = new clsUtilTools();
            double xbarra = clt1.Mean(dados);
            double desvio = clt1.Despadc(dados)[0, 0];
            Normal normdist = new Normal(xbarra, desvio);

            switch (m_tipo_distribuicao)
            {
                case Modelagem.TipoDistribuicao.Normal:
                    for (int i = 0; i < dados.GetLength(0); i++)
                    {
                        invcdf[i, 0] = normdist.InverseCumulativeDistribution(probs[i, 0]);
                    }
                    break;
                case Modelagem.TipoDistribuicao.Exponencial:
                    Exponential expdist = new Exponential(1.0 / xbarra);
                    for (int i = 0; i < dados.GetLength(0); i++)
                    {
                        invcdf[i, 0] = expdist.InverseCumulativeDistribution(probs[i, 0]);
                    }
                    break;
                case Modelagem.TipoDistribuicao.Uniform:
                    double max1 = clt1.Max(dados);
                    double min1 = clt1.Min(dados);
                    double amp1 = max1 - min1;
                    for (int i = 0; i < dados.GetLength(0); i++)
                    {
                        invcdf[i, 0] = amp1 * probs[i, 0] + min1;
                    }
                    break;
                default:
                    for (int i = 0; i < dados.GetLength(0); i++)
                    {
                        invcdf[i, 0] = normdist.InverseCumulativeDistribution(probs[i, 0]);
                        
                    }
                    break;
            }

            return invcdf;
        }

        #endregion

        #region Cdf distribution

        private double[,] cdf_distribuicao(double[,] dados)
        {
            double[,] cdf = new double[dados.GetLength(0), 1];
            clsUtilTools clt1 = new clsUtilTools();
            double xbarra = clt1.Mean(dados);
            double desvio = (clt1.Despadc(dados)[0, 0]) * Math.Sqrt(((double)dados.GetLength(0)) / ((double)dados.GetLength(0) - 1.0));
            Normal normdist = new Normal(xbarra, desvio);

            switch (m_tipo_distribuicao)
            {
                case Modelagem.TipoDistribuicao.Normal:
                    for (int i = 0; i < dados.GetLength(0); i++)
                    {
                        cdf[i, 0] = normdist.CumulativeDistribution(dados[i, 0]);
                    }
                    break;
                case Modelagem.TipoDistribuicao.Exponencial:
                    Exponential expdist = new Exponential(1.0 / xbarra);
                    for (int i = 0; i < dados.GetLength(0); i++)
                    {
                        cdf[i, 0] = expdist.CumulativeDistribution(dados[i, 0]);
                    }
                    break;
                case Modelagem.TipoDistribuicao.Uniform:
                    double max1 = clt1.Max(dados);
                    double min1 = clt1.Min(dados);
                    double amp1 = max1 - min1;
                    for (int i = 0; i < dados.GetLength(0); i++)
                    {
                        cdf[i, 0] = (dados[i, 0] - min1) / amp1;
                    }
                    break;
                case Modelagem.TipoDistribuicao.Poisson:
                    MathPoissondist poisdist = new MathPoissondist(xbarra);
                    double media = clt1.Mean(dados);
                    for (int i = 0; i < dados.GetLength(0); i++)
                    {
                        cdf[i, 0] = poisdist.cdf((int)dados[i, 0]);
                    }
                    break;
                default:
                    for (int i = 0; i < dados.GetLength(0); i++)
                    {
                        cdf[i, 0] = normdist.CumulativeDistribution(dados[i, 0]);
                    }
                    break;
            }

            return cdf;
        }

        #endregion


        #region Teste Quiquadrado Tabelas
        //TODO: Teste quiquadrado para tabelas de contigência - Independencia e Homogeneidade
        public void Teste_QQuadrado(double[,] dados_in, out double test_stat, out double p_valor)
        {
            test_stat = 0.0;
            p_valor = 0.0;

            clsUtilTools clt = new clsUtilTools();

            double[,] dados = clt.SubColumnArrayDouble(dados_in, 0);

            double[,] sdados = clt.SortcDoubleArray(dados);

            int n = sdados.GetLength(0);

            double[,] emp_dist = new double[n, 1];
            for (int i = 0; i < n; i++)
            {
                emp_dist[i, 0] = ((double)i + 1) / (double)n;
            }

            double[,] cdf = this.cdf_distribuicao(sdados);
            double max_dif = cdf[0, 0] - emp_dist[0, 0];
            double new_dif = 0.0;

            for (int i = 1; i < n; i++)
            {
                new_dif = Math.Max(cdf[i, 0] - emp_dist[i, 0], cdf[i, 0] - emp_dist[i - 1, 0]);
                if (new_dif > max_dif)
                {
                    max_dif = new_dif;
                }
            }

            test_stat = max_dif;

            MathKSdist ksd = new MathKSdist();
            p_valor = ksd.invqks(test_stat);
        }

        #endregion

        #region Teste Quiquadrado  Ajuste

        //TODO: Teste quiquadrado de ajuste para outras distribuições

        /// <summary>
        /// Teste de Qui-Quadrado de ajuste.
        /// </summary>
        /// <param name="dados">Tabela com os dados</param>
        /// <param name="test_stat">Estatística do Teste</param>
        /// <param name="p_valor">P-valor do teste</param>
        /// <param name="tabela">Tabela de saída de frequencia</param>
        public void Teste_AjusteQQuadrado(double[,] dados, out double test_stat, out double p_valor, out double[,] tabela, out double par1, out  double par2, out double verificacao, out object[,] tabelapoisson)
        {
            clsUtilTools clt1 = new clsUtilTools();
            tabela = new double[10, 3];
            test_stat = 0.0;
            p_valor = 0.0;
            par1 = 0.0;
            par2 = 0.0;
            verificacao = 0.0;
            tabelapoisson = new object[0, 0];

            double[,] sdados = clt1.SortcDoubleArray(dados);
            int n = sdados.GetLength(0);
            double[,] emp_dist = new double[n, 1];
            double[,] freq = new double[0, 0];

            if (m_tipo_distribuicao == Modelagem.TipoDistribuicao.Poisson)
            {
                ////test_stat = 0.0;
                ////p_valor = 0.0;
                ////m_tipo_distribuicao = TipoDistribuicao.Poisson;

                bool adicionazero = false;
                if (clt1.Min(dados) > 0)
                {
                    adicionazero = true;
                }

                //double[,] freq = new double[0, 0];

                par1 = clt1.Mean(dados);

                Poisson dist_prob = new Poisson(par1);

                clt1.FrequencyTable(ref freq, dados);

                int minimo = (int)clt1.Min(clt1.SubColumnArrayDouble(freq, 0));
                int maximo = (int)clt1.Max(clt1.SubColumnArrayDouble(freq, 0));
                int faltante = Math.Max(maximo - minimo + 1 - freq.GetLength(0), 0);

                int lag0 = 0;
                double[,] freqajustado = new double[freq.GetLength(0) + faltante, 2];

                for (int i = 0; i < freqajustado.GetLength(0); i++)
                {
                    if (i + minimo == freq[i + lag0, 0])
                    {
                        freqajustado[i, 0] = freq[i + lag0, 0];
                        freqajustado[i, 1] = freq[i + lag0, 1];
                    }
                    else
                    {
                        freqajustado[i, 0] = i + minimo;
                        freqajustado[i, 1] = 0;
                        lag0 -= 1;
                    }
                }

                #region Tabela das probabilidades esperadas e observadas

                double[,] freq1 = new double[0, 0];

                int casela = 0;

                double[,] dfp = new double[0, 0];

                if (adicionazero)
                {
                    freq1 = new double[freqajustado.GetLength(0) + 2, 2];
                    freq1[0, 0] = 0;
                    freq1[0, 1] = 0;
                    freq1[freqajustado.GetLength(0) + 1, 0] = clt1.Max(clt1.SubColumnArrayDouble(freqajustado, 0)) + 1;
                    freq1[freqajustado.GetLength(0) + 1, 1] = 0;

                    for (int i = 0; i < freqajustado.GetLength(0); i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            freq1[i + 1, j] = freqajustado[i, j];
                        }
                    }

                    dfp = new double[freq1.GetLength(0), 1];

                    dfp[0, 0] = n * dist_prob.CumulativeDistribution((int)clt1.Min(clt1.SubColumnArrayDouble(freqajustado, 0)));
                    dfp[dfp.GetLength(0) - 1, 0] = n * (1 - dist_prob.CumulativeDistribution((int)clt1.Max(clt1.SubColumnArrayDouble(freqajustado, 0)) + 1));
                    for (int i = 1; i < dfp.GetLength(0) - 1; i++)
                    {
                        int valor = (int)freq1[i, 0];
                        dfp[i, 0] = dist_prob.Probability(valor) * n;

                    }

                    for (int i = 0; i < dfp.GetLength(0); i++)
                    {
                        if (dfp[i, 0] <= 5)
                        {
                            casela = casela + 1;
                        }
                    }
                }
                else
                {
                    freq1 = new double[freqajustado.GetLength(0) + 1, 2];
                    freq1[freqajustado.GetLength(0), 0] = clt1.Max(clt1.SubColumnArrayDouble(freqajustado, 0)) + 1;
                    freq1[freqajustado.GetLength(0), 1] = 0;

                    for (int i = 0; i < freqajustado.GetLength(0); i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            freq1[i, j] = freqajustado[i, j];
                        }
                    }
                    
                    dfp = new double[freq1.GetLength(0), 1];
                    dfp[dfp.GetLength(0) - 1, 0] = n * (1 - dist_prob.CumulativeDistribution((int)clt1.Max(clt1.SubColumnArrayDouble(freqajustado, 0)) + 1));

                    for (int i = 0; i < dfp.GetLength(0) - 1; i++)
                    {
                        int valor = (int)freq1[i, 0];
                        dfp[i, 0] = dist_prob.Probability(valor) * n;

                    }

                    for (int i = 0; i < dfp.GetLength(0); i++)
                    {
                        if (dfp[i, 0] <= 5)
                        {
                            casela = casela + 1;
                        }
                    }
                }

                /// Adequa o vetor de frequencia para diminuir a quantidade de caselas com valores esperados menores que 5
                #region Adequa o vetor
                int tam = Math.Max((int)(Math.Truncate(casela - 0.2 * dfp.GetLength(0)) + 1), 0);
                double[,] freq2 = new double[freq1.GetLength(0), 3];
                object[,] freq3 = new object[freq1.GetLength(0) - tam, 3];
                int n2 = freq1.GetLength(0);
                string[,] label = new string[n2, 1];
                
                for (int i = 0; i < freq1.GetLength(0); i++)
                {
                    label[i, 0] = "" + (int)freq1[i, 0];
                    freq2[i, 1] = freq1[i, 1];
                    freq2[i, 2] = dfp[i, 0];
                }

                if (adicionazero)
                {
                    label[0, 0] = "...";
                    label[n2 - 1, 0] = "...";
                }
                else
                {
                    label[n2 - 1, 0] = "...";
                }
                int indice = 0;
                int cont = n2;
                int cont2 = 0;
                int lag = 0;
                if (tam > 0)
                {
                    for (int i = 0; i < freq1.GetLength(0); i++)
                    {
                        indice = n2 * ((i + 1) % 2) + (int)((Math.Pow(-1, (i + 1) % 2) * i + (i % 2)) * (0.5)) - 1; // gera uma sequencia n,1,n-1,2,n-3,3,n-4,4 ... por meio do 0,1,2,3,4,5,6 

                        if (freq2[indice, 2] < 5 & cont2 < tam)
                        {
                            cont2 += 1;
                            freq2[indice + (int)Math.Pow(-1, ((i + 1) % 2)), 1] += freq2[indice, 1];
                            freq2[indice + (int)Math.Pow(-1, ((i + 1) % 2)), 2] += freq2[indice, 2];
                            if ((int)Math.Pow(-1, ((i + 1) % 2)) == 1)
                            {
                                label[indice + (int)Math.Pow(-1, ((i + 1) % 2)), 0] = label[indice, 0] + "," + label[indice + (int)Math.Pow(-1, ((i + 1) % 2)), 0];
                            }
                            else
                            {
                                label[indice + (int)Math.Pow(-1, ((i + 1) % 2)), 0] += "," + label[indice, 0];
                            }
                            string a1 = label[indice + (int)Math.Pow(-1, ((i + 1) % 2)), 0];
                            freq2[indice, 1] = -1;
                            freq2[indice, 2] = -1;
                            label[indice, 0] = "NULL";
                        }
                    }
                }

                freq3 = new object[freq1.GetLength(0) - cont2, 3];
                for (int i = 0; i < freq2.GetLength(0); i++)
                {
                    if (freq2[i, 2] == -1)
                    {
                        lag += 1;
                    }
                    else
                    {
                        freq3[i - lag, 0] = label[i, 0];
                        freq3[i - lag, 1] = freq2[i, 1];
                        freq3[i - lag, 2] = freq2[i, 2];
                    }
                }

                for (int i = 0; i < freq3.GetLength(0); i++)
                {
                    freq3[i, 0] = "{" + freq3[i, 0] + "}";
                }
                
                #endregion

                #endregion

                for (int i = 0; i < freq3.GetLength(0); i++)
                {
                    test_stat = test_stat + Math.Pow((double)freq3[i, 1] - (double)freq3[i, 2], 2) / (double)freq3[i, 2];
                }

                ChiSquared qq = new ChiSquared(freq3.GetLength(0) - 1 - 1);
                p_valor = qq.Density(test_stat);

                tabelapoisson = clt1.ArrayObjectClone(freq3);
            }
            
            #region Outras distribuições
            
            else
            {
                for (int i = 10; i < (tabela.GetLength(0) * 10) + 1; i += 10)
                {
                    tabela[(i / 10) - 1, 0] = clt1.Percentil(dados, i);
                }

                int lag2 = 0;
                int contadora = 0;
                for (int i = 0; i < sdados.GetLength(0); i++)
                {
                    if (sdados[i, 0] <= tabela[lag2, 0]) contadora++;
                    else
                    {
                        tabela[lag2, 1] = contadora;
                        lag2++;
                        contadora = 0;
                        i -= 1;
                    }

                    if (i == (sdados.GetLength(0) - 1))
                    {
                        tabela[lag2, 1] = contadora;
                        break;
                    }
                }

                switch (m_tipo_distribuicao)
                {
                    case Modelagem.TipoDistribuicao.Exponencial:
                        par1 = clt1.Mean(dados);
                        Exponential exp = new Exponential(1.0 / par1);
                        for (int i = 0; i < tabela.GetLength(0); i++)
                        {
                            tabela[i, 2] = exp.CumulativeDistribution(tabela[i, 0]);
                        }

                        break;

                    case Modelagem.TipoDistribuicao.Uniform:
                        par1 = clt1.Min(dados);
                        par2 = clt1.Max(dados);
                        ContinuousUniform unif = new ContinuousUniform(par1, par2);
                        for (int i = 0; i < tabela.GetLength(0); i++)
                        {

                            tabela[i, 2] = unif.CumulativeDistribution(tabela[i, 0]);
                        }

                        break;

                    //case Modelagem.TipoDistribuicao.Poisson:
                    //    par1 = clt1.Mean(dados);
                    //    Poisson pois = new Poisson(par1);
                    //    for (int i = 0; i < tabela.GetLength(0); i++)
                    //    {

                    //        tabela[i, 2] = pois.CumulativeDistribution(tabela[i, 0]);

                    //    }
                    //    break;

                    default:
                        par1 = clt1.Mean(dados);
                        par2 = (clt1.Despadc(clt1.SubColumnArrayDouble(dados, 0))[0, 0]) * Math.Sqrt((dados.GetLength(0)) / (dados.GetLength(0) - 1.0));
                        Normal norm = new Normal(par1, par2);
                        for (int i = 0; i < tabela.GetLength(0); i++)
                        {

                            tabela[i, 2] = norm.CumulativeDistribution(tabela[i, 0]);
                        }
                        break;
                }

                double contadora2 = 0.0;
                for (int i = 0; i < tabela.GetLength(0); i++)
                {
                    tabela[i, 2] = (tabela[i, 2] * n);

                    if (i != 0)
                    {
                        contadora2 += tabela[i - 1, 2];
                        tabela[i, 2] = tabela[i, 2] - contadora2;
                        //if(tabela[i, 2] == 0) 
                    }
                    if (tabela[i, 2] <= 5) verificacao++;

                    if (i == 9) tabela[i, 2] = n - contadora2;
                }
                double[] diferenca = new double[10];

                for (int i = 0; i < tabela.GetLength(0); i++)
                {
                    if (tabela[i, 2] == 0 & tabela[i, 1] == 0) tabela = clt1.DeleteRow(tabela,i);
                    diferenca[i] = Math.Pow(tabela[i, 1] - tabela[i, 2], 2) / tabela[i, 2];
                    test_stat += diferenca[i];
                }

                ChiSquared qq = new ChiSquared(tabela.GetLength(0) - 1);

                p_valor = qq.Density(test_stat);
            }

            #endregion            
        }
        
        #endregion

        #region ShapiroWilk

        public void Test_Shapiro(double[,] dados, out double test_stat, out double p_valor)
        {
            test_stat = 0.0;
            p_valor = 0.0;
        }
        #endregion

        #region Wilcoxon

        public void Wilcoxon(double[,] dados, double[,] dados1, out double estat_wil, out double pvalue_wil, out bool ver_empates)
        {
            clsUtilTools clt = new clsUtilTools();
            Normal nrm = new Normal(0, 1);
            double rank1 = new double();
            double rank2 = new double();
            double[,] cor_empates = new double[0, 0];

            double soma = new double();
            double n1 = new double();
            double n2 = new double();
            ver_empates = false;

            double[,] diferenca = clt.DiffArrayDouble(dados, dados1);

            for (int i = 0; i < diferenca.GetLength(0); i++)
            {
                if (diferenca[i, 0] <= 0.0001 & diferenca[i, 0] >= -0.0001)
                {
                    diferenca = clt.DeleteRow(diferenca, i);
                }
            }

            double[,] diferenca_abs = new double[diferenca.GetLength(0), 1];

            for (int i = 0; i < diferenca.GetLength(0); i++)
            {
                diferenca_abs[i, 0] = Math.Abs(diferenca[i, 0]);
            }

            double[,] sorteado = clt.ArrayRanksEmpate(diferenca_abs);
            double[,] concatenada = clt.Concateh(diferenca, sorteado);

            for (int i = 0; i < concatenada.GetLength(0); i++)
            {
                if (concatenada[i, 0] < 0)
                {
                    rank1 += concatenada[i, 1];
                }
                if (concatenada[i, 0] > 0)
                {
                    rank2 += concatenada[i, 1];
                }
            }

            estat_wil = Math.Min(rank1, rank2);

            //correção para dados 
            clt.FrequencyTable(ref cor_empates, sorteado);
            double[] ti = new double[cor_empates.GetLength(0)];
            for (int i = 0; i < cor_empates.GetLength(0); i++)
            {
                if (cor_empates[i, 1] != 1)
                {
                    ti[i] = cor_empates[i, 1];
                }
                else
                {
                    ti[i] = 0.0;
                }
                soma += ti[i];
            }

            for (int i = 0; i < ti.GetLength(0); i++)
            {
                n1 += Math.Pow(ti[i], 3.0);
                n2 += ti[i];
            }

            if (soma != 0)
            {
                double n = dados.GetLength(0);
                double estat_aprox = (estat_wil - ((n * (n + 1)) / 4)) / (Math.Sqrt(n * (n + 1) * (2 * n + 1) / 24) - ((n1 - n2) / 48));

                pvalue_wil = 2 * nrm.CumulativeDistribution(-Math.Abs(estat_aprox));
                ver_empates = true;
            }
            else
            {
                double n = dados.GetLength(0);
                double estat_aprox = (estat_wil - ((n * (n + 1)) / 4)) / Math.Sqrt(n * (n + 1) * (2 * n + 1) / 24);

                pvalue_wil = 2 * nrm.CumulativeDistribution(-Math.Abs(estat_aprox));
            }
        }

        #endregion

        #region MAnn-Whitney

        public void Mann_Whitney(double[,] dados, double[,] dados1, out double posto, out double estat, out double dp_sas, out double pvalue)
        {
            clsUtilTools clt = new clsUtilTools();
            Normal nrm = new Normal(0, 1);
            double[,] sorteado = clt.ArrayRanksEmpate(dados);
            double rank1 = 0.0;
            double rank2 = 0.0;
            double[,] cat = new double[0, 0];
            clt.FrequencyTable(ref cat, dados1);

            for (int i = 0; i < sorteado.GetLength(0); i++)
            {
                if (dados1[i, 0] == cat[0, 0])
                {
                    rank1 += sorteado[i, 0];
                }
                else
                {
                    rank2 += sorteado[i, 0];
                }
            }

            dp_sas = Math.Pow((clt.Varianciac(sorteado)[0, 0]) * (cat[0, 1] * cat[1, 1]) / (cat[0, 1] + cat[1, 1] - 1), 0.5);
            //double dp_r = Math.Pow(((double)cat[0, 1] * (double)cat[1, 1] * ((double)cat[0, 1] + (double)cat[1, 1] + 1.0)) / 12.0, 0.5); //IME
            double exp1 = (cat[0, 1] / (double)sorteado.GetLength(0)) * (((double)sorteado.GetLength(0) * ((double)sorteado.GetLength(0) + 1.0)) / 2.0);
            double exp2 = (cat[1, 1] / (double)sorteado.GetLength(0)) * (((double)sorteado.GetLength(0) * ((double)sorteado.GetLength(0) + 1.0)) / 2.0);
            if (rank1 - exp1 >= 0)
            {
                estat = (rank1 - exp1 - 0.5) / dp_sas;   //correção de continuidade ref SAS
                double z2 = (rank2 - exp2 + 0.5) / dp_sas;
            }
            else
            {
                estat = (rank1 - exp1 + 0.5) / dp_sas;   //correção de continuidade ref SAS
                double z2 = (rank2 - exp2 - 0.5) / dp_sas;
            }

            posto = rank1;
            pvalue = nrm.CumulativeDistribution(-Math.Abs(estat));
        }
        
        #endregion

        #region Kruskal-Wallis

        public void Kruskal_Wallis(double[,] dados, double[,] dados1, out double h, out double estat, out double pvalue, out double[,] dif)
        {
            clsUtilTools clt = new clsUtilTools();

            double[,] sorteado = new double[0, 0];
            double[,] empates = new double[0, 0];
            clt.ArrayRanksEmpateC(dados, out sorteado, out empates);

            double[,] cat = new double[0, 0];
            clt.FrequencyTable(ref cat, dados1);
            ChiSquared qq = new ChiSquared(cat.GetLength(0) - 1);
            Normal nrm = new Normal(0.0, 1.0);
            estat = 0.0;
            pvalue = 0.0;
            h = 0.0;

            double[,] rank = new double[cat.GetLength(0), 2];
            for (int k = 0; k < cat.GetLength(0); k++)
            {
                for (int i = 0; i < sorteado.GetLength(0); i++)
                {
                    if (dados1[i, 0] == cat[k, 0])
                    {
                        rank[k, 0] += sorteado[i, 0];
                        rank[k, 1] += empates[i, 0];
                    }
                }
            }

            double acum = 0.0;
            double acum2 = 0.0;
            double h_empate = 0.0;
            for (int k = 0; k < rank.GetLength(0); k++)
            {
                acum += Math.Pow(rank[k, 0], 2) / cat[k, 1];
                acum2 += Math.Pow(rank[k, 1], 3) - rank[k, 1];
            }

            h = (12.0 / (dados.GetLength(0) * (dados.GetLength(0) + 1.0))) * acum - 3.0 * (dados.GetLength(0) + 1.0);
            h_empate = h / (1.0 - (acum2 / (Math.Pow(dados.GetLength(0), 3) - dados.GetLength(0))));
            estat = h_empate;
            pvalue = 1.0 - qq.CumulativeDistribution(h_empate);
            double a = nrm.InverseCumulativeDistribution(0.05);
            dif = new double[(rank.GetLength(0) * (rank.GetLength(0) - 1)) / 2, 5];
            int lin = 0;
            
            for (int i = 0; i < rank.GetLength(0) - 1; i++)
            {
                for (int j = i + 1; j < rank.GetLength(0); j++)
                {
                    dif[lin, 0] = (int)(i + 1);
                    dif[lin, 1] = (int)(j + 1);
                    dif[lin, 2] = Math.Abs(nrm.InverseCumulativeDistribution(0.05 / ((rank.GetLength(0) * (rank.GetLength(0) - 1)) / 2)) * Math.Sqrt(((dados1.GetLength(0) * (dados1.GetLength(0) + 1)) / 12) * (1 / cat[i, 1] + 1 / cat[j, 1])));
                    dif[lin, 3] = Math.Abs(rank[i, 0] / cat[i, 1] - rank[j, 0] / cat[j, 1]);
                    
                    if (dif[lin, 2] < dif[lin, 3])
                    {
                        dif[lin, 4] = 1;
                    }
                    else
                    {
                        dif[lin, 4] = 0;
                    }
                    lin += 1;
                }
            }           
        }
        
        #endregion

        #region Testes comparação de médias paramétricos
        
        /// <summary>
        ///  Teste de comparação de médias para amostras independentes quando as variâncias são desconhecidas e desiguais.
        /// </summary>
        /// <param name="dados"> Amostra 1</param>
        /// <param name="dados1">Variável de definição de classe</param>
        /// <param name="estat"> Estatística do teste</param>
        /// <param name="pvalue"> P-valor</param>
        [Obsolete("This method will be rewritten soon.")]
        public void TesteT_Independente(double[,] dados, double[,] dados1, out double estat, out double pvalue)
        {
            clsUtilTools clt = new clsUtilTools();
            double[,] tabclasse = new double[0, 0];
            clt.FrequencyTable(ref tabclasse, dados1);
            double[] g1 = new double[(int)tabclasse[0, 1]];
            double[] g2 = new double[(int)tabclasse[1, 1]];
            int c1 = 0;
            int c2 = 0;

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados1[i, 0] == tabclasse[0, 0])
                {
                    g1[c1] = dados[i, 0];
                    c1 += 1;
                }
                else
                {
                    g2[c2] = dados[i, 0];
                    c2 += 1;
                }
            }

            double media_g1 = clt.Mean(g1);
            double media_g2 = clt.Mean(g2);

            double var_g1 = 0;
            for (int k = 0; k < g1.Length; k++) var_g1 += Math.Pow(g1[k] - media_g1, 2) / (g1.Length - 1);

            double var_g2 = 0;
            for (int k = 0; k < g2.Length; k++) var_g2 += Math.Pow(g2[k] - media_g2, 2) / (g2.Length - 1);

            int n = g1.Length;
            int m = g2.Length;
            double a = var_g1 / n;
            double b = var_g2 / m;
            estat = (media_g1 - media_g2) / Math.Pow(a + b, 0.5);
            double v = Math.Pow(a + b, 2.0) / (Math.Pow(a, 2.0) / (n - 1.0) + Math.Pow(b, 2.0) / (m - 1.0));

            MathStudenttdist T = new MathStudenttdist(v);
            pvalue = T.cdf(-Math.Abs(estat)) * 2.0;
        }

        /// <summary>
        /// Teste de comparação de médias para amostras independentes quando as variâncias são conhecidas.
        /// </summary>
        /// <param name="dados"> Amostra 1</param>
        /// <param name="dados1"> Variável de definição de classe</param>
        /// <param name="estat"> Estatística do teste</param>
        /// <param name="pvalue"> P-valor</param>
        /// <param name="var1"> Variância do grupo 1</param>
        /// <param name="var2"> Variância do grupo 2</param>
        [Obsolete("This method will be rewritten soon.")]
        public void TesteT_Independente(double[,] dados, double[,] dados1, out double estat, out double pvalue, double var1, double var2)
        {
            clsUtilTools clt = new clsUtilTools();
            double[,] tabclasse = new double[0, 0];
            clt.FrequencyTable(ref tabclasse, dados1);
            double[] g1 = new double[(int)tabclasse[0, 1]];
            double[] g2 = new double[(int)tabclasse[1, 1]];
            int c1 = 0;
            int c2 = 0;
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados1[i, 0] == tabclasse[0, 0])
                {
                    g1[c1] = dados[i, 0];
                    c1 += 1;
                }
                else
                {
                    g2[c2] = dados[i, 0];
                    c2 += 1;
                }
            }
            double media_g1 = clt.Mean(g1);
            double media_g2 = clt.Mean(g2);
            double var_g1 = var1;
            double var_g2 = var2;
            int n = g1.GetLength(0);
            int m = g2.GetLength(0);
            double a = var_g1 / n;
            double b = var_g2 / m;
            estat = (media_g1 - media_g2) / Math.Pow(a + b, 0.5);
            Normal N = new Normal(0, a + b);
            pvalue = N.CumulativeDistribution(-Math.Abs(estat)) * 2.0;
        }
        
        /// <summary>
        /// Teste de comparação de médias para amostras independentes quando as variâncias são desconhecidas e iguais.
        /// </summary>
        /// <param name="dados"> Amostra 1</param>
        /// <param name="dados1">Variável de definição de classe</param>
        /// <param name="estat"> Estatística do teste</param>
        /// <param name="pvalue"> P-valor</param>
        [Obsolete("This method will be rewritten soon.")]
        public void TesteT_Independente(double[,] dados, double[,] dados1, out double estat, out double pvalue, bool igual)
        {
            clsUtilTools clt = new clsUtilTools();
            double[,] tabclasse = new double[0, 0];
            clt.FrequencyTable(ref tabclasse, dados1);
            double[] g1 = new double[(int)tabclasse[0, 1]];
            double[] g2 = new double[(int)tabclasse[1, 1]];
            int c1 = 0;
            int c2 = 0;

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados1[i, 0] == tabclasse[0, 0])
                {
                    g1[c1] = dados[i, 0];
                    c1 += 1;
                }
                else
                {
                    g2[c2] = dados[i, 0];
                    c2 += 1;
                }
            }
            double media_g1 = clt.Mean(g1);
            double media_g2 = clt.Mean(g2);

            double var_g1 = 0;
            for (int k = 0; k < g1.Length; k++) var_g1 += Math.Pow(g1[k] - media_g1, 2) / (g1.Length - 1);

            double var_g2 = 0;
            for (int k = 0; k < g2.Length; k++) var_g2 += Math.Pow(g2[k] - media_g2, 2) / (g2.Length - 1);

            int n = g1.GetLength(0);
            int m = g2.GetLength(0);
            double a = var_g1 / n;
            double b = var_g2 / m;
            double sp = ((n - 1) * var_g1 + (m - 1) * var_g2) / (n + m - 2.0);
            estat = (media_g1 - media_g2) / (Math.Pow(sp * (1.0 / n + 1.0 / m), 0.5));
            MathStudenttdist T = new MathStudenttdist(n + m - 2.0);
            pvalue = T.cdf(-Math.Abs(estat)) * 2.0;
        }

        /// <summary>
        /// Teste de comparação de médias para amostras dependentes (pareadas) com variancia desconhecida
        /// </summary>
        /// <param name="dados">Amostra 1</param>
        /// <param name="dados1">Amostra 2</param>
        /// <param name="estat">Estatística do teste</param>
        /// <param name="pvalue">P-valor</param>
        [Obsolete("This method will be rewritten soon.")]
        public void TesteT_Dependentes(double[,] dados, double[,] dados1, out double estat, out double pvalue)
        {
            clsUtilTools clt = new clsUtilTools();
            double[,] tabclasse = new double[0, 0];
            clt.FrequencyTable(ref tabclasse, dados1);
            double[] g1 = new double[(int)tabclasse[0, 1]];
            double[] g2 = new double[(int)tabclasse[1, 1]];
            int c1 = 0;
            int c2 = 0;

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados1[i, 0] == tabclasse[0, 0])
                {
                    g1[c1] = dados[i, 0];
                    c1 += 1;
                }
                else
                {
                    g2[c2] = dados[i, 0];
                    c2 += 1;
                }
            }

            double[] D = new double[Math.Min(g1.Length, g2.Length)];
            for (int k = 0; k < D.Length; k++)
                D[k] = g1[k] - g2[k];

            double media_d = D.Average();
            double var_d = 0.0;
            for (int k = 0; k < D.Length; k++) var_d += Math.Pow(D[k] - media_d, 2) / (D.Length - 1);

            int n = D.Length;
            estat = (media_d) / Math.Sqrt(var_d / n);
            MathStudenttdist T = new MathStudenttdist(n - 1);
            pvalue = T.cdf(-Math.Abs(estat)) * 2.0;
        }

        /// <summary>
        /// Teste de comparação de médias para amostras dependentes (pareadas) com variancia conhecidas
        /// </summary>
        /// <param name="dados">Amostra 1</param>
        /// <param name="dados1">Amostra 2</param>
        /// <param name="estat">Estatística do teste</param>
        /// <param name="pvalue">P-valor</param>
        /// <param name="var1"> Variância</param>
        [Obsolete("This method will be rewritten soon.")]
        public void TesteT_Dependentes(double[,] dados, double[,] dados1, out double estat, out double pvalue, double var1)
        {
            clsUtilTools clt = new clsUtilTools();
            double[,] tabclasse = new double[0, 0];
            clt.FrequencyTable(ref tabclasse, dados1);
            double[] g1 = new double[(int)tabclasse[0, 1]];
            double[] g2 = new double[(int)tabclasse[1, 1]];
            int c1 = 0;
            int c2 = 0;

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                if (dados1[i, 0] == tabclasse[0, 0])
                {
                    g1[c1] = dados[i, 0];
                    c1 += 1;
                }
                else
                {
                    g2[c2] = dados[i, 0];
                    c2 += 1;
                }
            }

            double[] D = new double[Math.Min(g1.Length, g2.Length)];
            for (int k = 0; k < D.Length; k++)
                D[k] = g1[k] - g2[k];

            double media_d = D.Average();
            double var_d = var1;

            int n = D.Length;
            estat = (media_d) / Math.Sqrt(var_d / n);
            Normal N = new Normal(0.0, 1.0);
            pvalue = N.CumulativeDistribution(-Math.Abs(estat)) * 2.0;
        }

        #endregion

        #region Teste Anova
        /// <summary>
        /// Teste paramétrico de comparação de médias para mais de duas populações.
        /// </summary>
        /// <param name="dados">Amostra 1</param>
        /// <param name="dados1">Variável de definição do fator</param>
        public void TesteAnova(double[,] dados, double[,] dados1, out string[] nomes, out double[] vetorsomadequadrados, out double[] vetorgl, out string[] vetorqdmedio, out string[] vetorF, out string[] vetorpvalue)
        {
            double qmd = 0.0;
            double qme = 0.0;
            double sqt = 0.0;
            double sqd = 0.0;
            double sqe = 0.0;
            double F = 0.0;
            double glE = 0.0;
            double glD = 0.0;
            double glT = 0.0;
            double m = 0.0;
            double somatY2 = 0.0;
            double somatY = 0.0;
            double somatYbarra2 = 0.0;
            double Ybarra = 0.0;
            double Ybarra2 = 0.0;
            double somatnYbarra2 = 0.0;
            double N = 0.0;
            double pvalue = 0.0;
            double[,] tabclasse = new double[0, 0];
            double dif = 0.0;

            clsUtilTools clt = new clsUtilTools();
            clt.FrequencyTable(ref tabclasse, dados1);
            double[,] tabclasseMedias = new double[tabclasse.GetLength(0), 3];
            double[,] D = new double[tabclasse.GetLength(0), 1];

            for (int i = 0; i < dados.GetLength(0); i++)
                for (int j = 0; j < tabclasse.GetLength(0); j++)
                    if (dados1[i, 0] == tabclasse[j, 0])
                        D[j, 0] += dados[i, 0];

            for (int i = 0; i < tabclasse.GetLength(0); i++)
            {
                tabclasseMedias[i, 0] = tabclasse[i, 0];
                tabclasseMedias[i, 1] = tabclasse[i, 1];
                tabclasseMedias[i, 2] = D[i, 0] / tabclasse[i, 1];
            }

            int k = tabclasseMedias.GetLength(0);
            // Verificando se os tamanhos das amostras são iguais entre os Grupos
            for (int i = 0; i < k - 1; i++)
            {
                dif += tabclasseMedias[i, 1] - tabclasseMedias[i + 1, 1];
            }

            //Para Grupos/Classes com tamanhos Iguais
            if (dif == 0.0) // Todas as diferenças de tamanhos N devem ser iguais a zero, isto é, não existe diferenças 
            {
                for (int j = 0; j < k; j++)
                {
                    somatYbarra2 += Math.Pow(tabclasseMedias[j, 2], 2.0);
                    m = tabclasseMedias[0, 1];
                }

                for (int l = 0; l < m * k; l++)
                {
                    somatY2 += Math.Pow(dados[l, 0], 2.0);
                    somatY += dados[l, 0];
                }

                Ybarra = somatY / (k * m);
                Ybarra2 = Math.Pow(Ybarra, 2.0);
                sqt = somatY2 - m * k * Ybarra2;
                sqd = somatY2 - m * somatYbarra2;
                sqe = sqt - sqd;
                qmd = sqd / (k * m - k);
                qme = sqe / (k - 1);
                F = qme / qmd;
                glT = k * m - 1;
                glD = k * (m - 1);
                glE = k - 1;

                FisherSnedecor Distf = new FisherSnedecor(glE, glD);
                pvalue = 1.0 - Distf.CumulativeDistribution(F);

                nomes = new string[] { "Entre", "Dentro", "Total" };
                vetorsomadequadrados = new double[] { sqe, sqd, sqt };
                vetorgl = new double[] { glE, glD, glT };
                vetorqdmedio = new string[] { clt.Double2Texto(qme, 6), clt.Double2Texto(qmd, 6), " " };
                vetorF = new string[] { clt.Double2Texto(F, 6), "", "" };
                string spval_Fstatistic = clt.Double2Texto(pvalue, 6);
                vetorpvalue = new string[] { spval_Fstatistic, "", "" };
            }
            //Para Grupos de tamanhos Diferentes
            else
            {
                for (int j = 0; j < k; j++)
                {
                    somatYbarra2 += Math.Pow(tabclasseMedias[j, 2], 2.0);
                    somatnYbarra2 += tabclasseMedias[j, 1] * (Math.Pow(tabclasseMedias[j, 2], 2.0));
                    N += tabclasseMedias[j, 1];
                }

                for (int l = 0; l < N; l++)
                {
                    somatY2 += Math.Pow(dados[l, 0], 2.0);
                    somatY += dados[l, 0];
                }

                Ybarra = somatY / (N);
                Ybarra2 = Math.Pow(Ybarra, 2.0);
                sqt = somatY2 - N * Ybarra2;
                sqd = somatY2 - somatnYbarra2;
                sqe = somatnYbarra2 - N * Ybarra2;
                qmd = sqd / (N - k);
                qme = sqe / (k - 1);
                F = qme / qmd;
                glT = N - 1;
                glD = N - k;
                glE = k - 1;
                FisherSnedecor fdist = new FisherSnedecor(glE, glD);
                pvalue = 1.0 - fdist.CumulativeDistribution(F);

                nomes = new string[] { "Entre", "Dentro", "Total" };
                vetorsomadequadrados = new double[] { sqe, sqd, sqt };
                vetorgl = new double[] { glE, glD, glT };
                vetorqdmedio = new string[] { clt.Double2Texto(qme, 6), clt.Double2Texto(qmd, 6), " " };
                vetorF = new string[] { clt.Double2Texto(F, 6), "", "" };
                string spval_Fstatistic = clt.Double2Texto(pvalue, 6);
                vetorpvalue = new string[] { spval_Fstatistic, "", "" };
            }
        }
    }
}

#endregion






