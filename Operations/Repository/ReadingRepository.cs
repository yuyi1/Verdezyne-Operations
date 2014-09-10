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
    public class ReadingRepository : GenericPilotPlantRepository<Reading>, IReadingRepository
    {
        public ReadingRepository(PilotPlantEntities dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<Reading> GetReadingListForEdit(DTOs.ReadingEditDto dto)
        {
            var firstOrDefault = DbContext.Tanks.FirstOrDefault(t => t.Name == dto.Tankname);
            if (firstOrDefault != null)
            {
                var tankid = firstOrDefault.Id;
                var gooddate = System.Convert.ToDateTime("2012-1-1");

                return DbContext.Readings
                    .Where(r => r.RunId == dto.RunId)
                    .Where(r => r.TankId == tankid)
                    .Where(r => r.ReadingDescriptionId == dto.ReadingDescriptionId)
                    .Where(r => r.ReadingDateTime > gooddate)
                    .AsQueryable();
            }
            return null;
        }

        public int DeleteReadingsForRun(int runid)
        {
            var readings = DbContext.Readings.Where(r => r.RunId == runid);
            foreach (Reading r in readings)
            {
                var entitystate = DeleteReading(r.Id);
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
        public async Task<int> DeleteReadingsForRunAsync(int runid)
        {
            var readings = await FindAllAsync(r => r.RunId == runid);
            foreach (Reading r in readings)
            {
                var result = await base.RemoveAsync(r);
            }
            return 1;
        }
        /// <summary>
        /// When a Reading Session is Updated in [HttpPost] ReadingSessionController.Edit, it calls this procedure
        /// which updates all the Reading.ReadingTime to the ReadingSession.EFT
        /// </summary>
        /// <param name="readingSession">ReadingSession - The session being updated</param>
        /// <returns>int - 1</returns>
        public async Task<int> UpdateReadingsForChangedEftAsync(ReadingSession readingSession)
        {
            var readings = await FindAllAsync(r => r.ReadingSessionId == readingSession.Id);
            foreach (Reading r in readings)
            {
                r.ReadingTime = readingSession.EFT;
                await UpdateReadingAsync(r);
            }
            return 1;
        }


        public EntityState DeleteReading(int readingid)
        {
            var reading = DbContext.Readings.FirstOrDefault(r => r.Id == readingid);
            if (reading == null)
                return EntityState.Deleted;
            
            DbContext.Entry(reading).State = EntityState.Deleted;
            return DbContext.Entry(reading).State;
        }


        public async Task<Reading> UpdateReadingAsync(Reading rd)
        {
            if (await base.UpdateAsync(rd) == 1)
                return rd;

            throw new Exception("UpdateReadingAsync failed");

        }

        public IQueryable<Reading> GetReadingsForRun(int runid)
        {
            return DbContext.Readings.Where(r => r.RunId == runid).AsQueryable();
        }
    }
}