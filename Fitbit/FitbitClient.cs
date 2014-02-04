using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fitbit.Models;
using RestSharp;
using RestSharp.Authenticators;
using System.Xml.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Net;

namespace Fitbit.Api
{
    /// <summary>
    /// Contains most of the calls that consume the Fitbit API.
    /// For reference on some inspired structure: 
    /// https://github.com/Fitbit/fitbit4j/blob/master/fitbit4j/src/main/java/com/fitbit/api/client/FitbitApiClientAgent.java
    /// </summary>
    public class FitbitClient : IFitbitClient
    {
        private string consumerKey;
        private string consumerSecret;
        private string accessToken;
        private string accessSecret;
        private IRestClient restClient;

        private string baseApiUrl = "https://api.fitbit.com";

        public FitbitClient(IRestClient restClient)
        {
            this.restClient = restClient;
            restClient.Authenticator = OAuth1Authenticator.ForProtectedResource(this.consumerKey, this.consumerSecret, this.accessToken, this.accessSecret);

        }

        public FitbitClient(string consumerKey, string consumerSecret)
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
        }


        /// <summary>
        /// Initialize the FitbitClient using the provided access and the default API endpoint and RestSharp RestClient
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="accessToken"></param>
        /// <param name="accessSecret"></param>
        public FitbitClient(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.accessToken = accessToken;
            this.accessSecret = accessSecret;
            this.restClient = new RestClient(baseApiUrl);

            restClient.Authenticator = OAuth1Authenticator.ForProtectedResource(this.consumerKey, this.consumerSecret, this.accessToken, this.accessSecret);

        }

        /// <summary>
        /// Initialize the FitbitClient using the provided access and specifying an IRestClient (good for testing)
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="accessToken"></param>
        /// <param name="accessSecret"></param>
        /// <param name="restClient"></param>
        public FitbitClient(string consumerKey, string consumerSecret, string accessToken, string accessSecret, IRestClient restClient)
        {
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.accessToken = accessToken;
            this.accessSecret = accessSecret;
            this.restClient = restClient;

            restClient.Authenticator = OAuth1Authenticator.ForProtectedResource(this.consumerKey, this.consumerSecret, this.accessToken, this.accessSecret);
        }

        public ActivitySummary GetDayActivitySummary(DateTime activityDate)
        {
            //RestClient client = new RestClient(baseApiUrl);
                        
            string apiCall = GetActivityApiExtentionURL(activityDate);

            RestRequest request = new RestRequest(apiCall);
            request.RootElement = "summary";

            var response = restClient.Execute<Fitbit.Models.ActivitySummary>(request);

            HandleResponseCode(response.StatusCode);

            //Console.WriteLine(response.ToString());
            //Console.WriteLine(response.Content);
            //Console.WriteLine(response.Data.steps);

            return response.Data;
        }

        public Activity GetDayActivity(DateTime activityDate)
        {
            string apiCall = GetActivityApiExtentionURL(activityDate);

            RestRequest request = new RestRequest(apiCall);

            var response = restClient.Execute<Fitbit.Models.Activity>(request);

            HandleResponseCode(response.StatusCode);

            //Console.WriteLine(response.ToString());
            //Console.WriteLine(response.Content);
            //Console.WriteLine(response.Data.steps);

            return response.Data;
        }

        public Weight GetWeight(DateTime startDate, DateTime? endDate = null)
        {
            if (startDate.AddDays(31) < endDate)
                throw new Exception("31 days is the max span. Try using period format instead for longer: https://wiki.fitbit.com/display/API/API-Get-Weight-Fat");

            string apiCall;
            if (endDate == null)
            {
                apiCall = String.Format("/1/user/-/body/log/weight/date/{0}.xml", startDate.ToString("yyyy-MM-dd"));
            }
            else
            {
                apiCall = String.Format("/1/user/-/body/log/weight/date/{0}/{1}.xml", startDate.ToString("yyyy-MM-dd"), endDate.Value.ToString("yyyy-MM-dd"));
            }

            RestRequest request = new RestRequest(apiCall);
            request.RootElement = "weight";

            var response = restClient.Execute<Fitbit.Models.Weight>(request);

            HandleResponseCode(response.StatusCode);

            return response.Data;
        }

