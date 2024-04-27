using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data;

namespace IpeaGEO
{
    class clsFuncaoAgregacao
    {

            

        private double[,] primeiro_by(DataTable tabela, string var1, string nome)
        {
            clsUtilObjects clo = new clsUtilObjects();

            object[,] dados = clo.DataTableToMatrizObject(tabela, nome);
            object[,] freqs = new object[0, 0];
            object[,] variavel = clo.DataTableToMatrizObject(tabela, var1);

            clo.FrequencyTable(ref freqs, dados);

            object[,] somas_by = new object[freqs.GetLength(0), 2];
            for (int k = 0; k < freqs.GetLength(0); k++)
            {
                somas_by[k, 0] = freqs[k, 0];
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                for (int k = 0; k < freqs.GetLength(0); k++)
                {
                    if (dados[i, 0] == freqs[k, 0] && somas_by[k, 1]==null)
                    {
                        somas_by[k, 1] = Convert.ToDouble(variavel[i, 0]);
                    }
                }
            }


            return new double[0, 0];
        }



        private double[,] soma_by(DataTable tabela, string var1, string nome)
        {
            clsUtilObjects clo = new clsUtilObjects();

            object[,] dados = clo.DataTableToMatrizObject(tabela, nome);
            object[,] freqs = new object[0, 0];
            object[,] variavel = clo.DataTableToMatrizObject(tabela, var1);

            clo.FrequencyTable(ref freqs, dados);
            
            object[,] somas_by = new object[freqs.GetLength(0), 2];
            for (int k = 0; k < freqs.GetLength(0); k++)
            {
                somas_by[k, 0] = freqs[k, 0];
            }

            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                for (int k=0; k<freqs.GetLength(0); k++)
                {
                    if (dados[i,0] == freqs[k, 0])
                    {
                        somas_by[k, 1] = Convert.ToDouble(somas_by[k, 1]) + Convert.ToDouble(variavel[i,0]);
                    }
                }
            }


            return new double[0, 0];
        }



        private double[,] soma_by_ponderada(DataTable tabela, string var1, string nome, string ponderar)
        {
            clsUtilObjects clo = new clsUtilObjects();

            double[,] dados = clo.DataTableToMatrizDouble(tabela, nome);
            double[,] freqs = new double[0, 0];
            double[,] variavel = clo.DataTableToMatrizDouble(tabela, var1);
            double[,] ponderacao = clo.DataTableToMatrizDouble(tabela, ponderar);

            
            clo.FrequencyTable(ref freqs, dados);

            object[,] somas_by = new object[freqs.GetLength(0), 2];
            for (int k = 0; k < freqs.GetLength(0); k++)
            {
                somas_by[k, 0] = freqs[k, 0];
            }
            double somapop = 0;
            for (int m = 0; m < ponderacao.Length; m++)
            {
                somapop = somapop + Convert.ToDouble(ponderacao[m, 0]);
            }


            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                for (int k = 0; k < freqs.GetLength(0); k++)
                {
                    if (dados[i, 0] == freqs[k, 0])
                    {
                        somas_by[k, 1] = Convert.ToDouble(somas_by[k, 1]) + Convert.ToDouble(variavel[i, 0]) * Convert.ToDouble(ponderacao[i, 0])/somapop;
                    }
                }
            }

        
            


