using System.Collections.Generic;
using Operations.Models;

namespace Operations.DTOs
{
    public class SampleIronDto
    {
        public string InputFile { get; set; }
        public List<SampleIron> comparison;
        public List<SampleIron> values;
    }
}