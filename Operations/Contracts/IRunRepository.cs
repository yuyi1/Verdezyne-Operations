using System.Data.Entity;
using System.Threading.Tasks;
using Operations.Models;
using Operations.ViewModels;
using System.Linq;

namespace Operations.Contracts
{
    public interface IRunRepository : IRepository<Run>
    {
        VmRuns GetRunsDto();
        VmRuns GetReadingsForRun(int runid);
        Task<Run> DeleteRunAsync(Run run);
        EntityState Delete(int runid);
        int DeleteRun(int runid);
        IQueryable<Run> GetRun(int runid);
        Task<Run> GetRunAsync(int runid);
        Task<Run> UpdateOrAddRunAsync(string rundate, Run run);
    }

}
