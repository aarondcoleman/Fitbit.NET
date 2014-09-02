using Newtonsoft.Json.Linq;

namespace Fitbit.Api.Portable
{
    internal class JsonDotNetSerializer
    {
        /// <summary>
        /// Root property value; only required if trying to access nested information or an array is hanging off a property
        /// </summary>
        internal string RootProperty { get; set; }

        internal T Deserialize<T>(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
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

            T result = string.IsNullOrWhiteSpace(RootProperty) ? token.ToObject<T>() : token[RootProperty].ToObject<T>();
            return result;            
        }
    }
}