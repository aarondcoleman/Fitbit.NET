using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fitbit.Models
{
    public class HeartActivitiesIntraday
    {
        [JsonProperty(PropertyName = "dataset")]
        public List<DatasetInterval> Dataset { get; set; }

        [JsonProperty(PropertyName = "datasetInterval")]
        public int DatasetInterval { get; set; }

        [JsonProperty(PropertyName = "datasetType")]
        public string DatasetType { get; set; }
    }

    public class DatasetInterval
    {
        [JsonProperty(PropertyName = "time")]
        public DateTime Time { get; set; }

        [JsonProperty(PropertyName = "value")]
        public int Value { get; set;}
    }
}