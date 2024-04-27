using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;

namespace IpeaGeo.RegressoesEspaciais
{
    #region Enumerações

    public enum TipoImputacaoNumMissing : int
    {
        MediaGeral = 1,
        MediaCategoria = 2,
        MediaVizinhos = 3,
        ValorFixo = 4,
        Zero = 5
    };

    public enum TipoImputacaoVizinhanca : int
    {
        FuncaoDecaimento = 1,
        MatrizContiguidade = 2,
        VizinhosMaisProximos = 3
    };

    #endregion

    #region Classes de variáveis numéricas missing

    public class clsVariaveisNumMissing
    {
        #region variaveis internas

        private ArrayList m_indices_valores_missing = new ArrayList();
        private string m_nome_variavel = "";
        private double[] m_valores_variavel = new double[0];
        private object[,] m_valores_categorias = new object[0, 0];
        private string[] m_nomes_categoricas = new string[0];
        private TipoImputacaoNumMissing m_tipo_imputacao = TipoImputacaoNumMissing.Zero;
        private double m_valor_fixo = 0.0;
        private string m_status_imputacao = "";

        private TipoFuncaoDecaimento m_tipo_decaimento = TipoFuncaoDecaimento.Exponencial;
        private double m_parametro_decaimento = 1.0;
        private TipoContiguidade m_tipo_contiguidade = TipoContiguidade.Rook;
        private int m_num_vizinhos_proximos = 10;
        private TipoImputacaoVizinhanca m_tipo_vizinhanca = TipoImputacaoVizinhanca.VizinhosMaisProximos;
        private double m_dist_Euclidiana_corte = 1000.0;
        private string m_nome_variavel_x = "";
        private string m_nome_variavel_y = "";
        private double[] m_vetor_variavel_x = new double[0];
        private double[] m_vetor_variavel_y = new double[0];
        private clsIpeaShape m_shape = new clsIpeaShape();

        #endregion

        #region accessando as variáveis internas

        public clsIpeaShape Shape
        {
            get
            {
                return m_shape;
            }
            set
            {
            	m_shape = value;
            }
        }

        public double[] VetorVariavelY
        {
            get
            {
                return m_vetor_variavel_y;
            }
            set
            {
                m_vetor_variavel_y = value;
            }
        }

        public double[] VetorVariavelX
        {
            get
            {
                return m_vetor_variavel_x;
            }
            set
            {
                m_vetor_variavel_x = value;
            }
        }

        public string NomeVariavelCoordY
        {
            get
            {
                return m_nome_variavel_y;
            }
            set
            {
                m_nome_variavel_y = value;
            }
        }

        public string NomeVariavelCoordX
        {
            get
            {
                return m_nome_variavel_x;
            }
            set
            {
                m_nome_variavel_x = value;
            }
        }

        public double DistEuclidianaCorte
        {
            get
            {
                return m_dist_Euclidiana_corte;
            }
            set
            {
                m_dist_Euclidiana_corte = value;
            }
        }

        public TipoImputacaoVizinhanca TipoVizinhancaParaImputacao
        {
            get
            {
                return m_tipo_vizinhanca;
            }
            set
            {
                m_tipo_vizinhanca = value;
            }
        }

        public int NumVizinhosImputacao
        {
            get
            {
                return m_num_vizinhos_proximos;
            }
            set
            {
                m_num_vizinhos_proximos = value;
            }
        }

        public TipoContiguidade TipoContiguidade
        {
            get
            {
                return m_tipo_contiguidade;
            }
            set
            {
                m_tipo_contiguidade = value;
            }
        }

        public double ParametroDecaimento
        {
            get
            {
                return m_parametro_decaimento;
            }
            set
            {
                m_parametro_decaimento = value;
            }
        }

        public TipoFuncaoDecaimento TipoFuncaoDecaimento
        {
            get
            {
                return m_tipo_decaimento;
            }
            set
            {
                m_tipo_decaimento = value;
            }
        }

        public string StatusImputacao
        {
            get
            {
                return m_status_imputacao;
            }
            set
            {
            	m_status_imputacao = value;
            }
        }

        public string[] NomesVariaveisCategoricas
        {
            get
            {
            	return m_nomes_categoricas;
            }
            set
            {
            	m_nomes_categoricas = value;
            }
        }

        public double ValorFixo
        {
            get
            {
                return m_valor_fixo;
            }
            set
            {
                m_valor_fixo = value;
            }
        }

