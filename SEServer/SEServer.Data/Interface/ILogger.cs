namespace SEServer.Data.Interface;

/// <summary>
/// 日志接口
/// </summary>
public interface ILogger : IService
{
    void LogInfo(object msg);
    void LogWarning(object msg);
    void LogError(object msg);
}