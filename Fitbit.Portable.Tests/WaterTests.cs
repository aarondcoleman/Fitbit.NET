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
    }
}
