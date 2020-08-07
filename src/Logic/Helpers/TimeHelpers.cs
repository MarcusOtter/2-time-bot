using System;

namespace Logic.Helpers
{
    public static class TimeHelpers
    {
        public static bool Matches(this DateTimeOffset dateTime, TwoTimeEntry entry)
        {
            // If one of these fields are provided in the entry (not null) and does not equal the time
            if ((entry.Year          != null && dateTime.Year      != entry.Year)       ||
                (entry.Month         != null && dateTime.Month     != entry.Month)      ||
                (entry.DayOfYear     != null && dateTime.DayOfYear != entry.DayOfYear)  ||
                (entry.DayOfMonth    != null && dateTime.Day       != entry.DayOfMonth))
            {
                return false;
            }

            if (entry.ReferenceDate != null)
            {
                if (entry.DaysAfterReferenceDate != null)
                {
                    var dateAfterSubtractingDays = dateTime.AddDays(entry.DaysAfterReferenceDate.Value * -1);
                    if (dateAfterSubtractingDays != entry.ReferenceDate) { return false; }
                }

                if (entry.DaysUntilReferenceDate != null)
                {
                    var dateAfterAddingDays = dateTime.AddDays(entry.DaysUntilReferenceDate.Value);
                    if (dateAfterAddingDays != entry.ReferenceDate) { return false; }
                }
            }

            return true;
        }
    }
}
