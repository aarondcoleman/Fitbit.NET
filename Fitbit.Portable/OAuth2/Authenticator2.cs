using Fitbit.Api.Portable.OAuth2;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fitbit.Api.Portable
{
    public class Authenticator2
    {
        const string FitbitWebAuthBaseUrl = "https://www.fitbit.com";
        
        const string OAuthBase = "/oauth2";

        private readonly string _clientId;
        private readonly string _appSecret;
        private readonly string _redirectUri;

        public Authenticator2(string clientId, string appSecret, string redirectUri)
        {
            _clientId = clientId;
            _appSecret = appSecret;
            _redirectUri = redirectUri;
        }
        
        public string GenerateAuthUrl(string[] scopeTypes, string state = null)
        {
            var sb = new StringBuilder();

            sb.Append(FitbitWebAuthBaseUrl);
            sb.Append(OAuthBase);
            sb.Append("/authorize?");
            sb.Append("response_type=code");
            sb.Append(string.Format("&client_id={0}", _clientId));
            sb.Append(string.Format("&redirect_uri={0}", Uri.EscapeDataString(_redirectUri)));
            sb.Append(string.Format("&scope={0}", string.Join(" ", scopeTypes)));

            if(!string.IsNullOrWhiteSpace(state))
                sb.Append(string.Format("&state={0}", state));

            return sb.ToString();
        }

        /// <summary>
        /// Exchange the supplied code for an OAuth2AccessToken. If the response is unsuccessful a FitbitException will be thrown
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<OAuth2AccessToken> ExchangeAuthCodeForAccessTokenAsync(string code)
        {
            string postUrl = OAuth2Helper.FitbitOauthPostUrl;

            var content = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("client_id", _clientId),
                //new KeyValuePair<string, string>("client_secret", _appSecret),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", _redirectUri)
            });

            HttpClient httpClient = new HttpClient();

            string clientIdConcatSecret = OAuth2Helper.Base64Encode(_clientId + ":" + _appSecret);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", clientIdConcatSecret); 

            HttpResponseMessage response = await httpClient.PostAsync(postUrl, content);
            string responseString = await response.Content.ReadAsStringAsync();

            return OAuth2Helper.ParseAccessTokenResponse(responseString);
        }
    }
}