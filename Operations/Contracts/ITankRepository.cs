using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Operations.Models;

namespace Operations.Contracts
{
    interface ITankRepository : IRepository<Tank>
    {
        Tank FindTankByName(string name);
        IQueryable<Tank> FindTanksByMachine(int machineId);
    }
}
