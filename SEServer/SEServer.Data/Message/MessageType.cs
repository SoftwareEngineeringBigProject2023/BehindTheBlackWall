namespace SEServer.Data.Message;

public enum MessageType
{
    /// <summary>
    /// 心跳包
    /// </summary>
    Heartbeat,
    /// <summary>
    /// 目前的状态信息快照
    /// </summary>
    Snapshot,
    /// <summary>
    /// 同步实体信息
    /// </summary>
    SyncEntity,
}