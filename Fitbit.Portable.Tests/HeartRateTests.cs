using System;
using System.Collections.Generic;
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
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/heart/date/{1}/{2}.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetHeartRateTimeSeries("", "");
            //ValidateBloodPressureData(response);
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
            var deserializer = new JsonDotNetSerializer();

            List<HeartActivitiesTimeSeries> stats = deserializer.Deserialize<List<HeartActivitiesTimeSeries>>(content);

            //ValidateActivity(stats);
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
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/heart/date/{1}/{2}/{3}.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetHeartRateTimeSeries("", "");
            //ValidateBloodPressureData(response);
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
            var deserializer = new JsonDotNetSerializer();

            HeartActivitiesIntraday stats = deserializer.Deserialize<HeartActivitiesIntraday>(content);

            //ValidateActivity(stats);
        }
    }
}
