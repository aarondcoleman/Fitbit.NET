using System;
using System.Collections.Generic;
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
    public class HeartRateTests
    {
        [Test]
        [Category("Portable")]
        public async void GetHeartRateTimeSeriesAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetHeartRateTimeSeries.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/heart/date/today/1d.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetHeartRateTimeSeries("today", "1d");
            ValidateHeartRateTimeSeriesData(response);
        }

        [Test]
        [Category("Portable")]
        public void GetHeartRateTimeSeriesAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.BadRequest);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<List<HeartActivitiesTimeSeries>>> result = () => fitbitClient.GetHeartRateTimeSeries("", "");

            result.ShouldThrow<FitbitRequestException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test]
        [Category("Portable")]
        public void Can_Deserialize_HeartRateTimeSeries()
        {
            string content = SampleDataHelper.GetContent("GetHeartRateTimeSeries.json");
            var deserializer = new JsonDotNetSerializer { RootProperty = "activities-heart" };

            List<HeartActivitiesTimeSeries> stats = deserializer.Deserialize<List<HeartActivitiesTimeSeries>>(content);

            ValidateHeartRateTimeSeriesData(stats);
        }

        [Test]
        [Category("Portable")]
        public async void GetHeartRateIntradayTimeSeriesAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetHeartRateIntradayTimeSeries.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/heart/date/today/1d.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetHeartRateTimeSeries("today", "1d");
            ValidateHeartRateTimeSeriesData(response);
        }

        [Test]
        [Category("Portable")]
        public void GetHeartRateIntradayTimeSeriesAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.BadRequest);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<HeartActivitiesIntraday>> result = () => fitbitClient.GetHeartRateIntradayTimeSeries("", "");

            result.ShouldThrow<FitbitRequestException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test]
        [Category("Portable")]
        public void Can_Deserialize_HeartRateIntradayTimeSeries()
        {
            string content = SampleDataHelper.GetContent("GetHeartRateIntradayTimeSeries.json");
            var deserializer = new JsonDotNetSerializer {RootProperty = "activities-heart-intraday"};

            HeartActivitiesIntraday stats = deserializer.Deserialize<HeartActivitiesIntraday>(content);

            ValidateHeartRateTimeSeriesData(stats);
        }

        private void ValidateHeartRateTimeSeriesData(HeartActivitiesIntraday activity)
        {
            activity.Dataset.First().Time.TimeOfDay.Should().Be(new TimeSpan(0,0,0,0));
            activity.Dataset.First().Value.Should().Be(58);
        }

        private void ValidateHeartRateTimeSeriesData(List<HeartActivitiesTimeSeries> activities)
        {
            var activity = activities.First();

            activity.DateTime.Should().Be(new DateTime(2017, 6, 29));
            //activity.Value.CustomHeartRateZones
              
            //activity.Value.HeartRateZones.First().CaloriesOut.Should().Be(1693.83222);
            activity.Value.HeartRateZones.First().Max.Should().Be(95);
            activity.Value.HeartRateZones.First().Min.Should().Be(30);
            activity.Value.HeartRateZones.First().Minutes.Should().Be(1122);
            activity.Value.HeartRateZones.First().Name.Should().Be("Out of Range");

            activity.Value.RestingHeartRate.Should().Be(59);
        }
    }
}
