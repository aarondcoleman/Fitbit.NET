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
    public class FitbitClient
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

            //hack to force this done in xml - there is a bug in the api
            //which returns the content type as json
            //bug reported: https://groups.google.com/forum/?fromgroups#!topic/fitbit-api/kPrHJjK9IBs
            request.OnBeforeDeserialization = resp => { 
                resp.ContentType = "application/xml"; 
            }; 

            
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

        /// <summary>
        /// Get TimeSeries data for another user accessible with this user's credentials
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TimeSeriesDataList GetTimeSeries(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string userId)
        {

            string userSignifier = "-"; //used for current user
            if (!string.IsNullOrWhiteSpace(userId))
                userSignifier = userId;

            string requestUrl = string.Format("/1/user/{0}{1}/date/{2}/{3}.xml", userSignifier, StringEnum.GetStringValue(timeSeriesResourceType), startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
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

        /// <summary>
        /// Get TimeSeries data for another user accessible with this user's credentials
        /// </summary>
        /// <param name="timeSeriesResourceType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TimeSeriesDataListInt GetTimeSeriesInt(TimeSeriesResourceType timeSeriesResourceType, DateTime startDate, DateTime endDate, string userId)
        {

            string userSignifier = "-"; //used for current user
            if (!string.IsNullOrWhiteSpace(userId))
                userSignifier = userId;

            string requestUrl = string.Format("/1/user/{0}{1}/date/{2}/{3}.xml", userSignifier, StringEnum.GetStringValue(timeSeriesResourceType), startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
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
