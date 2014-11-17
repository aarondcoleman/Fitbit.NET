﻿using System;
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
    [TestFixture]
    public class ActivityTestsJson : TestsBase
    {
        public ActivityTestsJson() 
            : base(ResponseType.Json)
        {
            //base class initializes client
        }

        //protected FitbitClient client;

        [Test]
        public void Retrieve_Activity_Summary_For_Yesterday()
        {
            ActivitySummary activitySummary = client.GetDayActivitySummary(DateTime.Now.Subtract(new TimeSpan(24, 0, 0)));

            Assert.IsNotNull(activitySummary);

            Assert.IsNotNull(activitySummary.Steps);
            Console.WriteLine("Steps: " + activitySummary.Steps);
            
            Assert.IsNotNull(activitySummary.SedentaryMinutes);
            Console.WriteLine("SedentaryMinutes: " + activitySummary.SedentaryMinutes);

            Assert.IsNotNull(activitySummary.Distances);
        }

        
    }
}
