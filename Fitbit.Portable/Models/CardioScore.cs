using Newtonsoft.Json;

namespace Fitbit.Models
{
    public class CardioScore
    {
        [JsonProperty("vo2Max")]
        public string VO2Max { get; set; }
    }
}