using System;

namespace Fitbit.Api.Portable.Models
{

    /// <summary>
    /// When calling sleep logs by a single date this is the objects that are returned
    /// </summary>
    public class RootSleepLogDate
    {
        public SleepLogDate[] Sleep { get; set; }
        public Summary Summary { get; set; }
    }

    /// <summary>
    /// Primary Object One, the sleep log date data
    /// </summary>
    public class SleepLogDate
    {
        public string DateOfSleep { get; set; }
        public int Duration { get; set; }
        public int Efficiency { get; set; }
        public bool IsMainSleep { get; set; }
        public Levels Levels { get; set; }
        public long LogId { get; set; }
        public int MinutesAfterWakeup { get; set; }
        public int MinutesAsleep { get; set; }
        public int MinutesAwake { get; set; }
        public int MinutesToFallAsleep { get; set; }
        public DateTime StartTime { get; set; }
        public int TimeInBed { get; set; }
        public string Type { get; set; }
    }

    public class Levels
    {
        public SummaryOfSleepTypes Summary { get; set; }
        public LevelsData[] Data { get; set; }
        public LevelsShortdata[] ShortData { get; set; }
    }

    public class SummaryOfSleepTypes
    {
        public Deep Deep { get; set; }
        public Light Light { get; set; }
        public Rem Rem { get; set; }
        public Wake Wake { get; set; }
        public Asleep Asleep { get; set; }
        public Awake Awake { get; set; }
        public Restless Restless { get; set; }
    }

    public class Deep
    {
        public int Count { get; set; }
        public int Minutes { get; set; }
        public int ThirtyDayAvgMinutes { get; set; }
    }

    public class Light
    {
        public int Count { get; set; }
        public int Minutes { get; set; }
        public int ThirtyDayAvgMinutes { get; set; }
    }

    public class Rem
    {
        public int Count { get; set; }
        public int Minutes { get; set; }
        public int ThirtyDayAvgMinutes { get; set; }
    }

    public class Wake
    {
        public int Count { get; set; }
        public int Minutes { get; set; }
        public int ThirtyDayAvgMinutes { get; set; }
    }

    public class Asleep
    {
        public int Count { get; set; }
        public int Minutes { get; set; }
    }

    public class Awake
    {
        public int Count { get; set; }
        public int Minutes { get; set; }
    }

    public class Restless
    {
        public int Count { get; set; }
        public int Minutes { get; set; }
    }

    public class LevelsData
    {
        public DateTime Datetime { get; set; }
        public string Level { get; set; }
        public int Seconds { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class LevelsShortdata
    {
        public DateTime Datetime { get; set; }
        public string Level { get; set; }
        public int Seconds { get; set; }
    }

    /// <summary>
    /// Secondary object that is returned from the api call
    /// </summary>
    public class Summary
    {
        public int TotalMinutesAsleep { get; set; }
        public int TotalSleepRecords { get; set; }
        public int TotalTimeInBed { get; set; }
    }

}