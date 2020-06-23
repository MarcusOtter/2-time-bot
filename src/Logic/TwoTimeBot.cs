using Logic.OutputClients;
using System.Threading.Tasks;

namespace Logic
{
    public interface ITwoTimeBot
    {
        Task StartAsync();
        Task StopAsync();
    }

    public class TwoTimeBot : ITwoTimeBot
    {
        private IOutputClient _outputClient;

        public TwoTimeBot(IOutputClient outputClient)
        {
            _outputClient = outputClient;
        }

        public async Task StartAsync()
        {
            var message = new TwoTimeMessage("test");
            await _outputClient.SendMessageAsync(message);
        }

        public async Task StopAsync()
        {
        }
    }
}
