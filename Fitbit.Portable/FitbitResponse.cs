using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using Fitbit.Models;

namespace Fitbit.Api.Portable
{
    public class FitbitResponse<T> where T : class
    {
        public T Data { get; set; }

        public List<ApiError> Errors { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        public HttpHeaders HttpHeaders { get; private set; }

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

        internal FitbitResponse(HttpStatusCode httpStatusCode, HttpHeaders httpHeaders, List<ApiError> errors)
        {
            StatusCode = httpStatusCode;
            HttpHeaders = httpHeaders;
            Errors = errors ?? new List<ApiError>();
        }
    }
}