using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;
using System.Collections;

namespace IpeaGEO.RegressoesEspaciais
{
    public class BLRegressaoGMMConley
    {
        public BLRegressaoGMMConley()
        {
        }

        #region Variáveis

        private DataTable m_dt_tabela_dados = new DataTable();
        private string[] m_variaveis_dependentes = new string[0];
        private string[] m_variaveis_independentes = new string[0];
        private string[] m_variaveis_instrumentais = new string[0];
        private bool m_usa_intercepto = true;
        private string m_output_text = "";
        private string m_output_variaveis_geradas = "";
        private bool m_adiciona_novas_variaveis = false;

        public bool AdicionaNovaVariaveis
        {
            get
            {
                return m_adiciona_novas_variaveis;
            }
            set
            {
            	m_adiciona_novas_variaveis = value;
            }
        }

        //private DataTable m_dt_variaveis_geradas = new DataTable();

        private bool m_apresenta_covmatrix_beta_hat = false;
        private TipoCalculoLogDetWMatrix m_tipo_logdet = TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo;

        public TipoCalculoLogDetWMatrix TipoCalculoLogDetW
        {
            get
            {
                return this.m_tipo_logdet;
            }
            set
            {
                this.m_tipo_logdet = value;
            }
        }

        //public DataTable DtVariaveisGeradas
        //{
        //    get
        //    {
        //        return m_dt_variaveis_geradas;
        //    }
        //    set
        //    {
        //        m_dt_variaveis_geradas = value;
        //    }
        //}

        public bool ApresentaCovMatrixBetaHat { set { this.m_apresenta_covmatrix_beta_hat = value; } }

        private string m_variavel_coord_X = "";
        private string m_variavel_coord_Y = "";
        private double m_cutoff_X = 0.0;
        private double m_cutoff_Y = 0.0;

        public string VariavelCoordenadaX { set { this.m_variavel_coord_X = value; } }
        public string VariavelCoordenadaY { set { this.m_variavel_coord_Y = value; } }
        public double CutOffCoordenadaX { set { this.m_cutoff_X = value; } }
        public double CutOffCoordenadaY { set { this.m_cutoff_Y = value; } }
        
        private clsLinearRegressionModelsMLE m_geomle = new clsLinearRegressionModelsMLE();
        private int num_nonzero_elementos_matriz_W = 0;

        private clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            get { return this.m_shape.Clone(); }
            set { this.m_shape = value.Clone(); }
        }

        private TipoMatrizVizinhanca m_tipo_matriz_vizinhanca = TipoMatrizVizinhanca.Normalizada;
        public TipoMatrizVizinhanca TipoMatrizVizinhanca
        {
            set { this.m_tipo_matriz_vizinhanca = value; }
            get { return this.m_tipo_matriz_vizinhanca; }
        }

        public string VariaveisGeradas
        {
            get { return this.m_output_variaveis_geradas; }
        }

        public string ResultadoEstimacao
        {
            get { return this.m_output_text; }
        }

        public bool IncluiIntercepto
        {
            get { return this.m_usa_intercepto; }
            set { this.m_usa_intercepto = value; }
        }

        public DataTable TabelaDados
        {
            set {this.m_dt_tabela_dados = value;}
            get { return this.m_dt_tabela_dados; }
        }

        public string[] VariaveisDependentes
        {
            set {this.m_variaveis_dependentes = value;}
            get { return this.m_variaveis_dependentes; }
        }

        public string[] VariaveisIndependentes
        {
            set {this.m_variaveis_independentes = value;}
            get { return this.m_variaveis_independentes; }
        }

        public string[] VariaveisInstrumentais
        {
            set {this.m_variaveis_instrumentais = value;}
            get { return this.m_variaveis_instrumentais; }
        }

        public double GeraMaximaDistancia(DataTable dt, string variavel)
        {
            clsUtilTools clt = new clsUtilTools();
            double[,] w = clt.GetMatrizFromDataTable(dt, variavel);

            return Math.Abs(clt.Max(w) - clt.Min(w));
        }

        #endregion

        #region Análise de multicolinearidade