        public TipoImputacaoNumMissing TipoImputacao
        {
            get
            {
                return m_tipo_imputacao;
            }
            set
            {
                m_tipo_imputacao = value;
            }
        }

        public object[,] ValoresCategorias
        {
            get
            {
                return m_valores_categorias;
            }
            set
            {
                m_valores_categorias = value;
            }
        }

        public double[] ValoresVariavel
        {
            get
            {
                return m_valores_variavel;
            }
            set
            {
                m_valores_variavel = value;
            }
        }

        public string NomeVariavelNum
        {
            get
            {
                return m_nome_variavel;
            }
            set
            {
                m_nome_variavel = value;
            }
        }

        public ArrayList IndicesValoresMissing
        {
            get
            {
                return m_indices_valores_missing;
            }
            set
            {
                m_indices_valores_missing = value;
            }
        }

        public clsVariaveisNumMissing()
        {
        }

        #endregion

        #region texto com as opções

        public string ToText
        {
            get
            {
                string out_text = "Imputação para valores missing em variáveis numéricas \n\n";

                out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
                out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

                out_text += "Variável numérica: " + this.m_nome_variavel + "\n";
                out_text += "Número total de observações: " + m_valores_variavel.GetLength(0) + "\n";
                out_text += "Número de observações missing: " + m_indices_valores_missing.Count + "\n\n";

                if (m_indices_valores_missing.Count > 0)
                {
                    string lista_indices = "[";
                    for (int i = 0; i < m_indices_valores_missing.Count - 1; i++)
                    {
                        lista_indices += m_indices_valores_missing[i].ToString() + ", ";
                    }
                    lista_indices += m_indices_valores_missing[m_indices_valores_missing.Count - 1].ToString() + "]";

                    out_text += "Observações missing (primeira observação tem índice 0): \n";
                    out_text += lista_indices;
                    out_text += "\n\n";
                }

                clsUtilTools clt = new clsUtilTools();

                string tipo_imput = "substitui por valor nulo";
                string opcoes = "";
                switch (m_tipo_imputacao)
                {
                    case TipoImputacaoNumMissing.MediaCategoria:
                        tipo_imput = "média por categoria";
                        opcoes += "Variáveis categóricas para as médias: \n";
                        if (m_nomes_categoricas.GetLength(0) > 0)
                        {
                            opcoes += "[";
                            for (int i = 0; i < m_nomes_categoricas.GetLength(0) - 1; i++)
                            {
                                opcoes += m_nomes_categoricas[i] + ", ";
                            }
                            opcoes += m_nomes_categoricas[m_nomes_categoricas.GetLength(0) - 1] + "]";
                        }
                        else
                        {
                            opcoes += "[nenhuma variável escolhida]";
                        }
                        opcoes += "\n\n";
                        break;
                    case TipoImputacaoNumMissing.MediaGeral:
                        tipo_imput = "média geral da variável";
                        break;
                    case TipoImputacaoNumMissing.MediaVizinhos:
                        tipo_imput = "média das observações vizinhas";
                        opcoes += "Opções de vizinhança: ";
                        switch (m_tipo_vizinhanca)
                        {
                            case TipoImputacaoVizinhanca.FuncaoDecaimento:
                                opcoes += "função de decaimento do tipo ";
                                switch (m_tipo_decaimento)
                                {
                                    case TipoFuncaoDecaimento.Exponencial:
                                        opcoes += "exponencial";
                                        break;
                                    case TipoFuncaoDecaimento.Gaussiana:
                                        opcoes += "Gaussiano";
                                        break;
                                    case TipoFuncaoDecaimento.Linear:
                                        opcoes += "Linear";
                                        break;
                                    case TipoFuncaoDecaimento.Uniforme:
                                        opcoes += "Uniforme";
                                        break;
                                    case TipoFuncaoDecaimento.Quadratico:
                                        opcoes += "Quadrático";
                                        break;
                                    default:
                                        break;
                                }
                                opcoes += "\n\n";
                                opcoes += "Variável coordenadas X: " + m_nome_variavel_x + "\n";
                                opcoes += "Variável coordenadas Y: " + m_nome_variavel_y + "\n";
                                opcoes += "Paramétro de decaimento: " + clt.Double2Texto(m_parametro_decaimento, 8) + "\n";
                                opcoes += "Distância máxima de corte: " + clt.Double2Texto(m_dist_Euclidiana_corte, 8);
                                opcoes += "\n\n";
                                break;
                            case TipoImputacaoVizinhanca.MatrizContiguidade:
                                opcoes += "contiguidade do tipo ";
                                if (m_tipo_contiguidade == TipoContiguidade.Queen) { opcoes += "Queen"; }
                                else { opcoes += "Rook"; }
                                opcoes += "\n\n";
                                break;
                            case TipoImputacaoVizinhanca.VizinhosMaisProximos:
                                opcoes += "média dos " + m_num_vizinhos_proximos.ToString() + " vizinhos mais próximos";
                                opcoes += "\n\n";
                                opcoes += "Variável coordenadas X: " + m_nome_variavel_x + "\n";
                                opcoes += "Variável coordenadas Y: " + m_nome_variavel_y;
                                opcoes += "\n\n";
                                break;
                            default:
                                break;
                        }
                        break;
                    case TipoImputacaoNumMissing.ValorFixo:
                        tipo_imput = "substitui por valor fixo";
                        opcoes += "Valor fixo utilizado: " + clt.Double2Texto(m_valor_fixo, 6) + "\n\n";
                        break;
                    default:
                        tipo_imput = "substitui por valor nulo";
                        break;
                }

                out_text += "Tipo de imputação: " + tipo_imput + "\n\n";
                out_text += opcoes;

                out_text += m_status_imputacao;

                return out_text;
            }
        }

