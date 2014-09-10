using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Operations.DTOs;
//using Operations.Repository;

namespace Operations.Contracts
{
    public interface ISampleDetailsRepository : IRepository<SampleDetailDto>
    {
        SampleDto GetSamplePlanForRun(int runid);
    }
}
