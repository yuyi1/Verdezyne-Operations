using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Operations.Contracts;
using Operations.Models;

namespace Operations.Repository
{
    public class ProductRepository : GenericPilotPlantRepository<Product>, IProductRepository
    {
        public ProductRepository(PilotPlantEntities dbContext)
            : base(dbContext)
        {
            
        }

        public IQueryable<Product> GetProducts()
        {
            return DbContext.Products.AsQueryable();
        }
    }
}
