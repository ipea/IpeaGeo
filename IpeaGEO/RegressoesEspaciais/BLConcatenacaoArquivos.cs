using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace IpeaGeo.RegressoesEspaciais
{
    public class BLConcatenacaoArquivos
    {
        public BLConcatenacaoArquivos()
        {
        }
        
        #region Tratamento números missing

        private DataTable m_dt_dados_missing = new DataTable();
        public DataTable TabelaDadosMissing
        {
            get
            {
                if (m_dt_dados_missing.Columns.Contains("sequencial_tabela_shape"))
                {
                    m_dt_dados_missing.Columns.Remove("sequencial_tabela_shape");
                }
                if (m_dt_dados_missing.Columns.Contains("sequencial_tabela_dados"))
                {
                    m_dt_dados_missing.Columns.Remove("sequencial_tabela_dados");
                }
                return m_dt_dados_missing;
            }
        }
     
        #region montando a tabela com obervações missing

        public void GeraTabelaDadosMissing(clsArrayVariaveisNumMissing lista_var_missing, DataTable dt)
        {
            m_dt_dados_missing = dt.Clone();
            ArrayList all_indices = new ArrayList();
            for (int i = 0; i < lista_var_missing.Count; i++)
            {
                for (int j = 0; j < lista_var_missing[i].IndicesValoresMissing.Count; j++)
                {
                    if (!all_indices.Contains(Convert.ToInt32(lista_var_missing[i].IndicesValoresMissing[j])))
                    {
                        all_indices.Add(Convert.ToInt32(lista_var_missing[i].IndicesValoresMissing[j]));
                    }
                }
            }

            all_indices.Sort();
            object[] linha;
            int indice = 0;
            for (int i = 0; i < all_indices.Count; i++)
            {
                indice = Convert.ToInt32(all_indices[i]);
                linha = dt.Rows[indice].ItemArray;
                m_dt_dados_missing.Rows.Add(linha);
            }

            if (m_dt_dados_missing.Columns.Contains("Observacoes_missing"))
            {
                m_dt_dados_missing.Columns.Remove("Observacoes_missing");
            }
            m_dt_dados_missing.Columns.Add("Observacoes_missing", typeof(int));
            m_dt_dados_missing.Columns["Observacoes_missing"].SetOrdinal(0);
            for (int i = 0; i < all_indices.Count; i++)
            {
                indice = Convert.ToInt32(all_indices[i]);
                m_dt_dados_missing.Rows[i]["Observacoes_missing"] = indice;
            }
        }

        #endregion

        public clsArrayVariaveisNumMissing IdentificarVariaveisNumericasMissing(DataTable dt)
        {
            clsArrayVariaveisNumMissing res = new clsArrayVariaveisNumMissing();

            clsVariaveisNumMissing item = new clsVariaveisNumMissing();
            clsUtilTools clt = new clsUtilTools();
            string[] col_num = clt.RetornaColunasDouble(dt);
            double[] dados = new double[dt.Rows.Count];
            double valor = 0.0;
            ArrayList indices_missing = new ArrayList();
            for (int j = 0; j < col_num.GetLength(0); j++)
            {
                dados = new double[dt.Rows.Count];
                indices_missing = new ArrayList();
                for (int i = 0; i < dados.GetLength(0); i++)
                {
                    try
                    {
                        valor = Convert.ToDouble(dt.Rows[i][col_num[j]]);
                        if (double.IsInfinity(valor) || double.IsNaN(valor) || double.IsNegativeInfinity(valor) 
                            || double.IsPositiveInfinity(valor))
                        {
                            dados[i] = double.NaN;
                            indices_missing.Add(i);
                        }
                        else
                        {
                            dados[i] = valor;
                        }
                    }
                    catch
                    {
                        dados[i] = double.NaN;
                        indices_missing.Add(i);
                    }
                }

                if (indices_missing.Count > 0)
                {
                    item = new clsVariaveisNumMissing();
                    item.IndicesValoresMissing = indices_missing;
                    item.NomeVariavelNum = col_num[j];
                    item.ValoresVariavel = dados;
                    item.TipoImputacao = TipoImputacaoNumMissing.Zero;
                    item.ValorFixo = 0.0;
                    res.Add(item);
                }
            }

            return res;
        }

        #endregion

        private int m_num_obs_dados = 0;
        private int m_num_obs_shape = 0;
        private int m_num_obs_em_comum = 0;

        private string m_output_text = "";
        public string OutputText
        {
            get
            {
                return m_output_text;
            }
        }

        public object[,] FrequenciasTabelaDadosPainelEspacial(DataTable dt, string var_periodo, DataTable dt_shape, ref string messagem, ref DataSet ds_painel, ref string[] colunas_identificadores_unicos)
        {
            clsUtilTools clt = new clsUtilTools();

            object[,] col = clt.GetObjMatrizFromDataTable(dt, var_periodo);
            clt.FrequencyTable(ref col, col);

            if (col.GetLength(0) == 1)
            {
                messagem = "A variável selecionada para o período do painel espacial possui apenas um valor. Escolha outra variável para os períodos ou execute a importação e concatenação com o shape sem utilizar a opção de painel espacial.";
                return col;
            }

            messagem = "";
            int nobs_shape = dt_shape.Rows.Count;
            int nperiodo = 0;

            for (int i = 0; i < col.GetLength(0); i++)
            {
                nperiodo = Convert.ToInt32(col[i, 1]);
                if ((double)nperiodo < 0.8 * (double)nobs_shape)
                {
                    messagem = "Para o período " + col[i,0].ToString() + ", o número de observações na tabela de dados é menor do que 80% do número de polígonos no arquivo shape.";
                    messagem += " Escolha outra variável para o período dos dados ou refaça a importação com outra tabela.";
                    return col; 
                }
            }

            DataTable aux = new DataTable();
            string nome_periodo = "";
            object[] itens;
            string[] colunas_unicas = new string[0];
            string[] colunas_novas = new string[0];

            for (int k = 0; k < col.GetLength(0); k++)
            {
                nome_periodo = col[k, 0].ToString();

                aux = dt.Clone();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (nome_periodo == dt.Rows[i][var_periodo].ToString())
                    {
                        itens = dt.Rows[i].ItemArray;
                        aux.Rows.Add(itens);
                    }
                }

                if (k == 0)
                {
                    colunas_unicas = clt.RetornaUniqueColunas(aux);
                }
                else
                {
                    colunas_novas = clt.RetornaUniqueColunas(aux);
                    colunas_unicas = clt.RetornaIntersecaoDuasListas(colunas_novas, colunas_unicas);
                }

                aux.TableName = nome_periodo;
                ds_painel.Tables.Add(aux.Copy());

                if (colunas_unicas.GetLength(0) <= 0)
                {
                    messagem = "Não existem colunas com identificadores únicos para os dados de painel espacial. Verifique a sua tabela de dados."; 
                    ds_painel = new DataSet();
                    return col;
                }
            }

            colunas_identificadores_unicos = colunas_unicas;

            return col;
        }

        public DataTable DadosPainelConcatenarShapeToDados(DataTable dt_shape, string id_shape, DataSet ds_dados, string id_dados, bool anexar_dados_shape, 
            object[,] freqs_periodos, ref DataSet ds_concatenados, ref string output_relatorio)
        {
            output_relatorio = "===========================================================================================\n\n";
            output_relatorio += "CONCATENAÇÃO DA TABELA DE DADOS DE PAINEL COM O ARQUIVO EM FORMATO SHAPE \n\n";
            output_relatorio += "###### Período do painel espacial de dados: " + freqs_periodos[0, 0].ToString() + "\n\n";

            DataTable dt = (DataTable)ds_dados.Tables[0];
            DataTable res = new DataTable();
            DataTable aux = ConcatenaShapeToDados(dt_shape, id_shape, dt, id_dados, anexar_dados_shape);
            res = aux.Copy();

            output_relatorio += this.OutputText;
            
            ds_concatenados = new DataSet();

            aux.TableName = freqs_periodos[0, 0].ToString();
            ds_concatenados.Tables.Add(aux);

            for (int k = 1; k < ds_dados.Tables.Count; k++)
            {                
                dt = (DataTable)ds_dados.Tables[k];
                aux = ConcatenaShapeToDados(dt_shape, id_shape, dt, id_dados, anexar_dados_shape);
                res.Merge(aux);

                aux.TableName = freqs_periodos[k, 0].ToString();
                ds_concatenados.Tables.Add(aux);

                output_relatorio += "\n\n===========================================================================================\n\n";
                output_relatorio += "CONCATENAÇÃO DA TABELA DE DADOS DE PAINEL COM O ARQUIVO EM FORMATO SHAPE \n\n";
                output_relatorio += "###### Período do painel espacial de dados: " + freqs_periodos[k, 0].ToString() + "\n\n";

                output_relatorio += this.OutputText;
            }
            output_relatorio += "\n";

            return res;
        }

        public DataTable ConcatenaShapeToDados(DataTable dt_shape, string id_shape, DataTable dt_dados, string id_dados, bool anexar_dados_shape)
        {
            DataTable dt = new DataTable();
            clsUtilTools clt = new clsUtilTools();

            #region checando as variáveis identificadoras

            object[,] vid_shape = new object[dt_shape.Rows.Count, 1];
            for (int i = 0; i < vid_shape.GetLength(0); i++)
            {
                vid_shape[i, 0] = dt_shape.Rows[i][id_shape];
            }

            if (!ChecaVariavelChaveUnica(vid_shape))
            {
                throw new Exception("Variável identificadora na tabela do shape não é uma chave única. Escolha uma outra variável");
            }

            object[,] vid_dados = new object[dt_dados.Rows.Count, 1];
            for (int i = 0; i < vid_dados.GetLength(0); i++)
            {
                vid_dados[i, 0] = dt_dados.Rows[i][id_dados];
            }

            if (!ChecaVariavelChaveUnica(vid_dados))
            {
                throw new Exception("Variável identificadora na tabela de dados não é uma chave única. Escolha uma outra variável.");
            }

            #endregion

            #region encontrando a associação entre observações nas duas tabelas 

            m_num_obs_dados = dt_dados.Rows.Count;
            m_num_obs_shape = dt_shape.Rows.Count;

            object[,] sid_shape = new object[vid_shape.GetLength(0), 2];
            for (int i = 0; i < sid_shape.GetLength(0); i++)
            {
                sid_shape[i, 0] = vid_shape[i, 0];
                sid_shape[i, 1] = i;
            }
            clt.SortByColumn(ref sid_shape, sid_shape, 0);

            object[,] sid_dados = new object[vid_dados.GetLength(0), 2];
            for (int i = 0; i < sid_dados.GetLength(0); i++)
            {
                sid_dados[i, 0] = vid_dados[i, 0];
                sid_dados[i, 1] = i;
            }
            clt.SortByColumn(ref sid_dados, sid_dados, 0);

            object[,] vid_all = clt.Concatev(vid_dados, vid_shape);
            object[,] ftable = new object[0, 0];
            clt.FrequencyTable(ref ftable, vid_all);
            ftable = clt.SubColumnArrayDouble(ftable, 0);
            clt.SortByColumn(ref ftable, ftable, 0);

            vid_all = new object[ftable.GetLength(0), 3];

            int indice_dados = 0;
            int indice_shape = 0;

            for (int i = 0; i < vid_all.GetLength(0); i++)
            {
                vid_all[i, 0] = ftable[i, 0];
                vid_all[i, 1] = -1;
                vid_all[i, 2] = -1;

                for (int j = indice_shape; j < sid_shape.GetLength(0); j++)
                {
                    if (vid_all[i, 0].ToString() == sid_shape[j, 0].ToString())
                    {
                        vid_all[i, 1] = sid_shape[j, 1];
                        indice_shape = j + 1;
                        break;
                    }
                }

                for (int j = indice_dados; j < sid_dados.GetLength(0); j++)
                {
                    if (vid_all[i, 0].ToString() == sid_dados[j, 0].ToString())
                    {
                        vid_all[i, 2] = sid_dados[j, 1];
                        indice_dados = j + 1;
                        break;
                    }
                }
            }

            m_num_obs_em_comum = 0;

            object[,] vid_shape_dados = new object[sid_shape.GetLength(0), 3];
            int indice_shape_dados = 0;
            for (int i = 0; i < vid_all.GetLength(0); i++)
            {
                if (Convert.ToInt32(vid_all[i, 1]) >= 0)
                {
                    if (Convert.ToInt32(vid_all[i, 2]) >= 0)
                    {
                        m_num_obs_em_comum++;
                    }

                    vid_shape_dados[indice_shape_dados, 0] = vid_all[i, 0];
                    vid_shape_dados[indice_shape_dados, 1] = vid_all[i, 1];
                    vid_shape_dados[indice_shape_dados, 2] = vid_all[i, 2];
                    indice_shape_dados++;
                }
            }

            #endregion

            #region montando a tabela de dados concatenados

            #region atribuindo novo nome ao identificador no arquivo shape

            string nome_novo_id = "shape_" + id_shape;
            if (dt_dados.Columns.Contains(nome_novo_id)) 
            {
                if (!dt_dados.Columns.Contains(nome_novo_id + "_1"))
                {
                    nome_novo_id = nome_novo_id + "_1";
                }
                else
                {
                    if (!dt_dados.Columns.Contains(nome_novo_id + "_2"))
                    {
                        nome_novo_id = nome_novo_id + "_2";
                    }
                    else
                    {
                        if (!dt_dados.Columns.Contains(nome_novo_id + "_3"))
                        {
                            nome_novo_id = nome_novo_id + "_3";
                        }
                        else
                        {
                            if (!dt_dados.Columns.Contains(nome_novo_id + "_4"))
                            {
                                nome_novo_id = nome_novo_id + "_4";
                            }
                            else
                            {
                                if (!dt_dados.Columns.Contains(nome_novo_id + "_5"))
                                {
                                    nome_novo_id = nome_novo_id + "_5";
                                }
                                else
                                {
                                    if (!dt_dados.Columns.Contains(nome_novo_id + "_6"))
                                    {
                                        nome_novo_id = nome_novo_id + "_6";
                                    }
                                    else
                                    {
                                        if (!dt_dados.Columns.Contains(nome_novo_id + "_7"))
                                        {
                                            nome_novo_id = nome_novo_id + "_7";
                                        }
                                        else
                                        {
                                            nome_novo_id = nome_novo_id + "_8";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            if (dt.Columns.Contains("sequencial_tabela_shape")) { dt.Columns.Remove("sequencial_tabela_shape"); }
            if (dt.Columns.Contains("sequencial_tabela_dados")) { dt.Columns.Remove("sequencial_tabela_dados"); }

            dt.Columns.Add(nome_novo_id, dt_shape.Columns[id_shape].DataType);
            dt.Columns.Add("sequencial_tabela_shape", typeof(Int32));
            dt.Columns.Add("sequencial_tabela_dados", typeof(Int32));
            for (int k = 0; k < dt_dados.Columns.Count; k++)
            {
                dt.Columns.Add(dt_dados.Columns[k].ColumnName, dt_dados.Columns[k].DataType);
            }

            object[] nova_linha = new object[3+dt_dados.Columns.Count];
            int i_shape = 0;
            int j_dados = 0;

            for (int k = 0; k < vid_shape_dados.GetLength(0); k++)
            {
                i_shape = Convert.ToInt32(vid_shape_dados[k, 1]);
                j_dados = Convert.ToInt32(vid_shape_dados[k, 2]);
                nova_linha[0] = dt_shape.Rows[i_shape][id_shape];
                nova_linha[1] = i_shape;
                nova_linha[2] = j_dados;

                if (j_dados >= 0)
                {
                    for (int l = 0; l < dt_dados.Columns.Count; l++)
                    {
                        nova_linha[3 + l] = dt_dados.Rows[j_dados][l];
                        nova_linha[0] = dt_shape.Rows[i_shape][id_shape];
                        nova_linha[1] = i_shape;
                        nova_linha[2] = j_dados;
                    }
                }
                else
                {
                    nova_linha = new object[3 + dt_dados.Columns.Count];
                    nova_linha[0] = dt_shape.Rows[i_shape][id_shape];
                    nova_linha[1] = i_shape;
                    nova_linha[2] = j_dados;
                }

                dt.Rows.Add(nova_linha);
            }

            #region adicionando as variáveis da tabela do shape 

            if (anexar_dados_shape)
            {
                DataTable dt_aux = dt_shape.Copy();
                string nome_novo = "";

                dt_aux.Columns.Remove(id_shape);

                for (int k = 0; k < dt_aux.Columns.Count; k++)
                {
                    nome_novo = "shape_" + dt_aux.Columns[k].ColumnName;
                    nome_novo = clt.nome_nova_coluna(dt_aux, nome_novo);
                    dt_aux.Columns[k].ColumnName = nome_novo;
                }

                int[] posicao_tabela_shape = new int[dt.Rows.Count];
                for (int i = 0; i < posicao_tabela_shape.GetLength(0); i++)
                {
                    posicao_tabela_shape[i] = Convert.ToInt32(dt.Rows[i]["sequencial_tabela_shape"]);
                }

                for (int k = 0; k < dt_aux.Columns.Count; k++)
                {
                    nome_novo = dt_aux.Columns[k].ColumnName;
                    nome_novo = clt.nome_nova_coluna(dt, nome_novo);

                    dt.Columns.Add(nome_novo, dt_aux.Columns[k].DataType);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i][nome_novo] = dt_aux.Rows[posicao_tabela_shape[i]][dt_aux.Columns[k].ColumnName];
                    }
                }
            }

            #endregion
            
            dt.DefaultView.Sort = "[" + dt.Columns[1].ColumnName + "] asc";

            #endregion

            #region montando o relatório de saida

            if ((double)m_num_obs_em_comum / (double)m_num_obs_shape < 0.95)
            {
                throw new Exception("Mais de 5% das observações no arquivo shape não possuem correspondentes na tabela de dados. Escolha outras variáveis de ligação ou cheque os seus dados.");
            }

            string out_text = "";
            out_text += "===========================================================================================\n\n";
            out_text += "Concatenação da tabela de dados com o arquivo em formato shape \n\n";

            out_text += "Data: " + System.DateTime.Now.ToLongDateString() + "\n";
            out_text += "Hora: " + System.DateTime.Now.ToLongTimeString() + "\n\n";

            out_text += "Variável identificadora na tabela de dados: " + id_dados + "\n";
            out_text += "Variável identificadora no arquivo shape: " + id_shape + "\n\n";

            out_text += "Número de observações na tabela de dados: " + m_num_obs_dados + "\n";
            out_text += "Número de observações no arquivo shape: " + m_num_obs_shape + "\n";
            out_text += "Número de observações em comum: " + m_num_obs_em_comum + "\n\n";

            if (anexar_dados_shape)
            {
                out_text += "Obs: as colunas da tabela do shape foram anexadas à tabela final gerada.\n" +
                    " Quando necessário, os nomes das colunas da tabela do shape foram modificados\n" +
                    " para evitar duplicação de nomes de variáveis.";
            }
            else
            {
                out_text += "Obs: as colunas da tabela do shape não foram anexadas à tabela final gerada.";
            }

            m_output_text = out_text;

            #endregion

            return dt.DefaultView.ToTable();
        }

        /// <summary>
        /// Função para checar se uma determinada variável é uma chave única ou não.
        /// </summary>
        /// <param name="v">Matriz com apenas uma coluna, com os valores da variável a ser checada.</param>
        /// <returns>Retorna true se variável na coluna é chave única e false caso contrário.</returns>
        public bool ChecaVariavelChaveUnica(object[,] v)
        {
            bool res = true;

            clsUtilTools clt = new clsUtilTools();
            object[,] vs = clt.SortcDoubleArray(v);

            for (int i = 1; i < vs.GetLength(0); i++)
            {
                if (vs[i, 0].ToString() == vs[i - 1, 0].ToString())
                    return false;
            }

            return res;
        }
    }
}
