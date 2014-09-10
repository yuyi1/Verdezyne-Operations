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
    public class StdIronRepository : GenericAnalyticsRepository<STDIron>, IStdIronRepository
    {
        public StdIronRepository(AnalyticsEntities dbContext)
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

        public IQueryable<STDIron> FindByDate(DateTime? date)
        {
            return DbContext.STDIrons.Where(a => a.CalibrationDate == date);
        }
        public Task<List<STDIron>> FindByDateAsync(DateTime? date)
        {
            var ret = FindAllAsync(a => a.CalibrationDate == date);
            return ret;
        }
        public IQueryable<STDIron> GetLatest()
        {
            ICalibrationStandardRepository csr = new CalibrationStandardRepository(DbContext);
            string name = this.GetType().Name.Replace("Std", string.Empty).Replace("Repository", String.Empty);
            CalibrationStandard cs = csr.FindByName(name);
            return FindByDate(cs.CalibrationDate);
        }

        public Task<List<STDIron>> GetLatestAsync()
        {
            ICalibrationStandardRepository csr = new CalibrationStandardRepository(DbContext);
            string name = this.GetType().Name.Replace("Std", string.Empty).Replace("Repository", String.Empty);
            CalibrationStandard cs = csr.FindByName(name);
            return FindByDateAsync(cs.CalibrationDate);
        }



        private async Task<STDIron> SaveRowAsync(DataRow row, CalibrationStandard cs)
        {
            STDIron item;
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
                }
            }
            return item;
        }

        private async Task<STDIron> AddNewItemAsync(DataRow row, CalibrationStandard cs)
        {
            STDIron item = new STDIron();
            item = PopulateItemAsync(row, cs, item);
            var result = await AddAsync(item);
            return item;
        }

        private static STDIron PopulateItemAsync(DataRow row, CalibrationStandard cs, STDIron item)
        {
            item.CalibrationStandardId = cs.Id;
            item.CalibrationDate = cs.CalibrationDate;
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
    }
}