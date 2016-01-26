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
            var sb = new StringBuilder();

            sb.Append(FitbitWebAuthBaseUrl);
            sb.Append(OAuthBase);
            sb.Append("/authorize?");
            sb.Append("response_type=code");
            sb.Append(string.Format("&client_id={0}", this.ClientId));
            sb.Append(string.Format("&redirect_uri={0}", Uri.EscapeDataString(this.RedirectUri)));
            sb.Append(string.Format("&scope={0}", String.Join(" ", scopeTypes)));

            if(!string.IsNullOrWhiteSpace(state))
                sb.Append(string.Format("&state={0}", state));

            return sb.ToString();
        }

        public async Task<OAuth2AccessToken> ExchangeAuthCodeForAccessTokenAsync(string code)
        {
            HttpClient httpClient = new HttpClient();

            string postUrl = GenerateFitbitOAuthPostUrl();

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

            OAuth2AccessToken accessToken = ParseAccessTokenResponse(responseString);

            return accessToken;
        }



        public async Task<OAuth2AccessToken> RefreshAccessToken(OAuth2AccessToken accessToken)
        {
            string postUrl = GenerateFitbitOAuthPostUrl();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", accessToken.RefreshToken),
            });


            var httpClient = new HttpClient();

            var clientIdConcatSecret = Base64Encode(ClientId + ":" + AppSecret);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", clientIdConcatSecret);

            HttpResponseMessage response = await httpClient.PostAsync(postUrl, content);
            string responseString = await response.Content.ReadAsStringAsync();

            return ParseAccessTokenResponse(responseString);
        }

        private string GenerateFitbitOAuthPostUrl()
        {
            var sb = new StringBuilder();

            sb.Append(FitbitApiBaseUrl);
            sb.Append(OAuthBase);
            sb.Append(@"/token");

            return sb.ToString();
        }

        private OAuth2AccessToken ParseAccessTokenResponse(string responseString)
        {
            JObject responseObject = JObject.Parse(responseString);

            // Note: if user cancels the auth process Jawbone returns a 200 response, but the JSON payload is way different.
            var error = responseObject["error"];
            if (error != null)
            {
                // TODO: Actually should probably raise an exception here maybe?
                //mxa0079: agree. This is not transparent and makes it hard to debug for consumers of the library
                return null;
            }

            var accessToken = new OAuth2AccessToken();

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
