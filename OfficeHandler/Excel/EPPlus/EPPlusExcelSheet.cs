using NPOI.HSSF.Record;
using OfficeHandler.Contracts;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace OfficeHandler.Excel.EPPlus
{
    /// <summary>
    /// EPPlus implementation of IExcelSheet
    /// </summary>
    public class EPPlusExcelSheet : IExcelSheet
    {
        /// <summary>
        /// Worksheet being wrapped by this object.
        /// </summary>
        private ExcelWorksheet _worksheet;

        /// <summary>
        /// Creates a new instance of the class from an existing <c>ExcelWorksheet</c>
        /// object.
        /// </summary>
        /// <param name="worksheet">Worksheet wrapped by this object.</param>
        public EPPlusExcelSheet(ExcelWorksheet worksheet)
        {
            this._worksheet = worksheet;
        }

        public string Name { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }

        /// <summary>
        /// Worksheet being wrapped by this object.
        /// </summary>
        public object Worksheet
        {
            get { return _worksheet; }
            set
            {
                _worksheet = (ExcelWorksheet)value;
            }
        }
        

        /// <summary>
        /// Gets the worksheet's name.
        /// </summary>

        /// <summary>
        /// Gets the specified cell value as string.
        /// </summary>
        /// <param name="row">Cell row index starting at 1.</param>
        /// <param name="column">Cell column using the format A,B,C...</param>
        /// <returns>The specified cell value retrieved as string.</returns>
        public string GetCellValue(int row, string column)
        {
            var cellrange = this._worksheet.Cells[row, ExcelUtils.GetColumnIndex(column)];

            return (cellrange.Value != null) ? cellrange.Value.ToString() : string.Empty;
        }

        public string GetCellValue(int row, int column)
        {
            var v = _worksheet.Cells[row, column].Value;
            return (v != null) ? v.ToString() : string.Empty;
        }

        /// <summary>
        /// Sets the specified cell value.
        /// </summary>
        /// <param name="row">Cell row index starting at 1.</param>
        /// <param name="column">Cell column using the format A,B,C...</param>
        /// <param name="value">The specified cell value to store.</param>
        public void SetCellValue(int row, string column, object value)
        {
            var cellrange = this._worksheet.Cells[row, ExcelUtils.GetColumnIndex(column)];

            cellrange.Value = value;
        }

        public void SetCellValue(int row, int column, object value)
        {
            this._worksheet.Cells[row, column].Value = value;
        }

        public void SetCellToDateFormat(int row, int column, object value)
        {
            ExcelRange cellrange = this._worksheet.Cells[row, column];
            cellrange.Style.Numberformat.Format = "mm-dd-yy";
        }

    }
}
