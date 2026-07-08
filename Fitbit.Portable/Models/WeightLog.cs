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

        // Origin of the log entry. "API" = manually entered (app/web, 3rd-party API write, or a
        // manually-edited scale reading); connected scales report "Aria"/"AriaAir"/"Withings".
        // https://dev.fitbit.com/build/reference/web-api/body/get-weight-log/
        public string Source { get; set; }

        public DateTime DateTime { get { return Date.Date.Add(Time.TimeOfDay); } }
    }
}