            return new double[0, 0];
        }

        private bool existenalista(ArrayList lista,string valor)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                if (valor == lista[i]) return true;
            }

            return (false);
        }

        private ArrayList listaMicro(DataTable tabela, string var)
        {
            ArrayList lista=new ArrayList();
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                if(!existenalista(lista,tabela.Rows[i][var].ToString())) lista.Add(tabela.Rows[i][var].ToString()); 
            }
            return(lista);
 
        }
        private ArrayList listaMicroMaisPosicao(DataTable tabela, string var)
        {
            ArrayList lista=new ArrayList();
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                if(!existenalista(lista,tabela.Rows[i][var].ToString())) 
                {
                    ArrayList guarda=new ArrayList(2);
                    guarda.Add(tabela.Rows[i][var].ToString());
                    guarda.Add(i);
                    lista.Add(guarda); 
                }
            }
            return(lista);
 
        }
        private ArrayList listaMicroCompleta(DataTable tabela, string var)
        {
            ArrayList lista=new ArrayList();
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                if(!existenalista(lista,tabela.Rows[i][var].ToString())) 
                {       
                    
                    ArrayList guarda=new ArrayList();                    
                    guarda.Add(tabela.Rows[i][var].ToString());
                    
                    for(int j=i;j<tabela.Rows.Count;j++)
                    {
                        if(tabela.Rows[j][var]==tabela.Rows[i][var].ToString())
                        {
                            guarda.Add(j);
                            lista.Add(guarda); 
                        }
                    }
                }
            }
            return(lista);
 
        }
        public void agregacao_primeiro_valor(ref DataTable novo, DataTable tabela, string micro, string var)
        {
            ArrayList resultados=listaMicroMaisPosicao(tabela,var);
            DataColumn coluna=new DataColumn(var);
            coluna.DataType = tabela.GetType();
            novo.Columns.Add(coluna);

            DataRow myNewRow; 

            for(int i=0;i<resultados.Count;i++)
            {

                ArrayList posicaoi=(ArrayList )resultados[i];
                int j=(int)posicaoi[1];
                myNewRow = novo.NewRow();
                myNewRow[var] =tabela.Rows[j][var];
                novo.Rows.Add(myNewRow);
            }
        
        }

        public void agregacao_soma(ref DataTable novo, DataTable tabela, string micro, string var)
        {
            ArrayList resultados = listaMicroCompleta(tabela, var);
            DataColumn coluna = new DataColumn(var);
            coluna.DataType = tabela.GetType();
            novo.Columns.Add(coluna);

            DataRow myNewRow;

            for (int i = 0; i < resultados.Count; i++)
            {
                ArrayList posicaoi = (ArrayList)resultados[i];
                int tamanho = posicaoi.Count - 1;
                double ivariavel = 0;
                for (int j = 0; j < tamanho - 1; j++)
                {
                    int linha = (int)posicaoi[j + 1];
                    ivariavel += (double)tabela.Rows[linha][var];
                }
                myNewRow = novo.NewRow();
                myNewRow[var] = ivariavel;
                novo.Rows.Add(myNewRow);
            }

        }

        public void agregacao_media_simples(ref DataTable novo, DataTable tabela, string micro, string var)
        {
            ArrayList resultados=listaMicroCompleta(tabela,var);
            DataColumn coluna=new DataColumn(var);
            coluna.DataType = tabela.GetType();
            novo.Columns.Add(coluna);

            DataRow myNewRow; 

            for(int i=0;i<resultados.Count;i++)
            {
               ArrayList posicaoi=(ArrayList)resultados[i];
               int tamanho=posicaoi.Count-1;
                double ivariavel=0;
                for(int j=0;j<tamanho-1;j++)
                {
                    int linha=(int)posicaoi[j+1];
                    ivariavel+=(double)tabela.Rows[linha][var];
                }
                ivariavel/=tamanho;
                myNewRow = novo.NewRow();
                myNewRow[var] =ivariavel;
                novo.Rows.Add(myNewRow);
            }
        
        }
        public void agregacao_media_ponderada(ref DataTable novo, DataTable tabela, string micro, string var, string ponderador)
        {
            ArrayList resultados=listaMicroCompleta(tabela,var);
            DataColumn coluna=new DataColumn(var);
            coluna.DataType = tabela.GetType();
            novo.Columns.Add(coluna);

            DataRow myNewRow; 

            for(int i=0;i<resultados.Count;i++)
            {
               ArrayList posicaoi=(ArrayList)resultados[i];
               int tamanho=posicaoi.Count-1;
                double ivariavel=0;
                double totalpond=0;
                for(int j=0;j<tamanho-1;j++)
                {
                    int linha=(int)posicaoi[j+1];
                    double pondera=(double)tabela.Rows[linha][ponderador];
                    ivariavel+=(double)tabela.Rows[linha][var]*pondera;
                    totalpond+=pondera;
                }
                ivariavel/=totalpond;
                myNewRow = novo.NewRow();
                myNewRow[var] =ivariavel;
                novo.Rows.Add(myNewRow);
            }
        
        }

        public void agregacao_media_densidade_demografica(ref DataTable novo, DataTable tabela, string micro, string var, string populacao, string area)
        {
            ArrayList resultados = listaMicroCompleta(tabela, var);
            DataColumn coluna = new DataColumn(var);
            coluna.DataType = tabela.GetType();
            novo.Columns.Add(coluna);

            DataRow myNewRow;

            for (int i = 0; i < resultados.Count; i++)
            {
                ArrayList posicaoi = (ArrayList)resultados[i];
                int tamanho = posicaoi.Count - 1;
                double ivariavel = 0;
                double totalarea = 0;
                double totalpop = 0;
                for (int j = 0; j < tamanho - 1; j++)
                {
                    int linha = (int)posicaoi[j + 1];
                    double pondera1 = (double)tabela.Rows[linha][populacao];
                    double pondera2 = (double)tabela.Rows[linha][area];
                    double pondera = pondera1 / pondera2;
                    ivariavel += (double)tabela.Rows[linha][var] * pondera;
                    totalarea += pondera2;
                    totalpop += pondera1;
                }
                double densidadetotal = totalpop / totalarea;
                ivariavel /= densidadetotal;
                myNewRow = novo.NewRow();
                myNewRow[var] = ivariavel;
                novo.Rows.Add(myNewRow);
            }

        }







         






    }
}
