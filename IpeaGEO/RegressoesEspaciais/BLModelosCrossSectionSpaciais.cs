using System;
using System.Collections;
using System.Data;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class BLModelosCrossSectionSpaciais : clsModelosRegressaoEspacial
    {        
        #region variáveis

        protected string m_tipo_matriz_vizinhanca_predefinida = "";
        public string TipoMatrizVizinhancaPredefinida
        {
            get { return  m_tipo_matriz_vizinhanca_predefinida; }
            set { m_tipo_matriz_vizinhanca_predefinida = value; }
        }

        private delegate double funcao_kernel(double d, double d_max);
        private funcao_kernel m_kernel;

        protected bool m_usa_num_vizinhos_para_distancia = false;
        public bool UsaNumVizinhosParaDistancia
        {
            get
            {
                return m_usa_num_vizinhos_para_distancia;
            }
            set
            {
                this.m_usa_num_vizinhos_para_distancia = value;
            }
        }

        protected int m_num_vizinhos_para_distancia = 10;
        public int NumVizinhosParaDistancia
        {
            get
            {
                return m_num_vizinhos_para_distancia;
            }
            set
            {
            	this.m_num_vizinhos_para_distancia = value;
            }
        }

        protected double m_bandkwidth_kernel_HAC = 0.0;
        public double BandWidthKernelHAC
        {
            get { return this.m_bandkwidth_kernel_HAC; }
            set { this.m_bandkwidth_kernel_HAC = value; }
        }

        protected TipoKernelCorrecaoHAC m_tipo_kernel_correcao_HAC = TipoKernelCorrecaoHAC.Barlett;
        public TipoKernelCorrecaoHAC TipoKernelCorrecaoHAC
        {
            get { return this.m_tipo_kernel_correcao_HAC; }
            set 
            { 
                this.m_tipo_kernel_correcao_HAC = value;
                switch (value)
                {
                    case TipoKernelCorrecaoHAC.Barlett:
                        m_kernel = new funcao_kernel(Barlett);
                        break;
                    case TipoKernelCorrecaoHAC.Epanechnikov:
                        m_kernel = new funcao_kernel(Epanechnikov);
                        break;
                    case TipoKernelCorrecaoHAC.Biquadrado:
                        m_kernel = new funcao_kernel(Biquadrado);
                        break;
                    default:
                        m_kernel = new funcao_kernel(Barlett);
                        break;
                }
            }
        }

        protected TipoCorrecaoMatrizCovariancia m_tipo_correcao_cov_matrix = TipoCorrecaoMatrizCovariancia.SemCorrecao;
        public TipoCorrecaoMatrizCovariancia TipoCorrecaoCovMatrix
        {
            get { return this.m_tipo_correcao_cov_matrix; }
            set { this.m_tipo_correcao_cov_matrix = value; }
        }

        protected DataTable m_dt_tabela_dados = new DataTable();
        protected string[] m_variaveis_dependentes = new string[0];
        protected string[] m_variaveis_independentes = new string[0];
        protected string[] m_variaveis_instrumentais = new string[0];
        protected string[] m_variaveis_exogenas = new string[0];
        protected string[] m_variaveis_endogenas = new string[0];

        protected bool m_usa_intercepto = true;
        protected string m_output_text = "";
        protected string m_output_variaveis_geradas = "";
        protected bool m_adiciona_novas_variaveis = false;

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

        protected bool m_apresenta_covmatrix_beta_hat = false;
        protected TipoCalculoLogDetWMatrix m_tipo_logdet = TipoCalculoLogDetWMatrix.SimulacoesMonteCarlo;

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

        public bool ApresentaCovMatrixBetaHat { set { this.m_apresenta_covmatrix_beta_hat = value; } }

        protected string m_variavel_coord_X = "";
        protected string m_variavel_coord_Y = "";
        protected double m_cutoff_X = 0.0;
        protected double m_cutoff_Y = 0.0;

        public string VariavelCoordenadaX { set { this.m_variavel_coord_X = value; } }
        public string VariavelCoordenadaY { set { this.m_variavel_coord_Y = value; } }
        public double CutOffCoordenadaX { set { this.m_cutoff_X = value; } }
        public double CutOffCoordenadaY { set { this.m_cutoff_Y = value; } }

        protected clsLinearRegressionModelsMLE m_geomle = new clsLinearRegressionModelsMLE();

        public clsMatrizEsparsa MatrizEsparsaFromDistancias
        {
            get
            {
                return m_geomle.Wesparsa;
            }
            set
            {
                m_geomle.Wesparsa = value;
            }
        }

        protected clsIpeaShape m_shape = new clsIpeaShape();
        public clsIpeaShape Shape
        {
            get { return this.m_shape.Clone(); }
            set { this.m_shape = value.Clone(); }
        }

        protected TipoMatrizVizinhanca m_tipo_matriz_vizinhanca = TipoMatrizVizinhanca.Normalizada;
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
            set { this.m_dt_tabela_dados = value.Copy(); }
            get { return this.m_dt_tabela_dados.Copy(); }
        }

        public string[] VariaveisDependentes
        {
            set { this.m_variaveis_dependentes = value; }
            get { return this.m_variaveis_dependentes; }
        }

        public string[] VariaveisIndependentes
        {
            set { this.m_variaveis_independentes = value; }
            get { return this.m_variaveis_independentes; }
        }

        public string[] VariaveisExogenas
        {
            get { return this.m_variaveis_exogenas; }
        }

        public string[] VariaveisInstrumentais
        {
            set { this.m_variaveis_instrumentais = value; }
            get { return this.m_variaveis_instrumentais; }
        }

        public double GeraMaximaDistancia(DataTable dt, string variavel)
        {
            clsUtilTools clt = new clsUtilTools();
            double[,] w = clt.GetMatrizFromDataTable(dt, variavel);

            return Math.Abs(clt.Max(w) - clt.Min(w));
        }

        #endregion

        #region checando a presença de multicolinearidade

        public bool ChecarMulticolinearidade(out string mensagem)
        {
            bool res = false;
            mensagem = "";

            clsUtilTools clt = new clsUtilTools();

            double[,] X_original = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, VariaveisIndependentes);
            double[,] X = clt.ArrayDoubleClone(X_original);

            double[] S = new double[X.GetLength(1)];
            double[,] x;
            for (int k = 0; k < X.GetLength(1); k++)
            {
                x = clt.SubColumnArrayDouble(X, k);

                if (clt.Sum(x) == 0.0 && clt.Varianciac(x)[0, 0] == 0.0)
                {
                    mensagem = "A variável independente " + VariaveisIndependentes[k] + " é constante (igual a " + clt.Double2Texto(X_original[0, k], 8)
                                + ") para todas as observações na tabela de dados."; 
                    return true;
                }

                S[k] = Math.Sqrt(clt.MatrizMult(clt.MatrizTransp(x), x)[0, 0] / ((double)x.GetLength(0)));
                for (int i = 0; i < X.GetLength(0); i++)
                {
                    X[i, k] = X[i, k] / S[k];
                }
            }
            double[,] X_noint = clt.ArrayDoubleClone(X);

            double variancia_x = 0.0;
            for (int k = 0; k < X_noint.GetLength(1); k++)
            {
                x = clt.SubColumnArrayDouble(X, k);
                variancia_x = clt.Varianciac(x)[0, 0];
                if (variancia_x < 1.0e-10)
                {
                    mensagem = "A variável independente " + VariaveisIndependentes[k] + " é constante (igual a " + clt.Double2Texto(X_original[0,k], 8) 
                                + ") para todas as observações na tabela de dados.";
                    return true;
                }
            }

            double[,] XtX;
            double[,] XtY;
            double[,] yk;
            double[,] Xk;
            double[,] beta = new double[0,0];
            double[,] yk_hat;
            double[,] R2k = new double[X_noint.GetLength(1), 1];
            clsSingularValueDecomposition svd;

            for (int k = 0; k < X_noint.GetLength(1); k++)
            {
                yk = clt.SubColumnArrayDouble(X_noint, k);
                Xk = clt.Concateh(clt.Ones(X_noint.GetLength(0), 1), clt.DeleteCol(X_noint, k));

                XtX = clt.MatrizMult(clt.MatrizTransp(Xk), Xk);
                XtY = clt.MatrizMult(clt.MatrizTransp(Xk), yk);

                beta = new double[XtX.GetLength(0), 1];
                svd = new clsSingularValueDecomposition(ref XtX);
                svd.solve(ref XtY, ref beta);

                //beta = clt.MatrizMult(clt.MatrizInversa(XtX, XtY);

                yk_hat = clt.MatrizMult(Xk, beta);
                R2k[k, 0] = clt.Varianciac(yk_hat)[0, 0] / clt.Varianciac(yk)[0, 0];
                if (R2k[k, 0] > 0.999999)
                {
                    string[] variaveis = clt.DeleteStringFromArray(VariaveisIndependentes, k);
                    mensagem = "A variável independente " + VariaveisIndependentes[k] 
                        + " é uma combinação linear dos demais regressores (R2 da regressão dessa variável versus as demais é igual a = " 
                        + clt.Double2Texto(R2k[k,0], 8) + "). A relação linear para as variáveis é " 
                        + VariaveisIndependentes[k] + " = " + clt.Double2Texto(beta[0,0] * S[k], 4);

                    for (int i=1; i<beta.GetLength(0); i++)
                    {
                        mensagem += " + " + clt.Double2Texto(beta[i,0] * S[k] / S[i], 4) + "x" + variaveis[i-1];
                    }
                    mensagem += ".";

                    return true;
                }
            }

            return res;
        }

        #endregion

        #region tabulações indicadores para diagnóstico de multicolinearidade

        protected string IndicadoresDuasColunas(string[] variaveis, double[,] beta, double[,] stderror, string nome_indicador1, string nome_indicador2)
        {
            clsUtilTools clt = new clsUtilTools();

            int max_length_variaveis = 0;
            int max_length_beta = 0;
            int max_length_stderror = 0;

            string[] st_beta = new string[beta.GetLength(0)];
            string[] st_stderror = new string[beta.GetLength(0)];
 
            for (int i = 0; i < beta.GetLength(0); i++)
            {
                st_beta[i] = clt.Double2Texto(beta[i, 0], 6);
                if (st_beta[i].Length > max_length_beta) max_length_beta = st_beta[i].Length;

                st_stderror[i] = clt.Double2Texto(stderror[i, 0], 6);
                if (st_stderror[i].Length > max_length_stderror) max_length_stderror = st_stderror[i].Length;

                if (variaveis[i].Length > max_length_variaveis) max_length_variaveis = variaveis[i].Length;
            }

            if ("Variável".Length > max_length_variaveis) max_length_variaveis = "Variável".Length;
            if (nome_indicador1.Length > max_length_beta) max_length_beta = nome_indicador1.Length;
            if (nome_indicador2.Length > max_length_stderror) max_length_stderror = nome_indicador2.Length;

            max_length_variaveis += 2;
            max_length_beta += 6;
            max_length_stderror += 15;

            string res = "";

            res += "Variável" + PreencheEspacos(max_length_variaveis - "Variável".Length);
            res += PreencheEspacos(max_length_beta - nome_indicador1.Length) + nome_indicador1;
            res += PreencheEspacos(max_length_stderror - nome_indicador2.Length) + nome_indicador2;
            res += "\n\n";

            for (int i = 0; i < beta.GetLength(0); i++)
            {
                res += variaveis[i] + PreencheEspacos(max_length_variaveis - variaveis[i].Length);
                res += PreencheEspacos(max_length_beta - st_beta[i].Length) + st_beta[i];
                res += PreencheEspacos(max_length_stderror - st_stderror[i].Length) + st_stderror[i];
                res += "\n";
            }

            return res;
        }

        #endregion

        #region tabulação matriz de variância-covariância

        protected string GeraTabelaCovMatrix(double[,] covmat, string[] variaveis_independentes, bool inclui_intercepto)
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

        protected void AdicionaNovasVariaveisToDataTable(double[,] dados, string[] variaveis)
        {
            clsUtilTools clt = new clsUtilTools();

            this.LimparTabelaDasVariaveisGeradas(ref this.m_dt_tabela_dados);

            if (m_adiciona_novas_variaveis)
            {
                clt.AdicionaColunasToDataTable(ref m_dt_tabela_dados, dados, variaveis);

                //for (int i = 0; i < variaveis.GetLength(0); i++)
                //{
                //    if (!this.m_dt_tabela_dados.Columns.Contains(variaveis[i]))
                //    {
                //        this.m_dt_tabela_dados.Columns.Add(variaveis[i], typeof(double));
                //    }

                //    for (int j = 0; j < dados.GetLength(0); j++)
                //    {
                //        this.m_dt_tabela_dados.Rows[j][variaveis[i]] = dados[j, i];
                //    }
                //}
            }
        }

        protected string GeraTabelaNovasVariaveis(double[,] dados, string[] variaveis)
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
                    sb.Append("\t" + PreencheEspacos(max_length[j] - st_dados[i, j].Length) + st_dados[i, j]);
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }

        #endregion

        #region tabulação estimações

        protected string ImprimeVetorColuna(double[] v)
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

        protected string GeraTabelaEstimacoes(string[] variaveis_independentes, double[,] beta,
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
                st_beta[i] = clt.Double2Texto(beta[i, 0], 6);
                if (st_beta[i].Length > max_length_beta) max_length_beta = st_beta[i].Length;

                st_stderror[i] = clt.Double2Texto(stderror[i, 0], 6);
                if (st_stderror[i].Length > max_length_stderror) max_length_stderror = st_stderror[i].Length;

                st_tstat[i] = clt.Double2Texto(tstat[i, 0], 6);
                if (st_tstat[i].Length > max_length_tstat) max_length_tstat = st_tstat[i].Length;

                st_pvalue[i] = clt.Double2Texto(pvalue[i, 0], 6);
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

        protected string PreencheEspacos(int n)
        {
            string res = "";
            for (int i = 0; i < n; i++) res += " ";
            return res;
        }

        #endregion

        #region weight matrices for GMM estimation

        private clsUtilTools clt = new clsUtilTools();

        protected double[,] GMM_weight_matrix(double[,] gt_vector)
        {
            double[,] dgt_vector = new double[gt_vector.GetLength(0), gt_vector.GetLength(1)];
            double[,] media = clt.Meanc(gt_vector);
            for (int i = 0; i < gt_vector.GetLength(0); i++)
            {
                for (int j = 0; j < gt_vector.GetLength(1); j++)
                {
                    dgt_vector[i, j] = gt_vector[i, j] - media[0, j];
                }
            }
            return clt.MatrizInversa(clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(dgt_vector), dgt_vector),
                                        1.0 / (double)dgt_vector.GetLength(0)));
        }

        protected double[,] Spatial_GMM_weight_matrix(double[,] gt_vector_in, double[,] x_coord, double[,] y_coord, double cutoff_x, double cutoff_y)
        {
            double[,] gt_vector = new double[gt_vector_in.GetLength(0), gt_vector_in.GetLength(1)];
            double[,] media = clt.Meanc(gt_vector_in);
            for (int i = 0; i < gt_vector_in.GetLength(0); i++)
            {
                for (int j = 0; j < gt_vector_in.GetLength(1); j++)
                {
                    gt_vector[i, j] = gt_vector_in[i, j] - media[0, j];
                }
            }

            int T = gt_vector.GetLength(0);
            double peso = 0.0;
            double D_horizontal = 0.0;
            double D_vertical = 0.0;
            int col_gt = gt_vector.GetLength(1);
            double[,] Omega_hat = new double[col_gt, col_gt];
            int contador = 0;
            for (int i = 0; i < T; i++)
            {
                for (int j = 0; j < T; j++)
                {
                    D_horizontal = Math.Abs(x_coord[i, 0] - x_coord[j, 0]);
                    D_vertical = Math.Abs(y_coord[i, 0] - y_coord[j, 0]);
                    if ((D_horizontal <= cutoff_x) && (D_vertical <= cutoff_y))
                    {
                        peso = (1.0 - (D_horizontal / cutoff_x)) * (1.0 - (D_vertical / cutoff_y));
                        Omega_hat = clt.MatrizSoma(Omega_hat,
                            clt.MatrizMult(peso, clt.MatrizMult(clt.MatrizTransp(clt.SubRowArrayDouble(gt_vector, i)), clt.SubRowArrayDouble(gt_vector, j))));
                        contador++;
                    }
                }
            }
            Omega_hat = clt.MatrizMult(Omega_hat, 1.0 / (double)gt_vector.GetLength(0));
            return clt.MatrizInversa(Omega_hat);
        }

        #endregion

        #region identificando variáveis endógenas e exógenas

        protected void variaveis_exogenas_e_endogenas(ref string[] nomes_vars_exogenas, ref double[,] vars_exogenas, 
                                                      ref string[] nomes_vars_endogenas, ref double[,] vars_endogenas,
                                                      ref string[] nomes_vars_instrumentais_puras, ref double[,] vars_instrumentais_puras)
        {
            ArrayList a_exogenas = new ArrayList();
            ArrayList a_endogenas = new ArrayList();
            for (int i = 0; i < this.m_variaveis_independentes.GetLength(0); i++)
            {
                if (clt.PosicaoNaLista(this.m_variaveis_instrumentais, m_variaveis_independentes[i]) >= 0)
                {
                    a_exogenas.Add(m_variaveis_independentes[i]);
                }
                else
                {
                    a_endogenas.Add(m_variaveis_independentes[i]);
                }
            }
            nomes_vars_endogenas = new string[a_endogenas.Count];
            nomes_vars_exogenas = new string[a_exogenas.Count];
            for (int i = 0; i < nomes_vars_exogenas.GetLength(0); i++) nomes_vars_exogenas[i] = (string)a_exogenas[i];
            for (int i = 0; i < nomes_vars_endogenas.GetLength(0); i++) nomes_vars_endogenas[i] = (string)a_endogenas[i];

            vars_endogenas = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, nomes_vars_endogenas);
            vars_exogenas = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, nomes_vars_exogenas);

            m_variaveis_exogenas = nomes_vars_exogenas;
            m_variaveis_endogenas = nomes_vars_endogenas;

            ArrayList a_inst_puras = new ArrayList();
            for (int i = 0; i < this.m_variaveis_instrumentais.GetLength(0); i++)
            {
                if (clt.PosicaoNaLista(this.m_variaveis_independentes, m_variaveis_instrumentais[i]) < 0)
                {
                    a_inst_puras.Add(m_variaveis_instrumentais[i]);
                }
            }
            nomes_vars_instrumentais_puras = new string[a_inst_puras.Count];
            for (int i = 0; i < nomes_vars_instrumentais_puras.GetLength(0); i++) nomes_vars_instrumentais_puras[i] = (string)a_inst_puras[i];

            vars_instrumentais_puras = clt.GetMatrizFromDataTable(this.m_dt_tabela_dados, nomes_vars_instrumentais_puras);
        }

        #endregion

        #region matriz phi para correção HAC da matriz de covariância das estimativas dos parâmetros

        protected double[,] Phi_correcao_HAC(double[,] mat, double[,] u_hat, double[,] coord_x, double[,] coord_y, double[] bandwidth)
        {
            int n = mat.GetLength(0);
            double[,] a1 = new double[mat.GetLength(1), mat.GetLength(1)];
            double[,] aux1;
            double[,] qi;
            double[,] qj;
            double d = 0.0;
            double peso = 0.0;
            double d_max = 0.0;
            for (int i = 0; i < n; i++)
            {
                d_max = bandwidth[i];
                qi = clt.SubRowArrayDouble(mat, i);
                aux1 = clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(qi), qi), Math.Pow(u_hat[i, 0], 2.0));
                a1 = clt.MatrizSoma(a1, aux1);
                for (int j = i + 1; j < n; j++)
                {
                    d = Math.Sqrt(Math.Pow(coord_x[i, 0] - coord_x[j, 0], 2.0) + Math.Pow(coord_y[i, 0] - coord_y[j, 0], 2.0));
                    if (d <= d_max)
                    {
                        peso = m_kernel(d, d_max);
                        qj = clt.SubRowArrayDouble(mat, j);
                        aux1 = clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(qi), qj), u_hat[i,0]*u_hat[j,0]);
                        aux1 = clt.MatrizMult(peso, aux1);
                        a1 = clt.MatrizSoma(a1, aux1);
                        aux1 = clt.MatrizMult(clt.MatrizMult(clt.MatrizTransp(qj), qi), u_hat[j, 0] * u_hat[i, 0]);
                        aux1 = clt.MatrizMult(peso, aux1);
                        a1 = clt.MatrizSoma(a1, aux1);
                    }
                }
            }
            return clt.MatrizMult(a1, 1.0 / ((double)n));
        }

        #endregion
    }
}
