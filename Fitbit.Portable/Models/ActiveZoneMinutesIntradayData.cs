using System;

namespace Fitbit.Models
{
    public class ActiveZoneMinutesIntradayData
    {
        public DateTime Minute { get; set; }
        public ActiveZoneMinuteValue Value { get; set; }
    }
}
