using System.IO;
using System.Threading.Tasks;
using Operations.Models;

namespace Operations.Contracts
{
    public interface ISamplePlanRepository
    {
        Task<SamplePlan> GetSamplePlanForRunAsync(int runid);
        Task<SamplePlan> SaveTrackingDataAsync(int runid, FileInfo fermentationOutlineWordFileInfo, string userid);
        SamplePlan DeletePlan(int planid);
        Task<SamplePlan> SeedDatabase(int runid);
        SamplePlan DeleteForRun(int runid);
    }
}