using System;
using System.IO;
using OfficeHandler.Contracts;
using OfficeHandler.Excel.EPPlus;
using OfficeHandler.Excel.NPOI;

namespace OfficeHandler.Excel.Factory
{

    /// <summary>
    /// Quick implementation of IExcelHandlerFactory interface. Provides the method Create 
    /// that handles the creation process according to the file extension (.xls or .xlsx)
    /// </summary>
    public class ExcelHandlerFactory : IExcelHandlerFactory
    {
        /// <summary>
        /// The class' factory.
        /// </summary>
        private static ExcelHandlerFactory _handlerFactory;

        /// <summary> 
        /// Gets the single instance of ExcelHandlerFactory
        /// </summary>
        public static ExcelHandlerFactory Instance
        {
            get { return _handlerFactory ?? (_handlerFactory = new ExcelHandlerFactory()); }
        }

        /// <summary>
        /// Creates the concrete IExcelHandler implementation according to the file extension
        /// </summary>
        /// <param name="filename">Excel file physical location (.xls or .xlsx) according to implementation.</param>
        /// <returns>An instance of the IExcelHandler according to the file extension specified.</returns>
        /// <exception cref="ArgumentNullException">The file name can not be null.</exception>
        /// <exception cref="InvalidOperationException">if the file is encrypted, this operation is not supported.</exception>
        /// <exception cref="ArgumentException">The only file extensions supported are .xls and .xlsx</exception>
        public IExcelHandler Create(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename", "The file name can not be null");
            }

            try
            {
                if (filename.EndsWith(".xls", StringComparison.InvariantCultureIgnoreCase))
                {
                    var exhnd = new NpoiExcelHandler();
                    exhnd.LoadExelFile(filename);
                    return exhnd;
                }

                if (filename.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase))
                {
                    var exhnd = new EPPlusExcelHandler();
                    exhnd.LoadExelFile(filename);
                    return exhnd;
                }
                if (filename.EndsWith(".xlsm", StringComparison.InvariantCultureIgnoreCase))
                {
                    var exhnd = new EPPlusExcelHandler();
                    exhnd.LoadExelFile(filename);
                    return exhnd;
                }

            }
            catch (ObjectDisposedException ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception Thrown: The file is encrypted and it's impossible to open it");
                throw new InvalidOperationException("The file is encrypted and it's impossible to open it", ex);
            }
            catch (Exception ex)
            {
                if (ex.InnerException is FileFormatException)
                {
                    System.Diagnostics.Debug.WriteLine("Exception Thrown: The file is encrypted and it's impossible to open it.");
                    throw new InvalidOperationException("The file is encrypted and it's impossible to open it", ex);
                }

                throw;
            }
            System.Diagnostics.Debug.WriteLine("Exception Thrown: Excel file extension not supported (only .xls or .xlsx).");
            throw new ArgumentException("Excel file extension not supported (only .xls or .xlsx).");
        }
    }
}
