using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;
using System.Collections;

namespace IpeaGeo.RegressoesEspaciais
{
    public class BLModelosCrossSectionSpaciaisLineares : BLModelosCrossSectionSpaciais
    {
        public BLModelosCrossSectionSpaciaisLineares()
        {
        }

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

            out_text += "Auto-valores da matrix SX'XS (a partir da matriz X normalizada): \n\n";

            double[,] X_noint = clt.ArrayDoubleClone(X);
            if (this.m_usa_intercepto)
            {
                X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
            }

            double[,] S = new double[X.GetLength(1), X.GetLength(1)];
            double[,] x;
            for (int i = 0; i<S.GetLength(0); i++)
            {
                x = clt.SubColumnArrayDouble(X, i);
                S[i,i] = 1.0 / Math.Sqrt(clt.MatrizMult(clt.MatrizTransp(x), x)[0,0]);
            }

            double[,] XX = clt.MatrizMult(clt.MatrizTransp(X), X);
            double[,] SXXS = clt.MatrizMult(clt.MatrizMult(S, XX), S);

            double[,] V = new double[0, 0];
            double[] D = new double[0];

            clt.AutovaloresMatrizSimetrica(SXXS, ref V, ref D);

            string a = this.ImprimeVetorColuna(D);

            a += "\n";

            a += "Raiz quadrada da razão entre o maior e menor auto-valor: " 
                + (D[D.GetLength(0)-1] != 0 ? clt.Double2Texto(Math.Sqrt(D[0] / D[D.GetLength(0)-1]), 6) : "Infinito");

            a += "\n\n";

            a += "OBS 1) A matriz (SX'SX) corresponde à matriz X'X normalizada, para evitar problemas de escala das variáveis. \n";
            a += "A razão acima é conhecida como condition number. A definição do condition number utilizado aqui pode ser encontrada \n";
            a += "na página 269, em W. H. Greene (1993), Econometric Analysis, Segunda Edição, Prentice Hall. Se o condition number for \n";
            a += "maior do que 20, há indicações de possíveis problemas causados por multicolinearidade.";

            a += "\n\n";