        #endregion

        #region executa imputação

        public void ExecutaImputacao()
        {
            switch (m_tipo_imputacao)
            {
                case TipoImputacaoNumMissing.Zero:
                    ImputaValorNulo();
                    break;
                case TipoImputacaoNumMissing.ValorFixo:
                    ImputaValorFixo();
                    break;
                case TipoImputacaoNumMissing.MediaGeral:
                    ImputaMediaGeral();
                    break;
                case TipoImputacaoNumMissing.MediaCategoria:
                    if (m_nomes_categoricas.GetLength(0) > 0)
                    {
                        ImputaMediaSubGrupo();
                    }
                    else
                    {
                        ImputaMediaGeral();
                    }
                    break;
                case TipoImputacaoNumMissing.MediaVizinhos:
                    ImputaMediaVizinhos();
                    break;
                default:
                    ImputaValorNulo();
                    break;
            }
        }

        #endregion

        #region imputacao com média do sub-grupo

        private int[] m_status_media_subgrupo = new int[0];

        private void ImputaMediaSubGrupo()
        {
            clsUtilTools clt = new clsUtilTools();
            StringBuilder sb = new StringBuilder();
            sb.Append("========= Resultados da imputação \n\n");

            m_status_media_subgrupo = new int[m_indices_valores_missing.Count];
            int indice = 0;
            string status = "";
            for (int i = 0; i < m_indices_valores_missing.Count; i++)
            {
                indice = Convert.ToInt32(m_indices_valores_missing[i]);
                m_valores_variavel[indice] = MediaSubgrupo(indice, i);

                switch (m_status_media_subgrupo[i])
                {
                    case 0:
                        status = "(média do subgrupo) = \t" + clt.Double2Texto(m_valores_variavel[indice], 8);
                        break;
                    case 1:
                        status = "(média geral) = \t" + clt.Double2Texto(m_valores_variavel[indice], 8);
                        break;
                    case 2:
                        status = "(usa valor fixo) = \t" + clt.Double2Texto(m_valores_variavel[indice], 8);
                        break;
                    default:
                        status = "(usa valor fixo) = \t" + clt.Double2Texto(m_valores_variavel[indice], 8);
                        break;
                }

                sb.Append("Observação " + indice.ToString() + " " + status + "\n");
            }

            m_status_imputacao = sb.ToString();
        }

