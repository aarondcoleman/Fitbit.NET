using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fitbit.Models;
using Newtonsoft.Json;

namespace Fitbit.Api.Portable.Models
{
    public class HeartActivitiesIntradayTimeSeries
    {
        [JsonProperty(PropertyName = "activities-heart")]
        public List<ActivitesHeart> ActivitiesHeart { get; set; }
        [JsonProperty(PropertyName = "activities-heart-intraday")]
        public ActivitesHeartIntraday ActivitiesHeartIntraday { get; set; }
    }
    public class ActivitesHeart
    {
        [JsonProperty(PropertyName = "customHeartRateZones")]
        public List<HeartRateZone> CustomHeartRateZones { get; set; }
        [JsonProperty(PropertyName = "dateTime")]
        public DateTime DateTime { get; set; }
        [JsonProperty(PropertyName = "heartRateZones")]
        public List<HeartRateZone> HeartRateZones { get; set; }
        [JsonProperty(PropertyName = "value")]
        public double Value { get; set; }
    }

    public class ActivitesHeartIntraday
    {
        [JsonProperty(PropertyName = "dataset")]
        public List<HeartRateDatasetInterval> Dataset { get; set; }

        [JsonProperty(PropertyName = "datasetInterval")]
        public int DatasetInterval { get; set; }

        [JsonProperty(PropertyName = "datasetType")]
        public string DatasetType { get; set; }
    }
    public class HeartRateDatasetInterval
    {
        [JsonProperty(PropertyName = "time")]
        public DateTime Time { get; set; }

        [JsonProperty(PropertyName = "value")]
        public int Value { get; set; }
    }
}
