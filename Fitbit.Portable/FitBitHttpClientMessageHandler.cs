namespace Fitbit.Api.Portable
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    internal class FitbitHttpClientMessageHandler : DelegatingHandler
    {
        private IFitbitClientInterceptor interceptor;
        //private ITokenManager tokenManager;

        public FitbitHttpClientMessageHandler(IFitbitClientInterceptor interceptor)
        {
            this.interceptor = interceptor;
            //this.tokenManager = tokenManager;
            //Define the inner must handler. Otherwise exception is thrown.
            InnerHandler = new HttpClientHandler();
        }

        //Handle the following method with EXTREME care as it will be invoked on ALL requests and responses made by FitbitClient
        // We override the SendAsync method to intercept both the request and response path
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Entering Http client's request message handler. Request details: {0}", request.ToString());

            if (interceptor != null)
                interceptor.InterceptRequest(request, cancellationToken);

            return base.SendAsync(request, cancellationToken).ContinueWith(
                     requestTask =>
                     {
                         Debug.WriteLine("Entering Http client's response message handler. Response details: {0}", requestTask.Result);
                         if (interceptor != null)
                             interceptor.InterceptResponse(requestTask.Result, cancellationToken);

                         return requestTask.Result;
                     }, TaskContinuationOptions.OnlyOnRanToCompletion
                 );
        }
    }
}
