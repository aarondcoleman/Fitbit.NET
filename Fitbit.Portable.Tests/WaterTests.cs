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
    public class WaterTests
    {
        [Test]
        public async void GetWaterAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetWater-WaterData.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/foods/log/water/date/2015-01-12.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            
            var response = await fitbitClient.GetWaterAsync(new DateTime(2015, 1, 12));

            Assert.IsTrue(response.Success);
            ValidateWaterData(response.Data);
        }

        [Test]
        public async void GetWaterAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse();
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            
            var response = await fitbitClient.GetFoodAsync(new DateTime(2015, 1, 12));

            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
            Assert.AreEqual(1, response.Errors.Count);
        }

        [Test]
        public async void PostWaterLogAsync_Success()
        {
            string content = SampleDataHelper.GetContent("LogWater-WaterLog.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Post, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/foods/log/water.json?amount=300&date=2015-01-12", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            
            var response = await fitbitClient.LogWaterAsync(new DateTime(2015, 1, 12), new WaterLog { Amount = 300 });

            Assert.IsTrue(response.Success);
            Assert.IsNotNull(response.Data);
            Assert.AreEqual(300, response.Data.Amount);
        }

        [Test]
        public async void DeleteWaterLogAsync_Success()
        {
            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Delete, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/foods/log/water/1234.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            
            var response = await fitbitClient.DeleteWaterLogAsync(1234);

            Assert.IsTrue(response.Success);
        }

        [Test]
        public void Can_Deserialize_Water_Data_Json()
        {
            string content = SampleDataHelper.GetContent("GetWater-WaterData.json");

            var deserializer = new JsonDotNetSerializer();

            WaterData result = deserializer.Deserialize<WaterData>(content);

            ValidateWaterData(result);
        }

        [Test]
        public void Can_Deserialize_Water_Log_Json()
        {
            string content = SampleDataHelper.GetContent("LogWater-WaterLog.json");

            var deserializer = new JsonDotNetSerializer { RootProperty = "waterLog"};

            WaterLog result = deserializer.Deserialize<WaterLog>(content);

            Assert.IsNotNull(result);
            Assert.AreEqual(508728882, result.LogId);
            Assert.AreEqual(300, result.Amount);
        }

        private void ValidateWaterData(WaterData result)
        {
            var firstWaterLog = result.Water.FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual(1300, result.Summary.Water);
            Assert.AreEqual(5, result.Water.Count);

            Assert.IsNotNull(firstWaterLog);
            Assert.AreEqual(200, firstWaterLog.Amount);
            Assert.AreEqual(508693835, firstWaterLog.LogId);
        }
    }
}
