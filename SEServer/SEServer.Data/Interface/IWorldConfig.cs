namespace SEServer.Data.Interface;

public interface IWorldConfig : IService
{
    public int FramePerSecond { get; set; }
}