        private double MediaSubgrupo(int indice, int numeracao_variavel)
        {
            string[] vetor_categorias = new string[m_nomes_categoricas.GetLength(0)];
            for (int k = 0; k < vetor_categorias.GetLength(0); k++)
            {
                vetor_categorias[k] = m_valores_categorias[indice, k].ToString();
            }
            double m = 0.0;
            int n = 0;
            bool comparacao = false;
            for (int i = 0; i < m_valores_variavel.GetLength(0); i++)
            {
                comparacao = true;
                for (int k = 0; k < vetor_categorias.GetLength(0); k++)
                {
                    if (vetor_categorias[k] != m_valores_categorias[i, k].ToString())
                    {
                        comparacao = false;
                        break;
                    }
                }
                if (comparacao)
                {
                    if (double.IsNaN(m_valores_variavel[i]) || double.IsInfinity(m_valores_variavel[i])
                        || double.IsNegativeInfinity(m_valores_variavel[i]) || double.IsPositiveInfinity(m_valores_variavel[i]))
                    {
                    }
                    else
                    {
                        if (!m_indices_valores_missing.Contains(i))
                        {
                            m += m_valores_variavel[i];
                            n++;
                        }
                    }
                }
            }

            if (n > 0)
            {
                m_status_media_subgrupo[numeracao_variavel] = 0; // usa média sub-grupo
                return m / ((double)n);
            }
            else
            {
                double media_geral = this.MediaGeral();
                if (double.IsInfinity(media_geral) || double.IsNaN(media_geral)
                    || double.IsNegativeInfinity(media_geral) || double.IsPositiveInfinity(media_geral))
                {
                    m_status_media_subgrupo[numeracao_variavel] = 2; // usa valor nulo
                    return 0.0;
                }
                else
                {
                    m_status_media_subgrupo[numeracao_variavel] = 1; // usa média geral
                    return media_geral;
                }
            }
        }

        #endregion

        #region imputacao com média dos vizinhos

        private void ImputaMediaVizinhos()
        {
            switch (this.m_tipo_vizinhanca)
            {
                case TipoImputacaoVizinhanca.FuncaoDecaimento:
                    ImputaVizinhosFuncaoDecaimento();
                    break;
                case TipoImputacaoVizinhanca.MatrizContiguidade:
                    ImputaVizinhosContiguidade();
                    break;
                case TipoImputacaoVizinhanca.VizinhosMaisProximos:
                    ImputacaoVizinhosMaisProximos();
                    break;
                default:
                    break;
            }
        }

        #region média dos vizinhos via função de decaimento

        private void ImputaVizinhosFuncaoDecaimento()
        {
            clsUtilTools clt = new clsUtilTools();
            StringBuilder sb = new StringBuilder();
            sb.Append("========= Resultados da imputação \n\n");

            string status = "";

            m_indices_temp_missing = new ArrayList();
            for (int i = 0; i < m_indices_valores_missing.Count; i++)
            {
                m_indices_temp_missing.Add(Convert.ToInt32(m_indices_valores_missing[i]));
            }

            int obs_foco = 0;
            ArrayList obs_pesos_vizinhos = new ArrayList();
            ArrayList obs_vizinhos = IndiceNumeroMaximoVizinhosDecaimento(ref obs_foco, ref obs_pesos_vizinhos);
            double m = 0.0;
            double n = 0;
            while (obs_vizinhos.Count > 0 && m_indices_temp_missing.Count > 0)
            {
                m = 0.0;
                n = 0.0;
                for (int i = 0; i < obs_vizinhos.Count; i++)
                {
                    m += m_valores_variavel[Convert.ToInt32(obs_vizinhos[i])] * Convert.ToDouble(obs_pesos_vizinhos[i]);
                    n += Convert.ToDouble(obs_pesos_vizinhos[i]);
                }

                m_valores_variavel[obs_foco] = m / n;

                status = "(média ponderada dos vizinhos) = \t" + clt.Double2Texto(m_valores_variavel[obs_foco], 8);
                sb.Append("Observação " + obs_foco.ToString() + " " + status + "\n");

                m_indices_temp_missing.Remove(obs_foco);
                if (m_indices_temp_missing.Count > 0)
                {
                    obs_vizinhos = IndiceNumeroMaximoVizinhosDecaimento(ref obs_foco, ref obs_pesos_vizinhos);
                }
                else
                {
                    break;
                }
            }

            if (m_indices_temp_missing.Count > 0)
            {
                for (int i = 0; i < m_indices_temp_missing.Count; i++)
                {
                    obs_foco = Convert.ToInt32(m_indices_temp_missing[i]);
                    m_valores_variavel[obs_foco] = 0.0;

                    status = "(usa valor fixo) = \t" + clt.Double2Texto(m_valores_variavel[obs_foco], 8);
                    sb.Append("Observação " + obs_foco.ToString() + " " + status + "\n");
                }
            }

            m_status_imputacao = sb.ToString();
        }

