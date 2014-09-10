using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Operations.DTOs
{
    public class QcSampleDto
    {
        public IQueryable<QcSampleItemDto> Items { get; set; }
        IEnumerable<Operations.Models.STDAnion> StdAnions { get; set; }
        IEnumerable<Operations.Models.STDAnion> StdCations { get; set; }
        IEnumerable<Operations.Models.STDAnion> StdIrons { get; set; }
        IEnumerable<Operations.Models.SampleAnion> SampleAnions { get; set; }
        IEnumerable<Operations.Models.SampleAnion> SampleCations { get; set; }
        IEnumerable<Operations.Models.SampleAnion> SampleIrons { get; set; }
    }
}