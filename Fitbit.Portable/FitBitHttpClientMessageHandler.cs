namespace Fitbit.Api.Portable
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    internal class FitbitHttpClientMessageHandler : DelegatingHandler
    {
        private IFitbitInterceptor interceptor;
        Func<Task<HttpResponseMessage>, CancellationToken, Task<HttpResponseMessage>> responseHandler;

        //private ITokenManager tokenManager;

        public FitbitHttpClientMessageHandler(IFitbitInterceptor interceptor)
        {
            this.interceptor = interceptor;
            //this.tokenManager = tokenManager;
            responseHandler = ResponseHandler; 
            //Define the inner must handler. Otherwise exception is thrown.
            InnerHandler = new HttpClientHandler();
        }

        //Handle the following method with EXTREME care as it will be invoked on ALL requests made by FitbitClient
        // We override the SendAsync method to intercept both the request and response path
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Task<HttpResponseMessage> interceptorResponse = null;
            Debug.WriteLine("Entering Http client's request message handler. Request details: {0}", request.ToString());

            if (interceptor != null)
                interceptorResponse = interceptor.InterceptRequest(request, cancellationToken);

            if(interceptorResponse != null) //then highjack the request pipeline and return the HttpResponse returned by interceptor. Invoke Response handler at return.
            {
                return interceptorResponse.ContinueWith(
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
            Debug.WriteLine("Entering Http client's response message handler. Response details: {0}", responseTask.Result);
            if (interceptor != null)
                await interceptor.InterceptResponse(responseTask, cancellationToken);

            return responseTask.Result;
        }
    }
}
