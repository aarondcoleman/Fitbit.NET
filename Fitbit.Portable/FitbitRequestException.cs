using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Fitbit.Models;
using System;

namespace Fitbit.Api.Portable
{
    public class FitbitRequestException : FitbitException
    {
        public HttpResponseMessage Response { get; set; }

        public FitbitRequestException(HttpResponseMessage response, IEnumerable<ApiError> errors, string message = default(string), Exception innerEx = null)
            : base(message ?? $"Fitbit Request exception - Http Status Code: {response.StatusCode} - see errors for more details.", errors, innerEx)
        {
            this.Response = response;
        }
    }
}