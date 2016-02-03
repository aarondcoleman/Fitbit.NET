namespace Fitbit.Api.Portable.OAuth2
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    internal class OAuth2HttpClientFactory : IFitbitHttpClientFactory
    {
        private OAuth2AccessToken accessToken;

        public OAuth2HttpClientFactory(OAuth2AccessToken token)
        {
            accessToken = token;
        }

        public HttpClient Create(HttpMessageHandler handler = null)
        {
            var httpClient = new HttpClient(handler);
            AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", this.accessToken.Token);
            httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;

            return httpClient;
        }
    }
}
