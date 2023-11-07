using SEServer.Data;

namespace SEServer.Core;

public class ServerStatistics : IServerStatistics
{
    public ServerContainer ServerContainer { get; set; }
    public void Init()
    {
        
    }

    public void Start()
    {
        
    }

    public void Stop()
    {
        
    }

    private int _uploadBandwidth;
    
    public void AddUploadBandwidth(int bytes)
    {
        // 多线程下的原子操作
        Interlocked.Add(ref _uploadBandwidth, bytes);
    }

    public int GetUploadBandwidthAndReset()
    {
        var bandwidth = _uploadBandwidth;
        Interlocked.Exchange(ref _uploadBandwidth, 0);
        return bandwidth;
    }
}