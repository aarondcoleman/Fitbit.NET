using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public class FitbitResponse<T> where T : class
    {
        public T Data { get; set; }

        public List<ApiError> Errors { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        public int RetryAfterInSeconds { get; private set; }

        public bool Success
        {
            get
            {
                bool success = !Errors.Any();

                if (success && !new[] {HttpStatusCode.OK, HttpStatusCode.Created}.Contains(StatusCode))
                {
                    success = false;
                }

                return success;
            }
        }

        public FitbitResponse(HttpStatusCode httpStatusCode, int retryAfterInSeconds, List<ApiError> errors)
        {
            StatusCode = httpStatusCode;
            RetryAfterInSeconds = retryAfterInSeconds;
            Errors = errors ?? new List<ApiError>();
        }
    }
}