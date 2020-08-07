using Logic;
using Logic.Logging;
using Logic.OutputClients;
using Logic.Storage;
using Logic.Time;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;

namespace UI
{
    public class Program
    {
        private async static Task Main()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<ITwoTimeBot, TwoTimeBot>()
                .AddSingleton<IStorage, InMemoryStorage>()
                .AddSingleton<IOutputClient, TwitterOutputClient>()
                .AddSingleton<ILogger, ConsoleLogger>()
                .AddSingleton<IMessageProvider, MessageProvider>()
                .AddSingleton<ITimeSynchronization, TimeSynchronization>()
                .AddSingleton<HttpClient>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var bot = serviceProvider.GetService<ITwoTimeBot>();
            await bot.RunIndefinitelyAsync().FreeContext();
        }
    }
}
