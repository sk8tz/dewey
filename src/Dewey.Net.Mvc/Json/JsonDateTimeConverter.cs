using System;
using Dewey.Net.Temporal;
using Newtonsoft.Json;

namespace Dewey.Net.Mvc.Json
{
    public class JsonDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == null) {
                return false;
            }

            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader == null || reader.Value == null) {
                return null;
            }

            var dateTime = long.Parse(reader.Value.ToString());

            return DateTimeUtils.FromMillis(dateTime);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long result = DateTimeUtils.GetValue((DateTime)value);

            writer.WriteValue(result);
        }
    }
}