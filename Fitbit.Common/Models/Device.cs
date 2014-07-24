using System;
#if REQUIRES_JSONNET
using Newtonsoft.Json;
#endif

namespace Fitbit.Models
{
    public class Device
    {
#if REQUIRES_JSONNET
        [JsonProperty("battery")]
#endif
        public string Battery { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("id")]
#endif
        public string Id { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("lastSyncTime")]
#endif
        public DateTime LastSyncTime { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("type")]
#endif
        public DeviceType Type { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("deviceVersion")]
#endif
        public string DeviceVersion { get; set; } // todo: this should be an enum; "Flex|One|Zip|Ultra|Classic|Aria"

#if REQUIRES_JSONNET
        [JsonProperty("mac")]
#endif
        public string Mac { get; set; }
    }
}