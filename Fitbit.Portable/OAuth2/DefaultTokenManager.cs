namespace Fitbit.Api.Portable.OAuth2
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    internal class DefaultTokenManager : ITokenManager
    {
        private static string FitbitOauthPostUrl => "https://api.fitbit.com/oauth2/token";

        public async Task<OAuth2AccessToken> RefreshTokenAsync(FitbitClient client)
        {
            return await RefreshTokenAsync(client, null);
        }

        public async Task<OAuth2AccessToken> RefreshTokenAsync(FitbitClient client, int? expiresIn = null)
        {
            string postUrl = FitbitOauthPostUrl;

            List<KeyValuePair<string, string>> contentValues = new List<KeyValuePair<string, string>>();
            contentValues.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
            contentValues.Add(new KeyValuePair<string, string>("refresh_token", client.AccessToken.RefreshToken));

            if (expiresIn.HasValue)
                contentValues.Add(new KeyValuePair<string, string>("expires_in", expiresIn.Value.ToString()));

            var content = new FormUrlEncodedContent(contentValues);

            var httpClient = new HttpClient();

            var clientIdConcatSecret = OAuth2Helper.Base64Encode(client.AppCredentials.ClientId + ":" + client.AppCredentials.ClientSecret);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", clientIdConcatSecret);

            HttpResponseMessage response = await httpClient.PostAsync(postUrl, content);
            string responseString = await response.Content.ReadAsStringAsync();

            return OAuth2Helper.ParseAccessTokenResponse(responseString);
        }
    }
}
