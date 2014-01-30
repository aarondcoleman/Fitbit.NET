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
        public int heartRate { get; set; }
        public int logId { get; set; }
        public DateTime time { get; set; }
        public string tracker { get; set; }
    }
}
