using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operations.Contracts
{
    public interface IEftCalculator
    {
        System.Double CalculateEft(DateTime? startdate, string endDate);
        System.Double CalculateEft(string startdate, string endDate);
        System.Double CalculateEft(DateTime startdate, DateTime endDate);

    }
}
