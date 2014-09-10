using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilterPipeline.Filters;

namespace FilterPipeline.Filters 
{
    
    public class SampleSerialNoFromIdentFilter : FilterBase<string>
    {
        /// <summary>
        /// Filters out everything from before and including the first _ to
        /// everything after and including the second _
        /// Expects "12_00145_7/29/2014_L493___"
        /// Returns    "00145"
        /// </summary>
        /// <param name="input">string - the string to be filtered</param>
        /// <returns>string - everthing between first and second _</returns>
        protected override string Process(string input)
        {
            return DoFilter(input);
        }

        public string Filter(string input)
        {
            return DoFilter(input);
        }

        private string DoFilter(string input)
        {
            if (!input.Contains("_"))
                return string.Empty;
            string temp = input.Substring(input.IndexOf("_", System.StringComparison.Ordinal) + 1);
            if (!temp.Contains("_"))
                return string.Empty;
            temp = temp.Substring(0, temp.IndexOf("_", System.StringComparison.Ordinal));
            if (temp.Length != 5)
                return string.Empty;

            return temp;
        }
    }
}
