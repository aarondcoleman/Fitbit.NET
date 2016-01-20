using System;
using System.Net;
using System.Net.Http;
using Fitbit.Api.Portable;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class FitbitResponseTests
    {
        [Test] [Category("Portable")]
        public void RetryAfter_No_Status_Applicable()
        {
            var response = new FitbitResponse<string>(HttpStatusCode.OK, null, null);
            Assert.AreEqual(0, response.RetryAfter());
        }

        [Test] [Category("Portable")]
        public void RetryAfter_NoRetryHeader()
        {
            var headers = new HttpResponseMessage().Headers;
            var response = new FitbitResponse<string>((HttpStatusCode)429, headers, null);
            Assert.AreEqual(0, response.RetryAfter());
        }

        [Test] [Category("Portable")]
        public void RetryAfter_RetryHeader_NoValue()
        {
            var headers = new HttpResponseMessage().Headers;
            headers.Add("Retry-After", new string[0]);
            var response = new FitbitResponse<string>((HttpStatusCode)429, headers, null);
            Assert.AreEqual(0, response.RetryAfter());
        }

        [Test] [Category("Portable")]
        public void RetryAfter_RetryHeader_100()
        {
            var headers = new HttpResponseMessage().Headers;
            headers.Add("Retry-After", "100");
            var response = new FitbitResponse<string>((HttpStatusCode)429, headers, null);
            Assert.AreEqual(100, response.RetryAfter());
        }

        //status tests - https://wiki.fitbit.com/display/API/API+Response+Format+And+Errors
        [Test] [Category("Portable")]
        public void NoContent()
        {
            var response = new FitbitResponse<NoData>(HttpStatusCode.NoContent);
            Assert.IsTrue(response.Success);
        }

        [Test] [Category("Portable")]
        public void Created()
        {
            var response = new FitbitResponse<NoData>(HttpStatusCode.Created);
            Assert.IsTrue(response.Success);
        }

        [Test] [Category("Portable")]
        public void Ok()
        {
            var response = new FitbitResponse<NoData>(HttpStatusCode.OK);
            Assert.IsTrue(response.Success);
        }

        [Test] [Category("Portable")]
        public void AnyOther()
        {
            foreach (HttpStatusCode value in Enum.GetValues(typeof (HttpStatusCode)))
            {
                switch (value)
                {
                    case HttpStatusCode.NoContent:
                    case HttpStatusCode.Created:
                    case HttpStatusCode.OK:
                        break;

                    default:
                        var response = new FitbitResponse<NoData>(value);
                        Assert.IsFalse(response.Success);
                        break;
                }
            }
        }
    }
}
