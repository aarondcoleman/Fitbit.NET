using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fitbit.Models;

namespace Fitbit.Api.Portable.OAuth2
{
    /// <summary>
    /// An Http interceptor that intercepts "stale token" responses and invokes the Token Manager of the FitbitClient to get a new token.
    /// </summary>
    public class OAuth2AutoRefreshInterceptor : IFitbitInterceptor
    {
        private const string CUSTOM_HEADER = "Fitbit.NET-StaleTokenRetry";

        public Task<HttpResponseMessage> InterceptRequest(HttpRequestMessage request, CancellationToken cancellationToken, FitbitClient Client)
        {
            return null;
        }

        public async Task<HttpResponseMessage> InterceptResponse(Task<HttpResponseMessage> response, CancellationToken cancellationToken, FitbitClient Client)
        {
            if (response.Result.StatusCode == System.Net.HttpStatusCode.Unauthorized)//Unauthorized, then there is a chance token is stale
            {
                var responseBody = await response.Result.Content.ReadAsStringAsync();

                if (IsTokenStale(responseBody))
                {
                    Debug.WriteLine("Stale token detected. Invoking registered tokenManager.RefreskToken to refresh it");
                    await Client.RefreshOAuth2TokenAsync();

                    //Only retry the first time.
                    if (!response.Result.RequestMessage.Headers.Contains(CUSTOM_HEADER))
                    {
                        var clonedRequest = await response.Result.RequestMessage.CloneAsync();
                        clonedRequest.Headers.Add(CUSTOM_HEADER, CUSTOM_HEADER);
                        return  await Client.HttpClient.SendAsync(clonedRequest, cancellationToken);
                    }
                    else if (response.Result.RequestMessage.Headers.Contains(CUSTOM_HEADER))
                    {
                        throw new FitbitTokenException(response.Result, message: $"In interceptor {nameof(OAuth2AutoRefreshInterceptor)} inside method {nameof(InterceptResponse)} we received an unexpected stale token response - during the retry for a call whose token we just refreshed {response.Result.StatusCode}");   
                    }
                }                
            }

            //let the pipeline continue
            return null;
        }

        private bool IsTokenStale(string responseBody)
        {
            var errors = new JsonDotNetSerializer().ParseErrors(responseBody);
            return errors.Any(error => error.ErrorType == "expired_token");
        }
    }
}