using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Retaguarda.Api.Converters
{
    /// <summary>
    /// Conversor JSON customizado que aceita múltiplos formatos de data (ISO 8601, brasileiro, etc.)
    /// </summary>
    public class FlexibleDateTimeConverter : JsonConverter<DateTime>
    {
        private static readonly string[] DateFormats = new[]
        {
            // ISO 8601 (padrão)
            "yyyy-MM-ddTHH:mm:ss.fffZ",
            "yyyy-MM-ddTHH:mm:ss.fff",
            "yyyy-MM-ddTHH:mm:ssZ",
            "yyyy-MM-ddTHH:mm:ss",
            
            // Apenas data (ISO)
            "yyyy-MM-dd",
            
            // Formato brasileiro
            "dd/MM/yyyy HH:mm:ss",
            "dd/MM/yyyy",
            
            // Variações
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd HH:mm:ss.fff"
        };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    return default;
                }

                foreach (var format in DateFormats)
                {
                    if (DateTime.TryParseExact(stringValue, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out var dateTime))
                    {
                        return dateTime;
                    }
                }

                // Tenta parsing padrão do .NET como último recurso
                if (DateTime.TryParse(stringValue, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out var defaultParsedDate))
                {
                    return defaultParsedDate;
                }

                throw new JsonException($"Formato de data não suportado: '{stringValue}'. Formatos aceitos: ISO 8601, dd/MM/yyyy, yyyy-MM-dd");
            }

            // Se for número (timestamp Unix)
            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt64(out var unixTime))
                {
                    return UnixTimeStampToDateTime(unixTime);
                }
            }

            throw new JsonException($"Tipo de token JSON não esperado: {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Escreve em ISO 8601 (formato padrão)
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
        }

        private static DateTime UnixTimeStampToDateTime(long unixTime)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTime);
            return dateTime;
        }
    }

    /// <summary>
    /// Conversor para DateTime nullable
    /// </summary>
    public class FlexibleNullableDateTimeConverter : JsonConverter<DateTime?>
    {
        private static readonly FlexibleDateTimeConverter DateTimeConverter = new();

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            return DateTimeConverter.Read(ref reader, typeof(DateTime), options);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                DateTimeConverter.Write(writer, value.Value, options);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
