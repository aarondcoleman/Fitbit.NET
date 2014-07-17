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
    public class ApiErrorTests
    {
        [Test]
        public void Can_Deserialize_ApiError()
        {
            string content = File.ReadAllText(SampleData.PathFor("ApiError.txt"));

            var deserializer = new JsonDotNetSerializer();
            deserializer.RootProperty = "errors";
            List<ApiError> result = deserializer.Deserialize<List<ApiError>>(content);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
            ApiError error = result.First();
            Assert.AreEqual(ErrorType.Request, error.ErrorType);
            Assert.AreEqual("n/a", error.FieldName);
        }
    }
}