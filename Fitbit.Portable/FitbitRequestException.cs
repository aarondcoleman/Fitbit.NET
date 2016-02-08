using System.Net;

namespace Fitbit.Api.Portable
{
    public class FitbitRequestException : FitbitException
    {
        public FitbitRequestException(HttpStatusCode statusCode) : base("Request exception - see errors for more details.", statusCode)
        {
        }
    }
}