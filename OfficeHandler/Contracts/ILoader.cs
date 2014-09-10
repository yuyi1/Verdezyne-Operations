using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeHandler.Loaders;

namespace OfficeHandler.Contracts
{
    public interface ILoader
    {

        void Load();
        LoaderBase LoadBase();

        //void setExcelFilePath(string excelFilePath);
    }
}
