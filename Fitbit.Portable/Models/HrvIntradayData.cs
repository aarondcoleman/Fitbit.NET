using System;

namespace Fitbit.Models
{
    public class HrvIntradayData
    {
        public DateTime Minute { get; set; }
        public HRV Value { get; set; }
    }
}
