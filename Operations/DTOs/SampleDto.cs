using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Operations.DTOs
{
    public class SampleDto
    {
        public int Id { get; set; }
        public int RunId { get; set; }
        public string WordFileUri { get; set; }
        public string ExcelFileUri { get; set; }
        public List<SampleDetailDto> SampleListDto { get; set; }
    }
}