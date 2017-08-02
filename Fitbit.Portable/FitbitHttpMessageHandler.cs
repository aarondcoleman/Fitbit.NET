namespace Fitbit.Api.Portable
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    internal class FitbitHttpMessageHandler : DelegatingHandler
    {
        private readonly IFitbitInterceptor _interceptor;

        public FitbitClient FitbitClient { get; }

        public FitbitHttpMessageHandler(FitbitClient fitbitClient, IFitbitInterceptor interceptor)
        {
            FitbitClient = fitbitClient;
            _interceptor = interceptor;
            //Define the inner must handler. Otherwise exception is thrown.
            InnerHandler = new HttpClientHandler();
        }

        //Handle the following method with EXTREME care as it will be invoked on ALL requests made by FitbitClient
        // We override the SendAsync method to intercept both the request and response path
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Task<HttpResponseMessage> interceptorFakeResponse = null;
            Debug.WriteLine("Entering Http client's request message handler. Request details: {0}", request.ToString());

            if (_interceptor != null)
            {
                interceptorFakeResponse = _interceptor.InterceptRequest(request, cancellationToken, FitbitClient);
            }

            if (interceptorFakeResponse != null) //then highjack the request pipeline and return the HttpResponse returned by interceptor. Invoke Response handler at return.
            {
                //If we are faking the response, have the courtesy of setting the original HttpRequestMessage
                interceptorFakeResponse.Result.RequestMessage = request;
                return interceptorFakeResponse.ContinueWith(responseTask => ResponseHandler(responseTask, cancellationToken).Result, cancellationToken);
            }
            
            //Let the base object continue with the request pipeline. Invoke Response handler at return.
            return base.SendAsync(request, cancellationToken).ContinueWith(responseTask => ResponseHandler(responseTask, cancellationToken).Result, cancellationToken);
        }

        //Handle the following method with EXTREME care as it will be invoked on ALL responses made by FitbitClient
        private async Task<HttpResponseMessage> ResponseHandler(Task<HttpResponseMessage> responseTask, CancellationToken cancellationToken)
        {
            DebugLogResponse(responseTask);

            if (_interceptor != null)
            {
                var interceptorFakeResponse = await _interceptor.InterceptResponse(responseTask, cancellationToken, FitbitClient);

                if (interceptorFakeResponse != null) //then highjack the request pipeline and return the HttpResponse returned by interceptor. Invoke Response handler at return.
                {
                    //If we are faking the response, have the courtesy of setting the original HttpRequestMessage
                    interceptorFakeResponse.RequestMessage = (await responseTask).RequestMessage;
                    return interceptorFakeResponse;
                }
            }

            return await responseTask;
        }

        [Conditional("DEBUG")]
        private static void DebugLogResponse(Task<HttpResponseMessage> requestTask)
        {
            var responseContent = requestTask?.Result?.Content?.ReadAsStringAsync().Result;

            Debug.WriteLine("Entering Http client's response message handler. Response details: \n {0}", requestTask?.Result);
            Debug.WriteLine("Response Content: \n {0}", responseContent ?? "Response body was empty");
        }
    }
}