        public Weight GetWeight(DateTime startDate, DateRangePeriod period)
        {
            if (period != DateRangePeriod.OneDay && period != DateRangePeriod.OneWeek && period != DateRangePeriod.ThirtyDays && period != DateRangePeriod.OneMonth)
                throw new Exception("This API endpoint only supports range up to 31 days. See https://wiki.fitbit.com/display/API/API-Get-Body-Weight");

            string apiCall = String.Format("/1/user/-/body/log/weight/date/{0}/{1}.xml", startDate.ToString("yyyy-MM-dd"), StringEnum.GetStringValue(period));

            RestRequest request = new RestRequest(apiCall);
            request.RootElement = "weight";

            var response = restClient.Execute<Fitbit.Models.Weight>(request);

            HandleResponseCode(response.StatusCode);

            return response.Data;
        }

        public Fat GetFat(DateTime startDate, DateTime? endDate = null)
        {
            if (startDate.AddDays(31) < endDate)
                throw new Exception("31 days is the max span. Try using period format instead for longer: https://wiki.fitbit.com/display/API/API-Get-Body-Weight");

            string apiCall;
            if (endDate == null)
            {
                apiCall = String.Format("/1/user/-/body/log/fat/date/{0}.xml", startDate.ToString("yyyy-MM-dd"));
            }
            else
            {
                apiCall = String.Format("/1/user/-/body/log/fat/date/{0}/{1}.xml", startDate.ToString("yyyy-MM-dd"), endDate.Value.ToString("yyyy-MM-dd"));
            }

            RestRequest request = new RestRequest(apiCall);
            request.RootElement = "fat";

            var response = restClient.Execute<Fitbit.Models.Fat>(request);

            HandleResponseCode(response.StatusCode);

            return response.Data;
        }

        /// <summary>
        /// Get Fat for a period of time starting at date.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public Fat GetFat(DateTime startDate, DateRangePeriod period)
        {
            if (period != DateRangePeriod.OneDay && period != DateRangePeriod.OneWeek && period != DateRangePeriod.ThirtyDays && period != DateRangePeriod.OneMonth)
                throw new Exception("This API endpoint only supports range up to 31 days. See https://wiki.fitbit.com/display/API/API-Get-Body-Fat");

            string apiCall = String.Format("/1/user/-/body/log/fat/date/{0}/{1}.xml", startDate.ToString("yyyy-MM-dd"), StringEnum.GetStringValue(period));

            RestRequest request = new RestRequest(apiCall);
            request.RootElement = "fat";

            var response = restClient.Execute<Fitbit.Models.Fat>(request);

            HandleResponseCode(response.StatusCode);

            return response.Data;

        }


        public Food GetFood(DateTime date, string userId = null)
        {
            string userSignifier = "-"; //used for current user
            if (!string.IsNullOrWhiteSpace(userId))
                userSignifier = userId;

            string apiCall = String.Format("/1/user/{0}/foods/log/date/{1}.xml", userSignifier, date.ToString("yyyy-MM-dd"));

            RestRequest request = new RestRequest(apiCall);

            var response = restClient.Execute<Fitbit.Models.Food>(request);

            HandleResponseCode(response.StatusCode);

            return response.Data;
        }

        /// <summary>
        /// Get current authenticated user's profile
        /// </summary>
        /// <returns></returns>
        public UserProfile GetUserProfile()
        {
            return GetUserProfile(null);
        }

        /// <summary>
        /// Get a specific user's profile
        /// </summary>
        /// <param name="encodedUserId"></param>
        /// <returns></returns>
        public UserProfile GetUserProfile(string encodedUserId)
        {
            string apiCall;

            if (string.IsNullOrWhiteSpace(encodedUserId))
                apiCall = "/1/user/-/profile.xml";
            else
                apiCall = string.Format("/1/user/{0}/profile.xml", encodedUserId);

            RestRequest request = new RestRequest("/1/user/-/profile.xml");
            request.RootElement = "user";

            //var response = restClient.Execute<List<Friend>>(request);
            var response = restClient.Execute<UserProfile>(request);


            HandleResponseCode(response.StatusCode);

            return response.Data;

        }


