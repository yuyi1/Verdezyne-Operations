using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Operations.Models;

namespace Operations.Contracts
{
    public interface IStdIronRepository : IRepository<STDIron>
    {
        System.Threading.Tasks.Task<int> SaveTableAsync(System.Data.DataTable table, CalibrationStandard c);
        IQueryable<STDIron> FindByDate(DateTime? date);
        IQueryable<STDIron> GetLatest();
        Task<List<STDIron>> GetLatestAsync();
        Task<List<STDIron>> FindByDateAsync(DateTime? date);

        
    }
}