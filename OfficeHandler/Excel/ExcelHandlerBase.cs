using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeHandler.Contracts;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace OfficeHandler.Excel
{
    public class  ExcelHandlerBase : IExcelHandler
    {
        public virtual void Dispose()
        {
            
        }

        public virtual int NumberOfSheets { get; private set; }
        public virtual void LoadExelFile(string fileName)
        {
            
        }

        public virtual IExcelSheet GetSheet(int index)
        {
            return null;
        }

        public virtual IExcelSheet FindSheet(string sheetname)
        {
            return null;
        }

        public virtual bool RemoveSheet(string sheetname)
        {
            return false;
            
        }

        public virtual IExcelSheet CreateSheet(string name)
        {
            return null;
        }

        public virtual void Save()
        {
            
        }

        public virtual void Save(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
