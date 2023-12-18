using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SEServer.Data;

public static class ConfigStaticSerializer
{
    static ConfigStaticSerializer()
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

    public static string SerializeJson(object obj)
    {
        return JsonConvert.SerializeObject(obj, JsonSerializerSettings);
    }

    public static JToken SerializeJsonToJToken(object obj)
    {
        return JToken.FromObject(obj, JsonSerializer.Create(JsonSerializerSettings));
    }
}