using System;
using System.Collections;

using IpeaGeo.RegressoesEspaciais;
using MathNet.Numerics.Distributions;

namespace IpeaGeo.Modelagem
{
    public enum TipoDeMatching : int
    {
        NearestNeighbo,
        Gaussiano,
        Epanechnikov,
        Biweight,
        Triangular,
        Retangular,
        Stratification
    };

    public enum TipoDeAverage : int
    {
        ATT,
        ATE,
    };

    class BLogicPropensityScoreMatching : BLogicRegressaoDadosBinarios 
    {
        protected clsUtilTools m_clt = new clsUtilTools();

        public BLogicPropensityScoreMatching()
        {
            m_outbin[0] = "";
            m_outbin[1] = "";
        }

        protected double [,,] m_matrizDif;

        public void criaMatrizDif()
        {
            m_matrizDif = new double[nCat, nCat, 5];
        }

        public double[,,] matrizDif
        {
            get{return this.m_matrizDif;}
        }

        public void setMatrizDif(double x, int i, int j, int k)
        {
            m_matrizDif[i,j,k] = x;
        }

        protected string[] m_outbin = new string[2];
        public string [] outBin
        {
            get { return this.m_outbin; }
        }

        protected int m_nCat = 0;
        public int nCat
        {
            get { return this.m_nCat; }
            set { m_nCat = value; }
        }

        protected bool m_isCategoriaDistante = false;

        protected TipoDeMatching m_tipo_de_matching = TipoDeMatching.NearestNeighbo;
        public TipoDeMatching Matching
        {
            set { m_tipo_de_matching = value; }
        }

        protected string m_nome_matching = "";
        public string nome_matching
        {
            get { return this.m_nome_matching; }
            set { m_nome_matching = value; }
        }

        protected double m_ate;
        protected double m_bandwidth;
        public double Bandwidth
        {
            get { return this.m_bandwidth; }
            set { m_bandwidth = value; }
        }

        protected double m_VARate;

        protected double[] m_outcome;
        public double[] Outcome
        {
            set { m_outcome = value; }
        }

        protected TipoDeAverage cbxAT = TipoDeAverage.ATT;
        public TipoDeAverage AT
        {
            set { cbxAT = value; }
        }

        protected double[,] m_estrato;
        public double[,] Estrato
        {
            set { m_estrato = value; }
        }

        protected double[] m_participante;
        public double[] Participante
        {
            set { m_participante = value; }
        }
       
        private double GaussianKernel(double u)
        {
            return((1/Math.Sqrt(2*Math.PI))*Math.Exp(-(u*u)/2));
        }

         private double EpanechnikovKernel(double u)
        {
             if(Math.Abs(u)<=1) return((3.00/4.00)*(1-(u*u)));
             else return(0);
        }

        private double biweightKernel(double u)
        {
            if (Math.Abs(u) < 1) return (0.9375 * Math.Pow((1 - Math.Pow(u, 2)),2));
            else return 0;
        }

        private double triangularKernel(double u)
        {
            if (Math.Abs(u) < 1) return (1 - Math.Abs(u));
            else return 0;
        }

        protected double[] m_categorias;
        public double[] categorias
        {
            get { return m_categorias; }
        }

        public double calculaBandwidth(double[,] valoresPreditos)
        {
            double bd;
            double dv;
            double R;
            double A;
            int i;
            object[,] aux = new object[valoresPreditos.GetLength(0),1];

            //Calcula o desvio padro dos valores preditos
            dv = Math.Sqrt(m_clt.VarianciaColumnMatrix(valoresPreditos));

            // Calcula o interquartil dos valores preditos
            for(i = 0 ; i < valoresPreditos.GetLength(0) ; i++)
                aux[i,0] = valoresPreditos[i,0];

            m_clt.SortByColumn(ref aux, aux, 0);
            int q = valoresPreditos.GetLength(0)/4;
            R = (((double)aux[3 * q, 0]) - ((double)aux[q, 0]))/1.34;

            //calcula o minimo
            double[] vet = { dv, R };
            A = m_clt.Min(vet);
            bd = 0.9*A*(Math.Pow(valoresPreditos.GetLength(0),-0.2));

            return bd;
        }

        public string ChecaNumeroCategorias(int n)
        {
             string mensagem = "";
             double[,] y = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variaveis_dependentes);
             object[,] yo = new object[y.GetLength(0), 1];
             object[] categoriasO = new object[n];
             int i;

             m_categorias = new double[n];

             for (i = 0; i < yo.GetLength(0); i++)
             {
                 yo[i, 0] = y[i, 0];
             }

             if (!m_clt.ChecaLimiteCategorias(n, yo,out categoriasO))
             {
                 mensagem = "Variável resposta escolhida não é discreta. Deseja prosseguir definindo dados discretos, utilizando o quantil como valor de corte?";
             }
             else
                 for(i = 0 ; i < m_categorias.Length ; i++)
                 m_categorias[i] = (double)categoriasO[i];

             return mensagem;
        }

        public double[,] RespostaDiscreta(double[,] y, int n)
        {
             int i,j,k;
             
             double[,] corte = new double[(n+1),1];
             object[,] aux = new object[y.GetLength(0), 1];
             double[,] res = new double[y.GetLength(0), 1];

             for(i = 0; i < y.GetLength(0); i++)
                 aux[i, 0] = y[i, 0];

             for (i = 0; i < m_categorias.Length; i++)
                 m_categorias[i] = i;
             
             m_clt.SortByColumn(ref aux, aux,0);

             k = (int)(y.GetLength(0) / n);

             corte[n, 0] = (double)aux[(aux.GetLength(0) -1), 0];

             corte[0, 0] = (double)aux[0, 0];

             for (i = 1; i < n; i++)
                 corte[i, 0] = (double)aux[k * i, 0];

             for (i = 0; i < y.GetLength(0); i++)
             {
                 res[i, 0] = 0;
                 if (y[i, 0] == corte[n, 0])
                     res[i, 0] = n - 1;
                 else
                     for (j = 0; j < n; j++)
                     {
                         if (y[i, 0] >= corte[j, 0] && y[i, 0] < corte[(j + 1), 0])
                         {
                             res[i, 0] = j;
                             break;
                         }
                     }
             }
             return res;
         }

