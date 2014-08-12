using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class DeviceTests
    {
        [Test]
        public async void GetDevicesAsync_Success()
        {
            string content = "GetDevices-Single.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/devices.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetDevicesAsync();
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            var devices = response.Data;
            
            Assert.AreEqual(1, devices.Count);
            Assert.AreEqual(1, fakeResponseHandler.CallCount);
        }

        [Test]
        public async void GetDevicesAsync_Success_Mulitiple()
        {
            string content = "GetDevices-Double.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/-/devices.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetDevicesAsync();
            Assert.IsTrue(response.Success);
            fakeResponseHandler.AssertAllCalled();

            var devices = response.Data;

            Assert.AreEqual(2, devices.Count);
            Assert.AreEqual(1, fakeResponseHandler.CallCount);
        }

        [Test]
        public async void GetDevicesAsync_Failure_Errors()
        {
            string content = "GetDevices-Single.json".GetContent();

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddResponse(new Uri("https://api.fitbit.com/1/user/qwert/devices.json"), new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) });

            var httpClient = new HttpClient(fakeResponseHandler);
            var fitbitClient = new FitbitClient(httpClient);

            var response = await fitbitClient.GetDevicesAsync();
            Assert.IsFalse(response.Success);
            Assert.IsNull(response.Data);
        }

        [Test]
        public void Can_Deserialise_Single_Device_Details()
        {
            string content = "GetDevices-Single.json".GetContent();
            var deserializer = new JsonDotNetSerializer();
            
            List<Device> result = deserializer.Deserialize<List<Device>>(content);

            Assert.IsTrue(result.Count == 1);

            Device device = result.First();

            Assert.AreEqual("High", device.Battery);
            Assert.AreEqual("Zip", device.DeviceVersion);
            Assert.AreEqual("5656888", device.Id);
            Assert.AreEqual(DateTime.Parse("2014-07-17T13:38:13.000"), device.LastSyncTime);
            Assert.AreEqual("FE1111111111", device.Mac);
            Assert.AreEqual(DeviceType.Tracker, device.Type);
        }

        [Test]
        public void Can_Deserialise_Multiple_Device_Details()
        {
            string content = "GetDevices-Double.json".GetContent();
            var deserializer = new JsonDotNetSerializer();
            
            List<Device> result = deserializer.Deserialize<List<Device>>(content);

            Assert.IsTrue(result.Count == 2);

            Device device = result.First();

            Assert.AreEqual("High", device.Battery);
            Assert.AreEqual("Zip", device.DeviceVersion);
            Assert.AreEqual("5656888", device.Id);
            Assert.AreEqual(DateTime.Parse("2014-07-17T13:38:13.000"), device.LastSyncTime);
            Assert.AreEqual("FE1111111111", device.Mac);
            Assert.AreEqual(DeviceType.Tracker, device.Type);

            device = result.Last();

            Assert.AreEqual("High", device.Battery);
            Assert.AreEqual("Aria", device.DeviceVersion);
            Assert.AreEqual("5656777", device.Id);
            Assert.AreEqual(DateTime.Parse("2014-07-17T13:38:13.000"), device.LastSyncTime);
            Assert.AreEqual("SC1111111111", device.Mac);
            Assert.AreEqual(DeviceType.Scale, device.Type);
        }
    }
}