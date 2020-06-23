using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Logic.OAuth
{
    public interface IOAuth1Client
    {
        Task<HttpResponseMessage> PostAsync(Uri requestUri);
    }

    public class OAuth1Client : IOAuth1Client
    {
        private readonly HttpClient _httpClient;

        public OAuth1Client(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<HttpResponseMessage> PostAsync(Uri requestUri)
        {
            // This is temporary, will come from storage later
            var consumerKey    = "Redacted";
            var consumerSecret = "Redacted";
            var accessToken    = "Redacted";
            var tokenSecret    = "Redacted";

            var oauth1Request = new OAuth1Request(HttpMethod.Post, requestUri, consumerKey, consumerSecret, accessToken, tokenSecret);
            var httpRequest = oauth1Request.ToHttpRequestMessage();

            httpRequest.Headers.Connection.Clear();
            httpRequest.Headers.ConnectionClose = true;

            return _httpClient.SendAsync(httpRequest);
        }
    }
}
