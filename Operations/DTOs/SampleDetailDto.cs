using System;

namespace Operations.DTOs
{
    public class SampleDetailDto
    {
        public int Id { get; set; }
        public int SampleDtoId { get; set; }
        public string Name { get; set; }
        public string DateAndTime { get; set; }
        public bool IsPrepared { get; set; }
        public DateTime DatePrepared { get; set; }
        public string PreparedBy { get; set; }
        public bool IsWeighed { get; set; }
        public DateTime DateWeighed { get; set; }
        public string WeighedBy { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string SubmittedBy { get; set; }

    }
}