using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fitbit.Models
{
    public class HeartActivitiesTimeSeries
    {
        [JsonProperty(PropertyName = "dateTime")]
        public DateTime DateTime { get; set; }

        [JsonProperty(PropertyName = "value")]
        public HeartActivitiesValue Value { get; set; }
    }

    public class HeartActivitiesValue
    {
        [JsonProperty(PropertyName = "customHeartRateZones")]
        public List<HeartRateZone> CustomHeartRateZones { get; set; }

        [JsonProperty(PropertyName = "heartRateZones")]
        public List<HeartRateZone> HeartRateZones { get; set; }

        [JsonProperty(PropertyName = "restingHeartRate")]
        public int RestingHeartRate { get; set; }
    }

}
