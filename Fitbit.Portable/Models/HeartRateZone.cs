using Newtonsoft.Json;

namespace Fitbit.Models
{
    public class HeartRateZone
    {
        [JsonProperty(PropertyName = "caloriesOut")]
        public float CaloriesOut { get; set; }

        [JsonProperty(PropertyName = "max")]
        public int Max { get; set; }

        [JsonProperty(PropertyName = "min")]
        public int Min { get; set; }

        [JsonProperty(PropertyName = "minutes")]
        public int Minutes { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
