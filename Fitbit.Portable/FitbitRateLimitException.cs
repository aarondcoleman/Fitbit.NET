using System;
using System.Net;

namespace Fitbit.Api.Portable
{
    public class FitbitRateLimitException : FitbitException
    {
        /// <summary>
        /// The DateTime, in UTC, to retry after from when the exception is created.
        /// </summary>
        public DateTime RetryAfter { get; }

        public FitbitRateLimitException(int retryAfterSeconds) 
            : base($"Rate limit exceeded. Try again in {retryAfterSeconds} seconds. See exception for details.", (HttpStatusCode)429)
        {
            RetryAfter = DateTime.UtcNow.AddSeconds(retryAfterSeconds);
        }
    }
}