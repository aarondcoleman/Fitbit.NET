using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Fitbit.Api.Portable
{
    /// <summary>
    /// This class implements IAuthorization adding auth signature to requests.
    /// The OAuth2.0 implementation returns an HttpClient object with bearer token set.
    /// 
    /// </summary>
    public class OAuth2Authorization : IAuthorization
    {
        internal OAuth2AccessToken _accessToken;

        /*
        public OAuth2Authorization(string bearerToken, string refreshToken)
        {
            this.RefreshToken = refreshToken;
            this.BearerToken = bearerToken;
        }
        */

        public OAuth2Authorization(IOAuth2AccessTokenRepository repository)
        {
            _accessToken = repository.Get();
        }

        public HttpClient CreateAuthorizedHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", this._accessToken.Token);
            httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;

            return httpClient;
        }

    }
}
