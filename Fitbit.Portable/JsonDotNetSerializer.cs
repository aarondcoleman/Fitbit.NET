using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Fitbit.Api.Portable
{
    internal class JsonDotNetSerializer
    {
        /// <summary>
        /// Root property value; only required if trying to access nested information or an array is hanging off a property
        /// </summary>
        public string RootProperty { get; set; }

        public T Deserialize<T>(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return default(T);
            }

            // use the generic JToken to allow for any data structure to be processed including arrays
            JToken o = JToken.Parse(data);

            if (o == null)
            {
                return default(T);
            }

            T result = string.IsNullOrWhiteSpace(RootProperty) ? o.ToObject<T>() : o[RootProperty].ToObject<T>();
            return result;
        }
    }
}