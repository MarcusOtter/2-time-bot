using Logic.Storage;
using System.Threading.Tasks;
using Tweetinvi;

namespace Logic.OutputClients
{
    public class TwitterOutputClient : IOutputClient
    {
        private readonly TwitterClient _twitterClient;

        public TwitterOutputClient(IStorage storage)
        {
            // TODO: Get strings from storage
            _twitterClient = new TwitterClient("replace me", "replace me", "replace me", "replace me");
        }

        public async Task<bool> SendMessageAsync(TwoTimeMessage text)
        {
            var tweet = await _twitterClient.Tweets.PublishTweetAsync(text.Text);
            return true;
        }
    }
}
