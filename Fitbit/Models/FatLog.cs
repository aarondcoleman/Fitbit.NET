using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Models
{
    public class FatLog
    {
        public long LogId { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public float Fat { get; set; }

        public DateTime DateTime { get { return Date.Date.Add(Time.TimeOfDay); } }
    }
}
