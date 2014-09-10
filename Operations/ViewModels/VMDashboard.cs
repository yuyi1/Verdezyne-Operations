using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Operations.Models;

namespace Operations.ViewModels
{
    public class VmDashboard : VmRuns
    {
        public IQueryable<QC> QC { get; set; }
        public IQueryable<Sequence> Sequences { get; set; }
    }
}