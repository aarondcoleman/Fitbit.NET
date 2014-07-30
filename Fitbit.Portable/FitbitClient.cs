using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public class FitbitClient : IFitbitClient
    {
        private HttpClient httpClient;

        public FitbitClient(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
        {
            httpClient = AsyncOAuth.OAuthUtility.CreateOAuthClient(consumerKey, consumerSecret, new AsyncOAuth.AccessToken(accessToken, accessSecret));
        }

        /// <summary>
        /// Requests the devices for the current logged in user
        /// </summary>
        /// <returns>List of <see cref="Device"/></returns>
        public async Task<List<Device>> GetDevicesAsync()
        {
            var apiCall = "/1/user/-/devices.json".ToFullUrl();

            HttpResponseMessage response = await httpClient.GetAsync(apiCall);
            HandleResponse(response);

            string responseBody = await response.Content.ReadAsStringAsync();

            var serializer = new JsonDotNetSerializer();
            return serializer.Deserialize<List<Device>>(responseBody);
        }

        /// <summary>
        /// Requests the friends of the encoded user id or if none supplied the current logged in user
        /// </summary>
        /// <param name="encodedUserId">encoded user id, can be null for current logged in user</param>
        /// <returns>List of <see cref="UserProfile"/></returns>
        public async Task<List<UserProfile>> GetFriendsAsync(string encodedUserId = default(string))
        {
            string apiCall;

            if (string.IsNullOrWhiteSpace(encodedUserId))
                apiCall = "/1/user/-/friends.json";
            else
                apiCall = string.Format("/1/user/{0}/friends.json", encodedUserId);

            HttpResponseMessage response = await httpClient.GetAsync(apiCall.ToFullUrl());
            HandleResponse(response);

            string responseBody = await response.Content.ReadAsStringAsync();

            var serializer = new JsonDotNetSerializer();
            return serializer.GetFriends(responseBody);
        }

        /// <summary>
        /// Requests the user profile of the encoded user id or if none specified the current logged in user
        /// </summary>
        /// <param name="encodedUserId"></param>
        /// <returns><see cref="UserProfile"/></returns>
        public async Task<UserProfile> GetUserProfileAsync(string encodedUserId = default(string))
        {
            string apiCall;

            if (string.IsNullOrWhiteSpace(encodedUserId))
                apiCall = "/1/user/-/profile.json";
            else
                apiCall = string.Format("/1/user/{0}/profile.json", encodedUserId);

            HttpResponseMessage response = await httpClient.GetAsync(apiCall.ToFullUrl());
            HandleResponse(response);

            string responseBody = await response.Content.ReadAsStringAsync();

            var serializer = new JsonDotNetSerializer {RootProperty = "user"};
            return serializer.Deserialize<UserProfile>(responseBody);
        }

        private async void HandleResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            IList<ApiError> errors = null;
            try
            {
                var serializer = new JsonDotNetSerializer { RootProperty = "errors" };
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