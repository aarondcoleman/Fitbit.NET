using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class DayActivityTests
    {
        [Test]
        public async void GetDayActivityAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetActivities.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/date/2014-09-27.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetDayActivityAsync(new DateTime(2014, 9, 27));

            Assert.IsTrue(response.Success);
            ValidateActivity(response.Data);
        }

        [Test]
        public async void GetDayActivityAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse();
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetDayActivityAsync(new DateTime(2014, 9, 27));

            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual(1, response.Errors.Count);
        }

        [Test]
        public async void GetDayActivitySummaryAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetActivities.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/date/2014-09-27.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetDayActivitySummaryAsync(new DateTime(2014, 9, 27));

            Assert.IsTrue(response.Success);
            ValidateActivitySummary(response.Data);
        }

        [Test]
        public async void GetDayActivitySummaryAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse();
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetDayActivitySummaryAsync(new DateTime(2014, 9, 27));

            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual(1, response.Errors.Count);
        } 

        [Test]
        public void Can_Deserialize_Activity()
        {
            string content = SampleDataHelper.GetContent("GetActivities.json");
            var deserializer = new JsonDotNetSerializer();

            Activity activity = deserializer.Deserialize<Activity>(content);

            ValidateActivity(activity);
        }

        [Test]
        public void Can_Deserialize_ActivitySummary()
        {
            string content = SampleDataHelper.GetContent("GetActivities.json");
            var deserializer = new JsonDotNetSerializer { RootProperty = "summary"};

            ActivitySummary summary = deserializer.Deserialize<ActivitySummary>(content);

            ValidateActivitySummary(summary);
        }

        [Test]
        public void Can_Deserialize_ActivityGoal_Invidual()
        {
            string content = SampleDataHelper.GetContent("ActivityGoals.json");
            var deserializer = new JsonDotNetSerializer { RootProperty = "goals" };

            ActivityGoals goal = deserializer.Deserialize<ActivityGoals>(content);

            ValidateActivityGoals(goal);
        }

        [Test]
        public void Can_Deserialize_ActivityGoal_FromActivities()
        {
            string content = SampleDataHelper.GetContent("GetActivities.json");
            var deserializer = new JsonDotNetSerializer { RootProperty = "goals" };

            ActivityGoals goal = deserializer.Deserialize<ActivityGoals>(content);

            ValidateActivityGoals(goal);
        }

        private void ValidateActivity(Activity activity)
        {
            // high level
            Assert.IsNotNull(activity);
            Assert.IsNotNull(activity.Summary);
            Assert.IsNotNull(activity.Activities);
            Assert.IsNotNull(activity.Goals);

            // summary
            ValidateActivitySummary(activity.Summary);

            // activities
            Assert.AreEqual(1, activity.Activities.Count);
            var a = activity.Activities.First();
            Assert.AreEqual(2382124, a.ActivityId);
            Assert.AreEqual(2382124, a.ActivityParentId);
            // activity parent name
            Assert.AreEqual(541, a.Calories);
            Assert.AreEqual("", a.Description);
            // distance?
            Assert.AreEqual(2400000, a.Duration);
            Assert.AreEqual(true, a.HasStartTime);
            Assert.AreEqual(false, a.IsFavorite);
            // last modified
            Assert.AreEqual(105415286, a.LogId);
            Assert.AreEqual("Running (jogging), 6.7 mph (9 min mile) (MyFitnessPal)", a.Name);
            // startdate
            Assert.AreEqual("20:16", a.StartTime); // todo: change this to date time as per FatLog and WeightLog
            // steps?

            // goals
            var g = activity.Goals;
            ValidateActivityGoals(g);
        }

        private static void ValidateActivityGoals(ActivityGoals g)
        {
            Assert.AreEqual(10000, g.Steps);
            Assert.AreEqual(8.05, g.Distance);
            Assert.AreEqual(2820, g.CaloriesOut);
        }

        private void ValidateActivitySummary(ActivitySummary summary)
        {
            // activity calories?
            // calories BMR?
            Assert.AreEqual(2828, summary.CaloriesOut);
            Assert.AreEqual(63, summary.FairlyActiveMinutes);
            Assert.AreEqual(59, summary.LightlyActiveMinutes);
            // marginal caloties?
            Assert.AreEqual(1155, summary.SedentaryMinutes);
            Assert.AreEqual(15720, summary.Steps);
            Assert.AreEqual(91, summary.VeryActiveMinutes);

            // distances
            var d = summary.Distances.First(x => x.Activity == "total");
            Assert.IsNotNull(d);
            Assert.AreEqual(12.32f, d.Distance);

            d = summary.Distances.First(x => x.Activity == "tracker");
            Assert.IsNotNull(d);
            Assert.AreEqual(12.32f, d.Distance);

            d = summary.Distances.First(x => x.Activity == "loggedActivities");
            Assert.IsNotNull(d);
            Assert.AreEqual(0, d.Distance);

            d = summary.Distances.First(x => x.Activity == "veryActive");
            Assert.IsNotNull(d);
            Assert.AreEqual(9.69f, d.Distance);

            d = summary.Distances.First(x => x.Activity == "moderatelyActive");
            Assert.IsNotNull(d);
            Assert.AreEqual(1.99f, d.Distance);

            d = summary.Distances.First(x => x.Activity == "lightlyActive");
            Assert.IsNotNull(d);
            Assert.AreEqual(0.57f, d.Distance);

            d = summary.Distances.First(x => x.Activity == "sedentaryActive");
            Assert.IsNotNull(d);
            Assert.AreEqual(0, d.Distance);

            d = summary.Distances.First(x => x.Activity == "Running (jogging), 6.7 mph (9 min mile) (MyFitnessPal)");
            Assert.IsNotNull(d);
            Assert.AreEqual(0, d.Distance);
        }
    }
}