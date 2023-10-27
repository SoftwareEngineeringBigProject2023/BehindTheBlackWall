namespace SEServer.Data;

/// <summary>
/// 由客户端发送到服务器的组件
/// </summary>
public interface IC2SComponent : INetComponent
{
    PlayerId Owner { get; set; }
}