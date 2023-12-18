using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SEServer.Data.Interface;

namespace SEServer.Data;

public class ConfigTable : IConfigTable
{
    public ServerContainer ServerContainer { get; set; } = null!;
    
    public delegate string LoadConfigDelegate(string path);
    public delegate string[] LoadAllConfigDelegate(string path);
    
    public List<ConfigTableName> ConfigTableNames { get; set; } = new List<ConfigTableName>();
    
    public LoadConfigDelegate LoadConfig { get; set; }
    public LoadAllConfigDelegate LoadAllConfig { get; set; }
    
    public Dictionary<Type, ConfigDataCollection> ConfigDataCollections { get; set; } = new Dictionary<Type, ConfigDataCollection>();

    public ConfigTable(LoadConfigDelegate loadConfig, LoadAllConfigDelegate loadAllConfig)
    {
        LoadConfig = loadConfig;
        LoadAllConfig = loadAllConfig;
    }

    public void Init()
    {

    }

    private string[] LoadConfigs(string folderPath)
    {
        return LoadAllConfig(folderPath);
    }

    public void Start()
    {
        foreach (var tableName in ConfigTableNames)
        {
            if (tableName.IsFolder)
            {
                // 读取文件夹下所有文件
                var jsons = LoadConfigs(tableName.Path);
                var configDataCollection = GetConfigDataCollection(tableName.DataType);
                foreach (var json in jsons)
                {
                    var data = ConfigStaticDeserializer.ParseJson(json, tableName.DataType);
                    if (data is IConfigData configData)
                    {
                        configDataCollection.Add(configData);
                    
                        if (configData is IConfigDataInit configDataInit)
                        {
                            configDataInit.OnInit();
                        }
                    }
                }
            }
            else
            {
                // 读取单个文件
                var json = LoadConfig(tableName.Path);
                var configDataCollection = GetConfigDataCollection(tableName.DataType);
                var dataList = JArray.Parse(json);
                foreach (var data in dataList)
                {
                    if (ConfigStaticDeserializer.ParseJson(data, tableName.DataType) is IConfigData configData)
                    {
                        configDataCollection.Add(configData);
                    
                        if (configData is IConfigDataInit configDataInit)
                        {
                            configDataInit.OnInit();
                        }
                    }
                }
            }
        }
    }

    public void Stop()
    {
        
    }
    
    private ConfigDataCollection GetConfigDataCollection(Type tableType)
    {
        if (ConfigDataCollections.TryGetValue(tableType, out var collection))
        {
            return collection;
        }
        
        collection = new ConfigDataCollection();
        ConfigDataCollections.Add(tableType, collection);
        return collection;
    }

    public T? Get<T>(string key) where T : IConfigData
    {
        var collection = GetConfigDataCollection(typeof(T));
        return collection.Get<T>(key);
    }

    public IEnumerable<T> GetAll<T>() where T : IConfigData
    {
        var collection = GetConfigDataCollection(typeof(T));
        return collection.GetAll<T>();
    }

    public int GetIndexByData<T>(T data) where T : IConfigData
    {
        var collection = GetConfigDataCollection(typeof(T));
        return collection.GetIndexByData<T>(data);
    }

    public T GetDataByIndex<T>(int index) where T : IConfigData
    {
        var collection = GetConfigDataCollection(typeof(T));
        return collection.GetDataByIndex<T>(index);
    }
}