            double[,] yk;
            double[,] Xk;
            double[,] beta;
            double[,] yk_hat;
            double[,] R2k = new double[X_noint.GetLength(1), 1];
            double[,] Ck = new double[X_noint.GetLength(1), 1];
            for (int k = 0; k < X_noint.GetLength(1); k++)
            {
                yk = clt.SubColumnArrayDouble(X_noint, k);
                Xk = clt.Concateh(clt.Ones(X_noint.GetLength(0),1), clt.DeleteCol(X_noint, k));
                beta = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(Xk), Xk)), clt.MatrizMult(clt.MatrizTransp(Xk), yk));
                yk_hat = clt.MatrizMult(Xk, beta);
                R2k[k,0] = clt.Varianciac(yk_hat)[0,0] / clt.Varianciac(yk)[0,0];
                Ck[k,0] = Math.Sqrt(1.0 - R2k[k,0]);
            }

            a += IndicadoresDuasColunas(VariaveisIndependentes, R2k, Ck, "R2k", "Ck = (1 - R2k)^(1/2)");

            double[,] Y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisDependentes);

            yk = clt.ArrayDoubleClone(Y);
            Xk = clt.Concateh(clt.Ones(X_noint.GetLength(0), 1), X_noint);
            beta = clt.MatrizMult(clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(Xk), Xk)), clt.MatrizMult(clt.MatrizTransp(Xk), yk));
            yk_hat = clt.MatrizMult(Xk, beta);
            double R2 = clt.Varianciac(yk_hat)[0, 0] / clt.Varianciac(yk)[0, 0];

            a += "\n";

            a += "R2 para a regressão original (estimada via OLS): " + clt.Double2Texto(R2, 6);

            a += "\n\n";

            a += "OBS 2) O R2k na tabela acima corresponde ao R2 da regressão entre cada variável independente versus as demais. Quando o R2k \n";
            a += "é igual a 1, significa que existe uma relação linear perfeita entre a variável independentes k e as demais, indicando a \n";
            a += "presença de multicolinearidade perfeita. Em casos menos extremos, valores de R2k próximos a 1, indicam regressores próximos da \n";
            a += "multicolinearidade. Quando o R2 da regressão (estimada via OLS) original for menor do que algum dos R2k, então há indicativo \n";
            a += "de problemas com multicolinearidade."; 
 
            a += "\n\n";
            
            a += "OBS 3) A medida Ck = (1 - R2k)^(1/2) é sugerida na página 268, em  em W. H. Greene (1993), Econometric Analysis, Segunda  \n";
            a += "Edição, Prentice Hall. Ck corresponde à raiz quadrada da razão entre a variância de beta_k estimado com e sem as demais \n";
            a += "variáveis explicativas. Quando xk é não correlacionado com as demais variáveis independentes, Ck = 1."; 

            a += "\n\n";

            return out_text + a;
        }

        #endregion

        #region Matriz de vizinhança

        private bool m_estimacao_bem_sucedida = false;
        public bool EstimacaoBemSucedida
        {
            get
            {
                return m_estimacao_bem_sucedida;
            }
            set
            {
            	m_estimacao_bem_sucedida = value;
            }
        }

        public void GeraMatrizVizinhanca()
        {
            m_geomle.TipoMatrizVizinhanca = m_tipo_matriz_vizinhanca;

            if (!m_estimacao_bem_sucedida
                   || m_geomle.Shape == null
                   || m_geomle.Shape.Count <= 0
                   || m_geomle.Shape.HoraCriacao != this.Shape.HoraCriacao
                   || m_tipo_vizinhanca_anterior != m_tipo_matriz_vizinhanca
                   || m_tipo_calculo_logdet_anterior != m_tipo_logdet
                   || m_geomle.Shape.TipoVizinhanca != this.Shape.TipoVizinhanca)
            {
                m_geomle.Shape = this.Shape;

                if (m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Normalizada)
                {
                    m_geomle.MatrizWesparsaFromVizinhosNorm();
                }

                if (m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Original)
                {
                    m_geomle.MatrizWesparsaFromVizinhos();
                }

                m_tipo_calculo_logdet_anterior = m_tipo_logdet;
                m_tipo_vizinhanca_anterior = m_tipo_matriz_vizinhanca;
            }
        }

        #endregion

        #region Vetor de distâncias até o n-ésimo vizinho

        public double[] GeraDistanciasAteVizinhoN(int num_vizinhos, double[,] x, double[,] y)
        {
            int n = x.GetLength(0);
            int m = num_vizinhos;
            if (m > n - 1) m = n - 1;
            double[] res = new double[n];
            ArrayList a_dist = new ArrayList();
            for (int i = 0; i < n; i++)
            {
                a_dist.Clear();
                for (int j = 0; j < n; j++)
                {
                    a_dist.Add(Math.Pow(x[i, 0] - x[j, 0], 2.0) + Math.Pow(y[i, 0] - y[j, 0], 2.0));
                }
                a_dist.Sort();
                res[i] = Math.Sqrt(Convert.ToDouble(a_dist[m]));
            }
            return res;
        }

        #endregion

        #region Estimação modelos SAR (via S2SLS de Kelejian e Prucha)

        public void EstimaModelosSAR_Kelejian_Prucha()
        {
            clsUtilTools clt = new clsUtilTools();

            if (this.m_variaveis_independentes.GetLength(0) > this.m_variaveis_instrumentais.GetLength(0))
            {
                this.m_variaveis_instrumentais = this.m_variaveis_independentes;
            }

            double[,] Y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisDependentes);   /* left-hand-side variable */

            m_nobs = Y.GetLength(0);

            double[,] X = new double[0, 0];         /* variáveis exógenas */
            double[,] X_endo = new double[0, 0];    /* variáveis endógenas no right-hand-side */
            double[,] H = new double[0,0];          /* variaveis instrumentais puras */

            string[] nomes_vars_exogenas = new string[0];
            string[] nomes_vars_endogenas = new string[0];
            string[] nomes_vars_instrumentais_puras = new string[0];

            this.variaveis_exogenas_e_endogenas(ref nomes_vars_exogenas, ref X, 
                                                ref nomes_vars_endogenas, ref X_endo, 
                                                ref nomes_vars_instrumentais_puras, ref H);

            clsFuncoesMatrizEsparsa fme = new clsFuncoesMatrizEsparsa();
            double[,] WY = fme.MatrizMult(this.m_geomle.Wesparsa, Y);
            double[,] WX = fme.MatrizMult(this.m_geomle.Wesparsa, X);

            int n_variaveis_regressao = nomes_vars_exogenas.GetLength(0) + nomes_vars_endogenas.GetLength(0) + 1;
            string[] variaveis_regressao = new string[n_variaveis_regressao];
            int indice = 0;
            for (int i = 0; i < nomes_vars_exogenas.GetLength(0); i++)
            {
                variaveis_regressao[i] = nomes_vars_exogenas[i];
                indice++;
            }
            for (int i = 0; i < nomes_vars_endogenas.GetLength(0); i++)
            {
                variaveis_regressao[i+indice] = nomes_vars_endogenas[i];
            }
            variaveis_regressao[variaveis_regressao.GetLength(0) - 1] = "Rho";

            if (this.m_usa_intercepto)
            {
                X = clt.Concateh(clt.Ones(X.GetLength(0), 1), X);
            }

            double[,] Z = clt.Concateh(clt.Concateh(X, X_endo), WY);
            double[,] Q = clt.Concateh(clt.Concateh(X, H), WX);

            //-------- checando singularidade da matriz X ------------//
            double[,] invX = clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(Q), Q));

            double[,] m1 = clt.MatrizMult(clt.MatrizTransp(Z), Q);
            double[,] m2 = clt.MatrizMult(clt.MatrizTransp(Q), Q);
            double[,] m3 = clt.MatrizInversa(m2);
            double[,] m4 = clt.MatrizMult(clt.MatrizMult(m1, m3), clt.MatrizTransp(Q));
            double[,] m5 = clt.MatrizInversa(clt.MatrizMult(m4, Z));
            double[,] m6 = clt.MatrizMult(m4, Y);

            double[,] beta_hat = clt.MatrizMult(m5, m6);

            double[,] Y_hat = clt.MatrizMult(Z, beta_hat);
            double[,] erro = clt.MatrizSubtracao(Y, Y_hat);
            double sigma2_hat = clt.MatrizMult(clt.MatrizTransp(erro), erro)[0, 0] / ((double)m_nobs);
            double[,] cov_matriz = new double[0, 0];

            double[,] cov_matriz_homocedastic = new double[0, 0];           /*----- assumindo erros homocedasticos e não espacialmente auto-correlacionados    -----*/
            double[,] cov_matriz_heteroscedastic = new double[0, 0];        /*----- assumindo erros heteroscedasticos e não espacialmente auto-correlacionados -----*/
            double[,] cov_matriz_HAC = new double[0, 0];                    /*----- assumindo erros heteroscedasticos e espacialmente correlacionados          -----*/

            switch (m_tipo_correcao_cov_matrix)
            {
                case TipoCorrecaoMatrizCovariancia.SemCorrecao:
                    {
                        cov_matriz_homocedastic = clt.MatrizMult(m5, sigma2_hat);
                        cov_matriz = clt.ArrayDoubleClone(cov_matriz_homocedastic);
                        break;
                    }
                case TipoCorrecaoMatrizCovariancia.Heteroscedasticidade:
                    {
                        double[,] QS = new double[Q.GetLength(0), Q.GetLength(1)];
                        for (int i = 0; i < QS.GetLength(0); i++)
                        {
                            for (int j = 0; j < QS.GetLength(1); j++)
                            {
                                QS[i, j] = Q[i, j] * Math.Abs(erro[i, 0]);
                            }
                        }
                        double[,] m7 = clt.MatrizMult(clt.MatrizTransp(QS), QS);
                        double[,] m8 = clt.MatrizInversa(m7);
                        double[,] m9 = clt.MatrizMult(clt.MatrizMult(m1, m8), clt.MatrizTransp(m1));
                        cov_matriz_heteroscedastic = clt.MatrizInversa(m9);
                        cov_matriz = clt.ArrayDoubleClone(cov_matriz_heteroscedastic);
                        break;
                    }
                case TipoCorrecaoMatrizCovariancia.HAC:
                    double[,] coord_x = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, this.m_variavel_coord_X);
                    double[,] coord_y = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, this.m_variavel_coord_Y);
                    double[] distancias_bandwidth = new double[coord_x.GetLength(0)];
                    if (!m_usa_num_vizinhos_para_distancia)
                    {
                        double bandwidth = this.BandWidthKernelHAC;
                        for (int i = 0; i < m_nobs; i++)
                        {
                            distancias_bandwidth[i] = bandwidth;
                        }
                    }
                    else
                    {
                        distancias_bandwidth = this.GeraDistanciasAteVizinhoN(this.m_num_vizinhos_para_distancia, coord_x, coord_y);
                    }
                    double[,] Phi = this.Phi_correcao_HAC(Q, erro, coord_x, coord_y, distancias_bandwidth);
                    double[,] m10 = clt.MatrizMult(m1, m3);
                    double[,] m11 = clt.MatrizMult(m5, m10);
                    cov_matriz_HAC = clt.MatrizMult(clt.MatrizMult(m11, Phi), clt.MatrizTransp(m11));
                    cov_matriz = clt.ArrayDoubleClone(cov_matriz_HAC);
                    cov_matriz = clt.MatrizMult(cov_matriz, ((double)m_nobs));
                    break;
                default:
                    break;
            }

            #region outras estatísticas

            this.m_beta_hat = beta_hat;
            this.m_beta_hat_cov = cov_matriz;

            this.TipoModeloRegressaoEspacial = TipoModeloEspacial.nao_espacial;     /*--- opção apenas para não calcular agora valores preditos e resíduos ---*/
            this.GeraSignificanciaCoeficientes();

            #endregion

            #region saída dos resultados

            string correcao_cov_matrix = "sem correção";
            if (this.TipoCorrecaoCovMatrix == TipoCorrecaoMatrizCovariancia.Heteroscedasticidade) correcao_cov_matrix = "heteroscedasticidade";
            if (this.TipoCorrecaoCovMatrix == TipoCorrecaoMatrizCovariancia.HAC) correcao_cov_matrix = "Heteroscedasticidade de autocorrelação espacial";

            string funcao_kernel = "Barlett (triangular)";
            if (this.TipoKernelCorrecaoHAC == TipoKernelCorrecaoHAC.Biquadrado) funcao_kernel = "Biquadrado";
            if (this.TipoKernelCorrecaoHAC == TipoKernelCorrecaoHAC.Epanechnikov) funcao_kernel = "Epanechnikov";

            double fstat = 0.0;
            double pvalue_f = 0.0;
            string[] nomes = new string[3];
            double[] vetorsomadequadrados = new double[3];
            double[] vetorgl = new double[3];
            string[] vetorerroPAD = new string[3];
            string[] vetorF = new string[3];
            string[] vetorpvalue = new string[3];
                       
            testF(Y, Z, m_beta_hat, ref fstat, ref pvalue_f, ref nomes, ref vetorsomadequadrados, ref vetorgl, ref vetorerroPAD, ref vetorF, ref vetorpvalue);
            
            string out_text = "============================================================================================================================\n\n";

            out_text += "Modelos SAR estimados via 2SLS (Kelejian e Prucha)\n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + Y.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + beta_hat.GetLength(0) + "\n\n";

            string origem_dados = "arquivo shape";
            if (m_tipo_origem_matriz_vizinhanca == TipoOrigemMatrizVizinhanca.MatrizFromArquivo) origem_dados = "importado do arquivo";
            if (m_tipo_origem_matriz_vizinhanca == TipoOrigemMatrizVizinhanca.MatrizFromDistancias) origem_dados = "coordenadas do centróide";
            if (m_tipo_origem_matriz_vizinhanca == RegressoesEspaciais.TipoOrigemMatrizVizinhanca.MatrizPreDefinida) origem_dados = "carregado do formulário de mapas";
            out_text += "Origem da matriz de vizinhança: " + origem_dados + "\n";

            if (m_tipo_origem_matriz_vizinhanca == TipoOrigemMatrizVizinhanca.ArquivoShape)
            {
                out_text += "Tipo de contiguidade: " + this.Shape.TipoVizinhanca + "\n";
            }

            if (m_tipo_origem_matriz_vizinhanca == RegressoesEspaciais.TipoOrigemMatrizVizinhanca.MatrizPreDefinida)
            {
                out_text += "Tipo de vizinhança (matriz importada do formulário de mapas): " + this.m_tipo_matriz_vizinhanca_predefinida + "\n";
            }
            else
            {
                if (m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Normalizada)
                {
                    out_text += "Matriz de vizinhança normalizada pela soma das linhas \n";
                }
            }

            out_text += "\n";

            out_text += "Correção para a matriz de covariância: " + correcao_cov_matrix + "\n";
            if (this.m_tipo_correcao_cov_matrix == TipoCorrecaoMatrizCovariancia.HAC)
            {
                out_text += "Coordenada x: " + this.m_variavel_coord_X + "\n";
                out_text += "Coordenada y: " + this.m_variavel_coord_Y + "\n";
                out_text += "Função kernel: " + funcao_kernel + "\n";
                if (!m_usa_num_vizinhos_para_distancia)
                {
                    out_text += "Distância para a função kernel: " + clt.Double2Texto(BandWidthKernelHAC, 6) + "\n";
                }
                else
                {
                    out_text += "Distância para a função kernel: " + "variável" + "\n";
                    out_text += "Número de vizinhos considerado para distância variável: " + m_num_vizinhos_para_distancia.ToString() + "\n";
                }
            }

            out_text += "\n";
 
            out_text += GeraTabelaEstimacoes(variaveis_regressao, this.m_beta_hat, this.m_beta_stderror,
                                             this.m_beta_tstat, this.m_beta_pvalor, this.m_usa_intercepto);

            out_text += "\n\n" + GeraANOVA(nomes, vetorsomadequadrados, vetorgl, vetorerroPAD, vetorF, vetorpvalue);  

            
            out_text += "\n";
            out_text += "Variáveis instrumentais utilizadas na estimação: \n\n";
            if (this.m_usa_intercepto) out_text += "Constante" + "\n";
            for (int i = 0; i < VariaveisInstrumentais.GetLength(0); i++) out_text += VariaveisInstrumentais[i] + "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(this.m_beta_hat_cov, variaveis_regressao, m_usa_intercepto);
            }

            out_text += "\n";
            out_text += m_geomle.Wesparsa.EstatisticasDescritivas();

            this.m_output_text = out_text;

            #endregion
            
            #region novas variáveis na base de dados

            double[,] WErro = fme.MatrizMult(this.m_geomle.Wesparsa, erro);
            double[,] erro_stand = clt.Standardizec(erro);

            double[,] variaveis_geradas = new double[m_nobs, 6];
            for (int i = 0; i < m_nobs; i++)
            {
                variaveis_geradas[i, 0] = (double)(i + 1);
                variaveis_geradas[i, 1] = Y[i, 0];
                variaveis_geradas[i, 2] = Y_hat[i, 0];
                variaveis_geradas[i, 3] = erro[i, 0];
                variaveis_geradas[i, 4] = erro_stand[i, 0];
                variaveis_geradas[i, 5] = WErro[i, 0];
            }

            string[] nomes_variaveis = new string[6];
            nomes_variaveis[0] = "Observacao_";
            nomes_variaveis[1] = "Y_observado_";
            nomes_variaveis[2] = "Y_predito_";
            nomes_variaveis[3] = "Residuo_";
            nomes_variaveis[4] = "Residuo_padronizado_";
            nomes_variaveis[5] = "SpatialW_residuo_";

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação via S2SLS (Kelejian e Prucha)\n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);

            AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);

            #endregion            
        }

        #endregion
        
        #region Estimação modelos SAC (via MLE)

        public void EstimaModelosSAC()
        {
            m_estimacao_bem_sucedida = false;

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

            double fstat = 0.0;
            double pvalue_f = 0.0;
            string[] nomes = new string[3];
            double[] vetorsomadequadrados = new double[3];
            double[] vetorgl = new double[3];
            string[] vetorerroPAD = new string[3];
            string[] vetorF = new string[3];
            string[] vetorpvalue = new string[3];

            m_geomle.EstimateModeloSAC();
            testF(Y, X, m_geomle.BetaHat, ref fstat, ref pvalue_f, ref nomes, ref vetorsomadequadrados, ref vetorgl, ref vetorerroPAD, ref vetorF, ref vetorpvalue);

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação do Modelo SAC via Máxima Verossimilhança \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Variância dos erros: " + clt.Double2Texto(m_geomle.Sigma2Hat, 6) + "\n\n";

            string origem_dados = "arquivo shape";
            if (m_tipo_origem_matriz_vizinhanca == TipoOrigemMatrizVizinhanca.MatrizFromArquivo) origem_dados = "importado do arquivo";
            if (m_tipo_origem_matriz_vizinhanca == TipoOrigemMatrizVizinhanca.MatrizFromDistancias) origem_dados = "coordenadas do centróide";
            if (m_tipo_origem_matriz_vizinhanca == RegressoesEspaciais.TipoOrigemMatrizVizinhanca.MatrizPreDefinida) origem_dados = "carregado do formulário de mapas";
            out_text += "Origem da matriz de vizinhança: " + origem_dados + "\n";

            if (m_tipo_origem_matriz_vizinhanca == TipoOrigemMatrizVizinhanca.ArquivoShape)
            {
                out_text += "Tipo de contiguidade: " + this.Shape.TipoVizinhanca + "\n";
            }

            if (m_tipo_origem_matriz_vizinhanca == RegressoesEspaciais.TipoOrigemMatrizVizinhanca.MatrizPreDefinida)
            {
                out_text += "Tipo de vizinhança (matriz importada do formulário de mapas): " + this.m_tipo_matriz_vizinhanca_predefinida + "\n";
            }
            else
            {
                if (m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Normalizada)
                {
                    out_text += "Matriz de vizinhança normalizada pela soma das linhas \n";
                }
            }

            string tipo_log_determinante = "matriz densa";
            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.MatrizEsparsaDecomposicaoLU) tipo_log_determinante = "decomposição LU para matriz esparsa";
            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo) tipo_log_determinante = "simulações de Monte Carlo";
            out_text += "Cálculo do log-determinante: " + tipo_log_determinante;
            out_text += "\n\n";

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

            out_text += "Lambda (coeficiente): " + clt.Double2Texto(m_geomle.LambdaHat, 6) + "\n";
            out_text += "Lambda (erro padrão): " + clt.Double2Texto(m_geomle.LambdaStdError, 6) + "\n";
            out_text += "Lambda (estatística t): " + clt.Double2Texto(m_geomle.LambdaTStat, 6) + "\n";
            out_text += "Lambda (p-valor): " + clt.Double2Texto(m_geomle.LambdaPValue, 6) + "\n";
            out_text += "Lambda (lim. inf. 95%): " + clt.Double2Texto(m_geomle.LambdaLimInfCI, 6) + "\n";
            out_text += "Lambda (lim. sup. 95%): " + clt.Double2Texto(m_geomle.LambdaLimSupCI, 6) + "\n\n";

            out_text += "Teste da razão de verossimilhança =========> H0: rho = 0" + "\n";
            out_text += "Sob H0: rho = 0.0 e lambda = " + clt.Double2Texto(m_geomle.LambdaComRhoNulo, 6) + "\n";
            out_text += "Estatística teste: " + clt.Double2Texto(m_geomle.LikelihoodRatioTestStatRho, 6) + "\n";
            out_text += "P-valor: " + clt.Double2Texto(m_geomle.LikelihoodRatioTestPvalueRho, 6) + "\n\n";

            out_text += "Teste da razão de verossimilhança =========> H0: lambda = 0" + "\n";
            out_text += "Sob H0: rho = " + clt.Double2Texto(m_geomle.RhoComLambdaNulo, 6) + " e lambda = 0.0" + "\n";
            out_text += "Estatística teste: " + clt.Double2Texto(m_geomle.LikelihoodRatioTestStatLambda, 6) + "\n";
            out_text += "P-valor: " + clt.Double2Texto(m_geomle.LikelihoodRatioTestPvalueLambda, 6) + "\n\n";

            out_text += "Teste da razão de verossimilhança =========> H0: rho = 0 e lambda = 0" + "\n";
            out_text += "Estatística teste: " + clt.Double2Texto(m_geomle.LikelihoodRatioTestStatRhoLambda, 6) + "\n";
            out_text += "P-valor: " + clt.Double2Texto(m_geomle.LikelihoodRatioTestPvalueRhoLambda, 6) + "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(m_geomle.BetaHatCovMatrix, VariaveisIndependentes, m_usa_intercepto);
            }

            out_text += "\n\n" + GeraANOVA(nomes, vetorsomadequadrados, vetorgl, vetorerroPAD, vetorF, vetorpvalue);  

            out_text += "\n";
            out_text += m_geomle.Wesparsa.EstatisticasDescritivas();

            this.m_output_text = out_text;

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação do Modelo SAR via Máxima Verossimilhança \n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(m_geomle.VariaveisGeradas, m_geomle.NomesVariaveisGeradas);

            AdicionaNovasVariaveisToDataTable(m_geomle.VariaveisGeradas, m_geomle.NomesVariaveisGeradas);
        }

        #endregion

        #region Estimação modelos SAR (via MLE)

        public void EstimaModelosSAR()
        {
            m_estimacao_bem_sucedida = false;

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

            double fstat = 0.0;
            double pvalue_f = 0.0;
            string[] nomes = new string[3];
            double[] vetorsomadequadrados = new double[3];
            double[] vetorgl = new double[3];
            string[] vetorerroPAD = new string[3];
            string[] vetorF = new string[3];
            string[] vetorpvalue = new string[3];

            m_geomle.EstimateModeloSAR();
            testF(Y, X, m_geomle.BetaHat, ref fstat, ref pvalue_f, ref nomes, ref vetorsomadequadrados, ref vetorgl, ref vetorerroPAD, ref vetorF, ref vetorpvalue);

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação do Modelo SAR via Máxima Verossimilhança \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Variância dos erros: " + clt.Double2Texto(m_geomle.Sigma2Hat, 6) + "\n\n";

            string origem_dados = "arquivo shape";
            if (m_tipo_origem_matriz_vizinhanca == TipoOrigemMatrizVizinhanca.MatrizFromArquivo) origem_dados = "importado do arquivo";
            if (m_tipo_origem_matriz_vizinhanca == TipoOrigemMatrizVizinhanca.MatrizFromDistancias) origem_dados = "coordenadas do centróide";
            if (m_tipo_origem_matriz_vizinhanca == RegressoesEspaciais.TipoOrigemMatrizVizinhanca.MatrizPreDefinida) origem_dados = "carregado do formulário de mapas";
            out_text += "Origem da matriz de vizinhança: " + origem_dados + "\n";

            if (m_tipo_origem_matriz_vizinhanca == TipoOrigemMatrizVizinhanca.ArquivoShape)
            {
                out_text += "Tipo de contiguidade: " + this.Shape.TipoVizinhanca + "\n";
            }

            if (m_tipo_origem_matriz_vizinhanca == RegressoesEspaciais.TipoOrigemMatrizVizinhanca.MatrizPreDefinida)
            {
                out_text += "Tipo de vizinhança (matriz importada do formulário de mapas): " + this.m_tipo_matriz_vizinhanca_predefinida + "\n";
            }
            else
            {
                if (m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Normalizada)
                {
                    out_text += "Matriz de vizinhança normalizada pela soma das linhas \n";
                }
            }

            string tipo_log_determinante = "matriz densa";
            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.MatrizEsparsaDecomposicaoLU) tipo_log_determinante = "decomposição LU para matriz esparsa";
            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo) tipo_log_determinante = "simulações de Monte Carlo";
            out_text += "Cálculo do log-determinante: " + tipo_log_determinante;
            out_text += "\n\n";

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

            out_text += "\n\n" + GeraANOVA(nomes, vetorsomadequadrados, vetorgl, vetorerroPAD, vetorF, vetorpvalue);  

            out_text += "\n";
            out_text += m_geomle.Wesparsa.EstatisticasDescritivas();

            this.m_output_text = out_text;

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação do Modelo SAR via Máxima Verossimilhança \n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(m_geomle.VariaveisGeradas, m_geomle.NomesVariaveisGeradas);

            AdicionaNovasVariaveisToDataTable(m_geomle.VariaveisGeradas, m_geomle.NomesVariaveisGeradas);
        }

        #endregion

        #region Estimação modelos SEM (via MLE)

        public void EstimaModelosSEM()
        {
            m_estimacao_bem_sucedida = false;

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

            double fstat = 0.0;
            double pvalue_f = 0.0;
            string[] nomes = new string[3];
            double[] vetorsomadequadrados = new double[3];
            double[] vetorgl = new double[3];
            string[] vetorerroPAD = new string[3];
            string[] vetorF = new string[3];
            string[] vetorpvalue = new string[3];

            m_geomle.EstimateModeloSEM();

            testF(Y, X, m_geomle.BetaHat, ref fstat, ref pvalue_f, ref nomes, ref vetorsomadequadrados, ref vetorgl, ref vetorerroPAD, ref vetorF, ref vetorpvalue);
            
            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação do Modelo SEM via Máxima Verossimilhança \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Variância dos erros: " + clt.Double2Texto(m_geomle.Sigma2Hat, 6) + "\n\n";

            string origem_dados = "arquivo shape";
            if (m_tipo_origem_matriz_vizinhanca == TipoOrigemMatrizVizinhanca.MatrizFromArquivo) origem_dados = "importado do arquivo";
            if (m_tipo_origem_matriz_vizinhanca == RegressoesEspaciais.TipoOrigemMatrizVizinhanca.MatrizPreDefinida) origem_dados = "carregado do formulário de mapas";
            if (m_tipo_origem_matriz_vizinhanca == TipoOrigemMatrizVizinhanca.MatrizFromDistancias) origem_dados = "coordenadas do centróide";
            out_text += "Origem da matriz de vizinhança: " + origem_dados + "\n";

            if (m_tipo_origem_matriz_vizinhanca == TipoOrigemMatrizVizinhanca.ArquivoShape)
            {
                out_text += "Tipo de contiguidade: " + this.Shape.TipoVizinhanca + "\n";
            }

            if (m_tipo_origem_matriz_vizinhanca == RegressoesEspaciais.TipoOrigemMatrizVizinhanca.MatrizPreDefinida)
            {
                out_text += "Tipo de vizinhança (matriz importada do formulário de mapas): " + this.m_tipo_matriz_vizinhanca_predefinida + "\n";
            }
            else
            {
                if (m_tipo_matriz_vizinhanca == TipoMatrizVizinhanca.Normalizada)
                {
                    out_text += "Matriz de vizinhança normalizada pela soma das linhas \n";
                }
            }

            string tipo_log_determinante = "matriz densa";
            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.MatrizEsparsaDecomposicaoLU) tipo_log_determinante = "decomposição LU para matriz esparsa";
            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo) tipo_log_determinante = "simulações de Monte Carlo";
            out_text += "Cálculo do log-determinante: " + tipo_log_determinante;
            out_text += "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, m_geomle.BetaHat, 
                m_geomle.BetaStdError, m_geomle.BetaTStat, m_geomle.BetaPValue, m_usa_intercepto);
            
            out_text += "\nLog-likelihood: " + clt.Double2Texto(m_geomle.LogLik, 6) + "\n";
            out_text += "AIC: " + clt.Double2Texto(m_geomle.AIC, 6) + "\n";
            out_text += "BIC: " + clt.Double2Texto(m_geomle.BIC, 6) + "\n\n";

            out_text += "Lambda (coeficiente): " + clt.Double2Texto(m_geomle.RhoHat, 6) + "\n";
            out_text += "Lambda (erro padrão): " + clt.Double2Texto(m_geomle.RhoStdError, 6) + "\n";
            out_text += "Lambda (estatística t): " + clt.Double2Texto(m_geomle.RhoTStat, 6) + "\n";
            out_text += "Lambda (p-valor): " + clt.Double2Texto(m_geomle.RhoPValue, 6) + "\n";
            out_text += "Lambda (lim. inf. 95%): " + clt.Double2Texto(m_geomle.RhoLimInfCI, 6) + "\n";
            out_text += "Lambda (lim. sup. 95%): " + clt.Double2Texto(m_geomle.RhoLimSupCI, 6) + "\n\n";

            out_text += "Teste da razão de verossimilhança (estatística teste): " + clt.Double2Texto(m_geomle.LikelihoodRatioTestStatRho, 6) + "\n";
            out_text += "Teste da razão de verossimilhança (p-valor): " + clt.Double2Texto(m_geomle.LikelihoodRatioTestPvalueRho, 6) + "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(m_geomle.BetaHatCovMatrix, VariaveisIndependentes, m_usa_intercepto);
            }

            out_text += "\n\n" + GeraANOVA(nomes, vetorsomadequadrados, vetorgl, vetorerroPAD, vetorF, vetorpvalue);  

            out_text += "\n";
            out_text += m_geomle.Wesparsa.EstatisticasDescritivas();

            this.m_output_text = out_text;

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação do Modelo SEM via Máxima Verossimilhança \n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(m_geomle.VariaveisGeradas, m_geomle.NomesVariaveisGeradas);

            AdicionaNovasVariaveisToDataTable(m_geomle.VariaveisGeradas, m_geomle.NomesVariaveisGeradas);
        }

        #endregion

        #region Estimação modelos SEM (via OLS)

        public void EstimaModelosSEM_via_OLS()
        {
            m_estimacao_bem_sucedida = false;

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
            double[,] invXX = clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(X), X));

            m_geomle.X = X;
            m_geomle.Y = Y;
            m_geomle.TipoCalculoLogDetW = this.m_tipo_logdet;
            m_geomle.TipoModeloRegressaoEspacial = TipoModeloEspacial.SEM;

            m_geomle.EstimateModeloSEM_via_OLS();

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação do Modelo SEM via OLS (com Correção da Matriz de Covariância) \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Variância dos erros: " + clt.Double2Texto(m_geomle.Sigma2Hat, 6) + "\n";

            string tipo_log_determinante = "matriz densa";
            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.MatrizEsparsaDecomposicaoLU) tipo_log_determinante = "decomposição LU para matriz esparsa";
            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo) tipo_log_determinante = "simulações de Monte Carlo";
            out_text += "Cálculo do log-determinante: " + tipo_log_determinante;
            out_text += "\n\n";

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
            out_text += "Rho (lim. sup. 95%): " + clt.Double2Texto(m_geomle.RhoLimSupCI, 6) + "\n";

            //out_text += "Teste da razão de verossimilhança (estatística teste): " + clt.Double2Texto(m_geomle.LikelihoodRatioTestStatRho, 6) + "\n";
            //out_text += "Teste da razão de verossimilhança (p-valor): " + clt.Double2Texto(m_geomle.LikelihoodRatioTestPvalueRho, 6) + "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(m_geomle.BetaHatCovMatrix, VariaveisIndependentes, m_usa_intercepto);
            }

            out_text += "\n";
            out_text += m_geomle.Wesparsa.EstatisticasDescritivas();

            this.m_output_text = out_text;

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação do Modelo SEM via OLS (com Correção da Matriz de Covariância) \n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(m_geomle.VariaveisGeradas, m_geomle.NomesVariaveisGeradas);

            AdicionaNovasVariaveisToDataTable(m_geomle.VariaveisGeradas, m_geomle.NomesVariaveisGeradas);
        }

        #endregion

        #region Estimação modelos SEM (via FGLS)

        public void EstimaModelosSEM_via_FGLS()
        {
            m_estimacao_bem_sucedida = false;

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
            double[,] invXX = clt.MatrizInversa(clt.MatrizMult(clt.MatrizTransp(X), X));

            m_geomle.X = X;
            m_geomle.Y = Y;
            m_geomle.TipoCalculoLogDetW = this.m_tipo_logdet;
            m_geomle.TipoModeloRegressaoEspacial = TipoModeloEspacial.SEM;
            m_geomle.IterateTillConvergence = this.m_iterate_till_convergence;

            double fstat = 0.0;
            double pvalue_f = 0.0;
            string[] nomes = new string[3];
            double[] vetorsomadequadrados = new double[3];
            double[] vetorgl = new double[3];
            string[] vetorerroPAD = new string[3];
            string[] vetorF = new string[3];
            string[] vetorpvalue = new string[3];

            m_geomle.EstimateModeloSEM_via_GLS();

            testF(Y, X, m_geomle.BetaHat, ref fstat, ref pvalue_f, ref nomes, ref vetorsomadequadrados, ref vetorgl, ref vetorerroPAD, ref vetorF, ref vetorpvalue);

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação do Modelo SEM via FGLS \n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            if (this.m_iterate_till_convergence)
            {
                out_text += m_geomle.MensagemIterationsTillConvergence + "\n";
                out_text += "Número de iterações até convergência: " + m_geomle.NumIterations + "\n";
            }
            else
            {
                out_text += "Estimações sem processo iterativo \n";
            }
            out_text += "Variância dos erros: " + clt.Double2Texto(m_geomle.Sigma2Hat, 6) + "\n";

            string tipo_log_determinante = "matriz densa";
            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.MatrizEsparsaDecomposicaoLU) tipo_log_determinante = "decomposição LU para matriz esparsa";
            if (this.m_tipo_logdet == TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo) tipo_log_determinante = "simulações de Monte Carlo";
            out_text += "Cálculo do log-determinante: " + tipo_log_determinante;
            out_text += "\n\n";

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
            out_text += "Rho (lim. sup. 95%): " + clt.Double2Texto(m_geomle.RhoLimSupCI, 6) + "\n";

            //out_text += "Teste da razão de verossimilhança (estatística teste): " + clt.Double2Texto(m_geomle.LikelihoodRatioTestStatRho, 6) + "\n";
            //out_text += "Teste da razão de verossimilhança (p-valor): " + clt.Double2Texto(m_geomle.LikelihoodRatioTestPvalueRho, 6) + "\n";

            if (this.m_apresenta_covmatrix_beta_hat)
            {
                out_text += "\n";
                out_text += "Matriz de variância-covariância do estimador do vetor de coeficientes: \n\n";

                out_text += this.GeraTabelaCovMatrix(m_geomle.BetaHatCovMatrix, VariaveisIndependentes, m_usa_intercepto);
            }

            out_text += "\n";
            out_text += m_geomle.Wesparsa.EstatisticasDescritivas();

            out_text += "\n\n" + GeraANOVA(nomes, vetorsomadequadrados, vetorgl, vetorerroPAD, vetorF, vetorpvalue);  
            
            this.m_output_text = out_text;

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação do Modelo SEM via FGLS \n\n";
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
            double fstat = 0.0;
            double pvalue_f = 0.0;
            string[] nomes = new string[3];
            double[] vetorsomadequadrados = new double[3];
            double[] vetorgl = new double[3];
            string[] vetorerroPAD = new string[3];
            string[] vetorF = new string[3];
            string[] vetorpvalue = new string[3];

            int num_iterations = 0;
            string mensagem_iterations = "";

            gmm.Limited_info_spatial_GMM_estimation(Y, X, Z, coord_x, coord_y, 
                this.m_cutoff_X, this.m_cutoff_Y, ref beta, ref covbeta, ref stderror, ref tstat, ref pvalue,
                ref Jstat, ref Jpvalue, ref sigma2,
                this.m_iterate_till_convergence, this.m_tol_iterate_till_convergence, this.m_max_iterations_till_convergence,
                ref num_iterations, ref mensagem_iterations);

            testF(Y, X, beta, ref fstat, ref pvalue_f, ref nomes, ref vetorsomadequadrados, ref vetorgl, ref vetorerroPAD, ref vetorF, ref vetorpvalue);

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação via GMM Espacial (Conley, 1999)\n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            if (this.m_iterate_till_convergence)
            {
                out_text += mensagem_iterations + "\n";
                out_text += "Número de iterações até convergência: " + num_iterations + "\n";
            }
            else
            {
                out_text += "Estimações sem processo iterativo \n";
            }
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Variância dos erros: " + clt.Double2Texto(sigma2, 6) + "\n\n";

            out_text += "Variável de coordenada x: " + m_variavel_coord_X + "\n";
            out_text += "Variável de coordenada y: " + m_variavel_coord_Y + "\n";
            out_text += "Cut-off coordenada x: " + clt.Double2Texto(m_cutoff_X, 6) + "\n";
            out_text += "Cut-off coordenada y: " + clt.Double2Texto(m_cutoff_Y, 6) + "\n\n";

            out_text += "Estatística J: " + clt.Double2Texto(Jstat, 6) + "\n";
            out_text += "P-valor J: " + clt.Double2Texto(Jpvalue, 6) + "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, beta, stderror, tstat, pvalue, m_usa_intercepto);

            out_text += "\n\n" + GeraANOVA(nomes, vetorsomadequadrados, vetorgl, vetorerroPAD, vetorF, vetorpvalue);  

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
            nomes_variaveis[0] = "Observacao_";
            nomes_variaveis[1] = "Y_observado_";
            nomes_variaveis[2] = "Y_predito_";
            nomes_variaveis[3] = "Residuo_";
            nomes_variaveis[4] = "Residuo_padronizado_";

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
            double fstat = 0.0;
            double pvalue_f = 0.0;
            string[] nomes = new string[3];
            double[] vetorsomadequadrados = new double[3];
            double[] vetorgl = new double[3];
            string[] vetorerroPAD = new string[3];
            string[] vetorF = new string[3];
            string[] vetorpvalue = new string[3];


            int num_iterations = 0;
            string mensagem_iterations = "";

            gmm.Limited_info_GMM_estimation(Y, X, Z, ref beta, ref covbeta, ref stderror, ref tstat, ref pvalue,
                ref Jstat, ref Jpvalue, ref sigma2, 
                this.m_iterate_till_convergence, this.m_tol_iterate_till_convergence, this.m_max_iterations_till_convergence, 
                ref num_iterations, ref mensagem_iterations);
            testF(Y, X, beta, ref fstat, ref pvalue_f, ref nomes, ref vetorsomadequadrados, ref vetorgl, ref vetorerroPAD, ref vetorF, ref vetorpvalue);

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação via GMM\n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            if (this.m_iterate_till_convergence)
            {
                out_text += mensagem_iterations + "\n";
                out_text += "Número de iterações até convergência: " + num_iterations + "\n";
            }
            else
            {
                out_text += "Estimações sem processo iterativo \n";
            }
            out_text += "Variância dos erros: " + clt.Double2Texto(sigma2, 6) + "\n\n";
            out_text += "Estatística J: " + clt.Double2Texto(Jstat, 6) + "\n";
            out_text += "P-valor J: " + clt.Double2Texto(Jpvalue, 6) + "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, beta, stderror, tstat, pvalue, m_usa_intercepto);

            out_text += "\n\n" + GeraANOVA(nomes, vetorsomadequadrados, vetorgl, vetorerroPAD, vetorF, vetorpvalue);

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
            nomes_variaveis[0] = "Observacao_";
            nomes_variaveis[1] = "Y_observado_";
            nomes_variaveis[2] = "Y_predito_";
            nomes_variaveis[3] = "Residuo_";
            nomes_variaveis[4] = "Residuo_padronizado_";

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
            double fstat = 0.0;
            double pvalue_f = 0.0;
            string[] nomes = new string[3];
            double[] vetorsomadequadrados = new double[3];
            double[] vetorgl = new double[3];
            string[] vetorerroPAD = new string[3];
            string[] vetorF = new string[3];
            string[] vetorpvalue = new string[3];

            gmm.Simple_2SLS_estimation(Y, X, Z, ref beta, ref covbeta, ref stderror, ref tstat, ref pvalue, ref sigma2);
            testF(Y, X, beta, ref fstat, ref pvalue_f, ref nomes, ref vetorsomadequadrados, ref vetorgl, ref vetorerroPAD, ref vetorF, ref vetorpvalue);

            string out_text = "============================================================================================================================\n\n";

            out_text += "Estimação via 2SLS\n\n";
            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável dependente: " + VariaveisDependentes[0] + "\n";
            out_text += "Número de observações: " + X.GetLength(0) + "\n";
            out_text += "Número de coeficientes: " + X.GetLength(1) + "\n";
            out_text += "Variância dos erros: " + clt.Double2Texto(sigma2, 6) + "\n\n";

            out_text += GeraTabelaEstimacoes(VariaveisIndependentes, beta, stderror, tstat, pvalue, m_usa_intercepto);

            out_text += "\n\n" + GeraANOVA(nomes, vetorsomadequadrados, vetorgl, vetorerroPAD, vetorF, vetorpvalue);

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
            nomes_variaveis[0] = "Observacao_";
            nomes_variaveis[1] = "Y_observado_";
            nomes_variaveis[2] = "Y_predito_";
            nomes_variaveis[3] = "Residuo_";
            nomes_variaveis[4] = "Residuo_padronizado_";

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
            double fstat = 0.0;
            double pvalue_f = 0.0;
            string[] nomes = new string[3];
            double[] vetorsomadequadrados = new double[3];
            double[] vetorgl = new double[3];
            string[] vetorerroPAD = new string[3];
            string[] vetorF = new string[3];
            string[] vetorpvalue = new string[3];

            gmm.Simple_OLS_estimation(Y, X, ref beta, ref covbeta, ref stderror, ref tstat, ref pvalue, ref sigma2);
            testF(Y, X, beta, ref fstat, ref pvalue_f, ref nomes, ref vetorsomadequadrados, ref vetorgl, ref vetorerroPAD, ref vetorF, ref vetorpvalue);
            
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

            out_text += "\n\n" + GeraANOVA(nomes,vetorsomadequadrados,vetorgl,vetorerroPAD,vetorF,vetorpvalue) ;

            this.m_output_text = out_text;

            double[,] variaveis_geradas = gmm.VariaveisGeradas;

            string[] nomes_variaveis = new string[5];
            nomes_variaveis[0] = "Observacao_";
            nomes_variaveis[1] = "Y_observado_";
            nomes_variaveis[2] = "Y_predito_";
            nomes_variaveis[3] = "Residuo_";
            nomes_variaveis[4] = "Residuo_padronizado_";

            m_output_variaveis_geradas = "============================================================================================================================\n\n";

            m_output_variaveis_geradas += "Estimação via OLS\n\n";
            m_output_variaveis_geradas += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            m_output_variaveis_geradas += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            m_output_variaveis_geradas += GeraTabelaNovasVariaveis(variaveis_geradas, nomes_variaveis);

            AdicionaNovasVariaveisToDataTable(variaveis_geradas, nomes_variaveis);
        }

        #endregion

        #region teste F

        public void testF(double[,] y, double[,] X, double[,] m_beta_hat, ref double Fstatistic, ref double pval_Fstatistic, ref string[] nomes, 
                            ref double[] vetorsomadequadrados, ref double[] vetorgl, ref string[] vetorerroPAD, ref string[] vetorF, ref string[] vetorpvalue)
        {
            clsUtilTools clt = new clsUtilTools();
            double n = (double)X.GetLength(0);
            double k = (double)X.GetLength(1);


            double[,] y_hat = clt.MatrizMult(X, m_beta_hat);
            double[,] erro = clt.MatrizSubtracao(y, y_hat);
            double SQE = (clt.MatrizMult(clt.MatrizTransp(erro), erro))[0, 0];

            //talvez esteja dando erro na estimação desta variância;
            double sigma2_hat = SQE / (n - k);

            double ybarravalor = clt.Meanc(y)[0, 0];
            double[,] ybarra = new double[X.GetLength(0), 1];

            for (int i = 0; i < X.GetLength(0); i++)
            {
                ybarra[i, 0] = ybarravalor;
            }

            double[,] M = clt.MatrizSubtracao(y_hat, ybarra);

            double SQM = (clt.MatrizMult(clt.MatrizTransp(M), M))[0, 0];

            double SQT = SQE + SQM;

            int GLreg = X.GetLength(1) - 1;

            int GLerro = X.GetLength(0) - X.GetLength(1);

            int GLtotal = GLreg + GLerro;

            double EPreg = SQM / GLreg;

            double EPerro = SQE / GLerro;

            Fstatistic = EPreg / EPerro;

            MathFdist fdist = new MathFdist(X.GetLength(1) - 1, X.GetLength(0) - X.GetLength(1));
            pval_Fstatistic = 1.0 - fdist.cdf(Fstatistic);

            double Rsquare = SQM / SQT;

            double AdjustedRsquare = 1 - ((SQE * GLtotal) / (SQT * GLerro));

            nomes[0] = "Modelo";
            nomes[1] = "Erro"; 
            nomes[2] = "Total";

            vetorsomadequadrados[0] = SQM;
            vetorsomadequadrados[1] = SQE;
            vetorsomadequadrados[2] = SQT;

            vetorgl[0] = GLreg;
            vetorgl[1] = GLerro;
            vetorgl[2] = GLtotal;

            vetorerroPAD[0] = clt.Double2Texto(EPreg, 6);
            vetorerroPAD[1] = clt.Double2Texto(EPerro, 6);
            vetorerroPAD[2] = " ";

            vetorF[0] = clt.Double2Texto(Fstatistic, 6);
            vetorF[1] = "";
            vetorF[2] = "";

            string spval_Fstatistic = clt.Double2Texto(pval_Fstatistic, 6);

            vetorpvalue[0] = spval_Fstatistic;
            vetorpvalue[1] = "";
            vetorpvalue[2] = "";

            return;

        }

        #endregion

        #region geraAnova
        /// <summary>
        /// Tabulação da tabela ANOVA para regressão linear.
        /// </summary>
        /// <param name="nomes">nomes</param>
        /// <param name="somadequadrados">vetor com soma de quadrados</param>
        /// <param name="grausdeliberdades">vetor com graus de liberdade</param>
        /// <param name="erropadrao">vetor com erros padrões e resto vazio</param>
        /// <param name="fstat">vetor com valor da estatistica F e o resto vazio</param>
        /// <param name="pvalue"></param>
        /// <returns></returns>
        protected string GeraANOVA(string[] nomes, double[] somadequadrados, double[] grausdeliberdades, string[] erropadrao, string[] fstat, string[] pvalue)
        {
            clsUtilTools clt = new clsUtilTools();
            string[] variaveis = new string[somadequadrados.GetLength(0)];

            int max_length_variaveis = 0;
            int max_length_somadequadrados = 0;
            int max_length_grausdeliberdades = 0;
            int max_length_erropadrao = 0;
            int max_length_pvalue = 0;
            int max_length_fstat = 0;

            for (int j = 0; j < nomes.GetLength(0); j++)
            {
                variaveis[j] = nomes[j];
            }

            string[] st_somadequadrados = new string[somadequadrados.GetLength(0)];
            string[] st_grausdeliberdades = new string[somadequadrados.GetLength(0)];
            string[] st_erropadrao = new string[somadequadrados.GetLength(0)];
            string[] st_pvalue = new string[somadequadrados.GetLength(0)];
            string[] st_fstat = new string[somadequadrados.GetLength(0)];

            for (int i = 0; i < somadequadrados.GetLength(0); i++)
            {
                st_somadequadrados[i] = clt.Double2Texto(somadequadrados[i], 6);
                if (st_somadequadrados[i].Length > max_length_somadequadrados) max_length_somadequadrados = st_somadequadrados[i].Length;

                st_grausdeliberdades[i] = clt.Double2Texto(grausdeliberdades[i], 6);
                if (st_grausdeliberdades[i].Length > max_length_grausdeliberdades) max_length_grausdeliberdades = st_grausdeliberdades[i].Length;

                st_erropadrao[i] = clt.Double2Texto(erropadrao[i], 6);
                if (st_erropadrao[i].Length > max_length_erropadrao) max_length_erropadrao = st_erropadrao[i].Length;

                st_fstat[i] = clt.Double2Texto(fstat[i], 6);
                if (st_fstat[i].Length > max_length_fstat) max_length_fstat = st_fstat[i].Length;

                st_pvalue[i] = clt.Double2Texto(pvalue[i], 6);
                if (st_pvalue[i].Length > max_length_pvalue) max_length_pvalue = st_pvalue[i].Length;

                if (variaveis[i].Length > max_length_variaveis) max_length_variaveis = variaveis[i].Length;
            }

            if (" ".Length > max_length_variaveis) max_length_variaveis = " ".Length;
            if ("Soma de Quadrados".Length > max_length_somadequadrados) max_length_somadequadrados = "Soma de Quadrados".Length;
            if ("Graus de Liberdade".Length > max_length_grausdeliberdades) max_length_grausdeliberdades = "Graus de Liberdade".Length;
            if ("Erro Padrão".Length > max_length_erropadrao) max_length_erropadrao = "Erro Padrão".Length;
            if ("Estatística F".Length > max_length_fstat) max_length_fstat = "Estatística F".Length;
            if ("P-valor".Length > max_length_pvalue) max_length_pvalue = "P-valor".Length;

            max_length_variaveis += 10;
            max_length_somadequadrados += 6;
            max_length_pvalue += 6;
            max_length_grausdeliberdades += 6;
            max_length_erropadrao += 6;
            max_length_fstat += 6;

            string res = "";

            res += " " + PreencheEspacos(max_length_variaveis - " ".Length);
            res += PreencheEspacos(max_length_somadequadrados - "Soma de Quadrados".Length) + "Soma de Quadrados";
            res += PreencheEspacos(max_length_grausdeliberdades - "Graus de Liberdade".Length) + "Graus de Liberdade";
            res += PreencheEspacos(max_length_erropadrao - "Erro Padrão".Length) + "Erro Padrão";
            res += PreencheEspacos(max_length_fstat - "Estatística F".Length) + "Estatística F";
            res += PreencheEspacos(max_length_pvalue - "P-valor".Length) + "P-valor";
            res += "\n\n";

            for (int i = 0; i < somadequadrados.GetLength(0); i++)
            {
                res += variaveis[i] + PreencheEspacos(max_length_variaveis - variaveis[i].Length);
                res += PreencheEspacos(max_length_somadequadrados - st_somadequadrados[i].Length) + st_somadequadrados[i];
                res += PreencheEspacos(max_length_grausdeliberdades - st_grausdeliberdades[i].Length) + st_grausdeliberdades[i];
                res += PreencheEspacos(max_length_erropadrao - st_erropadrao[i].Length) + st_erropadrao[i];
                res += PreencheEspacos(max_length_fstat - st_fstat[i].Length) + st_fstat[i];
                res += PreencheEspacos(max_length_pvalue - st_pvalue[i].Length) + st_pvalue[i];
                res += "\n";
            }

            return res;
        }

        #endregion
    }

}
