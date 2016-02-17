using System;
using System.Collections.Generic;

namespace Fitbit.Models
{
    public class HeartActivities
    {

        public DateTime DateTime { get; set; }
        public List<HeartRateZone> HeartRateZones { get; set; }

        public List<HeartRateZone> CustomHeartRateZones { get; set; }

        public float AverageHeartRate { get; set; }

        public int RestingHeartRate { get; set; }
    }
}
