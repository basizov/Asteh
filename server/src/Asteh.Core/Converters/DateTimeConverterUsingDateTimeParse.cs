using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Asteh.Core.Converters
{
	public class DateTimeConverterUsingDateTimeParse : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            var dateTime = reader.GetString();
            return dateTime is not null
                ? DateTime.ParseExact(dateTime, "dd.MM.yyyy", CultureInfo.CurrentCulture)
                : DateTime.UtcNow;
        }

		public override void Write(
			Utf8JsonWriter writer,
			DateTime value,
			JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString("dd.MM.yyyy"));
	}
}
