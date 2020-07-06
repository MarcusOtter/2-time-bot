using Logic.OutputClients;
using Logic.Helpers;
using System;
using System.Threading.Tasks;
using Logic.Logging;

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
        private readonly ILogger _logger;
        private bool _running;

        public TwoTimeBot(IOutputClient outputClient, ILogger logger)
        {
            _outputClient = outputClient;
            _logger = logger;
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
                _logger.Log("It's 2-time!! I've sent the tweet.");

                currentTime = TimeHelper.GetCurrentTime(); // Recalculate current time to account for the time spent sending the messages
                await DelayUntilTwoTimeAsync(currentTime).FreeContext();
            }
        }

        public Task StopAsync()
        {
            _running = false;
            return Task.CompletedTask;
        }

        private Task DelayUntilTwoTimeAsync(DateTimeOffset currentTime)
        {
            var timeUntilTwoTime = currentTime.GetTimeUntilNextTwoTime();
            var delayInMs = (int) timeUntilTwoTime.TotalMilliseconds;

            _logger.Log($"Waiting {timeUntilTwoTime} until next two-time.");
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