         public void imprimirResultadoBinario(string res, string est, int categoriaBase, int categoriaComparada)
         {
             m_outbin[0] += "\n\n============================================================================================================================\n";
             m_outbin[0] +="============================================================================================================================\n\n";
             m_outbin[0] += "Resultado da regrecao binaria entre os tratamentos " + m_clt.Double2Texto(m_categorias[categoriaBase]) + " e " + m_clt.Double2Texto(m_categorias[categoriaComparada]) + ":\n\n";
             m_outbin[0] += res;

             m_outbin[1] += "\n\n============================================================================================================================\n";
             m_outbin[1] +="============================================================================================================================\n\n";
             m_outbin[1] += "Resultado da regrecao binaria entre os tratamentos " + m_clt.Double2Texto(m_categorias[categoriaBase]) + " e " + m_clt.Double2Texto(m_categorias[categoriaComparada]) + ":\n\n";
             m_outbin[1] += est;
         }

         public void imprimirResultadoPropensityScore(string nomeOutcome)
         {
             int i,j;
             string[, ,] matrizImp = new string[matrizDif.GetLength(0), matrizDif.GetLength(1), matrizDif.GetLength(2)];
             string[] categoriasImp = new string[m_categorias.Length];
             int maxLenght = 0;

             for(i = 0 ; i < matrizDif.GetLength(0) ; i++)
                 for (j = 0; j < matrizDif.GetLength(1); j++)
                 {
                     matrizImp[i, j,0] = m_clt.Double2Texto(matrizDif[i, j,0], 6);
                     if (matrizImp[i, j,0].Length > maxLenght)
                         maxLenght = matrizImp[i, j,0].Length;

                     matrizImp[i, j,1] = "(" + m_clt.Double2Texto(matrizDif[i, j,1], 6) + ")";
                     if (matrizImp[i, j,1].Length > maxLenght)
                         maxLenght = matrizImp[i, j,1].Length;

                     matrizImp[i, j,2] = m_clt.Double2Texto(matrizDif[i, j,2], 6);
                     if (matrizImp[i, j,2].Length > maxLenght)
                         maxLenght = matrizImp[i, j,2].Length;

                     matrizImp[i, j,3] = m_clt.Double2Texto(matrizDif[i, j,3], 6);
                     if (matrizImp[i, j,3].Length > maxLenght)
                         maxLenght = matrizImp[i, j,3].Length;

                     matrizImp[i, j, 4] = m_clt.Double2Texto(matrizDif[i, j, 4], 0);
                     if (matrizImp[i, j, 4].Length > maxLenght)
                         maxLenght = matrizImp[i, j, 4].Length;
                 }

             for (i = 0; i < m_categorias.Length; i++)
                 categoriasImp[i] = m_clt.Double2Texto(m_categorias[i], 0);
                     
             string out_text = "============================================================================================================================\n\n";

             out_text += "Estimação do efeito do tratamento por propensity score matching\n\n";
             out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
             out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
             out_text += "Função de matching: " + nome_matching + "\n";
             if (nome_matching == "Gaussiano" || nome_matching == "Epanechnikov")
                 out_text += "Bandwidth: " + m_bandwidth.ToString() + "\n";

             out_text += "Outcome: " + nomeOutcome + "\n";
             out_text += "Variavel de tratamento: " + VariaveisDependentes[0] + "\n";
             for (i = 0; i < VariaveisIndependentes.GetLength(0); i++)
                 out_text += "Variavel explicativa " + (i+1).ToString() + ": " + VariaveisIndependentes[i] + "\n";
             out_text += "\n\n";

             if(m_isCategoriaDistante)
                 out_text += "Alguns tratamentos nao foram comparados pois os grupos amostrais correspondentes sao muitos distintos para o bandwidth escolhido\n\n";

             out_text += "Tratamento comparado\t";
             for (i = 0; i < nCat; i++)
                 out_text += "\t" + categoriasImp[i] + PreencheEspacos(maxLenght-categoriasImp[i].Length);
             out_text += "\n";

             out_text += "Tratamento base\n";
             for (i = 0; i < nCat; i++)
             {
                 out_text += categoriasImp[i] + PreencheEspacos(20-categoriasImp[i].Length) + "\t";
                 for (j = 0; j < nCat; j++)
                     out_text += "\t" + matrizImp[i, j, 0] + PreencheEspacos(maxLenght - matrizImp[i,j,0].Length);
                 out_text += "\n\t\t\t";
                 for (j = 0; j < nCat; j++)
                     out_text += "\t" + matrizImp[i, j, 1] + PreencheEspacos(maxLenght - matrizImp[i,j,1].Length);
                 out_text += "\n\t\t\t";
                 for (j = 0; j < nCat; j++)
                     out_text += "\t" + matrizImp[i, j, 2] + PreencheEspacos(maxLenght - matrizImp[i,j,2].Length);
                 out_text += "\n\t\t\t";
                 for (j = 0; j < nCat; j++)
                     out_text += "\t" + matrizImp[i, j, 3] + PreencheEspacos(maxLenght - matrizImp[i,j,3].Length);
                 out_text += "\n\t\t\t";
                 for (j = 0; j < nCat; j++)
                     out_text += "\t" + matrizImp[i, j, 4] + PreencheEspacos(maxLenght - matrizImp[i,j,4].Length);
                 out_text += "\n\n";
             }

             out_text += "Em cada cruzamento, os valores representam:\n";
             out_text += "Linha 1 : Diferença de medias entre o grupo base e o grupo comparado ajustada\n";
             out_text += "Linha 2 : Desvio padrao da diferenca de medias\n";
             out_text += "Linha 3 : Teste t da diferenca de medias\n";
             out_text += "Linha 4 : p-valor bicaudal da diferença de medias\n";
             out_text += "Linha 5 : numero de comparacoes realizadas\n\n";

             this.m_output_text = out_text;
         }
        
