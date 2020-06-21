using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Logic.OAuth
{
    public interface IOAuth1Client
    {

    }

    public class OAuth1Client : IOAuth1Client
    {
        private readonly HttpClient _httpClient;

        public OAuth1Client(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


    }
}
