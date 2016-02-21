using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.OAuth1Migration.Tests
{
    [TestFixture]
    public class AuthenticatorTests
    {
        [Test] [Category("Portable")]
        public void Constructor()
        {
            var authenticator = new Authenticator("key", "secret");
            Assert.IsNotNull(authenticator);
            Assert.AreEqual("key", authenticator.ConsumerKey);
            Assert.AreEqual("secret", authenticator.ConsumerSecret);
        }

        [Test] [Category("Portable")]
        public void Generate_Auth_Url_ForceLogout_True()
        {
            var authenticator = new Authenticator("key", "secret");
            var url = authenticator.GenerateAuthUrlFromRequestToken(new RequestToken {Token = "something"}, true);
            Assert.AreEqual("https://api.fitbit.com/oauth/logout_and_authorize?oauth_token=something", url);
        }

        [Test] [Category("Portable")]
        public void Generate_Auth_Url_ForceLogout_False()
        {
            var authenticator = new Authenticator("key", "secret");
            var url = authenticator.GenerateAuthUrlFromRequestToken(new RequestToken { Token = "something" }, false);
            Assert.AreEqual("https://api.fitbit.com/oauth/authorize?oauth_token=something", url);
        }
    }
}
