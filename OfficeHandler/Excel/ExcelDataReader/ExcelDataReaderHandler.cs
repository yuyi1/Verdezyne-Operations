using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;
using OfficeHandler.Contracts;
using OfficeHandler.Excel.Factory;
using OfficeHandler.Loaders;

namespace OfficeHandler.Excel.ExcelDataReader
{
    public class ExcelDataReaderHandler : ExcelHandlerBase
    {
        private DataSet _excelPackage;

        public override void Dispose()
        {
            _excelPackage.Dispose();
        }


        public override int NumberOfSheets
        {
            get
            {
                return _excelPackage.Tables.Count;
            }
            
        }

        /// <summary>
        /// Gets the excel file and loads it into a DataSet
        /// </summary>
        /// <param name="fileName"></param>
        public override void LoadExelFile(string fileName)
        {
            LoaderBase lb = new LoaderBase(fileName);
            lb.Load();
            _excelPackage = lb.Data;
        }


        public override IExcelSheet GetSheet(int index)
        {
            if (this._excelPackage == null)
            {
                throw new InvalidOperationException("Excel workbook not loaded. You must call LoadExceFile(String) first.");
            }
            
            return new ExcelDataReaderSheet(_excelPackage.Tables[index]);
        }
        private IExcelSheet FindSheetInternal(DataSet workbook, string sheetname)
        {
            var sheet = _excelPackage.Tables[sheetname];
            if (sheet == null)
            {
                throw new KeyNotFoundException(string.Format("{0} not found", sheetname));
            }
            return new ExcelDataReaderSheet(_excelPackage.Tables[sheetname]);
        }

        public override IExcelSheet FindSheet(string sheetname)
        {
            if (this._excelPackage == null)
            {
                throw new InvalidOperationException("Excel workbook not loaded. You must call LoadExceFile(String) first.");
            }

             var sheet = _excelPackage.Tables[sheetname];
             if (sheet == null)
             {
                 throw new KeyNotFoundException(string.Format("{0} not found", sheetname));
             }
             return new ExcelDataReaderSheet(_excelPackage.Tables[sheetname]);
        }

        public override bool RemoveSheet(string sheetname)
        {
            if (this._excelPackage == null)
            {
                throw new InvalidOperationException("Excel workbook not loaded. You must call LoadExceFile(String) first.");
            }
            if (_excelPackage.Tables.Contains(sheetname))
            {
                if (_excelPackage.Tables.CanRemove(_excelPackage.Tables[sheetname]))
                {
                    _excelPackage.Tables.Remove(sheetname);
                }
                else
                {
                    throw new ArgumentException("Sheet cannot be removed because other sheets depend on it");
                }
            }
            return true;
        }

        public override IExcelSheet CreateSheet(string name)
        {
            _excelPackage.Tables.Add(DataTableFactory.Create(name));
            return new ExcelDataReaderSheet(_excelPackage.Tables[name]);
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Save(string filename)
        {
            
        }
    }
}
