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
    public class HRVTests
    {
        [Test]
        [Category("Portable")]
        public async Task GetHRVSummaryAsync_ByDate_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetHRVSummaryByDate.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/hrv/date/2021-10-25.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            var response = await fitbitClient.GetHRVSummaryAsync(new DateTime(2021, 10, 25));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(new DateTime(2021, 10, 25), response.First().DateTime);
            Assert.IsNotNull(response.First().Value);
            Assert.AreEqual(34.938, response.First().Value.DailyRMSSD);
            Assert.AreEqual(31.567, response.First().Value.DeepRMSSD);
        }

        [Test]
        [Category("Portable")]
        public void GetHRVSummaryAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.BadRequest);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<List<HrvSummaryLog>>> result = () => fitbitClient.GetHRVSummaryAsync(new DateTime(2021, 10, 25));

            result.Should().Throw<FitbitRequestException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test]
        [Category("Portable")]
        public async Task GetHRVSummaryAsync_ByInterval_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetHRVSummaryByInterval.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/hrv/date/2021-10-25/2021-10-27.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            List<HrvSummaryLog> response = await fitbitClient.GetHRVSummaryAsync(new DateTime(2021, 10, 25), new DateTime(2021, 10, 27));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(3, response.Count);
            Assert.IsNotNull(response.Last().Value);
            Assert.AreEqual(60.887, response.Last().Value.DailyRMSSD);
            Assert.AreEqual(64.887, response.Last().Value.DeepRMSSD);
        }

        [Test]
        [Category("Portable")]
        public async Task GetHRVIntradayAsync_ByDate_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetHRVIntradayByDate.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/hrv/date/2021-10-25/all.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            List<HrvIntraday> response = await fitbitClient.GetHRVIntradayAsync(new DateTime(2021, 10, 25));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(new DateTime(2021, 10, 25), response.First().DateTime);
            Assert.AreEqual(4, response.First().Minutes.Count);
            Assert.AreEqual(26.617, response.First().Minutes[0].Value.RMSSD);
            Assert.AreEqual(0.935, response.First().Minutes[0].Value.Coverage);
            Assert.AreEqual(126.514, response.First().Minutes[0].Value.HF);
            Assert.AreEqual(471.897, response.First().Minutes[0].Value.LF);

        }

        [Test]
        [Category("Portable")]
        public void GetHRVIntradayAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.BadRequest);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<List<HrvIntraday>>> result = () => fitbitClient.GetHRVIntradayAsync(new DateTime(2021, 10, 25));

            result.Should().Throw<FitbitRequestException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test]
        [Category("Portable")]
        public async Task GetHRVIntradayAsync_ByInterval_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetHRVIntradayByInterval.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/hrv/date/2021-10-25/2021-10-26/all.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            List<HrvIntraday> response = await fitbitClient.GetHRVIntradayAsync(new DateTime(2021, 10, 25), new DateTime(2021, 10, 26));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count);
            Assert.AreEqual(new DateTime(2021, 10, 25), response.First().DateTime);
            Assert.AreEqual(4, response.First().Minutes.Count);
            Assert.AreEqual(new DateTime(2021, 10, 26), response.Last().DateTime);
            Assert.AreEqual(4, response.Last().Minutes.Count);
        }
    }
}

