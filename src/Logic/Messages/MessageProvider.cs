using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Logic.Helpers;
using Logic.Time;
using Logic.Configuration;

namespace Logic
{
    public interface IMessageProvider
    {
        Task<TwoTimeMessage> FetchRandomTwoTimeMessageAsync();
    }

    public class MessageProvider : IMessageProvider
    {
        private readonly Random _random = new Random();
        private readonly TwoTimeMessage _defaultTwoTimeMessage = new TwoTimeMessage()
        {
            Text = "2-time!!! This one feels a bit odd... but UNTZ nonetheless!"
        };

        private readonly HttpClient _httpClient;
        private readonly ITimeSynchronization _timeSync;
        private readonly IMessagesConfiguration _messagesConfig;

        public MessageProvider(HttpClient httpClient, ITimeSynchronization timeSync, IMessagesConfiguration messagesConfig)
        {
            _httpClient = httpClient;
            _timeSync = timeSync;
            _messagesConfig = messagesConfig;
        }

        public async Task<TwoTimeMessage> FetchRandomTwoTimeMessageAsync()
        {
            var allTwoTimeEntires = await GetAllTwoTimeEntries().FreeContext();
            var output = _defaultTwoTimeMessage;
            var currentTime = _timeSync.GetCurrentTime();

            foreach(var entry in allTwoTimeEntires)
            {
                if (!currentTime.Matches(entry)) { continue; }
                if (entry.TwoTimeMessages == null) { continue; }

                do
                {
                    var randomIndex = _random.Next(0, entry.TwoTimeMessages.Length);
                    // TODO: Check if we have sent this message recently
                    output = entry.TwoTimeMessages[randomIndex];
                }
                while (string.IsNullOrWhiteSpace(output.Text));
            }

            return output;
        }



        private async Task<TwoTimeEntry[]> GetAllTwoTimeEntries()
        {
            var messagesUrl = _messagesConfig.MessagesUrl;
            var response = await _httpClient.GetAsync(messagesUrl).FreeContext();
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync().FreeContext();
            return await JsonSerializer.DeserializeAsync<TwoTimeEntry[]>(stream, _jsonSerializerOptions).FreeContext();
        }


        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            IgnoreNullValues = true,
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
