using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fitbit.Api.Portable
{
    internal class JsonDotNetSerializer
    {
        private readonly JsonSerializer _jsonSerializer;
        private readonly JsonSerializerSettings _settings;
        public JsonDotNetSerializer(JsonSerializerSettings settings = null)
        {
            _settings = settings ?? new JsonSerializerSettings();
            _settings.Converters.Add(new EmptyDateToMinDateConverter());
            _jsonSerializer = JsonSerializer.CreateDefault(_settings);
        }

        /// <summary>
        /// Root property value; only required if trying to access nested information or an array is hanging off a property
        /// </summary>
        internal string RootProperty { get; set; }

        internal T Deserialize<T>(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                //TO DO: Is this the behavior we want? Or should we be throwing an exception?
                return default(T);
            }

            if (_settings.DateParseHandling == DateParseHandling.DateTimeOffset )
            {
                /* Json.NET conversions do not correctly handle datetimeoffsets by default.
                 Instead they convert the offset into the local time of the server running the code.
                 This is problematic for using Fitbit data sets that include the user's timezone info.
                 In order to get around this, DateParseHandling.DateTimeOffset is set on the JsonConvert serializer.
                 Unfortunately, Jtoken.Parse does not have this ability and can not be used for this purpose.
                 On the flip side, JsonConvert does not seem to allow a RootProperty, so right now
                 only one or the other can be used.*/
                if (!string.IsNullOrWhiteSpace(RootProperty))
                {
                    var message = "Error occured by parsing JSON. Root property not compatible with DateParseHandling.DateTimeOffset";
                    throw new FitbitParseException(message);
                }
                return JsonConvert.DeserializeObject<T>(data);
            }

            JToken o = JToken.Parse(data);
            return Deserialize<T>(o);
        }

        internal T Deserialize<T>(JToken token)
        {
            if (token == null)
            {
                return default(T);
            }

            T result = string.IsNullOrWhiteSpace(RootProperty) ? token.ToObject<T>(_jsonSerializer) : token[RootProperty].ToObject<T>(_jsonSerializer);

            // T result = string.IsNullOrWhiteSpace(RootProperty) ? token.ToObject<T>(_jsonSerializer) : token[RootProperty].ToObject<T>(_jsonSerializer);
            return result;
        }
    }
}