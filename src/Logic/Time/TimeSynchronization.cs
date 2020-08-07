using System;

namespace Logic.Time
{
    public interface ITimeSynchronization
    {
        DateTimeOffset GetCurrentTime();
        TimeSpan GetTimeUntilNextTwoTime();
        bool IsCurrentlyTwoTime();
        bool IsTwoTime(DateTimeOffset time);
        bool NextTwoTimeIsTomorrow(DateTimeOffset currentTime);
    }

    public class TimeSynchronization : ITimeSynchronization
    {
        public DateTimeOffset GetCurrentTime()
        {
            return DateTimeOffset.Now;
        }

        public TimeSpan GetTimeUntilNextTwoTime()
        {
            var currentTime = GetCurrentTime();
            var nextTwoTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 22, 22, 22, 22, DateTimeKind.Local);

            if (NextTwoTimeIsTomorrow(currentTime))
            {
                nextTwoTime = nextTwoTime.AddDays(1);
            }

            return new DateTimeOffset(nextTwoTime) - currentTime;
        }

        public bool IsCurrentlyTwoTime()
        {
            return IsTwoTime(GetCurrentTime());
        }

        public bool IsTwoTime(DateTimeOffset time)
        {
            return time.Hour == 22 && time.Minute == 22;
        }

        public bool NextTwoTimeIsTomorrow(DateTimeOffset currentTime)
        {
            return IsTwoTime(currentTime) || currentTime.Hour > 22 || (currentTime.Hour == 22 && currentTime.Minute > 22);
        }
    }
}
