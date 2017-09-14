using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Fitbit.Api.Portable.Models
{
    public class ManualValuesSpecified
    {
        [JsonProperty(PropertyName = "calories")]
        public bool Calories { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public bool Distance { get; set; }

        [JsonProperty(PropertyName = "steps")]
        public bool Steps { get; set; }
    }
}
