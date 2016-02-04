namespace Fitbit.Portable.Tests
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Threading;
    using Fitbit.Api.Portable;
    using NUnit.Framework;
    using Fitbit.Api.Portable.OAuth2;
    using Fitbit.Portable.Tests.Helpers;

    [TestFixture]
    public class FitbitClientHttpMessageHandlerTests
    {
        FitbitAppCredentials dummyCredentials = new FitbitAppCredentials();
        OAuth2AccessToken dummyToken = new OAuth2AccessToken();

        [Test]
        [Category("Portable")]
        [Category("Interceptor")]

        public void CanSnifftHttpRequests()
        {
            //arrenge
            var messageHandler = new InterceptorCounter();
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
            var messageHandler = new InterceptorCounter();
            var sut = new FitbitClient(dummyCredentials, dummyToken, messageHandler);

            //Act
            var r = sut.HttpClient.GetAsync("https://dev.fitbit.com/");
            r.Wait();
            var responseContent = r.Result.Content.ReadAsStringAsync().Result;

            //Assert
            Assert.AreEqual(messageHandler.responseContent, responseContent);
        }

        [Test]
        [Category("Portable")]
        [Category("Interceptor")]
        public void CanInterceptHttpRequestAndFakeResponse()
        {
            const int EXPECT_ONE_REQUEST = 1;
            var fakeResponse = new HttpResponseMessage(System.Net.HttpStatusCode.Unused);
            var responseFaker = new ResponseFaker(fakeResponse);

            //arrenge
            var sut = new FitbitClient(dummyCredentials, dummyToken, responseFaker);

            //Act
            var r = sut.HttpClient.GetAsync("https://dev.fitbit.com/");
            r.Wait();
            var actualResponse = r.Result;

            Assert.AreSame(fakeResponse, actualResponse);
            //Ensure that the response handler is still invoked, even though we short circuited the request 
            Assert.AreEqual(EXPECT_ONE_REQUEST, responseFaker.ResponseCount);
        }


        public class InterceptorCounter : IFitbitInterceptor
        {
            public int RequestCount = 0;
            public int ResponseCount = 0;

            public string responseContent;

            public Task<HttpResponseMessage> InterceptRequest(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                RequestCount++;
                return null;
            }

            public async Task InterceptResponse(Task<HttpResponseMessage> response, CancellationToken cancellationToken)
            {
                ResponseCount++;
                this.responseContent = await response.Result.Content.ReadAsStringAsync();
            }
        }

    }
}
