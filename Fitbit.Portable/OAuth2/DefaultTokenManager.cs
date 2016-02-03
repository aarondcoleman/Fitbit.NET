namespace Fitbit.Api.Portable.OAuth2
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    internal class DefaultTokenManager : ITokenManager
    {
        public async Task<OAuth2AccessToken> RefreshToken(FitbitClient client)
        {
            string postUrl = OAuth2Helper.FitbitOauthPostUrl;

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", client.AccessToken.RefreshToken),
            });


            var httpClient = new HttpClient();

            var clientIdConcatSecret = OAuth2Helper.Base64Encode(client.AppCredentials.Value.ClientId + ":" + client.AppCredentials.Value.ClientSecret);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", clientIdConcatSecret);

            HttpResponseMessage response = await httpClient.PostAsync(postUrl, content);
            string responseString = await response.Content.ReadAsStringAsync();

            return OAuth2Helper.ParseAccessTokenResponse(responseString);
        }
    }
}
