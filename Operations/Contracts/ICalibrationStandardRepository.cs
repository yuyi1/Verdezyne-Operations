using System.Threading.Tasks;
using Operations.DTOs;
using Operations.Models;

namespace Operations.Contracts
{
    public interface ICalibrationStandardRepository : IRepository<CalibrationStandard>
    {
        CalibrationStandard FindByName(string name);
        Task<int> CreateCalibrationStandard(CalibrationStandardDto dto);
    }
}