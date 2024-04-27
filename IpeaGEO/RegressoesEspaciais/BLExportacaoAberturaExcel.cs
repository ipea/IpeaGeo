using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace IpeaGeo.RegressoesEspaciais
{
    public class BLExportacaoAberturaExcel
    {
        public BLExportacaoAberturaExcel()
        {
        }

        public void ExportaToExcel(System.Data.DataTable dt, string strFile, string striSheet)
        {
            Microsoft.Office.Interop.Excel.Application xla = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlw = xla.Workbooks.Add(Microsoft.Office.Interop.Excel.XlSheetType.xlWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet xls = (Microsoft.Office.Interop.Excel.Worksheet)xla.ActiveSheet;
                       
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                xls.Cells[1, j + 1] = dt.Columns[j].ColumnName;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    xls.Cells[i + 2, j + 1] = dt.Rows[i][j];
                }
            }

            xla.Visible = true;
        }
    }
}
