using System.Collections.Generic;
using System.Linq;
using Operations.Models;

namespace Operations.DTOs
{
    public class RunHistoryChartDto : RunHistoryItemDto
    {
        public List<List<string>> Series { get; set; }
        public List<string> Categorys { get; set; }
        public IQueryable<Tank> Tanks { get; set; }
        public List<Reading> Readings { get; set; }
        public IQueryable<ReadingDescription> ReadingDescriptions { get; set; }

        public RunHistoryChartDto()
        {
            Series = new List<List<string>>();
            Categorys = new List<string>();
            Readings=new List<Reading>();
        }
    }
}