using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fitbit.Models
{
    public class ActivityList
    {
        [JsonProperty(PropertyName = "activeDuration")]
        public int ActiveDuration { get; set; }

        [JsonProperty(PropertyName = "activityLevel")]
        public List<ActivityLevel> ActivityLevel { get; set; }

        [JsonProperty(PropertyName = "activityName")]
        public string ActivityName { get; set; }

        [JsonProperty(PropertyName = "activityTypeId")]
        public int ActivityTypeId { get; set; }

        [JsonProperty(PropertyName = "averageHeartRate")]
        public int AverageHeartRate { get; set; }

        [JsonProperty(PropertyName = "calories")]
        public int Calories { get; set; }
        
        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }

        [JsonProperty(PropertyName = "distanceUnit")]
        public string DistanceUnit { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        [JsonProperty(PropertyName = "elevationGain")]
        public double ElevationGain { get; set; }

        [JsonProperty(PropertyName = "heartRateZones")]
        public List<HeartRateZone> HeartRateZones { get; set; }

        [JsonProperty(PropertyName = "lastModified")]
        public DateTime LastModified { get; set; }

        [JsonProperty(PropertyName = "logId")]
        public int LogId { get; set; }

        [JsonProperty(PropertyName = "logType")]
        public string LogType { get; set; }

        [JsonProperty(PropertyName = "manualValuesSpecified")]
        public ManualValuesSpecified ManualValuesSpecified { get; set; }

        [JsonProperty(PropertyName = "originalDuration")]
        public int OriginalDuration { get; set; }

        [JsonProperty(PropertyName = "originalStartTime")]
        public DateTime OriginalStartTime { get; set; }

        [JsonProperty(PropertyName = "pace")]
        public double Pace { get; set; }

        [JsonProperty(PropertyName = "source")]
        public ActivityLogSource Source { get; set; }

        [JsonProperty(PropertyName = "speed")]
        public double Speed { get; set; }

        [JsonProperty(PropertyName = "startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty(PropertyName = "steps")]
        public int Steps { get; set; }

        [JsonProperty(PropertyName = "tcxLink")]
        public string TcxLink { get; set; }
    }

    public class ActivityLevel
    {
        [JsonProperty(PropertyName = "minutes")]
        public double Minutes { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    //public class HeartRateZone
    //{
    //    public int max { get; set; }
    //    public int min { get; set; }
    //    public int minutes { get; set; }
    //    public string name { get; set; }
    //}

    public class ManualValuesSpecified
    {
        [JsonProperty(PropertyName = "calories")]
        public bool Calories { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public bool Distance { get; set; }

        [JsonProperty(PropertyName = "steps")]
        public bool Steps { get; set; }
    }

    public class ActivityLogSource
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}
