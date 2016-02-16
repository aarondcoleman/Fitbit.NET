using System;
using System.Collections.Generic;
using Fitbit.Api.Portable.Models;

namespace Fitbit.Api.Portable
{
    public class FitbitException : Exception
    {
        public List<ApiError> ApiErrors { get; set; }

        public FitbitException(string message, IEnumerable<ApiError> errors) : base(message)
        {
            ApiErrors = errors != null ? new List<ApiError>(errors) : new List<ApiError>();
        }
    }
}