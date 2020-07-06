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
                var currentTime = TimeHelper.GetCurrentTime();

                if (!currentTime.IsTwoTime())
                {
                    await DelayUntilTwoTimeAsync(currentTime).FreeContext();
                    continue;
                }

                await SendTwoTimeMessageAsync().FreeContext();
                Console.WriteLine("2-time tweet sent!");

                currentTime = TimeHelper.GetCurrentTime(); // Recalculate current time to account for the time spent sending the messages
                await DelayUntilTwoTimeAsync(currentTime).FreeContext();
            }
        }

        public Task StopAsync()
        {
            _running = false;
            return Task.CompletedTask;
        }

        private static Task DelayUntilTwoTimeAsync(DateTimeOffset currentTime)
        {
            var timeUntilTwoTime = currentTime.GetTimeUntilNextTwoTime();
            var delayInMs = (int) timeUntilTwoTime.TotalMilliseconds;

            Console.WriteLine($"Waiting {timeUntilTwoTime} until next two-time.");
            return Task.Delay(delayInMs);
        }

        private Task SendTwoTimeMessageAsync()
        {
            // TODO: Get message from message provider
            var message = new TwoTimeMessage("This is our first automated 2-time message! Happy 2-time friends! :)");
            return _outputClient.SendMessageAsync(message);
        }
    }
}
