using Newtonsoft.Json;

namespace Fitbit.Api.Portable.OAuth2
{
    public class OAuth2AccessToken
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; } // "Bearer" is expected

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; } //maybe convert this to a DateTime ?

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}