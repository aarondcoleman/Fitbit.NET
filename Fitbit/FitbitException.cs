using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Fitbit.Api
{
    /// <summary>
    /// Custom exception class to be used to gather response data from the API when there's an error
    /// </summary>
    public class FitbitException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public IList<Fitbit.Models.ApiError> ApiErrors { get; set; }

        /// <summary>
        /// Number of seconds until the request can be retried - not null if provided by fitbit
        /// </summary>
        public int? retryAfter { get; set; }

        public FitbitException(string message, HttpStatusCode statusCode)
            : this(message, statusCode, new List<Fitbit.Models.ApiError>())
        {
        }

        public FitbitException(string message, HttpStatusCode statusCode, IList<Fitbit.Models.ApiError> apiErrors)
            : base(message)
        {
            this.HttpStatusCode = statusCode;
            this.ApiErrors = apiErrors;
        }

        public bool containsRateError
        {
            get
            {
                return 429 == (int)HttpStatusCode;
            }
        }
    }
}
