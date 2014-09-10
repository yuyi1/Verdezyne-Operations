using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Operations.DTOs
{
    public class RunHistoryItemDto
    {
        public int Id { get; set; }
        public int RunId { get; set; }
        public string RunDate { get; set; }
        public int TankId { get; set; }
        public string TankName { get; set; }
        public int ReadingDescriptionId { get; set; }
        public string ReadingDescriptionName { get; set; }
        public Dictionary<string, string> Values { get; set; }
        public string ValuesArray { get; set; }
    }
}