using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Operations.Contracts;
using Operations.Models;

namespace Operations.Repository
{
    public class OutlineTopicRepository : GenericPilotPlantRepository<OutlineTopic>, IOutlineTopicRepository
    {
        public OutlineTopicRepository(PilotPlantEntities dbContext) : base(dbContext)
        {
        }
    }
}