using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fitbit.Api.Portable
{
    public class Authenticator2
    {
        const string FitbitWebAuthBaseUrl = "https://www.fitbit.com";
        const string FitbitApiBaseUrl = "https://api.fitbit.com";

        const string OAuthBase = "/oauth2";

        private string ClientId;
        private string AppSecret;
        private string RedirectUri;

        public Authenticator2(string clientId, string appSecret, string redirectUri)
        {
            this.ClientId = clientId;
            this.AppSecret = appSecret;
            this.RedirectUri = redirectUri;
        }


        public string GenerateAuthUrl(string[] scopeTypes, string state = null)
        {
            string url = FitbitWebAuthBaseUrl;

            url += OAuthBase;
            url += "/authorize?";
            url += "response_type=code";
            url += string.Format("&client_id={0}", this.ClientId);
            url += string.Format("&redirect_uri={0}", Uri.EscapeDataString(this.RedirectUri));
            url += string.Format("&scope={0}", String.Join(" ", scopeTypes));

            if(!string.IsNullOrWhiteSpace(state))
                url += string.Format("&state={0}", state);

            return url;
        }

        public async Task<OAuth2AccessToken> ExchangeAuthCodeForAccessTokenAsync(string code)
        {
            HttpClient httpClient = new HttpClient();

            string postUrl = FitbitApiBaseUrl;
            postUrl += OAuthBase;
            postUrl += "/token";

            var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", ClientId),
                //new KeyValuePair<string, string>("client_secret", AppSecret),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", this.RedirectUri)
            });


            string clientIdConcatSecret = Base64Encode(ClientId + ":" + AppSecret);

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", clientIdConcatSecret); 

            HttpResponseMessage response = await httpClient.PostAsync(postUrl, content);
            string responseString = await response.Content.ReadAsStringAsync();

            JObject responseObject = JObject.Parse(responseString);

            // Note: if user cancels the auth process Jawbone returns a 200 response, but the JSON payload is way different.
            var error = responseObject["error"];
            if (error != null)
            {
                // TODO: Actually should probably raise an exception here maybe?
                return null;
            }

            OAuth2AccessToken accessToken = new OAuth2AccessToken();

            var temp_access_token = responseObject["access_token"];
            if (temp_access_token != null) accessToken.Token = temp_access_token.ToString();

            var temp_expires_in = responseObject["expires_in"];
            if (temp_expires_in != null) accessToken.ExpiresIn = Convert.ToInt32(temp_expires_in.ToString());

            var temp_token_type = responseObject["token_type"];
            if (temp_token_type != null) accessToken.TokenType = temp_token_type.ToString();

            var temp_refresh_token = responseObject["refresh_token"];
            if (temp_refresh_token != null) accessToken.RefreshToken = temp_refresh_token.ToString();

            return accessToken;
        }

        //http://stackoverflow.com/a/11743162
        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

    }
}
