using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Fitbit.Api.Portable
{
    public class FitbitClient : IFitbitClient
    {
        //private async void HandleResponse(HttpResponseMessage response)
        //{
        //    if (response.IsSuccessStatusCode)
        //        return;

        //    IList<ApiError> errors = null;
        //    try
        //    {
        //        var serializer = new JsonDotNetSerializer();
        //        serializer.RootProperty = "errors";
        //        errors = serializer.Deserialize<List<ApiError>>(await response.Content.ReadAsStringAsync());
        //    }
        //    finally
        //    {
        //        // If there's an issue deserializing the error we still want to raise a fitbit exception so
        //        // whatever happens we need an instantiated error list to create a FitbitException with
        //        errors = errors ?? new List<ApiError>();
        //    }

        //    HttpStatusCode httpStatusCode = response.StatusCode;

        //    FitbitException exception = new FitbitException("Http Error:" + httpStatusCode, httpStatusCode, errors);

        //    var retryAfterHeader = response.Headers.RetryAfter;
        //    if (retryAfterHeader != null)
        //    {
        //        if (retryAfterHeader.Delta.HasValue)
        //        {
        //            exception.RetryAfter = retryAfterHeader.Delta.Value.Seconds;
        //        }
        //    }

        //    throw exception;
        //}
    }
}
