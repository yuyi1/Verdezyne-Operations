using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FilterPipeline.Pipelines;
using NPOI.XWPF.UserModel;
using OfficeHandler.Word.Tables;

namespace OfficeHandler.Word
{
    public class DocxReader
    {
        public bool FermentationOutlineText(string filepath)
        {
            using (FileStream stream = File.OpenRead(filepath))
            {
                XWPFDocument doc = new XWPFDocument(stream);
                foreach (var para in doc.Paragraphs)
                {
                    String text = para.ParagraphText;

                    var runs = para.Runs;
                    String styleId = para.Style;

                    for (int i = 0; i < runs.Count; i++)
                    {
                        var run = runs[i];
                        string rtext = run.ToString(); // get run text

                    }
                }
            }
            return true;
        }


        public List<object> GetSamplingDetailsColumnList(string filepath)
        {
            SamplingDetailsTable tables = new SamplingDetailsTable();

            DataTable table = tables.FindTable(filepath);
            List<object> columnList = new List<object>();
            if (table != null)
            {
                columnList.AddRange((from DataColumn column in table.Columns select column.ColumnName).Cast<object>());
            }
            return WordTableColumnName.FilterColumnList(columnList);
        }
    }
}
