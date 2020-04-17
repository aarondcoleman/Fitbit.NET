using Fitbit.Api.Portable;
using System;

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
        [Obsolete("No longer supported by Fitbit. See https://github.com/aarondcoleman/Fitbit.NET/issues/283")]
        [StringValue("3m")]
        ThreeMonths,
        [Obsolete("No longer supported by Fitbit. See https://github.com/aarondcoleman/Fitbit.NET/issues/283")]
        [StringValue("6m")]
        SixMonths,
        [Obsolete("No longer supported by Fitbit. See https://github.com/aarondcoleman/Fitbit.NET/issues/283")]
        [StringValue("1y")]
        OneYear,
        [Obsolete("No longer supported by Fitbit. See https://github.com/aarondcoleman/Fitbit.NET/issues/283")]
        [StringValue("max")]
        Max
    }
}