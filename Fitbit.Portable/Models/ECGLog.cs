using System;

namespace Fitbit.Models
{
    public class ECGLog
    {
        public DateTime StartTime { get; set; }
        public int AverageHeartRate { get; set; }
        public string ResultClassification { get; set; }
        public int[] WaveFormSamples { get; set; }
        public string SamplingFrequencyHz { get; set; }
        public int ScalingFactor { get; set; }
        public int NumberOfWaveformSamples { get; set; }
        public int LeadNumber { get; set; }
        public string FeatureVersion { get; set; }
        public string DeviceName { get; set; }
        public string FirmwareVersion { get; set; }
    }
}
