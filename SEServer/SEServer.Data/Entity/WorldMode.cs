namespace SEServer.Data;

/// <summary>
/// 世界的模式
/// </summary>
public enum WorldMode
{
    /// <summary>
    /// 服务器模式，创建、销毁和更新所有组件与实体
    /// </summary>
    Server,
    /// <summary>
    /// 客户端模式，只更新客户端组件与实体
    /// 根据服务器信息创建和销毁实体
    /// </summary>
    Client
}