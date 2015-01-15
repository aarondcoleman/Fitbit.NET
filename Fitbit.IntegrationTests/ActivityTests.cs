using System;
using NUnit.Framework;
using Fitbit.Models;

namespace Fitbit.IntegrationTests
{
    [TestFixture]
    public class ActivityTests : TestsBase
    {
        [Test]
        [Ignore]
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