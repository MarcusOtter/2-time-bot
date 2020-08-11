using System.Threading.Tasks;
using Logic.OutputClients;
using Logic.Logging;
using Logic.Time;

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
        private readonly ITimeSynchronization _timeSync;
        private bool _running;

        public TwoTimeBot(IOutputClient outputClient, ILogger logger, IMessageProvider messageProvider, ITimeSynchronization timeSync)
        {
            _outputClient = outputClient;
            _logger = logger;
            _messageProvider = messageProvider;
            _timeSync = timeSync;
        }

        public async Task RunIndefinitelyAsync()
        {
            _running = true;
            var nextMessage = await FetchNextTwoTimeMessageAsync().FreeContext();

            while (_running)
            {
                if (!_timeSync.IsCurrentlyTwoTime())
                {
                    await DelayUntilTwoTimeAsync().FreeContext();
                    continue;
                }

                await SendTwoTimeMessageAsync(nextMessage).FreeContext();
                nextMessage = await FetchNextTwoTimeMessageAsync().FreeContext();

                await DelayUntilTwoTimeAsync().FreeContext();
            }
        }

        public Task StopAsync()
        {
            _running = false;
            return Task.CompletedTask;
        }

        private Task DelayUntilTwoTimeAsync()
        {
            var timeUntilTwoTime = _timeSync.GetTimeUntilNextTwoTime();
            _logger.Log($"Waiting {timeUntilTwoTime} until next 2-time.");
            return Task.Delay(timeUntilTwoTime);
        }

        private async Task<string> FetchNextTwoTimeMessageAsync()
        {
            var nextMessage = await _messageProvider.FetchRandomTwoTimeMessageAsync().FreeContext();
            _logger.Log($"Next 2-time message: \"{nextMessage}\"");
            return nextMessage;
        }

        private async Task<bool> SendTwoTimeMessageAsync(string message)
        {
            var result = await _outputClient.SendMessageAsync(message).FreeContext();
            _logger.Log($"I just tweeted \"{message}\"");
            return result;
        }
    }
}
