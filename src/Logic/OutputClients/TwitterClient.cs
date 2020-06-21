using System;
using System.Net.Http;
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
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.twitter.com/1.1/statuses/update.json")
            };

            //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue()


            _httpClient.SendAsync(request);
            return Task.FromResult(true);
        }
    }
}
