using SEServer.Data;
using SEServer.Data.Interface;

namespace SEServer.Core;

public interface IServerStatistics : IService
{
    /// <summary>
    /// 添加带宽统计
    /// </summary>
    /// <param name="bytes"></param>
    public void AddUploadBandwidth(int bytes);
    
    /// <summary>
    /// 统计带宽并重置
    /// </summary>
    /// <returns></returns>
    public int GetUploadBandwidthAndReset();
}