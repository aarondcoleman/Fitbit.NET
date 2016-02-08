using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Fitbit.Api.Portable.OAuth2;
using Fitbit.Models;
using System.Net.Http.Headers;

namespace Fitbit.Api.Portable
{
    public class FitbitClient : IFitbitClient
    {
        public FitbitAppCredentials AppCredentials { get; private set; }

        private OAuth2AccessToken _accesToken;
        public OAuth2AccessToken AccessToken {
            get
            {
                return _accesToken;
            }

            set
            {
                _accesToken = value;
                //If we update the AccessToken after HttpClient has been created, then reconfigure authorization header
                if(this.HttpClient != null)
                    ConfigureAuthorizationHeader();
            }
        }

        /// <summary>
        /// The httpclient which will be used for the api calls through the FitbitClient instance
        /// </summary>
        public HttpClient HttpClient { get; private set; }

        public ITokenManager TokenManager { get; private set; }
        public bool OAuth2TokenAutoRefresh { get; set; }
        public List<IFitbitInterceptor> FitbitInterceptorPipeline { get; private set; }

        /// <summary>
        /// Simplest constructor for OAuth2- requires the minimum information required by FitBit.Net client to make succesful calls to Fitbit Api
        /// </summary>
        /// <param name="credentials">Obtain this information from your developer dashboard. App credentials are required to perform token refresh</param>
        /// <param name="accessToken">Authenticate with Fitbit API using OAuth2. Authenticator2 class is a helper for this process</param>
        /// <param name="interceptor">An interface that enables sniffing all outgoing and incoming http requests from FitbitClient</param>
        public FitbitClient(FitbitAppCredentials credentials, OAuth2AccessToken accessToken, IFitbitInterceptor interceptor = null, ITokenManager tokenManager = null, bool enableOAuth2TokenRefresh = true)
        {
            this.AppCredentials = credentials;
            this.AccessToken = accessToken;

            this.FitbitInterceptorPipeline = new List<IFitbitInterceptor>();


            if(interceptor != null)
            {
                this.FitbitInterceptorPipeline.Add(interceptor);
            }

            ConfigureTokenManager(tokenManager);

            //Auto refresh should always be the last handle to be registered.
            ConfigureAutoRefresh(enableOAuth2TokenRefresh);

            CreateHttpClientForOAuth2();
        }

        private void ConfigureAutoRefresh(bool enableOAuth2TokenRefresh)
        {
            this.OAuth2TokenAutoRefresh = enableOAuth2TokenRefresh;
            if(OAuth2TokenAutoRefresh)
                this.FitbitInterceptorPipeline.Add(new OAuth2AutoRefreshInterceptor());
            return;
        }

        /// <summary>
        /// Simplest constructor for OAuth2- requires the minimum information required by FitBit.Net client to make succesful calls to Fitbit Api
        /// </summary>
        /// <param name="credentials">Obtain this information from your developer dashboard. App credentials are required to perform token refresh</param>
        /// <param name="accessToken">Authenticate with Fitbit API using OAuth2. Authenticator2 class is a helper for this process</param>
        /// <param name="interceptor">An interface that enables sniffing all outgoing and incoming http requests from FitbitClient</param>
        public FitbitClient(FitbitAppCredentials credentials, OAuth2AccessToken accessToken, List <IFitbitInterceptor> interceptors, bool enableOAuth2TokenRefresh = true, ITokenManager tokenManager = null)
        {
            this.AppCredentials = credentials;
            this.AccessToken = accessToken;

            this.FitbitInterceptorPipeline = new List<IFitbitInterceptor>();


            this.FitbitInterceptorPipeline.AddRange(interceptors);

            ConfigureTokenManager(tokenManager);

            //Auto refresh should always be the last handle to be registered.
            ConfigureAutoRefresh(enableOAuth2TokenRefresh);
            CreateHttpClientForOAuth2();
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
            this.HttpClient = customFactory(new FitbitHttpClientMessageHandler(this, interceptor));
        }

        private void ConfigureTokenManager(ITokenManager tokenManager)
        {
            if(tokenManager != null)
            {
                this.TokenManager = tokenManager;
            }
            else
            {
                this.TokenManager = new DefaultTokenManager();
            }
        }

        private void CreateHttpClientForOAuth2()
        {
            var pipeline = this.CreatePipeline(FitbitInterceptorPipeline);
            this.HttpClient = new HttpClient(pipeline);
            ConfigureAuthorizationHeader();
        }

        private void ConfigureAuthorizationHeader()
        {
            AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", this.AccessToken.Token);
            this.HttpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
        }

        public async Task<OAuth2AccessToken> RefreshOAuth2Token()
        {
            return await this.TokenManager.RefreshToken(this);
        }

