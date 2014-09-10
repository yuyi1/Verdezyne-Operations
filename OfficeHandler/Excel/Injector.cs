using System;
using System.IO;
using System.Management.Instrumentation;
using OfficeHandler.Contracts;
using OfficeHandler.Excel.Factory;
using OfficeOpenXml;

namespace OfficeHandler.Excel
{
    public class Injector
    {
        
        public bool CreateModernFile(string modernfilename)
        {
            try
            {
                File.Delete(modernfilename);
                using (var excelHandler = ExcelHandlerFactory.Instance.Create(modernfilename))
                {
                    IExcelSheet sheet = excelHandler.CreateSheet("new sheet");
                    sheet.SetCellValue(1, "A", "Test value in A1 cell");
                    excelHandler.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public bool CreateLegacyFile(string legacyfilename)
        {
            try
            {
                File.Delete(legacyfilename);
                using (var excelHandler = ExcelHandlerFactory.Instance.Create(legacyfilename))
                {
                    var sheet = excelHandler.CreateSheet("new sheet");
                    sheet.SetCellValue(1, "A", "Test value in A1 cell");
                    excelHandler.Save();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new InstanceNotFoundException(string.Format("Could not create file {0} \n {1}", legacyfilename, ex.Message));
            }
        }

        public IExcelSheet AddSheetToModernFile(FileInfo modernFileInfo, string tabname)
        {
            try
            {
                modernFileInfo.IsReadOnly = false;
                using (var excelHandler = ExcelHandlerFactory.Instance.Create(modernFileInfo.FullName))
                {
                    IExcelSheet s = excelHandler.FindSheet(tabname);
                    if (s == null)
                    {
                        s = excelHandler.CreateSheet(tabname);
                        excelHandler.Save();
                        return s;
                    }
                    return s;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Could not add sheet to {0} {1}", modernFileInfo, ex.Message));
            }
        }

        public bool AddSheetToLegacyFile(string legacyfilename)
        {
            using (var excelHandler = ExcelHandlerFactory.Instance.Create(legacyfilename))
            {
                var sheet = excelHandler.FindSheet("Second sheet") ?? excelHandler.CreateSheet("Second sheet");
                sheet.SetCellValue(1, "A", "Test value in A1 cell");
                excelHandler.Save();
                return true;
            }
        }

        public bool DeleteSheetFromModernFile(string modernfilename)
        {
            using (var excelHandler = ExcelHandlerFactory.Instance.Create(modernfilename))
            {
                try
                {
                    var ret = excelHandler.RemoveSheet("Second sheet");
                    excelHandler.Save();
                    return ret;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
        }

        public bool DeleteSheetFromLegacyFile(string legacyfilename)
        {
            using (var excelHandler = ExcelHandlerFactory.Instance.Create(legacyfilename))
            {
                try
                {
                    var ret = excelHandler.RemoveSheet("Second sheet");
                    excelHandler.Save();
                    return ret;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
        }

        public bool ReadDataSample(string filePath)
        {
            FileInfo existingFile = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                // get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int col = 2; //The item description
                // output the data in column 2
                for (int row = 2; row < 5; row++)
                    System.Diagnostics.Debug.WriteLine("\tCell({0},{1}).Value={2}", row, col, worksheet.Cells[row, col].Value);

                // output the formula in row 5
                System.Diagnostics.Debug.WriteLine("\tCell({0},{1}).Formula={2}", 3, 5, worksheet.Cells[3, 5].Formula);
                System.Diagnostics.Debug.WriteLine("\tCell({0},{1}).FormulaR1C1={2}", 3, 5, worksheet.Cells[3, 5].FormulaR1C1);

                // output the formula in row 5
                System.Diagnostics.Debug.WriteLine("\tCell({0},{1}).Formula={2}", 5, 3, worksheet.Cells[5, 3].Formula);
                System.Diagnostics.Debug.WriteLine("\tCell({0},{1}).FormulaR1C1={2}", 5, 3, worksheet.Cells[5, 3].FormulaR1C1);

            } // the using statement automatically calls Dispose() which closes the package.

            System.Diagnostics.Debug.WriteLine(string.Empty);
            System.Diagnostics.Debug.WriteLine("Sample 2 complete");
            System.Diagnostics.Debug.WriteLine(string.Empty);
            return true;
        }
        public bool ReadData(string filePath)
        {
            using (var excelHandler = ExcelHandlerFactory.Instance.Create(filePath))
            {
                try
                {
                    IExcelSheet sheet = excelHandler.FindSheet("Glossary");
                    if (sheet == null)
                        return false;
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
            return true;
        }
    }
}
