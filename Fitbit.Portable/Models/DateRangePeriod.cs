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
        OneDay = 0,
        [StringValue("7d")]
        SevenDays = 1,
        [StringValue("30d")]
        ThirtyDays = 2,
        [StringValue("1w")]
        OneWeek = 3,
        [StringValue("1m")]
        OneMonth = 4,
        [Obsolete("No longer supported by Fitbit. See https://github.com/aarondcoleman/Fitbit.NET/issues/283")]
        [StringValue("3m")]
        ThreeMonths = 5,
        [Obsolete("No longer supported by Fitbit. See https://github.com/aarondcoleman/Fitbit.NET/issues/283")]
        [StringValue("6m")]
        SixMonths = 6,
        [Obsolete("No longer supported by Fitbit. See https://github.com/aarondcoleman/Fitbit.NET/issues/283")]
        [StringValue("1y")]
        OneYear = 7,
        [Obsolete("No longer supported by Fitbit. See https://github.com/aarondcoleman/Fitbit.NET/issues/283")]
        [StringValue("max")]
        Max = 8
    }
}