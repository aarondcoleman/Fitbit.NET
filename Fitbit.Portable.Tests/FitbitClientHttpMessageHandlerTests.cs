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
            var logger = new MyCustomLogger();

            var handler = new FitBitHttpClientMessageHandler(logger);

            //var fc = new FitbitClient(logger);

            var c = new HttpClient(handler);

            var r = c.GetAsync("http://localhost:1531/");

            r.Wait();

            Assert.AreEqual(1, logger.RequestCount);
            Assert.AreEqual(1, logger.ResponseCount);
        }

        

        public class MyCustomLogger : IFItbitRequestInterceptor
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
