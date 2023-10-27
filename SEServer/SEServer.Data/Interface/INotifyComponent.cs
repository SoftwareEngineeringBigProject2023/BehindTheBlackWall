using System.Collections.Generic;

namespace SEServer.Data;

/// <summary>
/// 通知组件
/// 负责从服务器收集信息并发送到客户端
/// </summary>
public interface INotifyComponent : INetComponent
{
    public Queue<NotifyMessage> NotifyMessages { get; set; }
}