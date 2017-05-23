using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using Fitbit.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class DayActivityTests
    {
        [Test] [Category("Portable")]
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

            ValidateActivity(response);
        }

        [Test] [Category("Portable")]
        public void GetDayActivityAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.Unauthorized);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<Activity>> result = () => fitbitClient.GetDayActivityAsync(new DateTime(2014, 9, 27));

            result.ShouldThrow<FitbitRequestException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test] [Category("Portable")]
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

            ValidateActivitySummary(response);
        }

        [Test] [Category("Portable")]
        public void GetDayActivitySummaryAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.Forbidden);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            
            Func<Task<Activity>> result = () => fitbitClient.GetDayActivityAsync(new DateTime(2014, 9, 27));

            result.ShouldThrow<FitbitRequestException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test] [Category("Portable")]
        public void Can_Deserialize_Activity()
        {
            string content = SampleDataHelper.GetContent("GetActivities.json");
            var deserializer = new JsonDotNetSerializer();

            Activity activity = deserializer.Deserialize<Activity>(content);

            ValidateActivity(activity);
        }

        [Test] [Category("Portable")]
        public void Can_Deserialize_ActivitySummary()
        {
            string content = SampleDataHelper.GetContent("GetActivities.json");
            var deserializer = new JsonDotNetSerializer { RootProperty = "summary"};

            ActivitySummary summary = deserializer.Deserialize<ActivitySummary>(content);

            ValidateActivitySummary(summary);
        }

        [Test] [Category("Portable")]
        public void Can_Deserialize_ActivityGoal_Invidual()
        {
            string content = SampleDataHelper.GetContent("ActivityGoals.json");
            var deserializer = new JsonDotNetSerializer { RootProperty = "goals" };

            ActivityGoals goal = deserializer.Deserialize<ActivityGoals>(content);

            ValidateActivityGoals(goal);
        }

        [Test] [Category("Portable")]
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
            Assert.AreEqual(1457, summary.ActivityCalories);
            Assert.AreEqual(79.25, summary.Elevation);
            Assert.AreEqual(63, summary.FairlyActiveMinutes);
            Assert.AreEqual(59, summary.LightlyActiveMinutes);
            // marginal caloties?
            Assert.AreEqual(1155, summary.SedentaryMinutes);
            Assert.AreEqual(15720, summary.Steps);
            Assert.AreEqual(91, summary.VeryActiveMinutes);

            Assert.AreEqual(1623, summary.CaloriesBMR);
            Assert.AreEqual(26, summary.Floors);
            Assert.AreEqual(1067, summary.MarginalCalories);
            Assert.AreEqual(73, summary.RestingHeartRate);

            //HeartRateZones
            Assert.AreEqual(4, summary.HeartRateZones.Count);
            Assert.AreEqual("Out of Range", summary.HeartRateZones[0].Name);
            Assert.AreEqual(1198, summary.HeartRateZones[0].Minutes);
            Assert.AreEqual(30, summary.HeartRateZones[0].Min);
            Assert.AreEqual(94, summary.HeartRateZones[0].Max);
            Assert.AreEqual(1594.36823f, summary.HeartRateZones[0].CaloriesOut);

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