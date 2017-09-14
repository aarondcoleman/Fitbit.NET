using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fitbit.Models;
using Newtonsoft.Json;

namespace Fitbit.Api.Portable.Models
{
    public class IntradayActivitiesHeart
    {
        [JsonProperty(PropertyName = "customHeartRateZones")]
        public List<HeartRateZone> CustomHeartRateZones { get; set; }

        [JsonProperty(PropertyName = "heartRateZones")]
        public List<HeartRateZone> HeartRateZones { get; set; }

        [JsonProperty(PropertyName = "dateTime")]
        public DateTime DateTime { get; set; }

        [JsonProperty(PropertyName = "value")]
        public double Value { get; set; }
    }
}
