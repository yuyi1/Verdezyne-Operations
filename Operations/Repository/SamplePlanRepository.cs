using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeHandler.Contracts;
using OfficeHandler.Excel;
using OfficeHandler.Excel.Factory;
using OfficeHandler.Word.Tables;
using OfficeOpenXml;
using Operations.Contracts;
using Operations.Models;

namespace Operations.Repository
{
    public class SamplePlanRepository : GenericPilotPlantRepository<SamplePlan>, ISamplePlanRepository
    {
        private readonly IRunRepository _runRepository;

        // Inject the repo and let Ninject take care of the lifecycle
        public SamplePlanRepository(PilotPlantEntities dbContext)
            : base(dbContext)
        {
            _runRepository = new RunRepository(new PilotPlantEntities());
        }

        public async Task<SamplePlan> GetSamplePlanForRunAsync(int runid)
        {
            Run run = DbContext.Runs.FirstOrDefault(r => r.Id == runid);
            SamplePlan plan = await FindAsync(e => e.RunId == runid);
            if (plan == null)
            {
                throw new ObjectNotFoundException(string.Format("No Samples were found for Run Date {0} ", run.Rundate));
            }
            else
            {
                return plan;
            }
        }



        public async Task<SamplePlan> SeedDatabase(int runid)
        {
            SamplePlan plan = new SamplePlan();
            plan.RunId = runid;
            plan.WordFileUri =
                @"C:\\Users\\nstein\\Documents\\Visual Studio 2013\\Projects\\Process\\Process.UnitTest\\TestFiles\\140618_QA1-D3_Outline.docx";
            plan.ExcelFileUri = plan.WordFileUri.Replace(@"140618_QA1-D3_Outline.docx", "CLE_140509_140506_300L_GC1_LC1.xlsx");

            plan.SamplePlanDetails = new Collection<SamplePlanDetail>();

            SamplingDetailsTable sdtable = new SamplingDetailsTable();
            DataTable table = sdtable.FindTable(plan.WordFileUri);

            Run run = DbContext.Runs.Find(runid);
            var datestarted = DateTime.Parse(run.RunStart.ToString());
            int c = 1;
            foreach (DataRow row in table.Rows)
            {
                var det = new SamplePlanDetail();
                //det.Id = c;
                det.Name = row.ItemArray[0].ToString();
                det.DateAndTime = row.ItemArray[1].ToString();
                if (c <= 6)
                {
                    det.IsPrepared = true;
                    det.DatePrepared = datestarted;
                    det.PreparedBy = "nstein";
                }
                else
                {
                    det.IsPrepared = false;
                    det.DatePrepared = DateTime.Parse("1/1/1980");
                    det.PreparedBy = "";
                }
                if (c <= 4)
                {
                    det.IsWeighed = true;
                    det.DateWeighed = datestarted;
                    det.WeighedBy = "nstein";
                }
                else
                {
                    det.IsWeighed = false;
                    det.DateWeighed = DateTime.Parse("1/1/1980");
                    det.WeighedBy = "";
                }
                if (c <= 3)
                {
                    det.IsSubmitted = true;
                    det.DateSubmitted = datestarted;
                    det.SubmittedBy = "nstein";
                }
                else
                {
                    det.IsSubmitted = false;
                    det.DateSubmitted = DateTime.Parse("1/1/1980");
                    det.SubmittedBy = "";
                }


                plan.SamplePlanDetails.Add(det);
                c++;
                datestarted += new TimeSpan(0, 8, 30, 0);
            }

            await base.AddAsync(plan);
            return plan;
        }
        /// <summary>
        /// Saves tracking data from the Word file's Sampling Plan table to the Database
        /// </summary>
        /// <param name="runid">int - the Id of the Run</param>
        /// <param name="fermentationOutlineWordFileInfo">string - the fullpath </param>
        /// <param name="userid">string - the login user</param>
        /// <returns></returns>
        public async Task<SamplePlan> SaveTrackingDataAsync(int runid, FileInfo fermentationOutlineWordFileInfo, string userid)
        {
            SamplePlan plan = DbContext.SamplePlans.Include("SamplePlanDetails").FirstOrDefault(r => r.RunId == runid);

            SamplingDetailsTable sdtable = new SamplingDetailsTable();
            DataTable table = sdtable.FindTable(fermentationOutlineWordFileInfo.FullName);

            Run run = DbContext.Runs.FirstOrDefault(r => r.Id == runid);
            if (run == null)
                throw new NullReferenceException(string.Format("A run with ID of {0} does not exist in the Database.  You need to upload a Fermentation Outline and create a new Run", runid.ToString()));

            if (plan != null)
            {
                var details = DbContext.SamplePlanDetails.Where(s => s.SamplePlanId == plan.Id);
                if (table.Rows.Count != details.Count())
                {
                    plan = DeletePlan(plan.Id);
                }
            }

            if (plan == null)
            {
                plan = new SamplePlan();
                plan.RunId = runid;
                plan.WordFileUri = fermentationOutlineWordFileInfo.FullName;
                plan.ExcelFileUri = string.Empty;

                await base.AddAsync(plan);
                plan.SamplePlanDetails = new Collection<SamplePlanDetail>();
            }

            // Add the details to the SamplingPlanDetails in the SamplePlan
            foreach (DataRow row in table.Rows)
            {
                var name = row.ItemArray[0].ToString();
                var dateAndTime = row.ItemArray[1].ToString();
                SamplePlanDetail det = DbContext.SamplePlanDetails.FirstOrDefault(s => s.SamplePlanId == plan.Id && s.Name == name) ??
                                       new SamplePlanDetail();
                det.RunId = runid;
                det.Name = name;
                det.DateAndTime = dateAndTime;
                det.IsPrepared = false;
                det.DatePrepared = DateTime.Parse("1/1/1980");
                det.PreparedBy = "";
                det.IsWeighed = false;
                det.DateWeighed = DateTime.Parse("1/1/1980");
                det.WeighedBy = "";
                det.IsSubmitted = false;
                det.DateSubmitted = DateTime.Parse("1/1/1980");
                det.SubmittedBy = userid;

                if (plan.SamplePlanDetails.Contains(det))
                    plan.SamplePlanDetails.Remove(det);
                plan.SamplePlanDetails.Add(det);
            }


            // Make a copy of the master worksheet and store it in the same folder as the Word Doc (input param: fermentationOutlineWordFileInfo)
            FileInfo sampleSubmissionWorksheet = OfficeHandler.FileIO.SampleSubmissionPlan.CopySubmissionMasterToRun(fermentationOutlineWordFileInfo, userid);
            plan.ExcelFileUri = sampleSubmissionWorksheet.FullName;
            await base.UpdateAsync(plan);

            var boolres = StoreSamplePlanInSpreadsheet(sampleSubmissionWorksheet, runid, table, fermentationOutlineWordFileInfo);






            return plan;
        }


