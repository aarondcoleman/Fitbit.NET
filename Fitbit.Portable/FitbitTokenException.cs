using System.Collections.Generic;
using System.Net.Http;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public class FitbitTokenException : FitbitRequestException
    {
        public FitbitTokenException(HttpResponseMessage response, IEnumerable<ApiError> errors = null, string message = default(string))
            : base(response, errors, message ?? $"Fitbit Token exception - HTTP Status Code-- {response.StatusCode} -- see errors for more details.")
        {
        }
    }
}
