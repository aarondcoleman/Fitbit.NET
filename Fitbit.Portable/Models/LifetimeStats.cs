using Newtonsoft.Json;

namespace Fitbit.Api.Portable.Models
{
    public class LifetimeStats
    {
        [JsonProperty("total")]
        public LifetimeTotals Total { get; set; }

        [JsonProperty("tracker")]
        public LifetimeTotals Tracker { get; set; }
    }
}