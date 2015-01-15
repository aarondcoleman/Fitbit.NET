namespace Fitbit.IntegrationTests
{
       public class Configuration_Sample /* STEP 0 - change this class name to just "Configuration" */
       {
        
        //STEP 1 of Auth
        public readonly static string ConsumerKey = "YOUR_CONSUMER_KEY_HERE";
        public readonly static string ConsumerSecret = "YOUR_CONSUMER_SECRET_HERE";
        public readonly static string RequestTokenUrl = "http://api.fitbit.com/oauth/request_token";
        public readonly static string AccessTokenUrl = "http://api.fitbit.com/oauth/access_token";
        public readonly static string AuthorizeUrl = "http://api.fitbit.com/oauth/authorize";

        //Step 2 of Auth -- Insert this after first test runs and using the outputted authUrl
        public readonly static string TempAuthToken = "YOUR_TEMP_AUTH_TOKEN_HERE";
        public readonly static string TempAuthVerifier = "YOUR_TEMP_AUTH_VERIFIER_HERE";

        //STEP 3 Permanent Auth Tokens Here
        public readonly static string AuthToken = "YOUR_AUTH_TOKEN_HERE";
        public readonly static string AuthTokenSecret = "YOUR_AUTH_TOKEN_SECRET_HERE";
        public readonly static string FitbitUserId = "YOUR_FITBIT_USERID_HERE";
    }
}
