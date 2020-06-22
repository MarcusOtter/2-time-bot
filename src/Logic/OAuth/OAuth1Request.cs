using System;
using System.Collections.Generic;
using System.Web;

namespace Logic.OAuth
{
    public interface IOAuth1Request
    {
        void SetHeader(string headerKey, string value);
        void AddNewHeader(string newHeaderKey, string value);
    }

    public class OAuth1Request : IOAuth1Request
    {
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>()
        {
            { "oauth_consumer_key",     string.Empty },
            { "oauth_nonce",            string.Empty },
            { "oauth_signature",        string.Empty },
            { "oauth_signature_method", string.Empty },
            { "oauth_timestamp",        string.Empty },
            { "oauth_token",            string.Empty },
            { "oauth_version",          string.Empty }
        };

        public OAuth1Request(string consumerKey, string consumerSecret, string accessToken, string tokenSecret)
        {
            SetHeader("oauth_consumer_key", consumerKey);
            SetHeader("oauth_signature", "---");
            SetHeader("oauth_signature_method", "---");
            SetHeader("oauth_timestamp", "---");
            SetHeader("oauth_nonce", GetRandomAlphanumericString());
            SetHeader("oauth_token", "---");
            SetHeader("oauth_version", "---");
        }

        public void SetHeader(string headerKey, string value)
        {
            if (!_headers.ContainsKey(headerKey))
            {
                throw new ArgumentException($"The header {headerKey} does not exist. Use {nameof(AddNewHeader)} if you want to add a new header.");
            }

            _headers[headerKey] = HttpUtility.UrlEncode(value);
        }

        public void AddNewHeader(string newHeaderKey, string value)
        {
            if (_headers.ContainsKey(newHeaderKey))
            {
                throw new ArgumentException($"The header {newHeaderKey} already exists. Use {nameof(SetHeader)} if you want to add a new header.");
            }

            _headers.Add(newHeaderKey, value);
        }

        private string GetRandomAlphanumericString()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
