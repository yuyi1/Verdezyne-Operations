using System.Collections.Generic;
using System.Data;
using System.IO;
using NPOI.XWPF.UserModel;
using OfficeHandler.Contracts;
using OfficeHandler.Excel.Factory;
using OfficeHandler.Word.Converters;

namespace OfficeHandler.Word.Tables
{
    public class SamplingDetailsTable : IWordTable
    {
        public DataTable FindTable(string filepath)
        {
            var x = File.Exists(filepath);
            using (FileStream stream = File.OpenRead(filepath))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                foreach (XWPFTable table in doc.Tables)
                {
                    if (FirstRowContainsGc(table) && SecondRowFirstCellContainsInnoc(table))
                    {
                        return WordTableToDataTable.GetDataTable(table);
                    }
                }
                return null;
            }
        }

        private bool SecondRowFirstCellContainsInnoc(XWPFTable table)
        {
            XWPFTableCell cel = table.Rows[1].GetCell(0);
            {
                string celtext = CellText.GetCellText(cel);
                if (celtext.StartsWith("Inoc"))
                    return true;
            }
            return false;
        }

        private bool FirstRowContainsGc(XWPFTable table)
        {
            List<XWPFTableCell> cells = table.Rows[0].GetTableCells();
            foreach (XWPFTableCell cel in cells)
            {
                string celtext = CellText.GetCellText(cel);
                if (celtext.StartsWith("GC"))
                    return true;
            }
            return false;
        }



        
    }
}
