namespace SEServer.Data;

public interface IService
{
    ServerContainer ServerContainer { get; set; }

    void Init();
    
    void Start();
    
    void Stop();
}