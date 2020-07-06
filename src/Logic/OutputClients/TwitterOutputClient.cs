using Logic.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi;

namespace Logic.OutputClients
{
    public class TwitterOutputClient : IOutputClient
    {
        private readonly TwitterClient _twitterClient;

        public TwitterOutputClient(IStorage storage)
        {
            var hasConsumerKey    = storage.TryGet(StorageItem.ConsumerKey,    out string consumerKey);
            var hasConsumerSecret = storage.TryGet(StorageItem.ConsumerSecret, out string consumerSecret);
            var hasAccessToken    = storage.TryGet(StorageItem.AccessToken,    out string accessToken);
            var hasAccessSecret   = storage.TryGet(StorageItem.AccessSecret,   out string accessSecret);

            if (!hasConsumerKey || !hasConsumerSecret || !hasAccessToken || !hasAccessSecret)
            {
                throw new KeyNotFoundException("Something went wrong in the storage which prevented twitter initialization");
            }

            _twitterClient = new TwitterClient(consumerKey, consumerSecret, accessToken, accessSecret);
        }

        public async Task<bool> SendMessageAsync(TwoTimeMessage text)
        {
            await _twitterClient.Tweets.PublishTweetAsync(text.Text).FreeContext();
            return true;
        }
    }
}
