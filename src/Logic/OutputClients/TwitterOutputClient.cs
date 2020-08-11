using Logic.Configuration;
using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;

namespace Logic.OutputClients
{
    public class TwitterOutputClient : IOutputClient
    {
        private readonly TwitterClient _twitterClient;

        public TwitterOutputClient(ITwitterConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config.ConsumerKey) ||
                string.IsNullOrWhiteSpace(config.ConsumerSecret) ||
                string.IsNullOrWhiteSpace(config.AccessToken) ||
                string.IsNullOrWhiteSpace(config.AccessSecret))
            {
                throw new NullReferenceException("One or more of the fields in appsettings.json are empty. Please fill them out and try again.");
            }

            _twitterClient = new TwitterClient(config.ConsumerKey, config.ConsumerSecret, config.AccessToken, config.AccessSecret);
        }

        public async Task<bool> SendMessageAsync(string message)
        {
            var tweetParams = new PublishTweetParameters()
            {
                Text = message
            };

            await _twitterClient.Tweets.PublishTweetAsync(tweetParams).FreeContext();
            return true;
        }
    }
}
