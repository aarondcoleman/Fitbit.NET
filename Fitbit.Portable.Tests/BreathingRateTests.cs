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
    public class BreathingRateTests
    {
        [Test]
        [Category("Portable")]
        public async Task GetBreathingRateSummaryAsync_ByDate_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetBreathingRateSummaryByDate.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/br/date/2021-10-25.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            List<BreathingRateSummary> response = await fitbitClient.GetBreathingRateSummaryAsync(new DateTime(2021, 10, 25));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(new DateTime(2021, 10, 25), response.First().DateTime);
            Assert.IsNotNull(response.First().Value);
            Assert.AreEqual(17,8, response.First().Value.Rate);
        }

        [Test]
        [Category("Portable")]
        public void GetBreathingRateSummaryAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.BadRequest);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<List<BreathingRateSummary>>> result = () => fitbitClient.GetBreathingRateSummaryAsync(new DateTime(2021, 10, 25));

            result.Should().Throw<FitbitRequestException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test]
        [Category("Portable")]
        public async Task GetBreathingRateSummaryAsync_ByInterval_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetBreathingRateSummaryByInterval.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/br/date/2021-10-25/2021-10-26.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            List<BreathingRateSummary> response = await fitbitClient.GetBreathingRateSummaryAsync(new DateTime(2021, 10, 25), new DateTime(2021, 10, 26));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count);
            Assert.IsNotNull(response.Last().Value);
            Assert.AreEqual(17.8, response.First().Value.Rate);
            Assert.AreEqual(17.9, response.Last().Value.Rate);
        }

        [Test]
        [Category("Portable")]
        public async Task GetBreathingRateIntradayAsync_ByDate_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetBreathingRateIntradayByDate.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/br/date/2021-10-25/all.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            List<BreathingRateIntraday> response = await fitbitClient.GetBreathingRateIntradayAsync(new DateTime(2021, 10, 25));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(new DateTime(2021, 10, 25), response.First().DateTime);
            Assert.AreEqual(16.8, response.First().Value.DeepSleepSummary.Rate);
            Assert.AreEqual(-1, response.First().Value.RemSleepSummary.Rate);
            Assert.AreEqual(17.8, response.First().Value.FullSleepSummary.Rate);
            Assert.AreEqual(16.8, response.First().Value.LightSleepSummary.Rate);

        }

        [Test]
        [Category("Portable")]
        public void GetBreathingRateIntradayAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.BadRequest);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<List<BreathingRateIntraday>>> result = () => fitbitClient.GetBreathingRateIntradayAsync(new DateTime(2021, 10, 25));

            result.Should().Throw<FitbitRequestException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test]
        [Category("Portable")]
        public async Task GetBreathingRateIntradayAsync_ByInterval_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetBreathingRateIntradayByInterval.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/br/date/2021-10-25/2021-10-26/all.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            List<BreathingRateIntraday> response = await fitbitClient.GetBreathingRateIntradayAsync(new DateTime(2021, 10, 25), new DateTime(2021, 10, 26));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count);
            Assert.AreEqual(new DateTime(2021, 10, 25), response.First().DateTime);
            Assert.AreEqual(16.8, response.First().Value.DeepSleepSummary.Rate);
            Assert.AreEqual(-1, response.First().Value.RemSleepSummary.Rate);
            Assert.AreEqual(17.8, response.First().Value.FullSleepSummary.Rate);
            Assert.AreEqual(16.8, response.First().Value.LightSleepSummary.Rate);
            Assert.AreEqual(new DateTime(2021, 10, 26), response.Last().DateTime);
        }
    }
}

