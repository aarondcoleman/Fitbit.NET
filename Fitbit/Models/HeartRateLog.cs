using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitbit.Models
{
    public class HeartRateLog
    {
        [SerializeAs(Name="heartRate")]
        public int HeartRate { get; set; }
        [SerializeAs(Name = "logId")]
        public int LogId { get; set; }
        public DateTime Time { get; set; }
        public string Tracker { get; set; }
    }
}
