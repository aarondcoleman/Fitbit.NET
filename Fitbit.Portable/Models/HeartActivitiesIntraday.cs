using System;
using System.Collections.Generic;
using System.Linq;
using Fitbit.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#if NETSTANDARD1_3
using System.Reflection;
#endif

namespace Fitbit.Api.Portable.Models
{
    public class HeartActivitiesIntraday
    {
        public IntradayActivitiesHeart ActivitiesHeart { get; set; }
        public List<DatasetInterval> Dataset { get; set; }
        public int DatasetInterval { get; set; }
        public string DatasetType { get; set; }
    }
}
