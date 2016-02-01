namespace Fitbit.Api.Portable
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class FitbitHttpClientMessageHandler : DelegatingHandler
    {
        private IFitbitRequestInterceptor logger;

        public FitbitHttpClientMessageHandler(IFitbitRequestInterceptor logger)
        {
            this.logger = logger;
            //Define the inner must handler. Otherwise exception is thrown.
            InnerHandler = new HttpClientHandler();
        }

        // We override the SendAsync method to intercept both the request and response path
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("Entering Http client's request message handler. Request details: {0}", request.ToString());
            if (logger != null)
                logger.Request(request, cancellationToken);

            return base.SendAsync(request, cancellationToken).ContinueWith(
                     requestTask =>
                     {
                         Debug.WriteLine("Entering Http client's response message handler. Response details: {0}", requestTask.Result);
                         if (logger != null)
                             logger.Response(requestTask.Result, cancellationToken);

                         return requestTask.Result;
                     }, TaskContinuationOptions.OnlyOnRanToCompletion
                 );
        }
    }
}
