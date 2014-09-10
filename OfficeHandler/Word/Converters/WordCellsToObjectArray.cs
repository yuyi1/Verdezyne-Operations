using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XWPF.UserModel;

namespace OfficeHandler.Word.Converters
{
    public static class WordCellsToObjectArray
    {
        public static object[] CellsToArray(List<XWPFTableCell> cells)
        {
            var arr = cells.ToArray();
            var rowarray = new object[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                rowarray[i] = CellText.GetCellText(cells[i]);
            }
            return rowarray;
        }
    }
}
