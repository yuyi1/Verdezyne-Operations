using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XWPF.UserModel;

namespace OfficeHandler.Word.Converters
{
    public static class StringDataTableFactory
    {
        /// <summary>
        /// Creates a table from a string array
        /// </summary>
        /// <param name="fields">string[] - the column names</param>
        /// <returns>DataTable - the table created by the factory</returns>
        public static DataTable CreateTable(string[] fields)
        {
            DataTable dt = new DataTable();
            foreach (string t in fields)
            {
                var column = new DataColumn();
                column.AllowDBNull = true;
                column.ColumnName = t;
                column.DataType = System.Type.GetType("System.String");
                column.Caption = t;
                dt.Columns.Add(column);
            }
            return dt;
        }
        /// <summary>
        /// Creates a DataTable from table cells in WordTable list of TableCells
        /// </summary>
        /// <param name="wordTableRowCells">List<XWPFTableCell/> - the cells in the row from the WordTable</param>
        /// <returns>DataTable - the table created by the factory</returns>
        public static DataTable CreateTable(List<XWPFTableCell> wordTableRowCells)
        {
            var cells = OfficeHandler.Word.Converters.WordCellsToObjectArray.CellsToArray(wordTableRowCells);
            DataTable dt = new DataTable();
            foreach (object t in cells)
            {
                var column = new DataColumn();
                column.AllowDBNull = true;
                column.ColumnName = t.ToString();
                column.DataType = System.Type.GetType("System.String");
                column.Caption = t.ToString();
                dt.Columns.Add(column);
            }
            return dt;
        }
    }
}
