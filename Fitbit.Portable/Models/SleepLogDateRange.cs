using System;

namespace Fitbit.Api.Portable.Models
{
    public class SleepLogDateRange
    {
        public DateTime DateOfSleep { get; set; }
        public int Duration { get; set; }
        public int Efficiency { get; set; }
        public bool IsMainSleep { get; set; }
        public Levels Levels { get; set; }
        public long LogId { get; set; }
        public int MinutesAfterWakeup { get; set; }
        public int MinutesAsleep { get; set; }
        public int MinutesAwake { get; set; }
        public int MinutesToFallAsleep { get; set; }
        public string LogType { get; set; }
        public DateTime StartTime { get; set; }
        public int TimeInBed { get; set; }
        public string Type { get; set; }
    }
}