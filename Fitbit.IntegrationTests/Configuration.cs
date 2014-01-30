using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.IntegrationTests
{
    public class Configuration
    {
        //STEP 1 of Auth
        public readonly static string ConsumerKey = "a9e9e4f0c8ec491b918b9aebe3fa6b06";
        public readonly static string ConsumerSecret = "c4c9745caa3a4a2e803e8bd87ee964d9";
        public readonly static string RequestTokenUrl = "http://api.fitbit.com/oauth/request_token";
        public readonly static string AccessTokenUrl = "http://api.fitbit.com/oauth/access_token";
        public readonly static string AuthorizeUrl = "http://api.fitbit.com/oauth/authorize";

        //Step 2 of Auth -- Insert this after first test runs and using the outputted authUrl
        public readonly static string TempAuthToken = "YOUR_TEMP_AUTH_TOKEN_HERE";
        public readonly static string TempAuthVerifier = "YOUR_TEMP_AUTH_VERIFIER_HERE";

        //STEP 3 Permanent Auth Tokens Here
        public readonly static string AuthToken = "ae6714ba71ce4bffa77d5ae7279279e2";
        public readonly static string AuthTokenSecret = "687bac0d3eedacde54b35b577c4f2016";
        public readonly static string FitbitUserId = "26SCX3";
    }
}
