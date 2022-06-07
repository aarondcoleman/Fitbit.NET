using Newtonsoft.Json;

namespace Fitbit.Models
{
    public class BreathingRate
    {
        [JsonProperty("breathingRate")]
        public double Rate { get; set; }
    }
}
