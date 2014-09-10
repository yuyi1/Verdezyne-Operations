using System.Linq;
using Operations.Models;

namespace Operations.Contracts
{
    public interface IProductRepository
    {
        IQueryable<Product> GetProducts();
    }
}