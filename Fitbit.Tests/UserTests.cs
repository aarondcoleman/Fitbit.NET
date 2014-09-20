using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using Fibit.Tests.Helpers;
using Fitbit.Models;
using RestSharp;
using System.Xml.Linq;
using Fitbit.Api;

namespace Fibit.Tests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void Can_Deserialize_Weight()
        {
            string content = File.ReadAllText(SampleData.PathFor("Weight.txt"));
            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            deserializer.RootElement = "weight";

            Weight result = deserializer.Deserialize<Weight>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Weights.Count == 2);

            WeightLog log = result.Weights[0];
            Assert.AreEqual(log.LogId, 1330991999000);
            Assert.AreEqual(log.Bmi, 23.566633224487305);
            Assert.AreEqual(log.Date, new DateTime(2012, 3, 5));
            Assert.AreEqual(log.Time, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59));
            Assert.AreEqual(log.Weight, 73);
            Assert.AreEqual(log.DateTime, new DateTime(2012, 3, 5, 23, 59, 59));

            log = result.Weights[1];
            Assert.AreEqual(log.LogId, 1330991999000);
            Assert.AreEqual(log.Bmi, 22.566633224487305);
            Assert.AreEqual(log.Date, new DateTime(2012, 3, 5));
            Assert.AreEqual(log.Time, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 21, 10, 59));
            Assert.AreEqual(log.Weight, 72.5);
            Assert.AreEqual(log.DateTime, new DateTime(2012, 3, 5, 21, 10, 59));
        }

        [Test]
        public void Can_Deserialize_Fat()
        {
            string content = File.ReadAllText(SampleData.PathFor("Fat.txt"));
            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            deserializer.RootElement = "fat";

            Fat result = deserializer.Deserialize<Fat>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);
            Assert.IsTrue(result.FatLogs.Count == 2);

            FatLog log = result.FatLogs[0];
            Assert.AreEqual(log.LogId, 1330991999000);
            Assert.AreEqual(log.Date, new DateTime(2012, 3, 5));
            Assert.AreEqual(log.Time, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59));
            Assert.AreEqual(log.Fat, 14);
            Assert.AreEqual(log.DateTime, new DateTime(2012, 3, 5, 23, 59, 59));

            log = result.FatLogs[1];
            Assert.AreEqual(log.LogId, 1330991999000);
            Assert.AreEqual(log.Date, new DateTime(2012, 3, 5));
            Assert.AreEqual(log.Time, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 21, 20, 59));
            Assert.AreEqual(log.Fat, 13.5);
            Assert.AreEqual(log.DateTime, new DateTime(2012, 3, 5, 21, 20, 59));
        }

        [Test]
        public void Can_Deserialize_Food()
        {
            string content = File.ReadAllText(SampleData.PathFor("Food.txt"));
            var deserializer = new RestSharp.Deserializers.XmlDeserializer();

            Food result = deserializer.Deserialize<Food>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Foods);
            Assert.IsNotNull(result.Goals);
            Assert.IsNotNull(result.Summary);

            Assert.IsTrue(result.Foods.Count == 1);
            FoodLog food = result.Foods[0];
            Assert.IsTrue(food.IsFavorite);
            Assert.AreEqual(food.LogDate, new DateTime(2011, 06, 29));
            Assert.AreEqual(food.LogId, 1924);

            Assert.IsNotNull(food.LoggedFood);
            LoggedFood logged = food.LoggedFood;
            Assert.AreEqual(logged.AccessLevel, "PUBLIC");
            Assert.AreEqual(logged.Amount, 132.57f);
            Assert.IsNull(logged.Brand);
            Assert.AreEqual(logged.Calories, 752);
            Assert.AreEqual(logged.FoodId, 18828);
            Assert.AreEqual(logged.MealTypeId, 4);
            Assert.AreEqual(logged.Locale, "en_US");
            Assert.AreEqual(logged.Name, "Chocolate, Milk");

            Assert.IsNotNull(food.NutritionalValues);
            NutritionalValues values = food.NutritionalValues;
            Assert.AreEqual(values.Calories, 752);
            Assert.AreEqual(values.Carbs, 66.5);
            Assert.AreEqual(values.Fat, 49);
            Assert.AreEqual(values.Fiber, .5);
            Assert.AreEqual(values.Protein, 12.5);
            Assert.AreEqual(values.Sodium, 186);

            FoodSummary summary = result.Summary;
            Assert.AreEqual(summary.Calories, 752);
            Assert.AreEqual(summary.Carbs, 66.5);
            Assert.AreEqual(summary.Fat, 49);
            Assert.AreEqual(summary.Fiber, .5);
            Assert.AreEqual(summary.Protein, 12.5);
            Assert.AreEqual(summary.Sodium, 186);
            Assert.AreEqual(summary.Water, 0);

            FoodGoals goals = result.Goals;
            Assert.AreEqual(goals.Calories, 2286);
        }

        [Test]
        public void Can_Deserialize_Profile()
        {
            string content = File.ReadAllText(SampleData.PathFor("UserProfile.txt"));
            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            deserializer.RootElement = "user";

            UserProfile result = deserializer.Deserialize<UserProfile>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);
            Assert.AreEqual("I live in San Francisco.", result.AboutMe);
            Assert.AreEqual("http://www.fitbit.com/images/profile/defaultProfile_100_male.gif", result.Avatar);
            Assert.AreEqual("San Francisco", result.City);
            Assert.AreEqual("US", result.Country);
            Assert.AreEqual(new DateTime(1971, 2, 18), result.DateOfBirth);
            Assert.AreEqual("Nick", result.Nickname);
            Assert.AreEqual("en_US", result.DistanceUnit);
            Assert.AreEqual("2295YW", result.EncodedId);
            Assert.AreEqual("Fitbit User", result.FullName);
            Assert.AreEqual(Gender.MALE, result.Gender);
            Assert.AreEqual("METRIC", result.GlucoseUnit);
            Assert.AreEqual((double)176.75, result.Height);
            Assert.AreEqual("en_US", result.HeightUnit);
            Assert.AreEqual("en_GB", result.Locale);
            Assert.AreEqual(new DateTime(2010, 2, 7), result.MemberSince);
            Assert.AreEqual("Nick", result.Nickname);
            Assert.AreEqual(-25200000, result.OffsetFromUTCMillis);
            Assert.AreEqual("CA", result.State);
            Assert.AreEqual(0, result.StrideLengthRunning);
            Assert.AreEqual(0, result.StrideLengthWalking);
            Assert.AreEqual("America/Los_Angeles", result.Timezone);
            Assert.AreEqual("METRIC", result.VolumeUnit);
            Assert.AreEqual((double)80.55, result.Weight);
            Assert.AreEqual("METRIC", result.WeightUnit);

        }

        [Test]
        public void Can_Deserialize_Friends()
        {
            string content = File.ReadAllText(SampleData.PathFor("GetFriends.txt"));
            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            deserializer.RootElement = "friends";

            List<Friend> result = deserializer.Deserialize<List<Friend>>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);

            List<UserProfile> userProfiles = new List<UserProfile>();

            foreach (Friend friend in result)
            {
                userProfiles.Add(friend.User);
            }

            Assert.IsTrue(userProfiles.Count == 4);
            Assert.AreEqual("User A.", userProfiles[0].DisplayName);

            //TODO: More tests of the data objects coming in


        }

        [Test]
        public void Can_Deserialize_Devices_Single()
        {
            string content = File.ReadAllText(SampleData.PathFor("GetDevices-Single.txt"));
            var deserializer = new RestSharp.Deserializers.XmlDeserializer();

            List<Device> result = deserializer.Deserialize<List<Device>>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);


            Assert.IsTrue(result.Count == 1);

            /*
            Assert.AreEqual("Full", result[0].Battery);
            Assert.AreEqual("123456", result[0].Id);
            //TODO: compare the date

            Assert.AreEqual("TRACKER", result[0].Type);
            Assert.AreEqual("Ultra", result[0].DeviceVersion);
            */

            Assert.AreEqual("Medium", result[0].Battery);
            Assert.AreEqual("473384", result[0].Id);
            //TODO: compare the date

            Assert.AreEqual(DeviceType.Tracker, result[0].Type);
            Assert.AreEqual("Ultra", result[0].DeviceVersion);


        }

        [Test]
        public void Can_Deserialize_Devices_Double()
        {
            string content = File.ReadAllText(SampleData.PathFor("GetDevices-Double.txt"));
            var deserializer = new RestSharp.Deserializers.XmlDeserializer();

            List<Device> result = deserializer.Deserialize<List<Device>>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);


            Assert.IsTrue(result.Count == 2);
            Assert.AreEqual("Full", result[0].Battery);
            Assert.AreEqual("123456", result[0].Id);
            //test date
            //Assert.AreEqual("TRACKER", result[0].Type);
            Assert.AreEqual(DeviceType.Tracker, result[0].Type);

            Assert.AreEqual("Ultra", result[0].DeviceVersion);

            Assert.AreEqual("High", result[1].Battery);
            //test date
            Assert.AreEqual("987654", result[1].Id);
            Assert.AreEqual(DeviceType.Scale, result[1].Type);
            Assert.AreEqual("Aria", result[1].DeviceVersion);

            //TODO: More tests of the data objects coming in


        }


        [Test]
        public void Can_Deserialize_SubscriptionResponse()
        {
            string content = File.ReadAllText(SampleData.PathFor("SubscriptionResponse.txt"));
            var deserializer = new RestSharp.Deserializers.XmlDeserializer();

            SubscriptionResponse result = deserializer.Deserialize<SubscriptionResponse>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);

            Assert.AreEqual("user", result.CollectionType);
            Assert.AreEqual("227YZL", result.OwnerId);
            Assert.AreEqual("user", result.OwnerType);
            Assert.AreEqual("1", result.SubscriberId);
            Assert.AreEqual("323", result.SubscriptionId);


        }

        [Test]
        public void Can_Deserialize_TimeSeriesActivitiesSteps()
        {
            string content = File.ReadAllText(SampleData.PathFor("TimeSeriesActivitiesSteps.txt"));

            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            //var deserializer = new RestSharp.Deserializers.JsonDeserializer();

            //TimeSeriesResourceType type = TimeSeriesResourceType.Steps.GetRootElement();

            deserializer.RootElement = "activities-steps";


            List<TimeSeriesDataList.Data> result = deserializer.Deserialize<List<TimeSeriesDataList.Data>>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);

            Assert.AreEqual(new DateTime(2012, 5, 19), result[0].DateTime);
            Assert.AreEqual("11546", result[0].Value);

            Assert.AreEqual(8, result.Count);

        }

        [Test]
        public void Can_Deserialize_TimeSeriesActivitiesStepsInt()
        {
            string content = File.ReadAllText(SampleData.PathFor("TimeSeriesActivitiesSteps.txt"));

            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            //var deserializer = new RestSharp.Deserializers.JsonDeserializer();

            //TimeSeriesResourceType type = TimeSeriesResourceType.Steps.GetRootElement();

            deserializer.RootElement = "activities-steps";


            List<TimeSeriesDataListInt.Data> result = deserializer.Deserialize<List<TimeSeriesDataListInt.Data>>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);

            Assert.AreEqual(new DateTime(2012, 5, 19), result[0].DateTime);
            Assert.AreEqual(11546, result[0].Value);

            Assert.AreEqual(8, result.Count);

        }

        //removed from Fitbit API 2013-10-28
        /*
        [Test]
        public void Can_Deserialize_IntradayActivitiesSteps()
        {
            string content = File.ReadAllText(SampleData.PathFor("IntradayActivitiesSteps.txt"));

            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            //var deserializer = new RestSharp.Deserializers.JsonDeserializer();
            deserializer.DateFormat = "HH:mm:ss";
            //TimeSeriesResourceType type = TimeSeriesResourceType.Steps.GetRootElement();

            //deserializer.RootElement = "dataset";
            deserializer.RootElement = "activities-steps-intraday";

            IntradayData result = deserializer.Deserialize<IntradayData>(new RestResponse() { Content = content });
            //var result = deserializer.Deserialize<dynamic>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);

            Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), result.DataSet[0].Time);
            Assert.AreEqual("3", result.DataSet[0].Value);

            Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,0,1,0), result.DataSet[1].Time);
            Assert.AreEqual("10", result.DataSet[1].Value);


            Assert.AreEqual(1440, result.DataSet.Count);

            //Assert.AreEqual(8, result.Count);

        }

         */

        [Test]
        public void Can_Deserialize_IntradayActivitiesCalories()
        {
            string content = File.ReadAllText(SampleData.PathFor("IntradayActivitiesCalories.txt"));

            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            //var deserializer = new RestSharp.Deserializers.JsonDeserializer();
            deserializer.DateFormat = "HH:mm:ss";
            //TimeSeriesResourceType type = TimeSeriesResourceType.Steps.GetRootElement();

            //deserializer.RootElement = "dataset";
            //deserializer.RootElement = "activities-log-calories-intraday";
            XDocument doc = XDocument.Parse(content);

            var rootElement = doc.Descendants("result").FirstOrDefault().Descendants().Where(t => t.Name.LocalName.Contains("-intraday")).FirstOrDefault();

            //sometimes the API doesn't return that node, for isnstance a date queried before the start of an account 
            if (rootElement != null && !string.IsNullOrWhiteSpace(rootElement.Name.LocalName))
                deserializer.RootElement = rootElement.Name.LocalName;

            IntradayData result = deserializer.Deserialize<IntradayData>(new RestResponse() { Content = content });
            //var result = deserializer.Deserialize<dynamic>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);

            Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), result.DataSet[0].Time);
            Assert.AreEqual("1.159999966621399", result.DataSet[0].Value);
            Assert.AreEqual("0", result.DataSet[0].Level);
            Assert.AreEqual("10", result.DataSet[0].METs);


            Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 1, 0), result.DataSet[1].Time);
            Assert.AreEqual("1.159999966621399", result.DataSet[1].Value);
            Assert.AreEqual("2", result.DataSet[1].Level);
            Assert.AreEqual("32", result.DataSet[1].METs);

            //Assert.AreEqual(1440, result.DataSet.Count);

            //Assert.AreEqual(8, result.Count);

        }

        [Test]
        public void Can_Deserialize_IntradayActivitiesSteps()
        {
            string content = File.ReadAllText(SampleData.PathFor("IntradayActivitiesSteps.txt"));

            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            //var deserializer = new RestSharp.Deserializers.JsonDeserializer();
            deserializer.DateFormat = "HH:mm:ss";
            //TimeSeriesResourceType type = TimeSeriesResourceType.Steps.GetRootElement();

            //deserializer.RootElement = "dataset";
            //deserializer.RootElement = "activities-log-calories-intraday";
            XDocument doc = XDocument.Parse(content);

            var rootElement = doc.Descendants("result").FirstOrDefault().Descendants().Where(t => t.Name.LocalName.Contains("-intraday")).FirstOrDefault();

            //sometimes the API doesn't return that node, for isnstance a date queried before the start of an account 
            if (rootElement != null && !string.IsNullOrWhiteSpace(rootElement.Name.LocalName))
                deserializer.RootElement = rootElement.Name.LocalName;

            IntradayData result = deserializer.Deserialize<IntradayData>(new RestResponse() { Content = content });
            //var result = deserializer.Deserialize<dynamic>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);

            Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), result.DataSet[0].Time);
            Assert.AreEqual("3", result.DataSet[0].Value);

            Assert.AreEqual(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 1, 0), result.DataSet[1].Time);
            Assert.AreEqual("10", result.DataSet[1].Value);

            //Assert.AreEqual(1440, result.DataSet.Count);

            //Assert.AreEqual(8, result.Count);

        }

        /// <summary>
        /// This is the response that we'd get back from Fitbit when a resource or multiple resources have been updated
        /// </summary>
        [Test]
        public void Can_Deserialize_Subscription_Notification()
        {
            string content = File.ReadAllText(SampleData.PathFor("UpdatedResources.txt"));

            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            //var deserializer = new RestSharp.Deserializers.JsonDeserializer();
            //deserializer.DateFormat = "HH:mm:ss";
            //TimeSeriesResourceType type = TimeSeriesResourceType.Steps.GetRootElement();

            //deserializer.RootElement = "dataset";
            //deserializer.RootElement = "updates";

            List<UpdatedResource> result = deserializer.Deserialize<List<UpdatedResource>>(new RestResponse() { Content = content });
            //var result = deserializer.Deserialize<dynamic>(new RestResponse() { Content = content });

            Assert.AreEqual(2, result.Count);

        }

        [Test]
        public void Can_Process_Incoming_Updated_Resource_Stream()
        {
            string content = File.ReadAllText(SampleData.PathFor("SubscriptionUpdateSigned.txt"));

            SubscriptionManager subScriptionManager = new SubscriptionManager();
            List<UpdatedResource> updatedResources = subScriptionManager.ProcessUpdateReponseBody(content);

            Assert.AreEqual(1, updatedResources.Count);
            Assert.AreEqual(new DateTime(2012, 7, 25), updatedResources[0].Date);
            Assert.AreEqual("2242TQ", updatedResources[0].OwnerId);
            Assert.AreEqual(APICollectionType.activities, updatedResources[0].CollectionType);
            Assert.AreEqual(ResourceOwnerType.User, updatedResources[0].OwnerType);
            Assert.AreEqual("b4b6dc1a5ead4b4e84f7b7f5b2f16b21-activities", updatedResources[0].SubscriptionId);

        }

        [Test]
        public void Can_Process_Incoming_Updated_Multiple_Resource_Stream()
        {
            string content = File.ReadAllText(SampleData.PathFor("SubscriptionUpdateSignedMultiple.txt"));

            SubscriptionManager subScriptionManager = new SubscriptionManager();
            List<UpdatedResource> updatedResources = subScriptionManager.ProcessUpdateReponseBody(content);

            Assert.AreEqual(2, updatedResources.Count);

            Assert.AreEqual(new DateTime(2012, 7, 25), updatedResources[0].Date);
            Assert.AreEqual("2242TQ", updatedResources[0].OwnerId);
            Assert.AreEqual(APICollectionType.activities, updatedResources[0].CollectionType);
            Assert.AreEqual(ResourceOwnerType.User, updatedResources[0].OwnerType);
            Assert.AreEqual("b4b6dc1a5ead4b4e84f7b7f5b2f16b21-activities", updatedResources[0].SubscriptionId);

            Assert.AreEqual(new DateTime(2012, 7, 24), updatedResources[1].Date);
            Assert.AreEqual("228S74", updatedResources[1].OwnerId);
            Assert.AreEqual(APICollectionType.foods, updatedResources[1].CollectionType);
            Assert.AreEqual(ResourceOwnerType.User, updatedResources[1].OwnerType);
            Assert.AreEqual("1234", updatedResources[1].SubscriptionId);

        }

        [Test]
        public void Can_Deserialize_Api_Subscription_List()
        {
            string content = File.ReadAllText(SampleData.PathFor("ListApiSubscriptions.txt"));

            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            //var deserializer = new RestSharp.Deserializers.JsonDeserializer();
            //deserializer.DateFormat = "HH:mm:ss";
            //TimeSeriesResourceType type = TimeSeriesResourceType.Steps.GetRootElement();

            //deserializer.RootElement = "dataset";
            //deserializer.RootElement = "updates";

            List<ApiSubscription> result = deserializer.Deserialize<List<ApiSubscription>>(new RestResponse() { Content = content });
            //var result = deserializer.Deserialize<dynamic>(new RestResponse() { Content = content });

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(APICollectionType.user, result[0].CollectionType);
            Assert.AreEqual("227YZL", result[0].OwnerId);
            Assert.AreEqual("1", result[0].SubscriberId);
            Assert.AreEqual("323", result[0].SubscriptionId);
        }

        [Test]
        public void Can_Deserialize_Sleep_SingleSleepLog()
        {
            string content = File.ReadAllText(SampleData.PathFor("SleepData-SingleSleepLog.txt"));

            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            //var deserializer = new RestSharp.Deserializers.JsonDeserializer();
            //deserializer.DateFormat = "HH:mm:ss";
            //TimeSeriesResourceType type = TimeSeriesResourceType.Steps.GetRootElement();

            //deserializer.RootElement = "sleep";
            //deserializer.RootElement = "updates";

            SleepData result = deserializer.Deserialize<SleepData>(new RestResponse() { Content = content });
            //var result = deserializer.Deserialize<dynamic>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);
            //Assert.IsTrue(result.Sleep.SleepLog.Count > 0);

            Assert.IsNotNull(result.Summary);

            Assert.AreEqual(497, result.Summary.TotalMinutesAsleep);
            Assert.AreEqual(1, result.Summary.TotalSleepRecords);
            Assert.AreEqual(578, result.Summary.TotalTimeInBed);

            Assert.AreEqual(1, result.Sleep.Count);
            Assert.AreEqual(578, result.Sleep.First().MinuteData.Count); //this particular sleep had 578 minutes

            Assert.AreEqual(2, result.Sleep.First().MinuteData[1].Value);
            /*
            Assert.AreEqual(APICollectionType.user, result[0].CollectionType);
            Assert.AreEqual("227YZL", result[0].OwnerId);
            Assert.AreEqual("1", result[0].SubscriberId);
            Assert.AreEqual("323", result[0].SubscriptionId);
             */
        }

        [Test]
        public void Can_Deserialize_HeartRateResponse()
        {
            string content = File.ReadAllText(SampleData.PathFor("HeartLogResponse.txt"));

            var deserializer = new RestSharp.Deserializers.XmlDeserializer();
            deserializer.RootElement = "heartLog";

            HeartRateLog result = deserializer.Deserialize<HeartRateLog>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);

            Assert.AreEqual(150, result.HeartRate);
            Assert.AreEqual(1424, result.LogId);

            var now = DateTime.Now;
            DateTime expected = new DateTime(now.Year, now.Month, now.Day, 12, 20, 0);
            Assert.AreEqual(expected, result.Time);

            Assert.AreEqual("Running", result.Tracker);
        }

        public void Can_Deserialize_Rate_Limit()
        {
            string content = File.ReadAllText(SampleData.PathFor("RateLimitError.txt"));

            var deserializer = new RestSharp.Deserializers.XmlDeserializer();

            List<ApiError> result = deserializer.Deserialize<List<ApiError>>(new RestResponse() { Content = content });

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            ApiError error = result.First();
            Assert.AreEqual(ErrorType.Request, error.ErrorType);
            Assert.AreEqual("n/a", error.FieldName);
        }
    }
}