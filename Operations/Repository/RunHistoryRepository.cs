using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Operations.Contracts;
using Operations.DTOs;
using Operations.Models;
using Operations.Utility;

namespace Operations.Repository
{
    public class RunHistoryRepository : GenericPilotPlantRepository<RunHistory>, IRunHistoryRepository
    {
        public RunHistoryRepository(PilotPlantEntities dbContext)
            : base(dbContext)
        {
        }

        public Task<int> AddAsync(RunHistoryChartDto t)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveAsync(RunHistoryChartDto t)
        {
            throw new NotImplementedException();
        }

        public new Task<List<RunHistoryChartDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(RunHistoryChartDto t)
        {
            throw new NotImplementedException();
        }

        public Task<RunHistoryChartDto> FindAsync(Expression<Func<RunHistoryChartDto, bool>> match)
        {
            throw new NotImplementedException();
        }

        public Task<List<RunHistoryChartDto>> FindAllAsync(Expression<Func<RunHistoryChartDto, bool>> match)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateOrAddAsync(RunHistoryChartDto t)
        {
            throw new NotImplementedException();
        }

        public IQueryable<RunHistoryItemDto> GetRunHistoryItem(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TankHistroyDto> GetFlotDataModels(Operations.Models.RunHistory rh)
        {
            var run = DbContext.Runs.FirstOrDefault(r => r.Id == rh.RunId);
            var tanks = DbContext.Tanks.Where(m => m.MachineId == run.MachineId);
            var runHistoryDtos = GetRunHistoryChartDtos(rh);
            var list = runHistoryDtos.ToList();
            var gooddate = System.Convert.ToDateTime("2012-1-1");

            var Readings = DbContext.Readings
                .Where(r => r.RunId == run.Id)
                .Where(r => r.ReadingDescriptionId == rh.ReadingDescriptionId)
                .Where(r => r.ReadingDateTime > gooddate)
                .ToList().AsQueryable();
            
            List<TankHistroyDto> TankDataList = new List<TankHistroyDto>();

            foreach (Operations.Models.Tank t in tanks)
            {
                Operations.DTOs.TankHistroyDto dto = new TankHistroyDto();
                dto.TankId = t.Id;
                dto.TankName = t.Name;
                dto.DataPoints = new List<DataPoint>();
                foreach (Reading r in Readings)
                {
                    DataPoint dp = new DataPoint();
                    try
                    {
                        dp.x = !string.IsNullOrEmpty(r.Eft) ? System.Convert.ToDecimal(r.Eft) : 0;
                        dp.y = !string.IsNullOrEmpty(r.Value) ? System.Convert.ToDecimal(r.Value) : 0;
                    }
                    catch (FormatException ex)
                    {
                        string msg = string.Format("TankId: {0}, Tank Name {1}, ReadingTime {2}, Reading Value {3}",
                            t.Id, t.Name, r.ReadingTime, r.Value);
                        System.Diagnostics.Debug.WriteLine(msg);
                        throw new Exception(ex.Message + msg + "\n" + ex.StackTrace);
                    }
                    dto.DataPoints.Add(dp);
                }

                TankDataList.Add(dto);


            }
            return TankDataList.AsQueryable();



        }


        public RunHistoryChartDto GetRunHistoryChart(Models.RunHistory rh)
        {
            try
            {
                var run = DbContext.Runs.FirstOrDefault(r => r.Id == rh.RunId);
                var tanks = DbContext.Tanks.Where(m => m.MachineId == run.MachineId);

                var runHistoryDtos = GetRunHistoryChartDtos(rh);
                var list = runHistoryDtos.ToList();

                RunHistoryChartDto retval = runHistoryDtos.FirstOrDefault(tk => tk.TankId == tanks.FirstOrDefault().Id);

                if (runHistoryDtos != null)
                {
                    foreach (var rhdto in runHistoryDtos)
                    {
                        retval.RunDate = run.Rundate;
                        retval.ReadingDescriptionName = DbContext.ReadingDescriptions.FirstOrDefault(d => d.Id == rhdto.ReadingDescriptionId).Name;


                        var gooddate = System.Convert.ToDateTime("2012-1-1");

                        retval.Readings = DbContext.Readings
                            .Where(r => r.RunId == run.Id)
                            .Where(r => r.ReadingDescriptionId == rh.ReadingDescriptionId)
                            .Where(r => r.ReadingDateTime > gooddate)
                            .ToList();


                        var values = new Dictionary<string, string>();
                        string[] vals = rhdto.ValuesArray.Split('|');
                        List<string> serieslist = new List<string>();
                        var runstart = run.RunStart;
                        foreach (string t in vals)
                        {
                            // There is an additional unseen pair of characters in the array string of 0x001d decimal 20?
                            var index = t.IndexOf(",", System.StringComparison.Ordinal);
                            var d = t.Substring(1, index - 1).Trim();
                            //var dt = FilterDateString(d);
                            var v = t.Substring(index + 1).Trim();
                            var eft = FilterEFT(d);

                            if ((!values.ContainsKey(eft)) && (!values.ContainsValue(v)))
                                values.Add(eft, v);

                            var streft = eft.ToString(CultureInfo.InvariantCulture);
                            if (!rhdto.Categorys.Contains(streft))
                                rhdto.Categorys.Add(streft);

                            if (retval.ReadingDescriptionName.Contains("Y/N"))
                                v = v == "true" ? "1" : "0";

                            //if (!serieslist.Contains(v))
                            serieslist.Add(v);
                        }
                        retval.Series.Add(serieslist);
                        retval.Values = values;

                        retval.Categorys = rhdto.Categorys;
                        retval.Tanks = tanks.AsQueryable();
                        retval.ReadingDescriptions = DbContext.ReadingDescriptions.AsQueryable();
                    }
                }
                return retval;

            }
            catch (Exception ex)
            {
                throw new Exception("ChartValues FAILED" + ex.Message);
            }

        }




        #region RunHistoryChartDTO

        private Expression<Func<Models.RunHistory, RunHistoryChartDto>> AsRunHistoryChartDto =
            dz => new RunHistoryChartDto
            {
                Id = dz.Id,
                RunId = dz.RunId,
                //RunDate = db.Run.FirstOrDefault(r => r.Id == dz.RunId).Rundate,
                TankId = dz.TankId,
                //TankName = db.Tank.FirstOrDefault(r => r.Id == dz.TankId).Name,
                ReadingDescriptionId = dz.ReadingDescriptionId,
                //ReadingDescriptionName = db.ReadingDescription.FirstOrDefault(r => r.Id == dz.ReadingDescriptionId).Name,
                ValuesArray = dz.ValueArray
            };




        private IQueryable<RunHistoryChartDto> GetRunHistoryChartDtos(Operations.Models.RunHistory rh)
        {
            var dto = DbContext.RunHistories
                 .Select(AsRunHistoryChartDto)
                 .Where(n => n.RunId == rh.RunId)
                 .Where(n => n.ReadingDescriptionId == rh.ReadingDescriptionId)
                 .AsQueryable();

            if (dto == null)
            {
                throw new Exception("Chart not found");
            }
            return dto;
        }


        #endregion

        #region Helpers

        public double CalculateElapsedTime(DateTime? startdate, string enddate)
        {
            IEftCalculator calculator = new EftCalculator();
            return calculator.CalculateEft(startdate, enddate);
        }

        public string FilterDateString(string datestring)
        {
            var allowedChars =
                Enumerable.Range('0', 10).Concat(
                Enumerable.Range(':', 1)).Concat(
                Enumerable.Range(' ', 1)).Concat(
                Enumerable.Range('/', 1));

            var goodchars = datestring.Where(c => allowedChars.Contains(c));
            return new string(goodchars.ToArray());
        }

        public string FilterEFT(string eft)
        {
            var allowedChars =
                Enumerable.Range('0', 10).Concat(
                Enumerable.Range('.', 1));

            var goodchars = eft.Where(c => allowedChars.Contains(c));
            return new string(goodchars.ToArray());
        }

        #endregion
    }
}