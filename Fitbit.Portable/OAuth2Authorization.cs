using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Fitbit.Api.Portable
{
    public class OAuth2Authorization : IAuthorization
    {
        internal string BearerToken { get; private set; }
        internal string RefreshToken { get; private set; }

        public OAuth2Authorization(string bearerToken, string refreshToken)
        {
            this.RefreshToken = refreshToken;
            this.BearerToken = bearerToken;
        }

        public HttpClient CreateAuthorizedHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", BearerToken);
            httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;

            return httpClient;
        }

    }
}
