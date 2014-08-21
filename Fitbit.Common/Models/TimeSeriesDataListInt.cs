using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Models
{
    public class TimeSeriesDataListInt
    {
        public List<Data> DataList { get; set; }

        public class Data
        {
            public DateTime DateTime { get; set; }
            public int Value { get; set; }
        }

    }

//    public class DataValue
//    {
//        public TimeSeriesData Data { get; set; }
//    }

    


}
