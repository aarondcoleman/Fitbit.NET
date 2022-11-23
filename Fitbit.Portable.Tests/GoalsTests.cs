using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Fitbit.Api.Portable;
using NUnit.Framework;
using AutoFixture;
using System.Threading.Tasks;
using Fitbit.Models;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class GoalsTests
    {
        public Fixture fixture { get; set; }

        [OneTimeSetUp]
        public void Init()
        {
            fixture = new Fixture();
        }

        [Test] [Category("Portable")]
        public async Task SetGoalsAsync_CaloriesOutSet()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/activities/goals/daily.json?type=caloriesOut&value=2000");

            var response = await fitbitClient.SetGoalsAsync(GoalType.CaloriesOut, 2000);
           
            Assert.IsNotNull(response);
        }

        [Test] [Category("Portable")]
        public async Task SetGoalsAsync_DistanceSet()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/activities/goals/daily.json?type=distance&value=8.5");

            var response = await fitbitClient.SetGoalsAsync(GoalType.Distance, 8.5);

            Assert.IsNotNull(response);
        }

        [Test] [Category("Portable")]
        public async Task SetGoalsAsync_FloorsSet()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/activities/goals/daily.json?type=floors&value=20");

            var response = await fitbitClient.SetGoalsAsync(GoalType.Floors, 20);
            
            Assert.IsNotNull(response);
        }

        [Test] [Category("Portable")]
        public async Task SetGoalsAsync_StepsSet()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/activities/goals/daily.json?type=steps&value=10000");

            var response = await fitbitClient.SetGoalsAsync(GoalType.Steps, 10000);

            Assert.IsNotNull(response);
        }

        [Test] [Category("Portable")]
        public async Task SetGoalsAsync_ActiveMinuitesSet()
        {
            var fitbitClient = SetupFitbitClient("https://api.fitbit.com/1/user/-/activities/goals/daily.json?type=activeMinutes&value=50");

            var response = await fitbitClient.SetGoalsAsync(GoalType.ActiveMinutes, 50);

            Assert.IsNotNull(response);
        }

        [Test]
        [Category("Portable")]
        public async Task GetGoalsAsync()
        {
            string content = SampleDataHelper.GetContent("GetGoals.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/goals/weekly.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetGoalsAsync(GoalPeriod.Weekly);

            Assert.AreEqual(300, response.ActiveMinutes);
            Assert.AreEqual(3000, response.CaloriesOut);
            Assert.AreEqual(8.05, response.Distance);
            Assert.AreEqual(100, response.Floors);
            Assert.AreEqual(10000, response.Steps);
        }

        public FitbitClient SetupFitbitClient(string expectedURL)
        {
            string content = SampleDataHelper.GetContent("ActivityGoals.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.Created) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>(async (message, token) =>
            {
                Assert.AreEqual(HttpMethod.Post, message.Method);
                Assert.AreEqual(expectedURL, message.RequestUri.AbsoluteUri);
            });

            return Helper.CreateFitbitClient(responseMessage, verification);
        }
    }
}
