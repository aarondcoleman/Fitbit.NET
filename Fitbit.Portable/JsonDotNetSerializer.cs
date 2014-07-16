using Newtonsoft.Json.Linq;

namespace Fitbit.Api.Portable
{
    internal class JsonDotNetSerializer
    {
        public string RootProperty { get; set; }

        public T Deserialize<T>(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return default(T);
            }

            JObject o = JObject.Parse(data);
            if (o == null)
            {
                return default(T);
            }

            T result = string.IsNullOrWhiteSpace(RootProperty) ? o.ToObject<T>() : o[RootProperty].ToObject<T>();
            return result;
        }
    }
}