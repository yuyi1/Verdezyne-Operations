using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Operations.DTOs;
using Operations.Models;

namespace Operations.Contracts
{
    public interface ISampleIronRepository : IRepository<SampleIron>
    {
        IQueryable<string> GetFolderCsvAsync();
        IQueryable<SampleIron> FindBySampleSerialNo(string sampleSerialNo);
        Task<List<SampleIron>> ReadSampleFileAsync(string filename);
        List<SampleIron> ReadSampleFile(string filename);
        SampleIronDto GetLatest(string sampleSerialNo);
        Task<SampleIronDto> GetLatestAsync(string sampleSerialNo);
    }
}