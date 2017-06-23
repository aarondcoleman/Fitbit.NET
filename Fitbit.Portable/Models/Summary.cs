namespace Fitbit.Api.Portable.Models
{
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