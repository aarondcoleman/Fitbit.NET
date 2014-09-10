using System;
using System.Linq;
using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class TimeSeriesTests
    {
        [Test]
        public void Can_Deserialize_Activities_Distance()
        {
            string content = "TimeSeries-ActivitiesDistance.json".GetContent();
            var deserializer = new JsonDotNetSerializer();
            deserializer.RootProperty = TimeSeriesResourceType.Distance.ToTimeSeriesProperty();
 
            TimeSeriesDataList result = deserializer.GetTimeSeriesDataList(content);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DataList);
            Assert.AreEqual(8, result.DataList.Count);

            var item = result.DataList.First();
            result.DataList.Remove(item);

            Assert.AreEqual("1.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-12"), item.DateTime);

            item = result.DataList.First();
            result.DataList.Remove(item);

            Assert.AreEqual("2.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-13"), item.DateTime);

            item = result.DataList.First();
            result.DataList.Remove(item);

            Assert.AreEqual("4.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-14"), item.DateTime);
            
            item = result.DataList.First();
            result.DataList.Remove(item);

            Assert.AreEqual("8.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-15"), item.DateTime);
            
            item = result.DataList.First();
            result.DataList.Remove(item);

            Assert.AreEqual("16.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-16"), item.DateTime);

            item = result.DataList.First();
            result.DataList.Remove(item);

            Assert.AreEqual("32.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-17"), item.DateTime);

            item = result.DataList.First();
            result.DataList.Remove(item);

            Assert.AreEqual("64.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-18"), item.DateTime);

            item = result.DataList.First();
            result.DataList.Remove(item);

            Assert.AreEqual("128.0", item.Value);
            Assert.AreEqual(DateTime.Parse("2014-08-19"), item.DateTime);

            Assert.AreEqual(0, result.DataList.Count);
        }
    }
}