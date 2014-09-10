using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeHandler.FileIO;

namespace OfficeHandler.Word.Converters
{
    public class CsvToSqlCreateTableStatement
    {
        public void Convert(string inpath, string outpath, string tablename)
        {
            DataTable dt = new CsvToDataTable().Convert(inpath, tablename);
            SqlCreateTableWriter sqlCreateWriter = new SqlCreateTableWriter();
            List<object> list = (from DataColumn column in dt.Columns select (object)column.ColumnName).ToList();
            sqlCreateWriter.WriteTableCreateFromList(list, outpath, tablename);
        }
    }
}
