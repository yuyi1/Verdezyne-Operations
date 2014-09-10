using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Operations.Contracts;
using Operations.Models;

namespace Operations.Repository
{
    public class TankRepository : GenericPilotPlantRepository<Tank>, ITankRepository
    {
        public TankRepository(PilotPlantEntities dbContext)
            : base(dbContext)
        {
        }

        public Tank FindTankByName(string name)
        {
            return DbContext.Tanks.FirstOrDefault(o => o.Name == name);
        }

        public IQueryable<Tank> FindTanksByMachine(int machineId)
        {
            return DbContext.Tanks.Where(t => t.MachineId == machineId);
        }
    }
}