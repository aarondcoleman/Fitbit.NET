using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class TimeSeriesDataListTests
    {
        [Test] [Category("Portable")]
        public async void GetTimeSeriesDataListAsync_Success()
        {
            string content = SampleDataHelper.GetContent("TimeSeries-ActivitiesDistance.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/distance/date/2014-09-07/1d.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            
            var response = await fitbitClient.GetTimeSeriesAsync(TimeSeriesResourceType.Distance, new DateTime(2014, 9, 7), DateRangePeriod.OneDay);

            ValidateDataList(response);
        }

        [Test] [Category("Portable")]
        public async void GetTimeSeriesDataListAsync_DoubleDate_Success()
        {
            string content = SampleDataHelper.GetContent("TimeSeries-ActivitiesDistance.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/activities/distance/date/2014-09-07/2014-09-14.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            
            var response = await fitbitClient.GetTimeSeriesAsync(TimeSeriesResourceType.Distance, new DateTime(2014, 9, 7), new DateTime(2014, 9, 14));
            
            ValidateDataList(response);
        }

        [Test] [Category("Portable")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Serializer_Passed_Invalid_Data_To_Serialize()
        {
            var serialiser = new JsonDotNetSerializer();
            serialiser.GetTimeSeriesDataList(string.Empty);
        }

        [Test] [Category("Portable")]
        public void Can_Deserialize_Activities_Distance()
        {
            string content = SampleDataHelper.GetContent("TimeSeries-ActivitiesDistance.json");
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