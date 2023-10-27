using System.Collections.Generic;
using SEServer.Data.Message;

namespace SEServer.Data.Interface;

/// <summary>
/// 提交组件
/// 负责从客户端收集信息并发送到服务器
/// </summary>
public interface ISubmitComponent : INetComponent
{
    PlayerId Owner { get; set; }
    Queue<SubmitData> SubmitMessages { get; set; }
}