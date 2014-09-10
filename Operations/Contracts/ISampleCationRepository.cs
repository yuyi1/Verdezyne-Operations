using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Operations.DTOs;
using Operations.Models;

namespace Operations.Contracts
{
    public interface ISampleCationRepository : IRepository<SampleCation>
    {
        IQueryable<string> GetFolderCsvAsync();
        IQueryable<SampleCation> FindBySampleSerialNo(string sampleSerialNo);
        Task<List<SampleCation>> ReadSampleFileAsync(string filename);
        List<SampleCation> ReadSampleFile(string filename);
        SampleCationDto GetLatest(string sampleSerialNo);
        Task<SampleCationDto> GetLatestAsync(string sampleSerialNo);
    }
}