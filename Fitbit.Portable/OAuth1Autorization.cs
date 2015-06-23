using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Fitbit.Api.Portable
{
    public class OAuth1Autorization : IAuthorization
    {
        private string _consumerKey;
        private string _consumerSecret;
        private string _accessToken;
        private string _accessSecret;

        public OAuth1Autorization(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _accessToken = accessToken;
            _accessSecret = accessSecret;
        }

        public HttpClient CreateAuthorizedHttpClient()
        {
            #region Parameter checking
            if (string.IsNullOrWhiteSpace(_consumerKey))
            {
                throw new ArgumentNullException("consumerKey", "ConsumerKey must not be empty or null");
            }

            if (string.IsNullOrWhiteSpace(_consumerSecret))
            {
                throw new ArgumentNullException("consumerSecret", "ConsumerSecret must not be empty or null");
            }

            if (string.IsNullOrWhiteSpace(_accessToken))
            {
                throw new ArgumentNullException("accessToken", "AccessToken must not be empty or null");
            }

            if (string.IsNullOrWhiteSpace(_accessSecret))
            {
                throw new ArgumentNullException("accessSecret", "AccessSecret must not be empty or null");
            }
            #endregion

            HttpClient httpClient = AsyncOAuth.OAuthUtility.CreateOAuthClient(_consumerKey, _consumerSecret, new AsyncOAuth.AccessToken(_accessToken, _accessSecret));

            return httpClient;

        }
    }
}
