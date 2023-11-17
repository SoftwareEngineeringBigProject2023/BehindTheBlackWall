using SEServer.Cil;
using SEServer.Core;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.Game;
using SEServer.Game.System;


var serverInstance = new ServerInstance();

serverInstance.ServerContainer.Add<ILogger>(new SimpleLogger());
serverInstance.ServerContainer.Add<IComponentSerializer>(new ComponentSerializer());
serverInstance.ServerContainer.Add<IServerNetworkService>(new ServerNetworkService());
serverInstance.ServerContainer.Add<IDataSerializer>(new SimpleSerializer());
serverInstance.ServerContainer.Add<IServerStatistics>(new ServerStatistics());

var systemProvider = new SystemProvider();
systemProvider.AddSystem(new UnitMoveSystem());
systemProvider.AddSystem(new PlayerSystem());
systemProvider.AddSystem(new PhysicsSystem());
systemProvider.AddSystem(new ShootSystem());
systemProvider.AddSystem(new BulletSystem());
serverInstance.ServerContainer.Add<ISystemProvider>(systemProvider);

var config = new ServerWorldConfig();
config.FramePerSecond = 30;
serverInstance.ServerContainer.Add<IWorldConfig>(config);

serverInstance.StartGame();