using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPipeline.Filters
{
    public class DataColumnToDecimalFilter
    {
        public static decimal Process(object input)
        {
            return ConvertToDecimal(input);
        }

        public static decimal Filter(object input)
        {
            return ConvertToDecimal(input);
        }
        private static decimal ConvertToDecimal(object input)
        {
            string s = input.ToString();
            decimal output;
            bool ret = decimal.TryParse(s, out output);
            if (ret)
            {
                return output;
            }
            else
            {
                return (decimal)0.0;
            }            
        }
    }
}
