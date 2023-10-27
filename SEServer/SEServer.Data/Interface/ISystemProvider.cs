using System.Collections.Generic;

namespace SEServer.Data.Interface;

public interface ISystemProvider : IService
{
    /// <summary>
    /// 添加系统
    /// </summary>
    /// <param name="system"></param>
    void AddSystem(ISystem system);
    /// <summary>
    /// 获取所有系统，已经排序完毕
    /// </summary>
    /// <returns></returns>
    List<ISystem> GetAllSystems();
}