//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Process.Models
{
    using System;
    using System.Collections.Generic;

    public partial class SampleCation
    {
        public int Id { get; set; }
        [Display(Name = "Sample Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> SampleDate { get; set; }

        [Display(Name = "Sample Serial No")]
        public string SampleSerialNo { get; set; }
        public string Ident { get; set; }

        [DisplayFormat(DataFormatString = "{0:F3}")]
        public Nullable<decimal> Dilution { get; set; }

        [Display(Name = "Li Area")]
        [DisplayFormat(DataFormatString = "{0:F3}")]
        public Nullable<decimal> Li_Area { get; set; }
        
        [Display(Name = "Na Area")]
        [DisplayFormat(DataFormatString = "{0:F3}")]
        public Nullable<decimal> Na_Area { get; set; }
        
        [Display(Name = "NH4 Area")]
        [DisplayFormat(DataFormatString = "{0:F3}")]
        public Nullable<decimal> NH4__Area { get; set; }
        
        [Display(Name = "K Area")]
        [DisplayFormat(DataFormatString = "{0:F3}")]
        public Nullable<decimal> K_Area { get; set; }
        
        [Display(Name = "Ca 2 Area")]
        [DisplayFormat(DataFormatString = "{0:F3}")]
        public Nullable<decimal> Ca_2__Area { get; set; }
        
        [Display(Name = "Mg 2 Area")]
        [DisplayFormat(DataFormatString = "{0:F3}")]
        public Nullable<decimal> Mg_2__Area { get; set; }
        
        [Display(Name = "Li Area")]
        [DisplayFormat(DataFormatString = "{0:F3}")]
        public Nullable<decimal> Li__PPM { get; set; }
        
        [Display(Name = "Na PPM")]
        [DisplayFormat(DataFormatString = "{0:F3}")]
        public Nullable<decimal> Na__PPM { get; set; }
        
        [Display(Name = "NH4 PPM")]
        [DisplayFormat(DataFormatString = "{0:F3}")]
        public Nullable<decimal> NH4___PPM { get; set; }
        
        [Display(Name = "K PPM")]
        [DisplayFormat(DataFormatString = "{0:F3}")]
        public Nullable<decimal> K__PPM { get; set; }
        
        [Display(Name = "Ca 2 PPM")]
        [DisplayFormat(DataFormatString = "{0:F3}")]
        public Nullable<decimal> Ca__PPM { get; set; }
        
        [Display(Name = "Mg 2 PPM")]
        [DisplayFormat(DataFormatString = "{0:F3}")]
        public Nullable<decimal> Mg__PPM { get; set; }
        
        public string SpreadsheetLink { get; set; }
    }
}
