using Fitbit.Api.Portable.OAuth2;
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

            string postUrl = OAuth2Helper.FitbitOauthPostUrl;

            var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", ClientId),
                //new KeyValuePair<string, string>("client_secret", AppSecret),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", this.RedirectUri)
            });


            string clientIdConcatSecret = OAuth2Helper.Base64Encode(ClientId + ":" + AppSecret);

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", clientIdConcatSecret); 

            HttpResponseMessage response = await httpClient.PostAsync(postUrl, content);
            string responseString = await response.Content.ReadAsStringAsync();

            OAuth2AccessToken accessToken = OAuth2Helper.ParseAccessTokenResponse(responseString);

            return accessToken;
        }

    }
}
