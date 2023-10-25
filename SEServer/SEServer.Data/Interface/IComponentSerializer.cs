using System;
using System.Collections.Generic;

namespace SEServer.Data;

public interface IComponentSerializer : IService
{
    Type GetTypeByCode(int code);
    int GetCodeByType(Type type);
    ComponentArrayDataPack Serialize<T>(List<T> components) where T : IComponent;
    List<T> Deserialize<T>(ComponentArrayDataPack dataPack) where T : IComponent;
}