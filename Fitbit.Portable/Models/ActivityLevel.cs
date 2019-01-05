using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fitbit.Api.Portable.Models
{
    public class ActivityLevel
    {
        [JsonProperty(PropertyName = "minutes")]
        public int Minutes { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
