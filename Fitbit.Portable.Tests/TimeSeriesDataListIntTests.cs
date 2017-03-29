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
    public class TimeSeriesDataListIntTests
    {
        [Test] [Category("Portable")]
        public async void GetTimeSeriesDataListIntAsync_Success()
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

        [Test] [Category("Portable")]
        public async void GetTimeSeriesDataListIntAsync_DoubleDate_Success()
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

        [Test] [Category("Portable")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Serializer_Passed_Invalid_Data_To_Serialize()
        {
            var serialiser = new JsonDotNetSerializer();
            serialiser.GetTimeSeriesDataListInt(string.Empty);
        }

        [Test] [Category("Portable")]
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