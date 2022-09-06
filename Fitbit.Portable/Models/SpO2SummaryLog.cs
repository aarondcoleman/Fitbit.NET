using System;

namespace Fitbit.Models
{
    public class SpO2SummaryLog
    {
        public DateTime DateTime { get; set; }
        public SpO2Summary Value { get; set; }
    }
}
