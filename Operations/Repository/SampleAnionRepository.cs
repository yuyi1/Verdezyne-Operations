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
    public class SampleAnionRepository : GenericAnalyticsRepository<SampleAnion>, ISampleAnionRepository
    {
        private const string ConnectionStrings = "connectionStrings";
        private const string AppSettings = "appSettings";
        PropertiesParser _appSettings = new PropertiesParser(ConfigFile.GetSection(AppSettings));
        PropertiesParser _connectionStrings = new PropertiesParser(ConfigFile.GetSection(ConnectionStrings));

        public SampleAnionRepository(AnalyticsEntities dbContext)
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
        public IQueryable<SampleAnion> FindByDate(DateTime? date)
        {
            return DbContext.SampleAnions.Where(a => a.SampleDate == date);
        }
        public IQueryable<SampleAnion> FindBySampleSerialNo(string sampleSerialNo)
        {
            return DbContext.SampleAnions.Where(a => a.SampleSerialNo == sampleSerialNo);
        }
        /// <summary>
        /// Reads the csv file from \\\\James\\Research\\Analytical QC
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<List<SampleAnion>> ReadSampleFileAsync(string filename)
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
                DataTable table = converter.Convert(fi.FullName, "SampleAnion");
                CalibrationStandard cs = new CalibrationStandard
                {
                    CalibrationName = filename,
                    CalibrationDate = lastWriteTime,
                    Filename = ff._filename,
                    Folder = ff._basefolder,
                    Tablename = table.TableName
                };
                await SaveTableAsync(table, cs);
                return FindBySampleSerialNo(sampleSeralNo).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<SampleAnion>> ReadSampleFile(string filename)
        {
            FileFinder ff = new FileFinder();
            ff._basefolder = GetBaseFolder() + this.GetType().Name.Replace("Sample", string.Empty).Replace("Repository", string.Empty);
            ff._filename = filename + ".csv";
            try
            {
                FileInfo fi = ff.GetFileInfo();
                DateTime lastWriteTime = fi.LastWriteTime;
                CsvToDataTable converter = new CsvToDataTable();
                DataTable table = converter.Convert(fi.FullName, "SampleAnion");
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
        public SampleAnionDto GetLatest(string sampleSerialNo)
        {

            IStdAnionRepository repo = new StdAnionRepository(DbContext);
            IQueryable<STDAnion> stdAnion = repo.GetLatest();
            const int adoublecount = 14;
            var stdArray = FillAnionStdArray(stdAnion.Count(), adoublecount, stdAnion);


            var DBSamples = FindBySampleSerialNo(sampleSerialNo);
            //if (!DBSamples.Any())
            //{
            //    FileFinder ff = new FileFinder();
            //    ff._basefolder = GetBaseFolder() + this.GetType().Name.Replace("Sample", string.Empty).Replace("Repository", string.Empty);
            //    IQueryable<string> filelist = ff.FindByPartialFilename(sampleSerialNo + "*.csv");
            //    var filename = new FileInfo(filelist.First()).Name;


            //    var x = await ReadSampleFileAsync(filename);
            //    DBSamples = x.AsQueryable();

            //}

            var maxdt = DBSamples.Max(r => r.SampleDate);
            if (maxdt != null)
            {
                IOrderedQueryable<SampleAnion> latest = DBSamples
                    .Where(r => r.SampleDate == maxdt && r.Ident.Contains(r.SampleSerialNo))
                    .OrderBy(s => s.Ident);

                List<SampleAnion> comparison = new List<SampleAnion>();
                List<SampleAnion> values = new List<SampleAnion>();

                foreach (SampleAnion input in latest)
                {

                    SampleAnion comparator = new SampleAnion();
                    comparator.Id = input.Id;
                    comparator.SampleDate = input.SampleDate == null ? (DateTime)input.SampleDate : new DateTime();
                    comparator.SampleSerialNo = input.SampleSerialNo;
                    comparator.Ident = input.Ident;
                    comparator.Dilution = input.Dilution != null ? (decimal)input.Dilution : (decimal)0.0;

                    stdArray = ApplyComparisonFilter(ref comparator, input, stdArray);

                    comparison.Add(comparator);
                    values.Add(input);


                }
                SampleAnionDto dto = new SampleAnionDto();
                dto.values = values;
                dto.comparison = comparison;

                return dto;
            }
            return null;
        }
        public async Task<SampleAnionDto> GetLatestAsync(string sampleSerialNo)
        {

            IStdAnionRepository repo = new StdAnionRepository(DbContext);
            IQueryable<STDAnion> stdAnion = repo.GetLatest();
            const int adoublecount = 14;

            FileFinder ff = new FileFinder();
            ff._basefolder = GetBaseFolder();
            IQueryable<string> filelist = ff.FindByPartialFilename(sampleSerialNo + "*.csv");
            var filename = new FileInfo(filelist.First()).Name;

            List<SampleAnion> dbSamples = DbContext.SampleAnions.Where(r => r.SampleSerialNo == sampleSerialNo).ToList();
            if (dbSamples.FirstOrDefault() == null)
            {
                List<SampleAnion> x = await ReadSampleFileAsync(filename);
                dbSamples = x.OrderBy(r => System.Convert.ToInt32(r.Ident.Substring(0, r.Ident.IndexOf("_", System.StringComparison.Ordinal)))).ToList();
            }
            else
            {
                dbSamples = dbSamples
                    .OrderBy(r => System.Convert.ToInt32(r.Ident.Substring(0, r.Ident.IndexOf("_", System.StringComparison.Ordinal))))
                    .ToList();
            }
            //var maxdt = dbSamples.Max(r => r.SampleDate);
            //if (maxdt != null)
            //{
            //    IOrderedQueryable<SampleAnion> latest = dbSamples
            //        .Where(r => r.SampleDate == maxdt && r.Ident.Contains(r.SampleSerialNo))
            //        .OrderBy(s => s.Ident);

            List<SampleAnion> comparison = new List<SampleAnion>();
            List<SampleAnion> values = dbSamples;
            var stdArray = FillAnionStdArray(stdAnion.Count(), adoublecount, stdAnion);

            foreach (var input in dbSamples)
            {

                SampleAnion comparator = new SampleAnion();
                comparator.Id = input.Id;
                comparator.SampleDate = input.SampleDate == null ? (DateTime)input.SampleDate : new DateTime();
                comparator.SampleSerialNo = input.SampleSerialNo;
                comparator.Ident = input.Ident;
                comparator.Dilution = input.Dilution != null ? (decimal)input.Dilution : (decimal)0.0;

                stdArray = ApplyComparisonFilter(ref comparator, input, stdArray);

                comparison.Add(comparator);
            }
            SampleAnionDto dto = new SampleAnionDto {InputFile = filename, values = values, comparison = comparison};

            return dto;
            //}
            //return null;
        }
        /// <summary>
        /// Applies the Decimal comparison filter
        /// </summary>
        /// <param name="output">Process.Model.SampleAnion - a comparison to get the css class</param>
        /// <param name="input">Process.Model.SampleAnion - the values record</param>
        /// <param name="stdaArray">decimal[,] - an array of standards to apply.</param>
        /// <returns>decimal[,] - the standards array</returns>
        private decimal[,] ApplyComparisonFilter(ref SampleAnion output, SampleAnion input, decimal[,] stdaArray)
        {
            output.F__Area = DecimalComparisonFilter.Filter(input.F__Area, 0, ref stdaArray);
            output.Acet__Area = DecimalComparisonFilter.Filter(input.Acet__Area, 1, ref stdaArray);
            output.Cl__Area = DecimalComparisonFilter.Filter(input.Cl__Area, 2, ref stdaArray);
            output.Br__Area = DecimalComparisonFilter.Filter(input.Br__Area, 3, ref stdaArray);
            output.NO3_Area = DecimalComparisonFilter.Filter(input.NO3_Area, 4, ref stdaArray);
            output.Po4_Area = DecimalComparisonFilter.Filter(input.Po4_Area, 5, ref stdaArray);
            output.So4_Area = DecimalComparisonFilter.Filter(input.So4_Area, 6, ref stdaArray);
            output.F_PPM = DecimalComparisonFilter.Filter(input.F__Area, 7, ref stdaArray);
            output.Acet_PPM = DecimalComparisonFilter.Filter(input.Acet__Area, 8, ref stdaArray);
            output.Cl_PPM = DecimalComparisonFilter.Filter(input.Cl__Area, 9, ref stdaArray);
            output.Br_PPM = DecimalComparisonFilter.Filter(input.Br__Area, 10, ref stdaArray);
            output.No3_PPM = DecimalComparisonFilter.Filter(input.NO3_Area, 11, ref stdaArray);
            output.Po4_PPM = DecimalComparisonFilter.Filter(input.Po4_Area, 12, ref stdaArray);
            output.So4_PPM = DecimalComparisonFilter.Filter(input.So4_Area, 13, ref stdaArray);
            return stdaArray;
        }
        private static decimal[,] FillAnionStdArray(int areccount, int adoublecount, IQueryable<STDAnion> std)
        {
            decimal[,] arr = new decimal[areccount, adoublecount];
            int i = 0;
            foreach (STDAnion a in std)
            {
                arr[i, 0] = (decimal)a.F__Area;
                arr[i, 1] = (decimal)a.Acet__Area;
                arr[i, 2] = (decimal)a.Cl__Area;
                arr[i, 3] = (decimal)a.Br__Area;
                arr[i, 4] = (decimal)a.NO3_Area;
                arr[i, 5] = (decimal)a.Po4_Area;
                arr[i, 6] = (decimal)a.So4_Area;
                arr[i, 7] = (decimal)a.F_PPM;
                arr[i, 8] = (decimal)a.Acet_PPM;
                arr[i, 9] = (decimal)a.Cl_PPM;
                arr[i, 10] = (decimal)a.Br_PPM;
                arr[i, 11] = (decimal)a.No3_PPM;
                arr[i, 12] = (decimal)a.Po4_PPM;
                arr[i, 13] = (decimal)a.So4_PPM;
                i++;
            }
            return arr;
        }
        private async Task<SampleAnion> SaveRowAsync(DataRow row, CalibrationStandard cs)
        {
            SampleAnion sampleAnion = null;
            var list = FindByDate(cs.CalibrationDate);
            if (!list.Any())
            {
                if (string.Empty != GetSampleSerialNoFromIdent(row))
                    sampleAnion = await AddNewRowAsync(row, cs);
                //else
                //{
                //    throw new ObjectNotFoundException(string.Format("The sample serial number was not found in the Ident column of the csv file. ('{0}')", row.ItemArray[row.Table.Columns["Ident"].Ordinal].ToString()));
                //}
            }
            else
            {
                if (string.Empty != GetSampleSerialNoFromIdent(row))
                {
                    string ident = row.ItemArray[row.Table.Columns["Ident"].Ordinal].ToString();
                    sampleAnion = list.FirstOrDefault(i => i.Ident == ident);
                    if (sampleAnion == null)
                    {
                        sampleAnion = await AddNewRowAsync(row, cs);
                    }
                    else
                    {
                        sampleAnion = PopulateRowAsync(row, cs, sampleAnion);
                        await UpdateAsync(sampleAnion);
                    }
                }
                //else
                //{
                //    throw new ObjectNotFoundException(string.Format("The sample serial number was not found in the Ident column of the csv file. ('{0}')", row.ItemArray[row.Table.Columns["Ident"].Ordinal].ToString()));
                //}
            }
            return sampleAnion;
        }
        private async Task<SampleAnion> AddNewRowAsync(DataRow row, CalibrationStandard cs)
        {
            SampleAnion item = new SampleAnion();
            item = PopulateRowAsync(row, cs, item);
            var result = await AddAsync(item);
            return item;
        }
        private static SampleAnion PopulateRowAsync(DataRow row, CalibrationStandard cs, SampleAnion item)
        {
            item.SampleSerialNo = GetSampleSerialNoFromIdent(row);

            //item.SampleSerialNo = cs.CalibrationName.Substring(0, 5);
            item.SampleDate = cs.CalibrationDate;
            item.Ident = System.Convert.ToString(row.ItemArray[row.Table.Columns["Ident"].Ordinal]);
            item.Dilution = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Dilution"].Ordinal]);
            item.F__Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["F .Area"].Ordinal]);
            item.Acet__Area =
                DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Acet .Area"].Ordinal]);
            item.Cl__Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Cl .Area"].Ordinal]);
            item.Br__Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Br. Area"].Ordinal]);
            item.NO3_Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["NO3.Area"].Ordinal]);
            item.Po4_Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Po4.Area"].Ordinal]);
            item.So4_Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["So4.Area"].Ordinal]);
            item.F_PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["F PPM"].Ordinal]);
            item.Acet_PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Acet PPM"].Ordinal]);
            item.Cl_PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Cl PPM"].Ordinal]);
            item.Br_PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Br PPM"].Ordinal]);
            item.No3_PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["No3 PPM"].Ordinal]);
            item.Po4_PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Po4 PPM"].Ordinal]);
            item.So4_PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["So4 PPM"].Ordinal]);
            item.SpreadsheetLink = string.Empty;
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
        //    return _appSettings.GetStringProperty("Analytics.QC.BaseFolder");
            ICalibrationStandardRepository csr = new CalibrationStandardRepository(DbContext);
            string name = this.GetType().Name.Replace("Sample", string.Empty).Replace("Repository", String.Empty);
            CalibrationStandard cs = csr.FindByName(name);
            return SharedDriveToUri.Filter(cs.Folder);
        }
    }
}