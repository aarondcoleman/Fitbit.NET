using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class TimeSeriesDataListIntTests
    {
        [Test]
        public async void GetTimeSeriesDataListIntAsync_Success()
        {
            string content = "TimeSeries-ActivitiesSteps.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            string uri = "https://api.fitbit.com/1/user/-/activities/steps/date/2014-09-04/7d.json";
            fakeResponseHandler.AddResponse(new Uri(uri), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetTimeSeriesIntAsync(TimeSeriesResourceType.Steps, new DateTime(2014, 9, 4), DateRangePeriod.SevenDays);
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            ValidateDataList(response.Data);
        }

        [Test]
        public async void GetTimeSeriesDataListIntAsync_DoubleDate_Success()
        {
            string content = "TimeSeries-ActivitiesSteps.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            string uri = "https://api.fitbit.com/1/user/-/activities/steps/date/2014-09-04/2014-09-07.json";
            fakeResponseHandler.AddResponse(new Uri(uri), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetTimeSeriesIntAsync(TimeSeriesResourceType.Steps, new DateTime(2014, 9, 4), new DateTime(2014, 9, 7));
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            ValidateDataList(response.Data);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Serializer_Passed_Invalid_Data_To_Serialize()
        {
            var serialiser = new JsonDotNetSerializer();
            serialiser.GetTimeSeriesDataListInt(string.Empty);
        }

        [Test]
        public void Can_Deserialize_Activities_Distance()
        {
            string content = "TimeSeries-ActivitiesSteps.json".GetContent();
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