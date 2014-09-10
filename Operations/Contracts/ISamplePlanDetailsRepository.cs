using System;
using System.Threading.Tasks;
using Operations.ControllerHelpers;
using Operations.Models;

namespace Operations.Contracts
{
    public interface ISamplePlanDetailsRepository : IRepository<SamplePlanDetail>, IDisposable
    {
        Task<SamplePlanDetail> UpdateCheckBox(UpdateCheckboxPostParameters parameters);
        Task DeleteForRunID(int runid);

    }
}