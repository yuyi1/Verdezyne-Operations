using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Operations.Contracts;
using Operations.Models;

namespace Operations.Repository
{
    public class MachineRepository : GenericPilotPlantRepository<Machine>, IMachineRepository
    {
        public MachineRepository(PilotPlantEntities dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Looks for Machine Names in the passed string.  If one is found it returns the Id
        /// </summary>
        /// <param name="name">string - a name which contains the machine name</param>
        /// <returns>int - the Id of the machine found</returns>
        public int GetMachineIdFromName(string name)
        {
            try
            {
                var x = DbContext.Machines.FirstOrDefault(m => name.Contains(m.Name));
                return x.Id;
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(string.Format("Machine Not Found from {0}. {1}", name, ex.Message));
            }
            
        }

        public IQueryable<Machine> GetMachines()
        {
            return DbContext.Machines.AsQueryable();
        }
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}