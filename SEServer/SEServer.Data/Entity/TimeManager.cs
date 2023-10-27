namespace SEServer.Data;

public class TimeManager
{
    public double TotalTime { get; private set; }
    public float DeltaTime { get; private set; }
    public long FrameCount { get; private set; } = 0L;
    
    public void Update(float deltaTime)
    {
        TotalTime += deltaTime;
        DeltaTime = deltaTime;
        FrameCount++;
    }
}