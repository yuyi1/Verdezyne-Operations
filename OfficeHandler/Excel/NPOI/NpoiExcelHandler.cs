using System;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeHandler.Contracts;
using OfficeHandler.Word.Converters;

namespace OfficeHandler.Excel.NPOI
{
    /// <summary>
    /// NPOI implementation of IExcelHandler
    /// </summary>
    public class NpoiExcelHandler : IExcelHandler
    {
        /// <summary>
        /// Workbook being manipulated by this class.
        /// </summary>
        private HSSFWorkbook _workbook;

        /// <summary>
        /// Name of the file containing the workbook.
        /// </summary>
        private string _fileName;

        /// <summary>
        /// Gets the number of sheets in workbook.
        /// </summary>
        public int NumberOfSheets
        {
            get
            {
                if (this._workbook == null)
                {
                    throw new InvalidOperationException("Excel workbook not loaded. You must call LoadExceFile(String) first.");
                }

                return this._workbook.NumberOfSheets;
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
            this._fileName = fileName;
            if (File.Exists(fileName))
            {
                FileStream fs = File.OpenRead(fileName);
                _workbook = new HSSFWorkbook(fs, true);
            }
            else
            {
                _workbook = new HSSFWorkbook();
            }
        }

        /// <summary>
        /// Gets the specified Excel Sheet as an IExcelSheet object depending on the implementation.
        /// </summary>
        /// <param name="index">Excel Sheet index starting at 1.</param>
        public IExcelSheet GetSheet(int index)
        {
            if (this._workbook == null)
            {
                throw new InvalidOperationException("Excel workbook not loaded. You must call LoadExceFile(String) first.");
            }

            // Getting the worksheet by its index...
            ISheet sh = _workbook.GetSheetAt(index - 1);
            return new NpoiExcelSheet(sh);
        }

        public IExcelSheet FindSheet(string sheetname)
        {
            if (this._workbook == null)
            {
                throw new InvalidOperationException("Excel workbook not loaded. You must call LoadExceFile(String) first.");
            }


            ISheet sheet = _workbook.GetSheet(sheetname);
            int index = _workbook.GetSheetIndex(sheet);
            if (index > -1)
            {
                // Getting the worksheet by its index...
                ISheet sh = _workbook.GetSheetAt(index - 1);
                return new NpoiExcelSheet(sh);
            }
            return null;
        }

        public bool RemoveSheet(string sheetname)
        {
            if (this._workbook == null)
            {
                throw new InvalidOperationException("Excel workbook not loaded. You must call LoadExceFile(String) first.");
            }

            ISheet sheet = _workbook.GetSheet(sheetname);
            int index = _workbook.GetSheetIndex(sheet);
            if (index > -1)
            {
                _workbook.RemoveSheetAt(index);
            }
            return true;
        }


        /// <summary>
        /// Creates the specified Excel Sheet and returns as an IExcelSheet object depending on the implementation.
        /// </summary>
        /// <param name="name">Name for the new Excel Sheet.</param>
        public IExcelSheet CreateSheet(string name)
        {
            if (_workbook == null)
                throw new InvalidOperationException("Excel workbook not loaded. You must call LoadExceFile(String) first.");

            var sheet = _workbook.CreateSheet(name);
            // Getting the worksheet by its index...
            //Sheet sh = _workbook.GetSheet(index - 1);
            return new NpoiExcelSheet(sheet);
        }
        /// <summary>
        /// Saves The changes to the original Excel file.
        /// </summary>
        public void Save()
        {
            FileStream fs = File.OpenWrite(this._fileName);
            this._workbook.Write(fs);
            fs.Close();
        }

        /// <summary>
        /// Saves the changes to another file specified at filename parameter.
        /// </summary>
        /// <param name="filename">The physical location to save the file.</param>
        public void Save(string filename)
        {
            FileStream fs = File.OpenWrite(filename);
            this._workbook.Write(fs);
            fs.Close();
        }

        public bool ReadSubmissionInfoSheet()
        {
            throw new NotImplementedException();
        }

        public DataTable ReadFermentationOutline()
        {
            string[] columns = new[] { "Outline_Topic", "OutlineDescriptionId", "Value" };
            DataTable table = StringDataTableFactory.CreateTable(columns);
            return table;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // TODO find out how to dispose because the Dispose method is missing with version 1.2.5 
            //_workbook.Dispose();
        }
    }
}
