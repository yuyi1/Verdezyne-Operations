using System.Collections.Generic;
using Operations.Models;

namespace Operations.DTOs
{
    public class SampleAnionDto
    {
        public string InputFile { get; set; }
        public List<SampleAnion> comparison;
        public List<SampleAnion> values;
    }
}