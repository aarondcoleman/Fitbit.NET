using System;
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
    public class WaterTests
    {
        [Test] [Category("Portable")]
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

            ValidateWaterData(response);
        }

        [Test] [Category("Portable")]
        public void GetWaterAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse();
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);
            
            Func<Task<Food>> result = () => fitbitClient.GetFoodAsync(new DateTime(2015, 1, 12));

            result.ShouldThrow<FitbitException>();
        }

        [Test] [Category("Portable")]
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
            
            Assert.AreEqual(300, response.Amount);
        }

        [Test] [Category("Portable")]
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
            
            await fitbitClient.DeleteWaterLogAsync(1234);

            //Assert.IsTrue(response.Success);
        }

        [Test] [Category("Portable")]
        public void Can_Deserialize_Water_Data_Json()
        {
            string content = SampleDataHelper.GetContent("GetWater-WaterData.json");

            var deserializer = new JsonDotNetSerializer();

            WaterData result = deserializer.Deserialize<WaterData>(content);

            ValidateWaterData(result);
        }

        [Test] [Category("Portable")]
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
