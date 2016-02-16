using Newtonsoft.Json;

namespace Fitbit.Api.Portable.Models
{
    public class BestStats
    {
        [JsonProperty("total")]
        public BestTotals Total { get; set; }

        [JsonProperty("tracker")]
        public BestTotals Tracker { get; set; }
    }
}