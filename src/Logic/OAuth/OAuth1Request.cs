using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logic.OAuth
{
    public interface IOAuth1Request
    {
        void SetHeader(string headerKey, string value);
    }

    public class OAuth1Request : IOAuth1Request
    {
        private readonly Random _random;

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
            _random = new Random();

            SetHeader("oauth_consumer_key", consumerKey);
            SetHeader("oauth_nonce", GetRandomUrlSafeString(42, _random));
            SetHeader("oauth_signature", "---");
            SetHeader("oauth_signature_method", "---");
            SetHeader("oauth_timestamp", "---");
            SetHeader("oauth_token", "---");
            SetHeader("oauth_version", "---");
        }

        public void SetHeader(string headerKey, string value)
        {
            if (!_headers.ContainsKey(headerKey))
            {
                throw new ArgumentException($"The header {headerKey} does not exist. Use AddHeader() if you want to add a new header.");
            }

            _headers[headerKey] = HttpUtility.UrlEncode(value);
        }


        // Will probably remove the two methods below and instead use an easier method:
        // Base64 encode 32 bytes of random data, strip out all non-alphanumeric characters

        private string GetRandomUrlSafeString(int length, Random random)
        {
            var safeCharacters = GetAllSafeUrlCharacters();

            var output = new char[length];
            for (var i = 0; i < length; i++)
            {
                output[i] = safeCharacters[random.Next(0, safeCharacters.Length)];
            }

            return new string(output);
        }

        private char[] GetAllSafeUrlCharacters()
        {
            // Some safe url characters, used a unicode table for these numbers.
            var charIntegerValue = Enumerable.Range(48, 59)
                .Concat(Enumerable.Range(64, 90))
                .Concat(Enumerable.Range(97, 122))
                .ToArray();

            var output = new char[charIntegerValue.Length];

            for (int i = 0; i < charIntegerValue.Length; i++)
            {
                // Convert the unicode integer value to the character
                output[i] = (char) charIntegerValue[i];
            }

            return output;
        }
    }
}
