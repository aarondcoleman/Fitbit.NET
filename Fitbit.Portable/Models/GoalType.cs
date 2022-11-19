using Fitbit.Api.Portable;

namespace Fitbit.Models
{
    /// <summary>
    /// All goal data types that can be queried from fitbit
    /// </summary>
    public enum GoalType
    {
        [StringValue("activeMinutes")]
        ActiveMinutes,
        [StringValue("activeZoneMinutes")]
        ActiveZoneMinutes,
        [StringValue("caloriesOut")]
        CaloriesOut,
        [StringValue("distance")]
        Distance,
        [StringValue("floors")]
        Floors,
        [StringValue("steps")]
        Steps
    }
}
