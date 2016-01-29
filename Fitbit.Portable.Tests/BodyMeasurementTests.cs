using System;
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
    public class BodyMeasurementTests
    {
        [Test] [Category("Portable")]
        public async void GetBodyMeasurementsAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetBodyMeasurements.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/body/date/2014-09-27.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetBodyMeasurementsAsync(new DateTime(2014, 9, 27));

            ValidateBodyMeasurements(response);
        }

        [Test] [Category("Portable")]
        public void GetBodyMeasurementsAsync_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse(HttpStatusCode.Unauthorized);
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<BodyMeasurements>> result = () => fitbitClient.GetBodyMeasurementsAsync(new DateTime(2014, 9, 27));

            result.ShouldThrow<FitbitRequestException>().Which.ApiErrors.Count.Should().Be(1);
        }

        [Test] [Category("Portable")]
        public void Can_Deserialize_BodyMeasurements()
        {
            string content = SampleDataHelper.GetContent("GetBodyMeasurements.json");
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