        private ArrayList IndiceNumeroMaximoVizinhosDecaimento(ref int obs_foco, ref ArrayList obs_foco_pesos_vizinhos)
        {
            ArrayList[] listas_vizinhos_validos = new ArrayList[m_indices_temp_missing.Count];
            ArrayList[] listas_pesos_vizinhos = new ArrayList[m_indices_temp_missing.Count];

            int num_max_vizinhos_validos = 0;
            int indice = 0;
            ArrayList pesos_vizinhos = new ArrayList();
            for (int i = 0; i < listas_vizinhos_validos.GetLength(0); i++)
            {
                listas_vizinhos_validos[i] = ListaVizinhosValidosDecaimento(Convert.ToInt32(m_indices_temp_missing[i]), ref pesos_vizinhos);
                listas_pesos_vizinhos[i] = pesos_vizinhos;

                if (listas_vizinhos_validos[i].Count > num_max_vizinhos_validos)
                {
                    num_max_vizinhos_validos = listas_vizinhos_validos[i].Count;
                    indice = i;
                }
            }
            obs_foco = Convert.ToInt32(m_indices_temp_missing[indice]);
            obs_foco_pesos_vizinhos = listas_pesos_vizinhos[indice];
            return listas_vizinhos_validos[indice];
        }

        private ArrayList ListaVizinhosValidosDecaimento(int indice, ref ArrayList pesos_vizinhos)
        {
            ArrayList lista = new ArrayList();
            pesos_vizinhos = new ArrayList();

            double x1 = m_vetor_variavel_x[indice];
            double y1 = m_vetor_variavel_y[indice];
            double x2, y2;
            double peso;

            for (int i = 0; i < m_vetor_variavel_x.GetLength(0); i++)
            {
                if (!this.m_indices_temp_missing.Contains(i))
                {
                    x2 = m_vetor_variavel_x[i];
                    y2 = m_vetor_variavel_y[i];
                    peso = PesoDecaimentoVizinho(x1, y1, x2, y2);
                    if (peso >= 0.0)
                    {
                        lista.Add(i);
                        pesos_vizinhos.Add(peso);
                    }
                }
            }
            return lista;
        }

        private double PesoDecaimentoVizinho(double x1, double y1, double x2, double y2)
        {
            double res = 0.0;
            double d = Math.Sqrt(Math.Pow(x1 - x2, 2.0) + Math.Pow(y1 - y2, 2.0));
            if (d <= m_dist_Euclidiana_corte)
            {
                res = funcao_decaimento(d);
            }
            return res;
        }

        #region funções de decaimento

        private double funcao_decaimento(double d)
        {
            double fx = 0.0;
            if (d <= this.m_dist_Euclidiana_corte)
            {
                switch (m_tipo_decaimento)
                {
                    case TipoFuncaoDecaimento.Exponencial:
                        fx = this.exponencial(d);
                        break;
                    case TipoFuncaoDecaimento.Gaussiana:
                        fx = this.normal(d);
                        break;
                    case TipoFuncaoDecaimento.Linear:
                        fx = this.linear(d);
                        break;
                    case TipoFuncaoDecaimento.Quadratico:
                        fx = this.quadratico(d);
                        break;
                    case TipoFuncaoDecaimento.Uniforme:
                        fx = uniforme(d);
                        break;
                    default:
                        fx = 0.0;
                        break;
                }
            }
            return fx;
        }

        private double linear(double d)
        {       
            double fx = 0.0;
            if (d < m_parametro_decaimento)
            {
                fx = 1.0 - (d / m_parametro_decaimento);
            }
            return fx;
        }

        private double quadratico(double d)
        {
            double fx = 0.0;
            if (d < m_parametro_decaimento)
            {
                fx = 1.0 - Math.Pow(d / m_parametro_decaimento, 2.0);
            }
            return fx;
        }

        private double normal(double d)
        {
            double sigma2 = m_parametro_decaimento * m_parametro_decaimento;
            double fx = Math.Exp(-d * d / (2 * sigma2));
            return fx;
        }

        private double exponencial(double d)
        {
            double fx = Math.Exp(-m_parametro_decaimento * d);
            return fx;
        }

        private double uniforme(double d)
        {
            double fx = 1.0;
            return fx;
        }

        #endregion

        #endregion

        #region média dos vizinhos via contiguidade

