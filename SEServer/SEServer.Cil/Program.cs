using SEServer.Cil;
using SEServer.Core;
using SEServer.Data;
using SEServer.Game;

var systemProvider = new SystemProvider();
systemProvider.AddSystem(new MoveSystem());

var serverInstance = new ServerInstance();
serverInstance.ServerContainer.Add<ILogger>(new SimpleLogger());
serverInstance.ServerContainer.Add<IComponentSerializer>(new ComponentSerializer());
serverInstance.ServerContainer.Add<IServerNetworkService>(new ServerNetworkService());
serverInstance.ServerContainer.Add<IDataSerializer>(new SimpleSerializer());
serverInstance.ServerContainer.Add<ISystemProvider>(systemProvider);
serverInstance.StartGame();