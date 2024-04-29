using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo
{
    public class clsUtilArquivos
    {
        public clsUtilArquivos()
        {
        }

        #region exportar tabela de dados

        public void ExportarTabela(DataTable tabela_dados)
        {
            ExportarTabela(tabela_dados, "Dados");
        }

        public void ExportarTabela(DataTable tabela_dados, string nome_tabela)
        {
            try
            {
                if (tabela_dados.Rows.Count > 0 && tabela_dados.Columns.Count > 0)
                {
                    clsUtilTools clt = new clsUtilTools();

                    DataTable dsTemp = tabela_dados;

                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.InitialDirectory = "C:\\";
                    saveFileDialog1.Filter = "Excel (*.xls)|*.xls|Access (*.mdb)|*.mdb|XML (*.xml)|*.xml|Texto (*.txt)|*.txt";
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.RestoreDirectory = true;
                    
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                        string strFile = saveFileDialog1.FileName;

                        ExportData exporta = new ExportData();

                        if (strExtensao == ".XLS")
                        {
                            //Cria datatable somente de strings
                            dsTemp = clt.DataTableNumberToTexto2(dsTemp);

                            exporta.exportToExcel(dsTemp, strFile);
                        }
                        else if (strExtensao == ".XML")
                        {
                            dsTemp.WriteXml(strFile);
                        }
                        else if (strExtensao == ".MDB")
                        {
                            //Cria o arquivo MDB
                            exporta.exportaToAccess(dsTemp, strFile, nome_tabela);
                        }
                        else if (strExtensao == ".TXT")
                        {
                            //Cria uma string para exportar
                            StreamWriter meustream = new StreamWriter(strFile);

                            for (int k = 0; k < dsTemp.Columns.Count; k++)
                            {
                                meustream.Write(dsTemp.Columns[k].ColumnName + "\t");
                            }
                            meustream.WriteLine();

                            for (int i = 0; i < dsTemp.Rows.Count; i++)
                            {
                                for (int j = 0; j < dsTemp.Columns.Count; j++)
                                {
                                    meustream.Write(dsTemp.Rows[i][j].ToString() + "\t");
                                }
                                meustream.WriteLine();
                            }

                            meustream.Close();
                        }
                        
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                {
                    MessageBox.Show("Tabela de dados está vazia");
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region importar arquivo shape

        public void ImportarArquivoShape(ref IpeaGeo.RegressoesEspaciais.clsIpeaShape m_shape, ref DataTable m_tabela_shape)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "ShapeFile (*.shp)|*.shp|All Files (*.*)|*.*";
                string FileName = "";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileName = openFileDialog.FileName;
                    Cursor.Current = Cursors.WaitCursor;

                    clsMapa m_mapa = new clsMapa();

                    m_mapa.LoadingMapa("mapa teste", FileName, true);

                    m_tabela_shape = m_mapa.TabelaDados;

                    SharpMap.Map m_sharp_mapa = m_mapa.Sharp_Mapa;

                    Cursor.Current = Cursors.Default;

                    if (m_mapa.VetorShapes.GetLength(0) > 0 && (m_mapa.VetorShapes)[0] != null)
                    {
                        m_shape = (m_mapa.VetorShapes)[0];
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void ImportarArquivoShape(ref clsIpeaShape m_shape, ref DataTable m_tabela_shape)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "ShapeFile (*.shp)|*.shp|All Files (*.*)|*.*";
                string FileName = "";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileName = openFileDialog.FileName;
                    Cursor.Current = Cursors.WaitCursor;

                    clsMapa m_mapa = new clsMapa();

                    m_mapa.LoadingMapa("mapa teste", FileName, true);

                    m_tabela_shape = m_mapa.TabelaDados;

                    SharpMap.Map m_sharp_mapa = m_mapa.Sharp_Mapa;

                    Cursor.Current = Cursors.Default;

                    if (m_mapa.VetorShapes.GetLength(0) > 0 && (m_mapa.VetorShapes)[0] != null)
                    {
                        m_shape = (m_mapa.VetorShapes)[0];
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region importar tabela de dados

        public bool ImportarTabelaDados(ref DataTable tabela_dados)
        {
            string nome_arquivo_importado = "";
            string path_arquivo_importado = "";

            return this.ImportarTabelaDados(ref tabela_dados, ref nome_arquivo_importado, ref path_arquivo_importado);
        }

        public bool ImportarTabelaDados(ref DataTable tabela_dados, ref string nome_arquivo_importado, ref string path_arquivo_importado)
        {
            try
            {
                FormAberturaTabelaDados frm = new FormAberturaTabelaDados();
                
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    tabela_dados = frm.TabelaDeDados;
                }
                
                if (tabela_dados.Rows.Count > 0 && tabela_dados.Columns.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}

