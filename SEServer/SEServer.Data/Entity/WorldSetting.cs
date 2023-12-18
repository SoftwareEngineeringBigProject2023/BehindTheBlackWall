using System.Collections.Generic;

namespace SEServer.Data;

public class WorldSetting
{
    private Dictionary<string, string> _data = new();
    
    public string this[string key]
    {
        get => _data[key];
        set => _data[key] = value;
    }
}