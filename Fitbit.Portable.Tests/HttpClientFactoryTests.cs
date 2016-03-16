using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using Fitbit.Portable.Tests.Helpers;
using Fitbit.Portable.Tests.Interceptors;
using FluentAssertions;
using FluentAssertions.Common;
using Moq;
using NUnit.Core;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Fitbit.Portable.Tests
{
    using SUT = Fitbit.Api.Portable.Helpers.HttpClientFactory;

    [TestFixture]
    public class HttpClientFactoryTests
    {
        private Fixture fixture;
        private ResponseFaker successResponseFaker;
        private string expectedFakeContent = "Open source rocks";

        public HttpClientFactoryTests()
        {
            fixture = new Fixture();
            
            successResponseFaker = new ResponseFaker(expectedFakeContent);
        }

        [Test]
        [Category("HttpClient Factory")]
        public async Task Correctly_creates_client_with_single_fitbit_interceptor()
        {
            var vanillaClient = fixture.Create<FitbitClient>();
            var vanillaUri = new Uri(@"http://www.bing.com");

            var r = SUT.Create(vanillaClient, successResponseFaker);

            var actualContent = (await r.GetAsync(vanillaUri)).Content.ReadAsStringAsync();

            Assert.NotNull(r);
            Assert.AreEqual(expectedFakeContent, await actualContent);
        }

        [Test]
        [Category("HttpClient Factory")]
        public async Task Correctly_creates_client_with_multiple_fitbit_interceptor()
        {
            var vanillaClient = fixture.Create<FitbitClient>();
            var interceptorCounter = new InterceptorCounter();

            var interceptorList = new List<IFitbitInterceptor>();

            //Order matters. Interceptors do a LIFO execution order on the request pipelin and a FIFO on te response pipeline
            interceptorList.Add(successResponseFaker);
            interceptorList.Add(interceptorCounter);

            var vanillaUri = new Uri(@"http://www.bing.com");

            var r = SUT.Create(vanillaClient, interceptorList);

            var actualContent = (await r.GetAsync(vanillaUri)).Content.ReadAsStringAsync();

            Assert.NotNull(r);
            Assert.AreEqual(expectedFakeContent, await actualContent);

            //Ensure second interceptor is invoked
            interceptorCounter.RequestCount.Should().Be(1);
        }

        [Test]
        [Category("HttpClient Factory")]
        public async Task Correctly_creates_client_when_interceptor_list_is_empty()
        {
            var vanillaClient = fixture.Create<FitbitClient>();
            var emptyList = new List<IFitbitInterceptor>();

            var vanillaUri = new Uri(@"http://www.bing.com");

            var r = SUT.Create(vanillaClient, emptyList);


            Assert.NotNull(r);
        }
    }
}