        public bool StoreSamplePlanInSpreadsheet(FileInfo sampleSubmissionWorksheet, int runid, DataTable table, FileInfo fermentationOutlineWordFileInfo)
        {
            Run run = DbContext.Runs.FirstOrDefault(r => r.Id == runid);
            Machine machine = DbContext.Machines.FirstOrDefault(m => m.Id == run.MachineId);
            IQueryable<Tank> tanks = DbContext.Tanks.Where(r => r.MachineId == run.MachineId);
            //sheet.SetCellValue(1, "A", "Test value in A1 cell");
            string sheetname = "DataImport";

            Injector injector = new Injector();
            IExcelSheet thesheet = injector.AddSheetToModernFile(sampleSubmissionWorksheet, sheetname);

            // Adding the sheet closes the workbook and therefore disposes of it,
            //  so we need to get a new instance of the handler factory.
            using (var excelHandler = ExcelHandlerFactory.Instance.Create(sampleSubmissionWorksheet.FullName))
            {
                ExcelWorksheet worksheet = null;
                try
                {
                    thesheet = excelHandler.FindSheet(sheetname);

                    // Get the underlying NPPlus model
                    worksheet = (ExcelWorksheet)thesheet.Worksheet;
                    ExcelWorkbook workbook = worksheet.Workbook;
                    Debug.Assert(run != null, "run != null");
                    Debug.Assert(machine != null, "machine != null");
                    workbook.Properties.Title = string.Format("Base Sample Submission for Machine {0}: RunDate {1}", machine.Name, run.Rundate);
                    workbook.Properties.Keywords = string.Format("Fermentation Sample Submission {0}", run.Rundate);
                    workbook.Properties.Company = "Verdezyne, Inc.";

                    //Set column width
                    worksheet.Column(1).Width = 15.0;
                    worksheet.Column(2).Width = 18.0;
                    worksheet.Column(3).Width = 7.0;
                    worksheet.Column(4).Width = 9.0;
                    worksheet.Column(5).Width = 9.0;
                    worksheet.Column(6).Width = 20.0;
                    worksheet.Column(7).Width = 20.0;
                    worksheet.TabColor = Color.SteelBlue;


                    WriteColumnHeadings(table, thesheet);
                    using (ExcelRange r = worksheet.Cells["A1:G1"])
                    {
                        r.Merge = false;
                        r.Style.Font.SetFromFont(new Font("Arial Bold", 16, FontStyle.Regular));
                        r.Style.Font.Color.SetColor(Color.White);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        //r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 13));
                        r.Style.Fill.BackgroundColor.SetColor(Color.SteelBlue);
                    }
                    using (ExcelRange fRange = worksheet.Cells["F1:G130"])
                    {
                        fRange.Style.Numberformat.Format = "0.0000";
                    }

                    int rownumber = 2;
                    foreach (DataRow row in table.Rows)
                    {
                        int gcColumnOrdinal = table.Columns["GC"].Ordinal;
                        double sampleCount = 0;
                        double.TryParse(row.ItemArray[gcColumnOrdinal].ToString(), out sampleCount);
                        if (sampleCount < 1.0)
                        {
                            continue;
                        }

                        foreach (Tank tank in tanks)
                        {
                            // work here add loop for sampleCount > 1
                            for (int i = 0; i < sampleCount; i++)
                            {
                                string cellvalue = row.ItemArray[0].ToString();
                                if (sampleCount > 1)
                                    cellvalue += string.Format("-{0}", i + 1);
                                worksheet.Cells[rownumber, 1].Value = cellvalue;                    // Name
                                //thesheet.SetCellValue(rownumber, 1, row.ItemArray[0].ToString()); // Name
                                thesheet.SetCellValue(rownumber, 2, row.ItemArray[1].ToString());   // Date & Time
                                thesheet.SetCellValue(rownumber, 3, row.ItemArray[gcColumnOrdinal].ToString()); // GC
                                thesheet.SetCellValue(rownumber, 4, tank.Name);                     // Tank
                                thesheet.SetCellValue(rownumber, 5, 0.0);                           // Time
                                thesheet.SetCellValue(rownumber, 6, 0.0);                           // Tare
                                thesheet.SetCellValue(rownumber, 7, 0.0);                           // Net
                                rownumber++;
                            }
                            //var fileInfo = new FileInfo(sampleSubmissionWorksheet.FullName);
                            //ExcelPackage workbook =  new ExcelPackage(fileInfo);
                            //var x = workbook.Load()
                            //ExcelWorksheet worksheet = workbook.Worksheets.FirstOrDefault(sheet => sheet.Name == sheetname);

                        }

                    }
                    rownumber++;
                    string hyperlink = fermentationOutlineWordFileInfo.FullName;

                    worksheet.Cells[rownumber, 1].Hyperlink = new Uri(hyperlink, UriKind.Absolute);
                    using (ExcelRange h = worksheet.Cells[rownumber, 1, rownumber, 1])
                    {
                        h.Merge = false;
                        h.Style.Font.Color.SetColor(Color.Blue);
                        h.Style.Font.UnderLine = true;
                    }



                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }


                excelHandler.Save();
            }

            return true;
        }

