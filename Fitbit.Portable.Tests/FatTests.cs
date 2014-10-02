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
    public class FatTests
    {
        [Test]
        [ExpectedException(typeof(Exception))]
        public async void GetFatAsync_DateRangePeriod_ThreeMonths()
        {
            var client = new FitbitClient("key", "secret", "access", "accessSecret");
            await client.GetFatAsync(DateTime.Now, DateRangePeriod.ThreeMonths);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public async void GetFatAsync_DateRangePeriod_SixMonths()
        {
            var client = new FitbitClient("key", "secret", "access", "accessSecret");
            await client.GetFatAsync(DateTime.Now, DateRangePeriod.SixMonths);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public async void GetFatAsync_DateRangePeriod_OneYear()
        {
            var client = new FitbitClient("key", "secret", "access", "accessSecret");
            await client.GetFatAsync(DateTime.Now, DateRangePeriod.OneYear);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public async void GetFatAsync_DateRangePeriod_Max()
        {
            var client = new FitbitClient("key", "secret", "access", "accessSecret");
            await client.GetFatAsync(DateTime.Now, DateRangePeriod.Max);
        }

        [Test]
        public async void GetFatAsync_OneDay_Success()
        {
            string content = "GetFat.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05/1d.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5), DateRangePeriod.OneDay);
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            ValidateFat(response.Data);
        }

        [Test]
        public async void GetFatAsync_SevenDay_Success()
        {
            string content = "GetFat.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05/7d.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5), DateRangePeriod.SevenDays);
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            ValidateFat(response.Data);
        }

        [Test]
        public async void GetFatAsync_OneWeek_Success()
        {
            string content = "GetFat.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05/1w.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5), DateRangePeriod.OneWeek);
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            ValidateFat(response.Data);
        }

        [Test]
        public async void GetFatAsync_ThirtyDays_Success()
        {
            string content = "GetFat.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05/30d.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5), DateRangePeriod.ThirtyDays);
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            ValidateFat(response.Data);
        }

        [Test]
        public async void GetFatAsync_OneMonth_Success()
        {
            string content = "GetFat.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05/1m.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5), DateRangePeriod.OneMonth);
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            ValidateFat(response.Data);
        }

        [Test]
        public async void GetFatAsync_Success()
        {
            string content = "GetFat.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5));
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            ValidateFat(response.Data);
        }

        [Test]
        public async void GetFatAsync_TimeSpan_Success()
        {
            string content = "GetFat.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/body/log/fat/date/2012-03-05/2012-03-06.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetFatAsync(new DateTime(2012, 3, 5), new DateTime(2012, 3, 6));
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            ValidateFat(response.Data);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public async void GetFatAsync_DateRange_Span_Too_Large()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            
            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var basedate = DateTime.Now;
            await fitbitClient.GetFatAsync(basedate.AddDays(-35), basedate);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Throws_Exception_With_Empty_String()
        {
            var deserializer = new JsonDotNetSerializer();
            deserializer.GetFat(string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Throws_Exception_With_Null_String()
        {
            var deserializer = new JsonDotNetSerializer();
            deserializer.GetFat(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Throws_Exception_With_WhiteSpace()
        {
            var deserializer = new JsonDotNetSerializer();
            deserializer.GetFat("         ");
        }

        [Test]
        public void Can_Deserialize_Fat()
        {
            string content = "GetFat.json".GetContent();
            var deserializer = new JsonDotNetSerializer();

            Fat fat = deserializer.GetFat(content);

            ValidateFat(fat);
        }

        private void ValidateFat(Fat fat)
        {
            Assert.IsNotNull(fat);

            Assert.AreEqual(2, fat.FatLogs.Count);

            var log = fat.FatLogs.First();
            Assert.IsNotNull(log);

            Assert.AreEqual(new DateTime(2012, 3, 5), log.Date);
            Assert.AreEqual(1330991999000, log.LogId);
            Assert.AreEqual(14, log.Fat);
            Assert.AreEqual(new DateTime(2012, 3,5,23,59,59).TimeOfDay, log.Time.TimeOfDay);

            fat.FatLogs.Remove(log);
            log = fat.FatLogs.First();

            Assert.IsNotNull(log);

            Assert.AreEqual(new DateTime(2012, 3, 5), log.Date);
            Assert.AreEqual(1330991999000, log.LogId);
            Assert.AreEqual(13.5, log.Fat);
            Assert.AreEqual(new DateTime(2012, 3, 5, 21, 20, 59).TimeOfDay, log.Time.TimeOfDay);

        }
    }
}