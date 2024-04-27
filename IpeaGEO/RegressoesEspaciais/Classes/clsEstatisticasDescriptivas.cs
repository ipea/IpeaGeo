using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsEstatisticasDescriptivas
    {
        public clsEstatisticasDescriptivas()
        {
            string[] lista = new string[16];

            lista[0] = "Média";
            lista[1] = "Mediana";
            lista[2] = "Desvio padrão";
            lista[3] = "Variância";
            lista[4] = "Máximo";
            lista[5] = "Mínimo";
            lista[6] = "Coeficiente de assimetria";
            lista[7] = "Kurtose";
            lista[8] = "Número de observações";
            lista[9] = "Primeiro quartil";
            lista[10] = "Terceiro quartil";
            lista[11] = "Intervalo interquartil";
            lista[12] = "Percentil 1";
            lista[13] = "Percentil 2";
            lista[14] = "Percentil 3";
            lista[15] = "Percentil 4";

            m_todas_estatisticas = lista;
        }

        #region variáveis internas

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
            }
        }

        private double m_percentil1 = 0.80;
        private double m_percentil2 = 0.90;
        private double m_percentil3 = 0.95;
        private double m_percentil4 = 0.99;

        public double PercPercentil1
        {
            get { return m_percentil1; }
            set { m_percentil1 = value; }
        }

        public double PercPercentil2
        {
            get { return m_percentil2; }
            set { m_percentil2 = value; }
        }

        public double PercPercentil3
        {
            get { return m_percentil3; }
            set { m_percentil3 = value; }
        }

        public double PercPercentil4
        {
            get { return m_percentil4; }
            set { m_percentil4 = value; }
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
            }
        }

        private double[,] m_dados_continuos = new double[0, 0];
        public double[,] DadosContinuos
        {
            get
            {
                return m_dados_continuos;
            }
            set
            {
            	m_dados_continuos = value;
            }
        }

        private int[] m_freq_categorias = new int[0];
        public int[] FreqCategorias
        {
            get
            {
                return m_freq_categorias;
            }
            set
            {
            	m_freq_categorias = value;
            }
        }

        private string[] m_todas_estatisticas = new string[0];
        private string[] m_variaveis_continuas = new string[0];
        private string[] m_estatisticas = new string[0];

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
            }
        }

        private clsUtilTools m_clt = new clsUtilTools();

        #endregion

        #region funções de cálculo

        public object[,] EstatisticasSemCategorias()
        {
            object[,] res = new object[m_estatisticas.GetLength(0), m_dados_continuos.GetLength(1)];

            for (int i = 0; i < m_estatisticas.GetLength(0); i++)
            {
                for (int j = 0; j < m_dados_continuos.GetLength(1); j++)
                {
                    res[i, j] = EstatisticaEscolhida(m_estatisticas[i], m_clt.SubColumnArrayDouble(m_dados_continuos, j));
                }
            }

            return res;
        }

        public object[,] GerarEstatisticaParaVariavel(string estatistica)
        {
            object[,] res = new object[m_freq_categorias.GetLength(0), m_dados_continuos.GetLength(1)];
            int inicio = 0;
            int fim = 0;

            int[] cortes = new int[m_freq_categorias.GetLength(0)];
            cortes[0] = m_freq_categorias[0];
            for (int k = 1; k < cortes.GetLength(0); k++)
            {
                cortes[k] = cortes[k - 1] + m_freq_categorias[k];
            }

            double[,] dados;

            for (int j = 0; j < m_dados_continuos.GetLength(1); j++)
            {
                inicio = 0;
                for (int k = 0; k < m_freq_categorias.GetLength(0); k++)
                {
                    fim = cortes[k] - 1;
                    dados = m_clt.SubMatriz(m_dados_continuos, inicio, fim, j, j);
                    res[k, j] = EstatisticaEscolhida(estatistica, dados);
                    inicio = fim + 1;
                }
            }

            return res;
        }

        private object EstatisticaEscolhida(string estatistica, double[,] a)
        {
            switch ((estatistica.Trim()))
            {
                case "Média":
                    return this.Mean(a);
                case "Mediana":
                    return this.Mediana(a);
                case "Desvio padrão":
                    return this.StdDev(a);
                case "Variância":
                    return this.Variancia(a);
                case "Máximo":
                    return this.Max(a);
                case "Mínimo":
                    return Min(a);
                case "Coeficiente de assimetria":
                    return Assimetria(a);
                case "Kurtose":
                    return Kurtose(a);
                case "Número de observações":
                    return NumObs(a);
                case "Primeiro quartil":
                    return Quartil1(a);
                case "Terceiro quartil":
                    return Quartil3(a);
                case "Intervalo interquartil":
                    return IntervaloInterQuartil(a);
                case "Percentil 1":
                    return Percentil1(a);
                case "Percentil 2":
                    return Percentil2(a);
                case "Percentil 3":
                    return Percentil3(a);
                case "Percentil 4":
                    return Percentil4(a);
                default:
                   return this.NumObs(a);
            }
        }

        private object IntervaloInterQuartil(double[,] a)
        {
            return Convert.ToDouble(Quartil3(a)) - Convert.ToDouble(Quartil1(a));
        }

        private object Quartil3(double[,] a)
        {
            if (this.m_dados_populacionais)
            {
                return m_clt.Percentil(a, 75.0);
            }
            else
            {
                return m_clt.Percentil(a, 75.0);
            }
        }

        private object Quartil1(double[,] a)
        {
            if (this.m_dados_populacionais)
            {
                return m_clt.Percentil(a, 25.0);
            }
            else
            {
                return m_clt.Percentil(a, 25.0);
            }
        }

        private object Mediana(double[,] a)
        {
            if (this.m_dados_populacionais)
            {
                return m_clt.Percentil(a, 50.0);
            }
            else
            {
                return m_clt.Percentil(a, 50.0);
            }
        }

        private object Percentil4(double[,] a)
        {
            if (this.m_dados_populacionais)
            {
                return m_clt.Percentil(a, m_percentil4);
            }
            else
            {
                return m_clt.Percentil(a, m_percentil4);
            }
        }

        private object Percentil3(double[,] a)
        {
            if (this.m_dados_populacionais)
            {
                return m_clt.Percentil(a, m_percentil3);
            }
            else
            {
                return m_clt.Percentil(a, m_percentil3);
            }
        }

        private object Percentil2(double[,] a)
        {
            if (this.m_dados_populacionais)
            {
                return m_clt.Percentil(a, m_percentil2);
            }
            else
            {
                return m_clt.Percentil(a, m_percentil2);
            }
        }

        private object Percentil1(double[,] a)
        {
            if (this.m_dados_populacionais)
            {
                return m_clt.Percentil(a, m_percentil1);
            }
            else
            {
                return m_clt.Percentil(a, m_percentil1);
            }
        }

        private object Assimetria(double[,] a)
        {
            double media = Convert.ToDouble(this.Mean(a));            
            double soma = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    soma += Math.Pow(((a[i,j] - media) / Math.Pow((double)this.Variancia(a),0.5)), 3.0);                    
                }
            }
            if (m_dados_populacionais)
            {
                return (((double)a.GetLength(0) / ((double)a.GetLength(0))) * (1.0 / ((double)a.GetLength(0))) * soma);
            }
            else
            {
                if (a.GetLength(0) * a.GetLength(1) > 1)
                {
                    return (((double)a.GetLength(0) / ((double)a.GetLength(0) - 2.0)) * (1.0 / ((double)a.GetLength(0) - 1.0)) * soma);
                }
                else
                {
                    return double.NaN;
                }
            }
        }

        private object Kurtose(double[,] a)
        {
            double media = Convert.ToDouble(this.Mean(a));
            double soma = 0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    soma += Math.Pow(((a[i, j] - media) / Math.Pow((double)this.Variancia(a), 0.5)), 4.0);
                }
            }
            if (m_dados_populacionais)
            {               
                //Excesso de curtose
                return ((soma / (double)a.GetLength(0)) - 3);
            }
            else
            {
                if (a.GetLength(0) * a.GetLength(1) > 1)
                {
                    double g1 = (((double)a.GetLength(0)) * ((double)a.GetLength(0) + 1.0)) / (((double)a.GetLength(0) - 1.0) * ((double)a.GetLength(0) - 2.0) * ((double)a.GetLength(0) - 3.0));
                    double g2 = (Math.Pow(((double)a.GetLength(0) - 1.0), 2.0)) / (((double)a.GetLength(0) - 2.0) * ((double)a.GetLength(0) - 3.0));

                    return ((g1 * soma) - (3.0 * g2));
                }
                else
                {
                    return double.NaN;
                }
            }
        }

        private object Variancia(double[,] a)
        {
            double media = Convert.ToDouble(this.Mean(a));
            double[,] res2 = new double[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    res2[i, j] = Math.Pow(a[i, j] - media, 2.0);
                }
            }
            if (m_dados_populacionais)
            {
                return Convert.ToDouble(Sum(res2)) / (((double)a.GetLength(0)) * ((double)a.GetLength(1)));
            }
            else
            {
                if (a.GetLength(0) * a.GetLength(1) > 1)
                {
                    return Convert.ToDouble(Sum(res2)) / (((double)a.GetLength(0)) * ((double)a.GetLength(1)) - 1.0);
                }
                else
                {
                    return double.NaN;
                }
            }
        }

        private object StdDev(double[,] a)
        {
            if (Convert.ToInt32(NumObs(a)) > 1) return Math.Sqrt(Convert.ToDouble(Variancia(a)));
            else return double.NaN;
        }

        /// <summary>Returns the number of elements of the matrix</summary>
        private object NumObs(double[,] a)
        {
            return ((a.GetLength(0)) * (a.GetLength(1)));
        }

        /// <summary>Returns the maximum of elements of the matrix</summary>
        private object Max(double[,] a)
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

        private object Sum(double[,] a)
        {
            double m = 0.0;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    m += a[i, j];
                }
            }
            return m;
        }

        /// <summary>Returns the mean of elements of the matrix</summary>
        private object Mean(double[,] a)
        {            
            return Convert.ToDouble(Sum(a)) / (((double)a.GetLength(0))*((double)a.GetLength(1)));
        }

        /// <summary>Returns the minimum of elements of the matrix</summary>
        private object Min(double[,] a)
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

        #endregion
    }
}
