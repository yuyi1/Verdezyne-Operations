using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Operations.Models;

namespace Operations.Contracts
{
    public interface IReadingSessionRepository : IRepository<ReadingSession>
    {
        Task<int> DeleteReadingSessionsForRunAsync(int runid);
        int DeleteReadingSessionsForRun(int runid);
        EntityState DeleteReadingSession(int readingsessionid);
        IQueryable<ReadingSession> GetReadingSessionsForRun(int runid);
        Task<ReadingSession> UpdateReadingSessionAsync(ReadingSession rd);
        Task<ReadingSession> GetReadingSessionByIdAsync(int id);
        Task<List<ReadingSession>> GetReadingSessionsForRunAsync(int runid);

    }
}
