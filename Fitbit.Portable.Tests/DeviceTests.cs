using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fitbit.Api.Portable;
using Fitbit.Models;
using Fitbit.Portable.Tests.Helpers;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class DeviceTests
    {
        [Test]
        public void Can_Deserialise_Single_Device_Details()
        {
            string content = File.ReadAllText(SampleData.PathFor("GetDevices-Single.json"));
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
            string content = File.ReadAllText(SampleData.PathFor("GetDevices-Double.json"));
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