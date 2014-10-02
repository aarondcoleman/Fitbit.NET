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
    public class TimeSeriesDataListTests
    {
        [Test]
        public async void GetTimeSeriesDataListAsync_Success()
        {
            string content = "TimeSeries-ActivitiesDistance.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            string uri = "https://api.fitbit.com/1/user/-/activities/distance/date/2014-09-07/1d.json";
            fakeResponseHandler.AddResponse(new Uri(uri), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetTimeSeriesAsync(TimeSeriesResourceType.Distance, new DateTime(2014, 9, 7), DateRangePeriod.OneDay);
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            ValidateDataList(response.Data);
        }

        [Test]
        public async void GetTimeSeriesDataListAsync_DoubleDate_Success()
        {
            string content = "TimeSeries-ActivitiesDistance.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            string uri = "https://api.fitbit.com/1/user/-/activities/distance/date/2014-09-07/2014-09-14.json";
            fakeResponseHandler.AddResponse(new Uri(uri), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetTimeSeriesAsync(TimeSeriesResourceType.Distance, new DateTime(2014, 9, 7), new DateTime(2014, 9, 14));
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
            serialiser.GetTimeSeriesDataList(string.Empty);
        }

        [Test]
        public void Can_Deserialize_Activities_Distance()
        {
            string content = "TimeSeries-ActivitiesDistance.json".GetContent();
            var deserializer = new JsonDotNetSerializer
            {
                RootProperty = TimeSeriesResourceType.Distance.ToTimeSeriesProperty()
            };

            var result = deserializer.GetTimeSeriesDataList(content);
            ValidateDataList(result);
        }

        private void ValidateDataList(TimeSeriesDataList dataList)
        {
            Assert.IsNotNull(dataList);
            Assert.IsNotNull(dataList.DataList);
            Assert.AreEqual(8, dataList.DataList.Count);

            var item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual("1.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-12"), item.DateTime);

            item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual("2.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-13"), item.DateTime);

            item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual("4.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-14"), item.DateTime);

            item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual("8.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-15"), item.DateTime);

            item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual("16.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-16"), item.DateTime);

            item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual("32.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-17"), item.DateTime);

            item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual("64.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-18"), item.DateTime);

            item = dataList.DataList.First();
            dataList.DataList.Remove(item);

            Assert.AreEqual("128.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-19"), item.DateTime);

            Assert.AreEqual(0, dataList.DataList.Count);
        }
    }
}