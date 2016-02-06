using Fitbit.Api.Portable;
using Fitbit.Models;
using NUnit.Framework;

namespace Fitbit.Portable.Tests
{
    public class ActivitiesStatsTests
    {
        [Test]
        [Category("Portable")]
        public void Can_Deserialize_Activities()
        {
            string content = SampleDataHelper.GetContent("ActivitiesStats.json");
            var deserializer = new JsonDotNetSerializer();

            ActivitiesStats stats = deserializer.Deserialize<ActivitiesStats>(content);


        }
    }
}
