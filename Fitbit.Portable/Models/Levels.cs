namespace Fitbit.Api.Portable.Models
{
    public class Levels
    {
        public SummaryOfSleepTypes Summary { get; set; }
        public LevelsData[] Data { get; set; }
        public LevelsShortdata[] ShortData { get; set; }
    }
}