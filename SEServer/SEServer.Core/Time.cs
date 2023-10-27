using System.Runtime.InteropServices;

namespace SEServer.Core;

public class Time
{
    [DllImport("winmm.dll")] 
    internal static extern uint timeBeginPeriod(uint period);

    [DllImport("winmm.dll")]
    internal static extern uint timeEndPeriod(uint period);

    /// <summary>
    /// 游戏启动的时间，单位毫秒
    /// </summary>
    private long _startTime;

    /// <summary>
    /// 上一帧开始时间，单位毫秒
    /// </summary>
    private long _lastFrameStartTime;
    
    /// <summary>
    /// 当前帧开始时间，单位毫秒
    /// </summary>
    private long _curFrameStartTime;

    private long _totalPassTimeOneSecond;
    private int _totalFpsThisSecond;
    private int _totalFpsLastSecond;

    private long _totalSleepTimeThisSecond;
    private long _totalSleepTimeLastSecond;
    
    public int MaxFps { get; set; } = 30;
    public int Fps => _totalFpsLastSecond;
    public float FrameInterval => 1f / MaxFps;

    /// <summary>
    /// 从帧开始已经过去的时间
    /// </summary>
    public float DeltaTime => (GetCurTime() - _lastFrameStartTime) /1000f;

    public int CurFrame { get; set; } = 0;
    
    // 当前负载，根据休眠时间与帧总时间的倒数计算，休眠时间月底负载越高
    public float LoadPercentage => 1 - Math.Clamp(_totalSleepTimeLastSecond / 1000f, 0f, 1f);
    public float TotalSleepTimeLastSecond => _totalSleepTimeLastSecond;

    public void Init()
    {
        _startTime = GetCurTime();
        _lastFrameStartTime = _startTime;
        _curFrameStartTime = _startTime;
    }

    public void StartFrame()
    {
        var curTime = GetCurTime();
        _totalPassTimeOneSecond += curTime - _curFrameStartTime;
        _lastFrameStartTime = _curFrameStartTime;
        _curFrameStartTime = curTime;

        if (_totalPassTimeOneSecond > 1000)
        {
            _totalPassTimeOneSecond = 0;
            _totalFpsLastSecond = _totalFpsThisSecond;
            _totalFpsThisSecond = 0;
            
            _totalSleepTimeLastSecond = _totalSleepTimeThisSecond;
            _totalSleepTimeThisSecond = 0;
        }

        _totalFpsThisSecond += 1;
    }

    public void EndFrame()
    {
        CurFrame += 1;
        long sleepTime = (long) (FrameInterval * 1000 - (GetCurTime() - _curFrameStartTime));
        _totalSleepTimeThisSecond += sleepTime;
        if (sleepTime > 0)
        {
            timeBeginPeriod(1);
            Thread.Sleep((int)sleepTime);
            timeEndPeriod(1);
        }
    }

    private long GetCurTime() => DateTime.Now.Ticks / 10000L;
}