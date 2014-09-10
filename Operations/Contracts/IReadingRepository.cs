using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Operations.Models;

namespace Operations.Contracts
{
    public interface IReadingRepository : IRepository<Reading>
    {
        int DeleteReadingsForRun(int runid);
        IQueryable<Reading> GetReadingsForRun(int runid);
        Task<int> DeleteReadingsForRunAsync(int runid);
        Task<int> UpdateReadingsForChangedEftAsync(ReadingSession readingSession);
    }
}
