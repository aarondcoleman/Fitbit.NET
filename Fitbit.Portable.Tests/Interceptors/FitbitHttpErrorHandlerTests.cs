using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.Interceptors;
using NUnit.Core;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Fitbit.Portable.Tests.Interceptors
{
    [TestFixture]
    public class FitbitHttpErrorHandlerTests
    {
        Fixture fixture = new Fixture();

        public FitbitHttpErrorHandlerTests()
        {
            fixture = new Fixture();

            fixture.Customizations.Add(new TypeRelay(typeof(HttpContent), typeof(ByteArrayContent)));
        }

        //TO DO: Migrate to newer framework to move away from atribute base exception checking
        [Test]
        [ExpectedException(typeof(FitbitRequestException))]
        public async void Throws_On_500()
        {
            var cancellationToken = new CancellationToken();
            var unsuccesfulResponse =
                fixture.Build<HttpResponseMessage>().With(r => r.StatusCode, HttpStatusCode.InternalServerError).Create();
            var sut = fixture.Create<FitbitHttpErrorHandler>();

            await sut.InterceptResponse(Task.FromResult(unsuccesfulResponse), cancellationToken, null);
        }

        //TO DO: Migrate to newer framework to move away from atribute base exception checking
        [Test]
        public async void Proceeds_On_200()
        {
            var cancellationToken = new CancellationToken();
            var unsuccesfulResponse =
                fixture.Build<HttpResponseMessage>().With(r => r.StatusCode, HttpStatusCode.OK).Create();
            var sut = fixture.Create<FitbitHttpErrorHandler>();

            var result = await sut.InterceptResponse(Task.FromResult(unsuccesfulResponse), cancellationToken, null);

            //A null response means the interceptor is letting the pipeline continue its normal flow.
            Assert.IsNull(result);
        }
    }
}
