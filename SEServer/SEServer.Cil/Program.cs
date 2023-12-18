using SEServer.Cil;
using SEServer.Core;
using SEServer.Data;
using SEServer.Data.Interface;
using SEServer.Game;
using SEServer.Game.System;
using SEServer.GameData.CTData;
using SEServer.GameData.Data;


var serverInstance = new ServerInstance();

serverInstance.ServerContainer.Add<ILogger>(new SimpleLogger());
serverInstance.ServerContainer.Add<IComponentSerializer>(new ComponentSerializer());
serverInstance.ServerContainer.Add<IServerNetworkService>(new HttpServerNetworkService());
serverInstance.ServerContainer.Add<IDataSerializer>(new SimpleSerializer());
serverInstance.ServerContainer.Add<IServerStatistics>(new ServerStatistics());

var netConfig = new NetConfig();
netConfig.ListenUrls = new string[]
{
    //"http://127.0.0.1:33700/Game/",
    "http://*:33700/Game/",
};

serverInstance.ServerContainer.Add<INetConfig>(netConfig);

var systemProvider = new SystemProvider();
systemProvider.AddSystem(new PlayerSystem());
systemProvider.AddSystem(new PhysicsSystem());
systemProvider.AddSystem(new UnitShootSystem());
systemProvider.AddSystem(new UnitBagSystem());
systemProvider.AddSystem(new UnitMoveSystem());
systemProvider.AddSystem(new UnitViewSystem());
systemProvider.AddSystem(new BulletSystem());
systemProvider.AddSystem(new MapSystem());
systemProvider.AddSystem(new ScoreSystem());
systemProvider.AddSystem(new PickDataSystem());
serverInstance.ServerContainer.Add<ISystemProvider>(systemProvider);

var config = new ServerWorldConfig();
config.FramePerSecond = 30;
config.DefaultSettings.Add("MapId", "SimpleMap");
serverInstance.ServerContainer.Add<IWorldConfig>(config);

string LoadConfig(string path)
{
    var json = File.ReadAllText(path);
    return json;
}

string[] LoadAllConfigs(string path)
{
    var files = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
    var jsons = new List<string>();
    foreach (var file in files)
    {
        var json = File.ReadAllText(file);
        jsons.Add(json);
    }

    return jsons.ToArray();
}

// 配置表
var configTable = new ConfigTable(LoadConfig, LoadAllConfigs);
configTable.ConfigTableNames.Add(new ConfigTableName("Maps", typeof(GameMapCTData), true));
configTable.ConfigTableNames.Add(new ConfigTableName("CTData/BulletCTData.json", typeof(BulletCTData)));
configTable.ConfigTableNames.Add(new ConfigTableName("CTData/WeaponCTData.json", typeof(WeaponCTData)));
serverInstance.ServerContainer.Add<IConfigTable>(configTable);

serverInstance.StartGame();