        private static void WriteColumnHeadings(DataTable table, IExcelSheet sheet)
        {
            sheet.GetCellValue(1, 1);
            sheet.SetCellValue(1, 1, "Name");

            sheet.SetCellValue(1, 2, table.Columns[1].ColumnName);
            int gcColumnOrdinal = table.Columns["GC"].Ordinal;
            sheet.SetCellValue(1, 3, table.Columns[gcColumnOrdinal].ToString());
            sheet.SetCellValue(1, 4, "Tank");
            sheet.SetCellValue(1, 5, "Time");
            sheet.SetCellValue(1, 7, "Tare (g)");
            sheet.SetCellValue(1, 6, "Net Wght(g)");
        }

        // Test cascading delete.  Cannot cascade so remove details explicitly
        public SamplePlan DeletePlan(int planid)
        {
            var plan = DbContext.SamplePlans.FirstOrDefault(s => s.Id == planid);
            if (plan != null)
            {
                try
                {
                    DbContext.SamplePlanDetails.RemoveRange(DbContext.SamplePlanDetails.Where(s => s.SamplePlanId == planid));
                    DbContext.SamplePlans.Remove(plan);
                    DbContext.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            // It should be null;
            return DbContext.SamplePlans.FirstOrDefault(s => s.Id == planid);
        }

        public SamplePlan DeleteForRun(int runid)
        {
            var plan = DbContext.SamplePlans.FirstOrDefault((s => s.RunId == runid));
            if (plan != null)
            {
                try
                {
                    return DeletePlan(plan.Id);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            // It should be null
            return DbContext.SamplePlans.FirstOrDefault(s => s.RunId == runid);
        }
    }
}