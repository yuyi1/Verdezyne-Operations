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
    public class StdAnionRepository : GenericAnalyticsRepository<STDAnion>, IStdAnionRepository
    {
        public StdAnionRepository(AnalyticsEntities dbContext)
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

        public IQueryable<STDAnion> FindByDate(DateTime? date)
        {
            return DbContext.STDAnions.Where(a => a.CalibrationDate == date);
        }
        public Task<List<STDAnion>> FindByDateAsync(DateTime? date)
        {
            return FindAllAsync(a => a.CalibrationDate == date);
            
        }
        public IQueryable<STDAnion> GetLatest()
        {
            ICalibrationStandardRepository csr = new CalibrationStandardRepository(DbContext);
            string name = this.GetType().Name.Replace("Std", string.Empty).Replace("Repository", String.Empty);
            CalibrationStandard cs = csr.FindByName(name);
            return FindByDate(cs.CalibrationDate);
        }
        public async Task<List<STDAnion>> GetLatestAsync()
        {
            ICalibrationStandardRepository csr = new CalibrationStandardRepository(DbContext);
            string name = this.GetType().Name.Replace("Std", string.Empty).Replace("Repository", String.Empty);
            CalibrationStandard cs = csr.FindByName(name);
            return await FindByDateAsync(cs.CalibrationDate);
        }
        private async Task<STDAnion> SaveRowAsync(DataRow row, CalibrationStandard cs)
        {
            STDAnion item;
            var list = FindByDate(cs.CalibrationDate);
            if (!list.Any())
            {
                item = await AddNewItemAsync(row, cs);
            }
            else
            {
                string ident = AcetateToAnionFilter.Filter(row.ItemArray[row.Table.Columns["Ident"].Ordinal].ToString());
                item = list.FirstOrDefault(i => i.Ident == ident);
                if (item == null)
                {
                    item = await AddNewItemAsync(row, cs);
                }
                else
                {
                    var acetArea = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["Acet .Area"].Ordinal]);
                    var acetPpm = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["Acet PPM"].Ordinal]);
                    if (acetArea > 0 || acetPpm > 0)
                    {
                        item = PopulateAcetateItem(row, cs, item);
                    }
                    else
                    {
                        item = PopulateItemAsync(row, cs, item);
                    }
                    int result = await UpdateAsync(item);
                }
            }
            return item;
        }

        private async Task<STDAnion> AddNewItemAsync(DataRow row, CalibrationStandard cs)
        {
            STDAnion item = new STDAnion();
            item = PopulateItemAsync(row, cs, item);
            item = PopulateAcetateItem(row, cs, item);
            var result = await AddAsync(item);
            return item;
        }
        private static STDAnion PopulateItemAsync(DataRow row, CalibrationStandard cs, STDAnion item)
        {
            item.CalibrationStandardId = cs.Id;
            item.CalibrationDate = cs.CalibrationDate;
            item.Ident = AcetateToAnionFilter.Filter(row.ItemArray[row.Table.Columns["Ident"].Ordinal].ToString());
            item.Dilution = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["Dilution"].Ordinal]);
            item.F__Area = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["F .Area"].Ordinal]);
            item.Cl__Area = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["Cl .Area"].Ordinal]);
            item.Br__Area = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["Br. Area"].Ordinal]);
            item.NO3_Area = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["NO3.Area"].Ordinal]);
            item.Po4_Area = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["Po4.Area"].Ordinal]);
            item.So4_Area = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["So4.Area"].Ordinal]);
            item.F_PPM = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["F PPM"].Ordinal]);
            item.Cl_PPM = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["Cl PPM"].Ordinal]);
            item.Br_PPM = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["Br PPM"].Ordinal]);
            item.No3_PPM = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["No3 PPM"].Ordinal]);
            item.Po4_PPM = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["Po4 PPM"].Ordinal]);
            item.So4_PPM = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["So4 PPM"].Ordinal]);
            return item;
        }

        private static STDAnion PopulateAcetateItem(DataRow row, CalibrationStandard cs, STDAnion item)
        {
            item.CalibrationStandardId = cs.Id;
            item.CalibrationDate = cs.CalibrationDate;
            item.Ident = AcetateToAnionFilter.Filter(row.ItemArray[row.Table.Columns["Ident"].Ordinal].ToString());
            item.Dilution = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["Dilution"].Ordinal]);
            item.Acet__Area =
                DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["Acet .Area"].Ordinal]);
            item.Acet_PPM = DataColumnToDecimalFilter.Filter(row.ItemArray[row.Table.Columns["Acet PPM"].Ordinal]);
            return item;
        }
    }
}