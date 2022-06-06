using System;
using System.Collections.Generic;

namespace Fitbit.Models
{
    public class HrvIntraday
    {
        public List<HrvIntradayData> Minutes { get; set; }
        public DateTime DateTime { get; set; }
    }
}
