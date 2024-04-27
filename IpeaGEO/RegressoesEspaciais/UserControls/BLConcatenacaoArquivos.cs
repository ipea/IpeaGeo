using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace IpeaGEO.RegressoesEspaciais
{
    public class BLConcatenacaoArquivos
    {
        public BLConcatenacaoArquivos()
        {
        }

        private int m_num_obs_dados = 0;
        private int m_num_obs_shape = 0;
        private int m_num_obs_em_comum = 0;

        public DataTable ConcatenaShapeToDados(DataTable dt_shape, string id_shape, DataTable dt_dados, string id_dados)
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

            string nome_novo_id = "shape_" + id_shape;
            if (dt_dados.Columns.Contains(nome_novo_id)) 
            {
                nome_novo_id = nome_novo_id + "_1";
            }

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
                    }
                }

                dt.Rows.Add(nova_linha);
            }

            dt.DefaultView.Sort = "[" + dt.Columns[1].ColumnName + "] asc";

            #endregion

            #region montando o relatório de saida

            if ((double)m_num_obs_em_comum / (double)m_num_obs_shape < 0.95)
            {
                throw new Exception("Mais de 5% das observações no arquivo shape não possuem correspondentes na tabela de dados. Escolha outras variáveis de ligação ou cheque os seus dados.");
            }

            #endregion

            return dt.DefaultView.ToTable();
        }

        private string completa_inteiro(int n)
        {
            string sn = n.ToString();
            string r = sn;
            for (int i = 0; i < 12 - sn.Length; i++)
            {
                r = "0" + r;
            }
            return r;
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
