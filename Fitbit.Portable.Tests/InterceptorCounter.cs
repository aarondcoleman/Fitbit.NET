using Fitbit.Api.Portable;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fitbit.Portable.Tests
{
    public class InterceptorCounter : IFitbitInterceptor
    {
        public int RequestCount = 0;
        public int ResponseCount = 0;

        public string responseContent;

        public Task<HttpResponseMessage> InterceptRequest(HttpRequestMessage request, CancellationToken cancellationToken, FitbitClient client)
        {
            RequestCount++;
            return null;
        }

        public async Task<HttpResponseMessage> InterceptResponse(Task<HttpResponseMessage> response, CancellationToken cancellationToken, FitbitClient client)
        {
            ResponseCount++;
            this.responseContent = await response.Result.Content.ReadAsStringAsync();
            return null;
        }
    }
}
