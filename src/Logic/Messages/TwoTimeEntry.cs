using System;

namespace Logic
{
    public class TwoTimeEntry
    {
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? DayOfYear { get; set; }
        public int? DayOfMonth { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public int? DaysAfterReferenceDate { get; set; }
        public int? DaysUntilReferenceDate { get; set; }
        public string[]? TwoTimeMessages { get; set; }
    }
}
