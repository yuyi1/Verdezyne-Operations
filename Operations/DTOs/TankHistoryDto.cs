using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Operations.DTOs
{
    public class TankHistroyDto
    {
        public int TankId { get; set; }
        public string TankName { get; set; }
        public List<DataPoint> DataPoints { get; set; }
    }
}