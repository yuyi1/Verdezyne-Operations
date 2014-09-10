using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XWPF.UserModel;
using OfficeHandler.Excel.Factory;

namespace OfficeHandler.Word.Converters
{
    public static class WordTableToDataTable
    {
        public static DataTable GetDataTable(XWPFTable table)
        {
            if (table != null)
            {
                System.Data.DataTable dataTable = StringDataTableFactory.CreateTable(table.Rows[0].GetTableCells());

                var rowcounter = 0;
                foreach (XWPFTableRow row in table.Rows)
                {
                    List<XWPFTableCell> cells = row.GetTableCells();
                    if (rowcounter > 0)
                    {
                        dataTable.Rows.Add(OfficeHandler.Word.Converters.WordCellsToObjectArray.CellsToArray(cells));
                    }
                    rowcounter++;
                }
                return dataTable;
            }
            return null;
        }
    }
}
