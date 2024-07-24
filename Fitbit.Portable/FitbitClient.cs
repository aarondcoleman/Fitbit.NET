﻿using Fitbit.Api.Portable.Models;
using Fitbit.Api.Portable.OAuth2;
using Fitbit.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Fitbit.Api.Portable
{
    public class FitbitClient : IFitbitClient
    {
        private CancellationTokenSource _cancellationTokenSource = null;

        public FitbitAppCredentials AppCredentials { get; private set; }

        public OAuth2AccessToken AccessToken { get; set; }

        /// <summary>
        /// The httpclient which will be used for the api calls through the FitbitClient instance
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        public ITokenManager TokenManager { get; private set; }
        public bool OAuth2TokenAutoRefresh { get; set; }
        public List<IFitbitInterceptor> FitbitInterceptorPipeline { get; private set; }

        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Simplest constructor for OAuth2- requires the minimum information required by FitBit.Net client to make succesful calls to Fitbit Api
        /// </summary>
        /// <param name="credentials">Obtain this information from your developer dashboard. App credentials are required to perform token refresh</param>
        /// <param name="accessToken">Authenticate with Fitbit API using OAuth2. Authenticator2 class is a helper for this process</param>
        /// <param name="interceptor">An interface that enables sniffing all outgoing and incoming http requests from FitbitClient</param>
        public FitbitClient(FitbitAppCredentials credentials, OAuth2AccessToken accessToken, IFitbitInterceptor interceptor = null,
            bool enableOAuth2TokenRefresh = true, ITokenManager tokenManager = null, HttpClient httpClient = null, CancellationToken cancellationToken = default)
        {
            if (accessToken == null)
            {
                throw new ArgumentNullException("accessToken");
            }

            if (cancellationToken != default)
            {
                CancellationToken = cancellationToken;
            }
            else
            {
                _cancellationTokenSource = new CancellationTokenSource();
                CancellationToken = _cancellationTokenSource.Token;
            }

            this.AppCredentials = credentials;
            this.AccessToken = accessToken;

            this.FitbitInterceptorPipeline = new List<IFitbitInterceptor>();


            if (interceptor != null)
            {
                this.FitbitInterceptorPipeline.Add(interceptor);
            }

            ConfigureTokenManager(tokenManager);

            //Auto refresh should always be the last handle to be registered.
            ConfigureAutoRefresh(enableOAuth2TokenRefresh);

            CreateHttpClientForOAuth2(httpClient);
        }

        private void ConfigureAutoRefresh(bool enableOAuth2TokenRefresh)
        {
            this.OAuth2TokenAutoRefresh = enableOAuth2TokenRefresh;
            if (OAuth2TokenAutoRefresh)
                this.FitbitInterceptorPipeline.Add(new OAuth2AutoRefreshInterceptor());
            return;
        }

        /// <summary>
        /// Simplest constructor for OAuth2- requires the minimum information required by FitBit.Net client to make succesful calls to Fitbit Api
        /// </summary>
        /// <param name="credentials">Obtain this information from your developer dashboard. App credentials are required to perform token refresh</param>
        /// <param name="accessToken">Authenticate with Fitbit API using OAuth2. Authenticator2 class is a helper for this process</param>
        /// <param name="interceptor">An interface that enables sniffing all outgoing and incoming http requests from FitbitClient</param>
        public FitbitClient(FitbitAppCredentials credentials, OAuth2AccessToken accessToken, List<IFitbitInterceptor> interceptors, bool enableOAuth2TokenRefresh = true,
            ITokenManager tokenManager = null, HttpClient httpClient = null, CancellationToken cancellationToken = default)
        {
            if (accessToken == null)
            {
                throw new ArgumentNullException("accessToken");
            }

            if (cancellationToken != default)
            {
                CancellationToken = cancellationToken;
            }
            else
            {
                _cancellationTokenSource = new CancellationTokenSource();
                CancellationToken = _cancellationTokenSource.Token;
            }

            this.AppCredentials = credentials;
            this.AccessToken = accessToken;

            this.FitbitInterceptorPipeline = new List<IFitbitInterceptor>();

            if (interceptors != null && interceptors.Count > 0)
                this.FitbitInterceptorPipeline.AddRange(interceptors);

            ConfigureTokenManager(tokenManager);

            //Auto refresh should always be the last handle to be registered.
            ConfigureAutoRefresh(enableOAuth2TokenRefresh);
            CreateHttpClientForOAuth2(httpClient);

        }


        public FitbitClient(FitbitAppCredentials credentials, OAuth2AccessToken accessToken, bool enableOAuth2TokenRefresh) : this(credentials, accessToken, null, enableOAuth2TokenRefresh)
        {

        }

        public FitbitClient(FitbitAppCredentials credentials, OAuth2AccessToken accessToken, List<IFitbitInterceptor> interceptors, bool enableOAuth2TokenRefresh) : this(credentials, accessToken, interceptors, enableOAuth2TokenRefresh, null)
        {

        }

        public FitbitClient(FitbitAppCredentials credentials, OAuth2AccessToken accessToken, List<IFitbitInterceptor> interceptors, ITokenManager tokenManager) : this(credentials, accessToken, interceptors, true, tokenManager)
        {

        }

        public FitbitClient(FitbitAppCredentials credentials, OAuth2AccessToken accessToken, IFitbitInterceptor interceptor, ITokenManager tokenManager) : this(credentials, accessToken, interceptor, true, tokenManager)
        {

        }

        /// <summary>
        /// Advanced mode for library usage. Allows custom creation of HttpClient to account for future authentication methods
        /// </summary>
        /// <param name="customFactory">A function or lambda expression who is in charge of creating th HttpClient. It takes as an argument a HttpMessageHandler which does wiring for IFitbitInterceptor. To use IFitbitInterceptor you must pass this HttpMessageHandler as anargument to the constuctor of HttpClient</param>
        /// <param name="interceptor">An interface that enables sniffing all outgoing and incoming http requests from FitbitClient</param>
        public FitbitClient(Func<HttpMessageHandler, HttpClient> customFactory, IFitbitInterceptor interceptor = null, ITokenManager tokenManager = null)
        {
            this.OAuth2TokenAutoRefresh = false;

            ConfigureTokenManager(tokenManager);
            this.HttpClient = customFactory(new FitbitHttpMessageHandler(this, interceptor));
        }

        private void ConfigureTokenManager(ITokenManager tokenManager)
        {
            TokenManager = tokenManager ?? new DefaultTokenManager();
        }

        private void CreateHttpClientForOAuth2(HttpClient httpClient = null)
        {
            if (httpClient != null)
            {
                this.HttpClient = httpClient;
            }
            else
            {
                HttpMessageHandler pipeline = this.CreatePipeline(FitbitInterceptorPipeline);

                if (pipeline != null)
                {
                    this.HttpClient = new HttpClient(pipeline);
                }
                else
                {
                    this.HttpClient = new HttpClient();
                }
            }
        }

        private HttpRequestMessage GetRequest(HttpMethod method, string requestUri)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, requestUri);

            if (!string.IsNullOrEmpty(AccessToken?.Token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken.Token);
            }

            return request;
        }

        /// <summary>
        /// Creates the processing request pipeline using the message handlers, not specific to an instance of FitbitClient
        /// </summary>
        /// <param name="interceptors"></param>
        /// <returns>HttpMessageHandler</returns>
        public static HttpMessageHandler CreatePipeline(List<IFitbitInterceptor> interceptors, int? maxConnectionsPerServer = null)
        {
            if (interceptors.Count > 0)
            {
                // inspired by the code referenced from the web api source; this creates the russian doll effect
                FitbitHttpMessageHandler innerHandler = new FitbitHttpMessageHandler(null, interceptors[0], maxConnectionsPerServer);

                var innerHandlers = interceptors.GetRange(1, interceptors.Count - 1);

                foreach (var handler in innerHandlers)
                {
                    var messageHandler = new FitbitHttpMessageHandler(null, handler, maxConnectionsPerServer);
                    messageHandler.InnerHandler = innerHandler;
                    innerHandler = messageHandler;
                }

                return innerHandler;
            }

            return null;
        }

        public async Task<OAuth2AccessToken> RefreshOAuth2TokenAsync()
        {
            AccessToken = await TokenManager.RefreshTokenAsync(this);
            return AccessToken;
        }

        /// <summary>
        /// Requests the activity details of the encoded user id or if none supplied the current logged in user for the specified date
        /// </summary>
        /// <param name="activityDate"></param>
        /// <param name="encodedUserId">encoded user id, can be null for current logged in user</param>
        /// <returns>FitbitResponse of <see cref="ActivitySummary"/></returns>
        public async Task<Activity> GetDayActivityAsync(DateTime activityDate, string encodedUserId = null)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/activities/date/{1}.json", encodedUserId, activityDate.ToFitbitFormat());
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer();
                    return serializer.Deserialize<Activity>(responseBody);
                }
            }
        }

        /// <summary>
        /// Requests the activity summary of the encoded user id or if none supplied the current logged in user for the specified date
        /// </summary>
        /// <param name="activityDate"></param>
        /// <param name="encodedUserId">encoded user id, can be null for current logged in user</param>
        /// <returns>FitbitResponse of <see cref="ActivitySummary"/></returns>
        public async Task<ActivitySummary> GetDayActivitySummaryAsync(DateTime activityDate, string encodedUserId = null)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/activities/date/{1}.json", encodedUserId, activityDate.ToFitbitFormat());
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer { RootProperty = "summary" };
                    return serializer.Deserialize<ActivitySummary>(responseBody);
                }
            }
        }

        /// <summary>
        /// Requests the lifetime activity details of the encoded user id or none supplied the current logged in user
        /// </summary>
        /// <param name="encodedUserId">encoded user id, can be null for current logged in user</param>
        /// <returns></returns>
        public async Task<ActivitiesStats> GetActivitiesStatsAsync(string encodedUserId = null)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/activities.json", encodedUserId);
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer();
                    return serializer.Deserialize<ActivitiesStats>(responseBody);
                }
            }
        }

        #region ECG
        /// <summary>
        /// The Get ECG Logs List endpoint returns a list of a user's ECG logs 
        /// before or after a given day with sort order.  
        /// **WARNING** ECG responses can be much larger than other API requests and this method should be used with caution.
        /// </summary>
        /// <param name="dateToList">Date to begin or end log list. Required.</param>
        /// <param name="dateDirection">Specify entries before or after the given date. (before/after)</param>
        /// <param name="sortDirection">The sort order of entries by date. (asc/desc)</param>
        /// <param name="encodedUserId">Encoded user id, can be null for current logged in user.</param>
        /// <returns></returns>
        public async Task<List<ECGLog>> GetECGLogListAsync(DateTime dateToList, string dateDirection, string sortDirection, string encodedUserId = null)
        {
            List<ECGLog> ECG = new List<ECGLog>();
            bool getMorePages = true;
            string setDateDirection, setSortDirection;

            int limit = 10;
            int offset = 0;

            setDateDirection = dateDirection?.IndexOf("after") != -1 ? DateDirection.After : DateDirection.Before;

            setSortDirection = sortDirection?.IndexOf("asc") != -1 ? SortDirection.Ascending : SortDirection.Descending;

            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/ecg/list.json?{1}={2}&sort={3}&limit={4}&offset={5}",
                encodedUserId, setDateDirection, dateToList.ToFitbitFormat(), setSortDirection, limit, offset);

            while (getMorePages)
            {
                using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
                {
                    using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                    {
                        await HandleResponse(response);

                        string responseBody = await response.Content.ReadAsStringAsync();
                        var serializer = new JsonDotNetSerializer();
                        var data = serializer.Deserialize<ECGLogListBase>(responseBody);

                        foreach (var log in data.ECGReadings)
                        {
                            if (log != null)
                                ECG.Add(log);
                        }

                        if (data.HasMorePages)
                        {
                            apiCall = Constants.BaseApiUrl + data.Pagination.Next.Substring(1);
                        }
                        else
                        {
                            getMorePages = false;
                        }
                    }
                }
            }

            return ECG;
        }

        /// <summary>
        /// The Get ECG Logs List endpoint returns a list of a user's ECG logs 
        /// within a specified date range, including result limit and sort direction.
        /// </summary>
        /// <param name="startDate">Date to begin log list. Required.</param>
        /// <param name="endDate">Date to end log list. Required.</param>
        /// <param name="limit">Specify entries before or after the given date. (before/after)</param>
        /// <param name="sortDirection">The sort order of entries by date. (asc/desc)</param>
        /// <param name="encodedUserId">Encoded user id, can be null for current logged in user.</param>
        /// <returns></returns>
        public async Task<List<ECGLog>> GetECGLogListAsync(DateTime startDate, DateTime endDate, string sortDirection, int limit, string encodedUserId = null)
        {
            List<ECGLog> ECG = new List<ECGLog>();
            bool getMorePages = true;
            string dateDirection, getSortDirection;

            limit = limit > 0 && limit <= 10 ? limit : 10;
            int offset = 0;
            dateDirection = DateDirection.After;
            getSortDirection = SortDirection.Ascending;

            sortDirection = sortDirection?.IndexOf("asc") != -1 ? SortDirection.Ascending : SortDirection.Descending;

            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/ecg/list.json?{1}={2}&sort={3}&limit={4}&offset={5}",
                encodedUserId, dateDirection, startDate.ToFitbitFormat(), getSortDirection, limit, offset);

            while (getMorePages)
            {
                using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
                {
                    using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                    {
                        await HandleResponse(response);

                        string responseBody = await response.Content.ReadAsStringAsync();
                        var serializer = new JsonDotNetSerializer();
                        var data = serializer.Deserialize<ECGLogListBase>(responseBody);

                        foreach (var log in data.ECGReadings)
                        {
                            if (log != null)
                                ECG.Add(log);
                        }

                        if (data.HasMorePages && ECG.LastOrDefault().StartTime < endDate.AddDays(1))
                        {
                            apiCall = Constants.BaseApiUrl + data.Pagination.Next.Substring(1);
                        }
                        else
                        {
                            getMorePages = false;
                        }
                    }
                }
            }

            foreach (var ecg in ECG.OrderByDescending(x => x.StartTime).ToList())
            {
                if (ecg.StartTime > endDate.AddDays(1))
                {
                    ECG.Remove(ecg);
                }
            }

            return sortDirection == SortDirection.Ascending ?
                ECG.OrderBy(x => x.StartTime).ToList() : ECG.OrderByDescending(x => x.StartTime).ToList();
        }
        #endregion ECG

        #region  Sleep

        /// <summary>
        /// Requests the sleep data for the specified date for the logged in user 
        /// NOTE: This is for the V1 of the sleep api which is now Deprecated
        /// </summary>
        /// <param name="sleepDate"></param>
        /// <returns></returns>
        public async Task<SleepData> GetSleepAsync(DateTime sleepDate)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/sleep/date/{1}.json", args: sleepDate.ToFitbitFormat());
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer();
                    var data = serializer.Deserialize<SleepData>(responseBody);
                    FitbitClientExtensions.ProcessSleepData(data);
                    return data;
                }
            }
        }

        /// <summary>
        /// Requests the sleep data for a specified date for the logged in user
        /// </summary>
        /// <param name="sleepDate"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public async Task<SleepLogDateBase> GetSleepDateAsync(DateTime sleepDate, string encodedUserId = null)
        {
            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1.2/user/{0}/sleep/date/{1}.json", encodedUserId, sleepDate.ToFitbitFormat());
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer();
                    var data = serializer.Deserialize<SleepLogDateBase>(responseBody);

                    return data;
                }
            }
        }

        /// <summary>
        /// Requests the sleep data for a specified date range for the logged in user
        /// </summary>
        /// <param name="endDate"></param>
        /// <param name="encodedUserId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public async Task<SleepDateRangeBase> GetSleepDateRangeAsync(DateTime startDate, DateTime endDate, string encodedUserId = null)
        {
            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1.2/user/{0}/sleep/date/{1}/{2}.json", encodedUserId, startDate.ToFitbitFormat(), endDate.ToFitbitFormat());
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer();
                    var data = serializer.Deserialize<SleepDateRangeBase>(responseBody);

                    return data;
                }
            }
        }


        /// <summary>
        /// The Get Sleep Logs List endpoint returns a list of a user's sleep logs (including naps) 
        /// before or after a given day with offset, limit, and sort order.
        /// </summary>
        /// <param name="dateToList">	The date in the format yyyy-MM-ddTHH:mm:ss. Only yyyy-MM-dd is required. Set sort to desc when using beforeDate.</param>
        /// <param name="decisionDate"></param>
        /// <param name="sort">The sort order of entries by date. Required. asc for ascending, desc for descending</param>
        /// <param name="limit">The max of the number of sleep logs returned. Required.</param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public async Task<SleepLogListBase> GetSleepLogListAsync(DateTime dateToList, SleepEnum decisionDate, SortEnum sort, int limit,
            string encodedUserId = null)
        {
            string setSleepDecision, setSort;

            //decide if date retrieval is before or after
            switch (decisionDate)
            {
                case SleepEnum.After:
                    setSleepDecision = "afterDate";
                    break;
                case SleepEnum.Before:
                    setSleepDecision = "beforeDate";
                    break;
                default:
                    //in case of some sort of error we will set our date to before
                    setSleepDecision = "beforeDate";
                    break;
            }

            //decide if we are sorting asc or dsc
            switch (sort)
            {
                case SortEnum.Asc:
                    setSort = "asc";
                    break;

                case SortEnum.Dsc:
                    setSort = "desc";
                    break;
                default:
                    //in case of some sort of error we will set our sort to asc
                    setSort = "asc";
                    break;
            }

            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1.2/user/{0}/sleep/list.json?{1}={2}&sort={3}&offset=0&limit={4}",
                encodedUserId, setSleepDecision, dateToList.ToFitbitFormat(), setSort, limit);

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serialzer = new JsonDotNetSerializer();
                    var data = serialzer.Deserialize<SleepLogListBase>(responseBody);

                    return data;
                }
            }
        }


        /// <summary>
        /// Creates a log entry for a sleep event and returns a response in the format requested
        /// </summary>
        /// <param name="startTime">Start time; hours and minutes in the format HH:mm. </param>
        /// <param name="duration">Duration in milliseconds.</param>
        /// <param name="date">Log entry date in the format yyyy-MM-dd. </param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public async Task<SleepLogDateRange> PostLogSleepAsync(string startTime, int duration, DateTime date, string encodedUserId = null)
        {

            var apiCall =
                FitbitClientHelperExtensions.ToFullUrl("/1.2/user/{0}/sleep.json?date={1}&startTime={2}&duration={3}",
                    encodedUserId, date.ToFitbitFormat(), startTime, duration);
            using (HttpRequestMessage request = GetRequest(HttpMethod.Post, apiCall))
            {
                request.Content = new StringContent(string.Empty);

                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responeBody = await response.Content.ReadAsStringAsync();
                    var serialzer = new JsonDotNetSerializer();

                    return serialzer.Deserialize<SleepLogDateRange>(responeBody);
                }
            }
        }

        #endregion Sleep

        /// <summary>
        /// Requests the devices for the current logged in user
        /// </summary>
        /// <returns>List of <see cref="Device"/></returns>
        public async Task<List<Device>> GetDevicesAsync()
        {
            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/devices.json");
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer();
                    return serializer.Deserialize<List<Device>>(responseBody);
                }
            }
        }

        /// <summary>
        /// Requests the friends of the encoded user id or if none supplied the current logged in user
        /// </summary>
        /// <param name="encodedUserId">encoded user id, can be null for current logged in user</param>
        /// <returns>List of <see cref="UserProfile"/></returns>
        public async Task<List<UserProfile>> GetFriendsAsync(string encodedUserId = default(string))
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/friends.json", encodedUserId);
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer();
                    return serializer.GetFriends(responseBody);
                }
            }
        }

        /// <summary>
        /// Requests the user profile of the encoded user id or if none specified the current logged in user
        /// </summary>
        /// <param name="encodedUserId"></param>
        /// <returns><see cref="UserProfile"/></returns>
        public async Task<UserProfile> GetUserProfileAsync(string encodedUserId = default(string))
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/profile.json", encodedUserId);
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer { RootProperty = "user" };
                    return serializer.Deserialize<UserProfile>(responseBody);
                }
            }
        }

        /// <summary>
        /// Requests the specified <see cref="TimeSeriesResourceType"/> for the date range and user specified
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public async Task<TimeSeriesDataList> GetTimeSeriesAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string encodedUserId = default(string))
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
        public async Task<TimeSeriesDataList> GetTimeSeriesAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period, string encodedUserId = default(string))
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
        private async Task<TimeSeriesDataList> GetTimeSeriesAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime baseDate, string endDateOrPeriod, string encodedUserId = default(string))
        {
            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}{1}/date/{2}/{3}.json", encodedUserId, timeSeriesResourceType.GetStringValue(), baseDate.ToFitbitFormat(), endDateOrPeriod);
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer { RootProperty = timeSeriesResourceType.ToTimeSeriesProperty() };
                    return serializer.GetTimeSeriesDataList(responseBody);
                }
            }
        }

        /// <summary>
        /// Requests the specified <see cref="TimeSeriesResourceType"/> for the date range and user specified
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public Task<TimeSeriesDataListInt> GetTimeSeriesIntAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string encodedUserId = null)
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
        public Task<TimeSeriesDataListInt> GetTimeSeriesIntAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period, string encodedUserId = null)
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
        private async Task<TimeSeriesDataListInt> GetTimeSeriesIntAsync(TimeSeriesResourceType timeSeriesResourceType, DateTime baseDate, string endDateOrPeriod, string encodedUserId)
        {
            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}{1}/date/{2}/{3}.json", encodedUserId, timeSeriesResourceType.GetStringValue(), baseDate.ToFitbitFormat(), endDateOrPeriod);
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer { RootProperty = timeSeriesResourceType.ToTimeSeriesProperty() };
                    return serializer.GetTimeSeriesDataListInt(responseBody);
                }
            }
        }

        public async Task<IntradayData> GetIntraDayTimeSeriesAsync(IntradayResourceType timeSeriesResourceType, DateTime dayAndStartTime, TimeSpan intraDayTimeSpan)
        {
            string apiCall;

            if (intraDayTimeSpan > new TimeSpan(0, 1, 0) && //the timespan is greater than a minute
                dayAndStartTime.Day == dayAndStartTime.Add(intraDayTimeSpan).Day) //adding the timespan doesn't go in to the next day
            {
                apiCall = string.Format("/1/user/-{0}/date/{1}/1d/time/{2}/{3}.json",
                    timeSeriesResourceType.GetStringValue(),
                    dayAndStartTime.ToFitbitFormat(),
                    dayAndStartTime.ToString("HH:mm"),
                    dayAndStartTime.Add(intraDayTimeSpan).ToString("HH:mm"));
            }
            else //just get the today data, there was a date specified but the timerange was likely too large or negative
            {
                apiCall = string.Format("/1/user/-{0}/date/{1}/1d/1min.json",
                    timeSeriesResourceType.GetStringValue(),
                    dayAndStartTime.ToFitbitFormat());
            }

            apiCall = FitbitClientHelperExtensions.ToFullUrl(apiCall);
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {

                HttpResponseMessage response = null;
                try
                {
                    try
                    {
                        response = await HttpClient.SendAsync(request, CancellationToken);
                    }
                    catch (FitbitRequestException fre)
                    {
                        if (fre.ApiErrors.Any(err => err.Message == Constants.FloorsUnsupportedOnDeviceError))
                        {
                            return null;
                        }
                        else
                        {
                            //otherwise, rethrow because we only want to alter behavior for the very specific case above
                            throw;
                        }
                    }
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(responseBody))
                    {
                        throw new FitbitRequestException(response, null, "The Intraday data response body was null");
                    }

                    var serializer = new JsonDotNetSerializer { RootProperty = timeSeriesResourceType.ToTimeSeriesProperty() };

                    IntradayData data = null;

                    try
                    {
                        data = serializer.GetIntradayTimeSeriesData(responseBody);
                    }
                    catch (Exception ex)
                    {
                        FitbitRequestException fEx = new FitbitRequestException(response, null, "Serialization Error in GetIntradayTimeSeriesData", ex);
                        throw fEx;
                    }

                    return data;
                }
                finally
                {
                    if (response != null)
                    {
                        response.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Get food information for date value and user specified
        /// </summary>
        /// <param name="date"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public async Task<Food> GetFoodAsync(DateTime date, string encodedUserId = default(string))
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/foods/log/date/{1}.json", encodedUserId, date.ToFitbitFormat());
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer();
                    return serializer.Deserialize<Food>(responseBody);
                }
            }
        }

        /// <summary>
        /// Get blood pressure data for date value and user specified
        /// </summary>
        /// <param name="date"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public async Task<BloodPressureData> GetBloodPressureAsync(DateTime date, string encodedUserId = default(string))
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/bp/date/{1}.json", encodedUserId, date.ToFitbitFormat());
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer();
                    return serializer.Deserialize<BloodPressureData>(responseBody);
                }
            }
        }

        /// <summary>
        /// Get the set body measurements for the date value and user specified
        /// </summary>
        /// <param name="date"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public async Task<BodyMeasurements> GetBodyMeasurementsAsync(DateTime date, string encodedUserId = default(string))
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/body/date/{1}.json", encodedUserId, date.ToFitbitFormat());
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer();
                    return serializer.Deserialize<BodyMeasurements>(responseBody);
                }
            }
        }

        /// <summary>
        /// Get Fat for a period of time starting at date.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public async Task<Fat> GetFatAsync(DateTime startDate, DateRangePeriod period)
        {
            switch (period)
            {
                case DateRangePeriod.OneDay:
                case DateRangePeriod.SevenDays:
                case DateRangePeriod.OneWeek:
                case DateRangePeriod.ThirtyDays:
                case DateRangePeriod.OneMonth:
                    break;

                default:
                    throw new ArgumentException("This API endpoint only supports range up to 31 days. See https://wiki.fitbit.com/display/API/API-Get-Body-Fat");
            }

            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/body/log/fat/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), period.GetStringValue() });
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer();
                    return seralizer.GetFat(responseBody);
                }
            }
        }

        /// <summary>
        /// Get Fat for a specific date or a specific period between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<Fat> GetFatAsync(DateTime startDate, DateTime? endDate = null)
        {
            string apiCall = string.Empty;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/body/log/fat/date/{1}.json", args: new object[] { startDate.ToFitbitFormat() });
            }
            else
            {
                if (startDate.AddDays(31) < endDate)
                {
                    throw new ArgumentOutOfRangeException("31 days is the max span. Try using period format instead for longer: https://wiki.fitbit.com/display/API/API-Get-Body-Fat");
                }

                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/body/log/fat/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat() });
            }

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer();
                    return seralizer.GetFat(responseBody);
                }
            }
        }

        /// <summary>
        /// Get Fat for a period of time starting at date.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public async Task<Weight> GetWeightAsync(DateTime startDate, DateRangePeriod period)
        {
            switch (period)
            {
                case DateRangePeriod.OneDay:
                case DateRangePeriod.SevenDays:
                case DateRangePeriod.OneWeek:
                case DateRangePeriod.ThirtyDays:
                case DateRangePeriod.OneMonth:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("This API endpoint only supports range up to 31 days. See https://wiki.fitbit.com/display/API/API-Get-Body-Weight");
            }

            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/body/log/weight/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), period.GetStringValue() });
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer();
                    return seralizer.GetWeight(responseBody);
                }
            }
        }

        /// <summary>
        /// Get Weight for a specific date or a specific period between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<Weight> GetWeightAsync(DateTime startDate, DateTime? endDate = null)
        {
            string apiCall;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/body/log/weight/date/{1}.json", args: new object[] { startDate.ToFitbitFormat() });
            }
            else
            {
                if (startDate.AddDays(31) < endDate)
                {
                    throw new ArgumentOutOfRangeException("31 days is the max span. Try using period format instead for longer: https://wiki.fitbit.com/display/API/API-Get-Body-Weight");
                }

                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/body/log/weight/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat() });
            }

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer();
                    return seralizer.GetWeight(responseBody);
                }
            }
        }

        /// <summary>
        /// Get Weight goal for the current logged in user.
        /// </summary>
        /// <returns></returns>
        public async Task<WeightGoal> GetWeightGoalsAsync()
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/body/log/weight/goal.json");
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "goal" };
                    return seralizer.Deserialize<WeightGoal>(responseBody);
                }
            }
        }

        /// <summary>
        /// Set the Weight Goal for the current logged in user.</summary>
        /// <param name="startDate"></param>
        /// <param name="startWeight"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public async Task<WeightGoal> SetWeightGoalAsync(DateTime startDate, double startWeight, double weight)
        {
            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/body/log/weight/goal.json");

            var messageContentParameters = new Dictionary<string, string>
            {
                { "startDate", startDate.ToString("yyyy-MM-dd") },
                { "startWeight", startWeight.ToString() },
                { "weight", weight.ToString() }
            };
            using (HttpRequestMessage request = GetRequest(HttpMethod.Post, apiCall))
            {
                request.Content = new FormUrlEncodedContent(messageContentParameters);

                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "goal" };
                    return seralizer.Deserialize<WeightGoal>(responseBody);
                }
            }
        }

        /// <summary>
        /// Set the Goals for the current logged in user; individual goals, multiple or all can be specified in one call.
        /// NOTE: on 2022-11-18 it was observed that a breaking change occurred. This is reflective of the new Update Activity / Goal. 
        /// </summary>
        /// <param name="goalType"></param>
        /// <param name="value"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public async Task<ActivityGoals> SetGoalsAsync(GoalType goalType, Double value, GoalPeriod period = GoalPeriod.Daily)
        {
            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/activities/goals/{1}.json?type={2}&value={3}", args: new object[] { period.GetStringValue(), goalType.GetStringValue(), value });
            using (HttpRequestMessage request = GetRequest(HttpMethod.Post, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "goals" };
                    return seralizer.Deserialize<ActivityGoals>(responseBody);
                }
            }
        }

        /// <summary>
        /// Get the Activity Goals for the current logged in user; individual goals, multiple or all can be specified in one call.</summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public async Task<ActivityGoals> GetGoalsAsync(GoalPeriod period)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/activities/goals/{1}.json", args: new object[] { period.GetStringValue() });
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "goals" };
                    return seralizer.Deserialize<ActivityGoals>(responseBody);
                }
            }
        }

        /// <summary>
        /// Get current sleep goal for the current logged in user.
        /// </summary>
        /// <returns></returns>
        public async Task<SleepGoal> GetSleepGoalAsync()
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/sleep/goal.json");
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "goal" };
                    return seralizer.Deserialize<SleepGoal>(responseBody);
                }
            }
        }

        /// <summary>
        /// Gets the water date for the specified date - https://wiki.fitbit.com/display/API/API-Get-Water
        /// </summary>
        /// <remarks>
        /// GET https://api.fitbit.com/1/user/-/foods/log/water/date/yyyy-mm-dd.json
        /// </remarks>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<WaterData> GetWaterAsync(DateTime date)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/foods/log/water/date/{1}.json", args: date.ToFitbitFormat());
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer();
                    return serializer.Deserialize<WaterData>(responseBody);
                }
            }
        }

        /// <summary>
        /// Logs the specified WaterLog item for the current logged in user - https://wiki.fitbit.com/display/API/API-Log-Water
        /// </summary>
        /// <param name="date"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<WaterLog> LogWaterAsync(DateTime date, WaterLog log)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/foods/log/water.json");

            var items = new Dictionary<string, string>();
            items.Add("amount", log.Amount.ToString());
            items.Add("date", date.ToFitbitFormat());
            apiCall = string.Format("{0}?{1}", apiCall, string.Join("&", items.Select(x => string.Format("{0}={1}", x.Key, x.Value))));

            using (HttpRequestMessage request = GetRequest(HttpMethod.Post, apiCall))
            {
                request.Content = new StringContent(string.Empty);

                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer { RootProperty = "waterLog" };
                    return serializer.Deserialize<WaterLog>(responseBody);
                }
            }
        }

        /// <summary>
        /// Deletes the specific water log entry for the logId provided for the current logged in user
        /// </summary>
        /// <remarks>
        /// DELETE https://api.fitbit.com/1/user/-/foods/log/water/XXXXX.json
        /// </remarks>
        /// <param name="logId"></param>
        /// <returns></returns>
        public async Task DeleteWaterLogAsync(long logId)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/foods/log/water/{1}.json", args: logId);
            using (HttpRequestMessage request = GetRequest(HttpMethod.Delete, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                }
            }
        }

        /// <summary>
        /// Gets a list of the current subscriptions for the current logged in user
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApiSubscription>> GetSubscriptionsAsync()
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/apiSubscriptions.json");
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer { RootProperty = "apiSubscriptions" };
                    return serializer.Deserialize<List<ApiSubscription>>(responseBody);
                }
            }
        }

        /// <summary>
        /// Add subscription
        /// </summary>
        /// <param name="apiCollectionType"></param>
        /// <param name="uniqueSubscriptionId"></param>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        public async Task<ApiSubscription> AddSubscriptionAsync(APICollectionType apiCollectionType, string uniqueSubscriptionId, string subscriberId = default(string))
        {
            string path = FormatKey(apiCollectionType, Constants.Formatting.TrailingSlash);
            string resource = FormatKey(apiCollectionType, Constants.Formatting.LeadingDash);

            string url = "/1/user/-/{1}apiSubscriptions/{3}{2}.json";
            string apiCall = FitbitClientHelperExtensions.ToFullUrl(url, args: new object[] { path, resource, uniqueSubscriptionId });
            using (HttpRequestMessage request = GetRequest(HttpMethod.Post, apiCall))
            {
                request.Content = new StringContent(string.Empty);

                if (!string.IsNullOrWhiteSpace(subscriberId))
                {
                    request.Headers.Add(Constants.Headers.XFitbitSubscriberId, subscriberId);
                }

                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var serializer = new JsonDotNetSerializer();
                    return serializer.Deserialize<ApiSubscription>(responseBody);
                }
            }
        }

        public async Task DeleteSubscriptionAsync(APICollectionType collection, string uniqueSubscriptionId, string subscriberId = null)
        {
            var collectionString = string.Empty;

            if (collection == APICollectionType.user)
                collectionString = string.Empty;
            else
                collectionString = collection.ToString() + @"/";

            string url = "/1/user/-/{2}apiSubscriptions/{1}.json";
            string apiCall = FitbitClientHelperExtensions.ToFullUrl(url, args: new object[] { uniqueSubscriptionId, collectionString });
            using (HttpRequestMessage request = GetRequest(HttpMethod.Delete, apiCall))
            {
                if (subscriberId != null)
                {
                    request.Headers.Add(Constants.Headers.XFitbitSubscriberId, subscriberId);
                }

                using (var response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    if (response.StatusCode != HttpStatusCode.NoContent)
                    {
                        var errors = new JsonDotNetSerializer().ParseErrors(await response.Content.ReadAsStringAsync());
                        throw new FitbitException("Unexpected response message", errors);
                    }
                }
            }
        }

        /// <summary>
        /// Logs the specified Activity item for the current logged in user - https://dev.fitbit.com/docs/activity/#activity-logging
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ActivityLog</returns>
        public async Task<ActivityLog> LogActivityAsync(ActivityLog model)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/activities.json");

            var items = new Dictionary<string, string>
            {
                {"activityName", Uri.EscapeDataString(model.Name)},
                {"manualCalories", model.Calories.ToString()},
                {"startTime", model.StartTime},
                {"durationMillis", model.Duration.ToString()},
                {"date", model.Date},
                {"distance", model.Distance.ToString()}
            };

            if (!string.IsNullOrEmpty(model.ActivityId.ToString()) && model.ActivityId != 0)
            {
                items.Add("activityId", model.ActivityId.ToString());
            }

            apiCall = $"{apiCall}?{string.Join("&", items.Select(x => $"{x.Key}={x.Value}"))}";
            using (HttpRequestMessage request = GetRequest(HttpMethod.Post, apiCall))
            {
                request.Content = new StringContent(string.Empty);

                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return (new JsonDotNetSerializer() { RootProperty = "activityLog" }).Deserialize<ActivityLog>(responseBody);
                }
            }
        }

        /// <summary>
        /// The Get Activity Logs List endpoint retrieves a list of a user's activity log entries before or after a given day with offset and limit using units in the unit system which corresponds to the Accept-Language header provided. - https://dev.fitbit.com/docs/activity/#get-activity-logs-list
        /// </summary>
        /// <param name="beforeDate">The date in the format yyyy-MM-ddTHH:mm:ss. Only yyyy-MM-dd is required. Either beforeDate or afterDate must be specified. Set sort to desc when using beforeDate.</param>
        /// <param name="afterDate">The date in the format yyyy-MM-ddTHH:mm:ss. Only yyyy-MM-dd is required. Either beforeDate or afterDate must be specified. Set sort to asc when using afterDate.</param>
        /// <param name="limit">The max of the number of entries returned (maximum: 20)</param>
        /// <param name="encodedUserId">encoded user id, can be null for current logged in user</param>
        /// <returns>ActivityLogsList</returns>
        public async Task<ActivityLogsList> GetActivityLogsListAsync(DateTime? beforeDate, DateTime? afterDate, int limit = 20, string encodedUserId = default(string))
        {
            //Check to make sure limit is gt 1 and less than 20
            limit = limit < 1 || limit > 20 ? 20 : limit;

            const int offset = 0;
            var sort = string.Empty;
            var dateString = string.Empty;
            var date = string.Empty;

            if (beforeDate != null && afterDate != null)
            {
                throw new ArgumentException("Please only specify a beforeDate or afterDate, not both: https://dev.fitbit.com/docs/activity/#get-activity-logs-list");
            }

            if (beforeDate != null)
            {
                dateString = "beforeDate";
                date = beforeDate.Value.ToString("yyyy-MM-dd");
                sort = "desc";
            }
            if (afterDate != null)
            {
                dateString = "afterDate";
                date = afterDate.Value.ToString("yyyy-MM-dd");
                sort = "asc";
            }

            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/activities/list.json?{1}={2}&sort={3}&limit={4}&offset={5}", encodedUserId, dateString, date, sort, limit, offset);
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    JsonSerializerSettings settings = new JsonSerializerSettings { DateParseHandling = DateParseHandling.DateTimeOffset };
                    JsonDotNetSerializer serializer = new JsonDotNetSerializer(settings);
                    ActivityLogsList result = serializer.Deserialize<ActivityLogsList>(responseBody);

                    return result;
                }
            }
        }

        /// <summary>
        /// Retrieves a list of a user's activity log entries on a given day (Max: 20 records)
        /// </summary>
        /// <param name="date">The date of Activities.</param>
        /// <param name="encodedUserId">encoded user id, can be null for current logged in user</param>
        /// <returns>ActivityLogsList</returns>
        public async Task<ActivityLogsList> GetActivityLogsListAsync(DateTime date, string encodedUserId = default(string))
        {
            ActivityLogsList logsAfterDate = await GetActivityLogsListAsync(null, date, 20, encodedUserId);

            ActivityLogsList result = new ActivityLogsList()
            {
                Activities = logsAfterDate?.Activities?.Where(x => x.StartTime.Date == date.Date).ToList()
            };

            return result;
        }

        #region HeartRateTimeSeries

        private async Task<HeartActivitiesTimeSeries> ProcessHeartRateTimeSeries(string url)
        {
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, url))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer();
                    var fitbitResponse = seralizer.GetHeartActivitiesTimeSeries(responseBody);

                    return fitbitResponse;
                }
            }
        }

        /// <summary>
        /// Requests the Heart Rate Time Series for a specific time period.
        /// </summary>
        /// <param name="date">The end date of the period specified.</param>
        /// <param name="dateRangePeriod">The range for which data will be returned.</param>
        /// <param name="userId">The encoded ID of the user.</param>
        /// <returns></returns>
        [Obsolete("Version 1.1 of the endpoint is no longer supported by Fitbit.  See https://github.com/aarondcoleman/Fitbit.NET/issues/283")]
        public async Task<HeartActivitiesTimeSeries> GetHeartRateTimeSeries(DateTime date, DateRangePeriod dateRangePeriod, string userId = "-")
        {
            string url = "1.1/user/{0}/" + "activities/heart/date/" + date.ToString("yyyy-MM-dd") + "/" + dateRangePeriod.GetStringValue() + ".json";
            string apiCall = FitbitClientHelperExtensions.ToFullUrl(url, userId);
            return await ProcessHeartRateTimeSeries(apiCall);
        }

        /// <summary>
        /// Requests the Heart Rate Time Series for a specific time period.
        /// </summary>
        /// <param name="date">The end date of the period specified.</param>
        /// <param name="dateRangePeriod">The range for which data will be returned.</param>
        /// <param name="userId">The encoded ID of the user.</param>
        /// <returns></returns>
        public async Task<HeartActivitiesTimeSeries> GetHeartRateTimeSeriesV1(DateTime date, DateRangePeriod dateRangePeriod, string userId = "-")
        {
            string url = "1/user/{0}/" + "activities/heart/date/" + date.ToString("yyyy-MM-dd") + "/" + dateRangePeriod.GetStringValue() + ".json";
            string apiCall = FitbitClientHelperExtensions.ToFullUrl(url, userId);
            return await ProcessHeartRateTimeSeries(apiCall);
        }

		/// <summary>
		/// Requests the Heart Rate Time Series for a specific time period.
		/// </summary>
		/// <param name="startDate">The start date of the period specified.</param>
		/// <param name="endDate">The range for which data will be returned.</param>
		/// <param name="userId">The encoded ID of the user.</param>
		/// <returns></returns>
		public async Task<HeartActivitiesTimeSeries> GetHeartRateTimeSeriesV1(DateTime startDate, DateTime endDate, string userId = "-")
		{
			string url = "1/user/{0}/" + "activities/heart/date/" + startDate.ToFitbitFormat() + "/" + endDate.ToFitbitFormat() + ".json";
			string apiCall = FitbitClientHelperExtensions.ToFullUrl(url, userId);
			return await ProcessHeartRateTimeSeries(apiCall);
		}

		#endregion

		#region HeartRateIntraday

		private async Task<HeartActivitiesIntraday> ProcessHeartRateIntradayTimeSeries(DateTime date, string url)
        {
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, url))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer();
                    var fitbitResponse = seralizer.GetHeartRateIntraday(date, responseBody);

                    return fitbitResponse;
                }
            }
        }

        private string GetHeartRateResolution(HeartRateResolution res)
        {
            switch (res)
            {
                case HeartRateResolution.oneSecond:
                    return "1sec";
                case HeartRateResolution.oneMinute:
                    return "1min";
                default:
                    return "1min";
            }
        }

        [Obsolete("No longer supported by Fitbit. See https://github.com/aarondcoleman/Fitbit.NET/issues/283")]
        private string GetHeartRateResolutionDeprecated(HeartRateResolution res)
        {
            switch (res)
            {
                case HeartRateResolution.oneSecond:
                    return "1sec";
                case HeartRateResolution.oneMinute:
                    return "1min";
                case HeartRateResolution.fifteenMinute:
                    return "15min";
                default:
                    return "15min";
            }
        }

        /// <summary>
        /// Requests the Intraday Heart Rate Time Series for a specific date.
        /// </summary>
        /// <param name="date">The start date and time of series.</param>
        /// <param name="resolution">Number of data points to include.</param>
        /// <param name="encodedUserId">Optional: Encoded id of the user.</param>
        /// <returns></returns>
        [Obsolete("Version 1.1 of the endpoint is no longer supported by Fitbit.  See https://github.com/aarondcoleman/Fitbit.NET/issues/283")]
        public async Task<HeartActivitiesIntraday> GetHeartRateIntraday(DateTime date, HeartRateResolution resolution, string encodedUserId = "-")
        {
            string resolutionText = GetHeartRateResolutionDeprecated(resolution);

            string apiPath = $"1.1/user/{encodedUserId}/activities/heart/date/{date:yyyy-MM-dd}/{date:yyyy-MM-dd}/{resolutionText}/time/00:00:00/23:59:59.json";
            string apiCall = FitbitClientHelperExtensions.ToFullUrl(apiPath);

            return await ProcessHeartRateIntradayTimeSeries(date, apiCall);
        }

        /// <summary>
        /// Requests the Intraday Heart Rate Time Series for a specific date.
        /// </summary>
        /// <param name="date">The start date and time of series.</param>
        /// <param name="resolution">Number of data points to include.</param>
        /// <param name="encodedUserId">Optional: Encoded id of the user.</param>
        /// <returns></returns>
        public async Task<HeartActivitiesIntraday> GetHeartRateIntradayV1(DateTime date, HeartRateResolution resolution, string encodedUserId = "-")
        {
            string resolutionText = GetHeartRateResolution(resolution);

            string apiPath = $"1/user/{encodedUserId}/activities/heart/date/{date:yyyy-MM-dd}/{date:yyyy-MM-dd}/{resolutionText}/time/00:00:00/23:59:59.json";
            string apiCall = FitbitClientHelperExtensions.ToFullUrl(apiPath);

            return await ProcessHeartRateIntradayTimeSeries(date, apiCall);
        }

        #endregion

        #region SpO2

        public async Task<List<SpO2SummaryLog>> GetSpO2SummaryAsync(DateTime startDate, DateTime? endDate = null)
        {
            string apiCall;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/spo2/date/{1}.json", args: new object[] { startDate.ToFitbitFormat() });
            }
            else
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/spo2/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat() });
            }

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer();

                    if (endDate == null)
                    {
                        SpO2SummaryLog singleSpo2Summary = seralizer.Deserialize<SpO2SummaryLog>(responseBody);
                        return new List<SpO2SummaryLog> { singleSpo2Summary };
                    }
                    else
                    {
                        return seralizer.Deserialize<List<SpO2SummaryLog>>(responseBody);
                    }
                }
            }
        }

        public async Task<List<SpO2Intraday>> GetSpO2IntradayAsync(DateTime startDate, DateTime? endDate = null)
        {
            string apiCall;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/spo2/date/{1}/all.json", args: new object[] { startDate.ToFitbitFormat() });
            }
            else
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/spo2/date/{1}/{2}/all.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat() });
            }

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer();

                    if (endDate == null)
                    {
                        SpO2Intraday singleSpo2Intraday = seralizer.Deserialize<SpO2Intraday>(responseBody);
                        return new List<SpO2Intraday> { singleSpo2Intraday };
                    }
                    else
                    {
                        return seralizer.Deserialize<List<SpO2Intraday>>(responseBody);
                    }
                }
            }
        }

        #endregion


        #region HRV

        public async Task<List<HrvSummaryLog>> GetHRVSummaryAsync(DateTime startDate, DateTime? endDate = null)
        {
            string apiCall;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/hrv/date/{1}.json", args: new object[] { startDate.ToFitbitFormat() });
            }
            else
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/hrv/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat() });
            }

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "hrv" };
                    return seralizer.Deserialize<List<HrvSummaryLog>>(responseBody);
                }
            }
        }

        public async Task<List<HrvIntraday>> GetHRVIntradayAsync(DateTime startDate, DateTime? endDate = null)
        {
            string apiCall;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/hrv/date/{1}/all.json", args: new object[] { startDate.ToFitbitFormat() });
            }
            else
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/hrv/date/{1}/{2}/all.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat() });
            }

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "hrv" };
                    return seralizer.Deserialize<List<HrvIntraday>>(responseBody);
                }
            }
        }

        #endregion

        #region BreathingRate

        #region Temperature

        public async Task<List<TemperatureCore>> GetCoreTemperatureSummaryAsync(DateTime startDate, DateTime? endDate = null)
        {
            string apiCall;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/temp/core/date/{1}.json", args: new object[] { startDate.ToFitbitFormat() });
            }
            else
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/temp/core/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat() });
            }

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "tempCore" };
                    return seralizer.Deserialize<List<TemperatureCore>>(responseBody);
                }
            }
        }

        public async Task<List<TemperatureSkin>> GetSkinTemperatureSummaryAsync(DateTime startDate, DateTime? endDate = null)
        {
            string apiCall;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/temp/skin/date/{1}.json", args: new object[] { startDate.ToFitbitFormat() });
            }
            else
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/temp/skin/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat() });
            }

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "tempSkin" };
                    return seralizer.Deserialize<List<TemperatureSkin>>(responseBody);
                }
            }
        }

        #endregion


        public async Task<List<BreathingRateSummary>> GetBreathingRateSummaryAsync(DateTime startDate, DateTime? endDate = null)
        {
            string apiCall;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/br/date/{1}.json", args: new object[] { startDate.ToFitbitFormat() });
            }
            else
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/br/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat() });
            }

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "br" };
                    return seralizer.Deserialize<List<BreathingRateSummary>>(responseBody);
                }
            }
        }

        public async Task<List<BreathingRateIntraday>> GetBreathingRateIntradayAsync(DateTime startDate, DateTime? endDate = null)
        {
            string apiCall;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/br/date/{1}/all.json", args: new object[] { startDate.ToFitbitFormat() });
            }
            else
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/br/date/{1}/{2}/all.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat() });
            }

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "br" };
                    return seralizer.Deserialize<List<BreathingRateIntraday>>(responseBody);
                }
            }
        }

        #endregion

        #region Active Zone Minutes

        public async Task<List<ActiveZoneMinutesSummary>> GetActiveZoneMinutesTimeSeriesAsync(DateTime startDate, DateTime? endDate = null, DateRangePeriod period = DateRangePeriod.OneDay)
        {
            string apiCall;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/activities/active-zone-minutes/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), period.GetStringValue() });
            }
            else
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/activities/active-zone-minutes/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat() });
            }

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "activities-active-zone-minutes" };
                    return seralizer.Deserialize<List<ActiveZoneMinutesSummary>>(responseBody);
                }
            }
        }

        public async Task<List<ActiveZoneMinutesIntraday>> GetActiveZoneMinutesIntradayAsync(DateTime startDate, DateTime? endDate = null, DataResolution resolution = DataResolution.OneMinute)
        {
            switch (resolution)
            {
                case DataResolution.OneMinute:
                case DataResolution.FiveMinute:
                case DataResolution.FifteenMinute:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("This API endpoint only supports 1min, 5min, 15min resolutions. See https://dev.fitbit.com/build/reference/web-api/intraday/get-azm-intraday-by-interval/");
            }

            string apiCall;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/activities/active-zone-minutes/date/{1}/1d/{2}.json", args: new object[] { startDate.ToFitbitFormat(), resolution.GetStringValue() });
            }
            else
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/activities/active-zone-minutes/date/{1}/{2}/{3}.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat(), resolution.GetStringValue() });
            }

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "activities-active-zone-minutes-intraday" };
                    return seralizer.Deserialize<List<ActiveZoneMinutesIntraday>>(responseBody);
                }
            }
        }

        #endregion

        #region Cardio Fitness Score
        public async Task<List<CardioScoreSummary>> GetCardioScoreSummaryAsync(DateTime startDate, DateTime? endDate = null)
        {
            string apiCall;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/cardioscore/date/{1}.json", args: new object[] { startDate.ToFitbitFormat() });
            }
            else
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/cardioscore/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat() });
            }

            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiCall))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);

                    string responseBody = await response.Content.ReadAsStringAsync();
                    var seralizer = new JsonDotNetSerializer { RootProperty = "cardioScore" };
                    return seralizer.Deserialize<List<CardioScoreSummary>>(responseBody);
                }
            }
        }
        #endregion

        private string FormatKey(APICollectionType apiCollectionType, string format)
        {
            string strValue = apiCollectionType == APICollectionType.user ? string.Empty : apiCollectionType.ToString();
            return string.IsNullOrWhiteSpace(strValue) ? strValue : string.Format(format, strValue);
        }

        /// <summary>
        /// Pass a freeform url. Good for debuging pursposes to get the pure json response back.
        /// </summary>
        /// <param name="apiPath">Fully qualified path including the Fitbit domain.</param>
        /// <returns></returns>
        public async Task<string> GetApiFreeResponseAsync(string apiPath)
        {
            using (HttpRequestMessage request = GetRequest(HttpMethod.Get, apiPath))
            {
                using (HttpResponseMessage response = await HttpClient.SendAsync(request, CancellationToken))
                {
                    await HandleResponse(response);
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        /// <summary>
        /// General error checking of the response before specific processing is done.
        /// </summary>
        /// <param name="response"></param>
        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                // assumption is error response from fitbit in the 4xx range  
                var errors = new JsonDotNetSerializer().ParseErrors(await response.Content.ReadAsStringAsync());

                // rate limit hit
                if (429 == (int)response.StatusCode)
                {
                    // not sure if we can use 'RetryConditionHeaderValue' directly as documentation is minimal for the header
                    var retryAfterHeader = response.Headers.GetValues("Retry-After").FirstOrDefault();
                    if (retryAfterHeader != null)
                    {
                        int retryAfter;
                        if (int.TryParse(retryAfterHeader, out retryAfter))
                        {
                            throw new FitbitRateLimitException(retryAfter, errors);
                        }
                    }
                }

                // request exception parsing
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                    case HttpStatusCode.NotFound:
                        throw new FitbitRequestException(response, errors);
                }

                // if we've got here then something unexpected has occured
                throw new FitbitException($"An error has occured. Please see error list for details - {(int)response.StatusCode}", errors);
            }
        }

    }
}