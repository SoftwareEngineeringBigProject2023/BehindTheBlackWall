using SEServer.Data.Interface;

namespace SEServer.Core;

public interface INetConfig : IService
{
    string[] ListenUrls { get; set; }
}