        /// <summary>
        /// Use this constructor if an authorized httpclient has already been setup and accessing the resources is what is required.
        /// </summary>
        /// <param name="httpClient"></param>
        [Obsolete]
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
        [Obsolete]
        public FitbitClient(string consumerKey, string consumerSecret, string accessToken, string accessSecret) : this(consumerKey, consumerSecret, accessToken, accessSecret, httpClient: null)
        {
            // note: do not remove the httpclient optional parameter above, even if resharper says you should, as otherwise it will make a cyclic constructor call .... which is bad!
        }

        /// <summary>
        /// Private base constructor which takes it all and constructs or throws exceptions as appropriately
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="accessToken"></param>
        /// <param name="accessSecret"></param>
        /// <param name="httpClient"></param>
        [Obsolete]
        private FitbitClient(string consumerKey, string consumerSecret, string accessToken, string accessSecret, HttpClient httpClient = null)
        {
            HttpClient = httpClient;
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
        /// Requests the activity details of the encoded user id or if none supplied the current logged in user for the specified date
        /// </summary>
        /// <param name="activityDate"></param>
        /// <param name="encodedUserId">encoded user id, can be null for current logged in user</param>
        /// <returns>FitbitResponse of <see cref="ActivitySummary"/></returns>
        public async Task<Activity> GetDayActivityAsync(DateTime activityDate, string encodedUserId = null)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/activities/date/{1}.json", encodedUserId, activityDate.ToFitbitFormat());

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer();
            return serializer.Deserialize<Activity>(responseBody);
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
            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer {RootProperty = "summary"};
            return serializer.Deserialize<ActivitySummary>(responseBody);
        }

        /// <summary>
        /// Requests the sleep data for the specified date for the logged in user
        /// </summary>
        /// <param name="sleepDate"></param>
        /// <returns></returns>
        public async Task<SleepData> GetSleepAsync(DateTime sleepDate)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/sleep/date/{1}.json", args: sleepDate.ToFitbitFormat());

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer();
            var data = serializer.Deserialize<SleepData>(responseBody);
            FitbitClientExtensions.ProcessSleepData(data);
            return data;
        }

        /// <summary>
        /// Requests the devices for the current logged in user
        /// </summary>
        /// <returns>List of <see cref="Device"/></returns>
        public async Task<List<Device>> GetDevicesAsync()
        {
            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/devices.json");

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
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
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/friends.json", encodedUserId);

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
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
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/profile.json", encodedUserId);

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer { RootProperty = "user" };
            return serializer.Deserialize<UserProfile>(responseBody);    
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

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);

