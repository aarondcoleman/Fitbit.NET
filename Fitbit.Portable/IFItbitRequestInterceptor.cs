namespace Fitbit.Api.Portable
{
    using System.Net.Http;
    using System.Threading;

    public interface IFitbitRequestInterceptor
    {
        void Request(HttpRequestMessage request, CancellationToken cancellationToken);
        void Response(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}
