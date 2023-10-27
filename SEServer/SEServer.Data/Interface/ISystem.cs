namespace SEServer.Data.Interface;

public interface ISystem
{
    World World { get; set; }
    void Init();
    void Update();
}