using System.Collections.Generic;
using System.Linq;

namespace Fitbit.Models
{
    public class ActivitySummary
    {
        // removed from Fitbit API:  https://groups.google.com/forum/#!topic/fitbit-api/8IRaX6RW7g4
        //public int ActiveScore{ get; set; }
        public int ActivityCalories { get; set; }
        public int CaloriesOut { get; set; }
        public int CaloriesBMR { get; set; }
        public List<ActivityDistance> Distances { get; set; }
        public float? Elevation { get; set; }
        public float FairlyActiveMinutes { get; set; }
        public int? Floors { get; set; }
        public List<HeartRateZone> HeartRateZones { get; set; }
        public float LightlyActiveMinutes { get; set; }
        public float MarginalCalories { get; set; }
        public int? RestingHeartRate { get; set; }
        public float SedentaryMinutes { get; set; }
        public int Steps { get; set; }
        public float VeryActiveMinutes { get; set; }

        public Dictionary<string, float> GetDistancesAsDictionary()
        {
            return (Distances ?? new List<ActivityDistance>()).ToDictionary(ad => ad.Activity, ad => ad.Distance);
        }
    }
}