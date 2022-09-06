namespace Fitbit.Models
{
    public class HrvSummary
    {
        public double DailyRMSSD { get; set; } //The Root Mean Square of Successive Differences(RMSSD) between heart beats. It measures short-term variability in the user’s daily heart rate in milliseconds (ms).
        public double DeepRMSSD { get; set; } //The Root Mean Square of Successive Differences(RMSSD) between heart beats. It measures short-term variability in the user’s heart rate while in deep sleep, in milliseconds (ms).
    }
}
