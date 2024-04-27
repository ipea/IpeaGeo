using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsUtilObjects
    {
        public clsUtilObjects()
        {
        }
        public double[,] DataTableToMatrizDouble(DataTable dt, string nome_coluna)
        {
            double[,] res = new double[dt.Rows.Count, 1];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                res[i, 0] = Convert.ToDouble(dt.Rows[i][nome_coluna]);
            }
            return res;
        }
        public object[,] DataTableToMatrizObject(DataTable dt, string nome_coluna)
        {
            object[,] res = new object[dt.Rows.Count, 1];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                res[i, 0] = dt.Rows[i][nome_coluna];
            }
            return res;
        }

        public object[,] DataTableToMatrizObject(DataTable dt)
        {
            object[,] res = new object[dt.Rows.Count, dt.Columns.Count];
            for (int i=0; i<dt.Rows.Count; i++)
            {
                for (int j=0; j<dt.Columns.Count; j++)
                {
                    res[i, j] = dt.Rows[i][j];
                }
            }
            return res;
        }
        /// <summary>
        /// Tabela de frequência para uma variável categórica. 
        /// </summary>
        /// <param name="table">Matriz de saída: primeira coluna corresponde às categorias; segunda coluna corresponde à contagem de cada categoria.</param>
        /// <param name="cats">Vetor coluna com as categorias.</param>
        public void FrequencyTable(ref double[,] table, double[,] cats)
        {
            ArrayList lista_categorias = new ArrayList();
            for (int i = 0; i < cats.GetLength(0); i++)
            {
                if (!lista_categorias.Contains(cats[i, 0])) lista_categorias.Add(cats[i, 0]);
            }

            table = new double[lista_categorias.Count, 2];
            for (int j = 0; j < table.GetLength(0); j++)
            {
                table[j, 0] = Convert.ToDouble(lista_categorias[j]);
            }

            int[] freq = new int[table.GetLength(0)];
            for (int i = 0; i < cats.GetLength(0); i++)
            {
                for (int k = 0; k < table.GetLength(0); k++)
                {
                    if (cats[i, 0] == table[k, 0])
                    {
                        freq[k]++;
                        break;
                    }
                }
            }

            for (int j = 0; j < table.GetLength(0); j++)
            {
                table[j, 1] = freq[j];
            }
        }
        /// <summary>
        /// Tabela de frequência para uma variável categórica. 
        /// </summary>
        /// <param name="table">Matriz de saída: primeira coluna corresponde às categorias; segunda coluna corresponde à contagem de cada categoria.</param>
        /// <param name="cats">Vetor coluna com as categorias.</param>
        public void FrequencyTable(ref object[,] table, object[,] cats)
        {
            ArrayList lista_categorias = new ArrayList();
            for (int i = 0; i < cats.GetLength(0); i++)
            {
                if (!lista_categorias.Contains(cats[i, 0])) lista_categorias.Add(cats[i, 0]);
            }

            table = new object[lista_categorias.Count, 2];
            for (int j = 0; j < table.GetLength(0); j++)
            {
                table[j, 0] = lista_categorias[j];
            }

            int[] freq = new int[table.GetLength(0)];
            for (int i = 0; i < cats.GetLength(0); i++)
            {
                for (int k = 0; k < table.GetLength(0); k++)
                {
                    if (cats[i, 0] == table[k, 0])
                    {
                        freq[k]++;
                        break;
                    }
                }
            }

            for (int j = 0; j < table.GetLength(0); j++)
            {
                table[j, 1] = freq[j];
            }            
        }
    }
}
