using System;
using System.Collections.Generic;
using SEServer.Data.Interface;

namespace SEServer.Data;

public class ConfigDataCollection
{
    public Type ConfigDataType { get; set; } = null!;
    public List<IConfigData> ConfigData { get; set; } = new List<IConfigData>();
    public Dictionary<string, IConfigData> ConfigDataDict { get; set; } = new Dictionary<string, IConfigData>();

    public void Add(IConfigData configData)
    {
        if (ConfigDataDict.ContainsKey(configData.PrimaryKey))
        {
            ConfigData.Remove(ConfigDataDict[configData.PrimaryKey]);
            ConfigDataDict.Remove(configData.PrimaryKey);
        }
        
        ConfigData.Add(configData);
        ConfigDataDict.Add(configData.PrimaryKey, configData);
    }
    
    public void Remove(IConfigData configData)
    {
        if (ConfigDataDict.ContainsKey(configData.PrimaryKey))
        {
            ConfigData.Remove(ConfigDataDict[configData.PrimaryKey]);
            ConfigDataDict.Remove(configData.PrimaryKey);
        }
    }
    
    public IConfigData? Get(string primaryKey)
    {
        if (ConfigDataDict.TryGetValue(primaryKey, out var value))
        {
            return value;
        }
        
        return null;
    }
    
    public IEnumerable<IConfigData> GetAll()
    {
        return ConfigData;
    }
    
    public T? Get<T>(string primaryKey) where T : IConfigData
    {
        if (ConfigDataDict.TryGetValue(primaryKey, out var value))
        {
            return (T)value;
        }
        
        return default;
    }
    
    public IEnumerable<T> GetAll<T>() where T : IConfigData
    {
        foreach (var configData in ConfigData)
        {
            if (configData is T t)
            {
                yield return t;
            }
        }
    }
    
    public void Clear()
    {
        ConfigData.Clear();
        ConfigDataDict.Clear();
    }

    public int GetIndexByData<T>(T data) where T : IConfigData
    {
        return ConfigData.IndexOf(data);
    }

    public T GetDataByIndex<T>(int index) where T : IConfigData
    {
        return (T)ConfigData[index];
    }
}