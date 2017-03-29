using System;
using System.Collections.Generic;

namespace Fitbit.Models
{
    public class IntradayDataValues
    {
        public DateTime Time { get; set; }
        public string Value { get; set; }
        public string Level { get; set; }
        public string METs { get; set; }
    }

    public class IntradayData
    {
        public List<IntradayDataValues> DataSet { get; set; }
    }

}
