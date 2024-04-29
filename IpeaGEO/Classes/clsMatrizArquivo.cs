using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace IpeaGeo
{
    public class MatrizArquivo
    {
        #region Variáveis internas

        private int m_num_linhas_corte = 1000;
        private int m_num_cols_corte = 1000;

        private double[,] m_valores = new double[0, 0];

        private int m_numero_linhas = 10;
        private int m_numero_colunas = 10;
        private int m_numero_registros = 100;
        private int size_of_double = 8;

        private FileStream fileOutput = null;
        private FileStream fileInput = null;
        private BinaryWriter binaryOutput = null;
        private BinaryReader binaryInput = null;
        private string fileName = "";

        public int GetLength(int d)
        {
            if (d == 0) return this.m_numero_linhas;
            else return this.m_numero_colunas;
        }
         
        #endregion 

        #region Limpando o diretório dos arquivos temporários de matrizes

        public static void LimpaMatrizesArquivosTemporarios()
        {
            string diretorio_nome = Directory.GetCurrentDirectory();
            string[] arquivos = Directory.GetFiles(diretorio_nome);

            for (int i = 0; i < arquivos.GetLength(0); i++)
            {
                arquivos[i] = arquivos[i].Replace(diretorio_nome + "\\", "");
                arquivos[i] = arquivos[i].Replace(diretorio_nome, "");
            }

            ArrayList files = new ArrayList();
            string teste1 = "";
            string teste2 = "";
            for (int i = 0; i < arquivos.GetLength(0); i++)
            {
                if (arquivos[i].Length > 24)
                {
                    teste1 = arquivos[i].Substring(0, 24);
                    teste2 = arquivos[i].Substring(arquivos[i].Length - 4, 4);
                    if (teste1 == "MatrizArquivoTemporario_")
                    {
                        if (teste2.ToUpper() == ".DAT")
                        {
                            files.Add(arquivos[i]);
                        }
                    }
                }
            }

            for (int i = 0; i < files.Count; i++)
            {
                FileInfo atual = new FileInfo(files[i].ToString());
                atual.Delete();
            }
        }

        #endregion

        #region Construtores

        public MatrizArquivo()
            : this(10, 10)
        {
        }

        public MatrizArquivo(int nlinhas, int ncolunas)
        {
            if (nlinhas > m_num_linhas_corte || ncolunas > m_num_cols_corte)
            {
                this.m_valores = new double[Math.Min(nlinhas, m_num_linhas_corte), Math.Min(ncolunas, m_num_cols_corte)];

                int dia = DateTime.Now.Day;
                int ano = DateTime.Now.Year;
                int mes = DateTime.Now.Month;
                int sec = DateTime.Now.Second;
                int hour = DateTime.Now.Hour;
                int min = DateTime.Now.Minute;

                string sdia = dia > 9 ? dia.ToString() : ("0" + dia.ToString()).Trim();
                string smes = mes > 9 ? mes.ToString() : ("0" + mes.ToString()).Trim();
                string sano = ano.ToString();
                string ssec = sec > 9 ? sec.ToString() : ("0" + sec.ToString()).Trim();
                string shour = hour > 9 ? hour.ToString() : ("0" + hour.ToString()).Trim();
                string smin = min > 9 ? min.ToString() : ("0" + min.ToString()).Trim();

                string extensao = sano + smes + sdia + shour + smin + ssec;

                fileName = "MatrizArquivoTemporario_" + extensao + ".dat";

                FileInfo teste = new FileInfo(fileName);
                if (teste.Exists)
                    teste.Delete();

                this.m_numero_colunas = ncolunas;
                this.m_numero_linhas = nlinhas;
                this.m_numero_registros = nlinhas * ncolunas;

                fileOutput = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                fileOutput.SetLength(this.size_of_double * this.m_numero_registros);
                binaryOutput = new BinaryWriter(fileOutput);
                binaryInput = new BinaryReader(fileOutput);
            }
            else
            {
                this.m_numero_colunas = ncolunas;
                this.m_numero_linhas = nlinhas;
                this.m_numero_registros = nlinhas * ncolunas;

                this.m_valores = new double[nlinhas, ncolunas];
            }
        }

        #endregion 

        #region Acessando elementos da matriz

        public double this[int i, int j]
        {
            get
            {
                if (i < 0 || i >= this.m_numero_linhas || j < 0 || j >= this.m_numero_colunas)
                    throw new Exception("Índices fora das dimensões da matriz");

                if (i >= m_num_linhas_corte || j >= m_num_cols_corte)
                {
                    this.fileOutput.Seek((j + i * this.m_numero_colunas) * this.size_of_double, SeekOrigin.Begin);
                    return binaryInput.ReadDouble();
                }
                else
                {
                    return m_valores[i, j];
                }
            }
            set
            {
                if (i < 0 || i >= this.m_numero_linhas || j < 0 || j >= this.m_numero_colunas)
                    throw new Exception("Índices fora das dimensões da matriz");

                if (i >= m_num_linhas_corte || j >= m_num_cols_corte)
                {
                    this.fileOutput.Seek((j + i * this.m_numero_colunas) * this.size_of_double, SeekOrigin.Begin);
                    this.binaryOutput.Write(value);
                }
                else
                {
                    this.m_valores[i, j] = value;
                }
            }
        }

        #endregion 

        #region Destrutor

        public void DisposeMatriz()
        {
            if (m_numero_linhas > m_num_linhas_corte || m_numero_colunas > m_num_cols_corte)
            {
                if (fileInput != null)
                    fileInput.Close();

                binaryInput = null;
                fileInput = null;

                if (fileOutput != null)
                    fileOutput.Close();

                binaryOutput = null;
                fileOutput = null;

                FileInfo arq = new FileInfo(this.fileName);
                arq.Delete();
            }
            else
            {
                m_valores = new double[0, 0];
            }
        }

        #endregion 
    }
}

