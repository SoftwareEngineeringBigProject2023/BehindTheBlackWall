namespace SEServer.Data.Interface;

public interface IService
{
    ServerContainer ServerContainer { get; set; }

    void Init();
    
    void Start();
    
    void Stop();
}