            string responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer {RootProperty = timeSeriesResourceType.ToTimeSeriesProperty()};
            return serializer.GetTimeSeriesDataList(responseBody);   

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

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer { RootProperty = timeSeriesResourceType.ToTimeSeriesProperty() };
            return serializer.GetTimeSeriesDataListInt(responseBody);
        }

        public async Task<IntradayData> GetIntraDayTimeSeriesAsync(IntradayResourceType timeSeriesResourceType, DateTime dayAndStartTime, TimeSpan intraDayTimeSpan)
        {
            string apiCall;

            if (intraDayTimeSpan > new TimeSpan(0, 1, 0) && //the timespan is greater than a minute
                dayAndStartTime.Day == dayAndStartTime.Add(intraDayTimeSpan).Day //adding the timespan doesn't go in to the next day
            )
            {
                apiCall = string.Format("/1/user/-{0}/date/{1}/1d/time/{2}/{3}.json",
                                        timeSeriesResourceType.GetStringValue(),
                                        dayAndStartTime.ToFitbitFormat(),
                                        dayAndStartTime.ToString("HH:mm"),
                                        dayAndStartTime.Add(intraDayTimeSpan).ToString("HH:mm"));
            }
            else //just get the today data, there was a date specified but the timerange was likely too large or negative
            {
                apiCall = string.Format("/1/user/-{0}/date/{1}/1d.json",
                                        timeSeriesResourceType.GetStringValue(),
                                        dayAndStartTime.ToFitbitFormat());
            }

            apiCall = FitbitClientHelperExtensions.ToFullUrl(apiCall);

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer { RootProperty = timeSeriesResourceType.ToTimeSeriesProperty() };
            return serializer.GetIntradayTimeSeriesData(responseBody);
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

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer();
            return serializer.Deserialize<Food>(responseBody);
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

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer();
            return serializer.Deserialize<BloodPressureData>(responseBody);
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
            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer();
            return serializer.Deserialize<BodyMeasurements>(responseBody);
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

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var seralizer = new JsonDotNetSerializer();
            return seralizer.GetFat(responseBody);
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

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var seralizer = new JsonDotNetSerializer();
            return seralizer.GetFat(responseBody);
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

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var seralizer = new JsonDotNetSerializer();
            return seralizer.GetWeight(responseBody);
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

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var seralizer = new JsonDotNetSerializer();
            return seralizer.GetWeight(responseBody);
        }

        /// <summary>
        /// Set the Goals for the current logged in user; individual goals, multiple or all can be specified in one call.</summary>
        /// <param name="caloriesOut"></param>
        /// <param name="distance"></param>
        /// <param name="floors"></param>
        /// <param name="steps"></param>
        /// <param name="activeMinutes"></param>
        /// <returns></returns>
        public async Task<ActivityGoals> SetGoalsAsync(int caloriesOut = default(int), decimal distance = default(decimal), int floors = default(int), int steps = default(int), int activeMinutes = default(int))
        {
            // parameter checking; at least one needs to be specified
            if ((caloriesOut == default(int))
                && (distance == default(decimal))
                && (floors == default(int))
                && (steps == default(int))
                && (activeMinutes == default(int)))
            {
                throw new ArgumentException("Unable to call SetGoalsAsync without specifying at least one goal parameter to set.");
            }

            var messageContentParameters = new Dictionary<string, string>();

            if (caloriesOut != default(int))
            {
                messageContentParameters.Add("caloriesOut", caloriesOut.ToString());
            }

            if (distance != default(decimal))
            {
                messageContentParameters.Add("distance", distance.ToString());
            }

            if (floors != default(int))
            {
                messageContentParameters.Add("floors", floors.ToString());
            }

            if (steps != default(int))
            {
                messageContentParameters.Add("steps", steps.ToString());
            }

            if (activeMinutes != default(int))
            {
                messageContentParameters.Add("activeMinutes", activeMinutes.ToString());
            }
            
            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/activities/goals/daily.json");

            //caloriesOut=100&distance=1.0&floors=1&steps=8000&activeMinutes=10
            HttpResponseMessage response = await HttpClient.PostAsync(apiCall, new FormUrlEncodedContent(messageContentParameters));
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var seralizer = new JsonDotNetSerializer {RootProperty = "goals"};
            return seralizer.Deserialize<ActivityGoals>(responseBody);
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

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer();
            return serializer.Deserialize<WaterData>(responseBody);
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

            HttpResponseMessage response = await HttpClient.PostAsync(apiCall, new StringContent(string.Empty));
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer { RootProperty = "waterLog" };
            return serializer.Deserialize<WaterLog>(responseBody);
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
            HttpResponseMessage response = await HttpClient.DeleteAsync(apiCall);
            await HandleResponse(response);
        }

        /// <summary>
        /// Gets a list of the current subscriptions for the current logged in user
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApiSubscription>> GetSubscriptionsAsync()
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/apiSubscriptions.json");
            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            string responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer { RootProperty = "apiSubscriptions" };
            return serializer.Deserialize<List<ApiSubscription>>(responseBody);
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
            string apiCall = FitbitClientHelperExtensions.ToFullUrl(url, args: new object[] {path, resource, uniqueSubscriptionId});
            if (!string.IsNullOrWhiteSpace(subscriberId))
            {
                HttpClient.DefaultRequestHeaders.Add(Constants.Headers.XFitbitSubscriberId, subscriberId);    
            }

            HttpResponseMessage response = await HttpClient.PostAsync(apiCall, new StringContent(string.Empty));
            await HandleResponse(response);
            var responseBody = await response.Content.ReadAsStringAsync();
            var serializer = new JsonDotNetSerializer();
            return serializer.Deserialize<ApiSubscription>(responseBody);
        }

        private string FormatKey(APICollectionType apiCollectionType, string format)
        {
            string strValue = apiCollectionType == APICollectionType.user ? string.Empty : apiCollectionType.ToString();
            return string.IsNullOrWhiteSpace(strValue) ? strValue : string.Format(format, strValue);
        }

        /// <summary>
        /// Pass a freeform url. Good for debuging pursposes
        /// </summary>
        /// <param name="date"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public async Task<string> GetApiFreeResponse(string apiPath)
        {
            string apiCall = apiPath;

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            await HandleResponse(response);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// General error checking of the response before specific processing is done.
        /// </summary>
        /// <param name="response"></param>
        private async Task HandleResponse(HttpResponseMessage response)
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
                            throw new FitbitRateLimitException(retryAfter) { ApiErrors =  errors };
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
                        throw new FitbitRequestException(response.StatusCode) { ApiErrors = errors };
                }

                // if we've got here then something unexpected has occured
                throw new FitbitException("An error has occured. Please see error list for details.", response.StatusCode);
            }
        }
    }
}