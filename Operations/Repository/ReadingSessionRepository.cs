using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Operations.Contracts;
using Operations.Models;


namespace Operations.Repository
{
    public class ReadingSessionRepository : GenericPilotPlantRepository<ReadingSession>, IReadingSessionRepository
    {

        public ReadingSessionRepository(PilotPlantEntities dbContext)
            : base(dbContext)
        {
        }


        public async Task<int> DeleteReadingSessionsForRunAsync(int runid)
        {
            var sessions = DbContext.ReadingSessions.Where(r => r.RunId == runid);
            IReadingRepository readingRepository = new ReadingRepository(DbContext);
            var ret = await readingRepository.DeleteReadingsForRunAsync(runid);


            foreach (ReadingSession r in sessions)
            {
                await base.RemoveAsync(r);
            }
            return 1;
        }


        public int DeleteReadingSessionsForRun(int runid)
        {

            var sessions = DbContext.ReadingSessions.Where(r => r.RunId == runid);


            IReadingRepository readingRepository = new ReadingRepository(DbContext);
            int count = DbContext.Readings.Count(r => r.RunId == runid);
            {
                if (count > 0)
                    readingRepository.DeleteReadingsForRun(runid);
            }

            foreach (ReadingSession r in sessions)
            {
                var entitystate = DeleteReadingSession(r.Id);
            }
            try
            {
                this.DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return 0;
            }
            return 1;
        }

        public EntityState DeleteReadingSession(int readingsessionid)
        {
            var session = DbContext.ReadingSessions.FirstOrDefault(r => r.Id == readingsessionid);
            if (session == null)
                return EntityState.Deleted;

            DbContext.Entry(session).State = EntityState.Deleted;
            return DbContext.Entry(session).State;
        }

        public IQueryable<ReadingSession> GetReadingSessionsForRun(int runid)
        {
            return DbContext.ReadingSessions.Where(r => r.RunId == runid).AsQueryable();
        }

        public async Task<ReadingSession> UpdateReadingSessionAsync(ReadingSession rd)
        {
            if (await base.UpdateAsync(rd) == 1)
                return rd;

            throw new Exception("UpdateReadingAsync failed");
        }

        public async Task<ReadingSession> GetReadingSessionByIdAsync(int id)
        {
            var result = await FindAsync(s => s.Id == id);
            return result;
        }

        public async Task<List<ReadingSession>> GetReadingSessionsForRunAsync(int runid)
        {
            var result = await FindAllAsync(s => s.RunId == runid);
            return result;
        }

        public async Task<int> UpdateReadingsForChangedEft(ReadingSession readingSession)
        {
            IReadingRepository repo = new ReadingRepository(this.DbContext);
            return await repo.UpdateReadingsForChangedEftAsync(readingSession);

        }
    }
}