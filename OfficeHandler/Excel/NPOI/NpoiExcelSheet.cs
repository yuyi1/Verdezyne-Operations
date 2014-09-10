using System;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeHandler.Contracts;

namespace OfficeHandler.Excel.NPOI
{
    /// <summary>
    /// NOPI implementation of IExcelSheet
    /// </summary>
    public class NpoiExcelSheet : IExcelSheet
    {
        /// <summary>
        /// Worksheet being wrapped by this object.
        /// </summary>
        private readonly ISheet _sheet;

        private object _worksheet;

        /// <summary>
        /// Creates a new instance of NpoiExcelSheet using a Sheet from NOPI library.
        /// </summary>
        /// <param name="sheet">Worksheet wrapped by this object.</param>
        public NpoiExcelSheet(ISheet sheet)
        {
            this._sheet = sheet;
        }


        public string Name { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }

        public object Worksheet
        {
            get { return _worksheet; }
            set { _worksheet = value; }
        }

        /// <summary>
        /// Gets the specified cell value as string.
        /// </summary>
        /// <param name="row">Cell row index starting at 1.</param>
        /// <param name="column">Cell column using the format A,B,C...</param>
        /// <returns>The specified cell value retrieved as string.</returns>
        public string GetCellValue(int row, string column)
        {
            // Getting the row... 0 is the first row.
            IRow dataRow = _sheet.GetRow(row - 1);

            if (dataRow == null)
            {
                return string.Empty;
            }

            // Getting the column... 0 is the first column.
            var cell = dataRow.GetCell(ExcelUtils.GetColumnIndex(column) - 1);
            if (cell == null)
            {
                return string.Empty;
            }

            if (cell.CellType == CellType.Numeric)
            {
                return cell.NumericCellValue.ToString();
            }

            return cell.ToString();
        }

        public string GetCellValue(int row, int column)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the specified cell value.
        /// </summary>
        /// <param name="row">Cell row index starting at 1.</param>
        /// <param name="column">Cell column using the format A,B,C...</param>
        /// <param name="value">The specified cell value to store.</param>
        public void SetCellValue(int row, string column, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "Value can't be null");
            }

            IRow dataRow = _sheet.GetRow(row - 1) ?? _sheet.CreateRow(row - 1);
            var cellIndex = ExcelUtils.GetColumnIndex(column) - 1;
            var cell = dataRow.GetCell(cellIndex) ?? dataRow.CreateCell(cellIndex);

            cell.SetCellValue(value.ToString());
        }

        public void SetCellValue(int row, int column, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "Value can't be null");
            }
            IRow dataRow = _sheet.GetRow(row - 1) ?? _sheet.CreateRow(row - 1);
            var cellIndex = column;
            var cell = dataRow.GetCell(cellIndex) ?? dataRow.CreateCell(cellIndex);

            cell.SetCellValue(value.ToString());
        }

        public void SetCellToDateFormat(int row, int column, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "value can't be null");
            }
            IRow dataRow = _sheet.GetRow(row - 1) ?? _sheet.CreateRow(row - 1);
            var cellIndex = column;
            var cell = dataRow.GetCell(cellIndex) ?? dataRow.CreateCell(cellIndex);
            cell.CellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("mm/dd/yy");
            
        }

    }
}
