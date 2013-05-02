using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.IntegrationTests
{
    public class Configuration
    {
        //STEP 1 of Auth
        public readonly static string ConsumerKey = "fc287dde0f1f46dcb8faef3c0fbc2d09";
        public readonly static string ConsumerSecret = "078c148596b141a689ee03513259371e";
        public readonly static string RequestTokenUrl = "http://api.fitbit.com/oauth/request_token";
        public readonly static string AccessTokenUrl = "http://api.fitbit.com/oauth/access_token";
        public readonly static string AuthorizeUrl = "http://api.fitbit.com/oauth/authorize";

        //Step 2 of Auth -- Insert this after first test runs and using the outputted authUrl
        public readonly static string TempAuthToken = "d2c52d187223535791a717ae538f0381";
        public readonly static string TempAuthVerifier = "3iuqaj4ojt86qnujv442sn94ph";

        //STEP 3 Permanent Auth Tokens Here
        public readonly static string AuthToken = "28263b9a29e79f9d6f07539ab9109aba";
        public readonly static string AuthTokenSecret = "f6ee9dd75f0a8bc5f20415b8b445ca66";
        public readonly static string FitbitUserId = "24VPWW";
    }
}