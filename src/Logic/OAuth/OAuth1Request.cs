using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Logic.OAuth
{
    public interface IOAuth1Request
    {
        void AddNewHeader(string newHeaderKey, string value);
        HttpRequestMessage ToHttpRequestMessage();
        void RemoveHeader(string headerKey);
        void SetHeader(string headerKey, string value);
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

        private readonly HttpMethod _httpMethod;
        private readonly Uri _requestUri;

        public OAuth1Request(
            HttpMethod httpMethod,
            Uri requestUri,
            string consumerKey,
            string consumerSecret,
            string accessToken,
            string tokenSecret)
        {
            _httpMethod = httpMethod;
            _requestUri = requestUri;

            SetHeader("oauth_consumer_key", consumerKey);
            SetHeader("oauth_nonce", GetRandomAlphanumericString());
            SetHeader("oauth_signature_method", "HMAC-SHA1");
            SetHeader("oauth_timestamp", GetUnixEpochTimeNow().ToString());
            SetHeader("oauth_token", accessToken);
            SetHeader("oauth_version", "1.0");

            // Must be after the rest of the headers
            var signingKey = GetSigningKey(consumerSecret, tokenSecret);
            var signatureBaseString = GetSignatureBaseString();
            var signature = CalculateSignature(signatureBaseString, signingKey);
            SetHeader("oauth_signature", signature);
        }

        public void AddNewHeader(string newHeaderKey, string value)
        {
            if (_headers.ContainsKey(newHeaderKey))
            {
                ThrowHeaderMissingException(newHeaderKey, $"Use {nameof(SetHeader)} if you want to add a new header");
            }

            var newKey = HttpUtility.UrlEncode(newHeaderKey);
            var newValue = HttpUtility.UrlEncode(value);

            _headers.Add(newKey, newValue);
        }

        public void RemoveHeader(string headerKey)
        {
            if (!_headers.ContainsKey(headerKey))
            {
                ThrowHeaderMissingException(headerKey);
            }

            _headers.Remove(headerKey);
        }

        public void SetHeader(string headerKey, string value)
        {
            if (!_headers.ContainsKey(headerKey))
            {
                ThrowHeaderMissingException(headerKey, $"Use {nameof(AddNewHeader)} if you want to add a new header");
            }

            var newKey = HttpUtility.UrlEncode(headerKey);
            var newValue = HttpUtility.UrlEncode(value);

            _headers[newKey] = newValue;
        }

        public HttpRequestMessage ToHttpRequestMessage()
        {
            ThrowIfUninitializedHeaders(_headers.Keys);

            var authHeaderString = GetAuthHeaderString();
            var authHeader = new AuthenticationHeaderValue("OAuth", authHeaderString);

            var requestMessage = new HttpRequestMessage();
            requestMessage.Headers.Authorization = authHeader;
            requestMessage.RequestUri = _requestUri;
            requestMessage.Method = _httpMethod;

            return requestMessage;
        }

        private string GetAuthHeaderString()
        {
            var parameters = new string[_headers.Count];
            for (int i = 0; i < _headers.Count; i++)
            {
                var header = _headers.ElementAt(i);
                parameters[i] = $"{header.Key}=\"{header.Value}\"";
            }
            return string.Join(", ", parameters);
        }

        private string CalculateSignature(string signatureBaseString, string signingKey)
        {
            var signingKeyBytes = Encoding.ASCII.GetBytes(signingKey);
            var signatureBaseBytes = Encoding.ASCII.GetBytes(signatureBaseString);

            var hashingAlgorithm = new HMACSHA1(signingKeyBytes);
            var signatureBytes = hashingAlgorithm.ComputeHash(signatureBaseBytes);
            return Convert.ToBase64String(signatureBytes);
        }

        // https://developer.twitter.com/en/docs/basics/authentication/oauth-1-0a/creating-a-signature
        private string GetSignatureBaseString()
        {
            ThrowIfUninitializedHeaders(_headers.Keys.Where(x => x != "oauth_signature"));

            _headers.OrderBy(x => x.Key);
            var parameterStringBuilder = new StringBuilder();

            for(int i = 0; i < _headers.Count; i++)
            {
                var header = _headers.ElementAt(i);

                parameterStringBuilder
                    .Append(header.Key)
                    .Append('=')
                    .Append(header.Value);

                // Don't append '&' on last iteration
                if (i == _headers.Count - 1) { break; }
                parameterStringBuilder.Append('&');
            }

            var encodedHttpMethod = HttpUtility.UrlEncode(_httpMethod.Method.ToUpperInvariant());
            var encodedUrl = HttpUtility.UrlEncode(_requestUri.AbsoluteUri);
            var encodedParameterString = HttpUtility.UrlEncode(parameterStringBuilder.ToString());

            return $"{encodedHttpMethod}&{encodedUrl}&{encodedParameterString}";
        }

        private string GetSigningKey(string consumerSecret, string tokenSecret)
        {
            var encodedConsumerSecret = HttpUtility.UrlEncode(consumerSecret);
            var encodedTokenSecret = HttpUtility.UrlEncode(tokenSecret);

            return $"{encodedConsumerSecret}&{encodedTokenSecret}";
        }

        private void ThrowIfUninitializedHeaders(IEnumerable<string> headerKeysToCheck)
        {
            foreach(var headerKey in headerKeysToCheck)
            {
                if (!_headers.ContainsKey(headerKey))
                {
                    ThrowHeaderMissingException(headerKey);
                }

                if (_headers[headerKey] == string.Empty)
                { 
                    throw new InvalidOperationException($"The header \"{headerKey}\" has not been initialized");
                }
            }
        }

        private void ThrowHeaderMissingException(string headerKey, string additionalInfo = "")
        {
            var defaultMessage = $"The header \"{headerKey}\" does not exist";
            var message = string.IsNullOrEmpty(additionalInfo)
                ? defaultMessage
                : $"{defaultMessage}. {additionalInfo}";

            throw new KeyNotFoundException(message);
        }

        private long GetUnixEpochTimeNow()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        private string GetRandomAlphanumericString()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
