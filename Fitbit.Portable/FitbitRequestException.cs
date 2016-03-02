using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public class FitbitRequestException : FitbitException
    {
        public HttpResponseMessage Response { get; set; }

        public FitbitRequestException(HttpResponseMessage response, IEnumerable<ApiError> errors)
            : base($"Request exception - see errors for more details - {response.StatusCode}", errors)
        {
            this.Response = response;
        }
    }
}