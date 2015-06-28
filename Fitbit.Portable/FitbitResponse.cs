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

        internal HttpStatusCode StatusCode { get; private set; }

        internal HttpHeaders HttpHeaders { get; private set; }

        public bool Success
        {
            get
            {
                bool success = !Errors.Any();

                if (success && !new[] {HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.NoContent}.Contains(StatusCode))
                {
                    success = false;
                }

                return success;
            }
        }

        public FitbitResponse(HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, HttpHeaders httpHeaders = null, List<ApiError> errors = null)
        {
            StatusCode = httpStatusCode;
            HttpHeaders = httpHeaders;
            Errors = errors ?? new List<ApiError>();
        }

        public FitbitResponse()
        {
            //not to be used, but good for mocking in client code
        }
    }
}