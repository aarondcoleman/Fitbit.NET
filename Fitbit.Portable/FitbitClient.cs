using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public class FitbitClient : IFitbitClient
    {
        /// <summary>
        /// The httpclient which will be used for the api calls through the FitbitClient instance
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        /// <summary>
        /// Use this constructor if an authorized httpclient has already been setup and accessing the resources is what is required.
        /// </summary>
        /// <param name="httpClient"></param>
        public FitbitClient(HttpClient httpClient) : this(string.Empty, string.Empty, string.Empty, string.Empty, httpClient)
        {
        }

        /// <summary>
        /// Use this constructor if the access tokens and keys are known. A httpclient with the correct authorizaton information will be setup to use in the calls.
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="accessToken"></param>
        /// <param name="accessSecret"></param>
        public FitbitClient(string consumerKey, string consumerSecret, string accessToken, string accessSecret) : this(consumerKey, consumerSecret, accessToken, accessSecret, httpClient: null)
        {
            // note: do not remove the httpclient optional parameter above, even if resharper says you should, as otherwise it will make a cyclic constructor call .... which is bad!
        }

        private FitbitClient(string consumerKey, string consumerSecret, string accessToken, string accessSecret, HttpClient httpClient = null)
        {
            HttpClient = httpClient;
            if (HttpClient == null)
            {
                #region Parameter checking
                if (string.IsNullOrWhiteSpace(consumerKey))
                {
                    throw new ArgumentNullException("consumerKey", "ConsumerKey must not be empty or null");
                }

                if (string.IsNullOrWhiteSpace(consumerSecret))
                {
                    throw new ArgumentNullException("consumerSecret", "ConsumerSecret must not be empty or null");
                }

                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    throw new ArgumentNullException("accessToken", "AccessToken must not be empty or null");
                }

                if (string.IsNullOrWhiteSpace(accessSecret))
                {
                    throw new ArgumentNullException("accessSecret", "AccessSecret must not be empty or null");
                }
                #endregion

                HttpClient = AsyncOAuth.OAuthUtility.CreateOAuthClient(consumerKey, consumerSecret, new AsyncOAuth.AccessToken(accessToken, accessSecret));
            }
        }

        /// <summary>
        /// Requests the devices for the current logged in user
        /// </summary>
        /// <returns>List of <see cref="Device"/></returns>
        public async Task<FitbitResponse<List<Device>>> GetDevicesAsync()
        {
            var apiCall = "/1/user/-/devices.json".ToFullUrl();

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<List<Device>>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer();
                fitbitResponse.Data = serializer.Deserialize<List<Device>>(responseBody);    
            }
            return fitbitResponse;
        }

        /// <summary>
        /// Requests the friends of the encoded user id or if none supplied the current logged in user
        /// </summary>
        /// <param name="encodedUserId">encoded user id, can be null for current logged in user</param>
        /// <returns>List of <see cref="UserProfile"/></returns>
        public async Task<FitbitResponse<List<UserProfile>>> GetFriendsAsync(string encodedUserId = default(string))
        {
            string apiCall = "/1/user/{0}/friends.json".ToFullUrl(encodedUserId);

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<List<UserProfile>>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer();
                fitbitResponse.Data = serializer.GetFriends(responseBody);
            }
            return fitbitResponse;
        }

        /// <summary>
        /// Requests the user profile of the encoded user id or if none specified the current logged in user
        /// </summary>
        /// <param name="encodedUserId"></param>
        /// <returns><see cref="UserProfile"/></returns>
        public async Task<FitbitResponse<UserProfile>> GetUserProfileAsync(string encodedUserId = default(string))
        {
            string apiCall = "/1/user/{0}/profile.json".ToFullUrl(encodedUserId);

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<UserProfile>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer { RootProperty = "user" };
                fitbitResponse.Data = serializer.Deserialize<UserProfile>(responseBody);    
            }
            return fitbitResponse;
        }

        /// <summary>
        /// Requests the specified <see cref="TimeSeriesResourceType"/> for the date range and user specified
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<TimeSeriesDataList>> GetTimeSeriesAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string encodedUserId = default(string))
        {
            return await GetTimeSeriesAsync(timeSeriesResourceType, startDate, endDate.ToFitbitFormat(), encodedUserId);
        }

        /// <summary>
        /// Requests the specified <see cref="TimeSeriesResourceType"/> for the date range and user specified 
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="endDate"></param>
        /// <param name="period"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<TimeSeriesDataList>> GetTimeSeriesAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period, string encodedUserId = default(string))
        {
            return await GetTimeSeriesAsync(timeSeriesResourceType, endDate, period.GetStringValue(), encodedUserId);
        }

        /// <summary>
        /// Requests the specified <see cref="TimeSeriesResourceType"/> for the date range and user specified
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="baseDate"></param>
        /// <param name="endDateOrPeriod"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        private async Task<FitbitResponse<TimeSeriesDataList>> GetTimeSeriesAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime baseDate, string endDateOrPeriod, string encodedUserId = default(string))
        {
            var apiCall = "/1/user/{0}{1}/date/{2}/{3}.json".ToFullUrl(encodedUserId, timeSeriesResourceType.GetStringValue(), baseDate.ToFitbitFormat(), endDateOrPeriod);

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<TimeSeriesDataList>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer {RootProperty = timeSeriesResourceType.ToTimeSeriesProperty()};
                fitbitResponse.Data = serializer.GetTimeSeriesDataList(responseBody);   
            }
            return fitbitResponse;
        }

        /// <summary>
        /// Requests the specified <see cref="TimeSeriesResourceType"/> for the date range and user specified
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public Task<FitbitResponse<TimeSeriesDataListInt>> GetTimeSeriesIntAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string encodedUserId = null)
        {
            return GetTimeSeriesIntAsync(timeSeriesResourceType, startDate, endDate.ToFitbitFormat(), encodedUserId);
        }

        /// <summary>
        /// Requests the specified <see cref="TimeSeriesResourceType"/> for the date range and user specified
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="endDate"></param>
        /// <param name="period"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public Task<FitbitResponse<TimeSeriesDataListInt>> GetTimeSeriesIntAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period, string encodedUserId = null)
        {
            return GetTimeSeriesIntAsync(timeSeriesResourceType, endDate, period.GetStringValue(), encodedUserId);
        }

        /// <summary>
        /// Get TimeSeries data for another user accessible with this user's credentials
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="baseDate"></param>
        /// <param name="endDateOrPeriod"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        private async Task<FitbitResponse<TimeSeriesDataListInt>> GetTimeSeriesIntAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime baseDate, string endDateOrPeriod, string encodedUserId)
        {
            var apiCall = "/1/user/{0}{1}/date/{2}/{3}.json".ToFullUrl(encodedUserId, timeSeriesResourceType.GetStringValue(), baseDate.ToFitbitFormat(), endDateOrPeriod);

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<TimeSeriesDataListInt>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer { RootProperty = timeSeriesResourceType.ToTimeSeriesProperty() };
                fitbitResponse.Data = serializer.GetTimeSeriesDataListInt(responseBody);
            }
            return fitbitResponse;
        }

        /// <summary>
        /// General error checking of the response before specific processing is done.
        /// </summary>
        /// <param name="response"></param>
        private async Task<FitbitResponse<T>> HandleResponse<T>(HttpResponseMessage response) where T : class
        {
            var errors = new List<ApiError>();
            
            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var serializer = new JsonDotNetSerializer { RootProperty = "errors" };
                    errors.AddRange(serializer.Deserialize<List<ApiError>>(await response.Content.ReadAsStringAsync()));
                }
                catch
                {
                    // if there is an error with the serialization then we need to default the errors back to an instantiated list
                    errors = new List<ApiError>();
                }  
            }

            // todo: handle "success" responses which return errors?

            return new FitbitResponse<T>(response.StatusCode, response.Headers, errors);
        }
    }
}