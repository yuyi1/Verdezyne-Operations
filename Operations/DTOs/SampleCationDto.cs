using System.Collections.Generic;
using Operations.Models;

namespace Operations.DTOs
{
    public class SampleCationDto
    {
        public string InputFile { get; set; }
        public List<SampleCation> comparison;
        public List<SampleCation> values;
    }
}