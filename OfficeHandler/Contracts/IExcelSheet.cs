namespace OfficeHandler.Contracts
{
    /// <summary>
    /// Interface that defines a worksheet. 
    /// </summary>
    public interface IExcelSheet
    {
        /// <summary>
        /// Gets the worksheet's name.
        /// </summary>
        string Name { get; set; }
        int RowCount { get; set; }
        int ColumnCount { get; set; }
        object Worksheet { get; set; }

        /// <summary>
        /// Gets the specified cell value as string.
        /// </summary>
        /// <param name="row">Cell row index starting at 1.</param>
        /// <param name="column">Cell column using the format A,B,C...</param>
        /// <returns>The specified cell value retrieved as string.</returns>
        string GetCellValue(int row, string column);
        string GetCellValue(int row, int column);

        /// <summary>
        /// Sets the specified cell value.
        /// </summary>
        /// <param name="row">Cell row index starting at 1.</param>
        /// <param name="column">Cell column using the format A,B,C...</param>
        /// <param name="value">The specified cell value to store.</param>
        void SetCellValue(int row, string column, object value);
        void SetCellValue(int row, int column, object value);
        void SetCellToDateFormat(int row, int column, object value);

    }
}