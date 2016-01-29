using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Fitbit.Api.Portable;
using System.Diagnostics;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class FitbitClientHttpMessageHandlerTests
    {
        [Test]
        public void CanInterceptHttpRequests()
        {
            //arrenge
            var logger = new MyCustomLogger();
            var authorizer = new OAuth2Authorization("bearertoken", "refreshtoken");
            var handler = new FitBitHttpClientMessageHandler(logger);
            var sut = new FitbitClient(authorizer, null, logger);

            //Act
            var r = sut.HttpClient.GetAsync("https://dev.fitbit.com/");

            r.Wait();

            //Assert
            Assert.AreEqual(1, logger.RequestCount);
            Assert.AreEqual(1, logger.ResponseCount);
        }

        

        public class MyCustomLogger : IFitbitRequestInterceptor
        {
            public int RequestCount = 0;
            public int ResponseCount = 0;

            public void Request(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                RequestCount++;
            }

            public void Response(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                ResponseCount++;
            }
        }
    }
}
