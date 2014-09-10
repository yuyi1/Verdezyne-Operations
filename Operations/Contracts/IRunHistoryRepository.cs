using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Operations.DTOs;
using Operations.Models;

namespace Operations.Contracts
{
    interface IRunHistoryRepository : IRepository<RunHistoryChartDto>
    {
        IQueryable<RunHistoryItemDto> GetRunHistoryItem(int id);
        RunHistoryChartDto GetRunHistoryChart(RunHistory rh);
    }
}
