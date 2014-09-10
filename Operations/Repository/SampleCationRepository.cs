using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FilterPipeline.Filters;
using Microsoft.Ajax.Utilities;
using OfficeHandler;
using OfficeHandler.Word.Converters;
using Operations.Config;
using Operations.Contracts;

using Operations.DTOs;
using Operations.Models;
using Operations.RepositoryHelpers;

namespace Operations.Repository
{
    public class SampleCationRepository : GenericAnalyticsRepository<SampleCation>, ISampleCationRepository
    {
        private const string ConnectionStrings = "connectionStrings";
        private const string AppSettings = "appSettings";
        PropertiesParser _appSettings = new PropertiesParser(ConfigFile.GetSection(AppSettings));
        PropertiesParser _connectionStrings = new PropertiesParser(ConfigFile.GetSection(ConnectionStrings));

        public SampleCationRepository(AnalyticsEntities dbContext)
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
        public IQueryable<SampleCation> FindByDate(DateTime? date)
        {
            return DbContext.SampleCations.Where(a => a.SampleDate == date);
        }
        public IQueryable<SampleCation> FindBySampleSerialNo(string sampleSerialNo)
        {
            return DbContext.SampleCations.Where(a => a.SampleSerialNo == sampleSerialNo);
        }
        /// <summary>
        /// Reads the csv file from \\\\James\\Research\\Analytical QC
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<List<SampleCation>> ReadSampleFileAsync(string filename)
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
                DataTable table = converter.Convert(fi.FullName, "SampleCation");
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
        public List<SampleCation> ReadSampleFile(string filename)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the Latest additions to the SampleCation Table and the comparison list for determining the css class to be applied. 
        /// </summary>
        /// <returns>SampleCationDto - the data transfer object with a list of values and comparisons</returns>
        public SampleCationDto GetLatest(string sampleSerialNo)
        {

            IStdCationRepository repo = new StdCationRepository(DbContext);
            IQueryable<STDCation> stdCation = repo.GetLatest();
            const int adoublecount = 12;
            var stdArray = FillCationStdArray(stdCation.Count(), adoublecount, stdCation);


            var dbSamples = FindBySampleSerialNo(sampleSerialNo);


            var maxdt = dbSamples.Max(r => r.SampleDate);
            if (maxdt != null)
            {
                IOrderedQueryable<SampleCation> latest = dbSamples
                    .Where(r => r.SampleDate == maxdt && r.Ident.Contains(r.SampleSerialNo))
                    .OrderBy(s => s.Ident);

                List<SampleCation> comparison = new List<SampleCation>();
                List<SampleCation> values = new List<SampleCation>();

                foreach (SampleCation input in latest)
                {

                    SampleCation comparator = new SampleCation();
                    comparator.Id = input.Id;
                    comparator.SampleDate = input.SampleDate == null ? (DateTime)input.SampleDate : new DateTime();
                    comparator.SampleSerialNo = input.SampleSerialNo;
                    comparator.Ident = input.Ident;
                    comparator.Dilution = input.Dilution != null ? (decimal)input.Dilution : (decimal)0.0;

                    stdArray = ApplyComparisonFilter(ref comparator, input, ref stdArray);

                    comparison.Add(comparator);
                    values.Add(input);


                }
                SampleCationDto dto = new SampleCationDto();
                dto.values = values;
                dto.comparison = comparison;

                return dto;
            }
            return null;
        }
        public async Task<SampleCationDto> GetLatestAsync(string sampleSerialNo)
        {

            IStdCationRepository repo = new StdCationRepository(DbContext);
            IQueryable<STDCation> stdCation = repo.GetLatest();
            const int adoublecount = 12;

            FileFinder ff = new FileFinder();
            ff._basefolder = GetBaseFolder();
            IQueryable<string> filelist = ff.FindByPartialFilename(sampleSerialNo + "*.csv");
            var filename = new FileInfo(filelist.First()).Name;


            List<SampleCation> dbSamples = DbContext.SampleCations.Where(r => r.SampleSerialNo == sampleSerialNo).ToList();
            if (dbSamples.FirstOrDefault() == null)
            {
                List<SampleCation> x = await ReadSampleFileAsync(filename);
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
            //    IOrderedQueryable<SampleCation> latest = dbSamples
            //        .Where(r => r.SampleDate == maxdt && r.Ident.Contains(r.SampleSerialNo))
            //        .OrderBy(s => s.Ident);

            List<SampleCation> comparison = new List<SampleCation>();
            List<SampleCation> values = dbSamples;
            var stdArray = FillCationStdArray(stdCation.Count(), adoublecount, stdCation);

            foreach (var input in dbSamples)
            {

                SampleCation comparator = new SampleCation();
                comparator.Id = input.Id;
                comparator.SampleDate = input.SampleDate == null ? (DateTime)input.SampleDate : new DateTime();
                comparator.SampleSerialNo = input.SampleSerialNo;
                comparator.Ident = input.Ident;
                comparator.Dilution = input.Dilution != null ? (decimal)input.Dilution : (decimal)0.0;

                stdArray = ApplyComparisonFilter(ref comparator, input, ref stdArray);

                comparison.Add(comparator);
                


            }
            SampleCationDto dto = new SampleCationDto {InputFile = filename, values = values, comparison = comparison};

            return dto;
            //}
            //return null;
        }
        /// <summary>
        /// Calls the Filter for each decimal property in the Model class.
        /// </summary>
        /// <param name="output">SampleCation - the Model class with the ViewConstants set for applying a class</param>
        /// <param name="input">SampleCation - the values class</param>
        /// <param name="stdaArray"> decimal[,] - the standards as an array</param>
        /// <returns> decimal[,] - the standards array</returns>
        private decimal[,] ApplyComparisonFilter(ref SampleCation output, SampleCation input, ref decimal[,] stdaArray)
        {
            output.Li_Area = DecimalComparisonFilter.Filter(input.Li_Area, 0, ref stdaArray);
            output.Na_Area = DecimalComparisonFilter.Filter(input.Na_Area, 1, ref stdaArray);
            output.NH4__Area = DecimalComparisonFilter.Filter(input.NH4__Area, 2, ref stdaArray);
            output.K_Area = DecimalComparisonFilter.Filter(input.K_Area, 3, ref stdaArray);
            output.Ca_2__Area = DecimalComparisonFilter.Filter(input.Ca_2__Area, 4, ref stdaArray);
            output.Mg_2__Area = DecimalComparisonFilter.Filter(input.Mg_2__Area, 5, ref stdaArray);
            output.Li__PPM = DecimalComparisonFilter.Filter(input.Li_Area, 6, ref stdaArray);
            output.Na__PPM = DecimalComparisonFilter.Filter(input.Na_Area, 7, ref stdaArray);
            output.NH4___PPM = DecimalComparisonFilter.Filter(input.NH4__Area, 8, ref stdaArray);
            output.K__PPM = DecimalComparisonFilter.Filter(input.K_Area, 9, ref stdaArray);
            output.Ca__PPM = DecimalComparisonFilter.Filter(input.Ca_2__Area, 10, ref stdaArray);
            output.Mg__PPM = DecimalComparisonFilter.Filter(input.Mg_2__Area, 11, ref stdaArray);
            return stdaArray;
        }
        private decimal[,] FillCationStdArray(int areccount, int adoublecount, IQueryable<STDCation> stdCation)
        {
            decimal[,] arr = new decimal[areccount, adoublecount];
            int i = 0;
            foreach (STDCation a in stdCation)
            {
                arr[i, 0] = (decimal)a.Li_Area;
                arr[i, 1] = (decimal)a.Na_Area;
                arr[i, 2] = (decimal)a.NH4__Area;
                arr[i, 3] = (decimal)a.K_Area;
                arr[i, 4] = (decimal)a.Ca_2__Area;
                arr[i, 5] = (decimal)a.Mg_2__Area;
                arr[i, 6] = (decimal)a.Li__PPM;
                arr[i, 7] = (decimal)a.Na__PPM;
                arr[i, 8] = (decimal)a.NH4___PPM;
                arr[i, 9] = (decimal)a.K__PPM;
                arr[i, 10] = (decimal)a.Ca__PPM;
                arr[i, 11] = (decimal)a.Mg__PPM;
                i++;
            }
            return arr;
        }
        /// <summary>
        /// Saves a row from the table to the database
        /// </summary>
        /// <param name="row">DataRow - the row from the DataTable created from the csv file</param>
        /// <param name="cs">CalibrationStandard - the latest standard</param>
        /// <returns>Task<SampleCation> - The item which was added/updated to the database</returns>
        private async Task<SampleCation> SaveRowAsync(DataRow row, CalibrationStandard cs)
        {
            SampleCation sampleCation = null;
            var list = FindByDate(cs.CalibrationDate);
            if (!list.Any())
            {
                if (string.Empty != GetSampleSerialNoFromIdent(row))
                    sampleCation = await AddNewRowAsync(row, cs);
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
                    sampleCation = list.FirstOrDefault(i => i.Ident == ident);
                    if (sampleCation == null)
                    {
                        sampleCation = await AddNewRowAsync(row, cs);
                    }
                    else
                    {
                        sampleCation = PopulateRowAsync(row, cs, sampleCation);
                        await UpdateAsync(sampleCation);
                    }
                }
                //else
                //{
                //    throw new ObjectNotFoundException(string.Format("The sample serial number was not found in the Ident column of the csv file. ('{0}')", row.ItemArray[row.Table.Columns["Ident"].Ordinal].ToString()));
                //}
            }
            return sampleCation;
        }
        private async Task<SampleCation> AddNewRowAsync(DataRow row, CalibrationStandard cs)
        {
            SampleCation item = new SampleCation();
            item = PopulateRowAsync(row, cs, item);
            var result = await AddAsync(item);
            return item;
        }
        /// <summary>
        /// Applies the DataColumnToDecimalFilter to each column in the DataRow and translates it into a SampleCation Model
        /// </summary>
        /// <param name="row">DataRow - the row from the table created from the csv file</param>
        /// <param name="cs">CalibrationStandard - The standard used to pass the GetSampleSerialNoFromIdent and CalibrationDate</param>
        /// <param name="item">The empty SampleCation to be populated</param>
        /// <returns>SampleCation - the populated item. </returns>
        private static SampleCation PopulateRowAsync(DataRow row, CalibrationStandard cs, SampleCation item)
        {
            item.SampleSerialNo = GetSampleSerialNoFromIdent(row);

            item.SampleDate = cs.CalibrationDate;
            item.Ident = System.Convert.ToString(row.ItemArray[row.Table.Columns["Ident"].Ordinal]);
            item.Dilution = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Dilution"].Ordinal]);
            item.Li_Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Li.Area"].Ordinal]);
            item.Na_Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Na.Area"].Ordinal]);
            item.NH4__Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["NH4+.Area"].Ordinal]);
            item.K_Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["K.Area"].Ordinal]);
            item.Mg_2__Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Mg 2+.Area"].Ordinal]);
            item.Ca_2__Area = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Ca 2+.Area"].Ordinal]);
            item.Li__PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Li. PPM"].Ordinal]);
            item.Na__PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Na. PPM"].Ordinal]);
            item.NH4___PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["NH4+. PPM"].Ordinal]);
            item.K__PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["K. PPM"].Ordinal]);
            item.Mg__PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Mg. PPM"].Ordinal]);
            item.Ca__PPM = DataColumnToDecimalFilter.Process(row.ItemArray[row.Table.Columns["Ca. PPM"].Ordinal]);
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
            ICalibrationStandardRepository csr = new CalibrationStandardRepository(DbContext);
            string name = this.GetType().Name.Replace("Sample", string.Empty).Replace("Repository", String.Empty);
            CalibrationStandard cs = csr.FindByName(name);
            return SharedDriveToUri.Filter(cs.Folder);
        }
    }
}