using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace IpeaGeo.RegressoesEspaciais
{
    public class BLEstatisticasDescritivas
    {
        #region variáveis internas

        public BLEstatisticasDescritivas()
        {            
            m_todas_estatisticas = m_clstats.ListaTodasEstatisticas;
        }

        private bool m_estatisticas_por_categorias = false;
        public bool EstatisticasPorCategorias
        {
            get
            {
                return m_estatisticas_por_categorias;
            }
            set
            {
                m_estatisticas_por_categorias = value;
                m_clstats.EstatisticasPorCategorias = value;
            }
        }

        private string[] m_variaveis_categorias = new string[0];
        private string[] m_variaveis_continuas = new string[0];
        private string[] m_estatisticas = new string[0];
        private string[] m_todas_estatisticas = new string[0];
        private clsEstatisticasDescriptivas m_clstats = new clsEstatisticasDescriptivas();

        public string[] ListaTodasEstatisticas
        {
            get
            {
                return this.m_todas_estatisticas;
            }
        }

        public string[] EstatisticasEscolhidas
        {
            get
            {
                return this.m_estatisticas;
            }
            set
            {
            	this.m_estatisticas = value;
                this.m_clstats.EstatisticasEscolhidas = value;
            }
        }

        public string[] VariaveisContinuas
        {
            get
            {
                return m_variaveis_continuas;
            }
            set
            {
            	this.m_variaveis_continuas = value;
                this.m_clstats.VariaveisContinuas = value;
            }
        }

        public string[] VariaveisCategoricas
        {
            get
            {
                return m_variaveis_categorias;
            }
            set
            {
            	m_variaveis_categorias = value;
            }
        }

        private DataTable m_dt_tabela_dados = new DataTable();
        public DataTable TabelaDados
        {
            get
            {
                return this.m_dt_tabela_dados;
            }
            set
            {
                this.m_dt_tabela_dados = value;
            }
        }

        private int m_num_decimais = 4;
        public int NumDecimais
        {
            get
            {
                return m_num_decimais;
            }
            set
            {
            	m_num_decimais = value;
            }
        }

        private double m_percentil1 = 0.80;
        private double m_percentil2 = 0.90;
        private double m_percentil3 = 0.95;
        private double m_percentil4 = 0.99;

        public double PercPercentil1
        {
            get { return m_percentil1; }
            set { m_percentil1 = value; m_clstats.PercPercentil1 = value; }
        }

        public double PercPercentil2
        {
            get { return m_percentil2; }
            set { m_percentil2 = value; m_clstats.PercPercentil2 = value; }
        }

        public double PercPercentil3
        {
            get { return m_percentil3; }
            set { m_percentil3 = value; m_clstats.PercPercentil3 = value; }
        }

        public double PercPercentil4
        {
            get { return m_percentil4; }
            set { m_percentil4 = value; m_clstats.PercPercentil4 = value; }
        }

        private bool m_dados_populacionais = false;
        public bool DadosPopulacionais
        {
            get
            {
                return m_dados_populacionais;
            }
            set
            {
                this.m_dados_populacionais = value;
                m_clstats.DadosPopulacionais = this.m_dados_populacionais;
            }
        }

        private string m_output_text = "";
        public string OutputText
        {
            get
            {
                return m_output_text;
            }
        }

        private bool m_gera_corr_Spearman = false;
        public bool GeraCorrSpearman
        {
            get
            {
                return m_gera_corr_Spearman;
            }
            set
            {
            	m_gera_corr_Spearman = value;
            }
        }

        private bool m_gera_corr_Pearson = false;
        public bool GeraCorrPearson
        {
            get
            {
                return m_gera_corr_Pearson;
            }
            set
            {
                m_gera_corr_Pearson = value;
            }
        }        
        
        private bool m_gera_cov_mat = false;
        public bool GeraMatrizCovariancias
        {
            get
            {
                return m_gera_cov_mat;
            }
            set
            {
                m_gera_cov_mat = value;
            }
        }

        private bool m_gera_cross_tab_count = true;
        public bool GeraCrossTabCount
        {
            set
            {
            	m_gera_cross_tab_count = value;
            }
        }

        private bool m_gera_cross_tab_totpercent = false;
        public bool GeraCrossTabTotPercent
        {
            set
            {
            	m_gera_cross_tab_totpercent = value;
            }
        }

        private bool m_gera_cross_tab_colpercent = false;
        public bool GeraCrossTabColPercent
        {
            set
            {
            	m_gera_cross_tab_colpercent = value;
            }
        }

        private bool m_gera_cross_tab_rowpercent = false;
        public bool GeraCrossTabRowPercent
        {
            set
            {
            	m_gera_cross_tab_rowpercent = value;
            }
        }

        private object[,] m_freqs_cruzadas = new object[0, 0];
        public object[,] FreqsCruzadas
        {
            
            get { return m_freqs_cruzadas; }
        }

        #endregion

        #region Tabulaçoes cruzadas

        public void GeraTabulacoesCruzadas()
        {
            string out_text = "============================================================================================================================\n\n";

            out_text += "Tabulações Cruzadas \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";
            out_text += "Total de observações: " + this.m_dt_tabela_dados.Rows.Count.ToString() + "\n";
            out_text += "Total de variáveis: " + this.m_variaveis_categorias.GetLength(0).ToString() + "\n\n";

            for (int i = 0; i < m_variaveis_categorias.GetLength(0); i++)
            {
                for (int j = i+1; j < m_variaveis_categorias.GetLength(0); j++)
                {
                    out_text += "Variável para as linhas: " + m_variaveis_categorias[i] + "\n";
                    out_text += "Variável para as colunas: " + m_variaveis_categorias[j] + "\n";
                    out_text += FreqAbsolutasDuasVariaveis(m_variaveis_categorias[i], m_variaveis_categorias[j]);
                    out_text += "\n";
                }
            }

            this.m_output_text = out_text;
        }

        private string FreqAbsolutasDuasVariaveis(string var1, string var2)
        {
            int n = m_dt_tabela_dados.Rows.Count;
            object[,] dados = new object[n, 2];
            for (int i = 0; i < n; i++)
            {
                dados[i, 0] = m_dt_tabela_dados.Rows[i][var1];
                dados[i, 1] = m_dt_tabela_dados.Rows[i][var2];
            }

            clsUtilTools clt = new clsUtilTools();

            object[,] sdados = new object[0, 0];
            int num_categorias = 0;
            
            object[,] categorias_cruzadas = new object[0, 0];
            object[,] categorias_var1 = new object[0, 0];
            object[,] categorias_var2 = new object[0, 0];

            int[] freq_categorias_cruzadas = new int[0];
            int[] freq_categorias_var1 = new int[0];
            int[] freq_categorias_var2 = new int[0];

            int[] sort_columns = new int[2];
            sort_columns[0] = 0;
            sort_columns[1] = 1;

            int[] sort_col1 = new int[1];
            int[] sort_col2 = new int[1];

            sort_col1[0] = 0;
            sort_col2[0] = 1;

            clt.SortByColumn(ref sdados, ref categorias_cruzadas, ref freq_categorias_cruzadas, ref num_categorias, dados, sort_columns);
            clt.SortByColumn(ref sdados, ref categorias_var1, ref freq_categorias_var1, ref num_categorias, dados, sort_col1);
            clt.SortByColumn(ref sdados, ref categorias_var2, ref freq_categorias_var2, ref num_categorias, dados, sort_col2);

            object[,] mat = new object[categorias_var1.GetLength(0), categorias_var2.GetLength(0)];
            for (int i = 0; i < categorias_var1.GetLength(0); i++)
            {
                for (int j = 0; j < categorias_var2.GetLength(0); j++)
                {
                    mat[i, j] = 0;
                }                
            }

            string[] scats_var1 = new string[categorias_var1.GetLength(0)];
            string[] scats_var2 = new string[categorias_var2.GetLength(0)];
            string[,] scats = new string[categorias_cruzadas.GetLength(0), 2];

            for (int i = 0; i < scats_var1.GetLength(0); i++)
            {
                scats_var1[i] = categorias_var1[i,0].ToString();
            }
            for (int i = 0; i < scats_var2.GetLength(0); i++)
            {
                scats_var2[i] = categorias_var2[i,0].ToString();
            }
            for (int i = 0; i < categorias_cruzadas.GetLength(0); i++)
            {
                scats[i, 0] = categorias_cruzadas[i, 0].ToString();
                scats[i, 1] = categorias_cruzadas[i, 1].ToString();
            }

            int ind_1 = 0;
            int ind_2 = 0;
            for (int i = 0; i < categorias_cruzadas.GetLength(0); i++)
            {
                ind_1 = 0;
                ind_2 = 0;
                for (int k = 0; k < scats_var1.GetLength(0); k++)
                {
                    if (scats[i, 0] == scats_var1[k])
                    {
                        ind_1 = k;
                        break;
                    }
                }
                for (int k = 0; k < scats_var2.GetLength(0); k++)
                {
                    if (scats[i, 1] == scats_var2[k])
                    {
                        ind_2 = k;
                        break;
                    }
                }
                mat[ind_1, ind_2] = freq_categorias_cruzadas[i];
            }

            string res = "";
            if (m_gera_cross_tab_count)
            {
                res += "\nTabela com frequências absolutas\n\n";
                res += this.GeraTabelaResultados(mat, scats_var2, scats_var1, true);
            }

            if (m_gera_cross_tab_rowpercent)
            {
                object[,] mat_rowpercent = new object[categorias_var1.GetLength(0), categorias_var2.GetLength(0)];
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    for (int j = 0; j < mat.GetLength(1); j++)
                    {
                        mat_rowpercent[i, j] = 100.0 * Convert.ToDouble(mat[i, j]) / (double)freq_categorias_var1[i];
                    }
                }
                res += "\nTabela com percentuais (%) nas linhas \n\n";
                res += this.GeraTabelaResultados(mat_rowpercent, scats_var2, scats_var1, true);
            }

            if (m_gera_cross_tab_colpercent)
            {
                object[,] mat_colpercent = new object[categorias_var1.GetLength(0), categorias_var2.GetLength(0)];
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    for (int j = 0; j < mat.GetLength(1); j++)
                    {
                        mat_colpercent[i, j] = 100.0 * Convert.ToDouble(mat[i, j]) / (double)freq_categorias_var2[j];
                    }
                }
                res += "\nTabela com percentuais (%) nas colunas \n\n";
                res += this.GeraTabelaResultados(mat_colpercent, scats_var2, scats_var1, true);
            }

            if (m_gera_cross_tab_totpercent)
            {
                double nobs = (double)m_dt_tabela_dados.Rows.Count;
                object[,] mat_totpercent = new object[categorias_var1.GetLength(0), categorias_var2.GetLength(0)];
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    for (int j = 0; j < mat.GetLength(1); j++)
                    {
                        mat_totpercent[i, j] = 100.0 * Convert.ToDouble(mat[i, j]) / nobs;
                    }
                }
                res += "\nTabela com percentuais (%) do total \n\n";
                res += this.GeraTabelaResultados(mat_totpercent, scats_var2, scats_var1, true);
            }

            m_freqs_cruzadas = mat;

            return res;
        }

        #endregion

        #region Correlações

        public void CalculaCorrelacoes()
        {
            string out_text = "============================================================================================================================\n\n";

            out_text += "Matrizes de Correlações e Covariâncias \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";
            out_text += "Total de observações: " + this.m_dt_tabela_dados.Rows.Count.ToString() + "\n";
            out_text += "Total de variáveis: " + m_variaveis_continuas.GetLength(0).ToString() + "\n\n";

            if (m_gera_cov_mat)
            {
                out_text += "Matriz de covariâncias: \n\n";

                if (m_dados_populacionais) out_text += this.CovMatrizPopulacionais();
                else out_text += this.CovMatrizAmostrais();

                out_text += "\n";
            }

            if (m_gera_corr_Pearson)
            {
                out_text += "Matriz de correlações de Pearson: \n\n";

                if (m_dados_populacionais) out_text += this.CorrPearsonPopulacionais();
                else out_text += this.CorrPearsonAmostrais();

                out_text += "\n";
            }

            if (m_gera_corr_Spearman)
            {
                out_text += "Matriz de correlações de Spearman: \n\n";

                if (m_dados_populacionais) out_text += this.CorrSpearmanPopulacionais();
                else out_text += this.CorrSpearmanAmostrais();

                out_text += "\n";
            }

            m_output_text = out_text;
        }
        #region Funções Marina
        //Funções Marina
        public double media(double[,] dados, int col1)
        {
            double soma = 0;
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                soma += dados[i, col1];
            }
            return ((soma / (double)dados.GetLength(0)));
        }
        public double variancia(double[,] dados, int coluna)
        {
            double soma = 0;
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                soma += Math.Pow(dados[i, coluna], 2);
            }
            return (((soma / ((double)dados.GetLength(0) - 1.0)) - (((double)dados.GetLength(0) / ((double)dados.GetLength(0) - 1.0)) * (Math.Pow(media(dados, coluna), 2.0)))));
        }
        public double variancia_populacional(double[,] dados, int coluna)
        {
            double soma = 0;
            for (int i = 0; i < dados.GetLength(0); i++)
            {
                soma += Math.Pow(dados[i, coluna], 2);
            }
            return (((soma / ((double)dados.GetLength(0))) - (((double)dados.GetLength(0) / ((double)dados.GetLength(0))) * (Math.Pow(media(dados, coluna), 2.0)))));
        }
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
        public double covariancia_populacional(double[,] x, int col1, int col2)
        {
            double[] xy = new double[x.GetLength(0)];
            double soma = 0;
            for (int i = 0; i < x.GetLength(0); i++)
            {
                soma += ((x[i, col1] - media(x, col1)) * (x[i, col2] - media(x, col2)));

            }

            return (soma / ((double)x.GetLength(0)));
        }
        public double correlacao(double[,] x, int col1, int col2)
        {
            return (covariancia(x, col1, col2) / (Math.Sqrt(variancia(x, col1)) * Math.Sqrt(variancia(x, col2))));

        }
        public double correlacao_spearman(double[,] x, int col1, int col2)
        {
            double[] coluna1 = new double[x.GetLength(0)];
            double[] coluna2 = new double[x.GetLength(0)];            

            for (int i = 0; i < x.GetLength(0); i++)
            {
                coluna1[i] = x[i, col1];
                coluna2[i] = x[i, col2];                
            }
            //Ordena pela primeira coluna
            Array.Sort(coluna1, coluna2);

            //Numera a primeira coluna de 1 a n
            for (int i = 0; i < coluna1.Length; i++)
            {
                coluna1[i] = i;
            }

            //Ordena pela segunda coluna
            Array.Sort(coluna2, coluna1);

            //Numera a segunda coluna de 1 a n
            for (int i = 0; i < coluna1.Length; i++)
            {
                coluna2[i] = i;
            }

            //Calcular a correlação entre as duas colunas criadas.
            double[,] matriz = new double[coluna1.Length, 2];
            for (int i = 0; i < coluna1.Length; i++)
            {
                matriz[i, 0] = coluna1[i];
                matriz[i, 1] = coluna2[i];
            }


            return (covariancia(matriz, 0, 1) / (Math.Sqrt(variancia(matriz, 0)) * Math.Sqrt(variancia(matriz, 1))));

        }
        public double correlacao_spearman_populacional(double[,] x, int col1, int col2)
        {
            double[] coluna1 = new double[x.GetLength(0)];
            double[] coluna2 = new double[x.GetLength(0)];

            for (int i = 0; i < x.GetLength(0); i++)
            {
                coluna1[i] = x[i, col1];
                coluna2[i] = x[i, col2];
            }
            //Ordena pela primeira coluna
            Array.Sort(coluna1, coluna2);

            //Numera a primeira coluna de 1 a n
            for (int i = 0; i < coluna1.Length; i++)
            {
                coluna1[i] = i;
            }

            //Ordena pela segunda coluna
            Array.Sort(coluna2, coluna1);

            //Numera a segunda coluna de 1 a n
            for (int i = 0; i < coluna1.Length; i++)
            {
                coluna2[i] = i;
            }

            //Calcular a correlação entre as duas colunas criadas.
            double[,] matriz = new double[coluna1.Length, 2];
            for (int i = 0; i < coluna1.Length; i++)
            {
                matriz[i, 0] = coluna1[i];
                matriz[i, 1] = coluna2[i];
            }


            return (covariancia_populacional(matriz, 0, 1) / (Math.Sqrt(variancia_populacional(matriz, 0)) * Math.Sqrt(variancia_populacional(matriz, 1))));

        }
        public double correlacao_populacional(double[,] x, int col1, int col2)
        {
            return (covariancia_populacional(x, col1, col2) / (Math.Sqrt(variancia_populacional(x, col1)) * Math.Sqrt(variancia_populacional(x, col2))));

        }
        public double[,] matrizcov(double[,] x)
        {
            double[,] m = new double[x.GetLength(1), x.GetLength(1)];

            for (int row = 0; row < x.GetLength(1); row++)
            {
                for (int col = 0; col < x.GetLength(1); col++)
                {
                    m[row, col] = covariancia(x, row, col);
                }

            }

            return (m);

        }
        public double[,] matrizcov_populacional(double[,] x)
        {
            double[,] m = new double[x.GetLength(1), x.GetLength(1)];

            for (int row = 0; row < x.GetLength(1); row++)
            {
                for (int col = 0; col < x.GetLength(1); col++)
                {
                    m[row, col] = covariancia_populacional(x, row, col);
                }

            }

            return (m);

        }
        
        public double[,] matrizcorr(double[,] x)
        {
            double[,] cor = new double[x.GetLength(1), x.GetLength(1)];
            for (int row = 0; row < x.GetLength(1); row++)
            {
                for (int col = 0; col < x.GetLength(1); col++)
                {
                    cor[row, col] = correlacao(x, row, col);

                }

            }
            return (cor);
        }
        public double[,] matrizcorr_spearman(double[,] x)
        {
            double[,] cor = new double[x.GetLength(1), x.GetLength(1)];
            for (int row = 0; row < x.GetLength(1); row++)
            {
                for (int col = 0; col < x.GetLength(1); col++)
                {
                    cor[row, col] = correlacao_spearman(x, row, col);

                }

            }
            return (cor);
        }
        public double[,] matrizcorr_spearman_populacional(double[,] x)
        {
            double[,] cor = new double[x.GetLength(1), x.GetLength(1)];
            for (int row = 0; row < x.GetLength(1); row++)
            {
                for (int col = 0; col < x.GetLength(1); col++)
                {
                    cor[row, col] = correlacao_spearman_populacional(x, row, col);

                }

            }
            return (cor);
        }
        public double[,] matrizcorr_populacional(double[,] x)
        {
            double[,] cor = new double[x.GetLength(1), x.GetLength(1)];
            for (int row = 0; row < x.GetLength(1); row++)
            {
                for (int col = 0; col < x.GetLength(1); col++)
                {
                    cor[row, col] = correlacao_populacional(x, row, col);

                }

            }
            return (cor);
        }
        //Fim: Funções Marina
        #endregion

        private string CovMatrizAmostrais()
        {
            string res = "";

            object[,] mat = new object[this.m_variaveis_continuas.GetLength(0), this.m_variaveis_continuas.GetLength(0)];
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                mat[i, i] = 1.0;
                for (int j = i + 1; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = 0.0;
                    mat[j, i] = mat[i, j];
                }
            }           
            

            double[,] matriz = new double[m_dt_tabela_dados.Rows.Count, m_variaveis_continuas.Length];
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    matriz[i, j] = Convert.ToDouble(m_dt_tabela_dados.Rows[i][m_variaveis_continuas[j]]);
                }
            }

            double[,] matriz2 = matrizcov(matriz);            

            //Convertendo para object
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = matriz2[i, j];
                    //if (i == j)
                    //{
                       //mat[i, j] = 1.0;
                    //}
                }
            }


            res = this.GeraTabelaResultados(mat, m_variaveis_continuas, m_variaveis_continuas, true);

            return res;
        }

        private string CovMatrizPopulacionais()
        {
            string res = "";

            object[,] mat = new object[this.m_variaveis_continuas.GetLength(0), this.m_variaveis_continuas.GetLength(0)];
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                mat[i, i] = 1.0;
                for (int j = i + 1; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = 0.0;
                    mat[j, i] = mat[i, j];
                }
            }

            double[,] matriz = new double[m_dt_tabela_dados.Rows.Count, m_variaveis_continuas.Length];
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    matriz[i, j] = Convert.ToDouble(m_dt_tabela_dados.Rows[i][m_variaveis_continuas[j]]);
                }
            }

            double[,] matriz2 = matrizcov_populacional(matriz);

            //Convertendo para object
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = matriz2[i, j];
                    //if (i == j)
                    //{
                        //mat[i, j] = 1.0;
                    //}
                }
            }


            res = this.GeraTabelaResultados(mat, m_variaveis_continuas, m_variaveis_continuas, true);

            return res;
        }

        private string CorrSpearmanAmostrais()
        {
            string res = "";

            object[,] mat = new object[this.m_variaveis_continuas.GetLength(0), this.m_variaveis_continuas.GetLength(0)];
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                mat[i, i] = 1.0;
                for (int j = i + 1; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = 0.0;
                    mat[j, i] = mat[i, j];
                }
            }

            double[,] matriz = new double[m_dt_tabela_dados.Rows.Count, m_variaveis_continuas.Length];
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    matriz[i, j] = Convert.ToDouble(m_dt_tabela_dados.Rows[i][m_variaveis_continuas[j]]);
                }
            }

            double[,] matriz2 = matrizcorr_spearman(matriz);

            //Convertendo para object
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = matriz2[i, j];
                    if (i == j)
                    {
                        mat[i, j] = 1.0;
                    }
                }
            }



            res = this.GeraTabelaResultados(mat, m_variaveis_continuas, m_variaveis_continuas, true);

            return res;
        }

        private string CorrSpearmanPopulacionais()
        {
            string res = "";

            object[,] mat = new object[this.m_variaveis_continuas.GetLength(0), this.m_variaveis_continuas.GetLength(0)];
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                mat[i, i] = 1.0;
                for (int j = i + 1; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = 0.0;
                    mat[j, i] = mat[i, j];
                }
            }

            double[,] matriz = new double[m_dt_tabela_dados.Rows.Count, m_variaveis_continuas.Length];
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    matriz[i, j] = Convert.ToDouble(m_dt_tabela_dados.Rows[i][m_variaveis_continuas[j]]);
                }
            }

            double[,] matriz2 = matrizcorr_spearman_populacional(matriz);

            //Convertendo para object
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = matriz2[i, j];
                    if (i == j)
                    {
                        mat[i, j] = 1.0;
                    }
                }
            }



            res = this.GeraTabelaResultados(mat, m_variaveis_continuas, m_variaveis_continuas, true);

            return res;
        }

        private string CorrPearsonAmostrais()
        {
            string res = "";

            object[,] mat = new object[this.m_variaveis_continuas.GetLength(0), this.m_variaveis_continuas.GetLength(0)];
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                mat[i, i] = 1.0;
                for (int j = i + 1; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = 0.0;
                    mat[j, i] = mat[i, j];
                }
            }

            double[,] matriz = new double[m_dt_tabela_dados.Rows.Count, m_variaveis_continuas.Length];
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    matriz[i, j] = Convert.ToDouble(m_dt_tabela_dados.Rows[i][m_variaveis_continuas[j]]);
                }
            }

            double[,] matriz2 = matrizcorr(matriz);

            //Convertendo para object
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = matriz2[i, j];
                    if (i == j)
                    {
                        mat[i, j] = 1.0;
                    }
                }

            }


            res = this.GeraTabelaResultados(mat, m_variaveis_continuas, m_variaveis_continuas, true);

            return res;
        }

        private string CorrPearsonPopulacionais()
        {
            string res = "";

            object[,] mat = new object[this.m_variaveis_continuas.GetLength(0), this.m_variaveis_continuas.GetLength(0)];
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                mat[i, i] = 1.0;
                for (int j = i+1; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = 0.0;
                    mat[j, i] = mat[i, j];
                }
            }

            double[,] matriz = new double[m_dt_tabela_dados.Rows.Count, m_variaveis_continuas.Length];
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    matriz[i, j] = Convert.ToDouble(m_dt_tabela_dados.Rows[i][m_variaveis_continuas[j]]);
                }
            }

            double[,] matriz2 = matrizcorr_populacional(matriz);

            //Convertendo para object
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = matriz2[i, j];
                    if (i == j)
                    {
                        mat[i, j] = 1.0;
                    }
                }
            }


            res = this.GeraTabelaResultados(mat, m_variaveis_continuas, m_variaveis_continuas, true);

            return res;
        }

        #endregion

        #region tabela de frequencias

        public void GeraTabelaFrequencias()
        {
            clsUtilTools clt = new clsUtilTools();
            string res = "";
            object[,] dados = new object[this.m_dt_tabela_dados.Rows.Count, 1];

            for (int i = 0; i < this.m_variaveis_categorias.GetLength(0); i++)
            {
                dados = new object[this.m_dt_tabela_dados.Rows.Count, 1];
                for (int k = 0; k < dados.GetLength(0); k++)
                {
                    dados[k, 0] = m_dt_tabela_dados.Rows[k][m_variaveis_categorias[i]];
                }

                res += "Variável: " + m_variaveis_categorias[i] + "\n\n";
                res += TabelaFrequenciasVariavel(dados);
                res += "\n";
            }

            string out_text = "============================================================================================================================\n\n";

            out_text += "Tabelas de Frequências \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";
            out_text += "Total de observações: " + dados.GetLength(0).ToString() + "\n\n";

            out_text += res;

            m_output_text = out_text;
        }

        private string TabelaFrequenciasVariavel(object[,] dados)
        {
            clsUtilTools clt = new clsUtilTools();

            object[,] sdados = new object[0, 0];
            object[,] categorias = new object[0, 0];
            int[] freq_categorias = new int[0];
            int num_categorias = 0;
            int[] sort_columns = new int[1];
            sort_columns[0] = 0;

            clt.SortByColumn(ref sdados, ref categorias, ref freq_categorias, ref num_categorias, dados, sort_columns);
            ArrayList ordem_sdados = new ArrayList();
            object[,] stats = new object[num_categorias, 4];
            int n = dados.GetLength(0);
            int[] freq_acumulada = new int[num_categorias];
            freq_acumulada[0] = freq_categorias[0];
            for (int i = 1; i < freq_categorias.GetLength(0); i++)
            {
                freq_acumulada[i] = freq_acumulada[i - 1] + freq_categorias[i];
            }

            string[] labels_categorias = new string[num_categorias];

            for (int i = 0; i < freq_categorias.GetLength(0); i++)
            {
                labels_categorias[i] = categorias[i, 0].ToString();
                stats[i, 0] = freq_categorias[i];
                stats[i, 1] = 100.0 * ((double)freq_categorias[i] / (double)n);
                stats[i, 2] = freq_acumulada[i];
                stats[i, 3] = 100.0 * ((double)freq_acumulada[i] / (double)n);
            }

            string[] colunas = new string[4];
            colunas[0] = "Freq. absoluta";
            colunas[1] = "Frequencia (%)";
            colunas[2] = "Freq. acumulada";
            colunas[3] = "Freq. acumulada (%)";

            string res = this.GeraTabelaResultados(stats, colunas, labels_categorias, true);
            return res;
        }

        #endregion

        #region estatísticas descritivas

        #region funções de cálculo

        public void GeraEstatisticas()
        {
            string[] todas_variaveis = new string[m_variaveis_categorias.GetLength(0) + m_variaveis_continuas.GetLength(0)];
            object[,] dados = new object[this.m_dt_tabela_dados.Rows.Count, 
                this.m_variaveis_categorias.GetLength(0) + this.m_variaveis_continuas.GetLength(0)];

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                for (int k = 0; k < m_variaveis_categorias.GetLength(0); k++)
                {
                    if (i == 0)
                    {
                        todas_variaveis[k] = m_variaveis_categorias[k];
                    }
                    dados[i, k] = this.m_dt_tabela_dados.Rows[i][m_variaveis_categorias[k]];
                }
                for (int k = 0; k < m_variaveis_continuas.GetLength(0); k++)
                {
                    if (i == 0)
                    {
                        todas_variaveis[this.m_variaveis_categorias.GetLength(0) + k] = m_variaveis_continuas[k];
                    }
                    dados[i, this.m_variaveis_categorias.GetLength(0) + k] = this.m_dt_tabela_dados.Rows[i][m_variaveis_continuas[k]];
                }
            }

            int[] sort_columns = new int[m_variaveis_categorias.GetLength(0)];
            for (int i = 0; i < sort_columns.GetLength(0); i++)
            {
                sort_columns[i] = i;
            }

            clsUtilTools clt = new clsUtilTools();
            object[,] sdados = new object[0, 0];
            object[,] categorias = new object[0, 0];
            int[] freq_categorias = new int[0];
            int num_categorias = 0;
            object[,] res = new object[0, 0];
            double[,] dados_continuos;

            if (m_estatisticas_por_categorias)
            {
                clt.SortByColumn(ref sdados, ref categorias, ref freq_categorias, ref num_categorias, dados, sort_columns);

                dados_continuos = new double[sdados.GetLength(0), m_variaveis_continuas.GetLength(0)];
                for (int i = 0; i < dados_continuos.GetLength(0); i++)
                {
                    for (int j = 0; j < m_variaveis_continuas.GetLength(0); j++)
                    {
                        dados_continuos[i, j] = Convert.ToDouble(sdados[i, j + this.m_variaveis_categorias.GetLength(0)]);
                    }
                }

                m_clstats.DadosContinuos = dados_continuos;
                m_clstats.FreqCategorias = freq_categorias;

                res = m_clstats.GerarEstatisticaParaVariavel(this.m_estatisticas[0]);
                res = clt.Concateh(categorias, res);

                m_output_text = "Estatística calculada: " + m_estatisticas[0] + "\n\n";
                m_output_text += this.GeraTabelaResultadosPorCategorias(res, todas_variaveis, true, this.m_variaveis_categorias.GetLength(0));
                m_output_text += "\n";

                for (int i = 1; i < this.m_estatisticas.GetLength(0); i++)
                {
                    res = m_clstats.GerarEstatisticaParaVariavel(this.m_estatisticas[i]);
                    res = clt.Concateh(categorias, res);

                    m_output_text += "Estatística calculada: " + m_estatisticas[i] + "\n\n";
                    m_output_text += this.GeraTabelaResultadosPorCategorias(res, todas_variaveis, true, this.m_variaveis_categorias.GetLength(0));
                    m_output_text += "\n";
                }
            }
            else
            {
                dados_continuos = new double[dados.GetLength(0), m_variaveis_continuas.GetLength(0)];
                for (int i = 0; i < dados_continuos.GetLength(0); i++)
                {
                    for (int j = 0; j < m_variaveis_continuas.GetLength(0); j++)
                    {
                        dados_continuos[i, j] = Convert.ToDouble(dados[i, j + this.m_variaveis_categorias.GetLength(0)]);
                    }
                }

                m_clstats.DadosContinuos = dados_continuos;
                m_clstats.FreqCategorias = freq_categorias;

                res = m_clstats.EstatisticasSemCategorias();

                m_output_text = this.GeraTabelaResultados(res, this.m_variaveis_continuas, this.m_estatisticas, true);
            }

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estatísticas Descriptivas para Variáveis Contínuas \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n";

            if (m_dados_populacionais)
                out_text += "Tipo de dados: populacionais" + "\n\n";
            else
                out_text += "Tipo de dados: amostrais" + "\n\n";
            
            m_output_text = out_text + m_output_text;
        }

        #endregion

        #region tabulação matriz de resultados

        private string GeraTabelaResultadosPorCategorias(object[,] matriz, string[] variaveis, bool espaco_titulo_colunas, int num_categoricas)
        {
            clsUtilTools clt = new clsUtilTools();
            int pula_linha = 0;
            if (espaco_titulo_colunas) pula_linha = 1;

            string[,] mat = new string[matriz.GetLength(0) + 1 + pula_linha, variaveis.GetLength(0)];
            for (int j = 0; j < variaveis.GetLength(0); j++)
            {
                mat[pula_linha, j] = " ";
                mat[0, j] = variaveis[j];
            }

            for (int i = 1 + pula_linha; i < mat.GetLength(0); i++)
            {
                for (int j = 0; j < mat.GetLength(1); j++)
                {
                    if (j >= num_categoricas)
                    {
                        mat[i, j] = clt.Double2Texto(matriz[i - 1 - pula_linha, j], this.m_num_decimais);
                    }
                    else
                    {
                        mat[i, j] = matriz[i - 1 - pula_linha, j].ToString();
                    }
                }
            }

            int[] cols_length = new int[mat.GetLength(1)];
            for (int j = 0; j < mat.GetLength(1); j++)
            {
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    if (cols_length[j] < mat[i, j].Length) cols_length[j] = mat[i, j].Length;
                }
                cols_length[j] += 3;
            }

            string res = "";
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                res += mat[i, 0] + PreencheEspacos(cols_length[0] - mat[i, 0].Length);
                for (int j = 1; j < mat.GetLength(1); j++)
                {
                    if (j >= num_categoricas)
                    {
                        res += "\t" + PreencheEspacos(cols_length[j] - mat[i, j].Length) + mat[i, j];
                    }
                    else
                    {
                        res += "\t" + mat[i, j] + PreencheEspacos(cols_length[j] - mat[i, j].Length);
                    }
                }
                res += "\n";
            }

            return res;
        }

        private string GeraTabelaResultados(object[,] matriz, string[] variaveis, string[] estatisticas, bool espaco_titulo_colunas)
        {
            clsUtilTools clt = new clsUtilTools();
            int pula_linha = 0;
            if (espaco_titulo_colunas) pula_linha = 1;

            string[,] mat = new string[estatisticas.GetLength(0) + 1 + pula_linha, variaveis.GetLength(0) + 1];
            mat[0, 0] = " ";
            mat[pula_linha, 0] = " ";
            for (int j = 0; j < variaveis.GetLength(0); j++)
            {
                mat[pula_linha, j + 1] = " ";
                mat[0, j + 1] = variaveis[j];
            }

            for (int j = 0; j < estatisticas.GetLength(0); j++)
            {
                mat[j + 1 + pula_linha, 0] = estatisticas[j];
            }

            for (int i = 1 + pula_linha; i < mat.GetLength(0); i++)
            {
                for (int j = 1; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = clt.Double2Texto(matriz[i - 1 - pula_linha, j - 1], this.m_num_decimais);
                }
            }

            int[] cols_length = new int[mat.GetLength(1)];
            for (int j = 0; j < mat.GetLength(1); j++)
            {
                for (int i = 0; i < mat.GetLength(0); i++)
                {
                    if (cols_length[j] < mat[i, j].Length) cols_length[j] = mat[i, j].Length;
                }
                cols_length[j] += 3;
            }

            string res = "";
            for (int i = 0; i < mat.GetLength(0); i++)
            {
                res += mat[i, 0] + PreencheEspacos(cols_length[0] - mat[i, 0].Length);
                for (int j = 1; j < mat.GetLength(1); j++)
                {
                    res += "\t" + PreencheEspacos(cols_length[j] - mat[i, j].Length) + mat[i, j];
                }
                res += "\n";
            }

            return res;
        }

        private string PreencheEspacos(int n)
        {
            string res = "";
            for (int i = 0; i < n; i++) res += " ";
            return res;
        }

        #endregion

        #endregion
    }
}
