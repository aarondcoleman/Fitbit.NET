namespace Fitbit.Api.Portable.Models
{
    public class SleepDateRangeLevels
    {
        public SleepDateRangeSummary Summary { get; set; }
        public LevelsShortdata[] Data { get; set; }
        public LevelsShortdata[] ShortData { get; set; }
    }
}