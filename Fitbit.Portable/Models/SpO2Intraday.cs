using System;
using System.Collections.Generic;

namespace Fitbit.Models
{
    public class SpO2Intraday
    {
        public DateTime DateTime { get; set; }
        public List<SpO2IntradayData> Minutes { get; set; }
    }
}
