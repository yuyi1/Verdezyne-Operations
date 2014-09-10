using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Operations.Models;

namespace Operations.DTOs
{
    public class RotatedReadingsListDto
    {
        public List<RotatedReading> Readings;
        public List<string> NonNullColumnList;
    }
}