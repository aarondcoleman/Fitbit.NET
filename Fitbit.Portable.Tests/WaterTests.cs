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
    public class WaterTests
    {
        [Test]
        public async void GetWaterAsync_Success()
        {
            string content = "GetWater-WaterData.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/foods/log/water/date/2015-01-12.json"),new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) } );
            
            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetWaterAsync(new DateTime(2015, 1, 12));
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);
        }

        [Test]
        public async void GetWaterAsync_Errors()
        {
            string content = "GetWater-WaterData.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/ghjhg/foods/log/water/date/2015-001-12.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetFoodAsync(new DateTime(2015, 1, 12));
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
        }

        [Test]
        public async void PostWaterLogAsync_Success()
        {
            string content = "LogWater-WaterLog.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/foods/log/water.json?amount=300&date=2015-01-12"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.LogWaterAsync(new DateTime(2015, 1, 12), new WaterLog { Amount = 300 });
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            Assert.IsNotNull(response.Data);
            Assert.AreEqual(300, response.Data.Amount);
        }

        [Test]
        public async void DeleteWaterLogAsync_Success()
        {
            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/foods/log/water/1234.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(string.Empty) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.DeleteWaterLogAsync(1234);
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);
        }

        [Test]
        public void Can_Deserialize_Water_Data_Json()
        {
            string content = "GetWater-WaterData.json".GetContent();

            var deserializer = new JsonDotNetSerializer();

            WaterData result = deserializer.Deserialize<WaterData>(content);

            var firstWaterLog = result.Water.FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual(1300, result.Summary.Water);
            Assert.AreEqual(5, result.Water.Count);

            Assert.IsNotNull(firstWaterLog);
            Assert.AreEqual(200, firstWaterLog.Amount);
            Assert.AreEqual(508693835, firstWaterLog.LogId);

        }

        [Test]
        public void Can_Deserialize_Water_Log_Json()
        {
            string content = "LogWater-WaterLog.json".GetContent();

            var deserializer = new JsonDotNetSerializer { RootProperty = "waterLog"};

            WaterLog result = deserializer.Deserialize<WaterLog>(content);

            Assert.IsNotNull(result);
            Assert.AreEqual(508728882, result.LogId);
            Assert.AreEqual(300, result.Amount);
        }
    }
}
