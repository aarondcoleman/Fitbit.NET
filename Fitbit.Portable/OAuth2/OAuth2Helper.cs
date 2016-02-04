using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Api.Portable.OAuth2
{
    internal static class OAuth2Helper
    {
        const string FitbitWebAuthBaseUrl = "https://www.fitbit.com";
        const string FitbitApiBaseUrl = "https://api.fitbit.com";
        const string OAuthBase = "/oauth2";

        public static string FitbitOauthPostUrl { get; }

        static OAuth2Helper()
        {
            var sb = new StringBuilder();

            sb.Append(FitbitApiBaseUrl);
            sb.Append(OAuthBase);
            sb.Append(@"/token");

            FitbitOauthPostUrl = sb.ToString();
        }

        internal static OAuth2AccessToken ParseAccessTokenResponse(string responseString)
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
        internal static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
