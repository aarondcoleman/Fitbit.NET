using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fitbit.Api.Portable
{
    internal class JsonDotNetSerializer
    {
        private readonly JsonSerializer _jsonSerializer;

        public JsonDotNetSerializer()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new EmptyDateToMinDateConverter());
            _jsonSerializer = JsonSerializer.CreateDefault(settings);
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
            return result;
        }
    }
}