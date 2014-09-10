using System.Threading.Tasks;
using Operations.DTOs;
using Operations.Models;

namespace Operations.Contracts
{
    public interface IOutlineItemRepository : IRepository<OutlineItem>
    {
        OutlineItemsDto GetDtoForRunIdAsync(int runid);
        Task<Run> PersistWordFile(string filename, string startingfolder, string productid, string machineid, string userid);
        Task<int> UpdateOrAddOutlineItemAsync(OutlineItem oi);
    }
}