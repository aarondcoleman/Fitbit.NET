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
        public OAuth2Authorization(string bearerToken, string refreshToken)
        {

        }

        public void SetAuthorizationHeader(HttpClient httpClient)
        {
            AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", bearerToken); 
            httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue; 

        }

    }
}
