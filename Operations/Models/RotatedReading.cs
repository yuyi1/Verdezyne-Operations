//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Operations.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class RotatedReading
    {
        public int Id { get; set; }
        public Nullable<int> RunId { get; set; }
        public string Run_Date { get; set; }
        public Nullable<int> MachineId { get; set; }
        public string Machine_Name { get; set; }
        public Nullable<int> ReadingSessionId { get; set; }
        public Nullable<int> TankId { get; set; }
        public string Tank_Name { get; set; }
        public Nullable<int> GrowthPhaseId { get; set; }
        public string Growth_Phase_Name { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string Product_Name { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public Nullable<double> EFT { get; set; }
        public string TANK { get; set; }
        public string TEMP_C { get; set; }
        public string JTEMP_C { get; set; }
        public string pO2_pct { get; set; }
        public string pH { get; set; }
        public string STIRR_RPM { get; set; }
        public string AIRF_LPM { get; set; }
        public string PRESS_mbar { get; set; }
        public string WEIGHT_KG { get; set; }
        public string Agitator_Seal_psi { get; set; }
        public string Clean_Steam_psi { get; set; }
        public string Plant_Steam_psi { get; set; }
        public string Base_L { get; set; }
        public string Antifoam_L { get; set; }
        public string OD600 { get; set; }
        public string OD600_dilution_factor { get; set; }
        public string Off_line_pH { get; set; }
        public string Plate_Sample { get; set; }
        public string YSI_Glucose_g_per_L { get; set; }
        public string Feed_KG { get; set; }
        public string Feed_Rate_L_per_hr { get; set; }
        public string Co_Feed_KG { get; set; }
        public string Co_Feed_Rate_L_per_hr { get; set; }
        public string Actual_Co_Feed_Rate_kg_per_hr { get; set; }
        public string New_Feed_Rate_L_per_hr { get; set; }
        public string GC_weight_g { get; set; }
        public string Base_mL { get; set; }
        public string Feed_mL { get; set; }
        public string CoFeed_mL { get; set; }
        public string Foam { get; set; }
        public string Crust { get; set; }
        public string Swirl { get; set; }
        public string Over_Flow { get; set; }
        public string Comments { get; set; }
        public string Base_g { get; set; }
        public string Feed_g { get; set; }
        public string Initials { get; set; }
        public string Feed { get; set; }
        public string Cofeed { get; set; }
        public string Fermentation_End_Time_hrs { get; set; }
        public string LCAA_pct_Ymax { get; set; }
        public string LCAA_Final_g_per_L { get; set; }
        public string LCAA_Produced_g { get; set; }
        public string LCAA_Productivity_g_per_L_hr { get; set; }
        public string Primary_Feed_Consumed_g { get; set; }
        public string Primary_Feed_End_time_hrs { get; set; }
        public string Primary_Feed_Start_Time_hrs { get; set; }
        public string Purity_pct { get; set; }
        public string Specific_Productivity_g_per_L_hr_per_g_per_L_DCW { get; set; }
        public string TPAA_pct_Ymax { get; set; }
        public string TPAA_Produced_g { get; set; }
        public string Analyical_Sample_Taken { get; set; }
    }
}
