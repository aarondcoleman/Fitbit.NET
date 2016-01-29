using System;
using System.Collections.Generic;
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
    public class DeviceTests
    {
        [Test] [Category("Portable")]
        public async void GetDevicesAsync_Success()
        {
            string content = SampleDataHelper.GetContent("GetDevices-Single.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/devices.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetDevicesAsync();
            
            Assert.AreEqual(1, response.Count);
            Device device = response.First();
            ValidateSingleDevice(device);
        }

        [Test] [Category("Portable")]
        public async void GetDevicesAsync_Success_Mulitiple()
        {
            string content = SampleDataHelper.GetContent("GetDevices-Double.json");

            var responseMessage = new Func<HttpResponseMessage>(() =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
            });

            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
                Assert.AreEqual("https://api.fitbit.com/1/user/-/devices.json", message.RequestUri.AbsoluteUri);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            var response = await fitbitClient.GetDevicesAsync();
            Assert.AreEqual(2, response.Count);
        }

        [Test] [Category("Portable")]
        public void GetDevicesAsync_Failure_Errors()
        {
            var responseMessage = Helper.CreateErrorResponse();
            var verification = new Action<HttpRequestMessage, CancellationToken>((message, token) =>
            {
                Assert.AreEqual(HttpMethod.Get, message.Method);
            });

            var fitbitClient = Helper.CreateFitbitClient(responseMessage, verification);

            Func<Task<List<Device>>> result = () => fitbitClient.GetDevicesAsync();

            result.ShouldThrow<FitbitException>();
        }

        [Test] [Category("Portable")]
        public void Can_Deserialise_Single_Device_Details()
        {
            string content = SampleDataHelper.GetContent("GetDevices-Single.json");
            var deserializer = new JsonDotNetSerializer();
            
            List<Device> result = deserializer.Deserialize<List<Device>>(content);

            Assert.IsTrue(result.Count == 1);

            Device device = result.First();

            ValidateSingleDevice(device);
        }

        [Test] [Category("Portable")]
        public void Can_Deserialise_Multiple_Device_Details()
        {
            string content = SampleDataHelper.GetContent("GetDevices-Double.json");
            var deserializer = new JsonDotNetSerializer();
            
            List<Device> result = deserializer.Deserialize<List<Device>>(content);

            Assert.IsTrue(result.Count == 2);
            Device device = result.First();
            ValidateSingleDevice(device);

            device = result.Last();
            Assert.AreEqual("High", device.Battery);
            Assert.AreEqual("Aria", device.DeviceVersion);
            Assert.AreEqual("5656777", device.Id);
            Assert.AreEqual(DateTime.Parse("2014-07-17T13:38:13.000"), device.LastSyncTime);
            Assert.AreEqual("SC1111111111", device.Mac);
            Assert.AreEqual(DeviceType.Scale, device.Type);
        }

        private void ValidateSingleDevice(Device device)
        {
            Assert.AreEqual("High", device.Battery);
            Assert.AreEqual("Zip", device.DeviceVersion);
            Assert.AreEqual("5656888", device.Id);
            Assert.AreEqual(DateTime.Parse("2014-07-17T13:38:13.000"), device.LastSyncTime);
            Assert.AreEqual("FE1111111111", device.Mac);
            Assert.AreEqual(DeviceType.Tracker, device.Type);
        }
    }
}