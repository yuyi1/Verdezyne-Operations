using System;
using Operations.Contracts;

namespace Operations.Utility
{
    public class EftCalculator : IEftCalculator
    {
        public double CalculateEft(DateTime? startdate, string enddate)
        {
            if (startdate == DateTime.MinValue || startdate == null)
                throw new ArgumentOutOfRangeException("startdate", "Start Date is null");

            DateTime end = System.Convert.ToDateTime(enddate);
            DateTime start = System.Convert.ToDateTime(startdate);
            return CalculateEft(start, end);
        }

        public double CalculateEft(string startdate, string endDate)
        {
            throw new NotImplementedException();
        }

        public double CalculateEft(DateTime startdate, DateTime enddate)
        {

            TimeSpan ts = enddate.Subtract(startdate);

            var difftime = Decimal.Round(System.Convert.ToDecimal(ts.TotalHours), 2);
            var hours = (double)(int)ts.TotalHours;

            var ret = hours;
            if (ts.Minutes > 45)
            {
                if (difftime > 0)
                {
                    ret = ret + 1;
                }
                else
                {
                    ret = ret - 1;
                }

            }
            else
            {
                // ts.Minutes will be negative with a negative number.
                //This differs from Sql where the minutes is absolute
                if (ts.Minutes > 15)
                {
                    ret = ret + 0.5;
                }
                else
                {
                    if (ts.Minutes < 0)
                    {
                        ret = ret - 0.5;
                    }
                }
            }
            return ret;
        }
    }
}