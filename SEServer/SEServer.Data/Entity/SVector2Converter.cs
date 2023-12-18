using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SEServer.Data;

public class SVector2Converter : JsonConverter<SVector2>
{
    public override void WriteJson(JsonWriter writer, SVector2 value, JsonSerializer serializer)
    {
        var str = value.ToString();
        writer.WriteValue(str);
    }

    public override SVector2 ReadJson(JsonReader reader, Type objectType, SVector2 existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return SVector2.Zero;

        var jToken = JToken.Load(reader);
        var value = jToken.Value<string>();
        if (value == null)
        {
            throw new JsonSerializationException($"Unexpected token or value when parsing SVector2. Token: {reader.TokenType}, Value: {jToken}");
        }
        
        return SVector2.Parse(value);
    }
}