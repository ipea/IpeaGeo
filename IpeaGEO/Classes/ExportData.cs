using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using ADOX;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using IpeaGeo.RegressoesEspaciais;

namespace IpeaGeo
{
    class ExportData
    {
        public void ExportaToExcel_Novo(DataTable ds, string strFile)
        {
            try
            {
                clsExportExcel objExport = new clsExportExcel("Win");
                objExport.ExportDetails(ds, clsExportExcel.ExportFormat.Excel, strFile);
            }
            catch
            {
            }
        }

        private IpeaGeo.RegressoesEspaciais.clsUtilTools m_cltools = new IpeaGeo.RegressoesEspaciais.clsUtilTools();

        #region exportar tabela de dados

        private void ExportarTabelaDadosToExcel(DataGridView dataGridView1, string strFile)
        {
            try
            {
                if (((DataTable)dataGridView1.DataSource).Columns.Count > 0 && ((DataTable)dataGridView1.DataSource).Rows.Count > 0)
                {
                    DataTable dsTemp = (DataTable)dataGridView1.DataSource;
                    //dsTemp.Tables[0].Columns.Remove("Mapa"+strIDmapa);
                    //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    //saveFileDialog1.InitialDirectory = "C:\\";
                    //saveFileDialog1.Filter = "Excel (*.xls)|*.xls|Access (*.mdb)|*.mdb|XML (*.xml)|*.xml|Texto (*.txt)|*.txt";
                    //saveFileDialog1.FilterIndex = 1;
                    //saveFileDialog1.RestoreDirectory = true;
                    //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        //string strFile = saveFileDialog1.FileName;
                        //ExportData obExpData = new ExportData();
                        this.ExportaToExcel_Novo(dsTemp, strFile);
                    }
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static bool fechar;
        public bool fechamento = fechar;
        public void ExportarDados(DataGridView dataGridView1, string Name)
        {
            try
            {
                if (((DataTable)dataGridView1.DataSource).Columns.Count > 0 && ((DataTable)dataGridView1.DataSource).Rows.Count > 0)
                {
                    frmMapa mapa = new frmMapa();
                    DataTable dsTemp = (DataTable)dataGridView1.DataSource;
                    //dsTemp.Tables[0].Columns.Remove("Mapa"+strIDmapa);
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.InitialDirectory = "C:\\";
                    if (mapa.salvamento_efetuado == true)
                    {
                        saveFileDialog1.Filter = "Texto (*.txt)|*.txt"/*|Excel (*.xls)|*.xls"*/;
                    }
                    else
                    {
                        saveFileDialog1.Filter = "Excel (*.csv)|*.csv|Access (*.mdb)|*.mdb|XML (*.xml)|*.xml|Texto (*.txt)|*.txt";
                    }
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        fechar = fechamento = true;
                        Cursor.Current = Cursors.WaitCursor;

                        string strExtensao = Path.GetExtension(saveFileDialog1.FileName).ToUpper();
                        string strFile = saveFileDialog1.FileName;

                        //ExportData exporta = new ExportData();

                        if (strExtensao == ".CSV")
                        {
                            try
                            {
                                ExportarTabelaDadosToExcel(dataGridView1, strFile);
                            }
                            catch
                            {
                                //Cria datatable somente de strings
                                IpeaGeo.RegressoesEspaciais.clsUtilTools clt = new IpeaGeo.RegressoesEspaciais.clsUtilTools();
                                dsTemp = clt.DataTableNumberToTexto2(dsTemp);

                                this.exportToExcel(dsTemp, strFile);
                            }
                        }
                        else if (strExtensao == ".XML")
                        {
                            dsTemp.WriteXml(strFile);
                        }
                        else if (strExtensao == ".MDB")
                        {
                            //Cria o arquivo MDB
                            this.exportaToAccess(dsTemp, strFile, Name);
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

                        if (mapa.salvamento_efetuado == true)
                        {
                            Classes.clsArmazenamentoDados salvar = new Classes.clsArmazenamentoDados();
                            salvar.salvarStatus(Path.GetDirectoryName(strFile) + "\\" + Path.GetFileNameWithoutExtension(strFile), Path.GetFileNameWithoutExtension(strFile));
                            mapa.salvamento_efetuado = false;
                        }

                        Cursor.Current = Cursors.Default;
                    }
                    else 
                    {
                        fechar = fechamento = false;
                    }
                }
                else
                {
                    MessageBox.Show("Tabela de dados não possui observações.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception er)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(er.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region exportação to Access

        public void exportaToAccess(DataTable ds, string strFile, string strTable)
        {
            //Deleta o MDB caso ja exista
            if (File.Exists(strFile) == true) File.Delete(strFile);

            //Cria o MDB
            ADOX.CatalogClass cat = new ADOX.CatalogClass();
            cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFile + ";Jet OLEDB:Engine Type=5");
            cat = null;

            //Abre a conexão
            OleDbConnection myConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFile);
            myConnection.Open();

            //Cria a tabela e adiciona as variáveis   
            string sql = "CREATE TABLE " + strTable + " (";
            int cnt = 0;
            foreach (DataColumn dc in ds.Columns)
            {
                sql += dc.ColumnName;
                if (dc.DataType == typeof(double) || dc.DataType == typeof(int))
                {
                    sql += " NUMBER NULL";
                }
                else
                {
                    sql += " STRING(120) NULL";
                }
                cnt++;
                if (cnt != ds.Columns.Count)
                {
                    sql += ",";
                }
            }

            sql += ")";

            // Command for Creating Table
            OleDbCommand myCommand = new OleDbCommand(sql, myConnection);
            myCommand.ExecuteNonQuery();

            /************************************   inserindo as observações *****************************************************/

            //Insert into access database start
            OleDbConnection oledbCon = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFile + ";");

            string insertSql = "INSERT INTO " + strTable + " (";
            string valueClause = "VALUES (";
            cnt = 0;
            foreach (DataColumn dc in ds.Columns)
            {
                insertSql += dc.ColumnName;
                valueClause += "@" + dc.ColumnName;

                if (cnt != ds.Columns.Count - 1)
                {
                    insertSql += ",";
                    valueClause += ",";
                }
                cnt++;
            }
            insertSql += ")" + valueClause + ")";

            cnt = 0;
            foreach (DataRow dr in ds.Rows)
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = oledbCon;
                cmd.CommandText = insertSql;
                foreach (DataColumn dc in ds.Columns)
                {
                    string columnName = dc.ColumnName;
                    string paramName = "@" + dc.ColumnName;
                    string paramValue = Convert.ToString(dr[columnName]);
                    if (paramValue == string.Empty)
                    {
                        cmd.Parameters.Add(paramName, OleDbType.VarChar);
                        cmd.Parameters[paramName].Value = DBNull.Value;
                    }
                    else if (paramValue != string.Empty)
                    {
                        cmd.Parameters.Add(paramName, OleDbType.VarChar);
                        cmd.Parameters[paramName].Value = paramValue;
                    }
                }
                oledbCon.Open();
                cmd.ExecuteNonQuery();
                oledbCon.Close();
            }
        }
        #endregion

        #region exportação do Excel (versão antiga, do IPEAGEO versão 1.0)

        public void exportToExcel(DataTable source, string fileName)
        {
            System.IO.StreamWriter excelDoc;
            
            excelDoc = new System.IO.StreamWriter(fileName);
            const string startExcelXML = "<xml version>\r\n<Workbook " +
                  "xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n" +
                  " xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n " +
                  "xmlns:x=\"urn:schemas-    microsoft-com:office:" +
                  "excel\"\r\n xmlns:ss=\"urn:schemas-microsoft-com:" +
                  "office:spreadsheet\">\r\n <Styles>\r\n " +
                  "<Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n " +
                  "<Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>" +
                  "\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>" +
                  "\r\n <Protection/>\r\n </Style>\r\n " +
                  "<Style ss:ID=\"BoldColumn\">\r\n <Font " +
                  "x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n </Style>\r\n " +
                  "<Style     ss:ID=\"StringLiteral\">\r\n <NumberFormat" +
                  " ss:Format=\"@\"/>\r\n </Style>\r\n <Style " +
                  "ss:ID=\"Decimal\">\r\n <NumberFormat " +
                  "ss:Format=\"0.0000\"/>\r\n </Style>\r\n " +
                  "<Style ss:ID=\"Integer\">\r\n <NumberFormat " +
                  "ss:Format=\"0\"/>\r\n </Style>\r\n <Style " +
                  "ss:ID=\"DateLiteral\">\r\n <NumberFormat " +
                  "ss:Format=\"mm/dd/yyyy;@\"/>\r\n </Style>\r\n " +
                  "</Styles>\r\n ";
            const string endExcelXML = "</Workbook>";

            int rowCount = 0;
            int sheetCount = 1;

            excelDoc.Write(startExcelXML);
            excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
            excelDoc.Write("<Table>");
            excelDoc.Write("<Row>");
            for (int x = 0; x < source.Columns.Count; x++)
            {
                excelDoc.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
                excelDoc.Write(source.Columns[x].ColumnName);
                excelDoc.Write("</Data></Cell>");
            }
            excelDoc.Write("</Row>");
            foreach (DataRow x in source.Rows)
            {
                rowCount++;
                //if the number of rows is > 64000 create a new page to continue output
                if (rowCount == 64000)
                {
                    rowCount = 0;
                    sheetCount++;
                    excelDoc.Write("</Table>");
                    excelDoc.Write(" </Worksheet>");
                    excelDoc.Write("<Worksheet ss:Name=\"Sheet" + sheetCount + "\">");
                    excelDoc.Write("<Table>");
                }
                excelDoc.Write("<Row>"); //ID=" + rowCount + "
                for (int y = 0; y < source.Columns.Count; y++)
                {
                    System.Type rowType;
                    rowType = x[y].GetType();
                    switch (rowType.ToString())
                    {
                        case "System.String":
                            string XMLstring = x[y].ToString();
                            XMLstring = XMLstring.Trim();
                            XMLstring = XMLstring.Replace("&", "&");
                            XMLstring = XMLstring.Replace(">", ">");
                            XMLstring = XMLstring.Replace("<", "<");
                            excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                           "<Data ss:Type=\"String\">");
                            excelDoc.Write(XMLstring);
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.DateTime":
                            //Excel has a specific Date Format of YYYY-MM-DD followed by  
                            //the letter 'T' then hh:mm:sss.lll Example 2005-01-31T24:01:21.000
                            //The Following Code puts the date stored in XMLDate 
                            //to the format above
                            DateTime XMLDate = (DateTime)x[y];
                            string XMLDatetoString = ""; //Excel Converted Date
                            XMLDatetoString = XMLDate.Year.ToString() +
                                 "-" +
                                 (XMLDate.Month < 10 ? "0" +
                                 XMLDate.Month.ToString() : XMLDate.Month.ToString()) +
                                 "-" +
                                 (XMLDate.Day < 10 ? "0" +
                                 XMLDate.Day.ToString() : XMLDate.Day.ToString()) +
                                 "T" +
                                 (XMLDate.Hour < 10 ? "0" +
                                 XMLDate.Hour.ToString() : XMLDate.Hour.ToString()) +
                                 ":" +
                                 (XMLDate.Minute < 10 ? "0" +
                                 XMLDate.Minute.ToString() : XMLDate.Minute.ToString()) +
                                 ":" +
                                 (XMLDate.Second < 10 ? "0" +
                                 XMLDate.Second.ToString() : XMLDate.Second.ToString()) +
                                 ".000";
                            excelDoc.Write("<Cell ss:StyleID=\"DateLiteral\">" +
                                         "<Data ss:Type=\"DateTime\">");
                            excelDoc.Write(XMLDatetoString);
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.Boolean":
                            excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                        "<Data ss:Type=\"String\">");
                            excelDoc.Write(x[y].ToString());
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            excelDoc.Write("<Cell ss:StyleID=\"Integer\">" +
                                    "<Data ss:Type=\"Number\">");
                            excelDoc.Write(x[y].ToString());
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.Decimal":
                        case "System.Double":
                            excelDoc.Write("<Cell ss:StyleID=\"Decimal\">" +
                                  "<Data ss:Type=\"Number\">");
                            excelDoc.Write(x[y].ToString());
                            //excelDoc.Write(m_cltools.Double2TextoBrazilCulture(x[y], 15));
                            excelDoc.Write("</Data></Cell>");
                            break;
                        case "System.DBNull":
                            excelDoc.Write("<Cell ss:StyleID=\"StringLiteral\">" +
                                  "<Data ss:Type=\"String\">");
                            excelDoc.Write("");
                            excelDoc.Write("</Data></Cell>");
                            break;
                        default:
                            throw (new Exception(rowType.ToString() + " not handled."));
                    }
                }
                excelDoc.Write("</Row>");
            }
            excelDoc.Write("</Table>");
            excelDoc.Write(" </Worksheet>");
            excelDoc.Write(endExcelXML);
            excelDoc.Close();
        }
        #endregion
    }
}
