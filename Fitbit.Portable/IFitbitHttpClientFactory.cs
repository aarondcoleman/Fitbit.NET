
namespace Fitbit.Api.Portable
{
    using System.Net.Http;

    public interface IFitbitHttpClientFactory
    {
        HttpClient Create(HttpMessageHandler handler = null);
    }
}