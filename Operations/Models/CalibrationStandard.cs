//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using Operations.Models;

namespace Process.Models
{
    using System;
    using System.Collections.Generic;

    public partial class CalibrationStandard
    {
        public CalibrationStandard()
        {
            this.STDAnions = new HashSet<STDAnion>();
            this.STDIrons = new HashSet<STDIron>();
            this.STDCations = new HashSet<STDCation>();
        }

        public int Id { get; set; }
        public string CalibrationName { get; set; }

        [Display(Name = "Calibration Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> CalibrationDate { get; set; }

        public string Folder { get; set; }
        public string Filename { get; set; }
        public string Tablename { get; set; }

        public virtual ICollection<STDAnion> STDAnions { get; set; }
        public virtual ICollection<STDIron> STDIrons { get; set; }
        public virtual ICollection<STDCation> STDCations { get; set; }
    }
}