         public void imprimirResultadoPropensityScoreATE(string nomeOutcome)
         {
             Normal norm = new Normal(0, 1);
             int i, j;
             string[, ,] matrizImp = new string[matrizDif.GetLength(0), matrizDif.GetLength(1), matrizDif.GetLength(2)];
             string[] categoriasImp = new string[m_categorias.Length];
             int maxLenght = 0;
             double m_corrigido = 0.0;
             double v_corrigido = 0.0;
             double t_corrigido = 0.0;
             double p_corrigido = 0.0;
             for (i = 0; i < matrizDif.GetLength(0); i++)
                 for (j = 0; j < matrizDif.GetLength(1); j++)
                 {
                     if (i != j)
                     {
                         m_corrigido = (matrizDif[i, j, 0] * matrizDif[i, j, 4] - matrizDif[j, i, 0] * matrizDif[j, i, 4]) / (matrizDif[j, i, 4] + matrizDif[i, j, 4]);
                         v_corrigido = Math.Sqrt(matrizDif[i, j, 1] + matrizDif[j, i, 1]);
                         t_corrigido = m_corrigido / v_corrigido;
                         p_corrigido = (1.0 - norm.CumulativeDistribution(Math.Abs(t_corrigido))) * 2;
                     }
                     else
                     {
                         m_corrigido = 0.0;
                         v_corrigido = 0.0;
                         t_corrigido = 0.0;
                         p_corrigido = 0.0;
                     }
                         matrizImp[i, j, 0] = m_clt.Double2Texto(m_corrigido, 6);
                         if (matrizImp[i, j, 0].Length > maxLenght)
                             maxLenght = matrizImp[i, j, 0].Length;
                         
                         matrizImp[i, j, 1] = "(" + m_clt.Double2Texto(v_corrigido, 6) + ")";
                         if (matrizImp[i, j, 1].Length > maxLenght)
                             maxLenght = matrizImp[i, j, 1].Length;
                         
                         matrizImp[i, j, 2] = m_clt.Double2Texto((matrizDif[i, j, 1] + matrizDif[j, i, 1]), 6);
                         if (matrizImp[i, j, 2].Length > maxLenght)
                             maxLenght = matrizImp[i, j, 2].Length;
                         
                         matrizImp[i, j, 3] = m_clt.Double2Texto(p_corrigido, 6);
                         if (matrizImp[i, j, 3].Length > maxLenght)
                             maxLenght = matrizImp[i, j, 3].Length;

                         matrizImp[i, j, 4] = m_clt.Double2Texto(matrizDif[i, j, 4], 0);
                         if (matrizImp[i, j, 4].Length > maxLenght)
                             maxLenght = matrizImp[i, j, 4].Length;
                 }

             for (i = 0; i < m_categorias.Length; i++)
                 categoriasImp[i] = m_clt.Double2Texto(m_categorias[i], 0);

             string out_text = "============================================================================================================================\n\n";

             out_text += "Estimação do efeito do tratamento por propensity score matching\n\n";
             out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
             out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";
             out_text += "Função de matching: " + nome_matching + "\n";
             if (nome_matching == "Gaussiano" || nome_matching == "Epanechnikov")
                 out_text += "Bandwidth: " + m_bandwidth.ToString() + "\n";

             out_text += "Outcome: " + nomeOutcome + "\n";
             out_text += "Variavel de tratamento: " + VariaveisDependentes[0] + "\n";
             for (i = 0; i < VariaveisIndependentes.GetLength(0); i++)
                 out_text += "Variavel de explicativa " + (i + 1).ToString() + ": " + VariaveisIndependentes[i] + "\n";
             out_text += "\n\n";

             if (m_isCategoriaDistante)
                 out_text += "Alguns tratamentos nao foram comparados pois os grupos amostrais correspondentes sao muitos distintos para o bandwidth escolhido\n\n";

             out_text += "Tratamento comparado\t";
             for (i = 0; i < nCat; i++)
                 out_text += "\t" + categoriasImp[i] + PreencheEspacos(maxLenght - categoriasImp[i].Length);
             out_text += "\n";

             out_text += "Tratamento base\n";
             for (i = 0; i < nCat; i++)
             {
                 out_text += categoriasImp[i] + PreencheEspacos(20 - categoriasImp[i].Length) + "\t";
                 for (j = 0; j < nCat; j++)
                     out_text += "\t" + matrizImp[i, j, 0] + PreencheEspacos(maxLenght - matrizImp[i, j, 0].Length);
                 out_text += "\n\t\t\t";
                 for (j = 0; j < nCat; j++)
                     out_text += "\t" + matrizImp[i, j, 1] + PreencheEspacos(maxLenght - matrizImp[i, j, 1].Length);
                 out_text += "\n\t\t\t";
                 for (j = 0; j < nCat; j++)
                     out_text += "\t" + matrizImp[i, j, 2] + PreencheEspacos(maxLenght - matrizImp[i, j, 2].Length);
                 out_text += "\n\t\t\t";
                 for (j = 0; j < nCat; j++)
                     out_text += "\t" + matrizImp[i, j, 3] + PreencheEspacos(maxLenght - matrizImp[i, j, 3].Length);
                 out_text += "\n\t\t\t";
                 for (j = 0; j < nCat; j++)
                     out_text += "\t" + matrizImp[i, j, 4] + PreencheEspacos(maxLenght - matrizImp[i, j, 4].Length);
                 out_text += "\n\n";
             }

             out_text += "Em cada cruzamento, os valores representam:\n";
             out_text += "Linha 1 : Diferença de medias entre o grupo base e o grupo comparado ajustada\n";
             out_text += "Linha 2 : Desvio padrao da diferenca de medias\n";
             out_text += "Linha 3 : Teste t da diferenca de medias\n";
             out_text += "Linha 4 : p-valor bicaudal da diferença de medias\n";
             out_text += "Linha 5 : numero de comparacoes realizadas\n\n";

             this.m_output_text = out_text;
         }

