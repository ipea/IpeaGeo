using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public class BLogicCalculoTaxas : BLogicBaseModelagem
    {
        public BLogicCalculoTaxas()
            : base()
        {
        }

        private ComboBox m_CombOx = new ComboBox();
        public ComboBox ComBOX
        {
            get { return m_CombOx; }
            set { m_CombOx = value; }
        }

        #region variáveis internas

        protected bool m_usa_multiplicador_taxas = true;
        public bool UsaMultiplicadorTaxas
        {
            set
            {
                m_usa_multiplicador_taxas = value;
            }
        }

        protected double m_valor_multiplicador_taxas = 100000.0;
        public double ValorMultiplicadorTaxas
        {
            set
            {
                m_valor_multiplicador_taxas = value;
            }
        }


        protected double tolerancia = 0.0;
        public double ValorTolerancia
        {
            set
            {
                tolerancia = value;
            }
        }

        protected int simulacoes = 1000;
        public int ValorSimulacoes
        {
            set
            {
                simulacoes = value;
            }
        }

        protected string[] m_variaveis_ocorrencias = new string[0];
        public string[] VariaveisOcorrencias
        {
            get { return m_variaveis_ocorrencias; }
            set { this.m_variaveis_ocorrencias = value; }
        }

        protected string m_variavel_populacao = "";
        public string VariavelPopulacao
        {
            get { return m_variavel_populacao; }
            set { m_variavel_populacao = value; }
        }

        string m_medidas_resumo = "";

        public string MedidasResumo 
        {
            get { return this.m_medidas_resumo; }
        }
        
        public bool CalculaTaxaBruta = false;
        public bool CalculaRiscoRelativo = false;
        public bool CalculaEmpiricalBayesRate = false;
        public bool CalculaEmpiricalSpatialBayesRate = false;
        public bool CalculaTaxaEspacial = false;
        public bool CalculaTaxaBayesClaytonGama = false;
        public bool CalculaTaxaBayesClaytonLogN = false;
        public bool CalculaRRelBayesClaytonGama = false;
        public bool CalculaRRelBayesClaytonLogN = false;
        public bool CalculaTaxaBayesClaytonGamaEsp = false;
        public bool CalculaTaxaBayesClaytonLogNEsp = false;
        public bool CalculaRRelBayesClaytonGamaEsp = false;
        public bool CalculaRRelBayesClaytonLogNEsp = false;
        
        #endregion

        #region chamada da estimação das taxas

        private clsUtilTools m_clt = new clsUtilTools();
        private double[] m_vetor_populacao = new double[0];
        private double[] m_vetor_ocorrencias = new double[0];
        protected string[] comblabel = new string [0];
        private object[,] bah = new object[0,0];

        private ProgressBar m_pgbar;
        private Label m_lblpgbar;

        public void EstimarTaxas(ref ProgressBar pgBar, ref Label lblpgBar)
        {
            m_pgbar = pgBar;
            m_lblpgbar = lblpgBar;

            double[,] populacao = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_variavel_populacao);

            bah = m_clt.GetObjMatrizFromDataTable(m_dt_tabela_dados, m_CombOx.SelectedItem.ToString());
            //labels = new string[bah.GetLength(0)];
            //for (int i = 0; i < labels.GetLength(0); i++)
            //{
            //    labels[i] = bah[i, 0].ToString();
            //}

            //comblabel[0] = m_CombOx.ToString;

            //for (int i = 0; i < m_dt_tabela_dados.Rows.Count; i++)
            //{
            //    string label = m_dt_tabela_dados.Rows[i][comblabel[0]];
            //}

            /*double[,] label = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, m_CombOx.Text);*/

            m_vetor_populacao = new double[populacao.GetLength(0)];
            for (int i = 0; i < populacao.GetLength(0); i++)
                m_vetor_populacao[i] = populacao[i, 0];

            DataTable dt_variaveis = new DataTable();

            m_output_variaveis_geradas = "==============================================================================================================================================\n";
            m_output_variaveis_geradas += "\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            if (this.m_usa_multiplicador_taxas)
            {
                m_output_variaveis_geradas += "As taxas calculadas foram multiplicadas por " + this.m_clt.Double2Texto(m_valor_multiplicador_taxas, 4);
                m_output_variaveis_geradas += "\n\n";
            }

            string resultado = EstimarTaxaOcorrenciaEspecifica(this.m_variaveis_ocorrencias, ref dt_variaveis);
            m_output_variaveis_geradas += resultado + "\n";    
        }

        private string EstimarTaxaOcorrenciaEspecifica(string[] svar, ref DataTable dt_variaveis)
        {
            object[,] all_taxas = new object[0, 0];
            ArrayList nomes_novas_variaveis = new ArrayList();
            nomes_novas_variaveis.Add("ID");
            int ntaxas = 0;
            int tamanho = Convert.ToInt16(CalculaEmpiricalBayesRate) + Convert.ToInt16(CalculaEmpiricalSpatialBayesRate) + Convert.ToInt16(CalculaTaxaBruta) + 
                Convert.ToInt16(CalculaTaxaEspacial) + Convert.ToInt16(CalculaRiscoRelativo) + Convert.ToInt16(CalculaTaxaBayesClaytonGama) + 
                Convert.ToInt16(CalculaTaxaBayesClaytonLogN) + Convert.ToInt16(CalculaRRelBayesClaytonGama) + Convert.ToInt16(CalculaRRelBayesClaytonLogN) +
                Convert.ToInt16(CalculaTaxaBayesClaytonGamaEsp) + Convert.ToInt16(CalculaTaxaBayesClaytonLogNEsp) + Convert.ToInt16(CalculaRRelBayesClaytonGamaEsp) + 
                Convert.ToInt16(CalculaRRelBayesClaytonLogNEsp);
            string[] nomes_taxas= new string[tamanho];

            double[,] ocorrenciastotal = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, svar);

            for (int i=0 ; i < (ocorrenciastotal.GetLength(1)); i++)
            {
                for (int j=0 ; j < (ocorrenciastotal.GetLength(0)); j++)
                {
                    if (ocorrenciastotal[j, i] > m_vetor_populacao[i])
                        throw new Exception("Variável de ocorrência maior que população!");
                }
            }

            for (int k = 0; k < svar.Length; k++)
            {
                ntaxas = 0;
                double[,] ocorrencias = m_clt.GetMatrizFromDataTable(m_dt_tabela_dados, svar[k]);
                m_vetor_ocorrencias = new double[ocorrencias.GetLength(0)];
                for (int i = 0; i < ocorrencias.GetLength(0); i++)
                {
                    m_vetor_ocorrencias[i] = ocorrencias[i, 0];
                }

                // corrigindo para valores inválidos (not a number ou valores negativos)
                for (int i = 0; i < m_vetor_populacao.GetLength(0); i++)
                {
                    if (Double.IsNaN(m_vetor_populacao[i]) || Double.IsNegativeInfinity(m_vetor_populacao[i]) || Double.IsPositiveInfinity(m_vetor_populacao[i]) 
                        || Double.IsInfinity(m_vetor_populacao[i]) || m_vetor_populacao[i] < 0.0)
                    {
                        m_vetor_populacao[i] = 0.0;
                        m_vetor_ocorrencias[i] = 0.0;
                    }

                    if (Double.IsNaN(m_vetor_ocorrencias[i]) || Double.IsNegativeInfinity(m_vetor_ocorrencias[i]) || Double.IsPositiveInfinity(m_vetor_ocorrencias[i])
                        || Double.IsInfinity(m_vetor_ocorrencias[i]) || m_vetor_ocorrencias[i] < 0.0)
                    {
                        m_vetor_ocorrencias[i] = 0.0;
                    }
                }

                double[,] taxas = new double[0, 0];
                if (this.CalculaEmpiricalBayesRate)
                {
                    nomes_novas_variaveis.Add(svar[k] + "_EmpBayes");

                    //--- taxas via Empirical Bayes ---//
                    taxas = this.EmpiricalBayes(m_vetor_populacao, m_vetor_ocorrencias);
                    all_taxas = m_clt.Concateh(all_taxas, taxas);
                    ntaxas += 1;
                    nomes_taxas[ntaxas -1] = "Taxa Bayesiana Empírica";
                }

                if (this.CalculaEmpiricalSpatialBayesRate)
                {
                    nomes_novas_variaveis.Add(svar[k] + "_SpatEmpBayes");

                    //--- taxas via spatial empirical Bayes ---//
                    taxas = this.SpatialEmpiricalBayes(m_vetor_populacao, m_vetor_ocorrencias);
                    all_taxas = m_clt.Concateh(all_taxas, taxas);
                    ntaxas += 1;
                    nomes_taxas[ntaxas-1] = "Taxa Bayesiana Empírica Espacial";
                }
                
                if (this.CalculaTaxaBruta)
                {
                    nomes_novas_variaveis.Add(svar[k] + "_TaxBruta");

                    //--- taxas via taxa bruta ---//
                    taxas = this.TaxaBruta(m_vetor_populacao, m_vetor_ocorrencias);
                    all_taxas = m_clt.Concateh(all_taxas, taxas);
                    ntaxas += 1;
                    nomes_taxas[ntaxas-1] = "Taxa Bruta";
                }

                if (this.CalculaTaxaEspacial)
                {
                    nomes_novas_variaveis.Add(svar[k] + "_TaxSpat");

                    //--- taxas via taxa espacial ---//
                    taxas = TaxaEspacial(m_vetor_populacao, m_vetor_ocorrencias);
                    all_taxas = m_clt.Concateh(all_taxas, taxas);
                    ntaxas += 1; 
                    nomes_taxas[ntaxas-1] = "Taxa Espacial";
                }

                if (this.CalculaRiscoRelativo)
                {
                    nomes_novas_variaveis.Add(svar[k] + "_RiscoRel");

                    //--- taxas via risco absoluto ---//
                    taxas = this.ExcessRisk(m_vetor_populacao, m_vetor_ocorrencias);
                    all_taxas = m_clt.Concateh(all_taxas, taxas);
                    ntaxas += 1;
                    nomes_taxas[ntaxas-1] = "Risco Relativo";
                }
                
                if (this.CalculaTaxaBayesClaytonGama)
                {
                    nomes_novas_variaveis.Add(svar[k] + "_TaxBayesGamma");

                    //--- taxas via taxa espacial ---//
                    taxas = TaxaBayesClaytonGama(m_vetor_populacao, m_vetor_ocorrencias);
                    all_taxas = m_clt.Concateh(all_taxas, taxas);
                    ntaxas += 1;
                    nomes_taxas[ntaxas-1] = "Taxa de Clayton e Kaldor - Priori Gamma";
                }

                //"_RRelBayesLogN"

                if (this.CalculaTaxaBayesClaytonLogN)
                {
                    nomes_novas_variaveis.Add(svar[k] + "_TaxBayesLogN");

                    //--- taxas via taxa espacial ---//
                    taxas = TaxaBayesClaytonLogN(m_vetor_populacao, m_vetor_ocorrencias);
                    all_taxas = m_clt.Concateh(all_taxas, taxas);
                    ntaxas += 1;
                    nomes_taxas[ntaxas-1] = "Taxa de Clayton e Kaldor - Priori LogNormal";
                }
                
                if (this.CalculaRRelBayesClaytonGama)
                {
                    nomes_novas_variaveis.Add(svar[k] + "_RRelBayesGamma");

                    //--- taxas via taxa espacial ---//
                    taxas = RRelBayesClaytonGama(m_vetor_populacao, m_vetor_ocorrencias);
                    all_taxas = m_clt.Concateh(all_taxas, taxas);
                    ntaxas += 1;
                    nomes_taxas[ntaxas-1] = "Risco Relativo de Clayton e Kaldor - Priori Gamma";
                }

                //"_RRelBayesLogN"

                if (this.CalculaRRelBayesClaytonLogN)
                {
                    nomes_novas_variaveis.Add(svar[k] + "_RRelBayesLogN");

                    //--- taxas via taxa espacial ---//
                    taxas = RRelBayesClaytonLogN(m_vetor_populacao, m_vetor_ocorrencias);
                    all_taxas = m_clt.Concateh(all_taxas, taxas);
                    ntaxas += 1;
                    nomes_taxas[ntaxas-1] = "Risco Relativo de Clayton e Kaldor - Priori LogNormal";
                }

                if (this.CalculaTaxaBayesClaytonGamaEsp)
                {
                    nomes_novas_variaveis.Add(svar[k] + "_TaxBayesGammaEsp");

                    //--- taxas via taxa espacial ---//
                    taxas = TaxaBayesClaytonGamaEsp(m_vetor_populacao, m_vetor_ocorrencias);
                    all_taxas = m_clt.Concateh(all_taxas, taxas);
                    ntaxas += 1;
                    nomes_taxas[ntaxas-1] = "Taxa de Clayton e Kaldor Espacial - Priori Gamma";
                }

                //"_RRelBayesLogN"

                if (this.CalculaTaxaBayesClaytonLogNEsp)
                {
                    nomes_novas_variaveis.Add(svar[k] + "_TaxBayesLogNEsp");

                    //--- taxas via taxa espacial ---//
                    taxas = TaxaBayesClaytonLogNEsp(m_vetor_populacao, m_vetor_ocorrencias);
                    all_taxas = m_clt.Concateh(all_taxas, taxas);
                    ntaxas += 1;
                    nomes_taxas[ntaxas-1] = "Taxa de Clayton e Kaldor Espacial - Priori LogNormal";
                }
                
                if (this.CalculaRRelBayesClaytonGamaEsp)
                {
                    nomes_novas_variaveis.Add(svar[k] + "_RRelBayesGammaEsp");

                    //--- taxas via taxa espacial ---//
                    taxas = RRelBayesClaytonGamaEsp(m_vetor_populacao, m_vetor_ocorrencias);
                    all_taxas = m_clt.Concateh(all_taxas, taxas);
                    ntaxas += 1;
                    nomes_taxas[ntaxas-1] = "Risco Relativo de Clayton e Kaldor Espacial - Priori Gamma";
                }

                //"_RRelBayesLogN"

                if (this.CalculaRRelBayesClaytonLogNEsp)
                {
                    nomes_novas_variaveis.Add(svar[k] + "_RRelBayesLogNEsp");

                    //--- taxas via taxa espacial ---//
                    taxas = RRelBayesClaytonLogNEsp(m_vetor_populacao, m_vetor_ocorrencias);
                    all_taxas = m_clt.Concateh(all_taxas, taxas);
                    ntaxas += 1;
                    nomes_taxas[ntaxas-1] = "Risco Relativo de Clayton e Kaldor Espacial - Priori LogNormal";
                }                            
            }           
            
            #region Medidas resumo das taxas
                        
            m_medidas_resumo = "============================================================================================================================\n";
            m_medidas_resumo += "\n";
            m_medidas_resumo += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_medidas_resumo += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            if (this.m_usa_multiplicador_taxas)
            {
                m_medidas_resumo += "As taxas calculadas foram multiplicadas por " + this.m_clt.Double2Texto(m_valor_multiplicador_taxas, 4);
                m_medidas_resumo += "\n\n";
            }

            double[,] alltaxas_mr = m_clt.ConvertMatrixObjToDouble(all_taxas);

            double[,] resumos = new double[0,0];
            double[,] media;
            double[,] mediana;
            double[,] desvio;
            double[,] minimo;
            double[,] maximo;
            media = m_clt.Meanc(alltaxas_mr);
            mediana = m_clt.Medianc(alltaxas_mr);
            desvio= m_clt.Despadc(alltaxas_mr);
            minimo = m_clt.Minc(alltaxas_mr);
            maximo = m_clt.Maxc(alltaxas_mr);
            resumos = m_clt.Concatev(media, mediana);
            resumos = m_clt.Concatev(resumos, desvio);
            resumos = m_clt.Concatev(resumos, minimo);
            resumos = m_clt.Concatev(resumos, maximo);

            string[] med_resumos = new string[6];
            object[,] aux = new object[ntaxas ,6];
            med_resumos[0] = "Taxas";
            med_resumos[1] = "Média";
            med_resumos[2] = "Mediana";
            med_resumos[3] = "Desvio";
            med_resumos[4] = "Mínimo";
            med_resumos[5] = "Máximo";

            

            for (int j = 0; j < svar.Length; j++)
            {
                int contador1 = 0;
                for (int i = j; i < (j + ntaxas); i++)
                {
                    aux[contador1, 0] = nomes_taxas[contador1];
                    aux[contador1, 1] = resumos[0, i];
                    aux[contador1, 2] = resumos[1, i];
                    aux[contador1, 3] = resumos[2, i];
                    aux[contador1, 4] = resumos[3, i];
                    aux[contador1, 5] = resumos[4, i];
                    

                    contador1 += 1;
                }
                m_medidas_resumo += "============================================================================================================================ \n\n";
                m_medidas_resumo += svar[j].ToString();
                m_medidas_resumo += "\n";

                string resultado2 = GeraTabelaNovasVariaveis(aux, med_resumos);

                m_medidas_resumo += "\n";

                m_medidas_resumo += resultado2;
                m_medidas_resumo += "\n";

                

            }



            #endregion

            /*
            double[,] observacoes = new double[all_taxas.GetLength(0), 1];
            for (int i = 0; i < observacoes.GetLength(0); i++) observacoes[i, 0] = (double)(i + 1);
            all_taxas = m_clt.Concateh(observacoes, all_taxas);
            */
            
            object[,] observacoes = new object[all_taxas.GetLength(0), 1];
            for (int i = 0; i < observacoes.GetLength(0); i++) observacoes[i, 0] = bah[i,0];
            all_taxas = m_clt.Concateh(bah, all_taxas);
                       
            string[] nomes_variaveis = new string[nomes_novas_variaveis.Count];
            for (int i = 0; i < nomes_novas_variaveis.Count; i++)
            {
                nomes_variaveis[i] = nomes_novas_variaveis[i].ToString();
            }

            Type[] tipos_vars = new Type[nomes_variaveis.GetLength(0)-1];
            string[] nomes_vars = new string[nomes_variaveis.GetLength(0) - 1];
            for (int i = 0; i < tipos_vars.GetLength(0); i++)
            {
                nomes_vars[i] = nomes_variaveis[i + 1];
                tipos_vars[i] = typeof(double);
            }

            m_clt.AdicionaColunasToDataTable(ref m_dt_tabela_dados, m_clt.DeleteCol(all_taxas, 0), nomes_vars, tipos_vars);

            return GeraTabelaNovasVariaveis(all_taxas, nomes_variaveis);
        }

        #endregion

        #region funções para cálculo das taxas 

        /// <summary>
        /// Taxa Empirical Bayes, usando a taxa de todo o mapa para "m"
        /// </summary>
        /// <param name="populacao"></param>
        /// <param name="casos"></param>
        /// <param name="shape"></param>
        /// <returns></returns>
        private double[,] EmpiricalBayes(double[] populacao, double[] casos)
        {
            IpeaGeo.RegressoesEspaciais.clsIpeaShape shape = m_shape;
            //clsIpeaShape shape = (IpeaGEO.clsIpeaShape)m_shape;
            double[,] taxa = new double[populacao.GetLength(0),1];
            if (taxa.GetLength(0) < populacao.GetLength(0)) taxa = new double[populacao.GetLength(0),1];

            double soma_pop = 0;
            double soma_casos = 0;
            double m = 0;
            for (int i = 0; i < taxa.GetLength(0); i++)
            {
                soma_casos += casos[i];
                soma_pop += populacao[i];
            }
            m = soma_casos / soma_pop;

            double v = 0;
            double numerador = 0;
            for (int i = 0; i < taxa.GetLength(0); i++)
            {
                numerador += populacao[i] * Math.Pow(((casos[i] / (populacao[i]+0.000001)) - m), 2);
            }
            double popmedia = soma_pop / (double)taxa.GetLength(0);
            v = (numerador / soma_pop) - (m / popmedia);

            double[] wi = new double[taxa.GetLength(0)];
            for (int i = 0; i < taxa.GetLength(0); i++)
            {
                wi[i] = v / (v + (m / populacao[i]));
                taxa[i,0] = (wi[i] * (casos[i] / (populacao[i]+0.00001))) + (1 - wi[i]) * m;
            }

            if (m_usa_multiplicador_taxas)
            {
                for (int i = 0; i < taxa.GetLength(0); i++)
                {
                    taxa[i, 0] = taxa[i, 0] * m_valor_multiplicador_taxas;
                }
            }

            return (taxa);
        }

        /// <summary>
        /// Taxa Empirical Bayes, usando a taxa dos vizinhos para "m"
        /// </summary>
        /// <param name="populacao"></param>
        /// <param name="casos"></param>
        /// <param name="shape"></param>
        /// <returns></returns>
        private double[,] SpatialEmpiricalBayes(double[] populacao, double[] casos)
        {
            IpeaGeo.RegressoesEspaciais.clsIpeaShape shape = m_shape;
            //clsIpeaShape shape = (IpeaGEO.clsIpeaShape)m_shape;            

            double[,] taxa = new double[populacao.GetLength(0),1];
            double[] m = new double[populacao.GetLength(0)];
            double[] popmedia = new double[populacao.GetLength(0)];
            double[] v = new double[populacao.GetLength(0)];

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                double numerador = 0;
                double popvizinhos = 0; 
                double casosvizinhos = 0;
                int[] indices = new int[shape[i].NumeroVizinhos + 1];
                
                for (int j = 0; j < shape[i].NumeroVizinhos ; j++) 
                {
                    indices[j] = shape[i].ListaIndicesVizinhos[j];

                }
                indices[shape[i].NumeroVizinhos] = i;
                
                for (int j = 0; j < indices.GetLength(0); j++)
                {
                    popvizinhos += populacao[indices[j]];
                    casosvizinhos += casos[indices[j]];
                }

                m[i] = casosvizinhos / (popvizinhos+0.000001);
                popmedia[i] = popvizinhos / indices.GetLength(0); 

                for (int j = 0; j < indices.GetLength(0); j++)
                {
                    numerador += populacao[indices[j]]* Math.Pow(((casos[indices[j]]/ (populacao[indices[j]]+0.00001)) - m[i]), 2); 

                }

                v[i] = (numerador / (popvizinhos+0.000001)) - (m[i] / popmedia[i]);
                if (v[i] < 0.0)
                {
                    v[i] = 0.0;
                }
            }

            double[] wi = new double[populacao.GetLength(0)];
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                wi[i] = v[i] / (v[i] + (m[i] / (populacao[i] + 0.000001)));
                taxa[i, 0] = (wi[i] * (casos[i] / (populacao[i] + 0.000001))) + ((1 - wi[i]) * m[i]);
                taxa[i, 0] = taxa[i, 0];
                if (v[i] == 0)
                    taxa[i, 0] = casos[i] / (populacao[i] + 0.000001);
            }

            if (m_usa_multiplicador_taxas)
            {
                for (int i = 0; i < taxa.GetLength(0); i++)
                {
                    taxa[i, 0] = taxa[i, 0] * m_valor_multiplicador_taxas;
                }
            }

            return (taxa);
        }

        /// <summary>
        /// Risco relativo
        /// </summary>
        /// <param name="populacao"></param>
        /// <param name="casos"></param>
        /// <param name="shape"></param>
        /// <returns></returns>
        private double[,] ExcessRisk(double[] populacao, double[] casos)
        {
            IpeaGeo.RegressoesEspaciais.clsIpeaShape shape = m_shape;
            //clsIpeaShape shape = (IpeaGEO.clsIpeaShape)m_shape;
            double[,] taxa = new double[populacao.GetLength(0), 1];
            double[] valores_esperados = new double[populacao.GetLength(0)];
            double[] valores_observados = new double[populacao.GetLength(0)];
            double somapop = 0;
            double somacasos = 0;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                somapop += populacao[i];
                somacasos += casos[i];
            }
            double taxaglobal = somacasos / somapop;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                valores_esperados[i] = taxaglobal * populacao[i];
                valores_observados[i] = casos[i] ;
                taxa[i, 0] = valores_observados[i] / (valores_esperados[i]+0.000001);
            }

            for (int i = 0; i < taxa.GetLength(0); i++)
            {
                 taxa[i, 0] = taxa[i, 0];
            } 

            return (taxa);
        }

        private double[,] TaxaBruta(double[] populacao, double[] casos)
        {
            IpeaGeo.RegressoesEspaciais.clsIpeaShape shape = m_shape;
            //clsIpeaShape shape = (IpeaGEO.clsIpeaShape)m_shape;

            double[,] taxa = new double[populacao.GetLength(0), 1];
            if (populacao.GetLength(0) < populacao.GetLength(0)) taxa = new double[populacao.GetLength(0), 1];

            for (int i = 0; i < taxa.GetLength(0); i++)
            {
                //taxa[i, 0] = casos[i] / (populacao[i]+0.0000001);

                if (populacao[i] > 0.0) taxa[i, 0] = casos[i] / (populacao[i] + 0.0000001);
                else taxa[i, 0] = 0.0;
            }

            if (m_usa_multiplicador_taxas)
            {
                for (int i = 0; i < taxa.GetLength(0); i++)
                {
                    taxa[i, 0] = taxa[i, 0] * m_valor_multiplicador_taxas;
                }
            }
            
            return (taxa);
        }

        private double[,] TaxaEspacial(double[] populacao, double[] casos)
        {
            IpeaGeo.RegressoesEspaciais.clsIpeaShape shape = m_shape;
            //clsIpeaShape shape = (IpeaGEO.clsIpeaShape)m_shape;
            double[,] taxa = new double[populacao.GetLength(0), 1];

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                double numerador = casos[i];
                double denominador = populacao[i];
                for (int j = 0; j < shape[i].NumeroVizinhos; j++)
                {
                    numerador += casos[shape[i].ListaIndicesVizinhos[j]];
                    denominador += populacao[shape[i].ListaIndicesVizinhos[j]];
                }

                taxa[i, 0] = numerador / denominador;   
            }

            if (m_usa_multiplicador_taxas)
            {
                for (int i = 0; i < taxa.GetLength(0); i++)
                {
                    taxa[i, 0] = taxa[i, 0] * m_valor_multiplicador_taxas;
                }
            }
            
            return (taxa);
        }

        private double[,] TaxaBayesClaytonGama(double[] populacao, double[] casos)
        {
            IpeaGeo.RegressoesEspaciais.clsIpeaShape shape = m_shape;
            //clsIpeaShape shape = (IpeaGEO.clsIpeaShape)m_shape;

            double[,] taxa = new double[populacao.GetLength(0), 1];
            double[] risco = new double[populacao.GetLength(0)];
            double[] valores_esperados = new double[populacao.GetLength(0)];
            double[] valores_observados = new double[populacao.GetLength(0)];
            double somapop = 0;
            double somacasos = 0;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                somapop += populacao[i];
                somacasos += casos[i];
            }

            double taxaglobal = somacasos / somapop;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                valores_esperados[i] = taxaglobal * populacao[i];
                valores_observados[i] = casos[i];
                risco[i] = valores_observados[i] / (valores_esperados[i]+0.00001);
            }

            double mean0 = 0;
            double var0 = 0;
            double alpha_new = 1;
            double nu_new = 1;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                mean0 += risco[i];
            }
            mean0 /= (double)populacao.GetLength(0);

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                var0 += Math.Pow((risco[i] - mean0), 2);
            }
            var0 /= (double)(populacao.GetLength(0) - 1);

            alpha_new = mean0 / var0;
            nu_new = Math.Pow(mean0, 2) / var0;

            double[] theta_new = new double[populacao.GetLength(0)];
            int num_iterations = 1;
            //double[] alpha_path = new double[num_iterations];
            //double[] nu_path = new double[num_iterations];
            //double[] min_gamma_path = new double[num_iterations];
            //double[] max_gamma_path = new double[num_iterations];
            double aux1old = mean0;
            double aux2old = var0;
            double aux2a = new double();

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                theta_new[i] = ((double)valores_observados[i] + nu_new) / ((double)valores_esperados[i] + alpha_new+ 0.000001);
            }

            double aux1 = m_clt.Mean(theta_new);
            double aux2 = 0;
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux2a += (1 + (alpha_new / (double)(valores_esperados[i]+0.000001))) * Math.Pow((theta_new[i] - aux1), 2);
            }

            aux2 = (1 / (double)(populacao.GetLength(0) - 1.0)) * aux2a;

            m_lblpgbar.Text = "Executando as simulações de Monte Carlo";
            m_pgbar.Minimum = 0;
            m_pgbar.Maximum = simulacoes;
            m_pgbar.Value = 0;

            while ((Math.Abs(aux1old - aux1) > (Math.Abs(aux1old - aux1) * tolerancia) || Math.Abs(aux2old - aux2) > (Math.Abs(aux2old - aux2) * tolerancia)) &&
                   (num_iterations <= simulacoes))
            {
                alpha_new = aux1 / aux2;
                nu_new = aux1 * alpha_new;

                theta_new[0] = ((double)valores_observados[0] + nu_new) / ((double)valores_esperados[0] + alpha_new + 0.000001);
                //min_gamma_path[num_iterations] = theta_new[0];
                //max_gamma_path[num_iterations] = theta_new[0];

                for (int i = 1; i < populacao.GetLength(0); i++)
                {
                    theta_new[i] = ((double)valores_observados[i] + nu_new) / ((double)valores_esperados[i] + alpha_new+0.000001);

                    //if (theta_new[i] < theta_new[i - 1])
                    //{
                    //    min_gamma_path[num_iterations] = theta_new[i];
                    //}
                    //else
                    //{
                    //    max_gamma_path[num_iterations] = theta_new[i];
                    //}
                }

                aux1old = aux1;
                aux2old = aux2;
                aux1 = aux2a = aux2 = 0;

                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux1 += theta_new[i];
                }
                aux1 /= (double)populacao.GetLength(0);

                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux2a += (1 + (alpha_new / (double)(valores_esperados[i]+0.000001))) * Math.Pow((theta_new[i] - aux1), 2);
                }

                aux2 = (1 / (double)(populacao.GetLength(0) - 1.0)) * aux2a;
                
                this.m_pgbar.Value = num_iterations;
                Application.DoEvents();

                //alpha_path[num_iterations] = alpha_new;
                //nu_path[num_iterations] = nu_new; 
                num_iterations = num_iterations +1;
            }

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                taxa[i, 0] = theta_new[i] * taxaglobal;
            }

            if (m_usa_multiplicador_taxas)
            {
                for (int i = 0; i < taxa.GetLength(0); i++)
                {
                    taxa[i, 0] = taxa[i, 0] * m_valor_multiplicador_taxas;
                }
            }

            this.m_lblpgbar.Text = "Executando o cálculo de taxas";
            this.m_pgbar.Value = 0;
            this.m_pgbar.Refresh();
            Application.DoEvents();

            return (taxa);
        }

        private double[,] TaxaBayesClaytonLogN(double[] populacao, double[] casos)
        {
            IpeaGeo.RegressoesEspaciais.clsIpeaShape shape = m_shape;

            double[,] taxa = new double[populacao.GetLength(0), 1];
            double[] risco = new double[populacao.GetLength(0)];
            double[] valores_esperados = new double[populacao.GetLength(0)];
            double[] valores_observados = new double[populacao.GetLength(0)];
            double somapop = 0;
            double somacasos = 0;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                somapop += populacao[i];
                somacasos += casos[i];
            }
            double taxaglobal = somacasos / somapop;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                valores_esperados[i] = taxaglobal * populacao[i];
                valores_observados[i] = casos[i];
                risco[i] = (valores_observados[i]+ 0.5) / (valores_esperados[i]+0.00001);
            }

            double[] b_new = new double[populacao.GetLength(0)];
            double sigma2_old = 0.0;
            double phi_old = 0.0;
            //double[] sigma2_path = new double[num_iterations];
            //double[] phi_path = new double[num_iterations];
            //double[] min_lnormal_path = new double[num_iterations];
            //double[] max_lnormal_path = new double[num_iterations];

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                b_new[i] = Math.Log(risco[i]);
            }

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                phi_old += b_new[i];
            }

            phi_old /= (double)populacao.GetLength(0);
            double phi_new = phi_old;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                sigma2_old += Math.Pow((b_new[i] - phi_old), 2);
            }
            sigma2_old /= (double)(populacao.GetLength(0) - 1);
            
            double aux3 = 0.0;
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux3 += 1 / (1 + (sigma2_old * (valores_observados[i] + 0.5)));
            }
            aux3 = sigma2_old * aux3;

            double aux4 = 0;
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux4 += Math.Pow((b_new[i] - phi_new), 2);
            }
            double sigma2_new = (aux3 + aux4) / (double)populacao.GetLength(0);

            double[] aux5 = new double[populacao.GetLength(0)];
            double[] aux6 = new double[populacao.GetLength(0)];
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux5[i] = sigma2_new * (valores_observados[i] + 0.5) * Math.Log((valores_observados[i] + 0.5) / (valores_esperados[i]+0.000001));
                aux6[i] = 1 + (valores_observados[i] + 0.5) * sigma2_new;
                b_new[i] = (phi_new + aux5[i] - (sigma2_new / 2)) / aux6[i];
            }

            m_lblpgbar.Text = "Executando as simulações de Monte Carlo";
            m_pgbar.Minimum = 0;
            m_pgbar.Maximum = simulacoes;
            m_pgbar.Value = 0;

            int num_iterations = 1;

            while ((Math.Abs(sigma2_old - sigma2_new) > (Math.Abs(sigma2_old - sigma2_new) * tolerancia) || Math.Abs(phi_old - phi_new) > (Math.Abs(phi_old - phi_new) * tolerancia)) &&
                   (num_iterations <= simulacoes))
            {
                sigma2_old = sigma2_new;
                phi_old = phi_new;
                phi_new = m_clt.Mean(b_new);
                aux3 = 0.0;
                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux3 += 1 / (1 + (sigma2_old * (valores_observados[i] + 0.5)));
                }

                aux3 = sigma2_old * aux3;

                aux4 = 0;
                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux4 += Math.Pow((b_new[i] - phi_new), 2);
                }
                sigma2_new = (aux3 + aux4) / (double)populacao.GetLength(0);
                
                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux5[i] = sigma2_new * (valores_observados[i] + 0.5) * Math.Log((valores_observados[i] + 0.5) / (valores_esperados[i]+0.000001));
                    aux6[i] = 1 + (valores_observados[i] + 0.5) * sigma2_new;
                    b_new[i] = (phi_new + aux5[i] - (sigma2_new / 2)) / aux6[i];
                }

                this.m_pgbar.Value = num_iterations;
                Application.DoEvents();

                num_iterations = num_iterations +1;
            }
   
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                taxa[i, 0] = Math.Exp(b_new[i]) * taxaglobal;
            }

            if (m_usa_multiplicador_taxas)
            {
                for (int i = 0; i < taxa.GetLength(0); i++)
                {
                    taxa[i, 0] = taxa[i, 0] * m_valor_multiplicador_taxas;
                }
            }

            this.m_lblpgbar.Text = "Executando o cálculo de taxas";
            this.m_pgbar.Value = 0;
            this.m_pgbar.Refresh();
            Application.DoEvents();

            return (taxa);
        }

        private double[,] RRelBayesClaytonGama(double[] populacao, double[] casos)
        {
            IpeaGeo.RegressoesEspaciais.clsIpeaShape shape = m_shape;
            //clsIpeaShape shape = (IpeaGEO.clsIpeaShape)m_shape;

            double[,] taxa = new double[populacao.GetLength(0), 1];
            double[] risco = new double[populacao.GetLength(0)];
            double[] valores_esperados = new double[populacao.GetLength(0)];
            double[] valores_observados = new double[populacao.GetLength(0)];
            double somapop = 0;
            double somacasos = 0;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                somapop += populacao[i];
                somacasos += casos[i];
            }
            double taxaglobal = somacasos / somapop;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                valores_esperados[i] = taxaglobal * populacao[i];
                valores_observados[i] = casos[i];
                risco[i] = valores_observados[i] / (valores_esperados[i] + 0.000001);
            }

            double mean0 = 0;
            double var0 = 0;
            double alpha_new = 1;
            double nu_new = 1;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                mean0 += risco[i];
            }
            mean0 /= (double)populacao.GetLength(0);

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                var0 += Math.Pow((risco[i] - mean0), 2);
            }
            var0 /= (populacao.GetLength(0) - 1);

            alpha_new = mean0 / var0;
            nu_new = Math.Pow(mean0, 2) / var0;

            double[] theta_new = new double[populacao.GetLength(0)];
            int num_iterations = 1;
            //double[] alpha_path = new double[num_iterations];
            //double[] nu_path = new double[num_iterations];
            //double[] min_gamma_path = new double[num_iterations];
            //double[] max_gamma_path = new double[num_iterations];
            double aux1old = mean0;
            double aux2old = var0;
            double aux2a = new double();

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                theta_new[i] = ((double)valores_observados[i] + nu_new) / ((double)valores_esperados[i] + alpha_new+0.000001);
            }

            double aux1 = m_clt.Mean(theta_new);
            double aux2 = 0;
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux2a += (1 + (alpha_new / (double)(valores_esperados[i] + 0.000001))) * Math.Pow((theta_new[i] - aux1), 2);
            }

            aux2 = (1 / (double)(populacao.GetLength(0) - 1.0)) * aux2a;

            m_lblpgbar.Text = "Executando as simulações de Monte Carlo";
            m_pgbar.Minimum = 0;
            m_pgbar.Maximum = simulacoes;
            m_pgbar.Value = 0;

            while ((Math.Abs(aux1old - aux1) > (Math.Abs(aux1old - aux1) * tolerancia) || Math.Abs(aux2old - aux2) > (Math.Abs(aux2old - aux2) * tolerancia)) &&
                   (num_iterations <= simulacoes))
            {
                alpha_new = aux1 / aux2;
                nu_new = aux1 * alpha_new;

                theta_new[0] = ((double)valores_observados[0] + nu_new) / ((double)valores_esperados[0] + alpha_new + 0.000001);
                //min_gamma_path[num_iterations] = theta_new[0];
                //max_gamma_path[num_iterations] = theta_new[0];

                for (int i = 1; i < populacao.GetLength(0); i++)
                {
                    theta_new[i] = ((double)valores_observados[i] + nu_new) / ((double)valores_esperados[i] + alpha_new+0.000001);

                    //if (theta_new[i] < theta_new[i - 1])
                    //{
                    //    min_gamma_path[num_iterations] = theta_new[i];
                    //}
                    //else
                    //{
                    //    max_gamma_path[num_iterations] = theta_new[i];
                    //}
                }

                aux1old = aux1;
                aux2old = aux2;
                aux1 = aux2a = aux2 = 0;

                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux1 += theta_new[i];
                }
                aux1 /= (double)populacao.GetLength(0);

                for (int i = 0; i < populacao.GetLength(0); i++)
                {

                    aux2a += (1 + (alpha_new / (double)(valores_esperados[i] + 0.000001))) * Math.Pow((theta_new[i] - aux1), 2);
                }

                aux2 = (1 / (double)(populacao.GetLength(0) - 1.0)) * aux2a;

                this.m_pgbar.Value = num_iterations;
                Application.DoEvents();

                //alpha_path[num_iterations] = alpha_new;
                //nu_path[num_iterations] = nu_new; 
                num_iterations = num_iterations +1;
            }

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                taxa[i, 0] = theta_new[i];
            }

            this.m_lblpgbar.Text = "Executando o cálculo de taxas";
            this.m_pgbar.Value = 0;
            this.m_pgbar.Refresh();
            Application.DoEvents();

            return (taxa);
        }

        private double[,] RRelBayesClaytonLogN(double[] populacao, double[] casos)
        {
            IpeaGeo.RegressoesEspaciais.clsIpeaShape shape = m_shape;

            double[,] taxa = new double[populacao.GetLength(0), 1];
            double[] risco = new double[populacao.GetLength(0)];
            double[] valores_esperados = new double[populacao.GetLength(0)];
            double[] valores_observados = new double[populacao.GetLength(0)];
            double somapop = 0.0;
            double somacasos = 0.0;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                somapop += populacao[i];
                somacasos += casos[i];
            }
            double taxaglobal = somacasos / somapop;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                valores_esperados[i] = taxaglobal * populacao[i];
                valores_observados[i] = casos[i];
                risco[i] = (valores_observados[i] + 0.5) / (valores_esperados[i] + 0.000001);
            }

            double[] b_new = new double[populacao.GetLength(0)];
            double sigma2_old = 0.0;
            double phi_old = 0.0;
            //double[] sigma2_path = new double[num_iterations];
            //double[] phi_path = new double[num_iterations];
            //double[] min_lnormal_path = new double[num_iterations];
            //double[] max_lnormal_path = new double[num_iterations];

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                b_new[i] = Math.Log(risco[i]);
            }

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                phi_old  += b_new[i];
            }

            phi_old /= (double)populacao.GetLength(0);
            double phi_new=phi_old;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                sigma2_old += Math.Pow((b_new[i] - phi_old), 2);
            }
            sigma2_old /= (double)(populacao.GetLength(0)-1);


            double aux3 = 0.0;
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux3 += 1 / (1 + (sigma2_old * (valores_observados[i] + 0.5)));
            }
            aux3 = sigma2_old * aux3;

            double aux4 =0;
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux4 += Math.Pow((b_new[i] - phi_new), 2);
            }
            double sigma2_new = (aux3 + aux4) / (double)populacao.GetLength(0);

            double[] aux5 = new double[populacao.GetLength(0)];
            double[] aux6 = new double[populacao.GetLength(0)];
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux5[i] = sigma2_new * (valores_observados[i] + 0.5) * Math.Log((valores_observados[i] + 0.5) / (valores_esperados[i] + 0.000001));
                aux6[i] = 1 + (valores_observados[i] + 0.5) * sigma2_new;
                b_new[i] = (phi_new + aux5[i] - (sigma2_new / 2)) / aux6[i];
            }

            m_lblpgbar.Text = "Executando as simulações de Monte Carlo";
            m_pgbar.Minimum = 0;
            m_pgbar.Maximum = simulacoes;
            m_pgbar.Value = 0;

            int num_iterations = 1;

            while ((Math.Abs(sigma2_old - sigma2_new) > (Math.Abs(sigma2_old - sigma2_new) * tolerancia) || Math.Abs(phi_old - phi_new) > (Math.Abs(phi_old - phi_new) * tolerancia)) &&
                   (num_iterations <= simulacoes))
            {
                sigma2_old=sigma2_new;
                phi_old=phi_new;
                phi_new= m_clt.Mean(b_new);
                aux3 = 0.0;
                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux3 += 1 / (1 + (sigma2_old * (valores_observados[i] + 0.5)));
                }

                aux3 = sigma2_old * aux3;

                aux4 =0;
                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux4 += Math.Pow((b_new[i] - phi_new), 2);
                }
                sigma2_new = (aux3 + aux4) / (double)populacao.GetLength(0);
                
                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux5[i] = sigma2_new * (valores_observados[i] + 0.5) * Math.Log((valores_observados[i] + 0.5) / (valores_esperados[i] + 0.000001));
                    aux6[i] = 1 + (valores_observados[i] + 0.5) * sigma2_new;
                    b_new[i] = (phi_new + aux5[i] - (sigma2_new / 2)) / aux6[i];
                }

                this.m_pgbar.Value = num_iterations;
                Application.DoEvents();

                num_iterations = num_iterations +1;
            }

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                taxa[i, 0] = Math.Exp(b_new[i]);
            }

            this.m_lblpgbar.Text = "Executando o cálculo de taxas";
            this.m_pgbar.Value = 0;
            this.m_pgbar.Refresh();
            Application.DoEvents();

            return (taxa);
        }
                
        private double[,] TaxaBayesClaytonGamaEsp(double[] populacao, double[] casos)
        {
            IpeaGeo.RegressoesEspaciais.clsIpeaShape shape = m_shape;
            //clsIpeaShape shape = (IpeaGEO.clsIpeaShape)m_shape;

            double[,] taxa = new double[populacao.GetLength(0), 1];
            double[] risco = new double[populacao.GetLength(0)];
            double[] valores_esperados = new double[populacao.GetLength(0)];
            double[] valores_observados = new double[populacao.GetLength(0)];
            double somapop = 0;
            double somacasos = 0;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                somapop += populacao[i];
                somacasos += casos[i];
            }
            double taxaglobal = somacasos / somapop;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                valores_esperados[i] = taxaglobal * populacao[i];
                valores_observados[i] = casos[i];
                risco[i] = valores_observados[i] / (valores_esperados[i] + 0.000001);
            }

            double mean0 = 0;
            double var0 = 0;
            double alpha_new = 1;
            double nu_new = 1;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                mean0 += risco[i];
            }
            mean0 /= (double)populacao.GetLength(0);

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                var0 += Math.Pow((risco[i] - mean0), 2);
            }
            var0 /= (double)(populacao.GetLength(0) - 1);

            alpha_new = mean0 / var0;
            nu_new = Math.Pow(mean0, 2) / var0;
            
            double[] theta_new = new double[populacao.GetLength(0)];
            int num_iterations = 1;
            //double[] alpha_path = new double[num_iterations];
            //double[] nu_path = new double[num_iterations];
            //double[] min_gamma_path = new double[num_iterations];
            //double[] max_gamma_path = new double[num_iterations];
            double aux1old = mean0;
            double aux2old = var0;
            double aux2a = new double();

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                theta_new[i] = ((double)valores_observados[i] + nu_new) / ((double)valores_esperados[i] + alpha_new+0.000001);
            }

            double aux1 = m_clt.Mean(theta_new);
            double aux2 = 0;
            for (int i = 0; i < populacao.GetLength(0); i++)
            {

                aux2a += (1 + (alpha_new / (double)(valores_esperados[i] + 0.000001))) * Math.Pow((theta_new[i] - aux1), 2);
            }

            aux2 = (1 / (double)(populacao.GetLength(0) - 1.0)) * aux2a;

            while ((Math.Abs(aux1old - aux1) > (Math.Abs(aux1old - aux1) * tolerancia) || Math.Abs(aux2old - aux2) > (Math.Abs(aux2old - aux2) * tolerancia)) &&
                   (num_iterations <= simulacoes))
            {
                alpha_new = aux1 / aux2;
                nu_new = aux1 * alpha_new;

                theta_new[0] = ((double)valores_observados[0] + nu_new) / ((double)valores_esperados[0] + alpha_new + 0.000001);
                //min_gamma_path[num_iterations] = theta_new[0];
                //max_gamma_path[num_iterations] = theta_new[0];

                for (int i = 1; i < populacao.GetLength(0); i++)
                {
                    theta_new[i] = ((double)valores_observados[i] + nu_new) / ((double)valores_esperados[i] + alpha_new+0.000001);

                    //if (theta_new[i] < theta_new[i - 1])
                    //{
                    //    min_gamma_path[num_iterations] = theta_new[i];
                    //}
                    //else
                    //{
                    //    max_gamma_path[num_iterations] = theta_new[i];
                    //}
                }

                aux1old = aux1;
                aux2old = aux2;
                aux1 = aux2a = aux2 = 0;

                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux1 += theta_new[i];
                }
                aux1 /= (double)populacao.GetLength(0);

                for (int i = 0; i < populacao.GetLength(0); i++)
                {

                    aux2a += (1 + (alpha_new / (double)(valores_esperados[i] + 0.000001))) * Math.Pow((theta_new[i] - aux1), 2);
                }

                aux2 = (1 / (double)(populacao.GetLength(0) - 1.0)) * aux2a;

                //alpha_path[num_iterations] = alpha_new;
                //nu_path[num_iterations] = nu_new; 
                num_iterations = num_iterations +1;
            }

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                taxa[i, 0] = theta_new[i] * taxaglobal;
            }

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                double soma = taxa[i,0];
                for (int j = 0; j < shape[i].NumeroVizinhos; j++)
                {
                    soma += taxa[shape[i].ListaIndicesVizinhos[j],0];
                }

                taxa[i, 0] = soma / (shape[i].NumeroVizinhos+1);
            }

            if (m_usa_multiplicador_taxas)
            {
                for (int i = 0; i < taxa.GetLength(0); i++)
                {
                    taxa[i, 0] = taxa[i, 0] * m_valor_multiplicador_taxas;
                }
            }


            return (taxa);
        }



        private double[,] TaxaBayesClaytonLogNEsp(double[] populacao, double[] casos)
        {
            IpeaGeo.RegressoesEspaciais.clsIpeaShape shape = m_shape;

            double[,] taxa = new double[populacao.GetLength(0), 1];
            double[] risco = new double[populacao.GetLength(0)];
            double[] valores_esperados = new double[populacao.GetLength(0)];
            double[] valores_observados = new double[populacao.GetLength(0)];
            double somapop = 0;
            double somacasos = 0;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                somapop += populacao[i];
                somacasos += casos[i];
            }
            double taxaglobal = somacasos / somapop;

                 
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                valores_esperados[i] = taxaglobal * populacao[i];
                valores_observados[i] = casos[i];
                risco[i] = (valores_observados[i] + 0.5) / (valores_esperados[i] + 0.000001);
            }
            double[] b_new = new double[populacao.GetLength(0)];
            double sigma2_old = 0.0;
            double phi_old = 0.0;
            //double[] sigma2_path = new double[num_iterations];
            //double[] phi_path = new double[num_iterations];
            //double[] min_lnormal_path = new double[num_iterations];
            //double[] max_lnormal_path = new double[num_iterations];

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                b_new[i] = Math.Log(risco[i]);
            }

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                phi_old += b_new[i];
            }

            phi_old /= (double)populacao.GetLength(0);
            double phi_new = phi_old;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                sigma2_old += Math.Pow((b_new[i] - phi_old), 2);
            }
            sigma2_old /= (double)(populacao.GetLength(0) - 1);


            double aux3 = 0.0;
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux3 += 1 / (1 + (sigma2_old * (valores_observados[i] + 0.5)));
            }
            aux3 = sigma2_old * aux3;

            double aux4 = 0;
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux4 += Math.Pow((b_new[i] - phi_new), 2);
            }
            double sigma2_new = (aux3 + aux4) / (double)populacao.GetLength(0);

            double[] aux5 = new double[populacao.GetLength(0)];
            double[] aux6 = new double[populacao.GetLength(0)];
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux5[i] = sigma2_new * (valores_observados[i] + 0.5) * Math.Log((valores_observados[i] + 0.5) / (valores_esperados[i] + 0.000001));
                aux6[i] = 1 + (valores_observados[i] + 0.5) * sigma2_new;
                b_new[i] = (phi_new + aux5[i] - (sigma2_new / 2)) / aux6[i];
            }

            int num_iterations = 1;

            while ((Math.Abs(sigma2_old - sigma2_new) > (Math.Abs(sigma2_old - sigma2_new) * tolerancia) || Math.Abs(phi_old - phi_new) > (Math.Abs(phi_old - phi_new) * tolerancia)) &&
                   (num_iterations <= simulacoes))
            {

                sigma2_old = sigma2_new;
                phi_old = phi_new;
                phi_new = m_clt.Mean(b_new);
                aux3 = 0.0;
                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux3 += 1 / (1 + (sigma2_old * (valores_observados[i] + 0.5)));
                }

                aux3 = sigma2_old * aux3;

                aux4 = 0;
                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux4 += Math.Pow((b_new[i] - phi_new), 2);
                }
                sigma2_new = (aux3 + aux4) / (double)populacao.GetLength(0);

                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux5[i] = sigma2_new * (valores_observados[i] + 0.5) * Math.Log((valores_observados[i] + 0.5) / (valores_esperados[i] + 0.000001));
                    aux6[i] = 1 + (valores_observados[i] + 0.5) * sigma2_new;
                    b_new[i] = (phi_new + aux5[i] - (sigma2_new / 2)) / aux6[i];
                }
                num_iterations = num_iterations +1;
            }

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                taxa[i, 0] = Math.Exp(b_new[i]) * taxaglobal;
            }

            //clsIpeaShape shape = (IpeaGEO.clsIpeaShape)m_shape;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                double soma = taxa[i,0];
                for (int j = 0; j < shape[i].NumeroVizinhos; j++)
                {
                    soma += taxa[shape[i].ListaIndicesVizinhos[j],0];
                }

                taxa[i, 0] = soma / (shape[i].NumeroVizinhos + 1);
            }



            if (m_usa_multiplicador_taxas)
            {
                for (int i = 0; i < taxa.GetLength(0); i++)
                {
                    taxa[i, 0] = taxa[i, 0] * m_valor_multiplicador_taxas;
                }
            }

            return (taxa);
        }

        private double[,] RRelBayesClaytonGamaEsp(double[] populacao, double[] casos)
        {
            IpeaGeo.RegressoesEspaciais.clsIpeaShape shape = m_shape;
            //clsIpeaShape shape = (IpeaGEO.clsIpeaShape)m_shape;

            double[,] taxa = new double[populacao.GetLength(0), 1];
            double[] risco = new double[populacao.GetLength(0)];
            double[] valores_esperados = new double[populacao.GetLength(0)];
            double[] valores_observados = new double[populacao.GetLength(0)];
            double somapop = 0;
            double somacasos = 0;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                somapop += populacao[i];
                somacasos += casos[i];
            }
            double taxaglobal = somacasos / somapop;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                valores_esperados[i] = taxaglobal * populacao[i];
                valores_observados[i] = casos[i];
                risco[i] = valores_observados[i] / (valores_esperados[i] + 0.000001);
            }
            double mean0 = 0;
            double var0 = 0;
            double alpha_new = 1;
            double nu_new = 1;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                mean0 += risco[i];
            }
            mean0 /= (double)populacao.GetLength(0);

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                var0 += Math.Pow((risco[i] - mean0), 2);
            }
            var0 /= (populacao.GetLength(0) - 1);

            alpha_new = mean0 / var0;
            nu_new = Math.Pow(mean0, 2) / var0;

            double[] theta_new = new double[populacao.GetLength(0)];
            int num_iterations = 1;
            //double[] alpha_path = new double[num_iterations];
            //double[] nu_path = new double[num_iterations];
            //double[] min_gamma_path = new double[num_iterations];
            //double[] max_gamma_path = new double[num_iterations];
            double aux1old = mean0;
            double aux2old = var0;
            double aux2a = new double();

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                theta_new[i] = ((double)valores_observados[i] + nu_new) / ((double)valores_esperados[i] + alpha_new+0.000001);
            }

            double aux1 = m_clt.Mean(theta_new);
            double aux2 = 0;
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux2a += (1 + (alpha_new / (double)(valores_esperados[i] + 0.000001))) * Math.Pow((theta_new[i] - aux1), 2);
            }

            aux2 = (1 / (populacao.GetLength(0) - 1.0)) * aux2a;

            while ((Math.Abs(aux1old - aux1) > (Math.Abs(aux1old - aux1) * tolerancia) || Math.Abs(aux2old - aux2) > (Math.Abs(aux2old - aux2) * tolerancia)) &&
                   (num_iterations <= simulacoes))
            {
                alpha_new = aux1 / aux2;
                nu_new = aux1 * alpha_new;

                theta_new[0] = ((double)valores_observados[0] + nu_new) / ((double)valores_esperados[0] + alpha_new + 0.000001);
                //min_gamma_path[num_iterations] = theta_new[0];
                //max_gamma_path[num_iterations] = theta_new[0];

                for (int i = 1; i < populacao.GetLength(0); i++)
                {
                    theta_new[i] = ((double)valores_observados[i] + nu_new) / ((double)valores_esperados[i] + alpha_new+0.000001);

                    //if (theta_new[i] < theta_new[i - 1])
                    //{
                    //    min_gamma_path[num_iterations] = theta_new[i];
                    //}
                    //else
                    //{
                    //    max_gamma_path[num_iterations] = theta_new[i];
                    //}
                }

                aux1old = aux1;
                aux2old = aux2;
                aux1 = aux2a = aux2 = 0;

                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux1 += theta_new[i];
                }
                aux1 /= (double)populacao.GetLength(0);

                for (int i = 0; i < populacao.GetLength(0); i++)
                {

                    aux2a += (1 + (alpha_new / (double)(valores_esperados[i] + 0.000001))) * Math.Pow((theta_new[i] - aux1), 2);
                }

                aux2 = (1 / (double)(populacao.GetLength(0) - 1.0)) * aux2a;

                //alpha_path[num_iterations] = alpha_new;
                //nu_path[num_iterations] = nu_new; 
                num_iterations = num_iterations +1;
            }

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                taxa[i, 0] = theta_new[i];
            }

            //clsIpeaShape shape = (IpeaGEO.clsIpeaShape)m_shape;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                double soma = taxa[i,0];
                for (int j = 0; j < shape[i].NumeroVizinhos; j++)
                {
                    soma += taxa[shape[i].ListaIndicesVizinhos[j],0];
                }

                taxa[i, 0] = soma / (double)(shape[i].NumeroVizinhos + 1);
            }

            return (taxa);
        }

        private double[,] RRelBayesClaytonLogNEsp(double[] populacao, double[] casos)
        {
            IpeaGeo.RegressoesEspaciais.clsIpeaShape shape = m_shape;

            double[,] taxa = new double[populacao.GetLength(0), 1];
            double[] risco = new double[populacao.GetLength(0)];
            double[] valores_esperados = new double[populacao.GetLength(0)];
            double[] valores_observados = new double[populacao.GetLength(0)];
            double somapop = 0.0;
            double somacasos = 0.0;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                somapop += populacao[i];
                somacasos += casos[i];
            }
            double taxaglobal = somacasos / somapop;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                valores_esperados[i] = taxaglobal * populacao[i];
                valores_observados[i] = casos[i];
                risco[i] = (valores_observados[i] + 0.5) / (valores_esperados[i] + 0.000001);
            }
            double[] b_new = new double[populacao.GetLength(0)];
            double sigma2_old = 0.0;
            double phi_old = 0.0;
            //double[] sigma2_path = new double[num_iterations];
            //double[] phi_path = new double[num_iterations];
            //double[] min_lnormal_path = new double[num_iterations];
            //double[] max_lnormal_path = new double[num_iterations];

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                b_new[i] = Math.Log(risco[i]);
            }

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                phi_old += b_new[i];
            }

            phi_old /= (double)populacao.GetLength(0);
            double phi_new = phi_old;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                sigma2_old += Math.Pow((b_new[i] - phi_old), 2);
            }
            sigma2_old /= (double)(populacao.GetLength(0) - 1);

            double aux3 = 0.0;
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux3 += 1 / (1 + (sigma2_old * (valores_observados[i] + 0.5)));
            }
            aux3 = sigma2_old * aux3;

            double aux4 = 0;
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux4 += Math.Pow((b_new[i] - phi_new), 2);
            }
            double sigma2_new = (aux3 + aux4) / (double)populacao.GetLength(0);

            double[] aux5 = new double[populacao.GetLength(0)];
            double[] aux6 = new double[populacao.GetLength(0)];
            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                aux5[i] = sigma2_new * (valores_observados[i] + 0.5) * Math.Log((valores_observados[i] + 0.5) / (valores_esperados[i] + 0.000001));
                aux6[i] = 1 + (valores_observados[i] + 0.5) * sigma2_new;
                b_new[i] = (phi_new + aux5[i] - (sigma2_new / 2)) / aux6[i];
            }

            int num_iterations = 1;

            while ((Math.Abs(sigma2_old - sigma2_new) > (Math.Abs(sigma2_old - sigma2_new) * tolerancia) || Math.Abs(phi_old - phi_new) > (Math.Abs(phi_old - phi_new) * tolerancia)) &&
                   (num_iterations <= simulacoes))
            {
                sigma2_old = sigma2_new;
                phi_old = phi_new;
                phi_new = m_clt.Mean(b_new);
                aux3 = 0.0;
                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux3 += 1 / (1 + (sigma2_old * (valores_observados[i] + 0.5)));
                }

                aux3 = sigma2_old * aux3;

                aux4 = 0;
                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux4 += Math.Pow((b_new[i] - phi_new), 2);
                }
                sigma2_new = (aux3 + aux4) / (double)populacao.GetLength(0);

                for (int i = 0; i < populacao.GetLength(0); i++)
                {
                    aux5[i] = sigma2_new * (valores_observados[i] + 0.5) * Math.Log((valores_observados[i] + 0.5) / (valores_esperados[i] + 0.000001));
                    aux6[i] = 1 + (valores_observados[i] + 0.5) * sigma2_new;
                    b_new[i] = (phi_new + aux5[i] - (sigma2_new / 2)) / aux6[i];
                }
                num_iterations = num_iterations +1;
            }

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                taxa[i, 0] = Math.Exp(b_new[i]);
            }
            
            //clsIpeaShape shape = (IpeaGEO.clsIpeaShape)m_shape;

            for (int i = 0; i < populacao.GetLength(0); i++)
            {
                double soma = taxa[i, 0];
                for (int j = 0; j < shape[i].NumeroVizinhos; j++)
                {
                    soma += taxa[shape[i].ListaIndicesVizinhos[j], 0];
                }

                taxa[i, 0] = soma / (shape[i].NumeroVizinhos + 1);
            }

            return (taxa);
        }

        #endregion
    }
}
