namespace Fitbit.Api.Portable
{
    using System.Net.Http;
    using System.Threading;

    public interface IFitbitClientInterceptor
    {
        void InterceptRequest(HttpRequestMessage request, CancellationToken cancellationToken);
        void InterceptResponse(HttpResponseMessage response, CancellationToken cancellationToken);
    }
}
