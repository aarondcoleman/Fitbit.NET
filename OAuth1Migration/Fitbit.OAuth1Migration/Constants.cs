using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitbit.OAuth1Migration
{
    internal class Constants
    {
        public const string BaseApiUrl = "https://api.fitbit.com/";
        public const string TemporaryCredentialsRequestTokenUri = "oauth/request_token";
        public const string TemporaryCredentialsAccessTokenUri = "oauth/access_token";
        public const string AuthorizeUri = "oauth/authorize";
        public const string LogoutAndAuthorizeUri = "oauth/logout_and_authorize";

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
