using System.Collections.Generic;

namespace Fitbit.Models
{
    public class SleepData
    {
        public List<SleepLog> Sleep { get; set; }
        public SleepSummary Summary { get; set; }
    }
}