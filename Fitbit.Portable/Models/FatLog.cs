using System;

namespace Fitbit.Models
{
    //https://wiki.fitbit.com/display/API/API-Get-Body-Fat
    public class FatLog
    {
        public long LogId { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public float Fat { get; set; }

        public DateTime DateTime { get { return Date.Date.Add(Time.TimeOfDay); } }
    }
}