using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Operations.Contracts;
using Operations.Models;

namespace Operations.Contracts
{
    public interface IStdCationRepository : IRepository<STDCation>
    {
        System.Threading.Tasks.Task<int> SaveTableAsync(System.Data.DataTable table, CalibrationStandard c);
        IQueryable<STDCation> FindByDate(DateTime? date);
        IQueryable<STDCation> GetLatest();
        Task<List<STDCation>> GetLatestAsync();
        Task<List<STDCation>> FindByDateAsync(DateTime? date);
    }
}