        public string AnaliseMulticolinearidade()
        {
            clsUtilTools clt = new clsUtilTools();

            string[] variaveis_na_base;
            string[] Wvariaveis_na_base;
            SeparaBlocosVariaveis(out variaveis_na_base, out Wvariaveis_na_base, VariaveisIndependentes);

            double[,] X = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, variaveis_na_base);
            double[,] WX = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, Wvariaveis_na_base);
            if (WX.GetLength(1) > 0)
            {
                clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
                this.GeraMatrizVizinhanca();
                WX = fme.MatrizMult(this.m_geomle.Wesparsa, WX);
                X = clt.Concateh(X, WX);
            }

            double[,] corr_mat = clt.CorrSampleMatrix(X);
            
            string out_text = "============================================================================================================================\n\n";

            out_text += "Análise de multicolinearidade dos regressores\n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de variáveis: " + X.GetLength(1) + "\n";

            out_text += "\n";
            out_text += "Matriz de correlação: \n\n";

            out_text += this.GeraTabelaCovMatrix(corr_mat, VariaveisIndependentes, false);

            out_text += "\n";

            out_text += "Auto-valores da matrix X'X: \n\n";
            
            if (this.m_usa_intercepto)
            {
                X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
            }

            double[,] XX = clt.MatrizMult(clt.MatrizTransp(X), X);
            double[,] V = new double[0, 0];
            double[] D = new double[0];

            clt.AutovaloresMatrizSimetrica(XX, ref V, ref D);

            string a = this.ImprimeVetorColuna(D);

            a += "\n";

            a += "Razão entre o maior e menor auto-valor: " 
                + (D[D.GetLength(0)-1] != 0 ? clt.Double2Texto(D[0] / D[D.GetLength(0)-1], 6) : "Infinito");

            a += "\n\n";

            return out_text + a;
        }

        #endregion

        #region Matriz de vizinhança

        public void GeraMatrizVizinhanca()
        {
            m_geomle.Shape = this.Shape;

            if (m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Normalizada)
            {
                //m_geomle.MatrizWFromVizinhosNorm();
                m_geomle.MatrizWesparsaFromVizinhosNorm();
            }

            if (m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Original)
            {
                //m_geomle.MatrizWFromVizinhos();
                m_geomle.MatrizWesparsaFromVizinhos();
            }

            //m_geomle.MatrizWFromVizinhos();

            //num_nonzero_elementos_matriz_W = m_geomle.NumNonZeroElementsMatrizW;
            //double[] rows_sum = m_geomle.RowsSumFromDenseW();

            //m_geomle.MatrizWesparsaFromVizinhos();
            //double[,] w = m_geomle.Wesparsa.AsDoubleMatrix;

            //clsUtilTools clt = new clsUtilTools();
            //double[,] diff = clt.MatrizSubtracao(w, m_geomle.Wmatriz);
            //double norm = clt.Norm(diff);

            //m_geomle.MatrizWesparsaFromVizinhosNorm();
            //m_geomle.MatrizWFromVizinhosNorm();

            //double[,] wn = m_geomle.Wesparsa.AsDoubleMatrix;

            //double[,] diffn = clt.MatrizSubtracao(wn, m_geomle.Wmatriz);
            //double normn = clt.Norm(diffn);
        }

        #endregion

        #region Estimação modelos SAR
        
        public void EstimaModelosSAR()
        {
            clsUtilTools clt = new clsUtilTools();

            string[] variaveis_na_base;
            string[] Wvariaveis_na_base;
            SeparaBlocosVariaveis(out variaveis_na_base, out Wvariaveis_na_base, VariaveisIndependentes);

            double[,] X = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, variaveis_na_base);
            double[,] WX = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, Wvariaveis_na_base);
            if (WX.GetLength(1) > 0)
            {
                clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
                this.GeraMatrizVizinhanca();
                WX = fme.MatrizMult(this.m_geomle.Wesparsa, WX);
                X = clt.Concateh(X, WX);
            }

            double[,] Y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisDependentes);

            if (this.m_usa_intercepto)
            {
                X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
            }

            //-------- checando singularidade da matriz X ------------//
            double[,] invX = clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(X), X));

            m_geomle.X = X;
            m_geomle.Y = Y;
            m_geomle.TipoCalculoLogDetW = this.m_tipo_logdet;
            m_geomle.TipoModeloRegressaoEspacial = TipoModeloEspacial.SAR;

            m_geomle.EstimateModeloSAR();

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação do Modelo SAR via Máxima Verossimilhança \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Variância dos erros: " + clt.Double2Texto(m_geomle.Sigma2Hat, 6) + "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, m_geomle.BetaHat,
                m_geomle.BetaStdError, m_geomle.BetaTStat, m_geomle.BetaPValue, m_usa_intercepto);

            out_text += "\nLog-likelihood: " + clt.Double2Texto(m_geomle.LogLik, 6) + "\n";
            out_text += "AIC: " + clt.Double2Texto(m_geomle.AIC, 6) + "\n";
            out_text += "BIC: " + clt.Double2Texto(m_geomle.BIC, 6) + "\n\n";

            out_text += "Rho (coeficiente): " + clt.Double2Texto(m_geomle.RhoHat, 6) + "\n";
            out_text += "Rho (erro padrão): " + clt.Double2Texto(m_geomle.RhoStdError, 6) + "\n";
            out_text += "Rho (estatística t): " + clt.Double2Texto(m_geomle.RhoTStat, 6) + "\n";
            out_text += "Rho (p-valor): " + clt.Double2Texto(m_geomle.RhoPValue, 6) + "\n";
            out_text += "Rho (lim. inf. 95%): " + clt.Double2Texto(m_geomle.RhoLimInfCI, 6) + "\n";
            out_text += "Rho (lim. sup. 95%): " + clt.Double2Texto(m_geomle.RhoLimSupCI, 6) + "\n\n";

            out_text += "Teste da razão de verossimilhança (estatística teste): " + clt.Double2Texto(m_geomle.LikelihoodRatioTestStatRho, 6) + "\n";
            out_text += "Teste da razão de verossimilhança (p-valor): " + clt.Double2Texto(m_geomle.LikelihoodRatioTestPvalueRho, 6) + "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(m_geomle.BetaHatCovMatrix, VariaveisIndependentes, m_usa_intercepto);
            }

            this.m_output_text = out_text;

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação via OLS\n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(m_geomle.VariaveisGeradas, m_geomle.NomesVariaveisGeradas);

            AdicionaNovasVariaveisToDataTable(m_geomle.VariaveisGeradas, m_geomle.NomesVariaveisGeradas);
        }

        #endregion

        #region Estimação modelos SEM

        public void EstimaModelosSEM()
        {
            clsUtilTools clt = new clsUtilTools();

            string[] variaveis_na_base;
            string[] Wvariaveis_na_base;
            SeparaBlocosVariaveis(out variaveis_na_base, out Wvariaveis_na_base, VariaveisIndependentes);

            double[,] X = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, variaveis_na_base);
            double[,] WX = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, Wvariaveis_na_base);
            if (WX.GetLength(1) > 0)
            {
                clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
                this.GeraMatrizVizinhanca();
                WX = fme.MatrizMult(this.m_geomle.Wesparsa, WX);
                X = clt.Concateh(X, WX);
            }

            double[,] Y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisDependentes);

            if (this.m_usa_intercepto)
            {
                X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
            }

            //-------- checando singularidade da matriz X ------------//
            double[,] invX = clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(X), X));

            m_geomle.X = X;
            m_geomle.Y = Y;
            m_geomle.TipoCalculoLogDetW = this.m_tipo_logdet;
            m_geomle.TipoModeloRegressaoEspacial = TipoModeloEspacial.SEM;
            
            m_geomle.EstimateModeloSEM();

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação do Modelo SEM via Máxima Verossimilhança \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Variância dos erros: " + clt.Double2Texto(m_geomle.Sigma2Hat, 6) + "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, m_geomle.BetaHat, 
                m_geomle.BetaStdError, m_geomle.BetaTStat, m_geomle.BetaPValue, m_usa_intercepto);
            
            out_text += "\nLog-likelihood: " + clt.Double2Texto(m_geomle.LogLik, 6) + "\n";
            out_text += "AIC: " + clt.Double2Texto(m_geomle.AIC, 6) + "\n";
            out_text += "BIC: " + clt.Double2Texto(m_geomle.BIC, 6) + "\n\n";

            out_text += "Rho (coeficiente): " + clt.Double2Texto(m_geomle.RhoHat, 6) + "\n";
            out_text += "Rho (erro padrão): " + clt.Double2Texto(m_geomle.RhoStdError, 6) + "\n";
            out_text += "Rho (estatística t): " + clt.Double2Texto(m_geomle.RhoTStat, 6) + "\n";
            out_text += "Rho (p-valor): " + clt.Double2Texto(m_geomle.RhoPValue, 6) + "\n";
            out_text += "Rho (lim. inf. 95%): " + clt.Double2Texto(m_geomle.RhoLimInfCI, 6) + "\n";
            out_text += "Rho (lim. sup. 95%): " + clt.Double2Texto(m_geomle.RhoLimSupCI, 6) + "\n\n";

            out_text += "Teste da razão de verossimilhança (estatística teste): " + clt.Double2Texto(m_geomle.LikelihoodRatioTestStatRho, 6) + "\n";
            out_text += "Teste da razão de verossimilhança (p-valor): " + clt.Double2Texto(m_geomle.LikelihoodRatioTestPvalueRho, 6) + "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(m_geomle.BetaHatCovMatrix, VariaveisIndependentes, m_usa_intercepto);
            }

            this.m_output_text = out_text;

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação via OLS\n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(m_geomle.VariaveisGeradas, m_geomle.NomesVariaveisGeradas);

            AdicionaNovasVariaveisToDataTable(m_geomle.VariaveisGeradas, m_geomle.NomesVariaveisGeradas);
        }

        #endregion

        #region Bloco de variáveis W x X

        private void SeparaBlocosVariaveis(out string[] variaveis_na_base, out string[] Wvariaveis_na_base, string[] lista_variaveis)
        {
            ArrayList xvs = new ArrayList();
            ArrayList wsvs = new ArrayList();

            for (int i = 0; i < lista_variaveis.GetLength(0); i++)
            {
                if (lista_variaveis[i].Length > 3 && lista_variaveis[i].Substring(0, 4) == "W_x_")
                {
                    wsvs.Add(lista_variaveis[i].Substring(4));
                }
                else
                {
                    xvs.Add(lista_variaveis[i]);
                }
            }

            variaveis_na_base = new string[xvs.Count];
            Wvariaveis_na_base = new string[wsvs.Count];

            for (int i = 0; i < xvs.Count; i++)
            {
                variaveis_na_base[i] = xvs[i].ToString();
            }

            for (int i = 0; i < wsvs.Count; i++)
            {
                Wvariaveis_na_base[i] = wsvs[i].ToString();
            }
        }

        #endregion

        #region Estimação GMM spacial

        public void EstimaEspacialGMM()
        {
            clsUtilTools clt = new clsUtilTools();

            string[] variaveis_na_base;
            string[] Wvariaveis_na_base;

            SeparaBlocosVariaveis(out variaveis_na_base, out Wvariaveis_na_base, VariaveisIndependentes);
            double[,] X = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, variaveis_na_base);
            double[,] WX = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, Wvariaveis_na_base);

            SeparaBlocosVariaveis(out variaveis_na_base, out Wvariaveis_na_base, VariaveisInstrumentais);
            double[,] Z = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, variaveis_na_base);
            double[,] WZ = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, Wvariaveis_na_base);

            if (WX.GetLength(1) + WZ.GetLength(1) > 0)
            {
                clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
                this.GeraMatrizVizinhanca();

                if (WX.GetLength(1) > 0)
                {
                    WX = fme.MatrizMult(this.m_geomle.Wesparsa, WX);
                    X = clt.Concateh(X, WX);
                }

                if (WZ.GetLength(1) > 0)
                {
                    WZ = fme.MatrizMult(this.m_geomle.Wesparsa, WZ);
                    Z = clt.Concateh(Z, WZ);
                }
            }

            double[,] Y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisDependentes);

            if (Z.GetLength(1) < X.GetLength(1))
            {
                Z = clt.ArrayDoubleClone(X);
                VariaveisInstrumentais = VariaveisIndependentes;
            }

            if (this.m_usa_intercepto)
            {
                X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
                Z = clt.Concateh(clt.Ones(Z.GetLength(0), 1), Z);
            }

            double[,] coord_x = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, m_variavel_coord_X);
            double[,] coord_y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, m_variavel_coord_Y);

            clsSpatialGMM gmm = new clsSpatialGMM();

            double[,] beta = new double[0, 0];
            double[,] covbeta = new double[0, 0];
            double[,] stderror = new double[0, 0];
            double[,] tstat = new double[0, 0];
            double[,] pvalue = new double[0, 0];
            double sigma2 = 0.0;
            double Jstat = 0.0;
            double Jpvalue = 0.0;

            gmm.Limited_info_spatial_GMM_estimation(Y, X, Z, coord_x, coord_y, 
                this.m_cutoff_X, this.m_cutoff_Y, ref beta, ref covbeta, ref stderror, ref tstat, ref pvalue,
                ref Jstat, ref Jpvalue, ref sigma2);

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação via GMM Espacial (Conley, 1999)\n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Variância dos erros: " + clt.Double2Texto(sigma2, 6) + "\n\n";
            out_text += "Estatística J: " + clt.Double2Texto(Jstat, 6) + "\n";
            out_text += "P-valor J: " + clt.Double2Texto(Jpvalue, 6) + "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, beta, stderror, tstat, pvalue, m_usa_intercepto);

            out_text += "\n";
            out_text += "Variáveis instrumentais utilizadas na estimação: \n\n";
            if (m_usa_intercepto) out_text += "Constante" + "\n";
            for (int i = 0; i < VariaveisInstrumentais.GetLength(0); i++) out_text += VariaveisInstrumentais[i] + "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(covbeta, VariaveisIndependentes, m_usa_intercepto);
            }

            this.m_output_text = out_text;

            double[,] variaveis_geradas = gmm.VariaveisGeradas;

            string[] nomes_variaveis = new string[5];
            nomes_variaveis[0] = "Observacao";
            nomes_variaveis[1] = "Y_observado";
            nomes_variaveis[2] = "Y_predito";
            nomes_variaveis[3] = "Residuo";
            nomes_variaveis[4] = "Residuo_padronizado";

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação via GMM Espacial\n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);

            AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);
        }

        #endregion

        #region Estimação GMM não espacial

        public void EstimaNaoEspacialGMM()
        {
            clsUtilTools clt = new clsUtilTools();

            string[] variaveis_na_base;
            string[] Wvariaveis_na_base;

            SeparaBlocosVariaveis(out variaveis_na_base, out Wvariaveis_na_base, VariaveisIndependentes);
            double[,] X = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, variaveis_na_base);
            double[,] WX = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, Wvariaveis_na_base);

            SeparaBlocosVariaveis(out variaveis_na_base, out Wvariaveis_na_base, VariaveisInstrumentais);
            double[,] Z = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, variaveis_na_base);
            double[,] WZ = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, Wvariaveis_na_base);

            if (WX.GetLength(1) + WZ.GetLength(1) > 0)
            {
                clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
                this.GeraMatrizVizinhanca();

                if (WX.GetLength(1) > 0)
                {
                    WX = fme.MatrizMult(this.m_geomle.Wesparsa, WX);
                    X = clt.Concateh(X, WX);
                }

                if (WZ.GetLength(1) > 0)
                {
                    WZ = fme.MatrizMult(this.m_geomle.Wesparsa, WZ);
                    Z = clt.Concateh(Z, WZ);
                }
            }

            double[,] Y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisDependentes);

            if (Z.GetLength(1) < X.GetLength(1))
            {
                Z = clt.ArrayDoubleClone(X);
                VariaveisInstrumentais = VariaveisIndependentes;
            }

            if (this.m_usa_intercepto)
            {
                X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
                Z = clt.Concateh(clt.Ones(Z.GetLength(0), 1), Z);
            }

            clsSpatialGMM gmm = new clsSpatialGMM();

            double[,] beta = new double[0, 0];
            double[,] covbeta = new double[0, 0];
            double[,] stderror = new double[0, 0];
            double[,] tstat = new double[0, 0];
            double[,] pvalue = new double[0, 0];
            double sigma2 = 0.0;
            double Jstat = 0.0;
            double Jpvalue = 0.0;

            gmm.Limited_info_GMM_estimation(Y, X, Z, ref beta, ref covbeta, ref stderror, ref tstat, ref pvalue,
                ref Jstat, ref Jpvalue, ref sigma2);

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação via GMM\n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Variância dos erros: " + clt.Double2Texto(sigma2, 6) + "\n\n";
            out_text += "Estatística J: " + clt.Double2Texto(Jstat, 6) + "\n";
            out_text += "P-valor J: " + clt.Double2Texto(Jpvalue, 6) + "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, beta, stderror, tstat, pvalue, m_usa_intercepto);

            out_text += "\n";
            out_text += "Variáveis instrumentais utilizadas na estimação: \n\n";
            if (m_usa_intercepto) out_text += "Constante" + "\n";
            for (int i = 0; i < VariaveisInstrumentais.GetLength(0); i++) out_text += VariaveisInstrumentais[i] + "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(covbeta, VariaveisIndependentes, m_usa_intercepto);
            }

            this.m_output_text = out_text;

            double[,] variaveis_geradas = gmm.VariaveisGeradas;

            string[] nomes_variaveis = new string[5];
            nomes_variaveis[0] = "Observacao";
            nomes_variaveis[1] = "Y_observado";
            nomes_variaveis[2] = "Y_predito";
            nomes_variaveis[3] = "Residuo";
            nomes_variaveis[4] = "Residuo_padronizado";

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação via GMM\n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);

            AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);
        }

        #endregion

        #region Estimação 2SLS

        public void Estima2SLSRegression()
        {
            clsUtilTools clt = new clsUtilTools();

            string[] variaveis_na_base;
            string[] Wvariaveis_na_base;

            SeparaBlocosVariaveis(out variaveis_na_base, out Wvariaveis_na_base, VariaveisIndependentes);
            double[,] X = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, variaveis_na_base);
            double[,] WX = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, Wvariaveis_na_base);

            SeparaBlocosVariaveis(out variaveis_na_base, out Wvariaveis_na_base, VariaveisInstrumentais);
            double[,] Z = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, variaveis_na_base);
            double[,] WZ = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, Wvariaveis_na_base);

            if (WX.GetLength(1) + WZ.GetLength(1) > 0)
            {
                clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
                this.GeraMatrizVizinhanca();
             
                if (WX.GetLength(1) > 0)
                {
                    WX = fme.MatrizMult(this.m_geomle.Wesparsa, WX);
                    X = clt.Concateh(X, WX);
                }

                if (WZ.GetLength(1) > 0)
                {
                    WZ = fme.MatrizMult(this.m_geomle.Wesparsa, WZ);
                    Z = clt.Concateh(Z, WZ);
                }
            }

            double[,] Y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisDependentes);

            if (Z.GetLength(1) < X.GetLength(1))
            {
                Z = clt.ArrayDoubleClone(X);
                VariaveisInstrumentais = VariaveisIndependentes;
            }

            if (this.m_usa_intercepto)
            {
                X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
                Z = clt.Concateh(clt.Ones(Z.GetLength(0), 1), Z);
            }

            clsSpatialGMM gmm = new clsSpatialGMM();

            double[,] beta = new double[0, 0];
            double[,] covbeta = new double[0, 0];
            double[,] stderror = new double[0, 0];
            double[,] tstat = new double[0, 0];
            double[,] pvalue = new double[0, 0];
            double sigma2 = 0.0;

            gmm.Simple_2SLS_estimation(Y, X, Z, ref beta, ref covbeta, ref stderror, ref tstat, ref pvalue, ref sigma2);

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação via 2SLS\n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Variância dos erros: " + clt.Double2Texto(sigma2, 6) + "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, beta, stderror, tstat, pvalue, m_usa_intercepto);

            out_text += "\n";
            out_text += "Variáveis instrumentais utilizadas na estimação: \n\n";
            if (m_usa_intercepto) out_text += "Constante" + "\n";
            for (int i = 0; i < VariaveisInstrumentais.GetLength(0); i++) out_text += VariaveisInstrumentais[i] + "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(covbeta, VariaveisIndependentes, m_usa_intercepto);
            }

            this.m_output_text = out_text;

            double[,] variaveis_geradas = gmm.VariaveisGeradas;

            string[] nomes_variaveis = new string[5];
            nomes_variaveis[0] = "Observacao";
            nomes_variaveis[1] = "Y_observado";
            nomes_variaveis[2] = "Y_predito";
            nomes_variaveis[3] = "Residuo";
            nomes_variaveis[4] = "Residuo_padronizado";

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação via 2SLS\n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);

            AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);
        }

        #endregion

        #region Estimação OLS

        public void EstimaOLSRegression()
        {
            clsUtilTools clt = new clsUtilTools();

            string[] variaveis_na_base;
            string[] Wvariaveis_na_base;
            SeparaBlocosVariaveis(out variaveis_na_base, out Wvariaveis_na_base, VariaveisIndependentes);

            double[,] X = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, variaveis_na_base);
            double[,] WX = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, Wvariaveis_na_base);
            if (WX.GetLength(1) > 0)
            {
                clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
                this.GeraMatrizVizinhanca();
                WX = fme.MatrizMult(this.m_geomle.Wesparsa, WX);
                X = clt.Concateh(X, WX);
            }            

            double[,] Y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisDependentes);

            if (this.m_usa_intercepto)
            {
                X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
            }

            clsSpatialGMM gmm = new clsSpatialGMM();

            double[,] beta = new double[0, 0];
            double[,] covbeta = new double[0, 0];
            double[,] stderror = new double[0, 0];
            double[,] tstat = new double[0, 0];
            double[,] pvalue = new double[0, 0];
            double sigma2 = 0.0;

            gmm.Simple_OLS_estimation(Y, X, ref beta, ref covbeta, ref stderror, ref tstat, ref pvalue, ref sigma2);

            string out_text = "============================================================================================================================\n\n";
                
            out_text += "Estimação via OLS\n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Variância dos erros: " + clt.Double2Texto(sigma2, 6) + "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, beta, stderror, tstat, pvalue, m_usa_intercepto);

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(covbeta, VariaveisIndependentes, m_usa_intercepto);
            }

            this.m_output_text = out_text;

            double[,] variaveis_geradas = gmm.VariaveisGeradas;

            string[] nomes_variaveis = new string[5];
            nomes_variaveis[0] = "Observacao";
            nomes_variaveis[1] = "Y_observado";
            nomes_variaveis[2] = "Y_predito";
            nomes_variaveis[3] = "Residuo";
            nomes_variaveis[4] = "Residuo_padronizado";

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação via OLS\n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);

            AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);
        }

        #endregion

        #region tabulação matriz de variância-covariância

        private string GeraTabelaCovMatrix(double[,] covmat, string[] variaveis_independentes, bool inclui_intercepto)
        {
            clsUtilTools clt = new clsUtilTools();
            string[] variaveis = new string[covmat.GetLength(0)];
            if (inclui_intercepto)
            {
                variaveis[0] = "Intercepto";
                for (int j = 0; j < variaveis_independentes.GetLength(0); j++)
                {
                    variaveis[j + 1] = variaveis_independentes[j];
                }
            }
            else
            {
                for (int j = 0; j < variaveis_independentes.GetLength(0); j++)
                {
                    variaveis[j] = variaveis_independentes[j];
                }
            }

            string[,] mat = new string[covmat.GetLength(0) + 1, covmat.GetLength(1) + 1];
            mat[0, 0] = " ";
            for (int j = 0; j < variaveis.GetLength(0); j++)
            {
                mat[0, j + 1] = variaveis[j];
                mat[j + 1, 0] = variaveis[j];
            }

            for (int i = 1; i < mat.GetLength(0); i++)
            {
                for (int j = 1; j < mat.GetLength(1); j++)
                {
                    mat[i, j] = clt.Double2Texto(covmat[i - 1, j - 1], 6);
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

        #endregion

        #region tabulação novas variáveis

        private void AdicionaNovasVariaveisToDataTable(double[,] dados, string[] variaveis)
        {
            if (m_adiciona_novas_variaveis)
            {
                for (int i = 0; i < variaveis.GetLength(0); i++)
                {
                    if (!this.m_dt_tabela_dados.Columns.Contains(variaveis[i]))
                    {
                        this.m_dt_tabela_dados.Columns.Add(variaveis[i], typeof(double));
                    }

                    for (int j = 0; j < dados.GetLength(0); j++)
                    {
                        this.m_dt_tabela_dados.Rows[j][variaveis[i]] = dados[j, i];
                    }
                }
            }
        }

        private string GeraTabelaNovasVariaveis(double[,] dados, string[] variaveis)
        {
            clsUtilTools clt = new clsUtilTools();

            int[] max_length = new int[variaveis.GetLength(0)];
            string[,] st_dados = new string[dados.GetLength(0), dados.GetLength(1)];
            for (int i = 0; i < st_dados.GetLength(0); i++)
            {
                st_dados[i, 0] = dados[i, 0].ToString();
                if (max_length[0] < st_dados[i, 0].Length) max_length[0] = st_dados[i, 0].Length;

                for (int j = 1; j < st_dados.GetLength(1); j++)
                {
                    st_dados[i, j] = clt.Double2Texto(dados[i, j], 6);
                    if (max_length[j] < st_dados[i, j].Length) max_length[j] = st_dados[i, j].Length;
                }
            }

            StringBuilder sb = new StringBuilder();

            for (int j = 0; j < variaveis.GetLength(0); j++)
            {
                if (max_length[j] < variaveis[j].Length) max_length[j] = variaveis[j].Length;
                max_length[j] = max_length[j] + 3;

                if (j == 0)
                {
                    sb.Append(variaveis[j] + PreencheEspacos(max_length[j] - variaveis[j].Length));
                }
                else
                {
                    sb.Append("\t" + PreencheEspacos(max_length[j] - variaveis[j].Length) + variaveis[j]);
                }
            }
            sb.Append("\n");

            for (int i = 0; i < dados.GetLength(0); i++)
            {
                sb.Append(st_dados[i, 0] + PreencheEspacos(max_length[0] - st_dados[i, 0].Length));

                for (int j = 1; j < dados.GetLength(1); j++)
                {
                    sb.Append("\t" + PreencheEspacos(max_length[j] - st_dados[i,j].Length) + st_dados[i,j]);
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }

        #endregion 

        #region tabulação estimações

        private string ImprimeVetorColuna(double[] v)
        {
            clsUtilTools clt = new clsUtilTools();
            string[] rv = new string[v.GetLength(0)];
            int max_length = 0;
            for (int i = 0; i < rv.GetLength(0); i++)
            {
                rv[i] = clt.Double2Texto(v[i], 6);
                if (rv[i].Length > max_length) max_length = rv[i].Length;
            }
            string r = "";
            for (int i = 0; i < rv.GetLength(0); i++)
            {
                r += "[" + this.PreencheEspacos(max_length - rv[i].Length) + rv[i] + "]\n";
            }
            return r;
        }

        private string GeraTabelaEstimacoes(string[] variaveis_independentes, double[,] beta, 
            double[,] stderror, double[,] tstat, double[,] pvalue, bool inclui_intercepto)
        {
            clsUtilTools clt = new clsUtilTools();
            string[] variaveis = new string[beta.GetLength(0)];

            if (inclui_intercepto)
            {
                variaveis[0] = "Intercepto";
                for (int j = 0; j < variaveis_independentes.GetLength(0); j++)
                {
                    variaveis[j + 1] = variaveis_independentes[j];
                }
            }
            else
            {
                for (int j = 0; j < variaveis_independentes.GetLength(0); j++)
                {
                    variaveis[j] = variaveis_independentes[j];
                }
            }

            int max_length_variaveis = 0;
            int max_length_beta = 0;
            int max_length_stderror = 0;
            int max_length_tstat = 0;
            int max_length_pvalue = 0;

            string[] st_beta = new string[beta.GetLength(0)];
            string[] st_stderror = new string[beta.GetLength(0)];
            string[] st_tstat = new string[beta.GetLength(0)];
            string[] st_pvalue = new string[beta.GetLength(0)];

            for (int i = 0; i < beta.GetLength(0); i++)
            {
                st_beta[i] = clt.Double2Texto(beta[i,0], 6);
                if (st_beta[i].Length > max_length_beta) max_length_beta = st_beta[i].Length;

                st_stderror[i] = clt.Double2Texto(stderror[i,0], 6);
                if (st_stderror[i].Length > max_length_stderror) max_length_stderror = st_stderror[i].Length;
                
                st_tstat[i] = clt.Double2Texto(tstat[i,0], 6);
                if (st_tstat[i].Length > max_length_tstat) max_length_tstat = st_tstat[i].Length;
                
                st_pvalue[i] = clt.Double2Texto(pvalue[i,0], 6);
                if (st_pvalue[i].Length > max_length_pvalue) max_length_pvalue = st_pvalue[i].Length;

                if (variaveis[i].Length > max_length_variaveis) max_length_variaveis = variaveis[i].Length;
            }

            if ("Variável".Length > max_length_variaveis) max_length_variaveis = "Variável".Length;
            if ("Coeficiente".Length > max_length_beta) max_length_beta = "Coeficiente".Length;
            if ("Erro padrão".Length > max_length_stderror) max_length_stderror = "Erro padrão".Length;
            if ("Estatística t".Length > max_length_tstat) max_length_tstat = "Estatística t".Length;
            if ("P-valor".Length > max_length_pvalue) max_length_pvalue = "P-valor".Length;

            max_length_variaveis += 2;
            max_length_beta += 6;
            max_length_pvalue += 6;
            max_length_stderror += 6;
            max_length_tstat += 6;

            string res = "";

            res += "Variável" + PreencheEspacos(max_length_variaveis - "Variável".Length);
            res += PreencheEspacos(max_length_beta - "Coeficiente".Length) + "Coeficiente";
            res += PreencheEspacos(max_length_stderror - "Erro padrão".Length) + "Erro padrão";
            res += PreencheEspacos(max_length_tstat - "Estatística t".Length) + "Estatística t";
            res += PreencheEspacos(max_length_pvalue - "P-valor".Length) + "P-valor";
            res += "\n\n";

            for (int i = 0; i < beta.GetLength(0); i++)
            {
                res += variaveis[i] + PreencheEspacos(max_length_variaveis - variaveis[i].Length);
                res += PreencheEspacos(max_length_beta - st_beta[i].Length) + st_beta[i];
                res += PreencheEspacos(max_length_stderror - st_stderror[i].Length) + st_stderror[i];
                res += PreencheEspacos(max_length_tstat - st_tstat[i].Length) + st_tstat[i];
                res += PreencheEspacos(max_length_pvalue - st_pvalue[i].Length) + st_pvalue[i];
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
    }
}
