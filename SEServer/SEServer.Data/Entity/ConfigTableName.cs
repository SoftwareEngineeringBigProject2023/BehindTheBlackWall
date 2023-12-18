using System;

namespace SEServer.Data;

public class ConfigTableName
{
    public string Path { get; }
    public Type DataType { get; }
    public bool IsFolder { get; }
    
    public ConfigTableName(string path, Type dataType, bool isFolder = false)
    {
        Path = path;
        DataType = dataType;
        IsFolder = isFolder;
    }
}