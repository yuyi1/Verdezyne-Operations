using System;
using System.Linq;
using Operations.Models;

namespace Operations.Contracts
{
    public interface IMachineRepository : IRepository<Machine>, IDisposable
    {
        IQueryable<Machine> GetMachines();

    }
}