namespace SEServer.Core;

public enum ClientConnectState
{
    Disconnected = 0,     // 未连接状态
    Connected = 1,        // 连接已建立，但未获得授权
    Authorized = 2,       // 客户端已获得授权
}