        private void ImputaVizinhosContiguidade()
        {
            clsUtilTools clt = new clsUtilTools();
            StringBuilder sb = new StringBuilder();
            sb.Append("========= Resultados da imputação \n\n");

            string status = "";

            int npol = m_shape.Poligonos.GetLength(0);

            double max_y = m_shape.Poligonos[0].BoundingBoxYMax;
            double min_y = m_shape.Poligonos[0].BoundingBoxYMin;
            double max_x = m_shape.Poligonos[0].BoundingBoxXMax;
            double min_x = m_shape.Poligonos[0].BoundingBoxXMin;

            for (int i = 1; i < npol; i++)
            {
                if (m_shape.Poligonos[i].BoundingBoxXMax > max_x) max_x = m_shape.Poligonos[i].BoundingBoxXMax;
                if (m_shape.Poligonos[i].BoundingBoxXMin < min_x) min_x = m_shape.Poligonos[i].BoundingBoxXMin;
                if (m_shape.Poligonos[i].BoundingBoxYMax > max_y) max_y = m_shape.Poligonos[i].BoundingBoxYMax;
                if (m_shape.Poligonos[i].BoundingBoxYMin < min_y) min_y = m_shape.Poligonos[i].BoundingBoxYMin;
            }
            double dimensao_max = Math.Abs(Math.Max(max_x - min_x, max_y - min_y));
            m_tolerancia_contiguidade = 1.0e-6 * dimensao_max;

            m_indices_temp_missing = new ArrayList();
            for (int i = 0; i < m_indices_valores_missing.Count; i++)
            {
                m_indices_temp_missing.Add(Convert.ToInt32(m_indices_valores_missing[i]));
            }

            int obs_foco = 0;
            ArrayList obs_vizinhos = IndiceNumeroMaximoVizinhos(ref obs_foco);
            double m = 0.0;
            int n = 0;
            while (obs_vizinhos.Count > 0 && m_indices_temp_missing.Count > 0)
            {
                m = 0.0;
                n = 0;
                for (int i = 0; i < obs_vizinhos.Count; i++)
                {
                    m += m_valores_variavel[Convert.ToInt32(obs_vizinhos[i])];
                    n++;
                }

                m_valores_variavel[obs_foco] = m / ((double)n);

                status = "(média dos vizinhos contíguos) = \t" + clt.Double2Texto(m_valores_variavel[obs_foco], 8);
                sb.Append("Observação " + obs_foco.ToString() + " " + status + "\n");

                m_indices_temp_missing.Remove(obs_foco);
                if (m_indices_temp_missing.Count > 0)
                {
                    obs_vizinhos = IndiceNumeroMaximoVizinhos(ref obs_foco);
                }
                else
                {
                    break;
                }
            }

            if (m_indices_temp_missing.Count > 0)
            {
                for (int i = 0; i < m_indices_temp_missing.Count; i++)
                {
                    obs_foco = Convert.ToInt32(m_indices_temp_missing[i]);
                    m_valores_variavel[obs_foco] = 0.0;

                    status = "(usa valor fixo) = \t" + clt.Double2Texto(m_valores_variavel[obs_foco], 8);
                    sb.Append("Observação " + obs_foco.ToString() + " " + status + "\n");
                }
            }

            m_status_imputacao = sb.ToString();
        }

        private double m_tolerancia_contiguidade = 1.0e-6;
        private ArrayList m_indices_temp_missing = new ArrayList();

        private ArrayList IndiceNumeroMaximoVizinhos(ref int obs_foco)
        {
            ArrayList[] listas_vizinhos_validos = new ArrayList[m_indices_temp_missing.Count];
            int num_max_vizinhos_validos = 0;
            int indice = 0;
            for (int i = 0; i < listas_vizinhos_validos.GetLength(0); i++)
            {
                listas_vizinhos_validos[i] = ListaVizinhosValidos(Convert.ToInt32(m_indices_temp_missing[i]));
                if (listas_vizinhos_validos[i].Count > num_max_vizinhos_validos)
                {
                    num_max_vizinhos_validos = listas_vizinhos_validos[i].Count;
                    indice = i;
                }
            }
            obs_foco = Convert.ToInt32(m_indices_temp_missing[indice]);
            return listas_vizinhos_validos[indice];
        }

