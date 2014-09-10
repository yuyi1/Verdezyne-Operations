using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.SS.Formula.Functions;
using OfficeHandler.Contracts;
using OfficeHandler.Excel.EPPlus;
using OfficeHandler.Excel.ExcelDataReader;
using OfficeOpenXml;

namespace OfficeHandler.Excel
{
    public class TestWriter
    {
        public static void WriteTestData(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            if (fi.Exists)
            {
                fi.Delete();
                fi = new FileInfo(filename);
            }

            ExcelDataReaderHandler source = new ExcelDataReaderHandler();
            source.LoadExelFile(@"..\..\TestFiles\CLE_140509_140506_300L_GC1_LC1.xlsx");
            
            EPPlusExcelHandler dest = new EPPlusExcelHandler();
            dest.LoadExelFile(filename);
            //ExcelWorksheet ws = package.Workbook.Worksheets.Add("Test");
            for (int i = 0; i < source.NumberOfSheets; i++)
            {
                IExcelSheet sourcesheet = source.GetSheet(i);
                IExcelSheet destsheet = dest.CreateSheet(sourcesheet.Name);

                for (int r = 1; r <= sourcesheet.RowCount; r++)
                {
                    for (int c = 1; c <= sourcesheet.ColumnCount; c++)
                    {
                        string value = sourcesheet.GetCellValue(r, c);
                        destsheet.SetCellValue(r, c, value);
                    }
                }
            }
            dest.Save(filename);
        }

        //using (SpreadsheetDocument mydoc = SpreadsheetDocument.Open(filename, true))
        //{
        //    WorkbookPart wp = mydoc.WorkbookPart;
        //    WorksheetPart ws = wp.WorksheetParts.First();
        //    SheetData sd = ws.Worksheet.Elements<SheetData>().First();
        //    for (int rowcounter = 0; rowcounter < numrows; rowcounter++)
        //    {
        //        Row r = new Row();
        //        for (int col = 0; col < numcols; col++)
        //        {
        //            Cell c = new Cell();
        //            CellFormula f = new CellFormula();
        //            f.CalculateCell = true;
        //            f.Text = "RAND()";
        //            c.Append(f);
        //            CellValue v = new CellValue();
        //            c.Append(v);
        //            r.Append(c);
        //        }
        //        sd.Append(r);
        //        ws.Worksheet.Save();
        //        wp.Workbook.Save();






    }
}

