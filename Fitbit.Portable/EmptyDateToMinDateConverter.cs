using System;
using Newtonsoft.Json;

namespace Fitbit.Api.Portable
{
    internal class EmptyDateToMinDateConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime date = (DateTime)value;
            if (date == DateTime.MinValue)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteValue(date);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (CanConvert(objectType))
            {
                if (string.IsNullOrWhiteSpace(reader.Value.ToString()))
                {
                    return DateTime.MinValue;
                }

                return DateTime.Parse(reader.Value.ToString());
            }
            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime));
        }
    }
}
