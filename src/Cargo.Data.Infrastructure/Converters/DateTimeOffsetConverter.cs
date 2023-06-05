using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cargo.Data.Infrastructure.Converters;

public class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public static readonly string DateTimeFormat = "MM-dd-yyyy HH:mm:ss";

    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        DateTimeOffset.ParseExact(
            reader.GetString() ?? string.Empty,
            DateTimeFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal);

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateTimeFormat));
    }
}