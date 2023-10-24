namespace SEServer.Data;

public interface ISystem
{
    World World { get; set; }
    void Update();
}