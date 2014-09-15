using System;
using System.Security.AccessControl;
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

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ConsumerKey_Empty()
        {
            new FitbitClient(string.Empty, "secret", "access", "accessSecret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ConsumerKey_Null()
        {
            new FitbitClient(null, "secret", "access", "accessSecret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ConsumerSecret_Empty()
        {
            new FitbitClient("key", string.Empty, "access", "accessSecret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ConsumerSecret_Null()
        {
            new FitbitClient("key", null, "access", "accessSecret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AccesToken_Empty()
        {
            new FitbitClient("key", "secret", string.Empty, "accessSecret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AccessToken_Null()
        {
            new FitbitClient("key", "secret", null, "accessSecret");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AccessSecret_Empty()
        {
            new FitbitClient("key", "secret", "access", null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_AccessSecret_Null()
        {
            new FitbitClient("key", "secret", "access", string.Empty);
        }

        [Test]
        public void Constructor_HttpClient_Create()
        {
            var client = new FitbitClient("key", "secret", "access", "accessToken");
            Assert.IsNotNull(client.HttpClient);
        }
    }
}