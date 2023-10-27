using System;
using System.Collections.Generic;
using System.Text;

namespace SEServer.Data;

/// <summary>
/// 用于序列化组件的静态类
/// </summary>
public class ComponentSerializer : IComponentSerializer
{
    public ServerContainer ServerContainer { get; set; } = null!;
    
    public void Init()
    {
        // 从所有程序集中获取 IComponent 的实现类并注册
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IComponent).IsAssignableFrom(type))
                {
                    RegisterComponentType(type);
                }
            }
        }
    }

    public void Start()
    {
        
    }

    public void Stop()
    {
        
    }

    /// <summary>
    /// 组件顺序标记器
    /// </summary>
    public Dictionary<int, Type> ComponentTypeMapper { get; set; } = new();

    public void RegisterComponentType(Type type)
    {
        var code = GetCodeByType(type);
        ComponentTypeMapper.Add(code, type);
    }

    public Type GetTypeByCode(int code)
    {
        return ComponentTypeMapper[code];
    }

    public int GetCodeByType(Type type)
    {
        // 根据类型计算唯一标识
        var typeName = type.FullName;
        if (typeName != null)
        {
            var code = (int)Compute(typeName);
            return code;
        }
        
        throw new Exception("无法计算类型的唯一标识");
    }
    
    /// <summary>
    /// 根据字符串计算唯一标识
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static uint Compute(string input)
    {
        byte[] data = Encoding.UTF8.GetBytes(input);
        int length = data.Length;
        uint m = 0x5bd1e995;
        int r = 24;
        uint h = 0 ^ (uint)length;

        int dataIndex = 0;

        while (length >= 4)
        {
            uint k = BitConverter.ToUInt32(data, dataIndex);
            k *= m;
            k ^= k >> r;
            k *= m;

            h *= m;
            h ^= k;

            length -= 4;
            dataIndex += 4;
        }

        switch (length)
        {
            case 3:
                h ^= (ushort)(data[dataIndex + 2] << 16);
                goto case 2;
            case 2:
                h ^= (ushort)(data[dataIndex + 1] << 8);
                goto case 1;
            case 1:
                h ^= data[dataIndex];
                h *= m;
                break;
            default:
                break;
        }

        h ^= h >> 13;
        h *= m;
        h ^= h >> 15;

        return h;
    }

    public ComponentArrayDataPack Serialize<T>(List<T> components) where T : IComponent
    {
        ComponentArrayDataPack dataPack = new();
        dataPack.TypeCode = GetCodeByType(typeof(T));
        
        var componentPacks = new List<ComponentDataPack<T>>();
        for (var index = 0; index < components.Count; index++)
        {
            var component = components[index];
            var componentPack = new ComponentDataPack<T>();
            componentPack.Id = component.Id;
            componentPack.Component = component;
            componentPacks.Add(componentPack);
        }

        dataPack.Data = ServerContainer.Get<IDataSerializer>().Serialize(componentPacks);

        return dataPack;
    }
    
    public List<T> Deserialize<T>(ComponentArrayDataPack dataPack) where T : IComponent
    {
        var components = new List<T>();

        var componentPacks = ServerContainer.Get<IDataSerializer>()
            .Deserialize<List<ComponentDataPack<T>>>(dataPack.Data, 0, dataPack.Data.Length);
        foreach (var componentPack in componentPacks)
        {
            components.Add(componentPack.Component);
        }
        
        return components;
    }
}