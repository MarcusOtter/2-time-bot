using Logic;
using Logic.OutputClients;
using Logic.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace UI
{
    public class Program
    {
        private async static Task Main(string[] args)
        {
            Console.WriteLine(MessageProvider.DefaultCelebrationMessage.Text);

            var serviceCollection = new ServiceCollection()
                .AddSingleton<ITwoTimeBot, TwoTimeBot>()
                .AddSingleton<IStorage, InMemoryStorage>()
                .AddSingleton<IOutputClient, TwitterClient>()
                .AddSingleton<HttpClient>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var bot = serviceProvider.GetService<ITwoTimeBot>();
            bot.Start();
        }
    }
}
