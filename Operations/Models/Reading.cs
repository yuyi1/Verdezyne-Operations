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
    
    public partial class Reading
    {
        public int Id { get; set; }
        public string RunDate { get; set; }
        public System.DateTime ReadingDateTime { get; set; }
        public int PersonId { get; set; }
        public int RunId { get; set; }
        public int MachineId { get; set; }
        public int ReadingSessionId { get; set; }
        public int TankId { get; set; }
        public int GrowthPhaseId { get; set; }
        public int ProductId { get; set; }
        public int ReadingDescriptionId { get; set; }
        public int MachineReadingDescriptionId { get; set; }
        public int ValuesState { get; set; }
        public string ReadingTime { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime LastUpdate { get; set; }
        public string Eft { get; set; }
    }
}
