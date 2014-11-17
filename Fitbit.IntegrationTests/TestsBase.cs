using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fitbit.Models.Enums;
using NUnit.Framework;
using Fitbit.Api;
using RestSharp;
using Fitbit.Models;


namespace Fitbit.IntegrationTests
{
    public class TestsBase
    {
        public TestsBase(ResponseType responseType)
        {
            RestClient restClient = new RestClient("https://api.fitbit.com");
            client = new FitbitClient(Configuration.ConsumerKey,
                                        Configuration.ConsumerSecret,
                                        Configuration.AuthToken,
                                        Configuration.AuthTokenSecret,
                                        restClient, responseType);
        }

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
