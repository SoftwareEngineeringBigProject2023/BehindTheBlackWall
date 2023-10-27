using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SEServer.Data;

public class ServerContainer
{
    public Dictionary<Type, object> Services { get; } = new Dictionary<Type, object>();

    public void Add<T>(object service) where T : IService
    { 
        if (!(service is T tService))
        {
            throw new Exception($"服务 {service.GetType()} 类型不为 {typeof(T)}");
        }
        tService.ServerContainer = this;
        if (Services.ContainsKey(typeof(T)))
        {
            Get<ILogger>().LogWarning($"服务 {typeof(T)} 重复注册");
        }
        Services.Add(typeof(T), service);
    }
    
    public T Get<T>() where T : IService
    {
        if (Services.TryGetValue(typeof(T), out var service))
        {
            return (T) service;
        }
        throw new Exception($"服务 {typeof(T)} 未注册");
    }
    
    public void Remove<T>() where T : IService
    {
        Services.Remove(typeof(T));
    }

    public void Init()
    {
        foreach (var pair in Services)
        {
            var service = pair.Value as IService;
            service?.Init();
        }
    }

    public void Start()
    {
        foreach (var pair in Services)
        {
            var service = pair.Value as IService;
            service?.Start();
        }
    }
}