        public List<UserProfile> GetFriends()
        {
            RestRequest request = new RestRequest("/1/user/-/friends.xml");
            request.RootElement = "friends";
            


            var response = restClient.Execute<List<Friend>>(request);

            HandleResponseCode(response.StatusCode);

            List<UserProfile> userProfiles = new List<UserProfile>();

            foreach (Friend friend in response.Data)
            {
                userProfiles.Add(friend.User);
            }

            return userProfiles;

            //return response.Data;

        }

        public List<Device> GetDevices()
        {
            RestRequest request = new RestRequest("/1/user/-/devices.xml");
           
            var response = restClient.Execute<List<Device>>(request);

            HandleResponseCode(response.StatusCode);

            return response.Data;

        }

        /// <summary>
        /// Get TimeSeries data for this authenticated user
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public TimeSeriesDataList GetTimeSeries(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate)
        {
            return GetTimeSeries(timeSeriesResourceType, startDate, endDate, null);
        }

        public TimeSeriesDataList GetTimeSeries(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string userId)
        {
            return GetTimeSeries(timeSeriesResourceType, startDate, endDate.ToString("yyyy-MM-dd"), userId);
        }

        public TimeSeriesDataList GetTimeSeries(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period)
        {
            return GetTimeSeries(timeSeriesResourceType, endDate, period, null);
        }

        public TimeSeriesDataList GetTimeSeries(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period, string userId)
        {
            return GetTimeSeries(timeSeriesResourceType, endDate, StringEnum.GetStringValue(period), userId);
        }

        /// <summary>
        /// Get TimeSeries data for another user accessible with this user's credentials
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private TimeSeriesDataList GetTimeSeries(TimeSeriesResourceType timeSeriesResourceType, DateTime baseDate, string endDateOrPeriod, string userId)
        {

            string userSignifier = "-"; //used for current user
            if (!string.IsNullOrWhiteSpace(userId))
                userSignifier = userId;

            string requestUrl = string.Format("/1/user/{0}{1}/date/{2}/{3}.xml", userSignifier, StringEnum.GetStringValue(timeSeriesResourceType), baseDate.ToString("yyyy-MM-dd"), endDateOrPeriod);
            RestRequest request = new RestRequest(requestUrl);

            request.OnBeforeDeserialization = resp => {
                XDocument doc = XDocument.Parse(resp.Content);
                //IEnumerable<XElement> links = doc.Descendants("result");
                var rootElement = doc.Descendants("result").FirstOrDefault().Descendants().FirstOrDefault();

                request.RootElement = rootElement.Name.LocalName;

                //foreach (XElement link in links)
                //{
                    //RemoveDuplicateElement(link, "category"); 
                    //RemoveDuplicateElement(link, "click-commission"); 
                    //RemoveDuplicateElement(link, "creative-height"); 
                    //RemoveDuplicateElement(link, "creative-width"); 
                //}


            };

            //request.RootElement = timeSeriesResourceType.GetRootElement();

            var response = restClient.Execute<TimeSeriesDataList>(request);
            

            HandleResponseCode(response.StatusCode);
            /*
            */
            return response.Data;

        }

        /// <summary>
        /// Get TimeSeries data for this authenticated user as integer
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public TimeSeriesDataListInt GetTimeSeriesInt(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate)
        {
            return GetTimeSeriesInt(timeSeriesResourceType, startDate, endDate, null);
        }

        public TimeSeriesDataListInt GetTimeSeriesInt(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string userId)
        {
            return GetTimeSeriesInt(timeSeriesResourceType, startDate, endDate.ToString("yyyy-MM-dd"), userId);
        }

        public TimeSeriesDataListInt GetTimeSeriesInt(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period)
        {
            return GetTimeSeriesInt(timeSeriesResourceType, endDate, period, null);
        }

        public TimeSeriesDataListInt GetTimeSeriesInt(TimeSeriesResourceType timeSeriesResourceType, DateTime endDate, DateRangePeriod period, string userId)
        {
            return GetTimeSeriesInt(timeSeriesResourceType, endDate, StringEnum.GetStringValue(period), userId);
        }

