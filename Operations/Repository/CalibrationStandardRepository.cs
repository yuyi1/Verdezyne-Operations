using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using FilterPipeline.Filters;
using OfficeHandler.Word.Converters;
using Operations.Contracts;
using Operations.DTOs;
using Operations.Models;

namespace Operations.Repository
{
    public class CalibrationStandardRepository : GenericAnalyticsRepository<CalibrationStandard>, ICalibrationStandardRepository
    {
        public CalibrationStandardRepository(AnalyticsEntities dbContext) : base(dbContext)
        {
        }


        public CalibrationStandard FindByName(string name)
        {
            return DbContext.CalibrationStandards.FirstOrDefault(i => i.CalibrationName == name);
        }

        public async Task<int> CreateCalibrationStandard(CalibrationStandardDto dto)
        {
            CalibrationStandard c = FindByName(dto.CalibrationName);
            if (c == null)
            {
                c = new CalibrationStandard();
                await AddAsync(c);
            }
            c.CalibrationDate = dto.CalibrationDate;
            c.CalibrationName = dto.CalibrationName;
            c.Filename = dto.Filename;
            c.Folder = dto.Folder;
            c.Tablename = "STD" + dto.CalibrationName;

            await UpdateAsync(c);

            // Convert the csv to a DataTable
            CsvToDataTable converter = new CsvToDataTable();
            string path = SharedDriveToUri.Filter(c.Folder);
            if (!path.EndsWith("\\"))
                path += "\\";
            path += c.Filename;
            DataTable table = converter.Convert(path, "STD" + c.CalibrationName);


            switch (c.CalibrationName)
            {
                case "Anion":
                    IStdAnionRepository stdAnionRepository = new StdAnionRepository(DbContext);
                    return await stdAnionRepository.SaveTableAsync(table, c);
                    
                case "Cation":
                    IStdCationRepository stdCationRepository = new StdCationRepository(DbContext);
                    return await stdCationRepository.SaveTableAsync(table, c);
                case "Iron":
                    IStdIronRepository stdIronRepository = new StdIronRepository(DbContext);
                    return await stdIronRepository.SaveTableAsync(table, c);
                default:
                    throw new InvalidDataException("Invalid Calibration Name");
            }

            


        }

    }
}