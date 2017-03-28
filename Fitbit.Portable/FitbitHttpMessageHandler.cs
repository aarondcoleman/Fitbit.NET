namespace Fitbit.Api.Portable
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    internal class FitbitHttpMessageHandler : DelegatingHandler
    {
        private IFitbitInterceptor interceptor;
        Func<Task<HttpResponseMessage>, CancellationToken, Task<HttpResponseMessage>> responseHandler;


        public FitbitClient FitbitClient { get; private set; }

        public FitbitHttpMessageHandler(FitbitClient fitbitClient, IFitbitInterceptor interceptor)
        {
            this.FitbitClient = fitbitClient;
            this.interceptor = interceptor;
            responseHandler = ResponseHandler;
            //Define the inner must handler. Otherwise exception is thrown.
            InnerHandler = new HttpClientHandler();
        }

        //Handle the following method with EXTREME care as it will be invoked on ALL requests made by FitbitClient
        // We override the SendAsync method to intercept both the request and response path
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Task<HttpResponseMessage> interceptorFakeResponse = null;
            Debug.WriteLine("Entering Http client's request message handler. Request details: {0}", request.ToString());

            if (interceptor != null)
                interceptorFakeResponse = interceptor.InterceptRequest(request, cancellationToken, FitbitClient);

            if (interceptorFakeResponse != null) //then highjack the request pipeline and return the HttpResponse returned by interceptor. Invoke Response handler at return.
            {
                //If we are faking the response, have the courtesy of setting the original HttpRequestMessage
                interceptorFakeResponse.Result.RequestMessage = request;
                return interceptorFakeResponse.ContinueWith(
                        responseTask => ResponseHandler(responseTask, cancellationToken).Result
                    );
            }
            else //Let the base object continue with the request pipeline. Invoke Response handler at return.
            {
                return base.SendAsync(request, cancellationToken).ContinueWith(
                     responseTask => ResponseHandler(responseTask, cancellationToken).Result
                 );
            }
        }

        //Handle the following method with EXTREME care as it will be invoked on ALL responses made by FitbitClient
        private async Task<HttpResponseMessage> ResponseHandler(Task<HttpResponseMessage> responseTask, CancellationToken cancellationToken)
        {
            DebugLogResponse(responseTask);

            if (interceptor != null)
            {
                var interceptorFakeResponse = await interceptor.InterceptResponse(responseTask, cancellationToken, FitbitClient);

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
            string responseContent = null;

            if (requestTask.Result.Content != null)
                responseContent = requestTask.Result.Content.ReadAsStringAsync().Result;

            Debug.WriteLine("Entering Http client's response message handler. Response details: \n {0}", requestTask.Result);
            Debug.WriteLine("Response Content: \n {0}", responseContent ?? "Response body was empty");
        }
    }
}
