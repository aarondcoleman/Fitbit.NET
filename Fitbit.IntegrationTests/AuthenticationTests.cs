using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Fitbit;
using Fitbit.Api;
using Fitbit.Models;

namespace Fitbit.IntegrationTests
{
    [TestFixture]
    class AuthenticationTests
    {
        public AuthenticationTests()
        {
            authenticator = new Fitbit.Api.Authenticator(Configuration.ConsumerKey,
                                            Configuration.ConsumerSecret,
                                            "http://api.fitbit.com/oauth/request_token",
                                            "http://api.fitbit.com/oauth/access_token",
                                            "http://api.fitbit.com/oauth/authorize");

        }

        private Authenticator authenticator;

        /// <summary>
        /// STEP 1
        /// Run this first, it will succeed and then use the outputed URL to get the TempAuthToken and TempAuthVerifier
        /// STEP 2
        /// Place both of these is Configuration.cs in "STEP 2" as the next authentication test depends on these
        /// NOTE: Do this quickly. Your temp token expire within minutes
        /// </summary>
        [Test]
        public void Can_Retrieve_Access_Token_Authorization_Url()
        {
            string authUrl = authenticator.GetAuthUrlToken();
            
            Assert.IsNotNull(authUrl);
            Console.Write("authUrl:" + authUrl);
            Assert.IsTrue(authUrl.Contains("https://api.fitbit.com/oauth/authorize?oauth_token="));
        }

        /// <summary>
        /// STEP 3
        /// With the the user having approved the test app, and in possession of temp auth token and verifier
        /// Asks Fitbit for the permanent AuthToken and AuthTokenSecret
        /// </summary>
        [Test]
        public void Can_Retrieve_Access_Token_And_Access_Token_Secret()
        {
            AuthCredential authCredential = authenticator.ProcessApprovedAuthCallback(Configuration.TempAuthToken, Configuration.TempAuthVerifier);

            Assert.IsNotNull(authCredential.AuthToken);
            Console.WriteLine("AuthToken: " + authCredential.AuthToken);

            Assert.IsNotNull(authCredential.AuthTokenSecret);
            Console.WriteLine("AuthTokenSecret: " + authCredential.AuthTokenSecret);

            Assert.IsNotNull(authCredential.UserId); //encoded Fitbit UserId
            Console.WriteLine("Fitbit UserId: " + authCredential.UserId);


        }




    }
}
