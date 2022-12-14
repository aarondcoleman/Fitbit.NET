using Fitbit.Api.Portable;

namespace Fitbit.Models
{
    public enum DataResolution
    {
        [StringValue("1sec")]
        OneSecond = 0,

        [StringValue("1min")]
        OneMinute = 1,

        [StringValue("5min")]
        FiveMinute = 2,

        [StringValue("15min")]
        FifteenMinute = 3
    }
}
