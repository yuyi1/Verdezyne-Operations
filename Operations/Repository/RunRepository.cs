using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Operations.Contracts;
using Operations.Models;
using Operations.RepositoryHelpers;
using Operations.ViewModels;
using System.Data.Entity;


namespace Operations.Repository
{
    public class RunRepository : GenericPilotPlantRepository<Run>, IRunRepository
    {

        public RunRepository(PilotPlantEntities dbContext)
            : base(dbContext)
        {
        }

        public VmRuns GetRunsDto()
        {
            var model = new VmRuns();
            //var l = DbContext.Run.Where(r => r.RunStatusId > 4 & r.RunStatusId < 11)
            //    .ToList()
            //    .Select(run => DbContext.Reading.SqlQuery(
            //        "select * from reading r where r.runid in ( select id from run where runstatusid between 5 and 10)")
            //        .ToList()
            //        .AsQueryable()
            //        .Where(r => r.RunId == run.Id)
            //        .OrderByDescending(r => r.ReadingDateTime)
            //        .First())
            //    .ToList();
            var l = DbContext.Readings;

            model.Runs = DbContext.Runs.Where(rm => rm.RunStatusId > 4 & rm.RunStatusId < 11).AsQueryable();
            model.Machines = DbContext.Machines.AsQueryable();
            model.GrowthPhases = DbContext.GrowthPhases.AsQueryable();
            model.Products = DbContext.Products.AsQueryable();
            model.RunStatus = DbContext.RunStatus.AsQueryable();
            model.Readings = DbContext.Readings.AsQueryable();

            return model;
        }

        /// <summary>
        /// Gets a VmRuns with Run History for a run
        /// </summary>
        /// <param name="runid">int - the id of the run to retreive</param>
        /// <returns>VmRuns - A view model suitable for passing to a PartialView</returns>
        /// <remarks>It gets the connection string to pass into the Stored Proc from the context as per...
        /// //new SqlConnection(ConfigurationManager.ConnectionStrings["PilotPlantEntities"].ConnectionString)
        /// http://stackoverflow.com/questions/13003797/get-the-entity-framework-connection-string
        /// </remarks>
        public VmRuns GetReadingsForRun(int runid)
        {
            var model = new VmRuns();
            var l = DbContext.Readings.Where(n => n.RunId == runid)
                .ToList();
            try
            {

                PilotPlantStoredProcedure sp = new PilotPlantStoredProcedure(DbContext);
                NameValueCollection parameters = new NameValueCollection
                {
                    {"RunId", runid.ToString(CultureInfo.InvariantCulture)}
                };
                sp.ExecuteNonQueryProcedure("usp_RunHistory_CreateForRunId", parameters);


                //using (SqlConnection conn = new SqlConnection(DbContext.Database.Connection.ConnectionString))
                //{
                //    conn.Open();
                //    SqlCommand cmd = new SqlCommand();
                //    cmd.Connection = conn;
                //    cmd.CommandText = "[usp_RunHistory_CreateForRunId]";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("RunId", runid));
                //    var result = cmd.ExecuteNonQuery();

                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            /*
             * Removed to use EF to run the sp instead.
             */
            //var rh = new PilotPlantBreeze.Data.RunHistory
            //{
            //    RunId = runid,
            //    ConnectionString = DbContext.Database.Connection.ConnectionString
            //};

            //rh.DoCreateForRunId();
            //DataSet ds = rh.DoSelectAll();

            var count = DbContext.RunHistories.Count();

            model.Runs = DbContext.Runs.Where(n => n.Id == runid).AsQueryable();
            model.Machines = DbContext.Machines.AsQueryable();
            model.GrowthPhases = DbContext.GrowthPhases.AsQueryable();
            model.Products = DbContext.Products.AsQueryable();
            model.RunStatus = DbContext.RunStatus.AsQueryable();
            model.Readings = l.AsQueryable();
            model.RunHistory = DbContext.RunHistories.AsQueryable();
            model.ReadingDescriptions = DbContext.ReadingDescriptions.AsQueryable();



            return model;
        }

        public async Task<Run> DeleteRunAsync(Run run)
        {
            IReadingRepository readingRepository = new ReadingRepository(DbContext);
            readingRepository.DeleteReadingsForRun(run.Id);

            IReadingSessionRepository readingSessionRepository = new ReadingSessionRepository(DbContext);
            readingSessionRepository.DeleteReadingSessionsForRun(run.Id);
            var ret = await GetRunAsync(run.Id);
            DeleteRun(run.Id);

            return ret;
        }

        public int DeleteRun(int runid)
        {
            try
            {
                IReadingRepository readingRepository = new ReadingRepository(DbContext);
                readingRepository.DeleteReadingsForRun(runid);

                IReadingSessionRepository readingSessionRepository = new ReadingSessionRepository(DbContext);
                readingSessionRepository.DeleteReadingSessionsForRun(runid);
                var entitystate = Delete(runid);

                ISamplePlanRepository samplePlanRepository = new SamplePlanRepository(DbContext);
                var sampleplan = samplePlanRepository.DeleteForRun(runid);
                
                this.DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return 0;
            }
            return 1;
        }

        public EntityState Delete(int runid)
        {
            var run = DbContext.Runs.FirstOrDefault(r => r.Id == runid);
            DbContext.Entry(run).State = EntityState.Deleted;
            return DbContext.Entry(run).State;
        }

        public IQueryable<Run> GetRun(int runid)
        {
            return DbContext.Runs.Where(r => r.Id == runid).AsQueryable();
        }

        public Task<Run> GetRunAsync(int runid)
        {
            return base.FindAsync(r => r.Id == runid);
        }

        public async Task<Run> UpdateOrAddRunAsync(string rundate, Run run)
        {
            var item = await FindAsync(r => r.Rundate == rundate);
            if (item != null)
            {
                item.Name = run.Name;
                item.Rundate = rundate;
                item.RunStart = run.RunStart;
                item.InductionStart = run.InductionStart;
                item.MachineId = run.MachineId;
                item.ProductId = run.ProductId;
                item.RunStatusId = run.RunStatusId;
                item.Comments = run.Comments;
                item.LastUpdate = DateTime.Now;
                item.UpdateBy = run.UpdateBy;
                await UpdateAsync(item);
                return item;
            }
            else
            {
                await AddAsync(run);
                return run;
            }
        }
    }
}