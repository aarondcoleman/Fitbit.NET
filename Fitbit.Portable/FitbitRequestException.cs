using System.Collections.Generic;
using System.Net;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public class FitbitRequestException : FitbitException
    {
        public FitbitRequestException(HttpStatusCode statusCode, IEnumerable<ApiError> errors)
            : base($"Request exception - see errors for more details - {statusCode}", errors)
        {
        }
    }
}