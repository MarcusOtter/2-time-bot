using Logic.OAuth;
using System;
using System.Threading.Tasks;
using System.Web;

namespace Logic.OutputClients
{
    public class TwitterClient : IOutputClient
    {
        private const string _twitterApiUrl = "https://api.twitter.com/1.1/statuses/update.json";
        private readonly IOAuth1Client _oAuth1Client;

        public TwitterClient(IOAuth1Client oAuth1Client)
        {
            _oAuth1Client = oAuth1Client;
        }
        
        public Task<bool> SendMessageAsync(TwoTimeMessage message)
        {
            return Tweet(message);
        }

        private async Task<bool> Tweet(TwoTimeMessage message)
        {
            var msgString = message.Text;
            // Temp url encoding here, should take a string in client probably
            var url = $"{_twitterApiUrl}?status={HttpUtility.UrlEncode(msgString)}";
            var uri = new Uri(url);

            var response = await _oAuth1Client.PostAsync(uri);
            return response.IsSuccessStatusCode;
        }
    }
}
