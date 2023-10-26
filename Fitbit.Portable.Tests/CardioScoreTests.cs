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
    public class CardioScoreTests
    {
        [Test]
        [Category("Portable")]
        public async Task GetCardioScoreSummaryAsync_ByDate_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetCardioScoreSummaryByDate.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/cardioscore/date/2023-10-15.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            var response = await fitbitClient.GetCardioScoreSummaryAsync(new DateTime(2023, 10, 15));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(new DateTime(2023, 10, 15), response.First().DateTime);
            Assert.IsNotNull(response.First().Value);
            Assert.AreEqual("42-46", response.First().Value.VO2Max);
        }

        [Test]
        [Category("Portable")]
        public async Task GetCardioScoreSummaryAsync_ByInterval_Success()
        {
            //Arrange
            string content = SampleDataHelper.GetContent("GetCardioScoreSummaryByInterval.json");

            var responseMessage = new Func<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/cardioscore/date/2023-10-15/2023-10-16.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            //Act
            List<CardioScoreSummary> response = await fitbitClient.GetCardioScoreSummaryAsync(new DateTime(2023, 10, 15), new DateTime(2023, 10, 16));

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count);
            Assert.IsNotNull(response.Last().Value.VO2Max);
            Assert.AreEqual("42-46", response.First().Value.VO2Max);
            Assert.AreEqual("41-45", response.Last().Value.VO2Max);
        }

        [Test]
        [Category("Portable")]
        public void GetCardioScoreSummaryAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.BadRequest);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<List<CardioScoreSummary>>> result = () => fitbitClient.GetCardioScoreSummaryAsync(new DateTime(2023, 10, 15));

            result.Should().Throw<FitbitRequestException>().Which.ApiErrors.Count.Should().Be(1);
        }
    }
}

