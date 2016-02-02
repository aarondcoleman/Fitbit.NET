using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Fitbit.Api.Portable;
using System.Diagnostics;
using NUnit.Framework;
using Fitbit.Api.Portable.OAuth2;

namespace Fitbit.Portable.Tests
{
    [TestFixture]
    public class FitbitClientHttpMessageHandlerTests
    {
        [Test]
        [Category("Portable")]
        [Category("Interceptor")]

        public void CanInterceptHttpRequests()
        {
            //arrenge
            var messageHandler = new MyCustomLogger();

            var dummyCredentials = new FitbitAppCredentials();
            var dummyToken = new OAuth2AccessToken();

            var sut = new FitbitClient(dummyCredentials, dummyToken, messageHandler);

            //Act
            var r = sut.HttpClient.GetAsync("https://dev.fitbit.com/");
            r.Wait();

            //Assert
            Assert.AreEqual(1, messageHandler.RequestCount);
            Assert.AreEqual(1, messageHandler.ResponseCount);
        }

        [Test]
        [Category("Portable")]
        [Category("Interceptor")]
        public void CanReadResponseMultipleTimes()
        {
            //arrenge
            var messageHandler = new MyCustomLogger();

            var dummyCredentials = new FitbitAppCredentials();
            var dummyToken = new OAuth2AccessToken();

            var sut = new FitbitClient(dummyCredentials, dummyToken, messageHandler);

            //Act
            var r = sut.HttpClient.GetAsync("https://dev.fitbit.com/");

            r.Wait();

            var responseContent = r.Result.Content.ReadAsStringAsync().Result;

            //Assert
            Assert.AreEqual(messageHandler.responseContent, responseContent);
        }



        public class MyCustomLogger : IFitbitClientInterceptor
        {
            public int RequestCount = 0;
            public int ResponseCount = 0;

            public string responseContent;

            public void InterceptRequest(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                RequestCount++;
            }

            public void InterceptResponse(HttpResponseMessage response, CancellationToken cancellationToken)
            {
                ResponseCount++;
                this.responseContent = response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
