using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SEServer.Data;

public static class ConfigStaticDeserializer
{
    static ConfigStaticDeserializer()
    {
        JsonSerializerSettings.Converters = new List<JsonConverter>()
        {
            new SVector2Converter(),
        };
        JsonSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
        JsonSerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
        
        JsonSerializer = JsonSerializer.Create(JsonSerializerSettings);
    }
    
    private static JsonSerializerSettings JsonSerializerSettings { get; set; } = new JsonSerializerSettings();
    private static JsonSerializer JsonSerializer { get; set; }
    
    public static object? ParseJson(JToken json, Type type)
    {
        return json.ToObject(type, JsonSerializer);
    }
    
    public static T? ParseJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings);
    }
    
    public static object? ParseJson(string json, Type type)
    {
        return JsonConvert.DeserializeObject(json, type, JsonSerializerSettings);
    }
}