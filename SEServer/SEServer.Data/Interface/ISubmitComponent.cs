using System.Collections.Generic;

namespace SEServer.Data;

/// <summary>
/// 提交组件
/// 负责从客户端收集信息并发送到服务器
/// </summary>
public interface ISubmitComponent : INetComponent
{
    PlayerId Owner { get; set; }
    Queue<ISubmitMessage> SubmitMessages { get; set; }
}