using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace Fitbit.Api.Portable.OAuth2
{
    internal static class OAuth2Helper
    {
        public static string FitbitOauthPostUrl => "https://api.fitbit.com/oauth2/token";

        internal static OAuth2AccessToken ParseAccessTokenResponse(string responseString)
        {
            // assumption is the errors json will return in usual format eg. errors array
            JObject responseObject = JObject.Parse(responseString);

            var error = responseObject["errors"];
            if (error != null)
            {
                var errors = new JsonDotNetSerializer().Errors(responseString);
                throw new FitbitTokenException(errors);
            }

            var deserializer = new JsonDotNetSerializer();
            return deserializer.Deserialize<OAuth2AccessToken>(responseString);
        }

        //http://stackoverflow.com/a/11743162
        internal static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}