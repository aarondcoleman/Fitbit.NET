using Fitbit.Api.Portable;

namespace Fitbit.Models
{
    /// <summary>
    /// All goal periods that can be queried from fitbit
    /// </summary>
    public enum GoalPeriod
    {
        [StringValue("daily")]
        Daily,
        [StringValue("weekly")]
        Weekly
    }
}