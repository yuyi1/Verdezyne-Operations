using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OfficeHandler.Word.Converters
{
    public class CsvToDataTable
    {
        private DataTable _dt = null;
        public DataTable Convert(string filePath, string tablename)
        {
            string[] csvRows = System.IO.File.ReadAllLines(filePath);

            foreach (string csvRow in csvRows)
            {
                if (csvRow.Trim().Length > 0)
                {
                    string[] fields = csvRow.Split(',');
                    if (_dt == null)
                    {
                        _dt = StringDataTableFactory.CreateTable(fields);
                        _dt.TableName = tablename;
                    }
                    else
                    {
                        var row = _dt.NewRow();
                        row.ItemArray = fields;
                        _dt.Rows.Add(row);
                    }
                }
            }
            return _dt;
        }
    }
}

