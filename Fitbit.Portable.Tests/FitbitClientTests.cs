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
    }
}