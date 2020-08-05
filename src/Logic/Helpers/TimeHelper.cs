using System;

namespace Logic.Helpers
{
    public static class TimeHelper
    {
        public static DateTimeOffset GetCurrentTime()
        {
            return DateTimeOffset.Now;
        }

        public static bool IsTwoTime(this DateTimeOffset dateTime)
        {
            return dateTime.Hour == 22 && dateTime.Minute == 22;
        }

        public static bool NextTwoTimeIsTomorrow(this DateTimeOffset currentTime)
        {
            return currentTime.IsTwoTime() || currentTime.Hour > 22 || (currentTime.Hour == 22 && currentTime.Minute > 22);
        }

        public static TimeSpan GetTimeUntilNextTwoTime(this DateTimeOffset currentTime)
        {
            var nextTwoTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 22, 22, 22, 22, DateTimeKind.Local);

            if (currentTime.NextTwoTimeIsTomorrow())
            {
                nextTwoTime = nextTwoTime.AddDays(1);
            }

            return new DateTimeOffset(nextTwoTime) - currentTime;
        }

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
