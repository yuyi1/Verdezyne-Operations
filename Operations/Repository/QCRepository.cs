using System.Linq;
using Operations.Contracts;
using Operations.Models;

namespace Operations.Repository
{
    public class QcRepository : GenericAnalyticsRepository<QC>, IQCRepository
    {
        public QcRepository(AnalyticsEntities dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<QC> GetQCs()
        {
            return DbContext.QCs.AsQueryable().OrderByDescending(i => i.Id);
        }

        public IQueryable<QC> GetRunningQCsGetUnclosedQCs()
        {
            return DbContext.QCs.Where(n => n.Status == "Submitted").Where(n => n.Status == "Running");
        }
        public IQueryable<QC> GetRunningQCs()
        {
            return DbContext.QCs.Where(n => n.Status == "Running").OrderByDescending(i => i.Id);
        }
    }
}