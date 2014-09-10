using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterPipeline.Filters
{
    public class NonNumericFilter : FilterBase<string>
    {
        protected override string Process(string input)
        {
            var numbers = input.Where(x => char.IsDigit(x)).ToArray();
            return new string(numbers);
        }
    }
}
