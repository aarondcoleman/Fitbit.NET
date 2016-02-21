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
        public static FitbitClient Create(string consumerKey, string consumerSecret, string accessToken, string accessSecret, IFitbitInterceptor interceptor = null)
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

            if (interceptor != null)
            {
                return new FitbitClient(mh =>
                {
                    //Injecting the Message Handler provided by FitbitClient library (mh) allows us to invoke any IFitbitInterceptor that has been registered
                    return AsyncOAuth.OAuthUtility.CreateOAuthClient(mh, consumerKey, consumerSecret,
                        new AsyncOAuth.AccessToken(accessToken, accessSecret));
                }, interceptor);
            }
            else
            {
                return new FitbitClient(mh =>
                {
                    return AsyncOAuth.OAuthUtility.CreateOAuthClient(consumerKey, consumerSecret,
                        new AsyncOAuth.AccessToken(accessToken, accessSecret));
                });
            }
        }
    }
}
