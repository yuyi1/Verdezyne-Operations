using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FilterPipeline.Filters;
using Operations.Contracts;

using Operations.Models;
using Operations.ControllerHelpers;

namespace Operations.Repository
{
    public class SamplePlanDetailsRepository : GenericPilotPlantRepository<SamplePlanDetail>, ISamplePlanDetailsRepository
    {
        public SamplePlanDetailsRepository(PilotPlantEntities dbContext) : base(dbContext)
        {
        }
        public async Task<SamplePlanDetail> UpdateCheckBox(UpdateCheckboxPostParameters parameters)
        {
            Pipeline<string> pipeline = new Pipeline<string>();
            string strid = pipeline.Register(new NonNumericFilter())
                .Execute(parameters.id);

            int sampleid = 0;
            var success = int.TryParse(strid, out sampleid);

            SamplePlanDetail det = await FindAsync(e => e.Id == sampleid);
            if (det == null)
                throw new ObjectNotFoundException(string.Format(@"No SamplePlanDetail Record found in the database for Id = {0}", sampleid));
            switch (parameters.name)
            {
                case "r.IsPrepared":
                    det.IsPrepared = parameters.ischecked;
                    det.DatePrepared = DateTime.Now;
                    det.PreparedBy = parameters.currentverdezyneuser;        
                    break;
                case "r.IsWeighed":
                    det.IsWeighed = parameters.ischecked;
                    det.DateWeighed = DateTime.Now;
                    det.WeighedBy = parameters.currentverdezyneuser;        
                    break;
                case "r.IsSubmitted":
                    det.IsSubmitted = parameters.ischecked;
                    det.DateSubmitted = DateTime.Now;
                    det.SubmittedBy = parameters.currentverdezyneuser;        
                    break;
                default:
                    throw new Exception("Invalid checkbox name.");
             
            }
            
            var result = await UpdateAsync(det);
            return det;
        }

        public Task DeleteForRunID(int runid)
        {
            throw new NotImplementedException();
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