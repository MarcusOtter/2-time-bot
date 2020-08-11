namespace Logic.Configuration
{
    public interface ITwitterConfiguration
    {
        string? ConsumerKey { get; }
        string? ConsumerSecret { get; }
        string? AccessToken { get; }
        string? AccessSecret { get; }
    }

    public class TwitterConfiguration : ITwitterConfiguration
    {
        public string? ConsumerKey { get; set; }
        public string? ConsumerSecret { get; set; }
        public string? AccessToken { get; set; }
        public string? AccessSecret { get; set; }
    }
}
