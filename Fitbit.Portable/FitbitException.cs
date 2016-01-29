using System;
using System.Collections.Generic;
using System.Net;

namespace Fitbit.Api.Portable
{
    public class FitbitException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }

        public List<Models.ApiError> ApiErrors { get; set; }

        public FitbitException(string message, HttpStatusCode statusCode) : base(message)
        {
            HttpStatusCode = statusCode;
        }
    }
}
