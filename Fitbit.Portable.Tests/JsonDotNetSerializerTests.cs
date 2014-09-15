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

            // todo: array etc.
        }

        [Test]
        public void DefaultValueCreated_String()
        {
            var serializer = new JsonDotNetSerializer();
            var defaultValue = serializer.Deserialize<TestClass>(string.Empty);
            Assert.AreEqual(default(TestClass), defaultValue);
        }

        [Test]
        public void DefaultValueCreated_JToken()
        {
            var serializer = new JsonDotNetSerializer();
            var defaultValue = serializer.Deserialize<TestClass>((JToken)null);
            Assert.AreEqual(default(TestClass), defaultValue);
        }

        [Test]
        public void NoRootValueCreated()
        {
            string data = "{\"testproperty\" : \"bob\" }";
            var serializer = new JsonDotNetSerializer();
            var value = serializer.Deserialize<TestClass>(data);
            Assert.IsNotNull(value);
            Assert.AreEqual("bob", value.TestProperty);
        }

        [Test]
        public void RootPropertyValueCreated()
        {
            string data = "{\"testclass\" : {\"testproperty\" : \"bob\" } }";
            var serializer = new JsonDotNetSerializer();
            serializer.RootProperty = "testclass";
            var value = serializer.Deserialize<TestClass>(data);
            Assert.IsNotNull(value);
            Assert.AreEqual("bob", value.TestProperty);
        }
    }
}
