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
    
    public partial class MachineReadingDescription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MachineId { get; set; }
        public int GrowthPhaseId { get; set; }
        public int ProductId { get; set; }
        public int TankId { get; set; }
        public int Sequence { get; set; }
        public int ReadingDescriptionId { get; set; }
        public string ReadingDescriptionName { get; set; }
        public string DataColumnType { get; set; }
        public string ReadingTypeName { get; set; }
        public bool IsNullable { get; set; }
        public int ValuesState { get; set; }
        public Nullable<decimal> High { get; set; }
        public Nullable<decimal> Low { get; set; }
    }
}