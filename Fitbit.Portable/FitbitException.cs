using System;
using System.Collections.Generic;
using System.Net;
using Fitbit.Api.Portable.Models;

namespace Fitbit.Api.Portable
{
    public class FitbitException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }

        public List<ApiError> ApiErrors { get; set; }

        public FitbitException(string message, HttpStatusCode statusCode) : base(message)
        {
            HttpStatusCode = statusCode;
        }
    }
}
