using System.Net;
using System.Net.Http;
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

        [Test]
        public void RetryAfter_NoRetryHeader()
        {
            var headers = new HttpResponseMessage().Headers;
            var response = new FitbitResponse<string>((HttpStatusCode)429, headers, null);
            Assert.AreEqual(0, response.RetryAfter());
        }

        [Test]
        public void RetryAfter_RetryHeader_NoValue()
        {
            var headers = new HttpResponseMessage().Headers;
            headers.Add("Retry-After", new string[0]);
            var response = new FitbitResponse<string>((HttpStatusCode)429, headers, null);
            Assert.AreEqual(0, response.RetryAfter());
        }

        [Test]
        public void RetryAfter_RetryHeader_100()
        {
            var headers = new HttpResponseMessage().Headers;
            headers.Add("Retry-After", "100");
            var response = new FitbitResponse<string>((HttpStatusCode)429, headers, null);
            Assert.AreEqual(100, response.RetryAfter());
        }
    }
}
