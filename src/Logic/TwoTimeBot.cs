using Logic.OutputClients;
using Logic.Helpers;
using System;
using System.Threading.Tasks;

namespace Logic
{
    public interface ITwoTimeBot
    {
        Task RunIndefinitelyAsync();
        Task StopAsync();
    }

    public class TwoTimeBot : ITwoTimeBot
    {
        private readonly IOutputClient _outputClient;
        private bool _running;

        public TwoTimeBot(IOutputClient outputClient)
        {
            _outputClient = outputClient;
        }

        public async Task RunIndefinitelyAsync()
        {
            _running = true;
            while (_running)
            {
                var currentTime = GetCurrentTime();

                if (!IsTwoTime(currentTime))
                {
                    await DelayUntilTwoTimeAsync(currentTime).FreeContext();
                    continue;
                }

                await SendTwoTimeMessageAsync().FreeContext();
                Console.WriteLine("2-time tweet sent!");

                currentTime = TimeHelper.GetCurrentTime(); // Recalculates current time to account for the time spent sending the messages
                await DelayUntilTwoTimeAsync(currentTime).FreeContext();
            }
        }

        public Task StopAsync()
        {
            _running = false;
            return Task.CompletedTask;
        }

        private DateTimeOffset GetCurrentTime()
        {
            return DateTimeOffset.Now;
        }

        private Task DelayUntilTwoTimeAsync(DateTimeOffset currentTime)
        {
            var timeUntilTwoTime = GetTimeUntilNextTwoTime(currentTime);
            var delayInMs = (int) timeUntilTwoTime.TotalMilliseconds;

            Console.WriteLine($"Waiting {timeUntilTwoTime} until next two-time.");
            return Task.Delay(delayInMs);
        }

        private TimeSpan GetTimeUntilNextTwoTime(DateTimeOffset currentTime)
        {
            var dayOffset = NextTwoTimeIsTomorrow(currentTime) ? 1 : 0;
            var nextTwoTime = new DateTimeOffset(new DateTime(currentTime.Year, currentTime.Month, currentTime.Day + dayOffset, 22, 22, 22, 22, DateTimeKind.Local));
            return nextTwoTime - currentTime;
        }

        private bool NextTwoTimeIsTomorrow(DateTimeOffset currentTime)
        {
            return IsTwoTime(currentTime) || currentTime.Hour > 22 || (currentTime.Hour == 22 && currentTime.Minute > 22);
        }

        private bool IsTwoTime(DateTimeOffset dateTime)
        {
            return dateTime.Hour == 22 && dateTime.Minute == 22;
        }

        private Task SendTwoTimeMessageAsync()
        {
            // TODO: Get message from message provider
            var message = new TwoTimeMessage("This is our first automated 2-time message! Happy 2-time friends! :)");
            return _outputClient.SendMessageAsync(message);
        }
    }
}
