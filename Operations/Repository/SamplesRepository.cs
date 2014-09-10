using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using FilterPipeline.Filters;
using OfficeHandler.Word;
using OfficeHandler.Word.Tables;
using Operations.Contracts;
using System.Web;
using Operations.Controllers;
using Operations.DTOs;
using Operations.Models;

namespace Operations.Repository
{
    public class SamplesRepository : GenericPilotPlantRepository<SampleDetailDto>, ISampleDetailsRepository
    {
        public SamplesRepository(PilotPlantEntities dbContext)
            : base(dbContext)
        {
        }

        public SampleDto GetSamplePlanForRun(int runid)
        {
            var sample = new SampleDto();
            sample.Id = 1;
            sample.RunId = runid;
            sample.WordFileUri = @"C:\\Users\\nstein\\Documents\\Visual Studio 2013\\Projects\\Process\\Process.UnitTest\\TestFiles\\140618_QA1-D3_Outline.docx";
            sample.ExcelFileUri = sample.WordFileUri.Replace(@"140618_QA1-D3_Outline.docx", "CLE_140509_140506_300L_GC1_LC1.xlsx");
            sample.SampleListDto = new List<SampleDetailDto>();



            SamplingDetailsTable sdtable = new SamplingDetailsTable();
            DataTable table = sdtable.FindTable(sample.WordFileUri);

            Run run = DbContext.Runs.Find(runid);
            var datestarted = DateTime.Parse(run.RunStart.ToString());
            int c = 1;

            foreach (DataRow row in table.Rows)
            {
                var det = new SampleDetailDto();
                det.Id = c;
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


                sample.SampleListDto.Add(det);
                c++;
                datestarted += new TimeSpan(0, 8, 30, 0);
            }

            return sample;
        }

    }
}