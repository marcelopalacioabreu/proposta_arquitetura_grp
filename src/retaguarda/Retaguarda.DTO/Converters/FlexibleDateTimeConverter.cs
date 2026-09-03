using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Retaguarda.DTO.Converters
{
    /// <summary>
    /// Conversor flexível para DateTime que aceita múltiplos formatos:
    /// - ISO 8601 (YYYY-MM-DDTHH:mm:ss ou YYYY-MM-DD)
    /// - DateTime-local HTML5 (YYYY-MM-DDTHH:mm)
    /// - Formato brasileiro (DD/MM/YYYY ou DD/MM/YYYY HH:mm:ss)
    /// </summary>
    public class FlexibleDateTimeConverter : JsonConverter<DateTime>
    {
        private static readonly string[] DateTimeFormats = new[]
        {
            // ISO 8601 completo
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss.fffffff",
            "yyyy-MM-ddTHH:mm:ss.ffffff",
            "yyyy-MM-ddTHH:mm:ss.fff",
            
            // ISO 8601 sem hora
            "yyyy-MM-dd",
            
            // HTML5 datetime-local
            "yyyy-MM-ddTHH:mm",
            
            // Formato brasileiro
            "dd/MM/yyyy HH:mm:ss",
            "dd/MM/yyyy HH:mm",
            "dd/MM/yyyy",
            
            // Outros formatos comuns
            "dd-MM-yyyy",
            "MM/dd/yyyy",
            "yyyy/MM/dd"
        };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return default;

            if (reader.TokenType == JsonTokenType.String)
            {
                string? stringValue = reader.GetString();
                
                if (string.IsNullOrWhiteSpace(stringValue))
                    return default;

                // Tenta cada formato
                foreach (var format in DateTimeFormats)
                {
                    if (DateTime.TryParseExact(stringValue, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
                        return result;
                }

                // Tenta parse geral como fallback
                if (DateTime.TryParse(stringValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out var generalResult))
                    return generalResult;

                throw new JsonException($"Não foi possível converter '{stringValue}' para DateTime. Formatos aceitos: YYYY-MM-DD ou YYYY-MM-DDTHH:mm:ss ou DD/MM/YYYY");
            }

            throw new JsonException($"Esperado string para DateTime, mas recebeu {reader.TokenType}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Sempre escreve em ISO 8601 completo
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
        }
    }

    /// <summary>
    /// Conversor para Nullable DateTime
    /// </summary>
    public class FlexibleNullableDateTimeConverter : JsonConverter<DateTime?>
    {
        private readonly FlexibleDateTimeConverter _innerConverter = new();

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            return _innerConverter.Read(ref reader, typeof(DateTime), options);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                _innerConverter.Write(writer, value.Value, options);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
