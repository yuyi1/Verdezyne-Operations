using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Operations.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CopyFileOnJamesTest()
        {
            string startingpath = "S:\\Research\\Fermentation\\Project--Darwin\\140729_300L_sAA2178_2.50EL-1.76D_NH4OH_1.5xKA1(4.05% D) 5.25gLammoniumsulfate, HighPhosphate_34C";
            FileInfo fi = OfficeHandler.FileIO.SampleSubmissionPlan.FindWordFileInfo(
                "140729_300L_Outline.docx",
                ref startingpath);

            FileInfo info = OfficeHandler.FileIO.SampleSubmissionPlan.CopySubmissionMasterToRun(fi, "nstein");
            if (info.Name == "140729_300L_Sample_Submission_Nick.xlsx")
            {
                return View("Contact");
            }
            return View("About");
        }
    }
}