using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Retaguarda.DTO.Converters
{
    /// <summary>
    /// Aceita string numérica, número JSON ou vazio/nulo para long? — comportamento similar ao Newtonsoft.
    /// </summary>
    public class FlexibleNullableLongConverter : JsonConverter<long?>
    {
        public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt64(out var l)) return l;
                if (reader.TryGetDouble(out var d)) return (long)d;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                var s = reader.GetString();
                if (string.IsNullOrWhiteSpace(s)) return null;
                if (long.TryParse(s, out var parsed)) return parsed;
                if (double.TryParse(s, System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out var dbl))
                    return (long)dbl;
                throw new JsonException($"Valor '{s}' não é um número inteiro válido.");
            }

            throw new JsonException($"Token inesperado '{reader.TokenType}' para campo numérico.");
        }

        public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
        {
            if (value.HasValue) writer.WriteNumberValue(value.Value);
            else writer.WriteNullValue();
        }
    }
}
