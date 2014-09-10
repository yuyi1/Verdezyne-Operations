using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Principal;
using OfficeHandler.Contracts;
using OfficeHandler.Word.Converters;
using OfficeOpenXml;

namespace OfficeHandler.Excel.EPPlus
{
    /// <summary>
    /// EPPlus implementation of IExcelHandler
    /// </summary>
    public class EPPlusExcelHandler : IExcelHandler
    {
        /// <summary>
        /// File being manipulated by this class.
        /// </summary>
        private ExcelPackage _excelPackage;

        /// <summary>
        /// Gets the number of sheets in workbook.
        /// </summary>
        public int NumberOfSheets
        {
            get
            {
                if (this._excelPackage == null)
                {
                    throw new InvalidOperationException(
                        "Excel worksheet not loaded. You must call LoadExceFile(String) first.");
                }

                return this._excelPackage.Workbook.Worksheets.Count;
            }
        }

        /// <summary>
        /// Loads the Excel file to memory using the physical location. This method is invoked by the Factory
        /// in the CreateExcelHandler method right after the constructor is called.
        /// </summary>
        /// <param name="fileName">
        /// Excel file physical location (.xls or .xlsx) according to implementation.
        /// </param>
        public void LoadExelFile(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            this._excelPackage = new ExcelPackage(fileInfo);
        }

        /// <summary>
        /// Gets the specified Excel Sheet as an IExcelSheet object depending on the implementation.
        /// </summary>
        /// <param name="index">Excel Sheet index starting at 1.</param>
        public IExcelSheet GetSheet(int index)
        {
            if (this._excelPackage == null)
            {
                throw new InvalidOperationException("Excel workbook not loaded. You must call LoadExceFile(String) first.");
            }

            ExcelWorkbook workbook = this._excelPackage.Workbook;

            // Getting the worksheet by its index...
            ExcelWorksheet worksheet = workbook.Worksheets[index];
            return new EPPlusExcelSheet(worksheet);
        }

        public IExcelSheet FindSheet(string name)
        {
            if (this._excelPackage == null)
            {
                throw new InvalidOperationException("Excel workbook not loaded. You must call LoadExceFile(String) first.");
            }

            ExcelWorkbook workbook = this._excelPackage.Workbook;

            // search the sheets
            var sheet = FindSheetInternal(workbook, name);
            {
                if (sheet != null)
                {
                    return GetSheet(sheet.Index);
                }
            }
            return null;
        }

        private ExcelWorksheet FindSheetInternal(ExcelWorkbook workbook, string sheetname)
        {
            return workbook.Worksheets.FirstOrDefault(sheet => sheet.Name == sheetname);
        }

        /// <summary>
        /// Creates the specified Excel Sheet and returns as an IExcelSheet object depending on the implementation.
        /// </summary>
        /// <param name="sheetname">The Name for the new Excel Sheet.</param>
        public IExcelSheet CreateSheet(string sheetname)
        {
            if (_excelPackage == null)
                throw new InvalidOperationException("Excel workbook not loaded. You must call LoadExceFile(String) first.");

            ExcelWorkbook workbook = _excelPackage.Workbook;
            var worksheet = workbook.Worksheets.Add(sheetname);
            return new EPPlusExcelSheet(worksheet);
        }

        public bool RemoveSheet(string sheetname)
        {
            if (_excelPackage == null)
                throw new InvalidOperationException("Excel workbook not loaded. You must call LoadExceFile(String) first.");

            IExcelSheet sheet = FindSheet(sheetname);
            if (sheet != null)
            {
                ExcelWorkbook workbook = _excelPackage.Workbook;
                var internalsheet = FindSheetInternal(workbook, sheetname);
                workbook.Worksheets.Delete(internalsheet.Index);
            }
            return true;
        }

        /// <summary>
        /// Saves The changes to the original Excel file.
        /// </summary>
        public void Save()
        {
            this._excelPackage.Save();
        }

        /// <summary>
        /// Saves the changes to another file specified at filename parameter.
        /// </summary>
        /// <param name="filename">The physical location to save the file.</param>
        public void Save(string filename)
        {
            this._excelPackage.SaveAs(new FileInfo(filename));
        }



        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this._excelPackage.Dispose();
        }

        

        public bool ReadSubmissionInfoSheet()
        {
            ExcelWorksheet worksheet = FindSheetInternal(this._excelPackage.Workbook, "GC&LC Submissions");
            // get the first worksheet in the workbook
            //ExcelWorksheet worksheet = this._excelPackage.Workbook.Worksheets[2];
            for (int row = 1; row < 31; row++)
            {
                for (int col = 1; col < 4; col++)
                {
                    System.Diagnostics.Debug.WriteLine("\tCell({0},{1}).Value={2}", row, col, worksheet.Cells[row, col].Value);
                }
            }

            //int col = 2; //The item description
            //// output the data in column 2
            //for (int row = 2; row < 5; row++)
            //    System.Diagnostics.Debug.WriteLine("\tCell({0},{1}).Value={2}", row, col, worksheet.Cells[row, col].Value);

            //// output the formula in row 5
            //System.Diagnostics.Debug.WriteLine("\tCell({0},{1}).Formula={2}", 3, 5, worksheet.Cells[3, 5].Formula);
            //System.Diagnostics.Debug.WriteLine("\tCell({0},{1}).FormulaR1C1={2}", 3, 5, worksheet.Cells[3, 5].FormulaR1C1);

            //// output the formula in row 5
            //System.Diagnostics.Debug.WriteLine("\tCell({0},{1}).Formula={2}", 5, 3, worksheet.Cells[5, 3].Formula);
            //System.Diagnostics.Debug.WriteLine("\tCell({0},{1}).FormulaR1C1={2}", 5, 3, worksheet.Cells[5, 3].FormulaR1C1);



            System.Diagnostics.Debug.WriteLine(string.Empty);
            System.Diagnostics.Debug.WriteLine("Reading complete");
            System.Diagnostics.Debug.WriteLine(string.Empty);
            return true;
        }

        /// <summary>
        /// Reads the Fermentation Outline tab into a DataTable
        /// </summary>
        /// <returns>DataTable - the table containing the Fermentation Outline</returns>
        public DataTable ReadFermentationOutline()
        {
            var table = new DataTable();
            return table;
        }
    }
}
