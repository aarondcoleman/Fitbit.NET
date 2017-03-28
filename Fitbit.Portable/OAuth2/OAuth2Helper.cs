using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Fitbit.Api.Portable.OAuth2
{
    public class OAuth2Helper
    {
        private const string FitbitWebAuthBaseUrl = "https://www.fitbit.com";
        private const string FitbitApiBaseUrl = "https://api.fitbit.com";

        private const string OAuthBase = "/oauth2";

        private string ClientId;
        private string ClientSecret;

        private string RedirectUri;

        public OAuth2Helper(FitbitAppCredentials credentials, string redirectUri)
        {
            this.ClientId = credentials.ClientId;
            this.ClientSecret = credentials.ClientSecret;
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

            if (!string.IsNullOrWhiteSpace(state))
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


            string clientIdConcatSecret = OAuth2Helper.Base64Encode(ClientId + ":" + ClientSecret);

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", clientIdConcatSecret);

            HttpResponseMessage response = await httpClient.PostAsync(postUrl, content);
            string responseString = await response.Content.ReadAsStringAsync();

            OAuth2AccessToken accessToken = OAuth2Helper.ParseAccessTokenResponse(responseString);

            return accessToken;
        }

        public static readonly string FitbitOauthPostUrl = "https://api.fitbit.com/oauth2/token";


        public static OAuth2AccessToken ParseAccessTokenResponse(string responseString)
        {
            // assumption is the errors json will return in usual format eg. errors array
            JObject responseObject = JObject.Parse(responseString);

            var error = responseObject["errors"];
            if (error != null)
            {
                var errors = new JsonDotNetSerializer().ParseErrors(responseString);
                throw new FitbitException($"Unable to parse token response in method -- {nameof(ParseAccessTokenResponse)}.", errors);
            }

            var deserializer = new JsonDotNetSerializer();
            return deserializer.Deserialize<OAuth2AccessToken>(responseString);
        }

        /// <summary>
        /// Convert plain text to a base 64 encoded string - http://stackoverflow.com/a/11743162
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}