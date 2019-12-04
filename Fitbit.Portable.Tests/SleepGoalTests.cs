using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using NUnit.Framework;
using AutoFixture;
using System.Threading.Tasks;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class SleepGoalTests
    {
        public Fixture fixture { get; set; }

        [OneTimeSetUp]
        public void Init()
        {
            fixture = new Fixture();
        }

        [Test]
        [Category("Portable")]
        public async Task GetSleepGoalAsync()
        {
            string content = SampleDataHelper.GetContent("GetSleepGoal.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/sleep/goal.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            var response = await fitbitClient.GetSleepGoalAsync();

            Assert.AreEqual("2019-01-01", response.UpdatedOn.ToString("yyyy-MM-dd"));
            Assert.AreEqual(420, response.MinDuration);
        }
    }
}
