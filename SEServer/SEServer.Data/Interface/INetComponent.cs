namespace SEServer.Data;

/// <summary>
/// 会被网络同步的组件
/// </summary>
public interface INetComponent : IComponent
{
    bool IsDirty { get; set; }
}