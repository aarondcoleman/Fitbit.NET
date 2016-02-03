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
            var customFactory = new CustomHttpFactory();

            Assert.Throws<NotImplementedException>(() => new FitbitClient(customFactory));
        }

        public class CustomHttpFactory : IFitbitHttpClientFactory
        {
            public HttpClient Create(HttpMessageHandler handler = null)
            {
                throw new NotImplementedException();
            }
        }
    }
}
