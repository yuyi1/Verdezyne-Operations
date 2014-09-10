using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Operations.DTOs;
using Operations.Models;

namespace Operations.Contracts
{
    public interface ISampleAnionRepository : IRepository<SampleAnion>
    {
        IQueryable<string> GetFolderCsvAsync();
        IQueryable<SampleAnion> FindBySampleSerialNo(string sampleSerialNo);
        Task<List<SampleAnion>> ReadSampleFile(string filename);
        SampleAnionDto GetLatest(string SampleSerialNo);
        Task<SampleAnionDto> GetLatestAsync(string SampleSerialNo);
    }
}