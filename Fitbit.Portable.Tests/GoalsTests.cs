using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Fitbit.Api.Portable;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class GoalsTests
    {
        public Fixture fixture { get; set; }

        [TestFixtureSetUp]
        public void Init()
        {
            fixture = new Fixture();
        }

        [Test] [Category("Portable")]
        [ExpectedException(typeof(ArgumentException))]
        public async void SetGoalsAsync_NoGoalsSet()
        {
            var client = fixture.Create<FitbitClient>();
            await client.SetGoalsAsync();
        }

        [Test] [Category("Portable")]
        public async void SetGoalsAsync_CaloriesOutSet()
        {
            var fitbitClient = SetupFitbitClient("caloriesOut=2000");

            var response = await fitbitClient.SetGoalsAsync(caloriesOut: 2000);
           
            Assert.IsNotNull(response);
        }

        [Test] [Category("Portable")]
        public async void SetGoalsAsync_DistanceSet()
        {
            var fitbitClient = SetupFitbitClient("distance=8.5");

            var response = await fitbitClient.SetGoalsAsync(distance: 8.5M);

            Assert.IsNotNull(response);
        }

        [Test] [Category("Portable")]
        public async void SetGoalsAsync_FloorsSet()
        {
            var fitbitClient = SetupFitbitClient("floors=20");

            var response = await fitbitClient.SetGoalsAsync(floors: 20);
            
            Assert.IsNotNull(response);
        }

        [Test] [Category("Portable")]
        public async void SetGoalsAsync_StepsSet()
        {
            var fitbitClient = SetupFitbitClient("steps=10000");

            var response = await fitbitClient.SetGoalsAsync(steps: 10000);

            Assert.IsNotNull(response);
        }

        [Test] [Category("Portable")]
        public async void SetGoalsAsync_ActiveMinuitesSet()
        {
            var fitbitClient = SetupFitbitClient("activeMinutes=50");

            var response = await fitbitClient.SetGoalsAsync(activeMinutes: 50);

            Assert.IsNotNull(response);
        }

        [Test] [Category("Portable")]
        public async void SetGoalsAsync_AllSet()
        {
            var fitbitClient = SetupFitbitClient("caloriesOut=2000&distance=8.5&floors=20&steps=10000&activeMinutes=50");

            var response = await fitbitClient.SetGoalsAsync(2000, 8.5M, 20, 10000, 50);

            Assert.IsNotNull(response);
        }

        public FitbitClient SetupFitbitClient(string expectedBody)
        {
            string content = SampleDataHelper.GetContent("ActivityGoals.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.Created) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>(async (message, token) =>
            {
                Assert.AreEqual(HttpMethod.Post, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/goals/daily.json", message.RequestUri.AbsoluteUri);

                var body = await message.Content.ReadAsStringAsync();
                Assert.AreEqual(true, body.Equals(expectedBody));
            });

            return Helper.CreateFitbitClient(responseMessage, verification);
        }
    }
}
