using System.Collections.Generic;
using SEServer.Core;

namespace SEServer.Data;

public interface IServerNetworkService : IService
{
    List<ClientConnect> ClientConnects { get; }
    void RemoveUnconnectedClient();
}