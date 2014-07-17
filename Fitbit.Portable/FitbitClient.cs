using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public class FitbitClient : IFitbitClient
    {
        private string _consumerKey;
        private string _consumerSecret;
        private string _accessToken;
        private string _accessSecret;
        private HttpClient httpClient;
        
        public FitbitClient(string consumerKey, string consumerSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
        }

        public FitbitClient(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _accessToken = accessToken;
            _accessSecret = accessSecret;

            httpClient = AsyncOAuth.OAuthUtility.CreateOAuthClient(_consumerKey, _consumerSecret, new AsyncOAuth.AccessToken(_accessToken, _accessSecret));
        }

        public async Task<UserProfile> GetUserProfileAsync(string encodedUserId = default(string))
        {
            string apiCall;

            if (string.IsNullOrWhiteSpace(encodedUserId))
                apiCall = "/1/user/-/profile.json";
            else
                apiCall = string.Format("/1/user/{0}/profile.json", encodedUserId);

            var fullCall = PrepareUrl(apiCall);

            HttpResponseMessage response = await httpClient.GetAsync(fullCall);
            HandleResponse(response);

            string responseBody = await response.Content.ReadAsStringAsync();

            var serializer = new JsonDotNetSerializer {RootProperty = "user"};
            var user = serializer.Deserialize<UserProfile>(responseBody);

            return user;
        }

        private string PrepareUrl(string apiCall)
        {
            if (apiCall.StartsWith("/"))
            {
                apiCall = apiCall.TrimStart(new[] { '/' });
            }
            return Constants.BaseApiUrl + apiCall;
        }

        private async void HandleResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            IList<ApiError> errors = null;
            try
            {
                var serializer = new JsonDotNetSerializer {RootProperty = "errors"};
                errors = serializer.Deserialize<List<ApiError>>(await response.Content.ReadAsStringAsync());
            }
            finally
            {
                // If there's an issue deserializing the error we still want to raise a fitbit exception so
                // whatever happens we need an instantiated error list to create a FitbitException with
                errors = errors ?? new List<ApiError>();
            }

            HttpStatusCode httpStatusCode = response.StatusCode;

            FitbitException exception = new FitbitException("Http Error:" + httpStatusCode, httpStatusCode, errors);

            var retryAfterHeader = response.Headers.RetryAfter;
            if (retryAfterHeader != null)
            {
                if (retryAfterHeader.Delta.HasValue)
                {
                    exception.RetryAfter = retryAfterHeader.Delta.Value.Seconds;
                }
            }

            throw exception;
        }
    }
}