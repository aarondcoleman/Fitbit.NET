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
    public class TimeSeriesDataListIntTests
    {
        [Test]
        [Category("Portable")]
        public async Task GetTimeSeriesDataListIntAsync_Success()
        {
            string content = SampleDataHelper.GetContent("TimeSeries-ActivitiesSteps.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/steps/date/2014-09-04/7d.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetTimeSeriesIntAsync(TimeSeriesResourceType.Steps, new DateTime(2014, 9, 4), DateRangePeriod.SevenDays);

            ValidateDataList(response);
        }

        [Test]
        [Category("Portable")]
        public async Task GetTimeSeriesDataListIntAsync_DoubleDate_Success()
        {
            string content = SampleDataHelper.GetContent("TimeSeries-ActivitiesSteps.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/steps/date/2014-09-04/2014-09-07.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetTimeSeriesIntAsync(TimeSeriesResourceType.Steps, new DateTime(2014, 9, 4), new DateTime(2014, 9, 7));

            ValidateDataList(response);
        }

        [Test]
        [Category("Portable")]
        public async Task GetTimeSeriesDataListIntAsync_ExtendedDateRange_Success()
        {
            string content1 = SampleDataHelper.GetContent("TimeSeries-ActivitiesSteps.json");
            string content2 = SampleDataHelper.GetContent("TimeSeries-ActivitiesSteps-Empty.json");

            // Define the expected requests
            var expectedRequests = new List<string>
            {
                "https://api.fitbit.com/1/user/-/activities/steps/date/2014-09-04/2017-09-03.json",
                "https://api.fitbit.com/1/user/-/activities/steps/date/2017-09-04/2018-09-07.json"
            };

            // Mock the HttpMessageHandler
            var mockHandler = new Mock<HttpMessageHandler>();

            // Setup the mock to return different responses for each call
            var responseQueue = new Queue<HttpResponseMessage>();
            responseQueue.Enqueue(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content1) }); // First call
            responseQueue.Enqueue(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content2) }); // Second call (empty response)

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
            var response = await fitbitClient.GetTimeSeriesIntAsync(TimeSeriesResourceType.Steps, new DateTime(2014, 9, 4), new DateTime(2018, 9, 7));

            // Validate the response
            ValidateDataList(response);

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

        [Test]
        [Category("Portable")]
        public void Serializer_Passed_Invalid_Data_To_Serialize()
        {
            var serialiser = new JsonDotNetSerializer();
            Assert.That(
                new TestDelegate(() => serialiser.GetTimeSeriesDataListInt(string.Empty)),
                Throws.ArgumentNullException
            );
        }

        [Test]
        [Category("Portable")]
        public void Can_Deserialize_Activities_Distance()
        {
            string content = SampleDataHelper.GetContent("TimeSeries-ActivitiesSteps.json");
            var deserializer = new JsonDotNetSerializer
            {
                RootProperty = TimeSeriesResourceType.Steps.ToTimeSeriesProperty()
            };

            var result = deserializer.GetTimeSeriesDataListInt(content);
            ValidateDataList(result);
        }

        private void ValidateDataList(TimeSeriesDataListInt dataList)
        {
            Assert.IsNotNull(dataList);
            Assert.IsNotNull(dataList.DataList);
            Assert.AreEqual(7, dataList.DataList.Count);

            var item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual(7428, item.Value);
            Assert.AreEqual(DateTime.Parse("2014-09-04"), item.DateTime);

            item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual(9846, item.Value);
            Assert.AreEqual(DateTime.Parse("2014-09-05"), item.DateTime);

            item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual(4274, item.Value);
            Assert.AreEqual(DateTime.Parse("2014-09-06"), item.DateTime);

            item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual(7595, item.Value);
            Assert.AreEqual(DateTime.Parse("2014-09-07"), item.DateTime);

            item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual(0, item.Value);
            Assert.AreEqual(DateTime.Parse("2014-09-08"), item.DateTime);

            item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual(9499, item.Value);
            Assert.AreEqual(DateTime.Parse("2014-09-09"), item.DateTime);

            item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual(7026, item.Value);
            Assert.AreEqual(DateTime.Parse("2014-09-10"), item.DateTime);

            Assert.AreEqual(0, dataList.DataList.Count);
        }
    }
}