        private ArrayList ListaVizinhosValidos(int indice)
        {
            ArrayList lista = new ArrayList();
            clsIpeaPoligono pol_foco = m_shape.Poligonos[indice];
            clsIpeaPoligono pol = new clsIpeaPoligono();

            for (int i = 0; i < m_shape.Poligonos.GetLength(0); i++)
            {
                if (!this.m_indices_temp_missing.Contains(i))
                {
                    pol = m_shape.Poligonos[i];
                    if (IsVizinho(pol_foco, pol))
                    {
                        lista.Add(i);
                    }
                }
            }
            return lista;
        }

        private bool IsVizinho(clsIpeaPoligono pol1, clsIpeaPoligono pol2)
        {
            int n_vertices_comuns = 0;
            double dist = 0.0;

            if (pol1.BoundingBoxXMax < pol2.BoundingBoxXMin) return false;
            if (pol1.BoundingBoxXMin > pol2.BoundingBoxXMax) return false;
            if (pol1.BoundingBoxYMax < pol2.BoundingBoxYMin) return false;
            if (pol1.BoundingBoxYMin > pol2.BoundingBoxYMax) return false;

            for (int i = 0; i < pol1.Count; i++)
            {
                for (int j = 0; j < pol2.Count; j++)
                {
                    dist = Math.Abs(pol1.X(i) - pol2.X(j)) + Math.Abs(pol1.Y(i) - pol2.Y(j));
                    if (dist <= m_tolerancia_contiguidade)
                    {
                        n_vertices_comuns++;

                        if (n_vertices_comuns >= 2)
                        {
                            return true;
                        }
                        if (m_tipo_contiguidade == TipoContiguidade.Queen && n_vertices_comuns >= 1)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region média dos vizinhos mais próximos

        private void ImputacaoVizinhosMaisProximos()
        {
            clsUtilTools clt = new clsUtilTools();
            StringBuilder sb = new StringBuilder();
            sb.Append("========= Resultados da imputação \n\n");

            m_status_media_subgrupo = new int[m_indices_valores_missing.Count];
            int indice = 0;
            string status = "";

            //=============================== checando o número de observações válidas =========================//
            int n_validos = 0;
            double m = 0.0;
            for (int i = 0; i < m_valores_variavel.GetLength(0); i++)
            {
                if (double.IsInfinity(m_valores_variavel[i]) || double.IsNaN(m_valores_variavel[i])
                    || double.IsNegativeInfinity(m_valores_variavel[i]) || double.IsPositiveInfinity(m_valores_variavel[i]))
                {
                }
                else
                {
                    m += m_valores_variavel[i];
                    n_validos++;
                }
            }
            //================================ executando a imputação ==========================================//
            if (n_validos <= m_num_vizinhos_proximos)
            {
                for (int i = 0; i < m_indices_valores_missing.Count; i++)
                {
                    indice = Convert.ToInt32(m_indices_valores_missing[i]);
                    //m_valores_variavel[indice] = MediaVizinhosProximos(indice, i);

                    if (n_validos <= 0)
                    {
                        m_valores_variavel[indice] = 0.0;
                        status = "(usa valor fixo) = \t" + clt.Double2Texto(m_valores_variavel[indice], 8);
                    }
                    else
                    {
                        m_valores_variavel[indice] = m / ((double)n_validos);
                        status = "(média de " + n_validos + " visinhos) = \t" + clt.Double2Texto(m_valores_variavel[indice], 8);
                    }

                    sb.Append("Observação " + indice.ToString() + " " + status + "\n");
                }

                m_status_imputacao = sb.ToString();
                return;
            }
            else
            {
                for (int i = 0; i < m_indices_valores_missing.Count; i++)
                {
                    indice = Convert.ToInt32(m_indices_valores_missing[i]);
                    m_valores_variavel[indice] = MediaVizinhosProximos(indice, i);

                    if (m_status_media_subgrupo[i] == 0)
                    {
                        status = "(usa valor fixo) = \t" + clt.Double2Texto(m_valores_variavel[indice], 8);
                    }
                    else
                    {
                        status = "(média de " + m_status_media_subgrupo[i] + " vizinhos) = \t" + clt.Double2Texto(m_valores_variavel[indice], 8);
                    }

                    sb.Append("Observação " + indice.ToString() + " " + status + "\n");
                }

                m_status_imputacao = sb.ToString();
            }
        }

        private double MediaVizinhosProximos(int indice, int numeracao_variavel)
        {
            double x_foco = m_vetor_variavel_x[indice];
            double y_foco = m_vetor_variavel_y[indice];

            object[,] aux = new object[m_vetor_variavel_x.GetLength(0) - m_indices_valores_missing.Count, 2];
            int row = 0;
            for (int i = 0; i < aux.GetLength(0); i++)
            {
                if (!m_indices_valores_missing.Contains(i))
                {
                    aux[row, 0] = Math.Pow(x_foco - m_vetor_variavel_x[i], 2.0) + Math.Pow(y_foco - m_vetor_variavel_y[i], 2.0);
                    aux[row, 1] = m_valores_variavel[i];
                    row++;
                }
            }

            clsUtilTools clt = new clsUtilTools();
            clt.SortByColumn(ref aux, aux, 0);

            int n = 0;
            double m = 0.0;
            double v = 0.0;
            for (int i = 0; i < aux.GetLength(0); i++)
            {
                v = Convert.ToDouble(aux[i, 1]);
                if (double.IsInfinity(v) || double.IsNaN(v) || double.IsNegativeInfinity(v)
                    || double.IsPositiveInfinity(v))
                { 
                }
                else
                {
                    m += v;
                    n++;
                }

                if (n >= m_num_vizinhos_proximos)
                {
                    break;
                }
            }

            if (n > 0)
            {
                m_status_media_subgrupo[numeracao_variavel] = n; // usando n observações para a média 
                return m / ((double)n);
            }
            else
            {
                m_status_media_subgrupo[numeracao_variavel] = 0; // usando valor fixo igual a zero
                return 0.0;
            }
        }

        #endregion

        #endregion

        #region imputacao com média geral

        private void ImputaMediaGeral()
        {
            clsUtilTools clt = new clsUtilTools();
            StringBuilder sb = new StringBuilder();
            sb.Append("========= Resultados da imputação \n\n");

            string mensagem = "média geral";

            double media_geral = MediaGeral();
            if (double.IsInfinity(media_geral) || double.IsNaN(media_geral)
                || double.IsNegativeInfinity(media_geral) || double.IsPositiveInfinity(media_geral))
            {
                media_geral = 0.0;
                mensagem = "usa valor fixo";
            }
            int indice = 0;
            for (int i = 0; i < m_indices_valores_missing.Count; i++)
            {
                indice = Convert.ToInt32(m_indices_valores_missing[i]);
                m_valores_variavel[indice] = media_geral;

                sb.Append("Observação " + indice.ToString() + " " 
                    + "(" + mensagem + ") = \t" + clt.Double2Texto(m_valores_variavel[indice], 8) + "\n");
            }

            m_status_imputacao = sb.ToString();
        }

        private double MediaGeral()
        {
            double m = 0.0;
            int n = 0;
            for (int i = 0; i < m_valores_variavel.GetLength(0); i++)
            {
                if (!m_indices_valores_missing.Contains(i))
                {
                    m += m_valores_variavel[i];
                    n++;
                }
            }
            if (n > 0) return m / ((double)n);
            else return double.NaN;
        }

        #endregion

        #region imputação com valor nulo

        private void ImputaValorNulo()
        {
            clsUtilTools clt = new clsUtilTools();
            StringBuilder sb = new StringBuilder();
            sb.Append("========= Resultados da imputação \n\n");

            int indice = 0;
            for (int i = 0; i < m_indices_valores_missing.Count; i++)
            {
                indice = Convert.ToInt32(m_indices_valores_missing[i]);
                m_valores_variavel[indice] = 0.0;

                sb.Append("Observação " + indice.ToString() + " "
                    + "(usa valor fixo) = \t" + clt.Double2Texto(m_valores_variavel[indice], 8) + "\n");
            }

            m_status_imputacao = sb.ToString();
        }

        #endregion

        #region imputação com valor fixo

        private void ImputaValorFixo()
        {
            clsUtilTools clt = new clsUtilTools();
            StringBuilder sb = new StringBuilder();
            sb.Append("========= Resultados da imputação \n\n");

            int indice = 0;
            for (int i = 0; i < m_indices_valores_missing.Count; i++)
            {
                indice = Convert.ToInt32(m_indices_valores_missing[i]);
                m_valores_variavel[indice] = m_valor_fixo;

                sb.Append("Observação " + indice.ToString() + " "
                    + "(usa valor fixo) = \t" + clt.Double2Texto(m_valores_variavel[indice], 8) + "\n");
            }

            m_status_imputacao = sb.ToString();
        }

        #endregion
    }

    #endregion
}
