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
        private readonly IMessageProvider _messageProvider;
        private bool _running;

        public TwoTimeBot(IOutputClient outputClient, ILogger logger, IMessageProvider messageProvider)
        {
            _outputClient = outputClient;
            _logger = logger;
            _messageProvider = messageProvider;
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

                await SendTwoTimeMessageAsync(currentTime).FreeContext();
                _logger.Log("It's 2-time!! I've sent the tweet.");

                currentTime = TimeHelper.GetCurrentTime(); // Recalculate current time to account for the time spent sending the message
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
            _logger.Log($"Waiting {timeUntilTwoTime} until next 2-time.");
            return Task.Delay(timeUntilTwoTime);
        }

        private async Task SendTwoTimeMessageAsync(DateTimeOffset currentTime)
        {
            var message = await _messageProvider.FetchRandomTwoTimeMessageAsync(currentTime).FreeContext();
            await _outputClient.SendMessageAsync(message).FreeContext();
        }
    }
}