        /// <summary>
        /// Get TimeSeries data for another user accessible with this user's credentials
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TimeSeriesDataListInt GetTimeSeriesInt(TimeSeriesResourceType timeSeriesResourceType, DateTime baseDate, string endDateOrPeriod, string userId)
        {

            string userSignifier = "-"; //used for current user
            if (!string.IsNullOrWhiteSpace(userId))
                userSignifier = userId;

            string requestUrl = string.Format("/1/user/{0}{1}/date/{2}/{3}.xml", userSignifier, StringEnum.GetStringValue(timeSeriesResourceType), baseDate.ToString("yyyy-MM-dd"), endDateOrPeriod);
            RestRequest request = new RestRequest(requestUrl);

            request.OnBeforeDeserialization = resp =>
            {
                XDocument doc = XDocument.Parse(resp.Content);
                //IEnumerable<XElement> links = doc.Descendants("result");
                var rootElement = doc.Descendants("result").FirstOrDefault().Descendants().FirstOrDefault();

                request.RootElement = rootElement.Name.LocalName;

                //foreach (XElement link in links)
                //{
                //RemoveDuplicateElement(link, "category"); 
                //RemoveDuplicateElement(link, "click-commission"); 
                //RemoveDuplicateElement(link, "creative-height"); 
                //RemoveDuplicateElement(link, "creative-width"); 
                //}


            };

            //request.RootElement = timeSeriesResourceType.GetRootElement();

            var response = restClient.Execute<TimeSeriesDataListInt>(request);


            HandleResponseCode(response.StatusCode);
            /*
            */
            return response.Data;

        }

        
        public IntradayData GetIntraDayTimeSeries(IntradayResourceType timeSeriesResourceType, DateTime dayAndStartTime, TimeSpan intraDayTimeSpan)
        {

            string requestUrl = null;

            if (intraDayTimeSpan > new TimeSpan(0, 1, 0) && //the timespan is greater than a minute
                dayAndStartTime.Day == dayAndStartTime.Add(intraDayTimeSpan).Day //adding the timespan doesn't go in to the next day
            )
            { 
                requestUrl = string.Format("/1/user/-{0}/date/{1}/1d/time/{2}/{3}.xml", 
                                        StringEnum.GetStringValue(timeSeriesResourceType), 
                                        dayAndStartTime.ToString("yyyy-MM-dd"), 
                                        dayAndStartTime.ToString("HH:mm"), 
                                        dayAndStartTime.Add(intraDayTimeSpan).ToString("HH:mm"));
            }
            else //just get the today data, there was a date specified but the timerange was likely too large or negative
            {
                requestUrl = string.Format("/1/user/-{0}/date/{1}/1d.xml", 
                                        StringEnum.GetStringValue(timeSeriesResourceType), 
                                        dayAndStartTime.ToString("yyyy-MM-dd"));
            }
            //                /1/user/-/activities/calories/date/2011-07-05/1d/time/12:20/12:45.xml
            RestRequest request = new RestRequest(requestUrl);

            request.OnBeforeDeserialization = resp =>
            {
                XDocument doc = XDocument.Parse(resp.Content);
                
                //find the name of the 2nd level element that contains "-intraday" and set it as the rootElement to start parsing through
                var rootElement = doc.Descendants("result").FirstOrDefault().Descendants().Where(t => t.Name.LocalName.Contains("-intraday")).FirstOrDefault();

                request.RootElement = rootElement.Name.LocalName;

                //foreach (XElement link in links)
                //{
                //RemoveDuplicateElement(link, "category"); 
                //RemoveDuplicateElement(link, "click-commission"); 
                //RemoveDuplicateElement(link, "creative-height"); 
                //RemoveDuplicateElement(link, "creative-width"); 
                //}


            };

            //request.RootElement = timeSeriesResourceType.GetRootElement();

            var response = restClient.Execute<IntradayData>(request);

            HandleResponseCode(response.StatusCode);

            //after the deserialization, need to set the date parts correctly
            for(int i=0; i < response.Data.DataSet.Count; i++)
            {
                //the serializing gets the time right, but we have to set the explicit time part from passed in
                response.Data.DataSet[i].Time = new DateTime(
                    dayAndStartTime.Year,
                    dayAndStartTime.Month,
                    dayAndStartTime.Day,
                    response.Data.DataSet[i].Time.Hour,
                    response.Data.DataSet[i].Time.Minute,
                    response.Data.DataSet[i].Time.Second);
            }


            return response.Data;

        }

