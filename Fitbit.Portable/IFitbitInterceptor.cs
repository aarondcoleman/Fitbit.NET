namespace Fitbit.Api.Portable
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    public interface IFitbitInterceptor
    {
        Task<HttpResponseMessage> InterceptRequest(HttpRequestMessage request, CancellationToken cancellationToken);
        void InterceptResponse(HttpResponseMessage response, CancellationToken cancellationToken);
    }
}
