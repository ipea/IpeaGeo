using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace IpeaGeo.RegressoesEspaciais
{
    public class BLAreasMinimasComparaveis
    {
        public BLAreasMinimasComparaveis()
        {
        }

        #region variáveis internas

        private string VariavelNameAmcsDtAmcs(int ano_amcs)
        {
            if (ano_amcs == 1970) return "nomeamc70";
            if (ano_amcs == 1991) return "nomeamc91";
            if (ano_amcs == 1997) return "nomeamc97";

            return "nomeamc70";
        }

        private string VariavelIdAmcsDtAmcs(int ano_amcs)
        {
            if (ano_amcs == 1970) return "codamc70";
            if (ano_amcs == 1991) return "codamc91";
            if (ano_amcs == 1997) return "codamc97";

            return "codamc70";
        }

        private DataTable m_dt_dados = new DataTable();
        private DataTable m_dt_amcs = new DataTable();

        private string m_id_mun_dt_amcs = "";
        private string m_id_mun_dt_dados = "";
        private string m_id_amcs = "";
        private string m_nomes_amcs = "";
        private string m_nome_variavel_peso = "";

        public string VariavelPeso
        {
            set { m_nome_variavel_peso = value; }
        }

        public string NomesAmcs
        {
            set { m_nomes_amcs = value; }
        }

        public DataTable DtDados
        {
            set { m_dt_dados = value; }
        }

        public DataTable DtAmcs
        {
            set { m_dt_amcs = value; }
        }

        public string IdMunDtAmcs
        {
            set { m_id_mun_dt_amcs = value; }
        }

        public string IdMunDtDados
        {
            set { m_id_mun_dt_dados = value; }
        }

        public string IdAmcs
        {
            set { m_id_amcs = value; }
        }

        private ArrayList m_variaveis_na_tabela = new ArrayList();
        public ArrayList VariaveisNaTabela
        {
            set { m_variaveis_na_tabela = value; }
        }

        private string VariavelPopulacao(int ano_municipio)
        {
            if (ano_municipio == 1970) return "popmun70";
            if (ano_municipio == 1980) return "popmun80";
            if (ano_municipio == 1991) return "popmun91";
            if (ano_municipio == 1993) return "popmun93";
            if (ano_municipio == 1997) return "popmun97";
            if (ano_municipio == 1998) return "popmun98";
            if (ano_municipio == 1999) return "popmun99";
            if (ano_municipio == 2000) return "popmun00";
            if (ano_municipio == 2001) return "popmun01";
            if (ano_municipio == 2002) return "popmun02";
            if (ano_municipio == 2003) return "popmun03";
            if (ano_municipio == 2004) return "popmun04";
            if (ano_municipio == 2005) return "popmun05";
            if (ano_municipio == 2006) return "popmun06";
            if (ano_municipio == 2007) return "popmun07";
            if (ano_municipio == 2008) return "popmun08";
            if (ano_municipio == 2009) return "popmun09";
            if (ano_municipio == 2010) return "popmun10";

            return "popmun70";
        }

        #endregion

        #region joinning data tables

        public void JoinDataTables(ref DataTable dt_res)
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("idmun1_", typeof(string));
            dt1.Columns.Add(m_id_amcs, typeof(string));
            dt1.Columns.Add(m_nomes_amcs, typeof(string));
            object[] itens = new object[3];
            for (int i = 0; i < this.m_dt_amcs.Rows.Count; i++)
            {
                itens[0] = m_dt_amcs.Rows[i][m_id_mun_dt_amcs].ToString().Trim().Substring(0, 6);
                itens[1] = m_dt_amcs.Rows[i][m_id_amcs].ToString();
                itens[2] = m_dt_amcs.Rows[i][m_nomes_amcs].ToString();
                dt1.Rows.Add(itens);
            }
            dt1.DefaultView.Sort = "[" + "idmun1_" + "] asc";
            dt1 = dt1.DefaultView.ToTable();

            DataTable dt2 = m_dt_dados.Copy();
            for (int j = 0; j < dt2.Columns.Count; j++)
            {
                if (dt2.Columns[j].ColumnName != m_id_mun_dt_dados && !m_variaveis_na_tabela.Contains(dt2.Columns[j].ColumnName))
                {
                    dt2.Columns.Remove(dt2.Columns[j].ColumnName);
                }
            }

            dt2.Columns.Add("idmun2_", typeof(string));
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                dt2.Rows[i]["idmun2_"] = dt2.Rows[i][m_id_mun_dt_dados].ToString().Trim().Substring(0, 6);
            }
            dt2.DefaultView.Sort = "[" + "idmun2_" + "] asc";
            dt2 = dt2.DefaultView.ToTable();

            #region checando duplicidade dos ids dos municípios na tabela de dados

            string id_foco = dt2.Rows[0]["idmun2_"].ToString();
            for (int i = 1; i < dt2.Rows.Count; i++)
            {
                if (id_foco == dt2.Rows[i]["idmun2_"].ToString())
                {
                    throw new Exception("Há duplicidade na variável id do município na tabela de dados. O valor duplicado é '" + id_foco + "'. " 
                        + "Escolha uma outra variável ou verifique a sua base de dados.");
                }
                else
                {
                    id_foco = dt2.Rows[i]["idmun2_"].ToString();
                }
            }

            #endregion

            int[,] associacao = new int[dt1.Rows.Count, 2];
            int foco = 0;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                associacao[i, 0] = i;
                associacao[i, 1] = -1;
                for (int j = foco; j < dt2.Rows.Count; j++)
                {
                    if (dt1.Rows[i]["idmun1_"].ToString() == dt2.Rows[j]["idmun2_"].ToString())
                    {
                        associacao[i, 1] = j;
                        foco = j + 1;
                        break;
                    }
                }
            }

            dt_res = dt1.Copy();

            if (dt2.Columns.Contains(m_id_amcs)) dt2.Columns.Remove(m_id_amcs);
            if (dt2.Columns.Contains(m_nomes_amcs)) dt2.Columns.Remove(m_nomes_amcs);

            for (int j = 0; j < dt2.Columns.Count; j++)
            {
                dt_res.Columns.Add(dt2.Columns[j].ColumnName, dt2.Columns[j].DataType);
            }

            for (int i = 0; i < dt_res.Rows.Count; i++)
            {
                if (associacao[i, 1] >= 0)
                {
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        dt_res.Rows[i][dt2.Columns[j].ColumnName] = dt2.Rows[associacao[i, 1]][dt2.Columns[j].ColumnName];
                    }
                }
            }

            dt_res.Columns["idmun1_"].ColumnName = "IDMUN_AMC_";
            dt_res.Columns["idmun2_"].ColumnName = "IDMUN_DADOS_";
        }

        #endregion

        public void GeraVariavelPorAMCs(DataTable dt_dados, DataTable dt_amcs, string variavel_dados,
            int ano_municipio, int ano_amcs, string id_mun_dt_dados, string id_mun_dt_amcs, string tipo_agregacao,
            string variavel_ponderacao,
            out DataTable dt_resultado, out DataTable dt_concatenada, out int num_pares_concatenados, ref ProgressBar prBar)
        {
            #region montando a tabela de dados com variáveis selecionadas

            DataTable dt_aux_dados = dt_dados.Copy();
            for (int i = 0; i < dt_dados.Columns.Count; i++)
            {
                if (dt_dados.Columns[i].ColumnName != variavel_ponderacao
                    && dt_dados.Columns[i].Caption != variavel_dados
                    && dt_dados.Columns[i].ColumnName != variavel_dados
                    && dt_dados.Columns[i].ColumnName != id_mun_dt_dados)
                {
                    dt_aux_dados.Columns.Remove(dt_dados.Columns[i].ColumnName);
                }
            }
            dt_aux_dados.Columns.Add("id_mun1__", typeof(string));
            if (dt_aux_dados.Columns.Contains(variavel_ponderacao))
            {
                dt_aux_dados.Columns.Add("peso_ponderacao__", typeof(double));
            }

            for (int i = 0; i < dt_aux_dados.Rows.Count; i++)
            {
                if (ano_municipio > 1970) 
                {
                    if (dt_aux_dados.Rows[i][id_mun_dt_dados].ToString().Length >= 6)
                    {
                        dt_aux_dados.Rows[i]["id_mun1__"] = dt_aux_dados.Rows[i][id_mun_dt_dados].ToString().Substring(0, 6);
                    }
                    else
                    {
                        dt_aux_dados.Rows[i]["id_mun1__"] = dt_aux_dados.Rows[i][id_mun_dt_dados].ToString();
                    }
                }
                else
                {
                    if (dt_aux_dados.Rows[i][id_mun_dt_dados].ToString().Length >= 5)
                    {
                        dt_aux_dados.Rows[i]["id_mun1__"] = dt_aux_dados.Rows[i][id_mun_dt_dados].ToString().Substring(0, 5);
                    }
                    else
                    {
                        dt_aux_dados.Rows[i]["id_mun1__"] = dt_aux_dados.Rows[i][id_mun_dt_dados].ToString();
                    }
                }
                if (dt_aux_dados.Columns.Contains(variavel_ponderacao))
                {
                    if (dt_aux_dados.Rows[i][variavel_ponderacao] is DBNull)
                    {
                        dt_aux_dados.Rows[i]["peso_ponderacao__"] = 0.0;
                    }
                    else
                    {
                        dt_aux_dados.Rows[i]["peso_ponderacao__"] = Convert.ToDouble(dt_aux_dados.Rows[i][variavel_ponderacao]);
                    }
                }
            }

            dt_aux_dados.DefaultView.Sort = "[" + "id_mun1__" + "] asc";
            dt_aux_dados = dt_aux_dados.DefaultView.ToTable();

            #region checando duplicação de ID's dos municípios

            string id_anterior = dt_aux_dados.Rows[0]["id_mun1__"].ToString();
            for (int i = 1; i < dt_aux_dados.Rows.Count; i++)
            {
                if (dt_aux_dados.Rows[i]["id_mun1__"].ToString() != id_anterior)
                {
                    id_anterior = dt_aux_dados.Rows[i]["id_mun1__"].ToString();
                }
                else
                {
                    throw new Exception("Há duplicação de identificadores para os municípios na tabela de dados. Cheque a sua tabela de dados e remova as duplicações.");
                }                
            }

            #endregion

            #endregion

            #region montando a tabela de dados com o depara das AMC's

            DataTable dt_aux_amcs = dt_amcs.Copy();
            dt_aux_amcs.Columns.Add("id_mun2__", typeof(string));

            if (variavel_ponderacao == "População do município")
            {
                dt_aux_amcs.Columns.Add("peso_ponderacao__", typeof(double));
                string var_pop = VariavelPopulacao(ano_municipio);
                for (int i = 0; i < dt_amcs.Rows.Count; i++)
                {
                    if (!(dt_amcs.Rows[i][var_pop] is DBNull))
                    {
                        dt_aux_amcs.Rows[i]["peso_ponderacao__"] = Convert.ToDouble(dt_amcs.Rows[i][var_pop]);
                    }
                    else
                    {
                        dt_aux_amcs.Rows[i]["peso_ponderacao__"] = 0.0;
                    }
                }
            }

            for (int i = 0; i < dt_aux_amcs.Rows.Count; i++)
            {
                if (ano_municipio > 1970)
                {
                    if (dt_aux_amcs.Rows[i][id_mun_dt_amcs].ToString().Length >= 6)
                    {
                        dt_aux_amcs.Rows[i]["id_mun2__"] = dt_aux_amcs.Rows[i][id_mun_dt_amcs].ToString().Substring(0, 6);
                    }
                    else
                    {
                        dt_aux_amcs.Rows[i]["id_mun2__"] = dt_aux_amcs.Rows[i][id_mun_dt_amcs].ToString();
                    }
                }
                else
                {
                    if (dt_aux_amcs.Rows[i][id_mun_dt_amcs].ToString().Length >= 5)
                    {
                        dt_aux_amcs.Rows[i]["id_mun2__"] = dt_aux_amcs.Rows[i][id_mun_dt_amcs].ToString().Substring(0, 5);
                    }
                    else
                    {
                        dt_aux_amcs.Rows[i]["id_mun2__"] = dt_aux_amcs.Rows[i][id_mun_dt_amcs].ToString();
                    }
                }
            }

            dt_aux_amcs.DefaultView.Sort = "[" + "id_mun2__" + "] asc";
            dt_aux_amcs = dt_aux_amcs.DefaultView.ToTable();

            #endregion

            #region montando a tabela concatenada

            DataTable dt_concat = dt_aux_amcs.Copy();

            for (int i = 0; i < dt_aux_dados.Columns.Count; i++)
            {
                if (dt_concat.Columns.Contains(dt_aux_dados.Columns[i].ColumnName))
                {
                    dt_aux_dados.Columns[i].ColumnName = dt_aux_dados.Columns[i].ColumnName.TrimEnd() + "_";
                    dt_concat.Columns.Add(dt_aux_dados.Columns[i].ColumnName, dt_aux_dados.Columns[i].DataType);
                }
                else
                {
                    dt_concat.Columns.Add(dt_aux_dados.Columns[i].ColumnName, dt_aux_dados.Columns[i].DataType);
                }
            }

            num_pares_concatenados = 0;

            string id_foco = "";
            int row_dados = 0;
            for (int i = 0; i < dt_aux_amcs.Rows.Count; i++)
            {
                id_foco = dt_aux_amcs.Rows[i]["id_mun2__"].ToString();
                for (int j = row_dados; j < dt_aux_dados.Rows.Count; j++)
                {
                    if (id_foco == dt_aux_dados.Rows[j]["id_mun1__"].ToString())
                    {
                        for (int k = 0; k < dt_aux_dados.Columns.Count; k++)
                        {
                            if (!(dt_aux_dados.Rows[j][k] is DBNull))
                            {
                                dt_concat.Rows[i][dt_aux_dados.Columns[k].ColumnName] = dt_aux_dados.Rows[j][k];
                            }
                        }
                        num_pares_concatenados++;

                        row_dados = j;
                        break;
                    }
                }
            }

            #endregion

            //string[] tipos_agregacao = new string[] { "Soma", "Média simples", 
            //    "Média ponderada", "Máximo", "Mínimo" };
            
            string[] tipos_agregacao = new string[] { "Soma", "Média simples", 
                "Média ponderada", "Máximo", "Mínimo", "Primeiro ordem dos dados", "Último ordem dos dados", "Primeiro ordem alfabética", 
                "Último ordem alfabética", "Primeiro ordem ponderação", "Último ordem ponderação" };

            string var_id_amc = VariavelIdAmcsDtAmcs(ano_amcs);
            string var_nome_amc = VariavelNameAmcsDtAmcs(ano_amcs);

            string comando_sorting = "[" + var_id_amc + "] asc";

            dt_concat.DefaultView.Sort = comando_sorting;
            dt_concat = dt_concat.DefaultView.ToTable();

            object[,] codes = new object[dt_concat.Rows.Count, 1];
            ArrayList vcodes = new ArrayList();
            for (int i = 0; i<codes.GetLength(0); i++)
            {
                codes[i,0] = dt_concat.Rows[i][var_id_amc].ToString();
                if (!vcodes.Contains(codes[i, 0].ToString()))
                {
                    vcodes.Add(codes[i, 0].ToString());
                }
            }

            vcodes.Sort();
            codes = new object[vcodes.Count, 1];
            for (int i = 0; i < vcodes.Count; i++)
            {
                codes[i, 0] = vcodes[i].ToString();
            }

            clsUtilTools clt = new clsUtilTools();

            //clt.FrequencyTable(ref codes, codes);
            //codes = clt.SubColumnArrayObject(codes, 0);
            
            DataTable dt_final = new DataTable();
            object[] itens = new object[1];
            dt_final.Columns.Add(var_id_amc, typeof(string));
            for (int i = 0; i < codes.GetLength(0); i++)
            {
                itens[0] = codes[i,0].ToString();
                dt_final.Rows.Add(itens);
            }

            dt_final.Columns.Add(var_nome_amc, typeof(string));
            dt_final.Columns.Add("UF_amc_", typeof(string));

            int nobs = 0;
            double soma = 0.0;
            double soma_ponderada = 0.0;
            double min = 0.0;
            double max = 0.0;
            row_dados = 0;

            if ((dt_dados.Columns[variavel_dados].DataType == typeof(double)
                || dt_dados.Columns[variavel_dados].DataType == typeof(decimal)
                || dt_dados.Columns[variavel_dados].DataType == typeof(Decimal)
                || dt_dados.Columns[variavel_dados].DataType == typeof(float)
                || dt_dados.Columns[variavel_dados].DataType == typeof(uint)
                || dt_dados.Columns[variavel_dados].DataType == typeof(Double)
                || dt_dados.Columns[variavel_dados].DataType == typeof(int)
                || dt_dados.Columns[variavel_dados].DataType == typeof(Int16)
                || dt_dados.Columns[variavel_dados].DataType == typeof(Int32)
                || dt_dados.Columns[variavel_dados].DataType == typeof(Int64))
                && (tipo_agregacao == "Soma" || tipo_agregacao == "Média simples"
                    || tipo_agregacao == "Média ponderada" || tipo_agregacao == "Máximo" || tipo_agregacao == "Mínimo"))
            {
                #region executando a agregação para variáveis numéricas

                dt_final.Columns.Add(variavel_dados, typeof(double));

                for (int i = 0; i < dt_final.Rows.Count; i++)
                {
                    prBar.Increment(1);
                    Application.DoEvents();

                    nobs = 0;
                    soma = 0.0;
                    soma_ponderada = 0.0;
                    min = Double.PositiveInfinity;
                    max = Double.NegativeInfinity;

                    id_foco = dt_final.Rows[i][var_id_amc].ToString();
                    for (int j = row_dados; j < dt_concat.Rows.Count; j++)
                    {
                        dt_final.Rows[i]["UF_amc_"] = dt_concat.Rows[j]["UF"].ToString();
                        dt_final.Rows[i][var_nome_amc] = dt_concat.Rows[j][var_nome_amc].ToString();

                        if (id_foco == dt_concat.Rows[j][var_id_amc].ToString())
                        {
                            if (!(dt_concat.Rows[j][variavel_dados] is DBNull))
                            {
                                if (dt_concat.Rows[j]["peso_ponderacao__"] is DBNull)
                                {
                                    dt_concat.Rows[j]["peso_ponderacao__"] = 0.0;
                                }

                                if (max < Convert.ToDouble(dt_concat.Rows[j][variavel_dados]))
                                {
                                    max = Convert.ToDouble(dt_concat.Rows[j][variavel_dados]);
                                }

                                if (min > Convert.ToDouble(dt_concat.Rows[j][variavel_dados]))
                                {
                                    min = Convert.ToDouble(dt_concat.Rows[j][variavel_dados]);
                                }

                                soma_ponderada += Convert.ToDouble(dt_concat.Rows[j][variavel_dados]) * Convert.ToDouble(dt_concat.Rows[j]["peso_ponderacao__"]);
                                soma += Convert.ToDouble(dt_concat.Rows[j][variavel_dados]);
                                nobs++;
                            }
                        }
                        else
                        {
                            row_dados = j;
                            break;
                        }
                    }
                    if (nobs > 0)
                    {
                        if (tipo_agregacao == "Soma") dt_final.Rows[i][variavel_dados] = soma;
                        if (tipo_agregacao == "Média") dt_final.Rows[i][variavel_dados] = soma / ((double)nobs);
                        if (tipo_agregacao == "Média ponderada") dt_final.Rows[i][variavel_dados] = soma_ponderada / ((double)nobs);
                        if (tipo_agregacao == "Mínimo") dt_final.Rows[i][variavel_dados] = min;
                        if (tipo_agregacao == "Máximo") dt_final.Rows[i][variavel_dados] = max;
                    }
                }

                #endregion
            }
            else
            {
                #region executando a agregação para variáveis não numéricas

                dt_final.Columns.Add(variavel_dados, dt_dados.Columns[variavel_dados].DataType);

                double peso_ponderacao_maximo = 0.0, peso_ponderacao_minimo = 0.0;
                string ultimo_valor = "", primeiro_valor = "";
                object valor_selecionado = new object();

                for (int i = 0; i < dt_final.Rows.Count; i++)
                {
                    prBar.Increment(1);
                    Application.DoEvents();

                    nobs = 0;
                    peso_ponderacao_maximo = double.NegativeInfinity;
                    peso_ponderacao_minimo = double.PositiveInfinity;

                    id_foco = dt_final.Rows[i][var_id_amc].ToString();
                    for (int j = row_dados; j < dt_concat.Rows.Count; j++)
                    {
                        dt_final.Rows[i]["UF_amc_"] = dt_concat.Rows[j]["UF"].ToString();
                        dt_final.Rows[i][var_nome_amc] = dt_concat.Rows[j][var_nome_amc].ToString();

                        if (id_foco == dt_concat.Rows[j][var_id_amc].ToString())
                        {
                            if (!(dt_concat.Rows[j][variavel_dados] is DBNull))
                            {
                                if (dt_concat.Rows[j]["peso_ponderacao__"] is DBNull)
                                {
                                    dt_concat.Rows[j]["peso_ponderacao__"] = 0.0;
                                }

                                if (tipo_agregacao == "Primeiro ordem dos dados")
                                {
                                    if (nobs <= 0)
                                    {
                                        valor_selecionado = dt_concat.Rows[j][variavel_dados];
                                    }
                                }

                                if (tipo_agregacao == "Último ordem dos dados")
                                {
                                    valor_selecionado = dt_concat.Rows[j][variavel_dados];
                                }

                                if (tipo_agregacao == "Primeiro ordem alfabética")
                                {
                                    if (nobs <= 0)
                                    {
                                        primeiro_valor = dt_concat.Rows[j][variavel_dados].ToString();
                                        valor_selecionado = dt_concat.Rows[j][variavel_dados];
                                    }
                                    else
                                    {
                                        if (primeiro_valor.CompareTo(dt_concat.Rows[j][variavel_dados].ToString()) < 0)
                                        {
                                            primeiro_valor = dt_concat.Rows[j][variavel_dados].ToString();
                                            valor_selecionado = dt_concat.Rows[j][variavel_dados];
                                        }
                                    }
                                }

                                if (tipo_agregacao == "Último ordem alfabética")
                                {
                                    if (nobs <= 0)
                                    {
                                        ultimo_valor = dt_concat.Rows[j][variavel_dados].ToString();
                                        valor_selecionado = dt_concat.Rows[j][variavel_dados];
                                    }
                                    else
                                    {
                                        if (ultimo_valor.CompareTo(dt_concat.Rows[j][variavel_dados].ToString()) > 0)
                                        {
                                            ultimo_valor = dt_concat.Rows[j][variavel_dados].ToString();
                                            valor_selecionado = dt_concat.Rows[j][variavel_dados];
                                        }
                                    }
                                }

                                if (tipo_agregacao == "Primeiro ordem ponderação")
                                {
                                    if (nobs <= 0)
                                    {
                                        peso_ponderacao_minimo = Convert.ToDouble(dt_concat.Rows[j]["peso_ponderacao__"]);
                                        valor_selecionado = dt_concat.Rows[j][variavel_dados];
                                    }
                                    else
                                    {
                                        if (peso_ponderacao_minimo > Convert.ToDouble(dt_concat.Rows[j]["peso_ponderacao__"]))
                                        {
                                            peso_ponderacao_minimo = Convert.ToDouble(dt_concat.Rows[j]["peso_ponderacao__"]);
                                            valor_selecionado = dt_concat.Rows[j][variavel_dados];
                                        }
                                    }
                                }

                                if (tipo_agregacao == "Último ordem ponderação")
                                {
                                    if (nobs <= 0)
                                    {
                                        peso_ponderacao_maximo = Convert.ToDouble(dt_concat.Rows[j]["peso_ponderacao__"]);
                                        valor_selecionado = dt_concat.Rows[j][variavel_dados];
                                    }
                                    else
                                    {
                                        if (peso_ponderacao_maximo > Convert.ToDouble(dt_concat.Rows[j]["peso_ponderacao__"]))
                                        {
                                            peso_ponderacao_maximo = Convert.ToDouble(dt_concat.Rows[j]["peso_ponderacao__"]);
                                            valor_selecionado = dt_concat.Rows[j][variavel_dados];
                                        }
                                    }
                                }

                                nobs++;
                            }
                        }
                        else
                        {
                            row_dados = j;
                            break;
                        }
                    }
                    if (nobs > 0)
                    {
                        dt_final.Rows[i][variavel_dados] = valor_selecionado;
                    }
                }

                #endregion
            }

            dt_resultado = new DataTable();
            dt_resultado = dt_final;

            dt_concatenada = dt_concat;

            dt_concatenada.DefaultView.Sort = "[" + id_mun_dt_dados + "] asc";
            dt_concatenada = dt_concatenada.DefaultView.ToTable();
            dt_concatenada.Columns.Remove("id_mun1__");
            dt_concatenada.Columns.Remove("id_mun2__");
            dt_concatenada.Columns.Remove("peso_ponderacao__");
        }
    }
}
