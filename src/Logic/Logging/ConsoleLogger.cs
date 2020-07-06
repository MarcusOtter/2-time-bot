using Logic.Helpers;
using System;

namespace Logic.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[{TimeHelper.GetCurrentTime():dd MMMM hh:mm}]: {message}");
        }
    }
}
