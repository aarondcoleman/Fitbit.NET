using System;

namespace Fitbit.Models
{
    public class WeightLog
    {
        public long LogId { get; set; }
        public float Bmi { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public float Weight { get; set; }

        public DateTime DateTime { get { return Date.Date.Add(Time.TimeOfDay); } }
    }
}