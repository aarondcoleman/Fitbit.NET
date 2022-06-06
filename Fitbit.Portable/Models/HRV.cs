namespace Fitbit.Models
{
    public class HRV
    {
        public double RMSSD { get; set; } //The Root Mean Square of Successive Differences(RMSSD) between heart beats. It measures short-term variability in the user’s heart rate in milliseconds (ms).

        public double Coverage { get; set; } //Data completeness in terms of the number of interbeat intervals.

        public double HF { get; set; } //The power in interbeat interval fluctuations within the high frequency band(0.15 Hz - 0.4 Hz).

        public double LF { get; set; } //The power in interbeat interval fluctuations within the low frequency band(0.04 Hz - 0.15 Hz).
    }
}
