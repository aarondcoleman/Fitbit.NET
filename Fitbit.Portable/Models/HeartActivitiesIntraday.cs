using System;
using System.Collections.Generic;
using System.Linq;
using Fitbit.Api.Portable.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fitbit.Models
{
    public class HeartActivitiesIntraday
    {
        public IntradayActivitiesHeart ActivitiesHeart { get; set; }
        public List<DatasetInterval> Dataset { get; set; }
        public int DatasetInterval { get; set; }
        public string DatasetType { get; set; }
    }
}