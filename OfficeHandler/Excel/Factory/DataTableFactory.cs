using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XWPF.UserModel;
using OfficeHandler.Word;

namespace OfficeHandler.Excel.Factory
{
    public static class DataTableFactory
    {
        public static  DataTable Create(List<XWPFTableCell> cells)
        {
            try
            {
                DataTable table = new DataTable();
                int colno = 1;
                foreach (XWPFTableCell cel in cells)
                {
                    string text = CellText.GetCellText(cel);
                    if (text.Length == 0)
                        text = "Column" + colno++.ToString(CultureInfo.InvariantCulture);
                    table.Columns.Add(text, typeof(System.String));
                }
                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Creates a new empty DataTable with no columns
        /// </summary>
        /// <param name="name">string - the name of the table</param>
        /// <returns>DataTable - a new DataTable</returns>
        public static DataTable Create(string name)
        {
            return new DataTable();
            ;
        }
    }
}
