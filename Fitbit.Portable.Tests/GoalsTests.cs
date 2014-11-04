using System;
using System.Net;
using System.Net.Http;
using Fitbit.Api.Portable;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class GoalsTests
    {
        [Test]
        [Ignore]
        [ExpectedException(typeof(Exception))]
        public void SetGoalsAsync_NoGoalsSet()
        {
            var client = new FitbitClient("key", "secret", "token", "asecret");
            client.SetGoalsAsync();
        }

        [Test]
        [Ignore]
        public async void SetGoalsAsync_CaloriesOutSet()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            // no need for a response as not testing that here
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/activities/goals/daily.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var caloriesOut = 2000;

            var response = await fitbitClient.SetGoalsAsync(caloriesOut: caloriesOut);
            fakeResponseHandler.AssertAllCalled();
            Assert.AreEqual(1, fakeResponseHandler.CallCount);
        }

        [Test]
        [Ignore]
        public async void SetGoalsAsync_DistanceSet()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            // no need for a response as not testing that here
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/activities/goals/daily.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            decimal distance = 8.5M;

            var response = await fitbitClient.SetGoalsAsync(distance: distance);
            fakeResponseHandler.AssertAllCalled();
            Assert.AreEqual(1, fakeResponseHandler.CallCount);
        }

        [Test]
        [Ignore]
        public async void SetGoalsAsync_FloorsSet()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            // no need for a response as not testing that here
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/activities/goals/daily.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            int floors = 20;

            var response = await fitbitClient.SetGoalsAsync(floors: floors);
            fakeResponseHandler.AssertAllCalled();
            Assert.AreEqual(1, fakeResponseHandler.CallCount);
        }

        [Test]
        [Ignore]
        public async void SetGoalsAsync_StepsSet()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            // no need for a response as not testing that here
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/activities/goals/daily.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            int steps = 10000;

            var response = await fitbitClient.SetGoalsAsync(steps: steps);
            fakeResponseHandler.AssertAllCalled();
            Assert.AreEqual(1, fakeResponseHandler.CallCount);
        }

        [Test]
        [Ignore]
        public async void SetGoalsAsync_ActiveMinuitesSet()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            // no need for a response as not testing that here
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/activities/goals/daily.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            int steps = 10000;

            var response = await fitbitClient.SetGoalsAsync(steps: steps);
            fakeResponseHandler.AssertAllCalled();
            Assert.AreEqual(1, fakeResponseHandler.CallCount);
        }
    }
}