         private double[] AverageTreatmentEffect(ArrayList arLista, out int numeroComparacoes)
         {
            double dblSomaDasDiferencas = 0;
            double dblSomaDasDiferencasAoQuadrado = 0;

            //Passo 1: Para cada participante calcular a soma das diferenças e a soma dos quadrados das diferenças.
            for (int i = 0; i < arLista.Count; i++)
            {
                ArrayList arListaMatching = (ArrayList)arLista[i];
                double somaOutcomeNaoParticipa = 0;
                double outcomeParticipa = 0;
                double diferenca = 0;

                //Indice do Participante na Base
                int iParticipa = (int)arListaMatching[0];

                //Soma do Outcome para os participantes
                outcomeParticipa = m_outcome[iParticipa];

                double somaPeso = 0;
                for (int j = 0; j < arListaMatching.Count; j++)
                {
                    double[] dblNaoParticipa = (double[]) arListaMatching[j];
                    //Indice do Não Participante na Base
                    int iNaoParticipa = (int)dblNaoParticipa[0];
                    //Propensão a participar do não participante
                    double peso = dblNaoParticipa[1];
                    somaPeso += peso;

                    //Soma do Outcome para os participantes ponderado pela propensão
                    somaOutcomeNaoParticipa += peso * m_outcome[iNaoParticipa];
                }
                diferenca = outcomeParticipa - (somaOutcomeNaoParticipa/somaPeso);
                dblSomaDasDiferencas += diferenca;
                dblSomaDasDiferencasAoQuadrado += diferenca*diferenca;
            }

            int n = arLista.Count;
            numeroComparacoes = n;
            double dblATE = dblSomaDasDiferencas / n;
            double dblVarianciaATE;
            if(n==1)
                dblVarianciaATE = 0;
            else
                dblVarianciaATE = ((dblSomaDasDiferencasAoQuadrado) - n*(dblATE * dblATE))*(1.0/(n-1));

            double[] resultados = new double[] { dblATE, dblVarianciaATE };
            return (resultados);
         }

         public void GeraMatching(double[,] valoresPreditos, int categoriaBase, int categoriaComp)
         {
             ArrayList arrayMatching = new ArrayList();
             clsUtilTools clt = new clsUtilTools();
             double valor_t;
             double valor_p;
             int iParticipa = 0;
             int inParticipa = 0;
             int numeroComparacoes =0;
             int M = 5;

             for (int i = 0; i < valoresPreditos.GetLength(0); i++) if (m_participante[i] == 1) iParticipa++;
             
             //Separa em dois Grupos: Participantes e Não Participantes
             int iContaParticipa = 0;
             int iContaNaoParticipa = 0;

             int[] intParticipantes = new int[iParticipa];
             int[] intNaoParticipantes = new int[valoresPreditos.GetLength(0) - iParticipa];
             for (int i = 0; i < valoresPreditos.GetLength(0); i++)
             {
                 if (m_participante[i] == 1)
                 {
                     intParticipantes[iContaParticipa] = i;
                     iContaParticipa++;
                 }
                 else
                 {
                     intNaoParticipantes[iContaNaoParticipa] = i;
                     iContaNaoParticipa++;
                 }
             }

             double tau = 0.0;
             double v_tau = 0.0;
             
             //Estatísticas dos participantes e não participantes
             double m_part = 0.0;
             double v_part = 0.0;
             double m_npart = 0.0;
             double v_npart = 0.0;
             double[,] VetorSomaPeso = new double[intNaoParticipantes.Length, 1];
             double somapesoquadrado = 0.0;
             double estrato_v_tau = 0.0;
             double media_n_part = 0.0;
             double media_part = 0.0;

             double m_tot = 0.0;
             
             //Media
             for (int i = 0; i < valoresPreditos.GetLength(0); i++)
             {
                 m_tot += m_outcome[i] / valoresPreditos.Length;
                 if (m_participante[i] == 1)
                 {
                     m_part += m_outcome[i] / intParticipantes.Length;
                 }
                 else
                 {
                     m_npart += m_outcome[i] / intNaoParticipantes.Length;
                 }
             }
             
             //Variancia
             double v_tot = 0.0;
             for (int i = 0; i < valoresPreditos.GetLength(0); i++)
             {
                 v_tot += Math.Pow((m_outcome[i] - m_tot), 2) / (valoresPreditos.Length - 1);
                 if (m_participante[i] == 1)
                 {
                     v_part += Math.Pow((m_outcome[i] - m_part), 2) / (intParticipantes.Length - 1);
                 }
                 else
                 {
                     v_npart += Math.Pow((m_outcome[i] - m_npart), 2) / (intNaoParticipantes.Length - 1);
                 }
             }

             double[,] m_outcome_band = new double[m_outcome.Length, 1];
             for (int i = 0; i < m_outcome.Length; i++)
             {
                 m_outcome_band[i, 0] = m_outcome[i];
             }

             //Passo 1: Define o tipo de Matching
             switch (m_tipo_de_matching)
             {
                 case TipoDeMatching.NearestNeighbo:
                     nome_matching = "Nearest Neighbor";
                     if (m_bandwidth == 0) m_bandwidth = calculaBandwidth(m_outcome_band);

                     for (int i = 0; i < intParticipantes.Length; i++)
                     {
                         media_n_part = 0.0;
                         ArrayList arLista = new ArrayList();
                         double[,] difNaoParticipa = new double[intNaoParticipantes.Length, 1];
                         double[,] peso = new double[intNaoParticipantes.Length, 1];
                         arLista.Add(intParticipantes[i]);
                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             difNaoParticipa[j, 0] = Math.Abs(valoresPreditos[intParticipantes[i], 0] - valoresPreditos[intNaoParticipantes[j], 0]);
                         }
                         double media_dif = clt.Mean(difNaoParticipa);
                         double min_dif = clt.Min(difNaoParticipa);
                         double max_dif = clt.Max(difNaoParticipa);
                         double desv_dif = clt.Despadca(difNaoParticipa)[0, 0];
                         double[,] table_dif = new double[0, 0];
                         clt.FrequencyTable(ref table_dif, difNaoParticipa);
                         double[,] sorteado = clt.SortcDoubleArray(table_dif);
                         int contador_M = 0;
                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             for (int k = 0; k < M; k++)
                             {
                                 if (difNaoParticipa[j, 0] == sorteado[k, 0] & contador_M <= M)
                                 {
                                     peso[j, 0] = 1;
                                     contador_M += 1;
                                 }
                             }
                         }
                         double somapeso = clt.Sum(peso);
                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             VetorSomaPeso[j, 0] += peso[j, 0] / somapeso;
                         }
                         
