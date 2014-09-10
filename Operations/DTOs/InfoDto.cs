using System;
using System.Collections.Generic;

namespace Operations.DTOs
{
    public class InfoDto
    {
        public int InfoID { get; set; }
        public string FileName { get; set; }
        public string Submitter { get; set; }
        public Nullable<System.DateTime> SubmitDate { get; set; }
        public string Plate_Name { get; set; }
        public string Carbon_Source { get; set; }
        public string t_dot_C_Source { get; set; }
        public string Strains { get; set; }
        public string Other_Analytes { get; set; }
        public string Project { get; set; }
        public string Group { get; set; }
        public string Special_Instructions { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string SpreadsheetType { get; set; }
        public Nullable<double> Feed { get; set; }
        public string SampleType { get; set; }
        public string KeyAnalytes { get; set; }
        public string ConcGL { get; set; }
        public string SubmissionName { get; set; }
        public Nullable<int> GC_Number_Of_Samples { get; set; }
        public string GC_Type_Of_Samples { get; set; }
        public string GC_SOP { get; set; }
        public string GC_Key_Analytes { get; set; }
        public Nullable<decimal> GC_Estimated_Concentration_Grams_Per_Liter { get; set; }
        public Nullable<decimal> GC_Feedstocks_Approximate_Conc { get; set; }
        public Nullable<int> LC_Number_Of_Samples { get; set; }
        public string LC_SOP { get; set; }
        public string LC_Key_Analytes { get; set; }
        public Nullable<decimal> LC_Estimated_Concentration_Grams_Per_Liter { get; set; }


    }
}
