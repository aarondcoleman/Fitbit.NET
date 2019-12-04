using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Fitbit.Api.Portable;
using NUnit.Framework;
using AutoFixture;
using System.Threading.Tasks;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class WeightGoalTests
    {
        public Fixture fixture { get; set; }

        [OneTimeSetUp]
        public void Init()
        {
            fixture = new Fixture();
        }

        [Test]
        [Category("Portable")]
        public async Task SetWeightGoalAsync()
        {
            var date = new DateTime(2019, 1, 1);
            var fitbitClient = SetupFitbitClient($"startDate={date.ToString("yyyy-MM-dd")}&startWeight=500&weight=200");

            var response = await fitbitClient.SetWeightGoalAsync(date, 500, 200);
           
            Assert.IsNotNull(response);
        }

        [Test]
        [Category("Portable")]
        public async Task GetWeightGoalAsync()
        {
            string content = SampleDataHelper.GetContent("GetWeightGoal.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/body/log/weight/goal.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetWeightGoalsAsync();

            Assert.AreEqual("2019-01-01", response.StartDate.ToString("yyyy-MM-dd"));
            Assert.AreEqual(500, response.StartWeight);
            Assert.AreEqual(100, response.Weight);
            Assert.AreEqual("LOSE", response.GoalType);
            Assert.AreEqual(0.11, response.WeightThreshold);
        }


        public FitbitClient SetupFitbitClient(string expectedBody)
        {
            string content = SampleDataHelper.GetContent("WeightGoals.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.Created) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>(async (message, token) =>
            {
                Assert.AreEqual(HttpMethod.Post, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/body/log/weight/goal.json", message.RequestUri.AbsoluteUri);

                var body = await message.Content.ReadAsStringAsync();
                Assert.AreEqual(true, body.Equals(expectedBody));
            });

            return Helper.CreateFitbitClient(responseMessage, verification);
        }
    }
}
