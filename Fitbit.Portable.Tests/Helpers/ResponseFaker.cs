using System.Net;

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

        public ResponseFaker(string content, HttpStatusCode code = HttpStatusCode.OK)
        {
            var c = new StringContent(content);
            this.fakeResponse = new HttpResponseMessage(code) { Content = c };
        }

        public ResponseFaker(HttpResponseMessage fakeResponse = null)
        {
            this.fakeResponse = fakeResponse;
        }

        public Task<HttpResponseMessage> InterceptRequest(HttpRequestMessage request, CancellationToken cancellationToken, FitbitClient client)
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

        public async Task<HttpResponseMessage> InterceptResponse(Task<HttpResponseMessage> response, CancellationToken cancellationToken, FitbitClient client)
        {
            ResponseCount++;
            return null;
        }

    }
}