                         // Estimando valor nos não participantes do participante "i"
                         for (int j = 0; j < difNaoParticipa.GetLength(0); j++)
                         {
                             media_n_part += m_outcome[intNaoParticipantes[j]] * peso[j, 0] / somapeso;
                         }
                         
                         // Estatistica do teste do participante "i"
                         tau += (m_outcome[intParticipantes[i]] - media_n_part) / intParticipantes.Length;
                     }
                     
                     for (int i = 0; i < intNaoParticipantes.Length; i++)
                     {
                         somapesoquadrado += Math.Pow(VetorSomaPeso[i, 0], 2.0);
                     }

                     //variancia especifica caso especifico de ATT ou ATE. Para o cas do ATE, o erro padrão e a estatísticas são somadas no output.
                     //Para o cálculo de medida de erro associada ao PSM, é necessário fazer suposições sobre a variabilidade
                     double v_r = 0.0;
                     if (cbxAT == TipoDeAverage.ATT)
                     {
                         for (int i = 0; i < intNaoParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow((1 - m_participante[i]) * (VetorSomaPeso[i, 0]), 2) * (v_npart)) / Math.Pow(intParticipantes.Length, 2);
                         }

                         for (int i = 0; i < intParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow((m_participante[i]), 2) * (v_part)) / Math.Pow(intParticipantes.Length, 2);
                         }

                         //REFERÊNCIA DADA PELO ALEXANDRE
                         //v_tau = v_part / intParticipantes.Length + (v_npart / Math.Pow(intParticipantes.Length, 2.0)) * somapesoquadrado;
                     }
                     else
                     {
                         for (int i = 0; i < intNaoParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow(1 + (VetorSomaPeso[i, 0]), 2) * (v_npart)) / Math.Pow(valoresPreditos.Length, 2);
                         }
                     }
                     v_tau = v_r;
                     break;

                 case TipoDeMatching.Epanechnikov:
                     nome_matching = "Epanechnikov";
                     if (m_bandwidth == 0) m_bandwidth = calculaBandwidth(m_outcome_band);

                     for (int i = 0; i < intParticipantes.Length; i++)
                     {
                         media_n_part = 0.0;
                         ArrayList arLista = new ArrayList();
                         double[,] difNaoParticipa = new double[intNaoParticipantes.Length, 1];
                         double[,] peso = new double[intNaoParticipantes.Length, 1];
                         arLista.Add(intParticipantes[i]);
                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             difNaoParticipa[j, 0] = valoresPreditos[intParticipantes[i], 0] - valoresPreditos[intNaoParticipantes[j], 0];
                         }
                         double media_dif = clt.Mean(difNaoParticipa);
                         double min_dif = clt.Min(difNaoParticipa);
                         double max_dif = clt.Max(difNaoParticipa);
                         double desv_dif = clt.Despadca(difNaoParticipa)[0, 0];
                         double[,] table_dif = new double[0, 0];

                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             //difNaoParticipa[j, 0] = (difNaoParticipa[j, 0] - min_dif) / (max_dif - min_dif);
                             difNaoParticipa[j, 0] = (difNaoParticipa[j, 0]) / Bandwidth;
                             peso[j, 0] = EpanechnikovKernel(difNaoParticipa[j, 0]);
                         }
                         double somapeso = clt.Sum(peso);
                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             VetorSomaPeso[j, 0] += peso[j, 0] / somapeso;
                         }
                         
                         // Estimando valor nos não participantes do participante "i"
                         for (int j = 0; j < difNaoParticipa.GetLength(0); j++)
                         {
                             media_n_part += m_outcome[intNaoParticipantes[j]] * peso[j, 0] / somapeso;
                         }
                         
                         // Estatistica do teste do participante "i"
                         tau += (m_outcome[intParticipantes[i]] - media_n_part) / intParticipantes.Length;
                     }
                     for (int i = 0; i < intNaoParticipantes.Length; i++)
                     {
                         somapesoquadrado += Math.Pow(VetorSomaPeso[i, 0], 2.0);
                     }

                     //variancia especifica caso especifico de ATT ou ATE. Para o cas do ATE, o erro padrão e a estatísticas são somadas no output.
                     //Para o cálculo de medida de erro associada ao PSM, é necessário fazer suposições sobre a variabilidade
                     v_r = 0.0;
                     if (cbxAT == TipoDeAverage.ATT)
                     {
                         for (int i = 0; i < intNaoParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow((1 - m_participante[i]) * (VetorSomaPeso[i, 0]), 2) * (v_npart)) / Math.Pow(intParticipantes.Length, 2);
                         }

                         for (int i = 0; i < intParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow((m_participante[i]), 2) * (v_part)) / Math.Pow(intParticipantes.Length, 2);
                         }

                         //REFERÊNCIA DADA PELO ALEXANDRE
                         //v_tau = v_part / intParticipantes.Length + (v_npart / Math.Pow(intParticipantes.Length, 2.0)) * somapesoquadrado;
                     }
                     else
                     {
                         for (int i = 0; i < intNaoParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow(1 + (VetorSomaPeso[i, 0]), 2) * (v_npart)) / Math.Pow(valoresPreditos.Length, 2);
                         }
                     }
                     v_tau = v_r;
                     break;

                 case TipoDeMatching.Gaussiano:
                     nome_matching = "Gaussiano";
                     if (m_bandwidth == 0) m_bandwidth = calculaBandwidth(m_outcome_band);
                     for (int i = 0; i < intParticipantes.Length; i++)
                     {
                         media_n_part = 0.0;
                         ArrayList arLista = new ArrayList();
                         double[,] difNaoParticipa = new double[intNaoParticipantes.Length, 1];
                         double[,] peso = new double[intNaoParticipantes.Length, 1];
                         arLista.Add(intParticipantes[i]);
                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             difNaoParticipa[j, 0] = valoresPreditos[intParticipantes[i], 0] - valoresPreditos[intNaoParticipantes[j], 0];
                         }
                         double media_dif = clt.Mean(difNaoParticipa);
                         double min_dif = clt.Min(difNaoParticipa);
                         double max_dif = clt.Max(difNaoParticipa);
                         double desv_dif = clt.Despadca(difNaoParticipa)[0, 0];
                         double[,] table_dif = new double[0, 0];

                         for (int j = 0; j < difNaoParticipa.GetLength(0); j++)
                         {
                             //difNaoParticipa[j, 0] = (difNaoParticipa[j, 0] - media_dif) / desv_dif;
                             difNaoParticipa[j, 0] = (difNaoParticipa[j, 0]) / Bandwidth;
                             peso[j, 0] = this.GaussianKernel(difNaoParticipa[j, 0]);
                         }
                         double somapeso = clt.Sum(peso);
                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             VetorSomaPeso[j, 0] += peso[j, 0] / somapeso;
                         }
                         // Estimando valor nos não participantes do participante "i"
                         for (int j = 0; j < difNaoParticipa.GetLength(0); j++)
                         {
                             media_n_part += m_outcome[intNaoParticipantes[j]] * peso[j, 0] / somapeso;
                         }
                         // Estatistica do teste do participante "i"
                         tau += (m_outcome[intParticipantes[i]] - media_n_part) / intParticipantes.Length;
                     }
                     for (int i = 0; i < intNaoParticipantes.Length; i++)
                     {
                         somapesoquadrado += Math.Pow(VetorSomaPeso[i, 0], 2.0);
                     }

                     //variancia especifica caso especifico de ATT ou ATE. Para o cas do ATE, o erro padrão e a estatísticas são somadas no output.
                     //Para o cálculo de medida de erro associada ao PSM, é necessário fazer suposições sobre a variabilidade
                     v_r = 0.0;
                     if (cbxAT == TipoDeAverage.ATT)
                     {
                         for (int i = 0; i < intNaoParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow((1 - m_participante[i]) * (VetorSomaPeso[i, 0]), 2) * (v_npart)) / Math.Pow(intParticipantes.Length, 2);
                         }

                         for (int i = 0; i < intParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow((m_participante[i]), 2) * (v_part)) / Math.Pow(intParticipantes.Length, 2);
                         }

                         //REFERÊNCIA DADA PELO ALEXANDRE
                         //v_tau = v_part / intParticipantes.Length + (v_npart / Math.Pow(intParticipantes.Length, 2.0)) * somapesoquadrado;
                     }
                     else
                     {
                         for (int i = 0; i < intNaoParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow(1 + (VetorSomaPeso[i, 0]), 2) * (v_npart)) / Math.Pow(valoresPreditos.Length, 2);
                         }
                     }
                     v_tau = v_r;
                     break;

                 case TipoDeMatching.Triangular:
                     nome_matching = "Triangular";
                     if (m_bandwidth == 0) m_bandwidth = calculaBandwidth(m_outcome_band);
                     for (int i = 0; i < intParticipantes.Length; i++)
                     {
                         media_n_part = 0.0;
                         ArrayList arLista = new ArrayList();
                         double[,] difNaoParticipa = new double[intNaoParticipantes.Length, 1];
                         double[,] peso = new double[intNaoParticipantes.Length, 1];
                         arLista.Add(intParticipantes[i]);
                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             difNaoParticipa[j, 0] = valoresPreditos[intParticipantes[i], 0] - valoresPreditos[intNaoParticipantes[j], 0];
                         }
                         double media_dif = clt.Mean(difNaoParticipa);
                         double min_dif = clt.Min(difNaoParticipa);
                         double max_dif = clt.Max(difNaoParticipa);
                         double desv_dif = clt.Despadca(difNaoParticipa)[0, 0];
                         double[,] table_dif = new double[0, 0];
                         for (int j = 0; j < difNaoParticipa.GetLength(0); j++)
                         {
                             //difNaoParticipa[j, 0] = (difNaoParticipa[j, 0] - min_dif) / (max_dif - min_dif);
                             difNaoParticipa[j, 0] = (difNaoParticipa[j, 0]) / Bandwidth;
                             peso[j, 0] = triangularKernel(difNaoParticipa[j, 0]);
                         }
                         double somapeso = clt.Sum(peso);
                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             VetorSomaPeso[j, 0] += peso[j, 0] / somapeso;
                         }
                         // Estimando valor nos não participantes do participante "i"
                         for (int j = 0; j < difNaoParticipa.GetLength(0); j++)
                         {
                             media_n_part += m_outcome[intNaoParticipantes[j]] * peso[j, 0] / somapeso;
                         }
                         // Estatistica do teste do participante "i"
                         tau += (m_outcome[intParticipantes[i]] - media_n_part) / intParticipantes.Length;
                     }
                     for (int i = 0; i < intNaoParticipantes.Length; i++)
                     {
                         somapesoquadrado += Math.Pow(VetorSomaPeso[i, 0], 2.0);
                     }

                     //variancia especifica caso especifico de ATT ou ATE. Para o cas do ATE, o erro padrão e a estatísticas são somadas no output.
                     //Para o cálculo de medida de erro associada ao PSM, é necessário fazer suposições sobre a variabilidade
                     v_r = 0.0;
                     if (cbxAT == TipoDeAverage.ATT)
                     {
                         for (int i = 0; i < intNaoParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow((1 - m_participante[i]) * (VetorSomaPeso[i, 0]), 2) * (v_npart)) / Math.Pow(intParticipantes.Length, 2);
                         }

                         for (int i = 0; i < intParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow((m_participante[i]), 2) * (v_part)) / Math.Pow(intParticipantes.Length, 2);
                         }

                         //REFERÊNCIA DADA PELO ALEXANDRE
                         //v_tau = v_part / intParticipantes.Length + (v_npart / Math.Pow(intParticipantes.Length, 2.0)) * somapesoquadrado;
                     }
                     else
                     {
                         for (int i = 0; i < intNaoParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow(1 + (VetorSomaPeso[i, 0]), 2) * (v_npart)) / Math.Pow(valoresPreditos.Length, 2);
                         }
                     }
                     v_tau = v_r;
                     break;

                 case TipoDeMatching.Retangular:
                     nome_matching = "Retangular";
                     if (m_bandwidth == 0) m_bandwidth = calculaBandwidth(m_outcome_band);
                     for (int i = 0; i < intParticipantes.Length; i++)
                     {
                         media_n_part = 0.0;
                         ArrayList arLista = new ArrayList();
                         double[,] difNaoParticipa = new double[intNaoParticipantes.Length, 1];
                         double[,] peso = new double[intNaoParticipantes.Length, 1];
                         arLista.Add(intParticipantes[i]);
                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             difNaoParticipa[j, 0] = valoresPreditos[intParticipantes[i], 0] - valoresPreditos[intNaoParticipantes[j], 0];
                         }
                         double media_dif = clt.Mean(difNaoParticipa);
                         double min_dif = clt.Min(difNaoParticipa);
                         double max_dif = clt.Max(difNaoParticipa);
                         double desv_dif = clt.Despadca(difNaoParticipa)[0, 0];
                         double[,] table_dif = new double[0, 0];

                         for (int j = 0; j < difNaoParticipa.GetLength(0); j++)
                         {
                             //difNaoParticipa[j, 0] = (difNaoParticipa[j, 0] - min_dif) / (max_dif - min_dif);
                             difNaoParticipa[j, 0] = (difNaoParticipa[j, 0]) / Bandwidth;
                             peso[j, 0] = triangularKernel(difNaoParticipa[j, 0]);
                         }
                         double somapeso = clt.Sum(peso);
                         for (int j =0; j < intNaoParticipantes.Length; j++)
                         {
                             VetorSomaPeso[j, 0] += peso[j, 0] / somapeso;
                         }
                         // Estimando valor nos não participantes do participante "i"
                         for (int j = 0; j < difNaoParticipa.GetLength(0); j++)
                         {
                             media_n_part += m_outcome[intNaoParticipantes[j]] * peso[j, 0] / somapeso;
                         }
                         // Estatistica do teste do participante "i"
                         tau += (m_outcome[intParticipantes[i]] - media_n_part) / intParticipantes.Length;
                     }
                     for (int i = 0; i < intNaoParticipantes.Length; i++)
                     {
                         somapesoquadrado += Math.Pow(VetorSomaPeso[i, 0], 2.0);
                     }

                     //variancia especifica caso especifico de ATT ou ATE. Para o cas do ATE, o erro padrão e a estatísticas são somadas no output.
                     //Para o cálculo de medida de erro associada ao PSM, é necessário fazer suposições sobre a variabilidade
                     v_r = 0.0;
                     if (cbxAT == TipoDeAverage.ATT)
                     {
                         for (int i = 0; i < intNaoParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow((1 - m_participante[i]) * (VetorSomaPeso[i, 0]), 2) * (v_npart)) / Math.Pow(intParticipantes.Length, 2);
                         }

                         for (int i = 0; i < intParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow((m_participante[i]), 2) * (v_part)) / Math.Pow(intParticipantes.Length, 2);
                         }

                         //REFERÊNCIA DADA PELO ALEXANDRE
                         //v_tau = v_part / intParticipantes.Length + (v_npart / Math.Pow(intParticipantes.Length, 2.0)) * somapesoquadrado;
                     }
                     else
                     {
                         for (int i = 0; i < intNaoParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow(1 + (VetorSomaPeso[i, 0]), 2) * (v_npart)) / Math.Pow(valoresPreditos.Length, 2);
                         }
                     }
                     v_tau = v_r;
                     break;


                 case TipoDeMatching.Biweight:
                     nome_matching = "Biweight";
                     if (m_bandwidth == 0) m_bandwidth = calculaBandwidth(m_outcome_band);
                     for (int i = 0; i < intParticipantes.Length; i++)
                     {
                         media_n_part = 0.0;
                         ArrayList arLista = new ArrayList();
                         double[,] difNaoParticipa = new double[intNaoParticipantes.Length, 1];
                         double[,] peso = new double[intNaoParticipantes.Length, 1];
                         arLista.Add(intParticipantes[i]);
                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             difNaoParticipa[j, 0] = valoresPreditos[intParticipantes[i], 0] - valoresPreditos[intNaoParticipantes[j], 0];
                         }
                         double media_dif = clt.Mean(difNaoParticipa);
                         double min_dif = clt.Min(difNaoParticipa);
                         double max_dif = clt.Max(difNaoParticipa);
                         double desv_dif = clt.Despadca(difNaoParticipa)[0, 0];
                         double[,] table_dif = new double[0, 0];
                         for (int j = 0; j < difNaoParticipa.GetLength(0); j++)
                         {
                             //difNaoParticipa[j, 0] = (difNaoParticipa[j, 0] - min_dif) / (max_dif - min_dif);
                             difNaoParticipa[j, 0] = (difNaoParticipa[j, 0]) / Bandwidth;
                             peso[j, 0] = biweightKernel(difNaoParticipa[j, 0]);
                         }
                         double somapeso = clt.Sum(peso);
                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             VetorSomaPeso[j, 0] += peso[j, 0] / somapeso;
                         }
                         // Estimando valor nos não participantes do participante "i"
                         for (int j = 0; j < difNaoParticipa.GetLength(0); j++)
                         {
                             media_n_part += m_outcome[intNaoParticipantes[j]] * peso[j, 0] / somapeso;
                         }
                         // Estatistica do teste do participante "i"
                         tau += (m_outcome[intParticipantes[i]] - media_n_part) / intParticipantes.Length;
                     }
                     for (int i = 0; i < intNaoParticipantes.Length; i++)
                     {
                         somapesoquadrado += Math.Pow(VetorSomaPeso[i, 0], 2.0);
                     }

                     //variancia especifica caso especifico de ATT ou ATE. Para o cas do ATE, o erro padrão e a estatísticas são somadas no output.
                     //Para o cálculo de medida de erro associada ao PSM, é necessário fazer suposições sobre a variabilidade
                     v_r = 0.0;
                     if (cbxAT == TipoDeAverage.ATT)
                     {
                         for (int i = 0; i < intNaoParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow((1 - m_participante[i]) * (VetorSomaPeso[i, 0]), 2) * (v_npart)) / Math.Pow(intParticipantes.Length, 2);
                         }

                         for (int i = 0; i < intParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow((m_participante[i]), 2) * (v_part)) / Math.Pow(intParticipantes.Length, 2);
                         }

                         //REFERÊNCIA DADA PELO ALEXANDRE
                         //v_tau = v_part / intParticipantes.Length + (v_npart / Math.Pow(intParticipantes.Length, 2.0)) * somapesoquadrado;
                     }
                     else
                     {
                         for (int i = 0; i < intNaoParticipantes.Length; i++)
                         {
                             v_r += (Math.Pow(1 + (VetorSomaPeso[i, 0]), 2) * (v_npart)) / Math.Pow(valoresPreditos.Length, 2);
                         }
                     }
                     v_tau = v_r;
                     break;

                 case TipoDeMatching.Stratification:
                     nome_matching = "Stratification";
                     iParticipa = 0;
                     inParticipa = 0;
                     numeroComparacoes = 0;
                     //Criando matriz com os vetores de cada grupo
                     double[,] matrizgrupo = new double[0, 0];
                     clt.FrequencyTable(ref matrizgrupo, m_estrato);

                     int nt = intParticipantes.Length;
                     int nc = intNaoParticipantes.Length;
                     for (int s = 0; s < matrizgrupo.GetLength(0); s++)
                     {
                         for (int i = 0; i < valoresPreditos.GetLength(0); i++) if (m_participante[i] == 1 && m_estrato[i, 0] == matrizgrupo[s, 0]) iParticipa++;
                         for (int i = 0; i < valoresPreditos.GetLength(0); i++) if (m_participante[i] == 0 && m_estrato[i, 0] == matrizgrupo[s, 0]) inParticipa++;
                         //Separa em dois Grupos: Participantes e Não Participantes
                         intParticipantes = new int[iParticipa];
                         intNaoParticipantes = new int[inParticipa];
                         iContaParticipa = 0;
                         iContaNaoParticipa = 0;
                         for (int i = 0; i < valoresPreditos.GetLength(0); i++)
                         {
                             if (m_estrato[i, 0] == matrizgrupo[s, 0])
                             {
                                 if (m_participante[i] == 1)
                                 {
                                     intParticipantes[iContaParticipa] = i;
                                     iContaParticipa++;
                                 }
                                 else
                                 {
                                     intNaoParticipantes[iContaNaoParticipa] = i;
                                     iContaNaoParticipa++;
                                 }
                             }
                         }
                         media_n_part = 0.0;
                         media_part = 0.0;
                         for (int i = 0; i < intParticipantes.Length; i++)
                         {
                             media_part += m_outcome[intParticipantes[i]] / iParticipa;
                         }

                         for (int j = 0; j < intNaoParticipantes.GetLength(0); j++)
                         {
                             media_n_part += m_outcome[intNaoParticipantes[j]] / inParticipa;
                         }
                         tau += ((media_part - media_n_part) * (matrizgrupo[s, 1])) / valoresPreditos.Length;
                         estrato_v_tau += (double)((double)(iParticipa * iParticipa) / (double)(nt * inParticipa)) * (v_npart);
                     }
                     v_tau = (1.0 / intParticipantes.Length) * (v_part + estrato_v_tau);
                     break;

                 default:
                     //TODO: Qual e o tipo de matching default?
                     nome_matching = "Default";
                     //Para cada participante calculamos a lista de pesos em relação aos participantes.
                     for (int i = 0; i < intParticipantes.Length; i++)
                     {
                         ArrayList arLista = new ArrayList();
                         double[] dblNaoParticipa = new double[2];
                         for (int j = 0; j < intNaoParticipantes.Length; j++)
                         {
                             double peso = valoresPreditos[intNaoParticipantes[j], 0];

                             //Guarda a observação predita e seus Matchings
                             arLista.Add(intParticipantes[i]);
                             dblNaoParticipa[0] = intNaoParticipantes[j];
                             dblNaoParticipa[1] = peso;
                             arLista.Add(dblNaoParticipa);
                         }

                         //Guarda o grupo no ArrayGeral
                         arrayMatching.Add(arLista);
                     }
                     break;
             }

             //Passo 2: Calcula a média das diferenças.
             double[] resultados = AverageTreatmentEffect(arrayMatching, out numeroComparacoes);
             m_ate = resultados[0];
             m_VARate = resultados[1];
             //Passo 2*: Método Artigo: Estimation of average treatment effects based on propensity scores.
             m_ate = tau;
             //TODO: VERIFICAR COM PEDRO OU ALEX
             m_VARate = v_tau;

             #region testes

             double desvio_padrao = 0.0;
             if (cbxAT == TipoDeAverage.ATT)
             {
                 desvio_padrao = Math.Sqrt(m_VARate);
             }
             else
             {
                 desvio_padrao = m_VARate;
             }
             valor_t = m_ate / desvio_padrao;
             Normal norm = new Normal(0, 1);
             valor_p = (1.0 - norm.CumulativeDistribution(Math.Abs(valor_t))) * 2;

             #endregion

             #region Upload dos resultados

             matrizDif[categoriaBase, categoriaComp, 0] = m_ate;
             matrizDif[categoriaBase, categoriaComp, 1] = desvio_padrao;
             matrizDif[categoriaBase, categoriaComp, 2] = valor_t;
             matrizDif[categoriaBase, categoriaComp, 3] = valor_p;
             numeroComparacoes = intParticipantes.Length;
             matrizDif[categoriaBase, categoriaComp, 4] = numeroComparacoes;

             #endregion

             #region gerando o output para resultado das estimaes

             #endregion
         }
    }
}

