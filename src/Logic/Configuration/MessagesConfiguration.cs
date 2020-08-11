namespace Logic.Configuration
{
    public interface IMessagesConfiguration
    {
        string? MessagesUrl { get; }
    }

    public class MessagesConfiguration : IMessagesConfiguration
    {
        public string? MessagesUrl { get; set; }
    }
}
