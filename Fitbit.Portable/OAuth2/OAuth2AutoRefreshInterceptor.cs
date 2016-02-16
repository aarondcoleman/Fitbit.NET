using Fitbit.Models;
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

namespace Fitbit.Api.Portable.OAuth2
{
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
                    var RefreshedToken = Client.TokenManager.RefreshToken(Client).Result;
                    Client.AccessToken = RefreshedToken;

                    //Only retry the first time.
                    if (!response.Result.RequestMessage.Headers.Contains(CUSTOM_HEADER))
                    {
                        var clonedRequest = await response.Result.RequestMessage.CloneAsync();
                        clonedRequest.Headers.Add(CUSTOM_HEADER, CUSTOM_HEADER);
                        return  await Client.HttpClient.SendAsync(clonedRequest, cancellationToken);
                    }
                    else if (response.Result.RequestMessage.Headers.Contains(CUSTOM_HEADER))
                    {
                        throw new FitbitException("We received an unexpected stale token response -- during the retry for a call whose token we just refreshed", response.Result.StatusCode);
                    }
                }                
            }

            //let the pipeline continue
            return null;
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
    }
}
