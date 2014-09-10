using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Operations.DTOs
{
    public class ErrorReportDto
    {
        public string Title { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public int RunId { get; set; }
    }
}