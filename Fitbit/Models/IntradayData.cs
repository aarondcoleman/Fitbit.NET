using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Models
{
    public class IntradayDataValues
    {
        public DateTime Time { get; set; }
        public string Value { get; set; }
        public string Level { get; set; }
    }

    public class IntradayData
    {
        public List<IntradayDataValues> DataSet { get; set; }
    }

    /*
    public class IntradayDataListTwo
    {
        public IntradayDataSet Set { get; set; }
    }
    */
}
