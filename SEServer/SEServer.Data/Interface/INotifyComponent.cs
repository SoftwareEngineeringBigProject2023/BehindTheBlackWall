using System.Collections.Generic;
using SEServer.Data.Message;

namespace SEServer.Data.Interface;

/// <summary>
/// 通知组件
/// 负责从服务器收集信息并发送到客户端
/// </summary>
public interface INotifyComponent : INetComponent
{
    public Queue<NotifyData> NotifyMessages { get; set; }
}