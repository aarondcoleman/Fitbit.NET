using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fitbit.Models;

namespace Fitbit.Api.Portable.Interceptors
{
    public class FitbitHttpErrorHandler : IFitbitInterceptor
    {
        public Task<HttpResponseMessage> InterceptRequest(HttpRequestMessage request, CancellationToken cancellationToken, FitbitClient invokingClient)
        {
            return null;
        }

        public async Task<HttpResponseMessage> InterceptResponse(Task<HttpResponseMessage> response, CancellationToken cancellationToken, FitbitClient invokingClient)
        {
            if ((await response).IsSuccessStatusCode) return null;
            else
            {
                await GenerateFitbitRequestException(await response);
                return null;
            }

        }


        private async Task GenerateFitbitRequestException(HttpResponseMessage response)
        {
            List<ApiError> errors = null;

            try
            {
                // assumption is error response from fitbit in the 4xx range  
                errors = new JsonDotNetSerializer().ParseErrors(await response.Content.ReadAsStringAsync());
            }
            catch(ArgumentNullException emptyBodyException)
            {
                errors = new List<ApiError>() { {new ApiError() {ErrorType = "Fitbit.Net client library error", Message = "Error parsing content body. The content was empty"} } };
            }
            catch (Exception e)
            {
                errors = new List<ApiError>() { { new ApiError() { ErrorType = "Fitbit.Net client library error", Message = "Unexpected error when deserializing the content of Fitbit's response." } } };
            }

            var exception = new FitbitRequestException(response, errors);

            throw exception;                            
        }
    }
}
