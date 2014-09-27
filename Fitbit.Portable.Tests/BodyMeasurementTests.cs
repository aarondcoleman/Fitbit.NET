using System;
using System.Net;
using System.Net.Http;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class BodyMeasurementTests
    {
        [Test]
        public async void GetBodyMeasurementsAsync_Success()
        {
            string content = "GetBodyMeasurements.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/body/date/2014-09-27.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetBodyMeasurementsAsync(new DateTime(2014, 9, 27));
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            Assert.AreEqual(1, fakeResponseHandler.CallCount);

            ValidateBodyMeasurements(response.Data);
        }

        [Test]
        public async void GetBodyMeasurementsAsync_Errors()
        {
            string content = "GetBodyMeasurements.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/ghjhg/body/date/2014-09-27.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetBodyMeasurementsAsync(new DateTime(2014, 9, 27));
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
        }
        [Test]
        public void Can_Deserialize_BodyMeasurements()
        {
            string content = "GetBodyMeasurements.json".GetContent();
            var deserializer = new JsonDotNetSerializer();

            BodyMeasurements bodyMeasurements = deserializer.Deserialize<BodyMeasurements>(content);

            ValidateBodyMeasurements(bodyMeasurements);
        }

        private void ValidateBodyMeasurements(BodyMeasurements bm)
        {
            Assert.IsNotNull(bm);

            Assert.IsNotNull(bm.Body);
            Assert.IsNotNull(bm.Goals);

            // body
            Assert.AreEqual(40, bm.Body.Bicep);
            Assert.AreEqual(16.14f, bm.Body.BMI);
            Assert.AreEqual(11.2f, bm.Body.Calf);
            Assert.AreEqual(50, bm.Body.Chest);
            Assert.AreEqual(0, bm.Body.Fat);
            Assert.AreEqual(22.3f, bm.Body.Forearm);
            Assert.AreEqual(34, bm.Body.Hips);
            Assert.AreEqual(30, bm.Body.Neck);
            Assert.AreEqual(45, bm.Body.Thigh);
            Assert.AreEqual(60, bm.Body.Waist);
            Assert.AreEqual(80.55f, bm.Body.Weight);

            // goals
            Assert.AreEqual(75, bm.Goals.Weight);
        }
    }
}
