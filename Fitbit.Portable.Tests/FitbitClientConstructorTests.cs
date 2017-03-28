using Fitbit.Api.Portable;
using Fitbit.Api.Portable.OAuth2;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class FitbitClientConstructorTests
    {
        [Test]
        [Category("constructor")]
        public void Most_Basic()
        {
            var credentials = new FitbitAppCredentials() { ClientId = "SomeID", ClientSecret = "THE Secret ;)" };
            var accessToken = new OAuth2AccessToken() { Token = "", TokenType = "", ExpiresIn = 3600, RefreshToken = ""};

            var sut = new FitbitClient(credentials, accessToken);

            Assert.IsNotNull(sut.HttpClient);
        }

        [Test]
        [Category("constructor")]
        public void Use_Custom_HttpClient_Factory()
        {
            var sut = new FitbitClient(mh => { return new HttpClient(); });

            Assert.IsNotNull(sut.HttpClient);
        }

        [Test]
        [Category("constructor")]
        public void Can_Instantiate_Without_Any_Interceptors()
        {
            var credentials = new FitbitAppCredentials() { ClientId = "SomeID", ClientSecret = "THE Secret ;)" };
            var accessToken = new OAuth2AccessToken() { Token = "", TokenType = "", ExpiresIn = 3600, RefreshToken = "" };

            //Ensure not even the auto-token-refresh interceptor is active
            var sut = new FitbitClient(credentials, accessToken, false);

            Assert.IsNotNull(sut.HttpClient);
        }

        [Test]
        [Category("constructor")]
        public void Can_Use_Interceptors_Without_Autorefresh()
        {
            var credentials = new FitbitAppCredentials() { ClientId = "SomeID", ClientSecret = "THE Secret ;)" };
            var accessToken = new OAuth2AccessToken() { Token = "", TokenType = "", ExpiresIn = 3600, RefreshToken = "" };

            //Registere an interceptor, but disable the auto-token-refresh interceptor
            var sut = new FitbitClient(credentials, accessToken, new InterceptorCounter(), false);

            Assert.IsNotNull(sut.HttpClient);
        }
    }
}
