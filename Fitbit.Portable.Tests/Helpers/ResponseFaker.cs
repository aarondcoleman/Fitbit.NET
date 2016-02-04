namespace Fitbit.Portable.Tests.Helpers
{
    using Fitbit.Api.Portable;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class ResponseFaker : IFitbitInterceptor
    {
        public HttpResponseMessage fakeResponse = null;

        public int RequestCount = 0;
        public int ResponseCount = 0;


        public ResponseFaker(HttpResponseMessage fakeResponse = null)
        {
            this.fakeResponse = fakeResponse;
        }

        public Task<HttpResponseMessage> InterceptRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestCount++;
            if (fakeResponse != null)
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() =>
                {
                    return fakeResponse;
                });
            }
            else
                return null;
        }

        public async Task InterceptResponse(Task<HttpResponseMessage> response, CancellationToken cancellationToken)
        {
            ResponseCount++;
        }

    }
}
