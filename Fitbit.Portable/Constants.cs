﻿namespace Fitbit.Api.Portable
{
    internal class Constants
    {
        public const string BaseApiUrl = "https://api.fitbit.com/";
        public const string TemporaryCredentialsRequestTokenUri = "oauth/request_token";
        public const string TemporaryCredentialsAccessTokenUri = "oauth/access_token";
        public const string AuthorizeUri = "oauth/authorize";
        public const string LogoutAndAuthorizeUri = "oauth/logout_and_authorize";
        public const string FloorsUnsupportedOnDeviceError = "Invalid time series resource path: /activities/floors";

        public const int MAX_ACTIVITY_TIME_SERIES_DAYS = 1095;
        public const int MAX_DAYS_THIRTY = 30;

        public class Headers
        {
            public const string XFitbitSubscriberId = "X-Fitbit-Subscriber-Id";
        }

        public class Formatting
        {
            public const string TrailingSlash = "{0}/";
            public const string LeadingDash = "-{0}";
        }
    }
}
