using Logic.Time;
using System;

namespace Logic.Logging
{
    public class ConsoleLogger : ILogger
    {
        private readonly ITimeSynchronization _timeSync;

        public ConsoleLogger(ITimeSynchronization timeSync)
        {
            _timeSync = timeSync;
        }

        public void Log(string message)
        {
            Console.WriteLine($"[{_timeSync.GetCurrentTime():dd MMMM HH:mm:ss}]: {message}");
        }
    }
}
