using System;
using System.Net.Http;
using Fitbit.Api.Portable;

namespace Fitbit.OAuth1Migration
{
    public static class FitbitClientOA1Factory
    {
        /// <summary>
        /// Private base constructor which takes it all and constructs or throws exceptions as appropriately
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="accessToken"></param>
        /// <param name="accessSecret"></param>
        /// <param name="httpClient"></param>
        public static FitbitClient Create(string consumerKey, string consumerSecret, string accessToken, string accessSecret, HttpClient httpClient = null)
        {
            throw new NotImplementedException();

            var HttpClient = httpClient;
            if (HttpClient == null)
            {
                #region Parameter checking
                if (string.IsNullOrWhiteSpace(consumerKey))
                {
                    throw new ArgumentNullException(nameof(consumerKey), "ConsumerKey must not be empty or null");
                }

                if (string.IsNullOrWhiteSpace(consumerSecret))
                {
                    throw new ArgumentNullException(nameof(consumerSecret), "ConsumerSecret must not be empty or null");
                }

                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    throw new ArgumentNullException(nameof(accessToken), "AccessToken must not be empty or null");
                }

                if (string.IsNullOrWhiteSpace(accessSecret))
                {
                    throw new ArgumentNullException(nameof(accessSecret), "AccessSecret must not be empty or null");
                }
                #endregion

                HttpClient = AsyncOAuth.OAuthUtility.CreateOAuthClient(consumerKey, consumerSecret, new AsyncOAuth.AccessToken(accessToken, accessSecret));
            }
        }

        /// <summary>
        /// Use this constructor if the access tokens and keys are known. A httpclient with the correct authorizaton information will be setup to use in the calls.
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="accessToken"></param>
        /// <param name="accessSecret"></param>
        public static FitbitClient Create(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
        {
            // note: do not remove the httpclient optional parameter above, even if resharper says you should, as otherwise it will make a cyclic constructor call .... which is bad!
            return FitbitClientOA1Factory.Create(consumerKey, consumerSecret, accessToken, accessSecret, httpClient: null);
        }
    }
}
