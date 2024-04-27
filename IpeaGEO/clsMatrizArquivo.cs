using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace IpeaGEO
{
    /*
    public enum ModoAcessoMatrizArquivo : int
    {
        Fechado = 0,
        Leitura = 1,
        Gravacao = 2
    };
    */
      
    public class MatrizArquivo
    {
        #region Variáveis internas

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

        /*
        private ModoAcessoMatrizArquivo m_modo_acesso = ModoAcessoMatrizArquivo.Gravacao;
        public ModoAcessoMatrizArquivo ModoAcessoArquivo
        {
            get 
            { 
                return this.m_modo_acesso; 
            }
            set 
            { 
                this.m_modo_acesso = value;

                if (this.m_modo_acesso == ModoAcessoMatrizArquivo.Fechado)
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
                    if (this.m_modo_acesso == ModoAcessoMatrizArquivo.Gravacao)
                    {
                        if (fileInput != null)
                            fileInput.Close();

                        binaryInput = null;
                        fileInput = null;

                        fileOutput = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                        binaryOutput = new BinaryWriter(fileOutput);
                    }
                    else
                    {
                        if (fileOutput != null)
                            fileOutput.Close();

                        binaryOutput = null;
                        fileOutput = null;

                        fileInput = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                        binaryInput = new BinaryReader(fileInput);
                    }
                }
            }
        }
        */
         
        #endregion 

        #region Construtores

        public MatrizArquivo()
            : this(10, 10)
        {
        }

        public MatrizArquivo(int nlinhas, int ncolunas)
        {
            fileName = "MatrizArquivoTemporario.dat";

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

            /*
            for (int i = 0; i < this.m_numero_linhas; i++)
            {
                for (int j = 0; j < this.m_numero_colunas; j++)
                {
                    fileOutput.Position = (j + i * this.m_numero_colunas) * this.size_of_double;
                    binaryOutput.Write(0.0);
                }
            }
             */
        }

        #endregion 

        #region Acessando elementos da matriz

        public double this[int i, int j]
        {
            get
            {
                if (i < 0 || i >= this.m_numero_linhas || j < 0 || j >= this.m_numero_colunas)
                    throw new Exception("Índices fora das dimensões da matriz");

                //this.fileInput.Seek((j + i * this.m_numero_colunas) * this.size_of_double, SeekOrigin.Begin);
                
                this.fileOutput.Seek((j + i * this.m_numero_colunas) * this.size_of_double, SeekOrigin.Begin);
                return binaryInput.ReadDouble();
            }
            set
            {
                if (i < 0 || i >= this.m_numero_linhas || j < 0 || j >= this.m_numero_colunas)
                    throw new Exception("Índices fora das dimensões da matriz");

                this.fileOutput.Seek((j + i * this.m_numero_colunas) * this.size_of_double, SeekOrigin.Begin);
                this.binaryOutput.Write(value);
            }
        }

        #endregion 

        #region Destrutor

        public void DisposeMatriz()
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

        #endregion 
    }
}
