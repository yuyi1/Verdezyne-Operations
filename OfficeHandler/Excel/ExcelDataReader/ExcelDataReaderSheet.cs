using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.OpenXml4Net.Exceptions;
using OfficeHandler.Contracts;

namespace OfficeHandler.Excel.ExcelDataReader
{
    class ExcelDataReaderSheet : IExcelSheet
    {
        /// <summary>
        /// DataSet for worksheet being wrapped by this object
        /// </summary>
        private readonly DataTable _worksheet;

        //private object _worksheet1;

        public ExcelDataReaderSheet(DataTable dataTable)
        {
            this._worksheet = dataTable;
        }


        public string Name { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }

        public object Worksheet
        {
            get { return _worksheet; }
            set { throw new NotImplementedException("The DataTable in ExcelDataReader is read only."); }
        }

        /// <summary>
        /// Gets the specified cell value as string.
        /// </summary>
        /// <param name="row">Cell row index starting at 1.</param>
        /// <param name="column">Cell column using the format A,B,C...</param>
        /// <returns>The specified cell value retrieved as string.</returns>
        public string GetCellValue(int row, string column)
        {
            int r = row - 1;
            int c = ExcelUtils.GetColumnIndex(column);
            return _worksheet.Rows[r].ItemArray[c].ToString();
        }
        public string GetCellValue(int row, int column)
        {
            int r = row - 1;
            int c = column - 1;
            return _worksheet.Rows[r].ItemArray[c].ToString();
        }

        public void SetCellValue(int row, string column, object value)
        {
            int r = row - 1;
            int c = ExcelUtils.GetColumnIndex(column);
            _worksheet.Rows[r][c] = value;
        }

        public void SetCellValue(int row, int column, object value)
        {
            int r = row - 1;
            int c = column - 1;
            _worksheet.Rows[r][c] = value;
        }

        public void SetCellToDateFormat(int row, int column, object value)
        {
            int r = row - 1;
            int c = column - 1;

            try
            {
                var dt = DateTime.Parse(value.ToString());
                _worksheet.Rows[r][c] = value;
            }
            catch (ArgumentNullException isnullException)
            {
                throw new Exception("Cannot set a DateTime cell to null" + isnullException.Message + isnullException.StackTrace);
            }
            catch (InvalidFormatException formatException)
            {
                throw new Exception("The object is not a valid string that can coverted to a DateTime" + formatException.Message + formatException.StackTrace);
            }


        }

    }
}
