using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FilterPipeline.Filters;
using OfficeHandler;
using OfficeHandler.Word.Converters;
using Operations.Config;
using Operations.Contracts;
using Operations.DTOs;
using Operations.Models;
using Operations.RepositoryHelpers;

namespace Operations.Repository
{
    public class SampleIronRepository : GenericAnalyticsRepository<SampleIron>, ISampleIronRepository
    {
        private const string ConnectionStrings = "connectionStrings";
        private const string AppSettings = "appSettings";
        PropertiesParser _appSettings = new PropertiesParser(ConfigFile.GetSection(AppSettings));
        PropertiesParser _connectionStrings = new PropertiesParser(ConfigFile.GetSection(ConnectionStrings));

        public SampleIronRepository(AnalyticsEntities dbContext)
            : base(dbContext)
        {
        }
        public async Task<int> SaveTableAsync(DataTable table, CalibrationStandard cs)
        {
            foreach (DataRow row in table.Rows)
            {
                await SaveRowAsync(row, cs);
            }
            return 1;
        }

        public IQueryable<SampleIron> FindByDate(DateTime? date)
        {
            return DbContext.SampleIrons.Where(a => a.SampleDate == date);
        }
        public IQueryable<SampleIron> FindBySampleSerialNo(string sampleSerialNo)
        {
            return DbContext.SampleIrons.Where(a => a.SampleSerialNo == sampleSerialNo);
        }
        public async Task<List<SampleIron>> ReadSampleFileAsync(string filename)
        {
            string sampleSeralNo = filename.Substring(0, 5);
            FileFinder ff = new FileFinder();
            ff._basefolder = GetBaseFolder();
            ff._filename = filename;
            if (!ff._filename.EndsWith(".csv"))
                ff._filename = ff._filename + ".csv";
            try
            {
                FileInfo fi = ff.GetFileInfo();
                DateTime lastWriteTime = fi.LastWriteTime;
                CsvToDataTable converter = new CsvToDataTable();
                DataTable table = converter.Convert(fi.FullName, "SampleIron");
                CalibrationStandard cs = new CalibrationStandard
                {
                    CalibrationName = filename,
                    CalibrationDate = lastWriteTime,
                    Filename = ff._filename,
                    Folder = ff._basefolder,
                    Tablename = table.TableName
                };
                await SaveTableAsync(table, cs);
                return FindBySampleSerialNo(sampleSeralNo).ToList(); ;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        List<SampleIron> ISampleIronRepository.ReadSampleFile(string filename)
        {
            throw new NotImplementedException();
        }

        public SampleIronDto GetLatest(string sampleSerialNo)
        {

            IStdIronRepository repo = new StdIronRepository(DbContext);
            IQueryable<STDIron> stdIron = repo.GetLatest();
            const int adoublecount = 12;
            var stdArray = FillIronStdArray(stdIron.Count(), adoublecount, stdIron);


            var dbSamples = FindBySampleSerialNo(sampleSerialNo);
            var maxdt = dbSamples.Max(r => r.SampleDate);
            if (maxdt != null)
            {
                IOrderedQueryable<SampleIron> latest = dbSamples
                    .Where(r => r.SampleDate == maxdt && r.Ident.Contains(r.SampleSerialNo))
                    .OrderBy(s => s.Ident);

                List<SampleIron> comparison = new List<SampleIron>();
                List<SampleIron> values = new List<SampleIron>();

                foreach (SampleIron input in latest)
                {

                    SampleIron comparator = new SampleIron();
                    comparator.Id = input.Id;
                    comparator.SampleDate = input.SampleDate == null ? (DateTime)input.SampleDate : new DateTime();
                    comparator.SampleSerialNo = input.SampleSerialNo;
                    comparator.Ident = input.Ident;
                    comparator.Dilution = input.Dilution != null ? (decimal)input.Dilution : (decimal)0.0;

                    stdArray = ApplyComparisonFilter(ref comparator, input, stdArray);

                    comparison.Add(comparator);
                    values.Add(input);


                }
                SampleIronDto dto = new SampleIronDto();
                dto.values = values;
                dto.comparison = comparison;

                return dto;
            }
            return null;
        }

        public async Task<SampleIronDto> GetLatestAsync(string sampleSerialNo)
        {

            IStdIronRepository repo = new StdIronRepository(DbContext);
            IQueryable<STDIron> stdIron = repo.GetLatest();
            const int adoublecount = 12;

            FileFinder ff = new FileFinder();
            ff._basefolder = GetBaseFolder();
            IQueryable<string> filelist = ff.FindByPartialFilename(sampleSerialNo + "*.csv");
            var filename = new FileInfo(filelist.First()).Name;

            List<SampleIron> dbSamples = DbContext.SampleIrons.Where(r => r.SampleSerialNo == sampleSerialNo).ToList();
            if (dbSamples.FirstOrDefault() == null)
            {
                List<SampleIron> x = await ReadSampleFileAsync(filename);
                dbSamples = x.OrderBy(r => System.Convert.ToInt32(r.Ident.Substring(0, r.Ident.IndexOf("_", System.StringComparison.Ordinal)))).ToList();

            }
            else
            {
                dbSamples = dbSamples
                    .OrderBy(r => System.Convert.ToInt32(r.Ident.Substring(0, r.Ident.IndexOf("_", System.StringComparison.Ordinal))))
                    .ToList();
            }

            List<SampleIron> comparison = new List<SampleIron>();
            List<SampleIron> values = dbSamples;
            var stdArray = FillIronStdArray(stdIron.Count(), adoublecount, stdIron);

            foreach (var input in dbSamples)
            {

                SampleIron comparator = new SampleIron();
                comparator.Id = input.Id;
                comparator.SampleDate = input.SampleDate == null ? (DateTime)input.SampleDate : new DateTime();
                comparator.SampleSerialNo = input.SampleSerialNo;
                comparator.Ident = input.Ident;
                comparator.Dilution = input.Dilution != null ? (decimal)input.Dilution : (decimal)0.0;

                stdArray = ApplyComparisonFilter(ref comparator, input, stdArray);

                comparison.Add(comparator);



            }
            SampleIronDto dto = new SampleIronDto { InputFile = filename, values = values, comparison = comparison };

            return dto;

        }

        public async Task<List<SampleIron>> ReadSampleFile(string filename)
        {
            FileFinder ff = new FileFinder();
            ff._basefolder = GetBaseFolder() + this.GetType().Name.Replace("Sample", string.Empty).Replace("Repository", string.Empty);
            ff._filename = filename + ".csv";
            try
            {
                FileInfo fi = ff.GetFileInfo();
                DateTime lastWriteTime = fi.LastWriteTime;
                CsvToDataTable converter = new CsvToDataTable();
                DataTable table = converter.Convert(fi.FullName, "SampleIron");
                CalibrationStandard cs = new CalibrationStandard
                {
                    CalibrationName = filename,
                    CalibrationDate = lastWriteTime,
                    Filename = ff._filename,
                    Folder = ff._basefolder,
                    Tablename = table.TableName
                };
                await SaveTableAsync(table, cs);
                var ret = await GetAllAsync();
                return ret;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }
        //public SampleIronDto GetLatest()
        //{
        //    IStdIronRepository repo = new StdIronRepository(DbContext);
        //    IQueryable<STDIron> stdIron = repo.GetLatest();
        //    const int adoublecount = 12;
        //    var stdArray = FillIronStdArray(stdIron.Count(), adoublecount, stdIron);

        //    var maxdt = DbContext.SampleIrons.Max(r => r.SampleDate);
        //    if (maxdt != null)
        //    {
        //        IOrderedQueryable<SampleIron> latest = DbContext.SampleIrons
        //            .Where(r => r.SampleDate == maxdt && r.Ident.Contains(r.SampleSerialNo))
        //            .OrderBy(s => s.Ident);

        //        List<SampleIron> comparison = new List<SampleIron>();
        //        List<SampleIron> values = new List<SampleIron>();

        //        foreach (SampleIron input in latest)
        //        {

        //            SampleIron comparator = new SampleIron();
        //            comparator.Id = input.Id;
        //            comparator.SampleDate = input.SampleDate == null ? (DateTime)input.SampleDate : new DateTime();
        //            comparator.SampleSerialNo = input.SampleSerialNo;
        //            comparator.Ident = input.Ident;
        //            comparator.Dilution = input.Dilution != null ? (decimal)input.Dilution : (decimal)0.0;

        //            stdArray = ApplyComparisonFilter(ref comparator, input, stdArray);

        //            comparison.Add(comparator);
        //            values.Add(input);


        //        }
        //        SampleIronDto dto = new SampleIronDto();
        //        dto.values = values;
        //        dto.comparison = comparison;

        //        return dto;
        //    }
        //    return null;
        //}

        /// <summary>
        /// Applies the Decimal comparison filter
        /// </summary>
        /// <param name="output">Process.Model.SampleIron - a comparison to get the css class</param>
        /// <param name="input">Process.Model.SampleIron - the values record</param>
        /// <param name="stdaArray">decimal[,] - an array of standards to apply.</param>
        /// <returns>decimal[,] - the standards array</returns>
        private decimal[,] ApplyComparisonFilter(ref SampleIron output, SampleIron input, decimal[,] stdaArray)
        {
            output.Fe__III__Area = DecimalComparisonFilter.Filter(input.Fe__III__Area, 0, ref stdaArray);
            output.Cu_Area = DecimalComparisonFilter.Filter(input.Cu_Area, 1, ref stdaArray);
            output.Ni_Area = DecimalComparisonFilter.Filter(input.Ni_Area, 2, ref stdaArray);
            output.Zn_Area = DecimalComparisonFilter.Filter(input.Zn_Area, 3, ref stdaArray);
            output.Co_Area = DecimalComparisonFilter.Filter(input.Co_Area, 4, ref stdaArray);
            output.Mn_Area = DecimalComparisonFilter.Filter(input.Mn_Area, 5, ref stdaArray);
            output.Fe__III__PPM = DecimalComparisonFilter.Filter(input.Fe__III__Area, 6, ref stdaArray);
            output.Cu_PPM = DecimalComparisonFilter.Filter(input.Cu_Area, 7, ref stdaArray);
            output.Ni_PPM = DecimalComparisonFilter.Filter(input.Ni_Area, 8, ref stdaArray);
            output.Zn_PPM = DecimalComparisonFilter.Filter(input.Zn_Area, 9, ref stdaArray);
            output.Co_PPM = DecimalComparisonFilter.Filter(input.Co_Area, 10, ref stdaArray);
            output.Mn_PPM = DecimalComparisonFilter.Filter(input.Mn_Area, 11, ref stdaArray);
            return stdaArray;
        }

        private static decimal[,] FillIronStdArray(int areccount, int adoublecount, IQueryable<STDIron> std)
        {
            decimal[,] arr = new decimal[areccount, adoublecount];
            int i = 0;
            foreach (STDIron a in std)
            {
                arr[i, 0] = (decimal)a.Fe__III__Area;
                arr[i, 1] = (decimal)a.Cu_Area;
                arr[i, 2] = (decimal)a.Ni_Area;
                arr[i, 3] = (decimal)a.Zn_Area;
                arr[i, 4] = (decimal)a.Co_Area;
                arr[i, 5] = (decimal)a.Mn_Area;
                arr[i, 6] = (decimal)a.Fe__III__PPM;
                arr[i, 7] = (decimal)a.Cu_PPM;
                arr[i, 8] = (decimal)a.Ni_PPM;
                arr[i, 9] = (decimal)a.Zn_PPM;
                arr[i, 10] = (decimal)a.Co_PPM;
                arr[i, 11] = (decimal)a.Mn_PPM;
                i++;
            }
            return arr;
        }

        private async Task<SampleIron> SaveRowAsync(DataRow row, CalibrationStandard cs)
        {
            SampleIron sampleIron = null;
            var list = FindByDate(cs.CalibrationDate);
            if (!list.Any())
            {
                if (string.Empty != GetSampleSerialNoFromIdent(row))
                    sampleIron = await AddNewRowAsync(row, cs);
            }
            else
            {
                if (string.Empty != GetSampleSerialNoFromIdent(row))
                {

                    string ident = row.ItemArray[row.Table.Columns["Ident"].Ordinal].ToString();
                    sampleIron = list.FirstOrDefault(i => i.Ident == ident);
                    if (sampleIron == null)
                    {
                        sampleIron = await AddNewRowAsync(row, cs);
                    }
                    else
                    {
                        sampleIron = PopulateRowAsync(row, cs, sampleIron);
                        await UpdateAsync(sampleIron);
                    }
                }
            }
            return sampleIron;
        }

        private async Task<SampleIron> AddNewRowAsync(DataRow row, CalibrationStandard cs)
        {
            try
            {
                SampleIron item = new SampleIron();
                item = PopulateRowAsync(row, cs, item);
                var result = await AddAsync(item);
                return item;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private static SampleIron PopulateRowAsync(DataRow row, CalibrationStandard cs, SampleIron item)
        {
            item.SampleSerialNo = GetSampleSerialNoFromIdent(row);

            item.SampleDate = cs.CalibrationDate;
            item.Ident = System.Convert.ToString(row.ItemArray[row.Table.Columns["Ident"].Ordinal]);
            item.Dilution = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Dilution"].Ordinal]);
            item.Fe__III__Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Fe (III).Area"].Ordinal]);
            item.Cu_Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Cu.Area"].Ordinal]);
            item.Ni_Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Ni.Area"].Ordinal]);
            item.Zn_Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Zn.Area"].Ordinal]);
            item.Co_Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Co.Area"].Ordinal]);
            item.Mn_Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Mn.Area"].Ordinal]);
            item.Fe__III__PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Fe (III).PPM"].Ordinal]);
            item.Cu_PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Cu PPM"].Ordinal]);
            item.Ni_PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Ni PPM"].Ordinal]);
            item.Zn_PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Zn PPM"].Ordinal]);
            item.Co_PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Co PPM"].Ordinal]);
            item.Mn_PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Mn PPM"].Ordinal]);
            return item;
        }

        private static string GetSampleSerialNoFromIdent(DataRow row)
        {
            string input = System.Convert.ToString(row.ItemArray[row.Table.Columns["Ident"].Ordinal]);
            var pipeline = new Pipeline<string>();
            return pipeline.Register(new SampleSerialNoFromIdentFilter())
                .Execute(input);

        }
        public IQueryable<string> GetFolderCsvAsync()
        {
            FileFinder ff = new FileFinder();
            ff._basefolder = GetBaseFolder();
            IQueryable<string> filelist = ff.FindByPartialFilename("0*.csv");

            return filelist.OrderByDescending(s => s);
        }

        private string GetBaseFolder()
        {
            ICalibrationStandardRepository csr = new CalibrationStandardRepository(DbContext);
            string name = this.GetType().Name.Replace("Sample", string.Empty).Replace("Repository", String.Empty);
            CalibrationStandard cs = csr.FindByName(name);
            return SharedDriveToUri.Filter(cs.Folder);
        }

    }

}