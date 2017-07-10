namespace Fitbit.Api.Portable.Models
{
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
}