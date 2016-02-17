using System;
using System.Collections.Generic;

namespace Fitbit.Models
{
    public class SleepLog
    {
        public int AwakeningsCount { get; set; }
        public int Duration { get; set; }
        public int Efficiency { get; set; }
        public bool IsMainSleep { get; set; }
        public long LogId { get; set; }
        public List<MinuteData> MinuteData { get; set; }
        public int MinutesAfterWakeup { get; set; }
        public int MinutesAsleep { get; set; }
        public int MinutesAwake { get; set; }
        public int MinutesToFallAsleep { get; set; }
        public DateTime StartTime { get; set; }
        public int TimeInBed { get; set; }
        public int AwakeCount { get; set; }
        public int AwakeDuration { get; set; }
        public int RestlessCount { get; set; }
        public int RestlessDuration { get; set; }
    }
}
