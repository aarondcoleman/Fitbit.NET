using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Fitbit.Api;
using RestSharp;
using Fitbit.Models;


namespace Fitbit.IntegrationTests
{
    public class TestsBase
    {
        public TestsBase()
        {
            RestClient restClient = new RestClient("https://api.fitbit.com");
            client = new FitbitClient(Configuration.ConsumerKey,
                                        Configuration.ConsumerSecret,
                                        Configuration.AuthToken,
                                        Configuration.AuthTokenSecret,
                                        restClient);

            
        }

        protected IFitbitClient client;

    }
}
