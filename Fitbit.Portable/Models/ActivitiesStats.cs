using Newtonsoft.Json;

namespace Fitbit.Api.Portable.Models
{
    public class ActivitiesStats
    {
        [JsonProperty("best")]
        public BestStats Best { get; set; }

        [JsonProperty("lifetime")]
        public LifetimeStats Lifetime { get; set; }
    }
}