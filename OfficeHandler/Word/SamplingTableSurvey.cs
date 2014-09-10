using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FilterPipeline.Filters;
using OfficeHandler.Contracts;
using OfficeHandler.Excel;
using OfficeHandler.Excel.Factory;
using OfficeHandler.FileIO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace OfficeHandler.Word
{
    public class SamplingTableSurvey
    {
        public IQueryable<string> QueryableFileList { get; set; }
        public string Fermentation_Folder = "\\\\james\\Shared\\Research\\Fermentation\\";
        public List<object> columnList = new List<object>();
        public int filesprocessed;
        public int filesnotprocessed;

        public IExcelHandler ExcelHandler { get; set; }
        public IExcelSheet CurrentSheet { get; set; }

        public void InitializeSurveySheet(string filename)
        {
            try
            {
                File.Delete(filename);
                ExcelHandler = ExcelHandlerFactory.Instance.Create(filename);
                CurrentSheet = ExcelHandler.CreateSheet("SampleTableSurvey");
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }

        public List<object> SurveyColumns()
        {
            FindOutlineFiles(@"Outline");
            //int counter = 0;
            DocxReader reader = new DocxReader();
            string persistfile = @"..\..\TestFiles\ColumnList.xlsx";
            File.Delete(persistfile);
            try
            {
                using (var excelHandler = ExcelHandlerFactory.Instance.Create(persistfile))
                {
                    IExcelSheet sheet = excelHandler.CreateSheet("SampleTableSurvey");

                    int row = 2;
                    foreach (string filepath in QueryableFileList)
                    {
                        try
                        {
                            sheet.SetCellValue(row, 2, filepath);
                            var createDate = new FileInfo(filepath).CreationTime;
                            sheet.SetCellValue(row, 1, createDate);
                            sheet.SetCellToDateFormat(row, 1, createDate);
                            List<object> list = reader.GetSamplingDetailsColumnList(filepath);
                            columnList.AddRange(list.Where(l => !columnList.Contains(l)));
                            SetHeaderList(sheet);


                            foreach (object s in list)
                            {
                                int col = columnList.FindIndex(t => t.ToString() == s.ToString()) + 3;
                                sheet.SetCellValue(row, col, @"x");
                            }
                            filesprocessed++;
                            //if (row == 10)
                            //    break;
                        }
                        catch (Exception ex)
                        {
                            //System.Diagnostics.Debug.WriteLine(string.Format("Path: {0} Not Processed {1}", filepath, ex.Message));
                            sheet.SetCellValue(row, 3, "Error Reading File " + ex.Message);
                            filesnotprocessed++;
                        }
                        finally
                        {
                            row++;
                        }
                    }
                    
                    IExcelSheet listSheet = excelHandler.CreateSheet("ColumnList");
                    listSheet.SetCellValue(1, 1, @"Column List");
                    for (int index = 0; index < columnList.Count; index++)
                    {
                        listSheet.SetCellValue(index + 2, 1, columnList[index]);
                    }

                    //Write a Create Table sql statement to a file
                    const string sqlfile = @"..\..\TestFiles\ColumnList.sql";
                    new SqlCreateTableWriter().WriteTableCreateFromList(columnList, sqlfile, @"PlanColumns");
                    
                    excelHandler.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }


            //            Persist();
            return columnList;
        }


        public IQueryable<string> FindOutlineFiles(string fn)
        {
            if (QueryableFileList == null)
                QueryableFileList = System.IO.Directory
                    .EnumerateFiles(this.Fermentation_Folder, "*.docx", SearchOption.AllDirectories)
                    .Where(x => x.Contains(fn))
                    .AsQueryable<string>();

            return QueryableFileList;
        }

        private void Persist()
        {
            string persistfile = @"..\..\TestFiles\ColumnList.xlsx";
            File.Delete(persistfile);
            try
            {
                using (var excelHandler = ExcelHandlerFactory.Instance.Create(persistfile))
                {
                    IExcelSheet sheet = excelHandler.CreateSheet("SampleTableSurvey");
                    SetHeaderList(sheet);

                    excelHandler.Save();
                }
            }
            catch (Exception)
            {

                throw;
            }







            //StringBuilder sb = new StringBuilder();
            //foreach (string s in columnList)
            //{
            //    sb.AppendLine(s);
            //}
            //using (Stream stream = new FileStream( , FileMode.Truncate, FileAccess.ReadWrite, FileShare.ReadWrite))
            //{
            //    StreamWriter sw = new StreamWriter(stream);
            //    sw.Write(sb.ToString());
            //    sw.Flush();
            //    sw.Close();
            //    stream.Close();
            //}
        }

        private void SetHeaderList(IExcelSheet sheet)
        {
            sheet.SetCellValue(1, 1, @"Create Date");
            sheet.SetCellValue(1, 2, @"File Name");
            int col = 3;
            foreach (string s in columnList)
            {
                sheet.SetCellValue(1, col++, s);
            }
        }
    }
}