        public List<ApiSubscription> GetSubscriptions()
        {
            RestRequest request = new RestRequest("/1/user/-/apiSubscriptions.xml");

            var response = restClient.Execute<List<ApiSubscription>>(request);

            HandleResponseCode(response.StatusCode);

            return response.Data;

        }

        public ApiSubscription AddSubscription(APICollectionType apiCollectionType, string uniqueSubscriptionId)
        {
            
            string subscriptionAPIEndpoint = null;
            //POST /1/user/-/apiSubscriptions/320.xml
            //POST /1/user/-/activities/apiSubscriptions/320-activities.xml
            //POST /1/user/-/foods/apiSubscriptions/320-foods.json
            //POST /1/user/-/sleep/apiSubscriptions/320-sleep.json
            //POST /1/user/-/body/apiSubscriptions/320-body.json
            if (apiCollectionType == APICollectionType.activities)
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/activities/apiSubscriptions/{0}-activities.xml", uniqueSubscriptionId);
            }
            else if (apiCollectionType == APICollectionType.body)
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/body/apiSubscriptions/{0}-body.xml", uniqueSubscriptionId);
            }
            else if (apiCollectionType == APICollectionType.foods)
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/foods/apiSubscriptions/{0}-foods.xml", uniqueSubscriptionId);
            }                
            else if (apiCollectionType == APICollectionType.meals)
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/meals/apiSubscriptions/{0}-meals.xml", uniqueSubscriptionId);
            }
            else if (apiCollectionType == APICollectionType.sleep)
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/sleep/apiSubscriptions/{0}-sleep.xml", uniqueSubscriptionId);
            }
            else if (apiCollectionType == APICollectionType.user)
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/apiSubscriptions/{0}-user.xml", uniqueSubscriptionId);
            }
            else if (apiCollectionType == APICollectionType.weight) //untested and the docs don't show it, but the Fitbit4J enum does have this
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/weight/apiSubscriptions/{0}-weight.xml", uniqueSubscriptionId);
            }

            RestRequest request = new RestRequest(subscriptionAPIEndpoint);
            request.Method = Method.POST;
            var response = restClient.Execute<ApiSubscription>(request);

            HandleResponseCode(response.StatusCode);

            return response.Data;



        }

        public ApiSubscription RemoveSubscription(APICollectionType apiCollectionType, string uniqueSubscriptionId)
        {

            string subscriptionAPIEndpoint = null;
            //POST /1/user/-/apiSubscriptions/320.xml
            //POST /1/user/-/activities/apiSubscriptions/320-activities.xml
            //POST /1/user/-/foods/apiSubscriptions/320-foods.json
            //POST /1/user/-/sleep/apiSubscriptions/320-sleep.json
            //POST /1/user/-/body/apiSubscriptions/320-body.json
            if (apiCollectionType == APICollectionType.activities)
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/activities/apiSubscriptions/{0}.xml", uniqueSubscriptionId);
            }
            else if (apiCollectionType == APICollectionType.body)
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/body/apiSubscriptions/{0}.xml", uniqueSubscriptionId);
            }
            else if (apiCollectionType == APICollectionType.foods)
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/foods/apiSubscriptions/{0}.xml", uniqueSubscriptionId);
            }
            else if (apiCollectionType == APICollectionType.meals)
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/meals/apiSubscriptions/{0}.xml", uniqueSubscriptionId);
            }
            else if (apiCollectionType == APICollectionType.sleep)
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/sleep/apiSubscriptions/{0}.xml", uniqueSubscriptionId);
            }
            else if (apiCollectionType == APICollectionType.user)
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/apiSubscriptions/{0}.xml", uniqueSubscriptionId);
            }
            else if (apiCollectionType == APICollectionType.weight) //untested and the docs don't show it, but the Fitbit4J enum does have this
            {
                subscriptionAPIEndpoint = string.Format("/1/user/-/weight/apiSubscriptions/{0}.xml", uniqueSubscriptionId);
            }

            RestRequest request = new RestRequest(subscriptionAPIEndpoint);
            request.Method = Method.DELETE;
            var response = restClient.Execute<ApiSubscription>(request);

            HandleResponseCode(response.StatusCode);

            return response.Data;
        }

        #region Derived Methods from API Calls

        /// <summary>
        /// Helps to figure out when the first device usage is. Uses the Fitbit time series for steps to find the first day of steps
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public DateTime? GetActivityTrackerFirstDay()
        {
            
            DateTime firstActivityDay = GetUserProfile().MemberSince;

            while (firstActivityDay < DateTime.UtcNow)
            {
                var stepsFromStart = GetTimeSeriesInt(TimeSeriesResourceType.Steps, firstActivityDay, firstActivityDay.AddYears(1));

                foreach (var day in stepsFromStart.DataList)
                {
                    if (day.Value > 0)
                    {
                        return day.DateTime; //this is the most likely exit point -- we find the day with the first steps in it
                    }
                }

                firstActivityDay = firstActivityDay.AddYears(1); //advance the search in to the next year. 
                //Not likely an account will be activated a full year before a device is used
            }

            return null; //this is an account where the device 
        }

        public SleepData GetSleep(DateTime sleepDate)
        {
            string apiCall = string.Format("/1/user/-/sleep/date/{0}.xml", sleepDate.ToString("yyyy-MM-dd"));
            RestRequest request = new RestRequest(apiCall);
            var response = restClient.Execute<SleepData>(request);

            HandleResponseCode(response.StatusCode);


            foreach (var sleepLog in response.Data.Sleep)
            {
                int startSleepSeconds = sleepLog.StartTime.Hour * 60 * 60;
                startSleepSeconds += sleepLog.StartTime.Minute * 60;
                startSleepSeconds += sleepLog.StartTime.Second;

                for(int i=0 ; i < sleepLog.MinuteData.Count; i++)
                {
                    var minuteData = sleepLog.MinuteData[i]; //work with a local

                    int currentSeconds = minuteData.DateTime.Hour * 60 * 60; //hours * minutes * seconds
                    currentSeconds += minuteData.DateTime.Minute * 60;
                    currentSeconds += minuteData.DateTime.Second;

                    if (currentSeconds < startSleepSeconds) //we've gone over midnight and to the next day
                    {
                        DateTime nextDay = sleepLog.StartTime.AddDays(1);

                        sleepLog.MinuteData[i].DateTime = new DateTime(
                                                    nextDay.Year,
                                                    nextDay.Month,
                                                    nextDay.Day,
                                                    minuteData.DateTime.Hour,
                                                    minuteData.DateTime.Minute,
                                                    minuteData.DateTime.Second);
                    }
                    else //still in the same day
                    {
                        DateTime currentDay = sleepLog.StartTime;

                        sleepLog.MinuteData[i].DateTime = new DateTime(
                                                    currentDay.Year,
                                                    currentDay.Month,
                                                    currentDay.Day,
                                                    minuteData.DateTime.Hour,
                                                    minuteData.DateTime.Minute,
                                                    minuteData.DateTime.Second);
                    }
                }
            }

            return response.Data;

        }

        #endregion

        #region Helpers

        /// <summary>
        /// Generic handling of status responses
        /// See: https://wiki.fitbit.com/display/API/API+Response+Format+And+Errors
        /// </summary>
        /// <param name="httpStatusCode"></param>
        private void HandleResponseCode(System.Net.HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode == System.Net.HttpStatusCode.OK ||        //200
                httpStatusCode == System.Net.HttpStatusCode.Created ||   //201
                httpStatusCode == System.Net.HttpStatusCode.NoContent)   //204
            {
                return;
            }
            else //TODO: Get more granular than this
            {
                Console.WriteLine("HttpError:" + httpStatusCode.ToString());

                throw new FitbitException("Http Error:" + httpStatusCode.ToString(), httpStatusCode);

            }
        }

        private string GetActivityApiExtentionURL(DateTime activityDate)
        {
            const string ApiExtention = "/1/user/-/activities/date/{0}-{1}-{2}.xml";
            return string.Format(ApiExtention, activityDate.Year.ToString(), activityDate.Month.ToString(), activityDate.Day.ToString());
        }

        #endregion

        
    }
}
