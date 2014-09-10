using System.Linq;
using Operations.Contracts;
using Operations.Models;


namespace Operations.Repository
{
    public class SequenceRepository : GenericAnalyticsRepository<Sequence>, ISequenceRepository
    {
        public SequenceRepository(AnalyticsEntities dbContext) : base(dbContext)
        {
        }

        public IQueryable<Sequence> GetSequences()
        {
            return DbContext.Sequences.AsQueryable();
        }
        public IQueryable<Sequence> GetGetUnclosedSequences()
        {
            return DbContext.Sequences.Where(n => n.Status == "Submitted")
                .Where(n => n.Status == "Running")
                .AsQueryable();
        }
        public IQueryable<Sequence> GetRunningSequences()
        {
            return DbContext.Sequences.Where(n => n.Status == "Running")
                .AsQueryable();
        }
    }
}