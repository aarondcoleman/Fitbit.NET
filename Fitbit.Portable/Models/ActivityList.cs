using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fitbit.Models
{
    public class ActivityList
    {
        [JsonProperty(PropertyName = "activeDuration")]
        public double ActiveDuration { get; set; }

        [JsonProperty(PropertyName = "activityLevel")]
        public List<ActivityLevel> ActivityLevel { get; set; }

        [JsonProperty(PropertyName = "activityName")]
        public string ActivityName { get; set; }

        [JsonProperty(PropertyName = "activityTypeId")]
        public double ActivityTypeId { get; set; }
        
        //public int averageHeartRate { get; set; }

        [JsonProperty(PropertyName = "calories")]
        public double Calories { get; set; }
        
        //public string caloriesLink { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }

        [JsonProperty(PropertyName = "distanceUnit")]
        public string DistanceUnit { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public double Duration { get; set; }
        
        //public string heartRateLink { get; set; }

        [JsonProperty(PropertyName = "lastModified")]
        public DateTime LastModified { get; set; }

        [JsonProperty(PropertyName = "logId")]
        public double LogId { get; set; }

        [JsonProperty(PropertyName = "logType")]
        public string LogType { get; set; }

        [JsonProperty(PropertyName = "manualValuesSpecified")]
        public ManualValuesSpecified ManualValuesSpecified { get; set; }

        [JsonProperty(PropertyName = "originalDuration")]
        public double OriginalDuration { get; set; }

        [JsonProperty(PropertyName = "originalStartTime")]
        public DateTime OriginalStartTime { get; set; }

        [JsonProperty(PropertyName = "source")]
        public Source Source { get; set; }

        [JsonProperty(PropertyName = "speed")]
        public double Speed { get; set; }

        [JsonProperty(PropertyName = "startTime")]
        public DateTime StartTime { get; set; }
        
        //public int steps { get; set; }
        //public List<HeartRateZone> heartRateZones { get; set; }

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

    public class Source
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
