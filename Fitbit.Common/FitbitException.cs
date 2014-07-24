using System;
using System.Collections.Generic;
using System.Net;

namespace Fitbit.Api
{
    /// <summary>
    /// Custom exception class to be used to gather response data from the API when there's an error
    /// </summary>
    public class FitbitException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; private set; }

        public IList<Models.ApiError> ApiErrors { get; private set; }

        /// <summary>
        /// Number of seconds until the request can be retried - not null if provided by fitbit
        /// </summary>
        public int? RetryAfter { get; set; }

        public FitbitException(string message, HttpStatusCode statusCode) : this(message, statusCode, new List<Models.ApiError>())
        {
        }

        public FitbitException(string message, HttpStatusCode statusCode, IList<Models.ApiError> apiErrors) : base(message)
        {
            HttpStatusCode = statusCode;
            ApiErrors = apiErrors;
        }

        public bool ContainsRateError
        {
            get
            {
                return 429 == (int)HttpStatusCode;
            }
        }
    }
}