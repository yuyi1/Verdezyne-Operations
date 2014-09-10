using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Operations.Models;

namespace Operations.Contracts
{
    public interface IStdAnionRepository : IRepository<STDAnion>
    {
        Task<int> SaveTableAsync(DataTable table, CalibrationStandard cs);
        IQueryable<STDAnion> FindByDate(DateTime? date);
        IQueryable<STDAnion> GetLatest();
        Task<List<STDAnion>> GetLatestAsync();
        Task<List<STDAnion>> FindByDateAsync(DateTime? date);

    }
}