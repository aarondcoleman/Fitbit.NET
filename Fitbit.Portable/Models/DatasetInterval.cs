using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Api.Portable.Models
{
    //[JsonConverter(typeof(DatasetIntervalConverter))]
    public class DatasetInterval
    {
        public DateTime Time { get; set; }
        public int Value { get; set; }
    }
}
