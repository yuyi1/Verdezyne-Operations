using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Operations.DTOs
{
    public class ReadingEditDto : RunHistoryChartDto
    {
        public string Date { get; set; }
        public string Tankname { get; set; }
        public string Value { get; set; }
    }
}