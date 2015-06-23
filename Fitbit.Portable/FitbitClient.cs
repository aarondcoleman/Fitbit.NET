using System;
using System.Collections.Generic;
using System.Linq;
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
        /// The specific implementation that'll authorize the request. Usually encapsulates adding header tokens. See OAuth2Authorization and OAuth1Authorization
        /// </summary>
        public IAuthorization Authorization { get; private set; }

        public FitbitClient(IAuthorization authorization, HttpClient httpClient = null)
        {
            if (authorization == null)
                throw new ArgumentNullException("authorization", "Authorization can not be null; please provide an Authorization instance.");

            Authorization = authorization;

            if (httpClient == null)
                this.HttpClient = new HttpClient();
            else
                this.HttpClient = httpClient;

            this.HttpClient = authorization.CreateAuthorizedHttpClient(); //use whatever authorization method to provide the HttpClient

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
        /// Requests the activity details of the encoded user id or if none supplied the current logged in user for the specified date
        /// </summary>
        /// <param name="activityDate"></param>
        /// <param name="encodedUserId">encoded user id, can be null for current logged in user</param>
        /// <returns>FitbitResponse of <see cref="ActivitySummary"/></returns>
        public async Task<FitbitResponse<Activity>> GetDayActivityAsync(DateTime activityDate, string encodedUserId = null)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/activities/date/{1}.json", encodedUserId, activityDate.ToFitbitFormat());

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<Activity>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer();
                fitbitResponse.Data = serializer.Deserialize<Activity>(responseBody);
            }
            return fitbitResponse;
        }

        /// <summary>
        /// Requests the activity summary of the encoded user id or if none supplied the current logged in user for the specified date
        /// </summary>
        /// <param name="activityDate"></param>
        /// <param name="encodedUserId">encoded user id, can be null for current logged in user</param>
        /// <returns>FitbitResponse of <see cref="ActivitySummary"/></returns>
        public async Task<FitbitResponse<ActivitySummary>> GetDayActivitySummaryAsync(DateTime activityDate, string encodedUserId = null)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/activities/date/{1}.json", encodedUserId, activityDate.ToFitbitFormat());

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<ActivitySummary>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer {RootProperty = "summary"};
                fitbitResponse.Data = serializer.Deserialize<ActivitySummary>(responseBody);
            }
            return fitbitResponse;
        }

        /// <summary>
        /// Requests the sleep data for the specified date for the logged in user
        /// </summary>
        /// <param name="sleepDate"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<SleepData>> GetSleepAsync(DateTime sleepDate)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/sleep/date/{1}.json", args: sleepDate.ToFitbitFormat());

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<SleepData>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer();
                fitbitResponse.Data = serializer.Deserialize<SleepData>(responseBody);

                ProcessSleepData(fitbitResponse.Data);
            }

            return fitbitResponse;
        }

        internal void ProcessSleepData(SleepData sleepData)
        {
            if ((sleepData != null) && (sleepData.Sleep != null))
            {
                foreach (var sleepLog in sleepData.Sleep)
                {
                    if (sleepLog.MinuteData == null)
                        continue;

                    int startSleepSeconds = sleepLog.StartTime.ToElapsedSeconds();

                    for (int i = 0; i < sleepLog.MinuteData.Count; i++)
                    {
                        //work with a local minute data info instance
                        var minuteData = sleepLog.MinuteData[i];

                        // determine how many seconds have elapsed since mightnight in the date
                        int currentSeconds = minuteData.DateTime.ToElapsedSeconds();

                        // if the current time is post midnight then we've clicked over to the next day
                        int daysToAdd = (currentSeconds < startSleepSeconds) ? 1 : 0;
                        DateTime derivedDate = sleepLog.StartTime.AddDays(daysToAdd);

                        // update the time value with the updated asociated date of the sleep log
                        sleepLog.MinuteData[i].DateTime = new DateTime(
                                                        derivedDate.Year,
                                                        derivedDate.Month,
                                                        derivedDate.Day,
                                                        minuteData.DateTime.Hour,
                                                        minuteData.DateTime.Minute,
                                                        minuteData.DateTime.Second);
                    }
                }
            }
        }

        /// <summary>
        /// Requests the devices for the current logged in user
        /// </summary>
        /// <returns>List of <see cref="Device"/></returns>
        public async Task<FitbitResponse<List<Device>>> GetDevicesAsync()
        {
            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/devices.json");

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
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/friends.json", encodedUserId);

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
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/profile.json", encodedUserId);

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
            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}{1}/date/{2}/{3}.json", encodedUserId, timeSeriesResourceType.GetStringValue(), baseDate.ToFitbitFormat(), endDateOrPeriod);

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
            var apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}{1}/date/{2}/{3}.json", encodedUserId, timeSeriesResourceType.GetStringValue(), baseDate.ToFitbitFormat(), endDateOrPeriod);

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

        public async Task<FitbitResponse<IntradayData>> GetIntraDayTimeSeriesAsync(IntradayResourceType timeSeriesResourceType, DateTime dayAndStartTime, TimeSpan intraDayTimeSpan)
        {

            string apiCall = null;

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
            var fitbitResponse = await HandleResponse<IntradayData>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer { RootProperty = timeSeriesResourceType.ToTimeSeriesProperty() };
                fitbitResponse.Data = serializer.GetIntradayTimeSeriesData(responseBody);
            }
            return fitbitResponse;



        }

        /// <summary>
        /// Get food information for date value and user specified
        /// </summary>
        /// <param name="date"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<Food>> GetFoodAsync(DateTime date, string encodedUserId = default(string))
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/foods/log/date/{1}.json", encodedUserId, date.ToFitbitFormat());

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<Food>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer();
                fitbitResponse.Data = serializer.Deserialize<Food>(responseBody);
            }

            return fitbitResponse;
        }

        /// <summary>
        /// Get blood pressure data for date value and user specified
        /// </summary>
        /// <param name="date"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<BloodPressureData>> GetBloodPressureAsync(DateTime date, string encodedUserId = default(string))
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/bp/date/{1}.json", encodedUserId, date.ToFitbitFormat());

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<BloodPressureData>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer();
                fitbitResponse.Data = serializer.Deserialize<BloodPressureData>(responseBody);
            }

            return fitbitResponse;
        }

        /// <summary>
        /// Get the set body measurements for the date value and user specified
        /// </summary>
        /// <param name="date"></param>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<BodyMeasurements>> GetBodyMeasurementsAsync(DateTime date, string encodedUserId = default(string))
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/body/date/{1}.json", encodedUserId, date.ToFitbitFormat());
            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<BodyMeasurements>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer();
                fitbitResponse.Data = serializer.Deserialize<BodyMeasurements>(responseBody);
            }
            return fitbitResponse;
        }

        /// <summary>
        /// Get Fat for a period of time starting at date.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<Fat>> GetFatAsync(DateTime startDate, DateRangePeriod period)
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
            var fitbitResponse = await HandleResponse<Fat>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var seralizer = new JsonDotNetSerializer();
                fitbitResponse.Data = seralizer.GetFat(responseBody);
            }

            return fitbitResponse;
        }

        /// <summary>
        /// Get Fat for a specific date or a specific period between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<Fat>> GetFatAsync(DateTime startDate, DateTime? endDate = null)
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
            var fitbitResponse = await HandleResponse<Fat>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var seralizer = new JsonDotNetSerializer();
                fitbitResponse.Data = seralizer.GetFat(responseBody);
            }

            return fitbitResponse;
        }

        /// <summary>
        /// Get Fat for a period of time starting at date.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<Weight>> GetWeightAsync(DateTime startDate, DateRangePeriod period)
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
                    throw new Exception("This API endpoint only supports range up to 31 days. See https://wiki.fitbit.com/display/API/API-Get-Body-Weight");
            }

            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/body/log/weight/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), period.GetStringValue() });

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<Weight>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var seralizer = new JsonDotNetSerializer();
                fitbitResponse.Data = seralizer.GetWeight(responseBody);
            }

            return fitbitResponse;
        }

        /// <summary>
        /// Get Weight for a specific date or a specific period between two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<Weight>> GetWeightAsync(DateTime startDate, DateTime? endDate = null)
        {
            string apiCall = string.Empty;
            if (endDate == null)
            {
                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/body/log/weight/date/{1}.json", args: new object[] { startDate.ToFitbitFormat() });
            }
            else
            {
                if (startDate.AddDays(31) < endDate)
                {
                    throw new Exception("31 days is the max span. Try using period format instead for longer: https://wiki.fitbit.com/display/API/API-Get-Body-Weight");
                }

                apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/body/log/weight/date/{1}/{2}.json", args: new object[] { startDate.ToFitbitFormat(), endDate.Value.ToFitbitFormat() });
            }

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<Weight>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var seralizer = new JsonDotNetSerializer();
                fitbitResponse.Data = seralizer.GetWeight(responseBody);
            }

            return fitbitResponse;
        }

        /// <summary>
        /// Set the Goals for the current logged in user; individual goals, multiple or all can be specified in one call.</summary>
        /// <param name="caloriesOut"></param>
        /// <param name="distance"></param>
        /// <param name="floors"></param>
        /// <param name="steps"></param>
        /// <param name="activeMinutes"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<ActivityGoals>> SetGoalsAsync(int caloriesOut = default(int), decimal distance = default(decimal), int floors = default(int), int steps = default(int), int activeMinutes = default(int))
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
            
            var fitbitResponse = await HandleResponse<ActivityGoals>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var seralizer = new JsonDotNetSerializer {RootProperty = "goals"};
                fitbitResponse.Data = seralizer.Deserialize<ActivityGoals>(responseBody);
            }

            return fitbitResponse;
        }

        /// <summary>
        /// Gets the water date for the specified date - https://wiki.fitbit.com/display/API/API-Get-Water
        /// </summary>
        /// <remarks>
        /// GET https://api.fitbit.com/1/user/-/foods/log/water/date/yyyy-mm-dd.json
        /// </remarks>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<WaterData>> GetWaterAsync(DateTime date)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/{0}/foods/log/water/date/{1}.json", args: date.ToFitbitFormat());

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<WaterData>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer();
                fitbitResponse.Data = serializer.Deserialize<WaterData>(responseBody);
            }

            return fitbitResponse;
        }

        /// <summary>
        /// Logs the specified WaterLog item for the current logged in user - https://wiki.fitbit.com/display/API/API-Log-Water
        /// </summary>
        /// <param name="date"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<WaterLog>> LogWaterAsync(DateTime date, WaterLog log)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/foods/log/water.json");

            var items = new Dictionary<string, string>();
            items.Add("amount", log.Amount.ToString());
            items.Add("date", date.ToFitbitFormat());
            apiCall = string.Format("{0}?{1}", apiCall, string.Join("&", items.Select(x => string.Format("{0}={1}", x.Key, x.Value))));

            HttpResponseMessage response = await HttpClient.PostAsync(apiCall, new StringContent(string.Empty));
            var fitbitResponse = await HandleResponse<WaterLog>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer { RootProperty = "waterLog" };
                fitbitResponse.Data = serializer.Deserialize<WaterLog>(responseBody);
            }

            return fitbitResponse;
        }

        /// <summary>
        /// Deletes the specific water log entry for the logId provided for the current logged in user
        /// </summary>
        /// <remarks>
        /// DELETE https://api.fitbit.com/1/user/-/foods/log/water/XXXXX.json
        /// </remarks>
        /// <param name="logId"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<NoData>> DeleteWaterLogAsync(long logId)
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/foods/log/water/{1}.json", args: logId);
            HttpResponseMessage response = await HttpClient.DeleteAsync(apiCall);
            var fitbitResponse = await HandleResponse<NoData>(response);
            if (fitbitResponse.Success)
            {
                fitbitResponse.Data = new NoData();
            }
            return fitbitResponse;
        }

        /// <summary>
        /// Gets a list of the current subscriptions for the current logged in user
        /// </summary>
        /// <returns></returns>
        public async Task<FitbitResponse<List<ApiSubscription>>> GetSubscriptionsAsync()
        {
            string apiCall = FitbitClientHelperExtensions.ToFullUrl("/1/user/-/apiSubscriptions.json");
            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<List<ApiSubscription>>(response);
            if (fitbitResponse.Success)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer { RootProperty = "apiSubscriptions" };
                fitbitResponse.Data = serializer.Deserialize<List<ApiSubscription>>(responseBody);
            }
            return fitbitResponse;
        }

        /// <summary>
        /// Add subscription
        /// </summary>
        /// <param name="apiCollectionType"></param>
        /// <param name="uniqueSubscriptionId"></param>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        public async Task<FitbitResponse<ApiSubscription>> AddSubscriptionAsync(APICollectionType apiCollectionType, string uniqueSubscriptionId, string subscriberId = default(string))
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
            var fitbitResponse = await HandleResponse<ApiSubscription>(response);
            if (fitbitResponse.Success)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var serializer = new JsonDotNetSerializer();
                fitbitResponse.Data = serializer.Deserialize<ApiSubscription>(responseBody);
            }
            return fitbitResponse;
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
        public async Task<string> GetAPIFreeResponse(string apiPath)
        {
            string apiCall = apiPath;

            HttpResponseMessage response = await HttpClient.GetAsync(apiCall);
            var fitbitResponse = await HandleResponse<Food>(response);
            string responseBody = null;

            if (fitbitResponse.Success)
            {
                responseBody = await response.Content.ReadAsStringAsync();
            }

            return responseBody;
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