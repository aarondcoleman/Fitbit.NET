using Newtonsoft.Json;

namespace Fitbit.Models
{
    public class BestTotals
    {
        [JsonProperty("caloriesOut")]
        public CaloriesOutStats CaloriesOut { get; set; }

        [JsonProperty("distance")]
        public DistanceStats Distance { get; set; }

        [JsonProperty("floors")]
        public FloorStats Floors { get; set; }

        [JsonProperty("steps")]
        public StepStats Steps { get; set; }
    }
}