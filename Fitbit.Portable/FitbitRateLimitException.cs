using System;
using System.Collections.Generic;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public class FitbitRateLimitException : FitbitException
    {
        /// <summary>
        /// The DateTime, in UTC, to retry after from when the exception is created.
        /// </summary>
        public DateTime RetryAfter { get; }

        public FitbitRateLimitException(int retryAfterSeconds, IEnumerable<ApiError> errors)
            : base($"Rate limit exceeded. Try again in {retryAfterSeconds} seconds. See exception for details.", errors)
        {
            RetryAfter = DateTime.UtcNow.AddSeconds(retryAfterSeconds);
        }
    }
}