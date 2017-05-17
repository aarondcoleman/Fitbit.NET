using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.OAuth2;

namespace FitBitConsoleTester
{
    public class OAuth2
    {
        static void Main(string[] args)
        {

            Task.Run(async () =>
            {
                // credentials
                FitbitAppCredentials credentials = new FitbitAppCredentials()
                {
                    ClientId = "228HSC",
                    ClientSecret = "1fc08b390e5b1bbd88dab79ebd004902"
                };

                // authenticate
                var helper = new OAuth2Helper(credentials, "http://localhost:1531/fitbit/callback"); // example call back url
                var authUrl = helper.GenerateAuthUrl(new[] { "activity", "sleep" });

                Process.Start(authUrl);

                var pin = Console.ReadLine();

                var token = await helper.ExchangeAuthCodeForAccessTokenAsync(pin);

                // var request
                var fitbitClient = new FitbitClient(credentials, token);

                DateTime testDateTime = DateTime.Today;


                var stats = await fitbitClient.GetSleepDateAsync(testDateTime, fitbitClient.AccessToken.UserId);

                //Console.WriteLine($"Total steps: {stats.Lifetime.Total.Steps}");

                Console.WriteLine($"Total sleep:{stats.Summary.TotalSleepRecords}");
                Console.ReadLine();
            }).Wait();


        }
    }
}
