using SEServer.Data;

namespace SEServer.Core;

public class User
{
    public UserId Id { get; set; }
    public ClientConnect? ClientConnect { get; set; }
    public WId BindWorld { get; set; } = WId.Invalid;
    public PlayerId BindPlayer { get; set; }
    /// <summary>
    /// 新客户端连接标记 - 用于重新发送快照信息
    /// </summary>
    public bool NewClientConnect { get; set; }
}