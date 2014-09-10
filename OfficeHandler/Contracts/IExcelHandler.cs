using System;
using System.Data;

namespace OfficeHandler.Contracts
{
    /// <summary>
    /// Interface that defines an Excel handler. Provides methods for load, get sheet and save.
    /// </summary>
    public interface IExcelHandler : IDisposable
    {
        /// <summary>
        /// Gets the number of sheets in workbook.
        /// </summary>
        int NumberOfSheets { get; }

        /// <summary>
        /// Loads the Excel file to memory using the physical location. This method is invoked by the Factory
        /// in the CreateExcelHandler method right after the constructor is called.
        /// </summary>
        /// <param name="fileName">
        /// Excel file physical location (.xls or .xlsx) according to implementation.
        /// </param>
        void LoadExelFile(string fileName);

        /// <summary>
        /// Gets the specified Excel Sheet as an IExcelSheet object depending on the implementation.
        /// </summary>
        /// <param name="index">Excel Sheet index starting at 1.</param>
        IExcelSheet GetSheet(int index);
        
        /// <summary>
        /// Searchs the workbook's sheets collection for the name
        /// </summary>
        /// <param name="sheetname">string - the name of the sheet to find</param>
        /// <returns>IExcelSheet - the interface to the sheet</returns>
        IExcelSheet FindSheet(string sheetname);

        /// <summary>
        /// Deletes a worksheet if it exists in the collection
        /// </summary>
        /// <param name="sheetname">string - the name of the sheet to delete</param>
        bool RemoveSheet(string sheetname);

        /// <summary>
        /// Creates the specified Excel Sheet and returns as an IExcelSheet object depending on the implementation.
        /// </summary>
        /// <param name="name">Name for the new Excel Sheet.</param>
        IExcelSheet CreateSheet(string name);

        /// <summary>
        /// Saves The changes to the original Excel file.
        /// </summary>
        void Save();

        /// <summary>
        /// Saves the changes to another file specified at filename parameter.
        /// </summary>
        /// <param name="filename">The physical location to save the file.</param>
        void Save(string filename);

        
        //DataTable ReadFermentationOutline();
    }
}
