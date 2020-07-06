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
            var dayOffset = currentTime.NextTwoTimeIsTomorrow() ? 1 : 0;
            var nextTwoTime = new DateTimeOffset(new DateTime(currentTime.Year, currentTime.Month, currentTime.Day + dayOffset, 22, 22, 22, 22, DateTimeKind.Local));
            return nextTwoTime - currentTime;
        }
    }
}
