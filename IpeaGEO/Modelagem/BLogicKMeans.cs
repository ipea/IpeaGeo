using System;
using System.Data;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public class BLogicKMeans
    {
        public BLogicKMeans()
        {
        }

        private int m_max_iteracoes = 20000;

        private IpeaGeo.RegressoesEspaciais.clsUtilTools m_clt = new IpeaGeo.RegressoesEspaciais.clsUtilTools();

        public void GeraClusters(DataTable dados, string[] variaveis, int num_clusters, bool usa_centroides_aleatorios, int num_centroides_aleatorios, bool normaliza_variaveis,
            ref int[,] matrix_indicador_clusters, ref int[] vetor_num_iteracoes, ref double[] vetor_impureza, ref int[] opt_indicadores, ref int opt_num_iteracoes, ref double opt_impureza, ref ProgressBar pbar)
        {
            clsUtilTools clt = new clsUtilTools();

            #region extraindo as variáveis

            double[,] x = new double[dados.Rows.Count, variaveis.GetLength(0)];
            for (int i = 0; i < x.GetLength(0); i++)
            {
                for (int j=0; j<x.GetLength(1);  j++)
                {
                    x[i, j] = Convert.ToDouble(dados.Rows[i][variaveis[j]]);
                }
            }

            if (normaliza_variaveis)
            {
                double[,] mediasc = clt.Meanc(x);
                double[,] stddevc = clt.Despadca(x);

                double[,] z = new double[x.GetLength(0), x.GetLength(1)];
                for (int i = 0; i < z.GetLength(0); i++)
                {
                    for (int j = 0; j < z.GetLength(1); j++)
                    {
                        z[i, j] = (x[i, j] - mediasc[0, j]) / stddevc[0, j];
                    }
                }

                x = clt.ArrayDoubleClone(z);
            }

            #endregion

            double[,] medias = new double[num_clusters, x.GetLength(1)];
            int nobs = x.GetLength(0);
            int[] amostra = m_clt.amostra_sem_reposicao(num_clusters, x.GetLength(0));
            double impureza = 0.0;

            int[] indicador_clusters = new int[0];
            int num_iteracoes = 0;

            int num_centroides = 1;
            if (usa_centroides_aleatorios) num_centroides = num_centroides_aleatorios;

            vetor_impureza = new double[num_centroides];
            vetor_num_iteracoes = new int[num_centroides];
            matrix_indicador_clusters = new int[x.GetLength(0), num_centroides];

            pbar.Minimum = 0;
            pbar.Value = 0;
            pbar.Maximum = num_centroides;

            if (usa_centroides_aleatorios)
            {
                #region usando centróides aleatórios

                int opt_k = 0;
                opt_impureza = Double.PositiveInfinity;
                
                for (int k = 0; k < num_centroides; k++)
                {
                    medias = new double[num_clusters, x.GetLength(1)];
                    nobs = x.GetLength(0);
                    amostra = m_clt.amostra_sem_reposicao(num_clusters, x.GetLength(0));

                    for (int i = 0; i < medias.GetLength(0); i++)
                    {
                        for (int j = 0; j < medias.GetLength(1); j++)
                        {
                            medias[i, j] = x[amostra[i], j];
                        }
                    }

                    GeraClustersCentroidesDados(x, ref medias, num_clusters, ref indicador_clusters, ref num_iteracoes, ref impureza);

                    vetor_num_iteracoes[k] = num_iteracoes;
                    vetor_impureza[k] = impureza;
                    for (int i = 0; i < x.GetLength(0); i++)
                    {
                        matrix_indicador_clusters[i, k] = indicador_clusters[i];
                    }

                    if (impureza < opt_impureza)
                    {
                        opt_impureza = impureza;
                        opt_k = k;
                    }

                    pbar.Increment(1);
                    Application.DoEvents();
                }

                opt_num_iteracoes = vetor_num_iteracoes[opt_k];
                opt_impureza = vetor_impureza[opt_k];
                opt_indicadores = new int[x.GetLength(0)];
                for (int i = 0; i < x.GetLength(0); i++)
                {
                    opt_indicadores[i] = matrix_indicador_clusters[i, opt_k];
                }

                #endregion
            }
            else
            {
                #region usando o primeiro componente principal

                double[,] cov_matrix = clt.CovSampleMatrix(x);

                double[,] W = new double[0, 0];
                double[,] U = new double[0, 0];
                double[,] V = new double[0, 0];

                clt.SingularValueDecomposition(ref W, ref U, ref V, cov_matrix);

                double[,] v1 = clt.SubColumnArrayDouble(V, 0);
                double[,] z = clt.MatrizMult(x, v1);

                object[,] zx = clt.ConvertMatrizDoubleToObj(clt.Concateh(z, x));             
                int resp = clt.SortByColumn(ref zx, zx, 0);
                double[,] xdsorted = clt.ConvertMatrixObjToDouble(zx);
                xdsorted = clt.RemoveColumnArrayDouble(xdsorted, 0);

                int[] ind_seed = new int[nobs];
                for (int i = 0; i < nobs; i++)
                {
                    ind_seed[i] = Math.Min(num_clusters-1, (int)Math.Floor((double)i * (double)num_clusters / (double)nobs));
                }

                medias = new double[num_clusters, x.GetLength(1)];
                int[] nmedias = new int[num_clusters];
                for (int i = 0; i < xdsorted.GetLength(0); i++)
                {
                    for (int j = 0; j < xdsorted.GetLength(1); j++)
                    {
                        medias[ind_seed[i], j] += xdsorted[i, j];
                    }
                    nmedias[ind_seed[i]]++;
                }
                
                for (int k = 0; k < medias.GetLength(0); k++)
                {
                    for (int j = 0; j < medias.GetLength(1); j++)
                    {
                        medias[k, j] = medias[k, j] / (double)nmedias[k];
                    }
                }

                GeraClustersCentroidesDados(x, ref medias, num_clusters, ref indicador_clusters, ref num_iteracoes, ref impureza);

                vetor_num_iteracoes[0] = num_iteracoes;
                vetor_impureza[0] = impureza;
                for (int i = 0; i < x.GetLength(0); i++)
                {
                    matrix_indicador_clusters[i, 0] = indicador_clusters[i];
                }

                opt_impureza = impureza;
                opt_indicadores = indicador_clusters;
                opt_num_iteracoes = num_iteracoes;

                pbar.Increment(1);
                Application.DoEvents();

                #endregion
            }
        }

        private void GeraClustersCentroidesDados(double[,] x, ref double[,] medias, int num_clusters, ref int[] indicador_clusters, ref int num_iteracoes, ref double impureza)
        {
            int nobs = x.GetLength(0);

            int[] indicador = new int[x.GetLength(0)];
            int[] indicador_anterior = new int[x.GetLength(0)];

            double[,] v1 = new double[0, 0];
            double[,] v2 = new double[0, 0];
            double min_dist = 0.0;
            double dist = 0.0;
            int[] n_obs_clusters = new int[num_clusters];

            int dif_indicador = 0;
            int iter = 0;

            for (iter = 0; iter < m_max_iteracoes; iter++)
            {
                indicador_anterior = new int[indicador.GetLength(0)];
                for (int i = 0; i < indicador.GetLength(0); i++)
                {
                    indicador_anterior[i] = indicador[i];
                }

                for (int i = 0; i < indicador.GetLength(0); i++)
                {
                    v1 = m_clt.SubRowArrayDouble(x, i);
                    min_dist = Double.PositiveInfinity;

                    for (int k = 0; k < medias.GetLength(0); k++)
                    {
                        v2 = m_clt.SubRowArrayDouble(medias, k);
                        dist = m_clt.distancia_entre_vetores_linha(v1, v2);
                        if (dist < min_dist)
                        {
                            indicador[i] = k;
                            min_dist = dist;
                        }
                    }
                }

                dif_indicador = 0;
                for (int i = 0; i < nobs; i++)
                {
                    dif_indicador += Math.Abs(indicador[i] - indicador_anterior[i]);
                }

                medias = new double[num_clusters, x.GetLength(1)];
                n_obs_clusters = new int[num_clusters];
                for (int i = 0; i < nobs; i++)
                {
                    n_obs_clusters[indicador[i]]++;
                    for (int j = 0; j < x.GetLength(1); j++)
                    {
                        medias[indicador[i], j] += x[i, j];
                    }
                }

                for (int k = 0; k < num_clusters; k++)
                {
                    for (int j = 0; j < x.GetLength(1); j++)
                    {
                        medias[k, j] = medias[k, j] / (double)n_obs_clusters[k];
                    }
                }

                if (dif_indicador <= 0)
                {
                    break;
                }
            }

            indicador_clusters = indicador;
            num_iteracoes = iter + 1;

            #region cálculo da impureza dos clusters finais

            clsUtilTools clt = new clsUtilTools();

            double[,] soma_erros2 = new double[1, x.GetLength(1)];

            for (int i = 0; i < x.GetLength(0); i++)
            {
                for (int j = 0; j < x.GetLength(1); j++)
                {
                    soma_erros2[0, j] += Math.Pow(x[i, j] - medias[indicador[i], j], 2.0);
                }
            }

            impureza = clt.Sum(soma_erros2) / (double)(x.GetLength(0)*x.GetLength(1));

            #endregion 
        }
    }
}
