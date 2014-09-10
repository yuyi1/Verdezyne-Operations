using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Operations.Models;

namespace Operations.DTOs
{
    public class ReadingsForRunDto
    {
        public Run Run { get; set; }
        public IQueryable<Reading> Readings { get; set; }
        public IQueryable<Tank> Tanks { get; set; } 

    }
}