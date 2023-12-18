using System.Collections.Generic;

namespace SEServer.Data.Interface;

public interface IConfigTable : IService
{
    public T? Get<T>(string key) where T : IConfigData;
    public IEnumerable<T> GetAll<T>() where T : IConfigData;
    public int GetIndexByData<T>(T data) where T : IConfigData;
    public T GetDataByIndex<T>(int index) where T : IConfigData;
}