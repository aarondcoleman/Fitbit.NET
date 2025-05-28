using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq.Protected;
using Moq;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class ActiveZoneMinutesTests
    {
        [Test]
        [Category("Portable")]
        public async Task GetActiveZoneMinutesTimeSeriesAsync_Success()
        {
            string content = SampleDataHelper.GetContent("ActiveZoneMinutes.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/active-zone-minutes/date/2025-03-26/7d.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            List<ActiveZoneMinutesSummary> response = await fitbitClient.GetActiveZoneMinutesTimeSeriesAsync(new DateTime(2025, 3, 26), null, DateRangePeriod.SevenDays);

            ValidateDataList(response);
        }

        [Test]
        [Category("Portable")]
        public async Task GetGetActiveZoneMinutesTimeSeriesAsync_DoubleDate_Success()
        {
            string content = SampleDataHelper.GetContent("ActiveZoneMinutes.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/active-zone-minutes/date/2025-03-26/2025-03-29.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetActiveZoneMinutesTimeSeriesAsync(new DateTime(2025, 3, 26), new DateTime(2025, 3, 29));

            ValidateDataList(response);
        }

        [Test]
        [Category("Portable")]
        public async Task GetActiveZoneMinutesTimeSeriesAsync_ExtendedDateRange_Success()
        {
            string content1 = SampleDataHelper.GetContent("ActiveZoneMinutes2.json");
            string content2 = SampleDataHelper.GetContent("ActiveZoneMinutes.json");

            // Define the expected requests
            var expectedRequests = new List<string>
            {
                "https://api.fitbit.com/1/user/-/activities/active-zone-minutes/date/2022-03-15/2025-03-13.json",
                "https://api.fitbit.com/1/user/-/activities/active-zone-minutes/date/2025-03-14/2025-03-29.json"
            };

            // Mock the HttpMessageHandler
            var mockHandler = new Mock<HttpMessageHandler>();

            // Setup the mock to return different responses for each call
            var responseQueue = new Queue<HttpResponseMessage>();
            responseQueue.Enqueue(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content1) }); // First call
            responseQueue.Enqueue(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content2) }); // Second call

            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => expectedRequests.Contains(req.RequestUri.AbsoluteUri)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() =>
                {
                    Assert.IsTrue(responseQueue.Count > 0, "Unexpected number of requests made.");
                    return responseQueue.Dequeue();
                })
                .Verifiable();

            // Create the FitbitClient with the mocked handler
            var fitbitClient = new FitbitClient(messageHandler => new HttpClient(mockHandler.Object));

            // Call the method under test
            List<ActiveZoneMinutesSummary> response = await fitbitClient.GetActiveZoneMinutesTimeSeriesAsync(new DateTime(2022, 3, 15), new DateTime(2025, 3, 29));

            // Validate the response
            ValidateExtendedDataList(response);

            // Verify that all expected requests were made
            foreach (var expectedRequest in expectedRequests)
            {
                mockHandler.Protected().Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.AbsoluteUri == expectedRequest),
                    ItExpr.IsAny<CancellationToken>()
                );
            }
        }


        private void ValidateDataList(List<ActiveZoneMinutesSummary> dataList)
        {
            Assert.IsNotNull(dataList);
            Assert.AreEqual(4, dataList.Count);

            var item = dataList.First();
            dataList.Remove(item);

            Assert.IsNotNull(item.Value);
            Assert.AreEqual(DateTime.Parse("2025-03-26"), item.DateTime);
            Assert.AreEqual(10, item.Value.ActiveZoneMinutes);
            Assert.AreEqual(8, item.Value.FatBurnActiveZoneMinutes);
            Assert.AreEqual(2, item.Value.CardioActiveZoneMinutes);
            Assert.AreEqual(0, item.Value.PeakActiveZoneMinutes);

            item = dataList.First();
            dataList.Remove(item);

            Assert.IsNotNull(item.Value);
            Assert.AreEqual(DateTime.Parse("2025-03-27"), item.DateTime);
            Assert.AreEqual(8, item.Value.ActiveZoneMinutes);
            Assert.AreEqual(8, item.Value.FatBurnActiveZoneMinutes);
            Assert.AreEqual(0, item.Value.CardioActiveZoneMinutes);
            Assert.AreEqual(0, item.Value.PeakActiveZoneMinutes);

            item = dataList.First();
            dataList.Remove(item);

            Assert.IsNotNull(item.Value);
            Assert.AreEqual(DateTime.Parse("2025-03-28"), item.DateTime);
            Assert.AreEqual(15, item.Value.ActiveZoneMinutes);
            Assert.AreEqual(15, item.Value.FatBurnActiveZoneMinutes);
            Assert.AreEqual(0, item.Value.CardioActiveZoneMinutes);
            Assert.AreEqual(0, item.Value.PeakActiveZoneMinutes);

            item = dataList.First();
            dataList.Remove(item);

            Assert.IsNotNull(item.Value);
            Assert.AreEqual(DateTime.Parse("2025-03-29"), item.DateTime);
            Assert.AreEqual(14, item.Value.ActiveZoneMinutes);
            Assert.AreEqual(14, item.Value.FatBurnActiveZoneMinutes);
            Assert.AreEqual(0, item.Value.CardioActiveZoneMinutes);
            Assert.AreEqual(0, item.Value.PeakActiveZoneMinutes);
        }

        private void ValidateExtendedDataList(List<ActiveZoneMinutesSummary> dataList)
        {
            Assert.IsNotNull(dataList);
            Assert.AreEqual(7, dataList.Count);

            var item = dataList.First();
            dataList.Remove(item);

            Assert.IsNotNull(item.Value);
            Assert.AreEqual(DateTime.Parse("2022-04-01"), item.DateTime);
            Assert.AreEqual(4, item.Value.ActiveZoneMinutes);
            Assert.AreEqual(4, item.Value.FatBurnActiveZoneMinutes);
            Assert.AreEqual(0, item.Value.CardioActiveZoneMinutes);
            Assert.AreEqual(0, item.Value.PeakActiveZoneMinutes);

            item = dataList.First();
            dataList.Remove(item);

            Assert.IsNotNull(item.Value);
            Assert.AreEqual(DateTime.Parse("2022-04-02"), item.DateTime);
            Assert.AreEqual(19, item.Value.ActiveZoneMinutes);
            Assert.AreEqual(19, item.Value.FatBurnActiveZoneMinutes);
            Assert.AreEqual(0, item.Value.CardioActiveZoneMinutes);
            Assert.AreEqual(0, item.Value.PeakActiveZoneMinutes);

            item = dataList.First();
            dataList.Remove(item);

            Assert.IsNotNull(item.Value);
            Assert.AreEqual(DateTime.Parse("2022-04-03"), item.DateTime);
            Assert.AreEqual(1, item.Value.ActiveZoneMinutes);
            Assert.AreEqual(1, item.Value.FatBurnActiveZoneMinutes);
            Assert.AreEqual(0, item.Value.CardioActiveZoneMinutes);
            Assert.AreEqual(0, item.Value.PeakActiveZoneMinutes);
            
            ValidateDataList(dataList);
        }
    }
}