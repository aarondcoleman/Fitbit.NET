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
        Func<Task<HttpResponseMessage>, CancellationToken, HttpResponseMessage> responseHandler;

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
                        requestTask => ResponseHandler(requestTask, cancellationToken)
                    );
            }
            else //Let the base object continue with the request pipeline. Invoke Response handler at return.
            {
                return base.SendAsync(request, cancellationToken).ContinueWith(
                     requestTask => ResponseHandler(requestTask, cancellationToken)
                 );
            }
        }

        //Handle the following method with EXTREME care as it will be invoked on ALL responses made by FitbitClient
        private HttpResponseMessage ResponseHandler(Task<HttpResponseMessage> requestTask, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Entering Http client's response message handler. Response details: {0}", requestTask.Result);
            if (interceptor != null)
                interceptor.InterceptResponse(requestTask.Result, cancellationToken);

            return requestTask.Result;
        }
    }
}
