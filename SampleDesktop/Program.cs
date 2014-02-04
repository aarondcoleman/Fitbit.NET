using Fitbit.Api;
using Fitbit.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDesktop
{
    class Program
    {
        static void Main(string[] args)
        {
            //Example of getting the Auth credentials for the first time by directoring the
            //user to the fitbit site to get a PIN. 
            var consumerKey = "YOUR_CONSUMER_KEY_HERE";
            var consumerSecret = "YOUR_CONSUMER_SECRET_HERE";
            var requestTokenUrl = "http://api.fitbit.com/oauth/request_token";
            var accessTokenUrl = "http://api.fitbit.com/oauth/access_token";
            var authorizeUrl = "http://www.fitbit.com/oauth/authorize";

            var a = new Authenticator(consumerKey, consumerSecret,requestTokenUrl,accessTokenUrl,authorizeUrl);

            RequestToken token = a.GetRequestToken();

            var url = a.GenerateAuthUrlFromRequestToken(token, false);

            Process.Start(url);

            Console.WriteLine("Enter the verification code from the website");
            var pin = Console.ReadLine();

            var credentials = a.GetAuthCredentialFromPin(pin, token);


            //If you already have your credentials stored then rather than getting the users PIN again
            //you could just start here
            var fitbit = new FitbitClient(consumerKey, consumerSecret, credentials.AuthToken, credentials.AuthTokenSecret);
            var profile = fitbit.GetUserProfile();
            Console.WriteLine("Your last weight was {0}",profile.Weight);

            Console.ReadLine();
        }
    }
}
