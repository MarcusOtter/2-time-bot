using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Logic.OutputClients
{
    public class TwitterClient : IOutputClient
    {
        private readonly HttpClient _httpClient;

        public TwitterClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public Task<bool> SendMessage(TwoTimeMessage message)
        {
            return Tweet(message);
        }

        private async Task<bool> Tweet(TwoTimeMessage message)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.twitter.com/1.1/statuses/update.json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("OAuth ");


            await _httpClient.SendAsync(request);
            return await Task.FromResult(true);
        }
    }
}
