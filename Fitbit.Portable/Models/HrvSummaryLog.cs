using System;

namespace Fitbit.Models
{
    public class HrvSummaryLog
    {
        public DateTime DateTime { get; set; }
        public HrvSummary Value { get; set; }
    }
}
