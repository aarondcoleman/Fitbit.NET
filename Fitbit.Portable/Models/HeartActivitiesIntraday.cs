using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fitbit.Models
{
    public class HeartActivitiesIntraday
    {
        public IntradayActivitiesHeart ActivitiesHeart { get; set; }
        public List<DatasetInterval> Dataset { get; set; }
        public int DatasetInterval { get; set; }
        public string DatasetType { get; set; }
    }

    public class DatasetInterval
    {
        public DateTime Time { get; set; }
        public int Value { get; set; }
    }

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


    public class HeartActivitiesIntradayConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var heartActivitiesIntraday = value as HeartActivitiesIntraday;

            //{
            writer.WriteStartObject();

            writer.WritePropertyName("ActivitiesHeart");
            writer.WriteValue(heartActivitiesIntraday.ActivitiesHeart);

            // "DatasetInterval" : "1"
            writer.WritePropertyName("DatasetInterval");
            writer.WriteValue(heartActivitiesIntraday.DatasetInterval);

            // "DatasetType" : "SecondsHeartrate"
            writer.WritePropertyName("DatasetType");
            writer.WriteValue(heartActivitiesIntraday.DatasetType);

            writer.WritePropertyName("Dataset");
            writer.WriteStartArray();
            foreach (var datasetInverval in heartActivitiesIntraday.Dataset)
            {
                // "Time" : "2008-09-22T14:01:54.9571247Z"
                writer.WritePropertyName("Time");
                writer.WriteValue(datasetInverval.Time.ToString("o"));

                // "Value": 1
                writer.WritePropertyName("Value");
                writer.WriteValue(datasetInverval.Value);

            }
            writer.WriteEndArray();

            //}
            writer.WriteEndObject();

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            var properties = jsonObject.Properties().ToList();

            HeartActivitiesIntraday result = new HeartActivitiesIntraday();
            result.DatasetInterval = Convert.ToInt32(jsonObject["DatasetInterval"]);
            result.DatasetType = jsonObject["DatasetType"].ToString();
            result.Dataset = new List<DatasetInterval>();

            foreach (JToken item in jsonObject["Dataset"].Children())
            {
                result.Dataset.Add(new DatasetInterval()
                {
                    Time = DateTime.Parse(item["Time"].ToString()),
                    Value = Convert.ToInt32(item["Value"])
                });
            };

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(HeartActivitiesIntraday).IsAssignableFrom(objectType);
        }
    }
}