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

        public FitbitException(string message, HttpStatusCode statusCode) : base(message)
        {
            this.HttpStatusCode = statusCode;
        }
    }
}
