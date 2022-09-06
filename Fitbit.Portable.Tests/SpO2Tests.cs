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
    public class SpO2Tests
    {
        [Test]
        [Category("Portable")]
        public async Task GetSpO2SummaryAsync_ByDate_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetSpO2SummaryByDate.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/spo2/date/2021-10-04.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            var response = await fitbitClient.GetSpO2SummaryAsync(new DateTime(2021, 10, 4));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(new DateTime(2021, 10, 4), response.First().DateTime);
            Assert.IsNotNull(response.First().Value);
            Assert.AreEqual(97.5, response.First().Value.Avg);
            Assert.AreEqual(94.0, response.First().Value.Min);
            Assert.AreEqual(100.0, response.First().Value.Max);
        }

        [Test]
        [Category("Portable")]
        public void GetSpO2SummaryAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.BadRequest);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<List<SpO2SummaryLog>>> result = () => fitbitClient.GetSpO2SummaryAsync(new DateTime(2021, 10, 4));

            result.Should().Throw<FitbitRequestException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test]
        [Category("Portable")]
        public async Task GetSpO2SummaryAsync_ByInterval_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetSpO2SummaryByInterval.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/spo2/date/2021-10-01/2021-10-04.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            List<SpO2SummaryLog> response = await fitbitClient.GetSpO2SummaryAsync(new DateTime(2021, 10, 1), new DateTime(2021, 10, 4));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(4, response.Count);
            Assert.IsNotNull(response.Last().Value);
            Assert.AreEqual(96.7, response.Last().Value.Avg);
            Assert.AreEqual(94.0, response.Last().Value.Min);
            Assert.AreEqual(100.0, response.Last().Value.Max);
        }

        [Test]
        [Category("Portable")]
        public async Task GetSpO2IntradayAsync_ByDate_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetSpO2IntradayByDate.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/spo2/date/2021-10-04/all.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            var response = await fitbitClient.GetSpO2IntradayAsync(new DateTime(2021, 10, 4));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(new DateTime(2021, 10, 4), response.First().DateTime);
            Assert.AreEqual(3, response.First().Minutes.Count);
            Assert.AreEqual(95.7, response.First().Minutes[0].Value);
            Assert.AreEqual(99.5, response.First().Minutes[1].Value);
            Assert.AreEqual(99.0, response.First().Minutes[2].Value);
        }

        [Test]
        [Category("Portable")]
        public void GetSpO2IntradayAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.BadRequest);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<List<SpO2Intraday>>> result = () => fitbitClient.GetSpO2IntradayAsync(new DateTime(2021, 10, 4));

            result.Should().Throw<FitbitRequestException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test]
        [Category("Portable")]
        public async Task GetSpO2IntradayAsync_ByInterval_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetSpO2IntradayByInterval.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/spo2/date/2021-10-01/2021-10-02/all.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            List<SpO2Intraday> response = await fitbitClient.GetSpO2IntradayAsync(new DateTime(2021, 10, 1), new DateTime(2021, 10, 2));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count);
            Assert.AreEqual(new DateTime(2021, 10, 1), response.First().DateTime);
            Assert.AreEqual(3, response.First().Minutes.Count);
            Assert.AreEqual(new DateTime(2021, 10, 2), response.Last().DateTime);
            Assert.AreEqual(2, response.Last().Minutes.Count);
        }
    }
}

