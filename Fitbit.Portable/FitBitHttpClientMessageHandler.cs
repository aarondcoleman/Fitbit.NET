namespace Fitbit.Api.Portable
{
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    internal class FitbitHttpClientMessageHandler : DelegatingHandler
    {
        private IFitbitInterceptor interceptor;
        Func<Task<HttpResponseMessage>, CancellationToken, Task<HttpResponseMessage>> responseHandler;

        public ITokenManager TokenManager { get; private set; }

        public FitbitClient Client { get; private set; }

        public FitbitHttpClientMessageHandler(FitbitClient client, IFitbitInterceptor interceptor, ITokenManager tokenManager)
        {
            this.Client = client;
            this.interceptor = interceptor;
            this.TokenManager = tokenManager;
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
            DebugLogResponse(requestTask);

            if (requestTask.Result.StatusCode == System.Net.HttpStatusCode.Unauthorized)//Unauthorized, then there is a chance token is stale
            {
                var responseBody = requestTask.Result.Content.ReadAsStringAsync().Result;

                if (IsTokenStale(responseBody))
                {
                    Debug.WriteLine("Stale token detected. Invoking registered tokenManager.RefreskToken to refresh it");
                    var RefreshedToken = TokenManager.RefreshToken(this.Client).Result;
                    this.Client.AccessToken = RefreshedToken;
                    //TO Do: either retry or notify client consumer that the called failed but it has been automatically retried
                }
            }

            if (interceptor != null)
                await interceptor.InterceptResponse(responseTask, cancellationToken);

            return responseTask.Result;
        }

        private bool IsTokenStale(string responseBody)
        {
            JObject response = JObject.Parse(responseBody);
            IList<JToken> errors = response["errors"].Children().ToList();

            foreach (JToken error in errors)
            {
                var apiError = JsonConvert.DeserializeObject<ApiError>(error.ToString());
                if (apiError.ErrorType == "expired_token")
                    return true;
            }

            return false;
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
