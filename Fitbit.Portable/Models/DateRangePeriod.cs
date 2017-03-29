using Fitbit.Api.Portable;

namespace Fitbit.Models
{
    /// <summary>
    /// All date ranges that can be queried from fitbit
    /// </summary>
    public enum DateRangePeriod
    {
        //1d, 7d, 30d, 1w, 1m, 3m, 6m, 1y, max

        [StringValue("1d")]
        OneDay,
        [StringValue("7d")]
        SevenDays,
        [StringValue("30d")]
        ThirtyDays,
        [StringValue("1w")]
        OneWeek,
        [StringValue("1m")]
        OneMonth,
        [StringValue("3m")]
        ThreeMonths,
        [StringValue("6m")]
        SixMonths,
        [StringValue("1y")]
        OneYear,
        [StringValue("max")]
        Max
    }
}