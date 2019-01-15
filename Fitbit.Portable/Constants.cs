namespace Fitbit.Api.Portable
{
    internal class Constants
    {
        public const string BaseApiUrl = "https://api.fitbit.com/";
        public const string TemporaryCredentialsRequestTokenUri = "oauth/request_token";
        public const string TemporaryCredentialsAccessTokenUri = "oauth/access_token";
        public const string AuthorizeUri = "oauth/authorize";
        public const string LogoutAndAuthorizeUri = "oauth/logout_and_authorize";
        public const string FloorsUnsupportedOnDeviceError = "Invalid time series resource path: /activities/floors";

        public class Headers
        {
            public const string XFitbitSubscriberId = "X-Fitbit-Subscriber-Id";
            public const string AcceptLanguage = "Accept-Language";
        }

        public class UnitSystem
        {
            public const string US = "en_US";
            public const string UK = "en_UK";
            public const string Metric = "en_METRIC"; //Any value besides "en_US" and "en_UK" will be considered METRIC
        }

        public class Formatting
        {
            public const string TrailingSlash = "{0}/";
            public const string LeadingDash = "-{0}";
        }
    }
}
