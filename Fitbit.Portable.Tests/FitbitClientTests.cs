using System;
using Fitbit.Api.Portable;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class FitbitClientTests
    {
        [Test]
        public void ToFullUrl_No_Preceeding_Slash()
        {
            var apiCall = "1/user/-/devices.json".ToFullUrl();
            
            Assert.IsNotNull(apiCall);
            Assert.IsFalse(string.IsNullOrWhiteSpace(apiCall));
            Assert.AreEqual("https://api.fitbit.com/1/user/-/devices.json", apiCall);
        }

        [Test]
        public void ToFullUrl_Preceeding_Slash()
        {
            var apiCall = "/1/user/-/devices.json".ToFullUrl();

            Assert.IsNotNull(apiCall);
            Assert.IsFalse(string.IsNullOrWhiteSpace(apiCall));
            Assert.AreEqual("https://api.fitbit.com/1/user/-/devices.json", apiCall);
        }

        [Test]
        public void ToFullUrl_No_Preceeding_Slash_UserId_NotSpecified()
        {
            var apiCall = "1/user/{0}/friends.json".ToFullUrl();

            Assert.IsNotNull(apiCall);
            Assert.IsFalse(string.IsNullOrWhiteSpace(apiCall));
            Assert.AreEqual("https://api.fitbit.com/1/user/-/friends.json", apiCall);
        }

        [Test]
        public void ToFullUrl_Preceeding_Slash_UserId_NotSpecified()
        {
            var apiCall = "/1/user/{0}/friends.json".ToFullUrl();

            Assert.IsNotNull(apiCall);
            Assert.IsFalse(string.IsNullOrWhiteSpace(apiCall));
            Assert.AreEqual("https://api.fitbit.com/1/user/-/friends.json", apiCall);
        }

        [Test]
        public void ToFullUrl_No_Preceeding_Slash_UserId_Specified()
        {
            var apiCall = "1/user/{0}/friends.json".ToFullUrl("2KNXXX");

            Assert.IsNotNull(apiCall);
            Assert.IsFalse(string.IsNullOrWhiteSpace(apiCall));
            Assert.AreEqual("https://api.fitbit.com/1/user/2KNXXX/friends.json", apiCall);
        }

        [Test]
        public void ToFullUrl_Preceeding_Slash_UserId_Specified()
        {
            var apiCall = "/1/user/{0}/friends.json".ToFullUrl("2KNXXX");

            Assert.IsNotNull(apiCall);
            Assert.IsFalse(string.IsNullOrWhiteSpace(apiCall));
            Assert.AreEqual("https://api.fitbit.com/1/user/2KNXXX/friends.json", apiCall);
        }

        [Test]
        public void ToFitbitFormat_DateTime()
        {
            DateTime date = new DateTime(2014, 08, 21);
            Assert.AreEqual("2014-08-21", date.ToFitbitFormat());
        }

        //[Test]
        //public async void Test()
        //{
        //    var fakeResponseHandler = new FakeResponseHandler();
        //    fakeResponseHandler.AddResponse(new Uri("http://example.org/test"), new HttpResponseMessage(HttpStatusCode.OK));

        //    var httpClient = new HttpClient(fakeResponseHandler);

        //    var response1 = await httpClient.GetAsync("http://example.org/notthere");
        //    var response2 = await httpClient.GetAsync("http://example.org/test");

        //    Assert.AreEqual(response1.StatusCode, HttpStatusCode.NotFound);
        //    Assert.AreEqual(response2.StatusCode, HttpStatusCode.OK);

        //    Assert.AreEqual(2, fakeResponseHandler.CallCount);
        //}
    }
}