using System.Linq;
using Operations.Models;

namespace Operations.ViewModels
{
    public class VmRuns
    {
        public IQueryable<Machine> Machines { get; set; }
        public IQueryable<Run> Runs { get; set; }
        public IQueryable<GrowthPhase> GrowthPhases { get; set; }
        public IQueryable<Product> Products { get; set; }
        public IQueryable<RunStatus> RunStatus { get; set; }
        public IQueryable<Reading> Readings { get; set; }
        public IQueryable<RunHistory> RunHistory { get; set; }
        public IQueryable<ReadingDescription> ReadingDescriptions { get; set; } 
    }
}