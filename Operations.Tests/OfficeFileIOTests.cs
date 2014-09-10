using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Operations.Models;
using Operations.Repository;

namespace Operations.UnitTest
{
    [TestClass]
    public class OfficeFileIoTests
    {
        [TestMethod]
        public void ItShouldReturnAFileInfo_ForTheMasterSubmissionFile()
        {
            var result = OfficeHandler.FileIO.SampleSubmissionPlan.GetFileInfoForMasterSampleSubmissionFile();
            Assert.IsInstanceOfType(result, typeof(FileInfo));
            Assert.IsTrue(result.FullName.Contains("Sample Submission V"));
        }

        [TestMethod]
        public void ItShouldCopyAndRename_TheSampleSubmissionMaster_ToTheDocxFolder()
        {
            try
            {
                var run = new RunRepository(new PilotPlantEntities()).DbContext.Runs.FirstOrDefault(r => r.Id == 9);

            }
            catch (Exception ex)
            {

                throw;
            }
            string startingpath =
                "S:\\Research\\Fermentation\\Project--Darwin\\140729_300L_sAA2178_2.50EL-1.76D_NH4OH_1.5xKA1(4.05% D) 5.25gLammoniumsulfate, HighPhosphate_34C";
            FileInfo fi = OfficeHandler.FileIO.SampleSubmissionPlan.FindWordFileInfo(
                "140729_300L_Outline.docx",
                ref startingpath);

            FileInfo retval = OfficeHandler.FileIO.SampleSubmissionPlan.CopySubmissionMasterToRun(fi, "nstein");
            Assert.IsTrue(retval.Name == "140729_300L_Sample_Submission_Nick.xlsx");
        }


    }
}
