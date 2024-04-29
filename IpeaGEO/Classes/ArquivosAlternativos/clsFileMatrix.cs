using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IpeaGeo
{
    /*
    Esta classe corresponde a uma modificação da classe MatrizArquivo, com algumas alterações para possíveis ganhos de eficiência. 
    */

    public class FileMatrix
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

        private double[,] m_small_matrix = new double[10, 10];
        private int m_maximo_elementos = 250000;

        #endregion

        #region Construtores

        public FileMatrix()
            : this(10, 10)
        {
        }

        public FileMatrix(int nlinhas, int ncolunas)
        {
            m_numero_registros = nlinhas * ncolunas;
            m_numero_colunas = ncolunas;
            m_numero_linhas = nlinhas;

            if (nlinhas * ncolunas > m_maximo_elementos)
            {
                fileName = "FileMatrixTemporario.dat";

                FileInfo teste = new FileInfo(fileName);
                if (teste.Exists)
                    teste.Delete();

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
            else
            {
                m_small_matrix = new double[nlinhas, ncolunas];
            }
        }

        #endregion

        #region Acessando elementos da matriz

        public double this[int i, int j]
        {
            get
            {
                if (m_numero_registros > m_maximo_elementos)
                {
                    if (i < 0 || i >= this.m_numero_linhas || j < 0 || j >= this.m_numero_colunas)
                        throw new Exception("Índices fora das dimensões da matriz");

                    //this.fileInput.Seek((j + i * this.m_numero_colunas) * this.size_of_double, SeekOrigin.Begin);

                    this.fileOutput.Seek((j + i * this.m_numero_colunas) * this.size_of_double, SeekOrigin.Begin);
                    return binaryInput.ReadDouble();
                }
                else
                {
                    return m_small_matrix[i, j];
                }
            }
            set
            {
                if (m_numero_registros > m_maximo_elementos)
                {
                    if (i < 0 || i >= this.m_numero_linhas || j < 0 || j >= this.m_numero_colunas)
                        throw new Exception("Índices fora das dimensões da matriz");

                    this.fileOutput.Seek((j + i * this.m_numero_colunas) * this.size_of_double, SeekOrigin.Begin);
                    this.binaryOutput.Write(value);
                }
                else
                {
                    m_small_matrix[i, j] = value;
                }
            }
        }

        #endregion

        #region Destrutor

        public void DisposeMatriz()
        {
            if (m_numero_registros > m_maximo_elementos)
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
                this.m_small_matrix = null; 
            }
        }

        #endregion
    }
}

