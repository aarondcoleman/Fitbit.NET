#if REQUIRES_JSONNET
using Newtonsoft.Json;
#endif

namespace Fitbit.Models
{
    public class ApiError
    {
#if REQUIRES_JSONNET
        [JsonProperty("errorType")]
#endif
        public ErrorType ErrorType { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("fieldName")]
#endif
        public string FieldName { get; set; }

#if REQUIRES_JSONNET
        [JsonProperty("message")]
#endif
        public string Message { get; set; }
    }
}