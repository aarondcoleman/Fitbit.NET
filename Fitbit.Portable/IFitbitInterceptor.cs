namespace Fitbit.Api.Portable
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    public interface IFitbitInterceptor
    {
        Task<HttpResponseMessage> InterceptRequest(HttpRequestMessage request, CancellationToken cancellationToken, FitbitClient invokingClient);
        Task<HttpResponseMessage> InterceptResponse(Task<HttpResponseMessage> response, CancellationToken cancellationToken, FitbitClient invokingClient);
    }
}
