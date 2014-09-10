using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FilterPipeline.Filters;
using Operations.Contracts;
using Operations.Models;

namespace Operations.Repository
{
    public class StdCationRepository : GenericAnalyticsRepository<STDCation>, IStdCationRepository
    {
        public StdCationRepository(AnalyticsEntities dbContext)
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

        public IQueryable<STDCation> FindByDate(DateTime? date)
        {
            return DbContext.STDCations.Where(a => a.CalibrationDate == date);
        }

        public Task<List<STDCation>> FindByDateAsync(DateTime? date)
        {
            var ret =  FindAllAsync(a => a.CalibrationDate == date);
            return ret;
        }
        public IQueryable<STDCation> GetLatest()
        {
            ICalibrationStandardRepository csr = new CalibrationStandardRepository(DbContext);
            string name = this.GetType().Name.Replace("Std", string.Empty).Replace("Repository", String.Empty);
            CalibrationStandard cs = csr.FindByName(name);
            return FindByDate(cs.CalibrationDate);
        }
        public async Task<List<STDCation>> GetLatestAsync()
        {
            ICalibrationStandardRepository csr = new CalibrationStandardRepository(DbContext);
            string name = this.GetType().Name.Replace("Std", string.Empty).Replace("Repository", String.Empty);
            CalibrationStandard cs = csr.FindByName(name);
            return await FindByDateAsync(cs.CalibrationDate);
        }
        private async Task<STDCation> SaveRowAsync(DataRow row, CalibrationStandard cs)
        {
            STDCation item;
            var list = FindByDate(cs.CalibrationDate);
            if (!list.Any())
            {
                item = await AddNewItemAsync(row, cs);
            }
            else
            {
                string ident = row.ItemArray[row.Table.Columns["Ident"].Ordinal].ToString();
                item = list.FirstOrDefault(i => i.Ident == ident);
                if (item == null)
                {
                    item = await AddNewItemAsync(row, cs);
                }
                else
                {
                    item = PopulateItemAsync(row, cs, item);
                    int result = await UpdateAsync(item);
                }
            }
            return item;
        }

        private async Task<STDCation> AddNewItemAsync(DataRow row, CalibrationStandard cs)
        {
            STDCation item = new STDCation();
            item = PopulateItemAsync(row, cs, item);
            var result = await AddAsync(item);
            return item;
        }

        private static STDCation PopulateItemAsync(DataRow row, CalibrationStandard cs, STDCation item)
        {
            item.CalibrationStandardId = cs.Id;
            item.CalibrationDate = cs.CalibrationDate;
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
            return item;
        }
    }
}