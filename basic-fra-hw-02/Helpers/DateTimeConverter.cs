using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace basic_fra_hw_02.Helpers
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string EuropeanFormat = "dd/MM/yyyy , HH:mm";

        // Deserialize: Read a string value and convert to DateTime
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var input = reader.GetString();

            // Attempt ISO 8601 parsing first (Swagger default)
            if (DateTime.TryParse(input, out var result))
            {
                return result;
            }

            // If the input isn't ISO 8601, attempt European format
            return DateTime.ParseExact(input ?? string.Empty, EuropeanFormat, null);
        }

        // Serialize: Convert DateTime to a formatted string
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(EuropeanFormat));
        }
    }
}
