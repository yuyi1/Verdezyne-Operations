using System;
using System.Threading.Tasks;
using Operations.DTOs;
using Operations.Models;

namespace Operations.Contracts
{
    public interface IOutlineDescriptionRepository : IRepository<OutlineDescription>, IDisposable
    {
        Task<int> AddFromDtoAsync(OutlineDescriptionDto dto);

        Task<OutlineDescriptionDto> GetDtoAsync(int? id);
        OutlineDescription FindByName(string name);
    }
}