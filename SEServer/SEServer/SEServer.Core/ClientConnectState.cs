namespace SEServer.Core;

public enum ClientConnectState
{
    Disconnected,     // 未连接状态
    Connected,        // 连接已建立，但未获得授权
    Authorized,       // 客户端已获得授权
}