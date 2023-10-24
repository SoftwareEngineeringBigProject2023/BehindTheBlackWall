using SEServer.Cil;
using SEServer.Core;
using SEServer.Data;

var serverInstance = new ServerInstance();
serverInstance.ServerContainer.Add<ILogger>(new SimpleLogger());
serverInstance.ServerContainer.Add<IComponentSerializer>(new ComponentSerializer());
serverInstance.ServerContainer.Add<IServerNetworkService>(new ServerNetworkService());
serverInstance.ServerContainer.Add<IDataSerializer>(new SimpleSerializer());
serverInstance.StartGame();