namespace OfficeHandler.Contracts
{
    /// <summary>
    /// Interface that defines the factory. Provides a method Create(string) returning IExcelHandler
    /// </summary>
    public interface IExcelHandlerFactory
    {
        /// <summary>
        /// Creates the concrete IExcelHandler implementation according to the file extension
        /// </summary>
        /// <param name="filename">Excel file physical location (.xls or .xlsx) according to implementation.</param>
        /// <returns>An instance of the IExcelHandler according to the file extension specified.</returns>
        IExcelHandler Create(string filename);
    }
}
