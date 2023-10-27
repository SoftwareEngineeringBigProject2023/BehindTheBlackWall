using SEServer.Data.Interface;

namespace SEServer.Core;

public interface IServerNetworkService : IService
{
    List<ClientConnect> ClientConnects { get; }
    void RemoveUnconnectedClient();
}