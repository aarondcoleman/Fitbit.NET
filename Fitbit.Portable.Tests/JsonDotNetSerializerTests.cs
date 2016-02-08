using System;
using Fitbit.Api.Portable;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class JsonDotNetSerializerTests
    {
        public class TestClass
        {
            [JsonProperty("testproperty")]
            public string TestProperty { get; set; }

            [JsonProperty("mydate")]  
            public DateTime MyDate { get; set; }


            // todo: array etc.
        }

        [Test] [Category("Portable")]
        public void DefaultValueCreated_String()
        {
            var serializer = new JsonDotNetSerializer();
            var defaultValue = serializer.Deserialize<TestClass>(string.Empty);
            Assert.AreEqual(default(TestClass), defaultValue);
        }

        [Test] [Category("Portable")]
        public void DefaultValueCreated_JToken()
        {
            var serializer = new JsonDotNetSerializer();
            var defaultValue = serializer.Deserialize<TestClass>((JToken)null);
            Assert.AreEqual(default(TestClass), defaultValue);
        }

        [Test] [Category("Portable")]
        public void NoRootValueCreated()
        {
            string data = "{\"testproperty\" : \"bob\" }";
            var serializer = new JsonDotNetSerializer();
            var value = serializer.Deserialize<TestClass>(data);
            Assert.IsNotNull(value);
            Assert.AreEqual("bob", value.TestProperty);
        }

        [Test] [Category("Portable")]
        public void RootPropertyValueCreated()
        {
            string data = "{\"testclass\" : {\"testproperty\" : \"bob\" } }";
            var serializer = new JsonDotNetSerializer();
            serializer.RootProperty = "testclass";
            var value = serializer.Deserialize<TestClass>(data);
            Assert.IsNotNull(value);
            Assert.AreEqual("bob", value.TestProperty);
        }

        [Test]  [Category("Portable")]  
        public void DateParsingSuccess()
        {  
            string data = "{\"mydate\" : \"1970-01-01\" }";  
            var serializer = new JsonDotNetSerializer();  
            var value = serializer.Deserialize<TestClass>(data);  
            Assert.IsNotNull(value);  
            Assert.AreEqual(new DateTime(1970, 1, 1), value.MyDate);  
        }  
        
        [Test]  [Category("Portable")]  
        public void DateParsingEmptySuccess()
        {  
            string data = "{\"mydate\" : \"\" }";  
            var serializer = new JsonDotNetSerializer();  
            var value = serializer.Deserialize<TestClass>(data);  
            Assert.IsNotNull(value);  
            Assert.AreEqual(DateTime.MinValue, value.MyDate);  
        } 
    }
}
