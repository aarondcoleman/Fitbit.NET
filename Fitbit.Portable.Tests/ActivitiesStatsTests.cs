using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Fitbit.Api.Portable;
using Fitbit.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    public class ActivitiesStatsTests
    {
        [Test]
        [Category("Portable")]
        public async void GetActivityStatsAsync_Success()
        {
            string content = SampleDataHelper.GetContent("ActivitiesStats.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetActivitiesStatsAsync();

            ValidateActivity(response);
        }

        [Test]
        [Category("Portable")]
        public void Can_Deserialize_Activities()
        {
            string content = SampleDataHelper.GetContent("ActivitiesStats.json");
            var deserializer = new JsonDotNetSerializer();

            ActivitiesStats stats = deserializer.Deserialize<ActivitiesStats>(content);

            ValidateActivity(stats);
        }
        
        private void ValidateActivity(ActivitiesStats stats)
        {
            stats.Lifetime.Total.CaloriesOut.Should().Be(60223);
            stats.Lifetime.Total.Distance.Should().Be(2711.62);
            stats.Lifetime.Total.Floors.Should().Be(2500);
            stats.Lifetime.Total.Steps.Should().Be(203300);

            stats.Lifetime.Tracker.CaloriesOut.Should().Be(27565);
            stats.Lifetime.Tracker.Distance.Should().Be(2579.82);
            stats.Lifetime.Tracker.Floors.Should().Be(2500);
            stats.Lifetime.Tracker.Steps.Should().Be(106934);

            stats.Best.Total.CaloriesOut.Date.Should().Be(new DateTime(2012, 01, 07));
            stats.Best.Total.CaloriesOut.Value.Should().Be(4015);
            stats.Best.Total.Distance.Date.Should().Be(new DateTime(2012, 01, 07));
            stats.Best.Total.Distance.Value.Should().Be(20.31597);
            stats.Best.Total.Floors.Date.Should().Be(new DateTime(2012, 01, 29));
            stats.Best.Total.Floors.Value.Should().Be(14);
            stats.Best.Total.Steps.Date.Should().Be(new DateTime(2012, 01, 07));
            stats.Best.Total.Steps.Value.Should().Be(26901);

            stats.Best.Tracker.CaloriesOut.Date.Should().Be(new DateTime(2012, 01, 07));
            stats.Best.Tracker.CaloriesOut.Value.Should().Be(5699);
            stats.Best.Tracker.Distance.Date.Should().Be(new DateTime(2012, 01, 07));
            stats.Best.Tracker.Distance.Value.Should().Be(20.31597);
            stats.Best.Tracker.Floors.Date.Should().Be(new DateTime(2012, 01, 29));
            stats.Best.Tracker.Floors.Value.Should().Be(14);
            stats.Best.Tracker.Steps.Date.Should().Be(new DateTime(2012, 01, 07));
            stats.Best.Tracker.Steps.Value.Should().Be(26901);
        }
    }
}
