using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Operations.DTOs
{
    public class QcSampleItemDto
    {
        public int ColumnNumber { get; set; }
        public int RowNumber { get; set; }
        public decimal SampleValue { get; set; }
        public decimal StdRangeHigh { get; set; }
        public decimal StdRangeLow { get; set; }
        public string CssClass { get; set; }
        public string DisplayValue { get; set; }

    }
}