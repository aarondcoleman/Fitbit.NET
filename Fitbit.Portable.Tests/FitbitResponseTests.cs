using System.Net;
using Fitbit.Api.Portable;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class FitbitResponseTests
    {
        [Test]
        public void RetryAfter_No_Status_Applicable()
        {
            var response = new FitbitResponse<string>(HttpStatusCode.OK, null, null);
            Assert.AreEqual(0, response.RetryAfter());
        }
    }
}
