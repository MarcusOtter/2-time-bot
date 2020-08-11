using Logic;
using Logic.Configuration;
using Logic.Logging;
using Logic.OutputClients;
using Logic.Storage;
using Logic.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;

namespace UI
{
    public class Program
    {
        private async static Task Main()
        {
            var serviceCollection = GetServiceCollection();
            RegisterAppConfiguration(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var bot = serviceProvider.GetService<ITwoTimeBot>();
            await bot.RunIndefinitelyAsync().FreeContext();
        }

        private static IServiceCollection GetServiceCollection()
        {
            return new ServiceCollection()
               .AddSingleton<ITwoTimeBot, TwoTimeBot>()
               .AddSingleton<IStorage, InMemoryStorage>()
               .AddSingleton<IOutputClient, TwitterOutputClient>()
               .AddSingleton<ILogger, ConsoleLogger>()
               .AddSingleton<IMessageProvider, MessageProvider>()
               .AddSingleton<ITimeSynchronization, TimeSynchronization>()
               .AddSingleton<HttpClient>();
        }

        private static void RegisterAppConfiguration(IServiceCollection serviceCollection)
        {
            var configurationPath = GetConfigurationPath();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(configurationPath, optional: false)
                .Build();

            var twitterConfiguration = new TwitterConfiguration();
            var messagesConfiguration = new MessagesConfiguration();

            configuration.Bind("TwitterConfiguration", twitterConfiguration);
            configuration.Bind("MessagesConfiguration", messagesConfiguration);

            serviceCollection.AddSingleton<ITwitterConfiguration>(twitterConfiguration);
            serviceCollection.AddSingleton<IMessagesConfiguration>(messagesConfiguration);
        }

        private static string GetConfigurationPath()
        {
#if DEBUG
            return @"appsettings.Development.json";
#else
            return @"appsettings.json";
#endif
        }
    }
}
