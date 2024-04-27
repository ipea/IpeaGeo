using System;
using System.Data;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo.Modelagem
{
    public class BLogicGeracaoDummies
    {
        public BLogicGeracaoDummies()
        {
        }

        public void GerarDummies(ref DataTable dt_dados, string[] variaveis)
        {
            clsUtilTools clt = new clsUtilTools();

            object[,] dados = new object[dt_dados.Rows.Count, 1];
            object[,] freq_table = new object[0, 0];

            double[,] dummies = new double[0, 0];
            
            for (int j = 0; j < variaveis.GetLength(0); j++)
            {
                dados = new object[dt_dados.Rows.Count, 1];
                for (int i = 0; i < dados.GetLength(0); i++)
                {
                    dados[i, 0] = dt_dados.Rows[i][variaveis[j]];
                }

                if (!clt.ChecaLimiteCategorias(40, dados))
                {
                    throw new Exception("Variável " + variaveis[j] + " possui mais de 40 categorias. Não foi possível realizar a geração de dummies.");
                }

                clt.FrequencyTable(ref freq_table, dados);
                
                for (int z = 0; z < freq_table.GetLength(0); z++)
                {
                    string variavel = variaveis[j];
                    string variavel1 = freq_table[z, 0].ToString();
                    string nova_variavel = variavel + "_" + variavel1;

                    dt_dados.Columns.Add(nova_variavel, typeof(double));                                     
                }
              
                for (int i = 0; i < dt_dados.Rows.Count; i++)
                {                                     
                    for (int k = 0; k < freq_table.GetLength(0); k++)
                    {
                        string variavel = variaveis[j];
                        string variavel1 = freq_table[k, 0].ToString();
                        string nova_variavel = variavel + "_" + variavel1;
                        dt_dados.Rows[i][nova_variavel] = 0.0;
                                                                      
                        if (dados[i, 0].ToString() == freq_table[k, 0].ToString())
                        {                           
                            dt_dados.Rows[i][nova_variavel] = 1.0;                    
                        }                        
                    }
                }
            }
        }
    }
}
